namespace SatisfactorySaveEditor
{
    partial class frmCounter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCounter));
            this.lvCount = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvCount
            // 
            this.lvCount.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chCount});
            this.lvCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvCount.FullRowSelect = true;
            this.lvCount.Location = new System.Drawing.Point(0, 0);
            this.lvCount.Name = "lvCount";
            this.lvCount.Size = new System.Drawing.Size(292, 573);
            this.lvCount.TabIndex = 0;
            this.lvCount.UseCompatibleStateImageBehavior = false;
            this.lvCount.View = System.Windows.Forms.View.Details;
            this.lvCount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvCount_KeyDown);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 200;
            // 
            // chCount
            // 
            this.chCount.Text = "Count";
            // 
            // frmCounter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 573);
            this.Controls.Add(this.lvCount);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCounter";
            this.Text = "Object Counter";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmCounter_HelpRequested);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvCount;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chCount;
    }
}