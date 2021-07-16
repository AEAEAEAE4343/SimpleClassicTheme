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
using System.Security.Principal;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Windows.Forms.VisualStyles;

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

            Application.EnableVisualStyles();
            Application.VisualStyleState = VisualStyleState.NoneEnabled;
            Application.SetCompatibleTextRenderingDefault(false);

            bool windows = Environment.OSVersion.Platform == PlatformID.Win32NT;
            bool windows10 = Environment.OSVersion.Version.Major == 10 /*&& Int32.Parse(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").ToString()) >= 1803*/;
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

            //If it's the first time running SCT, start the wizard.
            if ("NO" == (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\1337ftw\SimpleClassicTheme", "EnableTaskbar", "NO") && 
                MessageBox.Show("It seems to be the first time you are running SCT.\nWould you like to run the automated setup tool?", "First run", MessageBoxButtons.YesNo) == DialogResult.Yes)
                SetupWizard.SetupHandler.ShowWizard(SetupWizard.SetupHandler.CreateWizard());

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

            //Clean up any files that might have been left over in the SCT directory
            File.Delete("C:\\SCT\\upm.reg");
            File.Delete("C:\\SCT\\restoreMetrics.reg");

            if (args.Length > 0)
			{

			}
            else if (args.Length > 0)
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
                        if (Directory.Exists("C:\\SCT"))
                            foreach (string f in Directory.EnumerateFiles("C:\\SCT\\AHK"))
                                Process.Start(f);
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
                    case "/wizard":
                        SetupWizard.SetupHandler.ShowWizard(SetupWizard.SetupHandler.CreateWizard());
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
                Application.Run(new MainForm());
            }
        }
    }
}
