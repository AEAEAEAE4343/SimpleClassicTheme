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

using static SimpleClassicTheme.Logger;

namespace SimpleClassicTheme.Forms
{
    public partial class LoadForm : Form
    {
        public LoadForm()
        {
            InitializeComponent();
            label3.Text = SCT.VersionString;
        }

        public void SetStatus(string status)
        {
            label2.Text = $"Status {status}";
            DebugMessage(status);
            Application.DoEvents();
        }

        public bool LoadSCT(string[] args)
        {
            Application.VisualStyleState = SCT.Configuration.Enabled ? VisualStyleState.NoneEnabled : VisualStyleState.ClientAndNonClientAreasEnabled;
            Directory.CreateDirectory(SCT.Configuration.InstallPath);

            // Generating classic theme load scripts
            SetStatus("Generating Classic Theme load scripts...");

            if (!File.Exists($"{SCT.Configuration.InstallPath}EnableThemeScript.bat"))
                File.WriteAllText($"{SCT.Configuration.InstallPath}EnableThemeScript.bat", SCT.ResourceFetcher.EnableThemeScript.Replace("{ver}", Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            if (!File.Exists($"{SCT.Configuration.InstallPath}DisableThemeScript.bat"))
                File.WriteAllText($"{SCT.Configuration.InstallPath}DisableThemeScript.bat", SCT.ResourceFetcher.DisableThemeScript.Replace("{ver}", Assembly.GetExecutingAssembly().GetName().Version.ToString()));

            // Check for updates
            SetStatus("Checking for updates...");

            if (SCT.Configuration.UpdateMode != UpdateMode.Manual && ExtraFunctions.Update(this))
                return false;

            // Clean up any files that might have been left over by old SCT versions
            SetStatus("Cleaning up files..");

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

            if (args.Length == 0)
                return true;

            SetStatus("Parsing command line...");
            switch (args[0])
            {
                case "--boot":
                    DebugMessage("--boot: Running Classic Theme applications...");
                    if (Directory.Exists($"{SCT.Configuration.InstallPath}AHK"))
                        foreach (string f in Directory.EnumerateFiles($"{SCT.Configuration.InstallPath}AHK"))
                            Process.Start(f);
                    if (Enabled)
                        goto case "--enable";
                    break;

                case "--enable":
                case "-e":
                case "/e":
                    DebugMessage("--enable: Enabling Classic Theme...");
                    if (!MainForm.CheckDependencies(SCT.Configuration.EnableTaskbar))
                    {
                        ErrorMessage("Failed to enable Classic Theme", "The required dependencies are not installed. Use the GUI or --install-dependencies to install them.");
                        break;
                    }
                    ClassicTheme.MasterEnable();
                    DebugMessage("Succesfully enabled Classic Theme");
                    break;

                case "--disable":
                case "-d":
                case "/d":
                    DebugMessage("--enable: Disabling Classic Theme...");
                    if (!MainForm.CheckDependencies(SCT.Configuration.EnableTaskbar))
                    {
                        ErrorMessage("Failed to disable Classic Theme", "The required dependencies are not installed. Use the GUI or --install-dependencies to install them.");
                        break;
                    }
                    ClassicTheme.MasterEnable();
                    DebugMessage("Succesfully disabled Classic Theme");
                    break;

                case "--configure":
                case "-c":
                case "/c":
                    DebugMessage("--configure: Launching configuration dialog...");
                    new Theming.ThemeConfigurationForm().ShowDialog(this);
                    DebugMessage("Configuration dialog closed");
                    break;

                case "--help":
                case "/help":
                case "-h":
                case "/h":
                case "/?":
                    DebugMessage("--help: Launching command line help form...");
                    new CommandLineHelpForm().ShowDialog();
                    DebugMessage("Help form closed");
                    break;

                case "--wizard":
                case "-w":
                case "/w":

                case "--set":
                case "-s":
                case "/s":

                case "--install":
                case "-i":
                case "/i":

                case "--install-dependencies":
                case "-r":
                case "/r":
                    DebugMessage($"{args[0]}: Unimplemented, but planned command specified");
                    ErrorMessage("Cannot parse command line", $"The specified command ${args[0]} is a valid command, but is not implemented in this version of SCT.");
                    break;
            }
            return false;
        }

        private void LoaderForm_Load(object sender, EventArgs e)
        {
            label3.Text = label3.Text.Replace("%v", Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
        }
    }
}
