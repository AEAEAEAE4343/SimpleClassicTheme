using Microsoft.Win32;
using NtApiDotNet;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SimpleClassicTheme
{
    public static class ClassicTaskbar
    {
        public static void Enable()
        {
            //Start settings app in order to increase chances of metro working
            Process.Start("ms-settings:");
            //Set registry so that SIB and OS will load
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "EnableStartButton", 1);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "CustomTaskbar", 1);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "WinKey", "ClassicMenu");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "MouseClick", "ClassicMenu");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\StartIsBack", "Disabled", 0);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 0);
            //Restart explorer
            Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
            Process.Start("cmd", "/c taskkill /im sihost.exe /f").WaitForExit();
            //Give Windows Explorer, StartIsBack and Classic Shell the time to load
            Thread.Sleep((int)Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme").GetValue("TaskbarDelay", 5000));
        }

        public static void Disable()
        {
            //Set registry so that SIB and OS won't load
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "EnableStartButton", 0);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "CustomTaskbar", 0);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "WinKey", "WindowsMenu");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenShell\StartMenu\Settings", "MouseClick", "WindowsMenu");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\StartIsBack", "Disabled", 1);
            //Restart explorer
            Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
            Process.Start("cmd", "/c taskkill /im sihost.exe /f").WaitForExit();
        }

        public static void FixWin8_1()
        {
            /*
             Remove taskbar blur
             */

            //Get a handle to the taskbar
            IntPtr taskBarHandle = User32.FindWindowExW(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", "");
            //Create an ACCENTPOLICY instance which describe to disable any sort of transparency or blur
            User32.ACCENTPOLICY accentPolicy = new User32.ACCENTPOLICY { nAccentState = 0 };
            //Get the size of the ACCENTPOLICY instance
            int accentPolicySize = Marshal.SizeOf(accentPolicy);
            //Get the pointer to the ACCENTPOLICY instance
            IntPtr accentPolicyPtr = Marshal.AllocHGlobal(accentPolicySize);
            //Copy the struct to unmanaged memory so that Win32 can read it
            Marshal.StructureToPtr(accentPolicy, accentPolicyPtr, false);
            //Create a WINCOMPATTRDATA instance which sets the WindowCompositionAttribute (19) to the ACCENTPOLICY instance
            var winCompatData = new User32.WINCOMPATTRDATA
            {
                nAttribute = 19,
                ulDataSize = accentPolicySize,
                pData = accentPolicyPtr
            };
            //Tell Windows to apply the attribute
            User32.SetWindowCompositionAttribute(taskBarHandle, ref winCompatData);
            //Free the pointer to the ACCENTPOLICY instance
            Marshal.FreeHGlobal(accentPolicyPtr);

            /*
             Remove taskbar borders
             */

            //Get the current taskbar WindowStyle
            IntPtr p = User32.GetWindowLongPtrW(taskBarHandle, -16);
            //Set the taskbar WindowStyle to the original plus an offset of 0x400000
            User32.SetWindowLongPtrW(taskBarHandle, -16, new IntPtr(p.ToInt64() + 0x400000));
            //Set the taskbar WindowStyle back to the original
            User32.SetWindowLongPtrW(taskBarHandle, -16, p);
        }

        public static void InstallSCTT(Form parent)
		{
            if (MessageBox.Show(parent, "Would you like to install SCTT?\n\nPlease note that SCTT is far from finished and may contain bugs/lack some features.", "Simple Classic Theme Taskbar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
                SCTTDownload download = new SCTTDownload();
                download.FormClosed += delegate
                {

                };
                download.ShowDialog(parent);
			}
		}

        public static void EnableSCTT()
		{
            if (Process.GetProcessesByName("SCT_Taskbar").Length == 0)
                Process.Start("C:\\SCT\\Taskbar\\SCT_Taskbar.exe", "--sct");
		}

        public static void DisableSCTT()
		{
            IntPtr window = File.Exists("C:\\SCT\\Taskbar\\MainWindow.txt") ? new IntPtr(Int32.Parse(File.ReadAllText("C:\\SCT\\Taskbar\\MainWindow.txt"))) : new IntPtr(0);
            IntPtr wParam = new IntPtr(0x5354); //ST
            IntPtr lParam = new IntPtr(0x4F50); //OP
            User32.SendMessage(window, User32.WM_EXITTASKBAR, wParam, lParam);
		}
    }
}
