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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClassicTheme.SetupWizard
{
    public partial class LicensePage : WizardPage
    {
        public LicensePage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".txt";
            File.WriteAllText(fileName, textBox1.Text);

            ProcessStartInfo psi = new ProcessStartInfo(fileName);
            psi.Verb = "PRINT";
            Process.Start(psi);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            AllowedButtons = checkBox1.Checked ? 
                Craftplacer.ClassicSuite.Wizards.Enums.AllowedButtons.All : 
                Craftplacer.ClassicSuite.Wizards.Enums.AllowedButtons.None;
        }
    }
}
