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
    public class BoardInfoObject
    {
        /// <summary>
        /// Get the name of this board.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; internal set; }
        /// <summary>
        /// The mapper number this board presents.
        /// </summary>
        public int MapperNumber { get; internal set; }
        /// <summary>
        /// Get if this board is supported or not.
        /// </summary>
        public bool IsSupported { get; internal set; }
        /// <summary>
        /// Get the issue (if any) with this board.
        /// </summary>
        public string Issues { get; internal set; }
        /// <summary>
        /// Get if this mapper have issues or not
        /// </summary>
        public bool HasIssues { get; internal set; }
    }
}
