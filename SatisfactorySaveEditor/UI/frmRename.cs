using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmRename : Form
    {
        public string RenameSessionName
        {
            get
            {
                return IsDisposed ? null : tbSessionName.Text;
            }
        }
        public string RenameFileName
        {
            get
            {
                return IsDisposed ? null : tbFileName.Text;
            }
        }

        public frmRename(string SessionName, string FileName)
        {
            InitializeComponent();
            MaximumSize = new Size(int.MaxValue, MinimumSize.Height);
            tbSessionName.Text = SessionName;
            tbFileName.Text = FileName;
            SetBtn();
            Tools.SetupKeyHandlers(this);
        }

        private void SetBtn()
        {
            var BaseState = true;
            if (BaseState && string.IsNullOrWhiteSpace(tbSessionName.Text))
            {
                BaseState = false;
            }
            if (BaseState && string.IsNullOrWhiteSpace(tbFileName.Text))
            {
                BaseState = false;
            }
            if (BaseState && Path.GetInvalidFileNameChars().Any(m => tbSessionName.Text.Contains(m)))
            {
                BaseState = false;
            }
            if (BaseState && Path.GetInvalidFileNameChars().Any(m => tbFileName.Text.Contains(m)))
            {
                BaseState = false;
            }
            if (btnOK.Enabled != BaseState)
            {
                btnOK.Enabled = BaseState;
            }
        }

        private void tbSessionName_TextChanged(object sender, EventArgs e)
        {
            SetBtn();
        }

        private void tbFileName_TextChanged(object sender, EventArgs e)
        {
            SetBtn();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Tools.ShowHelp(GetType().Name);
        }

        private void frmRename_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }
    }
}
