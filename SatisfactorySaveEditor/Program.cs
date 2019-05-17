using System;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    class Program
    {
        public const string SAVEDIR = @"%LOCALAPPDATA%\FactoryGame\Saved\SaveGames";

        public struct RET
        {
            public const int SUCCESS = 0;
            public const int ARG = 1;
        }

        [STAThread]
        static int Main(string[] args)
        {
            //Set "NOFORM" to better experiment
#if !NOFORM
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            return RET.SUCCESS;
#else
            using (var FS = File.OpenRead(Path.Combine(Environment.ExpandEnvironmentVariables(SAVEDIR), "Vanilla.sav")))
            {
                using (var BR = new BinaryReader(FS))
                {
                    var F = new SaveFile(BR);
                    Console.Error.WriteLine(string.Join("\r\n", F.Entries.Select(m => m.ObjectData.Name).Distinct()));
                    //Do your tests on the file here
                    using (var FSOut = File.Create(Path.Combine(Environment.ExpandEnvironmentVariables(SAVEDIR), "Test.sav")))
                    {
                        F.Export(FSOut);
                    }
                }
            }
            return Exit(RET.SUCCESS);
#endif
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
            //TODO: Help
        }
    }
}
