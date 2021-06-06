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
    [BoardInfo("68-in-1 (Game Star)", 58)]
    [HassIssues]
    class Mapper058 : Board
    {
        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper58; } }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            Switch08KCHR((address >> 3) & 0x7);
            if ((address & 0x40) == 0)
                Switch32KPRG((address & 0x7) >> 1, PRGArea.Area8000);
            else
            {
                Switch16KPRG(address & 0x7, PRGArea.Area8000);
                Switch16KPRG(address & 0x7, PRGArea.AreaC000);
            }
            Switch01KNMTFromMirroring((address & 0x80) == 0x80 ? Mirroring.Horz : Mirroring.Vert);
        }
    }
}
