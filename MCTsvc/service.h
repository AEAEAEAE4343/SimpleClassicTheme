#pragma once

#define SERVICE_NAME  TEXT("MCTsvc")  

#include <windows.h>

// Forward declarations
VOID WINAPI ServiceMain(DWORD argc, LPTSTR* argv);
VOID WINAPI ServiceCtrlHandler(DWORD);
DWORD WINAPI ServiceWorkerThread(LPVOID lpParam);