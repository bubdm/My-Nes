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
    [BoardInfo("Jaleco SS8806", 18)]
    class Mapper018 : Board
    {
        private int[] prg_reg;
        private int[] chr_reg;
        private int irqRelaod;
        private int irqCounter;
        private bool irqEnable;
        private int irqMask;

        internal override void HardReset()
        {
            base.HardReset();
            base.Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
            prg_reg = new int[3];
            chr_reg = new int[8];
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            //base.WritePRG(ref address, ref data);
            switch (address & 0xF003)
            {
                case 0x8000: prg_reg[0] = (prg_reg[0] & 0xF0) | (data & 0x0F);/*****/ Switch08KPRG(prg_reg[0], PRGArea.Area8000); break;
                case 0x8001: prg_reg[0] = (prg_reg[0] & 0x0F) | ((data & 0x0F) << 4); Switch08KPRG(prg_reg[0], PRGArea.Area8000); break;
                case 0x8002: prg_reg[1] = (prg_reg[1] & 0xF0) | (data & 0x0F);/*****/ Switch08KPRG(prg_reg[1], PRGArea.AreaA000); break;
                case 0x8003: prg_reg[1] = (prg_reg[1] & 0x0F) | ((data & 0x0F) << 4); Switch08KPRG(prg_reg[1], PRGArea.AreaA000); break;
                case 0x9000: prg_reg[2] = (prg_reg[2] & 0xF0) | (data & 0x0F);/*****/ Switch08KPRG(prg_reg[2], PRGArea.AreaC000); break;
                case 0x9001: prg_reg[2] = (prg_reg[2] & 0x0F) | ((data & 0x0F) << 4); Switch08KPRG(prg_reg[2], PRGArea.AreaC000); break;
                case 0xA000: chr_reg[0] = (chr_reg[0] & 0xF0) | (data & 0x0F);/*****/ Switch01KCHR(chr_reg[0], CHRArea.Area0000); break;
                case 0xA001: chr_reg[0] = (chr_reg[0] & 0x0F) | ((data & 0x0F) << 4); Switch01KCHR(chr_reg[0], CHRArea.Area0000); break;
                case 0xA002: chr_reg[1] = (chr_reg[1] & 0xF0) | (data & 0x0F);/*****/ Switch01KCHR(chr_reg[1], CHRArea.Area0400); break;
                case 0xA003: chr_reg[1] = (chr_reg[1] & 0x0F) | ((data & 0x0F) << 4); Switch01KCHR(chr_reg[1], CHRArea.Area0400); break;
                case 0xB000: chr_reg[2] = (chr_reg[2] & 0xF0) | (data & 0x0F);/*****/ Switch01KCHR(chr_reg[2], CHRArea.Area0800); break;
                case 0xB001: chr_reg[2] = (chr_reg[2] & 0x0F) | ((data & 0x0F) << 4); Switch01KCHR(chr_reg[2], CHRArea.Area0800); break;
                case 0xB002: chr_reg[3] = (chr_reg[3] & 0xF0) | (data & 0x0F);/*****/ Switch01KCHR(chr_reg[3], CHRArea.Area0C00); break;
                case 0xB003: chr_reg[3] = (chr_reg[3] & 0x0F) | ((data & 0x0F) << 4); Switch01KCHR(chr_reg[3], CHRArea.Area0C00); break;
                case 0xC000: chr_reg[4] = (chr_reg[4] & 0xF0) | (data & 0x0F);/*****/ Switch01KCHR(chr_reg[4], CHRArea.Area1000); break;
                case 0xC001: chr_reg[4] = (chr_reg[4] & 0x0F) | ((data & 0x0F) << 4); Switch01KCHR(chr_reg[4], CHRArea.Area1000); break;
                case 0xC002: chr_reg[5] = (chr_reg[5] & 0xF0) | (data & 0x0F);/*****/ Switch01KCHR(chr_reg[5], CHRArea.Area1400); break;
                case 0xC003: chr_reg[5] = (chr_reg[5] & 0x0F) | ((data & 0x0F) << 4); Switch01KCHR(chr_reg[5], CHRArea.Area1400); break;
                case 0xD000: chr_reg[6] = (chr_reg[6] & 0xF0) | (data & 0x0F);/*****/ Switch01KCHR(chr_reg[6], CHRArea.Area1800); break;
                case 0xD001: chr_reg[6] = (chr_reg[6] & 0x0F) | ((data & 0x0F) << 4); Switch01KCHR(chr_reg[6], CHRArea.Area1800); break;
                case 0xD002: chr_reg[7] = (chr_reg[7] & 0xF0) | (data & 0x0F);/*****/ Switch01KCHR(chr_reg[7], CHRArea.Area1C00); break;
                case 0xD003: chr_reg[7] = (chr_reg[7] & 0x0F) | ((data & 0x0F) << 4); Switch01KCHR(chr_reg[7], CHRArea.Area1C00); break;
                /*IRQ*/
                case 0xE000: irqRelaod = (irqRelaod & 0xFFF0) | ((data & 0x0F) << 00); break;
                case 0xE001: irqRelaod = (irqRelaod & 0xFF0F) | ((data & 0x0F) << 04); break;
                case 0xE002: irqRelaod = (irqRelaod & 0xF0FF) | ((data & 0x0F) << 08); break;
                case 0xE003: irqRelaod = (irqRelaod & 0x0FFF) | ((data & 0x0F) << 12); break;

                case 0xF000: irqCounter = irqRelaod; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                case 0xF001:
                    {
                        irqEnable = (data & 1) == 1;
                        if ((data & 0x8) == 0x8)
                            irqMask = 0x000F;
                        else if ((data & 0x4) == 0x4)
                            irqMask = 0x00FF;
                        else if ((data & 0x2) == 0x2)
                            irqMask = 0x0FFF;
                        else
                            irqMask = 0xFFFF;
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                    }
                case 0xF002:
                    switch (data & 0x3)
                    {
                        case 0: Switch01KNMTFromMirroring(Mirroring.Horz); break;
                        case 1: Switch01KNMTFromMirroring(Mirroring.Vert); break;
                        case 2: Switch01KNMTFromMirroring(Mirroring.OneScA); break;
                        case 3: Switch01KNMTFromMirroring(Mirroring.OneScB); break;
                    }
                    break;
            }
        }
        internal override void OnCPUClock()
        {
            if (irqEnable)
            {
                if (((irqCounter & irqMask) > 0) && ((--irqCounter & irqMask) == 0))
                {
                    irqEnable = false;
                    NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            for (int i = 0; i < prg_reg.Length; i++)
                stream.Write(prg_reg[i]);
            for (int i = 0; i < chr_reg.Length; i++)
                stream.Write(chr_reg[i]);
            stream.Write(irqRelaod);
            stream.Write(irqCounter);
            stream.Write(irqEnable);
            stream.Write(irqMask);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            for (int i = 0; i < prg_reg.Length; i++)
                prg_reg[i] = stream.ReadInt32();
            for (int i = 0; i < chr_reg.Length; i++)
                chr_reg[i] = stream.ReadInt32();
            irqRelaod = stream.ReadInt32();
            irqCounter = stream.ReadInt32();
            irqEnable = stream.ReadBoolean();
            irqMask = stream.ReadInt32();
        }
    }
}
