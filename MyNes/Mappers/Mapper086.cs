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
    [BoardInfo("Jaleco Early Mapper 2", 86)]
    class Mapper086 : Board
    {
        internal override void WriteSRM(ref ushort address, ref byte data)
        {
            if (address < 0x7000)
            {
                Switch32KPRG((data >> 4) & 0x3, PRGArea.Area8000);
                Switch08KCHR((data & 0x7) | ((data >> 4) & 0x4));
            }
        }
    }
}
