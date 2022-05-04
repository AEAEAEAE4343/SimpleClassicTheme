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
using System.Linq;
using System.Reflection;
using System.Windows;

namespace SimpleClassicTheme
{
	internal class Configuration
	{
		public bool Enabled
		{
			get => bool.Parse(GetItem("Enabled", false).ToString());
			set => SetItem("Enabled", value.ToString());
		}

		public ClassicTheme.ClassicThemeMethod ClassicThemeMethod
        {
			get => (ClassicTheme.ClassicThemeMethod)Enum.Parse(typeof(ClassicTheme.ClassicThemeMethod), GetItem("ClassicThemeMethod", "MultiUserClassicTheme").ToString());
			set => SetItem("ClassicThemeMethod", value.ToString());
		}

		public int TaskbarDelay
		{
			get => (int)GetItem("TaskbarDelay", 5000);
			set => SetItem("TaskbarDelay", value, RegistryValueKind.DWord);
		}

		public TaskbarType TaskbarType
		{
			get => (TaskbarType)Enum.Parse(typeof(TaskbarType), GetItem("TaskbarType", "RetroBar").ToString());
			set => SetItem("TaskbarType", value.ToString());
		}

		public string InstallPath
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

		public bool BetaUpdates
		{
			get => bool.Parse(GetItem("BetaUpdates", false).ToString());
			set => SetItem("BetaUpdates", value.ToString());
		}

		public UpdateMode UpdateMode
		{
			get => (UpdateMode)Enum.Parse(typeof(UpdateMode), GetItem("UpdateMode", "Automatic").ToString());
			set => SetItem("UpdateMode", value.ToString());
		}

		public Version ConfigVersion
		{
			get => Version.Parse(GetItem("ConfigVersion", Assembly.GetExecutingAssembly().GetName().Version).ToString());
			set => SetItem("ConfigVersion", value.ToString());
		}

		private RegistryKey GetRegistryKey() => Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("Simple Classic Theme").CreateSubKey("Base");

		public object GetItem(string itemName, object defaultValue)
		{
			RegistryKey key = GetRegistryKey();
			object returnValue = key.GetValue(itemName, defaultValue);
			key.Close();
			return returnValue;
		}

		public void SetItem(string itemName, object newValue, RegistryValueKind valueKind = RegistryValueKind.String)
		{
			RegistryKey key = GetRegistryKey();
			key.SetValue(itemName, newValue, valueKind);
			key.Close();
		}

		public Configuration()
		{
			UpdateConfig();
		}

        private void UpdateConfig()
        {
			if (Registry.CurrentUser.CreateSubKey("SOFTWARE").GetSubKeyNames().Contains("SimpleClassicTheme"))
			{
				RegistryKey source = Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("SimpleClassicTheme");
				RegistryKey dest = GetRegistryKey();
				RegistryExtensions.RecurseCopyKey(source, dest);
				RegistryKey software = Registry.CurrentUser.CreateSubKey("SOFTWARE");
				software.DeleteSubKey("SimpleClassicTheme", false);
			}
			else if (Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme").GetValueNames().Length > 0)
			{
				RegistryKey source = Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("SimpleClassicTheme");
				RegistryKey dest = GetRegistryKey();
				RegistryExtensions.RecurseCopyKey(source, dest);
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
							TaskbarType = TaskbarType.None;
							break;
						case "SCTT":
							TaskbarType = TaskbarType.SimpleClassicThemeTaskbar;
							break;
					}
				}

				ConfigVersion = new Version(1, 5, 0);
			}

			// 1.5.4 or lower -> 1.6.0
			//if (ConfigVersion.CompareString("1.6.0") < 0)
			//{
				//ShowWizard = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\1337ftw\Simple Classic Theme\Base", "EnableTaskbar", "NO") == "NO";
				//ConfigVersion = new Version(1, 6, 0);
			//}

			// Config version 1.6.0 to 1.7.0
			if (ConfigVersion.CompareString("1.7.0") < 0)
			{
				MessageBox.Show("You are migrating from SCT 1.6 or older. In the past SCT interfaced with system API's directly to enable Classic Theme. To circumvent the security and application restrictions of this method, starting from SCT 1.7 a new system service called MCT is used. Your setup will keep using the old method, but upgrading to the new one is recommended. This can be done through the options menu.", "Simple Classic Theme", MessageBoxButton.OK, MessageBoxImage.Warning);
				ClassicThemeMethod = ClassicTheme.ClassicThemeMethod.SingleUserSCT;

				string oldUpdateMode = (string)GetItem("UpdateMode", "Automatic");
				switch (oldUpdateMode)
                {
					default:
					case "Automatic":
						UpdateMode = UpdateMode.Automatic;
						break;
					case "Ask on startup":
						UpdateMode = UpdateMode.AskOnStartup;
						break;
					case "Manual":
						UpdateMode = UpdateMode.Manual;
						break;
                }

				bool enableTaskbar = Boolean.Parse(GetItem("EnableTaskbar", "True").ToString());
				if (!enableTaskbar)
					TaskbarType = TaskbarType.None;

				ConfigVersion = new Version(1, 7, 0);
			}

			ConfigVersion = Assembly.GetExecutingAssembly().GetName().Version;
		}
    }
}
