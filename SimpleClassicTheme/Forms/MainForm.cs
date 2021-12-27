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
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.IO.Compression;
using SimpleClassicTheme.Forms;
using System.Drawing;

namespace SimpleClassicTheme
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ExtraFunctions.UpdateStartupExecutable(false);
            File.WriteAllText(Path.Combine(Path.GetTempPath(), "\\addSchemes.bat"), Properties.Resources.reg_classicschemes_add);
            Process.Start(new ProcessStartInfo() { FileName = Path.Combine(Path.GetTempPath(), "\\addSchemes.bat"), Verb = "runas", UseShellExecute = false, CreateNoWindow = true });

            CheckDependenciesAndSetControls();

            if (ExtraFunctions.ShouldDrawLight(SystemColors.Control))
                pictureBox1.Image = Properties.Resources.sct_light_164;
            else
                pictureBox1.Image = Properties.Resources.sct_dark_164;
        }

		#region Configuration checks

		// Check if all requirements for SCT and, if selected, the classic taskbar are installed
		public static bool CheckDependencies(bool Taskbar)
        {
            if (!Taskbar)
                return true;

            switch (Configuration.TaskbarType)
            {
                case TaskbarType.SimpleClassicThemeTaskbar:
                    return File.Exists("C:\\SCT\\Taskbar\\SimpleClassicThemeTaskbar.exe");
                case TaskbarType.StartIsBackOpenShell:
                    bool osInstalled = Directory.Exists("C:\\Program Files\\Open-Shell\\");
                    bool sibInstalled = Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StartIsBack\\"));
                    return !((!osInstalled || !sibInstalled) && Taskbar);
                case TaskbarType.RetroBar:
                    return File.Exists("C:\\SCT\\RetroBar\\RetroBar.exe");
                case TaskbarType.Windows81Vanilla:
                default:
                    return true;
            }
        }

        // Check dependencies and set control visibilty/usability
        private void CheckDependenciesAndSetControls()
        {
            EnableAllControls();

            if (CheckDependencies(Configuration.EnableTaskbar))
            {
                buttonInstallRequirements.Enabled = false;
            }
            else
            {
                buttonEnable.Enabled = false;
                buttonDisable.Enabled = false;
            }

            // Do a bunch of version/configuration specific checks
            Version OSVersion = Environment.OSVersion.Version;

            // ECMT: Windows 10 x64
            buttonECMT.Enabled = OSVersion.Major == 10 && OSVersion.CompareString("10.0.22000.0") < 0 && IntPtr.Size == 8;
            // ExplorerPatcher: Windows 11 x64
            buttonExplorerPatcher.Enabled = OSVersion.Major == 10 && OSVersion.CompareString("10.0.22000.0") >= 0 && IntPtr.Size == 8;
            // T-Clock: Any version of Windows but only with Taskbar enhancements
            buttonTClock.Enabled = Configuration.EnableTaskbar ||
                               Configuration.TaskbarType == TaskbarType.StartIsBackOpenShell ||
                               Configuration.TaskbarType == TaskbarType.Windows81Vanilla;

            if (Directory.Exists("C:\\SCT\\T-Clock\\"))
                buttonTClock.Text = "Open T-Clock";
        }

        // Enables all controls
        private void EnableAllControls()
        {
            foreach (Control c in Controls)
                if (c is GroupBox g)
                    foreach (Control c2 in g.Controls)
                        c2.Enabled = true;
                else
                    c.Enabled = true;
        }

		#endregion

		#region Button click handlers

		// Enable
		private void ButtonEnable_Click(object sender, EventArgs e)
        {
            bool oldEnableValue = Configuration.Enabled;
            ClassicTheme.MasterEnable(Configuration.EnableTaskbar);
            if (oldEnableValue != Configuration.Enabled)
            {
                ApplicationEntryPoint.LoadGUI = true; Close();
            }
        }

        // Disable
        private void ButtonDisable_Click(object sender, EventArgs e)
        {
            bool oldEnableValue = Configuration.Enabled;
            ClassicTheme.MasterDisable(Configuration.EnableTaskbar);
            if (oldEnableValue != Configuration.Enabled)
            {
                ApplicationEntryPoint.LoadGUI = true; Close();
            }
        }

        // Install dependencies
        private void ButtonInstallRequirements_Click(object sender, EventArgs e)
        {
            bool osInstalled = Directory.Exists("C:\\Program Files\\Open-Shell\\");
            bool sibInstalled = Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StartIsBack\\"));
            if (buttonInstallRequirements.Text == "Reconfigure OS+SiB")
            {
                ExtraFunctions.ReConfigureOS(osInstalled, osInstalled, sibInstalled);
                return;
            }

            ExtraFunctions.InstallDependencies();
            
            //Make sure EVERYTHING is disabled before continuing
            ButtonDisable_Click(sender, e);
            CheckDependenciesAndSetControls();
        }

        // Open Classic Theme CPL
        private void ButtonConfigure_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory("C:\\SCT\\");
            File.WriteAllBytes("C:\\SCT\\deskn.cpl", Properties.Resources.desktopControlPanelCPL);
            Process.Start("C:\\SCT\\deskn.cpl");
        }

        // Auto-launch SCT on boot
        private void ButtonRunOnBoot_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This makes SCT automatically launch when you log onto your PC. You can use the boot scripts in C:\\SCT\\ to configure things to load before Classic Theme gets enabled. Continue?", "Run Simple Clasic Theme on boot", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ExtraFunctions.UpdateStartupExecutable(true);
            }
        }

        // Install ExplorerContextMenuTweaker
        private void ButtonECMT_Click(object sender, EventArgs e)
        {
            buttonECMT.Enabled = false;

            File.WriteAllBytes("C:\\Windows\\System32\\ExplorerContextMenuTweaker.dll", Properties.Resources.ExplorerContextMenuTweaker);
            File.WriteAllBytes("C:\\Windows\\System32\\ShellPayload.dll", Properties.Resources.ShellPayload);
            Process.Start(new ProcessStartInfo() { FileName = "C:\\Windows\\System32\\regsvr32.exe", Arguments = "ExplorerContextMenuTweaker.dll", Verb = "runas" }).WaitForExit();
        }

        // Show ExplorerPatcher UI
        private void ButtonExplorerPatcher_Click(object sender, EventArgs e)
        {
            new ExplorerPatcherForm().ShowDialog(this);
        }

        // Make borders 3D by changing UPM
        private void Button3DBorder_Click(object sender, EventArgs e)
        {
            File.WriteAllText("C:\\SCT\\reg_upm_enable3d.reg", Properties.Resources.reg_upm_enable3d);
            Process.Start("C:\\SCT\\reg_upm_enable3d.reg").WaitForExit();
            File.Delete("C:\\SCT\\reg_upm_enable3d.reg");
        }

        // Open RibbonDisabler 4.0
        private void ButtonRibbonDisabler_Click(object sender, EventArgs e)
        {
            if (!File.Exists("C:\\SCT\\RibbonDisabler.exe"))
            {
                File.WriteAllBytes("C:\\SCT\\RibbonDisabler.exe", Properties.Resources.ribbonDisabler);
            }
            Process.Start("C:\\SCT\\RibbonDisabler.exe");
        }

        // Install T-Clock
        private void ButtonTClock_Click(object sender, EventArgs e)
        {
            if (buttonTClock.Text == "Install T-Clock")
            {
                using (WebClient c = new WebClient())
                {
                    c.DownloadFile("https://github.com/White-Tiger/T-Clock/releases/download/v2.4.4%23492-rc/T-Clock.zip", "C:\\SCT\\t-clock.zip");
                }
                Directory.CreateDirectory("C:\\SCT\\T-Clock\\");
                ZipFile.ExtractToDirectory("C:\\SCT\\t-clock.zip", "C:\\SCT\\T-Clock\\");
                File.Delete("C:\\SCT\\t-clock.zip");
                MessageBox.Show("T-Clock has been installed on your system");
                buttonTClock.Text = "Open T-Clock";
            }
            else
            {
                Process.Start("C:\\SCT\\T-Clock\\Clock64.exe");
            }
        }

        // Run the SCT AHK script manager
        private void ButtonAHKScripts_Click(object sender, EventArgs e)
        {
            new AHKScriptManager().ShowDialog();
        }

        // Run the SCT utility manager
        private void ButtonUtilities_Click(object sender, EventArgs e)
        {
            new UtilityManagerForm().ShowDialog();
        }

        // Restore all window settings
        private void ButtonRestoreWindowSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "This action will log you out. Continue?", "Simple Classic Theme", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            //Put Windows Aero scheme on
            File.WriteAllText("C:\\SCT\\reg_windowcolors_restore.reg", Properties.Resources.reg_windowcolors_restore);
            Process.Start("C:\\Windows\\System32\\reg.exe", "import C:\\SCT\\reg_windowcolors_restore.reg").WaitForExit();
            Process.Start("C:\\Windows\\Resources\\Themes\\aero.theme").WaitForExit();

            //Restore WindowMetrics
            File.WriteAllText("C:\\SCT\\reg_windowmetrics_restore.reg", Environment.OSVersion.Version.Major == 10 ? Properties.Resources.reg_windowmetrics_restore : Properties.Resources.reg_windowmetrics_81);
            Process.Start("C:\\Windows\\System32\\reg.exe", "import C:\\SCT\\reg_windowmetrics_restore.reg").WaitForExit();

            System.Threading.Thread.Sleep(2000);
            User32.ExitWindowsEx(0 | 0x00000004, 0);
        }

		// Uninstall SCT
		private void ButtonUninstall_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This restores all default theme settings and restart you PC.\nContinue?", "SCT Uninstallation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ClassicTheme.RemoveSCT();
            }
        }

        #endregion

		#region ToolStrip click handlers

		//Exit
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Open guide
        private void guideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://winclassic.github.io/sctguide");
        }

        //Open github issues page
        private void reportBugsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/WinClassic/SimpleClassicTheme/issues");
        }

        //Show about dialog
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog(this);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/WinClassic/SimpleClassicTheme/issues");
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new OptionsForm().ShowDialog(this);
            CheckDependenciesAndSetControls();
        }

		#endregion
	}
}
