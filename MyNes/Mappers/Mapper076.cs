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
    [BoardInfo("Namco 109", 76)]
    class Mapper076 : Board
    {
        private int address_8001;
        private bool prg_a;
        private byte prg_reg;
        internal override void HardReset()
        {
            base.HardReset();
            Switch08KPRG(PRG_ROM_08KB_Mask - 1, PRGArea.AreaC000);
            Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
            address_8001 = 0;
            prg_a = false;
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xE001)
            {
                case 0x8000:
                    {
                        address_8001 = data & 0x7;
                        prg_a = (data & 0x40) == 0x40;
                        Switch08KPRG(prg_reg, prg_a ? PRGArea.AreaC000 : PRGArea.Area8000);
                        break;
                    }
                case 0x8001:
                    {
                        switch (address_8001)
                        {
                            case 0x2: Switch02KCHR(data, CHRArea.Area0000); break;
                            case 0x3: Switch02KCHR(data, CHRArea.Area0800); break;
                            case 0x4: Switch02KCHR(data, CHRArea.Area1000); break;
                            case 0x5: Switch02KCHR(data, CHRArea.Area1800); break;
                            case 0x6: Switch08KPRG(prg_reg = data, prg_a ? PRGArea.AreaC000 : PRGArea.Area8000); break;
                            case 0x7: Switch08KPRG(data, PRGArea.AreaA000); break;
                        }
                        break;
                    }
                case 0xA000: Switch01KNMTFromMirroring((data & 1) == 1 ? Mirroring.Horz : Mirroring.Vert); break;
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(address_8001);
            stream.Write(prg_a);
            stream.Write(prg_reg);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            address_8001 = stream.ReadInt32();
            prg_a = stream.ReadBoolean();
            prg_reg = stream.ReadByte();
        }
    }
}
