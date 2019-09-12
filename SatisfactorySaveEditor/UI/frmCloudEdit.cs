using System;
using System.Windows.Forms;
using SMRAPI;
using SMRAPI.Responses;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace SatisfactorySaveEditor
{
    public partial class frmCloudEdit : Form
    {
        InfoResponse.map CurrentMap = null;
        InfoResponse.DataValue.CategoriesResult Categories = null;

        public frmCloudEdit(InfoResponse.map Map)
        {
            CurrentMap = Map;
            InitializeComponent();
            Tools.SetupKeyHandlers(this);
            tbFileName.Text = Map.name;
            tbDescription.Text = Map.description;
            tbHiddenId.Text = Map.hidden_id.ToString();
            cbPublic.Checked = Map.@public != 0;
            LoadCategories();
        }

        private void LoadCategories()
        {
            Thread T = new Thread(delegate ()
            {
                try
                {
                    Categories = API.Info().data.categories;
                }
                catch (Exception ex)
                {
                    Tools.E("Unable to get the categories from the API.\r\n" + ex.Message, "Cloud File Edit", this);
                }
                if (Categories != null)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        var Boxes = new List<CheckBox>();
                        foreach (var Cat in Categories.list)
                        {
                            var Conflict = Categories.conflicts.FirstOrDefault(m => m.Category == Cat.id);
                            if (Conflict != null && !Conflict.IsDisabled)
                            {
                                var Active = CurrentMap.categories != null &&
                                    CurrentMap.categories.category != null &&
                                    CurrentMap.categories.category.Any(m => m == Cat.id);
                                var box = new CheckBox();
                                box.Size = new System.Drawing.Size(150, 25);
                                box.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                                box.Text = Cat.name;
                                box.Tag = Cat.id;
                                box.Checked = Active;
                                Boxes.Add(box);
                                ttMain.SetToolTip(box, Cat.description);
                            }
                        }
                        panelCategories.Controls.Clear();
                        panelCategories.Controls.AddRange(Boxes.ToArray());
                        ApplyCategoryMap();
                        //Attach events and render boxes.
                        foreach (var Box in Boxes)
                        {
                            Box.CheckedChanged += CategoryChangeHandler;
                        }
                    });
                }
            });
            T.IsBackground = true;
            T.Start();
        }

        private void ApplyCategoryMap()
        {
            SuspendLayout();
            bool HadChange;
            //All category checkboxes
            var AllBoxes = panelCategories.Controls
                .OfType<CheckBox>()
                .ToArray();
            //Unregister events temporarily
            foreach (var Box in AllBoxes)
            {
                Box.CheckedChanged -= CategoryChangeHandler;
            }
            do
            {
                HadChange = false;
                //All checkboxes that are active
                var ActiveBoxes = AllBoxes
                    .Where(m => m.Checked)
                    .ToArray();
                //All ids of active checkboxes
                var ActiveIds = ActiveBoxes
                    .Select(m => (int)m.Tag)
                    .ToArray();
                //All conflicts of checkboxes
                var Conflicts = Categories.conflicts
                    .Where(m => ActiveIds.Contains(m.Category))
                    .SelectMany(m => m.conflicts)
                    .Distinct();
                foreach (var Box in AllBoxes)
                {
                    var IsConflict = Conflicts.Contains((int)Box.Tag);
                    if (IsConflict && (Box.Enabled || Box.Checked))
                    {
                        Box.Checked = Box.Enabled = false;
                        HadChange = true;
                    }
                    else if (!IsConflict && !Box.Enabled)
                    {
                        Box.Enabled = true;
                        Box.Checked = false;
                    }
                }
                foreach (var Id in Conflicts)
                {
                    //Get box of that category
                    var Box = AllBoxes.FirstOrDefault(m => (int)m.Tag == Id);

                }

            } while (HadChange);
            //Add events back
            foreach (var Box in AllBoxes)
            {
                Box.CheckedChanged += CategoryChangeHandler;
            }
            ResumeLayout();
        }

        private void CategoryChangeHandler(object sender, EventArgs e)
        {
            ApplyCategoryMap();
            if (!(sender is CheckBox))
            {
                return;
            }
            CheckBox Current = (CheckBox)sender;
            var Cat = (int)Current.Tag;
        }

        private void frmCloudEdit_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnNewId_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Generate a new hidden id? This invalidates the current id immediately regardless of other pending changes in this window.", "Cloud File Edit", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                try
                {
                    var Res = API.NewId(CurrentMap.id);
                    if (Res.success)
                    {
                        CurrentMap.hidden_id = Res.data;
                        tbHiddenId.Text = Res.data.ToString();
                    }
                    else
                    {
                        throw new Exception(Res.msg);
                    }
                }
                catch (Exception ex)
                {
                    Tools.E("Unable to change the id.\r\n" + ex.Message, "Cloud File Edit", this);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var Categories = panelCategories.Controls
                .OfType<CheckBox>()
                .Where(m => m.Checked)
                .Select(m => (int)m.Tag)
                .ToArray();
            Thread T = new Thread(delegate ()
            {
                try
                {
                    var Result = API.EditMap(CurrentMap.id, tbFileName.Text, tbDescription.Text, Categories, cbPublic.Checked ? 1 : 0);
                    if (!Result.success)
                    {
                        throw new Exception(Result.msg);
                    }
                    Invoke((MethodInvoker)delegate
                    {
                        DialogResult = DialogResult.OK;
                    });
                }
                catch (Exception ex)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        btnCancel.Enabled = btnOK.Enabled = true;
                        Tools.E("Error editing the map.\r\n" + ex.Message, "Cloud File Edit", this);
                    });
                }
            });
            btnCancel.Enabled = btnOK.Enabled = false;
            T.IsBackground = true;
            T.Start();
        }
    }
}
