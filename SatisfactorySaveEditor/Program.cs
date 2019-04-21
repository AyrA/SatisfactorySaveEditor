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

                    //Important, change the session name to not fuck up your current game
                    H.SessionName = "EDITED_GAME";
                    H.Properties["sessionName"] = "EDITED_GAME";

                    //This is how you can read a specific entry and dump the hex contents:
                    //var Pod = H.Entries.First(m => m.ObjectData.Name == "/Game/FactoryGame/World/Benefit/DropPod/BP_DropPod.BP_DropPod_C");
                    //Console.Error.WriteLine(Tools.HexDump(Pod.Properties));

                    //These seem to be inventories across all items
                    var Container = H.Entries.Where(m => m.ObjectData.Name == "/Script/FactoryGame.FGInventoryComponent").First();
                    Console.Error.WriteLine(Tools.HexDump(Container.Properties, 24));

                    //Example save file editing using SaveFileHelper
                    //Console.Error.WriteLine("Processed {0} Entries", SaveFileHelper.RestoreDropPods(H));

                    /* This will list all types from the save file
                    Console.Error.WriteLine("Count\tType");
                    foreach (var E in H.Entries.OrderBy(m => m.ObjectData.Name).GroupBy(m => m.ObjectData.Name))
                    {
                        Console.Error.WriteLine("{1}\t{0}", E.Key, E.Count());
                    }
                    //*/

                    /* This lists all strings in the string list.
                    //The string list likely contains all entities removed from the map.
                    foreach (var E in H.StringList.Select(m => m.Value).Distinct())
                    {
                        Console.Error.WriteLine(E);
                    }
                    //*/

                    //* Writing back new save file
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
