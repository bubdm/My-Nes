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
using System.IO;
namespace MyNes.Core
{
    public class EmuSettings : ISettings
    {
        public EmuSettings(string path) : base(path)
        {
        }
        public string SnapsFolder = "Snaps";
        public string WavesFolder = "SoundRecords";
        public string SnapsFormat = ".png";
        public bool SnapsReplace = false;
        public int RegionSetting = 0;
        public string StateFolder = "States";
        public string GameGenieFolder = "GMCodes";
        public string SRAMFolder = "Srams";
        // Misc options
        public bool SaveSRAMAtEmuShutdown = true;

        public override void LoadSettings()
        {
            base.LoadSettings();

            if (MyNesMain.WorkingFolder == null)
                MyNesMain.MakeWorkingFolder();
            if (SnapsFolder == "Snaps")
                SnapsFolder = System.IO.Path.Combine(MyNesMain.WorkingFolder, "Snaps");
            if (StateFolder == "States")
                StateFolder = Path.Combine(MyNesMain.WorkingFolder, "States");
            if (GameGenieFolder == "GMCodes")
                GameGenieFolder = Path.Combine(MyNesMain.WorkingFolder, "GMCodes");
            if (SRAMFolder == "Srams")
                SRAMFolder = Path.Combine(MyNesMain.WorkingFolder, "Srams");
            if (WavesFolder == "SoundRecords")
                WavesFolder = Path.Combine(MyNesMain.WorkingFolder, "SoundRecords");
            try
            {
                System.IO.Directory.CreateDirectory(WavesFolder);
            }
            catch
            {
                Tracer.WriteError("Cannot create sound records folder !!");
            }
            try
            {
                System.IO.Directory.CreateDirectory(SnapsFolder);
            }
            catch
            {
                Tracer.WriteError("Cannot create snaps folder !!");
            }
            try
            {
                Directory.CreateDirectory(StateFolder);
            }
            catch
            {
                Tracer.WriteError("Cannot create states folder !!");
            }
            try
            {
                Directory.CreateDirectory(SRAMFolder);
            }
            catch
            {
                Tracer.WriteError("Cannot create srams folder !!");
            }
            try
            {
                Directory.CreateDirectory(GameGenieFolder);
            }
            catch
            {
                Tracer.WriteError("Cannot create game genie codes folder !!");
            }
            // Apply state settings
            StateHandler.StateFolder = StateFolder;
        }
    }
}
