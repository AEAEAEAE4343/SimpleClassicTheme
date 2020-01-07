using Microsoft.Win32;
using System.Diagnostics;
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
            {
                if (CheckMD5(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe") != CheckMD5(Assembly.GetExecutingAssembly().Location))
                {
                    if (MessageBox.Show("You have an old startup executable installed. Would you like to update it?", "Installation Detection", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (File.Exists(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\sct.cmd"))
                            File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\sct.cmd");
                        Directory.CreateDirectory("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\1337ftw\\");
                        File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe");
                        File.Copy(Assembly.GetExecutingAssembly().Location, @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe", true);
                        File.WriteAllText("C:\\TaskSchedule.cmd", Properties.Resources.taskScheduleCommands);
                        Process task = new Process()
                        {
                            StartInfo =  new ProcessStartInfo()
                            {
                                FileName = "C:\\TaskSchedule.cmd",
                                Verb = "runas",
                                UseShellExecute = false,
                                CreateNoWindow = true
                            }
                        };
                        task.Start();
                        task.WaitForExit();
                        File.Delete("C:\\TaskSchedule.cmd");
                        File.SetAttributes(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe", File.GetAttributes(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe") | FileAttributes.Hidden);
                        IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                        IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\1337ftw\\Simple Classic Theme.lnk");
                        shortcut.Description = "SCT";
                        shortcut.TargetPath = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe";
                        shortcut.Save();
                    }
                }
            }
            else if (File.Exists(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\Simple Classic Theme.exe"))
            {
                if (MessageBox.Show("You have an old startup executable installed. Would you like to update it?", "Installation Detection", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Directory.CreateDirectory("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\1337ftw\\");
                    File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\Simple Classic Theme.exe");
                    File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe");
                    File.Copy(Assembly.GetExecutingAssembly().Location, @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe", true);
                    File.WriteAllText("C:\\TaskSchedule.cmd", Properties.Resources.taskScheduleCommands);
                    Process task = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            FileName = "C:\\TaskSchedule.cmd",
                            Verb = "runas",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };
                    task.Start();
                    task.WaitForExit();
                    File.Delete("C:\\TaskSchedule.cmd");
                    File.SetAttributes(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe", File.GetAttributes(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe") | FileAttributes.Hidden);
                    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                    IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\1337ftw\\Simple Classic Theme.lnk");
                    shortcut.Description = "SCT";
                    shortcut.TargetPath = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe";
                    shortcut.Save();
                }
            }
            else if (createIfNot)
            {
                Directory.CreateDirectory("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\1337ftw\\");
                File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe");
                File.Copy(Assembly.GetExecutingAssembly().Location, @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe", true);
                File.WriteAllText("C:\\TaskSchedule.cmd", Properties.Resources.taskScheduleCommands);
                Process task = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "C:\\TaskSchedule.cmd",
                        Verb = "runas",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                task.Start();
                task.WaitForExit();
                File.Delete("C:\\TaskSchedule.cmd");
                File.SetAttributes(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe", File.GetAttributes(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe") | FileAttributes.Hidden);
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\1337ftw\\Simple Classic Theme.lnk");
                shortcut.Description = "SCT";
                shortcut.TargetPath = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\1337ftw\Simple Classic Theme.exe";
                shortcut.Save();
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
    }
}
