using SatisfactorySaveEditor.ObjectTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SatisfactorySaveEditor
{
    public struct DrawObject
    {
        public Color ObjectColor;
        public int ObjectSize;
        public PointF ObjectPosition;

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

        public Rectangle GetRectangle(int ScaleW, int ScaleH)
        {
            return new Rectangle(
                (int)(ObjectPosition.X * ScaleW),
                (int)(ObjectPosition.Y * ScaleH),
                ObjectSize,
                ObjectSize);
        }
    }

    public static class MapRender
    {
        private static Image BaseImage;
        private static Image ScaledImage;

        static MapRender()
        {
            using (var MS = new MemoryStream(Tools.GetMap()))
            {
                BaseImage = Image.FromStream(MS);
            }
            ScaledImage = Tools.ResizeImage(BaseImage, 1024, 1024);
        }

        public static void SetBaseSize(int MaxWidth, int MaxHeight)
        {
            ScaledImage.Dispose();
            ScaledImage = Tools.ResizeImage(BaseImage, MaxWidth, MaxHeight);
        }

        public static Image GetMap()
        {
            return new Bitmap(ScaledImage);
        }

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
    }
}
