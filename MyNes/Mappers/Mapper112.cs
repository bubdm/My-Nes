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
    [BoardInfo("Asder", 112)]
    class Mapper112 : Board
    {
        private int address_A000;

        internal override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(PRG_ROM_16KB_Mask, PRGArea.AreaC000);
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xE001)
            {
                case 0x8000:
                    {
                        address_A000 = data & 0x7;
                        break;
                    }
                case 0xA000:
                    {
                        switch (address_A000)
                        {
                            case 0: Switch08KPRG(data, PRGArea.Area8000); break;
                            case 1: Switch08KPRG(data, PRGArea.AreaA000); break;
                            case 2: Switch02KCHR(data >> 1, CHRArea.Area0000); break;
                            case 3: Switch02KCHR(data >> 1, CHRArea.Area0800); break;
                            case 4: Switch01KCHR(data, CHRArea.Area1000); break;
                            case 5: Switch01KCHR(data, CHRArea.Area1400); break;
                            case 6: Switch01KCHR(data, CHRArea.Area1800); break;
                            case 7: Switch01KCHR(data, CHRArea.Area1C00); break;
                        }
                        break;
                    }
                case 0xE000: base.Switch01KNMTFromMirroring((data & 1) == 1 ? Mirroring.Horz : Mirroring.Vert); break;

            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(address_A000);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            address_A000 = stream.ReadInt32();
        }
    }
}