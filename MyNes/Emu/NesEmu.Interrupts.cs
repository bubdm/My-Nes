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
    public partial class NesEmu
    {
        // Determines that IRQ flags (pins)
        internal static int IRQFlags = 0;
        // Represents the current NMI pin (connected to ppu)
        private static bool PPU_NMI_Current;
        // Represents the old status if NMI pin, used to generate NMI in raising edge
        private static bool PPU_NMI_Old;
        const int IRQ_APU = 0x1;
        internal const int IRQ_DMC = 0x2;
        internal const int IRQ_BOARD = 0x8;
        private static ushort InterruptVector;

        private static void PollInterruptStatus()
        {
            if (!cpu_suspend_nmi)
            {
                // The edge detector, see if nmi occurred. 
                if (PPU_NMI_Current & !PPU_NMI_Old) // Raising edge, set nmi request
                    CPU_NMI_PIN = true;
                PPU_NMI_Old = PPU_NMI_Current = false;// NMI detected or not, low both lines for this form ___|-|__
            }
            if (!cpu_suspend_irq)
            {
                // irq level detector
                CPU_IRQ_PIN = (!cpu_flag_i && IRQFlags != 0);
            }

            if (CPU_NMI_PIN)
                InterruptVector = 0xFFFA;
            else
                InterruptVector = 0xFFFE;
        }

        private static void InterruptsWriteState(ref System.IO.BinaryWriter bin)
        {
            bin.Write(IRQFlags);
            bin.Write(PPU_NMI_Current);
            bin.Write(PPU_NMI_Old);
            bin.Write(InterruptVector);
        }
        private static void InterruptsReadState(ref System.IO.BinaryReader bin)
        {
            IRQFlags = bin.ReadInt32();
            PPU_NMI_Current = bin.ReadBoolean();
            PPU_NMI_Old = bin.ReadBoolean();
            InterruptVector = bin.ReadUInt16();
        }
    }
}
