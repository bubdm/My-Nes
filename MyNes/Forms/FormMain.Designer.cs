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
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStateOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStateOpenRecentToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.launcherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator33 = new System.Windows.Forms.ToolStripSeparator();
            this.takeSnapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.romInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.slotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_slot0 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_slot1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_slot2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_slot3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_slot4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_slot5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_slot6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_slot7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_slot8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_slot9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.saveStateBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStateBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.saveStateAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStateAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.machineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.togglePauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.shutdownEmulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.hardResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.softResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.useEmulationThreadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator21 = new System.Windows.Forms.ToolStripSeparator();
            this.regionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceNTSCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forcePALToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceDendyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.turboToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resolutionUpscaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.keepAspectRatioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoStretchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stretchMultiplyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x1256X240ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x2256X240ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x3256X240ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x4256X240ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x5256X240ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x6256X240ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x7256X240ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x8256X240ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x9256X240ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x10256X240ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showFPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showNotifiacationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vSyncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
            this.fullscreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startGameInFullscreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
            this.frameSkipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableFrameSkipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator32 = new System.Windows.Forms.ToolStripSeparator();
            this.interval2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interval3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interval4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator31 = new System.Windows.Forms.ToolStripSeparator();
            this.rendererToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sDL2SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soundEnabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.volumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator25 = new System.Windows.Forms.ToolStripSeparator();
            this.frequencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hzToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.hzToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.hzToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator45 = new System.Windows.Forms.ToolStripSeparator();
            this.useMixerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.channelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.square1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.square2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.triangleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dMCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.mMC5Square1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mMC5Square2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mMC5PCMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.vRC6Square1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vRC6Square2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vRC6SawToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.sunsoft1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sunsoft5B2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sunsoft5B3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator35 = new System.Windows.Forms.ToolStripSeparator();
            this.namco1631ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.namco1632ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.namco1633ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.namco1634ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.namco1635ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.namco1636ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.namco1637ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.namco1638ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rendererToolStripMenuItem_audio = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator24 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator46 = new System.Windows.Forms.ToolStripSeparator();
            this.recordSoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.player1InputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.player2InputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.player3InputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.player4InputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortcutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.connect4PlayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
            this.enableGameGenieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameGenieCodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseEmulationWhenFocusLostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLauncherAtStartUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator27 = new System.Windows.Forms.ToolStripSeparator();
            this.snapshotFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jPGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bMPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gIFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tIFFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eMFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wMFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eXIFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceSnpashotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator26 = new System.Windows.Forms.ToolStripSeparator();
            this.saveSRAMFileOnEmuShutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownEmuExitMyNesOnEscapePressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator28 = new System.Windows.Forms.ToolStripSeparator();
            this.foldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.myNesWikiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.myNesInSourceforgenetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator37 = new System.Windows.Forms.ToolStripSeparator();
            this.gettingsStartedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator30 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutMyNesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_surface = new System.Windows.Forms.Panel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStateOpenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator44 = new System.Windows.Forms.ToolStripSeparator();
            this.recentToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStateOpenRecentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator38 = new System.Windows.Forms.ToolStripSeparator();
            this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hardResetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.softResetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator39 = new System.Windows.Forms.ToolStripSeparator();
            this.saveStateToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStateToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stateSlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator40 = new System.Windows.Forms.ToolStripSeparator();
            this.fullscreenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.startGameInFullscreenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.keepAspectRationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFPSToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator41 = new System.Windows.Forms.ToolStripSeparator();
            this.soundEnabledToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.volumeUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.volumeDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator42 = new System.Windows.Forms.ToolStripSeparator();
            this.configureInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator43 = new System.Windows.Forms.ToolStripSeparator();
            this.turboToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_mouse_hider = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.stateToolStripMenuItem,
            this.machineToolStripMenuItem,
            this.videoToolStripMenuItem,
            this.audioToolStripMenuItem,
            this.inputToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.loadStateOpenToolStripMenuItem,
            this.recentToolStripMenuItem,
            this.loadStateOpenRecentToolStripMenuItem1,
            this.toolStripSeparator1,
            this.launcherToolStripMenuItem,
            this.toolStripSeparator33,
            this.takeSnapshotToolStripMenuItem,
            this.toolStripSeparator10,
            this.romInfoToolStripMenuItem,
            this.toolStripSeparator9,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            this.fileToolStripMenuItem.DropDownOpening += new System.EventHandler(this.recentToolStripMenuItem_DropDownOpening);
            // 
            // openToolStripMenuItem
            // 
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // loadStateOpenToolStripMenuItem
            // 
            this.loadStateOpenToolStripMenuItem.Name = "loadStateOpenToolStripMenuItem";
            resources.ApplyResources(this.loadStateOpenToolStripMenuItem, "loadStateOpenToolStripMenuItem");
            this.loadStateOpenToolStripMenuItem.Click += new System.EventHandler(this.loadStateOpenToolStripMenuItem_Click);
            // 
            // recentToolStripMenuItem
            // 
            this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
            resources.ApplyResources(this.recentToolStripMenuItem, "recentToolStripMenuItem");
            this.recentToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.recentToolStripMenuItem_DropDownItemClicked);
            // 
            // loadStateOpenRecentToolStripMenuItem1
            // 
            this.loadStateOpenRecentToolStripMenuItem1.Name = "loadStateOpenRecentToolStripMenuItem1";
            resources.ApplyResources(this.loadStateOpenRecentToolStripMenuItem1, "loadStateOpenRecentToolStripMenuItem1");
            this.loadStateOpenRecentToolStripMenuItem1.Click += new System.EventHandler(this.whenOpenARomUsingRecentLoadStatDirectlyToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // launcherToolStripMenuItem
            // 
            this.launcherToolStripMenuItem.Name = "launcherToolStripMenuItem";
            resources.ApplyResources(this.launcherToolStripMenuItem, "launcherToolStripMenuItem");
            this.launcherToolStripMenuItem.Click += new System.EventHandler(this.launcherToolStripMenuItem_Click);
            // 
            // toolStripSeparator33
            // 
            this.toolStripSeparator33.Name = "toolStripSeparator33";
            resources.ApplyResources(this.toolStripSeparator33, "toolStripSeparator33");
            // 
            // takeSnapshotToolStripMenuItem
            // 
            resources.ApplyResources(this.takeSnapshotToolStripMenuItem, "takeSnapshotToolStripMenuItem");
            this.takeSnapshotToolStripMenuItem.Name = "takeSnapshotToolStripMenuItem";
            this.takeSnapshotToolStripMenuItem.Click += new System.EventHandler(this.takeSnapshotToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // romInfoToolStripMenuItem
            // 
            resources.ApplyResources(this.romInfoToolStripMenuItem, "romInfoToolStripMenuItem");
            this.romInfoToolStripMenuItem.Name = "romInfoToolStripMenuItem";
            this.romInfoToolStripMenuItem.Click += new System.EventHandler(this.romInfoToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // stateToolStripMenuItem
            // 
            this.stateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveStateToolStripMenuItem,
            this.loadStateToolStripMenuItem,
            this.toolStripSeparator6,
            this.slotToolStripMenuItem,
            this.toolStripSeparator7,
            this.saveStateBrowserToolStripMenuItem,
            this.loadStateBrowserToolStripMenuItem,
            this.toolStripSeparator8,
            this.saveStateAsToolStripMenuItem,
            this.loadStateAsToolStripMenuItem});
            this.stateToolStripMenuItem.Name = "stateToolStripMenuItem";
            resources.ApplyResources(this.stateToolStripMenuItem, "stateToolStripMenuItem");
            this.stateToolStripMenuItem.DropDownOpening += new System.EventHandler(this.stateToolStripMenuItem_DropDownOpening);
            // 
            // saveStateToolStripMenuItem
            // 
            resources.ApplyResources(this.saveStateToolStripMenuItem, "saveStateToolStripMenuItem");
            this.saveStateToolStripMenuItem.Name = "saveStateToolStripMenuItem";
            this.saveStateToolStripMenuItem.Click += new System.EventHandler(this.saveStateToolStripMenuItem_Click);
            // 
            // loadStateToolStripMenuItem
            // 
            resources.ApplyResources(this.loadStateToolStripMenuItem, "loadStateToolStripMenuItem");
            this.loadStateToolStripMenuItem.Name = "loadStateToolStripMenuItem";
            this.loadStateToolStripMenuItem.Click += new System.EventHandler(this.loadStateToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // slotToolStripMenuItem
            // 
            this.slotToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_slot0,
            this.toolStripMenuItem_slot1,
            this.toolStripMenuItem_slot2,
            this.toolStripMenuItem_slot3,
            this.toolStripMenuItem_slot4,
            this.toolStripMenuItem_slot5,
            this.toolStripMenuItem_slot6,
            this.toolStripMenuItem_slot7,
            this.toolStripMenuItem_slot8,
            this.toolStripMenuItem_slot9});
            this.slotToolStripMenuItem.Name = "slotToolStripMenuItem";
            resources.ApplyResources(this.slotToolStripMenuItem, "slotToolStripMenuItem");
            this.slotToolStripMenuItem.DropDownOpening += new System.EventHandler(this.slotToolStripMenuItem_DropDownOpening);
            this.slotToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.slotToolStripMenuItem_DropDownItemClicked);
            // 
            // toolStripMenuItem_slot0
            // 
            this.toolStripMenuItem_slot0.Name = "toolStripMenuItem_slot0";
            resources.ApplyResources(this.toolStripMenuItem_slot0, "toolStripMenuItem_slot0");
            // 
            // toolStripMenuItem_slot1
            // 
            this.toolStripMenuItem_slot1.Name = "toolStripMenuItem_slot1";
            resources.ApplyResources(this.toolStripMenuItem_slot1, "toolStripMenuItem_slot1");
            // 
            // toolStripMenuItem_slot2
            // 
            this.toolStripMenuItem_slot2.Name = "toolStripMenuItem_slot2";
            resources.ApplyResources(this.toolStripMenuItem_slot2, "toolStripMenuItem_slot2");
            // 
            // toolStripMenuItem_slot3
            // 
            this.toolStripMenuItem_slot3.Name = "toolStripMenuItem_slot3";
            resources.ApplyResources(this.toolStripMenuItem_slot3, "toolStripMenuItem_slot3");
            // 
            // toolStripMenuItem_slot4
            // 
            this.toolStripMenuItem_slot4.Name = "toolStripMenuItem_slot4";
            resources.ApplyResources(this.toolStripMenuItem_slot4, "toolStripMenuItem_slot4");
            // 
            // toolStripMenuItem_slot5
            // 
            this.toolStripMenuItem_slot5.Name = "toolStripMenuItem_slot5";
            resources.ApplyResources(this.toolStripMenuItem_slot5, "toolStripMenuItem_slot5");
            // 
            // toolStripMenuItem_slot6
            // 
            this.toolStripMenuItem_slot6.Name = "toolStripMenuItem_slot6";
            resources.ApplyResources(this.toolStripMenuItem_slot6, "toolStripMenuItem_slot6");
            // 
            // toolStripMenuItem_slot7
            // 
            this.toolStripMenuItem_slot7.Name = "toolStripMenuItem_slot7";
            resources.ApplyResources(this.toolStripMenuItem_slot7, "toolStripMenuItem_slot7");
            // 
            // toolStripMenuItem_slot8
            // 
            this.toolStripMenuItem_slot8.Name = "toolStripMenuItem_slot8";
            resources.ApplyResources(this.toolStripMenuItem_slot8, "toolStripMenuItem_slot8");
            // 
            // toolStripMenuItem_slot9
            // 
            this.toolStripMenuItem_slot9.Name = "toolStripMenuItem_slot9";
            resources.ApplyResources(this.toolStripMenuItem_slot9, "toolStripMenuItem_slot9");
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // saveStateBrowserToolStripMenuItem
            // 
            this.saveStateBrowserToolStripMenuItem.Name = "saveStateBrowserToolStripMenuItem";
            resources.ApplyResources(this.saveStateBrowserToolStripMenuItem, "saveStateBrowserToolStripMenuItem");
            this.saveStateBrowserToolStripMenuItem.Click += new System.EventHandler(this.saveStateBrowserToolStripMenuItem_Click);
            // 
            // loadStateBrowserToolStripMenuItem
            // 
            this.loadStateBrowserToolStripMenuItem.Name = "loadStateBrowserToolStripMenuItem";
            resources.ApplyResources(this.loadStateBrowserToolStripMenuItem, "loadStateBrowserToolStripMenuItem");
            this.loadStateBrowserToolStripMenuItem.Click += new System.EventHandler(this.loadStateBrowserToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // saveStateAsToolStripMenuItem
            // 
            this.saveStateAsToolStripMenuItem.Name = "saveStateAsToolStripMenuItem";
            resources.ApplyResources(this.saveStateAsToolStripMenuItem, "saveStateAsToolStripMenuItem");
            this.saveStateAsToolStripMenuItem.Click += new System.EventHandler(this.saveStateAsToolStripMenuItem_Click);
            // 
            // loadStateAsToolStripMenuItem
            // 
            this.loadStateAsToolStripMenuItem.Name = "loadStateAsToolStripMenuItem";
            resources.ApplyResources(this.loadStateAsToolStripMenuItem, "loadStateAsToolStripMenuItem");
            this.loadStateAsToolStripMenuItem.Click += new System.EventHandler(this.loadStateAsToolStripMenuItem_Click);
            // 
            // machineToolStripMenuItem
            // 
            this.machineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.togglePauseToolStripMenuItem,
            this.toolStripSeparator5,
            this.shutdownEmulationToolStripMenuItem,
            this.toolStripSeparator4,
            this.hardResetToolStripMenuItem,
            this.softResetToolStripMenuItem,
            this.toolStripSeparator17,
            this.useEmulationThreadToolStripMenuItem,
            this.toolStripSeparator21,
            this.regionToolStripMenuItem,
            this.toolStripSeparator11,
            this.turboToolStripMenuItem});
            this.machineToolStripMenuItem.Name = "machineToolStripMenuItem";
            resources.ApplyResources(this.machineToolStripMenuItem, "machineToolStripMenuItem");
            this.machineToolStripMenuItem.DropDownOpening += new System.EventHandler(this.machineToolStripMenuItem_DropDownOpening);
            // 
            // togglePauseToolStripMenuItem
            // 
            resources.ApplyResources(this.togglePauseToolStripMenuItem, "togglePauseToolStripMenuItem");
            this.togglePauseToolStripMenuItem.Name = "togglePauseToolStripMenuItem";
            this.togglePauseToolStripMenuItem.Click += new System.EventHandler(this.togglePauseToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // shutdownEmulationToolStripMenuItem
            // 
            resources.ApplyResources(this.shutdownEmulationToolStripMenuItem, "shutdownEmulationToolStripMenuItem");
            this.shutdownEmulationToolStripMenuItem.Name = "shutdownEmulationToolStripMenuItem";
            this.shutdownEmulationToolStripMenuItem.Click += new System.EventHandler(this.shutdownEmulationToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // hardResetToolStripMenuItem
            // 
            resources.ApplyResources(this.hardResetToolStripMenuItem, "hardResetToolStripMenuItem");
            this.hardResetToolStripMenuItem.Name = "hardResetToolStripMenuItem";
            this.hardResetToolStripMenuItem.Click += new System.EventHandler(this.hardResetToolStripMenuItem_Click);
            // 
            // softResetToolStripMenuItem
            // 
            resources.ApplyResources(this.softResetToolStripMenuItem, "softResetToolStripMenuItem");
            this.softResetToolStripMenuItem.Name = "softResetToolStripMenuItem";
            this.softResetToolStripMenuItem.Click += new System.EventHandler(this.softResetToolStripMenuItem_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            resources.ApplyResources(this.toolStripSeparator17, "toolStripSeparator17");
            // 
            // useEmulationThreadToolStripMenuItem
            // 
            this.useEmulationThreadToolStripMenuItem.Name = "useEmulationThreadToolStripMenuItem";
            resources.ApplyResources(this.useEmulationThreadToolStripMenuItem, "useEmulationThreadToolStripMenuItem");
            this.useEmulationThreadToolStripMenuItem.Click += new System.EventHandler(this.useEmulationThreadToolStripMenuItem_Click);
            // 
            // toolStripSeparator21
            // 
            this.toolStripSeparator21.Name = "toolStripSeparator21";
            resources.ApplyResources(this.toolStripSeparator21, "toolStripSeparator21");
            // 
            // regionToolStripMenuItem
            // 
            this.regionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoToolStripMenuItem,
            this.forceNTSCToolStripMenuItem,
            this.forcePALToolStripMenuItem,
            this.forceDendyToolStripMenuItem});
            this.regionToolStripMenuItem.Name = "regionToolStripMenuItem";
            resources.ApplyResources(this.regionToolStripMenuItem, "regionToolStripMenuItem");
            this.regionToolStripMenuItem.DropDownOpening += new System.EventHandler(this.regionToolStripMenuItem_DropDownOpening);
            // 
            // autoToolStripMenuItem
            // 
            this.autoToolStripMenuItem.Name = "autoToolStripMenuItem";
            resources.ApplyResources(this.autoToolStripMenuItem, "autoToolStripMenuItem");
            this.autoToolStripMenuItem.Click += new System.EventHandler(this.autoToolStripMenuItem_Click);
            // 
            // forceNTSCToolStripMenuItem
            // 
            this.forceNTSCToolStripMenuItem.Name = "forceNTSCToolStripMenuItem";
            resources.ApplyResources(this.forceNTSCToolStripMenuItem, "forceNTSCToolStripMenuItem");
            this.forceNTSCToolStripMenuItem.Click += new System.EventHandler(this.forceNTSCToolStripMenuItem_Click);
            // 
            // forcePALToolStripMenuItem
            // 
            this.forcePALToolStripMenuItem.Name = "forcePALToolStripMenuItem";
            resources.ApplyResources(this.forcePALToolStripMenuItem, "forcePALToolStripMenuItem");
            this.forcePALToolStripMenuItem.Click += new System.EventHandler(this.forcePALToolStripMenuItem_Click);
            // 
            // forceDendyToolStripMenuItem
            // 
            this.forceDendyToolStripMenuItem.Name = "forceDendyToolStripMenuItem";
            resources.ApplyResources(this.forceDendyToolStripMenuItem, "forceDendyToolStripMenuItem");
            this.forceDendyToolStripMenuItem.Click += new System.EventHandler(this.forceDendyToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            // 
            // turboToolStripMenuItem
            // 
            this.turboToolStripMenuItem.Name = "turboToolStripMenuItem";
            resources.ApplyResources(this.turboToolStripMenuItem, "turboToolStripMenuItem");
            this.turboToolStripMenuItem.Click += new System.EventHandler(this.turboToolStripMenuItem_Click);
            // 
            // videoToolStripMenuItem
            // 
            this.videoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resolutionUpscaleToolStripMenuItem,
            this.resolutionToolStripMenuItem,
            this.toolStripSeparator3,
            this.keepAspectRatioToolStripMenuItem,
            this.autoStretchToolStripMenuItem,
            this.stretchMultiplyToolStripMenuItem,
            this.toolStripSeparator2,
            this.showFPSToolStripMenuItem,
            this.showNotifiacationsToolStripMenuItem,
            this.toolStripSeparator19,
            this.filterToolStripMenuItem,
            this.vSyncToolStripMenuItem,
            this.toolStripSeparator20,
            this.fullscreenToolStripMenuItem,
            this.startGameInFullscreenToolStripMenuItem,
            this.toolStripSeparator22,
            this.frameSkipToolStripMenuItem,
            this.toolStripSeparator31,
            this.rendererToolStripMenuItem,
            this.sDL2SettingsToolStripMenuItem});
            this.videoToolStripMenuItem.Name = "videoToolStripMenuItem";
            resources.ApplyResources(this.videoToolStripMenuItem, "videoToolStripMenuItem");
            this.videoToolStripMenuItem.DropDownOpening += new System.EventHandler(this.videoToolStripMenuItem_DropDownOpening);
            // 
            // resolutionUpscaleToolStripMenuItem
            // 
            this.resolutionUpscaleToolStripMenuItem.Name = "resolutionUpscaleToolStripMenuItem";
            resources.ApplyResources(this.resolutionUpscaleToolStripMenuItem, "resolutionUpscaleToolStripMenuItem");
            this.resolutionUpscaleToolStripMenuItem.Click += new System.EventHandler(this.resolutionUpscaleToolStripMenuItem_Click);
            // 
            // resolutionToolStripMenuItem
            // 
            this.resolutionToolStripMenuItem.Name = "resolutionToolStripMenuItem";
            resources.ApplyResources(this.resolutionToolStripMenuItem, "resolutionToolStripMenuItem");
            this.resolutionToolStripMenuItem.DropDownOpening += new System.EventHandler(this.resolutionToolStripMenuItem_DropDownOpening);
            this.resolutionToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.resolutionToolStripMenuItem_DropDownItemClicked);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // keepAspectRatioToolStripMenuItem
            // 
            this.keepAspectRatioToolStripMenuItem.Name = "keepAspectRatioToolStripMenuItem";
            resources.ApplyResources(this.keepAspectRatioToolStripMenuItem, "keepAspectRatioToolStripMenuItem");
            this.keepAspectRatioToolStripMenuItem.Click += new System.EventHandler(this.keepAspectRatioToolStripMenuItem_Click);
            // 
            // autoStretchToolStripMenuItem
            // 
            this.autoStretchToolStripMenuItem.Name = "autoStretchToolStripMenuItem";
            resources.ApplyResources(this.autoStretchToolStripMenuItem, "autoStretchToolStripMenuItem");
            this.autoStretchToolStripMenuItem.Click += new System.EventHandler(this.autoStretchToolStripMenuItem_Click);
            // 
            // stretchMultiplyToolStripMenuItem
            // 
            this.stretchMultiplyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x1256X240ToolStripMenuItem,
            this.x2256X240ToolStripMenuItem,
            this.x3256X240ToolStripMenuItem,
            this.x4256X240ToolStripMenuItem,
            this.x5256X240ToolStripMenuItem,
            this.x6256X240ToolStripMenuItem,
            this.x7256X240ToolStripMenuItem,
            this.x8256X240ToolStripMenuItem,
            this.x9256X240ToolStripMenuItem,
            this.x10256X240ToolStripMenuItem});
            this.stretchMultiplyToolStripMenuItem.Name = "stretchMultiplyToolStripMenuItem";
            resources.ApplyResources(this.stretchMultiplyToolStripMenuItem, "stretchMultiplyToolStripMenuItem");
            this.stretchMultiplyToolStripMenuItem.DropDownOpening += new System.EventHandler(this.stretchMultiplyToolStripMenuItem_DropDownOpening);
            this.stretchMultiplyToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.stretchMultiplyToolStripMenuItem_DropDownItemClicked);
            // 
            // x1256X240ToolStripMenuItem
            // 
            this.x1256X240ToolStripMenuItem.Name = "x1256X240ToolStripMenuItem";
            resources.ApplyResources(this.x1256X240ToolStripMenuItem, "x1256X240ToolStripMenuItem");
            // 
            // x2256X240ToolStripMenuItem
            // 
            this.x2256X240ToolStripMenuItem.Name = "x2256X240ToolStripMenuItem";
            resources.ApplyResources(this.x2256X240ToolStripMenuItem, "x2256X240ToolStripMenuItem");
            // 
            // x3256X240ToolStripMenuItem
            // 
            this.x3256X240ToolStripMenuItem.Name = "x3256X240ToolStripMenuItem";
            resources.ApplyResources(this.x3256X240ToolStripMenuItem, "x3256X240ToolStripMenuItem");
            // 
            // x4256X240ToolStripMenuItem
            // 
            this.x4256X240ToolStripMenuItem.Name = "x4256X240ToolStripMenuItem";
            resources.ApplyResources(this.x4256X240ToolStripMenuItem, "x4256X240ToolStripMenuItem");
            // 
            // x5256X240ToolStripMenuItem
            // 
            this.x5256X240ToolStripMenuItem.Name = "x5256X240ToolStripMenuItem";
            resources.ApplyResources(this.x5256X240ToolStripMenuItem, "x5256X240ToolStripMenuItem");
            // 
            // x6256X240ToolStripMenuItem
            // 
            this.x6256X240ToolStripMenuItem.Name = "x6256X240ToolStripMenuItem";
            resources.ApplyResources(this.x6256X240ToolStripMenuItem, "x6256X240ToolStripMenuItem");
            // 
            // x7256X240ToolStripMenuItem
            // 
            this.x7256X240ToolStripMenuItem.Name = "x7256X240ToolStripMenuItem";
            resources.ApplyResources(this.x7256X240ToolStripMenuItem, "x7256X240ToolStripMenuItem");
            // 
            // x8256X240ToolStripMenuItem
            // 
            this.x8256X240ToolStripMenuItem.Name = "x8256X240ToolStripMenuItem";
            resources.ApplyResources(this.x8256X240ToolStripMenuItem, "x8256X240ToolStripMenuItem");
            // 
            // x9256X240ToolStripMenuItem
            // 
            this.x9256X240ToolStripMenuItem.Name = "x9256X240ToolStripMenuItem";
            resources.ApplyResources(this.x9256X240ToolStripMenuItem, "x9256X240ToolStripMenuItem");
            // 
            // x10256X240ToolStripMenuItem
            // 
            this.x10256X240ToolStripMenuItem.Name = "x10256X240ToolStripMenuItem";
            resources.ApplyResources(this.x10256X240ToolStripMenuItem, "x10256X240ToolStripMenuItem");
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // showFPSToolStripMenuItem
            // 
            this.showFPSToolStripMenuItem.Name = "showFPSToolStripMenuItem";
            resources.ApplyResources(this.showFPSToolStripMenuItem, "showFPSToolStripMenuItem");
            this.showFPSToolStripMenuItem.Click += new System.EventHandler(this.showFPSToolStripMenuItem_Click);
            // 
            // showNotifiacationsToolStripMenuItem
            // 
            this.showNotifiacationsToolStripMenuItem.Name = "showNotifiacationsToolStripMenuItem";
            resources.ApplyResources(this.showNotifiacationsToolStripMenuItem, "showNotifiacationsToolStripMenuItem");
            this.showNotifiacationsToolStripMenuItem.Click += new System.EventHandler(this.showNotifiacationsToolStripMenuItem_Click);
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            resources.ApplyResources(this.toolStripSeparator19, "toolStripSeparator19");
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pointToolStripMenuItem,
            this.linearToolStripMenuItem});
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            resources.ApplyResources(this.filterToolStripMenuItem, "filterToolStripMenuItem");
            this.filterToolStripMenuItem.DropDownOpening += new System.EventHandler(this.filterToolStripMenuItem_DropDownOpening);
            // 
            // pointToolStripMenuItem
            // 
            this.pointToolStripMenuItem.Name = "pointToolStripMenuItem";
            resources.ApplyResources(this.pointToolStripMenuItem, "pointToolStripMenuItem");
            this.pointToolStripMenuItem.Click += new System.EventHandler(this.pointToolStripMenuItem_Click);
            // 
            // linearToolStripMenuItem
            // 
            this.linearToolStripMenuItem.Name = "linearToolStripMenuItem";
            resources.ApplyResources(this.linearToolStripMenuItem, "linearToolStripMenuItem");
            this.linearToolStripMenuItem.Click += new System.EventHandler(this.linearToolStripMenuItem_Click);
            // 
            // vSyncToolStripMenuItem
            // 
            this.vSyncToolStripMenuItem.Name = "vSyncToolStripMenuItem";
            resources.ApplyResources(this.vSyncToolStripMenuItem, "vSyncToolStripMenuItem");
            this.vSyncToolStripMenuItem.Click += new System.EventHandler(this.vSyncToolStripMenuItem_Click);
            // 
            // toolStripSeparator20
            // 
            this.toolStripSeparator20.Name = "toolStripSeparator20";
            resources.ApplyResources(this.toolStripSeparator20, "toolStripSeparator20");
            // 
            // fullscreenToolStripMenuItem
            // 
            this.fullscreenToolStripMenuItem.Name = "fullscreenToolStripMenuItem";
            resources.ApplyResources(this.fullscreenToolStripMenuItem, "fullscreenToolStripMenuItem");
            this.fullscreenToolStripMenuItem.Click += new System.EventHandler(this.fullscreenToolStripMenuItem_Click);
            // 
            // startGameInFullscreenToolStripMenuItem
            // 
            this.startGameInFullscreenToolStripMenuItem.Name = "startGameInFullscreenToolStripMenuItem";
            resources.ApplyResources(this.startGameInFullscreenToolStripMenuItem, "startGameInFullscreenToolStripMenuItem");
            this.startGameInFullscreenToolStripMenuItem.Click += new System.EventHandler(this.startGameInFullscreenToolStripMenuItem_Click);
            // 
            // toolStripSeparator22
            // 
            this.toolStripSeparator22.Name = "toolStripSeparator22";
            resources.ApplyResources(this.toolStripSeparator22, "toolStripSeparator22");
            // 
            // frameSkipToolStripMenuItem
            // 
            this.frameSkipToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableFrameSkipToolStripMenuItem,
            this.toolStripSeparator32,
            this.interval2ToolStripMenuItem,
            this.interval3ToolStripMenuItem,
            this.interval4ToolStripMenuItem});
            this.frameSkipToolStripMenuItem.Name = "frameSkipToolStripMenuItem";
            resources.ApplyResources(this.frameSkipToolStripMenuItem, "frameSkipToolStripMenuItem");
            this.frameSkipToolStripMenuItem.DropDownOpening += new System.EventHandler(this.frameSkipToolStripMenuItem_DropDownOpening);
            // 
            // enableFrameSkipToolStripMenuItem
            // 
            this.enableFrameSkipToolStripMenuItem.Name = "enableFrameSkipToolStripMenuItem";
            resources.ApplyResources(this.enableFrameSkipToolStripMenuItem, "enableFrameSkipToolStripMenuItem");
            this.enableFrameSkipToolStripMenuItem.Click += new System.EventHandler(this.enableFrameSkipToolStripMenuItem_Click);
            // 
            // toolStripSeparator32
            // 
            this.toolStripSeparator32.Name = "toolStripSeparator32";
            resources.ApplyResources(this.toolStripSeparator32, "toolStripSeparator32");
            // 
            // interval2ToolStripMenuItem
            // 
            this.interval2ToolStripMenuItem.Name = "interval2ToolStripMenuItem";
            resources.ApplyResources(this.interval2ToolStripMenuItem, "interval2ToolStripMenuItem");
            this.interval2ToolStripMenuItem.Click += new System.EventHandler(this.interval2ToolStripMenuItem_Click);
            // 
            // interval3ToolStripMenuItem
            // 
            this.interval3ToolStripMenuItem.Name = "interval3ToolStripMenuItem";
            resources.ApplyResources(this.interval3ToolStripMenuItem, "interval3ToolStripMenuItem");
            this.interval3ToolStripMenuItem.Click += new System.EventHandler(this.interval3ToolStripMenuItem_Click);
            // 
            // interval4ToolStripMenuItem
            // 
            this.interval4ToolStripMenuItem.Name = "interval4ToolStripMenuItem";
            resources.ApplyResources(this.interval4ToolStripMenuItem, "interval4ToolStripMenuItem");
            this.interval4ToolStripMenuItem.Click += new System.EventHandler(this.interval4ToolStripMenuItem_Click);
            // 
            // toolStripSeparator31
            // 
            this.toolStripSeparator31.Name = "toolStripSeparator31";
            resources.ApplyResources(this.toolStripSeparator31, "toolStripSeparator31");
            // 
            // rendererToolStripMenuItem
            // 
            this.rendererToolStripMenuItem.Name = "rendererToolStripMenuItem";
            resources.ApplyResources(this.rendererToolStripMenuItem, "rendererToolStripMenuItem");
            this.rendererToolStripMenuItem.DropDownOpening += new System.EventHandler(this.rendererToolStripMenuItem_DropDownOpening);
            this.rendererToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.rendererToolStripMenuItem_DropDownItemClicked);
            // 
            // sDL2SettingsToolStripMenuItem
            // 
            this.sDL2SettingsToolStripMenuItem.Name = "sDL2SettingsToolStripMenuItem";
            resources.ApplyResources(this.sDL2SettingsToolStripMenuItem, "sDL2SettingsToolStripMenuItem");
            this.sDL2SettingsToolStripMenuItem.Click += new System.EventHandler(this.sDL2SettingsToolStripMenuItem_Click);
            // 
            // audioToolStripMenuItem
            // 
            this.audioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.soundEnabledToolStripMenuItem,
            this.volumeToolStripMenuItem,
            this.toolStripSeparator25,
            this.frequencyToolStripMenuItem,
            this.toolStripSeparator45,
            this.useMixerToolStripMenuItem,
            this.toolStripSeparator12,
            this.channelsToolStripMenuItem,
            this.rendererToolStripMenuItem_audio,
            this.toolStripSeparator24,
            this.toolStripMenuItem1,
            this.toolStripSeparator46,
            this.recordSoundToolStripMenuItem});
            this.audioToolStripMenuItem.Name = "audioToolStripMenuItem";
            resources.ApplyResources(this.audioToolStripMenuItem, "audioToolStripMenuItem");
            this.audioToolStripMenuItem.DropDownOpening += new System.EventHandler(this.audioToolStripMenuItem_DropDownOpening);
            // 
            // soundEnabledToolStripMenuItem
            // 
            this.soundEnabledToolStripMenuItem.Name = "soundEnabledToolStripMenuItem";
            resources.ApplyResources(this.soundEnabledToolStripMenuItem, "soundEnabledToolStripMenuItem");
            this.soundEnabledToolStripMenuItem.Click += new System.EventHandler(this.soundEnabledToolStripMenuItem_Click);
            // 
            // volumeToolStripMenuItem
            // 
            this.volumeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.upToolStripMenuItem,
            this.downToolStripMenuItem});
            this.volumeToolStripMenuItem.Name = "volumeToolStripMenuItem";
            resources.ApplyResources(this.volumeToolStripMenuItem, "volumeToolStripMenuItem");
            // 
            // upToolStripMenuItem
            // 
            this.upToolStripMenuItem.Name = "upToolStripMenuItem";
            resources.ApplyResources(this.upToolStripMenuItem, "upToolStripMenuItem");
            this.upToolStripMenuItem.Click += new System.EventHandler(this.upToolStripMenuItem_Click);
            // 
            // downToolStripMenuItem
            // 
            this.downToolStripMenuItem.Name = "downToolStripMenuItem";
            resources.ApplyResources(this.downToolStripMenuItem, "downToolStripMenuItem");
            this.downToolStripMenuItem.Click += new System.EventHandler(this.downToolStripMenuItem_Click);
            // 
            // toolStripSeparator25
            // 
            this.toolStripSeparator25.Name = "toolStripSeparator25";
            resources.ApplyResources(this.toolStripSeparator25, "toolStripSeparator25");
            // 
            // frequencyToolStripMenuItem
            // 
            this.frequencyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hzToolStripMenuItem,
            this.hzToolStripMenuItem1,
            this.hzToolStripMenuItem2,
            this.hzToolStripMenuItem3});
            this.frequencyToolStripMenuItem.Name = "frequencyToolStripMenuItem";
            resources.ApplyResources(this.frequencyToolStripMenuItem, "frequencyToolStripMenuItem");
            this.frequencyToolStripMenuItem.DropDownOpening += new System.EventHandler(this.frequencyToolStripMenuItem_DropDownOpening);
            // 
            // hzToolStripMenuItem
            // 
            this.hzToolStripMenuItem.Name = "hzToolStripMenuItem";
            resources.ApplyResources(this.hzToolStripMenuItem, "hzToolStripMenuItem");
            this.hzToolStripMenuItem.Click += new System.EventHandler(this.hzToolStripMenuItem_11025_Click);
            // 
            // hzToolStripMenuItem1
            // 
            this.hzToolStripMenuItem1.Name = "hzToolStripMenuItem1";
            resources.ApplyResources(this.hzToolStripMenuItem1, "hzToolStripMenuItem1");
            this.hzToolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem_22050_Click);
            // 
            // hzToolStripMenuItem2
            // 
            this.hzToolStripMenuItem2.Name = "hzToolStripMenuItem2";
            resources.ApplyResources(this.hzToolStripMenuItem2, "hzToolStripMenuItem2");
            this.hzToolStripMenuItem2.Click += new System.EventHandler(this.hzToolStripMenuItem_44100_Click);
            // 
            // hzToolStripMenuItem3
            // 
            this.hzToolStripMenuItem3.Name = "hzToolStripMenuItem3";
            resources.ApplyResources(this.hzToolStripMenuItem3, "hzToolStripMenuItem3");
            this.hzToolStripMenuItem3.Click += new System.EventHandler(this.ToolStripMenuItem_48000_hz_Click);
            // 
            // toolStripSeparator45
            // 
            this.toolStripSeparator45.Name = "toolStripSeparator45";
            resources.ApplyResources(this.toolStripSeparator45, "toolStripSeparator45");
            // 
            // useMixerToolStripMenuItem
            // 
            this.useMixerToolStripMenuItem.Name = "useMixerToolStripMenuItem";
            resources.ApplyResources(this.useMixerToolStripMenuItem, "useMixerToolStripMenuItem");
            this.useMixerToolStripMenuItem.Click += new System.EventHandler(this.useMixerToolStripMenuItem_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            // 
            // channelsToolStripMenuItem
            // 
            this.channelsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.square1ToolStripMenuItem,
            this.square2ToolStripMenuItem,
            this.triangleToolStripMenuItem,
            this.noiseToolStripMenuItem,
            this.dMCToolStripMenuItem,
            this.toolStripSeparator13,
            this.mMC5Square1ToolStripMenuItem,
            this.mMC5Square2ToolStripMenuItem,
            this.mMC5PCMToolStripMenuItem,
            this.toolStripSeparator14,
            this.vRC6Square1ToolStripMenuItem,
            this.vRC6Square2ToolStripMenuItem,
            this.vRC6SawToolStripMenuItem,
            this.toolStripSeparator15,
            this.sunsoft1ToolStripMenuItem,
            this.sunsoft5B2ToolStripMenuItem,
            this.sunsoft5B3ToolStripMenuItem,
            this.toolStripSeparator35,
            this.namco1631ToolStripMenuItem,
            this.namco1632ToolStripMenuItem,
            this.namco1633ToolStripMenuItem,
            this.namco1634ToolStripMenuItem,
            this.namco1635ToolStripMenuItem,
            this.namco1636ToolStripMenuItem,
            this.namco1637ToolStripMenuItem,
            this.namco1638ToolStripMenuItem});
            this.channelsToolStripMenuItem.Name = "channelsToolStripMenuItem";
            resources.ApplyResources(this.channelsToolStripMenuItem, "channelsToolStripMenuItem");
            this.channelsToolStripMenuItem.DropDownOpening += new System.EventHandler(this.channelsToolStripMenuItem_DropDownOpening);
            // 
            // square1ToolStripMenuItem
            // 
            this.square1ToolStripMenuItem.Name = "square1ToolStripMenuItem";
            resources.ApplyResources(this.square1ToolStripMenuItem, "square1ToolStripMenuItem");
            this.square1ToolStripMenuItem.Click += new System.EventHandler(this.square1ToolStripMenuItem_Click);
            // 
            // square2ToolStripMenuItem
            // 
            this.square2ToolStripMenuItem.Name = "square2ToolStripMenuItem";
            resources.ApplyResources(this.square2ToolStripMenuItem, "square2ToolStripMenuItem");
            this.square2ToolStripMenuItem.Click += new System.EventHandler(this.square2ToolStripMenuItem_Click);
            // 
            // triangleToolStripMenuItem
            // 
            this.triangleToolStripMenuItem.Name = "triangleToolStripMenuItem";
            resources.ApplyResources(this.triangleToolStripMenuItem, "triangleToolStripMenuItem");
            this.triangleToolStripMenuItem.Click += new System.EventHandler(this.triangleToolStripMenuItem_Click);
            // 
            // noiseToolStripMenuItem
            // 
            this.noiseToolStripMenuItem.Name = "noiseToolStripMenuItem";
            resources.ApplyResources(this.noiseToolStripMenuItem, "noiseToolStripMenuItem");
            this.noiseToolStripMenuItem.Click += new System.EventHandler(this.noiseToolStripMenuItem_Click);
            // 
            // dMCToolStripMenuItem
            // 
            this.dMCToolStripMenuItem.Name = "dMCToolStripMenuItem";
            resources.ApplyResources(this.dMCToolStripMenuItem, "dMCToolStripMenuItem");
            this.dMCToolStripMenuItem.Click += new System.EventHandler(this.dMCToolStripMenuItem_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
            // 
            // mMC5Square1ToolStripMenuItem
            // 
            this.mMC5Square1ToolStripMenuItem.Name = "mMC5Square1ToolStripMenuItem";
            resources.ApplyResources(this.mMC5Square1ToolStripMenuItem, "mMC5Square1ToolStripMenuItem");
            this.mMC5Square1ToolStripMenuItem.Click += new System.EventHandler(this.mMC5Square1ToolStripMenuItem_Click);
            // 
            // mMC5Square2ToolStripMenuItem
            // 
            this.mMC5Square2ToolStripMenuItem.Name = "mMC5Square2ToolStripMenuItem";
            resources.ApplyResources(this.mMC5Square2ToolStripMenuItem, "mMC5Square2ToolStripMenuItem");
            this.mMC5Square2ToolStripMenuItem.Click += new System.EventHandler(this.mMC5Square2ToolStripMenuItem_Click);
            // 
            // mMC5PCMToolStripMenuItem
            // 
            this.mMC5PCMToolStripMenuItem.Name = "mMC5PCMToolStripMenuItem";
            resources.ApplyResources(this.mMC5PCMToolStripMenuItem, "mMC5PCMToolStripMenuItem");
            this.mMC5PCMToolStripMenuItem.Click += new System.EventHandler(this.mMC5PCMToolStripMenuItem_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            resources.ApplyResources(this.toolStripSeparator14, "toolStripSeparator14");
            // 
            // vRC6Square1ToolStripMenuItem
            // 
            this.vRC6Square1ToolStripMenuItem.Name = "vRC6Square1ToolStripMenuItem";
            resources.ApplyResources(this.vRC6Square1ToolStripMenuItem, "vRC6Square1ToolStripMenuItem");
            this.vRC6Square1ToolStripMenuItem.Click += new System.EventHandler(this.vRC6Square1ToolStripMenuItem_Click);
            // 
            // vRC6Square2ToolStripMenuItem
            // 
            this.vRC6Square2ToolStripMenuItem.Name = "vRC6Square2ToolStripMenuItem";
            resources.ApplyResources(this.vRC6Square2ToolStripMenuItem, "vRC6Square2ToolStripMenuItem");
            this.vRC6Square2ToolStripMenuItem.Click += new System.EventHandler(this.vRC6Square2ToolStripMenuItem_Click);
            // 
            // vRC6SawToolStripMenuItem
            // 
            this.vRC6SawToolStripMenuItem.Name = "vRC6SawToolStripMenuItem";
            resources.ApplyResources(this.vRC6SawToolStripMenuItem, "vRC6SawToolStripMenuItem");
            this.vRC6SawToolStripMenuItem.Click += new System.EventHandler(this.vRC6SawToolStripMenuItem_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            resources.ApplyResources(this.toolStripSeparator15, "toolStripSeparator15");
            // 
            // sunsoft1ToolStripMenuItem
            // 
            this.sunsoft1ToolStripMenuItem.Name = "sunsoft1ToolStripMenuItem";
            resources.ApplyResources(this.sunsoft1ToolStripMenuItem, "sunsoft1ToolStripMenuItem");
            this.sunsoft1ToolStripMenuItem.Click += new System.EventHandler(this.sunsoft1ToolStripMenuItem_Click);
            // 
            // sunsoft5B2ToolStripMenuItem
            // 
            this.sunsoft5B2ToolStripMenuItem.Name = "sunsoft5B2ToolStripMenuItem";
            resources.ApplyResources(this.sunsoft5B2ToolStripMenuItem, "sunsoft5B2ToolStripMenuItem");
            this.sunsoft5B2ToolStripMenuItem.Click += new System.EventHandler(this.sunsoft5B2ToolStripMenuItem_Click);
            // 
            // sunsoft5B3ToolStripMenuItem
            // 
            this.sunsoft5B3ToolStripMenuItem.Name = "sunsoft5B3ToolStripMenuItem";
            resources.ApplyResources(this.sunsoft5B3ToolStripMenuItem, "sunsoft5B3ToolStripMenuItem");
            this.sunsoft5B3ToolStripMenuItem.Click += new System.EventHandler(this.sunsoft5B3ToolStripMenuItem_Click);
            // 
            // toolStripSeparator35
            // 
            this.toolStripSeparator35.Name = "toolStripSeparator35";
            resources.ApplyResources(this.toolStripSeparator35, "toolStripSeparator35");
            // 
            // namco1631ToolStripMenuItem
            // 
            this.namco1631ToolStripMenuItem.Name = "namco1631ToolStripMenuItem";
            resources.ApplyResources(this.namco1631ToolStripMenuItem, "namco1631ToolStripMenuItem");
            this.namco1631ToolStripMenuItem.Click += new System.EventHandler(this.namco1631ToolStripMenuItem_Click);
            // 
            // namco1632ToolStripMenuItem
            // 
            this.namco1632ToolStripMenuItem.Name = "namco1632ToolStripMenuItem";
            resources.ApplyResources(this.namco1632ToolStripMenuItem, "namco1632ToolStripMenuItem");
            this.namco1632ToolStripMenuItem.Click += new System.EventHandler(this.namco1632ToolStripMenuItem_Click);
            // 
            // namco1633ToolStripMenuItem
            // 
            this.namco1633ToolStripMenuItem.Name = "namco1633ToolStripMenuItem";
            resources.ApplyResources(this.namco1633ToolStripMenuItem, "namco1633ToolStripMenuItem");
            this.namco1633ToolStripMenuItem.Click += new System.EventHandler(this.namco1633ToolStripMenuItem_Click);
            // 
            // namco1634ToolStripMenuItem
            // 
            this.namco1634ToolStripMenuItem.Name = "namco1634ToolStripMenuItem";
            resources.ApplyResources(this.namco1634ToolStripMenuItem, "namco1634ToolStripMenuItem");
            this.namco1634ToolStripMenuItem.Click += new System.EventHandler(this.namco1634ToolStripMenuItem_Click);
            // 
            // namco1635ToolStripMenuItem
            // 
            this.namco1635ToolStripMenuItem.Name = "namco1635ToolStripMenuItem";
            resources.ApplyResources(this.namco1635ToolStripMenuItem, "namco1635ToolStripMenuItem");
            this.namco1635ToolStripMenuItem.Click += new System.EventHandler(this.namco1635ToolStripMenuItem_Click);
            // 
            // namco1636ToolStripMenuItem
            // 
            this.namco1636ToolStripMenuItem.Name = "namco1636ToolStripMenuItem";
            resources.ApplyResources(this.namco1636ToolStripMenuItem, "namco1636ToolStripMenuItem");
            this.namco1636ToolStripMenuItem.Click += new System.EventHandler(this.namco1636ToolStripMenuItem_Click);
            // 
            // namco1637ToolStripMenuItem
            // 
            this.namco1637ToolStripMenuItem.Name = "namco1637ToolStripMenuItem";
            resources.ApplyResources(this.namco1637ToolStripMenuItem, "namco1637ToolStripMenuItem");
            this.namco1637ToolStripMenuItem.Click += new System.EventHandler(this.namco1637ToolStripMenuItem_Click);
            // 
            // namco1638ToolStripMenuItem
            // 
            this.namco1638ToolStripMenuItem.Name = "namco1638ToolStripMenuItem";
            resources.ApplyResources(this.namco1638ToolStripMenuItem, "namco1638ToolStripMenuItem");
            this.namco1638ToolStripMenuItem.Click += new System.EventHandler(this.namco1638ToolStripMenuItem_Click);
            // 
            // rendererToolStripMenuItem_audio
            // 
            this.rendererToolStripMenuItem_audio.Name = "rendererToolStripMenuItem_audio";
            resources.ApplyResources(this.rendererToolStripMenuItem_audio, "rendererToolStripMenuItem_audio");
            this.rendererToolStripMenuItem_audio.DropDownOpening += new System.EventHandler(this.rendererToolStripMenuItem_audio_DropDownOpening);
            this.rendererToolStripMenuItem_audio.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.rendererToolStripMenuItem_audio_DropDownItemClicked);
            // 
            // toolStripSeparator24
            // 
            this.toolStripSeparator24.Name = "toolStripSeparator24";
            resources.ApplyResources(this.toolStripSeparator24, "toolStripSeparator24");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Click += new System.EventHandler(this.sDL2SettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator46
            // 
            this.toolStripSeparator46.Name = "toolStripSeparator46";
            resources.ApplyResources(this.toolStripSeparator46, "toolStripSeparator46");
            // 
            // recordSoundToolStripMenuItem
            // 
            this.recordSoundToolStripMenuItem.Name = "recordSoundToolStripMenuItem";
            resources.ApplyResources(this.recordSoundToolStripMenuItem, "recordSoundToolStripMenuItem");
            this.recordSoundToolStripMenuItem.Click += new System.EventHandler(this.recordSoundToolStripMenuItem_Click);
            // 
            // inputToolStripMenuItem
            // 
            this.inputToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.player1InputToolStripMenuItem,
            this.player2InputToolStripMenuItem,
            this.player3InputToolStripMenuItem,
            this.player4InputToolStripMenuItem,
            this.shortcutsToolStripMenuItem,
            this.toolStripSeparator18,
            this.connect4PlayersToolStripMenuItem,
            this.toolStripSeparator23,
            this.enableGameGenieToolStripMenuItem,
            this.gameGenieCodesToolStripMenuItem});
            this.inputToolStripMenuItem.Name = "inputToolStripMenuItem";
            resources.ApplyResources(this.inputToolStripMenuItem, "inputToolStripMenuItem");
            this.inputToolStripMenuItem.DropDownOpening += new System.EventHandler(this.inputToolStripMenuItem_DropDownOpening);
            // 
            // player1InputToolStripMenuItem
            // 
            this.player1InputToolStripMenuItem.Image = global::MyNes.Properties.Resources.joystick;
            this.player1InputToolStripMenuItem.Name = "player1InputToolStripMenuItem";
            resources.ApplyResources(this.player1InputToolStripMenuItem, "player1InputToolStripMenuItem");
            this.player1InputToolStripMenuItem.Click += new System.EventHandler(this.player1InputToolStripMenuItem_Click);
            // 
            // player2InputToolStripMenuItem
            // 
            resources.ApplyResources(this.player2InputToolStripMenuItem, "player2InputToolStripMenuItem");
            this.player2InputToolStripMenuItem.Name = "player2InputToolStripMenuItem";
            this.player2InputToolStripMenuItem.Click += new System.EventHandler(this.player2InputToolStripMenuItem_Click);
            // 
            // player3InputToolStripMenuItem
            // 
            resources.ApplyResources(this.player3InputToolStripMenuItem, "player3InputToolStripMenuItem");
            this.player3InputToolStripMenuItem.Name = "player3InputToolStripMenuItem";
            this.player3InputToolStripMenuItem.Click += new System.EventHandler(this.player3InputToolStripMenuItem_Click);
            // 
            // player4InputToolStripMenuItem
            // 
            resources.ApplyResources(this.player4InputToolStripMenuItem, "player4InputToolStripMenuItem");
            this.player4InputToolStripMenuItem.Name = "player4InputToolStripMenuItem";
            this.player4InputToolStripMenuItem.Click += new System.EventHandler(this.player4InputToolStripMenuItem_Click);
            // 
            // shortcutsToolStripMenuItem
            // 
            this.shortcutsToolStripMenuItem.Name = "shortcutsToolStripMenuItem";
            resources.ApplyResources(this.shortcutsToolStripMenuItem, "shortcutsToolStripMenuItem");
            this.shortcutsToolStripMenuItem.Click += new System.EventHandler(this.shortcutsToolStripMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            resources.ApplyResources(this.toolStripSeparator18, "toolStripSeparator18");
            // 
            // connect4PlayersToolStripMenuItem
            // 
            this.connect4PlayersToolStripMenuItem.Name = "connect4PlayersToolStripMenuItem";
            resources.ApplyResources(this.connect4PlayersToolStripMenuItem, "connect4PlayersToolStripMenuItem");
            this.connect4PlayersToolStripMenuItem.Click += new System.EventHandler(this.connect4PlayersToolStripMenuItem_Click);
            // 
            // toolStripSeparator23
            // 
            this.toolStripSeparator23.Name = "toolStripSeparator23";
            resources.ApplyResources(this.toolStripSeparator23, "toolStripSeparator23");
            // 
            // enableGameGenieToolStripMenuItem
            // 
            this.enableGameGenieToolStripMenuItem.Name = "enableGameGenieToolStripMenuItem";
            resources.ApplyResources(this.enableGameGenieToolStripMenuItem, "enableGameGenieToolStripMenuItem");
            this.enableGameGenieToolStripMenuItem.Click += new System.EventHandler(this.enableGameGenieToolStripMenuItem_Click);
            // 
            // gameGenieCodesToolStripMenuItem
            // 
            resources.ApplyResources(this.gameGenieCodesToolStripMenuItem, "gameGenieCodesToolStripMenuItem");
            this.gameGenieCodesToolStripMenuItem.Name = "gameGenieCodesToolStripMenuItem";
            this.gameGenieCodesToolStripMenuItem.Click += new System.EventHandler(this.gameGenieCodesToolStripMenuItem_Click);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pauseEmulationWhenFocusLostToolStripMenuItem,
            this.showLauncherAtStartUpToolStripMenuItem,
            this.toolStripSeparator27,
            this.snapshotFormatToolStripMenuItem,
            this.replaceSnpashotToolStripMenuItem,
            this.toolStripSeparator26,
            this.saveSRAMFileOnEmuShutdownToolStripMenuItem,
            this.shutdownEmuExitMyNesOnEscapePressToolStripMenuItem,
            this.toolStripSeparator28,
            this.foldersToolStripMenuItem});
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            resources.ApplyResources(this.preferencesToolStripMenuItem, "preferencesToolStripMenuItem");
            this.preferencesToolStripMenuItem.DropDownOpening += new System.EventHandler(this.preferencesToolStripMenuItem_DropDownOpening);
            // 
            // pauseEmulationWhenFocusLostToolStripMenuItem
            // 
            this.pauseEmulationWhenFocusLostToolStripMenuItem.Name = "pauseEmulationWhenFocusLostToolStripMenuItem";
            resources.ApplyResources(this.pauseEmulationWhenFocusLostToolStripMenuItem, "pauseEmulationWhenFocusLostToolStripMenuItem");
            this.pauseEmulationWhenFocusLostToolStripMenuItem.Click += new System.EventHandler(this.pauseEmulationWhenFocusLostToolStripMenuItem_Click);
            // 
            // showLauncherAtStartUpToolStripMenuItem
            // 
            this.showLauncherAtStartUpToolStripMenuItem.Name = "showLauncherAtStartUpToolStripMenuItem";
            resources.ApplyResources(this.showLauncherAtStartUpToolStripMenuItem, "showLauncherAtStartUpToolStripMenuItem");
            this.showLauncherAtStartUpToolStripMenuItem.Click += new System.EventHandler(this.showLauncherAtStartUpToolStripMenuItem_Click);
            // 
            // toolStripSeparator27
            // 
            this.toolStripSeparator27.Name = "toolStripSeparator27";
            resources.ApplyResources(this.toolStripSeparator27, "toolStripSeparator27");
            // 
            // snapshotFormatToolStripMenuItem
            // 
            this.snapshotFormatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jPGToolStripMenuItem,
            this.pNGToolStripMenuItem,
            this.bMPToolStripMenuItem,
            this.gIFToolStripMenuItem,
            this.tIFFToolStripMenuItem,
            this.eMFToolStripMenuItem,
            this.wMFToolStripMenuItem,
            this.eXIFToolStripMenuItem});
            this.snapshotFormatToolStripMenuItem.Name = "snapshotFormatToolStripMenuItem";
            resources.ApplyResources(this.snapshotFormatToolStripMenuItem, "snapshotFormatToolStripMenuItem");
            this.snapshotFormatToolStripMenuItem.DropDownOpening += new System.EventHandler(this.snapshotFormatToolStripMenuItem_DropDownOpening);
            // 
            // jPGToolStripMenuItem
            // 
            this.jPGToolStripMenuItem.Name = "jPGToolStripMenuItem";
            resources.ApplyResources(this.jPGToolStripMenuItem, "jPGToolStripMenuItem");
            this.jPGToolStripMenuItem.Click += new System.EventHandler(this.jPGToolStripMenuItem_Click);
            // 
            // pNGToolStripMenuItem
            // 
            this.pNGToolStripMenuItem.Name = "pNGToolStripMenuItem";
            resources.ApplyResources(this.pNGToolStripMenuItem, "pNGToolStripMenuItem");
            this.pNGToolStripMenuItem.Click += new System.EventHandler(this.pNGToolStripMenuItem_Click);
            // 
            // bMPToolStripMenuItem
            // 
            this.bMPToolStripMenuItem.Name = "bMPToolStripMenuItem";
            resources.ApplyResources(this.bMPToolStripMenuItem, "bMPToolStripMenuItem");
            this.bMPToolStripMenuItem.Click += new System.EventHandler(this.bMPToolStripMenuItem_Click);
            // 
            // gIFToolStripMenuItem
            // 
            this.gIFToolStripMenuItem.Name = "gIFToolStripMenuItem";
            resources.ApplyResources(this.gIFToolStripMenuItem, "gIFToolStripMenuItem");
            this.gIFToolStripMenuItem.Click += new System.EventHandler(this.gIFToolStripMenuItem_Click);
            // 
            // tIFFToolStripMenuItem
            // 
            this.tIFFToolStripMenuItem.Name = "tIFFToolStripMenuItem";
            resources.ApplyResources(this.tIFFToolStripMenuItem, "tIFFToolStripMenuItem");
            this.tIFFToolStripMenuItem.Click += new System.EventHandler(this.tIFFToolStripMenuItem_Click);
            // 
            // eMFToolStripMenuItem
            // 
            this.eMFToolStripMenuItem.Name = "eMFToolStripMenuItem";
            resources.ApplyResources(this.eMFToolStripMenuItem, "eMFToolStripMenuItem");
            this.eMFToolStripMenuItem.Click += new System.EventHandler(this.eMFToolStripMenuItem_Click);
            // 
            // wMFToolStripMenuItem
            // 
            this.wMFToolStripMenuItem.Name = "wMFToolStripMenuItem";
            resources.ApplyResources(this.wMFToolStripMenuItem, "wMFToolStripMenuItem");
            this.wMFToolStripMenuItem.Click += new System.EventHandler(this.wMFToolStripMenuItem_Click);
            // 
            // eXIFToolStripMenuItem
            // 
            this.eXIFToolStripMenuItem.Name = "eXIFToolStripMenuItem";
            resources.ApplyResources(this.eXIFToolStripMenuItem, "eXIFToolStripMenuItem");
            this.eXIFToolStripMenuItem.Click += new System.EventHandler(this.eXIFToolStripMenuItem_Click);
            // 
            // replaceSnpashotToolStripMenuItem
            // 
            this.replaceSnpashotToolStripMenuItem.Name = "replaceSnpashotToolStripMenuItem";
            resources.ApplyResources(this.replaceSnpashotToolStripMenuItem, "replaceSnpashotToolStripMenuItem");
            this.replaceSnpashotToolStripMenuItem.Click += new System.EventHandler(this.replaceSnpashotToolStripMenuItem_Click);
            // 
            // toolStripSeparator26
            // 
            this.toolStripSeparator26.Name = "toolStripSeparator26";
            resources.ApplyResources(this.toolStripSeparator26, "toolStripSeparator26");
            // 
            // saveSRAMFileOnEmuShutdownToolStripMenuItem
            // 
            this.saveSRAMFileOnEmuShutdownToolStripMenuItem.Name = "saveSRAMFileOnEmuShutdownToolStripMenuItem";
            resources.ApplyResources(this.saveSRAMFileOnEmuShutdownToolStripMenuItem, "saveSRAMFileOnEmuShutdownToolStripMenuItem");
            this.saveSRAMFileOnEmuShutdownToolStripMenuItem.Click += new System.EventHandler(this.saveSRAMFileOnEmuShutdownToolStripMenuItem_Click);
            // 
            // shutdownEmuExitMyNesOnEscapePressToolStripMenuItem
            // 
            this.shutdownEmuExitMyNesOnEscapePressToolStripMenuItem.Name = "shutdownEmuExitMyNesOnEscapePressToolStripMenuItem";
            resources.ApplyResources(this.shutdownEmuExitMyNesOnEscapePressToolStripMenuItem, "shutdownEmuExitMyNesOnEscapePressToolStripMenuItem");
            this.shutdownEmuExitMyNesOnEscapePressToolStripMenuItem.Click += new System.EventHandler(this.shutdownEmuExitMyNesOnEscapePressToolStripMenuItem_Click);
            // 
            // toolStripSeparator28
            // 
            this.toolStripSeparator28.Name = "toolStripSeparator28";
            resources.ApplyResources(this.toolStripSeparator28, "toolStripSeparator28");
            // 
            // foldersToolStripMenuItem
            // 
            resources.ApplyResources(this.foldersToolStripMenuItem, "foldersToolStripMenuItem");
            this.foldersToolStripMenuItem.Name = "foldersToolStripMenuItem";
            this.foldersToolStripMenuItem.Click += new System.EventHandler(this.foldersToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.myNesWikiToolStripMenuItem,
            this.toolStripSeparator16,
            this.myNesInSourceforgenetToolStripMenuItem,
            this.toolStripSeparator37,
            this.gettingsStartedToolStripMenuItem,
            this.toolStripSeparator30,
            this.aboutMyNesToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // helpToolStripMenuItem1
            // 
            resources.ApplyResources(this.helpToolStripMenuItem1, "helpToolStripMenuItem1");
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // myNesWikiToolStripMenuItem
            // 
            this.myNesWikiToolStripMenuItem.Name = "myNesWikiToolStripMenuItem";
            resources.ApplyResources(this.myNesWikiToolStripMenuItem, "myNesWikiToolStripMenuItem");
            this.myNesWikiToolStripMenuItem.Click += new System.EventHandler(this.myNesWikiToolStripMenuItem_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            resources.ApplyResources(this.toolStripSeparator16, "toolStripSeparator16");
            // 
            // myNesInSourceforgenetToolStripMenuItem
            // 
            this.myNesInSourceforgenetToolStripMenuItem.Name = "myNesInSourceforgenetToolStripMenuItem";
            resources.ApplyResources(this.myNesInSourceforgenetToolStripMenuItem, "myNesInSourceforgenetToolStripMenuItem");
            this.myNesInSourceforgenetToolStripMenuItem.Click += new System.EventHandler(this.myNesInSourceforgenetToolStripMenuItem_Click);
            // 
            // toolStripSeparator37
            // 
            this.toolStripSeparator37.Name = "toolStripSeparator37";
            resources.ApplyResources(this.toolStripSeparator37, "toolStripSeparator37");
            // 
            // gettingsStartedToolStripMenuItem
            // 
            this.gettingsStartedToolStripMenuItem.Name = "gettingsStartedToolStripMenuItem";
            resources.ApplyResources(this.gettingsStartedToolStripMenuItem, "gettingsStartedToolStripMenuItem");
            this.gettingsStartedToolStripMenuItem.Click += new System.EventHandler(this.gettingsStartedToolStripMenuItem_Click);
            // 
            // toolStripSeparator30
            // 
            this.toolStripSeparator30.Name = "toolStripSeparator30";
            resources.ApplyResources(this.toolStripSeparator30, "toolStripSeparator30");
            // 
            // aboutMyNesToolStripMenuItem
            // 
            this.aboutMyNesToolStripMenuItem.Image = global::MyNes.Properties.Resources.MyNes;
            this.aboutMyNesToolStripMenuItem.Name = "aboutMyNesToolStripMenuItem";
            resources.ApplyResources(this.aboutMyNesToolStripMenuItem, "aboutMyNesToolStripMenuItem");
            this.aboutMyNesToolStripMenuItem.Click += new System.EventHandler(this.aboutMyNesToolStripMenuItem_Click);
            // 
            // panel_surface
            // 
            this.panel_surface.BackColor = System.Drawing.Color.Black;
            this.panel_surface.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.panel_surface, "panel_surface");
            this.panel_surface.Name = "panel_surface";
            this.panel_surface.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.panel_surface_MouseDoubleClick);
            this.panel_surface.MouseEnter += new System.EventHandler(this.panel_surface_MouseEnter);
            this.panel_surface.MouseLeave += new System.EventHandler(this.panel_surface_MouseLeave);
            this.panel_surface.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_surface_MouseMove);
            this.panel_surface.Resize += new System.EventHandler(this.panel_surface_Resize);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem1,
            this.loadStateOpenToolStripMenuItem1,
            this.toolStripSeparator44,
            this.recentToolStripMenuItem1,
            this.loadStateOpenRecentToolStripMenuItem,
            this.toolStripSeparator38,
            this.shutdownToolStripMenuItem,
            this.hardResetToolStripMenuItem1,
            this.softResetToolStripMenuItem1,
            this.toolStripSeparator39,
            this.saveStateToolStripMenuItem1,
            this.loadStateToolStripMenuItem1,
            this.stateSlotToolStripMenuItem,
            this.toolStripSeparator40,
            this.fullscreenToolStripMenuItem1,
            this.startGameInFullscreenToolStripMenuItem1,
            this.keepAspectRationToolStripMenuItem,
            this.showFPSToolStripMenuItem1,
            this.toolStripSeparator41,
            this.soundEnabledToolStripMenuItem1,
            this.volumeUpToolStripMenuItem,
            this.volumeDownToolStripMenuItem,
            this.toolStripSeparator42,
            this.configureInputToolStripMenuItem,
            this.toolStripSeparator43,
            this.turboToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.contextMenuStrip1_Closing);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Image = global::MyNes.Properties.Resources.folder;
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            resources.ApplyResources(this.openToolStripMenuItem1, "openToolStripMenuItem1");
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // loadStateOpenToolStripMenuItem1
            // 
            this.loadStateOpenToolStripMenuItem1.Image = global::MyNes.Properties.Resources.folder;
            this.loadStateOpenToolStripMenuItem1.Name = "loadStateOpenToolStripMenuItem1";
            resources.ApplyResources(this.loadStateOpenToolStripMenuItem1, "loadStateOpenToolStripMenuItem1");
            this.loadStateOpenToolStripMenuItem1.Click += new System.EventHandler(this.loadStateOpenToolStripMenuItem_Click);
            // 
            // toolStripSeparator44
            // 
            this.toolStripSeparator44.Name = "toolStripSeparator44";
            resources.ApplyResources(this.toolStripSeparator44, "toolStripSeparator44");
            // 
            // recentToolStripMenuItem1
            // 
            this.recentToolStripMenuItem1.Name = "recentToolStripMenuItem1";
            resources.ApplyResources(this.recentToolStripMenuItem1, "recentToolStripMenuItem1");
            this.recentToolStripMenuItem1.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.recentToolStripMenuItem_DropDownItemClicked);
            // 
            // loadStateOpenRecentToolStripMenuItem
            // 
            this.loadStateOpenRecentToolStripMenuItem.Name = "loadStateOpenRecentToolStripMenuItem";
            resources.ApplyResources(this.loadStateOpenRecentToolStripMenuItem, "loadStateOpenRecentToolStripMenuItem");
            this.loadStateOpenRecentToolStripMenuItem.Click += new System.EventHandler(this.whenOpenARomUsingRecentLoadStatDirectlyToolStripMenuItem_Click);
            // 
            // toolStripSeparator38
            // 
            this.toolStripSeparator38.Name = "toolStripSeparator38";
            resources.ApplyResources(this.toolStripSeparator38, "toolStripSeparator38");
            // 
            // shutdownToolStripMenuItem
            // 
            this.shutdownToolStripMenuItem.Image = global::MyNes.Properties.Resources.control_eject;
            this.shutdownToolStripMenuItem.Name = "shutdownToolStripMenuItem";
            resources.ApplyResources(this.shutdownToolStripMenuItem, "shutdownToolStripMenuItem");
            this.shutdownToolStripMenuItem.Click += new System.EventHandler(this.shutdownEmulationToolStripMenuItem_Click);
            // 
            // hardResetToolStripMenuItem1
            // 
            this.hardResetToolStripMenuItem1.Image = global::MyNes.Properties.Resources.control_repeat;
            this.hardResetToolStripMenuItem1.Name = "hardResetToolStripMenuItem1";
            resources.ApplyResources(this.hardResetToolStripMenuItem1, "hardResetToolStripMenuItem1");
            this.hardResetToolStripMenuItem1.Click += new System.EventHandler(this.hardResetToolStripMenuItem_Click);
            // 
            // softResetToolStripMenuItem1
            // 
            this.softResetToolStripMenuItem1.Image = global::MyNes.Properties.Resources.control_repeat_blue;
            this.softResetToolStripMenuItem1.Name = "softResetToolStripMenuItem1";
            resources.ApplyResources(this.softResetToolStripMenuItem1, "softResetToolStripMenuItem1");
            this.softResetToolStripMenuItem1.Click += new System.EventHandler(this.softResetToolStripMenuItem_Click);
            // 
            // toolStripSeparator39
            // 
            this.toolStripSeparator39.Name = "toolStripSeparator39";
            resources.ApplyResources(this.toolStripSeparator39, "toolStripSeparator39");
            // 
            // saveStateToolStripMenuItem1
            // 
            this.saveStateToolStripMenuItem1.Image = global::MyNes.Properties.Resources.disk;
            this.saveStateToolStripMenuItem1.Name = "saveStateToolStripMenuItem1";
            resources.ApplyResources(this.saveStateToolStripMenuItem1, "saveStateToolStripMenuItem1");
            this.saveStateToolStripMenuItem1.Click += new System.EventHandler(this.saveStateToolStripMenuItem_Click);
            // 
            // loadStateToolStripMenuItem1
            // 
            this.loadStateToolStripMenuItem1.Image = global::MyNes.Properties.Resources.folder_page;
            this.loadStateToolStripMenuItem1.Name = "loadStateToolStripMenuItem1";
            resources.ApplyResources(this.loadStateToolStripMenuItem1, "loadStateToolStripMenuItem1");
            this.loadStateToolStripMenuItem1.Click += new System.EventHandler(this.loadStateToolStripMenuItem_Click);
            // 
            // stateSlotToolStripMenuItem
            // 
            this.stateSlotToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem12,
            this.toolStripMenuItem13,
            this.toolStripMenuItem14,
            this.toolStripMenuItem15,
            this.toolStripMenuItem16,
            this.toolStripMenuItem17,
            this.toolStripMenuItem18,
            this.toolStripMenuItem19,
            this.toolStripMenuItem20,
            this.toolStripMenuItem21});
            this.stateSlotToolStripMenuItem.Name = "stateSlotToolStripMenuItem";
            resources.ApplyResources(this.stateSlotToolStripMenuItem, "stateSlotToolStripMenuItem");
            this.stateSlotToolStripMenuItem.DropDownOpened += new System.EventHandler(this.stateSlotToolStripMenuItem_DropDownOpened);
            this.stateSlotToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.stateSlotToolStripMenuItem_DropDownItemClicked);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            resources.ApplyResources(this.toolStripMenuItem12, "toolStripMenuItem12");
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            resources.ApplyResources(this.toolStripMenuItem13, "toolStripMenuItem13");
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            resources.ApplyResources(this.toolStripMenuItem14, "toolStripMenuItem14");
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            resources.ApplyResources(this.toolStripMenuItem15, "toolStripMenuItem15");
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            resources.ApplyResources(this.toolStripMenuItem16, "toolStripMenuItem16");
            // 
            // toolStripMenuItem17
            // 
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            resources.ApplyResources(this.toolStripMenuItem17, "toolStripMenuItem17");
            // 
            // toolStripMenuItem18
            // 
            this.toolStripMenuItem18.Name = "toolStripMenuItem18";
            resources.ApplyResources(this.toolStripMenuItem18, "toolStripMenuItem18");
            // 
            // toolStripMenuItem19
            // 
            this.toolStripMenuItem19.Name = "toolStripMenuItem19";
            resources.ApplyResources(this.toolStripMenuItem19, "toolStripMenuItem19");
            // 
            // toolStripMenuItem20
            // 
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            resources.ApplyResources(this.toolStripMenuItem20, "toolStripMenuItem20");
            // 
            // toolStripMenuItem21
            // 
            this.toolStripMenuItem21.Name = "toolStripMenuItem21";
            resources.ApplyResources(this.toolStripMenuItem21, "toolStripMenuItem21");
            // 
            // toolStripSeparator40
            // 
            this.toolStripSeparator40.Name = "toolStripSeparator40";
            resources.ApplyResources(this.toolStripSeparator40, "toolStripSeparator40");
            // 
            // fullscreenToolStripMenuItem1
            // 
            this.fullscreenToolStripMenuItem1.Name = "fullscreenToolStripMenuItem1";
            resources.ApplyResources(this.fullscreenToolStripMenuItem1, "fullscreenToolStripMenuItem1");
            this.fullscreenToolStripMenuItem1.Click += new System.EventHandler(this.fullscreenToolStripMenuItem_Click);
            // 
            // startGameInFullscreenToolStripMenuItem1
            // 
            this.startGameInFullscreenToolStripMenuItem1.Name = "startGameInFullscreenToolStripMenuItem1";
            resources.ApplyResources(this.startGameInFullscreenToolStripMenuItem1, "startGameInFullscreenToolStripMenuItem1");
            this.startGameInFullscreenToolStripMenuItem1.Click += new System.EventHandler(this.startGameInFullscreenToolStripMenuItem_Click);
            // 
            // keepAspectRationToolStripMenuItem
            // 
            this.keepAspectRationToolStripMenuItem.Name = "keepAspectRationToolStripMenuItem";
            resources.ApplyResources(this.keepAspectRationToolStripMenuItem, "keepAspectRationToolStripMenuItem");
            this.keepAspectRationToolStripMenuItem.Click += new System.EventHandler(this.keepAspectRatioToolStripMenuItem_Click);
            // 
            // showFPSToolStripMenuItem1
            // 
            this.showFPSToolStripMenuItem1.Name = "showFPSToolStripMenuItem1";
            resources.ApplyResources(this.showFPSToolStripMenuItem1, "showFPSToolStripMenuItem1");
            this.showFPSToolStripMenuItem1.Click += new System.EventHandler(this.showFPSToolStripMenuItem_Click);
            // 
            // toolStripSeparator41
            // 
            this.toolStripSeparator41.Name = "toolStripSeparator41";
            resources.ApplyResources(this.toolStripSeparator41, "toolStripSeparator41");
            // 
            // soundEnabledToolStripMenuItem1
            // 
            this.soundEnabledToolStripMenuItem1.Image = global::MyNes.Properties.Resources.sound;
            this.soundEnabledToolStripMenuItem1.Name = "soundEnabledToolStripMenuItem1";
            resources.ApplyResources(this.soundEnabledToolStripMenuItem1, "soundEnabledToolStripMenuItem1");
            this.soundEnabledToolStripMenuItem1.Click += new System.EventHandler(this.soundEnabledToolStripMenuItem_Click);
            // 
            // volumeUpToolStripMenuItem
            // 
            this.volumeUpToolStripMenuItem.Name = "volumeUpToolStripMenuItem";
            resources.ApplyResources(this.volumeUpToolStripMenuItem, "volumeUpToolStripMenuItem");
            this.volumeUpToolStripMenuItem.Click += new System.EventHandler(this.volumeUpToolStripMenuItem_Click);
            // 
            // volumeDownToolStripMenuItem
            // 
            this.volumeDownToolStripMenuItem.Name = "volumeDownToolStripMenuItem";
            resources.ApplyResources(this.volumeDownToolStripMenuItem, "volumeDownToolStripMenuItem");
            this.volumeDownToolStripMenuItem.Click += new System.EventHandler(this.volumeDownToolStripMenuItem_Click);
            // 
            // toolStripSeparator42
            // 
            this.toolStripSeparator42.Name = "toolStripSeparator42";
            resources.ApplyResources(this.toolStripSeparator42, "toolStripSeparator42");
            // 
            // configureInputToolStripMenuItem
            // 
            this.configureInputToolStripMenuItem.Image = global::MyNes.Properties.Resources.joystick;
            this.configureInputToolStripMenuItem.Name = "configureInputToolStripMenuItem";
            resources.ApplyResources(this.configureInputToolStripMenuItem, "configureInputToolStripMenuItem");
            this.configureInputToolStripMenuItem.Click += new System.EventHandler(this.configureInputToolStripMenuItem_Click);
            // 
            // toolStripSeparator43
            // 
            this.toolStripSeparator43.Name = "toolStripSeparator43";
            resources.ApplyResources(this.toolStripSeparator43, "toolStripSeparator43");
            // 
            // turboToolStripMenuItem1
            // 
            this.turboToolStripMenuItem1.Name = "turboToolStripMenuItem1";
            resources.ApplyResources(this.turboToolStripMenuItem1, "turboToolStripMenuItem1");
            this.turboToolStripMenuItem1.Click += new System.EventHandler(this.turboToolStripMenuItem_Click);
            // 
            // timer_mouse_hider
            // 
            this.timer_mouse_hider.Interval = 1000;
            this.timer_mouse_hider.Tick += new System.EventHandler(this.timer_mouse_hider_Tick);
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_surface);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Activated += new System.EventHandler(this.FormMain_Activated);
            this.Deactivate += new System.EventHandler(this.FormMain_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.ResizeBegin += new System.EventHandler(this.FormMain_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.FormMain_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.MouseEnter += new System.EventHandler(this.panel_surface_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.panel_surface_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_surface_MouseMove);
            this.Move += new System.EventHandler(this.FormMain_Move);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        internal System.Windows.Forms.Panel panel_surface;
        private System.Windows.Forms.ToolStripMenuItem machineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutdownEmulationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem videoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFPSToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem keepAspectRatioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoStretchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stretchMultiplyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x1256X240ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x2256X240ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x3256X240ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x4256X240ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x5256X240ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x6256X240ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x7256X240ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x8256X240ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x9256X240ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x10256X240ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem hardResetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem softResetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem togglePauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem stateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem slotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_slot0;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_slot1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_slot2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_slot3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_slot4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_slot5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_slot6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_slot7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_slot8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_slot9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem saveStateAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadStateAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem saveStateBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadStateBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem takeSnapshotToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem audioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem soundEnabledToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem channelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem square1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem square2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem triangleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noiseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dMCToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem mMC5Square1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mMC5Square2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mMC5PCMToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem vRC6Square1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vRC6Square2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vRC6SawToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem sunsoft1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sunsoft5B2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sunsoft5B3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripMenuItem aboutMyNesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripMenuItem regionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceNTSCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forcePALToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceDendyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem player1InputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem player2InputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem player3InputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem player4InputToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem connect4PlayersToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
        private System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vSyncToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator20;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator21;
        private System.Windows.Forms.ToolStripMenuItem turboToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem romInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator22;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator23;
        private System.Windows.Forms.ToolStripMenuItem enableGameGenieToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gameGenieCodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator24;
        private System.Windows.Forms.ToolStripMenuItem recordSoundToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator25;
        private System.Windows.Forms.ToolStripMenuItem volumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem upToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showNotifiacationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseEmulationWhenFocusLostToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator27;
        private System.Windows.Forms.ToolStripMenuItem snapshotFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jPGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pNGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bMPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceSnpashotToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator26;
        private System.Windows.Forms.ToolStripMenuItem foldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gIFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tIFFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eMFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wMFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eXIFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSRAMFileOnEmuShutdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator28;
        private System.Windows.Forms.ToolStripMenuItem myNesInSourceforgenetToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator30;
        private System.Windows.Forms.ToolStripMenuItem frameSkipToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableFrameSkipToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator32;
        private System.Windows.Forms.ToolStripMenuItem interval2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interval3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interval4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator31;
        private System.Windows.Forms.ToolStripMenuItem launcherToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator33;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem linearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rendererToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rendererToolStripMenuItem_audio;
        private System.Windows.Forms.ToolStripMenuItem showLauncherAtStartUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gettingsStartedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator35;
        private System.Windows.Forms.ToolStripMenuItem namco1631ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem namco1632ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem namco1633ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem namco1634ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem namco1635ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem namco1636ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem namco1637ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem namco1638ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sDL2SettingsToolStripMenuItem;
        private System.Windows.Forms.Timer timer_mouse_hider;
        private System.Windows.Forms.ToolStripMenuItem startGameInFullscreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator37;
        private System.Windows.Forms.ToolStripMenuItem shutdownEmuExitMyNesOnEscapePressToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator38;
        private System.Windows.Forms.ToolStripMenuItem shutdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hardResetToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem softResetToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator39;
        private System.Windows.Forms.ToolStripMenuItem saveStateToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem loadStateToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem stateSlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator40;
        private System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem startGameInFullscreenToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem keepAspectRationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFPSToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator41;
        private System.Windows.Forms.ToolStripMenuItem soundEnabledToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem volumeUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem volumeDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator42;
        private System.Windows.Forms.ToolStripMenuItem configureInputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem15;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem16;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem17;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem18;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem19;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem20;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem21;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator43;
        private System.Windows.Forms.ToolStripMenuItem turboToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem loadStateOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadStateOpenToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator44;
        private System.Windows.Forms.ToolStripMenuItem loadStateOpenRecentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadStateOpenRecentToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem useMixerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator45;
        private System.Windows.Forms.ToolStripMenuItem frequencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hzToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hzToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem hzToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem hzToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem shortcutsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useEmulationThreadToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator46;
        private System.Windows.Forms.ToolStripMenuItem myNesWikiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resolutionUpscaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}

