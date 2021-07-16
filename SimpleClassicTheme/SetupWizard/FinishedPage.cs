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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClassicTheme.SetupWizard
{
    public partial class FinishedPage : WizardPage
    {
        public FinishedPage(bool failed = false)
        {
            InitializeComponent();
            if (failed)
			{
                label1.Text = "Setup Wizard Failed";
                label2.Text = "The wizard was interrupted before Simple Classic Theme could be completely installed.\n\nYour system has not been modified. To install this program at a later time, please run the wizard again.\n\n\n\nClick Finish to exit the wizard.";
                NextPageRequested += delegate
                {
                    Environment.Exit(0);
                };
			}
        }

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
            Process.Start("https://github.com/Craftplacer/ClassicSuite");
		}
	}
}
