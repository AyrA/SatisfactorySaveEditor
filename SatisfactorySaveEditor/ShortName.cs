using System;
using System.Linq;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Provides a short name for save file entries to make the UI look nicer
    /// </summary>
    public class ShortName : IComparable
    {
        /// <summary>
        /// Full entry name
        /// </summary>
        public string Long
        { get; private set; }

        /// <summary>
        /// Short entry name
        /// </summary>
        public string Short
        { get; private set; }

        /// <summary>
        /// Use <see cref="Long"/>
        /// in <see cref="ToString"/>
        /// instead of <see cref="Short"/>
        /// </summary>
        public bool UseLong
        { get; set; }

        /// <summary>
        /// Use <see cref="Long"/>
        /// in <see cref="CompareTo(object)"/>
        /// instead of <see cref="Short"/>
        /// </summary>
        public bool SortLong
        { get; set; }

        /// <summary>
        /// Initializes a short name instance from a long name instance
        /// </summary>
        /// <param name="LongName">Long name</param>
        public ShortName(string LongName)
        {
            Long = LongName;
            //Default short name is last segment of long name without very short parts
            Short = string.Join(" ", LongName.Split('/', '.').Last().Split('_').Where(m => m.Length > 2));

            //TODO: Provide better short names
            SortLong = UseLong = false;
        }

        /// <summary>
        /// Gets the string of this instance
        /// </summary>
        /// <returns>string</returns>
        /// <remarks>Use <see cref="UseLong"/> for configuration of this value</remarks>
        public override string ToString()
        {
            return UseLong ? Long : Short;
        }

        /// <summary>
        /// Compares this object to another for sorting purposes
        /// </summary>
        /// <param name="obj">Other object</param>
        /// <returns>Sort value</returns>
        /// <remarks>See <see cref="SortLong"/> for configuration of this value</remarks>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            if (obj.GetType() != typeof(ShortName))
            {
                throw new ArgumentException("Expecting argument of type ShortName");
            }
            var o = (ShortName)obj;

            return SortLong ? Long.CompareTo(o.Long) : Short.CompareTo(o.Short);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is ShortName)
            {
                return Short == ((ShortName)obj).Short;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Long.GetHashCode();
        }
    }
}
