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
    [BoardInfo("Irem 74HC161/32", 78)]
    class Mapper078 : Board
    {
        private bool mirroring_mode_single;
        internal override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(PRG_ROM_16KB_Mask, PRGArea.AreaC000);
            mirroring_mode_single = false;
            if (BoardType == "JALECO-JF-16")
                mirroring_mode_single = true;
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            Switch08KCHR((data >> 4) & 0xF);
            Switch16KPRG(data & 0x7, PRGArea.Area8000);
            if (mirroring_mode_single)
                Switch01KNMTFromMirroring((data & 0x8) == 0x8 ? Mirroring.OneScB : Mirroring.OneScA);
            else
                Switch01KNMTFromMirroring((data & 0x8) == 0x8 ? Mirroring.Vert : Mirroring.Horz);
        }
    }
}
