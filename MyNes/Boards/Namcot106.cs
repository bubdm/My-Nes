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
    [WithExternalSound]
    abstract class Namcot106 : Board
    {
        private int irq_counter;
        private bool irq_enable;
        private bool disables_chr_ram_A;
        private bool disables_chr_ram_B;
        private bool enable_mirroring_switch;
        private bool enable_N106_sound;
        private int temp_nmt;
        private Namcot106Chnl[] sound_channels;
        private byte soundReg;
        public int enabledChannels;
        private int enabledChannels1;
        private int channelIndex;
        private byte temp_val;
        private byte temp_i;
        public byte[] EXRAM;
        private bool[] sound_channels_enable;
        private double soundOut;
        private int sound_out_div;

        internal override void HardReset()
        {
            base.HardReset();
            EXRAM = new byte[128];
            base.Switch08KPRG(PRG_ROM_08KB_Mask, PRGArea.AreaE000);
            enable_mirroring_switch = enable_N106_sound = this.MapperNumber == 19;
            // This is not a hack, some games should be mapper 210 configured but assigned to mapper 19
            // TODO: find another way to setup Namcot 106 configurations for mapper 210 games
            switch (SHA1.ToUpper())
            {
                case "97E7E61EECB73CB1EA0C15AE51E65EA56301A685":// Wagyan Land 2
                case "3D554F55411AB2DDD1A87E7583E643970DB784F3":// Wagyan Land 3
                case "7FA51058307DB50825C2D3A3A98C0DA554BC3C92":// Dream Master
                case "1C476C795CFC17E987C22FFD6F09BAF1396ED2C9":// Family Circuit '91
                    {
                        enable_mirroring_switch = false;
                        enable_N106_sound = false;
                        break;
                    }
            }
            // TODO: Implement mapper 210 sound channels
            if (enable_N106_sound)
            {
                sound_channels = new Namcot106Chnl[8];
                sound_channels_enable = new bool[8];
                for (int i = 0; i < 8; i++)
                {
                    sound_channels[i] = new Namcot106Chnl(this);
                    sound_channels[i].HardReset();
                }
                soundReg = 0;
                enabledChannels = 0;
                channelIndex = 0;
                APUApplyChannelsSettings();
            }
        }
        internal override void WriteEX(ref ushort address, ref byte data)
        {
            switch (address & 0xF800)
            {
                #region Sound Registers
                case 0x4800:
                    {
                        if (soundReg >= 0x40)
                        {
                            switch (soundReg & 0x7F)
                            {
                                case 0x40: sound_channels[0].WriteA(ref data); break;
                                case 0x42: sound_channels[0].WriteB(ref data); break;
                                case 0x44: sound_channels[0].WriteC(ref data); break;
                                case 0x46: sound_channels[0].WriteD(ref data); break;
                                case 0x47: sound_channels[0].WriteE(ref data); break;
                                case 0x48: sound_channels[1].WriteA(ref data); break;
                                case 0x4A: sound_channels[1].WriteB(ref data); break;
                                case 0x4C: sound_channels[1].WriteC(ref data); break;
                                case 0x4E: sound_channels[1].WriteD(ref data); break;
                                case 0x4F: sound_channels[1].WriteE(ref data); break;
                                case 0x50: sound_channels[2].WriteA(ref data); break;
                                case 0x52: sound_channels[2].WriteB(ref data); break;
                                case 0x54: sound_channels[2].WriteC(ref data); break;
                                case 0x56: sound_channels[2].WriteD(ref data); break;
                                case 0x57: sound_channels[2].WriteE(ref data); break;
                                case 0x58: sound_channels[3].WriteA(ref data); break;
                                case 0x5A: sound_channels[3].WriteB(ref data); break;
                                case 0x5C: sound_channels[3].WriteC(ref data); break;
                                case 0x5E: sound_channels[3].WriteD(ref data); break;
                                case 0x5F: sound_channels[3].WriteE(ref data); break;
                                case 0x60: sound_channels[4].WriteA(ref data); break;
                                case 0x62: sound_channels[4].WriteB(ref data); break;
                                case 0x64: sound_channels[4].WriteC(ref data); break;
                                case 0x66: sound_channels[4].WriteD(ref data); break;
                                case 0x67: sound_channels[4].WriteE(ref data); break;
                                case 0x68: sound_channels[5].WriteA(ref data); break;
                                case 0x6A: sound_channels[5].WriteB(ref data); break;
                                case 0x6C: sound_channels[5].WriteC(ref data); break;
                                case 0x6E: sound_channels[5].WriteD(ref data); break;
                                case 0x6F: sound_channels[5].WriteE(ref data); break;
                                case 0x70: sound_channels[6].WriteA(ref data); break;
                                case 0x72: sound_channels[6].WriteB(ref data); break;
                                case 0x74: sound_channels[6].WriteC(ref data); break;
                                case 0x76: sound_channels[6].WriteD(ref data); break;
                                case 0x77: sound_channels[6].WriteE(ref data); break;
                                case 0x78: sound_channels[7].WriteA(ref data); break;
                                case 0x7A: sound_channels[7].WriteB(ref data); break;
                                case 0x7C: sound_channels[7].WriteC(ref data); break;
                                case 0x7E: sound_channels[7].WriteD(ref data); break;
                                case 0x7F:
                                    {
                                        sound_channels[7].WriteE(ref data);
                                        // Enable channels
                                        enabledChannels = ((data & 0x70) >> 4);
                                        channelIndex = 0;
                                        enabledChannels1 = enabledChannels + 1;
                                        for (temp_i = 7; temp_i >= 0; temp_i--)
                                        {
                                            if (enabledChannels1 > 0)
                                            {
                                                sound_channels[temp_i].Enabled = true;
                                                enabledChannels1--;
                                            }
                                            else
                                                break;
                                        }
                                        break;
                                    }
                            }
                        }
                        EXRAM[soundReg & 0x7F] = data;
                        if ((soundReg & 0x80) == 0x80)
                            soundReg = (byte)(((soundReg + 1) & 0x7F) | 0x80);
                        break;
                    }
                #endregion
                case 0x5000:
                    {
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        irq_counter = (irq_counter & 0x7F00) | data;
                        break;
                    }
                case 0x5800:
                    {
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        irq_counter = (irq_counter & 0x00FF) | ((data & 0x7F) << 8);
                        irq_enable = (data & 0x80) == 0x80;
                        break;
                    }
            }
        }
        internal override void ReadEX(ref ushort address, out byte value)
        {
            switch (address & 0xF800)
            {
                case 0x4800:
                    {
                        value = EXRAM[soundReg & 0x7F];
                        if ((soundReg & 0x80) == 0x80)
                            soundReg = (byte)(((soundReg + 1) & 0x7F) | 0x80);

                        break;
                    }
                case 0x5000: NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; value = (byte)(irq_counter & 0x00FF); break;
                case 0x5800: NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; value = (byte)((irq_enable ? 0x80 : 0x00) | ((irq_counter & 0x7F00) >> 8)); break;
                default: value = 0; break;
            }
        }
        internal override void WritePRG(ref ushort address, ref byte data)
        {
            switch (address & 0xF800)
            {
                case 0x8000:
                    {
                        if (!disables_chr_ram_A)
                        {
                            Toggle01KCHR_RAM(data >= 0xE0, CHRArea.Area0000);
                            Switch01KCHR((data >= 0xE0) ? (data - 0xE0) : data, CHRArea.Area0000);
                        }
                        else
                        {
                            Toggle01KCHR_RAM(false, CHRArea.Area0000);
                            base.Switch01KCHR(data, CHRArea.Area0000);
                        }
                        break;
                    }
                case 0x8800:
                    {
                        if (!disables_chr_ram_A)
                        {
                            Toggle01KCHR_RAM(data >= 0xE0, CHRArea.Area0400);
                            base.Switch01KCHR((data >= 0xE0) ? (data - 0xE0) : data, CHRArea.Area0400);
                        }
                        else
                        {
                            Toggle01KCHR_RAM(false, CHRArea.Area0400);
                            base.Switch01KCHR(data, CHRArea.Area0400);
                        }
                        break;
                    }
                case 0x9000:
                    {
                        if (!disables_chr_ram_A)
                        {
                            Toggle01KCHR_RAM(data >= 0xE0, CHRArea.Area0800);
                            base.Switch01KCHR((data >= 0xE0) ? (data - 0xE0) : data, CHRArea.Area0800);
                        }
                        else
                        {
                            Toggle01KCHR_RAM(false, CHRArea.Area0800);
                            base.Switch01KCHR(data, CHRArea.Area0800);
                        }
                        break;
                    }
                case 0x9800:
                    {
                        if (!disables_chr_ram_A)
                        {
                            Toggle01KCHR_RAM(data >= 0xE0, CHRArea.Area0C00);
                            base.Switch01KCHR((data >= 0xE0) ? (data - 0xE0) : data, CHRArea.Area0C00);
                        }
                        else
                        {
                            Toggle01KCHR_RAM(false, CHRArea.Area0C00);
                            base.Switch01KCHR(data, CHRArea.Area0C00);
                        }
                        break;
                    }
                case 0xA000:
                    {
                        if (!disables_chr_ram_B)
                        {
                            Toggle01KCHR_RAM(data >= 0xE0, CHRArea.Area1000);
                            base.Switch01KCHR((data >= 0xE0) ? (data - 0xE0) : data, CHRArea.Area1000);
                        }
                        else
                        {
                            Toggle01KCHR_RAM(false, CHRArea.Area1000);
                            base.Switch01KCHR(data, CHRArea.Area1000);
                        }
                        break;
                    }
                case 0xA800:
                    {
                        if (!disables_chr_ram_B)
                        {
                            Toggle01KCHR_RAM(data >= 0xE0, CHRArea.Area1400);
                            base.Switch01KCHR((data >= 0xE0) ? (data - 0xE0) : data, CHRArea.Area1400);
                        }
                        else
                        {
                            Toggle01KCHR_RAM(false, CHRArea.Area1400);
                            base.Switch01KCHR(data, CHRArea.Area1400);
                        }
                        break;
                    }
                case 0xB000:
                    {
                        if (!disables_chr_ram_B)
                        {
                            Toggle01KCHR_RAM(data >= 0xE0, CHRArea.Area1800);
                            base.Switch01KCHR((data >= 0xE0) ? (data - 0xE0) : data, CHRArea.Area1800);
                        }
                        else
                        {
                            Toggle01KCHR_RAM(false, CHRArea.Area1800);
                            base.Switch01KCHR(data, CHRArea.Area1800);
                        }
                        break;
                    }
                case 0xB800:
                    {
                        if (!disables_chr_ram_B)
                        {
                            Toggle01KCHR_RAM(data >= 0xE0, CHRArea.Area1C00);
                            base.Switch01KCHR((data >= 0xE0) ? (data - 0xE0) : data, CHRArea.Area1C00);
                        }
                        else
                        {
                            Toggle01KCHR_RAM(false, CHRArea.Area1C00);
                            base.Switch01KCHR(data, CHRArea.Area1C00);
                        }
                        break;
                    }
                case 0xC000:
                    {
                        if (enable_mirroring_switch)
                        {
                            NMT_AREA_BLK_INDEX[0] = data;
                        }
                        break;
                    }
                case 0xC800:
                    {
                        if (enable_mirroring_switch)
                        {
                            NMT_AREA_BLK_INDEX[1] = data;
                        }
                        break;
                    }
                case 0xD000:
                    {
                        if (enable_mirroring_switch)
                        {
                            NMT_AREA_BLK_INDEX[2] = data;
                        }
                        break;
                    }
                case 0xD800:
                    {
                        if (enable_mirroring_switch)
                        {
                            NMT_AREA_BLK_INDEX[3] = data;
                        }
                        break;
                    }
                case 0xE000: base.Switch08KPRG(data & 0x3F, PRGArea.Area8000); break;
                case 0xE800:
                    {
                        base.Switch08KPRG(data & 0x3F, PRGArea.AreaA000);
                        disables_chr_ram_A = (data & 0x40) == 0x40;
                        disables_chr_ram_B = (data & 0x80) == 0x80;
                        break;
                    }
                case 0xF000: base.Switch08KPRG(data & 0x3F, PRGArea.AreaC000); break;
                case 0xF800:
                    {
                        soundReg = data;
                        break;
                    }
            }
        }
        internal override void ReadNMT(ref ushort address, out byte data)
        {
            if (enable_mirroring_switch)
            {
                temp_nmt = NMT_AREA_BLK_INDEX[(address >> 10) & 0x3];
                if (temp_nmt >= 0xE0)
                    data = NMT_RAM[(temp_nmt - 0xE0) & 0x1][address & 0x3FF];
                else
                    data = CHR_ROM[temp_nmt][address & 0x3FF];
            }
            else
                base.ReadNMT(ref address, out data);
        }
        internal override void WriteNMT(ref ushort address, ref byte data)
        {
            if (enable_mirroring_switch)
            {
                temp_nmt = NMT_AREA_BLK_INDEX[(address >> 10) & 0x3];
                if (temp_nmt >= 0xE0)
                    NMT_RAM[(temp_nmt - 0xE0) & 0x1][address & 0x3FF] = data;
            }
            else
                base.WriteNMT(ref address, ref data);
        }
        internal override void OnCPUClock()
        {
            if (irq_enable)
            {
                if (irq_counter == 0x7FFF)
                {
                    NesEmu.IRQFlags |= NesEmu.IRQ_BOARD; irq_counter = 0;
                }
                else
                {
                    irq_counter++;
                }
            }
        }
        internal override void OnAPUClockSingle()
        {
            if (sound_channels != null)
                for (int i = 0; i < sound_channels.Length; i++)
                    sound_channels[i].ClockSingle();
        }
        internal override double APUGetSample()
        {
            // TODO: Namcot sound channels mixer !
            soundOut = 0;
            sound_out_div = 0;
            if (enabledChannels > 0)
            {
                for (int i = 0; i < sound_channels.Length; i++)
                    if (sound_channels[i].Enabled && sound_channels_enable[i])
                    {
                        if (sound_channels[i].clocks > 0)
                            sound_channels[i].output = sound_channels[i].output_av / sound_channels[i].clocks;
                        sound_channels[i].clocks = sound_channels[i].output_av = 0;

                        soundOut += sound_channels[i].output;
                        sound_out_div++;
                    }
                soundOut = (soundOut / 8) / 225;

                // each x from 1.0
                // each y from M (Max output value = max output of single channel * enabled channels count)
                // x = y * 1.0 / M
                // Then the output value (x) should be ranged 0 - 1.0
                // In apu mixing, this value will be added to the internal
                // sound channels mixer which output sound range 0 - 1.0
                // After this mix, the max gain value will be 0 - 2.0
            }
            return soundOut;
        }
        internal override void APUApplyChannelsSettings()
        {
            base.APUApplyChannelsSettings();
            sound_channels_enable[0] = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT1;
            sound_channels_enable[1] = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT2;
            sound_channels_enable[2] = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT3;
            sound_channels_enable[3] = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT4;
            sound_channels_enable[4] = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT5;
            sound_channels_enable[5] = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT6;
            sound_channels_enable[6] = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT7;
            sound_channels_enable[7] = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT8;
        }
        internal override void WriteStateData(ref System.IO.BinaryWriter stream)
        {
            base.WriteStateData(ref stream);
            stream.Write(irq_counter);
            stream.Write(irq_enable);
            stream.Write(disables_chr_ram_A);
            stream.Write(disables_chr_ram_B);
            stream.Write(enable_mirroring_switch);
            stream.Write(enable_N106_sound);
            stream.Write(temp_nmt);
            if (enable_N106_sound)
                for (int i = 0; i < sound_channels.Length; i++)
                    sound_channels[i].SaveState(stream);
            stream.Write(soundReg);
            stream.Write(enabledChannels);
            stream.Write(enabledChannels1);
            stream.Write(channelIndex);
            stream.Write(temp_val);
            stream.Write(temp_i);
            stream.Write(EXRAM);
        }
        internal override void ReadStateData(ref System.IO.BinaryReader stream)
        {
            base.ReadStateData(ref stream);
            irq_counter = stream.ReadInt32();
            irq_enable = stream.ReadBoolean();
            disables_chr_ram_A = stream.ReadBoolean();
            disables_chr_ram_B = stream.ReadBoolean();
            enable_mirroring_switch = stream.ReadBoolean();
            enable_N106_sound = stream.ReadBoolean();
            temp_nmt = stream.ReadInt32();
            if (enable_N106_sound)
                for (int i = 0; i < sound_channels.Length; i++)
                    sound_channels[i].LoadState(stream);
            soundReg = stream.ReadByte();
            enabledChannels = stream.ReadInt32();
            enabledChannels1 = stream.ReadInt32();
            channelIndex = stream.ReadInt32();
            temp_val = stream.ReadByte();
            temp_i = stream.ReadByte();
            stream.Read(EXRAM, 0, EXRAM.Length);
        }
    }
}
