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
    [BoardInfo("Taito X-005", 80)]
    class Mapper080 : Board
    {
        internal override void HardReset()
        {
            base.HardReset();
            Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
        }
        internal override void WriteSRM(ref ushort address, ref byte data)
        {
            switch (address)
            {
                case 0x7EF0: Switch02KCHR(data >> 1, CHRArea.Area0000); break;
                case 0x7EF1: Switch02KCHR(data >> 1, CHRArea.Area0800); break;
                case 0x7EF2: Switch01KCHR(data, CHRArea.Area1000); break;
                case 0x7EF3: Switch01KCHR(data, CHRArea.Area1400); break;
                case 0x7EF4: Switch01KCHR(data, CHRArea.Area1800); break;
                case 0x7EF5: Switch01KCHR(data, CHRArea.Area1C00); break;
                case 0x7EF6: Switch01KNMTFromMirroring((data & 0x1) == 0x1 ? Mirroring.Vert : Mirroring.Horz); break;
                case 0x7EFA:
                case 0x7EFB: Switch08KPRG(data, PRGArea.Area8000); break;
                case 0x7EFC:
                case 0x7EFD: Switch08KPRG(data, PRGArea.AreaA000); break;
                case 0x7EFE:
                case 0x7EFF: Switch08KPRG(data, PRGArea.AreaC000); break;
            }
        }
    }
}
