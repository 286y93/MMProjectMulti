@echo off
echo ========================================
echo MarkingMate OCX Diagnostic Tool
echo ========================================
echo.

set DLL_PATH=C:\Program Files (x86)\MarkingMate\sdk\ocx\CSharp_example\VS2017\LoadFile\LoadFile\bin\x64\Release

echo Checking DLL files...
echo.

if exist "%DLL_PATH%\MMEditx64Lib.dll" (
    echo [OK] MMEditx64Lib.dll found
) else (
    echo [MISSING] MMEditx64Lib.dll NOT FOUND
)

if exist "%DLL_PATH%\MMMarkx64Lib.dll" (
    echo [OK] MMMarkx64Lib.dll found
) else (
    echo [MISSING] MMMarkx64Lib.dll NOT FOUND
)

if exist "%DLL_PATH%\MMStatusx64Lib.dll" (
    echo [OK] MMStatusx64Lib.dll found
) else (
    echo [MISSING] MMStatusx64Lib.dll NOT FOUND
)

echo.
echo Checking if running as Administrator...
net session >nul 2>&1
if %errorLevel% EQU 0 (
    echo [OK] Running with Administrator privileges
) else (
    echo [WARNING] NOT running as Administrator
    echo Please run this script as Administrator to register DLLs
)

echo.
echo Checking registry entries...
reg query "HKEY_CLASSES_ROOT\MMEditx64.MMEditx64" >nul 2>&1
if %errorLevel% EQU 0 (
    echo [OK] MMEditx64 is registered
) else (
    echo [NOT REGISTERED] MMEditx64 is NOT registered
)

reg query "HKEY_CLASSES_ROOT\MMMarkx64.MMMarkx64" >nul 2>&1
if %errorLevel% EQU 0 (
    echo [OK] MMMarkx64 is registered
) else (
    echo [NOT REGISTERED] MMMarkx64 is NOT registered
)

reg query "HKEY_CLASSES_ROOT\MMStatusx64.MMStatusx64" >nul 2>&1
if %errorLevel% EQU 0 (
    echo [OK] MMStatusx64 is registered
) else (
    echo [NOT REGISTERED] MMStatusx64 is NOT registered
)

echo.
echo ========================================
echo Diagnostic Complete
echo ========================================
echo.
pause
