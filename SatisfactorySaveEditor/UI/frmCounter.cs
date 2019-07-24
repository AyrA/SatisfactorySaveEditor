using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmCounter : Form
    {
        private SaveFileEntry[] Entries;

        public frmCounter(IEnumerable<SaveFileEntry> Entries)
        {
            InitializeComponent();

            Tools.SetupEscHandler(this);

            this.Entries = Entries.ToArray();

            foreach (var E in this.Entries.GroupBy(m => m.ObjectData.Name).OrderByDescending(m => m.Count()))
            {
                var SN = new ShortName(E.Key);
                var Item = lvCount.Items.Add(SN.Short);
                Item.Tag = SN.Long;
                Item.SubItems.Add(E.Count().ToString());
            }
            lvCount.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void lvCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var Selected = lvCount.SelectedItems
                    .OfType<ListViewItem>()
                    .SelectMany(m => Entries.Where(n => n.ObjectData.Name == m.Tag.ToString()));
                MapRender.MapForm.BackgroundImage.Dispose();
                MapRender.MapForm.BackgroundImage = MapRender.RenderEntries(Selected);
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
    }
}
