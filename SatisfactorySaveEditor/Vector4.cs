using System;
using System.IO;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Map coordinates
    /// </summary>
    /// <remarks>This entire class is purely a guess</remarks>
    [Serializable]
    public class Vector4 : ICloneable
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

        public Vector4() : this(0, 0, 0, 0)
        {
        }

        public Vector4(float x = 0f, float y = 0f, float z = 0f, float w = 0f)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
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

        public object Clone()
        {
            return new Vector4(X, Y, Z, W);
        }
    }
}
