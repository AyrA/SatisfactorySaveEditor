using System;
using System.Linq;

namespace SatisfactorySaveEditor
{
    public static class SaveFileHelper
    {
        public static int RemoveLizardDoggos(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Character/Creature/Wildlife/SpaceRabbit/Char_SpaceRabbit.Char_SpaceRabbit_C"
            };
            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        public static int RestoreRocks(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Equipment/C4Dispenser/BP_DestructibleLargeRock.BP_DestructibleLargeRock_C",
                "/Game/FactoryGame/Equipment/C4Dispenser/BP_DestructibleSmallRock.BP_DestructibleSmallRock_C"
            };
            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        public static int RemoveRocks(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Equipment/C4Dispenser/BP_DestructibleLargeRock.BP_DestructibleLargeRock_C",
                "/Game/FactoryGame/Equipment/C4Dispenser/BP_DestructibleSmallRock.BP_DestructibleSmallRock_C"
            };
            var Entries = F.Entries.Where(m => ItemList.Contains(m.ObjectData.Name)).ToArray();
            
            //Something probably has to be done to the array here but I am not sure what
            //Default: 00 00 00 00 00 00 00 00 00 00 00 00 05 00 00 00 4E 6F 6E 65 00 00 00 00 00

            //Try adding to string list
            foreach (var E in Entries)
            {
                F.StringList.Add(new PropertyString(E.ObjectData.LevelType, E.ObjectData.Name));
            }
            //Remove from item list
            return F.Entries.RemoveAll(m => Entries.Contains(m));
        }

        public static int RestorePlants(SaveFile F)
        {
            var ItemList = new string[] {
                "/Script/FactoryGame.FGFoliageRemoval"
            };
            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        public static int RestorePickups(SaveFile F)
        {
            var ItemList = new string[] {
                "/Script/FactoryGame.FGItemPickup_Spawnable",
                "/Game/FactoryGame/Resource/BP_ItemPickup_Spawnable.BP_ItemPickup_Spawnable_C"
            };

            var StringList = new string[] {
                ":PersistentLevel.FGItemPickup_Spawnable_"
            };

            F.StringList.RemoveAll(m => StringList.Any(n => m.Value.Contains(n)));

            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        public static int RestoreBerries(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/World/Benefit/NutBush/BP_NutBush.BP_NutBush_C",
                "/Game/FactoryGame/World/Benefit/Mushroom/BP_Shroom_01.BP_Shroom_01_C",
                "/Game/FactoryGame/World/Benefit/BerryBush/BP_BerryBush.BP_BerryBush_C"
            };

            var StringList = new string[] {
                ":PersistentLevel.BP_Shroom_",
                ":PersistentLevel.BP_BerryBush",
                ":PersistentLevel.BP_NutBush"
            };

            F.StringList.RemoveAll(m => StringList.Any(n => m.Value.Contains(n)));
            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        public static int RestoreDropPods(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/World/Benefit/DropPod/BP_DropPod.BP_DropPod_C"
            };


            //String list?

            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        public static int RestoreArtifacts(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Prototype/WAT/BP_WAT1.BP_WAT1_C",
                "/Game/FactoryGame/Prototype/WAT/BP_WAT2.BP_WAT2_C",
            };

            var StringList = new string[] {
                ":PersistentLevel.BP_WAT1",
                ":PersistentLevel.BP_WAT2"
            };

            F.StringList.RemoveAll(m => StringList.Any(n => m.Value.Contains(n)));
            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        public static int RemoveAnimalParts(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Resource/Environment/AnimalParts/BP_HogParts.BP_HogParts_C",
                "/Game/FactoryGame/Resource/Environment/AnimalParts/BP_CrabEggParts.BP_CrabEggParts_C",
                "/Game/FactoryGame/Resource/Environment/AnimalParts/BP_SpitterParts.BP_SpitterParts_C"
            };

            //String list?

            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        public static int RestoreSlugs(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Resource/Environment/Crystal/BP_Crystal.BP_Crystal_C",
                "/Game/FactoryGame/Resource/Environment/Crystal/BP_Crystal_mk2.BP_Crystal_mk2_C",
                "/Game/FactoryGame/Resource/Environment/Crystal/BP_Crystal_mk3.BP_Crystal_mk3_C"
            };

            var StringList = new string[] {
                ":PersistentLevel.BP_Crystal"
            };

            F.StringList.RemoveAll(m => StringList.Any(n => m.Value.Contains(n)));
            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        public static int RemoveNiceCreatures(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Character/Creature/Wildlife/SpaceGiraffe/Char_SpaceGiraffe.Char_SpaceGiraffe_C",
                "/Game/FactoryGame/Character/Creature/Wildlife/NonFlyingBird/Char_NonFlyingBird.Char_NonFlyingBird_C"
            };

            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        public static int RemoveEvilCreatures(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Character/Creature/BP_CreatureSpawner.BP_CreatureSpawner_C",
                "/Game/FactoryGame/Character/Creature/Enemy/Stinger/SmallStinger/Char_CaveStinger_Child.Char_CaveStinger_Child_C",
                "/Game/FactoryGame/Character/Creature/Enemy/Spitter/SmallSpitter/Char_Spitter_Small.Char_Spitter_Small_C",
                "/Game/FactoryGame/Character/Creature/Enemy/Hog/Char_Hog.Char_Hog_C"
            };

            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        public static int RemoveCreatureSpawner(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Character/Creature/BP_CreatureSpawner.BP_CreatureSpawner_C"
            };

            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        public static int RestoreMap(SaveFile F, bool ReplantPlants)
        {
            var ItemList = new string[] {
                //Unknown items
                "/Game/FactoryGame/Prototype/WAT/BP_WAT1.BP_WAT1_C",
                "/Game/FactoryGame/Prototype/WAT/BP_WAT2.BP_WAT2_C"
            };
            F.StringList.Clear();

            int Count = 0;
            if (ReplantPlants)
            {
                Count += RestorePlants(F);
            }
            return RestorePickups(F) +
                RestoreBerries(F) +
                RestoreArtifacts(F) +
                RemoveAnimalParts(F) +
                F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

    }
}
