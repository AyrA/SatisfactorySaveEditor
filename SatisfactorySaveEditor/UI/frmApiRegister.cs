using System;
using System.Drawing;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmApiRegister : Form
    {
        private Settings S;
        private SMRAPI.HTTP Server = null;

        public frmApiRegister(Settings CurrentSettings)
        {
            S = CurrentSettings;
            InitializeComponent();
            MaximumSize = new Size(int.MaxValue, MinimumSize.Height);
            if (S.ApiKey != Guid.Empty)
            {
                tbKey.Text = S.ApiKey.ToString();
            }
            Tools.SetupKeyHandlers(this);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (Server == null)
            {
                Server = new SMRAPI.HTTP(Tools.GetRandom(5000, 50000));
                Server.Start();
                Server.ApiKeyEvent += delegate (object source, Guid Key)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        if (Key == Guid.Empty)
                        {
                            tbKey.Text = string.Empty;
                        }
                        else
                        {
                            tbKey.Text = Key.ToString();
                        }
                    });
                };
            }
            Server.OpenBrowser();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Guid TempKey;
            DialogResult = DialogResult.OK;
            if (string.IsNullOrEmpty(tbKey.Text))
            {
                S.ApiKey = Guid.Empty;
            }
            else if (!Guid.TryParse(tbKey.Text, out TempKey))
            {
                Log.Write($"Invalid Api Key in Text box: {tbKey.Text}");
                Tools.E("The entered API key is invalid. Please double check or use the button to obtain it automatically", Text);
                DialogResult = DialogResult.None;
            }
            else
            {
                var OldApiKey = SMRAPI.API.ApiKey;
                try
                {
                    SMRAPI.API.ApiKey = TempKey;
                    var I = SMRAPI.API.Info();
                    if (I.success)
                    {
                        S.ApiKey = TempKey;
                    }
                    else
                    {
                        throw new Exception(I.msg);
                    }
                }
                catch (Exception ex)
                {
                    SMRAPI.API.ApiKey = OldApiKey;
                    Tools.E("Error checking key:\r\n" + ex.Message, ex.GetType().Name);
                    DialogResult = DialogResult.None;
                }
            }
        }

        private void frmApiRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Server != null)
            {
                Server.Dispose();
                Server = null;
            }
        }

        private void frmApiRegister_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }
    }
}
