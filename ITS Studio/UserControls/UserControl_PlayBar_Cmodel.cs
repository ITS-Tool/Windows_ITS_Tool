using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonExt;
using Sunny.UI;

namespace CSharp_PlayBar
{
    public partial class UserControl_PlayBar_Cmodel : UserControl
    {
        public delegate void BtnClickHandler(object sender, EventArgs e);

        public delegate void UpdateFileEditHandler(bool b1, List<int> ls1, List<int> ls2);

        public delegate void PlusBtnHandler();

        public delegate void MinusBtnHandler();

        public delegate void PauseHandler();

        public event BtnClickHandler LoadCDCFileEvent;

        public event BtnClickHandler LoadPainting1FileEvent;

        public event BtnClickHandler LoadPainting2FileEvent;

        public event BtnClickHandler PlayFileEvent;

        public event BtnClickHandler StopPlayEvent;

        public event BtnClickHandler GenFileEvent;

        public event UpdateFileEditHandler UpdateFileEidtEvent;

        public event PlusBtnHandler PlusBtnEvent;

        public event MinusBtnHandler MinusBtnEvent;

        private string _CDCFilePath;
        private string _Paint1FilePath;
        private string _Paint2FilePath;
        private List<int> _lsStartPos = new List<int>();
        private List<int> _lsEndPos = new List<int>();

        private List<int> _lsPaint1Index = new List<int>();
        private List<int> _lsPaint2Index = new List<int>();

        private bool _isWaitEnd = false;

        private List<RectangleF> _lsPaint1Mark = new List<RectangleF>();
        private List<RectangleF> _lsPaint2Mark = new List<RectangleF>();

        private Bitmap paint1Bitmap;
        private Bitmap paint2Bitmap;
        private Graphics bitPaint1Drawer;
        private Graphics bitPaint2Drawer;

        private SolidBrush[] BrushSelect = { new SolidBrush(Color.Green), new SolidBrush(Color.Blue), new SolidBrush(Color.LightGreen), new SolidBrush(Color.Red), new SolidBrush(Color.FromArgb(255, 251, 245, 233)), new SolidBrush(Color.FromArgb(120, 255, 255, 0)) };

        public List<int> lsStartPos
        {
            get
            {
                return _lsStartPos;
            }
            set
            {
                _lsStartPos = value;
            }
        }

        public List<int> lsEndPos
        {
            get
            {
                return _lsEndPos;
            }
            set
            {
                _lsEndPos = value;
            }
        }

        public bool isWaitEnd
        {
            get
            {
                return _isWaitEnd;
            }
            set
            {
                _isWaitEnd = value;
            }
        }

        private Pen[] PenSelect = { new Pen(Color.LightGreen, 0.1f), new Pen(Color.Red, 0.1f) };

        public string CDCFileName
        {
            get
            {
                return _CDCFilePath;
            }
            set
            {
                _CDCFilePath = value;
                uiSymbolLabel_CDC.Text = string.Format("CDC : {0}", Path.GetFileName(_CDCFilePath));
            }
        }

        public string Paint1FileName
        {
            get
            {
                return _Paint1FilePath;
            }
            set
            {
                _Paint1FilePath = value;
                uiSymbolLabel_Paint1.Text = string.Format("Paint1 : {0}", Path.GetFileName(_Paint1FilePath));
            }
        }

        public string Paint2FileName
        {
            get
            {
                return _Paint2FilePath;
            }
            set
            {
                _Paint2FilePath = value;
                uiSymbolLabel_Paint2.Text = string.Format("Paint2 : {0}", Path.GetFileName(_Paint2FilePath));
            }
        }

        private ButtonStatus _ButtonStatus = ButtonStatus.LoadFile;

        private Stopwatch _watch = new Stopwatch();

        private BackgroundWorker _work = new BackgroundWorker();

        public UserControl_PlayBar_Cmodel()
        {
            InitializeComponent();

            StaticVar.PlayBarFormHandle = this.Handle;

            _work.DoWork += _work_DoWork;
            _work.WorkerSupportsCancellation = true;

            uiToolTip1.SetToolTip(btn_Load_File, "選取CDC檔");
            uiToolTip1.SetToolTip(btn_Load_Painting1, "選取Paint1檔");
            uiToolTip1.SetToolTip(btn_Load_Painting2, "選取Paint2檔");
            uiToolTip1.SetToolTip(btn_Tag, "Tag要擷取的資料");
            
            uiToolTip1.SetToolTip(btn_Play, "播放");
            uiToolTip1.SetToolTip(btn_Stop, "停止");
            uiToolTip1.SetToolTip(btn_GenFile, "匯出選取的資料");
            
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

        private void btn_Load_File_Click(object sender, EventArgs e)
        {
            if(GetButtonStatus != ButtonStatus.Stop)
            {
                if(StopPlayEvent != null)
                    StopPlayEvent(sender, e);
                ChangeStatus(ButtonStatus.Stop);
            }
            this.uiTrackBar1.ValueChanged -= uiTrackBar1_ValueChanged;
            _isWaitEnd = false;
            if(LoadCDCFileEvent != null)
                LoadCDCFileEvent(sender, e);
            this.uiTrackBar1.ValueChanged += uiTrackBar1_ValueChanged;
            this.Focus(); //Steven 20220609 : Load檔案後獲取焦點
        }

        private void btn_Play_Click(object sender, EventArgs e)
        {
            this.uiTrackBar1.ValueChanged -= uiTrackBar1_ValueChanged;
            if(PlayFileEvent != null)
                PlayFileEvent(sender, e);
            //PlayFile_API();
            if(_ButtonStatus != ButtonStatus.Pause)
                this.uiTrackBar1.ValueChanged += uiTrackBar1_ValueChanged;
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            //this.uiTrackBar1.ValueChanged -= uiTrackBar1_ValueChanged;
            //this.Enabled = false;
            //Stop_API();
            if(StopPlayEvent != null)
                StopPlayEvent(sender, e);

            ChangeStatus(ButtonStatus.Stop);
            //this.Enabled = true;
            //this.uiTrackBar1.ValueChanged += uiTrackBar1_ValueChanged;
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
                    this.btn_Load_Painting1.Enabled = true;
                    this.btn_Load_Painting2.Enabled = true;
                    break;

                case ButtonStatus.Pause:
                    this.btn_Play.Symbol = 61516;
                    this.btn_Load_File.Enabled = false;
                    this.btn_Load_Painting1.Enabled = false;
                    this.btn_Load_Painting2.Enabled = false;
                    break;

                case ButtonStatus.Stop:
                    this.btn_Play.Symbol = 61515;
                    this.btn_Load_File.Enabled = true;
                    this.btn_Load_Painting1.Enabled = true;
                    this.btn_Load_Painting2.Enabled = true;
                    this.uiTrackBar1.ValueChanged -= uiTrackBar1_ValueChanged;
                    uiTrackBar1.Value = uiTrackBar1.Minimum;
                    lb_Frame_Number.Text = uiTrackBar1.Value.ToString();
                    //this.uiTrackBar1.ValueChanged += uiTrackBar1_ValueChanged;
                    break;

                case ButtonStatus.Ready:
                    this.btn_Play.Enabled = true;
                    this.btn_Load_File.Enabled = true;
                    this.btn_Load_Painting1.Enabled = true;
                    this.btn_Load_Painting2.Enabled = true;

                    this.btn_Stop.Enabled = true;
                    //this.btn_Rewind.Enabled = true;
                    this.btn_Tag.Enabled = true;
                    break;

                case ButtonStatus.LoadFile:
                    this.btn_Play.Enabled = false;
                    this.btn_Load_File.Enabled = true;
                    this.btn_Load_Painting1.Enabled = true;
                    this.btn_Load_Painting2.Enabled = true;
                    this.btn_Stop.Enabled = false;
                    //this.btn_Rewind.Enabled = false;
                    this.btn_Tag.Enabled = false;
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

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, enWM_MSG msg, IntPtr wParam, IntPtr lParam);

        public void MultiDev_SendMSG(IntPtr hWnd, enWM_MSG Msg, IntPtr wParam, IntPtr lParam)
        {
            SendMessage(hWnd, Msg, wParam, lParam);
        }

        private void uiTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            var MySender = sender as UITrackBar;
            //Console.WriteLine("{0} {1} {2}",MySender.Maximum, MySender.Minimum, MySender.Value);
            int iNowValue = MySender.Value;
            if(iNowValue < 1)
                iNowValue = 1;
            this.uiTrackBar1.ValueChanged -= uiTrackBar1_ValueChanged;

            if(_ButtonStatus != ButtonStatus.Pause)
            {
                //TODO:改為委派做法
                MultiDev_SendMSG(StaticVar.CModelToolReplayFromHandle, enWM_MSG.WM_CModel_Replay, (IntPtr)(iNowValue), IntPtr.Zero);
            }
            this.uiTrackBar1.ValueChanged += uiTrackBar1_ValueChanged;
        }

        private void btn_Fast_Forward_Click(object sender, EventArgs e)
        {
            //this.multiple += 0.5;
            if(PlusBtnEvent != null)
                PlusBtnEvent();

            int frame = Convert.ToInt32(lb_Frame_Number.Text);
            if(!_isWaitEnd)
            {
                if(_lsEndPos.Count != 0)
                {
                    int index = _lsEndPos.Count - 1;
                    if(frame < _lsEndPos[index])
                    {
                        MessageBox.Show("Start Frame不能放在前一區的End Frame之前", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                _lsStartPos.Add(frame);
                using(Graphics g = Graphics.FromImage(paint1Bitmap))
                {
                    g.FillRectangle(BrushSelect[2], _lsPaint1Mark[frame - 1]);
                }
                UpdatePaintBar();

                if(UpdateFileEidtEvent != null)
                {
                    UpdateFileEidtEvent(_isWaitEnd, _lsStartPos, _lsEndPos);
                }
                _isWaitEnd = true;
            }
            else
            {
                int index = _lsStartPos.Count - 1;
                if(frame < _lsStartPos[index])
                {
                    MessageBox.Show("End Frame不能放在Start Frame之前", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _lsEndPos.Add(frame);
                using(Graphics g = Graphics.FromImage(paint1Bitmap))
                {
                    g.FillRectangle(BrushSelect[3], _lsPaint1Mark[frame - 1]);
                }

                if(UpdateFileEidtEvent != null)
                {
                    UpdateFileEidtEvent(_isWaitEnd, _lsStartPos, _lsEndPos);
                }
                for(int i = _lsStartPos[index]; i <= _lsEndPos[index]; i++)
                {
                    using(Graphics g = Graphics.FromImage(paint1Bitmap))
                    {
                        g.FillRectangle(BrushSelect[5], _lsPaint1Mark[i - 1]);
                    }
                }
                UpdatePaintBar();
                _isWaitEnd = false;
            }
        }

        private void btn_Rewind_Click(object sender, EventArgs e)
        {
            //this.multiple -= 0.5;
            if(MinusBtnEvent != null)
                MinusBtnEvent();

            //if (!isWaitEnd)
            //{
            //    MessageBox.Show("請先設定Start Point再選擇End Point");
            //    return;
            //}
            //int index = Convert.ToInt32(lb_Frame_Number.Text);
            //_lsEndPos.Add(index);
            //bitPaint1Drawer.FillRectangle(BrushSelect[3], _lsPaint1Mark[index-1]);
            //if (UpdateFileEidtEvent != null)
            //{
            //    UpdateFileEidtEvent(isWaitEnd, _lsStartPos, _lsEndPos);
            //}
            //isWaitEnd = false;
        }

        //public bool ButtonVisible
        //{
        //    set
        //    {
        //        btn_Rewind.Visible = value;
        //        btn_Fast_Forward.Visible = value;
        //        uiLedLabel1.Visible = value;
        //    }
        //}

        public void RefreshFrameLabel()
        {
            lb_Frame_Number.Refresh();
        }

        public void TrackBarValuePlus()
        {
            this.Invoke(new Action(() =>
                {
                    uiTrackBar1.Value++;
                }));
        }

        public void TrackBarValueReduce()
        {
            this.Invoke(new Action(() =>
            {
                uiTrackBar1.Value--;
            }));
        }

        public void InitialPaint1Mark(int iFrameTotalNumber)
        {
            paint1Bitmap = new Bitmap(pPaint1Mark.Width, pPaint1Mark.Height);
            bitPaint1Drawer = pPaint1Mark.CreateGraphics();

            _lsPaint1Mark.Clear();
            _lsPaint1Index.Clear();
            bitPaint1Drawer = pPaint1Mark.CreateGraphics();
            float fWidth = ((float)this.pPaint1Mark.Width / (float)iFrameTotalNumber);
            float fHeight = ((float)this.pPaint1Mark.Height);
            for(int i = 0; i < iFrameTotalNumber; i++)
            {
                _lsPaint1Mark.Add(new RectangleF(0f + (float)i * fWidth, 0f, fWidth, fHeight));
            }
            UpdatePaintBar();
        }

        public void InitialPaint2Mark(int iFrameTotalNumber)
        {
            paint2Bitmap = new Bitmap(pPaint2Mark.Width, pPaint2Mark.Height);
            bitPaint2Drawer = pPaint2Mark.CreateGraphics();

            _lsPaint2Mark.Clear();
            _lsPaint2Index.Clear();
            bitPaint2Drawer = pPaint2Mark.CreateGraphics();
            float fWidth = ((float)this.pPaint2Mark.Width / (float)iFrameTotalNumber);
            float fHeight = ((float)this.pPaint2Mark.Height);
            for(int i = 0; i < iFrameTotalNumber; i++)
            {
                _lsPaint2Mark.Add(new RectangleF(0f + (float)i * fWidth, 0f, fWidth, fHeight));
            }
            UpdatePaintBar();
        }

        public void SetPaint1Mark(int iFrameNumber)
        {
            _lsPaint1Index.Add(iFrameNumber);
            using(Graphics g = Graphics.FromImage(paint1Bitmap))
            {
                g.FillRectangle(BrushSelect[0], _lsPaint1Mark[iFrameNumber - 1]);
            }
        }

        public void SetPaint2Mark(int iFrameNumber)
        {
            _lsPaint2Index.Add(iFrameNumber);
            using(Graphics g = Graphics.FromImage(paint2Bitmap))
            {
                g.FillRectangle(BrushSelect[1], _lsPaint2Mark[iFrameNumber - 1]);
            }
        }

        public void UpdatePaintBar()
        {
            pPaint1Mark.Invalidate();
            pPaint2Mark.Invalidate();
        }

        public void ResetPaint1Mark(string sStartFrameNumbers, string sEndFrameNumbers)
        {
            int iStartFrameNumber = Convert.ToInt32(sStartFrameNumbers);
            int iEndFrameNumber = -1;
            if(!sEndFrameNumbers.IsNullOrEmpty())
                iEndFrameNumber = Convert.ToInt32(sEndFrameNumbers);

            if(_lsPaint1Index.FindIndex(x => x == iStartFrameNumber) != -1)
            {
                using(Graphics g = Graphics.FromImage(paint1Bitmap))
                    g.FillRectangle(BrushSelect[0], _lsPaint1Mark[iStartFrameNumber - 1]);
            }
            else
            {
                using(Graphics g = Graphics.FromImage(paint1Bitmap))
                    g.FillRectangle(BrushSelect[4], _lsPaint1Mark[iStartFrameNumber - 1]);
            }

            if(iEndFrameNumber != -1)
            {
                for(int i = iStartFrameNumber; i <= iEndFrameNumber; i++)
                {
                    if(_lsPaint1Index.FindIndex(x => x == i) != -1)
                    {
                        using(Graphics g = Graphics.FromImage(paint1Bitmap))
                            g.FillRectangle(BrushSelect[0], _lsPaint1Mark[i - 1]);
                    }
                    else
                    {
                        using(Graphics g = Graphics.FromImage(paint1Bitmap))
                            g.FillRectangle(BrushSelect[4], _lsPaint1Mark[i - 1]);
                    }
                }
            }
            UpdatePaintBar();
        }

        public bool Split_Opt_Panel2_En
        {
            set
            {
                split_Opt.Panel2.Enabled = value;
            }
            get
            {
                return split_Opt.Panel2.Enabled;
            }
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

        private void btn_Prev_Click(object sender, EventArgs e)
        {
            TrackBarValueReduce();
        }

        private void btn_Next_MouseDown(object sender, MouseEventArgs e)
        {
            TrackBarValuePlus();

            if(!_work.IsBusy)
            {
                _watch.Restart();
                _work.RunWorkerAsync(ButtonStatus.Next);
            }
        }

        private void _work_DoWork(object sender, DoWorkEventArgs e)
        {
            var arg = (ButtonStatus)e.Argument;
            while(!_work.CancellationPending)
            {
                if(_watch.ElapsedMilliseconds >= 1000)
                {
                    if(arg == ButtonStatus.Next)
                        TrackBarValuePlus();
                    else if(arg == ButtonStatus.Previous)
                        TrackBarValueReduce();
                }
            }
        }

        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            _watch.Stop();
            _work.CancelAsync();
        }

        private void btn_Prev_MouseDown(object sender, MouseEventArgs e)
        {
            TrackBarValueReduce();

            if(!_work.IsBusy)
            {
                _watch.Restart();
                _work.RunWorkerAsync(ButtonStatus.Previous);
            }
        }

        private void btn_Load_Painting1_Click(object sender, EventArgs e)
        {
            if(GetButtonStatus != ButtonStatus.Stop)
            {
                if(StopPlayEvent != null)
                    StopPlayEvent(sender, e);
                ChangeStatus(ButtonStatus.Stop);
            }
            this.uiTrackBar1.ValueChanged -= uiTrackBar1_ValueChanged;
            if(LoadPainting1FileEvent != null)
                LoadPainting1FileEvent(sender, e);
            this.uiTrackBar1.ValueChanged += uiTrackBar1_ValueChanged;
            this.Focus(); //Steven 20220609 : Load檔案後獲取焦點
        }

        private void btn_Load_Painting2_Click(object sender, EventArgs e)
        {
            if(GetButtonStatus != ButtonStatus.Stop)
            {
                if(StopPlayEvent != null)
                    StopPlayEvent(sender, e);
                ChangeStatus(ButtonStatus.Stop);
            }
            this.uiTrackBar1.ValueChanged -= uiTrackBar1_ValueChanged;
            if(LoadPainting2FileEvent != null)
                LoadPainting2FileEvent(sender, e);
            this.uiTrackBar1.ValueChanged += uiTrackBar1_ValueChanged;
            this.Focus(); //Steven 20220609 : Load檔案後獲取焦點
        }

        private void btn_GenFile_Click(object sender, EventArgs e)
        {
            if(GenFileEvent != null)
                GenFileEvent(sender, e);
        }

        private void pPaint1Mark_Paint(object sender, PaintEventArgs e)
        {
            if(paint1Bitmap == null)
                return;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(paint1Bitmap, Point.Empty);
        }

        private void pPaint2Mark_Paint(object sender, PaintEventArgs e)
        {
            if(paint2Bitmap == null)
                return;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(paint2Bitmap, Point.Empty);
        }

        bool isEnableEdit = false;

        private void lb_Frame_Number_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lb_Frame_Number.Focus();
            isEnableEdit = true;
            lb_Frame_Number.BackColor = Color.Blue;
            sPrevioudText = lb_Frame_Number.Text;
        }
      
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
                    if((int)e.KeyCode < 96 || (int)e.KeyCode > 105)
                        return;
                }
                int iCode = ((int)e.KeyCode > 57) ? (int)e.KeyCode - 48 : (int)e.KeyCode;
                sTemp += Convert.ToChar(iCode);
                lb_Frame_Number.Text = sTemp;
            }
        }

     

    }
}