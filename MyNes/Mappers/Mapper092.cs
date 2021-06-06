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
    [BoardInfo("Jaleco Early Mapper 1", 92)]
    class Mapper092 : Board
    {
        private int chr_reg;
        private int prg_reg;
        internal override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(0, PRGArea.Area8000);
            chr_reg = prg_reg = 0;
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch ((data >> 6) & 0x3)
            {
                case 0:// Do switches
                    {
                        Switch08KCHR(chr_reg);
                        Switch16KPRG(prg_reg, PRGArea.AreaC000);
                        break;
                    }
                case 1: chr_reg = data & 0x0F; break;// Set chr reg
                case 2: prg_reg = data & 0x0F; break;// Set prg reg 
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(chr_reg);
            stream.Write(prg_reg);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            chr_reg = stream.ReadInt32();
            prg_reg = stream.ReadInt32();
        }
    }
}
