# MarkingMate OCX Registration and Diagnostic Tool
# Run as Administrator

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "MarkingMate OCX Registration Tool" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if running as Administrator
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if (-not $isAdmin) {
    Write-Host "ERROR: This script must be run as Administrator!" -ForegroundColor Red
    Write-Host "Please right-click and select 'Run as Administrator'" -ForegroundColor Yellow
    Write-Host ""
    pause
    exit 1
}

Write-Host "[OK] Running with Administrator privileges" -ForegroundColor Green
Write-Host ""

# Define DLL path
$dllPath = "C:\Program Files (x86)\MarkingMate\sdk\ocx\CSharp_example\VS2017\LoadFile\LoadFile\bin\x64\Release"

# Check if DLL files exist
Write-Host "Checking DLL files..." -ForegroundColor Yellow
$dlls = @("MMEditx64Lib.dll", "MMMarkx64Lib.dll", "MMStatusx64Lib.dll")
$allExist = $true

foreach ($dll in $dlls) {
    $fullPath = Join-Path $dllPath $dll
    if (Test-Path $fullPath) {
        Write-Host "  [OK] $dll found" -ForegroundColor Green
    } else {
        Write-Host "  [MISSING] $dll NOT FOUND" -ForegroundColor Red
        $allExist = $false
    }
}

if (-not $allExist) {
    Write-Host ""
    Write-Host "ERROR: Some DLL files are missing!" -ForegroundColor Red
    Write-Host "Please reinstall MarkingMate or check the installation path" -ForegroundColor Yellow
    pause
    exit 1
}

Write-Host ""
Write-Host "Registering DLL files..." -ForegroundColor Yellow

foreach ($dll in $dlls) {
    $fullPath = Join-Path $dllPath $dll
    Write-Host "  Registering $dll..." -ForegroundColor Cyan

    try {
        $result = Start-Process -FilePath "regsvr32.exe" -ArgumentList "/s `"$fullPath`"" -Wait -PassThru -NoNewWindow

        if ($result.ExitCode -eq 0) {
            Write-Host "  [SUCCESS] $dll registered" -ForegroundColor Green
        } else {
            Write-Host "  [FAILED] $dll registration failed (Exit code: $($result.ExitCode))" -ForegroundColor Red
        }
    }
    catch {
        Write-Host "  [ERROR] Failed to register $dll : $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "Verifying registration..." -ForegroundColor Yellow

# Check registry
$registryChecks = @{
    "MMEditx64" = "HKCR:\MMEditx64.MMEditx64"
    "MMMarkx64" = "HKCR:\MMMarkx64.MMMarkx64"
    "MMStatusx64" = "HKCR:\MMStatusx64.MMStatusx64"
}

foreach ($name in $registryChecks.Keys) {
    $regPath = $registryChecks[$name]
    if (Test-Path $regPath) {
        Write-Host "  [OK] $name is registered" -ForegroundColor Green
    } else {
        Write-Host "  [WARNING] $name is NOT registered in expected location" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Registration process completed!" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Restart Visual Studio" -ForegroundColor White
Write-Host "2. Rebuild your project (Clean + Build)" -ForegroundColor White
Write-Host "3. Run your application" -ForegroundColor White
Write-Host ""
pause
