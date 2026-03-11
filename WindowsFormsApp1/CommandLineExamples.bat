@echo off
REM MarkingMateMulti Command Line Examples

echo ========================================
echo MarkingMateMulti Command Line Examples
echo ========================================
echo.

REM Set EXE path (adjust based on build configuration)
set EXE=bin\x64\Debug\WindowsFormsApp1.exe

if not exist "%EXE%" (
    echo ERROR: Cannot find %EXE%
    echo Please compile the project first!
    pause
    exit /b 1
)

echo Example 1: Show help
echo Command: %EXE% --help
echo.
pause
%EXE% --help
echo Exit Code: %ERRORLEVEL%
echo.
echo ========================================
echo.

echo Example 2: Draw a line on board 0 (no marking)
echo Command: %EXE% --board 0 --line 0,0,50,50
echo.
pause
%EXE% --board 0 --line 0,0,50,50
echo Exit Code: %ERRORLEVEL%
echo.
echo ========================================
echo.

echo Example 3: Draw a line on board 0 and mark
echo Command: %EXE% --board 0 --line 0,0,50,50 --mark
echo WARNING: This will execute actual laser marking!
echo.
pause
%EXE% --board 0 --line 0,0,50,50 --mark
echo Exit Code: %ERRORLEVEL%
echo.
echo ========================================
echo.

echo Example 4: Draw multiple lines on board 1 and mark
echo Command: %EXE% --board 1 --lines "0,0,50,50;10,10,40,40;20,20,30,30" --mark
echo WARNING: This will execute actual laser marking!
echo.
pause
%EXE% --board 1 --lines "0,0,50,50;10,10,40,40;20,20,30,30" --mark
echo Exit Code: %ERRORLEVEL%
echo.
echo ========================================
echo.

echo Example 5: Use custom config path
echo Command: %EXE% --board 2 --config /cfg_config_MM3 --line 0,0,100,100
echo.
pause
%EXE% --board 2 --config /cfg_config_MM3 --line 0,0,100,100
echo Exit Code: %ERRORLEVEL%
echo.
echo ========================================
echo.

echo All examples completed!
pause
