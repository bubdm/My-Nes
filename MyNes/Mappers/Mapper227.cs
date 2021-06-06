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
    [BoardInfo("Unknown", 227)]
    class Mapper227 : Board
    {
        private bool flag_o;
        private bool flag_s;
        private bool flag_l;
        private int prg_reg;
        internal override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(0, PRGArea.AreaC000);
            flag_o = false;
            flag_s = false;
            flag_l = false;
            prg_reg = 0;
            ToggleCHRRAMWritableEnable(true);
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            flag_s = (address & 0x1) == 0x1;
            flag_o = (address & 0x80) == 0x80;
            flag_l = (address & 0x200) == 0x200;
            prg_reg = ((address >> 2) & 0x1F) | ((address >> 3) & 0x20);
            Switch01KNMTFromMirroring((address & 0x2) == 0x2 ? Mirroring.Horz : Mirroring.Vert);
            ToggleCHRRAMWritableEnable(!flag_o);
            if (flag_o)
            {
                if (!flag_s)
                {
                    Switch16KPRG(prg_reg, PRGArea.Area8000);
                    Switch16KPRG(prg_reg, PRGArea.AreaC000);
                }
                else
                {
                    Switch32KPRG(prg_reg >> 1, PRGArea.Area8000);
                }
            }
            else
            {
                if (!flag_l)
                {
                    if (!flag_s)
                    {
                        Switch16KPRG(prg_reg, PRGArea.Area8000);
                        Switch16KPRG(prg_reg & 0x38, PRGArea.AreaC000);
                    }
                    else
                    {
                        Switch16KPRG(prg_reg & 0x3E, PRGArea.Area8000);
                        Switch16KPRG(prg_reg & 0x38, PRGArea.AreaC000);
                    }
                }
                else
                {
                    if (!flag_s)
                    {
                        Switch16KPRG(prg_reg, PRGArea.Area8000);
                        Switch16KPRG(prg_reg | 0x7, PRGArea.AreaC000);
                    }
                    else
                    {
                        Switch16KPRG(prg_reg & 0x3E, PRGArea.Area8000);
                        Switch16KPRG(prg_reg | 0x7, PRGArea.AreaC000);
                    }
                }
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(flag_o);
            stream.Write(flag_s);
            stream.Write(flag_l);
            stream.Write(prg_reg);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            flag_o = stream.ReadBoolean();
            flag_s = stream.ReadBoolean();
            flag_l = stream.ReadBoolean();
            prg_reg = stream.ReadInt32();
        }
    }
}