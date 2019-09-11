//Use console only mode
//#define NOFORM
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    class Program
    {
#if DEBUG
        /// <summary>
        /// Allows debug mode check without using #DEBUG
        /// </summary>
        public const bool DEBUG = true;
#else
        /// <summary>
        /// Allows debug mode check without using #DEBUG
        /// </summary>
        public const bool DEBUG = false;
#endif
        /// <summary>
        /// Default game save directory
        /// </summary>
        /// <remarks>Environment variables are permitted</remarks>
        public const string SAVEDIR = @"%LOCALAPPDATA%\FactoryGame\Saved\SaveGames";

        /// <summary>
        /// Gets the full path to the save file directory
        /// </summary>
        public static string SaveDirectory
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(SAVEDIR);
            }
        }

        /// <summary>
        /// Possible return values
        /// </summary>
        public struct RET
        {
            /// <summary>
            /// Successful run
            /// </summary>
            public const int SUCCESS = 0;
            /// <summary>
            /// Argument problem
            /// </summary>
            public const int ARG = 1;
            /// <summary>
            /// Command to open the User Interface
            /// </summary>
            public const int UI = 2;
            /// <summary>
            /// Help Request
            /// </summary>
            public const int HELP = 3;
            /// <summary>
            /// A File I/O error occured (bad disk, not found, in use, etc)
            /// </summary>
            public const int IO = 4;
        }

        private enum Modes
        {
            Verify,
            List,
            Pack,
            RenameSession,
            Render,
            Info
        }

        [STAThread]
        static int Main(string[] args)
        {
            Log.Write("Application version {0} start", Tools.CurrentVersion);
            FeatureReport.Id = Guid.Empty;
            //Set "NOFORM" to better experiment
#if !NOFORM
#if !DEBUG
            var ReleaseModeErrorHandler = new ErrorHandler();
            ReleaseModeErrorHandler.ErrorReport += delegate (Exception ex, ErrorHandlerEventArgs e)
                {
                    Log.Write("Sending error report");
                    var Msg = ErrorHandler.generateReport(ex);
                    var MainForm = Tools.GetForm<frmMain>();
                    byte[] CurrentFile = null;
                    if (MainForm != null && MainForm.HasFileOpen)
                    {
                        var F = MainForm.CurrentFile;
                        using (var MS = new MemoryStream())
                        {
                            F.Export(MS);
                            MS.Position = 0;
                            CurrentFile = Compression.CompressData(MS.ToArray());
                        }
                    }
                //Send report
                e.Handled = ReleaseModeErrorHandler.UploadReport(Msg, CurrentFile == null ? new byte[0] : CurrentFile);
                };
#endif
            //Perform update automatically if it's pending
            if (!DEBUG && args.Length == 0 && File.Exists(UpdateHandler.DefaultUpdateExecutable))
            {
                if (UpdateHandler.PerformUpdate())
                {
                    Log.Write("Application version {0} end (Update success)", Tools.CurrentVersion);
                    Log.Close();
                    return RET.SUCCESS;
                }
            }
            //Only run main application part if no update was performed
            if (DEBUG || !UpdateHandler.Update())
            {
#if DEBUG
                FeatureReport.Used(FeatureReport.Feature.DebugMode);
#endif
                int Status = HandleArguments(args);
                if (Status == RET.UI)
                {
                    //Remove console handle to not block any scripts.
                    Tools.FreeConsole();
                    QuickPlay.CheckQuickPlay();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmMain(args.FirstOrDefault()));
                }
                else if (DEBUG)
                {
                    return Exit(RET.SUCCESS);
                }
                //Send Report
                FeatureReport.Report();
            }
            Log.Write("Application version {0} end", Tools.CurrentVersion);
            Log.Close();
            return RET.SUCCESS;
#else
            Log.Write("Running in test mode");
            //Allocate console or the Console.ReadKey() will crash
            Tools.AllocConsole();

            Console.Error.WriteLine("Arguments: {0}", string.Join("\r\n", args));

            Test();
            return Exit(RET.SUCCESS);
#endif
        }

        private static void TestHTTP()
        {
            using (var L = new SMRAPI.HTTP(Tools.GetRandom(5000, 50000)))
            {
                L.StaticResources.Add("/favicon.ico", new SMRAPI.ResponseData(Tools.GetResource("SatisfactorySaveEditor.Images.Icons.edit.ico")));
                L.ApiKeyEvent += delegate (object sender, Guid key)
                {
                    Tools.E(key.ToString(), "HTTP event");
                    L.Stop();
                };
                L.Start();
                L.OpenBrowser();
                L.WaitForExit();
            }
        }

        private static int HandleArguments(string[] args)
        {
            var Ops = new List<Modes>();
            string Filename = null;
            string SessionName = null;
            if (args.Contains("/?"))
            {
                Help();
                return RET.HELP;
            }
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                Log.Write("{0}: processing argument: {1}", nameof(HandleArguments), arg);
                switch (arg.ToLower())
                {
                    case "/verify":
                        Ops.Add(Modes.Verify);
                        break;
                    case "/list":
                        Ops.Add(Modes.List);
                        break;
                    case "/pack":
                        Ops.Add(Modes.Pack);
                        break;
                    case "/render":
                        Ops.Add(Modes.Render);
                        break;
                    case "/info":
                        Ops.Add(Modes.Info);
                        break;
                    case "/rename":
                        Ops.Add(Modes.RenameSession);
                        if (i < args.Length - 1)
                        {
                            SessionName = args[++i];
                        }
                        else
                        {
                            Log.Write("{0}: /rename requires a new session name", nameof(HandleArguments));
                            Console.WriteLine("/rename requires a new session name");
                            return RET.ARG;
                        }
                        break;
                    default:
                        if (string.IsNullOrEmpty(Filename))
                        {
                            Filename = arg;
                        }
                        else
                        {
                            Log.Write("{0}: unknown argument: {1}", nameof(HandleArguments), arg);
                            Console.WriteLine("Unknown argument: {0}", arg);
                            return RET.ARG;
                        }
                        break;
                }
            }

            if (Ops.Count == 0 && args.Length < 2)
            {
                return RET.UI;
            }

            Tools.AllocConsole();

            if (string.IsNullOrEmpty(Filename))
            {
                Log.Write("{0}: No file name argument supplied", nameof(HandleArguments));
                Console.WriteLine("No file name argument supplied");
                return RET.ARG;
            }

            Ops = Ops.Distinct().ToList();

            var Changes = false;
            var IsGz = false;
            SaveFile SF = null;
            FileStream FS = null;
            try
            {
                FS = File.Open(Filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to open input file");
                Log.Write("{0}: Unable to open input file", nameof(HandleArguments));
                Log.Write(ex);
                return RET.IO;
            }
            using (FS)
            {
                IsGz = Tools.IsGzFile(FS);
                try
                {
                    SF = SaveFile.Open(FS);
                    if (SF == null)
                    {
                        throw new InvalidDataException("The file is not a valid satisfactory save game");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid game file");
                    Log.Write("{0}: Invalid game file", nameof(HandleArguments));
                    Log.Write(ex);
                    return RET.IO;
                }


                if (Ops.Contains(Modes.Verify))
                {
                    Log.Write("{0}: Verifying file", nameof(HandleArguments));
                    //Verify does nothing on its own
                    Check(SF.PlayTime.Ticks >= 0, "Positive play time");
                    Check(Tools.IsMatch(SF.SessionName, @"^[\w\.\- ]{1,31}$"), "Non-Problematic Session Name");
                    Check(SF.LevelType == SaveFile.DEFAULT_LEVEL_TYPE, $"Level type is '{SaveFile.DEFAULT_LEVEL_TYPE}'");
                    Check(SF.SaveDate.ToUniversalTime() < DateTime.UtcNow, "Date in past");
                    foreach (var key in "sessionName Visibility startloc".Split(' '))
                    {
                        Check(SF.Properties.ContainsKey(key), $"Header contains field '{key}'");
                    }
                    //Don't double error with the previous one
                    if (SF.Properties.ContainsKey("sessionName"))
                    {
                        Check(SF.Properties["sessionName"] == SF.SessionName, "Both session names match");
                    }
                    else
                    {
                        Console.WriteLine("[SKIP]: Both session names match");
                    }
                }

                if (Ops.Contains(Modes.List))
                {
                    Log.Write("{0}: Printing item list", nameof(HandleArguments));
                    foreach (var e in SF.Entries.GroupBy(m => m.ObjectData.Name).OrderBy(m => m.Key))
                    {
                        Console.WriteLine("{1}\t{0}", e.Key, e.Count());
                    }
                }

                if (Ops.Contains(Modes.Render))
                {
                    var ImgFile = Path.ChangeExtension(Filename, ".png");
                    Log.Write("{0}: Rendering map as original size to {1}", nameof(HandleArguments), ImgFile);
                    Console.WriteLine("Initializing image...");
                    MapRender.Init(-1, -1);
                    Console.WriteLine("Rendering file...");
                    using (var IMG = MapRender.RenderFile(SF, 8.192))
                    {
                        Console.WriteLine("Saving image...");
                        IMG.Save(ImgFile);
                    }
                }

                if (Ops.Contains(Modes.Info))
                {
                    Log.Write("{0}: Showing game info", nameof(HandleArguments));
                    Console.WriteLine("Save File Size:\t{0}", FS.Position);
                    Console.WriteLine("Build Version:\t{0}", SF.BuildVersion);
                    Console.WriteLine("Save Version:\t{0}", SF.SaveVersion);
                    Console.WriteLine("Header Version:\t{0}", SF.SaveHeaderVersion);
                    Console.WriteLine("Session Name:\t{0}", SF.SessionName);
                    Console.WriteLine("Save Date:\t{0}", SF.SaveDate);
                    Console.WriteLine("Play Time:\t{0}", SF.PlayTime);
                    foreach (var KV in SF.Properties)
                    {
                        Console.WriteLine("{0}:\t{1}", KV.Key, KV.Value);
                    }
                    Console.WriteLine("Object Count:\t{0}", SF.Entries.Count);
                    Console.WriteLine("String List:\t{0}", SF.StringList.Count);
                }

                if (Ops.Contains(Modes.RenameSession))
                {
                    Log.Write("{0}: Renaming session to {1}", nameof(HandleArguments), SessionName);
                    SF.SetSessionName(SessionName);
                    Changes = true;
                }

                if (Ops.Contains(Modes.Pack))
                {
                    string NewFile;
                    FileStream OUT;
                    FS.Seek(0, SeekOrigin.Begin);
                    if (IsGz)
                    {
                        if (Filename.ToLower().EndsWith(".sav.gz"))
                        {
                            NewFile = Path.ChangeExtension(Filename, null);
                        }
                        else
                        {
                            NewFile = Path.ChangeExtension(Filename, ".sav");
                        }
                    }
                    else
                    {
                        NewFile = Filename + ".gz";
                    }
                    try
                    {
                        OUT = File.Create(NewFile);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Can't create output file");
                        Log.Write("{0}: Can't create {1}", nameof(HandleArguments), NewFile);
                        Log.Write(ex);
                        return RET.IO;
                    }
                    Log.Write("{0}: {1} file", nameof(HandleArguments), IsGz ? "Compressing" : "Decompressing");
                    using (OUT)
                    {
                        if (IsGz)
                        {
                            Compression.DecompressStream(FS, OUT);
                        }
                        else
                        {
                            Compression.CompressStream(FS, OUT);
                        }
                    }
                    Log.Write("{0}: {1} file OK", nameof(HandleArguments), IsGz ? "Compressing" : "Decompressing");
                }
                if (Changes)
                {
                    Log.Write("{0}: Writing back changes", nameof(HandleArguments));
                    FS.Seek(0, SeekOrigin.Begin);
                    SF.Export(FS);
                    FS.Flush();
                    FS.SetLength(FS.Position);
                }
            }

            return RET.SUCCESS;
        }

        private static bool Check(bool Pass, string Message)
        {
            Console.ForegroundColor = Pass ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine("[{1}]: {0}", Message, Pass ? "PASS" : "FAIL");
            Console.ResetColor();
            return Pass;
        }

        private static void Test()
        {
            const string WHITELIST = "/Buildable/|ResourceNode|ResourceDeposit";
            const string PROTECTED = "MamIntegrated|HubTerminal|WorkBenchIntegrated|StorageIntegrated|GeneratorIntegratedBiomass";
            var WLItems = WHITELIST.Split('|');
            var ProtectedItems = PROTECTED.Split('|');
            var ZeroPos = new Vector3();

            //Note to testers: Do not close the stream early. This protects you from overwriting the save file.
            using (var FS = File.OpenRead(Path.Combine(SaveDirectory, "Reset.sav")))
            {
                var F = SaveFile.Open(FS);

                var R = new Random();
                foreach (var E in F.Entries)
                {
                    if (E.EntryType == ObjectTypes.OBJECT_TYPE.OBJECT)
                    {
                        var o = (ObjectTypes.GameObject)E.ObjectData;
                        if (
                            !o.ObjectPosition.Equals(ZeroPos) &&
                            WLItems.Any(n => o.Name.Contains(n)) &&
                            !ProtectedItems.Any(n => o.Name.Contains(n)))
                        {
                            var newPos = Tools.TranslateToMap(new System.Drawing.PointF((float)R.NextDouble(), (float)R.NextDouble()));
                            newPos.Z = 18000;
                            o.ObjectPosition = newPos;
                        }
                    }
                }

                //Show all entries
                Console.Error.WriteLine(string.Join("\r\n", F.Entries.OrderBy(m => m.Properties.Length).Select(m => m.ObjectData.Name).Distinct()));

                /* Test code to "align" things
                var o = (ObjectTypes.GameObject)e.ObjectData;
                o.ObjectPosition.X = (int)o.ObjectPosition.X;
                o.ObjectPosition.Y = (int)o.ObjectPosition.Y;
                o.ObjectPosition.Z = (int)o.ObjectPosition.Z;

                o.ObjectPosition.X -= o.ObjectPosition.X % 10;
                o.ObjectPosition.Y -= o.ObjectPosition.Y % 10;
                o.ObjectPosition.Z -= o.ObjectPosition.Z % 10;
                //o.ObjectRotation.W = o.ObjectRotation.X = o.ObjectRotation.Y = o.ObjectRotation.Z = 0;
                //Console.Error.WriteLine("{0} {1}", o.ObjectPosition, o.ObjectRotation);
                //*/

                /*
                //Duplicator test
                var BaseF = (SaveFileEntry)F.Entries.First(m => m.ObjectData.Name == "/Game/FactoryGame/Buildable/Building/Foundation/Build_Foundation_8x2_01.Build_Foundation_8x2_01_C").Clone();
                var BaseObject = (ObjectTypes.GameObject)BaseF.ObjectData;
                var BaseName = BaseObject.InternalName;
                BaseName = BaseName.Substring(0, BaseName.Length - 1);
                BaseObject.ObjectPosition.X = -324000;
                BaseObject.ObjectPosition.Y = -374000;
                BaseObject.ObjectPosition.Z = 24000;

                int ctr = F.Entries.Count(m => m.ObjectData.Name == "/Game/FactoryGame/Buildable/Building/Foundation/Build_Foundation_8x2_01.Build_Foundation_8x2_01_C");
                for (var x = 0; x < 940; x += 5)
                {
                    for (var y = 0; y < 940; y += 5)
                    {
                        var Copy = (SaveFileEntry)BaseF.Clone();
                        var o = (ObjectTypes.GameObject)Copy.ObjectData;
                        o.InternalName = BaseName + (ctr++).ToString();
                        o.ObjectPosition.X += x * 800;
                        o.ObjectPosition.Y += y * 800;
                        F.Entries.Add(Copy);
                    }
                    Console.Error.WriteLine(x);
                }
                Console.Error.WriteLine("Placed {0} foundations", ctr);
                //*/
                using (var FSOut = File.Create(Path.Combine(SaveDirectory, "Test-Edited.sav")))
                {
                    F.Export(FSOut);
                }
            }
        }

        private static int Exit(int ExitCode)
        {
#if DEBUG
            Console.Error.WriteLine("#END");
            Console.ReadKey(true);
#endif
            return ExitCode;
        }

        private static void Help()
        {
            Console.Error.WriteLine(@"SatisfactorySaveEditor.exe [/verify] [/list] [/pack] [/render] [/info] [/rename <new-name>] [SaveFile]
Satisfactory Save File Editor

/verify    - Reads the entire file and verifies basic constraints
/list      - Lists all entries and counts them
/pack      - Compresses file if uncompressed, or uncompresses if compressed.
/rename    - Renames the session
/render    - Renders a PNG image of the map into the same directory (replaces .sav with .png)
/info      - Print some basic information
SaveFile   - File to open. Required argument if switches are used");
        }
    }
}
