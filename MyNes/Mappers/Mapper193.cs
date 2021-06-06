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
    [BoardInfo("Unknown", 193, 1, 32)]
    [HassIssues]
    class Mapper193 : Board
    {
        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper193; } }
        internal override void HardReset()
        {
            base.HardReset();
            Switch08KPRG(PRG_ROM_08KB_Mask - 2, PRGArea.AreaA000);
            Switch08KPRG(PRG_ROM_08KB_Mask - 1, PRGArea.AreaC000);
            Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
        }
        internal override void WriteSRM(ref ushort address, ref byte data)
        {
            switch (address & 0x6003)
            {
                case 0x6000: Switch04KCHR(data >> 2, CHRArea.Area0000); break;
                case 0x6001: Switch02KCHR(data >> 1, CHRArea.Area1000); break;
                case 0x6002: Switch02KCHR(data >> 1, CHRArea.Area1800); break;
                case 0x6003: Switch08KPRG(data, PRGArea.Area8000); break;
            }
        }
    }
}