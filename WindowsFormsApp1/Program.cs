using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            // 確保只有一個實例執行（防止 EZDrawPlatform 衝突）
            bool createdNew;
            using (Mutex mutex = new Mutex(true, "MarkingMateMulti_SingleInstance", out createdNew))
            {
                if (!createdNew)
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

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                try
                {
                    // 解析命令列參數
                    CommandLineArgs cmdArgs = CommandLineArgs.Parse(args);

                    // 如果是顯示說明
                    if (cmdArgs.ShowHelp)
                    {
                        MessageBox.Show(CommandLineArgs.GetHelpText(), "使用說明",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return 0; // 顯示幫助後正常結束
                    }

                    Application.Run(new Form1(cmdArgs));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"執行錯誤: {ex.Message}\n\n{ex.StackTrace}", "程式錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }

                return 0;
            } // Mutex will be released here
        }
    }
}
