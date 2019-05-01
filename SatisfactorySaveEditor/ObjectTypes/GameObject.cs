using System.IO;

namespace SatisfactorySaveEditor.ObjectTypes
{
    /// <summary>
    /// "Constructable" Game object
    /// </summary>
    public class GameObject : GameBaseObject
    {
        /// <summary>
        /// Object scale
        /// </summary>
        public Position ObjectScale { get; set; }
        /// <summary>
        /// Unknown bytes with object specific information
        /// </summary>
        /// <remarks>Always 28 bytes, entry not length prefixed</remarks>
        public byte[] UnknownBytes { get; set; }
        /// <summary>
        /// Unknown integer at header end, always 1
        /// </summary>
        public int UnknownHeaderEnd { get; set; }
        /// <summary>
        /// Unknown integer at header start, always 1
        /// </summary>
        public int UnknownInt { get; set; }

        /// <summary>
        /// Creates new game object
        /// </summary>
        /// <param name="BR">Open Reader</param>
        public GameObject(BinaryReader BR)
        {
            Fill(BR);
            UnknownInt = BR.ReadInt32(); //Discard? always 1, maybe object type again
            UnknownBytes = BR.ReadBytes(28); //Unknown bytes, maybe object specific properties
            
            ObjectScale = new Position(BR);
            UnknownHeaderEnd = BR.ReadInt32();

            ObjectType = OBJECT_TYPE.OBJECT;
        }

        /// <summary>
        /// Exports this game object to file
        /// </summary>
        /// <param name="BW">Open Writer</param>
        public override void Export(BinaryWriter BW)
        {
            base.Export(BW);
            BW.Write(UnknownInt);
            BW.Write(UnknownBytes);
            ObjectScale.Export(BW);
            BW.Write(UnknownHeaderEnd);
        }
    }
}