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
        Windows81Vanilla = 2,
        SimpleClassicThemeTaskbar = 3,
        RetroBar = 4
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
            double r = (back.R / 255D) <= 0.03928D ? (back.R / 255D) / 12.92D : Math.Pow(((back.R / 255D) + 0.055D) / 1.055D, 2.4D);
            double g = (back.G / 255D) <= 0.03928D ? (back.G / 255D) / 12.92D : Math.Pow(((back.G / 255D) + 0.055D) / 1.055D, 2.4D);
            double b = (back.B / 255D) <= 0.03928D ? (back.B / 255D) / 12.92D : Math.Pow(((back.B / 255D) + 0.055D) / 1.055D, 2.4D);
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

        // Updates the startup executable
        internal static void UpdateStartupExecutable(bool createIfNot)
        {
            if (File.Exists(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe"))
                File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe");
            if (File.Exists(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\Simple Classic Theme.exe"))
                File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\Simple Classic Theme.exe");
            if (createIfNot)
            {
                if (Assembly.GetExecutingAssembly().Location != $"{Configuration.Instance.InstallPath}SCT.exe")
                {
                    File.Delete($"{Configuration.Instance.InstallPath}SCT.exe");
                    File.Copy(Assembly.GetExecutingAssembly().Location, $"{Configuration.Instance.InstallPath}SCT.exe");
                }
                File.WriteAllText($"{Configuration.Instance.InstallPath}SCTTask.xml", Properties.Resources.cmd_create_task);
                File.WriteAllText($"{Configuration.Instance.InstallPath}TaskSchedule.cmd", Properties.Resources.taskScheduleCommands);
                Process task = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = $"{Configuration.Instance.InstallPath}TaskSchedule.cmd",
                        Verb = "runas",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                task.Start();
                task.WaitForExit();
                File.SetAttributes($"{Configuration.Instance.InstallPath}SCT.exe", File.GetAttributes(Assembly.GetExecutingAssembly().Location) | FileAttributes.Hidden);
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                Directory.CreateDirectory(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\");
                IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.lnk");
                shortcut.Description = "Simple Classic Theme";
                shortcut.TargetPath = $"{Configuration.Instance.InstallPath}SCT.exe";
                shortcut.WorkingDirectory = Configuration.Instance.InstallPath;
                shortcut.Save();
            }
            /*else if (File.Exists($"{Configuration.Instance.InstallPath}SCT.exe") && Assembly.GetExecutingAssembly().Location != $"{Configuration.Instance.InstallPath}SCT.exe" && CheckMD5(@"C:\SCT\SCT.exe") != CheckMD5(Assembly.GetExecutingAssembly().Location))
			{
                try
                {
                    File.Delete($"{Configuration.Instance.InstallPath}SCT.exe");
                    File.Copy(Assembly.GetExecutingAssembly().Location, $"{Configuration.Instance.InstallPath}SCT.exe");
                }
                catch (UnauthorizedAccessException)
				{

				}
            }*/
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

        public static bool InstallDependencies(bool commandLineOutput = false)
		{
            switch (Configuration.Instance.TaskbarType)
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
                default:
                    if (commandLineOutput) Console.WriteLine("Warning: Nothing needed for current config. No dependencies will be installed.");
                    else MessageBox.Show("The current configuration does not need anything installed. No dependencies will be installed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
            if (commandLineOutput) Console.WriteLine("Dependencies installed succesfully");
            else MessageBox.Show("Dependencies installed succesfully", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
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
