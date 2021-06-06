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
    /*Triangle Sound Channel Section*/
    public partial class NesEmu
    {
        private static byte[] trl_step_seq =
        {
             15, 14, 13, 12, 11, 10,  9,  8,  7,  6,  5,  4,  3,  2,  1,  0,
             0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 13, 14, 15
        };
        // Reg1
        private static bool trl_liner_control_flag;
        private static byte trl_liner_control_reload;
        // Reg2
        private static ushort trl_timer;

        private static bool trl_length_enabled;
        private static byte trl_length_counter;
        private static bool trl_liner_control_reload_flag;
        private static byte trl_liner_counter;
        private static int trl_output;
        private static int trl_period_devider;
        private static int trl_step;
        private static bool trl_ignore_reload;

        private static void TRLHardReset()
        {
            trl_liner_control_flag = false;
            trl_liner_control_reload = 0;
            trl_timer = 0;
            trl_length_enabled = false;
            trl_length_counter = 0;
            trl_liner_control_reload_flag = false;
            trl_liner_counter = 0;
            trl_output = 0;
            trl_period_devider = 0;
            trl_step = 0;
            trl_ignore_reload = false;
        }
        private static void TRLSoftReset()
        {
            TRLHardReset();
        }
        private static void TRLClock()
        {
            trl_period_devider--;
            if (trl_period_devider <= 0)
            {
                trl_period_devider = trl_timer + 1;

                if (trl_length_counter > 0 && trl_liner_counter > 0)
                {
                    if (trl_timer >= 4)
                    {
                        trl_step++;
                        trl_step &= 0x1F;
                        if (audio_trl_outputable)
                            trl_output = trl_step_seq[trl_step];
                    }
                }
            }
        }
        private static void TRLClockLength()
        {
            if (trl_length_counter > 0 && !trl_liner_control_flag)
            {
                trl_length_counter--;
                if (apu_reg_access_happened)
                {
                    // This is not a hack, there is some hidden mechanism in the nes, that do reload and clock stuff
                    if (apu_reg_io_addr == 0xB && apu_reg_access_w)
                    {
                        trl_ignore_reload = true;
                    }
                }
            }
        }
        private static void TRLClockEnvelope()
        {
            if (trl_liner_control_reload_flag)
            {
                trl_liner_counter = trl_liner_control_reload;
            }
            else
            {
                if (trl_liner_counter > 0)
                    trl_liner_counter--;
            }
            if (!trl_liner_control_flag)
                trl_liner_control_reload_flag = false;
        }
        private static void APUOnRegister4008()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;
            trl_liner_control_flag = (apu_reg_io_db & 0x80) == 0x80;
            trl_liner_control_reload = (byte)(apu_reg_io_db & 0x7F);
        }
        private static void APUOnRegister4009()
        {
        }
        private static void APUOnRegister400A()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;
            trl_timer = (ushort)((trl_timer & 0x7F00) | apu_reg_io_db);
        }
        private static void APUOnRegister400B()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;
            trl_timer = (ushort)((trl_timer & 0x00FF) | (apu_reg_io_db & 0x7) << 8);

            if (trl_length_enabled && !trl_ignore_reload)
                trl_length_counter = sq_duration_table[apu_reg_io_db >> 3];
            if (trl_ignore_reload)
                trl_ignore_reload = false;
            trl_liner_control_reload_flag = true;
        }
        private static void TRLOn4015()
        {
            trl_length_enabled = (apu_reg_io_db & 0x04) != 0;
            if (!trl_length_enabled)
                trl_length_counter = 0;
        }
        private static void TRLRead4015()
        {
            if (trl_length_counter > 0)
                apu_reg_io_db = (byte)((apu_reg_io_db & 0xFB) | 0x04);
        }

        private static void TRLWriteState(ref System.IO.BinaryWriter bin)
        {
            bin.Write(trl_liner_control_flag);
            bin.Write(trl_liner_control_reload);
            bin.Write(trl_timer);
            bin.Write(trl_length_enabled);
            bin.Write(trl_length_counter);
            bin.Write(trl_liner_control_reload_flag);
            bin.Write(trl_liner_counter);
            bin.Write(trl_output);
            bin.Write(trl_period_devider);
            bin.Write(trl_step);
            bin.Write(trl_ignore_reload);
        }
        private static void TRLReadState(ref System.IO.BinaryReader bin)
        {
            trl_liner_control_flag = bin.ReadBoolean();
            trl_liner_control_reload = bin.ReadByte();
            trl_timer = bin.ReadUInt16();

            trl_length_enabled = bin.ReadBoolean();
            trl_length_counter = bin.ReadByte();
            trl_liner_control_reload_flag = bin.ReadBoolean();
            trl_liner_counter = bin.ReadByte();
            trl_output = bin.ReadInt32();
            trl_period_devider = bin.ReadInt32();
            trl_step = bin.ReadInt32();
            trl_ignore_reload = bin.ReadBoolean();
        }
    }
}
