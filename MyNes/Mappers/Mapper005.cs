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
namespace MyNes.Core
{
    [BoardInfo("MMC5", 5, 8, 16)]
    [WithExternalSound]
    [HassIssues]
    class Mapper005 : Board
    {
        // TODO: split screen and Uchuu Keibitai SDF chr corruption for unknown reason in the intro (not in the split screen).
        private int ram_protectA;
        private int ram_protectB;
        private int ExRAM_mode;
        private int[] CHROffset_spr;// The indexes to use with spr in spr fetches.
        private int[] CHROffsetEX;// For extra attr
        private int[] CHROffsetSP;// For split screen
        private int[] chrRegA;
        private int[] chrRegB;

        private int[] prgReg;
        private bool useSRAMmirroring;
        private int chr_high;
        private int chr_mode;
        private int prg_mode;
        private bool chr_setB_last;
        private byte temp_val;
        private byte temp_fill;
        private int lastAccessVRAM;
        private int paletteNo;
        private int shift;
        private int EXtilenumber;
        private byte multiplicand;
        private byte multiplier;
        private ushort product;
        private bool split_enable;
        private bool split_right;
        private int split_tile;
        private int split_yscroll;// The y scroll value for split.
        private bool split_doit;// Set to true to make nt changes; get split happening. Used in CHR read access.
        private int split_watch_tile;// A temp tile number, for the split.
        private byte irq_line;
        private byte irq_enable;
        private int irq_pending;
        private int irq_current_counter;
        private int irq_current_inframe;
        // External sound channels
        private MMC5Sqr snd_1;
        private MMC5Sqr snd_2;
        private MMC5Pcm snd_3;
        private double[][][][][] mix_table;

        internal override string Issues
        { get { return MNInterfaceLanguage.IssueMapper5; } }

        internal override void Initialize(IRom rom)
        {
            base.Initialize(rom);
            snd_1 = new MMC5Sqr();
            snd_2 = new MMC5Sqr();
            snd_3 = new MMC5Pcm();
            mix_table = new double[16][][][][];

            for (int sq1 = 0; sq1 < 16; sq1++)
            {
                mix_table[sq1] = new double[16][][][];

                for (int sq2 = 0; sq2 < 16; sq2++)
                {
                    mix_table[sq1][sq2] = new double[16][][];

                    for (int tri = 0; tri < 16; tri++)
                    {
                        mix_table[sq1][sq2][tri] = new double[16][];

                        for (int noi = 0; noi < 16; noi++)
                        {
                            mix_table[sq1][sq2][tri][noi] = new double[256];

                            for (int dmc = 0; dmc < 256; dmc++)
                            {
                                var sqr = (95.88 / (8128.0 / (sq1 + sq2) + 100));
                                var tnd = (159.79 / (1.0 / (tri / 8227.0 + noi / 12241.0 + dmc / 22638.0) + 100));

                                mix_table[sq1][sq2][tri][noi][dmc] = sqr + tnd;
                            }
                        }
                    }
                }
            }
        }
        internal override void HardReset()
        {
            base.HardReset();
            // This is not a hack, "Uncharted Waters" title actually use 2 chips of SRAM which depends on bit 2 of
            // $5113 register instead of first 2 bits for switching.
            // There's no other way to work around this.
            switch (SHA1.ToUpper())
            {
                // Uncharted Waters
                case "37267833C984F176DB4B0BC9D45DABA0FFF45304": useSRAMmirroring = true; break;
                // Daikoukai Jidai (J)
                case "800AEFE756E85A0A78CCB4DAE68EBBA5DF24BF41": useSRAMmirroring = true; break;
                    // L'Empereur
                    //case "6197D576DD1C2A2304BE82B7BE6768A13C40BCf9": useSRAMmirroring = true; break;
            }
            System.Console.WriteLine("MMC5: using PRG RAM mirroring = " + useSRAMmirroring);

            CHROffset_spr = new int[8];
            CHROffsetEX = new int[8];
            CHROffsetSP = new int[8];
            chrRegA = new int[8];
            chrRegB = new int[4];
            prgReg = new int[4];

            prgReg[3] = PRG_ROM_08KB_Mask;
            prg_mode = 3;
            base.Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.Area8000);
            base.Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaA000);
            base.Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaC000);
            base.Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);

            Switch04kCHREX(0, 0x0000);
            Switch04kCHRSP(0, 0x0000);
            Switch08kCHR_spr(0);

            TogglePRGRAMWritableEnable(true);
            TogglePRGRAMEnable(true);

            APUApplyChannelsSettings();
            snd_1.HardReset();
            snd_2.HardReset();
            snd_3.HardReset();
        }
        internal override void SoftReset()
        {
            base.SoftReset();
            snd_1.SoftReset();
            snd_2.SoftReset();
            snd_3.SoftReset();
        }
        // All registers writes here
        internal override void WriteEX(ref ushort address, ref byte value)
        {
            if (address >= 0x5C00)
            {
                if (ExRAM_mode == 2)// Only EX2 is writable.
                    base.NMT_RAM[2][address & 0x3FF] = value;
                else if (ExRAM_mode < 2)
                {
                    if (irq_current_inframe == 0x40)
                        base.NMT_RAM[2][address & 0x3FF] = value;
                    else
                        base.NMT_RAM[2][address & 0x3FF] = 0;
                }
                return;
            }
            switch (address)
            {
                #region Sound Channels
                case 0x5000: snd_1.Write0(ref value); break;
                case 0x5002: snd_1.Write2(ref value); break;
                case 0x5003: snd_1.Write3(ref value); break;
                case 0x5004: snd_2.Write0(ref value); break;
                case 0x5006: snd_2.Write2(ref value); break;
                case 0x5007: snd_2.Write3(ref value); break;
                case 0x5010: snd_3.Write5010(value); break;
                case 0x5011: snd_3.Write5011(value); break;
                case 0x5015:
                    {
                        snd_1.WriteEnabled((value & 0x1) != 0);
                        snd_2.WriteEnabled((value & 0x2) != 0);
                        break;
                    }
                #endregion
                case 0x5100: prg_mode = value & 0x3; break;
                case 0x5101: chr_mode = value & 0x3; break;
                case 0x5102: ram_protectA = value & 0x3; UpdateRamProtect(); break;
                case 0x5103: ram_protectB = value & 0x3; UpdateRamProtect(); break;
                case 0x5104: ExRAM_mode = value & 0x3; break;
                case 0x5105: Switch01KNMT(value); break;
                #region PRG
                case 0x5113:
                    {
                        if (!useSRAMmirroring)
                            base.Switch08KPRG(value & 0x7, PRGArea.Area6000);
                        else// Use chip switching (bit 2)...
                            base.Switch08KPRG((value >> 2) & 1, PRGArea.Area6000);
                        break;
                    }
                case 0x5114:
                    {
                        if (prg_mode == 3)
                        {
                            Toggle08KPRG_RAM((value & 0x80) == 0, PRGArea.Area8000);
                            base.Switch08KPRG(value & 0x7F, PRGArea.Area8000);
                        }
                        break;
                    }
                case 0x5115:
                    {
                        switch (prg_mode)
                        {
                            case 1: Toggle16KPRG_RAM((value & 0x80) == 0, PRGArea.Area8000); base.Switch16KPRG((value & 0x7F) >> 1, PRGArea.Area8000); break;
                            case 2: Toggle16KPRG_RAM((value & 0x80) == 0, PRGArea.Area8000); base.Switch16KPRG((value & 0x7F) >> 1, PRGArea.Area8000); break;
                            case 3: Toggle08KPRG_RAM((value & 0x80) == 0, PRGArea.AreaA000); base.Switch08KPRG(value & 0x7F, PRGArea.AreaA000); break;
                        }
                        break;
                    }
                case 0x5116:
                    {
                        switch (prg_mode)
                        {
                            case 2:
                            case 3: Toggle08KPRG_RAM((value & 0x80) == 0, PRGArea.AreaC000); base.Switch08KPRG(value & 0x7F, PRGArea.AreaC000); break;
                        }
                        break;
                    }
                case 0x5117:
                    {
                        switch (prg_mode)
                        {
                            case 0: base.Switch32KPRG((value & 0x7C) >> 2, PRGArea.Area8000); break;
                            case 1: base.Switch16KPRG((value & 0x7F) >> 1, PRGArea.AreaC000); break;
                            case 2: base.Switch08KPRG(value & 0x7F, PRGArea.AreaE000); break;
                            case 3: base.Switch08KPRG(value & 0x7F, PRGArea.AreaE000); break;
                        }
                        break;
                    }
                #endregion
                #region CHR
                // SPR SET
                case 0x5120:
                    {
                        chr_setB_last = false;
                        if (chr_mode == 3)
                            Switch01kCHR_spr(value | chr_high, 0x0000);
                        break;
                    }
                case 0x5121:
                    {
                        chr_setB_last = false;
                        switch (chr_mode)
                        {
                            case 2: Switch02kCHR_spr(value | chr_high, 0x0000); break;
                            case 3: Switch01kCHR_spr(value | chr_high, 0x0400); break;
                        }
                        break;
                    }
                case 0x5122:
                    {
                        chr_setB_last = false;
                        if (chr_mode == 3)
                            Switch01kCHR_spr(value | chr_high, 0x0800);
                        break;
                    }
                case 0x5123:
                    {
                        chr_setB_last = false;
                        switch (chr_mode)
                        {
                            case 1: Switch04kCHR_spr(value | chr_high, 0x0000); break;
                            case 2: Switch02kCHR_spr(value | chr_high, 0x0800); break;
                            case 3: Switch01kCHR_spr(value | chr_high, 0x0C00); break;
                        }
                        break;
                    }
                case 0x5124:
                    {
                        chr_setB_last = false;
                        if (chr_mode == 3)
                            Switch01kCHR_spr(value | chr_high, 0x1000);
                        break;
                    }
                case 0x5125:
                    {
                        chr_setB_last = false;
                        switch (chr_mode)
                        {
                            case 2: Switch02kCHR_spr(value | chr_high, 0x1000); break;
                            case 3: Switch01kCHR_spr(value | chr_high, 0x1400); break;
                        }
                        break;
                    }
                case 0x5126:
                    {
                        chr_setB_last = false;
                        if (chr_mode == 3)
                            Switch01kCHR_spr(value | chr_high, 0x1800);
                        break;
                    }
                case 0x5127:
                    {
                        chr_setB_last = false;
                        switch (chr_mode)
                        {
                            case 0: Switch08kCHR_spr(value | chr_high); break;
                            case 1: Switch04kCHR_spr(value | chr_high, 0x1000); break;
                            case 2: Switch02kCHR_spr(value | chr_high, 0x1800); break;
                            case 3: Switch01kCHR_spr(value | chr_high, 0x1C00); break;
                        }
                        break;
                    }
                // BKG SET
                case 0x5128:
                    {
                        chr_setB_last = true;
                        if (chr_mode == 3)
                        {
                            Switch01KCHR(value | chr_high, CHRArea.Area0000);
                            Switch01KCHR(value | chr_high, CHRArea.Area1000);
                        }
                        break;
                    }
                case 0x5129:
                    {
                        chr_setB_last = true;
                        switch (chr_mode)
                        {
                            case 2:
                                {
                                    Switch02KCHR(value | chr_high, CHRArea.Area0000);
                                    Switch02KCHR(value | chr_high, CHRArea.Area1000);
                                    break;
                                }
                            case 3:
                                {
                                    Switch01KCHR(value | chr_high, CHRArea.Area0400);
                                    Switch01KCHR(value | chr_high, CHRArea.Area1400);
                                    break;
                                }
                        }
                        break;
                    }
                case 0x512A:
                    {
                        chr_setB_last = true;
                        if (chr_mode == 3)
                        {
                            Switch01KCHR(value | chr_high, CHRArea.Area0800);
                            Switch01KCHR(value | chr_high, CHRArea.Area1800);
                        }
                        break;
                    }
                case 0x512B:
                    {
                        chr_setB_last = true;
                        switch (chr_mode)
                        {
                            case 0:
                                {
                                    Switch04kCHR_bkg((value | chr_high), 0x0000);
                                    Switch04kCHR_bkg((value | chr_high), 0x1000);
                                    break;
                                }
                            case 1:
                                {
                                    Switch04KCHR(value | chr_high, CHRArea.Area0000);
                                    Switch04KCHR(value | chr_high, CHRArea.Area1000);
                                    break;
                                }
                            case 2:
                                {
                                    Switch02KCHR(value | chr_high, CHRArea.Area0800);
                                    Switch02KCHR(value | chr_high, CHRArea.Area1800);
                                    break;
                                }
                            case 3:
                                {
                                    Switch01KCHR(value | chr_high, CHRArea.Area0C00);
                                    Switch01KCHR(value | chr_high, CHRArea.Area1C00);
                                    break;
                                }
                        }
                        break;
                    }
                case 0x5130:
                    {
                        chr_high = (value & 0x3) << 8;
                        break;
                    }
                #endregion
                //Fill-mode tile
                case 0x5106:
                    for (int i = 0; i < 0x3C0; i++)
                        base.NMT_RAM[3][i] = value;
                    break;
                //Fill-mode attr
                case 0x5107:
                    for (int i = 0x3C0; i < (0x3C0 + 0x40); i++)
                    {
                        temp_fill = (byte)((2 << (value & 0x03)) | (value & 0x03));
                        temp_fill |= (byte)((temp_fill & 0x0F) << 4);
                        base.NMT_RAM[3][i] = temp_fill;
                    }
                    break;
                case 0x5200:
                    {
                        split_tile = value & 0x1F;
                        split_enable = (value & 0x80) == 0x80;
                        split_right = (value & 0x40) == 0x40;
                        break;
                    }
                case 0x5201:
                    {
                        split_yscroll = value;
                        break;
                    }
                case 0x5202:
                    {
                        Switch04kCHRSP(value, address & 0x0000);
                        Switch04kCHRSP(value, address & 0x1000);
                        break;
                    }
                case 0x5203: irq_line = value; break;
                case 0x5204: irq_enable = value; break;
                case 0x5205: multiplicand = value; product = (ushort)(multiplicand * multiplier); break;
                case 0x5206: multiplier = value; product = (ushort)(multiplicand * multiplier); break;
            }
        }
        internal override void ReadEX(ref ushort address, out byte data)
        {
            if (address >= 0x5C00)
            {
                if (ExRAM_mode >= 2)
                {
                    data = base.NMT_RAM[2][address & 0x3FF];
                    return;
                }
            }
            switch (address)
            {
                case 0x5010: data = snd_3.Read5010(); break;
                case 0x5204:
                    {
                        data = (byte)(irq_current_inframe | irq_pending);
                        irq_pending = 0;
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0x5205: data = (byte)(product & 0xFF); break;
                case 0x5206: data = (byte)((product & 0xFF00) >> 8); break;
                case 0x5015:
                    {
                        data = (byte)((snd_1.ReadEnable() ? 0x1 : 0) | (snd_2.ReadEnable() ? 0x2 : 0));
                        data = 0;
                        break;
                    }
                default: data = 0; break;
            }
        }
        internal override void ReadCHR(ref ushort address, out byte data)
        {
            if (!NesEmu.ppu_is_sprfetch)
            {
                if (split_enable)
                {
                    if (ExRAM_mode < 2)// Split screen mode is only allowed in Ex0 or Ex1
                    {
                        split_watch_tile = address & 0x3FF / 16;
                        if (!split_right)// Left
                            split_doit = split_watch_tile < split_tile;// Tiles 0 to T-1 are the split.
                        else// Right
                            split_doit = split_watch_tile >= split_tile;// Tiles 0 to T-1 are rendered normally. Tiles T and on are the split.
                        if (split_doit)
                        {
                            //  return CHR[((address & 0x03FF) | CHROffsetSP[address >> 10 & 0x07]) & CHRMaxSizeInBytesMask];
                        }
                    }
                }
            }
            if (ExRAM_mode == 1)// Extended Attribute mode
            {
                if (!NesEmu.ppu_is_sprfetch)
                {
                    EXtilenumber = base.NMT_RAM[2][lastAccessVRAM] & 0x3F;
                    Switch04kCHREX(EXtilenumber | chr_high, address & 0x1000);
                    data = base.CHR_ROM[CHROffsetEX[(address >> 10) & 0x7]][address & 0x03FF];
                }
                else// Sprites not effected
                {
                    data = base.CHR_ROM[CHROffset_spr[(address >> 10) & 0x7]][address & 0x03FF];
                }
            }
            else
            {
                if (NesEmu.ppu_reg_2000_Sprite_size == 0x10)
                {
                    // When in 8x16 sprite mode, both sets of registers are used. 
                    // The 'A' set is used for sprite tiles, and the 'B' set is used for BG.
                    if (!NesEmu.ppu_is_sprfetch)
                        data = base.CHR_ROM[base.CHR_AREA_BLK_INDEX[(address >> 10) & 0x7]][address & 0x03FF];
                    else
                        data = base.CHR_ROM[CHROffset_spr[(address >> 10) & 0x7]][address & 0x03FF];
                }
                else
                {
                    // When in 8x8 sprite mode, only one set is used for both BG and sprites.  
                    // Either 'A' or 'B', depending on which set is written to last
                    if (chr_setB_last)
                        data = base.CHR_ROM[base.CHR_AREA_BLK_INDEX[(address >> 10) & 0x7]][address & 0x03FF];
                    else
                        data = base.CHR_ROM[CHROffset_spr[(address >> 10) & 0x7]][address & 0x03FF];
                }
            }
        }
        internal override void ReadNMT(ref ushort address, out byte data)
        {
            /*
             *  %00 = Extra Nametable mode    ("Ex0")
                %01 = Extended Attribute mode ("Ex1")
                %10 = CPU access mode         ("Ex2")
                %11 = CPU read-only mode      ("Ex3")

             * NT Values can be the following:
                  %00 = NES internal NTA
                  %01 = NES internal NTB
                  %10 = use ExRAM as NT
                  %11 = Fill Mode
             */
            if (split_doit)
            {
                // ExRAM is always used as the nametable in split screen mode.
                // return base.NMT[2][address & 0x03FF];
            }
            if (ExRAM_mode == 1)// Extended Attribute mode
            {
                if ((address & 0x03FF) <= 0x3BF)
                {
                    lastAccessVRAM = address & 0x03FF;
                }
                else
                {
                    paletteNo = base.NMT_RAM[2][lastAccessVRAM] & 0xC0;
                    // Fix Attribute bits
                    shift = ((lastAccessVRAM >> 4 & 0x04) | (lastAccessVRAM & 0x02));
                    switch (shift)
                    {
                        case 0: data = (byte)(paletteNo >> 6); return;
                        case 2: data = (byte)(paletteNo >> 4); return;
                        case 4: data = (byte)(paletteNo >> 2); return;
                        case 6: data = (byte)(paletteNo >> 0); return;
                    }
                }
            }
            data = base.NMT_RAM[base.NMT_AREA_BLK_INDEX[(address >> 10) & 0x03]][address & 0x03FF];// Reached here in some cases above.
        }
        internal override void WriteNMT(ref ushort address, ref byte value)
        {
            if (ExRAM_mode == 1)
            {
                if ((address & 0x03FF) <= 0x3BF)
                {
                    lastAccessVRAM = address & 0x03FF;
                }
            }
            base.NMT_RAM[base.NMT_AREA_BLK_INDEX[(address >> 10) & 0x03]][address & 0x03FF] = value;
        }

        private void UpdateRamProtect()
        {
            TogglePRGRAMWritableEnable((ram_protectA == 0x2) && (ram_protectB == 0x1));
        }
        private void Switch04kCHR_bkg(int index, int where)
        {
            int area = (where >> 10) & 0x07;
            index <<= 2;

            CHR_AREA_BLK_INDEX[area] = (index & CHR_ROM_01KB_Mask);
            area++;
            index++;
            CHR_AREA_BLK_INDEX[area] = (index & CHR_ROM_01KB_Mask);
            area++;
            index++;
            CHR_AREA_BLK_INDEX[area] = (index & CHR_ROM_01KB_Mask);
            area++;
            index++;
            CHR_AREA_BLK_INDEX[area] = (index & CHR_ROM_01KB_Mask);
        }
        private void Switch01kCHR_spr(int index, int where)
        {
            CHROffset_spr[(where >> 10) & 0x07] = (index & CHR_ROM_01KB_Mask);
        }
        private void Switch02kCHR_spr(int index, int where)
        {
            int area = (where >> 10) & 0x07;
            index <<= 1;

            CHROffset_spr[area] = (index & CHR_ROM_01KB_Mask); index++;
            CHROffset_spr[area + 1] = (index & CHR_ROM_01KB_Mask);
        }
        private void Switch04kCHR_spr(int index, int where)
        {
            int area = (where >> 10) & 0x07;
            index <<= 2;

            CHROffset_spr[area] = (index & CHR_ROM_01KB_Mask);
            area++;
            index++;
            CHROffset_spr[area] = (index & CHR_ROM_01KB_Mask);
            area++;
            index++;
            CHROffset_spr[area] = (index & CHR_ROM_01KB_Mask);
            area++;
            index++;
            CHROffset_spr[area] = (index & CHR_ROM_01KB_Mask);
        }
        private void Switch08kCHR_spr(int index)
        {
            index <<= 3;
            CHROffset_spr[0] = (index & CHR_ROM_01KB_Mask);
            index++;
            CHROffset_spr[1] = (index & CHR_ROM_01KB_Mask);
            index++;
            CHROffset_spr[2] = (index & CHR_ROM_01KB_Mask);
            index++;
            CHROffset_spr[3] = (index & CHR_ROM_01KB_Mask);
            index++;
            CHROffset_spr[4] = (index & CHR_ROM_01KB_Mask);
            index++;
            CHROffset_spr[5] = (index & CHR_ROM_01KB_Mask);
            index++;
            CHROffset_spr[6] = (index & CHR_ROM_01KB_Mask);
            index++;
            CHROffset_spr[7] = (index & CHR_ROM_01KB_Mask);
        }
        private void Switch04kCHREX(int index, int where)
        {
            int area = (where >> 10) & 0x07;
            index <<= 2;

            CHROffsetEX[area] = (index & CHR_ROM_01KB_Mask);
            area++;
            index++;
            CHROffsetEX[area] = (index & CHR_ROM_01KB_Mask);
            area++;
            index++;
            CHROffsetEX[area] = (index & CHR_ROM_01KB_Mask);
            area++;
            index++;
            CHROffsetEX[area] = (index & CHR_ROM_01KB_Mask);
        }
        private void Switch04kCHRSP(int index, int where)
        {
            int area = (where >> 10) & 0x07;
            index <<= 2;

            CHROffsetSP[area] = (index & CHR_ROM_01KB_Mask);
            area++;
            index++;
            CHROffsetSP[area] = (index & CHR_ROM_01KB_Mask);
            area++;
            index++;
            CHROffsetSP[area] = (index & CHR_ROM_01KB_Mask);
            area++;
            index++;
            CHROffsetSP[area] = (index & CHR_ROM_01KB_Mask);
        }

        // IRQ
        internal override void OnPPUScanlineTick()
        {
            // In frame signal
            irq_current_inframe = (NesEmu.IsInRender() && NesEmu.IsRenderingOn()) ? 0x40 : 0x00;
            if (irq_current_inframe == 0)
            {
                irq_current_inframe = 0x40;
                irq_current_counter = 0;
                irq_pending = 0;
                NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
            }
            else
            {
                irq_current_counter++;
                if (irq_current_counter == irq_line)
                {
                    irq_pending = 0x80;// IRQ pending flag is raised *regardless* of whether or not IRQs are enabled.
                    if (irq_enable == 0x80)// Trigger an IRQ on the 6502 if both this flag *and* the IRQ enable flag is set.
                        NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
        }

        // Sound clocks
        internal override void OnAPUClock()
        {
            base.OnAPUClock();
            snd_1.Clock();
            snd_2.Clock();
        }
        internal override void OnAPUClockEnvelope()
        {
            base.OnAPUClockEnvelope();
            // Both envelopes and lengths clock on envelope rate 240Hz
            snd_1.ClockLength();
            snd_2.ClockLength();
            snd_1.ClockEnvelope();
            snd_2.ClockEnvelope();
        }
        internal override double APUGetSample()
        {
            return mix_table[snd_1.output][snd_2.output][0][0][snd_3.output];
        }
        internal override void APUApplyChannelsSettings()
        {
            base.APUApplyChannelsSettings();
            snd_1.Outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_MMC5_SQ1;
            snd_2.Outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_MMC5_SQ2;
            snd_3.Outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_MMC5_PCM;
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(ram_protectA);
            stream.Write(ram_protectB);
            stream.Write(ExRAM_mode);
            for (int i = 0; i < CHROffset_spr.Length; i++)
                stream.Write(CHROffset_spr[i]);
            for (int i = 0; i < CHROffsetEX.Length; i++)
                stream.Write(CHROffsetEX[i]);
            for (int i = 0; i < CHROffsetSP.Length; i++)
                stream.Write(CHROffsetSP[i]);
            for (int i = 0; i < chrRegA.Length; i++)
                stream.Write(chrRegA[i]);
            for (int i = 0; i < chrRegB.Length; i++)
                stream.Write(chrRegB[i]);
            for (int i = 0; i < prgReg.Length; i++)
                stream.Write(prgReg[i]);
            stream.Write(useSRAMmirroring);
            stream.Write(chr_high);
            stream.Write(chr_mode);
            stream.Write(prg_mode);
            stream.Write(chr_setB_last);
            stream.Write(temp_val);
            stream.Write(temp_fill);
            stream.Write(lastAccessVRAM);
            stream.Write(paletteNo);
            stream.Write(shift);
            stream.Write(EXtilenumber);
            stream.Write(multiplicand);
            stream.Write(multiplier);
            stream.Write(product);
            stream.Write(split_enable);
            stream.Write(split_right);
            stream.Write(split_tile);
            stream.Write(split_yscroll);
            stream.Write(split_doit);
            stream.Write(split_watch_tile);
            stream.Write(irq_line);
            stream.Write(irq_enable);
            stream.Write(irq_pending);
            stream.Write(irq_current_counter);
            stream.Write(irq_current_inframe);

            snd_1.WriteStateData(ref stream);
            snd_2.WriteStateData(ref stream);
            snd_3.SaveState(ref stream);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            ram_protectA = stream.ReadInt32();
            ram_protectB = stream.ReadInt32();
            ExRAM_mode = stream.ReadInt32();
            for (int i = 0; i < CHROffset_spr.Length; i++)
                CHROffset_spr[i] = stream.ReadInt32();
            for (int i = 0; i < CHROffsetEX.Length; i++)
                CHROffsetEX[i] = stream.ReadInt32();
            for (int i = 0; i < CHROffsetSP.Length; i++)
                CHROffsetSP[i] = stream.ReadInt32();
            for (int i = 0; i < chrRegA.Length; i++)
                chrRegA[i] = stream.ReadInt32();
            for (int i = 0; i < chrRegB.Length; i++)
                chrRegB[i] = stream.ReadInt32();
            for (int i = 0; i < prgReg.Length; i++)
                prgReg[i] = stream.ReadInt32();
            useSRAMmirroring = stream.ReadBoolean();
            chr_high = stream.ReadInt32();
            chr_mode = stream.ReadInt32();
            prg_mode = stream.ReadInt32();
            chr_setB_last = stream.ReadBoolean();
            temp_val = stream.ReadByte();
            temp_fill = stream.ReadByte();
            lastAccessVRAM = stream.ReadInt32();
            paletteNo = stream.ReadInt32();
            shift = stream.ReadInt32();
            EXtilenumber = stream.ReadInt32();
            multiplicand = stream.ReadByte();
            multiplier = stream.ReadByte();
            product = stream.ReadUInt16();
            split_enable = stream.ReadBoolean();
            split_right = stream.ReadBoolean();
            split_tile = stream.ReadInt32();
            split_yscroll = stream.ReadInt32();
            split_doit = stream.ReadBoolean();
            split_watch_tile = stream.ReadInt32();
            irq_line = stream.ReadByte();
            irq_enable = stream.ReadByte();
            irq_pending = stream.ReadInt32();
            irq_current_counter = stream.ReadInt32();
            irq_current_inframe = stream.ReadInt32();

            snd_1.ReadStateData(ref stream);
            snd_2.ReadStateData(ref stream);
            snd_3.LoadState(ref stream);
        }
    }
}