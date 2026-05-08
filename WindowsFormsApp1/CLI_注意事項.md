# CLI 模式（命令提示字元）注意事項

> 本文件整理 `MarkingMateMulti.exe` 命令列模式的所有重要事項與已知行為。
> GUI 模式請見 [故障排除完整指南.md](故障排除完整指南.md)。

---

## ⚠️ 一定要知道的 4 件事

### 1. 板號從 0 開始，且會初始化「板號 + 1」片板

```powershell
MarkingMateMulti.exe --board 2 --line 0,0,50,50 --mark
```

- `--board 2` 代表 **MM3**（不是 MM2）
- 程式會驗證並部署 **MM1、MM2、MM3** 的設定，但只在 MM3 上打標
- 原因：[InitializeBoardAuto](Form1.cs) 呼叫 `DeployAndValidateLaserHeadConfigs(boardIndex + 1, ...)`

### 2. 必須以「系統管理員身分」執行

- 初始化會把整個 `Drivers\EMC6\` 等檔案部署到 `C:\Program Files (x86)\MarkingMate\`
- 一般使用者寫不進去 → `Error: 部署失敗 寫入權限不足` → ExitCode = 1
- **終端機本身**也要先以「系統管理員身分執行 cmd / PowerShell」

### 3. IP 必須先在 GUI 設定好

- CLI 模式**不能**編輯 IP
- 主表 `Drivers\EMC6\DevIPAddress.ini` 的 `DEV0~DEV(BoardIndex)` 任一空值 → ExitCode = 1
- 對應關係（固定）：

  | 主表 | → 板 / CARD |
  |---|---|
  | `DEV0` | MM1 / CARD0 |
  | `DEV1` | MM2 / CARD1 |
  | `DEV2` | MM3 / CARD2 |
  | `DEV3` | MM4 / CARD3 |

- 第一次使用流程：先 GUI 啟動 → 連接設定頁填 IP → 按「儲存IP」→ 之後才能用 CLI

### 4. 必須從 cmd / PowerShell 啟動才看得到輸出

```cmd
:: ✓ 看得到輸出
cmd> MarkingMateMulti.exe --board 0 --line 0,0,50,50 --mark

:: ✗ 看不到輸出（雙擊執行）
```

- [Program.cs](Program.cs) 用 `AttachConsole(ATTACH_PARENT_PROCESS)` 接到父終端機
- 雙擊或從 Explorer 啟動 → 沒有父終端機 → `Console.WriteLine` 看不到（程式仍會跑）

---

## 多開行為（不同板號並行）

| 場景 | 結果 |
|---|---|
| GUI 模式重複開 | 第二個 → MessageBox「程式已執行」→ ExitCode = -1 |
| CLI `--board 0` 開兩次 | 第二個 → stderr「Board 0 is already in use」→ **ExitCode = -2** |
| CLI 同時 `--board 0` + `--board 1` | ✓ 各自獨立執行（mutex 名稱以 board 區分） |
| CLI 並行時 | 不會清理 `MM27Dx64.exe` 背景程序，避免影響其他實例 |

> **Mutex 名稱**：CLI = `MarkingMateMulti_Board{N}`、GUI = `MarkingMateMulti_SingleInstance`

---

## 退出代碼

| Code | 意義 |
|---|---|
| `0` | 成功 |
| `1` | 初始化失敗（含主表 IP 缺值、部署權限不足、OCX 異常） |
| `2` | 繪圖失敗（DXF 載入失敗 / AddLine / AddBarcode 錯誤） |
| `3` | 打標失敗（`StartMarking` 回傳非 0） |
| `4` | 參數錯誤（保留，目前未實際用到） |
| `-1` | 一般錯誤、GUI 重複執行 |
| `-2` | 同 board 已在執行 |

---

## 參數小陷阱

1. **`--mark` 是必須的**
   少了它 → 只載入 / 繪製，不會真的打標（也不會預覽）

2. **`--preview` 必須搭配 `--mark`**
   - `outline` / `box` = 外框預覽
   - `full` / `path` = 全路徑預覽（預設）

3. **參數名要小寫**（`--Board` 不行）
   但值（DXF 路徑、QR 字串）保留大小寫

4. **線段座標自動置中**
   CLI 多條 `--line` / `--lines` 會整體置中到工作區原點，跟 DXF 行為一致

5. **座標原點偵測**
   - 任一座標為負 → 視為「中心原點座標」，不做平移
   - 全部非負 → 視為「左下角原點座標」自動平移

6. **預覽時間預設 15 秒**：用 `--preview-time <秒>` 調整

7. **`--workspace`**
   影響 `SetDesktopSize` 與 DXF 縮放，但**不**影響 `--line` 座標解讀

---

## 仍會跳出 GUI 的情況

CLI 模式並不是真正的 headless：

| 情況 | 表現 |
|---|---|
| 參數解析例外 | `MessageBox.Show("參數解析錯誤")` |
| `--help` | **用 MessageBox 顯示**，不是 stdout |
| Form1 啟動例外 | MessageBox + ExitCode = -1 |
| 一般執行 | Form1 仍會建立、開窗，OCX 初始化後執行 → 自動 Close |

> 如要完全 headless，需修 [Program.cs](Program.cs) 把 `--help` 改成 `Console.WriteLine` 並處理 `try/catch` 路徑。

---

## 範例工作流（首次使用）

```powershell
# 1. 先以系統管理員身分開 PowerShell

# 2. 體檢設定
cd C:\Users\MyUser\Documents\MMProjectMulti\MMProjectMulti\WindowsFormsApp1
.\CheckMMSetup.ps1

# 3.（首次）以 GUI 啟動填 IP
.\bin\x64\Debug\MarkingMate.exe
# → 連接設定頁填 IP → 儲存IP → 關閉

# 4. CLI 跑單片板
.\bin\x64\Debug\MarkingMate.exe --board 0 --line 0,0,50,50 --mark

# 5. 並行多片板（建議各自開 PowerShell）
Start-Process .\bin\x64\Debug\MarkingMate.exe -ArgumentList '--board 0','--dxf','test.dxf','--mark'
Start-Process .\bin\x64\Debug\MarkingMate.exe -ArgumentList '--board 1','--dxf','test.dxf','--mark'
```

---

## 常見錯誤訊息對照

| 終端機輸出 | 原因 | 解決方式 |
|---|---|---|
| `Error: Board N is already being used by another instance.` | 同 board 已有 process | 等舊的結束、或 kill 對應的 MarkingMate.exe |
| `Error: 雷射頭設定部署 / 驗證失敗（板 N）：主 IP 表 DEVx 為空…` | 主表沒填 IP | 開 GUI 模式設定 IP 再儲存 |
| `Error: 雷射頭設定部署 / 驗證失敗… 寫入權限不足` | 沒有系統管理員權限 | 以系統管理員身分啟動 cmd / PowerShell |
| `Error: 初始化板 N 失敗：…` | OCX 初始化失敗 | 檢查 OCX 註冊（`ELOCXRegister.exe`）、Lens 校正檔 |
| `Error: Failed to load DXF.` | DXF 解析失敗或檔不存在 | 檢查路徑（相對路徑以 exe 所在資料夾為基準） |
| `Error: AddBarcode failed with code N` | QR Code 新增失敗 | 確認 SDK 條碼類型常數正確 |
| `Error: StartMarking(N) failed with code …` | 引擎啟動打標失敗 | 確認 `MarkStandBy` 已執行、SDK 狀態正常 |

---

## 相關檔案

- [CommandLineArgs.cs](CommandLineArgs.cs) — 參數解析、`GetHelpText`
- [Program.cs](Program.cs) — Mutex、AttachConsole、Application.Run
- [Form1.cs](Form1.cs) — `ExecuteAutoMode`、`InitializeBoardAuto`
- [CheckMMSetup.ps1](CheckMMSetup.ps1) — 設定體檢腳本
- [故障排除完整指南.md](故障排除完整指南.md) — GUI 模式與 OCX 註冊故障排除
