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
                tbHelp.Text = value;
            }
        }

        public frmHelp()
        {
            InitializeComponent();

            //Increase from the default font size because this might contain lots of text
            var F = new Font(Font.FontFamily, Font.Size * 1.5f);
            Font = F;
        }
    }
}
