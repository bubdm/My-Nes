// This file is part of My Nes
//
// A Nintendo Entertainment System / Family Computer (Nes/Famicom)
// Emulator written in C#.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
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
// Author email: mailto:alaahadidfreeware@gmail.com
//
using System;
using MyNes.Core;
using System.Runtime.InteropServices;
using SDL2;

namespace MyNes
{
    unsafe class SDl2AudioProvider : IAudioProvider
    {
        private byte[] audio_samples;
        private int samples_count;
        internal int samples_added;
        private ushort buffer_size;
        private bool pitch;
        private int buffer_size_6;
        private int buffer_min;
        private int buffer_limit;
        private long w_pos;
        private long r_pos;
        private int sample;
        private bool is_rendering;

        private bool ready;
        private bool enabled;
        private double fps_nes_normal;
        private double fps_nes_missle;
        private double fps_pl_faster;
        private int fps_mode;
        private double volume = 0;
        internal SDL.SDL_AudioSpec specs;

        private uint audio_device_index;

        internal bool IsPlaying;
        public string Name
        { get { return "SDL2 Audio"; } }
        public string ID
        { get { return "sdl2.audio"; } }
        public bool AllowBufferChange
        { get { return false; } }
        public bool AllowFrequencyChange
        { get { return true; } }

        public void GetIsPlaying(out bool playing)
        {
            playing = IsPlaying;
        }
        public void Initialize()
        {
            ready = false;
            enabled = MyNesMain.RendererSettings.Audio_SoundEnabled;
            buffer_size = (ushort)(MyNesMain.RendererSettings.Audio_PlaybackBufferSizeInKB * 1024);
            buffer_min = buffer_size - (1024 * 2);
            buffer_limit = buffer_size + (1024 * 8);
            Console.WriteLine("SDL: Initializing audio ...");

            SDL2Settings sdl_settings = new SDL2Settings(System.IO.Path.Combine(Program.WorkingFolder, "sdlsettings.ini"));
            sdl_settings.LoadSettings();

            SDL.SDL_Init(SDL.SDL_INIT_AUDIO);
            int c = SDL.SDL_GetNumAudioDrivers();
            for (int i = 0; i < c; i++)
            {
                string n = SDL.SDL_GetAudioDeviceName(i, 0);
                Console.WriteLine(n);
            }

            // Open first device
            string audio_device = SDL.SDL_GetAudioDeviceName(sdl_settings.Audio_Device_Index, 0);
            Console.WriteLine("Device = " + audio_device);

            specs = new SDL.SDL_AudioSpec();
            SDL.SDL_AudioSpec specs1 = new SDL.SDL_AudioSpec();// dummy

            specs.channels = 1;
            specs.format = SDL.AUDIO_S16;
            specs.freq = MyNesMain.RendererSettings.Audio_Frequency;

            specs.samples = buffer_size;
            specs.callback = AudioCallback;

            samples_count = buffer_size * 20;
            audio_samples = new byte[samples_count];

            //SDL.SDL_OpenAudio(ref specs, out specs1);
            audio_device_index = SDL.SDL_OpenAudioDevice(audio_device, 0, ref specs, out specs1, (int)SDL.SDL_AUDIO_ALLOW_FREQUENCY_CHANGE);
            if (audio_device_index == 0)
            {
                Console.WriteLine("ERROR INITAILIZING AUDIO DEVICE");
                MyNesMain.VideoProvider.WriteErrorNotification("ERROR INITAILIZING AUDIO DEVICE, please configure SDL2 audio settings.", false);
            }
            w_pos = 0;
            r_pos = w_pos + buffer_size;

            IsPlaying = false;

            SDL.SDL_PauseAudioDevice(audio_device_index, 1);

            FixSpeed();

            ready = true;
            samples_added = 0;

            SetVolume(MyNesMain.RendererSettings.Audio_Volume);
            Console.WriteLine("SDL: Audio initialized success.");
        }
        public void Reset()
        {
            ShutDown();
            Initialize();
        }
        public void SetVolume(int Vol)
        {
            if (Vol > 0)
                volume = (double)Vol / 100.0;
            else
                volume = 0;
        }
        public void ShutDown()
        {
            SDL.SDL_CloseAudio();
        }
        private void FixSpeed()
        {
            fps_mode = 0;
            double target_fps = 0;
            NesEmu.GetTargetFPS(out target_fps);

            fps_nes_missle = 1.0 / (target_fps + 20);
            fps_pl_faster = 1.0 / (target_fps - 1);
            fps_nes_normal = 1.0 / target_fps;
        }
        public void SignalToggle(bool started)
        {
            fps_mode = 0;
            FixSpeed();

            if (started)
            {
                // Noise on shutdown; MISC
                Random r = new Random();
                for (int i = 0; i < audio_samples.Length; i++)
                    audio_samples[i] = (byte)r.Next(0, 20);

                for (int i = 0; i < audio_samples.Length; i++)
                    audio_samples[i] = (byte)r.Next(0, 20);
            }
            else
            {
                TogglePause(true);
            }
        }
        public void SubmitSamples(ref short[] samples, ref int samples_a)
        {
            if (!enabled)
                return;
            if (!ready)
                return;
            // if (is_rendering)
            //     return;
            // Nes emu call this method at the end of each frame to fill the sound buffer...
            // Code should still work if this method is not called
            for (int i = 0; i < samples_a; i++)
            {
                sample = (int)(samples[i] * volume);

                if (!is_rendering)
                {
                    audio_samples[w_pos++ % samples_count] = (byte)(sample >> 8);
                    audio_samples[w_pos++ % samples_count] = (byte)(sample & 0xFF);

                    samples_added++;
                }
            }
        }
        public void TogglePause(bool paused)
        {
            if (!enabled)
                return;
            if (paused)
                Pause();
            else
                Play();
        }
        internal void Play()
        {
            if (!enabled)
                return;
            if (!IsPlaying)
            {
                IsPlaying = true;
                SDL.SDL_PauseAudioDevice(audio_device_index, 0);

                FixSpeed();
            }
        }
        internal void Pause()
        {
            if (!enabled)
                return;
            if (IsPlaying)
            {
                IsPlaying = false;
                SDL.SDL_PauseAudioDevice(audio_device_index, 1);
            }
        }

        private void AudioCallback(IntPtr userdata, IntPtr stream, int len)
        {
            if (!enabled)
                return;
            if (!ready)
                return;
            is_rendering = true;

            // Write it to the data
            for (int i = 0; i < len; i++)
            {
                ((byte*)stream)[i] = audio_samples[r_pos++ % samples_count];
            }

            samples_added -= len / 2;


            // SDL.SDL_AudioSpec specs = (SDL.SDL_AudioSpec)Marshal.PtrToStructure(userdata, typeof(SDL.SDL_AudioSpec));

            // SPEED CONTROL !!
            if (NesEmu.FrameLimiterEnabled && NesEmu.ON)
            {
                if (samples_added <= buffer_min)
                {
                    if (fps_mode != 2)
                    {
                        fps_mode = 2;
                        // PL is faster than nes, make nes faster
                        NesEmu.SetFramePeriod(ref fps_nes_missle);
                        // Console.WriteLine("SDL: sound switched to OVERCLOCKED NES speed mode.");
                    }
                }
                else if (samples_added >= buffer_limit)
                {
                    if (fps_mode != 1)
                    {
                        fps_mode = 1;
                        // nes is faster than PL, make PL faster
                        NesEmu.SetFramePeriod(ref fps_pl_faster);
                        // Console.WriteLine("SDL: sound switched to PLAYER FASTER speed mode.");
                    }
                }
                else
                {
                    if (fps_mode != 0)
                    {
                        fps_mode = 0;
                        // between 1000 and 2000, set to normal speed
                        NesEmu.SetFramePeriod(ref fps_nes_normal);
                        //  Console.WriteLine("SDL: sound switched to normal speed mode.");
                    }
                }
            }
            else
            {
                samples_added = 0;
                w_pos = 0;
                r_pos = w_pos + buffer_size;
            }

            is_rendering = false;
        }
    }
}
