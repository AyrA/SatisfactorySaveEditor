using System;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmSettings : Form
    {
        private Settings CurrentSettings;

        public frmSettings(Settings S)
        {
            CurrentSettings = S;
            InitializeComponent();
            Tools.SetupKeyHandlers(this);
            SetUiValues();
        }

        /// <summary>
        /// Sets the UI values to match the current settings
        /// </summary>
        private void SetUiValues()
        {
            cbAutoUpdate.Checked = CurrentSettings.AutoUpdate;
            cbShowChangeLog.Checked = CurrentSettings.ShowChangelog;
        }

        private void frmSettings_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }

        private void btnMessageHide_Click(object sender, EventArgs e)
        {
            CurrentSettings.MarkDialogsRead(true);
        }

        private void btnMessageShow_Click(object sender, EventArgs e)
        {
            CurrentSettings.MarkDialogsRead(false);
        }

        private void cbAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            CurrentSettings.AutoUpdate = cbAutoUpdate.Checked;
        }

        private void cbShowChangeLog_CheckedChanged(object sender, EventArgs e)
        {
            CurrentSettings.ShowChangelog = cbShowChangeLog.Checked;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
