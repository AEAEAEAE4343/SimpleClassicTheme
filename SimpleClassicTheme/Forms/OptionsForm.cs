using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SimpleClassicTheme.Forms
{
	public partial class OptionsForm : Form
	{
		public Dictionary<string, string> TaskbarTypeDisplay = new Dictionary<string, string>();

		public OptionsForm()
		{
			TaskbarTypeDisplay.Add("SiB+OS", "SiB and OS");
			TaskbarTypeDisplay.Add("SCTT", "SCT Taskbar (alpha)");

			InitializeComponent();
			Load += OptionsForm_Load;

			foreach (KeyValuePair<string, string> f in TaskbarTypeDisplay)
				comboBoxTaskbar.Items.Add(f.Value);
		}

		private void OptionsForm_Load(object sender, EventArgs e)
		{
			comboBoxUpdates.SelectedItem = Configuration.GetItem("UpdateMode", "Automatic");
			comboBoxTaskbar.SelectedItem = TaskbarTypeDisplay[(string)Configuration.GetItem("TaskbarType", "SiB+OS")];
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Configuration.SetItem("UpdateMode", comboBoxUpdates.SelectedItem, RegistryValueKind.String);
			Configuration.SetItem("TaskbarType", comboBoxTaskbar.SelectedItem, RegistryValueKind.String);
		}

		private void comboBoxTaskbar_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((string)comboBoxTaskbar.SelectedItem == TaskbarTypeDisplay["SCTT"] && !Directory.Exists("C:\\SCT\\Taskbar\\") && !File.Exists("C:\\SCT\\Taskbar\\SCT_Taskbar.exe"))
			{
				ClassicTaskbar.InstallSCTT(this);
				if (!File.Exists("C:\\SCT\\Taskbar\\SCT_Taskbar.exe"))
					comboBoxTaskbar.SelectedItem = TaskbarTypeDisplay["SiB+OS"];
				else
					return;
			}
			comboBoxTaskbar.SelectedItem = TaskbarTypeDisplay["SiB+OS"];
		}
	}
}
