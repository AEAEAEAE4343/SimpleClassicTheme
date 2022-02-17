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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;

namespace SimpleClassicTheme.Forms
{
	public partial class UtilityManagerForm : Form
	{
		List<(string, InstallableUtility)> utilities = new List<(string, InstallableUtility)>();

		public UtilityManagerForm()
		{
			InitializeComponent();
			ListUtilities();
		}

		public void ListUtilities()
		{
			utilities.Clear();
			listBox1.Items.Clear();

			List<InstallableUtility> tempUtilities = InstallableUtility.GetInstallableUtilities();

			foreach (InstallableUtility utility in tempUtilities)
				utilities.Add(((utility.IsInstalled ? "(Installed) " : "") + utility.Name, utility));

			utilities.Sort((x, y) => { return x.Item1.CompareTo(y.Item1); });

			foreach ((string, InstallableUtility) utility in utilities)
				listBox1.Items.Add(utility.Item1);

			buttonOpenTClock.Enabled = Directory.Exists($"{Configuration.InstallPath}T-Clock\\");
			buttonInstallTClock.Text = buttonOpenTClock.Enabled ? "Uninstall" : "Install";

			// T-Clock: Any version of Windows but only with Taskbar enhancements
			groupBox1.Enabled = Configuration.EnableTaskbar &&
							   (Configuration.TaskbarType == TaskbarType.StartIsBackOpenShell ||
							   Configuration.TaskbarType == TaskbarType.Windows81Vanilla);
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex != -1)
			{
				bool isInstalled = ((string)listBox1.SelectedItem).StartsWith("(Installed) ");
				button2.Enabled = !isInstalled;
				button3.Enabled = isInstalled;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			int index = listBox1.SelectedIndex;
			InstallableUtility utility = utilities[index].Item2;
			int returncode = utility.Install();
			if (returncode != 0)
			{
				MessageBox.Show($"The installation has failed. (0x{returncode:X8})", "SCT Classic Utility Manager");
			}
			else
			{
				MessageBox.Show($"The installation was succesful. (0x{returncode:X8})", "SCT Classic Utility Manager");
			}
			ListUtilities();
			BringToFront();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			int index = listBox1.SelectedIndex;
			InstallableUtility utility = utilities[index].Item2;
			int returncode = utility.Uninstall();
			if (returncode != 0)
			{
				MessageBox.Show($"The removal has failed. (0x{returncode:X8})", "SCT Classic Utility Manager");
			}
			else
			{
				MessageBox.Show($"The removal was succesful. (0x{returncode:X8})", "SCT Classic Utility Manager");
			}
			ListUtilities();
			BringToFront();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}

        private void buttonInstallTClock_Click(object sender, EventArgs e)
        {
			if (!buttonOpenTClock.Enabled)
			{
				using (WebClient c = new WebClient())
				{
					c.DownloadFile("https://github.com/White-Tiger/T-Clock/releases/download/v2.4.4%23492-rc/T-Clock.zip", $"{Configuration.InstallPath}t-clock.zip");
				}
				Directory.CreateDirectory($"{Configuration.InstallPath}T-Clock\\");
				ZipFile.ExtractToDirectory($"{Configuration.InstallPath}t-clock.zip", $"{Configuration.InstallPath}T-Clock\\");
				File.Delete($"{Configuration.InstallPath}t-clock.zip");
			}
			else
            {
				Directory.Delete($"{Configuration.InstallPath}T-Clock\\", true);
            }
			ListUtilities();
			BringToFront();
		}

        private void buttonOpenTClock_Click(object sender, EventArgs e)
        {
			Process.Start($"{Configuration.InstallPath}T-Clock\\Clock64.exe");
		}
    }
}
