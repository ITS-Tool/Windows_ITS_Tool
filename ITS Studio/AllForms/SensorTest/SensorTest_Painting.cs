using CommonExt;
using ErrorMessage;
using GDI_32;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ThirdPartyTools;

namespace ITS_Studio
{
    public partial class fmSensorTest_Painting : Form
    {
        private int m_iSelectedDev = 0;

        public int SelectedDev
        {
            set
            {
                m_iSelectedDev = value;
            }
            get
            {
                return m_iSelectedDev;
            }
        }

        public SensorTest_Setting fm_Setting;
        public bool m_bTestRunning = true;
        public DynamicTool MyTool = new DynamicTool();
        public NotStaticVar LocalVariable;

        private double m_ScaleRatio_W = 0;
        private double m_ScaleRatio_H = 0;
        private double m_ScaleFW_PointRatio_H = 0;
        private double m_ScaleFW_PointRatio_W = 0;
        private bool isFinishUpdate = false;
        private Gdi32 GDI_Panel = new Gdi32();

        private Dictionary<enPaintPenStyle, IntPtr> m_Pen_Handle;
        private System.Timers.Timer TestTimer = new System.Timers.Timer();
        private stuLineTest[] NowTestLineStatus = new stuLineTest[2];//sensor test畫矩形測試支援兩指
        private bool m_bFirstPosition = true;
        private bool m_bAllBlocksPressed = false;
        private Point[] m_StartBlock;
        private Dictionary<int, Point[]> dic_StartPoint = new Dictionary<int, Point[]>();
        private Rectangle[] CrossCornerBlock = new Rectangle[4];
        private GraphicsPath BtnPASS_Range = new GraphicsPath();
        private GraphicsPath BtnFAIL_Range = new GraphicsPath();
        private GraphicsPath[] Corss3Range = new GraphicsPath[2];
        private Rectangle[] AllRectSection = new Rectangle[12];
        private Font arialBold = new Font("Consolas", 20.0F, FontStyle.Bold);
        private bool bDrawLineDisconnect = false;
        public Paint_Globe_Var m_SensorTestPaint_GBV;

        protected UIPage MyUIPage = new UIPage();
        private bool bLastTimeFail = false;
        private int iRecordFirstPointIndex = 0;
        private int iPrevPointRecord = 0;
        public Point DrawingPanelStart = new Point(0, 0);
        private List<bool> lsKeyResult;
        // Key Test
        private struct _RecPOINT
        {
            public int X1;
            public int Y1;
            public int X2;
            public int Y2;
            public bool bStatus;
            public TouchStatus tcsTouchStatus;

            public _RecPOINT(int X1, int Y1, int X2, int Y2)
            {
                this.X1 = X1;
                this.Y1 = Y1;
                this.X2 = X2;
                this.Y2 = Y2;
                this.bStatus = false;
                this.tcsTouchStatus = TouchStatus.Default;
            }
        }

        private class _Rectangle3_Struct
        {
            public Rectangle rect;
            public ResultStatus result;
        }

        private class _Cross3_Struct
        {
            public GraphicsPath grpicPath;
            public ResultStatus result;
        }

        private enum ResultStatus
        {
            PASS,
            FAIL,
            Continue,
            UserStop
        };

        private enum TouchStatus
        {
            Default,
            Down,
            Up,
        };

        private List<_RecPOINT> lsRectPoint = new List<_RecPOINT>();

        private struct stuLineTest
        {
            public Point Start;//測試線起始點
            public Point End;//測試線終結點
            public double dRadius;//半徑，用來判斷用
            public double dWidth;//Rect3專用
            public double dHeight;//Rect3專用
            public int inFigNub;//手指頭數量
            public enLineStatus IsClickStart;//是否按下起始點
            public enLineStatus IsOverRange;//過程中是否有超過
            public enLineStatus IsClickEnd;//是否按下終結點
            public enLineStatus IsContiune;//判斷在劃線過程是否有中斷過

            public stuLineTest(Point PointStart, Point PointEnd, double _dR, int iFigNub, enLineStatus _IsClickStart, enLineStatus _IsOverRange, enLineStatus _IsClickEnd)
            {
                this.Start = PointStart;
                this.End = PointEnd;
                this.dRadius = _dR;
                this.inFigNub = iFigNub;
                this.IsOverRange = _IsOverRange;
                this.IsClickStart = _IsClickStart;
                this.IsContiune = _IsOverRange;
                this.IsClickEnd = _IsClickEnd;
                this.dWidth = 0;
                this.dHeight = 0;
                //this.InJudgeRange = _InJudgeRange;
            }
        }

        private struct _Block
        {
            internal List<PointF> PolygonPath;
            internal bool bPressed;
            internal int ID;
            internal int LineGroup;
            internal int iCornerGroup;
            internal bool bCornerNeighbor;
        }

        public enum enLineStatus
        {
            Wait,
            _true,
            _false
        };

        private Label labTime = new Label();
        private Label labMeg = new Label();
        private Label labHint = new Label();
        private Label lbFingerID = new Label();
        private Dictionary<GraphicsPath, _Block> Blocks;
        private Dictionary<GraphicsPath, _Block> PressedBlocks;
        private Dictionary<int, _Rectangle3_Struct> Rect3Result = new Dictionary<int, _Rectangle3_Struct>();
        private Dictionary<int, _Cross3_Struct> Cross3Result = new Dictionary<int, _Cross3_Struct>();
        private List<Rectangle> lsRect3FingerRange = new List<Rectangle>();
        private GraphicsPath[] InsideCross = new GraphicsPath[2];
        private PointF[] InsideRectangle = new PointF[4];

        private delegate void function(Message msg);

        public fmSensorTest_Painting(ref NotStaticVar _NotStaticVar, int iDev)
        {
            InitializeComponent();
            ShowInTaskbar = false;

            LocalVariable = _NotStaticVar;
            m_SensorTestPaint_GBV = new Paint_Globe_Var();
            TestTimer.Enabled = false;
            TestTimer.Interval = 1000;
            if(StaticVar.bStartTimerOnTouch)
                LocalVariable.VarSenTest_Multi.m_TimerStart = false;
            else
                LocalVariable.VarSenTest_Multi.m_TimerStart = true;

            TestTimer.Elapsed += new System.Timers.ElapsedEventHandler(TestTimer_Tick);
            TestTimer.AutoReset = true;
            TestTimer.SynchronizingObject = this;

            GDI_Panel.BeginGDI(Graphics.FromHwnd(m_panDrawing.Handle));
            //20240704 Vane拿掉
            //20240809 Steven : 不能拿掉，否則OP Mode / Lite OP Mode無法讀取Drawing Test Pattern參數
            if (fm_Setting == null)
                fm_Setting = new SensorTest_Setting();//重複new 20230815 by evayn modify
            
            UI_GBV.mDev[iDev].m_GBV.Globe_Multi.m_Flow.TransformPosition += this.TransformPosition;
        }

        protected override void WndProc(ref Message m)
        {
            // Suppress the WM_UPDATEUISTATE message
            // 防止Ctrl or Tab觸發刷新
            if (m.Msg == 0x128)
                return;
            base.WndProc(ref m);
        }

        private Point TransformPosition(Point InPoint)
        {
            switch(LocalVariable.VarSenTest_Multi.RotationAngle)
            {
                default:
                case Orientations.DEGREES_CW_0:
                    return new Point(InPoint.X, InPoint.Y);

                case Orientations.DEGREES_CW_180:
                    return new Point(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax - InPoint.X, m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax - InPoint.Y);

                case Orientations.DEGREES_CW_90:
                    return new Point(InPoint.Y, m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax - InPoint.X);

                case Orientations.DEGREES_CW_270:
                    return new Point(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax - InPoint.Y, InPoint.X);
            }
        }

        //++畫線測試進入點
        public void DrawingLineTest_Pen()
        {
            if(!m_bTestRunning)
                return;

            LocalVariable.VarSenTest_Multi.m_TimerStart = true;//手指一碰就開始計時
            LocalVariable.VarSenTest_Multi.m_OnRepaintEvent = false;//讓UI Repaint事件停止進入
            var PaintVar = LocalVariable.VarPaint_Multi;
            if(PaintVar.PenTouchStatus == enTouchStatus.Hover)
                return;

            int inFigNumber = 0;
            this.Invoke(new Action(() =>
            {
                lbFingerID.Text = string.Format("FingerID : {0}", PaintVar.iFingerID);
            }));

            //結束點圓圈
            double offest_x = m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax / 150;
            double offest_y = m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax / 150;

            //更新第一點
            if(PaintVar.stuPen_New[inFigNumber].bDataIn == false)
                return;

            //+判斷
            PatternVerify(PaintVar, inFigNumber, PaintVar.PenTouchStatus, 0);
            //+手指經過的路線
            if(PaintVar.PenTouchStatus != enTouchStatus.Hover)
                DrawLine_Pen(ref PaintVar, offest_x, offest_y);

            //touch all up initial buffer
            //if ((!PaintVar.bTouchStatus) || _Result != ResultStatus.Continue)
            if(PaintVar.PenTouchStatus == enTouchStatus.Release)
            {
                for(int i = 0; i < PaintVar.iMaxSupportPen; i++)
                {
                    PaintVar.stuPen_Old[i] = new MultiPaintTool_VAR.PenStatus();
                    PaintVar.stuPen_New[i] = new MultiPaintTool_VAR.PenStatus();
                }

                //找不到rectangle 多指有機會在release時會畫出old線(會發生在inFigNumber > 2),暫解在releae時更新畫面
                if(LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PenRectangle)
                    LocalVariable.VarSenTest_Multi.m_OnRepaintEvent = true;
            }
        }

        private void DrawLine_Pen(ref MultiPaintTool_VAR PaintVar, double offest_x, double offest_y)
        {
            //bool bPaint_FreeDrawing = (LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PenFreeDrawing);
            Point old_P = new Point((int)(PaintVar.stuPen_Old[0].iPosition_X * m_ScaleRatio_W), (int)(PaintVar.stuPen_Old[0].iPosition_Y * m_ScaleRatio_H));
            Point new_P = new Point((int)(PaintVar.stuPen_New[0].iPosition_X * m_ScaleRatio_W), (int)(PaintVar.stuPen_New[0].iPosition_Y * m_ScaleRatio_H));
            GDI_Panel.AssignPenHandle(LocalVariable.PaintTool_Var._ColorPen[0]);
            //+顯示Struct內容
            //DBG_Tool.PrintProperties(PaintVar.stuPen_New[0]);

            if((LocalVariable.PaintTool_Var.g_NowTestPattern != enPaintTestPattern.Paint_PenCross2) &&
                    (LocalVariable.PaintTool_Var.g_NowTestPattern != enPaintTestPattern.Paint_PenRectangle2))
            {
                GDI_Panel.MoveTo(old_P);
                GDI_Panel.LineTo(new_P);
            }
            else
            {
                Cursor.Position = new Point(DrawingPanelStart.X + new_P.X, DrawingPanelStart.Y + new_P.Y);
            }

            bool bRelease = (PaintVar.stuPen_New[0].bTouch == false);
            bRelease &= (PaintVar.stuPen_Old[0].bTouch == true);
            if(!bRelease)
            {
                PaintVar.stuPen_Old[0] = new MultiPaintTool_VAR.PenStatus(PaintVar.stuPen_New[0]);

                if((PaintVar.stuPen_Old[0].bDataIn) && (!PaintVar.stuPen_Old[0].bTouch))
                    bDrawLineDisconnect = true;
            }
            else
            {
                if((LocalVariable.PaintTool_Var.g_NowTestPattern != enPaintTestPattern.Paint_PenCross2) &&
                    (LocalVariable.PaintTool_Var.g_NowTestPattern != enPaintTestPattern.Paint_PenRectangle2))
                    GDI_Panel.Ellipse((int)(old_P.X - offest_x * m_ScaleRatio_W)
                        , (int)(old_P.Y + offest_y * m_ScaleRatio_H)
                        , (int)(old_P.X + offest_x * m_ScaleRatio_W)
                        , (int)(old_P.Y - offest_y * m_ScaleRatio_H));
            }
        }
        /// <summary>
        /// SWAP_XY => X座標和Y座標對調
        /// Spin=Vertical => Position.Y = PanelSize.Height - Position.Y;
        /// Spin=Horizontal => Position.X = PanelSize.Width - Position.X;
        
        /// (RotationAngle 90)
        /// SWAP_XY && Spin=Vertical
        
        /// (RotationAngle 180)
        /// Spin=Vertical & Horizon
        
        /// (RotationAngle 270)
        /// SWAP_XY && Spin=Horizon
        /// </summary>
        public void DrawingLineTest_Finger()
        {
            if(!m_bTestRunning)
                return;
            LocalVariable.VarSenTest_Multi.m_TimerStart = true;//手指一碰就開始計時
            LocalVariable.VarSenTest_Multi.m_OnRepaintEvent = false;//讓UI Repaint事件停止進入
            var PaintVar = LocalVariable.VarPaint_Multi;
            int inCount = 0;
            int inFigNumber = 0;
            lbFingerID.Text = string.Format("FingerID : {0}", PaintVar.iFingerID);
            var _Result = ResultStatus.UserStop;
            //結束點圓圈
            double offest_x = m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax / 150;
            double offest_y = m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax / 150;
            //針對每一個FingerID處理
            for(inCount = 0; inCount < PaintVar.iFingerID; inCount++)
            {
                //除了Paint_Cross，其他的Pattern都可支援多指判斷
                if(LocalVariable.PaintTool_Var.g_NowTestPattern != enPaintTestPattern.Paint_FreeDrawing)
                {
                    if((LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PaintRectangle
                        || LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PaintRectangle3)
                        && (StaticVar.iRectFingerNum == 2))
                    {
                        if(inCount > 1)
                            break;
                    }
                    else if(LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_FingerDetection)
                    {
                        if(inCount > StaticVar.iDetect_FingerNum - 1)
                            break;
                    }
                }

                inFigNumber = PaintVar.iChangeFlag[inCount];

                bool bContinue = false;
                bContinue |= (PaintVar.stuFinger_Old[inFigNumber] == null);
                bContinue |= (PaintVar.stuFinger_New[inFigNumber] == null);
                bContinue |= (PaintVar.stuFinger_Old[inFigNumber].bDataIn == false);
                if(bContinue)
                    break;

                //更新第一點
                if(PaintVar.stuFinger_New[inFigNumber].bDataIn == false)
                    continue;

                if((LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PaintRectangle3)
                      && (StaticVar.iRectFingerNum == 1))
                {
                    PaintVar.bTouchStatus &= (PaintVar.stuFinger_Old[0].bTouch);
                    PaintVar.bTouchStatus &= !(PaintVar.stuFinger_Old[0].bTouch && PaintVar.stuFinger_Old[1].bTouch);
                }

                if(StaticVar.iRectFingerNum != 1)
                    PaintVar.bTouchStatus = PaintVar.stuFinger_Old[0].bTouch && PaintVar.stuFinger_Old[1].bTouch;

                //判斷
                _Result = PatternVerify(PaintVar, inFigNumber, (PaintVar.bTouchStatus ? enTouchStatus.Touch : enTouchStatus.Release), inCount);

                if(_Result == ResultStatus.Continue)
                    DrawLine(ref PaintVar, PaintVar.bTouchStatus, inFigNumber, offest_x, offest_y);//手指經過的路線
            }
            //Paint_FingerDetection是處理全部的Finger
            if(LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_FingerDetection)            
                DetectFingers_CheckPassFail(PaintVar, PaintVar.iFingerID, enTouchStatus.Touch);
            

            //touch all up initial buffer
            if(!PaintVar.bTouchStatus)
            {
                for(int i = 0; i < LocalVariable.Var_Multi.DevPanelInfo.iMaxTouchPoint; i++)
                {
                    PaintVar.stuFinger_Old[i] = new MultiPaintTool_VAR.FingerStatus();
                    PaintVar.stuFinger_New[i] = new MultiPaintTool_VAR.FingerStatus();
                }

                //找不到rectangle 多指有機會在release時會畫出old線(會發生在inFigNumber > 2),暫解在releae時更新畫面
                if(LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PaintRectangle)
                    LocalVariable.VarSenTest_Multi.m_OnRepaintEvent = true;
            }
        }

        private _DEBUG_ DBG_Tool = new _DEBUG_();

        private ResultStatus PatternVerify(MultiPaintTool_VAR PaintVar, int iFigNub, enTouchStatus TouchStatus, int finger_idx)
        {
            var tmpResult = ResultStatus.Continue;
            switch(LocalVariable.PaintTool_Var.g_NowTestPattern)
            {
                case enPaintTestPattern.Paint_PaintRectangle:
                case enPaintTestPattern.Paint_PenRectangle:
                case enPaintTestPattern.Paint_PaintCross:
                case enPaintTestPattern.Paint_PenCross:
                    if(TouchStatus != enTouchStatus.Hover)
                    {
                        tmpResult = PointToPoint_CheckPassFail(PaintVar, iFigNub, TouchStatus, LocalVariable.PaintTool_Var.g_NowTestPattern);
                        if(tmpResult != ResultStatus.Continue)
                            FinalCheck(LocalVariable.PaintTool_Var.g_NowTestPattern, tmpResult, finger_idx);
                    }
                    break;

                case enPaintTestPattern.Paint_FreeDrawing:
                case enPaintTestPattern.Paint_PenFreeDrawing:
                    if(LocalVariable.PaintTool_Var.bClickResultButton)
                    {
                        Point _Point = (LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_FreeDrawing) ?
                        new Point(PaintVar.stuFinger_Old[iFigNub].iPosition_X, PaintVar.stuFinger_Old[iFigNub].iPosition_Y) :
                        new Point(PaintVar.stuPen_Old[iFigNub].iPosition_X, PaintVar.stuPen_Old[iFigNub].iPosition_Y);
                        if(TouchStatus == enTouchStatus.Release)
                            tmpResult = Point_CheckPassFail(PaintVar, _Point, TouchStatus);
                        if(tmpResult != ResultStatus.Continue)
                            FinalCheck(LocalVariable.PaintTool_Var.g_NowTestPattern, tmpResult, finger_idx);
                    }
                    break;

                case enPaintTestPattern.Paint_PaintRectangle2:
                case enPaintTestPattern.Paint_PenRectangle2:
                    if(TouchStatus != enTouchStatus.Hover)
                    {
                        tmpResult = FillBorderColor(PaintVar, finger_idx, TouchStatus);
                        if(tmpResult != ResultStatus.Continue)
                            FinalCheck(LocalVariable.PaintTool_Var.g_NowTestPattern, tmpResult, finger_idx);
                    }
                    break;

                case enPaintTestPattern.Paint_PaintCross2:
                case enPaintTestPattern.Paint_PenCross2:
                    if(TouchStatus != enTouchStatus.Hover)
                    {
                        tmpResult = FillCrossColor(PaintVar, finger_idx, TouchStatus);
                        if(tmpResult != ResultStatus.Continue)
                            FinalCheck(LocalVariable.PaintTool_Var.g_NowTestPattern, tmpResult, finger_idx);
                    }
                    break;

                case enPaintTestPattern.Paint_PaintRectangle3:
                case enPaintTestPattern.Paint_PenRectangle3:
                    if(TouchStatus != enTouchStatus.Hover)
                    {
                        tmpResult = Rectangle3_CheckPassFail(PaintVar, iFigNub, TouchStatus, LocalVariable.PaintTool_Var.g_NowTestPattern);
                        if(tmpResult != ResultStatus.Continue)
                        {
                            Rect3Result.Clear();
                            lsRect3FingerRange.Clear();
                            FinalCheck(LocalVariable.PaintTool_Var.g_NowTestPattern, tmpResult, finger_idx);
                        }
                    }
                    break;

                case enPaintTestPattern.Paint_PaintCross3:
                case enPaintTestPattern.Paint_PenCross3:
                    if(TouchStatus != enTouchStatus.Hover)
                    {
                        tmpResult = Cross3_CheckPassFail(PaintVar, iFigNub, TouchStatus, LocalVariable.PaintTool_Var.g_NowTestPattern);
                        if(tmpResult != ResultStatus.Continue)
                        {
                            Cross3Result.Clear();
                            FinalCheck(LocalVariable.PaintTool_Var.g_NowTestPattern, tmpResult, finger_idx);
                        }
                    }
                    break;
                case enPaintTestPattern.Paint_PaintKey:
                     if(TouchStatus != enTouchStatus.Hover)
                    {
                        tmpResult = Key_CheckPassFail(TouchStatus);
                        if(tmpResult != ResultStatus.Continue)
                        {                         
                            FinalCheck(LocalVariable.PaintTool_Var.g_NowTestPattern, tmpResult, finger_idx);
                        }
                    }
                    break;

                case enPaintTestPattern.Paint_FingerDetection:
                default:
                    break;
            }
            return tmpResult;
        }

        private bool isFailOnceTime = false;

        private ResultStatus FirstPakID(MultiPaintTool_VAR PaintVar, int iFigID, enTouchStatus FigStatus, enPaintTestPattern _Pattern, List<Point> AllFingerData)
        {
            if((AllFingerData.Count == 1) && (NowTestLineStatus.Length == 1))
            {
                if(FirstPoint(_Pattern, new Point(AllFingerData[0].X, AllFingerData[0].Y)) == NowTestLineStatus.Length)
                {
                    bDrawLineDisconnect = false;
                    m_bFirstPosition = false;
                    return ResultStatus.Continue;
                }
                else
                {
                    bDrawLineDisconnect = true;
                    for(int inCount = 0; inCount < NowTestLineStatus.Length; inCount++)
                        NowTestLineStatus[inCount].IsOverRange = enLineStatus._true;

                    if(FigStatus == enTouchStatus.Release)
                    {
                        SetSideColor(_Pattern,
                            LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse,
                            LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse,
                            LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                        m_panDrawing.Invalidate();
                        InitLineStatus();
                        isFailOnceTime = false;
                    }
                    else
                    {
                        if(isFailOnceTime == false)
                            LocalVariable.PaintTool_Var.inRetry--;
                        if(!bLastTimeFail)
                            SetSideColor(_Pattern,
                                LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                                LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                                LocalVariable.PaintTool_Var._ComColor.Color_Fail);
                        bLastTimeFail = true;
                        isFailOnceTime = true;
                    }

                    string str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
                    vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
                    if(LocalVariable.PaintTool_Var.inRetry == 0)
                        return ResultStatus.FAIL;

                    return ResultStatus.Continue;
                }
            }
            //+兩指
            else if(AllFingerData.Count == 2 && NowTestLineStatus.Length == 2)
            {
                if(FirstPoint(_Pattern, iFigID, AllFingerData) == NowTestLineStatus.Length)
                {
                    //Console.WriteLine("FirstPoint");
                    bDrawLineDisconnect = false;
                    m_bFirstPosition = false;
                    return ResultStatus.Continue;
                }
                else
                {
                    //Console.WriteLine("not FirstPoint");
                    bDrawLineDisconnect = true;
                    for(int inCount = 0; inCount < NowTestLineStatus.Length; inCount++)
                        NowTestLineStatus[inCount].IsOverRange = enLineStatus._true;

                    if(FigStatus == enTouchStatus.Release)
                    {
                        SetSideColor(_Pattern, LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse, LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse, LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                        m_panDrawing.Invalidate();
                        InitLineStatus();

                        isFailOnceTime = false;
                    }
                    else
                    {
                        if(isFailOnceTime == false)
                            LocalVariable.PaintTool_Var.inRetry--;

                        if(!bLastTimeFail)
                            SetSideColor(_Pattern, LocalVariable.PaintTool_Var._ComColor.Color_Fail, LocalVariable.PaintTool_Var._ComColor.Color_Fail, LocalVariable.PaintTool_Var._ComColor.Color_Fail);

                        isFailOnceTime = true;
                        bLastTimeFail = true;
                    }

                    string str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
                    vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
                    if(LocalVariable.PaintTool_Var.inRetry == 0)
                        return ResultStatus.FAIL;

                    return ResultStatus.Continue;
                }
            }
            else if((AllFingerData.Count == 1) && (NowTestLineStatus.Length == 4)) //Rectangle3
            {
                if(FirstPoint(_Pattern, new Point(AllFingerData[0].X, AllFingerData[0].Y)) == 1)
                {
                    bDrawLineDisconnect = false;
                    m_bFirstPosition = false;
                    return ResultStatus.Continue;
                }
                else
                {
                    bDrawLineDisconnect = true;
                    for(int inCount = 0; inCount < NowTestLineStatus.Length; inCount++)
                        NowTestLineStatus[inCount].IsOverRange = enLineStatus._true;

                    if(FigStatus == enTouchStatus.Release)
                    {
                        //SetSideColor(_Pattern,
                        //    LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse,
                        //    LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse,
                        //    LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                        while(!isFinishUpdate)
                        {
                            Application.DoEvents();
                            MyTool.Delay_ms(10);
                        }

                        m_panDrawing.Invalidate();
                        InitLineStatus();
                        isFailOnceTime = false;
                    }
                    else
                    {
                        if(isFailOnceTime == false)
                            LocalVariable.PaintTool_Var.inRetry--;
                        if(!bLastTimeFail)
                        {
                            RectangleAllNodeResult(LocalVariable.PaintTool_Var._ComColor.Color_Fail);
                        }
                        bLastTimeFail = true;
                        isFailOnceTime = true;
                    }

                    string str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
                    vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
                    if(LocalVariable.PaintTool_Var.inRetry == 0)
                        return ResultStatus.FAIL;

                    return ResultStatus.Continue;
                }
            }
            else if(AllFingerData.Count == 2 && NowTestLineStatus.Length == 12) //Rectangle3 Double
            {
                if(FirstPoint(_Pattern, iFigID, AllFingerData) == 2)
                {
                    //Console.WriteLine("FirstPoint");
                    bDrawLineDisconnect = false;
                    m_bFirstPosition = false;
                    return ResultStatus.Continue;
                }
                else
                {
                    //Console.WriteLine("not FirstPoint");
                    bDrawLineDisconnect = true;
                    for(int inCount = 0; inCount < NowTestLineStatus.Length; inCount++)
                        NowTestLineStatus[inCount].IsOverRange = enLineStatus._true;

                    if(FigStatus == enTouchStatus.Release)
                    {
                        //SetSideColor(_Pattern, LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse, LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse, LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                        while(!isFinishUpdate)
                        {
                            Application.DoEvents();
                            MyTool.Delay_ms(10);
                        }
                        m_panDrawing.Invalidate();
                        InitLineStatus();

                        isFailOnceTime = false;
                    }
                    else
                    {
                        if(isFailOnceTime == false)
                            LocalVariable.PaintTool_Var.inRetry--;
                        if(!bLastTimeFail)
                        {
                            RectangleAllNodeResult(LocalVariable.PaintTool_Var._ComColor.Color_Fail);
                        }
                        bLastTimeFail = true;
                        isFailOnceTime = true;
                    }

                    string str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
                    vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
                    if(LocalVariable.PaintTool_Var.inRetry == 0)
                        return ResultStatus.FAIL;

                    return ResultStatus.Continue;
                }
            }
            else if((AllFingerData.Count == 1) && (NowTestLineStatus.Length == 2)) //Cross3
            {
                if(FirstPoint(_Pattern, new Point(AllFingerData[0].X, AllFingerData[0].Y)) == 2)
                {
                    bDrawLineDisconnect = false;
                    m_bFirstPosition = false;
                    return ResultStatus.Continue;
                }
                else
                {
                    bDrawLineDisconnect = true;
                    for(int inCount = 0; inCount < NowTestLineStatus.Length; inCount++)
                        NowTestLineStatus[inCount].IsOverRange = enLineStatus._true;

                    if(FigStatus == enTouchStatus.Release)
                    {
                        SetSideColor(_Pattern,
                            LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse,
                            LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse,
                            LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                        m_panDrawing.Invalidate();
                        InitLineStatus();
                        isFailOnceTime = false;
                    }
                    else
                    {
                        if(isFailOnceTime == false)
                            LocalVariable.PaintTool_Var.inRetry--;
                        if(!bLastTimeFail)
                            SetSideColor(_Pattern,
                                LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                                LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                                LocalVariable.PaintTool_Var._ComColor.Color_Fail);
                        bLastTimeFail = true;
                        isFailOnceTime = true;
                    }

                    string str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
                    vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
                    if(LocalVariable.PaintTool_Var.inRetry == 0)
                        return ResultStatus.FAIL;

                    return ResultStatus.Continue;
                }
            }
            else
            {
                m_bFirstPosition = true;
                if(FigStatus == enTouchStatus.Release)
                {
                    SetSideColor(_Pattern, LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse, LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse, LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                    m_panDrawing.Invalidate();
                    InitLineStatus();
                    isFailOnceTime = false;
                }
                else
                {
                    if(isFailOnceTime == false && NowTestLineStatus.Length != 2)
                        LocalVariable.PaintTool_Var.inRetry--;

                    if(NowTestLineStatus.Length == 1)
                    {
                        if(!bLastTimeFail)
                            SetSideColor(_Pattern, LocalVariable.PaintTool_Var._ComColor.Color_Fail, LocalVariable.PaintTool_Var._ComColor.Color_Fail, LocalVariable.PaintTool_Var._ComColor.Color_Fail);

                        isFailOnceTime = true;
                        bLastTimeFail = true;
                    }
                }
                string str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
                vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
                if(LocalVariable.PaintTool_Var.inRetry == 0)
                    return ResultStatus.FAIL;

                return ResultStatus.Continue;
            }
        }

        private Color tmpColor;

        private ResultStatus PointToPoint_CheckPassFail(MultiPaintTool_VAR PaintVar, int iFigID, enTouchStatus FigStatus, enPaintTestPattern _Pattern)
        {
            stuLineTest Testline1 = Array.FindLast(NowTestLineStatus, x => x.inFigNub == iFigID);
            stuLineTest Testline2 = Array.FindLast(NowTestLineStatus, x => x.inFigNub == iFigID);
            List<Point> AllFingerData = new List<Point>();
            if(PaintVar.NowPaintTestObj == enPaintTestObj.Finger)
            {
                foreach(var item in PaintVar.stuFinger_Old)
                    if(item.iPosition_X != 0 || item.iPosition_Y != 0)
                        AllFingerData.Add(new Point(item.iPosition_X, item.iPosition_Y));
            }
            else
            {
                foreach(var item in PaintVar.stuPen_Old)
                    if(item.iPosition_X != 0 || item.iPosition_Y != 0)
                        AllFingerData.Add(new Point(item.iPosition_X, item.iPosition_Y));
            }
            LocalVariable.VarSenTest_Multi.m_OnRepaintEvent = true;

            int iIndex = Array.FindIndex(NowTestLineStatus, x => x.inFigNub == iFigID);
            //第一次資料進入，找不到任何紀錄，所以要透過FirstPoint()
            if(iIndex == -1)
                return FirstPakID(PaintVar, iFigID, FigStatus, _Pattern, AllFingerData);

            if(m_bFirstPosition)
            {
                if(FigStatus == enTouchStatus.Release)
                {
                    m_panDrawing.Invalidate();
                    isFailOnceTime = false;
                    //Reset_stuTestLineStatus(_Pattern);
                    InitLineStatus();
                }
                return ResultStatus.Continue;
            }

            //=============================================
            // (Px,Py) 到 (X0,Y0),(X1,Y1) 垂直距離
            // L = | (Y0-Y1)Px + (X1-X0)Py + X0Y1 - Y0X1| / √[(Y0-Y1)2 + (X1-X0)2
            Testline1 = Array.FindLast(NowTestLineStatus, x => x.inFigNub == 0);
            int iY0_Y1 = Testline1.Start.Y - Testline1.End.Y;
            int iX1_X0 = Testline1.End.X - Testline1.Start.X;
            int iX0_Y1 = Testline1.Start.X * Testline1.End.Y;
            int iY0_X1 = Testline1.Start.Y * Testline1.End.X;
            int i2Y0_Y1 = Testline1.Start.Y - Testline1.End.Y;
            int i2X1_X0 = Testline1.End.X - Testline1.Start.X;
            int i2X0_Y1 = Testline1.Start.X * Testline1.End.Y;
            int i2Y0_X1 = Testline1.Start.Y * Testline1.End.X;

            if(NowTestLineStatus.Length == 2)
            {
                Testline2 = Array.FindLast(NowTestLineStatus, x => x.inFigNub == 1);
                i2Y0_Y1 = Testline2.Start.Y - Testline2.End.Y;
                i2X1_X0 = Testline2.End.X - Testline2.Start.X;
                i2X0_Y1 = Testline2.Start.X * Testline2.End.Y;
                i2Y0_X1 = Testline2.Start.Y * Testline2.End.X;
            }

            //!判斷畫線座標有無超出設定的範圍
            if(NowTestLineStatus.Length == 2)
            {
                double PositionRadius1 = Math.Abs(iY0_Y1 * AllFingerData[0].X + iX1_X0 * AllFingerData[0].Y + iX0_Y1 - iY0_X1) / Math.Sqrt(Math.Pow(iX1_X0, 2) + Math.Pow(iY0_Y1, 2));
                double PositionRadius2 = Math.Abs(i2Y0_Y1 * AllFingerData[1].X + i2X1_X0 * AllFingerData[1].Y + i2X0_Y1 - i2Y0_X1) / Math.Sqrt(Math.Pow(i2X1_X0, 2) + Math.Pow(i2Y0_Y1, 2));
                double PositionRadius3 = Math.Abs(i2Y0_Y1 * AllFingerData[0].X + i2X1_X0 * AllFingerData[0].Y + i2X0_Y1 - i2Y0_X1) / Math.Sqrt(Math.Pow(i2X1_X0, 2) + Math.Pow(i2Y0_Y1, 2));
                double PositionRadius4 = Math.Abs(iY0_Y1 * AllFingerData[1].X + iX1_X0 * AllFingerData[1].Y + iX0_Y1 - iY0_X1) / Math.Sqrt(Math.Pow(iX1_X0, 2) + Math.Pow(iY0_Y1, 2));
                if(((PositionRadius1 > Testline1.dRadius) || (PositionRadius2 > Testline2.dRadius)) && ((PositionRadius2 > Testline1.dRadius) || (PositionRadius1 > Testline2.dRadius))
                    && ((PositionRadius3 > Testline1.dRadius) || (PositionRadius4 > Testline2.dRadius)) && ((PositionRadius4 > Testline1.dRadius) || (PositionRadius3 > Testline2.dRadius)))
                {
                    if(NowTestLineStatus[iIndex].IsOverRange != enLineStatus._true)
                    {
                        NowTestLineStatus[iIndex].IsOverRange = enLineStatus._true;
                        SetSideColor(_Pattern,
                            LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                            LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                            LocalVariable.PaintTool_Var._ComColor.Color_Fail);
                        bLastTimeFail = true;
                        if(isFailOnceTime == false)
                        {
                            LocalVariable.PaintTool_Var.inRetry--;
                            isFailOnceTime = true;
                        }
                    }
                }
            }
            else
            {
                double PositionRadius = Math.Abs((iY0_Y1 * AllFingerData[iFigID].X) + (iX1_X0 * AllFingerData[iFigID].Y) + iX0_Y1 - iY0_X1) / Math.Sqrt(Math.Pow(iX1_X0, 2) + Math.Pow(iY0_Y1, 2));
                //Console.WriteLine("Val = {0}, Standard = {1}", PositionRadius, Testline.dRadius);
                if(PositionRadius > Testline1.dRadius)
                {
                    if(NowTestLineStatus[iIndex].IsOverRange != enLineStatus._true)
                    {
                        NowTestLineStatus[iIndex].IsOverRange = enLineStatus._true;
                        LocalVariable.PaintTool_Var.inRetry--;
                        SetSideColor(_Pattern,
                            LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                            LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                            LocalVariable.PaintTool_Var._ComColor.Color_Fail);
                        bLastTimeFail = true;
                    }
                }
            }
            string str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
            vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
            //!手指放開就還原
            if(NowTestLineStatus[iIndex].IsOverRange == enLineStatus._true)
            {
                //Console.WriteLine("==> OverRange");
                if(FigStatus == enTouchStatus.Release || FigStatus == enTouchStatus.Hover) //20220701 Steven Add
                {
                    SetSideColor(_Pattern,
                        LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse,
                        LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse,
                        LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                    m_panDrawing.Invalidate();
                    InitLineStatus();
                    isFailOnceTime = false;
                }

                if(LocalVariable.PaintTool_Var.inRetry == 0)
                    return ResultStatus.FAIL;

                return ResultStatus.Continue;
            }

            NowTestLineStatus[iIndex].IsOverRange = enLineStatus._false;

            switch(_Pattern)
            {
                case enPaintTestPattern.Paint_PaintRectangle:
                case enPaintTestPattern.Paint_PenRectangle:
                    // check End Bubble
                    if(NowTestLineStatus.Length == 2)
                    {
                        double _VectorValue1 = (double)Math.Sqrt(Math.Pow(Testline1.End.X - AllFingerData[0].X, 2) + Math.Pow(Testline1.End.Y - AllFingerData[0].Y, 2));
                        double _VectorValue2 = (double)Math.Sqrt(Math.Pow(Testline2.End.X - AllFingerData[1].X, 2) + Math.Pow(Testline2.End.Y - AllFingerData[1].Y, 2));
                        double _VectorValue3 = (double)Math.Sqrt(Math.Pow(Testline2.End.X - AllFingerData[0].X, 2) + Math.Pow(Testline2.End.Y - AllFingerData[0].Y, 2));
                        double _VectorValue4 = (double)Math.Sqrt(Math.Pow(Testline1.End.X - AllFingerData[1].X, 2) + Math.Pow(Testline1.End.Y - AllFingerData[1].Y, 2));
                        if((Testline1.dRadius >= _VectorValue1 && Testline2.dRadius >= _VectorValue2) || (Testline1.dRadius >= _VectorValue2 && Testline2.dRadius >= _VectorValue1)
                            || (Testline1.dRadius >= _VectorValue3 && Testline2.dRadius >= _VectorValue4) || (Testline1.dRadius >= _VectorValue4 && Testline2.dRadius >= _VectorValue3))
                        {
                            if(iFigID != 0)
                            {
                                SetSideColor(_Pattern,
                                    LocalVariable.PaintTool_Var._ComColor.Color_Pass,
                                    LocalVariable.PaintTool_Var._ComColor.Color_Pass,
                                    LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                                tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_Pass;

                                NowTestLineStatus[0].IsClickEnd = enLineStatus._true;
                                NowTestLineStatus[1].IsClickEnd = enLineStatus._true;
                            }
                        }
                    }
                    else
                    {
                        double _VectorValue = (double)Math.Sqrt(Math.Pow(Testline1.End.X - AllFingerData[iFigID].X, 2) + Math.Pow(Testline1.End.Y - AllFingerData[iFigID].Y, 2));
                        if(Testline1.dRadius >= _VectorValue)
                        {
                            if(tmpColor != LocalVariable.PaintTool_Var._ComColor.Color_Pass)
                            {
                                SetSideColor(_Pattern,
                                    LocalVariable.PaintTool_Var._ComColor.Color_Pass,
                                    LocalVariable.PaintTool_Var._ComColor.Color_Pass,
                                    LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                                tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_Pass;
                            }
                            if(FigStatus == enTouchStatus.Release)
                                NowTestLineStatus[iIndex].IsClickEnd = enLineStatus._true;
                        }
                        else
                        {
                            tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse;
                        }
                    }

                    break;

                case enPaintTestPattern.Paint_PaintCross:
                case enPaintTestPattern.Paint_PenCross:
                    //0==>起始點、1==>終結點
                    if(CrossCornerBlock[1].Contains(new Point(AllFingerData[iFigID].X, AllFingerData[iFigID].Y)))
                    {
                        if(tmpColor != LocalVariable.PaintTool_Var._ComColor.Color_Pass)
                        {
                            SetSideColor(_Pattern,
                                LocalVariable.PaintTool_Var._ComColor.Color_Pass,
                                LocalVariable.PaintTool_Var._ComColor.Color_Pass,
                                LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                            tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_Pass;
                        }
                        if(FigStatus == enTouchStatus.Release)
                            NowTestLineStatus[iIndex].IsClickEnd = enLineStatus._true;
                    }
                    else
                    {
                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse;
                    }
                    break;

                default:
                    break;
            }

            //假如手指在範圍內離開，就判定重來
            if(FigStatus == enTouchStatus.Release || FigStatus == enTouchStatus.Hover) //20220701 Steven Add
            {
                //Console.WriteLine("===> IsClickStart = {0}, IsClickEnd = {1}", NowTestLineStatus[iIndex].IsClickStart, NowTestLineStatus[iIndex].IsClickEnd);
                if((NowTestLineStatus[iIndex].IsClickEnd != enLineStatus._true) || (NowTestLineStatus[iIndex].IsClickStart != enLineStatus._true))
                {
                    if(isFailOnceTime == false)
                        LocalVariable.PaintTool_Var.inRetry--;
                    SetSideColor(_Pattern,
                        LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                        LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                        LocalVariable.PaintTool_Var._ComColor.Color_Fail);
                    isFailOnceTime = true;
                    bLastTimeFail = true;
                    str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
                    vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
                    Reset_stuTestLineStatus(_Pattern);
                    bDrawLineDisconnect = true;

                    if(LocalVariable.PaintTool_Var.inRetry == 0)
                        return ResultStatus.FAIL;
                }
                //bDrawLineDisconnect = true;
                m_panDrawing.Invalidate();
                isFailOnceTime = false;
            }
            //// check final status
            int inPassCount = 0;
            foreach(var item in NowTestLineStatus)
            {
                bool bResult = (item.inFigNub > -1);// 1. check FigNub
                //if (!bResult)
                //Console.WriteLine("1. check FigNub = {0}", bResult);

                bResult &= (item.IsClickStart == enLineStatus._true);// 2. check Start bubble
                //if (!bResult)
                //Console.WriteLine("2. check Start bubble = {0}", bResult);

                bResult &= (item.IsOverRange == enLineStatus._false);// 3. check Line Status
                //if (!bResult)
                //Console.WriteLine("3. check Line Status = {0}", bResult);

                bResult &= (bDrawLineDisconnect == false);//4. check line continue
                //if (!bResult)
                //Console.WriteLine("4. check line continue = {0}", bResult);

                bResult &= (item.IsClickEnd == enLineStatus._true);// 5. check End Bubble
                //if (!bResult)
                //Console.WriteLine("5. check End Bubble = {0}", bResult);

                bResult &= (LocalVariable.VarPaint_Multi.iFingerCount <= StaticVar.iRectFingerNum);
                //if (!bResult)
                //Console.WriteLine("6. iFingerCount = {0}", bResult);

                if(bResult)
                    inPassCount++;
            }

            return inPassCount == NowTestLineStatus.Length ? ResultStatus.PASS : ResultStatus.Continue;
        }

        private ResultStatus Rectangle3_CheckPassFail(MultiPaintTool_VAR PaintVar, int iFigID, enTouchStatus FigStatus, enPaintTestPattern _Pattern)
        {
            stuLineTest Testline1 = NowTestLineStatus[0];
            stuLineTest Testline2 = NowTestLineStatus[1];
            stuLineTest Testline3 = NowTestLineStatus[2];
            stuLineTest Testline4 = NowTestLineStatus[3];

            if (PaintVar.stuFinger_Old.Length >= 4 && PaintVar.stuFinger_New.Length >= 4)
            {
                PaintVar.stuFinger_Old[2] = new MultiPaintTool_VAR.FingerStatus();
                PaintVar.stuFinger_New[2] = new MultiPaintTool_VAR.FingerStatus();
                PaintVar.stuFinger_Old[3] = new MultiPaintTool_VAR.FingerStatus();
                PaintVar.stuFinger_New[3] = new MultiPaintTool_VAR.FingerStatus();
            }
          

            if (StaticVar.iRectFingerNum == 1)
            {
                PaintVar.stuFinger_Old[1] = new MultiPaintTool_VAR.FingerStatus();
                PaintVar.stuFinger_New[1] = new MultiPaintTool_VAR.FingerStatus();
            }
            List<Point> AllFingerData = new List<Point>();
            if(PaintVar.NowPaintTestObj == enPaintTestObj.Finger)
            {
                foreach(var item in PaintVar.stuFinger_Old)
                    if(item.iPosition_X != 0 || item.iPosition_Y != 0)
                        AllFingerData.Add(new Point(item.iPosition_X, item.iPosition_Y));
            }
            else
            {
                foreach(var item in PaintVar.stuPen_Old)
                    if(item.iPosition_X != 0 || item.iPosition_Y != 0)
                        AllFingerData.Add(new Point(item.iPosition_X, item.iPosition_Y));
            }
            LocalVariable.VarSenTest_Multi.m_OnRepaintEvent = true;

            if(AllFingerData.Count != StaticVar.iRectFingerNum)
            {
                return ResultStatus.Continue;
            }

            int iFigID2 = (iFigID == 0) ? 1 : 0;

            int iIndex = Array.FindIndex(NowTestLineStatus, x => x.inFigNub == iFigID);
            int iIndex2 = Array.FindIndex(NowTestLineStatus, x => x.inFigNub == iFigID2);
            //第一次資料進入，找不到任何紀錄，所以要透過FirstPoint()
            if(iIndex == -1)
                return FirstPakID(PaintVar, iFigID, FigStatus, _Pattern, AllFingerData);

            if(m_bFirstPosition)
            {
                if(FigStatus == enTouchStatus.Release)
                {
                    while(!isFinishUpdate)
                    {
                        Application.DoEvents();
                        MyTool.Delay_ms(10);
                    }
                    m_panDrawing.Invalidate();
                    isFailOnceTime = false;
                    //Reset_stuTestLineStatus(_Pattern);
                    InitLineStatus();
                }
                return ResultStatus.Continue;
            }
            //=============================================
            // (Px,Py) 到 (X0,Y0),(X1,Y1) 垂直距離
            // L = | (Y0-Y1)Px + (X1-X0)Py + X0Y1 - Y0X1| / √[(Y0-Y1)2 + (X1-X0)2
            //Console.WriteLine("Val = {0}, Standard = {1}", PositionRadius, Testline.dRadius);
            double topLeftX = (double)Testline1.Start.X + Testline1.dWidth / 2;
            double topLeftY = (double)Testline1.Start.Y + Testline1.dHeight / 2;

            double ButtomRightX = (double)Testline1.End.X - Testline1.dWidth / 2;
            double ButtomRightY = (double)Testline2.End.Y - Testline2.dHeight / 2;

            bool bOutofRange = false;

            if(StaticVar.iRectFingerNum == 1)
            {
                bOutofRange = ((AllFingerData[iFigID].X > topLeftX) && (AllFingerData[iFigID].X < ButtomRightX));
                bOutofRange &= ((AllFingerData[iFigID].Y > topLeftY) && (AllFingerData[iFigID].Y < ButtomRightY));

                if(bOutofRange)
                {
                    if(NowTestLineStatus[iIndex].IsOverRange != enLineStatus._true)
                    {
                        NowTestLineStatus[iIndex].IsOverRange = enLineStatus._true;

                        RectangleAllNodeResult(LocalVariable.PaintTool_Var._ComColor.Color_Fail);

                        if(isFailOnceTime == false)
                        {
                            LocalVariable.PaintTool_Var.inRetry--;
                            isFailOnceTime = true;
                        }
                    }
                }
            }
            else
            {
                if(iIndex2 > iIndex)
                {
                    bOutofRange = ((AllFingerData[iFigID].X > topLeftX) && (AllFingerData[iFigID].X < ButtomRightX));
                    bOutofRange &= ((AllFingerData[iFigID].Y > topLeftY) && (AllFingerData[iFigID].Y < ButtomRightY));
                }
                else
                {
                    foreach(var item in lsRect3FingerRange)
                    {
                        bOutofRange |= item.Contains(AllFingerData[iFigID].X, AllFingerData[iFigID].Y);
                    }
                    bOutofRange = !bOutofRange;
                }

                if(bOutofRange)
                {
                    if(iIndex != -1 && iIndex2 != -1)
                    {
                        if(NowTestLineStatus[iIndex].IsOverRange != enLineStatus._true || NowTestLineStatus[iIndex2].IsOverRange != enLineStatus._true)
                        {
                            NowTestLineStatus[iIndex].IsOverRange = enLineStatus._true;
                            NowTestLineStatus[iIndex2].IsOverRange = enLineStatus._true;

                            RectangleAllNodeResult(LocalVariable.PaintTool_Var._ComColor.Color_Fail);

                            if(isFailOnceTime == false)
                            {
                                LocalVariable.PaintTool_Var.inRetry--;
                                isFailOnceTime = true;
                            }
                        }
                    }
                    else
                        return ResultStatus.Continue;
                }
            }

            string str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
            vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
            //!手指放開就還原
            if(StaticVar.iRectFingerNum == 1)
            {
                if(NowTestLineStatus[iIndex].IsOverRange == enLineStatus._true)
                {
                    //Console.WriteLine("==> OverRange");
                    if(FigStatus == enTouchStatus.Release || FigStatus == enTouchStatus.Hover) //20220701 Steven Add
                    {
                        while(!isFinishUpdate)
                        {
                            Application.DoEvents();
                            MyTool.Delay_ms(10);
                        }
                        m_panDrawing.Invalidate();
                        InitLineStatus();
                        isFailOnceTime = false;
                    }

                    if(LocalVariable.PaintTool_Var.inRetry == 0)
                        return ResultStatus.FAIL;

                    return ResultStatus.Continue;
                }
            }
            else
            {
                if(NowTestLineStatus[iIndex].IsOverRange == enLineStatus._true || NowTestLineStatus[iIndex2].IsOverRange == enLineStatus._true)
                {
                    //Console.WriteLine("==> OverRange");
                    if(FigStatus == enTouchStatus.Release || FigStatus == enTouchStatus.Hover) //20220701 Steven Add
                    {
                        while(!isFinishUpdate)
                        {
                            Application.DoEvents();
                            MyTool.Delay_ms(10);
                        }
                        m_panDrawing.Invalidate();
                        InitLineStatus();
                        isFailOnceTime = false;
                    }

                    if(LocalVariable.PaintTool_Var.inRetry == 0)
                        return ResultStatus.FAIL;

                    return ResultStatus.Continue;
                }
            }

            if(isFailOnceTime)
                return ResultStatus.Continue;
            bool isInRange = false;
            if(StaticVar.iRectFingerNum == 1)
            {
                double _VectorValue1 = (double)Math.Sqrt(Math.Pow(Testline1.End.X - AllFingerData[iFigID].X, 2) + Math.Pow(Testline1.End.Y - AllFingerData[iFigID].Y, 2));
                double _VectorValue2 = (double)Math.Sqrt(Math.Pow(Testline2.End.X - AllFingerData[iFigID].X, 2) + Math.Pow(Testline2.End.Y - AllFingerData[iFigID].Y, 2));
                double _VectorValue3 = (double)Math.Sqrt(Math.Pow(Testline3.End.X - AllFingerData[iFigID].X, 2) + Math.Pow(Testline3.End.Y - AllFingerData[iFigID].Y, 2));
                double _VectorValue4 = (double)Math.Sqrt(Math.Pow(Testline4.End.X - AllFingerData[iFigID].X, 2) + Math.Pow(Testline4.End.Y - AllFingerData[iFigID].Y, 2));

                if(AllFingerData[iFigID].X < (Testline1.Start.X + Testline1.dWidth / 2) && AllFingerData[iFigID].Y < (Testline1.Start.Y + Testline1.dHeight / 2))
                {
                    if(tmpColor != LocalVariable.PaintTool_Var._ComColor.Color_tmpPass)
                    {
                        Rectangle3PatternReset(0, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;
                        NowTestLineStatus[0].inFigNub = 0;
                        NowTestLineStatus[0].IsClickStart = enLineStatus._true;
                        NowTestLineStatus[0].IsOverRange = enLineStatus._false;
                    }

                    if(iPrevPointRecord == 1)
                        Rectangle3PathResult(0, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                    else if(iPrevPointRecord != 0)
                        Rectangle3PathResult(3, LocalVariable.PaintTool_Var._ComColor.Color_Pass);

                    NowTestLineStatus[0].IsClickEnd = enLineStatus._true;

                    iPrevPointRecord = 0;
                    isInRange = true;
                }
                else if(AllFingerData[iFigID].X > (Testline2.Start.X - Testline2.dWidth / 2) && AllFingerData[iFigID].Y < (Testline2.Start.Y + Testline2.dHeight / 2))
                {
                    if(tmpColor != LocalVariable.PaintTool_Var._ComColor.Color_tmpPass)
                    {
                        Rectangle3PatternReset(1, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;
                        NowTestLineStatus[1].inFigNub = 0;
                        NowTestLineStatus[1].IsClickStart = enLineStatus._true;
                        NowTestLineStatus[1].IsOverRange = enLineStatus._false;
                    }

                    if(iPrevPointRecord == 0)
                        Rectangle3PathResult(0, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                    else if(iPrevPointRecord != 1)
                        Rectangle3PathResult(1, LocalVariable.PaintTool_Var._ComColor.Color_Pass);

                    NowTestLineStatus[1].IsClickEnd = enLineStatus._true;

                    iPrevPointRecord = 1;
                    isInRange = true;
                }
                else if(AllFingerData[iFigID].X > (Testline3.Start.X - Testline3.dWidth / 2) && AllFingerData[iFigID].Y > (Testline3.Start.Y - Testline3.dHeight / 2))
                {
                    if(tmpColor != LocalVariable.PaintTool_Var._ComColor.Color_tmpPass)
                    {
                        Rectangle3PatternReset(2, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;
                        NowTestLineStatus[2].inFigNub = 0;
                        NowTestLineStatus[2].IsClickStart = enLineStatus._true;
                        NowTestLineStatus[2].IsOverRange = enLineStatus._false;
                    }

                    if(iPrevPointRecord == 1)
                        Rectangle3PathResult(1, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                    else if(iPrevPointRecord != 2)
                        Rectangle3PathResult(2, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                    NowTestLineStatus[2].IsClickEnd = enLineStatus._true;

                    iPrevPointRecord = 2;
                    isInRange = true;
                }
                else if(AllFingerData[iFigID].X < (Testline4.Start.X + Testline4.dWidth / 2) && AllFingerData[iFigID].Y > (Testline4.Start.Y - Testline4.dHeight / 2))
                {
                    if(tmpColor != LocalVariable.PaintTool_Var._ComColor.Color_tmpPass)
                    {
                        Rectangle3PatternReset(3, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;
                        NowTestLineStatus[3].inFigNub = 0;
                        NowTestLineStatus[3].IsClickStart = enLineStatus._true;
                        NowTestLineStatus[3].IsOverRange = enLineStatus._false;
                    }

                    if(iPrevPointRecord == 0)
                        Rectangle3PathResult(3, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                    else if(iPrevPointRecord != 3)
                        Rectangle3PathResult(2, LocalVariable.PaintTool_Var._ComColor.Color_Pass);

                    NowTestLineStatus[3].IsClickEnd = enLineStatus._true;

                    iPrevPointRecord = 3;
                    isInRange = true;
                }
                else
                {
                    tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse;
                }
            }
            else
            {
                int[] index = new int[2];
                if((AllRectSection[0].Contains(AllFingerData[0]) || AllRectSection[0].Contains(AllFingerData[1]))
                    && (AllRectSection[4].Contains(AllFingerData[0]) || AllRectSection[4].Contains(AllFingerData[1]))) //左上
                {
                    if(iRecordFirstPointIndex == 9)
                    {
                        RectangleNodeResult(0, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                        RectangleNodeResult(4, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);

                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;

                        Rectangle3PathResult(0, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                        isInRange = true;
                        NowTestLineStatus[iIndex].IsClickStart = enLineStatus._true;
                        NowTestLineStatus[iIndex].IsOverRange = enLineStatus._false;
                        NowTestLineStatus[iIndex].IsClickEnd = enLineStatus._true;
                    }
                }
                else if((AllRectSection[1].Contains(AllFingerData[0]) || AllRectSection[1].Contains(AllFingerData[1]))
                   && (AllRectSection[9].Contains(AllFingerData[0]) || AllRectSection[9].Contains(AllFingerData[1]))) //左上
                {
                    if(iRecordFirstPointIndex == 4)
                    {
                        RectangleNodeResult(1, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                        RectangleNodeResult(9, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);

                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;

                        Rectangle3PathResult(0, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                        isInRange = true;
                        NowTestLineStatus[iIndex].IsClickStart = enLineStatus._true;
                        NowTestLineStatus[iIndex].IsOverRange = enLineStatus._false;
                        NowTestLineStatus[iIndex].IsClickEnd = enLineStatus._true;
                    }
                }
                else if((AllRectSection[1].Contains(AllFingerData[0]) || AllRectSection[1].Contains(AllFingerData[1]))
                   && (AllRectSection[5].Contains(AllFingerData[0]) || AllRectSection[5].Contains(AllFingerData[1]))) //左上
                {
                    if(iRecordFirstPointIndex == 10)
                    {
                        RectangleNodeResult(1, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                        RectangleNodeResult(5, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);

                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;

                        Rectangle3PathResult(1, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                        isInRange = true;
                        NowTestLineStatus[iIndex].IsClickStart = enLineStatus._true;
                        NowTestLineStatus[iIndex].IsOverRange = enLineStatus._false;
                        NowTestLineStatus[iIndex].IsClickEnd = enLineStatus._true;
                    }
                }
                else if((AllRectSection[10].Contains(AllFingerData[0]) || AllRectSection[10].Contains(AllFingerData[1]))
                   && (AllRectSection[2].Contains(AllFingerData[0]) || AllRectSection[2].Contains(AllFingerData[1]))) //左上
                {
                    if(iRecordFirstPointIndex == 5)
                    {
                        RectangleNodeResult(10, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                        RectangleNodeResult(2, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);

                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;

                        Rectangle3PathResult(1, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                        isInRange = true;
                        NowTestLineStatus[iIndex].IsClickStart = enLineStatus._true;
                        NowTestLineStatus[iIndex].IsOverRange = enLineStatus._false;
                        NowTestLineStatus[iIndex].IsClickEnd = enLineStatus._true;
                    }
                }
                else if((AllRectSection[2].Contains(AllFingerData[0]) || AllRectSection[2].Contains(AllFingerData[1]))
                  && (AllRectSection[6].Contains(AllFingerData[0]) || AllRectSection[6].Contains(AllFingerData[1]))) //左上
                {
                    if(iRecordFirstPointIndex == 11)
                    {
                        RectangleNodeResult(2, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                        RectangleNodeResult(6, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);

                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;

                        Rectangle3PathResult(2, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                        isInRange = true;
                        NowTestLineStatus[iIndex].IsClickStart = enLineStatus._true;
                        NowTestLineStatus[iIndex].IsOverRange = enLineStatus._false;
                        NowTestLineStatus[iIndex].IsClickEnd = enLineStatus._true;
                    }
                }
                else if((AllRectSection[11].Contains(AllFingerData[0]) || AllRectSection[11].Contains(AllFingerData[1]))
                   && (AllRectSection[3].Contains(AllFingerData[0]) || AllRectSection[3].Contains(AllFingerData[1]))) //左上
                {
                    if(iRecordFirstPointIndex == 6)
                    {
                        RectangleNodeResult(11, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                        RectangleNodeResult(3, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);

                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;

                        Rectangle3PathResult(2, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                        isInRange = true;
                        NowTestLineStatus[iIndex].IsClickStart = enLineStatus._true;
                        NowTestLineStatus[iIndex].IsOverRange = enLineStatus._false;
                        NowTestLineStatus[iIndex].IsClickEnd = enLineStatus._true;
                    }
                }
                else if((AllRectSection[3].Contains(AllFingerData[0]) || AllRectSection[3].Contains(AllFingerData[1]))
                 && (AllRectSection[7].Contains(AllFingerData[0]) || AllRectSection[7].Contains(AllFingerData[1]))) //左上
                {
                    if(iRecordFirstPointIndex == 8)
                    {
                        RectangleNodeResult(3, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                        RectangleNodeResult(7, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);

                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;

                        Rectangle3PathResult(3, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                        isInRange = true;
                        NowTestLineStatus[iIndex].IsClickStart = enLineStatus._true;
                        NowTestLineStatus[iIndex].IsOverRange = enLineStatus._false;
                        NowTestLineStatus[iIndex].IsClickEnd = enLineStatus._true;
                    }
                }
                else if((AllRectSection[0].Contains(AllFingerData[0]) || AllRectSection[0].Contains(AllFingerData[1]))
                   && (AllRectSection[8].Contains(AllFingerData[0]) || AllRectSection[8].Contains(AllFingerData[1]))) //左上
                {
                    if(iRecordFirstPointIndex == 7)
                    {
                        RectangleNodeResult(0, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                        RectangleNodeResult(8, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);

                        tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;

                        Rectangle3PathResult(3, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                        isInRange = true;
                        NowTestLineStatus[iIndex].IsClickStart = enLineStatus._true;
                        NowTestLineStatus[iIndex].IsOverRange = enLineStatus._false;
                        NowTestLineStatus[iIndex].IsClickEnd = enLineStatus._true;
                    }
                }

                //isInRange = true;
            }

            //假如手指在範圍內離開，就判定重來
            if(FigStatus == enTouchStatus.Release || FigStatus == enTouchStatus.Hover) //20220701 Steven Add
            {
                //Console.WriteLine("===> IsClickStart = {0}, IsClickEnd = {1}", NowTestLineStatus[iIndex].IsClickStart, NowTestLineStatus[iIndex].IsClickEnd);
                if(!isInRange)
                {
                    if(isFailOnceTime == false)
                    {
                        LocalVariable.PaintTool_Var.inRetry--;
                    }

                    for(int i = 0; i < LocalVariable.PaintTool_Var.lsRectangleLineSetting.Count; i++)
                    {
                        Rectangle3PatternReset(i, LocalVariable.PaintTool_Var._ComColor.Color_Fail);
                    }
                    //NowTestLineStatus[iIndex].IsOverRange = enLineStatus._true;
                    isFailOnceTime = true;
                    bLastTimeFail = true;
                    str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
                    vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
                    Reset_stuTestLineStatus(_Pattern);
                    bDrawLineDisconnect = true;

                    if(LocalVariable.PaintTool_Var.inRetry == 0)
                        return ResultStatus.FAIL;
                }
                //else
                //{
                //    m_panDrawing.Invalidate();
                //}
                while(!isFinishUpdate)
                {
                    Application.DoEvents();
                    MyTool.Delay_ms(10);
                }
                m_panDrawing.Invalidate();

                isFailOnceTime = false;
            }
            //// check final status
            int inPassCount = 0;

            foreach(var item in Rect3Result)
            {
                bool bResult = true;
                bResult &= (item.Value.result == ResultStatus.PASS);
                if(bResult)
                    inPassCount++;
            }

            return inPassCount == Rect3Result.Count ? ResultStatus.PASS : ResultStatus.Continue;
        }

        private void ChangeCross3Rectangle(int index, Color _color)
        {
            IntPtr inpPenBrush = GDI_Panel.CreateBrushHandle(_color);
            GDI_Panel.AssignPenHandle(inpPenBrush);
            GDI_Panel.Rectangle(dic_StartPoint[index][0], dic_StartPoint[index][1]);
            GDI_Panel.DeleteOBJECT(inpPenBrush);
        }

        private ResultStatus Cross3_CheckPassFail(MultiPaintTool_VAR PaintVar, int iFigID, enTouchStatus FigStatus, enPaintTestPattern _Pattern)
        {
            List<Point> AllFingerData = new List<Point>();
            if(PaintVar.NowPaintTestObj == enPaintTestObj.Finger)
            {
                foreach(var item in PaintVar.stuFinger_Old)
                    if(item.iPosition_X != 0 || item.iPosition_Y != 0)
                        AllFingerData.Add(new Point(item.iPosition_X, item.iPosition_Y));
            }
            else
            {
                foreach(var item in PaintVar.stuPen_Old)
                    if(item.iPosition_X != 0 || item.iPosition_Y != 0)
                        AllFingerData.Add(new Point(item.iPosition_X, item.iPosition_Y));
            }
            LocalVariable.VarSenTest_Multi.m_OnRepaintEvent = true;

            int iIndex = Array.FindIndex(NowTestLineStatus, x => x.inFigNub == iFigID);
            //第一次資料進入，找不到任何紀錄，所以要透過FirstPoint()
            if(iIndex == -1)
                return FirstPakID(PaintVar, iFigID, FigStatus, _Pattern, AllFingerData);

            if(m_bFirstPosition)
            {
                if(FigStatus == enTouchStatus.Release)
                {
                    m_panDrawing.Invalidate();
                    isFailOnceTime = false;
                    //Reset_stuTestLineStatus(_Pattern);
                    InitLineStatus();
                }
                return ResultStatus.Continue;
            }

            //=============================================
            // (Px,Py) 到 (X0,Y0),(X1,Y1) 垂直距離
            // L = | (Y0-Y1)Px + (X1-X0)Py + X0Y1 - Y0X1| / √[(Y0-Y1)2 + (X1-X0)2

            //Console.WriteLine("Val = {0}, Standard = {1}", PositionRadius, Testline.dRadius);
            bool bOutofRange = false;
            foreach(var _item in Corss3Range)
                bOutofRange |= _item.IsVisible(AllFingerData[iFigID].X, AllFingerData[iFigID].Y);
            bOutofRange = !bOutofRange;

            if(bOutofRange)
            {
                if(NowTestLineStatus[iIndex].IsOverRange != enLineStatus._true)
                {
                    NowTestLineStatus[iIndex].IsOverRange = enLineStatus._true;
                    LocalVariable.PaintTool_Var.inRetry--;
                    SetSideColor(_Pattern,
                        LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                        LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                        LocalVariable.PaintTool_Var._ComColor.Color_Fail);
                    bLastTimeFail = true;
                }
            }

            string str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
            vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
            //!手指放開就還原
            if(NowTestLineStatus[iIndex].IsOverRange == enLineStatus._true)
            {
                //Console.WriteLine("==> OverRange");
                if(FigStatus == enTouchStatus.Release || FigStatus == enTouchStatus.Hover) //20220701 Steven Add
                {
                    SetSideColor(_Pattern,
                        LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse,
                        LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse,
                        LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                    m_panDrawing.Invalidate();
                    InitLineStatus();
                    isFailOnceTime = false;
                }

                if(LocalVariable.PaintTool_Var.inRetry == 0)
                    return ResultStatus.FAIL;

                return ResultStatus.Continue;
            }
            bool isInRange = false;
            NowTestLineStatus[iIndex].IsOverRange = enLineStatus._false;

            if(CrossCornerBlock[0].Contains(new Point(AllFingerData[iFigID].X, AllFingerData[iFigID].Y)))
            {
                if(tmpColor != LocalVariable.PaintTool_Var._ComColor.Color_tmpPass)
                {
                    ChangeCross3Rectangle(0, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);

                    tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;

                    NowTestLineStatus[0].IsClickStart = enLineStatus._true;
                    NowTestLineStatus[0].IsOverRange = enLineStatus._false;
                }

                if(iRecordFirstPointIndex == 1)
                {
                    CrossPathResult(0, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                    if(FigStatus == enTouchStatus.Release)
                    {
                        NowTestLineStatus[0].IsClickEnd = enLineStatus._true;
                        NowTestLineStatus[0].inFigNub = -1;
                        NowTestLineStatus[1].inFigNub = -1;
                    }
                }

                NowTestLineStatus[0].IsClickEnd = enLineStatus._true;
                isInRange = true;
            }
            else if(CrossCornerBlock[1].Contains(new Point(AllFingerData[iFigID].X, AllFingerData[iFigID].Y)))
            {
                if(tmpColor != LocalVariable.PaintTool_Var._ComColor.Color_tmpPass)
                {
                    ChangeCross3Rectangle(1, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);

                    tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;

                    NowTestLineStatus[0].IsClickStart = enLineStatus._true;
                    NowTestLineStatus[0].IsOverRange = enLineStatus._false;
                }

                if(iRecordFirstPointIndex == 0)
                {
                    CrossPathResult(0, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                    if(FigStatus == enTouchStatus.Release)
                    {
                        NowTestLineStatus[0].IsClickEnd = enLineStatus._true;
                        NowTestLineStatus[0].inFigNub = -1;
                        NowTestLineStatus[1].inFigNub = -1;
                    }
                }

                NowTestLineStatus[0].IsClickEnd = enLineStatus._true;
                isInRange = true;
            }
            else if(CrossCornerBlock[2].Contains(new Point(AllFingerData[iFigID].X, AllFingerData[iFigID].Y)))
            {
                if(tmpColor != LocalVariable.PaintTool_Var._ComColor.Color_tmpPass)
                {
                    ChangeCross3Rectangle(2, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);

                    tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;

                    NowTestLineStatus[1].IsClickStart = enLineStatus._true;
                    NowTestLineStatus[1].IsOverRange = enLineStatus._false;
                }

                if(iRecordFirstPointIndex == 3)
                {
                    CrossPathResult(1, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                    if(FigStatus == enTouchStatus.Release)
                    {
                        NowTestLineStatus[0].IsClickEnd = enLineStatus._true;
                        NowTestLineStatus[0].inFigNub = -1;
                        NowTestLineStatus[1].inFigNub = -1;
                    }
                }

                NowTestLineStatus[0].IsClickEnd = enLineStatus._true;
                isInRange = true;
            }
            else if(CrossCornerBlock[3].Contains(new Point(AllFingerData[iFigID].X, AllFingerData[iFigID].Y)))
            {
                if(tmpColor != LocalVariable.PaintTool_Var._ComColor.Color_tmpPass)
                {
                    ChangeCross3Rectangle(3, LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);

                    tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_tmpPass;

                    NowTestLineStatus[1].IsClickStart = enLineStatus._true;
                    NowTestLineStatus[1].IsOverRange = enLineStatus._false;
                }

                if(iRecordFirstPointIndex == 2)
                {
                    CrossPathResult(1, LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                    if(FigStatus == enTouchStatus.Release)
                    {
                        NowTestLineStatus[0].IsClickEnd = enLineStatus._true;
                        NowTestLineStatus[0].inFigNub = -1;
                        NowTestLineStatus[1].inFigNub = -1;
                    }
                }

                NowTestLineStatus[0].IsClickEnd = enLineStatus._true;
                isInRange = true;
            }
            else
            {
                tmpColor = LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse;
            }

            //假如手指在範圍內離開，就判定重來
            if(FigStatus == enTouchStatus.Release || FigStatus == enTouchStatus.Hover) //20220701 Steven Add
            {
                //Console.WriteLine("===> IsClickStart = {0}, IsClickEnd = {1}", NowTestLineStatus[iIndex].IsClickStart, NowTestLineStatus[iIndex].IsClickEnd);
                if(!isInRange)
                {
                    if(isFailOnceTime == false)
                        LocalVariable.PaintTool_Var.inRetry--;

                    SetSideColor(_Pattern,
                        LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                        LocalVariable.PaintTool_Var._ComColor.Color_Fail,
                        LocalVariable.PaintTool_Var._ComColor.Color_Fail);
                    isFailOnceTime = true;
                    bLastTimeFail = true;
                    str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
                    vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
                    Reset_stuTestLineStatus(_Pattern);
                    bDrawLineDisconnect = true;

                    m_panDrawing.Invalidate();

                    if(LocalVariable.PaintTool_Var.inRetry == 0)
                        return ResultStatus.FAIL;
                }
            }

            int inPassCount = 0;

            foreach(var item in Cross3Result)
            {
                bool bResult = true;
                bResult &= (item.Value.result == ResultStatus.PASS);
                if(bResult)
                    inPassCount++;
            }

            return inPassCount == NowTestLineStatus.Length ? ResultStatus.PASS : ResultStatus.Continue;
        }

        private ResultStatus Key_CheckPassFail(enTouchStatus TouchStatus)
        {
            //m_panDrawing.Paint -= OnPaintEvent;
            var PaintVar = LocalVariable.VarPaint_Multi;

            if (TouchStatus == enTouchStatus.Touch)
            {
                for (int inCount = 0; inCount < LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts; inCount++)
                {
                    if (PaintVar.strKey_New[inCount].bTouch)
                    {
                        SolidBrush _BrushesColor = new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_TouchKey);

                        var MyTest = LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition[inCount];
                        Rectangle rect1 = new Rectangle(MyTest.Start_Point, new Size(MyTest.Btn_W, MyTest.Btn_H));
                        lsKeyResult[inCount] = true;
                        GDI_Panel.FillRoundRectangle(Graphics.FromHwnd(this.m_panDrawing.Handle), _BrushesColor, rect1, 7);

                    }
                    else
                    {
                        if (lsKeyResult[inCount] == true)
                        {
                            SolidBrush _BrushesColor = new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_PassKey);

                            var MyTest = LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition[inCount];
                            Rectangle rect1 = new Rectangle(MyTest.Start_Point, new Size(MyTest.Btn_W, MyTest.Btn_H));
                            GDI_Panel.FillRoundRectangle(Graphics.FromHwnd(this.m_panDrawing.Handle), _BrushesColor, rect1, 7);
                        }                                           
                    }
                    LocalVariable.VarPaint_Multi.strKey_Old[inCount].bTouch = LocalVariable.VarPaint_Multi.strKey_New[inCount].bTouch;
                    LocalVariable.VarPaint_Multi.strKey_New[inCount].bTouch = false;
                }
            }
            else
            {
                for (int inCount = 0; inCount < lsKeyResult.Count; inCount++)
                {
                    if (lsKeyResult[inCount] == true)
                    {
                        SolidBrush _BrushesColor = new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_PassKey);

                        var MyTest = LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition[inCount];
                        Rectangle rect1 = new Rectangle(MyTest.Start_Point, new Size(MyTest.Btn_W, MyTest.Btn_H));
                        GDI_Panel.FillRoundRectangle(Graphics.FromHwnd(this.m_panDrawing.Handle), _BrushesColor, rect1, 7);
                    }                                           
                }
                if (lsKeyResult.Where(x => x == false).ToList().Count != 0)
                {
                    //m_panDrawing.Paint += OnPaintEvent;
                    MyTool.Delay_ms(500);
                    return ResultStatus.Continue;
                }
                else
                {
                    //m_panDrawing.Paint += OnPaintEvent;
                    return ResultStatus.PASS;
                }
            
            }
            return ResultStatus.Continue;
          
        }

        public void PenHandle_Init()
        {
            int iSize = Enum.GetNames(typeof(enPaintPenStyle)).Length;
            m_Pen_Handle = new Dictionary<enPaintPenStyle, IntPtr>();
            foreach(var item in EnumTool.EnumToList<enPaintPenStyle>())
            {
                IntPtr Handle = IntPtr.Zero;
                switch(item)
                {
                    case enPaintPenStyle.KeyDef:
                        Handle = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.coKeyDef);
                        break;

                    case enPaintPenStyle.KeyPass:
                        Handle = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.coKeyPass);
                        break;

                    case enPaintPenStyle.KeyTouch:
                        Handle = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.coKeyTouch);
                        break;

                    case enPaintPenStyle.Rectangle:
                        Handle = GDI_Panel.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 1, LocalVariable.PaintTool_Var._ComColor.Color_Rectangle);
                        break;
                }
                m_Pen_Handle.Add(item, Handle);
            }

            //m_Pen_Handle = new IntPtr[iSize];
            //m_Pen_Handle[(int)enPaintPenStyle.Rectangle] = GDI_Panel.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 1, LocalVariable.PaintTool_Var._ComColor.Color_Rectangle);
            //m_Pen_Handle[(int)enPaintPenStyle.KeyDef] = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.coKeyDef);
            //m_Pen_Handle[(int)enPaintPenStyle.KeyTouch] = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.coKeyTouch);
            //m_Pen_Handle[(int)enPaintPenStyle.KeyPass] = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.coKeyPass);
            // Pen
            if((LocalVariable.PaintTool_Var._ColorPen == null) || (LocalVariable.PaintTool_Var.inFingerColorRBG == null))
            {
                int iMaxFingers = LocalVariable.PaintTool_Var._BasicColor_.Count();
                LocalVariable.PaintTool_Var.coFingerColor = new Color[iMaxFingers];
                LocalVariable.PaintTool_Var.inFingerColorRBG = new Color[iMaxFingers];
                LocalVariable.PaintTool_Var._ColorPen = new IntPtr[iMaxFingers];
            }

            for(int inCount = 0; inCount < LocalVariable.Var_Multi.DevPanelInfo.iMaxTouchPoint; inCount++)
            {
                if(LocalVariable.PaintTool_Var._ColorPen.Count() > inCount)
                    LocalVariable.PaintTool_Var._ColorPen[inCount] = GDI_Panel.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 2, LocalVariable.PaintTool_Var.inFingerColorRBG[inCount]);
            }

            GDI_Panel.BeginGDI(Graphics.FromHwnd(m_panDrawing.Handle));
            GDI_Panel.AssignPenHandle(GDI_Panel.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 2, Color.Black));
        }

        public void SensorTest_ResetPattern(enPaintTestPattern _NowTestPattern)
        {
            m_panDrawing.Controls.Clear();

            //倒數計時功能
            if(_NowTestPattern != enPaintTestPattern.Paint_FreeDrawing || LocalVariable.PaintTool_Var.bClickResultButton == false)
            {
                labTime.Text = string.Format("{0}", LocalVariable.PaintTool_Var.inTimeOut);
                labTime.Visible = false;
                labTime.AutoSize = true;
                labTime.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                labTime.ForeColor = Color.Red;
                labTime.Font = new System.Drawing.Font("Consolas", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                m_panDrawing.Controls.Add(labTime);
            }

            //顯示Finger ID
            if((_NowTestPattern == enPaintTestPattern.Paint_FreeDrawing)
                || (_NowTestPattern == enPaintTestPattern.Paint_FingerDetection))
            {
                lbFingerID.Text = "FingerID : 0";
                lbFingerID.Visible = false;
                lbFingerID.AutoSize = true;
                lbFingerID.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                lbFingerID.ForeColor = Color.Red;
                lbFingerID.Font = new System.Drawing.Font("Consolas", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                m_panDrawing.Controls.Add(lbFingerID);
            }

            //訊息提示
            labMeg.Text = _NowTestPattern.Desc();
            labMeg.Visible = false;
            labMeg.AutoSize = true;
            labMeg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labMeg.ForeColor = Color.Green;
            labMeg.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_panDrawing.Controls.Add(labMeg);

            labHint.Text = "";
            labHint.Visible = false;
            labHint.AutoSize = true;
            labHint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labHint.ForeColor = Color.Blue;
            labHint.Font = new System.Drawing.Font("Consolas", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_panDrawing.Controls.Add(labHint);

            m_panDrawing.BackColor = LocalVariable.PaintTool_Var._ComColor.Color_Background;
            LocalVariable.PaintTool_Var.ReturnErrMsg.Clear();
            LocalVariable.PaintTool_Var.m_bTestResult = false;
            LocalVariable.PaintTool_Var.m_bRetryFail = false;

            string strKey = "Panel_Info";
            m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.X_resolution.ToString()));
            m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.Y_resolution.ToString()));
            m_SensorTestPaint_GBV.Panel_Var.inAAXmax = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAXmax.ToString()));
            m_SensorTestPaint_GBV.Panel_Var.inAAYmax = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAYmax.ToString()));
            m_SensorTestPaint_GBV.Panel_Var.inAAXmin = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAXmin.ToString()));
            m_SensorTestPaint_GBV.Panel_Var.inAAYmin = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAYmin.ToString()));
            m_SensorTestPaint_GBV.Panel_Var.inXChannel = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.XChannel.ToString()));
            m_SensorTestPaint_GBV.Panel_Var.inYChannel = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.YChannel.ToString()));
            m_panDrawing.Height = Size.Height;
            m_panDrawing.Width = Size.Width;
            m_panDrawing.Location = new System.Drawing.Point(0, 0);

            m_ScaleRatio_H = (double)(m_panDrawing.Height - 1) / (double)(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax - m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmin + 1);
            m_ScaleRatio_W = (double)(m_panDrawing.Width - 1) / (double)(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax - m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmin + 1);

            labHint.Location = new Point((m_panDrawing.Width / 2) - (labHint.Width / 2), m_panDrawing.Height / 4 * 3);
            labHint.Visible = true;
            // Msg Label Location
            labMeg.Location = new Point((m_panDrawing.Width / 2) - (labMeg.Width / 2), labHint.Location.Y + labHint.Height);
            labMeg.Visible = true;
            // Time out Label Location
            labTime.Location = new Point((m_panDrawing.Width / 2) - (labTime.Width / 2), labMeg.Location.Y + labMeg.Height);
            labTime.Visible = true;

            lbFingerID.Location = new Point((m_panDrawing.Width / 2) - (lbFingerID.Width / 2), labMeg.Location.Y + labMeg.Height + lbFingerID.Height);
            lbFingerID.Visible = true;
            LocalVariable.PaintTool_Var.bOutOfRange = false;
            Reset_stuTestLineStatus(_NowTestPattern);
        }

        public void SensorTest_TestStart(bool bTimer)
        {
            //TestTimer.Enabled = bTimer;
            if(bTimer)
                TestTimer.Start();
            else
                TestTimer.Stop();

            LocalVariable.PaintTool_Var.MuHandle = true;
            LocalVariable.PaintTool_Var.mutRuningHandle.WaitOne();
            MyTool.Delay_ms(50);
            m_bTestRunning = true;
            if(LocalVariable.VarSenTest_Multi.m_bDrawTestStop)
            {
                m_bTestRunning = false;
                Set_DrawingTestResult(ResultStatus.FAIL);
            }

            LocalVariable.VarSenTest_Multi.m_TimerStart = StaticVar.bStartTimerOnTouch && LocalVariable.VarSenTest_Multi.m_TimerStart;

            LocalVariable.VarPaint_Multi.bTouchStatus = false;
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        /// <summary>
        /// 使用 mouse_event 模擬滑鼠移動
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void MoveMouse(int x, int y)
        {
            const uint MOUSEEVENTF_MOVE = 0x0001;
            mouse_event(MOUSEEVENTF_MOVE, (uint)x, (uint)y, 0, UIntPtr.Zero);
        }

        private void DrawLine(ref MultiPaintTool_VAR PaintVar, bool bTouchStatus, int inFigNumber, double offest_x, double offest_y)
        {
            bool bPaint_FreeDrawing = (LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_FreeDrawing);
            Point old_P = new Point((int)(PaintVar.stuFinger_Old[inFigNumber].iPosition_X * m_ScaleRatio_W), (int)(PaintVar.stuFinger_Old[inFigNumber].iPosition_Y * m_ScaleRatio_H));
            Point new_P = new Point((int)(PaintVar.stuFinger_New[inFigNumber].iPosition_X * m_ScaleRatio_W), (int)(PaintVar.stuFinger_New[inFigNumber].iPosition_Y * m_ScaleRatio_H));
            GDI_Panel.AssignPenHandle(LocalVariable.PaintTool_Var._ColorPen[inFigNumber]);
            if(bTouchStatus)
            {
                if((LocalVariable.PaintTool_Var.g_NowTestPattern != enPaintTestPattern.Paint_PaintCross2) &&
                     (LocalVariable.PaintTool_Var.g_NowTestPattern != enPaintTestPattern.Paint_PaintRectangle2))
                {
                    GDI_Panel.MoveTo(old_P);
                    GDI_Panel.LineTo(new_P);
                }
                else
                {
                    Cursor.Position = new Point(DrawingPanelStart.X + new_P.X, DrawingPanelStart.Y + new_P.Y);
                }

                //顏色反相
                //if (!bPaint_FreeDrawing)
                //{
                //    GDI_Panel.SetROP2(Gdi32.drawingMode.R2_NOT);
                //    GDI_Panel.MoveTo(old_P);
                //    GDI_Panel.LineTo(new_P);
                //}

                //FreeDrawing 畫release的圈
                bPaint_FreeDrawing &= (PaintVar.stuFinger_New[inFigNumber].bTouch == false);
                bPaint_FreeDrawing &= (PaintVar.stuFinger_Old[inFigNumber].bTouch == true);
                if(bPaint_FreeDrawing)
                {
                    GDI_Panel.Ellipse((int)(old_P.X - offest_x * m_ScaleRatio_W)
                        , (int)(old_P.Y + offest_y * m_ScaleRatio_H)
                        , (int)(old_P.X + offest_x * m_ScaleRatio_W)
                        , (int)(old_P.Y - offest_y * m_ScaleRatio_H));
                    PaintVar.stuFinger_Old[inFigNumber] = new MultiPaintTool_VAR.FingerStatus();
                }
                else
                {
                    PaintVar.stuFinger_Old[inFigNumber] = new MultiPaintTool_VAR.FingerStatus(PaintVar.stuFinger_New[inFigNumber]);
                }

                PaintVar.stuFinger_New[inFigNumber] = new MultiPaintTool_VAR.FingerStatus();
                if((PaintVar.stuFinger_Old[inFigNumber].bDataIn) && (!PaintVar.stuFinger_Old[inFigNumber].bTouch))
                    bDrawLineDisconnect = true;
            }
            else
            {
                if((LocalVariable.PaintTool_Var.g_NowTestPattern != enPaintTestPattern.Paint_PaintCross2) &&
                     (LocalVariable.PaintTool_Var.g_NowTestPattern != enPaintTestPattern.Paint_PaintRectangle2))
                {
                    GDI_Panel.MoveTo(old_P);
                    GDI_Panel.LineTo(new_P);
                }
                //FreeDrawing 畫release的圈
                if(bPaint_FreeDrawing)
                {
                    GDI_Panel.Ellipse((int)(old_P.X - offest_x * m_ScaleRatio_W)
                        , (int)(old_P.Y + offest_y * m_ScaleRatio_H)
                        , (int)(old_P.X + offest_x * m_ScaleRatio_W)
                        , (int)(old_P.Y - offest_y * m_ScaleRatio_H));
                }
            }
        }

        // Time Out
        private void TestTimer_Tick(object sender, EventArgs e)
        {
            if(LocalVariable.VarPaint_Multi.bTouchStatus || LocalVariable.VarSenTest_Multi.m_TimerStart)
            {
                LocalVariable.VarSenTest_Multi.m_TimerStart = true;
                vfSetlabTimeText(string.Format("{0,4}", --LocalVariable.PaintTool_Var.inTimeOut));
                if(m_bTestRunning)
                {
                    if(LocalVariable.PaintTool_Var.inTimeOut <= 0)
                    {
                        //TestTimer.Enabled = false;
                        TestTimer.Stop();
                        m_bTestRunning = false;
                        switch(LocalVariable.PaintTool_Var.g_NowTestPattern)
                        {
                            case enPaintTestPattern.Paint_FreeDrawing:
                            case enPaintTestPattern.Paint_PenFreeDrawing:
                            case enPaintTestPattern.Paint_FingerDetection:
                                LocalVariable.VarSenTest_Multi.m_TimerStart = false;
                                break;

                            case enPaintTestPattern.Paint_PaintCross:
                            case enPaintTestPattern.Paint_PenCross:
                                LocalVariable.PaintTool_Var.ReturnErrMsg.AppendFormat("({0}) Time Out.\r\n", ((LineSetting_Cross)LocalVariable.PaintTool_Var.m_LineDirection).DirectionRule);
                                vfSetlabMegText(ErrMsg.SensorTest.TimeOut);
                                break;

                            case enPaintTestPattern.Paint_PaintRectangle:
                            case enPaintTestPattern.Paint_PenRectangle:
                                LocalVariable.PaintTool_Var.ReturnErrMsg.AppendFormat("({0}) Time Out.\r\n", ((LineSetting_Rectangle)LocalVariable.PaintTool_Var.m_LineDirection).DirectionRule);
                                break;

                            case enPaintTestPattern.Paint_PaintKey:
                            case enPaintTestPattern.Paint_PaintCross2:
                            case enPaintTestPattern.Paint_PaintRectangle2:
                            case enPaintTestPattern.Paint_PenCross2:
                            case enPaintTestPattern.Paint_PenRectangle2:
                                vfSetlabMegText(ErrMsg.SensorTest.TimeOut);
                                break;
                        }

                        if(LocalVariable.PaintTool_Var.MuHandle)
                        {
                            LocalVariable.VarSenTest_Multi.m_OnRepaintEvent = true;
                            Set_DrawingTestResult(ResultStatus.FAIL);
                        }
                    }
                }
                else
                {
                    TestTimer.Stop();
                    vfSetlabMegText(ErrMsg.SensorTest.UserStop);
                    if(LocalVariable.PaintTool_Var.MuHandle)   //发现之前在mutRuningHandle.ReleaseMutex()之后，还会进入到这里，导致mutRuningHandle.ReleaseMutex()出错。
                        Set_DrawingTestResult(ResultStatus.FAIL);
                }
            }

            if(!LocalVariable.VarPaint_Multi.bTouchStatus)
                lbFingerID.Text = string.Format("FingerID : {0}", 0);

            //=====================================
            if(LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_FingerDetection)
            {
                //if (LocalVariable.PaintTool_Var.bDetect_FingerPass)
                //{
                //    if (!LocalVariable.VarPaint_Multi.bTouchStatus)
                //        FinalCheck(enPaintTestPattern.Paint_FingerDetection, ResultStatus.PASS, 0);
                //}
                //else
                //{
                if(bStartCountsDown)
                    iTestCnts--;
                //}
            }
        }

        // Set Result & Release mutRuningHandle
        private void Set_DrawingTestResult(ResultStatus _ResultStatus)
        {
            if((_ResultStatus == ResultStatus.FAIL) && StaticVar.bDrawingTestFailStop)
            {
                LocalVariable.VarSenTest_Multi.m_bDrawTestStop = true;
                m_bTestRunning = false;
            }
            var DirectionRule = new object();

            switch(LocalVariable.PaintTool_Var.g_NowTestPattern)
            {
                case enPaintTestPattern.Paint_PaintRectangle:
                case enPaintTestPattern.Paint_PaintCross:
                case enPaintTestPattern.Paint_PenRectangle:
                case enPaintTestPattern.Paint_PenCross:
                    if((LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PaintRectangle)
                        || (LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PenRectangle))
                        DirectionRule = ((LineSetting_Rectangle)LocalVariable.PaintTool_Var.m_LineDirection).DirectionRule;
                    else
                        DirectionRule = ((LineSetting_Cross)LocalVariable.PaintTool_Var.m_LineDirection).DirectionRule;

                    switch(_ResultStatus)
                    {
                        case ResultStatus.PASS:
                            LocalVariable.PaintTool_Var.ReturnErrMsg.AppendFormat("{0,6} test done.\r\n", DirectionRule);
                            break;

                        case ResultStatus.UserStop:
                            LocalVariable.PaintTool_Var.ReturnErrMsg.AppendFormat("{0,6} test result : {1}.\r\n", DirectionRule, _ResultStatus);
                            break;

                        case ResultStatus.FAIL:
                            LocalVariable.PaintTool_Var.ReturnErrMsg.AppendFormat("{0,6} Retry over the upper limit ({1})\r\n", DirectionRule, LocalVariable.PaintTool_Var.inRetryUplimit);
                            break;
                    }
                    break;

                default:
                    LocalVariable.PaintTool_Var.ReturnErrMsg.AppendFormat("{0,6} test result : {1}.\r\n", LocalVariable.PaintTool_Var.g_NowTestPattern.Desc(), _ResultStatus);
                    break;
            }
            Rect3Result.Clear();
            Cross3Result.Clear();
            lsRect3FingerRange.Clear();
            this.TopLevel = false;
            LocalVariable.PaintTool_Var.m_bTestResult = (_ResultStatus == ResultStatus.PASS);
            LocalVariable.PaintTool_Var.MuHandle = false;

            if(LocalVariable.PaintTool_Var.mutRuningHandle != null)
                LocalVariable.PaintTool_Var.mutRuningHandle.ReleaseMutex();

            this.Hide();
        }

        private void vfSetlabTimeText(string strValue)
        {
            this.Invoke(new Action(() =>
            {
                labTime.Text = strValue;
            }));
        }

        private void vfSetlabHintText(string strValue)
        {
            this.Invoke(new Action(() =>
            {
                labHint.Text = strValue;
                labHint.Location = new Point((m_panDrawing.Width / 2) - (labHint.Width / 2), m_panDrawing.Height / 4 * 3);
            }));
        }

        private void vfSetlabMegText(ErrMsg.SensorTest _Value, params object[] para)
        {
            var strValue = _Value.Desc();
            if(para.Length > 0)
                strValue = string.Format(_Value.Desc(), para);
            labMeg.Text = strValue;
            labMeg.Visible = true;
            if(!LocalVariable.PaintTool_Var.ReturnErrMsg.ToString().Contains(strValue))
            {
                if(StaticVar.bShowLogWithSpace)
                    LocalVariable.PaintTool_Var.ReturnErrMsg.AppendFormat("      {0}\r\n", strValue);
                else
                    LocalVariable.PaintTool_Var.ReturnErrMsg.AppendFormat("{0}\r\n", strValue);
            }

            labMeg.Refresh();
        }

        #region Cursor Shown

        public bool m_CursorShown
        {
            get
            {
                return UI_GBV.fmITS_Tool.CursorShown;
            }
            set
            {
                if(value == UI_GBV.fmITS_Tool.CursorShown)
                    return;

                if(value)
                    ShowCursor();
                else
                    HideCursor();
                UI_GBV.fmITS_Tool.CursorShown = value;
            }
        }

        private void ShowCursor()
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(ShowCursor));
            }
            else
            {
                Cursor.Show();
            }
            Console.WriteLine("====> Cursor.Show()");
        }

        private void HideCursor()
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(HideCursor));
            }
            else
            {
                this.Cursor = Cursors.Default;
                Cursor.Hide();
            }
            Console.WriteLine("====> Cursor.Hide()");
        }

        #endregion Cursor Shown
        public void Painting_PanelReflash(bool bInit_PanHandle, enPaintTestPattern NowTestPattern)
        {
            if(bInit_PanHandle)
                PenHandle_Init();
            SensorTest_ResetPattern(NowTestPattern);
            m_panDrawing.Refresh();
        }

        private void OnPaintEvent(object sender, PaintEventArgs e)
        {
            if(LocalVariable.PaintTool_Var.inTimeOut >= 0 && !LocalVariable.VarSenTest_Multi.m_OnRepaintEvent)
                return;
            isFinishUpdate = false;
            m_panDrawing.Paint -= OnPaintEvent;
            if(LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
            {
                MoveMouse(0, 0);
                m_CursorShown = false;
            }
            switch(LocalVariable.PaintTool_Var.g_NowTestPattern)
            {
                case enPaintTestPattern.Paint_PaintCross2:
                case enPaintTestPattern.Paint_PenCross2:
                    if(LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
                    {
                        Bitmap bitmap = global::ITS_Studio.Properties.Resources.HoverCircle2;
                        this.Cursor = new Cursor(bitmap.GetHicon());
                        m_CursorShown = true;
                        MoveMouse(1, 1);
                    }
                    StaticVar.iRectFingerNum = 1;
                    vfSetlabHintText("");
                    //20240809 Steven : 不能修改，否則OP Mode / Lite OP Mode無法讀取Drawing Test Pattern參數
                    fm_Setting.bReadValueSetting(LocalVariable.PaintTool_Var.g_NowTestPattern);
                    Fill_CrossPatternReset();
                    break;

                case enPaintTestPattern.Paint_PaintRectangle2:
                case enPaintTestPattern.Paint_PenRectangle2:
                    if(LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
                    {
                        Bitmap bitmap = global::ITS_Studio.Properties.Resources.HoverCircle2;
                        this.Cursor = new Cursor(bitmap.GetHicon());
                        m_CursorShown = true;
                        MoveMouse(1, 1);
                    }
                    StaticVar.iRectFingerNum = 1;
                    vfSetlabHintText("");
                    //20240809 Steven : 不能修改，否則OP Mode / Lite OP Mode無法讀取Drawing Test Pattern參數
                    fm_Setting.bReadValueSetting(LocalVariable.PaintTool_Var.g_NowTestPattern);
                    if(LocalVariable.PaintTool_Var._TrajectoryRectangle.CornerReliability == TestReliability.Exact)
                        Fill_RectangleReset();
                    else
                        Fill_RectangleReset_BypassCorner();
                    break;

                case enPaintTestPattern.Paint_PaintRectangle:
                case enPaintTestPattern.Paint_PenRectangle:
                    if(LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PenRectangle)
                        StaticVar.iRectFingerNum = 1;
                    vfSetlabHintText("");
                    RectanglePatternReset(LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse, LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse, LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                    InitLineStatus();
                    isFailOnceTime = false;
                    break;

                case enPaintTestPattern.Paint_PaintCross:
                case enPaintTestPattern.Paint_PenCross:
                    StaticVar.iRectFingerNum = 1;
                    vfSetlabHintText("");
                    CrossPatternReset(LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse, LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse, LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                    InitLineStatus();
                    isFailOnceTime = false;
                    break;

                case enPaintTestPattern.Paint_PaintRectangle3:
                case enPaintTestPattern.Paint_PenRectangle3:
                    if(LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PenRectangle3)
                        StaticVar.iRect3FingerNum = 1;
                    StaticVar.iRectFingerNum = StaticVar.iRect3FingerNum;
                    if(StaticVar.iRectFingerNum == 1)
                        vfSetlabHintText("Please start drawing the line from the blue area.");
                    else
                        vfSetlabHintText("");
                    Rectangle3PatternReset();
                    InitLineStatus();
                    isFailOnceTime = false;
                    break;

                case enPaintTestPattern.Paint_PaintCross3:
                case enPaintTestPattern.Paint_PenCross3:
                    StaticVar.iRectFingerNum = 1;
                    vfSetlabHintText("Please start drawing the line from the blue area.");
                    CrossPatternReset(LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse, LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse, LocalVariable.PaintTool_Var._ComColor.Color_Direction);
                    InitLineStatus();
                    isFailOnceTime = false;
                    break;

                case enPaintTestPattern.Paint_FreeDrawing:
                case enPaintTestPattern.Paint_PenFreeDrawing:
                    StaticVar.iRectFingerNum = 1;
                    if(LocalVariable.PaintTool_Var.bClickResultButton)
                        InitVirtualButton();
                    vfSetlabHintText("");
                    break;

                case enPaintTestPattern.Paint_PaintKey:
                    //Pattern Reset
                    StaticVar.iRectFingerNum = 1;
                    PaintKeyReset();
                    vfSetlabHintText("Please touch the key.");
                                
                    break;

                default:
                case enPaintTestPattern.Paint_FingerDetection:
                    iTestCnts = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inCountDown;
                    vfSetlabHintText("");
                    break;
            }
            m_panDrawing.Paint += OnPaintEvent;
            isFinishUpdate = true;
        }

        #region Rectangle-P2P

        private void Fill_RectangleReset_BypassCorner()
        {
            bool bCheck = false;
            // Drawing 大矩形
            GDI_Panel.AssignPenHandle(m_Pen_Handle[enPaintPenStyle.Rectangle]);
            SolidBrush _BrushesColor = new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_Pass);
            var W_Max = (int)((m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax) * m_ScaleRatio_W) - 1;
            var H_Max = (int)((m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax) * m_ScaleRatio_H) - 1;
            //外框
            DrawRectangle(0, 0, W_Max, H_Max);

            //==========================================================
            int Multiple = (int)(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryRectangle.RangeWidth);//放大倍率
            Blocks = new Dictionary<GraphicsPath, _Block>();
            var _X_CH = m_SensorTestPaint_GBV.Panel_Var.inXChannel;
            var _Y_CH = m_SensorTestPaint_GBV.Panel_Var.inYChannel;

            var W_uint = (float)m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax / (float)_X_CH;
            var H_uint = (float)m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax / (float)_Y_CH;
            int iMultipleCnt = 0;
            var grp = Graphics.FromHwnd(m_panDrawing.Handle);
            int iBlockID = 0;
            for(int iX = 0; iX < _X_CH; iX++)
            {
                for(int iY = 0; iY < _Y_CH; iY++)
                {
                    //面內區域都跳過
                    if((iY != 0) && (iY < _Y_CH - 1))
                        if((iX != 0) && (iX < _X_CH - 1))
                            continue;
                    var lsPoint = new List<PointF>();
                    var itemKey = new GraphicsPath();
                    //Console.Write("({0},{1})", iX, iY);

                    if(iX < Multiple) //左邊條
                    {
                        //Console.WriteLine("左邊條");
                        if(ArrayTool.NumberInRange(iY, (_Y_CH - 1 - Multiple), Multiple))
                        {
                            //Console.WriteLine("[Border] ({0},{1})", iX, iY);
                            itemKey.AddPolygon(
                                new PointF[] {
                                    new PointF((W_uint * iX), (H_uint * iY)),//左上
                                    new PointF((W_uint * (iX + Multiple)), (H_uint * iY)),//右上
                                    new PointF((W_uint * (iX + Multiple)), (H_uint * (iY + 1))),//右下
                                    new PointF((W_uint * iX), (H_uint * (iY + 1)))//左下
                                });
                            //bCheck = true;
                        }
                        else
                        {
                            if(iY == 0)
                            {
                                InsideRectangle[0] = new PointF((W_uint * (0 + Multiple)), (H_uint * (0 + Multiple)));//大內框左上
                                itemKey.AddPolygon(
                                    new PointF[] {
                                        new PointF((W_uint * 0), (H_uint * 0)),//左上
                                        new PointF((W_uint * (0 + Multiple)), (H_uint * 0)),//右上
                                        new PointF((W_uint * (0 + Multiple)), (H_uint * (0 + Multiple))),//右下
                                        new PointF((W_uint * 0), (H_uint * (0 + Multiple)))//左下
                                    });
                                //bCheck = true;
                            }
                            else if(iY == _Y_CH - 1)
                            {
                                InsideRectangle[3] = new PointF((W_uint * (0 + Multiple)), (H_uint * (_Y_CH - Multiple)));//大內框左下
                                itemKey.AddPolygon(
                                    new PointF[] {
                                        new PointF((W_uint * 0), (H_uint * (_Y_CH - Multiple))),//左上
                                        new PointF((W_uint * (0 + Multiple)), (H_uint * (_Y_CH - Multiple))),//右上
                                        new PointF((W_uint * (0 + Multiple)), (H_uint * _Y_CH)),//右下
                                        new PointF((W_uint * 0), (H_uint * _Y_CH))//左下
                                    });
                                //bCheck = true;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else if((iX > Multiple) && (((_X_CH - 1) - iX) < Multiple))//右邊條
                    {
                        //Console.WriteLine("右邊條");
                        if(ArrayTool.NumberInRange(iY, (_Y_CH - 1 - Multiple), Multiple))
                        {
                            itemKey.AddPolygon(
                                new PointF[] {
                                    new PointF((W_uint * (_X_CH - Multiple)), (H_uint * iY)),//左上
                                    new PointF((W_uint * _X_CH), (H_uint * iY)),//右上
                                    new PointF((W_uint * _X_CH), (H_uint * (iY + 1))),//右下
                                    new PointF((W_uint * (_X_CH - Multiple)), (H_uint * (iY + 1)))//左下
                                });
                            //Console.WriteLine("[Border] ({0},{1})", iX, iY);
                            //bCheck = true;
                        }
                        else
                        {
                            if(iY == 0)
                            {
                                InsideRectangle[1] = new PointF((W_uint * (_X_CH - Multiple)), (H_uint * Multiple));//右上
                                itemKey.AddPolygon(
                                    new PointF[] {
                                        new PointF((W_uint * (_X_CH - Multiple)), (H_uint * 0)),//左上
                                        new PointF((W_uint * _X_CH), (H_uint * 0)),//右上
                                        new PointF((W_uint * _X_CH), (H_uint * Multiple)),//右下
                                        new PointF((W_uint * (_X_CH - Multiple)), (H_uint * Multiple))//左下
                                    });
                                //bCheck = true;
                            }
                            else if(iY == _Y_CH - 1)
                            {
                                InsideRectangle[2] = new PointF((W_uint * (_X_CH - Multiple)), (H_uint * (_Y_CH - Multiple)));//右下
                                itemKey.AddPolygon(
                                    new PointF[] {
                                        new PointF((W_uint * (_X_CH - Multiple)), (H_uint * (_Y_CH - Multiple))),//左上
                                        new PointF((W_uint * _X_CH), (H_uint * (_Y_CH - Multiple))),//右上
                                        new PointF((W_uint * _X_CH), (H_uint * _Y_CH)),//右下
                                        new PointF((W_uint * (_X_CH - Multiple)), (H_uint * _Y_CH))//左下
                                    });
                                //bCheck = true;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if(iY == 0)
                        {
                            //Console.WriteLine("上邊條");
                            if(ArrayTool.NumberInRange(iX, (_X_CH - 1 - Multiple), Multiple))
                            {
                                itemKey.AddPolygon(
                                    new PointF[] {
                                        new PointF((W_uint * iX), (H_uint * iY)),//左上
                                        new PointF((W_uint * (iX + 1)), (H_uint * iY)),//右上
                                        new PointF((W_uint * (iX + 1)), (H_uint * (iY + Multiple))),//右下
                                        new PointF((W_uint * iX), (H_uint * (iY + Multiple)))//左下
                                });
                                //bCheck = true;
                            }
                        }
                        else
                        {
                            //Console.WriteLine("下邊條");
                            if(ArrayTool.NumberInRange(iX, (_X_CH - 1 - Multiple), Multiple))
                            {
                                itemKey.AddPolygon(
                                    new PointF[] {
                                    new PointF(W_uint * iX, (H_uint * (_Y_CH - Multiple))),//左上
                                    new PointF(W_uint * (iX + 1), H_uint * (_Y_CH - Multiple)),//右上
                                    new PointF(W_uint * (iX + 1), H_uint * _Y_CH),//右下
                                    new PointF(W_uint * iX, (H_uint * _Y_CH))//左下
                                });
                                //bCheck = true;
                            }
                        }
                        iMultipleCnt = Multiple;
                    }

                    foreach(var p in itemKey.PathPoints)
                        lsPoint.Add(new PointF((int)(p.X * (float)m_ScaleRatio_W + 0.5), (int)(p.Y * (float)m_ScaleRatio_H + 0.5)));

                    _Block tmp_Block = new _Block();
                    tmp_Block.PolygonPath = lsPoint;
                    if(bCheck)
                    {
                        grp.FillPolygon(_BrushesColor, lsPoint.ToArray(), FillMode.Alternate);
                        Font _arialBold = new Font("Consolas", 9.0F, FontStyle.Regular);
                        TextRenderer.DrawText(grp, iBlockID.ToString(), _arialBold, new Point((int)lsPoint[0].X, (int)lsPoint[0].Y), Color.Black, TextFormatFlags.EndEllipsis);
                    }
                    bCheck = false;
                    tmp_Block.bPressed = false;
                    tmp_Block.ID = iBlockID;
                    tmp_Block.iCornerGroup = -1;
                    tmp_Block.bCornerNeighbor = false;
                    Blocks.Add(itemKey, tmp_Block);
                    iBlockID++;
                }
            }
            //內框
            DrawRectangle(
                (int)(InsideRectangle[0].X * m_ScaleRatio_W + 0.5)
                , (int)(InsideRectangle[0].Y * m_ScaleRatio_H + 0.5)
                , (int)((InsideRectangle[2].X) * m_ScaleRatio_W + 0.5)
                , (int)((InsideRectangle[2].Y) * m_ScaleRatio_H + 0.5));

            PressedBlocks = Blocks.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        private void Fill_RectangleReset()
        {
            bool bCheck = false;
            // Drawing 大矩形
            GDI_Panel.AssignPenHandle(m_Pen_Handle[enPaintPenStyle.Rectangle]);
            SolidBrush _BrushesColor = new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_Pass);
            var W_Max = (int)((m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax) * m_ScaleRatio_W) - 1;
            var H_Max = (int)((m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax) * m_ScaleRatio_H) - 1;
            //外框
            DrawRectangle(0, 0, W_Max, H_Max);

            //==========================================================
            //PointF[] OneBlock = new PointF[4];
            int Multiple = (int)(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryRectangle.RangeWidth);//放大倍率
            Blocks = new Dictionary<GraphicsPath, _Block>();
            var _X_CH = m_SensorTestPaint_GBV.Panel_Var.inXChannel;
            var _Y_CH = m_SensorTestPaint_GBV.Panel_Var.inYChannel;

            var W_uint = (float)m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax / (float)_X_CH;
            var H_uint = (float)m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax / (float)_Y_CH;
            int iMultipleCnt = 0;
            var grp = Graphics.FromHwnd(m_panDrawing.Handle);
            int iBlockID = 0;
            for(int iX = 0; iX < _X_CH; iX++)
            {
                for(int iY = 0; iY < _Y_CH; iY++)
                {
                    //面內區域都跳過
                    if((iY != 0) && (iY < _Y_CH - 1))
                        if((iX != 0) && (iX < _X_CH - 1))
                            continue;
                    var lsPoint = new List<PointF>();
                    var itemKey = new GraphicsPath();

                    if(iX < Multiple) //左邊條
                    {
                        //左邊第一塊,左邊最後一塊
                        if(((iX == 0) && (iY == 0)) || ((iX == 0) && (iY == _Y_CH - 1)))
                        {
                            itemKey.AddPolygon(new PointF[] {
                                new PointF((W_uint * iX), (H_uint * iY)),//左上
                                new PointF((W_uint * (iX + 1)), (H_uint * iY)),//右上
                                new PointF((W_uint * (iX + 1)), (H_uint * (iY + 1))),//右下
                                new PointF((W_uint * iX), (H_uint * (iY + 1)))//左下
                            });
                            //bCheck = true;
                        }
                        else
                        {
                            if((iX == 0) && (iY < Multiple))//上左半部
                            {
                                //iMultipleCnt++;
                                iMultipleCnt = iY;
                                itemKey.AddPolygon(new PointF[] {
                                    new PointF(W_uint * iX, H_uint * iY),//左上
                                    new PointF(W_uint * (iX + iMultipleCnt), H_uint * iY),//右上
                                    new PointF(W_uint * (iX + iMultipleCnt + 1), H_uint * (iY + 1)),//右下
                                    new PointF(W_uint * iX, H_uint * (iY + 1))//左下
                                });
                                //bCheck = true;
                            }
                            else if((iX == 0) && ((_Y_CH - 1) - iY < Multiple))//下左半部
                            {
                                iMultipleCnt = (_Y_CH - 1) - iY;
                                itemKey.AddPolygon(new PointF[] {
                                    new PointF(W_uint * iX, H_uint * iY),//左上
                                    new PointF(W_uint * (iX + iMultipleCnt + 1), H_uint * iY),//右上
                                    new PointF(W_uint * (iX + iMultipleCnt), H_uint * (iY + 1)),//右下
                                    new PointF(W_uint * iX, H_uint * (iY + 1))//左下
                                });
                                //bCheck = true;
                            }
                            else if((iX != 0) && (iY < Multiple))//上右半部
                            {
                                iMultipleCnt = iX;
                                itemKey.AddPolygon(new PointF[] {
                                    new PointF(W_uint * iX, H_uint * iY),//左上
                                    new PointF(W_uint * (iX + 1), H_uint * iY),//右上
                                    new PointF(W_uint * (iX + 1), H_uint * (iY + iMultipleCnt + 1)),//右下
                                    new PointF(W_uint * iX, H_uint * (iY + iMultipleCnt))//左下
                                });
                                //bCheck = true;
                            }
                            else if((iX != 0) && ((_Y_CH - 1) - iY < Multiple))//下右半部
                            {
                                iMultipleCnt = iX;
                                itemKey.AddPolygon(new PointF[] {
                                    new PointF(W_uint * iX, H_uint * (iY - iMultipleCnt + 1)),//左上
                                    new PointF(W_uint * (iX + 1), H_uint * (iY - iMultipleCnt)),//右上
                                    new PointF(W_uint * (iX + 1), H_uint * (iY + 1)),//右下
                                    new PointF(W_uint * iX, H_uint * (iY + 1)),//左下
                                });
                                //bCheck = true;
                            }
                            else
                            {
                                itemKey.AddPolygon(new PointF[] {
                                    new PointF(W_uint * iX, H_uint * iY),//左上
                                    new PointF(W_uint * (iX + Multiple), H_uint * iY),//右上
                                    new PointF(W_uint * (iX + Multiple), H_uint * (iY + 1)),//右下
                                    new PointF(W_uint * iX, H_uint * (iY + 1))//左下
                                });
                                //bCheck = true;
                            }
                        }
                        if(iY == Multiple)
                            InsideRectangle[0] = new PointF(W_uint * (iX + Multiple), H_uint * iY);//左上
                        if((_Y_CH - 1 - iY == Multiple))
                            InsideRectangle[3] = new PointF(W_uint * (iX + Multiple), H_uint * (iY + Multiple));//左下
                    }
                    else if((iX > Multiple) && (((_X_CH - 1) - iX) < Multiple))//右邊條
                    {
                        if(((iX == _X_CH - 1) && (iY == 0))//右邊第一塊
                            || ((iX == _X_CH - 1) && (iY == _Y_CH - 1)))//右邊最後一塊
                        {
                            itemKey.AddPolygon(new PointF[] {
                                new PointF(W_uint * iX, H_uint * iY),//左上
                                new PointF(W_uint * (iX + 1), H_uint * iY),//右上
                                new PointF(W_uint * (iX + 1), H_uint * (iY + 1)),//右下
                                new PointF(W_uint * iX, H_uint * (iY + 1))//左下
                            });
                            //bCheck = true;
                        }
                        else
                        {
                            if((iX != _X_CH - 1) && (iY < Multiple))//上左半部
                            {
                                itemKey.AddPolygon(new PointF[] {
                                    new PointF(W_uint * iX, H_uint * iY),//左上
                                    new PointF(W_uint * (iX + 1), H_uint * iY),//右上
                                    new PointF(W_uint * (iX + 1), H_uint * (iY + iMultipleCnt - 1)),//右下
                                    new PointF(W_uint * iX, H_uint * (iY + iMultipleCnt))//左下
                                });
                                iMultipleCnt--;
                                //bCheck = true;
                            }
                            else if((iX != _X_CH - 1) && ((_Y_CH - 1) - iY < Multiple))//下左半部
                            {
                                itemKey.AddPolygon(new PointF[] {
                                    new PointF(W_uint * iX, H_uint * (iY - iMultipleCnt)),//左上
                                    new PointF(W_uint * (iX + 1), H_uint * (iY - iMultipleCnt + 1)),//右上
                                    new PointF(W_uint * (iX + 1), H_uint * _Y_CH),//右下
                                    new PointF(W_uint * iX, H_uint * _Y_CH)//左下
                                });
                                //bCheck = true;
                            }
                            else if((iX == _X_CH - 1) && (iY < Multiple))//上右半部
                            {
                                itemKey.AddPolygon(new PointF[] {
                                    new PointF(W_uint * (iX - iMultipleCnt + 1), H_uint * iY),//左上
                                    new PointF(W_uint * _X_CH, H_uint * iY),//右上
                                    new PointF(W_uint * _X_CH, H_uint * (iY + 1)),//右下
                                    new PointF(W_uint * (iX - iMultipleCnt), H_uint * (iY + 1))//左下
                                });
                                iMultipleCnt++;
                                //bCheck = true;
                            }
                            else if((iX == _X_CH - 1) && ((_Y_CH - 1) - iY < Multiple))//下右半部
                            {
                                itemKey.AddPolygon(new PointF[] {
                                    new PointF(W_uint * (iX - iMultipleCnt + 1), H_uint * iY),//左上
                                    new PointF(W_uint * _X_CH, H_uint * iY),//右上
                                    new PointF(W_uint * _X_CH, H_uint * (iY + 1)),//右下
                                    new PointF(W_uint * (iX - iMultipleCnt + 2), H_uint * (iY + 1))//左下
                                });
                                iMultipleCnt--;
                                //bCheck = true;
                            }
                            else
                            {
                                itemKey.AddPolygon(new PointF[] {
                                    new PointF(W_uint * (_X_CH - Multiple), H_uint * iY),//左上
                                    new PointF(W_uint * _X_CH, H_uint * iY),//右上
                                    new PointF(W_uint * _X_CH, H_uint * (iY + 1)),//右下
                                    new PointF(W_uint * (_X_CH - Multiple), H_uint * (iY + 1))//左下
                                });
                                //bCheck = true;
                            }
                        }

                        if(iY == Multiple)
                            InsideRectangle[1] = new PointF(W_uint * (_X_CH - Multiple), H_uint * Multiple);//右上

                        if((_Y_CH - 1 - iY == Multiple))
                            InsideRectangle[2] = new PointF(W_uint * (_X_CH - Multiple), H_uint * (_Y_CH - Multiple));//右下
                    }
                    else
                    {
                        if(iY == 0)
                        {
                            //bCheck = true;
                            itemKey.AddPolygon(new PointF[] {
                                new PointF(W_uint * iX, H_uint * iY),//左上
                                new PointF(W_uint * (iX + 1), H_uint * iY),//右上
                                new PointF(W_uint * (iX + 1), H_uint * (iY + Multiple)),//右下
                                new PointF(W_uint * iX, H_uint * (iY + Multiple))//左下
                            });
                        }
                        else
                        {
                            //bCheck = true;
                            itemKey.AddPolygon(new PointF[] {
                                new PointF(W_uint * iX, H_uint * (_Y_CH - Multiple)),//左上
                                new PointF(W_uint * (iX + 1), H_uint * (_Y_CH - Multiple)),//右上
                                new PointF(W_uint * (iX + 1), H_uint * _Y_CH),//右下
                                new PointF(W_uint * iX, H_uint * _Y_CH)//左下
                            });
                        }
                        iMultipleCnt = Multiple;
                    }

                    foreach(var p in itemKey.PathPoints)
                        lsPoint.Add(new PointF((int)(p.X * (float)m_ScaleRatio_W + 0.5), (int)(p.Y * (float)m_ScaleRatio_H + 0.5)));
                    //lsPoint.Add(new PointF(p.X * (float)m_ScaleRatio_W, p.Y * (float)m_ScaleRatio_H));

                    //Console.WriteLine("lsPoint:{0}", string.Join(",", lsPoint));
                    _Block tmp_Block = new _Block();
                    tmp_Block.PolygonPath = lsPoint;
                    if(bCheck)
                        grp.FillPolygon(_BrushesColor, lsPoint.ToArray(), FillMode.Alternate);

                    bCheck = false;
                    tmp_Block.bPressed = false;
                    tmp_Block.ID = iBlockID;
                    //目前這一段寫死只有隔壁一塊
                    if((iX == 0 && iY == 0) || (iX == 1 && iY == 0) || (iX == 0 && iY == 1))
                    {
                        tmp_Block.iCornerGroup = 0;
                        tmp_Block.bCornerNeighbor = (iX == 0 && iY == 0) ? false : true;
                    }
                    else if((iX == 0 && iY == _Y_CH - 1) || (iX == 0 && iY == _Y_CH - 2) || (iX == 1 && iY == _Y_CH - 1))
                    {
                        tmp_Block.iCornerGroup = 1;
                        tmp_Block.bCornerNeighbor = (iX == 0 && iY == _Y_CH - 1) ? false : true;
                    }
                    else if((iX == _X_CH - 1 && iY == 0) || (iX == _X_CH - 2 && iY == 0) || (iX == _X_CH - 1 && iY == 1))
                    {
                        tmp_Block.iCornerGroup = 2;
                        tmp_Block.bCornerNeighbor = (iX == _X_CH - 1 && iY == 0) ? false : true;
                    }
                    else if((iX == _X_CH - 1 && iY == _Y_CH - 1) || (iX == _X_CH - 1 && iY == _Y_CH - 2) || (iX == _X_CH - 2 && iY == _Y_CH - 1))
                    {
                        tmp_Block.iCornerGroup = 3;
                        tmp_Block.bCornerNeighbor = (iX == _X_CH - 1 && iY == _Y_CH - 1) ? false : true;
                    }
                    else
                    {
                        tmp_Block.iCornerGroup = -1;
                        tmp_Block.bCornerNeighbor = false;
                    }
                    Blocks.Add(itemKey, tmp_Block);
                    iBlockID++;
                }
            }
            //內框
            DrawRectangle((int)(InsideRectangle[0].X * m_ScaleRatio_W + 0.5), (int)(InsideRectangle[0].Y * m_ScaleRatio_H + 0.5)
                , (int)((InsideRectangle[2].X) * m_ScaleRatio_W + 0.5)
                , (int)((InsideRectangle[2].Y) * m_ScaleRatio_H + 0.5));

            PressedBlocks = Blocks.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        private void Rectangle_PointToPoint(LineSetting_Rectangle Line, int iBuffIndex, Color _Color_StartEllipse, Color _Color_EndEllipse, Color _Direction)
        {
            IntPtr BrushHandle = IntPtr.Zero;
            IntPtr BrushHandle_2 = IntPtr.Zero;
            int linegap = StaticVar.iLineGap;
            GDI_Panel.AssignPenHandle(m_Pen_Handle[enPaintPenStyle.Rectangle]);
            // Drawing 大矩形
            DrawRectangle((int)(Line.iXCoordinate * m_ScaleRatio_W)
                , (int)(Line.iYCoordinate * m_ScaleRatio_H)
                , (int)((Line.iXCoordinate + Line.iWidth) * m_ScaleRatio_W)
                , (int)((Line.iYCoordinate + Line.iHeight) * m_ScaleRatio_H));

            DrawSecondRectangle((LineSetting_Rectangle)Line, linegap);
            bool isNewPattern = (LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PaintRectangle3
                || LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PenRectangle3);

            if(isNewPattern)
                _Color_EndEllipse = _Color_StartEllipse;
            //====================================================================================
            // 畫起始點顏色=Color_StartEllipse
            switch(Line.DirectionRule)
            {
                case enRectangleDirection.LeftToRight:
                case enRectangleDirection.TopToBottom:
                    BrushHandle = GDI_Panel.CreateBrushHandle(_Color_StartEllipse);
                    BrushHandle_2 = GDI_Panel.CreateBrushHandle(_Color_StartEllipse);
                    break;

                case enRectangleDirection.RightToLeft:
                case enRectangleDirection.BottomToTop:
                    BrushHandle = GDI_Panel.CreateBrushHandle(_Color_EndEllipse);
                    BrushHandle_2 = GDI_Panel.CreateBrushHandle(_Color_EndEllipse);
                    break;
            }
            int iDiameter = (Line.iHeight < Line.iWidth) ? Line.iHeight : Line.iWidth;
            GDI_Panel.AssignPenHandle(BrushHandle);
            GDI_Panel.Ellipse((int)(Line.iXCoordinate * m_ScaleRatio_W),
                                (int)(Line.iYCoordinate * m_ScaleRatio_H),
                                (int)((Line.iXCoordinate + iDiameter) * m_ScaleRatio_W),
                                (int)((Line.iYCoordinate + iDiameter) * m_ScaleRatio_H));
            GDI_Panel.DeleteOBJECT(BrushHandle);
            DrawStartEllipse(Line, linegap, iDiameter, BrushHandle_2);

            //====================================================================================
            // 畫結束點顏色=Color_EndEllipse
            switch(Line.DirectionRule)
            {
                case enRectangleDirection.LeftToRight:
                case enRectangleDirection.TopToBottom:
                    BrushHandle = GDI_Panel.CreateBrushHandle(_Color_EndEllipse);
                    BrushHandle_2 = GDI_Panel.CreateBrushHandle(_Color_EndEllipse);
                    break;

                case enRectangleDirection.RightToLeft:
                case enRectangleDirection.BottomToTop:
                    BrushHandle = GDI_Panel.CreateBrushHandle(_Color_StartEllipse);
                    BrushHandle_2 = GDI_Panel.CreateBrushHandle(_Color_StartEllipse);
                    break;
            }

            GDI_Panel.AssignPenHandle(BrushHandle);
            GDI_Panel.Ellipse((int)((Line.iXCoordinate + Line.iWidth - iDiameter) * m_ScaleRatio_W),
                                (int)((Line.iYCoordinate + Line.iHeight - iDiameter) * m_ScaleRatio_H),
                                (int)((Line.iXCoordinate + Line.iWidth) * m_ScaleRatio_W),
                                (int)((Line.iYCoordinate + Line.iHeight) * m_ScaleRatio_H));
            GDI_Panel.DeleteOBJECT(BrushHandle);
            DrawEndEllipse(Line, linegap, iDiameter, BrushHandle_2);

            //====================================================================================
            // Drawing Direction
            NowTestLineStatus[iBuffIndex].dRadius = (double)iDiameter / 2;//半徑
            switch(Line.DirectionRule)
            {
                case enRectangleDirection.LeftToRight:
                case enRectangleDirection.TopToBottom:
                    NowTestLineStatus[iBuffIndex].Start.X = (int)((double)Line.iXCoordinate + NowTestLineStatus[iBuffIndex].dRadius);
                    NowTestLineStatus[iBuffIndex].Start.Y = (int)((double)Line.iYCoordinate + NowTestLineStatus[iBuffIndex].dRadius);

                    NowTestLineStatus[iBuffIndex].End.X = (int)(((double)(Line.iXCoordinate + Line.iWidth) - NowTestLineStatus[iBuffIndex].dRadius));
                    NowTestLineStatus[iBuffIndex].End.Y = (int)(((double)(Line.iYCoordinate + Line.iHeight) - NowTestLineStatus[iBuffIndex].dRadius));
                    break;

                case enRectangleDirection.RightToLeft:
                case enRectangleDirection.BottomToTop:
                    NowTestLineStatus[iBuffIndex].Start.X = (int)(((double)(Line.iXCoordinate + Line.iWidth) - NowTestLineStatus[iBuffIndex].dRadius));
                    NowTestLineStatus[iBuffIndex].Start.Y = (int)(((double)(Line.iYCoordinate + Line.iHeight) - NowTestLineStatus[iBuffIndex].dRadius));

                    NowTestLineStatus[iBuffIndex].End.X = (int)((double)Line.iXCoordinate + NowTestLineStatus[iBuffIndex].dRadius);
                    NowTestLineStatus[iBuffIndex].End.Y = (int)((double)Line.iYCoordinate + NowTestLineStatus[iBuffIndex].dRadius);
                    break;
            }
            if(!isNewPattern)
            {
                DrawDirectionLine((int)(NowTestLineStatus[iBuffIndex].Start.X * m_ScaleRatio_W),
                       (int)(NowTestLineStatus[iBuffIndex].Start.Y * m_ScaleRatio_H),
                       (int)(NowTestLineStatus[iBuffIndex].End.X * m_ScaleRatio_W),
                       (int)(NowTestLineStatus[iBuffIndex].End.Y * m_ScaleRatio_H),
                       enPaintTestPattern.Paint_PaintRectangle,
                       _Direction);
            }
            else
            {
                DrawDirectionLine_WithoutArrow((int)(NowTestLineStatus[iBuffIndex].Start.X * m_ScaleRatio_W),
                     (int)(NowTestLineStatus[iBuffIndex].Start.Y * m_ScaleRatio_H),
                     (int)(NowTestLineStatus[iBuffIndex].End.X * m_ScaleRatio_W),
                     (int)(NowTestLineStatus[iBuffIndex].End.Y * m_ScaleRatio_H),
                     enPaintTestPattern.Paint_PaintRectangle,
                     _Direction);
            }

            DrawDirectSecondLine(Line, linegap, iDiameter);
        }

        private void Rectangle3_PointToPoint(LineSetting_Rectangle3 Line, int iBuffIndex, Color _Color_StartEllipse)
        {
            IntPtr BrushHandle = IntPtr.Zero;
            IntPtr BrushHandle_2 = IntPtr.Zero;
            int linegap = StaticVar.iLineGap;
            GDI_Panel.AssignPenHandle(m_Pen_Handle[enPaintPenStyle.Rectangle]);
            // Drawing 大矩形
            DrawRectangle((int)(Line.iXCoordinate * m_ScaleRatio_W)
                , (int)(Line.iYCoordinate * m_ScaleRatio_H)
                , (int)((Line.iXCoordinate + Line.iWidth) * m_ScaleRatio_W)
                , (int)((Line.iYCoordinate + Line.iHeight) * m_ScaleRatio_H));

            DrawSecondRectangle3((LineSetting_Rectangle3)Line, 0);

            //====================================================================================
            // 畫起始點顏色=Color_StartEllipse
            BrushHandle = GDI_Panel.CreateBrushHandle(_Color_StartEllipse);
            Graphics grp = this.m_panDrawing.CreateGraphics();
            switch(Line.DirectionRule)
            {
                case enRectangleDirection.LeftToRight:
                case enRectangleDirection.TopToBottom:
                    grp.FillRectangle(new SolidBrush(_Color_StartEllipse),
                        (float)(Math.Round(Line.iXCoordinate * m_ScaleRatio_W, 2)),
                        (float)(Math.Round(Line.iYCoordinate * m_ScaleRatio_H, 2)),
                        (float)(Math.Round(Line.iBlockWidth * m_ScaleRatio_W, 2)),
                        (float)(Math.Round(Line.iBlockHeight * m_ScaleRatio_H, 2)));

                    break;

                case enRectangleDirection.RightToLeft:
                case enRectangleDirection.BottomToTop:
                    grp.FillRectangle(new SolidBrush(_Color_StartEllipse),
                       (float)(Math.Round((Line.iXCoordinate + Line.iWidth - Line.iBlockWidth) * m_ScaleRatio_W, 2)),
                       (float)(Math.Round((Line.iYCoordinate + Line.iHeight - Line.iBlockHeight) * m_ScaleRatio_H, 2)),
                       (float)(Math.Round(Line.iBlockWidth * m_ScaleRatio_W, 2)),
                       (float)(Math.Round(Line.iBlockHeight * m_ScaleRatio_H, 2)));
                    break;
            }
        }

        private void Rectangle3_PointToPoint(LineSetting_Rectangle3 Line, int iBuffIndex)
        {
            IntPtr BrushHandle = IntPtr.Zero;
            IntPtr BrushHandle_2 = IntPtr.Zero;
            _Rectangle3_Struct rect3Struct = new _Rectangle3_Struct();
            int linegap = StaticVar.iLineGap;
            GDI_Panel.AssignPenHandle(m_Pen_Handle[enPaintPenStyle.Rectangle]);
            // Drawing 大矩形
            if(StaticVar.iRectFingerNum == 1)
            {
                DrawRectangle((int)(Line.iXCoordinate * m_ScaleRatio_W)
                   , (int)(Line.iYCoordinate * m_ScaleRatio_H)
                   , (int)((Line.iXCoordinate + Line.iWidth) * m_ScaleRatio_W)
                   , (int)((Line.iYCoordinate + Line.iHeight) * m_ScaleRatio_H));
            }
            else
            {
                DrawRectangleWithMiddleLine((int)(Line.iXCoordinate * m_ScaleRatio_W)
                     , (int)(Line.iYCoordinate * m_ScaleRatio_H)
                     , (int)((Line.iXCoordinate + Line.iWidth) * m_ScaleRatio_W)
                     , (int)((Line.iYCoordinate + Line.iHeight) * m_ScaleRatio_H));
            }

            DrawSecondRectangle3((LineSetting_Rectangle3)Line, 0);
            int iDiameter = (Line.iHeight < Line.iWidth) ? Line.iHeight : Line.iWidth;
            //====================================================================================
            // Drawing Direction
            NowTestLineStatus[iBuffIndex].dRadius = 0;//半徑
            NowTestLineStatus[iBuffIndex].dWidth = Line.iBlockWidth;
            NowTestLineStatus[iBuffIndex].dHeight = Line.iBlockHeight;

            switch(Line.DirectionRule)
            {
                case enRectangleDirection.LeftToRight:
                case enRectangleDirection.TopToBottom:
                    NowTestLineStatus[iBuffIndex].Start.X = (int)((double)Line.iXCoordinate + (NowTestLineStatus[iBuffIndex].dWidth / 2));
                    NowTestLineStatus[iBuffIndex].Start.Y = (int)((double)Line.iYCoordinate + (NowTestLineStatus[iBuffIndex].dHeight / 2));

                    NowTestLineStatus[iBuffIndex].End.X = (int)(((double)(Line.iXCoordinate + Line.iWidth) - NowTestLineStatus[iBuffIndex].dWidth / 2));
                    NowTestLineStatus[iBuffIndex].End.Y = (int)(((double)(Line.iYCoordinate + Line.iHeight) - NowTestLineStatus[iBuffIndex].dHeight / 2));

                    if(Line.DirectionRule == enRectangleDirection.LeftToRight)
                    {
                        if(StaticVar.iRectFingerNum == 1)
                        {
                            rect3Struct.rect = new Rectangle((int)(Math.Round((Line.iXCoordinate + Line.iBlockWidth) * m_ScaleRatio_W)),
                             (int)(Math.Round(Line.iYCoordinate * m_ScaleRatio_H)),
                             (int)(Math.Round((Line.iWidth - (2 * Line.iBlockWidth)) * m_ScaleRatio_W)),
                             (int)(Math.Round(Line.iBlockHeight * m_ScaleRatio_H)));
                        }
                        else
                        {
                            rect3Struct.rect = new Rectangle((int)(Math.Round((Line.iXCoordinate + 2 * Line.iBlockWidth) * m_ScaleRatio_W)),
                               (int)(Math.Round(Line.iYCoordinate * m_ScaleRatio_H)),
                               (int)(Math.Round((Line.iWidth - (4 * Line.iBlockWidth)) * m_ScaleRatio_W)),
                               (int)(Math.Round(2 * Line.iBlockHeight * m_ScaleRatio_H)));
                        }
                    }
                    else
                    {
                        if(StaticVar.iRectFingerNum == 1)
                        {
                            rect3Struct.rect = new Rectangle((int)(Math.Round((Line.iXCoordinate) * m_ScaleRatio_W)),
                                 (int)(Math.Round((Line.iYCoordinate + Line.iBlockHeight) * m_ScaleRatio_H)),
                                 (int)(Math.Round(Line.iBlockWidth * m_ScaleRatio_W)),
                                 (int)(Math.Round((Line.iHeight - (2 * Line.iBlockHeight)) * m_ScaleRatio_H)));
                        }
                        else
                        {
                            rect3Struct.rect = new Rectangle((int)(Math.Round((Line.iXCoordinate - Line.iBlockWidth) * m_ScaleRatio_W)),
                             (int)(Math.Round((Line.iYCoordinate + (2 * Line.iBlockHeight)) * m_ScaleRatio_H)),
                             (int)(Math.Round(2 * Line.iBlockWidth * m_ScaleRatio_W)),
                             (int)(Math.Round((Line.iHeight - (4 * Line.iBlockHeight)) * m_ScaleRatio_H)));
                        }
                    }
                    break;

                case enRectangleDirection.RightToLeft:
                case enRectangleDirection.BottomToTop:
                    NowTestLineStatus[iBuffIndex].Start.X = (int)(((double)(Line.iXCoordinate + Line.iWidth) - NowTestLineStatus[iBuffIndex].dWidth / 2));
                    NowTestLineStatus[iBuffIndex].Start.Y = (int)(((double)(Line.iYCoordinate + Line.iHeight) - NowTestLineStatus[iBuffIndex].dHeight / 2));

                    NowTestLineStatus[iBuffIndex].End.X = (int)((double)Line.iXCoordinate + NowTestLineStatus[iBuffIndex].dWidth / 2);
                    NowTestLineStatus[iBuffIndex].End.Y = (int)((double)Line.iYCoordinate + NowTestLineStatus[iBuffIndex].dHeight / 2);
                    if(Line.DirectionRule == enRectangleDirection.RightToLeft)
                    {
                        if(StaticVar.iRectFingerNum == 1)
                        {
                            rect3Struct.rect = new Rectangle(
                          (int)(Math.Round((Line.iXCoordinate + Line.iBlockWidth) * m_ScaleRatio_W)),
                          (int)(Math.Round((Line.iYCoordinate + Line.iHeight - Line.iBlockHeight) * m_ScaleRatio_H)),
                          (int)(Math.Round((Line.iWidth - (2 * Line.iBlockWidth)) * m_ScaleRatio_W)),
                          (int)(Math.Round(Line.iBlockHeight * m_ScaleRatio_H)));
                        }
                        else
                        {
                            rect3Struct.rect = new Rectangle(
                          (int)(Math.Round((Line.iXCoordinate + (2 * Line.iBlockWidth)) * m_ScaleRatio_W)),
                          (int)(Math.Round((Line.iYCoordinate + Line.iHeight - 2 * Line.iBlockHeight) * m_ScaleRatio_H)),
                          (int)(Math.Round((Line.iWidth - (4 * Line.iBlockWidth)) * m_ScaleRatio_W)),
                          (int)(Math.Round(2 * Line.iBlockHeight * m_ScaleRatio_H)));
                        }
                    }
                    else
                    {
                        if(StaticVar.iRectFingerNum == 1)
                        {
                            rect3Struct.rect = new Rectangle(
                         (int)(Math.Round((Line.iXCoordinate) * m_ScaleRatio_W)),
                         (int)(Math.Round((Line.iYCoordinate + Line.iBlockHeight) * m_ScaleRatio_H)),
                         (int)(Math.Round(Line.iBlockWidth * m_ScaleRatio_W)),
                         (int)(Math.Round((Line.iHeight - (2 * Line.iBlockHeight)) * m_ScaleRatio_H)));
                        }
                        else
                        {
                            rect3Struct.rect = new Rectangle(
                           (int)(Math.Round((Line.iXCoordinate) * m_ScaleRatio_W)),
                           (int)(Math.Round((Line.iYCoordinate + (2 * Line.iBlockHeight)) * m_ScaleRatio_H)),
                           (int)(Math.Round(2 * Line.iBlockWidth * m_ScaleRatio_W)),
                           (int)(Math.Round((Line.iHeight - (4 * Line.iBlockHeight)) * m_ScaleRatio_H)));
                        }
                    }
                    break;
            }
            AllRectSection[iBuffIndex] = new Rectangle((NowTestLineStatus[iBuffIndex].Start.X - (int)NowTestLineStatus[iBuffIndex].dWidth / 2)
                                                       , NowTestLineStatus[iBuffIndex].Start.Y - (int)NowTestLineStatus[iBuffIndex].dHeight / 2
                                                       , (int)NowTestLineStatus[iBuffIndex].dWidth
                                                       , (int)NowTestLineStatus[iBuffIndex].dHeight);
            DrawDirectSecondLineForRectangle3(iBuffIndex, Line, 0, iDiameter);

            rect3Struct.result = ResultStatus.Continue;
            if(Rect3Result.Count < 4)
            {
                Rect3Result.Add(iBuffIndex, rect3Struct);
            }
            else if(Rect3Result[iBuffIndex].rect != rect3Struct.rect)
            {
                Rect3Result[iBuffIndex].rect = rect3Struct.rect;
            }
            else
            {
                foreach(var item in Rect3Result)
                {
                    if(item.Value.result == ResultStatus.PASS)
                    {
                        Graphics grp = m_panDrawing.CreateGraphics();
                        grp.FillRectangle(new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_Pass), item.Value.rect);
                    }
                }
            }
        }

        private void RectanglePatternReset(Color _Color_StartEllipse, Color _Color_EndEllipse, Color _Direction)
        {
            int iBuffIndex = 0;
            foreach(var Line in LocalVariable.PaintTool_Var.lsRectangleLineSetting)
                Rectangle_PointToPoint(Line, iBuffIndex++, _Color_StartEllipse, _Color_EndEllipse, _Direction);
        }

        private void Rectangle3PatternReset()
        {
            if(Rect3Result.Count == 0)
                Rect3Result.Clear();

            int iBuffIndex = 0;
            //Rectangle3_PointToPoint(LocalVariable.PaintTool_Var.lsRectangle3LineSetting);
            foreach(var Line in LocalVariable.PaintTool_Var.lsRectangle3LineSetting)
                Rectangle3_PointToPoint(Line, iBuffIndex++);

            if(StaticVar.iRectFingerNum == 2)
            {
                AllRectSection[8] = new Rectangle(NowTestLineStatus[7].End.X - (int)NowTestLineStatus[7].dWidth / 2
                                                     , NowTestLineStatus[7].End.Y - (int)NowTestLineStatus[7].dHeight / 2
                                                     , (int)NowTestLineStatus[7].dWidth
                                                     , (int)NowTestLineStatus[7].dHeight);

                NowTestLineStatus[8].dRadius = NowTestLineStatus[7].dRadius;//半徑
                NowTestLineStatus[8].dWidth = NowTestLineStatus[7].dWidth;
                NowTestLineStatus[8].dHeight = NowTestLineStatus[7].dHeight;
                NowTestLineStatus[8].Start.X = NowTestLineStatus[7].End.X;
                NowTestLineStatus[8].Start.Y = NowTestLineStatus[7].End.Y;
                NowTestLineStatus[8].End.X = NowTestLineStatus[7].Start.X;
                NowTestLineStatus[8].End.Y = NowTestLineStatus[7].Start.Y;

                AllRectSection[9] = new Rectangle(NowTestLineStatus[4].End.X - (int)NowTestLineStatus[4].dWidth / 2
                                     , NowTestLineStatus[4].End.Y - (int)NowTestLineStatus[4].dHeight / 2
                                     , (int)NowTestLineStatus[4].dWidth
                                     , (int)NowTestLineStatus[4].dHeight);

                NowTestLineStatus[9].dRadius = NowTestLineStatus[4].dRadius;//半徑
                NowTestLineStatus[9].dWidth = NowTestLineStatus[4].dWidth;
                NowTestLineStatus[9].dHeight = NowTestLineStatus[4].dHeight;
                NowTestLineStatus[9].Start.X = NowTestLineStatus[4].End.X;
                NowTestLineStatus[9].Start.Y = NowTestLineStatus[4].End.Y;
                NowTestLineStatus[9].End.X = NowTestLineStatus[4].Start.X;
                NowTestLineStatus[9].End.Y = NowTestLineStatus[4].Start.Y;

                AllRectSection[10] = new Rectangle(NowTestLineStatus[5].End.X - (int)NowTestLineStatus[5].dWidth / 2
                                     , NowTestLineStatus[5].End.Y - (int)NowTestLineStatus[5].dHeight / 2
                                     , (int)NowTestLineStatus[5].dWidth
                                     , (int)NowTestLineStatus[5].dHeight);

                NowTestLineStatus[10].dRadius = NowTestLineStatus[5].dRadius;//半徑
                NowTestLineStatus[10].dWidth = NowTestLineStatus[5].dWidth;
                NowTestLineStatus[10].dHeight = NowTestLineStatus[5].dHeight;
                NowTestLineStatus[10].Start.X = NowTestLineStatus[5].End.X;
                NowTestLineStatus[10].Start.Y = NowTestLineStatus[5].End.Y;
                NowTestLineStatus[10].End.X = NowTestLineStatus[5].Start.X;
                NowTestLineStatus[10].End.Y = NowTestLineStatus[5].Start.Y;

                AllRectSection[11] = new Rectangle(NowTestLineStatus[6].End.X - (int)NowTestLineStatus[6].dWidth / 2
                                     , NowTestLineStatus[6].End.Y - (int)NowTestLineStatus[6].dHeight / 2
                                     , (int)NowTestLineStatus[6].dWidth
                                     , (int)NowTestLineStatus[6].dHeight);

                NowTestLineStatus[11].dRadius = NowTestLineStatus[6].dRadius;//半徑
                NowTestLineStatus[11].dWidth = NowTestLineStatus[6].dWidth;
                NowTestLineStatus[11].dHeight = NowTestLineStatus[6].dHeight;
                NowTestLineStatus[11].Start.X = NowTestLineStatus[6].End.X;
                NowTestLineStatus[11].Start.Y = NowTestLineStatus[6].End.Y;
                NowTestLineStatus[11].End.X = NowTestLineStatus[6].Start.X;
                NowTestLineStatus[11].End.Y = NowTestLineStatus[6].Start.Y;
            }
            else
            {
                for(int i = 0; i < NowTestLineStatus.Length; i++)
                {
                    Graphics grp = m_panDrawing.CreateGraphics();
                    grp.FillRectangle(new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse),
                                                   (int)((NowTestLineStatus[i].Start.X - NowTestLineStatus[i].dWidth / 2) * m_ScaleRatio_W),
                                                   (int)((NowTestLineStatus[i].Start.Y - NowTestLineStatus[i].dHeight / 2) * m_ScaleRatio_H),
                                                   (int)(NowTestLineStatus[i].dWidth * m_ScaleRatio_W),
                                                   (int)(NowTestLineStatus[i].dHeight * m_ScaleRatio_H));
                }
            }
        }

        private void Rectangle3PatternReset(int index, Color _Color_StartEllipse)
        {
            int iBuffIndex = 0;
            var Line = LocalVariable.PaintTool_Var.lsRectangle3LineSetting[index];

            Rectangle3_PointToPoint(Line, iBuffIndex++, _Color_StartEllipse);
        }

        private void Rectangle3PatternResetByNowTestLineStatus(int index, Color _Color_StartEllipse)
        {
            int iBuffIndex = 0;
            var Line = LocalVariable.PaintTool_Var.lsRectangle3LineSetting[index];

            Rectangle3_PointToPoint(Line, iBuffIndex++, _Color_StartEllipse);
        }

        private void Rectangle3PathResult(int index, Color _Color_StartEllipse)
        {
            Graphics grp = m_panDrawing.CreateGraphics();
            if(Rect3Result.Count == 0)
                return;
            grp.FillRectangle(new SolidBrush(_Color_StartEllipse), Rect3Result[index].rect);
            Rect3Result[index].result = ResultStatus.PASS;
        }

        private void CrossPathResult(int index, Color _Color_StartEllipse)
        {
            Graphics grp = m_panDrawing.CreateGraphics();
            if(Cross3Result.Count == 0)
                return;
            grp.FillPolygon(new SolidBrush(_Color_StartEllipse), Cross3Result[index].grpicPath.PathPoints, FillMode.Alternate);
            Cross3Result[index].result = ResultStatus.PASS;
        }

        private void RectangleNodeResult(int index, Color _Color_StartEllipse)
        {
            Graphics grp = m_panDrawing.CreateGraphics();
            stuLineTest item = NowTestLineStatus[index];
            grp.FillRectangle(new SolidBrush(_Color_StartEllipse), new Rectangle((int)((item.Start.X - item.dWidth / 2) * m_ScaleRatio_W)
                , (int)((item.Start.Y - item.dHeight / 2) * m_ScaleRatio_H)
                , (int)(item.dWidth * m_ScaleRatio_W)
                , (int)(item.dHeight * m_ScaleRatio_H)));
        }

        private void RectangleAllNodeResult(Color _Color_StartEllipse)
        {
            Graphics grp = m_panDrawing.CreateGraphics();
            foreach(var item in NowTestLineStatus)
            {
                grp.FillRectangle(new SolidBrush(_Color_StartEllipse), new Rectangle((int)((item.Start.X - item.dWidth / 2) * m_ScaleRatio_W)
                    , (int)((item.Start.Y - item.dHeight / 2) * m_ScaleRatio_H)
                    , (int)(item.dWidth * m_ScaleRatio_W)
                    , (int)(item.dHeight * m_ScaleRatio_H)));
            }
        }

        private void DrawStartEllipse(LineSetting_Rectangle Line, int linegap, int iDiameter, IntPtr BrushHandle1)
        {
            if(StaticVar.iRectFingerNum == 2 && LocalVariable.PaintTool_Var.lsRectangleLineSetting.Count != 4)
            {
                GDI_Panel.AssignPenHandle(BrushHandle1);
                int x1 = Line.iXCoordinate, y1 = Line.iYCoordinate, x2 = Line.iXCoordinate + iDiameter, y2 = Line.iYCoordinate + iDiameter;
                if((y2 + linegap + Line.iHeight) >= m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax)
                {
                    if(Line.DirectionRule == enRectangleDirection.LeftToRight
                           || Line.DirectionRule == enRectangleDirection.RightToLeft)
                    {
                        y1 -= (Line.iHeight + linegap);
                        y2 -= (Line.iHeight + linegap);
                    }
                }
                else
                {
                    y1 += (Line.iHeight + linegap);
                    y2 += (Line.iHeight + linegap);
                }
                if((x2 + linegap + Line.iWidth) >= m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax)
                {
                    if(Line.DirectionRule == enRectangleDirection.BottomToTop
                        || Line.DirectionRule == enRectangleDirection.TopToBottom)
                    {
                        x1 -= (Line.iWidth + linegap);
                        x2 -= (Line.iWidth + linegap);
                    }
                }
                else
                {
                    x1 += (Line.iWidth + linegap);
                    x2 += (Line.iWidth + linegap);
                }
                GDI_Panel.Ellipse((int)(x1 * m_ScaleRatio_W),
                             (int)(y1 * m_ScaleRatio_H),
                             (int)(x2 * m_ScaleRatio_W),
                             (int)(y2 * m_ScaleRatio_H));
                GDI_Panel.DeleteOBJECT(BrushHandle1);
            }
        }

        private void DrawEndEllipse(LineSetting_Rectangle Line, int linegap, int iDiameter, IntPtr BrushHandle1)
        {
            if(StaticVar.iRectFingerNum == 2 && LocalVariable.PaintTool_Var.lsRectangleLineSetting.Count != 4)
            {
                GDI_Panel.AssignPenHandle(BrushHandle1);
                int x1 = Line.iXCoordinate + Line.iWidth - iDiameter, y1 = Line.iYCoordinate + Line.iHeight - iDiameter, x2 = Line.iXCoordinate + Line.iWidth, y2 = Line.iYCoordinate + Line.iHeight;
                if((y2 + linegap + Line.iHeight) >= m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax)
                {
                    if(Line.DirectionRule == enRectangleDirection.LeftToRight
                           || Line.DirectionRule == enRectangleDirection.RightToLeft)
                    {
                        y1 -= (Line.iHeight + linegap);
                        y2 -= (Line.iHeight + linegap);
                    }
                }
                else
                {
                    y1 += (Line.iHeight + linegap);
                    y2 += (Line.iHeight + linegap);
                }
                if((x2 + linegap + Line.iWidth) >= m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax)
                {
                    if(Line.DirectionRule == enRectangleDirection.BottomToTop
                        || Line.DirectionRule == enRectangleDirection.TopToBottom)
                    {
                        x1 -= (Line.iWidth + linegap);
                        x2 -= (Line.iWidth + linegap);
                    }
                }
                else
                {
                    x1 += (Line.iWidth + linegap);
                    x2 += (Line.iWidth + linegap);
                }
                GDI_Panel.Ellipse((int)(x1 * m_ScaleRatio_W),
                             (int)(y1 * m_ScaleRatio_H),
                             (int)(x2 * m_ScaleRatio_W),
                             (int)(y2 * m_ScaleRatio_H));
                GDI_Panel.DeleteOBJECT(BrushHandle1);
            }
        }

        private void DrawSecondRectangle(LineSetting_Rectangle Line, int linegap)
        {
            if((StaticVar.iRectFingerNum == 2 && LocalVariable.PaintTool_Var.lsRectangleLineSetting.Count != 4)
                || NowTestLineStatus.Length == 12)
            {
                int x1 = Line.iXCoordinate, y1 = Line.iYCoordinate, x2 = Line.iXCoordinate + Line.iWidth, y2 = Line.iYCoordinate + Line.iHeight;
                if((y2 + linegap + Line.iHeight) >= m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax)
                {
                    if(Line.DirectionRule == enRectangleDirection.LeftToRight || Line.DirectionRule == enRectangleDirection.RightToLeft)
                    {
                        y1 -= (Line.iHeight + linegap);
                        y2 -= (Line.iHeight + linegap);
                    }
                }
                else
                {
                    y1 += (Line.iHeight + linegap);
                    y2 += (Line.iHeight + linegap);
                }
                if((x2 + linegap + Line.iWidth) >= m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax)
                {
                    if(Line.DirectionRule == enRectangleDirection.BottomToTop
                        || Line.DirectionRule == enRectangleDirection.TopToBottom)
                    {
                        x1 -= (Line.iWidth + linegap);
                        x2 -= (Line.iWidth + linegap);
                    }
                }
                else
                {
                    x1 += (Line.iWidth + linegap);
                    x2 += (Line.iWidth + linegap);
                }
                if(NowTestLineStatus.Length != 12)
                {
                    DrawRectangle((int)(x1 * m_ScaleRatio_W)
                   , (int)(y1 * m_ScaleRatio_H)
                   , (int)(x2 * m_ScaleRatio_W)
                   , (int)(y2 * m_ScaleRatio_H));
                }
                else
                {
                    DrawRectangleWithMiddleLine((int)(x1 * m_ScaleRatio_W)
                   , (int)(y1 * m_ScaleRatio_H)
                   , (int)(x2 * m_ScaleRatio_W)
                   , (int)(y2 * m_ScaleRatio_H));
                }
            }
        }

        private void DrawSecondRectangle3(LineSetting_Rectangle3 Line, int linegap)
        {
            if((StaticVar.iRectFingerNum == 2 && LocalVariable.PaintTool_Var.lsRectangleLineSetting.Count != 4)
                || NowTestLineStatus.Length == 12)
            {
                int x1 = Line.iXCoordinate, y1 = Line.iYCoordinate, x2 = Line.iXCoordinate + Line.iWidth, y2 = Line.iYCoordinate + Line.iHeight;
                if((y2 + linegap + Line.iHeight) >= m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax)
                {
                    if(Line.DirectionRule == enRectangleDirection.LeftToRight || Line.DirectionRule == enRectangleDirection.RightToLeft)
                    {
                        y1 -= (Line.iHeight + linegap);
                        y2 -= (Line.iHeight + linegap);
                    }
                }
                else
                {
                    y1 += (Line.iHeight + linegap);
                    y2 += (Line.iHeight + linegap);
                }
                if((x2 + linegap + Line.iWidth) >= m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax)
                {
                    if(Line.DirectionRule == enRectangleDirection.BottomToTop
                        || Line.DirectionRule == enRectangleDirection.TopToBottom)
                    {
                        x1 -= (Line.iWidth + linegap);
                        x2 -= (Line.iWidth + linegap);
                    }
                }
                else
                {
                    x1 += (Line.iWidth + linegap);
                    x2 += (Line.iWidth + linegap);
                }
                if(NowTestLineStatus.Length != 12)
                {
                    DrawRectangle((int)(x1 * m_ScaleRatio_W)
                   , (int)(y1 * m_ScaleRatio_H)
                   , (int)(x2 * m_ScaleRatio_W)
                   , (int)(y2 * m_ScaleRatio_H));
                }
                else
                {
                    DrawRectangleWithMiddleLine((int)(x1 * m_ScaleRatio_W)
                   , (int)(y1 * m_ScaleRatio_H)
                   , (int)(x2 * m_ScaleRatio_W)
                   , (int)(y2 * m_ScaleRatio_H));
                }
            }
        }

        private void DrawDirectSecondLine(LineSetting_Rectangle Line, int linegap, int iDiameter)
        {
            if(StaticVar.iRectFingerNum == 2 && LocalVariable.PaintTool_Var.lsRectangleLineSetting.Count != 4)
            {
                NowTestLineStatus[1].dRadius = (double)iDiameter / 2;//半徑
                NowTestLineStatus[1].Start.X = NowTestLineStatus[0].Start.X;
                NowTestLineStatus[1].Start.Y = NowTestLineStatus[0].Start.Y;
                NowTestLineStatus[1].End.X = NowTestLineStatus[0].End.X;
                NowTestLineStatus[1].End.Y = NowTestLineStatus[0].End.Y;
                //
                if((NowTestLineStatus[1].End.Y + linegap + Line.iHeight) >= m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax)
                {
                    if(Line.DirectionRule == enRectangleDirection.LeftToRight
                            || Line.DirectionRule == enRectangleDirection.RightToLeft)
                    {
                        NowTestLineStatus[1].Start.Y -= (Line.iHeight + linegap);
                        NowTestLineStatus[1].End.Y -= (Line.iHeight + linegap);
                    }
                }
                else
                {
                    NowTestLineStatus[1].Start.Y += (Line.iHeight + linegap);
                    NowTestLineStatus[1].End.Y += (Line.iHeight + linegap);
                }
                if((NowTestLineStatus[1].End.X + linegap + Line.iWidth) >= m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax)
                {
                    if(Line.DirectionRule == enRectangleDirection.BottomToTop
                        || Line.DirectionRule == enRectangleDirection.TopToBottom)
                    {
                        NowTestLineStatus[1].Start.X -= (Line.iWidth + linegap);
                        NowTestLineStatus[1].End.X -= (Line.iWidth + linegap);
                    }
                }
                else
                {
                    NowTestLineStatus[1].Start.X += (Line.iWidth + linegap);
                    NowTestLineStatus[1].End.X += (Line.iWidth + linegap);
                }
                DrawDirectionLine((int)(NowTestLineStatus[1].Start.X * m_ScaleRatio_W),
                            (int)(NowTestLineStatus[1].Start.Y * m_ScaleRatio_H),
                            (int)(NowTestLineStatus[1].End.X * m_ScaleRatio_W),
                            (int)(NowTestLineStatus[1].End.Y * m_ScaleRatio_H),
                            enPaintTestPattern.Paint_PaintRectangle,
                            LocalVariable.PaintTool_Var._ComColor.Color_Direction);
            }
        }

        private void DrawDirectSecondLineForRectangle3(int index, LineSetting_Rectangle3 Line, int linegap, int iDiameter)
        {
            if(StaticVar.iRectFingerNum == 2 && LocalVariable.PaintTool_Var.lsRectangleLineSetting.Count != 4
                || NowTestLineStatus.Length == 12)
            {
                NowTestLineStatus[index + 4].dRadius = NowTestLineStatus[index].dRadius;//半徑
                NowTestLineStatus[index + 4].dWidth = NowTestLineStatus[index].dWidth;
                NowTestLineStatus[index + 4].dHeight = NowTestLineStatus[index].dHeight;
                NowTestLineStatus[index + 4].Start.X = NowTestLineStatus[index].Start.X;
                NowTestLineStatus[index + 4].Start.Y = NowTestLineStatus[index].Start.Y;
                NowTestLineStatus[index + 4].End.X = NowTestLineStatus[index].End.X;
                NowTestLineStatus[index + 4].End.Y = NowTestLineStatus[index].End.Y;

                NowTestLineStatus[index + 8].dRadius = NowTestLineStatus[index].dRadius;//半徑
                NowTestLineStatus[index + 8].dWidth = NowTestLineStatus[index].dWidth;
                NowTestLineStatus[index + 8].dHeight = NowTestLineStatus[index].dHeight;
                NowTestLineStatus[index + 8].Start.X = NowTestLineStatus[index].Start.X;
                NowTestLineStatus[index + 8].Start.Y = NowTestLineStatus[index].Start.Y;
                NowTestLineStatus[index + 8].End.X = NowTestLineStatus[index].End.X;
                NowTestLineStatus[index + 8].End.Y = NowTestLineStatus[index].End.Y;

                //
                if((NowTestLineStatus[index + 4].End.Y + linegap + Line.iHeight) >= m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax)
                {
                    if(Line.DirectionRule == enRectangleDirection.LeftToRight
                            || Line.DirectionRule == enRectangleDirection.RightToLeft)
                    {
                        NowTestLineStatus[index + 4].Start.Y -= (Line.iHeight + linegap);
                        NowTestLineStatus[index + 4].End.Y -= (Line.iHeight + linegap);
                    }
                }
                else
                {
                    NowTestLineStatus[index + 4].Start.Y += (Line.iHeight + linegap);
                    NowTestLineStatus[index + 4].End.Y += (Line.iHeight + linegap);
                }
                if((NowTestLineStatus[index + 4].End.X + linegap + Line.iWidth) >= m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax)
                {
                    if(Line.DirectionRule == enRectangleDirection.BottomToTop
                        || Line.DirectionRule == enRectangleDirection.TopToBottom)
                    {
                        NowTestLineStatus[index + 4].Start.X -= (Line.iWidth + linegap);
                        NowTestLineStatus[index + 4].End.X -= (Line.iWidth + linegap);
                    }
                }
                else
                {
                    NowTestLineStatus[index + 4].Start.X += (Line.iWidth + linegap);
                    NowTestLineStatus[index + 4].End.X += (Line.iWidth + linegap);
                }

                AllRectSection[index + 4] = new Rectangle(NowTestLineStatus[index + 4].Start.X - (int)NowTestLineStatus[index + 4].dWidth / 2
                                                      , NowTestLineStatus[index + 4].Start.Y - (int)NowTestLineStatus[index + 4].dHeight / 2
                                                      , (int)NowTestLineStatus[index + 4].dWidth
                                                      , (int)NowTestLineStatus[index + 4].dHeight);

                if(lsRect3FingerRange.Count < 4)
                {
                    if(NowTestLineStatus[index + 4].Start.Y == NowTestLineStatus[index + 4].End.Y)
                    {
                        if(NowTestLineStatus[index + 4].Start.X < NowTestLineStatus[index + 4].End.X)
                        {
                            lsRect3FingerRange.Add(new Rectangle(NowTestLineStatus[index + 4].Start.X - (int)NowTestLineStatus[index + 4].dWidth / 2
                                                             , NowTestLineStatus[index + 4].Start.Y - (int)NowTestLineStatus[index + 4].dHeight / 2
                                                             , (int)NowTestLineStatus[index + 4].End.X + (int)NowTestLineStatus[index + 4].dWidth / 2
                                                             , (int)NowTestLineStatus[index + 4].dHeight));
                        }
                        else
                        {
                            lsRect3FingerRange.Add(new Rectangle(NowTestLineStatus[index + 4].End.X - (int)NowTestLineStatus[index + 4].dWidth / 2
                                                                , NowTestLineStatus[index + 4].End.Y - (int)NowTestLineStatus[index + 4].dHeight / 2
                                                                , (int)NowTestLineStatus[index + 4].Start.X + (int)NowTestLineStatus[index + 4].dWidth / 2
                                                                , (int)NowTestLineStatus[index + 4].dHeight));
                        }
                    }
                    else
                    {
                        if(NowTestLineStatus[index + 4].Start.Y < NowTestLineStatus[index + 4].End.Y)
                        {
                            lsRect3FingerRange.Add(new Rectangle(NowTestLineStatus[index + 4].Start.X - (int)NowTestLineStatus[index + 4].dWidth / 2
                                                            , NowTestLineStatus[index + 4].Start.Y - (int)NowTestLineStatus[index + 4].dHeight / 2
                                                            , (int)NowTestLineStatus[index + 4].dWidth
                                                            , (int)NowTestLineStatus[index + 4].End.Y + (int)NowTestLineStatus[index + 4].dHeight / 2));
                        }
                        else
                        {
                            lsRect3FingerRange.Add(new Rectangle(NowTestLineStatus[index + 4].End.X - (int)NowTestLineStatus[index + 4].dWidth / 2
                                                                , NowTestLineStatus[index + 4].End.Y - (int)NowTestLineStatus[index + 4].dHeight / 2
                                                                , (int)NowTestLineStatus[index + 4].dWidth
                                                                , (int)NowTestLineStatus[index + 4].Start.Y + (int)NowTestLineStatus[index + 4].dHeight / 2));
                        }
                    }
                }
            }
        }

        #endregion Rectangle-P2P

        #region Cross-P2P

        private void CrossPatternReset(Color _Color_StartEllipse, Color _Color_EndEllipse, Color _Direction)
        {
            int iBuffIndex = 0;
            dic_StartPoint.Clear();
            Corss3Range = new GraphicsPath[2];
            foreach(var Line in LocalVariable.PaintTool_Var.lsCrossLineSetting)
            {
                //====================================================================================
                //填入測試判斷條件
                switch(Line.DirectionRule)
                {
                    case enCrossDirection.RB_LT:
                    case enCrossDirection.LB_RT:
                        NowTestLineStatus[iBuffIndex].Start = Line.EndPoint;
                        NowTestLineStatus[iBuffIndex].End = Line.StartPoint;
                        break;

                    case enCrossDirection.LT_RB:
                    case enCrossDirection.RT_LB:
                        NowTestLineStatus[iBuffIndex].Start = Line.StartPoint;
                        NowTestLineStatus[iBuffIndex].End = Line.EndPoint;
                        break;
                }
                bool isNewPattern = (LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PaintCross3
                    || LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PenCross3);

                if(isNewPattern)
                    _Color_EndEllipse = _Color_StartEllipse;
                //以正方形來考慮
                double dDiameter = (double)Math.Sqrt(Math.Pow(Line.iDistance, 2) + Math.Pow(Line.iDistance, 2));
                NowTestLineStatus[iBuffIndex].dRadius = (double)Line.iDistance;//判斷距離的條件
                var TestLine = NowTestLineStatus[iBuffIndex];
                iBuffIndex++;
                //====================================================================================
                // Drawing Direction
                if(!isNewPattern)
                {
                    DrawDirectionLine(
                     (int)(TestLine.Start.X * m_ScaleRatio_W), (int)(TestLine.Start.Y * m_ScaleRatio_H),
                     (int)(TestLine.End.X * m_ScaleRatio_W), (int)(TestLine.End.Y * m_ScaleRatio_H)
                     , enPaintTestPattern.Paint_PaintCross, _Direction, dDiameter);
                }
                else
                {
                    DrawDirectionLine_WithoutArrow(
                        (int)(TestLine.Start.X * m_ScaleRatio_W), (int)(TestLine.Start.Y * m_ScaleRatio_H),
                        (int)(TestLine.End.X * m_ScaleRatio_W), (int)(TestLine.End.Y * m_ScaleRatio_H)
                        , enPaintTestPattern.Paint_PaintCross, _Direction);
                }

                //, enPaintTestPattern.Paint_PaintCross, LocalVariable.PaintTool_Var._ComColor.Color_Direction);

                //====================================================================================
                //邊線
                var NewPoint = new Point();
                IntPtr BrushHandle = IntPtr.Zero;
                BrushHandle = GDI_Panel.CreateBrushHandle(_Direction);
                GDI_Panel.AssignPenHandle(BrushHandle);
                //this.Show();
                switch(Line.DirectionRule)
                {
                    case enCrossDirection.LT_RB:
                        //Start:{X+D,Y}
                        NewPoint = new Point((int)((TestLine.Start.X + dDiameter) * m_ScaleRatio_W), (int)(TestLine.Start.Y * m_ScaleRatio_H));
                        GDI_Panel.MoveTo(NewPoint);
                        //End:{X,Y-D}
                        NewPoint = new Point((int)(TestLine.End.X * m_ScaleRatio_W), (int)((TestLine.End.Y - dDiameter) * m_ScaleRatio_H));
                        GDI_Panel.LineTo(NewPoint);
                        //Start:{X,Y+D}
                        NewPoint = new Point((int)(TestLine.Start.X * m_ScaleRatio_W), (int)((TestLine.Start.Y + dDiameter) * m_ScaleRatio_H));
                        GDI_Panel.MoveTo(NewPoint);
                        //End:{X-D,Y}
                        NewPoint = new Point((int)((TestLine.End.X - dDiameter) * m_ScaleRatio_W), (int)(TestLine.End.Y * m_ScaleRatio_H));
                        GDI_Panel.LineTo(NewPoint);
                        break;

                    case enCrossDirection.RB_LT:
                        //Start:{X,Y-D}
                        NewPoint = new Point((int)((TestLine.Start.X) * m_ScaleRatio_W), (int)((TestLine.Start.Y - dDiameter) * m_ScaleRatio_H));
                        GDI_Panel.MoveTo(NewPoint);
                        //End:{X+D,Y}
                        NewPoint = new Point((int)((TestLine.End.X + dDiameter) * m_ScaleRatio_W), (int)((TestLine.End.Y) * m_ScaleRatio_H));
                        GDI_Panel.LineTo(NewPoint);
                        //Start:{X-D,Y}
                        NewPoint = new Point((int)((TestLine.Start.X - dDiameter) * m_ScaleRatio_W), (int)(TestLine.Start.Y * m_ScaleRatio_H));
                        GDI_Panel.MoveTo(NewPoint);
                        //End:{X,Y+D}
                        NewPoint = new Point((int)((TestLine.End.X) * m_ScaleRatio_W), (int)((TestLine.End.Y + dDiameter) * m_ScaleRatio_H));
                        GDI_Panel.LineTo(NewPoint);
                        break;

                    case enCrossDirection.RT_LB:
                        //Start:{X-D,Y}
                        NewPoint = new Point((int)((TestLine.Start.X - dDiameter) * m_ScaleRatio_W), (int)(TestLine.Start.Y * m_ScaleRatio_H));
                        GDI_Panel.MoveTo(NewPoint);
                        //End:{X,Y-D}
                        NewPoint = new Point((int)((TestLine.End.X) * m_ScaleRatio_W), (int)((TestLine.End.Y - dDiameter) * m_ScaleRatio_H));
                        GDI_Panel.LineTo(NewPoint);
                        //Start:{X,Y+D}
                        NewPoint = new Point((int)(TestLine.Start.X * m_ScaleRatio_W), (int)((TestLine.Start.Y + dDiameter) * m_ScaleRatio_H));
                        GDI_Panel.MoveTo(NewPoint);
                        //End:{X+D,Y}
                        NewPoint = new Point((int)((TestLine.End.X + dDiameter) * m_ScaleRatio_W), (int)(TestLine.End.Y * m_ScaleRatio_H));
                        GDI_Panel.LineTo(NewPoint);
                        break;

                    case enCrossDirection.LB_RT:
                        //Start:{X,Y-D}
                        NewPoint = new Point((int)((TestLine.Start.X) * m_ScaleRatio_W), (int)((TestLine.Start.Y - dDiameter) * m_ScaleRatio_H));
                        GDI_Panel.MoveTo(NewPoint);
                        //End:{X-D,Y}
                        NewPoint = new Point((int)((TestLine.End.X - dDiameter) * m_ScaleRatio_W), (int)((TestLine.End.Y) * m_ScaleRatio_H));
                        GDI_Panel.LineTo(NewPoint);
                        //Start:{X+D,Y}
                        NewPoint = new Point((int)((TestLine.Start.X + dDiameter) * m_ScaleRatio_W), (int)((TestLine.Start.Y) * m_ScaleRatio_H));
                        GDI_Panel.MoveTo(NewPoint);
                        //End:{X,Y+D}
                        NewPoint = new Point((int)((TestLine.End.X) * m_ScaleRatio_W), (int)((TestLine.End.Y + dDiameter) * m_ScaleRatio_H));
                        GDI_Panel.LineTo(NewPoint);
                        break;
                }

                //====================================================================================
                //中心虛線
                NewPoint = new Point((int)(TestLine.Start.X * m_ScaleRatio_W), (int)(TestLine.Start.Y * m_ScaleRatio_H));
                GDI_Panel.MoveTo(NewPoint);
                NewPoint = new Point((int)(TestLine.End.X * m_ScaleRatio_W), (int)(TestLine.End.Y * m_ScaleRatio_H));
                GDI_Panel.LineTo(NewPoint);

                GDI_Panel.DeleteOBJECT(BrushHandle);

                //====================================================================================
                // 畫起始點顏色=Color_StartEllipse
                IntPtr StartBrushHandle = IntPtr.Zero;
                Point StartEllipse_P1 = new Point();
                Point StartEllipse_P2 = new Point();
                //新增用來記錄另外兩組座標
                Point StartEllipse_2_P1 = new Point();
                Point StartEllipse_2_P2 = new Point();

                IntPtr EndBrushHandle = IntPtr.Zero;
                Point EndEllipse_P1 = new Point();
                Point EndEllipse_P2 = new Point();
                //新增用來記錄另外兩組座標
                Point EndEllipse_2_P1 = new Point();
                Point EndEllipse_2_P2 = new Point();
                StartBrushHandle = GDI_Panel.CreateBrushHandle(_Color_StartEllipse);
                EndBrushHandle = GDI_Panel.CreateBrushHandle(_Color_EndEllipse);
                //StartBrushHandle = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse);
                //EndBrushHandle = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse);
                var iDiameter = (int)dDiameter;
                switch(Line.DirectionRule)
                {
                    case enCrossDirection.LT_RB:
                        StartEllipse_P1 = new Point((int)(TestLine.Start.X * m_ScaleRatio_W), (int)(TestLine.Start.Y * m_ScaleRatio_H));
                        StartEllipse_P2 = new Point((int)((TestLine.Start.X + dDiameter) * m_ScaleRatio_W), (int)((TestLine.Start.Y + dDiameter) * m_ScaleRatio_H));
                        EndEllipse_P1 = new Point((int)((TestLine.End.X - dDiameter) * m_ScaleRatio_W), (int)((TestLine.End.Y - dDiameter) * m_ScaleRatio_H));
                        EndEllipse_P2 = new Point((int)(TestLine.End.X * m_ScaleRatio_W), (int)(TestLine.End.Y * m_ScaleRatio_H));

                        CrossCornerBlock[0] = new Rectangle(TestLine.Start.X, TestLine.Start.Y, iDiameter, iDiameter);
                        CrossCornerBlock[1] = new Rectangle(TestLine.End.X - iDiameter, TestLine.End.Y - iDiameter, iDiameter, iDiameter);

                        break;

                    case enCrossDirection.RB_LT:
                        StartEllipse_P1 = new Point((int)(TestLine.Start.X * m_ScaleRatio_W), (int)(TestLine.Start.Y * m_ScaleRatio_H));
                        StartEllipse_P2 = new Point((int)((TestLine.Start.X - dDiameter) * m_ScaleRatio_W), (int)((TestLine.Start.Y - dDiameter) * m_ScaleRatio_H));
                        EndEllipse_P1 = new Point((int)((TestLine.End.X + dDiameter) * m_ScaleRatio_W), (int)((TestLine.End.Y + dDiameter) * m_ScaleRatio_H));
                        EndEllipse_P2 = new Point((int)(TestLine.End.X * m_ScaleRatio_W), (int)(TestLine.End.Y * m_ScaleRatio_H));

                        CrossCornerBlock[0] = new Rectangle(TestLine.Start.X - iDiameter, TestLine.Start.Y - iDiameter, iDiameter, iDiameter);
                        CrossCornerBlock[1] = new Rectangle(TestLine.End.X + iDiameter, TestLine.End.Y + iDiameter, iDiameter, iDiameter);
                        break;

                    case enCrossDirection.RT_LB:
                        StartEllipse_P1 = new Point((int)(TestLine.Start.X * m_ScaleRatio_W), (int)(TestLine.Start.Y * m_ScaleRatio_H));
                        StartEllipse_P2 = new Point((int)((TestLine.Start.X - dDiameter) * m_ScaleRatio_W), (int)((TestLine.Start.Y + dDiameter) * m_ScaleRatio_H));
                        EndEllipse_P1 = new Point((int)((TestLine.End.X + dDiameter) * m_ScaleRatio_W), (int)((TestLine.End.Y - dDiameter) * m_ScaleRatio_H));
                        EndEllipse_P2 = new Point((int)(TestLine.End.X * m_ScaleRatio_W), (int)(TestLine.End.Y * m_ScaleRatio_H));

                        StartEllipse_2_P1 = new Point((int)(TestLine.End.X * m_ScaleRatio_W), (int)(TestLine.Start.Y * m_ScaleRatio_H));

                        StartEllipse_2_P2 = new Point((int)((TestLine.End.X + dDiameter) * m_ScaleRatio_W), (int)((TestLine.Start.Y + dDiameter) * m_ScaleRatio_H));

                        EndEllipse_2_P1 = new Point((int)((TestLine.Start.X - dDiameter) * m_ScaleRatio_W), (int)((TestLine.End.Y - dDiameter) * m_ScaleRatio_H));

                        EndEllipse_2_P2 = new Point((int)(TestLine.Start.X * m_ScaleRatio_W), (int)(TestLine.End.Y * m_ScaleRatio_H));

                        CrossCornerBlock[0] = new Rectangle(TestLine.Start.X - iDiameter, TestLine.Start.Y, iDiameter, iDiameter);
                        dic_StartPoint.Add(0, new Point[2] { StartEllipse_P1, StartEllipse_P2 });

                        CrossCornerBlock[1] = new Rectangle(TestLine.End.X, TestLine.End.Y - iDiameter, iDiameter, iDiameter);
                        dic_StartPoint.Add(1, new Point[2] { EndEllipse_P1, EndEllipse_P2 });

                        CrossCornerBlock[2] = new Rectangle(TestLine.End.X, TestLine.Start.Y, iDiameter, iDiameter);
                        dic_StartPoint.Add(2, new Point[2] { StartEllipse_2_P1, StartEllipse_2_P2 });

                        CrossCornerBlock[3] = new Rectangle(TestLine.Start.X - iDiameter, TestLine.End.Y - iDiameter, iDiameter, iDiameter);
                        dic_StartPoint.Add(3, new Point[2] { EndEllipse_2_P1, EndEllipse_2_P2 });

                        _Cross3_Struct corss3Struct0 = new _Cross3_Struct();
                        _Cross3_Struct corss3Struct1 = new _Cross3_Struct();
                        corss3Struct0.grpicPath = new GraphicsPath();
                        corss3Struct1.grpicPath = new GraphicsPath();

                        Corss3Range[0] = new GraphicsPath();
                        Point _LineStartPoint = new Point(TestLine.Start.X, TestLine.Start.Y);
                        Point _LineEndPoint = new Point(TestLine.End.X, TestLine.End.Y);
                        Point P_UpperR = new Point(TestLine.Start.X - iDiameter, TestLine.Start.Y);
                        Point P_LowerR = new Point(TestLine.End.X, TestLine.End.Y - iDiameter);
                        Point P_LowerL = new Point(TestLine.End.X + iDiameter, TestLine.End.Y);
                        Point P_UpperL = new Point(TestLine.Start.X, TestLine.Start.Y + iDiameter);
                        Corss3Range[0].AddPolygon(new Point[] { _LineStartPoint, P_UpperR, P_LowerR, _LineEndPoint, P_LowerL, P_UpperL });
                        corss3Struct0.grpicPath.AddPolygon(new Point[] { new Point((int)((TestLine.Start.X - iDiameter) * m_ScaleRatio_W), (int)((TestLine.Start.Y + iDiameter)* m_ScaleRatio_H))
                            , new Point((int)(P_UpperR.X*m_ScaleRatio_W ), (int)(P_UpperR.Y*m_ScaleRatio_H))
                            , new Point((int)(P_LowerR.X*m_ScaleRatio_W ), (int)(P_LowerR.Y*m_ScaleRatio_H))
                            , new Point((int)((TestLine.End.X + iDiameter)*m_ScaleRatio_W), (int)((TestLine.End.Y - iDiameter) * m_ScaleRatio_H))
                            , new Point((int)(P_LowerL.X*m_ScaleRatio_W ), (int)(P_LowerL.Y*m_ScaleRatio_H))
                            , new Point((int)(P_UpperL.X*m_ScaleRatio_W ), (int)(P_UpperL.Y*m_ScaleRatio_H)) });

                        Corss3Range[1] = new GraphicsPath();
                        _LineStartPoint = new Point(TestLine.End.X, TestLine.Start.Y);
                        _LineEndPoint = new Point(TestLine.Start.X, TestLine.End.Y);
                        P_UpperR = new Point(TestLine.End.X + iDiameter, TestLine.Start.Y);
                        P_LowerR = new Point(TestLine.Start.X, TestLine.End.Y - iDiameter);
                        P_LowerL = new Point(TestLine.Start.X - iDiameter, TestLine.End.Y);
                        P_UpperL = new Point(TestLine.End.X, TestLine.Start.Y + iDiameter);
                        Corss3Range[1].AddPolygon(new Point[] { _LineStartPoint, P_UpperR, P_LowerR, _LineEndPoint, P_LowerL, P_UpperL });
                        corss3Struct1.grpicPath.AddPolygon(new Point[] { new Point((int)((TestLine.End.X + iDiameter)*m_ScaleRatio_W), (int)((TestLine.Start.Y + iDiameter)*m_ScaleRatio_H))
                            , new Point((int)(P_UpperR.X*m_ScaleRatio_W ), (int)(P_UpperR.Y*m_ScaleRatio_H))
                            , new Point((int)(P_LowerR.X*m_ScaleRatio_W ), (int)(P_LowerR.Y*m_ScaleRatio_H))
                            , new Point((int)((TestLine.Start.X - iDiameter)*m_ScaleRatio_W), (int)((TestLine.End.Y - iDiameter)*m_ScaleRatio_H))
                            , new Point((int)(P_LowerL.X*m_ScaleRatio_W ), (int)(P_LowerL.Y*m_ScaleRatio_H))
                            , new Point((int)(P_UpperL.X*m_ScaleRatio_W ), (int)(P_UpperL.Y*m_ScaleRatio_H)) });

                        if(Cross3Result.Count < 2)
                        {
                            corss3Struct0.result = ResultStatus.Continue;
                            corss3Struct1.result = ResultStatus.Continue;
                            Cross3Result.Add(0, corss3Struct0);
                            Cross3Result.Add(1, corss3Struct1);
                        }
                        else
                        {
                            foreach(var item in Cross3Result)
                            {
                                if(item.Value.result == ResultStatus.PASS)
                                {
                                    Graphics grp = m_panDrawing.CreateGraphics();
                                    grp.FillPolygon(new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_Pass), item.Value.grpicPath.PathPoints, FillMode.Alternate);
                                }
                            }
                        }

                        break;

                    case enCrossDirection.LB_RT:
                        StartEllipse_P1 = new Point((int)(TestLine.Start.X * m_ScaleRatio_W), (int)(TestLine.Start.Y * m_ScaleRatio_H));
                        StartEllipse_P2 = new Point((int)((TestLine.Start.X + dDiameter) * m_ScaleRatio_W), (int)((TestLine.Start.Y - dDiameter) * m_ScaleRatio_H));
                        EndEllipse_P1 = new Point((int)((TestLine.End.X - dDiameter) * m_ScaleRatio_W), (int)((TestLine.End.Y + dDiameter) * m_ScaleRatio_H));
                        EndEllipse_P2 = new Point((int)(TestLine.End.X * m_ScaleRatio_W), (int)(TestLine.End.Y * m_ScaleRatio_H));

                        CrossCornerBlock[0] = new Rectangle(TestLine.Start.X, TestLine.Start.Y - iDiameter, iDiameter, iDiameter);
                        CrossCornerBlock[1] = new Rectangle(TestLine.End.X - iDiameter, TestLine.End.Y, iDiameter, iDiameter);
                        break;
                }

                m_StartBlock = new Point[2] { StartEllipse_P1, StartEllipse_P2 };

                GDI_Panel.AssignPenHandle(StartBrushHandle);
                GDI_Panel.Rectangle(StartEllipse_P1.X, StartEllipse_P1.Y, StartEllipse_P2.X, StartEllipse_P2.Y);
                GDI_Panel.DeleteOBJECT(StartBrushHandle);

                GDI_Panel.AssignPenHandle(EndBrushHandle);
                GDI_Panel.Rectangle(EndEllipse_P1.X, EndEllipse_P1.Y, EndEllipse_P2.X, EndEllipse_P2.Y);
                GDI_Panel.DeleteOBJECT(EndBrushHandle);
            }
        }

        #endregion Cross-P2P

        #region Rectangle-Fill Color

        private ResultStatus FillBorderColor(MultiPaintTool_VAR PaintVar, int finger_idx, enTouchStatus FigStatus)
        {
            if(LocalVariable.PaintTool_Var._TrajectoryRectangle.bDetectRandomPoint)
            {
                if(finger_idx != 0)
                {
                    vfSetlabMegText(ErrMsg.SensorTest.FingerNumber, finger_idx);
                    return ResultStatus.FAIL;
                }
            }
            if(finger_idx != 0)
                return ResultStatus.Continue;
            List<Point> AllFingerData = new List<Point>();
            if(PaintVar.NowPaintTestObj == enPaintTestObj.Finger)
            {
                foreach(var item in PaintVar.stuFinger_Old)
                    if(item.iPosition_X != 0 || item.iPosition_Y != 0)
                        AllFingerData.Add(new Point(item.iPosition_X, item.iPosition_Y));
            }
            else
            {
                foreach(var item in PaintVar.stuPen_Old)
                    if(item.iPosition_X != 0 || item.iPosition_Y != 0)
                        AllFingerData.Add(new Point(item.iPosition_X, item.iPosition_Y));
            }
            //=====================================
            //是否檢查超出範圍
            if(AllFingerData.Count == 0)
                return ResultStatus.Continue;
            bool bOutofRange = ((AllFingerData[finger_idx].X > InsideRectangle[0].X) && (AllFingerData[finger_idx].X < InsideRectangle[2].X));
            bOutofRange &= ((AllFingerData[finger_idx].Y > InsideRectangle[0].Y) && (AllFingerData[finger_idx].Y < InsideRectangle[2].Y));
            if(LocalVariable.PaintTool_Var._TrajectoryRectangle.bDetectOverRange)
            {
                if(bOutofRange)
                {
                    if(!LocalVariable.PaintTool_Var.bOutOfRange)
                    {
                        LocalVariable.PaintTool_Var.inRetry--;
                        SetSideColor(enPaintTestPattern.Paint_PaintRectangle2, LocalVariable.PaintTool_Var._ComColor.Color_Fail);
                        vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry);
                    }
                    LocalVariable.PaintTool_Var.bOutOfRange = true;
                }

                if(LocalVariable.PaintTool_Var.bOutOfRange)
                {
                    if(FigStatus == enTouchStatus.Release || FigStatus == enTouchStatus.Hover) //20220701 Steven Add
                    {
                        LocalVariable.PaintTool_Var.bOutOfRange = false;
                        SetSideColor(enPaintTestPattern.Paint_PaintRectangle2, LocalVariable.PaintTool_Var._ComColor.Color_Rectangle);
                    }

                    if(LocalVariable.PaintTool_Var.inRetry == 0)
                        return ResultStatus.FAIL;

                    return ResultStatus.Continue;
                }
            }
            else
            {
                if(bOutofRange && !m_bAllBlocksPressed)
                    return ResultStatus.Continue;
            }

            foreach(var _item in Blocks)
            {
                if(!_item.Key.IsVisible(AllFingerData[finger_idx].X, AllFingerData[finger_idx].Y))
                    continue;

                if(PressedBlocks[_item.Key].bPressed)
                    continue;
                var _Handle = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                GDI_Panel.AssignPenHandle(_Handle);
                SolidBrush _BrushesColor = new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                var grp = Graphics.FromHwnd(m_panDrawing.Handle);

                grp.FillPolygon(_BrushesColor, _item.Value.PolygonPath.ToArray(), FillMode.Alternate);

                GDI_Panel.DeleteOBJECT(_Handle);

                var employee = new _Block();
                employee = _item.Value;
                employee.bPressed = true;
                PressedBlocks[_item.Key] = employee;

                //20230721:Vane++ Sean的客戶反映角落很難畫得PASS，所以要放寬測試，角落點隔壁兩邊都有畫到，該角落就算PASS。
                //判斷成立條件(&&)：
                //1. 必須有放寬2倍以上
                //2. 如果按下的位置目是角落的隔壁，則要判斷角落的兩邊都有被按過(共8個位置要判斷)
                //3. 兩個邊都有被按下，則該角落自動填滿綠色並判斷pass
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryRectangle.RangeWidth > 1)
                {
                    if(_item.Value.bCornerNeighbor)
                    {
                        var grpID = _item.Value.iCornerGroup;
                        var SameCornerGroup = PressedBlocks.Values.ToList().FindAll(x => x.iCornerGroup == grpID);
                        var tmpNeighbor = SameCornerGroup.Where(x => x.bCornerNeighbor == true).Select(x => x);

                        //Console.WriteLine("grpID = {0}", grpID);
                        //判斷這一個Group是否已經全部按下
                        if(tmpNeighbor.All(x => x.bPressed) == true)
                        {
                            foreach(var test in SameCornerGroup)
                            {
                                _Handle = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                                GDI_Panel.AssignPenHandle(_Handle);
                                _BrushesColor = new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                                grp = Graphics.FromHwnd(m_panDrawing.Handle);
                                var sel = Blocks.FirstOrDefault(pair => pair.Value.ID == test.ID);
                                var employee_2 = new _Block();

                                grp.FillPolygon(_BrushesColor, sel.Value.PolygonPath.ToArray(), FillMode.Alternate);
                                GDI_Panel.DeleteOBJECT(_Handle);

                                employee_2 = sel.Value;
                                employee_2.bPressed = true;
                                PressedBlocks[sel.Key] = employee_2;
                            }
                        }
                    }
                }
                //Application.DoEvents();
                break;
            }

            //確認所有的點都有被按下
            if(PressedBlocks.Values.ToList().FindAll(x => !x.bPressed).Count() == 0)
            {
                m_bAllBlocksPressed = true;
                if(FigStatus == enTouchStatus.Release)
                    return ResultStatus.PASS;
            }
            return ResultStatus.Continue;
        }

        #endregion Rectangle-Fill Color

        #region Cross-Fill Color

        private bool[] bCrossLinePass = new bool[] { false, false };
        private int iIDCnts = 0;

        private ResultStatus FillCrossColor(MultiPaintTool_VAR PaintVar, int finger_idx, enTouchStatus FigStatus)
        {
            //=====================================
            //是否檢查手指數量
            if(LocalVariable.PaintTool_Var._TrajectoryCross.bDetectRandomPoint)
            {
                if(finger_idx != 0)
                {
                    vfSetlabMegText(ErrMsg.SensorTest.FingerNumber, finger_idx);
                    return ResultStatus.FAIL;
                }
            }
            if(finger_idx != 0)
                return ResultStatus.Continue;
            //List<Point> AllFingerData = new List<Point>();
            Point[] AllFingerData = new Point[0];
            if(PaintVar.NowPaintTestObj == enPaintTestObj.Finger)
            {
                AllFingerData = PaintVar.stuFinger_Old.Where(p => p.iPosition_X != 0 || p.iPosition_Y != 0).Select(x => new Point(x.iPosition_X, x.iPosition_Y)).ToArray();
            }
            else
            {
                AllFingerData = PaintVar.stuPen_Old.Where(p => p.iPosition_X != 0 || p.iPosition_Y != 0).Select(x => new Point(x.iPosition_X, x.iPosition_Y)).ToArray();
            }
            //if(AllFingerData.Count == 0)
            if(AllFingerData.Length == 0)
                return ResultStatus.Continue;
            //=====================================
            //是否檢查超出範圍
            bool bIsInRange = false;
            foreach(var _item in InsideCross)
                bIsInRange |= _item.IsVisible(AllFingerData[finger_idx].X, AllFingerData[finger_idx].Y);
            if(LocalVariable.PaintTool_Var._TrajectoryCross.bDetectOverRange)
            {
                if(!bIsInRange)
                {
                    if(!LocalVariable.PaintTool_Var.bOutOfRange)
                    {
                        LocalVariable.PaintTool_Var.inRetry--;
                        SetSideColor(enPaintTestPattern.Paint_PaintCross2, LocalVariable.PaintTool_Var._ComColor.Color_Fail);
                        vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry);
                    }
                    LocalVariable.PaintTool_Var.bOutOfRange = true;
                }

                if(LocalVariable.PaintTool_Var.bOutOfRange)
                {
                    if(FigStatus == enTouchStatus.Release || FigStatus == enTouchStatus.Hover) //20220701 Steven Add
                    {
                        LocalVariable.PaintTool_Var.bOutOfRange = false;
                        SetSideColor(enPaintTestPattern.Paint_PaintCross2, LocalVariable.PaintTool_Var._ComColor.Color_Rectangle);
                    }

                    if(LocalVariable.PaintTool_Var.inRetry == 0)
                        return ResultStatus.FAIL;

                    return ResultStatus.Continue;
                }
            }
            else
            {
                if(!bIsInRange && !m_bAllBlocksPressed)
                    //if(!bIsInRange)
                    return ResultStatus.Continue;
            }
            if(bCrossLinePass[0] && bCrossLinePass[1])
            {
                m_bAllBlocksPressed = true;
                if(FigStatus == enTouchStatus.Release)
                    return ResultStatus.PASS;
            }
            //DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Restart, "FillCrossColor");
            //=====================================
            //DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Show, "foreach Blocks = {0}", Blocks.Count);

            #region 改良方法2

            if(AllFingerData[finger_idx].X == 0 || AllFingerData[finger_idx].Y == 0) //20220207 Steven: 新增用來解決兩指觸控左上會有填滿的問題
                return ResultStatus.Continue;
            //搜尋ID_1 Group裡面還沒有按下的群組
            var lsBlock_1 = PressedBlocks.Where(x => x.Value.LineGroup == 0 && !x.Value.bPressed).ToDictionary(x => x.Key, y => y.Value);
            var tmp = lsBlock_1.Where(x => x.Key.IsVisible(AllFingerData[finger_idx].X, AllFingerData[finger_idx].Y)).FirstOrDefault();
            if(tmp.Key != null)
            {
                //DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Show, "\t [1]_item.Key.IsVisible\t");
                SolidBrush _BrushesColor = new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                var grp = Graphics.FromHwnd(m_panDrawing.Handle);
                grp.FillPolygon(_BrushesColor, tmp.Value.PolygonPath.ToArray(), FillMode.Alternate);
                grp.Dispose();
                //DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Show, "\t [3]FillPolygon\t");
                var employee = new _Block();
                employee = tmp.Value;
                employee.bPressed = true;
                PressedBlocks[tmp.Key] = employee;

                //檢查所有ID = 0的區塊是否都已被按下
                var Test_1 = PressedBlocks.Values.Where(x => x.LineGroup == 0).FirstOrDefault(x => !x.bPressed).ID;
                //var Test_1 = lsBlock_1.Values.FirstOrDefault(x => !x.bPressed).ID;
                //DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Show, string.Format("\t [Test_1] ID = {0}\n", Test_1));
                if(Test_1 == 0 && !bCrossLinePass[0])
                {
                    var ls = PressedBlocks.Values.Where(x => x.LineGroup == 0).ToList();
                    _BrushesColor = new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                    grp = Graphics.FromHwnd(m_panDrawing.Handle);
                    foreach(var item in ls)
                        grp.FillPolygon(_BrushesColor, item.PolygonPath.ToArray(), FillMode.Alternate);

                    grp.Dispose();
                    bCrossLinePass[0] = true;
                }
            }
            else
            {
                //沒找到再找ID_2 Group
                var lsBlock_2 = PressedBlocks.Where(x => x.Value.LineGroup == 1 && !x.Value.bPressed).ToDictionary(x => x.Key, y => y.Value);
                var tmp_2 = lsBlock_2.Where(x => x.Key.IsVisible(AllFingerData[finger_idx].X, AllFingerData[finger_idx].Y)).FirstOrDefault();
                if(tmp_2.Key == null)
                    return ResultStatus.Continue;

                SolidBrush _BrushesColor = new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                var grp = Graphics.FromHwnd(m_panDrawing.Handle);
                grp.FillPolygon(_BrushesColor, tmp_2.Value.PolygonPath.ToArray(), FillMode.Alternate);
                grp.Dispose();
                //DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Show, "\t [3]FillPolygon\t");
                var employee = new _Block();
                employee = tmp_2.Value;
                employee.bPressed = true;
                PressedBlocks[tmp_2.Key] = employee;

                var Test_2 = PressedBlocks.Values.Where(x => x.LineGroup == 1).FirstOrDefault(x => !x.bPressed).ID;
                //var Test_2 = lsBlock_2.Values.FirstOrDefault(x => !x.bPressed).ID;
                //DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Show, string.Format("\t [Test_2] ID = {0}\n", Test_2));
                if(Test_2 == 0 && !bCrossLinePass[1])
                {
                    var ls = PressedBlocks.Values.Where(x => x.LineGroup == 1).ToList();
                    _BrushesColor = new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                    grp = Graphics.FromHwnd(m_panDrawing.Handle);
                    foreach(var item in ls)
                        grp.FillPolygon(_BrushesColor, item.PolygonPath.ToArray(), FillMode.Alternate);

                    grp.Dispose();
                    bCrossLinePass[1] = true;
                }
            }

            #endregion 改良方法2


            return ResultStatus.Continue;
        }

        private PointD CalculateCrossPoint(PointD Line1_Start, PointD Line1_End, PointD Line2_Start, PointD Line2_End)
        {
            //邊線
            var TestRangePercent = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryCross.RangeWidth * 10);
            var TheMinimumRes = Math.Min(m_SensorTestPaint_GBV.Panel_Var.inAAXmax, m_SensorTestPaint_GBV.Panel_Var.inAAYmax);
            var W_unit = ((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax / TheMinimumRes) * TestRangePercent + 0.5;
            var H_unit = ((double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax / TheMinimumRes) * TestRangePercent + 0.5;
            //// 已知的兩個點座標
            //PointD P_1 = new PointD(W_unit, 0);
            //PointD P_2 = new PointD(0, H_unit);
            // 第一條直線的斜率和 y 軸截距
            double m1 = (Line1_End.Y - Line1_Start.Y) / (Line1_End.X - Line1_Start.X);
            double b1 = Line1_Start.Y - m1 * Line1_Start.X;
            //// 第二條直線的斜率和 y 軸截距
            //double m2 = (P_End.Y - P_Start.Y) / (P_End.X - P_Start.X);
            //double b2 = P_Start.Y - m2 * P_Start.X;
            // 第二條直線的斜率和 y 軸截距
            double m2 = (Line2_End.Y - Line2_Start.Y) / (Line2_End.X - Line2_Start.X);
            double b2 = Line2_Start.Y - m2 * Line2_Start.X;
            // 求交點的 x 座標
            double xIntersection = (b2 - b1) / (m1 - m2) + 0.5;
            // 求交點的 y 座標
            double yIntersection = m1 * xIntersection + b1 + 0.5;
            //Console.WriteLine("交點座標：(" + xIntersection + ", " + yIntersection + ")");
            return new PointD(xIntersection, yIntersection);
        }

        private void DrawLine(PointD Start, PointD End)
        {
            GDI_Panel.MoveTo((int)(Start.X * m_ScaleRatio_W + 0.5), (int)(Start.Y * m_ScaleRatio_H + 0.5));
            GDI_Panel.LineTo((int)(End.X * m_ScaleRatio_W + 0.5), (int)(End.Y * m_ScaleRatio_H + 0.5));
        }

        private Point PointDToPoint(PointD pointD)
        {
            int x = (int)Math.Round(pointD.X);
            int y = (int)Math.Round(pointD.Y);
            return new Point(x, y);
        }

        private void Fill_CrossPatternReset()
        {
            bCrossLinePass = new bool[] { false, false };
            Blocks = new Dictionary<GraphicsPath, _Block>();
            var _PenColor = GDI_Panel.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 2, LocalVariable.PaintTool_Var._ComColor.Color_Rectangle);
            GDI_Panel.AssignPenHandle(_PenColor);
            this.Show();
            var TestRangePercent = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryCross.RangeWidth * 100);
            var TheMinimumRes = Math.Min(m_SensorTestPaint_GBV.Panel_Var.inAAXmax, m_SensorTestPaint_GBV.Panel_Var.inAAYmax);
            var W_unit = ((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax / TheMinimumRes) * TestRangePercent;
            var H_unit = ((double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax / TheMinimumRes) * TestRangePercent;
            Graphics _Graphics = m_panDrawing.CreateGraphics();
            _Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            //====================================================================================
            //左上右下 1 & 2
            PointD Line_LTRB_1_Start = new PointD(W_unit, 0);
            PointD Line_LTRB_1_End = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax, (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax - H_unit);
            PointD Line_LTRB_2_Start = new PointD(0, H_unit);
            PointD Line_LTRB_2_End = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax - W_unit, (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax);
            //====================================================================================
            //邊線
            DrawLine(Line_LTRB_1_Start, Line_LTRB_1_End);
            DrawLine(Line_LTRB_2_Start, Line_LTRB_2_End);

            // 中心點直線的兩點座標
            Point Line_LTRB_Center_Start = new Point(m_SensorTestPaint_GBV.Panel_Var.inAAXmin, m_SensorTestPaint_GBV.Panel_Var.inAAYmin);
            Point Line_LTRB_Center_End = new Point(m_SensorTestPaint_GBV.Panel_Var.inAAXmax, m_SensorTestPaint_GBV.Panel_Var.inAAYmax);
            InsideCross[0] = new GraphicsPath();
            InsideCross[0].AddPolygon(new Point[] { Line_LTRB_Center_Start, PointDToPoint(Line_LTRB_1_Start), PointDToPoint(Line_LTRB_1_End)
                , Line_LTRB_Center_End, PointDToPoint(Line_LTRB_2_End), PointDToPoint(Line_LTRB_2_Start) });

            var BlockCounts = (int)((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax / W_unit) + ((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax % W_unit == 0 ? 0 : 1);

            iIDCnts = 1;
            Add_LTRB_BlockPath_1(BlockCounts, Line_LTRB_1_Start, Line_LTRB_2_Start, Line_LTRB_1_End, Line_LTRB_2_End, W_unit, H_unit);
            Add_LTRB_BlockPath_2(BlockCounts, Line_LTRB_1_Start, Line_LTRB_2_Start, Line_LTRB_1_End, Line_LTRB_2_End, W_unit, H_unit);

            //=====================
            // 畫虛線
            Pen My_Pen = new Pen(Color.Gray, 5);
            My_Pen.DashStyle = DashStyle.Dash;
            My_Pen.CustomStartCap = new AdjustableArrowCap(5, 5);
            My_Pen.CustomEndCap = new AdjustableArrowCap(5, 5);
            _Graphics.DrawLine(My_Pen, (int)((Line_LTRB_Center_End.X / 4) * m_ScaleRatio_W), (int)(Line_LTRB_Center_End.Y / 4 * m_ScaleRatio_H)
                , (int)(Line_LTRB_Center_End.X / 4 * 3 * m_ScaleRatio_W), (int)(Line_LTRB_Center_End.Y / 4 * 3 * m_ScaleRatio_H));
            //====================================================================================
            //右上左下 1 & 2
            PointD Line_RTLB_1_Start = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax - W_unit, 0);
            PointD Line_RTLB_1_End = new PointD(0, (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax - H_unit);
            PointD Line_RTLB_2_Start = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax, H_unit);
            PointD Line_RTLB_2_End = new PointD(W_unit, (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax);
            //====================================================================================
            //邊線
            DrawLine(Line_RTLB_1_Start, Line_RTLB_1_End);
            DrawLine(Line_RTLB_2_Start, Line_RTLB_2_End);
            // 中心點直線的兩點座標
            Point Line_RTLB_Center_Start = new Point(m_SensorTestPaint_GBV.Panel_Var.inAAXmax, m_SensorTestPaint_GBV.Panel_Var.inAAYmin);
            Point Line_RTLB_Center_End = new Point(m_SensorTestPaint_GBV.Panel_Var.inAAXmin, m_SensorTestPaint_GBV.Panel_Var.inAAYmax);
            InsideCross[1] = new GraphicsPath();
            InsideCross[1].AddPolygon(new Point[] { Line_RTLB_Center_Start, PointDToPoint(Line_RTLB_1_Start), PointDToPoint(Line_RTLB_1_End)
                , Line_RTLB_Center_End, PointDToPoint(Line_RTLB_2_End), PointDToPoint(Line_RTLB_2_Start) });
            iIDCnts = 1;
            Add_RTLB_BlockPath_1(BlockCounts, Line_RTLB_1_Start, Line_RTLB_2_Start, Line_RTLB_1_End, Line_RTLB_2_End, W_unit, H_unit);
            Add_RTLB_BlockPath_2(BlockCounts, Line_RTLB_1_Start, Line_RTLB_2_Start, Line_RTLB_1_End, Line_RTLB_2_End, W_unit, H_unit);
            //Console.WriteLine(Blocks);
            //foreach(var item in Blocks)
            //    MyTool.Save_DBG_Log("ID:{0} {1}", item.Value.ID, string.Join(":", item.Key.PathPoints.Select(p => new
            //    {
            //        x = string.Format("0x{0:X04}", (int)p.X),
            //        y = string.Format("0x{0:X04}", (int)p.Y)
            //    })));

            //=====================
            // 畫虛線
            _Graphics.DrawLine(My_Pen, (int)((Line_RTLB_Center_Start.X / 4 * 3) * m_ScaleRatio_W), (int)(Line_RTLB_Center_End.Y / 4 * m_ScaleRatio_H)
                , (int)(Line_RTLB_Center_Start.X / 4 * m_ScaleRatio_W), (int)(Line_RTLB_Center_End.Y / 4 * 3 * m_ScaleRatio_H));
            //MyTool.Save_DBG_Log(" PointArr {0} ", string.Join(",", test.ToArray()));
            PressedBlocks = Blocks.ToDictionary(entry => entry.Key, entry => entry.Value);
            _Graphics.Dispose();
            My_Pen.Dispose();
        }

        private void Add_LTRB_BlockPath_1(int BlockCounts, PointD P_1_Start, PointD P_2_Start, PointD P_1_End, PointD P_2_End, double W_unit, double H_unit)
        {
            for(double XCnt = 0; XCnt < BlockCounts; XCnt++)
            {
                List<PointD> DrawingPath = new List<PointD>();
                List<Point> JudgeRange = new List<Point>();
                if(XCnt == 0)
                {
                    // 已知的兩個點座標
                    PointD CrossPoint = new PointD(0, 0);

                    JudgeRange.Add(new Point((int)P_1_Start.X, (int)P_1_Start.Y));
                    JudgeRange.Add(new Point((int)P_2_Start.X, (int)P_2_Start.Y));
                    JudgeRange.Add(new Point((int)CrossPoint.X, (int)CrossPoint.Y));

                    DrawingPath.Add(P_1_Start);
                    DrawingPath.Add(P_2_Start);
                    DrawingPath.Add(CrossPoint);
                }
                else
                {
                    //左上右下 目前
                    PointD Cross_1_Start = new PointD(W_unit * XCnt, 0);
                    PointD Cross_1_End = new PointD(0, H_unit * XCnt);
                    //左上右下 下一條
                    PointD Cross_2_Start = new PointD(W_unit * (XCnt + 1), 0);
                    PointD Cross_2_End = new PointD(0, H_unit * (XCnt + 1));
                    ////畫線的座標要乘以m_ScaleRatio_W & m_ScaleRatio_H
                    PointD CrossPoint_1_Start = CalculateCrossPoint(Cross_1_Start, Cross_1_End, P_1_Start, P_1_End);
                    PointD CrossPoint_1_End = CalculateCrossPoint(Cross_1_Start, Cross_1_End, P_2_Start, P_2_End);
                    PointD CrossPoint_2_Start = CalculateCrossPoint(Cross_2_Start, Cross_2_End, P_1_Start, P_1_End);
                    PointD CrossPoint_2_End = CalculateCrossPoint(Cross_2_Start, Cross_2_End, P_2_Start, P_2_End);
                    JudgeRange.Add(new Point((int)CrossPoint_1_Start.X, (int)CrossPoint_1_Start.Y));
                    JudgeRange.Add(new Point((int)CrossPoint_1_End.X, (int)CrossPoint_1_End.Y));
                    JudgeRange.Add(new Point((int)CrossPoint_2_Start.X, (int)CrossPoint_2_Start.Y));
                    JudgeRange.Add(new Point((int)CrossPoint_2_End.X, (int)CrossPoint_2_End.Y));

                    DrawingPath.Add(CrossPoint_1_Start);
                    DrawingPath.Add(CrossPoint_2_Start);
                    DrawingPath.Add(CrossPoint_2_End);
                    DrawingPath.Add(CrossPoint_1_End);
                }

                var itemKey = new GraphicsPath();
                itemKey.AddPolygon(JudgeRange.ToArray());
                //4.紀錄兩條平行線上每個標記的座標範圍
                _Block tmp_Block = new _Block();
                tmp_Block.bPressed = false;
                tmp_Block.PolygonPath = new List<PointF>();
                tmp_Block.LineGroup = 0;
                tmp_Block.ID = iIDCnts++;
                foreach(var Pos in DrawingPath)
                    tmp_Block.PolygonPath.Add(new PointF((int)(Pos.X * m_ScaleRatio_W + 0.5), (int)(Pos.Y * m_ScaleRatio_H + 0.5)));
                //_Graphics.FillPolygon(_BrushesColor, tmp_Block.PolygonPath.ToArray(), FillMode.Alternate);
                Blocks.Add(itemKey, tmp_Block);
            }
        }

        private void Add_LTRB_BlockPath_2(int BlockCounts, PointD P_1_Start, PointD P_2_Start, PointD P_1_End, PointD P_2_End, double W_unit, double H_unit)
        {
            for(double YCnt = 0; YCnt < BlockCounts; YCnt++)
            {
                List<PointD> DrawingPath = new List<PointD>();
                List<Point> JudgeRange = new List<Point>();
                if(YCnt == 0)
                {
                    // 已知的兩個點座標
                    PointD CrossPoint = new PointD(m_SensorTestPaint_GBV.Panel_Var.inAAXmax, m_SensorTestPaint_GBV.Panel_Var.inAAYmax);

                    JudgeRange.Add(new Point((int)P_1_End.X, (int)P_1_End.Y));
                    JudgeRange.Add(new Point((int)P_2_End.X, (int)P_2_End.Y));
                    JudgeRange.Add(new Point((int)CrossPoint.X, (int)CrossPoint.Y));

                    DrawingPath.Add(P_1_End);
                    DrawingPath.Add(P_2_End);
                    DrawingPath.Add(CrossPoint);
                }
                else
                {
                    //左上右下 目前
                    PointD Cross_1_Start = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax, (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax - H_unit * YCnt);
                    PointD Cross_1_End = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax - W_unit * YCnt, (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax);
                    //左上右下 下一條
                    PointD Cross_2_Start = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax, (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax - H_unit * (YCnt + 1));
                    PointD Cross_2_End = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax - W_unit * (YCnt + 1), (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax);
                    ////畫線的座標要乘以m_ScaleRatio_W & m_ScaleRatio_H
                    PointD CrossPoint_1_Start = CalculateCrossPoint(Cross_1_Start, Cross_1_End, P_1_Start, P_1_End);
                    PointD CrossPoint_1_End = CalculateCrossPoint(Cross_1_Start, Cross_1_End, P_2_Start, P_2_End);
                    PointD CrossPoint_2_Start = CalculateCrossPoint(Cross_2_Start, Cross_2_End, P_1_Start, P_1_End);
                    PointD CrossPoint_2_End = CalculateCrossPoint(Cross_2_Start, Cross_2_End, P_2_Start, P_2_End);
                    JudgeRange.Add(new Point((int)CrossPoint_1_Start.X, (int)CrossPoint_1_Start.Y));
                    JudgeRange.Add(new Point((int)CrossPoint_1_End.X, (int)CrossPoint_1_End.Y));
                    JudgeRange.Add(new Point((int)CrossPoint_2_Start.X, (int)CrossPoint_2_Start.Y));
                    JudgeRange.Add(new Point((int)CrossPoint_2_End.X, (int)CrossPoint_2_End.Y));

                    DrawingPath.Add(CrossPoint_1_Start);
                    DrawingPath.Add(CrossPoint_2_Start);
                    DrawingPath.Add(CrossPoint_2_End);
                    DrawingPath.Add(CrossPoint_1_End);
                }

                var itemKey = new GraphicsPath();
                itemKey.AddPolygon(JudgeRange.ToArray());
                //4.紀錄兩條平行線上每個標記的座標範圍
                _Block tmp_Block = new _Block();
                tmp_Block.bPressed = false;
                tmp_Block.PolygonPath = new List<PointF>();
                tmp_Block.LineGroup = 0;
                tmp_Block.ID = iIDCnts++;
                foreach(var Pos in DrawingPath)
                    tmp_Block.PolygonPath.Add(new PointF((int)(Pos.X * m_ScaleRatio_W + 0.5), (int)(Pos.Y * m_ScaleRatio_H + 0.5)));
                //_Graphics.FillPolygon(_BrushesColor, tmp_Block.PolygonPath.ToArray(), FillMode.Alternate);
                Blocks.Add(itemKey, tmp_Block);
            }
        }

        private void Add_RTLB_BlockPath_1(int BlockCounts, PointD P_1_Start, PointD P_2_Start, PointD P_1_End, PointD P_2_End, double W_unit, double H_unit)
        {
            for(double XCnt = 0; XCnt < BlockCounts; XCnt++)
            {
                List<PointD> DrawingPath = new List<PointD>();
                List<Point> JudgeRange = new List<Point>();
                if(XCnt == 0)
                {
                    // 已知的兩個點座標
                    PointD CrossPoint = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax, 0);

                    JudgeRange.Add(new Point((int)P_1_Start.X, (int)P_1_Start.Y));
                    JudgeRange.Add(new Point((int)P_2_Start.X, (int)P_2_Start.Y));
                    JudgeRange.Add(new Point((int)CrossPoint.X, (int)CrossPoint.Y));

                    DrawingPath.Add(P_1_Start);
                    DrawingPath.Add(P_2_Start);
                    DrawingPath.Add(CrossPoint);
                }
                else
                {
                    //右上左下 目前
                    PointD Cross_1_Start = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax - W_unit * XCnt, 0);
                    PointD Cross_1_End = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax, H_unit * XCnt);
                    //右上左下 下一條
                    PointD Cross_2_Start = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax - W_unit * (XCnt + 1), 0);
                    PointD Cross_2_End = new PointD((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax, H_unit * (XCnt + 1));
                    ////畫線的座標要乘以m_ScaleRatio_W & m_ScaleRatio_H
                    PointD CrossPoint_1_Start = CalculateCrossPoint(Cross_1_Start, Cross_1_End, P_1_Start, P_1_End);
                    PointD CrossPoint_1_End = CalculateCrossPoint(Cross_1_Start, Cross_1_End, P_2_Start, P_2_End);
                    PointD CrossPoint_2_Start = CalculateCrossPoint(Cross_2_Start, Cross_2_End, P_1_Start, P_1_End);
                    PointD CrossPoint_2_End = CalculateCrossPoint(Cross_2_Start, Cross_2_End, P_2_Start, P_2_End);
                    JudgeRange.Add(new Point((int)CrossPoint_1_Start.X, (int)CrossPoint_1_Start.Y));
                    JudgeRange.Add(new Point((int)CrossPoint_1_End.X, (int)CrossPoint_1_End.Y));
                    JudgeRange.Add(new Point((int)CrossPoint_2_Start.X, (int)CrossPoint_2_Start.Y));
                    JudgeRange.Add(new Point((int)CrossPoint_2_End.X, (int)CrossPoint_2_End.Y));

                    DrawingPath.Add(CrossPoint_1_Start);
                    DrawingPath.Add(CrossPoint_2_Start);
                    DrawingPath.Add(CrossPoint_2_End);
                    DrawingPath.Add(CrossPoint_1_End);
                }

                var itemKey = new GraphicsPath();
                itemKey.AddPolygon(JudgeRange.ToArray());
                //4.紀錄兩條平行線上每個標記的座標範圍
                _Block tmp_Block = new _Block();
                tmp_Block.bPressed = false;
                tmp_Block.PolygonPath = new List<PointF>();
                tmp_Block.LineGroup = 1;
                tmp_Block.ID = iIDCnts++;
                foreach(var Pos in DrawingPath)
                    tmp_Block.PolygonPath.Add(new PointF((int)(Pos.X * m_ScaleRatio_W + 0.5), (int)(Pos.Y * m_ScaleRatio_H + 0.5)));
                //_Graphics.FillPolygon(_BrushesColor, tmp_Block.PolygonPath.ToArray(), FillMode.Alternate);
                Blocks.Add(itemKey, tmp_Block);
            }
        }

        private void Add_RTLB_BlockPath_2(int BlockCounts, PointD P_1_Start, PointD P_2_Start, PointD P_1_End, PointD P_2_End, double W_unit, double H_unit)
        {
            for(double YCnt = 0; YCnt < BlockCounts; YCnt++)
            {
                List<PointD> DrawingPath = new List<PointD>();
                List<Point> JudgeRange = new List<Point>();
                if(YCnt == 0)
                {
                    // 已知的兩個點座標
                    PointD CrossPoint = new PointD(0, (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax);

                    JudgeRange.Add(new Point((int)P_1_End.X, (int)P_1_End.Y));
                    JudgeRange.Add(new Point((int)P_2_End.X, (int)P_2_End.Y));
                    JudgeRange.Add(new Point((int)CrossPoint.X, (int)CrossPoint.Y));

                    DrawingPath.Add(P_1_End);
                    DrawingPath.Add(P_2_End);
                    DrawingPath.Add(CrossPoint);
                }
                else
                {
                    //左上右下 目前
                    PointD Cross_1_Start = new PointD(0, (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax - H_unit * YCnt);
                    PointD Cross_1_End = new PointD(W_unit * YCnt, (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax);
                    //左上右下 下一條
                    PointD Cross_2_Start = new PointD(0, (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax - H_unit * (YCnt + 1));
                    PointD Cross_2_End = new PointD(W_unit * (YCnt + 1), (double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax);
                    ////畫線的座標要乘以m_ScaleRatio_W & m_ScaleRatio_H
                    PointD CrossPoint_1_Start = CalculateCrossPoint(Cross_1_Start, Cross_1_End, P_1_Start, P_1_End);
                    PointD CrossPoint_1_End = CalculateCrossPoint(Cross_1_Start, Cross_1_End, P_2_Start, P_2_End);
                    PointD CrossPoint_2_Start = CalculateCrossPoint(Cross_2_Start, Cross_2_End, P_1_Start, P_1_End);
                    PointD CrossPoint_2_End = CalculateCrossPoint(Cross_2_Start, Cross_2_End, P_2_Start, P_2_End);
                    JudgeRange.Add(new Point((int)CrossPoint_1_Start.X, (int)CrossPoint_1_Start.Y));
                    JudgeRange.Add(new Point((int)CrossPoint_1_End.X, (int)CrossPoint_1_End.Y));
                    JudgeRange.Add(new Point((int)CrossPoint_2_Start.X, (int)CrossPoint_2_Start.Y));
                    JudgeRange.Add(new Point((int)CrossPoint_2_End.X, (int)CrossPoint_2_End.Y));

                    DrawingPath.Add(CrossPoint_1_Start);
                    DrawingPath.Add(CrossPoint_2_Start);
                    DrawingPath.Add(CrossPoint_2_End);
                    DrawingPath.Add(CrossPoint_1_End);
                }

                var itemKey = new GraphicsPath();
                itemKey.AddPolygon(JudgeRange.ToArray());
                //4.紀錄兩條平行線上每個標記的座標範圍
                _Block tmp_Block = new _Block();
                tmp_Block.bPressed = false;
                tmp_Block.PolygonPath = new List<PointF>();
                tmp_Block.LineGroup = 1;
                tmp_Block.ID = iIDCnts++;
                foreach(var Pos in DrawingPath)
                    tmp_Block.PolygonPath.Add(new PointF((int)(Pos.X * m_ScaleRatio_W + 0.5), (int)(Pos.Y * m_ScaleRatio_H + 0.5)));
                //_Graphics.FillPolygon(_BrushesColor, tmp_Block.PolygonPath.ToArray(), FillMode.Alternate);
                Blocks.Add(itemKey, tmp_Block);
            }
        }

        #endregion Cross-Fill Color

        #region Free Drawing
        private ResultStatus Point_CheckPassFail(MultiPaintTool_VAR PaintVar, Point _P, enTouchStatus FigStatus)
        {
            if(BtnPASS_Range.IsVisible(_P))
                return ResultStatus.PASS;
            else if(BtnFAIL_Range.IsVisible(_P))
                return ResultStatus.FAIL;
            else
                return ResultStatus.Continue;
        }

        #endregion Free Drawing

        #region Finger Detection

        private int iTestCnts = 1;
        private bool bStartCountsDown = false;

        private ResultStatus DetectFingers_CheckPassFail(MultiPaintTool_VAR PaintVar, int iFigNub, enTouchStatus FigStatus)
        {
            if(LocalVariable.PaintTool_Var.inTimeOut < 0)
            {
                return ResultStatus.FAIL;
            }
            else
            {
                if(!LocalVariable.PaintTool_Var.bDetect_FingerPass)
                    labMeg.Text = string.Format("Countdown : {0}", iTestCnts);

                if(iFigNub == StaticVar.iDetect_FingerNum)
                {
                    bStartCountsDown = true;
                    if(iTestCnts == 0)
                    {
                        LocalVariable.PaintTool_Var.bDetect_FingerPass = true;
                        FinalCheck(enPaintTestPattern.Paint_FingerDetection, ResultStatus.PASS, 0);
                    }
                    return ResultStatus.Continue;
                }
                else
                {
                    bStartCountsDown = false;
                    labMeg.Text = "";
                    iTestCnts = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inCountDown;
                    return ResultStatus.Continue;
                }
            }
        }
        #endregion

        #region Paint Key

        public void FillKey_ScreenPosition()
        {     
            Size PanelSize = new Size(this.m_panDrawing.Width,this.m_panDrawing.Height);

            double douScale_Width = (double)(PanelSize.Width - 1) / (double)(LocalVariable.Var_Multi.DevPanelInfo.X_RES - 1);
            double douScale_Height = (double)(PanelSize.Height - 1) / (double)(LocalVariable.Var_Multi.DevPanelInfo.Y_RES - 1);
            lsKeyResult = new List<bool>();
            int i_W = (int)(LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Template[0].Btn_W * douScale_Width);
            int i_H = (int)(LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Template[0].Btn_H * douScale_Height);
            int i_Key_X = (PanelSize.Width / 2) - ((i_W * LocalVariable.PaintTool_Var.Key_Scale / 2) * LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts);
            int i_Key_Y = (PanelSize.Height / 4);
          
            for (int inCount = 0; inCount < UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts; inCount++)
            {
                lsKeyResult.Add(false);
                LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Template[inCount]._FingerID = inCount + 1;
                LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition[inCount].Start_Point = new Point(i_Key_X + (inCount * i_W * LocalVariable.PaintTool_Var.Key_Scale), i_Key_Y);
                LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition[inCount].Btn_H = i_H * LocalVariable.PaintTool_Var.Key_Scale;
                LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition[inCount].Btn_W = i_W * LocalVariable.PaintTool_Var.Key_Scale;
            }
        }

        public void PaintKeyReset()
        {
            this.m_panDrawing.BackColor = Color.White;
            m_panDrawing.Refresh();
            if (LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts <= 0)
                return;
     
            FillKey_ScreenPosition();

            SolidBrush _BrushesColor = new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_TestKey);
                                   
            int iKeyCount =0;
            Graphics grp = m_panDrawing.CreateGraphics();
            foreach (var item in LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition)
            {
                Rectangle rect1 = new Rectangle(item.Start_Point, new Size(item.Btn_W, item.Btn_H));
                string sKeyTxt = string.Format("Key{0}", iKeyCount++);
                Font KeyFont = GDI_Panel.FindFont(Graphics.FromHwnd(m_panDrawing.Handle), sKeyTxt, rect1.Size, arialBold, FontStyle.Regular);
                
                SizeF string_size =  grp.MeasureString(sKeyTxt,KeyFont);


                Point CenterPos_2 = new Point(item.Start_Point.X + ((item.Btn_W - (int)string_size.Width) / 2), item.Start_Point.Y + ((item.Btn_H - (int)string_size.Height) / 2));
               

                GDI_Panel.FillRoundRectangle(Graphics.FromHwnd(this.m_panDrawing.Handle), _BrushesColor,
                                  rect1, 8);
                GDI_Panel.DrawRoundRectangle(Graphics.FromHwnd(this.m_panDrawing.Handle), Pens.Black,
                                 rect1, 7);

                SetPaintText(this.m_panDrawing, sKeyTxt, CenterPos_2, Color.Black, KeyFont);
            }
                           
        }

        #endregion

        private void InitLineStatus()
        {
            bDrawLineDisconnect = false;
            bLastTimeFail = false;
            if(NowTestLineStatus.Count() == 0)
                return;
            if(LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_PaintCross)
            {
                for(int i = 0; i < 1; i++)
                {
                    NowTestLineStatus[i].IsClickStart = enLineStatus.Wait;
                    NowTestLineStatus[i].IsContiune = enLineStatus.Wait;
                    NowTestLineStatus[i].IsClickEnd = enLineStatus.Wait;
                    NowTestLineStatus[i].IsOverRange = enLineStatus.Wait;
                    NowTestLineStatus[i].inFigNub = -1;
                }
            }
            else
            {
                for(int i = 0; i < NowTestLineStatus.Length; i++)
                {
                    NowTestLineStatus[i].IsClickStart = enLineStatus.Wait;
                    NowTestLineStatus[i].IsContiune = enLineStatus.Wait;
                    NowTestLineStatus[i].IsClickEnd = enLineStatus.Wait;
                    NowTestLineStatus[i].IsOverRange = enLineStatus.Wait;
                    NowTestLineStatus[i].inFigNub = -1;
                }
            }
        }

        private void Reset_stuTestLineStatus(enPaintTestPattern _NowTestPattern)
        {
            m_bFirstPosition = true;
            m_bAllBlocksPressed = false;
            switch(_NowTestPattern)
            {
                case enPaintTestPattern.Paint_PaintRectangle:
                case enPaintTestPattern.Paint_PenRectangle: //20220718 Steven
                    if(_NowTestPattern == enPaintTestPattern.Paint_PenRectangle)
                    {
                        StaticVar.iRectFingerNum = 1;
                    }
                    else
                    {
                        INI_Tool MyINI = new INI_Tool(LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                        StaticVar.iRectFingerNum = MyINI.IniReadInt("Paint_PaintRectangle", "FingerNumber", 1);
                    }
                    if(StaticVar.iRectFingerNum == 2 && LocalVariable.PaintTool_Var.lsRectangleLineSetting.Count != 4)
                        NowTestLineStatus = new stuLineTest[2];
                    else
                        NowTestLineStatus = new stuLineTest[LocalVariable.PaintTool_Var.lsRectangleLineSetting.Count];
                    break;

                case enPaintTestPattern.Paint_PaintRectangle3:
                case enPaintTestPattern.Paint_PenRectangle3: //20220718 Steven
                    if(_NowTestPattern == enPaintTestPattern.Paint_PenRectangle3)                    
                        StaticVar.iRect3FingerNum = 1;
                    
                    if(StaticVar.iRect3FingerNum == 2)
                        NowTestLineStatus = new stuLineTest[12];
                    else
                        NowTestLineStatus = new stuLineTest[LocalVariable.PaintTool_Var.lsRectangle3LineSetting.Count];

                    break;

                case enPaintTestPattern.Paint_PaintCross:
                case enPaintTestPattern.Paint_PenCross:
                case enPaintTestPattern.Paint_PaintCross3:
                case enPaintTestPattern.Paint_PenCross3:
                    NowTestLineStatus = new stuLineTest[LocalVariable.PaintTool_Var.lsCrossLineSetting.Count];
                    break;

                default:
                    NowTestLineStatus = new stuLineTest[0];
                    break;
            }

            for(int i = 0; i < NowTestLineStatus.Length; i++)
                NowTestLineStatus[i] = new stuLineTest(new Point(-1, -1), new Point(-1, -1), -1, -1, enLineStatus.Wait, enLineStatus.Wait, enLineStatus.Wait);
        }

        private int FirstPoint(enPaintTestPattern _Pattern, Point _point)
        {
            int inRegisteredCount = 0;
            for(int inCount = 0; inCount < NowTestLineStatus.Length; inCount++)
            {
                if(NowTestLineStatus[inCount].inFigNub != -1)
                {
                    inRegisteredCount++;
                    continue;
                }
                //將顏色塗上去起始的區塊
                switch(_Pattern)
                {
                    case enPaintTestPattern.Paint_PaintCross:
                    case enPaintTestPattern.Paint_PenCross:
                        if(CrossCornerBlock[0].Contains(_point))
                        {
                            // Registered and Set Start Bubble
                            NowTestLineStatus[inCount].inFigNub = inCount;
                            NowTestLineStatus[inCount].IsContiune = enLineStatus._true;//預設這次劃線不會有中斷現象
                            NowTestLineStatus[inCount].IsClickStart = enLineStatus._true;
                            inRegisteredCount++;

                            IntPtr inpPenBrush = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                            GDI_Panel.AssignPenHandle(inpPenBrush);
                            GDI_Panel.Rectangle(m_StartBlock[0], m_StartBlock[1]);
                            GDI_Panel.DeleteOBJECT(inpPenBrush);
                        }
                        break;

                    case enPaintTestPattern.Paint_PaintCross3:
                    case enPaintTestPattern.Paint_PenCross3:
                        for(int i = 0; i < CrossCornerBlock.Length; i++)
                        {
                            if(CrossCornerBlock[i].Contains(_point))
                            {
                                int iCount = (i < 2) ? 0 : 1;

                                iRecordFirstPointIndex = i;
                                // Registered and Set Start Bubble
                                NowTestLineStatus[iCount].inFigNub = 0;
                                NowTestLineStatus[iCount].IsContiune = enLineStatus._true;//預設這次劃線不會有中斷現象
                                NowTestLineStatus[iCount].IsClickStart = enLineStatus._true;
                                inRegisteredCount++;

                                IntPtr inpPenBrush = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.Color_tmpPass);
                                GDI_Panel.AssignPenHandle(inpPenBrush);
                                GDI_Panel.Rectangle(dic_StartPoint[i][0], dic_StartPoint[i][1]);
                                GDI_Panel.DeleteOBJECT(inpPenBrush);
                            }
                        }
                        break;

                    case enPaintTestPattern.Paint_PaintRectangle:
                    case enPaintTestPattern.Paint_PenRectangle:
                        double _VectorValue = (double)Math.Sqrt(Math.Pow(NowTestLineStatus[inCount].Start.X - _point.X, 2) + Math.Pow(NowTestLineStatus[inCount].Start.Y - _point.Y, 2));
                        if(NowTestLineStatus[inCount].dRadius >= _VectorValue)
                        {
                            // Registered and Set Start Bubble
                            NowTestLineStatus[inCount].inFigNub = inCount;
                            NowTestLineStatus[inCount].IsContiune = enLineStatus._true;//預設這次劃線不會有中斷現象
                            NowTestLineStatus[inCount].IsClickStart = enLineStatus._true;
                            inRegisteredCount++;

                            IntPtr inpPenBrush = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                            GDI_Panel.AssignPenHandle(inpPenBrush);
                            GDI_Panel.Ellipse((int)(((double)NowTestLineStatus[inCount].Start.X - NowTestLineStatus[inCount].dRadius) * m_ScaleRatio_W),
                                      (int)(((double)NowTestLineStatus[inCount].Start.Y + NowTestLineStatus[inCount].dRadius) * m_ScaleRatio_H),
                                      (int)(((double)NowTestLineStatus[inCount].Start.X + NowTestLineStatus[inCount].dRadius) * m_ScaleRatio_W),
                                      (int)(((double)NowTestLineStatus[inCount].Start.Y - NowTestLineStatus[inCount].dRadius) * m_ScaleRatio_H));
                            GDI_Panel.DeleteOBJECT(inpPenBrush);
                        }
                        break;

                    case enPaintTestPattern.Paint_PaintRectangle3:
                    case enPaintTestPattern.Paint_PenRectangle3:
                        _VectorValue = (double)Math.Sqrt(Math.Pow(NowTestLineStatus[inCount].Start.X - _point.X, 2) + Math.Pow(NowTestLineStatus[inCount].Start.Y - _point.Y, 2));

                        if(inCount == 0)
                        {
                            if(!(_point.X < (NowTestLineStatus[inCount].Start.X + NowTestLineStatus[inCount].dWidth / 2) && _point.Y < (NowTestLineStatus[inCount].Start.Y + NowTestLineStatus[inCount].dHeight / 2)))
                                continue;
                        }
                        else if(inCount == 1)
                        {
                            if(!(_point.X > (NowTestLineStatus[inCount].Start.X - NowTestLineStatus[inCount].dWidth / 2) && _point.Y < (NowTestLineStatus[inCount].Start.Y + NowTestLineStatus[inCount].dHeight / 2)))
                                continue;
                        }
                        else if(inCount == 2)
                        {
                            if(!(_point.X > (NowTestLineStatus[inCount].Start.X - NowTestLineStatus[inCount].dWidth / 2) && _point.Y > (NowTestLineStatus[inCount].Start.Y - NowTestLineStatus[inCount].dHeight / 2)))
                                continue;
                        }
                        else if(inCount == 3)
                        {
                            if(!(_point.X < (NowTestLineStatus[inCount].Start.X + NowTestLineStatus[inCount].dWidth / 2) && _point.Y > (NowTestLineStatus[inCount].Start.Y - NowTestLineStatus[inCount].dHeight / 2)))
                                continue;
                        }
                        {
                            iRecordFirstPointIndex = inCount;
                            iPrevPointRecord = inCount;
                            // Registered and Set Start Bubble
                            NowTestLineStatus[inCount].inFigNub = 0;
                            NowTestLineStatus[inCount].IsContiune = enLineStatus._true;//預設這次劃線不會有中斷現象
                            NowTestLineStatus[inCount].IsClickStart = enLineStatus._true;
                            inRegisteredCount++;

                            Graphics grp = m_panDrawing.CreateGraphics();
                            grp.FillRectangle(new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_tmpPass),
                                                   (int)((NowTestLineStatus[inCount].Start.X - NowTestLineStatus[inCount].dWidth / 2) * m_ScaleRatio_W),
                                                   (int)((NowTestLineStatus[inCount].Start.Y - NowTestLineStatus[inCount].dHeight / 2) * m_ScaleRatio_H),
                                                   (int)(NowTestLineStatus[inCount].dWidth * m_ScaleRatio_W),
                                                   (int)(NowTestLineStatus[inCount].dHeight * m_ScaleRatio_H));
                        }
                        break;
                }
            }
            return inRegisteredCount;
        }

        private int FirstPoint(enPaintTestPattern _Pattern, int iFigNub, List<Point> _pointList)
        {
            int inRegisteredCount = 0;
            for(int inCount = 0; inCount < NowTestLineStatus.Length; inCount++)
            {
                if(NowTestLineStatus[0].inFigNub != -1 && NowTestLineStatus[1].inFigNub != -1)
                {
                    inRegisteredCount++;
                    continue;
                }
                //將顏色塗上去起始的區塊
                switch(_Pattern)
                {
                    case enPaintTestPattern.Paint_PaintRectangle:
                        double _VectorValue1 = (double)Math.Sqrt(Math.Pow(NowTestLineStatus[0].Start.X - _pointList[inCount].X, 2) + Math.Pow(NowTestLineStatus[0].Start.Y - _pointList[inCount].Y, 2));
                        double _VectorValue2 = (double)Math.Sqrt(Math.Pow(NowTestLineStatus[1].Start.X - _pointList[inCount].X, 2) + Math.Pow(NowTestLineStatus[1].Start.Y - _pointList[inCount].Y, 2));

                        if(NowTestLineStatus[0].dRadius >= _VectorValue1)
                        {
                            // Registered and Set Start Bubble
                            NowTestLineStatus[0].inFigNub = 0;
                            NowTestLineStatus[0].IsContiune = enLineStatus._true;//預設這次劃線不會有中斷現象
                            NowTestLineStatus[0].IsClickStart = enLineStatus._true;
                            inRegisteredCount++;

                            IntPtr inpPenBrush = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                            GDI_Panel.AssignPenHandle(inpPenBrush);
                            GDI_Panel.Ellipse((int)(((double)NowTestLineStatus[0].Start.X - NowTestLineStatus[0].dRadius) * m_ScaleRatio_W),
                                      (int)(((double)NowTestLineStatus[0].Start.Y + NowTestLineStatus[0].dRadius) * m_ScaleRatio_H),
                                      (int)(((double)NowTestLineStatus[0].Start.X + NowTestLineStatus[0].dRadius) * m_ScaleRatio_W),
                                      (int)(((double)NowTestLineStatus[0].Start.Y - NowTestLineStatus[0].dRadius) * m_ScaleRatio_H));
                            GDI_Panel.DeleteOBJECT(inpPenBrush);
                        }
                        else if(NowTestLineStatus[1].dRadius >= _VectorValue2)
                        {
                            NowTestLineStatus[1].inFigNub = 1;
                            NowTestLineStatus[1].IsContiune = enLineStatus._true;//預設這次劃線不會有中斷現象
                            NowTestLineStatus[1].IsClickStart = enLineStatus._true;
                            inRegisteredCount++;

                            IntPtr inpPenBrush = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.Color_Pass);
                            GDI_Panel.AssignPenHandle(inpPenBrush);
                            GDI_Panel.Ellipse((int)(((double)NowTestLineStatus[1].Start.X - NowTestLineStatus[1].dRadius) * m_ScaleRatio_W),
                                      (int)(((double)NowTestLineStatus[1].Start.Y + NowTestLineStatus[1].dRadius) * m_ScaleRatio_H),
                                      (int)(((double)NowTestLineStatus[1].Start.X + NowTestLineStatus[1].dRadius) * m_ScaleRatio_W),
                                      (int)(((double)NowTestLineStatus[1].Start.Y - NowTestLineStatus[1].dRadius) * m_ScaleRatio_H));
                            GDI_Panel.DeleteOBJECT(inpPenBrush);
                        }

                        break;

                    case enPaintTestPattern.Paint_PaintRectangle3:
                        int iFinger = 0;
                        bool isTouch0Section = AllRectSection[inCount].Contains(_pointList[0]);
                        bool isTouch1Section = AllRectSection[inCount].Contains(_pointList[1]);
                        bool isTouchSection = isTouch0Section || isTouch1Section;

                        if(isTouchSection)
                        {
                            iRecordFirstPointIndex = inCount;
                            iPrevPointRecord = inCount;
                            // Registered and Set Start Bubble
                            iFinger = isTouch0Section ? 0 : 1;
                            NowTestLineStatus[inCount].inFigNub = iFinger;
                            NowTestLineStatus[inCount].IsContiune = enLineStatus._true;//預設這次劃線不會有中斷現象
                            NowTestLineStatus[inCount].IsClickStart = enLineStatus._true;
                            inRegisteredCount++;

                            Graphics grp = m_panDrawing.CreateGraphics();
                            //GDI_Panel.DeleteOBJECT(inpPenBrush);
                            grp.FillRectangle(new SolidBrush(LocalVariable.PaintTool_Var._ComColor.Color_tmpPass),
                                 (int)((NowTestLineStatus[inCount].Start.X - NowTestLineStatus[inCount].dWidth / 2) * m_ScaleRatio_W),
                                 (int)((NowTestLineStatus[inCount].Start.Y - NowTestLineStatus[inCount].dHeight / 2) * m_ScaleRatio_H),
                                 (int)(NowTestLineStatus[inCount].dWidth * m_ScaleRatio_W),
                                 (int)(NowTestLineStatus[inCount].dHeight * m_ScaleRatio_H));
                        }

                        break;
                }
            }
            return inRegisteredCount;
        }

        private void SetSideColor(enPaintTestPattern _Pattern, params object[] para)
        {
            List<Color> _SetColor = new List<Color>();
            foreach(var _Color in para)
                _SetColor.Add((Color)_Color);

            IntPtr _PenColor = IntPtr.Zero;
            switch(_Pattern)
            {
                case enPaintTestPattern.Paint_PenCross2:
                case enPaintTestPattern.Paint_PaintCross2:
                    var TestRangePercent = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryCross.RangeWidth * 100);
                    var TheMinimumRes = Math.Min(m_SensorTestPaint_GBV.Panel_Var.inAAXmax, m_SensorTestPaint_GBV.Panel_Var.inAAYmax);
                    var W_unit = ((double)m_SensorTestPaint_GBV.Panel_Var.inAAXmax / TheMinimumRes) * TestRangePercent;
                    var H_unit = ((double)m_SensorTestPaint_GBV.Panel_Var.inAAYmax / TheMinimumRes) * TestRangePercent;
                    _PenColor = GDI_Panel.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 2, _SetColor[0]);
                    GDI_Panel.AssignPenHandle(_PenColor);

                    var P_UpperR = new Point(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmin + (int)W_unit, m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmin);
                    var P_LowerR = new Point(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax, m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax - (int)H_unit);
                    GDI_Panel.MoveTo((int)((double)P_UpperR.X * m_ScaleRatio_W + 0.5), (int)((double)P_UpperR.Y * m_ScaleRatio_H + 0.5));
                    GDI_Panel.LineTo((int)((double)P_LowerR.X * m_ScaleRatio_W + 0.5), (int)((double)P_LowerR.Y * m_ScaleRatio_H + 0.5));
                    var P_UpperL = new Point(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmin, m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmin + (int)H_unit);
                    var P_LowerL = new Point(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax - (int)W_unit, m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax);
                    GDI_Panel.MoveTo((int)((double)P_UpperL.X * m_ScaleRatio_W + 0.5), (int)((double)P_UpperL.Y * m_ScaleRatio_H + 0.5));
                    GDI_Panel.LineTo((int)((double)P_LowerL.X * m_ScaleRatio_W + 0.5), (int)((double)P_LowerL.Y * m_ScaleRatio_H + 0.5));

                    //右上->左下
                    P_UpperL = new Point((int)(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax - W_unit + 0.5), m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmin);
                    P_LowerL = new Point(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmin, (int)(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax - H_unit + 0.5));
                    GDI_Panel.MoveTo((int)((double)P_UpperL.X * m_ScaleRatio_W + 0.5), (int)((double)P_UpperL.Y * m_ScaleRatio_H + 0.5));
                    GDI_Panel.LineTo((int)((double)P_LowerL.X * m_ScaleRatio_W + 0.5), (int)((double)P_LowerL.Y * m_ScaleRatio_H + 0.5));
                    P_UpperR = new Point(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax, (int)(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmin + H_unit + 0.5));
                    P_LowerR = new Point((int)(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmin + W_unit + 0.5), m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax);
                    GDI_Panel.MoveTo((int)((double)P_UpperR.X * m_ScaleRatio_W + 0.5), (int)((double)P_UpperR.Y * m_ScaleRatio_H + 0.5));
                    GDI_Panel.LineTo((int)((double)P_LowerR.X * m_ScaleRatio_W + 0.5), (int)((double)P_LowerR.Y * m_ScaleRatio_H + 0.5));
                    break;

                case enPaintTestPattern.Paint_PenRectangle2:
                case enPaintTestPattern.Paint_PaintRectangle2:
                    _PenColor = GDI_Panel.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 2, _SetColor[0]);
                    GDI_Panel.AssignPenHandle(_PenColor);
                    var W_Max = (int)((m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax) * m_ScaleRatio_W) - 1;
                    var H_Max = (int)((m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax) * m_ScaleRatio_H) - 1;
                    //外框
                    DrawRectangle(0, 0, W_Max, H_Max);
                    DrawRectangle((int)(InsideRectangle[0].X * m_ScaleRatio_W), (int)(InsideRectangle[0].Y * m_ScaleRatio_H)
                    , (int)((InsideRectangle[3].X) * m_ScaleRatio_W)
                    , (int)((InsideRectangle[3].Y) * m_ScaleRatio_H));
                    break;

                case enPaintTestPattern.Paint_PaintCross:
                case enPaintTestPattern.Paint_PenCross:
                case enPaintTestPattern.Paint_PaintCross3:
                case enPaintTestPattern.Paint_PenCross3:
                    CrossPatternReset(_SetColor[0], _SetColor[1], _SetColor[2]);
                    break;

                case enPaintTestPattern.Paint_PaintRectangle:
                case enPaintTestPattern.Paint_PenRectangle:
                case enPaintTestPattern.Paint_PaintRectangle3:
                case enPaintTestPattern.Paint_PenRectangle3:
                    RectanglePatternReset(_SetColor[0], _SetColor[1], _SetColor[2]);
                    break;
            }
        }

        private void PopResultMessage()
        {
            TestTimer.Stop();
            m_bTestRunning = false;
            MyUIPage.ShowSuccessDialog("", "PASS", UIStyle.Green, true);
        }

        //finger_idx的設定是為了避免若有一指以上的劃線失敗，會造成一次以上的retry次數減少，用索引判定retry次數減少時機是在索引0的時候才觸發。
        private void FinalCheck(enPaintTestPattern PatternType, ResultStatus _ResultStatus, int finger_idx)
        {
            if(_ResultStatus == ResultStatus.Continue)
                return;
            TestTimer.Stop();
            m_bTestRunning = false;
            LocalVariable.VarSenTest_Multi.m_OnRepaintEvent = true;
            switch(PatternType)
            {
                default:
                    if(_ResultStatus == ResultStatus.PASS)
                    {
                        if(StaticVar.bDrawingShowPass)
                            PopResultMessage();
                        if(LocalVariable.PaintTool_Var.MuHandle)
                            Set_DrawingTestResult(ResultStatus.PASS);
                    }
                    else if(_ResultStatus == ResultStatus.FAIL)
                    {
                        LocalVariable.PaintTool_Var.m_bRetryFail = true;
                        if(LocalVariable.PaintTool_Var.MuHandle)
                            Set_DrawingTestResult(ResultStatus.FAIL);
                    }
                    break;

                //20220516 Vane++ 下面這一段要再確認!!!
                case enPaintTestPattern.Paint_PaintKey:
                    if(PatternType == enPaintTestPattern.Paint_PenRectangle)
                        StaticVar.iRectFingerNum = 1;
                    int inPassCount = 0;
                    for(int inCount = 0; inCount < NowTestLineStatus.Length; inCount++)
                    {
                        bool bResult = (NowTestLineStatus[inCount].inFigNub > -1);// 1. check FigNub
                        bResult &= (NowTestLineStatus[inCount].IsClickStart == enLineStatus._true);// 2. check Start bubble
                        bResult &= (NowTestLineStatus[inCount].IsOverRange == enLineStatus._false);// 3. check Line Status
                        bResult &= (bDrawLineDisconnect == false);//4. check line continue
                        bResult &= (NowTestLineStatus[inCount].IsClickEnd == enLineStatus._true);// 5. check End Bubble
                        bResult &= (LocalVariable.VarPaint_Multi.iFingerCount <= StaticVar.iRectFingerNum);
                        if(bResult)
                            inPassCount++;
                    }

                    // Pass
                    if(inPassCount == NowTestLineStatus.Length)
                    {
                        if(LocalVariable.PaintTool_Var.MuHandle)
                        {
                            if(StaticVar.bDrawingShowPass)
                                PopResultMessage();

                            Set_DrawingTestResult(ResultStatus.PASS);
                        }
                    }
                    else// Fail retry
                    {
                        if(LocalVariable.PaintTool_Var.inRetry > 0)
                        {
                            LocalVariable.PaintTool_Var.inRetry--;
                            Reset_stuTestLineStatus(LocalVariable.PaintTool_Var.g_NowTestPattern);
                            string str = string.Format("{0} / {1}", LocalVariable.PaintTool_Var.inRetryUplimit - LocalVariable.PaintTool_Var.inRetry, LocalVariable.PaintTool_Var.inRetryUplimit);
                            vfSetlabMegText(ErrMsg.SensorTest.RetryTimes, str);
                        }
                        else
                        {
                            vfSetlabMegText(ErrMsg.SensorTest.RetryOverSpec);
                            LocalVariable.PaintTool_Var.m_bRetryFail = true;
                            if(LocalVariable.PaintTool_Var.MuHandle)
                                Set_DrawingTestResult(ResultStatus.FAIL);
                        }
                        m_panDrawing.Invalidate();
                    }
                    InitLineStatus();
                    break;
            }
        }

        private void InitVirtualButton()
        {
            m_ScaleFW_PointRatio_H = (double)(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax - m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmin + 1) / (double)(m_panDrawing.Height - 1);
            m_ScaleFW_PointRatio_W = (double)(m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax - m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmin + 1) / (double)(m_panDrawing.Width - 1);

            IntPtr BrushHandle = IntPtr.Zero;
            BrushHandle = GDI_Panel.CreateBrushHandle(LocalVariable.PaintTool_Var._ComColor.Color_Rectangle);
            GDI_Panel.AssignPenHandle(BrushHandle);

            int pos_H = (this.m_panDrawing.Height * 2) / 3;
            int pos_W = this.m_panDrawing.Width / 2;
            int iH = (int)((m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax / 10) * m_ScaleRatio_H);
            int iW = (int)((m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax / 10) * m_ScaleRatio_W);

            //==================================================
            var VirtualBtn = new MultiPaintTool_VAR._VirtualBtn();
            VirtualBtn.Text = "PASS";
            VirtualBtn.bVisible = true;
            VirtualBtn.iHeight = (int)(iH);
            VirtualBtn.iWidth = (int)(iW);
            VirtualBtn.StartPos = new Point(pos_W - (VirtualBtn.iWidth * 3) - (VirtualBtn.iWidth / 2), pos_H);
            VirtualBtn.EndPos = new Point(VirtualBtn.StartPos.X + VirtualBtn.iWidth, VirtualBtn.StartPos.Y + VirtualBtn.iHeight);

            GDI_Panel.Rectangle(VirtualBtn.StartPos, VirtualBtn.EndPos);
            Point CenterPos = new Point(VirtualBtn.StartPos.X + VirtualBtn.iWidth / 4, VirtualBtn.StartPos.Y + VirtualBtn.iHeight / 2);

            BtnPASS_Range.Reset();
            BtnPASS_Range.AddPolygon(
                new Point[]
                {
                    new Point((int)(VirtualBtn.StartPos.X * m_ScaleFW_PointRatio_W) , (int)(VirtualBtn.StartPos.Y * m_ScaleFW_PointRatio_H)),
                    new Point((int)((VirtualBtn.StartPos.X + VirtualBtn.iWidth) * m_ScaleFW_PointRatio_W), (int)(VirtualBtn.StartPos.Y * m_ScaleFW_PointRatio_H)),
                    new Point((int)((VirtualBtn.StartPos.X + VirtualBtn.iWidth) * m_ScaleFW_PointRatio_W), (int)((VirtualBtn.StartPos.Y + VirtualBtn.iHeight) * m_ScaleFW_PointRatio_H)),
                    new Point((int)(VirtualBtn.StartPos.X * m_ScaleFW_PointRatio_W) , (int)((VirtualBtn.StartPos.Y + VirtualBtn.iHeight) * m_ScaleFW_PointRatio_H))
                });

            Rectangle rect1 = new Rectangle(CenterPos.X, CenterPos.Y, VirtualBtn.iWidth / 2, VirtualBtn.iHeight / 2);
            Font goodFont = GDI_Panel.FindFont(Graphics.FromHwnd(m_panDrawing.Handle), VirtualBtn.Text, rect1.Size, arialBold);
            SetPaintText(this.m_panDrawing, VirtualBtn.Text, CenterPos, Color.Black, goodFont);

            //==================================================
            VirtualBtn = new MultiPaintTool_VAR._VirtualBtn();
            VirtualBtn.Text = "FAIL";
            VirtualBtn.bVisible = true;
            VirtualBtn.iHeight = (int)(iH);
            VirtualBtn.iWidth = (int)(iW);
            VirtualBtn.StartPos = new Point(pos_W + (VirtualBtn.iWidth * 3) - (VirtualBtn.iWidth / 2), pos_H);
            VirtualBtn.EndPos = new Point(VirtualBtn.StartPos.X + VirtualBtn.iWidth, VirtualBtn.StartPos.Y + VirtualBtn.iHeight);

            GDI_Panel.Rectangle(VirtualBtn.StartPos, VirtualBtn.EndPos);
            Point CenterPos_2 = new Point(VirtualBtn.StartPos.X + VirtualBtn.iWidth / 4, VirtualBtn.StartPos.Y + VirtualBtn.iHeight / 2);

            BtnFAIL_Range.Reset();
            BtnFAIL_Range.AddPolygon(new Point[] {
                new Point((int)(VirtualBtn.StartPos.X * m_ScaleFW_PointRatio_W) , (int)(VirtualBtn.StartPos.Y * m_ScaleFW_PointRatio_H)),
                new Point((int)((VirtualBtn.StartPos.X + VirtualBtn.iWidth) * m_ScaleFW_PointRatio_W), (int)(VirtualBtn.StartPos.Y * m_ScaleFW_PointRatio_H)),
                new Point((int)((VirtualBtn.StartPos.X + VirtualBtn.iWidth) * m_ScaleFW_PointRatio_W), (int)((VirtualBtn.StartPos.Y + VirtualBtn.iHeight) * m_ScaleFW_PointRatio_H)),
                new Point((int)(VirtualBtn.StartPos.X * m_ScaleFW_PointRatio_W) , (int)((VirtualBtn.StartPos.Y + VirtualBtn.iHeight) * m_ScaleFW_PointRatio_H))
            });

            Rectangle rect2 = new Rectangle(CenterPos_2.X, CenterPos_2.Y, VirtualBtn.iWidth / 2, VirtualBtn.iHeight / 2);
            goodFont = GDI_Panel.FindFont(Graphics.FromHwnd(m_panDrawing.Handle), VirtualBtn.Text, rect1.Size, arialBold);
            SetPaintText(this.m_panDrawing, VirtualBtn.Text, CenterPos_2, Color.Black, goodFont);
            GDI_Panel.DeleteOBJECT(BrushHandle);
        }

        private void SetPaintText(Panel inPanel, string str, Point Position, Color FontColor, Font _Font)
        {
            Graphics grp = inPanel.CreateGraphics();
            Mutex _mutex_ = new Mutex();
            _mutex_.WaitOne();
            if(FontColor == Color.White)
            {
                GDI_Panel.Rectangle(Position, new Point(Position.X + 200, Position.Y + 15));
            }
            else
            {
                Font _arialBold = (_Font == null) ? new Font("Consolas", 9.0F, FontStyle.Regular) : _Font;
                TextRenderer.DrawText(grp, str, _arialBold, Position, FontColor, TextFormatFlags.EndEllipsis);
            }
            grp.Dispose();
            if(_mutex_ != null)
                _mutex_.ReleaseMutex();
        }

        //=======================================
        private void PatternKey_Init()
        {
            int inGap = 50;
            int MaxKeyinLine = 4;
            int inDrawingXMin = m_SensorTestPaint_GBV.Panel_Var.inAAXmin;
            int inDrawingYMin = m_SensorTestPaint_GBV.Panel_Var.inAAYmin;

            int inDrewingXMax = m_SensorTestPaint_GBV.Panel_Var.inAAXmax;
            int inDrewingYMax = m_SensorTestPaint_GBV.Panel_Var.inAAYmax;

            if(m_SensorTestPaint_GBV.Panel_Var.inKeyNumber <= 0)
                return;

            int inKeyCol = m_SensorTestPaint_GBV.Panel_Var.inKeyNumber > MaxKeyinLine ? MaxKeyinLine : m_SensorTestPaint_GBV.Panel_Var.inKeyNumber;
            int inKeyRow = m_SensorTestPaint_GBV.Panel_Var.inKeyNumber % inKeyCol > 0 ? (m_SensorTestPaint_GBV.Panel_Var.inKeyNumber / inKeyCol) + 1 : m_SensorTestPaint_GBV.Panel_Var.inKeyNumber / inKeyCol;
            double douRecWidth = 0;
            double douRecHeight = 0;

            douRecWidth = (double)(inDrewingXMax - inDrawingXMin - ((inKeyCol + 1) * inGap)) / inKeyCol;
            douRecHeight = (double)(inDrewingYMax - inDrawingYMin - ((inKeyRow + 1) * inGap)) / inKeyRow;

            douRecWidth = Math.Min(douRecWidth, douRecHeight);

            lsRectPoint.Clear();

            for(int inRowCount = 0; inRowCount < inKeyRow; inRowCount++)
            {
                for(int inColCount = 0; inColCount < inKeyCol; inColCount++)
                {
                    int inX1 = (inGap * (inColCount + 1)) + (int)(douRecWidth * inColCount);
                    int inX2 = (inGap * (inColCount + 1)) + (int)(douRecWidth * (inColCount + 1));
                    int inY1 = (inGap * (inRowCount + 1)) + (int)(douRecWidth * inRowCount);
                    int inY2 = (inGap * (inRowCount + 1)) + (int)(douRecWidth * (inRowCount + 1));

                    lsRectPoint.Add(new _RecPOINT(inX1, inY1, inX2, inY2));
                }
            }

            if(lsRectPoint.Count > m_SensorTestPaint_GBV.Panel_Var.inKeyNumber)
                lsRectPoint.RemoveRange(m_SensorTestPaint_GBV.Panel_Var.inKeyNumber, lsRectPoint.Count - m_SensorTestPaint_GBV.Panel_Var.inKeyNumber);

            PatternKey_Drawing();
        }

        private void PatternKey_Drawing()
        {
            for(int inCount = 0; inCount < lsRectPoint.Count; inCount++)
            {
                GDI_Panel.SetROP2(Gdi32.drawingMode.R2_COPY_PEN);
                if(lsRectPoint[inCount].bStatus)
                {
                    GDI_Panel.AssignPenHandle(m_Pen_Handle[enPaintPenStyle.KeyPass]);
                }
                else
                {
                    var iKeyType = (lsRectPoint[inCount].tcsTouchStatus == TouchStatus.Default) ? enPaintPenStyle.KeyDef : enPaintPenStyle.KeyTouch;
                    GDI_Panel.AssignPenHandle(m_Pen_Handle[iKeyType]);
                }
                GDI_Panel.Rectangle((int)(lsRectPoint[inCount].X1 * m_ScaleRatio_W),
                                    (int)(lsRectPoint[inCount].Y1 * m_ScaleRatio_H),
                                    (int)(lsRectPoint[inCount].X2 * m_ScaleRatio_W),
                                    (int)(lsRectPoint[inCount].Y2 * m_ScaleRatio_H));
            }
        }

        //private void DrawingKey()
        //{
        //    var PaintVar = LocalVariable.VarPaint_Multi;
        //    if(PaintVar.bKeyChanged)
        //    {
        //        for(int inCount = 0; inCount < m_SensorTestPaint_GBV.Panel_Var.inKeyNumber; inCount++)
        //        {
        //            if(PaintVar.strKey_New[inCount].bTouch == false)
        //            {
        //                // Pass
        //                PaintVar.strKey_Old[inCount].bTouch = false;
        //                PaintVar.strKey_New[inCount].bTouch = false;
        //            }
        //            else
        //            {
        //                // Touch Down
        //                PaintVar.strKey_Old[inCount].bTouch = PaintVar.strKey_New[inCount].bTouch;
        //                PaintVar.strKey_New[inCount].bTouch = false;
        //            }
        //        }
        //    }
        //    for(int inCount = 0; inCount < m_SensorTestPaint_GBV.Panel_Var.inKeyNumber; inCount++)
        //    {
        //        TouchStatus _status = (PaintVar.strKey_Old[inCount].bTouch) ? TouchStatus.Down : TouchStatus.Up;
        //        PatternKey_CheckPassFail(inCount, _status);
        //    }
        //}

        private void PatternKey_CheckPassFail(int inIndex, TouchStatus enTouchStatus)
        {
            bool boRedraw = false;
            _RecPOINT RecPonitBuff = new _RecPOINT();

            if((enTouchStatus == TouchStatus.Down) && (lsRectPoint[inIndex].tcsTouchStatus == TouchStatus.Default))
            {
                RecPonitBuff = lsRectPoint[inIndex];
                RecPonitBuff.tcsTouchStatus = TouchStatus.Down;
                lsRectPoint[inIndex] = RecPonitBuff;
                boRedraw = true;
            }
            else if((enTouchStatus == TouchStatus.Up) && (lsRectPoint[inIndex].tcsTouchStatus == TouchStatus.Down))
            {
                RecPonitBuff = lsRectPoint[inIndex];
                RecPonitBuff.tcsTouchStatus = TouchStatus.Up;
                lsRectPoint[inIndex] = RecPonitBuff;
                boRedraw = true;
            }

            if(lsRectPoint[inIndex].tcsTouchStatus == TouchStatus.Up)
            {
                RecPonitBuff = lsRectPoint[inIndex];
                RecPonitBuff.bStatus = true;
                lsRectPoint[inIndex] = RecPonitBuff;
            }

            if(lsRectPoint.FindAll(x => x.bStatus).Count == m_SensorTestPaint_GBV.Panel_Var.inKeyNumber)
            {
                boRedraw = false;
                PatternKey_Drawing();                // All Pass
            }
            else if(boRedraw)
            {
                PatternKey_Drawing();
            }
        }

        //=======================================
        private void DrawRectangle(int inX1, int inY1, int inX2, int inY2)
        {
            GDI_Panel.MoveTo(inX1, inY1);
            GDI_Panel.LineTo(inX2, inY1);

            GDI_Panel.MoveTo(inX2, inY1);
            GDI_Panel.LineTo(inX2, inY2);

            GDI_Panel.MoveTo(inX2, inY2);
            GDI_Panel.LineTo(inX1, inY2);

            GDI_Panel.MoveTo(inX1, inY2);
            GDI_Panel.LineTo(inX1, inY1);
        }

        private void DrawRectangleWithMiddleLine(int inX1, int inY1, int inX2, int inY2)
        {
            GDI_Panel.AssignPenHandle(m_Pen_Handle[enPaintPenStyle.Rectangle]);

            GDI_Panel.MoveTo(inX1, inY1);
            GDI_Panel.LineTo(inX2, inY1);

            GDI_Panel.MoveTo(inX2, inY1);
            GDI_Panel.LineTo(inX2, inY2);

            GDI_Panel.MoveTo(inX2, inY2);
            GDI_Panel.LineTo(inX1, inY2);

            GDI_Panel.MoveTo(inX1, inY2);
            GDI_Panel.LineTo(inX1, inY1);

            IntPtr inpPenBrush = GDI_Panel.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 4, Color.Black);
            GDI_Panel.AssignPenHandle(inpPenBrush);

            Graphics grp = m_panDrawing.CreateGraphics();
            Pen pen2 = new Pen(Color.Gray, 3);
            pen2.DashStyle = DashStyle.Custom;
            pen2.DashPattern = new float[] { 1f, 1f };

            if((inX2 - inX1) > (inY2 - inY1))
                grp.DrawLine(pen2, new Point(inX1, inY1 + ((inY2 - inY1) / 2)), new Point(inX2, inY1 + ((inY2 - inY1) / 2)));
            else            
                grp.DrawLine(pen2, new Point(inX1 + ((inX2 - inX1) / 2), inY1), new Point(inX1 + ((inX2 - inX1) / 2), inY2));
            
            GDI_Panel.DeleteOBJECT(inpPenBrush);
        }

        private void DrawDirectionLine(int inX1, int inY1, int inX2, int inY2, enPaintTestPattern PatternType, Color _Color, double dDiameter = 0)
        {
            Graphics grp = m_panDrawing.CreateGraphics();
            grp.SmoothingMode = SmoothingMode.HighSpeed;
            grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            //=====================
            // 畫虛線
            //Pen My_Pen = new Pen(LocalVariable.PaintTool_Var._ComColor.Color_Direction, 2);
            Pen My_Pen = new Pen(_Color, 2);
            My_Pen.DashStyle = DashStyle.Dash;
            //p.StartCap = LineCap.Round;
            //My_Pen.EndCap = LineCap.Triangle;
            grp.DrawLine(My_Pen, inX1, inY1, inX2, inY2);

            //=====================
            //畫三個箭頭
            //My_Pen = new Pen(LocalVariable.PaintTool_Var._ComColor.Color_Direction, 8);
            int iSize = (int)(dDiameter / 100d);
            if(iSize < 10)
                iSize = 10;

            if(dDiameter == 0)
                My_Pen = new Pen(_Color, 8);//20220720  Steven 修改箭頭大小
            else
                My_Pen = new Pen(_Color, iSize);//20220720  Steven 修改箭頭大小

            My_Pen.EndCap = LineCap.ArrowAnchor;

            if(PatternType == enPaintTestPattern.Paint_PaintRectangle)
            {
                double douXm = 0;
                double douYm = 0;
                double douXp = (double)(inX1 + inX2) / 2;
                double douYp = (double)(inY1 + inY2) / 2;

                if(inY2 == inY1)
                {
                    douXm = (double)(inX2 - inX1) / Math.Abs(inX2 - inX1);
                    douYm = 0;
                }
                else if(inX2 == inX1)
                {
                    douXm = 0;
                    douYm = (double)(inY2 - inY1) / Math.Abs(inY2 - inY1);
                }
                else
                {
                    douXm = (double)(inX2 - inX1) / Math.Abs(inY2 - inY1);
                    douYm = (double)(inY2 - inY1) / Math.Abs(inX2 - inX1);
                }
                grp.DrawLine(My_Pen,
                    (float)douXp, (float)douYp,
                    (float)(douXp + (douXm * 2)), (float)(douYp + (douYm * 2)));

                grp.DrawLine(My_Pen,
                  (float)(douXp + (douXm * 24)), (float)(douYp + (douYm * 24)),
                  (float)(douXp + (douXm * 26)), (float)(douYp + (douYm * 26)));

                grp.DrawLine(My_Pen,
                  (float)(douXp + (douXm * 48)), (float)(douYp + (douYm * 48)),
                  (float)(douXp + (douXm * 50)), (float)(douYp + (douYm * 50)));
            }
            else if(PatternType == enPaintTestPattern.Paint_PaintCross)
            {
                //1. 先計算斜率
                //double MySlope = Slope(new Point(inX1, inY1), new Point(inX2, inY2));
                //Console.WriteLine("MySlope = {0}", MySlope);
                var tmpPoint_1 = new Point();
                var tmpPoint_2 = new Point();
                if(inX2 > inX1)
                {
                    if(inY2 > inY1)
                    {
                        //左到右 && 上到下
                        tmpPoint_1 = new Point((inX1 + inX2 - 2) / 2, (inY1 + inY2 - 2) / 2);
                        tmpPoint_2 = new Point((inX1 + inX2 + 2) / 2, (inY1 + inY2 + 2) / 2);
                    }
                    else
                    {
                        //左到右 && 下到上
                        tmpPoint_1 = new Point((inX1 + inX2 - 2) / 2, (inY1 + inY2 + 2) / 2);
                        tmpPoint_2 = new Point((inX1 + inX2 + 2) / 2, (inY1 + inY2 - 2) / 2);
                    }
                }
                else
                {
                    if(inY2 > inY1)
                    {
                        //右到左 && 上到下
                        tmpPoint_1 = new Point((inX1 + inX2 + 2) / 2, (inY1 + inY2 - 2) / 2);
                        tmpPoint_2 = new Point((inX1 + inX2 - 2) / 2, (inY1 + inY2 + 2) / 2);
                    }
                    else
                    {
                        //右到左 && 下到上
                        tmpPoint_1 = new Point((inX1 + inX2 + 2) / 2, (inY1 + inY2 + 2) / 2);
                        tmpPoint_2 = new Point((inX1 + inX2 - 2) / 2, (inY1 + inY2 - 2) / 2);
                    }
                }

                //Point Point_2;
                //GetProjectivePoint(new Point(inX1, inY1), MySlope);
                //2. 套用斜率公式，反推座標
                grp.DrawLine(My_Pen, tmpPoint_1, tmpPoint_2);
            }
            //=====================

            grp.Dispose();
            My_Pen.Dispose();
        }

        private void DrawDirectionLine_WithoutArrow(int inX1, int inY1, int inX2, int inY2, enPaintTestPattern PatternType, Color _Color)
        {
            Graphics grp = m_panDrawing.CreateGraphics();
            grp.SmoothingMode = SmoothingMode.HighSpeed;
            grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            //=====================
            // 畫虛線
            //Pen My_Pen = new Pen(LocalVariable.PaintTool_Var._ComColor.Color_Direction, 2);
            Pen My_Pen = new Pen(_Color, 2);
            My_Pen.DashStyle = DashStyle.Dash;
            //p.StartCap = LineCap.Round;
            //My_Pen.EndCap = LineCap.Triangle;
            grp.DrawLine(My_Pen, inX1, inY1, inX2, inY2);

            grp.Dispose();
            My_Pen.Dispose();
        }

        private double Slope(Point p1, Point p2)
        {
            //return Math.Abs(((double)(p2.Y - p1.Y) / (double)(p2.X - p1.X)));
            return (((double)(p2.Y - p1.Y) / (double)(p2.X - p1.X)));
        }

        public int iEscape = 0;

        private void SensorTest_Painting_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Escape)
            {
                //如果ESC按著不放，Tool會掛掉。
                if(iEscape != 0)
                    return;
                iEscape++;
                MyTool.Delay_ms(300);
                LocalVariable.VarSenTest_Multi.m_OnRepaintEvent = true;
                m_bTestRunning = false;
                switch(LocalVariable.PaintTool_Var.g_NowTestPattern)
                {
                    case enPaintTestPattern.Paint_PaintRectangle:
                    case enPaintTestPattern.Paint_PaintCross:
                    case enPaintTestPattern.Paint_PenRectangle:
                    case enPaintTestPattern.Paint_PenCross:
                        if(!LocalVariable.PaintTool_Var.isFinishChangePatten)
                            return;

                        LocalVariable.PaintTool_Var.isFinishChangePatten = false;
                        break;
                }
                TestTimer.Stop();
                vfSetlabMegText(ErrMsg.SensorTest.UserStop);
                if(LocalVariable.PaintTool_Var.MuHandle)   //发现之前在mutRuningHandle.ReleaseMutex()之后，还会进入到这里，导致mutRuningHandle.ReleaseMutex()出错。
                    Set_DrawingTestResult(ResultStatus.UserStop);
            }
        }
    }

    public class Paint_Globe_Var
    {
        public struct _Panel_Var_
        {
            public int inScreenInfoXmin;
            public int inScreenInfoYmin;
            public int inScreenInfoXmax;
            public int inScreenInfoYmax;
            public int inXChannel;
            public int inYChannel;
            public int inAAXmin;
            public int inAAYmin;
            public int inAAXmax;
            public int inAAYmax;
            public int inKeyNumber;
        }

        public _Panel_Var_ Panel_Var = new _Panel_Var_();
    }
}