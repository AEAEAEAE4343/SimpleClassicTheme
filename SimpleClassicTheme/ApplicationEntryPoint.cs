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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.Principal;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms.VisualStyles;
using System.Linq;

namespace SimpleClassicTheme
{
    static class ApplicationEntryPoint
    {
        static void ShowHelp()
        {
            Console.WriteLine(Properties.Resources.helpMessage);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            /*if (IntPtr.Size != 8)
            {
                MessageBox.Show("This binary is incorrectly compiled and cannot run. Please compile SCT as an x64 binary");
#if DEBUG
#else
                return;
#endif
            }*/

            Application.EnableVisualStyles();
            Application.VisualStyleState = VisualStyleState.NoneEnabled;
            Application.SetCompatibleTextRenderingDefault(false);

            bool windows = Environment.OSVersion.Platform == PlatformID.Win32NT;
            bool windows10 = Environment.OSVersion.Version.Major == 10 /*&& Int32.Parse(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").ToString()) >= 1803*/;
            bool windows8 = Environment.OSVersion.Version.Major == 6 && (Environment.OSVersion.Version.Minor == 2 || Environment.OSVersion.Version.Minor == 3);

            //Check if the OS is compatible
            if (!(windows && (windows10 || windows8)))
            {
                Kernel32.FreeConsole();
                Kernel32.AllocConsole();
                Console.WriteLine("Incompatible operating system");
                Kernel32.FreeConsole();
#if DEBUG
#else
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
                // Free the console if by any chance we are still attached to one.
                Kernel32.FreeConsole();

                // Allocate a console so we can output information.
                // TODO: Create a custom screen that will replace the console and display the status in a 'Classic' GUI way
                Kernel32.AllocConsole();

                // Show copyright header
                Console.WriteLine("Simple Classic Theme {0}\r\n(C) 2021 LeetFTW\r\n", Assembly.GetExecutingAssembly().GetName().Version);

                // Parse arguments
                List<(string, object[])> arguments = new List<(string, object[])>();
                for (int i = 0; i < args.Length; i++)
				{
                    string parseableArgument;
                    if (args[i].StartsWith("/"))
                        parseableArgument = "--" + args[i].Substring(1);
                    else
                        parseableArgument = args[i];
                    switch (parseableArgument)
                    {
                        case "--boot":
                            if (args.Length > 1)
                                Console.WriteLine("Warning: Any arguments beside --boot will be ignored");
                            arguments.Clear();
                            arguments.Add(("--boot", null));
                            goto execute_arguments;
                        case "--configure":
                        case "-c":
                            arguments.Add(("--configure", null));
                            break;
                        case "--enable":
                        case "-e":
                            arguments.Add(("--enable", null));
                            break;
                        case "--disable":
                        case "-d":
                            arguments.Add(("--disable", null));
                            break;
                        case "--install-dependencies":
                        case "-r":
                            arguments.Add(("--install-dependencies", null));
                            break;
                        case "--install":
                        case "-i":
                            arguments.Add(("--install", null));
                            break;
                        case "--set":
                        case "-s":
                            if (args.Length - 3 >= i)
							{
                                List<string> possibleSettingNames = new[] { "EnableTaskbar", "TaskbarDelay", "UpdateMode", "TaskbarType" }.ToList();
                                string[] possibleSettingTypes = { "Boolean", "Int32", "String", "String" };
                                if (possibleSettingNames.Contains(args[i + 1]))
								{
                                    switch (possibleSettingTypes[possibleSettingNames.IndexOf(args[i + 1])])
									{
                                        case "Boolean":
                                            if (Boolean.TryParse(args[i + 2], out bool boolean))
                                            {
                                                arguments.Add(("--set", new object[]{ args[i + 1], boolean.ToString(), RegistryValueKind.String }));
                                            }
											else
                                            {
                                                Console.WriteLine("Error: Could not parse '{0}' to type System.Boolean", args[i + 2]);
                                                goto exit;
                                            }
                                            break;
                                        case "Int32":
                                            if (Int32.TryParse(args[i + 2], out int int32))
                                            {
                                                arguments.Add(("--set", new object[] { args[i + 1], int32, RegistryValueKind.DWord }));
                                            }
                                            else
                                            {
                                                Console.WriteLine("Error: Could not parse '{0}' to type System.Int32", args[i + 2]);
                                                goto exit;
                                            }
                                            break;
                                        case "String":
                                            arguments.Add(("--set", new object[] { args[i + 1], args[i + 2], RegistryValueKind.String }));
                                            break;
									}
								}
                                else
								{
                                    Console.WriteLine("Error: Invalid setting name '{0}'\r\nPossible values: {1}", args[i + 1]);
                                    goto exit;
                                }
                            }
							else
							{
                                Console.WriteLine("Error: Insufficient information supplied for argument {0}", args[i]);
                                goto exit;
                            }
                            i += 2;
                            break;
                        case "--gui":
                        case "-g":
                            arguments.Add(("--gui", null));
                            if (i < args.Length - 1)
                                Console.WriteLine("Warning: Arguments after {0} will be ingnored\r\nIgnored arguments: {1}", args[i], string.Join(", ", args.Skip(i + 1)));
                            goto execute_arguments;
                        case "--help":
                        case "-h":
                        case "/help":
                        case "/h":
                        case "/?":
                            arguments.Clear();
                            arguments.Add(("--help", null));
                            goto execute_arguments;
                        default:
                            Console.WriteLine("Error: Invalid argument '{0}'", args[i]);
                            goto exit;
                        case "--wizard":
                        case "-w":
                            arguments.Add(("--wizard", null));
                            break;
                    }
				}
            execute_arguments:
                Console.WriteLine("Succesfully parsed {0} argument{1}", arguments.Count, arguments.Count > 1 ? "s" : "");
                bool enableTaskbar = (string)Configuration.GetItem("EnableTaskbar", "False") == "True";
                foreach ((string, object[]) argument in arguments)
                {
                    string selector = argument.Item1;
                run_argument:
                    Console.WriteLine();
                    switch (selector)
					{
                        case "--boot":
                            Console.WriteLine("Simple Classic Theme is restoring Classic Theme settings");
                            bool Enabled = bool.Parse(Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme").GetValue("Enabled", "False").ToString());
                            if (Directory.Exists("C:\\SCT") && Directory.Exists("C:\\SCT\\AHK"))
                                foreach (string f in Directory.EnumerateFiles("C:\\SCT\\AHK"))
                                    Process.Start(f);
                            if (Enabled)
                            {
                                selector = "--enable";
                                goto run_argument;
                            }
                            break;
                        case "--configure":
                            Directory.CreateDirectory("C:\\SCT\\");
                            File.WriteAllBytes("C:\\SCT\\deskn.cpl", Properties.Resources.desktopControlPanelCPL);
                            Process.Start("C:\\SCT\\deskn.cpl");
                            Console.WriteLine("Launched Clasic Theme configuration dialog");
                            break;
                        case "--disable":
                            if (!MainForm.CheckDependencies(enableTaskbar))
                            {
                                Console.WriteLine("Error: Not all dependencies are installed\r\nPlease use the GUI or --install-dependencies to install the dependencies");
                                goto exit;
                            }
                            Console.Write($"Disabling classic theme{(enableTaskbar ? " and taskbar" : "")}...");
                            ClassicTheme.MasterDisable(enableTaskbar); Console.WriteLine();
                            Console.WriteLine("Disabled SCT succesfully");
                            break;
                        case "--enable":
                            if (!MainForm.CheckDependencies(enableTaskbar))
                            {
                                Console.WriteLine("Error: Not all dependencies are installed\r\nPlease use the GUI or --install-dependencies to install the dependencies");
                                goto exit;
                            }
                            Console.Write($"Enabling classic theme{(enableTaskbar ? " and taskbar" : "")}...");
                            ClassicTheme.MasterEnable(enableTaskbar, false, true); Console.WriteLine();
                            Console.WriteLine("Enabled SCT succesfully");
                            break;
                        case "--gui":
                            Console.WriteLine("Starting GUI interface");
                            Kernel32.FreeConsole();
                            goto run_gui;
                        case "--help":
                            ShowHelp();
                            goto exit;
                        case "--install":
                            ExtraFunctions.UpdateStartupExecutable(true);
                            Console.WriteLine("Installed SCT succesfully");
                            break;
                        case "--install-dependencies":
                            if (!enableTaskbar)
							{
                                Console.WriteLine("Warning: Taskbar is not enabled so no dependencies will be installed");
                                break;
                            }
                            switch ((string)Configuration.GetItem("TaskbarType", "None"))
							{
                                case "None":
                                    Console.WriteLine("Error: TaskbarType is not set so no dependencies can be installed. Please set the Taskbar Type in the GUI or with --set");
                                    goto exit;
                                case "SCTT":
                                    if (!ExtraFunctions.IsDotNetRuntimeInstalled())
									{
                                        Console.WriteLine("Error: .NET 5.0 is not installed and is required for SCTT to be installed");
                                        goto exit;
									}
                                    ClassicTaskbar.InstallSCTT(null, false);
                                    Console.WriteLine("Installed SCTT succesfully");
                                    break;
                                case "OS+SiB":
                                    ExtraFunctions.ReConfigureOS(true, true, true);
                                    Console.WriteLine("Configured Open-Shell and StartIsBack++");
                                    int returnCode = InstallableUtility.OpenShell.Install();
                                    if (returnCode != 0)
									{
                                        Console.WriteLine("Error: Open-Shell installer returned error code {0}", returnCode);
                                        goto exit;
									}
                                    Console.WriteLine("Installed Open-Shell succesfully");
                                    returnCode = InstallableUtility.StartIsBackPlusPlus.Install();
                                    if (returnCode != 0)
                                    {
                                        Console.WriteLine("Error: StartIsBack++ installer returned error code {0}", returnCode);
                                        goto exit;
                                    }
                                    Console.WriteLine("Installed StartIsBack++ succesfully");
                                    break;
							}
                            Console.WriteLine("Dependencies installed succesfully");
                            break;
                        case "--set":
                            if ((string)argument.Item2[0] == "EnableTaskbar")
                                enableTaskbar = Boolean.Parse((string)argument.Item2[1]);
                            Configuration.SetItem((string)argument.Item2[0], argument.Item2[1], (RegistryValueKind)argument.Item2[2]);
                            Console.WriteLine("Set configuration item '{0}' to '{1}'", argument.Item2[0], argument.Item2[1]);
                            break;
                        case "--wizard":
                            Console.WriteLine("Running SCT wizard... (Note that this console will stay open)");
                            SetupWizard.SetupHandler.ShowWizard(SetupWizard.SetupHandler.CreateWizard());
                            Console.WriteLine("SCT Wizard finished");
                            break;
                    }
                }
                Kernel32.FreeConsole();
                return;
            exit:
                Console.Write("Press any key to exit..."); Console.ReadKey(); Console.WriteLine();
                Kernel32.FreeConsole();
                return;
            }
        run_gui:
            Application.Run(new MainForm());
        }
    }
}
