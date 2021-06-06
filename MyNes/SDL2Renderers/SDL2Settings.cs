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
using MyNes.Core;

namespace MyNes
{
    class SDL2Settings : ISettings
    {
        public SDL2Settings(string path) : base(path)
        {

        }
        public int Video_EnableOpenglShaders = 0;
        public string Video_Driver = "opengl";
        public int Audio_Device_Index = 0;
        public bool Video_Accelerated = false;
        public bool Video_Software = false;
        public int Video_FullScreen_Mode = 0;
    }
}
