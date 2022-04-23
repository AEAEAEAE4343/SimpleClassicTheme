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
 *  This file contains the application entry point for MCTsvc.exe.
 *
 */

#include "service.h"
#include <stdio.h>
#include <tchar.h>

BOOL StopService(SC_HANDLE service)
{
    DWORD bytesNeeded;
    SERVICE_STATUS_PROCESS status;
    if (!ControlService(service, SERVICE_CONTROL_STOP, &status))
    {
        if (GetLastError() == ERROR_SERVICE_NOT_ACTIVE)
            return NULL;

        _tprintf(TEXT("Could not stop the MCT service: 0x%8x\n"), GetLastError());
        return GetLastError();
    }

    ULONGLONG ullStartTime = GetTickCount64();
    while (status.dwCurrentState != SERVICE_STOPPED)
    {
        if (GetTickCount64() - ullStartTime > 10000ULL)
        {
            _tprintf(TEXT("Timed out waiting for the MCT service to stop\n"));
            return ERROR_TIMEOUT;
        }

        if (!QueryServiceStatusEx(
            service,
            SC_STATUS_PROCESS_INFO,
            (LPBYTE)&status,
            sizeof(SERVICE_STATUS_PROCESS),
            &bytesNeeded))
        {
            _tprintf(TEXT("Could not query service status: 0x%8x\n"), GetLastError());
            return GetLastError();
        }

        Sleep(status.dwWaitHint);
    }

    return NULL;
}

int _tmain(int argc, TCHAR* argv[])
{
    SERVICE_TABLE_ENTRY ServiceTable[] =
    {
        {SERVICE_NAME, (LPSERVICE_MAIN_FUNCTION)ServiceMain},
        {NULL, NULL}
    };

    if (StartServiceCtrlDispatcher(ServiceTable))
        return GetLastError();

    if (argc == 2 && (_tcscmp(argv[1], TEXT("/install")) == 0 || _tcscmp(argv[1], TEXT("/uninstall")) == 0))
    {
        _tprintf(TEXT("Preparing...\n"));
        TCHAR destPath[MAX_PATH];
        TCHAR executableSrcPath[MAX_PATH];
        TCHAR executableDestPath[MAX_PATH];
        TCHAR dllSrcPath[MAX_PATH];
        TCHAR dllDestPath[MAX_PATH];
        
        // Get current executable
        if (!GetModuleFileName(NULL, executableSrcPath, MAX_PATH))
        {
            _tprintf(TEXT("Could not get the file path of the current process: 0x%8x\n"), GetLastError());
            return GetLastError();
        }
        
        // Set current directory
        _tcscpy_s(destPath, MAX_PATH, executableSrcPath);
        TCHAR* p = _tcsrchr(destPath, TEXT('\\'));
        if (p) p[0] = 0;
        if (!SetCurrentDirectory(destPath))
        {
            _tprintf(TEXT("Could not set the current working directory: 0x%8x\n"), GetLastError());
            return GetLastError();
        }

        // Get destination directory
        if (!GetFullPathName(TEXT("MCTapi.dll"), MAX_PATH, dllSrcPath, NULL))
        {
            _tprintf(TEXT("Could not determine the file path of the library file: 0x%8x\n"), GetLastError());
            return GetLastError();
        }
        if (!GetEnvironmentVariable(TEXT("programfiles"), destPath, MAX_PATH))
        {
            _tprintf(TEXT("Could not determine the path of the Program Files directory: 0x%8x\n"), GetLastError());
            return GetLastError();
        }
        _tcscpy_s(executableDestPath, MAX_PATH, destPath);
        _tcscpy_s(dllDestPath, MAX_PATH, destPath);

        // Determine destination files
        _tcscat_s(destPath, MAX_PATH, TEXT("\\MCT"));
        _tcscat_s(executableDestPath, MAX_PATH, TEXT("\\MCT\\MCTsvc.exe"));
        _tcscat_s(dllDestPath, MAX_PATH, TEXT("\\MCT\\MCTapi.dll"));

        _tprintf(TEXT("Installation path: %s\n%s -> %s\n%s -> %s\n"), destPath, executableSrcPath, executableDestPath, dllSrcPath, dllDestPath);

        if (_tcscmp(argv[1], TEXT("/install")) == 0)
        {
            _tprintf(TEXT("Starting MCT installation...\n"));
            SC_HANDLE sc = OpenSCManager(NULL, NULL, SC_MANAGER_CREATE_SERVICE);
            if (sc == INVALID_HANDLE_VALUE)
            {
                _tprintf(TEXT("Could not open a handle to the service manager: 0x%8x\n"), GetLastError());
                return GetLastError();
            }

            SC_HANDLE service = OpenService(sc, SERVICE_NAME, SERVICE_ALL_ACCESS);
            if (service)
            {
                _tprintf(TEXT("Service already exists, stopping...\n"));
                BOOL bResult = StopService(service);
                if (bResult)
                    return bResult;
            }

            _tprintf(TEXT("Creating installation directory...\n"));
            if (!CreateDirectory(destPath, NULL) && GetLastError() != ERROR_ALREADY_EXISTS)
            {
                _tprintf(TEXT("Could not create installation directory: 0x%8x\n"), GetLastError());
                return GetLastError();
            }

            _tprintf(TEXT("Copying new files...\n"));
            if (!CopyFile(executableSrcPath, executableDestPath, FALSE))
            {
                _tprintf(TEXT("Could not create service executable: 0x%8x\n"), GetLastError());
                return GetLastError();
            }
            if (!CopyFile(dllSrcPath, dllDestPath, FALSE))
            {
                _tprintf(TEXT("Could not create library file: 0x%8x\n"), GetLastError());
                return GetLastError();
            }

            if (!service)
            {
                _tprintf(TEXT("Creating system service...\n"));
                service = CreateService(
                    sc,                                     // Handle to service controller
                    SERVICE_NAME,                           // Service name
                    SERVICE_DISPLAY_NAME,                   // Display name
                    SERVICE_ALL_ACCESS,                     // Service access
                    SERVICE_WIN32_OWN_PROCESS,              // Service type
                    SERVICE_AUTO_START,                     // Start options
                    SERVICE_ERROR_NORMAL,                   // Severity of error handling
                    executableDestPath,                     // Service executable
                    NULL,                                   // Service load group
                    NULL,                                   // Service tag
                    NULL,                                   // Dependencies
                    NULL,                                   // Run as user (Defaults to LocalSystem)
                    NULL                                    // User password
                );
                if (!service)
                {
                    _tprintf(TEXT("Could not create or open the MCT service: 0x%8x\n"), GetLastError());
                    return GetLastError();
                }
            }

            _tprintf(TEXT("Starting the MCT service...\n"));
            if (!StartService(service, 0, NULL))
            {
                _tprintf(TEXT("Could not start the MCT service: 0x%8x\n"), GetLastError());
                return GetLastError();
            }

            return 0;
        }
        else if (_tcscmp(argv[1], TEXT("/uninstall")) == 0)
        {
            _tprintf(TEXT("Starting MCT removal...\n"));
            SC_HANDLE sc = OpenSCManager(NULL, NULL, SC_MANAGER_CREATE_SERVICE);
            if (sc == INVALID_HANDLE_VALUE)
            {
                _tprintf(TEXT("Could not open a handle to the service manager: 0x%8x\n"), GetLastError());
                return GetLastError();
            }

            _tprintf(TEXT("Locating the MCT service...\n"));
            SC_HANDLE service = OpenService(sc, TEXT("MCTsvc"), SERVICE_ALL_ACCESS);
            if (service)
            {
                _tprintf(TEXT("Stopping the MCT service...\n"));
                BOOL bResult = StopService(service);
                if (bResult)
                    return bResult;

                _tprintf(TEXT("Removing the MCT service...\n"));
                if (!DeleteService(service))
                {
                    _tprintf(TEXT("Could not delete the MCT service: 0x%8x\n"), GetLastError());
                    return GetLastError();
                }
            }
            else 
            {
                _tprintf(TEXT("Could not open the MCT service: 0x%8x\nContinuing removal anyways...\n"), GetLastError());
            }

            destPath[_tcslen(destPath) + 1] = 0;
            _tprintf(TEXT("Deleting installation files...\n"));
            SHFILEOPSTRUCT file_op = {
            NULL,
            FO_DELETE,
            destPath,
            NULL,
            FOF_NO_UI,
            NULL,
            0,
            NULL };
            BOOL result = SHFileOperation(&file_op);
            if (result)
            {
                _tprintf(TEXT("Could not delete the MCT service executable and directory: 0x%8x\n"), result);
                return result;
            }
            return 0;
        }
    }
    else if (argc > 1)
    {
        _tprintf(TEXT("Invalid arguments. Valid arguments are: /install, /uninstall.\n"));
        return 0;
    }
    else 
    {
        _tprintf(TEXT("This application can only run as a system service or with one of the following arguments: /install, /uninstall.\n"));
        return 0;
    }

    return 0;
}