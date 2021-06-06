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

namespace MyNes.Core
{
    internal abstract class Board
    {
        public Board()
        {
            MapperNumber = -1;
            this.PRG_RAM_08KB_DEFAULT_BLK_Count = 1;
            this.CHR_ROM_01KB_DEFAULT_BLK_Count = 8;
            LoadAttrs();
        }
        protected byte[][] PRG_RAM;// The prg RAM blocks, 4KB (0x1000) each.
        protected bool[] PRG_RAM_ENABLED;// Indicates if a block is enabled (disabled ram blocks cannot be accessed, either read nor write)
        protected bool[] PRG_RAM_WRITABLE;// Indicates if a block is writable (false means writes are not accepted even if this block is RAM)
        protected bool[] PRG_RAM_BATTERY;// Indicates if a block is battery (RAM block battery will be saved to file on emu shutdown)
        protected byte[][] PRG_ROM;// The prg ROM blocks, 8KB each.
        protected int PRG_RAM_08KB_DEFAULT_BLK_Count;
        internal int PRG_ROM_04KB_Count;
        protected int PRG_ROM_08KB_Count;
        protected int PRG_ROM_16KB_Count;
        protected int PRG_ROM_32KB_Count;
        protected int PRG_ROM_04KB_Mask;
        protected int PRG_ROM_08KB_Mask;
        protected int PRG_ROM_16KB_Mask;
        protected int PRG_ROM_32KB_Mask;
        internal int PRG_RAM_04KB_Count;
        protected int PRG_RAM_08KB_Count;
        protected int PRG_RAM_16KB_Count;
        protected int PRG_RAM_32KB_Count;
        protected int PRG_RAM_04KB_Mask;
        protected int PRG_RAM_08KB_Mask;
        protected int PRG_RAM_16KB_Mask;
        protected int PRG_RAM_32KB_Mask;

        // Starting from 4xxx to Fxxx, each entry here configure a 8kb block area
        protected bool[] PRG_AREA_BLK_RAM;// Indicates if a blk is RAM (true) or ROM (false)
        protected int[] PRG_AREA_BLK_INDEX;// The index of RAM/ROM block in the area
        protected int PRG_TMP_INDX;
        protected int PRG_TMP_AREA;

        protected byte[][] CHR_RAM;// The chr RAM blocks, 1KB (0x400) each.
        protected bool[] CHR_RAM_ENABLED;// Indicates if a block is enabled (disabled ram blocks cannot be accessed, either read nor write)
        protected bool[] CHR_RAM_WRITABLE;// Indicates if a block is writable (false means writes are not accepted even if this block is RAM)
        protected bool[] CHR_RAM_BATTERY;// Indicates if a block is battery (RAM block battery will be saved to file on emu shutdown)
        protected byte[][] CHR_ROM;// The chr ROM blocks, 8KB each.

        // Starting from 0000 to F3FF, each entry here configure a 4kb block area
        protected bool[] CHR_AREA_BLK_RAM;// Indicates if a blk is RAM (true) or ROM (false)
        protected int[] CHR_AREA_BLK_INDEX;// The index of RAM/ROM block in the area
        protected int CHR_TMP_INDX;
        protected int CHR_TMP_AREA;
        protected int CHR_ROM_01KB_DEFAULT_BLK_Count;
        internal int CHR_ROM_01KB_Count;
        protected int CHR_ROM_02KB_Count;
        protected int CHR_ROM_04KB_Count;
        protected int CHR_ROM_08KB_Count;
        internal int CHR_ROM_01KB_Mask;
        protected int CHR_ROM_02KB_Mask;
        protected int CHR_ROM_04KB_Mask;
        protected int CHR_ROM_08KB_Mask;

        internal int CHR_RAM_01KB_Count;
        protected int CHR_RAM_02KB_Count;
        protected int CHR_RAM_04KB_Count;
        protected int CHR_RAM_08KB_Count;
        internal int CHR_RAM_01KB_Mask;
        protected int CHR_RAM_02KB_Mask;
        protected int CHR_RAM_04KB_Mask;
        protected int CHR_RAM_08KB_Mask;

        protected byte[][] NMT_RAM;
        internal int[] NMT_AREA_BLK_INDEX;// The index of NMT RAM block in the area
        protected int NMT_TMP_INDX;
        protected int NMT_TMP_AREA;

        internal Mirroring NMT_DEFAULT_MIRROR;

        internal string SHA1 = "";
        internal string CRC = "";

        internal bool IsGameFoundOnDB;
        internal NesCartDatabaseGameInfo GameInfo;
        internal NesCartDatabaseCartridgeInfo GameCartInfo;
        internal string BoardType { get; private set; }
        internal string BoardPCB { get; private set; }
        internal List<string> Chips { get; private set; }
        internal bool SRAMSaveRequired;

        // Properties
        /// <summary>
        /// Get the name of this board.
        /// </summary>
        /// <value>The name.</value>
        internal string Name { get; set; }
        /// <summary>
        /// The mapper number this board presents.
        /// </summary>
        internal int MapperNumber { get; set; }
        /// <summary>
        /// Get or set if this board have issues
        /// </summary>
        internal bool HasIssues { get; set; }
        /// <summary>
        /// Get the issues (if any)
        /// </summary>
        internal virtual string Issues { get; set; }
        // Methods
        internal virtual void Initialize(IRom rom)
        {
            // DIACTIVATE sound channels just in case.
            //NesEmu.apu_mmc5_soundchannels_active = false;
            //NesEmu.apu_vrc6_soundchannels_active = false;
            //NesEmu.apu_sun_soundchannels_active = false;
            SHA1 = rom.SHA1;
            SRAMSaveRequired = false;
            IsGameGenieActive = false;
            BoardType = "N/A";
            BoardPCB = "N/A";
            this.Chips = new List<string>();
            if (NesCartDatabase.Ready)
            {
                Tracer.WriteLine("Looking for rom in the database ..");
                // Find on DB
                GameInfo = NesCartDatabase.Find(SHA1, out IsGameFoundOnDB);
                //set cart info
                if (GameInfo.Cartridges != null)
                {
                    foreach (NesCartDatabaseCartridgeInfo cartinf in GameInfo.Cartridges)
                        if (cartinf.SHA1.ToLower() == SHA1.ToLower())
                        {
                            GameCartInfo = cartinf;
                            break;
                        }
                }

                if (IsGameFoundOnDB)
                {
                    Tracer.WriteInformation("Game found in Database !!");
                    Tracer.WriteLine("> Game name: " + GameInfo.Game_Name);
                    Tracer.WriteLine("> Game alt name: " + GameInfo.Game_AltName);
                    BoardType = GameCartInfo.Board_Type;
                    Tracer.WriteLine("> Board Type: " + BoardType);
                    BoardPCB = GameCartInfo.Board_Pcb;
                    Tracer.WriteLine("> Board Pcb: " + BoardPCB);
                    // Chips ... important for some boards
                    if (GameCartInfo.chip_type != null)
                        for (int i = 0; i < GameCartInfo.chip_type.Count; i++)
                        {
                            Console.WriteLine(string.Format("> CHIP {0}: {1}", (i + 1).ToString(),
                               GameCartInfo.chip_type[i]));
                            this.Chips.Add(GameCartInfo.chip_type[i]);
                        }
                }
                else
                {
                    Tracer.WriteWarning("Game is not found in database .");
                }
            }

            Tracer.WriteLine("Initializing the board (Mapper # " + MapperNumber + ") ....");
            Tracer.WriteLine("Loading PRG ROM ...");
            #region 1 PRG ROM, load all dump
            PRG_AREA_BLK_RAM = new bool[16];
            PRG_AREA_BLK_INDEX = new int[16];
            PRG_ROM = new byte[0][];
            int index = 0;
            for (int i = 0; i < rom.PRG.Length; i += 0x1000)
            {
                Array.Resize(ref PRG_ROM, PRG_ROM.GetLength(0) + 1);
                PRG_ROM[index] = new byte[0x1000];
                for (int j = 0; j < 0x1000; j++)
                {
                    PRG_ROM[index][j] = rom.PRG[i + j];
                }
                index++;
            }
            PRG_ROM_04KB_Count = PRG_ROM.GetLength(0);
            PRG_ROM_04KB_Mask = PRG_ROM_04KB_Count - 1;
            PRG_ROM_08KB_Count = PRG_ROM_04KB_Count / 2;
            PRG_ROM_08KB_Mask = PRG_ROM_08KB_Count - 1;
            PRG_ROM_16KB_Count = PRG_ROM_04KB_Count / 4;
            PRG_ROM_16KB_Mask = PRG_ROM_16KB_Count - 1;
            PRG_ROM_32KB_Count = PRG_ROM_04KB_Count / 8;
            PRG_ROM_32KB_Mask = PRG_ROM_32KB_Count - 1;
            Tracer.WriteLine("PRG ROM loaded successfully.");
            Tracer.WriteLine("PRG ROM Size = " + PRG_ROM_04KB_Count * 4 + "KB");
            #endregion
            #region 2 PRG RAM
            // Map 16KB for now
            Tracer.WriteLine("Loading PRG RAM ...");
            SRAMBankInfo[] prg_ram_bnks = GetPRGRAM8KCountFromDB();
            // 1 calculate total ram size
            PRG_RAM = new byte[0][];

            PRG_RAM_BATTERY = new bool[0];
            PRG_RAM_ENABLED = new bool[0];
            PRG_RAM_WRITABLE = new bool[0];

            foreach (SRAMBankInfo s in prg_ram_bnks)
            {
                if (s.BATTERY)
                    SRAMSaveRequired = true;
                int kb1_count = 0;
                int.TryParse(s.SIZE.Replace("k", ""), out kb1_count);

                if (kb1_count > 0)
                {
                    int kb4_count = kb1_count / 2;
                    for (int i = 0; i < kb4_count; i++)
                    {
                        Array.Resize(ref PRG_RAM_BATTERY, PRG_RAM_BATTERY.Length + 1);
                        Array.Resize(ref PRG_RAM_ENABLED, PRG_RAM_ENABLED.Length + 1);
                        Array.Resize(ref PRG_RAM_WRITABLE, PRG_RAM_WRITABLE.Length + 1);
                        Array.Resize(ref PRG_RAM, PRG_RAM.GetLength(0) + 1);

                        PRG_RAM[PRG_RAM.GetLength(0) - 1] = new byte[0x1000];
                        PRG_RAM_BATTERY[PRG_RAM_BATTERY.Length - 1] = s.BATTERY;
                        PRG_RAM_ENABLED[PRG_RAM_ENABLED.Length - 1] = true;
                        PRG_RAM_WRITABLE[PRG_RAM_WRITABLE.Length - 1] = true;
                    }
                }
            }
            PRG_RAM_04KB_Count = PRG_RAM.GetLength(0);

            PRG_RAM_04KB_Mask = PRG_RAM_04KB_Count - 1;
            PRG_RAM_08KB_Count = PRG_RAM_04KB_Count / 2;
            PRG_RAM_08KB_Mask = PRG_RAM_08KB_Count - 1;
            PRG_RAM_16KB_Count = PRG_RAM_04KB_Count / 4;
            PRG_RAM_16KB_Mask = PRG_RAM_16KB_Count - 1;
            PRG_RAM_32KB_Count = PRG_RAM_04KB_Count / 8;
            PRG_RAM_32KB_Mask = PRG_RAM_32KB_Count - 1;


            Tracer.WriteLine("PRG RAM loaded successfully.");
            Tracer.WriteLine("PRG RAM Size = " + PRG_RAM_04KB_Count * 4 + "KB");
            // 3 TRAINER, it should be copied into the RAM blk at 0x7000. In this case, it should be copied into ram blk 3
            if (rom.HasTrainer)
            {
                rom.Trainer.CopyTo(PRG_RAM[3], 0);
            }
            Tracer.WriteLine("Loading CHR ROM ...");
            #endregion
            #region 4 CHR ROM, load all dump
            CHR_ROM = new byte[0][];
            CHR_AREA_BLK_RAM = new bool[8];
            CHR_AREA_BLK_INDEX = new int[8];
            index = 0;
            for (int i = 0; i < rom.CHR.Length; i += 0x400)
            {
                Array.Resize(ref CHR_ROM, CHR_ROM.GetLength(0) + 1);
                CHR_ROM[index] = new byte[0x400];
                for (int j = 0; j < 0x400; j++)
                {
                    CHR_ROM[index][j] = rom.CHR[i + j];
                }
                index++;
            }
            CHR_ROM_01KB_Count = CHR_ROM.GetLength(0);
            CHR_ROM_01KB_Mask = CHR_ROM_01KB_Count - 1;
            CHR_ROM_02KB_Count = CHR_ROM_01KB_Count / 2;
            CHR_ROM_02KB_Mask = CHR_ROM_02KB_Count - 1;
            CHR_ROM_04KB_Count = CHR_ROM_01KB_Count / 4;
            CHR_ROM_04KB_Mask = CHR_ROM_04KB_Count - 1;
            CHR_ROM_08KB_Count = CHR_ROM_01KB_Count / 8;
            CHR_ROM_08KB_Mask = CHR_ROM_08KB_Count - 1;
            Tracer.WriteLine("CHR ROM loaded successfully.");
            Tracer.WriteLine("CHR ROM Size = " + CHR_ROM_01KB_Count + "KB");
            #endregion

            #region 5 CHR RAM
            // Map 8 Kb for now
            Tracer.WriteLine("Loading CHR RAM ...");
            int chr_ram_banks_1k = GetCHRRAM1KCountFromDB();
            CHR_RAM = new byte[0][];
            CHR_RAM_BATTERY = new bool[chr_ram_banks_1k];
            CHR_RAM_ENABLED = new bool[chr_ram_banks_1k];
            CHR_RAM_WRITABLE = new bool[chr_ram_banks_1k];
            for (int i = 0; i < chr_ram_banks_1k; i++)
            {
                Array.Resize(ref CHR_RAM, CHR_RAM.GetLength(0) + 1);
                CHR_RAM[i] = new byte[0x400];
                CHR_RAM_BATTERY[i] = false;
                CHR_RAM_ENABLED[i] = true;
                CHR_RAM_WRITABLE[i] = true;
            }
            CHR_RAM_01KB_Count = CHR_RAM.GetLength(0);

            CHR_RAM_01KB_Mask = CHR_RAM_01KB_Count - 1;
            CHR_RAM_02KB_Count = CHR_RAM_01KB_Count / 2;
            CHR_RAM_02KB_Mask = CHR_RAM_02KB_Count - 1;
            CHR_RAM_04KB_Count = CHR_RAM_01KB_Count / 4;
            CHR_RAM_04KB_Mask = CHR_RAM_04KB_Count - 1;
            CHR_RAM_08KB_Count = CHR_RAM_01KB_Count / 8;
            CHR_RAM_08KB_Mask = CHR_RAM_08KB_Count - 1;


            Tracer.WriteLine("CHR RAM loaded successfully.");
            Tracer.WriteLine("CHR RAM Size = " + CHR_RAM_01KB_Count + "KB");
            #endregion
            // 6 Nametables
            Tracer.WriteLine("Loading Nametables ...");
            NMT_AREA_BLK_INDEX = new int[4];
            NMT_RAM = new byte[0][];
            for (int i = 0; i < 4; i++)
            {
                Array.Resize(ref NMT_RAM, NMT_RAM.GetLength(0) + 1);
                NMT_RAM[i] = new byte[0x400];
            }

            NMT_DEFAULT_MIRROR = rom.Mirroring;
            Tracer.WriteLine("Mirroring set to " + NMT_DEFAULT_MIRROR);
            Tracer.WriteLine("Board (Mapper # " + MapperNumber + ") initialized successfully.");
        }
        internal virtual void HardReset()
        {
            // Use the configuration of mapper 0 (NRAM).
            Tracer.WriteLine("Hard reset board (Mapper # " + MapperNumber + ") ....");
            // PRG Switching
            // Toggle ram/rom
            // Switch 16KB ram, from 0x4000 into 0x7000
            Tracer.WriteLine("Switching 16KB PRG RAM at 0x4000 - 0x7000");
            Toggle16KPRG_RAM(true, PRGArea.Area4000);
            Switch16KPRG(0, PRGArea.Area4000);
            // Switch 32KB rom, from 0x8000 into 0xF000
            Tracer.WriteLine("Switching 32KB PRG ROM at 0x8000 - 0xF000");
            Toggle32KPRG_RAM(false, PRGArea.Area8000);
            Switch32KPRG(0, PRGArea.Area8000);

            // CHR Switching
            // Pattern tables
            Tracer.WriteLine("Switching 8KB CHR " + (CHR_ROM_01KB_Count == 0 ? "RAM" : "ROM") + " at 0x0000 - 0x1000");
            Toggle08KCHR_RAM((CHR_ROM_01KB_Count == 0));
            Switch08KCHR(0);

            // Nametables
            Tracer.WriteLine("Switching to mirroring: " + NMT_DEFAULT_MIRROR);
            Switch01KNMTFromMirroring(NMT_DEFAULT_MIRROR);
            Tracer.WriteLine("Hard reset board (Mapper # " + MapperNumber + ") is done successfully.");
        }
        internal virtual void SoftReset() { }
        protected virtual void LoadAttrs()
        {
            this.enable_external_sound = false;
            //this.Supported = true;
            //this.NotImplementedWell = false;
            foreach (Attribute attr in Attribute.GetCustomAttributes(this.GetType()))
            {
                if (attr.GetType() == typeof(BoardInfoAttribute))
                {
                    BoardInfoAttribute inf = (BoardInfoAttribute)attr;
                    this.Name = inf.Name;
                    this.MapperNumber = inf.Mapper;
                    PRG_RAM_08KB_DEFAULT_BLK_Count = inf.DefaultPRG_RAM_8KB_BanksCount;
                    CHR_ROM_01KB_DEFAULT_BLK_Count = inf.DefaultCHR_RAM_1KB_BanksCount;
                    this.enabled_ppuA12ToggleTimer = inf.Enabled_ppuA12ToggleTimer;
                    this.ppuA12TogglesOnRaisingEdge = inf.PPUA12TogglesOnRaisingEdge;
                }
                else if (attr.GetType() == typeof(WithExternalSoundAttribute))
                {
                    this.enable_external_sound = true;
                }
                else if (attr.GetType() == typeof(HassIssuesAttribute))
                {
                    this.HasIssues = true;
                }
                //else if (attr.GetType() == typeof(NotSupported))
                //{
                //     this.Supported = false;
                //    Console.WriteLine("BOARD FLAGED 'Not Supported'");
                //}
                // else if (attr.GetType() == typeof(NotImplementedWell))
                // {
                //     NotImplementedWell inf = (NotImplementedWell)attr;
                //     this.Issues = inf.Issues;
                //     this.NotImplementedWell = true;
                //     Console.WriteLine("BOARD FLAGED 'Not Implemented Well'");
                // }
            }
        }
        protected SRAMBankInfo[] GetPRGRAM8KCountFromDB()
        {
            Tracer.WriteLine("Retrieving PRG RAM information from database ....");
            List<SRAMBankInfo> bnks = new List<SRAMBankInfo>();
            if (this.IsGameFoundOnDB)
            {
                // Get the sram size
                if (GameCartInfo.WRAMBanks.Count > 0)
                {
                    foreach (SRAMBankInfo s in GameCartInfo.WRAMBanks)
                    {
                        bnks.Add(s);
                    }
                }
                else// No info for SRAM; This mean this rom has no sram !
                {
                    Tracer.WriteLine("This game has no PRG RAM !");
                    Tracer.WriteWarning("> Adding 8K x " + PRG_RAM_08KB_DEFAULT_BLK_Count + " PRG RAM BANKS to avoid exceptions.");

                    SRAMBankInfo ii = new SRAMBankInfo(0, (PRG_RAM_08KB_DEFAULT_BLK_Count * 8) + "k", true);
                    bnks.Add(ii);
                }
            }
            else// Not in database :(
            {
                Tracer.WriteWarning("Could't find this game in database .... Adding 8K x " + PRG_RAM_08KB_DEFAULT_BLK_Count + " PRG RAM BANKS to avoid exceptions.");
                SRAMBankInfo ii = new SRAMBankInfo(0, (PRG_RAM_08KB_DEFAULT_BLK_Count * 8) + "k", true);
                bnks.Add(ii);
            }

            return bnks.ToArray();
        }
        protected int GetCHRRAM1KCountFromDB()
        {
            int count = 0;
            Tracer.WriteLine("Retrieving CHR RAM information from database ....");

            if (this.IsGameFoundOnDB)
            {
                bool ramAdded = false;
                if (GameCartInfo.VRAM_sizes != null)
                {
                    Tracer.WriteLine("Using database to initialize CHR RAM .....");
                    foreach (string vramSize in GameCartInfo.VRAM_sizes)
                    {
                        // Get the vram size
                        int vsize = 0;
                        if (int.TryParse(vramSize.Replace("k", ""), out vsize))
                        {
                            Tracer.WriteLine(">CHR RAM CHIP SIZE " + vramSize + " KB added");
                            count += vsize;
                            if (count > 0)
                            {
                                ramAdded = true;
                            }
                        }
                    }
                }
                if (!ramAdded)
                {
                    Tracer.WriteLine("Game not found in database to initialize CHR RAM; CHR RAM size set to " + CHR_ROM_01KB_DEFAULT_BLK_Count + " KB");
                    count = CHR_ROM_01KB_DEFAULT_BLK_Count;
                }
            }
            else// Not in database :(
            {
                Tracer.WriteWarning("Game not found in database to initialize CHR RAM; CHR RAM size set to " + CHR_ROM_01KB_DEFAULT_BLK_Count + " KB");
                count = CHR_ROM_01KB_DEFAULT_BLK_Count;
            }
            return count;
        }
        // CPU ACCESS AREA

        internal virtual void WriteEX(ref ushort addr, ref byte val)
        {
            PRG_TMP_AREA = addr >> 12 & 0xF;
            if (PRG_AREA_BLK_RAM[PRG_TMP_AREA])
            {
                PRG_TMP_INDX = PRG_AREA_BLK_INDEX[PRG_TMP_AREA] & PRG_RAM_04KB_Mask;

                if (PRG_RAM_ENABLED[PRG_TMP_INDX])
                    if (PRG_RAM_WRITABLE[PRG_TMP_INDX])
                        PRG_RAM[PRG_TMP_INDX][addr & 0xFFF] = val;
            }
        }
        internal virtual void WriteSRM(ref ushort addr, ref byte val)
        {
            PRG_TMP_AREA = addr >> 12 & 0xF;
            if (PRG_AREA_BLK_RAM[PRG_TMP_AREA])
            {
                PRG_TMP_INDX = PRG_AREA_BLK_INDEX[PRG_TMP_AREA] & PRG_RAM_04KB_Mask;

                if (PRG_RAM_ENABLED[PRG_TMP_INDX])
                    if (PRG_RAM_WRITABLE[PRG_TMP_INDX])
                        PRG_RAM[PRG_TMP_INDX][addr & 0xFFF] = val;
            }
        }
        internal virtual void WritePRG(ref ushort addr, ref byte val)
        {
            PRG_TMP_AREA = addr >> 12 & 0xF;
            if (PRG_AREA_BLK_RAM[PRG_TMP_AREA])
            {
                PRG_TMP_INDX = PRG_AREA_BLK_INDEX[PRG_TMP_AREA] & PRG_RAM_04KB_Mask;

                if (PRG_RAM_ENABLED[PRG_TMP_INDX])
                    if (PRG_RAM_WRITABLE[PRG_TMP_INDX])
                        PRG_RAM[PRG_TMP_INDX][addr & 0xFFF] = val;
            }
        }

        internal virtual void ReadEX(ref ushort addr, out byte val)
        {
            PRG_TMP_AREA = addr >> 12 & 0xF;
            if (PRG_AREA_BLK_RAM[PRG_TMP_AREA])
            {
                PRG_TMP_INDX = PRG_AREA_BLK_INDEX[PRG_TMP_AREA] & PRG_RAM_04KB_Mask;
                if (PRG_RAM_ENABLED[PRG_TMP_INDX])
                    val = PRG_RAM[PRG_TMP_INDX][addr & 0xFFF];
                else
                    val = 0;
            }
            else
            {
                PRG_TMP_INDX = PRG_AREA_BLK_INDEX[PRG_TMP_AREA] & PRG_ROM_04KB_Mask;
                val = PRG_ROM[PRG_TMP_INDX][addr & 0xFFF];
            }
        }
        internal virtual void ReadSRM(ref ushort addr, out byte val)
        {
            PRG_TMP_AREA = addr >> 12 & 0xF;
            if (PRG_AREA_BLK_RAM[PRG_TMP_AREA])
            {
                PRG_TMP_INDX = PRG_AREA_BLK_INDEX[PRG_TMP_AREA] & PRG_RAM_04KB_Mask;
                if (PRG_RAM_ENABLED[PRG_TMP_INDX])
                    val = PRG_RAM[PRG_TMP_INDX][addr & 0xFFF];
                else
                    val = 0;
            }
            else
            {
                PRG_TMP_INDX = PRG_AREA_BLK_INDEX[PRG_TMP_AREA] & PRG_ROM_04KB_Mask;
                val = PRG_ROM[PRG_TMP_INDX][addr & 0xFFF];
            }
        }
        internal virtual void ReadPRG(ref ushort addr, out byte val)
        {
            PRG_TMP_AREA = addr >> 12 & 0xF;
            if (PRG_AREA_BLK_RAM[PRG_TMP_AREA])
            {
                PRG_TMP_INDX = PRG_AREA_BLK_INDEX[PRG_TMP_AREA] & PRG_RAM_04KB_Mask;
                if (PRG_RAM_ENABLED[PRG_TMP_INDX])
                    val = PRG_RAM[PRG_TMP_INDX][addr & 0xFFF];
                else
                    val = 0;
            }
            else
            {
                PRG_TMP_INDX = PRG_AREA_BLK_INDEX[PRG_TMP_AREA] & PRG_ROM_04KB_Mask;
                val = PRG_ROM[PRG_TMP_INDX][addr & 0xFFF];
            }

            if (IsGameGenieActive)
            {
                foreach (GameGenieCode code in GameGenieCodes)
                {
                    if (code.Enabled)
                    {
                        if (code.Address == addr)
                        {
                            if (code.IsCompare)
                            {
                                if (code.Compare == val)
                                    val = code.Value;
                            }
                            else
                            {
                                val = code.Value;
                            }
                            break;
                        }
                    }
                }
            }
        }

        // CHR ACCESS AREA
        /// <summary>
        /// Writes at ppu memory address, only for Pattern Tables.
        /// </summary>
        /// <param name="addr">Address, ranged between $0000-1FFF.</param>
        /// <param name="val">Value.</param>
        internal virtual void WriteCHR(ref ushort addr, ref byte val)
        {
            // 00-07 means patterntables
            // 08-11 means nametables, should not included
            // 12-15 nametables mirrors, should not included as well
            CHR_TMP_AREA = (addr >> 10) & 0x7;// 0x0000 - 0x1FFF, 0-7.

            if (CHR_AREA_BLK_RAM[CHR_TMP_AREA])
            {
                CHR_TMP_INDX = CHR_AREA_BLK_INDEX[CHR_TMP_AREA] & CHR_RAM_01KB_Mask;
                if (CHR_RAM_ENABLED[CHR_TMP_INDX])
                    if (CHR_RAM_WRITABLE[CHR_TMP_INDX])
                        CHR_RAM[CHR_TMP_INDX][addr & 0x3FF] = val;
            }
        }
        /// <summary>
        /// Read from ppu memory address, including Pattern Tables, Nametables and Palette RAM.
        /// </summary>
        /// <param name="addr">Address, ranged between $0000-4FFF.</param>
        /// <param name="val">Value.</param>
        internal virtual void ReadCHR(ref ushort addr, out byte val)
        {
            // 00-07 means patterntables
            // 08-11 means nametables, should not included
            // 12-15 nametables mirrors, should not included as well
            CHR_TMP_AREA = (addr >> 10) & 0x7;// 0x0000 - 0x1FFF, 0-7.
            CHR_TMP_INDX = CHR_AREA_BLK_INDEX[CHR_TMP_AREA];
            if (CHR_AREA_BLK_RAM[CHR_TMP_AREA])
            {
                CHR_TMP_INDX &= CHR_RAM_01KB_Mask;
                if (CHR_RAM_ENABLED[CHR_TMP_INDX])
                    val = CHR_RAM[CHR_TMP_INDX][addr & 0x3FF];
                else
                    val = 0;
            }
            else
            {
                CHR_TMP_INDX &= CHR_ROM_01KB_Mask;
                val = CHR_ROM[CHR_TMP_INDX][addr & 0x3FF];
            }
        }
        internal virtual void WriteNMT(ref ushort addr, ref byte val)
        {
            NMT_TMP_AREA = (addr >> 10) & 0x3;// 0x2000 - 0x2C00, 0-3.
            NMT_TMP_INDX = NMT_AREA_BLK_INDEX[NMT_TMP_AREA];

            NMT_RAM[NMT_TMP_INDX][addr & 0x3FF] = val;
        }
        internal virtual void ReadNMT(ref ushort addr, out byte val)
        {
            NMT_TMP_AREA = (addr >> 10) & 0x3;// 0x2000 - 0x2C00, 0-3.
            NMT_TMP_INDX = NMT_AREA_BLK_INDEX[NMT_TMP_AREA];

            val = NMT_RAM[NMT_TMP_INDX][addr & 0x3FF];
        }
        // PRG Switches
        /// <summary>
        /// Switch 4KB bank at specified area
        /// </summary>
        /// <param name="index">The bank index.</param>
        /// <param name="area">Area, can be 0-11. 0 means 0x4000, 1 means 0x5000 ...etc. You can use PRGArea enum instead.</param>
        protected void Switch04KPRG(int index, PRGArea area)
        {
            PRG_AREA_BLK_INDEX[(byte)area] = index;
        }
        /// <summary>
        /// Switch 8KB bank at specified area
        /// </summary>
        /// <param name="index">The bank index.</param>
        /// <param name="area">Area, can be 0-11. 0 means 0x4000, 1 means 0x5000 ...etc. You can use PRGArea enum instead.</param>
        protected void Switch08KPRG(int index, PRGArea area)
        {
            index *= 2;
            PRG_AREA_BLK_INDEX[(byte)area] = index;
            PRG_AREA_BLK_INDEX[(byte)area + 1] = index + 1;
        }
        /// <summary>
        /// Switch 16KB bank at specified area
        /// </summary>
        /// <param name="index">The bank index.</param>
        /// <param name="area">Area, can be 0-11. 0 means 0x4000, 1 means 0x5000 ...etc. You can use PRGArea enum instead.</param>
        protected void Switch16KPRG(int index, PRGArea area)
        {
            index *= 4;
            PRG_AREA_BLK_INDEX[(byte)area] = index;
            PRG_AREA_BLK_INDEX[(byte)area + 1] = index + 1;
            PRG_AREA_BLK_INDEX[(byte)area + 2] = index + 2;
            PRG_AREA_BLK_INDEX[(byte)area + 3] = index + 3;
        }
        /// <summary>
        /// Switch 32KB bank at specified area
        /// </summary>
        /// <param name="index">The bank index.</param>
        /// <param name="area">Area, can be 0-11. 0 means 0x4000, 1 means 0x5000 ...etc. You can use PRGArea enum instead.</param>
        protected void Switch32KPRG(int index, PRGArea area)
        {
            index *= 8;
            PRG_AREA_BLK_INDEX[(byte)area] = index;
            PRG_AREA_BLK_INDEX[(byte)area + 1] = index + 1;
            PRG_AREA_BLK_INDEX[(byte)area + 2] = index + 2;
            PRG_AREA_BLK_INDEX[(byte)area + 3] = index + 3;
            PRG_AREA_BLK_INDEX[(byte)area + 4] = index + 4;
            PRG_AREA_BLK_INDEX[(byte)area + 5] = index + 5;
            PRG_AREA_BLK_INDEX[(byte)area + 6] = index + 6;
            PRG_AREA_BLK_INDEX[(byte)area + 7] = index + 7;
        }
        /// <summary>
        /// Toggle 4KB area RAM/ROM
        /// </summary>
        /// <param name="ram">If true, the area become ram, otherwise rom</param>
        /// <param name="area">Area, can be 0-11. 0 means 0x4000, 1 means 0x5000 ...etc. You can use PRGArea enum instead.</param>
        protected void Toggle04KPRG_RAM(bool ram, PRGArea area)
        {
            PRG_AREA_BLK_RAM[(byte)area] = ram;
        }
        /// <summary>
        /// Toggle 8KB area RAM/ROM
        /// </summary>
        /// <param name="ram">If true, the area become ram, otherwise rom</param>
        /// <param name="area">Area, can be 0-11. 0 means 0x4000, 1 means 0x5000 ...etc. You can use PRGArea enum instead.</param>
        protected void Toggle08KPRG_RAM(bool ram, PRGArea area)
        {
            PRG_AREA_BLK_RAM[(byte)area] = ram;
            PRG_AREA_BLK_RAM[(byte)area + 1] = ram;
        }
        /// <summary>
        /// Toggle 16KB area RAM/ROM
        /// </summary>
        /// <param name="ram">If true, the area become ram, otherwise rom</param>
        /// <param name="area">Area, can be 0-11. 0 means 0x4000, 1 means 0x5000 ...etc. You can use PRGArea enum instead.</param>
        protected void Toggle16KPRG_RAM(bool ram, PRGArea area)
        {
            PRG_AREA_BLK_RAM[(byte)area] = ram;
            PRG_AREA_BLK_RAM[(byte)area + 1] = ram;
            PRG_AREA_BLK_RAM[(byte)area + 2] = ram;
            PRG_AREA_BLK_RAM[(byte)area + 3] = ram;
        }
        /// <summary>
        /// Toggle 32KB area RAM/ROM
        /// </summary>
        /// <param name="ram">If true, the area become ram, otherwise rom</param>
        /// <param name="area">Area, can be 0-11. 0 means 0x4000, 1 means 0x5000 ...etc. You can use PRGArea enum instead.</param>
        protected void Toggle32KPRG_RAM(bool ram, PRGArea area)
        {
            PRG_AREA_BLK_RAM[(byte)area] = ram;
            PRG_AREA_BLK_RAM[(byte)area + 1] = ram;
            PRG_AREA_BLK_RAM[(byte)area + 2] = ram;
            PRG_AREA_BLK_RAM[(byte)area + 3] = ram;
            PRG_AREA_BLK_RAM[(byte)area + 4] = ram;
            PRG_AREA_BLK_RAM[(byte)area + 5] = ram;
            PRG_AREA_BLK_RAM[(byte)area + 6] = ram;
            PRG_AREA_BLK_RAM[(byte)area + 7] = ram;
        }
        protected void TogglePRGRAMEnable(bool enable)
        {
            for (int i = 0; i < PRG_RAM_ENABLED.Length; i++)
                PRG_RAM_ENABLED[i] = enable;
        }
        protected void TogglePRGRAMWritableEnable(bool enable)
        {
            for (int i = 0; i < PRG_RAM_WRITABLE.Length; i++)
                PRG_RAM_WRITABLE[i] = enable;
        }
        /// <summary>
        /// Toggle 4KB PRG RAM Enable
        /// </summary>
        /// <param name="enable">If true, the bank become enabled, otherwise disabled.</param>
        /// <param name="index">Index of the bank (NOT AREA) within the RAM BANKS COLLECTION</param>
        protected void Toggle04KPRG_RAM_Enabled(bool enable, int index)
        {
            PRG_RAM_ENABLED[index] = enable;
        }
        /// <summary>
        /// Toggle 4KB PRG RAM Writable
        /// </summary>
        /// <param name="enable">If true, the bank become writable, otherwise read-only.</param>
        /// <param name="index">Index of the bank (NOT AREA) within the RAM BANKS COLLECTION</param>
        protected void Toggle04KPRG_RAM_Writable(bool enable, int index)
        {
            PRG_RAM_WRITABLE[index] = enable;
        }
        /// <summary>
        /// Toggle 4KB PRG RAM Battery
        /// </summary>
        /// <param name="enable">If true, the bank become battery, otherwise not battery.</param>
        /// <param name="index">Index of the bank (NOT AREA) within the RAM BANKS COLLECTION</param>
        protected void Toggle04KPRG_RAM_Battery(bool enable, int index)
        {
            PRG_RAM_BATTERY[index] = enable;
        }

        // CHR Switches
        /// <summary>
        /// Switch 1KB CHR RAM/ROM at area (including Pattern Tables and Nametables)
        /// </summary>
        /// <param name="index">The bank index.</param>
        /// <param name="area">Area, can be 0-7. 1 means 0x400, 2 means 0x800 ...etc. You can use CHRArea instead.</param>
        protected void Switch01KCHR(int index, CHRArea area)
        {
            CHR_AREA_BLK_INDEX[(byte)area] = index;
        }
        /// <summary>
        /// Switch 2KB CHR RAM/ROM at area (including Pattern Tables and Nametables)
        /// </summary>
        /// <param name="index">The bank index.</param>
        /// <param name="area">Area, can be 0-7. 1 means 0x400, 2 means 0x800 ...etc. You can use CHRArea instead.</param>
        protected void Switch02KCHR(int index, CHRArea area)
        {
            index *= 2;
            CHR_AREA_BLK_INDEX[(byte)area] = index;
            CHR_AREA_BLK_INDEX[(byte)area + 1] = index + 1;
        }
        /// <summary>
        /// Switch 4KB CHR RAM/ROM at area (including Pattern Tables and Nametables)
        /// </summary>
        /// <param name="index">The bank index.</param>
        /// <param name="area">Area, can be 0-7. 1 means 0x400, 2 means 0x800 ...etc. You can use CHRArea instead.</param>
        protected void Switch04KCHR(int index, CHRArea area)
        {
            index *= 4;
            CHR_AREA_BLK_INDEX[(byte)area] = index;
            CHR_AREA_BLK_INDEX[(byte)area + 1] = index + 1;
            CHR_AREA_BLK_INDEX[(byte)area + 2] = index + 2;
            CHR_AREA_BLK_INDEX[(byte)area + 3] = index + 3;
        }
        /// <summary>
        /// Switch 8KB CHR RAM/ROM at area (including Pattern Tables and Nametables)
        /// </summary>
        /// <param name="index">The bank index.</param>
        /// <param name="area">Area, can be 0-7. 1 means 0x400, 2 means 0x800 ...etc. You can use CHRArea instead.</param>
        protected void Switch08KCHR(int index)
        {
            index *= 8;
            CHR_AREA_BLK_INDEX[0] = index;
            CHR_AREA_BLK_INDEX[1] = index + 1;
            CHR_AREA_BLK_INDEX[2] = index + 2;
            CHR_AREA_BLK_INDEX[3] = index + 3;
            CHR_AREA_BLK_INDEX[4] = index + 4;
            CHR_AREA_BLK_INDEX[5] = index + 5;
            CHR_AREA_BLK_INDEX[6] = index + 6;
            CHR_AREA_BLK_INDEX[7] = index + 7;
        }
        /// <summary>
        /// Toggle 1KB CHR RAM/ROM
        /// </summary>
        /// <param name="ram">If true, the area become ram, otherwise rom</param>
        /// <param name="area">Area, can be 0-7. 1 means 0x400, 2 means 0x800 ...etc. You can use CHRArea instead.</param>
        protected void Toggle01KCHR_RAM(bool ram, CHRArea area)
        {
            CHR_AREA_BLK_RAM[(byte)area] = ram;
        }
        /// <summary>
        /// Toggle 2KB CHR RAM/ROM
        /// </summary>
        /// <param name="ram">If true, the area become ram, otherwise rom</param>
        /// <param name="area">Area, can be 0-7. 1 means 0x400, 2 means 0x800 ...etc. You can use CHRArea instead.</param>
        protected void Toggle02KCHR_RAM(bool ram, CHRArea area)
        {
            CHR_AREA_BLK_RAM[(byte)area] = ram;
            CHR_AREA_BLK_RAM[(byte)area + 1] = ram;
        }
        /// <summary>
        /// Toggle 4KB CHR RAM/ROM
        /// </summary>
        /// <param name="ram">If true, the area become ram, otherwise rom</param>
        /// <param name="area">Area, can be 0-7. 1 means 0x400, 2 means 0x800 ...etc. You can use CHRArea instead.</param>
        protected void Toggle04KCHR_RAM(bool ram, CHRArea area)
        {
            CHR_AREA_BLK_RAM[(byte)area] = ram;
            CHR_AREA_BLK_RAM[(byte)area + 1] = ram;
            CHR_AREA_BLK_RAM[(byte)area + 2] = ram;
            CHR_AREA_BLK_RAM[(byte)area + 3] = ram;
        }
        /// <summary>
        /// Toggle 8KB CHR RAM/ROM
        /// </summary>
        /// <param name="ram">If true, the area become ram, otherwise rom</param>
        /// <param name="area">Area, can be 0-7. 1 means 0x400, 2 means 0x800 ...etc. You can use CHRArea instead.</param>
        protected void Toggle08KCHR_RAM(bool ram)
        {
            CHR_AREA_BLK_RAM[0] = ram;
            CHR_AREA_BLK_RAM[1] = ram;
            CHR_AREA_BLK_RAM[2] = ram;
            CHR_AREA_BLK_RAM[3] = ram;
            CHR_AREA_BLK_RAM[4] = ram;
            CHR_AREA_BLK_RAM[5] = ram;
            CHR_AREA_BLK_RAM[6] = ram;
            CHR_AREA_BLK_RAM[7] = ram;
        }
        /// <summary>
        /// Toggle 1KB CHR RAM Enable
        /// </summary>
        /// <param name="enable">If true, the bank become enabled, otherwise not enabled.</param>
        /// <param name="index">Index of the bank (NOT AREA) within the RAM BANKS COLLECTION</param>
        protected void Toggle01KCHR_RAM_Enabled(bool enable, int index)
        {
            CHR_RAM_ENABLED[index] = enable;
        }
        /// <summary>
        /// Toggle 1KB CHR RAM Writable
        /// </summary>
        /// <param name="enable">If true, the bank become writable, otherwise read-only.</param>
        /// <param name="index">Index of the bank (NOT AREA) within the RAM BANKS COLLECTION</param>
        protected void Toggle01KCHR_RAM_Writable(bool enable, int index)
        {
            CHR_RAM_WRITABLE[index] = enable;
        }
        protected void ToggleCHRRAMWritableEnable(bool enable)
        {
            for (int i = 0; i < CHR_RAM_WRITABLE.Length; i++)
                CHR_RAM_WRITABLE[i] = enable;
        }
        /// <summary>
        /// Toggle 1KB CHR RAM Battery
        /// </summary>
        /// <param name="enable">If true, the bank become battery, otherwise not battery.</param>
        /// <param name="index">Index of the bank (NOT AREA) within the RAM BANKS COLLECTION</param>
        protected void Toggle01KCHR_RAM_Battery(bool enable, int index)
        {
            CHR_RAM_BATTERY[index] = enable;
        }

        // NMT Switches
        /// <summary>
        /// Switch 1KB nametable bank.
        /// </summary>
        /// <param name="index">The index of the bank, can be 0, 1, 2 or 3</param>
        /// <param name="area">The area where to switch into, can be 0, 1, 2 or 3. Or you can use the NTArea enum.</param>
        protected void Switch01KNMT(int index, byte area)
        {
            NMT_AREA_BLK_INDEX[area] = index;
        }
        protected void Switch01KNMT(byte mirroring)
        {
            // Mirroring value:
            // 0000 0000
            // ddcc bbaa
            // aa: index for area 0x2000
            // bb: index for area 0x2400
            // cc: index for area 0x2800
            // dd: index for area 0x2C00
            NMT_AREA_BLK_INDEX[0] = mirroring & 0x3;
            NMT_AREA_BLK_INDEX[1] = (mirroring >> 2) & 0x3;
            NMT_AREA_BLK_INDEX[2] = (mirroring >> 4) & 0x3;
            NMT_AREA_BLK_INDEX[3] = (mirroring >> 6) & 0x3;
        }
        /// <summary>
        /// Switch 1KB nametable bank using mirroring value.
        /// </summary>
        /// <param name="mirroring">The mirroring value, 8-bit, each 2 bits represents a bank at area, starting from lower 2 bits for area A to the heighst 2 bits for area D.</param>
        protected void Switch01KNMTFromMirroring(Mirroring mirroring)
        {
            // Mirroring value:
            // 0000 0000
            // ddcc bbaa
            // aa: index for area 0x2000
            // bb: index for area 0x2400
            // cc: index for area 0x2800
            // dd: index for area 0x2C00
            NMT_AREA_BLK_INDEX[0] = (byte)mirroring & 0x3;
            NMT_AREA_BLK_INDEX[1] = ((byte)mirroring >> 2) & 0x3;
            NMT_AREA_BLK_INDEX[2] = ((byte)mirroring >> 4) & 0x3;
            NMT_AREA_BLK_INDEX[3] = ((byte)mirroring >> 6) & 0x3;
        }

        #region Clocks
        protected bool enabled_ppuA12ToggleTimer;
        protected bool ppuA12TogglesOnRaisingEdge;
        protected int old_vram_address;
        protected int new_vram_address;
        protected int ppu_cycles_timer;
        internal bool enable_external_sound;
        /// <summary>
        /// Call this on VRAM address update
        /// </summary>
        /// <param name="newAddress"></param>
        internal virtual void OnPPUAddressUpdate(ref ushort address)
        {
            if (enabled_ppuA12ToggleTimer)
            {
                old_vram_address = new_vram_address;
                new_vram_address = address & 0x1000;
                if (ppuA12TogglesOnRaisingEdge)
                {
                    if (old_vram_address < new_vram_address)
                    {
                        if (ppu_cycles_timer > 8)
                        {
                            OnPPUA12RaisingEdge();
                        }
                        ppu_cycles_timer = 0;
                    }
                }
                else
                {
                    if (old_vram_address > new_vram_address)
                    {
                        if (ppu_cycles_timer > 8)
                        {
                            OnPPUA12RaisingEdge();
                        }
                        ppu_cycles_timer = 0;
                    }
                }
            }
        }
        /// <summary>
        /// Clocked on CPU cycle clock
        /// </summary>
        internal virtual void OnCPUClock()
        {
        }
        /// <summary>
        /// Clocked on ppu clock
        /// </summary>
        internal virtual void OnPPUClock()
        {
            if (enabled_ppuA12ToggleTimer)
                ppu_cycles_timer++;
        }
        /// <summary>
        /// Clocked when the PPU A12 rasing edge occur (scanline timer, used in MMC3)
        /// </summary>
        internal virtual void OnPPUA12RaisingEdge()
        {
        }
        /// <summary>
        /// Clocked each time ppu makes a new scanline
        /// </summary>
        internal virtual void OnPPUScanlineTick()
        {
        }
        /// <summary>
        /// Clocks on APU duration clock
        /// </summary>
        internal virtual void OnAPUClockDuration()
        {

        }
        /// <summary>
        /// Clocks on APU envelope clock
        /// </summary>
        internal virtual void OnAPUClockEnvelope()
        {

        }
        /// <summary>
        /// Clocks on cpu cycle with additional info from apu
        /// </summary>
        internal virtual void OnAPUClockSingle()
        {
        }
        /// <summary>
        /// Clock on apu cycle (the other cpu cycle)
        /// </summary>
        internal virtual void OnAPUClock()
        {
        }
        /// <summary>
        /// Get the sound channels output after mix. This value will be added (+) into the nes original output mix.
        /// </summary>
        /// <returns></returns>
        internal virtual double APUGetSample()
        {
            return 0;
        }
        internal virtual void APUApplyChannelsSettings()
        {

        }
        #endregion
        #region Game Genie
        internal bool IsGameGenieActive;
        internal GameGenieCode[] GameGenieCodes;
        internal void SetupGameGenie(bool IsGameGenieActive, GameGenieCode[] GameGenieCodes)
        {
            this.IsGameGenieActive = IsGameGenieActive;
            this.GameGenieCodes = GameGenieCodes;
        }
        #endregion
        internal virtual void WriteStateData(ref System.IO.BinaryWriter bin)
        {
            for (int i = 0; i < PRG_RAM.Length; i++)
                bin.Write(PRG_RAM[i]);
            for (int i = 0; i < PRG_RAM_ENABLED.Length; i++)
                bin.Write(PRG_RAM_ENABLED[i]);
            for (int i = 0; i < PRG_RAM_WRITABLE.Length; i++)
                bin.Write(PRG_RAM_WRITABLE[i]);
            for (int i = 0; i < PRG_RAM_BATTERY.Length; i++)
                bin.Write(PRG_RAM_BATTERY[i]);
            for (int i = 0; i < PRG_AREA_BLK_RAM.Length; i++)
                bin.Write(PRG_AREA_BLK_RAM[i]);
            for (int i = 0; i < PRG_AREA_BLK_INDEX.Length; i++)
                bin.Write(PRG_AREA_BLK_INDEX[i]);
            bin.Write(PRG_TMP_INDX);
            bin.Write(PRG_TMP_AREA);
            for (int i = 0; i < CHR_RAM.Length; i++)
                bin.Write(CHR_RAM[i]);
            for (int i = 0; i < CHR_RAM_ENABLED.Length; i++)
                bin.Write(CHR_RAM_ENABLED[i]);
            for (int i = 0; i < CHR_RAM_WRITABLE.Length; i++)
                bin.Write(CHR_RAM_WRITABLE[i]);
            for (int i = 0; i < CHR_RAM_BATTERY.Length; i++)
                bin.Write(CHR_RAM_BATTERY[i]);
            for (int i = 0; i < CHR_AREA_BLK_RAM.Length; i++)
                bin.Write(CHR_AREA_BLK_RAM[i]);
            for (int i = 0; i < CHR_AREA_BLK_INDEX.Length; i++)
                bin.Write(CHR_AREA_BLK_INDEX[i]);

            bin.Write(CHR_TMP_INDX);
            bin.Write(CHR_TMP_AREA);
            for (int i = 0; i < NMT_RAM.Length; i++)
                bin.Write(NMT_RAM[i]);
            for (int i = 0; i < NMT_AREA_BLK_INDEX.Length; i++)
                bin.Write(NMT_AREA_BLK_INDEX[i]);

            bin.Write(NMT_TMP_INDX);
            bin.Write(NMT_TMP_AREA);
        }
        internal virtual void ReadStateData(ref System.IO.BinaryReader bin)
        {
            for (int i = 0; i < PRG_RAM.Length; i++)
                bin.Read(PRG_RAM[i], 0, PRG_RAM[i].Length);
            for (int i = 0; i < PRG_RAM_ENABLED.Length; i++)
                PRG_RAM_ENABLED[i] = bin.ReadBoolean();
            for (int i = 0; i < PRG_RAM_WRITABLE.Length; i++)
                PRG_RAM_WRITABLE[i] = bin.ReadBoolean();
            for (int i = 0; i < PRG_RAM_BATTERY.Length; i++)
                PRG_RAM_BATTERY[i] = bin.ReadBoolean();
            for (int i = 0; i < PRG_AREA_BLK_RAM.Length; i++)
                PRG_AREA_BLK_RAM[i] = bin.ReadBoolean();
            for (int i = 0; i < PRG_AREA_BLK_INDEX.Length; i++)
                PRG_AREA_BLK_INDEX[i] = bin.ReadInt32();
            PRG_TMP_INDX = bin.ReadInt32();
            PRG_TMP_AREA = bin.ReadInt32();

            for (int i = 0; i < CHR_RAM.Length; i++)
                bin.Read(CHR_RAM[i], 0, CHR_RAM[i].Length);
            for (int i = 0; i < CHR_RAM_ENABLED.Length; i++)
                CHR_RAM_ENABLED[i] = bin.ReadBoolean();
            for (int i = 0; i < CHR_RAM_WRITABLE.Length; i++)
                CHR_RAM_WRITABLE[i] = bin.ReadBoolean();
            for (int i = 0; i < CHR_RAM_BATTERY.Length; i++)
                CHR_RAM_BATTERY[i] = bin.ReadBoolean();
            for (int i = 0; i < CHR_AREA_BLK_RAM.Length; i++)
                CHR_AREA_BLK_RAM[i] = bin.ReadBoolean();
            for (int i = 0; i < CHR_AREA_BLK_INDEX.Length; i++)
                CHR_AREA_BLK_INDEX[i] = bin.ReadInt32();

            CHR_TMP_INDX = bin.ReadInt32();
            CHR_TMP_AREA = bin.ReadInt32();
            for (int i = 0; i < NMT_RAM.Length; i++)
                bin.Read(NMT_RAM[i], 0, NMT_RAM[i].Length);
            for (int i = 0; i < NMT_AREA_BLK_INDEX.Length; i++)
                NMT_AREA_BLK_INDEX[i] = bin.ReadInt32();

            NMT_TMP_INDX = bin.ReadInt32();
            NMT_TMP_AREA = bin.ReadInt32();
        }

        /// <summary>
        /// Save prg ram banks that marked as baterry.
        /// </summary>
        /// <param name="stream">The stream to use to save. This stream must be initialized and ready to write.</param>
        internal void SaveSRAM(Stream stream)
        {
            for (int i = 0; i < PRG_RAM_04KB_Count; i++)
            {
                if (PRG_RAM_BATTERY[i])
                {
                    stream.Write(PRG_RAM[i], 0, 0x1000);
                }
            }
        }
        /// <summary>
        /// Get prg ram banks (all togather) that marked as battery as one buffer.
        /// </summary>
        /// <returns></returns>
        internal byte[] GetSRAMBuffer()
        {
            List<byte> buffer = new List<byte>();
            for (int i = 0; i < PRG_RAM_04KB_Count; i++)
            {
                if (PRG_RAM_BATTERY[i])
                {
                    buffer.AddRange(PRG_RAM[i]);
                }
            }
            return buffer.ToArray();
        }
        /// <summary>
        /// Load prg ram banks that marked as battery.
        /// </summary>
        /// <param name="stream">The stream to use to read. This stream must be initialized and ready to read (at the offset of the s-ram).</param>
        internal void LoadSRAM(Stream stream)
        {
            for (int i = 0; i < PRG_RAM_04KB_Count; i++)
            {
                if (PRG_RAM_BATTERY[i])
                {
                    stream.Read(PRG_RAM[i], 0, 0x1000);
                }
            }
        }
        /// <summary>
        /// Load prg ram banks that marked as battery.
        /// </summary>
        /// <param name="buffer"></param>
        internal void LoadSRAM(byte[] buffer)
        {
            int o = 0;
            for (int i = 0; i < PRG_RAM_04KB_Count; i++)
            {
                if (PRG_RAM_BATTERY[i])
                {
                    for (int j = 0; j < 0x1000; j++)
                    {
                        PRG_RAM[i][j] = buffer[j + o];
                    }
                    o += 0x1000;
                }
            }
        }
    }
}
