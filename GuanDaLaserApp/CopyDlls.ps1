# MarkingMate DLL 複製腳本
# 用途：將 MarkingMate SDK 的 DLL 複製到專案的輸出目錄

$sourcePath = "C:\Program Files (x86)\MarkingMate\sdk\ocx\CSharp_example\VS2017\LoadFile\LoadFile\bin\x64\Release"
$projectPath = Split-Path -Parent $MyInvocation.MyCommand.Path

# 定義目標目錄
$targets = @(
    "$projectPath\bin\x64\Debug",
    "$projectPath\bin\x64\Release"
)

Write-Host "開始複製 MarkingMate DLL 檔案..." -ForegroundColor Green
Write-Host "來源路徑: $sourcePath" -ForegroundColor Cyan

# 檢查來源路徑是否存在
if (-not (Test-Path $sourcePath)) {
    Write-Host "錯誤：找不到來源路徑！" -ForegroundColor Red
    Write-Host "請確認 MarkingMate 已安裝在 C:\Program Files (x86)\MarkingMate\" -ForegroundColor Yellow
    Read-Host "按任意鍵繼續"
    exit 1
}

# 取得 DLL 檔案列表
$dllFiles = Get-ChildItem -Path $sourcePath -Filter "*.dll"

if ($dllFiles.Count -eq 0) {
    Write-Host "錯誤：來源目錄中沒有找到 DLL 檔案！" -ForegroundColor Red
    Read-Host "按任意鍵繼續"
    exit 1
}

Write-Host "找到 $($dllFiles.Count) 個 DLL 檔案" -ForegroundColor Cyan

# 複製到每個目標目錄
foreach ($target in $targets) {
    Write-Host "`n處理目標: $target" -ForegroundColor Yellow

    # 如果目標目錄不存在，創建它
    if (-not (Test-Path $target)) {
        Write-Host "  創建目錄..." -ForegroundColor Gray
        New-Item -ItemType Directory -Path $target -Force | Out-Null
    }

    # 複製 DLL 檔案
    $copiedCount = 0
    foreach ($dll in $dllFiles) {
        try {
            Copy-Item -Path $dll.FullName -Destination $target -Force
            Write-Host "  ✓ $($dll.Name)" -ForegroundColor Green
            $copiedCount++
        }
        catch {
            Write-Host "  ✗ $($dll.Name) - 失敗: $($_.Exception.Message)" -ForegroundColor Red
        }
    }

    Write-Host "  完成：已複製 $copiedCount / $($dllFiles.Count) 個檔案" -ForegroundColor Cyan
}

Write-Host "`n所有 DLL 檔案已複製完成！" -ForegroundColor Green
Write-Host "`n複製的 DLL 清單：" -ForegroundColor Cyan
foreach ($dll in $dllFiles) {
    Write-Host "  - $($dll.Name)" -ForegroundColor White
}

Write-Host "`n提示：" -ForegroundColor Yellow
Write-Host "- 這些 DLL 檔案需要與您編譯的 .exe 檔案放在同一目錄" -ForegroundColor Gray
Write-Host "- 每次更新 MarkingMate SDK 後，請重新執行此腳本" -ForegroundColor Gray
Write-Host "- 如果遇到問題，請確認已使用 x64 平台編譯專案" -ForegroundColor Gray

Read-Host "`n按 Enter 鍵結束"
