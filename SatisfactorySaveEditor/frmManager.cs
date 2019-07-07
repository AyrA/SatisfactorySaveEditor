using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.IO.Compression;

namespace SatisfactorySaveEditor
{
    public partial class frmManager : Form
    {
        public frmManager()
        {
            InitializeComponent();
            initFiles();
        }

        private void initFiles()
        {
            tvFiles.Nodes.Clear();
            var Sessions = new Dictionary<string, TreeNode>();
            Thread T = new Thread(delegate ()
            {
                foreach (var FileName in Directory.GetFiles(Program.SaveDirectory, "*.sav"))
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
                    catch
                    {
                    }

                    if (F != null)
                    {
                        //File is valid game file
                        TreeNode Node = null;
                        if (Sessions.ContainsKey(F.SessionName))
                        {
                            Node = Sessions[F.SessionName];
                        }
                        else
                        {
                            Invoke((MethodInvoker)delegate ()
                            {
                                Node = tvFiles.Nodes.Add(F.SessionName);
                                Sessions.Add(F.SessionName, Node);
                            });
                        }
                        Invoke((MethodInvoker)delegate ()
                        {
                            Node.Nodes.Add(Path.GetFileNameWithoutExtension(FileName));
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
                            InvalidNode.ForeColor = System.Drawing.Color.Red;
                            InvalidNode.BackColor = System.Drawing.Color.Yellow;
                        }
                        var Invalid = InvalidNode.Nodes.Add(Path.GetFileNameWithoutExtension(FileName));
                        Invalid.ForeColor = System.Drawing.Color.Red;
                        Invalid.Tag = "INVALID";
                    }
                }
                //Add invalid nodes on the bottom
                if (Sessions.ContainsKey(""))
                {
                    Invoke((MethodInvoker)delegate ()
                    {
                        tvFiles.Nodes.Add(Sessions[""]);
                        Sessions[""].ExpandAll();
                    });
                }
            });
            T.Start();
        }

        private bool IsValidNode(TreeNode N)
        {
            return N.Tag == null || N.Tag.ToString() != "INVALID";
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
            return Path.Combine(Program.SaveDirectory, N.Text) + ".sav";
        }

        private void OpenSelected()
        {
            var Node = GetSelectedFile();
            if (Node != null)
            {
                if (IsValidNode(Node))
                {
                    Close();
                    Application.OpenForms.OfType<frmMain>().First().OpenFile(GetName(Node));
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
                    try
                    {
                        File.Delete(GetName(Node));
                        //Remove node or the entire tree section if it was the last one.
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
                        MessageBox.Show($"Unable to delete file. Reason: {ex.Message}", "error deleting file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void InvalidMessage()
        {
            MessageBox.Show("This save file seems to be invalid and can't be read.", "Invalid save file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

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
                    using (var FS = File.OpenRead(GetName(Node)))
                    {
                        SaveFile F;
                        try
                        {
                            F = SaveFile.Open(FS);
                        }
                        catch
                        {
                            InvalidMessage();
                            return;
                        }
                        using (var BMP = MapRender.RenderFile(F))
                        {
                            using (var frm = new Form())
                            {
                                frm.Text = "Preview of " + Node.Text;
                                frm.ShowIcon = frm.ShowInTaskbar = false;
                                frm.WindowState = FormWindowState.Maximized;
                                frm.BackgroundImageLayout = ImageLayout.Zoom;
                                frm.BackgroundImage = BMP;
                                frm.ShowDialog();
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

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Node = GetSelectedFile();
            if (Node != null)
            {
                SFD.FileName = Node.Text + ".sav.gz";
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
                        MessageBox.Show($"Backup complete. Reduced to {Math.Round(Perc)}% of original size", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to back up your savegame\r\n{ex.Message}", "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    CMS.Show(tvFiles, e.Location);
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
                    MessageBox.Show($"Unable to open the selected file.\r\n{ex.Message}", "Import error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                    initFiles();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"File looks compressed, but we are unable to decompress it.\r\n{ex.Message}", "Decompression error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                //File is not compressed. Just copy as-is
                                using (var OUT = File.Create(NewName))
                                {
                                    FS.CopyTo(OUT);
                                    initFiles();
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
                                    initFiles();
                                }
                            }
                        }
                        //TODO: Rename
                    }
                }
            }
        }
    }
}
