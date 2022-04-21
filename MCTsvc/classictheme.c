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
 *  This file contains functions to modify and query the ThemeSection
 *  memory section of a given user.
 * 
 */

#include "classictheme.h"

#include <Windows.h>
#include <sddl.h>
#include <stdio.h>
#include <strsafe.h>
#include "ntobj.h"

MCT_ERROR_CODE SetClassicTheme(int sessionId, BOOL state)
{
    MCT_ERROR_CODE res;

    wchar_t buffer[MAX_PATH];
    UNICODE_STRING string;
    OBJECT_ATTRIBUTES attrib;
    HANDLE section;
    DWORD ret;
    PSECURITY_DESCRIPTOR sd;

    printf("Setting sesssion %d to %s\n", sessionId, state ? "enabled" : "disabled");

    // Construct theme section path
    StringCchPrintfW(buffer, MAX_PATH, L"\\Sessions\\%d\\Windows\\ThemeSection", sessionId);
    string.Buffer = buffer;
    string.Length = lstrlenW(buffer) * sizeof(wchar_t);
    string.MaximumLength = MAX_PATH * sizeof(wchar_t);

    wprintf(L"%s\n", buffer);

    // Set object information
    InitializeObjectAttributes(&attrib, &string, OBJ_KERNEL_HANDLE | OBJ_CASE_INSENSITIVE, NULL, NULL);

    // Open theme section
    if (ret = NtOpenSection(&section, // Pointer to handle variable
        WRITE_DAC,   // 262144 is a value taken directly from SCT, can't remember what it means
        &attrib)) // Information that describes object to be opened
    {
        printf("NtOpenSection failed with 0x%8x.\n", ret);
        res.uChErrorSource = MCT_ERRSRC_NTDLL;
        res.uChResult = ret;
        return res;
    }

    // Get security descriptor from string
    if (!ConvertStringSecurityDescriptorToSecurityDescriptorA(state ? "O:BAG:SYD:(A;;RC;;;IU)(A;;DCSWRPSDRCWDWO;;;SY)" : "O:BAG:SYD:(A;;CCLCRC;;;IU)(A;;CCDCLCSWRPSDRCWDWO;;;SY)", SDDL_REVISION_1, &sd, NULL))
    {
        printf("ConvertStringSecurityDescriptorToSecurityDescriptorA failed with 0x%8x.\n", GetLastError());
        res.uChErrorSource = MCT_ERRSRC_WIN32;
        res.uChResult = GetLastError();
        return res;
    }

    // Set the security descriptor
    if (ret = NtSetSecurityObject(section, DACL_SECURITY_INFORMATION, sd))
    {
        printf("NtSetSecurityObject failed with 0x%8x.\n", ret);
        res.uChErrorSource = MCT_ERRSRC_NTDLL;
        res.uChResult = ret;
        return res;
    }

    // Free the security descriptor
    LocalFree(sd);

    // Close the theme handle
    NtClose(section);

    res.uChErrorSource = MCT_ERRSRC_NONE;
    res.uChResult = MCT_ERR_SUCCESS;
    return res;
}

MCT_ERROR_CODE GetClassicThemeState(int sessionId)
{
    MCT_ERROR_CODE res;

    wchar_t buffer[MAX_PATH];
    UNICODE_STRING string;
    OBJECT_ATTRIBUTES attrib;
    HANDLE section;
    DWORD ret;
    DWORD bytesNeeded;
    PSECURITY_DESCRIPTOR psd = (PSECURITY_DESCRIPTOR)malloc(sizeof(SECURITY_DESCRIPTOR));

    printf("Getting state of sesssion %d\n", sessionId);

    // Construct theme section path
    StringCchPrintfW(buffer, MAX_PATH, L"\\Sessions\\%d\\Windows\\ThemeSection", sessionId);
    string.Buffer = buffer;
    string.Length = lstrlenW(buffer) * sizeof(wchar_t);
    string.MaximumLength = MAX_PATH * sizeof(wchar_t);

    wprintf(L"%s\n", buffer);

    // Set object information
    InitializeObjectAttributes(&attrib, &string, OBJ_KERNEL_HANDLE | OBJ_CASE_INSENSITIVE, NULL, NULL);

    // Open theme section
    if (ret = NtOpenSection(&section, // Pointer to handle variable
        READ_CONTROL,   // 262144 is a value taken directly from SCT, can't remember what it means
        &attrib)) // Information that describes object to be opened
    {
        printf("NtOpenSection failed with 0x%8x.\n", ret);
        res.uChErrorSource = MCT_ERRSRC_NTDLL;
        res.uChResult = ret;
        return res;
    }

    // Get security descriptor
    if (ret = NtQuerySecurityObject(section, DACL_SECURITY_INFORMATION, psd, sizeof(SECURITY_DESCRIPTOR), &bytesNeeded))
    {
        if (ret == 0xC0000023) // STATUS_BUFFER_TOO_SMALL
        {
            free(psd);
            psd = (PSECURITY_DESCRIPTOR)malloc(bytesNeeded);
            if (ret = NtQuerySecurityObject(section, DACL_SECURITY_INFORMATION, psd, bytesNeeded, &bytesNeeded))
            {
                printf("NtQuerySecurityObject failed with 0x%8x.\n", ret);
                res.uChErrorSource = MCT_ERRSRC_NTDLL;
                res.uChResult = ret;
                return res;
            }
        }
        else
        {
            printf("NtQuerySecurityObject failed with 0x%8x.\n", ret);
            res.uChErrorSource = MCT_ERRSRC_NTDLL;
            res.uChResult = ret;
            return res;
        }
    }

    BOOL daclPresent; BOOL defaulted;
    PACL dacl;
    if (ret = RtlGetDaclSecurityDescriptor(psd, &daclPresent, &dacl, &defaulted))
    {
        printf("RtlGetDaclSecurityDescriptor failed with 0x%8x.\n", ret);
        res.uChErrorSource = MCT_ERRSRC_NTDLL;
        res.uChResult = ret;
        return res;
    }

    /*
    * Although the following is possible, it's harder than just checking the descriptor string
    *
    // Find (Everyone) in the ACL
    LPVOID ace;
    for (int i = 0; GetAce(dacl, i, &ace); i++)
    {

    }
    */
    // Get security descriptor in string format
    LPSTR descriptor; ULONG length;
    if (!ConvertSecurityDescriptorToStringSecurityDescriptorA(psd, SDDL_REVISION_1, DACL_SECURITY_INFORMATION, &descriptor, &length))
    {
        printf("ConvertSecurityDescriptorToStringSecurityDescriptorA failed with 0x%8x.\n", GetLastError());
        res.uChErrorSource = MCT_ERRSRC_WIN32;
        res.uChResult = GetLastError();
        return res;
    }

    // Free the security descriptor
    free(psd);

    // Close the theme handle
    NtClose(section);

    // Check if CC (create children) and LC (list children) are in the string
    char* cc = strstr(descriptor, "CC");
    char* lc = strstr(descriptor, "LC");

    res.uChErrorSource = MCT_ERRSRC_NONE;
    res.uChResult = cc && lc ? MCT_ERR_QUERY_DISABLED : MCT_ERR_QUERY_ENABLED;
    return res;
}

MCT_ERROR_CODE DestroyThemeSection(int sessionId)
{
    MCT_ERROR_CODE res;
    res.uChErrorSource = MCT_ERRSRC_MCT;
    res.uChResult = MCT_ERR_INVALID_REQUEST;
    return res;
}