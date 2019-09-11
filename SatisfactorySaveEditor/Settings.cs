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
        /// Performs automatic update checks
        /// </summary>
        public bool AutoUpdate
        { get; set; }

        /// <summary>
        /// Show the changelog automatically after an update
        /// </summary>
        public bool ShowChangelog
        { get; set; }

        /// <summary>
        /// Uses a random Id for <see cref="ReportId"/> each time the application is launched
        /// </summary>
        public bool UseRandomId
        { get; set; }

        /// <summary>
        /// Disables usage reporting completely
        /// </summary>
        public bool DisableUsageReport
        { get; set; }

        /// <summary>
        /// Id used for usage report
        /// </summary>
        public Guid ReportId
        { get; set; }

        /// <summary>
        /// Key for the SMR API
        /// </summary>
        /// <remarks>See https://cable.ayra.ch/satiafactory/maps/help</remarks>
        public Guid ApiKey
        { get; set; }

        /// <summary>
        /// Last time an update check was performed
        /// </summary>
        public DateTime LastUpdateCheck
        { get; set; }

        /// <summary>
        /// The last version where the changelog was shown
        /// </summary>
        public string LastVersionLogShown
        { get; set; }

        /// <summary>
        /// Initializes default settings
        /// </summary>
        public Settings()
        {
            ReportId = Guid.NewGuid();
            LastVersionLogShown = "0.0.0.0";
            LastUpdateCheck = DateTime.MinValue;
            //Default message status
            MarkDialogsRead(false);
            //Update settings
            AutoUpdate = true;
            //Changelog settings
            ShowChangelog = true;
            //Report settings
            UseRandomId = false;
            DisableUsageReport = false;
        }

        /// <summary>
        /// Marks all dialogs as read or unread
        /// </summary>
        /// <param name="MarkAsRead">Mark dialogs as read</param>
        public void MarkDialogsRead(bool MarkAsRead)
        {
            ShowResizeHint =
                ShowLimited =
                ShowDuplicationHint =
                ShowDeletionHint =
                ShowWelcomeMessage =
                ShowRangeDeleterHint =
                !MarkAsRead;

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
