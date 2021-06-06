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
    [BoardInfo("Unknown", 203)]
    [HassIssues]
    class Mapper203 : Board
    {
        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper203; } }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            Switch08KCHR(data & 0x3);
            Switch16KPRG(data & 0xFC >> 2, PRGArea.Area8000);
            Switch16KPRG(data & 0xFC >> 2, PRGArea.AreaC000);
        }
    }
}