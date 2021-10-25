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
			InitializeComponent();
			Load += OptionsForm_Load;
		}

		private void OptionsForm_Load(object sender, EventArgs e)
		{
			comboBoxUpdates.SelectedItem = Configuration.UpdateMode;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Configuration.UpdateMode = (string)comboBoxUpdates.SelectedItem;
			Configuration.TaskbarType = taskbarTypeSelector1.SelectedItem;
			button2.Enabled = false;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			button2_Click(sender, e);
			Close();
		}

		private void comboBoxUpdates_SelectedIndexChanged(object sender, EventArgs e)
		{
			button2.Enabled = true;
		}

		private void taskbarTypeSelector1_SelectedItemChanged(object sender, EventArgs e)
		{
			button2.Enabled = true;
		}
	}
}
