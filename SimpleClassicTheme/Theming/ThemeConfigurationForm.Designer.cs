
namespace SimpleClassicTheme.Theming
{
	partial class ThemeConfigurationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThemeConfigurationForm));
            this.panelWindowPreview = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxScheme = new System.Windows.Forms.ComboBox();
            this.comboBoxItem = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxFont = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.upDownItemSize = new System.Windows.Forms.NumericUpDown();
            this.buttonColorPrimary = new SimpleClassicTheme.UI.Controls.ColorButton();
            this.buttonColorSecondary = new SimpleClassicTheme.UI.Controls.ColorButton();
            this.buttonColorFont = new SimpleClassicTheme.UI.Controls.ColorButton();
            this.upDownFontSize = new System.Windows.Forms.NumericUpDown();
            this.checkBoxBold = new System.Windows.Forms.CheckBox();
            this.checkBoxItalic = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panelDialogPreview = new System.Windows.Forms.Panel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.upDownItemSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownFontSize)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelWindowPreview
            // 
            this.panelWindowPreview.BackColor = System.Drawing.SystemColors.Desktop;
            this.panelWindowPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelWindowPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWindowPreview.Location = new System.Drawing.Point(3, 3);
            this.panelWindowPreview.Margin = new System.Windows.Forms.Padding(6);
            this.panelWindowPreview.Name = "panelWindowPreview";
            this.panelWindowPreview.Size = new System.Drawing.Size(473, 268);
            this.panelWindowPreview.TabIndex = 0;
            this.panelWindowPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.panelWindowPreviewPaint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Scheme:";
            // 
            // comboBoxScheme
            // 
            this.comboBoxScheme.FormattingEnabled = true;
            this.comboBoxScheme.Location = new System.Drawing.Point(9, 44);
            this.comboBoxScheme.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxScheme.Name = "comboBoxScheme";
            this.comboBoxScheme.Size = new System.Drawing.Size(307, 21);
            this.comboBoxScheme.TabIndex = 2;
            this.comboBoxScheme.SelectedIndexChanged += new System.EventHandler(this.comboBoxScheme_SelectedIndexChanged);
            // 
            // comboBoxItem
            // 
            this.comboBoxItem.FormattingEnabled = true;
            this.comboBoxItem.Location = new System.Drawing.Point(9, 90);
            this.comboBoxItem.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxItem.Name = "comboBoxItem";
            this.comboBoxItem.Size = new System.Drawing.Size(307, 21);
            this.comboBoxItem.TabIndex = 4;
            this.comboBoxItem.SelectedIndexChanged += new System.EventHandler(this.comboBoxItem_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Item:";
            // 
            // comboBoxFont
            // 
            this.comboBoxFont.FormattingEnabled = true;
            this.comboBoxFont.Location = new System.Drawing.Point(9, 136);
            this.comboBoxFont.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxFont.Name = "comboBoxFont";
            this.comboBoxFont.Size = new System.Drawing.Size(307, 21);
            this.comboBoxFont.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 117);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Font:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(325, 44);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Save As...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonSave);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(406, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // upDownItemSize
            // 
            this.upDownItemSize.Location = new System.Drawing.Point(325, 90);
            this.upDownItemSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.upDownItemSize.Name = "upDownItemSize";
            this.upDownItemSize.Size = new System.Drawing.Size(48, 20);
            this.upDownItemSize.TabIndex = 9;
            this.upDownItemSize.ValueChanged += new System.EventHandler(this.upDownItemSize_ValueChanged);
            // 
            // buttonColorPrimary
            // 
            this.buttonColorPrimary.Color = System.Drawing.Color.White;
            this.buttonColorPrimary.Location = new System.Drawing.Point(379, 90);
            this.buttonColorPrimary.Name = "buttonColorPrimary";
            this.buttonColorPrimary.Size = new System.Drawing.Size(48, 23);
            this.buttonColorPrimary.TabIndex = 10;
            this.buttonColorPrimary.UseVisualStyleBackColor = true;
            this.buttonColorPrimary.ColorChanged += new System.EventHandler<System.EventArgs>(this.buttonColorPrimary_ColorChanged);
            // 
            // buttonColorSecondary
            // 
            this.buttonColorSecondary.Color = System.Drawing.Color.White;
            this.buttonColorSecondary.Location = new System.Drawing.Point(433, 90);
            this.buttonColorSecondary.Name = "buttonColorSecondary";
            this.buttonColorSecondary.Size = new System.Drawing.Size(48, 23);
            this.buttonColorSecondary.TabIndex = 11;
            this.buttonColorSecondary.UseVisualStyleBackColor = true;
            this.buttonColorSecondary.ColorChanged += new System.EventHandler<System.EventArgs>(this.buttonColorSecondary_ColorChanged);
            // 
            // buttonColorFont
            // 
            this.buttonColorFont.Color = System.Drawing.Color.White;
            this.buttonColorFont.Location = new System.Drawing.Point(379, 136);
            this.buttonColorFont.Name = "buttonColorFont";
            this.buttonColorFont.Size = new System.Drawing.Size(48, 23);
            this.buttonColorFont.TabIndex = 13;
            this.buttonColorFont.UseVisualStyleBackColor = true;
            this.buttonColorFont.ColorChanged += new System.EventHandler<System.EventArgs>(this.buttonColorFont_ColorChanged);
            // 
            // upDownFontSize
            // 
            this.upDownFontSize.Location = new System.Drawing.Point(325, 136);
            this.upDownFontSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.upDownFontSize.Name = "upDownFontSize";
            this.upDownFontSize.Size = new System.Drawing.Size(48, 20);
            this.upDownFontSize.TabIndex = 12;
            // 
            // checkBoxBold
            // 
            this.checkBoxBold.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxBold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxBold.Location = new System.Drawing.Point(433, 136);
            this.checkBoxBold.Name = "checkBoxBold";
            this.checkBoxBold.Size = new System.Drawing.Size(24, 23);
            this.checkBoxBold.TabIndex = 15;
            this.checkBoxBold.Text = "B";
            this.checkBoxBold.UseVisualStyleBackColor = true;
            // 
            // checkBoxItalic
            // 
            this.checkBoxItalic.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxItalic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxItalic.Location = new System.Drawing.Point(457, 136);
            this.checkBoxItalic.Name = "checkBoxItalic";
            this.checkBoxItalic.Size = new System.Drawing.Size(24, 23);
            this.checkBoxItalic.TabIndex = 16;
            this.checkBoxItalic.Text = "I";
            this.checkBoxItalic.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(262, 566);
            this.button5.Margin = new System.Windows.Forms.Padding(6, 6, 3, 6);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 17;
            this.button5.Text = "OK";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(343, 566);
            this.button7.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 18;
            this.button7.Text = "Cancel";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(424, 566);
            this.button8.Margin = new System.Windows.Forms.Padding(3, 6, 6, 6);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 19;
            this.button8.Text = "Apply";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(322, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Size:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(376, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Color 1:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(430, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Color 2:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(322, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Size:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(376, 117);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Color:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(430, 117);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "Style:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(487, 300);
            this.tabControl1.TabIndex = 26;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panelWindowPreview);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(479, 274);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Windows";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panelDialogPreview);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(479, 274);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Dialogs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panelDialogPreview
            // 
            this.panelDialogPreview.BackColor = System.Drawing.SystemColors.Desktop;
            this.panelDialogPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelDialogPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDialogPreview.Location = new System.Drawing.Point(3, 3);
            this.panelDialogPreview.Margin = new System.Windows.Forms.Padding(6);
            this.panelDialogPreview.Name = "panelDialogPreview";
            this.panelDialogPreview.Size = new System.Drawing.Size(473, 268);
            this.panelDialogPreview.TabIndex = 1;
            this.panelDialogPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDialogPreview_Paint);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(479, 274);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Icons";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBoxScheme);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.comboBoxItem);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.comboBoxFont);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.upDownItemSize);
            this.groupBox1.Controls.Add(this.buttonColorPrimary);
            this.groupBox1.Controls.Add(this.buttonColorSecondary);
            this.groupBox1.Controls.Add(this.checkBoxItalic);
            this.groupBox1.Controls.Add(this.upDownFontSize);
            this.groupBox1.Controls.Add(this.checkBoxBold);
            this.groupBox1.Controls.Add(this.buttonColorFont);
            this.groupBox1.Location = new System.Drawing.Point(12, 331);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 226);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Preview of the theme:";
            // 
            // ThemeConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 604);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ThemeConfigurationForm";
            this.Text = "Simple Classic Theme - Configure Appearance";
            ((System.ComponentModel.ISupportInitialize)(this.upDownItemSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownFontSize)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panelWindowPreview;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBoxScheme;
		private System.Windows.Forms.ComboBox comboBoxItem;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboBoxFont;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.NumericUpDown upDownItemSize;
		private SimpleClassicTheme.UI.Controls.ColorButton buttonColorPrimary;
		private SimpleClassicTheme.UI.Controls.ColorButton buttonColorSecondary;
		private SimpleClassicTheme.UI.Controls.ColorButton buttonColorFont;
		private System.Windows.Forms.NumericUpDown upDownFontSize;
		private System.Windows.Forms.CheckBox checkBoxBold;
		private System.Windows.Forms.CheckBox checkBoxItalic;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panelDialogPreview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
    }
}