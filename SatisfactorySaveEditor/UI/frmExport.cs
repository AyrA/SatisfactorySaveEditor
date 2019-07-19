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
                rbAllItems.Text = $"All ({Count})";
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
                    MessageBox.Show(
                        $"{Items.Count()} entr{(Items.Count() == 1 ? "y" : "ies")} Exported to clipboard",
                        "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                var Ser = new XmlSerializer(typeof(SaveFileEntry[]));
                using (var SR = new StringReader(Clipboard.GetText()))
                {
                    SaveFileEntry[] Entries;
                    try
                    {
                        Entries = (SaveFileEntry[])Ser.Deserialize(SR);
                    }
                    catch
                    {
                        Tools.E("The text in your clipboard is not valid save file data", "No content");
                        return;
                    }
                    int ReplaceCount = -1;
                    //Replace all items with the same name
                    if (cbReplaceAll.Checked)
                    {
                        var Names = Entries.Select(m => m.ObjectData.Name).Distinct().ToArray();
                        var TotalStart = F.Entries.Count;
                        F.Entries = F.Entries.Where(m => !Names.Contains(m.ObjectData.Name)).ToList();
                        ReplaceCount = TotalStart - F.Entries.Count;
                    }
                    //Automatically fix internal names by adding incrementing numbers until they're unique.
                    if (cbFixNames.Checked)
                    {
                        var Names = F.Entries.Select(m => m.ObjectData.InternalName).ToArray();
                        foreach (var E in Entries)
                        {
                            if (Names.Contains(E.ObjectData.InternalName))
                            {
                                string BaseName = E.ObjectData.InternalName;
                                if (BaseName.Contains("_"))
                                {
                                    BaseName = BaseName.Substring(0, BaseName.LastIndexOf('_')) + "_";
                                }
                                string NewName = null;
                                int NameCounter = 0;
                                do
                                {
                                    NewName = string.Format("{0}_{1}", BaseName, NameCounter++);
                                } while (Names.Contains(NewName));
                                E.ObjectData.InternalName = NewName;
                            }
                        }
                    }
                    F.Entries.AddRange(Entries);

                    if (ReplaceCount >= 0)
                    {
                        MessageBox.Show(
                            $"Import complete. Deleted {ReplaceCount} existing entries", "Import complete",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Import complete", "Import complete",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    HasChange = true;
                }
            }
            else
            {
                MessageBox.Show(
                    "Your clipboard is empty", "No content",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void frmExport_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }
    }
}
