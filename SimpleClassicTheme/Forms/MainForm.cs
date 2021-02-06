/*
 *  SimpleClassicTheme, a basic utility to bring back classic theme to newer version of the Windows operating system.
 *  Copyright (C) 2020 Anis Errais
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

namespace SimpleClassicTheme
{
    public partial class MainForm : Form
    {
        //Check installation of Open Shell and StartIsBack++
        public static bool CheckDependencies(bool Taskbar)
        {
            if (Taskbar && (string)Configuration.GetItem("TaskbarType", "SiB+OS") == "SCTT" && File.Exists("C:\\SCT\\Taskbar\\SCT_Taskbar.exe"))
                return true;
            bool osInstalled = Directory.Exists("C:\\Program Files\\Open-Shell\\");
            bool sibInstalled = Environment.OSVersion.Version.Major < 10 || Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StartIsBack\\"));
            if ((!osInstalled || !sibInstalled) && Taskbar)
            {
                return false;
            }
            return true;
        }

        //Main code: Construct and initialize all controls
        public MainForm()
        {
            InitializeComponent();
        }

        //Main code: loads the UI
        private void Form1_Load(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = Int32.MaxValue;
            ExtraFunctions.UpdateStartupExecutable(false);
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme");
            checkBox1.Checked = bool.Parse(Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\1337ftw\SimpleClassicTheme", "EnableTaskbar", false.ToString()).ToString());
            File.WriteAllText(Path.Combine(Path.GetTempPath(), "\\addSchemes.bat"), Properties.Resources.reg_classicschemes_add);
            Process.Start(new ProcessStartInfo() { FileName = Path.Combine(Path.GetTempPath(), "\\addSchemes.bat"), Verb = "runas", UseShellExecute = false, CreateNoWindow = true });
            Shown += Form1_Shown;
            var lol = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme").GetValue("TaskbarDelay", 5000);
            numericUpDown1.Value = (int)lol;
            CheckDependenciesAndSetControls();

            MainMenu menu = new MainMenu() 
            { 
                MenuItems = 
                {
                    new MenuItem("File")
                    {
                        MenuItems =
                        {
                            new MenuItem("Options", optionsToolStripMenuItem_Click),
                            new MenuItem("Exit", exitToolStripMenuItem_Click)
                        }
                    },

                    new MenuItem("Help")
                    {
                        MenuItems =
                        {
                            new MenuItem("Guide", guideToolStripMenuItem_Click),
                            new MenuItem("Report bugs", reportBugsToolStripMenuItem_Click),
                            new MenuItem("About", aboutToolStripMenuItem_Click)
                        }
                    }
                } 
            };

            //Menu = menu;
        }

        //Make the window itself use Classic Theme regardless of the current theme
        private void Form1_Shown(object sender, EventArgs e)
        {
            UxTheme.SetWindowTheme(Handle, " ", " ");
        }

        //Enable
        private void Button1_Click(object sender, EventArgs e)
        {
            ClassicTheme.MasterEnable(checkBox1.Checked);
        }

        //Disable
        private void Button2_Click(object sender, EventArgs e)
        {
            ClassicTheme.MasterDisable(checkBox1.Checked);
        }

        //Install dependencies
        private void Button3_Click(object sender, EventArgs e)
        {
            //Win 8.1 doesn't require anything
            if (Environment.OSVersion.Version.Major < 10)
                return;

            //Check what's installed
            bool osInstalled = Directory.Exists("C:\\Program Files\\Open-Shell\\");
            bool sibInstalled = Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StartIsBack\\"));

            if (button3.Text == "Reconfigure SCT+OS")
            {
                ExtraFunctions.ReConfigureOS(osInstalled, sibInstalled);
                return;
            }

            //Open-Shell installation
            if (!osInstalled && checkBox1.Checked)
            {
                //Open-Shell installation
                if (MessageBox.Show("To continue Open-Shell is required. Would you like Simple Classic Theme to install this for you?", "SimpleClassicTheme", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //Install Open-Shell
                    File.WriteAllBytes("C:\\SCT\\openShellSetup.exe", Properties.Resources.openShellSetup);
                    Process.Start("C:\\SCT\\openShellSetup.exe", "/qn").WaitForExit();

                    //Get user folder
                    string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                    if (Environment.OSVersion.Version.Major >= 6)
                        path = Directory.GetParent(path).ToString();

                    //Prepare files for Open-Shell
                    Directory.CreateDirectory(path + "\\AppData\\Local\\StartIsBack\\Orbs");
                    Properties.Resources.win7.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\win7.png");
                    Properties.Resources.win9x.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\win9x.png");

                    //Find out what start orb the user wants
                    string orbname = MessageBox.Show("Do you want to use a Win95 style start orb (If not a Windows 7 style orb will be used)?", "Simple Classic Theme", MessageBoxButtons.YesNo) == DialogResult.Yes ? "win9x.png" : "win7.png";

                    //Setup Open-Shell registry
                    File.WriteAllText(Path.Combine(Path.GetTempPath(), "\\ossettings.reg"), Properties.Resources.reg_os_settings);
                    Process.Start(Path.Combine(Path.GetTempPath(), "\\ossettings.reg")).WaitForExit();
                    Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\OpenShell\\StartMenu\\Settings", "StartButtonPath", @"%USERPROFILE%\AppData\Local\StartIsBack\Orbs\" + orbname);
                }
                else
                {
                    MessageBox.Show("Using taskbar without OpenShell is not possible!", "Simple Classic Theme");
                    checkBox1.Checked = false;
                }
            }

            //StartIsBack installation
            if (!sibInstalled && checkBox1.Checked)
            {
                bool b = MessageBox.Show("To continue StartIsBack++ is required. Would you like Simple Classic Theme to install this for you?", "Simple Classic Theme", MessageBoxButtons.YesNo) == DialogResult.Yes;
                if (b)
                {
                    //Get user folder
                    string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                    if (Environment.OSVersion.Version.Major >= 6)
                        path = Directory.GetParent(path).ToString();
                    
                    //Prepare files for StartIsBack
                    Directory.CreateDirectory(path + "\\AppData\\Local\\StartIsBack\\Orbs");
                    Directory.CreateDirectory(path + "\\AppData\\Local\\StartIsBack\\Styles");
                    Properties.Resources.win7.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\win7.png");
                    Properties.Resources.win9x.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\win9x.png");
                    Properties.Resources.taskbar.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\taskbar.png");
                    Properties.Resources.null_classic3small.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\null_classic3big.bmp");
                    File.WriteAllBytes(path + "\\AppData\\Local\\StartIsBack\\Styles\\Classic3.msstyles", Properties.Resources.classicStartIsBackTheme);
                    
                    //Setup StartIsBack registry
                    string f = Properties.Resources.reg_sib_settings.Replace("C:\\\\Users\\\\{Username}", $"{path.Replace("\\", "\\\\")}");
                    File.WriteAllText("C:\\sib.reg", f);
                    Process.Start(Path.Combine(Path.GetTempPath(), "\\sib.reg")).WaitForExit();

                    //Disable StartIsBack
                    Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\StartIsBack", "Disabled", 1);
                    
                    using (WebClient c = new WebClient()) 
                        c.DownloadFile("https://s3.amazonaws.com/startisback/StartIsBackPlusPlus_setup.exe", Path.Combine(Path.GetTempPath(), "\\sib.exe"));
                    
                    //Install StartIsBack++
                    Process p = new Process() { StartInfo = { FileName = Path.Combine(Path.GetTempPath(), "\\sib.exe"), Arguments = "/silent" } };
                    p.Start();
                    p.WaitForExit();
                    p.Dispose();
                    File.Delete("C:\\ossettings.reg");
                    File.Delete("C:\\sib.reg");
                    File.Delete("C:\\sib.exe");
                }
                else
                {
                    MessageBox.Show("Using taskbar without StartIsBack++ is not possible!", "Simple Classic Theme");
                    checkBox1.Checked = false;
                }
            }
            //Make sure EVERYTHING is disabled before continuing
            Button2_Click(sender, e);

            //Inform the user and update controls
            MessageBox.Show("All requirements are installed! Enjoy!!!!");
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
            if (MessageBox.Show("This launches the program in auto mode every time the pc starts up (after userinit.exe is loaded). Continue?", "Auto-launch Simple Clasic Theme", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ExtraFunctions.UpdateStartupExecutable(true);
            }
        }

        //Install ExplorerContextMenuTweaker
        private void Button9_Click(object sender, EventArgs e)
        {
            button9.Enabled = false;
            File.WriteAllBytes("C:\\Windows\\System32\\ExplorerContextMenuTweaker.dll", Properties.Resources.ExplorerContextMenuTweaker);
            File.WriteAllBytes("C:\\Windows\\System32\\ShellPayload.dll", Properties.Resources.ShellPayload);
            Process.Start(new ProcessStartInfo() { FileName = "C:\\Windows\\System32\\regsvr32.exe", Arguments = "ExplorerContextMenuTweaker.dll", Verb = "runas" }).WaitForExit();
        }

        //Turn on/off taskbar
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\1337ftw\SimpleClassicTheme", "EnableTaskbar", checkBox1.Checked.ToString());
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
            numericUpDown1.Enabled = checkBox1.Checked;
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
            string TaskbarType = (string)Configuration.GetItem("TaskbarType", "SiB+OS");

            button9.Enabled = OSVersion.Major == 10 && Architecture == 64;

            if (!checkBox1.Checked)
                return;

            //button8.Enabled = TaskbarType == "SiB+OS";
            button10.Enabled = TaskbarType == "SiB+OS";

        }

        //Enables all controls
        private void EnableAllControls()
        {
            foreach (Control c in Controls)
                if (!(c is GroupBox))
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
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\1337ftw\SimpleClassicTheme", "TaskbarDelay", (int)numericUpDown1.Value, RegistryValueKind.DWord);
        }

        //Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Open guide
        private void guideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://docs.google.com/document/d/1wsu2tkdB2TIR1fy7lp2HuQ9JeuVG713syzz9RGl2cV0/");
        }

        //Open github issues page
        private void reportBugsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/AEAEAEAE4343/SimpleClassicTheme/issues");
        }

        //Show about dialog
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog(this);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/AEAEAEAE4343/SimpleClassicTheme/issues");
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
            //Restore WindowMetrics
            File.WriteAllText("C:\\SCT\\reg_windowmetrics_restore.reg", Environment.OSVersion.Version.Major == 10 ? Properties.Resources.reg_windowmetrics_restore : Properties.Resources.reg_windowmetrics_81);
            Process.Start("C:\\SCT\\reg_windowmetrics_restore.reg").WaitForExit();

            //Restore Aero


        }
	}
}
