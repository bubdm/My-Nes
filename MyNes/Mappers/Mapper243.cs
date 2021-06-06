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
    [BoardInfo("Sachen Poker", 243)]
    [HassIssues]
    class Mapper243 : Board
    {
        private int addr;
        private int chr_reg;
        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper243; } }
        internal override void HardReset()
        {
            base.HardReset();
            addr = chr_reg = 0;
        }
        internal override void WriteEX(ref ushort address, ref byte data)
        {
            if ((address < 0x5000) && (address >= 0x4020))
                switch (address & 0x4101)
                {
                    case 0x4100: addr = data & 0x7; break;
                    case 0x4101:
                        {
                            switch (addr)
                            {
                                case 2: chr_reg = ((data << 3) & 0x8) | (chr_reg & 0x7); Switch08KCHR(chr_reg); break;
                                case 4: chr_reg = (data & 1) | (chr_reg & 0xE); Switch08KCHR(chr_reg); break;
                                case 5: Switch32KPRG(data & 0x7, PRGArea.Area8000); break;
                                case 6: chr_reg = ((data & 0x3) << 1) | (chr_reg & 0x9); Switch08KCHR(chr_reg); break;
                                case 7:
                                    {
                                        switch ((data >> 1) & 3)
                                        {
                                            case 0: Switch01KNMTFromMirroring(Mirroring.Horz); break;
                                            case 1: Switch01KNMTFromMirroring(Mirroring.Vert); break;
                                            case 2: Switch01KNMT(0xE); break;
                                            case 3: Switch01KNMTFromMirroring(Mirroring.OneScB); break;
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(addr);
            stream.Write(chr_reg);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            addr = stream.ReadInt32();
            chr_reg = stream.ReadInt32();
        }
    }
}