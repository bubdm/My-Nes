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
    class SoundLowPassFilter
    {
        public SoundLowPassFilter(double k)
        {
            this.K = k;
            K_1 = 1 - k;
        }
        private double K;
        private double K_1;
        private double y;
        private double y_1;
        private double x;
        private double x_1;
        public void Reset(double k)
        {
            y = y_1 = x = x_1 = 0;
            this.K = k;
            K_1 = 1 - k;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sample">X</param>
        /// <param name="filtered">Y</param>
        public void DoFiltering(double sample, out double filtered)
        {
            //filtered = y = y_1 + (K * (sample - y_1));
            filtered = (K * sample) + (K_1 * y_1);
            x_1 = sample;
            y_1 = filtered;
        }
        public static double GetK(double dt, double fc)
        {
            double V = (2 * Math.PI * dt * fc);

            return (V / (V + 1));
        }
    }
    class SoundHighPassFilter
    {
        public SoundHighPassFilter(double k)
        {
            this.K = k;
        }
        private double K;
        private double y_1;
        private double x_1;
        public void Reset()
        {
            y_1 = x_1 = 0;
        }
        public void DoFiltering(double sample, out double filtered)
        {
            filtered = (K * y_1) + (K * (sample - x_1));
            x_1 = sample;
            y_1 = filtered;
        }
        public static double GetK(double dt, double fc)
        {
            double V = (2 * Math.PI * dt * fc);

            return (V / (V + 1));
        }
    }
    class SoundDCBlockerFilter
    {
        public SoundDCBlockerFilter(double R)
        {
            this.R = R;
        }
        private double R;
        private double y;
        private double y_1;
        private double x;
        private double x_1;
        public void Reset()
        {
            y = y_1 = x = x_1 = 0;
        }
        public void DoFiltering(double sample, out double filtered)
        {
            x = sample;
            filtered = y = x - x_1 + (R * y_1);
            x_1 = x;
            y_1 = y;
        }
    }
}
