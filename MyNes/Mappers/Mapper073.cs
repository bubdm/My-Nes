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
    [BoardInfo("VRC3", 73)]
    class Mapper073 : Board
    {
        private bool irq_mode_8;
        private bool irq_enable;
        private bool irq_enable_on_ak;
        private int irq_reload;
        private int irq_counter;
        internal override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(PRG_ROM_16KB_Mask, PRGArea.AreaC000);
            irq_mode_8 = false;
            irq_enable = false;
            irq_enable_on_ak = false;
            irq_reload = 0;
            irq_counter = 0;
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xF000)
            {
                case 0x8000: irq_reload = (irq_reload & 0xFFF0) | (data & 0xF); break;
                case 0x9000: irq_reload = (irq_reload & 0xFF0F) | ((data & 0xF) << 4); break;
                case 0xA000: irq_reload = (irq_reload & 0xF0FF) | ((data & 0xF) << 8); break;
                case 0xB000: irq_reload = (irq_reload & 0x0FFF) | ((data & 0xF) << 12); break;
                case 0xC000:
                    {
                        irq_mode_8 = (data & 0x4) == 0x4;
                        irq_enable = (data & 0x2) == 0x2;
                        irq_enable_on_ak = (data & 0x1) == 0x1;
                        if (irq_enable)
                            irq_counter = irq_reload;

                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0xD000:
                    {
                        irq_enable = irq_enable_on_ak;
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0xF000: Switch16KPRG(data & 0xF, PRGArea.Area8000); break;

            }
        }
        internal override void OnCPUClock()
        {
            if (irq_enable)
            {
                if (irq_mode_8)
                {
                    irq_counter = irq_counter & 0xFF00 | (byte)((irq_counter & 0xFF) + 1);
                    if ((byte)(irq_counter & 0xFF) == 0xFF)
                    {
                        NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                        irq_counter = (irq_counter & 0xFF00) | (irq_reload & 0xFF);
                    }
                }
                else
                {
                    irq_counter++;
                    if (irq_counter == 0xFFFF)
                    {
                        NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                        irq_counter = irq_reload;
                    }
                }
            }
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(irq_mode_8);
            stream.Write(irq_enable);
            stream.Write(irq_enable_on_ak);
            stream.Write(irq_reload);
            stream.Write(irq_counter);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            irq_mode_8 = stream.ReadBoolean();
            irq_enable = stream.ReadBoolean();
            irq_enable_on_ak = stream.ReadBoolean();
            irq_reload = stream.ReadInt32();
            irq_counter = stream.ReadInt32();
        }
    }
}
