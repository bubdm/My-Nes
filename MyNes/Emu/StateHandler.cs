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
using System.Text;

namespace MyNes.Core
{
    public class StateHandler
    {
        public static int Slot = 0;
        internal static string StateFolder = "States";
        private const byte state_version = 7;// The state version. NEVER CHANGE THIS !!
        private static bool IsSavingState = false;
        private static bool IsLoadingState = false;

        public static void SaveState(string fileName, bool saveImage)
        {
            if (!NesEmu.ON)
            {
                Tracer.WriteError("Can't save state, emu is off.");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Message_Error1, false);
                return;
            }
            if (!File.Exists(NesEmu.CurrentFilePath))
            {
                Tracer.WriteError("Can't save state, no rom file is loaded.");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Message_Error2, false);
                return;
            }
            if (IsLoadingState)
            {
                Tracer.WriteError("Can't save state while loading a state !");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Message_Error3, false);
                return;
            }
            if (IsSavingState)
            {
                Tracer.WriteError("Already saving state !!");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Message_Error4, false);
                return;
            }

            IsSavingState = true;
            // Create the stream
            Stream stream = new MemoryStream();
            BinaryWriter bin = new BinaryWriter(stream);
            // Write header
            bin.Write(Encoding.ASCII.GetBytes("MNS"));// Write MNS (My Nes State)
            bin.Write(state_version);// Write version (1 byte)
            // Write SHA1 for compare later
            for (int i = 0; i < NesEmu.SHA1.Length; i += 2)
            {
                string v = NesEmu.SHA1.Substring(i, 2).ToUpper();
                bin.Write(Convert.ToByte(v, 16));
            }

            // Write data using bin ...
            NesEmu.WriteStateData(ref bin);

            // Compress data !
            byte[] outData = new byte[0];
            ZlipWrapper.CompressData(((MemoryStream)bin.BaseStream).GetBuffer(), out outData);
            // Write file !
            Stream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            fileStream.Write(outData, 0, outData.Length);
            // Save snapshot
            MyNesMain.VideoProvider.TakeSnapshotAs(fileName.Replace(".mns", ".jpg"), ".jpg");

            // Finished !
            bin.Flush();
            bin.Close();
            fileStream.Flush();
            fileStream.Close();
            IsSavingState = false;

            Tracer.WriteInformation("State saved at slot " + Slot);
            MyNesMain.VideoProvider.WriteInfoNotification(MNInterfaceLanguage.Message_Info1 + " " + Slot, false);
        }
        public static void SaveState(int Slot)
        {
            if (StateFolder == "States")
                StateFolder = Path.Combine(MyNesMain.WorkingFolder, "States");
            Directory.CreateDirectory(StateFolder);

            string fileName = Path.Combine(StateFolder, Path.GetFileNameWithoutExtension(NesEmu.CurrentFilePath)) + "_" + Slot + ".mns";

            SaveState(fileName, false);
        }
        public static void SaveState()
        {
            SaveState(Slot);
        }
        public static void LoadState()
        {
            LoadState(Slot);
        }
        public static void LoadState(string fileName)
        {
            if (!NesEmu.ON)
            {
                Tracer.WriteError("Can't load state, emu is off.");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Message_Error5, false);
                return;
            }
            if (!File.Exists(NesEmu.CurrentFilePath))
            {
                Tracer.WriteError("Can't load state, no rom file is loaded.");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Message_Error6, false);
                return;
            }
            if (IsSavingState)
            {
                Tracer.WriteError("Can't load state while saving a state !");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Message_Error7, false);
                return;
            }
            if (IsLoadingState)
            {
                Tracer.WriteError("Already loading state !!");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Message_Error8, false);
                return;
            }

            if (!File.Exists(fileName))
            {
                Tracer.WriteError("No state found in slot " + Slot);
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Message_Error9 + " " + Slot, false);
                return;
            }
            IsLoadingState = true;
            // Read the file
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            // Decompress
            byte[] inData = new byte[stream.Length];
            byte[] outData = new byte[0];
            stream.Read(inData, 0, inData.Length);
            stream.Close();
            ZlipWrapper.DecompressData(inData, out outData);

            // Create the reader
            BinaryReader bin = new BinaryReader(new MemoryStream(outData));
            // Read header
            byte[] header = new byte[3];
            bin.Read(header, 0, header.Length);
            if (Encoding.ASCII.GetString(header) != "MNS")
            {
                Tracer.WriteError("Unable load state at slot " + Slot + "; Not My Nes State File !");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Message_Error10 + " " + Slot + "; " + MNInterfaceLanguage.Message_Error11, false);
                IsLoadingState = false;
                return;
            }
            // Read version
            if (bin.ReadByte() != state_version)
            {
                Tracer.WriteError("Unable load state at slot " + Slot + "; Not compatible state file version !");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Message_Error10 + " " + Slot + "; " + MNInterfaceLanguage.Message_Error12, false);
                IsLoadingState = false;
                return;
            }
            string sha1 = "";
            for (int i = 0; i < NesEmu.SHA1.Length; i += 2)
            {
                sha1 += (bin.ReadByte()).ToString("X2");
            }
            if (sha1.ToLower() != NesEmu.SHA1.ToLower())
            {
                Tracer.WriteError("Unable load state at slot " + Slot + "; This state file is not for this game; not same SHA1 !");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Message_Error10 + " " + Slot + "; " + MNInterfaceLanguage.Message_Error13, false);
                IsLoadingState = false;
                return;
            }
            // Read data using bin ....
            NesEmu.ReadStateData(ref bin);

            // Finished !
            bin.Close();
            IsLoadingState = false;
            Tracer.WriteInformation("State loaded from slot " + Slot);
            MyNesMain.VideoProvider.WriteInfoNotification(MNInterfaceLanguage.Message_Info2 + " " + Slot, false);
        }
        public static void LoadState(int Slot)
        {
            if (StateFolder == "States")
                StateFolder = Path.Combine(MyNesMain.WorkingFolder, "States");
            Directory.CreateDirectory(StateFolder);

            string fileName = Path.Combine(StateFolder, Path.GetFileNameWithoutExtension(NesEmu.CurrentFilePath)) + "_" + Slot + ".mns";

            LoadState(fileName);
        }
        public static string GetStateFile(int slot)
        {
            if (File.Exists(NesEmu.CurrentFilePath))
                return Path.Combine(StateFolder, Path.GetFileNameWithoutExtension(NesEmu.CurrentFilePath)) + "_" + slot + ".mns";
            else
                return "";
        }
        public static string GetStateImageFile(int slot)
        {
            if (File.Exists(NesEmu.CurrentFilePath))
                return Path.Combine(StateFolder, Path.GetFileNameWithoutExtension(NesEmu.CurrentFilePath)) + "_" + slot + ".jpg";
            else
                return "";
        }
    }
}
