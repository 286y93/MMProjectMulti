using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /// <summary>
    /// HTTP-based daemon — 繞過 SDK multi-process 限制。
    /// 由 Form1 持有，所有 SDK 動作都透過 Form1.RunDaemonSpec 在 UI thread 上執行。
    /// </summary>
    internal class MarkingMateDaemon
    {
        private readonly Form1 m_Form;
        private readonly int m_Port;
        private HttpListener m_Listener;
        private Thread m_AcceptThread;
        private volatile bool m_Running;

        public MarkingMateDaemon(Form1 form, int port)
        {
            m_Form = form;
            m_Port = port;
        }

        public void Start()
        {
            m_Listener = new HttpListener();
            // 只 bind 127.0.0.1，避免被同網段其他機器存取
            m_Listener.Prefixes.Add($"http://127.0.0.1:{m_Port}/");
            m_Listener.Prefixes.Add($"http://localhost:{m_Port}/");
            m_Listener.Start();
            m_Running = true;
            m_AcceptThread = new Thread(AcceptLoop) { IsBackground = true, Name = "MMDaemonAccept" };
            m_AcceptThread.Start();
        }

        public void Stop()
        {
            m_Running = false;
            try { m_Listener?.Stop(); } catch { }
            try { m_Listener?.Close(); } catch { }
        }

        private void AcceptLoop()
        {
            while (m_Running)
            {
                HttpListenerContext ctx;
                try { ctx = m_Listener.GetContext(); }
                catch { break; }
                ThreadPool.QueueUserWorkItem(_ => HandleRequest(ctx));
            }
        }

        private void HandleRequest(HttpListenerContext ctx)
        {
            try
            {
                // CORS：允許任何 origin，方便網頁端整合
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                if (string.Equals(ctx.Request.HttpMethod, "OPTIONS", StringComparison.OrdinalIgnoreCase))
                {
                    ctx.Response.StatusCode = 204;
                    ctx.Response.Close();
                    return;
                }

                string path = ctx.Request.Url.AbsolutePath?.TrimEnd('/').ToLowerInvariant() ?? "";
                switch (path)
                {
                    case "":
                    case "/health":
                        WriteText(ctx, 200, "MarkingMate daemon OK");
                        return;

                    case "/shutdown":
                        WriteText(ctx, 200, "shutting down");
                        m_Form.BeginInvoke((Action)(() =>
                        {
                            try { m_Form.Close(); } catch { }
                        }));
                        return;

                    case "/cmd":
                        HandleCmd(ctx);
                        return;

                    default:
                        WriteText(ctx, 404, $"Not Found: {path}");
                        return;
                }
            }
            catch (Exception ex)
            {
                try { WriteText(ctx, 500, "Server Error: " + ex.Message); } catch { }
            }
        }

        private void HandleCmd(HttpListenerContext ctx)
        {
            if (!string.Equals(ctx.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
            {
                WriteText(ctx, 405, "Method Not Allowed (use POST)");
                return;
            }

            string body;
            using (var sr = new StreamReader(ctx.Request.InputStream, ctx.Request.ContentEncoding ?? Encoding.UTF8))
            {
                body = sr.ReadToEnd();
            }
            if (string.IsNullOrWhiteSpace(body))
            {
                WriteJson(ctx, 400, new Form1.DaemonSpecResult { ExitCode = 4, Logs = "empty body" });
                return;
            }

            Task<Form1.DaemonSpecResult> task;
            try { task = m_Form.RunDaemonSpec(body); }
            catch (Exception ex)
            {
                WriteJson(ctx, 500, new Form1.DaemonSpecResult { ExitCode = 1, Logs = $"dispatch failed: {ex.Message}" });
                return;
            }

            // 同步等 spec 完成（HTTP request thread block 在這）
            // 最長 5 分鐘，避免 preview-time 設太長或卡住
            Form1.DaemonSpecResult result;
            try
            {
                if (!task.Wait(TimeSpan.FromMinutes(5)))
                {
                    WriteJson(ctx, 504, new Form1.DaemonSpecResult { ExitCode = 3, Logs = "daemon timeout (>5min)" });
                    return;
                }
                result = task.Result;
            }
            catch (AggregateException aex)
            {
                result = new Form1.DaemonSpecResult { ExitCode = 1, Logs = $"task exception: {aex.InnerException?.Message ?? aex.Message}" };
            }

            WriteJson(ctx, 200, result);
        }

        private static void WriteText(HttpListenerContext ctx, int status, string text)
        {
            byte[] buf = Encoding.UTF8.GetBytes(text);
            ctx.Response.StatusCode = status;
            ctx.Response.ContentType = "text/plain; charset=utf-8";
            ctx.Response.ContentLength64 = buf.Length;
            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
            ctx.Response.OutputStream.Close();
        }

        private static void WriteJson(HttpListenerContext ctx, int status, Form1.DaemonSpecResult result)
        {
            string json = $"{{\"exitCode\":{result.ExitCode},\"logs\":\"{EscapeJson(result.Logs ?? "")}\"}}";
            byte[] buf = Encoding.UTF8.GetBytes(json);
            ctx.Response.StatusCode = status;
            ctx.Response.ContentType = "application/json; charset=utf-8";
            ctx.Response.ContentLength64 = buf.Length;
            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
            ctx.Response.OutputStream.Close();
        }

        private static string EscapeJson(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            var sb = new StringBuilder(s.Length + 16);
            foreach (var c in s)
            {
                switch (c)
                {
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (c < 0x20)
                            sb.Append("\\u").Append(((int)c).ToString("x4"));
                        else
                            sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
