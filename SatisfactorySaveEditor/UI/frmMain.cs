using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmMain : Form
    {
        private string FileName = null;
        private SaveFile F = null;
        private bool HasChange = false;
        private bool NameChanged = false;
        private Settings S;
        private string SettingsFile = null;

        public frmMain(string InitialFile = null)
        {
            SettingsFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "settings.xml");
            if (File.Exists(SettingsFile))
            {
                try
                {
                    S = Settings.Load(File.ReadAllText(SettingsFile));
                }
                catch (Exception ex)
                {
                    Tools.E($"Unable to load your settings. Defaults will be applied. Reason:\r\n{ex.Message}", "Settings Loader");
                    S = new Settings();
                }
            }
            else
            {
                S = new Settings();
            }

            InitializeComponent();

            MapRender.MapForm = this;

            //Don't block the application startup with the image rendering stuff
            Thread T = new Thread(delegate ()
            {
                MapRender.Init();
                var img = MapRender.GetMap();
                Invoke((MethodInvoker)delegate
                {
                    BackgroundImage = img;
                    if (S.ShowWelcomeMessage)
                    {
                        S.ShowWelcomeMessage = false;
                        Tools.ShowHelp("Welcome");
                    }
                });
            });
            T.Start();
            //Check for updates at most every 24 hours but never in debug mode
            if (!Program.DEBUG && S.LastUpdateCheck <= DateTime.Now.AddDays(-1))
            {
                S.LastUpdateCheck = DateTime.Now;
                CheckUpdate();
            }

            SFD.InitialDirectory = OFD.InitialDirectory = Program.SaveDirectory;
            if (!string.IsNullOrEmpty(InitialFile))
            {
                try
                {
                    OpenFile(InitialFile);
                }
                catch (Exception ex)
                {
                    Tools.E($"Unable to open {InitialFile}\r\n{ex.Message}", "File error");
                }
            }
            Tools.SetupEscHandler(this);
#if DEBUG
            //Enable not fully implemented items
            inventoriesToolStripMenuItem.Visible = true;
#endif
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
            Tools.E($"This function is currently unavailable. Reason:\r\n{Reason}", "Function unavailable");
        }

        private void SaveCurrent()
        {
            var Backup = Path.ChangeExtension(FileName, ".sav.gz");
            if (!File.Exists(Backup))
            {
                if (Tools.Compress(FileName, Backup))
                {
                    MessageBox.Show($"Because this is your first time overwriting this file, a backup ({Path.GetFileName(Backup)}) has been created in the same directory.", "Backup created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (MessageBox.Show($"Unable to create a backup. You can try using the Save file manager to manually create one. Still continue to save the file?", "Backup failed", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            try
            {
                using (var FS = File.Create(FileName))
                {
                    F.Export(FS);
                }
                HasChange = false;
            }
            catch (Exception ex)
            {
                Tools.E($"Unable to save your file.\r\n{ex.Message}", "Saving changes");
            }
        }

        private void ResizeDoggos(float Factor, int Offset)
        {
            var Doggos = F.Entries.Where(m => m.ObjectData.Name == "");
            InfoChange(SaveFileHelper.ItemResizer(Doggos, Factor, Offset), "doggos");
        }

        private void ResizeHint()
        {
            if (S.ShowResizeHint)
            {
                S.ShowResizeHint = false;
                MessageBox.Show(@"Resizing objects is dangerous.
- Creatures that get stuck in the ground because of the resizing will cause massive lag.
- Creatures are not aware of their changed size. Larger creatures don't get faster or stronger.

Resizing will offset the position to avoid the 'getting stuck' problem.
Repeatedly resizing will essentially teleport them into space.
Be aware that all creatures have fall damage", "Resizing objects", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void OpenFile(string SaveFileName)
        {
            FileName = SaveFileName;
            using (var FS = File.OpenRead(FileName))
            {
                F = SaveFile.Open(FS);
                HasChange = false;
                NameChanged = false;
                if (S.ShowLimited)
                {
                    S.ShowLimited = false;
                    MessageBox.Show(@"This is still in development and functionality is limited.
You can currently only use the 'Quick Actions' and the 'Header Editor'", "Limited Functionality", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                RedrawMap();
            }
        }

        private void RedrawMap()
        {
            BackgroundImage.Dispose();
            if (F != null)
            {
                using (var BMP = MapRender.RenderFile(F))
                {
                    //Add file info string
                    using (var G = Graphics.FromImage(BMP))
                    {
                        var Info = string.Format("{0}: {1} {2}",
                            Path.GetFileName(FileName),
                            DateTime.Now.ToShortDateString(),
                            DateTime.Now.ToShortTimeString());
                        using (var F = new Font("Arial", 16))
                        {
                            var Pos = G.MeasureString(Info, F);
                            G.DrawString(
                                Info, F,
                                Brushes.Red,
                                (int)(BMP.Width - Pos.Width),
                                (int)(BMP.Height - Pos.Height));
                        }
                    }
                    BackgroundImage = new Bitmap(BMP);
                    BMP.Save(Path.ChangeExtension(FileName, "png"));
                }
            }
            else
            {
                BackgroundImage = MapRender.GetMap();
            }
        }

        private void CheckUpdate()
        {
            Thread T = new Thread(delegate ()
            {
                //DEBUG ONLY, FORCE UPDATE
                var U = Program.DEBUG || UpdateHandler.HasUpdate();
                if (U)
                {
                    if (UpdateHandler.DownloadUpdate())
                    {
                        Invoke((MethodInvoker)delegate ()
                        {
                            updateAvailableToolStripMenuItem.Visible = true;
                        });
                    }
                }
            });
            T.IsBackground = true;
            T.Start();
        }

        #region Menu Actions

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                OpenFile(OFD.FileName);
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
            if (F != null && MessageBox.Show("You're about to make a big mistake. Delete the space rabits?", "Abandon Doggos", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
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

        private void spawnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                if (MessageBox.Show(@"Removes all spawner entries. This does not removed spawned animals", "Animal spawner", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    InfoChange(SaveFileHelper.RemoveCreatureSpawner(F), "spawner");
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
                        HasChange = true;
                    }
                }
            }
        }

        private void niceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                if (MessageBox.Show(@"Removes animals that generally are friendly to the player (excluding doggos).", "Friendly Animals", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    InfoChange(SaveFileHelper.RemoveNiceCreatures(F), "animals");
                }
            }
        }

        private void evilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                if (MessageBox.Show(@"Removes animals that are hostile to the player.", "Hostile Animals", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    InfoChange(SaveFileHelper.RemoveEvilCreatures(F), "animals");
                }
            }
        }

        private void restoreSlugsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                if (MessageBox.Show(@"Restore all slugs?", "Restors Slugs", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    InfoChange(SaveFileHelper.RestoreSlugs(F), "slugs");
                }
            }

        }

        private void tinyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                ResizeHint();
                if (MessageBox.Show(@"Resize all doggos to a quarter of their regular size? It will not be easy to find them again.", "Resize doggos", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    ResizeDoggos(0.25f, 0);
                }
            }
        }

        private void regularSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResizeHint();
            if (F != null)
            {
                if (MessageBox.Show(@"Reset all doggo sizes to the default?", "Resize doggos", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    //Move them up just a little in case they were smaller
                    ResizeDoggos(1f, 20);
                }
            }
        }

        private void largeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResizeHint();
            if (F != null)
            {
                if (MessageBox.Show(@"Resize doggos to 20 times their regular size? If their current position gets them stuck they will introduce heavy lags.", "Resize doggos", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    ResizeDoggos(20f, 200);
                }
            }
        }

        private void wTFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                ResizeHint();
                if (MessageBox.Show(@"Resize doggos to 100 times their regular size? They almost certainly get stuck and this will introduce massive lags.", "Resize doggos", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    ResizeDoggos(100f, 3000);
                }
            }
        }

        private void duplicatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                if (S.ShowDuplicationHint)
                {
                    MessageBox.Show(@"Duplication is dangerous. The duplicator will not check if duplication makes sense at all.
Some objects will show weird behaviour once duplicated.
Container duplicates for example will share the inventory.", "Duplicator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    S.ShowDuplicationHint = false;
                }
                using (var Cloner = new frmDuplicator(F))
                {
                    if (Cloner.ShowDialog() == DialogResult.OK)
                    {
                        HasChange = true;
                    }
                }
            }
        }

        private void saveFileManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var manager = new frmManager())
            {
                manager.ShowDialog();
            }
        }

        private void deleterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                if (S.ShowDeletionHint)
                {
                    MessageBox.Show(@"Deletion is dangerous.
- The object deleter will not validate your choices (you can delete the HUB)
- The object deleter will not handle dependencies. Example: Deleting containers leaves stray inventory entries behind in the save file.", "Deleter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    S.ShowDeletionHint = false;
                }
                using (var Deleter = new frmDeleter(F))
                {
                    if (Deleter.ShowDialog() == DialogResult.OK)
                    {
                        HasChange = true;
                    }
                }
            }
        }

        private void openLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Program.SaveDirectory);
        }

        private void exportImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                using (var Exporter = new frmExport(F))
                {
                    if (Exporter.ShowDialog() == DialogResult.OK)
                    {
                        HasChange = true;
                    }
                }
            }
        }

        private void redrawMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedrawMap();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var SB = new StringBuilder();
            SB.AppendLine("Satisfactory Save File Editor");
            SB.AppendLine($"Version: {Application.ProductVersion}");
            SB.AppendLine("License: MIT");
            SB.AppendLine("Source: https://cable.ayra.ch/satisfactory/editor.php");
            MessageBox.Show(
                SB.ToString(),
                "Application Info",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void openHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ShowHelp(GetType().Name);
        }

        private void updateAvailableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "An update is available for install. Do you want to install it now?" +
                (HasChange ? "\r\nWARNING! YOU HAVE UNSAVED CHANGES!" : ""),
                "Update Available",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information,
                HasChange ? MessageBoxDefaultButton.Button2 : MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                if (!UpdateHandler.PerformUpdate())
                {
                    Tools.E("We are currently unable to update your application.", "Update error");
                }
            }
        }

        private void clearStringListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                if (F.StringList.Count > 0)
                {
                    if (MessageBox.Show("Really clear the list of items/objects you picked up and/or destroyed?", "Clear String List", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        int C = F.StringList.Count;
                        F.StringList.Clear();
                        InfoChange(C, "String list");
                    }
                }
                else
                {
                    InfoChange(0, "String list");
                }
            }
        }

        private void rangeDeleterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F != null)
            {
                using (var Region = new frmRegionDeleter(F))
                {
                    if (Region.ShowDialog() == DialogResult.OK)
                    {
                        HasChange = true;
                        RedrawMap();
                    }
                }
            }
        }

        #endregion

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

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                File.WriteAllText(SettingsFile, S.Save());
            }
            catch (Exception ex)
            {
                Tools.E($"Unable to save your settings. Reason:\r\n{ex.Message}", "Settings Loader");
            }
        }

        private void frmMain_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }
    }
}
