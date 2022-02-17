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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClassicTheme.SetupWizard
{
    public partial class InstallationPage : WizardPage
    {
        public string progressText = "";
        public BackgroundWorker progressWorker => bgWork;

        public InstallationPage()
        {
            InitializeComponent();
        }

        private void InstallationPage_EnterPage(object sender, EventArgs e)
        {
            bgWork.RunWorkerAsync();
        }

        public void SetProgressBarColor(int state)
		{
            progressBar1.SetState(state);
		}

        private void bgWork_DoWork(object sender, DoWorkEventArgs e)
        {
#if DEBUG
            progressText = "Installing Simple Classic Theme...";
            progressWorker.ReportProgress(0);
            System.Threading.Thread.Sleep(5000);
            progressWorker.ReportProgress(100);
#else
            SetupHandler.InstallSCT(this);
#endif
        }

        private void bgWork_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label3.Text = progressText;
            progressBar1.Value = e.ProgressPercentage;
        }

        private void bgWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (progressBar1.Value == 100)
                OnNextPageRequested();
        }
    }

    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }
}
