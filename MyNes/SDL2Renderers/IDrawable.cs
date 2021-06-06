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
using System.Runtime.InteropServices;
using SDL2;

namespace MyNes
{
    abstract class IDrawable
    {
        private bool isRendering;
        protected bool doTexUpdate;
        internal bool mouse_is_down;
        internal bool mouse_is_over;
        internal bool surround_by_rect;
        internal SDL.SDL_Color surround_color;
        protected Point position;
        internal IntPtr Texture;
        protected bool isTextureCreated;
        internal SDL.SDL_Rect Rect1;
        internal SDL.SDL_Rect Rect2;

        internal bool custom_region;
        internal int custom_region_x;
        internal int custom_region_w;


        internal virtual Point Position
        {
            get { return position; }
            set
            {
                position = value;
            }
        }
        internal virtual int X1 { get { return Rect2.x; } }
        internal virtual int X2 { get { return Rect2.x + Rect2.w; } }
        internal virtual int Y1 { get { return Rect2.y; } }
        internal virtual int Y2 { get { return Rect2.y + Rect2.h; } }

        internal virtual void SetTextRegion(int x, int w)
        {
            custom_region = true;
            custom_region_x = x;
            custom_region_w = w;
        }
        protected abstract IntPtr GetSurface();
        protected virtual void UpdateTexture(ref IntPtr window, ref IntPtr renderer)
        {
            if (isRendering)
            {
                doTexUpdate = true;
                return;
            }
            if (isTextureCreated)
            {
                SDL.SDL_DestroyTexture(Texture);
                isTextureCreated = false;
            }

            IntPtr sur = GetSurface();

            //Create texture from surface pixels

            Texture = SDL.SDL_CreateTextureFromSurface(renderer, sur);
            isTextureCreated = true;

            int window_w = 0;
            int window_h = 0;
            SDL.SDL_GetWindowSize(window, out window_w, out window_h);

            SDL.SDL_Surface surface = (SDL.SDL_Surface)Marshal.PtrToStructure(sur, typeof(SDL.SDL_Surface));

            Rect2.x = position.X;
            Rect2.y = position.Y;
            Rect2.h = surface.h;
            Rect2.w = surface.w;

            if (custom_region)
            {
                Rect1.x = custom_region_x;
                Rect1.y = 0;
                Rect1.h = surface.h;
                Rect1.w = custom_region_w;

                Rect2.h = surface.h;
                Rect2.w = custom_region_w;
            }
            else
            {
                Rect1.x = 0;
                Rect1.y = 0;
                Rect1.h = surface.h;
                Rect1.w = surface.w;
            }
            Rect1.x = 0;
            Rect1.y = 0;
            Rect1.h = surface.h;
            Rect1.w = surface.w;
            //Get rid of old surface
            Marshal.DestroyStructure(sur, typeof(SDL.SDL_Surface));
            SDL.SDL_FreeSurface(sur);
        }
        internal void Destroy()
        {
            if (isTextureCreated)
            {
                SDL.SDL_DestroyTexture(Texture);
                isTextureCreated = false;
                doTexUpdate = true;
            }
        }
        internal void OnRender(ref IntPtr window, ref IntPtr renderer)
        {
            isRendering = true;
            Rect2.x = Position.X;
            Rect2.y = Position.Y;

            SDL.SDL_RenderCopy(renderer, Texture, ref Rect1, ref Rect2);

            if (surround_by_rect)
            {
                SDL.SDL_Rect rec = new SDL.SDL_Rect();
                rec.x = Rect2.x - 1;
                rec.y = Rect2.y - 1;
                rec.h = Rect2.h + 1;
                rec.w = Rect2.w + 1;
                SDL.SDL_SetRenderDrawColor(renderer, surround_color.r, surround_color.g, surround_color.b, surround_color.a);
                SDL.SDL_RenderDrawRect(renderer, ref rec);
            }
            isRendering = false;
            if (doTexUpdate)
            {
                UpdateTexture(ref window, ref renderer);
                doTexUpdate = false;
            }
        }
        internal void OnUpdate()
        {
            OnUpdating();
        }
        protected virtual void OnUpdating()
        {
        }
    }
}