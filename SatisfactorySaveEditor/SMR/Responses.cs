using System;
using System.Linq;
using System.Xml.Serialization;

//This file contains all possible API response types
namespace SMRAPI.Responses
{
    /// <summary>
    /// This is the bast type that all others should inherit from.
    /// it also provides utility functions for the other types
    /// </summary>
    [Serializable, XmlRoot(ElementName = "api")]
    public class BaseResponse
    {
        /// <summary>
        /// This is the number of ticks that represents 1970-01-01 00:00:00 UTC
        /// </summary>
        private const long TICKS = 621355968000000000;

        /// <summary>
        /// API success result. If <see cref="false"/>,
        /// more details can be found in the <see cref="msg"/> property
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// Contains error messages if <see cref="success"/> is <see cref="false"/>.
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// This is the Time of your request
        /// </summary>
        public double time { get; set; }

        /// <summary>
        /// Converts a unix timestamp into a date object
        /// </summary>
        /// <param name="TS">Timestamp in seconds since epoch</param>
        /// <returns>Date object</returns>
        internal static DateTime ConvertTimestamp(double TS)
        {
            return (new DateTime(TICKS, DateTimeKind.Utc)).AddSeconds(TS);
        }

        /// <summary>
        /// Converts a date object into a unix timestamp
        /// </summary>
        /// <param name="TS">Date object</param>
        /// <returns>Seconds since epoch</returns>
        /// <remarks>Due to the very precise nature of .NET date objects, you might want to round this</remarks>
        internal static double ConvertTimestamp(DateTime DT)
        {
            return DT.ToUniversalTime().Subtract((new DateTime(TICKS, DateTimeKind.Utc))).TotalSeconds;
        }
    }

    /// <summary>
    /// Response of the "details" command
    /// </summary>
    [Serializable, XmlRoot(ElementName = "api")]
    public class MapDetailsResponse : BaseResponse
    {
        public InfoResponse.map data { get; set; }
    }

    /// <summary>
    /// Response of the "test" command
    /// </summary>
    [Serializable, XmlRoot(ElementName = "api")]
    public class TestResponse : BaseResponse
    {
        /// <summary>
        /// "test" command response structure
        /// </summary>
        [Serializable]
        public class DataValue
        {
            /// <summary>
            /// Current API key
            /// </summary>
            public Guid key { get; set; }

            //We don't send additional data in this demo but you can add fields here
        }

        /// <summary>
        /// "test" command data
        /// </summary>
        public DataValue data { get; set; }
    }

    /// <summary>
    /// "info" command response structure
    /// </summary>
    [Serializable, XmlRoot(ElementName = "api")]
    public class InfoResponse : BaseResponse
    {
        /// <summary>
        /// The data of this command
        /// </summary>
        [Serializable]
        public class DataValue
        {
            /// <summary>
            /// Represents a user
            /// </summary>
            [Serializable]
            public class UserValue
            {
                /// <summary>
                /// User id
                /// </summary>
                [XmlAttribute]
                public int id { get; set; }
                /// <summary>
                /// User name
                /// </summary>
                [XmlText]
                public string name { get; set; }
            }

            /// <summary>
            /// Map categories and the conflict list
            /// </summary>
            [Serializable]
            public class CategoriesResult
            {
                /// <summary>
                /// Represents a single category
                /// </summary>
                [Serializable, XmlRoot(ElementName = "category")]
                public class category
                {
                    /// <summary>
                    /// Category id
                    /// </summary>
                    [XmlAttribute("id")]
                    public int id { get; set; }
                    /// <summary>
                    /// Category short name
                    /// </summary>
                    public string name { get; set; }
                    /// <summary>
                    /// Category description
                    /// </summary>
                    public string description { get; set; }
                }

                /// <summary>
                /// Represents the conflict map of a category
                /// </summary>
                [Serializable]
                public class conflict
                {
                    /// <summary>
                    /// Category id
                    /// </summary>
                    [XmlAttribute("category")]
                    public int Category { get; set; }
                    /// <summary>
                    /// Mutually exclusive categories
                    /// </summary>
                    [XmlElement("category")]
                    public int[] conflicts;

                    /// <summary>
                    /// If a category is its own conflict, it is disabled.
                    /// </summary>
                    public bool IsDisabled
                    {
                        get
                        {
                            return conflicts != null && conflicts.Contains(Category);
                        }
                    }
                }

                /// <summary>
                /// List of categories
                /// </summary>
                public category[] list { get; set; }
                /// <summary>
                /// Conflict map
                /// </summary>
                public conflict[] conflicts { get; set; }
            }

            /// <summary>
            /// Current user
            /// </summary>
            public UserValue user { get; set; }
            /// <summary>
            /// Available categories
            /// </summary>
            public CategoriesResult categories { get; set; }
            /// <summary>
            /// User maps
            /// </summary>
            /// <remarks>This includes private maps</remarks>
            public map[] maps { get; set; }
        }

        /// <summary>
        /// Represents a map entry
        /// </summary>
        [Serializable]
        public class map
        {
            /// <summary>
            /// Map size
            /// </summary>
            [Serializable]
            public class Size
            {
                /// <summary>
                /// Size of the compressed content delivered from the server
                /// </summary>
                /// <remarks>Map is gzip compressed</remarks>
                public long compressed { get; set; }
                /// <summary>
                /// Size of the decompressed map file
                /// </summary>
                public long real { get; set; }
            }

            /// <summary>
            /// Contains all categories of the map
            /// </summary>
            [Serializable]
            public class MapCategoryList
            {
                /// <summary>
                /// Category list
                /// </summary>
                [XmlElement("category")]
                public int[] category { get; set; }
            }


            /// <summary>
            /// Contains info about the map owner
            /// </summary>
            [Serializable]
            public class MapOwnerInfo
            {
                /// <summary>
                /// User id
                /// </summary>
                [XmlAttribute("id")]
                public int id { get; set; }
                /// <summary>
                /// User name
                /// </summary>
                [XmlText]
                public string name { get; set; }
            }


            /// <summary>
            /// Map id
            /// </summary>
            /// <remarks>
            /// This is the public id.
            /// It only works for the owner or public maps
            /// </remarks>
            public Guid id { get; set; }
            /// <summary>
            /// Date this map was published at as unix timestamp
            /// See <see cref="Published"/> for a <see cref="DateTime"/> object
            /// </summary>
            public long publishdate { get; set; }
            /// <summary>
            /// Map file name. Includes ".sav"
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// User specified map description
            /// </summary>
            public string description { get; set; }
            /// <summary>
            /// Game version this file was created with
            /// </summary>
            public int version { get; set; }
            /// <summary>
            /// Sharing status
            /// </summary>
            /// <remarks>0=Private, 1=Public</remarks>
            public int @public { get; set; }
            /// <summary>
            /// Hidden map Id
            /// </summary>
            /// <remarks>
            /// This id allows map preview and downloads of private maps.
            /// You can also use this id in API calls but you still need to be the owner
            /// </remarks>
            public Guid hidden_id { get; set; }
            /// <summary>
            /// Associated categories
            /// </summary>
            public MapCategoryList categories { get; set; }
            /// <summary>
            /// Map file sizes
            /// </summary>
            public Size size { get; set; }
            /// <summary>
            /// Map user
            /// </summary>
            public MapOwnerInfo user { get; set; }
            /// <summary>
            /// Gets or sets the publication date
            /// </summary>
            public DateTime Published
            {
                get
                {
                    return ConvertTimestamp(publishdate);
                }
                set
                {
                    publishdate = (long)Math.Floor(ConvertTimestamp(value));
                }
            }
        }

        /// <summary>
        /// Info data
        /// </summary>
        public DataValue data { get; set; }
    }

    /// <summary>
    /// "edit" command response structure
    /// </summary>
    [Serializable, XmlRoot(ElementName = "api")]
    public class EditResponse : BaseResponse
    {
        /// <summary>
        /// New map information
        /// </summary>
        public InfoResponse.map data { get; set; }
    }

    /// <summary>
    /// "del" command response data
    /// </summary>
    [Serializable, XmlRoot(ElementName = "api")]
    public class DelResponse : BaseResponse
    {
        //This command has no "data" property. Check "success"
    }

    /// <summary>
    /// "newid" command response data
    /// </summary>
    [Serializable, XmlRoot(ElementName = "api")]
    public class NewIdResponse : BaseResponse
    {
        /// <summary>
        /// New hidden id
        /// </summary>
        public Guid data { get; set; }
    }

    /// <summary>
    /// "list" command response data
    /// </summary>
    [Serializable, XmlRoot(ElementName = "api")]
    public class ListResponse : BaseResponse
    {
        /// <summary>
        /// List data
        /// </summary>
        [Serializable]
        public class ListData
        {
            /// <summary>
            /// If <see cref="true"/>,
            /// more entries are available by increasing the page counter
            /// </summary>
            public bool more { get; set; }
            /// <summary>
            /// List of maps on this page
            /// </summary>
            public InfoResponse.map[] maps { get; set; }
        }

        /// <summary>
        /// response data
        /// </summary>
        public ListData data { get; set; }
    }

    /// <summary>
    /// "add" command response data
    /// </summary>
    [Serializable, XmlRoot(ElementName = "api")]
    public class UploadResponse : BaseResponse
    {
        /// <summary>
        /// Uploaded map info
        /// </summary>
        public InfoResponse.map data { get; set; }
    }
}
