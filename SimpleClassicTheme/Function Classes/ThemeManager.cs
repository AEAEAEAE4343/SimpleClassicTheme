using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

using static SimpleClassicTheme.CommonControls;
using static SimpleClassicTheme.Logger;

namespace SimpleClassicTheme
{
    public static class ThemeManager
    {
        private static bool IsAdministrator => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        private static void RestartExplorer(bool wait = false)
        {
            Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
            Process.Start("explorer.exe", @"C:\Windows\explorer.exe");
            if (wait) Thread.Sleep(SCT.Configuration.TaskbarDelay);
        }

        /// <summary>
        /// Enables Classic Theme and if specified Classic Taskbar.
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully.</returns>
        public static bool EnableSCT(bool taskbar = true)
        {
            if (SCT.Configuration.ClassicThemeMethod == ClassicTheme.ClassicThemeMethod.SingleUserSCT && !IsAdministrator)
            {
                ErrorMessage("You don't have permission to modify the Classic Theme state", "To enable or disable Classic Theme, either run Simple Classic Theme as Administrator, or install MCT to enable and disable Classic Theme freely.");
                return false;
            }

            Process.Start($"{SCT.Configuration.InstallPath}EnableThemeScript.bat", "pre").WaitForExit();
            ClassicTheme.ClassicThemeResult res = ClassicTheme.Enable();
            if (!res.Success)
            {
                TaskDialog.Show(Application.OpenForms[typeof(MainForm).Name], $"{res.GetDescription()}", "Simple Classic Theme", "Failed to enable Classic Theme", TaskDialogButtons.OK, TaskDialogIcon.ErrorIcon);
                return false;
            }

            if (taskbar)
                if (!ClassicTaskbar.EnableCurrent())
                    return false;

            Process.Start($"{SCT.Configuration.InstallPath}EnableThemeScript.bat", "post").WaitForExit();
            SCT.Configuration.Enabled = true;
            return true;
        }

        /// <summary>
        /// Enables Classic Theme and if specified Classic Taskbar.
        /// </summary>
        /// <returns>A Boolean value specifying whether the operation completed succesfully.</returns>
        public static bool DisableSCT(bool taskbar = true)
        {
            if (SCT.Configuration.ClassicThemeMethod == ClassicTheme.ClassicThemeMethod.SingleUserSCT && !IsAdministrator)
            {
                TaskDialog.Show(Application.OpenForms[typeof(MainForm).Name], "To enable or disable Classic Theme, either run Simple Classic Theme as Administrator, or install MCT to enable and disable Classic Theme freely.", "Simple Classic Theme", "You don't have permission to modify the Classic Theme state", TaskDialogButtons.OK, TaskDialogIcon.ErrorIcon);
                return false;
            }

            Process.Start($"{SCT.Configuration.InstallPath}DisableThemeScript.bat", "pre").WaitForExit();
            ClassicTheme.ClassicThemeResult res = ClassicTheme.Disable();
            if (!res.Success)
            {
                TaskDialog.Show(Application.OpenForms[typeof(MainForm).Name], $"{res.GetDescription()}", "Simple Classic Theme", "Failed to disable Classic Theme", TaskDialogButtons.OK, TaskDialogIcon.ErrorIcon);
                return false;
            }

            if (taskbar)
                if (!ClassicTaskbar.DisableCurrent())
                    return false;

            Process.Start($"{SCT.Configuration.InstallPath}DisableThemeScript.bat", "post").WaitForExit();
            SCT.Configuration.Enabled = false;
            return true;
        }
    }
}
