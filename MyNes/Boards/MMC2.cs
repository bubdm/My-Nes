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
using System.IO;

namespace MyNes.Core
{
    abstract class MMC2 : Board
    {
        internal override void HardReset()
        {
            base.HardReset();
            base.Switch08KPRG((PRG_ROM_08KB_Mask) - 2, PRGArea.AreaA000);
            base.Switch08KPRG((PRG_ROM_08KB_Mask) - 1, PRGArea.AreaC000);
            base.Switch08KPRG((PRG_ROM_08KB_Mask), PRGArea.AreaE000);
            chr_reg0B = 4;
        }
        private byte chr_reg0A;
        private byte chr_reg0B;
        private byte chr_reg1A;
        private byte chr_reg1B;
        private byte latch_a = 0xFE;
        private byte latch_b = 0xFE;

        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xF000)
            {
                case 0xA000:
                    {
                        base.Switch08KPRG(data, PRGArea.Area8000);
                        break;
                    }
                case 0xB000:
                    {
                        chr_reg0A = data;
                        if (latch_a == 0xFD)
                            base.Switch04KCHR(chr_reg0A, CHRArea.Area0000);
                        break;
                    }
                case 0xC000:
                    {
                        chr_reg0B = data;
                        if (latch_a == 0xFE)
                            base.Switch04KCHR(chr_reg0B, CHRArea.Area0000);
                        break;
                    }
                case 0xD000:
                    {
                        chr_reg1A = data;
                        if (latch_b == 0xFD)
                            base.Switch04KCHR(chr_reg1A, CHRArea.Area1000);
                        break;
                    }
                case 0xE000:
                    {
                        chr_reg1B = data;
                        if (latch_b == 0xFE)
                            base.Switch04KCHR(chr_reg1B, CHRArea.Area1000);
                        break;
                    }
                case 0xF000:
                    {
                        base.Switch01KNMTFromMirroring((data & 0x1) == 0x1 ? Mirroring.Horz : Mirroring.Vert);
                        break;
                    }
            }
        }
        internal override void ReadCHR(ref ushort address, out byte data)
        {
            if ((address & 0x1FF0) == 0x0FD0 && latch_a != 0xFD)
            {
                latch_a = 0xFD;
                base.Switch04KCHR(chr_reg0A, CHRArea.Area0000);
            }
            else if ((address & 0x1FF0) == 0x0FE0 && latch_a != 0xFE)
            {
                latch_a = 0xFE;
                base.Switch04KCHR(chr_reg0B, CHRArea.Area0000);
            }
            else if ((address & 0x1FF0) == 0x1FD0 && latch_b != 0xFD)
            {
                latch_b = 0xFD;
                base.Switch04KCHR(chr_reg1A, CHRArea.Area1000);
            }
            else if ((address & 0x1FF0) == 0x1FE0 && latch_b != 0xFE)
            {
                latch_b = 0xFE;
                base.Switch04KCHR(chr_reg1B, CHRArea.Area1000);
            }
            base.ReadCHR(ref address, out data);
        }
        internal override void WriteStateData(ref BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(chr_reg0A);
            stream.Write(chr_reg0B);
            stream.Write(chr_reg1A);
            stream.Write(chr_reg1B);
            stream.Write(latch_a);
            stream.Write(latch_b);
        }
        internal override void ReadStateData(ref BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            chr_reg0A = stream.ReadByte();
            chr_reg0B = stream.ReadByte();
            chr_reg1A = stream.ReadByte();
            chr_reg1B = stream.ReadByte();
            latch_a = stream.ReadByte();
            latch_b = stream.ReadByte();
        }
    }
}
