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
    [BoardInfo("Tengen RAMBO-1", 64, true, true)]
    class Mapper064 : Board
    {
        private bool flag_c;
        private bool flag_p;
        private bool flag_k;
        private int address_8001;
        private int[] chr_reg;
        private int[] prg_reg;
        // IRQ
        private bool irq_enabled;
        private byte irq_counter;
        private byte irq_reload;
        private bool irq_mode;
        private bool irq_clear;
        private int irq_prescaler;

        internal override void HardReset()
        {
            base.HardReset();
            // Flags
            flag_c = flag_p = flag_k = false;
            address_8001 = 0;

            prg_reg = new int[3];
            prg_reg[0] = 0;
            prg_reg[1] = 1;
            prg_reg[2] = 2;
            Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
            SetupPRG();
            // CHR
            chr_reg = new int[8];
            for (int i = 0; i < 8; i++)
                chr_reg[i] = i;
            SetupCHR();

            // IRQ
            irq_enabled = false;
            irq_counter = 0;
            irq_prescaler = 0;
            irq_mode = false;
            irq_reload = 0xFF;
            irq_clear = false;
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xE001)
            {
                case 0x8000:
                    {
                        address_8001 = data & 0xF;
                        flag_c = (data & 0x80) != 0;
                        flag_p = (data & 0x40) != 0;
                        flag_k = (data & 0x20) != 0;
                        SetupCHR();
                        SetupPRG(); break;
                    }
                case 0x8001:
                    {
                        switch (address_8001)
                        {
                            case 0x0:
                            case 0x1:
                            case 0x2:
                            case 0x3:
                            case 0x4:
                            case 0x5: chr_reg[address_8001] = data; SetupCHR(); break;
                            case 0x6:
                            case 0x7: prg_reg[address_8001 - 6] = data; SetupPRG(); break;
                            case 0x8: chr_reg[6] = data; SetupCHR(); break;
                            case 0x9: chr_reg[7] = data; SetupCHR(); break;
                            case 0xF: prg_reg[2] = data; SetupPRG(); break;
                        }
                        break;
                    }
                case 0xA000:
                    {
                        if (NMT_DEFAULT_MIRROR != Mirroring.Full)
                        {
                            base.Switch01KNMTFromMirroring((data & 1) == 1 ? Mirroring.Horz : Mirroring.Vert);
                        }
                        break;
                    }
                case 0xC000: irq_reload = data; break;
                case 0xC001: irq_mode = (data & 0x1) == 0x1; irq_clear = true; irq_prescaler = 0; break;
                case 0xE000: irq_enabled = false; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                case 0xE001: irq_enabled = true; break;
            }
        }
        private void SetupCHR()
        {
            if (!flag_c)
            {
                if (!flag_k)
                {
                    base.Switch02KCHR(chr_reg[0] >> 1, CHRArea.Area0000);
                    base.Switch02KCHR(chr_reg[1] >> 1, CHRArea.Area0800);
                }
                else
                {
                    base.Switch01KCHR(chr_reg[0], CHRArea.Area0000);
                    base.Switch01KCHR(chr_reg[6], CHRArea.Area0400);
                    base.Switch01KCHR(chr_reg[1], CHRArea.Area0800);
                    base.Switch01KCHR(chr_reg[7], CHRArea.Area0C00);
                }
                base.Switch01KCHR(chr_reg[2], CHRArea.Area1000);
                base.Switch01KCHR(chr_reg[3], CHRArea.Area1400);
                base.Switch01KCHR(chr_reg[4], CHRArea.Area1800);
                base.Switch01KCHR(chr_reg[5], CHRArea.Area1C00);
            }
            else
            {
                if (!flag_k)
                {
                    base.Switch02KCHR(chr_reg[0] >> 1, CHRArea.Area1000);
                    base.Switch02KCHR(chr_reg[1] >> 1, CHRArea.Area1800);
                }
                else
                {
                    base.Switch01KCHR(chr_reg[0], CHRArea.Area1000);
                    base.Switch01KCHR(chr_reg[6], CHRArea.Area1400);
                    base.Switch01KCHR(chr_reg[1], CHRArea.Area1800);
                    base.Switch01KCHR(chr_reg[7], CHRArea.Area1C00);
                }
                base.Switch01KCHR(chr_reg[2], CHRArea.Area0000);
                base.Switch01KCHR(chr_reg[3], CHRArea.Area0400);
                base.Switch01KCHR(chr_reg[4], CHRArea.Area0800);
                base.Switch01KCHR(chr_reg[5], CHRArea.Area0C00);
            }
        }
        private void SetupPRG()
        {
            base.Switch08KPRG(prg_reg[flag_p ? 2 : 0], PRGArea.Area8000);
            base.Switch08KPRG(prg_reg[flag_p ? 0 : 1], PRGArea.AreaA000);
            base.Switch08KPRG(prg_reg[flag_p ? 1 : 2], PRGArea.AreaC000);
        }
        // The scanline timer, clocked on PPU A12 raising edge ...
        internal override void OnPPUA12RaisingEdge()
        {
            ClockIRQ();
        }
        internal override void OnCPUClock()
        {
            if (irq_mode)
            {
                irq_prescaler++;
                if (irq_prescaler == 4)
                {
                    irq_prescaler = 0;
                    ClockIRQ();
                }
            }
        }
        private void ClockIRQ()
        {
            if (irq_clear)
            {
                irq_counter = (byte)(irq_reload + 1); irq_clear = false;
            }
            else
            {
                if (irq_counter == 0)
                {
                    irq_counter = irq_reload;
                }
                else
                {
                    if (--irq_counter == 0 && irq_enabled)
                        NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(flag_c);
            stream.Write(flag_p);
            stream.Write(address_8001);
            for (int i = 0; i < chr_reg.Length; i++)
                stream.Write(chr_reg[i]);
            for (int i = 0; i < prg_reg.Length; i++)
                stream.Write(prg_reg[i]);
            stream.Write(irq_enabled);
            stream.Write(irq_counter);
            stream.Write(irq_reload);
            stream.Write(irq_clear);
            stream.Write(irq_prescaler);
            stream.Write(irq_mode);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            flag_c = stream.ReadBoolean();
            flag_p = stream.ReadBoolean();
            address_8001 = stream.ReadInt32();
            for (int i = 0; i < chr_reg.Length; i++)
                chr_reg[i] = stream.ReadInt32();
            for (int i = 0; i < prg_reg.Length; i++)
                prg_reg[i] = stream.ReadInt32();
            irq_enabled = stream.ReadBoolean();
            irq_counter = stream.ReadByte();
            irq_reload = stream.ReadByte();
            irq_clear = stream.ReadBoolean();
            irq_prescaler = stream.ReadInt32();
            irq_mode = stream.ReadBoolean();
        }
    }
}
