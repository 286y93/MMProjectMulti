# Simple MarkingMate DLL Copy Script

$source = "C:\Program Files (x86)\MarkingMate\sdk\ocx\CSharp_example\VS2017\LoadFile\LoadFile\bin\x64\Release"
$projectDir = Split-Path -Parent $MyInvocation.MyCommand.Path

$targets = @(
    "$projectDir\bin\x64\Debug",
    "$projectDir\bin\x64\Release"
)

Write-Host "Copying MarkingMate DLL files..." -ForegroundColor Green
Write-Host "Source: $source"

if (-not (Test-Path $source)) {
    Write-Host "ERROR: Source path not found!" -ForegroundColor Red
    Write-Host "Please make sure MarkingMate is installed" -ForegroundColor Yellow
    pause
    exit 1
}

$dllFiles = Get-ChildItem -Path $source -Filter "*.dll"

if ($dllFiles.Count -eq 0) {
    Write-Host "ERROR: No DLL files found in source directory!" -ForegroundColor Red
    pause
    exit 1
}

Write-Host "Found $($dllFiles.Count) DLL files"

foreach ($target in $targets) {
    Write-Host ""
    Write-Host "Processing: $target" -ForegroundColor Yellow

    if (-not (Test-Path $target)) {
        Write-Host "  Creating directory..." -ForegroundColor Gray
        New-Item -ItemType Directory -Path $target -Force | Out-Null
    }

    $copiedCount = 0
    foreach ($dll in $dllFiles) {
        try {
            Copy-Item -Path $dll.FullName -Destination $target -Force
            Write-Host "  OK: $($dll.Name)" -ForegroundColor Green
            $copiedCount++
        }
        catch {
            Write-Host "  FAIL: $($dll.Name)" -ForegroundColor Red
        }
    }

    Write-Host "  Done: copied $copiedCount / $($dllFiles.Count) files" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "All DLL files copied successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "DLL List:" -ForegroundColor Cyan
foreach ($dll in $dllFiles) {
    Write-Host "  - $($dll.Name)"
}

Write-Host ""
pause
