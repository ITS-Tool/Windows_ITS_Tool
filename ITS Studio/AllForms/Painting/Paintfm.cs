using CommonExt;
using CSharp_PlayBar;
using FuncMethodCmd;
using GDI_32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ThirdPartyTools;
using System.Text.RegularExpressions;
using Sunny.UI;

namespace ITS_Studio.AllForms
{
    public partial class Paintfm : Form
    {
        private int m_iSelectedDev = 0;
        public int SelectedDev
        {
            set
            {
                m_iSelectedDev = value;
                AssignedDev();
            }
            get
            {
                return m_iSelectedDev;
            }
        }
        Bitmap bitmap;
        Bitmap lastBit;
        private Graphics grp;
        private Graphics grpReplay;
        private int LABLE_PADDING = 20;
        private bool boFingerGriFlag = true;
        private bool bReportRate_Cleared = true;
        private Mutex mu_Clear = new Mutex();
        private Gdi32 MyOwnPen = new Gdi32();
        public Gdi32 GDI_Pen = new Gdi32();//move from CommonDefine\Variable.cs 20230306 Evayn
        private Mutex mu_bAlgo = new Mutex();
        private bool bAlgo_Cleared = true;
        private bool m_bFingerRelease = false;
        private int iFingerDrawingLength = 200;
        private bool _bAlgo_Cleared
        {
            set
            {
                mu_bAlgo.WaitOne();
                bAlgo_Cleared = value;
                if (mu_bAlgo != null)
                    mu_bAlgo.ReleaseMutex();
            }
            get
            {
                return bAlgo_Cleared;
            }
        }
        private UInt32 Pen_ID_Purple = 0;

        private delegate void function(Message msg);
        private Dictionary<enWM_MSG, function> mMessageReceiver = new Dictionary<enWM_MSG, function>();
        private _LabelLog LabelLog = new _LabelLog();
        private List<string> DataColumnName;
        private MultiPaintTool_VAR._VirtualBtn[] VirtualBtn = new MultiPaintTool_VAR._VirtualBtn[2];
        private Font arialBold = new Font("Consolas", 20.0F, FontStyle.Bold);
        private GraphicsPath Btn_ClearRange = new GraphicsPath();
        private GraphicsPath Btn_ExitRange = new GraphicsPath();
        private Gdi32 BtnPen;
        private int iReplay_SelectedIndex = 0;
        private bool isNeedUpdateDBGUI = true;
        private bool isFinishReplayByScrollBar = true;
        public double tempFingerSNR = 0.0f;
        Dictionary<enPenSNRX, double> TempPenSNR_X = new Dictionary<enPenSNRX, double>();
        Dictionary<enPenSNRY, double> TempPenSNR_Y = new Dictionary<enPenSNRY, double>();

        Dictionary<enPenSNRX, List<float>> DicSNRXCalculate = new Dictionary<enPenSNRX, List<float>>();
        Dictionary<enPenSNRY, List<float>> DicSNRYCalculate = new Dictionary<enPenSNRY, List<float>>();

        Dictionary<string, int> DicPenDebugInfo = new Dictionary<string, int>();

        protected DynamicTool MyTool = new DynamicTool();

        public enum enFigGridCol { Name = 0, Col_X, Col_Y, Width, Height, Pressure, MaxGapRatio, Jitter }

        public enum enKeyGridCol { Name = 0, Pressure }

        public enum enKortekAlgoGridCol { Name = 0, Normal, Mist, Water, Hopping, Thumb, Palm }

        private System.Timers.Timer Timer_FingerGrid = new System.Timers.Timer();

        private System.Timers.Timer Timer_ReportRate = new System.Timers.Timer();

        private INI_Tool MY_INI = new INI_Tool(ITS_Directory.PaintTool_INI);

        public enum DataFormatSection : int
        {
            Gap_Ratio = 0,
            Jitter,
            Width,
            Height,
            Pressure,
        }

        public Dictionary<DataFormatSection, DataGridViewTextBoxColumn> DicDataShow = new Dictionary<DataFormatSection, DataGridViewTextBoxColumn>();

        public Paintfm()
        {
            InitializeComponent();
            bitmap = new Bitmap(m_panAAscen.Width, m_panAAscen.Height);
            lastBit = new Bitmap(m_panAAscen.Width, m_panAAscen.Height);
          
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            StaticVar.PaintToolFormHandle = this.Handle;
            //m_panAAscen.BackColor = Color.FromArgb(0, 255, 255, 255);//設成透明背景色
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic
                , null, m_panAAscen, new object[] { true });

            var _PenHandle = GDI_Pen.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 2, Color.Black);
            GDI_Pen.AssignPenHandle(_PenHandle);
            UI_GBV.mDev[SelectedDev].LocalVariable.stuReportIndex = new MultiPaintTool_VAR.ReportIndex();

            // Init DataGridView System Data
            splitContainer_PaintMain.Panel1Collapsed = true;
            splitContainer_PaintMain.Panel1.Hide();
            splitContainer_PaintMain.IsSplitterFixed = true;

            splitKortek.Panel1Collapsed = true;
            splitKortek.Panel1.Hide();
            splitKortek.IsSplitterFixed = true;

            m_dgvKeyStatus.Rows.Clear();

            // ====================
            // Timer_FingerGrid
            Timer_FingerGrid.Enabled = false;
            Timer_FingerGrid.Interval = 33;
            Timer_FingerGrid.Elapsed += new System.Timers.ElapsedEventHandler(m_tiFingerGri_Tick);
            Timer_FingerGrid.AutoReset = false;

            // ====================
            // Timer_ReportRate
            Timer_ReportRate.Enabled = false;
            Timer_ReportRate.Interval = 1000;
            Timer_ReportRate.Elapsed += new System.Timers.ElapsedEventHandler(m_tiReportRate_Tick);
            Timer_ReportRate.AutoReset = true;
            Timer_ReportRate.SynchronizingObject = this;


            //Timer_Bar.Enabled = false;
            //Timer_Bar.Interval = 100;
            //Timer_Bar.Elapsed += new System.Timers.ElapsedEventHandler(timerBar_Tick);
            //Timer_Bar.AutoReset = true;
            //Timer_Bar.SynchronizingObject = this;

            Option_ToolStripMenuItem.Visible = false;
            vfLoadSetting(SelectedDev);
            GetDebugAddressRange();
            CreatToolStripComboBoxList();

            mMessageReceiver.Add(enWM_MSG.WM_PaintReplayFrame, Paint_Replay_Frame);


            grpReplay = this.m_panAAscen.CreateGraphics();
            //Graphics grp = this.m_panAAscen.CreateGraphics();
            MyOwnPen.BeginGDI(grpReplay);

            var _handle = MyOwnPen.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 2, Color.White);
            MyOwnPen.AssignPenHandle(_handle);
            DicDataShow.Clear();
            foreach (var item in EnumTool.EnumToList<DataFormatSection>())
            {
                DataGridViewTextBoxColumn columnData = new DataGridViewTextBoxColumn();
                columnData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                columnData.HeaderText = item.ToString();
                columnData.MinimumWidth = 40;
                columnData.Name = string.Format("m_dgvFigerCol_{0}", item.ToString());
                columnData.ReadOnly = true;
                columnData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                columnData.Width = 40;

                DicDataShow.Add(item, columnData);
            }

            m_panAAscen.BackColor = Color.FromArgb((int)UI_GBV.mDev[0].LocalVariable.PaintTool_Var.BackGroundColor);

            this.dataGridView_FingerDebugInfo.DoubleBuffered(true);
            this.dataGridView_PenDebugInfo.DoubleBuffered(true);
            this.m_dgvAlgoData.DoubleBuffered(true);
            this.m_dgvFingerStatus.DoubleBuffered(true);

            // Init Variable            
            foreach (int iDev in Enumerable.Range(0, StaticVar.iMaxSupportDev))
            {
                UI_GBV.mDev[iDev].LocalVariable.PaintTool_Var.SelectBasicColor.Add(enBackGroundColor.White, UI_GBV.mDev[SelectedDev].LocalVariable.PaintTool_Var._BasicColor_);
                UI_GBV.mDev[iDev].LocalVariable.PaintTool_Var.SelectBasicColor.Add(enBackGroundColor.Black, UI_GBV.mDev[SelectedDev].LocalVariable.PaintTool_Var._BlackBackGroundBasicColor_);
                int iMaxFingers = UI_GBV.mDev[SelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[UI_GBV.mDev[SelectedDev].LocalVariable.PaintTool_Var.BackGroundColor].Count();
                UI_GBV.mDev[SelectedDev].LocalVariable.PaintTool_Var.coFingerColor = new Color[iMaxFingers];
                UI_GBV.mDev[SelectedDev].LocalVariable.PaintTool_Var.inFingerColorRBG = new Color[iMaxFingers];
                UI_GBV.mDev[SelectedDev].LocalVariable.PaintTool_Var._ColorPen = new IntPtr[iMaxFingers];
            }
        }

        private void AssignedDev()
        {
            m_panAAscen.BackColor = Color.FromArgb((int)UI_GBV.mDev[SelectedDev].LocalVariable.PaintTool_Var.BackGroundColor);
        }

        protected override void WndProc(ref Message msg)
        {
            enWM_MSG pEnumPanoMsg = (enWM_MSG)msg.Msg;
            //Console.WriteLine("WndProc() = {0}", pEnumPanoMsg);
            if(mMessageReceiver.Keys.Contains(pEnumPanoMsg))
                mMessageReceiver[pEnumPanoMsg](msg);
            base.WndProc(ref msg);
        }

        public void AssignReplayBar()
        {
            userControl_ReplayPaint.AssignToolFunc(StaticVar.NowExecuteTool, this.userControl_ReplayPaint, UI_GBV.mDev[m_iSelectedDev]);
        }

        public void InitAlgoModePara()
        {
            string strSector = "";
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                strSector = "DataColumnName_V6";
            else if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                strSector = "DataColumnName_V3";

            List<string> HeaderNames = new List<string>();
            foreach (var item in EnumTool.EnumToList<PaintTool.enAlgoNameRowIndex>())
            {
                if (item == PaintTool.enAlgoNameRowIndex._Report_Rate)
                    continue;
                HeaderNames.Add(MY_INI.IniReadValue(strSector, item.ToString(), item.Desc()));
            }
            DataColumnName = new List<string>(HeaderNames);
        }

        public void ShowFingerCoor_Grid(bool boFingerRelease)
        {
            if ((!boFingerGriFlag) && (!boFingerRelease))
                return;
            if (m_dgvFingerStatus.Rows.Count == 0)
                return;
            boFingerGriFlag = false;//restart flag status
            // finger status
            bool show_performace = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Performance_En;

            bool isContainWidth = m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Width]);
            bool isContainHeight = m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Height]);
            bool isContainPressure = m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Pressure]);

            for (int inCount = 0; inCount < UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iMaxTouchPoint; inCount++)
            {
                bool bTouch = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].bTouch;
                int curr_x = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].iPosition_X;
                int curr_y = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].iPosition_Y;

                int iOffset = 0;

                m_dgvFingerStatus.Rows[inCount].Cells[(int)enFigGridCol.Col_X].Value = bTouch ? curr_x.ToString() : "0";
                m_dgvFingerStatus.Rows[inCount].Cells[(int)enFigGridCol.Col_Y].Value = bTouch ? curr_y.ToString() : "0";
                if (isContainWidth)
                {
                    int width = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].iWidth;
                    m_dgvFingerStatus.Rows[inCount].Cells[(int)enFigGridCol.Width].Value = bTouch ? width.ToString() : "0";

                }
                else
                {
                    iOffset++;
                }
                   
                if (isContainHeight)
                {
                    int height = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].iHeight;
                    m_dgvFingerStatus.Rows[inCount].Cells[(int)enFigGridCol.Height].Value = bTouch ? height.ToString() : "0";
                }
                else
                {
                    iOffset++;
                }
                    

                if (isContainPressure)
                {
                    int index = (isContainWidth && isContainHeight) ? 0 : 2;
                    int pressure = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].iPressure;
                    m_dgvFingerStatus.Rows[inCount].Cells[(int)enFigGridCol.Pressure - index].Value = bTouch ? pressure.ToString() : "0";    
                }
                else 
                {
                    iOffset++;
                }

                /* Perfomance-related update */
                if (!show_performace)
                    continue;
                m_dgvFingerStatus.Rows[inCount].Cells[(int)enFigGridCol.MaxGapRatio - iOffset].Value =
                    bTouch ? UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.performance[inCount].max_gap_ratio.ToString("#0.000") : "0";
                m_dgvFingerStatus.Rows[inCount].Cells[(int)enFigGridCol.Jitter - iOffset].Value =
                    bTouch ? UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.performance[inCount].max_jitter.ToString("#0.000") : "0";
            }

            // key status
            for (int inCount = 0; inCount < UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts; inCount++)
            {
                Color tmpColor = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.strKey_Old[inCount].bTouch) ? Color.Yellow : Color.White;
                m_dgvKeyStatus.Rows[inCount].Cells[(int)enKeyGridCol.Name].Style.BackColor = tmpColor;
            }

            Timer_FingerGrid.Enabled = true;
        }

        public void ShowFingerCoor_Lable()
        {
            // Finger Label
            bool isContainWidth = m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Width]);
            bool isContainHeight = m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Height]);
            bool isContainPressure = m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Pressure]);

            for (int inCount = 0; inCount < UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iMaxTouchPoint; inCount++)
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].bTouch)
                {
                    string sFingerDetailData = string.Format("Finger_{0:D2} : {1:D04} , {2:D04}"
                        , inCount + 1
                        , UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].iPosition_X
                        , UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].iPosition_Y);
                    iFingerDrawingLength = 180;

                    if (isContainWidth)
                    {
                        sFingerDetailData += string.Format(" ,{0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].iWidth);
                        iFingerDrawingLength += 45;
                    }
                    if (isContainHeight)
                    {
                        sFingerDetailData += string.Format(" ,{0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].iHeight);
                        iFingerDrawingLength += 45;
                    }
                    if (isContainPressure)
                    {
                        sFingerDetailData += string.Format(" ,{0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].iPressure);
                        iFingerDrawingLength += 45;
                    }
                        
                    SetFingerInfo(inCount,sFingerDetailData);
                }
                else
                {
                    SetFingerInfo(inCount, "");
                }
            }
        }

        public void ShowPenCoor_Lable()
        {
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_Old[0].bTouch)
            {
                SetPenInfo(0, string.Format("Pen : {0:D04} , {1:D04}"
                    , UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_Old[0].iPosition_X
                    , UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_Old[0].iPosition_Y));
            }
            else
            {
                SetPenInfo(0, "");
            }
        }

        public void vfChangeResolution()
        {
            m_panAAscen.Dock = DockStyle.Fill; //為了固定初始大小，故意設成Fill
            int inHeight = m_panAAscen.Height;
            int inWidth = m_panAAscen.Width;
            bitmap = new Bitmap(inWidth, inHeight);
            lastBit = new Bitmap(inWidth, inHeight);
            splitContainer1.SplitterDistance = this.Height * 8 / 10;

            foreach (ToolStripItem item in Resolution_ToolStripMenuItem.DropDownItems)
            {
                ToolStripComboBox SelectedItem = item as ToolStripComboBox;
                if (SelectedItem.Text == ScreenList.Keep_aspect_ratio.ToString())
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.ScreenShow = ScreenList.Keep_aspect_ratio;
                    m_panAAscen.Height = inHeight - 30;
                    m_panAAscen.Width = inWidth - 30;
                    m_panAAscen.Dock = DockStyle.None;
                    m_panAAscen.Location = new System.Drawing.Point(15, 15);

                }
                else if (SelectedItem.Text == ScreenList.Fill_Form.ToString())
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.ScreenShow = ScreenList.Fill_Form;
                    m_panAAscen.Location = new System.Drawing.Point(0, 0);
                    m_panAAscen.Dock = DockStyle.Fill;
                    m_panAAscen.Height = inHeight;
                    m_panAAscen.Width = inWidth;
                    //m_panAAscen.Dock = DockStyle.Fill;
                }
            }

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize = new System.Drawing.Size(m_panAAscen.Width, m_panAAscen.Height);
            var PanelSize = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize;

            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool != null)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.MappingResolution(PanelSize, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.X_RES, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.Y_RES);

            MY_INI.IniWriteValue("Setting", "ScreenList", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.ScreenShow.ToString());

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo != null)
            {
                if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts > 0) &&
                   UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.isI2C(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType) &&
                   !StaticVar.IsPlayback_Mode)
                    FillKey_ScreenPosition();


                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.FullScreen)
                {
                    if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.douScale_Height <= 0) || (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.douScale_Width <= 0))
                        return;
                    //MyTool.Delay_ms(500);
                    if (!StaticVar.bPaint_Record && !StaticVar.IsPlayback_Mode)
                    {
                        InitLabelLog();
                        InitVirtualButton();
                    }

                    if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts > 0) &&
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.isI2C(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType))
                    {
                        PaintKeyRect();
                    }
                }
                else if (StaticVar.IsPlayback_Mode)
                {
                    if (m_dgvAlgoData.Rows.Count > 0)
                        m_dgvAlgoData.Rows[m_dgvAlgoData.Rows.Count - 1].Cells[0].Value = 0;
                }
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.iErrPktCnts = 0;
                if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool != null)
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.Parameter_Init_API(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize);//手指資料初始化
            }
            m_panAAscen.Invalidate();
            DrawPanelLine();
            DrawPattern(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Pattern);
        }

        private bool isTouchDown = false;

        private Pen drawPen;

        private Color PenColor = Color.Red;

        private void PenMove(int iFingerNum, Point Old_Position, Point New_Position, int iOld_Pressure, int iNew_Pressure,UInt32 iPen_ID)
        {
            ///===================
            /// Touch Move
            ///===================
            var NewPen = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_New[iFingerNum];
            // Touch dowm
            int iNumNew = 2 + ((iNew_Pressure / 200) * (int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PenPressure);
            int iNumOld = 2 + ((iOld_Pressure / 200) * (int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PenPressure);
            //panel_Hover.Location = new Point(New_Position.X - (panel_Hover.Width / 2), New_Position.Y - (panel_Hover.Height / 2));
                              
            PenColor = (iPen_ID == Pen_ID_Purple) ? Color.Purple : Color.Red;
            if (NewPen.bTouch)
            {
                if (isFinishReplayByScrollBar)
                {
                    var _PenHandle = GDI_Pen.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, iNumOld, PenColor);
                    GDI_Pen.AssignPenHandle(_PenHandle);
                    GDI_Pen.MoveTo(Old_Position);
                    GDI_Pen.DeleteOBJECT(_PenHandle);

                    _PenHandle = GDI_Pen.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, iNumNew, PenColor);
                    GDI_Pen.AssignPenHandle(_PenHandle);
                    GDI_Pen.LineTo(New_Position);
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bIsShowTrajectoryDot)
                        GDI_Pen.Ellipse(New_Position, 3);
                    GDI_Pen.DeleteOBJECT(_PenHandle);                   
                }


                //Console.WriteLine("{0} -> {1}", Old_Position, New_Position);

                drawPen = new Pen(PenColor, iNumNew);
                drawPen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
                drawPen.LineJoin = LineJoin.Round;
                using (grp = Graphics.FromImage(bitmap))
                {
                    grp.DrawLine(drawPen, Old_Position, New_Position);
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bIsShowTrajectoryDot)
                        grp.DrawEllipse(drawPen, New_Position.X - 6 / 2, New_Position.Y - 6 / 2, 6, 6);
                }
                                
                panel_Hover.Visible = false;

                isTouchDown = true;
                StatusLabel_X.Text = string.Format("X = {0:00000}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_Old[iFingerNum].iPosition_X);
                StatusLabel_Y.Text = string.Format("Y = {0:00000}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_Old[iFingerNum].iPosition_Y);

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_Old[iFingerNum] = new MultiPaintTool_VAR.PenStatus(NewPen);

            }
            else//Touch up
            {
                //20220419 Steven : Touch down有重新設定筆顏色，Touch up也補上
                drawPen = new Pen(PenColor, iNumNew);
                drawPen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
                drawPen.LineJoin = LineJoin.Round;

                if (!Point.Equals(Old_Position, New_Position))
                {
                    if (isFinishReplayByScrollBar)
                    {
                        GDI_Pen.MoveTo(Old_Position);
                        GDI_Pen.LineTo(New_Position);
                        if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bIsShowTrajectoryDot)
                            GDI_Pen.Ellipse(New_Position, 3);
                    }
                    using (grp = Graphics.FromImage(bitmap))
                    {
                        grp.DrawLine(drawPen, Old_Position, New_Position);
                        if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bIsShowTrajectoryDot)
                            grp.DrawEllipse(drawPen, New_Position.X - 6 / 2, New_Position.Y - 6 / 2, 6, 6);
                    }                                        
                }
                //20220503 Steven : 將TouchUpCircle_En移動到畫圈位置而不是Hover位置
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.TouchUpCircle_En)
                {
                    if (isTouchDown)
                    {
                        using (grp = this.m_panAAscen.CreateGraphics())
                            grp.DrawEllipse(drawPen, New_Position.X - 10 / 2, New_Position.Y - 10 / 2, 10, 10);

                        using (grp = Graphics.FromImage(bitmap))
                            grp.DrawEllipse(drawPen, New_Position.X - 10 / 2, New_Position.Y - 10 / 2, 10, 10);

                    }
                }
             
                panel_Hover.Visible = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.HoverCircle_En;
                panel_Hover.Location = new Point(New_Position.X - (panel_Hover.Width / 2), New_Position.Y - (panel_Hover.Height / 2));

                if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.FullScreen))  //Full Screen paint / 1 finger
                {
                    if (Btn_ClearRange.IsVisible(Old_Position) && isTouchDown)
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintFullScreenClear, (IntPtr)0, (IntPtr)0);
                    else if (Btn_ExitRange.IsVisible(Old_Position) && isTouchDown)
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_PostMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintFullScreenExit, (IntPtr)0, (IntPtr)0);
                }
                StatusLabel_X.Text = string.Format("X = {0:00000}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_Old[iFingerNum].iPosition_X);
                StatusLabel_Y.Text = string.Format("Y = {0:00000}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_Old[iFingerNum].iPosition_Y);

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_Old[iFingerNum] = new MultiPaintTool_VAR.PenStatus();
                iNew_Pressure = 0;
                if (isTouchDown)
                    isTouchDown = false;
            }

            double dTilt = 0d;
            double dAzimuth = 0d;

            StatusLabel_TipSwitch.BackColor = (NewPen.bTouch) ? Color.Yellow : Color.Transparent;
            StatusLabel_BarrelSwitch.BackColor = (NewPen.bBarrelSwitch) ? Color.Yellow : Color.Transparent;
            StatusLabel_Erase.BackColor = (NewPen.bErase) ? Color.Yellow : Color.Transparent;
            StatusLabel_Invert.BackColor = (NewPen.bInvert) ? Color.Yellow : Color.Transparent;
            StatusLabel_InRange.BackColor = (NewPen.bInRange) ? Color.Yellow : Color.Transparent;

            panel_Hover.Visible = (NewPen.bInRange && !isTouchDown) && UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.HoverCircle_En;

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PenTiltType == enPenTiltType.XY_Tilt)
            {
                StatusLabel_X_Tilt.Visible = true;
                StatusLabel_Y_Tilt.Visible = true;
                StatusLabel_X_Tilt.Text = string.Format("X Tilt = {0:0000}", NewPen.iX_Tilt);
                StatusLabel_Y_Tilt.Text = string.Format("Y Tilt = {0:0000}", NewPen.iY_Tilt);

                StatusLabel_Tilt.Visible = false;
                StatusLabel_Azimuth.Visible = false;
            }
            else
            {
                StatusLabel_Tilt.Visible = true;
                StatusLabel_Azimuth.Visible = true;
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.GetTiltAndAzimuth(NewPen.iX_Tilt, NewPen.iY_Tilt, ref dTilt, ref dAzimuth);
                StatusLabel_Tilt.Text = string.Format("Tilt = {0:0000}", dTilt);
                StatusLabel_Azimuth.Text = string.Format("Azimuth = {0:0000}", dAzimuth);

                StatusLabel_X_Tilt.Visible = false;
                StatusLabel_Y_Tilt.Visible = false;
            }
            StatusLabel_Pressure.Text = string.Format("Pen Pressure = {0:0000}", iNew_Pressure);
           

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Performance_En)
            {
                StatusLabel_GapRatio.Visible = true;
                StatusLabel_Jitter.Visible = true;
                StatusLabel_GapRatio.Text = string.Format("GapRatio = {0:000.00}",
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stylus_performance.max_gap_ratio);
                StatusLabel_Jitter.Text = string.Format("Jitter = {0:00000.00}",
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stylus_performance.max_jitter);
            }
            else
            {
                StatusLabel_GapRatio.Visible = false;
                StatusLabel_Jitter.Visible = false;
            }

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_New[iFingerNum] = new MultiPaintTool_VAR.PenStatus();
        }

        private void TouchMove(int iFingerNum, Point Old_Position, Point New_Position)
        {
            ///===================
            /// Touch Move
            ///===================
            // Touch dowm
            drawPen = new Pen(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inFingerColorRBG[iFingerNum], 2.0f);
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_New[iFingerNum].bTouch)
            {
                if (isFinishReplayByScrollBar)
                {
                    GDI_Pen.MoveTo(Old_Position);
                    GDI_Pen.LineTo(New_Position);

                    if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bIsShowTrajectoryDot)
                        GDI_Pen.Ellipse(New_Position, 3);
                }

                var tmp = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_New[iFingerNum];
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[iFingerNum] = new MultiPaintTool_VAR.FingerStatus(tmp.bDataIn, tmp.bTouch, tmp.iPosition_X, tmp.iPosition_Y, tmp.iWidth,tmp.iHeight,tmp.iPressure);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_New[iFingerNum] = new MultiPaintTool_VAR.FingerStatus();

                using (grp = Graphics.FromImage(bitmap))
                {
                    grp.DrawLine(drawPen, Old_Position, New_Position);
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bIsShowTrajectoryDot)
                        grp.DrawEllipse(drawPen, New_Position.X - 6 / 2, New_Position.Y - 6 / 2, 6, 6);
                }
                   
            }
            else//Touch up
            {
                drawPen = new Pen(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inFingerColorRBG[iFingerNum], 2.0f);

                m_bFingerRelease = true;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_lsSNR_Value.Clear();
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts = 0;

                if (!Point.Equals(Old_Position, New_Position))
                {
                    if (isFinishReplayByScrollBar)
                    {
                        GDI_Pen.MoveTo(Old_Position);
                        GDI_Pen.LineTo(New_Position);
                        if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bIsShowTrajectoryDot)
                            GDI_Pen.Ellipse(New_Position, 3);
                    }
                    using (grp = Graphics.FromImage(bitmap))
                    {
                        grp.DrawLine(drawPen, Old_Position, New_Position);
                        if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bIsShowTrajectoryDot)
                            grp.DrawEllipse(drawPen, New_Position.X - 6 / 2, New_Position.Y - 6 / 2, 6, 6);
                    }
                        
                }

                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.TouchUpCircle_En)
                {
                    //Key不要畫圈圈
                    if(!UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bKeyChanged)
                    {
                        using(grp = this.m_panAAscen.CreateGraphics())
                            grp.DrawEllipse(drawPen, New_Position.X - 10 / 2, New_Position.Y - 10 / 2, 10, 10);
                        using(grp = Graphics.FromImage(bitmap))
                            grp.DrawEllipse(drawPen, New_Position.X - 10 / 2, New_Position.Y - 10 / 2, 10, 10);
                    }
                }


                if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.FullScreen)
                    && (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.iFingerID == 1))  //Full Screen paint / 1 finger
                {
                    if (Btn_ClearRange.IsVisible(Old_Position))
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintFullScreenClear, (IntPtr)0, (IntPtr)0);
                    else if (Btn_ExitRange.IsVisible(Old_Position))
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_PostMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintFullScreenExit, (IntPtr)0, (IntPtr)0);
                }
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[iFingerNum] = new MultiPaintTool_VAR.FingerStatus();
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_New[iFingerNum] = new MultiPaintTool_VAR.FingerStatus();
                //panel1.BringToFront();
            }
        }

        private Action<int, Point, Point> TouchMoveAction
        {
            get;
            set;
        }

        private Action<int, Point, Point, int, int,UInt32> PenMoveAction
        {
            get;
            set;
        }

        public void DrawingLine_Pen()
        {
            var scale_W = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.douScale_Width;
            var scale_H = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.douScale_Height;

            var oldPen = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_Old[0];
            var newPen = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_New[0];
            var OldPenPressure = oldPen.iPressure;
            var NewPenPressure = newPen.iPressure;
           
            //20240813 Steven : 拿掉這段，不然Painting有機會斷線
            //bool isDataClear = !UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_Old[0].bInRange && !UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuPen_New[0].bInRange;
            //if (isDataClear && (oldPen.iPosition_X == 0 && oldPen.iPosition_Y == 0 && newPen.iPosition_X == 0 && newPen.iPosition_Y == 0))
            //    return;

            //    Console.Write("{0} and {1}\r\n", newPen.iPosition_X, newPen.iPosition_Y);
            Point Old_Position = new Point((int)(oldPen.iPosition_X * scale_W), (int)(oldPen.iPosition_Y * scale_H));
            Point New_Position = new Point((int)(newPen.iPosition_X * scale_W), (int)(newPen.iPosition_Y * scale_H));

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bSWAPXY)
            {
                Old_Position = new Point((int)(oldPen.iPosition_Y * scale_W), (int)(oldPen.iPosition_X * scale_H));
                New_Position = new Point((int)(newPen.iPosition_Y * scale_W), (int)(newPen.iPosition_X * scale_H));
            }

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.SpinDirection.Contains(enSpinDirection.Horizontal))
            {
                Old_Position.X = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Width - Old_Position.X;
                New_Position.X = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Width - New_Position.X;
            }

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.SpinDirection.Contains(enSpinDirection.Vertical))
            {
                Old_Position.Y = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Height - Old_Position.Y;
                New_Position.Y = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Height - New_Position.Y;
            }
            UInt32 Pid = oldPen.iPen_ID != newPen.iPen_ID ? oldPen.iPen_ID : newPen.iPen_ID;

            panel_Hover.BackgroundImage = (Pid == Pen_ID_Purple) ? ITS_Studio.Properties.Resources.HoverCircle2 : ITS_Studio.Properties.Resources.HoverCircle;

            PenMoveAction = PenMove;
            PenMoveAction.Invoke(0, Old_Position, New_Position, OldPenPressure, NewPenPressure, Pid);
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.FullScreen)
            {
                ShowPenCoor_Lable();
            }
        }

        private double CalAngle(Point pa, Point pb)
        {
            /// Y alias is reverse from Cartesian plane
            var _value = Math.Abs(Math.Atan2(pa.Y - pb.Y, pb.X - pa.X) * 180 / Math.PI);
            return _value > 90 ? (180 - _value) : _value;
        }

        public void DrawingLine_Finger()
        {
            int iFingerNum = 0;

            m_bFingerRelease = false;
            bool bDataContinueIn = false;
            var scale_W = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.douScale_Width;
            var scale_H = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.douScale_Height;
            for (int iFingIndx = 0; iFingIndx < UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.iFingerID; iFingIndx++)
            {
                iFingerNum = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.iChangeFlag[iFingIndx];
                var oldFinger = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[iFingerNum];
                var newFinger = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_New[iFingerNum];

                bDataContinueIn = oldFinger.bDataIn && newFinger.bDataIn;

                Point Old_Position = new Point((int)(oldFinger.iPosition_X * scale_W), (int)(oldFinger.iPosition_Y * scale_H));
                Point New_Position = new Point((int)(newFinger.iPosition_X * scale_W), (int)(newFinger.iPosition_Y * scale_H));

                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bSWAPXY)
                {
                    Old_Position = new Point((int)(oldFinger.iPosition_Y * scale_W), (int)(oldFinger.iPosition_X * scale_H));
                    New_Position = new Point((int)(newFinger.iPosition_Y * scale_W), (int)(newFinger.iPosition_X * scale_H));
                }

                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.SpinDirection.Contains(enSpinDirection.Horizontal))
                {
                    Old_Position.X = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Width - Old_Position.X;
                    New_Position.X = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Width - New_Position.X;
                }

                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.SpinDirection.Contains(enSpinDirection.Vertical))
                {
                    Old_Position.Y = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Height - Old_Position.Y;
                    New_Position.Y = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Height - New_Position.Y;
                }

                if (!bDataContinueIn)
                    continue;

                GDI_Pen.AssignPenHandle(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ColorPen[iFingerNum]);
                TouchMoveAction = TouchMove;
                TouchMoveAction.Invoke(iFingerNum, Old_Position, New_Position);

            }

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bKeyChanged)
            {
                for (int inCount = 0; inCount < UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts; inCount++)
                {
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.FullScreen)
                    {
                        var MyTest = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition[inCount];
                        Rectangle rect1 = new Rectangle(MyTest.Start_Point, new Size(MyTest.Btn_W, MyTest.Btn_H));
                        SolidBrush _BrushesColor;
                        if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.strKey_New[inCount].bTouch))
                            _BrushesColor = new SolidBrush(Color.Black);
                        else
                            _BrushesColor = new SolidBrush(Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor));

                        GDI_Pen.FillRoundRectangle(Graphics.FromHwnd(this.m_panAAscen.Handle), _BrushesColor, rect1, 8);
                    }
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.strKey_Old[inCount].bTouch = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.strKey_New[inCount].bTouch;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.strKey_New[inCount].bTouch = false;
                }
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bKeyChanged = false;
            }
            //============================================
            // Show Finger Information
            //============================================
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.FullScreen)
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Display_AlgoInfo)
                    SetAlgoInfo(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.AlgoMode, true);

                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.DisplayCoordinate)
                    ShowFingerCoor_Lable();
            }
            else
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Display_AlgoInfo)
                {
                    Action UpdateDisplay_AlgoInfoAction = UpdateDisplay_AlgoInfo;
                    UpdateDisplay_AlgoInfoAction.Invoke();
                }

                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.DisplayCoordinate)
                {
                    Action<bool> ShowFingerCoor_GridAction = ShowFingerCoor_Grid;
                    ShowFingerCoor_GridAction.Invoke(m_bFingerRelease);
                }
            }
        }

        private void UpdateDisplay_AlgoInfo()
        {
            int iBitCnts = 0;
            foreach (var item in EnumTool.EnumToList<PaintTool.enAlgoNameRowIndex>())
            {
                if (item == PaintTool.enAlgoNameRowIndex._Report_Rate)
                    continue;

                int iValue = MyTool.GetIntegerSomeBit(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.AlgoMode, iBitCnts);
                m_dgvAlgoData.Rows[(int)item].Cells[0].Value = (iValue == 1) ? "Active" : "Inactive";
                iBitCnts++;
            }

            if (splitKortek.Panel1Collapsed == false)
            {
                for (int inCount = 0; inCount < UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iMaxTouchPoint; inCount++)
                {
                    bool bTouch = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.stuFinger_Old[inCount].bTouch;
                    int Normal = MyTool.GetIntegerSomeBit(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Algo_Kortek[inCount], 0);
                    int Mist = MyTool.GetIntegerSomeBit(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Algo_Kortek[inCount], 1);
                    int Water = MyTool.GetIntegerSomeBit(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Algo_Kortek[inCount], 2);
                    int Hopping = MyTool.GetIntegerSomeBit(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Algo_Kortek[inCount], 3);
                    int Thumb = MyTool.GetIntegerSomeBit(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Algo_Kortek[inCount], 6);
                    int Palm = MyTool.GetIntegerSomeBit(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Algo_Kortek[inCount], 7);

                    m_dgvKortek.Rows[inCount].Cells[(int)enKortekAlgoGridCol.Normal].Value = bTouch ? ((Normal == 1) ? "Active" : "Inactive") : "";
                    m_dgvKortek.Rows[inCount].Cells[(int)enKortekAlgoGridCol.Mist].Value = bTouch ? ((Mist == 1) ? "Active" : "Inactive") : "";
                    m_dgvKortek.Rows[inCount].Cells[(int)enKortekAlgoGridCol.Water].Value = bTouch ? ((Water == 1) ? "Active" : "Inactive") : "";
                    m_dgvKortek.Rows[inCount].Cells[(int)enKortekAlgoGridCol.Hopping].Value = bTouch ? ((Hopping == 1) ? "Active" : "Inactive") : "";
                    m_dgvKortek.Rows[inCount].Cells[(int)enKortekAlgoGridCol.Thumb].Value = bTouch ? ((Thumb == 1) ? "Active" : "Inactive") : "";
                    m_dgvKortek.Rows[inCount].Cells[(int)enKortekAlgoGridCol.Palm].Value = bTouch ? ((Palm == 1) ? "Active" : "Inactive") : "";

                }
            }
            m_dgvAlgoData.Rows[m_dgvAlgoData.Rows.Count - 1].Cells[0].Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.iErrPktCnts;
        }

        private void SetReportRate(int iReportRate)
        {
            if (iReportRate != 0)
                Update_LabelLogText(ref LabelLog, _LabelLog.Label_Type.ReportRate, 0, string.Format("Report Rate : {0}", iReportRate));
            else
                Update_LabelLogText(ref LabelLog, _LabelLog.Label_Type.ReportRate, 0, "");
        }

        private void SetAlgoInfo(int Number, bool bEnable)
        {
            int iBitCnts = 0;
            foreach (var item in EnumTool.EnumToList<PaintTool.enAlgoNameRowIndex>())
            {
                if (item == PaintTool.enAlgoNameRowIndex._Report_Rate)
                    continue;

                if (MyTool.GetIntegerSomeBit(Number, iBitCnts) == 1)
                    Update_LabelLogText(ref LabelLog, _LabelLog.Label_Type.Algo, iBitCnts, bEnable ? DataColumnName[iBitCnts] : "");
                else
                    Update_LabelLogText(ref LabelLog, _LabelLog.Label_Type.Algo, iBitCnts, "");
                iBitCnts++;
            }
        }

        private void SetFingerInfo(int Number, string str)
        {
            Update_LabelLogText(ref LabelLog, _LabelLog.Label_Type.Finger, Number, str);
        }

        private void SetPenInfo(int Number, string str)
        {
            Update_LabelLogText(ref LabelLog, _LabelLog.Label_Type.Pen, Number, str);
        }

        private void InitFingerColor()
        {
            int itmp = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iMaxTouchPoint;
            for (int inCount = 0; inCount < itmp; inCount++)
            {
                var _Color = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor][inCount]);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.coFingerColor[inCount] = _Color;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inFingerColorRBG[inCount] = _Color;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ColorPen[inCount] = GDI_Pen.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 2, _Color);// Pen
            }
        }

        public void Paint_Toolfm_Init()
        {
            this.Visible = false;
            Paint_Record.Enabled = !StaticVar.IsPlayback_Mode;
            splitContainer_Collapsed(StaticVar.IsPlayback_Mode);
            userControl_ReplayPaint.TrackBar_En = true; //20220613 Steven : 支援滑鼠拖動
            vfChangeResolution();
            this.Visible = true;
        }

        private int SelectedDevIndex = 0;

        public void UpdateUIByFormat()
        {
            CMD_V6.ReportFormat reportFormat = (CMD_V6.ReportFormat)Enum.Parse(typeof(CMD_V6.ReportFormat), UI_GBV.mDev[SelectedDev].LocalVariable.Var_Multi.DevPanelInfo.ReportFormatV6);
            string sUISetting = reportFormat.Desc();
            string[] sUISettingArrary = sUISetting.Split(';');

            if (reportFormat == CMD_V6.ReportFormat.Format_10)
            {
                splitKortek.Panel1Collapsed = false;
                splitKortek.Panel1.Show();
            }
            else
            {
                splitKortek.Panel1Collapsed = true;
                splitKortek.Panel1.Hide();
            }

            if (sUISettingArrary[0].Contains("True"))
            {
                if (!m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Width]))
                    m_dgvFingerStatus.Columns.Add(DicDataShow[DataFormatSection.Width]);
            }
            else
            {
                if (m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Width]))
                    m_dgvFingerStatus.Columns.Remove(DicDataShow[DataFormatSection.Width]);
            }
            if (sUISettingArrary[1].Contains("True"))
            {
                if (!m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Height]))
                    m_dgvFingerStatus.Columns.Add(DicDataShow[DataFormatSection.Height]);
            }
            else
            {
                if (m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Height]))
                    m_dgvFingerStatus.Columns.Remove(DicDataShow[DataFormatSection.Height]);
            }
            if (sUISettingArrary[2].Contains("True"))
            {
                if (!m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Pressure]))
                    m_dgvFingerStatus.Columns.Add(DicDataShow[DataFormatSection.Pressure]);
            }
            else
            {
                if (m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Pressure]))
                    m_dgvFingerStatus.Columns.Remove(DicDataShow[DataFormatSection.Pressure]);
            }
            m_dgvFingerStatus.Columns[(int)enFigGridCol.Col_X].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            m_dgvFingerStatus.Columns[(int)enFigGridCol.Col_Y].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        public void Paint_Toolfm_Init(int iSelectedDev)
        {
            SelectedDevIndex = iSelectedDev;
            vfLoadSetting(iSelectedDev);//讀取INI設定
            GetDebugAddressRange();

            if (UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.ShowIlitekIcon)
            {
                m_panAAscen.BackgroundImage = global::ITS_Studio.Properties.Resources.ILIlogo;
                m_panAAscen.BackgroundImageLayout = ImageLayout.Center;
            }

            UpdateUIByFormat();

            if (!UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.Performance_En)
            {
                if (m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Jitter]))
                    m_dgvFingerStatus.Columns.Remove(DicDataShow[DataFormatSection.Jitter]);
                if (m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Gap_Ratio]))
                    m_dgvFingerStatus.Columns.Remove(DicDataShow[DataFormatSection.Gap_Ratio]);

                m_dgvFingerStatus.Columns[(int)enFigGridCol.Col_X].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                m_dgvFingerStatus.Columns[(int)enFigGridCol.Col_Y].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else
            {
                if (!m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Gap_Ratio]))
                    m_dgvFingerStatus.Columns.Add(DicDataShow[DataFormatSection.Gap_Ratio]);
                if (!m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Jitter]))
                    m_dgvFingerStatus.Columns.Add(DicDataShow[DataFormatSection.Jitter]);

                m_dgvFingerStatus.Columns[(int)enFigGridCol.Col_X].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
                m_dgvFingerStatus.Columns[(int)enFigGridCol.Col_Y].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            }

            // Init Variable            
            int iMaxFingers = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor].Count();
            UI_GBV.mDev[SelectedDev].LocalVariable.PaintTool_Var.coFingerColor = new Color[iMaxFingers];
            UI_GBV.mDev[SelectedDev].LocalVariable.PaintTool_Var.inFingerColorRBG = new Color[iMaxFingers];
            UI_GBV.mDev[SelectedDev].LocalVariable.PaintTool_Var._ColorPen = new IntPtr[iMaxFingers];

            Graphics Testgrp = this.m_panAAscen.CreateGraphics();
            GDI_Pen.BeginGDI(Testgrp);

            int iFingers = UI_GBV.mDev[iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iMaxTouchPoint;
            UI_GBV.mDev[iSelectedDev].LocalVariable.VarPaint_Multi.MonitorFingerBuff = new MultiPaintTool_VAR.FingerStatus[iFingers];
            UI_GBV.mDev[iSelectedDev].LocalVariable.VarPaint_Multi.MonitorFingerBuff = UI_GBV.mDev[iSelectedDev].LocalVariable.VarPaint_Multi.MonitorFingerBuff.Select(x => x = new MultiPaintTool_VAR.FingerStatus()).ToArray();

            UI_GBV.mDev[iSelectedDev].LocalVariable.VarPaint_Multi.MonitorPenBuff = new MultiPaintTool_VAR.PenStatus();
            if (!StaticVar.IsPlayback_Mode)
                UI_GBV.mDev[iSelectedDev].m_GBV.Globe_Multi._PaintTool.Interface_Init_API();

            UI_GBV.mDev[iSelectedDev].m_GBV.Globe_Multi._PaintTool.Parameter_Init_API(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize);//手指資料初始化

            InitFingerColor();
            Set_Figer_Default_Status(iSelectedDev);
            InitAlgoModePara();
            // Data Grid View
            if (UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.ListMode)
            {
                splitContainer_PaintMain.Panel1Collapsed = false;
                splitContainer_PaintMain.Panel1.Show();
                vfChangeResolution();
                m_dgvFingerStatus.Rows.Clear();
                //m_dgvFigerCol_Pre.Visible = StaticVar.Paint.stuPressure.Visible;

                for (int inCount = 0; inCount < iFingers; inCount++)
                {
                    m_dgvFingerStatus.Rows.Add(string.Format("F_{0}", inCount + 1), "0", "0", "0", "0", "0");
                    m_dgvFingerStatus.Rows[inCount].DefaultCellStyle.ForeColor = UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.coFingerColor[inCount];
                    m_dgvFingerStatus.Rows[inCount].DefaultCellStyle.SelectionForeColor = UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.coFingerColor[inCount];
                    //Kortek
                    m_dgvKortek.Rows.Add(string.Format("F_{0}", inCount + 1), "", "", "", "", "", "");
                    m_dgvKortek.Rows[inCount].DefaultCellStyle.ForeColor = UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.coFingerColor[inCount];
                    m_dgvKortek.Rows[inCount].DefaultCellStyle.SelectionForeColor = UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.coFingerColor[inCount];
                }

                if (UI_GBV.mDev[iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts > 0)
                {
                    tabPage_Key.Parent = uiTabControl1;
                    m_dgvKeyStatus.Rows.Clear();
                    for (int inCount = 0; inCount < UI_GBV.mDev[iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts; inCount++)
                        m_dgvKeyStatus.Rows.Add(string.Format("Key_{0}", inCount + 1), "", "0");
                }
                else
                {
                    tabPage_Key.Parent = null;
                }
            }
            else
            {
                if (StaticVar.IsPlayback_Mode)
                    vfChangeResolution();
                splitContainer_PaintMain.Panel1Collapsed = true;
                splitContainer_PaintMain.Panel1.Hide();

                InitLabelLog();

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintFullScreenClear, (IntPtr)0, (IntPtr)0);
            }

            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.iReportRateCount = 0;
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.iPenReportRate = 0;

            Paint_Record.Enabled = !StaticVar.IsPlayback_Mode;

            Timer_ReportRate.Enabled = !StaticVar.IsPlayback_Mode;
        }

        private void InitLabelLog()
        {
            // 設定Report Rate 位置
            Initial_LabelLogText(ref LabelLog, _LabelLog.Label_Type.ReportRate, new Point(10, 10));
            // 設定Algo Label 位置
            Initial_LabelLogText(ref LabelLog, _LabelLog.Label_Type.Algo, new Point(10, 10 + LABLE_PADDING));
            // 設定Finger Label 位置
            int iH = m_panAAscen.Height - 10 * LABLE_PADDING;
            Initial_LabelLogText(ref LabelLog, _LabelLog.Label_Type.Finger, new Point(10, iH));
            // 設定Pen Label 位置 - Finger的上面
            iH = iH - 20;
            Initial_LabelLogText(ref LabelLog, _LabelLog.Label_Type.Pen, new Point(10, iH));
        }

        public void InitDataColumn()
        {
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.none)
            {
                splitContainer_PaintMain.Panel1Collapsed = true;
                StaticVar.IsPlayback_Mode = true;
                //this.Refresh();
                return;
            }
            SetDebugDataGridView();
            DicPenDebugInfo.Clear();

            string strSector = string.Format("DataColumnName_{0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType);
            splitContainer_PaintMain.Panel1Collapsed = false;

            m_dgvAlgoData.Rows.Clear();
            m_dgvKortek.Columns.Clear();

            List<string> HeaderBits = MY_INI.IniReadAllKeys(strSector);
            foreach (var item in EnumTool.EnumToList<PaintTool.enAlgoNameRowIndex>())
            {
                string Header = MY_INI.IniReadValue(strSector, item.ToString(), "");
                switch (item)
                {
                    case PaintTool.enAlgoNameRowIndex._Report_Rate:
                        m_dgvAlgoData.Rows.Add("0");
                        break;

                    default:
                        m_dgvAlgoData.Rows.Add("Inactive");
                        break;
                }
                m_dgvAlgoData.Rows[(int)item].HeaderCell.Value = Header;
            }

            DataGridViewTextBoxColumn CloumnIndex;
            foreach (var item in EnumTool.EnumToList<PaintTool.enAlgoNameKortekIndex>())
            {
                CloumnIndex = new DataGridViewTextBoxColumn();
                CloumnIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                CloumnIndex.HeaderText = item.Desc();
                CloumnIndex.Name = "colText_" + item.Desc();
                CloumnIndex.ReadOnly = true;
                CloumnIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                CloumnIndex.MinimumWidth = 10;
                CloumnIndex.Width = 10;

                switch (item)
                {
                    default:
                        m_dgvKortek.Columns.Add(CloumnIndex);
                        break;
                }

            }
            //2022/4/19: 針對錯誤封包去計算出現的次數
            DataGridViewRow _Row = new DataGridViewRow();
            _Row.HeaderCell.Value = "";
            _Row.CreateCells(m_dgvAlgoData);
            _Row.Cells[0].Value = "0";
            m_dgvAlgoData.Rows.Add(_Row);

            splitContainer_TouchInfo.SplitterDistance = (m_dgvAlgoData.Rows.Count + 1) * 24;
        }

        internal void splitContainer_Collapsed(bool b)
        {
            splitContainer1.Panel2Collapsed = !b;
        }

        private void Set_Figer_Default_Status(int iSelectedDev)
        {
            int iFingers = UI_GBV.mDev[iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iMaxTouchPoint;
            UI_GBV.mDev[iSelectedDev].LocalVariable.VarPaint_Multi.MonitorFingerBuff = new MultiPaintTool_VAR.FingerStatus[iFingers];
            UI_GBV.mDev[iSelectedDev].LocalVariable.VarPaint_Multi.MonitorFingerBuff = UI_GBV.mDev[iSelectedDev].LocalVariable.VarPaint_Multi.MonitorFingerBuff.Select(x => x = new MultiPaintTool_VAR.FingerStatus()).ToArray();
        }

        public void vfLoadSetting(int iSelectedDev)
        {
            string strSection = "Setting";

            string str = MY_INI.IniReadValue(strSection, "Display_Mode", DataDisplayMode.ListMode.ToString());
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode = (DataDisplayMode)Enum.Parse(typeof(DataDisplayMode), str);

            str = MY_INI.IniReadValue(strSection, "ScreenList", ScreenList.Fill_Form.ToString());
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.ScreenShow = (ScreenList)Enum.Parse(typeof(ScreenList), str);

            //20221014 Steven Add
            str = MY_INI.IniReadValue(strSection, "BackgroundColor", enBackGroundColor.White.ToString());
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor = (enBackGroundColor)Enum.Parse(typeof(enBackGroundColor), str);

            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.ShowIlitekIcon = MY_INI.IniReadBool(strSection, "ShowBackGroundImage", false);

            str = MY_INI.IniReadValue(strSection, "PenTiltType", enPenTiltType.XY_Tilt.ToString());
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.PenTiltType = (enPenTiltType)Enum.Parse(typeof(enPenTiltType), str);

            str = MY_INI.IniReadValue(strSection, "Pattern", enBackGroundPattern.None.ToString());
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Pattern = (enBackGroundPattern)Enum.Parse(typeof(enBackGroundPattern), str);
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.Pattern.Clear();

            
            foreach (string item in str.Split(','))
            {
                if (item == enBackGroundPattern.None.ToString())
                    continue;
                UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.Pattern.Add((enBackGroundPattern)Enum.Parse(typeof(enBackGroundPattern), item));
            }

            /* Performance variable handling */
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.Performance_En = MY_INI.IniReadBool(strSection, "Performance_Enable", false);
            if (UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.Performance_En)
            {
                UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.lcm_physical_x = (uint)MY_INI.IniReadInt("Performance", "LCM_PHYSICAL_X", 0);
                UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.lcm_physical_y = (uint)MY_INI.IniReadInt("Performance", "LCM_PHYSICAL_Y", 0);
            }

            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.TouchUpCircle_En = MY_INI.IniReadBool(strSection, "TouchUp_Circle_Enable", true);
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.bSWAPXY = MY_INI.IniReadBool(strSection, "Swap_X_Y_Enable", false);
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.bIsShowTrajectoryDot = MY_INI.IniReadBool(strSection, "Show_Trajectory_Dot", false);
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.HoverCircle_En = MY_INI.IniReadBool(strSection, "Hover_Circle_Enable", false);

            bool bSupportPen = false;
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo != null)
                bSupportPen = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType != Pen_CDC_Format.enPenType.Disable;
            //UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.PenStatusBar_En = MY_INI.IniReadBool(strSection, "Pen_Status_Bar_Enable", true);
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.PenStatusBar_En = bSupportPen && MY_INI.IniReadBool(strSection, "Pen_Status_Bar_Enable", true);
            string strKeyWord = MY_INI.IniReadValue(strSection, "SpinDirection", enSpinDirection.Normal.ToString());
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.SpinDirection.Clear();
            foreach (string item in strKeyWord.Split(','))
            {
                if (item == enSpinDirection.Normal.ToString())
                    continue;
                UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.SpinDirection.Add((enSpinDirection)Enum.Parse(typeof(enSpinDirection), item));
            }

            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.DisplayCoordinate = MY_INI.IniReadBool(strSection, "Display_Coordinate", true);
            splitContainer_TouchInfo.Panel2Collapsed = !UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.DisplayCoordinate;

            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.Display_AlgoInfo = MY_INI.IniReadBool(strSection, "Display_AlgoInfo", true);
            splitContainer_TouchInfo.Panel1Collapsed = !UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.Display_AlgoInfo;


            touchUpCircle_ToolStripMenuItem.Checked = UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.TouchUpCircle_En;
            SwapXY_ToolStripMenuItem.Checked = UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.bSWAPXY;
            ShowTrajectoryDot.Checked = UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.bIsShowTrajectoryDot;
            Hover_ToolStripMenuItem1.Checked = UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.HoverCircle_En;

            penStatus_BarToolStripMenuItem.Checked = bSupportPen && UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.PenStatusBar_En;
            penStatus_BarToolStripMenuItem.Visible = bSupportPen;
            penTiltTypeToolStripMenuItem.Visible = bSupportPen;
            Pressure_ToolStripMenuItem1.Visible = bSupportPen;
            
            Performance_ToolStripMenuItem.Checked = UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.Performance_En;
            //statusStrip1.Visible = UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.PenStatusBar_En;
            statusStrip1.Visible = bSupportPen && UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.Settings.PenStatusBar_En;
            //20221020 Steven
            strSection = "Pressure";
            str = MY_INI.IniReadValue(strSection, "DouScale", enPenPressureEffect.None.ToString());
            UI_GBV.mDev[iSelectedDev].LocalVariable.PaintTool_Var.PenPressure = (enPenPressureEffect)Enum.Parse(typeof(enPenPressureEffect), str);

            UInt32 returnPurpleID = 0;

            returnPurpleID = MY_INI.IniReadUInt("Setting", "Pen_ID_Purple", 0);

            Pen_ID_Purple = (UInt32)returnPurpleID;
        }

        public void MenuClose()
        {
            Main_MenuStrip.Close();
        }

        private void DrawPattern(List<enBackGroundPattern> lsPattern)
        {
            Color nowBackColor = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor);
            Color PenColor = Color.FromArgb(80, (255 - nowBackColor.R), (255 - nowBackColor.G), (255 - nowBackColor.B));
            var p = new Pen(PenColor, 2);
            p.DashStyle = DashStyle.Custom;
            p.DashPattern = new float[] { 1f, 1f };

            foreach (var item in lsPattern)
            {
                switch (item)
                {
                    case enBackGroundPattern.None:
                        break;
                    case enBackGroundPattern.Straight_Line:
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            Graphics grp = m_panAAscen.CreateGraphics();
                            var point1 = new PointF(0, 0); // 0,0
                            var point2 = new PointF(0, m_panAAscen.Height); //0,720
                            for (int i = 0; i < 9; i++) //Steven : 目前切成9等分，可以開放設定
                            {
                                g.DrawLine(p, point1, point2);
                                grp.DrawLine(p, point1, point2);
                                point1.X = point1.X + (((float)m_panAAscen.Width) / ((float)9 - 1f));
                                point2.X = point2.X + (((float)m_panAAscen.Width) / ((float)9 - 1f));
                            }

                            point1 = new PointF(0, 0); // 0,0
                            point2 = new PointF(m_panAAscen.Width, 0); //1280,0
                            for (int i = 0; i < 9; i++)
                            {
                                g.DrawLine(p, point1, point2);
                                grp.DrawLine(p, point1, point2);
                                point1.Y = point1.Y + (((float)m_panAAscen.Height) / ((float)9 - 1f));
                                point2.Y = point2.Y + (((float)m_panAAscen.Height) / ((float)9 - 1f));
                            }
                        }
                        break;
                    case enBackGroundPattern.Diagonal_Line:
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            Graphics grp = m_panAAscen.CreateGraphics();
                            var point1 = new PointF(0, 0); // 0,0
                            var point2 = new PointF(m_panAAscen.Width, m_panAAscen.Height); //0,720
                            g.DrawLine(p, point1, point2);
                            grp.DrawLine(p, point1, point2);

                            point1 = new PointF(m_panAAscen.Width, 0); // 0,0
                            point2 = new PointF(0, m_panAAscen.Height); //1280,0

                            g.DrawLine(p, point1, point2);
                            grp.DrawLine(p, point1, point2);
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        private void DrawPanelLine()
        {
            Color nowBackColor = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor);
            Color PenColor = Color.FromArgb(255, (255 - nowBackColor.R), (255 - nowBackColor.G), (255 - nowBackColor.B));
            var p = new Pen(PenColor, 2);
            Point[] PointArr = new Point[4] { new Point(0, 0), new Point(0, m_panAAscen.Height), 
                new Point(m_panAAscen.Width, m_panAAscen.Height), new Point(m_panAAscen.Width, 0)};
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Graphics grp = m_panAAscen.CreateGraphics();
                g.DrawLine(p, PointArr[0], PointArr[1]);
                grp.DrawLine(p, PointArr[0], PointArr[1]);

                g.DrawLine(p, PointArr[2], PointArr[3]);
                grp.DrawLine(p, PointArr[2], PointArr[3]);

                g.DrawLine(p, PointArr[0], PointArr[3]);
                grp.DrawLine(p, PointArr[0], PointArr[3]);

                g.DrawLine(p, PointArr[1], PointArr[2]);
                grp.DrawLine(p, PointArr[1], PointArr[2]);
            }
        }

        //private CancellationTokenSource _CancellationTokenSource;
        #region Paint_Replay
        private BackgroundWorker Work_Paint_Replay;

        private ManualResetEvent Pause_Paint_Replay;

        public bool Paint_Replay_Pause
        {
            set
            {
                if (value == true)
                    Pause_Paint_Replay.Reset();
                else
                    Pause_Paint_Replay.Set();
            }
        }

        public void Painting_Replay_Start(bool bUseNewFormat)
        {
            clearToolStripMenuItem_Click(this, null);
        
            Pause_Paint_Replay = new ManualResetEvent(true);            
            GC.Collect();
            Work_Paint_Replay = new BackgroundWorker();
            if(bUseNewFormat)            
                Work_Paint_Replay.DoWork += new DoWorkEventHandler(Painting_Replay_paint);                
            else            
                Work_Paint_Replay.DoWork += new DoWorkEventHandler(Painting_Replay_csv);
             
            Work_Paint_Replay.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Paint_Replay_csv_Completed);
            Work_Paint_Replay.WorkerSupportsCancellation = true;

            Work_Paint_Replay.RunWorkerAsync();
        }
        private ParseLog _ParseLog = new ParseLog();
        private int iProgess = 0;
        private void Painting_Replay_paint(object sender, DoWorkEventArgs e)
        {
            var FrameCnts = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.FileEmployee.iTotal_Frame_Count;
            var FileName = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.FileEmployee.FileName;
            long iLastPosition = 0;
            for (int idx = 1; idx <= FrameCnts; idx++)
            {
               
                if (Work_Paint_Replay == null)
                    break;
                if (Work_Paint_Replay.CancellationPending)
                    break;
                if (idx != (iProgess + 1))
                    idx = (iProgess + 2);
                List<string> RecordRawData = _ParseLog.OneFrameBuff;
                string TitlePattern = string.Format("#{0}@", idx);
                _ParseLog.SeekReadFileRange_EndTag_1(FileName, iLastPosition, TitlePattern, string.Format("#{0}@", idx + 1), out iLastPosition, ref RecordRawData);
                //Console.WriteLine("iLastPosition = {0}", iLastPosition);
                string pattern = @"\b(?:[0-9a-fA-F]{2}\b(?:,|\n$))+";
                int iDelay = 0;
                foreach (var line in RecordRawData)
                {
                    if (Regex.IsMatch(line, TitlePattern))
                        iDelay = int.Parse(Regex.Match(line, @"@(\d+)").Groups[1].Value);
                    else if (!Regex.IsMatch(line, pattern))
                        continue;
                    if (line.Contains("Gap") || line.Contains("Jitter"))
                        continue;

                    foreach (Match match in Regex.Matches(line, pattern))
                    {
                        var arr = match.Value.Split(',').Where(hex => !string.IsNullOrEmpty(hex)).Select(hex => Convert.ToByte(hex, 16)).ToArray();
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.RePainting_API(arr);
                    }
                }
                //UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.RePainting_API();
                this.Invoke(new MethodInvoker(delegate()
                {
                    userControl_ReplayPaint.TrackBar_Value = idx;
                    this.userControl_ReplayPaint.FrameNum_Text = userControl_ReplayPaint.TrackBar_Value.ToString();
                }));
                var _Num = (iDelay * userControl_ReplayPaint.multiple);
                MyTool.Delay_ms((long)_Num);
                Pause_Paint_Replay.WaitOne(Timeout.Infinite);
                //i = iProgess;
                iProgess++;
            }
        }
        private void Painting_Replay_csv(object sender, DoWorkEventArgs e)
        {
            for(int idx = 0; idx < UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame.Count; idx++)
            {
                if(Work_Paint_Replay == null)
                    break;
                if(Work_Paint_Replay.CancellationPending)
                    break;

                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.RePainting_API(UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame[idx]);

                this.Invoke(new MethodInvoker(delegate()
                {
                    this.userControl_ReplayPaint.TrackBar_Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame[idx].FrameNo;
                    this.userControl_ReplayPaint.FrameNum_Text = this.userControl_ReplayPaint.TrackBar_Value.ToString();
                }));

                var _Num = (UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame[idx].FrameTime * userControl_ReplayPaint.multiple);

                MyTool.Delay_ms((long)_Num);

                Pause_Paint_Replay.WaitOne(Timeout.Infinite);

                //20220613 Steven Add : 紀錄iSelectedIndex的值並修改進度； iSelectedIndex和i一起增加，如果修改iSelectedIndex，i也要修改                
                idx = iReplay_SelectedIndex;                
                iReplay_SelectedIndex++;
            }
        }

        private void Paint_Replay_csv_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            userControl_ReplayPaint.Stop_API();
            userControl_ReplayPaint.ChangeStatus(ButtonStatus.Stop);

            GC.Collect();
        }

        private void Paint_Replay_Frame(Message msg)
        {            
            iProgess = (int)msg.WParam;            
            AssignSelectFrameToBuffer((int)msg.WParam, userControl_ReplayPaint.b_Paint_ReplayUseNewFormat);
        }

        public void OnShowMonitorLog()
        {
            Action<Dictionary<string, List<int>>> UpdateGridViewAction = UpdateGridView;
            Action<Dictionary<string, List<int>>> UpdatePenGridViewAction = UpdatePenGridView;
            //Pen or Finger 擇一進入
            if (!UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.isPenUsing)
                UpdateGridViewAction.Invoke(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugMappingBuff);
            else
                UpdatePenGridViewAction.Invoke(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugMappingBuff);

            foreach (var Item in UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugMappingBuff.ToList())            
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugMappingBuff[Item.Key].Clear();
            
        }

        private void UpdateGridView(Dictionary<string, List<int>> lsArray)
        {
            int iNowRow = 0;
            var Employee = 0;
            foreach (var item in lsArray)
            {
                if (item.Key == "NA")
                    continue;
                if (dataGridView_FingerDebugInfo.Rows.Count == 0)
                    break;
                if (item.Key == "SNR")
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts++;
                    var SNR_Value = lsArray["SNR"];
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_lsSNR_Value.Add(SNR_Value[1] << 8 | SNR_Value[0]);
                    //ProgressBar_SNR.Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts;
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iSNR_FrameCount)
                    {
                        //ProgressBar_SNR.Value = ProgressBar_SNR.Maximum;
                        var tmp = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.CalculateSNR(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_lsSNR_Value);

                        if (isNeedUpdateDBGUI)
                            dataGridView_FingerDebugInfo.Rows[iNowRow].Cells["Value"].Value = string.Format("{0}", tmp);
                        tempFingerSNR = tmp;

                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_lsSNR_Value.Clear();
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts = 0;
                    }
                    if (StaticVar.IsPlayback_Mode && isNeedUpdateDBGUI)
                        dataGridView_FingerDebugInfo.Rows[iNowRow].Cells["Value"].Value = string.Format("{0}", tempFingerSNR);
                    //lbSNR_Frame.Text = string.Format("Frame : {0} / {1}"
                    //    , UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts
                    //    , UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iSNR_FrameCount);
                }
                else
                {
                    if (item.Value.Count == 1)
                        Employee = (item.Value[0]);
                    else
                        Employee = (item.Value[1] << 8 | item.Value[0]);

                    if (isNeedUpdateDBGUI)
                        dataGridView_FingerDebugInfo.Rows[iNowRow].Cells["Value"].Value = string.Format("{0}", Employee);
                }
                iNowRow++;
            }
        }

        private void UpdatePenGridView(Dictionary<string, List<int>> lsArray)
        {
            int iNowRow = 0; //Pen的Start Location

            foreach (var item in lsArray)
            {
                var Employee = 0;
                if (item.Key == "NA")
                    continue;
                if (dataGridView_PenDebugInfo.Rows.Count == 0)
                    break;

                if (item.Key.Contains("Pen_SNR_X"))
                {
                    enPenSNRX PenSNRXIndex = (enPenSNRX)Enum.Parse(typeof(enPenSNRX), item.Key);
                    var Pen_SNR_Value_X = lsArray[item.Key];
                    if (Pen_SNR_Value_X.Count == 0 || (int)PenSNRXIndex >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iPen_TermCount)
                    {
                        if (StaticVar.IsPlayback_Mode && isNeedUpdateDBGUI)
                            if (TempPenSNR_X.ContainsKey(PenSNRXIndex))
                                dataGridView_PenDebugInfo.Rows[iNowRow].Cells["Value"].Value = string.Format("{0}", TempPenSNR_X[PenSNRXIndex]);

                        iNowRow++;
                        continue;
                    }

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[(int)PenSNRXIndex]++;

                    if (!DicSNRXCalculate.ContainsKey(PenSNRXIndex))
                        DicSNRXCalculate.Add(PenSNRXIndex, new List<float>());

                    DicSNRXCalculate[PenSNRXIndex].Add(Pen_SNR_Value_X[1] << 8 | Pen_SNR_Value_X[0]);

                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[(int)PenSNRXIndex] >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iSNR_FrameCount)
                    {
                        var tmp = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.CalculateSNR(DicSNRXCalculate[PenSNRXIndex]);
                        this.Invoke(new Action(() =>
                        {
                            if (isNeedUpdateDBGUI)
                                dataGridView_PenDebugInfo.Rows[iNowRow].Cells["Value"].Value = string.Format("{0}", tmp);
                        }));
                        if (!TempPenSNR_X.ContainsKey(PenSNRXIndex))
                            TempPenSNR_X.Add(PenSNRXIndex, 0.0f);
                        TempPenSNR_X[PenSNRXIndex] = tmp;

                        DicSNRXCalculate[PenSNRXIndex].Clear();
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[(int)PenSNRXIndex] = 0;
                    }
                    if (StaticVar.IsPlayback_Mode && isNeedUpdateDBGUI)
                        if (TempPenSNR_X.ContainsKey(PenSNRXIndex))
                            dataGridView_PenDebugInfo.Rows[iNowRow].Cells["Value"].Value = string.Format("{0}", TempPenSNR_X[PenSNRXIndex]);

                }
                else if (item.Key.Contains("Pen_SNR_Y"))
                {
                    enPenSNRY PenSNRYIndex = (enPenSNRY)Enum.Parse(typeof(enPenSNRY), item.Key);

                    var Pen_SNR_Value_Y = lsArray[item.Key];
                    if (Pen_SNR_Value_Y.Count == 0 || (int)PenSNRYIndex >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iPen_TermCount)
                    {
                        if (StaticVar.IsPlayback_Mode && isNeedUpdateDBGUI)
                            if (TempPenSNR_Y.ContainsKey(PenSNRYIndex))
                                dataGridView_PenDebugInfo.Rows[iNowRow].Cells["Value"].Value = string.Format("{0}", TempPenSNR_Y[PenSNRYIndex]);
                        iNowRow++;
                        continue;
                    }


                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[(int)PenSNRYIndex]++;

                    if (!DicSNRYCalculate.ContainsKey(PenSNRYIndex))
                        DicSNRYCalculate.Add(PenSNRYIndex, new List<float>());

                    DicSNRYCalculate[PenSNRYIndex].Add(Pen_SNR_Value_Y[1] << 8 | Pen_SNR_Value_Y[0]);

                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[(int)PenSNRYIndex] >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iSNR_FrameCount)
                    {

                        var tmp = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.CalculateSNR(DicSNRYCalculate[PenSNRYIndex]);
                        this.Invoke(new Action(() =>
                        {
                            if (isNeedUpdateDBGUI)
                                dataGridView_PenDebugInfo.Rows[iNowRow].Cells["Value"].Value = string.Format("{0}", tmp);
                        }));
                        if (!TempPenSNR_Y.ContainsKey(PenSNRYIndex))
                            TempPenSNR_Y.Add(PenSNRYIndex, 0.0f);
                        TempPenSNR_Y[PenSNRYIndex] = tmp;

                        DicSNRYCalculate[PenSNRYIndex].Clear();
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[(int)PenSNRYIndex] = 0;
                    }
                    if (StaticVar.IsPlayback_Mode && isNeedUpdateDBGUI)
                        if (TempPenSNR_Y.ContainsKey(PenSNRYIndex))
                            dataGridView_PenDebugInfo.Rows[iNowRow].Cells["Value"].Value = string.Format("{0}", TempPenSNR_Y[PenSNRYIndex]);
                }
                else
                {
                    bool isNeedUpdateUi = false;
                    //暫時未定義 Add by Steven 20220511
                    if (!DicPenDebugInfo.ContainsKey(item.Key))
                        DicPenDebugInfo.Add(item.Key, 0);

                    if (item.Value.Count == 1)
                        Employee = (item.Value[0]);
                    else
                        Employee = (item.Value[1] << 8 | item.Value[0]);

                    if (Employee != DicPenDebugInfo[item.Key] || StaticVar.IsPlayback_Mode)
                    {
                        DicPenDebugInfo[item.Key] = Employee;
                        isNeedUpdateUi = true;
                    }
                    if (item.Key != "Algo_Pen_Stage" && Employee != 0 && isNeedUpdateUi)
                    {
                        this.Invoke(new Action(() =>
                        {
                            if (isNeedUpdateDBGUI)
                                dataGridView_PenDebugInfo.Rows[iNowRow].Cells["Value"].Value = string.Format("{0}", Employee);
                        }));
                    }
                }
                iNowRow++;
            }
        }
        private UIPage MyUIPage = new UIPage();
        private bool AssignSelectFrameToBuffer(int SelectIndex, bool bUseNewFormat)
        {
            isFinishReplayByScrollBar = false;
            panel_Hover.Visible = false;
          
            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool == null)
                return false;
            string _str = "";
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_En;
            else
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_Cn;
            MyUIPage.ShowWaitForm(_str);
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.Parameter_Init_API(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize);//手指資料初始化
         
            this.userControl_ReplayPaint.FrameNum_Text = SelectIndex.ToString();
            this.userControl_ReplayPaint.Refresh();
            iReplay_SelectedIndex = SelectIndex;
            bitmap = new Bitmap(m_panAAscen.Width, m_panAAscen.Height);

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_lsSNR_Value.Clear();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts = 0;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.DisplayCoordinate = false;

            for (int i = 0; i < UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts.Length; i++)
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[i] = 0;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[i] = 0;
            }
            DicSNRXCalculate.Clear();
            DicSNRYCalculate.Clear();

            if (SelectIndex != 0)
                isNeedUpdateDBGUI = false;
            //Finger DBG
            for (int i = 0; i < dataGridView_FingerDebugInfo.Rows.Count; i++)            
                dataGridView_FingerDebugInfo.Rows[i].Cells["Value"].Value = "";
            
            //PEN DBG
            for (int i = 0; i < dataGridView_PenDebugInfo.Rows.Count; i++)            
                dataGridView_PenDebugInfo.Rows[i].Cells["Value"].Value = "";
            
            tempFingerSNR = 0;
            TempPenSNR_X.Clear();
            TempPenSNR_Y.Clear();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.DisplayCoordinate = true;
            bool bUpdateInfo = false;
            if(bUseNewFormat)
            {                
                var FileName = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.FileEmployee.FileName;
                long iLastPosition = 0;
                for(int idx = 1; idx <= iReplay_SelectedIndex; idx++)
                {
                    if (idx == iReplay_SelectedIndex)
                        MyTool.Delay_ms(50);
                    isNeedUpdateDBGUI = true;
                    bUpdateInfo = idx == iReplay_SelectedIndex ? true : false;
                    List<string> RecordRawData = _ParseLog.OneFrameBuff;
                    string TitlePattern = string.Format("#{0}@", idx);
                    _ParseLog.SeekReadFileRange_EndTag_1(FileName, iLastPosition, TitlePattern, string.Format("#{0}@", idx + 1), out iLastPosition, ref RecordRawData);

                    string pattern = @"\b(?:[0-9a-fA-F]{2}\b(?:,|\n$))+";
                    foreach(var line in RecordRawData)
                    {
                        if(!Regex.IsMatch(line, pattern))
                            continue;

                        if (line.Contains("Gap") || line.Contains("Jitter"))
                            continue;

                        foreach(Match match in Regex.Matches(line, pattern))
                        {
                            var arr = match.Value.Split(',').Where(hex => !string.IsNullOrEmpty(hex)).Select(hex => Convert.ToByte(hex, 16)).ToArray();
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.RePainting_API(arr, enPaintingCount.Painting1, bUpdateInfo);
                        }
                    }
                }
            }
            else
            {
                for(int idx = 0; idx < iReplay_SelectedIndex; idx++)
                {
                    if(idx == iReplay_SelectedIndex - 1)                    
                        isNeedUpdateDBGUI = true;
                    
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.RePainting_API(UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame[idx]);
                }
            }
            m_panAAscen.Invalidate();
            DrawPattern(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Pattern);
            DrawPanelLine();
            isFinishReplayByScrollBar = true;
            MyUIPage.HideWaitForm();
            return true;
        }
        #endregion Paint_Replay

        public void OnLine_StartPaint()
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bRunningStatus = true;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.StartPainting_API();
        }

        public void StopPaint()
        {
            if (StaticVar.IsPlayback_Mode)
            {
                iReplay_SelectedIndex = 0;
                iProgess = 0;
                if (Work_Paint_Replay != null)
                    if (Work_Paint_Replay.IsBusy)
                    {
                        Work_Paint_Replay.CancelAsync();
                        if (Pause_Paint_Replay != null)
                            Paint_Replay_Pause = false;
                        return;
                    }

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_lsSNR_Value.Clear();
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts = 0;

                for (int i = 0; i < UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts.Length; i++)
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[i] = 0;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[i] = 0;
                }
                DicSNRXCalculate.Clear();
                DicSNRYCalculate.Clear();
                //Finger DBG
                for (int i = 0; i < dataGridView_FingerDebugInfo.Rows.Count; i++)
                {
                    dataGridView_FingerDebugInfo.Rows[i].Cells["Value"].Value = string.Format("{0}", "");
                }
                //PEN DBG
                for (int i = 0; i < dataGridView_PenDebugInfo.Rows.Count; i++)
                {
                    dataGridView_PenDebugInfo.Rows[i].Cells["Value"].Value = string.Format("{0}", "");
                }

                tempFingerSNR = 0;
                TempPenSNR_X.Clear();
                TempPenSNR_Y.Clear();


                this.userControl_ReplayPaint.FrameNum_Text = this.userControl_ReplayPaint.TrackBar_Min.ToString();
            }

            if (StaticVar.bPaint_Record)
                RecordPainting();
            if (!StaticVar.IsPlayback_Mode)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.StopPainting_API();

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bThreadDecodeData_En = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bRunningStatus = false;
            StatusLabel_ReportRate.Text = string.Format("Pen Report-Rate : {0:000}", 0);
            Timer_ReportRate.Enabled = false;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!StaticVar.bPaint_Record && !StaticVar.IsPlayback_Mode)
            {
                vfChangeResolution();
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.FullScreen)
                {
                    //InitVirtualButton();
                    if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts > 0)
                        && UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.isI2C(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType))
                        PaintKeyRect();
                }
            }
        }

        private void CreatToolStripComboBoxList()
        {
            ToolStripComboBox TmpComboBox;
            //=====================================
            TmpComboBox = new ToolStripComboBox();
            TmpComboBox.Name = "ScreenListComboBox";
            TmpComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var item in EnumTool.EnumToList<ScreenList>())
                TmpComboBox.Items.Add(item.ToString());
            TmpComboBox.SelectedItem = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.ScreenShow.ToString();
            TmpComboBox.SelectedIndexChanged += ScreenShow_SelectedIndexChanged;
            Resolution_ToolStripMenuItem.DropDownItems.Add(TmpComboBox);
            //=====================================
            //ToolStripComboBox toolStripComboBox_Mode = new ToolStripComboBox();
            TmpComboBox = new ToolStripComboBox();
            TmpComboBox.Name = "DataDisplayModeComboBox";
            TmpComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var item in Enum.GetNames(typeof(DataDisplayMode)))
                TmpComboBox.Items.Add(item);
            TmpComboBox.SelectedItem = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode.ToString();
            TmpComboBox.SelectedIndexChanged += DataDisplayMode_SelectedIndexChanged;
            Mode_ToolStripMenuItem.DropDownItems.Add(TmpComboBox);
            //=====================================
            TmpComboBox = new ToolStripComboBox();
            TmpComboBox.Name = "ColorDisplayModeComboBox";
            TmpComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var item in Enum.GetNames(typeof(enBackGroundColor)))
                TmpComboBox.Items.Add(item);
            TmpComboBox.SelectedItem = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor.ToString();
            TmpComboBox.SelectedIndexChanged += ColorDisplayMode_SelectedIndexChanged;
            BackgroundColor_ToolStripMenuItem.DropDownItems.Add(TmpComboBox);
            //=====================================
            TmpComboBox = new ToolStripComboBox();
            TmpComboBox.Name = "PressureEffect";
            TmpComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var item in Enum.GetNames(typeof(enPenPressureEffect)))
                TmpComboBox.Items.Add(item);
            TmpComboBox.SelectedItem = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PenPressure.ToString();
            TmpComboBox.SelectedIndexChanged += PenPressureEffect_SelectedIndexChanged;
            Pressure_ToolStripMenuItem1.DropDownItems.Add(TmpComboBox);
            ////=====================================
            TmpComboBox = new ToolStripComboBox();
            TmpComboBox.Name = "PenTiltType";
            TmpComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var item in EnumTool.EnumToList<enPenTiltType>())
                TmpComboBox.Items.Add(item.Desc());
            TmpComboBox.SelectedItem = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PenTiltType.Desc();
            TmpComboBox.SelectedIndexChanged += PenTiltType_SelectedIndexChanged;
            penTiltTypeToolStripMenuItem.DropDownItems.Add(TmpComboBox);

            ////SpinDirection
            //TmpComboBox = new ToolStripComboBox();
            //TmpComboBox.Name = "SpinDirectionComboBox";
            //TmpComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            //foreach (var item in Enum.GetNames(typeof(StaticVar.Paint.SpinDirection)))
            //    TmpComboBox.Items.Add(item);
            //TmpComboBox.SelectedItem = StaticVar.Paint._SpinDirection.ToString();
            //TmpComboBox.SelectedIndexChanged += SpinDirection_SelectedIndexChanged;
            //Spin_ToolStripMenuItem.DropDownItems.Add(TmpComboBox);

            //this.Spin_ToolStripMenuItem.Name = "SpinDirectionComboBox";
            foreach (var item in Enum.GetNames(typeof(enSpinDirection)))
            {
                if (item == enSpinDirection.Normal.ToString())
                    continue;
                ToolStripMenuItem TmpMenuItem = new ToolStripMenuItem();
                TmpMenuItem.Name = item.ToString();
                TmpMenuItem.Text = item.ToString();
                TmpMenuItem.Click += MenuItem_Click;
                Spin_ToolStripMenuItem.DropDownItems.Add(TmpMenuItem);
            }

            foreach (ToolStripMenuItem item in Spin_ToolStripMenuItem.DropDownItems)
            {
                enSpinDirection strKey = (enSpinDirection)Enum.Parse(typeof(enSpinDirection), item.Name);

                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.SpinDirection.Contains(strKey))
                    item.Checked = true;
            }

            foreach (var item in Enum.GetNames(typeof(enBackGroundPattern)))
            {
                if (item == enBackGroundPattern.None.ToString())
                    continue;
                ToolStripMenuItem TmpMenuItemPattern = new ToolStripMenuItem();
                TmpMenuItemPattern.Name = item.ToString();
                TmpMenuItemPattern.Text = item.ToString();
                TmpMenuItemPattern.Click += MenuItem_Click;
                PatternSelect_ToolStripMenuItem.DropDownItems.Add(TmpMenuItemPattern);
            }

            foreach (ToolStripMenuItem item in PatternSelect_ToolStripMenuItem.DropDownItems)
            {
                enBackGroundPattern strKey = (enBackGroundPattern)Enum.Parse(typeof(enBackGroundPattern), item.Name);
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Pattern.Contains(strKey))
                    item.Checked = true;
            }
        }

        private void m_dgvKeyStatus_SelectionChanged(object sender, EventArgs e)
        {
            if (m_dgvKeyStatus.SelectedCells.Count > 0)
                m_dgvKeyStatus.ClearSelection();
        }

        private void m_tiFingerGri_Tick(object sender, EventArgs e)
        {
            Timer_FingerGrid.Enabled = false;
            boFingerGriFlag = true;
        }

        private void m_timer_CloseMonitor_Tick(object sender, EventArgs e)
        {
            m_timer_CloseMonitor.Enabled = false;
        }

        private void m_tiReportRate_Tick(object sender, EventArgs e)
        {
            int inReportRateCount = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.iReportRateCount;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.iReportRateCount = 0;

            int iPenReportRateCount = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.iPenReportRate;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.iPenReportRate = 0;

            Resolution_ToolStripMenuItem.Enabled = false;
            Mode_ToolStripMenuItem.Enabled = false;
            switch (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode)
            {
                case DataDisplayMode.FullScreen:
                    //labReprotRate.Text = string.Format("Report Rate : {0:D} Hz", inReportRateCount);
                    SetReportRate(inReportRateCount);

                    if (inReportRateCount == 0)
                    {
                        SetAlgoInfo(0xFF, false);
                        Resolution_ToolStripMenuItem.Enabled = true;
                        Mode_ToolStripMenuItem.Enabled = true;
                    }
                    StatusLabel_ReportRate.Text = string.Format("Pen Report-Rate : {0:000}", iPenReportRateCount);
                    break;

                case DataDisplayMode.ListMode:
                    if (inReportRateCount == 0)
                    {
                        foreach (var item in EnumTool.EnumToList<PaintTool.enAlgoNameRowIndex>())
                        {
                            switch (item)
                            {
                                case PaintTool.enAlgoNameRowIndex._Report_Rate:
                                    if (m_dgvAlgoData.Rows.Count == 0)
                                        return;
                                    m_dgvAlgoData.Rows[(int)item].Cells[0].Value = string.Format("{0}", inReportRateCount);
                                    break;

                                default:
                                    m_dgvAlgoData.Rows[(int)item].Cells[0].Value = "Inactive";
                                    break;
                            }
                        }
                        Resolution_ToolStripMenuItem.Enabled = true;
                        Mode_ToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        int iReport_Rate = (int)PaintTool.enAlgoNameRowIndex._Report_Rate;
                        m_dgvAlgoData.Rows[iReport_Rate].Cells[0].Value = string.Format("{0}", inReportRateCount);
                    }

                    StatusLabel_ReportRate.Text = string.Format("Pen Report-Rate : {0:000}", iPenReportRateCount);
                    break;
            }
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (Enum.IsDefined(typeof(enSpinDirection), item.Name))
            {
                enSpinDirection strKey = (enSpinDirection)Enum.Parse(typeof(enSpinDirection), item.Name);
                item.Checked = (item.Checked) ? false : true;
                if (item.Checked)
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.SpinDirection.Add(strKey);
                else
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.SpinDirection.Remove(strKey);

                MY_INI.IniWriteValue("Setting", "SpinDirection", string.Join(",", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.SpinDirection.Select(s => s.ToString()).ToArray()));
            }
            else
            {
                enBackGroundPattern strKey = (enBackGroundPattern)Enum.Parse(typeof(enBackGroundPattern), item.Name);
                item.Checked = (item.Checked) ? false : true;
                if (item.Checked)
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Pattern.Add(strKey);
                else
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Pattern.Remove(strKey);

                MY_INI.IniWriteValue("Setting", "Pattern", string.Join(",", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Pattern.Select(s => s.ToString()).ToArray()));
                vfChangeResolution();
                if (StaticVar.IsPlayback_Mode)
                {
                    AssignSelectFrameToBuffer(iReplay_SelectedIndex, userControl_ReplayPaint.b_Paint_ReplayUseNewFormat);
                }
            }
        }

        private void MenuItemTouchUp_Circle_Enable_Click(object sender, EventArgs e)
        {
            var _Sender = sender as ToolStripMenuItem;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.TouchUpCircle_En = _Sender.Checked;
            touchUpCircle_ToolStripMenuItem.Checked = _Sender.Checked;
            MY_INI.IniWriteValue("Setting", "TouchUp_Circle_Enable", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.TouchUpCircle_En.ToString());
        }

        private void Hover_ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var _Sender = sender as ToolStripMenuItem;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.HoverCircle_En = _Sender.Checked;
            Hover_ToolStripMenuItem1.Checked = _Sender.Checked;
            MY_INI.IniWriteValue("Setting", "Hover_Circle_Enable", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.HoverCircle_En.ToString());
        }  

        private void SwapXY_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _Sender = sender as ToolStripMenuItem;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bSWAPXY = _Sender.Checked;
            SwapXY_ToolStripMenuItem.Checked = _Sender.Checked;
            MY_INI.IniWriteValue("Setting", "Swap_X_Y_Enable", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bSWAPXY.ToString());
            vfChangeResolution();
        }

        private void ShowTrajectoryDot_Click(object sender, EventArgs e)
        {
            var _Sender = sender as ToolStripMenuItem;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bIsShowTrajectoryDot = _Sender.Checked;
            ShowTrajectoryDot.Checked = _Sender.Checked;
            MY_INI.IniWriteValue("Setting", "Show_Trajectory_Dot", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bIsShowTrajectoryDot.ToString());
        }

        private void MenuItemTouchUp_PenStatusBar_Enable_Click(object sender, EventArgs e)
        {
            var _Sender = sender as ToolStripMenuItem;
            statusStrip1.Visible = _Sender.Checked;
            vfChangeResolution();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.PenStatusBar_En = _Sender.Checked;
            penStatus_BarToolStripMenuItem.Checked = _Sender.Checked;
            MY_INI.IniWriteValue("Setting", "Pen_Status_Bar_Enable", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.PenStatusBar_En.ToString());
        }

        private void Performance_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _Sender = sender as ToolStripMenuItem;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Performance_En = _Sender.Checked;
            Performance_ToolStripMenuItem.Checked = _Sender.Checked;
            MY_INI.IniWriteValue("Setting", "Performance_Enable", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Performance_En.ToString());

            vfLoadSetting(SelectedDevIndex);//讀取INI設定

            UpdateUIByFormat();

            if (!UI_GBV.mDev[SelectedDevIndex].LocalVariable.PaintTool_Var.Settings.Performance_En)
            {
                if (m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Jitter]))
                    m_dgvFingerStatus.Columns.Remove(DicDataShow[DataFormatSection.Jitter]);
                if (m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Gap_Ratio]))
                    m_dgvFingerStatus.Columns.Remove(DicDataShow[DataFormatSection.Gap_Ratio]);

                m_dgvFingerStatus.Columns[(int)enFigGridCol.Col_X].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                m_dgvFingerStatus.Columns[(int)enFigGridCol.Col_Y].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else
            {
                if (!m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Gap_Ratio]))
                    m_dgvFingerStatus.Columns.Add(DicDataShow[DataFormatSection.Gap_Ratio]);
                if (!m_dgvFingerStatus.Columns.Contains(DicDataShow[DataFormatSection.Jitter]))
                    m_dgvFingerStatus.Columns.Add(DicDataShow[DataFormatSection.Jitter]);

                m_dgvFingerStatus.Columns[(int)enFigGridCol.Col_X].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
                m_dgvFingerStatus.Columns[(int)enFigGridCol.Col_Y].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            }
        }

        public bool GetDebugAddressRange()
        {
            INI_Tool MyINI = new INI_Tool(ITS_Directory.PaintTool_INI);
            string strSection = "V6ReportIendex";
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iContactCount = MY_INI.IniReadInt(strSection, "ContactCount", 0);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iAlgoMode = MY_INI.IniReadInt(strSection, "AlgoMode", 0);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iSNR_FrameCount = MY_INI.IniReadInt(strSection, "SNR_FrameCount", 100);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iPen_TermCount = MY_INI.IniReadInt(strSection, "Pen_TermCount", 6);

            CommonFlow.setting[m_iSelectedDev].contact_byte = (byte)UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iContactCount;
            CommonFlow.setting[m_iSelectedDev].algo_byte = (byte)UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iAlgoMode;

            CommonFlow.ilitek_dev_setting(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.setting[m_iSelectedDev]);

            #region Finger
            strSection = "DebugInfoName";
            List<string> DebugInfoIndex = MyINI.IniReadAllValues(strSection);
            List<string> DebugInfoSector = MyINI.IniReadAllKeys(strSection);

            if (DebugInfoIndex.Count == 0)
                return false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugMappingBuff.Clear();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugSectionMapping.Clear();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugSectionMapping = new Dictionary<string, string>();
            int iDebugInfo_Cnt = 0;
            foreach (var item in DebugInfoSector)
            {
                List<string> ls = new List<string>();
                ls = MyINI.IniReadAllValues(item);

                if (ls.GroupBy(i => i).Where(g => g.Count() == 8).Count() > 0)//byte功能
                {
                    var ByteName = ls[0];
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugSectionMapping.ContainsKey(ByteName))
                    {
                        var Ori = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugSectionMapping[ByteName];
                        Ori += string.Format("{0},", DebugInfoIndex[iDebugInfo_Cnt]);
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugSectionMapping[ByteName] = Ori;
                    }
                    else
                    {
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugSectionMapping.Add(ByteName
                        , string.Format("{0},", DebugInfoIndex[iDebugInfo_Cnt]));
                    }
                }
                else//bit功能
                {
                    foreach (var BitName in ls)
                    {
                        if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugSectionMapping.ContainsKey(BitName))
                            continue;
                        int iStart = ls.FindIndex(x => x == BitName);
                        int iEnd = ls.FindLastIndex(x => x == BitName);
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugSectionMapping.Add(BitName
                            , string.Format("{0}_{1}_{2},", DebugInfoIndex[iDebugInfo_Cnt], iStart, iEnd));
                    }
                }
                iDebugInfo_Cnt++;
            }

            foreach (var employee in UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugSectionMapping)
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugMappingBuff.Add(employee.Key, new List<int>());
            #endregion

            #region Pen
            strSection = "PenDebugInfoName";
            List<string> PenDebugInfoIndex = MyINI.IniReadAllValues(strSection);
            List<string> PenDebugInfoSector = MyINI.IniReadAllKeys(strSection);

            if (PenDebugInfoIndex.Count == 0)
                return false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugMappingBuff.Clear();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugSectionMapping.Clear();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugSectionMapping = new Dictionary<string, string>();
            int iPenDebugInfo_Cnt = 0;
            foreach (var item in PenDebugInfoSector)
            {
                List<string> ls = new List<string>();
                ls = MyINI.IniReadAllValues(item);

                if (ls.GroupBy(i => i).Where(g => g.Count() == 8).Count() > 0)//byte功能
                {
                    var ByteName = ls[0];
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugSectionMapping.ContainsKey(ByteName))
                    {
                        var Ori = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugSectionMapping[ByteName];
                        Ori += string.Format("{0},", PenDebugInfoIndex[iPenDebugInfo_Cnt]);
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugSectionMapping[ByteName] = Ori;
                    }
                    else
                    {
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugSectionMapping.Add(ByteName
                        , string.Format("{0},", PenDebugInfoIndex[iPenDebugInfo_Cnt]));
                    }
                }
                else//bit功能
                {
                    foreach (var BitName in ls)
                    {
                        if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugSectionMapping.ContainsKey(BitName))
                            continue;
                        int iStart = ls.FindIndex(x => x == BitName);
                        int iEnd = ls.FindLastIndex(x => x == BitName);
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugSectionMapping.Add(BitName
                            , string.Format("{0}_{1}_{2},", PenDebugInfoIndex[iPenDebugInfo_Cnt], iStart, iEnd));
                    }
                }
                iPenDebugInfo_Cnt++;
            }

            Dictionary<string, string> PenDebugSectionTemp = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugSectionMapping.Where(x => x.Key.Contains("SNR")).ToDictionary(x => x.Key, x => x.Value);

            for (int i = 0; i < UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iPen_TermCount; i++)
            {
                foreach (var employee in PenDebugSectionTemp)
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugMappingBuff.Add(employee.Key + "_" + i.ToString(), new List<int>());
                }
            }

            foreach (var employee in UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugSectionMapping)
            {
                if (!employee.Key.Contains("SNR"))
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugMappingBuff.Add(employee.Key, new List<int>());
            }
            #endregion
            return true;
        }

        private void DataDisplayMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSelected = ((ToolStripComboBox)sender).SelectedItem.ToString();
            DataDisplayMode nowValue = (DataDisplayMode)Enum.Parse(typeof(DataDisplayMode), strSelected);
            if (nowValue.Equals(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode))
            {
                Main_MenuStrip.Close();
                return;
            }

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode = nowValue;
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bRunningStatus || StaticVar.IsPlayback_Mode)
            {
                MY_INI.IniWriteValue("Setting", "Display_Mode", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode.ToString());
                Main_MenuStrip.Close();
                //Make it Reset
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintFullScreenExit, (IntPtr)0, (IntPtr)0);
            }
        }

        private void ScreenShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bRunningStatus)
                vfChangeResolution();

            Main_MenuStrip.Close();
        }

        private void ColorDisplayMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSelected = ((ToolStripComboBox)sender).SelectedItem.ToString();
            enBackGroundColor nowValue = (enBackGroundColor)Enum.Parse(typeof(enBackGroundColor), strSelected);
            if (nowValue.Equals(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor))
            {
                Main_MenuStrip.Close();
                return;
            }

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor = nowValue;
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bRunningStatus || StaticVar.IsPlayback_Mode)
            {
                MY_INI.IniWriteValue("Setting", "BackgroundColor", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor.ToString());
                Main_MenuStrip.Close();
                m_panAAscen.BackColor = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor);
                //UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintFullScreenExit, (IntPtr)0, (IntPtr)0);
            }
            vfChangeResolution();

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo != null)
            {
                InitFingerColor();
                if (UI_GBV.mDev[SelectedDevIndex].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.ListMode)
                {
                    int iFingers = UI_GBV.mDev[SelectedDevIndex].LocalVariable.Var_Multi.DevPanelInfo.iMaxTouchPoint;
                    m_dgvFingerStatus.Rows.Clear();
                    for (int inCount = 0; inCount < iFingers; inCount++)
                    {
                        m_dgvFingerStatus.Rows.Add(string.Format("F_{0}", inCount + 1), "0", "0", "0", "0", "0");
                        m_dgvFingerStatus.Rows[inCount].DefaultCellStyle.ForeColor = UI_GBV.mDev[SelectedDevIndex].LocalVariable.PaintTool_Var.coFingerColor[inCount];
                        m_dgvFingerStatus.Rows[inCount].DefaultCellStyle.SelectionForeColor = UI_GBV.mDev[SelectedDevIndex].LocalVariable.PaintTool_Var.coFingerColor[inCount];
                    }
                }
                if (StaticVar.IsPlayback_Mode)
                {
                    AssignSelectFrameToBuffer(iReplay_SelectedIndex, userControl_ReplayPaint.b_Paint_ReplayUseNewFormat);
                }
            }

        }

        private void PenPressureEffect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSelected = ((ToolStripComboBox)sender).SelectedItem.ToString();
            enPenPressureEffect nowValue = (enPenPressureEffect)Enum.Parse(typeof(enPenPressureEffect), strSelected);
            if (nowValue.Equals(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PenPressure))
            {
                Main_MenuStrip.Close();
                return;
            }

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PenPressure = nowValue;
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bRunningStatus || StaticVar.IsPlayback_Mode)
            {
                MY_INI.IniWriteValue("Pressure", "DouScale", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PenPressure.ToString());
                Main_MenuStrip.Close();
                //UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintFullScreenExit, (IntPtr)0, (IntPtr)0);
            }
            //vfChangeResolution();
        }

        private void PenTiltType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSelected = ((ToolStripComboBox)sender).SelectedItem.ToString();
            enPenTiltType nowValue = EnumTool.GetValueFromDescription<enPenTiltType>(strSelected);
            if (nowValue.Equals(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PenTiltType))
            {
                Main_MenuStrip.Close();
                return;
            }

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PenTiltType = nowValue;

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bRunningStatus || StaticVar.IsPlayback_Mode)
            {
                MY_INI.IniWriteValue("Setting", "PenTiltType", UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PenTiltType.ToString());
                Main_MenuStrip.Close();
                //UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintFullScreenExit, (IntPtr)0, (IntPtr)0);
            }
                    
            //vfChangeResolution();
        }

        private void Initial_LabelLogText(ref _LabelLog obj, _LabelLog.Label_Type obj_Type, Point StartPosition)
        {
            if (obj_Type == _LabelLog.Label_Type.ReportRate)
            {
                obj.ReportRate = new KeyValuePair<Point, string>(StartPosition, "");
            }
            else if (obj_Type == _LabelLog.Label_Type.Algo)
            {
                int iLen = obj.Algo_Bit.Length;
                for (int i = 0; i < iLen; i++)
                {
                    Point NewPos = new Point(StartPosition.X, StartPosition.Y + LABLE_PADDING * i);
                    obj.Algo_Bit[i] = new KeyValuePair<Point, string>(NewPos, "");
                }
            }
            else if (obj_Type == _LabelLog.Label_Type.Pen)
            {
                Point NewPos = new Point(StartPosition.X, StartPosition.Y);
                obj.Pen[0] = new KeyValuePair<Point, string>(NewPos, "");
            }
            else
            {
                int iLen = obj.Finger.Length;
                int iCol = 0;
                int iRow = 0;
                int iLebelW = 180;
                for (int i = 0; i < iLen; i++)
                {
                    Point NewPos;
                    if (i % 10 == 0)
                    {
                        NewPos = new Point(StartPosition.X + iLebelW * iCol, StartPosition.Y);
                        iCol = (i == 0) ? 0 : iCol++;
                        iRow = 1;
                    }
                    else
                    {
                        NewPos = new Point(StartPosition.X + iLebelW * iCol, StartPosition.Y + LABLE_PADDING * iRow);
                        iRow++;
                    }
                    obj.Finger[i] = new KeyValuePair<Point, string>(NewPos, "");
                }
            }
        }
        //20221227 Steven : 修復Full Sceen座標資料會覆蓋畫線的問題
        private void Update_LabelLogText(ref _LabelLog obj, _LabelLog.Label_Type obj_Type, int index, string Msg)
        {
            Font clear = new System.Drawing.Font("Consolas", 9.0F, FontStyle.Bold);
            if (obj_Type == _LabelLog.Label_Type.ReportRate)
            {
                if (Msg == "")
                {
                    if ((!string.Equals(obj.ReportRate.Value, Msg)) || (bReportRate_Cleared))
                    {
                        Point tmpPos = obj.ReportRate.Key;
                        MyPanelTest.BackColor = Color.Transparent;
                        MyPanelTest.Size = new Size(122, 15);
                        MyPanelTest.Location = tmpPos;
                        MyPanelTest.Visible = true;
                        MyPanelTest.Visible = false;

                        obj.ReportRate = new KeyValuePair<Point, string>(tmpPos, Msg);
                    }
                }

                if ((!string.Equals(obj.ReportRate.Value, Msg)) || (bReportRate_Cleared))
                {
                    Point tmpPos = obj.ReportRate.Key;
                    //SetPaintText(m_panAAscen, obj.ReportRate.Value, tmpPos, Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor), clear);

                    obj.ReportRate = new KeyValuePair<Point, string>(tmpPos, Msg);
                    SetPaintText(m_panAAscen, Msg, tmpPos, Color.Gold, null,122);
                    bReportRate_Cleared = false;
                }
            }
            else if (obj_Type == _LabelLog.Label_Type.Algo)
            {
                if (Msg == "")
                {
                    if ((!string.Equals(obj.Algo_Bit[index].Value, Msg)) || (_bAlgo_Cleared))
                    {
                        Point tmpPos = obj.Algo_Bit[index].Key;
                        MyPanelTest.BackColor = Color.Transparent;
                        MyPanelTest.Size = new Size(122, 15);
                        MyPanelTest.Location = tmpPos;
                        MyPanelTest.Visible = true;
                        MyPanelTest.Visible = false;

                        obj.Algo_Bit[index] = new KeyValuePair<Point, string>(tmpPos, Msg);
                    }
                }

                if ((!string.Equals(obj.Algo_Bit[index].Value, Msg)) || (_bAlgo_Cleared))//增加效能，避免收不到release點
                //if (!string.Equals(obj.Algo_Bit[index].Value, Msg))
                {
                    Point tmpPos = obj.Algo_Bit[index].Key;
                    //SetPaintText(m_panAAscen, obj.Algo_Bit[index].Value, tmpPos, Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor), clear);

                    obj.Algo_Bit[index] = new KeyValuePair<Point, string>(tmpPos, Msg);
                    SetPaintText(m_panAAscen, Msg, obj.Algo_Bit[index].Key, Color.BlueViolet, null);
                    _bAlgo_Cleared = false;
                }
            }
            else if (obj_Type == _LabelLog.Label_Type.Pen)
            {
                if (Msg == "")
                {
                    if (!string.Equals(obj.Pen[index].Value, Msg))
                    {
                        Point tmpPos = obj.Pen[index].Key;
                        MyPanelTest.BackColor = Color.Transparent;
                        MyPanelTest.Size = new Size(200, 15);
                        MyPanelTest.Location = tmpPos;
                        MyPanelTest.Visible = true;
                        MyPanelTest.Visible = false;

                        obj.Pen[index] = new KeyValuePair<Point, string>(tmpPos, Msg);
                    }
                }

                else if ((!string.Equals(obj.Pen[index].Value, Msg)))
                {
                    Point tmpPos = obj.Pen[index].Key;
                    SetPaintText(m_panAAscen, obj.Pen[index].Value, tmpPos, Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor), clear,180);

                    obj.Pen[index] = new KeyValuePair<Point, string>(tmpPos, Msg);
                    SetPaintText(m_panAAscen, Msg, obj.Pen[index].Key, Color.BlueViolet, null,180);

                }
            }

            else
            {
                if (Msg == "")
                {
                    if (!string.Equals(obj.Finger[index].Value, Msg))
                    {
                        Point tmpPos = obj.Finger[index].Key;
                        MyPanelTest.BackColor = Color.Transparent;
                        MyPanelTest.Size = new Size(iFingerDrawingLength, 15);
                        MyPanelTest.Location = tmpPos;
                        MyPanelTest.Visible = true;
                        MyPanelTest.Visible = false;

                        obj.Finger[index] = new KeyValuePair<Point, string>(tmpPos, Msg);
                    }
                }
                else if (!string.Equals(obj.Finger[index].Value, Msg))
                {
                    Point tmpPos = obj.Finger[index].Key;
                    SetPaintText(m_panAAscen, obj.Finger[index].Value, tmpPos, Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor), clear, iFingerDrawingLength);

                    obj.Finger[index] = new KeyValuePair<Point, string>(tmpPos, Msg);
                    SetPaintText(m_panAAscen, Msg, obj.Finger[index].Key, Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor][index]), null, iFingerDrawingLength);
                }
            }
        }

        private void SetPaintText(Panel inPanel, string str, Point Position, Color FontColor, Font _Font,int iRectangleWidith = 105)
        {
            //Graphics grp = this.m_panAAscen.CreateGraphics();
            Graphics grp = inPanel.CreateGraphics();
            grp.FillRectangle(new SolidBrush(Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor)), Position.X, Position.Y, iRectangleWidith, 15);
            //grp.Clear(Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor));

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
                | BindingFlags.Instance | BindingFlags.NonPublic, null, m_panAAscen, new object[] { true });

            if (FontColor == Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor))
            {
                mu_Clear.WaitOne();

                //MyOwnPen.Rectangle(new Point(Position.X +5, Position.Y + 5), new Point(Position.X + 200, Position.Y + 15));
                //Font _arialBold = (_Font == null) ? new Font("Consolas", 9.0F, FontStyle.Regular) : _Font;
                //using (Graphics gBit = Graphics.FromImage(bitmap))
                //{
                //    gBit.FillRectangle(new SolidBrush(FontColor), Position.X, Position.Y, 200, 15);
                //}

                if (mu_Clear != null)
                    mu_Clear.ReleaseMutex();
            }
            else
            {
                //TextRenderer.DrawText(Graphics.FromHwnd(this.m_panAAscen.Handle), str, _arialBold, Position, FontColor, TextFormatFlags.EndEllipsis);
                mu_Clear.WaitOne();
                Font _arialBold = (_Font == null) ? new Font("Consolas", 9.0F, FontStyle.Regular) : _Font;
                //StaticVar.LogWrite(_arialBold);
                //SetPaintText(Graphics.FromImage(bitmap), str, Position, FontColor, _arialBold);
                TextRenderer.DrawText(grp, str, _arialBold, Position, FontColor, TextFormatFlags.EndEllipsis);

                if (mu_Clear != null)
                    mu_Clear.ReleaseMutex();
            }
        }

        private void SetPaintText(Graphics grp, string str, Point Position, Color FontColor, Font _Font)
        {
            if (FontColor == Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor))
            {
                mu_Clear.WaitOne();
                //MyOwnPen.Rectangle(new Point(Position.X +5, Position.Y + 5), new Point(Position.X + 200, Position.Y + 15));
                if (mu_Clear != null)
                    mu_Clear.ReleaseMutex();
            }
            else
            {
                mu_Clear.WaitOne();
                Font _arialBold = (_Font == null) ? new Font("Consolas", 9.0F, FontStyle.Regular) : _Font;
                //修改畫到Bit Map的方法
                grp.DrawString(str, _Font, new SolidBrush(FontColor), Position);
                //TextRenderer.DrawText(grp, str, _arialBold, Position, FontColor, TextFormatFlags.EndEllipsis);
                if (mu_Clear != null)
                    mu_Clear.ReleaseMutex();
            }
        }

        public void InitVirtualButton()
        {
            if (BtnPen == null)
                BtnPen = new Gdi32();

            BtnPen.BeginGDI(Graphics.FromHwnd(this.m_panAAscen.Handle));
            //Font arialBold = new Font("Consolas", 20.0F, FontStyle.Bold);
            Color red = ColorTranslator.FromHtml("#FF0000");
            //int iButtonSize = 1000;
            int iH = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Height * 2) / 3;
            int iW = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Width / 2;
            //==================================================
            VirtualBtn[0] = new MultiPaintTool_VAR._VirtualBtn();
            VirtualBtn[0].Text = "Clean";
            VirtualBtn[0].bVisible = true;
            VirtualBtn[0].iHeight = (int)(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Height) / 20; //20220623 Steven: Virtual長寬改為1/20螢幕大小，避免RES影響到大小
            VirtualBtn[0].iWidth = (int)(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Width) / 20;
            VirtualBtn[0].StartPos = new Point(iW - (VirtualBtn[0].iWidth * 3) - (VirtualBtn[0].iWidth / 2), iH);
            VirtualBtn[0].EndPos = new Point(VirtualBtn[0].StartPos.X + VirtualBtn[0].iWidth, VirtualBtn[0].StartPos.Y + VirtualBtn[0].iHeight);

            BtnPen.Rectangle(VirtualBtn[0].StartPos, VirtualBtn[0].EndPos);
            //20220503 Steven : 同步畫在Bitmap上
            Color nowBackColor = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor);
            Color PenColor = Color.FromArgb(255, (255 - nowBackColor.R), (255 - nowBackColor.G), (255 - nowBackColor.B));
            drawPen = new Pen(PenColor, 1);
            using (grp = Graphics.FromImage(bitmap))
                grp.DrawRectangle(drawPen, VirtualBtn[0].StartPos.X, VirtualBtn[0].StartPos.Y, (VirtualBtn[0].EndPos.X - VirtualBtn[0].StartPos.X) - 1, (VirtualBtn[0].EndPos.Y - VirtualBtn[0].StartPos.Y) - 1);

            Point CenterPos = new Point(VirtualBtn[0].StartPos.X + VirtualBtn[0].iWidth / 4, VirtualBtn[0].StartPos.Y + VirtualBtn[0].iHeight / 2);
            Btn_ClearRange.Reset();
            Btn_ClearRange.AddPolygon(new Point[] {VirtualBtn[0].StartPos
                , new Point(VirtualBtn[0].StartPos.X + VirtualBtn[0].iWidth, VirtualBtn[0].StartPos.Y)
                , new Point(VirtualBtn[0].StartPos.X + VirtualBtn[0].iWidth, VirtualBtn[0].StartPos.Y + VirtualBtn[0].iHeight)
                , new Point(VirtualBtn[0].StartPos.X, VirtualBtn[0].StartPos.Y + VirtualBtn[0].iHeight)});

            Rectangle rect1 = new Rectangle(CenterPos.X, CenterPos.Y, VirtualBtn[0].iWidth / 2, VirtualBtn[0].iHeight / 2);
            Font goodFont = MyOwnPen.FindFont(Graphics.FromHwnd(this.m_panAAscen.Handle), VirtualBtn[0].Text, rect1.Size, arialBold);
            SetPaintText(m_panAAscen, VirtualBtn[0].Text, CenterPos, PenColor, goodFont);
            //同步畫在bitmap上 20220503 Steven Add : 避免Hover清掉Button           
            SetPaintText(Graphics.FromImage(bitmap), VirtualBtn[0].Text, CenterPos, PenColor, goodFont);

            //==================================================
            VirtualBtn[1] = new MultiPaintTool_VAR._VirtualBtn();
            VirtualBtn[1].Text = "Exit";
            VirtualBtn[1].bVisible = true;
            VirtualBtn[1].iHeight = (int)(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Height) / 20; //20220623 Steven: Virtual長寬改為1/20螢幕大小，避免RES影響到大小
            VirtualBtn[1].iWidth = (int)(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Width) / 20;
            VirtualBtn[1].StartPos = new Point(iW + (VirtualBtn[1].iWidth * 3) - (VirtualBtn[1].iWidth / 2), iH);
            VirtualBtn[1].EndPos = new Point(VirtualBtn[1].StartPos.X + VirtualBtn[1].iWidth, VirtualBtn[1].StartPos.Y + VirtualBtn[1].iHeight);

            BtnPen.Rectangle(VirtualBtn[1].StartPos, VirtualBtn[1].EndPos);
            //20220503 Steven : 同步畫在Bitmap上
            drawPen = new Pen(PenColor, 1);
            using (grp = Graphics.FromImage(bitmap))
                grp.DrawRectangle(drawPen, VirtualBtn[1].StartPos.X, VirtualBtn[1].StartPos.Y, (VirtualBtn[1].EndPos.X - VirtualBtn[1].StartPos.X) - 1, (VirtualBtn[1].EndPos.Y - VirtualBtn[1].StartPos.Y) - 1);

            Point CenterPos_2 = new Point(VirtualBtn[1].StartPos.X + VirtualBtn[1].iWidth / 4, VirtualBtn[1].StartPos.Y + VirtualBtn[1].iHeight / 2);
            Btn_ExitRange.Reset();
            Btn_ExitRange.AddPolygon(new Point[] {
                VirtualBtn[1].StartPos
                , new Point(VirtualBtn[1].StartPos.X + VirtualBtn[1].iWidth, VirtualBtn[1].StartPos.Y)
                , new Point(VirtualBtn[1].StartPos.X + VirtualBtn[1].iWidth, VirtualBtn[1].StartPos.Y+VirtualBtn[1].iHeight)
                , new Point(VirtualBtn[1].StartPos.X, VirtualBtn[1].StartPos.Y+VirtualBtn[1].iHeight)});
            Rectangle rect2 = new Rectangle(CenterPos_2.X, CenterPos_2.Y, VirtualBtn[1].iWidth / 2, VirtualBtn[1].iHeight / 2);
            goodFont = MyOwnPen.FindFont(Graphics.FromHwnd(this.m_panAAscen.Handle), VirtualBtn[1].Text, rect2.Size, arialBold);
            SetPaintText(m_panAAscen, VirtualBtn[1].Text, CenterPos_2, PenColor, goodFont);
            //同步畫在bitmap上 20220503 Steven Add : 避免Hover清掉Button          
            SetPaintText(Graphics.FromImage(bitmap), VirtualBtn[1].Text, CenterPos_2, PenColor, goodFont);

            bReportRate_Cleared = true;
            _bAlgo_Cleared = true;
        }

        public void FillKey_ScreenPosition()
        {
            int i_W = (int)(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Template[0].Btn_W * UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.douScale_Width);
            int i_H = (int)(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Template[0].Btn_H * UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.douScale_Height);
            int i_Key_X = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Width / 2) - ((i_W / 2) * UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts);
            int i_Key_Y = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize.Height / 4);
            for (int inCount = 0; inCount < UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts; inCount++)
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Template[inCount]._FingerID = inCount + 1;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition[inCount].Start_Point = new Point(i_Key_X + (inCount * i_W), i_Key_Y);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition[inCount].Btn_H = i_H;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition[inCount].Btn_W = i_W;
            }
        }

        public void PaintKeyRect()
        {
            foreach (var item in UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition)
                GDI_Pen.DrawRoundRectangle(Graphics.FromHwnd(this.m_panAAscen.Handle), Pens.Black
                    , new Rectangle(item.Start_Point, new Size(item.Btn_W, item.Btn_H)), 8);
        }

        private void Paintfm_Shown(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void Paintfm_Resize(object sender, EventArgs e)
        {
            splitContainer_TouchInfo.SplitterDistance = (m_dgvAlgoData.Rows.Count + 1) * 24;
        }

        private void OnPanelAAScreen_Paint(object sender, PaintEventArgs e)
        {
            BufferedGraphicsContext current = BufferedGraphicsManager.Current;

            Rectangle resultRect = new Rectangle(e.ClipRectangle.Location, e.ClipRectangle.Size);
            BufferedGraphics bg = current.Allocate(e.Graphics, resultRect);
            Graphics grp = bg.Graphics;
            grp.Clear(Color.FromArgb((int)UI_GBV.mDev[0].LocalVariable.PaintTool_Var.BackGroundColor));

            //grp.DrawImage(HoverBmp, 0, 0);

            if (!StaticVar.IsPlayback_Mode)
            {
                grp.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                grp.DrawImage(bitmap, Point.Empty);
            }
            else
            {
                grp.DrawImage(bitmap, 0, 0);
                //Draw(e.Graphics);
            }

            bg.Render();
            bg.Dispose();

        }

        //private void Draw(Graphics e)
        //{
        //    BufferedGraphicsContext current = BufferedGraphicsManager.Current;
        //    BufferedGraphics bg;

        //    bg = current.Allocate(e, this.DisplayRectangle);
        //    Graphics g = bg.Graphics;//(3) 
        //    g.Clear(Color.FromArgb((int)UI_GBV.mDev[0].LocalVariable.PaintTool_Var.BackGroundColor));
        //    //if (mi_DrawObjects.Count == 0)
        //    //    return
        //    g.DrawImage(bitmap, 0, 0);

        //    bg.Render();
        //    bg.Dispose();
        //}

        public void RecordPainting()
        {
            if (StaticVar.bPaint_Record)
            {
                // Stop Record
                StaticVar.bPaint_Record = false;
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.StopRecord();
            }
            else
            {
                // Start Record
                string FILE_NAME = ITS_Directory.RecordPath + "Record_Paint";
                if (!Directory.Exists(FILE_NAME))
                    Directory.CreateDirectory(FILE_NAME);

                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.RecordFile_Header(Application.ProductVersion);
                //Timer_Bar.Start();
                StaticVar.bPaint_Record = true;
            }
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.FullScreen)
            {
                vfChangeResolution();
            }

            uiPanel_OnAIR.Visible = StaticVar.bPaint_Record;
            uiWaitingBar1.Enabled = StaticVar.bPaint_Record;
            Paint_Record.Checked = StaticVar.bPaint_Record;
        }

        private void Paint_Record_Click(object sender, EventArgs e)
        {
            RecordPainting();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!userControl_ReplayPaint.Visible)
                return false;

            if (!isFinishReplayByScrollBar)
                return false;

            switch (keyData)
            {
                case Keys.Right:
                    userControl_ReplayPaint.TrackBarValuePlus();
                    break;
                case Keys.Left:
                    userControl_ReplayPaint.TrackBarValueReduce();
                    break;
                default:
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Paintfm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_PostMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintCMMonitor_Close, IntPtr.Zero, IntPtr.Zero);
        }

        private void Paintfm_Load(object sender, EventArgs e)
        {
            GetDebugAddressRange();

        }

        private void SetDebugDataGridView()
        {
            INI_Tool MyINI = new INI_Tool(ITS_Directory.PaintTool_INI);
            //List<string> m_Values = MyINI.IniReadAllKeys("DebugInfoName");
            List<string> m_Values = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.DebugMappingBuff.Keys.ToList();
            DataTable _dt_ = new DataTable();
            _dt_.Columns.AddRange(new DataColumn[] {
                new DataColumn("Name", typeof(string)) ,
                new DataColumn("Value", typeof(string))
            });

            //Finger
            foreach (var item in m_Values)
            {
                if (item == "NA")
                    continue;
                DataRow _row_ = _dt_.NewRow();
                _row_["Name"] = item;
                _dt_.Rows.Add(_row_);
            }
            dataGridView_FingerDebugInfo.DataSource = null;
            dataGridView_PenDebugInfo.DataSource = null;

            dataGridView_FingerDebugInfo.Rows.Clear();
            dataGridView_PenDebugInfo.Rows.Clear();

            dataGridView_FingerDebugInfo.DataSource = _dt_;
            dataGridView_FingerDebugInfo.Columns["Name"].Frozen = true;
            dataGridView_FingerDebugInfo.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView_FingerDebugInfo.Columns["Name"].ReadOnly = true;

            dataGridView_FingerDebugInfo.Columns["Value"].Frozen = false;
            dataGridView_FingerDebugInfo.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_FingerDebugInfo.Columns["Value"].ReadOnly = true;

            //Pen
            //if (!StaticVar.IsPlayback_Mode)
            //{
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType != Pen_CDC_Format.enPenType.Disable || StaticVar.IsPlayback_Mode)
            {
                _dt_ = new DataTable();

                _dt_.Columns.AddRange(new DataColumn[] {
                new DataColumn("Name", typeof(string)) ,
                new DataColumn("Value", typeof(string))
                });

                m_Values = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.PenDebugMappingBuff.Keys.ToList();
                foreach (var item in m_Values)
                {
                    if (item == "NA")
                        continue;
                    DataRow _row_ = _dt_.NewRow();
                    _row_["Name"] = item;
                    _dt_.Rows.Add(_row_);
                }

                dataGridView_PenDebugInfo.DataSource = _dt_;
                dataGridView_PenDebugInfo.Columns["Name"].Frozen = true;
                dataGridView_PenDebugInfo.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView_PenDebugInfo.Columns["Name"].ReadOnly = true;

                dataGridView_PenDebugInfo.Columns["Value"].Frozen = false;
                dataGridView_PenDebugInfo.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_PenDebugInfo.Columns["Value"].ReadOnly = true;
            }
            //}
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl MySender = (TabControl)sender;
            MySender.SelectedTab.Refresh();
        }      
    }

    public class _LabelLog
    {
        public enum Label_Type { ReportRate, Algo, Finger, Pen };

        public KeyValuePair<Point, string> ReportRate = new KeyValuePair<Point, string>();
        public KeyValuePair<Point, string>[] Algo_Bit = new KeyValuePair<Point, string>[8];
        public KeyValuePair<Point, string>[] Finger = new KeyValuePair<Point, string>[40];
        public KeyValuePair<Point, string>[] Pen = new KeyValuePair<Point, string>[40];
    }

    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}