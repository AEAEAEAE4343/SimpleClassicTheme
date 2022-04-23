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
 *
 *  Multi-user Classic Theme
 *  Copyright (C) 2022 Anis Errais (Leet)
 *  This code is part of the Simple Classic Theme project, which can be
 *  found here: https://github.com/WinClassic/SimpleClassicTheme
 *
 *  Credits to the following resources:
 *  Creating a Win32 Service: https://www.codeproject.com/Articles/499465/Simple-Windows-Service-in-Cplusplus
 *  Creating a named pipe: https://docs.microsoft.com/en-us/windows/win32/ipc/named-pipe-server-using-overlapped-i-o
 *
 *  This file contains .NET versions of the API definitions exported
 *  by MCTapi.dll. It also contains extra enums to aid in development.
 *
 */

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MCT.NET
{
    public static class MctApi
    {
        [DllImport("Kernel32.dll")]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        private delegate int GetMCTRevisionDelegate();

        public enum MctErrorSource : byte
        {
            /// <summary>
            /// The error didn't have any source. This means no error occured.
            /// </summary>
            None = 0,

            /// <summary>
            /// The error occured within MCT itself, you should cast it to a <typeparamref name="MctError"/>.
            /// </summary>
            Mct = 1,

            /// <summary>
            /// The error occured within Win32. Use <code>new Win32Exception(errorCode).Message</code> for a detailed description of the error.
            /// </summary>
            Win32 = 2,

            /// <summary>
            /// The error happened in low-level system API's. 
            /// </summary>
            Ntdll = 3,
        }

        public enum MctError : uint
        {
            /// <summary>
            /// The request was parsed and executed successfully
            /// </summary>
            Success = 0,

            /// <summary>
            /// The provided login session ID was invalid
            /// </summary>
            SessionIdInvalid = 1,

            /// <summary>
            /// The MCT service is not running or inavailable
            /// </summary>
            ServiceNotRunning = 2,

            /// <summary>
            /// This version of the MCT API (MCTapi.dll) is too old for the installed MCT version
            /// </summary>
            ApiTooOld = 0xFFFFFFFD,

            /// <summary>
            /// This version of the MCT API (MCTapi.dll) is too new for the installed MCT version
            /// </summary>
            ApiTooNew = 0xFFFFFFFE,

            /// <summary>
            /// Internal error: The request was invalid
            /// </summary>
            InvalidRequest = 0xFFFFFFFF,
        }

        public enum MctQueryResult : uint
        {
            /// <summary>
            /// This indicates that Classic Theme is disabled for the given session ID
            /// </summary>
            ClassicThemeDisabled = 0x80000000,

            /// <summary>
            /// This indicates that Classic Theme is enabled for the given session ID
            /// </summary>
            ClassicThemeEnabled = 0x80000001,
        }

        public enum MctRevision : int
        {
            /// <summary>
            /// This indicates that MCT is not installed on the system
            /// </summary>
            NotInstalled = -1,

            /// <summary>
            /// This indicates that MCT was found, but that MCTapi could not be loaded
            /// </summary>
            LoadFailed = -2,

            /// <summary>
            /// This indicates that MCT has been loaded, but that the MCTapi library that has been loaded is not of a known format
            /// </summary>
            InvalidLibrary = -3,

            /// <summary>
            /// This value is not used
            /// </summary>
            InvalidRevision = 0,

            /// <summary>
            /// Indicates that the MCT revision of the loaded MCTapi library is equal to MCT_REV_1
            /// </summary>
            MctRevision1 = 1
        }

        /// <summary>
        /// A structure for storing error information returned from MCTapi functions
        /// </summary>
        
        [StructLayout(LayoutKind.Sequential)]
        public struct MctErrorCode
        {
            /// <summary>
            /// Specifies where the error originated from.
            /// </summary>
            public MctErrorSource ErrorSource;

            /// <summary>
            /// The result of the function. If <c>Success</c> is <c>false</c>, this variable contains the error code retrieved from <c>ErrorSource</c>.
            /// </summary>
            public uint Result;

            /// <summary>
            /// Specifies whether an error occured executing the function.
            /// </summary>
            public bool Success => ErrorSource == MctErrorSource.None;
        };

        /// <summary>
        /// Finds MCT and the correct API library and loads them into the current process.
        /// </summary>
        /// <returns>An MctRevision specifying the revision number of the loaded MCTapi, or an error code if the loading failed.</returns>
        public static MctRevision InitializeAPI()
        {
            string path = Environment.GetEnvironmentVariable("programfiles") + "\\MCT\\MCTapi.dll";
            if (!File.Exists(path))
                return MctRevision.NotInstalled;

            IntPtr hModule = LoadLibrary(path);
            if (hModule == IntPtr.Zero)
                return MctRevision.LoadFailed;

            IntPtr functionAddress = GetProcAddress(hModule, "GetMCTRevision");
            if (functionAddress == IntPtr.Zero)
                return MctRevision.InvalidLibrary;

            GetMCTRevisionDelegate function = (GetMCTRevisionDelegate)Marshal.GetDelegateForFunctionPointer(functionAddress, typeof(GetMCTRevisionDelegate));
            return (MctRevision)function();
        }

        /// <summary>
        /// Enables Classic Theme in the specified session.
        /// </summary>
        /// <param name="sessionId">A Remote Desktop Services (formerly Windows Terminal Services) session ID. This can be retrieved using <c>Process.SessionId</c>.</param>
        /// <param name="mctError">A reference to a <typeparamref name="MctErrorCode"/> structure. This structure will be filled with error information.</param>
        /// <returns>A <typeparamref name="Boolean"/> value representing if the function succeeded.</returns>
        [DllImport("MCTapi.dll")]
        public static extern bool EnableClassicTheme(ulong sessionId, ref MctErrorCode mctError);

        /// <summary>
        /// Disables Classic Theme in the specified session.
        /// </summary>
        /// <param name="sessionId">A Remote Desktop Services (formerly Windows Terminal Services) session ID. This can be retrieved using <c>Process.SessionId</c>.</param>
        /// <param name="mctError">A reference to a <typeparamref name="MctErrorCode"/> structure. This structure will be filled with error information.</param>
        /// <returns>A <typeparamref name="Boolean"/> value representing if the function succeeded.</returns>
        [DllImport("MCTapi.dll")]
        public static extern int DisableClassicTheme(ulong sessionId, ref MctErrorCode mctError);

        /// <summary>
        /// Queries if Classic Theme is enabled in the specified session.
        /// </summary>
        /// <param name="sessionId">A Remote Desktop Services (formerly Windows Terminal Services) session ID. This can be retrieved using <c>Process.SessionId</c>.</param>
        /// <param name="mctError">A reference to a <typeparamref name="MctErrorCode"/> structure. This structure will be filled with error information.</param>
        /// <returns>A <typeparamref name="Boolean"/> value representing if the function succeeded. If the function succeeded, <c><paramref name="mctError"/>.Result</c> will be of type <typeparamref name="MctQueryResult"/>.</returns>
        [DllImport("MCTapi.dll")]
        public static extern int QueryClassicTheme(ulong sessionId, ref int enabled, ref MctErrorCode mctError);

        /// <summary>
        /// Destroys the theme secion in the specified session, permanently enabling Classic Theme until the session ends.
        /// </summary>
        /// <param name="sessionId">A Remote Desktop Services (formerly Windows Terminal Services) session ID. This can be retrieved using <c>Process.SessionId</c>.</param>
        /// <param name="mctError">A reference to a <typeparamref name="MctErrorCode"/> structure. This structure will be filled with error information.</param>
        /// <returns>A <typeparamref name="Boolean"/> value representing if the function succeeded.</returns>
        [DllImport("MCTapi.dll")]
        public static extern int KillThemeSection(ulong sessionId, ref MctErrorCode mctError);
    }
}
