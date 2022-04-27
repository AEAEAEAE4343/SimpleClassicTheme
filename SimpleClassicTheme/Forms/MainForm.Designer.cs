/*
 *  Simple Classic Theme, a basic utility to bring back classic theme to 
 *  newer versions of the Windows operating system.
 *  Copyright (C) 2022 Anis Errais
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
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonEnable = new System.Windows.Forms.Button();
            this.buttonDisable = new System.Windows.Forms.Button();
            this.buttonConfigure = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonAHKScripts = new System.Windows.Forms.Button();
            this.buttonECMT = new System.Windows.Forms.Button();
            this.buttonRibbonDisabler = new System.Windows.Forms.Button();
            this.buttonRunOnBoot = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button3DBorders = new System.Windows.Forms.Button();
            this.buttonInstallRequirements = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
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
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 2, 2, 2);
            this.menuStrip1.Size = new System.Drawing.Size(424, 24);
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
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
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
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(40, 20);
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
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SimpleClassicTheme.Properties.MainResources.sct_light_164;
            this.pictureBox1.Location = new System.Drawing.Point(11, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(164, 147);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // buttonEnable
            // 
            this.buttonEnable.Location = new System.Drawing.Point(191, 33);
            this.buttonEnable.Margin = new System.Windows.Forms.Padding(11, 0, 0, 5);
            this.buttonEnable.Name = "buttonEnable";
            this.buttonEnable.Size = new System.Drawing.Size(82, 23);
            this.buttonEnable.TabIndex = 0;
            this.buttonEnable.Text = "Enable";
            this.buttonEnable.UseVisualStyleBackColor = true;
            this.buttonEnable.Click += new System.EventHandler(this.ButtonEnable_Click);
            // 
            // buttonDisable
            // 
            this.buttonDisable.Location = new System.Drawing.Point(273, 33);
            this.buttonDisable.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.buttonDisable.Name = "buttonDisable";
            this.buttonDisable.Size = new System.Drawing.Size(82, 23);
            this.buttonDisable.TabIndex = 1;
            this.buttonDisable.Text = "Disable";
            this.buttonDisable.UseVisualStyleBackColor = true;
            this.buttonDisable.Click += new System.EventHandler(this.ButtonDisable_Click);
            // 
            // buttonConfigure
            // 
            this.buttonConfigure.Location = new System.Drawing.Point(191, 67);
            this.buttonConfigure.Margin = new System.Windows.Forms.Padding(11, 6, 0, 5);
            this.buttonConfigure.Name = "buttonConfigure";
            this.buttonConfigure.Size = new System.Drawing.Size(164, 23);
            this.buttonConfigure.TabIndex = 2;
            this.buttonConfigure.Text = "Configure colors";
            this.buttonConfigure.UseVisualStyleBackColor = true;
            this.buttonConfigure.Click += new System.EventHandler(this.ButtonConfigure_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.buttonAHKScripts);
            this.panel1.Controls.Add(this.buttonECMT);
            this.panel1.Controls.Add(this.buttonRibbonDisabler);
            this.panel1.Controls.Add(this.buttonRunOnBoot);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.button3DBorders);
            this.panel1.Controls.Add(this.buttonInstallRequirements);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.buttonEnable);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.buttonConfigure);
            this.panel1.Controls.Add(this.buttonDisable);
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(11);
            this.panel1.Size = new System.Drawing.Size(369, 311);
            this.panel1.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 253);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Version %v (revision mct%r)";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label1.Location = new System.Drawing.Point(11, 272);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(344, 28);
            this.label1.TabIndex = 30;
            this.label1.Text = "This is an alpha release of Simple Classic Theme. Unexpected issues will occur wi" +
    "th this version and support is not guaranteed.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(188, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Other Classic programs:";
            // 
            // buttonAHKScripts
            // 
            this.buttonAHKScripts.Location = new System.Drawing.Point(11, 222);
            this.buttonAHKScripts.Margin = new System.Windows.Forms.Padding(11, 6, 5, 5);
            this.buttonAHKScripts.Name = "buttonAHKScripts";
            this.buttonAHKScripts.Size = new System.Drawing.Size(164, 23);
            this.buttonAHKScripts.TabIndex = 6;
            this.buttonAHKScripts.Text = "Auto-load AHK scripts";
            this.buttonAHKScripts.UseVisualStyleBackColor = true;
            this.buttonAHKScripts.Click += new System.EventHandler(this.ButtonAHKScripts_Click);
            // 
            // buttonECMT
            // 
            this.buttonECMT.Location = new System.Drawing.Point(191, 188);
            this.buttonECMT.Margin = new System.Windows.Forms.Padding(11, 6, 0, 5);
            this.buttonECMT.Name = "buttonECMT";
            this.buttonECMT.Size = new System.Drawing.Size(164, 23);
            this.buttonECMT.TabIndex = 9;
            this.buttonECMT.Text = "Install ECMT (Win10 only)";
            this.buttonECMT.UseVisualStyleBackColor = true;
            this.buttonECMT.Click += new System.EventHandler(this.ButtonECMT_Click);
            // 
            // buttonRibbonDisabler
            // 
            this.buttonRibbonDisabler.Location = new System.Drawing.Point(191, 222);
            this.buttonRibbonDisabler.Margin = new System.Windows.Forms.Padding(11, 6, 0, 5);
            this.buttonRibbonDisabler.Name = "buttonRibbonDisabler";
            this.buttonRibbonDisabler.Size = new System.Drawing.Size(164, 23);
            this.buttonRibbonDisabler.TabIndex = 11;
            this.buttonRibbonDisabler.Text = "Run RibbonDisabler";
            this.buttonRibbonDisabler.UseVisualStyleBackColor = true;
            this.buttonRibbonDisabler.Click += new System.EventHandler(this.ButtonRibbonDisabler_Click);
            // 
            // buttonRunOnBoot
            // 
            this.buttonRunOnBoot.Location = new System.Drawing.Point(11, 188);
            this.buttonRunOnBoot.Margin = new System.Windows.Forms.Padding(6, 6, 5, 5);
            this.buttonRunOnBoot.Name = "buttonRunOnBoot";
            this.buttonRunOnBoot.Size = new System.Drawing.Size(164, 23);
            this.buttonRunOnBoot.TabIndex = 5;
            this.buttonRunOnBoot.Text = "Install SCT";
            this.buttonRunOnBoot.UseVisualStyleBackColor = true;
            this.buttonRunOnBoot.Click += new System.EventHandler(this.ButtonRunOnBoot_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 166);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Simple Classic Theme:";
            // 
            // button3DBorders
            // 
            this.button3DBorders.Location = new System.Drawing.Point(191, 101);
            this.button3DBorders.Margin = new System.Windows.Forms.Padding(11, 6, 0, 5);
            this.button3DBorders.Name = "button3DBorders";
            this.button3DBorders.Size = new System.Drawing.Size(164, 23);
            this.button3DBorders.TabIndex = 3;
            this.button3DBorders.Text = "Enable 3D borders";
            this.button3DBorders.UseVisualStyleBackColor = true;
            this.button3DBorders.Click += new System.EventHandler(this.Button3DBorders_Click);
            // 
            // buttonInstallRequirements
            // 
            this.buttonInstallRequirements.Location = new System.Drawing.Point(191, 135);
            this.buttonInstallRequirements.Margin = new System.Windows.Forms.Padding(11, 6, 0, 5);
            this.buttonInstallRequirements.Name = "buttonInstallRequirements";
            this.buttonInstallRequirements.Size = new System.Drawing.Size(164, 23);
            this.buttonInstallRequirements.TabIndex = 4;
            this.buttonInstallRequirements.Text = "Install requirements";
            this.buttonInstallRequirements.UseVisualStyleBackColor = true;
            this.buttonInstallRequirements.Click += new System.EventHandler(this.ButtonInstallRequirements_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(188, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Classic Theme:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(424, 444);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Simple Classic Theme";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem guideToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem reportBugsToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonEnable;
        private System.Windows.Forms.Button buttonDisable;
        private System.Windows.Forms.Button buttonConfigure;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonInstallRequirements;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3DBorders;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonRunOnBoot;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonAHKScripts;
        private System.Windows.Forms.Button buttonECMT;
        private System.Windows.Forms.Button buttonRibbonDisabler;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

