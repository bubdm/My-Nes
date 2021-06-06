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
using SlimDX.DirectInput;
using SlimDX.XInput;

namespace MyNes
{
    class InputControlShortcuts : IInputSettingsControl
    {
        public InputControlShortcuts()
        {
            InitializeComponent();
        }
        private List<string> deviceGuides;
        private CheckBox checkBox1;
        private Label label2;
        private Button button3;
        private Button button2;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button button1;
        private ComboBox comboBox_device;
        private Label label1;
        private List<SlimDX.DirectInput.DeviceType> deviceTypes;

        private Controller c1 = new Controller(UserIndex.One);
        private Controller c2 = new Controller(UserIndex.Two);
        private Controller c3 = new Controller(UserIndex.Three);
        private Controller c4 = new Controller(UserIndex.Four);

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputControlShortcuts));
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox_device = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listView1
            // 
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items1"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items2"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items3"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items4"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items5"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items6"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items7"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items8"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items9"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items10"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items11"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items12")))});
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox_device
            // 
            resources.ApplyResources(this.comboBox_device, "comboBox_device");
            this.comboBox_device.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_device.FormattingEnabled = true;
            this.comboBox_device.Name = "comboBox_device";
            this.comboBox_device.SelectedIndexChanged += new System.EventHandler(this.comboBox_device_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // InputControlShortcuts
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox_device);
            this.Controls.Add(this.label1);
            this.Name = "InputControlShortcuts";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        public override void LoadSettings()
        {
            RefreshDevices();
            // Load settings
            LoadDevice();
            RefreshList();
            checkBox1.Checked = Program.ControlSettings.ShortcutsAutoSwitchBackToKeyboard;
        }
        public override void SaveSettings()
        {
            Save();
            Program.ControlSettings.ShortcutsAutoSwitchBackToKeyboard = checkBox1.Checked;
        }
        private void RefreshDevices()
        {
            comboBox_device.Items.Clear();
            DirectInput di = new DirectInput();
            deviceGuides = new List<string>();
            deviceTypes = new List<SlimDX.DirectInput.DeviceType>();
            foreach (DeviceInstance ins in di.GetDevices())
            {
                if (ins.Type == SlimDX.DirectInput.DeviceType.Joystick || ins.Type == SlimDX.DirectInput.DeviceType.Keyboard)
                {
                    comboBox_device.Items.Add(ins.InstanceName);
                    deviceGuides.Add(ins.InstanceGuid.ToString());
                    deviceTypes.Add(ins.Type);
                }
            }
            // Add the X inputs devices if available
            if (c1.IsConnected)
            {
                comboBox_device.Items.Add(Properties.Resources.Name1);
                deviceGuides.Add("x-controller-1");
                deviceTypes.Add(SlimDX.DirectInput.DeviceType.Other);
            }
            if (c2.IsConnected)
            {
                comboBox_device.Items.Add(Properties.Resources.Name2);
                deviceGuides.Add("x-controller-2");
                deviceTypes.Add(SlimDX.DirectInput.DeviceType.Other);
            }
            if (c3.IsConnected)
            {
                comboBox_device.Items.Add(Properties.Resources.Name3);
                deviceGuides.Add("x-controller-3");
                deviceTypes.Add(SlimDX.DirectInput.DeviceType.Other);
            }
            if (c4.IsConnected)
            {
                comboBox_device.Items.Add(Properties.Resources.Name4);
                deviceGuides.Add("x-controller-4");
                deviceTypes.Add(SlimDX.DirectInput.DeviceType.Other);
            }
        }
        private void LoadDevice()
        {
            for (int i = 0; i < deviceGuides.Count; i++)
            {
                if (deviceGuides[i].ToLower() == Program.ControlSettings.ShortcutsDeviceGuid.ToLower())
                {
                    // This is it!
                    // Select the device
                    comboBox_device.SelectedIndex = i;
                    break;
                }
            }
        }
        private void RefreshList()
        {
            listView1.Items.Clear();
            if (comboBox_device.SelectedIndex < 0) return;
            for (int i = 0; i < Program.ControlSettings.ShortcutsDevices.Count; i++)
            {
                if (Program.ControlSettings.ShortcutsDevices[i].DeviceGuid.ToLower() ==
                    deviceGuides[comboBox_device.SelectedIndex].ToLower())
                {
                    // This is it!
                    ListViewItem item = new ListViewItem("Shutdown");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyShutDown);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Toggle Pause");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyTogglePause);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Soft Reset");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySoftReset);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Hard Reset");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyHardReset);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Take Snapshot");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyTakeSnapshot);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Save State");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySaveState);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Save State Browser");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySaveStateBrowser);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Save State As");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySaveStateAs);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Load State");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyLoadState);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Load State Browser");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyLoadStateBrowser);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Load State As");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyLoadStateAs);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Toggle Fullscreen");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyToggleFullscreen);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Toggle Turbo");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyTurbo);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Volume Up");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyVolumeUp);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Volume Down");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyVolumeDown);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Toggle Sound Enable");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyToggleSoundEnable);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Record Sound");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyRecordSound);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Toggle Keep Aspect Ratio");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyToggleKeepAspectRatio);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Set State Slot 0");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot0);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Set State Slot 1");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot1);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Set State Slot 2");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot2);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Set State Slot 3");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot3);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Set State Slot 4");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot4);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Set State Slot 5");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot5);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Set State Slot 6");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot6);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Set State Slot 7");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot7);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Set State Slot 8");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot8);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Set State Slot 9");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot9);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Toggle FPS");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyToggleFPS);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Connect/Disconnect 4 Players");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyConnect4Players);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Connect/Disconnect Game Genie");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyConnectGameGenie);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Edit Game Genie Codes");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyGameGenieCodes);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Enable/Disble My Nes Sound Mixer");
                    item.SubItems.Add(Program.ControlSettings.ShortcutsDevices[i].KeyToggleUseSoundMixer);
                    listView1.Items.Add(item);
                    return;
                }
            }
            // Not found !!
            ListViewItem item1 = new ListViewItem("Shutdown");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Toggle Pause");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Soft Reset");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Hard Reset");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Take Snapshot");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Save State");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Save State Browser");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Save State As");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Load State");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Load State Browser");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Load State As");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Toggle Fullscreen");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Toggle Turbo");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Volume Up");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Volume Down");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Toggle Sound Enable");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Record Sound");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Toggle Keep Aspect Ratio");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Set State Slot 0");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Set State Slot 1");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Set State Slot 2");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Set State Slot 3");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Set State Slot 4");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Set State Slot 5");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Set State Slot 6");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Set State Slot 7");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Set State Slot 8");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Set State Slot 9");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Toggle FPS");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Connect/Disconnect 4 Players");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Connect/Disconnect Game Genie");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Edit Game Genie Codes");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Enable/Disble My Nes Sound Mixer");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
        }
        private void Save()
        {
            if (comboBox_device.SelectedIndex < 0)
            {
                Program.ControlSettings.ShortcutsDeviceGuid = "";
                return;
            }
            bool found = false;
            for (int i = 0; i < Program.ControlSettings.ShortcutsDevices.Count; i++)
            {
                if (Program.ControlSettings.ShortcutsDevices[i].DeviceGuid.ToLower() ==
                    deviceGuides[comboBox_device.SelectedIndex].ToLower())
                {
                    Program.ControlSettings.ShortcutsDeviceGuid = Program.ControlSettings.ShortcutsDevices[i].DeviceGuid;
                    found = true;
                    // Add the inputs
                    Program.ControlSettings.ShortcutsDevices[i].KeyShutDown = listView1.Items[0].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyTogglePause = listView1.Items[1].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySoftReset = listView1.Items[2].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyHardReset = listView1.Items[3].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyTakeSnapshot = listView1.Items[4].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySaveState = listView1.Items[5].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySaveStateBrowser = listView1.Items[6].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySaveStateAs = listView1.Items[7].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyLoadState = listView1.Items[8].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyLoadStateBrowser = listView1.Items[9].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyLoadStateAs = listView1.Items[10].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyToggleFullscreen = listView1.Items[11].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyTurbo = listView1.Items[12].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyVolumeUp = listView1.Items[13].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyVolumeDown = listView1.Items[14].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyToggleSoundEnable = listView1.Items[15].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyRecordSound = listView1.Items[16].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyToggleKeepAspectRatio = listView1.Items[17].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot0 = listView1.Items[18].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot1 = listView1.Items[19].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot2 = listView1.Items[20].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot3 = listView1.Items[21].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot4 = listView1.Items[22].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot5 = listView1.Items[23].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot6 = listView1.Items[24].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot7 = listView1.Items[25].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot8 = listView1.Items[26].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeySetStateSlot9 = listView1.Items[27].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyToggleFPS = listView1.Items[28].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyConnect4Players = listView1.Items[29].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyConnectGameGenie = listView1.Items[30].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyGameGenieCodes = listView1.Items[31].SubItems[1].Text;
                    Program.ControlSettings.ShortcutsDevices[i].KeyToggleUseSoundMixer = listView1.Items[32].SubItems[1].Text;
                    break;
                }
            }
            if (!found)
            {
                // Add the device
                Program.ControlSettings.ShortcutsDeviceGuid = deviceGuides[comboBox_device.SelectedIndex];
                InputSettingsShortcuts shortcuts = new InputSettingsShortcuts();
                shortcuts.DeviceGuid = Program.ControlSettings.ShortcutsDeviceGuid;

                shortcuts.KeyShutDown = listView1.Items[0].SubItems[1].Text;
                shortcuts.KeyTogglePause = listView1.Items[1].SubItems[1].Text;
                shortcuts.KeySoftReset = listView1.Items[2].SubItems[1].Text;
                shortcuts.KeyHardReset = listView1.Items[3].SubItems[1].Text;
                shortcuts.KeyTakeSnapshot = listView1.Items[4].SubItems[1].Text;
                shortcuts.KeySaveState = listView1.Items[5].SubItems[1].Text;
                shortcuts.KeySaveStateBrowser = listView1.Items[6].SubItems[1].Text;
                shortcuts.KeySaveStateAs = listView1.Items[7].SubItems[1].Text;
                shortcuts.KeyLoadState = listView1.Items[8].SubItems[1].Text;
                shortcuts.KeyLoadStateBrowser = listView1.Items[9].SubItems[1].Text;
                shortcuts.KeyLoadStateAs = listView1.Items[10].SubItems[1].Text;
                shortcuts.KeyToggleFullscreen = listView1.Items[11].SubItems[1].Text;
                shortcuts.KeyTurbo = listView1.Items[12].SubItems[1].Text;
                shortcuts.KeyVolumeUp = listView1.Items[13].SubItems[1].Text;
                shortcuts.KeyVolumeDown = listView1.Items[14].SubItems[1].Text;
                shortcuts.KeyToggleSoundEnable = listView1.Items[15].SubItems[1].Text;
                shortcuts.KeyRecordSound = listView1.Items[16].SubItems[1].Text;
                shortcuts.KeyToggleKeepAspectRatio = listView1.Items[17].SubItems[1].Text;
                shortcuts.KeySetStateSlot0 = listView1.Items[18].SubItems[1].Text;
                shortcuts.KeySetStateSlot1 = listView1.Items[19].SubItems[1].Text;
                shortcuts.KeySetStateSlot2 = listView1.Items[20].SubItems[1].Text;
                shortcuts.KeySetStateSlot3 = listView1.Items[21].SubItems[1].Text;
                shortcuts.KeySetStateSlot4 = listView1.Items[22].SubItems[1].Text;
                shortcuts.KeySetStateSlot5 = listView1.Items[23].SubItems[1].Text;
                shortcuts.KeySetStateSlot6 = listView1.Items[24].SubItems[1].Text;
                shortcuts.KeySetStateSlot7 = listView1.Items[25].SubItems[1].Text;
                shortcuts.KeySetStateSlot8 = listView1.Items[26].SubItems[1].Text;
                shortcuts.KeySetStateSlot9 = listView1.Items[27].SubItems[1].Text;
                shortcuts.KeyToggleFPS = listView1.Items[28].SubItems[1].Text;
                shortcuts.KeyConnect4Players = listView1.Items[29].SubItems[1].Text;
                shortcuts.KeyConnectGameGenie = listView1.Items[30].SubItems[1].Text;
                shortcuts.KeyGameGenieCodes = listView1.Items[31].SubItems[1].Text;
                shortcuts.KeyToggleUseSoundMixer = listView1.Items[32].SubItems[1].Text;

                Program.ControlSettings.ShortcutsDevices.Add(shortcuts);
            }
        }
        private void SetPlayer()
        {
            FormKey frm = new FormKey(deviceTypes[comboBox_device.SelectedIndex],
                deviceGuides[comboBox_device.SelectedIndex], listView1.SelectedItems[0].Text);
            frm.Location = new Point(this.Parent.Parent.Parent.Location.X + this.Parent.Parent.Location.X + button2.Location.X,
            this.Parent.Parent.Parent.Location.Y + this.Parent.Parent.Location.Y + button2.Location.Y + 30);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                listView1.SelectedItems[0].SubItems[1].Text = frm.InputName;
            }
        }
        private void SetAllPlayer()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                FormKey frm = new FormKey(deviceTypes[comboBox_device.SelectedIndex],
                    deviceGuides[comboBox_device.SelectedIndex],
                    listView1.Items[i].Text);
                frm.Location = new Point(this.Parent.Parent.Parent.Location.X + this.Parent.Parent.Location.X + button3.Location.X,
                this.Parent.Parent.Parent.Location.Y + this.Parent.Parent.Location.Y + button3.Location.Y + 30);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    listView1.Items[i].SubItems[1].Text = frm.InputName;
                }
                else
                {
                    break;
                }
            }
        }
        public override string ToString()
        {
            return "Emulation Shortucts";
        }
        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
        }
        // Refresh
        private void button1_Click(object sender, EventArgs e)
        {
            RefreshDevices();
        }
        // Set
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox_device.SelectedIndex < 0)
            {
                MessageBox.Show(Properties.Resources.Message52);
                return;
            }
            if (listView1.SelectedItems.Count != 1)
                return;

            SetPlayer();
        }
        // Set all
        private void button3_Click(object sender, EventArgs e)
        {
            SetAllPlayer();
        }
        private void comboBox_device_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshList();
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = listView1.SelectedItems.Count == 1;
        }
    }
}
