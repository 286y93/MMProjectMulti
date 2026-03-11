@echo off
echo ==========================================
echo Kill All MarkingMate Instances
echo ==========================================
echo.
echo This will forcefully close:
echo - WindowsFormsApp1.exe
echo - Any MarkingMate related processes
echo.
pause

taskkill /F /IM WindowsFormsApp1.exe 2>nul
if %ERRORLEVEL% EQU 0 (
    echo [OK] WindowsFormsApp1.exe terminated
) else (
    echo [INFO] WindowsFormsApp1.exe not running
)

timeout /t 2

echo.
echo Cleaning up...
echo Please wait for 5 seconds...
timeout /t 5

echo.
echo Done! Now you can restart the program.
echo.
pause
