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

using Craftplacer.ClassicSuite.Wizards.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClassicTheme.SetupWizard
{
    public partial class UtilitiesPage : WizardPage
    {
        List<(string, InstallableUtility)> utilities = new List<(string, InstallableUtility)>();

        public UtilitiesPage()
        {
            InitializeComponent();
        }

        private void UtilitiesPage_EnterPage(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            utilities.Clear();

            List<InstallableUtility> tempUtilities = InstallableUtility.GetInstallableUtilities();
            tempUtilities.Remove(tempUtilities.Where((a) => a.Name == "Open-Shell").FirstOrDefault());
            foreach (InstallableUtility utility in tempUtilities)
                utilities.Add(((utility.IsInstalled ? "(Installed) " : "") + utility.Name, utility));

            if (SetupHandler.SelectedTaskbarType == SetupHandler.TaskbarType.OS_SiB)
            {
                label2.Visible = true;
                label3.Visible = true;
                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                utilities.Remove(utilities.Where((a) => a.Item2.Name == "StartIsBack++").FirstOrDefault());
            }
            else
            {
                label2.Visible = false;
                label3.Visible = false;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
            }

            utilities.Sort((x, y) => { return x.Item1.CompareTo(y.Item1); });
            foreach ((string, InstallableUtility) utility in utilities)
                listBox1.Items.Add(utility.Item1);
        }

		private void UtilitiesPage_LeavePage(object sender, EventArgs e)
		{
            foreach (string s in listBox1.SelectedItems)
                SetupHandler.UtilitiesToBeInstalled.Add(utilities.Where((a) => a.Item1 == s).FirstOrDefault().Item2);

            if (SetupHandler.SelectedTaskbarType != SetupHandler.TaskbarType.OS_SiB)
            {
                if (checkBox1.Checked)
                    SetupHandler.UtilitiesToBeInstalled.Add(InstallableUtility.OpenShell);
                SetupHandler.ConfigureOSSM = checkBox2.Checked;
                SetupHandler.ConfigureOSTB = checkBox3.Checked;
            }
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
            listBox1.ClearSelected();
		}
	}
}
