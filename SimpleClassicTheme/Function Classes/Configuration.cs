/*
 *  SimpleClassicTheme, a basic utility to bring back classic theme to newer versions of the Windows operating system.
 *  Copyright (C) 2021 Anis Errais
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
 *  along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *
 */

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
