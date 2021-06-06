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
using System.Windows.Forms;
using SlimDX.DirectInput;
using SlimDX.XInput;
namespace MyNes
{
    public partial class FormKey : Form
    {
        public FormKey(SlimDX.DirectInput.DeviceType type, string deviceGuid, string keyName)
        {
            this.deviceType = type;
            InitializeComponent();

            DirectInput di = new DirectInput();

            switch (type)
            {
                case SlimDX.DirectInput.DeviceType.Keyboard:
                    {
                        keyboard = new Keyboard(di);
                        keyboard.SetCooperativeLevel(this.Handle, CooperativeLevel.Nonexclusive | CooperativeLevel.Foreground);
                        break;
                    }
                case SlimDX.DirectInput.DeviceType.Joystick:
                    {
                        joystick = new Joystick(di, Guid.Parse(deviceGuid));
                        joystick.SetCooperativeLevel(this.Handle, CooperativeLevel.Nonexclusive | CooperativeLevel.Foreground);

                        break;
                    }
                case SlimDX.DirectInput.DeviceType.Other:
                    {
                        switch (deviceGuid)
                        {
                            case "x-controller-1": x_controller = new Controller(UserIndex.One); break;
                            case "x-controller-2": x_controller = new Controller(UserIndex.Two); break;
                            case "x-controller-3": x_controller = new Controller(UserIndex.Three); break;
                            case "x-controller-4": x_controller = new Controller(UserIndex.Four); break;
                        }
                        break;
                    }
            }

            timer_hold.Start();
            label1.Text = string.Format(Properties.Resources.Status12 + " [{0}]", keyName);
            stopTimer = 10;
            label_cancel.Text = string.Format(Properties.Resources.Status13 + " {0} " + Properties.Resources.Seconds, stopTimer);
            timer2.Start();
            this.Select();
        }
        private SlimDX.DirectInput.DeviceType deviceType;
        private Keyboard keyboard;
        private KeyboardState keyboardState;
        private Joystick joystick;
        private JoystickState joystickState;
        private Controller x_controller;
        private string _inputName;
        private int stopTimer = 0;
        public string InputName { get { return _inputName; } }
        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (deviceType)
            {
                case SlimDX.DirectInput.DeviceType.Keyboard:
                    {
                        if (keyboard.Acquire().IsSuccess)
                        {
                            keyboardState = keyboard.GetCurrentState();
                            if (keyboardState.PressedKeys.Count > 0)
                            {
                                _inputName = keyboardState.PressedKeys[0].ToString();

                                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                                timer1.Enabled = false;
                                this.Close();
                                return;
                            }
                        }
                        break;
                    }
                case SlimDX.DirectInput.DeviceType.Joystick:
                    {
                        if (CheckJoystickInput())
                        {
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            timer1.Enabled = false;
                            this.Close();
                            return;
                        }
                        break;
                    }
                case SlimDX.DirectInput.DeviceType.Other:
                    {
                        if (x_controller.GetState().Gamepad.Buttons != GamepadButtonFlags.None)
                        {
                            _inputName = x_controller.GetState().Gamepad.Buttons.ToString();

                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            timer1.Enabled = false;
                            this.Close();
                            return;
                        }
                        break;
                    }
            }
        }
        private bool CheckJoystickInput()
        {
            if (joystick.Acquire().IsSuccess)
            {
                joystickState = joystick.GetCurrentState();
                if (joystickState == null)
                    return false;

                bool[] buttons = joystickState.GetButtons();
                for (int button = 0; button < buttons.Length; button++)
                {
                    if (buttons[button])
                    {
                        _inputName = button.ToString();
                        return true;
                    }

                    if (joystickState.X > 0xC000)
                    {
                        _inputName = "X+";
                        return true;
                    }
                    else if (joystickState.X < 0x4000)
                    {
                        _inputName = "X-";
                        return true;
                    }
                    else if (joystickState.Y > 0xC000)
                    {
                        _inputName = "Y+";
                        return true;
                    }
                    else if (joystickState.Y < 0x4000)
                    {
                        _inputName = "Y-";
                        return true;
                    }
                }
            }
            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (stopTimer > 0)
            {
                stopTimer--;
                label_cancel.Text = string.Format(Properties.Resources.Status13 + " {0} " + Properties.Resources.Seconds, stopTimer);
            }
            else
            {
                timer2.Stop();

                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                timer1.Enabled = false;
                this.Close();
                return;
            }
        }
        private void timer_hold_Tick(object sender, EventArgs e)
        {
            timer_hold.Stop();
            timer1.Interval = 1000 / 30;
            timer1.Start();
        }
    }
}
