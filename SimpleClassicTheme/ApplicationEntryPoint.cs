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
		public static bool LoadGUI { get; set; }

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
            Application.EnableVisualStyles();
            Application.VisualStyleState = VisualStyleState.NoneEnabled;
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new Forms.ThemeConfigurationForm()); return;

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

            Configuration.MigrateOldSCTRegistry();
            Application.VisualStyleState = Configuration.Enabled ? VisualStyleState.NoneEnabled : VisualStyleState.ClientAndNonClientAreasEnabled;

            //If it's the first time running SCT, start the wizard.
            if ("NO" == (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\1337ftw\Simple Classic Theme\Base", "EnableTaskbar", "NO") && 
                MessageBox.Show("It seems to be the first time you are running SCT.\nWould you like to run the automated setup tool?", "First run", MessageBoxButtons.YesNo) == DialogResult.Yes)
                SetupWizard.SetupHandler.ShowWizard(SetupWizard.SetupHandler.CreateWizard());

            Directory.CreateDirectory("C:\\SCT\\");

            Forms.LoaderForm loader = new Forms.LoaderForm();
            loader.Show();
            LoadGUI = loader.LoadSCT(args);
            loader.Close();

            while (LoadGUI)
            {
                LoadGUI = false;
                Application.VisualStyleState = Configuration.Enabled ? VisualStyleState.NoneEnabled : VisualStyleState.ClientAndNonClientAreasEnabled;
                Application.Run(new MainForm());
            }
        }
    }
}
