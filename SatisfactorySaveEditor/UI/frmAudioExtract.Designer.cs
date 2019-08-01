namespace SatisfactorySaveEditor
{
    partial class frmAudioExtract
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAudioExtract));
            this.label1 = new System.Windows.Forms.Label();
            this.tbResourceFile = new System.Windows.Forms.TextBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.pbFilePos = new System.Windows.Forms.ProgressBar();
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.gbAudio = new System.Windows.Forms.GroupBox();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnOpenDir = new System.Windows.Forms.Button();
            this.gbAudio.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Resource File";
            // 
            // tbResourceFile
            // 
            this.tbResourceFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResourceFile.Location = new System.Drawing.Point(90, 14);
            this.tbResourceFile.Name = "tbResourceFile";
            this.tbResourceFile.Size = new System.Drawing.Size(509, 20);
            this.tbResourceFile.TabIndex = 1;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFile.Location = new System.Drawing.Point(605, 12);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "Select...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnScan
            // 
            this.btnScan.Enabled = false;
            this.btnScan.Location = new System.Drawing.Point(12, 62);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(72, 23);
            this.btnScan.TabIndex = 3;
            this.btnScan.Text = "&Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // pbFilePos
            // 
            this.pbFilePos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbFilePos.Location = new System.Drawing.Point(90, 62);
            this.pbFilePos.Name = "pbFilePos";
            this.pbFilePos.Size = new System.Drawing.Size(590, 23);
            this.pbFilePos.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbFilePos.TabIndex = 4;
            // 
            // OFD
            // 
            this.OFD.Title = "Satisfactory Game File";
            // 
            // gbAudio
            // 
            this.gbAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAudio.Controls.Add(this.btnOpenDir);
            this.gbAudio.Controls.Add(this.btnPrev);
            this.gbAudio.Controls.Add(this.btnStart);
            this.gbAudio.Controls.Add(this.btnStop);
            this.gbAudio.Controls.Add(this.btnNext);
            this.gbAudio.Enabled = false;
            this.gbAudio.Location = new System.Drawing.Point(12, 99);
            this.gbAudio.Name = "gbAudio";
            this.gbAudio.Size = new System.Drawing.Size(668, 62);
            this.gbAudio.TabIndex = 5;
            this.gbAudio.TabStop = false;
            this.gbAudio.Text = "Audio Player";
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(9, 21);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(75, 23);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "&<<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(90, 21);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "&Play";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(171, 21);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "&Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(252, 21);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "&>>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnOpenDir
            // 
            this.btnOpenDir.Location = new System.Drawing.Point(333, 21);
            this.btnOpenDir.Name = "btnOpenDir";
            this.btnOpenDir.Size = new System.Drawing.Size(75, 23);
            this.btnOpenDir.TabIndex = 4;
            this.btnOpenDir.Text = "&Open Dir";
            this.btnOpenDir.UseVisualStyleBackColor = true;
            this.btnOpenDir.Click += new System.EventHandler(this.btnOpenDir_Click);
            // 
            // frmAudioExtract
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 173);
            this.Controls.Add(this.gbAudio);
            this.Controls.Add(this.pbFilePos);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.tbResourceFile);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 200);
            this.Name = "frmAudioExtract";
            this.Text = "Audio Extractor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAudioExtract_FormClosing);
            this.Load += new System.EventHandler(this.frmAudioExtract_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmAudioExtract_HelpRequested);
            this.gbAudio.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbResourceFile;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.ProgressBar pbFilePos;
        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.GroupBox gbAudio;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnOpenDir;
    }
}