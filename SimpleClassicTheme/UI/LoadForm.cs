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

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SimpleClassicTheme.Forms
{
    public partial class LoadForm : Form
    {
        public LoadForm()
        {
            InitializeComponent();
            label2.Text = SCT.VersionString;
        }

        public bool LoadSCT(string[] args)
        {
            Application.VisualStyleState = SCT.Configuration.Enabled ? VisualStyleState.NoneEnabled : VisualStyleState.ClientAndNonClientAreasEnabled;
            Directory.CreateDirectory(SCT.Configuration.InstallPath);

            // Generating classic theme load scripts
            label2.Text = "Status: Generating SCT load scripts";
            Application.DoEvents();

            if (!File.Exists($"{SCT.Configuration.InstallPath}EnableThemeScript.bat"))
                File.WriteAllText($"{SCT.Configuration.InstallPath}EnableThemeScript.bat", SCT.ResourceFetcher.EnableThemeScript.Replace("{ver}", Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            if (!File.Exists($"{SCT.Configuration.InstallPath}DisableThemeScript.bat"))
                File.WriteAllText($"{SCT.Configuration.InstallPath}DisableThemeScript.bat", SCT.ResourceFetcher.DisableThemeScript.Replace("{ver}", Assembly.GetExecutingAssembly().GetName().Version.ToString()));

            // Check for updates
            label2.Text = "Status: Checking for updates";
            Application.DoEvents();

            string updateMode = SCT.Configuration.UpdateMode;
            if (updateMode == "Automatic" || updateMode == "Ask on startup")
                if (ExtraFunctions.Update(this))
                    return false;

            // Clean up any files that might have been left over by old SCT versions
            label2.Text = "Status: Cleaning up files";
            Application.DoEvents();

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

            Application.DoEvents();
            if (args.Length > 0)
            {
                /*// Free the console if by any chance we are still attached to one.
                Kernel32.FreeConsole();

                // Allocate a console so we can output information.
                // TODO: Create a custom screen that will replace the console and display the status in a 'Classic' GUI way
                Kernel32.AllocConsole();

                // Show copyright header
                WriteLine("Simple Classic Theme {0}\r\n(C) 2021 LeetFTW\r\n", Assembly.GetExecutingAssembly().GetName().Version);
                */

                // Parse arguments
                List<(string, object[])> arguments = new List<(string, object[])>();
                for (int i = 0; i < args.Length; i++)
                {
                    label2.Text = $"Status: Parsing arguments ({i}/{args.Length})";
                    Application.DoEvents();

                    string parseableArgument;
                    if (args[i].StartsWith("/"))
                        parseableArgument = "--" + args[i].Substring(1);
                    else
                        parseableArgument = args[i];
                    switch (parseableArgument)
                    {
                        case "--verbose":
                        case "-v":
                            button1_Click(null, null);
                            break;
                        case "--boot":
                            if (args.Length > 1)
                                WriteLine("Warning: Any arguments beside --boot will be ignored");
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
                                                arguments.Add(("--set", new object[] { args[i + 1], boolean.ToString(), RegistryValueKind.String }));
                                            }
                                            else
                                            {
                                                WriteLine($"Error: Could not parse '{args[i + 2]}' to type System.Boolean");
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
                                                WriteLine($"Error: Could not parse '{args[i + 2]}' to type System.Int32");
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
                                    WriteLine($"Error: Invalid setting name '{args[i + 1]}'\r\nPossible values: {1}");
                                    goto exit;
                                }
                            }
                            else
                            {
                                WriteLine($"Error: Insufficient information supplied for argument {args[i]}");
                                goto exit;
                            }
                            i += 2;
                            break;
                        case "--gui":
                        case "-g":
                            arguments.Add(("--gui", null));
                            if (i < args.Length - 1)
                                WriteLine($"Warning: Arguments after {args[i]} will be ingnored\r\nIgnored arguments: {string.Join(", ", args.Skip(i + 1))}");
                            goto execute_arguments;
                        default:
                            WriteLine("Error: Invalid argument '{args[i]}'");
                            goto case "/?";
                        case "--help":
                        case "-h":
                        case "/help":
                        case "/h":
                        case "/?":
                            arguments.Clear();
                            arguments.Add(("--help", null));
                            goto execute_arguments;
                        case "--wizard":
                        case "-w":
                            arguments.Add(("--wizard", null));
                            break;
                    }
                }
            execute_arguments:
                WriteLine($"Succesfully parsed {arguments.Count} argument{(arguments.Count > 1 ? "s" : "")}");
                for (int i = 0; i < arguments.Count; i++)
                {
                    label2.Text = $"Status: Parsing arguments ({i}/{args.Length})";
                    Application.DoEvents();

                    (string, object[]) argument = arguments[i];
                    string selector = argument.Item1;
                run_argument:
                    WriteLine("");
                    switch (selector)
                    {
                        case "--boot":
                            WriteLine("Simple Classic Theme is restoring Classic Theme settings");
                            bool Enabled = SCT.Configuration.Enabled;
                            if (Directory.Exists($"{SCT.Configuration.InstallPath}AHK"))
                                foreach (string f in Directory.EnumerateFiles($"{SCT.Configuration.InstallPath}AHK"))
                                    Process.Start(f);
                            if (Enabled)
                            {
                                selector = "--enable";
                                goto run_argument;
                            }
                            break;
                        case "--configure":
                            File.WriteAllBytes($"{SCT.Configuration.InstallPath}deskn.cpl", SCT.ResourceFetcher.AppearanceCPL);
                            Process.Start($"{SCT.Configuration.InstallPath}deskn.cpl");
                            WriteLine("Launched Clasic Theme configuration dialog");
                            break;
                        case "--disable":
                            if (!MainForm.CheckDependencies(SCT.Configuration.EnableTaskbar))
                            {
                                WriteLine("Error: Not all dependencies are installed\r\nPlease use the GUI or --install-dependencies to install the dependencies");
                                goto exit;
                            }
                            Write("Disabling Simple Classic Theme...");
                            ClassicTheme.MasterDisable(); WriteLine("");
                            WriteLine("Disabled SCT succesfully");
                            break;
                        case "--enable":
                            if (!MainForm.CheckDependencies(SCT.Configuration.EnableTaskbar))
                            {
                                WriteLine("Error: Not all dependencies are installed\r\nPlease use the GUI or --install-dependencies to install the dependencies");
                                goto exit;
                            }
                            Write("Enabling Simple Classic Theme...");
                            ClassicTheme.MasterEnable(); WriteLine("");
                            WriteLine("Enabled SCT succesfully");
                            break;
                        case "--gui":
                            WriteLine("Starting GUI interface"); 
                            goto run_gui;
                        case "--help":
                            new CommandLineHelpForm().ShowDialog(this);
                            goto exit;
                        case "--install":
                            ExtraFunctions.UpdateStartupExecutable(true);
                            WriteLine("Installed SCT succesfully");
                            break;
                        case "--install-dependencies":
                            if (!ExtraFunctions.InstallDependencies())
                                goto exit;
                            break;
                        case "--set":
                            SCT.Configuration.SetItem((string)argument.Item2[0], argument.Item2[1], (RegistryValueKind)argument.Item2[2]);
                            WriteLine($"Set configuration item '{argument.Item2[0]}' to '{argument.Item2[1]}'");
                            break;
                        case "--wizard":
                            WriteLine("The wizard was removed in 1.7.0.6, it will be rebuilt soon.");
                            break;
                    }
                }
                Kernel32.FreeConsole();
                if (ControlBox)
                    goto exit;
                return false;
            exit:
                Kernel32.FreeConsole();
                Application.Run(this);
                return false;
            }

        run_gui:
            return true;
        }

        private void LoaderForm_Load(object sender, EventArgs e)
        {
            label3.Text = label3.Text.Replace("%v", Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
        }
    }
}
