using System;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace SimpleClassicTheme
{
    internal static class NtApi
    {
        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        internal static extern uint NtOpenSection(out IntPtr sectionHandle, AccessMask desiredAccess, ref ObjectAttributes objectAttributes);
        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        internal static extern uint NtSetSecurityObject(IntPtr sectionHandle, SecurityInformation desiredAccess, IntPtr securityDescriptor);
        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        internal static extern uint NtClose(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        internal static extern uint ConvertStringSecurityDescriptorToSecurityDescriptor(string StringSecurityDescriptor, uint StringSDRevision, out IntPtr SecurityDescriptor, out UIntPtr SecurityDescriptorSize);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr LocalFree(IntPtr hMem);

        private const int MAX_PATH = 260;
        [StructLayout(LayoutKind.Sequential)]
        internal struct UnicodeString
        {
            public ushort Length;
            public ushort MaxLength;
            public IntPtr Buffer;

            public static UnicodeString Create(string text)
            {
                UnicodeString s = new UnicodeString();
                s.Length = (ushort)(text.Length * 2);
                s.MaxLength = 2 * MAX_PATH;
                s.Buffer = Marshal.AllocHGlobal(s.MaxLength);

                int strSize = Math.Min(s.Length, s.MaxLength - 2);
                Marshal.Copy(Encoding.Unicode.GetBytes(text), 0, s.Buffer, strSize);
                Marshal.Copy(new byte[]{ 0, 0 }, 0, s.Buffer + strSize, 2);
                return s;
            }

            public void Free()
            {
                Marshal.FreeHGlobal(Buffer);
            }
        }

        internal enum AttributesEnum : uint
        {
            OBJ_CASE_INSENSITIVE = 0x40U,
            OBJ_KERNEL_HANDLE = 0x200U,
        }

        internal enum AccessMask : uint
        {
            WRITE_DAC = 0x00040000U,
        }

        internal enum SecurityInformation : uint
        {
            DACL_SECURITY_INFORMATION = 0x4U,
        }

        internal struct ObjectAttributes
        {
            public uint Length;
            public IntPtr RootDirectory;
            [MarshalAs(UnmanagedType.LPStruct)]
            public IntPtr ObjectName;
            public AttributesEnum Attributes;
            public IntPtr SecurityDescriptor;
            public IntPtr SecurityQualityOfService;
        }

        /*internal class NtException
        {
            public uint ErrorCode;

            public string Message
            {
                get
                {
                    return new Win32Exception(ErrorCode).Message;
                }
            }

            public NtException(uint errorCode)
            {
                ErrorCode = errorCode;
            }
        }*/
    }
}