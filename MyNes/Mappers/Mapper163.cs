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
    [BoardInfo("Unknown", 163)]
    class Mapper163 : Board
    {
        internal override void HardReset()
        {
            base.HardReset();
            security_reg101 = 1;
            security_trigger = false;
            do_chr_ram = true;
            Switch32KPRG(15, PRGArea.Area8000);
            Toggle08KCHR_RAM(true);
        }
        internal byte prg_reg;
        internal byte security_reg;
        internal bool security_trigger;
        internal byte security_reg101;
        internal bool do_chr_ram;

        internal override void WriteEX(ref ushort address, ref byte data)
        {
            if (address == 0x5101)
            {
                if (security_reg101 != 0 && data == 0)
                    security_trigger = true;
                security_reg101 = data;
            }
            else if (address >= 0x5000)
            {
                switch (address & 0x7300)
                {
                    case 0x5000:
                        prg_reg = (byte)((prg_reg & 0xF0) | (data & 0x0F));
                        Switch32KPRG(prg_reg, PRGArea.Area8000);
                        do_chr_ram = (data & 0x80) != 0;
                        if (!do_chr_ram && NesEmu.ppu_clock_v < 128)
                        {
                            Switch08KCHR(0);
                        }
                        break;
                    case 0x5100: if (data == 6) Switch32KPRG(3, PRGArea.Area8000); break;
                    case 0x5200: prg_reg = (byte)((prg_reg & 0x0F) | ((data & 0x0F) << 4)); Switch32KPRG(prg_reg, PRGArea.Area8000); break;
                    case 0x5300: security_reg = data; break;
                }
            }
        }
        internal override void ReadEX(ref ushort addr, out byte val)
        {
            val = 0;
            if (addr >= 0x5000)
            {
                switch (addr & 7700)
                {
                    case 0x5100:
                        {
                            val = security_reg;
                            break;
                        }
                    case 0x5500:
                        {
                            if (security_trigger)
                                val = security_reg;
                            break;
                        }
                }
            }
        }
        internal override void OnPPUScanlineTick()
        {
            base.OnPPUScanlineTick();
            if (do_chr_ram && NesEmu.IsRenderingOn())
            {
                // When turned on, both 4K CHR RAM banks 0000-0FFF and 1000-1FFF map to 0000-0FFF for scanline 240 until scanline 128. 
                // Then at scanline 128, both 4K CHR banks point to 1000-1FFF.
                if (NesEmu.ppu_clock_v == 127)
                {
                    Switch04KCHR(1, CHRArea.Area0000);
                    Switch04KCHR(1, CHRArea.Area1000);
                }
                if (NesEmu.ppu_clock_v == 237)
                {
                    Switch04KCHR(0, CHRArea.Area0000);
                    Switch04KCHR(0, CHRArea.Area1000);
                }

            }
        }
    }
}
