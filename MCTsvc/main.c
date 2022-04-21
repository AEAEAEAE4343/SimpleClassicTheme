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
 *  SCT as a Service / Multi-user Classic Theme
 *  Copyright (C) 2022 Anis Errais (Leet)
 *  This code is part of the Simple Classic Theme project, which can be
 *  found here: https://github.com/WinClassic/SimpleClassicTheme
 *
 *  Credits to the following resources:
 *  Creating a Win32 Service: https://www.codeproject.com/Articles/499465/Simple-Windows-Service-in-Cplusplus
 *  Creating a named pipe: https://docs.microsoft.com/en-us/windows/win32/ipc/named-pipe-server-using-overlapped-i-o
 *
 *  This file contains the application entry point for MCTsvc.exe.
 *
 */

#include "service.h"
#include <stdio.h>

int main(int argc, char* argv[])
{
    SERVICE_TABLE_ENTRY ServiceTable[] =
    {
        {SERVICE_NAME, (LPSERVICE_MAIN_FUNCTION)ServiceMain},
        {NULL, NULL}
    };

    if (StartServiceCtrlDispatcher(ServiceTable))
        return GetLastError();

    if (argc == 2)
    {
        char destPath[MAX_PATH];
        char executablePath[MAX_PATH];
        char dllSrcPath[MAX_PATH];
        char dllDestPath[MAX_PATH];
        
        strcpy_s(destPath, MAX_PATH, argv[0]);
        char* p = strrchr(destPath, '\\');
        if (p) p[0] = 0;

        SetCurrentDirectoryA(destPath);

        GetFullPathNameA("MCTapi.dll", MAX_PATH, dllSrcPath, NULL);
        GetEnvironmentVariableA("programfiles", destPath, MAX_PATH);
        strcpy_s(executablePath, MAX_PATH, destPath);
        strcpy_s(dllDestPath, MAX_PATH, destPath);

        strcat_s(destPath, MAX_PATH, "\\MCT");
        strcat_s(executablePath, MAX_PATH, "\\MCT\\MCTsvc.exe");
        strcat_s(dllDestPath, MAX_PATH, "\\MCT\\MCTapi.dll");

        if (strcmp(argv[1], "--install") == 0)
        {

            SC_HANDLE sc = OpenSCManager(NULL, NULL, SC_MANAGER_CREATE_SERVICE);
            if (sc == INVALID_HANDLE_VALUE)
            {
                printf("Could not open a handle to the service manager: 0x%8x\n", GetLastError());
                return GetLastError();
            }

            if (!CreateDirectoryA(destPath, NULL) && GetLastError() != ERROR_ALREADY_EXISTS)
            {
                printf("Could not create installation directory: 0x%8x\n", GetLastError());
                return GetLastError();
            }

            if (!CopyFileA(argv[0], executablePath, FALSE))
            {
                printf("Could not create service executable: 0x%8x\n", GetLastError());
                return GetLastError();
            }
            if (!CopyFileA(dllSrcPath, dllDestPath, FALSE))
            {
                printf("Could not create library file: 0x%8x\n", GetLastError());
                return GetLastError();
            }

            SC_HANDLE service = CreateServiceA(
                sc,                                     // Handle to service controller
                "MCTsvc",                               // Service name
                "Multi-user Classic Theme service",     // Display name
                SERVICE_ALL_ACCESS,                     // Service access
                SERVICE_WIN32_OWN_PROCESS,              // Service type
                SERVICE_AUTO_START,                     // Start options
                SERVICE_ERROR_NORMAL,                   // Severity of error handling
                executablePath,                         // Service executable
                NULL,                                   // Service load group
                NULL,                                   // Service tag
                NULL,                                   // Dependencies
                NULL,                                   // Run as user (Defaults to LocalSystem)
                NULL                                    // User password
            );
            if (!service && GetLastError() == ERROR_SERVICE_EXISTS)
                service = OpenServiceA(sc, "MCTsvc", SERVICE_ALL_ACCESS);
            if (!service)
            {
                printf("Could not create or open the MCT service: 0x%8x\n", GetLastError());
                return GetLastError();
            }

            if (!StartServiceA(service, 0, NULL))
            {
                printf("Could not start the MCT service: 0x%8x\n", GetLastError());
                return GetLastError();
            }

            return 0;
        }
        else if (strcmp(argv[1], "--uninstall") == 0)
        {
            DWORD bytesNeeded;

            SC_HANDLE sc = OpenSCManagerA(NULL, NULL, SC_MANAGER_CREATE_SERVICE);
            if (sc == INVALID_HANDLE_VALUE)
            {
                printf("Could not open a handle to the service manager: 0x%8x\n", GetLastError());
                return GetLastError();
            }

            SC_HANDLE service = OpenServiceA(sc, "MCTsvc", SERVICE_ALL_ACCESS);
            if (!service)
            {
                printf("Could not open the MCT service: 0x%8x\n", GetLastError());
                return GetLastError();
            }

            SERVICE_STATUS_PROCESS status;
            if (!ControlService(service, SERVICE_CONTROL_STOP, &status))
            {
                printf("Could not stop the MCT service: 0x%8x\n", GetLastError());
                return GetLastError();
            }

            ULONGLONG ullStartTime = GetTickCount64();
            while (status.dwCurrentState != SERVICE_STOPPED)
            {
                if (GetTickCount64() - ullStartTime > 10000ULL)
                {
                    printf("Timed out waiting for the MCT service to stop\n");
                    return ERROR_TIMEOUT;
                }

                Sleep(status.dwWaitHint);
                if (!QueryServiceStatusEx(
                    service,
                    SC_STATUS_PROCESS_INFO,
                    (LPBYTE)&status,
                    sizeof(SERVICE_STATUS_PROCESS),
                    &bytesNeeded))
                {
                    printf("Could not query service status: 0x%8x\n", GetLastError());
                    return GetLastError();
                }
            }

            if (!DeleteService(service))
            {
                printf("Could not delete the MCT service: 0x%8x\n", GetLastError());
                return GetLastError();
            }

            SHFILEOPSTRUCTA file_op = {
            NULL,
            FO_DELETE,
            destPath,
            NULL,
            FOF_NO_UI,
            NULL,
            0,
            NULL };
            BOOL result = SHFileOperationA(&file_op);
            if (result)
            {
                printf("Could not delete the MCT service executable and directory: 0x%8x\n", result);
                return result;
            }
            return 0;
        }
    }
    else if (argc > 1)
    {
        printf("Invalid arguments. Valid arguments are: --install, --uninstall.\n");
        return 0;
    }
    else 
    {
        printf("This application can only run as a system service or with one of the following arguments: --install, --uninstall.\n");
        return 0;
    }

    return 0;
}