using System;
using System.IO;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Map coordinates
    /// </summary>
    /// <remarks>This entire class is purely a guess</remarks>
    public struct Vector4
    {
        /// <summary>
        /// X position
        /// </summary>
        public float X
        { get; set; }
        /// <summary>
        /// Y position
        /// </summary>
        public float Y
        { get; set; }
        /// <summary>
        /// Z position
        /// </summary>
        public float Z
        { get; set; }
        /// <summary>
        /// W position
        /// </summary>
        public float W
        { get; set; }

        /// <summary>
        /// Reads positons
        /// </summary>
        /// <param name="BR">Open Reader</param>
        public Vector4(BinaryReader BR)
        {
            X = BR.ReadSingle();
            Y = BR.ReadSingle();
            Z = BR.ReadSingle();
            W = BR.ReadSingle();
        }

        /// <summary>
        /// Show nice coordinates
        /// </summary>
        /// <returns>Coordinates</returns>
        public override string ToString()
        {
            return string.Format("X={0} Y={1} Z={2} W={2}", Math.Round(X, 3), Math.Round(Y, 3), Math.Round(Z, 3), Math.Round(W, 3));
        }

        /// <summary>
        /// Writes position to a stream
        /// </summary>
        /// <param name="BW">Open Writer</param>
        public void Export(BinaryWriter BW)
        {
            BW.Write(X);
            BW.Write(Y);
            BW.Write(Z);
            BW.Write(W);
        }
    }
}
