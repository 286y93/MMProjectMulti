# MarkingMate OCX API 參考指南

## 概述
本文档基于 MarkingMate SDK v2.7，整理了在四晶片板雷射打標系統中常用的 API 方法。

---

## 一、控件初始化

### 1.1 三大控件
MarkingMate 系統使用三個主要的 OCX 控件：

| 控件名稱 | 功能說明 |
|---------|---------|
| `AxMMMarkx64` | 主控件：顯示、打標、控制 |
| `AxMMEditx64` | 編輯控件：添加圖形、編輯對象 |
| `AxMMStatusx64` | 狀態控件：監控打標狀態、事件處理 |

### 1.2 初始化方法

```csharp
// 基本初始化
m_MMMark.Initial();
m_MMEdit.Initial();
m_MMStatus.Initial();

// 多板系統初始化（使用不同配置）
m_MMMark1.InitialExt("/cfg_config_MM1");
m_MMMark2.InitialExt("/cfg_config_MM2");
```

---

## 二、工作區域設定

### 2.1 設定工作區域

```csharp
// 設定工作區域中心點（單位：毫米）
m_MMMark.SetDesktopCenter(double x, double y);

// 設定工作區域大小（單位：毫米）
m_MMMark.SetDesktopSize(double width, double height);

// 設定當前資料庫 ID
m_MMMark.SetActiveDB(int dbId);

// 進入待機狀態
m_MMMark.MarkStandBy();

// 設定編輯模式（2 = 標準編輯模式）
m_MMMark.SetCurEditFun(int mode);
```

### 2.2 獲取工作區域資訊

```csharp
double centerX = m_MMMark.GetDesktopCenterX();
double centerY = m_MMMark.GetDesktopCenterY();
double width = m_MMMark.GetDesktopWidth();
double height = m_MMMark.GetDesktopHeight();
```

---

## 三、圖形繪製（MMEdit）

### 3.1 添加線段

**方法簽名：**
```csharp
int AddLine(double x1, double y1, double x2, double y2,
            string parentName, string newName);
```

**參數說明：**
- `x1, y1`: 線段起點座標（mm）
- `x2, y2`: 線段終點座標（mm）
- `parentName`: 父對象名稱（可為空 ""）
- `newName`: 新對象名稱（可為空 ""）

**返回值：**
- `0`: 成功
- 非 `0`: 失敗（錯誤碼）

**使用範例：**
```csharp
// 繪製從 (0,0) 到 (50,50) 的線段
int result = m_MMEdit.AddLine(0, 0, 50, 50, "", "");
if (result == 0)
{
    m_MMMark.Redraw();  // 更新顯示
}
```

### 3.2 添加其他圖形

#### 添加圓形
```csharp
int AddCircle(double centerX, double centerY, double radius,
              string parentName, string newName);
```

#### 添加矩形
```csharp
int AddRect(double left, double top, double right, double bottom,
            double round, string parentName, string newName);
```

#### 添加圓弧
```csharp
int AddArc(double startX, double startY, double endX, double endY,
           double radius, int ccw, string parentName, string newName);
```

#### 添加文字
```csharp
int AddText(string text, double centerX, double centerY,
            string parentName, string newName);
```

#### 添加點
```csharp
int AddDot(double x, double y, string parentName, string newName);
```

### 3.3 刪除對象

```csharp
// 刪除指定對象
int DeleteObject(string parentName, string objectName);
```

**注意：** MMEdit 控件**沒有** `ClearAll()` 方法！
要清除所有對象，需要逐一刪除或重新載入空白文件。

---

## 四、檔案操作

### 4.1 載入檔案

```csharp
// 載入 .ezm 檔案
int LoadFile(string filePath);

// 範例
int result = m_MMMark.LoadFile("C:\\path\\to\\file.ezm");
if (result == 0)
{
    // 載入成功
}
```

### 4.2 執行 MarkingMate 編輯器

```csharp
// 開啟 MarkingMate 主程式進行編輯
// mode: 1 = 編輯模式
m_MMMark.RunMarkingMate(string filePath, int mode);
```

---

## 五、雷射打標控制

### 5.1 開始打標

```csharp
// 開始雷射打標
// mode: 4 = 非阻塞模式（推薦）
int StartMarking(int mode);

// 範例
int result = m_MMMark.StartMarking(4);
if (result != 0)
{
    // 打標失敗
}
```

**模式說明：**
- `4`: 非阻塞模式 - 不會凍結 UI，可透過事件或計時器檢查狀態

### 5.2 停止打標

```csharp
// 停止當前打標操作
m_MMMark.StopMarking();
```

### 5.3 關閉打標引擎

```csharp
// 在打標完成後關閉打標引擎
m_MMMark.MarkShutdown();
```

### 5.4 檢查打標狀態

```csharp
// 檢查是否正在打標
// 返回: 0 = 未在打標, 非0 = 正在打標
int IsMarking();

// 範例（配合計時器使用）
if (m_MMMark.IsMarking() == 0)
{
    // 打標已完成
    m_MMMark.MarkShutdown();
}
```

---

## 六、顯示控制

### 6.1 重繪顯示

```csharp
// 重新繪製顯示內容
m_MMMark.Redraw();
```

### 6.2 桌面編輯模式

```csharp
// 獲取當前桌面編輯模式
int mode = m_MMMark.GetDesktopEditMode();

// 設定桌面編輯模式
// 0 = 禁用, 1 = 啟用
m_MMMark.SetDesktopEditMode(int mode);
```

---

## 七、狀態資訊

### 7.1 獲取打標資訊

```csharp
// 獲取打標次數
int count = m_MMMark.GetMarkCounts();

// 獲取打標耗時（毫秒）
int time = m_MMMark.GetMarkTime();

// 轉換為秒
double seconds = (double)time / 1000.0;
```

### 7.2 設定打標計數

```csharp
// 重置打標計數器
m_MMMark.SetMarkCounts(int count);
```

---

## 八、事件處理（MMStatus）

### 8.1 打標完成事件

```csharp
// 訂閱打標完成事件
m_MMStatus.MarkEnd += OnMarkEnd;

// 事件處理方法
private void OnMarkEnd(object sender, _DMMStatusx64Events_MarkEndEvent e)
{
    // 打標完成後的處理
    int markTime = m_MMMark.GetMarkTime();
    int markCount = m_MMMark.GetMarkCounts();

    m_MMMark.Redraw();
}
```

---

## 九、資源清理

### 9.1 正確的清理順序

```csharp
// 在窗體關閉時清理資源
private void Form1_FormClosed(object sender, FormClosedEventArgs e)
{
    if (m_bInit)
    {
        // 1. 關閉打標引擎
        m_MMMark.MarkShutdown();

        // 2. 完成並釋放控件
        m_MMStatus.Finish();
        m_MMEdit.Finish();
        m_MMMark.Finish();
    }
}
```

**重要：** 必須按照正確順序清理，否則可能導致資源泄漏或程式崩潰。

---

## 十、多板系統特殊考量

### 10.1 多板初始化

對於多個獨立的晶片板系統，使用不同的配置路徑：

```csharp
// 晶片板 1
m_MMMark[0].InitialExt("/cfg_config_MM1");

// 晶片板 2
m_MMMark[1].InitialExt("/cfg_config_MM2");

// 晶片板 3
m_MMMark[2].InitialExt("/cfg_config_MM3");

// 晶片板 4
m_MMMark[3].InitialExt("/cfg_config_MM4");
```

### 10.2 獨立控制

每個晶片板可以：
- 獨立初始化
- 獨立繪製圖形
- 獨立執行打標
- 獨立監控狀態

---

## 十一、常見錯誤與解決方案

### 11.1 清除畫面失敗

**錯誤：** 使用 `ClearAll()` 方法
**原因：** MMEdit 控件沒有此方法

**解決方案：**
1. 使用 `DeleteObject()` 逐一刪除
2. 或重新載入空白檔案

### 11.2 線段不顯示

**可能原因：**
1. 忘記調用 `Redraw()`
2. 座標超出工作區域範圍

**解決方案：**
```csharp
// 添加線段後必須重繪
m_MMEdit.AddLine(x1, y1, x2, y2, "", "");
m_MMMark.Redraw();  // 必須！

// 檢查座標範圍
// 預設工作區域: (-50, -50) 到 (50, 50)
```

### 11.3 初始化失敗

**檢查項目：**
1. DLL 檔案是否正確複製到輸出目錄
2. 是否使用 x64 平台編譯
3. MarkingMate 軟體是否正確安裝

---

## 十二、完整使用範例

### 12.1 基本線段繪製流程

```csharp
// 1. 初始化
m_MMMark.Initial();
m_MMEdit.Initial();
m_MMStatus.Initial();

// 2. 設定工作區域
m_MMMark.SetDesktopCenter(0, 0);
m_MMMark.SetDesktopSize(100, 100);
m_MMMark.SetActiveDB(0);
m_MMMark.MarkStandBy();
m_MMMark.SetCurEditFun(2);

// 3. 繪製線段
int result = m_MMEdit.AddLine(0, 0, 50, 50, "", "");
if (result == 0)
{
    m_MMMark.Redraw();
}

// 4. 開始打標
m_MMMark.StartMarking(4);

// 5. 停止打標（需要時）
m_MMMark.StopMarking();

// 6. 清理資源（關閉時）
m_MMMark.MarkShutdown();
m_MMEdit.Finish();
m_MMMark.Finish();
```

---

## 十三、API 快速查詢表

| 功能 | 方法 | 所屬控件 |
|------|------|---------|
| 初始化 | `Initial()` | MMMark/MMEdit/MMStatus |
| 多板初始化 | `InitialExt(string)` | MMMark |
| 設定中心 | `SetDesktopCenter(x, y)` | MMMark |
| 設定大小 | `SetDesktopSize(w, h)` | MMMark |
| 添加線段 | `AddLine(x1,y1,x2,y2,"","")` | MMEdit |
| 添加圓形 | `AddCircle(x,y,r,"","")` | MMEdit |
| 添加文字 | `AddText(text,x,y,"","")` | MMEdit |
| 刪除對象 | `DeleteObject(parent,name)` | MMEdit |
| 重繪 | `Redraw()` | MMMark |
| 開始打標 | `StartMarking(4)` | MMMark |
| 停止打標 | `StopMarking()` | MMMark |
| 檢查狀態 | `IsMarking()` | MMMark |
| 載入檔案 | `LoadFile(path)` | MMMark |
| 清理資源 | `Finish()` | 所有控件 |

---

## 參考資源

- **SDK 文檔位置：** `C:\Program Files (x86)\MarkingMate\sdk\DOC\`
  - OCX Manual(v2.7)(CT).pdf - 中文繁體版手冊
  - OCX Manual(v2.7)(Eng).pdf - 英文版手冊
  - MultiMM OCX Manual(v2.7)(CT).pdf - 多板系統手冊

- **範例代碼位置：**
  - C# 範例: `C:\Program Files (x86)\MarkingMate\sdk\ocx\CSharp_example\`
  - C++ 範例: `C:\Program Files (x86)\MarkingMate\sdk\ocx\vc_example\`
  - 多板範例: `C:\Program Files (x86)\MarkingMate\MulSDK\OCXSample\`

---

**版本：** 1.0
**最後更新：** 2026-03-03
**適用於：** MarkingMate SDK v2.7
