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
    [BoardInfo("Unknown", 222)]
    class Mapper222 : Board
    {
        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper222; } }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xF003)
            {
                case 0x8000:
                    Switch08KPRG(data, PRGArea.Area8000);
                    break;
                case 0x9000: Switch01KNMTFromMirroring((data & 1) == 1 ? Mirroring.Horz : Mirroring.Vert); break;
                case 0xA000:
                    Switch08KPRG(data, PRGArea.AreaA000);
                    break;
                case 0xB000:
                    Switch01KCHR(data, CHRArea.Area0000);
                    break;
                case 0xB002:
                    Switch01KCHR(data, CHRArea.Area0400);
                    break;
                case 0xC000:
                    Switch01KCHR(data, CHRArea.Area0800);
                    break;
                case 0xC002:
                    Switch01KCHR(data, CHRArea.Area0C00);
                    break;
                case 0xD000:
                    Switch01KCHR(data, CHRArea.Area1000);
                    break;
                case 0xD002:
                    Switch01KCHR(data, CHRArea.Area1400);
                    break;
                case 0xE000:
                    Switch01KCHR(data, CHRArea.Area1800);
                    break;
                case 0xE002:
                    Switch01KCHR(data, CHRArea.Area1C00);
                    break;
            }
        }
    }
}