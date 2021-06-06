// This file is part of My Nes
//
// A Nintendo Entertainment System / Family Computer (Nes/Famicom)
// Emulator written in C#.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2018
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// 
// Author email: mailto:ahdsoftwares@hotmail.com
//
using MyNes.Core;
using SlimDX.DirectSound;
using SlimDX.Multimedia;
using System;

namespace MyNes
{
    class SlimDXAudioProvider : IAudioProvider
    {
        public string Name { get { return "SlimDX (DirectSound)"; } }
        public string ID { get { return "slimdx.directsound"; } }
        public bool AllowBufferChange { get { return true; } }
        public bool AllowFrequencyChange { get { return true; } }

        // slimdx
        public DirectSound _SoundDevice;
        public SecondarySoundBuffer buffer;

        private double temp;
        private bool IsPaused;
        public bool IsRendering = false;
        public int BufferSize = 44100;
        private bool isInitialized;
        private int[] buffer_internal;
        private int buffer_internal_size;
        private int buffer_internal_r_pos;
        private int buffer_internal_w_pos;
        private int fps_mode;
        private static double fps_nes_normal;
        private static double fps_nes_missle;
        private static double fps_pl_faster;

        // Don`t ask what are those and how i got them :D
        public int samples_period_max = 5156;//2291/2538/2777/5156
        public int samples_period_min = 2703;//1572/1469/1554/2703

        private byte[] buffer_playback;
        private int buffer_playback_size;
        private int buffer_playback_current_pos;
        private int buffer_playback_last_pos;
        private int buffer_playback_w_pos;
        private int buffer_playback_required_samples;
        private int buffer_playback_i;
        private int buffer_playback_currentSample;
        private int buffer_freq;
        private double target_fps;

        public System.Collections.Generic.List<int> vals = new System.Collections.Generic.List<int>();

        private int samples_added;

        public int PEEK_LIMIT = 124;

        public void GetIsPlaying(out bool playing)
        {
            playing = !IsPaused;
        }
        public void Initialize()
        {
            if (isInitialized)
            {
                Dispose();
            }
            isInitialized = false;

            //  LoadSettings();
            //Create the device
            Tracer.WriteLine("DirectSound: Initializing directSound ...");
            _SoundDevice = new DirectSound();
            _SoundDevice.SetCooperativeLevel(Program.FormMain.Handle, CooperativeLevel.Normal);

            //Create the wav format
            WaveFormat wav = new WaveFormat();
            wav.FormatTag = WaveFormatTag.Pcm;
            wav.SamplesPerSecond = MyNesMain.RendererSettings.Audio_Frequency;
            wav.Channels = 1;
            wav.BitsPerSample = 16;
            wav.AverageBytesPerSecond = wav.SamplesPerSecond * wav.Channels * (wav.BitsPerSample / 8);
            wav.BlockAlignment = (short)(wav.Channels * wav.BitsPerSample / 8);

            BufferSize = MyNesMain.RendererSettings.Audio_PlaybackBufferSizeInKB * 1024 * 2;
            buffer_internal_r_pos = 0;
            buffer_internal_w_pos = 0;
            samples_added = 0;
            buffer_playback_current_pos = 0;
            buffer_playback_last_pos = 0;
            buffer_playback_w_pos = 0;
            buffer_playback_required_samples = 0;
            buffer_playback_i = 0;
            buffer_playback_currentSample = 0;

            //samples_period_max = BufferSize - (1024 * 2);

            samples_period_min = (1024 * 2);
            samples_period_max = samples_period_min * 12;

            Tracer.WriteLine("DirectSound: BufferSize = " + BufferSize + " Byte");
            //Description
            SoundBufferDescription des = new SoundBufferDescription();
            des.Format = wav;
            des.SizeInBytes = BufferSize;
            des.Flags = BufferFlags.ControlVolume | BufferFlags.ControlFrequency | BufferFlags.ControlPan | BufferFlags.Software | BufferFlags.ControlPositionNotify;

            buffer = new SecondarySoundBuffer(_SoundDevice, des);
            // Set volume
            ApplySettings();
            Tracer.WriteLine("DirectSound: DirectSound initialized OK.");
            isInitialized = true;

            ShutDown();
        }
        public void ApplySettings()
        {
            SetVolume(MyNesMain.RendererSettings.Audio_Volume);
        }
        public void Dispose()
        {
            if (isInitialized)
                if (!buffer.Disposed & !IsRendering)
                {
                    buffer.Stop();
                    IsPaused = true;
                }

            isInitialized = false;

            buffer.Dispose(); buffer = null;
            _SoundDevice.Dispose(); _SoundDevice = null;

            GC.Collect();
        }
        public void ShutDown()
        {
            // Stop

            //  if (Recorder.IsRecording)
            //      Recorder.Stop();
            // Set buffers
            buffer_internal_size = (BufferSize / 2);
            buffer_internal = new int[buffer_internal_size];
            buffer_internal_r_pos = 0;
            buffer_internal_w_pos = 0;
            samples_added = 0;
            buffer_playback_size = BufferSize;
            buffer_playback = new byte[buffer_playback_size];
            buffer_playback_current_pos = 0;
            buffer_playback_last_pos = 0;
            buffer_playback_required_samples = 0;
            buffer_playback_i = 0;
            buffer_playback_currentSample = 0;
            // Noise on shutdown; MISC
            Random r = new Random();
            for (int i = 0; i < buffer_internal.Length; i++)
                buffer_internal[i] = (byte)r.Next(0, 20);

            for (int i = 0; i < buffer_playback.Length; i++)
                buffer_playback[i] = (byte)r.Next(0, 20);
        }
        public void SetVolume(int Vol)
        {
            if (buffer != null && !buffer.Disposed & !IsRendering)
            {
                int volLev = (Vol * 3000) / 100;
                buffer.Volume = -3000 + volLev;
            }
        }
        public void SubmitSamples(ref short[] nesBuffer, ref int samplesAdded)
        {
            if (!isInitialized)
                return;
            IsRendering = true;
            // Get the playback buffer needed samples
            buffer_playback_current_pos = buffer.CurrentWritePosition;
            buffer_playback_w_pos = buffer_playback_last_pos;

            buffer_playback_required_samples = buffer_playback_current_pos - buffer_playback_last_pos;
            if (buffer_playback_required_samples < 0)
                buffer_playback_required_samples = (buffer_playback_size - buffer_playback_last_pos) + buffer_playback_current_pos;

            // fill up the internal buffer using the nes buffer
            for (int i = 0; i < samplesAdded; i++)
            {
                buffer_playback_currentSample = nesBuffer[i];
                /*
                // Limit peek level
                if (buffer_playback_currentSample > PEEK_LIMIT)
                    buffer_playback_currentSample = PEEK_LIMIT;
                if (buffer_playback_currentSample < -PEEK_LIMIT)
                    buffer_playback_currentSample = -PEEK_LIMIT;*/

                if (buffer_internal_w_pos >= buffer_internal_size)
                    buffer_internal_w_pos = 0;

                buffer_internal[buffer_internal_w_pos] = buffer_playback_currentSample;

                buffer_internal_w_pos++;
                if (buffer_internal_w_pos >= buffer_internal_size)
                    buffer_internal_w_pos = 0;

                //if (Recorder.IsRecording)
                //    Recorder.AddSample(ref buffer_playback_currentSample);

                samples_added++;
            }

            /*buffer_freq = samplesAdded * (int)target_fps;
            if (buffer.Frequency != buffer_freq)
                buffer.Frequency = buffer_freq;*/
            // Fill up the playback buffer
            // for (buffer_playback_i = 0; buffer_playback_i < buffer_playback_required_samples; buffer_playback_i += 2)
            for (buffer_playback_i = 0; buffer_playback_i < buffer_playback_required_samples; buffer_playback_i += 2)
            {
                // Get the sample from the internal buffer
                if (buffer_internal_r_pos >= buffer_internal_size || buffer_internal_r_pos < 0)
                    buffer_internal_r_pos = 0;
                buffer_playback_currentSample = buffer_internal[buffer_internal_r_pos];
                buffer_internal_r_pos++;
                if (buffer_internal_r_pos >= buffer_internal_size)
                    buffer_internal_r_pos = 0;

                // Put it in the playback buffer
                if (buffer_playback_w_pos >= buffer_playback_size)
                    buffer_playback_w_pos = 0;

                buffer_playback[buffer_playback_w_pos] = (byte)((buffer_playback_currentSample & 0xFF00) >> 8);
                buffer_playback_w_pos++;
                if (buffer_playback_w_pos >= buffer_playback_size)
                    buffer_playback_w_pos = 0;

                buffer_playback[buffer_playback_w_pos] = (byte)(buffer_playback_currentSample & 0xFF);
                buffer_playback_w_pos++;
                if (buffer_playback_w_pos >= buffer_playback_size)
                    buffer_playback_w_pos = 0;

                samples_added--;

            }
            //samples_added = 0;
            buffer.Write(buffer_playback, 0, LockFlags.EntireBuffer);
            buffer_playback_last_pos = buffer_playback_current_pos;

            /* if (samples_added < 0)
                 MyNesMain.VideoProvider.WriteInfoNotification("OVERFLOW (PL faster than NES) = " + samples_added + ", Mod = " + fps_mode);
             if (samples_added > 0)
                 MyNesMain.VideoProvider.WriteInfoNotification("OVERFLOW (NES faster than PL)= " + samples_added + ", Mod = " + fps_mode);*/


            // current_time = GetTime();
            // frame_time = current_time - last_time;
            //dead_time = NesEmu.ImmediateFrameTime - frame_time;

            // Program.FormMain.video.WriteNotification("SFT= " + (1.0 / frame_time).ToString("F2") + ", dead = " +  dead_time.ToString("F2"), 60, System.Drawing.Color.Lime);
            //  Program.FormMain.video.WriteNotification("sound= " + frame_time.ToString("F3") + "; nes = " + NesEmu.ImmediateFrameTime.ToString("F3") + "; diff = " +
            //      (NesEmu.ImmediateFrameTime - frame_time).ToString("F3")+"; samples = "+samples_added, 60, System.Drawing.Color.Lime);
            // SPEED CONTROL !!
            // Adjust the emu speed by changing the frame period.
            // samples_added determines how many samples that left after emu generates them and playback uses them.
            // samples_added < 0 means playback is faster than emu, it take samples more than emu can generate at frame
            // samples_added > 0 means emu is faster than playback, emu generates frames more than playback can play at frame
            // The only solution for sound synchronization is to make the samples count at a rate than no pointers-overflow happens.
            // 2000 <= samples_added <= 2500 is the best rate.
            // SPEED CONTROL !!
            if (NesEmu.FrameLimiterEnabled)
            {
                if (samples_added >= samples_period_max)
                {
                    if (fps_mode != 1)
                    {
                        fps_mode = 1;
                        // nes is faster than PL, make PL faster
                        NesEmu.SetFramePeriod(ref fps_pl_faster);
                        //Console.WriteLine("DirectSound: sound switched to PLAYER FASTER speed mode.");
                    }
                }
                else if (samples_added < samples_period_min)
                {
                    if (fps_mode != 2)
                    {
                        fps_mode = 2;
                        // nes is very slow, make it missle to at least get some samples.
                        NesEmu.SetFramePeriod(ref fps_nes_missle);
                        //Console.WriteLine("DirectSound: sound switched to NES MISSLE speed mode.");
                    }
                }
                else
                {
                    if (fps_mode != 0)
                    {
                        fps_mode = 0;
                        // between 1000 and 2000, set to normal speed
                        NesEmu.SetFramePeriod(ref fps_nes_normal);
                        //Console.WriteLine("DirectSound: sound switched to normal speed mode.");
                    }
                }
            }
            else
            {
                samples_added = 0;
                //  buffer_internal_w_pos = 0;
                // buffer_internal_r_pos = buffer_internal_w_pos + latency_in_samples;
            }
            IsRendering = false;
            //last_time = current_time;
        }
        public void TogglePause(bool paused)
        {
            NesEmu.GetTargetFPS(out target_fps);
            switch (NesEmu.Region)
            {
                case EmuRegion.NTSC:
                    {
                        fps_nes_missle = 1.0 / 80;
                        fps_pl_faster = 1.0 / 58;
                        fps_nes_normal = 1.0 / target_fps;
                        break;
                    }
                case EmuRegion.PALB:
                case EmuRegion.DENDY:
                    {
                        fps_nes_missle = 1.0 / 80;
                        fps_pl_faster = 1.0 / 49;
                        fps_nes_normal = 1.0 / target_fps;
                        break;
                    }
            }
            if (!paused)
            {
                IsPaused = false;
                if (isInitialized)
                    if (!buffer.Disposed & !IsRendering)
                        buffer.Play(0, PlayFlags.Looping);

                Tracer.WriteLine("DirectSound: Playing.");
            }
            else
            {
                if (isInitialized)
                    if (!buffer.Disposed & !IsRendering)
                    {
                        buffer.Stop();
                        IsPaused = true;
                    }
                Tracer.WriteLine("DirectSound: Stopped.");
            }
        }
        public void SignalToggle(bool started)
        {
            if (started)
            {
                // Noise on shutdown; MISC
                Random r = new Random();
                for (int i = 0; i < buffer_internal.Length; i++)
                    buffer_internal[i] = (byte)r.Next(0, 20);

                for (int i = 0; i < buffer_playback.Length; i++)
                    buffer_playback[i] = (byte)r.Next(0, 20);
            }
            else
            {
                TogglePause(true);
            }
        }

        public void Reset()
        {
            // 1 Dispose
            Dispose();
            // 2 Initialize
            Initialize();
        }
    }
}
