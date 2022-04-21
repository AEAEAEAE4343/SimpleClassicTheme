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
 *  This file contains a pipe that listens for control messages or a stop
 *  message received by the service controller.
 *
 */

#include "pipe.h"

#include <sddl.h>
#include "../structures.h"
#include "classictheme.h"

#define CONNECTING_STATE 0 
#define READING_STATE 1 
#define WRITING_STATE 2 

void DisconnectAndReconnect(HANDLE pipeInstance, LPOVERLAPPED lpOverlapped, LPBOOL lpFPendingIO, LPDWORD lpDwState);

HANDLE hEvents[2];

BOOL RunPipe(HANDLE stopEvent)
{
	HANDLE pipeInstance;
    OVERLAPPED overlapped = { 0 };
    BOOL fPendingIO = FALSE;
    BOOL fSuccess = FALSE;
    DWORD bytes = 0;
    DWORD bytesToWrite = 0;
    DWORD dwPipeState = CONNECTING_STATE;
    PSECURITY_DESCRIPTOR sd;
    SECURITY_ATTRIBUTES sa;

	hEvents[0] = stopEvent;
	hEvents[1] = CreateEvent(NULL, TRUE, TRUE, NULL);

    //ConvertStringSecurityDescriptorToSecurityDescriptor(TEXT("O:COG:DAD:(A;;GAFA;;;WD)"), SDDL_REVISION_1, &sd, NULL);
    ConvertStringSecurityDescriptorToSecurityDescriptor(TEXT("D:(A;;GAFA;;;WD)"), SDDL_REVISION_1, &sd, NULL);
    sa.lpSecurityDescriptor = sd;
    sa.nLength = sizeof(SECURITY_ATTRIBUTES);
    sa.bInheritHandle = FALSE;
    pipeInstance = CreateNamedPipe(
        TEXT("\\\\.\\pipe\\sctpipe"),  // pipe name 
        PIPE_ACCESS_DUPLEX |             // read/write access 
        FILE_FLAG_OVERLAPPED,            // overlapped mode
        PIPE_TYPE_MESSAGE |              // message-type pipe 
        PIPE_READMODE_MESSAGE |          // message-read mode 
        PIPE_WAIT,                       // blocking mode 
        1,                               // number of instances 
        sizeof(char),                    // output buffer size 
        sizeof(char),                    // input buffer size 
        0,                               // client time-out 
        &sa);                             // default security attributes 
        //NULL);

    if (pipeInstance == INVALID_HANDLE_VALUE)
    {
        printf("CreateNamedPipe failed with 0x%8x.\n", GetLastError());
        return FALSE;
    }

    overlapped.hEvent = hEvents[1];
    if (ConnectNamedPipe(pipeInstance, &overlapped))
    {
        printf("ConnectNamedPipe failed with 0x%8x.\n", GetLastError());
        return FALSE;
    }
    
    switch (GetLastError())
    {
        // The overlapped connection in progress. 
    case ERROR_IO_PENDING:
        fPendingIO = TRUE;
        break;
        // Client is already connected, so signal an event. 
    case ERROR_PIPE_CONNECTED:
        if (SetEvent(hEvents[1]))
            break;
        else
        {
            printf("SetEvent failed with 0x%8x.\n", GetLastError());
            return FALSE;
        }
    }

    MCT_REQUEST_DATA inputBuffer;
    MCT_RESPONSE_DATA outputBuffer;
    unsigned char* outputBytes = (unsigned char*)malloc(sizeof(MCT_RESPONSE_DATA));

    dwPipeState = fPendingIO ? CONNECTING_STATE : READING_STATE;
    while (TRUE)
    {
        DWORD dwWait = WaitForMultipleObjects(2, &hEvents, FALSE, INFINITE);
        if (dwWait < 0 || dwWait > 1)
        {
            printf("Index out of range.\n");
            return FALSE;
        }
        else if (dwWait == 0)
        {
            printf("Service exit requested, closing pipe.\n");
            break;
        }
        
        if (fPendingIO)
        {
            printf("I/O Pending...\n");
            fSuccess = GetOverlappedResult(pipeInstance, &overlapped, &bytes, FALSE);
            switch (dwPipeState)
            {
            case CONNECTING_STATE:
                if (!fSuccess)
                {
                    printf("GetOverlappedResult 0x%8x.\n", GetLastError());
                    DisconnectAndReconnect(pipeInstance, &overlapped, &fPendingIO, &dwPipeState);
                    continue;
                }
                dwPipeState = READING_STATE;
                break;
            case READING_STATE:
                if (!fSuccess || bytes == 0)
                {
                    DisconnectAndReconnect(pipeInstance, &overlapped, &fPendingIO, &dwPipeState);
                    continue;
                }
                break;
            case WRITING_STATE:
                if (!fSuccess || bytes != bytesToWrite)
                {
                    DisconnectAndReconnect(pipeInstance, &overlapped, &fPendingIO, &dwPipeState);
                    continue;
                }
                break;
            }
            fPendingIO = FALSE;
        }

        switch (dwPipeState)
        {
        case READING_STATE:
            printf("Reading from pipe\n");
            
            // Read a revision 1 request
            fSuccess = ReadFile(pipeInstance, &inputBuffer, sizeof(MCT_REQUEST_DATA_REV_1), &bytes, &overlapped);
            if (!fSuccess && GetLastError() == ERROR_IO_PENDING)
            {
                fPendingIO = TRUE;
                continue;
            }
            else if (!fSuccess && GetLastError() != ERROR_MORE_DATA)
            {
                printf("ReadFile 0x%8x.\n", GetLastError()); 
                DisconnectAndReconnect(pipeInstance, &overlapped, &fPendingIO, &dwPipeState);
                continue;
            }

            // If the request data doesn't match, return a revision 1 request
            if (inputBuffer.uChRevision != MCT_REVISION)
            {
                MCT_RESPONSE_DATA_REV_1 tempData;
                tempData.uChRevision = MCT_REVISION;
                tempData.uChSuccess = 0;
                tempData.uChErrorSource = MCT_ERRSRC_MCT;
                tempData.uChResult = inputBuffer.uChRevision < MCT_REVISION ? MCT_ERR_REQUEST_TOO_OLD : MCT_ERR_REQUEST_TOO_NEW;
                bytesToWrite = sizeof(MCT_RESPONSE_DATA_REV_1);
                memcpy(outputBytes, &tempData, bytesToWrite);
                continue;
            }

            // Read the rest of the request
#if(MCT_REVISION!=1)
            fSuccess = ReadFile(pipeInstance, ((char*)(&inputBuffer)) + sizeof(MCT_REQUEST_DATA_REV_1), sizeof(MCT_REQUEST_DATA) - sizeof(MCT_REQUEST_DATA_REV_1), &bytes, &overlapped);
            if (!fSuccess && GetLastError() == ERROR_IO_PENDING)
                GetOverlappedResult(pipeInstance, &overlapped, &bytes, TRUE);
#endif

            else if (!fSuccess)
            {
                printf("ReadFile 0x%8x.\n", GetLastError());
                DisconnectAndReconnect(pipeInstance, &overlapped, &fPendingIO, &dwPipeState);
                continue;
            }

            outputBuffer.uChRevision = MCT_REVISION;
            switch (inputBuffer.uChOperation)
            {
            // Disable CT
            // Enable CT
            case MCT_OP_ENABLE:
            case MCT_OP_DISABLE:
            {
                MCT_ERROR_CODE res = SetClassicTheme(inputBuffer.uChSessionID, inputBuffer.uChOperation);

                outputBuffer.uChErrorSource = res.uChErrorSource;
                outputBuffer.uChResult = res.uChResult;
                outputBuffer.uChSuccess = res.uChErrorSource == MCT_ERRSRC_NONE;

                bytesToWrite = sizeof(MCT_RESPONSE_DATA);
                memcpy(outputBytes, &outputBuffer, bytesToWrite);

                dwPipeState = WRITING_STATE;
                break;
            }
            // Query CT (NIY)
            case MCT_OP_QUERY:
            {
                MCT_ERROR_CODE res = GetClassicThemeState(inputBuffer.uChSessionID);

                outputBuffer.uChErrorSource = res.uChErrorSource;
                outputBuffer.uChResult = res.uChResult;
                outputBuffer.uChSuccess = res.uChErrorSource == MCT_ERRSRC_NONE;

                bytesToWrite = sizeof(MCT_RESPONSE_DATA);
                memcpy(outputBytes, &outputBuffer, bytesToWrite);

                dwPipeState = WRITING_STATE;
                break;
            }
            // Destroy ThemeSection (NIY)
            case MCT_OP_DESTROY:
            {
                MCT_ERROR_CODE res = DestroyThemeSection(inputBuffer.uChSessionID);

                outputBuffer.uChErrorSource = res.uChErrorSource;
                outputBuffer.uChResult = res.uChResult;
                outputBuffer.uChSuccess = res.uChErrorSource == MCT_ERRSRC_NONE;

                bytesToWrite = sizeof(MCT_RESPONSE_DATA);
                memcpy(outputBytes, &outputBuffer, bytesToWrite);

                dwPipeState = WRITING_STATE;
                break;
            }
            }
            break;
        case WRITING_STATE:
            printf("Writing result to pipe\n");

            fSuccess = WriteFile(pipeInstance, outputBytes, bytesToWrite, &bytes, &overlapped);
            if (!fSuccess && GetLastError() == ERROR_IO_PENDING)
            {
                fPendingIO = TRUE;
                continue;
            }
            else if (!fSuccess)
            {
                printf("WriteFile 0x%8x.\n", GetLastError());
                DisconnectAndReconnect(pipeInstance, &overlapped, &fPendingIO, &dwPipeState);
                continue;
            }
            dwPipeState = READING_STATE;
            break;
        }
    }

    free(outputBytes);
    if (!DisconnectNamedPipe(pipeInstance))
    {
        printf("DisconnectNamedPipe failed with 0x%8x.\n", GetLastError());
    }
    return TRUE;
}

void DisconnectAndReconnect(HANDLE pipeInstance, LPOVERLAPPED lpOverlapped, LPBOOL lpFPendingIO, LPDWORD lpDwState)
{
    if (!DisconnectNamedPipe(pipeInstance))
    {
        printf("DisconnectNamedPipe failed with 0x%8x.\n", GetLastError());
    }

    if (ConnectNamedPipe(pipeInstance, lpOverlapped))
    {
        printf("ConnectNamedPipe failed with 0x%8x.\n", GetLastError());
        return FALSE;
    }

    switch (GetLastError())
    {
        // The overlapped connection in progress. 
    case ERROR_IO_PENDING:
        *lpFPendingIO = TRUE;
        break;
        // Client is already connected, so signal an event. 
    case ERROR_PIPE_CONNECTED:
        if (SetEvent(hEvents[1]))
            break;
        else
        {
            printf("SetEvent failed with 0x%8x.\n", GetLastError());
            return FALSE;
        }
    }

    *lpDwState = *lpFPendingIO ? CONNECTING_STATE : READING_STATE;
}