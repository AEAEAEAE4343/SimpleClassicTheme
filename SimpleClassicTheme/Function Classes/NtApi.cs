using System;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace SimpleClassicTheme
{
    internal static class NtApi
    {
        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        internal static extern unsafe uint NtOpenSection(out IntPtr sectionHandle, AccessMask desiredAccess, ref ObjectAttributes objectAttributes);
        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        internal static extern unsafe uint NtSetSecurityObject(IntPtr sectionHandle, SecurityInformation desiredAccess, IntPtr securityDescriptor);
        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        internal static extern unsafe uint NtClose(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        internal static extern unsafe uint ConvertStringSecurityDescriptorToSecurityDescriptor(string StringSecurityDescriptor, uint StringSDRevision, out IntPtr SecurityDescriptor, out UIntPtr SecurityDescriptorSize);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr LocalFree(IntPtr hMem);

        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct UnicodeString
        {
            public ushort Length;
            public ushort MaxLength;
            public byte* Buffer;

            public static UnicodeString Create(string text)
            {
                UnicodeString s = new UnicodeString();
                s.Length = (ushort)(text.Length * 2);
                s.MaxLength = 520;
                s.Buffer = (byte*)Marshal.AllocHGlobal(520);

                int strSize = Math.Min(text.Length * 2, 518);
                Marshal.Copy(Encoding.Unicode.GetBytes(text), 0, (IntPtr)s.Buffer, strSize);
                s.Buffer[strSize] = 0; s.Buffer[strSize + 1] = 0;
                return s;
            }

            public void Free()
            {
                Marshal.FreeHGlobal((IntPtr)Buffer);
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

        internal unsafe struct ObjectAttributes
        {
            public uint Length;
            public IntPtr RootDirectory;
            [MarshalAs(UnmanagedType.LPStruct)]
            public UnicodeString* ObjectName;
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