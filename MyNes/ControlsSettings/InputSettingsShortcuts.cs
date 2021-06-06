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
namespace MyNes
{
    [System.Serializable]
    public class InputSettingsShortcuts : IInputSettings
    {
        public string KeyShutDown { get; set; }
        public string KeyTogglePause { get; set; }
        public string KeySoftReset { get; set; }
        public string KeyHardReset { get; set; }
        public string KeyTakeSnapshot { get; set; }
        public string KeySaveState { get; set; }
        public string KeySaveStateBrowser { get; set; }
        public string KeySaveStateAs { get; set; }
        public string KeyLoadState { get; set; }
        public string KeyLoadStateBrowser { get; set; }
        public string KeyLoadStateAs { get; set; }
        public string KeyToggleFullscreen { get; set; }
        public string KeyTurbo { get; set; }
       
        public string KeyVolumeUp { get; set; }
        public string KeyVolumeDown { get; set; }
        public string KeyToggleSoundEnable { get; set; }
        public string KeyRecordSound { get; set; }
        public string KeyToggleKeepAspectRatio { get; set; }
        public string KeySetStateSlot0 { get; set; }
        public string KeySetStateSlot1 { get; set; }
        public string KeySetStateSlot2 { get; set; }
        public string KeySetStateSlot3 { get; set; }
        public string KeySetStateSlot4 { get; set; }
        public string KeySetStateSlot5 { get; set; }
        public string KeySetStateSlot6 { get; set; }
        public string KeySetStateSlot7 { get; set; }
        public string KeySetStateSlot8 { get; set; }
        public string KeySetStateSlot9 { get; set; }
        public string KeyToggleFPS { get; set; }
        public string KeyConnect4Players { get; set; }
        public string KeyConnectGameGenie { get; set; }
        public string KeyGameGenieCodes { get; set; }
        public string KeyToggleUseSoundMixer { get; set; }
    }
}
