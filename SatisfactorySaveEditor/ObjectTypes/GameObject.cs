using System.IO;

namespace SatisfactorySaveEditor.ObjectTypes
{
    /// <summary>
    /// "Constructable" Game object
    /// </summary>
    public class GameObject : GameBaseObject
    {
        /// <summary>
        /// Object rotation
        /// </summary>
        public Vector4 ObjectRotation { get; set; }
        /// <summary>
        /// Object position
        /// </summary>
        public Vector3 ObjectPosition { get; set; }
        /// <summary>
        /// Object scale
        /// </summary>
        public Vector3 ObjectScale { get; set; }
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
            
            ObjectRotation = new Vector4(BR);
            ObjectPosition = new Vector3(BR);
            ObjectScale = new Vector3(BR);
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
            ObjectRotation.Export(BW);
            ObjectPosition.Export(BW);
            ObjectScale.Export(BW);
            BW.Write(UnknownHeaderEnd);
        }
    }
}