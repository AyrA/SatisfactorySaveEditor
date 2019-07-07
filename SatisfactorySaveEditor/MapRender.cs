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
        public int ObjectSize;
        /// <summary>
        /// Relative (0-1) location on the map
        /// </summary>
        public PointF ObjectPosition;

        /// <summary>
        /// Creates a DrawObject from template data
        /// </summary>
        /// <param name="E">Game object</param>
        /// <param name="C">Color</param>
        /// <param name="S">Object size</param>
        public DrawObject(SaveFileEntry E, Color C = default(Color), int S = 0)
        {
            if (E.EntryType != OBJECT_TYPE.OBJECT)
            {
                throw new ArgumentException("Only game objects are supported as draw object");
            }
            var OD = (GameObject)E.ObjectData;
            ObjectPosition = Tools.TranslateFromMap(OD.ObjectPosition);
            ObjectColor = C;
            ObjectSize = S;
        }

        /// <summary>
        /// Gets the absolute drawing area of this object for the given image dimensions
        /// </summary>
        /// <param name="ScaleW">Image width</param>
        /// <param name="ScaleH">Image height</param>
        /// <returns>Absolute image coordinates</returns>
        public Rectangle GetRectangle(int ScaleW, int ScaleH)
        {
            return new Rectangle(
                (int)(ObjectPosition.X * ScaleW),
                (int)(ObjectPosition.Y * ScaleH),
                ObjectSize,
                ObjectSize);
        }
    }

    /// <summary>
    /// Provides features to render objects on the map
    /// </summary>
    public static class MapRender
    {
        /// <summary>
        /// Unaltered base image
        /// </summary>
        private static Image BaseImage;
        /// <summary>
        /// Resized base image
        /// </summary>
        private static Image ScaledImage;

        /// <summary>
        /// Form containing the map
        /// </summary>
        public static System.Windows.Forms.Form MapForm;

        /// <summary>
        /// Static initializer
        /// </summary>
        static MapRender()
        {
            //Load base image and provide initially scaled version
            using (var MS = new MemoryStream(Tools.GetMap()))
            {
                BaseImage = Image.FromStream(MS);
            }
            ScaledImage = Tools.ResizeImage(BaseImage, 1024, 1024);
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
            ScaledImage.Dispose();
            ScaledImage = Tools.ResizeImage(BaseImage, MaxWidth, MaxHeight);
        }

        /// <summary>
        /// Gets the unaltered scaled map
        /// </summary>
        /// <returns>Map</returns>
        /// <remarks>It's the users responsibility to dispose the image</remarks>
        public static Image GetMap()
        {
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
            return Render(new DrawObject[] { Object });
        }

        /// <summary>
        /// Renders the contents of a save file onto the map
        /// </summary>
        /// <param name="F">Save File</param>
        /// <returns>Map</returns>
        public static Image RenderFile(SaveFile F)
        {
            var Objects = new List<DrawObject>();

            foreach (var P in F.Entries.Where(m => m.EntryType == OBJECT_TYPE.OBJECT))
            {
                var O = new DrawObject(P, Color.Green, 2);
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
            foreach (var P in F.Entries.Where(m => m.ObjectData.Name == "/Game/FactoryGame/Character/Player/Char_Player.Char_Player_C"))
            {
                Objects.Add(new DrawObject(P, Color.Red, 10));
            }

            return Render(Objects);
        }
    }
}
