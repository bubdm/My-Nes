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
    [BoardInfo("Taito TC0190/TC0350", 33)]
    [HassIssues]
    class Mapper033 : Board
    {
        private bool MODE;// Mapper 33 [TC0350FMR] mode ?
        private bool irq_enabled;
        private byte irq_counter;
        private int old_irq_counter;
        private byte irq_reload;
        private bool irq_clear;
        private bool mmc3_alt_behavior;

        internal override string Issues { get { return MNInterfaceLanguage.IssueMapper33; } }
        internal override void HardReset()
        {
            base.HardReset();
            base.Switch16KPRG(PRG_ROM_16KB_Mask, PRGArea.AreaC000);
            // This is not a hack, some games are mapper 48 and assigned as mapper 33
            // We need to confirm which type given game it is ...
            MODE = true;// Set as mapper 33 mode [board TC0350XXX]
            if (IsGameFoundOnDB)
            {
                foreach (string chip in Chips)
                {
                    if (chip.Contains("TC0190"))
                    {
                        // Board TC0190XXX mode, mapper 48 ....
                        MODE = false;
                        ppuA12TogglesOnRaisingEdge = true;
                        enabled_ppuA12ToggleTimer = true;
                        break;
                    }
                }
            }
            // IRQ
            irq_enabled = false;
            irq_counter = 0;
            irq_reload = 0xFF;
            old_irq_counter = 0;
            mmc3_alt_behavior = false;
            irq_clear = false;
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            if (!MODE)
            {
                // Mapper 48 mode
                switch (address & 0xE003)
                {
                    case 0x8000: base.Switch08KPRG(data, PRGArea.Area8000); break;
                    case 0x8001: base.Switch08KPRG(data, PRGArea.AreaA000); break;
                    case 0x8002: base.Switch02KCHR(data, CHRArea.Area0000); break;
                    case 0x8003: base.Switch02KCHR(data, CHRArea.Area0800); break;
                    case 0xA000: base.Switch01KCHR(data, CHRArea.Area1000); break;
                    case 0xA001: base.Switch01KCHR(data, CHRArea.Area1400); break;
                    case 0xA002: base.Switch01KCHR(data, CHRArea.Area1800); break;
                    case 0xA003: base.Switch01KCHR(data, CHRArea.Area1C00); break;
                    case 0xC000: irq_reload = (byte)(data ^ 0xFF); break;
                    case 0xC001: if (mmc3_alt_behavior) irq_clear = true; irq_counter = 0; break;
                    case 0xC002: irq_enabled = false; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                    case 0xC003: irq_enabled = true; break;
                    case 0xE000: Switch01KNMTFromMirroring((data & 0x40) == 0x40 ? Mirroring.Horz : Mirroring.Vert); break;
                }
            }
            else
            {
                // Mapper 33 mode
                switch (address & 0xA003)
                {
                    case 0x8000:
                        {
                            base.Switch01KNMTFromMirroring((data & 0x40) == 0x40 ? Mirroring.Horz : Mirroring.Vert);
                            base.Switch08KPRG((data & 0x3F), PRGArea.Area8000);
                            break;
                        }
                    case 0x8001: base.Switch08KPRG((data & 0x3F), PRGArea.AreaA000); break;
                    case 0x8002: base.Switch02KCHR(data, CHRArea.Area0000); break;
                    case 0x8003: base.Switch02KCHR(data, CHRArea.Area0800); break;
                    case 0xA000: base.Switch01KCHR(data, CHRArea.Area1000); break;
                    case 0xA001: base.Switch01KCHR(data, CHRArea.Area1400); break;
                    case 0xA002: base.Switch01KCHR(data, CHRArea.Area1800); break;
                    case 0xA003: base.Switch01KCHR(data, CHRArea.Area1C00); break;
                }
            }
        }
        // The scanline timer, clocked on PPU A12 raising edge ...
        internal override void OnPPUA12RaisingEdge()
        {
            if (MODE) return;
            old_irq_counter = irq_counter;

            if (irq_counter == 0 || irq_clear)
                irq_counter = irq_reload;
            else
                irq_counter = (byte)(irq_counter - 1);

            if ((!mmc3_alt_behavior || old_irq_counter != 0 || irq_clear) && irq_counter == 0 && irq_enabled)
                NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;

            irq_clear = false;
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(irq_enabled);
            stream.Write(irq_counter);
            stream.Write(old_irq_counter);
            stream.Write(irq_reload);
            stream.Write(irq_clear);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            irq_enabled = stream.ReadBoolean();
            irq_counter = stream.ReadByte();
            old_irq_counter = stream.ReadInt32();
            irq_reload = stream.ReadByte();
            irq_clear = stream.ReadBoolean();
        }
    }
}
