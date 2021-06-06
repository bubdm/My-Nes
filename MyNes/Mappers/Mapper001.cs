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
    [BoardInfo("MMC1", 1, 4, 64)]
    class Mapper001 : Board
    {
        private int address_reg;
        private byte[] reg = new byte[4];
        private byte shift = 0;
        private byte buffer = 0;
        private bool flag_p;
        private bool flag_c;
        private bool flag_s;
        private bool enable_wram_enable;
        private int prg_hijackedbit;
        private bool use_hijacked;
        private bool use_sram_switch;
        private int sram_switch_mask;
        private int cpuCycles;

        internal override void HardReset()
        {
            base.HardReset();
            cpuCycles = 0;
            // Registers
            address_reg = 0;
            reg = new byte[4];
            reg[0] = 0x0C;
            flag_c = false;
            flag_s = flag_p = true;
            prg_hijackedbit = 0;
            reg[1] = reg[2] = reg[3] = 0;
            // Buffers
            buffer = 0;
            shift = 0;
            if (base.Chips.Contains("MMC1B") || base.Chips.Contains("MMC1B2"))
            {
                base.TogglePRGRAMEnable(false);
                System.Console.WriteLine("MMC1: SRAM Disabled.");
            }
            enable_wram_enable = !base.Chips.Contains("MMC1A");
            System.Console.WriteLine("MMC1: enable_wram_enable = " + enable_wram_enable);
            //  use hijacked
            use_hijacked = (PRG_ROM_16KB_Mask & 0x10) == 0x10;
            if (use_hijacked)
                prg_hijackedbit = 0x10;
            // SRAM Switch ?
            use_sram_switch = false;
            if (PRG_RAM_08KB_Count > 0)
            {
                use_sram_switch = true;
                sram_switch_mask = use_hijacked ? 0x08 : 0x18;
                sram_switch_mask &= PRG_RAM_08KB_Mask << 3;

                if (sram_switch_mask == 0)
                    use_sram_switch = false;
            }
            base.Switch16KPRG(0xF | prg_hijackedbit, PRGArea.AreaC000);
            System.Console.WriteLine("MMC1: use_hijacked = " + use_hijacked.ToString());
            System.Console.WriteLine("MMC1: use_sram_switch = " + use_sram_switch.ToString());
            System.Console.WriteLine("MMC1: sram_switch_mask = " + sram_switch_mask.ToString("X2"));
        }
        internal override void WritePRG(ref ushort address, ref byte value)
        {
            // Too close writes ignored !
            if (cpuCycles > 0)
            {
                return;
            }
            cpuCycles = 3;// Make save cycles ...
            //Temporary reg port ($8000-FFFF):
            //[r... ...d]
            //r = reset flag
            //d = data bit

            //r is set
            if ((value & 0x80) == 0x80)
            {
                reg[0] |= 0x0C;//bits 2,3 of reg $8000 are set (16k PRG mode, $8000 swappable)
                flag_s = flag_p = true;
                shift = buffer = 0;//hidden temporary reg is reset
                return;
            }
            //d is set
            if ((value & 0x01) == 0x01)
                buffer |= (byte)(1 << shift);//'d' proceeds as the next bit written in the 5-bit sequence
            if (++shift < 5)
                return;
            // If this completes the 5-bit sequence:
            // - temporary reg is copied to actual internal reg (which reg depends on the last address written to)
            address_reg = (address & 0x7FFF) >> 13;
            reg[address_reg] = buffer;

            // - temporary reg is reset (so that next write is the "first" write)
            shift = buffer = 0;

            // Update internal registers ...
            switch (address_reg)
            {
                case 0:// $8000-9FFF [Flags and mirroring]
                    {
                        // Flags
                        flag_c = (reg[0] & 0x10) != 0;
                        flag_p = (reg[0] & 0x08) != 0;
                        flag_s = (reg[0] & 0x04) != 0;
                        UpdatePRG();
                        UpdateCHR();
                        // Mirroring
                        switch (reg[0] & 3)
                        {
                            case 0: base.Switch01KNMTFromMirroring(Mirroring.OneScA); break;
                            case 1: base.Switch01KNMTFromMirroring(Mirroring.OneScB); break;
                            case 2: base.Switch01KNMTFromMirroring(Mirroring.Vert); break;
                            case 3: base.Switch01KNMTFromMirroring(Mirroring.Horz); break;
                        }
                        break;
                    }
                case 1:// $A000-BFFF [CHR REG 0]
                    {
                        // CHR
                        if (!flag_c)
                            base.Switch08KCHR(reg[1] >> 1);
                        else
                            base.Switch04KCHR(reg[1], 0x0000);
                        // SRAM
                        if (use_sram_switch)
                            base.Switch08KPRG((reg[1] & sram_switch_mask) >> 3, PRGArea.Area6000);
                        // PRG hijack
                        if (use_hijacked)
                        { prg_hijackedbit = reg[1] & 0x10; UpdatePRG(); }
                        break;
                    }
                case 2:// $C000-DFFF [CHR REG 1]
                    {
                        // CHR 
                        if (flag_c)
                            base.Switch04KCHR(reg[2], CHRArea.Area1000);
                        // SRAM
                        if (use_sram_switch)
                            base.Switch08KPRG((reg[2] & sram_switch_mask) >> 3, PRGArea.Area6000);
                        // PRG hijack
                        if (use_hijacked)
                        { prg_hijackedbit = reg[2] & 0x10; UpdatePRG(); }
                        break;
                    }
                case 3:// $E000-FFFF [PRG REG]
                    {
                        if (enable_wram_enable)
                            base.TogglePRGRAMEnable((reg[3] & 0x10) == 0);
                        UpdatePRG();
                        break;
                    }
            }
        }
        private void UpdateCHR()
        {
            if (!flag_c)
                base.Switch08KCHR(reg[1] >> 1);
            else
            {
                base.Switch04KCHR(reg[1], CHRArea.Area0000);
                base.Switch04KCHR(reg[2], CHRArea.Area1000);
            }
            // SRAM
            if (use_sram_switch)
                base.Switch08KPRG((reg[1] & sram_switch_mask) >> 3, PRGArea.Area6000);
        }
        private void UpdatePRG()
        {
            if (!flag_p)
            {
                base.Switch32KPRG(((reg[3] & 0xF) | prg_hijackedbit) >> 1, PRGArea.Area8000);
            }
            else
            {
                if (flag_s)
                {
                    base.Switch16KPRG((reg[3] & 0xF) | prg_hijackedbit, PRGArea.Area8000);
                    base.Switch16KPRG(0xF | prg_hijackedbit, PRGArea.AreaC000);
                }
                else
                {
                    base.Switch16KPRG(prg_hijackedbit, PRGArea.Area8000);
                    base.Switch16KPRG((reg[3] & 0xF) | prg_hijackedbit, PRGArea.AreaC000);
                }
            }
        }
        internal override void OnCPUClock()
        {
            if (cpuCycles > 0)
                cpuCycles--;
        }

        internal override void WriteStateData(ref BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(reg);
            stream.Write(shift);
            stream.Write(buffer);
            stream.Write(flag_p);
            stream.Write(flag_c);
            stream.Write(flag_s);
            stream.Write(enable_wram_enable);
            stream.Write(prg_hijackedbit);
            stream.Write(use_hijacked);
            stream.Write(use_sram_switch);
            stream.Write(cpuCycles);
        }
        internal override void ReadStateData(ref BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            stream.Read(reg, 0, reg.Length);
            shift = stream.ReadByte();
            buffer = stream.ReadByte();
            flag_p = stream.ReadBoolean();
            flag_c = stream.ReadBoolean();
            flag_s = stream.ReadBoolean();
            enable_wram_enable = stream.ReadBoolean();
            prg_hijackedbit = stream.ReadInt32();
            use_hijacked = stream.ReadBoolean();
            use_sram_switch = stream.ReadBoolean();
            cpuCycles = stream.ReadInt32();
        }
    }
}
