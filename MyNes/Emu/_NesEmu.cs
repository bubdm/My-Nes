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
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace MyNes.Core
{
    public partial class NesEmu
    {
        public static bool ON;
        public static bool PAUSED;
        public static bool isPaused;
        public static string CurrentFilePath;
        public static bool FrameLimiterEnabled;
        private static Thread mainThread;

        private static double fps_time_last;// the frame end time (time point)
        private static double fps_time_start;// the frame start time (time point)
        private static double fps_time_token;// how much time in seconds the rendering process toke
        private static double fps_time_dead;// how much time remains for the target frame rate. For example, if a frame toke 3 milliseconds to complete, we'll have 16-3=13 dead time for 60 fps.
        private static double fps_time_period;// how much time in seconds a frame should take for target fps. i.e. ~16 milliseconds for 60 fps.
        private static double fps_time_frame_time;// the actual frame time after rendering and speed limiting. This should be equal to fps_time_period for perfect emulation timing.

        private static double emu_time_target_fps = 60;
        private static bool emu_frame_clocking_mode;
        private static bool emu_frame_done;
        private static bool render_initialized;
        private static RenderVideoFrame render_video;
        private static RenderAudioSamples render_audio;
        private static TogglePause render_audio_toggle_pause;
        private static GetIsPlaying render_audio_get_is_playing;
        private static bool render_audio_is_playing;
        // Region stuff
        public static EmuRegion Region;
        internal static Action EmuClockComponents;
        internal static int pal_add_cycle = 0;
        private static int SystemIndex;
        private static RequestMode emu_request_mode = RequestMode.None;
        private enum RequestMode
        {
            None, HardReset, SoftReset, LoadState, SaveState, TakeSnapshot
        }
        // Frame skipping
        public static bool FrameSkipEnabled;
        public static int FrameSkipInterval;
        private static int FrameSkipCounter;

        public static event EventHandler EmuShutdown;

        /// <summary>
        /// Check a file and indicate if this file is a supported and loadable.
        /// </summary>
        /// <param name="fileName">The complete file path. Compressed files are not supported.</param>
        /// <param name="valid">Set to true if file is valid (supported and playable), otherwise set to false.</param>
        internal static void CheckGame(string fileName, out bool valid)
        {
            // Load the rom using header
            // TODO: support more file formats.
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".nes":
                    {
                        Tracer.WriteLine("Checking INES header ...");
                        INes ines = new INes();
                        ines.Load(fileName, false);
                        valid = ines.IsValid;
                        Tracer.WriteLine("INES header is valid.");
                        break;
                    }
                default:
                    {
                        Tracer.WriteWarning("File format is not supported. Format: " + Path.GetExtension(fileName));
                        valid = false;
                        break;
                    }
            }
        }
        internal static void Initialize()
        {
            Tracer.WriteLine("Loading database file ...");
            string filePath = Path.Combine(MyNesMain.AppPath, "database.xml");
            if (File.Exists(filePath))
            {
                bool success = false;
                NesCartDatabase.LoadDatabase(filePath, out success);
                if (success)
                    Tracer.WriteInformation("Nes Cart database file loaded successfully.");
                else
                    Tracer.WriteError("Error loading Nes Cart database file.");
            }
            else
            {
                Tracer.WriteWarning("Nes Cart database file cannot be located at " + filePath);
            }
            EmuClockComponents = EmuClockComponentsNTSC;

            FrameLimiterEnabled = true;
            CPUInitialize();
            PPUInitialize();
            APUInitialize();
            PORTSInitialize();
        }
        internal static void SetupRenderingMethods(RenderVideoFrame renderVideo, RenderAudioSamples renderAudio, TogglePause renderTogglePause, GetIsPlaying renderGetIsPlaying)
        {
            render_initialized = false;
            render_video = renderVideo;
            render_audio = renderAudio;
            render_audio_toggle_pause = renderTogglePause;
            render_audio_get_is_playing = renderGetIsPlaying;
            render_initialized = render_video != null && render_audio != null && render_audio_toggle_pause != null && render_audio_get_is_playing != null;
            if (render_initialized)
                Tracer.WriteInformation("Renderer methods initialized successfully.");
            else
            {
                Tracer.WriteError("ERROR RENDERER INITIALIZING !!");
                Tracer.WriteError("Faild to initialize the renderers methods. Please use the method 'SetupRenderingMethods' to initialize the renderers methods before you can run the emulation.");
            }
        }
        public static void LoadGame(string fileName, out bool success)
        {
            if (!render_initialized)
            {
                Tracer.WriteError("NO RENDERER INITIALIZED !! EMU CANNOT BE INTIALIZED WITHOUT A RENDERER !!");
                Tracer.WriteError("Please use the method 'SetupRenderingMethods' to initialize the renderers methods before you can run the emulation.");
                success = false;
                return;
            }
            // Load the rom using header
            // TODO: support more file formats.
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".nes":
                    {
                        Tracer.WriteLine("Checking INES header ...");
                        INes ines = new INes();
                        ines.Load(fileName, true);
                        if (ines.IsValid)
                        {
                            emu_request_mode = RequestMode.None;
                            CurrentFilePath = fileName;
                            if (ON)
                                ShutDown();
                            Tracer.WriteLine("INES header is valid, loading game ...");

                            // 1 Initialize components that needs to be initialized.
                            MEMInitialize(ines);

                            // Setup region ... this should be done using database
                            ApplyRegionSetting();


                            ApplyAudioSettings();
                            ApplyFrameSkipSettings();

                            // 3 Check ports connections
                            PORTSInitialize();
                            // 4 Hard reset all
                            hardReset();
                            // Ready!
                            Tracer.WriteLine("EMU is ready.");
                            success = true;

                            emu_frame_clocking_mode = !MyNesMain.RendererSettings.UseEmuThread;

                            ON = true;
                            PAUSED = false;
                            isPaused = false;

                            FrameLimiterEnabled = true;
                            if (MyNesMain.RendererSettings.UseEmuThread)
                            {
                                Tracer.WriteLine("Running in a thread ... using custom frame limiter.");
                                mainThread = new Thread(new ThreadStart(EmuClock));

                                mainThread.Start();
                            }
                            MyNesMain.VideoProvider.SignalToggle(true);
                            MyNesMain.AudioProvider.SignalToggle(true);
                        }
                        else
                        {
                            success = false;
                        }
                        break;
                    }
                default:
                    {
                        success = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Hard reset the emu
        /// </summary>
        public static void HardReset()
        {
            PAUSED = true;
            emu_request_mode = RequestMode.HardReset;
        }
        private static void hardReset()
        {
            if (MyNesMain.WaveRecorder.IsRecording)
                MyNesMain.WaveRecorder.Stop();
            render_audio_toggle_pause(true);
            switch (Region)
            {
                case EmuRegion.NTSC:
                    {
                        emu_time_target_fps = 60.0988;
                        break;
                    }
                case EmuRegion.PALB:
                case EmuRegion.DENDY:
                    {
                        emu_time_target_fps = 50;
                        break;
                    }
            }

            fps_time_period = 1.0 / emu_time_target_fps;

            MEMHardReset();
            CPUHardReset();
            PPUHardReset();
            APUHardReset();
            DMAHardReset();
            render_audio_toggle_pause(false);

            MyNesMain.VideoProvider.WriteWarningNotification(MNInterfaceLanguage.Message_HardReset, false);
        }
        public static void SoftReset()
        {
            PAUSED = true;
            emu_request_mode = RequestMode.SoftReset;
        }
        private static void softReset()
        {
            CPUSoftReset();
            APUSoftReset();
            MyNesMain.VideoProvider.WriteWarningNotification(MNInterfaceLanguage.Message_SoftReset, false);
        }
        public static void SaveState()
        {
            PAUSED = true;
            emu_request_mode = RequestMode.SaveState;
        }
        public static void LoadState()
        {
            PAUSED = true;
            emu_request_mode = RequestMode.LoadState;
        }
        internal static void TakeSnapshot()
        {
            PAUSED = true;
            emu_request_mode = RequestMode.TakeSnapshot;
        }
        public static void ShutDown()
        {
            MyNesMain.VideoProvider.SignalToggle(false);
            MyNesMain.AudioProvider.SignalToggle(false);
            if (MyNesMain.WaveRecorder.IsRecording)
                MyNesMain.WaveRecorder.Stop();
            render_audio_get_is_playing(out render_audio_is_playing);
            if (render_audio_is_playing)
                render_audio_toggle_pause(true);
            // Stop the thread
            Tracer.WriteLine("Shutting down the emulation core...");
            ON = false;
            // Wait ..
            if (mainThread != null)
            {
                Tracer.WriteLine("Aborting thread ..");
                while (mainThread.IsAlive)
                {
                }
                mainThread.Abort();
                mainThread = null;
            }

            // Save sram .. 
            SaveSRAM();
            Tracer.WriteInformation("Emulation core shutdown successfully.");

            EmuShutdown?.Invoke(null, new EventArgs());
        }
        public static void EMUClockFrame()
        {
            if (PAUSED)
            {
                render_audio_get_is_playing(out render_audio_is_playing);
                if (render_audio_is_playing)
                    render_audio_toggle_pause(true);

                shortucts.Update();
                // Do we have requests ?
                switch (emu_request_mode)
                {
                    case RequestMode.HardReset:
                        {
                            hardReset();
                            PAUSED = false;
                            emu_request_mode = RequestMode.None;
                            break;
                        }
                    case RequestMode.SoftReset:
                        {
                            softReset(); PAUSED = false;
                            emu_request_mode = RequestMode.None;
                            break;
                        }
                    case RequestMode.SaveState:
                        {
                            StateHandler.SaveState(); PAUSED = false;
                            emu_request_mode = RequestMode.None;
                            break;
                        }
                    case RequestMode.LoadState:
                        {
                            StateHandler.LoadState(); PAUSED = false;
                            emu_request_mode = RequestMode.None;
                            break;
                        }
                    case RequestMode.TakeSnapshot:
                        {
                            MyNesMain.VideoProvider.TakeSnapshot(); PAUSED = false;
                            emu_request_mode = RequestMode.None;
                            break;
                        }
                }
                isPaused = true;// Cleared in frame finish method

                return;
            }
            // Do render ...
            emu_frame_done = false;
            while (!ppu_frame_finished && ON)
            {
                CPUClock();
            }

            // Reached here means a frame finish !
            if (!FrameSkipEnabled)
                render_video(ref ppu_screen_pixels);
            else
            {
                FrameSkipCounter++;
                if (FrameSkipCounter >= FrameSkipInterval)
                {
                    render_video(ref ppu_screen_pixels);
                    FrameSkipCounter = 0;
                }
            }
            isPaused = false;
            ppu_frame_finished = false;
            emu_frame_done = true;

            joypad1.Update();
            joypad2.Update();
            if (IsFourPlayers)
            {
                joypad3.Update();
                joypad4.Update();
            }
            shortucts.Update();
            // Sound
            if (SoundEnabled)
            {
                render_audio_get_is_playing(out render_audio_is_playing);
                if (!render_audio_is_playing)
                    render_audio_toggle_pause(false);
                render_audio(ref audio_samples, ref audio_samples_added);

                audio_w_pos = 0;
                audio_samples_added = 0;
                audio_timer = 0;
            }
            fps_time_token = GetTime() - fps_time_start;

            fps_time_last = GetTime();
            fps_time_frame_time = fps_time_last - fps_time_start;

            // This point is the start of the next frame
            fps_time_start = GetTime();
        }
        public static void EMUClockSamples(int samples_required, out int audio_added_samples)
        {
            int last_added = audio_samples_added;
            // Do render ...
            while (samples_required > 0 && audio_w_pos < audio_samples_count)
            {
                CPUClock();
                if (audio_samples_added - last_added >= 1)
                {
                    samples_required--;

                }
                last_added = audio_samples_added;
            }
            audio_added_samples = audio_samples_added;
        }
        private static void EmuClock()
        {
            while (ON)
            {
                if (!PAUSED)
                {
                    CPUClock();

                    if (ppu_frame_finished)
                        FrameFinished();
                }
                else
                {
                    render_audio_get_is_playing(out render_audio_is_playing);
                    if (render_audio_is_playing)
                        render_audio_toggle_pause(true);
                    // Relieve the cpu a little bit.
                    Thread.Sleep(100);
                    shortucts.Update();
                    // Do we have requests ?
                    switch (emu_request_mode)
                    {
                        case RequestMode.HardReset:
                            {
                                hardReset();
                                PAUSED = false;
                                emu_request_mode = RequestMode.None;
                                break;
                            }
                        case RequestMode.SoftReset:
                            {
                                softReset(); PAUSED = false;
                                emu_request_mode = RequestMode.None;
                                break;
                            }
                        case RequestMode.SaveState:
                            {
                                StateHandler.SaveState(); PAUSED = false;
                                emu_request_mode = RequestMode.None;
                                break;
                            }
                        case RequestMode.LoadState:
                            {
                                StateHandler.LoadState(); PAUSED = false;
                                emu_request_mode = RequestMode.None;
                                break;
                            }
                        case RequestMode.TakeSnapshot:
                            {
                                MyNesMain.VideoProvider.TakeSnapshot(); PAUSED = false;
                                emu_request_mode = RequestMode.None;
                                break;
                            }
                    }
                    isPaused = true;// Cleared in frame finish method
                }
            }
        }
        internal static void EmuClockComponentsNTSC()
        {
            PPUClock();

            PollInterruptStatus();// On the second half of memory access time...  

            PPUClock();
            PPUClock();
            APUClock();
            DMAClock();

            mem_board.OnCPUClock();
        }
        internal static void EmuClockComponentsPALB()
        {
            PPUClock();

            PollInterruptStatus();// On the second half of memory access time...  

            PPUClock();
            PPUClock();

            pal_add_cycle++;
            if (pal_add_cycle == 5)
            {
                pal_add_cycle = 0;
                PPUClock();

                //In pal, ppu does 3.2 per 1 cpu cycle
                //here, every 5 cpu cycles, the ppu
                //will do 1 additional cycle
                //0.2 * 5 = 1
            }
            APUClock();
            DMAClock();

            mem_board.OnCPUClock();
        }
        internal static void ApplyFrameSkipSettings()
        {
            FrameSkipEnabled = MyNesMain.RendererSettings.FrameSkipEnabled;
            FrameSkipInterval = MyNesMain.RendererSettings.FrameSkipInterval;
        }
        private static void FrameFinished()
        {
            if (!FrameSkipEnabled)
                render_video(ref ppu_screen_pixels);
            else
            {
                FrameSkipCounter++;
                if (FrameSkipCounter >= FrameSkipInterval)
                {
                    render_video(ref ppu_screen_pixels);
                    FrameSkipCounter = 0;
                }
            }
            isPaused = false;
            ppu_frame_finished = false;
            emu_frame_done = true;

            joypad1.Update();
            joypad2.Update();
            if (IsFourPlayers)
            {
                joypad3.Update();
                joypad4.Update();
            }
            shortucts.Update();
            // Sound
            if (SoundEnabled)
            {
                render_audio_get_is_playing(out render_audio_is_playing);
                if (!render_audio_is_playing)
                    render_audio_toggle_pause(false);
                render_audio(ref audio_samples, ref audio_samples_added);

                audio_w_pos = 0;
                audio_samples_added = 0;
                audio_timer = 0;
            }

            fps_time_token = GetTime() - fps_time_start;

            if (FrameLimiterEnabled)
            {
                if (fps_time_token > 0)
                {
                    fps_time_dead = fps_time_period - fps_time_token;
                    if (fps_time_dead > 0)
                    {
                        Thread.Sleep((int)Math.Floor(fps_time_dead * 1000));

                        fps_time_dead = GetTime() - fps_time_start;
                        while (fps_time_period - fps_time_dead > 0)
                        {
                            fps_time_dead = GetTime() - fps_time_start;
                        }
                    }

                }
                fps_time_last = GetTime();
                fps_time_frame_time = fps_time_last - fps_time_start;
            }

            // This point is the start of the next frame
            fps_time_start = GetTime();
        }
        private static double GetTime()
        {
            return Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency;
        }
        public static void GetSpeedValues(out double frame_time, out double immediate_frame_time)
        {
            frame_time = fps_time_token;
            immediate_frame_time = fps_time_frame_time;
        }
        public static void SetFramePeriod(ref double period)
        {
            fps_time_period = period;
        }
        public static void GetTargetFPS(out double fps)
        {
            fps = emu_time_target_fps;
        }
        public static void ApplyRegionSetting()
        {
            RegionSetting regionSetting = (RegionSetting)MyNesMain.EmuSettings.RegionSetting;
            switch (regionSetting)
            {
                case RegionSetting.AUTO:
                    {
                        Tracer.WriteLine("REGION = AUTO");
                        Region = EmuRegion.NTSC;
                        EmuClockComponents = EmuClockComponentsNTSC;

                        bool found = false;
                        if (mem_board != null)
                        {
                            if (mem_board.IsGameFoundOnDB)
                            {
                                Tracer.WriteLine("REGION SELECTION IS FROM DATABASE !!");
                                if (mem_board.GameCartInfo.System.ToUpper().Contains("PAL"))
                                {
                                    Region = EmuRegion.PALB;
                                    EmuClockComponents = EmuClockComponentsPALB;
                                    found = true;
                                }
                                else if (mem_board.GameCartInfo.System.ToUpper().Contains("DENDY"))
                                {
                                    Region = EmuRegion.DENDY;
                                    EmuClockComponents = EmuClockComponentsNTSC;
                                    found = true;
                                }
                                else
                                {
                                    Region = EmuRegion.NTSC;
                                    EmuClockComponents = EmuClockComponentsNTSC;
                                    found = true;
                                }
                            }
                        }

                        if (!found)
                        {
                            if (CurrentFilePath.Contains("(E)"))
                            {
                                Region = EmuRegion.PALB;
                                EmuClockComponents = EmuClockComponentsPALB;
                            }
                        }
                        Tracer.WriteLine("REGION SELECTED: " + Region.ToString());
                        break;
                    }
                case RegionSetting.ForceNTSC:
                    {
                        Tracer.WriteLine("REGION: FORCE NTSC");
                        Region = EmuRegion.NTSC;
                        EmuClockComponents = EmuClockComponentsNTSC;
                        break;
                    }
                case RegionSetting.ForcePALB:
                    {
                        Tracer.WriteLine("REGION: FORCE PALB");
                        Region = EmuRegion.PALB;
                        EmuClockComponents = EmuClockComponentsPALB;
                        break;
                    }
                case RegionSetting.ForceDENDY:
                    {
                        Tracer.WriteLine("REGION: FORCE DENDY");
                        Region = EmuRegion.DENDY;
                        EmuClockComponents = EmuClockComponentsNTSC;
                        break;
                    }
            }
            SystemIndex = (int)Region;
        }
        public static string SHA1
        { get { return mem_board.SHA1; } }

        internal static void WriteStateData(ref BinaryWriter bin)
        {
            APUWriteState(ref bin);
            CPUWriteState(ref bin);
            DMAWriteState(ref bin);
            InterruptsWriteState(ref bin);
            MEMWriteState(ref bin);
            PORTWriteState(ref bin);
            PPUWriteState(ref bin);
        }
        internal static void ReadStateData(ref BinaryReader bin)
        {
            APUReadState(ref bin);
            CPUReadState(ref bin);
            DMAReadState(ref bin);
            InterruptsReadState(ref bin);
            MEMReadState(ref bin);
            PORTReadState(ref bin);
            PPUReadState(ref bin);
        }
    }
}
