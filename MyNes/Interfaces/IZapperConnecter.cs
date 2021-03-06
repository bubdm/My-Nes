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
    /// <summary>
    /// Represents a Zapper connecter
    /// </summary>
    public abstract class IZapperConnecter
    {
        protected bool Trigger;
        protected bool State;

        /// <summary>
        /// Nes emulation engine call this at frame end, can be used to update input device status.
        /// </summary>
        public abstract void Update();
        /// <summary>
        /// Nes emulation engine call this at $4016 writes.
        /// </summary>
        /// <returns>The input data</returns>
        public virtual byte GetData()
        {
            return (byte)((Trigger ? 0x10 : 0x0) | (State ? 0x8 : 0x0));
        }
    }
}
