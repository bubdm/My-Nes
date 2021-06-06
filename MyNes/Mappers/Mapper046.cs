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
    [BoardInfo("15-in-1 Color Dreams", 46)]
    class Mapper046 : Board
    {
        private int prg_reg;
        private int chr_reg;

        internal override void WriteSRM(ref ushort address, ref byte data)
        {
            prg_reg = (prg_reg & 0x01) | ((data << 1) & 0x1E);
            chr_reg = (chr_reg & 0x07) | ((data >> 1) & 0x78);
            Switch08KCHR(chr_reg);
            Switch32KPRG(prg_reg, PRGArea.Area8000);
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            prg_reg = (data >> 0 & 0x1) | (prg_reg & 0x1E);
            chr_reg = (data >> 4 & 0x7) | (chr_reg & 0x78);
            Switch08KCHR(chr_reg);
            Switch32KPRG(prg_reg, PRGArea.Area8000);
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(prg_reg);
            stream.Write(chr_reg);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            prg_reg = stream.ReadInt32();
            chr_reg = stream.ReadInt32();
        }
    }
}
