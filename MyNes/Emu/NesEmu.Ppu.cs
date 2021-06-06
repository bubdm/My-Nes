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
    /*PPU Section*/
    public partial class NesEmu
    {
        private static byte[] reverseLookup =
        {
                0x00, 0x80, 0x40, 0xC0, 0x20, 0xA0, 0x60, 0xE0, 0x10, 0x90, 0x50, 0xD0, 0x30, 0xB0, 0x70, 0xF0,
                0x08, 0x88, 0x48, 0xC8, 0x28, 0xA8, 0x68, 0xE8, 0x18, 0x98, 0x58, 0xD8, 0x38, 0xB8, 0x78, 0xF8,
                0x04, 0x84, 0x44, 0xC4, 0x24, 0xA4, 0x64, 0xE4, 0x14, 0x94, 0x54, 0xD4, 0x34, 0xB4, 0x74, 0xF4,
                0x0C, 0x8C, 0x4C, 0xCC, 0x2C, 0xAC, 0x6C, 0xEC, 0x1C, 0x9C, 0x5C, 0xDC, 0x3C, 0xBC, 0x7C, 0xFC,
                0x02, 0x82, 0x42, 0xC2, 0x22, 0xA2, 0x62, 0xE2, 0x12, 0x92, 0x52, 0xD2, 0x32, 0xB2, 0x72, 0xF2,
                0x0A, 0x8A, 0x4A, 0xCA, 0x2A, 0xAA, 0x6A, 0xEA, 0x1A, 0x9A, 0x5A, 0xDA, 0x3A, 0xBA, 0x7A, 0xFA,
                0x06, 0x86, 0x46, 0xC6, 0x26, 0xA6, 0x66, 0xE6, 0x16, 0x96, 0x56, 0xD6, 0x36, 0xB6, 0x76, 0xF6,
                0x0E, 0x8E, 0x4E, 0xCE, 0x2E, 0xAE, 0x6E, 0xEE, 0x1E, 0x9E, 0x5E, 0xDE, 0x3E, 0xBE, 0x7E, 0xFE,
                0x01, 0x81, 0x41, 0xC1, 0x21, 0xA1, 0x61, 0xE1, 0x11, 0x91, 0x51, 0xD1, 0x31, 0xB1, 0x71, 0xF1,
                0x09, 0x89, 0x49, 0xC9, 0x29, 0xA9, 0x69, 0xE9, 0x19, 0x99, 0x59, 0xD9, 0x39, 0xB9, 0x79, 0xF9,
                0x05, 0x85, 0x45, 0xC5, 0x25, 0xA5, 0x65, 0xE5, 0x15, 0x95, 0x55, 0xD5, 0x35, 0xB5, 0x75, 0xF5,
                0x0D, 0x8D, 0x4D, 0xCD, 0x2D, 0xAD, 0x6D, 0xED, 0x1D, 0x9D, 0x5D, 0xDD, 0x3D, 0xBD, 0x7D, 0xFD,
                0x03, 0x83, 0x43, 0xC3, 0x23, 0xA3, 0x63, 0xE3, 0x13, 0x93, 0x53, 0xD3, 0x33, 0xB3, 0x73, 0xF3,
                0x0B, 0x8B, 0x4B, 0xCB, 0x2B, 0xAB, 0x6B, 0xEB, 0x1B, 0x9B, 0x5B, 0xDB, 0x3B, 0xBB, 0x7B, 0xFB,
                0x07, 0x87, 0x47, 0xC7, 0x27, 0xA7, 0x67, 0xE7, 0x17, 0x97, 0x57, 0xD7, 0x37, 0xB7, 0x77, 0xF7,
                0x0F, 0x8F, 0x4F, 0xCF, 0x2F, 0xAF, 0x6F, 0xEF, 0x1F, 0x9F, 0x5F, 0xDF, 0x3F, 0xBF, 0x7F, 0xFF,
        };
        // Buffers and function accesses
        private static Action[] ppu_v_clocks;
        private static Action[] ppu_h_clocks;
        private static Action[] ppu_bkg_fetches;
        private static Action[] ppu_spr_fetches;
        private static Action[] ppu_oam_phases;
        private static int[] ppu_bkg_pixels;
        private static int[] ppu_spr_pixels;
        private static int[] ppu_screen_pixels;
        private static int[] ppu_colors;
        // Clocks
        private static int ppu_clock_h;
        internal static ushort ppu_clock_v;
        private static ushort ppu_clock_vblank_start;
        private static ushort ppu_clock_vblank_end;
        private static bool ppu_use_odd_cycle;
        private static bool ppu_use_odd_swap;
        private static bool ppu_odd_swap_done;
        private static bool ppu_is_nmi_time;
        private static bool ppu_frame_finished;
        // Memory 
        private static byte[] ppu_oam_bank;
        private static byte[] ppu_oam_bank_secondary;
        private static byte[] ppu_palette_bank;
        // DATA REG
        private static byte ppu_reg_io_db;// The data bus
        private static byte ppu_reg_io_addr;// The address bus (only first 3 bits are used, will be ranged 0-7)
        private static bool ppu_reg_access_happened;// Triggers when cpu accesses ppu bus.
        private static bool ppu_reg_access_w;// True= write access, False= Read access.
        private static Action[] ppu_reg_update_func;
        private static Action[] ppu_reg_read_func;
        // 0x2000 register values
        private static byte ppu_reg_2000_vram_address_increament;
        private static ushort ppu_reg_2000_sprite_pattern_table_address_for_8x8_sprites;
        private static ushort ppu_reg_2000_background_pattern_table_address;
        internal static byte ppu_reg_2000_Sprite_size;
        private static bool ppu_reg_2000_VBI;
        // 0x2001 register values
        private static bool ppu_reg_2001_show_background_in_leftmost_8_pixels_of_screen;
        private static bool ppu_reg_2001_show_sprites_in_leftmost_8_pixels_of_screen;
        private static bool ppu_reg_2001_show_background;
        private static bool ppu_reg_2001_show_sprites;
        private static int ppu_reg_2001_grayscale;
        private static int ppu_reg_2001_emphasis;
        // 0x2002 register values.
        private static bool ppu_reg_2002_SpriteOverflow;
        private static bool ppu_reg_2002_Sprite0Hit;
        private static bool ppu_reg_2002_VblankStartedFlag;
        // 0x2003 register values.
        private static byte ppu_reg_2003_oam_addr;
        // VRAM
        private static ushort ppu_vram_addr;
        private static byte ppu_vram_data;
        private static ushort ppu_vram_addr_temp;
        private static ushort ppu_vram_addr_access_temp;
        private static bool ppu_vram_flip_flop;
        private static byte ppu_vram_finex;
        // Fetches
        private static ushort ppu_bkgfetch_nt_addr;
        private static byte ppu_bkgfetch_nt_data;
        private static ushort ppu_bkgfetch_at_addr;
        private static byte ppu_bkgfetch_at_data;
        private static ushort ppu_bkgfetch_lb_addr;
        private static byte ppu_bkgfetch_lb_data;
        private static ushort ppu_bkgfetch_hb_addr;
        private static byte ppu_bkgfetch_hb_data;

        private static int ppu_sprfetch_slot;
        private static byte ppu_sprfetch_y_data;
        private static byte ppu_sprfetch_t_data;
        private static byte ppu_sprfetch_at_data;
        private static byte ppu_sprfetch_x_data;
        private static ushort ppu_sprfetch_lb_addr;
        private static byte ppu_sprfetch_lb_data;
        private static ushort ppu_sprfetch_hb_addr;
        private static byte ppu_sprfetch_hb_data;

        internal static bool ppu_is_sprfetch;

        // Helper temps
        private static int ppu_bkg_render_i;
        private static int ppu_bkg_render_pos;
        private static int ppu_bkg_render_tmp_val;
        private static int ppu_bkg_current_pixel;
        private static int ppu_spr_current_pixel;
        private static int ppu_current_pixel;
        // Renderer helper
        private static int ppu_render_x;
        private static int ppu_render_y;
        // OAM
        private static byte ppu_oamev_n;
        private static byte ppu_oamev_m;
        private static bool ppu_oamev_compare;
        private static byte ppu_oamev_slot;
        private static byte ppu_fetch_data;
        private static byte ppu_phase_index;
        private static bool ppu_sprite0_should_hit;

        private static int ppu_temp_comparator;

        private static int ppu_color_temp;
        private static ushort ppu_color_address_temp;
        private static byte ppu_color_data_temp;

        private static void PPUInitialize()
        {
            ppu_reg_update_func = new Action[]
            {
                PPUOnRegister2000, PPUOnRegister2001, PPUOnRegister2002, PPUOnRegister2003, PPUOnRegister2004, PPUOnRegister2005, PPUOnRegister2006, PPUOnRegister2007,
            };
            ppu_reg_read_func = new Action[]
            {
                PPURead2000, PPURead2001, PPURead2002, PPURead2003, PPURead2004, PPURead2005, PPURead2006, PPURead2007,
            };
            ppu_bkg_fetches = new Action[]
            {
                PPUBKFetch0, PPUBKFetch1, PPUBKFetch2, PPUBKFetch3, PPUBKFetch4, PPUBKFetch5, PPUBKFetch6, PPUBKFetch7,
            };
            ppu_spr_fetches = new Action[]
            {
                PPUBKFetch0, PPUBKFetch1, PPUBKFetch2, PPUBKFetch3, PPUSPRFetch0, PPUSPRFetch1, PPUSPRFetch2, PPUSPRFetch3,
            };
            ppu_oam_phases = new Action[]
            {
                PPUOamPhase0, PPUOamPhase1, PPUOamPhase2, PPUOamPhase3, PPUOamPhase4, PPUOamPhase5, PPUOamPhase6, PPUOamPhase7, PPUOamPhase8,
            };
            // Map clocks
            ppu_h_clocks = new Action[341];
            ppu_h_clocks[0] = PPUHClock_000_Idle;

            for (int i = 1; i < 257; i++)
            {
                ppu_h_clocks[i] = PPUHClock_1_256_BKGClocks;
            }
            for (int i = 257; i < 321; i++)
            {
                ppu_h_clocks[i] = PPUHClock_257_320_SPRClocks;
            }
            for (int i = 321; i < 337; i++)
            {
                ppu_h_clocks[i] = PPUHClock_321_336_DUMClocks;
            }
            for (int i = 337; i < 341; i++)
            {
                ppu_h_clocks[i] = PPUHClock_337_340_DUMClocks;
            }
            ppu_v_clocks = new Action[320];
            for (int i = 0; i < 240; i++)
            {
                ppu_v_clocks[i] = PPUScanlineRender;
            }
            ppu_v_clocks[240] = PPUScanlineVBLANK;
            // OAM
            ppu_oam_bank = new byte[256];
            ppu_oam_bank_secondary = new byte[32];
            // Palettes
            ppu_palette_bank = new byte[32];
            // Pixels
            ppu_bkg_pixels = new int[512];
            ppu_spr_pixels = new int[512];
            ppu_screen_pixels = new int[256 * 240];
            ppu_colors = new int[1024];
        }
        private static void PPUHardReset()
        {
            switch (Region)
            {
                case EmuRegion.NTSC:
                    {
                        ppu_clock_vblank_start = 241;
                        ppu_clock_vblank_end = 261;
                        ppu_use_odd_cycle = true;
                        break;
                    }
                case EmuRegion.PALB:
                    {
                        ppu_clock_vblank_start = 241;
                        ppu_clock_vblank_end = 311;
                        ppu_use_odd_cycle = false;
                        break;
                    }
                case EmuRegion.DENDY:
                    {
                        ppu_clock_vblank_start = 291;
                        ppu_clock_vblank_end = 311;
                        // We need to do mapping from 241 to 290
                        for (int i = 241; i <= 290; i++)
                        {
                            ppu_v_clocks[i] = PPUScanlineVBLANK;
                        }
                        ppu_use_odd_cycle = false;
                        break;
                    }
            }

            // Map clocks
            ppu_v_clocks[ppu_clock_vblank_start] = PPUScanlineVBLANKStart;
            for (int i = ppu_clock_vblank_start + 1; i <= ppu_clock_vblank_end - 1; i++)
            {
                ppu_v_clocks[i] = PPUScanlineVBLANK;
            }
            ppu_v_clocks[ppu_clock_vblank_end] = PPUScanlineVBLANKEnd;

            // OAM
            ppu_oam_bank = new byte[256];
            ppu_oam_bank_secondary = new byte[32];
            PPUOamReset();
            // Palettes
            ppu_palette_bank = new byte[] // Miscellaneous, real NES loads values similar to these during power up
            {
                0x09, 0x01, 0x00, 0x01, 0x00, 0x02, 0x02, 0x0D, 0x08, 0x10, 0x08, 0x24, 0x00, 0x00, 0x04, 0x2C, // Bkg palette
                0x09, 0x01, 0x34, 0x03, 0x00, 0x04, 0x00, 0x14, 0x08, 0x3A, 0x00, 0x02, 0x00, 0x20, 0x2C, 0x08  // Spr palette
            };

            Color_Light_Add = MyNesMain.RendererSettings.Vid_SaturAdd;
            Color_Saturation_Add = MyNesMain.RendererSettings.Vid_SaturAdd;

            GenerrateColors(ref ppu_colors, (int)Region);

            ppu_reg_io_db = 0;
            ppu_reg_io_addr = 0;
            ppu_reg_access_happened = false;
            ppu_reg_access_w = false;

            ppu_reg_2000_vram_address_increament = 1;
            ppu_reg_2000_sprite_pattern_table_address_for_8x8_sprites = 0;
            ppu_reg_2000_background_pattern_table_address = 0;
            ppu_reg_2000_Sprite_size = 0;
            ppu_reg_2000_VBI = false;

            ppu_reg_2001_show_background_in_leftmost_8_pixels_of_screen = false;
            ppu_reg_2001_show_sprites_in_leftmost_8_pixels_of_screen = false;
            ppu_reg_2001_show_background = false;
            ppu_reg_2001_show_sprites = false;
            ppu_reg_2001_emphasis = 0;
            ppu_reg_2002_SpriteOverflow = false;
            ppu_reg_2002_Sprite0Hit = false;
            ppu_reg_2002_VblankStartedFlag = false;

            ppu_reg_2003_oam_addr = 0;

            ppu_is_sprfetch = false;
            ppu_use_odd_swap = false;

            ppu_clock_h = 0;
            ppu_clock_v = 0;
        }
        private static void PPUClock()
        {
            mem_board.OnPPUClock();
            // Clock a scanline
            ppu_v_clocks[ppu_clock_v]();

            ppu_clock_h++;

            // Advance
            if (ppu_clock_h >= 341)
            {
                mem_board.OnPPUScanlineTick();
                // Advance scanline ...
                if (ppu_clock_v == ppu_clock_vblank_end)
                {
                    ppu_clock_v = 0;

                    // ppu_frame_finished = true;
                }
                else
                    ppu_clock_v++;
                ppu_clock_h = 0;
            }

            /* THEORY:
             * After read/write (io access at 0x200x), the registers take effect at the end of the ppu cycle.
             * That's why we have the vbl and nmi effects on writing at 0x2000 and reading from 0x2002.
             * Same for other registers. First, when cpu access the IO at 0x200x, a data register sets from cpu written value.
             * At the end of the next ppu clock, ppu check out what happened in IO, then updates flags and other stuff.
             * For writes, first cpu sets the data bus, then AT THE END OF PPU CYCLE, update flags and data from the data bus to use at the next cycle.
             * For reads, first update the data bus with flags and data, then return the value to cpu. 
             * After that, AT THE END OF PPU CYCLE, ppu acknowledge the read: flags get reset, data updated ...etc
             */
            if (ppu_reg_access_happened)
            {
                ppu_reg_access_happened = false;
                ppu_reg_update_func[ppu_reg_io_addr]();
            }
        }
        public static int GetPixel(int x, int y)
        {
            return ppu_screen_pixels[(y * 256) + x];
        }
        #region Scanlines
        private static void PPUScanlineRender()
        {
            ppu_h_clocks[ppu_clock_h]();
        }
        private static void PPUScanlineVBLANKStart()
        {
            // This is scanline 241
            ppu_is_nmi_time = ppu_clock_h >= 1 & ppu_clock_h <= 3;
            if (ppu_is_nmi_time)// 0 - 3
            {
                if (ppu_clock_h == 1)
                {
                    ppu_reg_2002_VblankStartedFlag = true;
                    ppu_frame_finished = true;
                }

                PPU_NMI_Current = (ppu_reg_2002_VblankStartedFlag & ppu_reg_2000_VBI);
            }
        }
        private static void PPUScanlineVBLANKEnd()
        {
            // This is scanline 261, also called pre-render line
            ppu_is_nmi_time = ppu_clock_h >= 1 & ppu_clock_h <= 3;


            if (ppu_clock_h == 1)
            {
                // Clear vbl flag
                ppu_reg_2002_Sprite0Hit = false;
                ppu_reg_2002_VblankStartedFlag = false;
                ppu_reg_2002_SpriteOverflow = false;
            }

            // Pre-render line is here !
            // Do a pre-render 
            PPUScanlineRender();

            if (ppu_use_odd_cycle)
            {
                // http://wiki.nesdev.com/w/index.php/File:Ntsc_timing.png:
                // "The skipped tick is implemented by jumping directly from (339,261) to (0,0)"
                if (ppu_clock_h == 339)
                {
                    // ODD Cycle
                    ppu_use_odd_swap = !ppu_use_odd_swap;
                    if (!ppu_use_odd_swap & (ppu_reg_2001_show_background || ppu_reg_2001_show_sprites))
                    {
                        ppu_odd_swap_done = true;
                        ppu_clock_h++;
                    }
                }
            }
        }
        private static void PPUScanlineVBLANK()
        {
        }
        #endregion
        #region A Scanline Clocks
        private static void PPUHClock_000_Idle()
        {
            if (ppu_odd_swap_done)
            {
                ppu_bkg_fetches[1]();
                ppu_odd_swap_done = false;
            }
        }
        private static void PPUHClock_1_256_BKGClocks()
        {
            if (ppu_reg_2001_show_background || ppu_reg_2001_show_sprites)
            {
                // H clocks 1 - 256
                // OAM evaluation doesn't occur on pre-render scanline.
                if (ppu_clock_v != ppu_clock_vblank_end)
                {
                    // Sprite evaluation
                    if (ppu_clock_h > 0 && ppu_clock_h < 65)
                    {
                        ppu_oam_bank_secondary[(ppu_clock_h - 1) & 0x1F] = 0xFF;
                    }
                    else
                    {
                        if (ppu_clock_h == 65)
                            PPUOamReset();

                        if (((ppu_clock_h - 1) & 1) == 0)
                            PPUOamEvFetch();
                        else
                            ppu_oam_phases[ppu_phase_index]();

                        if (ppu_clock_h == 256)
                            PPUOamClear();
                    }
                }

                // BKG fetches
                ppu_bkg_fetches[(ppu_clock_h - 1) & 7]();
                if (ppu_clock_v < 240)
                    RenderPixel();
            }
            else if (ppu_clock_v < 240)
            {
                // Rendering is off, draw color at vram address
                //ppu_screen_pixels[ppu_clock_h - 1 + (ppu_clock_v * 256)] = MakeRGBColor(ppu_vram_addr);
                if (ppu_vram_addr < 0x3F00)
                {
                    ppu_color_temp = 0xF;
                }
                else
                {
                    // 1 read data
                    if ((ppu_vram_addr & 0x03) == 0)
                        ppu_color_data_temp = ppu_palette_bank[ppu_vram_addr & 0x0C];
                    else
                        ppu_color_data_temp = ppu_palette_bank[ppu_vram_addr & 0x1F];
                    // 2 construct color
                    ppu_color_temp = (ppu_color_data_temp & 0x3F) | ppu_reg_2001_grayscale | ppu_reg_2001_emphasis;
                }
                ppu_screen_pixels[ppu_clock_h - 1 + (ppu_clock_v * 256)] = ppu_colors[ppu_color_temp];
            }
        }
        private static void PPUHClock_257_320_SPRClocks()
        {
            if (ppu_reg_2001_show_background || ppu_reg_2001_show_sprites)
            {
                // H clocks 256 - 320
                // BKG garbage fetches and sprite fetches
                ppu_spr_fetches[(ppu_clock_h - 1) & 7]();

                if (ppu_clock_h == 257)
                    ppu_vram_addr = (ushort)((ppu_vram_addr & 0x7BE0) | (ppu_vram_addr_temp & 0x041F));

                if (ppu_clock_v == ppu_clock_vblank_end && ppu_clock_h >= 280 && ppu_clock_h <= 304)
                    ppu_vram_addr = (ushort)((ppu_vram_addr & 0x041F) | (ppu_vram_addr_temp & 0x7BE0));
            }
        }
        private static void PPUHClock_321_336_DUMClocks()
        {
            if (ppu_reg_2001_show_background || ppu_reg_2001_show_sprites)
            {
                // 321 - 336
                // BKG dummy fetch
                ppu_bkg_fetches[(ppu_clock_h - 1) & 7]();
            }
        }
        private static void PPUHClock_337_340_DUMClocks()
        {
            if (ppu_reg_2001_show_background || ppu_reg_2001_show_sprites)
            {
                // 337 - 340
                // BKG dummy fetch
                ppu_bkg_fetches[(ppu_clock_h - 1) & 1]();
            }
        }
        #endregion
        #region BKG Fetches
        private static void PPUBKFetch0()
        {
            // Calculate NT address
            ppu_bkgfetch_nt_addr = (ushort)(0x2000 | (ppu_vram_addr & 0x0FFF));
            mem_board.OnPPUAddressUpdate(ref ppu_bkgfetch_nt_addr);
        }
        private static void PPUBKFetch1()
        {
            // Fetch NT data
            mem_board.ReadNMT(ref ppu_bkgfetch_nt_addr, out ppu_bkgfetch_nt_data);
        }
        private static void PPUBKFetch2()
        {
            // Calculate AT address
            ppu_bkgfetch_at_addr = (ushort)(0x23C0 | (ppu_vram_addr & 0xC00) | ((ppu_vram_addr >> 4) & 0x38) | ((ppu_vram_addr >> 2) & 0x7));
            mem_board.OnPPUAddressUpdate(ref ppu_bkgfetch_at_addr);
        }
        private static void PPUBKFetch3()
        {
            // Fetch AT data
            mem_board.ReadNMT(ref ppu_bkgfetch_at_addr, out ppu_bkgfetch_at_data);
            ppu_bkgfetch_at_data = (byte)(ppu_bkgfetch_at_data >> ((ppu_vram_addr >> 4 & 0x04) | (ppu_vram_addr & 0x02)));
        }
        private static void PPUBKFetch4()
        {
            // Calculate tile low-bit address
            ppu_bkgfetch_lb_addr = (ushort)(ppu_reg_2000_background_pattern_table_address | (ppu_bkgfetch_nt_data << 4) | (ppu_vram_addr >> 12 & 7));
            mem_board.OnPPUAddressUpdate(ref ppu_bkgfetch_lb_addr);
        }
        private static void PPUBKFetch5()
        {
            // Fetch tile low-bit data
            mem_board.ReadCHR(ref ppu_bkgfetch_lb_addr, out ppu_bkgfetch_lb_data);
        }
        private static void PPUBKFetch6()
        {
            // Calculate tile high-bit address
            ppu_bkgfetch_hb_addr = (ushort)(ppu_reg_2000_background_pattern_table_address | (ppu_bkgfetch_nt_data << 4) | 8 | (ppu_vram_addr >> 12 & 7));
            mem_board.OnPPUAddressUpdate(ref ppu_bkgfetch_hb_addr);
        }
        private static void PPUBKFetch7()
        {
            // Fetch tile high-bit data
            mem_board.ReadCHR(ref ppu_bkgfetch_hb_addr, out ppu_bkgfetch_hb_data);

            /* if (ppu_clock_h > 320)
                 ppu_bkg_render_pos = ppu_clock_h - 328;
             else
                 ppu_bkg_render_pos = ppu_clock_h + 8;*/

            ppu_bkg_render_pos = ppu_clock_h + 8;

            ppu_bkg_render_pos = ppu_bkg_render_pos % 336;

            // Increments
            if (ppu_clock_h == 256)
            {
                // Increment Y
                if ((ppu_vram_addr & 0x7000) != 0x7000)
                {
                    ppu_vram_addr += 0x1000;
                }
                else
                {
                    ppu_vram_addr ^= 0x7000;

                    switch (ppu_vram_addr & 0x3E0)
                    {
                        case 0x3A0: ppu_vram_addr ^= 0xBA0; break;
                        case 0x3E0: ppu_vram_addr ^= 0x3E0; break;
                        default: ppu_vram_addr += 0x20; break;
                    }
                }
            }
            else
            {
                // Increment X
                if ((ppu_vram_addr & 0x001F) == 0x001F)
                    ppu_vram_addr ^= 0x041F;
                else
                    ppu_vram_addr++;
            }

            // Rendering background pixel
            for (ppu_bkg_render_i = 0; ppu_bkg_render_i < 8; ppu_bkg_render_i++)
            {
                ppu_bkg_render_tmp_val = ((ppu_bkgfetch_at_data << 2) & 0xC) | (ppu_bkgfetch_lb_data >> 7 & 1) | (ppu_bkgfetch_hb_data >> 6 & 2);
                ppu_bkg_pixels[ppu_bkg_render_i + ppu_bkg_render_pos] = ppu_bkg_render_tmp_val;
                ppu_bkgfetch_lb_data <<= 1;
                ppu_bkgfetch_hb_data <<= 1;
            }

        }
        #endregion
        #region SPR Fetches
        private static void PPUSPRFetch0()
        {
            ppu_sprfetch_slot = (((ppu_clock_h - 1) >> 3) & 7);
            ppu_sprfetch_slot = 7 - ppu_sprfetch_slot;
            ppu_sprfetch_y_data = ppu_oam_bank_secondary[(ppu_sprfetch_slot * 4)];
            ppu_sprfetch_t_data = ppu_oam_bank_secondary[(ppu_sprfetch_slot * 4) + 1];
            ppu_sprfetch_at_data = ppu_oam_bank_secondary[(ppu_sprfetch_slot * 4) + 2];
            ppu_sprfetch_x_data = ppu_oam_bank_secondary[(ppu_sprfetch_slot * 4) + 3];
            ppu_temp_comparator = (ppu_clock_v - ppu_sprfetch_y_data) ^ ((ppu_sprfetch_at_data & 0x80) != 0 ? 0x0F : 0x00);
            if (ppu_reg_2000_Sprite_size == 0x10)
            {
                ppu_sprfetch_lb_addr = (ushort)((ppu_sprfetch_t_data << 0x0C & 0x1000) | (ppu_sprfetch_t_data << 0x04 & 0x0FE0) |
                             (ppu_temp_comparator << 0x01 & 0x0010) | (ppu_temp_comparator & 0x0007));
            }
            else
            {
                ppu_sprfetch_lb_addr = (ushort)(ppu_reg_2000_sprite_pattern_table_address_for_8x8_sprites | (ppu_sprfetch_t_data << 0x04) | (ppu_temp_comparator & 0x0007));
            }
            mem_board.OnPPUAddressUpdate(ref ppu_sprfetch_lb_addr);
        }
        private static void PPUSPRFetch1()
        {
            // Fetch tile low-bit data
            ppu_is_sprfetch = true;
            mem_board.ReadCHR(ref ppu_sprfetch_lb_addr, out ppu_sprfetch_lb_data);
            ppu_is_sprfetch = false;

            if ((ppu_sprfetch_at_data & 0x40) != 0)
                ppu_sprfetch_lb_data = reverseLookup[ppu_sprfetch_lb_data];
        }
        private static void PPUSPRFetch2()
        {
            ppu_sprfetch_hb_addr = (ushort)(ppu_sprfetch_lb_addr | 0x08);
            mem_board.OnPPUAddressUpdate(ref ppu_sprfetch_hb_addr);
        }
        private static void PPUSPRFetch3()
        {
            // Same as fetch 3 but with render
            // Fetch tile high-bit data
            ppu_is_sprfetch = true;
            mem_board.ReadCHR(ref ppu_sprfetch_hb_addr, out ppu_sprfetch_hb_data);
            ppu_is_sprfetch = false;

            if ((ppu_sprfetch_at_data & 0x40) != 0)
                ppu_sprfetch_hb_data = reverseLookup[ppu_sprfetch_hb_data];

            // Render the sprite
            if (ppu_sprfetch_x_data == 255)
                return;
            // Rendering sprite pixel
            for (ppu_bkg_render_i = 0; ppu_bkg_render_i < 8; ppu_bkg_render_i++)
            {
                if (ppu_sprfetch_x_data < 255)
                {
                    ppu_bkg_render_tmp_val = ((ppu_sprfetch_at_data << 2) & 0xC) | (ppu_sprfetch_lb_data >> 7 & 1) | (ppu_sprfetch_hb_data >> 6 & 2);
                    //if ((ppu_bkg_render_tmp_val & 3) != 0 && (ppu_spr_pixels[ppu_bkg_render_pos] & 3) == 0)
                    if ((ppu_bkg_render_tmp_val & 3) != 0)
                    {
                        ppu_spr_pixels[ppu_sprfetch_x_data] = ppu_bkg_render_tmp_val;

                        if (ppu_sprfetch_slot == 0 && ppu_sprite0_should_hit)
                        {
                            // ppu_sprite0_should_hit = false;
                            ppu_spr_pixels[ppu_sprfetch_x_data] |= 0x4000;// Sprite 0
                        }

                        if ((ppu_sprfetch_at_data & 0x20) == 0)
                            ppu_spr_pixels[ppu_sprfetch_x_data] |= 0x8000;
                    }

                    ppu_sprfetch_lb_data <<= 1;
                    ppu_sprfetch_hb_data <<= 1;

                    ppu_sprfetch_x_data++;
                }
            }

        }
        #endregion
        #region OAM Evaluation
        private static void PPUOamReset()
        {
            ppu_oamev_n = 0;
            ppu_oamev_m = 0;
            ppu_oamev_slot = 0;
            ppu_phase_index = 0;
            ppu_sprite0_should_hit = false;
        }
        private static void PPUOamClear()
        {
            for (int i = 0; i < ppu_spr_pixels.Length; i++)
                ppu_spr_pixels[i] = 0;
        }
        private static void PPUOamEvFetch()
        {
            ppu_fetch_data = ppu_oam_bank[(ppu_oamev_n * 4) + ppu_oamev_m];
        }
        private static void PPUOamPhase0()
        {
            ppu_oamev_compare = ppu_clock_v >= ppu_fetch_data && ppu_clock_v < ppu_fetch_data + ppu_reg_2000_Sprite_size;

            // Check if read data in range
            if (ppu_oamev_compare)
            {
                // Sprite in range, copy the remaining bytes
                ppu_oam_bank_secondary[(ppu_oamev_slot * 4)] = ppu_fetch_data;
                ppu_oamev_m = 1;
                ppu_phase_index++;
                if (ppu_oamev_n == 0)
                    ppu_sprite0_should_hit = true;
            }
            else
            {
                ppu_oamev_m = 0;
                ppu_oamev_n++;
                if (ppu_oamev_n == 64)
                {
                    ppu_oamev_n = 0;
                    ppu_phase_index = 8;
                }
            }
        }
        // If the first phase found in-range sprite, it will continue with these phases to copy the remaining bytes of that sprites.
        private static void PPUOamPhase1()
        {
            ppu_oam_bank_secondary[(ppu_oamev_slot * 4) + ppu_oamev_m] = ppu_fetch_data;
            ppu_oamev_m = 2;
            ppu_phase_index++;
        }
        private static void PPUOamPhase2()
        {
            ppu_oam_bank_secondary[(ppu_oamev_slot * 4) + ppu_oamev_m] = ppu_fetch_data;
            ppu_oamev_m = 3;
            ppu_phase_index++;
        }
        private static void PPUOamPhase3()
        {
            ppu_oam_bank_secondary[(ppu_oamev_slot * 4) + ppu_oamev_m] = ppu_fetch_data;
            ppu_oamev_m = 0;
            // Increment n
            ppu_oamev_n++;
            // Increment the slot, i.e. open the next slot.
            ppu_oamev_slot++;

            if (ppu_oamev_n == 64)
            {
                // n overflows, go to phase 8 (Attempt (and fail) to copy OAM[n][0] into the next free slot in secondary OAM, and increment n (repeat until HBLANK is reached))
                ppu_oamev_n = 0;
                ppu_phase_index = 8;
            }
            else if (ppu_oamev_slot < 8)
            {
                // Repeat the first phases
                ppu_phase_index = 0;
            }
            else if (ppu_oamev_slot == 8)
            {
                // Go to phase to: 1 set overflow flag 2 to attempt to write into secondry oam but fails.
                ppu_phase_index = 4;
            }
        }
        // If the first phase does not found in-range sprite, it will continue with these phases.
        private static void PPUOamPhase4()
        {
            ppu_oamev_compare = ppu_clock_v >= ppu_fetch_data && ppu_clock_v < ppu_fetch_data + ppu_reg_2000_Sprite_size;

            // Check if read data in range
            if (ppu_oamev_compare)
            {
                ppu_oamev_m = 1;
                ppu_phase_index++;
                ppu_reg_2002_SpriteOverflow = true;
            }
            else
            {
                ppu_oamev_m++;
                if (ppu_oamev_m == 4)
                    ppu_oamev_m = 0;
                ppu_oamev_n++;
                if (ppu_oamev_n == 64)
                {
                    ppu_oamev_n = 0;
                    ppu_phase_index = 8;
                }
                else
                    ppu_phase_index = 4;
            }
        }
        private static void PPUOamPhase5()
        {
            ppu_oamev_m = 2;
            ppu_phase_index++;
        }
        private static void PPUOamPhase6()
        {
            ppu_oamev_m = 3;
            ppu_phase_index++;
        }
        private static void PPUOamPhase7()
        {
            ppu_oamev_m = 0;
            ppu_oamev_n++;
            if (ppu_oamev_n == 64)
            {
                ppu_oamev_n = 0;

            }
            else
            {
                //ppu_phase_index = 4;
            }
            ppu_phase_index = 8;
        }
        private static void PPUOamPhase8()
        {
            ppu_oamev_n++;
            if (ppu_oamev_n >= 64)
                ppu_oamev_n = 0;
        }
        #endregion
        private static void RenderPixel()
        {
            if (ppu_clock_v == ppu_clock_vblank_end)
                return;

            ppu_render_x = ppu_clock_h - 1;
            ppu_render_y = ppu_clock_v * 256;

            // Get the pixels.
            if (ppu_render_x < 8)
            {
                // This area is not rendered
                if (ppu_reg_2001_show_background_in_leftmost_8_pixels_of_screen)
                    ppu_bkg_current_pixel = 0x3F00 | ppu_bkg_pixels[ppu_render_x + ppu_vram_finex];
                else
                    ppu_bkg_current_pixel = 0x3F00;

                if (ppu_reg_2001_show_sprites_in_leftmost_8_pixels_of_screen)
                    ppu_spr_current_pixel = 0x3F10 | ppu_spr_pixels[ppu_render_x];
                else
                    ppu_spr_current_pixel = 0x3F10;
            }
            else
            {
                if (!ppu_reg_2001_show_background)
                    ppu_bkg_current_pixel = 0x3F00;
                else
                    ppu_bkg_current_pixel = 0x3F00 | ppu_bkg_pixels[ppu_render_x + ppu_vram_finex];

                if (!ppu_reg_2001_show_sprites || ppu_clock_v == 0)
                    ppu_spr_current_pixel = 0x3F10;
                else
                    ppu_spr_current_pixel = 0x3F10 | ppu_spr_pixels[ppu_render_x];

            }

            ppu_current_pixel = 0;

            // Use priority
            if ((ppu_spr_current_pixel & 0x8000) != 0)
            {
                ppu_current_pixel = ppu_spr_current_pixel;
            }
            else
            {
                ppu_current_pixel = ppu_bkg_current_pixel;
            }

            if ((ppu_bkg_current_pixel & 3) == 0)
            {
                ppu_current_pixel = ppu_spr_current_pixel;
                goto RENDER;
            }
            if ((ppu_spr_current_pixel & 3) == 0)
            {
                ppu_current_pixel = ppu_bkg_current_pixel;
                goto RENDER;
            }

            // Sprite 0 Hit
            if ((ppu_spr_pixels[ppu_render_x] & 0x4000) != 0)
            {
                //if (ppu_reg_2001_show_background && ppu_reg_2001_show_sprites)
                //   if (ppu_clock_h < 255)
                ppu_reg_2002_Sprite0Hit = true;
            }

        RENDER:
            {
                ppu_color_address_temp = (ushort)(ppu_current_pixel & 0x3FFF);
                // 1 read data
                if ((ppu_color_address_temp & 0x03) == 0)
                    ppu_color_data_temp = ppu_palette_bank[ppu_color_address_temp & 0x0C];
                else
                    ppu_color_data_temp = ppu_palette_bank[ppu_color_address_temp & 0x1F];
                // 2 construct color
                ppu_color_temp = (ppu_color_data_temp & 0x3F) | ppu_reg_2001_grayscale | ppu_reg_2001_emphasis;

                ppu_screen_pixels[ppu_render_x + ppu_render_y] = ppu_colors[ppu_color_temp];
            }
        }

        #region IO
        private static void PPUIORead(ref ushort addr, out byte value)
        {
            // PPU IO Registers. Emulating bus here.
            ppu_reg_io_addr = (byte)(addr & 0x7);
            ppu_reg_access_happened = true;
            ppu_reg_access_w = false;
            ppu_reg_read_func[ppu_reg_io_addr]();
            value = ppu_reg_io_db;
        }
        private static void PPUIOWrite(ref ushort addr, ref byte value)
        {
            // PPU IO Registers. Emulating bus here.
            ppu_reg_io_addr = (byte)(addr & 0x7);
            ppu_reg_io_db = value;
            ppu_reg_access_w = true;
            ppu_reg_access_happened = true;
        }

        // Called after cpu access io and ppu finishes one cycle. These functions should acknowledge last access (data update, flags updates ...etc)
        private static void PPUOnRegister2000()
        {
            // Only writes accepted
            if (!ppu_reg_access_w)
                return;
            // Update vram address
            ppu_vram_addr_temp = (ushort)((ppu_vram_addr_temp & 0x73FF) | ((ppu_reg_io_db & 0x3) << 10));

            if ((ppu_reg_io_db & 4) != 0)
                ppu_reg_2000_vram_address_increament = 32;
            else
                ppu_reg_2000_vram_address_increament = 1;

            if ((ppu_reg_io_db & 0x8) != 0)
                ppu_reg_2000_sprite_pattern_table_address_for_8x8_sprites = 0x1000;
            else
                ppu_reg_2000_sprite_pattern_table_address_for_8x8_sprites = 0x0000;

            if ((ppu_reg_io_db & 0x10) != 0)
                ppu_reg_2000_background_pattern_table_address = 0x1000;
            else
                ppu_reg_2000_background_pattern_table_address = 0x0000;

            if ((ppu_reg_io_db & 0x20) != 0)
                ppu_reg_2000_Sprite_size = 0x0010;
            else
                ppu_reg_2000_Sprite_size = 0x0008;

            if (!ppu_reg_2000_VBI && ((ppu_reg_io_db & 0x80) != 0))
            {
                if (ppu_reg_2002_VblankStartedFlag)// Special case ! NMI can be enabled anytime if vbl already set
                    PPU_NMI_Current = true;
            }
            ppu_reg_2000_VBI = ((ppu_reg_io_db & 0x80) != 0);

            if (!ppu_reg_2000_VBI)// NMI disable effect only at vbl set period (HClock between 1 and 3)
            {
                if (ppu_is_nmi_time)// 0 - 3
                    PPU_NMI_Current = false;
            }
        }
        private static void PPUOnRegister2001()
        {
            // Only writes accepted
            if (!ppu_reg_access_w)
                return;

            ppu_reg_2001_show_background_in_leftmost_8_pixels_of_screen = (ppu_reg_io_db & 0x2) != 0;
            ppu_reg_2001_show_sprites_in_leftmost_8_pixels_of_screen = (ppu_reg_io_db & 0x4) != 0;
            ppu_reg_2001_show_background = (ppu_reg_io_db & 0x8) != 0;
            ppu_reg_2001_show_sprites = (ppu_reg_io_db & 0x10) != 0;
            ppu_reg_2001_grayscale = (ppu_reg_io_db & 0x01) << 6;
            ppu_reg_2001_emphasis = (ppu_reg_io_db & 0xE0) << 2;
        }
        private static void PPUOnRegister2002()
        {
            // Only reads accepted
            if (ppu_reg_access_w)
                return;
            ppu_vram_flip_flop = false;
            ppu_reg_2002_VblankStartedFlag = false;
            if (ppu_clock_v == ppu_clock_vblank_start)
                PPU_NMI_Current = (ppu_reg_2002_VblankStartedFlag & ppu_reg_2000_VBI);
        }
        private static void PPUOnRegister2003()
        {
            // Only writes accepted
            if (!ppu_reg_access_w)
                return;
            ppu_reg_2003_oam_addr = ppu_reg_io_db;
        }
        private static void PPUOnRegister2004()
        {
            if (ppu_reg_access_w)
            {
                if (ppu_clock_v < 240 && IsRenderingOn())
                    ppu_reg_io_db = 0xFF;
                if ((ppu_reg_2003_oam_addr & 0x03) == 0x02)
                    ppu_reg_io_db &= 0xE3;
                // ON Writes
                ppu_oam_bank[ppu_reg_2003_oam_addr] = ppu_reg_io_db;
                ppu_reg_2003_oam_addr = (byte)((ppu_reg_2003_oam_addr + 1) & 0xFF);
            }
            // Nothing happens on reads
        }
        private static void PPUOnRegister2005()
        {
            // Only writes accepted
            if (!ppu_reg_access_w)
                return;
            if (!ppu_vram_flip_flop)
            {
                ppu_vram_addr_temp = (ushort)((ppu_vram_addr_temp & 0x7FE0) | ((ppu_reg_io_db & 0xF8) >> 3));
                ppu_vram_finex = (byte)(ppu_reg_io_db & 0x07);
            }
            else
            {
                ppu_vram_addr_temp = (ushort)((ppu_vram_addr_temp & 0x0C1F) | ((ppu_reg_io_db & 0x7) << 12) | ((ppu_reg_io_db & 0xF8) << 2));
            }
            ppu_vram_flip_flop = !ppu_vram_flip_flop;
        }
        private static void PPUOnRegister2006()
        {
            // Only writes accepted
            if (!ppu_reg_access_w)
                return;
            if (!ppu_vram_flip_flop)
            {
                ppu_vram_addr_temp = (ushort)((ppu_vram_addr_temp & 0x00FF) | ((ppu_reg_io_db & 0x3F) << 8));
            }
            else
            {
                ppu_vram_addr_temp = (ushort)((ppu_vram_addr_temp & 0x7F00) | ppu_reg_io_db);
                ppu_vram_addr = ppu_vram_addr_temp;
                mem_board.OnPPUAddressUpdate(ref ppu_vram_addr);
            }
            ppu_vram_flip_flop = !ppu_vram_flip_flop;
        }
        private static void PPUOnRegister2007()
        {
            if (ppu_reg_access_w)
            {
                ppu_vram_addr_access_temp = (ushort)(ppu_vram_addr & 0x3FFF);
                // ON Writes
                if (ppu_vram_addr_access_temp < 0x2000)
                {
                    mem_board.WriteCHR(ref ppu_vram_addr_access_temp, ref ppu_reg_io_db);
                }
                else if (ppu_vram_addr_access_temp < 0x3F00)
                {
                    mem_board.WriteNMT(ref ppu_vram_addr_access_temp, ref ppu_reg_io_db);
                }
                else
                {
                    if ((ppu_vram_addr_access_temp & 3) != 0)
                        ppu_palette_bank[ppu_vram_addr_access_temp & 0x1F] = ppu_reg_io_db;
                    else
                        ppu_palette_bank[ppu_vram_addr_access_temp & 0x0C] = ppu_reg_io_db;
                }
            }
            else
            {
                // ON Reads
                if ((ppu_vram_addr & 0x3F00) == 0x3F00)
                    ppu_vram_addr_access_temp = (ushort)(ppu_vram_addr & 0x2FFF);
                else
                    ppu_vram_addr_access_temp = (ushort)(ppu_vram_addr & 0x3FFF);

                // Update the vram data bus
                if (ppu_vram_addr_access_temp < 0x2000)
                {
                    mem_board.ReadCHR(ref ppu_vram_addr_access_temp, out ppu_vram_data);
                }
                else if (ppu_vram_addr_access_temp < 0x3F00)
                {
                    mem_board.ReadNMT(ref ppu_vram_addr_access_temp, out ppu_vram_data);
                }
            }
            ppu_vram_addr = (ushort)((ppu_vram_addr + ppu_reg_2000_vram_address_increament) & 0x7FFF);
            mem_board.OnPPUAddressUpdate(ref ppu_vram_addr);
        }

        // Called when cpu access io. These functions should update the ppu data bus.
        private static void PPURead2000()
        {
        }
        private static void PPURead2001()
        {
        }
        private static void PPURead2002()
        {
            ppu_reg_io_db = (byte)((ppu_reg_io_db & 0xDF) | (ppu_reg_2002_SpriteOverflow ? 0x20 : 0x0));
            ppu_reg_io_db = (byte)((ppu_reg_io_db & 0xBF) | (ppu_reg_2002_Sprite0Hit ? 0x40 : 0x0));
            ppu_reg_io_db = (byte)((ppu_reg_io_db & 0x7F) | (ppu_reg_2002_VblankStartedFlag ? 0x80 : 0x0));
        }
        private static void PPURead2003()
        {
        }
        private static void PPURead2004()
        {
            ppu_reg_io_db = ppu_oam_bank[ppu_reg_2003_oam_addr];
            if (ppu_clock_v < 240 && IsRenderingOn())
            {
                if (ppu_clock_h < 64)
                    ppu_reg_io_db = 0xFF;
                else if (ppu_clock_h < 192)
                    ppu_reg_io_db = ppu_oam_bank[((ppu_clock_h - 64) << 1) & 0xFC];
                else if (ppu_clock_h < 256)
                    ppu_reg_io_db = ((ppu_clock_h & 1) == 1) ? ppu_oam_bank[0xFC] : ppu_oam_bank[((ppu_clock_h - 192) << 1) & 0xFC];
                else if (ppu_clock_h < 320)
                    ppu_reg_io_db = 0xFF;
                else
                    ppu_reg_io_db = ppu_oam_bank[0];
            }
        }
        private static void PPURead2005()
        {
        }
        private static void PPURead2006()
        {
        }
        private static void PPURead2007()
        {
            ppu_vram_addr_access_temp = (ushort)(ppu_vram_addr & 0x3FFF);
            if (ppu_vram_addr_access_temp < 0x3F00)
                // Reading will put the vram data bus into the io bus, 
                // then later it will transfer the data from vram datas bus into io data bus. 
                // This causes the 0x2007 dummy reads effect.
                ppu_reg_io_db = ppu_vram_data;
            else
            {
                // Reading from palettes puts the value in the io bus immediately
                if ((ppu_vram_addr_access_temp & 3) != 0)
                    ppu_reg_io_db = ppu_palette_bank[ppu_vram_addr_access_temp & 0x1F];
                else
                    ppu_reg_io_db = ppu_palette_bank[ppu_vram_addr_access_temp & 0x0C];
            }
        }
        #endregion

        internal static bool IsRenderingOn()
        {
            return (ppu_reg_2001_show_background || ppu_reg_2001_show_sprites);
        }

        internal static bool IsInRender()
        {
            return (ppu_clock_v < 240) || (ppu_clock_v == ppu_clock_vblank_end);
        }

        private static void PPUWriteState(ref System.IO.BinaryWriter bin)
        {
            bin.Write(ppu_clock_h);
            bin.Write(ppu_clock_v);
            bin.Write(ppu_clock_vblank_start);
            bin.Write(ppu_clock_vblank_end);
            bin.Write(ppu_use_odd_cycle);
            bin.Write(ppu_use_odd_swap);
            bin.Write(ppu_is_nmi_time);
            bin.Write(ppu_frame_finished);
            bin.Write(ppu_oam_bank);
            bin.Write(ppu_oam_bank_secondary);
            bin.Write(ppu_palette_bank);
            bin.Write(ppu_reg_io_db);
            bin.Write(ppu_reg_io_addr);
            bin.Write(ppu_reg_access_happened);
            bin.Write(ppu_reg_access_w);
            bin.Write(ppu_reg_2000_vram_address_increament);
            bin.Write(ppu_reg_2000_sprite_pattern_table_address_for_8x8_sprites);
            bin.Write(ppu_reg_2000_background_pattern_table_address);
            bin.Write(ppu_reg_2000_Sprite_size);
            bin.Write(ppu_reg_2000_VBI);
            bin.Write(ppu_reg_2001_show_background_in_leftmost_8_pixels_of_screen);
            bin.Write(ppu_reg_2001_show_sprites_in_leftmost_8_pixels_of_screen);
            bin.Write(ppu_reg_2001_show_background);
            bin.Write(ppu_reg_2001_show_sprites);
            bin.Write(ppu_reg_2001_grayscale);
            bin.Write(ppu_reg_2001_emphasis);
            bin.Write(ppu_reg_2002_SpriteOverflow);
            bin.Write(ppu_reg_2002_Sprite0Hit);
            bin.Write(ppu_reg_2002_VblankStartedFlag);
            bin.Write(ppu_reg_2003_oam_addr);
            bin.Write(ppu_vram_addr);
            bin.Write(ppu_vram_data);
            bin.Write(ppu_vram_addr_temp);
            bin.Write(ppu_vram_addr_access_temp);
            bin.Write(ppu_vram_flip_flop);
            bin.Write(ppu_vram_finex);
            bin.Write(ppu_bkgfetch_nt_addr);
            bin.Write(ppu_bkgfetch_nt_data);
            bin.Write(ppu_bkgfetch_at_addr);
            bin.Write(ppu_bkgfetch_at_data);
            bin.Write(ppu_bkgfetch_lb_addr);
            bin.Write(ppu_bkgfetch_lb_data);
            bin.Write(ppu_bkgfetch_hb_addr);
            bin.Write(ppu_bkgfetch_hb_data);
            bin.Write(ppu_sprfetch_slot);
            bin.Write(ppu_sprfetch_y_data);
            bin.Write(ppu_sprfetch_t_data);
            bin.Write(ppu_sprfetch_at_data);
            bin.Write(ppu_sprfetch_x_data);
            bin.Write(ppu_sprfetch_lb_addr);
            bin.Write(ppu_sprfetch_lb_data);
            bin.Write(ppu_sprfetch_hb_addr);
            bin.Write(ppu_sprfetch_hb_data);
            bin.Write(ppu_bkg_render_i);
            bin.Write(ppu_bkg_render_pos);
            bin.Write(ppu_bkg_render_tmp_val);
            bin.Write(ppu_bkg_current_pixel);
            bin.Write(ppu_spr_current_pixel);
            bin.Write(ppu_current_pixel);
            bin.Write(ppu_render_x);
            bin.Write(0);// this is old value for color and, add 0 for compatibility
            bin.Write(ppu_oamev_n);
            bin.Write(ppu_oamev_m);
            bin.Write(ppu_oamev_compare);
            bin.Write(ppu_oamev_slot);
            bin.Write(ppu_fetch_data);
            bin.Write(ppu_phase_index);
            bin.Write(ppu_sprite0_should_hit);
        }
        private static void PPUReadState(ref System.IO.BinaryReader bin)
        {
            ppu_clock_h = bin.ReadInt32();
            ppu_clock_v = bin.ReadUInt16();
            ppu_clock_vblank_start = bin.ReadUInt16();
            ppu_clock_vblank_end = bin.ReadUInt16();
            ppu_use_odd_cycle = bin.ReadBoolean();
            ppu_use_odd_swap = bin.ReadBoolean();
            ppu_is_nmi_time = bin.ReadBoolean();
            ppu_frame_finished = bin.ReadBoolean();
            bin.Read(ppu_oam_bank, 0, ppu_oam_bank.Length);
            bin.Read(ppu_oam_bank_secondary, 0, ppu_oam_bank_secondary.Length);
            bin.Read(ppu_palette_bank, 0, ppu_palette_bank.Length);
            ppu_reg_io_db = bin.ReadByte();
            ppu_reg_io_addr = bin.ReadByte();
            ppu_reg_access_happened = bin.ReadBoolean();
            ppu_reg_access_w = bin.ReadBoolean();
            ppu_reg_2000_vram_address_increament = bin.ReadByte();
            ppu_reg_2000_sprite_pattern_table_address_for_8x8_sprites = bin.ReadUInt16();
            ppu_reg_2000_background_pattern_table_address = bin.ReadUInt16();
            ppu_reg_2000_Sprite_size = bin.ReadByte();
            ppu_reg_2000_VBI = bin.ReadBoolean();
            ppu_reg_2001_show_background_in_leftmost_8_pixels_of_screen = bin.ReadBoolean();
            ppu_reg_2001_show_sprites_in_leftmost_8_pixels_of_screen = bin.ReadBoolean();
            ppu_reg_2001_show_background = bin.ReadBoolean();
            ppu_reg_2001_show_sprites = bin.ReadBoolean();
            ppu_reg_2001_grayscale = bin.ReadInt32();
            ppu_reg_2001_emphasis = bin.ReadInt32();
            ppu_reg_2002_SpriteOverflow = bin.ReadBoolean();
            ppu_reg_2002_Sprite0Hit = bin.ReadBoolean();
            ppu_reg_2002_VblankStartedFlag = bin.ReadBoolean();
            ppu_reg_2003_oam_addr = bin.ReadByte();
            ppu_vram_addr = bin.ReadUInt16();
            ppu_vram_data = bin.ReadByte();
            ppu_vram_addr_temp = bin.ReadUInt16();
            ppu_vram_addr_access_temp = bin.ReadUInt16();
            ppu_vram_flip_flop = bin.ReadBoolean();
            ppu_vram_finex = bin.ReadByte();
            ppu_bkgfetch_nt_addr = bin.ReadUInt16();
            ppu_bkgfetch_nt_data = bin.ReadByte();
            ppu_bkgfetch_at_addr = bin.ReadUInt16();
            ppu_bkgfetch_at_data = bin.ReadByte();
            ppu_bkgfetch_lb_addr = bin.ReadUInt16();
            ppu_bkgfetch_lb_data = bin.ReadByte();
            ppu_bkgfetch_hb_addr = bin.ReadUInt16();
            ppu_bkgfetch_hb_data = bin.ReadByte();
            ppu_sprfetch_slot = bin.ReadInt32();
            ppu_sprfetch_y_data = bin.ReadByte();
            ppu_sprfetch_t_data = bin.ReadByte();
            ppu_sprfetch_at_data = bin.ReadByte();
            ppu_sprfetch_x_data = bin.ReadByte();
            ppu_sprfetch_lb_addr = bin.ReadUInt16();
            ppu_sprfetch_lb_data = bin.ReadByte();
            ppu_sprfetch_hb_addr = bin.ReadUInt16();
            ppu_sprfetch_hb_data = bin.ReadByte();
            ppu_bkg_render_i = bin.ReadInt32();
            ppu_bkg_render_pos = bin.ReadInt32();
            ppu_bkg_render_tmp_val = bin.ReadInt32();
            ppu_bkg_current_pixel = bin.ReadInt32();
            ppu_spr_current_pixel = bin.ReadInt32();
            ppu_current_pixel = bin.ReadInt32();
            ppu_render_x = bin.ReadInt32();
            /*ppu_color_and =*/
            bin.ReadInt32();// Read a 4 byte for compatibility
            ppu_oamev_n = bin.ReadByte();
            ppu_oamev_m = bin.ReadByte();
            ppu_oamev_compare = bin.ReadBoolean();
            ppu_oamev_slot = bin.ReadByte();
            ppu_fetch_data = bin.ReadByte();
            ppu_phase_index = bin.ReadByte();
            ppu_sprite0_should_hit = bin.ReadBoolean();
        }
    }
}
