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
    /*DMA section*/
    public partial class NesEmu
    {
        // I suspect the SN74LS373N chip: "OCTAL TRANSPARENT LATCH WITH 3-STATE OUTPUTS; OCTAL D-TYPE FLIP-FLOP
        // WITH 3-STATE OUTPUT"
        // http://html.alldatasheet.com/html-pdf/28021/TI/SN74LS373N/24/1/SN74LS373N.html
        // This chip (somehow, not confirmed yet) is responsible for dma operations inside nes
        // This class emulate the dma behaviors, not as the real chip.

        private static int dma_DMCDMAWaitCycles;
        private static int dma_OAMDMAWaitCycles;
        private static bool dma_isOamDma;
        private static int dma_oamdma_i;
        private static bool dma_DMCOn;
        private static bool dma_OAMOn;
        private static bool dma_DMC_occurring;
        private static bool dma_OAM_occurring;
        private static int dma_OAMFinishCounter;
        private static ushort dma_Oamaddress;
        private static int dma_OAMCYCLE;
        private static byte dma_latch;
        private static byte dma_dummy;
        private static ushort reg_2004;

        private static void DMAHardReset()
        {
            dma_DMCDMAWaitCycles = 0;
            dma_OAMDMAWaitCycles = 0;
            dma_isOamDma = false;
            dma_oamdma_i = 0;
            dma_DMCOn = false;
            dma_OAMOn = false;
            dma_DMC_occurring = false;
            dma_OAM_occurring = false;
            dma_OAMFinishCounter = 0;
            dma_Oamaddress = 0;
            dma_OAMCYCLE = 0;
            dma_latch = 0;
            reg_2004 = 0x2004;
        }
        private static void DMASoftReset()
        {
            dma_DMCDMAWaitCycles = 0;
            dma_OAMDMAWaitCycles = 0;
            dma_isOamDma = false;
            dma_oamdma_i = 0;
            dma_DMCOn = false;
            dma_OAMOn = false;
            dma_DMC_occurring = false;
            dma_OAM_occurring = false;
            dma_OAMFinishCounter = 0;
            dma_Oamaddress = 0;
            dma_OAMCYCLE = 0;
            dma_latch = 0;
        }
        internal static void AssertDMCDMA()
        {
            if (dma_OAM_occurring)
            {
                if (dma_OAMCYCLE < 508)
                    // OAM DMA is occurring here
                    dma_DMCDMAWaitCycles = BUS_RW ? 1 : 0;
                else
                {
                    // Here the oam dma is about to finish
                    // Remaining cycles of oam dma determines the dmc dma waiting cycles.
                    dma_DMCDMAWaitCycles = 4 - (512 - dma_OAMCYCLE);
                }
            }
            else if (dma_DMC_occurring)
            {
                // DMC occurring now !!? is that possible ?
                // Anyway, let's ignore this call !
                return;
            }
            else
            {
                // Nothing occurring, initialize brand new dma
                // DMC DMA depends on r/w flag for the wait cycles.
                dma_DMCDMAWaitCycles = BUS_RW ? 3 : 2;
                // After 2 cycles of oam dma, add extra cycle for the waiting.
                if (dma_OAMFinishCounter == 3)
                    dma_DMCDMAWaitCycles++;
            }
            dma_isOamDma = false;
            dma_DMCOn = true;
        }
        private static void AssertOAMDMA()
        {
            if (dma_OAM_occurring) return;
            // Setup
            // OAM DMA depends on apu odd timer for odd cycles
            dma_OAMDMAWaitCycles = apu_odd_cycle ? 1 : 2;

            dma_isOamDma = true;
            dma_OAMOn = true;
        }
        private static void DMAClock()
        {
            if (dma_OAMFinishCounter > 0)
                dma_OAMFinishCounter--;
            if (!BUS_RW)// Clocks only on reads
            {
                if (dma_DMCDMAWaitCycles > 0)
                    dma_DMCDMAWaitCycles--;
                if (dma_OAMDMAWaitCycles > 0)
                    dma_OAMDMAWaitCycles--;
                return;
            }
            if (dma_DMCOn)
            {
                dma_DMC_occurring = true;
                // This is it !
                dma_DMCOn = false;
                // Do wait cycles (extra reads)
                if (dma_DMCDMAWaitCycles > 0)
                {
                    if (BUS_ADDRESS == 0x4016 || BUS_ADDRESS == 0x4017)
                    {
                        Read(ref BUS_ADDRESS, out dma_dummy);
                        dma_DMCDMAWaitCycles--;

                        while (dma_DMCDMAWaitCycles > 0)
                        {
                            EmuClockComponents();
                            dma_DMCDMAWaitCycles--;
                        }
                    }
                    else
                    {
                        if (dma_DMCDMAWaitCycles > 0)
                        {
                            EmuClockComponents();
                            dma_DMCDMAWaitCycles--;
                        }
                        while (dma_DMCDMAWaitCycles > 0)
                        {
                            Read(ref BUS_ADDRESS, out dma_dummy);
                            dma_DMCDMAWaitCycles--;
                        }
                    }
                }
                // Do DMC DMA
                DMCDoDMA();

                dma_DMC_occurring = false;
            }
            if (dma_OAMOn)
            {
                dma_OAM_occurring = true;
                // This is it ! pause the cpu
                dma_OAMOn = false;
                // Do wait cycles (extra reads)
                if (dma_OAMDMAWaitCycles > 0)
                {
                    if (BUS_ADDRESS == 0x4016 || BUS_ADDRESS == 0x4017)
                    {
                        Read(ref BUS_ADDRESS, out dma_dummy);
                        dma_OAMDMAWaitCycles--;

                        while (dma_OAMDMAWaitCycles > 0)
                        {
                            EmuClockComponents();
                            dma_OAMDMAWaitCycles--;
                        }
                    }
                    else
                    {
                        if (dma_OAMDMAWaitCycles > 0)
                        {
                            EmuClockComponents();
                            dma_OAMDMAWaitCycles--;
                        }
                        while (dma_OAMDMAWaitCycles > 0)
                        {
                            Read(ref BUS_ADDRESS, out dma_dummy);
                            dma_OAMDMAWaitCycles--;
                        }
                    }
                }

                // Do OAM DMA
                dma_OAMCYCLE = 0;
                for (dma_oamdma_i = 0; dma_oamdma_i < 256; dma_oamdma_i++)
                {
                    Read(ref dma_Oamaddress, out dma_latch);
                    dma_OAMCYCLE++;
                    Write(ref reg_2004, ref dma_latch);
                    dma_OAMCYCLE++;
                    dma_Oamaddress = (ushort)((++dma_Oamaddress) & 0xFFFF);
                }
                dma_OAMCYCLE = 0;
                dma_OAMFinishCounter = 5;
                dma_OAM_occurring = false;
            }
        }
        private static void DMAWriteState(ref System.IO.BinaryWriter bin)
        {
            bin.Write(dma_DMCDMAWaitCycles);
            bin.Write(dma_OAMDMAWaitCycles);
            bin.Write(dma_isOamDma);
            bin.Write(dma_oamdma_i);
            bin.Write(dma_DMCOn);
            bin.Write(dma_OAMOn);
            bin.Write(dma_DMC_occurring);
            bin.Write(dma_OAM_occurring);
            bin.Write(dma_OAMFinishCounter);
            bin.Write(dma_Oamaddress);
            bin.Write(dma_OAMCYCLE);
            bin.Write(dma_latch);
            bin.Write(dma_dummy);
        }
        private static void DMAReadState(ref System.IO.BinaryReader bin)
        {
            dma_DMCDMAWaitCycles = bin.ReadInt32();
            dma_OAMDMAWaitCycles = bin.ReadInt32();
            dma_isOamDma = bin.ReadBoolean();
            dma_oamdma_i = bin.ReadInt32();
            dma_DMCOn = bin.ReadBoolean();
            dma_OAMOn = bin.ReadBoolean();
            dma_DMC_occurring = bin.ReadBoolean();
            dma_OAM_occurring = bin.ReadBoolean();
            dma_OAMFinishCounter = bin.ReadInt32();
            dma_Oamaddress = bin.ReadUInt16();
            dma_OAMCYCLE = bin.ReadInt32();
            dma_latch = bin.ReadByte();
            dma_dummy = bin.ReadByte();
        }
    }
}
