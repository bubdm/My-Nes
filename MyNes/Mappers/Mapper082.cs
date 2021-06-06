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
    [BoardInfo("Taito X1-17 ", 82)]
    class Mapper082 : Board
    {
        private bool chr_mode;
        internal override void HardReset()
        {
            base.HardReset();
            Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
        }
        internal override void WriteSRM(ref ushort address, ref byte data)
        {
            switch (address)
            {
                case 0x7EF0: Switch02KCHR(data >> 1, chr_mode ? CHRArea.Area1000 : CHRArea.Area0000); break;
                case 0x7EF1: Switch02KCHR(data >> 1, chr_mode ? CHRArea.Area1800 : CHRArea.Area0800); break;
                case 0x7EF2: Switch01KCHR(data, chr_mode ? CHRArea.Area0000 : CHRArea.Area1000); break;
                case 0x7EF3: Switch01KCHR(data, chr_mode ? CHRArea.Area0400 : CHRArea.Area1400); break;
                case 0x7EF4: Switch01KCHR(data, chr_mode ? CHRArea.Area0800 : CHRArea.Area1800); break;
                case 0x7EF5: Switch01KCHR(data, chr_mode ? CHRArea.Area0C00 : CHRArea.Area1C00); break;
                case 0x7EF6: Switch01KNMTFromMirroring((data & 0x1) == 0x1 ? Mirroring.Vert : Mirroring.Horz); chr_mode = (data & 0x2) == 0x2; break;
                case 0x7EFA: Switch08KPRG(data >> 2, PRGArea.Area8000); break;
                case 0x7EFB: Switch08KPRG(data >> 2, PRGArea.AreaA000); break;
                case 0x7EFC: Switch08KPRG(data >> 2, PRGArea.AreaC000); break;
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(chr_mode);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            chr_mode = stream.ReadBoolean();
        }
    }
}
