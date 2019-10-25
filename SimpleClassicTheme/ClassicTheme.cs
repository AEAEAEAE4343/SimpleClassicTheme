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
            File.WriteAllText("\\windowmetrics.reg", Properties.Resources.WindowMetrics);
            Process.Start("C:\\Windows\\regedit.exe", "/s C:\\windowmetrics.reg").WaitForExit();
            File.Delete("C:\\windowmetrics.reg");
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

        //Enables Classic Theme and if specified Classic Taskbar. Also makes sure ExplorerContextMenuTweaker can load.
        public static void MasterEnable(bool taskbar)
        {
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "Enabled", true.ToString());
            if (taskbar)
            {
                ClassicTaskbar.Enable();
                ClassicTheme.Enable();
            }
            else
            {
                ClassicTheme.Enable();
                if (File.Exists("C:/Windows/System32/ExplorerContextMenuTweaker.dll"))
                {
                    Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                    Process.Start("cmd", "/c taskkill /im sihost.exe /f").WaitForExit();
                    //Give Windows Explorer, StartIsBack and Classic Shell the time to load
                    Thread.Sleep((int)Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme").GetValue("TaskbarDelay", 5000));
                }
            }
        }

        //Disables Classic Theme and if specified Classic Taskbar. Also makes sure ExplorerContextMenuTweaker can unload.
        public static void MasterDisable(bool taskbar)
        {
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\SimpleClassicTheme", "Enabled", false.ToString());
            if (taskbar)
            {
                ClassicTheme.Disable();
                ClassicTaskbar.Disable();
            }
            else
            {
                ClassicTheme.Disable();
                if (File.Exists("C:/Windows/System32/ExplorerContextMenuTweaker.dll"))
                {
                    Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                    Process.Start("cmd", "/c taskkill /im sihost.exe /f").WaitForExit();
                }
            }
        }
    }
}
