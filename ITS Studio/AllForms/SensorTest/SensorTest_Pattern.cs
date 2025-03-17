using System.Drawing;
using System.Windows.Forms;

namespace ITS_Studio.AllForms.SensorTest
{
    public partial class SensorTest_Pattern : Form
    {
        public SensorTest_Pattern()
        {
            InitializeComponent();
        }
        public void SetLabelMiddle()
        {
            lb_Data.Location = new Point((this.Width / 2) - lb_Data.Width / 2, this.Height / 2);
        }
    }
}
