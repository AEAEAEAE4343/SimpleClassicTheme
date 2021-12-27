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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClassicTheme
{
	public partial class AHKScriptManager : Form
	{
		public class AHKScript
		{
			public string Name;
			public string Filename;
			public string ResourceName;

			public string GetScript => Properties.AHKScripts.ResourceManager.GetString(ResourceName);
			public override string ToString() => Name;
		}

		public AHKScriptManager()
		{
			InitializeComponent();
			
			if (!Directory.Exists("C:\\SCT\\AHK"))
				Directory.CreateDirectory("C:\\SCT\\AHK");

			ListScripts();
		}

		public void ListScripts()
		{
			listBox1.Items.Clear();
			foreach (string f in Directory.EnumerateFiles("C:\\SCT\\AHK"))
				listBox1.Items.Add(Path.GetFileName(f));
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Form scriptSourceSelection = new Form()
			{
				Controls =
				{
					new Label()
					{
						Location = new Point(12, 12),
						Text = "Choose script source",
						AutoSize = true
					},
					new Button()
					{
						Location = new Point(12, 38),
						Size = new Size(75, 23),
						Text = "Preloaded",
						Name = "ButtonPreloaded"
					},
					new Button()
					{
						Location = new Point(93, 38),
						Size = new Size(75, 23),
						Text = "Browse",
						Name = "ButtonBrowse"
					}
				},
				MaximizeBox = false,
				MinimizeBox = false,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				StartPosition = FormStartPosition.CenterParent,
				ClientSize = new Size(180, 73),
				Text = "Simple Classic Theme - AutoHotKey Script Manager"
			};
			Button ButtonPreloaded = (Button)scriptSourceSelection.Controls.Find("ButtonPreloaded", true).FirstOrDefault();
			Button ButtonBrowse = (Button)scriptSourceSelection.Controls.Find("ButtonBrowse", true).FirstOrDefault();
			
			ButtonPreloaded.Click += delegate
			{
				Form preloadedScriptSelection = new Form()
				{
					Controls =
					{
						new ListBox()
						{
							Location = new Point(12, 12),
							Size = new Size(423, 251),
							Name = "ListBoxItems"
						},
						new Button()
						{
							Location = new Point(360, 276),
							Size = new Size(75, 23),
							Text = "Select",
							Name = "ButtonSelect"
						}
					},
					MaximizeBox = false,
					MinimizeBox = false,
					FormBorderStyle = FormBorderStyle.FixedDialog,
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(453, 340),
					Text = "SCT AutoHotKey Script Manager",
					Icon = Icon
				};
				ListBox ListBoxItems = (ListBox)preloadedScriptSelection.Controls.Find("ListBoxItems", true).FirstOrDefault();
				Button ButtonSelect = (Button)preloadedScriptSelection.Controls.Find("ButtonSelect", true).FirstOrDefault();

				ListBoxItems.Items.AddRange(new AHKScript[] 
				{
					new AHKScript() { Name = "Explorer ClientEdge",				Filename = "clientedge.ahk",	ResourceName = "clientedge" },
					new AHKScript() { Name = "Explorer Remove navigaiton bar",	Filename = "noaddressbar.ahk",	ResourceName = "noaddressbar" },
					new AHKScript() { Name = "Explorer Quero hotkeys",			Filename = "querohotkeys.ahk",	ResourceName = "querohotkeys" }
				});

				ButtonSelect.Click += delegate
				{ 
					if (ListBoxItems.SelectedIndex == -1)
					{
						MessageBox.Show("Please select an item before continuing");
					}
					else
					{
						AHKScript script = ListBoxItems.SelectedItem as AHKScript;
						string path = script.Filename;
						while (File.Exists("C:\\SCT\\AHK\\" + path))
						{
							path = "_" + path;
						}
						path = "C:\\SCT\\AHK\\" + path;
						File.WriteAllText(path, script.GetScript);
						preloadedScriptSelection.Close();
					}
				};
				preloadedScriptSelection.ShowDialog(scriptSourceSelection);
				scriptSourceSelection.Close();
			};
			ButtonBrowse.Click += delegate
			{
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.Filter = "AHK Scripts|*.ahk|Other files|*.*";
				if (ofd.ShowDialog(scriptSourceSelection) == DialogResult.OK)
				{
					string path = Path.GetFileName(ofd.FileName);
					while (File.Exists("C:\\SCT\\AHK\\" + path))
					{
						path = "_" + path;
					}
					path = "C:\\SCT\\AHK\\" + path;
					File.Copy(ofd.FileName, path, true);
				}
				scriptSourceSelection.Close();
			};
			scriptSourceSelection.ShowDialog(this);
			ListScripts();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1)
				return;

			DateTime time = DateTime.Now;
			string processName = Path.GetFileNameWithoutExtension((string)listBox1.SelectedItem);
			while (Process.GetProcessesByName(processName).Length > 0 && DateTime.Now.Subtract(time).TotalSeconds < 5)
				if (Process.GetProcessesByName(processName)[0].MainModule.FileName == "C:\\SCT\\AHK\\" + listBox1.SelectedItem)
					Process.GetProcessesByName(processName)[0].Kill();

			if (Process.GetProcessesByName((string)listBox1.SelectedItem).Length > 0)
				MessageBox.Show($"Could not kill and delete {listBox1.SelectedItem}. Timed out.");
			else
			{
				File.Delete("C:\\SCT\\AHK\\" + listBox1.SelectedItem);
				ListScripts(); 
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}

        private void button4_Click(object sender, EventArgs e)
        {
			if (MessageBox.Show(this, "SCT.FEH is a general purpose hook File Explorer on Windows 10. It replaces the functionality of some popular AHK scripts. Would you like to continue?", "SCT.FEH", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
				new GithubDownloader(GithubDownloader.DownloadableGithubProject.SimpleClassicThemeFEH).ShowDialog(this);
            }
        }
    }
}
