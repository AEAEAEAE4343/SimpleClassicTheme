using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassicTheme
{
	internal static class ExplorerPatcher
	{
		public static bool Enabled => IsInstalled;
		public static bool IsInstalled => File.Exists(ApplicationEntryPoint.windir + "\\dxgi.dll");

		public static bool Install()
		{
			try
			{
				new GithubDownloader(GithubDownloader.DownloadableGithubProject.RetroBar).ShowDialog();
				File.Copy($"{SimpleClassicTheme.Configuration.InstallPath}ExplorerPatcher\\dxgi.dll", ApplicationEntryPoint.windir + "\\dxgi.dll");
				Process.Start("cmd", "/c taskkill /im explorer.exe /f").WaitForExit();
				Process.Start("explorer.exe", @ApplicationEntryPoint.windir + @"\Windows\explorer.exe");
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool Uninstall()
		{
			File.Delete(ApplicationEntryPoint.windir + "\\dxgi.dll");
			return true;
		}

		public static void ApplyConfiguration(bool restartExplorer = false)
		{
			Configuration.ConfigurationWhileEnabledOrDisabled = SimpleClassicTheme.Configuration.Enabled;

			var CLSIDReg = Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("Classes").CreateSubKey("CLSID");
			var CurrentVersionReg = Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion");
			var ExplorerPatcherReg = CurrentVersionReg.CreateSubKey("Explorer").CreateSubKey("ExplorerPatcher");

			// ClassicThemeMitigations -> SimpleClassicTheme.Configuration.Enabled;
			// Boolean      True    False
			// REG_DWORD    0x1     0x0
			// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\ExplorerPatcher\ClassicThemeMitigations
			ExplorerPatcherReg.SetValue("ClassicThemeMitigations", SimpleClassicTheme.Configuration.Enabled ? 1 : 0);

			// FileExplorerLegacyContextMenu
			// Boolean      True    False
			// REG_SZ       Blank   Delete InprocServer32
			// REG_DWORD    0x0     0x1
			// HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32\(Default)
			// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\ExplorerPatcher\DisableImmersiveContextMenu
			if (Configuration.FileExplorerLegacyContextMenu)
			{
				CLSIDReg.CreateSubKey("{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}").CreateSubKey("InprocServer32").SetValue("", "");
				ExplorerPatcherReg.SetValue("DisableImmersiveContextMenu", 0);
			}
			else
			{
				CLSIDReg.CreateSubKey("{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}").DeleteSubKeyTree("InprocServer32", false);
				ExplorerPatcherReg.SetValue("DisableImmersiveContextMenu", 1);
			}

			// FileExplorerLegacyRibbon
			// Boolean        True   False
			// REG_SZ         Blank  Delete InprocServer32
			// HKEY_CURRENT_USER\Software\Classes\CLSID\{d93ed569-3b3e-4bff-8355-3c44f6a52bb5}\InprocServer32\(Default)
			if (Configuration.FileExplorerLegacyRibbon)
				CLSIDReg.CreateSubKey("{d93ed569-3b3e-4bff-8355-3c44f6a52bb5}").CreateSubKey("InprocServer32").SetValue("", "");
			else
				CLSIDReg.CreateSubKey("{d93ed569-3b3e-4bff-8355-3c44f6a52bb5}").DeleteSubKeyTree("InprocServer32", false);

			// FileExplorerSearchMode
			// Int32/Enum   0               1                                       2
			// REG_SZ       Delete TreatAs  {64bc32b5-4eec-4de7-972d-bd8bd0324537}  Delete TreatAs
			// REG_DWORD    0x0             0x0                                     0x1
			// HKEY_CURRENT_USER\Software\Classes\CLSID\{1d64637d-31e9-4b06-9124-e83fb178ac6e}\TreatAs\(Default)
			// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\ExplorerPatcher\HideExplorerSearchBar
			switch (Configuration.FileExplorerSearchMode)
			{
				case Configuration.ExplorerPatcherSearchMode.Disabled:
					CLSIDReg.CreateSubKey("{1d64637d-31e9-4b06-9124-e83fb178ac6e}").DeleteSubKeyTree("TreatAs", false);
					ExplorerPatcherReg.SetValue("HideExplorerSearchBar", 0);
					break;
				case Configuration.ExplorerPatcherSearchMode.Legacy:
					CLSIDReg.CreateSubKey("{1d64637d-31e9-4b06-9124-e83fb178ac6e}").CreateSubKey("TreatAs").SetValue("", "{64bc32b5-4eec-4de7-972d-bd8bd0324537}");
					ExplorerPatcherReg.SetValue("DisableImmersiveContextMenu", 0);
					break;
				case Configuration.ExplorerPatcherSearchMode.Normal:
				default:
					CLSIDReg.CreateSubKey("{1d64637d-31e9-4b06-9124-e83fb178ac6e}").DeleteSubKeyTree("TreatAs", false);
					ExplorerPatcherReg.SetValue("HideExplorerSearchBar", 0);
					break;
			}

			// FileExplorerNoNavigationBar
			// Boolean      True    False
			// REG_SZ       Blank   Delete InprocServer32
			// HKEY_CURRENT_USER\Software\Classes\CLSID\{056440fd-8568-48e7-a632-72157243b55b}\InprocServer32\(Default)
			if (Configuration.FileExplorerNoNavigationBar)
				CLSIDReg.CreateSubKey("{056440fd-8568-48e7-a632-72157243b55b}").CreateSubKey("InprocServer32").SetValue("", "");
			else
				CLSIDReg.CreateSubKey("{056440fd-8568-48e7-a632-72157243b55b}").DeleteSubKeyTree("InprocServer32", false);

			// TaskbarWindows10Taskbar
			// Boolean      True    False
			// REG_DWORD    0x1     0x0
			// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\ExplorerPatcher\OldTaskbar
			ExplorerPatcherReg.SetValue("OldTaskbar", Configuration.TaskbarWindows10Taskbar ? 1 : 0);

			// TaskbarLegacyClockFlyout
			// Boolean      True    False
			// REG_DWORD    0x1     0x2
			// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell\UseWin32TrayClockExperience
			CurrentVersionReg.CreateSubKey("ImmersiveShell").SetValue("UseWin32TrayClockExperience", Configuration.TaskbarLegacyClockFlyout ? 1 : 2);

			// TaskbarLegacyVolumeFlyout
			// Boolean      True    False
			// REG_DWORD    0x1     0x0
			// HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\MTCUVC\EnableMtcUvc
			CurrentVersionReg.CreateSubKey("MTCUVC").SetValue("EnableMtcUvc", Configuration.TaskbarLegacyVolumeFlyout ? 1 : 0);

			// Disabled override
			if (!Configuration.Enabled)
			{
				ExplorerPatcherReg.SetValue("ClassicThemeMitigations", 0);

				CLSIDReg.CreateSubKey("{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}").DeleteSubKeyTree("InprocServer32", false);
				ExplorerPatcherReg.SetValue("DisableImmersiveContextMenu", 1);
				CLSIDReg.CreateSubKey("{d93ed569-3b3e-4bff-8355-3c44f6a52bb5}").DeleteSubKeyTree("InprocServer32", false);
				CLSIDReg.CreateSubKey("{1d64637d-31e9-4b06-9124-e83fb178ac6e}").DeleteSubKeyTree("TreatAs", false);
				ExplorerPatcherReg.SetValue("HideExplorerSearchBar", 0);
				CLSIDReg.CreateSubKey("{056440fd-8568-48e7-a632-72157243b55b}").DeleteSubKeyTree("InprocServer32", false);
				ExplorerPatcherReg.SetValue("OldTaskbar", 0);
				CurrentVersionReg.CreateSubKey("ImmersiveShell").SetValue("UseWin32TrayClockExperience", 2);
				CurrentVersionReg.CreateSubKey("MTCUVC").SetValue("EnableMtcUvc", 0);
			}

			if (restartExplorer)
				Process.Start(new ProcessStartInfo() { FileName = ApplicationEntryPoint.windir + "\\System32\\rundll32.exe", Arguments = ApplicationEntryPoint.windir + "\\dxgi.dll,ZZRestartExplorer", Verb = "runas" }).WaitForExit();
		}

		internal static class Configuration
		{
			public enum ExplorerPatcherSearchMode
			{
				Normal = 0,
				Legacy = 1,
				Disabled = 2
			}

			public static bool ConfigurationApplying { get; set; } = true;
			public static bool ConfigurationWhileEnabledOrDisabled { get; set; } = true;

			public static bool Enabled
			{
				get => bool.Parse((string)GetItem("Enabled", true.ToString()));
				set => SetItem("Enabled", value.ToString());
			}

			public static bool FileExplorerLegacyContextMenu
			{
				get => bool.Parse((string)GetItem("FileExplorerLegacyContextMenu", ConfigurationWhileEnabledOrDisabled.ToString()));
				set => SetItem("FileExplorerLegacyContextMenu", value.ToString());
			}

			public static bool FileExplorerLegacyRibbon
			{
				get => bool.Parse((string)GetItem("FileExplorerLegacyRibbon", ConfigurationWhileEnabledOrDisabled.ToString()));
				set => SetItem("FileExplorerLegacyRibbon", value.ToString());
			}

			public static ExplorerPatcherSearchMode FileExplorerSearchMode
			{
				get => (ExplorerPatcherSearchMode)Enum.Parse(typeof(ExplorerPatcherSearchMode), (string)GetItem("FileExplorerSearchMode", ExplorerPatcherSearchMode.Normal.ToString()));
				set => SetItem("FileExplorerSearchMode", value.ToString());
			}

			public static bool FileExplorerNoNavigationBar
			{
				get => bool.Parse((string)GetItem("FileExplorerNoNavigationBar", false.ToString()));
				set => SetItem("FileExplorerNoNavigationBar", value.ToString());
			}

			public static bool TaskbarWindows10Taskbar
			{
				get 
				{
					if (SimpleClassicTheme.Configuration.TaskbarType == TaskbarType.ExplorerPatcher
						&& ConfigurationWhileEnabledOrDisabled) 
						return true;
					return bool.Parse((string)GetItem("TaskbarWindows10Taskbar", ConfigurationWhileEnabledOrDisabled.ToString())); 
				}
				set => SetItem("TaskbarWindows10Taskbar", value.ToString());
			}

			public static bool TaskbarLegacyClockFlyout
			{
				get => bool.Parse((string)GetItem("TaskbarLegacyClockFlyout", ConfigurationWhileEnabledOrDisabled.ToString()));
				set => SetItem("TaskbarLegacyClockFlyout", value.ToString());
			}

			public static bool TaskbarLegacyVolumeFlyout
			{
				get => bool.Parse((string)GetItem("TaskbarLegacyVolumeFlyout", ConfigurationWhileEnabledOrDisabled.ToString()));
				set => SetItem("TaskbarLegacyVolumeFlyout", value.ToString());
			}

			public static RegistryKey GetRegistryKey() => Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("Simple Classic Theme").CreateSubKey("Base").CreateSubKey("ExplorerPatcher").CreateSubKey(ConfigurationWhileEnabledOrDisabled ? "Enabled" : "Disabled");
			private static object GetItem(string itemName, object defaultValue)
			{
				RegistryKey key = GetRegistryKey();
				object returnValue = key.GetValue(itemName, defaultValue);
				key.Close();
				return returnValue;
			}
			private static void SetItem(string itemName, object newValue, RegistryValueKind valueKind = RegistryValueKind.String)
			{
				RegistryKey key = GetRegistryKey();
				key.SetValue(itemName, newValue, valueKind);
				key.Close();
			}
		}
	}
}
