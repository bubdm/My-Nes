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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MyNes.Core;

namespace MyNes
{
    public partial class FormStatesBrowser : Form
    {
        public FormStatesBrowser(bool isSave)
        {
            InitializeComponent();
            this.isSave = isSave;
            button_save_load.Text = isSave ? Properties.Resources.Button0 : Properties.Resources.Button1;
            SelectedSlot = StateHandler.Slot;
            RefreshSlots(SelectedSlot);
        }
        private bool isSave;
        private int SelectedSlot;

        private int slot_0_index;
        private int slot_1_index;
        private int slot_2_index;

        private void RefreshSlots(int slot_index)
        {
            slot_0_index = slot_index;
            slot_1_index = slot_0_index + 1;
            slot_2_index = slot_0_index + 2;
            // Get file
            string slot_state_file = StateHandler.GetStateFile(slot_index);
            string slot_state_image = StateHandler.GetStateImageFile(slot_index);
            // Set info
            if (File.Exists(slot_state_file))
            {
                pictureBox_slot0.Image = LoadImage(slot_state_image);
                string inf = Properties.Resources.SLOT + " " + slot_index + "\n";

                FileInfo file_info = new FileInfo(slot_state_file);
                if (file_info != null)
                {
                    inf += ": " + Properties.Resources.Status14 + " " + file_info.LastWriteTime.ToLocalTime();
                }
                else
                    inf += Properties.Resources.Status15;

                richTextBox_slot0.BackColor = (slot_index == SelectedSlot) ? Color.Blue : Color.White;
                richTextBox_slot0.ForeColor = (slot_index == SelectedSlot) ? Color.White : Color.Black;
                richTextBox_slot0.Text = inf;
            }
            else
            {
                pictureBox_slot0.Image = null;

                richTextBox_slot0.BackColor = (slot_index == SelectedSlot) ? Color.Blue : Color.White;
                richTextBox_slot0.ForeColor = (slot_index == SelectedSlot) ? Color.White : Color.Black;
                richTextBox_slot0.Text = Properties.Resources.SLOT + " " + slot_index + "\n" + Properties.Resources.Status16;
            }
            slot_index++;

            slot_state_file = StateHandler.GetStateFile(slot_index);
            slot_state_image = StateHandler.GetStateImageFile(slot_index);
            // Set info
            if (File.Exists(slot_state_file))
            {
                pictureBox_slot1.Image = LoadImage(slot_state_image);
                string inf = Properties.Resources.SLOT + " " + slot_index + "\n";

                FileInfo file_info = new FileInfo(slot_state_file);
                if (file_info != null)
                {
                    inf += ": " + Properties.Resources.Status14 + " " + file_info.LastWriteTime.ToLocalTime();
                }
                else
                    inf += Properties.Resources.Status15;
                richTextBox_slot1.BackColor = (slot_index == SelectedSlot) ? Color.Blue : Color.White;
                richTextBox_slot1.ForeColor = (slot_index == SelectedSlot) ? Color.White : Color.Black;
                richTextBox_slot1.Text = inf;
            }
            else
            {
                pictureBox_slot1.Image = null;

                richTextBox_slot1.BackColor = (slot_index == SelectedSlot) ? Color.Blue : Color.White;
                richTextBox_slot1.ForeColor = (slot_index == SelectedSlot) ? Color.White : Color.Black;
                richTextBox_slot1.Text = Properties.Resources.SLOT + " " + slot_index + "\n" + Properties.Resources.Status16;
            }
            slot_index++;

            slot_state_file = StateHandler.GetStateFile(slot_index);
            slot_state_image = StateHandler.GetStateImageFile(slot_index);
            // Set info
            if (File.Exists(slot_state_file))
            {
                pictureBox_slot2.Image = LoadImage(slot_state_image);
                string inf = Properties.Resources.SLOT + " " + slot_index + "\n";

                FileInfo file_info = new FileInfo(slot_state_file);
                if (file_info != null)
                {
                    inf += ": " + Properties.Resources.Status14 + " " + file_info.LastWriteTime.ToLocalTime();
                }
                else
                    inf += Properties.Resources.Status15;
                richTextBox_slot2.BackColor = (slot_index == SelectedSlot) ? Color.Blue : Color.White;
                richTextBox_slot2.ForeColor = (slot_index == SelectedSlot) ? Color.White : Color.Black;
                richTextBox_slot2.Text = inf;
            }
            else
            {
                pictureBox_slot2.Image = null;

                richTextBox_slot2.BackColor = (slot_index == SelectedSlot) ? Color.Blue : Color.White;
                richTextBox_slot2.ForeColor = (slot_index == SelectedSlot) ? Color.White : Color.Black;
                richTextBox_slot2.Text = Properties.Resources.SLOT + " " + slot_index + "\n" + Properties.Resources.Status16;
            }
        }
        private Image LoadImage(string file)
        {
            if (!File.Exists(file))
                return null;
            Stream str = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buff = new byte[str.Length];
            str.Read(buff, 0, (int)str.Length);
            str.Dispose();
            str.Close();
            if (str != null)
                return Image.FromStream(new MemoryStream(buff));

            return null;
        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            RefreshSlots(vScrollBar1.Value);
        }
        private void pictureBox_slot0_Click(object sender, EventArgs e)
        {
            SelectedSlot = slot_0_index;
            richTextBox_slot0.BackColor = Color.Blue;
            richTextBox_slot0.ForeColor = Color.White;

            richTextBox_slot1.BackColor = Color.White;
            richTextBox_slot1.ForeColor = Color.Black;

            richTextBox_slot2.BackColor = Color.White;
            richTextBox_slot2.ForeColor = Color.Black;
        }
        private void pictureBox_slot1_Click(object sender, EventArgs e)
        {
            SelectedSlot = slot_1_index;
            richTextBox_slot1.BackColor = Color.Blue;
            richTextBox_slot1.ForeColor = Color.White;

            richTextBox_slot0.BackColor = Color.White;
            richTextBox_slot0.ForeColor = Color.Black;

            richTextBox_slot2.BackColor = Color.White;
            richTextBox_slot2.ForeColor = Color.Black;
        }
        private void pictureBox_slot2_Click(object sender, EventArgs e)
        {
            SelectedSlot = slot_2_index;
            richTextBox_slot2.BackColor = Color.Blue;
            richTextBox_slot2.ForeColor = Color.White;

            richTextBox_slot0.BackColor = Color.White;
            richTextBox_slot0.ForeColor = Color.Black;

            richTextBox_slot1.BackColor = Color.White;
            richTextBox_slot1.ForeColor = Color.Black;
        }
        // Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Save-Load
        private void button_save_load_Click(object sender, EventArgs e)
        {
            StateHandler.Slot = SelectedSlot;
            if (isSave)
            {
                StateHandler.SaveState();
            }
            else
            {
                StateHandler.LoadState();
            }
            Close();
        }
        private void richTextBox_slot0_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button_save_load_Click(this, new EventArgs());
        }
    }
}
