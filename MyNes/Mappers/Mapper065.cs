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
    [BoardInfo("Irem H-3001", 65)]
    class Mapper065 : Board
    {
        private bool irq_enable;
        private int irq_reload;
        private int irq_counter;
        internal override void HardReset()
        {
            base.HardReset();
            Switch08KPRG(0x00, PRGArea.Area8000);
            Switch08KPRG(0x01, PRGArea.AreaA000);
            Switch08KPRG(0xFE, PRGArea.AreaC000);
            Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address)
            {
                case 0x8000: Switch08KPRG(data, PRGArea.Area8000); break;
                case 0x9001: Switch01KNMTFromMirroring((data & 0x80) == 0x80 ? Mirroring.Horz : Mirroring.Vert); break;
                case 0x9003: irq_enable = (data & 0x80) == 0x80; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                case 0x9004: irq_counter = irq_reload; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                case 0x9005: irq_reload = (irq_reload & 0x00FF) | (data << 8); break;
                case 0x9006: irq_reload = (irq_reload & 0xFF00) | data; break;
                case 0xA000: Switch08KPRG(data, PRGArea.AreaA000); break;
                case 0xC000: Switch08KPRG(data, PRGArea.AreaC000); break;
                case 0xB000: Switch01KCHR(data, CHRArea.Area0000); break;
                case 0xB001: Switch01KCHR(data, CHRArea.Area0400); break;
                case 0xB002: Switch01KCHR(data, CHRArea.Area0800); break;
                case 0xB003: Switch01KCHR(data, CHRArea.Area0C00); break;
                case 0xB004: Switch01KCHR(data, CHRArea.Area1000); break;
                case 0xB005: Switch01KCHR(data, CHRArea.Area1400); break;
                case 0xB006: Switch01KCHR(data, CHRArea.Area1800); break;
                case 0xB007: Switch01KCHR(data, CHRArea.Area1C00); break;
            }
        }
        internal override void OnCPUClock()
        {
            if (irq_enable)
            {
                if (irq_counter > 0)
                    irq_counter--;
                else if (irq_counter == 0)
                {
                    irq_counter = -1;
                    NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
        }
    }
}
