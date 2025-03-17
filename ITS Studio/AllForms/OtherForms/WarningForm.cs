using System;
using System.Drawing;
using System.Windows.Forms;

namespace ITS_Studio.AllForms
{
    public struct _FormInfo
    {
        public string strTitle { get; set; }
        public string strShowMessage { get; set; }
        public int? iBlinkTime { get; set; }
    };

    public partial class WarningForm : Form
    {
        private _FormInfo FormInfo = new _FormInfo();

        public WarningForm(string strTitle, string strMsg, int? iBlink)
        {
            FormInfo.strTitle = strTitle;
            FormInfo.iBlinkTime = iBlink;
            FormInfo.strShowMessage = strMsg;
            InitializeComponent();
        }

        public WarningForm(string strTitle, string strMsg)
            : this(strTitle, strMsg, null)
        { }

        public WarningForm(string strMsg)
            : this(null, strMsg, null)
        { }

        private Font FindFont(Graphics g, string longString, Size Room, Font PreferedFont)
        {
            // you should perform some scale functions!!!
            SizeF RealSize = g.MeasureString(longString, PreferedFont);
            float HeightScaleRatio = Room.Height / RealSize.Height;
            float WidthScaleRatio = Room.Width / RealSize.Width;

            float ScaleRatio = (HeightScaleRatio < WidthScaleRatio)
               ? ScaleRatio = HeightScaleRatio
               : ScaleRatio = WidthScaleRatio;

            float ScaleFontSize = PreferedFont.Size * ScaleRatio;

            return new Font(PreferedFont.FontFamily, (int)ScaleFontSize, FontStyle.Bold);
        }

        private void WarningForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormInfo.strTitle))
            {
                toolStripStatusLabel1.Visible = false;
                toolStripStatusLabel1.Text = "";
            }
            else
            {
                toolStripStatusLabel1.Visible = true;
                toolStripStatusLabel1.Text = FormInfo.strTitle;
            }
            label1.Text = FormInfo.strShowMessage;
            Font arialBold = new Font("Consolas", 72.0F, FontStyle.Bold);
            Rectangle rect1 = new Rectangle(label1.Location.X, label1.Location.Y
                , label1.Width, label1.Height);
            Font goodFont = FindFont(Graphics.FromHwnd(this.label1.Handle), label1.Text, rect1.Size, arialBold);
            label1.Font = goodFont;
            timer_Blink.Start();
        }

        private void WarningForm_Shown(object sender, EventArgs e)
        {
            int iInterval = FormInfo.iBlinkTime == null ? 1000 : (int)(FormInfo.iBlinkTime);
            timer_Blink.Interval = iInterval;
            timer_Blink.Enabled = true;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            timer_Blink.Stop();
            this.Close();
        }

        private void timer_Blink_Tick(object sender, EventArgs e)
        {
            if (label1.ForeColor == Color.LightSkyBlue)
                label1.ForeColor = Color.SpringGreen;
            else
                label1.ForeColor = Color.LightSkyBlue;
        }
    }
}