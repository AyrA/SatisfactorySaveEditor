using System;
using System.IO;
using System.IO.Compression;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Provides Easy access to gzip compression
    /// </summary>
    public static class Compression
    {
        /// <summary>
        /// Copies a file to a new location and compresses it
        /// </summary>
        /// <param name="InName">Original file</param>
        /// <param name="OutName">New file</param>
        /// <returns>true, if copied and compressed sucessfully</returns>
        /// <remarks>
        /// The user must clean up the file referenced by <paramref name="OutName"/> if the function fails.
        /// </remarks>
        public static bool CompressFile(string InName, string OutName)
        {
            Log.Write("Compressing \"{0}\" --> \"{1}\"", InName, OutName);
            try
            {
                using (var IN = File.OpenRead(InName))
                {
                    using (var OUT = File.Create(OutName))
                    {
                        using (var GZS = new GZipStream(OUT, CompressionLevel.Optimal))
                        {
                            IN.CopyTo(GZS);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Compresses a byte array into gzip
        /// </summary>
        /// <param name="In">byte array</param>
        /// <returns>Compressed data</returns>
        public static byte[] CompressData(byte[] In)
        {
            return CompressData(In, 0, In.Length);
        }

        /// <summary>
        /// Compresses a byte array into gzip
        /// </summary>
        /// <param name="In">byte array</param>
        /// <param name="Index">array offset</param>
        /// <param name="Count">byte count</param>
        /// <returns>Compressed data</returns>
        public static byte[] CompressData(byte[] In, int Index, int Count)
        {
            using (var MS = new MemoryStream())
            {
                using (var GZS = new GZipStream(MS, CompressionLevel.Optimal))
                {
                    GZS.Write(In, Index, Count);
                }
                return MS.ToArray();
            }
        }

        /// <summary>
        /// Compresses a stream
        /// </summary>
        /// <param name="In">Input stream</param>
        /// <param name="Out">Output stream</param>
        /// <remarks><paramref name="In"/> is left open after the operation</remarks>
        public static void CompressStream(Stream In, Stream Out)
        {
            using (var GZS = new GZipStream(Out, CompressionLevel.Optimal, true))
            {
                In.CopyTo(GZS);
            }
        }

        /// <summary>
        /// Copies a file to a new location and decompresses it
        /// </summary>
        /// <param name="InName">Original file</param>
        /// <param name="OutName">New file</param>
        /// <returns>true, if copied and decompressed sucessfully</returns>
        /// <remarks>
        /// The user must clean up the file referenced by <paramref name="OutName"/> if the function fails.
        /// </remarks>
        public static bool DecompressFile(string InName, string OutName)
        {
            Log.Write("Decompressing \"{0}\" --> \"{1}\"", InName, OutName);
            try
            {
                using (var IN = File.OpenRead(InName))
                {
                    using (var OUT = File.Create(OutName))
                    {
                        using (var GZS = new GZipStream(IN, CompressionMode.Decompress))
                        {
                            GZS.CopyTo(OUT);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Decompresses a byte array from gzip
        /// </summary>
        /// <param name="In">Compressed byte array</param>
        /// <param name="Index">array offset</param>
        /// <param name="Count">byte count</param>
        /// <returns>Decompressed data</returns>
        public static byte[] DecompressData(byte[] In)
        {
            return DecompressData(In, 0, In.Length);
        }

        /// <summary>
        /// Decompresses a byte array from gzip
        /// </summary>
        /// <param name="In">Compressed byte array</param>
        /// <param name="Index">array offset</param>
        /// <param name="Count">byte count</param>
        /// <returns>Decompressed data</returns>
        public static byte[] DecompressData(byte[] In, int Index, int Count)
        {
            using (var MS = new MemoryStream(In, Index, Count))
            {
                using (var OUT = new MemoryStream())
                {
                    using (var GZS = new GZipStream(MS, CompressionMode.Decompress))
                    {
                        GZS.CopyTo(OUT);
                    }
                    return OUT.ToArray();
                }
            }
        }

        /// <summary>
        /// Decompresses a stream
        /// </summary>
        /// <param name="In">Input stream</param>
        /// <param name="Out">Output stream</param>
        /// <remarks><paramref name="In"/> is left open after the operation</remarks>
        public static void DecompressStream(Stream In, Stream Out)
        {
            using (var GZS = new GZipStream(In, CompressionMode.Decompress, true))
            {
                GZS.CopyTo(Out);
            }
        }

    }
}
