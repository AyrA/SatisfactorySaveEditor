namespace SatisfactorySaveEditor
{
    partial class frmElementList
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
            this.divider = new System.Windows.Forms.SplitContainer();
            this.lvProcess = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvSkip = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnConfirm = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSkip = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.divider)).BeginInit();
            this.divider.Panel1.SuspendLayout();
            this.divider.Panel2.SuspendLayout();
            this.divider.SuspendLayout();
            this.SuspendLayout();
            // 
            // divider
            // 
            this.divider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divider.Location = new System.Drawing.Point(0, 0);
            this.divider.Name = "divider";
            this.divider.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // divider.Panel1
            // 
            this.divider.Panel1.Controls.Add(this.lvProcess);
            this.divider.Panel1.Controls.Add(this.label1);
            // 
            // divider.Panel2
            // 
            this.divider.Panel2.Controls.Add(this.lvSkip);
            this.divider.Panel2.Controls.Add(this.lblSkip);
            this.divider.Panel2.Controls.Add(this.btnConfirm);
            this.divider.Size = new System.Drawing.Size(592, 573);
            this.divider.SplitterDistance = 306;
            this.divider.SplitterWidth = 10;
            this.divider.TabIndex = 0;
            // 
            // lvProcess
            // 
            this.lvProcess.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chCount});
            this.lvProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvProcess.FullRowSelect = true;
            this.lvProcess.Location = new System.Drawing.Point(0, 23);
            this.lvProcess.Name = "lvProcess";
            this.lvProcess.Size = new System.Drawing.Size(592, 283);
            this.lvProcess.TabIndex = 0;
            this.lvProcess.UseCompatibleStateImageBehavior = false;
            this.lvProcess.View = System.Windows.Forms.View.Details;
            this.lvProcess.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvProcess_KeyDown);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 300;
            // 
            // chCount
            // 
            this.chCount.Text = "Count";
            // 
            // lvSkip
            // 
            this.lvSkip.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvSkip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSkip.FullRowSelect = true;
            this.lvSkip.Location = new System.Drawing.Point(0, 23);
            this.lvSkip.Name = "lvSkip";
            this.lvSkip.Size = new System.Drawing.Size(592, 211);
            this.lvSkip.TabIndex = 1;
            this.lvSkip.UseCompatibleStateImageBehavior = false;
            this.lvSkip.View = System.Windows.Forms.View.Details;
            this.lvSkip.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvSkip_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Count";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnConfirm.Location = new System.Drawing.Point(0, 234);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(592, 23);
            this.btnConfirm.TabIndex = 1;
            this.btnConfirm.Text = "&Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(592, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Items in this list will be processed";
            // 
            // lblSkip
            // 
            this.lblSkip.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSkip.Location = new System.Drawing.Point(0, 0);
            this.lblSkip.Name = "lblSkip";
            this.lblSkip.Size = new System.Drawing.Size(592, 23);
            this.lblSkip.TabIndex = 2;
            this.lblSkip.Text = "Items in this list will be skipped";
            // 
            // frmElementList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 573);
            this.Controls.Add(this.divider);
            this.Name = "frmElementList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Element Process List";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmElementList_HelpRequested);
            this.divider.Panel1.ResumeLayout(false);
            this.divider.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.divider)).EndInit();
            this.divider.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer divider;
        private System.Windows.Forms.ListView lvProcess;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chCount;
        private System.Windows.Forms.ListView lvSkip;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSkip;
    }
}