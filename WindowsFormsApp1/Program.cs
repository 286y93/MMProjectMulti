using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            // 附加到父程序的控制台，讓命令列模式的 Console.WriteLine 可見
            if (args != null && args.Length > 0)
            {
                AttachConsole(ATTACH_PARENT_PROCESS);
            }

            // 先解析命令列參數，決定 Mutex 名稱
            CommandLineArgs cmdArgs;
            try
            {
                cmdArgs = CommandLineArgs.Parse(args);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"參數解析錯誤: {ex.Message}", "程式錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            // Client 模式：完全不開 Form、不持 Mutex，直接 HTTP POST 到 daemon
            if (cmdArgs.ClientMode)
            {
                return RunClientMode(args, cmdArgs.DaemonPort);
            }

            // daemon 運行中時，模式 A（單命令）不可直接碰 SDK：
            // 兩個 process 各自 init OCX 會撞 SDK 的 process-global 鎖，破壞共用的
            // EMC6 / MM27Dx64 驅動狀態，症狀是 "Please initial MMMark_1 OCX first!"，
            // 且因壞在驅動 / 全域鎖層級，重啟 daemon 也接不回來，往往需重開機才能恢復。
            // 因此偵測到 daemon 在跑時，即使漏帶 --client，也自動轉發成 client 派工
            // （HTTP POST 到 daemon，不自行 init OCX）。
            // 注意：自動轉發使用預設 / --port 指定的 port；自訂 port 的 daemon 仍需帶 --port。
            if (cmdArgs.IsAutoMode && !cmdArgs.DaemonMode && !cmdArgs.ShowHelp)
            {
                if (Mutex.TryOpenExisting("MarkingMateMulti_Daemon", out Mutex daemonMutex))
                {
                    daemonMutex.Dispose();
                    AttachConsole(ATTACH_PARENT_PROCESS);
                    Console.Error.WriteLine("Info: 偵測到 daemon 正在運行，自動改以 client 模式派工（避免撞 SDK OCX 鎖）。");
                    return RunClientMode(args, cmdArgs.DaemonPort);
                }
            }

            // 各模式使用不同 Mutex name 避免互鎖
            string mutexName;
            if (cmdArgs.DaemonMode)
                mutexName = "MarkingMateMulti_Daemon";
            else if (cmdArgs.IsAutoMode)
                mutexName = $"MarkingMateMulti_Board{cmdArgs.BoardIndex}";
            else
                mutexName = "MarkingMateMulti_SingleInstance";

            bool createdNew;
            using (Mutex mutex = new Mutex(true, mutexName, out createdNew))
            {
                if (!createdNew)
                {
                    if (cmdArgs.DaemonMode)
                    {
                        AttachConsole(ATTACH_PARENT_PROCESS);
                        Console.Error.WriteLine($"Error: 已有 daemon 在 port {cmdArgs.DaemonPort} 運行。一台機器只能有一個 daemon。");
                        return -3;
                    }
                    if (cmdArgs.IsAutoMode)
                    {
                        // CLI 模式：同板號已在執行中
                        AttachConsole(ATTACH_PARENT_PROCESS);
                        Console.Error.WriteLine($"Error: Board {cmdArgs.BoardIndex} is already being used by another instance.");
                        return -2;
                    }
                    else
                    {
                        MessageBox.Show(
                            "程式已經在執行中！\n\n" +
                            "如果看不到視窗，請：\n" +
                            "1. 開啟工作管理員 (Ctrl+Shift+Esc)\n" +
                            "2. 結束所有 WindowsFormsApp1.exe\n" +
                            "3. 等待 5 秒後重新啟動\n\n" +
                            "或執行 KillAllInstances.bat 自動清理",
                            "程式已執行",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return -1;
                    }
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                try
                {
                    // 如果是顯示說明
                    if (cmdArgs.ShowHelp)
                    {
                        MessageBox.Show(CommandLineArgs.GetHelpText(), "使用說明",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return 0; // 顯示幫助後正常結束
                    }

                    var form = new Form1(cmdArgs);
                    Application.Run(form);
                    return form.ExitCode;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"執行錯誤: {ex.Message}\n\n{ex.StackTrace}", "程式錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            } // Mutex will be released here
        }

        /// <summary>
        /// Client mode：把命令字串 POST 到 localhost daemon，等回應後印出並回傳 ExitCode。
        /// 命令字串 = 原始 argv 排除 --client / --port N 之後重新串接。
        /// 特殊：若有 --shutdown，呼叫 /shutdown endpoint。
        /// </summary>
        private static int RunClientMode(string[] args, int port)
        {
            AttachConsole(ATTACH_PARENT_PROCESS);

            bool shutdown = false;
            var passthrough = new List<string>();
            for (int i = 0; i < args.Length; i++)
            {
                string a = args[i].ToLower();
                if (a == "--client") continue;
                if (a == "--port") { if (i + 1 < args.Length) i++; continue; }
                if (a == "--shutdown") { shutdown = true; continue; }
                passthrough.Add(args[i]);
            }

            string url = $"http://localhost:{port}/" + (shutdown ? "shutdown" : "cmd");
            string body = string.Join(" ", passthrough.Select(QuoteIfNeeded));

            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "text/plain; charset=utf-8";
                req.Timeout = 5 * 60 * 1000; // 5 分鐘上限，preview-time 最長設計

                if (!shutdown)
                {
                    byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
                    req.ContentLength = bodyBytes.Length;
                    using (var stream = req.GetRequestStream())
                    {
                        stream.Write(bodyBytes, 0, bodyBytes.Length);
                    }
                }
                else
                {
                    req.ContentLength = 0;
                }

                using (var resp = (HttpWebResponse)req.GetResponse())
                using (var rs = resp.GetResponseStream())
                using (var sr = new StreamReader(rs, Encoding.UTF8))
                {
                    string text = sr.ReadToEnd();
                    Console.WriteLine(text);

                    if (shutdown) return 0;

                    // 從回應 JSON 抽出 exitCode
                    var m = Regex.Match(text, "\"exitCode\"\\s*:\\s*(-?\\d+)");
                    if (m.Success && int.TryParse(m.Groups[1].Value, out int ec))
                        return ec;
                    return 0;
                }
            }
            catch (WebException wex)
            {
                Console.Error.WriteLine($"Error: 無法連線到 daemon ({url}): {wex.Message}");
                Console.Error.WriteLine($"  請確認 daemon 已啟動：MarkingMate.exe --daemon");
                return 6;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: client 例外 - {ex.Message}");
                return 7;
            }
        }

        private static string QuoteIfNeeded(string s)
        {
            if (string.IsNullOrEmpty(s)) return "\"\"";
            if (s.IndexOfAny(new[] { ' ', '\t', '"' }) < 0) return s;
            return "\"" + s.Replace("\"", "\\\"") + "\"";
        }
    }
}
