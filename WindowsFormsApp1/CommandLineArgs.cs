using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    /// <summary>
    /// 命令列參數解析器
    /// </summary>
    public class CommandLineArgs
    {
        public bool IsAutoMode { get; private set; }
        public int BoardIndex { get; private set; }
        public string ConfigPath { get; private set; }
        public List<LineSegment> Lines { get; private set; }
        public bool AutoMark { get; private set; }
        public bool ShowHelp { get; private set; }
        public string DxfPath { get; private set; }
        public double WorkspaceSize { get; private set; }

        // 雷射參數（null 表示不設定，使用預設值）
        public double? Power { get; private set; }
        public double? Speed { get; private set; }
        public double? Frequency { get; private set; }
        public double? PulseWidth { get; private set; }
        public int? MarkRepeat { get; private set; }
        // 擺動參數（null 表示不設定）
        public double? WobbleWidth { get; private set; }
        public double? WobbleOverlap { get; private set; }
        public double? WobbleSpeed { get; private set; }
        // QR Code 參數
        public string QRContent { get; private set; }
        public double QRWidth { get; private set; }
        public double QRHeight { get; private set; }
        public double QRPosX { get; private set; }
        public double QRPosY { get; private set; }
        // 預覽模式：0=不預覽, 1=外框預覽, 2=全路徑預覽
        public int PreviewMode { get; private set; }
        public double? PreviewSpeed { get; private set; }
        public int PreviewTime { get; private set; }

        // Daemon / Client 模式：解決 SDK multi-process 限制
        // Daemon：常駐 process，一次 init 全 4 板，listen HTTP localhost:DaemonPort/cmd
        // Client：thin client，把命令 POST 到 daemon、收到 ExitCode 後結束（無 SDK 動作）
        public bool DaemonMode { get; private set; }
        public bool ClientMode { get; private set; }
        public int DaemonPort { get; private set; }

        // 是否使用者明確指定了 --config（影響後續是否要依 --board 自動推導預設值）
        private bool m_ConfigPathExplicit = false;

        public CommandLineArgs()
        {
            IsAutoMode = false;
            BoardIndex = 0;
            ConfigPath = "/cfg_config_MM1";
            Lines = new List<LineSegment>();
            AutoMark = false;
            ShowHelp = false;
            DxfPath = null;
            WorkspaceSize = 150.0;
            Power = null;
            Speed = null;
            Frequency = null;
            PulseWidth = null;
            MarkRepeat = null;
            WobbleWidth = null;
            WobbleOverlap = null;
            WobbleSpeed = null;
            QRContent = null;
            QRWidth = 10.0;
            QRHeight = 10.0;
            QRPosX = 0.0;
            QRPosY = 0.0;
            PreviewMode = 0;
            PreviewSpeed = null;
            PreviewTime = 15;
            DaemonMode = false;
            ClientMode = false;
            DaemonPort = 19527;
        }

        /// <summary>
        /// 解析命令列參數
        /// </summary>
        /// <param name="args">命令列參數陣列</param>
        /// <returns>解析後的參數物件</returns>
        public static CommandLineArgs Parse(string[] args)
        {
            var result = new CommandLineArgs();

            if (args != null && args.Length > 0)
            {
                result.IsAutoMode = true; // 只要有參數就設為 AutoMode
            }
            else
            {
                // 無參數則為 GUI 模式
                result.IsAutoMode = false;
                return result;
            }

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i]; // 不要轉小寫，因為 --lines 可能包含 DxfPath 或其他大小寫敏感參數嗎？不，參數名轉小寫，值不轉
                string argLower = arg.ToLower();

                if (argLower == "--help" || argLower == "-h" || argLower == "/?")
                {
                    result.ShowHelp = true;
                    result.IsAutoMode = true;
                    return result;
                }
                else if (argLower == "--board" || argLower == "-b")
                {
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int board))
                    {
                        result.BoardIndex = board;
                        i++;
                        result.IsAutoMode = true;
                    }
                }
                else if (argLower == "--config" || argLower == "-c")
                {
                    if (i + 1 < args.Length)
                    {
                        result.ConfigPath = args[i + 1];
                        result.m_ConfigPathExplicit = true;
                        i++;
                        result.IsAutoMode = true;
                    }
                }
                else if (argLower == "--line" || argLower == "-l")
                {
                    if (i + 1 < args.Length)
                    {
                        var line = LineSegment.Parse(args[i + 1]);
                        if (line != null)
                        {
                            result.Lines.Add(line);
                        }
                        i++;
                        result.IsAutoMode = true; // 確保設置為 true
                    }
                }
                else if (argLower == "--lines")
                {
                    if (i + 1 < args.Length)
                    {
                        // 支援多條線段，以分號分隔：x1,y1,x2,y2;x1,y1,x2,y2;...
                        string[] lineStrings = args[i + 1].Split(';');
                        foreach (string lineStr in lineStrings)
                        {
                            var line = LineSegment.Parse(lineStr);
                            if (line != null)
                            {
                                result.Lines.Add(line);
                            }
                        }
                        i++;
                        result.IsAutoMode = true;
                    }
                }
                else if (argLower == "--dxf" || argLower == "-d")
                {
                    if (i + 1 < args.Length)
                    {
                        result.DxfPath = args[i + 1];
                        i++;
                        result.IsAutoMode = true;
                    }
                }
                else if (argLower == "--workspace" || argLower == "-w")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double ws))
                    {
                        result.WorkspaceSize = ws;
                        i++;
                    }
                }
                else if (argLower == "--power" || argLower == "-p")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double power))
                    {
                        result.Power = power;
                        i++;
                    }
                }
                else if (argLower == "--speed" || argLower == "-s")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double speed))
                    {
                        result.Speed = speed;
                        i++;
                    }
                }
                else if (argLower == "--freq" || argLower == "-f")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double freq))
                    {
                        result.Frequency = freq;
                        i++;
                    }
                }
                else if (argLower == "--pulse-width" || argLower == "--pw")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double pw))
                    {
                        result.PulseWidth = pw;
                        i++;
                    }
                }
                else if (argLower == "--repeat" || argLower == "-r")
                {
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int repeat))
                    {
                        result.MarkRepeat = repeat;
                        i++;
                    }
                }
                else if (argLower == "--wobble-width" || argLower == "--wobble")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double wobbleWidth))
                    {
                        result.WobbleWidth = wobbleWidth;
                        i++;
                    }
                }
                else if (argLower == "--wobble-overlap")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double wobbleOverlap))
                    {
                        result.WobbleOverlap = wobbleOverlap;
                        i++;
                    }
                }
                else if (argLower == "--wobble-speed")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double wobbleSpeed))
                    {
                        result.WobbleSpeed = wobbleSpeed;
                        i++;
                    }
                }
                else if (argLower == "--preview")
                {
                    if (i + 1 < args.Length)
                    {
                        string modeLower = args[i + 1].ToLower();
                        if (modeLower == "outline" || modeLower == "box")
                        {
                            result.PreviewMode = 1; // 外框預覽
                            i++;
                        }
                        else if (modeLower == "full" || modeLower == "path")
                        {
                            result.PreviewMode = 2; // 全路徑預覽
                            i++;
                        }
                        else
                        {
                            result.PreviewMode = 2; // 預設全路徑
                        }
                    }
                    else
                    {
                        result.PreviewMode = 2; // 預設全路徑
                    }
                    result.AutoMark = true;
                    result.IsAutoMode = true;
                }
                else if (argLower == "--preview-speed")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double previewSpeed))
                    {
                        result.PreviewSpeed = previewSpeed;
                        i++;
                    }
                }
                else if (argLower == "--preview-time")
                {
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int previewTime) && previewTime > 0)
                    {
                        result.PreviewTime = previewTime;
                        i++;
                    }
                }
                else if (argLower == "--qrcode" || argLower == "-qr")
                {
                    if (i + 1 < args.Length)
                    {
                        result.QRContent = args[i + 1];
                        i++;
                        result.IsAutoMode = true;
                    }
                }
                else if (argLower == "--qr-width")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double qrWidth))
                    {
                        result.QRWidth = qrWidth;
                        i++;
                    }
                }
                else if (argLower == "--qr-height")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double qrHeight))
                    {
                        result.QRHeight = qrHeight;
                        i++;
                    }
                }
                else if (argLower == "--qr-x")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double qrX))
                    {
                        result.QRPosX = qrX;
                        i++;
                    }
                }
                else if (argLower == "--qr-y")
                {
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double qrY))
                    {
                        result.QRPosY = qrY;
                        i++;
                    }
                }
                else if (argLower == "--mark" || argLower == "-m")
                {
                    result.AutoMark = true;
                    result.IsAutoMode = true;
                }
                else if (argLower == "--daemon")
                {
                    result.DaemonMode = true;
                    result.IsAutoMode = true;
                }
                else if (argLower == "--client")
                {
                    result.ClientMode = true;
                    result.IsAutoMode = true;
                }
                else if (argLower == "--port")
                {
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int p) && p > 0 && p < 65536)
                    {
                        result.DaemonPort = p;
                        i++;
                    }
                }
            }

            // 若使用者沒明確指定 --config，依 --board 推導對應的預設 config 路徑。
            // 否則 board >0 卻用 cfg_config_MM1 會造成 InitialExt 配置錯誤、StartMarking 失敗。
            if (!result.m_ConfigPathExplicit)
            {
                result.ConfigPath = $"/cfg_config_MM{result.BoardIndex + 1}";
            }

            return result;
        }

        /// <summary>
        /// 取得說明文字
        /// </summary>
        public static string GetHelpText()
        {
            return @"MarkingMate Multi-Board 雷射打標系統

使用方式：
  MarkingMateMulti.exe                  啟動 GUI 模式
  MarkingMateMulti.exe [選項]           命令列模式

選項：
  --help, -h, /?                        顯示此說明訊息
  --board <0-3>, -b <0-3>               指定晶片板編號 (預設: 0)
  --config <path>, -c <path>            指定配置路徑 (預設: /cfg_config_MM1)
  --line <x1,y1,x2,y2>, -l <x1,y1,x2,y2>  新增單一線段
  --lines <線段列表>                    新增多條線段 (以分號分隔)
  --dxf <path>, -d <path>              載入 DXF 檔案（手動解析線段）
  --workspace <size>, -w <size>        工作區大小 mm (預設: 150)
  --power <0-100>, -p <0-100>          雷射功率 % (不指定則使用預設值)
  --speed <mm/s>, -s <mm/s>            打標速度 mm/s (不指定則使用預設值)
  --freq <kHz>, -f <kHz>               脈衝頻率 kHz (不指定則使用預設值)
  --pulse-width <val>, --pw <val>      脈波寬度 (不指定則使用預設值)
  --repeat <n>, -r <n>                 雷射次數 (不指定則使用預設值)
  --wobble-width <val>, --wobble <val> 擺動寬度 (不指定則不啟動擺動)
  --wobble-overlap <n>                  擺動重疊率 % (預設: 50)
  --wobble-speed <mm/s>                 擺動速度 mm/s (預設: 5026.55)
  --qrcode <content>, -qr <content>   QR Code 內容字串
  --qr-width <mm>                       QR Code 寬度 mm (預設: 10)
  --qr-height <mm>                      QR Code 高度 mm (預設: 10)
  --qr-x <mm>                           QR Code X 位置 mm (預設: 0)
  --qr-y <mm>                           QR Code Y 位置 mm (預設: 0)
  --mark, -m                            自動執行打標
  --preview <outline|full>               紅光預覽模式（不打雷射，需搭配 --mark）
                                          outline = 外框預覽, full = 全路徑預覽 (預設: full)
  --preview-speed <mm/s>                預覽速度 mm/s (不指定則使用預設值)
  --preview-time <秒>                   預覽持續時間 (預設: 15 秒)
  --daemon                              啟動常駐 daemon process（一次 init 全 4 板，
                                        listen HTTP localhost:port/cmd，接受 client
                                        命令派發到對應板）。繞過 SDK multi-process 限制。
  --client                              Thin client：把後續命令字串 POST 到 daemon，
                                        收到 ExitCode 後結束。不直接動 SDK。
  --port <N>                            Daemon 監聽 port（預設 19527）；client 連接 port。

並行模式：
  SDK 不支援多個 process 同時 init OCX。要讓多塊板平行動作的正確做法是
  跑一個 daemon process（一次 init 全 4 板），其餘 CLI / 網頁透過 HTTP 把
  命令送進 daemon，由 daemon 在同一 process 內派發到不同板（in-process
  parallel，SDK 已實測支援）。

範例：
  # 在板 0 上畫一條線並打標
  MarkingMateMulti.exe --board 0 --line 0,0,50,50 --mark

  # 載入 DXF 並指定雷射參數
  MarkingMateMulti.exe --board 0 --dxf ""File\test.dxf"" --power 50 --speed 800 --freq 20 --pw 5 --repeat 1 --mark

  # 在板 1 上畫多條線
  MarkingMateMulti.exe --board 1 --lines ""0,0,50,50;10,10,40,40"" --mark

  # 外框預覽（紅光描外框）
  MarkingMateMulti.exe --board 0 --dxf ""File\test.dxf"" --mark --preview outline

  # 全路徑預覽（紅光走完整路徑）
  MarkingMateMulti.exe --board 0 --dxf ""File\test.dxf"" --mark --preview full

  # 全路徑預覽並指定預覽速度
  MarkingMateMulti.exe --board 0 --dxf ""File\test.dxf"" --mark --preview full --preview-speed 500

  # 指定工作區大小 200mm 載入 DXF
  MarkingMateMulti.exe --board 0 --workspace 200 --dxf ""File\上翼板-2.dxf"" --mark

  # QR Code 打標
  MarkingMateMulti.exe --board 0 --qrcode ""Hello World"" --qr-width 10 --qr-height 10 --power 50 --speed 1000 --mark

  # QR Code 指定位置
  MarkingMateMulti.exe --board 0 --qrcode ""https://example.com"" --qr-x 5 --qr-y -5 --qr-width 15 --qr-height 15 --mark

  # 使用自訂配置
  MarkingMateMulti.exe --board 2 --config /cfg_config_MM3 --line 0,0,100,100

  # Daemon / Client 模式：繞過 SDK multi-process 限制，網頁也能呼叫
  # 步驟 1：先啟動 daemon（背景跑，一次 init 4 板）
  start """" MarkingMate.exe --daemon
  # 步驟 2：在 daemon 還活著的情況下，多個 client 可以連發命令到不同板
  MarkingMate.exe --client --board 0 --line 0,0,50,50 --mark --preview full --preview-time 10
  MarkingMate.exe --client --board 1 --qrcode ""TEST"" --mark --preview full --preview-time 8
  # 網頁端 JS（CORS 已允許 *）：
  #   fetch('http://localhost:19527/cmd', {
  #     method: 'POST',
  #     headers: { 'Content-Type': 'text/plain' },
  #     body: '--board 2 --line -20,-20,20,20 --mark --preview full'
  #   }).then(r => r.json()).then(r => console.log(r.exitCode, r.logs))
  # 停止 daemon：
  MarkingMate.exe --client --shutdown
  # 或直接：  curl -X POST http://localhost:19527/shutdown

雷射頭 IP 設定（重要）：
  IP 來源為「主表」：WindowsFormsApp1\Drivers\EMC6\DevIPAddress.ini
  - DEV0 → MM1 / CARD0    DEV1 → MM2 / CARD1
  - DEV2 → MM3 / CARD2    DEV3 → MM4 / CARD3
  初始化（含本 CLI 模式）會自動：
  1. 驗證主表 DEV0~DEV(--board) 已填入有效 IP，缺值即中止
  2. 依固定 CARD 對應，把主表 IP 同步到各 EMC6_MMx\DevIPAddress.ini 的 DEV0
  3. 把整個 Drivers\EMC6\ 目錄遞迴部署到 MarkingMate 安裝路徑
  4. 部署 config\config.ini 與各 config_MMx.ini、cfg\*.cfg
  若主表 IP 未填或部署失敗，CLI 會以 stderr 列出具體原因並回 ExitCode=1。
  IP 編輯目前僅 GUI 模式支援（在「1. 連接設定」頁填入後按「儲存IP」）。

結束代碼：
  0  - 成功
  1  - 初始化失敗（含 IP 主表未填、部署權限不足等）
  2  - 繪圖失敗
  3  - 打標失敗
  4  - 參數錯誤
";
        }

        /// <summary>
        /// 驗證參數是否有效
        /// </summary>
        public bool Validate(out string errorMessage)
        {
            if (BoardIndex < 0 || BoardIndex > 3)
            {
                errorMessage = "晶片板編號必須在 0-3 之間";
                return false;
            }

            if (Lines.Count == 0 && string.IsNullOrEmpty(DxfPath) && string.IsNullOrEmpty(QRContent) && AutoMark)
            {
                errorMessage = "自動打標模式至少需要一條線段、DXF 檔案或 QR Code 內容";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }

    /// <summary>
    /// 線段資料結構
    /// </summary>
    public class LineSegment
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public LineSegment(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        /// <summary>
        /// 從字串解析線段 (格式: x1,y1,x2,y2)
        /// </summary>
        public static LineSegment Parse(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;

            string[] parts = str.Split(',');
            if (parts.Length != 4)
                return null;

            if (double.TryParse(parts[0], out double x1) &&
                double.TryParse(parts[1], out double y1) &&
                double.TryParse(parts[2], out double x2) &&
                double.TryParse(parts[3], out double y2))
            {
                return new LineSegment(x1, y1, x2, y2);
            }

            return null;
        }

        public override string ToString()
        {
            return $"({X1}, {Y1}) -> ({X2}, {Y2})";
        }
    }
}
