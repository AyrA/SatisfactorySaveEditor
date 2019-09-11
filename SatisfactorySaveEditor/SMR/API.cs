using SMRAPI.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SMRAPI
{
    /// <summary>
    /// Provides an interface to the satisfactory map repository on cable.ayra.ch
    /// </summary>
    /// <remarks>
    /// WARNING!
    /// This is a reference implementation only that demonstrates all functions for testing purposes.
    /// It's not recommended that you use it as-is in your own projects but add proper error handling and data processing.
    /// </remarks>
    public static class API
    {
        /// <summary>
        /// Placeholder for authentication URL
        /// </summary>
        public const string API_AUTH_URL_PLACEHOLDER = "{URL}";
        /// <summary>
        /// Placeholder for application name
        /// </summary>
        public const string API_AUTH_NAME_PLACEHOLDER = "{NAME}";
        /// <summary>
        /// Placeholder for the map id
        /// </summary>
        public const string API_MAP_ID_PLACEHOLDER = "{ID}";
        /// <summary>
        /// Base URL of the repository
        /// </summary>
        public const string API_BASE = "https://cable.ayra.ch/satisfactory/maps";
        /// <summary>
        /// Base URL of the API endpoint
        /// </summary>
        public const string API_API = API_BASE + "/api";
        /// <summary>
        /// URL for downloading maps
        /// </summary>
        public const string API_DOWNLOAD = API_BASE + "/download/?map=" + API_MAP_ID_PLACEHOLDER;
        /// <summary>
        /// URL for previewing maps
        /// </summary>
        public const string API_PREVIEW = API_BASE + "/preview/?map=" + API_MAP_ID_PLACEHOLDER;
        /// <summary>
        /// Authentication URL
        /// </summary>
        public const string API_AUTH = API_BASE + "/remoteauth/?redir=" + API_AUTH_URL_PLACEHOLDER + "&name=" + API_AUTH_NAME_PLACEHOLDER;

        /// <summary>
        /// ID that represents an anonymous user
        /// </summary>
        public static readonly Guid API_ANONYMOUS_KEY = default(Guid);

        /// <summary>
        /// Current API key
        /// </summary>
        public static Guid ApiKey { get; set; } = API_ANONYMOUS_KEY;

        /// <summary>
        /// Creates a new request
        /// </summary>
        /// <param name="method">API method</param>
        /// <param name="values">Fields</param>
        /// <returns>API Response handler</returns>
        private static WebRequest Req(string method, Dictionary<string, object> values = null)
        {
            var Request = WebRequest.CreateHttp(API_API + "/" + method);
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";

            if (values == null)
            {
                values = new Dictionary<string, object>();
            }
            //Overwrite attempts to use a custom key
            values["key"] = ApiKey;
            List<string> Chain = new List<string>();
            foreach (var KV in values)
            {
                //Key is always a string
                var Key = Uri.EscapeDataString(KV.Key == null ? string.Empty : KV.Key);
                object Value = KV.Value;
                //Add array entries multiple times according to the API spec
                if (Value != null && Value.GetType().IsArray)
                {
                    foreach (var V in (Array)Value)
                    {
                        //Add array values as array
                        Chain.Add(string.Format(
                            "{0}[]={1}", Key,
                            Uri.EscapeDataString(V == null ? string.Empty : V.ToString())));
                    }
                }
                else
                {
                    //Add signle value as-is
                    Chain.Add(string.Format(
                        "{0}={1}", Key,
                        Value == null ? string.Empty : Value));
                }
            }
            //Build request body string
            var Data = Encoding.UTF8.GetBytes(string.Join("&", Chain));
            //Set precomputed data length
            Request.ContentLength = Data.Length;

            //Write data
            using (var S = Request.GetRequestStream())
            {
                S.Write(Data, 0, Data.Length);
            }

            //Return processed request
            return Request;
        }

        /// <summary>
        /// Gets the preview of a save file
        /// </summary>
        /// <param name="MapId">Save file id or hidden id</param>
        /// <returns>Preview data (PNG image), or null on errors</returns>
        public static byte[] Preview(Guid MapId)
        {
            if (MapId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(MapId));
            }
            var Request = WebRequest.CreateHttp(API_PREVIEW.Replace(API_MAP_ID_PLACEHOLDER, MapId.ToString()));
            try
            {
                using (var Res = Request.GetResponse())
                {
                    using (var MS = new MemoryStream())
                    {
                        using (var S = Res.GetResponseStream())
                        {
                            S.CopyTo(MS);
                        }
                        return MS.ToArray();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a save file
        /// </summary>
        /// <param name="MapId">Save file id or hidden id</param>
        /// <param name="Output">Stream to write save file to</param>
        /// <returns><see cref="true"/> on success, <see cref="false"/> on failure</returns>
        /// <remarks>
        /// The state of <paramref name="Output"/> is unknown on errors.
        /// An example would be a connection termination mid transmission.
        /// </remarks>
        public static bool Download(Guid MapId, Stream Output)
        {
            if (Output == null)
            {
                throw new ArgumentNullException(nameof(Output));
            }
            if (MapId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(MapId));
            }
            if (!Output.CanWrite)
            {
                throw new ArgumentException("Stream is not writable", nameof(Output));
            }
            var Request = WebRequest.CreateHttp(API_DOWNLOAD.Replace(API_MAP_ID_PLACEHOLDER, MapId.ToString()));
            //Automatically decompress the data. If not available in your language, use regular gzip decompression.
            Request.AutomaticDecompression = DecompressionMethods.GZip;

            try
            {
                using (var Res = Request.GetResponse())
                {
                    using (var S = Res.GetResponseStream())
                    {
                        S.CopyTo(Output);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets basic information from the API
        /// </summary>
        /// <returns>API "info" result</returns>
        public static InfoResponse Info()
        {
            var R = Req("info");
            using (var Res = R.GetResponse())
            {
                using (var SR = new StreamReader(Res.GetResponseStream()))
                {
                    return SR.ReadToEnd().FromXml<InfoResponse>();
                }
            }
        }

        /// <summary>
        /// Tests the API
        /// </summary>
        /// <returns>API "test" result</returns>
        /// <remarks>Can be used to validate the API key</remarks>
        public static TestResponse Test()
        {
            var R = Req("test");
            using (var Res = R.GetResponse())
            {
                using (var SR = new StreamReader(Res.GetResponseStream()))
                {
                    return SR.ReadToEnd().FromXml<TestResponse>();
                }
            }
        }

        /// <summary>
        /// Edits map properties
        /// </summary>
        /// <param name="MapId">Map Id</param>
        /// <param name="FileName">New file name (with or without ".sav")</param>
        /// <param name="Description">New map description</param>
        /// <param name="Category">New map category list</param>
        /// <param name="Public">New map public state</param>
        /// <returns>API "edit" result</returns>
        /// <remarks>Unset parameters are left at their default values</remarks>
        public static EditResponse EditMap(Guid MapId, string FileName = null, string Description = null, int[] Category = null, int? Public = null)
        {
            var Values = new Dictionary<string, object>();
            Values["id"] = MapId;
            if (FileName != null)
            {
                Values["name"] = FileName;
            }
            if (Description != null)
            {
                Values["description"] = Description;
            }
            if (Category != null)
            {
                Values["category"] = Category;
            }
            if (Public.HasValue)
            {
                Values["public"] = Public.Value;
            }
            var R = Req("edit", Values);
            using (var Res = R.GetResponse())
            {
                using (var SR = new StreamReader(Res.GetResponseStream()))
                {
                    return SR.ReadToEnd().FromXml<EditResponse>();
                }
            }
        }

        /// <summary>
        /// Deletes a map
        /// </summary>
        /// <param name="MapId">Map Id</param>
        /// <returns>API "del" result</returns>
        public static DelResponse DelMap(Guid MapId)
        {
            var Values = new Dictionary<string, object>();
            Values["id"] = MapId;
            var R = Req("del", Values);
            using (var Res = R.GetResponse())
            {
                using (var SR = new StreamReader(Res.GetResponseStream()))
                {
                    return SR.ReadToEnd().FromXml<DelResponse>();
                }
            }
        }

        /// <summary>
        /// Generates a new hidden id
        /// </summary>
        /// <param name="MapId">Map Id</param>
        /// <returns>API "newid" result</returns>
        public static NewIdResponse NewId(Guid MapId)
        {
            var Values = new Dictionary<string, object>();
            Values["id"] = MapId;
            var R = Req("newid", Values);
            using (var Res = R.GetResponse())
            {
                using (var SR = new StreamReader(Res.GetResponseStream()))
                {
                    return SR.ReadToEnd().FromXml<NewIdResponse>();
                }
            }
        }

        /// <summary>
        /// Lists/Searches public maps
        /// </summary>
        /// <param name="UserName">User name filter</param>
        /// <param name="Category">Category filter</param>
        /// <param name="Page">Page</param>
        /// <returns>API "list" result</returns>
        /// <remarks>Unset parameters will not filter</remarks>
        public static ListResponse List(string UserName = null, int Category = -1, int Page = -1)
        {
            var Values = new Dictionary<string, object>();
            if (UserName != null)
            {
                Values["user"] = UserName;
            }
            if (Category > 0)
            {
                Values["category"] = Category;
            }
            else if (Category != -1)
            {
                throw new ArgumentOutOfRangeException(nameof(Category));
            }
            if (Page > 0)
            {
                Values["page"] = Page;
            }
            else if (Page != -1)
            {
                throw new ArgumentOutOfRangeException(nameof(Page));
            }

            var R = Req("list", Values);
            using (var Res = R.GetResponse())
            {
                using (var SR = new StreamReader(Res.GetResponseStream()))
                {
                    return SR.ReadToEnd().FromXml<ListResponse>();
                }
            }
        }

        /// <summary>
        /// Uploads a map to the repository
        /// </summary>
        /// <param name="FullFileName">File name</param>
        /// <returns>API "add" result</returns>
        public static UploadResponse AddMap(string FullFileName)
        {
            using (var FS = File.OpenRead(FullFileName))
            {
                //Detect if compressed already
                byte[] GZ = new byte[2];
                FS.Read(GZ, 0, 2);
                FS.Position = 0;
                var Compress = GZ[0] != 0x1F || GZ[1] != 0x8B;
                if (Compress)
                {
                    FullFileName = Path.ChangeExtension(FullFileName, ".gz");
                }
                return AddMap(FS, FullFileName, Compress);
            }
        }

        /// <summary>
        /// Uploads a map to the repository
        /// </summary>
        /// <param name="MapData">Map data stream</param>
        /// <param name="OriginalFileName">File name</param>
        /// <param name="Compress">Compress before upload. Do not enable if already compressed</param>
        /// <returns>API "add" result</returns>
        public static UploadResponse AddMap(Stream MapData, string OriginalFileName, bool Compress)
        {
            if (MapData == null)
            {
                throw new ArgumentNullException(nameof(MapData));
            }
            if (!MapData.CanRead)
            {
                throw new ArgumentException("Stream must be readable", nameof(MapData));
            }
            if (string.IsNullOrEmpty(OriginalFileName))
            {
                throw new ArgumentNullException(nameof(OriginalFileName));
            }
            OriginalFileName = OriginalFileName.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Last();
            if (string.IsNullOrEmpty(OriginalFileName))
            {
                throw new ArgumentException("Invalid file name", nameof(OriginalFileName));
            }
            Stream S = MapData;
            if (Compress)
            {
                S = new MemoryStream();
                using (var GZS = new GZipStream(S, CompressionLevel.Optimal, true))
                {
                    MapData.CopyTo(GZS, 1 << 16);
                    GZS.Flush();
                }
                S.Position = 0;
            }
            try
            {
                using (HttpClient C = new HttpClient())
                {
                    var Values = new MultipartFormDataContent();
                    Values.Add(new StringContent(ApiKey.ToString()), "key");
                    Values.Add(new StreamContent(S), "map", OriginalFileName);
                    using (var Result = C.PostAsync(API_API + "/add", Values).Result)
                    {
                        return Result.Content.ReadAsStringAsync().Result.FromXml<UploadResponse>();
                    }
                }
            }
            finally
            {
                if (Compress)
                {
                    S.Dispose();
                }
            }
        }
    }
}
