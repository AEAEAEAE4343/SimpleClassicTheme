using System;
using System.IO;

namespace SimpleClassicTheme
{
    public enum UILevel
    {
        Standard,
        LogWarningsAndErrors,
        Silent,
    }

    public static class Logger
    {
        public static UILevel UILevel { get; internal set; } = UILevel.Standard;

        public static void WriteLog(string level, string message)
        {
            File.AppendAllText($"{SCT.Configuration.InstallPath}latest.log", $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss,fff}] [{level,-8}] {message}");
        }

        public static void DebugMessage(string message)
        {
            WriteLog("DEBUG", message);  
        }

        public static void WarningMessage(string title, string message)
        {
            switch (UILevel)
            {
                case UILevel.Standard:
                    CommonControls.TaskDialog.Show(message, "Simple Classic Theme", title, icon: CommonControls.TaskDialogIcon.WarningIcon);
                    goto case UILevel.LogWarningsAndErrors;
                case UILevel.LogWarningsAndErrors:
                case UILevel.Silent:
                    WriteLog("WARNING", message);
                    break;
            }
        }

        public static void ErrorMessage(string title, string message)
        {
            switch (UILevel)
            {
                case UILevel.Standard:
                    CommonControls.TaskDialog.Show(message, "Simple Classic Theme", title, icon: CommonControls.TaskDialogIcon.ErrorIcon);
                    goto case UILevel.LogWarningsAndErrors;
                case UILevel.LogWarningsAndErrors:
                case UILevel.Silent:
                    WriteLog("ERROR", message);
                    break;
            }
        }
    }
}