using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SatisfactorySaveEditor.Audio
{
    public static class WaveFinder
    {
        private static Thread T;
        private static volatile bool Cont;
        private static List<WaveFileInfo> Files;

        public static WaveFileInfo[] WaveFiles
        {
            get
            {
                return Files == null ? null : Files.ToArray();
            }
        }

        public static bool IsScanning
        {
            get
            {
                return T != null && T.IsAlive;
            }
        }

        public static void StopSearch()
        {
            if (T != null)
            {
                Cont = false;
                T.Join();
            }
        }

        public static void FindAudio(Stream Input)
        {
            if (T != null)
            {
                throw new InvalidOperationException("Can't start a secondary scan process");
            }
            if (Input == null)
            {
                throw new ArgumentNullException(nameof(Input));
            }
            if (!Input.CanSeek)
            {
                throw new ArgumentException("Stream must be seekable", nameof(Input));
            }
            T = new Thread(Scanner);
            T.Priority = ThreadPriority.BelowNormal;
            T.IsBackground = true;
            T.Start(Input);
        }

        private static void Scanner(object o)
        {
            var S = (Stream)o;
            Cont = true;
            Files = new List<WaveFileInfo>();
            var buffer = new byte[100 * 1000 * 1000];
            var SEQ = Encoding.Default.GetBytes("WAVEfmt ");
#if DEBUG
            //For debug only.
            //Right now the audio files seem to be towards the end of the file.
            //To speed up debugging, we skip the first half of the file.
            S.Seek(S.Length / 2, SeekOrigin.Begin);
#endif
            while (Cont)
            {
                if (S.Position > 0)
                {
                    S.Seek(-SEQ.Length, SeekOrigin.Current);
                }
                var StartPos = S.Position;
                int count = S.Read(buffer, 0, buffer.Length);
                Cont = count == buffer.Length;
                for (var i = 0; i < count - SEQ.Length; i++)
                {
                    //Don't bother doing anything if the first character is no match
                    if (SEQ[0] == buffer[i])
                    {
                        for (var j = 0; j < SEQ.Length; j++)
                        {
                            if (SEQ[j] != buffer[i + j])
                            {
                                break;
                            }
                            else if (j == SEQ.Length - 1)
                            {
                                Log.Write("WaveFinder: Found audio at {0}", S.Position - count + i);
                                var LastPos = S.Position;
                                //Found wave chunk, go to start of it
                                S.Seek(StartPos + i - 8, SeekOrigin.Begin);
                                var Info = new WaveFileInfo()
                                {
                                    FilePosition = StartPos + i - 8,
                                    Header = new WAVHeader(S)
                                };
                                //This is always 1 for some reason in Satisfactory
                                Log.Write("WaveFinder: Audio channel count: {0}", Info.Header.ChannelCount);
                                //Info.Header.ChannelCount = 2;

                                S.Seek(Info.Header.DataOffset, SeekOrigin.Begin);
                                if (IsOgg(S))
                                {
                                    Info.Type = WaveFileType.OGG;
                                }
                                else if (Info.Header.SampleRate == 44100 && Info.Header.BitsPerSample == 16)
                                {
                                    Info.Type = WaveFileType.PCM;
                                }
                                else
                                {
                                    Info.Type = WaveFileType.Invalid;
                                }
                                //Remove overlapping section
                                //This means the Wave file was inside another wave file
                                if (Overlaps(Info, Files.LastOrDefault()))
                                {
                                    Log.Write("WaveFinder: Removed overlapping WAV segment at NEW={0}; OLD={1}", Info.FilePosition, Files.Last().FilePosition);
                                    Files.RemoveAt(Files.Count - 1);
                                }
                                Files.Add(Info);
                                //Restore stream position
                                S.Seek(LastPos, SeekOrigin.Begin);
                            }
                        }
                    }
                }
            }
            T = null;
        }

        private static bool Overlaps(WaveFileInfo A, WaveFileInfo B)
        {
            if (A == null || B == null)
            {
                return false;
            }
            //End of A
            var AE = A.FilePosition + A.Header.DataSize + (A.Header.DataOffset - A.FilePosition);
            //End of B
            var BE = B.FilePosition + B.Header.DataSize + (B.Header.DataOffset - B.FilePosition);
            //A is entirely before B or B is entirely before A
            if (AE <= B.FilePosition || BE <= A.FilePosition)
            {
                return false;
            }

            //A starts inside B or B starts inside A
            if ((A.FilePosition > B.FilePosition && A.FilePosition < BE) || (B.FilePosition > A.FilePosition && B.FilePosition < AE))
            {
                return true;
            }

            //A ends inside B or B ends inside A
            if ((AE > B.FilePosition && AE < BE) || (BE > A.FilePosition && BE < AE))
            {
                return true;
            }

            throw new NotImplementedException("");
        }

        private static bool IsOgg(Stream S)
        {
            byte[] Data = new byte[4];
            S.Seek(-S.Read(Data, 0, Data.Length), SeekOrigin.Current);
            return Data[0] == 'O' && Data[1] == 'g' && Data[2] == 'g' && Data[3] == 'S';
        }
    }

    public class WaveFileInfo
    {
        public long FilePosition;
        public WaveFileType Type;
        public WAVHeader Header;
    }

    public enum WaveFileType
    {
        PCM,
        OGG,
        Invalid
    }
}
