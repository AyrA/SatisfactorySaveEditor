using System;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmHeaderEditor : Form
    {
        public string SessionName { get; private set; }
        public string PlayTime { get; private set; }

        public frmHeaderEditor(SaveFile F)
        {
            InitializeComponent();
            tbSessionName.Text = F.SessionName;
            tbPlayTime.Text = F.PlayTime.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tbSessionName.Text))
            {
                MessageBox.Show("Please enter a session name", "Invalid session name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if(!IsValidTime(tbPlayTime.Text))
            {
                MessageBox.Show("Please enter a valid play time in hh:mm:ss format", "Invalid game time", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SessionName = tbSessionName.Text;
            PlayTime = tbPlayTime.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool IsValidTime(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                TimeSpan T;
                return TimeSpan.TryParse(text, out T) && T.TotalMilliseconds >= 0.0;
            }
            return false;
        }
    }
}
