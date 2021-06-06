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
    [BoardInfo("NAMCOT-3453", 154)]
    [HassIssues]
    class Mapper154 : Board
    {
        private int address_8001;
        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper154; } }
        internal override void HardReset()
        {
            base.HardReset();
            address_8001 = 0;
            Switch16KPRG(PRG_ROM_16KB_Mask, PRGArea.AreaC000);
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0x8001)
            {
                case 0x8000:
                    {
                        address_8001 = data & 0x7;
                        Switch01KNMTFromMirroring((data & 0x40) == 0x40 ? Mirroring.OneScB : Mirroring.OneScA);
                        break;
                    }
                case 0x8001:
                    {
                        switch (address_8001)
                        {
                            case 0: Switch02KCHR((data & 0x3F) >> 1, CHRArea.Area0000); break;
                            case 1: Switch02KCHR((data & 0x3F) >> 1, CHRArea.Area0800); break;
                            case 2: Switch01KCHR(data | 0x40, CHRArea.Area1000); break;
                            case 3: Switch01KCHR(data | 0x40, CHRArea.Area1400); break;
                            case 4: Switch01KCHR(data | 0x40, CHRArea.Area1800); break;
                            case 5: Switch01KCHR(data | 0x40, CHRArea.Area1C00); break;
                            case 6: Switch08KPRG(data, PRGArea.Area8000); break;
                            case 7: Switch08KPRG(data, PRGArea.AreaA000); break;
                        }
                        break;
                    }
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(address_8001);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            address_8001 = stream.ReadInt32();
        }
    }
}