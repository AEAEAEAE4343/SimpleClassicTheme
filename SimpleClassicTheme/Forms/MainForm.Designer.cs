/*
 *  SimpleClassicTheme, a basic utility to bring back classic theme to newer versions of the Windows operating system.
 *  Copyright (C) 2021 Anis Errais
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *
 */

namespace SimpleClassicTheme
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonEnable = new System.Windows.Forms.Button();
            this.buttonDisable = new System.Windows.Forms.Button();
            this.buttonInstallRequirements = new System.Windows.Forms.Button();
            this.buttonConfigure = new System.Windows.Forms.Button();
            this.buttonRunOnBoot = new System.Windows.Forms.Button();
            this.buttonTClock = new System.Windows.Forms.Button();
            this.buttonECMT = new System.Windows.Forms.Button();
            this.buttonUninstall = new System.Windows.Forms.Button();
            this.button3DBorders = new System.Windows.Forms.Button();
            this.buttonRibbonDisabler = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonExplorerPatcher = new System.Windows.Forms.Button();
            this.buttonAHKScripts = new System.Windows.Forms.Button();
            this.buttonUtilities = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonRestoreWindowSettings = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.guideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportBugsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonEnable
            // 
            this.buttonEnable.Location = new System.Drawing.Point(6, 19);
            this.buttonEnable.Name = "buttonEnable";
            this.buttonEnable.Size = new System.Drawing.Size(152, 23);
            this.buttonEnable.TabIndex = 0;
            this.buttonEnable.Text = "Enable";
            this.buttonEnable.UseVisualStyleBackColor = true;
            this.buttonEnable.Click += new System.EventHandler(this.ButtonEnable_Click);
            // 
            // buttonDisable
            // 
            this.buttonDisable.Location = new System.Drawing.Point(6, 48);
            this.buttonDisable.Name = "buttonDisable";
            this.buttonDisable.Size = new System.Drawing.Size(152, 23);
            this.buttonDisable.TabIndex = 1;
            this.buttonDisable.Text = "Disable";
            this.buttonDisable.UseVisualStyleBackColor = true;
            this.buttonDisable.Click += new System.EventHandler(this.ButtonDisable_Click);
            // 
            // buttonInstallRequirements
            // 
            this.buttonInstallRequirements.Location = new System.Drawing.Point(6, 77);
            this.buttonInstallRequirements.Name = "buttonInstallRequirements";
            this.buttonInstallRequirements.Size = new System.Drawing.Size(152, 23);
            this.buttonInstallRequirements.TabIndex = 2;
            this.buttonInstallRequirements.Text = "Install required stuff";
            this.buttonInstallRequirements.UseVisualStyleBackColor = true;
            this.buttonInstallRequirements.Click += new System.EventHandler(this.ButtonInstallRequirements_Click);
            // 
            // buttonConfigure
            // 
            this.buttonConfigure.Location = new System.Drawing.Point(6, 106);
            this.buttonConfigure.Name = "buttonConfigure";
            this.buttonConfigure.Size = new System.Drawing.Size(152, 23);
            this.buttonConfigure.TabIndex = 3;
            this.buttonConfigure.Text = "Configure colors";
            this.buttonConfigure.UseVisualStyleBackColor = true;
            this.buttonConfigure.Click += new System.EventHandler(this.ButtonConfigure_Click);
            // 
            // buttonRunOnBoot
            // 
            this.buttonRunOnBoot.Location = new System.Drawing.Point(6, 135);
            this.buttonRunOnBoot.Name = "buttonRunOnBoot";
            this.buttonRunOnBoot.Size = new System.Drawing.Size(152, 23);
            this.buttonRunOnBoot.TabIndex = 4;
            this.buttonRunOnBoot.Text = "Run SCT on boot";
            this.buttonRunOnBoot.UseVisualStyleBackColor = true;
            this.buttonRunOnBoot.Click += new System.EventHandler(this.ButtonRunOnBoot_Click);
            // 
            // buttonTClock
            // 
            this.buttonTClock.Location = new System.Drawing.Point(6, 135);
            this.buttonTClock.Name = "buttonTClock";
            this.buttonTClock.Size = new System.Drawing.Size(152, 23);
            this.buttonTClock.TabIndex = 4;
            this.buttonTClock.Text = "Install T-Clock";
            this.buttonTClock.UseVisualStyleBackColor = true;
            this.buttonTClock.Click += new System.EventHandler(this.ButtonTClock_Click);
            // 
            // buttonECMT
            // 
            this.buttonECMT.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.buttonECMT.Location = new System.Drawing.Point(6, 19);
            this.buttonECMT.Name = "buttonECMT";
            this.buttonECMT.Size = new System.Drawing.Size(152, 23);
            this.buttonECMT.TabIndex = 0;
            this.buttonECMT.Text = "Inst. ExplorerContextMenuTweaker";
            this.buttonECMT.UseVisualStyleBackColor = true;
            this.buttonECMT.Click += new System.EventHandler(this.ButtonECMT_Click);
            // 
            // buttonUninstall
            // 
            this.buttonUninstall.Location = new System.Drawing.Point(6, 283);
            this.buttonUninstall.Name = "buttonUninstall";
            this.buttonUninstall.Size = new System.Drawing.Size(152, 23);
            this.buttonUninstall.TabIndex = 8;
            this.buttonUninstall.Text = "Uninstall (restores all UI)";
            this.buttonUninstall.UseVisualStyleBackColor = true;
            this.buttonUninstall.Click += new System.EventHandler(this.ButtonUninstall_Click);
            // 
            // button3DBorders
            // 
            this.button3DBorders.Location = new System.Drawing.Point(6, 77);
            this.button3DBorders.Name = "button3DBorders";
            this.button3DBorders.Size = new System.Drawing.Size(152, 23);
            this.button3DBorders.TabIndex = 2;
            this.button3DBorders.Text = "Enable 3D borders";
            this.button3DBorders.UseVisualStyleBackColor = true;
            this.button3DBorders.Click += new System.EventHandler(this.Button3DBorder_Click);
            // 
            // buttonRibbonDisabler
            // 
            this.buttonRibbonDisabler.Location = new System.Drawing.Point(6, 106);
            this.buttonRibbonDisabler.Name = "buttonRibbonDisabler";
            this.buttonRibbonDisabler.Size = new System.Drawing.Size(152, 23);
            this.buttonRibbonDisabler.TabIndex = 3;
            this.buttonRibbonDisabler.Text = "Run RibbonDisabler 4.0";
            this.buttonRibbonDisabler.UseVisualStyleBackColor = true;
            this.buttonRibbonDisabler.Click += new System.EventHandler(this.ButtonRibbonDisabler_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonExplorerPatcher);
            this.groupBox1.Controls.Add(this.buttonAHKScripts);
            this.groupBox1.Controls.Add(this.buttonUtilities);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button3DBorders);
            this.groupBox1.Controls.Add(this.buttonTClock);
            this.groupBox1.Controls.Add(this.buttonRestoreWindowSettings);
            this.groupBox1.Controls.Add(this.buttonUninstall);
            this.groupBox1.Controls.Add(this.buttonRibbonDisabler);
            this.groupBox1.Controls.Add(this.buttonECMT);
            this.groupBox1.Location = new System.Drawing.Point(184, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(164, 312);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Extra stuff";
            // 
            // buttonExplorerPatcher
            // 
            this.buttonExplorerPatcher.Location = new System.Drawing.Point(6, 48);
            this.buttonExplorerPatcher.Name = "buttonExplorerPatcher";
            this.buttonExplorerPatcher.Size = new System.Drawing.Size(152, 23);
            this.buttonExplorerPatcher.TabIndex = 1;
            this.buttonExplorerPatcher.Text = "Patch Explorer";
            this.buttonExplorerPatcher.UseVisualStyleBackColor = true;
            this.buttonExplorerPatcher.Click += new System.EventHandler(this.ButtonExplorerPatcher_Click);
            // 
            // buttonAHKScripts
            // 
            this.buttonAHKScripts.Location = new System.Drawing.Point(6, 164);
            this.buttonAHKScripts.Name = "buttonAHKScripts";
            this.buttonAHKScripts.Size = new System.Drawing.Size(152, 23);
            this.buttonAHKScripts.TabIndex = 5;
            this.buttonAHKScripts.Text = "Auto-load AHK scripts";
            this.buttonAHKScripts.UseVisualStyleBackColor = true;
            this.buttonAHKScripts.Click += new System.EventHandler(this.ButtonAHKScripts_Click);
            // 
            // buttonUtilities
            // 
            this.buttonUtilities.Location = new System.Drawing.Point(6, 193);
            this.buttonUtilities.Name = "buttonUtilities";
            this.buttonUtilities.Size = new System.Drawing.Size(152, 23);
            this.buttonUtilities.TabIndex = 6;
            this.buttonUtilities.Text = "Install Classic utilities";
            this.buttonUtilities.UseVisualStyleBackColor = true;
            this.buttonUtilities.Click += new System.EventHandler(this.ButtonUtilities_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 238);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Restore/Remove:";
            // 
            // buttonRestoreWindowSettings
            // 
            this.buttonRestoreWindowSettings.Location = new System.Drawing.Point(6, 254);
            this.buttonRestoreWindowSettings.Name = "buttonRestoreWindowSettings";
            this.buttonRestoreWindowSettings.Size = new System.Drawing.Size(152, 23);
            this.buttonRestoreWindowSettings.TabIndex = 7;
            this.buttonRestoreWindowSettings.Text = "Restore window settings";
            this.buttonRestoreWindowSettings.UseVisualStyleBackColor = true;
            this.buttonRestoreWindowSettings.Click += new System.EventHandler(this.ButtonRestoreWindowSettings_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonEnable);
            this.groupBox2.Controls.Add(this.buttonDisable);
            this.groupBox2.Controls.Add(this.buttonInstallRequirements);
            this.groupBox2.Controls.Add(this.buttonConfigure);
            this.groupBox2.Controls.Add(this.buttonRunOnBoot);
            this.groupBox2.Location = new System.Drawing.Point(14, 39);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(164, 162);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Classic Theme";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SimpleClassicTheme.Properties.Resources.sct_light_164;
            this.pictureBox1.Location = new System.Drawing.Point(12, 207);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(164, 144);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point(12, 359);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(336, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Experiencing issues? Don\'t reply to the WinClassic thread. ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel1.Location = new System.Drawing.Point(12, 372);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(336, 13);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Report it on GitHub instead";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(122, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(67, 22);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(360, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.guideToolStripMenuItem,
            this.reportBugsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(41, 20);
            this.helpToolStripMenuItem1.Text = "&Help";
            // 
            // guideToolStripMenuItem
            // 
            this.guideToolStripMenuItem.Name = "guideToolStripMenuItem";
            this.guideToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.guideToolStripMenuItem.Text = "&Guide";
            this.guideToolStripMenuItem.Click += new System.EventHandler(this.guideToolStripMenuItem_Click);
            // 
            // reportBugsToolStripMenuItem
            // 
            this.reportBugsToolStripMenuItem.Name = "reportBugsToolStripMenuItem";
            this.reportBugsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reportBugsToolStripMenuItem.Text = "Report bugs";
            this.reportBugsToolStripMenuItem.Click += new System.EventHandler(this.reportBugsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(360, 395);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Simple Classic Theme";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonEnable;
        private System.Windows.Forms.Button buttonDisable;
        private System.Windows.Forms.Button buttonInstallRequirements;
        private System.Windows.Forms.Button buttonConfigure;
        private System.Windows.Forms.Button buttonRunOnBoot;
        private System.Windows.Forms.Button buttonECMT;
        private System.Windows.Forms.Button buttonTClock;
        private System.Windows.Forms.Button buttonRibbonDisabler;
        private System.Windows.Forms.Button button3DBorders;
        private System.Windows.Forms.Button buttonUninstall;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Button buttonUtilities;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.Button buttonAHKScripts;
		private System.Windows.Forms.Button buttonRestoreWindowSettings;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem guideToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem reportBugsToolStripMenuItem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonExplorerPatcher;
	}
}

