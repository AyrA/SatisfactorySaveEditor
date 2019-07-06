//#define NOFORM
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    class Program
    {
        public const string SAVEDIR = @"%LOCALAPPDATA%\FactoryGame\Saved\SaveGames";

        public static string SaveDirectory
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(SAVEDIR);
            }
        }

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
            //Remove console handle to not block any scripts.
            Tools.FreeConsole();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(args.FirstOrDefault()));
            return RET.SUCCESS;
#else
            //Allocate console or the Console.ReadKey() will crash
            Tools.AllocConsole();
            Test();
            return Exit(RET.SUCCESS);
#endif
        }

        private static void Test()
        {
            //Note to testers: Do not close the stream early. This protects you from overwriting the save file.
            using (var FS = File.OpenRead(Path.Combine(Program.SaveDirectory, "Test.sav")))
            {
                using (var BR = new BinaryReader(FS))
                {
                    var F = new SaveFile(BR);

                    //Show all entries
                    Console.Error.WriteLine(string.Join("\r\n", F.Entries.OrderBy(m => m.Properties.Length).Select(m => m.ObjectData.Name).Distinct()));

                    var E = F.Entries.FirstOrDefault(m => m.ObjectData.Name == "/Game/FactoryGame/Resource/BP_ResourceNode.BP_ResourceNode_C");
                    Console.Error.WriteLine(Tools.HexDump(E.Properties));

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
                    BaseObject.ObjectPosition.Z = 18000;

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
                    using (var FSOut = File.Create(Path.Combine(Environment.ExpandEnvironmentVariables(SAVEDIR), "Test-Edited.sav")))
                    {
                        F.Export(FSOut);
                    }
                    //*/
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
            //TODO: Help
        }
    }
}
