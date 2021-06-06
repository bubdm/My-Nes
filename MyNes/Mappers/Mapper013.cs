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
    [BoardInfo("CPROM", 13, 1, 16)]
    class Mapper013 : Board
    {
        private byte writeData;
        internal override void HardReset()
        {
            base.HardReset();
            Toggle08KCHR_RAM(true);
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            // Bus Conflicts
            ReadPRG(ref address, out writeData);
            writeData &= data;

            base.Switch04KCHR(writeData & 0x3, CHRArea.Area1000);
        }
    }
}
