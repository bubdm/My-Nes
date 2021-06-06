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
using MyNes.Core;
using SlimDX;
using SlimDX.XInput;

namespace MyNes
{
    class NesJoypadXControllerConnection : IJoypadConnecter
    {
        public NesJoypadXControllerConnection(string guid, IInputSettingsJoypad settings)
        {
            switch (guid)
            {
                case "x-controller-1": x_controller = new Controller(UserIndex.One); break;
                case "x-controller-2": x_controller = new Controller(UserIndex.Two); break;
                case "x-controller-3": x_controller = new Controller(UserIndex.Three); break;
                case "x-controller-4": x_controller = new Controller(UserIndex.Four); break;
            }
            if (settings.ButtonUp != "")
                KeyUp = ParseKey(settings.ButtonUp);
            if (settings.ButtonDown != "")
                KeyDown = ParseKey(settings.ButtonDown);
            if (settings.ButtonLeft != "")
                KeyLeft = ParseKey(settings.ButtonLeft);
            if (settings.ButtonRight != "")
                KeyRight = ParseKey(settings.ButtonRight);
            if (settings.ButtonStart != "")
                KeyStart = ParseKey(settings.ButtonStart);
            if (settings.ButtonSelect != "")
                KeySelect = ParseKey(settings.ButtonSelect);
            if (settings.ButtonA != "")
                KeyA = ParseKey(settings.ButtonA);
            if (settings.ButtonB != "")
                KeyB = ParseKey(settings.ButtonB);
            if (settings.ButtonTurboA != "")
                KeyTurboA = ParseKey(settings.ButtonTurboA);
            if (settings.ButtonTurboB != "")
                KeyTurboB = ParseKey(settings.ButtonTurboB);
        }
        private Controller x_controller;
        private GamepadButtonFlags x_current_buttons = GamepadButtonFlags.None;
        private GamepadButtonFlags KeyUp = 0;
        private GamepadButtonFlags KeyDown = 0;
        private GamepadButtonFlags KeyLeft = 0;
        private GamepadButtonFlags KeyRight = 0;
        private GamepadButtonFlags KeyStart = 0;
        private GamepadButtonFlags KeySelect = 0;
        private GamepadButtonFlags KeyA = 0;
        private GamepadButtonFlags KeyB = 0;
        private GamepadButtonFlags KeyTurboA = 0;
        private GamepadButtonFlags KeyTurboB = 0;
        private bool turbo;
        // TODO: turbo doesn't work
        public override void Update()
        {
            turbo = !turbo;

            DATA = 0;
            x_current_buttons = x_controller.GetState().Gamepad.Buttons;

            if (IsPressed(KeyA))
                DATA |= 1;

            if (IsPressed(KeyB))
                DATA |= 2;

            if (IsPressed(KeyTurboA) && turbo)
                DATA |= 1;

            if (IsPressed(KeyTurboB) && turbo)
                DATA |= 2;

            if (IsPressed(KeySelect))
                DATA |= 4;

            if (IsPressed(KeyStart))
                DATA |= 8;

            if (IsPressed(KeyUp))
                DATA |= 0x10;

            if (IsPressed(KeyDown))
                DATA |= 0x20;

            if (IsPressed(KeyLeft))
                DATA |= 0x40;

            if (IsPressed(KeyRight))
                DATA |= 0x80;
            // Can use left thump too !
            CheckDirections();
        }
        private bool IsPressed(GamepadButtonFlags key)
        {
            return (x_current_buttons & key) == key;
        }
        void CheckDirections()
        {
            if (x_controller.GetState().Gamepad.LeftThumbY <= -8000)
                DATA |= 0x20;// Up

            if (x_controller.GetState().Gamepad.LeftThumbY >= 8000)
                DATA |= 0x10;// Down

            if (x_controller.GetState().Gamepad.LeftThumbX <= -8000)
                DATA |= 0x40;// Left

            if (x_controller.GetState().Gamepad.LeftThumbX >= 8000)
                DATA |= 0x80;// Right
        }
        private GamepadButtonFlags ParseKey(string key)
        {
            if (key == GamepadButtonFlags.A.ToString())
                return GamepadButtonFlags.A;
            if (key == GamepadButtonFlags.B.ToString())
                return GamepadButtonFlags.B;
            if (key == GamepadButtonFlags.Back.ToString())
                return GamepadButtonFlags.Back;
            if (key == GamepadButtonFlags.DPadDown.ToString())
                return GamepadButtonFlags.DPadDown;
            if (key == GamepadButtonFlags.DPadLeft.ToString())
                return GamepadButtonFlags.DPadLeft;
            if (key == GamepadButtonFlags.DPadRight.ToString())
                return GamepadButtonFlags.DPadRight;
            if (key == GamepadButtonFlags.DPadUp.ToString())
                return GamepadButtonFlags.DPadUp;
            if (key == GamepadButtonFlags.LeftShoulder.ToString())
                return GamepadButtonFlags.LeftShoulder;
            if (key == GamepadButtonFlags.LeftThumb.ToString())
                return GamepadButtonFlags.LeftThumb;
            if (key == GamepadButtonFlags.RightShoulder.ToString())
                return GamepadButtonFlags.RightShoulder;
            if (key == GamepadButtonFlags.RightThumb.ToString())
                return GamepadButtonFlags.RightThumb;
            if (key == GamepadButtonFlags.Start.ToString())
                return GamepadButtonFlags.Start;
            if (key == GamepadButtonFlags.X.ToString())
                return GamepadButtonFlags.X;
            if (key == GamepadButtonFlags.Y.ToString())
                return GamepadButtonFlags.Y;
            return 0;
        }
    }
}
