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
    internal delegate void MemReadAccess(ref ushort addr, out byte value);
    internal delegate void MemWriteAccess(ref ushort addr, ref byte value);
    internal delegate void RenderVideoFrame(ref int[] buffer);
    internal delegate void RenderAudioSamples(ref short[] buffer, ref int samples_added);
    internal delegate void TogglePause(bool paused);
    internal delegate void GetIsPlaying(out bool playing);
}
