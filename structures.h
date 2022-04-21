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
 *  This file contains the definitions for the structures used in the
 *  inter-process communication between the service the service controller.
 *
 */

#pragma once
#pragma pack(push, 1)

#define MCT_REV_1 0x01

#ifndef MCT_REVISION
#define MCT_REVISION MCT_REV_1
#endif

#define MCT_OP_ENABLE				0b01
#define MCT_OP_DISABLE				0b00
#define MCT_OP_QUERY				0b10
#define MCT_OP_DESTROY				0b11

#define MCT_ERRSRC_NONE				0b0000
#define MCT_ERRSRC_MCT				0b0001
#define MCT_ERRSRC_WIN32			0b0010
#define MCT_ERRSRC_NTDLL			0b0011

#define MCT_ERR_SUCCESS				0
#define MCT_ERR_SESSION_ID_INVALID	1
#define MCT_ERR_SERVICE_NOT_RUNNING	2

#define MCT_ERR_QUERY_ENABLED		0x80000001UL
#define MCT_ERR_QUERY_DISABLED		0x80000000UL

#define MCT_ERR_REQUEST_TOO_OLD		0xFFFFFFFDUL
#define MCT_ERR_REQUEST_TOO_NEW		0xFFFFFFFEUL
#define MCT_ERR_INVALID_REQUEST		0xFFFFFFFFUL

/*
	Per-revision definitions
*/

#ifndef MCT_ERROR_CODE_DEF
typedef struct _MCT_ERROR_CODE
{
	unsigned char uChErrorSource;
	unsigned long uChResult;
} MCT_ERROR_CODE, * PMCT_ERROR_CODE;
#define MCT_ERROR_CODE_DEF
#endif

typedef struct _MCT_REQUEST_DATA_REV_1
{
	unsigned char uChRevision;
	unsigned char uChOperation : 2;
	unsigned char uChSessionID : 6;
} MCT_REQUEST_DATA_REV_1, *PMCT_REQUEST_DATA_REV_1;

typedef struct _MCT_RESPONSE_DATA_REV_1
{
	unsigned char uChRevision;
	unsigned char uChSuccess : 4;
	unsigned char uChErrorSource : 4;
	unsigned long uChResult;
} MCT_RESPONSE_DATA_REV_1, *PMCT_RESPONSE_DATA_REV_1;

/*
	Final definitions
*/

#if(MCT_REVISION==MCT_REV_1)
#define MCT_REQUEST_DATA MCT_REQUEST_DATA_REV_1
#define PMCT_REQUEST_DATA PMCT_REQUEST_DATA_REV_1
#define MCT_RESPONSE_DATA MCT_RESPONSE_DATA_REV_1
#define PMCT_RESPONSE_DATA PMCT_RESPONSE_DATA_REV_1
#endif

#pragma pack(pop)