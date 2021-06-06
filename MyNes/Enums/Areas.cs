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
    enum PRGArea : byte
    {
        Area4000 = 4,
        Area5000 = 5,
        Area6000 = 6,
        Area7000 = 7,
        Area8000 = 8,
        Area9000 = 9,
        AreaA000 = 10,
        AreaB000 = 11,
        AreaC000 = 12,
        AreaD000 = 13,
        AreaE000 = 14,
        AreaF000 = 15
    }
    enum CHRArea : byte
    {
        Area0000 = 0,
        Area0400 = 1,
        Area0800 = 2,
        Area0C00 = 3,
        Area1000 = 4,
        Area1400 = 5,
        Area1800 = 6,
        Area1C00 = 7,
    }
    enum NTArea : byte
    {
        Area2000NT0 = 0,
        Area2400NT1 = 1,
        Area2800NT2 = 2,
        Area2C00NT3 = 3,
    }
}
