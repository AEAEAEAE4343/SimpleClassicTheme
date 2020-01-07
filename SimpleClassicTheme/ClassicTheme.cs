using Microsoft.Win32;
using NtApiDotNet;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

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
            //Windows 8.1
            if (Environment.OSVersion.Version.Major != 10)
            {
                //Enable the theme
                Enable();

                //Make explorer apply theme
                Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
                Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
                Thread.Sleep((int)Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SimpleClassicTheme").GetValue("TaskbarDelay", 5000));

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
            //Windows 8.1
            if(Environment.OSVersion.Version.Major != 10)
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
        }
    }
}
