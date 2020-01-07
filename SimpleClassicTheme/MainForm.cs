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
            bool sibInstalled = Environment.OSVersion.Version.Major < 10 || Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StartIsBack\\"));
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
            numericUpDown1.Maximum = Int32.MaxValue;
            ExtraFunctions.UpdateStartupExecutable(false);
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
            checkBox1.Checked = bool.Parse(Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "EnableTaskbar", false.ToString()).ToString());
            File.WriteAllText(Path.Combine(Path.GetTempPath(), "\\addSchemes.bat"), Properties.Resources.addSchemes);
            Process.Start(new ProcessStartInfo() { FileName = Path.Combine(Path.GetTempPath(), "\\addSchemes.bat"), Verb = "runas", UseShellExecute = false, CreateNoWindow = true });
            Shown += Form1_Shown;
            var lol = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme").GetValue("TaskbarDelay", 5000);
            numericUpDown1.Value = (int)lol;
            CheckDependenciesAndSetControls(); 
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
                    File.WriteAllText(Path.Combine(Path.GetTempPath(), "\\ossettings.reg"), Properties.Resources.openShellSettings);
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
                    string f = Properties.Resources.startIsBackSettings.Replace("C:\\\\Users\\\\{Username}", $"{path.Replace("\\", "\\\\")}");
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
            File.WriteAllBytes(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\deskn.cpl", Properties.Resources.desktopControlPanelCPL);
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

        //Install Classic Task Manager
        private void Button7_Click(object sender, EventArgs e)
        {
            File.WriteAllBytes("C:\\SCT\\ctm.exe", Properties.Resources.classicTaskManager);
            Process.Start("C:\\SCT\\ctm.exe", "/silent").WaitForExit();
            MessageBox.Show("Classic Task Manager has been installed on your system");
            File.Delete("C:\\SCT\\ctm.exe");
        }

        //Install 7+ Taskbar Tweaker
        private void Button8_Click(object sender, EventArgs e)
        {
            using (WebClient c = new WebClient())
            {
                c.DownloadFile("https://rammichael.com/downloads/7tt_setup.exe", "C:\\SCT\\7tt.exe");
            }
            Process.Start("C:\\SCT\\7tt.exe", "/S").WaitForExit();
            MessageBox.Show("7+ Taskbar Tweaker has been installed on your system");
            File.Delete("C:\\SCT\\7tt.exe");
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
                numericUpDown1.Enabled = true;
            }
            numericUpDown1.Enabled = checkBox1.Checked;
            if (Directory.Exists("C:\\SCT\\T-Clock\\"))
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

        //Install Folder Options X
        private void Button11_Click(object sender, EventArgs e)
        {
            File.WriteAllBytes("C:\\SCT\\fox.exe", Properties.Resources.folderOptionsX);
            Process.Start("C:\\SCT\\fox.exe", "/silent").WaitForExit();
            MessageBox.Show("Folder Options X has been installed on your system");
            File.Delete("C:\\SCT\\fox.exe");
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
            File.WriteAllText("C:\\SCT\\upm.reg", Properties.Resources.upmReg);
            Process.Start("C:\\SCT\\upm.reg").WaitForExit();
            File.Delete("C:\\SCT\\upm.reg");
        }

        //Restore WindowMetrics
        private void Button14_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This restores the default WindowMetrics for Windows 10. Restore guide:\r\n1. Set Classic Theme to \"Windows Aero\"\r\n2. Disable Classic Theme\r\n3. Use this option\r\nWould you like to continue?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.WriteAllText("C:\\SCT\\restoreMetrics.reg", Properties.Resources.restoreWindowMetrics);
                Process.Start("C:\\SCT\\restoreMetrics.reg").WaitForExit();
                File.Delete("C:\\SCT\\restoreMetrics.reg");
            }
        }

        //Update Delay
        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "TaskbarDelay", (int)numericUpDown1.Value, RegistryValueKind.DWord);
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
    }
}
