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
    [BoardInfo("Unknown", 228)]
    [HassIssues]
    class Mapper228 : Board
    {
        private byte[] RAM;
        private int bank;
        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper228; } }
        internal override void HardReset()
        {
            base.HardReset();
            RAM = new byte[4];
        }
        internal override void WriteEX(ref ushort address, ref byte data)
        {
            RAM[address & 0x3] = (byte)(data & 0xF);
        }
        internal override void ReadEX(ref ushort address, out byte data)
        {
            data = RAM[address & 0x3];
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            Switch08KCHR(((address & 0xF) << 2) | (data & 0x3));
            Switch01KNMTFromMirroring((address & 0x2000) == 0x2000 ? Mirroring.Horz : Mirroring.Vert);
            bank = (address >> 7 & 0x1F) + (address >> 7 & address >> 8 & 0x10);
            if ((address & 0x0020) == 0x0020)
            {
                Switch16KPRG((bank << 2) | (address >> 5 & 0x2), PRGArea.Area8000);
                Switch16KPRG((bank << 2) | (address >> 5 & 0x2), PRGArea.AreaC000);
            }
            else
            {
                Switch32KPRG(bank, PRGArea.Area8000);
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(RAM);
            stream.Write(bank);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            stream.Read(RAM, 0, RAM.Length);
            bank = stream.ReadInt32();
        }
    }
}