using System;
using System.IO;
using System.Xml.Serialization;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Provides application settings
    /// </summary>
    [Serializable]
    public class Settings
    {
        /// <summary>
        /// Show the resize hint
        /// </summary>
        public bool ShowResizeHint
        { get; set; }
        /// <summary>
        /// Show the general editor limitations
        /// </summary>
        public bool ShowLimited
        { get; set; }
        /// <summary>
        /// Show the duplication problems hint
        /// </summary>
        public bool ShowDuplicationHint
        { get; set; }
        /// <summary>
        /// Show the item removal hint
        /// </summary>
        public bool ShowDeletionHint
        { get; set; }

        /// <summary>
        /// Initializes default settings
        /// </summary>
        public Settings()
        {
            ShowResizeHint =
                ShowLimited =
                ShowDuplicationHint =
                ShowDeletionHint =
                true;
        }

        /// <summary>
        /// Loads settings from a string
        /// </summary>
        /// <param name="Content">Swettings string</param>
        /// <returns>Settings instance</returns>
        public static Settings Load(string Content)
        {
            var S = new XmlSerializer(typeof(Settings));
            using (var SR = new StringReader(Content))
            {
                return (Settings)S.Deserialize(SR);
            }
        }

        /// <summary>
        /// Saves settings to a string
        /// </summary>
        /// <returns>Settings string</returns>
        public string Save()
        {
            var S = new XmlSerializer(typeof(Settings));
            using (var SW = new StringWriter())
            {
                S.Serialize(SW, this);
                return SW.ToString();
            }
        }
    }
}
