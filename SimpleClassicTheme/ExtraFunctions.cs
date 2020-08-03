using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SimpleClassicTheme
{
    //Helper Class: Contains all longish functions that make code unreadable
    static class ExtraFunctions
    {
        //Check if there's a network connection
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch { return false; }
        }

        //Updates the application
        internal static void Update()
        {
            //Check if there is an internet connection
            if (CheckForInternetConnection())
            {
                Application.Run(new Updater());
            }
        }

        //Updates the startup executable
        internal static void UpdateStartupExecutable(bool createIfNot)
        {
            if (File.Exists(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe"))
                File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe");
            if (File.Exists(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\Simple Classic Theme.exe"))
                File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\Simple Classic Theme.exe");
            if (createIfNot)
            {
                if (Assembly.GetExecutingAssembly().Location != @"C:\SCT\SCT.exe")
                {
                    File.Delete(@"C:\SCT\SCT.exe");
                    File.Copy(Assembly.GetExecutingAssembly().Location, @"C:\SCT\SCT.exe");
                }
                File.WriteAllText("C:\\SCT\\SCTTask.xml", Properties.Resources.scttask);
                File.WriteAllText("C:\\SCT\\TaskSchedule.cmd", Properties.Resources.taskScheduleCommands);
                Process task = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "C:\\SCT\\TaskSchedule.cmd",
                        Verb = "runas",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                task.Start();
                task.WaitForExit();
                File.SetAttributes(@"C:\SCT\SCT.exe", File.GetAttributes(Assembly.GetExecutingAssembly().Location) | FileAttributes.Hidden);
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                Directory.CreateDirectory(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\");
                IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.lnk");
                shortcut.Description = "SCT";
                shortcut.TargetPath = @"C:\SCT\SCT.exe";
                shortcut.Save();
            }
            else if (Assembly.GetExecutingAssembly().Location != @"C:\SCT\SCT.exe" && CheckMD5(@"C:\SCT\SCT.exe") != CheckMD5(Assembly.GetExecutingAssembly().Location))
			{
                File.Delete(@"C:\SCT\SCT.exe");
                File.Copy(Assembly.GetExecutingAssembly().Location, @"C:\SCT\SCT.exe");
            }
        }

        //Renames a subkey
        public static bool RenameSubKey(RegistryKey parentKey,
            string subKeyName, string newSubKeyName)
        {
            CopyKey(parentKey, subKeyName, newSubKeyName);
            parentKey.DeleteSubKeyTree(subKeyName);
            return true;
        }

        //Copies a key
        public static bool CopyKey(RegistryKey parentKey,
            string keyNameToCopy, string newKeyName)
        {
            RegistryKey destinationKey = parentKey.CreateSubKey(newKeyName);
            RegistryKey sourceKey = parentKey.OpenSubKey(keyNameToCopy);
            RecurseCopyKey(sourceKey, destinationKey);
            return true;
        }

        //Recursively copies a key
        private static void RecurseCopyKey(RegistryKey sourceKey, RegistryKey destinationKey)
        {
            foreach (string valueName in sourceKey.GetValueNames())
            {
                object objValue = sourceKey.GetValue(valueName);
                RegistryValueKind valKind = sourceKey.GetValueKind(valueName);
                destinationKey.SetValue(valueName, objValue, valKind);
            }
            foreach (string sourceSubKeyName in sourceKey.GetSubKeyNames())
            {
                RegistryKey sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName);
                RegistryKey destSubKey = destinationKey.CreateSubKey(sourceSubKeyName);
                RecurseCopyKey(sourceSubKey, destSubKey);
            }
        }

        //Gets the MD5 checksum of a file
        public static string CheckMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return Encoding.Default.GetString(md5.ComputeHash(stream));
                }
            }
        }

        public static void ReConfigureOS(bool os, bool sib)
		{
            if (os)
            {
                //Get user folder
                string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                    path = Directory.GetParent(path).ToString();

                //Prepare files for Open-Shell
                Directory.CreateDirectory(path + "\\AppData\\Local\\StartIsBack\\Orbs");
                Properties.Resources.win7.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\win7.png");
                Properties.Resources.win9x.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\win9x.png");

                //Find out what start orb the user wants
                string orbname = MessageBox.Show("Do you want to use a Win95 style start orb (If not a Windows 7 style orb will be used)?", "Simple Classic Theme", MessageBoxButtons.YesNo) == DialogResult.Yes ? "win9x.png" : "win7.png";

                //Setup Open-Shell registry
                File.WriteAllText(Path.Combine(Path.GetTempPath(), "\\ossettings.reg"), Properties.Resources.openShellSettings);
                Process.Start(Path.Combine(Path.GetTempPath(), "\\ossettings.reg")).WaitForExit();
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\OpenShell\\StartMenu\\Settings", "StartButtonPath", @"%USERPROFILE%\AppData\Local\StartIsBack\Orbs\" + orbname);
            }
            if (sib)
			{
                //Get user folder
                string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                    path = Directory.GetParent(path).ToString();

                //Prepare files for StartIsBack
                Directory.CreateDirectory(path + "\\AppData\\Local\\StartIsBack\\Orbs");
                Directory.CreateDirectory(path + "\\AppData\\Local\\StartIsBack\\Styles");
                Properties.Resources.win7.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\win7.png");
                Properties.Resources.win9x.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\win9x.png");
                Properties.Resources.taskbar.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\taskbar.png");
                Properties.Resources.null_classic3small.Save(path + "\\AppData\\Local\\StartIsBack\\Orbs\\null_classic3big.bmp");
                File.WriteAllBytes(path + "\\AppData\\Local\\StartIsBack\\Styles\\Classic3.msstyles", Properties.Resources.classicStartIsBackTheme);

                //Setup StartIsBack registry
                string f = Properties.Resources.startIsBackSettings.Replace("C:\\\\Users\\\\{Username}", $"{path.Replace("\\", "\\\\")}");
                File.WriteAllText("C:\\sib.reg", f);
                Process.Start(Path.Combine(Path.GetTempPath(), "\\sib.reg")).WaitForExit();

                //Disable StartIsBack
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\StartIsBack", "Disabled", 1);
                File.Delete("C:\\ossettings.reg");
                File.Delete("C:\\sib.reg");
                File.Delete("C:\\sib.exe");
            }
        }
    }
}
