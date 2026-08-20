using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RicePack
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += OnThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                ReportCrash("Startup failure", ex);
            }
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ReportCrash("UI failure", e.Exception);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            ReportCrash("Unhandled failure", ex ?? new Exception(Convert.ToString(e.ExceptionObject)));
        }

        private static void ReportCrash(string title, Exception ex)
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AGTExporter-crash.log");
                var text = new StringBuilder()
                    .AppendLine(DateTime.Now.ToString("O"))
                    .AppendLine(title)
                    .AppendLine(ex == null ? "Unknown error" : ex.ToString())
                    .AppendLine(new string('-', 80))
                    .ToString();
                File.AppendAllText(path, text);

                MessageBox.Show(
                    (ex == null ? "Unknown error" : ex.Message) +
                    "\r\n\r\nA full error report was written to:\r\n" + path,
                    "AGT Exporter - " + title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch
            {
                try
                {
                    MessageBox.Show(ex == null ? "Unknown error" : ex.ToString(), "AGT Exporter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch { }
            }
        }
    }
}
