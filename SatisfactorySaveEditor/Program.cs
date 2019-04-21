using System;
using System.IO;
using System.Linq;

namespace SatisfactorySaveEditor
{
    class Program
    {
        const string SAVEDIR = @"%LOCALAPPDATA%\FactoryGame\Saved\SaveGames";

        public struct RET
        {
            public const int SUCCESS = 0;
            public const int ARG = 1;
        }

        static int Main(string[] args)
        {
#if DEBUG
            args = new string[] {
                @"C:\Users\AyrA\AppData\Local\FactoryGame\Saved\SaveGames\YAY.sav"
            };
#endif
            string FileName = args.FirstOrDefault();
            if (string.IsNullOrEmpty(FileName) || !File.Exists(FileName))
            {
                Help();
                Console.Error.WriteLine("File list:");
                Console.Error.WriteLine(
                    string.Join("\r\n", Directory.GetFiles(Environment.ExpandEnvironmentVariables(SAVEDIR), "*.sav"))
                    );
                return Exit(RET.ARG);
            }

            using (var FS = File.OpenRead(FileName))
            {
                using (var BR = new BinaryReader(FS))
                {
                    var H = new SaveFile(BR);
                    Console.Error.WriteLine(H.PlayTime);
                    Console.Error.WriteLine(H.LevelType);
                    H.SessionName = "EDITED_GAME";
                    H.Properties["sessionName"] = "EDITED_GAME";
                    H.StringList.Clear();
                    var NewFile = Path.Combine(Environment.ExpandEnvironmentVariables(SAVEDIR), "EDITED_GAME.SAV");
                    using (var FSW = File.Create(NewFile))
                    {
                        H.Export(FSW);
                    }
                }
            }

            return Exit(RET.SUCCESS);
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
