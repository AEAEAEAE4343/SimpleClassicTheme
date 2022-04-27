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
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel;

namespace SimpleClassicTheme
{
    public enum ClassicThemeMethod
    {
        SingleUserSCT,
        MultiUserClassicTheme,
    }

    public static class ClassicTheme
    {
        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        private static extern unsafe uint NtOpenSection(out IntPtr sectionHandle, AccessMask desiredAccess, ref ObjectAttributes objectAttributes);
        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        private static extern unsafe uint NtSetSecurityObject(IntPtr sectionHandle, SecurityInformation desiredAccess, IntPtr securityDescriptor);
        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        private static extern unsafe uint NtClose(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        private static extern unsafe uint ConvertStringSecurityDescriptorToSecurityDescriptor(string StringSecurityDescriptor, uint StringSDRevision, out IntPtr SecurityDescriptor, out UIntPtr SecurityDescriptorSize);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr LocalFree(IntPtr hMem);


        [StructLayout(LayoutKind.Sequential)]
        private unsafe struct UnicodeString
        {
            public ushort Length;
            public ushort MaxLength;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 520)]
            public byte* Buffer;

            public static UnicodeString Create(string text)
            {
                UnicodeString s = new UnicodeString();
                s.Length = (ushort)(text.Length * 2);
                s.MaxLength = 520;
                s.Buffer = (byte*)Marshal.AllocHGlobal(520);

                int strSize = Math.Min(text.Length * 2, 518);
                Marshal.Copy(Encoding.Unicode.GetBytes(text), 0, (IntPtr)s.Buffer, strSize);
                s.Buffer[strSize] = 0; s.Buffer[strSize + 1] = 0;
                return s;
            }

            public void Free()
            {
                Marshal.FreeHGlobal((IntPtr)Buffer);
            }
        }

        private enum AttributesEnum : uint
        {
            OBJ_CASE_INSENSITIVE = 0x40U,
            OBJ_KERNEL_HANDLE = 0x200U,
        }

        private enum AccessMask : uint
        {
            WRITE_DAC = 0x00040000U,
        }

        private enum SecurityInformation : uint
        {
            DACL_SECURITY_INFORMATION = 0x4U,
        }

        private unsafe struct ObjectAttributes
        {
            public uint Length;
            public IntPtr RootDirectory;
            [MarshalAs(UnmanagedType.LPStruct)]
            public UnicodeString* ObjectName;
            public AttributesEnum Attributes;
            public IntPtr SecurityDescriptor;
            public IntPtr SecurityQualityOfService;
        }

        /// <summary>
        /// Enables Classic Theme by changing the ThemeSection permissions directly. This will only work with in an elevated process.
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully. If the elevation of the current process is not high enough, this returns false.</returns>
        public static unsafe bool EnableSingleUser()
        {
            UnicodeString uniStr = UnicodeString.Create($@"\Sessions\{Process.GetCurrentProcess().SessionId}\Windows\ThemeSection");
            ObjectAttributes attrib = new ObjectAttributes();
            attrib.Length = (uint)sizeof(ObjectAttributes);
            attrib.ObjectName = &uniStr;
            attrib.Attributes = AttributesEnum.OBJ_CASE_INSENSITIVE | AttributesEnum.OBJ_KERNEL_HANDLE;

            uint result = NtOpenSection(out IntPtr section, AccessMask.WRITE_DAC, ref attrib);
            if (result != 0U)
                throw new NtException((NtStatus)result);

            result = ConvertStringSecurityDescriptorToSecurityDescriptor("O:BAG:SYD:(A;;RC;;;IU)(A;;DCSWRPSDRCWDWO;;;SY)", 1, out IntPtr securityDescriptor, out _);
            if (result == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            result = NtSetSecurityObject(section, SecurityInformation.DACL_SECURITY_INFORMATION, securityDescriptor);
            if (result != 0)
                throw new NtException((NtStatus)result);

            LocalFree(securityDescriptor);

            NtClose(section);

            /*try
            {
                NtObject g = NtObject.OpenWithType("Section", $@"\Sessions\{Process.GetCurrentProcess().SessionId}\Windows\ThemeSection", null, GenericAccessRights.WriteDac);
                g.SetSecurityDescriptor(new SecurityDescriptor("O:BAG:SYD:(A;;RC;;;IU)(A;;DCSWRPSDRCWDWO;;;SY)"), NtApiDotNet.SecurityInformation.Dacl);
                g.Close();
            }
            catch (NtException e) 
            {
                if ((uint)e.HResult != 0xC0000022)
                    throw e;
                return false;
            }*/
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
                g.SetSecurityDescriptor(new SecurityDescriptor("O:BAG:SYD:(A;;CCLCRC;;;IU)(A;;CCDCLCSWRPSDRCWDWO;;;SY)"), NtApiDotNet.SecurityInformation.Dacl);
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
