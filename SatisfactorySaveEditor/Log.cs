using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace SatisfactorySaveEditor
{
    public static class Log
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern void OutputDebugString(string message);

        private static TextWriter Logger;
        private static Stopwatch SW;
        private static List<string> LogMessages;

        public static string Logfile
        { get; private set; }

        static Log()
        {
            LogMessages = new List<string>();
            SW = new Stopwatch();
            SW.Start();
            using (var P = Process.GetCurrentProcess())
            {
                try
                {
                    //Try logging into application directory first
                    Logger = File.CreateText(Logfile = Path.Combine(Path.GetDirectoryName(P.MainModule.FileName), "lastrun.log"));
                }
                catch
                {
                    try
                    {
                        //Try temporary file on errors
                        var F = Path.GetTempFileName();
                        File.Delete(F);
                        Path.ChangeExtension(F, ".log");
                        Logger = File.CreateText(Logfile = F);
                    }
                    catch
                    {
                        //Give up
                        Logfile = "<null>";
                        Logger = TextWriter.Null;
                    }
                }
            }
            var DT = DateTime.UtcNow;
            Write("{0} {1}: Logger initialized", DT.ToLongDateString(), DT.ToLongTimeString());
            //Flush logger periodically
            Thread T = new Thread(delegate ()
            {
                while (Logger != null && Logger != TextWriter.Null)
                {
                    Thread.Sleep(5000);
                    lock (Logger)
                    {
                        Logger.Flush();
                    }
                }
            });
            T.IsBackground = true;
            T.Start();
        }

        public static string[] GetMessages()
        {
            lock (Logger)
            {
                return LogMessages.ToArray();
            }
        }

        public static void Close()
        {
            lock (Logger)
            {
                if (Logger != TextWriter.Null)
                {
                    var DT = DateTime.UtcNow;
                    Write("{0} {1}: Logger ended", DT.ToLongDateString(), DT.ToLongTimeString());
                    Logger.Flush();
                    Logger.Close();
                    Logger.Dispose();
                    Logger = TextWriter.Null;
                }
            }
        }

        public static void Write(string Text)
        {
            var Msg = string.Format("[{0,9}] {1}", SW.ElapsedMilliseconds, Text);
            lock (Logger)
            {
                if (Program.DEBUG)
                {
                    Debug.WriteLine(Msg);
                    OutputDebugString(Msg);
                }
                Logger.WriteLine(Msg);
                LogMessages.Add(Msg);
                if (LogMessages.Count > 1000)
                {
                    LogMessages.RemoveRange(1, LogMessages.Count - 999);
                    LogMessages[0] = "<Log Truncated>";
                }
            }
        }

        public static void Write(string Text, params object[] Args)
        {
            Write(string.Format(Text, Args));
        }

        public static void Write(Exception ex, bool IsRoot = true)
        {
            if (ex == null)
            {
                return;
            }
            if (IsRoot)
            {
                Write("=== START: {0} handler ===", ex.GetType().FullName);
            }
            Write("Error: {0}", ex.Message);
            Write("Stack: {0}", ex.StackTrace);
            if (ex.Data != null && ex.Data.Count > 0)
            {
                foreach (var Entry in ex.Data)
                {
                    Write("Data: {0}", Entry);
                }
            }
            if (ex is AggregateException)
            {
                foreach (var E in ((AggregateException)ex).InnerExceptions)
                {
                    Write(E, false);
                }
            }
            else
            {
                Write(ex.InnerException, false);
            }
            if (IsRoot)
            {
                Write("=== END: {0} handler ===", ex.GetType().FullName);
                lock (Logger)
                {
                    //Always flush on exceptions
                    Logger.Flush();
                }
            }
        }
    }
}
