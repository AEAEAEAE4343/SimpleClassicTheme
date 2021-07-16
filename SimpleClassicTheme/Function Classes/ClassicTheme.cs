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
                Thread.Sleep(100);
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
        public static void MasterEnable(bool taskbar, bool force = false, bool commandLineError = false)
        {
            Process.Start("C:\\SCT\\EnableThemeScript.bat", "pre").WaitForExit();
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\1337ftw\SimpleClassicTheme", "Enabled", true.ToString());
            //SCTT
            if (taskbar && (string)Configuration.GetItem("TaskbarType", "SiB+OS") == "SCTT")
            {
#if DEBUG
#else
                if (!force && !File.Exists("C:\\SCT\\SCT.exe"))
                {
                    if (!commandLineError)
                        MessageBox.Show("This action requires SCT to be installed");
                    else
                        Console.WriteLine("This action requires SCT to be installed");
                }
                else
#endif
                {
                    Enable();
                    Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                    Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
                    ClassicTaskbar.EnableSCTT();
                }
            }
            //Windows 8.1
            else if (Environment.OSVersion.Version.Major != 10)
            {
                //Enable the theme
                Enable();

                //Make explorer apply theme
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
                Thread.Sleep((int)Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme").GetValue("TaskbarDelay", 5000));

                ClassicTaskbar.FixWin8_1();
            }
            //Windows 10 with taskbar
            else if (taskbar)
            {
                ClassicTaskbar.Enable();
                Enable();
            }
            //Just enable
            else
            {
                Enable();
            }
            Process.Start("C:\\SCT\\EnableThemeScript.bat", "post").WaitForExit();
        }

        //Disables Classic Theme and if specified Classic Taskbar.
        public static void MasterDisable(bool taskbar)
        {
            Process.Start("C:\\SCT\\DisableThemeScript.bat", "pre").WaitForExit();
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\1337ftw\SimpleClassicTheme", "Enabled", false.ToString());
            //SCTT
            if (taskbar && (string)Configuration.GetItem("TaskbarType", "SiB+OS") == "SCTT")
            {
                ClassicTaskbar.DisableSCTT();
                Disable();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
            }
            //Windows 8.1
            else if (Environment.OSVersion.Version.Major != 10)
            {
                Disable();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
            }
            //Windows 10 with taskbar
            else if (taskbar)
            {
                Disable();
                ClassicTaskbar.Disable();
            }
            //Just disable
            else
            {
                Disable();
            }
            Process.Start("C:\\SCT\\DisableThemeScript.bat", "post").WaitForExit();
        }

        //Full SCT uninstall
        public static void RemoveSCT()
        {
            Directory.CreateDirectory("C:\\SCT");

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
            if (Directory.Exists(path + "\\AppData\\Local\\StartIsBack") && MessageBox.Show("StartIsBack++ has been found on the system.\nWould you like SCT to remove it?", "SCT Uninstallation", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                if (!File.Exists("C:\\SCT\\RibbonDisabler.exe"))
                {
                    File.WriteAllBytes("C:\\SCT\\RibbonDisabler.exe", Properties.Resources.ribbonDisabler);
                }
                Process.Start("C:\\SCT\\RibbonDisabler.exe").WaitForExit();
            }

            //Delete SCT, SCTT and T-Clock
            MessageBox.Show("SCT Pre-removal has finished succesfully.\nSCT will now be uninstalled.", "SCT Uninstallation");
            MessageBox.Show("Several registry imports will be done to restore settings.\nPlease click yes on all of them.", "SCT Uninstallation");

            //Put Windows Aero scheme on
            File.WriteAllText("C:\\SCT\\reg_windowcolors_restore.reg", Properties.Resources.reg_windowcolors_restore);
            Process.Start("C:\\SCT\\reg_windowcolors_restore.reg").WaitForExit();
            Process.Start("C:\\Windows\\Resources\\Themes\\aero.theme").WaitForExit();

            //Restore WindowMetrics
            File.WriteAllText("C:\\SCT\\reg_windowmetrics_restore.reg", Environment.OSVersion.Version.Major == 10 ? Properties.Resources.reg_windowmetrics_restore : Properties.Resources.reg_windowmetrics_81);
            Process.Start("C:\\SCT\\reg_windowmetrics_restore.reg").WaitForExit();

            //Make borders 2D
            File.WriteAllText("C:\\SCT\\reg_upm_disable3d.reg", Properties.Resources.reg_upm_disable3d);
            Process.Start("C:\\SCT\\reg_upm_disable3d.reg").WaitForExit();

            //Remove SCT Task
            Process.Start("C:\\Windows\\System32\\schtasks.exe", "/Delete /TN \"Simple Classic Theme\" /F").WaitForExit();

            //Remove registry for both SCT and SCTT
            Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme");
            Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("SimpleClassicThemeTaskbar");
            Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").DeleteSubKeyTree("SimpleClassicTheme");
            Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").DeleteSubKeyTree("SimpleClassicThemeTaskbar");
            if (Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").SubKeyCount == 0)
                Registry.CurrentUser.CreateSubKey("SOFTWARE").DeleteSubKeyTree("1337ftw");

            //File removal
            File.WriteAllText("C:\\RemoveSCT.bat", Properties.Resources.removalString);
            Process.Start("C:\\RemoveSCT.bat");
            Environment.Exit(0);
        }
    }
}
