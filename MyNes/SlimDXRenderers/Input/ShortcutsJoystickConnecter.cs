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
using SlimDX.DirectInput;

namespace MyNes
{
    class ShortcutsJoystickConnecter : IShortcutsHandler
    {
        public ShortcutsJoystickConnecter(IntPtr handle, string guid, InputSettingsShortcuts settings)
        {
            DirectInput di = new DirectInput();
            joystick = new Joystick(di, Guid.Parse(guid));
            joystick.SetCooperativeLevel(handle, CooperativeLevel.Nonexclusive | CooperativeLevel.Foreground);

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
        private Joystick joystick;
        private JoystickState joystickState;
        private int KeyShutDown = 0;
        private int KeyTogglePause = 0;
        private int KeySoftReset = 0;
        private int KeyHardReset = 0;
        private int KeyTakeSnapshot = 0;
        private int KeySaveState = 0;
        private int KeySaveStateBrowser = 0;
        private int KeySaveStateAs = 0;
        private int KeyLoadState = 0;
        private int KeyLoadStateBrowser = 0;
        private int KeyLoadStateAs = 0;
        private int KeyToggleFullscreen = 0;
        private int KeyTurbo = 0;

        private int KeyVolumeUp = 0;
        private int KeyVolumeDown = 0;
        private int KeyToggleSoundEnable = 0;
        private int KeyRecordSound = 0;
        private int KeyToggleKeepAspectRatio = 0;
        private int KeySetStateSlot0 = 0;
        private int KeySetStateSlot1 = 0;
        private int KeySetStateSlot2 = 0;
        private int KeySetStateSlot3 = 0;
        private int KeySetStateSlot4 = 0;
        private int KeySetStateSlot5 = 0;
        private int KeySetStateSlot6 = 0;
        private int KeySetStateSlot7 = 0;
        private int KeySetStateSlot8 = 0;
        private int KeySetStateSlot9 = 0;
        private int KeyToggleFPS = 0;
        private int KeyConnect4Players = 0;
        private int KeyConnectGameGenie = 0;
        private int KeyGameGenieCodes = 0;
        private int KeyToggleEnableSoundFilters = 0;
        private int KeyToggleUseSoundMixer = 0;

        private int timerCounter = 0;
        const int timerReload = 15;
        private bool IsPressed(int keyNumber)
        {
            if (keyNumber < 0)
            {
                if (keyNumber == -1)
                    return joystickState.X > 0xC000;
                else if (keyNumber == -2)
                    return joystickState.X < 0x4000;
                else if (keyNumber == -3)
                    return joystickState.Y > 0xC000;
                else if (keyNumber == -4)
                    return joystickState.Y < 0x4000;
            }
            else
            {
                return joystickState.IsPressed(keyNumber);
            }
            return false;
        }
        private int ParseKey(string key)
        {
            if (key == "X+")
                return -1;
            else if (key == "X-")
                return -2;
            else if (key == "Y+")
                return -3;
            else if (key == "Y-")
                return -4;
            else
                return Convert.ToInt32(key);
        }

        public void Update()
        {
            if (timerCounter > 0)
            { timerCounter--; return; }
            if (joystick.Acquire().IsSuccess)
            {
                joystickState = joystick.GetCurrentState();

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
