using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RicePack
{
    static class Program
    {
        private static readonly string TracePath = Path.Combine(Path.GetTempPath(), "AGTExporter-startup.log");

        [STAThread]
        static void Main()
        {
            Trace("Main entered. BaseDirectory=" + AppDomain.CurrentDomain.BaseDirectory);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += OnThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            try
            {
                Trace("EnableVisualStyles");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Trace("Constructing MainForm");
                var form = new MainForm();
                Trace("MainForm constructed. Starting message loop.");

                form.Shown += delegate { Trace("MainForm shown."); };
                form.FormClosed += delegate(object sender, FormClosedEventArgs e)
                {
                    Trace("MainForm closed. CloseReason=" + e.CloseReason);
                };

                Application.Run(form);
                Trace("Application.Run returned normally.");
            }
            catch (Exception ex)
            {
                Trace("Startup exception: " + ex);
                ReportCrash("Startup failure", ex);
            }
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Trace("UI exception: " + e.Exception);
            ReportCrash("UI failure", e.Exception);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            Trace("Unhandled exception: " + Convert.ToString(e.ExceptionObject));
            ReportCrash("Unhandled failure", ex ?? new Exception(Convert.ToString(e.ExceptionObject)));
        }

        private static void Trace(string text)
        {
            try
            {
                File.AppendAllText(
                    TracePath,
                    DateTime.Now.ToString("O") + " " + text + Environment.NewLine,
                    Encoding.UTF8);
            }
            catch { }
        }

        private static void ReportCrash(string title, Exception ex)
        {
            var fullText = ex == null ? "Unknown error" : ex.ToString();

            try
            {
                var path = Path.Combine(Path.GetTempPath(), "AGTExporter-crash.log");
                var text = new StringBuilder()
                    .AppendLine(DateTime.Now.ToString("O"))
                    .AppendLine(title)
                    .AppendLine(fullText)
                    .AppendLine(new string('-', 80))
                    .ToString();
                File.AppendAllText(path, text, Encoding.UTF8);

                MessageBox.Show(
                    (ex == null ? "Unknown error" : ex.Message) +
                    "\r\n\r\nA full error report was written to:\r\n" + path +
                    "\r\n\r\nStartup trace:\r\n" + TracePath,
                    "AGT Exporter - " + title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch
            {
                try
                {
                    MessageBox.Show(fullText, "AGT Exporter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch { }
            }
        }
    }
}
