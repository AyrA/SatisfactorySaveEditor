using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SMRAPI
{
    /// <summary>
    /// Tools to make our life easier
    /// </summary>
    internal static class Tools
    {
        /// <summary>
        /// Serialize an object as XML
        /// </summary>
        /// <param name="o">object</param>
        /// <returns>XML string</returns>
        internal static string ToXml(this object o)
        {
            XmlSerializer S = new XmlSerializer(o.GetType());
            using (var MS = new MemoryStream())
            {
                S.Serialize(MS, o);
                return Encoding.UTF8.GetString(MS.ToArray());
            }
        }

        /// <summary>
        /// Deserialize an XML string to an object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="s">XML string</param>
        /// <returns>Deserialized object</returns>
        internal static T FromXml<T>(this string s)
        {
            XmlSerializer S = new XmlSerializer(typeof(T));
            using (var SR = new StringReader(s))
            {
                return (T)S.Deserialize(SR);
            }
        }
    }
}
