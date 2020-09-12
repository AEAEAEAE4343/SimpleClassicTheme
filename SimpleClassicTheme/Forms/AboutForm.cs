using Microsoft.Win32;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace SimpleClassicTheme
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            ClientSize = new Size(400, 420);
        }

        private void About_Load(object sender, EventArgs e)
        {
            string ver = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", Environment.OSVersion.Version.Build.ToString()).ToString();
            label2.Text = label2.Text.Replace("%build%", ver);

            string name = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "Windows " + Environment.OSVersion.Version.Major.ToString()).ToString();
            label2.Text = label2.Text.Replace("%osver%", name);

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            label2.Text = label2.Text.Replace("%username%", userName);

            string sctVer = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            label2.Text = label2.Text.Replace("%ver%", sctVer);
        }

        private void About_Shown(object sender, EventArgs e)
        {
            UxTheme.SetWindowTheme(Handle, " ", " ");
        }
    }
}
