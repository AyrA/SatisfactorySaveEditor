using System.IO;
using System.Text;

namespace SatisfactorySaveEditor
{
    public class CompressedHeader
    {
        /// <summary>
        /// The default chunk size utilized by the engine
        /// </summary>
        public const ulong DEFAULT_CHUNK_SIZE = 131072;
        /// <summary>
        /// Some kind of magic number
        /// </summary>
        public const ulong DEFAULT_FILE_TAG = 2653586369;

        /// <summary>
        /// File tag
        /// </summary>
        public ulong FileTag;
        /// <summary>
        /// Maximum supported chunk size
        /// </summary>
        public ulong MaxChunkSize;
        /// <summary>
        /// Length of compressed byte data
        /// </summary>
        public ulong CompressedLength;
        /// <summary>
        /// Length of decompressed byte data
        /// </summary>
        public ulong DecompressedLength;

        public CompressedHeader()
        {
            FileTag = DEFAULT_FILE_TAG;
            MaxChunkSize = DEFAULT_CHUNK_SIZE;
            CompressedLength = 0;
            DecompressedLength = 0;
        }

        public CompressedHeader(Stream Input)
        {
            using (var BR = new BinaryReader(Input, Encoding.Default, true))
            {
                FileTag = BR.ReadUInt64();
                MaxChunkSize = BR.ReadUInt64();
                CompressedLength = BR.ReadUInt64();
                DecompressedLength = BR.ReadUInt64();
                //Discard duplicate values
                CompressedLength = BR.ReadUInt64();
                DecompressedLength = BR.ReadUInt64();
            }
        }

        public void WriteHeader(BinaryWriter BW)
        {
            BW.Write(FileTag);
            BW.Write(MaxChunkSize);
            BW.Write(CompressedLength);
            BW.Write(DecompressedLength);
            BW.Write(CompressedLength);
            BW.Write(DecompressedLength);
        }

        public void WriteHeader(Stream S)
        {
            using (var BW = new BinaryWriter(S, Encoding.Default, true))
            {
                WriteHeader(BW);
            }
        }
    }
}
