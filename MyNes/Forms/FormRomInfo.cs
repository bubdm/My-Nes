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
using System.Windows.Forms;
using System.IO;
using MyNes.Core;
using System.Reflection;

namespace MyNes
{
    public partial class FormRomInfo : Form
    {
        public FormRomInfo(string fileName)
        {
            InitializeComponent();
            LoadFile(fileName);
        }
        public void LoadFile(string fileName)
        {
            // Clear tabs
            tabControl1.TabPages.Clear();
            // See what header it is
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".nes":
                    {
                        // INES INFO !!
                        INes header = new INes();
                        header.Load(fileName, false);
                        if (header.IsValid)
                        {
                            ListView listView = new ListView();
                            listView.View = View.Details;
                            listView.ShowItemToolTips = true;
                            listView.GridLines = true;
                            listView.Columns.Add(Properties.Resources.I0, 100);
                            listView.Columns.Add(Properties.Resources.I1, 100);
                            listView.Dock = DockStyle.Fill;
                            TabPage page = new TabPage();
                            page.Controls.Add(listView);
                            page.Text = Properties.Resources.I2;
                            tabControl1.TabPages.Add(page);
                            // Add list view items
                            ListViewItem item = new ListViewItem(Properties.Resources.I3);
                            item.SubItems.Add(fileName);
                            listView.Items.Add(item);
                            item = new ListViewItem(Properties.Resources.I4);
                            item.SubItems.Add(GetFileSize(fileName));
                            listView.Items.Add(item);
                            item = new ListViewItem("SHA1");
                            item.SubItems.Add(header.SHA1);
                            listView.Items.Add(item);
                            item = new ListViewItem("CRC32");
                            item.SubItems.Add(CalculateCRC(fileName, 16));
                            listView.Items.Add(item);
                            item = new ListViewItem(Properties.Resources.I5);
                            item.SubItems.Add(header.MapperNumber.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Properties.Resources.I6);
                            item.SubItems.Add(header.CHRCount.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Properties.Resources.I7);
                            item.SubItems.Add(GetSize(header.CHRCount * 0x2000));
                            listView.Items.Add(item);
                            item = new ListViewItem(Properties.Resources.I8);
                            item.SubItems.Add(header.PRGCount.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Properties.Resources.I9);
                            item.SubItems.Add(GetSize(header.PRGCount * 0x4000));
                            listView.Items.Add(item);
                            item = new ListViewItem(Properties.Resources.I10);
                            item.SubItems.Add(header.HasBattery.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Properties.Resources.I11);
                            item.SubItems.Add(header.HasTrainer.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Properties.Resources.I12);
                            item.SubItems.Add(header.IsPlaychoice10.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Properties.Resources.I13);
                            item.SubItems.Add(header.IsVSUnisystem.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Properties.Resources.I14);
                            item.SubItems.Add(header.Mirroring.ToString());
                            listView.Items.Add(item);
                        }
                        else
                        {
                            // Add normal file info
                            ListView listView = new ListView();
                            listView.View = View.Details;
                            listView.ShowItemToolTips = true;
                            listView.GridLines = true;
                            listView.Columns.Add(Properties.Resources.I0, 100);
                            listView.Columns.Add(Properties.Resources.I1, 100);
                            listView.Dock = DockStyle.Fill;
                            TabPage page = new TabPage();
                            page.Controls.Add(listView);
                            page.Text = Properties.Resources.I15;
                            tabControl1.TabPages.Add(page);
                            // Add list view items
                            ListViewItem item = new ListViewItem(Properties.Resources.I3);
                            item.SubItems.Add(fileName);
                            listView.Items.Add(item);
                            item = new ListViewItem(Properties.Resources.I4);
                            item.SubItems.Add(GetFileSize(fileName));
                            listView.Items.Add(item);
                            item = new ListViewItem("SHA1");
                            item.SubItems.Add(header.SHA1);
                            listView.Items.Add(item);
                            item = new ListViewItem("CRC32");
                            item.SubItems.Add(CalculateCRC(fileName, 16));
                            listView.Items.Add(item);
                        }
                        // Add database info if found !
                        //Get database info
                        bool found = false;
                        NesCartDatabaseGameInfo info = NesCartDatabase.Find(header.SHA1, out found);
                        NesCartDatabaseCartridgeInfo cart = new NesCartDatabaseCartridgeInfo();
                        if (info.Cartridges != null)
                        {
                            foreach (NesCartDatabaseCartridgeInfo cartinf in info.Cartridges)
                                if (cartinf.SHA1.ToLower() == header.SHA1.ToLower())
                                {
                                    cart = cartinf;
                                    break;
                                }
                        }
                        if (found)
                        {
                            ListView listView = new ListView();
                            listView.View = View.Details;
                            listView.ShowItemToolTips = true;
                            listView.GridLines = true;
                            listView.Columns.Add(Properties.Resources.I0, 100);
                            listView.Columns.Add(Properties.Resources.I1, 100);
                            listView.Dock = DockStyle.Fill;
                            TabPage page = new TabPage();
                            page.Controls.Add(listView);
                            page.Text = Properties.Resources.I16;
                            tabControl1.TabPages.Add(page);
                            //Game info
                            ListViewGroup gr = new ListViewGroup(Properties.Resources.I17);
                            listView.Groups.Add(gr);
                            FieldInfo[] Fields = typeof(NesCartDatabaseGameInfo).GetFields(BindingFlags.Public
                            | BindingFlags.Instance);
                            bool ColorOr = false;
                            for (int i = 0; i < Fields.Length; i++)
                            {
                                if (Fields[i].FieldType == typeof(System.String))
                                {
                                    listView.Items.Add(Fields[i].Name.Replace("_", " "));
                                    gr.Items.Add(listView.Items[listView.Items.Count - 1]);
                                    try
                                    {
                                        listView.Items[listView.Items.Count - 1].SubItems.Add(Fields[i].GetValue
                                            (info).ToString());
                                    }
                                    catch
                                    {
                                        listView.Items[listView.Items.Count - 1].SubItems.Add("");
                                    }
                                    if (ColorOr)
                                        listView.Items[listView.Items.Count - 1].BackColor = Color.WhiteSmoke;
                                    ColorOr = !ColorOr;
                                }
                            }

                            //chips
                            if (cart.chip_type != null)
                            {
                                for (int i = 0; i < cart.chip_type.Count; i++)
                                {
                                    listView.Items.Add("Chip " + (i + 1));
                                    gr.Items.Add(listView.Items[listView.Items.Count - 1]);
                                    listView.Items[listView.Items.Count - 1].SubItems.Add(cart.chip_type[i]);
                                    if (ColorOr)
                                        listView.Items[listView.Items.Count - 1].BackColor = Color.WhiteSmoke;
                                    ColorOr = !ColorOr;
                                }
                            }

                            //Cartridge
                            gr = new ListViewGroup(Properties.Resources.I18);
                            listView.Groups.Add(gr);
                            Fields = typeof(NesCartDatabaseCartridgeInfo).GetFields(BindingFlags.Public
                            | BindingFlags.Instance);
                            ColorOr = false;
                            for (int i = 0; i < Fields.Length; i++)
                            {
                                if (Fields[i].FieldType == typeof(System.String))
                                {
                                    listView.Items.Add(Fields[i].Name.Replace("_", " "));
                                    gr.Items.Add(listView.Items[listView.Items.Count - 1]);
                                    try
                                    {
                                        listView.Items[listView.Items.Count - 1].SubItems.Add(Fields[i].GetValue(cart).ToString());
                                    }
                                    catch
                                    {
                                        listView.Items[listView.Items.Count - 1].SubItems.Add("");
                                    }
                                    if (ColorOr)
                                        listView.Items[listView.Items.Count - 1].BackColor = Color.WhiteSmoke;
                                    ColorOr = !ColorOr;
                                }
                            }

                            //DataBase
                            gr = new ListViewGroup(Properties.Resources.I19);
                            listView.Groups.Add(gr);
                            Fields = typeof(NesCartDatabase).GetFields(BindingFlags.Public
                          | BindingFlags.Static);
                            ColorOr = false;
                            for (int i = 0; i < Fields.Length; i++)
                            {
                                if (Fields[i].FieldType == typeof(System.String))
                                {
                                    listView.Items.Add(Fields[i].Name.Remove(0, 2));
                                    gr.Items.Add(listView.Items[listView.Items.Count - 1]);
                                    try
                                    {
                                        listView.Items[listView.Items.Count - 1].SubItems.Add(Fields[i].GetValue(info).ToString());
                                    }
                                    catch
                                    {
                                        listView.Items[listView.Items.Count - 1].SubItems.Add("");
                                    }
                                    if (ColorOr)
                                        listView.Items[listView.Items.Count - 1].BackColor = Color.WhiteSmoke;
                                    ColorOr = !ColorOr;
                                }
                            }
                        }
                        break;
                    }
            }
        }
        private string GetFileSize(string FilePath)
        {
            if (File.Exists(Path.GetFullPath(FilePath)) == true)
            {
                FileInfo Info = new FileInfo(FilePath);
                string Unit = " " + "Byte";
                double Len = Info.Length;
                if (Info.Length >= 1024)
                {
                    Len = Info.Length / 1024.00;
                    Unit = " KB";
                }
                if (Len >= 1024)
                {
                    Len /= 1024.00;
                    Unit = " MB";
                }
                if (Len >= 1024)
                {
                    Len /= 1024.00;
                    Unit = " GB";
                }
                return Len.ToString("F2") + Unit;
            }
            return "";
        }
        private string GetSize(long size)
        {
            string Unit = " Byte";
            double Len = size;
            if (size >= 1024)
            {
                Len = size / 1024.00;
                Unit = " KB";
            }
            if (Len >= 1024)
            {
                Len /= 1024.00;
                Unit = " MB";
            }
            if (Len >= 1024)
            {
                Len /= 1024.00;
                Unit = " GB";
            }
            if (Len < 0)
                return "???";
            return Len.ToString("F2") + Unit;
        }
        private string CalculateCRC(string filePath, int bytesToSkip)
        {
            if (File.Exists(filePath))
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileStream.Read(new byte[bytesToSkip], 0, bytesToSkip);
                byte[] fileBuffer = new byte[fileStream.Length - bytesToSkip];
                fileStream.Read(fileBuffer, 0, (int)(fileStream.Length - bytesToSkip));
                fileStream.Close();

                string crc = "";
                Crc32 crc32 = new Crc32();
                byte[] crc32Buffer = crc32.ComputeHash(fileBuffer);

                foreach (byte b in crc32Buffer)
                    crc += b.ToString("x2").ToLower();

                return crc;
            }
            return "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Copy to clipboard
        private void button2_Click(object sender, EventArgs e)
        {
            ListView listView1 = (ListView)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];
            RichTextBox textBox = new RichTextBox();
            foreach (ListViewItem item in listView1.Items)
            {
                textBox.Text += item.Text + ": " + item.SubItems[1].Text + "\n";
            }
            textBox.SelectAll();
            textBox.Copy();
            MessageBox.Show(Properties.Resources.Done);
        }
    }
}
