using System;
using System.IO;
using System.Text;

namespace SatisfactorySaveEditor.Audio
{
    public class WAVHeader
    {
        public uint RIFFSize
        { get; set; }
        public ushort AudioFormat
        { get; set; }
        public ushort ChannelCount
        { get; set; }
        public uint SampleRate
        { get; set; }
        public uint ByteRate
        { get; set; }
        public ushort BlockAlign
        { get; set; }
        public ushort BitsPerSample
        { get; set; }
        public uint DataSize
        { get; set; }
        public long DataOffset
        { get; set; }

        public TimeSpan Duration
        {
            get
            {
                if (ChannelCount == 0 || BitsPerSample == 0)
                {
                    return TimeSpan.FromSeconds(0);
                }
                var BytesPerSecond = SampleRate * BitsPerSample * ChannelCount / 8;
                return TimeSpan.FromSeconds(DataSize / BytesPerSecond);
            }
        }

        public WAVHeader(Stream Input)
        {
            var DE = Encoding.Default;
            using (var BR = new BinaryReader(Input, DE, true))
            {
                if (DE.GetString(BR.ReadBytes(4)) != "RIFF")
                {
                    throw new InvalidDataException("Wave file must start with 'RIFF'");
                }
                RIFFSize = BR.ReadUInt32();
                if (DE.GetString(BR.ReadBytes(4)) != "WAVE")
                {
                    throw new InvalidDataException("'WAVE' chunk expected but not there");
                }
                if (DE.GetString(BR.ReadBytes(4)) != "fmt ")
                {
                    throw new InvalidDataException("Format chunk expected but not there");
                }
                var Chunksize = BR.ReadUInt32();
                AudioFormat = BR.ReadUInt16();
                ChannelCount = BR.ReadUInt16();
                SampleRate = BR.ReadUInt32();
                ByteRate = BR.ReadUInt32();
                BlockAlign = BR.ReadUInt16();
                BitsPerSample = BR.ReadUInt16();
                //Discard rest of chunk
                BR.ReadBytes((int)(Chunksize - 16));
                while (DE.GetString(BR.ReadBytes(4)) != "data")
                {
                    var Length = BR.ReadUInt32();
                    BR.ReadBytes((int)Length);
                }
                DataSize = BR.ReadUInt32();
                DataOffset = Input.Position;
            }
        }

        public void Write(Stream Output)
        {
            var DE = Encoding.Default;
            using (var BW = new BinaryWriter(Output, DE, true))
            {
                BW.Write(DE.GetBytes("RIFF"));
                BW.Write((uint)(DataOffset + DataSize));
                BW.Write(DE.GetBytes("WAVEfmt "));
                BW.Write(16);
                BW.Write(AudioFormat);
                BW.Write(ChannelCount);
                BW.Write(SampleRate);
                BW.Write(ByteRate);
                BW.Write(BlockAlign);
                BW.Write(BitsPerSample);
                BW.Write(DE.GetBytes("data"));
                BW.Write(DataSize);
                BW.Flush();
            }
        }
    }
}
