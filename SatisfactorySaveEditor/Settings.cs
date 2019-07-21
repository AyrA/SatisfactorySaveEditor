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
        /// Shows a one time welcome message
        /// </summary>
        public bool ShowWelcomeMessage
        { get; set; }
        /// <summary>
        /// Shows a hint about the range deleter
        /// </summary>
        public bool ShowRangeDeleterHint
        { get; set; }

        /// <summary>
        /// Last time an update check was performed
        /// </summary>
        public DateTime LastUpdateCheck
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
                ShowWelcomeMessage =
                ShowRangeDeleterHint =
                true;
            LastUpdateCheck = DateTime.MinValue;
        }

        /// <summary>
        /// Loads settings from a string
        /// </summary>
        /// <param name="Content">Swettings string</param>
        /// <returns>Settings instance</returns>
        public static Settings Load(string Content)
        {
            Log.Write("Settings: Loading settings");
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
            Log.Write("Settings: Saving settings");
            var S = new XmlSerializer(typeof(Settings));
            using (var SW = new StringWriter())
            {
                S.Serialize(SW, this);
                return SW.ToString();
            }
        }
    }
}
