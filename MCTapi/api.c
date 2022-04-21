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
 *  This file contains the code that communicates with the service for 
 *  the API functions exposed by MCTapi.dll.
 *
 */

#define MCT_EXPORTS
#include "../api.h"
#include "framework.h"

MCT_EXPORT int GetMCTRevision()
{
    return MCT_REVISION;
}

MCT_EXPORT int EnableClassicTheme(unsigned long dwSessionId, PMCT_ERROR_CODE pErrorCode)
{
    if (pErrorCode)  pErrorCode->uChErrorSource = MCT_ERRSRC_WIN32;

    MCT_REQUEST_DATA request = { 0 };
    MCT_RESPONSE_DATA response = { 0 };

    WaitNamedPipeW(L"\\\\.\\pipe\\sctpipe", NMPWAIT_USE_DEFAULT_WAIT);
    HANDLE hPipe = CreateFileW(
        L"\\\\.\\pipe\\sctpipe",
        GENERIC_READ | GENERIC_WRITE,
        0, NULL, OPEN_EXISTING,
        0, NULL);

    if (hPipe == INVALID_HANDLE_VALUE)
    {
        if (pErrorCode) pErrorCode->uChErrorSource = MCT_ERRSRC_MCT;
        if (pErrorCode) pErrorCode->uChResult = MCT_ERR_SERVICE_NOT_RUNNING;
        goto fail;
    }

    request.uChRevision = MCT_REVISION;
    request.uChOperation = MCT_OP_ENABLE;
    request.uChSessionID = dwSessionId;

    if (!WriteFile(hPipe, &request, sizeof(request), NULL, NULL))
        goto fail;

    if (!ReadFile(hPipe, &response, sizeof(MCT_RESPONSE_DATA_REV_1), NULL, NULL))
        goto fail;

    if (pErrorCode) pErrorCode->uChErrorSource = response.uChErrorSource;
    if (pErrorCode) pErrorCode->uChResult = response.uChResult;

    if (request.uChRevision != response.uChRevision)
        goto fail;

    // For future: if MCT_RESPONSE_DATA gets bigger, do a second read to read past MCT_RESPONSE_DATA_REV_1
    //if (!ReadFile(hPipe, &response, sizeof(MCT_RESPONSE_DATA_REV_1), NULL, NULL))
        //goto fail;

    CloseHandle(hPipe);
    return response.uChErrorSource == MCT_ERRSRC_NONE;

fail:
    if (pErrorCode)
        if (pErrorCode->uChErrorSource == MCT_ERRSRC_WIN32)
            pErrorCode->uChResult = GetLastError();

    CloseHandle(hPipe);
    return FALSE;
}

MCT_EXPORT int DisableClassicTheme(unsigned long dwSessionId, PMCT_ERROR_CODE pErrorCode)
{
    if (pErrorCode) pErrorCode->uChErrorSource = MCT_ERRSRC_WIN32;

    MCT_REQUEST_DATA request = { 0 };
    MCT_RESPONSE_DATA response = { 0 };

    WaitNamedPipeW(L"\\\\.\\pipe\\sctpipe", NMPWAIT_USE_DEFAULT_WAIT);
    HANDLE hPipe = CreateFileW(
        L"\\\\.\\pipe\\sctpipe",
        GENERIC_READ | GENERIC_WRITE,
        0, NULL, OPEN_EXISTING,
        0, NULL);

    if (hPipe == INVALID_HANDLE_VALUE)
    {
        if (pErrorCode) pErrorCode->uChErrorSource = MCT_ERRSRC_MCT;
        if (pErrorCode) pErrorCode->uChResult = MCT_ERR_SERVICE_NOT_RUNNING;
        goto fail;
    }

    request.uChRevision = MCT_REVISION;
    request.uChOperation = MCT_OP_DISABLE;
    request.uChSessionID = dwSessionId;

    if (!WriteFile(hPipe, &request, sizeof(request), NULL, NULL))
        goto fail;

    if (!ReadFile(hPipe, &response, sizeof(MCT_RESPONSE_DATA_REV_1), NULL, NULL))
        goto fail;

    if (pErrorCode) pErrorCode->uChErrorSource = response.uChErrorSource;
    if (pErrorCode) pErrorCode->uChResult = response.uChResult;

    if (request.uChRevision != response.uChRevision)
        goto fail;

    // For future: if MCT_RESPONSE_DATA gets bigger, do a second read to read past MCT_RESPONSE_DATA_REV_1
    //if (!ReadFile(hPipe, &response, sizeof(MCT_RESPONSE_DATA_REV_1), NULL, NULL))
        //goto fail;

    CloseHandle(hPipe);
    return response.uChErrorSource == MCT_ERRSRC_NONE;

fail:
    if (pErrorCode)
        if (pErrorCode->uChErrorSource == MCT_ERRSRC_WIN32)
            pErrorCode->uChResult = GetLastError();

    CloseHandle(hPipe);
    return FALSE;
}

MCT_EXPORT int QueryClassicTheme(unsigned long dwSessionId, int* pbEnabled, PMCT_ERROR_CODE pErrorCode)
{
    if (!pbEnabled) return FALSE;
    if (pErrorCode) pErrorCode->uChErrorSource = MCT_ERRSRC_WIN32;

    MCT_REQUEST_DATA request = { 0 };
    MCT_RESPONSE_DATA response = { 0 };

    WaitNamedPipeW(L"\\\\.\\pipe\\sctpipe", NMPWAIT_USE_DEFAULT_WAIT);
    HANDLE hPipe = CreateFileW(
        L"\\\\.\\pipe\\sctpipe",
        GENERIC_READ | GENERIC_WRITE, 
        0, NULL, OPEN_EXISTING,
        0, NULL);

    if (hPipe == INVALID_HANDLE_VALUE)
    {
        if (pErrorCode) pErrorCode->uChErrorSource = MCT_ERRSRC_MCT;
        if (pErrorCode) pErrorCode->uChResult = MCT_ERR_SERVICE_NOT_RUNNING;
        goto fail;
    }

    request.uChRevision = MCT_REVISION;
    request.uChOperation = MCT_OP_QUERY;
    request.uChSessionID = dwSessionId;

    if (!WriteFile(hPipe, &request, sizeof(request), NULL, NULL))
        goto fail;

    if (!ReadFile(hPipe, &response, sizeof(MCT_RESPONSE_DATA_REV_1), NULL, NULL))
        goto fail;

    if (pErrorCode) pErrorCode->uChErrorSource = response.uChErrorSource;
    if (pErrorCode) pErrorCode->uChResult = response.uChResult;

    if (request.uChRevision != response.uChRevision)
        goto fail;

    // For future: if MCT_RESPONSE_DATA gets bigger, do a second read to read past MCT_RESPONSE_DATA_REV_1
    //if (!ReadFile(hPipe, &response, sizeof(MCT_RESPONSE_DATA_REV_1), NULL, NULL))
        //goto fail;

    if (response.uChErrorSource == MCT_ERRSRC_NONE)
        *pbEnabled = response.uChResult == MCT_ERR_QUERY_ENABLED ? 1 : 0;

    CloseHandle(hPipe);
    return response.uChErrorSource == MCT_ERRSRC_NONE;

fail:
    if (pErrorCode)
        if (pErrorCode->uChErrorSource == MCT_ERRSRC_WIN32)
            pErrorCode->uChResult = GetLastError();

    CloseHandle(hPipe);
    return FALSE;
}

MCT_EXPORT int KillThemeSection(unsigned long dwSessionId, PMCT_ERROR_CODE pErrorCode)
{
    if (pErrorCode) pErrorCode->uChErrorSource = MCT_ERRSRC_WIN32;

    MCT_REQUEST_DATA request = { 0 };
    MCT_RESPONSE_DATA response = { 0 };

    WaitNamedPipeW(L"\\\\.\\pipe\\sctpipe", NMPWAIT_USE_DEFAULT_WAIT);
    HANDLE hPipe = CreateFileW(
        L"\\\\.\\pipe\\sctpipe",
        GENERIC_READ | GENERIC_WRITE,
        0, NULL, OPEN_EXISTING,
        0, NULL);

    if (hPipe == INVALID_HANDLE_VALUE)
    {
        if (pErrorCode) pErrorCode->uChErrorSource = MCT_ERRSRC_MCT;
        if (pErrorCode) pErrorCode->uChResult = MCT_ERR_SERVICE_NOT_RUNNING;
        goto fail;
    }

    request.uChRevision = MCT_REVISION;
    request.uChOperation = MCT_OP_DESTROY;
    request.uChSessionID = dwSessionId;

    if (!WriteFile(hPipe, &request, sizeof(request), NULL, NULL))
        goto fail;

    if (!ReadFile(hPipe, &response, sizeof(MCT_RESPONSE_DATA_REV_1), NULL, NULL))
        goto fail;

    if (pErrorCode) pErrorCode->uChErrorSource = response.uChErrorSource;
    if (pErrorCode) pErrorCode->uChResult = response.uChResult;

    if (request.uChRevision != response.uChRevision)
        goto fail;

    // For future: if MCT_RESPONSE_DATA gets bigger, do a second read to read past MCT_RESPONSE_DATA_REV_1
    //if (!ReadFile(hPipe, &response, sizeof(MCT_RESPONSE_DATA_REV_1), NULL, NULL))
        //goto fail;

    CloseHandle(hPipe);
    return response.uChErrorSource == MCT_ERRSRC_NONE;

fail:
    if (pErrorCode)
        if (pErrorCode->uChErrorSource == MCT_ERRSRC_WIN32)
            pErrorCode->uChResult = GetLastError();

    CloseHandle(hPipe);
    return FALSE;
}

