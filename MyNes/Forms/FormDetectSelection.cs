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
using System.Windows.Forms;

namespace MyNes
{
    public partial class FormDetectSelection : Form
    {
        public FormDetectSelection()
        {
            InitializeComponent();
        }
        public DetectMode MODE
        {
            get
            {
                if (radioButton_snaps.Checked)
                    return DetectMode.SNAPS;
                else if (radioButton_covers.Checked)
                    return DetectMode.COVERS;
                else if (radioButton_infos.Checked)
                    return DetectMode.INFOS;
                else
                    return DetectMode.NONE;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
    public enum DetectMode { SNAPS, COVERS, INFOS, NONE }
}
