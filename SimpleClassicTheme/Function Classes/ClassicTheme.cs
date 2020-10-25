/*
 *  SimpleClassicTheme, a basic utility to bring back classic theme to newer version of the Windows operating system.
 *  Copyright (C) 2020 Anis Errais
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
        public static void MasterEnable(bool taskbar)
        {
            Process.Start("C:\\SCT\\EnableThemeScript.bat", "pre").WaitForExit();
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\1337SimpleClassicTheme", "Enabled", true.ToString());
            //SCTT
            if ((string)Configuration.GetItem("TaskbarType", "SiB+OS") == "SCTT")
            {
#if DEBUG
#else
                if (Assembly.GetExecutingAssembly().Location != "C:\\SCT\\SCT.exe")
                {
                    MessageBox.Show("This action requires SCT to be installed and SCT to be ran from the Start Menu");
                }
                else
#endif
                {
                    ClassicTaskbar.EnableSCTT();
                    Enable();
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
            if ((string)Configuration.GetItem("TaskbarType", "SiB+OS") == "SCTT")
            {
                ClassicTaskbar.DisableSCTT();
                Disable();
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
    }
}
