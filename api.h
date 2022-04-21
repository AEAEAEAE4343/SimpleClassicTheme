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
#include "structures.h"

#ifdef MCT_EXPORTS
#define MCT_EXPORT __declspec(dllexport)
#else
#define MCT_EXPORT __declspec(dllimport) 
#endif

MCT_EXPORT int GetMCTRevision();
MCT_EXPORT int EnableClassicTheme(unsigned long dwSessionId, PMCT_ERROR_CODE pErrorCode);
MCT_EXPORT int DisableClassicTheme(unsigned long dwSessionId, PMCT_ERROR_CODE pErrorCode);
MCT_EXPORT int QueryClassicTheme(unsigned long dwSessionId, int* pbEnabled, PMCT_ERROR_CODE pErrorCode);
MCT_EXPORT int KillThemeSection(unsigned long dwSessionId, PMCT_ERROR_CODE pErrorCode);