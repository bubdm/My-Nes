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
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MyNes
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
            // Set version info
            string ver_m = Assembly.LoadFile(Path.Combine(Program.ApplicationFolder, "MyNes.exe")).GetName().Version.ToString();
            label_version.Text = Properties.Resources.Version + " " + ver_m;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        // License
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Path.Combine(Program.ApplicationFolder, "GNU GENERAL PUBLIC LICENSE 3.0.html"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Copyright notice
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Path.Combine(Program.ApplicationFolder, "Copyright Notice.txt"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("mailto:alaahadidfreeware@gmail.com");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.gnu.org/licenses/translations.en.html");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
