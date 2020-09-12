using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SimpleClassicTheme.Forms
{
	public partial class OptionsForm : Form
	{
		public OptionsForm()
		{
			InitializeComponent();
			Load += OptionsForm_Load;
		}

		private void OptionsForm_Load(object sender, EventArgs e)
		{
			comboBoxUpdates.SelectedItem = Configuration.GetItem("UpdateMode", "Automatic");
			comboBoxTaskbar.SelectedItem = Configuration.GetItem("TaskbarType", "SiB and OS");
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
			if ((string)comboBoxTaskbar.SelectedItem == "SCT Taskbar (beta)")
				if (!Directory.Exists("C:\\SCT\\Taskbar\\") && !File.Exists("C:\\SCT\\Taskbar\\SCT_Taskbar.exe"))
				{
					ClassicTaskbar.InstallSCTT(this);
					if (!File.Exists("C:\\SCT\\Taskbar\\SCT_Taskbar.exe"))
						comboBoxTaskbar.SelectedItem = "SiB and OS";
				}
		}
	}
}
