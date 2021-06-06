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
    [BoardInfo("Unknown", 53)]
    [HassIssues]
    class Mapper053 : Board
    {
        private byte[] regs = new byte[2];
        private bool epromFirst;

        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper53; } }
        internal override void HardReset()
        {
            base.HardReset();
            regs = new byte[2];
            epromFirst = true;
            Switch08KPRG(0, PRGArea.Area6000);
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            regs[1] = data;
            UpdatePrg();
        }
        internal override void WriteSRM(ref ushort address, ref byte data)
        {
            regs[0] = data;
            UpdatePrg();
            Switch01KNMTFromMirroring((data & 0x20) == 0x20 ? Mirroring.Horz : Mirroring.Vert);
        }
        private void UpdatePrg()
        {
            int r = regs[0] << 3 & 0x78;

            Switch08KPRG((r << 1 | 0xF) + (epromFirst ? 0x4 : 0x0), PRGArea.Area6000);

            Switch16KPRG((regs[0] & 0x10) == 0x10 ? (r | (regs[1] & 0x7)) + (epromFirst ? 0x2 : 0x0) : epromFirst ? 0x00 : 0x80, PRGArea.Area8000);
            Switch16KPRG((regs[0] & 0x10) == 0x10 ? (r | (0xFF & 0x7)) + (epromFirst ? 0x2 : 0x0) : epromFirst ? 0x01 : 0x81, PRGArea.AreaC000);
        }

        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(regs);
            stream.Write(epromFirst);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            stream.Read(regs, 0, 2);
            epromFirst = stream.ReadBoolean();
        }
    }
}
