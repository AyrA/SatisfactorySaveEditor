using SatisfactorySaveEditor.ObjectTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Represents an object that is to draw on the map
    /// </summary>
    public struct DrawObject
    {
        /// <summary>
        /// Color to use
        /// </summary>
        public Color ObjectColor;
        /// <summary>
        /// Size (in pixels) of the object
        /// </summary>
        /// <remarks>This is used as width and height of the square that is plotted</remarks>
        public double ObjectSize;
        /// <summary>
        /// Relative (0-1) location on the map
        /// </summary>
        public PointF ObjectPosition;

        /// <summary>
        /// Creates a DrawObject from template data
        /// </summary>
        /// <param name="Entry">Game object</param>
        /// <param name="ItemColor">Color</param>
        /// <param name="ItemSize">Object size</param>
        public DrawObject(SaveFileEntry Entry, Color ItemColor = default(Color), double ItemSize = 0)
        {
            if (Entry.EntryType != OBJECT_TYPE.OBJECT)
            {
                throw new ArgumentException("Only game objects are supported as draw object");
            }
            var OD = (GameObject)Entry.ObjectData;
            ObjectPosition = Tools.TranslateFromMap(OD.ObjectPosition);
            ObjectColor = ItemColor;
            ObjectSize = ItemSize;
        }

        /// <summary>
        /// Gets the absolute drawing area of this object for the given image dimensions
        /// </summary>
        /// <param name="ScaleW">Image width</param>
        /// <param name="ScaleH">Image height</param>
        /// <returns>Absolute image coordinates</returns>
        public RectangleF GetRectangle(int ScaleW, int ScaleH)
        {
            return new RectangleF(
                (int)(ObjectPosition.X * ScaleW),
                (int)(ObjectPosition.Y * ScaleH),
                (float)ObjectSize,
                (float)ObjectSize);
        }
    }

    /// <summary>
    /// Provides features to render objects on the map
    /// </summary>
    public static class MapRender
    {
        public const int DEFAULT_WIDTH = 1000;
        public const int DEFAULT_HEIGHT = 1000;
        public const int ORIGINAL_WIDTH = -1;
        public const int ORIGINAL_HEIGHT = -1;

        /// <summary>
        /// Unaltered base image
        /// </summary>
        private static Image BaseImage = null;
        /// <summary>
        /// Resized base image
        /// </summary>
        private static Image ScaledImage = null;

        /// <summary>
        /// Form containing the map
        /// </summary>
        public static System.Windows.Forms.Form MapForm;

        /// <summary>
        /// Initializes the base image and resized copy
        /// </summary>
        /// <param name="MaxWidth">Maximum image width</param>
        /// <param name="MaxHeight">Maximum image height</param>
        /// <remarks>
        /// Will not do anything if already called once during the runtime.
        /// Calls <see cref="SetBaseSize(int, int)"/> to provide the resized option.
        /// </remarks>
        public static void Init(int MaxWidth = DEFAULT_WIDTH, int MaxHeight = DEFAULT_HEIGHT)
        {
            if (BaseImage == null)
            {
                Log.Write("Map: Initializing base image W={0},H={1}", MaxWidth, MaxHeight);
                //Load base image and provide initially scaled version
                using (var MS = new MemoryStream(Tools.GetMap()))
                {
                    BaseImage = Image.FromStream(MS);
                }
                SetBaseSize(MaxWidth, MaxHeight);
            }
        }

        /// <summary>
        /// Sets a new base size for the image
        /// </summary>
        /// <param name="MaxWidth">New width</param>
        /// <param name="MaxHeight">New height</param>
        /// <remarks>
        /// Image resizing is time consuming.
        /// Don't repeatedly call this in a resize handler or similar
        /// </remarks>
        public static void SetBaseSize(int MaxWidth, int MaxHeight)
        {
            if (BaseImage == null)
            {
                Init(MaxWidth, MaxHeight);
            }
            else
            {
                Log.Write("Map: Re-scale image W={0},H={1}", MaxWidth, MaxHeight);
                if (ScaledImage != null)
                {
                    ScaledImage.Dispose();
                }
                if (MaxWidth == ORIGINAL_WIDTH && MaxHeight == ORIGINAL_HEIGHT)
                {
                    ScaledImage = (Image)BaseImage.Clone();
                }
                else
                {
                    if (MaxWidth == ORIGINAL_WIDTH)
                    {
                        MaxWidth = BaseImage.Width;
                    }
                    if (MaxHeight == ORIGINAL_HEIGHT)
                    {
                        MaxHeight = BaseImage.Height;
                    }
                    ScaledImage = Tools.ResizeImage(BaseImage, MaxWidth, MaxHeight);
                }
            }
        }

        /// <summary>
        /// Gets the unaltered scaled map
        /// </summary>
        /// <returns>Map</returns>
        /// <remarks>It's the users responsibility to dispose the image</remarks>
        public static Image GetMap()
        {
            Init();
            return new Bitmap(ScaledImage);
        }

        /// <summary>
        /// Gets a map with the items rendered on it
        /// </summary>
        /// <param name="Objects">Items to render</param>
        /// <returns>Map</returns>
        /// <remarks>It's the users responsibility to dispose the image</remarks>
        public static Image Render(IEnumerable<DrawObject> Objects)
        {
            Log.Write("Map: Rendering {0} objects", Objects.Count());
            Init();
            var BMP = new Bitmap(ScaledImage);
            using (var G = Graphics.FromImage(BMP))
            {
                foreach (var Element in Objects)
                {
                    using (var B = new SolidBrush(Element.ObjectColor))
                    {
                        G.FillRectangle(B, Element.GetRectangle(BMP.Width, BMP.Height));
                    }
                }
            }

            return BMP;
        }

        /// <summary>
        /// Gets a map with the item rendered on it
        /// </summary>
        /// <param name="Object">Item to render</param>
        /// <returns>Map</returns>
        /// <remarks>It's the users responsibility to dispose the image</remarks>
        public static Image Render(DrawObject Object)
        {
            Init();
            return Render(new DrawObject[] { Object });
        }

        /// <summary>
        /// Renders the contents of a save file onto the map
        /// </summary>
        /// <param name="F">Save File</param>
        /// <returns>Map</returns>
        public static Image RenderFile(SaveFile F, double factor = 1.0)
        {
            Init();
            return RenderEntries(F.Entries, factor);
        }

        /// <summary>
        /// Renders the given save file entries to the map
        /// </summary>
        /// <param name="Entries">Save File Entries</param>
        /// <returns>Map</returns>
        public static Image RenderEntries(IEnumerable<SaveFileEntry> Entries, double factor = 1.0)
        {
            Init();
            var Objects = new List<DrawObject>();

            foreach (var P in Entries.Where(m => m.EntryType == OBJECT_TYPE.OBJECT))
            {
                var O = new DrawObject(P, Color.Green, 2 * factor);
                //Change color according to object type
                if (P.ObjectData.Name.Contains("Build"))
                {
                    O.ObjectColor = Color.Yellow;
                }
                if (P.ObjectData.Name.Contains("Node"))
                {
                    O.ObjectColor = Color.Fuchsia;
                }
                if (P.ObjectData.Name.Contains("Foundation"))
                {
                    O.ObjectColor = Color.DarkGray;
                }
                if (P.ObjectData.Name.Contains("Walkway"))
                {
                    O.ObjectColor = Color.White;
                }
                Objects.Add(O);
            }
            //Enumerate players seperately because we want them bigger
            foreach (var P in Entries.Where(m => m.ObjectData.Name == "/Game/FactoryGame/Character/Player/Char_Player.Char_Player_C"))
            {
                Objects.Add(new DrawObject(P, Color.Red, 10 * factor));
            }

            Log.Write("Map: Rendering {0} entries as {1} objects", Entries.Count(), Objects.Count);

            return Render(Objects);
        }
    }
}
