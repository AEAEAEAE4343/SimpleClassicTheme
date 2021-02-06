using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
				Text = "SCT AutoHotKey Script Manager"
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
					Text = "SCT AutoHotKey Script Manager"
				};
				ListBox ListBoxItems = (ListBox)preloadedScriptSelection.Controls.Find("ListBoxItems", true).FirstOrDefault();
				Button ButtonSelect = (Button)preloadedScriptSelection.Controls.Find("ButtonSelect", true).FirstOrDefault();

				ListBoxItems.Items.AddRange(new AHKScript[] 
				{
					new AHKScript() { Name = "Explorer Clientedge",				Filename = "clientedge.ahk",	ResourceName = "clientedge" },
					new AHKScript() { Name = "Explorer Remove address bar",		Filename = "noaddressbar.ahk",	ResourceName = "noaddressbar" },
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
			File.Delete("C:\\SCT\\AHK\\" + listBox1.SelectedValue);
			ListScripts();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
