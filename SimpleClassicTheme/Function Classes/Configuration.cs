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

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassicTheme
{
	internal static class Configuration
	{
		public static bool Enabled
		{
			get => bool.Parse((string)GetItem("Enabled", false.ToString()));
			set => SetItem("Enabled", value.ToString());
		}

		public static bool EnableTaskbar
		{
			get => bool.Parse((string)GetItem("EnableTaskbar", false.ToString()));
			set => SetItem("EnableTaskbar", value.ToString());
		}

		public static int TaskbarDelay
		{
			get => (int)GetItem("TaskbarDelay", 5000);
			set => SetItem("TaskbarDelay", value, RegistryValueKind.DWord);
		}

		public static bool ShowWizard
		{
			get => bool.Parse((string)GetItem("ShowWizard", true.ToString()));
			set => SetItem("ShowWizard", value.ToString());
		}

		public static bool BetaUpdates
		{
			get => bool.Parse((string)GetItem("BetaUpdates", false.ToString()));
			set => SetItem("BetaUpdates", value.ToString());
		}

		/*public static string TaskbarType
		{
			get => (string)GetItem("TaskbarType", "OS+SiB");
			set => SetItem("TaskbarType", value);
		}*/

		public static TaskbarType TaskbarType
		{
			get => (TaskbarType)Enum.Parse(typeof(TaskbarType), (string)GetItem("TaskbarType", "SimpleClassicThemeTaskbar"));
			set => SetItem("TaskbarType", value.ToString());
		}

		public static string UpdateMode
		{
			get => (string)GetItem("UpdateMode", "Automatic");
			set => SetItem("UpdateMode", value);
		}

		public static Version ConfigVersion
		{
			get => Version.Parse((string)GetItem("ConfigVersion", Assembly.GetExecutingAssembly().GetName().Version));
			set => SetItem("ConfigVersion", value.ToString());
		}

		public static string InstallPath
        {
			get
            {
				string path = (string)GetItem("InstallPath", "C:\\SCT\\");
				if (!path.EndsWith("\\"))
                {
					path += "\\";
					InstallPath = path;
                }
				return path;
			}
			set => SetItem("InstallPath", value);
		}

		public static RegistryKey GetRegistryKey() => Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("Simple Classic Theme").CreateSubKey("Base");

		private static object GetItem(string itemName, object defaultValue)
		{
			RegistryKey key = GetRegistryKey();
			object returnValue = key.GetValue(itemName, defaultValue);
			key.Close();
			return returnValue;
		}

		public static void SetItemManually(string itemName, object newValue, RegistryValueKind valueKind = RegistryValueKind.String)
			=> SetItem(itemName, newValue, valueKind);
		private static void SetItem(string itemName, object newValue, RegistryValueKind valueKind = RegistryValueKind.String)
		{
			RegistryKey key = GetRegistryKey();
			key.SetValue(itemName, newValue, valueKind);
			key.Close();
		}

		public static void MigrateOldSCTRegistry()
		{
			if (Registry.CurrentUser.CreateSubKey("SOFTWARE").GetSubKeyNames().Contains("SimpleClassicTheme"))
			{
				RegistryKey source = Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("SimpleClassicTheme");
				RegistryKey dest = GetRegistryKey();
				ExtraFunctions.RecurseCopyKey(source, dest);
				RegistryKey software = Registry.CurrentUser.CreateSubKey("SOFTWARE");
				software.DeleteSubKey("SimpleClassicTheme", false);
			}
			else if (Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme").GetValueNames().Length > 0)
			{
				RegistryKey source = Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("SimpleClassicTheme");
				RegistryKey dest = GetRegistryKey();
				ExtraFunctions.RecurseCopyKey(source, dest);
				foreach (string value in source.GetValueNames())
					source.DeleteValue(value);
				Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").DeleteSubKey("SimpleClassicTheme");
			} 

			// Starting from 1.5.0, SCT will track a config version. If any critical changes have been made to the config
			// SCT will automatically apply those changes starting from changes past the original config version.
			// Eg. When updating from 1.4 to 1.6 the config will be changed like this: 1.4.0 -> 1.5.0 -> 1.6.0

			// 1.4.0 or lower -> 1.5.0
			if (ConfigVersion.CompareString("1.4.9") < 0)
			{
				// TaskbarType was changed in to an enum
				string oldValue = (string)GetItem("TaskbarType", "NoValue");
				if (oldValue != "NoValue")
				{
					switch (oldValue)
					{
						case "SiB+OS":
							TaskbarType = Environment.OSVersion.Version.CompareTo("6.3") > 0 ? TaskbarType.StartIsBackOpenShell : TaskbarType.Windows81Vanilla;
							break;
						case "SCTT":
							TaskbarType = TaskbarType.SimpleClassicThemeTaskbar;
							break;
					}
				}

				ConfigVersion = new Version(1, 5, 0);
			}

			// 1.5.4 or lower -> 1.6.0
			if (ConfigVersion.CompareString("1.6.0") < 0)
			{
				ShowWizard = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\1337ftw\Simple Classic Theme\Base", "EnableTaskbar", "NO") == "NO";
			}

			ConfigVersion = Assembly.GetExecutingAssembly().GetName().Version;
		}
	}
}
