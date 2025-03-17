using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Sunny.UI;

namespace ITS_Studio.UserControls
{

    public partial class UserControl_LabelTextBox : UserControl
    {
        public UserControl_LabelTextBox()
        {
            InitializeComponent();
        }

        private Font _CustomFont = DefaultFont;
        [Browsable(true), Category("自訂屬性"), DefaultValue("1.0"), Description("字型大小")]
        public override Font Font
        {
            set
            {
                _CustomFont = value;
                uiPanel1.Font = _CustomFont;
            }
            get
            {
                return _CustomFont;
            }
        }

        private UIStyle _UIStyle = UIStyle.Orange;
        [Browsable(true), Category("自訂屬性")]
        public new UIStyle SetStyle
        {
            set
            {
                _UIStyle = value;
                this.uiPanel1.Style = _UIStyle;
            }
            get
            {
                return _UIStyle;
            }
        }

        public string LabelText
        {
            set
            {
                this.uiLabel1.Text = value;
            }
            get
            {
                return this.uiLabel1.Text;
            }
        }

        public int NumberText
        {
            set
            {
                this.uiLedLabel1.Text = (value).ToString();
            }
            get
            {
                return int.Parse(this.uiLedLabel1.Text);
            }
        }
    }
}
