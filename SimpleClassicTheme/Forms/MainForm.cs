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
using Microsoft.Win32;
using System.Net;
using System.IO.Compression;
using SimpleClassicTheme.Forms;
using System.Drawing;

namespace SimpleClassicTheme
{
    public partial class MainForm : Form
    {
        //Check installation of Open Shell and StartIsBack++
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

        //Main code: Construct and initialize all controls
        public MainForm()
        {
            InitializeComponent();
        }

        //Main code: loads the UI
        private void Form1_Load(object sender, EventArgs e)
        {
            ExtraFunctions.UpdateStartupExecutable(false);
            File.WriteAllText(Path.Combine(Path.GetTempPath(), "\\addSchemes.bat"), Properties.Resources.reg_classicschemes_add);
            Process.Start(new ProcessStartInfo() { FileName = Path.Combine(Path.GetTempPath(), "\\addSchemes.bat"), Verb = "runas", UseShellExecute = false, CreateNoWindow = true });

            Shown += Form1_Shown;

            checkBox1.Checked = Configuration.EnableTaskbar;
            numericUpDown1.Maximum = Int32.MaxValue;
            numericUpDown1.Value = Configuration.TaskbarDelay;
            CheckDependenciesAndSetControls();

            if (ExtraFunctions.ShouldDrawLight(SystemColors.Control))
                pictureBox1.Image = Properties.Resources.sct_light_164;
            else
                pictureBox1.Image = Properties.Resources.sct_dark_164;
        }

        //Make the window itself use Classic Theme regardless of the current theme
        private void Form1_Shown(object sender, EventArgs e)
        {
            //UxTheme.SetWindowTheme(Handle, " ", " ");
        }

        //Enable
        private void Button1_Click(object sender, EventArgs e)
        {
            bool oldEnableValue = Configuration.Enabled;
            ClassicTheme.MasterEnable(checkBox1.Checked);
            if (oldEnableValue != Configuration.Enabled)
            {
                ApplicationEntryPoint.LoadGUI = true; Close();
            }
        }

        //Disable
        private void Button2_Click(object sender, EventArgs e)
        {
            bool oldEnableValue = Configuration.Enabled;
            ClassicTheme.MasterDisable(checkBox1.Checked);
            if (oldEnableValue != Configuration.Enabled)
            {
                ApplicationEntryPoint.LoadGUI = true; Close();
            }
        }

        //Install dependencies
        private void Button3_Click(object sender, EventArgs e)
        {
            bool osInstalled = Directory.Exists("C:\\Program Files\\Open-Shell\\");
            bool sibInstalled = Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StartIsBack\\"));
            if (button3.Text == "Reconfigure OS+SiB")
            {
                ExtraFunctions.ReConfigureOS(osInstalled, osInstalled, sibInstalled);
                return;
            }

            ExtraFunctions.InstallDependencies();
            
            //Make sure EVERYTHING is disabled before continuing
            Button2_Click(sender, e);
            CheckDependenciesAndSetControls();
        }

        //Open Classic Theme CPL
        private void Button4_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory("C:\\SCT\\");
            File.WriteAllBytes("C:\\SCT\\deskn.cpl", Properties.Resources.desktopControlPanelCPL);
            Process.Start("C:\\SCT\\deskn.cpl");
        }

        //Enable install mode
        private void Button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This makes SCT automatically launch when you log onto your PC. You can use the boot scripts in C:\\SCT\\ to configure things to load before Classic Theme gets enabled. Continue?", "Run Simple Clasic Theme on boot", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ExtraFunctions.UpdateStartupExecutable(true);
            }
        }

        //Install ExplorerContextMenuTweaker
        private void Button9_Click(object sender, EventArgs e)
        {
            button9.Enabled = false;
            if (IntPtr.Size != 8)
			{
                MessageBox.Show("ExplorerContextMenuTweaker is only supported on 64-bit systems");
			}
            File.WriteAllBytes("C:\\Windows\\System32\\ExplorerContextMenuTweaker.dll", Properties.Resources.ExplorerContextMenuTweaker);
            File.WriteAllBytes("C:\\Windows\\System32\\ShellPayload.dll", Properties.Resources.ShellPayload);
            Process.Start(new ProcessStartInfo() { FileName = "C:\\Windows\\System32\\regsvr32.exe", Arguments = "ExplorerContextMenuTweaker.dll", Verb = "runas" }).WaitForExit();
        }

        //Turn on/off taskbar
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.EnableTaskbar = checkBox1.Checked;
            CheckDependenciesAndSetControls();
        }

        //Check dependencies and set control visibilty/usability
        private void CheckDependenciesAndSetControls()
        {
            EnableAllControls();

            if (CheckDependencies(checkBox1.Checked))
            {
                button3.Enabled = false;
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
            
            if (Directory.Exists("C:\\SCT\\T-Clock\\"))
                button10.Text = "Open T-Clock";
            if (Environment.OSVersion.Version.Major < 10)
            {
                button3.Hide();
                label1.Enabled = true; ;
                checkBox1.Hide();
                numericUpDown1.Enabled = true;
            }

            Version OSVersion = Environment.OSVersion.Version;
            int Architecture = IntPtr.Size == 8 ? 64 : 32;

            button9.Enabled = OSVersion.CompareString("10.0.22000") < 0 && IntPtr.Size == 8;
            numericUpDown1.Enabled = checkBox1.Checked;
            
            // T-Clock
            button10.Enabled = Configuration.EnableTaskbar ||
                               Configuration.TaskbarType == TaskbarType.StartIsBackOpenShell ||
                               Configuration.TaskbarType == TaskbarType.Windows81Vanilla;
        }

        //Enables all controls
        private void EnableAllControls()
        {
            foreach (Control c in Controls)
                if (c is GroupBox g)
                    foreach (Control c2 in g.Controls)
                        c2.Enabled = true;
                else
                    c.Enabled = true;
        }

        //Disables all controls
        private void DisableAllControls()
        {
            foreach (Control c in Controls)
                if (!(c is GroupBox))
                    c.Enabled = false;
        }

        //Install T-Clock
        private void Button10_Click(object sender, EventArgs e)
        {
            if (button10.Text == "Install T-Clock")
            {
                using (WebClient c = new WebClient())
                {
                    c.DownloadFile("https://github.com/White-Tiger/T-Clock/releases/download/v2.4.4%23492-rc/T-Clock.zip", "C:\\SCT\\t-clock.zip");
                }
                Directory.CreateDirectory("C:\\SCT\\T-Clock\\");
                ZipFile.ExtractToDirectory("C:\\SCT\\t-clock.zip", "C:\\SCT\\T-Clock\\");
                File.Delete("C:\\SCT\\t-clock.zip");
                MessageBox.Show("T-Clock has been installed on your system");
                button10.Text = "Open T-Clock";
            }
            else
            {
                Process.Start("C:\\SCT\\T-Clock\\Clock64.exe");
            }
        }

        //Open RibbonDisabler 4.0
        private void Button12_Click(object sender, EventArgs e)
        {
            if (!File.Exists("C:\\SCT\\RibbonDisabler.exe"))
            {
                File.WriteAllBytes("C:\\SCT\\RibbonDisabler.exe", Properties.Resources.ribbonDisabler);
            }
            Process.Start("C:\\SCT\\RibbonDisabler.exe");
        }

        //Make borders 3D by changing UPM
        private void Button13_Click(object sender, EventArgs e)
        {
            File.WriteAllText("C:\\SCT\\reg_upm_enable3d.reg", Properties.Resources.reg_upm_enable3d);
            Process.Start("C:\\SCT\\reg_upm_enable3d.reg").WaitForExit();
            File.Delete("C:\\SCT\\reg_upm_enable3d.reg");
        }

        //Restore WindowMetrics
        private void Button14_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This restores all default theme settings and restart you PC.\nContinue?", "SCT Uninstallation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ClassicTheme.RemoveSCT();
            }
        }

        //Update Delay
        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Configuration.TaskbarDelay = (int)numericUpDown1.Value;
        }

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

        private void button6_Click(object sender, EventArgs e)
        {
            new UtilityManagerForm().ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new AHKScriptManager().ShowDialog();
        }

		private void button8_Click(object sender, EventArgs e)
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
	}
}
