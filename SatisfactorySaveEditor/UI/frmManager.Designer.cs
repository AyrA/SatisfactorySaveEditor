namespace SatisfactorySaveEditor
{
    partial class frmManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManager));
            this.tvFiles = new System.Windows.Forms.TreeView();
            this.lblInfo = new System.Windows.Forms.Label();
            this.CMSLocal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImport = new System.Windows.Forms.Button();
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.SFD = new System.Windows.Forms.SaveFileDialog();
            this.splitter = new System.Windows.Forms.SplitContainer();
            this.lbCloud = new System.Windows.Forms.ListBox();
            this.CMSRemote = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.downloadRemoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewRemoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyHiddenIdRemoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importIdRemoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editRemoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRemoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CMSLocal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.Panel2.SuspendLayout();
            this.splitter.SuspendLayout();
            this.CMSRemote.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvFiles
            // 
            this.tvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvFiles.Location = new System.Drawing.Point(0, 0);
            this.tvFiles.Name = "tvFiles";
            this.tvFiles.Size = new System.Drawing.Size(392, 527);
            this.tvFiles.TabIndex = 0;
            this.tvFiles.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvFiles_NodeMouseClick);
            this.tvFiles.DoubleClick += new System.EventHandler(this.tvFiles_DoubleClick);
            this.tvFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvFiles_KeyDown);
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(792, 23);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "Use the context menu for more options";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CMSLocal
            // 
            this.CMSLocal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.renderToolStripMenuItem,
            this.uploadToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.backupToolStripMenuItem,
            this.duplicateToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.CMSLocal.Name = "CMS";
            this.CMSLocal.Size = new System.Drawing.Size(126, 158);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.ToolTipText = "Opens the selected file in the save file editor";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // renderToolStripMenuItem
            // 
            this.renderToolStripMenuItem.Name = "renderToolStripMenuItem";
            this.renderToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.renderToolStripMenuItem.Text = "&Render";
            this.renderToolStripMenuItem.ToolTipText = "Renders a map of the selected file";
            this.renderToolStripMenuItem.Click += new System.EventHandler(this.renderToolStripMenuItem_Click);
            // 
            // uploadToolStripMenuItem
            // 
            this.uploadToolStripMenuItem.Name = "uploadToolStripMenuItem";
            this.uploadToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.uploadToolStripMenuItem.Text = "&Upload";
            this.uploadToolStripMenuItem.ToolTipText = "Uploads the selected file to the cloud";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.renameToolStripMenuItem.Text = "R&ename...";
            this.renameToolStripMenuItem.ToolTipText = "Renames the selected file and/or session name";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // backupToolStripMenuItem
            // 
            this.backupToolStripMenuItem.Name = "backupToolStripMenuItem";
            this.backupToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.backupToolStripMenuItem.Text = "&Backup...";
            this.backupToolStripMenuItem.ToolTipText = "Creates a backup of the selected file";
            this.backupToolStripMenuItem.Click += new System.EventHandler(this.backupToolStripMenuItem_Click);
            // 
            // duplicateToolStripMenuItem
            // 
            this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.duplicateToolStripMenuItem.Text = "&Duplicate";
            this.duplicateToolStripMenuItem.ToolTipText = "Duplicates the selected file";
            this.duplicateToolStripMenuItem.Click += new System.EventHandler(this.duplicateToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.deleteToolStripMenuItem.Text = "De&lete";
            this.deleteToolStripMenuItem.ToolTipText = "Deletes the selected file";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // btnImport
            // 
            this.btnImport.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnImport.Location = new System.Drawing.Point(0, 550);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(792, 23);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "&Import file or restore from backup";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // OFD
            // 
            this.OFD.DefaultExt = "sav";
            this.OFD.Filter = "Save games|*.sav;*.sav.gz|All files|*.*";
            this.OFD.Title = "Import a save game or a backup of it";
            // 
            // SFD
            // 
            this.SFD.DefaultExt = "sav.gz";
            this.SFD.Filter = "Save file backup|*.sav.gz|All files|*.*";
            this.SFD.Title = "Backup save game";
            // 
            // splitter
            // 
            this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitter.Location = new System.Drawing.Point(0, 23);
            this.splitter.Name = "splitter";
            // 
            // splitter.Panel1
            // 
            this.splitter.Panel1.Controls.Add(this.tvFiles);
            this.splitter.Panel1MinSize = 100;
            // 
            // splitter.Panel2
            // 
            this.splitter.Panel2.Controls.Add(this.lbCloud);
            this.splitter.Panel2MinSize = 100;
            this.splitter.Size = new System.Drawing.Size(792, 527);
            this.splitter.SplitterDistance = 392;
            this.splitter.SplitterWidth = 8;
            this.splitter.TabIndex = 4;
            // 
            // lbCloud
            // 
            this.lbCloud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCloud.FormattingEnabled = true;
            this.lbCloud.Location = new System.Drawing.Point(0, 0);
            this.lbCloud.Name = "lbCloud";
            this.lbCloud.Size = new System.Drawing.Size(392, 527);
            this.lbCloud.TabIndex = 5;
            this.lbCloud.DoubleClick += new System.EventHandler(this.lbCloud_DoubleClick);
            this.lbCloud.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbCloud_KeyDown);
            this.lbCloud.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbCloud_MouseClick);
            // 
            // CMSRemote
            // 
            this.CMSRemote.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadRemoteToolStripMenuItem,
            this.previewRemoteToolStripMenuItem,
            this.copyHiddenIdRemoteToolStripMenuItem,
            this.importIdRemoteToolStripMenuItem,
            this.editRemoteToolStripMenuItem,
            this.deleteRemoteToolStripMenuItem});
            this.CMSRemote.Name = "CMSRemote";
            this.CMSRemote.Size = new System.Drawing.Size(188, 158);
            // 
            // downloadRemoteToolStripMenuItem
            // 
            this.downloadRemoteToolStripMenuItem.Name = "downloadRemoteToolStripMenuItem";
            this.downloadRemoteToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.downloadRemoteToolStripMenuItem.Text = "&Download";
            this.downloadRemoteToolStripMenuItem.ToolTipText = "Downloads the selected file";
            this.downloadRemoteToolStripMenuItem.Click += new System.EventHandler(this.downloadRemoteToolStripMenuItem_Click);
            // 
            // previewRemoteToolStripMenuItem
            // 
            this.previewRemoteToolStripMenuItem.Name = "previewRemoteToolStripMenuItem";
            this.previewRemoteToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.previewRemoteToolStripMenuItem.Text = "&Preview";
            this.previewRemoteToolStripMenuItem.ToolTipText = "Shows a preview of the selected file";
            this.previewRemoteToolStripMenuItem.Click += new System.EventHandler(this.previewRemoteToolStripMenuItem_Click);
            // 
            // copyHiddenIdRemoteToolStripMenuItem
            // 
            this.copyHiddenIdRemoteToolStripMenuItem.Name = "copyHiddenIdRemoteToolStripMenuItem";
            this.copyHiddenIdRemoteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyHiddenIdRemoteToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.copyHiddenIdRemoteToolStripMenuItem.Text = "&Copy Hidden Id";
            this.copyHiddenIdRemoteToolStripMenuItem.ToolTipText = "Copies the hidden id of the selected file";
            // 
            // importIdRemoteToolStripMenuItem
            // 
            this.importIdRemoteToolStripMenuItem.Name = "importIdRemoteToolStripMenuItem";
            this.importIdRemoteToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.importIdRemoteToolStripMenuItem.Text = "&Import Id";
            this.importIdRemoteToolStripMenuItem.ToolTipText = "Imports a map by the real or hidden id";
            // 
            // editRemoteToolStripMenuItem
            // 
            this.editRemoteToolStripMenuItem.Name = "editRemoteToolStripMenuItem";
            this.editRemoteToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.editRemoteToolStripMenuItem.Text = "&Edit...";
            this.editRemoteToolStripMenuItem.ToolTipText = "Edits properties of the selected file";
            // 
            // deleteRemoteToolStripMenuItem
            // 
            this.deleteRemoteToolStripMenuItem.Name = "deleteRemoteToolStripMenuItem";
            this.deleteRemoteToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.deleteRemoteToolStripMenuItem.Text = "De&lete";
            this.deleteRemoteToolStripMenuItem.ToolTipText = "Deletes the selected file";
            // 
            // frmManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnImport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmManager";
            this.Text = "Save File Manager";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmManager_HelpRequested);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmManager_KeyDown);
            this.CMSLocal.ResumeLayout(false);
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
            this.splitter.ResumeLayout(false);
            this.CMSRemote.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvFiles;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ContextMenuStrip CMSLocal;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.SaveFileDialog SFD;
        private System.Windows.Forms.ToolStripMenuItem backupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitter;
        private System.Windows.Forms.ListBox lbCloud;
        private System.Windows.Forms.ContextMenuStrip CMSRemote;
        private System.Windows.Forms.ToolStripMenuItem downloadRemoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewRemoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyHiddenIdRemoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editRemoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRemoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importIdRemoteToolStripMenuItem;
    }
}