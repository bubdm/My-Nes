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
    public enum PaletteSelectSetting : int
    {
        /// <summary>
        /// Auto select palette generator depending on selected region.
        /// </summary>
        AUTO = 0,
        /// <summary>
        /// Always use the NTSC palette generator.
        /// </summary>
        ForceNTSC = 1,
        /// <summary>
        /// Always use the PALB palette generator.
        /// </summary>
        ForcePALB = 2,
        /// <summary>
        /// Use palette file.
        /// </summary>
        File = 3
    }
}
