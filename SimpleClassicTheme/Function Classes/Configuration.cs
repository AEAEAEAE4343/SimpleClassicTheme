using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassicTheme
{
	static class Configuration
	{
		public static RegistryKey GetRegistryKey() => Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("1337ftw").CreateSubKey("SimpleClassicTheme");

		public static object GetItem(string itemName, object defaultValue)
		{
			RegistryKey key = GetRegistryKey();
			object returnValue = key.GetValue(itemName, defaultValue);
			key.Close();
			return returnValue;
		}

		public static void SetItem(string itemName, object newValue, RegistryValueKind valueKind)
		{
			RegistryKey key = GetRegistryKey();
			key.SetValue(itemName, newValue, valueKind);
			key.Close();
		}

		public static void MigrateOldSCTRegistry()
		{
			RegistryKey source = Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("SimpleClassicTheme");
			RegistryKey dest = GetRegistryKey();
			ExtraFunctions.RecurseCopyKey(source, dest);
			source.Close();
			dest.Close();
			RegistryKey software = Registry.CurrentUser.CreateSubKey("SOFTWARE");
			software.DeleteSubKey("SimpleClassicTheme", false);
			software.Close();
		}
	}
}
