using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    public class SystemMenuForm : Form
    {
        [DllImport("user32.dll")]
        private static extern bool SetMenu(IntPtr hWnd, IntPtr hMenu);

        private SystemMenu systemMenu;
        public SystemMenu SystemMenu 
        {
            get 
            {
                return systemMenu;
            }
            set
            {
                SetMenu(Handle, value != null ? value.MenuHandle : IntPtr.Zero);
                systemMenu = value;
            }
        }

        protected override void WndProc(ref Message m)
        {
            // WM_COMMAND with HIWORD(wParam) == 0 && lParam == 0
            if (m.Msg == 0x0111 && 
                (m.WParam.ToInt32() & 0xFFFF0000) == 0 &&
                m.LParam.ToInt32() == 0 &&
                SystemMenu != null)
            {
                SystemMenu.PerformAction(m.WParam.ToInt32());
            }

            base.WndProc(ref m);
        }
    }
}
