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
    /*Noise sound channel section*/
    public partial class NesEmu
    {
        private static ushort[][] nos_freq_table =
        {
            new ushort [] //NTSC
            {
                 4, 8, 16, 32, 64, 96, 128, 160, 202, 254, 380, 508, 762, 1016, 2034, 4068
            },
            new ushort [] //PAL
            {
                 4, 7, 14, 30, 60, 88, 118, 148, 188, 236, 354, 472, 708,  944, 1890, 3778
            },
            new ushort [] //DENDY (same as ntsc for now)
            {
                 4, 8, 16, 32, 64, 96, 128, 160, 202, 254, 380, 508, 762, 1016, 2034, 4068
            }
        };
        // Reg 1
        private static bool nos_length_halt;
        private static bool nos_constant_volume_envelope;
        private static byte nos_volume_devider_period;

        // Reg 3
        private static ushort nos_timer;
        private static bool nos_mode;

        // Controls
        private static int nos_period_devider;
        private static bool nos_length_enabled;
        private static int nos_length_counter;
        private static bool nos_envelope_start_flag;
        private static byte nos_envelope_devider;
        private static byte nos_envelope_decay_level_counter;
        private static byte nos_envelope;
        private static int nos_output;
        private static int nos_shift_reg;
        private static int nos_feedback;
        private static bool nos_ignore_reload;

        private static void NOSHardReset()
        {
            nos_length_halt = false;
            nos_constant_volume_envelope = false;
            nos_volume_devider_period = 0;
            nos_shift_reg = 1;
            nos_timer = 0;
            nos_mode = false;
            nos_period_devider = 0;
            nos_length_enabled = false;
            nos_length_counter = 0;
            nos_envelope_start_flag = false;
            nos_envelope_devider = 0;
            nos_envelope_decay_level_counter = 0;
            nos_envelope = 0;
            nos_output = 0;
            nos_feedback = 0;
            nos_ignore_reload = false;
        }
        private static void NOSSoftReset()
        {
            NOSHardReset();
        }
        private static void NOSClock()
        {
            nos_period_devider--;
            if (nos_period_devider <= 0)
            {
                nos_period_devider = nos_timer;

                if (nos_mode)
                    nos_feedback = ((nos_shift_reg >> 6) & 0x1) ^ (nos_shift_reg & 0x1);
                else
                    nos_feedback = ((nos_shift_reg >> 1) & 0x1) ^ (nos_shift_reg & 0x1);
                nos_shift_reg >>= 1;
                nos_shift_reg = (nos_shift_reg & 0x3FFF) | ((nos_feedback & 1) << 14);

                if (nos_length_counter > 0 && ((nos_shift_reg & 1) == 0))
                {
                    if (audio_nos_outputable)
                        nos_output = nos_envelope;
                }
                else
                    nos_output = 0;
            }
        }
        private static void NOSClockLength()
        {
            if (nos_length_counter > 0 && !nos_length_halt)
            {
                nos_length_counter--;
                if (apu_reg_access_happened)
                {
                    // This is not a hack, there is some hidden mechanism in the nes, that do reload and clock stuff
                    if (apu_reg_io_addr == 0xF && apu_reg_access_w)
                    {
                        nos_ignore_reload = true;
                    }
                }
            }
        }
        private static void NOSClockEnvelope()
        {
            if (nos_envelope_start_flag)
            {
                nos_envelope_start_flag = false;
                nos_envelope_decay_level_counter = 15;
                nos_envelope_devider = (byte)(nos_volume_devider_period + 1);
            }
            else
            {
                if (nos_envelope_devider > 0)
                    nos_envelope_devider--;
                else
                {
                    nos_envelope_devider = (byte)(nos_volume_devider_period + 1);
                    if (nos_envelope_decay_level_counter > 0)
                        nos_envelope_decay_level_counter--;
                    else if (nos_length_halt)
                        nos_envelope_decay_level_counter = 0xF;
                }
            }
            nos_envelope = nos_constant_volume_envelope ? nos_volume_devider_period : nos_envelope_decay_level_counter;
        }
        private static void APUOnRegister400C()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;
            nos_volume_devider_period = (byte)(apu_reg_io_db & 0xF);
            nos_length_halt = (apu_reg_io_db & 0x20) != 0;
            nos_constant_volume_envelope = (apu_reg_io_db & 0x10) != 0;

            nos_envelope = nos_constant_volume_envelope ? nos_volume_devider_period : nos_envelope_decay_level_counter;
        }
        private static void APUOnRegister400D()
        {

        }
        private static void APUOnRegister400E()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;
            nos_timer = (ushort)(nos_freq_table[SystemIndex][apu_reg_io_db & 0x0F] / 2);

            nos_mode = (apu_reg_io_db & 0x80) == 0x80;
        }
        private static void APUOnRegister400F()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;

            if (nos_length_enabled && !nos_ignore_reload)
                nos_length_counter = sq_duration_table[apu_reg_io_db >> 3];
            if (nos_ignore_reload)
                nos_ignore_reload = false;
            nos_envelope_start_flag = true;
        }
        private static void NOSOn4015()
        {
            nos_length_enabled = (apu_reg_io_db & 0x08) != 0;
            if (!nos_length_enabled)
                nos_length_counter = 0;
        }
        private static void NOSRead4015()
        {
            if (nos_length_counter > 0)
                apu_reg_io_db = (byte)((apu_reg_io_db & 0xF7) | 0x08);
        }

        private static void NOSWriteState(ref System.IO.BinaryWriter bin)
        {
            bin.Write(nos_length_halt);
            bin.Write(nos_constant_volume_envelope);
            bin.Write(nos_volume_devider_period);
            bin.Write(nos_timer);
            bin.Write(nos_mode);

            bin.Write(nos_period_devider);
            bin.Write(nos_length_enabled);
            bin.Write(nos_length_counter);
            bin.Write(nos_envelope_start_flag);
            bin.Write(nos_envelope_devider);
            bin.Write(nos_envelope_decay_level_counter);
            bin.Write(nos_envelope);
            bin.Write(nos_output);
            bin.Write(nos_shift_reg);
            bin.Write(nos_feedback);
            bin.Write(nos_ignore_reload);
        }
        private static void NOSReadState(ref System.IO.BinaryReader bin)
        {
            nos_length_halt = bin.ReadBoolean();
            nos_constant_volume_envelope = bin.ReadBoolean();
            nos_volume_devider_period = bin.ReadByte();

            nos_timer = bin.ReadUInt16();
            nos_mode = bin.ReadBoolean();

            nos_period_devider = bin.ReadInt32();
            nos_length_enabled = bin.ReadBoolean();
            nos_length_counter = bin.ReadInt32();
            nos_envelope_start_flag = bin.ReadBoolean();
            nos_envelope_devider = bin.ReadByte();
            nos_envelope_decay_level_counter = bin.ReadByte();
            nos_envelope = bin.ReadByte();
            nos_output = bin.ReadInt32();
            nos_shift_reg = bin.ReadInt32();
            nos_feedback = bin.ReadInt32();
            nos_ignore_reload = bin.ReadBoolean();
        }
    }
}
