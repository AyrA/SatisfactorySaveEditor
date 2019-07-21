using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace SatisfactorySaveEditor
{
    public static class Log
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern void OutputDebugString(string message);

        private static TextWriter Logger;
        private static Stopwatch SW;
        public static string Logfile
        { get; private set; }

        static Log()
        {
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
        }

        public static void Write(string Text)
        {
            var Msg = string.Format("[{0}] {1}", SW.ElapsedMilliseconds, Text);
            lock (Logger)
            {
                if (Program.DEBUG)
                {
                    Debug.WriteLine(Msg);
                }
                OutputDebugString(Msg);
                Logger.WriteLine(Msg);
            }
        }

        public static void Write(string Text, params object[] Args)
        {
            Write(string.Format(Text, Args));
        }
    }
}
