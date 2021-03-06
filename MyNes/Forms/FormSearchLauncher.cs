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
    public partial class FormSearchLauncher : Form
    {
        public FormSearchLauncher()
        {
            InitializeComponent();
            this.Tag = "find";
            RefreshModeCombobox();
            comboBox_condition.SelectedIndex = 0;
        }

        private bool IsNumberSelection;
        public event EventHandler<SearchRequestArgs> SearchRequest;

        private void RefreshModeCombobox()
        {
            comboBox_searchBy.Items.Clear();
            // Add the columns here
            MyNesDBColumn[] columns = MyNesDB.GetColumns();
            foreach (MyNesDBColumn c in columns)
                if (c.Visible)
                    comboBox_searchBy.Items.Add(c.Name);
            comboBox_searchBy.SelectedIndex = 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Find
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_findWhat.Text.Length == 0)
            {
                MessageBox.Show(Properties.Resources.Message30, Properties.Resources.MessageCap5);
                return;
            }
            if (comboBox_searchBy.SelectedIndex < 0)
            {
                MessageBox.Show(Properties.Resources.Message31, Properties.Resources.MessageCap5);
                return;
            }

            TextSearchCondition textCondition = TextSearchCondition.None;
            NumberSearchCondition numberCondition = NumberSearchCondition.None;
            bool isNumber = false;
            switch (comboBox_searchBy.SelectedItem.ToString())
            {
                case "Size":
                case "Played":
                case "Play Time":
                case "Board Mapper":
                case "Rating": isNumber = true; break;
            }
            if (!isNumber)
            {
                switch (comboBox_condition.SelectedIndex)
                {
                    case 0: textCondition = TextSearchCondition.Contains; break;
                    case 1: textCondition = TextSearchCondition.DoesNotContain; break;
                    case 2: textCondition = TextSearchCondition.Is; break;
                    case 3: textCondition = TextSearchCondition.IsNot; break;
                    case 4: textCondition = TextSearchCondition.StartWith; break;
                    case 5: textCondition = TextSearchCondition.DoesNotStartWith; break;
                    case 6: textCondition = TextSearchCondition.EndWith; break;
                    case 7: textCondition = TextSearchCondition.DoesNotEndWith; break;
                }
            }
            else
            {
                switch (comboBox_condition.SelectedIndex)
                {
                    case 0: numberCondition = NumberSearchCondition.Equal; break;
                    case 1: numberCondition = NumberSearchCondition.DoesNotEqual; break;
                    case 2: numberCondition = NumberSearchCondition.Larger; break;
                    case 3: numberCondition = NumberSearchCondition.EuqalLarger; break;
                    case 4: numberCondition = NumberSearchCondition.Smaller; break;
                    case 5: numberCondition = NumberSearchCondition.EqualSmaller; break;
                }
            }
            // Raise the event !
            if (SearchRequest != null)
                SearchRequest(this, new SearchRequestArgs(textBox_findWhat.Text, comboBox_searchBy.SelectedItem.ToString(), isNumber,
                    textCondition, numberCondition, checkBox_caseSensitive.Checked));
        }

        private void comboBox_searchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool _temp = IsNumberSelection;
            switch (comboBox_searchBy.SelectedItem.ToString())
            {
                case "Size":
                case "Played":
                case "Play Time":
                case "Board Mapper":
                case "Rating": IsNumberSelection = true; break;
                default: IsNumberSelection = false; break;
            }
            if (_temp != IsNumberSelection)
            {
                // Refresh condition combobox
                comboBox_condition.Items.Clear();
                if (!IsNumberSelection)
                {
                    comboBox_condition.Items.Add(Properties.Resources.S1);
                    comboBox_condition.Items.Add(Properties.Resources.S2);
                    comboBox_condition.Items.Add(Properties.Resources.S3);
                    comboBox_condition.Items.Add(Properties.Resources.S4);
                    comboBox_condition.Items.Add(Properties.Resources.S5);
                    comboBox_condition.Items.Add(Properties.Resources.S6);
                    comboBox_condition.Items.Add(Properties.Resources.S7);
                    comboBox_condition.Items.Add(Properties.Resources.S8);
                }
                else
                {
                    comboBox_condition.Items.Add(Properties.Resources.S9);
                    comboBox_condition.Items.Add(Properties.Resources.S10);
                    comboBox_condition.Items.Add(Properties.Resources.S11);
                    comboBox_condition.Items.Add(Properties.Resources.S12);
                    comboBox_condition.Items.Add(Properties.Resources.S13);
                    comboBox_condition.Items.Add(Properties.Resources.S14);
                }
                comboBox_condition.SelectedIndex = 0;
            }
        }
    }
    public class SearchRequestArgs : EventArgs
    {
        public SearchRequestArgs(string searchWhat, string searchColumn, bool isNumber, TextSearchCondition conditionForText,
            NumberSearchCondition conditionForNumber, bool caseSensitive)
        {
            this.SearchWhat = searchWhat;
            this.SearchColumn = searchColumn;
            this.IsNumber = isNumber;
            this.TextCondition = conditionForText;
            this.NumberCondition = conditionForNumber;
            this.CaseSensitive = caseSensitive;
        }
        public string SearchWhat { get; private set; }
        public string SearchColumn { get; private set; }
        public bool IsNumber { get; private set; }
        public TextSearchCondition TextCondition { get; private set; }
        public NumberSearchCondition NumberCondition { get; private set; }
        public bool CaseSensitive { get; private set; }
    }
    public enum TextSearchCondition : int
    {
        None = -1,
        Contains = 0,
        DoesNotContain = 1,
        Is = 2,
        IsNot = 3,
        StartWith = 4,
        DoesNotStartWith = 5,
        EndWith = 6,
        DoesNotEndWith = 7
    }
    public enum NumberSearchCondition
    {
        None = -1,
        Equal = 0,
        DoesNotEqual = 1,
        Larger = 2,
        EuqalLarger = 3,
        Smaller = 4,
        EqualSmaller = 5,
    }
}
