using System;
using System.Windows.Forms;

namespace ITS_Studio.ControlBox
{
    public partial class UserControl_TestStatistics : UserControl
    {
        private double iPassCounts = 0;
        private double iFailCounts = 0;
        private double iTotalCounts = 0;
        private double iYield = 0;

        public UserControl_TestStatistics()
        {
            InitializeComponent();
        }

        private void TestStatistics_Load(object sender, EventArgs e)
        {
            Show_StatisticsInfo();
        }

        public void Clear()
        {
            iPassCounts = 0;
            iFailCounts = 0;
            iTotalCounts = 0;
            iYield = 0;
        }

        public void Set_NowValue(int iPass, int iFail, int iTotal)
        {
            iPassCounts = iPass;
            iFailCounts = iFail;
            iTotalCounts = iTotal;
            if (iTotalCounts == 0)
                iYield = 0;
            else
                iYield = Math.Round((iPassCounts / iTotalCounts) * 100, 1, MidpointRounding.AwayFromZero);
        }

        public void Show_StatisticsInfo()
        {
            //GBV.SenTest_M[iDev]
            textBox_Pass.Text = iPassCounts.ToString();
            textBox_Fail.Text = iFailCounts.ToString();
            textBox_Total.Text = iTotalCounts.ToString();
            textBox_Percent.Text = string.Format("{0}%", iYield);
        }
    }
}