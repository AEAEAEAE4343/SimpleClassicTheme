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
		public TaskbarType SelectedItem => TaskbarTypeDisplay.FirstOrDefault(x => x.Value == (string)comboBoxTaskbar.SelectedItem).Key;

		public Dictionary<TaskbarType, string> TaskbarTypeDisplay = new Dictionary<TaskbarType, string>();
		public Dictionary<TaskbarType, string> TaskbarTypeDescription = new Dictionary<TaskbarType, string>();
		public Dictionary<TaskbarType, string> TaskbarTypeAuthor = new Dictionary<TaskbarType, string>();
		public Dictionary<TaskbarType, string> TaskbarTypeAuthorLink = new Dictionary<TaskbarType, string>();
		public Dictionary<TaskbarType, string> TaskbarTypeSupport = new Dictionary<TaskbarType, string>();
		public Dictionary<TaskbarType, string> TaskbarTypeSupportLink = new Dictionary<TaskbarType, string>();

		public TaskbarTypeSelector() : this(TaskbarType.SimpleClassicThemeTaskbar) { }
		public TaskbarTypeSelector(TaskbarType selectedTaskbar)
		{
			TaskbarTypeDisplay.Add(TaskbarType.SimpleClassicThemeTaskbar, "Simple Classic Theme Taskbar");
			TaskbarTypeDisplay.Add(TaskbarType.RetroBar, "RetroBar");
			if (Environment.OSVersion.Version.CompareString("6.3") == 0)
				TaskbarTypeDisplay.Add(TaskbarType.Windows81Vanilla, "Vanilla taskbar");
			if (Environment.OSVersion.Version.CompareString("10.0") == 0)
				TaskbarTypeDisplay.Add(TaskbarType.StartIsBackOpenShell, "StartIsBack with Open-Shell");

			TaskbarTypeDescription.Add(TaskbarType.StartIsBackOpenShell, "StartIsBack is a utility that brings back old quirks of the taskbar into newer versions of Windows. When combined with Open-Shell (a tool to bring back classic UI design to windows) you get a very convincing Classic Taskbar.");
			TaskbarTypeDescription.Add(TaskbarType.Windows81Vanilla, "Takes the default Windows 8.1 taskbar and forces Classic Theme on it. This fails, so SCT also patches it. The end result is a pretty much perfect Windows 7 Classic Taskbar.");
			TaskbarTypeDescription.Add(TaskbarType.SimpleClassicThemeTaskbar, "Simple Classic Theme Taskbar is a taskbar designed specifically for SCT. From the ground up it is designed to mimic a Win2K Classic Taskbar and it does so extremely good. Features built-in XP .msstyles support. Note that this taskbar requires more power out of your system than similar tools.");
			TaskbarTypeDescription.Add(TaskbarType.RetroBar, "RetroBar is the most refined taskbar alternative out there. Although it lacks certain functionality that taskbars like SCTT and the Windows taskbar have, it provides with a very stable and smooth taskbar featuring multiple themes to match your style.");

			TaskbarTypeAuthor.Add(TaskbarType.StartIsBackOpenShell, "Spitfire_x86");
			TaskbarTypeAuthorLink.Add(TaskbarType.StartIsBackOpenShell, "https://winclassic.boards.net/thread/280/classic-taskbar-superbar-startisback");
			TaskbarTypeSupport.Add(TaskbarType.StartIsBackOpenShell, "WinClassic");
			TaskbarTypeSupportLink.Add(TaskbarType.StartIsBackOpenShell, "https://winclassic.boards.net/thread/280/classic-taskbar-superbar-startisback");

			TaskbarTypeAuthor.Add(TaskbarType.Windows81Vanilla, "Leet");
			TaskbarTypeAuthorLink.Add(TaskbarType.Windows81Vanilla, "https://github.com/AEAEAEAE4343/");
			TaskbarTypeSupport.Add(TaskbarType.Windows81Vanilla, "GitHub Issues");
			TaskbarTypeSupportLink.Add(TaskbarType.Windows81Vanilla, "https://github.com/WinClassic/SimpleClassicTheme");

			TaskbarTypeAuthor.Add(TaskbarType.SimpleClassicThemeTaskbar, "Leet");
			TaskbarTypeAuthorLink.Add(TaskbarType.SimpleClassicThemeTaskbar, "https://github.com/AEAEAEAE4343/");
			TaskbarTypeSupport.Add(TaskbarType.SimpleClassicThemeTaskbar, "GitHub Issues");
			TaskbarTypeSupportLink.Add(TaskbarType.SimpleClassicThemeTaskbar, "https://github.com/WinClassic/SimpleClassicTheme");
			
			TaskbarTypeAuthor.Add(TaskbarType.RetroBar, "dremin/scj312");
			TaskbarTypeAuthorLink.Add(TaskbarType.RetroBar, "https://github.com/dremin/");
			TaskbarTypeSupport.Add(TaskbarType.RetroBar, "GitHub Issues");
			TaskbarTypeSupportLink.Add(TaskbarType.RetroBar, "https://github.com/dremin/RetroBar/issues");

			InitializeComponent();

			foreach (KeyValuePair<TaskbarType, string> f in TaskbarTypeDisplay)
				comboBoxTaskbar.Items.Add(f.Value);
			comboBoxTaskbar.SelectedItem = TaskbarTypeDisplay[TaskbarTypeDisplay.ContainsKey(selectedTaskbar) ? selectedTaskbar : TaskbarType.SimpleClassicThemeTaskbar];
		}

		private void comboBoxTaskbar_SelectedIndexChanged(object sender, EventArgs e)
		{
			label1.Text = TaskbarTypeDescription[SelectedItem];
			linkLabel1.Links.Clear();
			string a = TaskbarTypeAuthor[SelectedItem];
			string b = TaskbarTypeSupport[SelectedItem];
			string text = $"Author: (a) Support: (b)";
			linkLabel1.Links.Add(text.IndexOf("(a)"), a.Length, TaskbarTypeAuthorLink[SelectedItem]);
			text = text.Replace("(a)", a);
			linkLabel1.Links.Add(text.IndexOf("(b)"), b.Length, TaskbarTypeSupportLink[SelectedItem]);
			text = text.Replace("(b)", b);
			linkLabel1.Text = text;

			SelectedItemChanged?.Invoke(this, EventArgs.Empty);
		}

		private void TaskbarTypeSelector_Load(object sender, EventArgs e)
		{
			comboBoxTaskbar.SelectedItem = TaskbarTypeDisplay[TaskbarTypeDisplay.ContainsKey(Configuration.TaskbarType) ? Configuration.TaskbarType : TaskbarType.SimpleClassicThemeTaskbar];
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
