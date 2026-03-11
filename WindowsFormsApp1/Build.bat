@echo off
echo Building MarkingMateMulti project...

REM Try different Visual Studio paths
set MSBUILD=""

if exist "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD="C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
)

if exist "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD="C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
)

if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
)

if %MSBUILD%=="" (
    echo ERROR: MSBuild.exe not found!
    echo Please build the project manually in Visual Studio.
    pause
    exit /b 1
)

echo Using MSBuild: %MSBUILD%

%MSBUILD% MarkingMateMulti.csproj /p:Configuration=Debug /p:Platform=x64 /t:Build

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build succeeded!
    echo Output: bin\x64\Debug\WindowsFormsApp1.exe
) else (
    echo.
    echo Build failed!
)

pause
