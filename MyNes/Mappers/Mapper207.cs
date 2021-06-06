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
    [BoardInfo("Unknown", 207)]
    [HassIssues]
    class Mapper207 : Board
    {
        private int mirroring0;
        private int mirroring1;
        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper207; } }
        internal override void HardReset()
        {
            base.HardReset();
            Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
        }
        internal override void WriteSRM(ref ushort address, ref byte data)
        {
            switch (address)
            {
                case 0x7EF0:
                    {
                        Switch02KCHR(data & 0x7F >> 1, CHRArea.Area0000);
                        mirroring0 = data >> 7 & 1;
                        break;
                    }
                case 0x7EF1:
                    {
                        Switch02KCHR(data & 0x7F >> 1, CHRArea.Area0800);
                        mirroring1 = data >> 7 & 1;
                        break;
                    }
                case 0x7EF2: Switch01KCHR(data, CHRArea.Area1000); break;
                case 0x7EF3: Switch01KCHR(data, CHRArea.Area1400); break;
                case 0x7EF4: Switch01KCHR(data, CHRArea.Area1800); break;
                case 0x7EF5: Switch01KCHR(data, CHRArea.Area1C00); break;
                case 0x7EFA:
                case 0x7EFB: Switch08KPRG(data, PRGArea.Area8000); break;
                case 0x7EFC:
                case 0x7EFD: Switch08KPRG(data, PRGArea.AreaA000); break;
                case 0x7EFE:
                case 0x7EFF: Switch08KPRG(data, PRGArea.AreaC000); break;
            }
        }
        internal override void ReadNMT(ref ushort address, out byte data)
        {
            switch ((address >> 10) & 3)
            {
                case 0:
                case 1: data = NMT_RAM[mirroring0][address & 0x3FF]; break;
                case 2:
                case 3: data = NMT_RAM[mirroring1][address & 0x3FF]; break;
                default: data = 0; break;
            }
        }
        internal override void WriteNMT(ref ushort address, ref byte data)
        {
            switch ((address >> 10) & 3)
            {
                case 0:
                case 1: NMT_RAM[mirroring0][address & 0x3FF] = data; break;
                case 2:
                case 3: NMT_RAM[mirroring1][address & 0x3FF] = data; break;
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(mirroring0);
            stream.Write(mirroring1);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            mirroring0 = stream.ReadInt32();
            mirroring1 = stream.ReadInt32();
        }
    }
}