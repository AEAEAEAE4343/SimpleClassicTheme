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
using System.Drawing;
using System.Reflection;
using SimpleClassicTheme.Forms;
using SimpleClassicTheme.Theming;

namespace SimpleClassicTheme
{
    public partial class MainForm : SystemMenuForm
    {
        public MainForm()
        {
            InitializeComponent();
            label2.Text = SCT.VersionString;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SystemMenu menu = new SystemMenu();
            SystemMenu.CopyToolStripToMenu(menuStrip1, menu);
            Controls.Remove(menuStrip1);
            SystemMenu = menu;

            ClientSize = new Size(panel1.Width, panel1.Height + menu.Height);
            panel1.Location = new Point(0, 0);

            ExtraFunctions.UpdateStartupExecutable(false);

            Directory.CreateDirectory($"{SCT.Configuration.InstallPath}Resources\\");
            File.WriteAllText($"{SCT.Configuration.InstallPath}Resources\\schemes.bat", SCT.ResourceFetcher.ColorSchemesReg);
            Process.Start(new ProcessStartInfo() { FileName = $"{SCT.Configuration.InstallPath}Resources\\schemes.bat", Verb = "runas", UseShellExecute = false, CreateNoWindow = true });

            CheckDependenciesAndSetControls();

            if (ExtraFunctions.ShouldDrawLight(SystemColors.Control))
                pictureBox1.Image = SCT.ResourceFetcher.SCTLogoLight164;
            else
                pictureBox1.Image = SCT.ResourceFetcher.SCTLogoDark164;
        }

		#region Configuration checks

		// Check if all requirements for SCT and, if selected, the classic taskbar are installed
		public static bool CheckDependencies()
        {
            if (SCT.Configuration.ClassicThemeMethod == ClassicTheme.ClassicThemeMethod.MultiUserClassicTheme &&
                !File.Exists(Environment.GetEnvironmentVariable("programfiles") + "\\MCT\\MCTapi.dll"))
                return false;

            switch (SCT.Configuration.TaskbarType)
            {
                case TaskbarType.SimpleClassicThemeTaskbar:
                    return File.Exists($"{SCT.Configuration.InstallPath}Taskbar\\SimpleClassicThemeTaskbar.exe");
                case TaskbarType.RetroBar:
                    return File.Exists($"{SCT.Configuration.InstallPath}RetroBar\\RetroBar.exe");
                case TaskbarType.Windows81Vanilla:
                case TaskbarType.None:
                default:
                    return true;
            }
        }

        // Check dependencies and set control visibilty/usability
        private void CheckDependenciesAndSetControls()
        {
            EnableAllControls();

            bool dependenciesInstalled = CheckDependencies();
            buttonInstallRequirements.Enabled = !dependenciesInstalled;
            buttonEnable.Enabled = dependenciesInstalled;
            buttonDisable.Enabled = dependenciesInstalled;

            // Do a bunch of version/configuration specific checks
            Version OSVersion = Environment.OSVersion.Version;

            // ECMT: Windows 10 x64
            buttonECMT.Enabled = OSVersion.Major == 10 && OSVersion.CompareString("10.0.22000.0") < 0 && IntPtr.Size == 8;
            buttonECMT.Enabled &= !File.Exists("C:\\Windows\\System32\\ExplorerContextMenuTweaker.dll");

            button3DBorders.Text = SCT.Configuration.Borders3D ? "Disable 3D Borders" : "Enable 3D Borders";
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
            if (ClassicTheme.MasterEnable())
            {
                SCT.ShouldLoadGUI = true; Close();
            }
        }

        // Disable
        private void ButtonDisable_Click(object sender, EventArgs e)
        {
            if (ClassicTheme.MasterDisable())
            {
                SCT.ShouldLoadGUI = true; Close();
            }
        }

        // Install dependencies
        private void ButtonInstallRequirements_Click(object sender, EventArgs e)
        {
            ExtraFunctions.InstallDependencies();
            
            //Make sure EVERYTHING is disabled before continuing
            ButtonDisable_Click(sender, e);
            CheckDependenciesAndSetControls();
        }

        // Open Classic Theme CPL
        private void ButtonConfigure_Click(object sender, EventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Shift))
            {
                new ThemeConfigurationForm().ShowDialog();
                return;
            }
            if (!File.Exists($"{SCT.Configuration.InstallPath}Resources\\deskn.cpl"))
            {
                byte[] bytes = SCT.ResourceFetcher.AppearanceCPL;
                if (bytes is null)
                    return;
                Directory.CreateDirectory($"{SCT.Configuration.InstallPath}Resources\\");
                File.WriteAllBytes($"{SCT.Configuration.InstallPath}Resources\\deskn.cpl", bytes);
            }
            Process.Start($"{SCT.Configuration.InstallPath}Resources\\deskn.cpl");
        }

        // Auto-launch SCT on boot
        private void ButtonRunOnBoot_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"This makes SCT automatically launch when you log onto your PC. You can use the boot scripts in {SCT.Configuration.InstallPath} to configure things to load before Classic Theme gets enabled. Continue?", "Run Simple Clasic Theme on boot", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ExtraFunctions.UpdateStartupExecutable(true);
            }
        }

        // Install ExplorerContextMenuTweaker
        private void ButtonECMT_Click(object sender, EventArgs e)
        {
            buttonECMT.Enabled = false;

            if (!File.Exists($"{SCT.Configuration.InstallPath}Resources\\ExplorerContextMenuTweaker.dll"))
            {
                byte[] bytes = SCT.ResourceFetcher.ExplorerContextMenuTweaker;
                if (bytes is null)
                    return;
                Directory.CreateDirectory($"{SCT.Configuration.InstallPath}Resources\\");
                File.WriteAllBytes($"{SCT.Configuration.InstallPath}Resources\\ShellPayloadShellPayload.dll", bytes);
            }
            if (!File.Exists($"{SCT.Configuration.InstallPath}Resources\\ShellPayload.dll"))
            {
                byte[] bytes = SCT.ResourceFetcher.ShellPayload;
                if (bytes is null)
                    return;
                Directory.CreateDirectory($"{SCT.Configuration.InstallPath}Resources\\");
                File.WriteAllBytes($"{SCT.Configuration.InstallPath}Resources\\ShellPayload.dll", bytes);
            }

            Process.Start(new ProcessStartInfo() { FileName = "C:\\Windows\\System32\\regsvr32.exe", Arguments = "ExplorerContextMenuTweaker.dll", Verb = "runas" }).WaitForExit();
        }

        // Make borders 3D by changing UPM
        private void Button3DBorders_Click(object sender, EventArgs e)
        {
            SCT.Configuration.Borders3D = !SCT.Configuration.Borders3D;
            CheckDependenciesAndSetControls();
            MessageBox.Show(this, "Setting changed. You have to log off in order to apply changes.", "Simple Classic Theme");
        }

        // Open RibbonDisabler 4.0
        private void ButtonRibbonDisabler_Click(object sender, EventArgs e)
        {
            if (!File.Exists($"{SCT.Configuration.InstallPath}Resources\\RibbonDisabler.exe"))
            {
                byte[] bytes = SCT.ResourceFetcher.RibbonDisabler;
                if (bytes is null)
                    return;
                Directory.CreateDirectory($"{SCT.Configuration.InstallPath}Resources\\");
                File.WriteAllBytes($"{SCT.Configuration.InstallPath}Resources\\RibbonDisabler.exe", bytes);
            }
            Process.Start($"{SCT.Configuration.InstallPath}Resources\\RibbonDisabler.exe");
        }

        // Run the SCT AHK script manager
        private void ButtonAHKScripts_Click(object sender, EventArgs e)
        {
            new AHKScriptManager().ShowDialog();
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
