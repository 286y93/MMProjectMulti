# MarkingMate OCX 註冊問題解決方案

## 🚨 錯誤訊息
```
System.Runtime.InteropServices.COMException:
'類別未登錄 (發生例外狀況於 HRESULT: 0x80040154 (REGDB_E_CLASSNOTREG))'
```

## 📋 問題原因
MarkingMate 的 COM 組件（OCX/ActiveX 控件）沒有在 Windows 系統中註冊。

---

## ✅ 解決方案

### 方法 1：執行註冊腳本（推薦）

#### 步驟：
1. **以系統管理員身分執行**
   - 找到 `RegisterOCX.bat` 檔案
   - **右鍵點擊** → 選擇 **「以系統管理員身分執行」**

2. **等待註冊完成**
   - 腳本會自動註冊三個 DLL：
     - MMEditx64Lib.dll
     - MMMarkx64Lib.dll
     - MMStatusx64Lib.dll

3. **檢查結果**
   - 看到 `[OK]` 表示註冊成功
   - 看到 `[FAIL]` 表示註冊失敗，請檢查：
     - MarkingMate 是否正確安裝
     - 是否以系統管理員身分執行
     - DLL 檔案是否存在

---

### 方法 2：手動註冊（備用）

如果方法 1 失敗，請手動註冊：

#### 步驟：

1. **以系統管理員身分開啟命令提示字元**
   - 按 `Win + X`
   - 選擇「命令提示字元（系統管理員）」或「Windows PowerShell（系統管理員）」

2. **切換到 DLL 目錄**
   ```cmd
   cd "C:\Program Files (x86)\MarkingMate\sdk\ocx\CSharp_example\VS2017\LoadFile\LoadFile\bin\x64\Release"
   ```

3. **註冊每個 DLL**
   ```cmd
   regsvr32 MMEditx64Lib.dll
   regsvr32 MMMarkx64Lib.dll
   regsvr32 MMStatusx64Lib.dll
   ```

4. **確認註冊成功**
   - 每個命令應該會顯示「DllRegisterServer 在 XXX.dll 成功」

---

### 方法 3：使用 MarkingMate 安裝程式

1. 重新執行 MarkingMate 安裝程式
2. 選擇「修復」或「重新安裝」
3. 確保安裝時勾選了「SDK」和「OCX 控件」選項

---

## 🔍 驗證註冊

註冊完成後，請執行以下檢查：

### 檢查方法 1：查看註冊表
1. 按 `Win + R` 開啟執行視窗
2. 輸入 `regedit` 並按 Enter
3. 導航到：
   ```
   HKEY_CLASSES_ROOT\TypeLib\{CLSID}
   ```
4. 搜索 "MMEdit" 或 "MMMark"

### 檢查方法 2：重新執行程式
1. 在 Visual Studio 中重新編譯專案
2. 執行程式
3. 點擊「初始化」按鈕
4. 如果沒有錯誤，表示註冊成功 ✅

---

## ⚠️ 常見問題

### Q1: 註冊時出現「模組可能與此 Windows 版本不相容」
**A:** 確認您使用的是 x64 版本的 DLL，並在 x64 系統上執行。

### Q2: 註冊成功但仍然出現錯誤
**A:** 請嘗試：
1. 重新啟動電腦
2. 重新編譯專案（清理 → 重建）
3. 確認專案平台設定為 **x64**（不是 AnyCPU）

### Q3: 找不到 DLL 檔案
**A:** 檢查 MarkingMate 安裝路徑：
```
C:\Program Files (x86)\MarkingMate\sdk\ocx\CSharp_example\VS2017\LoadFile\LoadFile\bin\x64\Release\
```
如果不存在，請重新安裝 MarkingMate。

### Q4: 權限不足
**A:** 必須以**系統管理員身分**執行註冊腳本或命令。

---

## 📝 取消註冊

如果需要取消註冊（例如重新安裝前）：

1. **執行 UnregisterOCX.bat**
   - 右鍵 → 以系統管理員身分執行

2. **或使用命令**
   ```cmd
   regsvr32 /u MMEditx64Lib.dll
   regsvr32 /u MMMarkx64Lib.dll
   regsvr32 /u MMStatusx64Lib.dll
   ```

---

## 🎯 完整流程總結

1. ✅ **註冊 OCX 控件**
   - 執行 `RegisterOCX.bat`（以系統管理員身分）

2. ✅ **複製 DLL 到輸出目錄**
   - 執行 `CopyDlls_Simple.ps1`

3. ✅ **設定專案平台**
   - Visual Studio → 平台 → **x64**

4. ✅ **重新編譯**
   - 建置 → 清理方案
   - 建置 → 重建方案

5. ✅ **執行程式**
   - 偵錯 → 開始執行（F5）

---

## 📞 技術支援資訊

如果問題仍然存在，請檢查：

1. **MarkingMate 版本**
   - 確認已安裝最新版本
   - SDK 是否完整安裝

2. **系統需求**
   - Windows 版本（建議 Windows 10/11 x64）
   - .NET Framework 4.7.2 或更高版本
   - 管理員權限

3. **專案設定**
   - 平台目標：x64
   - 目標框架：.NET Framework 4.7.2
   - 引用路徑正確

---

**最後更新：** 2026-03-03
**適用於：** MarkingMate SDK v2.7
