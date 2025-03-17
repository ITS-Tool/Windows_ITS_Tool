using CommonExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ITS_Studio
{
    public partial class SensorTest_FPC_Open : Form
    {
        private Mutex MuIsFormClosed = new Mutex();
        private int m_iSelectedDev = 0;
        public int SelectedDev { set { m_iSelectedDev = value; } get { return m_iSelectedDev; } }

        //private bool bFormIsClosed = false;
        public SensorTest_FPC_Open()
        {
            InitializeComponent();
            IsFormClosed = false;
        }

        public void Set_Chart_Axes(int Channels, string Name)
        {
            tChart1.Axes.Bottom.SetMinMax(1, Channels);
            tChart1.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            //tChart1.Aspect.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            //tChart1.Panel.Gradient.Visible = false;
            //tChart1.Aspect.View3D = false;
            tChart1.Text = Name;
            bar1.Clear();
        }

        public void Set_Bar_Value(string str)
        {
            bar1.Clear();
            var _Arr = str.Split(',').Select(x => int.Parse(x)).ToList();
            List<int> tmpArr = new List<int>(_Arr);
            tmpArr.Insert(0, 0);
            bar1.Add(tmpArr.ToArray());
        }

        //public void Set_Bar_Value(List<int> Short_Array)
        //{
        //    bar1.Clear();
        //    List<int> tmpArr = new List<int>(Short_Array);
        //    tmpArr.Insert(0,0);
        //    bar1.Add(tmpArr.ToArray());
        //}

        public void Set_Bar_Max(int iMaxValue)
        {
            tChart1.Axes.Left.SetMinMax(0, iMaxValue);
        }

        public void Set_Timer_Text(int _Timer)
        {
            int iTmp_Timer = _Timer;
            if (_Timer >= Status_ProgressBar.Maximum)
                iTmp_Timer = Status_ProgressBar.Maximum;

            Status_Timer.Text = string.Format("Test : {0} sec", iTmp_Timer);
            Status_ProgressBar.Value = Status_ProgressBar.Maximum - iTmp_Timer;
        }

        public void Set_Timer_Max(int iMaxValue)
        {
            Status_ProgressBar.Maximum = iMaxValue;
            Status_ProgressBar.Value = iMaxValue;
        }

        private void tChart1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        public bool IsFormClosed
        {
            set
            {
                MuIsFormClosed.WaitOne();
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.g_bFPCFormIsClosed = value;
                if (MuIsFormClosed != null)
                    MuIsFormClosed.ReleaseMutex();
            }
            get { return UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.g_bFPCFormIsClosed; }
        }

        private void SensorTest_FPC_Open_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MultiSenTest_VAR.g_bFPCFormIsClosed = true;
            IsFormClosed = true;
        }

        private void SensorTest_FPC_Open_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.TopLevel = true;
            StaticVar.LogWrite(ConsoleLogType.Msg, "SensorTest_FPC_Open_Shown\r\n");
        }

        private delegate void function(Message msg);

        private Dictionary<enWM_MSG, function> mMessageReceiver = new Dictionary<enWM_MSG, function>();

        protected override void WndProc(ref Message msg)
        {
            enWM_MSG pEnumPanoMsg = (enWM_MSG)msg.Msg;
            if (mMessageReceiver.Keys.Contains(pEnumPanoMsg))
                mMessageReceiver[pEnumPanoMsg](msg);
            base.WndProc(ref msg);
        }

        private void SensorTest_FPC_Open_Activated(object sender, EventArgs e)
        {
            IsFormClosed = false;
            //MultiSenTest_VAR.g_bFPCFormIsClosed = false;
        }
    }
}