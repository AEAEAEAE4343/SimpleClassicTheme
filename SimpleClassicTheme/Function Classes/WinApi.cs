/*
 *  SimpleClassicTheme, a basic utility to bring back classic theme to newer versions of the Windows operating system.
 *  Copyright (C) 2021 Anis Errais
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleClassicTheme
{
    static class Kernel32
    {
        public const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll")]
        public static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeConsole();
    }

    static class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ACCENTPOLICY
        {
            public int nAccentState;
            public int nFlags;
            public uint nColor;
            public int nAnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINCOMPATTRDATA
        {
            public int nAttribute;
            public IntPtr pData;
            public int ulDataSize;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetWindowLongPtrW(IntPtr hWndParent, int index);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SetWindowLongPtrW(IntPtr hWndParent, int index, IntPtr dwNewLong);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowExW(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool SetWindowCompositionAttribute(IntPtr hWnd, ref WINCOMPATTRDATA pAttrData);

        public const int WM_NULL = 0x0000;
        public const int WM_GETTEXT = 0x000D;

        public const int WM_SCT = 0x0420;
        public const int SCTWP_EXIT = 0x0001;
        public const int SCTWP_ISMANAGED = 0x0002;
        public const int SCTWP_ISSCT = 0x0003;
        public const int SCTLP_FORCE = 0x0001;

        public delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        //[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        //public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        //public const int GWL_EXSTYLE = -20;

        public static List<IntPtr> EnumerateProcessWindowHandles(int processId, string name)
        {
            List<IntPtr> handles = new List<IntPtr>();

            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
            {
                EnumThreadWindows(thread.Id, (hWnd, lParam) =>
                {
                    handles.Add(hWnd);
                    return true;
                }, IntPtr.Zero);
            }
            return handles;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
    }

    static class UxTheme
    {
        //Sets the theme of a window. if pszSubAppName and pszSubIdList are both a space, the window will be themed Classic
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
    }
}
