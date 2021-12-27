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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SimpleClassicTheme
{
    /// <summary>
    /// Encapsulates the different taskbars compatible with SCT
    /// </summary>
    public enum TaskbarType
    {
        None = 0,
        StartIsBackOpenShell = 1,
        Windows81Vanilla = 2,
        SimpleClassicThemeTaskbar = 3,
        RetroBar = 4,
        ExplorerPatcher = 5
    }

    /// <summary>
    /// Helper class: Contains all longish functions that make code unreadable
    /// </summary>
    static class ExtraFunctions
    {
        /// <summary>
        /// Check if there's a network connectionX
        /// </summary>
        /// <returns>Whether a valid network connection is present</returns>
        public static bool CheckForInternetConnection()
        {
            try
            {
                HttpWebRequest r = (HttpWebRequest)WebRequest.Create("https://google.com/generate_204");
                using (HttpWebResponse response = (HttpWebResponse)r.GetResponse())
                    if (response.StatusCode == HttpStatusCode.NoContent)
                        return true;
            }
            catch { return false; }
            return false;
        }

        public static bool ShouldDrawLight(Color back)
		{
            double r = (back.R / 100D) <= 0.03928D ? (back.R / 100D) / 12.92D : Math.Pow(((back.R / 100D) + 0.055D) / 1.055D, 2.4D);
            double g = (back.G / 100D) <= 0.03928D ? (back.G / 100D) / 12.92D : Math.Pow(((back.G / 100D) + 0.055D) / 1.055D, 2.4D);
            double b = (back.B / 100D) <= 0.03928D ? (back.B / 100D) / 12.92D : Math.Pow(((back.B / 100D) + 0.055D) / 1.055D, 2.4D);
            double l = 0.2126 * r + 0.7152 * g + 0.0722 * b;

            return l > Math.Sqrt(1.05 * 0.05) - 0.05;
        }

        /// <summary>
        /// Checks if .NET 5.0 or higher is installed
        /// </summary>
        /// <returns></returns>
        public static bool IsDotNetRuntimeInstalled()
		{
            ProcessStartInfo psi = new ProcessStartInfo("C:\\Windows\\System32\\cmd.exe", "/c dotnet --list-runtimes");
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            Process process = Process.Start(psi);
            process.WaitForExit();

            foreach (string line in process.StandardOutput.ReadToEnd().Replace("\r\n", "\n").Split('\n'))
                if (line.StartsWith("Microsoft.WindowsDesktop.App "))
                    if (Version.TryParse(line.Remove(line.IndexOf(" [")).Substring(29), out Version result))
                        if (result.CompareTo(new Version(5, 0, 0)) >= 0)
                            return true;

            return false;
		}

        //Updates the application
        internal static bool Update(Form p = null)
        {
            //Check if there is an internet connection
            if (CheckForInternetConnection())
            {
                Updater d = new Updater();
                if (p != null) d.ShowDialog(p);
                else d.ShowDialog();
                return d.HasUpdated;
            }
            return false;
        }

        //Updates the startup executable
        internal static void UpdateStartupExecutable(bool createIfNot)
        {
            if (File.Exists(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe"))
                File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe");
            if (File.Exists(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\Simple Classic Theme.exe"))
                File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\Simple Classic Theme.exe");
            if (createIfNot)
            {
                if (Assembly.GetExecutingAssembly().Location != @"C:\SCT\SCT.exe")
                {
                    File.Delete(@"C:\SCT\SCT.exe");
                    File.Copy(Assembly.GetExecutingAssembly().Location, @"C:\SCT\SCT.exe");
                }
                File.WriteAllText("C:\\SCT\\SCTTask.xml", Properties.Resources.cmd_create_task);
                File.WriteAllText("C:\\SCT\\TaskSchedule.cmd", Properties.Resources.taskScheduleCommands);
                Process task = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "C:\\SCT\\TaskSchedule.cmd",
                        Verb = "runas",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                task.Start();
                task.WaitForExit();
                File.SetAttributes(@"C:\SCT\SCT.exe", File.GetAttributes(Assembly.GetExecutingAssembly().Location) | FileAttributes.Hidden);
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                Directory.CreateDirectory(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\");
                IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.lnk");
                shortcut.Description = "SCT";
                shortcut.TargetPath = @"C:\SCT\SCT.exe";
                shortcut.Save();
            }
            else if (File.Exists("C:\\SCT\\SCT.exe") && Assembly.GetExecutingAssembly().Location != @"C:\SCT\SCT.exe" && CheckMD5(@"C:\SCT\SCT.exe") != CheckMD5(Assembly.GetExecutingAssembly().Location))
			{
                try
                {
                    File.Delete(@"C:\SCT\SCT.exe");
                    File.Copy(Assembly.GetExecutingAssembly().Location, @"C:\SCT\SCT.exe");
                }
                catch (UnauthorizedAccessException)
				{

				}
            }
        }

        //Renames a subkey
        public static bool RenameSubKey(RegistryKey parentKey,
            string subKeyName, string newSubKeyName)
        {
            CopyKey(parentKey, subKeyName, newSubKeyName);
            parentKey.DeleteSubKeyTree(subKeyName);
            return true;
        }

        //Copies a key
        public static bool CopyKey(RegistryKey parentKey,
            string keyNameToCopy, string newKeyName)
        {
            RegistryKey destinationKey = parentKey.CreateSubKey(newKeyName);
            RegistryKey sourceKey = parentKey.OpenSubKey(keyNameToCopy);
            RecurseCopyKey(sourceKey, destinationKey);
            return true;
        }

        //Recursively copies a key
        public static void RecurseCopyKey(RegistryKey sourceKey, RegistryKey destinationKey)
        {
            foreach (string valueName in sourceKey.GetValueNames())
            {
                object objValue = sourceKey.GetValue(valueName);
                RegistryValueKind valKind = sourceKey.GetValueKind(valueName);
                destinationKey.SetValue(valueName, objValue, valKind);
            }
            foreach (string sourceSubKeyName in sourceKey.GetSubKeyNames())
            {
                RegistryKey sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName);
                RegistryKey destSubKey = destinationKey.CreateSubKey(sourceSubKeyName);
                RecurseCopyKey(sourceSubKey, destSubKey);
            }
        }

        //Gets the MD5 checksum of a file
        public static string CheckMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return Encoding.Default.GetString(md5.ComputeHash(stream));
                }
            }
        }

        public static void ReConfigureOS(bool ossm, bool ostb, bool sib)
		{
            if (ossm)
            {
                File.WriteAllText("C:\\SCT\\ossettings.reg", Properties.Resources.reg_os_sm_settings);
                Process.Start("C:\\Windows\\System32\\reg.exe", "import C:\\SCT\\ossettings.reg").WaitForExit();
                File.Delete("C:\\SCT\\ossettings.reg");
            }
            if (ostb)
            {
                Directory.CreateDirectory("C:\\SCT\\OpenShellAssets");
                Properties.Resources.win7.Save("C:\\SCT\\OpenShellAssets\\win7.png");
                Properties.Resources.win9x.Save("C:\\SCT\\OpenShellAssets\\win9x.png");
                Properties.Resources.taskbar.Save("C:\\SCT\\OpenShellAssets\\taskbar.png");
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\OpenShell\\StartMenu\\Settings", "StartButtonPath", "C:\\SCT\\OpenShellAssets\\win9x.png");
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\OpenShell\\StartMenu\\Settings", "TaskbarTexture", "C:\\SCT\\OpenShellAssets\\taskbar.png");

                File.WriteAllText("C:\\SCT\\ossettings.reg", Properties.Resources.reg_os_tb_settings);
                Process.Start("C:\\Windows\\System32\\reg.exe", "import C:\\SCT\\ossettings.reg").WaitForExit();
                File.Delete("C:\\SCT\\ossettings.reg");
            }
            if (sib)
			{
                string userFolder = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                    userFolder = Directory.GetParent(userFolder).ToString();

                Directory.CreateDirectory(userFolder + "\\AppData\\Local\\StartIsBack\\Orbs");
                Directory.CreateDirectory(userFolder + "\\AppData\\Local\\StartIsBack\\Styles");
                Properties.Resources.null_classic3small.Save(userFolder + "\\AppData\\Local\\StartIsBack\\Orbs\\null_classic3big.bmp");
                File.WriteAllBytes(userFolder + "\\AppData\\Local\\StartIsBack\\Styles\\Classic3.msstyles", Properties.Resources.classicStartIsBackTheme);

                string f = Properties.Resources.reg_sib_settings.Replace("C:\\\\Users\\\\{Username}", $"{userFolder.Replace("\\", "\\\\")}");
                File.WriteAllText("C:\\SCT\\sib.reg", f);
                Process.Start("C:\\Windows\\System32\\reg.exe", "import C:\\SCT\\sib.reg").WaitForExit();

                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\StartIsBack", "Disabled", 1);
                File.Delete("C:\\SCT\\sib.reg");
            }
        }

        public static bool InstallDependencies(bool commandLineOutput = false)
		{
            switch (Configuration.TaskbarType)
            {
                case TaskbarType.RetroBar:
                    GithubDownloader download = new GithubDownloader(GithubDownloader.DownloadableGithubProject.RetroBar);
                    download.ShowDialog();
                    break;
                case TaskbarType.SimpleClassicThemeTaskbar:
                    if (!ExtraFunctions.IsDotNetRuntimeInstalled())
                    {
                        if (commandLineOutput) Console.WriteLine("Error: .NET 5.0 is not installed and is required for SCTT to be installed");
                        else MessageBox.Show(".NET 5.0 is not installed and is required for SCTT to be installed", "Error installing dependencies", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    ClassicTaskbar.InstallSCTT(null, !commandLineOutput);
                    if (commandLineOutput) Console.WriteLine("Installed SCTT succesfully");
                    break;
                case TaskbarType.StartIsBackOpenShell:
                    ExtraFunctions.ReConfigureOS(true, true, true);
                    if (commandLineOutput) Console.WriteLine("Configured Open-Shell and StartIsBack++");
                    int returnCode = InstallableUtility.OpenShell.Install();
                    if (returnCode != 0)
                    {
                        if (commandLineOutput) Console.WriteLine("Error: Open-Shell installer returned error code {0}", returnCode);
                        else MessageBox.Show($"Open-Shell installer returned error code {returnCode}", "Error installing dependencies", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (commandLineOutput) Console.WriteLine("Installed Open-Shell succesfully");
                    returnCode = InstallableUtility.StartIsBackPlusPlus.Install();
                    if (returnCode != 0)
                    {
                        if (commandLineOutput) Console.WriteLine("Error: StartIsBack++ installer returned error code {0}", returnCode);
                        else MessageBox.Show($"StartIsBack++ installer returned error code {returnCode}", "Error installing dependencies", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (commandLineOutput) Console.WriteLine("Installed StartIsBack++ succesfully");
                    break;
                case TaskbarType.ExplorerPatcher:
                    MessageBox.Show("You have to install ExplorerPatcher through SCT's 'Patch Explorer' GUI.", "Error installing dependencies");
                    return false;
                default:
                    if (commandLineOutput) Console.WriteLine("Warning: TaskbarType is not SCTT or OS+SiB. No dependencies will be installed.");
                    else MessageBox.Show("Taskbar type is not SimpleClassicThemeTaskbar or Open-Shell with StartIsBack. No dependencies will be installed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
            if (commandLineOutput) Console.WriteLine("Dependencies installed succesfully");
            else MessageBox.Show("Dependencies installed succesfully", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }
    }

    /// <summary>
    /// Helper class: Allows comparing version numbers while specifying significance
    /// </summary>
    public static class Extensions
    {
        public static List<int> AllIndexesOf(this string str, char searchstring)
        {
            List<int> indexes = new List<int>();
            int minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                indexes.Add(minIndex);
                minIndex = str.IndexOf(searchstring, minIndex + 1);
            }
            return indexes;
        }

        public static int CompareString(this Version version, string otherVersionString)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }
            if (otherVersionString == null || otherVersionString == string.Empty)
            {
                return 1;
            }
            int significantParts = otherVersionString.AllIndexesOf('.').Count;
            Version otherVersion = new Version(otherVersionString);

            if (version.Major != otherVersion.Major && significantParts >= 1)
                if (version.Major > otherVersion.Major)
                    return 1;
                else
                    return -1;

            if (version.Minor != otherVersion.Minor && significantParts >= 2)
                if (version.Minor > otherVersion.Minor)
                    return 1;
                else
                    return -1;

            if (version.Build != otherVersion.Build && significantParts >= 3)
                if (version.Build > otherVersion.Build)
                    return 1;
                else
                    return -1;

            if (version.Revision != otherVersion.Revision && significantParts >= 4)
                if (version.Revision > otherVersion.Revision)
                    return 1;
                else
                    return -1;

            return 0;
        }
    }
}
