@echo off
REM MarkingMateMulti Command Line Debug Test

echo ========================================
echo MarkingMateMulti Command Line Test
echo ========================================
echo.

REM Set EXE path
set EXE=bin\x64\Debug\WindowsFormsApp1.exe

if not exist "%EXE%" (
    echo ERROR: Cannot find %EXE%
    echo Please compile the project first!
    pause
    exit /b 1
)

echo WARNING: This test will execute actual laser marking!
echo Please ensure:
echo 1. Hardware is connected
echo 2. IP settings are correct
echo 3. Work area is clear
echo.
pause

echo.
echo ========================================
echo Test 1: Draw line only (no marking)
echo ========================================
echo Command: %EXE% --board 0 --line 0,0,10,10
echo.

%EXE% --board 0 --line 0,0,10,10
set RESULT1=%ERRORLEVEL%

echo.
echo Result: Exit Code = %RESULT1%
if %RESULT1% EQU 0 (
    echo [SUCCESS] Line drawing completed
) else if %RESULT1% EQU 1 (
    echo [FAILED] Initialization failed - Check SDK and hardware
) else if %RESULT1% EQU 2 (
    echo [FAILED] Drawing failed - Check coordinates
) else if %RESULT1% EQU 4 (
    echo [FAILED] Parameter error - Check command line arguments
) else (
    echo [FAILED] Unknown error
)
echo.
echo ========================================
echo.
pause

echo.
echo ========================================
echo Test 2: Draw line and mark
echo ========================================
echo Command: %EXE% --board 0 --line 0,0,10,10 --mark
echo.
echo WARNING: This will execute actual laser marking!
pause

%EXE% --board 0 --line 0,0,10,10 --mark
set RESULT2=%ERRORLEVEL%

echo.
echo Result: Exit Code = %RESULT2%
if %RESULT2% EQU 0 (
    echo [SUCCESS] Marking completed
) else if %RESULT2% EQU 1 (
    echo [FAILED] Initialization failed
) else if %RESULT2% EQU 2 (
    echo [FAILED] Drawing failed
) else if %RESULT2% EQU 3 (
    echo [FAILED] Marking failed - Check:
    echo   - Control card connection
    echo   - Scan head status
    echo   - Laser power
    echo   - Marking parameters
) else if %RESULT2% EQU 4 (
    echo [FAILED] Parameter error
) else (
    echo [FAILED] Unknown error
)
echo.
echo ========================================
echo.

echo Troubleshooting:
echo.
echo 1. If initialization fails (Exit Code 1):
echo    - Check MarkingMate SDK installation
echo    - Verify config path exists
echo    - Try GUI mode first
echo.
echo 2. If drawing fails (Exit Code 2):
echo    - Check coordinate format (x1,y1,x2,y2)
echo    - Verify coordinates are in range (-50 to +50)
echo.
echo 3. If marking fails (Exit Code 3):
echo    - Use GUI mode "Test Connection"
echo    - Check control card IP settings
echo    - Verify laser head connection
echo    - Check marking parameters (power, speed)
echo.
echo 4. To see detailed Debug output:
echo    - Download DebugView from Microsoft Sysinternals
echo    - Run DebugView before executing this test
echo    - Debug output shows detailed execution steps
echo.

pause
