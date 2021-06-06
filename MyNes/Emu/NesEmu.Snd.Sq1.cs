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
    /*Square 1 sound channel section*/
    public partial class NesEmu
    {
        private static byte[][] sq_duty_cycle_sequences =
        {
          //  new byte[] {  0, 1, 0, 0, 0, 0, 0, 0 }, // 12.5%
          //  new byte[] {  0, 1, 1, 0, 0, 0, 0, 0 }, // 25.0%
          //  new byte[] {  0, 1, 1, 1, 1, 0, 0, 0 }, // 50.0%
           // new byte[] {  1, 0, 0, 1, 1, 1, 1, 1 }, // 75.0% (25.0% negated)
           new byte[] {  0, 0, 0, 0, 0, 0, 0, 1 }, // 12.5%
           new byte[] {  0, 0, 0, 0, 0, 0, 1, 1 }, // 25.0%
           new byte[] {  0, 0, 0, 0, 1, 1, 1, 1 }, // 50.0%
           new byte[] {  1, 1, 1, 1, 1, 1, 0, 0 }, // 75.0% (25.0% negated)
        };
        private static byte[] sq_duration_table =
        {
            0x0A, 0xFE, 0x14, 0x02, 0x28, 0x04, 0x50, 0x06, 0xA0, 0x08, 0x3C, 0x0A, 0x0E, 0x0C, 0x1A, 0x0E,
            0x0C, 0x10, 0x18, 0x12, 0x30, 0x14, 0x60, 0x16, 0xC0, 0x18, 0x48, 0x1A, 0x10, 0x1C, 0x20, 0x1E,
        };
        // Reg 1
        private static byte sq1_duty_cycle;
        private static bool sq1_length_halt;
        private static bool sq1_constant_volume_envelope;
        private static byte sq1_volume_devider_period;
        // Reg 2
        private static bool sq1_sweep_enable;
        private static byte sq1_sweep_devider_period;
        private static bool sq1_sweep_negate;
        private static byte sq1_sweep_shift_count;
        // Reg 3
        private static int sq1_timer;

        // Controls
        private static int sq1_period_devider;
        private static byte sq1_seqencer;
        private static bool sq1_length_enabled;
        private static int sq1_length_counter;
        private static bool sq1_envelope_start_flag;
        private static byte sq1_envelope_devider;
        private static byte sq1_envelope_decay_level_counter;
        private static byte sq1_envelope;
        private static int sq1_sweep_counter;
        private static bool sq1_sweep_reload;
        private static int sq1_sweep_change;
        private static bool sq1_valid_freq;
        private static int sq1_output;
        private static bool sq1_ignore_reload;

        private static void SQ1HardReset()
        {
            sq1_duty_cycle = 0;
            sq1_length_halt = false;
            sq1_constant_volume_envelope = false;
            sq1_volume_devider_period = 0;
            sq1_sweep_enable = false;
            sq1_sweep_devider_period = 0;
            sq1_sweep_negate = false;
            sq1_sweep_shift_count = 0;
            sq1_timer = 0;
            sq1_period_devider = 0;
            sq1_seqencer = 0;
            sq1_length_enabled = false;
            sq1_length_counter = 0;
            sq1_envelope_start_flag = false;
            sq1_envelope_devider = 0;
            sq1_envelope_decay_level_counter = 0;
            sq1_envelope = 0;
            sq1_sweep_counter = 0;
            sq1_sweep_reload = false;
            sq1_sweep_change = 0;
            sq1_valid_freq = false;
            sq1_output = 0;
            sq1_ignore_reload = false;
        }
        private static void SQ1SoftReset()
        {
            SQ1HardReset();
        }
        private static void SQ1Clock()
        {
            sq1_period_devider--;
            if (sq1_period_devider <= 0)
            {
                sq1_period_devider = sq1_timer + 1;
                sq1_seqencer = (byte)((sq1_seqencer + 1) & 0x7);
                if (sq1_length_counter > 0 && sq1_valid_freq)
                {
                    if (audio_sq1_outputable)
                        sq1_output = sq_duty_cycle_sequences[sq1_duty_cycle][sq1_seqencer] * sq1_envelope;
                }
                else
                    sq1_output = 0;
            }
        }
        private static void SQ1ClockLength()
        {
            if (sq1_length_counter > 0 && !sq1_length_halt)
            {
                sq1_length_counter--;
                if (apu_reg_access_happened)
                {
                    // This is not a hack, there is some hidden mechanism in the nes, that do reload and clock stuff
                    if (apu_reg_io_addr == 0x3 && apu_reg_access_w)
                    {
                        sq1_ignore_reload = true;
                    }
                }
            }
            // Sweep unit
            sq1_sweep_counter--;
            if (sq1_sweep_counter == 0)
            {
                sq1_sweep_counter = sq1_sweep_devider_period + 1;
                if (sq1_sweep_enable && sq1_sweep_shift_count > 0 && sq1_valid_freq)
                {
                    sq1_sweep_change = sq1_timer >> sq1_sweep_shift_count;
                    sq1_timer += sq1_sweep_negate ? ~sq1_sweep_change : sq1_sweep_change;
                    SQ1CalculateValidFreq();
                }
            }
            if (sq1_sweep_reload)
            {
                sq1_sweep_counter = sq1_sweep_devider_period + 1;
                sq1_sweep_reload = false;
            }
        }
        private static void SQ1ClockEnvelope()
        {
            if (sq1_envelope_start_flag)
            {
                sq1_envelope_start_flag = false;
                sq1_envelope_decay_level_counter = 15;
                sq1_envelope_devider = (byte)(sq1_volume_devider_period + 1);
            }
            else
            {
                if (sq1_envelope_devider > 0)
                    sq1_envelope_devider--;
                else
                {
                    sq1_envelope_devider = (byte)(sq1_volume_devider_period + 1);
                    if (sq1_envelope_decay_level_counter > 0)
                        sq1_envelope_decay_level_counter--;
                    else if (sq1_length_halt)
                        sq1_envelope_decay_level_counter = 0xF;
                }
            }
            sq1_envelope = sq1_constant_volume_envelope ? sq1_volume_devider_period : sq1_envelope_decay_level_counter;
        }
        private static void APUOnRegister4000()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;
            sq1_duty_cycle = (byte)((apu_reg_io_db & 0xC0) >> 6);
            sq1_volume_devider_period = (byte)(apu_reg_io_db & 0xF);
            sq1_length_halt = (apu_reg_io_db & 0x20) != 0;
            sq1_constant_volume_envelope = (apu_reg_io_db & 0x10) != 0;

            sq1_envelope = sq1_constant_volume_envelope ? sq1_volume_devider_period : sq1_envelope_decay_level_counter;
        }
        private static void APUOnRegister4001()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;

            sq1_sweep_enable = (apu_reg_io_db & 0x80) == 0x80;
            sq1_sweep_devider_period = (byte)((apu_reg_io_db >> 4) & 7);
            sq1_sweep_negate = (apu_reg_io_db & 0x8) == 0x8;
            sq1_sweep_shift_count = (byte)(apu_reg_io_db & 7);
            sq1_sweep_reload = true;
            SQ1CalculateValidFreq();
        }
        private static void APUOnRegister4002()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;
            sq1_timer = (sq1_timer & 0xFF00) | apu_reg_io_db;
            SQ1CalculateValidFreq();
        }
        private static void APUOnRegister4003()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;

            sq1_timer = (sq1_timer & 0x00FF) | ((apu_reg_io_db & 0x7) << 8);

            if (sq1_length_enabled && !sq1_ignore_reload)
            {
                sq1_length_counter = sq_duration_table[apu_reg_io_db >> 3];
            }
            if (sq1_ignore_reload)
                sq1_ignore_reload = false;

            sq1_seqencer = 0;
            sq1_envelope_start_flag = true;
            SQ1CalculateValidFreq();
        }
        private static void SQ1On4015()
        {
            sq1_length_enabled = (apu_reg_io_db & 0x01) != 0;
            if (!sq1_length_enabled)
                sq1_length_counter = 0;
        }
        private static void SQ1Read4015()
        {
            if (sq1_length_counter > 0)
                apu_reg_io_db = (byte)((apu_reg_io_db & 0xFE) | 0x01);
        }
        private static void SQ1CalculateValidFreq()
        {
            sq1_valid_freq = (sq1_timer >= 0x8) && ((sq1_sweep_negate) || (((sq1_timer + (sq1_timer >> sq1_sweep_shift_count)) & 0x800) == 0));
        }

        private static void SQ1WriteState(ref System.IO.BinaryWriter bin)
        {
            bin.Write(sq1_duty_cycle);
            bin.Write(sq1_length_halt);
            bin.Write(sq1_constant_volume_envelope);
            bin.Write(sq1_volume_devider_period);
            // Reg 2
            bin.Write(sq1_sweep_enable);
            bin.Write(sq1_sweep_devider_period);
            bin.Write(sq1_sweep_negate);
            bin.Write(sq1_sweep_shift_count);
            // Reg 3
            bin.Write(sq1_timer);

            // Controls
            bin.Write(sq1_period_devider);
            bin.Write(sq1_seqencer);
            bin.Write(sq1_length_enabled);
            bin.Write(sq1_length_counter);
            bin.Write(sq1_envelope_start_flag);
            bin.Write(sq1_envelope_devider);
            bin.Write(sq1_envelope_decay_level_counter);
            bin.Write(sq1_envelope);
            bin.Write(sq1_sweep_counter);
            bin.Write(sq1_sweep_reload);
            bin.Write(sq1_sweep_change);
            bin.Write(sq1_valid_freq);
            bin.Write(sq1_output);
            bin.Write(sq1_ignore_reload);
        }
        private static void SQ1ReadState(ref System.IO.BinaryReader bin)
        {
            sq1_duty_cycle = bin.ReadByte();
            sq1_length_halt = bin.ReadBoolean();
            sq1_constant_volume_envelope = bin.ReadBoolean();
            sq1_volume_devider_period = bin.ReadByte();
            sq1_sweep_enable = bin.ReadBoolean();
            sq1_sweep_devider_period = bin.ReadByte();
            sq1_sweep_negate = bin.ReadBoolean();
            sq1_sweep_shift_count = bin.ReadByte();
            sq1_timer = bin.ReadInt32();

            sq1_period_devider = bin.ReadInt32();
            sq1_seqencer = bin.ReadByte();
            sq1_length_enabled = bin.ReadBoolean();
            sq1_length_counter = bin.ReadInt32();
            sq1_envelope_start_flag = bin.ReadBoolean();
            sq1_envelope_devider = bin.ReadByte();
            sq1_envelope_decay_level_counter = bin.ReadByte();
            sq1_envelope = bin.ReadByte();
            sq1_sweep_counter = bin.ReadInt32();
            sq1_sweep_reload = bin.ReadBoolean();
            sq1_sweep_change = bin.ReadInt32();
            sq1_valid_freq = bin.ReadBoolean();
            sq1_output = bin.ReadInt32();
            sq1_ignore_reload = bin.ReadBoolean();
        }
    }
}
