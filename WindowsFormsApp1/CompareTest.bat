@echo off
echo ========================================
echo Command Comparison Test
echo ========================================
echo.

set EXE=bin\x64\Debug\WindowsFormsApp1.exe

echo This batch file will show you the exact command being executed.
echo.
echo Test 1: Full path command
echo ========================================
echo Command: "%CD%\%EXE%" --board 0 --line 0,0,10,10 --mark
echo.
pause
"%CD%\%EXE%" --board 0 --line 0,0,10,10 --mark
echo Exit Code: %ERRORLEVEL%
echo.
pause

echo Test 2: Relative path command
echo ========================================
echo Command: %EXE% --board 0 --line 0,0,10,10 --mark
echo.
pause
%EXE% --board 0 --line 0,0,10,10 --mark
echo Exit Code: %ERRORLEVEL%
echo.
pause

echo Test 3: Direct executable name (assumes in PATH)
echo ========================================
echo Command: WindowsFormsApp1.exe --board 0 --line 0,0,10,10 --mark
echo.
cd bin\x64\Debug
pause
WindowsFormsApp1.exe --board 0 --line 0,0,10,10 --mark
echo Exit Code: %ERRORLEVEL%
cd ..\..\..
echo.
pause

echo ========================================
echo For manual testing, copy one of these commands:
echo.
echo Option 1 (from WindowsFormsApp1 folder):
echo %EXE% --board 0 --line 0,0,10,10 --mark
echo.
echo Option 2 (from bin\x64\Debug folder):
echo WindowsFormsApp1.exe --board 0 --line 0,0,10,10 --mark
echo.
echo ========================================
pause
