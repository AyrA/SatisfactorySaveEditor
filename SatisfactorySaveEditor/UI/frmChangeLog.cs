using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmChangeLog : Form
    {
        public frmChangeLog()
        {
            InitializeComponent();
            cbVersion.Items.AddRange(Tools.GetVersionFiles().OrderByDescending(m => m).Cast<object>().ToArray());
            cbVersion.SelectedIndex = 0;
            Tools.SetupKeyHandlers(this);
        }

        private void cbVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbVersion.SelectedIndex >= 0)
            {
                tbVersion.Text = Tools.GetVersionFile((string)cbVersion.SelectedItem);
            }
        }

        private void frmChangeLog_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(nameof(frmChangeLog));
        }
    }
}
