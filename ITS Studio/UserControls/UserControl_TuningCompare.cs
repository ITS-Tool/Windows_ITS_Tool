using System;
using System.Windows.Forms;

namespace ITS_Studio.UserControls
{
    public partial class Ctrl_TuningCompare : UserControl
    {
        private const int _VALUE_COLUMN_ = 6;

        public enum EnumAlignSide { _RIGHT_, _LEFT_ };

        private EnumAlignSide AlignSide;

        public event EventHandler CopySingleEvent;

        public event EventHandler CopyAllEvent;

        public event EventHandler ReloadEvent;

        private int mouseClickRowIndex;

        public int RowIndex
        {
            get
            {
                return mouseClickRowIndex;
            }
        }

        private object menuClickSender;

        public object MenuClickSender
        {
            get
            {
                return menuClickSender;
            }
        }

        public Ctrl_TuningCompare(EnumAlignSide UserAlignSide)
        {
            AlignSide = UserAlignSide;
            InitializeComponent();
        }

        private void DGV_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView DGV = (DataGridView)sender;
                DataGridView.HitTestInfo hit = DGV.HitTest(e.X, e.Y);
                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    DGV.ClearSelection();
                    DGV.CurrentCell = DGV.Rows[hit.RowIndex].Cells[5];
                    DGV.Rows[hit.RowIndex].Cells[5].Selected = true;

                    ContextMenu m = new ContextMenu();
                    if (AlignSide == EnumAlignSide._LEFT_)
                    {
                        m.MenuItems.Add(new MenuItem("Copy to Right"));
                        m.MenuItems.Add(new MenuItem("Copy All to Right"));
                        m.MenuItems.Add(new MenuItem("Compare Again"));
                    }
                    else
                    {
                        m.MenuItems.Add(new MenuItem("Copy to Left"));
                        m.MenuItems.Add(new MenuItem("Copy All to Left"));
                        m.MenuItems.Add(new MenuItem("Compare Again"));
                    }
                    m.MenuItems[0].Click += UserControl1_CopySingle_Click;
                    m.MenuItems[1].Click += UserControl1_CopyAll_Click;
                    m.MenuItems[2].Click += UserControl1_Reload_Click;
                    mouseClickRowIndex = ((DataGridView)sender).HitTest(e.X, e.Y).RowIndex;
                    menuClickSender = sender;
                    m.Show((DataGridView)sender, e.Location);
                }
            }
        }

        private void UserControl1_CopySingle_Click(object sender, EventArgs e)
        {
            if (CopySingleEvent != null)
            {
                CopySingleEvent(sender, e);
            }
            menuClickSender = null;
        }

        private void UserControl1_CopyAll_Click(object sender, EventArgs e)
        {
            if (CopyAllEvent != null)
            {
                CopyAllEvent(sender, e);
            }
            menuClickSender = null;
        }

        private void UserControl1_Reload_Click(object sender, EventArgs e)
        {
            if (ReloadEvent != null)
            {
                ReloadEvent(sender, e);
            }
        }

        private void DGV_List_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // MessageBox.Show("Error happened " + e.Context.ToString());

            if (e.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (e.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (e.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (e.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((e.Exception) is System.Data.ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[e.RowIndex].ErrorText = "an error";
                view.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "an error";

                e.ThrowException = false;
            }
            //StaticVar.LogWriteLine(e.Exception);
        }
    }
}