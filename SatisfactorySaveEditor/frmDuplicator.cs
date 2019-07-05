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
            cbObject.Items.AddRange(F.Entries
                .Select(m => m.ObjectData.Name)
                .Distinct()
                .Select(m => new ShortName(m))
                .OrderBy(m => m)
                .Cast<object>()
                .ToArray());
            cbObject.SelectedIndex = 0;
        }

        private void cbObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbObject.SelectedIndex >= 0)
            {
                var ItemName = ((ShortName)cbObject.SelectedItem).Long;
                var Count = F.Entries.Count(m => m.ObjectData.Name == ItemName);
                nudOffset.Value = 1;
                nudOffset.Maximum = Count;
                nudCount.Value = 1;
                var Example = F.Entries.First(m => m.ObjectData.Name == ItemName);
                if (Example.ObjectData.ObjectType == ObjectTypes.OBJECT_TYPE.OBJECT)
                {
                    cbApplyOffset.Enabled = true;
                }
                else
                {
                    cbApplyOffset.Checked = false;
                    cbApplyOffset.Enabled = false;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Really copy the entry {nudCount.Value} times?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //Names of all existing objects
                var Names = F.Entries
                    .Select(m => m.ObjectData)
                    .OfType<ObjectTypes.GameObject>()
                    .Select(m => m.InternalName)
                    .ToArray();
                //User selected entry
                var Entry = F.Entries.Where(m => m.ObjectData.Name == ((ShortName)cbObject.SelectedItem).Long).Skip((int)nudOffset.Value - 1).First();
                //Entry base name (only if object)
                var BaseName = (Entry.ObjectData.ObjectType == ObjectTypes.OBJECT_TYPE.OBJECT) ? ((ObjectTypes.GameObject)Entry.ObjectData).InternalName : null;
                int NameCounter = 0;
                //Remove the number at the end
                if (!string.IsNullOrEmpty(BaseName))
                {
                    BaseName = BaseName.Substring(0, BaseName.LastIndexOf('_')) + "_";
                }
                if (Entry != null)
                {
                    for (var i = 0; i < nudCount.Value; i++)
                    {
                        var Copy = (SaveFileEntry)Entry.Clone();
                        //Replace the InternalName property of copied instances
                        if (Copy.ObjectData.ObjectType == ObjectTypes.OBJECT_TYPE.OBJECT)
                        {
                            var o = (ObjectTypes.GameObject)Copy.ObjectData;
                            var NewName = BaseName;
                            do
                            {
                                NewName = string.Format("{0}_{1}", BaseName, NameCounter++);
                            } while (Names.Contains(NewName));
                            o.InternalName = NewName;
                            if (cbApplyOffset.Checked)
                            {
                                o.ObjectPosition.X += (int)(i * nudOffsetX.Value);
                                o.ObjectPosition.Y += (int)(i * nudOffsetY.Value);
                                o.ObjectPosition.Y += (int)(i * nudOffsetZ.Value);
                            }
                        }
                        F.Entries.Add(Copy);
                    }
                    MessageBox.Show($"Done", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HasChange = true;
                }
            }
        }

        private void cbApplyOffset_CheckedChanged(object sender, EventArgs e)
        {
            nudOffsetX.Enabled = nudOffsetY.Enabled = nudOffsetZ.Enabled = cbApplyOffset.Checked;
        }

        private void btnMap_Click(object sender, EventArgs e)
        {
            var ItemName = ((ShortName)cbObject.SelectedItem).Long;
            var Item = F.Entries.Where(m => m.ObjectData.Name == ItemName).Skip((int)nudOffset.Value-1).First();
            if (Item.EntryType == ObjectTypes.OBJECT_TYPE.OBJECT)
            {
                MapRender.MapForm.BackgroundImage.Dispose();
                MapRender.MapForm.BackgroundImage = MapRender.Render(new DrawObject(Item, Color.Yellow, 10));
            }
            else
            {
                MessageBox.Show("This type of entry has no map coordinates", "Invalid entry type", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
