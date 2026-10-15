@echo off
echo ========================================
echo MarkingMate OCX Registration Script
echo ========================================
echo.

REM Check for admin rights
net session >nul 2>&1
if %errorLevel% NEQ 0 (
    echo ERROR: This script requires Administrator privileges!
    echo Please right-click and select "Run as Administrator"
    pause
    exit /b 1
)

echo Registering MarkingMate x64 OCX Components...
echo.

set DLL_PATH=C:\Program Files (x86)\MarkingMate\sdk\ocx\CSharp_example\VS2017\LoadFile\LoadFile\bin\x64\Release

REM Register each DLL
echo Registering MMEditx64Lib.dll...
regsvr32 /s "%DLL_PATH%\MMEditx64Lib.dll"
if %errorLevel% EQU 0 (
    echo [OK] MMEditx64Lib.dll registered successfully
) else (
    echo [FAIL] Failed to register MMEditx64Lib.dll
)

echo Registering MMMarkx64Lib.dll...
regsvr32 /s "%DLL_PATH%\MMMarkx64Lib.dll"
if %errorLevel% EQU 0 (
    echo [OK] MMMarkx64Lib.dll registered successfully
) else (
    echo [FAIL] Failed to register MMMarkx64Lib.dll
)

echo Registering MMStatusx64Lib.dll...
regsvr32 /s "%DLL_PATH%\MMStatusx64Lib.dll"
if %errorLevel% EQU 0 (
    echo [OK] MMStatusx64Lib.dll registered successfully
) else (
    echo [FAIL] Failed to register MMStatusx64Lib.dll
)

echo.
echo ========================================
echo Registration Complete!
echo ========================================
echo.
echo NOTE: If registration failed, please:
echo 1. Make sure MarkingMate is installed correctly
echo 2. Run this script as Administrator
echo 3. Check that the DLL files exist at:
echo    %DLL_PATH%
echo.
pause
