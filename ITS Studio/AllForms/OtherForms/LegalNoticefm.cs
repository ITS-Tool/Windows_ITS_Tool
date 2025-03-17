
using System;
using System.Windows.Forms;

namespace ITS_Studio.AllForms
{
    public partial class LegalNotice : Form
    {
        public LegalNotice()
        {
            this.TopMost = true;
            InitializeComponent();
        }

        private void LegalNotice_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; //關閉視窗時取消
            if(this != null)
            {
                if(this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        this.Hide(); //隱藏式窗,下次再show出
                    }));
                }
                else
                {
                    this.Hide(); //隱藏式窗,下次再show出
                }
            }
        }

    }
}