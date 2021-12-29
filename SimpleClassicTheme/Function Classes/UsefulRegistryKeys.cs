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

namespace SimpleClassicTheme
{
    internal static class UsefulRegistryKeys
    {
        public static bool Borders3D 
        { 
            get
            {
                RegistryKey hKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop");
                byte[] upm = (byte[])hKey.GetValue("UserPreferencesMask");
                hKey.Close();
                return (upm[2] & 0b10) == 0;
            }
            set
            {
                RegistryKey hKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop");
                byte[] upm = (byte[])hKey.GetValue("UserPreferencesMask");
                upm[2] ^= 0b10;
                hKey.SetValue("UserPreferencesMask", upm);
                hKey.Close();
            }
        }
    }
}
