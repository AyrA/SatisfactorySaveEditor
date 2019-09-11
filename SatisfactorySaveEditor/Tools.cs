using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Tools to make your life easier
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Global random number generator
        /// </summary>
        private static Random R = new Random();

        /// <summary>
        /// Dimensions to map game vectors to relative coordinates (0.0 - 1.0)
        /// </summary>
        public struct VectorDimensions
        {
            //NOTE: ONLY GUESSES, FEEL FREE TO IMPROVE

            /// <summary>
            /// Coordinate that is at the left border of the map image
            /// </summary>
            public const int MIN_X = -325000;
            /// <summary>
            /// Coordinate that is at the right border of the map image
            /// </summary>
            public const int MAX_X = 428000;
            /// <summary>
            /// Coordinate that is at the top border of the map image
            /// </summary>
            public const int MIN_Y = -370000;
            /// <summary>
            /// Coordinate that is at the bottom border of the map image
            /// </summary>
            public const int MAX_Y = 375000;
            /// <summary>
            /// Total width of the map
            /// </summary>
            public const int SIZE_X = MAX_X - MIN_X;
            /// <summary>
            /// Total height of the map
            /// </summary>
            public const int SIZE_Y = MAX_Y - MIN_Y;
        }

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
            //Map dimensions
            Rectangle R = new Rectangle(VectorDimensions.MIN_X, VectorDimensions.MIN_Y, VectorDimensions.SIZE_X, VectorDimensions.SIZE_Y);
            var X = Math.Min(1f, Math.Max(0f, (V.X - R.X) / R.Width));
            var Y = Math.Min(1f, Math.Max(0f, (V.Y - R.Y) / R.Height));
            return new PointF(X, Y);
        }

        /// <summary>
        /// Translates relative map coordinates to game coordinates
        /// </summary>
        /// <param name="Source">Map coordinate</param>
        /// <returns>Game coordinate</returns>
        public static Vector3 TranslateToMap(PointF Source)
        {
            //Map dimensions
            var OffsetX = Source.X * (VectorDimensions.SIZE_X);
            var OffsetY = Source.Y * (VectorDimensions.SIZE_Y);
            return new Vector3(OffsetX + VectorDimensions.MIN_X, OffsetY + VectorDimensions.MIN_Y, 0);
        }

        /// <summary>
        /// Generates a random number inside of a range
        /// </summary>
        /// <param name="Low">Low end (inclusive)</param>
        /// <param name="High">Upper end (exclusive)</param>
        /// <returns>
        /// Number that satisfies <paramref name="Low"/>&lt;=X&lt;<paramref name="High"/>
        /// </returns>
        public static int GetRandom(int Low, int High)
        {
            return R.Next(Low, High);
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
            Log.Write("Attempted to get non-existing resource: {0}", ResourceName);
            return null;
        }

        /// <summary>
        /// Gets all files associated with version information
        /// </summary>
        /// <returns>Version information list</returns>
        public static string[] GetVersionFiles()
        {
            var EA = Assembly.GetExecutingAssembly();
            return EA.GetManifestResourceNames()
                .Where(m => m.StartsWith("SatisfactorySaveEditor.Changelog."))
                .Select(m => Regex.Match(m, @"v\d+\.\d+").Value)
                .ToArray();
        }

        /// <summary>
        /// Gets a specific version information file
        /// </summary>
        /// <param name="VersionFileName">Version file name</param>
        /// <returns>Version information</returns>
        public static string GetVersionFile(string VersionFileName)
        {
            return Encoding.UTF8.GetString(GetResource($"SatisfactorySaveEditor.Changelog.{VersionFileName}.txt"));
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
                Log.Write("Tools: Initializing map from resource");
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
            try
            {
                Log.Write("Tools: Requesting image resize W={0}->{2} H={1}->{3}", I.Width, I.Height, MaxWidth, MaxHeight);
                var S = new SizeF(I.Size);
                var Factor = Math.Min(MaxWidth / S.Width, MaxHeight / S.Height);
                var NewSize = new Size((int)(S.Width * Factor), (int)(S.Height * Factor));
                return new Bitmap(I, NewSize);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
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
            catch (Exception ex)
            {
                Log.Write(new IOException("Unable to rewind stream in Gzip test", ex));
            }
            if (R == 2)
            {
                return Data[0] == 0x1F && Data[1] == 0x8B;
            }
            return false;
        }

        /// <summary>
        /// Gets the SHA1 hash of the given data
        /// </summary>
        /// <param name="Data">Data to hash</param>
        /// <returns>Hash of data as hex string</returns>
        public static string GetHash(byte[] Data)
        {
            using (var MS = new MemoryStream(Data))
            {
                return GetHash(MS);
            }
        }

        /// <summary>
        /// Gets the hash of the given stream
        /// </summary>
        /// <param name="S">Stream</param>
        /// <returns>Hash of data as hex string</returns>
        /// <remarks>The <see cref="Stream"/> is not rewound or disposed</remarks>
        public static string GetHash(Stream S)
        {
            using (var H = SHA1.Create())
            {
                byte[] Data = H.ComputeHash(S);
                return string.Join("", Data.Select(m => m.ToString("X2")).ToArray());
            }
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
        /// Forces an integer into the given range
        /// </summary>
        /// <param name="Low">Lowest allowed value</param>
        /// <param name="Value">Supplied value</param>
        /// <param name="High">Highest allowed value</param>
        /// <returns>Value in range of <paramref name="Low"/> to <paramref name="High"/></returns>
        /// <remarks>
        /// If <paramref name="Low"/> is more than <paramref name="High"/>,
        /// <paramref name="High"/> is returned without evaluating <paramref name="Value"/>
        /// </remarks>
        public static int Range(int Low, int Value, int High)
        {
            if (High <= Low)
            {
                return High;
            }
            return Math.Max(Low, Math.Min(High, Value));
        }

        /// <summary>
        /// Shows a generic Error message box
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="Title">Box Title</param>
        /// <remarks>Box has OK button and Error icon</remarks>
        public static void E(string Text, string Title)
        {
            Log.Write("Show UI Error: {0}: {1}", Title, Text);
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
            Log.Write("Requesting help for {0}", FormName);
            var F = GetForm<frmHelp>();
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
            FeatureReport.Used(FeatureReport.Feature.HelpRequest);
        }

        /// <summary>
        /// Shortcut to convert byte arrays into UTF8 strings
        /// </summary>
        /// <param name="Data">Data</param>
        /// <returns>Text</returns>
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

        /// <summary>
        /// Sets up a handler to close the given form with [ESC]
        /// and to select text in textboxes with [CTRL]+[A]
        /// </summary>
        /// <param name="Source">Form</param>
        public static void SetupKeyHandlers(Form Source)
        {
            try
            {
                Source.KeyPreview = true;
                Source.KeyDown += delegate (object sender, KeyEventArgs e)
                {
                    if (e.KeyCode == Keys.Escape)
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        ((Form)sender).DialogResult = DialogResult.Cancel;
                        ((Form)sender).Close();
                    }
                };
                Log.Write("{0}: Registered [ESC] handler on {0}", Source.GetType().Name);
            }
            catch (Exception ex)
            {
                Log.Write(new Exception(string.Format("{0}: Unable to register [ESC] handler on form.", Source == null ? "<unknown>" : Source.GetType().Name), ex));
            }
            //Register CTRL+A handler on all text boxes

            try
            {
                var Controls = new Stack<Control>(Source.Controls.Cast<Control>());
                while (Controls.Count > 0)
                {
                    var C = Controls.Pop();
                    if (C.Controls != null)
                    {
                        foreach (var Child in C.Controls.Cast<Control>())
                        {
                            Controls.Push(Child);
                        }
                    }
                    if (C is TextBox)
                    {
                        C.KeyDown += delegate (object sender, KeyEventArgs e)
                        {
                            if (e.KeyCode == Keys.A && e.Control && !e.Shift && !e.Alt)
                            {
                                e.Handled = true;
                                e.SuppressKeyPress = true;
                                ((TextBox)sender).SelectAll();
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(new Exception("Unable to register CTRL+A handler", ex));
            }
        }

        /// <summary>
        /// Gets the first open form of the given type
        /// </summary>
        /// <typeparam name="T">Type of form</typeparam>
        /// <returns><see cref="Form"/>, or <see cref="null"/> if not found</returns>
        public static T GetForm<T>() where T : Form
        {
            return Application.OpenForms.OfType<T>().FirstOrDefault();
        }

        public static bool IsMatch(string Str, string Expression)
        {
            return Regex.IsMatch(Str, Expression);
        }

        public static Match[] Matches(string Str, string Expression)
        {
            return Regex.Matches(Str, Expression).OfType<Match>().ToArray();
        }
    }
}
