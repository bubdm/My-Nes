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
    [BoardInfo("VRC7", 85)]
    [HassIssues]
    class Mapper085 : Board
    {
        private int irq_reload;
        private int irq_counter;
        private int prescaler;
        private bool irq_mode_cycle;
        private bool irq_enable;
        private bool irq_enable_on_ak;

        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper85; } }
        internal override void HardReset()
        {
            base.HardReset();
            base.Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
            irq_reload = 0;
            prescaler = 341;
            irq_counter = 0;
            irq_mode_cycle = false;
            irq_enable = false;
            irq_enable_on_ak = false;
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address)
            {
                case 0x8000: Switch08KPRG(data, PRGArea.Area8000); break;
                case 0x8008:
                case 0x8010: Switch08KPRG(data, PRGArea.AreaA000); break;
                case 0x9000: Switch08KPRG(data, PRGArea.AreaC000); break;
                case 0xA000: Switch01KCHR(data, 0x0000); break;
                case 0xA008:
                case 0xA010: Switch01KCHR(data, CHRArea.Area0400); break;
                case 0xB000: Switch01KCHR(data, CHRArea.Area0800); break;
                case 0xB008:
                case 0xB010: Switch01KCHR(data, CHRArea.Area0C00); break;
                case 0xC000: Switch01KCHR(data, CHRArea.Area1000); break;
                case 0xC008:
                case 0xC010: Switch01KCHR(data, CHRArea.Area1400); break;
                case 0xD000: Switch01KCHR(data, CHRArea.Area1800); break;
                case 0xD008:
                case 0xD010: Switch01KCHR(data, CHRArea.Area1C00); break;
                case 0xE000:
                    {
                        switch (data & 0x3)
                        {
                            case 0: Switch01KNMTFromMirroring(Mirroring.Vert); break;
                            case 1: Switch01KNMTFromMirroring(Mirroring.Horz); break;
                            case 2: Switch01KNMTFromMirroring(Mirroring.OneScA); break;
                            case 3: Switch01KNMTFromMirroring(Mirroring.OneScB); break;
                        }
                        break;
                    }
                case 0xE008:
                case 0xE010: irq_reload = data; break;
                case 0xF000:
                    {
                        irq_mode_cycle = (data & 0x4) == 0x4;
                        irq_enable = (data & 0x2) == 0x2;
                        irq_enable_on_ak = (data & 0x1) == 0x1;
                        if (irq_enable)
                        {
                            irq_counter = irq_reload;
                            prescaler = 341;
                        }
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0xF008:
                case 0xF010: NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; irq_enable = irq_enable_on_ak; break;
            }
        }
        internal override void OnCPUClock()
        {
            if (irq_enable)
            {
                if (!irq_mode_cycle)
                {
                    if (prescaler > 0)
                        prescaler -= 3;
                    else
                    {
                        prescaler = 341;
                        irq_counter++;
                        if (irq_counter == 0xFF)
                        {
                            NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                            irq_counter = irq_reload;
                        }
                    }
                }
                else
                {
                    irq_counter++;
                    if (irq_counter == 0xFF)
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
            stream.Write(prescaler);
            stream.Write(irq_counter);
            stream.Write(irq_mode_cycle);
            stream.Write(irq_reload);
            stream.Write(irq_enable);
            stream.Write(irq_enable_on_ak);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            prescaler = stream.ReadInt32();
            irq_counter = stream.ReadInt32();
            irq_mode_cycle = stream.ReadBoolean();
            irq_reload = stream.ReadInt32();
            irq_enable = stream.ReadBoolean();
            irq_enable_on_ak = stream.ReadBoolean();
        }
    }
}
