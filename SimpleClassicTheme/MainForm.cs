using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using System.Net;
using System.IO.Compression;

namespace SimpleClassicTheme
{
    public partial class MainForm : Form
    {
        //Check installation of Open Shell and StartIsBack++
        public static bool CheckDependencies(bool Taskbar)
        {
            bool osInstalled = Directory.Exists("C:\\Program Files\\Open-Shell\\");
            bool sibInstalled = Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StartIsBack\\"));
            if ((!osInstalled || !sibInstalled) && Taskbar)
            {
                return false;
            }
            return true;
        }

        int utilMenu = 0;

        //Main code: Construct and initialize all controls
        public MainForm()
        {
            InitializeComponent();
        }

        //Main code: loads the UI
        private void Form1_Load(object sender, EventArgs e)
        {
            ExtraFunctions.UpdateStartupExecutable(false);
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
            checkBox1.Checked = bool.Parse(Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "EnableTaskbar", false.ToString()).ToString());
            File.WriteAllText(Path.Combine(Path.GetTempPath(), "\\addSchemes.bat"), Properties.Resources.addSchemes);
            Process.Start(new ProcessStartInfo() { FileName = Path.Combine(Path.GetTempPath(), "\\addSchemes.bat"), Verb = "runas", UseShellExecute = false, CreateNoWindow = true });
            Shown += Form1_Shown;
            maskedTextBox1.Text = ((int)Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme").GetValue("TaskbarDelay", 5000)).ToString();
            CheckDependenciesAndSetControls(); 
        }

        //Make the window itself use Classic Theme regardless of the current theme
        private void Form1_Shown(object sender, EventArgs e)
        {
            ExtraFunctions.SetWindowTheme(Handle, " ", " ");
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

            bool osInstalled = Directory.Exists("C:\\Program Files\\Open-Shell\\");
            bool sibInstalled = Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StartIsBack\\"));
            if (!osInstalled && checkBox1.Checked)
            {
                MessageBox.Show("To continue installing dependencies you have to manually install Open-Shell\r\nThe page with the latest release will be opened.\r\nPlease download and install the file \"Open-Shell_X_X_X.exe\".\r\nAfter that reopen this program to continue", "Open-Shell");
                Process.Start("https://github.com/Open-Shell/Open-Shell-Menu/releases/latest");
                Environment.Exit(0);
            }
            if (!sibInstalled && checkBox1.Checked)
            {
                bool b = MessageBox.Show("To continue StartIsBack++ is required. Would you like Simple Classic Theme to install this for you?", "Open-Shell", MessageBoxButtons.YesNo) == DialogResult.Yes;
                if (b)
                {
                    string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                    if (Environment.OSVersion.Version.Major >= 6)
                        path = Directory.GetParent(path).ToString();
                    string orbname;
                    if (MessageBox.Show("YES: Windows 9x classic start orb\r\nNO: Windows 7 classic start orb", "Simple Classic Theme", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        orbname = "win9x.png";
                    else
                        orbname = "win7.png";
                    Directory.CreateDirectory(path + "\\AppData\\Local\\StartIsBack\\Orbs");
                    Directory.CreateDirectory(path + "\\AppData\\Local\\StartIsBack\\Styles");
                    Properties.Resources.win7.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\win7.png");
                    Properties.Resources.win9x.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\win9x.png");
                    Properties.Resources.taskbar.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\taskbar.png");
                    Properties.Resources.null_classic3small.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\null_classic3big.bmp");
                    File.WriteAllBytes(path + "\\AppData\\Local\\StartIsBack\\Styles\\Classic3.msstyles", Properties.Resources.Classic3);
                    File.WriteAllText(Path.Combine(Path.GetTempPath(), "\\ossettings.reg"), Properties.Resources.ossettings);
                    Process.Start(Path.Combine(Path.GetTempPath(), "\\ossettings.reg")).WaitForExit();
                    Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\OpenShell\\StartMenu\\Settings", "StartButtonPath", @"%USERPROFILE%\AppData\Local\StartIsBack\Orbs\" + orbname);
                    string f = Properties.Resources.sibsettings.Replace("C:\\\\Users\\\\{Username}", $"{path.Replace("\\", "\\\\")}");
                    File.WriteAllText(Path.Combine(Path.GetTempPath(), "\\sib.reg"), f);
                    Process.Start(Path.Combine(Path.GetTempPath(), "\\sib.reg")).WaitForExit();
                    Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\StartIsBack", "Disabled", 1);
                    using (WebClient c = new WebClient()) c.DownloadFile("https://s3.amazonaws.com/startisback/StartIsBackPlusPlus_setup.exe", Path.Combine(Path.GetTempPath(), "\\sib.exe"));
                    Process p = new Process() { StartInfo = { FileName = Path.Combine(Path.GetTempPath(), "\\sib.exe"), Arguments = "/silent" } };
                    p.Start();
                    p.WaitForExit();
                    p.Dispose();
                }
                else
                {
                    MessageBox.Show("This application cannot continue without StartIsBack++. It will now self-destruct", "Simple Classic Theme");
                    Environment.Exit(0);
                }
            }
            Button2_Click(sender, e);
            MessageBox.Show("All requirements are installed! Enjoy!!!!");
            CheckDependenciesAndSetControls();
        }

        //Open Classic Theme CPL
        private void Button4_Click(object sender, EventArgs e)
        {
            File.WriteAllBytes(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\deskn.cpl", ExtraFunctions.StringToByteArray(ExtraFunctions.Base64Decode(ExtraFunctions.deskn)));
            Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\deskn.cpl");
        }

        //Enable install mode
        private void Button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This launches the program in auto mode every time the pc starts up (after explorer.exe is loaded). Continue?", "Auto-launch Simple Clasic Theme", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ExtraFunctions.UpdateStartupExecutable(true);
            }
        }

        //Switch between menu's
        private void Button6_Click(object sender, EventArgs e)
        {
            utilMenu++;
            if (utilMenu == 3) utilMenu = 0;
            button6.Text = utilMenu == 0 ? "Utilities" : utilMenu == 1 ? "Next" : "Back";
            panel1.Visible = utilMenu == 1;
            panel2.Visible = utilMenu == 2;
        }

        //Install Classic Task Manager
        private void Button7_Click(object sender, EventArgs e)
        {
            File.WriteAllBytes("C:\\ctm.exe", Properties.Resources.ctm);
            Process.Start("C:\\ctm.exe", "/silent");
            MessageBox.Show("Classic Task Manager is being installed on your system");
        }

        //Install 7+ Taskbar Tweaker
        private void Button8_Click(object sender, EventArgs e)
        {
            using (WebClient c = new WebClient())
            {
                c.DownloadFile("https://rammichael.com/downloads/7tt_setup.exe", "\\7tt.exe");
            }
            Process.Start("\\7tt.exe", "/S");
            MessageBox.Show("7+ Taskbar Tweaker is being installed on your system");
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
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "EnableTaskbar", checkBox1.Checked.ToString());
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
                DisableAllControls();
                button3.Enabled = true;
                checkBox1.Enabled = true;
                maskedTextBox1.Enabled = true;
            }
            label1.Visible = checkBox1.Checked;
            maskedTextBox1.Visible = checkBox1.Checked;
            Height = checkBox1.Checked ? 293 : 254;
            if (Directory.Exists("C:\\T-Clock\\"))
                button10.Text = "Open T-Clock";
        }

        //Enables all controls
        private void EnableAllControls()
        {
            foreach (Control c in Controls)
                c.Enabled = true;
        }

        //Disables all controls
        private void DisableAllControls()
        {
            foreach (Control c in Controls)
                c.Enabled = false;
        }

        //Numbers only for taskbar delay
        private void MaskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        //Update delay
        private void MaskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "TaskbarDelay", Int32.Parse(maskedTextBox1.Text));
        }

        //Install T-Clock
        private void Button10_Click(object sender, EventArgs e)
        {
            if (button10.Text == "Install T-Clock")
            {
                using (WebClient c = new WebClient())
                {
                    c.DownloadFile("https://github.com/White-Tiger/T-Clock/releases/download/v2.4.4%23492-rc/T-Clock.zip", "C:\\t-clock.zip");
                }
                Directory.CreateDirectory("C:\\T-Clock\\");
                ZipFile.ExtractToDirectory("C:\\t-clock.zip", "C:\\T-Clock\\");
                MessageBox.Show("T-Clock has been installed on your system");
                button10.Text = "Open T-Clock";
            }
            else
            {
                Process.Start("C:\\T-Clock\\Clock64.exe");
            }
        }

        //Install Folder Options X
        private void Button11_Click(object sender, EventArgs e)
        {
            File.WriteAllBytes("C:\\fox.exe", Properties.Resources.fox);
            Process.Start("C:\\fox.exe", "/silent");
            MessageBox.Show("Folder Options X is being installed on your system");
        }

        //Open RibbonDisabler 4.0
        private void Button12_Click(object sender, EventArgs e)
        {
            if (!File.Exists("C:\\RibbonDisabler.exe"))
            {
                File.WriteAllBytes("C:\\RibbonDisabler.exe", Properties.Resources.RibbonDisabler);
            }
            Process.Start("C:\\RibbonDisabler.exe");
        }

        //Make borders 3D by changing UPM
        private void Button13_Click(object sender, EventArgs e)
        {
            File.WriteAllText("C:\\upm.reg", Properties.Resources.upm);
            Process.Start("C:\\upm.reg").WaitForExit();
        }

        //Restore WindowMetrics
        private void Button14_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This restores the default WindowMetrics for Windows 10. Restore guide:\r\n1. Set Classic Theme to \"Windows Aero\"\r\n2. Disable Classic Theme\r\n3. Use this option\r\nWould you like to continue?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.WriteAllText("C:\\restoreMetrics.reg", Properties.Resources.restoreMetrics);
                Process.Start("C:\\restoreMetrics.reg").WaitForExit();
            }
        }
    }
}
