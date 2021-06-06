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
using System.Windows.Forms;
using MyNes.Core;

namespace MyNes
{
    public partial class FormFolders : Form
    {
        public FormFolders()
        {
            InitializeComponent();

            textBox_gm_codes.Text = MyNesMain.EmuSettings.GameGenieFolder;
            textBox_snapshots.Text = MyNesMain.EmuSettings.SnapsFolder;
            textBox_sound_records.Text = MyNesMain.EmuSettings.WavesFolder;
            textBox_srams.Text = MyNesMain.EmuSettings.SRAMFolder;
            textBox_states.Text = MyNesMain.EmuSettings.StateFolder;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(textBox_snapshots.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(textBox_states.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(textBox_srams.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(textBox_gm_codes.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(textBox_sound_records.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.Description = Properties.Resources.Desc6;
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_snapshots.Text;
            if (fol.ShowDialog(this) == DialogResult.OK)
            {
                textBox_snapshots.Text = fol.SelectedPath;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.Description = Properties.Resources.desc7;
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_states.Text;
            if (fol.ShowDialog(this) == DialogResult.OK)
            {
                textBox_states.Text = fol.SelectedPath;
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.Description = Properties.Resources.Desc8;
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_srams.Text;
            if (fol.ShowDialog(this) == DialogResult.OK)
            {
                textBox_srams.Text = fol.SelectedPath;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.Description = Properties.Resources.Desc9;
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_gm_codes.Text;
            if (fol.ShowDialog(this) == DialogResult.OK)
            {
                textBox_gm_codes.Text = fol.SelectedPath;
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.Description = Properties.Resources.Desc10;
            fol.ShowNewFolderButton = true;
            fol.SelectedPath = textBox_sound_records.Text;
            if (fol.ShowDialog(this) == DialogResult.OK)
            {
                textBox_sound_records.Text = fol.SelectedPath;
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Defaults
        private void button13_Click(object sender, EventArgs e)
        {
            textBox_snapshots.Text = Path.Combine(MyNesMain.WorkingFolder, "Snaps");
            textBox_gm_codes.Text = Path.Combine(MyNesMain.WorkingFolder, "GMCodes");
            textBox_sound_records.Text = Path.Combine(MyNesMain.WorkingFolder, "SoundRecords");
            textBox_srams.Text = Path.Combine(MyNesMain.WorkingFolder, "Srams");
            textBox_states.Text = Path.Combine(MyNesMain.WorkingFolder, "States");
        }
        // Save
        private void button11_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox_gm_codes.Text))
            {
                MessageBox.Show(Properties.Resources.Message16);
                button8_Click(this, new EventArgs());
            }
            if (!Directory.Exists(textBox_snapshots.Text))
            {
                MessageBox.Show(Properties.Resources.Message17);
                button1_Click(this, new EventArgs());
            }
            if (!Directory.Exists(textBox_sound_records.Text))
            {
                MessageBox.Show(Properties.Resources.Message18);
                button10_Click(this, new EventArgs());
            }
            if (!Directory.Exists(textBox_srams.Text))
            {
                MessageBox.Show(Properties.Resources.Message19);
                button6_Click(this, new EventArgs());
            }
            if (!Directory.Exists(textBox_states.Text))
            {
                MessageBox.Show(Properties.Resources.Message20);
                button4_Click(this, new EventArgs());
            }

            MyNesMain.EmuSettings.GameGenieFolder = textBox_gm_codes.Text;
            MyNesMain.EmuSettings.SnapsFolder = textBox_snapshots.Text;
            MyNesMain.EmuSettings.WavesFolder = textBox_sound_records.Text;
            MyNesMain.EmuSettings.SRAMFolder = textBox_srams.Text;
            MyNesMain.EmuSettings.StateFolder = textBox_states.Text;

            Close();
        }
    }
}
