﻿namespace SatisfactorySaveEditor
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
            this.btnHide = new System.Windows.Forms.Button();
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
            this.lvCount.Size = new System.Drawing.Size(292, 550);
            this.lvCount.TabIndex = 0;
            this.lvCount.UseCompatibleStateImageBehavior = false;
            this.lvCount.View = System.Windows.Forms.View.Details;
            this.lvCount.DoubleClick += new System.EventHandler(this.lvCount_DoubleClick);
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
            // btnHide
            // 
            this.btnHide.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnHide.Location = new System.Drawing.Point(0, 550);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(292, 23);
            this.btnHide.TabIndex = 1;
            this.btnHide.Text = "Hide unpositioned objects";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // frmCounter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 573);
            this.Controls.Add(this.lvCount);
            this.Controls.Add(this.btnHide);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCounter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Object Counter";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmCounter_HelpRequested);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvCount;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chCount;
        private System.Windows.Forms.Button btnHide;
    }
}