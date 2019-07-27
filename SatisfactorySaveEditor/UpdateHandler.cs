using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Handles updating of this application
    /// </summary>
    public static class UpdateHandler
    {
        /// <summary>
        /// Information from the update API
        /// </summary>
        private struct UpdateInfo
        {
            /// <summary>
            /// Reported version string
            /// </summary>
            public Version NewVersion;
            /// <summary>
            /// Reported Update URL
            /// </summary>
            public Uri DownloadURL;
        }

        /// <summary>
        /// Update Info URL
        /// </summary>
        public const string UPDATE_URL = "https://cable.ayra.ch/satisfactory/api/info";
        /// <summary>
        /// The default name used for the update executable if it's not given
        /// </summary>
        public static readonly string DefaultUpdateExecutable;
        /// <summary>
        /// Current executable path and name
        /// </summary>
        public static readonly string CurrentExecutable;
        /// <summary>
        /// User agent string that is used for requests
        /// </summary>
        public static readonly string UserAgent;

        /// <summary>
        /// Update data
        /// </summary>
        private static UpdateInfo UpdateData;
        /// <summary>
        /// Info ready status
        /// </summary>
        private static bool InfoReady = false;

        static UpdateHandler()
        {
            Log.Write("Initialize UpdateHandler");
            //Enable all Protocols but not SSL3
            //The API might get mad at you if you don't enable the new TLS versions
            //The reason it's done "dynamically" here is to support future protocols automatically.
            var Protocols = (SecurityProtocolType)Enum
                .GetValues(typeof(SecurityProtocolType))
                .OfType<SecurityProtocolType>()
                .Cast<int>()
                .Sum() & ~SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = Protocols;
            using (var P = Process.GetCurrentProcess())
            {
                var N = P.MainModule.FileName;
                DefaultUpdateExecutable = Path.Combine(Path.GetDirectoryName(N),
                    Path.GetFileNameWithoutExtension(N) +
                    "_update" +
                    Path.GetExtension(N));
                CurrentExecutable = N;
            }
            UserAgent = $"AyrA-SatisfactorySaveEditor/{Tools.CurrentVersion} +https://github.com/AyrA/SatisfactorySaveEditor";
        }

        /// <summary>
        /// Gets a request object
        /// </summary>
        /// <param name="URL">Request URL</param>
        /// <returns>Rewuest object</returns>
        private static HttpWebRequest GetReq(string URL)
        {
            var Req = WebRequest.CreateHttp(URL);
            Req.UserAgent = UserAgent;
            return Req;
        }

        /// <summary>
        /// Resets the update status to "unchecked"
        /// </summary>
        public static void Reset()
        {
            Log.Write("Resetting Update Status");
            UpdateData = new UpdateInfo();
            InfoReady = false;
        }

        /// <summary>
        /// Downloads update info
        /// </summary>
        /// <returns>download success status</returns>
        public static bool ObtainUpdateInfo()
        {
            if (!InfoReady)
            {
                var Req = GetReq(UPDATE_URL);

                HttpWebResponse Res;
                try
                {
                    Res = (HttpWebResponse)Req.GetResponse();
                }
                catch (Exception ex)
                {
                    Log.Write(new Exception("Unable to make update request", ex));
                    //Unable to make request at this time
                    return false;
                }
                using (Res)
                {
                    using (var SR = new StreamReader(Res.GetResponseStream()))
                    {
                        try
                        {
                            //Make sure the version string is always parsed with 4 segments (W.X.Y.Z)
                            var V = string.Join(".", (SR.ReadLine() + ".0.0.0")
                                .Substring(1)
                                .Split('.')
                                .Take(4));
                            UpdateData = new UpdateInfo()
                            {
                                NewVersion = new Version(V),
                                DownloadURL = new Uri(SR.ReadLine())
                            };
                            InfoReady = true;
                            return true;
                        }
                        catch (Exception ex)
                        {
                            Log.Write(new Exception("Unable to parse update status response", ex));
                            //Invalid return values
                            return false;
                        }
                    }
                }
            }
            Log.Write("ObtainUpdateInfo already performed");
            return true;
        }

        /// <summary>
        /// Gets if an update is available
        /// </summary>
        /// <param name="V">Version to check against. Null uses current application version</param>
        /// <returns>true, if update available</returns>
        public static bool HasUpdate(Version V = null)
        {
            if (!ObtainUpdateInfo())
            {
                Log.Write("HasUpdate failed: ObtainUpdateInfo not successful");
                return false;
            }
            if (V == null)
            {
                return HasUpdate(Tools.CurrentVersion);
            }
            Log.Write("Checking Update for {0}", V);
            //Our version must be older
            return V.CompareTo(UpdateData.NewVersion) < 0;
        }

        /// <summary>
        /// Downloads an update to the given file
        /// </summary>
        /// <param name="FileName">File name, if null, uses executable path and name with _update suffix</param>
        /// <returns>download success status</returns>
        public static bool DownloadUpdate(string FileName = null)
        {
            if (!ObtainUpdateInfo())
            {
                Log.Write("DownloadUpdate failed: ObtainUpdateInfo not successful");
                return false;
            }
            //Generate file name
            if (string.IsNullOrEmpty(FileName))
            {
                return DownloadUpdate(DefaultUpdateExecutable);

            }
            FileStream FS = null;
            try
            {
                FS = File.Create(FileName);
            }
            catch (Exception ex)
            {
                Log.Write(new Exception($"Unable to write new executable to {FileName}", ex));
                return false;
            }
            using (FS)
            {
                //Don't bother the API in debug mode
                if (Program.DEBUG)
                {
                    Log.Write("Faking download in Debug mode by copying current executable instead.");
                    using (var IN = File.OpenRead(CurrentExecutable))
                    {
                        IN.CopyTo(FS);
                        return true;
                    }
                }
                var Req = GetReq(UpdateData.DownloadURL.ToString());

                HttpWebResponse Res;
                try
                {
                    Res = (HttpWebResponse)Req.GetResponse();
                }
                catch (Exception ex)
                {
                    Log.Write(new Exception("Unable to make executable download request", ex));
                    //Unable to make request at this time
                    return false;
                }
                using (Res)
                {
                    using (var S = Res.GetResponseStream())
                    {
                        S.CopyTo(FS);
                    }
                }
                Log.Write("Update ready at {0}", FileName);
                return true;
            }
        }

        /// <summary>
        /// Performs a live update of this executable.
        /// This is usually invoked upon user command
        /// </summary>
        /// <param name="FileName">File name of existing update file</param>
        /// <returns>true, if update in progress</returns>
        public static bool PerformUpdate(string FileName = null)
        {
            //Generate file name
            if (string.IsNullOrEmpty(FileName))
            {
                return PerformUpdate(DefaultUpdateExecutable);
            }
            if (File.Exists(FileName))
            {
                try
                {
                    Process.Start(FileName, $"update 1 \"{CurrentExecutable}\"");
                    Environment.Exit(0);
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Write(new Exception("Unable to launch phase 1 of update", ex));
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to update this application
        /// </summary>
        /// <returns>true, if update successful, false on failure or if no update needed</returns>
        /// <remarks>Run this as soon as possible</remarks>
        public static bool Update()
        {
            var Args = Environment.GetCommandLineArgs();
            //No update arguments
            if (Args.Length < 4 || Args[1] != "update" || !File.Exists(Args[3]))
            {
                Log.Write("Auto Update: No automatic update arguments found.");
                return false;
            }
            //Args:
            //0: Current file name
            //1: "update"
            //2: update step (1 or 2)
            //3: File name of target
            switch (Args[2])
            {
                case "1":
                    //Step 1 is performed by the temporary executable:
                    //1. copy itself (new version) over the old file name
                    //2. Run the old file name (now new file) with step 2
                    //3. Exit
                    try
                    {
                        if (WaitAndCopy(Args[0], Args[3], 5000))
                        {
                            //Start step 2
                            Process.Start(Args[3], $"update 2 \"{Args[0]}\"");
                            //C# doesn't "knows" that above line will exit
                            return true;
                        }
                        throw new IOException($"Timeout when trying to copy \"{Args[0]}\" to \"{Args[3]}\"");
                    }
                    catch (Exception ex)
                    {
                        Log.Write(new Exception("Unable to launch phase 2 of update", ex));
                    }
                    break;
                case "2":
                    //Step 2 is performed by the new version
                    //1. Delete temporary update file
                    //2. Restart application without arguments
                    if (WaitAndDel(Args[3], 5000))
                    {
                        //Restart without any command line arguments
                        Process.Start(Args[0]);
                        return true;
                    }
                    Log.Write("Update Phase 2: Automatic update did not complete in time: Unable to delete {0}", Args[3]);
                    break;
                default:
                    Log.Write("Invalid automatic update arguments.");
                    break;
            }
            return false;
        }

        /// <summary>
        /// Blocks until the given file could be deleted
        /// </summary>
        /// <param name="FileName">File to delete</param>
        /// <param name="Timeout">Maximum wait time in milliseconds. -1 for infinite</param>
        private static bool WaitAndDel(string FileName, int Timeout)
        {
            try
            {
                var FI = new FileInfo(FileName);
                FI.Attributes &= ~FileAttributes.ReadOnly;
            }
            catch (Exception ex)
            {
                Log.Write(new Exception("Unable to remove readonly attribute", ex));
                //Unable to remove readonly attribute.
                //Try deleting anyways
            }
            int TimeConsumed = 0;
            while (File.Exists(FileName) && (Timeout == -1 || TimeConsumed < Timeout))
            {
                try
                {
                    File.Delete(FileName);
                }
                catch
                {
                    Thread.Sleep(100);
                    TimeConsumed += 100;
                }
            }
            return Timeout == -1 || TimeConsumed < Timeout;
        }

        /// <summary>
        /// Blocks until a file could be copied
        /// </summary>
        /// <param name="Src">Source file</param>
        /// <param name="Dst">Destination file</param>
        /// <param name="Timeout">Maximum wait time in milliseconds. -1 for infinite</param>
        /// <returns></returns>
        private static bool WaitAndCopy(string Src, string Dst, int Timeout)
        {
            int TimeConsumed = 0;
            bool Cont = true;
            while (Cont && (Timeout == -1 || TimeConsumed < Timeout))
            {
                try
                {
                    File.Copy(Src, Dst, true);
                    Cont = false;
                }
                catch
                {
                    Thread.Sleep(100);
                    TimeConsumed += 100;
                }
            }
            return Timeout == -1 || TimeConsumed < Timeout;
        }
    }
}
