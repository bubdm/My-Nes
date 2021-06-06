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
using MyNes.Core;
using SlimDX.DirectInput;
using SlimDX.XInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MyNes
{
    public partial class FormMain : Form
    {
        public FormMain(string[] args)
        {
            InitializeComponent();

            isMouseVisible = true;
        }

        private bool isMaxing;
        private bool isMoving;
        private bool pausedByMainWindow;
        private bool isMouseVisible;
        private int mouseHiderCounter;
        const int mouseHiderReload = 1;
        private bool gameLoaded = false;

        internal void LoadSettings()
        {
            Location = new Point(Program.Settings.Win_Location_X, Program.Settings.Win_Location_Y);
            Size = new Size(Program.Settings.Win_Size_W, Program.Settings.Win_Size_H);
        }
        internal void LoadRenderers()
        {
            // Reload renderers
            rendererToolStripMenuItem.DropDownItems.Clear();
            rendererToolStripMenuItem_audio.DropDownItems.Clear();

            foreach (IVideoProvider pro in MyNesMain.VideoProviders)
            {
                ToolStripMenuItem it = new ToolStripMenuItem();
                it.Text = pro.Name;
                it.Tag = pro.ID;
                rendererToolStripMenuItem.DropDownItems.Add(it);
            }
            foreach (IAudioProvider pro in MyNesMain.AudioProviders)
            {
                ToolStripMenuItem it = new ToolStripMenuItem();
                it.Text = pro.Name;
                it.Tag = pro.ID;
                rendererToolStripMenuItem_audio.DropDownItems.Add(it);
            }

            MyNesMain.VideoProvider.ApplyFilter();

            // Reload resolutions
            resolutionToolStripMenuItem.DropDownItems.Clear();
            double x = 1;
            double y = 1;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    //lines.Add(string.Format("{0} x {1}", 256 * x, 240 * y));
                    double w = 256 * x;
                    double h = 240 * y;
                    // Let's calculate aspect ratio
                    // The idea is to find 2 numbers, the result of the division of these numbers
                    // equal the result of division of width / height. Result will be set in format
                    // x:y while x > y and x/y = width/height
                    // x and y smallest whole numbers possible to make it correct.
                    string as_ratio = "";
                    double ratio = w / h;
                    bool found = false;
                    for (double t = 1; t < 1000; t++)
                    {
                        for (double z = 1; z < 1000; z++)
                        {
                            if (ratio == z / t)
                            {
                                as_ratio = string.Format("{0}:{1}", z, t);
                                found = true;
                                break;
                            }
                        }
                        if (found)
                            break;
                    }

                    ToolStripMenuItem it = new ToolStripMenuItem();
                    it.Text = string.Format("{0} x {1}", w, h);
                    switch (as_ratio)
                    {
                        case "16:15": as_ratio += " NES OUTPUT"; break;
                        case "4:3": as_ratio += " Latterbox SDTV/NTSC TV"; break;
                        case "16:9": as_ratio += " Widescreen SDTV/HDTV"; break;
                    }
                    if (w == 256 && h == 240)
                        as_ratio += " (Maximum Performance)";
                    else if (w == 640 && h == 480)
                        as_ratio += " (Default, Recommended)";
                    else if (w == 1920 && h == 1080)
                        as_ratio += "/Full HD (Highest Quality)";

                    it.ShortcutKeyDisplayString = as_ratio;
                    resolutionToolStripMenuItem.DropDownItems.Add(it);
                    x += 0.5;
                }

                y += 0.5;
                x = y;
            }
        }

        internal void SaveSettings()
        {
            Program.Settings.Win_Location_X = Location.X;
            Program.Settings.Win_Location_Y = Location.Y;
            Program.Settings.Win_Size_W = Size.Width;
            Program.Settings.Win_Size_H = Size.Height;
        }
        internal void SetupInputs()
        {
            // prepare things
            IJoypadConnecter joy1 = null;
            IJoypadConnecter joy2 = null;
            IJoypadConnecter joy3 = null;
            IJoypadConnecter joy4 = null;
            // Refresh input devices !
            DirectInput di = new DirectInput();
            List<DeviceInstance> devices = new List<DeviceInstance>(di.GetDevices());
            bool found = false;
            #region Player 1
            switch (Program.ControlSettings.Joypad1DeviceGuid)
            {
                default:
                    {
                        foreach (DeviceInstance dev in devices)
                        {
                            if (dev.InstanceGuid.ToString().ToLower() == Program.ControlSettings.Joypad1DeviceGuid)
                            {
                                // We found the device !!
                                // Let's see if we have the settings for this device
                                foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad1Devices)
                                {
                                    if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                                    {
                                        // This is it !
                                        switch (dev.Type)
                                        {
                                            case SlimDX.DirectInput.DeviceType.Keyboard:
                                                {
                                                    joy1 = new NesJoypadPcKeyboardConnection(this.Handle, con);
                                                    found = true;
                                                    break;
                                                }
                                            case SlimDX.DirectInput.DeviceType.Joystick:
                                                {
                                                    joy1 = new NesJoypadPcJoystickConnection(this.Handle, dev.InstanceGuid.ToString(), con);
                                                    found = true;
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                case "x-controller-1":
                    {
                        SlimDX.XInput.Controller c1 = new Controller(UserIndex.One);
                        if (c1.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad1Devices)
                            {
                                if (con.DeviceGuid == "x-controller-1")
                                {
                                    joy1 = new NesJoypadXControllerConnection("x-controller-1", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-2":
                    {
                        SlimDX.XInput.Controller c2 = new Controller(UserIndex.Two);
                        if (c2.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad1Devices)
                            {
                                if (con.DeviceGuid == "x-controller-2")
                                {
                                    joy1 = new NesJoypadXControllerConnection("x-controller-2", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-3":
                    {
                        SlimDX.XInput.Controller c3 = new Controller(UserIndex.Three);
                        if (c3.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad1Devices)
                            {
                                if (con.DeviceGuid == "x-controller-3")
                                {
                                    joy1 = new NesJoypadXControllerConnection("x-controller-3", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-4":
                    {
                        SlimDX.XInput.Controller c4 = new Controller(UserIndex.Four);
                        if (c4.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad1Devices)
                            {
                                if (con.DeviceGuid == "x-controller-4")
                                {
                                    joy1 = new NesJoypadXControllerConnection("x-controller-4", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }
            if (!found && Program.ControlSettings.Joypad1AutoSwitchBackToKeyboard)
            {
                foreach (DeviceInstance dev in devices)
                {
                    if (dev.Type == SlimDX.DirectInput.DeviceType.Keyboard)
                    {
                        // We found the device !!
                        // Let's see if we have the settings for this device
                        foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad1Devices)
                        {
                            if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                            {
                                // This is it !
                                joy1 = new NesJoypadPcKeyboardConnection(this.Handle, con);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            #endregion
            #region Player 2
            found = false;
            switch (Program.ControlSettings.Joypad2DeviceGuid)
            {
                default:
                    {
                        foreach (DeviceInstance dev in devices)
                        {
                            if (dev.InstanceGuid.ToString().ToLower() == Program.ControlSettings.Joypad2DeviceGuid)
                            {
                                // We found the device !!
                                // Let's see if we have the settings for this device
                                foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad2Devices)
                                {
                                    if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                                    {
                                        // This is it !
                                        switch (dev.Type)
                                        {
                                            case SlimDX.DirectInput.DeviceType.Keyboard:
                                                {
                                                    joy2 = new NesJoypadPcKeyboardConnection(this.Handle, con);
                                                    found = true;
                                                    break;
                                                }
                                            case SlimDX.DirectInput.DeviceType.Joystick:
                                                {
                                                    joy2 = new NesJoypadPcJoystickConnection(this.Handle, dev.InstanceGuid.ToString(), con);
                                                    found = true;
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                case "x-controller-1":
                    {
                        SlimDX.XInput.Controller c1 = new Controller(UserIndex.One);
                        if (c1.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad2Devices)
                            {
                                if (con.DeviceGuid == "x-controller-1")
                                {
                                    joy2 = new NesJoypadXControllerConnection("x-controller-1", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-2":
                    {
                        SlimDX.XInput.Controller c2 = new Controller(UserIndex.Two);
                        if (c2.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad2Devices)
                            {
                                if (con.DeviceGuid == "x-controller-2")
                                {
                                    joy2 = new NesJoypadXControllerConnection("x-controller-2", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-3":
                    {
                        SlimDX.XInput.Controller c3 = new Controller(UserIndex.Three);
                        if (c3.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad2Devices)
                            {
                                if (con.DeviceGuid == "x-controller-3")
                                {
                                    joy2 = new NesJoypadXControllerConnection("x-controller-3", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-4":
                    {
                        SlimDX.XInput.Controller c4 = new Controller(UserIndex.Four);
                        if (c4.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad2Devices)
                            {
                                if (con.DeviceGuid == "x-controller-4")
                                {
                                    joy2 = new NesJoypadXControllerConnection("x-controller-4", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }
            if (!found && Program.ControlSettings.Joypad2AutoSwitchBackToKeyboard)
            {
                foreach (DeviceInstance dev in devices)
                {
                    if (dev.Type == SlimDX.DirectInput.DeviceType.Keyboard)
                    {
                        // We found the device !!
                        // Let's see if we have the settings for this device
                        foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad2Devices)
                        {
                            if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                            {
                                // This is it !
                                joy2 = new NesJoypadPcKeyboardConnection(this.Handle, con);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            #endregion
            #region Player 3
            found = false;
            switch (Program.ControlSettings.Joypad3DeviceGuid)
            {
                default:
                    {
                        foreach (DeviceInstance dev in devices)
                        {
                            if (dev.InstanceGuid.ToString().ToLower() == Program.ControlSettings.Joypad3DeviceGuid)
                            {
                                // We found the device !!
                                // Let's see if we have the settings for this device
                                foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad3Devices)
                                {
                                    if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                                    {
                                        // This is it !
                                        switch (dev.Type)
                                        {
                                            case SlimDX.DirectInput.DeviceType.Keyboard:
                                                {
                                                    joy3 = new NesJoypadPcKeyboardConnection(this.Handle, con);
                                                    found = true;
                                                    break;
                                                }
                                            case SlimDX.DirectInput.DeviceType.Joystick:
                                                {
                                                    joy3 = new NesJoypadPcJoystickConnection(this.Handle, dev.InstanceGuid.ToString(), con);
                                                    found = true;
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                case "x-controller-1":
                    {
                        SlimDX.XInput.Controller c1 = new Controller(UserIndex.One);
                        if (c1.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad3Devices)
                            {
                                if (con.DeviceGuid == "x-controller-1")
                                {
                                    joy3 = new NesJoypadXControllerConnection("x-controller-1", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }

                case "x-controller-2":
                    {
                        SlimDX.XInput.Controller c2 = new Controller(UserIndex.Two);
                        if (c2.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad3Devices)
                            {
                                if (con.DeviceGuid == "x-controller-2")
                                {
                                    joy3 = new NesJoypadXControllerConnection("x-controller-2", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-3":
                    {
                        SlimDX.XInput.Controller c3 = new Controller(UserIndex.Three);
                        if (c3.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad3Devices)
                            {
                                if (con.DeviceGuid == "x-controller-3")
                                {
                                    joy3 = new NesJoypadXControllerConnection("x-controller-3", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-4":
                    {
                        SlimDX.XInput.Controller c4 = new Controller(UserIndex.Four);
                        if (c4.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad3Devices)
                            {
                                if (con.DeviceGuid == "x-controller-4")
                                {
                                    joy3 = new NesJoypadXControllerConnection("x-controller-4", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }
            if (!found && Program.ControlSettings.Joypad3AutoSwitchBackToKeyboard)
            {
                foreach (DeviceInstance dev in devices)
                {
                    if (dev.Type == SlimDX.DirectInput.DeviceType.Keyboard)
                    {
                        // We found the device !!
                        // Let's see if we have the settings for this device
                        foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad3Devices)
                        {
                            if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                            {
                                // This is it !
                                joy3 = new NesJoypadPcKeyboardConnection(this.Handle, con);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            #endregion
            #region Player 4
            found = false;
            switch (Program.ControlSettings.Joypad4DeviceGuid)
            {
                default:
                    {
                        foreach (DeviceInstance dev in devices)
                        {
                            if (dev.InstanceGuid.ToString().ToLower() == Program.ControlSettings.Joypad4DeviceGuid)
                            {
                                // We found the device !!
                                // Let's see if we have the settings for this device
                                foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad4Devices)
                                {
                                    if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                                    {
                                        // This is it !
                                        switch (dev.Type)
                                        {
                                            case SlimDX.DirectInput.DeviceType.Keyboard:
                                                {
                                                    joy4 = new NesJoypadPcKeyboardConnection(this.Handle, con);
                                                    found = true;
                                                    break;
                                                }
                                            case SlimDX.DirectInput.DeviceType.Joystick:
                                                {
                                                    joy4 = new NesJoypadPcJoystickConnection(this.Handle, dev.InstanceGuid.ToString(), con);
                                                    found = true;
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                case "x-controller-1":
                    {
                        SlimDX.XInput.Controller c1 = new Controller(UserIndex.One);
                        if (c1.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad4Devices)
                            {
                                if (con.DeviceGuid == "x-controller-1")
                                {
                                    joy4 = new NesJoypadXControllerConnection("x-controller-1", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }

                case "x-controller-2":
                    {
                        SlimDX.XInput.Controller c2 = new Controller(UserIndex.Two);
                        if (c2.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad4Devices)
                            {
                                if (con.DeviceGuid == "x-controller-2")
                                {
                                    joy4 = new NesJoypadXControllerConnection("x-controller-2", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-3":
                    {
                        SlimDX.XInput.Controller c3 = new Controller(UserIndex.Three);
                        if (c3.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad4Devices)
                            {
                                if (con.DeviceGuid == "x-controller-3")
                                {
                                    joy4 = new NesJoypadXControllerConnection("x-controller-3", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-4":
                    {
                        SlimDX.XInput.Controller c4 = new Controller(UserIndex.Four);
                        if (c4.IsConnected)
                        {
                            foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad4Devices)
                            {
                                if (con.DeviceGuid == "x-controller-4")
                                {
                                    joy4 = new NesJoypadXControllerConnection("x-controller-4", con);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }
            if (!found && Program.ControlSettings.Joypad3AutoSwitchBackToKeyboard)
            {
                foreach (DeviceInstance dev in devices)
                {
                    if (dev.Type == SlimDX.DirectInput.DeviceType.Keyboard)
                    {
                        // We found the device !!
                        // Let's see if we have the settings for this device
                        foreach (IInputSettingsJoypad con in Program.ControlSettings.Joypad4Devices)
                        {
                            if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                            {
                                // This is it !
                                joy4 = new NesJoypadPcKeyboardConnection(this.Handle, con);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            #endregion
            NesEmu.SetupControllers(joy1, joy2, joy3, joy4);
            #region VSUnisystem DIP
            found = false;
            switch (Program.ControlSettings.VSUnisystemDIPDeviceGuid)
            {
                default:
                    {
                        foreach (DeviceInstance dev in devices)
                        {
                            if (dev.InstanceGuid.ToString().ToLower() == Program.ControlSettings.VSUnisystemDIPDeviceGuid)
                            {
                                // We found the device !!
                                // Let's see if we have the settings for this device
                                foreach (IInputSettingsVSUnisystemDIP con in Program.ControlSettings.VSUnisystemDIPDevices)
                                {
                                    if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                                    {
                                        // This is it !
                                        switch (dev.Type)
                                        {
                                            case SlimDX.DirectInput.DeviceType.Keyboard:
                                                {
                                                    NesEmu.SetupVSUnisystemDIP(new NesVSUnisystemDIPKeyboardConnection(this.Handle, con));
                                                    found = true; break;
                                                }
                                            case SlimDX.DirectInput.DeviceType.Joystick:
                                                {
                                                    NesEmu.SetupVSUnisystemDIP(new NesVSUnisystemDIPJoystickConnection(this.Handle, dev.InstanceGuid.ToString(), con));
                                                    found = true; break;
                                                }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                case "x-controller-1":
                    {
                        SlimDX.XInput.Controller c1 = new Controller(UserIndex.One);
                        if (c1.IsConnected)
                        {
                            foreach (IInputSettingsVSUnisystemDIP con in Program.ControlSettings.VSUnisystemDIPDevices)
                            {
                                if (con.DeviceGuid == "x-controller-1")
                                {
                                    NesEmu.SetupVSUnisystemDIP(new NesVSUnisystemDIPXControllerConnection("x-controller-1", con));
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }

                case "x-controller-2":
                    {
                        SlimDX.XInput.Controller c2 = new Controller(UserIndex.Two);
                        if (c2.IsConnected)
                        {
                            foreach (IInputSettingsVSUnisystemDIP con in Program.ControlSettings.VSUnisystemDIPDevices)
                            {
                                if (con.DeviceGuid == "x-controller-2")
                                {
                                    NesEmu.SetupVSUnisystemDIP(new NesVSUnisystemDIPXControllerConnection("x-controller-2", con));
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-3":
                    {
                        SlimDX.XInput.Controller c3 = new Controller(UserIndex.Three);
                        if (c3.IsConnected)
                        {
                            foreach (IInputSettingsVSUnisystemDIP con in Program.ControlSettings.VSUnisystemDIPDevices)
                            {
                                if (con.DeviceGuid == "x-controller-3")
                                {
                                    NesEmu.SetupVSUnisystemDIP(new NesVSUnisystemDIPXControllerConnection("x-controller-3", con));
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-4":
                    {
                        SlimDX.XInput.Controller c4 = new Controller(UserIndex.Four);
                        if (c4.IsConnected)
                        {
                            foreach (IInputSettingsVSUnisystemDIP con in Program.ControlSettings.VSUnisystemDIPDevices)
                            {
                                if (con.DeviceGuid == "x-controller-4")
                                {
                                    NesEmu.SetupVSUnisystemDIP(new NesVSUnisystemDIPXControllerConnection("x-controller-4", con));
                                    found = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }
            if (!found && Program.ControlSettings.VSUnisystemDIPAutoSwitchBackToKeyboard)
            {
                foreach (DeviceInstance dev in devices)
                {
                    if (dev.Type == SlimDX.DirectInput.DeviceType.Keyboard)
                    {
                        // We found the device !!
                        // Let's see if we have the settings for this device
                        foreach (IInputSettingsVSUnisystemDIP con in Program.ControlSettings.VSUnisystemDIPDevices)
                        {
                            if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                            {
                                // This is it !
                                NesEmu.SetupVSUnisystemDIP(new NesVSUnisystemDIPKeyboardConnection(this.Handle, con));
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            #endregion
            #region Shortcuts
            found = false;
            switch (Program.ControlSettings.ShortcutsDeviceGuid)
            {
                default:
                    {
                        foreach (DeviceInstance dev in devices)
                        {
                            if (dev.InstanceGuid.ToString().ToLower() == Program.ControlSettings.ShortcutsDeviceGuid)
                            {
                                // We found the device !!
                                // Let's see if we have the settings for this device
                                foreach (InputSettingsShortcuts con in Program.ControlSettings.ShortcutsDevices)
                                {
                                    if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                                    {
                                        // This is it !
                                        switch (dev.Type)
                                        {
                                            case SlimDX.DirectInput.DeviceType.Keyboard:
                                                {
                                                    NesEmu.SetupShortcutsHandler(new ShortcutsKeyboardConnecter(this.Handle, con));
                                                    found = true;
                                                    SetupShortcutsTexts(con);
                                                    break;
                                                }
                                            case SlimDX.DirectInput.DeviceType.Joystick:
                                                {
                                                    NesEmu.SetupShortcutsHandler(new ShortcutsJoystickConnecter(this.Handle, dev.InstanceGuid.ToString(), con));
                                                    found = true;
                                                    SetupShortcutsTexts(con);
                                                    break;
                                                }
                                        }

                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                case "x-controller-1":
                    {
                        SlimDX.XInput.Controller c1 = new Controller(UserIndex.One);
                        if (c1.IsConnected)
                        {
                            foreach (InputSettingsShortcuts con in Program.ControlSettings.ShortcutsDevices)
                            {
                                if (con.DeviceGuid == "x-controller-1")
                                {
                                    NesEmu.SetupShortcutsHandler(new ShortcutsXControllerConnecter("x-controller-1", con));
                                    found = true;
                                    SetupShortcutsTexts(con);
                                    break;
                                }
                            }
                        }
                        break;
                    }

                case "x-controller-2":
                    {
                        SlimDX.XInput.Controller c2 = new Controller(UserIndex.Two);
                        if (c2.IsConnected)
                        {
                            foreach (InputSettingsShortcuts con in Program.ControlSettings.ShortcutsDevices)
                            {
                                if (con.DeviceGuid == "x-controller-2")
                                {
                                    NesEmu.SetupShortcutsHandler(new ShortcutsXControllerConnecter("x-controller-2", con));
                                    found = true;
                                    SetupShortcutsTexts(con);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-3":
                    {
                        SlimDX.XInput.Controller c3 = new Controller(UserIndex.Three);
                        if (c3.IsConnected)
                        {
                            foreach (InputSettingsShortcuts con in Program.ControlSettings.ShortcutsDevices)
                            {
                                if (con.DeviceGuid == "x-controller-3")
                                {
                                    NesEmu.SetupShortcutsHandler(new ShortcutsXControllerConnecter("x-controller-3", con));
                                    found = true;
                                    SetupShortcutsTexts(con);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case "x-controller-4":
                    {
                        SlimDX.XInput.Controller c4 = new Controller(UserIndex.Four);
                        if (c4.IsConnected)
                        {
                            foreach (InputSettingsShortcuts con in Program.ControlSettings.ShortcutsDevices)
                            {
                                if (con.DeviceGuid == "x-controller-4")
                                {
                                    NesEmu.SetupShortcutsHandler(new ShortcutsXControllerConnecter("x-controller-4", con));
                                    found = true;
                                    SetupShortcutsTexts(con);
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }
            if (!found && Program.ControlSettings.ShortcutsAutoSwitchBackToKeyboard)
            {
                foreach (DeviceInstance dev in devices)
                {
                    if (dev.Type == SlimDX.DirectInput.DeviceType.Keyboard)
                    {
                        // We found the device !!
                        // Let's see if we have the settings for this device
                        foreach (InputSettingsShortcuts con in Program.ControlSettings.ShortcutsDevices)
                        {
                            if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                            {
                                // This is it !
                                NesEmu.SetupShortcutsHandler(new ShortcutsKeyboardConnecter(this.Handle, con));
                                SetupShortcutsTexts(con);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            #endregion
            // TODO:  ZAPPER
            //NesEmu.SetupZapper(zapper = new ZapperConnecter(this.Handle, this.Location.X, this.Location.Y + menuStrip1.Height,
            //     panel_surface.Width, panel_surface.Height));
            // video.SetupZapperBounds();
        }
        internal void SetupShortcutsTexts(InputSettingsShortcuts con)
        {
            shutdownToolStripMenuItem.ShortcutKeyDisplayString = shutdownEmulationToolStripMenuItem.ShortcutKeyDisplayString = con.KeyShutDown;
            togglePauseToolStripMenuItem.ShortcutKeyDisplayString = con.KeyTogglePause;
            softResetToolStripMenuItem.ShortcutKeyDisplayString = softResetToolStripMenuItem1.ShortcutKeyDisplayString = con.KeySoftReset;
            hardResetToolStripMenuItem.ShortcutKeyDisplayString = hardResetToolStripMenuItem1.ShortcutKeyDisplayString = con.KeyHardReset;
            takeSnapshotToolStripMenuItem.ShortcutKeyDisplayString = con.KeyTakeSnapshot;
            saveStateToolStripMenuItem.ShortcutKeyDisplayString = saveStateToolStripMenuItem1.ShortcutKeyDisplayString = con.KeySaveState;
            saveStateBrowserToolStripMenuItem.ShortcutKeyDisplayString = con.KeySaveStateBrowser;
            saveStateAsToolStripMenuItem.ShortcutKeyDisplayString = con.KeySaveStateAs;
            loadStateToolStripMenuItem.ShortcutKeyDisplayString = loadStateToolStripMenuItem1.ShortcutKeyDisplayString = con.KeyLoadState;
            loadStateBrowserToolStripMenuItem.ShortcutKeyDisplayString = con.KeyLoadStateBrowser;
            loadStateAsToolStripMenuItem.ShortcutKeyDisplayString = con.KeyLoadStateAs;
            turboToolStripMenuItem.ShortcutKeyDisplayString = turboToolStripMenuItem1.ShortcutKeyDisplayString = con.KeyTurbo;
            fullscreenToolStripMenuItem.ShortcutKeyDisplayString = fullscreenToolStripMenuItem1.ShortcutKeyDisplayString = con.KeyToggleFullscreen;
            keepAspectRationToolStripMenuItem.ShortcutKeyDisplayString = keepAspectRatioToolStripMenuItem.ShortcutKeyDisplayString = con.KeyToggleKeepAspectRatio;
            upToolStripMenuItem.ShortcutKeyDisplayString = con.KeyVolumeUp;
            downToolStripMenuItem.ShortcutKeyDisplayString = con.KeyVolumeDown;
            soundEnabledToolStripMenuItem.ShortcutKeyDisplayString = soundEnabledToolStripMenuItem1.ShortcutKeyDisplayString = con.KeyToggleSoundEnable;
            recordSoundToolStripMenuItem.ShortcutKeyDisplayString = con.KeyRecordSound;
            showFPSToolStripMenuItem.ShortcutKeyDisplayString = showFPSToolStripMenuItem1.ShortcutKeyDisplayString = con.KeyToggleFPS;
            connect4PlayersToolStripMenuItem.ShortcutKeyDisplayString = con.KeyConnect4Players;
            enableGameGenieToolStripMenuItem.ShortcutKeyDisplayString = con.KeyConnectGameGenie;
            gameGenieCodesToolStripMenuItem.ShortcutKeyDisplayString = con.KeyGameGenieCodes;
            useMixerToolStripMenuItem.ShortcutKeyDisplayString = con.KeyToggleUseSoundMixer;
            toolStripMenuItem_slot0.ShortcutKeyDisplayString = con.KeySetStateSlot0;
            toolStripMenuItem_slot1.ShortcutKeyDisplayString = con.KeySetStateSlot1;
            toolStripMenuItem_slot2.ShortcutKeyDisplayString = con.KeySetStateSlot2;
            toolStripMenuItem_slot3.ShortcutKeyDisplayString = con.KeySetStateSlot3;
            toolStripMenuItem_slot4.ShortcutKeyDisplayString = con.KeySetStateSlot4;
            toolStripMenuItem_slot5.ShortcutKeyDisplayString = con.KeySetStateSlot5;
            toolStripMenuItem_slot6.ShortcutKeyDisplayString = con.KeySetStateSlot6;
            toolStripMenuItem_slot7.ShortcutKeyDisplayString = con.KeySetStateSlot7;
            toolStripMenuItem_slot8.ShortcutKeyDisplayString = con.KeySetStateSlot8;
            toolStripMenuItem_slot9.ShortcutKeyDisplayString = con.KeySetStateSlot9;
        }
        internal void LoadGame(string filePath)
        {
            bool successed = false;
            switch (Path.GetExtension(filePath).ToLower())
            {
                case ".nes": NesEmu.LoadGame(filePath, out successed); break;
                case ".7z":
                case ".zip":
                case ".rar":
                case ".gzip":
                case ".tar":
                case ".bzip2":
                case ".xz":
                    {
                        string fileName = filePath;
                        string tempFolder = Path.GetTempPath() + "\\MYNES\\";
                        SevenZip.SevenZipExtractor EXTRACTOR;
                        try
                        {
                            EXTRACTOR = new SevenZip.SevenZipExtractor(fileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Properties.Resources.Message35 + ": \n" + ex.Message);
                            return;
                        }
                        if (EXTRACTOR.ArchiveFileData.Count == 1)
                        {
                            if (EXTRACTOR.ArchiveFileData[0].FileName.Substring(EXTRACTOR.ArchiveFileData[0].FileName.Length - 4, 4).ToLower() == ".nes")
                            {
                                EXTRACTOR.ExtractArchive(tempFolder);
                                fileName = tempFolder + EXTRACTOR.ArchiveFileData[0].FileName;
                            }
                        }
                        else
                        {
                            List<string> filenames = new List<string>();
                            foreach (SevenZip.ArchiveFileInfo file in EXTRACTOR.ArchiveFileData)
                            {
                                filenames.Add(file.FileName);
                            }
                            FormFilesList ar = new FormFilesList(filenames.ToArray());
                            if (ar.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            {
                                string[] fil = { ar.SelectedRom };
                                EXTRACTOR.ExtractFiles(tempFolder, fil);
                                fileName = tempFolder + ar.SelectedRom;
                            }
                            else
                            { return; }
                        }
                        NesEmu.LoadGame(fileName, out successed); break;
                    }
            }

            if (successed)
            {
                // Add the file into recent files
                if (Program.Settings.Misc_RecentFiles == null)
                    Program.Settings.Misc_RecentFiles = new string[0];
                List<string> items = new List<string>(Program.Settings.Misc_RecentFiles);

                if (items.Contains(filePath))
                    items.Remove(filePath);
                items.Insert(0, filePath);

                if (items.Count > 19)
                    items.RemoveAt(items.Count - 1);

                Program.Settings.Misc_RecentFiles = items.ToArray();

                gameLoaded = true;
                // Start in fullscreen ?
                if (Program.Settings.Win_StartInFullscreen)
                {
                    // Do start only if it not maximized
                    if (WindowState != FormWindowState.Maximized)
                    {
                        MyNesMain.VideoProvider.ResizeBegin();
                        System.Threading.Thread.Sleep(100);


                        MyNesMain.RendererSettings.Vid_Fullscreen = true;

                        FormBorderStyle = FormBorderStyle.None;
                        menuStrip1.Visible = false;

                        WindowState = FormWindowState.Maximized;


                        MyNesMain.VideoProvider.ResizeEnd();
                    }
                }
                else
                {
                    // Auto stretch
                    if (MyNesMain.RendererSettings.Vid_AutoStretch)
                    {
                        ApplyStretch(true);
                    }
                }
            }
            ApplyWindowTitle();
        }
        internal void ApplyStretch(bool applyRegion)
        {
            MyNesMain.VideoProvider.ResizeBegin();
            int w = 256;
            int h = menuStrip1.Height + 25;

            switch (NesEmu.Region)
            {
                case EmuRegion.NTSC:
                    {
                        if (MyNesMain.RendererSettings.Vid_Res_Upscale)
                        {
                            w = MyNesMain.RendererSettings.Vid_Res_W * MyNesMain.RendererSettings.Vid_StretchMultiply;
                            h += MyNesMain.RendererSettings.Vid_Res_H * MyNesMain.RendererSettings.Vid_StretchMultiply;
                        }
                        else
                        {
                            w = 640 * MyNesMain.RendererSettings.Vid_StretchMultiply;
                            h += 480 * MyNesMain.RendererSettings.Vid_StretchMultiply;
                        }
                        break;
                    }
                case EmuRegion.PALB:
                case EmuRegion.DENDY:
                    {
                        w = 720 * MyNesMain.RendererSettings.Vid_StretchMultiply;
                        h += 576 * MyNesMain.RendererSettings.Vid_StretchMultiply;
                        break;
                    }
            }


            // Apply new size
            if (Size.Width != w || Size.Height != h)
            {
                this.Size = new Size(w, h);
                if (applyRegion)
                    MyNesMain.VideoProvider.ApplyRegionChanges();
                MyNesMain.VideoProvider.ResizeEnd();
            }
            else
            {
                if (applyRegion)
                {
                    MyNesMain.VideoProvider.ApplyRegionChanges();
                    MyNesMain.VideoProvider.ResizeEnd();
                }
                else
                    MyNesMain.VideoProvider.Resume();
            }
        }
        internal void ApplyWindowTitle()
        {
            if (InvokeRequired)
                Invoke(new Action(ApplyWindowTitleInvoked));
            else
                ApplyWindowTitleInvoked();
        }
        private void ApplyWindowTitleInvoked()
        {
            if (!NesEmu.ON)
            {
                Text = "My Nes";
                return;
            }
            if (NesEmu.IsGameFoundOnDB)
            {
                if (NesEmu.GameInfo.Game_AltName != null && NesEmu.GameInfo.Game_AltName != "")
                    this.Text = NesEmu.GameInfo.Game_Name + " (" + NesEmu.GameInfo.Game_AltName + ") - My Nes";
                else
                    this.Text = NesEmu.GameInfo.Game_Name + " - My Nes";
            }
            else
            {
                this.Text = Path.GetFileName(NesEmu.CurrentFilePath) + " - My Nes";
            }
        }
        internal void ShowLauncher()
        {
            if (Program.Settings.LauncherShowAyAppStart)
            {
                launcherToolStripMenuItem_Click(this, new EventArgs());
            }
        }
        internal void ExecuteCommandLines(string[] args)
        {
            // First command must be rom file
            if (args != null)
            {
                if (args.Length > 0)
                {
                    LoadGame(args[0]);
                }
            }
        }
        internal void ApplyAudioFreq()
        {
            if (NesEmu.ON)
                NesEmu.PAUSED = true;

            System.Threading.Thread.Sleep(200);

            if (MyNesMain.AudioProvider != null)
            {
                MyNesMain.AudioProvider.TogglePause(!NesEmu.SoundEnabled);
                MyNesMain.AudioProvider.Reset();
            }
            if (NesEmu.ON)
            {
                NesEmu.ApplyAudioSettings(true);
                NesEmu.PAUSED = false;
            }
        }
        internal void ShowSaveStateBrowser()
        {
            if (InvokeRequired)
                Invoke(new Action(ShowSaveStateBrowserInvoked));
            else
                ShowSaveStateBrowserInvoked();
        }
        internal void ShowSaveStateBrowserInvoked()
        {
            if (NesEmu.ON)
            {
                NesEmu.PAUSED = true;
                FormStatesBrowser frm = new FormStatesBrowser(true);
                frm.ShowDialog(this);
                NesEmu.PAUSED = false;
            }
        }
        internal void ShowLoadStateBrowser()
        {
            if (InvokeRequired)
                Invoke(new Action(ShowLoadStateBrowserInvoked));
            else
                ShowLoadStateBrowserInvoked();
        }
        internal void ShowLoadStateBrowserInvoked()
        {
            if (NesEmu.ON)
            {
                NesEmu.PAUSED = true;
                FormStatesBrowser frm = new FormStatesBrowser(false);
                frm.ShowDialog(this);
                NesEmu.PAUSED = false;
            }
        }
        internal void SaveStateAs()
        {
            if (InvokeRequired)
                Invoke(new Action(SaveStateAsInvoked));
            else
                SaveStateAsInvoked();
        }
        internal void SaveStateAsInvoked()
        {
            if (NesEmu.ON)
            {
                NesEmu.PAUSED = true;
                SaveFileDialog sav = new SaveFileDialog();
                sav.Title = Properties.Resources.Desc14;
                sav.Filter = Properties.Resources.Filter_State;
                if (sav.ShowDialog(this) == DialogResult.OK)
                {
                    while (!NesEmu.isPaused) { }
                    StateHandler.SaveState(sav.FileName, false);
                }
                NesEmu.PAUSED = false;
            }
        }
        internal void LoadStateAs()
        {
            if (InvokeRequired)
                Invoke(new Action(LoadStateAsInvoked));
            else
                LoadStateAsInvoked();
        }
        internal void LoadStateAsInvoked()
        {
            if (NesEmu.ON)
            {
                NesEmu.PAUSED = true;
                OpenFileDialog op = new OpenFileDialog();
                op.Title = Properties.Resources.Desc15;
                op.Filter = Properties.Resources.Filter_State;
                if (op.ShowDialog(this) == DialogResult.OK)
                {
                    while (!NesEmu.isPaused) { }
                    StateHandler.LoadState(op.FileName);
                }
                NesEmu.PAUSED = false;
            }
        }
        internal void ToggleFullscreen()
        {
            if (InvokeRequired)
                Invoke(new Action(ToggleFullscreenInvoked));
            else
                ToggleFullscreenInvoked();
        }
        internal void ToggleFullscreenInvoked()
        {
            MyNesMain.VideoProvider.ResizeBegin();
            System.Threading.Thread.Sleep(100);
            if (WindowState == FormWindowState.Maximized)
            {
                MyNesMain.RendererSettings.Vid_Fullscreen = false;


                FormBorderStyle = FormBorderStyle.Sizable;
                menuStrip1.Visible = true;

                WindowState = FormWindowState.Normal;
            }
            else
            {
                MyNesMain.RendererSettings.Vid_Fullscreen = true;

                FormBorderStyle = FormBorderStyle.None;
                menuStrip1.Visible = false;

                WindowState = FormWindowState.Maximized;

                if (MyNesMain.VideoProvider != null)
                    MyNesMain.VideoProvider.WriteWarningNotification(Properties.Resources.Status44, false);
            }

            MyNesMain.VideoProvider.ResizeEnd();
        }
        internal void EditGameGenieCodes()
        {
            if (InvokeRequired)
                Invoke(new Action(EditGameGenieCodesInvoked));
            else
                EditGameGenieCodesInvoked();
        }
        internal void EditGameGenieCodesInvoked()
        {
            if (NesEmu.ON)
            {
                NesEmu.PAUSED = true;
                FormGameGenie frm = new FormGameGenie();
                frm.ShowDialog(this);
                NesEmu.PAUSED = false;
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0112) // WM_SYSCOMMAND
            {
                // Check your window state here
                if (m.WParam == new IntPtr(0xF030)) // Maximize event - SC_MAXIMIZE from Winuser.h
                {
                    // The window is being maximized
                    MyNesMain.VideoProvider.ResizeBegin();
                    isMaxing = true;
                }
                else if (m.WParam == new IntPtr(0xF120)) // Windowing
                {
                    // The window is being windowed
                    MyNesMain.VideoProvider.ResizeBegin();
                    isMaxing = true;
                }
                else if (m.WParam == new IntPtr(0xF020)) // Minimize event - SC_MINIMIZE from Winuser.h
                {

                }
            }
            base.WndProc(ref m);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.PAUSED = true;
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Properties.Resources.Desc11;
            op.Filter = Properties.Resources.Filter_Rom;
            if (Program.Settings.Misc_RecentFiles != null)
                if (Program.Settings.Misc_RecentFiles.Length > 0)
                    op.FileName = Program.Settings.Misc_RecentFiles[0];
            if (op.ShowDialog(this) == DialogResult.OK)
            {
                LoadGame(op.FileName);
            }
            NesEmu.PAUSED = false;
        }
        private void shutdownEmulationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.ON)
                NesEmu.ShutDown();
            ApplyWindowTitle();
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // We need to shutdown the video renderer manually because of the window become disposed before normal providers shutdown.
            MyNesMain.VideoProvider.ShutDown();
            if (WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;
            // Save settings of window 
            SaveSettings();
        }
        private void FormMain_ResizeBegin(object sender, EventArgs e)
        {
            MyNesMain.VideoProvider.ResizeBegin();
        }
        private void FormMain_ResizeEnd(object sender, EventArgs e)
        {
            if (!isMoving)
                MyNesMain.VideoProvider.ResizeEnd();
            else
            {
                MyNesMain.VideoProvider.Resume();

                isMoving = false;
            }
        }
        private void panel_surface_Resize(object sender, EventArgs e)
        {
            if (isMaxing)
            {
                MyNesMain.VideoProvider.ResizeEnd();
                isMaxing = false;
            }
            else
                isMoving = false;
        }
        private void FormMain_Move(object sender, EventArgs e)
        {
            isMoving = true;

        }
        private void showFPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Vid_ShowFPS = !MyNesMain.RendererSettings.Vid_ShowFPS;
            MyNesMain.VideoProvider.ToggleFPS(MyNesMain.RendererSettings.Vid_ShowFPS);
        }
        private void stretchMultiplyToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            for (int i = 0; i < stretchMultiplyToolStripMenuItem.DropDownItems.Count; i++)
            {
                stretchMultiplyToolStripMenuItem.DropDownItems[i].Text = string.Format("X {0} ( {1} x {2} )", i + 1, MyNesMain.RendererSettings.Vid_Res_W * (i + 1), MyNesMain.RendererSettings.Vid_Res_H * (i + 1));

                ((ToolStripMenuItem)stretchMultiplyToolStripMenuItem.DropDownItems[i]).Checked = (i == (MyNesMain.RendererSettings.Vid_StretchMultiply - 1));
            }
        }
        private void stretchMultiplyToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int index = stretchMultiplyToolStripMenuItem.DropDownItems.IndexOf(e.ClickedItem);

            MyNesMain.RendererSettings.Vid_StretchMultiply = index + 1;

            if (NesEmu.ON)
            {
                // Apply
                if (MyNesMain.RendererSettings.Vid_AutoStretch)
                    ApplyStretch(false);
            }
        }
        private void keepAspectRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Vid_KeepAspectRatio = !MyNesMain.RendererSettings.Vid_KeepAspectRatio;
            MyNesMain.VideoProvider.ToggleAspectRatio(MyNesMain.RendererSettings.Vid_KeepAspectRatio);
        }
        private void autoStretchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoStretchToolStripMenuItem.Checked = !autoStretchToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Vid_AutoStretch = autoStretchToolStripMenuItem.Checked;
        }
        private void hardResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.HardReset();
        }
        private void softResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.SoftReset();
        }
        private void togglePauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.PAUSED = !NesEmu.PAUSED;
        }
        // State slot filling
        private void slotToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            int index = StateHandler.Slot;

            for (int i = 0; i < slotToolStripMenuItem.DropDownItems.Count; i++)
            {
                // Add state file info
                string inf = i + ": " + Properties.Resources.Status16;

                string file = StateHandler.GetStateFile(i);
                if (System.IO.File.Exists(file))
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(file);
                    inf = i + ": " + Properties.Resources.Status14 + " " + info.LastWriteTime.ToLocalTime();
                }
                slotToolStripMenuItem.DropDownItems[i].Text = inf;

                ((ToolStripMenuItem)slotToolStripMenuItem.DropDownItems[i]).Checked = (i == index);
            }
        }
        private void saveStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.ON)
                NesEmu.SaveState();
        }
        private void loadStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.ON)
                NesEmu.LoadState();
        }
        private void slotToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            StateHandler.Slot = slotToolStripMenuItem.DropDownItems.IndexOf(e.ClickedItem);
        }
        private void recentToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            recentToolStripMenuItem.DropDownItems.Clear();
            if (Program.Settings.Misc_RecentFiles == null)
                return;
            for (int i = 0; i < Program.Settings.Misc_RecentFiles.Length; i++)
            {
                ToolStripMenuItem it = new ToolStripMenuItem();
                it.Text = System.IO.Path.GetFileName(Program.Settings.Misc_RecentFiles[i]);
                it.ToolTipText = Program.Settings.Misc_RecentFiles[i];
                it.Tag = Program.Settings.Misc_RecentFiles[i];

                recentToolStripMenuItem.DropDownItems.Add(it);
            }

            loadStateOpenRecentToolStripMenuItem1.Checked = Program.Settings.LoadStateOpenRecent;
        }
        private void recentToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            NesEmu.PAUSED = true;
            gameLoaded = false;
            LoadGame(e.ClickedItem.Tag.ToString());

            if (gameLoaded && Program.Settings.LoadStateOpenRecent)
            {
                NesEmu.PAUSED = true;
                while (!NesEmu.isPaused) { }
                StateHandler.LoadState();
            }
            NesEmu.PAUSED = false;
        }
        private void saveStateAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveStateAs();
        }
        private void loadStateAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadStateAs();
        }
        private void machineToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            togglePauseToolStripMenuItem.Enabled =
            shutdownEmulationToolStripMenuItem.Enabled =
                 hardResetToolStripMenuItem.Enabled =
                 softResetToolStripMenuItem.Enabled =
                 turboToolStripMenuItem.Enabled =
                 NesEmu.ON;
            turboToolStripMenuItem.Checked = !NesEmu.FrameLimiterEnabled;
            useEmulationThreadToolStripMenuItem.Checked = MyNesMain.RendererSettings.UseEmuThread;
        }
        private void stateToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            saveStateAsToolStripMenuItem.Enabled =
                saveStateBrowserToolStripMenuItem.Enabled =
                saveStateToolStripMenuItem.Enabled =
                loadStateAsToolStripMenuItem.Enabled =
                loadStateBrowserToolStripMenuItem.Enabled =
                loadStateToolStripMenuItem.Enabled = NesEmu.ON;
        }
        private void saveStateBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSaveStateBrowser();
        }
        private void loadStateBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLoadStateBrowser();
        }
        private void takeSnapshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MyNesMain.VideoProvider != null)
                MyNesMain.VideoProvider.TakeSnapshot();
        }
        private void audioToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            soundEnabledToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_SoundEnabled;
            useMixerToolStripMenuItem.Checked = !MyNesMain.RendererSettings.Audio_UseDefaultMixer;
            recordSoundToolStripMenuItem.Text = MyNesMain.WaveRecorder.IsRecording ? Properties.Resources.Button5 : Properties.Resources.Button6;

            if (!MyNesMain.WaveRecorder.IsRecording)
            {
                //bufferSizeToolStripMenuItem.Enabled = MyNesMain.AudioProvider.AllowBufferChange;
                frequencyToolStripMenuItem.Enabled = MyNesMain.AudioProvider.AllowFrequencyChange;

                soundEnabledToolStripMenuItem.Enabled =
                useMixerToolStripMenuItem.Enabled = true;
            }
            else
            {
                //bufferSizeToolStripMenuItem.Enabled =
                frequencyToolStripMenuItem.Enabled =
                soundEnabledToolStripMenuItem.Enabled =
                useMixerToolStripMenuItem.Enabled = false;
            }
        }
        private void soundEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_SoundEnabled = !MyNesMain.RendererSettings.Audio_SoundEnabled;
            if (NesEmu.ON)
            {
                NesEmu.SoundEnabled = MyNesMain.RendererSettings.Audio_SoundEnabled;
                if (MyNesMain.AudioProvider != null)
                    MyNesMain.AudioProvider.TogglePause(!NesEmu.SoundEnabled);
                if (!NesEmu.SoundEnabled)
                {
                    if (MyNesMain.WaveRecorder.IsRecording)
                        MyNesMain.WaveRecorder.Stop();
                }
            }
        }
        private void toolStripMenuItem_22050_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_Frequency = 22050;
            ApplyAudioFreq();
        }
        private void hzToolStripMenuItem_44100_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_Frequency = 44100;
            ApplyAudioFreq();
        }
        private void hzToolStripMenuItem_11025_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_Frequency = 11025;
            ApplyAudioFreq();
        }
        private void channelsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            square1ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_SQ1;
            square2ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_SQ2;
            noiseToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_NOZ;
            triangleToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_TRL;
            dMCToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_DMC;
            mMC5Square1ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_MMC5_SQ1;
            mMC5Square2ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_MMC5_SQ2;
            mMC5PCMToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_MMC5_PCM;
            vRC6Square1ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_VRC6_SQ1;
            vRC6Square2ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_VRC6_SQ2;
            vRC6SawToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_VRC6_SAW;
            sunsoft1ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_SUN1;
            sunsoft5B2ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_SUN2;
            sunsoft5B3ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_SUN3;
            namco1631ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT1;
            namco1632ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT2;
            namco1633ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT3;
            namco1634ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT4;
            namco1635ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT5;
            namco1636ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT6;
            namco1637ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT7;
            namco1638ToolStripMenuItem.Checked = MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT8;
        }
        private void square1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            square1ToolStripMenuItem.Checked = !square1ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_SQ1 = square1ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void square2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            square2ToolStripMenuItem.Checked = !square2ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_SQ2 = square2ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            triangleToolStripMenuItem.Checked = !triangleToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_TRL = triangleToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void noiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noiseToolStripMenuItem.Checked = !noiseToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_NOZ = noiseToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void dMCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dMCToolStripMenuItem.Checked = !dMCToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_DMC = dMCToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void mMC5Square1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mMC5Square1ToolStripMenuItem.Checked = !mMC5Square1ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_MMC5_SQ1 = mMC5Square1ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void mMC5Square2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mMC5Square2ToolStripMenuItem.Checked = !mMC5Square2ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_MMC5_SQ2 = mMC5Square2ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void mMC5PCMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mMC5PCMToolStripMenuItem.Checked = !mMC5PCMToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_MMC5_PCM = mMC5PCMToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void vRC6Square1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vRC6Square1ToolStripMenuItem.Checked = !vRC6Square1ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_VRC6_SQ1 = vRC6Square1ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void vRC6Square2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vRC6Square2ToolStripMenuItem.Checked = !vRC6Square2ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_VRC6_SQ2 = vRC6Square2ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void vRC6SawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vRC6SawToolStripMenuItem.Checked = !vRC6SawToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_VRC6_SAW = vRC6SawToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void sunsoft1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sunsoft1ToolStripMenuItem.Checked = !sunsoft1ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_SUN1 = sunsoft1ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void sunsoft5B2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sunsoft5B2ToolStripMenuItem.Checked = !sunsoft5B2ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_SUN2 = sunsoft5B2ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void sunsoft5B3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sunsoft5B3ToolStripMenuItem.Checked = !sunsoft5B3ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_SUN3 = sunsoft5B3ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void regionToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            autoToolStripMenuItem.Checked = MyNesMain.EmuSettings.RegionSetting == (int)RegionSetting.AUTO;
            forceNTSCToolStripMenuItem.Checked = MyNesMain.EmuSettings.RegionSetting == (int)RegionSetting.ForceNTSC;
            forcePALToolStripMenuItem.Checked = MyNesMain.EmuSettings.RegionSetting == (int)RegionSetting.ForcePALB;
            forceDendyToolStripMenuItem.Checked = MyNesMain.EmuSettings.RegionSetting == (int)RegionSetting.ForceDENDY;
        }
        private void autoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.RegionSetting = (int)RegionSetting.AUTO;
            if (NesEmu.ON)
            {
                NesEmu.ApplyRegionSetting();
                NesEmu.HardReset();
                ApplyStretch(true);
            }
            if (MyNesMain.VideoProvider != null)
                MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status17 + " AUTO.", false);
        }
        private void forceNTSCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.RegionSetting = (int)RegionSetting.ForceNTSC;
            if (NesEmu.ON)
            {
                NesEmu.ApplyRegionSetting();
                NesEmu.HardReset();
                ApplyStretch(true);
            }
            if (MyNesMain.VideoProvider != null)
                MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status17 + " Force NTSC.", false);
        }
        private void forcePALToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.RegionSetting = (int)RegionSetting.ForcePALB;
            if (NesEmu.ON)
            {
                NesEmu.ApplyRegionSetting();
                NesEmu.HardReset();
                ApplyStretch(true);
            }
            if (MyNesMain.VideoProvider != null)
                MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status17 + " Force PAL.", false);
        }
        private void forceDendyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.RegionSetting = (int)RegionSetting.ForceDENDY;
            if (NesEmu.ON)
            {
                NesEmu.ApplyRegionSetting();
                NesEmu.HardReset();
                ApplyStretch(true);
            }
            if (MyNesMain.VideoProvider != null)
                MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status17 + " Force DENDY.", false);
        }
        private void player1InputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.PAUSED = true;
            FormInputSettings frm = new FormInputSettings();
            frm.SelectSettingsPage(0);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                // Re-setup the input settings
                SetupInputs();
            }
            NesEmu.PAUSED = false;
        }
        private void player2InputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.PAUSED = true;
            FormInputSettings frm = new FormInputSettings();
            frm.SelectSettingsPage(1);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                // Re-setup the input settings
                SetupInputs();
            }
            NesEmu.PAUSED = false;
        }
        private void player3InputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.PAUSED = true;
            FormInputSettings frm = new FormInputSettings();
            frm.SelectSettingsPage(2);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                // Re-setup the input settings
                SetupInputs();
            }
            NesEmu.PAUSED = false;
        }
        private void player4InputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.PAUSED = true;
            FormInputSettings frm = new FormInputSettings();
            frm.SelectSettingsPage(3);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                // Re-setup the input settings
                SetupInputs();
            }
            NesEmu.PAUSED = false;
        }
        private void connect4PlayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.IsFourPlayers = !NesEmu.IsFourPlayers;

            if (MyNesMain.VideoProvider != null)
                MyNesMain.VideoProvider.WriteInfoNotification(NesEmu.IsFourPlayers ? Properties.Resources.Status42 : Properties.Resources.Status43, false);
        }
        private void inputToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            connect4PlayersToolStripMenuItem.Checked = NesEmu.IsFourPlayers;
            enableGameGenieToolStripMenuItem.Checked = NesEmu.IsGameGenieActive;
            enableGameGenieToolStripMenuItem.Enabled = gameGenieCodesToolStripMenuItem.Enabled = NesEmu.ON;
        }
        private void fullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleFullscreen();
        }
        private void videoToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            showFPSToolStripMenuItem.Checked = MyNesMain.RendererSettings.Vid_ShowFPS;
            autoStretchToolStripMenuItem.Checked = MyNesMain.RendererSettings.Vid_AutoStretch;
            keepAspectRatioToolStripMenuItem.Checked = MyNesMain.RendererSettings.Vid_KeepAspectRatio;

            fullscreenToolStripMenuItem.Checked = WindowState == FormWindowState.Maximized;
            vSyncToolStripMenuItem.Checked = MyNesMain.RendererSettings.Vid_VSync;
            showNotifiacationsToolStripMenuItem.Checked = MyNesMain.RendererSettings.Vid_ShowNotifications;

            vSyncToolStripMenuItem.Enabled = !MyNesMain.RendererSettings.FrameSkipEnabled;
            startGameInFullscreenToolStripMenuItem.Checked = Program.Settings.Win_StartInFullscreen;
            resolutionUpscaleToolStripMenuItem.Checked = MyNesMain.RendererSettings.Vid_Res_Upscale;
            resolutionToolStripMenuItem.Enabled = MyNesMain.RendererSettings.Vid_Res_Upscale;
        }
        private void vSyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Vid_VSync = !MyNesMain.RendererSettings.Vid_VSync;
            MyNesMain.VideoProvider.ResizeBegin();
            System.Threading.Thread.Sleep(100);
            MyNesMain.VideoProvider.ResizeEnd();
        }
        private void turboToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.FrameLimiterEnabled = !NesEmu.FrameLimiterEnabled;
        }
        private void romInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.ON)
            {
                NesEmu.PAUSED = true;

                FormRomInfo frm = new FormRomInfo(NesEmu.CurrentFilePath);
                frm.ShowDialog(this);

                NesEmu.PAUSED = false;
            }
            else
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = Properties.Resources.Desc11;
                op.Filter = Properties.Resources.Filter_Rom;
                if (Program.Settings.Misc_RecentFiles != null)
                    if (Program.Settings.Misc_RecentFiles.Length > 0)
                        op.FileName = Program.Settings.Misc_RecentFiles[0];
                if (op.ShowDialog(this) == DialogResult.OK)
                {
                    switch (Path.GetExtension(op.FileName).ToLower())
                    {
                        case ".nes":
                            {
                                FormRomInfo frm = new FormRomInfo(op.FileName);
                                frm.ShowDialog(this);
                                break;
                            }
                        case ".7z":
                        case ".zip":
                        case ".rar":
                        case ".gzip":
                        case ".tar":
                        case ".bzip2":
                        case ".xz":
                            {
                                string fileName = op.FileName;
                                string tempFolder = Path.GetTempPath() + "\\MYNES\\";
                                SevenZip.SevenZipExtractor EXTRACTOR;
                                try
                                {
                                    EXTRACTOR = new SevenZip.SevenZipExtractor(fileName);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(Properties.Resources.Message35 + ": \n" + ex.Message);
                                    return;
                                }
                                if (EXTRACTOR.ArchiveFileData.Count == 1)
                                {
                                    if (EXTRACTOR.ArchiveFileData[0].FileName.Substring(EXTRACTOR.ArchiveFileData[0].FileName.Length - 4, 4).ToLower() == ".nes")
                                    {
                                        EXTRACTOR.ExtractArchive(tempFolder);
                                        fileName = tempFolder + EXTRACTOR.ArchiveFileData[0].FileName;
                                    }
                                }
                                else
                                {
                                    List<string> filenames = new List<string>();
                                    foreach (SevenZip.ArchiveFileInfo file in EXTRACTOR.ArchiveFileData)
                                    {
                                        filenames.Add(file.FileName);
                                    }
                                    FormFilesList ar = new FormFilesList(filenames.ToArray());
                                    if (ar.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                                    {
                                        string[] fil = { ar.SelectedRom };
                                        EXTRACTOR.ExtractFiles(tempFolder, fil);
                                        fileName = tempFolder + ar.SelectedRom;
                                    }
                                    else
                                    { return; }
                                }
                                FormRomInfo frm = new FormRomInfo(fileName);
                                frm.ShowDialog(this);
                                break;
                            }
                    }

                }
            }
        }
        private void enableGameGenieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.IsGameGenieActive = !NesEmu.IsGameGenieActive;
            MyNesMain.VideoProvider.WriteInfoNotification(NesEmu.IsGameGenieActive ? Properties.Resources.Status18 : Properties.Resources.Status19, false);
        }
        private void gameGenieCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditGameGenieCodes();
        }
        private void recordSoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.ON)
            {
                if (MyNesMain.WaveRecorder.IsRecording)
                    MyNesMain.WaveRecorder.Stop();
                else
                    MyNesMain.RecordWave();
            }
        }
        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_Volume += 5;
            if (MyNesMain.RendererSettings.Audio_Volume > 100)
                MyNesMain.RendererSettings.Audio_Volume = 100;
            MyNesMain.AudioProvider.SetVolume(MyNesMain.RendererSettings.Audio_Volume);
            MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status20 + " " + MyNesMain.RendererSettings.Audio_Volume, true);
            audioToolStripMenuItem.ShowDropDown();
            volumeToolStripMenuItem.ShowDropDown();
        }
        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_Volume -= 5;
            if (MyNesMain.RendererSettings.Audio_Volume < 0)
                MyNesMain.RendererSettings.Audio_Volume = 0;
            MyNesMain.AudioProvider.SetVolume(MyNesMain.RendererSettings.Audio_Volume);
            MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status20 + " " + MyNesMain.RendererSettings.Audio_Volume, true);
            audioToolStripMenuItem.ShowDropDown();
            volumeToolStripMenuItem.ShowDropDown();
        }
        private void showNotifiacationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Vid_ShowNotifications = !MyNesMain.RendererSettings.Vid_ShowNotifications;
        }
        private void preferencesToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            pauseEmulationWhenFocusLostToolStripMenuItem.Checked = Program.Settings.PauseEmuWhenFocusLost;
            replaceSnpashotToolStripMenuItem.Checked = MyNesMain.EmuSettings.SnapsReplace;
            saveSRAMFileOnEmuShutdownToolStripMenuItem.Checked = MyNesMain.EmuSettings.SaveSRAMAtEmuShutdown;
            showLauncherAtStartUpToolStripMenuItem.Checked = Program.Settings.LauncherShowAyAppStart;
            shutdownEmuExitMyNesOnEscapePressToolStripMenuItem.Checked = Program.Settings.ShutdowOnEscapePress;
        }
        private void snapshotFormatToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            bMPToolStripMenuItem.Checked = MyNesMain.EmuSettings.SnapsFormat == ".bmp";
            gIFToolStripMenuItem.Checked = MyNesMain.EmuSettings.SnapsFormat == ".gif";
            jPGToolStripMenuItem.Checked = MyNesMain.EmuSettings.SnapsFormat == ".jpg";
            pNGToolStripMenuItem.Checked = MyNesMain.EmuSettings.SnapsFormat == ".png";
            tIFFToolStripMenuItem.Checked = MyNesMain.EmuSettings.SnapsFormat == ".tiff";
            eMFToolStripMenuItem.Checked = MyNesMain.EmuSettings.SnapsFormat == ".emf";
            wMFToolStripMenuItem.Checked = MyNesMain.EmuSettings.SnapsFormat == ".wmf";
            eXIFToolStripMenuItem.Checked = MyNesMain.EmuSettings.SnapsFormat == ".exif";
        }
        private void FormMain_Deactivate(object sender, EventArgs e)
        {
            if (!Program.Settings.PauseEmuWhenFocusLost)
                return;
            if (!NesEmu.PAUSED)
            {
                if (NesEmu.ON)
                    NesEmu.PAUSED = true;
                pausedByMainWindow = true;
            }
        }
        private void FormMain_Activated(object sender, EventArgs e)
        {
            if (!Program.Settings.PauseEmuWhenFocusLost)
                return;
            if (pausedByMainWindow)
            {
                pausedByMainWindow = false;
                if (NesEmu.ON)
                    NesEmu.PAUSED = false;
            }
        }
        private void pauseEmulationWhenFocusLostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.PauseEmuWhenFocusLost = !Program.Settings.PauseEmuWhenFocusLost;
        }
        private void replaceSnpashotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.SnapsReplace = !MyNesMain.EmuSettings.SnapsReplace;
        }
        private void jPGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.SnapsFormat = ".jpg";
        }
        private void pNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.SnapsFormat = ".png";
        }
        private void bMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.SnapsFormat = ".bmp";
        }
        private void gIFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.SnapsFormat = ".gif";
        }
        private void tIFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.SnapsFormat = ".tiff";
        }
        private void eMFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.SnapsFormat = ".emf";
        }
        private void wMFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.SnapsFormat = ".wmf";
        }
        private void eXIFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.SnapsFormat = ".exif";
        }
        private void foldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.ON)
                NesEmu.PAUSED = true;

            FormFolders frm = new FormFolders();
            frm.ShowDialog(this);

            if (NesEmu.ON)
                NesEmu.PAUSED = false;
        }
        private void saveSRAMFileOnEmuShutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.EmuSettings.SaveSRAMAtEmuShutdown = !MyNesMain.EmuSettings.SaveSRAMAtEmuShutdown;
        }
        private void myNesInSourceforgenetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/alaahadid/My-Nes");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void myNesFacebookPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.facebook.com/My-Nes-427707727244076/");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void aboutMyNesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.ON)
                NesEmu.PAUSED = true;

            FormAbout frm = new FormAbout();
            frm.ShowDialog(this);

            if (NesEmu.ON)
                NesEmu.PAUSED = false;
        }
        private void frameSkipToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            enableFrameSkipToolStripMenuItem.Checked = MyNesMain.RendererSettings.FrameSkipEnabled;
            interval2ToolStripMenuItem.Checked = MyNesMain.RendererSettings.FrameSkipInterval == 2;
            interval3ToolStripMenuItem.Checked = MyNesMain.RendererSettings.FrameSkipInterval == 3;
            interval4ToolStripMenuItem.Checked = MyNesMain.RendererSettings.FrameSkipInterval == 4;
        }
        private void enableFrameSkipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.FrameSkipEnabled = NesEmu.FrameSkipEnabled = !MyNesMain.RendererSettings.FrameSkipEnabled;

            MyNesMain.VideoProvider.ResizeBegin();
            System.Threading.Thread.Sleep(100);
            MyNesMain.VideoProvider.ResizeEnd();
        }
        private void interval2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.FrameSkipInterval = NesEmu.FrameSkipInterval = 2;

            MyNesMain.VideoProvider.ResizeBegin();
            System.Threading.Thread.Sleep(100);
            MyNesMain.VideoProvider.ResizeEnd();
        }
        private void interval3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.FrameSkipInterval = NesEmu.FrameSkipInterval = 3;

            MyNesMain.VideoProvider.ResizeBegin();
            System.Threading.Thread.Sleep(100);
            MyNesMain.VideoProvider.ResizeEnd();
        }
        private void interval4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.FrameSkipInterval = NesEmu.FrameSkipInterval = 4;
            MyNesMain.VideoProvider.ResizeBegin();
            System.Threading.Thread.Sleep(100);
            MyNesMain.VideoProvider.ResizeEnd();
        }
        private void launcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.OwnedForms)
            {
                if (frm.Tag.ToString() == "Launcher")
                {
                    frm.Activate();
                    return;
                }
            }
            FormLauncher newfrm = new FormLauncher();
            newfrm.Show(this);
        }
        private void filterToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            pointToolStripMenuItem.Checked = MyNesMain.RendererSettings.Vid_Filter == 0;
            linearToolStripMenuItem.Checked = MyNesMain.RendererSettings.Vid_Filter == 1;
        }
        private void pointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Vid_Filter = 0;
            MyNesMain.VideoProvider.ApplyFilter();
        }
        private void linearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Vid_Filter = 1;
            MyNesMain.VideoProvider.ApplyFilter();
        }
        private void rendererToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem it in rendererToolStripMenuItem.DropDownItems)
            {
                it.Checked = it.Tag.ToString() == MyNesMain.RendererSettings.Video_ProviderID;
            }
        }
        private void rendererToolStripMenuItem_audio_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem it in rendererToolStripMenuItem_audio.DropDownItems)
            {
                it.Checked = it.Tag.ToString() == MyNesMain.RendererSettings.Audio_ProviderID;
            }
        }
        private void rendererToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            MyNesMain.RendererSettings.Video_ProviderID = e.ClickedItem.Tag.ToString();
            MessageBox.Show(Properties.Resources.Message36);
        }
        private void rendererToolStripMenuItem_audio_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            MyNesMain.RendererSettings.Audio_ProviderID = e.ClickedItem.Tag.ToString();
            MessageBox.Show(Properties.Resources.Message36);
        }
        private void showLauncherAtStartUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.LauncherShowAyAppStart = !Program.Settings.LauncherShowAyAppStart;
        }
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string help_file = Path.Combine(Program.ApplicationFolder, "Manual.pdf");
                if (!File.Exists(help_file))
                    help_file = Path.Combine(Program.ApplicationFolder, "Help.chm");
                if (!File.Exists(help_file))
                    help_file = Path.Combine(Program.ApplicationFolder, "Readme.txt");

                if (File.Exists(help_file))
                    System.Diagnostics.Process.Start(help_file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void gettingsStartedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGettingStarted frm = new FormGettingStarted();
            frm.ShowDialog();

            MyNesMain.VideoProvider.ApplyFilter();
        }
        private void namco1631ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            namco1631ToolStripMenuItem.Checked = !namco1631ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT1 = namco1631ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void namco1632ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            namco1632ToolStripMenuItem.Checked = !namco1632ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT2 = namco1632ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void namco1633ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            namco1633ToolStripMenuItem.Checked = !namco1633ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT3 = namco1633ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void namco1634ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            namco1634ToolStripMenuItem.Checked = !namco1634ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT4 = namco1634ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void namco1635ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            namco1635ToolStripMenuItem.Checked = !namco1635ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT5 = namco1635ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void namco1636ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            namco1636ToolStripMenuItem.Checked = !namco1636ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT6 = namco1636ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void namco1637ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            namco1637ToolStripMenuItem.Checked = !namco1637ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT7 = namco1637ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void namco1638ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            namco1638ToolStripMenuItem.Checked = !namco1638ToolStripMenuItem.Checked;
            MyNesMain.RendererSettings.Audio_ChannelEnabled_NMT8 = namco1638ToolStripMenuItem.Checked;
            NesEmu.ApplyAudioSettings(false);
            audioToolStripMenuItem.ShowDropDown();
            channelsToolStripMenuItem.ShowDropDown();
        }
        private void sDL2SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.ON)
                NesEmu.PAUSED = true;

            FormSDL2Settings frm = new FormSDL2Settings();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                MyNesMain.VideoProvider.ResizeBegin();
                System.Threading.Thread.Sleep(100);
                ApplyAudioFreq();
                MyNesMain.VideoProvider.ResizeEnd();
            }

            if (NesEmu.ON)
                NesEmu.PAUSED = false;
        }
        // Mouse hide
        private void panel_surface_MouseEnter(object sender, EventArgs e)
        {
            if (!isMouseVisible)
            {
                Cursor.Show();
                isMouseVisible = true;
            }
            // Start the timer
            timer_mouse_hider.Start();
            mouseHiderCounter = mouseHiderReload;
        }
        private void panel_surface_MouseLeave(object sender, EventArgs e)
        {
            if (!isMouseVisible)
            {
                Cursor.Show();
                isMouseVisible = true;
            }
            timer_mouse_hider.Stop();
            mouseHiderCounter = mouseHiderReload;
        }
        private void panel_surface_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseVisible)
            {
                Cursor.Show();
                isMouseVisible = true;
            }
            // Start the timer
            timer_mouse_hider.Start();
            mouseHiderCounter = mouseHiderReload;
        }
        private void timer_mouse_hider_Tick(object sender, EventArgs e)
        {
            if (mouseHiderCounter > 0)
                mouseHiderCounter--;
            else
            {
                if (isMouseVisible)
                {
                    Cursor.Hide();
                    isMouseVisible = false;
                }
                timer_mouse_hider.Stop();
            }
        }
        private void startGameInFullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.Win_StartInFullscreen = !Program.Settings.Win_StartInFullscreen;
        }
        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    fullscreenToolStripMenuItem_Click(this, null);
                }
                else
                {
                    if (Program.Settings.ShutdowOnEscapePress)
                    {
                        if (NesEmu.ON)
                        {
                            NesEmu.ShutDown();
                            ApplyWindowTitle();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
            }
        }
        private void shutdownEmuExitMyNesOnEscapePressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.ShutdowOnEscapePress = !Program.Settings.ShutdowOnEscapePress;
        }
        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (NesEmu.ON)
                NesEmu.PAUSED = true;

            showFPSToolStripMenuItem1.Checked = MyNesMain.RendererSettings.Vid_ShowFPS;
            keepAspectRationToolStripMenuItem.Checked = MyNesMain.RendererSettings.Vid_KeepAspectRatio;
            fullscreenToolStripMenuItem1.Checked = WindowState == FormWindowState.Maximized;
            startGameInFullscreenToolStripMenuItem1.Checked = Program.Settings.Win_StartInFullscreen;
            loadStateOpenRecentToolStripMenuItem.Checked = Program.Settings.LoadStateOpenRecent;

            shutdownToolStripMenuItem.Enabled =
                 hardResetToolStripMenuItem1.Enabled =
                 softResetToolStripMenuItem1.Enabled =
                 turboToolStripMenuItem1.Enabled =
                 NesEmu.ON;

            soundEnabledToolStripMenuItem1.Checked = MyNesMain.RendererSettings.Audio_SoundEnabled;

            recentToolStripMenuItem1.DropDownItems.Clear();
            if (Program.Settings.Misc_RecentFiles == null)
                return;
            for (int i = 0; i < Program.Settings.Misc_RecentFiles.Length; i++)
            {
                ToolStripMenuItem it = new ToolStripMenuItem();
                it.Text = System.IO.Path.GetFileName(Program.Settings.Misc_RecentFiles[i]);
                it.ToolTipText = Program.Settings.Misc_RecentFiles[i];
                it.Tag = Program.Settings.Misc_RecentFiles[i];

                recentToolStripMenuItem1.DropDownItems.Add(it);
            }
        }
        private void stateSlotToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            int index = StateHandler.Slot;

            for (int i = 0; i < stateSlotToolStripMenuItem.DropDownItems.Count; i++)
            {
                // Add state file info
                string inf = i + ": " + Properties.Resources.Status16;

                string file = StateHandler.GetStateFile(i);
                if (System.IO.File.Exists(file))
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(file);
                    inf = i + ": " + Properties.Resources.Status14 + " " + info.LastWriteTime.ToLocalTime();
                }
                stateSlotToolStripMenuItem.DropDownItems[i].Text = inf;

                ((ToolStripMenuItem)stateSlotToolStripMenuItem.DropDownItems[i]).Checked = (i == index);
            }
        }
        private void stateSlotToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            StateHandler.Slot = stateSlotToolStripMenuItem.DropDownItems.IndexOf(e.ClickedItem);
        }
        private void volumeUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_Volume += 5;
            if (MyNesMain.RendererSettings.Audio_Volume > 100)
                MyNesMain.RendererSettings.Audio_Volume = 100;
            MyNesMain.AudioProvider.SetVolume(MyNesMain.RendererSettings.Audio_Volume);
            MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status20 + " " + MyNesMain.RendererSettings.Audio_Volume, true);

            contextMenuStrip1.Show();
        }
        private void volumeDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_Volume -= 5;
            if (MyNesMain.RendererSettings.Audio_Volume < 0)
                MyNesMain.RendererSettings.Audio_Volume = 0;
            MyNesMain.AudioProvider.SetVolume(MyNesMain.RendererSettings.Audio_Volume);
            MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status20 + " " + MyNesMain.RendererSettings.Audio_Volume, true);

            contextMenuStrip1.Show();
        }
        private void configureInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.PAUSED = true;
            FormInputSettings frm = new FormInputSettings();
            frm.SelectSettingsPage(0);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                // Re-setup the input settings
                SetupInputs();
            }
            NesEmu.PAUSED = false;
        }
        private void contextMenuStrip1_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (NesEmu.ON)
                NesEmu.PAUSED = false;
        }
        private void loadStateOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gameLoaded = false;
            openToolStripMenuItem_Click(this, new EventArgs());

            if (gameLoaded)
            {
                NesEmu.PAUSED = true;
                while (!NesEmu.isPaused) { }
                StateHandler.LoadState();
                NesEmu.PAUSED = false;
            }
        }
        private void whenOpenARomUsingRecentLoadStatDirectlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.LoadStateOpenRecent = !Program.Settings.LoadStateOpenRecent;
        }
        private void ToolStripMenuItem_48000_hz_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_Frequency = 48000;
            ApplyAudioFreq();
        }
        private void useMixerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_UseDefaultMixer = !MyNesMain.RendererSettings.Audio_UseDefaultMixer;
            if (NesEmu.ON)
                NesEmu.PAUSED = true;

            System.Threading.Thread.Sleep(200);
            NesEmu.InitializeDACTables(true);

            if (NesEmu.ON)
                NesEmu.PAUSED = false;
            MyNesMain.VideoProvider.WriteInfoNotification(MyNesMain.RendererSettings.Audio_UseDefaultMixer ? Properties.Resources.Status53 : Properties.Resources.Status52, false);
        }
        private void panel_surface_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            fullscreenToolStripMenuItem_Click(this, new EventArgs());
        }
        private void bufferSizeToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            /*int index = MyNesMain.RendererSettings.Audio_PlaybackBufferSizeInKB - 8;

            for (int i = 0; i < bufferSizeToolStripMenuItem.DropDownItems.Count; i++)
            {
                ((ToolStripMenuItem)bufferSizeToolStripMenuItem.DropDownItems[i]).Checked = i == index;
            }*/
        }
        private void bufferSizeToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            /*int index = bufferSizeToolStripMenuItem.DropDownItems.IndexOf(e.ClickedItem);
            MyNesMain.RendererSettings.Audio_PlaybackBufferSizeInKB = index + 8;
            ApplyAudioFreq();

            audioToolStripMenuItem.ShowDropDown();
            bufferSizeToolStripMenuItem.ShowDropDown();*/
        }
        private void frequencyToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)frequencyToolStripMenuItem.DropDownItems[0]).Checked = MyNesMain.RendererSettings.Audio_Frequency == 11025;
            ((ToolStripMenuItem)frequencyToolStripMenuItem.DropDownItems[1]).Checked = MyNesMain.RendererSettings.Audio_Frequency == 22050;
            ((ToolStripMenuItem)frequencyToolStripMenuItem.DropDownItems[2]).Checked = MyNesMain.RendererSettings.Audio_Frequency == 44100;
            ((ToolStripMenuItem)frequencyToolStripMenuItem.DropDownItems[3]).Checked = MyNesMain.RendererSettings.Audio_Frequency == 48000;
        }
        private void shortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.PAUSED = true;
            FormInputSettings frm = new FormInputSettings();
            frm.SelectSettingsPage(4);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                // Re-setup the input settings
                SetupInputs();
            }
            NesEmu.PAUSED = false;
        }
        private void FormMain_Shown(object sender, EventArgs e)
        {
            SetupInputs();
        }
        private void useEmulationThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.UseEmuThread = !MyNesMain.RendererSettings.UseEmuThread;
        }
        private void myNesWikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { System.Diagnostics.Process.Start("https://github.com/alaahadid/My-Nes/wiki"); } catch { }
        }

        private void resolutionToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Get res
            string[] res = e.ClickedItem.Text.Split(new string[] { " x " }, StringSplitOptions.RemoveEmptyEntries);
            int w = 0;
            int h = 0;

            int.TryParse(res[0], out w);
            int.TryParse(res[1], out h);

            if (w != 0 && h != 0)
            {
                MyNesMain.RendererSettings.Vid_Res_W = w;
                MyNesMain.RendererSettings.Vid_Res_H = h;
                MessageBox.Show("Rendering resolution is set to " + w + " x " + h + ", My Nes requires to be restarted in order to this change to take effect.");
            }
        }

        private void resolutionToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem it in resolutionToolStripMenuItem.DropDownItems)
            {
                string[] res = it.Text.Split(new string[] { " x " }, StringSplitOptions.RemoveEmptyEntries);
                int w = 0;
                int h = 0;

                int.TryParse(res[0], out w);
                int.TryParse(res[1], out h);

                it.Checked = MyNesMain.RendererSettings.Vid_Res_W == w && MyNesMain.RendererSettings.Vid_Res_H == h;
            }
        }
        private void resolutionUpscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Vid_Res_Upscale = !MyNesMain.RendererSettings.Vid_Res_Upscale;
            MessageBox.Show("My Nes requires to be restarted in order for this change to take effect.");
        }
    }
}
