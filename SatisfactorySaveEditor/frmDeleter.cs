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
        }

        private void initItemList()
        {
            var LastItem = cbItem.Items.Count > 0 ? cbItem.SelectedIndex : 0;
            cbItem.Items.Clear();
            cbItem.Items.AddRange(F.Entries.Select(m => m.ObjectData.Name).Distinct().OrderBy(m => m).Cast<object>().ToArray());
            cbItem.SelectedIndex = LastItem;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var Entries = F.Entries.Where(m => m.ObjectData.Name == cbItem.SelectedItem.ToString());

            if (rbRange.Checked)
            {
                Entries = Entries.Skip((int)nudStart.Value).Take((int)nudCount.Value);
            }

            if (MessageBox.Show(rbAllItems.Checked ? $"Really delete this entry ({Entries.Count()} occurences)?" : $"Really delete {nudCount.Value} instances of this entry?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (var E in Entries.ToArray())
                {
                    F.Entries.Remove(E);
                }
                MessageBox.Show($"Done", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                HasChange = true;
                //Reload list if last item taken, otherwise just update the total
                var Leftover = F.Entries.Count(m => m.ObjectData.Name == cbItem.SelectedItem.ToString());
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
                var ItemName = cbItem.SelectedItem.ToString();
                var Count = F.Entries.Count(m => m.ObjectData.Name == ItemName);
                nudStart.Value = 0;
                nudStart.Maximum = Count - 1;
                nudCount.Value = 1;
                nudCount.Maximum = Count;
            }
        }

        private void rbRange_CheckedChanged(object sender, EventArgs e)
        {
            nudStart.Enabled = nudCount.Enabled = rbRange.Checked;
        }
    }
}
