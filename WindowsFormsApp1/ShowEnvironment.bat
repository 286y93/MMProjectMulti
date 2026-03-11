@echo off
echo ========================================
echo Environment Information
echo ========================================
echo.
echo Current Directory:
cd
echo.
echo Working Directory (PWD):
echo %CD%
echo.
echo Checking if EXE exists:
set EXE=bin\x64\Debug\WindowsFormsApp1.exe
if exist "%EXE%" (
    echo [OK] Found: %EXE%
    echo Full path: %CD%\%EXE%
) else (
    echo [ERROR] Not found: %EXE%
)
echo.
echo ========================================
echo Test Commands
echo ========================================
echo.
echo If you are in: c:\Users\AlexYang_VW\source\repos\MMProject\MMProjectMulti\WindowsFormsApp1
echo Then use: bin\x64\Debug\WindowsFormsApp1.exe --board 0 --line 0,0,10,10 --mark
echo.
echo If you are in: c:\Users\AlexYang_VW\source\repos\MMProject\MMProjectMulti\WindowsFormsApp1\bin\x64\Debug
echo Then use: WindowsFormsApp1.exe --board 0 --line 0,0,10,10 --mark
echo.
echo ========================================
echo Quick Test (will execute marking!)
echo ========================================
pause
%EXE% --board 0 --line 0,0,10,10 --mark
echo Exit Code: %ERRORLEVEL%
echo.
pause
