namespace SatisfactorySaveEditor
{
    partial class frmExport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExport));
            this.gbExport = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.rbAllItems = new System.Windows.Forms.RadioButton();
            this.rbRange = new System.Windows.Forms.RadioButton();
            this.nudCount = new System.Windows.Forms.NumericUpDown();
            this.nudStart = new System.Windows.Forms.NumericUpDown();
            this.cbItem = new System.Windows.Forms.ComboBox();
            this.btnMap = new System.Windows.Forms.Button();
            this.gbImport = new System.Windows.Forms.GroupBox();
            this.cbFixNames = new System.Windows.Forms.CheckBox();
            this.cbReplaceAll = new System.Windows.Forms.CheckBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
            this.gbExport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).BeginInit();
            this.gbImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbExport
            // 
            this.gbExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbExport.Controls.Add(this.label3);
            this.gbExport.Controls.Add(this.label2);
            this.gbExport.Controls.Add(this.label1);
            this.gbExport.Controls.Add(this.btnExport);
            this.gbExport.Controls.Add(this.rbAllItems);
            this.gbExport.Controls.Add(this.rbRange);
            this.gbExport.Controls.Add(this.nudCount);
            this.gbExport.Controls.Add(this.nudStart);
            this.gbExport.Controls.Add(this.cbItem);
            this.gbExport.Controls.Add(this.btnMap);
            this.gbExport.Location = new System.Drawing.Point(12, 12);
            this.gbExport.Name = "gbExport";
            this.gbExport.Size = new System.Drawing.Size(468, 100);
            this.gbExport.TabIndex = 0;
            this.gbExport.TabStop = false;
            this.gbExport.Text = "Export";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(305, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Offset";
            this.ttInfo.SetToolTip(this.label3, "Start of range (starts at 1)");
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(382, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Count";
            this.ttInfo.SetToolTip(this.label2, "Number of items");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Object";
            this.ttInfo.SetToolTip(this.label1, "The object to export");
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(385, 64);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(71, 23);
            this.btnExport.TabIndex = 22;
            this.btnExport.Text = "&Export";
            this.ttInfo.SetToolTip(this.btnExport, "Export the given items to the clipboard");
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // rbAllItems
            // 
            this.rbAllItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbAllItems.AutoSize = true;
            this.rbAllItems.Location = new System.Drawing.Point(287, 64);
            this.rbAllItems.Name = "rbAllItems";
            this.rbAllItems.Size = new System.Drawing.Size(36, 17);
            this.rbAllItems.TabIndex = 16;
            this.rbAllItems.Text = "All";
            this.ttInfo.SetToolTip(this.rbAllItems, "Export all items");
            this.rbAllItems.UseVisualStyleBackColor = true;
            // 
            // rbRange
            // 
            this.rbRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbRange.AutoSize = true;
            this.rbRange.Checked = true;
            this.rbRange.Location = new System.Drawing.Point(287, 42);
            this.rbRange.Name = "rbRange";
            this.rbRange.Size = new System.Drawing.Size(14, 13);
            this.rbRange.TabIndex = 15;
            this.rbRange.TabStop = true;
            this.ttInfo.SetToolTip(this.rbRange, "Export the given range");
            this.rbRange.UseVisualStyleBackColor = true;
            // 
            // nudCount
            // 
            this.nudCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudCount.Location = new System.Drawing.Point(385, 38);
            this.nudCount.Name = "nudCount";
            this.nudCount.Size = new System.Drawing.Size(71, 20);
            this.nudCount.TabIndex = 20;
            // 
            // nudStart
            // 
            this.nudStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudStart.Location = new System.Drawing.Point(308, 38);
            this.nudStart.Name = "nudStart";
            this.nudStart.Size = new System.Drawing.Size(71, 20);
            this.nudStart.TabIndex = 18;
            // 
            // cbItem
            // 
            this.cbItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbItem.FormattingEnabled = true;
            this.cbItem.Location = new System.Drawing.Point(6, 38);
            this.cbItem.Name = "cbItem";
            this.cbItem.Size = new System.Drawing.Size(246, 21);
            this.cbItem.TabIndex = 13;
            this.cbItem.SelectedIndexChanged += new System.EventHandler(this.cbItem_SelectedIndexChanged);
            // 
            // btnMap
            // 
            this.btnMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMap.Location = new System.Drawing.Point(258, 37);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(23, 23);
            this.btnMap.TabIndex = 14;
            this.btnMap.Text = "M";
            this.ttInfo.SetToolTip(this.btnMap, "Renders the currently selected objects into the main window map");
            this.btnMap.UseVisualStyleBackColor = true;
            this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
            // 
            // gbImport
            // 
            this.gbImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbImport.Controls.Add(this.cbFixNames);
            this.gbImport.Controls.Add(this.cbReplaceAll);
            this.gbImport.Controls.Add(this.btnImport);
            this.gbImport.Location = new System.Drawing.Point(12, 118);
            this.gbImport.Name = "gbImport";
            this.gbImport.Size = new System.Drawing.Size(468, 58);
            this.gbImport.TabIndex = 0;
            this.gbImport.TabStop = false;
            this.gbImport.Text = "Import";
            // 
            // cbFixNames
            // 
            this.cbFixNames.AutoSize = true;
            this.cbFixNames.Checked = true;
            this.cbFixNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFixNames.Location = new System.Drawing.Point(189, 23);
            this.cbFixNames.Name = "cbFixNames";
            this.cbFixNames.Size = new System.Drawing.Size(110, 17);
            this.cbFixNames.TabIndex = 24;
            this.cbFixNames.Text = "Fix internal names";
            this.ttInfo.SetToolTip(this.cbFixNames, "This fixes the \"InternalName\" property of imported items.\r\nThis name is supposed " +
        "to be unique across your entire save file.\r\n\r\nIt will only fix names where neces" +
        "sary.");
            this.cbFixNames.UseVisualStyleBackColor = true;
            // 
            // cbReplaceAll
            // 
            this.cbReplaceAll.AutoSize = true;
            this.cbReplaceAll.Location = new System.Drawing.Point(9, 23);
            this.cbReplaceAll.Name = "cbReplaceAll";
            this.cbReplaceAll.Size = new System.Drawing.Size(174, 17);
            this.cbReplaceAll.TabIndex = 24;
            this.cbReplaceAll.Text = "&Remove identical existing types";
            this.ttInfo.SetToolTip(this.cbReplaceAll, "Setting this option will first remove all entries of the same type. For example i" +
        "t would delete all your foundations before importing new ones.");
            this.cbReplaceAll.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(385, 19);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(71, 23);
            this.btnImport.TabIndex = 23;
            this.btnImport.Text = "&Import";
            this.ttInfo.SetToolTip(this.btnImport, "Imports the given items from the clipboard");
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoEllipsis = true;
            this.label4.Location = new System.Drawing.Point(12, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(391, 18);
            this.label4.TabIndex = 21;
            this.label4.Text = "Hover over a text label to see the description";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(409, 198);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(71, 23);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ttInfo
            // 
            this.ttInfo.AutoPopDelay = 60000;
            this.ttInfo.InitialDelay = 500;
            this.ttInfo.ReshowDelay = 100;
            this.ttInfo.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttInfo.ToolTipTitle = "Export/Import Info";
            // 
            // frmExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 233);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.gbImport);
            this.Controls.Add(this.gbExport);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 260);
            this.Name = "frmExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export/Import entries";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmExport_HelpRequested);
            this.gbExport.ResumeLayout(false);
            this.gbExport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).EndInit();
            this.gbImport.ResumeLayout(false);
            this.gbImport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbExport;
        private System.Windows.Forms.GroupBox gbImport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip ttInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbAllItems;
        private System.Windows.Forms.RadioButton rbRange;
        private System.Windows.Forms.NumericUpDown nudCount;
        private System.Windows.Forms.NumericUpDown nudStart;
        private System.Windows.Forms.ComboBox cbItem;
        private System.Windows.Forms.Button btnMap;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.CheckBox cbReplaceAll;
        private System.Windows.Forms.CheckBox cbFixNames;
    }
}