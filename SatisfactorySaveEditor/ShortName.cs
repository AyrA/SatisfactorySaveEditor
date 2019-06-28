using System;
using System.Linq;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Provides a short name for save file entries
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

        public ShortName(string LongName)
        {
            Long = LongName;
            Short = LongName.Split('/', '.').Last().Replace('_', ' ');
            //TODO: Provide better short names
            SortLong = UseLong = false;
        }

        public override string ToString()
        {
            return UseLong ? Long : Short;
        }

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
    }
}
