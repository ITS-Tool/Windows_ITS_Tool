using System;
using System.Drawing;
using System.Windows.Forms;
using CommonExt;
using Sunny.UI;

namespace ITS_Studio
{
    public partial class FormShowAskDialog : Form
    {
        private FormShowAskDialog()
        {
            InitializeComponent();
        }

        private FormShowAskDialog(Form parentForm, string title, string msg, UIStyle _UIStyle, bool showMask = false)
        {
            InitializeComponent();
            uiTitlePanel1.Style = _UIStyle;
            uiLabel1.Style = _UIStyle;
            uiPanel1.Style = _UIStyle;
            uiButton2.Style = _UIStyle;
            uiButton1.Style = _UIStyle;
            uiButton1.Width = uiTitlePanel1.Width / 2;
            uiTitlePanel1.Text = title;
            uiLabel1.Text = msg;
            if((StaticVar.g_Lang == enLangTable.LANG_TRADITIONAL_CHINESE)
                || (StaticVar.g_Lang == enLangTable.LANG_SIMPLIFIED_CHINESE))
            {
                UILocalizeHelper.SetCH();
                uiButton1.Text = global::ITS_Studio.Properties.Resources.Yes_CN;
                uiButton2.Text = global::ITS_Studio.Properties.Resources.No_CN;
            }
            else
            {
                UILocalizeHelper.SetEN();
                uiButton1.Text = global::ITS_Studio.Properties.Resources.Yes_EN;
                uiButton2.Text = global::ITS_Studio.Properties.Resources.No_EN;
            }   
            MyShowDialog(parentForm, this, showMask);

        }

        public static DialogResult ShowAskDialog(Form fromForm, string title, string msg, UIStyle _UIStyle, bool showMask = false)
        {
            using(var dialog = new FormShowAskDialog(fromForm, title, msg, _UIStyle, showMask))
            {
                return dialog._Result;
            }
        }

        private void MyShowDialog(Form UpParent, Form forms, bool bshowMask)
        {
            Form formBackground = new Form();
            try
            {
                formBackground.Location = UpParent.Location;
                formBackground.StartPosition = FormStartPosition.CenterScreen;
                formBackground.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                formBackground.Opacity = 0.75;
                formBackground.BackColor = Color.Black;
                formBackground.WindowState = FormWindowState.Maximized;
                if(bshowMask)
                    formBackground.Show();

                forms.Location = UpParent.Location;
                forms.StartPosition = FormStartPosition.CenterScreen;
                forms.TopMost = true;
                forms.ShowDialog();

                formBackground.Dispose();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
        private DialogResult _Result = DialogResult.None;
        
        private void uiButton1_Click(object sender, EventArgs e)
        {
            _Result = DialogResult.OK;
            Close();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            _Result = DialogResult.No;
            Close();
        }
    }
}
