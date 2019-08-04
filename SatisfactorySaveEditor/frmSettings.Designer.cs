namespace SatisfactorySaveEditor
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.gbMessages = new System.Windows.Forms.GroupBox();
            this.btnMessageHide = new System.Windows.Forms.Button();
            this.btnMessageShow = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblDIalogHint = new System.Windows.Forms.Label();
            this.cbAutoUpdate = new System.Windows.Forms.CheckBox();
            this.cbShowChangeLog = new System.Windows.Forms.CheckBox();
            this.gbUpdate = new System.Windows.Forms.GroupBox();
            this.lblUpdateInfo = new System.Windows.Forms.Label();
            this.lblAutoSave = new System.Windows.Forms.Label();
            this.gbMessages.SuspendLayout();
            this.gbUpdate.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMessages
            // 
            this.gbMessages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbMessages.Controls.Add(this.lblDIalogHint);
            this.gbMessages.Controls.Add(this.btnMessageShow);
            this.gbMessages.Controls.Add(this.btnMessageHide);
            this.gbMessages.Location = new System.Drawing.Point(12, 12);
            this.gbMessages.Name = "gbMessages";
            this.gbMessages.Size = new System.Drawing.Size(368, 138);
            this.gbMessages.TabIndex = 0;
            this.gbMessages.TabStop = false;
            this.gbMessages.Text = "Dialog Boxes";
            // 
            // btnMessageHide
            // 
            this.btnMessageHide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMessageHide.Location = new System.Drawing.Point(6, 71);
            this.btnMessageHide.Name = "btnMessageHide";
            this.btnMessageHide.Size = new System.Drawing.Size(356, 23);
            this.btnMessageHide.TabIndex = 1;
            this.btnMessageHide.Text = "Mark all messages as &read";
            this.btnMessageHide.UseVisualStyleBackColor = true;
            this.btnMessageHide.Click += new System.EventHandler(this.btnMessageHide_Click);
            // 
            // btnMessageShow
            // 
            this.btnMessageShow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMessageShow.Location = new System.Drawing.Point(6, 100);
            this.btnMessageShow.Name = "btnMessageShow";
            this.btnMessageShow.Size = new System.Drawing.Size(356, 23);
            this.btnMessageShow.TabIndex = 2;
            this.btnMessageShow.Text = "Mark all messages as &unread";
            this.btnMessageShow.UseVisualStyleBackColor = true;
            this.btnMessageShow.Click += new System.EventHandler(this.btnMessageShow_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(305, 338);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblDIalogHint
            // 
            this.lblDIalogHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDIalogHint.Location = new System.Drawing.Point(11, 21);
            this.lblDIalogHint.Name = "lblDIalogHint";
            this.lblDIalogHint.Size = new System.Drawing.Size(345, 44);
            this.lblDIalogHint.TabIndex = 0;
            this.lblDIalogHint.Text = "Some dialog boxes are shown once to give hints on some features.\r\nHere you can ma" +
    "rk them all as read or unread";
            // 
            // cbAutoUpdate
            // 
            this.cbAutoUpdate.AutoSize = true;
            this.cbAutoUpdate.Location = new System.Drawing.Point(10, 96);
            this.cbAutoUpdate.Name = "cbAutoUpdate";
            this.cbAutoUpdate.Size = new System.Drawing.Size(149, 17);
            this.cbAutoUpdate.TabIndex = 1;
            this.cbAutoUpdate.Text = "&Enable automatic updates";
            this.cbAutoUpdate.UseVisualStyleBackColor = true;
            this.cbAutoUpdate.CheckedChanged += new System.EventHandler(this.cbAutoUpdate_CheckedChanged);
            // 
            // cbShowChangeLog
            // 
            this.cbShowChangeLog.AutoSize = true;
            this.cbShowChangeLog.Location = new System.Drawing.Point(10, 119);
            this.cbShowChangeLog.Name = "cbShowChangeLog";
            this.cbShowChangeLog.Size = new System.Drawing.Size(249, 17);
            this.cbShowChangeLog.TabIndex = 2;
            this.cbShowChangeLog.Text = "&Show Change log automatically after an update";
            this.cbShowChangeLog.UseVisualStyleBackColor = true;
            this.cbShowChangeLog.CheckedChanged += new System.EventHandler(this.cbShowChangeLog_CheckedChanged);
            // 
            // gbUpdate
            // 
            this.gbUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbUpdate.Controls.Add(this.lblUpdateInfo);
            this.gbUpdate.Controls.Add(this.cbAutoUpdate);
            this.gbUpdate.Controls.Add(this.cbShowChangeLog);
            this.gbUpdate.Location = new System.Drawing.Point(12, 156);
            this.gbUpdate.Name = "gbUpdate";
            this.gbUpdate.Size = new System.Drawing.Size(368, 150);
            this.gbUpdate.TabIndex = 1;
            this.gbUpdate.TabStop = false;
            this.gbUpdate.Text = "Update Settings";
            // 
            // lblUpdateInfo
            // 
            this.lblUpdateInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpdateInfo.Location = new System.Drawing.Point(11, 21);
            this.lblUpdateInfo.Name = "lblUpdateInfo";
            this.lblUpdateInfo.Size = new System.Drawing.Size(345, 72);
            this.lblUpdateInfo.TabIndex = 0;
            this.lblUpdateInfo.Text = resources.GetString("lblUpdateInfo.Text");
            // 
            // lblAutoSave
            // 
            this.lblAutoSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAutoSave.AutoSize = true;
            this.lblAutoSave.Location = new System.Drawing.Point(8, 343);
            this.lblAutoSave.Name = "lblAutoSave";
            this.lblAutoSave.Size = new System.Drawing.Size(163, 13);
            this.lblAutoSave.TabIndex = 2;
            this.lblAutoSave.Text = "Changes are saved automatically";
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 373);
            this.Controls.Add(this.lblAutoSave);
            this.Controls.Add(this.gbUpdate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbMessages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Application Settings";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmSettings_HelpRequested);
            this.gbMessages.ResumeLayout(false);
            this.gbUpdate.ResumeLayout(false);
            this.gbUpdate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMessages;
        private System.Windows.Forms.Button btnMessageShow;
        private System.Windows.Forms.Button btnMessageHide;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblDIalogHint;
        private System.Windows.Forms.CheckBox cbAutoUpdate;
        private System.Windows.Forms.CheckBox cbShowChangeLog;
        private System.Windows.Forms.GroupBox gbUpdate;
        private System.Windows.Forms.Label lblUpdateInfo;
        private System.Windows.Forms.Label lblAutoSave;
    }
}