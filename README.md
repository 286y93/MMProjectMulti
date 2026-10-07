# MarkingMate Multi-Board 雷射打標系統

基於 MarkingMate SDK 的多板雷射打標程式，可同時控制最多 4 張 EMC6 控制卡（MM1~MM4），支援 GUI、命令列與 daemon/client 三種模式。

## 環境需求
- Windows x64、.NET Framework 4.8
- MarkingMate 已安裝於 `C:\Program Files (x86)\MarkingMate`
- 建置平台必須為 **x64**，輸出檔為 `MarkingMate.exe`

```bash
MSBuild WindowsFormsApp1\MarkingMateMulti.csproj /p:Configuration=Debug /p:Platform=x64
```

## 執行模式

| 模式 | 啟動方式 | 說明 |
|------|----------|------|
| GUI | 不帶參數 | 單一實例 |
| CLI | 帶任意參數 | 執行單一命令後結束 |
| Daemon | `--daemon` | 常駐程序，一次初始化全部板，透過 HTTP 接收命令 |
| Client | `--client` | 將命令 POST 到 daemon，不直接操作 SDK |

SDK 不允許多個程序同時初始化 OCX。要讓多塊板並行動作，請先啟動 daemon，再以 client 派送命令。daemon 運行中時，一般 CLI 命令會自動改走 client 模式。

## 命令列參數

### 基本
| 參數 | 說明 | 預設 |
|------|------|------|
| `--help`, `-h`, `/?` | 顯示說明 | |
| `--board <0-3>`, `-b` | 板號 | `0` |
| `--config <path>`, `-c` | 配置路徑 | `/cfg_config_MM{board+1}` |
| `--workspace <mm>`, `-w`, `--workspace-w` | 工作區寬（未指定高時，高同寬） | `150` |
| `--workspace-h <mm>` | 工作區高 | 同寬 |

### 圖形
| 參數 | 說明 |
|------|------|
| `--line <x1,y1,x2,y2>`, `-l` | 單一線段 |
| `--lines "<x1,y1,x2,y2;...>"` | 多條線段，以 `;` 分隔 |
| `--dxf <path>`, `-d` | 載入 DXF |
| `--qrcode <內容>`, `-qr` | QR Code（位置固定於鏡頭中心） |
| `--qr-width <mm>` / `--qr-height <mm>` | QR 尺寸，預設 10 |
| `--qr-invert` | QR 反相 |
| `--qr-whitebg` | 白底反相 QR（白底矩形 + 反相 QR 雙圖層） |
| `--qr-border <cells>` | 白底 QR 外框單元數（白底模式預設 2） |

### 雷射參數（未指定則使用預設值）
| 參數 | 說明 |
|------|------|
| `--power <0-100>`, `-p` | 功率 % |
| `--speed <mm/s>`, `-s` | 速度 |
| `--freq <kHz>`, `-f` | 頻率 |
| `--pulse-width <val>`, `--pw` | 脈波寬度 |
| `--repeat <n>`, `-r` | 雷射次數 |
| `--wobble-width <mm>`, `--wobble` | 擺動線寬，預設 0.5 |
| `--wobble-overlap <%>` | 擺動重疊率，預設 50 |
| `--wobble-speed <mm/s>` | 擺動速度，預設 5026.55 |

白底 QR 各圖層參數：

| 圖層 | 參數 | 預設 |
|------|------|------|
| QR 資料層 | `--qr-power` `--qr-speed` `--qr-freq` `--qr-pw` | 90 / 1200 / 80 / 30（power、speed 未帶時沿用 `--power`、`--speed`） |
| 白底矩形層 | `--rect-power` `--rect-speed` `--rect-freq` `--rect-pw` | 100 / 800 / 80 / 250 |

### AprilTag（tagStandard41h12）
底部矩形 + 反相 AprilTag 雙圖層，tag 中心固定於 (0,0)。tag 為 9×9 格，中間是白色偵測框、外面一圈黑框，最外圈也是資料格。矩形蓋住整個 tag 先打；AprilTag 層為反相，只打白格，黑格保留矩形底色。GUI 對應「8. AprilTag」頁籤。

| 參數 | 說明 | 預設 |
|------|------|------|
| `--apriltag <id>` | Tag 編號（0-2114） | |
| `--tag-size <mm>` | 白色偵測框外緣邊長（AprilTag 慣例）；每格 = 大小/5，tag 總寬 = 大小 × 1.8 | `20` |
| `--tag-target <rect\|tag\|all>` | `rect` 只打矩形、`tag` 只打 AprilTag、`all` 矩形 + AprilTag | `all` |
| `--tag-rect-extra <mm>` | 矩形額外加大量（矩形邊長 = tag 總寬 + 此值） | `0` |
| `--tag-power` `--tag-speed` `--tag-freq` `--tag-pw` | AprilTag 層雷射參數（power、speed 未帶時沿用 `--power`、`--speed`） | 90 / 1200 / 80 / 30 |
| `--rect-power` `--rect-speed` `--rect-freq` `--rect-pw` | 矩形層雷射參數（與白底 QR 共用） | 100 / 800 / 80 / 250 |

### 執行控制
| 參數 | 說明 |
|------|------|
| `--mark`, `-m` | 執行打標 |
| `--preview <outline\|full>` | 紅光預覽（不出雷射），預設 `full` |
| `--preview-speed <mm/s>` | 預覽速度 |
| `--preview-time <秒>` | 預覽時間，預設 15 |
| `--daemon` | 啟動 daemon |
| `--client` | 以 client 模式送出命令 |
| `--port <N>` | daemon port，預設 `19527` |
| `--shutdown` | 搭配 `--client`，關閉 daemon |

### 範例
```bash
# 板 0 畫線並打標
MarkingMate.exe --board 0 --line 0,0,50,50 --mark

# 載入 DXF 並指定雷射參數
MarkingMate.exe --board 0 --dxf "File\test.dxf" --power 50 --speed 800 --freq 20 --pw 5 --repeat 1 --mark

# 長寬不同的工作區
MarkingMate.exe --board 0 --workspace-w 200 --workspace-h 120 --dxf "File\上翼板-2.dxf" --mark

# 外框紅光預覽
MarkingMate.exe --board 0 --dxf "File\test.dxf" --mark --preview outline

# QR Code
MarkingMate.exe --board 0 --qrcode "Hello World" --qr-width 10 --qr-height 10 --mark

# 白底反相 QR
MarkingMate.exe --board 1 --qrcode "SN-0001" --qr-whitebg --mark

# AprilTag 5 號、大小 20mm：矩形 + AprilTag / 只打矩形 / 只打 AprilTag
MarkingMate.exe --board 0 --apriltag 5 --tag-size 20 --mark
MarkingMate.exe --board 0 --apriltag 5 --tag-size 20 --tag-target rect --mark
MarkingMate.exe --board 0 --apriltag 5 --tag-size 20 --tag-target tag --mark
```

### Daemon / Client
```bash
start "" MarkingMate.exe --daemon
MarkingMate.exe --client --board 0 --line 0,0,50,50 --mark
MarkingMate.exe --client --board 1 --qrcode "TEST" --mark
MarkingMate.exe --client --shutdown
```

Daemon 僅監聽本機，HTTP 端點：

| 端點 | 方法 | 說明 |
|------|------|------|
| `/cmd` | POST | body 為命令字串（同 CLI 參數），回傳 `{"exitCode":N,"logs":"..."}` |
| `/health` | GET | 健康檢查 |
| `/shutdown` | POST | 關閉 daemon |

### 結束代碼
| 代碼 | 說明 |
|------|------|
| `0` | 成功 |
| `1` | 初始化失敗（含 IP 主表未填、部署權限不足） |
| `2` | 繪圖失敗 |
| `3` | 打標失敗 / daemon 逾時 |
| `4` | 參數錯誤 / 板未初始化 |
| `5` | 板忙碌中（daemon） |
| `6` | 無法連線到 daemon（client） |
| `7` | client 例外 |
| `9` | 控制卡未連接，請稍後重試（daemon） |
| `10` | 雷射致能失敗，未打標（daemon） |
| `-2` | 該板已被其他程序使用 |
| `-3` | daemon 已在執行 |

## 雷射頭 IP 設定
IP 主表：`WindowsFormsApp1\Drivers\EMC6\DevIPAddress.ini`，`DEV0`~`DEV3` 依序對應 MM1~MM4。

初始化時會自動：
1. 驗證主表已填入所需板的 IP，缺值即中止
2. 將 IP 同步到各 `EMC6_MMx\DevIPAddress.ini`
3. 部署 `Drivers\EMC6\`、`config\config.ini` 與 `cfg\*.cfg` 到 MarkingMate 安裝目錄

IP 只能在 GUI「1. 連接設定」頁編輯並儲存。

## 專案結構
```
WindowsFormsApp1/
├── Program.cs               # 進入點、模式判斷、client 模式
├── CommandLineArgs.cs       # 參數解析
├── MarkingMateDaemon.cs     # daemon HTTP 服務
├── Form1.cs                 # 初始化、部署、CLI/daemon 執行、打標
├── Form1.*.cs               # 依功能拆分的 partial（DXF、QR、雷射參數、CLI 編輯器等）
├── Drivers/                 # EMC6 驅動與雷射頭 cfg
├── config/                  # config.ini
└── MarkingMateMulti.csproj
```

## 疑難排解
詳見 [故障排除完整指南.md](WindowsFormsApp1/故障排除完整指南.md)。
