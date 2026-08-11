# CLI 模式（命令提示字元）注意事項

> 本文件整理 `MarkingMate.exe` 命令列模式的邏輯概念與使用方式。
> GUI 模式請見 [故障排除完整指南.md](故障排除完整指南.md)。
> 完整命令列使用手冊請見 [命令列模式使用說明.txt](../命令列模式使用說明.txt)。
>
> **最後同步日期：2026-08-07**（`--qr-whitebg` 兩圖層雷射參數改為可用 `--qr-*` / `--rect-*` CLI 覆寫；exe `WhiteBgQRParams` 預設：QR 25×25/border 2/power 90/speed 1200/freq 80/pw 30、矩形 power 100/speed 800/freq 80/pw 250）

---

## 📋 目錄

- [邏輯概念](#邏輯概念)
- [使用方式](#使用方式)
- [一定要知道的事](#一定要知道的事)
- [多開行為對照表](#多開行為對照表)
- [退出代碼](#退出代碼)
- [參數小陷阱](#參數小陷阱)
- [仍會跳出 GUI 的情況](#仍會跳出-gui-的情況)
- [常見錯誤訊息](#常見錯誤訊息)
- [相關檔案](#相關檔案)

---

## 邏輯概念

### 兩種使用模式

CLI 有兩條獨立的執行路徑，**目標需求決定要用哪一條**：

```
┌───────────────────────────────────────────────────────┐
│ 模式 A：傳統單命令 CLI（單 process）                    │
│                                                         │
│   一次 CLI 呼叫 = 一個 process = 一塊板 = 一筆命令      │
│   process 跑完後自動退出                               │
│                                                         │
│   ⚠️ 不能多 process 並行（SDK 限制，見下節）            │
└───────────────────────────────────────────────────────┘

┌───────────────────────────────────────────────────────┐
│ 模式 B：Daemon + Client（推薦給多板並行 / 網頁整合）     │
│                                                         │
│   先跑一個 daemon 常駐 process（init 全 4 板）          │
│   後續所有命令以 client 模式 POST 到 daemon            │
│   daemon 在同一 process 內派發到不同板                  │
│                                                         │
│   ✓ 多板並行 OK（SDK 支援 in-process parallel）         │
│   ✓ 網頁可直接 fetch() 呼叫                            │
└───────────────────────────────────────────────────────┘
```

### SDK 並行限制（為什麼要 daemon）

實機驗證結果：

| 並行類型 | SDK 行為 |
|---|---|
| **同一個 process 內**多板同時 `StartMarking` | ✅ 支援，4 板紅光可同時亮 |
| **不同 process** 各自 init OCX | ❌ 第二個 process 跳「Please initial MMMark_1 OCX first!」對話框 |

SDK 似乎用了 process-global 的鎖（COM 全域旗標或類似機制），第一個 process 取得 OCX「擁有權」後，第二個 process init 同一個 OCX 會失敗。

→ **要做多板並行，必須走 daemon 模式**（單一 process 服務多個 client 請求）。

### Daemon 架構

```
[CLI client] ──┐
[CLI client] ──┼──HTTP POST──> [Daemon] ──in-process──> [SDK / Board 0..3]
[網頁 fetch]  ──┘    localhost:19527
```

- Daemon 啟動時依機台主表 `EMC6\DevIPAddress.ini` 的 `[DEVICE]`（`DEV0` 起連續有值的 `DEVn` 數量）自動偵測軸卡數量，一次 init 該數量的板（OCX、配置部署、MarkStandBy）；例如只有 `DEV0` 有值 → 只 init 1 塊
- HttpListener 只 bind `127.0.0.1`（網段其他機器無法存取，這是設計上的安全保護）
- 每個 client 命令 → daemon 在 UI thread 上派發到 target 板，完成後回 ExitCode + logs
- 同板互斥：若 board N 還在跑前一條命令，第二個 client 立即拿到 `exitCode=5, "board N busy"`
- 不同板：完全並行，前後端皆 async

### Init 邏輯（兩個模式共用）

CLI 每次啟動（包含 daemon）都會跑這個流程：

1. **配置部署 / 驗證**
   - **軸卡數量與各軸卡 IP＝機台主表 `Drivers\EMC6\DevIPAddress.ini` 的 `[DEVICE]`**（`DEV0~DEVn`，由 `MultiMMSetting.exe` 維護）。本程式**只讀主表、不寫主表**，也**不提供「儲存IP」寫入**。
   - 驗證：要啟用的各板（`DEV0~DEV{N-1}`）必須都有值；否則報錯，請以 `MultiMMSetting.exe` 設定。
   - **對應軸卡**：把主表 `DEV{i}` 同步寫到各 `EMC6_MM{i+1}\DevIPAddress.ini` 的 `DEV0`（SDK 初始化各板實際讀此檔）；值已相同則跳過。
   - 部署把 `Drivers\EMC6\` 與各 `Drivers\EMC6_MMx\`（驅動 binary、cfg）複製到 `C:\Program Files (x86)\MarkingMate\`，但**一律排除 `DevIPAddress.ini`**——不覆蓋主表（避免把某台的 IP 佈到別台）。
   - 偵測到其他 MarkingMate 實例正在跑 → **跳過寫檔 / 同步**，只驗證主表 `DEV0~DEV{N-1}`（避免覆寫對方還在用的檔案）
2. **OCX 初始化**
   - 依主表 `EMC6\DevIPAddress.ini` 的 `[DEVICE]` 自動偵測軸卡數量：從 `DEV0` 起算連續有值的 `DEVn` 數量，遇第一個空值即停（SDK 要求 0..N-1 連續 init）
   - 只 init 偵測到的 N 塊（`m_MMMark[0..N-1]` + `m_MMEdit[0..N-1]`），順序固定 0 → 1 → …（SDK 要求）
   - 模式 A 指定的 `--board M` 若 `M ≥ N`（`DEV{M}` 未設定）→ 直接報錯 ExitCode=1，不再空等連線
   - GUI「連接設定」頁的板數微調器（`numBoardCount`）也會在讀 IP 時自動帶入偵測值，仍可手動覆寫
   - ⚠️ 注意：偵測反映主表「宣告的軸卡數」。若機台被 `MultiMMSetting.exe` 設成 4 個 `DEVn` 都有值，即使實體只插 1 張卡也會偵測為 4；「依實體連上幾張卡」需另以 `IsCardConnect()` 判定
   - 同 process 內已 init 過則跳過

### Config Path 自動推導

舊 CLI 預設 `--config /cfg_config_MM1`，不管 `--board` 是多少都用這個 → 板 1/2/3 會配錯。

現在邏輯：

- 使用者明確帶 `--config X` → 用 X
- 沒帶 `--config` → 依 `--board N` 自動推導為 `/cfg_config_MM{N+1}`

| `--board` | 自動 `--config` |
|---|---|
| `0` | `/cfg_config_MM1` |
| `1` | `/cfg_config_MM2` |
| `2` | `/cfg_config_MM3` |
| `3` | `/cfg_config_MM4` |

---

## 使用方式

### 模式 A：傳統單命令 CLI

適用情境：腳本只需要操作**一塊板**、不在乎並行、執行完就走。

```cmd
:: 基本：板 0 畫一條線並打標
MarkingMate.exe --board 0 --line 0,0,50,50 --mark

:: 板 1（會自動用 /cfg_config_MM2）
MarkingMate.exe --board 1 --line 0,0,50,50 --mark

:: QR Code + 紅光全路徑預覽 5 秒（位置固定為鏡頭中心 0,0，不接受 --qr-x/--qr-y）
MarkingMate.exe --board 0 --qrcode "TEST" --qr-width 10 --qr-height 10 ^
                --mark --preview full --preview-time 5

:: 白底反相 QR（白底矩形 + 反相 QR 雙圖層）：只需帶內容，其餘白底參數後端寫死
MarkingMate.exe --board 0 --qrcode "ABC123" --qr-whitebg --mark

:: 載入 DXF + 雷射參數 + 自訂工作區
MarkingMate.exe --board 0 --dxf "File\test.dxf" --workspace-w 200 --workspace-h 120 ^
                --power 50 --speed 800 --freq 20 --pw 5 --mark
```

⚠️ **不能多 process 並行**（SDK 鎖）。多板並行請用模式 B。

### 模式 B：Daemon + Client（推薦給多板並行 / 網頁整合）

#### B-1. 啟動 daemon（背景常駐）

```cmd
:: 開背景 window，daemon 一直跑直到收 /shutdown
start "" MarkingMate.exe --daemon

:: 或自訂 port（預設 19527）
start "" MarkingMate.exe --daemon --port 28800
```

啟動成功時 stdout 應看到：

```
[Daemon] 啟動中，初始化全部 4 塊板...
[Board 1] OCX 初始化完成 (TARGET)
[Board 2] OCX 初始化完成 (prereq)
[Board 3] OCX 初始化完成 (prereq)
[Board 4] OCX 初始化完成 (prereq)
[Daemon] 就緒，listening on http://localhost:19527/
[Daemon] Endpoints: POST /cmd, POST /shutdown, GET /health
```

#### B-2. 健康檢查

```cmd
curl http://localhost:19527/health
:: → MarkingMate daemon OK
```

#### B-3. CLI client 發命令

```cmd
:: 板 0 紅光預覽 10 秒（多線段範例，命令提示頁籤產生的線段都是多段）
MarkingMate.exe --client --board 0 --lines "0,0,50,50;10,10,40,40;-20,20,20,-20" ^
                --mark --preview full --preview-time 10

:: 同時板 1 跑 QR 預覽 8 秒（位置固定 0,0，不會等板 0 結束）
MarkingMate.exe --client --board 1 --qrcode "TEST" --qr-width 15 --qr-height 15 ^
                --mark --preview full --preview-time 8

:: 板 2 非 QR 內容 + 自訂工作區（daemon 模式也支援，QR 內容會忽略 workspace）
MarkingMate.exe --client --board 2 --line 0,0,80,80 --workspace-w 200 --workspace-h 120 --mark
```

Client 收到的回應（JSON）：

```json
{"exitCode":0,"logs":"[Board 1] added 1 line(s)\n[Board 1] StartMarking(3) OK\n[Board 1] preview done @10010ms\n"}
```

#### B-4. 網頁端整合

```html
<button onclick="sendCmd()">板 2 預覽 QR</button>
<script>
async function sendCmd() {
  const r = await fetch('http://localhost:19527/cmd', {
    method: 'POST',
    headers: { 'Content-Type': 'text/plain' },
    body: '--board 2 --qrcode "ABC-123" --qr-width 15 --qr-height 15 --mark --preview full --preview-time 5'
  });
  const result = await r.json();
  console.log('ExitCode:', result.exitCode);
  console.log('Logs:', result.logs);
}
</script>
```

CORS 注意事項：

- Daemon 回 `Access-Control-Allow-Origin: *`，所有 origin 都能呼叫
- 若網頁是 HTTPS（`https://...`），瀏覽器會擋 HTTP localhost 呼叫（mixed content）。網頁本身要走 HTTP 才能 call
- Daemon 只 bind `127.0.0.1`，網段其他機器無法存取

#### B-5. 停止 daemon

```cmd
:: 經由 client
MarkingMate.exe --client --shutdown

:: 或直接 curl
curl -X POST http://localhost:19527/shutdown
```

### 模式選擇對照表

| 需求 | 推薦方案 |
|---|---|
| 單 board、單筆命令、執行完即走 | 模式 A |
| 多 board 同時動作 | 模式 B |
| 網頁要呼叫 | 模式 B |
| 互動式測試（人手按按鈕） | GUI「命令提示」頁籤（in-process parallel） |
| 批次腳本連發多個動作 | 模式 B（先啟 daemon、再連發 client） |

### Daemon HTTP API 摘要

| Endpoint | Method | Body | 回應 |
|---|---|---|---|
| `/` 或 `/health` | GET | – | `text/plain` `MarkingMate daemon OK` |
| `/cmd` | POST | CLI args 字串（text/plain），例如 `--board 1 --line 0,0,50,50 --mark` | `application/json` `{"exitCode":N,"logs":"..."}` |
| `/shutdown` | POST | – | `text/plain` + 立即關閉 daemon |

CORS preflight (`OPTIONS`) 已支援。

---

## 一定要知道的事

### 1. 必須以「系統管理員身分」執行

- 初始化會部署整個 `Drivers\EMC6\` 到 `C:\Program Files (x86)\MarkingMate\`
- 一般使用者寫不進去 → `Error: 部署失敗 寫入權限不足` → ExitCode = 1
- **終端機本身**也要先以「系統管理員身分執行 cmd / PowerShell」
- 模式 A 與 daemon 啟動都適用；**client 模式不需要**（只是 HTTP 呼叫，不碰檔案系統與 SDK）

### 2. IP 必須先用 MultiMMSetting.exe 設定好

- 本程式（GUI / CLI）**都不能編輯或寫入主表 IP**；「連接設定」頁的 IP 欄位只是**唯讀顯示**（已移除「儲存IP」按鈕）。
- **軸卡數量與各軸卡 IP 的唯一來源＝主表 `Drivers\EMC6\DevIPAddress.ini` 的 `[DEVICE]`（`DEV0~DEVn`）**，由 `MultiMMSetting.exe` 維護；本程式只讀主表、初始化時把 `DEV{i}` 同步到各 `EMC6_MMx\DEV0`，部署時排除 `DevIPAddress.ini` 不覆蓋主表。
- 要啟用的板其主表 `DEVn` 為空 → 部署 / 驗證失敗 → ExitCode = 1。
- 對應關係（固定）：

  | 主表 | → 板 / CARD | 同步到 |
  |---|---|---|
  | `DEV0` | MM1 / CARD0 | `EMC6_MM1\DEV0` |
  | `DEV1` | MM2 / CARD1 | `EMC6_MM2\DEV0` |
  | `DEV2` | MM3 / CARD2 | `EMC6_MM3\DEV0` |
  | `DEV3` | MM4 / CARD3 | `EMC6_MM4\DEV0` |

- 第一次使用流程：先用 `C:\Program Files (x86)\MarkingMate\MultiMMSetting.exe` 設定軸卡 IP → 之後才能用本程式 GUI / CLI

### 3. 必須從 cmd / PowerShell 啟動才看得到輸出

```cmd
:: ✓ 看得到輸出
cmd> MarkingMate.exe --board 0 --line 0,0,50,50 --mark

:: ✗ 看不到輸出（雙擊執行）
```

[Program.cs](Program.cs) 用 `AttachConsole(ATTACH_PARENT_PROCESS)` 接到父終端機。雙擊或從 Explorer 啟動沒有父終端機可以接，所有 `Console.WriteLine` 都看不到（程式仍會跑）。

### 4. SDK 是 STA + 單 process 模型

- 模式 A：一個 process 處理完一筆命令就退出
- 模式 B：daemon 是**唯一**接觸 SDK 的 process，client 只是 HTTP 中介
- 千萬不要試圖把模式 A 同時對多個 board 跑 — 第二個 process 一定會撞 OCX 初始化錯誤對話框

---

## 多開行為對照表

| 場景 | 結果 |
|---|---|
| GUI 模式重複開 | 第二個 → MessageBox「程式已執行」→ ExitCode = -1 |
| 模式 A 同 `--board 0` 開兩次 | 第二個 → stderr「Board 0 is already in use」→ ExitCode = -2 |
| 模式 A 不同 `--board` 同時開 | ❌ 第二個會跳 OCX 初始化錯誤對話框（SDK 限制） |
| **daemon 運行中又跑模式 A** | 自動轉發成 client 派工（stderr 印「自動改以 client 模式派工」），不會自行 init OCX、不會撞 SDK 鎖 |
| Daemon 開兩次 | 第二個 → stderr「已有 daemon 在 port N 運行」→ ExitCode = -3 |
| Client 模式（並行多個） | ✅ 隨便開幾個都行，並行 fire 到 daemon |
| Client 同板連發 | 第二個立即拿到 `exitCode=5, "board N busy"` |

**Mutex 名稱**：

- GUI = `MarkingMateMulti_SingleInstance`
- 模式 A = `MarkingMateMulti_Board{N}`
- Daemon = `MarkingMateMulti_Daemon`
- Client = 無 mutex

---

## 退出代碼

| Code | 意義 | 來源 |
|---|---|---|
| `0` | 成功 | 模式 A / Daemon spec 完成 |
| `1` | 初始化失敗（主表 IP 缺值、部署權限不足、OCX 異常） | 共用 |
| `2` | 繪圖失敗（DXF 載入、AddLine、AddBarcode 錯誤） | 模式 A |
| `3` | 打標 / 預覽啟動失敗（`StartMarking` 回非 0、timeout） | 共用 |
| `4` | 參數錯誤（spec 沒有 `--board` / 內容） | 共用 |
| `5` | 板忙碌（同板已有命令在跑） | Daemon |
| `6` | Client 無法連線 daemon | Client |
| `7` | Client 例外 | Client |
| `-1` | 一般錯誤、GUI 重複執行 | Program.cs |
| `-2` | 同 board 已在執行（模式 A Mutex 未取得） | Program.cs |
| `-3` | Daemon 已在運行（重複 `--daemon`） | Program.cs |

> **daemon 運行中自動轉發**：偵測到 daemon 在跑時，模式 A（漏帶 `--client`）會**自動改以 client 派工**，回傳的即是 client 的結果與 ExitCode（0/3/6/7…），不再自行 init OCX，避免撞 SDK 鎖。

---

## 參數小陷阱

1. **`--mark` 是必須的**
   少了它 → 只載入 / 繪製，不會真的打標（也不會預覽）

2. **`--preview` 必須搭配 `--mark`**
   - `outline` / `box` = 外框預覽
   - `full` / `path` = 全路徑預覽（預設）

3. **`--config` 通常不用帶**
   依 `--board` 自動推導對應的 `/cfg_config_MM{N+1}`。要覆寫才帶。

4. **參數名要小寫**（`--Board` 不行）
   但值（DXF 路徑、QR 字串）保留大小寫

5. **線段座標自動置中**
   多條 `--line` / `--lines` 會整體置中到工作區原點，跟 DXF 行為一致

6. **座標原點偵測**
   - 任一座標為負 → 視為「中心原點座標」，不做平移
   - 全部非負 → 視為「左下角原點座標」自動用 `halfW/halfH` 平移

7. **預覽時間預設 15 秒**：用 `--preview-time <秒>` 調整

8. **`--workspace-w` / `--workspace-h`（非 QR 內容）**
   - 模式 A：建構子直接套用，影響 `SetDesktopSize`、DXF 縮放、line corner→center 平移
   - 模式 B（daemon）：client 若帶 `--workspace-w/h`，daemon 會用 spec 的值做這次命令的 SetDesktopSize 與 halfW/halfH；**沒帶**就沿用 daemon UI 的 workspace 設定。不會把 daemon UI 改掉。
   - **QR 內容會完全忽略 workspace**（QR 位置固定 0,0，且不做工作範圍平移）

9. **QR Code 限制與旗標**
   - **位置固定為鏡頭中心 (0,0)**：CLI 不接受 `--qr-x` / `--qr-y`，要 QR 就只能定在原點。要移位請改用 GUI 的 QR 載入頁面。
   - **不會套用 wobble（線條寬度）**：EMC6 對 QR 物件做 `SetWobble` 會回 `Unknown Commands=127` 對話框；CLI/daemon 的 wobble loop 偵測到 QR 內容會跳過。要加粗 QR 請改 `--qr-width` / `--qr-height`。
   - **`--qr-invert` 反相**（黑白互換）：旗標型參數，不吃下一個 token。內部呼叫 `m_MMEdit.SetBarcodeInvert(name, 1)`（SDK 對 QR 安全，與 SetWobble 不同）。範例：`--qrcode "INV" --qr-width 15 --qr-height 15 --qr-invert --mark`。
   - **容錯等級固定 LOW**：CLI/daemon 的 QR 一律 `Set2DBarcodeQRECLevel = 0`（Low），無參數可調。

9a. **`--qr-whitebg` 白底反相 QR（雙圖層）**
   - 旗標型。加上後會建成「白底矩形 + 反相 QR」兩個圖層（矩形先打、QR 後打），等同 GUI「QRCODE_白底」GroupBox。
   - **只需帶 `--qrcode` 內容即可**，其餘由 `WhiteBgQRParams` 預設補齊：
     - QR 資料區：**25×25mm**（可用 `--qr-width/--qr-height` 覆寫）
     - QR 外框：**2 單元**（可用 `--qr-border` 覆寫）
     - QR 圖層：power 90 / speed 1200 / freq 80 / pulse-width 30 / spot-size 0.04 / line-times 4 / step-angle 45
     - 矩形圖層：power 100 / speed 800 / freq 80 / pulse-width 250 / FillStyle 3 / FillTimes 4 / 向內填滿 (InsideOut) / StartAngle 90 / StepAngle 45
   - **兩圖層雷射參數可透過 CLI 覆寫**（速度/功率/頻率/脈寬）：

     | 圖層 | 功率 | 速度 | 頻率 | 脈寬 |
     |---|---|---|---|---|
     | QR 資料層 | `--qr-power` | `--qr-speed` | `--qr-freq` | `--qr-pw` |
     | 白底矩形層 | `--rect-power` | `--rect-speed` | `--rect-freq` | `--rect-pw` |

     - **QR 層功率/速度的 fallback**：未帶 `--qr-power`/`--qr-speed` 時，會退回通用 `--power`/`--speed`（向後相容），皆無則用預設 90/1200。優先序：`--qr-power` > `--power` > 預設。
     - QR 層頻率/脈寬、以及**矩形層四個參數**都只認自己的 `--qr-freq`/`--qr-pw`/`--rect-*`，**不吃** `--power`/`--speed` fallback。
   - 尺寸/外框覆寫：`--qr-width` / `--qr-height` / `--qr-border`。
   - **CLI 仍無法覆寫的寫死參數**：矩形 FillStyle/FillTimes/InsideOut/StartAngle/StepAngle、QR SpotSize/LineTimes/StepAngle。若需調整需改 `Form1.cs`（`BuildWhiteBgQR` 或 `WhiteBgQRParams`）並重編。
   - 白底模式會**跳過** `ApplyLaserParamsAuto`，避免覆寫各圖層已個別設好的雷射參數。
   - 適用 daemon、模式 A、GUI「命令提示」頁籤（tab 8）與「CLI 編輯器」頁籤（tab 7 的 QRCODE 區塊），四條路徑共用 `BuildWhiteBgQR` 後端。
   - 範例：
     - 最簡（全走預設）：`--client --board 0 --qrcode "1234567" --qr-whitebg --mark`
     - 顯式帶兩層雷射參數（slm-api `whiteBgQrLaserArgs()` 產生的形式）：`--client --board 0 --qrcode "1234567" --qr-whitebg --qr-width 20 --qr-height 20 --qr-border 2 --qr-power 90 --qr-speed 1200 --qr-freq 80 --qr-pw 30 --rect-power 100 --rect-speed 800 --rect-freq 80 --rect-pw 250 --mark`
     - 只調 QR 層功率（矩形不動）：`--client --board 0 --qrcode "ABC123" --qr-whitebg --qr-power 60 --qr-speed 1500 --mark`

10. **Daemon 目前不支援 `--dxf`（MVP 階段）**
    要打 DXF 請用模式 A，或之後擴充 `RunDaemonSpec` 加入 `LoadDxfAuto` 呼叫

11. **Client 引號**
    含空格的值（如 `--qrcode "Hello World"`）一定要在外層 shell 加引號，client 會自動 re-quote 傳給 daemon

12. **命令提示頁籤（tab 8）產生的範例**
    - 5 個 slot 各自隨機 50/50 選 Lines 或 QR 內容（可能全 Lines / 全 QR / 混合）
    - 線段：多線段 `--lines "x1,y1,x2,y2;..."`（2-4 段隨機），且會自動加 `--wobble-width 0.5`
    - QR：從 `{DEMO-001, TEST, ABC-123, QR-XYZ, Hello, MarkingMate}` 隨機取一個，只帶 `--qrcode "X" --qr-whitebg --mark`（其餘走 `WhiteBgQRParams` 預設）
    - 按鈕文字「執行預覽」實際是 `PreviewMode=0`（正式打標），與文字不符
    - 按鈕按下走 in-process 執行（不繞 daemon HTTP），共用 `BuildWhiteBgQR` / `DrawLineAuto`
    - 不再產生單線段 `--line` 範例

13. **CLI 編輯器頁籤（tab 7）產生的範例**
    - 左側「命令參數編輯」GroupBox：DXF / Lines / 全部雷射參數 / Wobble / Preview 欄位可填
    - 右上「QRCODE (白底反相，僅可異動 Content)」GroupBox：只能編輯 QR Content，其餘白底參數固定顯示
    - 兩區塊各自產生獨立命令，互不干擾：
      - 左側 → `txtCLIOutput`：`--lines "..." --power N --speed M ... --mark`
      - QRCODE → `txtCLIQROutput`：`--qrcode "content" --qr-whitebg --qr-width 15 --qr-height 15 --qr-border 2 --power 80 --speed 1000 --mark`
    - 兩區塊各自的「依此命令打標」按鈕會使用對應的參數集合走 `ExecuteCLIArgs`
    - 「停止預覽/打標」共用，會同時恢復兩個 execute 按鈕的可用狀態

---

## 仍會跳出 GUI 的情況

CLI 模式不是真正的 headless：

| 情況 | 表現 |
|---|---|
| 參數解析例外 | `MessageBox.Show("參數解析錯誤")` |
| `--help` | **用 MessageBox 顯示**，不是 stdout |
| Form1 啟動例外 | MessageBox + ExitCode = -1 |
| Daemon Form | 最小化、不顯示 taskbar，但仍有 Window Handle（OCX 要求） |
| OCX 初始化失敗（如多 process 撞鎖） | SDK 跳對話框「Initial Error!」 |

---

## 範例工作流

### 首次使用

```powershell
# 1. 先以系統管理員身分開 PowerShell

# 2. 體檢設定
cd C:\Users\MyUser\Documents\MMProjectMulti\MMProjectMulti\WindowsFormsApp1
.\CheckMMSetup.ps1

# 3.（首次）用 MarkingMate 官方工具設定各板 IP
& "C:\Program Files (x86)\MarkingMate\MultiMMSetting.exe"
# → 設定軸卡 IP（寫入主表 EMC6\DevIPAddress.ini 的 [DEVICE] DEV0~DEVn）→ 關閉
# 本程式 GUI 的「連接設定」頁只唯讀顯示 IP，不再提供「儲存IP」；初始化時會把主表同步到各板
```

### 日常單板 CLI（模式 A）

```cmd
MarkingMate.exe --board 0 --line 0,0,50,50 --mark
MarkingMate.exe --board 0 --dxf test.dxf --power 50 --speed 800 --mark
```

### 日常多板並行（模式 B）

```cmd
:: 一次性啟動 daemon
start "" MarkingMate.exe --daemon

:: 任意連發 client（不同 cmd、bat、ps1 都行）
MarkingMate.exe --client --board 0 --line 0,0,50,50 --mark --preview full --preview-time 10
MarkingMate.exe --client --board 1 --qrcode "TEST" --mark --preview full
MarkingMate.exe --client --board 2 --line -20,-20,20,20 --mark

:: 收工
MarkingMate.exe --client --shutdown
```

### 網頁端整合（模式 B）

```javascript
// 確認 daemon 健康
fetch('http://localhost:19527/health').then(r => r.text()).then(console.log);

// 發命令（return { exitCode, logs }）
async function fire(args) {
  const r = await fetch('http://localhost:19527/cmd', {
    method: 'POST',
    headers: { 'Content-Type': 'text/plain' },
    body: args
  });
  return r.json();
}

// 4 塊板同時各做不同事
//   - 多線段：用 --lines "x1,y1,x2,y2;..." 一次傳多段
//   - QR：位置固定 0,0，只需 --qr-width/--qr-height
//   - 非 QR 內容可帶 --workspace-w / --workspace-h 改本次命令的工作區（daemon 也支援）
const results = await Promise.all([
  fire('--board 0 --lines "0,0,50,50;10,10,40,40" --mark --preview full --preview-time 5'),
  fire('--board 1 --qrcode "A" --qr-width 15 --qr-height 15 --mark --preview full --preview-time 5'),
  fire('--board 2 --qrcode "B" --qr-width 10 --qr-height 10 --mark --preview full --preview-time 5'),
  fire('--board 3 --line -30,-30,30,30 --workspace-w 200 --workspace-h 120 --mark --preview full --preview-time 5'),
]);
console.log(results);
```

---

## 常見錯誤訊息

| 訊息（終端機 / 對話框） | 原因 | 解決方式 |
|---|---|---|
| `Initial Error! Please initial MMMark_1 OCX first!` | 有另一個實體佔著 SDK 的 process-global OCX 鎖，或首次使用尚未完成雷射頭設定。三大來源：①**第一次使用、環境尚未設定**（OCX / 雷射頭設定未初始化）②**殘留的 `MM27Dx64.exe` 孤兒 process**（前一場次結束沒收掉，霸佔 EMC6 卡）③**daemon 運行中又跑模式 A**（第二個 process init OCX）| **①第一次使用**：先執行 `C:\Program Files (x86)\MarkingMate\MultiMMSetting.exe` 完成雷射頭 / 環境設定，再啟動本程式。**②③殘留鎖不用重開機**：以系統管理員開 PowerShell 跑 `Get-Process MM27Dx64 \| Stop-Process -Force` 清掉孤兒即可恢復。已修：MarkingMate 結束時（`OnFormClosed` → `CleanupMM27Dx64OnExit`）會自動收掉自己場次的 MM27Dx64；daemon 運行中的模式 A 也會自動轉發成 client。若仍出現，檢查是否有非本程式的第三方 process 在碰 SDK |
| `Error: Board N is already being used by another instance.` | 模式 A 同 board 已有 process | 等舊的結束、或 kill 對應的 MarkingMate.exe |
| `Error: 已有 daemon 在 port N 運行` | Daemon 重複啟動 | 用 `--client --shutdown` 收掉舊的、或換 port |
| `Error: 無法連線到 daemon (...)` | Client 找不到 daemon | 先 `start "" MarkingMate.exe --daemon` |
| `{"exitCode":5,"logs":"board N busy"}` | 同板已有 spec 在跑 | 等該板結束、或改派到別塊板 |
| `Error: 主 IP 表 DEVx 為空…` | 主表 `EMC6\DevIPAddress.ini` 的 `[DEVICE]` 該 `DEVx` 未設定 | 執行 `C:\Program Files (x86)\MarkingMate\MultiMMSetting.exe` 設定軸卡 IP |
| `Error: 雷射頭設定部署 / 驗證失敗… 寫入權限不足` | 沒有系統管理員權限 | 以系統管理員身分啟動 cmd / PowerShell |
| `Error: 初始化板 N 失敗：…` | OCX 初始化失敗 | 檢查 OCX 註冊（`ELOCXRegister.exe`）、Lens 校正檔 |
| 對話框：`...\Drivers\EMC6_MM1\EMC6_x64.DRV driver load Error !` +「驅動程式載入失敗！」，隨後 `MarkLibrary4_x64  Init Time out!! (config_MM1)` | 已部署的 `EMC6_MMx\` 缺驅動 binary（`EMC6_x64.DRV` 等），SDK 用 `config_MMx` 初始化時載不到驅動。根因：舊版 `DeployAndValidateLaserHeadConfigs` 只複製 `DevIPAddress.ini` + `cfg\*.cfg`，未複製 `EMC6_MMx\` 根目錄的驅動 binary | 已修：部署改為 `CopyDirectoryRecursive` 遞迴複製**整個** `EMC6_MMx\` 目錄（含驅動 binary）。修好前的機器可手動把 repo `Drivers\EMC6_MMx\` 整包複製到 `C:\Program Files (x86)\MarkingMate\Drivers\EMC6_MMx\`（保留該處既有 `DevIPAddress.ini`）再重新初始化 |
| `Error: Failed to load DXF.` | DXF 解析失敗或檔不存在 | 檢查路徑（相對路徑以 exe 所在資料夾為基準） |
| `Error: StartMarking(N) failed with code 1` | SDK 拒絕啟動（多半是配置 / standby / 內容問題） | 確認 `--config` 對得上 `--board`、有實際內容、`MarkStandBy` 成功 |
| daemon log：`[Board N] StartMarking(4) returned 1`（或 `9`）＝ daemon exitCode 3 | **首次打標卡未就緒**：rc=1=MMMark 未初始化、rc=9=EMC6 控制卡未連接。多發生在 daemon 剛啟動、EMC6 乙太網卡還沒連上時的頭幾條命令 | 已修：① daemon init 後加 `WaitForCardsConnected`（輪詢 `IsCardConnect` 等卡連上、逾時 15s 才讓 /health 就緒）；② 前端 `sendDaemonCmd` 對 rc=1/9 自動等 1.5s 重送最多 4 次。若仍持續出現，檢查該板 IP（主表 `DevIPAddress.ini`）與網路連線 |
| `EMC6_x64` 對話框：`Unknown Commands=127  PreCommand=200, 26, 29, 26, 9` | 對 QR 物件套了 `SetWobble`（CLI 的 wobble 預設 0.5 mm 被誤套到 barcode） | 已修：CLI/daemon 的 wobble loop 會自動跳過 QR；若仍出現，檢查是否有自寫程式在 AddBarcode 後對 QR 物件呼叫 `SetWobble/SetWobbleSwitch` |

---

## 相關檔案

- [CommandLineArgs.cs](CommandLineArgs.cs) — 參數解析、`GetHelpText`、`--daemon` / `--client` / `--port` 旗標
- [Program.cs](Program.cs) — Mutex、`AttachConsole`、Client 模式 HTTP 呼叫、模式分派
- [Form1.cs](Form1.cs) — `ExecuteAutoMode`（模式 A）、`StartDaemonMode`、`InitializeBoardAuto`、`RunDaemonSpec`、`BuildWhiteBgQR`（QR 白底共用後端）、`ExecuteCLIArgs`（tab 7 CLI 編輯器共用執行流程）
- [MarkingMateDaemon.cs](MarkingMateDaemon.cs) — HttpListener 主體、`/health` `/cmd` `/shutdown` 三個 endpoint
- [CheckMMSetup.ps1](CheckMMSetup.ps1) — 設定體檢腳本
- [故障排除完整指南.md](故障排除完整指南.md) — GUI 模式與 OCX 註冊故障排除
- [../命令列模式使用說明.txt](../命令列模式使用說明.txt) — 給非工程師的入門說明（v3.0）
