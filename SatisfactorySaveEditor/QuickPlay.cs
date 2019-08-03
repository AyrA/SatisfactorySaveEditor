using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Provides a QuickPlay obtain and update mechanism
    /// </summary>
    public static class QuickPlay
    {
        /// <summary>
        /// Expected QuickPlay hash
        /// </summary>
        private const string QP_HASH = "888178F6B888A295DB8269CF12B8803BAF2065B6";
        /// <summary>
        /// QuickPlay download URL
        /// </summary>
        private const string QP_URL = @"https://cable.ayra.ch/satisfactory/QuickPlay.exe";
        /// <summary>
        /// Default number of retries
        /// </summary>
        private const int DEFAULT_RETRIES = 5;

        /// <summary>
        /// Gets if QuickPlay is available
        /// </summary>
        public static bool HasQuickPlay
        { get; private set; }

        /// <summary>
        /// Number of Retries to download so far
        /// </summary>
        public static int QuickPlayTries
        { get; private set; }

        /// <summary>
        /// Gets or sets the maximum number of download retries
        /// </summary>
        public static int MaximumRetries
        { get; set; }

        /// <summary>
        /// Gets the Path+Name of QuickPlay
        /// </summary>
        public static string QuickPlayPath
        {
            get
            {
                return Path.Combine(UpdateHandler.ExecutablePath, "QuickPlay.exe");
            }
        }

        static QuickPlay()
        {
            MaximumRetries = DEFAULT_RETRIES;
        }

        public static void ResetQuickPlay()
        {
            try
            {
                File.Delete(QuickPlayPath);
            }
            catch (Exception ex)
            {
                Log.Write(new Exception("Unable to reset QuickPlay because we can't delete the file.", ex));
            }
            HasQuickPlay = File.Exists(QuickPlayPath);
            if (!HasQuickPlay)
            {
                QuickPlayTries = 0;
            }
            var F = Tools.GetForm<frmMain>();
            if(F!=null)
            {
                F.QPComplete(new Exception("QuickPlay was reset"));
            }
        }

        /// <summary>
        /// Attempts to obtain QuickPlay
        /// </summary>
        public static void CheckQuickPlay()
        {
            if (!File.Exists(QuickPlayPath))
            {
                ++QuickPlayTries;
                HasQuickPlay = false;
                var F = Tools.GetForm<frmMain>();
                if(F!=null)
                {
                    F.QPProgress(0);
                }
                WebClient WC = new WebClient();
                WC.Headers.Add("User-Agent", UpdateHandler.UserAgent);
                WC.DownloadProgressChanged += WC_DownloadProgressChanged;
                WC.DownloadFileCompleted += WC_DownloadFileCompleted;
                try
                {
                    WC.DownloadFileAsync(new Uri(QP_URL), QuickPlayPath);
                }
                catch (Exception ex)
                {
                    Log.Write(new Exception("Unable to 'DownloadFileAsync' QuickPlay", ex));
                    HasQuickPlay = false;
                }
            }
            else
            {
                if (!IsQuickPlayValid())
                {
                    HasQuickPlay = false;
                    //Hash invalid. Get new copy
                    //But not immediately
                    Thread.Sleep(1000);
                    try
                    {
                        File.Delete(QuickPlayPath);
                        CheckQuickPlay();
                    }
                    catch (Exception ex)
                    {
                        Log.Write(new Exception("Unable to retry QuickPlay Download", ex));
                    }
                }
                else
                {
                    HasQuickPlay = true;
                }
            }
        }

        private static bool IsQuickPlayValid()
        {
            if (File.Exists(QuickPlayPath))
            {
                try
                {
                    using (var FS = File.OpenRead(QuickPlayPath))
                    {
                        return Tools.GetHash(FS) == QP_HASH;
                    }
                }
                catch(Exception ex)
                {
                    Log.Write(new Exception("Unable to test QuickPlay Hash", ex));
                }
            }
            return false;
        }

        private static void WC_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var WC = (WebClient)sender;
            var F = Tools.GetForm<frmMain>();
            if (F != null && e.Cancelled)
            {
                F.QPComplete(new Exception("The user cancelled the operation"));
            }
            //Check QuickPlay after an update but not too often to avoid infinite loops
            if (QuickPlayTries < MaximumRetries)
            {
                if (F != null)
                {
                    F.QPComplete(e.Error);
                }
                //On some network errors, this is fired immediately, so we wait for a second.
                Thread.Sleep(1000);
                CheckQuickPlay();
            }
            else
            {
                if (F != null)
                {
                    F.QPComplete(new Exception($"QuickPlay download failed after {MaximumRetries} attempts"));
                }
            }
            WC.Dispose();
        }

        private static void WC_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive > e.BytesReceived)
            {
                var F = Tools.GetForm<frmMain>();
                if (F != null)
                {
                    var Perc = (int)(e.BytesReceived * 100 / e.TotalBytesToReceive);
                    F.QPProgress(Perc);
                }
            }
        }
    }
}
