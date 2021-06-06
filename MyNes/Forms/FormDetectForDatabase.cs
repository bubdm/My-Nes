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
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
namespace MyNes
{
    public partial class FormDetectForDatabase : Form
    {
        public FormDetectForDatabase(DetectMode mode)
        {
            this.mode = mode;
            InitializeComponent();
            switch (mode)
            {
                case DetectMode.SNAPS:
                    {
                        this.Text = Properties.Resources.Desc3;
                        textBox_extensions.Text = ".jpg;.png;.bmp;.gif;.jpeg;.tiff;.tif;.tga;.ico";
                        if (Program.Settings.Database_FoldersSnapshots == null)
                            Program.Settings.Database_FoldersSnapshots = new string[0];
                        foreach (string folder in Program.Settings.Database_FoldersSnapshots)
                        {
                            if (Directory.Exists(folder))
                                listView1.Items.Add(folder, 0);
                        }
                        break;
                    }
                case DetectMode.COVERS:
                    {
                        this.Text = Properties.Resources.Desc4;
                        textBox_extensions.Text = ".jpg;.png;.bmp;.gif;.jpeg;.tiff;.tif;.tga;.ico";
                        if (Program.Settings.Database_FoldersCovers == null)
                            Program.Settings.Database_FoldersCovers = new string[0];
                        foreach (string folder in Program.Settings.Database_FoldersCovers)
                        {
                            if (Directory.Exists(folder))
                                listView1.Items.Add(folder, 0);
                        }
                        break;
                    }
                case DetectMode.INFOS:
                    {
                        this.Text = Properties.Resources.Desc5;
                        textBox_extensions.Text = ".txt;.doc;.rtf";
                        if (Program.Settings.Database_FoldersInfos == null)
                            Program.Settings.Database_FoldersInfos = new string[0];
                        foreach (string folder in Program.Settings.Database_FoldersInfos)
                        {
                            if (Directory.Exists(folder))
                                listView1.Items.Add(folder, 0);
                        }
                        break;
                    }
            }
        }
        private DetectMode mode;
        private Thread mainThread;
        private string status;
        private int process;
        // Options
        private List<string> foldersToSearch;
        private List<string> extensions;
        private bool includeSubFolders;
        private bool clearOldRomDetectedFiles;
        private bool useRomNameInsteadRomFileName;
        private bool matchCase;
        private bool matchWord;
        private bool oneFilePerRom;
        private bool useNameWhenPathNotValid;
        private bool searchmode_FileInRom;
        private bool searchmode_RomInFile;
        private bool searchmode_Both;
        private bool startWith;
        private bool contains;
        private bool endWith;
        private bool dontAllowSameFileDetectedByMoreThanOneRom;
        private bool finished;

        private void SEARCH()
        {
            int matchedRoms = 0;
            DataSet set = MyNesDB.GetDataSet("GAMES");
            List<string> files = new List<string>();
            foreach (string folder in foldersToSearch)
            {
                files.AddRange(Directory.GetFiles(folder, "*",
                    includeSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
            }
            // Clear detected files first ?
            if (clearOldRomDetectedFiles)
                MyNesDB.DeleteDetects(mode.ToString());
            // Start the operation, loop through roms
            for (int i = 0; i < set.Tables[0].Rows.Count; i++)
            {
                string id = set.Tables[0].Rows[i]["Id"].ToString();
                string entryName = set.Tables[0].Rows[i]["Name"].ToString().Replace("&apos;", "'");
                string entryPath = set.Tables[0].Rows[i]["Path"].ToString().Replace("&apos;", "'");
                // Decode path
                if (entryPath.StartsWith("("))
                {
                    // Decode
                    string[] pathCodes = entryPath.Split(new char[] { '(', ')' });
                    entryPath = pathCodes[2];
                }
                // Loop through files, look for files for this rom
                for (int j = 0; j < files.Count; j++)
                {
                    if (!extensions.Contains(Path.GetExtension(files[j]).ToLower()))
                    {
                        Trace.WriteLine("File ignored (no match for extension): " + files[j], "Detect Files");
                        // Useless file ...
                        files.RemoveAt(j);
                        j--;
                        continue;
                    }
                    if (FilterSearch(entryName, entryPath, files[j]))
                    {
                        matchedRoms++;
                        // Add it !
                        // Make sure this file isn;t exist for selected game
                        MyNesDetectEntryInfo[] detects = MyNesDB.GetDetects(mode.ToString(), id);
                        bool found = false;
                        if (detects != null)
                        {
                            foreach (MyNesDetectEntryInfo inf in detects)
                            {
                                if (inf.Path == files[j])
                                {
                                    found = true; break;
                                }
                            }
                        }
                        if (!found)
                        {
                            // Add it !
                            MyNesDetectEntryInfo newDetect = new MyNesDetectEntryInfo();
                            newDetect.GameID = id;
                            newDetect.Path = files[j];
                            newDetect.Name = Path.GetFileNameWithoutExtension(files[j]);
                            newDetect.FileInfo = "";
                            MyNesDB.AddDetect(mode.ToString(), newDetect);
                        }
                        // To reduce process, delete detected file
                        if (dontAllowSameFileDetectedByMoreThanOneRom)
                        {
                            files.RemoveAt(j);
                            j--;
                        }

                        if (oneFilePerRom)
                            break;
                    }
                }
                // Update progress
                process = (i * 100) / set.Tables[0].Rows.Count;
                status = string.Format(Properties.Resources.Status9 +
                    " {0} / {1} [{2} " + Properties.Resources.status10 + "][{3} %]", (i + 1),
                    set.Tables[0].Rows.Count, matchedRoms, process);
            }
            // Done !
            Trace.WriteLine("Detect process finished at " + DateTime.Now.ToLocalTime().ToString(), "Detect Files");
            finished = true;
            Trace.WriteLine("----------------------------");
            CloseWin();
        }
        private void CloseWin()
        {
            if (!this.InvokeRequired)
                CloseWin1();
            else
                this.Invoke(new Action(CloseWin1));
        }
        private void CloseWin1()
        {
            timer1.Stop();
            // Save folders
            switch (mode)
            {
                case DetectMode.SNAPS:
                    {
                        Program.Settings.Database_FoldersSnapshots = new string[listView1.Items.Count];
                        for (int i = 0; i < listView1.Items.Count; i++)
                            Program.Settings.Database_FoldersSnapshots[i] = listView1.Items[i].Text;
                        break;
                    }
                case DetectMode.COVERS:
                    {
                        Program.Settings.Database_FoldersCovers = new string[listView1.Items.Count];
                        for (int i = 0; i < listView1.Items.Count; i++)
                            Program.Settings.Database_FoldersCovers[i] = listView1.Items[i].Text;
                        break;
                    }
                case DetectMode.INFOS:
                    {
                        Program.Settings.Database_FoldersInfos = new string[listView1.Items.Count];
                        for (int i = 0; i < listView1.Items.Count; i++)
                            Program.Settings.Database_FoldersInfos[i] = listView1.Items[i].Text;
                        break;
                    }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        private bool FilterSearch(string entryName, string entryPath, string filePath)
        {
            // Let's see what's the mode
            string searchWord_ROM = "";
            string searchTargetText_FILE = "";
            bool isUsingName = false;

            if (entryPath == "N/A")
                if (useNameWhenPathNotValid)
                    isUsingName = true;
                else
                    entryPath = "";

            if (!useRomNameInsteadRomFileName)
            {
                if (!isUsingName)
                    searchWord_ROM = matchCase ? Path.GetFileNameWithoutExtension(entryPath) : Path.GetFileNameWithoutExtension(entryPath).ToLower();
                else
                    searchWord_ROM = matchCase ? entryName : entryName.ToLower();
                searchTargetText_FILE = matchCase ? Path.GetFileNameWithoutExtension(filePath) : Path.GetFileNameWithoutExtension(filePath).ToLower();
            }
            else
            {
                searchWord_ROM = matchCase ? entryName : entryName.ToLower();
                searchTargetText_FILE = matchCase ? Path.GetFileNameWithoutExtension(filePath) : Path.GetFileNameWithoutExtension(filePath).ToLower();
            }
            if (!matchWord)// Contain or IS
            {
                if (searchWord_ROM.Length == searchTargetText_FILE.Length)
                {
                    if (searchTargetText_FILE == searchWord_ROM)
                        return true;
                }
                // Contains
                else
                {
                    if (searchmode_Both)
                    {
                        if (searchWord_ROM.Length > searchTargetText_FILE.Length)
                        {
                            if (contains)
                            {
                                if (searchWord_ROM.Contains(searchTargetText_FILE))
                                    return true;
                            }
                            else if (startWith)
                            {
                                if (searchWord_ROM.StartsWith(searchTargetText_FILE))
                                    return true;
                            }
                            else if (endWith)
                            {
                                if (searchWord_ROM.EndsWith(searchTargetText_FILE))
                                    return true;
                            }
                        }
                        else
                        {
                            if (contains)
                            {
                                if (searchTargetText_FILE.Contains(searchWord_ROM))
                                    return true;
                            }
                            else if (startWith)
                            {
                                if (searchTargetText_FILE.StartsWith(searchWord_ROM))
                                    return true;
                            }
                            else if (endWith)
                            {
                                if (searchTargetText_FILE.EndsWith(searchWord_ROM))
                                    return true;
                            }
                        }
                    }
                    else if (searchmode_FileInRom)
                    {
                        if (searchWord_ROM.Length > searchTargetText_FILE.Length)
                        {
                            if (contains)
                            {
                                if (searchWord_ROM.Contains(searchTargetText_FILE))
                                    return true;
                            }
                            else if (startWith)
                            {
                                if (searchWord_ROM.StartsWith(searchTargetText_FILE))
                                    return true;
                            }
                            else if (endWith)
                            {
                                if (searchWord_ROM.EndsWith(searchTargetText_FILE))
                                    return true;
                            }
                        }
                    }
                    else if (searchmode_RomInFile)
                    {
                        if (searchWord_ROM.Length < searchTargetText_FILE.Length)
                        {
                            if (contains)
                            {
                                if (searchTargetText_FILE.Contains(searchWord_ROM))
                                    return true;
                            }
                            else if (startWith)
                            {
                                if (searchTargetText_FILE.StartsWith(searchWord_ROM))
                                    return true;
                            }
                            else if (endWith)
                            {
                                if (searchTargetText_FILE.EndsWith(searchWord_ROM))
                                    return true;
                            }
                        }
                    }
                }

            }
            else// IS
            {
                if (searchWord_ROM.Length == searchTargetText_FILE.Length)
                {
                    if (searchTargetText_FILE == searchWord_ROM)
                        return true;
                }
            }

            return false;
        }
        // Add folder
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.ShowNewFolderButton = true;
            // Set selected directory
            if (listView1.Items.Count > 0)
                fol.SelectedPath = listView1.Items[listView1.Items.Count - 1].Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // Make sure the folder is not already exist in the list
                bool found = false;
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.Text == fol.SelectedPath)
                    {
                        found = true; break;
                    }
                }
                if (!found)
                    listView1.Items.Add(fol.SelectedPath, 0);
            }
        }
        // Remove folder
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            DialogResult res = MessageBox.Show(Properties.Resources.Message13, Properties.Resources.MessageCap2, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].Selected)
                    {
                        listView1.Items.RemoveAt(i);
                        i = -1;
                    }
                }
            }
        }
        // Cancel
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // Start
        private void button3_Click(object sender, EventArgs e)
        {
            // Make a check
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show(Properties.Resources.Message14, Properties.Resources.MessageCap2);
                return;
            }
            if (textBox_extensions.Text.Length == 0)
            {
                MessageBox.Show(Properties.Resources.Message15, Properties.Resources.MessageCap2);
                return;
            }
            // Get options
            foldersToSearch = new List<string>();
            foreach (ListViewItem item in listView1.Items)
            {
                if (Directory.Exists(item.Text))
                    foldersToSearch.Add(item.Text);
            }
            extensions = new List<string>(textBox_extensions.Text.ToLower().Split(new char[] { ';' }));
            includeSubFolders = checkBox_includeSubFolders.Checked;
            clearOldRomDetectedFiles = checkBox_deleteOldDetected.Checked;
            matchCase = checkBox_matchCase.Checked;
            matchWord = checkBox_matchWord.Checked;
            dontAllowSameFileDetectedByMoreThanOneRom = checkBox_dontAllowMoreThanOneFile.Checked;
            oneFilePerRom = checkBox_oneFilePerRom.Checked;
            useRomNameInsteadRomFileName = checkBox_useRomNameInstead.Checked;
            searchmode_FileInRom = radioButton_searchmode_fileinrom.Checked;
            searchmode_RomInFile = radioButton_searchmode_rominfile.Checked;
            searchmode_Both = radioButton_searchmode_both.Checked;
            startWith = radioButton_startWith.Checked;
            contains = radioButton_contains.Checked;
            endWith = radioButton_endwith.Checked;
            useNameWhenPathNotValid = checkBox_useNameWhenPathNotValid.Checked;
            finished = false;
            // Disable things
            listView1.Enabled = button1.Enabled = button2.Enabled = textBox_extensions.Enabled
            = checkBox_deleteOldDetected.Enabled = checkBox_includeSubFolders.Enabled =
            checkBox_matchCase.Enabled = checkBox_matchWord.Enabled = button3.Enabled =
            checkBox_oneFilePerRom.Enabled = checkBox_dontAllowMoreThanOneFile.Enabled = checkBox_useRomNameInstead.Enabled =
            button5.Enabled = radioButton_searchmode_both.Enabled =
            radioButton_searchmode_fileinrom.Enabled = radioButton_searchmode_rominfile.Enabled =
            radioButton_contains.Enabled = radioButton_endwith.Enabled = radioButton_startWith.Enabled =
            checkBox_useNameWhenPathNotValid.Enabled = false;
            progressBar1.Visible = label_status.Visible = groupBox1.Enabled = groupBox2.Enabled = true;
            timer1.Start();
            button4.Text = "Stop";
            // Start the thread !
            mainThread = new Thread(new ThreadStart(SEARCH));
            mainThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            mainThread.Start();
        }
        // Status timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_status.Text = status;
            progressBar1.Value = process;
        }
        private void Form_Detect_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mainThread != null && !finished)
            {
                if (mainThread.IsAlive)
                {
                    DialogResult res = MessageBox.Show(Properties.Resources.Message5, Properties.Resources.MessageCap2, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        mainThread.Abort();
                    }
                    else
                    { e.Cancel = true; return; }
                }
            }
        }
        // Reset extensions
        private void button5_Click(object sender, EventArgs e)
        {
            switch (mode)
            {
                case DetectMode.SNAPS:
                    {
                        this.Text = Properties.Resources.Desc3;
                        textBox_extensions.Text = ".jpg;.png;.bmp;.gif;.jpeg;.tiff;.tif;.tga;.ico";
                        break;
                    }
                case DetectMode.COVERS:
                    {
                        this.Text = Properties.Resources.Desc4;
                        textBox_extensions.Text = ".jpg;.png;.bmp;.gif;.jpeg;.tiff;.tif;.tga;.ico";
                        break;
                    }
                case DetectMode.INFOS:
                    {
                        this.Text = Properties.Resources.Desc5;
                        textBox_extensions.Text = ".txt;.doc;.rtf";
                        break;
                    }
            }
        }
        private void checkBox_matchWord_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = groupBox2.Enabled = !checkBox_matchWord.Checked;
        }

        private void radioButton_startWith_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
