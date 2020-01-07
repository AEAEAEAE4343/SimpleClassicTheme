using Microsoft.Win32;
using NtApiDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "Enabled", true.ToString());
            //Windows 8.1 with taskbar
            if (taskbar && Environment.OSVersion.Version.Major != 10)
            {
                Enable();
                File.WriteAllBytes("C:\\SCT\\fixstrips.exe", Properties.Resources.fixstrips);
                Process.Start("C:\\SCT\\fixstrips.exe").WaitForExit();

                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "EnableStartButton", 1);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "CustomTaskbar", 1);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "WinKey", "ClassicMenu");
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "MouseClick", "ClassicMenu");
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
        }

        //Disables Classic Theme and if specified Classic Taskbar.
        public static void MasterDisable(bool taskbar)
        {
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "Enabled", false.ToString());
            if (taskbar)
            {
                Disable();
                ClassicTaskbar.Disable();
            }
            else
            {
                Disable();
            }
        }
    }
}
