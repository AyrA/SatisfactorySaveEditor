using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmCounter : Form
    {
        private SaveFileEntry[] AllEntries;
        private SaveFileEntry[] MapEntries;
        private bool HiddenElements = false;

        public frmCounter(IEnumerable<SaveFileEntry> Entries)
        {
            var Blank = new Vector3(0, 0, 0);
            InitializeComponent();
            Tools.SetupKeyHandlers(this);

            AllEntries = Entries.ToArray();
            MapEntries = AllEntries
                .Where(m => m.EntryType == ObjectTypes.OBJECT_TYPE.OBJECT && !((ObjectTypes.GameObject)m.ObjectData).ObjectPosition.Equals(Blank))
                .ToArray();

            RenderList(AllEntries);
        }

        private void RenderList(SaveFileEntry[] Entries)
        {
            lvCount.Items.Clear();
            foreach (var E in Entries.GroupBy(m => m.ObjectData.Name).OrderByDescending(m => m.Count()))
            {
                var SN = new ShortName(E.Key);
                var Item = lvCount.Items.Add(SN.Short);
                Item.UseItemStyleForSubItems = true;
                Item.Tag = SN.Long;
                Item.SubItems.Add(E.Count().ToString());
                if (!MapEntries.Contains(E.First()))
                {
                    Item.BackColor = Color.FromKnownColor(KnownColor.Control);
                    Item.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                }
            }
            Text = $"Object Counter ({Entries.Length} Objects)";
            lvCount.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void RenderSelected()
        {
            if (lvCount.SelectedItems.Count > 0)
            {
                var Blank = new Vector3(0, 0, 0);
                var Selected = lvCount.SelectedItems
                    .OfType<ListViewItem>()
                    .SelectMany(m => MapEntries.Where(n => n.ObjectData.Name == m.Tag.ToString()))
                    .Select(m => new DrawObject(m, Color.Fuchsia, 3))
                    .ToArray();
                if (Selected.Length > 0)
                {
                    MapRender.MapForm.BackgroundImage.Dispose();
                    MapRender.MapForm.BackgroundImage = MapRender.Render(Selected);
                }
                else
                {
                    MessageBox.Show("The selected objects don't have map coordinates.", "No objects to render");
                }
            }
        }

        private void lvCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RenderSelected();
            }
            if (e.KeyCode == Keys.A && e.Control)
            {
                lvCount.Items.OfType<ListViewItem>().All(m => m.Selected = true);
            }
        }

        private void frmCounter_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(nameof(frmCounter));
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            if (HiddenElements)
            {
                HiddenElements = false;
                RenderList(AllEntries);
                btnHide.Text = "Hide unpositioned objects";
            }
            else
            {
                HiddenElements = true;
                RenderList(MapEntries);
                btnHide.Text = "Show unpositioned objects";
            }
        }

        private void lvCount_DoubleClick(object sender, EventArgs e)
        {
            RenderSelected();
        }
    }
}
