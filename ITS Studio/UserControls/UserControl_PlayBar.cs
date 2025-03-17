using CommonExt;
using ILITek_ITS_Tool_Lib;
using Sunny.UI;
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace CSharp_PlayBar
{
    public enum ButtonStatus
    {
        Stop,
        Pause,
        Play,
        LoadFile,
        Ready,
        Previous,
        Next,
    }

    public partial class UserControl_PlayBar : UserControl
    {
        private ButtonStatus _ButtonStatus = ButtonStatus.LoadFile;

        public Func<bool> LoadFile_API;

        public Action PlayFile_API;

        public Action Stop_API;

        private CDC_Replay _CDC_Replay;

        private Paint_Replay _Paint_Replay;

        private CtrlILIDevice tmpCtrl;

        private BackgroundWorker _work = new BackgroundWorker();

        public delegate bool delStringFunction(out string tmp);

        public delStringFunction ShowCDCStatisticsFunc;

        private UIPage MyUIPage = new UIPage();

        public UserControl_PlayBar()
        {
            InitializeComponent();

            StaticVar.PlayBarFormHandle = this.Handle;

            _work.DoWork += _work_DoWork;
            _work.WorkerSupportsCancellation = true;

            uiToolTip1.SetToolTip(btn_Load_File, "讀檔");
            uiToolTip1.SetToolTip(btn_Play, "播放/暫停");
            uiToolTip1.SetToolTip(btn_Stop, "停止");
            uiToolTip1.SetToolTip(btn_ShowForm, "顯示CDC的統計數據");
        }

        public bool b_Paint_ReplayUseNewFormat
        {
            set
            {
                if(_Paint_Replay != null)
                    _Paint_Replay.bUseNewFormat = value;
            }
            get
            {
                return (_Paint_Replay == null) ? false : _Paint_Replay.bUseNewFormat;
            }
        }

        public bool b_CDC_ReplayUseNewFormat
        {
            set
            {
                if(_CDC_Replay != null)
                    _CDC_Replay.bUseNewFormat = value;
            }
            get
            {
                return (_CDC_Replay == null) ? false : _CDC_Replay.bUseNewFormat;
            }
        }

        private double iSpeed = 1;

        public double multiple
        {
            set
            {
                var _Value = value > 4 ? 4 : value;
                _Value = value < 0 ? 0 : value;
                iSpeed = _Value;
                this.Invoke(new Action(() =>
                {
                    uiLedLabel1.Text = "Multiple : " + iSpeed.ToString();
                }));
                //uiLedLabel1.Text = multiple.ToString();
            }
            get
            {
                return iSpeed;
            }
        }
        public bool SetTitlePanelCollapsed
        {
            set
            {
                if(value == true)
                    lb_Frame_Number.Dock = DockStyle.Fill;
                else
                    lb_Frame_Number.Dock = DockStyle.Bottom;
            }            
        }
        public string SetTitlePanelTitle
        {
            set
            {
                uiPanel1.Text = value;
            }
        }
        public bool AssignToolFunc(enToolName NowExecuteTool, UserControl_PlayBar _MyPlayBar, CtrlILIDevice ctl)
        {
            tmpCtrl = ctl;
            if(NowExecuteTool == enToolName.PaintTool)
            {
                if(_Paint_Replay == null)
                    _Paint_Replay = new Paint_Replay(_MyPlayBar, tmpCtrl.LocalVariable, tmpCtrl.m_GBV.Globe_Multi);
                LoadFile_API = _Paint_Replay.Load_Painting_RecordFile;
                PlayFile_API = _Paint_Replay.PlayFile_Paint;
                Stop_API = _Paint_Replay.StopPlay;
                ChangeStatus(ButtonStatus.LoadFile);
                btn_ShowForm.Visible = false;
                return true;
            }
            else if(NowExecuteTool == enToolName.CDCTool)
            {
                if(_CDC_Replay == null)
                    _CDC_Replay = new CDC_Replay(_MyPlayBar, tmpCtrl.LocalVariable, tmpCtrl.m_GBV.Globe_Multi);
                LoadFile_API = _CDC_Replay.Load_CDC_RecordFile;
                PlayFile_API = _CDC_Replay.PlayFile_CDC;
                Stop_API = _CDC_Replay.StopPlay;
                ChangeStatus(ButtonStatus.LoadFile);
                btn_ShowForm.Visible = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btn_Load_File_Click(object sender, EventArgs e)
        {
            if(GetButtonStatus != ButtonStatus.Stop)
            {
                Stop_API();
                ChangeStatus(ButtonStatus.Stop);
            }
            this.uiTrackBar1.ValueChanged -= uiTrackBar1_ValueChanged;
            if(LoadFile_API())
                ChangeStatus(ButtonStatus.Ready);
            //else
            //    MyUIPage.ShowErrorDialog("系统提示", "Log is not valid", UIStyle.Red, true);

            this.uiTrackBar1.ValueChanged += uiTrackBar1_ValueChanged;
            this.Focus(); //Steven 20220609 : Load檔案後獲取焦點
        }

        private void btn_Play_Click(object sender, EventArgs e)
        {
            this.uiTrackBar1.ValueChanged -= uiTrackBar1_ValueChanged;
            PlayFile_API();
            this.uiTrackBar1.ValueChanged += uiTrackBar1_ValueChanged;
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            this.uiTrackBar1.ValueChanged -= uiTrackBar1_ValueChanged;
            this.Enabled = false;
            Stop_API();
            ChangeStatus(ButtonStatus.Stop);
            this.Enabled = true;
            this.uiTrackBar1.ValueChanged += uiTrackBar1_ValueChanged;
        }

        public ButtonStatus GetButtonStatus
        {
            get
            {
                return this._ButtonStatus;
            }
        }

        public void ChangeStatus(ButtonStatus NowStatus)
        {
            //Console.WriteLine(_ButtonStatus);
            _ButtonStatus = NowStatus;
            switch(NowStatus)
            {
                case ButtonStatus.Play:
                    this.btn_Play.Symbol = 61515;
                    this.btn_Load_File.Enabled = true;
                    this.btn_Prev.Enabled = true;
                    this.btn_Next.Enabled = true;
                    break;

                case ButtonStatus.Pause:
                    this.btn_Play.Symbol = 61516;
                    this.btn_Load_File.Enabled = false;
                    this.btn_Prev.Enabled = false;
                    this.btn_Next.Enabled = false;
                    break;

                case ButtonStatus.Stop:
                    this.btn_Play.Symbol = 61515;
                    this.btn_Load_File.Enabled = true;
                    this.btn_Prev.Enabled = true;
                    this.btn_Next.Enabled = true;
                    uiTrackBar1.Value = uiTrackBar1.Minimum;
                    lb_Frame_Number.Text = uiTrackBar1.Value.ToString();
                    break;

                case ButtonStatus.Ready:
                    this.btn_Play.Enabled = true;
                    this.btn_Load_File.Enabled = true;
                    this.btn_Prev.Enabled = true;
                    this.btn_Next.Enabled = true;
                    this.btn_Stop.Enabled = true;
                    this.btn_Rewind.Enabled = true;
                    this.btn_Fast_Forward.Enabled = true;
                    break;

                case ButtonStatus.LoadFile:
                    this.btn_Play.Enabled = false;
                    this.btn_Load_File.Enabled = true;
                    this.btn_Prev.Enabled = true;
                    this.btn_Next.Enabled = true;
                    this.btn_Stop.Enabled = false;
                    this.btn_Rewind.Enabled = false;
                    this.btn_Fast_Forward.Enabled = false;
                    break;
            }
        }

        public bool TrackBar_En
        {
            set
            {
                uiTrackBar1.Enabled = value;
            }
            get
            {
                return uiTrackBar1.Enabled;
            }
        }

        public int TrackBar_Value
        {
            set
            {
                uiTrackBar1.Value = value;
            }
            get
            {
                return uiTrackBar1.Value;
            }
        }

        public int TrackBar_Max
        {
            set
            {
                uiTrackBar1.Maximum = value;
            }
            get
            {
                return uiTrackBar1.Maximum;
            }
        }

        public int TrackBar_Min
        {
            set
            {
                uiTrackBar1.Minimum = value;
            }
            get
            {
                return uiTrackBar1.Minimum;
            }
        }

        public string FrameNum_Text
        {
            set
            {
                this.lb_Frame_Number.Text = value;
            }
            get
            {
                return this.lb_Frame_Number.Text;
            }
        }

        private void uiTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            var MySender = sender as UITrackBar;
            //Console.WriteLine("{0} {1} {2}",MySender.Maximum, MySender.Minimum, MySender.Value);
            int iNowValue = MySender.Value;
            if(iNowValue < 1)
                iNowValue = 1;
            this.uiTrackBar1.ValueChanged -= uiTrackBar1_ValueChanged;
            if(StaticVar.NowExecuteTool == enToolName.CDCTool)
            {
                if(!b_CDC_ReplayUseNewFormat && (tmpCtrl.LocalVariable._PlayBar_VAR.PlayBar_Frame.Count == 0))
                    return;
                tmpCtrl.LocalVariable.VarCDCTool_Multi.bRunningStatus = true;
                tmpCtrl.LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.CDCToolformHandle, enWM_MSG.WM_CDC_Replay_Frame, (IntPtr)(iNowValue - 1), IntPtr.Zero);
            }
            else if(StaticVar.NowExecuteTool == enToolName.PaintTool)
            {
                if((_ButtonStatus == ButtonStatus.Ready) 
                    || (_ButtonStatus == ButtonStatus.Play)
                    || (_ButtonStatus == ButtonStatus.Stop))
                {
                    if(!b_Paint_ReplayUseNewFormat && (tmpCtrl.LocalVariable._PlayBar_VAR.PlayBar_Frame.Count == 0))
                        return;
                    tmpCtrl.LocalVariable.VarPaint_Multi.bRunningStatus = true;
                    tmpCtrl.LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.PaintToolFormHandle, enWM_MSG.WM_PaintReplayFrame, (IntPtr)(iNowValue - 1), IntPtr.Zero);
                }
            }
            this.uiTrackBar1.ValueChanged += uiTrackBar1_ValueChanged;
        }

        public string FileName_Text
        {
            set
            {
                lb_fileName.Text = value;
            }
            get
            {
                return lb_fileName.Text;
            }
        }

        private void btn_Fast_Forward_Click(object sender, EventArgs e)
        {
            this.multiple += 0.5;
        }

        private void btn_Rewind_Click(object sender, EventArgs e)
        {
            this.multiple -= 0.5;
        }

        public bool ButtonVisible
        {
            set
            {
                btn_Rewind.Visible = value;
                btn_Fast_Forward.Visible = value;
                uiLedLabel1.Visible = value;
            }

        }

        public void TrackBarValuePlus()
        {
            this.Invoke(new Action(() =>
                {
                    Console.WriteLine("TrackBarValuePlus");
                    uiTrackBar1.Value++;
                }));
        }

        public void TrackBarValueReduce()
        {
            this.Invoke(new Action(() =>
            {
                Console.WriteLine("TrackBarValueReduce");
                uiTrackBar1.Value--;
            }));
        }

        private void uiTrackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            this.uiTrackBar1.ValueChanged -= uiTrackBar1_ValueChanged;
        }

        private void uiTrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            this.uiTrackBar1.Enabled = false;
            this.uiTrackBar1.ValueChanged += uiTrackBar1_ValueChanged;
            uiTrackBar1_ValueChanged(sender, null);
            this.uiTrackBar1.Enabled = true;
        }

        private void btn_Next_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("btn_Next_MouseDown");
            TrackBarValuePlus();
        }

        private void _work_DoWork(object sender, DoWorkEventArgs e)
        {
            var arg = (ButtonStatus)e.Argument;
            while(!_work.CancellationPending)
            {
                if(arg == ButtonStatus.Next)
                    TrackBarValuePlus();
                else if(arg == ButtonStatus.Previous)
                    TrackBarValueReduce();
            }
        }

        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("btn_MouseUp");
            _work.CancelAsync();
        }

        private void btn_Prev_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("btn_Prev_MouseDown");
            TrackBarValueReduce();
        }

        private void btn_ShowForm_Click(object sender, EventArgs e)
        {
            string Mesg = "";
            if(ShowCDCStatisticsFunc != null)
                if(!ShowCDCStatisticsFunc(out Mesg))
                    MyUIPage.ShowErrorDialog("系统提示", Mesg, UIStyle.Red, true);
        }

        public bool SetBtn_Next_Visible
        {
            get
            {
                return btn_Next.Visible;
            }
            set
            {
                btn_Next.Visible = value;
            }
        }

        public bool SetBtn_Prev_Visible
        {
            get
            {
                return btn_Prev.Visible;
            }
            set
            {
                btn_Prev.Visible = value;
            }
        }

        bool isEnableEdit = false;
        string sTemp = "";
        string sPrevioudText = "";

        private void lb_Frame_Number_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isEnableEdit)
                return;

            if (e.KeyCode == Keys.Enter)
            {

                lb_Frame_Number.BackColor = Color.FromArgb(255, 251, 245, 233);
                if (string.IsNullOrEmpty(sTemp))
                    sTemp = "1";
                int iTrackValue = Convert.ToInt32(sTemp);
                if (iTrackValue > uiTrackBar1.Maximum || iTrackValue < uiTrackBar1.Minimum)
                {
                    MessageBox.Show("Invalid Value!");
                    lb_Frame_Number.Text = sPrevioudText;
                    sTemp = "";
                    return;
                }
                lb_Frame_Number.Text = sTemp;
                uiTrackBar1.Value = Convert.ToInt32(sTemp);
                sTemp = "";
                isEnableEdit = false;
            }
            else
            {

                if (((int)e.KeyCode < 48 || (int)e.KeyCode > 57))
                {
                    if ((int)e.KeyCode < 96 || (int)e.KeyCode > 105)
                        return;
                }
                int iCode = ((int)e.KeyCode > 57) ? (int)e.KeyCode - 48 : (int)e.KeyCode;
                sTemp += Convert.ToChar(iCode);
                lb_Frame_Number.Text = sTemp;
            }
        }

        private void lb_Frame_Number_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lb_Frame_Number.Focus();
            isEnableEdit = true;
            lb_Frame_Number.BackColor = Color.Blue;
            sPrevioudText = lb_Frame_Number.Text;
        }
    }
}