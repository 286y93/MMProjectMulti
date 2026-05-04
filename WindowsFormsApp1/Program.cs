using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

            // CLI 模式：每塊板各自 single instance（允許不同板號並行）
            // GUI 模式：全域 single instance（防止 EZDrawPlatform 衝突）
            string mutexName = cmdArgs.IsAutoMode
                ? $"MarkingMateMulti_Board{cmdArgs.BoardIndex}"
                : "MarkingMateMulti_SingleInstance";

            bool createdNew;
            using (Mutex mutex = new Mutex(true, mutexName, out createdNew))
            {
                if (!createdNew)
                {
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
    }
}
