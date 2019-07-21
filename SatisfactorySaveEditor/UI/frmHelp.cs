using System.Drawing;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmHelp : Form
    {
        public string HelpText
        {
            get
            {
                return tbHelp.Text;
            }
            set
            {
                Log.Write("{0}: Updating help text", GetType().Name);
                tbHelp.Text = value;
            }
        }

        public frmHelp()
        {
            InitializeComponent();

            //Increase from the default font size because this might contain lots of text
            var F = new Font(Font.FontFamily, Font.Size * 1.5f);
            Font = F;
            Log.Write("{0}: Form created", GetType().Name);
            Tools.SetupEscHandler(this);
        }

        private void frmHelp_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }
    }
}
