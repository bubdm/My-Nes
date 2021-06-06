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
    [BoardInfo("BxROM/NINA-001", 34)]
    class Mapper034 : Board
    {
        private bool BxROM;
        private byte writeData;
        internal override void HardReset()
        {
            base.HardReset();
            //Board Type: IREM-BNROM
            BxROM = true;// Assume BxROM
            if (BoardType.Contains("NINA"))
                BxROM = false;
        }
        internal override void WriteSRM(ref ushort address, ref byte data)
        {
            base.WriteSRM(ref address, ref data);
            if (!BxROM)// NINA-001
            {
                switch (address)
                {
                    case 0x7FFD: base.Switch32KPRG(data, PRGArea.Area8000); break;
                    case 0x7FFE: base.Switch04KCHR(data, CHRArea.Area0000); break;
                    case 0x7FFF: base.Switch04KCHR(data, CHRArea.Area1000); break;
                }
            }
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            if (BxROM)
            {
                // Bus Conflicts
                ReadPRG(ref address, out writeData);
                writeData &= data;

                base.Switch32KPRG(writeData, PRGArea.Area8000);
            }
        }
    }
}
