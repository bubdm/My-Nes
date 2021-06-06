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
        // Reg 1
        private static byte sq2_duty_cycle;
        private static bool sq2_length_halt;
        private static bool sq2_constant_volume_envelope;
        private static byte sq2_volume_devider_period;
        // Reg 2
        private static bool sq2_sweep_enable;
        private static byte sq2_sweep_devider_period;
        private static bool sq2_sweep_negate;
        private static byte sq2_sweep_shift_count;
        // Reg 3
        private static int sq2_timer;

        // Controls
        private static int sq2_period_devider;
        private static byte sq2_seqencer;
        private static bool sq2_length_enabled;
        private static int sq2_length_counter;
        private static bool sq2_envelope_start_flag;
        private static byte sq2_envelope_devider;
        private static byte sq2_envelope_decay_level_counter;
        private static byte sq2_envelope;
        private static int sq2_sweep_counter;
        private static bool sq2_sweep_reload;
        private static int sq2_sweep_change;
        private static bool sq2_valid_freq;
        private static int sq2_output;
        private static bool sq2_ignore_reload;

        private static void SQ2HardReset()
        {
            sq2_duty_cycle = 0;
            sq2_length_halt = false;
            sq2_constant_volume_envelope = false;
            sq2_volume_devider_period = 0;
            sq2_sweep_enable = false;
            sq2_sweep_devider_period = 0;
            sq2_sweep_negate = false;
            sq2_sweep_shift_count = 0;
            sq2_timer = 0;
            sq2_period_devider = 0;
            sq2_seqencer = 0;
            sq2_length_enabled = false;
            sq2_length_counter = 0;
            sq2_envelope_start_flag = false;
            sq2_envelope_devider = 0;
            sq2_envelope_decay_level_counter = 0;
            sq2_envelope = 0;
            sq2_sweep_counter = 0;
            sq2_sweep_reload = false;
            sq2_sweep_change = 0;
            sq2_valid_freq = false;
            sq2_output = 0;
            sq2_ignore_reload = false;
        }
        private static void SQ2SoftReset()
        {
            SQ2HardReset();
        }
        private static void SQ2Clock()
        {
            sq2_period_devider--;
            if (sq2_period_devider <= 0)
            {
                sq2_period_devider = sq2_timer + 1;
                sq2_seqencer = (byte)((sq2_seqencer + 1) & 0x7);
                if (sq2_length_counter > 0 && sq2_valid_freq)
                {
                    if (audio_sq2_outputable)
                        sq2_output = sq_duty_cycle_sequences[sq2_duty_cycle][sq2_seqencer] * sq2_envelope;
                }
                else
                    sq2_output = 0;
            }
        }
        private static void SQ2ClockLength()
        {
            if (sq2_length_counter > 0 && !sq2_length_halt)
            {
                sq2_length_counter--;
                if (apu_reg_access_happened)
                {
                    // This is not a hack, there is some hidden mechanism in the nes, that do reload and clock stuff
                    if (apu_reg_io_addr == 0x7 && apu_reg_access_w)
                    {
                        sq2_ignore_reload = true;
                    }
                }
            }

            // Sweep unit
            sq2_sweep_counter--;
            if (sq2_sweep_counter == 0)
            {
                sq2_sweep_counter = sq2_sweep_devider_period + 1;
                if (sq2_sweep_enable && sq2_sweep_shift_count > 0 && sq2_valid_freq)
                {
                    sq2_sweep_change = sq2_timer >> sq2_sweep_shift_count;
                    sq2_timer += sq2_sweep_negate ? -sq2_sweep_change : sq2_sweep_change;
                    SQ2CalculateValidFreq();
                }
            }
            else if (sq2_sweep_reload)
            {
                sq2_sweep_counter = sq2_sweep_devider_period + 1;
                sq2_sweep_reload = false;
            }


        }
        private static void SQ2ClockEnvelope()
        {
            if (sq2_envelope_start_flag)
            {
                sq2_envelope_start_flag = false;
                sq2_envelope_decay_level_counter = 15;
                sq2_envelope_devider = (byte)(sq2_volume_devider_period + 1);
            }
            else
            {
                if (sq2_envelope_devider > 0)
                    sq2_envelope_devider--;
                else
                {
                    sq2_envelope_devider = (byte)(sq2_volume_devider_period + 1);
                    if (sq2_envelope_decay_level_counter > 0)
                        sq2_envelope_decay_level_counter--;
                    else if (sq2_length_halt)
                        sq2_envelope_decay_level_counter = 0xF;
                }
            }
            sq2_envelope = sq2_constant_volume_envelope ? sq2_volume_devider_period : sq2_envelope_decay_level_counter;
        }
        private static void APUOnRegister4004()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;
            sq2_duty_cycle = (byte)((apu_reg_io_db & 0xC0) >> 6);
            sq2_volume_devider_period = (byte)(apu_reg_io_db & 0xF);
            sq2_length_halt = (apu_reg_io_db & 0x20) != 0;
            sq2_constant_volume_envelope = (apu_reg_io_db & 0x10) != 0;

            sq2_envelope = sq2_constant_volume_envelope ? sq2_volume_devider_period : sq2_envelope_decay_level_counter;
        }
        private static void APUOnRegister4005()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;

            sq2_sweep_enable = (apu_reg_io_db & 0x80) == 0x80;
            sq2_sweep_devider_period = (byte)((apu_reg_io_db >> 4) & 7);
            sq2_sweep_negate = (apu_reg_io_db & 0x8) == 0x8;
            sq2_sweep_shift_count = (byte)(apu_reg_io_db & 7);
            sq2_sweep_reload = true;
            SQ2CalculateValidFreq();
        }
        private static void APUOnRegister4006()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;
            sq2_timer = (sq2_timer & 0xFF00) | apu_reg_io_db;
            SQ2CalculateValidFreq();
        }
        private static void APUOnRegister4007()
        {
            // Only writes accepted
            if (!apu_reg_access_w)
                return;

            sq2_timer = (sq2_timer & 0x00FF) | ((apu_reg_io_db & 0x7) << 8);

            if (sq2_length_enabled && !sq2_ignore_reload)
                sq2_length_counter = sq_duration_table[apu_reg_io_db >> 3];
            if (sq2_ignore_reload)
                sq2_ignore_reload = false;

            sq2_seqencer = 0;
            sq2_envelope_start_flag = true;
            SQ2CalculateValidFreq();
        }
        private static void SQ2On4015()
        {
            sq2_length_enabled = (apu_reg_io_db & 0x02) != 0;
            if (!sq2_length_enabled)
                sq2_length_counter = 0;
        }
        private static void SQ2Read4015()
        {
            if (sq2_length_counter > 0)
                apu_reg_io_db = (byte)((apu_reg_io_db & 0xFD) | 0x02);
        }
        private static void SQ2CalculateValidFreq()
        {
            sq2_valid_freq = (sq2_timer >= 0x8) && ((sq2_sweep_negate) ||
         (((sq2_timer + (sq2_timer >> sq2_sweep_shift_count)) & 0x800) == 0));
        }

        private static void SQ2WriteState(ref System.IO.BinaryWriter bin)
        {
            bin.Write(sq2_duty_cycle);
            bin.Write(sq2_length_halt);
            bin.Write(sq2_constant_volume_envelope);
            bin.Write(sq2_volume_devider_period);
            // Reg 2
            bin.Write(sq2_sweep_enable);
            bin.Write(sq2_sweep_devider_period);
            bin.Write(sq2_sweep_negate);
            bin.Write(sq2_sweep_shift_count);
            // Reg 3
            bin.Write(sq2_timer);

            // Controls
            bin.Write(sq2_period_devider);
            bin.Write(sq2_seqencer);
            bin.Write(sq2_length_enabled);
            bin.Write(sq2_length_counter);
            bin.Write(sq2_envelope_start_flag);
            bin.Write(sq2_envelope_devider);
            bin.Write(sq2_envelope_decay_level_counter);
            bin.Write(sq2_envelope);
            bin.Write(sq2_sweep_counter);
            bin.Write(sq2_sweep_reload);
            bin.Write(sq2_sweep_change);
            bin.Write(sq2_valid_freq);
            bin.Write(sq2_output);
            bin.Write(sq2_ignore_reload);
        }
        private static void SQ2ReadState(ref System.IO.BinaryReader bin)
        {
            sq2_duty_cycle = bin.ReadByte();
            sq2_length_halt = bin.ReadBoolean();
            sq2_constant_volume_envelope = bin.ReadBoolean();
            sq2_volume_devider_period = bin.ReadByte();
            sq2_sweep_enable = bin.ReadBoolean();
            sq2_sweep_devider_period = bin.ReadByte();
            sq2_sweep_negate = bin.ReadBoolean();
            sq2_sweep_shift_count = bin.ReadByte();
            sq2_timer = bin.ReadInt32();

            sq2_period_devider = bin.ReadInt32();
            sq2_seqencer = bin.ReadByte();
            sq2_length_enabled = bin.ReadBoolean();
            sq2_length_counter = bin.ReadInt32();
            sq2_envelope_start_flag = bin.ReadBoolean();
            sq2_envelope_devider = bin.ReadByte();
            sq2_envelope_decay_level_counter = bin.ReadByte();
            sq2_envelope = bin.ReadByte();
            sq2_sweep_counter = bin.ReadInt32();
            sq2_sweep_reload = bin.ReadBoolean();
            sq2_sweep_change = bin.ReadInt32();
            sq2_valid_freq = bin.ReadBoolean();
            sq2_output = bin.ReadInt32();
            sq2_ignore_reload = bin.ReadBoolean();
        }
    }
}
