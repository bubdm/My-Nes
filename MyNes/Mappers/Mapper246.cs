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
    [BoardInfo("Unknown", 246)]
    class Mapper246 : Board
    {
        internal override void HardReset()
        {
            base.HardReset();
            Switch08KPRG(0xFF, PRGArea.AreaE000);
        }
        internal override void WriteSRM(ref ushort address, ref byte data)
        {
            if (address < 0x6800)
            {
                switch (address)
                {
                    case 0x6000: Switch08KPRG(data, PRGArea.Area8000); break;
                    case 0x6001: Switch08KPRG(data, PRGArea.AreaA000); break;
                    case 0x6002: Switch08KPRG(data, PRGArea.AreaC000); break;
                    case 0x6003: Switch08KPRG(data, PRGArea.AreaE000); break;
                    case 0x6004: Switch02KCHR(data, CHRArea.Area0000); break;
                    case 0x6005: Switch02KCHR(data, CHRArea.Area0800); break;
                    case 0x6006: Switch02KCHR(data, CHRArea.Area1000); break;
                    case 0x6007: Switch02KCHR(data, CHRArea.Area1800); break;
                }
            }
            else
                base.WriteSRM(ref address, ref data);
        }
    }
}