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
using Microsoft.Win32;
using System.Windows.Forms.VisualStyles;
using SimpleClassicTheme.Theming;
using System.Reflection;

namespace SimpleClassicTheme
{
    static class SCT
    {
        static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public static Configuration Configuration { get; } = new Configuration();
        public static ResourceFetcher ResourceFetcher { get; } = new ResourceFetcher();
        public static string VersionString => $"Version {Version:3} revision {Version.Revision}";

		public static bool ShouldLoadGUI { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.VisualStyleState = VisualStyleState.NoneEnabled;
            Application.SetCompatibleTextRenderingDefault(false);

            //new ThemeConfigurationForm().ShowDialog(); 
            //Environment.Exit(0);

            //new Forms.Unfinished.DialogTest().ShowDialog(); return;

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

            // Check if SCT is running on a compatible version of .NET (4.8 or higher).
            int netReleaseVersion = (int)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\", "Release", 0);
            if (netReleaseVersion < 528040)
            {
                MessageBox.Show("SCT requires .NET Framework version 4.8 or higher.", "Simple Classic Theme");
                return;
            }

            Forms.LoadForm loader = new Forms.LoadForm();
            loader.Show();
            ShouldLoadGUI = loader.LoadSCT(args);
            loader.Close();

            while (ShouldLoadGUI)
            {
                ShouldLoadGUI = false;
                Application.VisualStyleState = SCT.Configuration.Enabled ? VisualStyleState.NoneEnabled : VisualStyleState.ClientAndNonClientAreasEnabled;
                Application.Run(new MainForm());
            }

            Environment.Exit(0);
        }
    }
}
