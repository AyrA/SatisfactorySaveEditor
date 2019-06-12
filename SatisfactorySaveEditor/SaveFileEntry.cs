using SatisfactorySaveEditor.ObjectTypes;
using System;
using System.IO;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Generic entry in the save file
    /// </summary>
    public class SaveFileEntry : ICloneable
    {
        /// <summary>
        /// Entry specific data
        /// </summary>
        public GameBaseObject ObjectData
        { get; set; }

        /// <summary>
        /// Type of entry
        /// </summary>
        /// <remarks>See <see cref="OBJECT_TYPE"/></remarks>
        public int EntryType
        { get; set; }

        /// <summary>
        /// Entry specific properties
        /// </summary>
        public byte[] Properties
        { get; set; }

        /// <summary>
        /// Reads a new entry from the stream
        /// </summary>
        /// <param name="BR">Open Reader</param>
        public SaveFileEntry(BinaryReader BR)
        {
            EntryType = BR.ReadInt32();
            switch (EntryType)
            {
                case OBJECT_TYPE.SCRIPT:
                    ObjectData = new GameScript(BR);
                    break;
                case OBJECT_TYPE.OBJECT:
                    ObjectData = new GameObject(BR);
                    break;
                default:
                    throw new NotImplementedException($"Unknown Entry TYPE={EntryType} at POS={BR.BaseStream.Position - 4}");
            }

        }

        /// <summary>
        /// Export Entry to stream
        /// </summary>
        /// <param name="BW"></param>
        /// <remarks>Not the <see cref="Properties"/> as this is done later</remarks>
        public void Export(BinaryWriter BW)
        {
            BW.Write(EntryType);
            ObjectData.Export(BW);
        }

        public object Clone()
        {
            var Copy = (SaveFileEntry)MemberwiseClone();
            //Make sure the properties are only a copy and not a reference
            Copy.Properties = (byte[])Copy.Properties.Clone();
            Copy.ObjectData = (GameBaseObject)Copy.ObjectData.Clone();
            return Copy;
        }
    }
}
