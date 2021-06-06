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
    [BoardInfo("Sunsoft 4", 68)]
    class Mapper068 : Board
    {
        private bool flag_r;
        private bool flag_m;
        private int nt_reg0, nt_reg1;
        private int temp;
        internal override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(PRG_ROM_16KB_Mask, PRGArea.AreaC000);
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xF000)
            {
                case 0x8000: Switch02KCHR(data, CHRArea.Area0000); break;
                case 0x9000: Switch02KCHR(data, CHRArea.Area0800); break;
                case 0xA000: Switch02KCHR(data, CHRArea.Area1000); break;
                case 0xB000: Switch02KCHR(data, CHRArea.Area1800); break;
                case 0xC000: nt_reg0 = ((data & 0x7F) | 0x80); break;
                case 0xD000: nt_reg1 = ((data & 0x7F) | 0x80); break;
                case 0xE000:
                    {
                        flag_r = (data & 0x10) == 0x10;
                        flag_m = (data & 0x01) == 0x01;
                        Switch01KNMTFromMirroring(flag_m ? Mirroring.Horz : Mirroring.Vert);
                        break;
                    }
                case 0xF000: Switch16KPRG(data, PRGArea.Area8000); break;
            }
        }
        internal override void ReadNMT(ref ushort address, out byte data)
        {
            if (!flag_r)
                data = NMT_RAM[NMT_AREA_BLK_INDEX[(address >> 10) & 3]][address & 0x3FF];
            else
            {
                switch ((address >> 10) & 3)
                {
                    case 0: data = CHR_ROM[nt_reg0][address & 0x3FF]; break;
                    case 1: data = CHR_ROM[(flag_m ? nt_reg0 : nt_reg1)][address & 0x3FF]; break;
                    case 2: data = CHR_ROM[(flag_m ? nt_reg1 : nt_reg0)][address & 0x3FF]; break;
                    case 3: data = CHR_ROM[nt_reg1][address & 0x3FF]; break;
                    default: data = 0; break;
                }
            }
        }
        internal override void WriteNMT(ref ushort address, ref byte data)
        {
            if (!flag_r)
                base.WriteNMT(ref address, ref data);
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(flag_r);
            stream.Write(flag_m);
            stream.Write(nt_reg0);
            stream.Write(nt_reg1);
            stream.Write(temp);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            flag_r = stream.ReadBoolean();
            flag_m = stream.ReadBoolean();
            nt_reg0 = stream.ReadInt32();
            nt_reg1 = stream.ReadInt32();
            temp = stream.ReadInt32();
        }
    }
}
