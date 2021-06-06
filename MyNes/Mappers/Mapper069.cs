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
    [BoardInfo("FME-7/Sunsoft 5B", 69)]
    [WithExternalSound]
    class Mapper069 : Board
    {
        private int address_A000;
        private int address_E000;
        private int irq_counter;
        private bool irq_count_enabled;
        private bool irq_trigger_enabled;

        private Sunsoft5BChnl snd_1;
        private Sunsoft5BChnl snd_2;
        private Sunsoft5BChnl snd_3;
        private double[] audio_pulse_table;
        private double[] audio_tnd_table;

        internal override void Initialize(IRom rom)
        {
            base.Initialize(rom);
            snd_1 = new Sunsoft5BChnl();
            snd_2 = new Sunsoft5BChnl();
            snd_3 = new Sunsoft5BChnl();
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
            Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
            address_A000 = 0;
            irq_counter = 0xFFFF;
            irq_count_enabled = false;
            irq_trigger_enabled = false;

            APUApplyChannelsSettings();
            snd_1.HardReset();
            snd_2.HardReset();
            snd_3.HardReset();
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xE000)
            {
                case 0x8000: address_A000 = data & 0xF; break;
                case 0xA000:
                    {
                        switch (address_A000)
                        {
                            case 0x0: Switch01KCHR(data, CHRArea.Area0000); break;
                            case 0x1: Switch01KCHR(data, CHRArea.Area0400); break;
                            case 0x2: Switch01KCHR(data, CHRArea.Area0800); break;
                            case 0x3: Switch01KCHR(data, CHRArea.Area0C00); break;
                            case 0x4: Switch01KCHR(data, CHRArea.Area1000); break;
                            case 0x5: Switch01KCHR(data, CHRArea.Area1400); break;
                            case 0x6: Switch01KCHR(data, CHRArea.Area1800); break;
                            case 0x7: Switch01KCHR(data, CHRArea.Area1C00); break;
                            case 0x8:
                                {
                                    TogglePRGRAMEnable((data & 0x80) == 0x80);
                                    if ((data & 0x40) != 0)
                                    {
                                        Toggle08KPRG_RAM(true, PRGArea.Area6000);
                                        Switch08KPRG((data & 0x3F) & PRG_RAM_08KB_Mask, PRGArea.Area6000);
                                    }
                                    else
                                    {
                                        Toggle08KPRG_RAM(false, PRGArea.Area6000);
                                        Switch08KPRG(data & 0x3F, PRGArea.Area6000);
                                    }
                                    break;
                                }
                            case 0x9: Switch08KPRG(data, PRGArea.Area8000); break;
                            case 0xA: Switch08KPRG(data, PRGArea.AreaA000); break;
                            case 0xB: Switch08KPRG(data, PRGArea.AreaC000); break;
                            case 0xC:
                                {
                                    switch (data & 0x3)
                                    {
                                        case 0: Switch01KNMTFromMirroring(Mirroring.Vert); break;
                                        case 1: Switch01KNMTFromMirroring(Mirroring.Horz); break;
                                        case 2: Switch01KNMTFromMirroring(Mirroring.OneScA); break;
                                        case 3: Switch01KNMTFromMirroring(Mirroring.OneScB); break;
                                    }
                                    break;
                                }
                            case 0xD:
                                {
                                    irq_count_enabled = (data & 0x80) == 0x80;
                                    irq_trigger_enabled = (data & 0x1) == 0x1;
                                    if (!irq_trigger_enabled)
                                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                                    break;
                                }
                            case 0xE: irq_counter = (irq_counter & 0xFF00) | data; break;
                            case 0xF: irq_counter = (irq_counter & 0x00FF) | (data << 8); break;
                        }
                        break;
                    }
                case 0xC000: address_E000 = data & 0xF; break;
                case 0xE000:
                    {
                        switch (address_E000)
                        {
                            case 0x0: snd_1.Write0(ref data); break;
                            case 0x1: snd_1.Write1(ref data); break;
                            case 0x2: snd_2.Write0(ref data); break;
                            case 0x3: snd_2.Write1(ref data); break;
                            case 0x4: snd_3.Write0(ref data); break;
                            case 0x5: snd_3.Write1(ref data); break;
                            case 0x7:
                                {
                                    snd_1.Enabled = (data & 0x1) == 0;
                                    snd_2.Enabled = (data & 0x2) == 0;
                                    snd_3.Enabled = (data & 0x4) == 0;
                                    break;
                                }
                            case 0x8: snd_1.Volume = (byte)(data & 0xF); break;
                            case 0x9: snd_2.Volume = (byte)(data & 0xF); break;
                            case 0xA: snd_3.Volume = (byte)(data & 0xF); break;
                        }
                        break;
                    }
            }
        }
        internal override void OnCPUClock()
        {
            if (irq_count_enabled)
            {
                irq_counter--;
                if (irq_counter <= 0)
                {
                    irq_counter = 0xFFFF;
                    if (irq_trigger_enabled)
                        NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
        }
        internal override double APUGetSample()
        {
            return audio_pulse_table[snd_1.output + snd_2.output] + audio_tnd_table[snd_3.output];
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
            snd_1.Outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_SUN1;
            snd_2.Outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_SUN2;
            snd_3.Outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_SUN3;
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(address_A000);
            stream.Write(address_E000);
            stream.Write(irq_counter);
            stream.Write(irq_count_enabled);
            stream.Write(irq_trigger_enabled);

            snd_1.SaveState(ref stream);
            snd_2.SaveState(ref stream);
            snd_3.SaveState(ref stream);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            address_A000 = stream.ReadInt32();
            address_E000 = stream.ReadInt32();
            irq_counter = stream.ReadInt32();
            irq_count_enabled = stream.ReadBoolean();
            irq_trigger_enabled = stream.ReadBoolean();
            snd_1.LoadState(ref stream);
            snd_2.LoadState(ref stream);
            snd_3.LoadState(ref stream);
        }
    }
}
