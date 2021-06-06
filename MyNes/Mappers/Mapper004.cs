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
    [BoardInfo("MMC3", 4, true, true)]
    class Mapper004 : Board
    {
        private bool flag_c;
        private bool flag_p;
        private int address_8001;
        private int[] chr_reg;
        private int[] prg_reg;
        // IRQ
        private bool irq_enabled;
        private byte irq_counter;
        private int old_irq_counter;
        private byte irq_reload;
        private bool irq_clear;
        private bool mmc3_alt_behavior;

        internal override void HardReset()
        {
            base.HardReset();
            // Flags
            flag_c = flag_p = false;
            address_8001 = 0;

            prg_reg = new int[4];
            prg_reg[0] = 0;
            prg_reg[1] = 1;
            prg_reg[2] = PRG_ROM_08KB_Mask-1;
            prg_reg[3] = PRG_ROM_08KB_Mask;
            SetupPRG();

            // CHR
            chr_reg = new int[6];
            for (int i = 0; i < 6; i++)
                chr_reg[i] = 0;

            // IRQ
            irq_enabled = false;
            irq_counter = 0;
            irq_reload = 0xFF;
            old_irq_counter = 0;
            mmc3_alt_behavior = false;
            irq_clear = false;

            if (IsGameFoundOnDB)
            {
                switch (GameCartInfo.chip_type[0].ToLower())
                {
                    case "mmc3a": mmc3_alt_behavior = true; Tracer.WriteWarning("Chip= MMC3 A, MMC3 IQR mode switched to RevA"); break;
                    case "mmc3b": mmc3_alt_behavior = false; Tracer.WriteWarning("Chip= MMC3 B, MMC3 IQR mode switched to RevB"); break;
                    case "mmc3c": mmc3_alt_behavior = false; Tracer.WriteWarning("Chip= MMC3 C, MMC3 IQR mode switched to RevB"); break;
                }
            }
        }
        internal override void WritePRG(ref ushort addr, ref byte val)
        {
            switch (addr & 0xE001)
            {
                case 0x8000:
                    {
                        address_8001 = val & 0x7;
                        flag_c = (val & 0x80) != 0;
                        flag_p = (val & 0x40) != 0;
                        SetupCHR();
                        SetupPRG(); break;
                    }
                case 0x8001:
                    {
                        switch (address_8001)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5: chr_reg[address_8001] = val; SetupCHR(); break;
                            case 6:
                            case 7: prg_reg[address_8001 - 6] = val & PRG_ROM_08KB_Mask; SetupPRG(); break;
                        }
                        break;
                    }
                case 0xA000:
                    {
                        if (NMT_DEFAULT_MIRROR != Mirroring.Full)
                        {
                            base.Switch01KNMTFromMirroring((val & 1) == 1 ? Mirroring.Horz : Mirroring.Vert);
                        }
                        break;
                    }
                case 0xA001:
                    {
                        TogglePRGRAMEnable((val & 0x80) != 0);
                        TogglePRGRAMWritableEnable((val & 0x40) == 0);

                        break;
                    }
                case 0xC000: irq_reload = val; break;
                case 0xC001: if (mmc3_alt_behavior) irq_clear = true; irq_counter = 0; break;
                case 0xE000: irq_enabled = false; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                case 0xE001: irq_enabled = true; break;
            }
        }
        private void SetupCHR()
        {
            if (!flag_c)
            {
                base.Switch02KCHR(chr_reg[0] >> 1, CHRArea.Area0000);
                base.Switch02KCHR(chr_reg[1] >> 1, CHRArea.Area0800);
                base.Switch01KCHR(chr_reg[2], CHRArea.Area1000);
                base.Switch01KCHR(chr_reg[3], CHRArea.Area1400);
                base.Switch01KCHR(chr_reg[4], CHRArea.Area1800);
                base.Switch01KCHR(chr_reg[5], CHRArea.Area1C00);
            }
            else
            {
                base.Switch02KCHR(chr_reg[0] >> 1, CHRArea.Area1000);
                base.Switch02KCHR(chr_reg[1] >> 1, CHRArea.Area1800);
                base.Switch01KCHR(chr_reg[2], CHRArea.Area0000);
                base.Switch01KCHR(chr_reg[3], CHRArea.Area0400);
                base.Switch01KCHR(chr_reg[4], CHRArea.Area0800);
                base.Switch01KCHR(chr_reg[5], CHRArea.Area0C00);
            }
        }
        private void SetupPRG()
        {
            base.Switch08KPRG(prg_reg[flag_p ? 2 : 0], PRGArea.Area8000);
            base.Switch08KPRG(prg_reg[1], PRGArea.AreaA000);
            base.Switch08KPRG(prg_reg[flag_p ? 0 : 2], PRGArea.AreaC000);
            base.Switch08KPRG(prg_reg[3], PRGArea.AreaE000);
        }

        // The scanline timer, clocked on PPU A12 raising edge ...
        internal override void OnPPUA12RaisingEdge()
        {
            old_irq_counter = irq_counter;

            if (irq_counter == 0 || irq_clear)
                irq_counter = irq_reload;
            else
                irq_counter = (byte)(irq_counter - 1);

            if ((!mmc3_alt_behavior || old_irq_counter != 0 || irq_clear) && irq_counter == 0 && irq_enabled)
                NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;

            irq_clear = false;
        }

        internal override void WriteStateData(ref BinaryWriter bin)
        {
            base.WriteStateData(ref bin);

            bin.Write(flag_c);
            bin.Write(flag_p);
            bin.Write(address_8001);
            for (int i = 0; i < chr_reg.Length; i++)
                bin.Write(chr_reg[i]);
            for (int i = 0; i < prg_reg.Length; i++)
                bin.Write(prg_reg[i]);
            // IRQ
            bin.Write(irq_enabled);
            bin.Write(irq_counter);
            bin.Write(old_irq_counter);
            bin.Write(irq_reload);
            bin.Write(irq_clear);
            bin.Write(mmc3_alt_behavior);
        }
        internal override void ReadStateData(ref BinaryReader bin)
        {
            base.ReadStateData(ref bin);
            flag_c = bin.ReadBoolean();
            flag_p = bin.ReadBoolean();
            address_8001 = bin.ReadInt32();
            for (int i = 0; i < chr_reg.Length; i++)
                chr_reg[i] = bin.ReadInt32();
            for (int i = 0; i < prg_reg.Length; i++)
                prg_reg[i] = bin.ReadInt32();
            // IRQ
            irq_enabled = bin.ReadBoolean();
            irq_counter = bin.ReadByte();
            old_irq_counter = bin.ReadInt32();
            irq_reload = bin.ReadByte();
            irq_clear = bin.ReadBoolean();
            mmc3_alt_behavior = bin.ReadBoolean();
        }
    }
}
