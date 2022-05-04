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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClassicTheme.Forms
{
	public partial class TaskbarTypeSelector : UserControl
	{
		public event EventHandler SelectedItemChanged;
		public TaskbarType SelectedItem => comboBoxTaskbar.SelectedIndex != -1 ? ((TaskbarTypeItem)comboBoxTaskbar.Items[comboBoxTaskbar.SelectedIndex]).Value : TaskbarType.None;

		public class TaskbarTypeItem
        {
			public string Text { get; set; }
			public string Description { get; set; }
			public string Author { get; set; }
			public string AuthorLink { get; set; }
			public string Support { get; set; }
			public string SupportLink { get; set; }
			public TaskbarType Value { get; set; }

            public override string ToString()
            {
				return Text;
            }
        }

		public TaskbarTypeSelector()
        {
			InitializeComponent();

			comboBoxTaskbar.Items.AddRange(new[]
			{
				new TaskbarTypeItem
				{
					Text = "RetroBar",
					Description = "RetroBar is the most refined taskbar alternative out there. Although it lacks certain functionality that taskbars like SCTT and the Windows taskbar have, it provides with a very stable and smooth taskbar featuring multiple themes to match your style.",
					Author = "dremin/scj312",
					AuthorLink = "https://github.com/dremin/",
					Support = "GitHub Issues",
					SupportLink = "https://github.com/dremin/RetroBar/issues",
					Value = TaskbarType.RetroBar,
				},
				new TaskbarTypeItem
				{
					Text = "Simple Classic Theme Taskbar",
					Description = "RetroBar is the most refined taskbar alternative out there. Although it lacks certain functionality that taskbars like SCTT and the Windows taskbar have, it provides with a very stable and smooth taskbar featuring multiple themes to match your style.",
					Author = "Leet",
					AuthorLink = "https://github.com/AEAEAEAE4343/",
					Support = "GitHub Issues",
					SupportLink = "https://github.com/WinClassic/SimpleClassicTheme/issues",
					Value = TaskbarType.SimpleClassicThemeTaskbar,
				},
			});

			if (Environment.OSVersion.Version.CompareString("6.3") == 0)
				comboBoxTaskbar.Items.Add(new TaskbarTypeItem
				{
					Text = "Vanilla taskbar",
					Description = "Takes the default Windows 8.1 taskbar and forces Classic Theme on it. This fails, so SCT also patches it. The end result is a pretty much perfect Windows 7 Classic Taskbar.",
					Author = "Leet",
					AuthorLink = "https://github.com/AEAEAEAE4343/",
					Support = "GitHub Issues",
					SupportLink = "https://github.com/WinClassic/SimpleClassicTheme/issues",
					Value = TaskbarType.Windows81Vanilla,
				});
		}

		public TaskbarTypeSelector(TaskbarType selectedTaskbar = TaskbarType.RetroBar) : this()
		{
			comboBoxTaskbar.SelectedItem = comboBoxTaskbar.Items.OfType<TaskbarTypeItem>().Where((a) => a.Value == selectedTaskbar).FirstOrDefault();
		}

		private void comboBoxTaskbar_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBoxTaskbar.SelectedIndex == -1)
				return;

			TaskbarTypeItem item = (TaskbarTypeItem)comboBoxTaskbar.Items[comboBoxTaskbar.SelectedIndex];

			label1.Text = item.Description;
			linkLabel1.Links.Clear();
			string a = item.Author;
			string b = item.Support;
			string text = $"Author: (a) Support: (b)";
			linkLabel1.Links.Add(text.IndexOf("(a)"), a.Length, item.AuthorLink);
			text = text.Replace("(a)", a);
			linkLabel1.Links.Add(text.IndexOf("(b)"), b.Length, item.SupportLink);
			text = text.Replace("(b)", b);
			linkLabel1.Text = text;

			SelectedItemChanged?.Invoke(this, EventArgs.Empty);
		}

		private void TaskbarTypeSelector_EnabledChanged(object sender, EventArgs e)
		{
			comboBoxTaskbar.Enabled = Enabled;
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start((string)e.Link.LinkData);
		}
	}
}
