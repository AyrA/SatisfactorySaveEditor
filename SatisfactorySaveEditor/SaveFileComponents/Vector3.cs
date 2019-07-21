using System;
using System.IO;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Map coordinates for 3 dimensions
    /// </summary>
    [Serializable]
    public class Vector3 : ICloneable
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
        /// <remarks>This will be the height if this instance represents map coordinates</remarks>
        public float Z
        { get; set; }

        /// <summary>
        /// Reads positons
        /// </summary>
        /// <param name="BR">Open Reader</param>
        public Vector3(BinaryReader BR)
        {
            X = BR.ReadSingle();
            Y = BR.ReadSingle();
            Z = BR.ReadSingle();
        }

        /// <summary>
        /// Creates an empty instance for deserialization
        /// </summary>
        public Vector3() : this(0, 0, 0)
        {
        }

        /// <summary>
        /// Creates an instance with the given points
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        public Vector3(float x = 0f, float y = 0f, float z = 0f)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Show nice coordinates
        /// </summary>
        /// <returns>Coordinates</returns>
        public override string ToString()
        {
            return string.Format("X={0} Y={1} Z={2}", Math.Round(X, 3), Math.Round(Y, 3), Math.Round(Z, 3));
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
        }

        /// <summary>
        /// Creates a copy of this instance
        /// </summary>
        /// <returns>Copy</returns>
        public object Clone()
        {
            return new Vector3(X, Y, Z);
        }

        /// <summary>
        /// Checks if the given Vector has the same coordinates
        /// </summary>
        /// <param name="obj">Vector</param>
        /// <returns>true if same coordinates, false if other coordinates or not same vector type</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is Vector3)
            {
                var o = (Vector3)obj;
                return o.X == X && o.Y == Y && o.Z == Z;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }
    }
}
