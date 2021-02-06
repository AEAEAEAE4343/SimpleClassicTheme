/*
 *  SimpleClassicTheme, a basic utility to bring back classic theme to newer version of the Windows operating system.
 *  Copyright (C) 2020 Anis Errais
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SimpleClassicTheme.Forms
{
	public partial class OptionsForm : Form
	{
		public Dictionary<string, string> TaskbarTypeDisplay = new Dictionary<string, string>();

		public OptionsForm()
		{
			TaskbarTypeDisplay.Add("SiB+OS", Environment.OSVersion.Version.Major == 10 ? "SiB and OS" : "Vanilla taskbar");
			TaskbarTypeDisplay.Add("SCTT", "SCT Taskbar (alpha)");

			InitializeComponent();
			Load += OptionsForm_Load;

			foreach (KeyValuePair<string, string> f in TaskbarTypeDisplay)
				comboBoxTaskbar.Items.Add(f.Value);
		}

		private void OptionsForm_Load(object sender, EventArgs e)
		{
			comboBoxUpdates.SelectedItem = Configuration.GetItem("UpdateMode", "Automatic");
			comboBoxTaskbar.SelectedItem = TaskbarTypeDisplay[TaskbarTypeDisplay.ContainsKey((string)Configuration.GetItem("TaskbarType", "SiB+OS")) ? (string)Configuration.GetItem("TaskbarType", "SiB+OS") : "SiB+OS"];
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Configuration.SetItem("UpdateMode", comboBoxUpdates.SelectedItem, RegistryValueKind.String);
			Configuration.SetItem("TaskbarType", TaskbarTypeDisplay.FirstOrDefault(x => x.Value == (string)comboBoxTaskbar.SelectedItem).Key, RegistryValueKind.String);
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
			else if (Directory.Exists("C:\\SCT\\Taskbar\\") && File.Exists("C:\\SCT\\Taskbar\\SCT_Taskbar.exe"))
				return;
			comboBoxTaskbar.SelectedItem = TaskbarTypeDisplay["SiB+OS"];
		}

		private void button1_Click(object sender, EventArgs e)
		{
			button2_Click(sender, e);
			Close();
		}
	}
}
