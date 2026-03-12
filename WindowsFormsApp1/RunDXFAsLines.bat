@echo off
setlocal EnableDelayedExpansion

REM Helper tool to convert DXF to --lines format
set SCRIPT_DIR=%~dp0
cd /d "%SCRIPT_DIR%"

set EXE=bin\x64\Debug\WindowsFormsApp1.exe

if not exist "%EXE%" (
    echo EXE not found at: %EXE%
    exit /b
)

echo Parsing DXF file to extracting lines...

REM Run the PowerShell script (it is in the parent directory)
pushd ..
powershell -ExecutionPolicy Bypass -File "%CD%\parse_dxf_v2.ps1"
if %ERRORLEVEL% NEQ 0 (
    echo Failed to run PowerShell script.
    popd
    exit /b
)
popd

REM Read the lines from the file in the parent directory
set LINES_FILE=..\dxf_lines.txt
if not exist "%LINES_FILE%" (
    echo Lines file not found: %LINES_FILE%
    exit /b
)

set /p LINES=<"%LINES_FILE%"

if not defined LINES (
    echo Error: Failed to extract lines or file is empty.
    exit /b
)

echo.
echo Executing command with extracted lines...
echo Command: %EXE% --board 0 --lines "..." --mark
echo.

%EXE% --board 0 --lines "%LINES%" --mark

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Execution failed with error code: %ERRORLEVEL%
) else (
    echo.
    echo Execution completed successfully.
)

echo.
echo ==========================================
echo Auto-Cleaning up all instances...
echo ==========================================
call "%SCRIPT_DIR%\KillAllInstances.bat" nopause

endlocal
