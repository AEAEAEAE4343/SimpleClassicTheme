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

using MCT.NET;

namespace SimpleClassicTheme
{
    public enum ClassicThemeMethod
    {
        SingleUserSCT,
        MultiUserClassicTheme,
    }

    public static class ClassicTheme
    {
        /// <summary>
        /// Enables Classic Theme by changing the ThemeSection permissions directly. This will only work with in an elevated process.
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully. If the elevation of the current process is not high enough, this returns false.</returns>
        public static bool EnableSingleUser()
        {
            try
            {
                NtObject g = NtObject.OpenWithType("Section", $@"\Sessions\{Process.GetCurrentProcess().SessionId}\Windows\ThemeSection", null, GenericAccessRights.WriteDac);
                g.SetSecurityDescriptor(new SecurityDescriptor("O:BAG:SYD:(A;;RC;;;IU)(A;;DCSWRPSDRCWDWO;;;SY)"), SecurityInformation.Dacl);
                g.Close();
            }
            catch (NtException e) 
            {
                if ((uint)e.HResult != 0xC0000022)
                    throw e;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Enables Classic Theme by sending a request to MCTsvc. This requires MCT to be installed on the system.
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully.</returns>
        public static bool EnableMCT()
        {
            MctApi.InitializeAPI();

            MctApi.MctErrorCode errorCode = new MctApi.MctErrorCode();
            MctApi.EnableClassicTheme((ulong)Process.GetCurrentProcess().SessionId, ref errorCode);
            if (!errorCode.Success)
                return false;
            return true;
        }

        /// <summary>
        /// Enables Classic Theme using the currently configures ClassicThemeMethod specified in SCT.Configuration
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully.</returns>
        public static bool Enable()
        {
            if (Environment.OSVersion.Version.Major == 10)
            { 
                Process.Start("explorer.exe", "C:\\Windows\\System32\\ApplicationFrameHost.exe").WaitForExit();
                Thread.Sleep(200);
            }
            
            switch (SCT.Configuration.ClassicThemeMethod)
            {
                case ClassicThemeMethod.SingleUserSCT:
                    return EnableSingleUser();
                case ClassicThemeMethod.MultiUserClassicTheme:
                    return EnableMCT();
                default:
                    return false;
            }
        }

        /// <summary>
        /// Disables Classic Theme by changing the ThemeSection permissions directly. This will only work with in an elevated process.
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully. If the elevation of the current process is not high enough, this returns false.</returns>
        public static bool DisableSingleUser()
        {
            try
            {
                NtObject g = NtObject.OpenWithType("Section", $@"\Sessions\{Process.GetCurrentProcess().SessionId}\Windows\ThemeSection", null, GenericAccessRights.WriteDac);
                g.SetSecurityDescriptor(new SecurityDescriptor("O:BAG:SYD:(A;;CCLCRC;;;IU)(A;;CCDCLCSWRPSDRCWDWO;;;SY)"), SecurityInformation.Dacl);
                g.Close();
            }
            catch (NtException e)
            {
                if ((uint)e.HResult != 0xC0000022)
                    throw e;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Disables Classic Theme by sending a request to MCTsvc. This requires MCT to be installed on the system.
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully.</returns>
        public static bool DisableMCT()
        {
            MctApi.InitializeAPI();

            MctApi.MctErrorCode errorCode = new MctApi.MctErrorCode();
            MctApi.DisableClassicTheme((ulong)Process.GetCurrentProcess().SessionId, ref errorCode);
            if (!errorCode.Success)
                return false;
            return true;
        }

        /// <summary>
        /// Disables Classic Theme using the currently configures ClassicThemeMethod specified in SCT.Configuration
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully.</returns>
        public static bool Disable()
        {
            switch (SCT.Configuration.ClassicThemeMethod)
            {
                case ClassicThemeMethod.SingleUserSCT:
                    return DisableSingleUser();
                case ClassicThemeMethod.MultiUserClassicTheme:
                    return DisableMCT();
                default:
                    return false;
            }
        }

        //Enables Classic Theme and if specified Classic Taskbar.
        public static void MasterEnable(bool taskbar, bool commandLineError = false)
        {
            Process.Start($"{SCT.Configuration.InstallPath}EnableThemeScript.bat", "pre").WaitForExit();
            SCT.Configuration.Enabled = true;
            if (!taskbar)
            {
                // No taskbar
                Enable();
            }
            else if (!File.Exists($"{SCT.Configuration.InstallPath}SCT.exe"))
			{
                if (!commandLineError)
                    MessageBox.Show("You need to install Simple Classic Theme in order to enable it with Classic Taskbar enabled. Either disable Classic Taskbar from the options menu, or install SCT by pressing 'Run SCT on boot' in the main menu.", "Unsupported action");
                else
                    Console.WriteLine($"Enabling SCT with a taskbar requires SCT to be installed to \"{SCT.Configuration.InstallPath}SCT.exe\".");
                SCT.Configuration.Enabled = false;
                return;
            }
            else if (SCT.Configuration.TaskbarType == TaskbarType.SimpleClassicThemeTaskbar)
            {
                // Simple Classic Theme Taskbar
                Enable();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
                ClassicTaskbar.EnableSCTT();
            }
            else if (SCT.Configuration.TaskbarType == TaskbarType.Windows81Vanilla)
			{
                // Windows 8.1 Vanilla taskbar with post-load patches
                Enable();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
                Thread.Sleep(SCT.Configuration.TaskbarDelay);
                ClassicTaskbar.FixWin8_1();
            }
            else if (SCT.Configuration.TaskbarType == TaskbarType.RetroBar)
			{
                // RetroBar
                Enable();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
                Process.Start($"{SCT.Configuration.InstallPath}RetroBar\\RetroBar.exe");
            }
            Process.Start($"{SCT.Configuration.InstallPath}EnableThemeScript.bat", "post").WaitForExit();
        }

        //Disables Classic Theme and if specified Classic Taskbar.
        public static void MasterDisable(bool taskbar)
        {
            Process.Start($"{SCT.Configuration.InstallPath}DisableThemeScript.bat", "pre").WaitForExit();
            SCT.Configuration.Enabled = false;
            if (!taskbar)
            {
                Disable();
            }
            else if (SCT.Configuration.TaskbarType == TaskbarType.SimpleClassicThemeTaskbar)
            {
                ClassicTaskbar.DisableSCTT();
                Disable();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
            }
            else if (SCT.Configuration.TaskbarType == TaskbarType.Windows81Vanilla)
            {
                Disable();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
            }
            else if (SCT.Configuration.TaskbarType == TaskbarType.RetroBar)
            {
                foreach (Process p in Process.GetProcessesByName("RetroBar"))
                    p.Kill();

                Disable();
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
            }
            Process.Start($"{SCT.Configuration.InstallPath}DisableThemeScript.bat", "post").WaitForExit();
        }
    }
}
