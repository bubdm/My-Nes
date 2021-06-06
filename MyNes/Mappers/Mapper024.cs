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
    [BoardInfo("VRC6", 24)]
    [WithExternalSound]
    class Mapper024 : Board
    {
        private int irq_reload;
        private int irq_counter;
        private int prescaler;
        private bool irq_mode_cycle;
        private bool irq_enable;
        private bool irq_enable_on_ak;

        private VRC6Pulse snd_1;
        private VRC6Pulse snd_2;
        private VRC6Sawtooth snd_3;
        private double[] audio_pulse_table;
        private double[] audio_tnd_table;

        internal override void Initialize(IRom rom)
        {
            base.Initialize(rom);
            snd_1 = new VRC6Pulse();
            snd_2 = new VRC6Pulse();
            snd_3 = new VRC6Sawtooth();
            audio_pulse_table = new double[32];
            for (int n = 0; n < 32; n++)
            {
                audio_pulse_table[n] = 95.52 / (8128.0 / n + 100);
            }
            audio_tnd_table = new double[204];
            for (int n = 0; n < 204; n++)
            {
                audio_tnd_table[n] = 163.67 / (24329.0 / n + 100);
            }
        }
        internal override void HardReset()
        {
            base.HardReset();
            base.Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
            APUApplyChannelsSettings();
            snd_1.HardReset();
            snd_2.HardReset();
            snd_3.HardReset();
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address)
            {
                case 0x8000:
                case 0x8001:
                case 0x8002:
                case 0x8003: Switch16KPRG(data, PRGArea.Area8000); break;
                case 0x9000: snd_1.Write0(ref data); break;
                case 0x9001: snd_1.Write1(ref data); break;
                case 0x9002: snd_1.Write2(ref data); break;
                case 0xA000: snd_2.Write0(ref data); break;
                case 0xA001: snd_2.Write1(ref data); break;
                case 0xA002: snd_2.Write2(ref data); break;
                case 0xB000: snd_3.Write0(ref data); break;
                case 0xB001: snd_3.Write1(ref data); break;
                case 0xB002: snd_3.Write2(ref data); break;
                case 0xB003:
                    {
                        switch ((data & 0xC) >> 2)
                        {
                            case 0: Switch01KNMTFromMirroring(Mirroring.Vert); break;
                            case 1: Switch01KNMTFromMirroring(Mirroring.Horz); break;
                            case 2: Switch01KNMTFromMirroring(Mirroring.OneScA); break;
                            case 3: Switch01KNMTFromMirroring(Mirroring.OneScB); break;
                        }
                        break;
                    }
                case 0xC000:
                case 0xC001:
                case 0xC002:
                case 0xC003: Switch08KPRG(data, PRGArea.AreaC000); break;
                case 0xD000: Switch01KCHR(data, CHRArea.Area0000); break;
                case 0xD001: Switch01KCHR(data, CHRArea.Area0400); break;
                case 0xD002: Switch01KCHR(data, CHRArea.Area0800); break;
                case 0xD003: Switch01KCHR(data, CHRArea.Area0C00); break;
                case 0xE000: Switch01KCHR(data, CHRArea.Area1000); break;
                case 0xE001: Switch01KCHR(data, CHRArea.Area1400); break;
                case 0xE002: Switch01KCHR(data, CHRArea.Area1800); break;
                case 0xE003: Switch01KCHR(data, CHRArea.Area1C00); break;
                case 0xF000: irq_reload = data; break;
                case 0xF001:
                    {
                        irq_mode_cycle = (data & 0x4) == 0x4;
                        irq_enable = (data & 0x2) == 0x2;
                        irq_enable_on_ak = (data & 0x1) == 0x1;
                        if (irq_enable)
                        {
                            irq_counter = irq_reload;
                            prescaler = 341;
                        }
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0xF002: NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; irq_enable = irq_enable_on_ak; break;
            }
        }
        internal override void OnCPUClock()
        {
            if (irq_enable)
            {
                if (!irq_mode_cycle)
                {
                    if (prescaler > 0)
                        prescaler -= 3;
                    else
                    {
                        prescaler = 341;
                        irq_counter++;
                        if (irq_counter == 0xFF)
                        {
                            NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                            irq_counter = irq_reload;
                        }
                    }
                }
                else
                {
                    irq_counter++;
                    if (irq_counter == 0xFF)
                    {
                        NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                        irq_counter = irq_reload;
                    }
                }
            }
        }
        internal override void OnAPUClockSingle()
        {
            base.OnAPUClockSingle();
            snd_1.ClockSingle();
            snd_2.ClockSingle();
            snd_3.ClockSingle();
        }
        internal override void APUApplyChannelsSettings()
        {
            base.APUApplyChannelsSettings();
            snd_1.Outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_VRC6_SQ1;
            snd_2.Outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_VRC6_SQ2;
            snd_3.Outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_VRC6_SAW;
        }
        internal override double APUGetSample()
        {
            return audio_pulse_table[snd_1.output + snd_2.output] + audio_tnd_table[snd_3.output];
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(irq_reload);
            stream.Write(irq_counter);
            stream.Write(prescaler);
            stream.Write(irq_mode_cycle);
            stream.Write(irq_enable);
            stream.Write(irq_enable_on_ak);

            snd_1.SaveState(ref stream);
            snd_2.SaveState(ref stream);
            snd_3.SaveState(ref stream);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            irq_reload = stream.ReadInt32();
            irq_counter = stream.ReadInt32();
            prescaler = stream.ReadInt32();
            irq_mode_cycle = stream.ReadBoolean();
            irq_enable = stream.ReadBoolean();
            irq_enable_on_ak = stream.ReadBoolean();

            snd_1.LoadState(ref stream);
            snd_2.LoadState(ref stream);
            snd_3.LoadState(ref stream);
        }
    }
}
