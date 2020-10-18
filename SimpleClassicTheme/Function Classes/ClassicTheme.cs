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
                    MessageBox.Show("This action requires SCT to be installed");
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
