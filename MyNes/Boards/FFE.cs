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
    abstract class FFE : Board
    {
        protected bool irqEnable;
        protected int irqCounter;

        internal override void WriteEX(ref ushort address, ref byte data)
        {
            switch (address)
            {
                case 0x4501:
                    {
                        irqEnable = false;
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0x4502:
                    {
                        irqCounter = (irqCounter & 0xFF00) | data;
                        break;
                    }
                case 0x4503:
                    {
                        irqEnable = true;
                        irqCounter = (irqCounter & 0x00FF) | (data << 8);
                        break;
                    }
            }
        }
        internal override void OnCPUClock()
        {
            if (irqEnable)
            {
                irqCounter++;
                if (irqCounter >= 0xFFFF)
                {
                    irqCounter = 0;
                    NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
        }
        internal override void WriteStateData(ref BinaryWriter bin)
        {
            base.WriteStateData(ref bin);
            bin.Write(irqEnable);
            bin.Write(irqCounter);
        }
        internal override void ReadStateData(ref BinaryReader bin)
        {
            base.ReadStateData(ref bin);
            irqEnable = bin.ReadBoolean();
            irqCounter = bin.ReadInt32();
        }
    }
}
