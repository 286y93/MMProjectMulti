@echo off
REM Convert DXF to Lines command test
set EXE=bin\x64\Debug\WindowsFormsApp1.exe
set DXF="File\上翼板-2.dxf"

if not exist %EXE% (
    echo EXE not found!
    exit /b
)

echo.
echo ========================================
echo Helper tool to convert DXF to --lines format
echo ========================================
REM Note: This batch file does not perform the conversion itself.
REM The user wants the C# program to handle this internally or provide a way to pass DXF content as lines?
REM "請將 ... 解析線段 並調整 命令 ... 調整為 ... --lines 的方式"
REM This implies the USER wants ME to parse the DXF file NOW and generate the command line string for them?
REM OR, the user wants me to modify the program to support a new mode?
REM "請將 ... 解析線段" -> "Please parse segments of [file]"
REM "並調整 命令 ... 調整為 ... --lines 的方式" -> "And adjust command ... to use --lines way"

REM So I should read the DXF file, extract lines, and construct the command string for the user.
