using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClassicTheme.Forms
{
	public partial class ExplorerPatcherForm : Form
	{
		public ExplorerPatcherForm()
		{
			InitializeComponent();

			linkLabel1.Links.Clear();
			linkLabel1.Links.Add(8, 31);
			linkLabel1.Links.Add(49, 10);
			linkLabel1.Links.Add(64, 21);

			LoadConfig();
		}

		public void LoadConfig()
		{
			ExplorerPatcher.Configuration.ConfigurationWhileEnabledOrDisabled = radioButton1.Checked;

			checkBox1.Checked = ExplorerPatcher.Configuration.FileExplorerLegacyContextMenu;
			checkBox3.Checked = ExplorerPatcher.Configuration.FileExplorerLegacyRibbon;
			comboBox1.SelectedIndex = (int)ExplorerPatcher.Configuration.FileExplorerSearchMode;
			checkBox2.Checked = ExplorerPatcher.Configuration.FileExplorerNoNavigationBar;

			checkBox4.Checked = ExplorerPatcher.Configuration.TaskbarWindows10Taskbar;
			checkBox5.Checked = ExplorerPatcher.Configuration.TaskbarLegacyClockFlyout;
			checkBox6.Checked = ExplorerPatcher.Configuration.TaskbarLegacyVolumeFlyout;
			buttonApply.Enabled = false;

			button1.Enabled = groupBox1.Enabled = groupBox2.Enabled = ExplorerPatcher.IsInstalled;
			buttonInstallUninstall.Text = ExplorerPatcher.IsInstalled ? "Uninstall ExplorerPatcher" : "Install ExplorerPatcher";
		}

		public void SaveConfig()
		{
			ExplorerPatcher.Configuration.FileExplorerLegacyContextMenu = checkBox1.Checked;
			ExplorerPatcher.Configuration.FileExplorerLegacyRibbon = checkBox3.Checked;
			ExplorerPatcher.Configuration.FileExplorerSearchMode = (ExplorerPatcher.Configuration.ExplorerPatcherSearchMode)comboBox1.SelectedIndex;
			ExplorerPatcher.Configuration.FileExplorerNoNavigationBar = checkBox2.Checked;

			ExplorerPatcher.Configuration.TaskbarWindows10Taskbar = checkBox4.Checked;
			ExplorerPatcher.Configuration.TaskbarLegacyClockFlyout = checkBox5.Checked;
			ExplorerPatcher.Configuration.TaskbarLegacyVolumeFlyout = checkBox6.Checked;
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			if (!buttonApply.Enabled || MessageBox.Show(this, "You have not applied your changes yet, continuing will discard your changes. Would you like to still continue?", "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				LoadConfig();
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MessageBox.Show("Simple Classic Theme allows you to have two configurations for ExplorerPatcher: one for when Classic Theme is enabled, and one for when Classic Theme is disabled.\nThis allows you to only use certain ExplorerPatcher features when SCT is active.", "ExplorerPatcher configuration sets");
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			buttonApply.PerformClick();
			buttonCancel.PerformClick();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void buttonApply_Click(object sender, EventArgs e)
		{
			SaveConfig();
			buttonApply.Enabled = false;

			if (Configuration.Enabled)
			{
				ExplorerPatcher.Configuration.ConfigurationApplying = true;
				ClassicTheme.MasterDisable(Configuration.EnableTaskbar);
				ExplorerPatcher.ApplyConfiguration(true);
				ClassicTheme.MasterEnable(Configuration.EnableTaskbar);
				ExplorerPatcher.Configuration.ConfigurationApplying = false;
			}
			else
			{
				ExplorerPatcher.ApplyConfiguration(true);
			}
		}

		private void ConfigurationChanged(object sender, EventArgs e)
		{
			buttonApply.Enabled = true;
		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
			ConfigurationChanged(sender, e);
			checkBox5.Enabled = checkBox6.Enabled = checkBox4.Checked;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(this, "This will reset all settings to the recommended default for both Enabled and Disabled states. This means your own settings will get voided. Would you like to continue?", "Auto-Patch", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return; 

			ExplorerPatcher.Configuration.ConfigurationWhileEnabledOrDisabled = true;
			ExplorerPatcher.Configuration.FileExplorerLegacyContextMenu = true;
			ExplorerPatcher.Configuration.FileExplorerLegacyRibbon = true;
			ExplorerPatcher.Configuration.FileExplorerSearchMode = ExplorerPatcher.Configuration.ExplorerPatcherSearchMode.Normal;
			ExplorerPatcher.Configuration.FileExplorerNoNavigationBar = false;
			ExplorerPatcher.Configuration.TaskbarWindows10Taskbar = true;
			ExplorerPatcher.Configuration.TaskbarLegacyClockFlyout = true;
			ExplorerPatcher.Configuration.TaskbarLegacyVolumeFlyout = true;

			ExplorerPatcher.Configuration.ConfigurationWhileEnabledOrDisabled = false;
			ExplorerPatcher.Configuration.FileExplorerLegacyContextMenu = false;
			ExplorerPatcher.Configuration.FileExplorerLegacyRibbon = false;
			ExplorerPatcher.Configuration.FileExplorerSearchMode = ExplorerPatcher.Configuration.ExplorerPatcherSearchMode.Normal;
			ExplorerPatcher.Configuration.FileExplorerNoNavigationBar = false;
			ExplorerPatcher.Configuration.TaskbarWindows10Taskbar = false;
			ExplorerPatcher.Configuration.TaskbarLegacyClockFlyout = false;
			ExplorerPatcher.Configuration.TaskbarLegacyVolumeFlyout = false;

			LoadConfig();
			buttonApply_Click(sender, e);
		}
	}
}
