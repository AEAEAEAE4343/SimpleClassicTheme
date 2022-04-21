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
 */

#pragma once

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files
#include <windows.h>
