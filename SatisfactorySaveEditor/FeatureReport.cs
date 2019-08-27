using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactorySaveEditor
{
    public static class FeatureReport
    {
        public const string REPORT_URL = "https://cable.ayra.ch/satisfactory/report/";

        public enum Feature : int
        {
            OpenFile,
            OpenByCommandLine,
            Manager,
            SaveFile,
            QuickAction,
            DoBackup,
            RestoreBackup,
            EditHeader,
            CountItems,
            DuplicateItems,
            DeleteByType,
            DeleteByArea,
            XmlExport,
            XmlImport,
            ClearStringList,
            ExtractAudio,
            ChangeSettings,
            DisableReport,
            HelpRequest,
            ViewChanges,
            /// <summary>
            /// Application is running in debug mode
            /// </summary>
            DebugMode,
            /// <summary>
            /// Application was terminated due to an unhandled exception
            /// </summary>
            TerminateByError,
            /// <summary>
            /// The image renderer crashed
            /// </summary>
            RendererCrash
        }

        public static Guid Id
        { get; set; }

        private static List<Feature> UsedFeatures;

        static FeatureReport()
        {
            UsedFeatures = new List<Feature>();
            Id = Guid.NewGuid();
        }

        public static void Used(Feature F)
        {
            UsedFeatures.Add(F);
        }

        public static void Report()
        {
            //Don't bother reporting if it doesn't works
            if (Id == Guid.Empty)
            {
                Log.Write("{0}: Reporting Failed. ID not set", nameof(FeatureReport));
                return;
            }
            //Prevent multiple reports from stacking up
            lock (UsedFeatures)
            {
                //Don't bother reporting if nothing is there to report
                if (UsedFeatures.Count > 0)
                {
                    var BaseFields = new string[] {
                    $"id={Id}",
                    $"version={Tools.CurrentVersion}"
                };
                    WebResponse Res = null;
                    var Req = WebRequest.CreateHttp(REPORT_URL);
                    Req.UserAgent = UpdateHandler.UserAgent;
                    Req.Method = "POST";
                    Req.ContentType = "application/x-www-form-urlencoded";
                    try
                    {
                        var Features = Encoding.UTF8.GetBytes(string.Join("&", UsedFeatures.Distinct().Select(m => $"feature[]={m}").Concat(BaseFields)));
                        Req.ContentLength = Features.Length;
                        using (var S = Req.GetRequestStream())
                        {
                            S.Write(Features, 0, Features.Length);
                            S.Close();
                        }
                        Res = Req.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        try
                        {
                            using (Res = ex.Response)
                            {
                                using (var SR = new StreamReader(Res.GetResponseStream()))
                                {
#if DEBUG
                                    Tools.E(SR.ReadToEnd(), "DEBUG Report Error");
#else
                                    Log.Write("{0}: Reporting failed. Data: {1}", nameof(FeatureReport), SR.ReadToEnd());
#endif
                                }
                            }
                        }
                        catch
                        {
                            Log.Write("{0}: Reporting failed (unable to provide a reason, response is null)", nameof(FeatureReport));
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        Log.Write("{0}: Reporting failed (unable to provide a reason, not a WebException)", nameof(FeatureReport));
                        Log.Write(ex);
                    }
                    //Report OK. Clear list and log message
                    UsedFeatures.Clear();
                    using (Res)
                    {
                        using (var SR = new StreamReader(Res.GetResponseStream()))
                        {
                            Log.Write("{0}: Reporting OK. Data: {1}", nameof(FeatureReport), SR.ReadToEnd());
                        }
                    }
                }
            }
        }
    }
}
