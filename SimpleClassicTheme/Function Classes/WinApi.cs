using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
        public const int WM_EXITTASKBAR = 0x0420;
    }

    static class UxTheme
    {
        //Sets the theme of a window. if pszSubAppName and pszSubIdList are both a space, the window will be themed Classic
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
    }
}
