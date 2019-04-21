using System;
using System.IO;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmMain : Form
    {
        string FileName = null;
        SaveFile F = null;
        bool HasChange = false;
        bool NameChanged = false;


        public frmMain()
        {
            InitializeComponent();
            SFD.InitialDirectory = OFD.InitialDirectory = Environment.ExpandEnvironmentVariables(Program.SAVEDIR);
        }

        private void InfoChange(int Count, string ItemName)
        {
            HasChange |= Count > 0;

            if (Count > 0)
            {
                MessageBox.Show($"Processed {Count} {ItemName} entries", "File Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Did not find any {ItemName} entries", "File Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void NA(string Reason)
        {
            MessageBox.Show($"This function is currently unavailable. Reason:\r\n{Reason}", "Function unavailable", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                FileName = OFD.FileName;
                using (var FS = OFD.OpenFile())
                {
                    using (var BR = new BinaryReader(FS))
                    {
                        F = new SaveFile(BR);
                        HasChange = false;
                        NameChanged = false;
                        MessageBox.Show(@"This is still in development and functionality is limited.
You can currently only use the 'Quick Actions' and the 'Header Editor'", "Limited Functionality", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null && HasChange)
            {
                if (MessageBox.Show("Overweite your existing game?", "Overwrite save file", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    SaveCurrent();
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                if (NameChanged || MessageBox.Show(@"It's recommended that you change the session name to avoid confusion.
It's totally fine to not change it but different session names makes it easier to tell them apart.

Proceed WITHOUT changing it?", "Session Name Change recommended", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (SFD.ShowDialog() == DialogResult.OK)
                    {
                        using (var FS = File.Create(SFD.FileName))
                        {
                            F.Export(FS);
                        }
                        FileName = SFD.FileName;
                        HasChange = false;
                    }
                }
            }
        }

        private void removeLizardDoggosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                InfoChange(SaveFileHelper.RemoveLizardDoggos(F), "Doggos");
            }
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                InfoChange(SaveFileHelper.RestoreRocks(F), "Rocks");
            }

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NA("Entry layout for removed rocks is unknown");
        }

        private void restorePlantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                InfoChange(SaveFileHelper.RestorePlants(F), "plants and trees");
            }

        }

        private void restoreBerriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                InfoChange(SaveFileHelper.RestoreBerries(F), "berries, nuts and mushrooms");
            }

        }

        private void removeAnimalPartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                InfoChange(SaveFileHelper.RemoveAnimalParts(F), "animal parts (organs, etc)");
            }

        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                if (MessageBox.Show(@"This resets all drop pods to their initial state.
This means you can recover harddrives from them again.
You will likely end up with more drives than there are recipes in the game.
To just restore the collectable items around them, use the 'Restore Pickups' option instead.
Really continue?", "Surplus drives", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    InfoChange(SaveFileHelper.RestoreDropPods(F), "drop pods");
                }
            }

        }

        private void restorePickupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                if (MessageBox.Show(@"This only restores the items around drop pods and not the hard drives.
Really continue?", "Surplus drives", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    InfoChange(SaveFileHelper.RestorePickups(F), "items");
                }
            }
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NA("I have not yet decoded the inventory entries.");
        }

        private void linkTogetherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NA(@"I have not yet decoded the inventory entries.
Once done, you will be able to link two containers together so they share their inventory.");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (HasChange)
            {
                switch (MessageBox.Show($"You have unsaved changes. Overwrite {Path.GetFileName(FileName)} before exiting?", "Unsaved changes.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
                {
                    case DialogResult.Yes:
                        SaveCurrent();
                        break;
                    case DialogResult.No:
                        //Do nothing
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }

        }

        private void SaveCurrent()
        {
            var Backup = Path.ChangeExtension(FileName, ".bak");
            if (!File.Exists(Backup))
            {
                File.Copy(FileName, Backup);
                MessageBox.Show($"Because this is your first time overwriting this file, a backup ({Path.GetFileName(Backup)}) has been created in the same directory.", "Backup created", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            using (var FS = File.Create(FileName))
            {
                F.Export(FS);
            }
            HasChange = false;
        }

        private void editHeaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                using (var FH = new frmHeaderEditor(F))
                {
                    if (FH.ShowDialog() == DialogResult.OK)
                    {
                        NameChanged |= (F.SessionName != FH.SessionName);
                        F.SessionName = FH.SessionName;
                        F.PlayTime = TimeSpan.Parse(FH.PlayTime);
                    }
                }
            }
        }
    }
}
