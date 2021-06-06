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
    [BoardInfo("Unknown", 255)]
    [HassIssues]
    class Mapper255 : Board
    {
        private byte[] RAM;
        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper255; } }
        internal override void HardReset()
        {
            base.HardReset();
            RAM = new byte[4];
        }
        internal override void WriteEX(ref ushort address, ref byte data)
        {
            if (address >= 0x5800)
                RAM[address & 0x3] = (byte)(data & 0xF);
        }
        internal override void ReadEX(ref ushort address, out byte data)
        {
            if (address >= 0x5800)
                data = RAM[address & 0x3];
            else
                data = 0;
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            Switch08KCHR(address & 0x3F);
            if ((address & 0x1000) == 0x1000)
            {
                Switch16KPRG(address >> 6 & 0x3F, PRGArea.Area8000);
                Switch16KPRG(address >> 6 & 0x3F, PRGArea.AreaC000);
            }
            else
                Switch32KPRG(((address >> 6) & 0x3F) >> 1, PRGArea.Area8000);
            Switch01KNMTFromMirroring((address & 0x2000) == 0x2000 ? Mirroring.Horz : Mirroring.Vert);
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(RAM);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            stream.Read(RAM, 0, RAM.Length);
        }
    }
}