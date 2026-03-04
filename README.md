# 四晶片板雷射打標系統

## 專案說明
這是一個基於 MarkingMate SDK 開發的四晶片板雷射打標應用程式，可以同時控制四個獨立的晶片板進行雷射打標操作。

## 功能特點
- **四晶片板支持**：同時管理和控制四個獨立的晶片板
- **線段繪製**：可以在任意晶片板上繪製線段
- **雷射打標**：使用雷射在材料上打出線段
- **獨立控制**：每個晶片板可以獨立操作和控制

## 系統要求
- Windows 作業系統
- .NET Framework 4.7.2 或更高版本
- MarkingMate 軟體已安裝於 `C:\Program Files (x86)\MarkingMate`
- Visual Studio 2017 或更高版本（用於開發）

## 使用說明

### 1. 初始化系統
- 啟動應用程式後，點擊 **初始化** 按鈕
- 系統將初始化四個晶片板控件
- 初始化成功後會顯示提示訊息

### 2. 設定線段參數
在右側的「線段參數」區域設定：
- **X1, Y1**：線段起點座標（單位：毫米）
- **X2, Y2**：線段終點座標（單位：毫米）
- **選擇板**：選擇要操作的晶片板（1-4）

### 3. 繪製線段
- 設定好參數後，點擊 **繪製線段** 按鈕
- 線段將顯示在選定的晶片板預覽區域中

### 4. 開始雷射打標
- 確認線段位置正確後，點擊 **開始雷射** 按鈕
- 選定的晶片板將開始執行雷射打標操作

### 5. 停止操作
- 如需緊急停止，點擊 **停止** 按鈕

## 專案結構
```
MMProjectMulti/
├── WindowsFormsApp1/
│   ├── Form1.cs              # 主窗體邏輯代碼
│   ├── Form1.Designer.cs     # 主窗體設計代碼
│   ├── Form1.resx            # 窗體資源檔案
│   ├── Program.cs            # 程式入口點
│   └── WindowsFormsApp1.csproj  # 專案檔案
└── README.md                 # 本說明文件
```

## 技術細節

### 使用的 MarkingMate SDK 控件
- **AxMMMarkx64**：主要的打標控件，負責顯示和執行打標
- **AxMMEditx64**：編輯控件，用於添加和編輯圖形對象
- **AxMMStatusx64**：狀態控件，用於監控打標狀態

### 主要 API 方法
- `Initial()`：初始化控件
- `SetDesktopCenter(x, y)`：設定工作區域中心
- `SetDesktopSize(w, h)`：設定工作區域大小
- `AddObj(type)`：添加圖形對象（1 = 線段）
- `SetObjPara(index, param, value)`：設定對象參數
- `StartMarking(mode)`：開始打標（4 = 非阻塞模式）
- `StopMarking()`：停止打標
- `Redraw()`：重繪顯示

## 配置說明

### DLL 引用路徑
專案引用了以下 MarkingMate SDK 的 DLL：
- `AxMMEdit_x64.dll`
- `AxMMMark_x64.dll`
- `AxMMStatus_x64.dll`
- `MMEditx64Lib.dll`
- `MMMarkx64Lib.dll`
- `MMStatusx64Lib.dll`

預設路徑：`C:\Program Files (x86)\MarkingMate\sdk\ocx\CSharp_example\VS2017\LoadFile\LoadFile\bin\x64\Release\`

## 編譯說明

1. 確保已安裝 MarkingMate 軟體
2. 使用 Visual Studio 開啟 `WindowsFormsApp1.sln`
3. 選擇平台為 **x64**
4. 建置解決方案（建議使用 Release 配置）

## 注意事項

1. **平台設定**：必須使用 x64 平台編譯，因為 MarkingMate SDK 為 64 位元版本
2. **權限要求**：執行時可能需要管理員權限以訪問雷射硬體
3. **安全操作**：使用雷射設備時請遵守安全規範
4. **硬體連接**：確保晶片板硬體已正確連接到電腦

## 故障排除

### 初始化失敗
- 檢查 MarkingMate 是否正確安裝
- 確認 DLL 檔案路徑是否正確
- 檢查是否有足夠的權限訪問檔案

### 雷射無法啟動
- 確認硬體連接正常
- 檢查驅動程式是否正確安裝
- 查看 MarkingMate 主程式是否能正常運作

### 顯示異常
- 嘗試重新初始化
- 檢查座標參數是否在有效範圍內

## 版本資訊
- 版本：1.0.0
- 建立日期：2026-03-03
- 開發環境：Visual Studio 2017+, .NET Framework 4.7.2

## 授權
本專案基於 MarkingMate SDK 開發，請遵守相關授權協議。
