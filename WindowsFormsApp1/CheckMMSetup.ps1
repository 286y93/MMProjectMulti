# MarkingMate 四顆雷射頭設定體檢腳本
#
# 檢查項目：
#   全域：
#     A. 主 IP 表        : <ProjectRoot>\Drivers\EMC6\DevIPAddress.ini（DEV0~DEV3 = CARD0~3 = MM1~4）
#     B. EMC6 共用目錄   : <ProjectRoot>\Drivers\EMC6\（含驅動 / cfg / 主表）
#     C. 共用 config.ini : <ProjectRoot>\config\config.ini
#     D. OCX 註冊        : ProgID MMMarkx64 / MMEditx64 / MMStatusx64
#   每顆 MM1~MM4：
#     1. 專案來源 IP 檔   : <ProjectRoot>\Drivers\EMC6_MMx\DevIPAddress.ini（DEV0 由主表同步）
#     2. 專案來源 config : <ProjectRoot>\config\config_MMx.ini
#     3. 專案來源 cfg 目錄: <ProjectRoot>\Drivers\EMC6_MMx\cfg\*.cfg
#     4. 部署目標 IP     : C:\Program Files (x86)\MarkingMate\Drivers\EMC6_MMx\DevIPAddress.ini
#     5. 部署目標 config : C:\Program Files (x86)\MarkingMate\config_MMx.ini
#     6. 部署目標 cfg    : C:\Program Files (x86)\MarkingMate\Drivers\EMC6_MMx\cfg\*.cfg
#     7. 鏡頭資料夾      : C:\Program Files (x86)\MarkingMate\Lens_MMx
#
# 用法：在專案根（WindowsFormsApp1\）執行
#   .\CheckMMSetup.ps1                # 檢查 4 顆
#   .\CheckMMSetup.ps1 -BoardCount 2  # 只檢查 MM1~MM2

param(
    [ValidateRange(1, 4)]
    [int]$BoardCount = 4
)

$ErrorActionPreference = 'Continue'

$ProjectRoot     = $PSScriptRoot
$MarkingMateRoot = 'C:\Program Files (x86)\MarkingMate'

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "MarkingMate 四雷射頭設定體檢" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "ProjectRoot     : $ProjectRoot"
Write-Host "MarkingMateRoot : $MarkingMateRoot"
Write-Host "BoardCount      : $BoardCount"
Write-Host ""

$problems = New-Object System.Collections.Generic.List[string]

function Test-FileNonEmpty($path) {
    return (Test-Path $path) -and ((Get-Item $path).Length -gt 0)
}

function Get-Dev0Ip($iniPath) {
    if (-not (Test-Path $iniPath)) { return $null }
    foreach ($line in Get-Content $iniPath) {
        $t = $line.Trim()
        if ($t -match '^DEV0\s*=\s*(.*)$') { return $Matches[1].Trim() }
    }
    return ''
}

function Get-DevIp($iniPath, $devIndex) {
    if (-not (Test-Path $iniPath)) { return $null }
    $pattern = "^DEV$devIndex\s*=\s*(.*)$"
    foreach ($line in Get-Content $iniPath) {
        $t = $line.Trim()
        if ($t -match $pattern) { return $Matches[1].Trim() }
    }
    return ''
}

# --- 主 IP 表 / EMC6 共用目錄 / 共用 config.ini ---
$srcMasterIp        = Join-Path $ProjectRoot "Drivers\EMC6\DevIPAddress.ini"
$srcSharedDriverDir = Join-Path $ProjectRoot "Drivers\EMC6"
$srcSharedConfig    = Join-Path $ProjectRoot "config\config.ini"

Write-Host "[全域]" -ForegroundColor Yellow

# A. 主 IP 表
if (Test-Path $srcMasterIp) {
    $masterMissing = @()
    for ($k = 0; $k -lt $BoardCount; $k++) {
        $ipK = Get-DevIp $srcMasterIp $k
        if ([string]::IsNullOrWhiteSpace($ipK)) {
            $masterMissing += "DEV$k"
        } else {
            Write-Host "  [OK]   主 IP 表 DEV$k → MM$($k + 1) / CARD$k = $ipK" -ForegroundColor Green
        }
    }
    if ($masterMissing.Count -gt 0) {
        Write-Host "  [FAIL] 主 IP 表缺值：$($masterMissing -join ', ')  ($srcMasterIp)" -ForegroundColor Red
        $problems.Add("主 IP 表缺值：$($masterMissing -join ', ')")
    }
} else {
    Write-Host "  [FAIL] 主 IP 表不存在：$srcMasterIp" -ForegroundColor Red
    $problems.Add("主 IP 表不存在：$srcMasterIp")
}

# B. EMC6 共用目錄
if (Test-Path $srcSharedDriverDir) {
    $emc6FileCount = (Get-ChildItem -Path $srcSharedDriverDir -File -Recurse -ErrorAction SilentlyContinue).Count
    Write-Host "  [OK]   EMC6 共用目錄存在     : $srcSharedDriverDir ($emc6FileCount 個檔)" -ForegroundColor Green
} else {
    Write-Host "  [FAIL] EMC6 共用目錄不存在  : $srcSharedDriverDir" -ForegroundColor Red
    $problems.Add("EMC6 共用目錄不存在：$srcSharedDriverDir")
}

# C. 共用 config.ini
if (Test-FileNonEmpty $srcSharedConfig) {
    Write-Host "  [OK]   共用 config.ini 存在  : $srcSharedConfig" -ForegroundColor Green
} else {
    Write-Host "  [WARN] 共用 config.ini 不存在或空 : $srcSharedConfig（初始化會跳過）" -ForegroundColor DarkYellow
}

Write-Host ""

# --- OCX 註冊狀態 ---
Write-Host "[OCX 註冊]" -ForegroundColor Yellow
foreach ($progId in 'MMEditx64.MMEditx64','MMMarkx64.MMMarkx64','MMStatusx64.MMStatusx64') {
    $type = [Type]::GetTypeFromProgID($progId)
    if ($type) {
        Write-Host "  [OK]   $progId  (CLSID $($type.GUID))" -ForegroundColor Green
    } else {
        Write-Host "  [FAIL] $progId  未註冊" -ForegroundColor Red
        $problems.Add("OCX 未註冊：$progId （請以系統管理員身分執行 $MarkingMateRoot\ELOCXRegister.exe）")
    }
}
Write-Host ""

# --- 各 MMx 設定 ---
for ($i = 1; $i -le $BoardCount; $i++) {
    $mm = "MM$i"
    Write-Host "[$mm]" -ForegroundColor Yellow

    $srcIp     = Join-Path $ProjectRoot     "Drivers\EMC6_$mm\DevIPAddress.ini"
    $srcCfg    = Join-Path $ProjectRoot     "config\config_$mm.ini"
    $srcCfgDir = Join-Path $ProjectRoot     "Drivers\EMC6_$mm\cfg"
    $destIp    = Join-Path $MarkingMateRoot "Drivers\EMC6_$mm\DevIPAddress.ini"
    $destCfg   = Join-Path $MarkingMateRoot "config_$mm.ini"
    $destCfgDir= Join-Path $MarkingMateRoot "Drivers\EMC6_$mm\cfg"
    $lensDir   = Join-Path $MarkingMateRoot "Lens_$mm"

    # 1. 專案來源 IP
    if (Test-Path $srcIp) {
        $ip = Get-Dev0Ip $srcIp
        if ([string]::IsNullOrWhiteSpace($ip)) {
            Write-Host "  [FAIL] 專案 IP 來源 DEV0 為空：$srcIp" -ForegroundColor Red
            $problems.Add("$mm 專案 IP 為空：$srcIp")
        } else {
            Write-Host "  [OK]   專案 IP 來源       : DEV0=$ip" -ForegroundColor Green
        }
    } else {
        Write-Host "  [FAIL] 專案 IP 來源不存在 : $srcIp" -ForegroundColor Red
        $problems.Add("$mm 缺專案 IP 檔：$srcIp")
    }

    # 2. 專案來源 config
    if (Test-FileNonEmpty $srcCfg) {
        Write-Host "  [OK]   專案 config 來源   : $srcCfg" -ForegroundColor Green
    } else {
        Write-Host "  [FAIL] 專案 config 來源缺失或為空 : $srcCfg" -ForegroundColor Red
        $problems.Add("$mm 缺專案 config：$srcCfg")
    }

    # 3. 專案來源 cfg 目錄（含 DEFAULT CARD、MultiCard、MultiSYN* 等多卡同步參數）
    if (Test-Path $srcCfgDir) {
        $srcCfgFiles = @(Get-ChildItem -Path $srcCfgDir -Filter '*.cfg' -File -ErrorAction SilentlyContinue)
        if ($srcCfgFiles.Count -gt 0) {
            Write-Host "  [OK]   專案 cfg 目錄來源   : $srcCfgDir ($($srcCfgFiles.Count) 個 .cfg)" -ForegroundColor Green
        } else {
            Write-Host "  [FAIL] 專案 cfg 目錄無檔案 : $srcCfgDir" -ForegroundColor Red
            $problems.Add("$mm 專案 cfg 目錄無 .cfg：$srcCfgDir")
        }
    } else {
        Write-Host "  [FAIL] 專案 cfg 目錄不存在 : $srcCfgDir" -ForegroundColor Red
        $problems.Add("$mm 缺專案 cfg 目錄：$srcCfgDir")
    }

    # 4. 部署目標 IP
    if (Test-Path $destIp) {
        $deployIp = Get-Dev0Ip $destIp
        $tag = if ([string]::IsNullOrWhiteSpace($deployIp)) { '(空)' } else { $deployIp }
        Write-Host "  [OK]   部署 IP 目標已存在  : DEV0=$tag" -ForegroundColor Green
    } else {
        Write-Host "  [WARN] 部署 IP 目標不存在  : $destIp  （初始化時會自動建立）" -ForegroundColor DarkYellow
    }

    # 5. 部署目標 config
    if (Test-FileNonEmpty $destCfg) {
        Write-Host "  [OK]   部署 config 已存在 : $destCfg" -ForegroundColor Green
    } else {
        Write-Host "  [WARN] 部署 config 不存在 : $destCfg  （初始化時會自動建立）" -ForegroundColor DarkYellow
    }

    # 6. 部署目標 cfg 目錄
    if (Test-Path $destCfgDir) {
        $destCfgFiles = @(Get-ChildItem -Path $destCfgDir -Filter '*.cfg' -File -ErrorAction SilentlyContinue)
        Write-Host "  [OK]   部署 cfg 目錄已存在 : $destCfgDir ($($destCfgFiles.Count) 個 .cfg)" -ForegroundColor Green
    } else {
        Write-Host "  [WARN] 部署 cfg 目錄不存在 : $destCfgDir  （初始化時會自動建立）" -ForegroundColor DarkYellow
    }

    # 7. 鏡頭資料夾
    if (Test-Path $lensDir) {
        $lensFiles = @('default.CFG','default.cor','default.lens','default.scx','default.stf')
        $missing = $lensFiles | Where-Object { -not (Test-Path (Join-Path $lensDir $_)) }
        if ($missing.Count -eq 0) {
            Write-Host "  [OK]   鏡頭資料夾完整     : $lensDir" -ForegroundColor Green
        } else {
            Write-Host "  [WARN] 鏡頭資料夾缺檔     : $($missing -join ', ')  ($lensDir)" -ForegroundColor DarkYellow
            $problems.Add("$mm 鏡頭資料夾缺檔：$($missing -join ', ')")
        }
    } else {
        Write-Host "  [FAIL] 鏡頭資料夾不存在   : $lensDir" -ForegroundColor Red
        $problems.Add("$mm 缺 Lens 資料夾：$lensDir")
    }

    Write-Host ""
}

# --- 結論 ---
Write-Host "========================================" -ForegroundColor Cyan
if ($problems.Count -eq 0) {
    Write-Host "全部通過。可執行程式並按「初始化」。" -ForegroundColor Green
} else {
    Write-Host "發現 $($problems.Count) 個阻擋項：" -ForegroundColor Red
    foreach ($p in $problems) { Write-Host "  - $p" -ForegroundColor Red }
    exit 1
}
Write-Host "========================================" -ForegroundColor Cyan
