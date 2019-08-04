//Use console only mode
//#define NOFORM
using System;
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
        }

        [STAThread]
        static int Main(string[] args)
        {
            ErrorHandler ReleaseModeErrorHandler;
            Log.Write("Application version {0} start", Tools.CurrentVersion);
            //Set "NOFORM" to better experiment
#if !NOFORM
            if (!DEBUG)
            {
                ReleaseModeErrorHandler = new ErrorHandler();
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
            }
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
                //Remove console handle to not block any scripts.
                Tools.FreeConsole();
                QuickPlay.CheckQuickPlay();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain(args.FirstOrDefault()));
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
            Console.Error.WriteLine("#END");
            return Exit(RET.SUCCESS);
#endif
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
            Console.Error.WriteLine("SatisfactorySaveEditor.exe [SaveFile]");
        }
    }
}
