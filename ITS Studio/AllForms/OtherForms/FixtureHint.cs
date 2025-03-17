using System;
using System.Windows.Forms;

namespace ITS_Studio.AllForms
{
    public partial class FixtureHint : Form
    {
        private System.Timers.Timer _timer = new System.Timers.Timer();
        
        private int Number = 0;
        public FixtureHint(int iNum)
        {
            this.TopMost = true;
            InitializeComponent();
            this.uiLedDisplay1.Parent = this;

            Number = iNum;
            uiLedDisplay2.Text = (Number / 10).ToString();
            uiLedDisplay1.Text = (Number % 10).ToString();

            _timer.Enabled = false;
            _timer.Interval = 1000;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(m_timChangeCDCOption_Tick);
        }

        public FixtureHint(int iNum, bool bIsNoisePattern)
        {
            this.TopMost = true;
            InitializeComponent();
            this.uiLedDisplay1.Parent = this;

            if (!bIsNoisePattern)
            {
                uiSymbolLabel1.Text = "请点亮LCM";
            }
            else
                uiSymbolLabel1.Text = "请切换画面";

            Number = iNum;
            uiLedDisplay2.Text = (Number / 10).ToString();
            uiLedDisplay1.Text = (Number % 10).ToString();

            _timer.Enabled = false;
            _timer.Interval = 1000;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(m_timChangeCDCOption_Tick);
        }

        private void m_timChangeCDCOption_Tick(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                Number--;

                if (Number > 0)
                {
                    uiLedDisplay2.Text = (Number / 10).ToString();
                    uiLedDisplay1.Text = (Number % 10).ToString();
                }

                else
                    this.Close();
            }));

        }

        private void FixtureHint_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer.Stop();
            //e.Cancel = true; //關閉視窗時取消
            //this.Hide(); //隱藏式窗,下次再show出
            //this.pictureBox1.Parent = this;
            //this.pictureBox1.Visible = false;
        }

        private void FixtureHint_Load(object sender, EventArgs e)
        {
            _timer.Start();
        }

    }
}
