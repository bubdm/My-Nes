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
    [BoardInfo("Irem G-101", 32)]
    class Mapper032 : Board
    {
        private bool prg_mode;
        private byte prg_reg0;
        private bool enable_mirroring_switch;
        internal override void HardReset()
        {
            base.HardReset();
            enable_mirroring_switch = true;
            // This is not a hack !! this is from mapper docs v0.6.1 by Disch, 032.txt:
            /*"Major League seems to want hardwired 1-screen mirroring.  As far as I know, there is no seperate mapper
               number assigned to address this issue, so you'll have to rely on a CRC or hash check or something for
               treating Major League as a special case."
             */
            if (SHA1 == "7E4180432726A433C46BA2206D9E13B32761C11E")
            { enable_mirroring_switch = false; Switch01KNMTFromMirroring(Mirroring.OneScA); }

            base.Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xF007)
            {
                case 0x8000:
                case 0x8001:
                case 0x8002:
                case 0x8003:
                case 0x8004:
                case 0x8005:
                case 0x8006:
                case 0x8007:
                    {
                        prg_reg0 = data;
                        base.Switch08KPRG(prg_mode ? 0 : prg_reg0, PRGArea.Area8000);
                        base.Switch08KPRG(prg_mode ? prg_reg0 : (PRG_ROM_08KB_Mask - 1), PRGArea.AreaC000);
                        break;
                    }
                case 0x9000:
                case 0x9001:
                case 0x9002:
                case 0x9003:
                case 0x9004:
                case 0x9005:
                case 0x9006:
                case 0x9007:
                    {
                        prg_mode = (data & 0x2) == 0x2;
                        base.Switch08KPRG(prg_mode ? 0 : prg_reg0, PRGArea.Area8000);
                        base.Switch08KPRG(prg_mode ? prg_reg0 : (PRG_ROM_08KB_Mask - 1), PRGArea.AreaC000);
                        if (enable_mirroring_switch)
                            Switch01KNMTFromMirroring((data & 1) == 1 ? Mirroring.Horz : Mirroring.Vert);
                        break;
                    }
                case 0xA000:
                case 0xA001:
                case 0xA002:
                case 0xA003:
                case 0xA004:
                case 0xA005:
                case 0xA006:
                case 0xA007: base.Switch08KPRG(data, PRGArea.AreaA000); break;
                case 0xB000: base.Switch01KCHR(data, CHRArea.Area0000); break;
                case 0xB001: base.Switch01KCHR(data, CHRArea.Area0400); break;
                case 0xB002: base.Switch01KCHR(data, CHRArea.Area0800); break;
                case 0xB003: base.Switch01KCHR(data, CHRArea.Area0C00); break;
                case 0xB004: base.Switch01KCHR(data, CHRArea.Area1000); break;
                case 0xB005: base.Switch01KCHR(data, CHRArea.Area1400); break;
                case 0xB006: base.Switch01KCHR(data, CHRArea.Area1800); break;
                case 0xB007: base.Switch01KCHR(data, CHRArea.Area1C00); break;
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(prg_mode);
            stream.Write(prg_reg0);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            prg_mode = stream.ReadBoolean();
            prg_reg0 = stream.ReadByte();
        }
    }
}
