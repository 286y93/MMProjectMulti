@echo off
echo 複製 MarkingMate SDK DLL 檔案到輸出目錄...

set SOURCE_PATH=C:\Program Files (x86)\MarkingMate\sdk\ocx\CSharp_example\VS2017\LoadFile\LoadFile\bin\x64\Release
set TARGET_PATH_DEBUG=bin\x64\Debug
set TARGET_PATH_RELEASE=bin\x64\Release

REM 建立目錄（如果不存在）
if not exist "%TARGET_PATH_DEBUG%" mkdir "%TARGET_PATH_DEBUG%"
if not exist "%TARGET_PATH_RELEASE%" mkdir "%TARGET_PATH_RELEASE%"

REM 複製 DLL 檔案到 Debug 目錄
echo 複製到 Debug 目錄...
copy "%SOURCE_PATH%\AxMMEdit_x64.dll" "%TARGET_PATH_DEBUG%\" /Y
copy "%SOURCE_PATH%\AxMMMark_x64.dll" "%TARGET_PATH_DEBUG%\" /Y
copy "%SOURCE_PATH%\AxMMStatus_x64.dll" "%TARGET_PATH_DEBUG%\" /Y
copy "%SOURCE_PATH%\MMEditx64Lib.dll" "%TARGET_PATH_DEBUG%\" /Y
copy "%SOURCE_PATH%\MMMarkx64Lib.dll" "%TARGET_PATH_DEBUG%\" /Y
copy "%SOURCE_PATH%\MMStatusx64Lib.dll" "%TARGET_PATH_DEBUG%\" /Y

REM 複製 DLL 檔案到 Release 目錄
echo 複製到 Release 目錄...
copy "%SOURCE_PATH%\AxMMEdit_x64.dll" "%TARGET_PATH_RELEASE%\" /Y
copy "%SOURCE_PATH%\AxMMMark_x64.dll" "%TARGET_PATH_RELEASE%\" /Y
copy "%SOURCE_PATH%\AxMMStatus_x64.dll" "%TARGET_PATH_RELEASE%\" /Y
copy "%SOURCE_PATH%\MMEditx64Lib.dll" "%TARGET_PATH_RELEASE%\" /Y
copy "%SOURCE_PATH%\MMMarkx64Lib.dll" "%TARGET_PATH_RELEASE%\" /Y
copy "%SOURCE_PATH%\MMStatusx64Lib.dll" "%TARGET_PATH_RELEASE%\" /Y

echo.
echo 完成！所有 DLL 檔案已複製。
pause
