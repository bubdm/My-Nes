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
    class NesVSUnisystemDIPXControllerConnection : IVSUnisystemDIPConnecter
    {
        public NesVSUnisystemDIPXControllerConnection(string guid, IInputSettingsVSUnisystemDIP settings)
        {
            switch (guid)
            {
                case "x-controller-1": x_controller = new Controller(UserIndex.One); break;
                case "x-controller-2": x_controller = new Controller(UserIndex.Two); break;
                case "x-controller-3": x_controller = new Controller(UserIndex.Three); break;
                case "x-controller-4": x_controller = new Controller(UserIndex.Four); break;
            }
            if (settings.CreditServiceButton != "")
                CreditServiceButton = ParseKey(settings.CreditServiceButton);
            if (settings.DIPSwitch1 != "")
                DIPSwitch1 = ParseKey(settings.DIPSwitch1);
            if (settings.DIPSwitch2 != "")
                DIPSwitch2 = ParseKey(settings.DIPSwitch2);
            if (settings.DIPSwitch3 != "")
                DIPSwitch3 = ParseKey(settings.DIPSwitch3);
            if (settings.DIPSwitch4 != "")
                DIPSwitch4 = ParseKey(settings.DIPSwitch4);
            if (settings.DIPSwitch5 != "")
                DIPSwitch5 = ParseKey(settings.DIPSwitch5);
            if (settings.DIPSwitch6 != "")
                DIPSwitch6 = ParseKey(settings.DIPSwitch6);
            if (settings.DIPSwitch7 != "")
                DIPSwitch7 = ParseKey(settings.DIPSwitch7);
            if (settings.DIPSwitch8 != "")
                DIPSwitch8 = ParseKey(settings.DIPSwitch8);
            if (settings.CreditLeftCoinSlot != "")
                CreditLeftCoinSlot = ParseKey(settings.CreditLeftCoinSlot);
            if (settings.CreditRightCoinSlot != "")
                CreditRightCoinSlot = ParseKey(settings.CreditRightCoinSlot);
        }
        private Controller x_controller;
        private int CreditServiceButton = 0;
        private int DIPSwitch1 = 0;
        private int DIPSwitch2 = 0;
        private int DIPSwitch3 = 0;
        private int DIPSwitch4 = 0;
        private int DIPSwitch5 = 0;
        private int DIPSwitch6 = 0;
        private int DIPSwitch7 = 0;
        private int DIPSwitch8 = 0;
        private int CreditLeftCoinSlot = 0;
        private bool leftCoin = false;
        private int CreditRightCoinSlot = 0;
        private bool rightCoin = false;
        private byte data4016;
        private byte data4017;
        public override void OnEmuShutdown()
        {
            leftCoin = false;
            rightCoin = false;
        }
        public override void Update()
        {
            if (x_controller.GetState().Gamepad.Buttons != GamepadButtonFlags.None)
            {
                data4016 = 0;

                if (IsPressed(CreditServiceButton))
                    data4016 |= 0x04;
                if (IsPressed(DIPSwitch1))
                    data4016 |= 0x08;
                if (IsPressed(DIPSwitch2))
                    data4016 |= 0x10;
                if (IsPressed(CreditLeftCoinSlot))
                    leftCoin = true;
                if (leftCoin)
                    data4016 |= 0x20;
                if (IsPressed(CreditRightCoinSlot))
                    rightCoin = true;
                if (rightCoin)
                    data4016 |= 0x40;

                data4017 = 0;
                if (IsPressed(DIPSwitch3))
                    data4017 |= 0x04;
                if (IsPressed(DIPSwitch4))
                    data4017 |= 0x08;
                if (IsPressed(DIPSwitch5))
                    data4017 |= 0x10;
                if (IsPressed(DIPSwitch6))
                    data4017 |= 0x20;
                if (IsPressed(DIPSwitch7))
                    data4017 |= 0x40;
                if (IsPressed(DIPSwitch8))
                    data4017 |= 0x80;
            }
        }
        private bool IsPressed(int key)
        {
            GamepadButtonFlags k = (GamepadButtonFlags)key;
            return (x_controller.GetState().Gamepad.Buttons & k) == k;
        }
        private int ParseKey(string key)
        {
            if (key.Contains(GamepadButtonFlags.A.ToString()))
                return (int)GamepadButtonFlags.A;
            if (key.Contains(GamepadButtonFlags.B.ToString()))
                return (int)GamepadButtonFlags.B;
            if (key.Contains(GamepadButtonFlags.Back.ToString()))
                return (int)GamepadButtonFlags.Back;
            if (key.Contains(GamepadButtonFlags.DPadDown.ToString()))
                return (int)GamepadButtonFlags.DPadDown;
            if (key.Contains(GamepadButtonFlags.DPadLeft.ToString()))
                return (int)GamepadButtonFlags.DPadLeft;
            if (key.Contains(GamepadButtonFlags.DPadRight.ToString()))
                return (int)GamepadButtonFlags.DPadRight;
            if (key.Contains(GamepadButtonFlags.DPadUp.ToString()))
                return (int)GamepadButtonFlags.DPadUp;
            if (key.Contains(GamepadButtonFlags.LeftShoulder.ToString()))
                return (int)GamepadButtonFlags.LeftShoulder;
            if (key.Contains(GamepadButtonFlags.LeftThumb.ToString()))
                return (int)GamepadButtonFlags.LeftThumb;
            if (key.Contains(GamepadButtonFlags.RightShoulder.ToString()))
                return (int)GamepadButtonFlags.RightShoulder;
            if (key.Contains(GamepadButtonFlags.RightThumb.ToString()))
                return (int)GamepadButtonFlags.RightThumb;
            if (key.Contains(GamepadButtonFlags.Start.ToString()))
                return (int)GamepadButtonFlags.Start;
            if (key.Contains(GamepadButtonFlags.X.ToString()))
                return (int)GamepadButtonFlags.X;
            if (key.Contains(GamepadButtonFlags.Y.ToString()))
                return (int)GamepadButtonFlags.Y;
            return 0;
        }
        public override byte GetData4016()
        {
            return data4016;
        }
        public override byte GetData4017()
        {
            return data4017;
        }
        public override void Write4020(ref byte data)
        {
            if ((data & 0x1) == 0x1)
            {
                leftCoin = false;
                rightCoin = false;
            }
        }
    }
}
