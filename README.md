# MarkingMate Multi-Board 雷射打標系統

## 專案說明
基於 MarkingMate SDK 開發的多晶片板雷射打標應用程式，支援 GUI 操作與命令列自動化模式，可控制最多四個獨立晶片板。

## 功能特點
- **四晶片板支持**：同時管理和控制四個獨立的晶片板
- **DXF 載入**：支援載入 DXF 檔案，自動解析線段並繪製
- **雷射參數設定**：功率、速度、頻率、脈波寬度、雷射次數
- **擺動功能（Wobble）**：支援擺動寬度、重疊率、擺動速度設定
- **紅光預覽**：打標前可使用紅光預覽路徑（外框預覽 / 全路徑預覽）
- **命令列模式**：支援透過命令列參數自動化執行，適合批次整合

## 系統要求
- Windows 作業系統
- .NET Framework 4.7.2 或更高版本
- MarkingMate 軟體已安裝於 `C:\Program Files (x86)\MarkingMate`
- Visual Studio 2017 或更高版本（用於開發）

---

## 命令列模式

不帶參數啟動為 GUI 模式，帶參數則進入命令列自動化模式。

### 使用方式

```
MarkingMateMulti.exe                  啟動 GUI 模式
MarkingMateMulti.exe [選項]           命令列模式
```

### 選項一覽

#### 基本設定

| 參數 | 縮寫 | 說明 | 預設值 |
|------|------|------|--------|
| `--help` | `-h`, `/?` | 顯示說明訊息 | - |
| `--board <0-3>` | `-b` | 指定晶片板編號 | `0` |
| `--config <path>` | `-c` | 指定配置路徑 | `/cfg_config_MM1` |
| `--workspace <size>` | `-w` | 工作區大小 (mm) | `150` |

#### 圖形輸入

| 參數 | 縮寫 | 說明 |
|------|------|------|
| `--line <x1,y1,x2,y2>` | `-l` | 新增單一線段 |
| `--lines <線段列表>` | - | 新增多條線段（以分號 `;` 分隔） |
| `--dxf <path>` | `-d` | 載入 DXF 檔案（手動解析線段） |

#### 雷射參數（不指定則使用預設值）

| 參數 | 縮寫 | 說明 |
|------|------|------|
| `--power <0-100>` | `-p` | 雷射功率 (%) |
| `--speed <mm/s>` | `-s` | 打標速度 (mm/s) |
| `--freq <kHz>` | `-f` | 脈衝頻率 (kHz) |
| `--pulse-width <val>` | `--pw` | 脈波寬度 |
| `--repeat <n>` | `-r` | 雷射次數 |

#### 擺動參數（不指定 `--wobble-width` 則不啟動擺動）

| 參數 | 說明 | 預設值 |
|------|------|--------|
| `--wobble-width <val>` 或 `--wobble` | 擺動寬度 (mm) | - |
| `--wobble-overlap <n>` | 擺動重疊率 (%) | `50` |
| `--wobble-speed <mm/s>` | 擺動速度 (mm/s) | `5026.55` |

#### 執行控制

| 參數 | 縮寫 | 說明 |
|------|------|------|
| `--mark` | `-m` | 自動執行打標 |
| `--preview <outline\|full>` | - | 紅光預覽模式（需搭配 `--mark`）<br>`outline` = 外框預覽, `full` = 全路徑預覽（預設） |
| `--preview-speed <mm/s>` | - | 預覽速度 (mm/s) |

### 範例

```bash
# 在板 0 上畫一條線並打標
MarkingMateMulti.exe --board 0 --line 0,0,50,50 --mark

# 載入 DXF 並指定雷射參數
MarkingMateMulti.exe --board 0 --dxf "File\test.dxf" --power 50 --speed 800 --freq 20 --pw 5 --repeat 1 --mark

# 在板 1 上畫多條線
MarkingMateMulti.exe --board 1 --lines "0,0,50,50;10,10,40,40" --mark

# 載入 DXF 並啟用擺動
MarkingMateMulti.exe --board 0 --dxf "File\test.dxf" --wobble-width 0.5 --wobble-speed 5026.55 --mark

# 外框預覽（紅光描外框，不打雷射）
MarkingMateMulti.exe --board 0 --dxf "File\test.dxf" --mark --preview outline

# 全路徑預覽並指定預覽速度
MarkingMateMulti.exe --board 0 --dxf "File\test.dxf" --mark --preview full --preview-speed 500

# 指定工作區大小 200mm 載入 DXF
MarkingMateMulti.exe --board 0 --workspace 200 --dxf "File\上翼板-2.dxf" --mark

# 使用自訂配置
MarkingMateMulti.exe --board 2 --config /cfg_config_MM3 --line 0,0,100,100
```

### 結束代碼

| 代碼 | 說明 |
|------|------|
| `0` | 成功 |
| `1` | 初始化失敗 |
| `2` | 繪圖失敗 |
| `3` | 打標失敗 |
| `4` | 參數錯誤 |

---

## GUI 使用說明

### 1. 初始化系統
- 啟動應用程式後，在「連接設定」頁簽點擊 **初始化** 按鈕
- 系統將依序初始化晶片板控件
- 初始化成功後會顯示提示訊息

### 2. 載入圖形
- **DXF 頁簽**：點擊「載入 DXF」選擇 DXF 檔案
- **繪圖頁簽**：手動輸入線段座標（X1, Y1, X2, Y2）後點擊「繪製線段」

### 3. 設定雷射參數
在「雷射功率」頁簽中設定：
- 功率 (0-100%)、速度 (mm/s)、頻率 (kHz)、脈波寬度、雷射次數
- 擺動功能：勾選啟用後設定寬度、重疊率、速度
- 點擊「套用」將參數寫入所有物件（打標時也會自動套用）

### 4. 紅光預覽
- 在 DXF 頁簽點擊「紅光預覽」進行預覽
- 點擊「停止預覽」結束

### 5. 打標
- 點擊「打標」開始雷射打標（會自動套用雷射參數）
- 點擊「停止打標」可中止

## 專案結構
```
MMProjectMulti/
├── WindowsFormsApp1/
│   ├── Form1.cs              # 主窗體邏輯代碼
│   ├── Form1.Designer.cs     # 主窗體設計代碼
│   ├── Form1.resx            # 窗體資源檔案
│   ├── CommandLineArgs.cs    # 命令列參數解析器
│   ├── Program.cs            # 程式入口點
│   └── WindowsFormsApp1.csproj
├── CLAUDE.md
└── README.md
```

## 技術細節

### MarkingMate SDK 控件
- **AxMMMarkx641**：打標控件（MultiMM 版本，支援多板）
- **AxMMEditx641**：編輯控件，用於添加和編輯圖形對象

### 主要 API
| 方法 | 說明 |
|------|------|
| `Initial()` | 初始化控件 |
| `SetDesktopCenter(x, y)` | 設定工作區中心 |
| `SetDesktopSize(w, h)` | 設定工作區大小 |
| `SetPower(objName, %)` | 設定雷射功率 |
| `SetSpeed(objName, mm/s)` | 設定打標速度 |
| `SetFrequency(objName, kHz)` | 設定脈衝頻率 |
| `SetPulseWidth(objName, pw)` | 設定脈波寬度 |
| `SetMarkRepeat(objName, n)` | 設定雷射次數 |
| `SetWobble(objName, width, freq)` | 設定擺動（freq = speed / (π × width)） |
| `SetWobbleSwitch(objName, 1)` | 啟用擺動 |
| `StartMarking(3)` | 紅光預覽 |
| `StartMarking(4)` | 開始打標（非阻塞） |
| `StopMarking()` | 停止打標 |
| `SetLensXReverse(1)` | X 軸鏡像修正 |

### 座標系統
- 原點 (0, 0) 位於工作區中心
- X 軸：右為正方向
- Y 軸：下為正方向
- 單位：毫米 (mm)
- 預設工作區：150 × 150 mm

## 編譯說明

1. 確保已安裝 MarkingMate 軟體
2. 使用 Visual Studio 開啟方案檔
3. 選擇平台為 **x64**
4. 建置（或使用 MSBuild：`MSBuild WindowsFormsApp1.csproj /p:Configuration=Debug /p:Platform=x64`）

## 注意事項

1. **平台設定**：必須使用 x64 平台編譯
2. **權限要求**：執行時可能需要管理員權限
3. **安全操作**：使用雷射設備時請遵守安全規範
4. **硬體連接**：確保晶片板硬體已正確連接
5. **單一實例**：程式使用 Mutex 確保同時只執行一個實例

## 故障排除

| 問題 | 排查方式 |
|------|----------|
| 初始化失敗 | 檢查 MarkingMate 安裝、DLL 路徑、存取權限 |
| 雷射無法啟動 | 確認硬體連接、驅動程式、MarkingMate 主程式是否正常 |
| 打標左右相反 | 已內建 `SetLensXReverse(1)` 修正 |
| 擺動無效果 | 確認擺動寬度 > 0 且已勾選啟用 |

## 授權
本專案基於 MarkingMate SDK 開發，請遵守相關授權協議。
