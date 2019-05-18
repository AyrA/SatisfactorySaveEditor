using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmDuplicator : Form
    {
        private SaveFile F;
        private bool HasChange = false;

        public frmDuplicator(SaveFile SaveGame)
        {
            InitializeComponent();
            //Make width infinitely resizable only
            MaximumSize = new Size(int.MaxValue, MinimumSize.Height);
            F = SaveGame;
            cbObject.Items.AddRange(F.Entries.Select(m => m.ObjectData.Name).Distinct().OrderBy(m => m).Cast<object>().ToArray());
            cbObject.SelectedIndex = 0;
        }

        private void cbObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbObject.SelectedIndex >= 0)
            {
                var ItemName = cbObject.SelectedItem.ToString();
                var Count = F.Entries.Count(m => m.ObjectData.Name == ItemName);
                nudOffset.Value = 1;
                nudOffset.Maximum = Count;
                nudCount.Value = 1;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(HasChange)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Really copy the entry {nudCount.Value} times?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var Entry = F.Entries.Where(m => m.ObjectData.Name == cbObject.SelectedItem.ToString()).Skip((int)nudOffset.Value - 1).FirstOrDefault();
                if (Entry != null)
                {
                    using (var MS = new MemoryStream())
                    {
                        using (var BW = new BinaryWriter(MS))
                        {
                            Entry.Export(BW);
                            BW.Flush();
                            var Props = Entry.Properties;
                            using (var BR = new BinaryReader(MS))
                            {
                                for (var i = 0; i < nudCount.Value; i++)
                                {
                                    MS.Seek(0, SeekOrigin.Begin);
                                    F.Entries.Add(new SaveFileEntry(BR) { Properties = (byte[])Props.Clone() });
                                }
                            }
                        }
                    }
                    MessageBox.Show($"Done", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HasChange = true;
                }
            }
        }
    }
}
