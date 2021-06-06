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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace MyNes
{
    public partial class FormInputSettings : Form
    {
        public FormInputSettings()
        {
            InitializeComponent();
            // Add controls
            controls.Add(new InputControlJoypad(0));
            controls.Add(new InputControlJoypad(1));
            controls.Add(new InputControlJoypad(2));
            controls.Add(new InputControlJoypad(3));
            controls.Add(new InputControlShortcuts());
           // controls.Add(new InputControlVSUnisystemDIP());
            foreach (IInputSettingsControl control in controls)
            {
                control.LoadSettings();
                listBox1.Items.Add(control.ToString());
            }
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }
        private List<IInputSettingsControl> controls = new List<IInputSettingsControl>();

        public void SelectSettingsPage(int page)
        {
            if (listBox1.Items.Count > 0 && page >= 0 && page < listBox1.Items.Count)
                listBox1.SelectedIndex = page;
        }
        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (IInputSettingsControl control in controls)
            {
                if (control.CanSaveSettings)
                    control.SaveSettings();
                else
                    return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        // Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            IInputSettingsControl control = controls[listBox1.SelectedIndex];
            control.Location = new Point(0, 0);
            splitContainer1.Panel2.Controls.Add(control);
        }
    }
}
