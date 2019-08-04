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
        }
    }
}
