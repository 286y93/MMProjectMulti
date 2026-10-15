@echo off
echo ========================================
echo MarkingMate OCX Unregistration Script
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

echo Unregistering MarkingMate x64 OCX Components...
echo.

set DLL_PATH=C:\Program Files (x86)\MarkingMate\sdk\ocx\CSharp_example\VS2017\LoadFile\LoadFile\bin\x64\Release

REM Unregister each DLL
echo Unregistering MMEditx64Lib.dll...
regsvr32 /u /s "%DLL_PATH%\MMEditx64Lib.dll"
echo [OK] MMEditx64Lib.dll unregistered

echo Unregistering MMMarkx64Lib.dll...
regsvr32 /u /s "%DLL_PATH%\MMMarkx64Lib.dll"
echo [OK] MMMarkx64Lib.dll unregistered

echo Unregistering MMStatusx64Lib.dll...
regsvr32 /u /s "%DLL_PATH%\MMStatusx64Lib.dll"
echo [OK] MMStatusx64Lib.dll unregistered

echo.
echo ========================================
echo Unregistration Complete!
echo ========================================
echo.
pause
