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
    /*DMC Sound Channel Section*/
    public partial class NesEmu
    {
        private static int[][] dmc_freq_table =
        {
            new int[]//NTSC
            {
               428, 380, 340, 320, 286, 254, 226, 214, 190, 160, 142, 128, 106,  84,  72,  54
            },
            new int[]//PAL
            {
               398, 354, 316, 298, 276, 236, 210, 198, 176, 148, 132, 118,  98,  78,  66,  50
            },
            new int[]//DENDY (same as ntsc for now)
            {
               428, 380, 340, 320, 286, 254, 226, 214, 190, 160, 142, 128, 106,  84,  72,  54
            },
        };
        private static int dmc_output_a;
        private static int dmc_output;
        private static int dmc_period_devider;
        private static bool dmc_irq_enabled;
        private static bool dmc_loop_flag;
        private static byte dmc_rate_index;
        private static ushort dmc_addr_refresh;
        private static int dmc_size_refresh;

        private static bool dmc_dmaEnabled;
        private static byte dmc_dmaByte;
        private static int dmc_dmaBits;
        private static bool dmc_bufferFull;
        private static byte dmc_dmaBuffer;
        private static int dmc_dmaSize;
        private static ushort dmc_dmaAddr;

        private static void DMCHardReset()
        {
            dmc_output_a = 0;
            dmc_output = 0;
            dmc_period_devider = 0;
            dmc_loop_flag = false;
            dmc_rate_index = 0;

            dmc_irq_enabled = false;
            dmc_dmaAddr = 0xC000;
            dmc_addr_refresh = 0xC000;
            dmc_size_refresh = 0;
            dmc_dmaBits = 1;
            dmc_dmaByte = 1;
            dmc_period_devider = 0;
            dmc_dmaEnabled = false;
            dmc_bufferFull = false;
            dmc_dmaSize = 0;
        }
        private static void DMCSoftReset()
        {
            DMCHardReset();
        }
        private static void DMCClock()
        {
            dmc_period_devider--;
            if (dmc_period_devider <= 0)
            {
                dmc_period_devider = dmc_freq_table[SystemIndex][dmc_rate_index];
                if (dmc_dmaEnabled)
                {
                    if ((dmc_dmaByte & 0x01) != 0)
                    {
                        if (dmc_output_a <= 0x7D)
                            dmc_output_a += 2;
                    }
                    else
                    {
                        if (dmc_output_a >= 0x02)
                            dmc_output_a -= 2;
                    }
                    dmc_dmaByte >>= 1;
                }
                dmc_dmaBits--;
                if (dmc_dmaBits == 0)
                {
                    dmc_dmaBits = 8;
                    if (dmc_bufferFull)
                    {
                        dmc_bufferFull = false;
                        dmc_dmaEnabled = true;
                        dmc_dmaByte = dmc_dmaBuffer;
                        // RDY ?
                        if (dmc_dmaSize > 0)
                        {
                            AssertDMCDMA();
                        }
                    }
                    else
                    {
                        dmc_dmaEnabled = false;
                    }
                }
                if (audio_dmc_outputable)
                {
                    dmc_output = dmc_output_a;
                }
            }
        }
        private static void DMCDoDMA()
        {
            dmc_bufferFull = true;

            Read(ref dmc_dmaAddr, out dmc_dmaBuffer);

            if (dmc_dmaAddr == 0xFFFF)
                dmc_dmaAddr = 0x8000;
            else
                dmc_dmaAddr++;

            if (dmc_dmaSize > 0)
                dmc_dmaSize--;

            if (dmc_dmaSize == 0)
            {
                if (dmc_loop_flag)
                {
                    dmc_dmaSize = dmc_size_refresh;
                    dmc_dmaAddr = dmc_addr_refresh;
                }
                else if (dmc_irq_enabled)
                {
                    IRQFlags |= IRQ_DMC;
                    apu_irq_delta_occur = true;
                }
            }
        }

        private static void APUOnRegister4010()
        {
            if (!apu_reg_access_w)
                return;
            dmc_irq_enabled = (apu_reg_io_db & 0x80) != 0;
            dmc_loop_flag = (apu_reg_io_db & 0x40) != 0;

            if (!dmc_irq_enabled)
            {
                apu_irq_delta_occur = false;
                IRQFlags &= ~IRQ_DMC;
            }
            dmc_rate_index = (byte)(apu_reg_io_db & 0x0F);
        }
        private static void APUOnRegister4011()
        {
            if (!apu_reg_access_w)
                return;
            dmc_output_a = (byte)(apu_reg_io_db & 0x7F);
        }
        private static void APUOnRegister4012()
        {
            if (!apu_reg_access_w)
                return;
            dmc_addr_refresh = (ushort)((apu_reg_io_db << 6) | 0xC000);
        }
        private static void APUOnRegister4013()
        {
            if (!apu_reg_access_w)
                return;
            dmc_size_refresh = (apu_reg_io_db << 4) | 0x0001;
        }
        private static void DMCOn4015()
        {    
            // Disable DMC IRQ
            apu_irq_delta_occur = false;
            IRQFlags &= ~IRQ_DMC;
        }
        private static void DMCRead4015()
        {
            if (dmc_dmaSize > 0)
                apu_reg_io_db = (byte)((apu_reg_io_db & 0xEF) | 0x10);
        }

        private static void DMCWriteState(ref System.IO.BinaryWriter bin)
        {
            bin.Write(dmc_output_a);
            bin.Write(dmc_output);
            bin.Write(dmc_period_devider);
            bin.Write(dmc_irq_enabled);
            bin.Write(dmc_loop_flag);
            bin.Write(dmc_rate_index);
            bin.Write(dmc_addr_refresh);
            bin.Write(dmc_size_refresh);

            bin.Write(dmc_dmaEnabled);
            bin.Write(dmc_dmaByte);
            bin.Write(dmc_dmaBits);
            bin.Write(dmc_bufferFull);
            bin.Write(dmc_dmaBuffer);
            bin.Write(dmc_dmaSize);
            bin.Write(dmc_dmaAddr);
        }
        private static void DMCReadState(ref System.IO.BinaryReader bin)
        {
            dmc_output_a = bin.ReadInt32();
            dmc_output = bin.ReadInt32();
            dmc_period_devider = bin.ReadInt32();
            dmc_irq_enabled = bin.ReadBoolean();
            dmc_loop_flag = bin.ReadBoolean();
            dmc_rate_index = bin.ReadByte();
            dmc_addr_refresh = bin.ReadUInt16();
            dmc_size_refresh = bin.ReadInt32();

            dmc_dmaEnabled = bin.ReadBoolean();
            dmc_dmaByte = bin.ReadByte();
            dmc_dmaBits = bin.ReadInt32();
            dmc_bufferFull = bin.ReadBoolean();
            dmc_dmaBuffer = bin.ReadByte();
            dmc_dmaSize = bin.ReadInt32();
            dmc_dmaAddr = bin.ReadUInt16();
        }
    }
}
