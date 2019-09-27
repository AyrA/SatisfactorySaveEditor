using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.IO.Compression;
using System.Drawing;
using System.Text.RegularExpressions;

namespace SatisfactorySaveEditor
{
    public partial class frmManager : Form
    {
        private class LocalFileView
        {
            public string FullFile;
            public string ShortName;
            public bool IsValid;

            public LocalFileView(string FileName, bool ValidFile)
            {
                FullFile = Path.GetFullPath(FileName);
                ShortName = Path.GetFileNameWithoutExtension(FileName);
                IsValid = ValidFile;
            }

            public override string ToString()
            {
                return ShortName;
            }
        }

        private class MapView
        {
            public SMRAPI.Responses.InfoResponse.map Map { get; private set; }

            public MapView(SMRAPI.Responses.InfoResponse.map M)
            {
                Map = M;
            }

            public override string ToString()
            {
                return Map.name;
            }
        }

        private Settings CurrentSettings;

        public frmManager(Settings S)
        {
            CurrentSettings = S;
            InitializeComponent();
            InitFiles();
            InitCloud();
            Tools.SetupKeyHandlers(this);
        }

        private void InitCloud()
        {
            lbCloud.Items.Clear();
            if (SMRAPI.API.ApiKey != Guid.Empty)
            {
                lbCloud.Items.Add("Loading...");
                Thread T = new Thread(delegate ()
                {
                    try
                    {
                        var Res = SMRAPI.API.Info();
                        if (!Res.success)
                        {
                            throw new Exception(Res.msg);
                        }
                        var M = Res.data.maps;
                        if (M != null && M.Length > 0)
                        {
                            Invoke((MethodInvoker)delegate ()
                            {
                                lbCloud.Items.Clear();
                                foreach (var Map in M)
                                {
                                    lbCloud.Items.Add(new MapView(Map));
                                }
                            });
                        }
                        else
                        {
                            Invoke((MethodInvoker)delegate ()
                            {
                                lbCloud.Items.Clear();
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            lbCloud.Items.Clear();
                            lbCloud.Items.Add("API problem. Press [F5] to try again");
                            lbCloud.Items.Add("Double click to change your API key");
                            lbCloud.Items.Add("Error: " + ex.Message);
                        });
                    }
                });
                T.IsBackground = true;
                T.Start();
            }
            else
            {
                lbCloud.Items.Add("API not enabled");
                lbCloud.Items.Add("Double click to enable");
            }
            lbCloud.Enabled = true;
        }

        private void InitFiles()
        {
            tvFiles.Nodes.Clear();
            var Sessions = new Dictionary<string, TreeNode>();
            Thread T = new Thread(delegate ()
            {
                foreach (var FileName in Directory.GetFiles(Program.SaveDirectory, "*.sav", SearchOption.AllDirectories))
                {
                    if (Disposing || IsDisposed)
                    {
                        return;
                    }
                    SaveFile F = null;
                    try
                    {
                        using (var FS = File.OpenRead(FileName))
                        {
                            F = SaveFile.Open(FS);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write(new Exception($"Invalid or locked save file: {FileName}", ex));
                    }

                    if (F != null)
                    {
                        //File is valid game file
                        TreeNode Node = null;
                        var SessionName = string.IsNullOrWhiteSpace(F.SessionName) ? "<no name>" : F.SessionName;
                        if (Sessions.ContainsKey(SessionName))
                        {
                            Node = Sessions[SessionName];
                        }
                        else
                        {
                            Invoke((MethodInvoker)delegate ()
                            {
                                Node = tvFiles.Nodes.Add(SessionName);
                                Sessions.Add(SessionName, Node);
                            });
                        }
                        Invoke((MethodInvoker)delegate ()
                        {
                            var AddedNode = Node.Nodes.Add(Path.GetFileNameWithoutExtension(FileName));
                            AddedNode.Tag = new LocalFileView(FileName, true);
                            Node.ExpandAll();
                        });
                    }
                    else
                    {
                        //File is invalid game file
                        TreeNode InvalidNode = null;
                        if (Sessions.ContainsKey(""))
                        {
                            InvalidNode = Sessions[""];
                        }
                        else
                        {
                            Sessions[""] = InvalidNode = new TreeNode("INVALID");
                            InvalidNode.ForeColor = Color.Red;
                            InvalidNode.BackColor = Color.Yellow;
                        }
                        var Invalid = InvalidNode.Nodes.Add(Path.GetFileNameWithoutExtension(FileName));
                        Invalid.ForeColor = Color.Red;
                        Invalid.Tag = new LocalFileView(FileName, false);
                    }
                }
                //Add invalid nodes on the bottom
                if (Sessions.ContainsKey(""))
                {
                    Log.Write("{0}: At least one invalid save file was found", GetType().Name);
                    Invoke((MethodInvoker)delegate ()
                    {
                        if (Sessions[""].TreeView == null)
                        {
                            tvFiles.Nodes.Add(Sessions[""]);
                            Sessions[""].ExpandAll();
                        }
                    });
                }
            });
            T.Start();
        }

        private bool IsValidNode(TreeNode N)
        {
            return N.Tag == null || ((LocalFileView)N.Tag).IsValid;
        }

        private TreeNode GetSelectedFile()
        {
            var Node = tvFiles.SelectedNode;
            if (Node != null)
            {
                //Don't run on the main nodes
                if (Node.Parent != null)
                {
                    return Node;
                }
            }
            return null;
        }

        private string GetName(TreeNode N)
        {
            return ((LocalFileView)N.Tag).FullFile;
        }

        private void OpenSelected()
        {
            var Node = GetSelectedFile();
            if (Node != null)
            {
                if (IsValidNode(Node))
                {
                    Close();
                    //Main form is guaranteed to exist if the manager could be opened
                    Tools.GetForm<frmMain>().OpenFile(GetName(Node));
                }
                else
                {
                    InvalidMessage();
                }
            }
        }

        private void DeleteSelected()
        {
            var Node = GetSelectedFile();
            if (Node != null)
            {
                if (MessageBox.Show($"Really delete the file {Node.Text}.sav of session {Node.Parent.Text}", "Delete file", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Log.Write("{0}: Deleting {1}.sav", GetType().Name, Node.Text);
                    try
                    {
                        File.Delete(GetName(Node));
                        //Remove node or the entire tree section if it was the last one.
                        //No need to reload the tree
                        if (Node.Parent.Nodes.Count == 1)
                        {
                            Node.Parent.Remove();
                        }
                        else
                        {
                            Node.Remove();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write(new Exception($"Unable to delete {Node.Text}.sav", ex));
                        Tools.E($"Unable to delete file. Reason: {ex.Message}", "error deleting file");
                    }
                }
            }
        }

        private void InvalidMessage()
        {
            Tools.E("This save file seems to be invalid and can't be read.", "Invalid save file");
        }

        private void PreviewFile(SaveFile F)
        {
            using (var BMP = MapRender.RenderFile(F))
            {
                PreviewImage(BMP, F.SessionName);
            }
        }

        private void PreviewImage(Image I, string Name)
        {
            using (var frm = new Form())
            {
                frm.Text = "Preview of " + Name;
                frm.ShowIcon = frm.ShowInTaskbar = false;
                frm.WindowState = FormWindowState.Maximized;
                frm.BackgroundImageLayout = ImageLayout.Zoom;
                frm.BackgroundImage = I;
                Tools.SetupKeyHandlers(frm);
                frm.ShowDialog();
            }
        }

        private void RenderCloudEntry(MapView MapEntry)
        {
            var T = new Thread(delegate ()
            {
                byte[] ImageData;
                try
                {
                    ImageData = SMRAPI.API.Preview(MapEntry.Map.hidden_id);
                    if (ImageData == null || ImageData.Length == 0)
                    {
                        throw new InvalidDataException("API returned an empty image");
                    }
                }
                catch (Exception ex)
                {
                    Tools.E("Error getting preview image.\r\n" + ex.Message, "Cloud Save Preview", this);
                    return;
                }
                Invoke((MethodInvoker)delegate
                {
                    using (var MS = new MemoryStream(ImageData, false))
                    {
                        try
                        {
                            using (var IMG = Image.FromStream(MS))
                            {
                                PreviewImage(IMG, MapEntry.Map.name);
                            }
                        }
                        catch (Exception ex)
                        {
                            Tools.E("Error getting preview image.\r\n" + ex.Message, "Cloud Save Preview", this);
                            return;
                        }
                    }
                });
            });
            T.IsBackground = true;
            T.Start();
        }

        private void DeleteCloudItem(MapView Item)
        {
            if (MessageBox.Show($"Delete the file {Item.Map.name} from your cloud account?", "Cloud Save File", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                var T = new Thread(delegate ()
                {
                    SMRAPI.Responses.DelResponse DelData;
                    try
                    {
                        DelData = SMRAPI.API.DelMap(Item.Map.hidden_id);
                        if (DelData == null)
                        {
                            throw new InvalidDataException("API returned an invalid response");
                        }
                        if (!DelData.success)
                        {
                            throw new Exception(DelData.msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        Tools.E("Error deleting the map.\r\n" + ex.Message, "Cloud Save Removal", this);
                        return;
                    }
                    Invoke((MethodInvoker)delegate
                    {
                        InitCloud();
                        Tools.I("File Deleted", "Cloud Save File");
                    });
                });
                T.IsBackground = true;
                T.Start();
            }
        }

        private void DownloadCloudItem(MapView Item)
        {
            string FileName = Item.Map.name;
            //Make name unique
            int i = 1;
            while (File.Exists(Path.Combine(Program.SaveDirectory, FileName)))
            {
                FileName = FileName.Substring(0, FileName.Length - 4) + $"_{i++}.sav";
            }
            var T = new Thread(delegate ()
            {
                try
                {
                    using (var FS = File.Create(Path.Combine(Program.SaveDirectory, FileName)))
                    {
                        if (!SMRAPI.API.Download(Item.Map.hidden_id, FS))
                        {
                            throw new Exception("Unable to download file");
                        }
                        Tools.I($"Download completed. Map saved as {Path.GetFileName(FileName)}", "Cloud Save Download", this);
                    }
                }
                catch (Exception ex)
                {
                    Tools.E("Error getting preview image.\r\n" + ex.Message, "Cloud Save Download", this);
                    try
                    {
                        File.Delete(FileName);
                    }
                    catch
                    {
                        Tools.E($"Unable to delete partial file {FileName}. Please do manually", "Cloud Save Download", this);
                    }
                }
                Invoke((MethodInvoker)InitFiles);
            });
            T.IsBackground = true;
            T.Start();
        }

        #region Form Events

        private void tvFiles_DoubleClick(object sender, EventArgs e)
        {
            OpenSelected();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSelected();
        }

        private void renderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Node = GetSelectedFile();
            if (Node != null)
            {
                if (IsValidNode(Node))
                {
                    Log.Write("{0}: Rendering {1}.sav", GetType().Name, Node.Text);
                    using (var FS = File.OpenRead(GetName(Node)))
                    {
                        SaveFile F;
                        try
                        {
                            F = SaveFile.Open(FS);
                        }
                        catch (Exception ex)
                        {
                            Log.Write(new Exception("Attempted to render invalid file", ex));
                            InvalidMessage();
                            return;
                        }
                        PreviewFile(F);
                    }
                }
                else
                {
                    InvalidMessage();
                }
            }
        }

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Node = GetSelectedFile();
            if (Node != null)
            {
                SFD.FileName = Node.Text + ".sav.gz";
                SFD.InitialDirectory = Path.GetDirectoryName(((LocalFileView)Node.Tag).FullFile);

                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        double Perc = 0;
                        using (var IN = File.OpenRead(GetName(Node)))
                        {
                            Perc = IN.Length;
                            using (var FS = File.Create(SFD.FileName))
                            {
                                using (var OUT = new GZipStream(FS, CompressionLevel.Optimal, true))
                                {
                                    IN.CopyTo(OUT);
                                }
                                Perc = FS.Position / Perc * 100;
                            }
                        }
                        FeatureReport.Used(FeatureReport.Feature.DoBackup);
                        MessageBox.Show($"Backup complete. Reduced to {Math.Round(Perc)}% of original size", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        Log.Write(new Exception("Unable to perform a backup", ex));
                        Tools.E($"Unable to back up your savegame\r\n{ex.Message}", "Backup Error");
                    }
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        private void tvFiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.Parent != null)
                {
                    bool Opt = IsValidNode(e.Node);
                    renderToolStripMenuItem.Enabled = Opt;
                    openToolStripMenuItem.Enabled = Opt;
                    renameToolStripMenuItem.Enabled = Opt;
                    backupToolStripMenuItem.Enabled = Opt;
                    uploadToolStripMenuItem.Enabled = Opt;

                    CMSLocal.Show(tvFiles, e.Location);
                    tvFiles.SelectedNode = e.Node;
                }
            }
        }

        private void tvFiles_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    DeleteSelected();
                    break;
                case Keys.Enter:
                    OpenSelected();
                    break;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                FileStream FS;
                try
                {
                    FS = File.OpenRead(OFD.FileName);
                }
                catch (Exception ex)
                {
                    Log.Write(new Exception($"Unable to import {OFD.FileName}", ex));
                    Tools.E($"Unable to open the selected file.\r\n{ex.Message}", "Import error");
                    return;
                }
                using (FS)
                {
                    var NewName = Path.GetFileName(OFD.FileName);
                    if (NewName.ToLower().Contains(".sav"))
                    {
                        //Make .sav the last part of the name
                        NewName = NewName.Substring(0, NewName.ToLower().LastIndexOf(".sav")) + ".sav";
                        if (NewName == ".sav")
                        {
                            NewName = "Import.sav";
                        }
                    }
                    else
                    {
                        //Add .sav extension because it's not there
                        NewName += ".sav";
                    }
                    //Make name unique
                    int i = 1;
                    while (File.Exists(Path.Combine(Program.SaveDirectory, NewName)))
                    {
                        NewName = NewName.Substring(0, NewName.Length - 4) + $"_{i++}.sav";
                    }

                    //Check if selected file is actually a (somewhat) valid save game
                    var F = SaveFile.Open(FS);
                    if (F == null)
                    {
                        NewName = Path.Combine(Program.SaveDirectory, NewName);
                        if (MessageBox.Show("This file doesn't looks like it's a save file. Import anyways?", "Incompatible file", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            //User decided to copy anyway. Decompress the file now as needed
                            FS.Seek(0, SeekOrigin.Begin);
                            if (Tools.IsGzFile(FS))
                            {
                                Log.Write("{0}: looks compressed: {1}", GetType().Name, OFD.FileName);
                                try
                                {
                                    using (var GZS = new GZipStream(FS, CompressionMode.Decompress))
                                    {
                                        //Try to manually decompress a few bytes before fully copying.
                                        //This will throw an exception for files that are not decompressable before the output file is created
                                        byte[] Data = new byte[100];
                                        int R = GZS.Read(Data, 0, Data.Length);
                                        using (var OUT = File.Create(NewName))
                                        {
                                            OUT.Write(Data, 0, R);
                                            GZS.CopyTo(OUT);
                                        }
                                    }
                                    InitFiles();
                                    FeatureReport.Used(FeatureReport.Feature.RestoreBackup);
                                }
                                catch (Exception ex)
                                {
                                    Log.Write(new Exception($"Unable to decompress {OFD.FileName}", ex));
                                    Tools.E($"File looks compressed, but we are unable to decompress it.\r\n{ex.Message}", "Decompression error");
                                }
                            }
                            else
                            {
                                Log.Write("{0}: Importing uncompressed {1}", GetType().Name, OFD.FileName);
                                //File is not compressed. Just copy as-is
                                using (var OUT = File.Create(NewName))
                                {
                                    FS.CopyTo(OUT);
                                    InitFiles();
                                }
                            }
                        }
                    }
                    else
                    {
                        //Supply the NewName path to have a name that's not a conflict by default
                        using (var Ren = new frmRename(F.SessionName, Path.GetFileNameWithoutExtension(NewName)))
                        {
                            if (Ren.ShowDialog() == DialogResult.OK)
                            {
                                NewName = Path.Combine(Program.SaveDirectory, Ren.RenameFileName + ".sav");
                                if (!File.Exists(NewName) || MessageBox.Show($"There is already a file named {Ren.RenameFileName}.sav. Overwrite this file?", "Confirm overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                {
                                    F.SessionName = Ren.RenameSessionName;
                                    using (var OUT = File.Create(NewName))
                                    {
                                        F.Export(OUT);
                                    }
                                    InitFiles();
                                }
                                else
                                {
                                    Log.Write("{0}: import destination exists: User cancelled", GetType().Name);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Node = GetSelectedFile();
            if (Node != null)
            {
                FileStream FS = null;
                SaveFile F = null;
                try
                {
                    FS = File.OpenRead(GetName(Node));
                }
                catch (Exception ex)
                {
                    Log.Write(new Exception("Unable to rename the file", ex));
                    Tools.E($"Unable to rename the file.\r\n{ex.Message}", "File rename");
                    return;
                }
                using (FS)
                {
                    F = SaveFile.Open(FS);
                }
                if (F != null)
                {
                    using (var Ren = new frmRename(F.SessionName, Node.Text))
                    {
                        if (Ren.ShowDialog() == DialogResult.OK)
                        {
                            var NewName = Path.Combine(Path.GetDirectoryName(GetName(Node)), Ren.RenameFileName + ".sav");
                            //Show rename dialog if the file itself is renamed and the destination exists already
                            if (
                                Node.Text == Ren.RenameFileName ||
                                !File.Exists(NewName) ||
                                MessageBox.Show($"There is already a file named {Ren.RenameFileName}.sav. Overwrite this file?", "Confirm overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                F.SessionName = Ren.RenameSessionName;
                                try
                                {
                                    FS = File.Create(NewName);
                                }
                                catch (Exception ex)
                                {
                                    Log.Write(new Exception("Unable to rename the file", ex));
                                    FS = null;
                                    Tools.E($"Unable to rename the file.\r\n{ex.Message}", "File rename");
                                }
                                if (FS != null)
                                {
                                    using (FS)
                                    {
                                        F.Export(FS);
                                        try
                                        {
                                            File.Delete(GetName(Node));
                                        }
                                        catch (Exception ex)
                                        {
                                            Log.Write(new Exception("Unable to delete the old copy", ex));
                                            Tools.E($"File renamed, but we are unable to delete the old copy. Please do so manually.\r\n{ex.Message}", "Unable to rename");
                                        }
                                        InitFiles();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    InvalidMessage();
                }
            }
        }

        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Node = GetSelectedFile();
            if (Node != null)
            {
                try
                {
                    File.Copy(GetName(Node), Tools.GetNewName(GetName(Node)));
                }
                catch (Exception ex)
                {
                    Log.Write(new Exception("Unable to duplicate save file", ex));
                    Tools.E($"Unable to duplicate file.\r\n{ex.Message}", "Duplication error");
                    return;
                }
                InitFiles();
            }
        }

        private void frmManager_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }

        private void frmManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                InitCloud();
            }
        }

        private void lbCloud_DoubleClick(object sender, EventArgs e)
        {
            var Item = lbCloud.SelectedItem;
            if (Item != null)
            {
                if (Item.GetType() == typeof(MapView))
                {
                    var MapEntry = ((MapView)Item);
                    RenderCloudEntry(MapEntry);
                }
                else
                {
                    using (var ApiForm = new frmApiRegister(CurrentSettings))
                    {
                        if (ApiForm.ShowDialog() == DialogResult.OK)
                        {
                            InitCloud();
                        }
                    }
                }
            }
        }

        private void lbCloud_MouseClick(object sender, MouseEventArgs e)
        {
            var Index = lbCloud.IndexFromPoint(e.Location);
            if (Index >= 0 && e.Button == MouseButtons.Right)
            {
                lbCloud.SelectedIndex = Index;
                CMSRemote.Show(lbCloud, e.Location);
            }
        }

        private void lbCloud_KeyDown(object sender, KeyEventArgs e)
        {
            if (lbCloud.SelectedIndex < 0 || !(lbCloud.SelectedItem is MapView))
            {
                return;
            }
            var Item = (MapView)lbCloud.SelectedItem;

            switch (e.KeyCode)
            {
                case Keys.C:
                    if (e.Modifiers == Keys.Control)
                    {
                        Clipboard.Clear();
                        Clipboard.SetText(Item.Map.hidden_id.ToString());
                    }
                    break;
                case Keys.Delete:
                    DeleteCloudItem(Item);
                    break;
                case Keys.Enter:
                    RenderCloudEntry(Item);
                    break;
            }
        }

        private void previewRemoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbCloud.SelectedItem is MapView)
            {
                RenderCloudEntry((MapView)lbCloud.SelectedItem);
            }
        }

        private void downloadRemoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbCloud.SelectedItem is MapView)
            {
                DownloadCloudItem((MapView)lbCloud.SelectedItem);
            }
        }

        private void editRemoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbCloud.SelectedItem is MapView)
            {
                using (var editForm = new frmCloudEdit(((MapView)lbCloud.SelectedItem).Map))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        InitCloud();
                    }
                }
            }
        }

        private void copyHiddenIdRemoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbCloud.SelectedItem is MapView)
            {
                Clipboard.Clear();
                Clipboard.SetText(((MapView)lbCloud.SelectedItem).Map.hidden_id.ToString());
            }
        }

        private void deleteRemoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbCloud.SelectedItem is MapView)
            {
                DeleteCloudItem((MapView)lbCloud.SelectedItem);
            }
        }

        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Node = GetSelectedFile();
            if (Node != null)
            {
                if (SMRAPI.API.ApiKey != SMRAPI.API.API_ANONYMOUS_KEY)
                {
                    var FileName = GetName(Node);
                    lbCloud.Items.Clear();
                    lbCloud.Items.Add("Uploading and processing file");
                    lbCloud.Items.Add("This can take a while");
                    lbCloud.Enabled = false;
                    Thread T = new Thread(delegate ()
                    {
                        try
                        {
                            var Result = SMRAPI.API.AddMap(FileName);
                            if (!Result.success)
                            {
                                throw new Exception(Result.msg);
                            }
                            else
                            {
                                Tools.I("Map uploaded", "Map Upload Success", this);
                            }
                        }
                        catch (Exception ex)
                        {
                            Tools.E("Unable to upload the map at this time.\r\n" + ex.Message, "Map Upload Error", this);
                        }
                        Invoke((MethodInvoker)InitCloud);
                    });
                    T.IsBackground = true;
                    T.Start();
                }
                else
                {
                    Tools.E("You don't have an API key registered. Check the right list for more information.", "API key not registered", this);
                }
            }
        }

        private void btnImportId_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    var Id = Clipboard.GetText();

                    //{12345678-1234-1234-1234-123456789012}
                    var M = Regex.Match(Id, @"[\da-f]{8}-[\da-f]{4}-[\da-f]{4}-[\da-f]{4}-[\da-f]{12}", RegexOptions.IgnoreCase);
                    if (M.Success)
                    {
                        var MapId = Guid.Parse(M.Value);
                        var Res = SMRAPI.API.Details(MapId);
                        if (!Res.success)
                        {
                            throw new Exception(Res.msg);
                        }
                        if (MessageBox.Show($"Do you want to import '{Res.data.name}' from user '{Res.data.user.name}'?", "Map Import", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            bool Result;
                            string NewName = Tools.GetNewName(Path.Combine(Program.SaveDirectory, Res.data.name));
                            using (var FS = File.Create(NewName))
                            {
                                Result = SMRAPI.API.Download(MapId, FS);
                            }
                            if (!Result)
                            {
                                try
                                {
                                    File.Delete(NewName);
                                }
                                catch
                                {
                                    //Don't care. It will show up under invalids and can be removed later.
                                }
                                throw new Exception("Problem downloading the map. Please try again later");
                            }
                            else
                            {
                                Tools.I($"Import of {Path.GetFileName(NewName)} successful", "Map Import", this);
                                InitFiles();
                            }

                        }
                    }
                    else
                    {
                        throw new FormatException("Your clipboard doesn't contains a map id.");
                    }
                }
                else
                {
                    throw new Exception("No Map Id found in your clipboard");
                }
            }
            catch (Exception ex)
            {
                Tools.E("Error importing map.\r\n" + ex.Message, "Map Import", this);
            }
        }

        #endregion
    }
}