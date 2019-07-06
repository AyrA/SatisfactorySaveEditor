using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SatisfactorySaveEditor
{
    public partial class frmExport : Form
    {
        private bool HasChange = false;
        private SaveFile F;

        public frmExport(SaveFile F)
        {
            this.F = F;
            InitializeComponent();
            MaximumSize = new Size(int.MaxValue, MinimumSize.Height);

            initItemList();
        }
        private void initItemList()
        {
            var LastItem = cbItem.Items.Count > 0 ? cbItem.SelectedIndex : 0;
            cbItem.Items.Clear();
            cbItem.Items.AddRange(F.Entries
                .Select(m => m.ObjectData.Name)
                .Distinct()
                .Select(m => new ShortName(m))
                .OrderBy(m => m)
                .Cast<object>()
                .ToArray());
            cbItem.SelectedIndex = LastItem;
        }

        private void cbItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbItem.SelectedIndex >= 0)
            {
                var ItemName = ((ShortName)cbItem.SelectedItem).Long;
                var Count = F.Entries.Count(m => m.ObjectData.Name == ItemName);
                nudStart.Value = 1;
                nudStart.Maximum = Count;
                nudCount.Value = 1;
                nudCount.Maximum = Count;
                var Pos = rbAllItems.Location;
                rbAllItems.Text = $"All items (total: {Count})";
                rbAllItems.Location = Pos;
            }
        }

        private void btnMap_Click(object sender, EventArgs e)
        {
            var ItemName = ((ShortName)cbItem.SelectedItem).Long;
            var Items = F.Entries.Where(m => m.ObjectData.Name == ItemName);
            //Filter if needed
            if (rbRange.Checked)
            {
                Items = Items.Skip((int)nudStart.Value - 1).Take((int)nudCount.Value);
            }
            if (Items.Count() > 0)
            {
                if (Items.First().EntryType == ObjectTypes.OBJECT_TYPE.OBJECT)
                {
                    MapRender.MapForm.BackgroundImage.Dispose();
                    MapRender.MapForm.BackgroundImage = MapRender.Render(Items.Select(m => new DrawObject(m, Color.Yellow, 10)));
                }
                else
                {
                    MessageBox.Show("This type of entry has no map coordinates", "Invalid entry type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No items to show (0 items selected)", "Invalid entry type", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var ItemName = ((ShortName)cbItem.SelectedItem).Long;
            var Items = F.Entries.Where(m => m.ObjectData.Name == ItemName);
            //Filter if needed
            if (rbRange.Checked)
            {
                Items = Items.Skip((int)nudStart.Value - 1).Take((int)nudCount.Value);
            }
            if (Items.Count() > 0)
            {
                var Ser = new XmlSerializer(typeof(SaveFileEntry[]));
                Clipboard.Clear();
                using (var SW = new StringWriter())
                {
                    Ser.Serialize(SW, Items.ToArray());
                    Clipboard.SetText(SW.ToString());
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (HasChange)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
            Close();
        }
    }
}
