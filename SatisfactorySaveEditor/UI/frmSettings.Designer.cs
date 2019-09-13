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
            this.lblDIalogHint = new System.Windows.Forms.Label();
            this.btnMessageShow = new System.Windows.Forms.Button();
            this.btnMessageHide = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cbAutoUpdate = new System.Windows.Forms.CheckBox();
            this.cbShowChangeLog = new System.Windows.Forms.CheckBox();
            this.lblUpdateInfo = new System.Windows.Forms.Label();
            this.lblAutoSave = new System.Windows.Forms.Label();
            this.lblId = new System.Windows.Forms.LinkLabel();
            this.cbRandom = new System.Windows.Forms.CheckBox();
            this.cbStopReporting = new System.Windows.Forms.CheckBox();
            this.lblFeatureReport = new System.Windows.Forms.Label();
            this.cbAutostartManager = new System.Windows.Forms.CheckBox();
            this.tabSettings = new System.Windows.Forms.TabControl();
            this.tabTips = new System.Windows.Forms.TabPage();
            this.tabUpdates = new System.Windows.Forms.TabPage();
            this.tabReport = new System.Windows.Forms.TabPage();
            this.tabUI = new System.Windows.Forms.TabPage();
            this.tabCloud = new System.Windows.Forms.TabPage();
            this.lnkOpenRepository = new System.Windows.Forms.LinkLabel();
            this.btnClearKey = new System.Windows.Forms.Button();
            this.lblApiInfo = new System.Windows.Forms.Label();
            this.tabSettings.SuspendLayout();
            this.tabTips.SuspendLayout();
            this.tabUpdates.SuspendLayout();
            this.tabReport.SuspendLayout();
            this.tabUI.SuspendLayout();
            this.tabCloud.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDIalogHint
            // 
            this.lblDIalogHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDIalogHint.Location = new System.Drawing.Point(10, 7);
            this.lblDIalogHint.Name = "lblDIalogHint";
            this.lblDIalogHint.Size = new System.Drawing.Size(344, 55);
            this.lblDIalogHint.TabIndex = 0;
            this.lblDIalogHint.Text = "Some dialog boxes are shown once to give hints on some features.\r\nHere you can ma" +
    "rk them all as read or unread";
            // 
            // btnMessageShow
            // 
            this.btnMessageShow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMessageShow.Location = new System.Drawing.Point(5, 94);
            this.btnMessageShow.Name = "btnMessageShow";
            this.btnMessageShow.Size = new System.Drawing.Size(349, 23);
            this.btnMessageShow.TabIndex = 2;
            this.btnMessageShow.Text = "Mark all messages as &unread";
            this.btnMessageShow.UseVisualStyleBackColor = true;
            this.btnMessageShow.Click += new System.EventHandler(this.btnMessageShow_Click);
            // 
            // btnMessageHide
            // 
            this.btnMessageHide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMessageHide.Location = new System.Drawing.Point(5, 65);
            this.btnMessageHide.Name = "btnMessageHide";
            this.btnMessageHide.Size = new System.Drawing.Size(349, 23);
            this.btnMessageHide.TabIndex = 1;
            this.btnMessageHide.Text = "Mark all messages as &read";
            this.btnMessageHide.UseVisualStyleBackColor = true;
            this.btnMessageHide.Click += new System.EventHandler(this.btnMessageHide_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(305, 238);
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
            this.cbAutoUpdate.Location = new System.Drawing.Point(13, 110);
            this.cbAutoUpdate.Name = "cbAutoUpdate";
            this.cbAutoUpdate.Size = new System.Drawing.Size(149, 17);
            this.cbAutoUpdate.TabIndex = 1;
            this.cbAutoUpdate.Text = "&Enable automatic updates";
            this.cbAutoUpdate.CheckedChanged += new System.EventHandler(this.cbAutoUpdate_CheckedChanged);
            // 
            // cbShowChangeLog
            // 
            this.cbShowChangeLog.AutoSize = true;
            this.cbShowChangeLog.Location = new System.Drawing.Point(13, 137);
            this.cbShowChangeLog.Name = "cbShowChangeLog";
            this.cbShowChangeLog.Size = new System.Drawing.Size(249, 17);
            this.cbShowChangeLog.TabIndex = 2;
            this.cbShowChangeLog.Text = "&Show Change log automatically after an update";
            this.cbShowChangeLog.CheckedChanged += new System.EventHandler(this.cbShowChangeLog_CheckedChanged);
            // 
            // lblUpdateInfo
            // 
            this.lblUpdateInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpdateInfo.Location = new System.Drawing.Point(10, 7);
            this.lblUpdateInfo.Name = "lblUpdateInfo";
            this.lblUpdateInfo.Size = new System.Drawing.Size(344, 96);
            this.lblUpdateInfo.TabIndex = 0;
            this.lblUpdateInfo.Text = resources.GetString("lblUpdateInfo.Text");
            // 
            // lblAutoSave
            // 
            this.lblAutoSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAutoSave.AutoSize = true;
            this.lblAutoSave.Location = new System.Drawing.Point(8, 243);
            this.lblAutoSave.Name = "lblAutoSave";
            this.lblAutoSave.Size = new System.Drawing.Size(163, 13);
            this.lblAutoSave.TabIndex = 3;
            this.lblAutoSave.Text = "Changes are saved automatically";
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(10, 147);
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
            this.cbRandom.Location = new System.Drawing.Point(9, 87);
            this.cbRandom.Name = "cbRandom";
            this.cbRandom.Size = new System.Drawing.Size(201, 17);
            this.cbRandom.TabIndex = 1;
            this.cbRandom.Text = "&New ID each time you start the editor";
            // 
            // cbStopReporting
            // 
            this.cbStopReporting.AutoSize = true;
            this.cbStopReporting.Location = new System.Drawing.Point(9, 114);
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
            this.lblFeatureReport.Location = new System.Drawing.Point(10, 7);
            this.lblFeatureReport.Name = "lblFeatureReport";
            this.lblFeatureReport.Size = new System.Drawing.Size(344, 77);
            this.lblFeatureReport.TabIndex = 0;
            this.lblFeatureReport.Text = "We collect limited statistics to improve the application.\r\nClick on your ID to vi" +
    "ew the statistics online.\r\nPress [F1] for more information.";
            // 
            // cbAutostartManager
            // 
            this.cbAutostartManager.AutoSize = true;
            this.cbAutostartManager.Location = new System.Drawing.Point(10, 10);
            this.cbAutostartManager.Name = "cbAutostartManager";
            this.cbAutostartManager.Size = new System.Drawing.Size(251, 17);
            this.cbAutostartManager.TabIndex = 2;
            this.cbAutostartManager.Text = "&Show Manager window automatically on launch";
            this.cbAutostartManager.CheckedChanged += new System.EventHandler(this.cbAutostartManager_CheckedChanged);
            // 
            // tabSettings
            // 
            this.tabSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSettings.Controls.Add(this.tabTips);
            this.tabSettings.Controls.Add(this.tabUpdates);
            this.tabSettings.Controls.Add(this.tabReport);
            this.tabSettings.Controls.Add(this.tabUI);
            this.tabSettings.Controls.Add(this.tabCloud);
            this.tabSettings.Location = new System.Drawing.Point(12, 12);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.SelectedIndex = 0;
            this.tabSettings.Size = new System.Drawing.Size(368, 220);
            this.tabSettings.TabIndex = 6;
            // 
            // tabTips
            // 
            this.tabTips.Controls.Add(this.lblDIalogHint);
            this.tabTips.Controls.Add(this.btnMessageShow);
            this.tabTips.Controls.Add(this.btnMessageHide);
            this.tabTips.Location = new System.Drawing.Point(4, 22);
            this.tabTips.Name = "tabTips";
            this.tabTips.Padding = new System.Windows.Forms.Padding(3);
            this.tabTips.Size = new System.Drawing.Size(360, 194);
            this.tabTips.TabIndex = 0;
            this.tabTips.Text = "Usage Tips";
            this.tabTips.UseVisualStyleBackColor = true;
            // 
            // tabUpdates
            // 
            this.tabUpdates.Controls.Add(this.lblUpdateInfo);
            this.tabUpdates.Controls.Add(this.cbAutoUpdate);
            this.tabUpdates.Controls.Add(this.cbShowChangeLog);
            this.tabUpdates.Location = new System.Drawing.Point(4, 22);
            this.tabUpdates.Name = "tabUpdates";
            this.tabUpdates.Padding = new System.Windows.Forms.Padding(3);
            this.tabUpdates.Size = new System.Drawing.Size(360, 194);
            this.tabUpdates.TabIndex = 1;
            this.tabUpdates.Text = "Updates";
            this.tabUpdates.UseVisualStyleBackColor = true;
            // 
            // tabReport
            // 
            this.tabReport.Controls.Add(this.lblId);
            this.tabReport.Controls.Add(this.cbRandom);
            this.tabReport.Controls.Add(this.lblFeatureReport);
            this.tabReport.Controls.Add(this.cbStopReporting);
            this.tabReport.Location = new System.Drawing.Point(4, 22);
            this.tabReport.Name = "tabReport";
            this.tabReport.Padding = new System.Windows.Forms.Padding(3);
            this.tabReport.Size = new System.Drawing.Size(360, 194);
            this.tabReport.TabIndex = 2;
            this.tabReport.Text = "Usage Reporter";
            this.tabReport.UseVisualStyleBackColor = true;
            // 
            // tabUI
            // 
            this.tabUI.Controls.Add(this.cbAutostartManager);
            this.tabUI.Location = new System.Drawing.Point(4, 22);
            this.tabUI.Name = "tabUI";
            this.tabUI.Padding = new System.Windows.Forms.Padding(3);
            this.tabUI.Size = new System.Drawing.Size(360, 194);
            this.tabUI.TabIndex = 3;
            this.tabUI.Text = "User Interface";
            this.tabUI.UseVisualStyleBackColor = true;
            // 
            // tabCloud
            // 
            this.tabCloud.Controls.Add(this.lblApiInfo);
            this.tabCloud.Controls.Add(this.btnClearKey);
            this.tabCloud.Controls.Add(this.lnkOpenRepository);
            this.tabCloud.Location = new System.Drawing.Point(4, 22);
            this.tabCloud.Name = "tabCloud";
            this.tabCloud.Padding = new System.Windows.Forms.Padding(3);
            this.tabCloud.Size = new System.Drawing.Size(360, 194);
            this.tabCloud.TabIndex = 4;
            this.tabCloud.Text = "Cloud";
            this.tabCloud.UseVisualStyleBackColor = true;
            // 
            // lnkOpenRepository
            // 
            this.lnkOpenRepository.AutoSize = true;
            this.lnkOpenRepository.Location = new System.Drawing.Point(113, 98);
            this.lnkOpenRepository.Name = "lnkOpenRepository";
            this.lnkOpenRepository.Size = new System.Drawing.Size(53, 13);
            this.lnkOpenRepository.TabIndex = 7;
            this.lnkOpenRepository.TabStop = true;
            this.lnkOpenRepository.Text = "Visit SMR";
            this.lnkOpenRepository.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOpenRepository_LinkClicked);
            // 
            // btnClearKey
            // 
            this.btnClearKey.Location = new System.Drawing.Point(13, 93);
            this.btnClearKey.Name = "btnClearKey";
            this.btnClearKey.Size = new System.Drawing.Size(94, 23);
            this.btnClearKey.TabIndex = 8;
            this.btnClearKey.Text = "&Clear API key";
            this.btnClearKey.UseVisualStyleBackColor = true;
            this.btnClearKey.Click += new System.EventHandler(this.btnClearKey_Click);
            // 
            // lblApiInfo
            // 
            this.lblApiInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblApiInfo.Location = new System.Drawing.Point(10, 7);
            this.lblApiInfo.Name = "lblApiInfo";
            this.lblApiInfo.Size = new System.Drawing.Size(344, 79);
            this.lblApiInfo.TabIndex = 9;
            this.lblApiInfo.Text = resources.GetString("lblApiInfo.Text");
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 273);
            this.Controls.Add(this.tabSettings);
            this.Controls.Add(this.lblAutoSave);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Application Settings";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmSettings_HelpRequested);
            this.tabSettings.ResumeLayout(false);
            this.tabTips.ResumeLayout(false);
            this.tabUpdates.ResumeLayout(false);
            this.tabUpdates.PerformLayout();
            this.tabReport.ResumeLayout(false);
            this.tabReport.PerformLayout();
            this.tabUI.ResumeLayout(false);
            this.tabUI.PerformLayout();
            this.tabCloud.ResumeLayout(false);
            this.tabCloud.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnMessageShow;
        private System.Windows.Forms.Button btnMessageHide;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblDIalogHint;
        private System.Windows.Forms.CheckBox cbAutoUpdate;
        private System.Windows.Forms.CheckBox cbShowChangeLog;
        private System.Windows.Forms.Label lblUpdateInfo;
        private System.Windows.Forms.Label lblAutoSave;
        private System.Windows.Forms.Label lblFeatureReport;
        private System.Windows.Forms.CheckBox cbRandom;
        private System.Windows.Forms.CheckBox cbStopReporting;
        private System.Windows.Forms.LinkLabel lblId;
        private System.Windows.Forms.CheckBox cbAutostartManager;
        private System.Windows.Forms.TabControl tabSettings;
        private System.Windows.Forms.TabPage tabTips;
        private System.Windows.Forms.TabPage tabUpdates;
        private System.Windows.Forms.TabPage tabReport;
        private System.Windows.Forms.TabPage tabUI;
        private System.Windows.Forms.TabPage tabCloud;
        private System.Windows.Forms.Label lblApiInfo;
        private System.Windows.Forms.Button btnClearKey;
        private System.Windows.Forms.LinkLabel lnkOpenRepository;
    }
}