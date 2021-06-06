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
    public class RendererSettings : ISettings
    {
        public RendererSettings(string path) : base(path)
        {
        }
        // Video
        /// <summary>
        /// Current video provider id. This option will switch the video provider.
        /// </summary>
        public string Video_ProviderID = "";
        public bool Vid_AutoStretch = true;
        public bool Vid_Res_Upscale = true;
        public int Vid_Res_W = 640;
        public int Vid_Res_H = 480;
        public int Vid_StretchMultiply = 2;
        public bool Vid_KeepAspectRatio = true;
        public bool Vid_ShowFPS = false;
        public bool Vid_Fullscreen = false;
        public bool Vid_HardwareVertexProcessing = false;
        public bool Vid_VSync = false;
        public bool Vid_ShowNotifications = true;
        public int Vid_Filter = 1;// 0 means nearest, 1 means normal (linear)
        public double Vid_LightAdd = 100;
        public double Vid_SaturAdd = 23;
        // Frame skipp
        public bool FrameSkipEnabled = false;
        public int FrameSkipInterval = 2;

        // Thread
        public bool UseEmuThread = true;

        // Audio
        /// <summary>
        /// Current audio provider id. This option will switch the audio provider.
        /// </summary>
        public string Audio_ProviderID = "";
        public int Audio_Volume = 100;
        public bool Audio_SoundEnabled = true;
        public int Audio_Frequency = 44100;
        public int Audio_InternalSamplesCount = 4096;
        public int Audio_InternalPeekLimit = 124;
        public int Audio_PlaybackAmplitude = 200;
        public int Audio_PlaybackBufferSizeInKB = 8;
        public bool Audio_UseDefaultMixer = true;
        public bool Audio_ChannelEnabled_SQ1 = true;
        public bool Audio_ChannelEnabled_SQ2 = true;
        public bool Audio_ChannelEnabled_NOZ = true;
        public bool Audio_ChannelEnabled_TRL = true;
        public bool Audio_ChannelEnabled_DMC = true;
        public bool Audio_ChannelEnabled_MMC5_SQ1 = true;
        public bool Audio_ChannelEnabled_MMC5_SQ2 = true;
        public bool Audio_ChannelEnabled_MMC5_PCM = true;
        public bool Audio_ChannelEnabled_VRC6_SQ1 = true;
        public bool Audio_ChannelEnabled_VRC6_SQ2 = true;
        public bool Audio_ChannelEnabled_VRC6_SAW = true;
        public bool Audio_ChannelEnabled_SUN1 = true;
        public bool Audio_ChannelEnabled_SUN2 = true;
        public bool Audio_ChannelEnabled_SUN3 = true;

        public bool Audio_ChannelEnabled_NMT1 = true;
        public bool Audio_ChannelEnabled_NMT2 = true;
        public bool Audio_ChannelEnabled_NMT3 = true;
        public bool Audio_ChannelEnabled_NMT4 = true;
        public bool Audio_ChannelEnabled_NMT5 = true;
        public bool Audio_ChannelEnabled_NMT6 = true;
        public bool Audio_ChannelEnabled_NMT7 = true;
        public bool Audio_ChannelEnabled_NMT8 = true;
    }
}
