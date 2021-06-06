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
    [BoardInfo("VRC2", 22)]
    class Mapper022 : Board
    {
        private int[] chr_Reg;

        internal override void HardReset()
        {
            base.HardReset();
            base.Switch16KPRG(PRG_ROM_16KB_Mask, PRGArea.AreaC000);
            chr_Reg = new int[8];
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address)
            {
                case 0x8000:
                case 0x8002:
                case 0x8001:
                case 0x8003: base.Switch08KPRG(data & 0x0F, PRGArea.Area8000); break;
                case 0x9000:
                case 0x9002:
                case 0x9001:
                case 0x9003:
                    {
                        switch (data & 0x3)
                        {
                            case 0: Switch01KNMTFromMirroring(Mirroring.Vert); break;
                            case 1: Switch01KNMTFromMirroring(Mirroring.Horz); break;
                            case 2: Switch01KNMTFromMirroring(Mirroring.OneScA); break;
                            case 3: Switch01KNMTFromMirroring(Mirroring.OneScB); break;
                        }
                        break;
                    }
                case 0xA000:
                case 0xA002:
                case 0xA001:
                case 0xA003: base.Switch08KPRG(data & 0x0F, PRGArea.AreaA000); break;
                case 0xB000: chr_Reg[0] = (chr_Reg[0] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[0] >> 1, CHRArea.Area0000); break;
                case 0xB002: chr_Reg[0] = (chr_Reg[0] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[0] >> 1, CHRArea.Area0000); break;
                case 0xB001: chr_Reg[1] = (chr_Reg[1] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[1] >> 1, CHRArea.Area0400); break;
                case 0xB003: chr_Reg[1] = (chr_Reg[1] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[1] >> 1, CHRArea.Area0400); break;
                case 0xC000: chr_Reg[2] = (chr_Reg[2] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[2] >> 1, CHRArea.Area0800); break;
                case 0xC002: chr_Reg[2] = (chr_Reg[2] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[2] >> 1, CHRArea.Area0800); break;
                case 0xC001: chr_Reg[3] = (chr_Reg[3] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[3] >> 1, CHRArea.Area0C00); break;
                case 0xC003: chr_Reg[3] = (chr_Reg[3] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[3] >> 1, CHRArea.Area0C00); break;
                case 0xD000: chr_Reg[4] = (chr_Reg[4] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[4] >> 1, CHRArea.Area1000); break;
                case 0xD002: chr_Reg[4] = (chr_Reg[4] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[4] >> 1, CHRArea.Area1000); break;
                case 0xD001: chr_Reg[5] = (chr_Reg[5] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[5] >> 1, CHRArea.Area1400); break;
                case 0xD003: chr_Reg[5] = (chr_Reg[5] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[5] >> 1, CHRArea.Area1400); break;
                case 0xE000: chr_Reg[6] = (chr_Reg[6] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[6] >> 1, CHRArea.Area1800); break;
                case 0xE002: chr_Reg[6] = (chr_Reg[6] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[6] >> 1, CHRArea.Area1800); break;
                case 0xE001: chr_Reg[7] = (chr_Reg[7] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[7] >> 1, CHRArea.Area1C00); break;
                case 0xE003: chr_Reg[7] = (chr_Reg[7] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[7] >> 1, CHRArea.Area1C00); break;
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            for (int i = 0; i < chr_Reg.Length; i++)
                stream.Write(chr_Reg[i]);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            for (int i = 0; i < chr_Reg.Length; i++)
                chr_Reg[i] = stream.ReadInt32();
        }
    }
}
