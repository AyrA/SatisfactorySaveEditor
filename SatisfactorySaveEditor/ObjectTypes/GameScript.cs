using System;
using System.IO;

namespace SatisfactorySaveEditor.ObjectTypes
{
    /// <summary>
    /// Script object entry
    /// </summary>
    [Serializable]
    public class GameScript : GameBaseObject
    {
        /// <summary>
        /// Name of script
        /// </summary>
        public string ScriptName { get; set; }

        /// <summary>
        /// Initializes agame script
        /// </summary>
        /// <param name="BR">Open Reader</param>
        public GameScript(BinaryReader BR)
        {
            Fill(BR);
            ScriptName = BR.ReadIntString();

            ObjectType = OBJECT_TYPE.SCRIPT;
        }

        /// <summary>
        /// Creates an empty script entry
        /// </summary>
        public GameScript()
        {

        }

        /// <summary>
        /// Exports script to stream
        /// </summary>
        /// <param name="BW">Open Writer</param>
        public override void Export(BinaryWriter BW)
        {
            base.Export(BW);
            BW.WriteIntString(ScriptName);
        }

        public override object Clone()
        {
            return (GameScript)MemberwiseClone();
        }
    }
}
