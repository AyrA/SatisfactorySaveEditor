using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

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
                    SaveFile F;
                    try
                    {
                        using (var BR = new BinaryReader(File.OpenRead(FileName)))
                        {
                            F = new SaveFile(BR);
                        }
                    }
                    catch
                    {
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
                        continue;
                    }

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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to delete file. Reason: {ex.Message}", "error deleting file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    initFiles();
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
                    using (var BR = new BinaryReader(File.OpenRead(GetName(Node))))
                    {
                        SaveFile F;
                        try
                        {
                            F = new SaveFile(BR);
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

                    CMS.Show(tvFiles, e.Location);
                    tvFiles.SelectedNode = e.Node;
                }
            }
        }

        private void tvFiles_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Delete:
                    DeleteSelected();
                    break;
                case Keys.Enter:
                    OpenSelected();
                    break;
            }
        }
    }
}
