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
using System.IO;
namespace MyNes.Core
{
    abstract class Bandai : Board
    {
        private bool irq_enable;
        private int irq_counter;
        private Eprom eprom;
        internal override void Initialize(IRom rom)
        {
            base.Initialize(rom);
            if (BoardType.ToLower().Contains("24c01"))// mapper 159
            {
                eprom = new Eprom(128);
            }
            else
            {
                eprom = new Eprom(base.MapperNumber == 16 ? 256 : 128);
            }
        }
        internal override void HardReset()
        {
            base.HardReset();
            base.Switch16KPRG(PRG_ROM_16KB_Mask, PRGArea.AreaC000);
            irq_enable = false;
            irq_counter = 0;
            eprom.HardReset();
        }
        internal override void WriteSRM(ref ushort address, ref byte data)
        {
            WritePRG(ref address, ref data);
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0x000F)
            {
                case 0x0: Switch01KCHR(data, CHRArea.Area0000); break;
                case 0x1: Switch01KCHR(data, CHRArea.Area0400); break;
                case 0x2: Switch01KCHR(data, CHRArea.Area0800); break;
                case 0x3: Switch01KCHR(data, CHRArea.Area0C00); break;
                case 0x4: Switch01KCHR(data, CHRArea.Area1000); break;
                case 0x5: Switch01KCHR(data, CHRArea.Area1400); break;
                case 0x6: Switch01KCHR(data, CHRArea.Area1800); break;
                case 0x7: Switch01KCHR(data, CHRArea.Area1C00); break;
                case 0x8: Switch16KPRG(data, PRGArea.Area8000); break;
                case 0x9:
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
                case 0xA:
                    {
                        irq_enable = (data & 0x1) == 0x1; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0xB:
                    {
                        irq_counter = (irq_counter & 0xFF00) | data;
                        break;
                    }
                case 0xC:
                    {
                        irq_counter = (irq_counter & 0x00FF) | (data << 8);
                        break;
                    }
                case 0xD: eprom.Write(address, data); break;
            }
        }
        internal override void ReadSRM(ref ushort address, out byte value)
        {
            value = eprom.Read(address);
        }
        internal override void OnCPUClock()
        {
            if (irq_enable)
            {
                irq_counter--;
                if (irq_counter == 0)
                {
                    NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
                if (irq_counter < 0)
                {
                    irq_counter = 0xFFFF;
                }
            }
        }

        internal override void WriteStateData(ref BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(irq_enable);
            stream.Write(irq_counter);
            eprom.SaveState(stream);
        }
        internal override void ReadStateData(ref BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            irq_enable = stream.ReadBoolean();
            irq_counter = stream.ReadInt32();
            eprom.LoadState(stream);
        }
    }
}
