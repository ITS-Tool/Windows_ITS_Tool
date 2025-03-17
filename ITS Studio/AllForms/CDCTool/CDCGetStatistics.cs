using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ITS_Studio.AllForms
{
    public partial class CDCGetStatistics : Form
    {
        private int _iFrameTotalCount = 0;

        private int _iStartFrame = 0;
        private int _iEndFrame = 0;
        private int _iWin2Threshold = 0;

        public delegate void GetFunction(object sender, EventArgs e);

        public event GetFunction GetCDCStatisticsFunc;

        public int iStartFrame
        {
            get
            {
                return uiInt_StartFrame.Value;
            }
            set
            {
                _iStartFrame = value;
                uiInt_StartFrame.Value = _iStartFrame;
            }
        }

        public int iEndFrame
        {
            get
            {
                return uiInt_EndFrame.Value;
            }
            set
            {
                _iEndFrame = value;
                uiInt_EndFrame.Value = _iEndFrame;
            }
        }

        public int iWin2Threshold
        {
            get
            {
                return uiInt_Win2Threshold.Value;
            }
            set
            {
                _iWin2Threshold = value;
                uiInt_Win2Threshold.Value = _iWin2Threshold;
            }
        }

        public int iMaxValue
        {
            set
            {
                textBox_Max.Text = value.ToString();
                textBox_Max.Refresh();
            }
        }

        public int iMinValue
        {
            set
            {
                textBox_Min.Text = value.ToString();
                textBox_Min.Refresh();
            }
        }

        public int iMaxFrameValue
        {
            set
            {
                textBox_Max_Frame.Text = value.ToString();
                textBox_Max_Frame.Refresh();
            }
        }

        public int iMinFrameValue
        {
            set
            {
                textBox_Min_Frame.Text = value.ToString();
                textBox_Min_Frame.Refresh();
            }
        }

        public int iDeltaValue
        {
            set
            {
                textBox_Delta.Text = value.ToString();
                textBox_Delta.Refresh();
            }
        }

        public int iWin2Cnt
        {
            set
            {
                textBox_Win2_Cnt.Text = value.ToString();
                textBox_Win2_Cnt.Refresh();
            }
        }

        public int iMaxFrameCount
        {
            set
            {
                uiInt_StartFrame.Maximum = value;
                uiInt_EndFrame.Maximum = value;
            }
        }

        public CDCGetStatistics()
        {
            InitializeComponent();
            Console.WriteLine(this.Location);
            this.Location = new Point(0, 0);
            this.StartPosition = FormStartPosition.CenterParent;
        }

        public CDCGetStatistics(int iFrameCount)
        {
            InitializeComponent();            
            _iFrameTotalCount = iFrameCount;            
        }

        private void uiBtn_GetCDCStatistics_Click(object sender, EventArgs e)
        {
            if (GetCDCStatisticsFunc != null)
                GetCDCStatisticsFunc(sender, e);
        }

        public void ClearWin2FrameList()
        {
            uiRichTextBox_Frame.Clear();
        }

        public void AddWin2FrameList(string sFrameCnt)
        {
            uiRichTextBox_Frame.AppendText(sFrameCnt + "\r\n");
        }

        public void AddWin2FrameList(int iFrameCnt)
        {
            AddWin2FrameList(iFrameCnt.ToString());
        }

        public void AddWin2FrameList(List<int> lsiFrameCnt)
        {
            foreach (int item in lsiFrameCnt)
            {
                AddWin2FrameList(item);
            }

        }

        private void CDCGetStatistics_Shown(object sender, EventArgs e)
        {
        }
    }
}
