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
using System;

namespace MyNes.Core
{
    /*APU section*/
    public partial class NesEmu
    {
        // DATA REG
        private static byte apu_reg_io_db;// The data bus
        private static byte apu_reg_io_addr;// The address bus
        private static bool apu_reg_access_happened;// Triggers when cpu accesses apu bus.
        private static bool apu_reg_access_w;// True= write access, False= Read access.
        private static Action[] apu_reg_update_func;
        private static Action[] apu_reg_read_func;
        private static Action[] apu_reg_write_func;

        private static bool apu_odd_cycle;
        private static bool apu_irq_enabled;
        private static bool apu_irq_flag;
        private static bool apu_irq_do_it;
        internal static bool apu_irq_delta_occur;

        private static bool apu_seq_mode;
        private static int apu_ferq_f;// IRQ clock frequency
        private static int apu_ferq_l;// Length counter clock
        private static int apu_ferq_e;// Envelope counter clock
        private static int apu_cycle_f;
        private static int apu_cycle_f_t;
        private static int apu_cycle_e;
        private static int apu_cycle_l;
        private static bool apu_odd_l;
        private static bool apu_check_irq;
        private static bool apu_do_env;
        private static bool apu_do_length;
        /*Playback*/
        public static bool SoundEnabled;
        // DAC
        public static double audio_playback_amplitude = 1.5;// 256
        public static int audio_playback_peek_limit = 124;
        private static bool audio_playback_dac_initialized;

        public static double cpu_clock_per_frame;
        internal static double apu_target_samples_count_per_frame;
        private static short[] audio_samples;
        private static int audio_w_pos;
        private static int audio_samples_added;
        internal static int audio_samples_count;
        private static int[][][][][] mix_table;

        // Output values
        private static double audio_x, audio_y;
        public static double audio_timer_ratio = 40;
        private static double audio_timer;


        private static SoundLowPassFilter audio_low_pass_filter_14K;
        private static SoundHighPassFilter audio_high_pass_filter_90;
        private static SoundHighPassFilter audio_high_pass_filter_440;

        private static bool audio_sq1_outputable;
        private static bool audio_sq2_outputable;
        private static bool audio_nos_outputable;
        private static bool audio_trl_outputable;
        private static bool audio_dmc_outputable;

        // External sound channels activation
        private static bool apu_use_external_sound;

        private static void APUInitialize()
        {
            apu_reg_update_func = new Action[0x20];
            apu_reg_read_func = new Action[0x20];
            apu_reg_write_func = new Action[0x20];
            for (int i = 0; i < 0x20; i++)
            {
                apu_reg_update_func[i] = APUBlankAccess;
                apu_reg_read_func[i] = APUBlankAccess;
                apu_reg_write_func[i] = APUBlankAccess;
            }

            apu_reg_update_func[0x00] = APUOnRegister4000;
            apu_reg_update_func[0x01] = APUOnRegister4001;
            apu_reg_update_func[0x02] = APUOnRegister4002;
            apu_reg_update_func[0x03] = APUOnRegister4003;
            apu_reg_update_func[0x04] = APUOnRegister4004;
            apu_reg_update_func[0x05] = APUOnRegister4005;
            apu_reg_update_func[0x06] = APUOnRegister4006;
            apu_reg_update_func[0x07] = APUOnRegister4007;
            apu_reg_update_func[0x08] = APUOnRegister4008;
            apu_reg_update_func[0x09] = APUOnRegister4009;
            apu_reg_update_func[0x0A] = APUOnRegister400A;
            apu_reg_update_func[0x0B] = APUOnRegister400B;
            apu_reg_update_func[0x0C] = APUOnRegister400C;
            apu_reg_update_func[0x0D] = APUOnRegister400D;
            apu_reg_update_func[0x0E] = APUOnRegister400E;
            apu_reg_update_func[0x0F] = APUOnRegister400F;
            apu_reg_update_func[0x10] = APUOnRegister4010;
            apu_reg_update_func[0x11] = APUOnRegister4011;
            apu_reg_update_func[0x12] = APUOnRegister4012;
            apu_reg_update_func[0x13] = APUOnRegister4013;
            //apu_reg_update_func[0x14] = APUOnRegister4014;
            apu_reg_update_func[0x15] = APUOnRegister4015;
            apu_reg_update_func[0x16] = APUOnRegister4016;
            apu_reg_update_func[0x17] = APUOnRegister4017;

            apu_reg_read_func[0x15] = APURead4015;
            apu_reg_read_func[0x16] = APURead4016;
            apu_reg_read_func[0x17] = APURead4017;

            apu_reg_write_func[0x14] = APUWrite4014;
            apu_reg_write_func[0x15] = APUWrite4015;

            audio_low_pass_filter_14K = new SoundLowPassFilter(0.00815686);// 14KHz
            audio_high_pass_filter_90 = new SoundHighPassFilter(0.999835);// 90 Hz
            audio_high_pass_filter_440 = new SoundHighPassFilter(0.996039);// 440 Hz
        }
        public static void ApplyAudioSettings(bool all = true)
        {
            SoundEnabled = MyNesMain.RendererSettings.Audio_SoundEnabled;
            // Channel outputs
            audio_sq1_outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_SQ1;
            audio_sq2_outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_SQ2;
            audio_nos_outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_NOZ;
            audio_trl_outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_TRL;
            audio_dmc_outputable = MyNesMain.RendererSettings.Audio_ChannelEnabled_DMC;

            if (apu_use_external_sound)
                mem_board.APUApplyChannelsSettings();
            if (all)
                CalculateAudioPlaybackValues();
        }
        private static void APUHardReset()
        {
            apu_reg_io_db = 0;
            apu_reg_io_addr = 0;
            apu_reg_access_happened = false;
            apu_reg_access_w = false;
            apu_seq_mode = false;
            apu_odd_cycle = true;
            apu_cycle_f_t = 0;
            apu_cycle_e = 4;
            apu_cycle_f = 4;
            apu_cycle_l = 4;
            apu_odd_l = false;
            apu_check_irq = false;
            apu_do_env = false;
            apu_do_length = false;
            switch (Region)
            {
                case EmuRegion.NTSC:
                    {
                        cpu_clock_per_frame = 29780.5;// 29780.5 ?
                        apu_ferq_f = 14914;
                        apu_ferq_e = 3728;
                        apu_ferq_l = 7456;
                        break;
                    }
                // TODO: define APU clock frequencies at PAL/DENDY
                case EmuRegion.PALB:
                    {
                        cpu_clock_per_frame = 33247.5;// 33247.5 ?
                        apu_ferq_f = 14914;
                        apu_ferq_e = 3728;
                        apu_ferq_l = 7456;
                        break;
                    }
                case EmuRegion.DENDY:
                    {
                        cpu_clock_per_frame = 35464;
                        apu_ferq_f = 14914;
                        apu_ferq_e = 3728;
                        apu_ferq_l = 7456;
                        break;
                    }
            }

            SQ1HardReset();
            SQ2HardReset();
            NOSHardReset();
            DMCHardReset();
            TRLHardReset();

            apu_irq_enabled = true;
            apu_irq_flag = false;
            reg_2004 = 0x2004;
            CalculateAudioPlaybackValues();
            // External sound channel activation
            apu_use_external_sound = mem_board.enable_external_sound;
            if (apu_use_external_sound)
                Tracer.WriteInformation("External sound channels has been enabled on apu.");
        }
        private static void APUSoftReset()
        {
            apu_reg_io_db = 0;
            apu_reg_io_addr = 0;
            apu_reg_access_happened = false;
            apu_reg_access_w = false;
            apu_seq_mode = false;
            apu_odd_cycle = false;
            apu_cycle_f_t = 0;
            apu_cycle_e = 4;
            apu_cycle_f = 4;
            apu_cycle_l = 4;
            apu_odd_l = false;
            apu_check_irq = false;
            apu_do_env = false;
            apu_do_length = false;
            apu_irq_enabled = true;
            apu_irq_flag = false;
            SQ1SoftReset();
            SQ2SoftReset();
            TRLSoftReset();
            NOSSoftReset();
            DMCSoftReset();
        }
        private static void APUIORead(ref ushort addr, out byte value)
        {
            if (addr >= 0x4020)
                mem_board.ReadEX(ref addr, out value);
            else
            {
                apu_reg_io_addr = (byte)(addr & 0x1F);
                apu_reg_access_happened = true;
                apu_reg_access_w = false;
                apu_reg_read_func[apu_reg_io_addr]();
                value = apu_reg_io_db;
            }
        }
        private static void APUIOWrite(ref ushort addr, ref byte value)
        {
            if (addr >= 0x4020)
                mem_board.WriteEX(ref addr, ref value);
            else
            {
                apu_reg_io_addr = (byte)(addr & 0x1F);
                apu_reg_io_db = value;
                apu_reg_access_w = true;
                apu_reg_access_happened = true;
                apu_reg_write_func[apu_reg_io_addr]();
            }
        }

        #region IO Registers
        private static void APUBlankAccess()
        {
        }
        private static void APUWrite4014()
        {
            // OAM DMA
            dma_Oamaddress = (ushort)(apu_reg_io_db << 8);
            AssertOAMDMA();
        }
        private static void APUWrite4015()
        {
            // DMC DMA
            if ((apu_reg_io_db & 0x10) != 0)
            {
                if (dmc_dmaSize == 0)
                {
                    dmc_dmaSize = dmc_size_refresh;
                    dmc_dmaAddr = dmc_addr_refresh;
                }
            }
            else { dmc_dmaSize = 0; }

            if (!dmc_bufferFull && dmc_dmaSize > 0)
            {
                AssertDMCDMA();
            }
        }
        private static void APUOnRegister4015()
        {
            if (apu_reg_access_w)
            {
                // do a normal write
                SQ1On4015();
                SQ2On4015();
                NOSOn4015();
                TRLOn4015();
                DMCOn4015();
            }
            else
            {
                // on reads, do the effects we know
                apu_irq_flag = false;
                IRQFlags &= ~IRQ_APU;
            }
        }
        private static void APUOnRegister4016()
        {
            // Only writes accepted
            if (apu_reg_access_w)
            {
                if (inputStrobe > (apu_reg_io_db & 0x01))
                {
                    if (IsFourPlayers)
                    {
                        PORT0 = joypad3.GetData() << 8 | joypad1.GetData() | 0x01010000;
                        PORT1 = joypad4.GetData() << 8 | joypad2.GetData() | 0x02020000;
                    }
                    else
                    {
                        PORT0 = joypad1.GetData() | 0x01010100;
                        PORT1 = joypad2.GetData() | 0x02020200;
                    }
                }
                inputStrobe = apu_reg_io_db & 0x01;
            }
        }
        private static void APUOnRegister4017()
        {
            if (apu_reg_access_w)
            {
                apu_seq_mode = (apu_reg_io_db & 0x80) != 0;
                apu_irq_enabled = (apu_reg_io_db & 0x40) == 0;

                // Reset counters
                apu_cycle_e = -1;
                apu_cycle_l = -1;
                apu_cycle_f = -1;
                apu_odd_l = false;
                // Clock immediately ?
                apu_do_length = apu_seq_mode;
                apu_do_env = apu_seq_mode;
                // Reset irq
                apu_check_irq = false;

                if (!apu_irq_enabled)
                {
                    apu_irq_flag = false;
                    IRQFlags &= ~IRQ_APU;
                }
            }
        }

        private static void APURead4015()
        {
            apu_reg_io_db = (byte)(apu_reg_io_db & 0x20);
            // Channels enable
            SQ1Read4015();
            SQ2Read4015();
            NOSRead4015();
            TRLRead4015();
            DMCRead4015();
            // IRQ
            if (apu_irq_flag)
                apu_reg_io_db = (byte)((apu_reg_io_db & 0xBF) | 0x40);
            if (apu_irq_delta_occur)
                apu_reg_io_db = (byte)((apu_reg_io_db & 0x7F) | 0x80);

        }
        private static void APURead4016()
        {
            apu_reg_io_db = (byte)(PORT0 & 1);
            PORT0 >>= 1;
        }
        private static void APURead4017()
        {
            apu_reg_io_db = (byte)(PORT1 & 1);
            PORT1 >>= 1;
        }
        #endregion

        private static void APUClock()
        {
            apu_odd_cycle = !apu_odd_cycle;

            if (apu_do_env)
                APUClockEnvelope();

            if (apu_do_length)
                APUClockDuration();

            if (apu_odd_cycle)
            {
                // IRQ
                apu_cycle_f++;
                if (apu_cycle_f >= apu_ferq_f)
                {
                    apu_cycle_f = -1;
                    apu_check_irq = true;
                    apu_cycle_f_t = 3;
                }
                // Envelope
                apu_cycle_e++;
                if (apu_cycle_e >= apu_ferq_e)
                {
                    apu_cycle_e = -1;
                    // Clock envelope and other units except when:
                    // 1 the seq mode is set
                    // 2 it is the time of irq check clock
                    if (apu_check_irq)
                    {
                        if (!apu_seq_mode)
                        {
                            // this is the 3rd step of mode 0, do a reset
                            apu_do_env = true;
                        }
                        else
                        {
                            // the next step will be the 4th step of mode 1
                            // so, shorten the step then do a reset
                            apu_cycle_e = 4;
                        }
                    }
                    else
                        apu_do_env = true;
                }
                // Length
                apu_cycle_l++;
                if (apu_cycle_l >= apu_ferq_l)
                {
                    apu_odd_l = !apu_odd_l;

                    apu_cycle_l = apu_odd_l ? -2 : -1;

                    // Clock duration and sweep except when:
                    // 1 the seq mode is set
                    // 2 it is the time of irq check clock
                    if (apu_check_irq && apu_seq_mode)
                    {
                        apu_cycle_l = 3730;// Next step will be after 7456 - 3730 = 3726 cycles, 2 cycles shorter than e freq
                        apu_odd_l = true;
                    }
                    else
                    {
                        apu_do_length = true;
                    }
                }

                SQ1Clock();
                SQ2Clock();
                NOSClock();

                if (apu_use_external_sound)
                    mem_board.OnAPUClock();

                if (apu_reg_access_happened)
                {
                    apu_reg_access_happened = false;
                    apu_reg_update_func[apu_reg_io_addr]();
                }

            }

            // Clock internal components
            TRLClock();
            DMCClock();

            if (apu_check_irq)
            {
                if (!apu_seq_mode)
                    APUCheckIRQ();
                // This is stupid ... :(
                apu_cycle_f_t--;
                if (apu_cycle_f_t == 0)
                    apu_check_irq = false;
            }
            if (apu_use_external_sound)
                mem_board.OnAPUClockSingle();

            APUUpdatePlayback();
        }
        private static void APUClockDuration()
        {
            SQ1ClockLength();
            SQ2ClockLength();
            NOSClockLength();
            TRLClockLength();
            if (apu_use_external_sound)
                mem_board.OnAPUClockDuration();
            apu_do_length = false;
        }
        private static void APUClockEnvelope()
        {
            SQ1ClockEnvelope();
            SQ2ClockEnvelope();
            NOSClockEnvelope();
            TRLClockEnvelope();
            if (apu_use_external_sound)
                mem_board.OnAPUClockEnvelope();
            apu_do_env = false;
        }
        private static void APUCheckIRQ()
        {
            if (apu_irq_enabled)
                apu_irq_flag = true;
            if (apu_irq_flag)
                IRQFlags |= IRQ_APU;
        }

        private static void CalculateAudioPlaybackValues()
        {
            // Playback buffer
            // Calculate how many samples should be rendered each frame
            apu_target_samples_count_per_frame = (double)MyNesMain.RendererSettings.Audio_Frequency / emu_time_target_fps;
            // Ratio = cpu cycles per frames / target samples each frame
            audio_timer_ratio = cpu_clock_per_frame / apu_target_samples_count_per_frame;

            audio_playback_peek_limit = MyNesMain.RendererSettings.Audio_InternalPeekLimit;
            audio_samples_count = MyNesMain.RendererSettings.Audio_InternalSamplesCount;
            audio_playback_amplitude = MyNesMain.RendererSettings.Audio_PlaybackAmplitude;

            audio_samples = new short[audio_samples_count];
            audio_w_pos = 0;
            audio_samples_added = 0;
            audio_timer = 0;
            audio_x = audio_y = 0;
            Tracer.WriteLine("AUDIO: frequency = " + MyNesMain.RendererSettings.Audio_Frequency);
            Tracer.WriteLine("AUDIO: timer ratio = " + audio_timer_ratio);
            Tracer.WriteLine("AUDIO: internal samples count = " + audio_samples_count);
            Tracer.WriteLine("AUDIO: amplitude = " + audio_playback_amplitude);
            double dt = 0;

            // See https://en.wikipedia.org/wiki/Low-pass_filter,
            // we are calculating the alpha constant using the accurate information.

            dt = (double)MyNesMain.RendererSettings.Audio_Frequency / 14000;
            audio_low_pass_filter_14K = new SoundLowPassFilter(SoundLowPassFilter.GetK(dt, 14000));

            dt = (double)MyNesMain.RendererSettings.Audio_Frequency / 90;
            audio_high_pass_filter_90 = new SoundHighPassFilter(SoundHighPassFilter.GetK(dt, 90));

            dt = (double)MyNesMain.RendererSettings.Audio_Frequency / 440;
            audio_high_pass_filter_440 = new SoundHighPassFilter(SoundHighPassFilter.GetK(dt, 440));

            InitializeDACTables(false);
        }
        public static void InitializeDACTables(bool force_intitialize)
        {
            if (audio_playback_dac_initialized && !force_intitialize)
                return;

            int[] channelsOut = new int[5];

            mix_table = new int[16][][][][];

            for (int sq1 = 0; sq1 < 16; sq1++)
            {
                mix_table[sq1] = new int[16][][][];

                for (int sq2 = 0; sq2 < 16; sq2++)
                {
                    mix_table[sq1][sq2] = new int[16][][];

                    for (int tri = 0; tri < 16; tri++)
                    {
                        mix_table[sq1][sq2][tri] = new int[16][];

                        for (int noi = 0; noi < 16; noi++)
                        {
                            mix_table[sq1][sq2][tri][noi] = new int[128];

                            for (int dmc = 0; dmc < 128; dmc++)
                            {
                                if (MyNesMain.RendererSettings.Audio_UseDefaultMixer)
                                {
                                    var sqr = (95.88 / (8128.0 / (sq1 + sq2) + 100));
                                    var tnd = (159.79 / (1.0 / (tri / 8227.0 + noi / 12241.0 + dmc / 22638.0) + 100));

                                    mix_table[sq1][sq2][tri][noi][dmc] = (int)Math.Ceiling((sqr + tnd) * audio_playback_amplitude);
                                }
                                else
                                {
                                    // 1 convert the output of each channel into the output domain
                                    GetPrec(sq1, 255, 2 * 1024, out channelsOut[0]);
                                    GetPrec(sq2, 255, 2 * 1024, out channelsOut[1]);
                                    GetPrec(noi, 255, 2 * 1024, out channelsOut[2]);
                                    GetPrec(tri, 255, 2 * 1024, out channelsOut[3]);
                                    GetPrec(dmc, 255, 2 * 1024, out channelsOut[4]);

                                    channelsOut[4] /= 2;// Reduce dmc level to half

                                    // 2 average them
                                    int avv = channelsOut[0] + channelsOut[1] + channelsOut[2] + channelsOut[3] + channelsOut[4];
                                    avv /= 5;

                                    mix_table[sq1][sq2][tri][noi][dmc] = avv;
                                }
                            }
                        }
                    }
                }
            }
            audio_playback_dac_initialized = true;
        }
        private static void APUUpdatePlayback()
        {
            if (!SoundEnabled)
                return;



            // TODO: audio downsampling
            audio_timer++;

            if (audio_timer >= audio_timer_ratio)
            {
                // Clock by output sample rate. This is the point where the sample should be put in the rendering buffer of playback
                audio_timer -= audio_timer_ratio;

                audio_x = mix_table[sq1_output][sq2_output][trl_output][nos_output][dmc_output];

                if (apu_use_external_sound)
                {
                    audio_x = (audio_x + (mem_board.APUGetSample() * audio_playback_amplitude)) / 2;
                }
                // Apply filters as described here: http://wiki.nesdev.com/w/index.php/APU_Mixer
                // Note that these works exactly as real nes and they are accurately calculated
                audio_high_pass_filter_90.DoFiltering(audio_x, out audio_y);// 90 Hz
                audio_high_pass_filter_440.DoFiltering(audio_y, out audio_y);// 440 Hz
                audio_low_pass_filter_14K.DoFiltering(audio_y, out audio_y);// 14 KHz

                // DC blocker ? is needed to make the sound better ?
                // audio_dc_blocker_filter.DoFiltering(audio_y, out audio_y);

                // Write into buffer
                if (audio_w_pos < audio_samples_count)
                {
                    if (audio_y > audio_playback_peek_limit)
                        audio_y = audio_playback_peek_limit;
                    if (audio_y < -audio_playback_peek_limit)
                        audio_y = -audio_playback_peek_limit;
                    audio_samples[audio_w_pos] = (short)audio_y;

                    if (MyNesMain.WaveRecorder.IsRecording)
                        MyNesMain.WaveRecorder.AddSample((short)audio_y);

                    audio_w_pos++;
                    audio_samples_added++;
                }

                audio_y = 0;
            }
        }

        private static void GetPrec(int inVal, int inMax, int outMax, out int val)
        {
            val = (outMax * inVal) / inMax;
        }
        private static void APUWriteState(ref System.IO.BinaryWriter bin)
        {
            bin.Write(apu_reg_io_db);
            bin.Write(apu_reg_io_addr);
            bin.Write(apu_reg_access_happened);
            bin.Write(apu_reg_access_w);
            bin.Write(apu_odd_cycle);
            bin.Write(apu_irq_enabled);
            bin.Write(apu_irq_flag);
            bin.Write(apu_irq_delta_occur);

            bin.Write(apu_seq_mode);
            bin.Write(apu_ferq_f);
            bin.Write(apu_ferq_l);
            bin.Write(apu_ferq_e);
            bin.Write(apu_cycle_f);
            bin.Write(apu_cycle_e);
            bin.Write(apu_cycle_l);
            bin.Write(apu_odd_l);
            bin.Write(apu_cycle_f_t);

            bin.Write(apu_check_irq);
            bin.Write(apu_do_env);
            bin.Write(apu_do_length);

            SQ1WriteState(ref bin);
            SQ2WriteState(ref bin);
            NOSWriteState(ref bin);
            TRLWriteState(ref bin);
            DMCWriteState(ref bin);
        }
        private static void APUReadState(ref System.IO.BinaryReader bin)
        {
            apu_reg_io_db = bin.ReadByte();
            apu_reg_io_addr = bin.ReadByte();
            apu_reg_access_happened = bin.ReadBoolean();
            apu_reg_access_w = bin.ReadBoolean();
            apu_odd_cycle = bin.ReadBoolean();
            apu_irq_enabled = bin.ReadBoolean();
            apu_irq_flag = bin.ReadBoolean();
            apu_irq_delta_occur = bin.ReadBoolean();

            apu_seq_mode = bin.ReadBoolean();
            apu_ferq_f = bin.ReadInt32();
            apu_ferq_l = bin.ReadInt32();
            apu_ferq_e = bin.ReadInt32();
            apu_cycle_f = bin.ReadInt32();
            apu_cycle_e = bin.ReadInt32();
            apu_cycle_l = bin.ReadInt32();
            apu_odd_l = bin.ReadBoolean();
            apu_cycle_f_t = bin.ReadInt32();

            apu_check_irq = bin.ReadBoolean();
            apu_do_env = bin.ReadBoolean();
            apu_do_length = bin.ReadBoolean();

            SQ1ReadState(ref bin);
            SQ2ReadState(ref bin);
            NOSReadState(ref bin);
            TRLReadState(ref bin);
            DMCReadState(ref bin);
        }
    }
}
