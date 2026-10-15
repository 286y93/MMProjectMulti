$source = "C:\Program Files (x86)\MarkingMate\MmAssembly"
$projectDir = Split-Path -Parent $MyInvocation.MyCommand.Path

$targets = @(
    "$projectDir\bin\x64\Debug",
    "$projectDir\bin\x64\Release"
)

$dlls = @(
    "AxMMMark_x64_1.dll",
    "MMMarkx641Lib.dll",
    "AxMMEdit_x64_1.dll",
    "MMEditx641Lib.dll"
)

Write-Host "Copying MultiMM DLL files..." -ForegroundColor Green
Write-Host "Source: $source"

foreach ($target in $targets) {
    if (-not (Test-Path $target)) {
        New-Item -ItemType Directory -Path $target -Force | Out-Null
    }
    Write-Host ""
    Write-Host "Target: $target" -ForegroundColor Yellow
    foreach ($dll in $dlls) {
        $src = Join-Path $source $dll
        if (Test-Path $src) {
            Copy-Item $src $target -Force
            Write-Host "  [OK] $dll" -ForegroundColor Green
        } else {
            Write-Host "  [MISSING] $dll" -ForegroundColor Red
        }
    }
}
Write-Host ""
Write-Host "Done!" -ForegroundColor Cyan
