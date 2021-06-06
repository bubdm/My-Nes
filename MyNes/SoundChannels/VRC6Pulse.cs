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
    class VRC6Pulse
    {
        private int dutyForm;
        private int dutyStep;
        private bool enabled = true;
        internal bool Outputable;
        private bool mode = false;
        private byte volume;
        private int freqTimer;
        private int frequency;
        private int cycles;

        internal int output;

        internal void HardReset()
        {
            dutyForm = 0;
            dutyStep = 0xF;
            enabled = true;
            mode = false;
            output = 0;
        }
        internal void Write0(ref byte data)
        {
            mode = (data & 0x80) == 0x80;
            dutyForm = ((data & 0x70) >> 4);
            volume = (byte)(data & 0xF);
        }
        internal void Write1(ref byte data)
        {
            frequency = (frequency & 0x0F00) | data;
        }
        internal void Write2(ref byte data)
        {
            frequency = (frequency & 0x00FF) | ((data & 0xF) << 8);
            enabled = (data & 0x80) == 0x80;
        }
        internal void ClockSingle()
        {
            if (--cycles <= 0)
            {
                cycles = (frequency << 1) + 2;

                if (enabled)
                {
                    if (mode)
                        output = volume;
                    else
                    {
                        dutyStep--;
                        if (dutyStep < 0)
                            dutyStep = 0xF;

                        if (dutyStep <= dutyForm)
                            if (Outputable)
                                output = volume;
                            else
                                output = 0;
                        else
                            output = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Save state
        /// </summary>
        /// <param name="stream">The stream that should be used to write data</param>
        internal void SaveState(ref System.IO.BinaryWriter stream)
        {
            stream.Write(dutyForm);
            stream.Write(dutyStep);
            stream.Write(enabled);
            stream.Write(mode);
            stream.Write(volume);
            stream.Write(freqTimer);
            stream.Write(frequency);
            stream.Write(cycles);
        }
        /// <summary>
        /// Load state
        /// </summary>
        /// <param name="stream">The stream that should be used to read data</param>
        internal void LoadState(ref System.IO.BinaryReader stream)
        {
            dutyForm = stream.ReadInt32();
            dutyStep = stream.ReadInt32();
            enabled = stream.ReadBoolean();
            mode = stream.ReadBoolean();
            volume = stream.ReadByte();
            freqTimer = stream.ReadInt32();
            frequency = stream.ReadInt32();
            cycles = stream.ReadInt32();
        }
    }
}
