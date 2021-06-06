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
    [BoardInfo("Sunsoft 3", 67)]
    class Mapper067 : Board
    {
        private bool irq_enabled;
        private int irq_counter;
        private bool odd;
        internal override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(PRG_ROM_16KB_Mask, PRGArea.AreaC000);
            irq_enabled = false;
            irq_counter = 0xFFFF;
            odd = false;
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xF800)
            {
                case 0x8800: Switch02KCHR(data, CHRArea.Area0000); break;
                case 0x9800: Switch02KCHR(data, CHRArea.Area0800); break;
                case 0xA800: Switch02KCHR(data, CHRArea.Area1000); break;
                case 0xB800: Switch02KCHR(data, CHRArea.Area1800); break;
                case 0xC800:
                    {
                        if (!odd)
                            irq_counter = (irq_counter & 0x00FF) | (data << 8);
                        else
                            irq_counter = (irq_counter & 0xFF00) | data;
                        odd = !odd;
                        break;
                    }
                case 0xD800:
                    {
                        irq_enabled = (data & 0x10) == 0x10;
                        odd = false;
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0xE800:
                    {
                        switch (data & 3)
                        {
                            case 0: Switch01KNMTFromMirroring(Mirroring.Vert); break;
                            case 1: Switch01KNMTFromMirroring(Mirroring.Horz); break;
                            case 2: Switch01KNMTFromMirroring(Mirroring.OneScA); break;
                            case 3: Switch01KNMTFromMirroring(Mirroring.OneScB); break;
                        }
                        break;
                    }
                case 0xF800: Switch16KPRG(data, PRGArea.Area8000); break;
            }
        }
        internal override void OnCPUClock()
        {
            if (irq_enabled)
            {
                irq_counter--;
                if (irq_counter == 0)
                {
                    irq_counter = 0xFFFF;
                    NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                    irq_enabled = false;
                }
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(irq_enabled);
            stream.Write(irq_counter);
            stream.Write(odd);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            irq_enabled = stream.ReadBoolean();
            irq_counter = stream.ReadInt32();
            odd = stream.ReadBoolean();
        }
    }
}
