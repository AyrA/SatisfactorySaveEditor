using System.Collections.Generic;
using System.Linq;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Provides simplified save file operation
    /// </summary>
    public static class SaveFileHelper
    {
        /// <summary>
        /// Remove all lizard doggos
        /// </summary>
        /// <remarks>Includes tamed ones</remarks>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
        public static int RemoveLizardDoggos(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Character/Creature/Wildlife/SpaceRabbit/Char_SpaceRabbit.Char_SpaceRabbit_C"
            };
            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        /// <summary>
        /// Restores all rocks on the map
        /// </summary>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
        public static int RestoreRocks(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Equipment/C4Dispenser/BP_DestructibleLargeRock.BP_DestructibleLargeRock_C",
                "/Game/FactoryGame/Equipment/C4Dispenser/BP_DestructibleSmallRock.BP_DestructibleSmallRock_C"
            };
            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        /// <summary>
        /// Removes all rocks from the map
        /// </summary>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
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

        /// <summary>
        /// Restores all removed plants
        /// </summary>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
        public static int RestorePlants(SaveFile F)
        {
            var ItemList = new string[] {
                "/Script/FactoryGame.FGFoliageRemoval"
            };
            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        /// <summary>
        /// Restores all pickup items and removes custom pickup items
        /// </summary>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
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

        /// <summary>
        /// Restores all berry bushes, nuts and mushrooms
        /// </summary>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
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

        /// <summary>
        /// Restores all drop pods into the unopened state
        /// </summary>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
        public static int RestoreDropPods(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/World/Benefit/DropPod/BP_DropPod.BP_DropPod_C"
            };


            //String list?

            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        /// <summary>
        /// Restores all artifacts
        /// </summary>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
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

        /// <summary>
        /// Removes all stray animal parts the player forgot to pick up
        /// </summary>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
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

        /// <summary>
        /// Restores all power slugs
        /// </summary>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
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

        /// <summary>
        /// Removes all nice/passive creatures
        /// </summary>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
        public static int RemoveNiceCreatures(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Character/Creature/Wildlife/SpaceGiraffe/Char_SpaceGiraffe.Char_SpaceGiraffe_C",
                "/Game/FactoryGame/Character/Creature/Wildlife/NonFlyingBird/Char_NonFlyingBird.Char_NonFlyingBird_C"
            };

            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        /// <summary>
        /// Removes all evil creatures
        /// </summary>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
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

        /// <summary>
        /// Removes all spawners
        /// </summary>
        /// <param name="F">Save file</param>
        /// <returns>Number of items processed</returns>
        public static int RemoveCreatureSpawner(SaveFile F)
        {
            var ItemList = new string[] {
                "/Game/FactoryGame/Character/Creature/BP_CreatureSpawner.BP_CreatureSpawner_C"
            };

            return F.Entries.RemoveAll(m => ItemList.Contains(m.ObjectData.Name));
        }

        /// <summary>
        /// Restores the map into a mostly "untouched" state,
        /// ignoring player built objects
        /// </summary>
        /// <param name="F">Save file</param>
        /// <param name="ReplantPlants">Add all collected plants back to the map</param>
        /// <returns>Number of items processed</returns>
        public static int RestoreMap(SaveFile F, bool ReplantPlants)
        {
            var ItemList = new string[] {
                //Artifacts
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
                RestoreSlugs(F);
        }

        /// <summary>
        /// Resizes all given objects
        /// </summary>
        /// <param name="Entries">Enumeration of objects</param>
        /// <param name="factor">
        /// Factor to scale to.
        /// The default factor of objects is 1.0
        /// </param>
        /// <param name="Offset">
        /// Offset to apply to position.
        /// Use 0 to not move. Positive numbers move the object up.
        /// Generally you want to do this when inflating by large factors (10 and more)
        /// </param>
        /// <remarks>
        /// This function filters script types.
        /// The factor is always from base size.
        /// Not all entities are resizable.
        /// </remarks>
        /// <returns>Number of items resized</returns>
        public static int ItemResizer(IEnumerable<SaveFileEntry> Entries, float factor = 1.0f, int Offset = 0)
        {
            int Ret = 0;
            foreach (var E in Entries.Where(m => m.EntryType == ObjectTypes.OBJECT_TYPE.OBJECT))
            {
                var OD = (ObjectTypes.GameObject)E.ObjectData;
                OD.ObjectScale.X = factor;
                OD.ObjectScale.Y = factor;
                OD.ObjectScale.Z = factor;
                if (Offset != 0)
                {
                    OD.ObjectPosition.Z += Offset;
                }
                ++Ret;
            }
            return Ret;
        }

        /// <summary>
        /// Gets the shortest possible data entry for an item, the "None" entry
        /// </summary>
        /// <returns>"None" entry</returns>
        public static byte[] GetNullData()
        {
            return new byte[] {
                0x05, 0x00, 0x00, 0x00,
                0x4E, 0x6F, 0x6E, 0x65,
                0x00, 0x00, 0x00, 0x00,
                0x00
            };
        }
    }
}
