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

using Microsoft.Win32;
using NtApiDotNet;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace SimpleClassicTheme
{
    public static class ClassicTheme
    {
        //Enables Classic Theme
        public static void Enable()
        {
            if (Environment.OSVersion.Version.Major == 10)
            { 
                Process.Start("explorer.exe", "C:\\Windows\\System32\\ApplicationFrameHost.exe").WaitForExit();
                Thread.Sleep(200);
            }
            NtObject g = NtObject.OpenWithType("Section", $@"\Sessions\{Process.GetCurrentProcess().SessionId}\Windows\ThemeSection", null, GenericAccessRights.WriteDac);
            g.SetSecurityDescriptor(new SecurityDescriptor("O:BAG:SYD:(A;;RC;;;IU)(A;;DCSWRPSDRCWDWO;;;SY)"), SecurityInformation.Dacl);
            g.Close();
            Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Themes").CreateSubKey("DefaultColors");
            ExtraFunctions.RenameSubKey(Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Themes"), "DefaultColors", "DefaultColorsOld");
        }

        //Disables Classic Theme
        public static void Disable()
        {
            NtObject g = NtObject.OpenWithType("Section", $@"\Sessions\{Process.GetCurrentProcess().SessionId}\Windows\ThemeSection", null, GenericAccessRights.WriteDac);
            g.SetSecurityDescriptor(new SecurityDescriptor("O:BAG:SYD:(A;;CCLCRC;;;IU)(A;;CCDCLCSWRPSDRCWDWO;;;SY)"), SecurityInformation.Dacl);
            g.Close();
        }

        //Enables Classic Theme and if specified Classic Taskbar.
        public static void MasterEnable(bool taskbar, bool commandLineError = false)
        {
            Process.Start($"{Configuration.InstallPath}EnableThemeScript.bat", "pre").WaitForExit();
            Configuration.Enabled = true;
            if (!taskbar)
            {
                // No taskbar
                if (ExplorerPatcher.Enabled) ExplorerPatcher.ApplyConfiguration(true);
                Enable();
            }
            else if (!File.Exists($"{Configuration.InstallPath}SCT.exe"))
			{
                if (!commandLineError)
                    MessageBox.Show("You need to install Simple Classic Theme in order to enable it with Classic Taskbar enabled. Either disable Classic Taskbar from the options menu, or install SCT by pressing 'Run SCT on boot' in the main menu.", "Unsupported action");
                else
                    Console.WriteLine($"Enabling SCT with a taskbar requires SCT to be installed to \"{Configuration.InstallPath}SCT.exe\".");
                Configuration.Enabled = false;
                return;
            }
            else if (Configuration.TaskbarType == TaskbarType.StartIsBackOpenShell)
            {
                if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 3)
                {
                    // StartIsBack+ and Open-Shell
                    // Unsupported for now
                    if (!commandLineError)
                        MessageBox.Show("StartIsBack+ is still unsupported. Please select another taskbar method through the options menu or with the --set commandline option.", "Unsupported action");
                    else
                        Console.WriteLine("StartIsBack+ is still unsupported. Please select another taskbar method.");
                    Configuration.Enabled = false;
                }
                else if (Environment.OSVersion.Version.Major == 10 && Environment.OSVersion.Version.Build < 22000)
                {
                    // StartIsBack++ and Open-Shell
                    if (ExplorerPatcher.Enabled) ExplorerPatcher.ApplyConfiguration();
                    ClassicTaskbar.Enable();
                    Thread.Sleep(Configuration.TaskbarDelay);
                    Enable();
                }
                else if (Environment.OSVersion.Version.Major == 10 && Environment.OSVersion.Version.Build >= 22000)
                {
                    // StartAllBack and Open-Shell
                    // Unsupported for now
                    if (!commandLineError)
                        MessageBox.Show("StartAllBack is still unsupported. Please select another taskbar method through the options menu or with the --set commandline option.", "Unsupported action");
                    else
                        Console.WriteLine("StartAllBack is still unsupported. Please select another taskbar method.");
                    Configuration.Enabled = false;
                }
                else
                {
                    // Unsupported operating system
                    if (!commandLineError)
                        MessageBox.Show("There's no version of StartIsBack for your version of Windows. Please select another taskbar method through the options menu or with the --set commandline option.", "Unsupported action");
                    else
                        Console.WriteLine("There's no version of StartIsBack for your version of Windows. Please select another taskbar method.");
                    Configuration.Enabled = false;
                }
            }
            else if (Configuration.TaskbarType == TaskbarType.SimpleClassicThemeTaskbar)
            {
                // Simple Classic Theme Taskbar
                Enable();
                if (ExplorerPatcher.Enabled) ExplorerPatcher.ApplyConfiguration();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
                ClassicTaskbar.EnableSCTT();
            }
            else if (Configuration.TaskbarType == TaskbarType.Windows81Vanilla)
			{
                // Windows 8.1 Vanilla taskbar with post-load patches
                Enable();

                if (ExplorerPatcher.Enabled) ExplorerPatcher.ApplyConfiguration();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
                Thread.Sleep(Configuration.TaskbarDelay);
                ClassicTaskbar.FixWin8_1();
            }
            else if (Configuration.TaskbarType == TaskbarType.RetroBar)
			{
                // RetroBar
                Enable();
                if (ExplorerPatcher.Enabled) ExplorerPatcher.ApplyConfiguration();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
                Process.Start($"{Configuration.InstallPath}RetroBar\\RetroBar.exe");
            }
            else if (Configuration.TaskbarType == TaskbarType.ExplorerPatcher)
			{
                // Windows 11 Vanilla taskbar with ExplorerPatcher
                Enable();
                if (ExplorerPatcher.Enabled) ExplorerPatcher.ApplyConfiguration(true);
            }
            Process.Start($"{Configuration.InstallPath}EnableThemeScript.bat", "post").WaitForExit();
        }

        //Disables Classic Theme and if specified Classic Taskbar.
        public static void MasterDisable(bool taskbar)
        {
            Process.Start($"{Configuration.InstallPath}DisableThemeScript.bat", "pre").WaitForExit();
            Configuration.Enabled = false;
            if (!taskbar || Configuration.TaskbarType == TaskbarType.ExplorerPatcher)
            {
                Disable();
                if (ExplorerPatcher.Enabled) ExplorerPatcher.ApplyConfiguration(true);
            }
            else if (Configuration.TaskbarType == TaskbarType.StartIsBackOpenShell)
            {
                if (Environment.OSVersion.Version.Major == 10 && Environment.OSVersion.Version.Build < 22000)
                {
                    Disable();
                    ClassicTaskbar.Disable();
                }
            }
            else if (Configuration.TaskbarType == TaskbarType.SimpleClassicThemeTaskbar)
            {
                ClassicTaskbar.DisableSCTT();
                Disable();
                if (ExplorerPatcher.Enabled) ExplorerPatcher.ApplyConfiguration();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
            }
            else if (Configuration.TaskbarType == TaskbarType.Windows81Vanilla)
            {
                Disable();
                if (ExplorerPatcher.Enabled) ExplorerPatcher.ApplyConfiguration();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
            }
            else if (Configuration.TaskbarType == TaskbarType.RetroBar)
            {
                foreach (Process p in Process.GetProcessesByName("RetroBar"))
                    p.Kill();

                Disable();
                if (ExplorerPatcher.Enabled) ExplorerPatcher.ApplyConfiguration();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
            }
            Process.Start($"{Configuration.InstallPath}DisableThemeScript.bat", "post").WaitForExit();
        }

        //Full SCT uninstall
        public static void RemoveSCT()
        {
            //Stop classic theme
            MasterDisable(false);

            //Prepare local appdata path for uninstallation of certain utilities
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
                path = Directory.GetParent(path).ToString();

            //Optionally also uninstall OS, SiB, Classic Task Manager, Folder Options X, 7+TT and ExplorerContextMenuTweaker
            if (Directory.Exists("C:\\Program Files\\Open-Shell") && MessageBox.Show("Open-Shell has been found on the system.\nWould you like SCT to remove it?", "SCT Uninstallation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start("C:\\Windows\\System32\\msiexec.exe", "/X{FD722BB1-4960-455F-89C6-EFAEB79527EF}").WaitForExit();
            }
            if (Directory.Exists(path + "\\AppData\\Local\\StartIsBack") && MessageBox.Show("StartIsBack(+(+)) has been found on the system.\nWould you like SCT to remove it?", "SCT Uninstallation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(path + "\\AppData\\Local\\StartIsBack\\StartIsBackCfg.exe", "/uninstall").WaitForExit();
            }
            if (Directory.Exists("C:\\Program Files\\ClassicTaskmgr") && MessageBox.Show("Classic Task Manager has been found on the system.\nWould you like SCT to remove it?", "SCT Uninstallation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start("C:\\Program Files\\ClassicTaskmgr\\unins000.exe").WaitForExit();
            }
            if (Directory.Exists("C:\\Program Files\\T800 Productions\\Folder Options X") && MessageBox.Show("Folder Options X has been found on the system.\nWould you like SCT to remove it?", "SCT Uninstallation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start("C:\\Program Files\\T800 Productions\\Folder Options X\\unins000.exe").WaitForExit();
            }
            if (Directory.Exists(path + "\\AppData\\Roaming\\7+ Taskbar Tweaker") && MessageBox.Show("7+ Taskbar Tweaker has been found on the system.\nWould you like SCT to remove it?", "SCT Uninstallation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(path + "\\AppData\\Roaming\\7+ Taskbar Tweaker\\uninstall.exe").WaitForExit();
            }
            if (File.Exists("C:\\Windows\\System32\\ExplorerContextMenuTweaker.dll") && MessageBox.Show("ExplorerContextMenuTweaker has been found on the system.\nWould you like SCT to remove it?", "SCT Uninstallation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start("C:\\Windows\\System32\\regsvr32.exe", "/u C:\\Windows\\System32\\ExplorerContextMenuTweaker.dll").WaitForExit();
                Process.Start("C:\\Windows\\System32\\taskkill.exe", "/im explorer.exe /f").WaitForExit();
                File.Delete("C:\\Windows\\System32\\ExplorerContextMenuTweaker.dll");
                File.Delete("C:\\Windows\\System32\\ShellPayload.dll");
            }

            //Ask user if they want to disable Ribbon now
            if (MessageBox.Show("Would you like to run RibbonDisabler to enable the ribbon before removing SCT?", "SCT Uninstallation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (!File.Exists($"{Configuration.InstallPath}RibbonDisabler.exe"))
                {
                    File.WriteAllBytes($"{Configuration.InstallPath}RibbonDisabler.exe", Properties.Resources.ribbonDisabler);
                }
                Process.Start($"{Configuration.InstallPath}RibbonDisabler.exe").WaitForExit();
            }

            //Delete SCT, SCTT and T-Clock
            MessageBox.Show("SCT Pre-removal has finished succesfully.\nSCT will now be uninstalled.", "SCT Uninstallation");
            MessageBox.Show("Several registry imports will be done to restore settings.\nPlease click yes on all of them.", "SCT Uninstallation");

            //Put Windows Aero scheme on
            File.WriteAllText($"{Configuration.InstallPath}reg_windowcolors_restore.reg", Properties.Resources.reg_windowcolors_restore);
            Process.Start($"{Configuration.InstallPath}reg_windowcolors_restore.reg").WaitForExit();
            Process.Start("C:\\Windows\\Resources\\Themes\\aero.theme").WaitForExit();

            //Restore WindowMetrics
            File.WriteAllText($"{Configuration.InstallPath}reg_windowmetrics_restore.reg", Environment.OSVersion.Version.Major == 10 ? Properties.Resources.reg_windowmetrics_restore : Properties.Resources.reg_windowmetrics_81);
            Process.Start($"{Configuration.InstallPath}reg_windowmetrics_restore.reg").WaitForExit();

            //Make borders 2D
            File.WriteAllText($"{Configuration.InstallPath}reg_upm_disable3d.reg", Properties.Resources.reg_upm_disable3d);
            Process.Start($"{Configuration.InstallPath}reg_upm_disable3d.reg").WaitForExit();

            //Remove SCT Task
            Process.Start("C:\\Windows\\System32\\schtasks.exe", "/Delete /TN \"Simple Classic Theme\" /F").WaitForExit();

            //Remove registry for both SCT and SCTT
            Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("Simple Classic Theme").CreateSubKey("Base");
            Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("Simple Classic Theme").CreateSubKey("Taskbar");
            Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("Simple Classic Theme").CreateSubKey("Common");
            Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("Simple Classic Theme").DeleteSubKeyTree("Base");
            Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("Simple Classic Theme").DeleteSubKeyTree("Taskbar");
            Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("Simple Classic Theme").DeleteSubKeyTree("Common");
            if (Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("Simple Classic Theme").SubKeyCount <= 1)
                Registry.CurrentUser.CreateSubKey("SOFTWARE").DeleteSubKeyTree("1337ftw");

            //File removal
            File.WriteAllText("C:\\RemoveSCT.bat", Properties.Resources.removalString.Replace("{InstallPath}", $"{Configuration.InstallPath}"));
            Process.Start("C:\\RemoveSCT.bat");
            Environment.Exit(0);
        }
    }
}
