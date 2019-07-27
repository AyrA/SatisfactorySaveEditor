using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmDeleter : Form
    {
        private bool HasChange = false;
        private SaveFile F;

        public frmDeleter(SaveFile SaveGame)
        {
            F = SaveGame;
            InitializeComponent();
            MaximumSize = new Size(int.MaxValue, MinimumSize.Height);

            initItemList();
            Tools.SetupEscHandler(this);
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
            if (cbItem.Items.Count > 0)
            {
                //Don't select beyond the list
                cbItem.SelectedIndex = Math.Min(LastItem, cbItem.Items.Count - 1);
            }
            Log.Write("{0}: List initialized with {1} entries", GetType().Name, cbItem.Items.Count);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var Entries = F.Entries.Where(m => m.ObjectData.Name == ((ShortName)cbItem.SelectedItem).Long);

            if (rbRange.Checked)
            {
                Entries = Entries.Skip((int)nudStart.Value - 1).Take((int)nudCount.Value);
            }

            if (MessageBox.Show(rbAllItems.Checked ? $"Really delete this entry ({Entries.Count()} occurences)?" : $"Really delete {nudCount.Value} instances of this entry?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Log.Write("{0}: Deleting {1} entries", GetType().Name, Entries.Count());
                foreach (var E in Entries.ToArray())
                {
                    F.Entries.Remove(E);
                }
                MessageBox.Show($"Done", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                HasChange = true;
                //Reload list if last item taken, otherwise just update the total
                var Leftover = F.Entries.Count(m => m.ObjectData.Name == ((ShortName)cbItem.SelectedItem).Long);
                if (Leftover == 0)
                {
                    initItemList();
                }
                else
                {
                    //Clamp count if needed
                    nudCount.Value = Math.Min(nudCount.Value, Leftover);
                    nudCount.Maximum = Leftover;
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

        private void rbRange_CheckedChanged(object sender, EventArgs e)
        {
            nudStart.Enabled = nudCount.Enabled = rbRange.Checked;
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

        private void frmDeleter_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }
    }
}
