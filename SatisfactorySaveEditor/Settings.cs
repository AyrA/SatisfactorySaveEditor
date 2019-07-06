using System;
using System.IO;
using System.Xml.Serialization;

namespace SatisfactorySaveEditor
{
    [Serializable]
    public class Settings
    {
        public bool ShowResizeHint
        { get; set; }
        public bool ShowLimited
        { get; set; }
        public bool ShowDuplicationHint
        { get; set; }
        public bool ShowDeletionHint
        { get; set; }

        public Settings()
        {
            ShowResizeHint =
                ShowLimited =
                ShowDuplicationHint =
                ShowDeletionHint =
                true;
        }

        public static Settings Load(string Content)
        {
            var S = new XmlSerializer(typeof(Settings));
            using (var SR = new StringReader(Content))
            {
                return (Settings)S.Deserialize(SR);
            }
        }

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
