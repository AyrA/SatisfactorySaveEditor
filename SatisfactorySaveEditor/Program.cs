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
                    H.SessionName = "EDITED_GAME";
                    H.Properties["sessionName"] = "EDITED_GAME";

                    var Rock = H.Entries.First(m => m.ObjectData.Name == "/Game/FactoryGame/Equipment/C4Dispenser/BP_DestructibleLargeRock.BP_DestructibleLargeRock_C");
                    Console.Error.WriteLine(Tools.HexDump(Rock.Properties));

                    Console.Error.WriteLine("Removed {0} Rocks", SaveFileHelper.RemoveRocks(H));

                    /*
                    Console.Error.WriteLine("Count\tType");
                    foreach (var E in H.Entries.GroupBy(m => m.ObjectData.Name))
                    {
                        Console.Error.WriteLine("{1}\t{0}", E.Key, E.Count());
                    }
                    //*/

                    /*
                    foreach (var E in H.StringList.Select(m => m.Value).Distinct())
                    {
                        Console.Error.WriteLine(E);
                    }
                    //*/

                    //*
                    var NewFile = Path.Combine(Environment.ExpandEnvironmentVariables(SAVEDIR), "EDITED_GAME.SAV");
                    using (var FSW = File.Create(NewFile))
                    {
                        H.Export(FSW);
                    }
                    //*/
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
