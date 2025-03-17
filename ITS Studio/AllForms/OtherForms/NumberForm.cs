using System;
using System.Windows.Forms;

namespace ITS_Studio.AllForms
{
    public partial class NumberForm : Form
    {
        private System.Timers.Timer _timer = new System.Timers.Timer();
        
        private int Number = 0;
        public NumberForm(int iNum)
        {
            this.TopMost = true;
            InitializeComponent();

            uiSymbolLabel1.Visible = true;
            uiSymbolLabel2.Visible = false;
            uiSymbolLabel3.Visible = false;
            this.uiLedDisplay1.Visible = true;

            this.uiLedDisplay1.Parent = this;

            pictureBox1.Image = ITS_Studio.Properties.Resources.scrolldowngesture;

            Number = iNum;
            uiLedDisplay1.Text = Number.ToString();

            _timer.Enabled = false;
            _timer.Interval = 1000;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(m_timChangeCDCOption_Tick);
        }

        public NumberForm(IntPtr ParentForm)
        {
            this.TopMost = true;
            InitializeComponent();
            this.uiLedDisplay1.Visible = false;
            uiSymbolLabel1.Visible = false;
            uiSymbolLabel2.Visible = true;
            uiSymbolLabel3.Visible = true;
            pictureBox1.Image = ITS_Studio.Properties.Resources.Do_Not_Touch;

            this.WindowState = FormWindowState.Minimized;
            this.TopMost = false;
            this.pictureBox1.Parent = GetForm(ParentForm);
            this.Dock = DockStyle.Fill;
            this.pictureBox1.Visible = true;
        }

        public NumberForm()
        {
            this.TopMost = true;
            InitializeComponent();
            this.uiLedDisplay1.Visible = false;
            uiSymbolLabel1.Visible = false;
            uiSymbolLabel2.Visible = true;
            uiSymbolLabel3.Visible = true;
            pictureBox1.Image = ITS_Studio.Properties.Resources.Do_Not_Touch;           
        }

        static public Control GetForm(IntPtr handle)
        {
            return handle == IntPtr.Zero ?
                null : Control.FromHandle(handle) as Control;
        }

        private void m_timChangeCDCOption_Tick(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                Number--;
                if (Number > 0)
                    uiLedDisplay1.Text = Number.ToString();
                else
                    this.Close();
            }));
           
        }

        private void NumberForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.uiLedDisplay1.Visible)
                _timer.Stop();
            //else
            //{
            //    e.Cancel = true; //關閉視窗時取消
            //    if (this != null)
            //    {
            //        if (this.InvokeRequired)
            //        {
            //            this.Invoke(new Action(() =>
            //            {
            //                this.Hide(); //隱藏式窗,下次再show出
            //                this.pictureBox1.Parent = this;
            //                this.pictureBox1.Visible = false;
            //            }));
            //        }
            //        else
            //        {
            //            this.Hide(); //隱藏式窗,下次再show出
            //            this.pictureBox1.Parent = this;
            //            this.pictureBox1.Visible = false;
            //        }
            //    }
            //}
            //e.Cancel = true; //關閉視窗時取消
            //this.Hide(); //隱藏式窗,下次再show出
            //this.pictureBox1.Parent = this;
            //this.pictureBox1.Visible = false;
        }

        private void NumberForm_Load(object sender, EventArgs e)
        {
            if (this.uiLedDisplay1.Visible)
                _timer.Start();
        }

    }
}
