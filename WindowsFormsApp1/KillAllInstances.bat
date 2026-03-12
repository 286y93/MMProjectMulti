@echo off
set "AUTO_MODE=%1"

echo ==========================================
echo Kill All MarkingMate Instances
echo ==========================================
echo.
echo This will forcefully close:
echo - WindowsFormsApp1.exe
echo - MM27Dx64.exe
echo - Any MarkingMate related processes
echo.

if /I "%AUTO_MODE%"=="nopause" goto :skip_intro_pause
pause
:skip_intro_pause

taskkill /F /IM WindowsFormsApp1.exe 2>nul
if %ERRORLEVEL% EQU 0 (
    echo [OK] WindowsFormsApp1.exe terminated
) else (
    echo [INFO] WindowsFormsApp1.exe not running
)

taskkill /F /IM MM27Dx64.exe 2>nul
if %ERRORLEVEL% EQU 0 (
    echo [OK] MM27Dx64.exe terminated
) else (
    echo [INFO] MM27Dx64.exe not running
)

timeout /t 2 /nobreak >nul

echo.
echo Cleaning up...
echo Please wait for 2 seconds...
timeout /t 2 /nobreak >nul

echo.
echo Done! Now you can restart the program.
echo.

if /I "%AUTO_MODE%"=="nopause" goto :skip_end_pause
pause
:skip_end_pause
