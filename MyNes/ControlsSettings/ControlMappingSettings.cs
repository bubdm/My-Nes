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
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyNes
{
    [System.Serializable]
    public class ControlMappingSettings
    {
        public ControlMappingSettings()
        {
            FilePath = Path.Combine(Program.WorkingFolder, "controls.mnc");
        }
        public string FilePath { get; private set; }
        // Joypads
        public List<IInputSettingsJoypad> Joypad1Devices = new List<IInputSettingsJoypad>();
        public string Joypad1DeviceGuid;
        public bool Joypad1AutoSwitchBackToKeyboard;

        public List<IInputSettingsJoypad> Joypad2Devices = new List<IInputSettingsJoypad>();
        public string Joypad2DeviceGuid;
        public bool Joypad2AutoSwitchBackToKeyboard;

        public List<IInputSettingsJoypad> Joypad3Devices = new List<IInputSettingsJoypad>();
        public string Joypad3DeviceGuid;
        public bool Joypad3AutoSwitchBackToKeyboard;

        public List<IInputSettingsJoypad> Joypad4Devices = new List<IInputSettingsJoypad>();
        public string Joypad4DeviceGuid;
        public bool Joypad4AutoSwitchBackToKeyboard;

        public List<IInputSettingsVSUnisystemDIP> VSUnisystemDIPDevices = new List<IInputSettingsVSUnisystemDIP>();
        public string VSUnisystemDIPDeviceGuid;
        public bool VSUnisystemDIPAutoSwitchBackToKeyboard;

        public List<InputSettingsShortcuts> ShortcutsDevices = new List<InputSettingsShortcuts>();
        public string ShortcutsDeviceGuid;
        public bool ShortcutsAutoSwitchBackToKeyboard;

        public void BuildDefaultControlSettings()
        {
            this.Joypad1Devices = new List<IInputSettingsJoypad>();
            this.Joypad2Devices = new List<IInputSettingsJoypad>();
            this.Joypad3Devices = new List<IInputSettingsJoypad>();
            this.Joypad4Devices = new List<IInputSettingsJoypad>();
            this.VSUnisystemDIPDevices = new List<IInputSettingsVSUnisystemDIP>();
            this.ShortcutsDevices = new List<InputSettingsShortcuts>();

            DirectInput di = new DirectInput();
            foreach (DeviceInstance ins in di.GetDevices())
            {
                if (ins.Type == DeviceType.Keyboard)
                {
                    // Player 1 joypad
                    IInputSettingsJoypad joy1 = new IInputSettingsJoypad();
                    joy1.DeviceGuid = ins.InstanceGuid.ToString();
                    joy1.ButtonA = "X";
                    joy1.ButtonB = "Z";
                    joy1.ButtonTurboA = "S";
                    joy1.ButtonTurboB = "A";
                    joy1.ButtonDown = "DownArrow";
                    joy1.ButtonLeft = "LeftArrow";
                    joy1.ButtonRight = "RightArrow";
                    joy1.ButtonUp = "UpArrow";
                    joy1.ButtonSelect = "C";
                    joy1.ButtonStart = "V";
                    this.Joypad1Devices.Add(joy1);
                    this.Joypad1DeviceGuid = joy1.DeviceGuid;
                    this.Joypad1AutoSwitchBackToKeyboard = true;
                    // Player 2 joypad
                    IInputSettingsJoypad joy2 = new IInputSettingsJoypad();
                    joy2.DeviceGuid = ins.InstanceGuid.ToString();
                    joy2.ButtonA = "K";
                    joy2.ButtonB = "L";
                    joy2.ButtonTurboA = "I";
                    joy2.ButtonTurboB = "O";
                    joy2.ButtonDown = "S";
                    joy2.ButtonLeft = "A";
                    joy2.ButtonRight = "D";
                    joy2.ButtonUp = "W";
                    joy2.ButtonSelect = "B";
                    joy2.ButtonStart = "N";
                    this.Joypad2Devices.Add(joy2);
                    this.Joypad2DeviceGuid = joy2.DeviceGuid;
                    this.Joypad2AutoSwitchBackToKeyboard = true;
                    // Player 3
                    this.Joypad3Devices = new List<IInputSettingsJoypad>();
                    this.Joypad3DeviceGuid = "";
                    this.Joypad3AutoSwitchBackToKeyboard = true;
                    // Player 4
                    this.Joypad4Devices = new List<IInputSettingsJoypad>();
                    this.Joypad4DeviceGuid = "";
                    this.Joypad4AutoSwitchBackToKeyboard = true;
                    // VSUnisystem
                    IInputSettingsVSUnisystemDIP vs = new IInputSettingsVSUnisystemDIP();
                    vs.DeviceGuid = ins.InstanceGuid.ToString();
                    vs.CreditServiceButton = "End";
                    vs.DIPSwitch1 = "NumberPad1";
                    vs.DIPSwitch2 = "NumberPad2";
                    vs.DIPSwitch3 = "NumberPad3";
                    vs.DIPSwitch4 = "NumberPad4";
                    vs.DIPSwitch5 = "NumberPad5";
                    vs.DIPSwitch6 = "NumberPad6";
                    vs.DIPSwitch7 = "NumberPad7";
                    vs.DIPSwitch8 = "NumberPad8";
                    vs.CreditLeftCoinSlot = "Insert";
                    vs.CreditRightCoinSlot = "Home";
                    this.VSUnisystemDIPDevices.Add(vs);
                    this.VSUnisystemDIPDeviceGuid = vs.DeviceGuid;
                    this.VSUnisystemDIPAutoSwitchBackToKeyboard = true;
                    // Shortcuts
                    InputSettingsShortcuts shortcuts = new InputSettingsShortcuts();
                    shortcuts.DeviceGuid = ins.InstanceGuid.ToString();
                    shortcuts.KeyShutDown = "F1";
                    shortcuts.KeyTogglePause = "F2";
                    shortcuts.KeySoftReset = "F3";
                    shortcuts.KeyHardReset = "F4";
                    shortcuts.KeyTakeSnapshot = "F5";
                    shortcuts.KeySaveState = "F6";
                    shortcuts.KeySaveStateBrowser = "F7";
                    shortcuts.KeySaveStateAs = "F8";
                    shortcuts.KeyLoadState = "F9";
                    shortcuts.KeyLoadStateBrowser = "F10";
                    shortcuts.KeyLoadStateAs = "F11";
                    shortcuts.KeyToggleFullscreen = "F12";
                    shortcuts.KeyTurbo = "Delete";
                    shortcuts.KeyVolumeUp = "NumberPadPlus";
                    shortcuts.KeyVolumeDown = "NumberPadMinus";
                    shortcuts.KeyToggleSoundEnable = "NumberPadStar";
                    shortcuts.KeyRecordSound = "NumberPadEnter";
                    shortcuts.KeyToggleKeepAspectRatio = "NumberPadSlash";
                    shortcuts.KeySetStateSlot0 = "D0";
                    shortcuts.KeySetStateSlot1 = "D1";
                    shortcuts.KeySetStateSlot2 = "D2";
                    shortcuts.KeySetStateSlot3 = "D3";
                    shortcuts.KeySetStateSlot4 = "D4";
                    shortcuts.KeySetStateSlot5 = "D5";
                    shortcuts.KeySetStateSlot6 = "D6";
                    shortcuts.KeySetStateSlot7 = "D7";
                    shortcuts.KeySetStateSlot8 = "D8";
                    shortcuts.KeySetStateSlot9 = "D9";
                    shortcuts.KeyToggleFPS = "NumberPad0";
                    shortcuts.KeyConnect4Players = "NumberPad4";
                    shortcuts.KeyConnectGameGenie = "PageUp";
                    shortcuts.KeyGameGenieCodes = "PageDown";
                    shortcuts.KeyToggleUseSoundMixer = "NumberPad8";
                    this.ShortcutsDevices.Add(shortcuts);
                    this.ShortcutsDeviceGuid = shortcuts.DeviceGuid;
                    this.ShortcutsAutoSwitchBackToKeyboard = true;
                    break;
                }
            }

            // X Controllers
            IInputSettingsJoypad joy = new IInputSettingsJoypad();
            joy.DeviceGuid = "x-controller-1";
            joy.ButtonA = "A";
            joy.ButtonB = "X";
            joy.ButtonTurboA = "B";
            joy.ButtonTurboB = "Y";
            joy.ButtonDown = "DPadDown";
            joy.ButtonLeft = "DPadLeft";
            joy.ButtonRight = "DPadRight";
            joy.ButtonUp = "DPadUp";
            joy.ButtonSelect = "Back";
            joy.ButtonStart = "Start";
            this.Joypad1DeviceGuid = joy.DeviceGuid;
            this.Joypad1Devices.Add(joy);

            joy = new IInputSettingsJoypad();
            joy.DeviceGuid = "x-controller-2";
            joy.ButtonA = "A";
            joy.ButtonB = "X";
            joy.ButtonTurboA = "B";
            joy.ButtonTurboB = "Y";
            joy.ButtonDown = "DPadDown";
            joy.ButtonLeft = "DPadLeft";
            joy.ButtonRight = "DPadRight";
            joy.ButtonUp = "DPadUp";
            joy.ButtonSelect = "Back";
            joy.ButtonStart = "Start";
            this.Joypad2DeviceGuid = joy.DeviceGuid;
            this.Joypad2Devices.Add(joy);

            joy = new IInputSettingsJoypad();
            joy.DeviceGuid = "x-controller-3";
            joy.ButtonA = "A";
            joy.ButtonB = "X";
            joy.ButtonTurboA = "B";
            joy.ButtonTurboB = "Y";
            joy.ButtonDown = "DPadDown";
            joy.ButtonLeft = "DPadLeft";
            joy.ButtonRight = "DPadRight";
            joy.ButtonUp = "DPadUp";
            joy.ButtonSelect = "Back";
            joy.ButtonStart = "Start";
            this.Joypad3DeviceGuid = joy.DeviceGuid;
            this.Joypad3Devices.Add(joy);

            joy = new IInputSettingsJoypad();
            joy.DeviceGuid = "x-controller-4";
            joy.ButtonA = "A";
            joy.ButtonB = "X";
            joy.ButtonTurboA = "B";
            joy.ButtonTurboB = "Y";
            joy.ButtonDown = "DPadDown";
            joy.ButtonLeft = "DPadLeft";
            joy.ButtonRight = "DPadRight";
            joy.ButtonUp = "DPadUp";
            joy.ButtonSelect = "Back";
            joy.ButtonStart = "Start";
            this.Joypad4DeviceGuid = joy.DeviceGuid;
            this.Joypad4Devices.Add(joy);

            /*
            // Shortcuts
            InputSettingsShortcuts shortcut = new InputSettingsShortcuts();
            shortcut.DeviceGuid = "x-controller-1";
            shortcut.KeyShutDown = "";
            shortcut.KeyTogglePause = "";
            shortcut.KeySoftReset = "";
            shortcut.KeyHardReset = "";
            shortcut.KeyTakeSnapshot = "";
            shortcut.KeySaveState = "RightShoulder";
            shortcut.KeySaveStateBrowser = "";
            shortcut.KeySaveStateAs = "";
            shortcut.KeyLoadState = "LeftShoulder";
            shortcut.KeyLoadStateBrowser = "";
            shortcut.KeyLoadStateAs = "";
            shortcut.KeyToggleFullscreen = "";
            shortcut.KeyTurbo = "";
            this.ShortcutsDeviceGuid = shortcut.DeviceGuid;
            this.ShortcutsDevices.Add(shortcut);*/
        }
        public void Check()
        {
            // Player 1 joypad
            if (Joypad1DeviceGuid == null)
            {
                Console.WriteLine("Input: error found for Joypad 1, it has been reset to none.");
                Joypad1DeviceGuid = "";
                DirectInput di = new DirectInput();
                foreach (DeviceInstance ins in di.GetDevices())
                {
                    if (ins.Type == DeviceType.Keyboard)
                    {
                        this.Joypad1Devices = new List<IInputSettingsJoypad>();
                        IInputSettingsJoypad joy1 = new IInputSettingsJoypad();
                        joy1.DeviceGuid = ins.InstanceGuid.ToString();
                        joy1.ButtonA = "X";
                        joy1.ButtonB = "Z";
                        joy1.ButtonTurboA = "S";
                        joy1.ButtonTurboB = "A";
                        joy1.ButtonDown = "DownArrow";
                        joy1.ButtonLeft = "LeftArrow";
                        joy1.ButtonRight = "RightArrow";
                        joy1.ButtonUp = "UpArrow";
                        joy1.ButtonSelect = "C";
                        joy1.ButtonStart = "V";
                        this.Joypad1Devices.Add(joy1);
                        this.Joypad1DeviceGuid = joy1.DeviceGuid;
                        this.Joypad1AutoSwitchBackToKeyboard = true;
                        Console.WriteLine("Input: error found for Joypad 1, it has been reset to keyboard.");
                        break;
                    }
                }
            }
            // Player 2 joypad
            if (Joypad2DeviceGuid == null)
            {
                Console.WriteLine("Input: error found for Joypad 2, it has been reset to none.");
                Joypad2DeviceGuid = "";
                DirectInput di = new DirectInput();
                foreach (DeviceInstance ins in di.GetDevices())
                {
                    if (ins.Type == DeviceType.Keyboard)
                    {
                        this.Joypad2Devices = new List<IInputSettingsJoypad>();
                        IInputSettingsJoypad joy2 = new IInputSettingsJoypad();
                        joy2.DeviceGuid = ins.InstanceGuid.ToString();
                        joy2.ButtonA = "K";
                        joy2.ButtonB = "L";
                        joy2.ButtonTurboA = "I";
                        joy2.ButtonTurboB = "O";
                        joy2.ButtonDown = "S";
                        joy2.ButtonLeft = "A";
                        joy2.ButtonRight = "D";
                        joy2.ButtonUp = "W";
                        joy2.ButtonSelect = "B";
                        joy2.ButtonStart = "N";
                        this.Joypad2Devices.Add(joy2);
                        this.Joypad2DeviceGuid = joy2.DeviceGuid;
                        this.Joypad2AutoSwitchBackToKeyboard = true;
                        Console.WriteLine("Input: error found for Joypad 2, it has been reset to keyboard.");
                        break;
                    }
                }
            }
            // Player 3
            if (Joypad3DeviceGuid == "")
            {
                this.Joypad3Devices = new List<IInputSettingsJoypad>();
                this.Joypad3DeviceGuid = "";
                this.Joypad3AutoSwitchBackToKeyboard = true;
                Console.WriteLine("Input: error found for Joypad 3, it has been reset to none.");
            }

            // Player 4
            if (Joypad4DeviceGuid == "")
            {
                this.Joypad4Devices = new List<IInputSettingsJoypad>();
                this.Joypad4DeviceGuid = "";
                this.Joypad4AutoSwitchBackToKeyboard = true;
                Console.WriteLine("Input: error found for Joypad 4, it has been reset to none.");
            }
            // Shortcuts
            if (ShortcutsDeviceGuid == null)
            {
                Console.WriteLine("Input: error found for shortcuts, it has been reset to none.");
                this.ShortcutsDevices = new List<InputSettingsShortcuts>();
                ShortcutsDeviceGuid = "";
                DirectInput di = new DirectInput();
                foreach (DeviceInstance ins in di.GetDevices())
                {
                    if (ins.Type == DeviceType.Keyboard)
                    {
                        InputSettingsShortcuts shortcuts = new InputSettingsShortcuts();
                        shortcuts.DeviceGuid = ins.InstanceGuid.ToString();
                        shortcuts.KeyShutDown = "F1";
                        shortcuts.KeyTogglePause = "F2";
                        shortcuts.KeySoftReset = "F3";
                        shortcuts.KeyHardReset = "F4";
                        shortcuts.KeyTakeSnapshot = "F5";
                        shortcuts.KeySaveState = "F6";
                        shortcuts.KeySaveStateBrowser = "F7";
                        shortcuts.KeySaveStateAs = "F8";
                        shortcuts.KeyLoadState = "F9";
                        shortcuts.KeyLoadStateBrowser = "F10";
                        shortcuts.KeyLoadStateAs = "F11";
                        shortcuts.KeyToggleFullscreen = "F12";
                        shortcuts.KeyTurbo = "Delete";
                        shortcuts.KeyVolumeUp = "NumberPadPlus";
                        shortcuts.KeyVolumeDown = "NumberPadMinus";
                        shortcuts.KeyToggleSoundEnable = "NumberPadStar";
                        shortcuts.KeyRecordSound = "NumberPadEnter";
                        shortcuts.KeyToggleKeepAspectRatio = "NumberPadSlash";
                        shortcuts.KeySetStateSlot0 = "D0";
                        shortcuts.KeySetStateSlot1 = "D1";
                        shortcuts.KeySetStateSlot2 = "D2";
                        shortcuts.KeySetStateSlot3 = "D3";
                        shortcuts.KeySetStateSlot4 = "D4";
                        shortcuts.KeySetStateSlot5 = "D5";
                        shortcuts.KeySetStateSlot6 = "D6";
                        shortcuts.KeySetStateSlot7 = "D7";
                        shortcuts.KeySetStateSlot8 = "D8";
                        shortcuts.KeySetStateSlot9 = "D9";
                        shortcuts.KeyToggleFPS = "NumberPad0";
                        shortcuts.KeyConnect4Players = "NumberPad4";
                        shortcuts.KeyConnectGameGenie = "PageUp";
                        shortcuts.KeyGameGenieCodes = "PageDown";
                        shortcuts.KeyToggleUseSoundMixer = "NumberPad8";
                        this.ShortcutsDevices.Add(shortcuts);
                        this.ShortcutsDeviceGuid = shortcuts.DeviceGuid;
                        this.ShortcutsAutoSwitchBackToKeyboard = true;
                        Console.WriteLine("Input: error found for shortcuts, it has been reset to keyboard.");
                        break;
                    }
                }
            }
        }
        public void Save()
        {
            FileStream str = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            if (str != null)
            {
                try
                {
                    BinaryFormatter formm = new BinaryFormatter();
                    formm.Serialize(str, Program.ControlSettings);
                    str.Flush();
                    str.Close();
                    str.Dispose();
                    Tracer.WriteInformation("Controls settings file saved successfully.");
                }
                catch (Exception ex)
                {
                    str.Flush();
                    str.Close();
                    str.Dispose();
                    Tracer.WriteInformation("Controls settings file cannot be saved: " + ex.Message);
                }
            }
            else
            {
                Tracer.WriteInformation("Controls settings file cannot be saved: Unable to create file stream.");
            }
        }
        public void Load()
        {
            if (File.Exists(FilePath))
            {
                Tracer.WriteWarning("Control settings file found, loading...");
                FileStream str = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                if (str != null)
                {
                    try
                    {
                        BinaryFormatter formm = new BinaryFormatter();
                        Program.ControlSettings = (ControlMappingSettings)formm.Deserialize(str);
                        str.Flush();
                        str.Close();
                        str.Dispose();

                        Check();
                        Tracer.WriteInformation("Controls settings file loaded successfully.");
                    }
                    catch (Exception ex)
                    {
                        str.Flush();
                        str.Close();
                        str.Dispose();
                        Tracer.WriteInformation("Controls settings file cannot be loaded: " + ex.Message);
                    }
                }
                else
                {
                    Tracer.WriteInformation("Controls settings file cannot be loaded: Unable to create file stream.");
                }
            }
            else
            {
                Tracer.WriteWarning("Control settings file cannot be found, building default controls settings.");
                BuildDefaultControlSettings();
            }
        }
    }
}
