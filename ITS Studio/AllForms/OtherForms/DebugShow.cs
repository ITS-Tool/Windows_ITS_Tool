using System.Data;
using System.Windows.Forms;

namespace ITS_Studio.AllForms
{
    public partial class DebugShow : Form
    {
        public DebugShow()
        {
            InitializeComponent();
        }

        public static void WantToSeeDatatable(DataTable inDatatable)
        {
            DebugShow MyForm = new DebugShow();
            DataGridView DView = new DataGridView();
            DView.DataSource = inDatatable;
            DView.Parent = MyForm;
            DView.Visible = true;
            DView.Dock = DockStyle.Fill;
            MyForm.ShowDialog();
        }

        public static void WantToSeeDataGridview(DataGridView inDataGridview)
        {
            DebugShow MyForm = new DebugShow();
            //DataGridView DView = new DataGridView();
            //DView.DataSource = inDataGridview.DataSource;
            inDataGridview.Parent = MyForm;
            inDataGridview.Visible = true;
            inDataGridview.Dock = DockStyle.Fill;
            MyForm.ShowDialog();
        }
    }
}