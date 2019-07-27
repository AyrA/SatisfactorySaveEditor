using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Http;

namespace SatisfactorySaveEditor
{
    public delegate void ErrorReportHandler(Exception ex, ErrorHandlerEventArgs e);

    public class ErrorHandlerEventArgs
    {
        public bool Handled
        { get; set; }
    }

    public class ErrorHandler
    {
        public string LastReportResult
        { get; private set; }

        public event ErrorReportHandler ErrorReport;

        /// <summary>
        /// Creates a new ErrorHandler instance
        /// </summary>
        public ErrorHandler()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
            {
                Log.Write("{0}: Unhandled exception", nameof(ErrorHandler));
                Log.Write((Exception)e.ExceptionObject);
            }
            else
            {
                Log.Write("{0}: Unhandled Exception: {1}", nameof(ErrorHandler), e.ExceptionObject == null ? "<no data given>" : e.ExceptionObject.ToString());
            }
            using (var f = new frmErrorHandler((Exception)e.ExceptionObject, this))
            {
                f.ShowDialog();
            }
            Log.Write("{0}: Application terminating abnormally", nameof(ErrorHandler));
            Log.Close();
            Process P = Process.GetCurrentProcess();
            P.Kill();
            P.WaitForExit();
        }

        /// <summary>
        /// generates a report from the exception
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>report</returns>
        public static string generateReport(Exception ex)
        {
            StringBuilder SB = new StringBuilder();
            Exception e = ex;
            SB.AppendLine(@"An error happened that was not handled by the application.
You can find the report below.
Because this was not handled, the application has to terminate now.

============
Error Report
============
");
            while (e != null)
            {
                SB.Append(FormatException(e));
                if (e is AggregateException)
                {
                    foreach (var aEx in ((AggregateException)e).InnerExceptions)
                    {
                        SB.Append(FormatException(aEx));
                    }
                }
                e = ex.InnerException;
            }
            SB.AppendLine(@"=================
== Environment ==
=================

");
            foreach (var KV in GetEnv())
            {
                //Replace CR and LF with spaces
                var Value = KV.Value.Replace('\r', ' ').Replace('\n', ' ');
                //Remove username from value
                Value = Regex.Replace(Value, Regex.Escape(Environment.UserName), "<USERNAME>", RegexOptions.IgnoreCase);
                //Avoid logging potentially unsafe variables
                if (!Regex.IsMatch($"{KV.Key}={Value}", "(password|pwd|login)", RegexOptions.IgnoreCase))
                {
                    SB.AppendFormat("{0}={1}\r\n", KV.Key, Value);
                }
            }
            return SB.ToString();
        }

        private static string FormatException(Exception ex)
        {
            if (ex != null)
            {
                var SB = new StringBuilder();

                var Data = "<none>";

                if (ex.Data.Count > 0)
                {
                    foreach (var Entry in (Dictionary<string, object>)ex.Data)
                    {
                        Data += string.Format("{0}={1}\r\n", Entry.Key, Entry.Value);
                    }
                }

                SB.AppendFormat(@"Name: {0}
Description: {1}
Source: {2}
Data: {3}
Trace:
{4}

",
    exName(ex),
    string.IsNullOrEmpty(ex.Message) ? "<none>" : ex.Message,
    string.IsNullOrEmpty(ex.Source) ? "<none>" : ex.Source,
    Data.Trim(),
    string.IsNullOrEmpty(ex.StackTrace) ? "<none>" : ex.StackTrace);

                return SB.ToString();
            }
            return string.Empty;
        }

        public bool UploadReport(string ErrorMessage, byte[] SaveFileData)
        {
            Log.Write("{0}: Uploading error report", nameof(ErrorHandler));
            try
            {
                using (var C = new HttpClient())
                {
                    C.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UpdateHandler.UserAgent);
                    using (var form = new MultipartFormDataContent(Guid.NewGuid().ToString()))
                    {
                        using (var EM = new StringContent(ErrorMessage))
                        {
                            using (var VM = new StringContent(Tools.CurrentVersion.ToString()))
                            {
                                using (var LM = new StringContent(string.Join("\r\n", Log.GetMessages())))
                                {
                                    using (var FD = new ByteArrayContent(SaveFileData))
                                    {
                                        //Version message
                                        form.Add(VM, "version");
                                        //Log Data
                                        form.Add(LM, "log");
                                        //Error message
                                        form.Add(EM, "error");
                                        //File data
                                        form.Add(FD, "file", "file.sav");
                                        using (var res = C.PostAsync("https://cable.ayra.ch/satisfactory/error/", form).Result)
                                        {
                                            Log.Write(LastReportResult = res.Content.ReadAsStringAsync().Result);
                                            res.EnsureSuccessStatusCode();
                                            Log.Write("{0}: Error report sent", nameof(ErrorHandler));
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write("{0}: Unable to uplaod error report", nameof(ErrorHandler));
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        /// Gets the name of an exception
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>Exception name</returns>
        public static string exName(Exception ex)
        {
            return ex == null ? "<Unknown Exception>" : ex.GetType().Name;
        }

        /// <summary>
        /// raises the ErrorReport event
        /// </summary>
        /// <param name="ex">Exception</param>
        public bool raise(Exception ex)
        {
            if (ErrorReport != null)
            {
                var e = new ErrorHandlerEventArgs();
                ErrorReport(ex, e);
                return e.Handled;
            }
            return false;
        }

        public static Dictionary<string, string> GetEnv()
        {
            var Dict = new Dictionary<string, string>();
            foreach (var Entry in Environment.GetEnvironmentVariables())
            {
                Dict.Add(
                    Entry.GetType().GetProperty("Key").GetValue(Entry).ToString(),
                    Entry.GetType().GetProperty("Value").GetValue(Entry).ToString()
                    );
            }
            return Dict;
        }
    }
}
