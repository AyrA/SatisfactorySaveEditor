using System.IO;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// String from the property list.
    /// </summary>
    public class PropertyString
    {
        /// <summary>
        /// String Name
        /// </summary>
        /// <remarks>Names can be duplicate, so don't use them as key</remarks>
        public string Name { get; private set; }
        /// <summary>
        /// String Value
        /// </summary>
        /// <remarks>Values seem to be unique as they seem to reference objects on the map</remarks>
        public string Value { get; private set; }

        /// <summary>
        /// Reads a string from the property list
        /// </summary>
        /// <param name="BR">Open Reader</param>
        public PropertyString(BinaryReader BR)
        {
            Name = BR.ReadIntString();
            Value = BR.ReadIntString();
        }

        /// <summary>
        /// Writes this string to a stream
        /// </summary>
        /// <param name="BW">Open Writer</param>
        public void Export(BinaryWriter BW)
        {
            BW.WriteIntString(Name);
            BW.WriteIntString(Value);
        }
    }
}
