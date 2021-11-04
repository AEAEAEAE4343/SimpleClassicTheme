
namespace SimpleClassicTheme.Forms
{
	partial class ExplorerPatcherForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerPatcherForm));
			this.label1 = new System.Windows.Forms.Label();
			this.buttonInstallUninstall = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonApply = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.button1 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 20);
			this.label1.Margin = new System.Windows.Forms.Padding(6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(269, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Most features here require you to install ExplorerPatcher";
			// 
			// buttonInstallUninstall
			// 
			this.buttonInstallUninstall.Location = new System.Drawing.Point(308, 15);
			this.buttonInstallUninstall.Margin = new System.Windows.Forms.Padding(6);
			this.buttonInstallUninstall.Name = "buttonInstallUninstall";
			this.buttonInstallUninstall.Size = new System.Drawing.Size(200, 23);
			this.buttonInstallUninstall.TabIndex = 1;
			this.buttonInstallUninstall.Text = "Install ExplorerPatcher";
			this.buttonInstallUninstall.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkBox3);
			this.groupBox1.Controls.Add(this.checkBox2);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.comboBox1);
			this.groupBox1.Controls.Add(this.checkBox1);
			this.groupBox1.Location = new System.Drawing.Point(15, 182);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
			this.groupBox1.Size = new System.Drawing.Size(493, 163);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "File Explorer settings";
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Location = new System.Drawing.Point(12, 64);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(129, 17);
			this.checkBox3.TabIndex = 11;
			this.checkBox3.Text = "Restore legacy ribbon";
			this.checkBox3.UseVisualStyleBackColor = true;
			this.checkBox3.CheckedChanged += new System.EventHandler(this.ConfigurationChanged);
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(12, 132);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(131, 17);
			this.checkBox2.TabIndex = 10;
			this.checkBox2.Text = "Disable navigation bar";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new System.EventHandler(this.ConfigurationChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 98);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(73, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Search mode:";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "Normal",
            "Legacy search",
            "Fully disable search"});
			this.comboBox1.Location = new System.Drawing.Point(281, 95);
			this.comboBox1.Margin = new System.Windows.Forms.Padding(6);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(200, 21);
			this.comboBox1.TabIndex = 6;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ConfigurationChanged);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(12, 29);
			this.checkBox1.Margin = new System.Windows.Forms.Padding(6);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(164, 17);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Restore legacy context menu";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.ConfigurationChanged);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.Location = new System.Drawing.Point(346, 534);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(6);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 9;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonApply
			// 
			this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonApply.Enabled = false;
			this.buttonApply.Location = new System.Drawing.Point(433, 534);
			this.buttonApply.Margin = new System.Windows.Forms.Padding(6);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new System.Drawing.Size(75, 23);
			this.buttonApply.TabIndex = 8;
			this.buttonApply.Text = "&Apply";
			this.buttonApply.UseVisualStyleBackColor = true;
			this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.Location = new System.Drawing.Point(259, 534);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(6);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 7;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.checkBox6);
			this.groupBox2.Controls.Add(this.checkBox5);
			this.groupBox2.Controls.Add(this.checkBox4);
			this.groupBox2.Location = new System.Drawing.Point(15, 357);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(6);
			this.groupBox2.Size = new System.Drawing.Size(493, 165);
			this.groupBox2.TabIndex = 10;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Taskbar settings";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label3.Location = new System.Drawing.Point(12, 135);
			this.label3.Margin = new System.Windows.Forms.Padding(6);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(324, 13);
			this.label3.TabIndex = 16;
			this.label3.Text = "All Taskbar settings require the Restore Windows 10 taskbar option";
			// 
			// checkBox6
			// 
			this.checkBox6.AutoSize = true;
			this.checkBox6.Location = new System.Drawing.Point(12, 99);
			this.checkBox6.Margin = new System.Windows.Forms.Padding(6);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(162, 17);
			this.checkBox6.TabIndex = 15;
			this.checkBox6.Text = "Restore legacy volume flyout";
			this.checkBox6.UseVisualStyleBackColor = true;
			this.checkBox6.CheckedChanged += new System.EventHandler(this.ConfigurationChanged);
			// 
			// checkBox5
			// 
			this.checkBox5.AutoSize = true;
			this.checkBox5.Location = new System.Drawing.Point(12, 64);
			this.checkBox5.Margin = new System.Windows.Forms.Padding(6);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(154, 17);
			this.checkBox5.TabIndex = 13;
			this.checkBox5.Text = "Restore legacy clock flyout";
			this.checkBox5.UseVisualStyleBackColor = true;
			this.checkBox5.CheckedChanged += new System.EventHandler(this.ConfigurationChanged);
			// 
			// checkBox4
			// 
			this.checkBox4.AutoSize = true;
			this.checkBox4.Location = new System.Drawing.Point(12, 29);
			this.checkBox4.Margin = new System.Windows.Forms.Padding(6);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(163, 17);
			this.checkBox4.TabIndex = 0;
			this.checkBox4.Text = "Restore Windows 10 taskbar";
			this.checkBox4.UseVisualStyleBackColor = true;
			this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.linkLabel1);
			this.groupBox3.Controls.Add(this.radioButton2);
			this.groupBox3.Controls.Add(this.radioButton1);
			this.groupBox3.Location = new System.Drawing.Point(15, 116);
			this.groupBox3.Margin = new System.Windows.Forms.Padding(6);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(6);
			this.groupBox3.Size = new System.Drawing.Size(493, 54);
			this.groupBox3.TabIndex = 11;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Configuration while SCT is";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(359, 27);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(113, 13);
			this.linkLabel1.TabIndex = 2;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "What does this mean?";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// radioButton2
			// 
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new System.Drawing.Point(108, 25);
			this.radioButton2.Margin = new System.Windows.Forms.Padding(6);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(66, 17);
			this.radioButton2.TabIndex = 1;
			this.radioButton2.Text = "Disabled";
			this.radioButton2.UseVisualStyleBackColor = true;
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new System.Drawing.Point(12, 25);
			this.radioButton1.Margin = new System.Windows.Forms.Padding(6);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(64, 17);
			this.radioButton1.TabIndex = 0;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "Enabled";
			this.radioButton1.UseVisualStyleBackColor = true;
			this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(18, 534);
			this.button1.Margin = new System.Windows.Forms.Padding(6);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(121, 23);
			this.button1.TabIndex = 12;
			this.button1.Text = "Auto-Patch";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(15, 50);
			this.label4.Margin = new System.Windows.Forms.Padding(6);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(493, 29);
			this.label4.TabIndex = 13;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// linkLabel2
			// 
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Location = new System.Drawing.Point(15, 91);
			this.linkLabel2.Margin = new System.Windows.Forms.Padding(6);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(407, 13);
			this.linkLabel2.TabIndex = 14;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "Author: Valentin-Gabriel Radu (valinet) Support: Git Issues and SCT\'s Git Issues " +
    "page";
			// 
			// ExplorerPatcherForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(523, 572);
			this.Controls.Add(this.linkLabel2);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonApply);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonInstallUninstall);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExplorerPatcherForm";
			this.Text = "Simple Classic Theme - ExplorerPatcher";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonInstallUninstall;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonApply;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.LinkLabel linkLabel2;
	}
}