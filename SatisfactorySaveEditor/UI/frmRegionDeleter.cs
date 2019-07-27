using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmRegionDeleter : Form
    {
        /// <summary>
        /// Items with these name parts will be added to the remove list by default
        /// </summary>
        private const string WHITELIST = "/Buildable/|ResourceNode|ResourceDeposit";
        /// <summary>
        /// Items with these names override the <see cref="WHITELIST"/>
        /// </summary>
        private const string PROTECTED = "TradingPost|MamIntegrated|HubTerminal|WorkBenchIntegrated|StorageIntegrated|GeneratorIntegratedBiomass";

        private SaveFile F;
        private List<PointF> Points;
        private bool HasChange;

        public frmRegionDeleter(SaveFile F)
        {
            this.F = F;
            HasChange = false;
            Points = new List<PointF>();
            InitializeComponent();
            pbMap.Image = MapRender.RenderFile(F);
            Tools.SetupEscHandler(this);
        }

        private void frmRegionDeleter_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ShowHelp(GetType().Name);
        }

        private void pbMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Points.Add(new PointF(
                    e.Location.X / (float)pbMap.Width,
                    e.Location.Y / (float)pbMap.Height
                    ));
                Log.Write("{0}: Added point {1}", GetType().Name, Points.Last());
                RedrawMap(F.Entries);
            }
            if (e.Button == MouseButtons.Right && Points.Count > 0)
            {
                if (Points.Count > 3)
                {
                    Log.Write("{0}: Removed point {1}", GetType().Name, Points.Last());
                    Points.RemoveAt(Points.Count - 1);
                }
                else
                {
                    Log.Write("{0}: Removed all points", GetType().Name);
                    Points.Clear();
                }
                RedrawMap(F.Entries);
            }
        }

        private void newSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Points.Count > 0)
            {
                if (MessageBox.Show("Remove all points?", "Reset Map", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Log.Write("{0}: Cleared point list", GetType().Name);
                    Points.Clear();
                    RedrawMap(F.Entries);
                }
            }
        }

        private void deleteObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var WLItems = WHITELIST.Split('|');
            var ProtectedItems = PROTECTED.Split('|');

            var A = Points.ToArray();
            var ZeroPos = new Vector3();
            //Get all items inside the polygon
            var Matches = F.Entries.Where(m => m.EntryType == ObjectTypes.OBJECT_TYPE.OBJECT &&
                !((ObjectTypes.GameObject)m.ObjectData).ObjectPosition.Equals(ZeroPos) &&
                IsInside(Tools.TranslateFromMap(((ObjectTypes.GameObject)m.ObjectData).ObjectPosition), A)
            ).ToArray();
            Log.Write("{0}: Items to potentially delete: {1}", GetType().Name, Matches.Length);
            RedrawMap(Matches);
            //Filter Entries
            var WL = Matches
                .Where(m =>
                    WLItems.Any(n => m.ObjectData.Name.Contains(n)) &&
                    !ProtectedItems.Any(n => m.ObjectData.Name.Contains(n))
                ).ToArray();
            Log.Write("{0}: Whitelist contains {1} items", GetType().Name, WL.Length);
            if (Matches.Length > 0)
            {
                using (var Confirm = new frmElementList(WL, Matches.Where(m => !WL.Contains(m))))
                {
                    if (Confirm.ShowDialog() == DialogResult.OK && Confirm.ItemsToProcess.Length > 0)
                    {
                        var ToRemove = Matches.Where(m => Confirm.ItemsToProcess.Contains(m.ObjectData.Name)).ToArray();
                        F.Entries.RemoveAll(m => ToRemove.Contains(m));
                        Log.Write("{0}: Removed {1} objects", GetType().Name, ToRemove.Length);
                        MessageBox.Show($"Removed objects: {ToRemove.Length}", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        HasChange |= ToRemove.Length > 0;
                    }
                    else
                    {
                        Log.Write("{0}: User cancelled delete operation", GetType().Name);
                        MessageBox.Show("Operation cancelled", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                Log.Write("{0}: User selected empty region", GetType().Name);
                MessageBox.Show("The selected region contains no objects.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            RedrawMap(F.Entries);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RedrawMap(IEnumerable<SaveFileEntry> Entries)
        {
            Log.Write("{0}: Redrawing the map. Points: {1}", GetType().Name, Points.Count);
            var I = MapRender.RenderEntries(Entries);
            using (var G = Graphics.FromImage(I))
            {
                //Render points
                if (Points.Count > 2)
                {
                    using (var B = new SolidBrush(Color.FromArgb(100, Color.Lime)))
                    {
                        G.FillPolygon(B, Points
                            .Select(m => new Point((int)(m.X * I.Width), (int)(m.Y * I.Height)))
                            .ToArray());
                    }
                }
                using (var F = new Font("Arial", 16))
                {
                    G.DrawString($"Number of points: {Points.Count}", F, Brushes.Red, new PointF(0, 0));
                }
            }
            if (pbMap.Image != null)
            {
                pbMap.Image.Dispose();
            }
            pbMap.Image = I;
            Log.Write("{0}: Redrawing complete", GetType().Name);
        }

        /// <summary>
        /// Checks if the given point is inside of the given polygon
        /// </summary>
        /// <param name="Query">Point to check</param>
        /// <param name="Polygon">Polygon list</param>
        /// <returns>true, if inside</returns>
        /// <remarks>This handles intersecting polygons properly</remarks>
        /// <seealso cref="https://stackoverflow.com/a/14998816"/>
        private bool IsInside(PointF Query, PointF[] Polygon)
        {
            bool result = false;
            int j = Polygon.Count() - 1;
            for (int i = 0; i < Polygon.Length; i++)
            {
                if (Polygon[i].Y < Query.Y && Polygon[j].Y >= Query.Y || Polygon[j].Y < Query.Y && Polygon[i].Y >= Query.Y)
                {
                    if (Polygon[i].X + (Query.Y - Polygon[i].Y) / (Polygon[j].Y - Polygon[i].Y) * (Polygon[j].X - Polygon[i].X) < Query.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        private void frmRegionDeleter_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = HasChange ? DialogResult.OK : DialogResult.Cancel;
        }
    }
}
