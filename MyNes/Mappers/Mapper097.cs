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
    [BoardInfo("Irem - PRG HI", 97)]
    class Mapper097 : Board
    {
        internal override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(PRG_ROM_16KB_Mask, PRGArea.Area8000);
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            Switch16KPRG(data & 0xF, PRGArea.AreaC000);
            switch ((address >> 6) & 0x3)
            {
                case 0: Switch01KNMTFromMirroring(Mirroring.OneScA); break;
                case 1: Switch01KNMTFromMirroring(Mirroring.Horz); break;
                case 2: Switch01KNMTFromMirroring(Mirroring.Vert); break;
                case 3: Switch01KNMTFromMirroring(Mirroring.OneScB); break;
            }
        }
    }
}
