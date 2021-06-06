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
using System;
using System.Windows.Forms;

namespace MyNes
{
    public partial class FormGettingStarted : Form
    {
        public FormGettingStarted()
        {
            InitializeComponent();

            for (int i = 0; i < MyNesMain.VideoProviders.Count; i++)
            {
                comboBox_video_renderer.Items.Add(MyNesMain.VideoProviders[i].Name);
                if (MyNesMain.VideoProviders[i].ID == MyNesMain.RendererSettings.Video_ProviderID)
                    comboBox_video_renderer.SelectedIndex = i;
            }
            for (int i = 0; i < MyNesMain.AudioProviders.Count; i++)
            {
                comboBox_audio.Items.Add(MyNesMain.AudioProviders[i].Name);
                if (MyNesMain.AudioProviders[i].ID == MyNesMain.RendererSettings.Audio_ProviderID)
                    comboBox_audio.SelectedIndex = i;
            }

            checkBox_enable_sound.Checked = MyNesMain.RendererSettings.Audio_SoundEnabled;
            checkBox_open_launcher.Checked = Program.Settings.LauncherShowAyAppStart;
            checkBox_start_fullscreen.Checked = Program.Settings.Win_StartInFullscreen;
            checkBox_shutdownOnEscape.Checked = Program.Settings.ShutdowOnEscapePress;
            checkBox_auto_stretch.Checked = MyNesMain.RendererSettings.Vid_AutoStretch;

            if (comboBox_video_renderer.SelectedIndex == -1)
            {
                for (int i = 0; i < MyNesMain.VideoProviders.Count; i++)
                {
                    if (MyNesMain.VideoProviders[i].ID == "sdl2.video")
                    { comboBox_video_renderer.SelectedIndex = i; break; }
                }
            }
            if (comboBox_audio.SelectedIndex == -1)
            {
                for (int i = 0; i < MyNesMain.AudioProviders.Count; i++)
                {
                    if (MyNesMain.AudioProviders[i].ID == "slimdx.directsound")
                    { comboBox_audio.SelectedIndex = i; break; }
                }
            }

            richTextBox1.Text = Properties.Resources.GettingStartedDesc;
            richTextBox2.Text = Properties.Resources.GettingStartedNotes;

            radioButton_point.Checked = MyNesMain.RendererSettings.Vid_Filter == 0;
            radioButton2.Checked = MyNesMain.RendererSettings.Vid_Filter == 1;
            radioButton_audio_use_default_mixer.Checked = MyNesMain.RendererSettings.Audio_UseDefaultMixer;
            radioButton_audio_use_mynes_mixer.Checked = !MyNesMain.RendererSettings.Audio_UseDefaultMixer;

            radioButton_use_upscale.Checked = MyNesMain.RendererSettings.Vid_Res_Upscale;
            radioButton_no_upscale.Checked = !MyNesMain.RendererSettings.Vid_Res_Upscale;

            switch (MyNesMain.RendererSettings.Audio_Frequency)
            {
                case 11025: comboBox_audio_freq.SelectedIndex = 0; break;
                case 22050: comboBox_audio_freq.SelectedIndex = 1; break;
                case 44100: comboBox_audio_freq.SelectedIndex = 2; break;
                case 48000: comboBox_audio_freq.SelectedIndex = 3; break;
            }

            for (int i = 0; i < 10; i++)
            {
                comboBox_stretch_size.Items.Add(string.Format("X {0} ( {1} x {2} )", i + 1, 640 * (i + 1), 480 * (i + 1)));
            }
            int index = MyNesMain.RendererSettings.Vid_StretchMultiply - 1;
            if (index >= 0 && index < comboBox_stretch_size.Items.Count)
                comboBox_stretch_size.SelectedIndex = index;
        }
        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_SoundEnabled = checkBox_enable_sound.Checked;
            Program.Settings.LauncherShowAyAppStart = checkBox_open_launcher.Checked;
            Program.Settings.Win_StartInFullscreen = checkBox_start_fullscreen.Checked;
            Program.Settings.ShutdowOnEscapePress = checkBox_shutdownOnEscape.Checked;

            if (comboBox_audio.SelectedIndex >= 0)
                MyNesMain.RendererSettings.Audio_ProviderID = MyNesMain.AudioProviders[comboBox_audio.SelectedIndex].ID;
            if (comboBox_video_renderer.SelectedIndex >= 0)
                MyNesMain.RendererSettings.Video_ProviderID = MyNesMain.VideoProviders[comboBox_video_renderer.SelectedIndex].ID;

            MyNesMain.RendererSettings.Vid_Filter = radioButton_point.Checked ? 0 : 1;
            MyNesMain.RendererSettings.Audio_UseDefaultMixer = radioButton_audio_use_default_mixer.Checked;
            Program.Settings.ShowGettingStarted = !checkBox_never_show_me.Checked;

            MyNesMain.RendererSettings.Vid_Res_Upscale = radioButton_use_upscale.Checked;

            switch (comboBox_audio_freq.SelectedIndex)
            {
                case 0: MyNesMain.RendererSettings.Audio_Frequency = 11025; break;
                case 1: MyNesMain.RendererSettings.Audio_Frequency = 22050; break;
                case 2: MyNesMain.RendererSettings.Audio_Frequency = 44100; break;
                case 3: MyNesMain.RendererSettings.Audio_Frequency = 48000; break;
            }
            MyNesMain.RendererSettings.Vid_StretchMultiply = comboBox_stretch_size.SelectedIndex + 1;
            MyNesMain.RendererSettings.Vid_AutoStretch = checkBox_auto_stretch.Checked;
            Close();
        }
        // SlimDX
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < MyNesMain.VideoProviders.Count; i++)
            {
                if (MyNesMain.VideoProviders[i].ID == "slimdx.video")
                { comboBox_video_renderer.SelectedIndex = i; break; }
            }

            for (int i = 0; i < MyNesMain.AudioProviders.Count; i++)
            {
                if (MyNesMain.AudioProviders[i].ID == "slimdx.directsound")
                { comboBox_audio.SelectedIndex = i; break; }
            }
        }
        // SDL2
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < MyNesMain.VideoProviders.Count; i++)
            {
                if (MyNesMain.VideoProviders[i].ID == "sdl2.video")
                { comboBox_video_renderer.SelectedIndex = i; break; }
            }
            for (int i = 0; i < MyNesMain.AudioProviders.Count; i++)
            {
                if (MyNesMain.AudioProviders[i].ID == "sdl2.audio")
                { comboBox_audio.SelectedIndex = i; break; }
            }
        }
        // MIX
        private void LinkLabel3_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < MyNesMain.VideoProviders.Count; i++)
            {
                if (MyNesMain.VideoProviders[i].ID == "sdl2.video")
                { comboBox_video_renderer.SelectedIndex = i; break; }
            }
            for (int i = 0; i < MyNesMain.AudioProviders.Count; i++)
            {
                if (MyNesMain.AudioProviders[i].ID == "slimdx.directsound")
                { comboBox_audio.SelectedIndex = i; break; }
            }
        }
    }
}
