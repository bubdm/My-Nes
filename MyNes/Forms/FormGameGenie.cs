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
using System.Windows.Forms;
using System.Xml;
using MyNes.Core;
namespace MyNes
{
    public partial class FormGameGenie : Form
    {
        public FormGameGenie()
        {
            InitializeComponent();
            if (!NesEmu.ON)
            {
                MessageBox.Show(Properties.Resources.Message21);
                Close();
                return;
            }
            gameGenie = new GameGenie();
            textBox2.SelectAll();
            //load list if found
            LoadListFile(NesEmu.GMFileName);

            checkBox1.Checked = NesEmu.IsGameGenieActive;
        }
        private GameGenie gameGenie;

        private void LoadListFile(string file)
        {
            if (!System.IO.File.Exists(file))
            {
                listView1.Items.Clear();
                return;
            }
            XmlReaderSettings sett = new XmlReaderSettings();
            sett.DtdProcessing = DtdProcessing.Ignore;
            sett.IgnoreWhitespace = true;
            XmlReader XMLread = XmlReader.Create(file, sett);
            XMLread.Read();//Reads the XML definition <XML>
            XMLread.Read();//Reads the header
            if (XMLread.Name != "MyNesGameGenieCodesList")
            {
                MessageBox.Show(Properties.Resources.Message22);
                XMLread.Close();
                return;
            }
            listView1.Items.Clear();
            while (XMLread.Read())
            {
                if (XMLread.Name == "Code")
                {
                    XMLread.MoveToAttribute("code");
                    listView1.Items.Add(XMLread.Value.ToString());
                    listView1.Items[listView1.Items.Count - 1].Checked = true;
                    XMLread.MoveToAttribute("comment");
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(XMLread.Value);
                }
            }
            XMLread.Close();
        }
        private void SaveListFile(string file)
        {
            XmlWriterSettings sett = new XmlWriterSettings();
            sett.Indent = true;
            XmlWriter XMLwrt = XmlWriter.Create(file, sett);
            XMLwrt.WriteStartElement("MyNesGameGenieCodesList");//header
            foreach (ListViewItem item in listView1.Items)
            {
                XMLwrt.WriteStartElement("Code");
                XMLwrt.WriteAttributeString("code", item.Text);
                XMLwrt.WriteAttributeString("comment", item.SubItems[1].Text);
                XMLwrt.WriteEndElement();//Code end
            }
            XMLwrt.WriteEndElement();//header end
            XMLwrt.Flush();
            XMLwrt.Close();
        }
        private void ShowValues()
        {
            if (textBox2.Text.Length == 6)
            {
                textBox_address.Text = string.Format("{0:X4}", gameGenie.GetGGAddress(gameGenie.GetCodeAsHEX(textBox2.Text), 6));
                textBox_value.Text = string.Format("{0:X2}", gameGenie.GetGGValue(gameGenie.GetCodeAsHEX(textBox2.Text), 6));
                textBox1.Text = "00";
            }
            else if (textBox2.Text.Length == 8)//8 code length
            {
                textBox_address.Text = string.Format("{0:X4}", gameGenie.GetGGAddress(gameGenie.GetCodeAsHEX(textBox2.Text), 8));
                textBox_value.Text = string.Format("{0:X2}", gameGenie.GetGGValue(gameGenie.GetCodeAsHEX(textBox2.Text), 8));
                textBox1.Text = string.Format("{0:X2}", gameGenie.GetGGCompareValue(gameGenie.GetCodeAsHEX(textBox2.Text)));
            }
            else//code incomplete
            {
                textBox_address.Text = Properties.Resources.ErrorStatus;
                textBox_value.Text = textBox1.Text = Properties.Resources.ErrorStatus;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button3.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button4.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button5.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button6.Text;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button9.Text;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button8.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button7.Text;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button10.Text;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button12.Text;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button14.Text;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button16.Text;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button17.Text;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button15.Text;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button13.Text;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + button11.Text;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
                textBox2.Text = textBox2.Text.Substring(0, textBox2.Text.Length - 1);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label_error.Visible = false;
            textBox2.Text = textBox2.Text.ToUpper();
            textBox2.Text.Replace(" ", "A");
            textBox2.Text.Replace("B", "A");
            textBox2.Text.Replace("C", "A");
            textBox2.Text.Replace("D", "A");
            textBox2.Text.Replace("F", "A");
            textBox2.Text.Replace("H", "A");
            textBox2.Text.Replace("J", "A");
            textBox2.Text.Replace("M", "A");
            textBox2.Text.Replace("Q", "A");
            textBox2.Text.Replace("R", "A");
            textBox2.Text.Replace("W", "A");
            textBox2.Text.Replace("Z", "A");
            if (textBox2.Text.Length > 8)
                textBox2.Text = textBox2.Text.Substring(0, 8);
            textBox2.SelectionStart = textBox2.Text.Length;
            ShowValues();
        }
        //add code
        private void button20_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length != 6 && textBox2.Text.Length != 8)
            {
                label_error.Visible = true;
                return;
            }
            if (textBox2.Text.Contains(" "))
            {
                label_error.Visible = true;
                return;
            }
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text == textBox2.Text)
                {
                    MessageBox.Show(Properties.Resources.Message23);
                    return;
                }
            }
            FormEnterName comment = new FormEnterName(Properties.Resources.Message24, Properties.Resources.MessageCap3, true, false);
            if (comment.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                listView1.Items.Add(textBox2.Text);
                listView1.Items[listView1.Items.Count - 1].Checked = true;
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(comment.EnteredName);
            }
        }
        //remove selected
        private void button24_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            if (MessageBox.Show(Properties.Resources.Message25, Properties.Resources.MessageCap4, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.Yes)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
        }

        private void FormGameGenie_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & Keys.Control) == Keys.Control && (e.KeyCode & Keys.C) == Keys.C)
            {
                textBox2.Copy();
            }
            if ((e.KeyCode & Keys.Control) == Keys.Control && (e.KeyCode & Keys.V) == Keys.V)
            {
                textBox2.Paste();
            }
        }
        //save list as
        private void button22_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show(Properties.Resources.Message26);
                return;
            }
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "XML (*.xml)|*.xml";
            save.FileName = System.IO.Path.GetFileNameWithoutExtension(NesEmu.CurrentFilePath) + ".xml";
            if (save.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                SaveListFile(save.FileName);
            }
        }
        //load list
        private void button21_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "XML (*.xml)|*.xml";
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                LoadListFile(op.FileName);
            }
        }
        //ok
        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show(Properties.Resources.Message27);
                return;
            }
            //Nes.Board.IsGameGenieActive = checkBox1.Checked;
            List<GameGenieCode> codes = new List<GameGenieCode>();
            gameGenie = new GameGenie();
            foreach (ListViewItem item in listView1.Items)
            {
                GameGenieCode code = new GameGenieCode();
                code.Enabled = item.Checked;
                code.Name = item.Text;
                code.Descreption = item.SubItems[1].Text;
                if (item.Text.Length == 6)
                {
                    code.Address = gameGenie.GetGGAddress(gameGenie.GetCodeAsHEX(item.Text), 6) | 0x8000;
                    code.Value = gameGenie.GetGGValue(gameGenie.GetCodeAsHEX(item.Text), 6);
                    code.IsCompare = false;
                }
                else
                {
                    code.Address = gameGenie.GetGGAddress(gameGenie.GetCodeAsHEX(item.Text), 8) | 0x8000;
                    code.Value = gameGenie.GetGGValue(gameGenie.GetCodeAsHEX(item.Text), 8);
                    code.Compare = gameGenie.GetGGCompareValue(gameGenie.GetCodeAsHEX(item.Text));
                    code.IsCompare = true;
                }
                //add to active list
                codes.Add(code);
            }
            NesEmu.SetupGameGenie(checkBox1.Checked, codes.ToArray());
            SaveListFile(NesEmu.GMFileName);
            this.Close();
        }
    }
}
