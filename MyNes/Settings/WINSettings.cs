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

namespace MyNes
{
    class WINSettings : ISettings
    {
        public WINSettings(string path) : base(path)
        {

        }
        // Application
        public string App_Version = "";

        // Window
        public int Win_Location_X = 10;
        public int Win_Location_Y = 10;
        public int Win_Size_W = 768;
        public int Win_Size_H = 743;
        public bool Win_StartInFullscreen = false;
        // Misc
        public string[] Misc_RecentFiles = new string[0];
        public bool PauseEmuWhenFocusLost = true;
        public bool ShowGettingStarted = true;
        public string InterfaceLanguage = "English";
        public bool ShutdowOnEscapePress = true;
        public bool LoadStateOpenRecent = false;
        // Database
        public string Database_FilePath = "";
        public string[] Database_FoldersSnapshots;
        public string[] Database_FoldersCovers;
        public string[] Database_FoldersInfos;
        public string[] Database_FoldersScanned;
        public bool LauncherRememberLastSelection = true;
        public int LauncherLatestSelection = 0;
        public int LauncherLocationX = 10;
        public int LauncherLocationY = 10;
        public int LauncherSizeW = 1480;
        public int LauncherSizeH = 920;
        public int LauncherSpliter1 = 807;
        public int LauncherSpliter2 = 420;
        public int LauncherSpliter3 = 308;
        public int LauncherSpliter4 = 271;
        public bool LauncherAutoMinimize = true;
        public bool LauncherAutoCycleImagesInGameTab = true;
        public bool LauncherShowAyAppStart = false;
        public int SnapsView_ImageMode = 1;
        public bool SnapsView_ShowBar = true;
        public bool SnapsView_ShowStatus = true;
        public bool SnapsView_AutoCycle = true;
        public int CoversView_ImageMode = 1;
        public bool CoversView_ShowBar = true;
        public bool CoversView_ShowStatus = true;
        public bool CoversView_AutoCycle = true;
        public override void LoadSettings()
        {
            base.LoadSettings();

            if (App_Version != System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())
            {
                ShowGettingStarted = true;
                App_Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
    }
}
