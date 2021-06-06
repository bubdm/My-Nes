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
using System.IO;
using System.Windows.Forms;
using MyNes.Core;
using SDL2;

namespace MyNes
{
    public partial class FormSDL2Settings : Form
    {
        public FormSDL2Settings()
        {
            InitializeComponent();
            int c = SDL.SDL_GetNumAudioDrivers();
            for (int i = 0; i < c; i++)
            {
                string n = SDL.SDL_GetAudioDeviceName(i, 0);
                if (n != null)
                    comboBox1.Items.Add(n);
            }

            sdl_settings = new SDL2Settings(Path.Combine(Program.WorkingFolder, "sdlsettings.ini"));
            sdl_settings.LoadSettings();

            radioButton_opengl.Checked = sdl_settings.Video_Driver == "opengl";

            richTextBox1.Text = Properties.Resources.SDL2SettingsWarning;

            if (sdl_settings.Audio_Device_Index >= 0 && sdl_settings.Audio_Device_Index < comboBox1.Items.Count)
                comboBox1.SelectedIndex = sdl_settings.Audio_Device_Index;
        }
        SDL2Settings sdl_settings;

        private void button2_Click(object sender, System.EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, System.EventArgs e)
        {
            sdl_settings.Video_Driver = radioButton_opengl.Checked ? "opengl" : "direct3d";
            sdl_settings.Audio_Device_Index = comboBox1.SelectedIndex;

            if (sdl_settings.Audio_Device_Index < 0)
                sdl_settings.Audio_Device_Index = 0;

            sdl_settings.SaveSettings();

            DialogResult = DialogResult.OK;
            Close();
        }
        private void button3_Click(object sender, System.EventArgs e)
        {
            radioButton_direct3d.Checked = false;
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NesEmu.PAUSED = !NesEmu.PAUSED;
        }
    }
}
