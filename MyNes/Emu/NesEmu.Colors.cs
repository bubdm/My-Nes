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
    public partial class NesEmu
    {
        // Color sequences
        private static byte[] color_temp_blue__sequence = { 7, 7, 6, 5, 4, 3, 2, 1, 2, 3, 4, 5, 6, 1, 1, 1 };
        private static byte[] color_temp_red___sequence = { 7, 1, 2, 3, 4, 5, 6, 7, 6, 5, 4, 3, 2, 1, 1, 1 };
        private static byte[] color_temp_green_sequence = { 7, 1, 1, 1, 1, 2, 3, 4, 5, 6, 7, 6, 5, 1, 1, 1 };

        internal static double Color_Light_Add;// 0 to 1000
        internal static double Color_Saturation_Add;// 0 to 1000

        /// <summary>
        /// Generate 64 colors index
        /// </summary>
        /// <param name="colors">The colors array to fill, must be 64 in size or larger</param>
        /// <param name="system_index">The system index</param>
        internal static void GenerrateColors(ref int[] colors, int system_index)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = MakeRGBColor(i, system_index) | (0xFF << 24);
            }
        }
        private static int MakeRGBColor(int pixel_data, int system_index)
        {
            // Implements the color decoding as written here <https://github.com/alaahadid/Nes-Docs/blob/main/Color%20Decoding%20In%20PPU.txt>.
            // It is better to be stored in color indexes for better performance

            // The expected pixel should be
            // bit ...
            // 15 14 13 12  11 10 9 8   7 6 5 4  3 2 1 0
            //                 ´  | |   | | | |  | | | |
            //                    | |   | | | |  +-+-+-+---- Color sequence value
            //                    | |   | | +-+------------- Color level or strength
            //                    | |   | +----------------- Gray scale bit
            //                    +-+---+------------------- Emphasizes bits


            int val = pixel_data & 0xF;// or sequence step 
            int level = (pixel_data & 0x30) << 2;
            bool gray = (pixel_data & 0x40) != 0;
            int emphasis = (pixel_data & 0x380) >> 7;

            // GRAY SCALE !! 
            if (gray)
            {
                val = 0;
            }

            if (val >= 14)
                level = 0;

            int blue = ((color_temp_blue__sequence[val] << 3) | level);
            int red = ((color_temp_red___sequence[val] << 3) | level);
            int green = ((color_temp_green_sequence[val] << 3) | level);

            bool e_red = false;
            bool e_green = false;
            bool e_blue = false;

            // EMPHASIZE !!
            if (system_index == 0)
                e_red = (emphasis & 0x1) != 0;
            else
                e_green = (emphasis & 0x1) != 0;

            if (system_index == 0)
                e_green = (emphasis & 0x2) != 0;
            else
                e_red = (emphasis & 0x2) != 0;

            e_blue = (emphasis & 0x4) != 0;

            if (e_blue)
            {
                blue |= 0x7;
            }
            if (e_red)
            {
                red |= 0x7;
            }
            if (e_green)
            {
                green |= 0x7;
            }

            // We are using FCC NTSC Standard
            // Convert RGB to YIQ
            // 1 We need to convert r,g,b from 0-255 value into 0-1 (double)
            /* double red_one = (double)red / 255.0;
             double green_one = (double)green / 255.0;
             double blue_one = (double)blue / 255.0;

             // 2 Apply the matrix to get YIQ
             double Y = (0.30 * red_one) + (0.59 * green_one) + (0.11 * blue_one);
             double I = (0.599 * red_one) + (-0.2773 * green_one) + (-0.3217 * blue_one);
             double Q = (0.213 * red_one) + (-0.5251 * green_one) + (0.3121 * blue_one);

             // MODIFY 
             // SATURATION:

             //Y += 0.023;
             //I += 0.04;
             //Q += 0.017;

             // 3 Convert back from YIQ into RGB
             double red_new = Y + (0.9469 * I) + (0.6236 * Q);
             double green_new = Y + (-0.2748 * I) + (-0.6357 * Q);
             double blue_new = Y + (-1.1 * I) + (1.7 * Q);

             // 4 Return from 0-1 value into 0-255 value
             red = (int)(red_new * 255);
             green = (int)(green_new * 255);
             blue = (int)(blue_new * 255);*/

            double l = 0;
            double h = 0;
            double s = 0;

            RgbToHls(red, green, blue, out h, out l, out s);

            // modify
            h -= 15;// In real nes, HUE is adjusted by 15 degree
            double l_diff = 1 - l;
            l += (l_diff * Color_Light_Add) / 1000.0;

            double s_diff = 1 - s;
            s += (s_diff * Color_Saturation_Add) / 1000.0;

            HlsToRgb(h, l, s, out red, out green, out blue);

            // return color R8G8B8 in format
            return ((red & 0xFF) << 16) | ((green & 0xFF) << 8) | (blue & 0xFF);
        }
        // Convert an RGB value into an HLS value.
        private static void RgbToHls(int r, int g, int b,
            out double h, out double l, out double s)
        {
            // Convert RGB to a 0.0 to 1.0 range.
            double double_r = r / 255.0;
            double double_g = g / 255.0;
            double double_b = b / 255.0;

            // Get the maximum and minimum RGB components.
            double max = double_r;
            if (max < double_g) max = double_g;
            if (max < double_b) max = double_b;

            double min = double_r;
            if (min > double_g) min = double_g;
            if (min > double_b) min = double_b;

            double diff = max - min;
            l = (max + min) / 2;
            if (Math.Abs(diff) < 0.00001)
            {
                s = 0;
                h = 0;  // H is really undefined.
            }
            else
            {
                if (l <= 0.5) s = diff / (max + min);
                else s = diff / (2 - max - min);

                double r_dist = (max - double_r) / diff;
                double g_dist = (max - double_g) / diff;
                double b_dist = (max - double_b) / diff;

                if (double_r == max) h = b_dist - g_dist;
                else if (double_g == max) h = 2 + r_dist - b_dist;
                else h = 4 + g_dist - r_dist;

                h = h * 60;
                if (h < 0) h += 360;
            }
        }// Convert an HLS value into an RGB value.
        private static void HlsToRgb(double h, double l, double s,
            out int r, out int g, out int b)
        {
            double p2;
            if (l <= 0.5) p2 = l * (1 + s);
            else p2 = l + s - l * s;

            double p1 = 2 * l - p2;
            double double_r, double_g, double_b;
            if (s == 0)
            {
                double_r = l;
                double_g = l;
                double_b = l;
            }
            else
            {
                double_r = QqhToRgb(p1, p2, h + 120);
                double_g = QqhToRgb(p1, p2, h);
                double_b = QqhToRgb(p1, p2, h - 120);
            }

            // Convert RGB to the 0 to 255 range.
            r = (int)(double_r * 255.0);
            g = (int)(double_g * 255.0);
            b = (int)(double_b * 255.0);
        }

        private static double QqhToRgb(double q1, double q2, double hue)
        {
            if (hue > 360) hue -= 360;
            else if (hue < 0) hue += 360;

            if (hue < 60) return q1 + (q2 - q1) * hue / 60;
            if (hue < 180) return q2;
            if (hue < 240) return q1 + (q2 - q1) * (240 - hue) / 60;
            return q1;
        }
    }
}
