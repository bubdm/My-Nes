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
    class ShortcutsXControllerConnecter : IShortcutsHandler
    {
        public ShortcutsXControllerConnecter(string guid, InputSettingsShortcuts settings)
        {
            switch (guid)
            {
                case "x-controller-1": x_controller = new Controller(UserIndex.One); break;
                case "x-controller-2": x_controller = new Controller(UserIndex.Two); break;
                case "x-controller-3": x_controller = new Controller(UserIndex.Three); break;
                case "x-controller-4": x_controller = new Controller(UserIndex.Four); break;
            }
            if (settings.KeyShutDown != "")
                KeyShutDown = ParseKey(settings.KeyShutDown);
            if (settings.KeyTogglePause != "")
                KeyTogglePause = ParseKey(settings.KeyTogglePause);
            if (settings.KeySoftReset != "")
                KeySoftReset = ParseKey(settings.KeySoftReset);
            if (settings.KeyHardReset != "")
                KeyHardReset = ParseKey(settings.KeyHardReset);
            if (settings.KeyTakeSnapshot != "")
                KeyTakeSnapshot = ParseKey(settings.KeyTakeSnapshot);
            if (settings.KeySaveState != "")
                KeySaveState = ParseKey(settings.KeySaveState);
            if (settings.KeySaveStateBrowser != "")
                KeySaveStateBrowser = ParseKey(settings.KeySaveStateBrowser);
            if (settings.KeySaveStateAs != "")
                KeySaveStateAs = ParseKey(settings.KeySaveStateAs);
            if (settings.KeyLoadState != "")
                KeyLoadState = ParseKey(settings.KeyLoadState);
            if (settings.KeyLoadStateBrowser != "")
                KeyLoadStateBrowser = ParseKey(settings.KeyLoadStateBrowser);
            if (settings.KeyLoadStateAs != "")
                KeyLoadStateAs = ParseKey(settings.KeyLoadStateAs);
            if (settings.KeyToggleFullscreen != "")
                KeyToggleFullscreen = ParseKey(settings.KeyToggleFullscreen);
            if (settings.KeyTurbo != "")
                KeyTurbo = ParseKey(settings.KeyTurbo);
            if (settings.KeyVolumeUp != "")
                KeyVolumeUp = ParseKey(settings.KeyVolumeUp);
            if (settings.KeyVolumeDown != "")
                KeyVolumeDown = ParseKey(settings.KeyVolumeDown);
            if (settings.KeyToggleSoundEnable != "")
                KeyToggleSoundEnable = ParseKey(settings.KeyToggleSoundEnable);
            if (settings.KeyRecordSound != "")
                KeyRecordSound = ParseKey(settings.KeyRecordSound);
            if (settings.KeyToggleKeepAspectRatio != "")
                KeyToggleKeepAspectRatio = ParseKey(settings.KeyToggleKeepAspectRatio);
            if (settings.KeySetStateSlot0 != "")
                KeySetStateSlot0 = ParseKey(settings.KeySetStateSlot0);
            if (settings.KeySetStateSlot1 != "")
                KeySetStateSlot1 = ParseKey(settings.KeySetStateSlot1);
            if (settings.KeySetStateSlot2 != "")
                KeySetStateSlot2 = ParseKey(settings.KeySetStateSlot2);
            if (settings.KeySetStateSlot3 != "")
                KeySetStateSlot3 = ParseKey(settings.KeySetStateSlot3);
            if (settings.KeySetStateSlot4 != "")
                KeySetStateSlot4 = ParseKey(settings.KeySetStateSlot4);
            if (settings.KeySetStateSlot5 != "")
                KeySetStateSlot5 = ParseKey(settings.KeySetStateSlot5);
            if (settings.KeySetStateSlot6 != "")
                KeySetStateSlot6 = ParseKey(settings.KeySetStateSlot6);
            if (settings.KeySetStateSlot7 != "")
                KeySetStateSlot7 = ParseKey(settings.KeySetStateSlot7);
            if (settings.KeySetStateSlot8 != "")
                KeySetStateSlot8 = ParseKey(settings.KeySetStateSlot8);
            if (settings.KeySetStateSlot9 != "")
                KeySetStateSlot9 = ParseKey(settings.KeySetStateSlot9);
            if (settings.KeyToggleFPS != "")
                KeyToggleFPS = ParseKey(settings.KeyToggleFPS);
            if (settings.KeyConnect4Players != "")
                KeyConnect4Players = ParseKey(settings.KeyConnect4Players);
            if (settings.KeyConnectGameGenie != "")
                KeyConnectGameGenie = ParseKey(settings.KeyConnectGameGenie);
            if (settings.KeyGameGenieCodes != "")
                KeyGameGenieCodes = ParseKey(settings.KeyGameGenieCodes);
            if (settings.KeyToggleUseSoundMixer != "")
                KeyToggleUseSoundMixer = ParseKey(settings.KeyToggleUseSoundMixer);
        }
        private Controller x_controller;
        private GamepadButtonFlags x_current_buttons = GamepadButtonFlags.None;
        private GamepadButtonFlags KeyShutDown = 0;
        private GamepadButtonFlags KeyTogglePause = 0;
        private GamepadButtonFlags KeySoftReset = 0;
        private GamepadButtonFlags KeyHardReset = 0;
        private GamepadButtonFlags KeyTakeSnapshot = 0;
        private GamepadButtonFlags KeySaveState = 0;
        private GamepadButtonFlags KeySaveStateBrowser = 0;
        private GamepadButtonFlags KeySaveStateAs = 0;
        private GamepadButtonFlags KeyLoadState = 0;
        private GamepadButtonFlags KeyLoadStateBrowser = 0;
        private GamepadButtonFlags KeyLoadStateAs = 0;
        private GamepadButtonFlags KeyToggleFullscreen = 0;
        private GamepadButtonFlags KeyTurbo = 0;

        private GamepadButtonFlags KeyVolumeUp = 0;
        private GamepadButtonFlags KeyVolumeDown = 0;
        private GamepadButtonFlags KeyToggleSoundEnable = 0;
        private GamepadButtonFlags KeyRecordSound = 0;
        private GamepadButtonFlags KeyToggleKeepAspectRatio = 0;
        private GamepadButtonFlags KeySetStateSlot0 = 0;
        private GamepadButtonFlags KeySetStateSlot1 = 0;
        private GamepadButtonFlags KeySetStateSlot2 = 0;
        private GamepadButtonFlags KeySetStateSlot3 = 0;
        private GamepadButtonFlags KeySetStateSlot4 = 0;
        private GamepadButtonFlags KeySetStateSlot5 = 0;
        private GamepadButtonFlags KeySetStateSlot6 = 0;
        private GamepadButtonFlags KeySetStateSlot7 = 0;
        private GamepadButtonFlags KeySetStateSlot8 = 0;
        private GamepadButtonFlags KeySetStateSlot9 = 0;
        private GamepadButtonFlags KeyToggleFPS = 0;
        private GamepadButtonFlags KeyConnect4Players = 0;
        private GamepadButtonFlags KeyConnectGameGenie = 0;
        private GamepadButtonFlags KeyGameGenieCodes = 0;
        private GamepadButtonFlags KeyToggleUseSoundMixer = 0;

        private int timerCounter = 0;
        const int timerReload = 15;

        private bool IsPressed(GamepadButtonFlags key)
        {
            if (x_current_buttons == GamepadButtonFlags.None)
                return false;
            if (key == GamepadButtonFlags.None)
                return false;

            return (x_current_buttons & key) == key;
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
        public void Update()
        {
            if (timerCounter > 0)
            { timerCounter--; return; }
            if (x_controller.GetState().Gamepad.Buttons != GamepadButtonFlags.None)
            {
                x_current_buttons = x_controller.GetState().Gamepad.Buttons;
                // Shutdown
                if (IsPressed(KeyShutDown))
                {
                    timerCounter = timerReload;
                    if (NesEmu.ON)
                        NesEmu.ShutDown();
                    Program.FormMain.ApplyWindowTitle();
                    return;
                }
                // Toggle pause
                if (IsPressed(KeyTogglePause))
                {
                    timerCounter = timerReload;
                    NesEmu.PAUSED = !NesEmu.PAUSED;
                    return;
                }
                // Soft reset
                if (IsPressed(KeySoftReset))
                {
                    timerCounter = timerReload;
                    NesEmu.SoftReset();
                    return;
                }
                // Hard reset
                if (IsPressed(KeyHardReset))
                {
                    timerCounter = timerReload;
                    NesEmu.HardReset();
                    return;
                }
                // Take snapshot
                if (IsPressed(KeyTakeSnapshot))
                {
                    timerCounter = timerReload;
                    if (MyNesMain.VideoProvider != null)
                        MyNesMain.VideoProvider.TakeSnapshot();
                    return;
                }
                // save state
                if (IsPressed(KeySaveState))
                {
                    timerCounter = timerReload;
                    if (NesEmu.ON)
                        NesEmu.SaveState();
                    return;
                }
                // Save State Browser
                if (IsPressed(KeySaveStateBrowser))
                {
                    timerCounter = timerReload;
                    MyNesMain.AudioProvider.TogglePause(true);
                    Program.FormMain.ShowSaveStateBrowser();
                    return;
                }
                // Save State As
                if (IsPressed(KeySaveStateAs))
                {
                    timerCounter = timerReload;
                    MyNesMain.AudioProvider.TogglePause(true);
                    Program.FormMain.SaveStateAs();
                    return;
                }
                // load state
                if (IsPressed(KeyLoadState))
                {
                    timerCounter = timerReload;
                    if (NesEmu.ON)
                        NesEmu.LoadState();
                    return;
                }
                // load state browser
                if (IsPressed(KeyLoadStateBrowser))
                {
                    timerCounter = timerReload;
                    MyNesMain.AudioProvider.TogglePause(true);
                    Program.FormMain.ShowLoadStateBrowser();
                    return;
                }
                // Load State As
                if (IsPressed(KeyLoadStateAs))
                {
                    timerCounter = timerReload;
                    MyNesMain.AudioProvider.TogglePause(true);
                    Program.FormMain.LoadStateAs();
                    return;
                }
                // Toggle fullscreen
                if (IsPressed(KeyToggleFullscreen))
                {
                    timerCounter = timerReload;
                    MyNesMain.AudioProvider.TogglePause(true);
                    Program.FormMain.ToggleFullscreen();
                    return;
                }
                // Toggle turbo
                if (IsPressed(KeyTurbo))
                {
                    timerCounter = timerReload;
                    NesEmu.FrameLimiterEnabled = !NesEmu.FrameLimiterEnabled;
                    return;
                }
                // Toggle sound enable
                if (IsPressed(KeyToggleSoundEnable))
                {
                    timerCounter = timerReload;
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
                    return;
                }
                // Record sound
                if (IsPressed(KeyRecordSound))
                {
                    timerCounter = timerReload;
                    if (NesEmu.ON)
                    {
                        if (MyNesMain.WaveRecorder.IsRecording)
                            MyNesMain.WaveRecorder.Stop();
                        else
                            MyNesMain.RecordWave();
                    }
                    return;
                }
                // Toggle keep aspect ratio
                if (IsPressed(KeyToggleKeepAspectRatio))
                {
                    timerCounter = timerReload;
                    MyNesMain.RendererSettings.Vid_KeepAspectRatio = !MyNesMain.RendererSettings.Vid_KeepAspectRatio;
                    MyNesMain.VideoProvider.ToggleAspectRatio(MyNesMain.RendererSettings.Vid_KeepAspectRatio);
                    return;
                }
                // Toggle FPS
                if (IsPressed(KeyToggleFPS))
                {
                    timerCounter = timerReload;
                    MyNesMain.RendererSettings.Vid_ShowFPS = !MyNesMain.RendererSettings.Vid_ShowFPS;
                    MyNesMain.VideoProvider.ToggleFPS(MyNesMain.RendererSettings.Vid_ShowFPS);
                    return;
                }
                // Connect 4 Players
                if (IsPressed(KeyConnect4Players))
                {
                    timerCounter = timerReload;
                    NesEmu.IsFourPlayers = !NesEmu.IsFourPlayers;

                    if (MyNesMain.VideoProvider != null)
                        MyNesMain.VideoProvider.WriteInfoNotification(NesEmu.IsFourPlayers ? Properties.Resources.Status42 : Properties.Resources.Status43, false);
                    return;
                }
                // Connect Game Genie
                if (IsPressed(KeyConnectGameGenie))
                {
                    timerCounter = timerReload;
                    NesEmu.IsGameGenieActive = !NesEmu.IsGameGenieActive;
                    MyNesMain.VideoProvider.WriteInfoNotification(NesEmu.IsGameGenieActive ? Properties.Resources.Status18 : Properties.Resources.Status19, false);
                    return;
                }
                // Edit Game Genie Codes
                if (IsPressed(KeyGameGenieCodes))
                {
                    timerCounter = timerReload;
                    Program.FormMain.EditGameGenieCodes();
                    return;
                }
                // Toggle Use Sound Mixer
                if (IsPressed(KeyToggleUseSoundMixer))
                {
                    timerCounter = timerReload;
                    MyNesMain.RendererSettings.Audio_UseDefaultMixer = !MyNesMain.RendererSettings.Audio_UseDefaultMixer;
                    MyNesMain.VideoProvider.WriteInfoNotification(MyNesMain.RendererSettings.Audio_UseDefaultMixer ? Properties.Resources.Status53 : Properties.Resources.Status52, false);
                    if (NesEmu.ON)
                        NesEmu.PAUSED = true;

                    System.Threading.Thread.Sleep(200);
                    NesEmu.InitializeDACTables(true);

                    if (NesEmu.ON)
                        NesEmu.PAUSED = false;
                    return;
                }
                if (IsPressed(KeySetStateSlot0))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 0;

                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (IsPressed(KeySetStateSlot1))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 1;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (IsPressed(KeySetStateSlot2))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 2;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (IsPressed(KeySetStateSlot3))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 3;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (IsPressed(KeySetStateSlot4))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 4;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (IsPressed(KeySetStateSlot5))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 5;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (IsPressed(KeySetStateSlot6))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 6;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (IsPressed(KeySetStateSlot7))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 7;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (IsPressed(KeySetStateSlot8))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 8;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (IsPressed(KeySetStateSlot9))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 9;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (IsPressed(KeyVolumeUp))
                {
                    timerCounter = timerReload;
                    MyNesMain.RendererSettings.Audio_Volume++;
                    if (MyNesMain.RendererSettings.Audio_Volume > 100)
                        MyNesMain.RendererSettings.Audio_Volume = 100;
                    MyNesMain.AudioProvider.SetVolume(MyNesMain.RendererSettings.Audio_Volume);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status55 + " " + MyNesMain.RendererSettings.Audio_Volume, true);
                    return;
                }
                if (IsPressed(KeyVolumeDown))
                {
                    timerCounter = timerReload;
                    MyNesMain.RendererSettings.Audio_Volume--;
                    if (MyNesMain.RendererSettings.Audio_Volume < 0)
                        MyNesMain.RendererSettings.Audio_Volume = 0;
                    MyNesMain.AudioProvider.SetVolume(MyNesMain.RendererSettings.Audio_Volume);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status55 + " " + MyNesMain.RendererSettings.Audio_Volume, true);
                    return;
                }
            }
        }
    }
}
