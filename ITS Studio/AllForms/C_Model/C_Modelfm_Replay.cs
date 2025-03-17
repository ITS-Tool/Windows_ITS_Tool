using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonExt;
using CSharp_PlayBar;
using FuncMethodCmd;
using ThirdPartyTools;
using System.Threading;
using GDI_32;
using Sunny.UI;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace ITS_Studio.AllForms
{
    public partial class C_Modelfm_Replay : Form
    {
        private int m_iSelectedDev = 0;
        private UIPage MyUIPage = new UIPage();
        //private CDC_Replay _CDC_Replay;
        //private Paint_Replay _Paint_Replay;
        private _FileEmployee[] FileEmployee = new _FileEmployee[Enum.GetNames(typeof(FileIdx)).Length];
        private DynamicTool MyTool = new DynamicTool();
        private ParseLog _ParseLog = new ParseLog();
        private enPaintingCount PaintCount = enPaintingCount.Painting1;
        private Dictionary<enPaintingCount, Panel> dicHoverControl = new Dictionary<enPaintingCount, Panel>();
        private INI_Tool CModel_Replay_INI = new INI_Tool(ITS_Directory.History_INI);
        private Dictionary<string, string> dmsg_touch_dict_total;
        private Dictionary<string, string> dmsg_pen_dict_total;
        private Dictionary<string, string> dmsg_touchAlgo;
        private Dictionary<string, string> dmsg_dict_selected = new Dictionary<string, string>();
        private string[] strDataFormatItemText;
        private ToolStripMenuItem[] StripCB_DataFormat;
        private bool isFinishReplayByScrollBar = true;
        private Dictionary<enPenSNRX, List<float>> DicSNRXCalculate = new Dictionary<enPenSNRX, List<float>>();
        private Dictionary<enPenSNRY, List<float>> DicSNRYCalculate = new Dictionary<enPenSNRY, List<float>>();
        private Dictionary<int, Point> DicPenCoor_Painting1 = new Dictionary<int, Point>();
        private Dictionary<int, Point> DicPenCoor_Painting2 = new Dictionary<int, Point>();
        private List<Int16> MutualData_1stFrame = new List<Int16>();
        private List<Int16> Mutual_Max = new List<Int16>();
        private List<Int16> Mutual_Min = new List<Int16>();
        private List<Int16> Mutual_Delta = new List<Int16>();
        private List<Int16> Mutual_Signal = new List<Int16>();
        private List<Int16> Mutual_Windows2 = new List<Int16>();

        private Dictionary<enSelf, List<Int16>> SelfData_1stFrame = new Dictionary<enSelf, List<short>>();
        private Dictionary<enSelf, List<Int16>> SelfData_Max = new Dictionary<enSelf, List<short>>();
        private Dictionary<enSelf, List<Int16>> SelfData_Min = new Dictionary<enSelf, List<short>>();
        private Dictionary<enSelf, List<Int16>> SelfData_Delta = new Dictionary<enSelf, List<short>>();
        private Dictionary<enSelf, List<Int16>> SelfData_Signal = new Dictionary<enSelf, List<short>>();

        private Dictionary<enPenCDC, List<Int16>> PenData_1stFrame = new Dictionary<enPenCDC, List<short>>();
        private Dictionary<enPenCDC, List<Int16>> PenData_Max = new Dictionary<enPenCDC, List<short>>();
        private Dictionary<enPenCDC, List<Int16>> PenData_Min = new Dictionary<enPenCDC, List<short>>();
        private Dictionary<enPenCDC, List<Int16>> PenData_Delta = new Dictionary<enPenCDC, List<short>>();
        private Dictionary<enPenCDC, List<Int16>> PenData_Signal = new Dictionary<enPenCDC, List<short>>();

        private Dictionary<enWM_MSG, function> mMessageReceiver = new Dictionary<enWM_MSG, function>();

        private bool isTouchDown = false;

        private Bitmap bitPaintDraw;
        private Graphics PaintDrawGrp;
        private Pen drawPen;
        private Mutex mu_Clear = new Mutex();
        private _LabelLog LabelLog = new _LabelLog();
        private _LabelLog LabelLof_Paint2 = new _LabelLog();
        private int LABLE_PADDING = 20;
        private bool bIsShowTrajectory = false;
        private bool bIsDrawCoorOnPanel = false;
        private int iMutual_Max = int.MinValue;
        private int iMutual_Min = int.MaxValue;

        private int iSelfX_Max = int.MinValue;
        private int iSelfX_Min = int.MaxValue;

        private int iSelfY_Max = int.MinValue;
        private int iSelfY_Min = int.MaxValue;

        private int iPenX_Max = int.MinValue;
        private int iPenX_Min = int.MaxValue;

        private int iPenY_Max = int.MinValue;
        private int iPenY_Min = int.MaxValue;

        private BufferedGraphicsContext current = BufferedGraphicsManager.Current;
        private Gdi32 GDI_Pen = new Gdi32();//move from CommonDefine\Variable.cs 20230306 Evayn

        private enPenTerm enPenTermNumber = enPenTerm.Term_1;
        private int iProgess = 0;
        private List<string[]> lsAllDBGData = new List<string[]>();
        private bool bIsUpdatePenMCPaintDot = true;

        public struct _finger
        {
            public int line_point_cnt;

            public bool need_handled;
            public double x, y;

            public Steema.TeeChart.Styles.Points point;
            public Steema.TeeChart.Styles.HorizLine line;
        }

        private _finger[] fingers = new _finger[10];
        private _finger[] fingersAfterCModel = new _finger[10];

        private BackgroundWorker Work_CModel_Replay;
        private ManualResetEvent Pause_CModel_Replay;
        private Pen_CDC_Format.enPenType TempPenType = Pen_CDC_Format.enPenType.Disable;
        private int ColorResolutionNumber = 256;

        public bool CModel_Replay_Pause
        {
            set
            {
                if (value == true)
                {
                    Pause_CModel_Replay.Reset();
                    SetUserControlPlayStatus(true, UserControls.UserControl_ColorView.emPlayStatus.Pause);
                    SetUserControlPlayStatus(false, UserControls.UserControl_ColorView.emPlayStatus.Pause);
                }

                else
                {
                    Pause_CModel_Replay.Set();
                    SetUserControlPlayStatus(true, UserControls.UserControl_ColorView.emPlayStatus.Play);
                    SetUserControlPlayStatus(false, UserControls.UserControl_ColorView.emPlayStatus.Play);
                }

                userControl_MC.MappingResolution(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.X_RES, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.Y_RES);
            }
        }

        public C_Modelfm_Replay()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            panel_StressTest.Visible = Program.bSTRESS_BUILD;
            StressMark.MySetStyle(STRESS_BUILD_Show.Ready);
        }
        
        private void C_Modelfm_Replay_Load(object sender, EventArgs e)
        {
            userControl_PlayBar_Cmodel1.LoadCDCFileEvent += btnCDC_Click;
            userControl_PlayBar_Cmodel1.LoadPainting1FileEvent += btnPaint_Click;
            userControl_PlayBar_Cmodel1.LoadPainting2FileEvent += btnPaint_Click;
            userControl_PlayBar_Cmodel1.PlayFileEvent += btnPlay_Click;
            userControl_PlayBar_Cmodel1.StopPlayEvent += btnStop;
            userControl_PlayBar_Cmodel1.UpdateFileEidtEvent += UpdateDataGridView_FileEidt;
            userControl_PlayBar_Cmodel1.GenFileEvent += GenNewCsvFile;

            dmsg_touch_dict_total = ParseIniDmsgInfo(ITS_Directory.PaintTool_INI, "DebugInfoName");
            dmsg_pen_dict_total = ParseIniDmsgInfo(ITS_Directory.PaintTool_INI, "PenDebugInfoName");
            dmsg_touchAlgo = ParseIniAlgoInfo(ITS_Directory.PaintTool_INI, "DataColumnName_V6");

            uiDataGridView_info_Update();
            Pause_CModel_Replay = new ManualResetEvent(true);
            StaticVar.CModelToolReplayFromHandle = this.Handle;
            mMessageReceiver.Add(enWM_MSG.WM_CModel_Replay, OnReplay_Frame);

            MenuItem_DataFormatItem(enCDCDataFormat.inOriginal);

            //TODO:加入讀取設定檔功能
            userControl_MC.StartColor = Color.LightBlue;
            userControl_MC.EndColor = Color.Orange;
            userControl_MC.InitBrushColorRange(ColorResolutionNumber);

            userControl_SC_X.StartColor = Color.LightBlue;
            userControl_SC_X.EndColor = Color.Orange;
            userControl_SC_X.InitBrushColorRange(ColorResolutionNumber);

            userControl_SC_Y.StartColor = Color.LightBlue;
            userControl_SC_Y.EndColor = Color.Orange;
            userControl_SC_Y.InitBrushColorRange(ColorResolutionNumber);

            userControl_Pen_X.StartColor = Color.LightBlue;
            userControl_Pen_X.EndColor = Color.Orange;
            userControl_Pen_X.InitBrushColorRange(ColorResolutionNumber);

            userControl_Pen_Y.StartColor = Color.LightBlue;
            userControl_Pen_Y.EndColor = Color.Orange;
            userControl_Pen_Y.InitBrushColorRange(ColorResolutionNumber);

            m_panAAscen.BackgroundImage = global::ITS_Studio.Properties.Resources.ILIlogo;
            m_panAAscen.BackgroundImageLayout = ImageLayout.Center;

            Graphics Testgrp = this.m_panAAscen.CreateGraphics();
            GDI_Pen.BeginGDI(Testgrp);

            dicHoverControl.Clear();
            dicHoverControl.Add(enPaintingCount.Painting1, panel_Hover);
            dicHoverControl.Add(enPaintingCount.Painting2, panel_Hover_P2);
            //splitContainer1.SplitterDistance = 380;
        }

        protected override void WndProc(ref Message msg)
        {
            enWM_MSG pEnumPanoMsg = (enWM_MSG)msg.Msg;
            if (mMessageReceiver.Keys.Contains(pEnumPanoMsg))
                mMessageReceiver[pEnumPanoMsg](msg);
            base.WndProc(ref msg);
        }

        private void ColorGrid_Marks_init(ref Steema.TeeChart.Styles.ColorGrid _ColorGrid)
        {
            _ColorGrid.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Value;
            _ColorGrid.Marks.Arrow.Visible = false;
            _ColorGrid.Marks.ArrowLength = 0;
            _ColorGrid.Marks.Bevel.StringColorOne = "FFFFFFFF";
            _ColorGrid.Marks.Bevel.StringColorTwo = "FF808080";
            _ColorGrid.Marks.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(224)))));
            _ColorGrid.Marks.Brush.Visible = false;
            _ColorGrid.Marks.Pen.Visible = false;
            _ColorGrid.Marks.Shadow.Visible = false;
            _ColorGrid.Marks.Symbol.Bevel.StringColorOne = "FFFFFFFF";
            _ColorGrid.Marks.Symbol.Bevel.StringColorTwo = "FF808080";
            _ColorGrid.Marks.Symbol.Pen.Visible = false;
            _ColorGrid.Marks.Symbol.Shadow.Visible = false;
            _ColorGrid.Marks.Transparent = true;
            _ColorGrid.Marks.Visible = false;
        }

        private delegate void function(Message msg);

        private Dictionary<string, string> ParseIniDmsgInfo(string ini_path, string section)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            INI_Tool MyINI = new INI_Tool(ini_path);

            List<string> keys = MyINI.IniReadAllKeys(section);
            List<string> values = MyINI.IniReadAllValues(section);
            if (keys.Count == 0)
                return null;

            for (int i = 0; i < keys.Count(); i++)
            {
                List<string> items = MyINI.IniReadAllValues(keys[i]);

                if (items.GroupBy(j => j).Where(g => g.Count() == 8).Count() > 0)
                {
                    string item = items[0];

                    if (!dict.ContainsKey(item))
                    {
                        dict.Add(item, string.Format("B'{0},", values[i].ToString()));
                        continue;
                    }

                    dict[item] += string.Format("B'{0},", values[i]);
                }
                else
                {
                    foreach (string item in items)
                    {
                        if (dict.ContainsKey(item))
                            continue;

                        int start_bit = items.FindIndex(x => x == item);
                        int end_bit = items.FindLastIndex(x => x == item);
                        dict.Add(item, string.Format("B'{0}:{1}-{2},", values[i], start_bit, end_bit));
                    }
                }
            }

            return dict;
        }

        private Dictionary<string, string> ParseIniAlgoInfo(string ini_path, string section)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            INI_Tool MyINI = new INI_Tool(ini_path);

            List<string> keys = MyINI.IniReadAllKeys(section);
            List<string> values = MyINI.IniReadAllValues(section);
            if (keys.Count == 0)
                return null;

            for (int i = 0; i < keys.Count(); i++)
            {

                if (dict.ContainsKey(keys[i]) || !keys[i].Contains("_Bit"))
                    continue;
                dict.Add(keys[i], values[i]);
            }

            return dict;
        }

        private bool SelectFile(string strFilter, string strInitialDirectory, out string FileName)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = strFilter;  //在對話方塊中可以使用檔案篩選選項
            if (!Path.IsPathRooted(strInitialDirectory))
                strInitialDirectory = ITS_Directory.Tool_CurrentDirectory + strInitialDirectory;
            openDlg.InitialDirectory = strInitialDirectory;//執行檔資料夾的位置

            openDlg.FileName = "";                     //取得或設定含有檔案對話方塊中所選取檔名的字串
            openDlg.CheckFileExists = true;      //如果當使用者指定不存在的檔名時對話方塊顯示警告訊息，屬性值為 true
            openDlg.CheckPathExists = true;    //如果當使用者指定了不存在的路徑時要顯示警告對話方塊，屬性值為 true
            openDlg.Multiselect = false;              //如果對話方塊允許一起或同時選取多個檔案，則為 true
            FileName = "";
            if (openDlg.ShowDialog(this) != DialogResult.OK)
                return false;
            FileName = openDlg.FileName;
            return true;
        }

        private void CModel_Replay_ThreadStart()
        {
            Pause_CModel_Replay = new ManualResetEvent(true);
            Work_CModel_Replay = new BackgroundWorker();
            Work_CModel_Replay.DoWork += new DoWorkEventHandler(PlayCModel);
            Work_CModel_Replay.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PlayCModel_Completed);
            Work_CModel_Replay.WorkerSupportsCancellation = true;
            Work_CModel_Replay.RunWorkerAsync();
        }

        private void btnCDC_Click(object sender, EventArgs e)
        {
            int iCDC_File = (int)FileIdx.CDC_File;
            int iPainting_1 = (int)FileIdx.Painting_1;
            int iPainting_2 = (int)FileIdx.Painting_2;
            uiDataGridView_FileEidt_DeleteAll();
            userControl_PlayBar_Cmodel1.CDCFileName = "";
            userControl_PlayBar_Cmodel1.Paint1FileName = "";
            userControl_PlayBar_Cmodel1.Paint2FileName = "";
            userControl_PlayBar_Cmodel1.InitialPaint1Mark(FileEmployee[iCDC_File].iTotal_Frame_Count);
            userControl_PlayBar_Cmodel1.InitialPaint2Mark(FileEmployee[iCDC_File].iTotal_Frame_Count);
            FileEmployee[iCDC_File].FileName = "";
            FileEmployee[iPainting_1].FileName = "";
            FileEmployee[iPainting_2].FileName = "";
            userControl_MC.ResetDrawObjects();
            userControl_SC_X.ResetDrawObjects();
            userControl_SC_Y.ResetDrawObjects();
            userControl_Pen_X.ResetDrawObjects();
            userControl_Pen_Y.ResetDrawObjects();
            userControl_MC.ClearPaintingBitmap();
            userControl_MC.ClearRawData();
            userControl_MC.InvalidateUI();
            userControl_MC.Refresh();
            ChangeResolution();
            string strC_ModelPath = string.Format(@"{0}Record_C_Model\", ITS_Directory.RecordPath);
            if (!Directory.Exists(strC_ModelPath))
                Directory.CreateDirectory(strC_ModelPath);//不存在就建立目錄 

            string strCdcFilePath = strC_ModelPath;

            if (CModel_Replay_INI.IniReadValue("CModelReplay_FilePath", "CDC_Path", "") != "")
                strCdcFilePath = Path.GetFullPath(CModel_Replay_INI.IniReadValue("CModelReplay_FilePath", "CDC_Path", ""));

            if (strCdcFilePath == "")
                strCdcFilePath = strC_ModelPath; //C_Model_Record           

            if (strCdcFilePath.Length > 259)
            {
                MyUIPage.ShowErrorDialog("Error", "The path length exceeds the operating system limit.", UIStyle.Red, true);
                CModel_Replay_INI.IniWriteValue("CModelReplay_FilePath", "CDC_Path", "");
                return;
            }

            string lastFolderName = Path.GetDirectoryName(strCdcFilePath);
            //選檔案
            if (!SelectFile("Report (*.cdc)|*.cdc", lastFolderName, out FileEmployee[iCDC_File].FileName))
                return;
            //UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.SHOW, IntPtr.Zero);
            userControl_PlayBar_Cmodel1.Split_Opt_Panel2_En = false;
            if (FileEmployee[iCDC_File].FileName.Length > 259)
            {
                MyUIPage.ShowErrorDialog("Error", "The path length exceeds the operating system limit.", UIStyle.Red, true);
                return;
            }
            string RelativePath = MyTool.GetRelativePath(ITS_Directory.RecordPath, FileEmployee[iCDC_File].FileName);
            CModel_Replay_INI.IniWriteValue("CModelReplay_FilePath", "CDC_Path", RelativePath);

            //解析前面資訊
            FileEmployee[iCDC_File].InfoBuff = new List<string>();
            FileEmployee[iCDC_File].ID_Table = new Dictionary<long, List<ID_Position>>();

            var workTask = Task.Factory.StartNew(() => _ParseLog.SeekReadFileRange_EndTag_1(FileEmployee[iCDC_File].FileName, 0, "", @"#1@0ms", ref FileEmployee[iCDC_File].InfoBuff));
            if (workTask.Status != TaskStatus.RanToCompletion)
                workTask.Wait();

            //更新Panel Info
            using(CDC_Replay _CDC_Replay = new CDC_Replay(null, UI_GBV.mDev[m_iSelectedDev].LocalVariable, UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi))
            {
                var strtmp = "";
                _CDC_Replay.UpdateTitleInfo(FileEmployee[iCDC_File].InfoBuff, "", ref strtmp);
            }

            //UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType
            //更新Total Buffer Size
            var tmp = FileEmployee[iCDC_File].InfoBuff.Find(x => x.Contains(enINFO_TAG_Name.Total_Frame_Count.ToString()));
            FileEmployee[iCDC_File].iTotal_Frame_Count = int.Parse(tmp.Split(':')[1].Trim());
            //GetAllDBGMsgBeforePlay();
            //建立檔案行數Table表
            _ParseLog.Capture_All_TagID(FileEmployee[iCDC_File].FileName, ref FileEmployee[iCDC_File].ID_Table);
            uiDataGridView_info_Update();
            userControl_PlayBar_Cmodel1.CDCFileName = FileEmployee[iCDC_File].FileName;
            userControl_PlayBar_Cmodel1.TrackBar_Max = FileEmployee[iCDC_File].iTotal_Frame_Count;
            userControl_PlayBar_Cmodel1.InitialPaint1Mark(FileEmployee[iCDC_File].iTotal_Frame_Count);
            userControl_PlayBar_Cmodel1.InitialPaint2Mark(FileEmployee[iCDC_File].iTotal_Frame_Count);

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType == Pen_CDC_Format.enPenType.Disable)
            {
                splitContainer_x.Panel1Collapsed = false;
                splitContainer_x.Panel2Collapsed = true;
                splitContainer_y.Panel1Collapsed = false;
                splitContainer_y.Panel2Collapsed = true;
            }
            else
            {
                splitContainer_x.Panel1Collapsed = true;
                splitContainer_x.Panel2Collapsed = false;
                splitContainer_y.Panel1Collapsed = true;
                splitContainer_y.Panel2Collapsed = false;
            }
            TempPenType = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType;
            ResetAllStatisticData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y);
            GetAllDBGMsgBeforePlay();
            userControl_PlayBar_Cmodel1.Split_Opt_Panel2_En = true;
            //UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.HIDE, IntPtr.Zero);
        }

        private void btnPaint_Click(object sender, EventArgs e)
        {
            var MySender = sender as UISymbolButton;
            int iPainting = -1;

            string Paint_Path = "";
            if (MySender.Name == "btn_Load_Painting1")
            {
                iPainting = (int)FileIdx.Painting_1;
                Paint_Path = "Paint1_Path";
            }
            else if (MySender.Name == "btn_Load_Painting2")
            {
                iPainting = (int)FileIdx.Painting_2;
                Paint_Path = "Paint2_Path";
            }
            //選檔案
            string strC_ModelPath = string.Format(@"{0}Record_C_Model\", ITS_Directory.RecordPath);
            if (!Directory.Exists(strC_ModelPath))
                Directory.CreateDirectory(strC_ModelPath);//不存在就建立目錄 

            string strPaintFilePath = strC_ModelPath;

            if (CModel_Replay_INI.IniReadValue("CModelReplay_FilePath", Paint_Path, "") != "")
                strPaintFilePath = Path.GetFullPath(CModel_Replay_INI.IniReadValue("CModelReplay_FilePath", Paint_Path, ""));

            if (strPaintFilePath == "")
                strPaintFilePath = strC_ModelPath; //C_Model_Record

            if (strPaintFilePath.Length > 259)
            {
                MyUIPage.ShowErrorDialog("Error", "The path length exceeds the operating system limit.", UIStyle.Red, true);
                CModel_Replay_INI.IniWriteValue("CModelReplay_FilePath", Paint_Path, "");
                return;
            }

            string lastFolderName = Path.GetDirectoryName(strPaintFilePath);
            if (!SelectFile("Report (*.paint)|*.paint", lastFolderName, out FileEmployee[iPainting].FileName))
                return;

            userControl_PlayBar_Cmodel1.Split_Opt_Panel2_En = true;
            if (FileEmployee[iPainting].FileName.Length > 259)
            {
                MyUIPage.ShowErrorDialog("Error", "The path length exceeds the operating system limit.", UIStyle.Red, true);
                return;
            }
            string RelativePath = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, FileEmployee[iPainting].FileName);
            CModel_Replay_INI.IniWriteValue("CModelReplay_FilePath", Paint_Path, RelativePath);

            //解析前面資訊
            FileEmployee[iPainting].InfoBuff = new List<string>();
            FileEmployee[iPainting].ID_Table = new Dictionary<long, List<ID_Position>>();

            var workTask = Task.Factory.StartNew(() => _ParseLog.SeekReadFileRange_EndTag_1(FileEmployee[iPainting].FileName, 0, "", @"@0ms", ref FileEmployee[iPainting].InfoBuff));
            if (workTask.Status != TaskStatus.RanToCompletion)
                workTask.Wait();
            using(Paint_Replay _Paint_Replay = new Paint_Replay(null, UI_GBV.mDev[m_iSelectedDev].LocalVariable, UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi))
            {
                var strtmp = "";
                _Paint_Replay.UpdateTitleInfo(FileEmployee[iPainting].InfoBuff, ref strtmp);
            }
            
            //解析所有的frame tag
            _ParseLog.Capture_All_TagID(FileEmployee[iPainting].FileName, ref FileEmployee[iPainting].ID_Table);

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.m_Int_InDataType = enInt_InDataType.CModelReplay;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.Display_AlgoInfo = false; //帶加入Debug訊息
            StaticVar.IsPlayback_Mode = true;
            ChangeResolution();
            if (MySender.Name == "btn_Load_Painting1")
                userControl_PlayBar_Cmodel1.Paint1FileName = FileEmployee[iPainting].FileName;
            else
                userControl_PlayBar_Cmodel1.Paint2FileName = FileEmployee[iPainting].FileName;

            for (int iFrame = 1; iFrame <= FileEmployee[(int)FileIdx.CDC_File].iTotal_Frame_Count; iFrame++)
            {
                List<byte> PaintArr = new List<byte>();
                if (_ParseLog.Transfer_Painting(FileEmployee[iPainting], iFrame, out PaintArr))
                {
                    if (MySender.Name == "btn_Load_Painting1")
                        userControl_PlayBar_Cmodel1.SetPaint1Mark(iFrame);
                    else
                        userControl_PlayBar_Cmodel1.SetPaint2Mark(iFrame);
                }
            }
            userControl_PlayBar_Cmodel1.UpdatePaintBar();
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo != null)
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType = TempPenType;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            switch (userControl_PlayBar_Cmodel1.GetButtonStatus)
            {
                case ButtonStatus.Play:
                    CModel_Replay_Pause = false;
                    userControl_PlayBar_Cmodel1.ChangeStatus(ButtonStatus.Pause);
                    break;

                case ButtonStatus.Ready:
                case ButtonStatus.Stop:
                    CModel_Replay_ThreadStart();
                    userControl_PlayBar_Cmodel1.ChangeStatus(ButtonStatus.Pause);
                    break;

                case ButtonStatus.Pause:
                    CModel_Replay_Pause = true;
                    userControl_PlayBar_Cmodel1.ChangeStatus(ButtonStatus.Play);
                    break;

                default:
                    break;
            }
        }

        private void ResetAllStatisticData(int iAX, int iAY)
        {
            MutualData_1stFrame.Clear();
            Mutual_Max.Clear();
            Mutual_Min.Clear();
            Mutual_Delta.Clear();
            Mutual_Signal.Clear();
            Mutual_Windows2.Clear();

            SelfData_Max.Clear();
            SelfData_Min.Clear();
            SelfData_Delta.Clear();
            SelfData_1stFrame.Clear();
            SelfData_Signal.Clear();

            PenData_Max.Clear();
            PenData_Min.Clear();
            PenData_Delta.Clear();
            PenData_1stFrame.Clear();
            PenData_Signal.Clear();

            SelfData_Max.Add(enSelf.Sense, new List<Int16>());
            SelfData_Max.Add(enSelf.Drive, new List<Int16>());
            SelfData_Min.Add(enSelf.Sense, new List<Int16>());
            SelfData_Min.Add(enSelf.Drive, new List<Int16>());
            SelfData_Delta.Add(enSelf.Sense, new List<Int16>());
            SelfData_Delta.Add(enSelf.Drive, new List<Int16>());
            SelfData_1stFrame.Add(enSelf.Sense, new List<Int16>());
            SelfData_1stFrame.Add(enSelf.Drive, new List<Int16>());
            SelfData_Signal.Add(enSelf.Sense, new List<Int16>());
            SelfData_Signal.Add(enSelf.Drive, new List<Int16>());

            PenData_Max.Add(enPenCDC._X, new List<Int16>());
            PenData_Max.Add(enPenCDC._Y, new List<Int16>());
            PenData_Min.Add(enPenCDC._X, new List<Int16>());
            PenData_Min.Add(enPenCDC._Y, new List<Int16>());
            PenData_Delta.Add(enPenCDC._X, new List<Int16>());
            PenData_Delta.Add(enPenCDC._Y, new List<Int16>());
            PenData_1stFrame.Add(enPenCDC._X, new List<Int16>());
            PenData_1stFrame.Add(enPenCDC._Y, new List<Int16>());
            PenData_Signal.Add(enPenCDC._X, new List<Int16>());
            PenData_Signal.Add(enPenCDC._Y, new List<Int16>());

            for (int i = 0; i < iAX * iAY; i++)
            {
                Mutual_Max.Add(Int16.MinValue);
                Mutual_Min.Add(Int16.MaxValue);
                Mutual_Delta.Add(0);
                Mutual_Signal.Add(0);
                Mutual_Windows2.Add(0);
            }

            for (int i = 0; i < iAX; i++)
            {
                SelfData_Max[enSelf.Sense].Add(Int16.MinValue);
                SelfData_Min[enSelf.Sense].Add(Int16.MaxValue);
                SelfData_Delta[enSelf.Sense].Add(0);
                SelfData_Signal[enSelf.Sense].Add(0);
            }

            for (int i = 0; i < iAY; i++)
            {
                SelfData_Max[enSelf.Drive].Add(Int16.MinValue);
                SelfData_Min[enSelf.Drive].Add(Int16.MaxValue);
                SelfData_Delta[enSelf.Drive].Add(0);
                SelfData_Signal[enSelf.Drive].Add(0);
            }

            for (int i = 0; i < iAX * 8; i++)
            {
                PenData_Max[enPenCDC._X].Add(Int16.MinValue);
                PenData_Min[enPenCDC._X].Add(Int16.MaxValue);
                PenData_Delta[enPenCDC._X].Add(0);
                PenData_Signal[enPenCDC._X].Add(0);
            }

            for (int i = 0; i < iAY * 8; i++)
            {
                PenData_Max[enPenCDC._Y].Add(Int16.MinValue);
                PenData_Min[enPenCDC._Y].Add(Int16.MaxValue);
                PenData_Delta[enPenCDC._Y].Add(0);
                PenData_Signal[enPenCDC._Y].Add(0);
            }
        }

        private void SetUserControlPlayStatus(bool isFinger, UserControls.UserControl_ColorView.emPlayStatus PlayStatus)
        {
            if (isFinger)
            {
                userControl_MC.PlayStatus = PlayStatus;
                userControl_SC_X.PlayStatus = PlayStatus;
                userControl_SC_Y.PlayStatus = PlayStatus;
            }
            else
            {
                userControl_Pen_X.PlayStatus = PlayStatus;
                userControl_Pen_Y.PlayStatus = PlayStatus;
            }

        }

        private void CalculateDataFormat(List<Int16> MutualData, List<Int16> SelfSense, List<Int16> SelfDrive)
        {
            int iAAX = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X;
            int iAAY = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y;
            var tmp = (enCDCDataFormat)UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType;

            for (int i = 0; i < MutualData.Count; i++)
            {
                Int16 tmpMax = Math.Max(Mutual_Max[i], MutualData[i]);
                Int16 tmpMin = Math.Min(Mutual_Min[i], MutualData[i]);

                Mutual_Max[i] = tmpMax;
                Mutual_Min[i] = tmpMin;
                Mutual_Delta[i] = (short)(tmpMax - tmpMin);
                if (MutualData_1stFrame.Count != 0)
                    Mutual_Signal[i] = (short)(MutualData_1stFrame[i] - MutualData[i]);
            }

            for (int i = 0; i < iAAY - 1; i++)
            {
                for (int j = 0; j < iAAX - 1; j++)
                {
                    int id1 = (i * iAAX + j);
                    int id2 = (i * iAAX + j) + 1;
                    int id3 = (i * iAAX + j) + iAAX;
                    int id4 = (i * iAAX + j) + iAAX + 1;
                    if (MutualData.Count > 0)
                    {
                        var num_1 = MutualData[id1];
                        var num_2 = MutualData[id2];
                        var num_3 = MutualData[id3];
                        var num_4 = MutualData[id4];
                        Mutual_Windows2[id1] = (short)(Math.Abs(Math.Abs(num_1 - num_2) - Math.Abs(num_3 - num_4)));
                    }
                }
            }

            for (int i = 0; i < SelfSense.Count; i++)
            {
                SelfData_Max[enSelf.Sense][i] = Math.Max(SelfData_Max[enSelf.Sense][i], SelfSense[i]);
                SelfData_Min[enSelf.Sense][i] = Math.Min(SelfData_Min[enSelf.Sense][i], SelfSense[i]);
                SelfData_Delta[enSelf.Sense][i] = (short)(SelfData_Max[enSelf.Sense][i] - SelfData_Min[enSelf.Sense][i]);
                if (SelfData_1stFrame[enSelf.Sense].Count != 0)
                    SelfData_Signal[enSelf.Sense][i] = (short)(SelfData_1stFrame[enSelf.Sense][i] - SelfSense[i]);
            }

            for (int i = 0; i < SelfDrive.Count; i++)
            {
                SelfData_Max[enSelf.Drive][i] = Math.Max(SelfData_Max[enSelf.Drive][i], SelfDrive[i]);
                SelfData_Min[enSelf.Drive][i] = Math.Min(SelfData_Min[enSelf.Drive][i], SelfDrive[i]);
                SelfData_Delta[enSelf.Drive][i] = (short)(SelfData_Max[enSelf.Drive][i] - SelfData_Min[enSelf.Drive][i]);
                if (SelfData_1stFrame[enSelf.Drive].Count != 0)
                    SelfData_Signal[enSelf.Drive][i] = (short)(SelfData_1stFrame[enSelf.Drive][i] - SelfDrive[i]);
            }

            userControl_MC.ClearPaintingBitmap();
        }

        private void CalculatePenDataFormat(List<Int16> PenX, List<Int16> PenY)
        {
            for (int i = 0; i < PenX.Count; i++)
            {
                PenData_Max[enPenCDC._X][i] = Math.Max(PenData_Max[enPenCDC._X][i], PenX[i]);
                PenData_Min[enPenCDC._X][i] = Math.Min(PenData_Min[enPenCDC._X][i], PenX[i]);
                PenData_Delta[enPenCDC._X][i] = (short)(PenData_Max[enPenCDC._X][i] - PenData_Min[enPenCDC._X][i]);
                if (PenData_1stFrame[enPenCDC._X].Count != 0)
                    PenData_Signal[enPenCDC._X][i] = (short)(PenData_1stFrame[enPenCDC._X][i] - PenX[i]);
            }

            for (int i = 0; i < PenY.Count; i++)
            {
                PenData_Max[enPenCDC._Y][i] = Math.Max(PenData_Max[enPenCDC._Y][i], PenY[i]);
                PenData_Min[enPenCDC._Y][i] = Math.Min(PenData_Min[enPenCDC._Y][i], PenY[i]);
                PenData_Delta[enPenCDC._Y][i] = (short)(PenData_Max[enPenCDC._Y][i] - PenData_Min[enPenCDC._Y][i]);
                if (PenData_1stFrame[enPenCDC._Y].Count != 0)
                    PenData_Signal[enPenCDC._Y][i] = (short)(PenData_1stFrame[enPenCDC._Y][i] - PenY[i]);
            }
        }

        private void ShowFingerColorGrid(List<Int16> MutualData, List<Int16> SelfSense, List<Int16> SelfDrive, short[] DBGMsg, int iFrameIndex)
        {
            var tmp = (enCDCDataFormat)UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType;

            int iMin = 0;
            int iMax = 0;
            switch (tmp)
            {
                case enCDCDataFormat.inOriginal:
                    if (MutualData.Count > 0)
                    {
                        iMin = MutualData.Min();
                        iMax = MutualData.Max();
                        ResetColorGrid(iMin, iMax, userControl_MC, ref iMutual_Max, ref iMutual_Min);
                        userControl_MC.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, MutualData);
                    }

                    if (SelfSense.Count > 0)
                    {
                        iMin = SelfSense.Min();
                        iMax = SelfSense.Max();
                        ResetColorGrid(iMin, iMax, userControl_SC_X, ref iSelfX_Max, ref iSelfX_Min);
                        userControl_SC_X.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, 1, SelfSense);
                    }

                    if (SelfDrive.Count > 0)
                    {
                        iMin = SelfDrive.Min();
                        iMax = SelfDrive.Max();
                        ResetColorGrid(iMin, iMax, userControl_SC_Y, ref iSelfY_Max, ref iSelfY_Min);
                        userControl_SC_Y.ShowColorGridData(1, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, SelfDrive);
                    }

                    break;
                case enCDCDataFormat.inMax:
                    if (Mutual_Max.Count > 0)
                    {
                        iMin = Mutual_Max.Min();
                        iMax = Mutual_Max.Max();
                        ResetColorGrid(iMin, iMax, userControl_MC, ref iMutual_Max, ref iMutual_Min);
                        userControl_MC.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, Mutual_Max);
                    }

                    if (SelfData_Max[enSelf.Sense].Count > 0)
                    {
                        iMin = SelfData_Max[enSelf.Sense].Min();
                        iMax = SelfData_Max[enSelf.Sense].Max();
                        ResetColorGrid(iMin, iMax, userControl_SC_X, ref iSelfX_Max, ref iSelfX_Min);
                        userControl_SC_X.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, 1, SelfData_Max[enSelf.Sense]);
                    }

                    if (SelfData_Max[enSelf.Drive].Count > 0)
                    {
                        iMin = SelfData_Max[enSelf.Drive].Min();
                        iMax = SelfData_Max[enSelf.Drive].Max();
                        ResetColorGrid(iMin, iMax, userControl_SC_Y, ref iSelfY_Max, ref iSelfY_Min);
                        userControl_SC_Y.ShowColorGridData(1, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, SelfData_Max[enSelf.Drive]);
                    }

                    break;
                case enCDCDataFormat.inMin:

                    if (Mutual_Min.Count > 0)
                    {
                        iMin = Mutual_Min.Min();
                        iMax = Mutual_Min.Max();
                        ResetColorGrid(iMin, iMax, userControl_MC, ref iMutual_Max, ref iMutual_Min);
                        userControl_MC.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, Mutual_Min);
                    }

                    if (SelfData_Min[enSelf.Sense].Count > 0)
                    {
                        iMin = SelfData_Min[enSelf.Sense].Min();
                        iMax = SelfData_Min[enSelf.Sense].Max();
                        ResetColorGrid(iMin, iMax, userControl_SC_X, ref iSelfX_Max, ref iSelfX_Min);
                        userControl_SC_X.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, 1, SelfData_Min[enSelf.Sense]);
                    }

                    if (SelfData_Min[enSelf.Drive].Count > 0)
                    {
                        iMin = SelfData_Min[enSelf.Drive].Min();
                        iMax = SelfData_Min[enSelf.Drive].Max();
                        ResetColorGrid(iMin, iMax, userControl_SC_Y, ref iSelfY_Max, ref iSelfY_Min);
                        userControl_SC_Y.ShowColorGridData(1, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, SelfData_Min[enSelf.Drive]);
                    }

                    break;
                case enCDCDataFormat.iDelta:

                    if (Mutual_Delta.Count > 0)
                    {
                        iMin = Mutual_Delta.Min();
                        iMax = Mutual_Delta.Max();
                        ResetColorGrid(iMin, iMax, userControl_MC, ref iMutual_Max, ref iMutual_Min);
                        userControl_MC.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, Mutual_Delta);
                    }

                    if (SelfData_Delta[enSelf.Sense].Count > 0)
                    {
                        iMin = SelfData_Delta[enSelf.Sense].Min();
                        iMax = SelfData_Delta[enSelf.Sense].Max();
                        ResetColorGrid(iMin, iMax, userControl_SC_X, ref iSelfX_Max, ref iSelfX_Min);
                        userControl_SC_X.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, 1, SelfData_Delta[enSelf.Sense]);
                    }

                    if (SelfData_Delta[enSelf.Drive].Count > 0)
                    {
                        iMin = SelfData_Delta[enSelf.Drive].Min();
                        iMax = SelfData_Delta[enSelf.Drive].Max();
                        ResetColorGrid(iMin, iMax, userControl_SC_Y, ref iSelfY_Max, ref iSelfY_Min);
                        userControl_SC_Y.ShowColorGridData(1, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, SelfData_Delta[enSelf.Drive]);
                    }

                    break;
                case enCDCDataFormat.iSignal:

                    if (Mutual_Signal.Count > 0)
                    {
                        iMin = Mutual_Signal.Min();
                        iMax = Mutual_Signal.Max();
                        ResetColorGrid(iMin, iMax, userControl_MC, ref iMutual_Max, ref iMutual_Min);
                        userControl_MC.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, Mutual_Signal);
                    }

                    if (SelfData_Signal[enSelf.Sense].Count > 0)
                    {
                        iMin = SelfData_Signal[enSelf.Sense].Min();
                        iMax = SelfData_Signal[enSelf.Sense].Max();
                        ResetColorGrid(iMin, iMax, userControl_SC_X, ref iSelfX_Max, ref iSelfX_Min);
                        userControl_SC_X.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, 1, SelfData_Signal[enSelf.Sense]);
                    }

                    if (SelfData_Signal[enSelf.Drive].Count > 0)
                    {
                        iMin = SelfData_Signal[enSelf.Drive].Min();
                        iMax = SelfData_Signal[enSelf.Drive].Max();
                        ResetColorGrid(iMin, iMax, userControl_SC_Y, ref iSelfY_Max, ref iSelfY_Min);
                        userControl_SC_Y.ShowColorGridData(1, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, SelfData_Signal[enSelf.Drive]);
                    }

                    break;
                case enCDCDataFormat.iWindows2:

                    if (Mutual_Windows2.Count > 0)
                    {
                        iMin = Mutual_Windows2.Min();
                        iMax = Mutual_Windows2.Max();
                        ResetColorGrid(iMin, iMax, userControl_MC, ref iMutual_Max, ref iMutual_Min);
                        userControl_MC.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, Mutual_Windows2);
                    }

                    break;
                default:
                    break;
            }

            userControl_MC.Refresh();
            userControl_SC_X.Refresh();
            userControl_SC_Y.Refresh();

            if (DBGMsg.Length != 0)
                UpdateDmsgByIndex(iFrameIndex);
        }

        private void ShowPenColorGrid(List<Int16> PenX, List<Int16> PenY, short[] DBGMsg, int iFrame)
        {
            List<short> lsYtemp = new List<short>();
            List<short> _lsYtemp = new List<short>();

            var tmp = (enCDCDataFormat)UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType;
            switch (tmp)
            {
                case enCDCDataFormat.inMax:
                    PenX = PenData_Max[enPenCDC._X];
                    PenY = PenData_Max[enPenCDC._Y];
                    break;
                case enCDCDataFormat.inMin:
                    PenX = PenData_Min[enPenCDC._X];
                    PenY = PenData_Min[enPenCDC._Y];
                    break;
                case enCDCDataFormat.iDelta:
                    PenX = PenData_Delta[enPenCDC._X];
                    PenY = PenData_Delta[enPenCDC._Y];
                    break;
                case enCDCDataFormat.iSignal:
                    PenX = PenData_Signal[enPenCDC._X];
                    PenY = PenData_Signal[enPenCDC._Y];
                    break;
                case enCDCDataFormat.inOriginal:
                case enCDCDataFormat.iWindows2:
                default:
                    break;
            }

            int iMin = PenX.Min();
            int iMax = PenX.Max();
            ResetColorGrid(iMin, iMax, userControl_Pen_X, ref iPenX_Max, ref iPenX_Min);

            iMin = PenY.Min();
            iMax = PenY.Max();
            ResetColorGrid(iMin, iMax, userControl_Pen_Y, ref iPenY_Max, ref iPenY_Min);

            for (int i = 0; i < UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y; i++)
            {

                int iY = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y;
                _lsYtemp.Clear();
                for (int y = 0; y < PenY.Count; y++)
                {

                    if (i == 0)
                    {
                        if (y % iY == 0 || y % iY == iY)
                            _lsYtemp.Add(PenY[y]);
                    }
                    else
                    {
                        if (y % iY == i)
                            _lsYtemp.Add(PenY[y]);
                    }

                }
                lsYtemp.AddRange(_lsYtemp);
            }

            userControl_MC.ShowColorGridData(0, 0, PenX);
            userControl_Pen_X.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, 8, PenX);
            userControl_Pen_Y.ShowColorGridData(8, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, lsYtemp);

            List<short> zeroData = new List<short>();

            for (int i = 0; i < UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X * UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y; i++)
            {
                zeroData.Add(0);
            }

            //userControl_MC.ShowColorGridData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y, zeroData);
            userControl_MC.InvalidateUI();

            userControl_MC.Refresh();
            userControl_Pen_X.Refresh();
            userControl_Pen_Y.Refresh();

            if (DBGMsg.Length != 0)
                UpdateDmsgByIndex(iFrame);
        }

        private void ResetColorGrid(int iMin, int iMax, UserControls.UserControl_ColorView colorGrid, ref int _NodeMax, ref int _NodeMin)
        {
            if ((_NodeMin < iMin) && (_NodeMax > iMax))
                return;

            _NodeMin = (_NodeMin > iMin) ? iMin : _NodeMin;
            colorGrid.iMutual_Min = _NodeMin;
            _NodeMax = (_NodeMax < iMax) ? iMax : _NodeMax;

            if (_NodeMax - _NodeMin < 5)
            {
                colorGrid.PaletteStep = 1;
                colorGrid.SetColorSteps(5);
            }
            else
            {
                if (_NodeMax - _NodeMin > 256)
                {
                    colorGrid.PaletteStep = (_NodeMax - _NodeMin) / 256;
                    colorGrid.SetColorSteps(256);
                }
                else
                {
                    colorGrid.PaletteStep = 1;
                    colorGrid.SetColorSteps(_NodeMax - _NodeMin);
                }
            }

        }

        private void GetAllDBGMsgBeforePlay()
        {
            ParseLog._CDC_Datas CDC_Datas = new ParseLog._CDC_Datas();
            var iCDC_File = (int)FileIdx.CDC_File;
            lsAllDBGData.Clear();
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType == Pen_CDC_Format.enPenType.Disable)
            {
                for (int iFrame = 1; iFrame <= FileEmployee[iCDC_File].iTotal_Frame_Count; iFrame++)
                {
                    if (!_ParseLog.Transfer_CDC(FileEmployee[(int)FileIdx.CDC_File], iFrame, ref CDC_Datas))
                        continue;
                    string[] dTemp = new string[dmsg_dict_selected.Count + dmsg_touchAlgo.Count - 1];
                    if (CDC_Datas.DMsg.Count == 0)
                        return;

                    for (int i = 0, idx = 0; i < dmsg_dict_selected.Count; i++)
                    {
                        string key = dmsg_dict_selected.ElementAt(i).Key;
                        string format = dmsg_dict_selected.ElementAt(i).Value;
                        int cnt = format.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Count();
                        uint val = 0;

                        if (format.Contains(':'))
                        {
                            format = format.Replace(",", "");
                            int start_bit = int.Parse(format.Split(':')[1].Split('-')[0]);
                            int end_bit = int.Parse(format.Split(':')[1].Split('-')[1]);
                            uint mask = (uint)0xFF >> (8 - (end_bit - start_bit + 1));

                            val = (uint)CDC_Datas.DMsg[idx];
                            val = val >> start_bit;
                            val = val & mask;
                            dTemp[i + dmsg_touchAlgo.Count - 1] = val.ToString();
                            idx = idx + ((end_bit == 7) ? 1 : 0);
                        }
                        else
                        {
                            val = (uint)CDC_Datas.DMsg[idx++];
                            for (int j = 1; j < cnt; j++)
                                val |= (uint)(CDC_Datas.DMsg[idx++] << (j * 8));

                            if (format.IsNullOrEmpty())
                            {
                                int iBitCnts = 0;
                                int iIndex = 0;
                                foreach (var item in EnumTool.EnumToList<PaintTool.enAlgoNameRowIndex>())
                                {
                                    if (item == PaintTool.enAlgoNameRowIndex._Report_Rate)
                                        continue;
                                    if (dmsg_touchAlgo.ElementAt(iBitCnts).Value != "NA")
                                    {
                                        int iValue = MyTool.GetIntegerSomeBit((int)val, iBitCnts);
                                        iIndex = i + iBitCnts;
                                        //Set_DataGridView_Val(iIndex, string.Format("{0}", (iValue == 1) ? "Active" : "Inactive"));
                                        dTemp[iIndex] = (iValue == 1) ? "Active" : "Inactive";
                                    }
                                    iBitCnts++;
                                }
                                continue;
                            }

                            dTemp[i + dmsg_touchAlgo.Count - 1] = val.ToString();
                        }
                        if (key.Equals("SNR"))
                        {
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts++;

                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_lsSNR_Value.Add((float)val);

                            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iSNR_FrameCount)
                            {
                                //ProgressBar_SNR.Value = ProgressBar_SNR.Maximum;
                                var tmp = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.CalculateSNR(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_lsSNR_Value);
                                dTemp[i + dmsg_touchAlgo.Count - 1] = tmp.ToString();
                                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_lsSNR_Value.Clear();
                                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Finger_SNR_FrameCounts = 0;
                                continue;
                            }
                            else
                            {
                                dTemp[i + dmsg_touchAlgo.Count - 1] = "N/A";
                                continue;
                            }
                        }
                    }
                    lsAllDBGData.Add(dTemp);
                }
            }
            else
            {
                for (int iFrame = 1; iFrame <= FileEmployee[iCDC_File].iTotal_Frame_Count; iFrame++)
                {
                    List<Int16> PEN_CDC_X, PEN_CDC_Y, PEN_DMSG;
                    if (!_ParseLog.Transfer_PenCDC(FileEmployee[(int)FileIdx.CDC_File], iFrame, out PEN_CDC_X, out PEN_CDC_Y, out PEN_DMSG))
                        continue;
                    int row_start = Enum.GetNames(typeof(C_MODEL_DATA_INFO)).Length;
                    int iIndex = 0;

                    if (PEN_DMSG.Count != 0)
                    {
                        if (dmsg_dict_selected.ContainsKey("Algo_Pen_Stage"))
                            iIndex = PEN_DMSG[PEN_DMSG.Count - 1];

                        string[] dTemp = new string[dmsg_dict_selected.Count];

                        for (int i = 0, idx = 0; i < dmsg_dict_selected.Count; i++)
                        {
                            string key = dmsg_dict_selected.ElementAt(i).Key;
                            string format = dmsg_dict_selected.ElementAt(i).Value;
                            uint val = 0;

                            if (key.Contains("Pen_SNR_X"))
                            {
                                enPenSNRX PenSNRXIndex = (enPenSNRX)Enum.Parse(typeof(enPenSNRX), key);
                                if ((int)PenSNRXIndex != iIndex)
                                    continue;

                                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[(int)PenSNRXIndex]++;
                                if (!DicSNRXCalculate.ContainsKey(PenSNRXIndex))
                                    DicSNRXCalculate.Add(PenSNRXIndex, new List<float>());

                                int cnt = format.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Count();


                                val = (uint)PEN_DMSG[idx++];
                                for (int j = 1; j < cnt; j++)
                                    val |= (uint)(PEN_DMSG[idx++] << (j * 8));

                                DicSNRXCalculate[PenSNRXIndex].Add(val);

                                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[(int)PenSNRXIndex] >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iSNR_FrameCount)
                                {
                                    var tmp = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.CalculateSNR(DicSNRXCalculate[PenSNRXIndex]);
                                    dTemp[i] = tmp.ToString();
                                    DicSNRXCalculate[PenSNRXIndex].Clear();
                                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[(int)PenSNRXIndex] = 0;
                                }
                                else
                                {
                                    dTemp[i] = "N/A";
                                    continue;
                                }

                            }
                            else if (key.Contains("Pen_SNR_Y"))
                            {
                                enPenSNRY PenSNRYIndex = (enPenSNRY)Enum.Parse(typeof(enPenSNRY), key);
                                if ((int)PenSNRYIndex != iIndex)
                                    continue;

                                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[(int)PenSNRYIndex]++;
                                if (!DicSNRYCalculate.ContainsKey(PenSNRYIndex))
                                    DicSNRYCalculate.Add(PenSNRYIndex, new List<float>());

                                int cnt = format.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Count();

                                val = (uint)PEN_DMSG[idx++];
                                for (int j = 1; j < cnt; j++)
                                    val |= (uint)(PEN_DMSG[idx++] << (j * 8));

                                DicSNRYCalculate[PenSNRYIndex].Add(val);

                                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[(int)PenSNRYIndex] >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iSNR_FrameCount)
                                {
                                    var tmp = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.CalculateSNR(DicSNRYCalculate[PenSNRYIndex]);
                                    dTemp[i] = tmp.ToString();
                                    DicSNRYCalculate[PenSNRYIndex].Clear();
                                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[(int)PenSNRYIndex] = 0;
                                }
                                else
                                {
                                    dTemp[i] = "N/A";
                                    continue;
                                }
                            }
                            else
                            {
                                int cnt = format.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Count();

                                val = (uint)PEN_DMSG[idx++];
                                for (int j = 1; j < cnt; j++)
                                    val |= (uint)(PEN_DMSG[idx++] << (j * 8));

                                dTemp[i] = val.ToString();
                            }

                        }
                        lsAllDBGData.Add(dTemp);
                    }

                }
            }
        }

        private void PlayCModel(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                StressMark.MySetStyle(STRESS_BUILD_Show.Ready);
            }));
            var iCDC_File = (int)FileIdx.CDC_File;
            ChangeResolution();

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType == Pen_CDC_Format.enPenType.Disable)
            {
                SetUserControlPlayStatus(true, UserControls.UserControl_ColorView.emPlayStatus.Play);
                //依據CDC的面數來讀取
                ParseLog._CDC_Datas CDC_Datas = new ParseLog._CDC_Datas();
                for (int iFrame = 1; iFrame <= FileEmployee[iCDC_File].iTotal_Frame_Count; iFrame++)
                {
                    if(!_ParseLog.Transfer_CDC(FileEmployee[(int)FileIdx.CDC_File], iFrame, ref CDC_Datas))
                    {
                        this.Invoke(new MethodInvoker(delegate()
                        {
                            StressMark.MySetStyle(STRESS_BUILD_Show.NG);
                        }));
                        continue;
                    }
                    if (iFrame == 1)
                    {
                        MutualData_1stFrame.AddRange(CDC_Datas.CDC);
                        SelfData_1stFrame[enSelf.Sense].AddRange(CDC_Datas.Self_X);
                        SelfData_1stFrame[enSelf.Drive].AddRange(CDC_Datas.Self_Y);
                    }

                    CalculateDataFormat(CDC_Datas.CDC, CDC_Datas.Self_X, CDC_Datas.Self_Y);
                    RePainting(FileIdx.Painting_1, iFrame);
                    RePainting(FileIdx.Painting_2, iFrame);

                    //Update UI Chart                     
                    short[] sArrary = CDC_Datas.DMsg.ToArray();
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        ShowFingerColorGrid(CDC_Datas.CDC, CDC_Datas.Self_X, CDC_Datas.Self_Y, sArrary, iFrame);
                        userControl_PlayBar_Cmodel1.TrackBar_Value = iFrame;
                        userControl_PlayBar_Cmodel1.FrameNum_Text = this.userControl_PlayBar_Cmodel1.TrackBar_Value.ToString();
                        userControl_PlayBar_Cmodel1.RefreshFrameLabel();
                    }));
                    Pause_CModel_Replay.WaitOne(Timeout.Infinite);
                    if (iFrame != iProgess)
                        iFrame = iProgess;

                    iProgess++;

                    if (Work_CModel_Replay.CancellationPending)
                        return;
                }
            }
            else
            {
                SetUserControlPlayStatus(false, UserControls.UserControl_ColorView.emPlayStatus.Play);
                //依據CDC的面數來讀取
                for (int iFrame = 1; iFrame <= FileEmployee[iCDC_File].iTotal_Frame_Count; iFrame++)
                {
                    List<Int16> PEN_CDC_X, PEN_CDC_Y, PEN_DMSG;

                    if(!_ParseLog.Transfer_PenCDC(FileEmployee[(int)FileIdx.CDC_File], iFrame, out PEN_CDC_X, out PEN_CDC_Y, out PEN_DMSG))
                    {
                        this.Invoke(new MethodInvoker(delegate()
                        {
                            StressMark.MySetStyle(STRESS_BUILD_Show.NG);
                        }));
                        continue;
                    }
                    if (iFrame == 1)
                    {
                        PenData_1stFrame[enPenCDC._X].AddRange(PEN_CDC_X);
                        PenData_1stFrame[enPenCDC._Y].AddRange(PEN_CDC_Y);
                    }
                    CalculatePenDataFormat(PEN_CDC_X, PEN_CDC_Y);

                    DicPenCoor_Painting1.Clear();
                    DicPenCoor_Painting2.Clear();

                    RePainting(FileIdx.Painting_1, iFrame);
                    RePainting(FileIdx.Painting_2, iFrame);

                    //Update UI Chart                     
                    short[] sArrary = PEN_DMSG.ToArray();
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        ShowPenCoor_Lable();
                        ShowPenColorGrid(PEN_CDC_X, PEN_CDC_Y, sArrary, iFrame);
                        userControl_PlayBar_Cmodel1.TrackBar_Value = iFrame;
                        userControl_PlayBar_Cmodel1.FrameNum_Text = this.userControl_PlayBar_Cmodel1.TrackBar_Value.ToString();
                        userControl_PlayBar_Cmodel1.RefreshFrameLabel();

                    }));
                    Pause_CModel_Replay.WaitOne(Timeout.Infinite);
                    if (iFrame != iProgess)
                        iFrame = iProgess;

                    iProgess++;

                    if (Work_CModel_Replay.CancellationPending)
                        return;
                }
            }
            this.Invoke(new MethodInvoker(delegate()
                {
                    StressMark.MySetStyle(STRESS_BUILD_Show.OK);
                }));
        }

        private void RePainting(FileIdx _FileIdx, int iFrame)
        {
            string FileName = FileEmployee[(int)_FileIdx].FileName;
            if (!string.IsNullOrEmpty(FileName))
            {
                PaintCount = (_FileIdx == FileIdx.Painting_1) ? enPaintingCount.Painting1 : enPaintingCount.Painting2;

                List<byte> PaintArr = new List<byte>();

                if (_ParseLog.Transfer_Painting(FileEmployee[(int)_FileIdx], iFrame, out PaintArr))
                {
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType == Pen_CDC_Format.enPenType.Disable)
                    {
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.RePainting_API(PaintArr.ToArray(), PaintCount);
                    }
                    else
                    {
                        int iPacketCount = PaintArr.Count / 64;
                        int iTemp = 0;
                        for (int i = 0; i < iPacketCount; i++)
                        {
                            List<byte> lsTemp = PaintArr.GetRange(iTemp, 64);
                            byte[] tmpArrary = lsTemp.ToArray();

                            if ((tmpArrary[0] == 0x0C || tmpArrary[0] == 0x0D))
                                enPenTermNumber = (enPenTerm)tmpArrary[60];
                            else
                                tmpArrary[0] = 0x03;

                            if (lsTemp[0] != 0x00)
                                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.RePainting_API(tmpArrary, PaintCount);
                            iTemp += 64;
                        }
                    }
                }
            }
            //bIsUpdatePenMCPaintDot = false;
        }

        private void PlayCModel_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            StopPlay();
        }

        private void btnStop(object sender, EventArgs e)
        {
            StopPlay();
        }

        private void StopPlay()
        {
            if (Work_CModel_Replay != null)
            {
                if (Work_CModel_Replay.IsBusy)
                    Work_CModel_Replay.CancelAsync();
                //Pause_CModel_Replay.Enabled = true;
                //CModel_Replay_Pause = false;
                iProgess = 0;
                userControl_PlayBar_Cmodel1.ChangeStatus(ButtonStatus.Stop);

                SetUserControlPlayStatus(true, UserControls.UserControl_ColorView.emPlayStatus.Stop);
                SetUserControlPlayStatus(false, UserControls.UserControl_ColorView.emPlayStatus.Stop);
                ResetAllStatisticData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y);
                ChangeResolution();
                ResetAllMaxMinValue();
            }

        }

        public void ChangeResolution()
        {
            userControl_MC.Dock = DockStyle.Fill;
            m_panAAscen.Dock = DockStyle.Fill; //為了固定初始大小，故意設成Fill
            int inHeight = m_panAAscen.Height;
            int inWidth = m_panAAscen.Width;
          
            //m_panAAscen.Height = inHeight - 30;
            //m_panAAscen.Width = inWidth - 30;
            //m_panAAscen.Dock = DockStyle.None;
            //m_panAAscen.Location = new System.Drawing.Point(15, 15);

            if (dicHoverControl.ContainsKey(enPaintingCount.Painting1))
                dicHoverControl[enPaintingCount.Painting1].Visible = false;
            if (dicHoverControl.ContainsKey(enPaintingCount.Painting2))
                dicHoverControl[enPaintingCount.Painting2].Visible = false;

            bitPaintDraw = new Bitmap(inWidth, inHeight);
            //using (SolidBrush br = new SolidBrush(Color.FromArgb(190, Color.Silver)))
            //    bg.Graphics.FillRectangle(br, ClientRectangle);

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize = new Size(m_panAAscen.Width, m_panAAscen.Height);
            var PanelSize = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize;
            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool != null)
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.MappingResolution(PanelSize, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.X_RES, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.Y_RES);
                userControl_MC.MappingResolution(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.X_RES, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.Y_RES);
            }

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo != null)
            {
                InitFingerColor();
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.iErrPktCnts = 0;
                if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool != null)
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.Parameter_Init_API(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize);//手指資料初始化
            }
            InitLabelLog();

            m_panAAscen.Invalidate();
        }

        private void InitLabelLog()
        {
            int iH = m_panAAscen.Height - 10 * LABLE_PADDING;
            Initial_LabelLogText(ref LabelLog, _LabelLog.Label_Type.Finger, new Point(10, 10));
            Initial_LabelLogText(ref LabelLof_Paint2, _LabelLog.Label_Type.Finger, new Point(10, iH));

            // 設定Pen Label 位置 - Finger的上面
            Initial_LabelLogText(ref LabelLog, _LabelLog.Label_Type.Pen, new Point(10, 10));
            Initial_LabelLogText(ref LabelLof_Paint2, _LabelLog.Label_Type.Pen, new Point(10, iH));

        }

        private void Initial_LabelLogText(ref _LabelLog obj, _LabelLog.Label_Type obj_Type, Point StartPosition)
        {
            //if (obj_Type == _LabelLog.Label_Type.Pen)
            //{
            //    Point NewPos = new Point(StartPosition.X, StartPosition.Y);
            //    obj.Pen[0] = new KeyValuePair<Point, string>(NewPos, "");
            //}
            //else
            {
                int iLen = obj_Type == _LabelLog.Label_Type.Pen ? obj.Pen.Length : obj.Finger.Length;
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
                    if (obj_Type == _LabelLog.Label_Type.Pen)
                        obj.Pen[i] = new KeyValuePair<Point, string>(NewPos, "");
                    else
                        obj.Finger[i] = new KeyValuePair<Point, string>(NewPos, "");
                }
            }
        }

        private bool StartReplay_Frame(int SelectIndex)
        {
            isFinishReplayByScrollBar = false;
            var iCDC_File = (int)FileIdx.CDC_File;

            if (SelectIndex < Convert.ToInt32(this.userControl_PlayBar_Cmodel1.FrameNum_Text))
            {
                if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool != null)
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.Parameter_Init_API(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.PanelSize);//手指資料初始化

            }

            this.userControl_PlayBar_Cmodel1.FrameNum_Text = SelectIndex.ToString();
            this.userControl_PlayBar_Cmodel1.Refresh();
            iProgess = SelectIndex;

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo == null)
                return false;

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType == Pen_CDC_Format.enPenType.Disable)
            {
                ParseLog._CDC_Datas CDC_Datas = new ParseLog._CDC_Datas();
                _ParseLog.Transfer_CDC(FileEmployee[iCDC_File], SelectIndex, ref CDC_Datas);

                if (SelectIndex == 1)
                {
                    MutualData_1stFrame.AddRange(CDC_Datas.CDC);
                    SelfData_1stFrame[enSelf.Sense].AddRange(CDC_Datas.Self_X);
                    SelfData_1stFrame[enSelf.Drive].AddRange(CDC_Datas.Self_Y);
                }

                CalculateDataFormat(CDC_Datas.CDC, CDC_Datas.Self_X, CDC_Datas.Self_Y);
                RePainting(FileIdx.Painting_1, SelectIndex);
                RePainting(FileIdx.Painting_2, SelectIndex);

                //Update UI Chart                     
                short[] sArrary = CDC_Datas.DMsg.ToArray();
                ShowFingerColorGrid(CDC_Datas.CDC, CDC_Datas.Self_X, CDC_Datas.Self_Y, sArrary, SelectIndex);
            }
            else
            {
                List<Int16> PEN_CDC_X, PEN_CDC_Y, PEN_DMSG;
                if (!_ParseLog.Transfer_PenCDC(FileEmployee[(int)FileIdx.CDC_File], SelectIndex, out PEN_CDC_X, out PEN_CDC_Y, out PEN_DMSG))
                    return false;
                if (PEN_CDC_X.Count == 0 || PEN_CDC_Y.Count == 0)
                    return false;
                if (SelectIndex == 1)
                {
                    PenData_1stFrame[enPenCDC._X].AddRange(PEN_CDC_X);
                    PenData_1stFrame[enPenCDC._Y].AddRange(PEN_CDC_Y);
                }
                CalculatePenDataFormat(PEN_CDC_X, PEN_CDC_Y);
                RePainting(FileIdx.Painting_1, SelectIndex);
                RePainting(FileIdx.Painting_2, SelectIndex);

                //Update UI Chart                     
                short[] sArrary = PEN_DMSG.ToArray();
                ShowPenCoor_Lable();
                ShowPenColorGrid(PEN_CDC_X, PEN_CDC_Y, sArrary, SelectIndex);
            }
            isFinishReplayByScrollBar = true;
            return true;
        }

        private void RefreshAllChart()
        {

        }

        private void Axis_init(int X, int Y, out int[] X_arr, out int[] Y_arr)
        {
            X_arr = new int[X * Y];
            Y_arr = new int[X * Y];

            for (int y = 0, i = 0; y < Y; y++)
            {
                for (int x = 0; x < X; x++)
                {
                    X_arr[i] = x + 1;
                    Y_arr[i++] = y + 1;
                }
            }
        }

        private void Line_init(ref Steema.TeeChart.Styles.HorizLine line)
        {
            line.LinePen.Width = 3;

            line.Pointer.Visible = false;
            line.Title = "Line";
            line.XValues.DataMember = "X";
            line.YValues.DataMember = "Y";

            line.Clear();
        }

        private Random rnd = new Random();

        private void Point_init(ref Steema.TeeChart.Styles.Points point)
        {
            point.Color = System.Drawing.Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));

            point.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle;
            point.Pointer.VertSize = 5;
            point.Pointer.HorizSize = 5;

            point.Pointer.Dark3D = false;
            point.Pointer.Draw3D = false;
            point.Pointer.Gradient.Visible = false;
            point.Pointer.InflateMargins = false;

            point.Pointer.Pen.Visible = false;
            point.Title = "Point";
            point.XValues.DataMember = "X";
            point.YValues.DataMember = "Y";

            point.Pointer.Pen.Color = System.Drawing.Color.Red;
            point.Pointer.Pen.Style = System.Drawing.Drawing2D.DashStyle.Dot;
            point.Pointer.Pen.Width = 4;
            point.Pointer.Pen.Visible = false;

            point.Clear();

        }

        //public void Reset_GridColor(ref Steema.TeeChart.Styles.ColorGrid ColorGrid, short[] data)
        //{
        //    int max = data.Max(), min = data.Min();

        //    ColorGrid.PaletteMin = min;

        //    if (max - min < 5)
        //    {
        //        ColorGrid.PaletteStep = 1;
        //        ColorGrid.PaletteSteps = 5;
        //    }
        //    else
        //    {
        //        if (max - min > 256)
        //        {
        //            ColorGrid.PaletteStep = (max - min) / 256;
        //            ColorGrid.PaletteSteps = 256;
        //        }
        //        else
        //        {
        //            ColorGrid.PaletteStep = 1;
        //            ColorGrid.PaletteSteps = max - min;
        //        }
        //    }
        //    ColorGrid.UsePaletteMin = true;
        //}

        private void ShowMarks_MenuItem_Click(object sender, EventArgs e)
        {
            bool bFlag = ShowMarks_MenuItem.Checked;
            ShowMarks_MenuItem.Checked = !bFlag;
            userControl_MC.isShowMarks = !bFlag;
            userControl_SC_X.isShowMarks = !bFlag;
            userControl_SC_Y.isShowMarks = !bFlag;
            userControl_Pen_X.isShowMarks = !bFlag;
            userControl_Pen_Y.isShowMarks = !bFlag;
        }

        private void C_Modelfm_Replay_Resize(object sender, EventArgs e)
        {
            var iPainting_1 = (int)FileIdx.Painting_1;
            var iPainting_2 = (int)FileIdx.Painting_2;
            //Clear_Painting();
            var iCDC_File = (int)FileIdx.CDC_File;

            if (splitMC_SCX.Height > splitMC_SCX.SplitterDistance)
                splitMC_SCX.SplitterDistance = (splitMC_SCX.Height / 5) * 4;

            if (splitMC_SCX_SCY.Width > splitMC_SCX_SCY.SplitterDistance)
            {
                splitMC_SCX_SCY.SplitterDistance = (splitMC_SCX_SCY.Width / 4) * 3;
                splitContainer_DBG_PlayBar.SplitterDistance = (splitMC_SCX_SCY.Width / 4) * 3;
            }


            ChangeResolution();
            if (FileEmployee[iCDC_File].FileName == "")
                return;

            userControl_PlayBar_Cmodel1.InitialPaint1Mark(FileEmployee[iCDC_File].iTotal_Frame_Count);
            userControl_PlayBar_Cmodel1.InitialPaint2Mark(FileEmployee[iCDC_File].iTotal_Frame_Count);
            if (FileEmployee[iPainting_1].FileName != "")
            {
                for (int iFrame = 1; iFrame <= FileEmployee[iCDC_File].iTotal_Frame_Count; iFrame++)
                {
                    List<byte> PaintArr = new List<byte>();
                    if (_ParseLog.Transfer_Painting(FileEmployee[iPainting_1], iFrame, out PaintArr))
                    {
                        userControl_PlayBar_Cmodel1.SetPaint1Mark(iFrame);
                    }
                }
                userControl_PlayBar_Cmodel1.UpdatePaintBar();
            }
            if (FileEmployee[iPainting_2].FileName != "")
            {
                for (int iFrame = 1; iFrame <= FileEmployee[iCDC_File].iTotal_Frame_Count; iFrame++)
                {
                    List<byte> PaintArr = new List<byte>();
                    if (_ParseLog.Transfer_Painting(FileEmployee[iPainting_2], iFrame, out PaintArr))
                    {
                        userControl_PlayBar_Cmodel1.SetPaint2Mark(iFrame);
                    }
                }
                userControl_PlayBar_Cmodel1.UpdatePaintBar();
            }
            if (!userControl_PlayBar_Cmodel1.FrameNum_Text.Equals("0"))
                StartReplay_Frame(int.Parse(userControl_PlayBar_Cmodel1.FrameNum_Text));
        }

        private void InitFingerColor()
        {
            int itmp = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iMaxTouchPoint;
            for (int inCount = 0; inCount < itmp; inCount++)
            {
                var _Color = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[enBackGroundColor.White][inCount]);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.coFingerColor[inCount] = _Color;

                _Color = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[enBackGroundColor.Black][inCount]);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inFingerColorRBG[inCount] = _Color;

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ColorPen[inCount] = GDI_Pen.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 2, _Color);// Pen
            }
        }

        public void DrawingLine_Finger()
        {
            int iFingerNum = 0;
            bool bDataContinueIn = false;
            var scale_W = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.douScale_Width;
            var scale_H = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.douScale_Height;

            //userControl_MC.ClearPaintingBitmap();

            for (int iFingIndx = 0; iFingIndx < UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.iFingerID; iFingIndx++)
            {
                //fingerTmp[iFingIndx].point.Clear();
                iFingerNum = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.iChangeFlag[iFingIndx];
                Color colorTmp = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inFingerColorRBG[iFingerNum];

                var oldFinger = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicFinger_Old[PaintCount][iFingerNum];
                var newFinger = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicFinger_New[PaintCount][iFingerNum];

                bDataContinueIn = oldFinger.bDataIn && newFinger.bDataIn;

                Point Old_Position = new Point((int)(oldFinger.iPosition_X * scale_W), (int)(oldFinger.iPosition_Y * scale_H));
                Point New_Position = new Point((int)(newFinger.iPosition_X * scale_W), (int)(newFinger.iPosition_Y * scale_H));

                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bSWAPXY)
                {
                    Old_Position = new Point((int)(oldFinger.iPosition_Y * scale_W), (int)(oldFinger.iPosition_X * scale_H));
                    New_Position = new Point((int)(newFinger.iPosition_Y * scale_W), (int)(newFinger.iPosition_X * scale_H));
                }

                if (!bDataContinueIn)
                    continue;


                Color _Color = new Color();
                if (PaintCount == enPaintingCount.Painting1)
                    _Color = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[enBackGroundColor.White][iFingerNum]);
                else
                    _Color = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[enBackGroundColor.Black][iFingerNum]);

                var _PenHandle = GDI_Pen.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 2, _Color);
                GDI_Pen.AssignPenHandle(_PenHandle);
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicFinger_New[PaintCount][iFingerNum].bTouch)
                {
                    //userControl_MC.DrawLine(PaintCount,false, new Point(oldFinger.iPosition_X, oldFinger.iPosition_Y), new Point(newFinger.iPosition_X, newFinger.iPosition_Y));
                    userControl_MC.DrawLine(_Color, new Point(oldFinger.iPosition_X, oldFinger.iPosition_Y), new Point(newFinger.iPosition_X, newFinger.iPosition_Y), PaintCount, iFingerNum);

                    GDI_Pen.MoveTo(Old_Position);
                    GDI_Pen.LineTo(New_Position);

                    if (bIsShowTrajectory)
                    {
                        GDI_Pen.Ellipse(New_Position, 3);
                    }

                    drawPen = new Pen(_Color, 2);
                    drawPen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
                    drawPen.LineJoin = LineJoin.Round;
                    using (PaintDrawGrp = Graphics.FromImage(bitPaintDraw))
                    {
                        PaintDrawGrp.DrawLine(drawPen, Old_Position, New_Position);
                        if (bIsShowTrajectory)
                            PaintDrawGrp.DrawEllipse(drawPen, New_Position.X - 6 / 2, New_Position.Y - 6 / 2, 6, 6);
                    }


                    var tmp = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicFinger_New[PaintCount][iFingerNum];
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicFinger_Old[PaintCount][iFingerNum] = new MultiPaintTool_VAR.FingerStatus(tmp.bDataIn, tmp.bTouch, tmp.iPosition_X, tmp.iPosition_Y, tmp.iPressure);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicFinger_New[PaintCount][iFingerNum] = new MultiPaintTool_VAR.FingerStatus();
                }
                else
                {
                    if (!Point.Equals(Old_Position, New_Position))
                    {
                        //userControl_MC.DrawLine(PaintCount, false, new Point(oldFinger.iPosition_X, oldFinger.iPosition_Y), new Point(newFinger.iPosition_X, newFinger.iPosition_Y));
                        userControl_MC.DrawLine(_Color, new Point(oldFinger.iPosition_X, oldFinger.iPosition_Y), new Point(newFinger.iPosition_X, newFinger.iPosition_Y), PaintCount, iFingerNum);

                        GDI_Pen.MoveTo(Old_Position);
                        GDI_Pen.LineTo(New_Position);

                        if (bIsShowTrajectory)
                        {
                            GDI_Pen.Ellipse(New_Position, 3);
                        }

                        drawPen = new Pen(_Color, 2);
                        drawPen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
                        drawPen.LineJoin = LineJoin.Round;
                        using (PaintDrawGrp = Graphics.FromImage(bitPaintDraw))
                        {
                            PaintDrawGrp.DrawLine(drawPen, Old_Position, New_Position);
                            if (bIsShowTrajectory)
                                PaintDrawGrp.DrawEllipse(drawPen, New_Position.X - 6 / 2, New_Position.Y - 6 / 2, 6, 6);
                        }
                    }
                    //fingerTmp[iFingerNum].line_point_cnt = 0;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicFinger_Old[PaintCount][iFingerNum] = new MultiPaintTool_VAR.FingerStatus();
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicFinger_New[PaintCount][iFingerNum] = new MultiPaintTool_VAR.FingerStatus();
                }
                GDI_Pen.DeleteOBJECT(_PenHandle);
            }
            ShowFingerCoor_Lable();
        }

        public void DrawingLine_Pen()
        {
            var scale_W = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.douScale_Width;
            var scale_H = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.douScale_Height;

            var oldPen = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicPen_Old[PaintCount][0];
            var newPen = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicPen_New[PaintCount][0];

            bool isDataClear = !UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicPen_Old[PaintCount][0].bInRange && !UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicPen_New[PaintCount][0].bInRange;
            if (isDataClear && (oldPen.iPosition_X == 0 && oldPen.iPosition_Y == 0 && newPen.iPosition_X == 0 && newPen.iPosition_Y == 0))
                return;
            //    Console.Write("{0} and {1}\r\n", newPen.iPosition_X, newPen.iPosition_Y);
            Point Old_Position = new Point((int)(oldPen.iPosition_X * scale_W), (int)(oldPen.iPosition_Y * scale_H));
            Point New_Position = new Point((int)(newPen.iPosition_X * scale_W), (int)(newPen.iPosition_Y * scale_H));

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Settings.bSWAPXY)
            {
                Old_Position = new Point((int)(oldPen.iPosition_Y * scale_W), (int)(oldPen.iPosition_X * scale_H));
                New_Position = new Point((int)(newPen.iPosition_Y * scale_W), (int)(newPen.iPosition_X * scale_H));
            }

            Color color;
            if (PaintCount == enPaintingCount.Painting1)
            {
                color = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[enBackGroundColor.White][(int)enPenTermNumber]);
                if (!DicPenCoor_Painting1.ContainsKey((int)enPenTermNumber))
                    DicPenCoor_Painting1.Add((int)enPenTermNumber, new Point(newPen.iPosition_X, newPen.iPosition_Y));
                else
                    DicPenCoor_Painting1[(int)enPenTermNumber] = new Point(newPen.iPosition_X, newPen.iPosition_Y);
            }

            else
            {
                color = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[enBackGroundColor.Black][(int)enPenTermNumber]);

                if (!DicPenCoor_Painting2.ContainsKey((int)enPenTermNumber))
                    DicPenCoor_Painting2.Add((int)enPenTermNumber, new Point(newPen.iPosition_X, newPen.iPosition_Y));
                else
                    DicPenCoor_Painting2[(int)enPenTermNumber] = new Point(newPen.iPosition_X, newPen.iPosition_Y);

            }


            var _PenHandle = GDI_Pen.CreatePenHandle(Gdi32.PenStyles.PS_SOLID, 2, color);
            GDI_Pen.AssignPenHandle(_PenHandle);

            if (newPen.bTouch)
            {
                if (bIsUpdatePenMCPaintDot)
                    userControl_MC.DrawLine(color, new Point(oldPen.iPosition_X, oldPen.iPosition_Y), new Point(newPen.iPosition_X, newPen.iPosition_Y), PaintCount, 0);

                GDI_Pen.MoveTo(Old_Position);
                GDI_Pen.LineTo(New_Position);

                if (bIsShowTrajectory)
                {
                    GDI_Pen.Ellipse(New_Position, 3);
                }

                drawPen = new Pen(color, 2);
                drawPen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
                drawPen.LineJoin = LineJoin.Round;
                using (PaintDrawGrp = Graphics.FromImage(bitPaintDraw))
                {
                    PaintDrawGrp.DrawLine(drawPen, Old_Position, New_Position);
                    if (bIsShowTrajectory)
                        PaintDrawGrp.DrawEllipse(drawPen, New_Position.X - 6 / 2, New_Position.Y - 6 / 2, 6, 6);
                }



                dicHoverControl[PaintCount].Visible = false;

                isTouchDown = true;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicPen_Old[PaintCount][0] = new MultiPaintTool_VAR.PenStatus(newPen);
            }
            else
            {
                if (!Point.Equals(Old_Position, New_Position))
                {
                    if (bIsUpdatePenMCPaintDot)
                        userControl_MC.DrawLine(color, new Point(oldPen.iPosition_X, oldPen.iPosition_Y), new Point(newPen.iPosition_X, newPen.iPosition_Y), PaintCount, 0);
                }

                if (newPen.bInRange)
                {
                    if (bIsUpdatePenMCPaintDot)
                        userControl_MC.DrawLine(color, new Point(oldPen.iPosition_X, oldPen.iPosition_Y), new Point(newPen.iPosition_X, newPen.iPosition_Y), PaintCount, 0);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicPen_Old[PaintCount][0] = new MultiPaintTool_VAR.PenStatus(newPen);
                }
                else
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicPen_Old[PaintCount][0] = new MultiPaintTool_VAR.PenStatus();
                }
                dicHoverControl[PaintCount].Visible = true;
                dicHoverControl[PaintCount].Location = new Point(New_Position.X - (dicHoverControl[PaintCount].Width / 2), New_Position.Y - (dicHoverControl[PaintCount].Height / 2));

                if (isTouchDown)
                    isTouchDown = false;

            }
            dicHoverControl[PaintCount].Visible = (newPen.bInRange && !isTouchDown) ? true : false;

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicPen_New[PaintCount][0] = new MultiPaintTool_VAR.PenStatus();
            GDI_Pen.DeleteOBJECT(_PenHandle);
        }

        private void axis_CheckStateChanged(object sender, EventArgs e)
        {
            var _tmp = sender as ToolStripMenuItem;

            if (_tmp == axisXToolStripMenuItem)
            {
                userControl_MC.XInverted = (_tmp.CheckState == CheckState.Checked);
                userControl_SC_X.XInverted = (_tmp.CheckState == CheckState.Checked);
                userControl_SC_Y.XInverted = (_tmp.CheckState == CheckState.Checked);
                userControl_Pen_X.XInverted = (_tmp.CheckState == CheckState.Checked);
                userControl_Pen_Y.XInverted = (_tmp.CheckState == CheckState.Checked);

            }
            else if (_tmp == axisYToolStripMenuItem)
            {
                userControl_MC.YInverted = (_tmp.CheckState == CheckState.Checked);
                userControl_SC_X.YInverted = (_tmp.CheckState == CheckState.Checked);
                userControl_SC_Y.YInverted = (_tmp.CheckState == CheckState.Checked);
                userControl_Pen_X.YInverted = (_tmp.CheckState == CheckState.Checked);
                userControl_Pen_Y.YInverted = (_tmp.CheckState == CheckState.Checked);
            }

            else if (_tmp == reversePaintXToolStripMenuItem)
            {
                userControl_MC.PaintX_Inverted = (_tmp.CheckState == CheckState.Checked);
            }

            else if (_tmp == reversePaintYToolStripMenuItem)
            {
                userControl_MC.PaintY_Inverted = (_tmp.CheckState == CheckState.Checked);
            }
        }

        private void uiDataGridView_info_Update()
        {
            uiDataGridView_info.ClearRows();

            foreach (C_MODEL_DATA_INFO item in Enum.GetValues(typeof(C_MODEL_DATA_INFO)))
            {
                string[] desc = item.DescArr(':');
                string name = desc[0];
                string val = desc[1];

                uiDataGridView_info.AddRow(new object[] { name, val });
            }

            dmsg_dict_selected.Clear();
            foreach (C_MODEL_DMSG_TYPE item in Enum.GetValues(typeof(C_MODEL_DMSG_TYPE)))
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo == null)
                    return;
                if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iCModelDmsgType & (int)item) == 0)
                    continue;

                string desc = item.Desc();
                string idx = (desc.Contains(':')) ? desc.Split(':').Last() : desc;

                /* Algo */
                if (!idx.Contains("B'"))
                {
                    if (item == C_MODEL_DMSG_TYPE.TOUCH_ALGO)
                    {
                        dmsg_dict_selected.Add(idx, "");
                        foreach (var itemAlgo in dmsg_touchAlgo)
                        {
                            uiDataGridView_info.AddRow(new object[] { itemAlgo.Value, "" });
                        }
                    }
                    else //Pen Algo
                    {
                        dmsg_dict_selected.Add(idx, "");
                        uiDataGridView_info.AddRow(new object[] { idx, "" });
                    }
                    continue;
                }

                object dict = (item < C_MODEL_DMSG_TYPE.PEN_ALGO) ?
                    dmsg_touch_dict_total : dmsg_pen_dict_total;

                foreach (var v in dict as Dictionary<string, string>)
                {
                    if (!v.Value.Contains(idx))
                        continue;

                    if (dmsg_dict_selected.ContainsKey(v.Key))
                        break;


                    if (!v.Key.Contains("SNR_X") && !v.Key.Contains("SNR_Y"))
                    {
                        dmsg_dict_selected.Add(v.Key, v.Value);
                        uiDataGridView_info.AddRow(new object[] { v.Key, "" });

                    }
                    else
                    {
                        for (int i = 0; i < UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iPen_TermCount; i++)
                        {
                            if (!dmsg_dict_selected.ContainsKey(v.Key + "_" + i.ToString()))
                            {
                                dmsg_dict_selected.Add(v.Key + "_" + i.ToString(), v.Value);
                                uiDataGridView_info.AddRow(new object[] { v.Key + "_" + i.ToString(), "" });
                            }
                        }
                    }
                }
            }
        }

        private void UpdateDmsgByIndex(int iIndex)
        {
            int row_start = Enum.GetNames(typeof(C_MODEL_DATA_INFO)).Length;

            for (int i = 0; i < lsAllDBGData[iIndex - 1].Length; i++)
            {
                if (lsAllDBGData[iIndex - 1][i] != "N/A")
                    Set_DataGridView_Val(row_start + i, string.Format("{0}", lsAllDBGData[iIndex - 1][i]));
            }
        }

        private void UpdatePenDmsg(ref short[] buf)
        {
            int row_start = Enum.GetNames(typeof(C_MODEL_DATA_INFO)).Length;
            int iIndex = 0;

            if (dmsg_dict_selected.ContainsKey("Algo_Pen_Stage"))
                iIndex = buf[buf.Length - 1];

            for (int i = 0, idx = 0; i < dmsg_dict_selected.Count; i++)
            {
                string key = dmsg_dict_selected.ElementAt(i).Key;
                string format = dmsg_dict_selected.ElementAt(i).Value;
                uint val = 0;

                if (key.Contains("Pen_SNR_X"))
                {
                    enPenSNRX PenSNRXIndex = (enPenSNRX)Enum.Parse(typeof(enPenSNRX), key);
                    if ((int)PenSNRXIndex != iIndex)
                        continue;

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[(int)PenSNRXIndex]++;
                    if (!DicSNRXCalculate.ContainsKey(PenSNRXIndex))
                        DicSNRXCalculate.Add(PenSNRXIndex, new List<float>());

                    int cnt = format.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Count();


                    val = (uint)buf[idx++];
                    for (int j = 1; j < cnt; j++)
                        val |= (uint)(buf[idx++] << (j * 8));

                    DicSNRXCalculate[PenSNRXIndex].Add(val);

                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[(int)PenSNRXIndex] >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iSNR_FrameCount)
                    {
                        var tmp = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.CalculateSNR(DicSNRXCalculate[PenSNRXIndex]);
                        Set_DataGridView_Val(row_start + i, string.Format("{0}", tmp));
                        DicSNRXCalculate[PenSNRXIndex].Clear();
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_X_FrameCounts[(int)PenSNRXIndex] = 0;
                    }
                    else
                    {
                        continue;
                    }

                }
                else if (key.Contains("Pen_SNR_Y"))
                {
                    enPenSNRY PenSNRYIndex = (enPenSNRY)Enum.Parse(typeof(enPenSNRY), key);
                    if ((int)PenSNRYIndex != iIndex)
                        continue;

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[(int)PenSNRYIndex]++;
                    if (!DicSNRYCalculate.ContainsKey(PenSNRYIndex))
                        DicSNRYCalculate.Add(PenSNRYIndex, new List<float>());

                    int cnt = format.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Count();

                    val = (uint)buf[idx++];
                    for (int j = 1; j < cnt; j++)
                        val |= (uint)(buf[idx++] << (j * 8));

                    DicSNRYCalculate[PenSNRYIndex].Add(val);

                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[(int)PenSNRYIndex] >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.stuReportIndex.iSNR_FrameCount)
                    {
                        var tmp = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._PaintTool.CalculateSNR(DicSNRYCalculate[PenSNRYIndex]);
                        Set_DataGridView_Val(row_start + i, string.Format("{0}", tmp));
                        DicSNRYCalculate[PenSNRYIndex].Clear();
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.Pen_SNR_Y_FrameCounts[(int)PenSNRYIndex] = 0;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    int cnt = format.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Count();

                    val = (uint)buf[idx++];
                    for (int j = 1; j < cnt; j++)
                        val |= (uint)(buf[idx++] << (j * 8));

                    Set_DataGridView_Val(row_start + i, string.Format("{0}", val));
                }
            }
        }

        private void Set_DataGridView_Val(int row, string val)
        {
            //this.Invoke(new Action(() =>
            //{
            uiDataGridView_info.Rows[row].Cells[1].Value = val;
            //}));
        }

        private void UpdateDataGridView_FileEidt(bool isWaitEnd, List<int> lsStartIndex, List<int> lsEndIndex)
        {
            tabControl1.SelectedTab = tpFileEdit;
            if (!isWaitEnd)
            {
                uiDataGridView_FileEidt.AddRow(new object[] { lsStartIndex[lsStartIndex.Count - 1].ToString(), "" });
                isWaitEnd = true;
            }
            else
            {
                int iEndIndex = uiDataGridView_FileEidt.RowCount - 1;
                uiDataGridView_FileEidt.Rows[iEndIndex].Cells[1].Value = lsEndIndex[lsEndIndex.Count - 1].ToString();
                isWaitEnd = false;
            }


        }

        private void uiDataGridView_FileEidt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                for (int i = 0; i < uiDataGridView_FileEidt.RowCount; i++)
                {
                    if (uiDataGridView_FileEidt.Rows[i].Selected)
                    {
                        userControl_PlayBar_Cmodel1.ResetPaint1Mark(uiDataGridView_FileEidt.Rows[i].Cells[0].Value.ToString(), uiDataGridView_FileEidt.Rows[i].Cells[1].Value.ToString());

                        userControl_PlayBar_Cmodel1.lsStartPos.Remove(Convert.ToInt32(uiDataGridView_FileEidt.Rows[i].Cells[0].Value.ToString()));

                        if (uiDataGridView_FileEidt.Rows[i].Cells[1].Value.ToString() != "")
                        {
                            userControl_PlayBar_Cmodel1.lsEndPos.Remove(Convert.ToInt32(uiDataGridView_FileEidt.Rows[i].Cells[1].Value.ToString()));
                        }

                        uiDataGridView_FileEidt.Rows.RemoveAt(uiDataGridView_FileEidt.Rows[i].Index);

                        userControl_PlayBar_Cmodel1.isWaitEnd = (userControl_PlayBar_Cmodel1.lsStartPos.Count != userControl_PlayBar_Cmodel1.lsEndPos.Count) ? true : false;

                    }
                }
            }
        }

        private void uiDataGridView_FileEidt_DeleteAll()
        {
            uiDataGridView_FileEidt.Rows.Clear();
            userControl_PlayBar_Cmodel1.lsStartPos.Clear();
            userControl_PlayBar_Cmodel1.lsEndPos.Clear();

        }

        private void GenNewCsvFile(object sender, EventArgs e)
        {
            var iCDC = (int)FileIdx.CDC_File;
            var iPainting_1 = (int)FileIdx.Painting_1;
            var iPainting_2 = (int)FileIdx.Painting_2;

            List<int> lsStartPos = userControl_PlayBar_Cmodel1.lsStartPos;
            List<int> lsEndPos = userControl_PlayBar_Cmodel1.lsEndPos;

            int iStartCount = lsStartPos.Count;
            int iEndCount = lsEndPos.Count;
            if (iStartCount != iEndCount)
            {
                MyUIPage.ShowErrorDialog("Error", "Start and End need to appear in pairs.", UIStyle.Red, true);
                return;
            }

            if (!string.IsNullOrEmpty(FileEmployee[iCDC].FileName))
            {
                GenCsvFile(lsStartPos, lsEndPos);
            }

            if (!string.IsNullOrEmpty(FileEmployee[iPainting_1].FileName))
            {
                GenPaintFile(lsStartPos, lsEndPos, (int)FileIdx.Painting_1);
            }

            if (!string.IsNullOrEmpty(FileEmployee[iPainting_2].FileName))
            {
                GenPaintFile(lsStartPos, lsEndPos, (int)FileIdx.Painting_2);
            }
            MyUIPage.ShowInfoDialog("File generation complete!");
        }

        private void GenCsvFile(List<int> lsStartPos, List<int> lsEndPos)
        {
            var iCDC = (int)FileIdx.CDC_File;
            string lastFolderName = Path.GetDirectoryName(FileEmployee[iCDC].FileName) + "\\GenCsvFolder\\";
            string lastFileName = Path.GetFileName(FileEmployee[iCDC].FileName);
            //Console.Write(lastFolderName);
            if (!Directory.Exists(lastFolderName))
            {
                System.IO.Directory.CreateDirectory(lastFolderName);//不存在就建立目錄 
            }

            string csvpath = lastFolderName + lastFileName.Replace(".cdc", "_Edited") + ".cdc";

            ParseLog._CDC_Datas CDC_Datas = new ParseLog._CDC_Datas();
            List<string> _OneFrameBuff;
            using (StreamWriter Sw = new StreamWriter(csvpath))
            {
                _OneFrameBuff = new List<string>();
                _ParseLog.SeekReadFileRange_EndTag_2(FileEmployee[iCDC].FileName, 0, "INFO_START_TAG", string.Format("#{0}@", 1), ref _OneFrameBuff);
                int iTotalFramCount = 0;
                for (int i = 0; i < lsStartPos.Count; i++)
                {
                    iTotalFramCount += (lsEndPos[i] - lsStartPos[i] + 1);
                }
                _OneFrameBuff[1] = string.Format("Total_Frame_Count: {0}", (iTotalFramCount));

                foreach (String s in _OneFrameBuff)
                {
                    Sw.WriteLine(s);
                }
                int iReCountTimeTemp = 0;
                int iTempCount = 0;
                for (int i = 0; i < lsStartPos.Count; i++)
                {
                    for (int y = lsStartPos[i]; y <= lsEndPos[i]; y++)
                    {
                        int iOffset = lsStartPos[i] - 1;
                        if (!_ParseLog.Transfer_CDC(FileEmployee[iCDC], y, ref CDC_Datas, true))
                            continue;

                        List<string> RecordRawData = _ParseLog.OneFrameBuff;

                        if (i == 0 && y - iOffset == 1) //第一個Frame
                        {
                            RecordRawData[0] = string.Format("#{0}@{1}ms", y - iOffset, 0); //0
                        }
                        else
                        {
                            iReCountTimeTemp = Convert.ToInt32(RecordRawData[0].Split('@')[1].Replace("ms", ""));
                            RecordRawData[0] = string.Format("#{0}@{1}ms", (y - iOffset) + iTempCount, iReCountTimeTemp);
                        }
                        foreach (String s in RecordRawData)
                        {
                            Sw.WriteLine(s);
                        }
                    }
                    iTempCount = iTempCount + (lsEndPos[i] - lsStartPos[i]) + 1;
                }
                Sw.Close();
                Sw.Dispose();
            }

        }

        private void GenPaintFile(List<int> lsStartPos, List<int> lsEndPos, int iPaintType)
        {
            string lastFolderName = Path.GetDirectoryName(FileEmployee[iPaintType].FileName) + "\\GenCsvFolder\\";
            string lastFileName = Path.GetFileName(FileEmployee[iPaintType].FileName);
            Console.Write(lastFolderName);
            if (!Directory.Exists(lastFolderName))
            {
                System.IO.Directory.CreateDirectory(lastFolderName);//不存在就建立目錄 
            }

            string csvpath = lastFolderName + lastFileName.Replace(".paint", "_Edited") + ".paint";

            List<byte> PaintArr = new List<byte>();
            List<string> _OneFrameBuff;
            using (StreamWriter Sw = new StreamWriter(csvpath))
            {
                _OneFrameBuff = new List<string>();
                _ParseLog.SeekReadFileRange_EndTag_2(FileEmployee[iPaintType].FileName, 0, "ILITek Test Studio Utility", string.Format("#", 1), ref _OneFrameBuff);

                foreach (String s in _OneFrameBuff)
                {
                    Sw.WriteLine(s);
                }
                int iReCountTimeTemp = 0;
                int iTempCount = 0;
                int iSuccessCount = 0;
                for (int i = 0; i < lsStartPos.Count; i++)
                {
                    iSuccessCount = 0;
                    for (int y = lsStartPos[i]; y <= lsEndPos[i]; y++)
                    {
                        int iOffset = lsStartPos[i] - 1;
                        if (_ParseLog.Transfer_Painting(FileEmployee[iPaintType], y, out PaintArr, true))
                        {
                            List<string> RecordRawData = _ParseLog.OneFrameBuff;
                            int iPacketCount = RecordRawData.Count / 3;
                            int iTemp = 0;
                            if (i == 0 && y == lsStartPos[i]) //第一個Frame
                            {
                                for (int s = 0; s < iPacketCount; s++)
                                {
                                    RecordRawData[iTemp] = string.Format("#{0}@{1}ms", y - iOffset, 0); //0
                                    iTemp += 3;
                                }
                            }
                            else
                            {
                                for (int s = 0; s < iPacketCount; s++)
                                {
                                    iReCountTimeTemp = Convert.ToInt32(RecordRawData[iTemp].Split('@')[1].Replace("ms", ""));
                                    RecordRawData[iTemp] = string.Format("#{0}@{1}ms", (y - iOffset) + iTempCount, iReCountTimeTemp);
                                    iTemp += 3;
                                }
                            }
                            foreach (String s in RecordRawData)
                            {
                                Sw.WriteLine(s);
                            }
                        }
                        iSuccessCount++;
                    }
                    iTempCount = iTempCount + iSuccessCount;
                }
                Sw.Close();
                Sw.Dispose();
            }

        }

        private void MenuItem_DataFormatItem(enCDCDataFormat Selected)
        {
            int inDataFormatItemCount = Enum.GetNames(typeof(enCDCDataFormat)).Count();
            strDataFormatItemText = new string[inDataFormatItemCount];
            MenuItem_DataFormat.DropDownItems.Clear();
            StripCB_DataFormat = new ToolStripMenuItem[inDataFormatItemCount];

            foreach (enCDCDataFormat item in (enCDCDataFormat[])Enum.GetValues(typeof(enCDCDataFormat)))
            {
                StripCB_DataFormat[(int)item] = new ToolStripMenuItem();
                StripCB_DataFormat[(int)item].Size = new System.Drawing.Size(152, 22);
                StripCB_DataFormat[(int)item].Text = item.Desc();
                StripCB_DataFormat[(int)item].Click += new System.EventHandler(StripCB_DataFormat_Mutual_Click);
                StripCB_DataFormat[(int)item].Checked = false;
                strDataFormatItemText[(int)item] = item.Desc();
                MenuItem_DataFormat.DropDownItems.Add(StripCB_DataFormat[(int)item]);
            }
            var iSelected = (int)Selected;
            StripCB_DataFormat[iSelected].Checked = true;
        }

        private void StripCB_DataFormat_Mutual_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem m = (ToolStripMenuItem)sender;
            int inSelectIndex = Array.FindIndex(strDataFormatItemText, s => s.Equals(m.Text));

            var item = StripCB_DataFormat.First(x => x.Checked == true);
            item.Checked = false;
            StripCB_DataFormat[inSelectIndex].Checked = true;

            var mySelect = EnumTool.GetValueFromDescription<enCDCDataFormat>(m.Text);

            UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType = mySelect;

            ResetAllMaxMinValue();

            //處理顯示
            //if (this.userControl_PlayBar1.TrackBar_Value == this.userControl_PlayBar1.TrackBar_Min)
            //    Replay_1stFrame();
            //Update_MutualPage();
        }

        private void ResetAllMaxMinValue()
        {
            iMutual_Max = int.MinValue;
            iMutual_Min = int.MaxValue;

            iSelfX_Max = int.MinValue;
            iSelfX_Min = int.MaxValue;

            iSelfY_Max = int.MinValue;
            iSelfY_Min = int.MaxValue;

            iPenX_Max = int.MinValue;
            iPenX_Min = int.MaxValue;

            iPenY_Max = int.MinValue;
            iPenY_Min = int.MaxValue;
        }

        private void OnReplay_Frame(Message msg)
        {
            for (int i = 0; i < uiDataGridView_info.Rows.Count; i++)
            {
                uiDataGridView_info.Rows[i].Cells[1].Value = "";
            }

            iProgess = (int)msg.WParam;
            StartReplay_Frame((int)msg.WParam);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!userControl_PlayBar_Cmodel1.Visible)
                return false;

            if (!isFinishReplayByScrollBar)
                return false;

            switch (keyData)
            {
                case Keys.Right:
                    userControl_PlayBar_Cmodel1.TrackBarValuePlus();
                    break;
                case Keys.Left:
                    userControl_PlayBar_Cmodel1.TrackBarValueReduce();
                    break;
                default:
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Clear_MenuItem_Click(object sender, EventArgs e)
        {
            userControl_MC.MappingResolution(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.X_RES, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.Y_RES);
            ChangeResolution();
        }

        private DialogResult SelectColor(ref Color _SelectColor)
        {
            ColorDialog colorDialog1 = new ColorDialog();

            // Show the dialog box
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
                _SelectColor = colorDialog1.Color;
            return result;
            // If the user clicks OK, set the background color of the form to the selected color
            //if(result == DialogResult.OK)
            //{
            //    //this.BackColor = colorDialog1.Color;
            //    userControl_MC.InitBrushColorRange(colorDialog1.Color, Color.Black, 0, 255);
            //}
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedUserControl_ColorView == null)
                return;
            var MySender = sender as ToolStripMenuItem;

            Color tmpColor = Color.White;
            if (SelectColor(ref tmpColor) == DialogResult.OK)
            {
                if (MySender == startColorToolStripMenuItem)
                    SelectedUserControl_ColorView.StartColor = tmpColor;
                else if (MySender == endColorToolStripMenuItem)
                    SelectedUserControl_ColorView.EndColor = tmpColor;
                else if (MySender == contactToolStripMenuItem)
                    SelectedUserControl_ColorView.Paint1ContactColor = tmpColor;
                else if (MySender == hoverToolStripMenuItem)
                    SelectedUserControl_ColorView.Paint1HoverColor = tmpColor;
                else if (MySender == contactToolStripMenuItem1)
                    SelectedUserControl_ColorView.Paint2ContactColor = tmpColor;
                else if (MySender == hoverToolStripMenuItem1)
                    SelectedUserControl_ColorView.Paint2HoverColor = tmpColor;

                SelectedUserControl_ColorView.InitBrushColorRange(ColorResolutionNumber);
            }

            SelectedUserControl_ColorView = null;
        }

        private UserControls.UserControl_ColorView SelectedUserControl_ColorView;

        private void userControl_MouseDown(object sender, MouseEventArgs e)
        {
            var Mysender = sender as UserControls.UserControl_ColorView;
            if (e.Button == MouseButtons.Right)
            {
                testToolStripMenuItem.Visible = false;
                SelectedUserControl_ColorView = Mysender;
            }
        }

        private void MenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            testToolStripMenuItem.Visible = false;
        }

        private void UIControl_CheckStateChanged(object sender, EventArgs e)
        {
            var _tmp = sender as ToolStripMenuItem;

            if (_tmp == MCToolStripMenuItem)
            {
                if (_tmp.CheckState == CheckState.Checked)
                {
                    splitMC_SCX_SCY.Panel1Collapsed = false;
                    splitMC_SCX.Panel1Collapsed = false;
                }
                else //MC隱藏
                {
                    if (SCXToolStripMenuItem.CheckState != CheckState.Checked)
                        splitMC_SCX_SCY.Panel1Collapsed = true;
                    else
                        splitMC_SCX.Panel1Collapsed = true;
                }
            }
            else if (_tmp == SCXToolStripMenuItem)
            {
                if (_tmp.CheckState == CheckState.Checked)
                {
                    splitMC_SCX_SCY.Panel1Collapsed = false;
                    splitMC_SCX.Panel2Collapsed = false;
                }
                else
                {
                    if (MCToolStripMenuItem.CheckState != CheckState.Checked)
                        splitMC_SCX_SCY.Panel1Collapsed = true;
                    else
                        splitMC_SCX.Panel2Collapsed = true;
                }
            }

            else if (_tmp == SCYToolStripMenuItem)
            {
                if (_tmp.CheckState == CheckState.Checked)
                {
                    splitMC_SCX_SCY.Panel2Collapsed = false;
                }
                else
                {
                    splitMC_SCX_SCY.Panel2Collapsed = true;
                }
            }
            userControl_MC.InvalidateUI();
            userControl_SC_X.InvalidateUI();
            userControl_SC_Y.InvalidateUI();
            userControl_Pen_X.InvalidateUI();
            userControl_Pen_Y.InvalidateUI();
        }

        private void ShowSupportLine_MenuItem_Click(object sender, EventArgs e)
        {
            bool bFlag = ShowSupportLine_MenuItem.Checked;
            ShowSupportLine_MenuItem.Checked = !bFlag;
            userControl_MC.isShowSupportLine = !bFlag;
        }

        private void ShowPaintPanel_MenuItem_Click(object sender, EventArgs e)
        {
            bool bFlag = ShowPaintPanel_MenuItem.Checked;
            ShowPaintPanel_MenuItem.Checked = !bFlag;

            splitContainerPainting.Panel1Collapsed = bFlag;

            userControl_MC.InvalidateUI();
            userControl_SC_X.InvalidateUI();
            userControl_SC_Y.InvalidateUI();
            userControl_Pen_X.InvalidateUI();
            userControl_Pen_Y.InvalidateUI();

            ChangeResolution();        
        }

        private void m_panAAscen_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        private void Draw(Graphics e)
        {
            BufferedGraphicsContext current = BufferedGraphicsManager.Current;
            BufferedGraphics bg;

            bg = current.Allocate(e, this.DisplayRectangle);
            Graphics g = bg.Graphics;//(3) 
            g.Clear(Color.White);

            Bitmap bitBackGround = new Bitmap(m_panAAscen.Width, m_panAAscen.Height);
            Graphics grp = Graphics.FromImage(bitBackGround);
            grp.DrawImage(global::ITS_Studio.Properties.Resources.ILIlogo,
                (bitBackGround.Width / 2) - global::ITS_Studio.Properties.Resources.ILIlogo.Width / 2,
                (bitBackGround.Height / 2) - global::ITS_Studio.Properties.Resources.ILIlogo.Height / 2);
            //if (mi_DrawObjects.Count == 0)
            //    return
            g.DrawImage(bitBackGround, 0, 0);
            g.DrawImage(bitPaintDraw, 0, 0);

            bg.Render();
            bg.Dispose();
        }

        private void ShowCoordinatesMenuItem_Click(object sender, EventArgs e)
        {
            bool bFlag = ShowCoordinatesMenuItem.Checked;
            ShowCoordinatesMenuItem.Checked = !bFlag;

            userControl_MC.isShowCoordinates = !bFlag;
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            var _sender = sender as SplitContainer;
            if (splitMC_SCX.Height < splitMC_SCX.SplitterDistance || splitSCY_DBG.Height < splitSCY_DBG.SplitterDistance)
                return;

            if (_sender == splitMC_SCX)
            {
                if (splitSCY_DBG.SplitterDistance != e.SplitY)
                    splitSCY_DBG.SplitterDistance = e.SplitY;
            }
            else if (_sender == splitSCY_DBG)
            {
                if (splitMC_SCX.SplitterDistance != e.SplitY)
                    splitMC_SCX.SplitterDistance = e.SplitY;
            }
        }

        private void ShowTrajectoryDot_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem MySender = sender as ToolStripMenuItem;
            bool bFlag = MySender.Checked;
            MySender.Checked = !bFlag;

            bIsShowTrajectory = !bFlag;
        }

        private void ShowPanelCoordinatesMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem MySender = sender as ToolStripMenuItem;
            bool bFlag = MySender.Checked;
            MySender.Checked = !bFlag;

            bIsDrawCoorOnPanel = !bFlag;

            if (!bIsDrawCoorOnPanel)
                m_panAAscen.Invalidate();

        }

        private void ShowFingerCoor_Lable()
        {
            if (!bIsDrawCoorOnPanel)
                return;
            // Finger Label       
            for (int inCount = 0; inCount < UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iMaxTouchPoint; inCount++)
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicFinger_Old[PaintCount][inCount].bTouch)
                {
                    string sFingerDetailData = string.Format("Finger_{0:D2} : {1:D04} , {2:D04}"
                        , inCount + 1
                        , UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicFinger_Old[PaintCount][inCount].iPosition_X
                        , UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.dicFinger_Old[PaintCount][inCount].iPosition_Y);

                    SetFingerInfo(inCount, sFingerDetailData);
                }
                else
                {
                    SetFingerInfo(inCount, "");
                }
            }
        }

        private void ShowPenCoor_Lable()
        {
            if (!bIsDrawCoorOnPanel)
                return;

            if (DicPenCoor_Painting1.Count > 0)
            {
                for (int inCount = 0; inCount < EnumTool.EnumToList<enPenTerm>().Count(); inCount++)
                {
                    if (DicPenCoor_Painting1.ContainsKey(inCount))
                    {
                        SetPenInfo(inCount, string.Format("Term_{0} : {1:D04} , {2:D04}"
                       , inCount
                       , DicPenCoor_Painting1[inCount].X
                       , DicPenCoor_Painting1[inCount].Y), enPaintingCount.Painting1);
                    }
                    else
                    {
                        SetPenInfo(inCount, "", enPaintingCount.Painting1);
                    }
                }
            }
            if (DicPenCoor_Painting2.Count > 0)
            {
                for (int inCount = 0; inCount < EnumTool.EnumToList<enPenTerm>().Count(); inCount++)
                {
                    if (DicPenCoor_Painting2.ContainsKey(inCount))
                    {
                        SetPenInfo(inCount, string.Format("Term_{0} : {1:D04} , {2:D04}"
                       , inCount
                       , DicPenCoor_Painting2[inCount].X
                       , DicPenCoor_Painting2[inCount].Y), enPaintingCount.Painting2);
                    }
                    else
                    {
                        SetPenInfo(inCount, "", enPaintingCount.Painting2);
                    }
                }
            }
        }

        private void SetFingerInfo(int Number, string str)
        {
            if (PaintCount == enPaintingCount.Painting1)
                Update_LabelLogText(ref LabelLog, _LabelLog.Label_Type.Finger, Number, str);
            else
                Update_LabelLogText(ref LabelLof_Paint2, _LabelLog.Label_Type.Finger, Number, str);
        }

        private void SetPenInfo(int Number, string str, enPaintingCount PaintNumber = enPaintingCount.Painting1)
        {
            if (PaintNumber == enPaintingCount.Painting1)
                Update_LabelLogText(ref LabelLog, _LabelLog.Label_Type.Pen, Number, str, PaintNumber);
            else
                Update_LabelLogText(ref LabelLof_Paint2, _LabelLog.Label_Type.Pen, Number, str, PaintNumber);
        }

        private void Update_LabelLogText(ref _LabelLog obj, _LabelLog.Label_Type obj_Type, int index, string Msg, enPaintingCount PaintNumber = enPaintingCount.Painting1)
        {
            Font clear = new System.Drawing.Font("Consolas", 9.0F, FontStyle.Bold);
            Color color;
            if (obj_Type == _LabelLog.Label_Type.Pen)
            {
                if (Msg == "")
                {
                    if (!string.Equals(obj.Pen[index].Value, Msg))
                    {
                        Point tmpPos = obj.Pen[index].Key;
                        MyPanelTest.BackColor = Color.Transparent;
                        MyPanelTest.Size = new Size(122, 15);
                        MyPanelTest.Location = tmpPos;
                        MyPanelTest.Visible = true;
                        MyPanelTest.Visible = false;

                        obj.Pen[index] = new KeyValuePair<Point, string>(tmpPos, Msg);
                    }
                }

                else //if ((!string.Equals(obj.Pen[index].Value, Msg)))
                {
                    if (PaintNumber == enPaintingCount.Painting1)
                        color = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[enBackGroundColor.White][index]);
                    else
                        color = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[enBackGroundColor.Black][index]);

                    Point tmpPos = obj.Pen[index].Key;
                    SetPaintText(m_panAAscen, obj.Pen[index].Value, tmpPos, Color.White, clear);

                    obj.Pen[index] = new KeyValuePair<Point, string>(tmpPos, Msg);
                    SetPaintText(m_panAAscen, Msg, obj.Pen[index].Key, color, null, 122);

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
                        MyPanelTest.Size = new Size(122, 15);
                        MyPanelTest.Location = tmpPos;
                        MyPanelTest.Visible = true;
                        MyPanelTest.Visible = false;

                        obj.Finger[index] = new KeyValuePair<Point, string>(tmpPos, Msg);
                    }
                }
                else //if (!string.Equals(obj.Finger[index].Value, Msg))
                {
                    if (PaintCount == enPaintingCount.Painting1)
                        color = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[enBackGroundColor.White][index]);
                    else
                        color = Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.SelectBasicColor[enBackGroundColor.Black][index]);

                    Point tmpPos = obj.Finger[index].Key;
                    //SetPaintText(m_panAAscen, obj.Finger[index].Value, tmpPos, Color.White, clear);

                    obj.Finger[index] = new KeyValuePair<Point, string>(tmpPos, Msg);
                    SetPaintText(m_panAAscen, Msg, obj.Finger[index].Key, color, null, 122);
                }
            }
        }

        private void SetPaintText(Panel inPanel, string str, Point Position, Color FontColor, Font _Font, int iRectangleWidith = 105)
        {
            //Graphics grp = this.m_panAAscen.CreateGraphics();
            Graphics grp = inPanel.CreateGraphics();
            grp.FillRectangle(new SolidBrush(Color.White), Position.X, Position.Y, iRectangleWidith, 15);
            //grp.Clear(Color.FromArgb((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.BackGroundColor));

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
                | BindingFlags.Instance | BindingFlags.NonPublic, null, m_panAAscen, new object[] { true });

            if (FontColor == Color.White)
            {
                mu_Clear.WaitOne();
                if (mu_Clear != null)
                    mu_Clear.ReleaseMutex();
            }
            else
            {
                mu_Clear.WaitOne();
                Font _arialBold = (_Font == null) ? new Font("Consolas", 7.0F, FontStyle.Regular) : _Font;
                TextRenderer.DrawText(grp, str, _arialBold, Position, FontColor, TextFormatFlags.EndEllipsis);
                if (mu_Clear != null)
                    mu_Clear.ReleaseMutex();
            }
        }

        private void userControl_PlayBar_Cmodel1_VisibleChanged(object sender, EventArgs e)
        {
            userControl_PlayBar_Cmodel1.Split_Opt_Panel2_En = false;
        }
    }
}
