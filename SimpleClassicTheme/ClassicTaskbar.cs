using Microsoft.Win32;
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
    public static class ClassicTaskbar
    {
        public static void Enable()
        {
            //Start settings app in order to increase chances of metro working
            Process.Start("ms-settings:");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "EnableStartButton", 1);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "CustomTaskbar", 1);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "WinKey", "ClassicMenu");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "MouseClick", "ClassicMenu");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\StartIsBack", "Disabled", 0);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 0);
            Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
            Process.Start("cmd", "/c taskkill /im sihost.exe /f").WaitForExit();
            //Give Windows Explorer, StartIsBack and Classic Shell the time to load
            Thread.Sleep((int)Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme").GetValue("TaskbarDelay", 5000));
        }

        public static void Disable()
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "EnableStartButton", 0);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "CustomTaskbar", 0);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "WinKey", "WindowsMenu");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "MouseClick", "WindowsMenu");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\StartIsBack", "Disabled", 1);
            Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
            Process.Start("cmd", "/c taskkill /im sihost.exe /f").WaitForExit();
        }
    }
}
