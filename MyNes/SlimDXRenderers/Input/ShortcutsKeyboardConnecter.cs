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
    class ShortcutsKeyboardConnecter : IShortcutsHandler
    {
        public ShortcutsKeyboardConnecter(IntPtr handle, InputSettingsShortcuts settings)
        {
            DirectInput di = new DirectInput();
            keyboard = new Keyboard(di);
            keyboard.SetCooperativeLevel(handle, CooperativeLevel.Nonexclusive | CooperativeLevel.Foreground);

            if (settings.KeyShutDown != "")
                KeyShutDown = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyShutDown);
            if (settings.KeyTogglePause != "")
                KeyTogglePause = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyTogglePause);
            if (settings.KeySoftReset != "")
                KeySoftReset = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySoftReset);
            if (settings.KeyHardReset != "")
                KeyHardReset = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyHardReset);
            if (settings.KeyTakeSnapshot != "")
                KeyTakeSnapshot = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyTakeSnapshot);
            if (settings.KeySaveState != "")
                KeySaveState = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySaveState);
            if (settings.KeySaveStateBrowser != "")
                KeySaveStateBrowser = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySaveStateBrowser);
            if (settings.KeySaveStateAs != "")
                KeySaveStateAs = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySaveStateAs);
            if (settings.KeyLoadState != "")
                KeyLoadState = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyLoadState);
            if (settings.KeyLoadStateBrowser != "")
                KeyLoadStateBrowser = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyLoadStateBrowser);
            if (settings.KeyLoadStateAs != "")
                KeyLoadStateAs = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyLoadStateAs);
            if (settings.KeyToggleFullscreen != "")
                KeyToggleFullscreen = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyToggleFullscreen);
            if (settings.KeyTurbo != "")
                KeyTurbo = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyTurbo);

            if (settings.KeyVolumeUp != "")
                KeyVolumeUp = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyVolumeUp);
            if (settings.KeyVolumeDown != "")
                KeyVolumeDown = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyVolumeDown);
            if (settings.KeyToggleSoundEnable != "")
                KeyToggleSoundEnable = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyToggleSoundEnable);
            if (settings.KeyRecordSound != "")
                KeyRecordSound = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyRecordSound);
            if (settings.KeyToggleKeepAspectRatio != "")
                KeyToggleKeepAspectRatio = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyToggleKeepAspectRatio);
            if (settings.KeySetStateSlot0 != "")
                KeySetStateSlot0 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySetStateSlot0);
            if (settings.KeySetStateSlot1 != "")
                KeySetStateSlot1 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySetStateSlot1);
            if (settings.KeySetStateSlot2 != "")
                KeySetStateSlot2 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySetStateSlot2);
            if (settings.KeySetStateSlot3 != "")
                KeySetStateSlot3 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySetStateSlot3);
            if (settings.KeySetStateSlot4 != "")
                KeySetStateSlot4 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySetStateSlot4);
            if (settings.KeySetStateSlot5 != "")
                KeySetStateSlot5 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySetStateSlot5);
            if (settings.KeySetStateSlot6 != "")
                KeySetStateSlot6 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySetStateSlot6);
            if (settings.KeySetStateSlot7 != "")
                KeySetStateSlot7 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySetStateSlot7);
            if (settings.KeySetStateSlot8 != "")
                KeySetStateSlot8 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySetStateSlot8);
            if (settings.KeySetStateSlot9 != "")
                KeySetStateSlot9 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeySetStateSlot9);
            if (settings.KeyToggleFPS != "")
                KeyToggleFPS = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyToggleFPS);
            if (settings.KeyConnect4Players != "")
                KeyConnect4Players = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyConnect4Players);
            if (settings.KeyConnectGameGenie != "")
                KeyConnectGameGenie = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyConnectGameGenie);
            if (settings.KeyGameGenieCodes != "")
                KeyGameGenieCodes = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyGameGenieCodes);
            if (settings.KeyToggleUseSoundMixer != "")
                KeyToggleUseSoundMixer = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.KeyToggleUseSoundMixer);
        }
        private Key KeyShutDown = Key.F1;
        private Key KeyTogglePause = Key.F2;
        private Key KeySoftReset = Key.F3;
        private Key KeyHardReset = Key.F4;
        private Key KeyTakeSnapshot = Key.F5;
        private Key KeySaveState = Key.F6;
        private Key KeySaveStateBrowser = Key.F7;
        private Key KeySaveStateAs = Key.F8;
        private Key KeyLoadState = Key.F9;
        private Key KeyLoadStateBrowser = Key.F10;
        private Key KeyLoadStateAs = Key.F11;
        private Key KeyToggleFullscreen = Key.F12;
        private Key KeyTurbo = Key.Delete;

        private Key KeyVolumeUp = Key.NumberPadPlus;
        private Key KeyVolumeDown = Key.NumberPadMinus;
        private Key KeyToggleSoundEnable = Key.NumberPadStar;
        private Key KeyRecordSound = Key.NumberPadEnter;
        private Key KeyToggleKeepAspectRatio = Key.NumberPadSlash;
        private Key KeySetStateSlot0 = Key.D0;
        private Key KeySetStateSlot1 = Key.D1;
        private Key KeySetStateSlot2 = Key.D2;
        private Key KeySetStateSlot3 = Key.D3;
        private Key KeySetStateSlot4 = Key.D4;
        private Key KeySetStateSlot5 = Key.D5;
        private Key KeySetStateSlot6 = Key.D6;
        private Key KeySetStateSlot7 = Key.D7;
        private Key KeySetStateSlot8 = Key.D8;
        private Key KeySetStateSlot9 = Key.D9;
        private Key KeyToggleFPS = Key.NumberPad0;
        private Key KeyConnect4Players = Key.NumberPad4;
        private Key KeyConnectGameGenie = Key.PageUp;
        private Key KeyGameGenieCodes = Key.PageDown;
        private Key KeyToggleUseSoundMixer = Key.NumberPad8;

        private Keyboard keyboard;
        private KeyboardState state;
        private int timerCounter = 0;
        const int timerReload = 15;

        public void Update()
        {
            if (timerCounter > 0)
            { timerCounter--; return; }
            if (keyboard.Acquire().IsSuccess)
            {
                state = keyboard.GetCurrentState();

                // Shutdown
                if (state.IsPressed(KeyShutDown))
                {
                    timerCounter = timerReload;
                    if (NesEmu.ON)
                        NesEmu.ShutDown();
                    Program.FormMain.ApplyWindowTitle();
                    return;
                }
                // Toggle pause
                if (state.IsPressed(KeyTogglePause))
                {
                    timerCounter = timerReload;
                    NesEmu.PAUSED = !NesEmu.PAUSED;
                    return;
                }
                // Soft reset
                if (state.IsPressed(KeySoftReset))
                {
                    timerCounter = timerReload;
                    NesEmu.SoftReset();
                    return;
                }
                // Hard reset
                if (state.IsPressed(KeyHardReset))
                {
                    timerCounter = timerReload;
                    NesEmu.HardReset();
                    return;
                }
                // Take snapshot
                if (state.IsPressed(KeyTakeSnapshot))
                {
                    timerCounter = timerReload;
                    if (MyNesMain.VideoProvider != null)
                        MyNesMain.VideoProvider.TakeSnapshot();
                    return;
                }
                // save state
                if (state.IsPressed(KeySaveState))
                {
                    timerCounter = timerReload;
                    if (NesEmu.ON)
                        NesEmu.SaveState();
                    return;
                }
                // Save State Browser
                if (state.IsPressed(KeySaveStateBrowser))
                {
                    timerCounter = timerReload;
                    MyNesMain.AudioProvider.TogglePause(true);
                    Program.FormMain.ShowSaveStateBrowser();
                    return;
                }
                // Save State As
                if (state.IsPressed(KeySaveStateAs))
                {
                    timerCounter = timerReload;
                    MyNesMain.AudioProvider.TogglePause(true);
                    Program.FormMain.SaveStateAs();
                    return;
                }
                // load state
                if (state.IsPressed(KeyLoadState))
                {
                    timerCounter = timerReload;
                    if (NesEmu.ON)
                        NesEmu.LoadState();
                    return;
                }
                // load state browser
                if (state.IsPressed(KeyLoadStateBrowser))
                {
                    timerCounter = timerReload;
                    MyNesMain.AudioProvider.TogglePause(true);
                    Program.FormMain.ShowLoadStateBrowser();
                    return;
                }
                // Load State As
                if (state.IsPressed(KeyLoadStateAs))
                {
                    timerCounter = timerReload;
                    MyNesMain.AudioProvider.TogglePause(true);
                    Program.FormMain.LoadStateAs();
                    return;
                }
                // Toggle fullscreen
                if (state.IsPressed(KeyToggleFullscreen))
                {
                    timerCounter = timerReload;
                    MyNesMain.AudioProvider.TogglePause(true);
                    Program.FormMain.ToggleFullscreen();
                    return;
                }
                // Toggle turbo
                if (state.IsPressed(KeyTurbo))
                {
                    timerCounter = timerReload;
                    NesEmu.FrameLimiterEnabled = !NesEmu.FrameLimiterEnabled;
                    return;
                }
                // Toggle sound enable
                if (state.IsPressed(KeyToggleSoundEnable))
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
                if (state.IsPressed(KeyRecordSound))
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
                if (state.IsPressed(KeyToggleKeepAspectRatio))
                {
                    timerCounter = timerReload;
                    MyNesMain.RendererSettings.Vid_KeepAspectRatio = !MyNesMain.RendererSettings.Vid_KeepAspectRatio;
                    MyNesMain.VideoProvider.ToggleAspectRatio(MyNesMain.RendererSettings.Vid_KeepAspectRatio);
                    return;
                }
                // Toggle FPS
                if (state.IsPressed(KeyToggleFPS))
                {
                    timerCounter = timerReload;
                    MyNesMain.RendererSettings.Vid_ShowFPS = !MyNesMain.RendererSettings.Vid_ShowFPS;
                    MyNesMain.VideoProvider.ToggleFPS(MyNesMain.RendererSettings.Vid_ShowFPS);
                    return;
                }
                // Connect 4 Players
                if (state.IsPressed(KeyConnect4Players))
                {
                    timerCounter = timerReload;
                    NesEmu.IsFourPlayers = !NesEmu.IsFourPlayers;

                    if (MyNesMain.VideoProvider != null)
                        MyNesMain.VideoProvider.WriteInfoNotification(NesEmu.IsFourPlayers ? Properties.Resources.Status42 : Properties.Resources.Status43, false);
                    return;
                }
                // Connect Game Genie
                if (state.IsPressed(KeyConnectGameGenie))
                {
                    timerCounter = timerReload;
                    NesEmu.IsGameGenieActive = !NesEmu.IsGameGenieActive;
                    MyNesMain.VideoProvider.WriteInfoNotification(NesEmu.IsGameGenieActive ? Properties.Resources.Status18 : Properties.Resources.Status19, false);
                    return;
                }
                // Edit Game Genie Codes
                if (state.IsPressed(KeyGameGenieCodes))
                {
                    timerCounter = timerReload;
                    Program.FormMain.EditGameGenieCodes();
                    return;
                }
                // Toggle Use Sound Mixer
                if (state.IsPressed(KeyToggleUseSoundMixer))
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
                if (state.IsPressed(KeySetStateSlot0))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 0;

                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (state.IsPressed(KeySetStateSlot1))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 1;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (state.IsPressed(KeySetStateSlot2))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 2;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (state.IsPressed(KeySetStateSlot3))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 3;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (state.IsPressed(KeySetStateSlot4))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 4;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (state.IsPressed(KeySetStateSlot5))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 5;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (state.IsPressed(KeySetStateSlot6))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 6;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (state.IsPressed(KeySetStateSlot7))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 7;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (state.IsPressed(KeySetStateSlot8))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 8;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (state.IsPressed(KeySetStateSlot9))
                {
                    timerCounter = timerReload;
                    StateHandler.Slot = 9;
                    Tracer.WriteLine(Properties.Resources.Status54 + " " + StateHandler.Slot);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status54 + " " + StateHandler.Slot, false);
                    return;
                }
                if (state.IsPressed(KeyVolumeUp))
                {
                    timerCounter = timerReload;
                    MyNesMain.RendererSettings.Audio_Volume++;
                    if (MyNesMain.RendererSettings.Audio_Volume > 100)
                        MyNesMain.RendererSettings.Audio_Volume = 100;
                    MyNesMain.AudioProvider.SetVolume(MyNesMain.RendererSettings.Audio_Volume);
                    MyNesMain.VideoProvider.WriteInfoNotification(Properties.Resources.Status55 + " " + MyNesMain.RendererSettings.Audio_Volume, true);
                    return;
                }
                if (state.IsPressed(KeyVolumeDown))
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
