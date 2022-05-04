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

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

using MCT.NET;

using static SimpleClassicTheme.NtApi;
using static SimpleClassicTheme.CommonControls;
using static SimpleClassicTheme.Logger;

namespace SimpleClassicTheme
{
    public static class ClassicTheme
    {
        public enum ClassicThemeMethod
        {
            SingleUserSCT,
            MultiUserClassicTheme,
        }

        public enum ClassicThemeErrorSource
        {
            /// <summary>
            /// The error didn't have any source. This means no error occured.
            /// </summary>
            None = 0,

            /// <summary>
            /// The error occured within MCT.
            /// </summary>
            Mct = 1,

            /// <summary>
            /// The error occured within Win32.
            /// </summary>
            Win32 = 2,

            /// <summary>
            /// The error happened in low-level system API's. 
            /// </summary>
            NtDll = 3,
        }

        public struct ClassicThemeResult
        {
            public bool Success;
            public ClassicThemeErrorSource Source;
            public uint ErrorCode;

            public string GetDescription()
            {
                if (Success)
                {
                    Source = ClassicThemeErrorSource.Win32;
                    ErrorCode = 0;
                }

                switch (Source)
                {
                    case ClassicThemeErrorSource.NtDll:
                    case ClassicThemeErrorSource.Win32:
                        return new Win32Exception((int)ErrorCode).Message;
                    case ClassicThemeErrorSource.Mct:
                        return MctApi.GetErrorString((MctApi.MctError)ErrorCode);
                    default:
                        return "";
                }
            }
        }

        private static bool IsAdministrator => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        private static unsafe ClassicThemeResult SetThemeSectionSecurity(string dacl)
        {
            UnicodeString uniStr = UnicodeString.Create($@"\Sessions\{Process.GetCurrentProcess().SessionId}\Windows\ThemeSection");
            ObjectAttributes attrib = new ObjectAttributes();
            attrib.Length = (uint)sizeof(ObjectAttributes);
            attrib.ObjectName = &uniStr;
            attrib.Attributes = AttributesEnum.OBJ_CASE_INSENSITIVE | AttributesEnum.OBJ_KERNEL_HANDLE;

            uint result = NtOpenSection(out IntPtr section, AccessMask.WRITE_DAC, ref attrib);
            if (result != 0U)
                return new ClassicThemeResult
                {
                    Success = false,
                    ErrorCode = result,
                    Source = ClassicThemeErrorSource.NtDll,
                };

            result = ConvertStringSecurityDescriptorToSecurityDescriptor(dacl, 1, out IntPtr securityDescriptor, out _);
            if (result == 0)
                return new ClassicThemeResult
                {
                    Success = false,
                    ErrorCode = (uint)Marshal.GetLastWin32Error(),
                    Source = ClassicThemeErrorSource.Win32,
                };

            result = NtSetSecurityObject(section, SecurityInformation.DACL_SECURITY_INFORMATION, securityDescriptor);
            if (result != 0)
                return new ClassicThemeResult
                {
                    Success = false,
                    ErrorCode = result,
                    Source = ClassicThemeErrorSource.NtDll,
                };

            LocalFree(securityDescriptor);
            NtClose(section);

            return new ClassicThemeResult
            {
                Success = true,
                ErrorCode = 0,
                Source = ClassicThemeErrorSource.None,
            };
        }

        private static void RestartExplorer(bool wait = false)
        {
            Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
            Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
            if (wait) Thread.Sleep(SCT.Configuration.TaskbarDelay);
        }

        /// <summary>
        /// Enables Classic Theme by changing the ThemeSection permissions directly. This will only work with in an elevated process.
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully. If the elevation of the current process is not high enough, this returns false.</returns>
        public static ClassicThemeResult EnableSingleUser()
        {
            return SetThemeSectionSecurity("O:BAG:SYD:(A;;RC;;;IU)(A;;DCSWRPSDRCWDWO;;;SY)");
        }

        /// <summary>
        /// Enables Classic Theme by sending a request to MCTsvc. This requires MCT to be installed on the system.
        /// </summary>
        /// <returns>A CtResult specifying whether the operation completed succesfully, and if not, what problem occured.</returns>
        public static ClassicThemeResult EnableMCT()
        {
            MctApi.InitializeAPI();

            MctApi.MctErrorCode errorCode = new MctApi.MctErrorCode();
            MctApi.EnableClassicTheme((ulong)Process.GetCurrentProcess().SessionId, ref errorCode);

            return new ClassicThemeResult
            {
                Success = errorCode.Success,
                ErrorCode = errorCode.Result,
                Source = (ClassicThemeErrorSource)errorCode.ErrorSource,
            };
        }

        /// <summary>
        /// Enables Classic Theme using the currently configures ClassicThemeMethod specified in SCT.Configuration
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully.</returns>
        public static ClassicThemeResult Enable()
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
                    return new ClassicThemeResult
                    {
                        Success = false,
                        ErrorCode = 0,
                        Source = ClassicThemeErrorSource.None,
                    };
            }
        }

        /// <summary>
        /// Disables Classic Theme by changing the ThemeSection permissions directly. This will only work with in an elevated process.
        /// </summary>
        /// <returns>A CtResult specifying whether the operation completed succesfully, and if not, what problem occured.</returns>
        public static ClassicThemeResult DisableSingleUser()
        {
            return SetThemeSectionSecurity("O:BAG:SYD:(A;;CCLCRC;;;IU)(A;;CCDCLCSWRPSDRCWDWO;;;SY)");
        }

        /// <summary>
        /// Disables Classic Theme by sending a request to MCTsvc. This requires MCT to be installed on the system.
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully.</returns>
        public static ClassicThemeResult DisableMCT()
        {
            MctApi.MctRevision revision = MctApi.InitializeAPI();

            MctApi.MctErrorCode errorCode = new MctApi.MctErrorCode();
            MctApi.DisableClassicTheme((ulong)Process.GetCurrentProcess().SessionId, ref errorCode);

            return new ClassicThemeResult
            {
                Success = errorCode.Success,
                ErrorCode = errorCode.Result,
                Source = (ClassicThemeErrorSource)errorCode.ErrorSource,
            };
        }

        /// <summary>
        /// Disables Classic Theme using the currently configures ClassicThemeMethod specified in SCT.Configuration
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully.</returns>
        public static ClassicThemeResult Disable()
        {
            switch (SCT.Configuration.ClassicThemeMethod)
            {
                case ClassicThemeMethod.SingleUserSCT:
                    return DisableSingleUser();
                case ClassicThemeMethod.MultiUserClassicTheme:
                    return DisableMCT();
                default:
                    return new ClassicThemeResult
                    {
                        Success = false,
                        ErrorCode = 0,
                        Source = ClassicThemeErrorSource.None,
                    };
            }
        }

        //Enables Classic Theme and if specified Classic Taskbar.
        public static bool MasterEnable()
        {
            if (SCT.Configuration.ClassicThemeMethod == ClassicThemeMethod.SingleUserSCT && !IsAdministrator)
            {
                ErrorMessage("You don't have permission to modify the Classic Theme state", "To enable or disable Classic Theme, either run Simple Classic Theme as Administrator, or install MCT to enable and disable Classic Theme freely.");
                return false;
            }

            Process.Start($"{SCT.Configuration.InstallPath}EnableThemeScript.bat", "pre").WaitForExit();
            ClassicThemeResult res = Enable();
            if (!res.Success)
            {
                TaskDialog.Show(Application.OpenForms[typeof(MainForm).Name], $"{res.GetDescription()}", "Simple Classic Theme", "Failed to enable Classic Theme", TaskDialogButtons.OK, TaskDialogIcon.ErrorIcon);
                return false;
            }

            switch (SCT.Configuration.TaskbarType)
            {
                case TaskbarType.None:
                    break;
                case TaskbarType.Windows81Vanilla:
                    RestartExplorer(true);
                    ClassicTaskbar.FixWin8_1();
                    break;
                case TaskbarType.SimpleClassicThemeTaskbar:
                    RestartExplorer(false);
                    ClassicTaskbar.EnableSCTT();
                    break;
                case TaskbarType.RetroBar:
                    RestartExplorer(false);
                    Process.Start($"{SCT.Configuration.InstallPath}RetroBar\\RetroBar.exe");
                    break;
            }

            Process.Start($"{SCT.Configuration.InstallPath}EnableThemeScript.bat", "post").WaitForExit();
            SCT.Configuration.Enabled = true;
            return true;
        }

        //Disables Classic Theme and if specified Classic Taskbar.
        public static bool MasterDisable()
        {
            if (SCT.Configuration.ClassicThemeMethod == ClassicThemeMethod.SingleUserSCT && !IsAdministrator)
            {
                TaskDialog.Show(Application.OpenForms[typeof(MainForm).Name], "To enable or disable Classic Theme, either run Simple Classic Theme as Administrator, or install MCT to enable and disable Classic Theme freely.", "Simple Classic Theme", "You don't have permission to modify the Classic Theme state", TaskDialogButtons.OK, TaskDialogIcon.ErrorIcon);
                return false;
            }

            Process.Start($"{SCT.Configuration.InstallPath}DisableThemeScript.bat", "pre").WaitForExit();
            ClassicThemeResult res = Disable();
            if (!res.Success)
            {
                TaskDialog.Show(Application.OpenForms[typeof(MainForm).Name], $"{res.GetDescription()}", "Simple Classic Theme", "Failed to disable Classic Theme", TaskDialogButtons.OK, TaskDialogIcon.ErrorIcon);
                return false;
            }

            switch (SCT.Configuration.TaskbarType)
            {
                case TaskbarType.None:
                    break;
                case TaskbarType.Windows81Vanilla:
                    RestartExplorer(false);
                    break;
                case TaskbarType.SimpleClassicThemeTaskbar:
                    ClassicTaskbar.DisableSCTT();
                    RestartExplorer(false);
                    break;
                case TaskbarType.RetroBar:
                    foreach (Process p in Process.GetProcessesByName("RetroBar"))
                        p.Kill();
                    RestartExplorer(false);
                    break;
            }

            Process.Start($"{SCT.Configuration.InstallPath}DisableThemeScript.bat", "post").WaitForExit();
            SCT.Configuration.Enabled = false;
            return true;
        }
    }
}
