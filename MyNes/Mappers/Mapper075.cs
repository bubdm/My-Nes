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
    [BoardInfo("VRC1", 75)]
    class Mapper075 : Board
    {
        private int chr0_reg;
        private int chr1_reg;
        internal override void HardReset()
        {
            base.HardReset();
            Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xF000)
            {
                case 0x8000: Switch08KPRG(data & 0xF, PRGArea.Area8000); break;
                case 0xA000: Switch08KPRG(data & 0xF, PRGArea.AreaA000); break;
                case 0xC000: Switch08KPRG(data & 0xF, PRGArea.AreaC000); break;
                case 0x9000:
                    {
                        Switch01KNMTFromMirroring((data & 0x1) == 0x1 ? Mirroring.Horz : Mirroring.Vert);
                        chr0_reg = (chr0_reg & 0xF) | ((data & 0x2) << 3);
                        Switch04KCHR(chr0_reg, CHRArea.Area0000);
                        chr1_reg = (chr1_reg & 0xF) | ((data & 0x4) << 2);
                        Switch04KCHR(chr1_reg, CHRArea.Area1000);
                        break;
                    }
                case 0xE000:
                    {
                        chr0_reg = (chr0_reg & 0x10) | (data & 0xF);
                        Switch04KCHR(chr0_reg, CHRArea.Area0000);
                        break;
                    }
                case 0xF000:
                    {
                        chr1_reg = (chr1_reg & 0x10) | (data & 0xF);
                        Switch04KCHR(chr1_reg, CHRArea.Area1000);
                        break;
                    }
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(chr0_reg);
            stream.Write(chr1_reg);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            chr0_reg = stream.ReadInt32();
            chr1_reg = stream.ReadInt32();
        }
    }
}
