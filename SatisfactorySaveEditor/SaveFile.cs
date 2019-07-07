using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Complete save File
    /// </summary>
    public class SaveFile
    {
        /// <summary>
        /// Unknown values at the start of the header (maybe entry type, version, magic number, etc)
        /// </summary>
        public int[] HeaderValues
        { get; set; }

        /// <summary>
        /// Time played. Maximum is likely <see cref="int.MaxValue"/>
        /// </summary>
        public TimeSpan PlayTime
        { get; set; }

        /// <summary>
        /// Type of Level. Always "Persistent_Level"?
        /// </summary>
        public string LevelType
        { get; set; }

        /// <summary>
        /// Generic level properties in URL style format
        /// </summary>
        public Dictionary<string, string> Properties
        { get; set; }

        /// <summary>
        /// Level objects (Mostly a list of entities on the map)
        /// </summary>
        public List<SaveFileEntry> Entries
        { get; set; }

        /// <summary>
        /// Session name stored outside of level property list
        /// </summary>
        public string SessionName { get; set; }
        /// <summary>
        /// Property strings at the end of the map. Likely a list of removed entities.
        /// </summary>
        /// <remarks>Clearing it restores slugs, artifacts and mushrooms but not berries for example</remarks>
        public List<PropertyString> StringList { get; set; }
        /// <summary>
        /// 9 Unknown bytes
        /// </summary>
        public byte[] UnknownBytes { get; set; }
        /// <summary>
        /// unknown 4 bytes that look like a somewhat believable integer
        /// </summary>
        public int UnknownInt { get; set; }

        /// <summary>
        /// Reads a new SaveFile
        /// </summary>
        /// <param name="BR">Open Reader</param>
        private SaveFile(BinaryReader BR)
        {
            Properties = new Dictionary<string, string>();
            Entries = new List<SaveFileEntry>();
            StringList = new List<PropertyString>();

            HeaderValues = new int[]
            {
                BR.ReadInt32(),
                BR.ReadInt32()
            };
            PlayTime = TimeSpan.FromSeconds(BR.ReadSingle());

            LevelType = BR.ReadIntString();
            var Props = BR.ReadIntString().Trim('?', '\0').Split('?');
            foreach (var P in Props)
            {
                var I = P.IndexOf('=');
                if (I >= 0)
                {
                    Properties[P.Substring(0, I)] = P.Substring(I + 1);
                }
                else
                {
                    Properties[P] = null;
                }
            }
            //Session name is in the properties too for some reason but appears again
            SessionName = BR.ReadIntString();
            //Somewhat believable integer?
            UnknownInt = BR.ReadInt32();
            //No idea
            UnknownBytes = BR.ReadBytes(9);

            int HeaderCount = BR.ReadInt32();
            for (var i = 0; i < HeaderCount; i++)
            {
                Entries.Add(new SaveFileEntry(BR));
            }
            //Read entry properties.
            //For some reason this has its own counter, even though it should be in sync with the object count
            int PropertyCount = BR.ReadInt32();
            for (var i = 0; i < PropertyCount; i++)
            {
                Entries[i].Properties = BR.ReadBytes(BR.ReadInt32());
            }

            int StringCount = BR.ReadInt32();
            for (var i = 0; i < StringCount; i++)
            {
                StringList.Add(new PropertyString(BR));
            }
        }

        /// <summary>
        /// Exports this save to a stream
        /// </summary>
        /// <param name="S">Open Stream</param>
        public void Export(Stream S)
        {
            using (var BW = new BinaryWriter(S, System.Text.Encoding.ASCII, true))
            {
                BW.Write(HeaderValues[0]);
                BW.Write(HeaderValues[1]);
                BW.Write((int)PlayTime.TotalSeconds);
                BW.WriteIntString(LevelType);
                BW.WriteIntString("?" + string.Join("?", Properties.Select(m => $"{m.Key}={m.Value}")));
                BW.WriteIntString(SessionName);
                BW.Write(UnknownInt);
                BW.Write(UnknownBytes);

                BW.Write(Entries.Count);
                foreach (var E in Entries)
                {
                    E.Export(BW);
                }

                BW.Write(Entries.Count);
                foreach (var E in Entries)
                {
                    BW.Write(E.Properties.Length);
                    BW.Write(E.Properties);
                }

                BW.Write(StringList.Count);
                foreach (var E in StringList)
                {
                    E.Export(BW);
                }
            }
        }

        /// <summary>
        /// Opens a save file
        /// </summary>
        /// <param name="S">Save file stream</param>
        /// <returns>Save file. Returns null on error</returns>
        /// <remarks>This also tries to gzip decompress the file if necessary and possible</remarks>
        public static SaveFile Open(Stream S)
        {
            var Pos = S.CanSeek ? S.Position : -1L;
            try
            {
                using (var BR = new BinaryReader(S, System.Text.Encoding.Default, true))
                {
                    return new SaveFile(BR);
                }
            }
            catch
            {
                if (S.CanSeek)
                {
                    S.Position = Pos;
                    using (var GZS = new System.IO.Compression.GZipStream(S, System.IO.Compression.CompressionMode.Decompress, true))
                    {
                        using (var BR = new BinaryReader(GZS, System.Text.Encoding.Default))
                        {
                            try
                            {
                                return new SaveFile(BR);
                            }
                            catch
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
