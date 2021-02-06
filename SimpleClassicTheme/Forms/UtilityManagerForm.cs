using System;
using System.Collections.Generic;
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

			List<InstallableUtility> tempUtilities = new List<InstallableUtility>();
			tempUtilities.Add(InstallableUtility.SevenPlusTaskbarTweaker);
			tempUtilities.Add(InstallableUtility.StartIsBackPlusPlus);
			tempUtilities.Add(InstallableUtility.ClassicTaskManager);
			tempUtilities.Add(InstallableUtility.FolderOptionsX);
			tempUtilities.Add(InstallableUtility.OpenShell);

			foreach (InstallableUtility utility in tempUtilities)
				utilities.Add(((utility.IsInstalled ? "(Installed) " : "") + utility.Name, utility));

			utilities.Sort((x, y) => { return x.Item1.CompareTo(y.Item1); });

			foreach ((string, InstallableUtility) utility in utilities)
				listBox1.Items.Add(utility.Item1);
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
	}
}
