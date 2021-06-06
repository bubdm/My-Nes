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
using System.Collections.Generic;
using System.IO;
using System.Xml;
namespace MyNes.Core
{
    public partial class NesEmu
    {
        private static byte[] mem_wram;
        internal static Board mem_board;

        private static MemReadAccess[] mem_read_accesses;
        private static MemWriteAccess[] mem_write_accesses;

        private static bool BUS_RW;
        private static ushort BUS_ADDRESS;
        private static string SRAMFileName;
        public static string GMFileName;

        public static GameGenieCode[] GameGenieCodes
        {
            get
            {
                if (mem_board != null)
                    return mem_board.GameGenieCodes;
                return null;
            }
        }
        public static bool IsGameGenieActive
        {
            get
            {
                if (mem_board != null)
                    return mem_board.IsGameGenieActive;
                return false;
            }
            set
            {
                if (mem_board != null)
                    mem_board.IsGameGenieActive = value;
            }
        }
        public static bool IsGameFoundOnDB
        {
            get
            {
                if (mem_board != null)
                    return mem_board.IsGameFoundOnDB;
                return false;
            }
        }
        public static NesCartDatabaseGameInfo GameInfo
        {
            get
            {
                if (mem_board != null)
                    return mem_board.GameInfo;
                return NesCartDatabaseGameInfo.Empty;
            }
        }
        public static NesCartDatabaseCartridgeInfo GameCartInfo
        {
            get
            {
                if (mem_board != null)
                    return mem_board.GameCartInfo;
                return new NesCartDatabaseCartridgeInfo();
            }
        }
        public static void SetupGameGenie(bool IsGameGenieActive, GameGenieCode[] GameGenieCodes)
        {
            if (mem_board != null)
                mem_board.SetupGameGenie(IsGameGenieActive, GameGenieCodes);
        }

        private static void MEMInitialize(IRom rom)
        {
            Tracer.WriteLine("Looking for mapper # " + rom.MapperNumber + "....");
            // Using the given rom, locate the mapper
            if (MyNesMain.IsBoardExist(rom.MapperNumber))
            {
                Tracer.WriteLine("Mapper # " + rom.MapperNumber + " located, assigning...");
                mem_board = MyNesMain.GetBoard(rom.MapperNumber);
                Tracer.WriteInformation("Mapper # " + rom.MapperNumber + " assigned successfully.");

                if (mem_board.HasIssues)
                {
                    Tracer.WriteWarning(MNInterfaceLanguage.Mapper + " # " + mem_board.MapperNumber + " [" + mem_board.Name + "] " +
                         MNInterfaceLanguage.Message_Error17);
                    MyNesMain.VideoProvider.WriteWarningNotification(MNInterfaceLanguage.Mapper + " # " + mem_board.MapperNumber + " [" + mem_board.Name + "] " +
                         MNInterfaceLanguage.Message_Error17, false);
                }
            }
            else
            {
                Tracer.WriteError("Mapper # " + rom.MapperNumber + " IS NOT LOCATED, mapper is not supported or unable to find it.");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Mapper + " # " + rom.MapperNumber + " " + MNInterfaceLanguage.Message_Error14, false);
                mem_board = MyNesMain.GetBoard(0);
                Tracer.WriteWarning("Mapper # 0 [NROM] will be used instead, assigned successfully.");
                MyNesMain.VideoProvider.WriteErrorNotification(MNInterfaceLanguage.Mapper + " # 0 [NROM] " + MNInterfaceLanguage.Message_Error15, false);
            }
            mem_read_accesses = new MemReadAccess[0xFFFF + 1];
            mem_write_accesses = new MemWriteAccess[0xFFFF + 1];

            // Map ...
            // Internal 2K Work RAM (mirrored to 800h-1FFFh)
            MEMMap(MEMReadWRAM, new ushort[] { 0x0000, 0x1000 });
            MEMMap(MEMWriteWRAM, new ushort[] { 0x0000, 0x1000 });
            // PPU IO REGISTERS, 0x2000 - 0x3FFF
            MEMMap(PPUIORead, new ushort[] { 0x2000, 0x3000 });
            MEMMap(PPUIOWrite, new ushort[] { 0x2000, 0x3000 });
            // APU IO Registers
            MEMMap(APUIORead, new ushort[] { 0x4000 });
            MEMMap(APUIOWrite, new ushort[] { 0x4000 });
            // BOARD
            //MEMMap(mem_board.ReadPRG4000, 0x4020, 0x4FFF);
            //MEMMap(mem_board.WritePRG4000, 0x4020, 0x4FFF);
            MEMMap(mem_board.ReadEX, new ushort[] { 0x5000 });
            MEMMap(mem_board.WriteEX, new ushort[] { 0x5000 });
            MEMMap(mem_board.ReadSRM, new ushort[] { 0x6000, 0x7000 });
            MEMMap(mem_board.WriteSRM, new ushort[] { 0x6000, 0x7000 });
            MEMMap(mem_board.ReadPRG, new ushort[] { 0x8000, 0x9000, 0xA000, 0xB000, 0xC000, 0xD000, 0xE000, 0xF000 });
            MEMMap(mem_board.WritePRG, new ushort[] { 0x8000, 0x9000, 0xA000, 0xB000, 0xC000, 0xD000, 0xE000, 0xF000 });
            mem_board.Initialize(rom);

            mem_wram = new byte[0x800];
        }
        private static void MEMHardReset()
        {
            mem_wram = new byte[0x800];
            mem_wram[0x0008] = 0xF7;
            mem_wram[0x0009] = 0xEF;
            mem_wram[0x000A] = 0xDF;
            mem_wram[0x000F] = 0xBF;
            Tracer.WriteLine("Reading SRAM ...");
            SRAMFileName = Path.Combine(MyNesMain.EmuSettings.SRAMFolder, Path.GetFileNameWithoutExtension(CurrentFilePath) + ".srm");
            if (File.Exists(SRAMFileName))
            {
                Stream str = new FileStream(SRAMFileName, FileMode.Open, FileAccess.Read);
                byte[] inData = new byte[str.Length];
                str.Read(inData, 0, inData.Length);
                str.Flush();
                str.Close();

                byte[] outData = new byte[0];
                ZlipWrapper.DecompressData(inData, out outData);

                mem_board.LoadSRAM(outData);

                Tracer.WriteLine("SRAM read successfully.");
            }
            else
            {
                Tracer.WriteLine("SRAM file not found; rom has no SRAM or file not exist.");
            }
            ReloadGameGenieCodes();

            mem_board.HardReset();
        }
        public static void ReloadGameGenieCodes()
        {
            Tracer.WriteLine("Reading game genie codes (if available)....");
            GMFileName = Path.Combine(MyNesMain.EmuSettings.GameGenieFolder, Path.GetFileNameWithoutExtension(CurrentFilePath) + ".txt");
            mem_board.GameGenieCodes = new GameGenieCode[0];

            if (File.Exists(GMFileName))
            {
                XmlReaderSettings sett = new XmlReaderSettings();
                sett.DtdProcessing = DtdProcessing.Ignore;
                sett.IgnoreWhitespace = true;
                XmlReader XMLread = XmlReader.Create(GMFileName, sett);
                XMLread.Read();//Reads the XML definition <XML>
                XMLread.Read();//Reads the header
                if (XMLread.Name != "MyNesGameGenieCodesList")
                {
                    XMLread.Close();
                    return;
                }
                GameGenie gameGenie = new GameGenie();
                List<GameGenieCode> codes = new List<GameGenieCode>();
                while (XMLread.Read())
                {
                    if (XMLread.Name == "Code")
                    {
                        GameGenieCode newcode = new GameGenieCode();
                        newcode.Enabled = true;

                        XMLread.MoveToAttribute("code");
                        newcode.Name = XMLread.Value.ToString();


                        if (newcode.Name.Length == 6)
                        {
                            newcode.Address = gameGenie.GetGGAddress(gameGenie.GetCodeAsHEX(newcode.Name), 6) | 0x8000;
                            newcode.Value = gameGenie.GetGGValue(gameGenie.GetCodeAsHEX(newcode.Name), 6);
                            newcode.IsCompare = false;
                        }
                        else
                        {
                            newcode.Address = gameGenie.GetGGAddress(gameGenie.GetCodeAsHEX(newcode.Name), 8) | 0x8000;
                            newcode.Value = gameGenie.GetGGValue(gameGenie.GetCodeAsHEX(newcode.Name), 8);
                            newcode.Compare = gameGenie.GetGGCompareValue(gameGenie.GetCodeAsHEX(newcode.Name));
                            newcode.IsCompare = true;
                        }
                        codes.Add(newcode);
                    }
                }
                XMLread.Close();

                if (codes.Count > 0)
                {
                    mem_board.GameGenieCodes = codes.ToArray();
                    Tracer.WriteInformation("Game Genie codes loaded successfully, total of " + codes.Count);
                }
                else
                {
                    Tracer.WriteError("There is no Game Genie code in the file to load.");
                }
            }
            else
            {
                Tracer.WriteWarning("No Game Genie file found for this game.");
            }
        }
        private static void MEMMap(MemReadAccess readAccess, ushort[] addresses)
        {
            for (int i = 0; i < addresses.Length; i++)
                mem_read_accesses[(addresses[i] & 0xF000) >> 12] = readAccess;
        }
        private static void MEMMap(MemWriteAccess writeAccess, ushort[] addresses)
        {
            for (int i = 0; i < addresses.Length; i++)
                mem_write_accesses[(addresses[i] & 0xF000) >> 12] = writeAccess;
        }
        private static void MEMReadWRAM(ref ushort addr, out byte value)
        {
            value = mem_wram[addr & 0x7FF];
        }
        private static void MEMWriteWRAM(ref ushort addr, ref byte value)
        {
            mem_wram[addr & 0x7FF] = value;
        }
        internal static void Read(ref ushort addr, out byte value)
        {
            BUS_RW = true;
            BUS_ADDRESS = addr;
            EmuClockComponents();
            mem_read_accesses[(addr & 0xF000) >> 12](ref addr, out value);
        }
        private static void Write(ref ushort addr, ref byte value)
        {
            BUS_RW = false;
            BUS_ADDRESS = addr;
            EmuClockComponents();
            mem_write_accesses[(addr & 0xF000) >> 12](ref addr, ref value);
        }
        internal static void SaveSRAM()
        {
            if (mem_board != null)
                if (MyNesMain.EmuSettings.SaveSRAMAtEmuShutdown && mem_board.SRAMSaveRequired)
                {
                    Tracer.WriteLine("Saving SRAM ...");
                    byte[] sramBuffer = new byte[0];
                    ZlipWrapper.CompressData(mem_board.GetSRAMBuffer(), out sramBuffer);

                    Stream str = new FileStream(SRAMFileName, FileMode.Create, FileAccess.Write);
                    str.Write(sramBuffer, 0, sramBuffer.Length);

                    str.Flush();
                    str.Close();
                    Tracer.WriteLine("SRAM saved successfully.");
                }
        }
        private static void MEMWriteState(ref System.IO.BinaryWriter bin)
        {
            mem_board.WriteStateData(ref bin);
            bin.Write(mem_wram);
            bin.Write(BUS_RW);
            bin.Write(BUS_ADDRESS);
        }
        private static void MEMReadState(ref System.IO.BinaryReader bin)
        {
            mem_board.ReadStateData(ref bin);
            bin.Read(mem_wram, 0, mem_wram.Length);
            BUS_RW = bin.ReadBoolean();
            BUS_ADDRESS = bin.ReadUInt16();
        }
    }
}
