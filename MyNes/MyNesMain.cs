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
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MyNes.Core
{
    public class MyNesMain
    {
        public static void Initialize(bool setupRenderers)
        {
            Tracer.WriteLine("Initializing My Nes Core ....");
            AppPath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            if (AppPath == "")
                AppPath = Path.GetFullPath(@".\");
            WorkingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyNes");
            Directory.CreateDirectory(WorkingFolder);

            Tracer.WriteLine("Loading emu settings ...");

            EmuSettings = new EmuSettings(Path.Combine(WorkingFolder, "emusettings.ini"));
            EmuSettings.LoadSettings();

            Tracer.WriteLine("Emu settings loaded successfully.");

            Tracer.WriteLine("Loading renderer settings ...");

            RendererSettings = new RendererSettings(Path.Combine(WorkingFolder, "renderersettings.ini"));
            RendererSettings.LoadSettings();

            Tracer.WriteLine("Renderer settings loaded successfully.");

            Tracer.WriteLine("Locating boards and providers ...");
            WaveRecorder = new WaveRecorder();
            Boards = new List<Board>();
            VideoProviders = new List<IVideoProvider>();
            AudioProviders = new List<IAudioProvider>();
            string[] files = Directory.GetFiles(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                try
                {
                    if (Path.GetExtension(file).ToLower() == ".exe" || Path.GetExtension(file).ToLower() == ".dll")
                    {
                        Tracer.WriteLine("Reading assembly: " + file);

                        Assembly ass = Assembly.LoadFile(file);
                        if (ass != null)
                        {
                            Type[] types = ass.GetTypes();
                            foreach (Type tp in types)
                            {
                                if (tp.IsSubclassOf(typeof(Board)) && !tp.IsAbstract)
                                {
                                    Board brd = Activator.CreateInstance(tp) as Board;
                                    Boards.Add(brd);
                                    Tracer.WriteLine("Board added: " + brd.Name + " [ Mapper " + brd.MapperNumber + "]");
                                }
                                else if (tp.GetInterface("MyNes.Core.IVideoProvider") != null)
                                {
                                    // This is a video provider !!
                                    IVideoProvider prov = Activator.CreateInstance(tp) as IVideoProvider;
                                    VideoProviders.Add(prov);
                                    Tracer.WriteLine("Video provider added: " + prov.Name + " [" + prov.ID + "]");
                                }
                                else if (tp.GetInterface("MyNes.Core.IAudioProvider") != null)
                                {
                                    // This is a video provider !!
                                    IAudioProvider aprov = Activator.CreateInstance(tp) as IAudioProvider;
                                    AudioProviders.Add(aprov);
                                    Tracer.WriteLine("Audio provider added: " + aprov.Name + " [" + aprov.ID + "]");
                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tracer.WriteLine("ERROR: " + ex.ToString());
                }
            }
            Tracer.WriteInformation("Done.");
            Tracer.WriteInformation("Total of " + Boards.Count + " board found.");
            Tracer.WriteInformation("Total of " + VideoProviders.Count + " video provider found.");
            Tracer.WriteInformation("Total of " + AudioProviders.Count + " audio provider found.");
            if (setupRenderers)
            {
                SetVideoProvider();
                SetAudioProvider();
                SetRenderingMethods();
            }

            NesEmu.Initialize();
        }
        public static void Shutdown()
        {
            if (NesEmu.ON)
                NesEmu.ShutDown();
            if (VideoProvider != null)
                VideoProvider.ShutDown();
            if (AudioProvider != null)
                AudioProvider.ShutDown();
            Tracer.WriteLine("Saving settings ...");
            EmuSettings.SaveSettings();
            RendererSettings.SaveSettings();
            Tracer.WriteLine("Settings saved successfully.");
            Tracer.WriteLine("Exiting My Nes.");
        }
        public static EmuSettings EmuSettings
        {
            get; private set;
        }
        public static RendererSettings RendererSettings
        {
            get; private set;
        }
        public static string AppPath
        {
            get; private set;
        }
        public static string WorkingFolder
        {
            get; private set;
        }
        internal static List<Board> Boards
        { get; private set; }
        public static List<IVideoProvider> VideoProviders
        {
            get; private set;
        }
        public static List<IAudioProvider> AudioProviders
        {
            get; private set;
        }
        /// <summary>
        /// Get current video provider
        /// </summary>
        public static IVideoProvider VideoProvider
        {
            get; private set;
        }
        public static IAudioProvider AudioProvider
        {
            get; private set;
        }
        public static WaveRecorder WaveRecorder
        {
            get; private set;
        }

        internal static bool IsBoardExist(int mapper)
        {
            foreach (Board brd in Boards)
            {
                if (brd.MapperNumber == mapper)
                {
                    return true;
                }
            }
            return false;
        }
        internal static Board GetBoard(int mapper)
        {
            foreach (Board brd in Boards)
            {
                if (brd.MapperNumber == mapper)
                {
                    return brd;
                }
            }
            return null;
        }
        public static void MakeWorkingFolder()
        {
            WorkingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyNes");
            Directory.CreateDirectory(WorkingFolder);
        }
        /// <summary>
        /// Get boards list
        /// </summary>
        /// <returns></returns>
        public static BoardInfoObject[] GetBoardsList(bool includeUnsupportedMappers)
        {
            List<BoardInfoObject> list = new List<BoardInfoObject>();
            if (includeUnsupportedMappers)
            {
                for (int i = 0; i < 256; i++)
                {
                    bool found = false;
                    foreach (Board brd in Boards)
                    {
                        if (brd.MapperNumber == i)
                        {
                            BoardInfoObject bb = new BoardInfoObject();
                            bb.Name = brd.Name;
                            bb.MapperNumber = brd.MapperNumber;
                            bb.IsSupported = true;
                            bb.HasIssues = brd.HasIssues;
                            bb.Issues = brd.Issues;

                            list.Add(bb);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        BoardInfoObject bb = new BoardInfoObject();
                        bb.Name = "N/A";
                        bb.MapperNumber = i;
                        bb.IsSupported = false;
                        bb.HasIssues = false;
                        bb.Issues = "";

                        list.Add(bb);
                    }
                }
            }
            else
            {
                foreach (Board brd in Boards)
                {
                    if (brd.MapperNumber < 0)
                        continue;
                    BoardInfoObject bb = new BoardInfoObject();
                    bb.Name = brd.Name;
                    bb.MapperNumber = brd.MapperNumber;
                    bb.IsSupported = true;
                    bb.HasIssues = brd.HasIssues;
                    bb.Issues = brd.Issues;

                    list.Add(bb);
                }
            }
            return list.ToArray();
        }
        public static IVideoProvider GetVideoProvider(string id)
        {
            foreach (IVideoProvider v in VideoProviders)
            {
                if (v.ID == id)
                {
                    return v;
                }
            }
            return null;
        }
        public static IAudioProvider GetAudioProvider(string id)
        {
            foreach (IAudioProvider a in AudioProviders)
            {
                if (a.ID == id)
                {
                    return a;
                }
            }
            return null;
        }
        public static void SetVideoProvider()
        {
            Tracer.WriteLine("Looking for the video provider that set in the settings...");

            VideoProvider = GetVideoProvider(RendererSettings.Video_ProviderID);

            if (VideoProvider == null)
            {
                Tracer.WriteError("ERROR: cannot find the video provider that set in the settings");
                Tracer.WriteWarning("Deciding video provider");
                if (VideoProviders.Count > 0)
                {
                    RendererSettings.Video_ProviderID = VideoProviders[0].ID;
                    VideoProvider = VideoProviders[0];
                    if (VideoProvider != null)
                    {
                        Tracer.WriteInformation("Video provider set to " + VideoProvider.Name + " [" + VideoProvider.ID + "]");
                        VideoProvider.Initialize();
                    }
                    else
                    {
                        Tracer.WriteError("ERROR: cannot set video provider.");
                    }
                }
                else
                {
                    Tracer.WriteError("ERROR: cannot set video provider, no video providers located.");
                }
            }
            else
            {
                Tracer.WriteInformation("Video provider set to " + VideoProvider.Name + " [" + VideoProvider.ID + "]");
                VideoProvider.Initialize();
            }
        }
        public static void SetAudioProvider()
        {
            Tracer.WriteLine("Looking for the audio provider that set in the settings...");

            AudioProvider = GetAudioProvider(RendererSettings.Audio_ProviderID);

            if (AudioProvider == null)
            {
                Tracer.WriteError("ERROR: cannot find the audio provider that set in the settings");
                Tracer.WriteWarning("Deciding audio provider");
                if (AudioProviders.Count > 0)
                {
                    RendererSettings.Audio_ProviderID = AudioProviders[0].ID;
                    AudioProvider = AudioProviders[0];
                    if (AudioProvider != null)
                    {
                        Tracer.WriteInformation("Audio provider set to " + AudioProvider.Name + " [" + AudioProvider.ID + "]");
                        AudioProvider.Initialize();
                    }
                    else
                    {
                        Tracer.WriteError("ERROR: cannot set audio provider.");
                    }
                }
                else
                {
                    Tracer.WriteError("ERROR: cannot set audio provider, no audio providers located.");
                }
            }
            else
            {
                Tracer.WriteInformation("Audio provider set to " + AudioProvider.Name + " [" + AudioProvider.ID + "]");
                AudioProvider.Initialize();
            }
        }
        public static void SetRenderingMethods()
        {
            if (VideoProvider != null && AudioProvider != null)
            {
                NesEmu.SetupRenderingMethods(VideoProvider.SubmitFrame, AudioProvider.SubmitSamples, AudioProvider.TogglePause, AudioProvider.GetIsPlaying);
            }
            else
            {
                Tracer.WriteError("ERROR: unable to setup rendering methods, one (or both) of the providers is not set (video and/or audio provider)");
            }
        }
        public static void RecordWave()
        {
            if (NesEmu.ON && NesEmu.SoundEnabled)
            {
                string file = Path.Combine(EmuSettings.WavesFolder, Path.GetFileNameWithoutExtension(NesEmu.CurrentFilePath) + ".wav");
                int i = 0;
                while (File.Exists(file))
                {
                    file = Path.Combine(EmuSettings.WavesFolder, Path.GetFileNameWithoutExtension(NesEmu.CurrentFilePath) + "_" + i + ".wav");
                    i++;
                }
                WaveRecorder.Record(file, 1, 16, RendererSettings.Audio_Frequency);
            }
            else
            {
                if (VideoProvider != null)
                    VideoProvider.WriteErrorNotification("Cannot record sound when the emu is off/sound is not enabled.", false);
            }
        }
    }
}
