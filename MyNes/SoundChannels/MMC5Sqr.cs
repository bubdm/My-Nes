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
    class MMC5Sqr
    {
        private byte[][] duty_cycle_sequences =
       {
           new byte[] {  0, 0, 0, 0, 0, 0, 0, 1 }, // 12.5%
           new byte[] {  0, 0, 0, 0, 0, 0, 1, 1 }, // 25.0%
           new byte[] {  0, 0, 0, 0, 1, 1, 1, 1 }, // 50.0%
           new byte[] {  1, 1, 1, 1, 1, 1, 0, 0 }, // 75.0% (25.0% negated)
        };
        private byte[] duration_table =
        {
            0x0A, 0xFE, 0x14, 0x02, 0x28, 0x04, 0x50, 0x06, 0xA0, 0x08, 0x3C, 0x0A, 0x0E, 0x0C, 0x1A, 0x0E,
            0x0C, 0x10, 0x18, 0x12, 0x30, 0x14, 0x60, 0x16, 0xC0, 0x18, 0x48, 0x1A, 0x10, 0x1C, 0x20, 0x1E,
        };
        // Reg 1
        private byte duty_cycle;
        private bool length_halt;
        private bool constant_volume_envelope;
        private byte volume_devider_period;
        // Reg 3
        private int timer;
        private int period_devider;
        private byte seqencer;
        private bool length_enabled;
        private int length_counter;
        private bool envelope_start_flag;
        private byte envelope_devider;
        private byte envelope_decay_level_counter;
        private byte envelope;
        internal int output;
        internal bool Outputable;

        internal void HardReset()
        {
            duty_cycle = 0;
            length_halt = false;
            constant_volume_envelope = false;
            volume_devider_period = 0;
            timer = 0;
            period_devider = 0;
            seqencer = 0;
            length_enabled = false;
            length_counter = 0;
            envelope_start_flag = false;
            envelope_devider = 0;
            envelope_decay_level_counter = 0;
            envelope = 0;
        }
        internal void SoftReset()
        {
            HardReset();
        }
        internal void Clock()
        {
            period_devider--;
            if (period_devider <= 0)
            {
                period_devider = timer + 1;

                if (length_counter > 0)
                {
                    if (Outputable)
                        output = duty_cycle_sequences[duty_cycle][seqencer] * envelope;
                }
                else
                    output = 0;

                if (seqencer == 0)
                    seqencer = 7;
                else
                    seqencer--;
            }
        }
        internal void ClockLength()
        {
            if (length_counter > 0 && !length_halt)
            {
                length_counter--;
            }
        }
        internal void ClockEnvelope()
        {
            if (envelope_start_flag)
            {
                envelope_start_flag = false;
                envelope_decay_level_counter = 15;
                envelope_devider = (byte)(volume_devider_period + 1);
            }
            else
            {
                if (envelope_devider > 0)
                    envelope_devider--;
                else
                {
                    envelope_devider = (byte)(volume_devider_period + 1);
                    if (envelope_decay_level_counter > 0)
                        envelope_decay_level_counter--;
                    else if (length_halt)
                        envelope_decay_level_counter = 0xF;
                }
            }
            envelope = constant_volume_envelope ? volume_devider_period : envelope_decay_level_counter;
        }
        internal void Write0(ref byte value)
        {
            duty_cycle = (byte)((value & 0xC0) >> 6);
            volume_devider_period = (byte)(value & 0xF);
            length_halt = (value & 0x20) != 0;
            constant_volume_envelope = (value & 0x10) != 0;

            envelope = constant_volume_envelope ? volume_devider_period : envelope_decay_level_counter;
        }
        internal void Write2(ref byte value)
        {
            timer = (timer & 0xFF00) | value;
        }
        internal void Write3(ref byte value)
        {
            timer = (timer & 0x00FF) | ((value & 0x7) << 8);

            if (length_enabled)
            {
                length_counter = duration_table[value >> 3];
            }

            seqencer = 0;
            envelope_start_flag = true;
        }
        internal void WriteEnabled(bool enabled)
        {
            length_enabled = enabled;
            if (!length_enabled)
                length_counter = 0;
        }
        internal bool ReadEnable()
        {
            return (length_counter > 0);
        }

        internal void WriteStateData(ref BinaryWriter bin)
        {
            bin.Write(duty_cycle);
            bin.Write(length_halt);
            bin.Write(constant_volume_envelope);
            bin.Write(volume_devider_period);
            bin.Write(timer);
            bin.Write(period_devider);
            bin.Write(seqencer);
            bin.Write(length_enabled);
            bin.Write(length_counter);
            bin.Write(envelope_start_flag);
            bin.Write(envelope_devider);
            bin.Write(envelope_decay_level_counter);
            bin.Write(envelope);
            bin.Write(output);
        }
        internal void ReadStateData(ref BinaryReader bin)
        {
            duty_cycle = bin.ReadByte();
            length_halt = bin.ReadBoolean();
            constant_volume_envelope = bin.ReadBoolean();
            volume_devider_period = bin.ReadByte();
            timer = bin.ReadInt32();
            period_devider = bin.ReadInt32();
            seqencer = bin.ReadByte();
            length_enabled = bin.ReadBoolean();
            length_counter = bin.ReadInt32();
            envelope_start_flag = bin.ReadBoolean();
            envelope_devider = bin.ReadByte();
            envelope_decay_level_counter = bin.ReadByte();
            envelope = bin.ReadByte();
            output = bin.ReadInt32();
        }
    }
}
