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
    class Sunsoft5BChnl
    {
        internal bool Enabled;
        internal byte Volume;
        private int dutyStep = 0;
        private int freqTimer;
        private int frequency;
        private int cycles;
        internal int output;
        internal bool Outputable;

        internal void HardReset() { }
        internal void SoftReset() { }
        internal void Write0(ref byte data)
        {
            frequency = (frequency & 0x0F00) | data;
            freqTimer = (frequency + 1) * 2;
        }
        internal void Write1(ref byte data)
        {
            frequency = (frequency & 0x00FF) | ((data & 0xF) << 8);
            freqTimer = (frequency + 1) * 2;
        }

        internal void ClockSingle()
        {
            if (--cycles <= 0)
            {
                cycles = freqTimer;
                dutyStep = (dutyStep + 1) & 0x1F;

                if (dutyStep <= 15)
                {
                    if (Enabled)
                        if (Outputable)
                            output = Volume;
                        else
                            output = 0;
                    else
                        output = 0;
                }
                else
                    output = 0;
            }
        }
        /// <summary>
        /// Save state
        /// </summary>
        /// <param name="stream">The stream that should be used to write data</param>
        internal void SaveState(ref System.IO.BinaryWriter stream)
        {
            stream.Write(Enabled);
            stream.Write(Volume);
            stream.Write(dutyStep);
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
            Enabled = stream.ReadBoolean();
            Volume = stream.ReadByte();
            dutyStep = stream.ReadInt32();
            freqTimer = stream.ReadInt32();
            frequency = stream.ReadInt32();
            cycles = stream.ReadInt32();
        }
    }
}
