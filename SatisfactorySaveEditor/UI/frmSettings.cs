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
            cbAutostartManager.Checked = CurrentSettings.AutostartManager;
            cbAutoUpdate.Checked = CurrentSettings.AutoUpdate;
            cbShowChangeLog.Checked = CurrentSettings.ShowChangelog;
            cbRandom.Checked = CurrentSettings.UseRandomId;
            cbStopReporting.Checked = CurrentSettings.DisableUsageReport;
            lblId.Text = CurrentSettings.DisableUsageReport ? Guid.Empty.ToString() : CurrentSettings.ReportId.ToString();
        }

        private void frmSettings_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }

        #region Settings

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

        private void cbStopReporting_CheckedChanged(object sender, EventArgs e)
        {
            cbRandom.Enabled = !cbStopReporting.Checked;
        }

        private void cbAutostartManager_CheckedChanged(object sender, EventArgs e)
        {
            CurrentSettings.AutostartManager = cbAutostartManager.Checked;
        }

        private void lblId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!CurrentSettings.DisableUsageReport)
            {
                if (CurrentSettings.UseRandomId)
                {
                    Tools.E("You can't view your usage report if you enable the random id feature because there will not be a report yet for the displayed id", "Usage Report");
                }
                else
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        System.Diagnostics.Process.Start(FeatureReport.REPORT_URL + CurrentSettings.ReportId.ToString());
                    }
                    if (e.Button == MouseButtons.Right)
                    {
                        if (MessageBox.Show(@"You will lose access to your old statistics if you change the id without writing the old one down first.

Really change your Id?", "New Id", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            FeatureReport.Id = CurrentSettings.ReportId = Guid.NewGuid();
                            SetUiValues();
                        }
                    }
                }
            }
            else
            {
                Tools.E("Usage reporting is currently disabled", "Usage Report");
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            //Disregard user random Id setting if reports are disabled
            CurrentSettings.UseRandomId = cbRandom.Checked && !cbStopReporting.Checked;
            //User disabled reports
            if (!CurrentSettings.DisableUsageReport && cbStopReporting.Checked)
            {
                FeatureReport.Used(FeatureReport.Feature.DisableReport);
                FeatureReport.Report();
                CurrentSettings.ReportId = FeatureReport.Id = Guid.Empty;
            }
            else if (CurrentSettings.ReportId == Guid.Empty && !cbStopReporting.Checked)
            {
                //Generate a new Id if reports are enabled and no Id is present
                CurrentSettings.ReportId = FeatureReport.Id = Guid.NewGuid();
            }
            Close();
        }
    }
}
