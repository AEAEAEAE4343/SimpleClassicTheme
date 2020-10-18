using System;
using System.Windows.Forms;
using System.Security.Principal;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Reflection;

namespace SimpleClassicTheme
{
    static class ApplicationEntryPoint
    {
        static void ShowError(string f)
        {
            ConsoleColor ccForeColor = Console.ForegroundColor;
            ConsoleColor ccBackColor = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("ERROR: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(f);

            Console.ForegroundColor = ccForeColor;
            Console.BackgroundColor = ccBackColor;
        }

        static void ShowHelp()
        {
            Console.Write(Properties.Resources.helpMessage);
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (IntPtr.Size != 8)
            {
                ShowError("This binary is incorrectly compiled and cannot run. Please compile SCT as an x64 binary");
                return;
            }

            Application.SetCompatibleTextRenderingDefault(false);

            bool windows = Environment.OSVersion.Platform == PlatformID.Win32NT;
            bool windows10 = Environment.OSVersion.Version.Major == 10 && Int32.Parse(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").ToString()) >= 1803;
            bool windows8 = Environment.OSVersion.Version.Major == 6 && (Environment.OSVersion.Version.Minor == 2 || Environment.OSVersion.Version.Minor == 3);

            //Check if the OS is compatible
            if (!(windows && (windows10 || windows8)))
            {
                Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);
                ShowError("");
#if DEBUG
#else
				Kernel32.FreeConsole();
                return;
#endif
            }

            //If for some odd reason the application hasn't started with administrative privileges, restart with them
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);
            if (!hasAdministrativeRight)
            {
                if (MessageBox.Show("This application requires admin privilages.\nClick Ok to elevate or Cancel to exit.", "Simple Classic Theme - Elevation required", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo
                    {
                        Verb = "runas",
                        FileName = Application.ExecutablePath,
                        Arguments = string.Join(" ", args)
                    };
                    Process.Start(processInfo);
                }
                return;
            }

            Configuration.MigrateOldSCTRegistry();

            Directory.CreateDirectory("C:\\SCT\\");

            //Write loading scripts
            if (!File.Exists("C:\\SCT\\EnableThemeScript.bat"))
                File.WriteAllText("C:\\SCT\\EnableThemeScript.bat", Properties.Resources.EnableThemeScript.Replace("{ver}", Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            if (!File.Exists("C:\\SCT\\DisableThemeScript.bat"))
                File.WriteAllText("C:\\SCT\\DisableThemeScript.bat", Properties.Resources.DisableThemeScript.Replace("{ver}", Assembly.GetExecutingAssembly().GetName().Version.ToString()));

            //Start update checking
            string updateMode = (string)Configuration.GetItem("UpdateMode", "Automatic");
            if (updateMode == "Automatic" || updateMode == "Ask on startup")
            ExtraFunctions.Update();
        
            //Get a console window
            Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);
            Console.WriteLine("SCT Version {0}\nCopyright 2020 Anis Errais", Assembly.GetExecutingAssembly().GetName().Version);
            Thread.Sleep(250);

            //Clean up any files that might have been left over on the root of the C: drive
            Console.WriteLine("Cleaning up...");
            File.Delete("C:\\upm.reg");
            File.Delete("C:\\restoreMetrics.reg");
            File.Delete("C:\\fox.exe");
            File.Delete("C:\\7tt.exe");
            File.Delete("C:\\ctm.exe");
            File.Delete("C:\\ossettings.reg");
            File.Delete("C:\\sib.reg");
            File.Delete("C:\\sib.exe");
            File.Delete("C:\\windowmetrics.reg");
            File.Delete("C:\\RibbonDisabler.exe");

            if (args.Length > 0)
            {
                bool withTaskbar = false;
                for (int i = 1; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "--enable-taskbar":
                        case "-t":
                            withTaskbar = true;
                            break;
                        case "--help":
                        case "-h":
                        case "/help":
                        case "/?":
                            ShowHelp();
                            break;
                        default:
                            break;
                    }
                }
                string arg = args[0];
                doArg:
                switch (arg)
                {
                    case "/enable":
                        if (!MainForm.CheckDependencies(withTaskbar))
                        {
                            ShowError("Not all dependencies are installed. Please run the GUI and install the dependencies.");
                        }
                        Console.Write($"INFO: Enabling classic theme{(withTaskbar ? " and taskbar" : "")}...");
                        ClassicTheme.MasterEnable(withTaskbar); Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("SUCCES");
                        Console.ResetColor();
                        break;
                    case "/disable":
                        if (!MainForm.CheckDependencies(withTaskbar))
                        {
                            ShowError("Not all dependencies are installed. Please run the GUI and install the dependencies.");
                        }
                        Console.Write($"INFO: Disabling classic theme{(withTaskbar ? " and taskbar" : "")}...");
                        ClassicTheme.MasterDisable(withTaskbar); Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("SUCCES");
                        Console.ResetColor();
                        break;
                    case "/configure":
                        Directory.CreateDirectory("C:\\SCT\\");
                        File.WriteAllBytes("C:\\SCT\\deskn.cpl", Properties.Resources.desktopControlPanelCPL);
                        Process.Start("C:\\SCT\\deskn.cpl");
                        break;
                    case "/boot":
                        bool Enabled = bool.Parse(Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme").GetValue("Enabled", "False").ToString());
                        if (Enabled)
                        {
                            arg = "/enable";
                            goto doArg;
                        }
                        break;
                    case "/enableauto":
                        Enabled = bool.Parse(Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme").GetValue("Enabled", "False").ToString());
                        if (Enabled)
                            arg = "/disable";
                        else
                            arg = "/enable";
                        goto doArg;
                    case "--help":
                    case "-h":
                    case "/help":
                    case "/?":
                        ShowHelp();
                        break;
                    default:
                        break;
                }
                return;
            }
            else
            {
                Kernel32.FreeConsole();
                Application.Run(new MainForm());
            }
        }
    }
}
