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
    [BoardInfo("11-in-1", 51)]
    class Mapper051 : Board
    {
        private int bank = 0;
        private int mode = 1;
        private int offset;
        internal override void HardReset()
        {
            base.HardReset();
            bank = 0;
            mode = 1;
            offset = 0;
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xE000)
            {
                case 0x8000:
                case 0xE000: bank = data & 0xF; UpdateBanks(); break;
                case 0xC000: bank = data & 0xF; mode = (data >> 3 & 0x2) | (mode & 0x1); UpdateBanks(); break;
            }
        }
        internal override void WriteSRM(ref ushort address, ref byte data)
        {
            mode = (data >> 3 & 0x2) | (data >> 1 & 0x1);
            UpdateBanks();
        }
        private void UpdateBanks()
        {
            offset = 0;

            if ((mode & 0x1) == 0x1)
            {
                Switch32KPRG(bank, PRGArea.Area8000);
                offset = 0x23;
            }
            else
            {
                Switch08KPRG((bank << 1) | (mode >> 1), PRGArea.Area8000);
                Switch08KPRG(bank << 1 | 0x7, PRGArea.Area8000);
                offset = 0x2F;
            }
            Switch08KPRG(offset | (bank << 2), PRGArea.Area6000);
            Switch01KNMTFromMirroring((mode == 0x3) ? Mirroring.Horz : Mirroring.Vert);
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(bank);
            stream.Write(mode);
            stream.Write(offset);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            bank = stream.ReadInt32();
            mode = stream.ReadInt32();
            offset = stream.ReadInt32();
        }
    }
}
