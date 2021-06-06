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
using SlimDX;
using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MyNes
{
    unsafe class SlimDXVideoProvider : IVideoProvider
    {
        public SlimDXVideoProvider()
        {
            timer = new System.Threading.Timer(new TimerCallback(FPSTimerCallback), null, 0, 1000);
        }
        private System.Threading.Timer timer;
        private Control surface_control;
        private Direct3D direct3D = new Direct3D();
        private int[] currentBuffer;
        private int linesToSkip = 8;
        private int scanlines;
        private Rectangle originalRect;
        private Rectangle destinationRect;
        private DisplayMode currentMode;
        private PresentParameters presentParams;
        private SlimDX.Direct3D9.Font font;
        private SlimDX.Direct3D9.Device displayDevice;
        private Format displayFormat;
        private Surface bufferedSurface;
        private Surface frontSurface;
        private Sprite displaySprite;
        private bool canRender;
        private int bufferSize;
        private bool initialized;
        private bool disposed;
        private bool isRendering;
        private bool deviceLost;
        private bool fullScreen;
        private bool hardware_vertex_processing;
        private bool keep_aspect;
        private List<string> not_texts;
        private bool not_show;
        private int not_counter;
        private List<Color> not_colors;

        private double fps_time_last;// the frame end time (time point)
        private double fps_time_start;// the frame start time (time point)
        private double fps_time_token;// how much time in seconds the rendering process toke
        private double fps_time_dead;// how much time remains for the target frame rate. For example, if a frame toke 3 milliseconds to complete, we'll have 16-3=13 dead time for 60 fps.
        private double fps_time_period;// how much time in seconds a frame should take for target fps. i.e. ~16 milliseconds for 60 fps.
        private double fps_time_frame_time;// the actual frame time after rendering and speed limiting. This should be equal to fps_time_period for perfect emulation timing.

        private SlimDX.Direct3D9.TextureFilter filter = TextureFilter.Point;

        internal bool show_fps;
        private string fps_text;
        private double fps_avg;
        private int fps_count;
        private double fps_emu_imm_av;
        private double fps_emu_fr_av;
        private double fps_emu_clocks;
        // Thread
        private Thread thread;
        public bool threadON;
        public bool threadPAUSED;
        private Random r = new Random();
        private byte c;
        private Result result;
        public string Name { get { return "SlimDX (Dircet3D)"; } }
        public string ID { get { return "slimdx.video"; } }

        private bool SignalON;
        private bool signal_old;
        private bool isResizing;
        private bool take_snap_next_frame = false;
        private bool take_snap_as_next_frame = false;
        private string take_snap_as_file_name = "";
        private string take_snap_as_format = "";

        private bool emu_use_thread;

        public void Initialize()
        {
            LoadSettings();
            not_colors = new List<Color>();
            not_texts = new List<string>();
            surface_control = Program.FormMain.panel_surface;
            currentBuffer = new int[256 * 240];
            originalRect = new Rectangle(0, 0, 256, scanlines);

            currentMode = direct3D.Adapters[0].CurrentDisplayMode;
            Tracer.WriteLine("SlimDX Direct3D: Initializing Direct 3d video device ...");
            try
            {
                if (!fullScreen)
                {
                    presentParams = new PresentParameters();
                    presentParams.BackBufferWidth = surface_control.Width;
                    presentParams.BackBufferCount = 1;
                    presentParams.BackBufferHeight = surface_control.Height;

                    presentParams.BackBufferFormat = Format.A8R8G8B8;
                    presentParams.Windowed = true;
                    presentParams.SwapEffect = SwapEffect.Flip;
                    presentParams.Multisample = MultisampleType.None;
                    if (!MyNesMain.RendererSettings.FrameSkipEnabled)
                        presentParams.PresentationInterval = MyNesMain.RendererSettings.Vid_VSync ? PresentInterval.Default : PresentInterval.Immediate;
                    else // VSynch ignored in frame skip.
                        presentParams.PresentationInterval = PresentInterval.Immediate;
                    fps_time_period = 1.0 / 60.0988;
                    if (MyNesMain.RendererSettings.FrameSkipEnabled)
                    {
                        fps_time_period = 1.0 / (60.0988 / MyNesMain.RendererSettings.FrameSkipInterval);
                    }

                    displayFormat = currentMode.Format;
                    displayDevice = new SlimDX.Direct3D9.Device(direct3D, 0, SlimDX.Direct3D9.DeviceType.Hardware, surface_control.Handle,
                    hardware_vertex_processing ? CreateFlags.HardwareVertexProcessing : CreateFlags.SoftwareVertexProcessing,
                    presentParams);
                    int x = 0;
                    int y = 0;
                    int w = 0;
                    int h = 0;
                    if (keep_aspect)
                    {
                        switch (NesEmu.Region)
                        {
                            case EmuRegion.NTSC:
                                {
                                    GetRatioStretchRectangle(640, 480, surface_control.Width, surface_control.Height, ref x, ref y, ref w, ref h);
                                    break;
                                }
                            case EmuRegion.PALB:
                            case EmuRegion.DENDY:
                                {
                                    GetRatioStretchRectangle(720, 576, surface_control.Width, surface_control.Height, ref x, ref y, ref w, ref h);
                                    break;
                                }
                        }

                        destinationRect = new Rectangle(x, y, w, h);
                    }
                    else
                        destinationRect = new Rectangle(0, 0, surface_control.Width, surface_control.Height);

                    bufferSize = (256 * scanlines * 4);
                    font = new SlimDX.Direct3D9.Font(displayDevice,
                    new System.Drawing.Font("Tahoma", 14, FontStyle.Bold));
                    CreateDisplayObjects();
                    initialized = true;
                    disposed = false;

                    //  surface_control.Select();

                    canRender = true;
                }

                Tracer.WriteLine("SlimDX Direct3D: Direct 3d video device initialized.");
            }
            catch (Exception e)
            {
                initialized = false;
                Tracer.WriteLine("SlimDX Direct3D: Failed to initialize the Direct 3d video device:");
                Tracer.WriteLine(e.Message);
            }

            thread = new Thread(new ThreadStart(ClockThread));
            threadON = true;
            threadPAUSED = false;
            thread.Start();
        }
        internal void LoadSettings()
        {
            ApplyRegionChanges();
            keep_aspect = MyNesMain.RendererSettings.Vid_KeepAspectRatio;
            show_fps = MyNesMain.RendererSettings.Vid_ShowFPS;
            hardware_vertex_processing = MyNesMain.RendererSettings.Vid_HardwareVertexProcessing;
            filter = MyNesMain.RendererSettings.Vid_Filter == 1 ? TextureFilter.Linear : TextureFilter.Point;
        }
        public void ApplyRegionChanges()
        {
            scanlines = 240;
            linesToSkip = 0;
        }
        private void CreateDisplayObjects()
        {
            displaySprite = new Sprite(displayDevice);
            bufferedSurface = Surface.CreateRenderTarget(displayDevice, 256, scanlines, Format.A8R8G8B8,
                MultisampleType.None, 0, true);

            frontSurface = displayDevice.GetBackBuffer(0, 0);
        }
        private void DisposeDisplayObjects()
        {
            Console.WriteLine("Direct3D: shutdown video ...");
            if (displaySprite != null)
                displaySprite.Dispose();

            if (bufferedSurface != null)
                bufferedSurface.Dispose();

            if (frontSurface != null)
                frontSurface.Dispose();

            if (font != null)
                font.Dispose();
            Console.WriteLine("Direct3D: video shutdown done.");
        }
        private void ResetDirect3D()
        {
            if (!initialized)
                return;

            DisposeDisplayObjects();

            try
            {
                displayDevice.Reset(presentParams);
                CreateDisplayObjects();
            }
            catch
            {
                displayDevice.Dispose();
                Initialize();
            }
        }
        public void ShutDown()
        {
            if (disposed)
                return;
            threadON = false;
            // Give sometime to finish video thread job
            Thread.Sleep(50);

            if (displayDevice != null)
            {
                displayDevice.Dispose();
            }

            this.DisposeDisplayObjects();

            disposed = true;
        }
        private void ClockThread()
        {
            while (threadON)
            {
                if (!threadPAUSED)
                {
                    PresentFrame();
                }
            }
        }

        private void PresentFrame()
        {
            if (disposed)
            { isRendering = false; return; }
            if (!initialized)
            { isRendering = false; return; }

            fps_time_start = GetTime();

            //if (displayDevice == null)
            // { isRendering = false; return; }

            // Make a snow buffer when emulation is off
            if (!SignalON)
            {
                for (int i = 0; i < currentBuffer.Length; i++)
                {
                    c = (byte)r.Next(0, 255);
                    currentBuffer[i] = c | (c << 8) | (c << 16) | (255 << 24);
                }
            }
            else
            {
                if (!emu_use_thread)
                    NesEmu.EMUClockFrame();
            }
            if (take_snap_next_frame)
            {
                take_snap_next_frame = false;
                TakeSnapshotThreaded();
            }
            if (take_snap_as_next_frame)
            {
                take_snap_as_next_frame = false;
                TakeSnapshotAsThreaded(take_snap_as_file_name, take_snap_as_format);
            }
            isRendering = true;
            /*Result result = Result.Last;
          try
           {
               result = displayDevice.TestCooperativeLevel();
           }
           catch
           {
               isRendering = false;
               return;
           }*/
            result = displayDevice.TestCooperativeLevel();
            switch (result.Code)
            {
                case -2005530510:
                case -2005530520: deviceLost = true; isRendering = false; break;
                case -2005530519: deviceLost = false; isRendering = false; ResetDirect3D(); break;
            }
            // Render current buffer
            if (!deviceLost && canRender && !disposed)
            {
                DataRectangle rect = bufferedSurface.LockRectangle(LockFlags.Discard);

                rect.Data.WriteRange(currentBuffer, linesToSkip * 256, 256 * scanlines);

                rect.Data.Close();
                bufferedSurface.UnlockRectangle();

                displayDevice.ColorFill(frontSurface, Color.Black);
                // copy the surface data to the device one with stretch. 
                // if (canRender && !disposed)
                displayDevice.StretchRectangle(bufferedSurface, originalRect, frontSurface, destinationRect, filter);

                displayDevice.BeginScene();

                displaySprite.Begin(SpriteFlags.AlphaBlend);

                if (not_show)
                {
                    if (not_counter > 0)
                    {
                        not_counter--;
                        if (not_texts.Count > 0)
                        {

                            font.DrawString(displaySprite, not_texts[0], 10, surface_control.Height - 50, not_colors[0]);
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
                {
                    font.DrawString(displaySprite, fps_text, 10, 10, Color.Lime);
                }
                if (MyNesMain.WaveRecorder.IsRecording)
                {
                    font.DrawString(displaySprite, "Recording Audio " + TimeSpan.FromSeconds(MyNesMain.WaveRecorder.Time), surface_control.Width - 300, 10, Color.Red);
                }
                if (NesEmu.ON)
                {
                    if (!NesEmu.FrameLimiterEnabled)
                    {
                        font.DrawString(displaySprite, "TURBO !", surface_control.Width - 100, surface_control.Height - 50, Color.Yellow);
                    }
                    if (NesEmu.PAUSED)
                    {
                        font.DrawString(displaySprite, "PAUSED !", surface_control.Width - 130, surface_control.Height - 50, Color.Red);
                    }
                }

                // FPS Limiting
                /*
                current_time = GetTime();
                frame_time = current_time - last_time;
                dead_time = frame_period - frame_time;

                if (dead_time > 0)
                    Thread.Sleep((int)(dead_time * 1000));

                fps_avg += (1.0 / (GetTime() - last_time));
                fps_count++;

                last_time = current_time;*/

                //********* Speed limitter **********************************
                fps_time_token = GetTime() - fps_time_start;
                if (fps_time_token > 0)
                {
                    fps_time_dead = fps_time_period - fps_time_token;
                    if (fps_time_dead > 0)
                    {
                        Thread.Sleep((int)(fps_time_dead * 1000));

                        fps_time_dead = GetTime() - fps_time_start;
                        while (fps_time_period - fps_time_dead > 0)
                        {
                            fps_time_dead = GetTime() - fps_time_start;
                        }
                    }
                }
                fps_avg += (1.0 / fps_time_frame_time);
                fps_count++;

                fps_time_last = GetTime();
                fps_time_frame_time = fps_time_last - fps_time_start;
                //***********************************************************

                displaySprite.End();
                displayDevice.EndScene();

                displayDevice.Present();
            }
            isRendering = false;
        }
        private void FPSTimerCallback(object state)
        {
            // Collect video fps
            double vid_fps = 0;
            if (fps_count > 0)
                vid_fps = fps_avg / fps_count;

            fps_avg = fps_count = 0;

            // emu fps
            double emu_imm_fps = 0;
            double emu_fp_fps = 0;
            if (fps_emu_clocks > 0)
            {
                emu_imm_fps = 1.0 / (fps_emu_imm_av / fps_emu_clocks);
                emu_fp_fps = 1.0 / (fps_emu_fr_av / fps_emu_clocks);
            }

            fps_text = string.Format("Video FPS: {0:F2} / Emu Clocked: {1}\nEmu FPS: {2:F2}\nEmu Can Make FPS: {3:F2}", vid_fps, fps_emu_clocks, emu_imm_fps, emu_fp_fps);

            fps_emu_imm_av = fps_emu_fr_av = fps_emu_clocks = 0;
        }
        public void SignalToggle(bool started)
        {
            SignalON = started;

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
            if (SignalON && canRender && initialized)
            {
                buffer.CopyTo(currentBuffer, 0);
            }
            double fps_emu_fr, fps_emu_imm = 0;

            NesEmu.GetSpeedValues(out fps_emu_fr, out fps_emu_imm);
            fps_emu_imm_av += fps_emu_imm;
            fps_emu_fr_av += fps_emu_fr;
            fps_emu_clocks++;
        }
        public void TakeSnapshot()
        {
            take_snap_next_frame = true;
        }
        private void TakeSnapshotThreaded()
        {
            canRender = false;
            //while (isRendering) { }
            threadPAUSED = true;
            //NesEmu.EmulationPaused = true; 
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
            TakeSnapshotAs(path, MyNesMain.EmuSettings.SnapsFormat);
        }
        public void TakeSnapshotAs(string image_path, string format)
        {
            take_snap_as_next_frame = true;
            take_snap_as_file_name = image_path;
            take_snap_as_format = format;
        }
        public void TakeSnapshotAsThreaded(string image_path, string format)
        {
            threadPAUSED = true;
            canRender = false;
            // get image data as bitmap
            Bitmap bitmap = new Bitmap(256, scanlines);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, 256, scanlines), ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb);
            int* pointer = (int*)bitmapData.Scan0;

            var rect = bufferedSurface.LockRectangle(LockFlags.Discard);
            byte[] data = new byte[bufferSize];
            rect.Data.Read(data, 0, bufferSize);
            rect.Data.Close();
            bufferedSurface.UnlockRectangle();

            for (int i = 0; i < data.Length; i += 4)
            {
                *pointer++ =
                    (data[i + 3] << 0x18) | // A
                    (data[i + 2] << 0x10) | // R
                    (data[i + 1] << 0x08) | // G
                    (data[i + 0] << 0x00);  // B
            }

            bitmap.UnlockBits(bitmapData);

            switch (format)
            {
                case ".bmp": bitmap.Save(image_path, ImageFormat.Bmp); break;
                case ".gif": bitmap.Save(image_path, ImageFormat.Gif); break;
                case ".jpg": bitmap.Save(image_path, ImageFormat.Jpeg); break;
                case ".png": bitmap.Save(image_path, ImageFormat.Png); break;
                case ".tiff": bitmap.Save(image_path, ImageFormat.Tiff); break;
                case ".emf": bitmap.Save(image_path, ImageFormat.Emf); break;
                case ".wmf": bitmap.Save(image_path, ImageFormat.Wmf); break;
                case ".exif": bitmap.Save(image_path, ImageFormat.Exif); break;
            }

            threadPAUSED = false;
            canRender = true;
            WriteInfoNotification("Snapshot taken. [" + Path.GetFileName(image_path) + "]", false);
            Console.WriteLine("Direct3D: Snapshot taken");
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
        public void ResizeBegin()
        {
            if (isResizing)
                return;
            isResizing = true;

            threadPAUSED = true;
            // Give sometime to finish video thread job. If the video frame take longer than 100 milli, an exception will be thrown.
            Thread.Sleep(100);
            // Do a resize
            while (isRendering) { }
        }
        public void ResizeEnd()
        {
            if (!isResizing)
                return;

            canRender = false;
            //shutdown renderer
            ShutDown();
            //re-Initialize
            Initialize();
            threadPAUSED = false;
            isResizing = false;
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
        public void Resume()
        {
            threadPAUSED = false;
        }
        public void ToggleAspectRatio(bool keep_aspect)
        {
            this.keep_aspect = keep_aspect;
            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;
            if (keep_aspect)
            {

                switch (NesEmu.Region)
                {
                    case EmuRegion.NTSC:
                        {
                            GetRatioStretchRectangle(640, 480, surface_control.Width, surface_control.Height, ref x, ref y, ref w, ref h);
                            break;
                        }
                    case EmuRegion.PALB:
                    case EmuRegion.DENDY:
                        {
                            GetRatioStretchRectangle(720, 576, surface_control.Width, surface_control.Height, ref x, ref y, ref w, ref h);
                            break;
                        }
                }

                destinationRect = new Rectangle(x, y, w, h);
            }
            else
                destinationRect = new Rectangle(0, 0, surface_control.Width, surface_control.Height);
        }
        public void ToggleFPS(bool show_fps)
        {
            this.show_fps = show_fps;
        }
        public void ApplyFilter()
        {
            filter = MyNesMain.RendererSettings.Vid_Filter == 1 ? TextureFilter.Linear : TextureFilter.Point;
        }
    }
}
