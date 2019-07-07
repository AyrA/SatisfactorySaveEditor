namespace SatisfactorySaveEditor
{
    partial class frmHelp
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
            this.tbHelp = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbHelp
            // 
            this.tbHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbHelp.Location = new System.Drawing.Point(0, 0);
            this.tbHelp.Multiline = true;
            this.tbHelp.Name = "tbHelp";
            this.tbHelp.ReadOnly = true;
            this.tbHelp.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbHelp.Size = new System.Drawing.Size(592, 373);
            this.tbHelp.TabIndex = 0;
            // 
            // frmHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 373);
            this.Controls.Add(this.tbHelp);
            this.Name = "frmHelp";
            this.Text = "Satisfactory Save Editor Help";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmHelp_HelpRequested);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbHelp;
    }
}