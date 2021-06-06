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
    [BoardInfo("Unknown", 233)]
    class Mapper233 : Board
    {
        private int title = 0;
        private int bank;
        internal override void HardReset()
        {
            base.HardReset();

            bank = title = 0;
        }
        internal override void SoftReset()
        {
            base.SoftReset();
            bank = 0;
            title ^= 0x20;
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {

            bank = data & 0x1F;

            if ((data & 0x20) == 0x20)
            {
                Switch16KPRG(title | bank, PRGArea.Area8000);
                Switch16KPRG(title | bank, PRGArea.AreaC000);
            }
            else
                Switch32KPRG(title >> 1 | bank >> 1, PRGArea.Area8000);

            switch ((data >> 6) & 0x3)
            {
                case 0: Switch01KNMT(0x80); break;
                case 1: Switch01KNMTFromMirroring(Mirroring.Vert); break;
                case 2: Switch01KNMTFromMirroring(Mirroring.Horz); break;
                case 3: Switch01KNMTFromMirroring(Mirroring.OneScB); break;
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(title);
            stream.Write(bank);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            title = stream.ReadInt32();
            bank = stream.ReadInt32();
        }
    }
}