using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmRename : Form
    {
        private char[] Invalids;

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
            Invalids = Path.GetInvalidFileNameChars()
                .Concat(Path.GetInvalidPathChars())
                .Where(m => m > 0)
                .Distinct()
                .ToArray();
            InitializeComponent();
            MaximumSize = new Size(int.MaxValue, MinimumSize.Height);
            tbSessionName.Text = SessionName;
            tbFileName.Text = FileName;
            SetBtn();
        }

        private void SetBtn()
        {
            var BaseState = true;
            if (string.IsNullOrWhiteSpace(tbSessionName.Text))
            {
                BaseState = false;
            }
            if (string.IsNullOrWhiteSpace(tbFileName.Text))
            {
                BaseState = false;
            }
            if (Path.GetInvalidFileNameChars().Any(m => tbSessionName.Text.Contains(m)))
            {
                BaseState = false;
            }
            if (Path.GetInvalidFileNameChars().Any(m => tbFileName.Text.Contains(m)))
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
            MessageBox.Show($@"Enter a valid session and file name to continue

Valid names do not contain any of these characters:

{new string(Invalids)}", "Rename Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
