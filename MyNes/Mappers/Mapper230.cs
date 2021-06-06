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
    [BoardInfo("Unknown", 230)]
    [HassIssues]
    class Mapper230 : Board
    {
        private bool contraMode = false;
        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper230; } }
        internal override void HardReset()
        {
            base.HardReset();

            //set contra mode
            contraMode = true;
            Switch16KPRG(0, PRGArea.Area8000);
            Switch16KPRG(7, PRGArea.AreaC000);
        }
        internal override void SoftReset()
        {
            base.SoftReset();
            contraMode = !contraMode;
            if (contraMode)
            {
                Switch16KPRG(0, PRGArea.Area8000);
                Switch16KPRG(7, PRGArea.AreaC000);
                Switch01KNMTFromMirroring(Mirroring.Vert);
            }
            else
            {
                Switch08KCHR(0);
                Switch16KPRG(8, PRGArea.Area8000);
                Switch16KPRG(39, PRGArea.AreaC000);
            }
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            if (contraMode)
            {
                Switch16KPRG(data & 0x7, PRGArea.Area8000);
                Switch16KPRG(7, PRGArea.AreaC000);
                Switch01KNMTFromMirroring(Mirroring.Vert);
            }
            else
            {
                Switch01KNMTFromMirroring((data & 0x40) == 0x40 ? Mirroring.Vert : Mirroring.Horz);

                if ((data & 0x20) == 0x20)
                {
                    Switch16KPRG((data & 0x1F) + 8, PRGArea.Area8000);
                    Switch16KPRG((data & 0x1F) + 8, PRGArea.AreaC000);
                }
                else
                    Switch32KPRG(((data & 0x1F) >> 1) + 4, PRGArea.Area8000);
            }
        }
    }
}