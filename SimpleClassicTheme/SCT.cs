/*
 *  Simple Classic Theme, a basic utility to bring back classic theme to 
 *  newer versions of the Windows operating system.
 *  Copyright (C) 2022 Anis Errais
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
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 *
 */

using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Reflection;
using System.Collections.Generic;

namespace SimpleClassicTheme
{
    static class SCT
    {
        static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public static Configuration Configuration { get; } = new Configuration();
        public static ResourceFetcher ResourceFetcher { get; } = new ResourceFetcher();
#if SCTLITE
        public static string VersionString => $"Version {Version.ToString(3)} revision mctlite{Version.Revision}";
#else
        public static string VersionString => $"Version {Version.ToString(3)} revision mct{Version.Revision}";
#endif

        public static bool ShouldLoadGUI { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] argv)
        {
            Application.EnableVisualStyles();
            Application.VisualStyleState = VisualStyleState.NoneEnabled;
            Application.SetCompatibleTextRenderingDefault(false);

            bool windows = Environment.OSVersion.Platform == PlatformID.Win32NT;
            bool windows10or11 = Environment.OSVersion.Version.Major == 10 /*&& Int32.Parse(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").ToString()) >= 1803*/;
            bool windows8 = Environment.OSVersion.Version.Major == 6 && (Environment.OSVersion.Version.Minor == 2 || Environment.OSVersion.Version.Minor == 3);

            // Check if SCT is running on a compatible operating system.
            if (!(windows && (windows10or11 || windows8)))
            {
                MessageBox.Show("SCT is incompatible with this version of Windows.", "Simple Classic Theme");
#if DEBUG
#else
                return;
#endif
            }

            // Parse silencer arguments
            List<string> arguments = new List<string>(argv);
            if (arguments.Contains("--noerr"))
            {
                arguments.RemoveAll((a) => a == "--noerr");
                Logger.UILevel = UILevel.LogWarningsAndErrors;
            }
            if (arguments.Contains("--silent"))
            {
                arguments.RemoveAll((a) => a == "--silent");
                Logger.UILevel = UILevel.Silent;
            }

            Forms.LoadForm loader = new Forms.LoadForm();
            if (Logger.UILevel != UILevel.Silent) loader.Show();

            ShouldLoadGUI = loader.LoadSCT(arguments.ToArray());
            loader.Close();

            while (Logger.UILevel != UILevel.Silent && ShouldLoadGUI)
            {
                ShouldLoadGUI = false;
                Application.VisualStyleState = Configuration.Enabled ? VisualStyleState.NoneEnabled : VisualStyleState.ClientAndNonClientAreasEnabled;
                Application.Run(new MainForm());
            }

            Environment.Exit(0);
        }
    }
}
