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
    [BoardInfo("Unknown", 226)]
    class Mapper226 : Board
    {
        private int prg_reg;
        private bool prg_mode;
        internal override void HardReset()
        {
            base.HardReset();
            prg_reg = 0;
            prg_mode = false;
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0x8001)
            {
                case 0x8000:
                    {
                        prg_reg = (data & 0x1F) | ((data & 0x80) >> 2) | (prg_reg & 0xC0);
                        prg_mode = (data & 0x20) == 0x20;
                        Switch01KNMTFromMirroring((data & 0x40) == 0x40 ? Mirroring.Vert : Mirroring.Horz);
                        if (prg_mode)
                        {
                            Switch16KPRG(prg_reg,PRGArea.Area8000 );
                            Switch16KPRG(prg_reg, PRGArea.AreaC000);
                        }
                        else
                        {
                            Switch32KPRG(prg_reg >> 1, PRGArea.Area8000);
                        }
                        break;
                    }
                case 0x8001:
                    {
                        prg_reg = ((data & 0x1) << 6) | (prg_reg & 0x3F);
                        if (prg_mode)
                        {
                            Switch16KPRG(prg_reg, PRGArea.Area8000);
                            Switch16KPRG(prg_reg, PRGArea.AreaC000);
                        }
                        else
                        {
                            Switch32KPRG(prg_reg >> 1, PRGArea.Area8000);
                        }
                        break;
                    }
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(prg_reg);
            stream.Write(prg_mode);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            prg_reg = stream.ReadInt32();
            prg_mode = stream.ReadBoolean();
        }
    }
}