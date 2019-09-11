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
            this.lblDIalogHint = new System.Windows.Forms.Label();
            this.btnMessageShow = new System.Windows.Forms.Button();
            this.btnMessageHide = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cbAutoUpdate = new System.Windows.Forms.CheckBox();
            this.cbShowChangeLog = new System.Windows.Forms.CheckBox();
            this.gbUpdate = new System.Windows.Forms.GroupBox();
            this.lblUpdateInfo = new System.Windows.Forms.Label();
            this.lblAutoSave = new System.Windows.Forms.Label();
            this.gbReport = new System.Windows.Forms.GroupBox();
            this.lblId = new System.Windows.Forms.LinkLabel();
            this.cbRandom = new System.Windows.Forms.CheckBox();
            this.cbStopReporting = new System.Windows.Forms.CheckBox();
            this.lblFeatureReport = new System.Windows.Forms.Label();
            this.gbUI = new System.Windows.Forms.GroupBox();
            this.cbAutostartManager = new System.Windows.Forms.CheckBox();
            this.gbMessages.SuspendLayout();
            this.gbUpdate.SuspendLayout();
            this.gbReport.SuspendLayout();
            this.gbUI.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMessages
            // 
            this.gbMessages.Controls.Add(this.lblDIalogHint);
            this.gbMessages.Controls.Add(this.btnMessageShow);
            this.gbMessages.Controls.Add(this.btnMessageHide);
            this.gbMessages.Location = new System.Drawing.Point(12, 12);
            this.gbMessages.Name = "gbMessages";
            this.gbMessages.Size = new System.Drawing.Size(295, 138);
            this.gbMessages.TabIndex = 0;
            this.gbMessages.TabStop = false;
            this.gbMessages.Text = "Dialog Boxes";
            // 
            // lblDIalogHint
            // 
            this.lblDIalogHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDIalogHint.Location = new System.Drawing.Point(11, 21);
            this.lblDIalogHint.Name = "lblDIalogHint";
            this.lblDIalogHint.Size = new System.Drawing.Size(272, 44);
            this.lblDIalogHint.TabIndex = 0;
            this.lblDIalogHint.Text = "Some dialog boxes are shown once to give hints on some features.\r\nHere you can ma" +
    "rk them all as read or unread";
            // 
            // btnMessageShow
            // 
            this.btnMessageShow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMessageShow.Location = new System.Drawing.Point(6, 100);
            this.btnMessageShow.Name = "btnMessageShow";
            this.btnMessageShow.Size = new System.Drawing.Size(283, 23);
            this.btnMessageShow.TabIndex = 2;
            this.btnMessageShow.Text = "Mark all messages as &unread";
            this.btnMessageShow.UseVisualStyleBackColor = true;
            this.btnMessageShow.Click += new System.EventHandler(this.btnMessageShow_Click);
            // 
            // btnMessageHide
            // 
            this.btnMessageHide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMessageHide.Location = new System.Drawing.Point(6, 71);
            this.btnMessageHide.Name = "btnMessageHide";
            this.btnMessageHide.Size = new System.Drawing.Size(283, 23);
            this.btnMessageHide.TabIndex = 1;
            this.btnMessageHide.Text = "Mark all messages as &read";
            this.btnMessageHide.UseVisualStyleBackColor = true;
            this.btnMessageHide.Click += new System.EventHandler(this.btnMessageHide_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(505, 338);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cbAutoUpdate
            // 
            this.cbAutoUpdate.AutoSize = true;
            this.cbAutoUpdate.Location = new System.Drawing.Point(14, 120);
            this.cbAutoUpdate.Name = "cbAutoUpdate";
            this.cbAutoUpdate.Size = new System.Drawing.Size(149, 17);
            this.cbAutoUpdate.TabIndex = 1;
            this.cbAutoUpdate.Text = "&Enable automatic updates";
            this.cbAutoUpdate.CheckedChanged += new System.EventHandler(this.cbAutoUpdate_CheckedChanged);
            // 
            // cbShowChangeLog
            // 
            this.cbShowChangeLog.AutoSize = true;
            this.cbShowChangeLog.Location = new System.Drawing.Point(14, 143);
            this.cbShowChangeLog.Name = "cbShowChangeLog";
            this.cbShowChangeLog.Size = new System.Drawing.Size(249, 17);
            this.cbShowChangeLog.TabIndex = 2;
            this.cbShowChangeLog.Text = "&Show Change log automatically after an update";
            this.cbShowChangeLog.CheckedChanged += new System.EventHandler(this.cbShowChangeLog_CheckedChanged);
            // 
            // gbUpdate
            // 
            this.gbUpdate.Controls.Add(this.lblUpdateInfo);
            this.gbUpdate.Controls.Add(this.cbAutoUpdate);
            this.gbUpdate.Controls.Add(this.cbShowChangeLog);
            this.gbUpdate.Location = new System.Drawing.Point(12, 156);
            this.gbUpdate.Name = "gbUpdate";
            this.gbUpdate.Size = new System.Drawing.Size(295, 171);
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
            this.lblUpdateInfo.Size = new System.Drawing.Size(272, 96);
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
            this.lblAutoSave.TabIndex = 3;
            this.lblAutoSave.Text = "Changes are saved automatically";
            // 
            // gbReport
            // 
            this.gbReport.Controls.Add(this.lblId);
            this.gbReport.Controls.Add(this.cbRandom);
            this.gbReport.Controls.Add(this.cbStopReporting);
            this.gbReport.Controls.Add(this.lblFeatureReport);
            this.gbReport.Location = new System.Drawing.Point(313, 12);
            this.gbReport.Name = "gbReport";
            this.gbReport.Size = new System.Drawing.Size(267, 190);
            this.gbReport.TabIndex = 2;
            this.gbReport.TabStop = false;
            this.gbReport.Text = "Feature Usage Report";
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(11, 153);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(43, 13);
            this.lblId.TabIndex = 3;
            this.lblId.TabStop = true;
            this.lblId.Text = "Your ID";
            this.lblId.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblId_LinkClicked);
            // 
            // cbRandom
            // 
            this.cbRandom.AutoSize = true;
            this.cbRandom.Location = new System.Drawing.Point(10, 101);
            this.cbRandom.Name = "cbRandom";
            this.cbRandom.Size = new System.Drawing.Size(201, 17);
            this.cbRandom.TabIndex = 1;
            this.cbRandom.Text = "&New ID each time you start the editor";
            // 
            // cbStopReporting
            // 
            this.cbStopReporting.AutoSize = true;
            this.cbStopReporting.Location = new System.Drawing.Point(10, 124);
            this.cbStopReporting.Name = "cbStopReporting";
            this.cbStopReporting.Size = new System.Drawing.Size(124, 17);
            this.cbStopReporting.TabIndex = 2;
            this.cbStopReporting.Text = "&Stop usage reporting";
            this.cbStopReporting.CheckedChanged += new System.EventHandler(this.cbStopReporting_CheckedChanged);
            // 
            // lblFeatureReport
            // 
            this.lblFeatureReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFeatureReport.Location = new System.Drawing.Point(11, 21);
            this.lblFeatureReport.Name = "lblFeatureReport";
            this.lblFeatureReport.Size = new System.Drawing.Size(243, 77);
            this.lblFeatureReport.TabIndex = 0;
            this.lblFeatureReport.Text = "We collect limited statistics to improve the application.\r\nClick on your ID to vi" +
    "ew the statistics online.\r\nPress [F1] for more information.";
            // 
            // gbUI
            // 
            this.gbUI.Controls.Add(this.cbAutostartManager);
            this.gbUI.Location = new System.Drawing.Point(313, 208);
            this.gbUI.Name = "gbUI";
            this.gbUI.Size = new System.Drawing.Size(267, 119);
            this.gbUI.TabIndex = 4;
            this.gbUI.TabStop = false;
            this.gbUI.Text = "UI";
            // 
            // cbAutostartManager
            // 
            this.cbAutostartManager.AutoSize = true;
            this.cbAutostartManager.Location = new System.Drawing.Point(14, 22);
            this.cbAutostartManager.Name = "cbAutostartManager";
            this.cbAutostartManager.Size = new System.Drawing.Size(213, 17);
            this.cbAutostartManager.TabIndex = 2;
            this.cbAutostartManager.Text = "&Show Manager window when launched";
            this.cbAutostartManager.CheckedChanged += new System.EventHandler(this.cbAutostartManager_CheckedChanged);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 373);
            this.Controls.Add(this.gbUI);
            this.Controls.Add(this.gbReport);
            this.Controls.Add(this.lblAutoSave);
            this.Controls.Add(this.gbUpdate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbMessages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Application Settings";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmSettings_HelpRequested);
            this.gbMessages.ResumeLayout(false);
            this.gbUpdate.ResumeLayout(false);
            this.gbUpdate.PerformLayout();
            this.gbReport.ResumeLayout(false);
            this.gbReport.PerformLayout();
            this.gbUI.ResumeLayout(false);
            this.gbUI.PerformLayout();
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
        private System.Windows.Forms.GroupBox gbReport;
        private System.Windows.Forms.Label lblFeatureReport;
        private System.Windows.Forms.CheckBox cbRandom;
        private System.Windows.Forms.CheckBox cbStopReporting;
        private System.Windows.Forms.LinkLabel lblId;
        private System.Windows.Forms.GroupBox gbUI;
        private System.Windows.Forms.CheckBox cbAutostartManager;
    }
}