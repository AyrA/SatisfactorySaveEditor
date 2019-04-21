using System.IO;

namespace SatisfactorySaveEditor.ObjectTypes
{
    /// <summary>
    /// Serves as the base for object entries
    /// </summary>
    public class GameBaseObject
    {
        /// <summary>
        /// Reads the base values present in every object
        /// </summary>
        /// <param name="BR">Open Reader</param>
        internal void Fill(BinaryReader BR)
        {
            Name = BR.ReadIntString();
            LevelType = BR.ReadIntString();
            InternalName = BR.ReadIntString();
        }

        /// <summary>
        /// Type of object. See <see cref="OBJECT_TYPE"/>
        /// </summary>
        /// <remarks>Maybe an enum would be better</remarks>
        public int ObjectType { get; set; }
        /// <summary>
        /// Some Path-like internal name, probably path inside the game data file
        /// </summary>
        public string InternalName { get; set; }
        /// <summary>
        /// Level type entry. Almost always "Persistent_Level"
        /// </summary>
        public string LevelType { get; set; }
        /// <summary>
        /// Property Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Exports the base data to a stream
        /// </summary>
        /// <param name="BW">Open Writer</param>
        /// <remarks>Do not call directly. Call on the child class</remarks>
        public virtual void Export(BinaryWriter BW)
        {
            BW.WriteIntString(Name);
            BW.WriteIntString(LevelType);
            BW.WriteIntString(InternalName);
        }
    }
}
