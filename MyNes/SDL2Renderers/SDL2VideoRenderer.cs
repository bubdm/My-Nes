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
using MyNes.Core;
using SDL2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
namespace MyNes
{
    class SDL2VideoRenderer : IVideoProvider
    {
        public SDL2VideoRenderer()
        {
            timer = new System.Threading.Timer(new TimerCallback(FPSTimerCallback), null, 0, 1000);
        }
        private System.Threading.Timer timer;
        private bool initialized;
        private bool res_upscale;
        private bool keep_aspect_ratio;
        private int current_window_width;
        private int current_window_height;
        private int RENDER_WIDTH;
        private int RENDER_HEIGHT;
        const int NES_BUFFER_SIZE = 256 * 240;
        private int add_x;
        private int add_y;
        private bool add_x_use_flip_flop;
        private bool add_y_use_flip_flop;
        private bool add_x_flip_flop;
        private bool add_y_flip_flop;
        private int current_x_t = 0;
        private int current_y_t = 0;
        private bool isRendering;
        private bool LockPixels;
        private bool show_fps;
        private bool signal;
        private int current_pixel = 0;

        private IntPtr window;
        private IntPtr renderer;
        private SDL.SDL_Rect targetPixelsRect;
        // The render screen pixels
        private SDL.SDL_Rect renderPixelsRect;
        private IntPtr text_normal_Font;
        string fontFile = "";
        // Emu out pixels stuff
        private IntPtr pixels_texture;
        private IntPtr buffer_pixels;
        // Screen pixels buffer.
        private int[] buffer_pixels_tmp;
        private int buffer_size;
        // Screen buffer size.
        private int int_size;

        TextObject fps_text_object;

        private string fps_text;
        private double fps_avg;
        private int fps_count;
        private double fps_emu_imm_av;
        private double fps_emu_fr_av;
        private double fps_emu_clocks;

        private TextObject not_text_object;
        private List<string> not_texts;
        private List<Color> not_colors;
        private bool not_show;
        private int not_counter;

        private TextObject not_up_text_object;
        private TextObject not_turbo_text_object;

        private double fps_time_last;// the frame end time (time point)
        private double fps_time_start;// the frame start time (time point)
        private double fps_time_token;// how much time in seconds the rendering process toke
        private double fps_time_dead;// how much time remains for the target frame rate. For example, if a frame toke 3 milliseconds to complete, we'll have 16-3=13 dead time for 60 fps.
        private double fps_time_period;// how much time in seconds a frame should take for target fps. i.e. ~16 milliseconds for 60 fps.
        private double fps_time_frame_time;// the actual frame time after rendering and speed limiting. This should be equal to fps_time_period for perfect emulation timing.


        // Frame period in ticks

        // Thread
        internal bool threadON;
        internal bool threadPAUSED;
        private Thread threadMain;
        private Random r = new Random();
        private byte c;
        private bool take_snap_request;
        private bool take_snap_as_request;
        private string take_snap_as_file;
        private string take_snap_as_format;

        private SDL2Settings sdl_settings;

        private bool emu_use_thread;

        public string Name
        { get { return "SDL2 (OpenGL/Direct3D, OpenGL by default)"; } }
        public string ID
        { get { return "sdl2.video"; } }

        private void LoadSettings()
        {
            keep_aspect_ratio = MyNesMain.RendererSettings.Vid_KeepAspectRatio;
            show_fps = MyNesMain.RendererSettings.Vid_ShowFPS;
            res_upscale = MyNesMain.RendererSettings.Vid_Res_Upscale;
            fps_text = " ";

            ApplyRegionChanges();
        }
        public void Execute(string command)
        {
            switch (command)
            {
                case "resume": Tracer.WriteLine("SlimDX Direct3D: Excuting command '" + command + "'"); threadPAUSED = false; break;
                case "pause": Tracer.WriteLine("SlimDX Direct3D: Excuting command '" + command + "'"); threadPAUSED = true; break;
                case "shutdown": Tracer.WriteLine("SlimDX Direct3D: Excuting command '" + command + "'"); ShutDown(); break;
            }
        }
        public void Initialize()
        {
            sdl_settings = new SDL2Settings(Path.Combine(Program.WorkingFolder, "sdlsettings.ini"));
            sdl_settings.LoadSettings();

            LoadSettings();
            not_texts = new List<string>();
            not_colors = new List<Color>();
            // Set video driver and configuration
            SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_DRIVER, sdl_settings.Video_Driver);
            Tracer.WriteLine("SDL: video driver set to '" + sdl_settings.Video_Driver + "'");

            //SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_OPENGL_SHADERS, sdl_settings.Video_EnableOpenglShaders.ToString());
            //Tracer.WriteLine("SDL: video enable opengl shaders set to '" + sdl_settings.Video_EnableOpenglShaders + "'");

            Tracer.WriteLine("SDL: Initializing video ...");
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);

            /*
             * 0 or nearest
                 nearest pixel sampling
               1 or linear
                 linear filtering (supported by OpenGL and Direct3D)
               2 or best
	             anisotropic filtering (supported by Direct3D)
             */
            SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, MyNesMain.RendererSettings.Vid_Filter.ToString());
            Tracer.WriteLine("SDL: video render scale quality set to '" + MyNesMain.RendererSettings.Vid_Filter + "'");

            Tracer.WriteLine("SDL: making window...");
            window = SDL.SDL_CreateWindowFrom(Program.FormMain.panel_surface.Handle);

            Tracer.WriteLine("SDL: window made successfully.");

            // Set fullscreen
            Tracer.WriteLine("SDL: making renderer for video ...");
            // Create the renderer that will render the texture.
            SDL.SDL_RendererFlags render_flags = SDL.SDL_RendererFlags.SDL_RENDERER_TARGETTEXTURE;

            if (MyNesMain.RendererSettings.Vid_VSync && !MyNesMain.RendererSettings.FrameSkipEnabled && NesEmu.Region == EmuRegion.NTSC)
            {
                render_flags |= SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC;
                Tracer.WriteLine("SDL: VSYNC ENABLED");
            }
            /*if (sdl_settings.Video_Accelerated)
            {
                render_flags |= SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED;
                Tracer.WriteLine("SDL: ACCELERATION ENABLED");
            }
            if (sdl_settings.Video_Software)
            {
                render_flags |= SDL.SDL_RendererFlags.SDL_RENDERER_SOFTWARE;
                Tracer.WriteLine("SDL: SOFTWARE RENDER ENABLED");
            }*/
            renderer = SDL.SDL_CreateRenderer(window, -1, render_flags);

            Console.WriteLine("SDL: renderer is ready !");
            Console.WriteLine("SDL: making textures and buffers...");

            // RENDER RESOLUTION !!!!!!
            if (res_upscale)
            {
                RENDER_WIDTH = MyNesMain.RendererSettings.Vid_Res_W;// 640
                RENDER_HEIGHT = MyNesMain.RendererSettings.Vid_Res_H;// 480
            }
            else
            {
                RENDER_WIDTH = 256;
                RENDER_HEIGHT = 240;
            }

            add_x = (int)Math.Floor((double)RENDER_WIDTH / 256);
            add_x_use_flip_flop = ((double)RENDER_WIDTH / 256) % 1 != 0;

            add_y = (int)Math.Floor((double)RENDER_HEIGHT / 240);
            add_y_use_flip_flop = ((double)RENDER_HEIGHT / 240) % 1 != 0;

            // Create the texture
            pixels_texture = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_ARGB8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STATIC, RENDER_WIDTH, RENDER_HEIGHT);

            // Buffers.
            buffer_pixels_tmp = new int[buffer_size = (RENDER_WIDTH * RENDER_HEIGHT)];
            int_size = Marshal.SizeOf(buffer_pixels_tmp[0]) * buffer_pixels_tmp.Length;
            buffer_pixels = Marshal.AllocHGlobal(int_size);

            renderPixelsRect = new SDL.SDL_Rect();

            renderPixelsRect.x = 0;
            renderPixelsRect.y = 0;
            renderPixelsRect.w = RENDER_WIDTH;
            renderPixelsRect.h = RENDER_HEIGHT;

            LockPixels = false;
            CalculateTargetRect();
            Console.WriteLine("SDL: done !!");

            // Do other check, if it still doesn't exist, point to default font file.
            if (!File.Exists(fontFile))
                fontFile = Path.Combine(MyNesMain.AppPath, "FreeSans.ttf");

            Tracer.WriteLine("SDL: ->Initializing fonts ...");

            SDL_ttf.TTF_Init();
            Tracer.WriteLine("SDL: ->FONT FILE = " + fontFile);
            text_normal_Font = SDL_ttf.TTF_OpenFont(fontFile, 16);
            if (text_normal_Font == IntPtr.Zero)
                Tracer.WriteLine("SDL: ->ERORR !! NORMAL FONT DOESN'T LOADED !");

            fps_text_object = new TextObject(window, renderer, " ", text_normal_Font, System.Drawing.Color.Lime, new System.Drawing.Point(10, 10), false);
            not_text_object = new TextObject(window, renderer, " ", text_normal_Font, System.Drawing.Color.Lime, new System.Drawing.Point(10, targetPixelsRect.h - 50), false);
            not_up_text_object = new TextObject(window, renderer, " ", text_normal_Font, System.Drawing.Color.Red, new System.Drawing.Point(targetPixelsRect.w - 220, 10), false);
            not_turbo_text_object = new TextObject(window, renderer, " ", text_normal_Font, System.Drawing.Color.Yellow, new System.Drawing.Point(targetPixelsRect.w - 100, targetPixelsRect.h - 50), false);
            double target_fps = 0;
            switch (NesEmu.Region)
            {
                case EmuRegion.NTSC:
                    {
                        target_fps = 60.0988;
                        break;
                    }
                case EmuRegion.PALB:
                case EmuRegion.DENDY:
                    {
                        target_fps = 50.0;
                        break;
                    }
            }

            // Make thread
            if (!MyNesMain.RendererSettings.FrameSkipEnabled)
            {
                fps_time_period = 1.0 / target_fps;
            }
            else
            {
                fps_time_period = 1.0 / (target_fps / MyNesMain.RendererSettings.FrameSkipInterval);
            }

            threadON = true;
            threadPAUSED = false;
            initialized = true;
            Console.WriteLine("SDL: video initialized successfully.");
            threadMain = new Thread(new ThreadStart(ClockThread));
            threadMain.Start();
            // ClockThread();
        }
        internal void ClockThread()
        {
            while (threadON)
            {
                if (!threadPAUSED)
                    RunThreaded();
            }
        }
        internal void CloseThread()
        {
            threadON = false;


            // Get out of here !
            //thread.Abort();
            //this.Dispose();
        }
        private void RunThreaded()
        {
            if (!initialized)
                return;
            fps_time_start = GetTime();
            isRendering = true;
            if (take_snap_request)
            {
                take_snap_request = false;
                TakeSnapThreaded();
            }
            if (take_snap_as_request)
            {
                take_snap_as_request = false;
                TakeSnapshotAsThreaded();
            }
            // Render
            RenderBackground();

            if (not_show)
            {
                if (not_counter > 0)
                {
                    not_counter--;
                    if (not_texts.Count > 0)
                    {
                        not_text_object.Color = not_colors[0];
                        not_text_object.SetText(not_texts[0], ref window, ref renderer);
                        not_text_object.OnRender(ref window, ref renderer);
                    }
                }
                else
                {
                    if (not_texts.Count > 0)
                    {
                        not_texts.RemoveAt(0);
                        not_colors.RemoveAt(0);
                        if (not_texts.Count > 0)
                        {
                            not_counter = 120;
                            not_show = true;
                        }
                    }
                    else
                    {
                        not_show = false;
                    }
                }

            }
            if (show_fps)
                fps_text_object.OnRender(ref window, ref renderer);
            if (MyNesMain.WaveRecorder.IsRecording)
            {
                not_up_text_object.SetText("Recording Audio " + TimeSpan.FromSeconds(MyNesMain.WaveRecorder.Time), ref window, ref renderer);
                not_up_text_object.OnRender(ref window, ref renderer);
            }
            if (NesEmu.ON)
            {
                if (NesEmu.PAUSED)
                {
                    not_turbo_text_object.SetText("PAUSED !", ref window, ref renderer);
                    not_turbo_text_object.OnRender(ref window, ref renderer);
                }
                else if (!NesEmu.FrameLimiterEnabled)
                {
                    not_turbo_text_object.SetText("TURBO !", ref window, ref renderer);
                    not_turbo_text_object.OnRender(ref window, ref renderer);
                }
            }

            SDL.SDL_RenderPresent(renderer);

            isRendering = false;

            // EVENTS ******************************
            CheckEvents();

            //********* Speed limitter **********************************
            if (NesEmu.FrameLimiterEnabled)
            {
                fps_time_token = GetTime() - fps_time_start;
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
            }
            fps_time_last = GetTime();
            fps_time_frame_time = fps_time_last - fps_time_start;
            //***********************************************************

            fps_avg += (1.0 / fps_time_frame_time);
            fps_count++;
            //***********************************************************
        }
        private void RenderBackground()
        {
            if (!signal)// Emu is off, draw snow.
            {
                for (int i = 0; i < buffer_size; i++)
                {
                    c = (byte)r.Next(0, 255);

                    buffer_pixels_tmp[i] = c | (c << 8) | (c << 16) | (255 << 24);
                }
                Marshal.Copy(buffer_pixels_tmp, 0, buffer_pixels, buffer_size);
            }
            else
            {
                if (!emu_use_thread)
                    NesEmu.EMUClockFrame();
            }
            SDL.SDL_RenderClear(renderer);


            // Render normally without skipping
            if (!LockPixels)
            {
                SDL.SDL_UpdateTexture(pixels_texture, ref renderPixelsRect, buffer_pixels, RENDER_WIDTH * 4);
                // Render the pixels

                SDL.SDL_RenderCopy(renderer, pixels_texture, ref renderPixelsRect, ref targetPixelsRect);
            }
        }
        private void CheckEvents()
        {
            SDL.SDL_PumpEvents();

            if (SDL.SDL_HasEvent(SDL.SDL_EventType.SDL_QUIT) == SDL.SDL_bool.SDL_TRUE)
            {
                threadON = false;
            }
        }
        private void HandleWindowResize()
        {
            int window_w = 0;
            int window_h = 0;
            SDL.SDL_GetWindowSize(window, out window_w, out window_h);
            if (current_window_width != window_w || current_window_height != window_h)
            {
                CalculateTargetRect();
            }
        }
        internal void CalculateTargetRect()
        {
            int window_w = 0;
            int window_h = 0;
            SDL.SDL_GetWindowSize(window, out window_w, out window_h);
            targetPixelsRect = new SDL.SDL_Rect();
            if (keep_aspect_ratio)
            {
                // Calculate aspect ratio
                //GetRatioStretchRectangle(renderPixelsRect.w, renderPixelsRect.h, window_w, window_h, ref targetPixelsRect.x, ref targetPixelsRect.y, ref targetPixelsRect.w, ref targetPixelsRect.h);

                switch (NesEmu.Region)
                {
                    case EmuRegion.NTSC:
                        {
                            if (res_upscale)
                                GetRatioStretchRectangle(RENDER_WIDTH, RENDER_HEIGHT, window_w, window_h, ref targetPixelsRect.x, ref targetPixelsRect.y, ref targetPixelsRect.w, ref targetPixelsRect.h);
                            else
                                GetRatioStretchRectangle(640, 480, window_w, window_h, ref targetPixelsRect.x, ref targetPixelsRect.y, ref targetPixelsRect.w, ref targetPixelsRect.h);
                            break;
                        }
                    case EmuRegion.PALB:
                    case EmuRegion.DENDY:
                        {
                            GetRatioStretchRectangle(720, 576, window_w, window_h, ref targetPixelsRect.x, ref targetPixelsRect.y, ref targetPixelsRect.w, ref targetPixelsRect.h);
                            break;
                        }
                }

            }
            else
            {
                // As it is !
                targetPixelsRect.x = 0;
                targetPixelsRect.y = 0;
                targetPixelsRect.h = window_h;
                targetPixelsRect.w = window_w;
            }
            current_window_width = window_w;
            current_window_height = window_h;
            if (not_text_object != null)
                not_text_object.Position = new System.Drawing.Point(10, window_h - 50);
            if (not_up_text_object != null)
                not_up_text_object.Position = new System.Drawing.Point(window_w - 220, 10);
            if (not_turbo_text_object != null)
                not_turbo_text_object.Position = new System.Drawing.Point(window_w - 100, window_h - 50);
            Tracer.WriteLine("Resize, w=" + window_w + ", h=" + window_h);
        }
        private void GetRatioStretchRectangle(int orgWidth, int orgHeight, int maxWidth, int maxHeight,
                                                     ref int out_x, ref int out_y, ref int out_w, ref int out_h)
        {
            float hRatio = orgHeight / maxHeight;
            float wRatio = orgWidth / maxWidth;
            bool touchTargetFromOutside = false;
            if ((wRatio > hRatio) ^ touchTargetFromOutside)
            {
                out_w = maxWidth;
                out_h = (orgHeight * maxWidth) / orgWidth;
            }
            else
            {
                out_h = maxHeight;
                out_w = (orgWidth * maxHeight) / orgHeight;
            }

            out_x = (maxWidth / 2) - (out_w / 2);
            out_y = 0;
        }
        private void FPSTimerCallback(object state)
        {
            if (!initialized)
                return;
            // Collect video fps
            double vid_fps = 0;
            if (fps_count > 0)
                vid_fps = fps_avg / fps_count;

            fps_avg = fps_count = 0;

            if (NesEmu.ON)
            {
                // emu fps
                double emu_imm_fps = 0;
                double emu_fp_fps = 0;
                if (fps_emu_clocks > 0)
                {
                    emu_imm_fps = 1.0 / (fps_emu_imm_av / fps_emu_clocks);
                    emu_fp_fps = 1.0 / (fps_emu_fr_av / fps_emu_clocks);
                }

                // fps_text = string.Format("Video FPS: {0:F2} / Emu Clocked: {1} | Emu FPS: {2:F2} | Emu Can Make FPS: {3:F2}", vid_fps, fps_emu_clocks, emu_imm_fps, emu_fp_fps);
                if (emu_use_thread)
                    fps_text = string.Format("Video FPS: {0:F2} / Emu Video Clocks In Sec: {1} | Emu FPS: {2:F2} | Emu Can Make FPS: {3:F2}", vid_fps, fps_emu_clocks, emu_imm_fps, emu_fp_fps);
                else
                    fps_text = string.Format("Video FPS: {0:F2} | Emu FPS: {1:F2}", vid_fps, emu_imm_fps);
                fps_emu_imm_av = fps_emu_fr_av = fps_emu_clocks = 0;
            }
            else
            {
                fps_text = string.Format("Video FPS: {0:F2}", vid_fps);
            }
            fps_text_object.SetText(fps_text, ref window, ref renderer);
        }
        public void ResizeBegin()
        {
            threadPAUSED = true;
        }
        public void ResizeEnd()
        {
            sdl_settings.LoadSettings();
            Thread.Sleep(50);
            initialized = false;
            fps_text_object.Destroy();
            not_text_object.Destroy();
            not_turbo_text_object.Destroy();
            not_up_text_object.Destroy();
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyTexture(pixels_texture);
            // Set fullscreen
            Tracer.WriteLine("SDL: creating renderer for video ...");
            // Create the renderer that will render the texture.
            SDL.SDL_RendererFlags render_flags = SDL.SDL_RendererFlags.SDL_RENDERER_TARGETTEXTURE;

            if (MyNesMain.RendererSettings.Vid_VSync && !MyNesMain.RendererSettings.FrameSkipEnabled && NesEmu.Region == EmuRegion.NTSC)
            {
                render_flags |= SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC;
                Tracer.WriteLine("SDL: VSYNC ENABLED");
            }
            /*if (sdl_settings.Video_Accelerated)
            {
                render_flags |= SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED;
                Tracer.WriteLine("SDL: ACCELERATION ENABLED");
            }
            if (sdl_settings.Video_Software)
            {
                render_flags |= SDL.SDL_RendererFlags.SDL_RENDERER_SOFTWARE;
                Tracer.WriteLine("SDL: SOFTWARE RENDER ENABLED");
            }*/
            double target_fps = 0;
            switch (NesEmu.Region)
            {
                case EmuRegion.NTSC:
                    {
                        target_fps = 60.0;
                        break;
                    }
                case EmuRegion.PALB:
                case EmuRegion.DENDY:
                    {
                        target_fps = 50.0;
                        break;
                    }
            }
            // Make thread
            if (!MyNesMain.RendererSettings.FrameSkipEnabled)
            {
                fps_time_period = 1.0 / target_fps;
            }
            else
            {
                fps_time_period = 1.0 / (target_fps / MyNesMain.RendererSettings.FrameSkipInterval);
            }
            renderer = SDL.SDL_CreateRenderer(window, -1, render_flags);

            Console.WriteLine("SDL: creating texture..");
            // Create the texture
            pixels_texture = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_ARGB8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STATIC, RENDER_WIDTH, RENDER_HEIGHT);

            CalculateTargetRect();
            initialized = true;
            threadPAUSED = false;
        }
        public void ShutDown()
        {
            threadON = false;
            Thread.Sleep(100);
            threadMain.Abort();

            SDL.SDL_DestroyWindow(window);
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyTexture(pixels_texture);

            sdl_settings.SaveSettings();
        }
        public void SignalToggle(bool started)
        {
            signal = started;
            // Apply period configuration each time the signal comes in
            double target_fps = 0;
            switch (NesEmu.Region)
            {
                case EmuRegion.NTSC:
                    {
                        target_fps = 60.0988;
                        break;
                    }
                case EmuRegion.PALB:
                case EmuRegion.DENDY:
                    {
                        target_fps = 50.0;
                        break;
                    }
            }
            if (!MyNesMain.RendererSettings.FrameSkipEnabled)
            {
                fps_time_period = 1.0 / target_fps;
            }
            else
            {
                fps_time_period = 1.0 / (target_fps / MyNesMain.RendererSettings.FrameSkipInterval);
            }

            emu_use_thread = MyNesMain.RendererSettings.UseEmuThread;
        }
        public void SubmitFrame(ref int[] buffer)
        {
            if (!initialized)
                return;
            //  if (isRendering)
            //     return;
            if (!res_upscale)
            {
                Marshal.Copy(buffer, 0, buffer_pixels, buffer_size);
            }
            else
            {
                // Res of 640 x 480, 3:2 of 1:1 of 256 x 240. For NTSC and PAL.
                // Implementing the Resolution-Blocks-Upscaler method for image upscaling <https://github.com/alaahadid/Resolution-Blocks-Upscaler>
                current_x_t = 0;
                current_y_t = 0;
                add_y_flip_flop = false;
                add_x_flip_flop = false;

                for (int i = 0; i < NES_BUFFER_SIZE; i++)// no skip
                {
                    current_pixel = buffer[i];

                    // Fill a block
                    for (int y_t = 0; y_t < add_y; y_t++)
                    {
                        for (int x_t = 0; x_t < add_x; x_t++)
                        {
                            // Modify pixel here to change target pixel for effects ...
                            buffer_pixels_tmp[(current_x_t + x_t) + ((current_y_t + y_t) * RENDER_WIDTH)] = current_pixel;
                        }
                    }
                    current_x_t += add_x;
                    if (current_x_t >= RENDER_WIDTH)
                    {
                        current_x_t = 0;
                        current_y_t += add_y;
                        if (current_y_t >= RENDER_HEIGHT)
                        {
                            current_y_t = 0;
                        }

                        if (add_y_use_flip_flop)
                        {
                            add_y_flip_flop = !add_y_flip_flop;
                            if (add_y_flip_flop)
                                add_y++;
                            else
                                add_y--;
                        }
                    }

                    if (add_x_use_flip_flop)
                    {
                        add_x_flip_flop = !add_x_flip_flop;
                        if (add_x_flip_flop)
                            add_x++;
                        else
                            add_x--;
                    }
                }
                Marshal.Copy(buffer_pixels_tmp, 0, buffer_pixels, buffer_size);

            }

            double fps_emu_fr, fps_emu_imm = 0;

            NesEmu.GetSpeedValues(out fps_emu_fr, out fps_emu_imm);
            fps_emu_imm_av += fps_emu_imm;
            fps_emu_fr_av += fps_emu_fr;
            fps_emu_clocks++;
        }
        public void TakeSnapshot()
        {
            take_snap_request = true;
        }
        private void TakeSnapThreaded()
        {
            // make sure the file is not replaced
            string path = "";
            string fileName = "";
            if (NesEmu.CurrentFilePath != null)
                if (NesEmu.CurrentFilePath != "")
                    fileName = Path.GetFileNameWithoutExtension(NesEmu.CurrentFilePath);
            if (fileName == "")
                fileName = "snap_shot";
            if (!MyNesMain.EmuSettings.SnapsReplace)
            {
                int j = 0;
                path = Path.Combine(MyNesMain.EmuSettings.SnapsFolder, fileName + "_" + j + MyNesMain.EmuSettings.SnapsFormat);
                while (System.IO.File.Exists(path))
                {
                    j++;
                    path = Path.Combine(MyNesMain.EmuSettings.SnapsFolder, fileName + "_" + j + MyNesMain.EmuSettings.SnapsFormat);
                }
            }
            else
            {
                path = Path.Combine(MyNesMain.EmuSettings.SnapsFolder, fileName + MyNesMain.EmuSettings.SnapsFormat);
            }


            Tracer.WriteLine("Saving snap at " + path);

            Bitmap bt = new Bitmap(RENDER_WIDTH, RENDER_HEIGHT);
            int[] buffer = new int[buffer_size];
            Marshal.Copy(buffer_pixels, buffer, 0, buffer_size);
            for (int i = 0; i < buffer_size; i++)
            {
                int y = i / RENDER_WIDTH;
                int x = i % RENDER_WIDTH;
                bt.SetPixel(x, y, Color.FromArgb(buffer[i]));
            }
            switch (MyNesMain.EmuSettings.SnapsFormat)
            {
                case ".png":
                    {
                        bt.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    }
                case ".jpg":
                    {
                        bt.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    }
                case ".bmp":
                    {
                        bt.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    }
            }


            threadPAUSED = false;
            if (NesEmu.ON)
                NesEmu.PAUSED = false;

            Tracer.WriteInformation("Snapshot saved: " + Path.GetFileName(path));

            WriteInfoNotification("Snapshot file saved: " + Path.GetFileName(path), false);
        }
        public void TakeSnapshotAs(string fileName, string format)
        {
            take_snap_as_request = true;
            take_snap_as_file = fileName;
            take_snap_as_format = format;
        }
        private void TakeSnapshotAsThreaded()
        {
            Tracer.WriteLine("Saving snap at " + take_snap_as_file);

            Bitmap bt = new Bitmap(RENDER_WIDTH, RENDER_HEIGHT);
            int[] buffer = new int[buffer_size];
            Marshal.Copy(buffer_pixels, buffer, 0, buffer_size);
            for (int i = 0; i < buffer_size; i++)
            {
                int y = i / RENDER_WIDTH;
                int x = i % RENDER_WIDTH;
                bt.SetPixel(x, y, Color.FromArgb(buffer[i]));
            }
            switch (take_snap_as_format)
            {
                case ".png":
                    {
                        bt.Save(take_snap_as_file, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    }
                case ".jpg":
                    {
                        bt.Save(take_snap_as_file, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    }
                case ".bmp":
                    {
                        bt.Save(take_snap_as_file, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    }
            }


            threadPAUSED = false;
            if (NesEmu.ON)
                NesEmu.PAUSED = false;

            Tracer.WriteInformation("Snapshot saved: " + Path.GetFileName(take_snap_as_file));
        }
        public void WriteErrorNotification(string message, bool instant)
        {
            if (!MyNesMain.RendererSettings.Vid_ShowNotifications)
                return;
            if (instant)
            {
                not_texts.Clear();
                not_colors.Clear();
                not_texts.Add(message);
                not_colors.Add(Color.Red);

                not_show = true;
                not_counter = 120;
            }
            else if (!not_texts.Contains(message))
            {
                not_texts.Add(message);
                not_counter = 120;
                not_colors.Add(Color.Red);
                not_show = true;
            }
        }
        public void WriteInfoNotification(string message, bool instant)
        {
            if (!MyNesMain.RendererSettings.Vid_ShowNotifications)
                return;
            if (instant)
            {
                not_texts.Clear();
                not_colors.Clear();

                not_texts.Add(message);
                not_colors.Add(Color.Lime);

                not_show = true;
                not_counter = 120;
            }
            else if (!not_texts.Contains(message))
            {
                not_texts.Add(message);
                not_counter = 120;
                not_colors.Add(Color.Lime);
                not_show = true;
            }
        }
        public void WriteWarningNotification(string message, bool instant)
        {
            if (!MyNesMain.RendererSettings.Vid_ShowNotifications)
                return;
            if (instant)
            {
                not_texts.Clear();
                not_colors.Clear();
                not_texts.Add(message);
                not_colors.Add(Color.Yellow);

                not_show = true;
                not_counter = 120;
            }
            else if (!not_texts.Contains(message))
            {
                not_texts.Add(message);
                not_counter = 120;
                not_colors.Add(Color.Yellow);
                not_show = true;
            }
        }
        private double GetTime()
        {
            return (double)Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency;
        }
        public void ApplyRegionChanges()
        {

        }
        public void Resume()
        {
            threadPAUSED = false;
        }
        public void ToggleAspectRatio(bool keep_aspect)
        {
            keep_aspect_ratio = keep_aspect;
            CalculateTargetRect();
        }
        public void ToggleFPS(bool show_fps)
        {
            this.show_fps = show_fps;
        }
        public void ApplyFilter()
        {
            threadPAUSED = true;
            SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, MyNesMain.RendererSettings.Vid_Filter.ToString());
            Tracer.WriteLine("SDL: video render scale quality set to '" + MyNesMain.RendererSettings.Vid_Filter + "'");

            ResizeEnd();
        }
    }
}
