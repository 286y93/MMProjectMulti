@echo off
echo ========================================
echo Multiple Lines Marking Test
echo ========================================
echo.

set EXE=bin\x64\Debug\WindowsFormsApp1.exe

if not exist "%EXE%" (
    echo ERROR: Cannot find %EXE%
    pause
    exit /b 1
)

echo WARNING: This will execute actual laser marking!
echo.
pause

echo Test 1: Single line (baseline)
echo ========================================
echo Command: %EXE% --board 0 --line 0,0,10,10 --mark
pause
%EXE% --board 0 --line 0,0,10,10 --mark
echo Exit Code: %ERRORLEVEL%
echo.
pause

echo Test 2: Two lines
echo ========================================
echo Command: %EXE% --board 0 --lines "0,0,10,10;20,20,30,30" --mark
pause
%EXE% --board 0 --lines "0,0,10,10;20,20,30,30" --mark
echo Exit Code: %ERRORLEVEL%
echo.
pause

echo Test 3: Three lines (triangle)
echo ========================================
echo Command: %EXE% --board 0 --lines "0,0,10,0;10,0,5,10;5,10,0,0" --mark
pause
%EXE% --board 0 --lines "0,0,10,0;10,0,5,10;5,10,0,0" --mark
echo Exit Code: %ERRORLEVEL%
echo.
pause

echo Test 4: Four lines (square)
echo ========================================
echo Command: %EXE% --board 0 --lines "-10,-10,10,-10;10,-10,10,10;10,10,-10,10;-10,10,-10,-10" --mark
pause
%EXE% --board 0 --lines "-10,-10,10,-10;10,-10,10,10;10,10,-10,10;-10,10,-10,-10" --mark
echo Exit Code: %ERRORLEVEL%
echo.
pause

echo Test 5: Large square (more visible)
echo ========================================
echo Command: %EXE% --board 0 --lines "-30,-30,30,-30;30,-30,30,30;30,30,-30,30;-30,30,-30,-30" --mark
pause
%EXE% --board 0 --lines "-30,-30,30,-30;30,-30,30,30;30,30,-30,30;-30,30,-30,-30" --mark
echo Exit Code: %ERRORLEVEL%
echo.

echo ========================================
echo All tests completed!
echo.
echo If Test 1 works but Test 2-5 don't:
echo - The issue is with multiple lines handling
echo - Check DebugView for detailed output
echo.
echo If all tests show Exit Code 0 but no laser:
echo - Check marking parameters in MarkingMate software
echo - Verify laser power settings
echo.
pause
