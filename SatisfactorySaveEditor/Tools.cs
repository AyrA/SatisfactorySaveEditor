using System.IO;
using System.Text;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Tools to make your life easier
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Reads the weirdly encoded strings
        /// </summary>
        /// <param name="BR">Open Reader</param>
        /// <returns>string</returns>
        public static string ReadIntString(this BinaryReader BR)
        {
            byte[] Data = BR.ReadBytes(BR.ReadInt32());
            if (Data.Length > 0)
            {
                //Strings are length prefixed AND null terminated in this file format for some reason
                //We remove the null termination to make editing easier
                return Encoding.UTF8.GetString(Data).TrimEnd('\0');
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes weirdly encoded strings to a stream
        /// </summary>
        /// <param name="BW">Open Writer</param>
        /// <param name="S">String</param>
        public static void WriteIntString(this BinaryWriter BW, string S)
        {
            //Add null termination back if needed
            if (string.IsNullOrEmpty(S))
            {
                S = "\0";
            }
            if (!S.EndsWith("\0"))
            {
                S += '\0';
            }
            byte[] Data = Encoding.UTF8.GetBytes(S);
            BW.Write(Data.Length);
            BW.Write(Data);
        }

    }
}
