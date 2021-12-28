using System;
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
