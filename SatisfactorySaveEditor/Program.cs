using System;
using System.IO;
using System.Linq;
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

                    //Try some container black magic


                    //These seem to be inventories across all items
                    //var Inventory = H.Entries.Where(m => m.ObjectData.Name == "/Script/FactoryGame.FGInventoryComponent").Skip(20).First();
                    //Console.Error.WriteLine(Tools.HexDump(Inventory.Properties, 24));
                    //File.WriteAllBytes(@"C:\Users\AyrA\Desktop\Inventory.bin", Inventory.Properties);

                    //Example save file editing using SaveFileHelper
                    //Console.Error.WriteLine("Processed {0} Entries", SaveFileHelper.RestoreDropPods(H));

                    //* This will list all types from the save file
                    Console.Error.WriteLine("Count\tType");

                    var Containers = H.Entries.Where(m => m.ObjectData.Name== "/Game/FactoryGame/Buildable/Factory/StorageContainerMk1/Build_StorageContainerMk1.Build_StorageContainerMk1_C").ToArray();

                    var LastContainer = Containers[0];
                    var SecondLastContainer = Containers[0];

                    int LastId = int.Parse(LastContainer.ObjectData.InternalName.Split('_').Last());
                    int SecondLastId= int.Parse(SecondLastContainer.ObjectData.InternalName.Split('_').Last());

                    foreach(var Container in Containers)
                    {
                        var Id = int.Parse(Container.ObjectData.InternalName.Split('_').Last());
                        if (Id > LastId)
                        {
                            SecondLastContainer = LastContainer;
                            LastContainer = Container;

                            SecondLastId = LastId;
                            LastId = Id;
                        }
                        else if(Id>SecondLastId)
                        {
                            SecondLastContainer = Container;
                            SecondLastId = Id;
                        }
                    }


                    Console.Error.WriteLine(Tools.HexDump(LastContainer.Properties, 24));
                    Console.Error.WriteLine(Tools.HexDump(SecondLastContainer.Properties, 24));

                    //Mess everything up
                    SecondLastContainer.Properties = LastContainer.Properties;

                    /*
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
