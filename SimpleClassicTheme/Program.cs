using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;

namespace SimpleClassicTheme
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);
            if (!hasAdministrativeRight)
            {
                if (MessageBox.Show("This application requires admin privilages.\nClick Ok to elevate or Cancel to exit.", "Elevate?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo
                    {
                        Verb = "runas",
                        FileName = Application.ExecutablePath
                    };
                    Process.Start(processInfo);
                    Environment.Exit(0);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            if (Assembly.GetExecutingAssembly().Location == @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\Simple Classic Theme.exe")
            {
                string cmdlet = "Set-NtSecurityDescriptor -path \\\"\\Sessions\\$([System.Diagnostics.Process]::GetCurrentProcess().SessionId)\\Windows\\ThemeSection\\\" \\\"O:BAG:SYD:(A;;RC;;;IU)(A;;DCSWRPSDRCWDWO;;;SY)\\\" Dacl";
                Process pwsh = new Process()
                {
                    StartInfo =
                {
                    FileName = @"C:\Program Files\PowerShell\6\\pwsh.exe",
                    Arguments = "-c " + cmdlet,
                    Verb = "runas"
                }
                };
                pwsh.Start();
                Environment.Exit(0);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
