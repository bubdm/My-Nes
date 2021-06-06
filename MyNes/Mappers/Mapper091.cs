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
    [BoardInfo("HK-SF3", 91, true, true)]
    class Mapper091 : Board
    {
        // IRQ
        private bool irq_enabled;
        private byte irq_counter;
        private int old_irq_counter;
        private byte irq_reload;
        private bool irq_clear;
        internal override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(PRG_ROM_16KB_Mask, PRGArea.AreaC000);
        }
        internal override void WriteSRM(ref ushort address, ref byte data)
        {
            switch (address & 0x7003)
            {
                case 0x6000: Switch02KCHR(data, CHRArea.Area0000); break;
                case 0x6001: Switch02KCHR(data, CHRArea.Area0800); break;
                case 0x6002: Switch02KCHR(data, CHRArea.Area1000); break;
                case 0x6003: Switch02KCHR(data, CHRArea.Area1800); break;
                case 0x7000: Switch08KPRG(data & 0xF, PRGArea.Area8000); break;
                case 0x7001: Switch08KPRG(data & 0xF, PRGArea.AreaA000); break;
                case 0x7002: irq_enabled = false; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                case 0x7003:
                    {
                        irq_enabled = true;
                        irq_reload = 0x7;
                        irq_counter = 0;
                        break;
                    }
            }
        }
        internal override void OnPPUA12RaisingEdge()
        {
            old_irq_counter = irq_counter;

            if (irq_counter == 0 || irq_clear)
                irq_counter = irq_reload;
            else
                irq_counter = (byte)(irq_counter - 1);

            if ((old_irq_counter != 0 || irq_clear) && irq_counter == 0 && irq_enabled)
                NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;

            irq_clear = false;
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(irq_enabled);
            stream.Write(irq_counter);
            stream.Write(old_irq_counter);
            stream.Write(irq_reload);
            stream.Write(irq_clear);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            irq_enabled = stream.ReadBoolean();
            irq_counter = stream.ReadByte();
            old_irq_counter = stream.ReadInt32();
            irq_reload = stream.ReadByte();
            irq_clear = stream.ReadBoolean();
        }
    }
}
