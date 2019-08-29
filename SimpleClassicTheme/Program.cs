using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

namespace SimpleClassicTheme
{
    static class Program
    {
        static void ShowHelp()
        {
            Console.Write(@"USAGE: SimpleClassicTheme.exe [OPERATION] {ARGS..}

Operations:
    /enable This enables the classic theme
    /disable This disables the classic theme

Arguments:
    -t, --enable-taskbar Enables/Disables classic taskbar (Depending on operation)");
        }
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeConsole();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);
            if (!hasAdministrativeRight)
            {
                if ((Assembly.GetExecutingAssembly().Location == @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\Simple Classic Theme.exe") || MessageBox.Show("This application requires admin privilages.\nClick Ok to elevate or Cancel to exit.", "Elevate?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
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
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("ERROR: ");
                            Console.ResetColor();
                            Console.WriteLine("Not all dependencies are installed. Please run the GUI and install the dependencies.");
                        }
                        if (withTaskbar)
                        { Console.Write("INFO: Enabling classic taskbar..."); MainForm.EnableTaskbar(); Console.WriteLine(); }
                        Console.Write("INFO: Enabling classic theme...");
                        MainForm.Enable(); Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("SUCCES");
                        Console.ResetColor();
                        break;
                    case "/disable":
                        if (!MainForm.CheckDependencies(withTaskbar))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("ERROR: ");
                            Console.ResetColor();
                            Console.WriteLine("Not all dependencies are installed. Please run the GUI and install the dependencies.");
                        }
                        Console.Write("INFO: Disabling classic theme...");
                        MainForm.Disable(); Console.WriteLine();

                        if (withTaskbar) { Console.Write("INFO: Disabling classic taskbar..."); MainForm.DisableTaskbar(); Console.WriteLine(); }
                        Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("SUCCES");
                        Console.ResetColor();
                        break;
                    case "/configure":
                        if (((int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware", 0) == 0) && MessageBox.Show("Windows Defender is enabled. The control panel item will be blocked.\r\nWanna disable Defender and restart??? Not like it's defending you anyways", "Fail", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware", 0);
                            Process.Start(@"C:\Windows\System32\shutdown.exe", "-r -t 00");
                            return;
                        }
                        else
                        {
                            File.WriteAllBytes(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\deskn.cpl", MainForm.StringToByteArray(MainForm.Base64Decode(MainForm.deskn)));
                            Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\deskn.cpl");
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
            if (Assembly.GetExecutingAssembly().Location == @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\Simple Classic Theme.exe")
            {
                Registry.CurrentUser.OpenSubKey("SOFTWARE").CreateSubKey("SimpleClassicTheme");
                bool withTaskbar = bool.Parse((string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "EnableTaskbar", false));
                if (!MainForm.CheckDependencies(withTaskbar))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("ERROR: ");
                    Console.ResetColor();
                    Console.WriteLine("Not all dependencies are installed. Please run the GUI and install the dependencies.");
                }
                if (withTaskbar)
                { Console.Write("INFO: Enabling classic taskbar..."); MainForm.EnableTaskbar(); Console.WriteLine(); }
                Console.Write("INFO: Enabling classic theme...");
                MainForm.Enable(); Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("SUCCES");
                Console.ResetColor();
            }
            else
            {
                FreeConsole();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
