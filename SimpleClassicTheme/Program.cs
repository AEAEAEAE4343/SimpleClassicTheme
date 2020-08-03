using System;
using System.Windows.Forms;
using System.Security.Principal;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace SimpleClassicTheme
{
    static class Program
    {
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
            Application.SetCompatibleTextRenderingDefault(false);

            bool windows = Environment.OSVersion.Platform == PlatformID.Win32NT;
            bool windows10 = Environment.OSVersion.Version.Major == 10 && Int32.Parse(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").ToString()) >= 1803;
            bool windows8 = Environment.OSVersion.Version.Major == 6 && (Environment.OSVersion.Version.Minor == 2 || Environment.OSVersion.Version.Minor == 3);

            //Check if the OS is compatible
            if (!(windows && (windows10 || windows8)))
            {
                Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);
                //If not, display a cool looking error message
                string t = Console.Title;
                Console.Title = "Simple Compatibilty Error";
                int x = Console.BufferWidth;
                int y = Console.BufferHeight;
                int width = Console.WindowWidth;
                int height = Console.WindowHeight;
                Console.SetWindowSize(45, 12);
                Console.BufferWidth = 45;
                Console.BufferHeight = 12;
                Console.SetCursorPosition(0, 0);
                Console.Write(Properties.Resources.compatibiltyError.Replace("\n", ""));
                Console.SetCursorPosition(40, 7);
                Console.ReadKey();
                Console.BufferWidth = x;
                Console.BufferHeight = y;
                Console.SetWindowSize(width, height);
                Console.SetCursorPosition(0, 11);
                Console.Title = t;
				Kernel32.FreeConsole();
				return;
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

            Directory.CreateDirectory("C:\\SCT\\");

            //Start update checking
            ExtraFunctions.Update();
        
            //Get a console window
            Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);
            Console.WriteLine("SCT Version {0}\nCopyright 2020 Anis Errais");

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
                switch (args[0])
                {
                    case "/enable":
                        if (!MainForm.CheckDependencies(withTaskbar))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("ERROR: ");
                            Console.ResetColor();
                            Console.WriteLine("Not all dependencies are installed. Please run the GUI and install the dependencies.");
                        }
                        Console.Write($"INFO: Enabling classic theme{(withTaskbar ? " and taskbar" : "")}...");
                        ClassicTheme.MasterEnable(withTaskbar); Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("SUCCES");
                        Console.ResetColor();
                        break;
                    case "/disable":
                        if (!MainForm.CheckDependencies(withTaskbar))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("ERROR: ");
                            Console.ResetColor();
                            Console.WriteLine("Not all dependencies are installed. Please run the GUI and install the dependencies.");
                        }
                        Console.Write($"INFO: Disabling classic theme{(withTaskbar ? " and taskbar" : "")}...");
                        ClassicTheme.MasterDisable(withTaskbar); Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("SUCCES");
                        Console.ResetColor();
                        break;
                    case "/configure":
                        File.WriteAllBytes(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\deskn.cpl", Properties.Resources.desktopControlPanelCPL);
                        Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\deskn.cpl");
                        break;
                    case "/boot":
                        bool Enabled = bool.Parse(Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme").GetValue("Enabled", "False").ToString());
                        if (Enabled)
                        {
                            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
                            withTaskbar = bool.Parse(Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "EnableTaskbar", false.ToString()).ToString());
                            if (!MainForm.CheckDependencies(withTaskbar))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("ERROR: ");
                                Console.ResetColor();
                                Console.WriteLine("Not all dependencies are installed. Please run the GUI and install the dependencies.");
                            }
                            Console.Write($"INFO: Enabling classic theme{(withTaskbar ? " and taskbar" : "")}...");
                            ClassicTheme.MasterEnable(withTaskbar); Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("SUCCES");
                            Console.ResetColor();
                        }
                        break;
                    case "/enableauto":
                        Enabled = bool.Parse(Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme").GetValue("Enabled", "False").ToString());
                        if (Enabled)
                        {
                            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
                            withTaskbar = bool.Parse(Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "EnableTaskbar", false.ToString()).ToString());
                            if (!MainForm.CheckDependencies(withTaskbar))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("ERROR: ");
                                Console.ResetColor();
                                Console.WriteLine("Not all dependencies are installed. Please run the GUI and install the dependencies.");
                            }
                            Console.Write($"INFO: Enabling classic theme{(withTaskbar ? " and taskbar" : "")}...");
                            ClassicTheme.MasterEnable(withTaskbar); Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("SUCCES");
                            Console.ResetColor();
                        }
                        else
                        {
                            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
                            withTaskbar = bool.Parse(Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "EnableTaskbar", false.ToString()).ToString());
                            if (!MainForm.CheckDependencies(withTaskbar))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("ERROR: ");
                                Console.ResetColor();
                                Console.WriteLine("Not all dependencies are installed. Please run the GUI and install the dependencies.");
                            }
                            Console.Write($"INFO: Disabling classic theme{(withTaskbar ? " and taskbar" : "")}...");
                            ClassicTheme.MasterDisable(withTaskbar); Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("SUCCES");
                            Console.ResetColor();
                        }
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
