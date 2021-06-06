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
    partial class FormStatesBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStatesBrowser));
            this.pictureBox_slot0 = new System.Windows.Forms.PictureBox();
            this.richTextBox_slot0 = new System.Windows.Forms.RichTextBox();
            this.richTextBox_slot1 = new System.Windows.Forms.RichTextBox();
            this.pictureBox_slot1 = new System.Windows.Forms.PictureBox();
            this.richTextBox_slot2 = new System.Windows.Forms.RichTextBox();
            this.pictureBox_slot2 = new System.Windows.Forms.PictureBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.button_save_load = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_slot0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_slot1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_slot2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_slot0
            // 
            resources.ApplyResources(this.pictureBox_slot0, "pictureBox_slot0");
            this.pictureBox_slot0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_slot0.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_slot0.Name = "pictureBox_slot0";
            this.pictureBox_slot0.TabStop = false;
            this.pictureBox_slot0.Click += new System.EventHandler(this.pictureBox_slot0_Click);
            this.pictureBox_slot0.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richTextBox_slot0_MouseDoubleClick);
            // 
            // richTextBox_slot0
            // 
            resources.ApplyResources(this.richTextBox_slot0, "richTextBox_slot0");
            this.richTextBox_slot0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox_slot0.Cursor = System.Windows.Forms.Cursors.Hand;
            this.richTextBox_slot0.Name = "richTextBox_slot0";
            this.richTextBox_slot0.ReadOnly = true;
            this.richTextBox_slot0.Click += new System.EventHandler(this.pictureBox_slot0_Click);
            this.richTextBox_slot0.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richTextBox_slot0_MouseDoubleClick);
            // 
            // richTextBox_slot1
            // 
            resources.ApplyResources(this.richTextBox_slot1, "richTextBox_slot1");
            this.richTextBox_slot1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox_slot1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.richTextBox_slot1.Name = "richTextBox_slot1";
            this.richTextBox_slot1.ReadOnly = true;
            this.richTextBox_slot1.Click += new System.EventHandler(this.pictureBox_slot1_Click);
            this.richTextBox_slot1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richTextBox_slot0_MouseDoubleClick);
            // 
            // pictureBox_slot1
            // 
            resources.ApplyResources(this.pictureBox_slot1, "pictureBox_slot1");
            this.pictureBox_slot1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_slot1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_slot1.Name = "pictureBox_slot1";
            this.pictureBox_slot1.TabStop = false;
            this.pictureBox_slot1.Click += new System.EventHandler(this.pictureBox_slot1_Click);
            this.pictureBox_slot1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richTextBox_slot0_MouseDoubleClick);
            // 
            // richTextBox_slot2
            // 
            resources.ApplyResources(this.richTextBox_slot2, "richTextBox_slot2");
            this.richTextBox_slot2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox_slot2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.richTextBox_slot2.Name = "richTextBox_slot2";
            this.richTextBox_slot2.ReadOnly = true;
            this.richTextBox_slot2.Click += new System.EventHandler(this.pictureBox_slot2_Click);
            this.richTextBox_slot2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richTextBox_slot0_MouseDoubleClick);
            // 
            // pictureBox_slot2
            // 
            resources.ApplyResources(this.pictureBox_slot2, "pictureBox_slot2");
            this.pictureBox_slot2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_slot2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_slot2.Name = "pictureBox_slot2";
            this.pictureBox_slot2.TabStop = false;
            this.pictureBox_slot2.Click += new System.EventHandler(this.pictureBox_slot2_Click);
            this.pictureBox_slot2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richTextBox_slot0_MouseDoubleClick);
            // 
            // vScrollBar1
            // 
            resources.ApplyResources(this.vScrollBar1, "vScrollBar1");
            this.vScrollBar1.Maximum = 16;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // button_save_load
            // 
            resources.ApplyResources(this.button_save_load, "button_save_load");
            this.button_save_load.Name = "button_save_load";
            this.button_save_load.UseVisualStyleBackColor = true;
            this.button_save_load.Click += new System.EventHandler(this.button_save_load_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormStatesBrowser
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_save_load);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.richTextBox_slot2);
            this.Controls.Add(this.pictureBox_slot2);
            this.Controls.Add(this.richTextBox_slot1);
            this.Controls.Add(this.pictureBox_slot1);
            this.Controls.Add(this.richTextBox_slot0);
            this.Controls.Add(this.pictureBox_slot0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStatesBrowser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_slot0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_slot1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_slot2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_slot0;
        private System.Windows.Forms.RichTextBox richTextBox_slot0;
        private System.Windows.Forms.RichTextBox richTextBox_slot1;
        private System.Windows.Forms.PictureBox pictureBox_slot1;
        private System.Windows.Forms.RichTextBox richTextBox_slot2;
        private System.Windows.Forms.PictureBox pictureBox_slot2;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Button button_save_load;
        private System.Windows.Forms.Button button2;
    }
}