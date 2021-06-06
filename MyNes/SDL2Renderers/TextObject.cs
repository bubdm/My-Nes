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
using System.Drawing;
using SDL2;

namespace MyNes
{
    class TextObject : IDrawable
    {
        public TextObject(IntPtr window, IntPtr renderer, string text, IntPtr font, Color color, Point position, bool blended)
        {
            base.position = position;
            this.text = text;
            this.color = color;
            this.font = font;
            this.blended = blended;
            backgroundColor = Color.Black;
            UpdateTexture(ref window, ref renderer);
        }

        protected SDL.SDL_Color staColor;
        protected SDL.SDL_Color bkColor;
        protected string text;
        protected Color color;
        protected Color backgroundColor;
        protected bool blended;
        protected IntPtr font;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public void SetText(string text, ref IntPtr window, ref IntPtr renderer)
        {
            if (this.text != text)
            {
                this.text = text;
                doTexUpdate = true;
            }
        }
        protected override IntPtr GetSurface()
        {
            // Create the surface for the text
            staColor = new SDL.SDL_Color();
            staColor.a = color.A;
            staColor.b = color.B;
            staColor.g = color.G;
            staColor.r = color.R;
            bkColor = new SDL.SDL_Color();
            bkColor.a = backgroundColor.A;
            bkColor.b = backgroundColor.B;
            bkColor.g = backgroundColor.G;
            bkColor.r = backgroundColor.R;

            return blended ? SDL_ttf.TTF_RenderUTF8_Blended(font, text, staColor) : SDL_ttf.TTF_RenderUNICODE_Shaded(font, text, staColor, bkColor);
        }
    }
}