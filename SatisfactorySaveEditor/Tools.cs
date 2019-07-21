using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Tools to make your life easier
    /// </summary>
    public static class Tools
    {

        /// <summary>
        /// Gets the current version without depending on the System.Windows.Forms namespace
        /// </summary>
        public static Version CurrentVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        /// <summary>
        /// Map data cache
        /// </summary>
        private static byte[] MapData = null;

        /// <summary>
        /// Creates a console window
        /// </summary>
        /// <returns>true, if window was created</returns>
        /// <remarks>if this returns false, you likely already have a window</remarks>
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        /// <summary>
        /// Removes the console window handle from this application.
        /// Also removes the window itself if this was the last handle.
        /// </summary>
        /// <returns>true, if removed</returns>
        /// <remarks>
        /// If this returns false, there is likely no window present.
        /// Releasing the window will stop blocking scripts that use this application
        /// </remarks>
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        /// <summary>
        /// Reads the weirdly encoded strings
        /// </summary>
        /// <param name="BR">Open Reader</param>
        /// <returns>string</returns>
        public static string ReadIntString(this BinaryReader BR)
        {
            byte[] Data = BR.ReadBytes(BR.ReadInt32());
            if (Data.Length > 0)
            {
                //Strings are length prefixed AND null terminated in this file format for some reason
                //We remove the null termination to make editing easier
                return Encoding.UTF8.GetString(Data).TrimEnd('\0');
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes weirdly encoded strings to a stream
        /// </summary>
        /// <param name="BW">Open Writer</param>
        /// <param name="S">String</param>
        public static void WriteIntString(this BinaryWriter BW, string S)
        {
            //Add null termination back if needed
            if (string.IsNullOrEmpty(S))
            {
                S = "\0";
            }
            if (!S.EndsWith("\0"))
            {
                S += '\0';
            }
            byte[] Data = Encoding.UTF8.GetBytes(S);
            BW.Write(Data.Length);
            BW.Write(Data);
        }

        /// <summary>
        /// Translates game coordinates to relative map coordinates for the image system
        /// </summary>
        /// <param name="V">Map coordinates</param>
        /// <returns>Image coordinates (relative, 0-1)</returns>
        public static PointF TranslateFromMap(Vector3 V)
        {
            //NOTE: ONLY A GUESS, FEEL FREE TO IMPROVE
            const int MIN_X = -330000;
            const int MAX_X = 428000;
            const int MIN_Y = -370000;
            const int MAX_Y = 370000;
            const int SIZE_X = MAX_X - MIN_X;
            const int SIZE_Y = MAX_Y - MIN_Y;
            //Map dimensions
            Rectangle R = new Rectangle(MIN_X, MIN_Y, SIZE_X, SIZE_Y);
            var X = Math.Min(1f, Math.Max(0f, (V.X - R.X) / R.Width));
            var Y = Math.Min(1f, Math.Max(0f, (V.Y - R.Y) / R.Height));
            return new PointF(X, Y);
        }

        /// <summary>
        /// Copies a file to a new location and compresses it
        /// </summary>
        /// <param name="InName">Original file</param>
        /// <param name="OutName">New file</param>
        /// <returns>true, if copied and compressed sucessfully</returns>
        /// <remarks>
        /// The user must clean up the file referenced by <paramref name="OutName"/> if the function fails.
        /// </remarks>
        public static bool Compress(string InName, string OutName)
        {
            try
            {
                using (var IN = File.OpenRead(InName))
                {
                    using (var OUT = File.Create(OutName))
                    {
                        using (var GZS = new GZipStream(OUT, CompressionLevel.Optimal))
                        {
                            IN.CopyTo(GZS);
                        }
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
        /// Gets an embedded resource from the application
        /// </summary>
        /// <param name="ResourceName">Full resource name</param>
        /// <returns>Resource content</returns>
        public static byte[] GetResource(string ResourceName)
        {
            var EA = Assembly.GetExecutingAssembly();
            if (EA.GetManifestResourceNames().Contains(ResourceName))
            {
                using (var S = EA.GetManifestResourceStream(ResourceName))
                {
                    using (var MS = new MemoryStream())
                    {
                        S.CopyTo(MS);
                        return MS.ToArray();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get the map data from the embedded resource stream
        /// </summary>
        /// <remarks>https://redd.it/bk6lnk</remarks>
        /// <returns>Map data</returns>
        public static byte[] GetMap()
        {
            if (MapData == null)
            {
                MapData = GetResource("SatisfactorySaveEditor.Images.Map.png");
            }
            return (byte[])MapData.Clone();
        }

        /// <summary>
        /// Resizes an image to the given maximum dimensions,
        /// respecting the aspect ratio of said image
        /// </summary>
        /// <param name="I">Source image</param>
        /// <param name="MaxWidth">New maximum width</param>
        /// <param name="MaxHeight">New maximum height</param>
        /// <returns>Scaled image</returns>
        /// <remarks>This will not dispose either of the images</remarks>
        public static Image ResizeImage(Image I, int MaxWidth, int MaxHeight)
        {
            var S = new SizeF(I.Size);
            var Factor = Math.Min(MaxWidth / S.Width, MaxHeight / S.Height);
            var NewSize = new Size((int)(S.Width * Factor), (int)(S.Height * Factor));
            return new Bitmap(I, NewSize);
        }

        /// <summary>
        /// Generate a Hex dump from binary data that looks like it would in a hex editor.
        /// Can be used to dump contents to the console when debugging
        /// </summary>
        /// <param name="Data">Binary data</param>
        /// <param name="Width">Hex Width in bytes</param>
        /// <param name="ASCII">Show textual representation too</param>
        /// <returns>Hex dump</returns>
        public static string HexDump(byte[] Data, int Width = 16, bool ASCII = true)
        {
            var SB = new StringBuilder();
            for (var i = 0; i < Data.Length; i += Width)
            {
                var Segment = Data.Skip(i).Take(Width).ToArray();
                for (var j = 0; j < Width; j++)
                {
                    if (j < Segment.Length)
                    {
                        SB.Append(Segment[j].ToString("X2") + " ");
                    }
                    else
                    {
                        SB.Append("   ");
                    }
                }
                if (ASCII)
                {
                    SB.AppendLine("\t" + Encoding.ASCII.GetString(Segment.Select(m => m > 0x1F && m < 0x7F ? m : (byte)'.').ToArray()));
                }
                else
                {
                    SB.AppendLine();
                }
            }
            return SB.ToString();
        }

        /// <summary>
        /// Checks if the given file header indicates gzip
        /// </summary>
        /// <param name="FS">possible compressed stream</param>
        /// <returns></returns>
        /// <remarks>will not try to decompress anything. Will try to seek back the number of bytes read</remarks>
        public static bool IsGzFile(Stream FS)
        {
            //Null is not a gzip stream
            if (FS == null)
            {
                return false;
            }
            //A gzip stream is a gzip stream. Who would have guessed?
            if (FS is GZipStream)
            {
                return true;
            }

            //GZip header has two magic bytes
            byte[] Data = new byte[2];
            int R = FS.Read(Data, 0, 2);
            try
            {
                //Try seeking back but don't care if we can't
                FS.Seek(-R, SeekOrigin.Current);
            }
            catch
            {

            }
            if (R == 2)
            {
                return Data[0] == 0x1F && Data[1] == 0x8B;
            }
            return false;
        }

        /// <summary>
        /// Changes a file name until it's unique on the file system
        /// </summary>
        /// <param name="ExistingFileName">Existing file name</param>
        /// <returns>New file name</returns>
        /// <remarks>
        /// If the existing name doesn't points to an existing file,
        /// the unchanged parameter is returned.
        /// </remarks>
        public static string GetNewName(string ExistingFileName)
        {
            if (string.IsNullOrWhiteSpace(ExistingFileName))
            {
                return Path.GetTempFileName();
            }
            var C = 1;
            //Get path, base name and extension
            var P = Path.GetDirectoryName(ExistingFileName);
            var B = Path.GetFileNameWithoutExtension(ExistingFileName);
            var E = Path.GetExtension(ExistingFileName);
            var TempName = ExistingFileName;
            while (File.Exists(TempName))
            {
                TempName = Path.Combine(P, $"{B}_{C++}");
                if (!string.IsNullOrEmpty(E))
                {
                    TempName += E;
                }
            }
            return TempName;
        }

        /// <summary>
        /// Shows a generic Error message box
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="Title">Box Title</param>
        /// <remarks>Box has OK button and Error icon</remarks>
        public static void E(string Text, string Title)
        {
            System.Windows.Forms.MessageBox.Show(
                Text,
                Title,
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows help for the current form
        /// </summary>
        /// <param name="FormName">Name of the form that requested the help</param>
        public static void ShowHelp(string FormName)
        {
            var F = System.Windows.Forms.Application.OpenForms
                .OfType<frmHelp>()
                .FirstOrDefault();
            //Show new form if not already there
            if (F == null)
            {
                F = new frmHelp();
                F.Show();
            }
            else
            {
                //Bring existing content to the front
                F.BringToFront();
                F.Focus();
            }

            string HelpText = ToString(GetResource("SatisfactorySaveEditor.Help._no.txt"));
            try
            {
                F.HelpText = ToString(GetResource($"SatisfactorySaveEditor.Help.{FormName}.txt"));
            }
            catch
            {
                F.HelpText = string.Format(HelpText, $"SatisfactorySaveEditor.Help.{FormName}.txt");
            }
        }

        private static string ToString(byte[] Data)
        {
            if (Data == null)
            {
                throw new ArgumentNullException(nameof(Data));
            }
            if (Data.Length == 0)
            {
                return string.Empty;
            }
            return Encoding.UTF8.GetString(Data);
        }
    }
}
