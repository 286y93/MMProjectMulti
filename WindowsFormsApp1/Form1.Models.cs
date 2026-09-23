using System.Collections.Generic;

namespace WindowsFormsApp1
{
    // Form1 的資料類別（自 Form1.cs 抽出，partial 拆檔，行為與存取範圍不變）。
    public partial class Form1
    {
        // Daemon 單筆命令執行結果
        public class DaemonSpecResult
        {
            public int ExitCode;
            public string Logs;
        }

        // 部署統計：用於 skip-if-same 機制下追蹤實際寫入 / 跳過數量
        private class DeployStats
        {
            public int Written;
            public int Skipped;
        }

        // 白底反相 QR（白底矩形 + 反相 QR 雙圖層）可調參數。
        // 其餘製程常數（填滿間距 / 線段遍數等）維持寫死於 BuildWhiteBgQR。
        private class WhiteBgQRParams
        {
            public string Content = "1234567";
            public double QrWidth = 25.0;    // QR 資料模組區寬 (mm)
            public double QrHeight = 25.0;   // QR 資料模組區高 (mm)
            public int Border = 2;           // 外框單元數 (cell)
            public double QrSpeed = 1200;    // QR 打標速度
            public double QrPower = 90;      // QR 功率
            public double QrFreq = 80;       // QR 頻率 (kHz)
            public double QrPulseWidth = 30; // QR 脈波寬度
            public double RectSpeed = 800;   // 白底矩形速度
            public double RectPower = 100;   // 白底矩形功率
            public double RectFreq = 80;     // 白底矩形頻率 (kHz)
            public double RectPulseWidth = 250; // 白底矩形脈波寬度
            public double RectExtra = 0;     // 矩形額外加大量 (mm)
        }

        // 命令提示頁籤：單筆指令規格（隨機產生 / 解析既有命令列共用）
        private class CmdPreviewSpec
        {
            public int BoardIndex;
            public List<LineSegment> Lines;     // 線段內容（單條或多條）；null = 非線段類
            public string QRContent;            // QR 內容；null = 非 QR
            public double QRWidth, QRHeight;
            public bool QRInvert;               // QR 反相（黑白互換）
            public int PreviewMode;             // 0=正式打標, 1=outline 預覽, 2=full 預覽
            public int PreviewTime;             // 預覽模式秒數；PreviewMode=0 時忽略
            public double? WobbleWidth;         // 線條寬度 mm（雷射加粗），null=不啟動 wobble
            public double? WobbleSpeed;         // 擺動速度 mm/s，null=用 SDK 預設 5026.55
            public bool QRWhiteBg;              // 白底反相 QR（--qr-whitebg）
            public CommandLineArgs Cli;         // 解析後的原始 CLI 參數（白底 QR 用來組 WhiteBgQRParams）
            public string DisplayText;          // 顯示在 textbox 內的命令字串
        }
    }
}
