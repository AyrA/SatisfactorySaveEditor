namespace SatisfactorySaveEditor
{
    partial class frmDuplicator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDuplicator));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbObject = new System.Windows.Forms.ComboBox();
            this.nudCount = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudOffset = new System.Windows.Forms.NumericUpDown();
            this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbApplyOffset = new System.Windows.Forms.CheckBox();
            this.btnMap = new System.Windows.Forms.Button();
            this.nudOffsetX = new System.Windows.Forms.NumericUpDown();
            this.nudOffsetY = new System.Windows.Forms.NumericUpDown();
            this.nudOffsetZ = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffsetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffsetZ)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(304, 89);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(385, 89);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Object";
            this.ttInfo.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(382, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Copies";
            this.ttInfo.SetToolTip(this.label2, "Defines the number of copies");
            // 
            // cbObject
            // 
            this.cbObject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObject.FormattingEnabled = true;
            this.cbObject.Location = new System.Drawing.Point(15, 29);
            this.cbObject.Name = "cbObject";
            this.cbObject.Size = new System.Drawing.Size(254, 21);
            this.cbObject.TabIndex = 1;
            this.cbObject.SelectedIndexChanged += new System.EventHandler(this.cbObject_SelectedIndexChanged);
            // 
            // nudCount
            // 
            this.nudCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudCount.Location = new System.Drawing.Point(385, 29);
            this.nudCount.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCount.Name = "nudCount";
            this.nudCount.Size = new System.Drawing.Size(75, 20);
            this.nudCount.TabIndex = 6;
            this.nudCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(301, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Offset";
            this.ttInfo.SetToolTip(this.label3, "You can use this box to select which entry to copy when there are multiple entrie" +
        "s of the same type.\r\n1 is the first entry.");
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoEllipsis = true;
            this.label4.Location = new System.Drawing.Point(15, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(283, 18);
            this.label4.TabIndex = 14;
            this.label4.Text = "Hover over a text label to see the description";
            // 
            // nudOffset
            // 
            this.nudOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudOffset.Location = new System.Drawing.Point(304, 29);
            this.nudOffset.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOffset.Name = "nudOffset";
            this.nudOffset.Size = new System.Drawing.Size(75, 20);
            this.nudOffset.TabIndex = 4;
            this.nudOffset.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ttInfo
            // 
            this.ttInfo.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttInfo.ToolTipTitle = "Object Duplicator Info";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(99, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "X";
            this.ttInfo.SetToolTip(this.label6, "X is the East-West direction. Bigger means more towards East");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(210, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Y";
            this.ttInfo.SetToolTip(this.label7, "Y is the North-South direction. Bigger means more towards South");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(324, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Z";
            this.ttInfo.SetToolTip(this.label8, "Z is the Up-Down direction. Bigger means more towards the Sky");
            // 
            // cbApplyOffset
            // 
            this.cbApplyOffset.AutoSize = true;
            this.cbApplyOffset.Location = new System.Drawing.Point(15, 63);
            this.cbApplyOffset.Name = "cbApplyOffset";
            this.cbApplyOffset.Size = new System.Drawing.Size(78, 17);
            this.cbApplyOffset.TabIndex = 7;
            this.cbApplyOffset.Text = "Map Offset";
            this.ttInfo.SetToolTip(this.cbApplyOffset, "If enabled, each copy will be offset by the give number from the previous. 100 is" +
        " 1 meter, so a foundation is 800");
            this.cbApplyOffset.UseVisualStyleBackColor = true;
            this.cbApplyOffset.CheckedChanged += new System.EventHandler(this.cbApplyOffset_CheckedChanged);
            // 
            // btnMap
            // 
            this.btnMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMap.Location = new System.Drawing.Point(275, 28);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(23, 23);
            this.btnMap.TabIndex = 2;
            this.btnMap.Text = "M";
            this.ttInfo.SetToolTip(this.btnMap, "Renders the currently selected objects into the main window map");
            this.btnMap.UseVisualStyleBackColor = true;
            this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
            // 
            // nudOffsetX
            // 
            this.nudOffsetX.Enabled = false;
            this.nudOffsetX.Location = new System.Drawing.Point(119, 61);
            this.nudOffsetX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudOffsetX.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudOffsetX.Name = "nudOffsetX";
            this.nudOffsetX.Size = new System.Drawing.Size(75, 20);
            this.nudOffsetX.TabIndex = 9;
            this.nudOffsetX.ThousandsSeparator = true;
            // 
            // nudOffsetY
            // 
            this.nudOffsetY.Enabled = false;
            this.nudOffsetY.Location = new System.Drawing.Point(230, 61);
            this.nudOffsetY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudOffsetY.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudOffsetY.Name = "nudOffsetY";
            this.nudOffsetY.Size = new System.Drawing.Size(75, 20);
            this.nudOffsetY.TabIndex = 11;
            this.nudOffsetY.ThousandsSeparator = true;
            // 
            // nudOffsetZ
            // 
            this.nudOffsetZ.Enabled = false;
            this.nudOffsetZ.Location = new System.Drawing.Point(345, 61);
            this.nudOffsetZ.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudOffsetZ.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudOffsetZ.Name = "nudOffsetZ";
            this.nudOffsetZ.Size = new System.Drawing.Size(75, 20);
            this.nudOffsetZ.TabIndex = 13;
            this.nudOffsetZ.ThousandsSeparator = true;
            // 
            // frmDuplicator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 123);
            this.Controls.Add(this.btnMap);
            this.Controls.Add(this.cbApplyOffset);
            this.Controls.Add(this.nudOffsetZ);
            this.Controls.Add(this.nudOffsetY);
            this.Controls.Add(this.nudOffsetX);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudOffset);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudCount);
            this.Controls.Add(this.cbObject);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 150);
            this.Name = "frmDuplicator";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Object Duplicator";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmDuplicator_HelpRequested);
            ((System.ComponentModel.ISupportInitialize)(this.nudCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffsetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffsetZ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbObject;
        private System.Windows.Forms.NumericUpDown nudCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudOffset;
        private System.Windows.Forms.ToolTip ttInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudOffsetX;
        private System.Windows.Forms.NumericUpDown nudOffsetY;
        private System.Windows.Forms.NumericUpDown nudOffsetZ;
        private System.Windows.Forms.CheckBox cbApplyOffset;
        private System.Windows.Forms.Button btnMap;
    }
}