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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MyNes
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if TRACE
            ConsoleTraceListener list = new ConsoleTraceListener();
            Trace.Listeners.Add(list);
#endif

            WorkingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyNes");
            ApplicationFolder = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            if (ApplicationFolder == "")
                ApplicationFolder = Path.GetFullPath(@".\");

            bool do_settings_reset = false;
            // Commandlines
            if (args != null)
            {
                foreach (string arg in args)
                {
                    if (arg == "-trace")
                    {
                        string logFileName = string.Format("{0}-master.txt",
                         DateTime.Now.ToLocalTime().ToString());
                        logFileName = logFileName.Replace(":", "");
                        logFileName = logFileName.Replace("/", "-");
                        Directory.CreateDirectory("Logs");
                        string logPath = Path.Combine(ApplicationFolder, "Logs", logFileName);
                        TextWriterTraceListener listner = new TextWriterTraceListener(HelperTools.GetFullPath(logPath));
                        Trace.Listeners.Add(listner);
                        break;
                    }
                    if (arg == "-reset")
                    {
                        do_settings_reset = true;
                        break;
                    }
                }
            }

            // Load settings
            Settings = new WINSettings(Path.Combine(WorkingFolder, "winsettings.ini"));
            Settings.LoadSettings();
            // Load controls settings
            ControlSettings = new ControlMappingSettings();
            ControlSettings.Load();
            // Detect languages and set the current selected

            if (do_settings_reset)
            {
                string pa = Path.Combine(WorkingFolder, "emusettings.ini");
                if (File.Exists(pa))
                {
                    try
                    {
                        File.Delete(pa);
                    }
                    catch
                    {
                    }
                }
                pa = Path.Combine(WorkingFolder, "renderersettings.ini");
                if (File.Exists(pa))
                {
                    try
                    {
                        File.Delete(pa);
                    }
                    catch
                    {
                    }
                }
                pa = Path.Combine(WorkingFolder, "sdlsettings.ini");
                if (File.Exists(pa))
                {
                    try
                    {
                        File.Delete(pa);
                    }
                    catch
                    {
                    }
                }
            }
            // Apply resources entries for interface language into emu core.
            ApplyLanguageToCore();

            // Load the main window
            FormMain = new FormMain(args);
            FormMain.LoadSettings();
            // Initialize the core, do it after main window because of we need the main window ready for the video provider
            MyNesMain.Initialize(false);

            if (Settings.ShowGettingStarted)
            {
                FormGettingStarted frm = new FormGettingStarted();
                frm.ShowDialog();
            }

            MyNesMain.SetVideoProvider();
            MyNesMain.SetAudioProvider();
            MyNesMain.SetRenderingMethods();

            FormMain.LoadRenderers();
            FormMain.ShowLauncher();
            FormMain.ExecuteCommandLines(args);

            // Run !
            Application.Run(FormMain);

            // Reached here means we are done
            MyNesMain.Shutdown();

            // Save settings
            Settings.SaveSettings();
            ControlSettings.Save();

            Trace.Flush();
        }

        public static string WorkingFolder { get; private set; }
        public static string ApplicationFolder { get; private set; }
        public static FormMain FormMain { get; private set; }
        public static WINSettings Settings { get; private set; }
        public static ControlMappingSettings ControlSettings { get; set; }

        // When you add new entry in the internal core interface language values, update it here.
        private static void ApplyLanguageToCore()
        {
            MNInterfaceLanguage.Message_RomInfoCanBeOnlyShown = Properties.Resources.CoreMessage_RomInfoCanBeOnlyShown;
            MNInterfaceLanguage.Message_StateSlotSetTo = Properties.Resources.CoreMessage_StateSlotSetTo;
            MNInterfaceLanguage.Message_LoadStateCanBeUsedOnly = Properties.Resources.CoreMessage_LoadStateCanBeUsedOnly;
            MNInterfaceLanguage.Message_SaveStateCanBeUseOnly = Properties.Resources.CoreMessage_SaveStateCanBeUseOnly;
            MNInterfaceLanguage.Message_HardResetCanBeUsedOnly = Properties.Resources.CoreMessage_HardResetCanBeUsedOnly;
            MNInterfaceLanguage.Message_SoftResetCanBeUsedOnly = Properties.Resources.CoreMessage_SoftResetCanBeUsedOnly;
            MNInterfaceLanguage.Message_TurboCanBeToggledOnly = Properties.Resources.CoreMessage_TurboCanBeToggledOnly;
            MNInterfaceLanguage.Message_GameGenieCanBeConfiguredOnly = Properties.Resources.CoreMessage_GameGenieCanBeConfiguredOnly;
            MNInterfaceLanguage.Message_Error1 = Properties.Resources.CoreMessage_Error1;
            MNInterfaceLanguage.Message_Error2 = Properties.Resources.CoreMessage_Error2;
            MNInterfaceLanguage.Message_Error3 = Properties.Resources.CoreMessage_Error3;
            MNInterfaceLanguage.Message_Error4 = Properties.Resources.CoreMessage_Error4;
            MNInterfaceLanguage.Message_Error5 = Properties.Resources.CoreMessage_Error5;
            MNInterfaceLanguage.Message_Error6 = Properties.Resources.CoreMessage_Error6;
            MNInterfaceLanguage.Message_Error7 = Properties.Resources.CoreMessage_Error7;
            MNInterfaceLanguage.Message_Error8 = Properties.Resources.CoreMessage_Error8;
            MNInterfaceLanguage.Message_Error9 = Properties.Resources.CoreMessage_Error9;
            MNInterfaceLanguage.Message_Error10 = Properties.Resources.CoreMessage_Error10;
            MNInterfaceLanguage.Message_Error11 = Properties.Resources.CoreMessage_Error11;
            MNInterfaceLanguage.Message_Error12 = Properties.Resources.CoreMessage_Error12;
            MNInterfaceLanguage.Message_Error13 = Properties.Resources.CoreMessage_Error13;
            MNInterfaceLanguage.Message_Error14 = Properties.Resources.CoreMessage_Error14;
            MNInterfaceLanguage.Message_Error15 = Properties.Resources.CoreMessage_Error15;
            MNInterfaceLanguage.Message_Error16 = Properties.Resources.CoreMessage_Error16;
            MNInterfaceLanguage.Message_Error17 = Properties.Resources.CoreMessage_Error17;
            MNInterfaceLanguage.Message_Info1 = Properties.Resources.CoreMessage_Info1;
            MNInterfaceLanguage.Message_Info2 = Properties.Resources.CoreMessage_Info2;
            MNInterfaceLanguage.Message_Info3 = Properties.Resources.CoreMessage_Info3;
            MNInterfaceLanguage.Message_Info4 = Properties.Resources.CoreMessage_Info4;
            MNInterfaceLanguage.Message_PleaseRestartToApply = Properties.Resources.CoreMessage_PleaseRestartToApply;
            MNInterfaceLanguage.Message_HardReset = Properties.Resources.CoreMessage_HardReset;
            MNInterfaceLanguage.Message_SoftReset = Properties.Resources.CoreMessage_SoftReset;
            MNInterfaceLanguage.Message_Paused = Properties.Resources.CoreMessage_Paused;
            MNInterfaceLanguage.Mapper = Properties.Resources.CoreMapper;

            MNInterfaceLanguage.IssueMapper5 = Properties.Resources.IssueMapper5;
            MNInterfaceLanguage.IssueMapper6 = Properties.Resources.IssueMapper6;
            MNInterfaceLanguage.IssueMapper8 = Properties.Resources.IssueMapper8;
            MNInterfaceLanguage.IssueMapper33 = Properties.Resources.IssueMapper33;
            MNInterfaceLanguage.IssueMapper44 = Properties.Resources.IssueMapper44;
            MNInterfaceLanguage.IssueMapper53 = Properties.Resources.IssueMapper53;
            MNInterfaceLanguage.IssueMapper56 = Properties.Resources.IssueMapper56;
            MNInterfaceLanguage.IssueMapper58 = Properties.Resources.IssueMapper58;
            MNInterfaceLanguage.IssueMapper60 = Properties.Resources.IssueMapper60;
            MNInterfaceLanguage.IssueMapper85 = Properties.Resources.IssueMapper85;
            MNInterfaceLanguage.IssueMapper90 = Properties.Resources.IssueMapper90;
            MNInterfaceLanguage.IssueMapper96 = Properties.Resources.IssueMapper96;
            MNInterfaceLanguage.IssueMapper105 = Properties.Resources.IssueMapper105;
            MNInterfaceLanguage.IssueMapper119 = Properties.Resources.IssueMapper119;
            MNInterfaceLanguage.IssueMapper154 = Properties.Resources.IssueMapper154;
            MNInterfaceLanguage.IssueMapper180 = Properties.Resources.IssueMapper180;
            MNInterfaceLanguage.IssueMapper191 = Properties.Resources.IssueMapper191;
            MNInterfaceLanguage.IssueMapper193 = Properties.Resources.IssueMapper193;
            MNInterfaceLanguage.IssueMapper202 = Properties.Resources.IssueMapper202;
            MNInterfaceLanguage.IssueMapper203 = Properties.Resources.IssueMapper203;
            MNInterfaceLanguage.IssueMapper207 = Properties.Resources.IssueMapper207;
            MNInterfaceLanguage.IssueMapper222 = Properties.Resources.IssueMapper222;
            MNInterfaceLanguage.IssueMapper228 = Properties.Resources.IssueMapper228;
            MNInterfaceLanguage.IssueMapper229 = Properties.Resources.IssueMapper229;
            MNInterfaceLanguage.IssueMapper230 = Properties.Resources.IssueMapper230;
            MNInterfaceLanguage.IssueMapper243 = Properties.Resources.IssueMapper243;
            MNInterfaceLanguage.IssueMapper245 = Properties.Resources.IssueMapper245;
            MNInterfaceLanguage.IssueMapper255 = Properties.Resources.IssueMapper255;
        }
    }
}
