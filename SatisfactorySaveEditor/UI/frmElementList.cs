using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SatisfactorySaveEditor
{
    public partial class frmElementList : Form
    {
        private List<ShortName> Process, Skip;
        private List<SaveFileEntry> Entries;

        /// <summary>
        /// Gets the list of all items to process
        /// </summary>
        public string[] ItemsToProcess
        { get; private set; }

        public frmElementList(IEnumerable<SaveFileEntry> ProcessItems, IEnumerable<SaveFileEntry> SkipItems)
        {
            InitializeComponent();
            EmptyList();
            Thread T = new Thread(delegate ()
            {
                Invoke((MethodInvoker)delegate
                {
                    lvProcess.Items[0].Text = "Creating initial lists 1/3...";
                });
                Log.Write("{0}: Creating list of all items", GetType().Name);
                Entries = ProcessItems
                    .Concat(SkipItems)
                    .Distinct()
                    .ToList();
                Invoke((MethodInvoker)delegate
                {
                    lvProcess.Items.Add("Creating initial lists 2/3...");
                });
                Log.Write("{0}: Creating name list of items that are processed", GetType().Name);
                Process = ProcessItems
                    .Select(m => new ShortName(m.ObjectData.Name))
                    .Distinct()
                    .ToList();
                Invoke((MethodInvoker)delegate
                {
                    lvProcess.Items.Add("Creating initial lists 3/3...");
                });
                Log.Write("{0}: Creating name list of items that are skipped", GetType().Name);
                Skip = SkipItems
                    .Select(m => new ShortName(m.ObjectData.Name))
                    .Distinct()
                    .ToList();
                RenderList();
            });
            T.IsBackground = true;
            T.Start();
            Tools.SetupEscHandler(this);
        }

        /// <summary>
        /// Empties both lists for processing
        /// </summary>
        private void EmptyList()
        {
            lvProcess.Items.Clear();
            lvSkip.Items.Clear();
            lvProcess.Enabled = false;
            lvSkip.Enabled = false;
            lvProcess.Items.Add($"Processing...");
            lvSkip.Items.Add($"Processing...");
        }

        /// <summary>
        /// Renders all items onto the list
        /// </summary>
        private void RenderList()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RenderList);
            }
            else
            {
                EmptyList();
                Thread T = new Thread(delegate ()
                {
                    var SW = new Stopwatch();
                    var ItemsToProcess = new List<ListViewItem>();
                    var ItemsToSkip = new List<ListViewItem>();

                    SW.Start();

                    Log.Write("{0}: Creating {1} UI Process list items", GetType().Name, Process.Count);
                    //Process
                    foreach (var SN in Process)
                    {
                        var Item = new ListViewItem(SN.Short);
                        Item.ToolTipText = SN.Long;
                        Item.Tag = SN;
                        Item.SubItems.Add(Entries.Count(m => m.ObjectData.Name == SN.Long).ToString());
                        ItemsToProcess.Add(Item);
                        if (ShowStatus(SW, lvProcess, ItemsToProcess.Count, Process.Count))
                        {
                            SW.Restart();
                        }
                    }
                    Invoke((MethodInvoker)delegate
                    {
                        lvProcess.Items[0].Text = "Complete";
                    });

                    Log.Write("{0}: Creating {1} UI skip list items", GetType().Name, Skip.Count);
                    //Skip
                    foreach (var SN in Skip)
                    {
                        var Item = new ListViewItem(SN.Short);
                        Item.ToolTipText = SN.Long;
                        Item.Tag = SN;
                        Item.SubItems.Add(Entries.Count(m => m.ObjectData.Name == SN.Long).ToString());
                        ItemsToSkip.Add(Item);
                        if (ShowStatus(SW, lvSkip, ItemsToProcess.Count, Process.Count))
                        {
                            SW.Restart();
                        }
                    }

                    Invoke((MethodInvoker)delegate
                    {
                        lvProcess.Items.Clear();
                        lvSkip.Items.Clear();
                        lvProcess.Enabled = true;
                        lvSkip.Enabled = true;
                        lvProcess.Items.AddRange(ItemsToProcess.ToArray());
                        lvSkip.Items.AddRange(ItemsToSkip.ToArray());
                    });
                });
                T.IsBackground = true;
                T.Start();
            }
        }

        /// <summary>
        /// Shows status in the given list after 500 ms has elapsed
        /// </summary>
        /// <param name="SW">Running stopwatch</param>
        /// <param name="ItemList">Item list to set status of</param>
        /// <param name="Index">Current item index</param>
        /// <param name="Total">Total item count</param>
        /// <returns>true if status was updated</returns>
        private bool ShowStatus(Stopwatch SW, ListView ItemList, int Index, int Total)
        {
            if (SW.ElapsedMilliseconds >= 500)
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        ShowStatus(SW, ItemList, Index, Total);
                    });
                }
                else
                {
                    ItemList.Items[0].Text = $"Processing {Index / Total} items...";
                }
                return true;
            }
            return false;
        }

        private void lvSkip_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (lvSkip.SelectedItems.Count > 0)
                {
                    var Items = lvSkip.SelectedItems.OfType<ListViewItem>().ToArray();
                    foreach (var I in Items)
                    {
                        lvSkip.Items.Remove(I);
                    }
                    lvProcess.Items.AddRange(Items);
                }
            }
            if (e.KeyCode == Keys.A && e.Control)
            {
                lvSkip.Items.OfType<ListViewItem>().All(m => m.Selected = true);
            }
        }

        private void lvProcess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (lvProcess.SelectedItems.Count > 0)
                {
                    var Items = lvProcess.SelectedItems.OfType<ListViewItem>().ToArray();
                    foreach (var I in Items)
                    {
                        lvProcess.Items.Remove(I);
                    }
                    lvSkip.Items.AddRange(Items);
                }
            }
            if (e.KeyCode == Keys.A && e.Control)
            {
                lvProcess.Items.OfType<ListViewItem>().All(m => m.Selected = true);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ItemsToProcess = lvProcess.Items
                .OfType<ListViewItem>()
                .Select(m => ((ShortName)m.Tag).Long)
                .ToArray();
            Log.Write("{0}: Total items selected: {1}", GetType().Name, ItemsToProcess.Length);
            DialogResult = ItemsToProcess.Length > 0 ? DialogResult.OK : DialogResult.Cancel;
            Close();
        }

        private void frmElementList_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Tools.ShowHelp(GetType().Name);
        }
    }
}
