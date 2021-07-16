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
    public partial class InstallOptionsPage : WizardPage
    {
        UtilitiesPage page;
        WizardPage nextPage;

        public InstallOptionsPage()
        {
            InitializeComponent();
            page = new UtilitiesPage();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                nextPage = NextPage;
                NextPage = page;
                page.NextPage = nextPage;
                NextButtonText = "";
            }
            else
            {
                NextPage = nextPage;
                NextButtonText = "&Install";
            }
        }

        private void InstallOptionsPage_EnterPage(object sender, EventArgs e)
        {
            SetupHandler.SelectedTaskbarType = SetupHandler.TaskbarType.OS_SiB;
            SetupHandler.ConfigureOSSM = false;
            SetupHandler.ConfigureOSTB = false;
            SetupHandler.ConfigureSiB = false;
            SetupHandler.EnableOnBoot = true;
            SetupHandler.UtilitiesToBeInstalled.Clear();
        }

        private void InstallOptionsPage_LeavePage(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                SetupHandler.SelectedTaskbarType = SetupHandler.TaskbarType.OS_SiB;
                SetupHandler.ConfigureOSSM = true;
                SetupHandler.ConfigureOSTB = true;
                SetupHandler.ConfigureSiB =  true;
                SetupHandler.UtilitiesToBeInstalled.AddRange(new InstallableUtility[] { InstallableUtility.OpenShell, InstallableUtility.StartIsBackPlusPlus });
            }
            if (radioButton2.Checked)
                SetupHandler.SelectedTaskbarType = SetupHandler.TaskbarType.SCTTaskbar;
            if (radioButton3.Checked)
                SetupHandler.SelectedTaskbarType = SetupHandler.TaskbarType.None;

            SetupHandler.EnableOnBoot = checkBox2.Checked;
        }
	}
}
