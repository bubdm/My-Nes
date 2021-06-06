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
    [BoardInfo("6-in-1 (SuperGK)", 57)]
    class Mapper057 : Board
    {
        private int chr_aaa;
        private int chr_bbb;
        private int chr_hhh;
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0x8800)
            {
                case 0x8000:
                    {
                        chr_aaa = data & 0x7;
                        chr_hhh = (data & 0x40) >> 3;
                        break;
                    }
                case 0x8800:
                    {
                        chr_bbb = data & 0x7;
                        if ((data & 0x10) == 0x10)
                        {
                            Switch32KPRG((data & 0xE0) >> 6, PRGArea.Area8000);
                        }
                        else
                        {
                            Switch16KPRG((data & 0xE0) >> 5, PRGArea.Area8000);
                            Switch16KPRG((data & 0xE0) >> 5, PRGArea.AreaC000);
                        }
                        Switch01KNMTFromMirroring((data & 0x8) == 0x8 ? Mirroring.Horz : Mirroring.Vert);
                        break;
                    }
            }
            Switch08KCHR(chr_hhh | (chr_aaa | chr_bbb));
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(chr_aaa);
            stream.Write(chr_bbb);
            stream.Write(chr_hhh);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            chr_aaa = stream.ReadInt32();
            chr_bbb = stream.ReadInt32();
            chr_hhh = stream.ReadInt32();
        }
    }
}
