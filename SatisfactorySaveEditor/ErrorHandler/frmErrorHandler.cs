using System;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    internal partial class frmErrorHandler : Form
    {
        private bool reported;
        private ErrorHandler Handler;
        private Exception SourceException;

        public frmErrorHandler(Exception E, ErrorHandler EH)
        {
            InitializeComponent();
            reported = false;
            Handler = EH;
            SourceException = E;
            tbReport.Text = ErrorHandler.generateReport(SourceException);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (!reported)
            {
                switch (MessageBox.Show("You have not yet reported the error. This makes it hard to improve the application. Do you want to report it before terminating the application?", "Unsent report", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.No:
                        Close();
                        break;
                    case DialogResult.Yes:
                        if (report())
                        {
                            Close();
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Close();
            }
        }

        private bool report()
        {
            if (reported)
            {
                MessageBox.Show("You have already reported the error", "Error reporting", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (Handler.raise(SourceException))
                {
                    reported = true;
                }
            }
            return reported;
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            var sent = report();
            btnReport.Enabled = !sent;
            if (!sent)
            {
                Tools.E($"Unable to send error report.\r\nDetails: {Handler.LastReportResult}", "Errror Report");
            }
        }
    }
}
