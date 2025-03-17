using CommonExt;
using FuncMethodCmd;
using ITS_Studio.AllForms.SensorTest;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ThirdPartyTools;
using ITS_Studio.Components;

namespace ITS_Studio
{
    public partial class SensorTest_Setting : Form
    {
        public SensorTest_Detail fmSenTest_Detail = null;
        public SensorTest_Multi fmSenTest_Multi = null;
        protected DynamicTool MyTool = new DynamicTool();

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

        private string[] strMainNode = null;

        private int[] m_Channel_List = new int[3];
        private Dictionary<string, delICFuntion> delTestFuntion;
        private SensorTest_RawDataSetting fmRawDataSetting = null;
        private bool iChangeFlag = false;
        private int iFolderRuleNumber = -1;
        private int iReportRuleNumber = -1;
        private int iUIDDisplayCount = -1;
        private CheckBox[][] m_cbReport = new CheckBox[2][];
        private List<LineSetting_Rectangle> m_lsRectangleLinebuf = new List<LineSetting_Rectangle>();
        private List<LineSetting_Cross> m_lsCrossLinebuf = new List<LineSetting_Cross>();
        private string m_PaintLineBuf = "";
        private Mutex mutex_PaintingLineValueChange = new Mutex();
        private string NowParentName = "";

        private List<KeyValuePair<string, _FlagArg_>> RawDataColor = new List<KeyValuePair<string, _FlagArg_>>();
        private List<string> TablePageNameBuff;
        List<MultiSenTest_VAR._SPEC_> publicSPEC = new List<MultiSenTest_VAR._SPEC_>();

        private string[][] str_ReportRule_2DArr = new string[2][];
        private string[][] str_ReportText_2DArr = new string[2][];
        private string strReportPath = "";
        private string strAutoGenPath = "";
        private stuPaintingItemGroup[] stuPaintingItemGp = new stuPaintingItemGroup[1];
        private UIPage MyUIPage = new UIPage();

        public bool m_ui_on_treeview = false;//用來知道treeView_MainSettings_AfterCheck是從哪裡被觸發。
        private bool IsSomeErrorSettings = false;
        private string ErrorSettingsReason = "";

        //private UIStyleManager StyleManager;
        private bool m_bKeepAdjustedData = false;

        public SensorTest_Setting()
        {
            InitializeComponent();

            tbSPEC_Settings.Appearance = TabAppearance.FlatButtons;
            tbSPEC_Settings.ItemSize = new Size(0, 1);
            tbSPEC_Settings.SizeMode = TabSizeMode.Fixed;

            BuildupTablePageBuff(ref TablePageNameBuff);
            //==============================================
            //絕對路徑或相對路徑選項
            //set auto gen path rule
            cb_autogen_path_rule.SelectedIndexChanged -= PathRule_SelectedIndexChanged;
            cb_autogen_path_rule.Items.Clear();
            cb_autogen_path_rule.Items.AddRange(new object[] {
                enReportPathRule.Absolute_Path.Desc(),
                enReportPathRule.Relative_Path.Desc()
            });
            cbAbsoluteRelativePath.Items.Clear();
            cbAbsoluteRelativePath.Items.AddRange(new object[] {
                enReportPathRule.Absolute_Path.Desc(),
                enReportPathRule.Relative_Path.Desc()
            });
            cb_autogen_path_rule.SelectedIndexChanged += PathRule_SelectedIndexChanged;

            strMainNode = new string[treeView_MainSettings.Nodes.Count];
            for (int inCount = 0; inCount < treeView_MainSettings.Nodes.Count; inCount++)
                strMainNode[inCount] = treeView_MainSettings.Nodes[inCount].Text;

            cbAbsoluteRelativePath.SelectedItem = enReportPathRule.Absolute_Path.Desc();

            //==============================================
            //資料夾規則
            int iReportIndex = (int)enReportGroup.ReportFolder;
            m_cbReport[iReportIndex] = new CheckBox[2];
            m_cbReport[iReportIndex][(int)enReportFolder.Time] = cbReportFile_1;
            m_cbReport[iReportIndex][(int)enReportFolder.Result] = cbReportFile_2;

            str_ReportText_2DArr[iReportIndex] = new string[2];
            str_ReportText_2DArr[iReportIndex][0] = @"Create Date Directory ( yyyy_mm_DD )";
            str_ReportText_2DArr[iReportIndex][1] = @"Create Result Directory ( OK/NG )";

            str_ReportRule_2DArr[iReportIndex] = new string[2];
            str_ReportRule_2DArr[iReportIndex][0] = @"{yyyy_mm_DD}";
            str_ReportRule_2DArr[iReportIndex][1] = @"{OK/NG}";

            for (int inCount = 0; inCount < str_ReportText_2DArr[iReportIndex].Length; inCount++)
                m_cbReport[iReportIndex][inCount].Text = str_ReportText_2DArr[iReportIndex][inCount];

            int iFileNameIndex = (int)enReportGroup.ReportFileName;
            m_cbReport[iFileNameIndex] = new CheckBox[4];
            m_cbReport[iFileNameIndex][(int)enReportFileName.Time] = cbReportTime;
            m_cbReport[iFileNameIndex][(int)enReportFileName.Result] = cbReportResult;
            m_cbReport[iFileNameIndex][(int)enReportFileName.Barcode] = cbReportBarcode;
            m_cbReport[iFileNameIndex][(int)enReportFileName.UID] = cbReportUID;

            str_ReportText_2DArr[iFileNameIndex] = new string[4];
            str_ReportText_2DArr[iFileNameIndex][0] = "File Name With Time (DateTime)";
            str_ReportText_2DArr[iFileNameIndex][1] = "File Name With Result (Result)";
            str_ReportText_2DArr[iFileNameIndex][2] = "File Name With Barcode (Barcode)";
            str_ReportText_2DArr[iFileNameIndex][3] = "File Name With Unique ID (UID)";

            str_ReportRule_2DArr[iFileNameIndex] = new string[4];
            str_ReportRule_2DArr[iFileNameIndex][0] = @"{Date:yyyyMMdd_HHmmss}";
            str_ReportRule_2DArr[iFileNameIndex][1] = @"{Result:OK_NG}";
            str_ReportRule_2DArr[iFileNameIndex][2] = @"{Barcode}";
            str_ReportRule_2DArr[iFileNameIndex][3] = @"{UID}";

            for (int inCount = 0; inCount < str_ReportText_2DArr[iFileNameIndex].Length; inCount++)
                m_cbReport[iFileNameIndex][inCount].Text = str_ReportText_2DArr[iFileNameIndex][inCount];

            //=============================================================            
            PaintingTreeViewReset(enPaintTestPattern.Paint_PaintRectangle);

            SubscribePage_TestFunc();
            FillTestitemDelgate();

            //=============================================================
            //set the screen number
            int cnt = GetScreenCnt;
            cb_monitor_num.Items.Clear();
            //while (cb_monitor_num.Items.Count != 0)
            //{
            //    cb_monitor_num.Items.RemoveAt(0);
            //    cb_short_monitor_num.Items.RemoveAt(0);
            //}


            for (int i = 0; i < cnt; i++)
            {
                cb_monitor_num.Items.Add(i + 1);
                cb_short_monitor_num.Items.Add(i + 1);
            }

            uiToolTip1.SetToolTip(lbTestFailStop, "單一測項Fail時就會停止。", "TestFailStop", 57366, 32, UIColor.Orange);
            uiToolTip1.SetToolTip(lbToolStart_AutoTest, "(只支援OP Mode) Tool開啟後自動執行測試。", "ToolStart_AutoTest", 57366, 32, UIColor.Orange);
            uiToolTip1.SetToolTip(lbToolStart_I2CDetect, "(只支援OP Mode)(只支援Bridge Board) Tool開啟後偵測I2C Bus是否資料。", "I2CDetect", 57366, 32, UIColor.Orange);
            uiToolTip1.SetToolTip(lbOPModeReadMPResult, "是否顯示Read MP Result按鈕。", "Show MP Result", 57366, 32, UIColor.Orange);
            uiToolTip1.SetToolTip(lbTestAllDone_FailStop, "全部測完後， 如果為Fail時，彈出Fail視窗。", "TestAllDone_FailStop", 57366, 32, UIColor.Orange);
            uiToolTip1.SetToolTip(lbTestAllDone_FailClose, "全部測完後，如果為Fail時，Tool自動關閉。", "TestAllDone_FailClose", 57366, 32, UIColor.Orange);
            uiToolTip1.SetToolTip(lbTestAllDone_PassClose, "全部測完後，如果為Pass時，Tool自動關閉。", "TestAllDone_PassClose", 57366, 32, UIColor.Orange);
            uiToolTip1.SetToolTip(lbTestFinishPwrDown, "全部測完後，Bridge斷電。", "TestFinishPwrDown", 57366, 32, UIColor.Orange);
            uiToolTip1.SetToolTip(lbBarcodeWaitForTest, "Barcode刷完後，會跳出對話視窗等待使用者按下確定。", "BarcodeWaitForTest", 57366, 32, UIColor.Orange);

            //20220705
            uiToolTip1.SetToolTip(lbShowTestDetailInfo, "是否顯示測試詳細訊息，包含TestItem / SPEC / Status...。", "Show Test Detail Info", 57366, 32, UIColor.Orange);
            uiToolTip1.SetToolTip(lbShowTestInfoViewer, "是否顯示Test List View。", "Show Test Info Viewer", 57366, 32, UIColor.Orange);
            //20221124
            uiToolTip1.SetToolTip(lbShowTestErrorCode, "是否顯示Test Error Code。", "Show Test Error Code", 57366, 32, UIColor.Orange);
            //20230309
            uiToolTip1.SetToolTip(lbShowBarcodeScanWaterMark, "是否顯示Bar Code Scan區域的Water Mark。", "Show Barcode Scan Water Mark", 57366, 32, UIColor.Orange);

            uiToolTip1.SetToolTip(lbShowICProductionInfo, "是否在Log內顯示IC Production。", "Show IC Production Info", 57366, 32, UIColor.Orange);
            uiToolTip1.SetToolTip(lbShowOPHint, "是否顯示提示放開手的視窗。", "Show Warning Box", 57366, 32, UIColor.Orange);

            txt_CustomID.KeyPress += new KeyPressEventHandler(txt_Judge);
            txt_FWID.KeyPress += new KeyPressEventHandler(txt_Judge);

            foreach (var item in EnumTool.EnumToList<enPatternType>())
            {
                cb_PatternType.Items.Add(item.ToString());
            }

        }

        private delegate void delICFuntion(TestItemNameDefine index);

        private enum _Ch_
        {
            X = 0,
            Y = 1,
            Key = 2
        };

        private enum enPaintingNodeName
        {
            [Description("Main")]
            Main = 0
        };

        private enum enReportFileName
        {
            Time = 0,
            Result = 1,
            Barcode = 2,
            UID = 3
        };

        private enum enReportFolder
        {
            Time = 0,
            Result = 1
        };

        private enum enReportGroup
        {
            ReportFolder = 0,
            ReportFileName = 1
        };

        private enum enReportPathRule
        {
            [Description("Absolute Path")]
            Absolute_Path = 0,

            [Description("Relative Path")]
            Relative_Path = 1,
        }

        public void Initial_treeView(object inSeletIndex)
        {
            this.Enabled = false;
            //tbSPEC_Settings.Visible = false;
            Initialize_TestFlow_treeView();
            treeView_MainSettings.ExpandAll();

            string strINIProfileVer = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("System", "ProfileVersion");
            //string DefineProtocolVer = string.Join(".", VersionInfo.ProfileVersion.Select(x => x.ToString()).ToArray());

            string strNodeKey = "Test Flow";
            int inNodeIndex = Array.FindIndex(strMainNode, s => s.Equals(strNodeKey));

            //if(!string.Equals(strbuf, DefineProtocolVer))
            if (strINIProfileVer != VersionInfo.ProfileVersion.ToString())
            {
                string str = "";
                if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    str = string.Format("Profile version is incompatible!\r\nTool is V{0}\r\nINI is V{1}\r\nPlease click Load Panelinfo!", VersionInfo.ProfileVersion, strINIProfileVer);
                else
                    str = string.Format("Profile版本不相符!\r\n軟體工具是 V{0}\r\n檔案是 V{1}\r\n請執行 Load Panel Info", VersionInfo.ProfileVersion, strINIProfileVer);
                MyUIPage.ShowWarningDialog(str);

                treeView_MainSettings.SelectedNode = treeView_MainSettings.Nodes[inNodeIndex].Nodes[(int)TestItemNameDefine.enPanel_Info];
            }
            else
            {
                var _SelectNode = treeView_MainSettings.Nodes[inNodeIndex].Nodes.Find(inSeletIndex.ToString(), false);
                treeView_MainSettings.SelectedNode = _SelectNode[0];
            }
            UpdateUI_FromMainSetting();
            //tbSPEC_Settings.Visible = true;
            this.Enabled = true;
        }

        private string ReturnDefaultValue(TestItemNameDefine _TestItemNameDefine, string strPattern, string strKey, string strDefault)
        {
            MultiSenTest_VAR._SPEC_ PaintingSpec = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[_TestItemNameDefine].SPEC.Find(x => x.Name.Contains(strPattern));
            if (string.IsNullOrEmpty(PaintingSpec.Name))
                return strDefault;
            return PaintingSpec.lsDrawingSpec.Find(x => x.Name.Contains(strKey)).strDefaultValue;
        }

        private void BuildupTablePageBuff(ref List<string> TablePageNameBuff)
        {
            TablePageNameBuff = new List<string>();
            foreach (TabPage tb in tbSPEC_Settings.TabPages)
                TablePageNameBuff.Add(tb.Text);
        }

        private void PaintingTreeViewReset(enPaintTestPattern Type)
        {
            PaintingTreeView.Visible = false;
            PaintingTreeView.Nodes.Clear();
            //增加父節點
            PaintingTreeView.Nodes.Add(enPaintingNodeName.Main.Desc());
            int idx = 0;
            //增加子節點
            switch (Type)
            {
                case enPaintTestPattern.Paint_PaintRectangle:
                case enPaintTestPattern.Paint_PenRectangle:
                    stuPaintingItemGp = new stuPaintingItemGroup[Enum.GetValues(typeof(enRectangle)).Length];

                    foreach (var varValue in EnumTool.EnumToList<enRectangle>())
                    {
                        stuPaintingItemGp[idx].ItemTreeNode = new TreeNode();
                        stuPaintingItemGp[idx].ItemTreeNode.Checked = true;
                        //stuPaintingItemGp[idx].ItemTreeNode.NodeFont = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                        stuPaintingItemGp[idx].ItemTreeNode.Text = varValue.Desc() + " ";
                        stuPaintingItemGp[idx].lsRectLineSetting = new List<LineSetting_Rectangle>();
                        PaintingTreeView.Nodes[(int)enPaintingNodeName.Main].Nodes.Add(stuPaintingItemGp[idx].ItemTreeNode);
                        idx++;
                    }
                    break;

                case enPaintTestPattern.Paint_PaintCross:
                case enPaintTestPattern.Paint_PenCross:
                    stuPaintingItemGp = new stuPaintingItemGroup[Enum.GetValues(typeof(enCross)).Length];
                    int iCnt = 1;
                    foreach (var varValue in EnumTool.EnumToList<enCross>())
                    {
                        stuPaintingItemGp[idx].ItemTreeNode = new TreeNode();
                        stuPaintingItemGp[idx].ItemTreeNode.Checked = true;
                        //stuPaintingItemGp[idx].ItemTreeNode.NodeFont = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                        stuPaintingItemGp[idx].ItemTreeNode.Text = string.Format("Line_{0}", iCnt++);
                        stuPaintingItemGp[idx].lsCrossLineSetting = new List<LineSetting_Cross>();
                        PaintingTreeView.Nodes[(int)enPaintingNodeName.Main].Nodes.Add(stuPaintingItemGp[idx].ItemTreeNode);
                        idx++;
                    }
                    break;
            }
            PaintingTreeView.ExpandAll();
            PaintingTreeView.Visible = true;
        }

        public bool bReadValueSetting(enPaintTestPattern NowPattern)
        {
            string strbuf = "";
            string INI_SectorName = NowPattern.ToString();
            if (UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult.Count == 0)
                return false;
          
            MultiSenTest_VAR._SPEC_ PaintingSpec = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enPaintingTest].SPEC.Find(x => x.Name.Contains(INI_SectorName));
            if (string.IsNullOrEmpty(PaintingSpec.Name))
                return false;

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Set_PanelInfo_Var(enReadFrom.INI);

            int inum = 0;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern = NowPattern;
            var _StrArr = new string[0];
            switch (NowPattern)
            {
                case enPaintTestPattern.Paint_FreeDrawing:
                case enPaintTestPattern.Paint_PenFreeDrawing:
                    _StrArr = INI_Name_FreeDrawing.TimeOut.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inTimeOut = inum;

                    _StrArr = INI_Name_FreeDrawing.Background.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Background = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_FreeDrawing.Rectangle.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Rectangle = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_FreeDrawing.ClickResultButton.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.bClickResultButton = Convert.ToBoolean(strbuf);
                    break;

                case enPaintTestPattern.Paint_PaintCross:
                case enPaintTestPattern.Paint_PenCross:
                    _StrArr = INI_Name_PaintCross.TimeOut.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inTimeOut = inum;

                    _StrArr = INI_Name_PaintCross.Retry.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inRetry = inum;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inRetryUplimit = inum;

                    _StrArr = INI_Name_PaintCross.Background.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Background = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintCross.StartEllipse.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintCross.EndEllipse.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintCross.Direction.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Direction = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintCross.PassEllipse.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Pass = Color.FromArgb((Int32)TransformColor(strbuf));

                    strbuf = m_PaintLineBuf;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.lsCrossLineSetting.Clear();
                    if (!string.IsNullOrEmpty(strbuf))
                    {
                        List<string> lsbuff = new List<string>();
                        lsbuff = strbuf.Split(';').ToList();

                        foreach (var item in lsbuff)
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.lsCrossLineSetting.Add(new LineSetting_Cross(item.Split(',').Cast<object>().ToList()));
                    }
                    break;

                case enPaintTestPattern.Paint_PaintRectangle:
                case enPaintTestPattern.Paint_PenRectangle:
                    _StrArr = INI_Name_PaintRectangle.TimeOut.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inTimeOut = inum;

                    _StrArr = INI_Name_PaintRectangle.FingerNumber.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    StaticVar.iRectFingerNum = inum;

                    _StrArr = INI_Name_PaintRectangle.LineGap.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    StaticVar.iLineGap = inum;

                    _StrArr = INI_Name_PaintRectangle.Retry.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inRetry = inum;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inRetryUplimit = inum;

                    _StrArr = INI_Name_PaintRectangle.Background.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Background = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintRectangle.Rectangle.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Rectangle = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintRectangle.StartEllipse.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintRectangle.EndEllipse.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintRectangle.Direction.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Direction = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintRectangle.PassEllipse.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Pass = Color.FromArgb((Int32)TransformColor(strbuf));
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.lsRectangleLineSetting.Clear();
                    strbuf = m_PaintLineBuf;
                    if (!string.IsNullOrEmpty(strbuf))
                    {
                        List<string> lsbuff = strbuf.Split(';').ToList();
                        foreach (var item in lsbuff)
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.lsRectangleLineSetting.Add(new LineSetting_Rectangle(item.Split(',').Cast<object>().ToList()));
                    }
                    break;
                case enPaintTestPattern.Paint_PaintRectangle3:
                case enPaintTestPattern.Paint_PenRectangle3:
                    _StrArr = INI_Name_PaintRectangle.TimeOut.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inTimeOut = inum;

                    _StrArr = INI_Name_PaintRectangle.LineGap.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    StaticVar.iLineGap = inum;

                    _StrArr = INI_Name_PaintRectangle.FingerNumber.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    StaticVar.iRect3FingerNum = inum;

                    _StrArr = INI_Name_PaintRectangle.Retry.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inRetry = inum;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inRetryUplimit = inum;

                    _StrArr = INI_Name_PaintRectangle.Background.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Background = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintRectangle.Rectangle.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Rectangle = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintRectangle.StartEllipse.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintRectangle.EndEllipse.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintRectangle.Direction.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Direction = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintRectangle.RangeWidth.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    double rangeWidth = Convert.ToDouble(strbuf);
                    // UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Direction = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintRectangle.PassEllipse.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Pass = Color.FromArgb((Int32)TransformColor(strbuf));
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.lsRectangle3LineSetting.Clear();
                    strbuf = m_PaintLineBuf;
                    if (!string.IsNullOrEmpty(strbuf))
                    {
                        INI_Tool Profile_INI = new INI_Tool(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                        List<string> lsbuff = new List<string>();

                        string strKey = "Panel_Info";
                        int xChannel = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.XChannel.ToString()));
                        int yChannel = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.YChannel.ToString()));

                        double xRes = (double)Convert.ToDouble(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.X_resolution.ToString()));
                        double yRes = (double)Convert.ToDouble(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.Y_resolution.ToString()));

                        double dXTemp = Math.Round((xRes / (double)xChannel) * rangeWidth, 0);
                        double dYTemp = Math.Round((yRes / (double)yChannel) * rangeWidth, 0);

                        int iMinChNum = Math.Min(xChannel, yChannel);
                        if (StaticVar.iRect3FingerNum * rangeWidth > iMinChNum)
                        {
                            MyUIPage.ShowErrorDialog(Application.ProductName, "Setting of Range Width Exceeds the Number of Channels.", UIStyle.Red, true);
                            return false;
                        }

                        //double dXTemp = Math.Round(xRes * (rangeWidth / 100), 0);
                        //double dYTemp = Math.Round(yRes * (rangeWidth / 100), 0);

                        ////ID , StartX , StartY , Width , Height
                        lsbuff.Add(String.Format("1,0,0,{0},{1},{2},{3},LeftToRight", xRes, dYTemp, dXTemp, dYTemp));
                        lsbuff.Add(String.Format("2,{0},0,{1},{2},{3},{4},TopToBottom,", (xRes - dXTemp), dXTemp, yRes, dXTemp, dYTemp));
                        lsbuff.Add(String.Format("3,0,{0},{1},{2},{3},{4},RightToLeft", (yRes - dYTemp), xRes, dYTemp, dXTemp, dYTemp));
                        lsbuff.Add(String.Format("4,0,0,{0},{1},{2},{3},BottomToTop", dXTemp, yRes, dXTemp, dYTemp));

                        //lsbuff.Add(String.Format("1,0,0,{0},{1},LeftToRight,0", xRes, dYTemp));
                        //lsbuff.Add(String.Format("2,{0},0,{1},{2},TopToBottom,1", (xRes - dXTemp), dXTemp, yRes));
                        //lsbuff.Add(String.Format("3,0,{0},{1},{2},RightToLeft,2", (yRes - dYTemp), xRes , dYTemp));
                        //lsbuff.Add(String.Format("4,0,0,{0},{1},BottomToTop,3", dXTemp, yRes));

                        string sLineSetting_Rect = string.Format("{0};{1};{2};{3}", lsbuff[0], lsbuff[1], lsbuff[2], lsbuff[3]);

                        Profile_INI.IniWriteValue("Paint_PaintRectangle3", "LineSetting_Rect3", sLineSetting_Rect);
                        Profile_INI.IniWriteValue("Paint_PenRectangle3", "LineSetting_Rect3", sLineSetting_Rect);

                        foreach (var item in lsbuff)
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.lsRectangle3LineSetting.Add(new LineSetting_Rectangle3(item.Split(',').Cast<object>().ToList()));
                    }

                    break;

                case enPaintTestPattern.Paint_PaintCross3:
                case enPaintTestPattern.Paint_PenCross3:
                    _StrArr = INI_Name_PaintCross.TimeOut.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inTimeOut = inum;

                    _StrArr = INI_Name_PaintCross.Retry.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inRetry = inum;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inRetryUplimit = inum;

                    _StrArr = INI_Name_PaintCross.Background.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Background = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintCross.StartEllipse.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_StartEllipse = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintCross.EndEllipse.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_EndEllipse = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintCross.Direction.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Direction = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintCross.PassEllipse.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Pass = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintCross.RangeWidth.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    rangeWidth = Convert.ToDouble(strbuf);

                    strbuf = m_PaintLineBuf;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.lsCrossLineSetting.Clear();
                    if (!string.IsNullOrEmpty(strbuf))
                    {
                        INI_Tool Profile_INI = new INI_Tool(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);

                        List<string> lsbuff = new List<string>();
                        //lsbuff = strbuf.Split(';').ToList();

                        string strKey = "Panel_Info";
                        int xChannel = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.XChannel.ToString()));
                        int yChannel = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.YChannel.ToString()));

                        double xRes = (double)Convert.ToDouble(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.X_resolution.ToString()));
                        double yRes = (double)Convert.ToDouble(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.Y_resolution.ToString()));

                        //double dXTemp = Math.Round((xRes / (double)xChannel) * rangeWidth, 0);
                        //double dYTemp = Math.Round((yRes / (double)yChannel) * rangeWidth, 0);


                        Point _tmpPoint = new Point(0, (int)yRes);

                        decimal dMaxValue = (decimal)DistanceForPointToABLine(_tmpPoint, new Point(0, 0), new Point((int)xRes, (int)yRes)); ;
                        double dValue = Math.Round(((double)dMaxValue / 100d) * rangeWidth, 0);

                        lsbuff.Add(String.Format("Line_1,0,0,{0},{1},{2},LT_RB", xRes, yRes, dValue));
                        lsbuff.Add(String.Format("Line_2,{0},0,0,{1},{2},RT_LB", xRes, yRes, dValue));

                        string sLineSetting_Cross3 = string.Format("{0};{1}", lsbuff[0], lsbuff[1]);

                        Profile_INI.IniWriteValue("Paint_PenCross3", "LineSetting_Cross3", sLineSetting_Cross3);
                        Profile_INI.IniWriteValue("Paint_PaintCross3", "LineSetting_Cross3", sLineSetting_Cross3);


                        foreach (var item in lsbuff)
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.lsCrossLineSetting.Add(new LineSetting_Cross(item.Split(',').Cast<object>().ToList()));
                    }
                    break;

                //Key Test
                case enPaintTestPattern.Paint_PaintKey:
                    _StrArr = INI_Name_PaintKey.TimeOut.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inTimeOut = inum;

                    _StrArr = INI_Name_PaintKey.Key_Scale.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.Key_Scale = inum;


                    _StrArr = INI_Name_PaintKey.Background.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Background = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintKey.TestKey.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_TestKey = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintKey.PassKey.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_PassKey = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_PaintKey.TouchKey.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_TouchKey = Color.FromArgb((Int32)TransformColor(strbuf));

                    break;

                case enPaintTestPattern.Paint_FingerDetection:
                    _StrArr = INI_Name_FingerDetection.TimeOut.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inTimeOut = inum;

                    _StrArr = INI_Name_FingerDetection.Background.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Background = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_FingerDetection.Rectangle.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Rectangle = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_FingerDetection.FingerNum.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    StaticVar.iDetect_FingerNum = inum;

                    _StrArr = EnumTool.GetEnumDescription(INI_Name_FingerDetection.CountDown).Split('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inCountDown = inum;
                    break;

                case enPaintTestPattern.Paint_PaintRectangle2:
                case enPaintTestPattern.Paint_PenRectangle2:
                    _StrArr = INI_Name_TrajectoryRectangle.Retry.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inRetry = inum;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inRetryUplimit = inum;

                    _StrArr = INI_Name_TrajectoryRectangle.TimeOut.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inTimeOut = inum;

                    _StrArr = INI_Name_TrajectoryRectangle.DetectOverRange.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryRectangle.bDetectOverRange = bool.Parse(strbuf);

                    _StrArr = INI_Name_TrajectoryRectangle.DetectRandomPoint.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryRectangle.bDetectRandomPoint = bool.Parse(strbuf);

                    _StrArr = INI_Name_TrajectoryRectangle.RangeWidth.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]).Replace("%", "");
                    double.TryParse(strbuf, out UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryRectangle.RangeWidth);
                    //UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryRectangle.iRangeWidth = inum;

                    _StrArr = INI_Name_TrajectoryRectangle.Background.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Background = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_TrajectoryRectangle.Rectangle.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Rectangle = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_TrajectoryRectangle.PassColor.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Pass = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_TrajectoryRectangle.FailColor.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Fail = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_TrajectoryRectangle.CornerReliability.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryRectangle.CornerReliability = EnumTool.ParseEnum<TestReliability>(strbuf);
                    break;

                case enPaintTestPattern.Paint_PaintCross2:
                case enPaintTestPattern.Paint_PenCross2:
                    _StrArr = INI_Name_TrajectoryCross.Retry.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inRetry = inum;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inRetryUplimit = inum;

                    _StrArr = INI_Name_TrajectoryCross.TimeOut.DescArr('=').Select(x => x.Trim()).ToArray();
                    int.TryParse(ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]), out inum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.inTimeOut = inum;

                    _StrArr = INI_Name_TrajectoryCross.DetectOverRange.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryCross.bDetectOverRange = bool.Parse(strbuf);

                    _StrArr = INI_Name_TrajectoryCross.DetectRandomPoint.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryCross.bDetectRandomPoint = bool.Parse(strbuf);

                    _StrArr = INI_Name_TrajectoryCross.RangeWidth.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]).Replace("%", "");
                    double.TryParse(strbuf, out UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryCross.RangeWidth);
                    //UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._TrajectoryCross.RangeWidth = inum;

                    _StrArr = INI_Name_TrajectoryRectangle.Background.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Background = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_TrajectoryRectangle.Rectangle.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Rectangle = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_TrajectoryRectangle.PassColor.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Pass = Color.FromArgb((Int32)TransformColor(strbuf));

                    _StrArr = INI_Name_TrajectoryRectangle.FailColor.DescArr('=').Select(x => x.Trim()).ToArray();
                    strbuf = ReturnDefaultValue(TestItemNameDefine.enPaintingTest, INI_SectorName, _StrArr[0], _StrArr[1]);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var._ComColor.Color_Fail = Color.FromArgb((Int32)TransformColor(strbuf));
                    break;
            }

            return true;
        }

        private void btn_CaptureFW_CRC_Click(object sender, EventArgs e)
        {
            btn_CaptureFW_CRC.Enabled = false;
            string NowProtocol = "";
            string Now_ICType = "";
            string DevProtocolVersion = "";
            richTextBox_FW_Info.Text = "";
            string ErrMsg = "";
            //===============================================
            // 重新確認裝置
            if (!UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ReloadNowDeviceStatus(out NowProtocol, out DevProtocolVersion, out Now_ICType, ref ErrMsg))
            {
                ErrMsg += "\nReload DeviceStatus Fail!\n";
                richTextBox_FW_Info.AppendText(ErrMsg);
                btn_CaptureFW_CRC.Enabled = true;
                return;
            }

            richTextBox_FW_Info.AppendText(string.Format("Now MCU : {0}\n",
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU));

            //===============================================
            // 判斷是否AP Mode
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode == FW_MODE.BL_MODE)
            {
                richTextBox_FW_Info.SelectionColor = Color.Red;
                richTextBox_FW_Info.SelectionFont = new Font("Consolas", 12.0F, FontStyle.Bold);
                richTextBox_FW_Info.AppendText("Now Device FW_Mode is BL mode!\n");
                richTextBox_FW_Info.SelectionFont = new Font("Consolas", 10.0F, FontStyle.Bold);
                richTextBox_FW_Info.SelectionColor = Color.Black;
                btn_CaptureFW_CRC.Enabled = true;
                return;
            }
            richTextBox_FW_Info.AppendText(string.Format("Now Device FW_Mode : {0}\n", UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode));

            //===============================================
            // 檢查Protocol Version是否符合Profile設定
            string str = "";
            string strProfileProtocol = "";
            string Profile_ICType = "";

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.bReadProtocol(
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath,
                out strProfileProtocol, out Profile_ICType);
            if (NowProtocol != strProfileProtocol)
            {
                str = string.Format("Protocol not match!\n(INI : {0}, Device : {1})\nPlease load PanelInfo first!\n", strProfileProtocol, NowProtocol);
                //MessageBox.Show(str, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                richTextBox_FW_Info.SelectionColor = Color.Red;
                richTextBox_FW_Info.SelectionFont = new Font("Consolas", 12.0F, FontStyle.Bold);
                richTextBox_FW_Info.AppendText(str);
                richTextBox_FW_Info.SelectionFont = new Font("Consolas", 10.0F, FontStyle.Bold);
                richTextBox_FW_Info.SelectionColor = Color.Black;
                btn_CaptureFW_CRC.Enabled = true;
                return;
            }

            if (!string.Equals(Now_ICType, Profile_ICType))
            {
                str = string.Format("IC not match!(INI = {0}, Device = {1})\nPlease load Panel_Info again.\n", Profile_ICType, Now_ICType);
                //MessageBox.Show(str, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyUIPage.ShowErrorDialog(Application.ProductName, str, UIStyle.Red, true);
                richTextBox_FW_Info.SelectionColor = Color.Red;
                richTextBox_FW_Info.SelectionFont = new Font("Consolas", 12.0F, FontStyle.Bold);
                richTextBox_FW_Info.AppendText(str);
                richTextBox_FW_Info.SelectionFont = new Font("Consolas", 10.0F, FontStyle.Bold);
                richTextBox_FW_Info.SelectionColor = Color.Black;
                btn_CaptureFW_CRC.Enabled = true;
                return;
            }

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Update_FW_Info();

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6 &&
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum < Ver.AP_603)
            {
                MyUIPage.ShowErrorDialog(Application.ProductName,
                    string.Format("Protocol : {0} not support this function.", UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum),
                    UIStyle.Red, true);
                richTextBox_FW_Info.Enabled = false;
                richTextBox_FW_Info.SelectionColor = Color.Red;
                richTextBox_FW_Info.SelectionFont = new Font("Consolas", 12.0F, FontStyle.Bold);
                richTextBox_FW_Info.AppendText(string.Format("Protocol : {0} not support this function.\n",
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum));

                richTextBox_FW_Info.SelectionColor = Color.BlueViolet;
                richTextBox_FW_Info.SelectionFont = new Font("Consolas", 12.0F, FontStyle.Bold);
                richTextBox_FW_Info.AppendText("Please load HEX to get Verify code.\n");

                richTextBox_FW_Info.Enabled = true;
                btn_CaptureFW_CRC.Enabled = true;
                timer_blink.Enabled = true;
            }
            else
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                    richTextBox_FW_Info.AppendText(string.Format("Block Number : {0}\n", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.BlockNumber));

                //檢查是否有載入hex的資料，有的話作比對動作，沒的話正常顯示
                bool HexExist = (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.LocalVariable.VarUpgrade_Multi.CodeBlock.Count != 0);
                string _str = "", _Title = "";
                if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                {
                    _Title = "Message";
                    _str = global::ITS_Studio.Properties.Resources.CRCDiff_EN;
                }
                else
                {
                    _Title = "訊息";
                    _str = global::ITS_Studio.Properties.Resources.CRCDiff_CN;
                }
                if (CompareICandHex_CRC(HexExist, false, Now_ICType))
                    MyUIPage.ShowAskDialog(_Title, _str, UIStyle.Red, true);
            }
            //===============================================
            //更新UI畫面
            var mySelect = EnumTool.GetValueFromDescription<TestItemNameDefine>(treeView_MainSettings.SelectedNode.Text);
            ShowSpecOnUI(mySelect);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
            btn_CaptureFW_CRC.Enabled = true;
            lbNowIC.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("IC_Verify", "IC_Type");
        }

        private DialogResult OpenHex(out string PathName)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Hex files (*.hex)|*.hex";
            string AbsolutePath = "";
            if (!string.IsNullOrEmpty(FWupgradePath.Text))
            {
                if (Path.IsPathRooted(FWupgradePath.Text))
                {
                    string _str = "", _Title = "";
                    if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    {
                        _Title = "Message";
                        _str = global::ITS_Studio.Properties.Resources.InvalidFileFormat_EN;
                    }
                    else
                    {
                        _Title = "訊息";
                        _str = global::ITS_Studio.Properties.Resources.InvalidFileFormat_CN;
                    }
                    MyUIPage.ShowErrorDialog(_Title, string.Format(_str, FWupgradePath.Text));
                    AbsolutePath = ITS_Directory.Tool_CurrentDirectory;
                }
                else
                {
                    AbsolutePath = Path.GetDirectoryName(string.Format(@"{0}{1}", ITS_Directory.Tool_CurrentDirectory, FWupgradePath.Text));
                }
            }
            openDlg.InitialDirectory = (Directory.Exists(AbsolutePath)) ? AbsolutePath : ITS_Directory.Tool_CurrentDirectory;
            openDlg.FileName = "";
            openDlg.CheckFileExists = true;    //如果當使用者指定不存在的檔名時對話方塊顯示警告訊息，屬性值為 true
            openDlg.CheckPathExists = true;    //如果當使用者指定了不存在的路徑時要顯示警告對話方塊，屬性值為 true
            openDlg.Multiselect = false;       //如果對話方塊允許一起或同時選取多個檔案，則為 true
            DialogResult opResult = openDlg.ShowDialog(this);
            PathName = openDlg.FileName;
            return opResult;
        }

        private void btnFWPath_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            bool bErrFlag = false;
           
            if (btn == btnFWPath)
                richTextBox_FW_Info.Clear();
            else
                richTextBox_FWFixed_Info.Clear();

            timer_blink.Enabled = false;
            string strFilePath = "";
            DialogResult opResult = OpenHex(out strFilePath);
            if (opResult != DialogResult.OK)
                return;

            string _waitStr = "";

            if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                _waitStr = global::ITS_Studio.Properties.Resources.ProcessingTip_En;
            else
                _waitStr = global::ITS_Studio.Properties.Resources.ProcessingTip_Cn;
            MyUIPage.ShowWaitForm(_waitStr);
            tableLayoutPanel1.Enabled = false;

            if (btn == btnFWPath)
                FWupgradePath.Text = strFilePath;
            else
                FWFixedPath.Text = strFilePath;
            //從INI取得MCU資訊
            string strMCU = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("IC_Verify", "IC_Type");

            //解析HEX檔案
            string strErrorMSG = "";
            bool bIsOffline = true;
            if (StaticVar._ITSTool_Var == enMode.RDMode)
                bIsOffline &= StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus != DevStatus.Connected;
            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_.DecodeFile_API(strMCU, strFilePath, ref strErrorMSG, bIsOffline, (btn == btnFWFixedPath)) < 0)
            {
                bErrFlag = true;
                goto GOTO_File_Error;
            }
            //========================================================            
            var Hex_MCU = (btn == btnFWFixedPath) ? UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.HEX_Panel_Info_For_FWFixed.Hex_MCU
                : UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.HEX_Panel_Info.Hex_MCU;
            if (Hex_MCU != strMCU)
            {
                strErrorMSG = "MCU Version does not match!\r\n";
                bErrFlag = true;
                goto GOTO_File_Error;
            }
            //========================================================
            //依據裝置有無，決定CRC要如何取得            
            var DevInfo = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo;
            bool bDeviceAlive = (DevInfo != null && DevInfo.VerifyCode != null) ? true : false;
            if (bDeviceAlive)
            {
                string NowProtocol = "";
                string NowICType = "";
                string nowProtocolVersion = "";
                strErrorMSG = "";
                if (!UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ReloadNowDeviceStatus(out NowProtocol, out nowProtocolVersion, out NowICType, ref strErrorMSG))
                {
                    bErrFlag = true;
                    goto GOTO_File_Error;
                }
            }

            bool show_warning_meg = CompareICandHex_CRC(bDeviceAlive, true, Hex_MCU, btn != btnFWPath); //這邊str_mcu是用來判斷V3使用的 不是V3就會是V6，故不用修改
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.UpdateHexInfo(strFilePath, (btn == btnFWPath));

            MyUIPage.HideWaitForm();
            tableLayoutPanel1.Enabled = true;

            /* V3 no DF firmware notification */
            if (btn == btnFWPath &&
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3 &&
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.CodeBlock[1].Block_CRC16 == 0)
                MyUIPage.ShowWarningDialog("251x/231x without Data Flash firmware was selected,\r\nPLEASE double check CRC/ FW_Ver. for \"Check FW Only\"\r\n");

            var mySelect = EnumTool.GetValueFromDescription<TestItemNameDefine>(treeView_MainSettings.SelectedNode.Text);
            ShowSpecOnUI(mySelect);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
            string _str = "", _Title = "";
            if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
            {
                _Title = "Message";
                _str = global::ITS_Studio.Properties.Resources.CRCDiff_EN;
            }
            else
            {
                _Title = "訊息";
                _str = global::ITS_Studio.Properties.Resources.CRCDiff_CN;
            }
            if (show_warning_meg)
                MyUIPage.ShowInfoDialog(_Title, _str, UIStyle.Orange, false);

        GOTO_File_Error:

            MyUIPage.HideWaitForm();
            tableLayoutPanel1.Enabled = true;

            if (bErrFlag)
            {
                MyUIPage.ShowInfoDialog("", strErrorMSG, UIStyle.Orange, false);
                if (btn == btnFWPath)
                    FWupgradePath.Text = "";
                else
                    FWFixedPath.Text = "";
                MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enUpgrade];
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, "Path", "");
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, "Master_CRC", "");
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, "Slave_CRC", "");
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, "FW_Ver", "");
            }

            //MessageBox.Show("CRC are differts between the device and the hex file", "Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void btnReportPath_Click(object sender, EventArgs e)
        {
            FolderSelectDialog fbd = new FolderSelectDialog();
            fbd.DirectoryPath = ITS_Directory.Tool_CurrentDirectory;
            string strPath = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Report", "Path");
            if (!string.IsNullOrEmpty(strPath))
            {
                string tmp = strPath;
                if (!Path.IsPathRooted(strPath))
                    tmp = Path.GetFullPath(strPath);
                strPath = tmp;
                fbd.DirectoryPath = strPath;
            }

            if (fbd.ShowDialog(this) == DialogResult.Cancel)
                return;

            var tmpItem = EnumTool.GetValueFromDescription<enReportPathRule>(cbAbsoluteRelativePath.SelectedItem.ToString());
            if (tmpItem == enReportPathRule.Relative_Path)
                strReportPath = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, fbd.DirectoryPath);
            else
                strReportPath = fbd.DirectoryPath;
            tbReportPath.Text = strReportPath;
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("Report", "Path", strReportPath);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
        }

        private void cb_monitor_num_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _MySender = sender as ComboBox;
            if (_MySender.Name == "cb_monitor_num")
            {
                cb_short_monitor_num.SelectedIndexChanged -= cb_monitor_num_SelectedIndexChanged;
                cb_short_monitor_num.SelectedIndex = _MySender.SelectedIndex;
                cb_short_monitor_num.SelectedIndexChanged += cb_monitor_num_SelectedIndexChanged;
            }
            else
            {
                cb_monitor_num.SelectedIndexChanged -= cb_monitor_num_SelectedIndexChanged;
                cb_monitor_num.SelectedIndex = _MySender.SelectedIndex;
                cb_monitor_num.SelectedIndexChanged += cb_monitor_num_SelectedIndexChanged;
            }

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.iScreenNum = Convert.ToUInt16(_MySender.Text, 16);
        }

        private void cb_SensorTestStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strIndexName = (((UIComboBox)sender)).Text;
            gb_Fixture_Tip.Enabled = false;
            textBox_Pickup.TextChanged -= textBox_TextChanged;
            textBox_Pickup.Text = "";
            textBox_Putdown.TextChanged -= textBox_TextChanged;
            textBox_Putdown.Text = "";
            textBox_BlinkTimer.TextChanged -= textBox_TextChanged;
            textBox_BlinkTimer.Text = "";
            textBox_ChangeConnect.TextChanged -= textBox_TextChanged;
            textBox_ChangeConnect.Text = "";

            foreach (var station in EnumTool.EnumToList<SensorTestStation>().ToList())
            {
                if (station.Desc().Equals(strIndexName))
                {
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", "SensorTestStation", cb_SensorTestStation.SelectedItem.ToString());
                    //
                    btn_autogen_thr_set.Enabled = uiSwitch_AutoGenEn.Active;
                    uitb_count.Enabled = uiSwitch_AutoGenEn.Active;
                    btn_collection_clr.Enabled = uiSwitch_AutoGenEn.Active;
                    cb_autogen_path_rule.Enabled = uiSwitch_AutoGenEn.Active;
                    chk_addDate.Enabled = uiSwitch_AutoGenEn.Active;
                    uitb_autogen_profilename.Enabled = uiSwitch_AutoGenEn.Active;
                    //
                    if (station == SensorTestStation.FPC)
                    {
                        tabPage_FPC.Parent = tabControl1;
                        tabPage_AutoGen.Parent = null;
                        //tabControl1.SelectedTab = tabPage_FPC;
                        gb_Fixture_Tip.Enabled = true;

                        textBox_Pickup.Text = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsFPC_Warning[0];
                        textBox_Pickup.TextChanged += textBox_TextChanged;

                        textBox_Putdown.Text = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsFPC_Warning[1];
                        textBox_Putdown.TextChanged += textBox_TextChanged;

                        textBox_ChangeConnect.Text = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsFPC_Warning[2];
                        textBox_ChangeConnect.TextChanged += textBox_TextChanged;
                        textBox_BlinkTimer.Text = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.BlinkTimer.ToString();
                        textBox_BlinkTimer.TextChanged += textBox_TextChanged;
                    }
                    else
                    {
                        tabPage_AutoGen.Parent = tabControl1;
                        tabPage_FPC.Parent = null;
                        //tabControl1.SelectedTab = tabPage_AutoGen;
                    }

                    break;
                }
            }
        }

        private void Change_ReportPathRuleSelect(int iFolderRule, int iReportRule)
        {
            int iRuleGroup = (int)enReportGroup.ReportFolder;

            foreach (int item in Enum.GetValues(typeof(ReportFolderRule)))
                m_cbReport[iRuleGroup][item].Checked = MyTool.GetIntegerSomeBit(iFolderRule, item) == 1 ? true : false;

            iRuleGroup = (int)enReportGroup.ReportFileName;

            foreach (int item in Enum.GetValues(typeof(ReportFileRule)))
                m_cbReport[iRuleGroup][item].Checked = MyTool.GetIntegerSomeBit(iReportRule, item) == 1 ? true : false;
        }

        private void Change_ReportRule(int iFolderRule, int iReportRule)
        {
            string strbuf = @"..\";

            //==========================================
            // Folder
            int iTmpIndex = 0;
            int iRuleType = (int)enReportGroup.ReportFolder;
            foreach (int EveryRule in Enum.GetValues(typeof(ReportFolderRule)))
            {
                if (MyTool.GetIntegerSomeBit(iFolderRule, EveryRule) == 1)
                    strbuf += str_ReportRule_2DArr[iRuleType][iTmpIndex] + @"\";
                iTmpIndex++;
            }

            //==========================================
            // File Name
            string ReportFileFormat = "";
            iRuleType = (int)enReportGroup.ReportFileName;
            iTmpIndex = 0;
            string INI_FileFormat = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Report", "ReportFileFormat").Replace(".csv", "");
            ICollection<string> matches = Regex.Matches(INI_FileFormat.Replace(Environment.NewLine, ""), @"\{([^}]*)\}")
                .Cast<Match>().Select(x => x.Groups[1].Value).ToList();
            //
            int real_name_bit = 0;
            if (INI_FileFormat.IndexOf(ReportFileRule.enDate.ToString().Replace("en", "")) != -1)
                real_name_bit |= 1;
            if (INI_FileFormat.IndexOf(ReportFileRule.enResult.ToString().Replace("en", "")) != -1)
                real_name_bit |= 2;
            if (INI_FileFormat.IndexOf(ReportFileRule.enBarcode.ToString().Replace("en", "")) != -1)
                real_name_bit |= 4;
            if (INI_FileFormat.IndexOf(ReportFileRule.enUID.ToString().Replace("en", "")) != -1)
                real_name_bit |= 8;
            //
            
            bool bFirst = true;
            List<string> lsReportRule = new List<string>();
            foreach (var _Item in (ReportFileRule[])Enum.GetValues(typeof(ReportFileRule)))
            {
                if (MyTool.GetIntegerSomeBit(iReportRule, (int)_Item) != 1)
                    continue;
                lsReportRule.Add(_Item.ToString());
            }
            //如有size差異，就使用預設的方式填格式
            //if (matches.Count != lsReportRule.Count)
            if (real_name_bit != iReportRule)
            {
                foreach (string _EnRule in lsReportRule)
                {
                    string str = matches.FirstOrDefault(x => x.Contains(_EnRule.Replace("en", "")));
                    string FoundVar = "";
                    if (string.IsNullOrEmpty(str))
                        FoundVar = str_ReportRule_2DArr[iRuleType].FirstOrDefault(x => x.Contains(_EnRule.Replace("en", "")));
                    else
                        FoundVar = string.Format("{{{0}}}", str.Replace("en", ""));
                    ReportFileFormat += (bFirst) ? FoundVar : "_" + FoundVar;
                    bFirst = false;
                }
            }
            else
            {
                ReportFileFormat = INI_FileFormat;
            }

            txtBox_ReportRulePath.Text = string.Format("{0}.csv", ReportFileFormat);

            //==========================================
            bFirst = true;
            matches = Regex.Matches(ReportFileFormat.Replace(Environment.NewLine, ""), @"\{([^}]*)\}")
                .Cast<Match>().Select(x => x.Groups[1].Value).ToList();
            foreach (string item in matches)
            {
                strbuf += (bFirst) ? string.Format("{{{0}}}", item.Split(':')[0]) : string.Format("_{{{0}}}", item.Split(':')[0]);
                bFirst = false;
            }
            lbReportRulePath.Text = strbuf + ".csv";
        }

        private bool ChangePaintingTestSpecValue(string sectionName, string strKey, string strValue)
        {
            int index = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enPaintingTest].SPEC.FindIndex(x => x.Name == sectionName);
            if (index == -1)
                return false;
            MultiSenTest_VAR._SPEC_ lsSpec = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enPaintingTest].SPEC[index];

            int index_2 = lsSpec.lsDrawingSpec.FindIndex(x => x.Name == strKey);
            if (index_2 == -1)
                return false;
            MultiSenTest_VAR.LineTestSetting LineTestSettingitem = lsSpec.lsDrawingSpec[index_2];
            LineTestSettingitem.strDefaultValue = strValue;
            lsSpec.lsDrawingSpec[index_2] = LineTestSettingitem;
            return true;
        }

        private void UIchkbox_CheckedChanged(object sender, EventArgs e)
        {
            List<string> lsDescriptions = EnumTool.GetDescriptions<TestItemNameDefine>();
            int iSelect = lsDescriptions.FindIndex(x => x.Contains(tbSPEC_Settings.SelectedTab.Name));
            TestItemNameDefine TestitemName = (TestItemNameDefine)Enum.Parse(typeof(TestItemNameDefine), iSelect.ToString());

            var MySender = sender as UICheckBox;
            List<MultiSenTest_VAR._SPEC_> tmpSPEC = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestitemName].SPEC;
            MultiSenTest_VAR._SPEC_ item;
            string strKeyWord = "";
            switch (TestitemName)
            {
                case TestItemNameDefine.enPaintingTest:
                    StaticVar.bAutoRotationAngle = MySender.Checked;
                    break;

                case TestItemNameDefine.enGpio_Test:
                    strKeyWord = MySender.Name.Replace("cb_", "").Replace("chk_", "");
                    //item = tmpSPEC.Find(x => x.Name == strKeyWord);
                    var _index = tmpSPEC.FindIndex(x => x.Name == strKeyWord);
                    if (Enum.IsDefined(typeof(INI_Name_GPIOTest), strKeyWord))
                    {
                        //item.bTestEn = MySender.Checked;
                        item = tmpSPEC[_index];
                        item.bTestEn = MySender.Checked;
                        tmpSPEC[_index] = item;
                    }
                    else
                    {
                        MultiSenTest_VAR._SPEC_ item_ID_Pin_Test = tmpSPEC.Find(x => x.Name == INI_Name_GPIOTest.ID_Pin_Test.ToString());
                        uint iValue = 0;
                        var arr = groupBox_PinID.Controls.OfType<UICheckBox>().Where(c => c.Name.Contains("chk_ID_Pin"));
                        for (int iShift = 0; iShift < arr.Count(); iShift++)
                        {
                            var _ID_Pin_Checkbox = arr.FirstOrDefault(x => x.Name.Equals(string.Format("chk_ID_Pin{0}_Test", iShift + 1)));
                            iValue |= MyTool.SetIntegerSomeBit(iValue, iShift, _ID_Pin_Checkbox.Checked);
                        }
                        item_ID_Pin_Test.dtValue.Rows[0][0] = string.Format("0x{0:X2}", iValue);
                    }

                    break;

                case TestItemNameDefine.enPowerTest:
                    strKeyWord = MySender.Name.Replace("cb_", "").Replace("chk_", "");
                    //item = tmpSPEC.Find(x => x.Name == strKeyWord);
                    _index = tmpSPEC.FindIndex(x => x.Name == strKeyWord);
                    if (Enum.IsDefined(typeof(INI_Name_PowerTest), strKeyWord))
                    {
                        //item.bTestEn = MySender.Checked;
                        item = tmpSPEC[_index];
                        item.bTestEn = MySender.Checked;
                        tmpSPEC[_index] = item;
                    }
                    else
                    {
                        MultiSenTest_VAR._SPEC_ item_LVD_Flag_Test = tmpSPEC.Find(x => x.Name == INI_Name_PowerTest.VDD33_LVD_Flag_BitMask.ToString());
                        uint iValue = 0;
                        var arr = groupBox_LVDFlag.Controls.OfType<UICheckBox>().Where(c => c.Name.Contains("chk_LVDFlag_Pin"));
                        for (int iShift = 0; iShift < arr.Count(); iShift++)
                        {
                            var _ID_Pin_Checkbox = arr.FirstOrDefault(x => x.Name.Equals(string.Format("chk_LVDFlag_Pin{0}_Test", iShift)));
                            iValue |= MyTool.SetIntegerSomeBit(iValue, iShift, _ID_Pin_Checkbox.Checked);
                        }
                        item_LVD_Flag_Test.dtValue.Rows[0][0] = string.Format("0x{0:X2}", iValue);
                    }

                    break;
            }
        }

        private void chkbox_CheckedChanged(object sender, EventArgs e)
        {
            List<string> lsDescriptions = EnumTool.GetDescriptions<TestItemNameDefine>();
            int iSelect = lsDescriptions.FindIndex(x => x.Contains(tbSPEC_Settings.SelectedTab.Name));
            TestItemNameDefine TestitemName = (TestItemNameDefine)Enum.Parse(typeof(TestItemNameDefine), iSelect.ToString());

            var MySender = sender as CheckBox;
            List<MultiSenTest_VAR._SPEC_> tmpSPEC = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestitemName].SPEC;
            MultiSenTest_VAR._SPEC_ item;
            string strKeyWord = "";
            switch (TestitemName)
            {
                case TestItemNameDefine.enGpio_Test:
                    strKeyWord = MySender.Name.Replace("cb_", "").Replace("chk_", "");
                    var _index = tmpSPEC.FindIndex(x => x.Name == strKeyWord);

                    if (Enum.IsDefined(typeof(INI_Name_GPIOTest), strKeyWord))
                    {
                        item = tmpSPEC[_index];
                        item.bTestEn = MySender.Checked;
                        tmpSPEC[_index] = item;
                    }
                    else
                    {
                        MultiSenTest_VAR._SPEC_ item_ID_Pin_Test = tmpSPEC.Find(x => x.Name == INI_Name_GPIOTest.ID_Pin_Test.ToString());
                        uint iValue = 0;
                        var arr = groupBox_PinID.Controls.OfType<CheckBox>().Where(c => c.Name.Contains("chk_ID_Pin"));
                        for (int iShift = 0; iShift < arr.Count(); iShift++)
                        {
                            var _ID_Pin_Checkbox = arr.FirstOrDefault(x => x.Name.Equals(string.Format("chk_ID_Pin{0}_Test", iShift + 1)));
                            iValue |= MyTool.SetIntegerSomeBit(iValue, iShift, _ID_Pin_Checkbox.Checked);
                        }
                        item_ID_Pin_Test.dtValue.Rows[0][0] = string.Format("0x{0:X2}", iValue);
                    }
                    break;

                case TestItemNameDefine.enPowerTest:
                    strKeyWord = MySender.Name.Replace("cb_", "").Replace("chk_", "");
                    _index = tmpSPEC.FindIndex(x => x.Name == strKeyWord);

                    if (Enum.IsDefined(typeof(INI_Name_PowerTest), strKeyWord))
                    {
                        item = tmpSPEC[_index];
                        item.bTestEn = MySender.Checked;
                        tmpSPEC[_index] = item;
                    }
                    else
                    {
                        MultiSenTest_VAR._SPEC_ item_LVDFlag_Pin_Test = tmpSPEC.Find(x => x.Name == INI_Name_PowerTest.VDD33_LVD_Flag_BitMask.ToString());
                        uint iValue = 0;
                        var arr = groupBox_LVDFlag.Controls.OfType<CheckBox>().Where(c => c.Name.Contains("chk_LVDFlag_Pin"));
                        for (int iShift = 0; iShift < arr.Count(); iShift++)
                        {
                            var _ID_Pin_Checkbox = arr.FirstOrDefault(x => x.Name.Equals(string.Format("chk_LVDFlag_Pin{0}_Test", iShift)));
                            iValue |= MyTool.SetIntegerSomeBit(iValue, iShift, _ID_Pin_Checkbox.Checked);
                        }
                        item_LVDFlag_Pin_Test.dtValue.Rows[0][0] = string.Format("0x{0:X2}", iValue);
                    }
                    break;

                case TestItemNameDefine.enUpgrade:
                    strKeyWord = MySender.Name.Replace("cb_", "").Replace("chk_", "");
                    _index = tmpSPEC.FindIndex(x => x.Name == strKeyWord);
                    item = tmpSPEC[_index];
                    item.bTestEn = MySender.Checked;
                    tmpSPEC[_index] = item;

                    if ((strKeyWord == INI_Name_FW_Verify.FW_CheckOnly.ToString()) && MySender.Checked)
                    {
                        if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                        {
                            //if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum > Protocol_Ver.v602)
                            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum > Ver.AP_602)
                            {
                                FWupgradePath.Clear();
                                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.LocalVariable.VarUpgrade_Multi.CodeBlock.Clear(); //20220831 Steven
                                MultiSenTest_VAR._SPEC_ item_2 = tmpSPEC.Find(x => x.Name == INI_Name_FW_Verify.Path.ToString());
                                item_2.dtValue.Rows[0][0] = "";
                            }
                        }
                        else if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                        {
                            FWupgradePath.Clear();
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.LocalVariable.VarUpgrade_Multi.CodeBlock.Clear(); //20220831 Steven
                            MultiSenTest_VAR._SPEC_ item_2 = tmpSPEC.Find(x => x.Name == INI_Name_FW_Verify.Path.ToString());
                            item_2.dtValue.Rows[0][0] = "";
                        }
                    }

                    //ForceUpgrade 和 CheckFW_Only互斥
                    if ((strKeyWord == INI_Name_FW_Verify.Force_Upgrade.ToString()) && (chk_FW_CheckOnly.Checked))
                    {
                        chk_FW_CheckOnly.CheckedChanged -= chkbox_CheckedChanged;
                        chk_FW_CheckOnly.Checked = false;
                        chk_FW_CheckOnly.CheckedChanged += chkbox_CheckedChanged;
                        _index = tmpSPEC.FindIndex(x => x.Name == INI_Name_FW_Verify.FW_CheckOnly.ToString());
                        item = tmpSPEC[_index];
                        item.bTestEn = false;
                        tmpSPEC[_index] = item;
                    }
                    else if ((strKeyWord == INI_Name_FW_Verify.FW_CheckOnly.ToString()) && (chk_Force_Upgrade.Checked))
                    {
                        chk_Force_Upgrade.CheckedChanged -= chkbox_CheckedChanged;
                        chk_Force_Upgrade.Checked = false;
                        chk_Force_Upgrade.CheckedChanged += chkbox_CheckedChanged;
                        _index = tmpSPEC.FindIndex(x => x.Name == INI_Name_FW_Verify.Force_Upgrade.ToString());
                        item = tmpSPEC[_index];
                        item.bTestEn = false;
                        tmpSPEC[_index] = item;
                    }
                    break;
            }
        }

        private void cmbTrigerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strIndexName = (((UIComboBox)sender)).Text;
            groupBox_Barcode.Enabled = false;
            cbReportBarcode.Checked = false;
            cbReportBarcode.Enabled = false;

            groupBox_GPIO.Enabled = false;

            foreach (var TrigItem in EnumTool.EnumToList<TriggerMode>())
            {
                if (TrigItem.Desc().Equals(strIndexName))
                {
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", "TriggerOption", cmbTrigerMode.SelectedItem.ToString());
                    if ((TrigItem == TriggerMode.BarCode) || (TrigItem == TriggerMode.BarCodeButton))
                    {
                        groupBox_Barcode.Enabled = true;
                        cbReportBarcode.Checked = true;
                        cbReportBarcode.Enabled = true;
                    }
                    else if (TrigItem == TriggerMode.GPIO)
                    {
                        groupBox_GPIO.Enabled = true;
                    }
                    break;
                }
            }
        }

        private bool CompareICandHex_CRC(bool bIsVerifyCodeExist, bool is_fromHex, string mcu, bool isFWFixed = false)
        {
            RichTextBox MySender = isFWFixed ? richTextBox_FWFixed_Info : richTextBox_FW_Info;
            var MyBlock = isFWFixed ? UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.LocalVariable.VarUpgrade_Multi.CodeBlock_FW_Fixed
                : UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.LocalVariable.VarUpgrade_Multi.CodeBlock;

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3 &&
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum <= new Version(3, 0, 12))
                MyUIPage.ShowInfoDialog(Application.ProductName,
                    string.Format("This protocol : ({0}) not support tuning checksum.",
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum), UIStyle.Orange, false);

            bool show_warning_meg = false;
            int iBlocks = 0;
            string ErrStr = "";
            if (EnumTool.IsEnumDefined<OldV3_MCU>(mcu))
            {
                if (bIsVerifyCodeExist)
                {
                    for (int i = 0; i < UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode.Count; i++)
                    {
                        int _DevInfoVerifyCode = MyTool.HexStringToInt(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode[i], HEX_TYPE.BIG_ENDIAN, out ErrStr);
                        MySender.SelectionColor = (MyBlock[i].Block_Checksum != _DevInfoVerifyCode) ? Color.Gray : Color.Green;
                        if (MyBlock[i].Block_Checksum != _DevInfoVerifyCode)
                            show_warning_meg = true;
                        MySender.AppendText(string.Format("Hex Block_{0} Checksum : 0x{1:X4}\n", i, MyBlock[i].Block_Checksum));
                        MySender.SelectionColor = Color.Black;
                        MySender.AppendText(string.Format("Device Block_{0} Checksum : 0x{1:X4}\n", i, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode[i]));
                    }
                }
                else
                {
                    MySender.SelectionColor = Color.Black;
                    if (is_fromHex)
                    {
                        MySender.SelectionColor = Color.Black;
                        MySender.AppendText(string.Format("Hex AP Checksum : 0x{0:X4}\n", MyBlock[0].Block_Checksum));
                    }
                    else
                    {
                        foreach (var item in UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode)
                        {
                            if (item.Contains("Timeout"))
                            {
                                MySender.AppendText(string.Format("{0}\n", item));
                            }
                            else
                            {
                                var str = string.IsNullOrEmpty(item) ? "0000" : item;
                                MySender.AppendText(string.Format("Device Block_{0} : 0x{1}\n", iBlocks++, str));
                            }
                        }
                    }
                }
            }
            else
            {
                if (bIsVerifyCodeExist)
                {
                    int iMaximum = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode.Count > MyBlock.Count)
                        ? UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode.Count
                        : MyBlock.Count;
                    for (int i = 0; i < iMaximum; i++)
                    {
                        uint _HexVerifyCode = 0;
                        //if (mcu == V3_MCU.IC_2510) || mcu == V3_MCU.IC_2511) || mcu == V3_MCU.IC_2712))
                        if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                        {
                            if (i == 0)//data code
                                _HexVerifyCode = (uint)MyBlock[i].Block_CRC16;
                            else
                                _HexVerifyCode = (uint)MyBlock[i].Block_Checksum;
                        }
                        else
                        {
                            if (i < MyBlock.Count)
                                _HexVerifyCode = MyBlock[i].Block_CRC16;
                            else
                                _HexVerifyCode = 0;
                        }

                        if (MyBlock.Count > UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode.Count)
                        {
                            if (i >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode.Count)
                            {
                                MySender.SelectionColor = Color.Gray;
                                MySender.AppendText(string.Format("Hex Block_{0} : 0x{1:X4}\n", i, MyBlock[i].Block_CRC16));
                                show_warning_meg = true;
                                MySender.SelectionColor = Color.Black;
                                MySender.AppendText(string.Format("Device Block_{0} : NULL\n", i));
                            }
                            else
                            {
                                int _DevInfoVerifyCode = MyTool.HexStringToInt(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode[i], HEX_TYPE.BIG_ENDIAN, out ErrStr);
                                MySender.SelectionColor = (_HexVerifyCode != MyTool.HexStringToInt(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode[i], HEX_TYPE.BIG_ENDIAN, out ErrStr)) ? Color.Gray : Color.Green;
                                if (MySender.SelectionColor == Color.Gray)
                                    show_warning_meg = true;
                                MySender.AppendText(string.Format("Hex Block_{0} : 0x{1:X4}\n", i, _HexVerifyCode));
                                MySender.SelectionColor = Color.Black;
                                MySender.AppendText(string.Format("Device Block_{0} : 0x{1:X4}\n", i, _DevInfoVerifyCode));
                            }
                        }
                        else if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode.Count >= MyBlock.Count)
                        {
                            int _DevInfoVerifyCode = MyTool.HexStringToInt(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode[i], HEX_TYPE.BIG_ENDIAN, out ErrStr);
                            if (i >= MyBlock.Count)
                            {
                                MySender.SelectionColor = Color.Gray;
                                MySender.AppendText(string.Format("Hex Block_{0} : NULL\n", i));
                                show_warning_meg = true;
                            }
                            else
                            {
                                MySender.SelectionColor = (_HexVerifyCode != _DevInfoVerifyCode) ? Color.Gray : Color.Green;
                                MySender.AppendText(string.Format("Hex Block_{0} : 0x{1:X4}\n", i, _HexVerifyCode));
                                if (_HexVerifyCode != _DevInfoVerifyCode)
                                    show_warning_meg = true;
                            }
                            MySender.SelectionColor = Color.Black;
                            MySender.AppendText(string.Format("Device Block_{0} : 0x{1:X4}\n", i, _DevInfoVerifyCode));
                        }
                    }
                }
                else
                {
                    MySender.SelectionColor = Color.Black;
                    if (is_fromHex)
                    {
                        //if(EnumTool.IsEnumDefined<V3_MCU>(mcu))
                        if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                        {
                            for (int i = 0; i < MyBlock.Count; i++)
                            {
                                if (i == 0)
                                    MySender.AppendText(string.Format("Hex Block_{0} CRC : 0x{1:X4}\n", i, MyBlock[i].Block_CRC16));
                                else
                                    MySender.AppendText(string.Format("Hex Block_{0} Checksum : 0x{1:X}\n", i, MyBlock[i].Block_Checksum));
                            }
                        }
                        else
                        {
                            for (int i = 0; i < MyBlock.Count; i++)
                                MySender.AppendText(string.Format("Hex Block_{0} CRC : 0x{1:X4}\n", i, MyBlock[i].Block_CRC16));
                        }
                    }
                    else
                    {
                        foreach (var item in UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode)
                        {
                            if (item.Contains("Timeout"))
                            {
                                MySender.AppendText(string.Format("{0}\n", item));
                            }
                            else
                            {
                                var str = string.IsNullOrEmpty(item) ? "0000" : item;
                                MySender.AppendText(string.Format("Device Block_{0} : 0x{1}\n", iBlocks++, str));
                            }
                        }
                    }
                }
            }
            return show_warning_meg;
        }

        private void CopyProfileINI(string SourcePath, string DestPath)
        {
            File.Copy(SourcePath, DestPath, true);
        }

        private void DataGridView_Paint(object sender, PaintEventArgs e)
        {
            var MyGridview = sender as DataGridView;
            int iRows = MyGridview.Rows.Count;
            for (int iRow = 0; iRow < iRows; iRow++)
            {
                DataGridViewRow row = MyGridview.Rows[iRow];
                if (MyGridview.Name == dgvRawDataSPEC.Name)
                    row.Cells[1].Style.BackColor = RawDataColor.Exists(x => x.Value.iRowIndex == iRow) ? Color.Gray : Color.White;
            }
        }

        private void DGV_MouseLeave(object sender, EventArgs e)
        {
            var _MySender = sender as DataGridView;
            _MySender.EndEdit();
        }

        private void EditPaintTest(object _DefineItem, string KeyName, string ValueName, object sender, DataGridViewCellEventArgs InEvent)
        {
            TestItemNameDefine DefineItem = (TestItemNameDefine)_DefineItem;
            bool boStatus = true;
            var sectionName = SelectedPattern(tabControl_Painting.SelectedTab).ToString();
            int inSPECIndex = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[DefineItem].SPEC.FindIndex(x => x.Name == sectionName);

            MultiSenTest_VAR._SPEC_ SPECBuffer = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[DefineItem].SPEC[inSPECIndex];
            if (KeyName == "Enable")
            {
                SPECBuffer.dtValue.Rows[0][0] = ValueName;
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[DefineItem].SPEC[inSPECIndex] = SPECBuffer;
                ((DataGridView)(sender)).Rows[InEvent.RowIndex].Cells[InEvent.ColumnIndex].Value = SPECBuffer.dtValue.Rows[0][0];
            }
            else
            {
                MultiSenTest_VAR.LineTestSetting LineSetting = SPECBuffer.lsDrawingSpec.Find(x => x.Name == KeyName);
                string strOriValue = LineSetting.strDefaultValue;
                if (string.IsNullOrEmpty(ValueName))
                {
                    ValueName = strOriValue;
                    ((DataGridView)(sender)).Rows[InEvent.RowIndex].Cells[InEvent.ColumnIndex].Value = ValueName;
                    if ((StaticVar.g_Lang == enLangTable.LANG_TRADITIONAL_CHINESE)
                        || (StaticVar.g_Lang == enLangTable.LANG_SIMPLIFIED_CHINESE))
                        MyUIPage.ShowInfoDialog("", "[Setting Error] 數值不能為空!", UIStyle.Orange, false);
                    else
                        MyUIPage.ShowInfoDialog("", "[Setting Error] Value can't be empty!", UIStyle.Orange, false);
                    //MessageBox.Show("[Setting Error] Value can't be empty!");
                }

                if (LineSetting.Spec_Up != LineSetting.Spec_Low)
                {
                    double dValueName = 0;
                    switch (LineSetting.ValueFormat)
                    {
                        case enSPECFormat._Double:
                            dValueName = double.Parse(ValueName);
                            if ((dValueName < LineSetting.Spec_Low) || (dValueName > LineSetting.Spec_Up))
                            {
                                ((DataGridView)(sender)).Rows[InEvent.RowIndex].Cells[InEvent.ColumnIndex].Value = int.Parse(strOriValue);
                                boStatus = false;
                            }
                            break;

                        case enSPECFormat._Hex:
                            dValueName = (double)Convert.ToInt32(ValueName, 16);
                            if ((dValueName < LineSetting.Spec_Low) || (dValueName > LineSetting.Spec_Up))
                            {
                                ((DataGridView)(sender)).Rows[InEvent.RowIndex].Cells[InEvent.ColumnIndex].Value = strOriValue;
                                boStatus = false;
                            }
                            break;
                    }
                }

                if (boStatus)
                {
                    if (LineSetting.ShowType == enSPECShowType.DataGridViewComboBox)
                    {
                        var tmp = LineSetting.strDisplayType.Split(';').ToList();
                        ValueName = tmp[Convert.ToInt16(ValueName)];
                    }

                    if (!ChangePaintingTestSpecValue(sectionName, KeyName, ValueName))
                    {
                        string ErrorMSG = string.Format("[Setting Error]: [{0}] {1} = {2}", sectionName, KeyName, ValueName);
                        MyUIPage.ShowErrorNotifier(ErrorMSG, false, 3000);
                    }
                }
                else
                {
                    string strMax = string.Format(LineSetting.strDisplayType, LineSetting.Spec_Up);
                    string strMin = string.Format(LineSetting.strDisplayType, LineSetting.Spec_Low);
                    string ErrorMSG = string.Format("[Setting Error] Set Data : {0} Range : {1} ~ {2}", ValueName, strMin, strMax);
                    MyUIPage.ShowErrorNotifier(ErrorMSG, false, 3000);
                    //MessageBox.Show("[Setting Error] Set Data : " + ValueName + " Range : " + strMin + " ~ " + strMax);
                }
            }
        }

        private void FillTestitemDelgate()
        {
            delTestFuntion = new Dictionary<string, delICFuntion>();
            //SwitchPages
            foreach (var item in EnumTool.EnumToList<TestItemNameDefine>())
            {
                string TableName = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(item);
                delTestFuntion.Add(TableName, SwitchPages);
            }
        }

        private int GetScreenCnt
        {
            get
            {
                return Screen.AllScreens.Length;
            }
        }

        private string GetSpecValue(string sectionName, string strKey)
        {
            MultiSenTest_VAR._SPEC_ lsSpec = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enPaintingTest].SPEC.Find(x => x.Name == sectionName);
            MultiSenTest_VAR.LineTestSetting LineTestSettingitem = lsSpec.lsDrawingSpec.Find(x => x.Name == strKey);
            return LineTestSettingitem.strDefaultValue;
        }

        private void Initialize_TestFlow_treeView()
        {
            //==============================
            // TriggerOption
            cmbTrigerMode.Items.Clear();
            foreach (var varValue in EnumTool.EnumToList<TriggerMode>())
            {
                if (varValue == TriggerMode.I2C)
                    continue;
                cmbTrigerMode.Items.Add(varValue.Desc());
            }
            //==============================
            // LCM Fixture Detect
            cmbLCMDetectPin.Items.Clear();
            cmbNoisePatternDetectPin.Items.Clear();
            cmbPatternChangePIN.Items.Clear();
            foreach (var varValue in EnumTool.EnumToList<enBridge_Pin>())
            {
                if (varValue != enBridge_Pin.TS_1)
                {
                    cmbLCMDetectPin.Items.Add(varValue.ToString());
                    cmbNoisePatternDetectPin.Items.Add(varValue.ToString());
                }

                if (varValue == enBridge_Pin.OFF || varValue == enBridge_Pin.TS_1)
                    cmbPatternChangePIN.Items.Add(varValue.ToString());
            }
            cmbLCMDetectType.Items.Clear();
            cmbNoisePatternDetectType.Items.Clear();
            foreach (var varValue in EnumTool.EnumToList<GPIO_PIN_TYPE>())
            {
                if (varValue == GPIO_PIN_TYPE.INT_DISABLE || varValue == GPIO_PIN_TYPE.RECOVER_SETTING || varValue == GPIO_PIN_TYPE.SAVE_NOW_SETTING)
                    continue;

                cmbLCMDetectType.Items.Add(varValue.ToString());
                cmbNoisePatternDetectType.Items.Add(varValue.ToString());
            }

            //==============================
            // SensorTestStation
            foreach (var page in tabControl1.TabPages)
                ((TabPage)page).Parent = null;//隱藏

            cb_SensorTestStation.Items.Clear();
            foreach (var varValue in EnumTool.EnumToList<SensorTestStation>())
            {
                switch (varValue)
                {
                    case SensorTestStation.REPORT:
                    case SensorTestStation.STRESS_TEST:
                    case SensorTestStation.VERIFY_TEST:

                    default:
                        cb_SensorTestStation.Items.Add(varValue.Desc());
                        break;
                }

            }
            //==============================
            // Sync Test Flow
            string strNodeKey = "Test Flow";
            int inNodeIndex = Array.FindIndex(strMainNode, s => s.Equals(strNodeKey));
            treeView_MainSettings.Nodes[inNodeIndex].Nodes.Clear();
            int iReduces = 0;
            foreach (var testitem in UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult)
            {
                if (testitem.Value.bTestitemVisible == false)
                {
                    iReduces++;
                    continue;
                }
                int iSelectIndex = (int)testitem.Key - iReduces;

                treeView_MainSettings.Nodes[inNodeIndex].Nodes.Add(testitem.Value.strTestItemName);
                treeView_MainSettings.Nodes[inNodeIndex].Nodes[iSelectIndex].Checked = false; //true;
                treeView_MainSettings.Nodes[inNodeIndex].Nodes[iSelectIndex].Name = testitem.Key.ToString();
                var rows = from products in UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.DT_SensorTest.AsEnumerable()
                           where products.Field<string>("Test_Name").Equals(testitem.Value.strTestItemName)
                           select products;

                foreach (var item_2 in rows)
                    treeView_MainSettings.Nodes[inNodeIndex].Nodes[iSelectIndex].Checked = true;
            }

            cmb_TS2_usage.Items.Clear();
            cmb_TS1_usage.Items.Clear();
            cmb_PN0_usage.Items.Clear();
            foreach (enBridgeGpioUsage item in Enum.GetValues(typeof(enBridgeGpioUsage)))
            {
                string desc = item.Desc();
                cmb_TS2_usage.Items.Add(desc);
                cmb_TS1_usage.Items.Add(desc);
                cmb_PN0_usage.Items.Add(desc);
            }

            cmb_TS2_trigger.Items.Clear();
            cmb_TS1_trigger.Items.Clear();
            cmb_PN0_trigger.Items.Clear();
            foreach (enBridgeGpioTrigger item in Enum.GetValues(typeof(enBridgeGpioTrigger)))
            {
                string desc = item.Desc();
                cmb_TS2_trigger.Items.Add(desc);
                cmb_TS1_trigger.Items.Add(desc);
                cmb_PN0_trigger.Items.Add(desc);
            }

            string[] ts2 = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioTS2.ToString()).Split(',');
            string[] ts1 = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioTS1.ToString()).Split(',');
            string[] pn0 = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioPN0.ToString()).Split(',');

            cmb_TS2_usage.SelectedIndex = 0;
            cmb_TS1_usage.SelectedIndex = 0;
            cmb_PN0_usage.SelectedIndex = 0;
            cmb_TS2_trigger.SelectedIndex = 0;
            cmb_TS1_trigger.SelectedIndex = 0;
            cmb_PN0_trigger.SelectedIndex = 0;
            if (ts2.Length == 2)
            {
                cmb_TS2_usage.SelectedIndex = cmb_TS2_usage.FindStringExact(ts2[0]);
                cmb_TS2_trigger.SelectedIndex = cmb_TS2_trigger.FindStringExact(ts2[1]);
            }
            if (ts1.Length == 2)
            {
                cmb_TS1_usage.SelectedIndex = cmb_TS1_usage.FindStringExact(ts1[0]);
                cmb_TS1_trigger.SelectedIndex = cmb_TS1_trigger.FindStringExact(ts1[1]);
            }
            if (pn0.Length == 2)
            {
                cmb_PN0_usage.SelectedIndex = cmb_PN0_usage.FindStringExact(pn0[0]);
                cmb_PN0_trigger.SelectedIndex = cmb_PN0_trigger.FindStringExact(pn0[1]);
            }
            uiTextBox_PassDuration.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioPassDuration.ToString());
            uiTextBox_FailDuration.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioFailDuration.ToString());
        }

        #region m_btPanelInfoLoad_Click

        private string ProfileProtocolVersion()
        {
            string DevInterface = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.TestInterface.Desc();
            //string Protocol = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.INI_ProtocolVersion.Split('.')[0];
            //if (Protocol.Contains("0x"))
            //    Protocol = Protocol.Replace("0x", "");
            //else if (Protocol.Contains("0X"))
            //    Protocol = Protocol.Replace("0X", "");
            var Protocol = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.INI_ProtocolVersion.Major;
            return string.Format("Protocol_V{0}_{1}", Protocol, DevInterface);
        }

        private void Judge_FW_FilePath(string ProfilePath)
        {
            INI_Tool TmpINI = new INI_Tool(ProfilePath);
            string strRelativeFilePath = TmpINI.IniReadValue("FW_Verify", "Path", "");
            bool bFileIsNotExist = false;

            if (string.IsNullOrEmpty(strRelativeFilePath))
            {
                bFileIsNotExist = true;
            }
            else
            {
                //string root_A = Path.GetPathRoot(ITS_Directory.Tool_CurrentDirectory);
                //string root_B = Path.GetPathRoot(strRelativeFilePath);
                //if(root_A != root_B)
                //    MyUIPage.ShowErrorDialog("Error Tip", string.Format("The root directory of the path is different from the root directory of the tool.The path will be cleared.\n--> {0}", strRelativeFilePath), UIStyle.Red, true);
                //Console.WriteLine("{0}", MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, strRelativeFilePath));
                if (!Path.IsPathRooted(strRelativeFilePath))
                {
                    string strProfile = ITS_Directory.Tool_CurrentDirectory + strRelativeFilePath;
                    if (!File.Exists(strProfile))
                        bFileIsNotExist = true;
                }
            }

            if (bFileIsNotExist)
            {
                MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enUpgrade];
                TmpINI.IniWriteValue("FW_Verify", "Path", "");
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, "Path", "");

                TmpINI.IniWriteValue("FW_Verify", "Master_CRC", "");
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, "Master_CRC", "");

                TmpINI.IniWriteValue("FW_Verify", "Slave_CRC", "");
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, "Slave_CRC", "");
            }
        }

        private void Judge_FW_Fixed_FilePath(string ProfilePath)
        {
            INI_Tool TmpINI = new INI_Tool(ProfilePath);
            string strRelativeFilePath = TmpINI.IniReadValue("FW_Fixed", "Path", "");
            bool bFileIsNotExist = false;

            if (string.IsNullOrEmpty(strRelativeFilePath))
            {
                bFileIsNotExist = true;
            }
            else
            {
                if (!Path.IsPathRooted(strRelativeFilePath))
                {
                    string strProfile = ITS_Directory.Tool_CurrentDirectory + strRelativeFilePath;
                    if (!File.Exists(strProfile))
                        bFileIsNotExist = true;
                }
            }

            if (bFileIsNotExist)
            {
                MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enFWFixed];
                TmpINI.IniWriteValue("FW_Fixed", "Path", "");
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, "Path", "");

                TmpINI.IniWriteValue("FW_Fixed", "Master_CRC", "");
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, "Master_CRC", "");

                TmpINI.IniWriteValue("FW_Fixed", "Slave_CRC", "");
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, "Slave_CRC", "");
            }
        }

        private void Judge_GPIO_Test(string ProfilePath)
        {
            var NowItemArr = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enGpio_Test];
            var _Tmp = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.TestInterface;
            int iFound = -1;
            string _Tmp1 = "";
            string _TestName = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enGpio_Test);
            INI_Tool TmpINI = new INI_Tool(ProfilePath);
            if (_Tmp != DEV_INTERFACE_TYPE.I2C)
            {
                _Tmp1 = INI_Name_GPIOTest.I2C_INT.ToString(); //20220714
                TmpINI.IniWriteValue(_TestName, _Tmp1, "OFF");
                iFound = NowItemArr.SPEC.FindIndex(x => x.Name.Equals(_Tmp1));
                var tmpSpec = NowItemArr.SPEC[iFound];
                tmpSpec.bTestEn = false;
                NowItemArr.SPEC[iFound] = tmpSpec;

                _Tmp1 = INI_Name_GPIOTest.I2C_Reset.ToString(); //20220714
                TmpINI.IniWriteValue(_TestName, _Tmp1, "OFF");
                iFound = NowItemArr.SPEC.FindIndex(x => x.Name.Equals(_Tmp1));
                tmpSpec = NowItemArr.SPEC[iFound];
                tmpSpec.bTestEn = false;
                NowItemArr.SPEC[iFound] = tmpSpec;
            }
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
            {
                _Tmp1 = INI_Name_GPIOTest.ID_Pin_Test.ToString();
                TmpINI.IniWriteValue(_TestName, _Tmp1, "0x00");
            }
            bool bChecked = false;
            foreach (var item in NowItemArr.SPEC)
            {
                if (item.Name != INI_Name_GPIOTest.ID_Pin_Test.ToString())
                    bChecked |= item.bTestEn;
                else
                    bChecked |= (item.dtValue.Rows[0][0].ToString() != "0x00");
            }
            NowItemArr.bEnable = bChecked;

            TmpINI.IniWriteValue(_TestName, "Enable", bChecked.ToString());
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enGpio_Test] = NowItemArr;
        }

        private void GenerateAllNodeData(string ProfilePath)
        {
#if BLOCKLAYOUT_DISABLE
            //bKeepAdjustedData
            bool bIsNeedUpdate = false;
            bool bAlreadyGen = false;
            INI_Tool TmpINI = new INI_Tool(ProfilePath);
            string SectorName = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enUniformityTest);
            var DataTypeArray = EnumTool.EnumToList<Uniformity_SectorName>();
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation == SensorTestStation.FPC)
            {
                TmpINI.ClearSector(SectorName);
                foreach (var item in DataTypeArray)
                    TmpINI.ClearSector(item.ToString());
                return;
            }

            foreach (var DataTypeName in EnumTool.EnumToList<Uniformity_SectorName>())
            {
                int iNum = 0;
                bIsNeedUpdate = false;
                List<string> lsCheck = TmpINI.IniReadAllContains(DataTypeName.ToString());
                if (lsCheck == null)
                    bIsNeedUpdate = true;
                else if (lsCheck.Count() == 0)
                    bIsNeedUpdate = true;

                int iColumns = 0;
                int iRows = 0;
                if (bIsNeedUpdate == false)
                {
                    iColumns = lsCheck[0].Split(';').Count() - 1;//最後會多一個空白欄位，需扣掉。
                    iRows = lsCheck.Count();
                }

                int iMaxValue = -1;
                int iMinValue = -1;
                int iBorderMax = -1;
                switch (DataTypeName)
                {
                    case Uniformity_SectorName.Uniformity_RawData:
                        bIsNeedUpdate |= (iColumns != m_Channel_List[(int)_Ch_.X]);
                        bIsNeedUpdate |= (iRows != m_Channel_List[(int)_Ch_.Y]);
                        //Channel數不同時，不管要不要保留都得清掉;Channel數相同時，由使用者決定要不要保留
                        if (!bIsNeedUpdate)
                            bIsNeedUpdate = !m_bKeepAdjustedData;
                        if (!bIsNeedUpdate)
                            break;
                        //===============================
                        // Reset BlockLay_Fix to true
                        //===============================
                        iColumns = m_Channel_List[(int)_Ch_.X];
                        iRows = m_Channel_List[(int)_Ch_.Y];
                        //iMaxValue = TmpINI.IniReadInt(SectorName, INI_Name_UniformityTest.Uniformity_RawData_Max_Threshold.ToString(), 4500);
                        //iMinValue = TmpINI.IniReadInt(SectorName, INI_Name_UniformityTest.Uniformity_RawData_Min_Threshold.ToString(), 500);
                        iMaxValue = 4500;
                        iMinValue = 500;
                        break;

                    case Uniformity_SectorName.Uniformity_Win1:
                        bIsNeedUpdate |= (iColumns != m_Channel_List[(int)_Ch_.X]);
                        bIsNeedUpdate |= (iRows != m_Channel_List[(int)_Ch_.Y] - 1);
                        //Channel數不同時，不管要不要保留都得清掉;Channel數相同時，由使用者決定要不要保留
                        if (!bIsNeedUpdate)
                            bIsNeedUpdate = !m_bKeepAdjustedData;
                        if (!bIsNeedUpdate)
                            break;
                        iColumns = m_Channel_List[(int)_Ch_.X];
                        iRows = m_Channel_List[(int)_Ch_.Y] - 1;
                        //iMaxValue = TmpINI.IniReadInt(SectorName, INI_Name_UniformityTest.Uniformity_Win1_Max_Threshold.ToString(), 200);
                        iMaxValue = 200;
                        break;

                    case Uniformity_SectorName.Uniformity_Win2:
                        bIsNeedUpdate |= (iColumns != m_Channel_List[(int)_Ch_.X] - 1);
                        bIsNeedUpdate |= (iRows != m_Channel_List[(int)_Ch_.Y] - 1);
                        //Channel數不同時，不管要不要保留都得清掉;Channel數相同時，由使用者決定要不要保留
                        if (!bIsNeedUpdate)
                            bIsNeedUpdate = !m_bKeepAdjustedData;
                        if (!bIsNeedUpdate)
                            break;
                        iColumns = m_Channel_List[(int)_Ch_.X] - 1;
                        iRows = m_Channel_List[(int)_Ch_.Y] - 1;
                        //iMaxValue = TmpINI.IniReadInt(SectorName, INI_Name_UniformityTest.Uniformity_Win2_Max_Threshold.ToString(), 30);
                        iMaxValue = 30;
                        break;

                    case Uniformity_SectorName.Uniformity_KeyRawData:
                        bIsNeedUpdate |= (iColumns != m_Channel_List[(int)_Ch_.Key]);
                        bIsNeedUpdate |= (iRows != 1);
                        //Channel數不同時，不管要不要保留都得清掉;Channel數相同時，由使用者決定要不要保留
                        if (!bIsNeedUpdate)
                            bIsNeedUpdate = !m_bKeepAdjustedData;
                        if (!bIsNeedUpdate)
                            break;
                        iColumns = m_Channel_List[(int)_Ch_.Key];
                        iRows = 1;
                        //iMaxValue = TmpINI.IniReadInt(SectorName, INI_Name_UniformityTest.Uniformity_KeyRawData_Max_Threshold.ToString(), 4500);
                        //iMinValue = TmpINI.IniReadInt(SectorName, INI_Name_UniformityTest.Uniformity_KeyRawData_Min_Threshold.ToString(), 500);
                        iMaxValue = 4500;
                        iMinValue = 500;
                        break;

                    case Uniformity_SectorName.Uniformity_TxDiff:
                        bIsNeedUpdate |= (iColumns != m_Channel_List[(int)_Ch_.X]);
                        bIsNeedUpdate |= (iRows != m_Channel_List[(int)_Ch_.Y] - 1);
                        //Channel數不同時，不管要不要保留都得清掉;Channel數相同時，由使用者決定要不要保留
                        if (!bIsNeedUpdate)
                            bIsNeedUpdate = !m_bKeepAdjustedData;
                        if (!bIsNeedUpdate)
                            break;
                        iColumns = m_Channel_List[(int)_Ch_.X];
                        iRows = m_Channel_List[(int)_Ch_.Y] - 1;
                        //var str = TmpINI.IniReadValue(SectorName, INI_Name_UniformityTest.Uniformity_TxDiff_Max_Threshold.ToString(), "15%").Replace("%", "");
                        //iMaxValue = int.Parse(str);
                        iMaxValue = 15;
                        //str = TmpINI.IniReadValue(SectorName, INI_Name_UniformityTest.Uniformity_TxDiffEdge_Max_Threshold.ToString(), "20%").Replace("%", "");
                        //iBorderMax = int.Parse(str);
                        iBorderMax = 20;
                        break;

                    case Uniformity_SectorName.Uniformity_RxDiff:
                        bIsNeedUpdate |= (iColumns != m_Channel_List[(int)_Ch_.X] - 1);
                        bIsNeedUpdate |= (iRows != m_Channel_List[(int)_Ch_.Y]);
                        //Channel數不同時，不管要不要保留都得清掉;Channel數相同時，由使用者決定要不要保留
                        if (!bIsNeedUpdate)
                            bIsNeedUpdate = !m_bKeepAdjustedData;
                        if (!bIsNeedUpdate)
                            break;
                        iColumns = m_Channel_List[(int)_Ch_.X] - 1;
                        iRows = m_Channel_List[(int)_Ch_.Y];
                        //var str_2 = TmpINI.IniReadValue(SectorName, INI_Name_UniformityTest.Uniformity_RxDiff_Max_Threshold.ToString(), "15%").Replace("%", "");
                        //iMaxValue = int.Parse(str_2);
                        iMaxValue = 15;
                        //str_2 = TmpINI.IniReadValue(SectorName, INI_Name_UniformityTest.Uniformity_RxDiffEdge_Max_Threshold.ToString(), "20%").Replace("%", "");
                        //iBorderMax = int.Parse(str_2);
                        iBorderMax = 20;
                        break;
                }
                TmpINI.ClearSector(DataTypeName.ToString());
                if ((DataTypeName == Uniformity_SectorName.Uniformity_TxDiff) || (DataTypeName == Uniformity_SectorName.Uniformity_RxDiff))
                {
                    if ((iColumns != 0) && (iRows != 0))
                    {
                        StringBuilder AllText = new StringBuilder();

                        for (int iRow = 0; iRow < iRows; iRow++)
                        {
                            for (int iCol = 0; iCol < iColumns; iCol++)
                            {
                                if ((iRow == 0) || iRow == iRows - 1)
                                    AllText.AppendFormat("{0},{1},{2},{3};", iNum, iBorderMax, -1, 1);
                                else if ((iCol == 0) || (iCol == iColumns - 1))
                                    AllText.AppendFormat("{0},{1},{2},{3};", iNum, iBorderMax, -1, 1);
                                else
                                    AllText.AppendFormat("{0},{1},{2},{3};", iNum, iMaxValue, -1, 1);
                            }
                            AllText.AppendLine("");
                        }

                        if (!bAlreadyGen)
                        {
                            StringBuilder _Golden = new StringBuilder();
                            for (int i = 0; i < m_Channel_List[(int)_Ch_.Y]; i++)
                                _Golden.AppendLine(string.Join("", Enumerable.Repeat("0,-1,-1,1;", m_Channel_List[(int)_Ch_.X]).ToArray()));
                            TmpINI.ClearSector("Uniformity_Golden");
                            TmpINI.IniWriteAllSector("Uniformity_Golden", _Golden);
                            bAlreadyGen = true;
                        }
                        TmpINI.IniWriteAllSector(DataTypeName.ToString(), AllText);
                    }
                }
                else
                {
                    if ((iColumns != 0) && (iRows != 0))
                    {
                        StringBuilder AllText = new StringBuilder();
                        for (int iRow = 0; iRow < iRows; iRow++)
                        {
                            for (int iCol = 0; iCol < iColumns; iCol++)
                                AllText.AppendFormat("{0},{1},{2},{3};", iNum, iMaxValue, iMinValue, 1);

                            AllText.AppendLine("");
                        }
                        TmpINI.IniWriteAllSector(DataTypeName.ToString(), AllText);
                    }
                }
            }
#else
            //bKeepAdjustedData
            int iNum = 0;
            bool bIsNeedUpdate = false;
            //bool bAlreadyGen = false;
            INI_Tool TmpINI = new INI_Tool(ProfilePath);
            string SectorName = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enUniformityTest);
            var DataTypeArray = EnumTool.EnumToList<Uniformity_SectorName>();
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation == SensorTestStation.FPC)
            {
                TmpINI.ClearSector(SectorName);
                foreach (var item in DataTypeArray)
                    TmpINI.ClearSector(item.ToString());
                return;
            }
            int iColumns = 0;
            int iRows = 0;
            bIsNeedUpdate |= (iColumns != m_Channel_List[(int)_Ch_.Key]);
            bIsNeedUpdate |= (iRows != 1);
            //Channel數不同時，不管要不要保留都得清掉;Channel數相同時，由使用者決定要不要保留
            if (!bIsNeedUpdate)
                bIsNeedUpdate = !m_bKeepAdjustedData;
            if (!bIsNeedUpdate)
                return;
            iColumns = m_Channel_List[(int)_Ch_.Key];
            iRows = 1;
            int iMaxValue = 4500;
            int iMinValue = 500;
            if (!bIsNeedUpdate)
                return;
            if ((iColumns != 0) && (iRows != 0))
            {
                StringBuilder AllText = new StringBuilder();
                for (int iRow = 0; iRow < iRows; iRow++)
                {
                    for (int iCol = 0; iCol < iColumns; iCol++)
                        AllText.AppendFormat("{0},{1},{2},{3};", iNum, iMaxValue, iMinValue, 1);

                    AllText.AppendLine("");
                }
                TmpINI.IniWriteAllSector(Uniformity_SectorName.Uniformity_KeyRawData.ToString(), AllText);
            }
#endif

        }

        private void GenerateLineSetting(string ProfilePath)
        {
            INI_Tool TmpINI = new INI_Tool(ProfilePath);
            string strSectorName = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enPaintingTest);
            //if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
            var lsPaintTestPattern = EnumTool.EnumToList<enPaintTestPattern>();
            //else


            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation == SensorTestStation.FPC)
            {
                TmpINI.ClearSector(strSectorName);
                foreach (var item in lsPaintTestPattern)
                    TmpINI.ClearSector(item.ToString());
                return;
            }

            Dictionary<enPanel_Info, double> Dic_Panel_Info = new Dictionary<enPanel_Info, double>();
            var Arr = new enPanel_Info[] { enPanel_Info.AAXmax, enPanel_Info.AAXmin, enPanel_Info.AAYmax, enPanel_Info.AAYmin };
            foreach (var item in Arr)
                Dic_Panel_Info.Add(item, double.Parse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", item.ToString())));

            foreach (var item in lsPaintTestPattern)
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3 && item.Desc().Contains("Pen"))
                    continue;
                double GapPercent = 10;
                switch (item)
                {
                    case enPaintTestPattern.Paint_PaintCross:
                    case enPaintTestPattern.Paint_PaintRectangle:
                    case enPaintTestPattern.Paint_PenCross:
                    case enPaintTestPattern.Paint_PenRectangle:
                        GapPercent = 10;
                        break;
                    default:
                        continue;
                }

                string lineSetting_str = TmpINI.IniReadValue(item.ToString(), "LineSetting", "");
                string[] line_split = lineSetting_str.Split(';');
                string[] line_dir = new string[line_split.Count()];
                for (int i = 0; i < line_split.Count(); i++)
                    line_dir[i] = line_split[i].Split(',')[5];

                string str = SetPatternDefault(item, GapPercent, Dic_Panel_Info, line_dir);
                if (ChangePaintingTestSpecValue(item.ToString(), "LineSetting", str))
                    TmpINI.IniWriteValue(item.ToString(), "LineSetting", str);
            }
        }

        private string SetPatternDefault(enPaintTestPattern _item, double _GapPercent, object _Dic_Panel_Info, string[] dir)
        {
            var tmp_Dic_Panel_Info = (Dictionary<enPanel_Info, double>)_Dic_Panel_Info;
            StringBuilder str = new StringBuilder();

            switch (_item)
            {
                case enPaintTestPattern.Paint_PaintRectangle:
                case enPaintTestPattern.Paint_PenRectangle:
                    int iH_Gap = (int)(tmp_Dic_Panel_Info[enPanel_Info.AAYmax] / 100 * _GapPercent + 0.5);
                    int iW_Gap = (int)(tmp_Dic_Panel_Info[enPanel_Info.AAXmax] / 100 * _GapPercent + 0.5);
                    str.AppendFormat("{0},{1},{2},{3},{4},{5};", 1
                            , tmp_Dic_Panel_Info[enPanel_Info.AAXmin], tmp_Dic_Panel_Info[enPanel_Info.AAYmin]
                            , tmp_Dic_Panel_Info[enPanel_Info.AAXmax], iH_Gap
                            , dir[0]);

                    str.AppendFormat("{0},{1},{2},{3},{4},{5};", 2
                        , tmp_Dic_Panel_Info[enPanel_Info.AAXmax] - iW_Gap, tmp_Dic_Panel_Info[enPanel_Info.AAYmin]
                        , iW_Gap, tmp_Dic_Panel_Info[enPanel_Info.AAYmax]
                        , dir[1]);

                    str.AppendFormat("{0},{1},{2},{3},{4},{5};", 3
                        , tmp_Dic_Panel_Info[enPanel_Info.AAXmin], tmp_Dic_Panel_Info[enPanel_Info.AAYmax] - iH_Gap
                        , tmp_Dic_Panel_Info[enPanel_Info.AAXmax], iH_Gap
                        , dir[2]);

                    str.AppendFormat("{0},{1},{2},{3},{4},{5}", 4
                        , tmp_Dic_Panel_Info[enPanel_Info.AAXmin], tmp_Dic_Panel_Info[enPanel_Info.AAYmin]
                        , iW_Gap, tmp_Dic_Panel_Info[enPanel_Info.AAYmax]
                        , dir[3]);
                    break;

                case enPaintTestPattern.Paint_PaintCross:
                case enPaintTestPattern.Paint_PenCross:
                    Point _A = new Point((int)tmp_Dic_Panel_Info[enPanel_Info.AAXmin], (int)tmp_Dic_Panel_Info[enPanel_Info.AAYmin]);
                    Point _B = new Point((int)tmp_Dic_Panel_Info[enPanel_Info.AAXmax], (int)tmp_Dic_Panel_Info[enPanel_Info.AAYmax]);
                    Point _P = new Point((int)tmp_Dic_Panel_Info[enPanel_Info.AAXmax], (int)tmp_Dic_Panel_Info[enPanel_Info.AAYmin]);
                    str.AppendFormat("Line_{0},{1},{2},{3},{4},{5},{6};"
                        , 1, _A.X, _A.Y, _B.X, _B.Y
                        , (int)(DistanceForPointToABLine(_P, _A, _B) / 100 * _GapPercent + 0.5)
                        , enCrossDirection.LT_RB);

                    _A = new Point((int)tmp_Dic_Panel_Info[enPanel_Info.AAXmax], (int)tmp_Dic_Panel_Info[enPanel_Info.AAYmin]);
                    _B = new Point((int)tmp_Dic_Panel_Info[enPanel_Info.AAXmin], (int)tmp_Dic_Panel_Info[enPanel_Info.AAYmax]);
                    _P = new Point((int)tmp_Dic_Panel_Info[enPanel_Info.AAXmin], (int)tmp_Dic_Panel_Info[enPanel_Info.AAYmin]);
                    str.AppendFormat("Line_{0},{1},{2},{3},{4},{5},{6}"
                        , 2, _A.X, _A.Y, _B.X, _B.Y
                        , (int)(DistanceForPointToABLine(_P, _A, _B) / 100 * _GapPercent + 0.5)
                        , enCrossDirection.RT_LB);
                    break;

                default:
                    break;
            }


            return str.ToString();
        }

        private void GenerateMicroOpenTest(string ProfilePath)
        {
            bool bIsNeedUpdate = false;
            INI_Tool TmpINI = new INI_Tool(ProfilePath);
            var list = EnumTool.EnumToList<MicroOpen_TestItems>();
            string SectorName = TestItemNameDefine.enMicroOpenTest.DescArr(':')[1];
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation == SensorTestStation.FPC)
            {
                TmpINI.ClearSector(SectorName);
                foreach (var item in list)
                    TmpINI.ClearSector(item.ToString());
                return;
            }

            foreach (var DataTypeName in list)
            {
                int iColorNum = 0;
                List<string> lsCheck = TmpINI.IniReadAllContains(DataTypeName.ToString());
                bIsNeedUpdate = false;
                if (lsCheck == null)
                    bIsNeedUpdate = true;
                else if (lsCheck.Count() == 0)
                    bIsNeedUpdate = true;

                int iColumns = 0;
                int iRows = 0;
                if (bIsNeedUpdate == false)
                {
                    iColumns = lsCheck[0].Split(';').Where(x => !string.IsNullOrEmpty(x)).Select(x => x).Count();
                    iRows = lsCheck.Count();
                }
                int iMaxValue = 0;
                switch (DataTypeName)
                {
                    case MicroOpen_TestItems.RX_Delta:

                        bIsNeedUpdate |= (iColumns != (m_Channel_List[(int)_Ch_.X] - 1));
                        bIsNeedUpdate |= (iRows != m_Channel_List[(int)_Ch_.Y]);
                        if (!bIsNeedUpdate)
                            bIsNeedUpdate = !m_bKeepAdjustedData;
                        if (!bIsNeedUpdate)
                            break;

                        iColumns = m_Channel_List[(int)_Ch_.X] - 1;
                        iRows = m_Channel_List[(int)_Ch_.Y];
                        //iMaxValue = TmpINI.IniReadInt(SectorName, INI_Name_MicroOpenTest.RX_Delta_Threshold.ToString(), 10);
                        iMaxValue = 200;
                        break;

                    case MicroOpen_TestItems.TX_Avg_Delta:
                        bIsNeedUpdate |= (iColumns != 1);
                        bIsNeedUpdate |= (iRows != (m_Channel_List[(int)_Ch_.Y] - 1));
                        if (!bIsNeedUpdate)
                            bIsNeedUpdate = !m_bKeepAdjustedData;
                        if (!bIsNeedUpdate)
                            break;
                        iColumns = 1;
                        iRows = m_Channel_List[(int)_Ch_.Y] - 1;
                        iMaxValue = TmpINI.IniReadInt(SectorName, INI_Name_MicroOpenTest.TX_Avg_Delta_Threshold.ToString(), 10);
                        break;
                }

                if (bIsNeedUpdate)
                {
                    TmpINI.ClearSector(DataTypeName.ToString());
                    StringBuilder AllText = new StringBuilder();
                    for (int iRow = 0; iRow < iRows; iRow++)
                    {
                        var OneLine = Enumerable.Repeat<string>(string.Format("{0},{1},{2},{3}", iColorNum, iMaxValue, -1, 1), iColumns).ToList();
                        AllText.AppendFormat("{0};", string.Join(";", OneLine));//為了相容格式，還是要加一個';'
                        AllText.AppendLine("");
                    }
                    TmpINI.IniWriteAllSector(DataTypeName.ToString(), AllText);
                }
            }
        }

        private void JudgeBenchmarkSetting(string ProfilePath)
        {
            INI_Tool TmpINI = new INI_Tool(ProfilePath);
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation == SensorTestStation.FPC)
            {
                TmpINI.ClearSector("Uniformity_Benchmark");
                return;
            }

            List<string> Uniformity_Benchmark = TmpINI.IniReadAllContains("Uniformity_Benchmark");
            if (Uniformity_Benchmark == null)
                return;
            int iWidth = Uniformity_Benchmark[0].Split(';').ToList().Count;
            int iHeight = Uniformity_Benchmark.Count;

            if ((m_Channel_List[(int)_Ch_.X] != iWidth) || (m_Channel_List[(int)_Ch_.Y] != iHeight))
            {
                //MessageBox.Show("Uniformity_Benchmark channel not match!!\nNow Remove.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                MyUIPage.ShowInfoDialog("Warning", "Uniformity_Benchmark channel not match!!\nNow Remove.", UIStyle.Orange, false);
                TmpINI.ClearSector("Uniformity_Benchmark");
            }
        }

        private void m_btPanelInfoLoad_Click(object sender, EventArgs e)
        {
            string ProfileProtocol = ProfileProtocolVersion();
            string NowProtocol = "";
            string NowICType = "";
            Version nowProtocolVersion = new Version();
            //DialogResult myResult = DialogResult.None;
            bool myResult = false;
            bool isSupportReadPidByCommand = false;
            string sPIDFromCommandRecord = string.Empty;
            string ErrorMSG = "";
            string strMsg = "";
            string _str = "";
            if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_En;
            else
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_Cn;
            MyUIPage.ShowWaitForm(_str);

            //============================
            // Read Device now protocol, MCU, Device channel
            var tmpstr = "";
            if (!UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ReloadNowDeviceStatus(out NowProtocol, out tmpstr, out NowICType, ref ErrorMSG))
            {
                nowProtocolVersion = new Version(0, 0, 0);
                if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    _str = global::ITS_Studio.Properties.Resources.DeviceStatusErr_EN;
                else
                    _str = global::ITS_Studio.Properties.Resources.DeviceStatusErr_CN;
                ErrorMSG += _str;
                goto GOTO_Return;
            }
            nowProtocolVersion = StringTool.HexStringToVer(tmpstr);
            #region 檢查channel、MCU、Protocol

            //============================
            // Judge protocol
            //============================
            if (ProfileProtocol != NowProtocol)
            {
                if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    _str = global::ITS_Studio.Properties.Resources.ProtocolErr_EN;
                else
                    _str = global::ITS_Studio.Properties.Resources.ProtocolErr_CN;
                ErrorMSG += _str;
                goto GOTO_AskDialog;
            }

            if (Wifi_Var.useWifi && (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType != UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.TestInterface))
            {
                if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    _str = global::ITS_Studio.Properties.Resources.ProtocolErr_EN;
                else
                    _str = global::ITS_Studio.Properties.Resources.ProtocolErr_CN;
                _str += string.Format("profile: {0} !\n", UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.TestInterface);
                ErrorMSG += _str;
                goto GOTO_AskDialog;
            }
            //============================
            // Get_IC_Information
            //============================
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.SuspendMode, Command._SWITCH.Dis);

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.Get_IC_Information(UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode, false, ref strMsg, ref ErrorMSG);

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);

            int.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.XChannel.ToString()), out m_Channel_List[(int)_Ch_.X]);
            int.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.YChannel.ToString()), out m_Channel_List[(int)_Ch_.Y]);
            int.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.TouchKeyChannel.ToString()), out m_Channel_List[(int)_Ch_.Key]);

            //============================
            // Judge channel
            //============================
            if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X != m_Channel_List[(int)_Ch_.X])
                || (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y != m_Channel_List[(int)_Ch_.Y])
                || (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts != m_Channel_List[(int)_Ch_.Key]))
            {
                if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    _str = global::ITS_Studio.Properties.Resources.ChannelErr_EN;
                else
                    _str = global::ITS_Studio.Properties.Resources.ChannelErr_CN;
                ErrorMSG += _str;
                goto GOTO_AskDialog;
            }

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts != 0)
            {
                INI_Tool Profile_INI = new INI_Tool(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                StringCollection lsBuf = new StringCollection();
                Profile_INI.ReadAllSections(ref lsBuf);
                if (lsBuf.IndexOf("Uniformity_KeyRawData") == -1)
                {
                    if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                        _str = global::ITS_Studio.Properties.Resources.ChannelErr_EN;
                    else
                        _str = global::ITS_Studio.Properties.Resources.ChannelErr_CN;
                    ErrorMSG += _str;
                    goto GOTO_AskDialog;
                }
            }
            //============================
            // Judge MCU
            //============================
            string Profile_MCU = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("IC_Verify", "IC_Type");
            if (!string.Equals(Profile_MCU, NowICType))
            {
                if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    _str = global::ITS_Studio.Properties.Resources.MCUErr_EN;
                else
                    _str = global::ITS_Studio.Properties.Resources.MCUErr_CN;
                ErrorMSG += _str;
                goto GOTO_AskDialog;
            }

            //============================
            // Judge Profile Version
            //============================
            string strINIProfile = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("System", "ProfileVersion");
            if (strINIProfile != VersionInfo.ProfileVersion.ToString())
            {
                if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    _str = global::ITS_Studio.Properties.Resources.ProfileErr_EN;
                else
                    _str = global::ITS_Studio.Properties.Resources.ProfileErr_CN;
                ErrorMSG += _str;
                goto GOTO_AskDialog;
            }

            //============================
            // Judge PID (Except HID Over I2C)
            //============================
            byte[] ReturnValue = new byte[0];
            Version real_protocol_Version = new Version(0, 0, 0);
            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_ProtocolVersion(out ReturnValue))
            {
                real_protocol_Version = new Version(ReturnValue[0], ReturnValue[1], ReturnValue[2]);
            }
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_HID_Info(FW_MODE.AP_MODE, real_protocol_Version, out ReturnValue); //傳入現在的Mode和Protocol Version

            var PID = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.lsMainSetting.Find(x => x.strKey == "PID").strValue;

            if (ReturnValue != null) //Protocol支援
            {
                isSupportReadPidByCommand = true;

                var Dev_Pid = string.Format("0x{0:X04}", (ReturnValue[1] << 8 | ReturnValue[0]).ToString("X4"));
                sPIDFromCommandRecord = Dev_Pid;
                if (Dev_Pid != PID)
                {
                    //ErrorMSG += string.Format("PID not mapping !!\n");
                    if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                        _str = global::ITS_Studio.Properties.Resources.PIDErr_EN;
                    else
                        _str = global::ITS_Studio.Properties.Resources.PIDErr_CN;
                    ErrorMSG += _str;
                    goto GOTO_AskDialog;
                }
            }
            else
            {
                if (!StaticVar.lsMain_USBDevInfo[m_iSelectedDev].bIsHID_Over_I2C)
                {
                    var Dev_Pid = string.Format("0x{0:X04}", StaticVar.lsMain_USBDevInfo[m_iSelectedDev].usPID);
                    if (Dev_Pid != PID)
                    {
                        //ErrorMSG += string.Format("PID not mapping !!\n");
                        if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                            _str = global::ITS_Studio.Properties.Resources.PIDErr_EN;
                        else
                            _str = global::ITS_Studio.Properties.Resources.PIDErr_CN;
                        ErrorMSG += _str;
                        goto GOTO_AskDialog;
                    }
                }
            }
            //============================
            // Judge Pen Type
            //============================
            tabPage_Pen.Parent = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType == Pen_CDC_Format.enPenType.Disable) ? null : tabControl_Painting;
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType != Pen_CDC_Format.enPenType.Disable)
            {
                string strPenType = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", "PenType");
                if (strPenType != UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType.ToString())
                {
                    //ErrorMSG += string.Format("PenType is not mapped!!\n");
                    if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                        _str = global::ITS_Studio.Properties.Resources.PenTypeErr_EN;
                    else
                        _str = global::ITS_Studio.Properties.Resources.PenTypeErr_CN;
                    ErrorMSG += _str;
                }

                goto GOTO_AskDialog;
            }


            #endregion 檢查channel、MCU、Protocol
        //============================
        GOTO_AskDialog:
            if (!string.IsNullOrEmpty(ErrorMSG))
            {
                string message = "", Title = "";
                if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                {
                    _str = global::ITS_Studio.Properties.Resources.UpdateProfile_EN;
                    message = global::ITS_Studio.Properties.Resources.UpdateProfileWarning_EN;
                    Title = "Note!!!";
                }
                else
                {
                    _str = global::ITS_Studio.Properties.Resources.UpdateProfile_CN;
                    message = global::ITS_Studio.Properties.Resources.UpdateProfileWarning_CN;
                    Title = "注意!!!";
                }
                _str = _str.Replace("/n", "\n");
                myResult = MyUIPage.ShowAskDialog(Title, ErrorMSG + _str, UIStyle.Orange, true);
                if (!myResult)
                    goto GOTO_Return;
                
                //m_bKeepAdjustedData = MyUIPage.ShowAskDialog("Note!!!", "\nIf all channels are the same, do you want to keep the adjusted frame data? (Uniformity、MicroOpen)\n", UIStyle.LayuiRed, true);
                var _Result = FormShowAskDialog.ShowAskDialog(this, Title, message, UIStyle.LayuiRed, true);
                if (_Result == System.Windows.Forms.DialogResult.OK)
                    m_bKeepAdjustedData = true;
                else
                    m_bKeepAdjustedData = false;
                ErrorMSG = string.Empty;
            }

            this.Enabled = false;

            //============================
            // Reset Device Protocol
            //============================
            #region Reset Protocol
            if (UI_GBV.fmITS_Tool.ResetProtocol(m_iSelectedDev, NowProtocol) == false)// Update protocol and channel
                goto GOTO_Return;

            if (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.BridgeSettingInit_API(StaticVar.bridgeSetting.m_bridge_var.iPower_On_DelayTime);

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();//開啟USB接收
            MyTool.Delay_ms(300);
            if (!UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.CheckDevMode_API(FW_MODE.AP_MODE))
            {
                //ErrorMSG = string.Format("Device is BL mode.\n");
                if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    _str = global::ITS_Studio.Properties.Resources.DevBLMode_EN;
                else
                    _str = global::ITS_Studio.Properties.Resources.DevBLMode_CN;
                ErrorMSG += _str;
                goto GOTO_Return;
            }
            #endregion Reset Protocol
            //===============================
            // Reset PID
            //===============================
            byte[] bReturnValue = new byte[0];
            real_protocol_Version = new Version(0, 0, 0);
            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_ProtocolVersion(out ReturnValue))
            {
                real_protocol_Version = new Version(ReturnValue[0], ReturnValue[1], ReturnValue[2]);
            }
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_HID_Info(FW_MODE.AP_MODE, real_protocol_Version, out ReturnValue); //傳入現在的Mode和Protocol Version           

            if (ReturnValue != null) //Protocol支援
            {
                isSupportReadPidByCommand = true;
                sPIDFromCommandRecord = string.Format("0x{0:X04}", (ReturnValue[1] << 8 | ReturnValue[0]).ToString("X4"));
            }
            else
            {
                if (!StaticVar.lsMain_USBDevInfo[m_iSelectedDev].bIsHID_Over_I2C)
                    sPIDFromCommandRecord = string.Format("0x{0:X04}", StaticVar.lsMain_USBDevInfo[m_iSelectedDev].usPID);
            }

            //=================================
            // ReLoad Profile
            //=================================
            #region Reset SensorTest、 Load Profile、MainSettings、SensorTest Condition

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ResetAllTestContains(enReadFrom.INI);
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.INI_ProtocolVersion != nowProtocolVersion)
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.INI_ProtocolVersion = nowProtocolVersion;

            //UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.strICType = NowICType;
            Console.WriteLine("DevPanelInfo.MCU = {0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU);
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.TestInterface = StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType;

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.vfProtocolInit();

            #endregion Reset SensorTest、 Load Profile、MainSettings、SensorTest Condition

            //============================
            // Get_IC_Information
            //============================
            #region Get Panel info
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.SuspendMode, Command._SWITCH.Dis);

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.Get_IC_Information(UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode, false, ref strMsg, ref ErrorMSG);

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);

            //============================
            // UpdateProtocolInfo
            //============================
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.UpdateProtocolInfo();
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Set_PanelInfo_Var(enReadFrom.Memory);

            int.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.XChannel.ToString()), out m_Channel_List[(int)_Ch_.X]);
            int.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.YChannel.ToString()), out m_Channel_List[(int)_Ch_.Y]);
            int.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.TouchKeyChannel.ToString()), out m_Channel_List[(int)_Ch_.Key]);

            //============================
            // Judge Resolution
            //============================
            int[] Resolution = new int[4];
            int.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.AAXmax.ToString()), out Resolution[0]);
            int.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.AAYmax.ToString()), out Resolution[1]);
            int.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.X_resolution.ToString()), out Resolution[2]);
            int.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.Y_resolution.ToString()), out Resolution[3]);

            bool bCheckRes = true;
            bCheckRes &= (UI_GBV.fm_SensorTest_Painting.m_SensorTestPaint_GBV.Panel_Var.inAAXmax == Resolution[0]);
            bCheckRes &= (UI_GBV.fm_SensorTest_Painting.m_SensorTestPaint_GBV.Panel_Var.inAAYmax == Resolution[1]);
            bCheckRes &= (UI_GBV.fm_SensorTest_Painting.m_SensorTestPaint_GBV.Panel_Var.inScreenInfoXmax == Resolution[2]);
            bCheckRes &= (UI_GBV.fm_SensorTest_Painting.m_SensorTestPaint_GBV.Panel_Var.inScreenInfoYmax == Resolution[3]);
            if (!bCheckRes)
                myResult = true;

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);

            #endregion Get Panel info

            //=================================
            UpdatePanelInfoGrid(isSupportReadPidByCommand, sPIDFromCommandRecord);

            //=================================

            #region 重新產生預設的Raw設定
            if (myResult)
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
                //===============================

                vfWriteProfile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                GenerateAllNodeData(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                GenerateLineSetting(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                GenerateMicroOpenTest(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                Judge_FW_FilePath(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                Judge_GPIO_Test(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                WriteDefaultFrequency();
                WriteDefaultChargeCurve();

            }
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.GenVal_fromIni(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation != SensorTestStation.FPC)              
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.WriteAutoGenItem(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);

            #endregion 重新產生預設的Raw設定

            #region 將CDC設定寫入Profile裡面
            CDC_To_Profile();
            #endregion 將CDC設定寫入Profile裡面

            //=================================
            // ReLoad Profile
            //=================================
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ResetAllTestContains(enReadFrom.INI);
            //UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.GenVal_fromIni(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);//重複了
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.CopyRunningProfile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);

            //===============================
            // Reset BlockLay_Fix to true
            //=================================       
            var mySelect = EnumTool.GetValueFromDescription<TestItemNameDefine>(treeView_MainSettings.SelectedNode.Text);
            ShowSpecOnUI(mySelect);

            Initial_treeView(TestItemNameDefine.enPanel_Info);//這一行要擺在最後面
            //清除在profile裡的多行空白
            string text = File.ReadAllText(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            string result = Regex.Replace(text, @"(^\p{Zs}*\r\n){2,}", "\r\n", RegexOptions.Multiline);
            File.WriteAllText(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath, result);
        //清除在profile裡的多行空白 end
        //=================================        
        GOTO_Return:
            this.Enabled = true;
            MyUIPage.HideWaitForm();
            if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                _str = global::ITS_Studio.Properties.Resources.LoadPanelFinish_EN;
            else
                _str = global::ITS_Studio.Properties.Resources.LoadPanelFinish_CN;
            if (string.IsNullOrEmpty(ErrorMSG))
                MyUIPage.ShowSuccessNotifier(_str);
            else
                MyUIPage.ShowErrorNotifier(ErrorMSG, false, 3000);
            return;
        }

        private void CDC_To_Profile()
        {
            INI_Tool Profile_INI = new INI_Tool(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            INI_Tool CDCINI = new INI_Tool(ITS_Directory.CDCTool_INI);
            INI_Tool SensorTestDefaultSpec = new INI_Tool(ITS_Directory.SensorTestDefaultSpec);

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                foreach (var item in EnumTool.EnumToList<V6_CDC_MODE_Define.ShortTestPara>())
                {
                    //20220414 Steven Added : 29系列要從Dump3和Dump4讀取值，並還是寫到Profile的Dump1和Dump2
                    string strValue = string.Empty;
                    if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.JudgeIs29Series()
                        && item == V6_CDC_MODE_Define.ShortTestPara.Dump1)
                    {
                        strValue = CDCINI.IniReadValue("ShortTest", "Dump3", item.Desc());
                    }
                    else if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.JudgeIs29Series()
                        && item == V6_CDC_MODE_Define.ShortTestPara.Dump2)
                    {
                        strValue = CDCINI.IniReadValue("ShortTest", "Dump4", item.Desc());
                    }
                    else if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.JudgeIs29Series()
                        && item == V6_CDC_MODE_Define.ShortTestPara.VrefL)
                    {
                        strValue = CDCINI.IniReadValue("ShortTest", "VrefL2", item.Desc());
                    }
                    else
                    {
                        strValue = CDCINI.IniReadValue("ShortTest", item.ToString(), item.Desc());
                    }

                    Profile_INI.IniWriteValue("Short_Test", item.ToString(), strValue);
                }

                foreach (var item in EnumTool.EnumToList<V6_CDC_MODE_Define.OpenDeltaInfoPara>())
                {
                    Match rxMatch = Regex.Match(item.Desc(), @"(?<=Default,)(.*)");
                    string strValue = CDCINI.IniReadValue("OpenDelta_Info", item.ToString(), rxMatch.Value);
                    Profile_INI.IniWriteValue("Open_Test", item.ToString(), strValue);
                }

                if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.JudgeIs29Series())
                {
                    string str_2901Series_Threshold = SensorTestDefaultSpec.IniReadValue("ShortTest", "2901Series_Threshold",
                         V6_CDC_MODE_Define.ShortTestDefaultSpec._2901Series_Threshold.Desc());
                    string str_2901Series_KeyTX_Threshold = SensorTestDefaultSpec.IniReadValue("ShortTest", "2901Series_KeyTX_Threshold",
                        V6_CDC_MODE_Define.ShortTestDefaultSpec._2901Series_KeyTX_Threshold.Desc());
                    string str_2901Series_KeyRX_Threshold = SensorTestDefaultSpec.IniReadValue("ShortTest", "2901Series_KeyRX_Threshold",
                        V6_CDC_MODE_Define.ShortTestDefaultSpec._2901Series_KeyRX_Threshold.Desc());
                    string str_2901Series_FailLimit = SensorTestDefaultSpec.IniReadValue("ShortTest", "2901Series_FailLimit",
                      V6_CDC_MODE_Define.ShortTestDefaultSpec._2901Series_FailLimit.Desc());

                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation != SensorTestStation.FPC) //FPC Mode
                    {
                        Profile_INI.IniWriteValue("Short_Test", "Max_Threshold", str_2901Series_Threshold);//20220707 Steven 經過討論將29XX系列預設改為300 / 其餘部分改為750
                        Profile_INI.IniWriteValue("Short_Test", "KeyTX_Threshold", str_2901Series_KeyTX_Threshold);
                        Profile_INI.IniWriteValue("Short_Test", "KeyRX_Threshold", str_2901Series_KeyRX_Threshold);
                        Profile_INI.IniWriteValue("Short_Test", "FailLimit", str_2901Series_FailLimit);
                    }
                    else
                    {
                        Profile_INI.IniWriteValue("Short_Test", "FPC_Max_Threshold", str_2901Series_Threshold);//20220707 Steven 經過討論將29XX系列預設改為300 / 其餘部分改為750
                        Profile_INI.IniWriteValue("Short_Test", "KeyTX_Threshold", str_2901Series_KeyTX_Threshold);
                        Profile_INI.IniWriteValue("Short_Test", "KeyRX_Threshold", str_2901Series_KeyRX_Threshold);
                        Profile_INI.IniWriteValue("Short_Test", "FailLimit", str_2901Series_FailLimit);
                    }
                }
                else //Module Mode
                {
                    string str_Lego_Threshold = SensorTestDefaultSpec.IniReadValue("ShortTest", "Lego_Threshold",
                             V6_CDC_MODE_Define.ShortTestDefaultSpec._Lego_Threshold.Desc());
                    string str_Lego_KeyTX_Threshold = SensorTestDefaultSpec.IniReadValue("ShortTest", "Lego_KeyTX_Threshold",
                            V6_CDC_MODE_Define.ShortTestDefaultSpec._Lego_KeyTX_Threshold.Desc());
                    string str_Lego_KeyRX_Threshold = SensorTestDefaultSpec.IniReadValue("ShortTest", "Lego_KeyRX_Threshold",
                            V6_CDC_MODE_Define.ShortTestDefaultSpec._Lego_KeyRX_Threshold.Desc());
                    string str_Lego_FailLimit = SensorTestDefaultSpec.IniReadValue("ShortTest", "Lego_FailLimit",
                      V6_CDC_MODE_Define.ShortTestDefaultSpec._Lego_FailLimit.Desc());

                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation != SensorTestStation.FPC) //FPC Mode
                    {
                        Profile_INI.IniWriteValue("Short_Test", "Max_Threshold", str_Lego_Threshold);
                        Profile_INI.IniWriteValue("Short_Test", "KeyTX_Threshold", str_Lego_KeyTX_Threshold);
                        Profile_INI.IniWriteValue("Short_Test", "KeyRX_Threshold", str_Lego_KeyRX_Threshold);
                        Profile_INI.IniWriteValue("Short_Test", "FailLimit", str_Lego_FailLimit);
                    }
                    else
                    {
                        Profile_INI.IniWriteValue("Short_Test", "FPC_Max_Threshold", str_Lego_Threshold);//20220707 Steven 經過討論將29XX系列預設改為300 / 其餘部分改為750
                        Profile_INI.IniWriteValue("Short_Test", "KeyTX_Threshold", str_Lego_KeyTX_Threshold);
                        Profile_INI.IniWriteValue("Short_Test", "KeyRX_Threshold", str_Lego_KeyRX_Threshold);
                        Profile_INI.IniWriteValue("Short_Test", "FailLimit", str_Lego_FailLimit);
                    }
                }
            }
            else  //V3寫入預設值7
            {
                foreach (var item in EnumTool.EnumToList<V6_CDC_MODE_Define.ShortTestPara>())
                {
                    string strValue = CDCINI.IniReadValue("ShortTest", item.ToString(), item.Desc());
                    Profile_INI.IniWriteValue("Short_Test", item.ToString(), strValue);
                }
                foreach (var item in EnumTool.EnumToList<V6_CDC_MODE_Define.OpenDeltaInfoPara>())
                {
                    Match rxMatch = Regex.Match(item.Desc(), @"(?<=Default,)(.*)");
                    string strValue = CDCINI.IniReadValue("OpenDelta_Info", item.ToString(), rxMatch.Value);
                    Profile_INI.IniWriteValue("Open_Test", item.ToString(), strValue);
                }
                string str_V3IC_Threshold = SensorTestDefaultSpec.IniReadValue("ShortTest", "V3IC_Threshold",
                        V3_CDC_MODE_Define.ShortTestDefaultSpec._V3IC_Threshold.Desc());
                string str_V3IC_KeyTX_Threshold = SensorTestDefaultSpec.IniReadValue("ShortTest", "V3IC_KeyTXThreshold",
                      V3_CDC_MODE_Define.ShortTestDefaultSpec._V3IC_KeyTX_Threshold.Desc());
                string str_V3IC_KeyRX_Threshold = SensorTestDefaultSpec.IniReadValue("ShortTest", "V3IC_KeyRXThreshold",
                      V3_CDC_MODE_Define.ShortTestDefaultSpec._V3IC_KeyRX_Threshold.Desc());

                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation != SensorTestStation.FPC) //FPC Mode
                {
                    Profile_INI.IniWriteValue("Short_Test", "Max_Threshold", str_V3IC_Threshold);//20221115 V3 Short預設值改7 (根據V3 Profile)
                    Profile_INI.IniWriteValue("Short_Test", "KeyTX_Threshold", str_V3IC_KeyTX_Threshold);
                    Profile_INI.IniWriteValue("Short_Test", "KeyRX_Threshold", str_V3IC_KeyRX_Threshold);
                }
                else
                {
                    Profile_INI.IniWriteValue("Short_Test", "FPC_Max_Threshold", str_V3IC_Threshold);//20221115 V3 Short預設值改7 (根據V3 Profile)
                    Profile_INI.IniWriteValue("Short_Test", "KeyTX_Threshold", str_V3IC_KeyTX_Threshold);
                    Profile_INI.IniWriteValue("Short_Test", "KeyRX_Threshold", str_V3IC_KeyRX_Threshold);
                }
            }
        }

        private void WriteDefaultFrequency()
        {
            var LocalVar = UI_GBV.mDev[m_iSelectedDev].LocalVariable;
            INI_Tool ProfileINI = new INI_Tool(LocalVar.VarSenTest_Multi.strProfileAbsolutePath);

            //====================================================================
            var Profile_NoiseFrq_V6 = "FrequencyScan_V6";
            var _defVal = LocalVar.VarSenTest_Multi.FrequencyScan_V6_Default;

            ProfileINI.IniWriteValue(Profile_NoiseFrq_V6, "Enable", true.ToString());
            foreach (var item in EnumTool.EnumToList<INI_Name_FrequencyScan_V6>())
            {
                ProfileINI.IniWriteValue(Profile_NoiseFrq_V6, item.ToString(), _defVal[item].ToString());
            }
        }

        private void WriteDefaultChargeCurve()
        {
            //UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ChargeCurveTool_.LoadXML_Params(ITS_Directory.ChargeCurveSetting, false);
            var LocalVar = UI_GBV.mDev[m_iSelectedDev].LocalVariable;
            //LocalVar.ChargeCurveVar.m_Positions = new Point[UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ChargeCurveTool_.iPointCnts];

            INI_Tool ProfileINI = new INI_Tool(LocalVar.VarSenTest_Multi.strProfileAbsolutePath);

            var Profile_ChargeCurve = "ChargeCurve";
            //ProfileINI.IniWriteValue(Profile_ChargeCurve, "Enable", true.ToString());
            var KeyName = ChargeCurveTool.KeyWord.ScanDataType.ToString();
            ProfileINI.IniWriteValue(Profile_ChargeCurve, KeyName, "_MC_SINE_");
            KeyName = ChargeCurveTool.KeyWord.Frame_Count.ToString();
            ProfileINI.IniWriteValue(Profile_ChargeCurve, KeyName, "1");
            KeyName = ChargeCurveTool.KeyWord.Frequency_Interval.ToString();
            ProfileINI.IniWriteValue(Profile_ChargeCurve, KeyName, "Start:100,Step:10,End:1000,");
            KeyName = ChargeCurveTool.KeyWord.ShotCnts.ToString();
            ProfileINI.IniWriteValue(Profile_ChargeCurve, KeyName, "1");
            KeyName = ChargeCurveTool.KeyWord.Position.ToString();
            ProfileINI.IniWriteValue(Profile_ChargeCurve, KeyName, "{2,2},{11,2},{21,2},{2,7},{11,7},{21,7},{2,13},{11,13},{21,13}");
        }
        #endregion

        private void setTextBox(UITextBox box, enReportPathRule tmpItem, ref string member)
        {
            string strPath = string.Format(@"{0}", box.Text);
            if (tmpItem == enReportPathRule.Relative_Path)
            {
                string tmp = strPath;
                if (!Path.IsPathRooted(strPath))
                    tmp = Path.GetFullPath(strPath);

                if (string.IsNullOrEmpty(Path.GetFileName(tmp)))
                    member = "";
                else
                    member = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, tmp);
                //MyTool.Save_DBG_Log("ITS_Directory.Tool_CurrentDirectory = {0}, tmp={1}", ITS_Directory.Tool_CurrentDirectory, tmp);
            }
            else
            {
                member = Path.GetFullPath(strPath);
            }
            box.Text = member;
        }

        private void PathRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            var MySender = sender as UIComboBox;
            UITextBox box;
            string memberPath;
            if (MySender == cbAbsoluteRelativePath)
            {
                box = tbReportPath;
                memberPath = strReportPath;
            }
            else
            {
                box = tb_autogen_path;
                memberPath = strAutoGenPath;
            }
            if (!iChangeFlag)
                return;
            if (string.IsNullOrEmpty(box.Text))
                return;

            string strItemSelect = MySender.SelectedItem.ToString();
            var tmpItem = EnumTool.GetValueFromDescription<enReportPathRule>(strItemSelect);

            setTextBox(box, tmpItem, ref memberPath);

        }

        private void ReportFile_CheckedChanged(object sender, EventArgs e)
        {
            if (!iChangeFlag)
                return;

            int iReportFolder = 0, iReportFileNumber = 0;
            int iRuleGroup = (int)enReportGroup.ReportFolder;

            foreach (int EveryRule in Enum.GetValues(typeof(ReportFolderRule)))
            {
                iReportFolder = MyTool.SetIntegerSomeBit(iReportFolder, EveryRule, m_cbReport[iRuleGroup][EveryRule].Checked);
            }

            iRuleGroup = (int)enReportGroup.ReportFileName;
            foreach (int EveryRule in Enum.GetValues(typeof(ReportFileRule)))
            {
                iReportFileNumber = MyTool.SetIntegerSomeBit(iReportFileNumber, EveryRule, m_cbReport[iRuleGroup][EveryRule].Checked);
            }
            iFolderRuleNumber = iReportFolder;
            iReportRuleNumber = iReportFileNumber;
            Change_ReportRule(iFolderRuleNumber, iReportRuleNumber);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
        }

        private void CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                ToolStripMenuItem_Profile.Enabled = false;
            }));

            var dgv = sender as DataGridView;
            if (dgv.Columns[e.ColumnIndex].Name == "Name")
                e.Cancel = true;
        }

        private void MouseClick_CellBeginEdit(object sender, DataGridViewCellEventArgs e)
        {
            var MySender = sender as DataGridView;
            if (MySender.Columns[e.ColumnIndex].Name == "Value")
                MySender.BeginEdit(true);
        }

        private void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ToolStripMenuItem_Profile.Enabled = true;
            var myGridView = sender as DataGridView;

            if(myGridView == dgvPanelInfoSPEC)
                return;

            if (myGridView == dgvShortTestSPEC)
                SetShortNoteMessage(myGridView);
            //===================================
            string _name = myGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
            string Parent_name = _name.Replace("_Tolerance", "");
            NowParentName = _name.Contains("_Tolerance") ? NowParentName : Parent_name;
            if (_name == "LineSetting")
                return;
            //===================================

            if (!_name.Contains("_Tolerance"))
            {
                var _Value = myGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                int i_value = -1;
                if (!int.TryParse(_Value, out i_value))
                {
                    _Value = System.Text.RegularExpressions.Regex.Replace(_Value, @"[^0-9]+", "");
                    int.TryParse(_Value, out i_value);
                }

                if (myGridView == dgvRawDataSPEC)
                    Update_dgvColor(e.RowIndex, Parent_name, _name, i_value, ref RawDataColor);
            }
        }

        private void DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var MyGridview = sender as DataGridView;

            int iRows = MyGridview.Rows.Count;
            for (int iRow = 0; iRow < iRows; iRow++)
            {
                DataGridViewRow row = MyGridview.Rows[iRow];
                var _value = row.Cells[1].Value;
                string _name = row.Cells[0].Value.ToString();
                string Parent_name = _name.Replace("_Tolerance", "");
                NowParentName = _name.Contains("_Tolerance") ? NowParentName : Parent_name;

                if (_value is DBNull)
                    return;
                if (!_value.GetType().Equals(typeof(string)))
                {
                    int i_value = Convert.ToInt32(_value);

                    if (MyGridview == dgvRawDataSPEC)
                        Update_dgvColor(iRow, Parent_name, _name, i_value, ref RawDataColor);
                    //else if (MyGridview == m_dgvUniformitySPEC)
                    //    Update_dgvColor(iRow, Parent_name, _name, i_value, ref UniformityColor);
                }
            }
        }

        private void SPEC_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            List<string> lsDescriptions = EnumTool.GetDescriptions<TestItemNameDefine>();
            if (lsDescriptions.Exists(x => x.Contains(tbSPEC_Settings.SelectedTab.Name)))
            {
                int iSelect = lsDescriptions.FindIndex(x => x.Contains(tbSPEC_Settings.SelectedTab.Name));
                string itemName = Enum.GetName(typeof(TestItemNameDefine), iSelect);
                TestItemNameDefine iParNum = (TestItemNameDefine)Enum.Parse(typeof(TestItemNameDefine), itemName);
                SetRawData(iParNum,
                            ((DataGridView)(sender)).Rows[e.RowIndex].Cells[0].Value.ToString(),
                            ((DataGridView)(sender)).Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString(),
                            ((DataGridView)(sender)),
                            e);
            }
        }

        #region PaintTest

        private void PaintingSPEC_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var DgvIndex = UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern;
            DataGridView mySender = sender as DataGridView;
            mySender.CellValueChanged -= new DataGridViewCellEventHandler(this.PaintingSPEC_CellValueChanged);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;

            if (mySender[e.ColumnIndex, e.RowIndex].GetType().Equals(typeof(DataGridViewTextBoxCell)))
            {

                SetRawData(TestItemNameDefine.enPaintingTest,
                                 mySender.Rows[e.RowIndex].Cells[0].Value.ToString(),
                                 mySender.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString(),
                                 mySender, e);
            }
            else if (mySender[e.ColumnIndex, e.RowIndex].GetType().Equals(typeof(DataGridViewComboBoxCell)))
            {
                var _num = -1;
                if (int.TryParse(mySender.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out _num))
                {
                    SetRawData(TestItemNameDefine.enPaintingTest,
                                mySender.Rows[e.RowIndex].Cells[0].Value.ToString(), _num.ToString(),
                                mySender, e);
                }
            }

            bool bResult = PatternReset(false, DgvIndex);

            mySender.CellValueChanged += new DataGridViewCellEventHandler(this.PaintingSPEC_CellValueChanged);

            if (!bResult)
                mySender.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "3";
        }

        private void PaintingSPEC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 48) && (e.KeyChar <= 57))  // 0 ~ 9
                e.Handled = false;
            else if (e.KeyChar == (Char)8) // Backpace
                e.Handled = false;
            else if (e.KeyChar == (Char)46) // .
                e.Handled = false;
            else if (e.KeyChar == (Char)45) // -
                e.Handled = false;
            else if (e.KeyChar == (Char)27) // ESC
                e.Handled = false;
            else
                e.Handled = true;
            //GBV.SenTest.bSettingsChange = true;
        }

        private enPaintTestPattern SelectedPattern(TabPage tab)
        {
            string SelectedTab = "";

            if (tab == tabPage_Finger)
                SelectedTab = tabCtl_FingerPatternItems.SelectedTab.Text;
            else if (tab == tabPage_Pen)
                SelectedTab = tabCtl_PenPatternItems.SelectedTab.Text;

            return EnumTool.GetValueFromDescription_DrawingTestSetting<enPaintTestPattern>(SelectedTab);
        }

        private void TreeViewItem_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView TreeViewBuf = (TreeView)sender;
            if (TreeViewBuf.SelectedNode == null)
                return;
            //====================================================
            var SelectedTebName = SelectedPattern(tabControl_Painting.SelectedTab);
            TestLineAttribute.SelectedIndexChanged -= PaintingAttributes_SelectedIndexChanged;
            TestLineAttribute.SelectedIndex = -1;
            TestLineAttribute.SelectedIndexChanged += PaintingAttributes_SelectedIndexChanged;
            TestLineAttribute.Enabled = true;
            switch (SelectedTebName)
            {
                case enPaintTestPattern.Paint_PaintRectangle:
                case enPaintTestPattern.Paint_PenRectangle:
                case enPaintTestPattern.Paint_PaintRectangle3:
                case enPaintTestPattern.Paint_PenRectangle3:
                    m_PaintLineBuf = string.Join(";", m_lsRectangleLinebuf.Select(x => x.ToString(true)).ToArray());
                    if (TreeViewBuf.SelectedNode.Parent != null)
                        Rectangle_AfterSelect(TreeViewBuf);
                    else
                        TestLineAttribute.Enabled = false;
                    break;

                case enPaintTestPattern.Paint_PaintCross:
                case enPaintTestPattern.Paint_PenCross:
                case enPaintTestPattern.Paint_PaintCross3:
                case enPaintTestPattern.Paint_PenCross3:
                    m_PaintLineBuf = string.Join(";", m_lsCrossLinebuf.Select(x => x.ToAttribute()).ToArray());
                    if (TreeViewBuf.SelectedNode.Parent != null)
                        Cross_AfterSelect(TreeViewBuf);
                    else
                        TestLineAttribute.Enabled = false;
                    break;
            }

            //====================================================
            PatternReset(true, SelectedTebName);//GUI Init
        }

        //樹狀清單選擇單項
        private void Rectangle_AfterSelect(object sender)
        {
            var TreeViewBuf = (TreeView)sender;

            string strvMainNode = enPaintingNodeName.Main.Desc();
            Match rxMatch;
            string strbuf = "";
            if (TreeViewBuf.SelectedNode.Parent.Text == strvMainNode)
            {
                rxMatch = Regex.Match(TreeViewBuf.SelectedNode.Text, @"Line ([0-9]) ");
                int iGP_Cnt = Convert.ToInt32(rxMatch.Groups[1].ToString());
                strbuf = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enPaintingTest].SPEC.Find(x => x.Name.Contains("Paint_")).Name;

                if (string.IsNullOrEmpty(strbuf))
                {
                    m_PaintLineBuf = "";
                }
                else
                {
                    List<LineSetting_Rectangle> lsLineSettingbuf = m_lsRectangleLinebuf.FindAll(x => x.iGroup == iGP_Cnt);
                    m_PaintLineBuf = string.Join(";", lsLineSettingbuf.Select(x => x.ToString(true)).ToArray());
                    TestLineAttribute.SelectedTab = tpPainting_Rectangle;
                }
            }
            else
            {
                rxMatch = Regex.Match(TreeViewBuf.SelectedNode.Parent.Text, @"Line ([0-9]) ");
                if (rxMatch.Success)
                {
                    int iGP_Cnt = Convert.ToInt32(rxMatch.Groups[1].ToString());
                    var sectionName = SelectedPattern(tabControl_Painting.SelectedTab).ToString();
                    strbuf = GetSpecValue(sectionName, "LineSetting");

                    if (strbuf != "")
                    {
                        List<LineSetting_Rectangle> lsLineSettingbuf = m_lsRectangleLinebuf.FindAll(x => x.iGroup == iGP_Cnt);
                        m_PaintLineBuf += lsLineSettingbuf[TreeViewBuf.SelectedNode.Index].ToString();
                    }
                }
                TestLineAttribute.SelectedTab = tpPainting_Rectangle;
            }
        }

        private void Cross_AfterSelect(object sender)
        {
            var TreeViewBuf = (TreeView)sender;

            string strvMainNode = enPaintingNodeName.Main.Desc();

            string strbuf = "";
            if (TreeViewBuf.SelectedNode.Parent.Text == strvMainNode)
            {
                var _select = TreeViewBuf.SelectedNode.Text;
                strbuf = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enPaintingTest].SPEC.Find(x => x.Name.Contains("Paint_")).Name;
                if (string.IsNullOrEmpty(strbuf))
                {
                    m_PaintLineBuf = "";
                }
                else
                {
                    List<LineSetting_Cross> lsLineSettingbuf = m_lsCrossLinebuf.FindAll(x => x.NameID == _select);
                    m_PaintLineBuf = string.Join(";", lsLineSettingbuf.Select(x => x.ToAttribute()).ToArray());
                    TestLineAttribute.SelectedTab = tpPainting_Cross;
                }
            }
            else
            {
                //int iGP_Cnt = Convert.ToInt32(rxMatch.Groups[1].ToString());
                var _select = TreeViewBuf.SelectedNode.Text;
                //var sectionName = EnumTool.GetValueFromDescription_DrawingTestSetting<enPaintTestPattern>(tabCtl_FingerPatternItems.SelectedTab.Text).ToString();
                var sectionName = SelectedPattern(tabControl_Painting.SelectedTab).ToString();
                strbuf = GetSpecValue(sectionName, "LineSetting");

                if (strbuf != "")
                {
                    List<LineSetting_Cross> lsLineSettingbuf = m_lsCrossLinebuf.FindAll(x => x.NameID == _select);
                    m_PaintLineBuf += lsLineSettingbuf[TreeViewBuf.SelectedNode.Index].ToString();
                }
                TestLineAttribute.SelectedTab = tpPainting_Cross;
            }
        }

        //樹狀清單被選擇的時候會被觸發
        private void PaintingAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TestLineAttribute.SelectedTab == tpPainting_Cross)
                ReflashCrossAttributes(m_PaintLineBuf);
            else if (TestLineAttribute.SelectedTab == tpPainting_Rectangle)
                ReflashRectangleAttributes(m_PaintLineBuf);
        }

        private void ReflashCrossAttributes(string strLineSetting)
        {
            if (string.IsNullOrEmpty(strLineSetting))
            {
                nuCrossStartX.ValueChanged -= new System.EventHandler(LineValueChange);
                nuCrossStartY.ValueChanged -= new System.EventHandler(LineValueChange);
                nuCrossEndX.ValueChanged -= new System.EventHandler(LineValueChange);
                nuCrossEndY.ValueChanged -= new System.EventHandler(LineValueChange);
                nuCrossDistance.ValueChanged -= new System.EventHandler(LineValueChange);
                cbCrossDirection.SelectedIndexChanged -= new System.EventHandler(LineValueChange);
                nuCrossStartX.Value = 0;
                nuCrossStartY.Value = 0;
                nuCrossEndX.Value = 0;
                nuCrossEndY.Value = 0;
                nuCrossDistance.Value = 0;
                //txt_Cross_Distance_Percent.Text = "0";
                nuCross_Distance_Percent.ResetText();
                cbCrossDirection.Items.Clear();
                return;
            }

            var lsLineSettingbuf = new List<LineSetting_Cross>();
            lsLineSettingbuf.Add(new LineSetting_Cross(strLineSetting.Split(',').Cast<object>().ToList()));

            var lsLineSettingItem = lsLineSettingbuf[0];

            cbCrossLineType.SelectedIndexChanged -= new System.EventHandler(LineValueChange);
            cbCrossDirection.SelectedIndexChanged -= new System.EventHandler(LineValueChange);
            cbCrossLineType.Items.Clear();
            cbCrossDirection.Items.Clear();
            cbCrossLineType.Items.AddRange(EnumTool.GetDescriptions<enCross>().ToArray());
            //foreach(var varValue in EnumTool.EnumToList<enCross>())
            //    cbCrossLineType.Items.Add(varValue.Desc());
            //cbCrossLineType.Items.Add(varValue.ToString());

            switch (lsLineSettingItem.DirectionRule)
            {
                case enCrossDirection.LT_RB:
                case enCrossDirection.RB_LT:
                    cbCrossLineType.SelectedItem = enCross.BackSlash.ToString();
                    cbCrossDirection.Items.Add(enCrossDirection.LT_RB.Desc());
                    cbCrossDirection.Items.Add(enCrossDirection.RB_LT.Desc());
                    break;
                case enCrossDirection.RT_LB:
                case enCrossDirection.LB_RT:
                    cbCrossLineType.SelectedItem = enCross.Slash.ToString();
                    cbCrossDirection.Items.Add(enCrossDirection.RT_LB.Desc());
                    cbCrossDirection.Items.Add(enCrossDirection.LB_RT.Desc());
                    break;
            }
            //cbCrossGroup.SelectedItem = lsLineSettingItem.NameID;
            cbCrossDirection.SelectedItem = lsLineSettingItem.DirectionRule.Desc();
            cbCrossLineType.SelectedIndexChanged += new System.EventHandler(LineValueChange);
            cbCrossDirection.SelectedIndexChanged += new System.EventHandler(LineValueChange);

            string strKey = "Panel_Info";
            int iResolution_X_max = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAXmax.ToString()));
            int iResolution_Y_max = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAYmax.ToString()));
            int iResolution_X_min = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAXmin.ToString()));
            int iResolution_Y_min = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAYmin.ToString()));
            // X => m_nuPaintingItemX
            nuCrossStartX.ValueChanged -= new System.EventHandler(LineValueChange);
            nuCrossStartX.Maximum = iResolution_X_max;
            nuCrossStartX.Minimum = iResolution_X_min;
            nuCrossStartX.Value = lsLineSettingItem.StartPoint.X;
            nuCrossStartX.ValueChanged += new System.EventHandler(LineValueChange);

            nuCrossStartY.ValueChanged -= new System.EventHandler(LineValueChange);
            nuCrossStartY.Maximum = iResolution_Y_max;
            nuCrossStartY.Minimum = iResolution_Y_min;
            nuCrossStartY.Value = lsLineSettingItem.StartPoint.Y;
            nuCrossStartY.ValueChanged += new System.EventHandler(LineValueChange);

            nuCrossEndX.ValueChanged -= new System.EventHandler(LineValueChange);
            nuCrossEndX.Maximum = iResolution_X_max;
            nuCrossEndX.Minimum = iResolution_X_min;
            nuCrossEndX.Value = lsLineSettingItem.EndPoint.X;
            nuCrossEndX.ValueChanged += new System.EventHandler(LineValueChange);

            nuCrossEndY.ValueChanged -= new System.EventHandler(LineValueChange);
            nuCrossEndY.Maximum = iResolution_Y_max;
            nuCrossEndY.Minimum = iResolution_Y_min;
            nuCrossEndY.Value = lsLineSettingItem.EndPoint.Y;
            nuCrossEndY.ValueChanged += new System.EventHandler(LineValueChange);

            nuCrossDistance.ValueChanged -= new System.EventHandler(LineValueChange);
            bool bSlash = GetSlope(lsLineSettingItem.EndPoint, lsLineSettingItem.StartPoint) > 0;
            int iX = bSlash ? lsLineSettingItem.StartPoint.X : lsLineSettingItem.EndPoint.X;
            int iY = bSlash ? lsLineSettingItem.EndPoint.Y : lsLineSettingItem.StartPoint.Y;
            Point _tmpPoint = new Point(iX, iY);
            nuCrossDistance.Maximum = (decimal)DistanceForPointToABLine(_tmpPoint, lsLineSettingItem.StartPoint, lsLineSettingItem.EndPoint);
            nuCrossDistance.Minimum = 0;
            nuCrossDistance.Value = lsLineSettingItem.iDistance;
            nuCrossDistance.ValueChanged += new System.EventHandler(LineValueChange);

            decimal _NowValue = lsLineSettingItem.iDistance;
            decimal _MaxValue = nuCrossDistance.Maximum;
            decimal _Value = ((_NowValue / _MaxValue) * 100);
            nuCross_Distance_Percent.Text = Math.Round(_Value, 0, MidpointRounding.AwayFromZero).ToString();
        }

        private double GetSlope(Point p1, Point p2)
        {
            return Math.Abs((double)((p2.Y - p1.Y) / (p2.X - p1.X)));
        }

        public float DistanceForPointToABLine(Point point, Point point_1, Point point_2)//所在點到AB線段的垂線長度
        {
            float reVal = 0f;
            bool retData = false;

            float cross = (point_2.X - point_1.X) * (point.X - point_1.X) + (point_2.Y - point_1.Y) * (point.Y - point_1.Y);
            if (cross <= 0)
            {
                reVal = (float)Math.Sqrt((point.X - point_1.X) * (point.X - point_1.X) + (point.Y - point_1.Y) * (point.Y - point_1.Y));
                retData = true;
            }

            float d2 = (point_2.X - point_1.X) * (point_2.X - point_1.X) + (point_2.Y - point_1.Y) * (point_2.Y - point_1.Y);
            if (cross >= d2)
            {
                reVal = (float)Math.Sqrt((point.X - point_2.X) * (point.X - point_2.X) + (point.Y - point_2.Y) * (point.Y - point_2.Y));
                retData = true;
            }

            if (!retData)
            {
                float r = cross / d2;
                float px = point_1.X + (point_2.X - point_1.X) * r;
                float py = point_1.Y + (point_2.Y - point_1.Y) * r;
                reVal = (float)Math.Sqrt((point.X - px) * (point.X - px) + (py - point.Y) * (py - point.Y));
            }

            return reVal;
        }

        private void ReflashRectangleAttributes(string strLineSetting)
        {
            if (string.IsNullOrEmpty(strLineSetting))
            {
                cbRectangleGroup.SelectedIndexChanged -= new System.EventHandler(LineValueChange);
                nuPaintingItemX.ValueChanged -= new System.EventHandler(LineValueChange);
                nuPaintingItemY.ValueChanged -= new System.EventHandler(LineValueChange);
                nuPaintingItemWidth.ValueChanged -= new System.EventHandler(LineValueChange);
                nuPaintingItemHeight.ValueChanged -= new System.EventHandler(LineValueChange);
                cbRectangleDirection.SelectedIndexChanged -= new System.EventHandler(LineValueChange);
                cbRectangleGroup.Items.Clear();
                nuPaintingItemX.Value = 0;
                nuPaintingItemY.Value = 0;
                nuPaintingItemWidth.Value = 0;
                nuPaintingItemHeight.Value = 0;
                nu_H_Percent.Value = 0;
                nu_W_Percent.Value = 0;
                cbRectangleDirection.Items.Clear();
                return;
            }

            var lsLineSettingbuf = new List<LineSetting_Rectangle>();
            lsLineSettingbuf.Add(new LineSetting_Rectangle(strLineSetting.Split(',').Cast<object>().ToList()));
            var lsLineSettingItem = lsLineSettingbuf[0];

            cbRectangleGroup.SelectedIndexChanged -= new System.EventHandler(LineValueChange);
            cbRectangleGroup.Items.Clear();
            cbRectangleGroup.Items.AddRange(EnumTool.GetDescriptions<enRectangle>().ToArray());
            //foreach(var varValue in EnumTool.EnumToList<enRectangle>())
            //    cbRectangleGroup.Items.Add(varValue);

            cbRectangleGroup.SelectedIndex = lsLineSettingItem.iGroup - 1;
            cbRectangleGroup.SelectedIndexChanged += new System.EventHandler(LineValueChange);

            string strKey = "Panel_Info";
            // X => m_nuPaintingItemX
            nuPaintingItemX.ValueChanged -= new System.EventHandler(LineValueChange);
            nuPaintingItemX.Maximum = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAXmax.ToString()));
            nuPaintingItemX.Minimum = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAXmin.ToString()));
            nuPaintingItemX.Value = lsLineSettingItem.iXCoordinate;
            nuPaintingItemX.ValueChanged += new System.EventHandler(LineValueChange);

            // Y => m_nuPaintingItemY
            nuPaintingItemY.ValueChanged -= new System.EventHandler(LineValueChange);
            nuPaintingItemY.Maximum = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAYmax.ToString()));
            nuPaintingItemY.Minimum = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAYmin.ToString()));
            nuPaintingItemY.Value = lsLineSettingItem.iYCoordinate;
            nuPaintingItemY.ValueChanged += new System.EventHandler(LineValueChange);

            // Width => m_nuPaintingItemWidth
            nuPaintingItemWidth.ValueChanged -= new System.EventHandler(LineValueChange);
            nuPaintingItemWidth.Maximum = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAXmax.ToString()));
            nuPaintingItemWidth.Minimum = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAXmin.ToString()));
            nuPaintingItemWidth.Value = lsLineSettingItem.iWidth;
            nuPaintingItemWidth.ValueChanged += new System.EventHandler(LineValueChange);

            // Height => m_nuPaintingItemHeight
            nuPaintingItemHeight.ValueChanged -= new System.EventHandler(LineValueChange);
            nuPaintingItemHeight.Maximum = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAYmax.ToString()));
            nuPaintingItemHeight.Minimum = Convert.ToInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue(strKey, enPanel_Info.AAYmin.ToString()));
            nuPaintingItemHeight.Value = lsLineSettingItem.iHeight;
            nuPaintingItemHeight.ValueChanged += new System.EventHandler(LineValueChange);

            decimal W_NowValue = nuPaintingItemWidth.Value;
            decimal W_MaxValue = nuPaintingItemWidth.Maximum;
            decimal _Value = ((W_NowValue / W_MaxValue) * 100);
            nu_W_Percent.Value = Math.Round(_Value, 0, MidpointRounding.AwayFromZero);
            //txt_W_Percent.Text = Math.Round(_Value, 0, MidpointRounding.AwayFromZero).ToString();

            decimal H_NowValue = nuPaintingItemHeight.Value;
            decimal H_MaxValue = nuPaintingItemHeight.Maximum;
            _Value = ((H_NowValue / H_MaxValue) * 100);
            //txt_H_Percent.Text = Math.Round(_Value, 0, MidpointRounding.AwayFromZero).ToString();
            nu_H_Percent.Value = Math.Round(_Value, 0, MidpointRounding.AwayFromZero);

            // Direction => m_cbPaintingItemDirection
            cbRectangleDirection.SelectedIndexChanged -= new System.EventHandler(LineValueChange);
            cbRectangleDirection.Items.Clear();
            if (lsLineSettingItem.DirectionRule == enRectangleDirection.BottomToTop || lsLineSettingItem.DirectionRule == enRectangleDirection.TopToBottom)
            {
                cbRectangleDirection.Items.Add(enRectangleDirection.BottomToTop.Desc());
                cbRectangleDirection.Items.Add(enRectangleDirection.TopToBottom.Desc());
            }
            else
            {
                cbRectangleDirection.Items.Add(enRectangleDirection.LeftToRight.Desc());
                cbRectangleDirection.Items.Add(enRectangleDirection.RightToLeft.Desc());
            }
            //cbRectangleDirection.Items.AddRange(EnumTool.GetDescriptions<enRectangleDirection>().ToArray());
            cbRectangleDirection.SelectedItem = lsLineSettingItem.DirectionRule.Desc();
            cbRectangleDirection.SelectedIndexChanged += new System.EventHandler(LineValueChange);
        }

        private void UpdateCrossSetting(object sender)
        {
            var lsLineSettingbuf = new LineSetting_Cross(m_PaintLineBuf.Split(',').Cast<object>().ToList());
            var tmp = new LineSetting_Cross();
            if (sender is ComboBox)
            {
                var MySender = sender as ComboBox;
                if (MySender == cbCrossLineType)
                {
                    tmp = lsLineSettingbuf;
                    tmp.NameID = MySender.Text;
                    lsLineSettingbuf = tmp;
                }
                else if (MySender == cbCrossDirection)
                {
                    tmp = lsLineSettingbuf;
                    var item = EnumTool.GetValueFromDescription<enCrossDirection>(MySender.SelectedItem.ToString());
                    tmp.DirectionRule = (enCrossDirection)Enum.Parse(typeof(enCrossDirection), item.ToString());
                    lsLineSettingbuf = tmp;
                }
            }
            else if (sender is NumericUpDown)
            {
                var MySender = sender as NumericUpDown;
                if (MySender == nuCrossDistance)
                {
                    tmp = lsLineSettingbuf;
                    tmp.iDistance = (int)MySender.Value;
                    lsLineSettingbuf = tmp;

                    decimal _NowValue = MySender.Value;
                    decimal _MaxValue = MySender.Maximum;
                    decimal _Value = ((_NowValue / _MaxValue) * 100);
                    //nuCrossDistance.Value = Math.Round(_Value, 0, MidpointRounding.AwayFromZero);
                    nuCross_Distance_Percent.Text = Math.Round(_Value, 0, MidpointRounding.AwayFromZero).ToString();
                    //txt_Cross_Distance_Percent.Text = Math.Round(_Value, 0, MidpointRounding.AwayFromZero).ToString();
                }
                else if (MySender == nuCrossStartX)
                {
                    tmp = lsLineSettingbuf;
                    tmp.StartPoint.X = (int)MySender.Value;
                    lsLineSettingbuf = tmp;
                }
                else if (MySender == nuCrossStartY)
                {
                    tmp = lsLineSettingbuf;
                    tmp.StartPoint.Y = (int)MySender.Value;
                    lsLineSettingbuf = tmp;
                }
                else if (MySender == nuCrossEndX)
                {
                    tmp = lsLineSettingbuf;
                    tmp.EndPoint.X = (int)MySender.Value;
                    lsLineSettingbuf = tmp;
                }
                else if (MySender == nuCrossEndY)
                {
                    tmp = lsLineSettingbuf;
                    tmp.EndPoint.Y = (int)MySender.Value;
                    lsLineSettingbuf = tmp;
                }
            }
            var BuffID = m_lsCrossLinebuf.FindIndex(x => x.NameID == lsLineSettingbuf.NameID);
            m_lsCrossLinebuf[BuffID] = lsLineSettingbuf;
            m_PaintLineBuf = m_lsCrossLinebuf[BuffID].ToAttribute();//取出含ID的字串
        }

        private void UpdateRectangleSetting(object sender)
        {
            var lsLineSettingbuf = new LineSetting_Rectangle(m_PaintLineBuf.Split(',').Cast<object>().ToList());
            var tmp = new LineSetting_Rectangle();

            if (sender is ComboBox)
            {
                var MySender = sender as ComboBox;
                if (MySender == cbRectangleGroup)
                {
                    tmp = lsLineSettingbuf;
                    tmp.iGroup = MySender.SelectedIndex + 1;
                    lsLineSettingbuf = tmp;
                }
                else if (MySender == cbRectangleDirection)
                {
                    tmp = lsLineSettingbuf;
                    var item = EnumTool.GetValueFromDescription<enRectangleDirection>(MySender.SelectedItem.ToString());
                    tmp.DirectionRule = (enRectangleDirection)Enum.Parse(typeof(enRectangleDirection), item.ToString());
                    lsLineSettingbuf = tmp;
                }
            }
            else if (sender is NumericUpDown)
            {
                var MySender = sender as NumericUpDown;
                if (MySender == nuPaintingItemX)
                {
                    tmp = lsLineSettingbuf;
                    tmp.iXCoordinate = (int)MySender.Value;
                    lsLineSettingbuf = tmp;
                }
                else if (MySender == nuPaintingItemY)
                {
                    tmp = lsLineSettingbuf;
                    tmp.iYCoordinate = (int)MySender.Value;
                    lsLineSettingbuf = tmp;
                }
                else if (MySender == nuPaintingItemWidth)
                {
                    tmp = lsLineSettingbuf;
                    tmp.iWidth = (int)MySender.Value;
                    lsLineSettingbuf = tmp;

                    decimal W_NowValue = MySender.Value;
                    decimal W_MaxValue = MySender.Maximum;
                    decimal _Value = ((W_NowValue / W_MaxValue) * 100);
                    nu_W_Percent.Value = Math.Round(_Value, 0, MidpointRounding.AwayFromZero);
                    //txt_W_Percent.Text = Math.Round(_Value, 0, MidpointRounding.AwayFromZero).ToString();
                }
                else if (MySender == nuPaintingItemHeight)
                {
                    tmp = lsLineSettingbuf;
                    tmp.iHeight = (int)MySender.Value;
                    lsLineSettingbuf = tmp;

                    decimal H_NowValue = MySender.Value;
                    decimal H_MaxValue = MySender.Maximum;
                    decimal _Value = ((H_NowValue / H_MaxValue) * 100);
                    nu_H_Percent.Value = Math.Round(_Value, 0, MidpointRounding.AwayFromZero);
                    //txt_H_Percent.Text = Math.Round(_Value, 0, MidpointRounding.AwayFromZero).ToString();
                }
            }

            var BuffID = lsLineSettingbuf.iID;
            m_lsRectangleLinebuf[BuffID] = lsLineSettingbuf;
            m_PaintLineBuf = m_lsRectangleLinebuf[BuffID].ToString(true);//取出含ID的字串
        }

        private void ChangecbRectangleItem(string strbuf)
        {
            List<string> lsbuff = new List<string>();
            List<LineSetting_Rectangle> lsLineSettingBuf = new List<LineSetting_Rectangle>();

            lsbuff = strbuf.Split(';').ToList();

            PaintingTreeViewReset(enPaintTestPattern.Paint_PaintRectangle);

            for (int inItemCount = 0; inItemCount < lsbuff.Count; inItemCount++)
            {
                lsbuff[inItemCount] += string.Format(",{0}", inItemCount);
                lsLineSettingBuf.Add(new LineSetting_Rectangle(lsbuff[inItemCount].Split(',').Cast<object>().ToList()));
            }

            int inGPCount = 0;
            foreach (var varValue in EnumTool.EnumToList<enRectangle>())
            {
                stuPaintingItemGp[inGPCount].lsRectLineSetting = new List<LineSetting_Rectangle>();
                stuPaintingItemGp[inGPCount].lsRectLineSetting = lsLineSettingBuf.FindAll(x => x.iGroup == (inGPCount + 1));
                var list = stuPaintingItemGp[inGPCount].lsRectLineSetting;
                foreach (var item in list)
                    stuPaintingItemGp[inGPCount].ItemTreeNode.Nodes.Add(EnumTool.GetEnumDescription<enRectangleDirection>((int)item.DirectionRule));
                inGPCount++;
            }
            PaintingTreeView.ExpandAll();
        }

        private void LineValueChange(object sender, EventArgs e)
        {
            mutex_PaintingLineValueChange.WaitOne();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
            //var SelectedTabName = EnumTool.GetValueFromDescription_DrawingTestSetting<enPaintTestPattern>(tabCtl_FingerPatternItems.SelectedTab.Text);
            var SelectedTabName = SelectedPattern(tabControl_Painting.SelectedTab);
            string sectionName = SelectedTabName.ToString();
            string strbuf = "";
            switch (SelectedTabName)
            {
                case enPaintTestPattern.Paint_PaintRectangle:
                case enPaintTestPattern.Paint_PenRectangle:
                    UpdateRectangleSetting(sender);

                    for (int inCount = 0; inCount < m_lsRectangleLinebuf.Count; inCount++)
                    {
                        strbuf += m_lsRectangleLinebuf[inCount].ToString(false);
                        if (inCount < m_lsRectangleLinebuf.Count - 1)
                            strbuf += ";";
                    }
                    if (!ChangePaintingTestSpecValue(sectionName, "LineSetting", strbuf))
                    {
                        string ErrorMSG = string.Format("[Setting Error]: [{0}] LineSetting = {1}", sectionName, strbuf);
                        MyUIPage.ShowErrorNotifier(ErrorMSG, false, 3000);
                    }

                    if (sender is ComboBox)
                        if (((ComboBox)sender) == cbRectangleGroup)
                            ChangecbRectangleItem(strbuf);
                    break;

                case enPaintTestPattern.Paint_PaintCross:
                case enPaintTestPattern.Paint_PenCross:
                    UpdateCrossSetting(sender);

                    for (int inCount = 0; inCount < m_lsCrossLinebuf.Count; inCount++)
                    {
                        strbuf += m_lsCrossLinebuf[inCount].ToAttribute();
                        if (inCount < m_lsCrossLinebuf.Count - 1)
                            strbuf += ";";
                    }
                    if (!ChangePaintingTestSpecValue(sectionName, "LineSetting", strbuf))
                    {
                        string ErrorMSG = string.Format("[Setting Error]: [{0}] LineSetting = {1}", sectionName, strbuf);
                        MyUIPage.ShowErrorNotifier(ErrorMSG, false, 3000);
                    }
                    break;
            }

            PatternReset(false, SelectedTabName);
            if (mutex_PaintingLineValueChange != null)
                mutex_PaintingLineValueChange.ReleaseMutex();
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("TabControl_SelectedIndexChanged()");
            var _tabCtl = sender as UITabControlMenu;
            if (_tabCtl.SelectedTab == null)
                return;
            _tabCtl.Enabled = false;
            m_PaintLineBuf = "";

            var SelectedTebName = SelectedPattern(tabControl_Painting.SelectedTab);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern = SelectedTebName;//預設

            MultiSenTest_VAR._SPEC_ tempSingleSpec = new MultiSenTest_VAR._SPEC_();
            foreach (var item in publicSPEC)
            {
                var ItemName = enPaintTestPattern.Paint_PaintRectangle;
                Enum.TryParse<enPaintTestPattern>(item.Name, out ItemName);
                if (ItemName == SelectedTebName)
                {
                    tempSingleSpec = item;
                    break;
                }
            }

            switch (SelectedTebName)
            {
                case enPaintTestPattern.Paint_PaintRectangle:
                case enPaintTestPattern.Paint_PenRectangle:
                    List<string> lsbuff = new List<string>();
                    string strbuf = tempSingleSpec.lsDrawingSpec.Find(x => x.Name.Contains("LineSetting")).strDefaultValue;
                    lsbuff.Clear();
                    lsbuff = strbuf.Split(';').ToList();
                    m_lsRectangleLinebuf.Clear();
                    for (int iID = 0; iID < lsbuff.Count; iID++)
                    {
                        if (lsbuff[iID] != "")
                        {
                            lsbuff[iID] += string.Format(",{0}", iID);
                            m_lsRectangleLinebuf.Add(new LineSetting_Rectangle(lsbuff[iID].Split(',').Cast<object>().ToList()));
                        }
                    }

                    break;

                case enPaintTestPattern.Paint_PaintCross:
                case enPaintTestPattern.Paint_PenCross:
                    lsbuff = new List<string>();
                    strbuf = tempSingleSpec.lsDrawingSpec.Find(x => x.Name.Contains("LineSetting")).strDefaultValue;
                    lsbuff.Clear();
                    lsbuff = strbuf.Split(';').ToList();
                    m_lsCrossLinebuf.Clear();
                    for (int iID = 0; iID < lsbuff.Count; iID++)
                    {
                        if (lsbuff[iID] != "")
                        {
                            lsbuff[iID] += string.Format(",{0}", iID);
                            m_lsCrossLinebuf.Add(new LineSetting_Cross(lsbuff[iID].Split(',').Cast<object>().ToList()));
                        }
                    }
                    break;

                default:
                    break;
            }

            //樹狀圖重繪
            PaintingTreeViewReset(SelectedTebName);
            //預覽圖重繪
            UI_GBV.fm_SensorTest_Painting.Painting_PanelReflash(false, SelectedTebName);
            splitContainer9.Panel1Collapsed = (PaintingTreeView.Nodes[(int)enPaintingNodeName.Main].Nodes.Count == 0) ? true : false;
            //樹狀圖刷新
            PaintingTreeView.SelectedNode = PaintingTreeView.Nodes[0];

            ////切換屬性的頁面
            //switch (SelectedTebName)
            //{
            //    case enPaintTestPattern.Paint_PaintCross:
            //    case enPaintTestPattern.Paint_PenCross:
            //        TestLineAttribute.SelectedTab = tpPainting_Cross;
            //        break;
            //    case enPaintTestPattern.Paint_PaintRectangle:
            //    case enPaintTestPattern.Paint_PenRectangle:
            //        TestLineAttribute.SelectedTab = tpPainting_Rectangle;
            //        break;
            //    default:
            //        TestLineAttribute.SelectedTab = null;
            //        break;
            //}
            _tabCtl.Enabled = true;

        }

        public bool PatternReset(bool bInit_PanHandle, enPaintTestPattern NowTestPattern)
        {
            bool isSuccess = bReadValueSetting(NowTestPattern);
            if (isSuccess)
                UI_GBV.fm_SensorTest_Painting.Painting_PanelReflash(bInit_PanHandle, UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern);
            	//Painting_PanelReflash(bInit_PanHandle, UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern);
            return isSuccess;
            //預覽圖重繪     
        }

        #endregion PaintTest

        private void SensorTest_Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern = enPaintTestPattern.Paint_PaintRectangle;
            panPaintingDemo.Controls.Clear();
            if (fmRawDataSetting != null)
                fmRawDataSetting.Dispose_LATool_Panel();
            tabCtl_FingerPatternItems.TabPages.Clear();
            tabCtl_PenPatternItems.TabPages.Clear();
        }

        private void SensorTest_Setting_Load(object sender, EventArgs e)
        {
            if (fmRawDataSetting != null)
                fmRawDataSetting.Dispose_LATool_Panel();
            TestLineAttribute.Appearance = TabAppearance.FlatButtons;
            TestLineAttribute.SizeMode = TabSizeMode.Fixed;
            TestLineAttribute.ItemSize = new Size(0, 1);
            TestLineAttribute.SelectedTab = null;
            cmbTrigerMode.Style = UIStyle.Orange;
            groupBox_Barcode.Style = UIStyle.Orange;
            groupBox_GPIO.Style = UIStyle.Orange;
            var PenType = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Panel_Info", enPanel_Info.PenType.ToString());
            if (string.IsNullOrEmpty(PenType))
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType = Pen_CDC_Format.enPenType.Disable;
            else
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType = EnumTool.ParseEnum<Pen_CDC_Format.enPenType>(PenType);
            tabPage_Pen.Parent = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType == Pen_CDC_Format.enPenType.Disable) ? null : tabControl_Painting;

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation != SensorTestStation.FPC)
            {
                swI2CDetect.Visible = true;  //暫時FPC Mode隱藏
                lbToolStart_I2CDetect.Visible = true;
            }
            else
            {
                swI2CDetect.Visible = false;
                lbToolStart_I2CDetect.Visible = false;
            }

            this.Text = Path.GetFileName(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.LoadTestEnable(enReadFrom.INI);

            var mySelect = EnumTool.GetValueFromDescription<TestItemNameDefine>(treeView_MainSettings.SelectedNode.Text);
            if (mySelect == TestItemNameDefine.enPaintingTest)
                tabControl_Painting_SelectedIndexChanged(this.tabControl_Painting, e);
        }

        private void SetRawData(TestItemNameDefine DefineItem, string KeyName, string ValueName, object sender, DataGridViewCellEventArgs InEvent)
        {
            if(KeyName == ValueName)
                return;
            //Console.WriteLine("KeyName:{0} ValueName:{1} RowIndex:{2} ColumnIndex:{3}", KeyName, ValueName, InEvent.RowIndex, InEvent.ColumnIndex);
            DataGridView MySender = sender as DataGridView;
            if(DefineItem == TestItemNameDefine.enPaintingTest)
            {
                EditPaintTest(DefineItem, KeyName, ValueName, sender, InEvent);
                return;
            }


            bool bStatus = true;
            int inSPECIndex = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[DefineItem].SPEC.FindIndex(x => string.Equals(x.Name, KeyName));
            MultiSenTest_VAR._SPEC_ SPECBuffer = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[DefineItem].SPEC[inSPECIndex];

            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                if(DefineItem == TestItemNameDefine.enShortTest)
                    ShowResConvert(ValueName);

            if(SPECBuffer.ShowType == enSPECShowType.DataGridViewTextBox)
            {
                string strOriValue = SPECBuffer.dtValue.Rows[0][0].ToString();
                if(string.IsNullOrEmpty(ValueName))
                {
                    ValueName = strOriValue;
                    MySender.Rows[InEvent.RowIndex].Cells[InEvent.ColumnIndex].Value = ValueName;
                    if((StaticVar.g_Lang == enLangTable.LANG_TRADITIONAL_CHINESE)
                        || (StaticVar.g_Lang == enLangTable.LANG_SIMPLIFIED_CHINESE))
                        MyUIPage.ShowInfoDialog("Warning", "[Setting Error] 數值不能為空!", UIStyle.Orange, false);
                    else
                        MyUIPage.ShowInfoDialog("Warning", "[Setting Error] Value can't be empty!", UIStyle.Orange, false);
                }

                if(SPECBuffer.dMin != SPECBuffer.dMax)
                {
                    double dValueName = 0;
                    object _OriValue = null;
                    switch(SPECBuffer.Format)
                    {
                        case enSPECFormat._Double:
                            dValueName = double.Parse(ValueName);
                            _OriValue = int.Parse(strOriValue);
                            break;

                        case enSPECFormat._Hex:
                            dValueName = (double)Convert.ToInt32(ValueName, 16);
                            _OriValue = strOriValue;
                            break;

                        default:
                            _OriValue = null;
                            break;
                    }

                    if((_OriValue != null) && ((dValueName < SPECBuffer.dMin) || (dValueName > SPECBuffer.dMax)))
                    {
                        MySender.Rows[InEvent.RowIndex].Cells[InEvent.ColumnIndex].Value = _OriValue;
                        bStatus = false;
                    }
                }

                if(bStatus)
                {
                    SPECBuffer.dtValue.Rows[0][0] = ValueName;
                }
                else
                {
                    string strMax = string.Format(SPECBuffer.strDataFormat, SPECBuffer.dMax);
                    string strMin = string.Format(SPECBuffer.strDataFormat, SPECBuffer.dMin);
                    MyUIPage.ShowInfoDialog("Warning", "[Setting Error] Set Data : " + ValueName + " Range : " + strMin + " ~ " + strMax, UIStyle.Orange, false);
                }
            }
            else if(SPECBuffer.ShowType == enSPECShowType.DataGridViewComboBox)
            {
                string strOriValue = SPECBuffer.dtValue.Rows[0][0].ToString();
                if(SPECBuffer.Format == enSPECFormat._Double)
                {
                    if(((double)Convert.ToInt32(ValueName) < SPECBuffer.dMin) || ((double)Convert.ToInt32(ValueName) > SPECBuffer.dMax))
                    {
                        MySender.Rows[InEvent.RowIndex].Cells[InEvent.ColumnIndex].Value = Convert.ToInt32(strOriValue);
                        bStatus = false;
                    }
                }

                if(bStatus)
                {
                    SPECBuffer.dtValue.Rows[0][0] = ValueName;
                    if(InEvent.ColumnIndex == 1)                    
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.TestInterface = EnumTool.ParseEnum<DEV_INTERFACE_TYPE>(ValueName);                    
                }
                else
                {
                    string strMax = string.Format(SPECBuffer.strDataFormat, SPECBuffer.dMax);
                    string strMin = string.Format(SPECBuffer.strDataFormat, SPECBuffer.dMin);
                    MyUIPage.ShowInfoDialog("Warning", "[Setting Error] Set Data : " + ValueName + " Range : " + strMin + " ~ " + strMax, UIStyle.Orange, false);
                    //MessageBox.Show("[Setting Error] Set Data : " + ValueName + " Range : " + strMin + " ~ " + strMax);
                }
            }

        }

        private void SetShortNoteMessage(DataGridView mysender)
        {
            if (mysender.CurrentRow.Index == 0)
            {
                double val = double.Parse(mysender.CurrentRow.Cells[1].Value.ToString());
                double result = 0.0;
                if (val == 0)
                {
                    richTextBox_Short_Hint.Text = "Pass: Less than Max_Threshold\n"
                        + "NG: Exceed Max_Threshold\n"
                        + "Short detection of sensor pin.\n"
                        + "Increase this value : Increase tolerance.\n"
                        + "Decrease this value: Decrease tolerance.\n";

                    //+ "R=Unlimited(KΩ)"; //20220418 Steven : 為AUO 2901暫時隱藏(Roy要求)
                }
                else
                {
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                    {
                        if (EnumTool.IsEnumDefined<OldV3_MCU>(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU))
                            result = 1333333 / val;
                        else
                            result = 2333333 / val;
                        result /= 1000.0;
                        richTextBox_Short_Hint.Text = "Pass: Less than Max_Threshold\n"
                            + "NG: Exceed Max_Threshold\n"
                            + "Short detection of sensor pin.\n"
                            + "Increase this value : Increase tolerance.\n"
                            + "Decrease this value: Decrease tolerance.\n";

                        //+ string.Format("R={0}(KΩ)\n", Math.Round(result, 2).ToString())
                        //+ "Default:7"; //20220418 Steven : 為AUO 2901暫時隱藏(Roy要求) 

                    }
                    else
                    {
                        richTextBox_Short_Hint.Text = "Pass: Less than Max_Threshold\n"
                            + "NG: Exceed Max_Threshold\n"
                            + "Short detection of sensor pin.\n"
                            + "Increase this value : Increase tolerance.\n"
                            + "Decrease this value: Decrease tolerance.\n";

                        //+ string.Format("R={0}(MΩ)\n", UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.ConvertRes(val, 0, 0))
                        //+ "Default:4000";  //20220418 Steven : 為AUO 2901暫時隱藏(Roy要求)
                    }
                }
            }
            else
            {
                richTextBox_Short_Hint.Text = "";
            }
        }

        private void ShowResConvert(string strValueName)
        {
            INI_Tool TmpINI = new INI_Tool(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            double dValueName = double.Parse(strValueName);
            double dValue = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.ConvertRes(TmpINI, "Short_Test", dValueName);
            richTextBox_Short_Hint.Text = "";
            richTextBox_Short_Hint.AppendText(string.Format("Up Limit Value = {0}\n", strValueName));
            richTextBox_Short_Hint.AppendText(string.Format("Up Limit RES = {0:00.00} M\n", dValue));
        }

        private void ShowSpecOnUI(TestItemNameDefine TestItemName)
        {
            var _TestItemName = TestItemName.Desc();
            treeView_MainSettings.Visible = false;
            string tabName = _TestItemName.Split(':')[2];
            int iTabIndex = tbSPEC_Settings.TabPages.IndexOfKey(tabName);
            if (iTabIndex == -1)
                return;

            string strNodeKey = "Test Flow";
            int inNodeIndex = Array.FindIndex(strMainNode, s => s.Equals(strNodeKey));
            string ItemName = _TestItemName.Split(':')[1];
            var _treeNode = treeView_MainSettings.Nodes[inNodeIndex].Nodes
                                .Cast<TreeNode>()
                                .Where(r => r.Text == ItemName)
                                .ToArray();
            if (_treeNode.Length == 0)
                return;

            tbSPEC_Settings.Enabled = _treeNode[0].Checked;
            tbSPEC_Settings.SelectedTab = tbSPEC_Settings.TabPages[iTabIndex];
            string TestName = TestItemName.Desc().Split(':')[1];
            delTestFuntion[TestName](TestItemName);
            treeView_MainSettings.Visible = true;

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
            {
                lb_MPResult_num.Visible = false;
                cb_station_num.Visible = false;
            }
            else
            {
                lb_MPResult_num.Visible = true;
                cb_station_num.Visible = true;
            }
            //為了讓點擊drawing test進入畫面後，tree view是跑rectangle的預設值。
            if (TestItemName == TestItemNameDefine.enPaintingTest)
                tabControl_Painting_SelectedIndexChanged(this.tabControl_Painting, null);
        }

        private bool AreBracesBalanced(string inputText)
        {
            // 定义匹配大括号的正则表达式模式，包括防范"{{"和"}}"的情况
            //string pattern = @"{(?<Open>{)|}(?(Open)(?!))";
            //string pattern = @"{(.*?[^{}]+.*?)\}|[^{}]+";
            string pattern = @"[{]|[}]";
            // 使用正则表达式进行匹配，获取大括号的数量
            MatchCollection matches = Regex.Matches(inputText, pattern);

            int openBraceCount = 0;
            int closeBraceCount = 0;

            foreach (Match match in matches)
            {
                if (match.Value == "{")
                    openBraceCount++;
                else if (match.Value == "}")
                    closeBraceCount++;
            }

            return openBraceCount == closeBraceCount;
        }

        private bool NonNumericOutsideBraces(string inputText)
        {
            string pattern = @"\{[^{}]+\}";
            string resultText = Regex.Replace(inputText, pattern, "");
            //檢查是否全部都數字
            return !Regex.IsMatch(resultText, @"\D");
        }

        private void textBox_Barcode_TextChanged(object sender, EventArgs e)
        {
            var MySender = sender as UITextBox;

            bool bResult = NonNumericOutsideBraces(MySender.Text);//檢查大括號之外有沒有非數字
            bResult &= AreBracesBalanced(MySender.Text);//檢查大括號有沒有符合規範
            uiToolTip2.Active = !bResult;
            if (!bResult)
            {
                MySender.Symbol = 61553;
                MySender.SymbolColor = Color.Red;
                MySender.FillColor = Color.MistyRose;
                textBox_BarcodeLength.Text = "0";
                IsSomeErrorSettings = true;
                if (IsSomeErrorSettings)
                {
                    ErrorSettingsReason = string.Format("Barcode format got some error.");
                    uiToolTip2.SetToolTip(MySender, ErrorSettingsReason, "Error format");
                }
                return;
            }
            else
            {
                MySender.Symbol = 61452;
                MySender.SymbolColor = Color.Green;
                MySender.FillColor = Color.White;
            }

            //取出括號外面的數字
            string removedBraces = Regex.Replace(MySender.Text, @"{[^{}]+}", ",");
            var strArray = Regex.Split(removedBraces, @"\D+");
            int iTotalLength = strArray.Where(x => x.IsNumber()).Select(x => int.Parse(x)).Sum();

            //取出括號裡面的char數量
            MatchCollection StrMatches = Regex.Matches(MySender.Text, @"\{([^{}]+)\}");
            foreach (Match match in StrMatches)
                iTotalLength += match.Groups[1].Value.ToCharArray().Length;

            textBox_BarcodeLength.Text = iTotalLength.ToString();

            IsSomeErrorSettings = (iTotalLength > 135);
            uiToolTip2.Active = IsSomeErrorSettings;
            if (IsSomeErrorSettings)
            {
                MySender.Symbol = 61553;
                MySender.SymbolColor = Color.Red;
                MySender.FillColor = Color.MistyRose;
                ErrorSettingsReason = string.Format("Barcode total length over 135, now is {0}.", iTotalLength);
                uiToolTip2.SetToolTip(MySender, ErrorSettingsReason, "Error format");
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // e.KeyChar == (Char)48 ~ 57 -----> 0~9
            // e.KeyChar == (Char)8 -----------> Backpace
            // e.KeyChar == (Char)13-----------> Enter
            switch (e.KeyChar)
            {
                case (Char)_KeyEventArg.Backpace:
                    e.Handled = false;
                    break;

                default:
                    if ((e.KeyChar >= 48) && (e.KeyChar <= 57))
                        e.Handled = false;
                    else
                        e.Handled = true;
                    break;
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            UITextBox obj = (UITextBox)sender;
            //string str = StringTool.Str_To_Unicode(obj.Text);
            string str = obj.Text;
            if (obj.Name == "textBox_Pickup")
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsFPC_Warning[0] = str;
            else if (obj.Name == "textBox_Putdown")
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsFPC_Warning[1] = str;
            else if (obj.Name == "textBox_ChangeConnect")
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsFPC_Warning[2] = str;
            else if (obj.Name == "textBox_BlinkTimer")
                int.TryParse(obj.Text, out UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.BlinkTimer);
        }

        private void timer_blink_Tick(object sender, EventArgs e)
        {
            if (btnFWPath.BackColor == Color.SeaShell)
                btnFWPath.BackColor = Color.Yellow;
            else if (btnFWPath.BackColor == Color.Yellow)
                btnFWPath.BackColor = Color.SeaShell;
        }

        private long TransformColor(string strHexCode)
        {
            long iColor = 0xFF000000;
            if (strHexCode.ToUpper().StartsWith("0X"))
                iColor |= (Convert.ToInt64(strHexCode, 16) & 0x00FFFFFF);
            else
                iColor |= (Int64.Parse(strHexCode, System.Globalization.NumberStyles.HexNumber) & 0x00FFFFFF);
            return iColor;
        }

        private void treeView_MainSettings_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                string str = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enPanel_Info);
                if (e.Node.Text.Equals(str))
                {
                    treeView_MainSettings.AfterCheck -= this.treeView_MainSettings_AfterCheck;
                    e.Node.Checked = true;
                    treeView_MainSettings.AfterCheck += this.treeView_MainSettings_AfterCheck;
                    return;
                }
                if (UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable)
                {
                    str = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enShortTest);
                    if (e.Node.Text.Equals(str))
                    {
                        treeView_MainSettings.AfterCheck -= this.treeView_MainSettings_AfterCheck;
                        e.Node.Checked = true;
                        treeView_MainSettings.AfterCheck += this.treeView_MainSettings_AfterCheck;
                        return;
                    }
                    str = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enOpenTest);
                    if (e.Node.Text.Equals(str))
                    {
                        treeView_MainSettings.AfterCheck -= this.treeView_MainSettings_AfterCheck;
                        e.Node.Checked = true;
                        treeView_MainSettings.AfterCheck += this.treeView_MainSettings_AfterCheck;
                        return;
                    }
                    str = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enUniformityTest);
                    if (e.Node.Text.Equals(str))
                    {
                        if (e.Node.Checked == false && m_ui_on_treeview)
                        {
                            if (UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.uniformity_judgement == true)
                            {
                                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.uniformity_judgement = e.Node.Checked;
                                if ((StaticVar.g_Lang == enLangTable.LANG_TRADITIONAL_CHINESE)
                                    || (StaticVar.g_Lang == enLangTable.LANG_SIMPLIFIED_CHINESE))
                                    MyUIPage.ShowInfoDialog("WARNING", "AutoGen的Uniformity Judgement 將會被取消", UIStyle.Orange, false);
                                else
                                    MyUIPage.ShowInfoDialog("WARNING", "Uniformity Judgement of AutoGen will be disabled.", UIStyle.Orange, false);
                            }
                        }
                        return;
                    }
                }

                //TODO: [Joe] Wifi 不支援 GPIO Test
                if (Wifi_Var.useWifi)
                {
                    str = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enGpio_Test);
                    if (e.Node.Text.Equals(str))
                    {
                        treeView_MainSettings.AfterCheck -= this.treeView_MainSettings_AfterCheck;
                        e.Node.Checked = false;
                        treeView_MainSettings.AfterCheck += this.treeView_MainSettings_AfterCheck;
                        return;
                    }
                }
                tbSPEC_Settings.Visible = false;
                treeView_MainSettings.SelectedNode = e.Node;
                var mySelect = EnumTool.GetValueFromDescription<TestItemNameDefine>(treeView_MainSettings.SelectedNode.Text);
                ShowSpecOnUI(mySelect);
                tbSPEC_Settings.Visible = true;
            }
            else
            {
                if (e.Node.Text == "Test Flow")
                {
                    bool boEnable = e.Node.Checked;
                    for (int iCnt = 0; iCnt < e.Node.Nodes.Count; iCnt++)
                        e.Node.Nodes[iCnt].Checked = boEnable;
                }
            }
        }

        private void treeView_MainSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView_MainSettings.SelectedNode != null)
            {
                if (treeView_MainSettings.SelectedNode.Parent != null)
                {
                    string strNodeKey = "Test Flow";
                    int inNodeIndex = Array.FindIndex(strMainNode, s => s.Equals(strNodeKey));

                    if (treeView_MainSettings.SelectedNode.Parent.Index == inNodeIndex)
                    {
                        var mySelect = EnumTool.GetValueFromDescription<TestItemNameDefine>(treeView_MainSettings.SelectedNode.Text);
                        ShowSpecOnUI(mySelect);
                        richTextBox_FW_Info.Text = "";
                    }
                }
                else
                {
                    int inNodeIndex = Array.FindIndex(strMainNode, s => s.Equals(e.Node.Text));
                    if (inNodeIndex == 1)
                        return;
                    tbSPEC_Settings.Enabled = true;
                    tbSPEC_Settings.SelectedIndex = TablePageNameBuff.FindIndex(s => s.Equals(strMainNode[inNodeIndex]));
                }
            }
        }

        private void treeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = ((e.Node.Text == "System") || (e.Node.Text == "Report")) ? true : false;
        }

        private void PaintingTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void txtBox_ReportRulePath_TextChanged(object sender, EventArgs e)
        {
            List<string> OriList = lbReportRulePath.Text.Split('\\').ToList();

            string strbuf = "";
            List<string> tmpArr = txtBox_ReportRulePath.Text.Split('}').ToList();
            foreach (string item in tmpArr)
            {
                string strTmp = item.Split(':')[0];
                if (!strTmp.Contains(".c"))
                    strbuf += strTmp.Contains("}") ? strTmp : strTmp + "}";
                else
                    strbuf += strTmp;
            }
            lbReportRulePath.Text = "";
            //foreach (var item in OriList)
            for (int index = 0; index < OriList.Count; index++)
            {
                if (index == (OriList.Count - 1))
                    lbReportRulePath.Text += strbuf;
                else
                    lbReportRulePath.Text += string.Format("{0}\\", OriList[index]);
            }
        }

        private void Update_dgvColor(int index, string ParentName, string InName, int iValue, ref List<KeyValuePair<string, _FlagArg_>> dgvColor)
        {
            _FlagArg_ FlagArg = new _FlagArg_(InName, index);
            bool bIsMainItem = (InName.Equals(ParentName));
            if ((iValue == 0) && bIsMainItem) //該項目設為不測
            {
                if (!dgvColor.Exists(x => x.Key.Contains(ParentName)))
                    dgvColor.Add(new KeyValuePair<string, _FlagArg_>(ParentName, FlagArg));
            }
            else
            {
                if (string.IsNullOrEmpty(NowParentName))
                    return;
                if (NowParentName != ParentName)
                    return;
                //移除
                if (dgvColor.Exists(x => x.Key == InName) && bIsMainItem)
                    dgvColor.RemoveAll(item => item.Key == NowParentName);
                //找出副屬項目設為不測
                else if (dgvColor.Exists(x => InName.Contains(x.Key)))
                    dgvColor.Add(new KeyValuePair<string, _FlagArg_>(ParentName, FlagArg));
            }
        }

        private void UpdatePanelInfoGrid(bool isReadPIDFromCommand, string sPIDFromCommand)
        {
            int iFound = 0;
            List<MultiSenTest_VAR._SPEC_> NowItemArr = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enPanel_Info].SPEC;

            // Set IC Type
            //string strTmp = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.strICType;//ILI2132S
            string strTmp = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU;
            strTmp = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.ILIxxxx_To_ILIxxxxS(strTmp);
            //Console.WriteLine("DevPanelInfo.MCU = {0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU);            
            iFound = NowItemArr.FindIndex(x => x.Name.Equals("IC_Type"));
            NowItemArr[iFound].dtValue.Rows[0][0] = strTmp;

            strTmp = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.TestInterface.ToString();
            iFound = NowItemArr.FindIndex(x => x.Name.Equals("Interface"));
            NowItemArr[iFound].dtValue.Rows[0][0] = strTmp;


            if (StaticVar.lsMain_USBDevInfo.Length > 0)
            {
                iFound = NowItemArr.FindIndex(x => x.Name.Equals("PID"));
                NowItemArr[iFound].dtValue.Rows[0][0] = (isReadPIDFromCommand) ? sPIDFromCommand : "0x" + StaticVar.lsMain_USBDevInfo[m_iSelectedDev].usPID.ToString("X4");
            }
        }

        private void UpdateUI_FromMainSetting()
        {
            iChangeFlag = false;
            SensorTestTool _SensorTestTool_ = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_;
            tbSystemProfilePath.Text = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath;

            linklb_ProfileVer.Text = _SensorTestTool_.GetMainSetValue("System", "ProfileVersion");
            lbSystemDate.Text = "Save Date : " + _SensorTestTool_.GetMainSetValue("System", "Save_Date");
            lbSystemToolVer.Text = "Make Tool Version : " + _SensorTestTool_.GetMainSetValue("System", "ITS_Tool_Ver");
            lbSystemICType.Text = "IC Type : " + _SensorTestTool_.GetMainSetValue("IC_Verify", "IC_Type");
            int num = Convert.ToInt16(_SensorTestTool_.GetMainSetValue("System", "Screen_Number"), 16);
            if (num == 0 || num > GetScreenCnt)
            {
                MyUIPage.ShowInfoDialog("WARNING", "The screen number was not supported, this value will be set to default(1)", UIStyle.Orange, false);
                cb_monitor_num.Text = "1";
                cb_short_monitor_num.Text = "1";
            }
            else
            {
                cb_monitor_num.Text = num.ToString();
                cb_short_monitor_num.Text = num.ToString();
            }

            chkBox_AutoRotation.Checked = StaticVar.bAutoRotationAngle;
            if (StaticVar.bAutoRotationAngle)
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.RotationAngle = StaticVar.lsAllScreenInfo.Find(x => x.ScreenNumber == num).RotationAngle;
            else
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.RotationAngle = Orientations.DEGREES_CW_0;

            string SectorName = "OP_Mode";

            cmbTrigerMode.SelectedItem = _SensorTestTool_.GetMainSetValue(SectorName, enOP_Mode.TriggerOption.ToString());

            //LCM_Fixture                                      
            string sItem = _SensorTestTool_.GetMainSetValue("OP_Mode", enOP_Mode.CheckLCMFixtureGPIOPin.ToString());
            cmbLCMDetectPin.SelectedItem = sItem.Equals(string.Empty) ? "OFF" : sItem;

            sItem = _SensorTestTool_.GetMainSetValue("OP_Mode", enOP_Mode.LCMDetectType.ToString());
            cmbLCMDetectType.SelectedItem = sItem.Equals(string.Empty) ? "HIGH" : sItem;

       
            sItem = _SensorTestTool_.GetMainSetValue("OP_Mode", enOP_Mode.CountDownNumber.ToString());
            textBox_ScanTimeout.Text = sItem.Equals(string.Empty) ? "60" : sItem;

            uiChk_CheckEachTest.Checked = _SensorTestTool_.GetMainSetValue("OP_Mode", enOP_Mode.CheckLCMFixtureEachItem.ToString()) == "True" ? true : false;

            //enOP_Mode.BarcodeWaitForTest
            foreach (var item in EnumTool.EnumToList<enOP_Mode>())
            {
                var tmpValue = _SensorTestTool_.GetMainSetValue(SectorName, item.ToString());
                bool bResult = false;
                bool.TryParse(tmpValue, out bResult);
                switch (item)
                {
                    case enOP_Mode.TestFailStop:
                        swTestFailStop.Active = bResult;
                        break;
                    case enOP_Mode.ToolStart_AutoTest:
                        swToolStart_AutoTest.Active = bResult;
                        break;
                    case enOP_Mode.TestAllDone_FailStop:
                        swTestAllDone_FailStop.Active = bResult;
                        break;
                    case enOP_Mode.TestAllDone_FailClose:
                        swTestAllDone_FailClose.Active = bResult;
                        break;
                    case enOP_Mode.TestAllDone_PassClose:
                        swTestAllDone_PassClose.Active = bResult;
                        break;
                    case enOP_Mode.TestFinishPwrDown:
                        swTestFinishPwrDown.Active = bResult;
                        break;
                    case enOP_Mode.BarcodeWaitForTest:
                        swBarcodeWaitForTest.Active = bResult;
                        break;
                    case enOP_Mode.I2CDetect:
                        swI2CDetect.Active = bResult;
                        break;
                    case enOP_Mode.ShowMPResultButton:
                        swOPModeReadMPResult.Active = bResult;
                        break;
                    case enOP_Mode.ShowTestDetail: //20220719 Steven
                        if (tmpValue != "")
                            swShowTestDetail.Active = bResult;
                        else
                            swShowTestDetail.Active = true;
                        break;
                    case enOP_Mode.ShowTestInfoViewer: //20220719 Steven
                        swShowTestInfoViewer.Active = bResult;
                        break;
                    case enOP_Mode.ShowTestErrorCode:
                        swShowTestErrorCode.Active = bResult;
                        break;
                    case enOP_Mode.ShowBarCodeScanWaterMark:
                        swShowBarcodeScanWaterMark.Active = bResult;
                        break;
                    case enOP_Mode.ShowProductionInfo:
                        swShowICProductionInfo.Active = bResult;
                        break;
                    case enOP_Mode.ShowWarningBox:
                        swShowOPHint.Active = bResult;
                        break;
                    default:
                        break;

                }
            }
            cb_SensorTestStation.SelectedItem = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation.Desc();

            SectorName = "Report";
            tbReportPath.Text = _SensorTestTool_.GetMainSetValue(SectorName, enReport.Path.ToString());
            cbAbsoluteRelativePath.SelectedItem = _SensorTestTool_.GetMainSetValue(SectorName, enReport.PathRule.ToString());
            textBox_BarcodeLength.Text = _SensorTestTool_.GetMainSetValue(SectorName, enReport.Barcode_Length.ToString());
            textBox_BarcodeSpecFormat.Text = _SensorTestTool_.GetMainSetValue(SectorName, enReport.Barcode_SpecifiedFormat.ToString());
            textBox_ScanInterval.Text = _SensorTestTool_.GetMainSetValue(SectorName, enReport.Barcode_ScanInterval.ToString());
            iFolderRuleNumber = Convert.ToInt32(_SensorTestTool_.GetMainSetValue(SectorName, enReport.ReportFolderRule.ToString()));
            iReportRuleNumber = Convert.ToInt32(_SensorTestTool_.GetMainSetValue(SectorName, enReport.ReportFileRule.ToString()));
            string sUID = _SensorTestTool_.GetMainSetValue(SectorName, enReport.UID_Display_Count.ToString());

            int iOutUID = 0;
            if (sUID != string.Empty && int.TryParse(sUID, out iOutUID) && iOutUID > 0 && iOutUID <= 8)
                iUIDDisplayCount = iOutUID;
            else
                iUIDDisplayCount = 8;

            bool bFlag = false;
            bool.TryParse(_SensorTestTool_.GetMainSetValue(SectorName, enReport.Barcode_FocusEnter.ToString()), out bFlag);
            chkBox_FocusEnter.Checked = bFlag;

            //bFlag = false;
            //bool.TryParse(_SensorTestTool_.GetMainSetValue(SectorName, enReport.MPResult_Save.ToString()), out bFlag);
            //chk_blackInfo.Checked = bFlag;

            //tb_stationName.Text = _SensorTestTool_.GetMainSetValue(SectorName, "MPResult_StationName");
            //cb_station_num.Text = _SensorTestTool_.GetMainSetValue(SectorName, "MPResult_Number");

            //20220705 Steven
            txt_CustomID.Text = _SensorTestTool_.GetMainSetValue(SectorName, "ChromeInfo_CustomerID");
            txt_FWID.Text = _SensorTestTool_.GetMainSetValue(SectorName, "ChromeInfo_FWID");

            lb_MPResult_stationname.Text = "MP Station Name:";
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
            {
                lb_MPResult_num.Visible = false;
                cb_station_num.Visible = false;
                //cb_station_num.Text = "1";
                cb_station_num.SelectedIndex = 0;
                chk_blackInfo.Enabled = false;//暫時關閉
                chk_blackInfo.Checked = false;
            }
            else
            {
                lb_MPResult_num.Visible = true;
                tb_stationName.Text = _SensorTestTool_.GetMainSetValue(SectorName, "MPResult_StationName");

                cb_station_num.Visible = true;
                var _val = _SensorTestTool_.GetMainSetValue(SectorName, "MPResult_Number");
                if (string.IsNullOrEmpty(_val))
                    cb_station_num.SelectedIndex = 0;
                else
                    cb_station_num.SelectedIndex = int.Parse(_val) - 1;

                chk_blackInfo.Enabled = true;
                bFlag = false;
                bool.TryParse(_SensorTestTool_.GetMainSetValue(SectorName, enReport.MPResult_Save.ToString()), out bFlag);
                chk_blackInfo.Checked = bFlag;
            }
            Change_ReportPathRuleSelect(iFolderRuleNumber, iReportRuleNumber);
            txtBox_ReportRulePath.TextChanged -= txtBox_ReportRulePath_TextChanged;
            Change_ReportRule(iFolderRuleNumber, iReportRuleNumber);
            txtBox_ReportRulePath.TextChanged += txtBox_ReportRulePath_TextChanged;
            num_UID_DisplayCount.Value = iUIDDisplayCount;
            iChangeFlag = true;
            //set auto gen ui
            if (_SensorTestTool_.lsMainSetting.Find(x => x.strName == "Auto_Gen.").strValue == null)
            {
                Show_AutoGen_Setting(false);
                m_ui_on_treeview = true;
            }
            else
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                {
                    Show_AutoGen_Setting(false);
                    _SensorTestTool_.m_AutoGen.m_setting.enable = false;
                }
                else
                {
                    Show_AutoGen_Setting(true);
                }
                m_ui_on_treeview = true;
            }
            uiSwitch_AutoGenEn.ActiveChanged -= chk_autogen_enable_CheckedChanged;
            uiSwitch_AutoGenEn.Active = _SensorTestTool_.m_AutoGen.m_setting.enable;
            uiSwitch_AutoGenEn.ActiveChanged += chk_autogen_enable_CheckedChanged;

            cb_autogen_path_rule.SelectedItem = _SensorTestTool_.m_AutoGen.m_setting.path_rule.Desc();
            tb_autogen_path.Text = _SensorTestTool_.GetMainSetValue("Auto_Gen.", "CollectionPath");
            uitb_autogen_profilename.Text = _SensorTestTool_.m_AutoGen.m_setting.new_ProfileName;
            if (uitb_autogen_profilename.Text == "")
                uitb_autogen_profilename.Text = "NewProfile";
            uitb_count.Text = _SensorTestTool_.m_AutoGen.m_setting.counts.ToString();
            chk_addDate.Checked = _SensorTestTool_.m_AutoGen.m_setting.addDate;
        }

        public void vfWriteProfile(string strPath)
        {
            INI_Tool DEFAULT_INI = new INI_Tool(strPath);
            bool IsHW_Key = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo == null) ? false : UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.IsHW_Key(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Mode);
            Version strINIProfile = new Version(0, 0, 0, 0);

            //=================================
            // 1. Main Setting
            List<MultiSenTest_VAR.MainSetting> tmpMainSetting = new List<MultiSenTest_VAR.MainSetting>(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.lsMainSetting);
            foreach (MultiSenTest_VAR.MainSetting item in tmpMainSetting)
            {
                if (item.strName != "Block_Setting" && item.strName != "Block_TH")
                    DEFAULT_INI.IniWriteValue(item.strName, item.strKey, item.strValue);
                //為了Daemen的相容性
                if (item.strName == "IC_Verify")
                    if (item.strKey == "Interface")
                        DEFAULT_INI.IniWriteValue("System", "Interface", item.strValue);
                //get profile version
                if (item.strName == "System" && item.strKey == "ProfileVersion")
                    strINIProfile = new Version(item.strValue);
            }

            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.LocalVariable.VarSenTest_Multi.NowStation == SensorTestStation.FPC)
            {
                DEFAULT_INI.IniWriteValue_Unicode("Fixture_Tip", "Message_PickUp", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsFPC_Warning[0]);
                DEFAULT_INI.IniWriteValue_Unicode("Fixture_Tip", "Message_Put", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsFPC_Warning[1]);
                DEFAULT_INI.IniWriteValue_Unicode("Fixture_Tip", "Message_Change", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsFPC_Warning[2]);
                DEFAULT_INI.IniWriteValue("Fixture_Tip", "Blink_Timer", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.BlinkTimer.ToString());
            }
            if (fmRawDataSetting != null)
                fmRawDataSetting.ConformCheckedBuffFunc();
            //=================================
            // 2. All Test Settings
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType == Pen_CDC_Format.enPenType.Disable)
                for (int i = 0; i < UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enPaintingTest].SPEC.Count; i++)
                    if (UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enPaintingTest].SPEC[i].Name.Contains("Pen"))
                        UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enPaintingTest].SPEC[i].dtValue.Rows[0][0] = "1"; // Disable

            Dictionary<TestItemNameDefine, MultiSenTest_VAR.TestStatus> tmpArrTestResult = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult;
            List<TestItemNameDefine> ArrTestResultKeys = new List<TestItemNameDefine>();
            ArrTestResultKeys.AddRange(tmpArrTestResult.Keys);

            foreach (var item in ArrTestResultKeys)
            {
                MultiSenTest_VAR.TestStatus NowTestitem = tmpArrTestResult[item];

                string strSection = string.Format("{0}", NowTestitem.strTestItemName);
                if (WriteTestEnable(strSection, item, ref NowTestitem, DEFAULT_INI) == false)
                    continue;
                if (item == TestItemNameDefine.enPaintingTest)
                    DEFAULT_INI.IniWriteValue(strSection, "AutoRotationAngle", StaticVar.bAutoRotationAngle);
                //======================================
                foreach (var item_2 in NowTestitem.SPEC)
                {
                    switch (item_2.ShowType)
                    {
                        case enSPECShowType.Checkbox:
                            DEFAULT_INI.IniWriteValue(strSection, item_2.Name, item_2.bTestEn.ToString());
                            break;

                        case enSPECShowType.DataGridViewComboBox:
                        case enSPECShowType.DataGridViewTextBox:
                            string strValue = item_2.dtValue.Rows[0][0].ToString();
                            if (item_2.Format == enSPECFormat._Bool)
                            {
                                int iNum = 0;
                                int.TryParse(strValue, out iNum);
                                string[] Arr = item_2.strDataFormat.Split(';');
                                strValue = Arr[iNum];
                            }

                            if (item_2.Name.Contains("Channel"))
                                break;

                            if ((strValue == "") && (item_2.Format == enSPECFormat._Double))
                            {
                                if (item_2.Name.Contains("Max_Threshold"))
                                    strValue = item_2.dMax.ToString();
                                if (item_2.Name.Contains("Min_Threshold"))
                                    strValue = item_2.dMin.ToString();
                            }
                            //DEFAULT_INI.IniWriteValue(strSection, item_2.Name, strValue);
                            DEFAULT_INI.IniWriteValueW(strSection, item_2.Name, strValue);

                            break;
                        case enSPECShowType.ComboBox:
                            DEFAULT_INI.IniWriteValue(strSection, item_2.Name, item_2.ComboBoxSelect);
                            break;
                        case enSPECShowType.DataTable:
                            break;
                    }
                }
                tmpArrTestResult[item] = NowTestitem;
            }

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation != SensorTestStation.FPC)
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3) //20220905 Steven Add
                {
                    DEFAULT_INI.IniWriteValue("Uniformity_Test", string.Format("{0}_En", Uniformity_SectorName.Uniformity_TxDiff), "False");
                    DEFAULT_INI.IniWriteValue("Uniformity_Test", string.Format("{0}_En", Uniformity_SectorName.Uniformity_RxDiff), "False");
                }

                //if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts == 0)
                if (!IsHW_Key)
                    DEFAULT_INI.IniWriteValue("Uniformity_Test", string.Format("{0}_En", Uniformity_SectorName.Uniformity_KeyRawData), "False");
            }
            //Profile預設值在V3/V6分別需要進行修改
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
            {
                DEFAULT_INI.IniWriteValue("GPIO_Test", "I2C_GPIO_0", "OFF");
                DEFAULT_INI.IniWriteValue("GPIO_Test", "I2C_GPIO_1", "OFF");
                DEFAULT_INI.IniWriteValue("GPIO_Test", "SensorID_Test", "False");
                DEFAULT_INI.IniWriteValue("Short_Test", "Frequency_Auto_PostIdle", "False");
                DEFAULT_INI.IniWriteValue("Short_Test", "Test_Count", "1");
                DEFAULT_INI.IniWriteValue("Short_Test", "Pass_Count", "1");
            }
            else
            {
                if (!UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bUseNewFrequencyScan)
                {
                    DEFAULT_INI.IniWriteValue("Short_Test", "Frequency_Auto_PostIdle", "False");
                }
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum < Ver.AP_604)
                {
                    DEFAULT_INI.IniWriteValue("GPIO_Test", "SensorID_Test", "False");
                }

                DEFAULT_INI.IniWriteValue("Short_Test", "Test_Count", "5");
                DEFAULT_INI.IniWriteValue("Short_Test", "Pass_Count", "1");
            }

            int iKeyAmounts = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts;

            DEFAULT_INI.IniWriteValue("Panel_Info", "TouchKeyChannel", IsHW_Key ? iKeyAmounts.ToString() : "0");

        }

        private IEnumerable<DataRow> GetTreeNodes(DataTable dt, TreeNode Node)
        {
            return dt.AsEnumerable()
                .Where(r => r.Field<string>(1) == Node.Name)
                .OrderBy(r => r.Field<string>(2));
        }

        private bool WriteTestEnable(string strSection, object Item, ref MultiSenTest_VAR.TestStatus NowTestitem, INI_Tool DEFAULT_INI)
        {
            string strNodeKey = "Test Flow";
            int inNodeIndex = Array.FindIndex(strMainNode, s => s.Equals(strNodeKey));
            var _treeNode = treeView_MainSettings.Nodes[inNodeIndex].Nodes.Cast<TreeNode>()
                .Where(r => r.Text == strSection).ToArray();

            if (_treeNode.Length == 0)
                return false;

            var _item = (TestItemNameDefine)Item;
            switch (_item)
            {
                //下列測項不支援FPC Mode
                case TestItemNameDefine.enUniformityTest:
                case TestItemNameDefine.enMicroOpenTest:
                    if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.LocalVariable.VarSenTest_Multi.NowStation == SensorTestStation.FPC)
                    {
                        DEFAULT_INI.ClearSector(strSection);
                        return false;
                    }
                    break;

                case TestItemNameDefine.enP2PTest://目前測項只支援V603以上
                    if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.LocalVariable.VarSenTest_Multi.NowStation == SensorTestStation.FPC)
                    {
                        DEFAULT_INI.ClearSector(strSection);
                        return false;
                    }
                    //if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum < Protocol_Ver.v603)
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum < Ver.AP_603)
                        return false;
                    if (NowTestitem.bTestitemVisible == false)
                        return false;
                    break;

                case TestItemNameDefine.enPaintingTest:
                    if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.LocalVariable.VarSenTest_Multi.NowStation == SensorTestStation.FPC)
                    {
                        DEFAULT_INI.ClearSector(strSection);
                        return false;
                    }

                    List<MultiSenTest_VAR._SPEC_> tmpSpec = new List<MultiSenTest_VAR._SPEC_>(NowTestitem.SPEC);
                    foreach (var PaintItem in Enum.GetNames(typeof(enPaintTestPattern)))
                    {
                        MultiSenTest_VAR._SPEC_ PaintSpec = tmpSpec.Find(x => x.Name.Contains(PaintItem));
                        if (PaintSpec.lsDrawingSpec == null)
                            continue;
                        foreach (MultiSenTest_VAR.LineTestSetting LineSet in PaintSpec.lsDrawingSpec)
                            DEFAULT_INI.IniWriteValue(PaintSpec.Name, LineSet.Name, LineSet.strDefaultValue);
                    }
                    break;
            }
            NowTestitem.bEnable = _treeNode[0].Checked;
            DEFAULT_INI.IniWriteValue(strSection, "Enable", NowTestitem.bEnable.ToString());
            return true;
        }

        public struct _FlagArg_
        {
            public int iRowIndex;
            public string Name;

            public _FlagArg_(string _Name, int _iRowIndex)
            {
                Name = _Name;
                iRowIndex = _iRowIndex;
            }
        }

        public struct stuPaintingItemGroup
        {
            public List<LineSetting_Rectangle> lsRectLineSetting;
            public List<LineSetting_Cross> lsCrossLineSetting;
            public TreeNode ItemTreeNode;
        }

        #region Switch Page
        private Dictionary<TestItemNameDefine, function> Page_TestFunc;

        private delegate void function(MultiSenTest_VAR.TestStatus Page_TestItem);
        private void Page_FWUpgrade(MultiSenTest_VAR.TestStatus Page_FWUpgradeItem)
        {
            lbUpgradePageName.Text = "● " + Page_FWUpgradeItem.strTestItemName;
            var lsTmp = Page_FWUpgradeItem.SPEC;
            int ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_FW_Verify.Path.ToString());
            FWupgradePath.Text = lsTmp[ItemIndex].dtValue.Rows[0][0].ToString();

            ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_FW_Verify.Force_Upgrade.ToString());
            chk_Force_Upgrade.Checked = lsTmp[ItemIndex].bTestEn;
            //chk_Force_Upgrade.Checked = ((CheckBox)lsTmp[ItemIndex]._Object).Checked;

            ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_FW_Verify.FW_CheckOnly.ToString());
            chk_FW_CheckOnly.Checked = lsTmp[ItemIndex].bTestEn;
            //chk_FW_CheckOnly.Checked = ((CheckBox)lsTmp[ItemIndex]._Object).Checked;

            string sMCUForCheck = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("IC_Verify", "IC_Type");
            lbNowIC.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.ILIxxxx_To_ILIxxxxS(sMCUForCheck);

            DataTable DTS_SPEC = new DataTable();
            DTS_SPEC.Columns.Add("Name", typeof(string));
            DTS_SPEC.Columns["Name"].ReadOnly = true;
            DTS_SPEC.Columns.Add("Value", typeof(string));

            DataRow DTS_SPEC_Row;
            for (int iCnt = 0; iCnt < lsTmp.Count; iCnt++)
            {
                if (lsTmp[iCnt].ShowType == enSPECShowType.Checkbox)
                    continue;
                if (lsTmp[iCnt].Name == INI_Name_FW_Verify.Path.ToString())
                    continue;

                if (lsTmp[iCnt].Visible != enSPECVisible.False)
                {
                    DTS_SPEC_Row = DTS_SPEC.NewRow();
                    DTS_SPEC_Row["Name"] = lsTmp[iCnt].Name;
                    if (lsTmp[iCnt].dtValue != null)
                        DTS_SPEC_Row["Value"] = lsTmp[iCnt].dtValue.Rows[0][0].ToString();
                    DTS_SPEC.Rows.Add(DTS_SPEC_Row);
                }
            }
            dgv_Upgrade.DataSource = DTS_SPEC;
        }

        private void Page_GpioTest(MultiSenTest_VAR.TestStatus Page_GpioTestItem)
        {
            label15.Text = "● " + Page_GpioTestItem.strTestItemName;
            var lsTmp = Page_GpioTestItem.SPEC;
            int ItemIndex = -1;
            groupBox_PinID.Enabled = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3) ? false : true;
            bool bIsI2C = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.isI2C(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.TestInterface);
            //groupBox_I2C.Enabled = bIsI2C;

            GroupBox_Bridge.Enabled = bIsI2C;

            //ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_INT.ToString()); //20220714
            //chk_I2C_INT_Pin_Test.Checked = groupBox_I2C.Enabled ? lsTmp[ItemIndex].bTestEn : false;
            //chk_I2C_INT_Pin_Test.Checked = bIsI2C ? lsTmp[ItemIndex].bTestEn : false;

            //ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_Reset.ToString()); //20220714
            //chk_I2C_Reset_Pin_Test.Checked = groupBox_I2C.Enabled ? lsTmp[ItemIndex].bTestEn : false;
            //chk_I2C_Reset_Pin_Test.Checked = bIsI2C ? lsTmp[ItemIndex].bTestEn : false;

            ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.UART_Test.ToString());
            chk_UART_Test.Checked = lsTmp[ItemIndex].bTestEn;

            CB_I2C_Touch_EN.Items.Clear();
            CB_I2C_GPIO_0.Items.Clear();
            CB_I2C_GPIO_1.Items.Clear();
            CB_I2C_Touch_EN_BridgePin.Items.Clear();
            CB_I2C_GPIO_0_BridgePin.Items.Clear();
            CB_I2C_GPIO_1_BridgePin.Items.Clear();
            //20220714
            CB_I2C_INT.Items.Clear();
            CB_I2C_INT_BridgePin.Items.Clear();
            CB_I2C_Reset.Items.Clear();
            CB_I2C_Reset_BridgePin.Items.Clear();

            //if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum < Protocol_Ver.v607)
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum < Ver.AP_607)
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum >= Ver.AP_604)
                {
                    ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.SensorID_Test.ToString());
                    chk_SensorID_Test.Checked = lsTmp[ItemIndex].bTestEn;
                    chk_SensorID_Test.Enabled = true;

                    ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.SensorID_BitMask.ToString());
                    txt_BitMask.Text = lsTmp[ItemIndex].dtValue.Rows[0][0].ToString();
                    txt_BitMask.Enabled = true;

                    ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.SensorID_CheckValue.ToString());
                    txt_SensorID.Text = lsTmp[ItemIndex].dtValue.Rows[0][0].ToString();
                    txt_SensorID.Enabled = true;
                }
                else
                {
                    chk_SensorID_Test.Enabled = false;
                    chk_SensorID_Test.Checked = false;
                    txt_BitMask.Enabled = false;
                    txt_SensorID.Enabled = false;
                }
                //20220726 Steven
                GroupBox_Bridge.Visible = true;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_Touch_EN.ToString());
                CB_I2C_Touch_EN.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_Touch_EN.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;
                CB_I2C_Touch_EN.Enabled = false;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_GPIO_0.ToString());
                CB_I2C_GPIO_0.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_GPIO_0.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;
                CB_I2C_GPIO_0.Enabled = false;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_GPIO_1.ToString());
                CB_I2C_GPIO_1.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_GPIO_1.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;
                CB_I2C_GPIO_1.Enabled = false;

                //20220714 Steven
                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_INT.ToString());
                CB_I2C_INT.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_INT.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_Reset.ToString());
                CB_I2C_Reset.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_Reset.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;


                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_Touch_EN_BridgePin.ToString());
                CB_I2C_Touch_EN_BridgePin.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_Touch_EN_BridgePin.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;
                CB_I2C_Touch_EN_BridgePin.Enabled = false;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_GPIO_0_BridgePin.ToString());
                CB_I2C_GPIO_0_BridgePin.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_GPIO_0_BridgePin.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;
                CB_I2C_GPIO_0_BridgePin.Enabled = false;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_GPIO_1_BridgePin.ToString());
                CB_I2C_GPIO_1_BridgePin.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_GPIO_1_BridgePin.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;
                CB_I2C_GPIO_1_BridgePin.Enabled = false;
                //20220714 Steven
                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_INT_BridgePin.ToString());
                CB_I2C_INT_BridgePin.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_INT_BridgePin.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_Reset_BridgePin.ToString());
                CB_I2C_Reset_BridgePin.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_Reset_BridgePin.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;

            }
            else
            {
                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.SensorID_Test.ToString());
                chk_SensorID_Test.Checked = lsTmp[ItemIndex].bTestEn;
                chk_SensorID_Test.Enabled = true;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.SensorID_BitMask.ToString());
                txt_BitMask.Text = lsTmp[ItemIndex].dtValue.Rows[0][0].ToString();
                txt_BitMask.Enabled = true;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.SensorID_CheckValue.ToString());
                txt_SensorID.Text = lsTmp[ItemIndex].dtValue.Rows[0][0].ToString();
                txt_SensorID.Enabled = true;

                GroupBox_Bridge.Visible = true;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_Touch_EN.ToString());
                CB_I2C_Touch_EN.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_Touch_EN.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;
                CB_I2C_Touch_EN.Enabled = true;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_GPIO_0.ToString());
                CB_I2C_GPIO_0.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_GPIO_0.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;
                CB_I2C_GPIO_0.Enabled = true;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_GPIO_1.ToString());
                CB_I2C_GPIO_1.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_GPIO_1.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;
                CB_I2C_GPIO_1.Enabled = true;
                //20220714 Steven
                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_INT.ToString());
                CB_I2C_INT.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_INT.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_Reset.ToString());
                CB_I2C_Reset.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_Reset.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;


                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_Touch_EN_BridgePin.ToString());
                CB_I2C_Touch_EN_BridgePin.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_Touch_EN_BridgePin.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;
                CB_I2C_Touch_EN_BridgePin.Enabled = true;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_GPIO_0_BridgePin.ToString());
                CB_I2C_GPIO_0_BridgePin.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_GPIO_0_BridgePin.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;
                CB_I2C_GPIO_0_BridgePin.Enabled = true;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_GPIO_1_BridgePin.ToString());
                CB_I2C_GPIO_1_BridgePin.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_GPIO_1_BridgePin.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;
                CB_I2C_GPIO_1_BridgePin.Enabled = true;
                //20220714 Steven
                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_INT_BridgePin.ToString());
                CB_I2C_INT_BridgePin.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_INT_BridgePin.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;

                ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.I2C_Reset_BridgePin.ToString());
                CB_I2C_Reset_BridgePin.Items.AddRange(lsTmp[ItemIndex].lsComBox);
                CB_I2C_Reset_BridgePin.SelectedItem = lsTmp[ItemIndex].ComboBoxSelect;

            }

            ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_GPIOTest.ID_Pin_Test.ToString());
            groupBox_PinID.Enabled = (ItemIndex == -1) ? false : true;
            if (ItemIndex != -1)
            {
                string ErrStr = "";
                var CheckNum = MyTool.HexStringToInt(lsTmp[ItemIndex].dtValue.Rows[0][0].ToString(), HEX_TYPE.BIG_ENDIAN, out ErrStr);
                var arr = groupBox_PinID.Controls.OfType<UICheckBox>().Where(c => c.Name.Contains("chk_ID_Pin"));
                for (int iShift = 0; iShift < arr.Count(); iShift++)
                {
                    var _ID_Pin_Checkbox = arr.FirstOrDefault(x => x.Name.Equals(string.Format("chk_ID_Pin{0}_Test", iShift + 1)));
                    _ID_Pin_Checkbox.Checked = (MyTool.GetIntegerSomeBit(CheckNum, iShift) == 1) ? true : false;
                }
            }
        }

        private void Page_MicroOpenTest(MultiSenTest_VAR.TestStatus Page_MicroOpen)
        {
            panel_MicroOpen.Controls.Clear();

            fmRawDataSetting = new SensorTest_RawDataSetting(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath, m_iSelectedDev);

            fmRawDataSetting.TopLevel = false;//IsMdiChild = false才能改變TopLevel
            fmRawDataSetting.TopMost = false;
            //fmRawDataSetting.Size = new System.Drawing.Size(1, 1);
            //fmRawDataSetting.Dock = DockStyle.Fill;
            Size acutalSize = fmRawDataSetting.GetFormActualSize();
            if (acutalSize.Width < panel_MicroOpen.Width && acutalSize.Height < panel_MicroOpen.Height)
                fmRawDataSetting.Dock = DockStyle.Fill;
            else
                fmRawDataSetting.Dock = DockStyle.None;

            fmRawDataSetting.FormLoad_KeyWord = SensorTest_RawDataSetting.Uniformity_Item.MicroOpen;

            panel_MicroOpen.Controls.Add(fmRawDataSetting);
            fmRawDataSetting.Parent = panel_MicroOpen;
            fmRawDataSetting.Show();
            //UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
        }

        private void Page_OpenTest(MultiSenTest_VAR.TestStatus Page_OpenTestItem)
        {
            lbRawDataPageName.Text = "● " + Page_OpenTestItem.strTestItemName;

            DataTable DTS_SPEC = new DataTable();
            DataRow DTS_SPEC_Row;
            DTS_SPEC.Columns.Add("Name", typeof(string));
            DTS_SPEC.Columns.Add("Value", typeof(double));
            DTS_SPEC.Columns["Name"].ReadOnly = true;

            for (int inCount = 0; inCount < Page_OpenTestItem.SPEC.Count; inCount++)
            {
                if (Page_OpenTestItem.SPEC[inCount].Visible == enSPECVisible.False)
                    continue;
                DTS_SPEC_Row = DTS_SPEC.NewRow();
                DTS_SPEC_Row["Name"] = Page_OpenTestItem.SPEC[inCount].Name;

                if (Page_OpenTestItem.SPEC[inCount].Format != enSPECFormat._Bool)
                    DTS_SPEC_Row["Value"] = Convert.ToDouble(Page_OpenTestItem.SPEC[inCount].dtValue.Rows[0][0].ToString());

                DTS_SPEC.Rows.Add(DTS_SPEC_Row);
            }
            dgvRawDataSPEC.DataSource = DTS_SPEC;

            foreach (DataGridViewColumn col in dgvRawDataSPEC.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void Page_P2PTest(MultiSenTest_VAR.TestStatus Page_P2PTestItem)
        {
            lbRawDataPageName.Text = "● " + Page_P2PTestItem.strTestItemName;

            DataTable DTS_SPEC = new DataTable();
            DataRow DTS_SPEC_Row;
            DTS_SPEC.Columns.Add("Name", typeof(string));
            DTS_SPEC.Columns.Add("Value", typeof(double));
            DTS_SPEC.Columns["Name"].ReadOnly = true;
            foreach (var item in Page_P2PTestItem.SPEC)
            {
                if (item.Visible == enSPECVisible.False)
                    continue;
                DTS_SPEC_Row = DTS_SPEC.NewRow();
                DTS_SPEC_Row["Name"] = item.Name;
                switch (item.Format)
                {
                    case enSPECFormat._Bool:
                        break;

                    case enSPECFormat._Double:
                        DTS_SPEC_Row["Value"] = Convert.ToDouble(item.dtValue.Rows[0][0].ToString());
                        break;

                    case enSPECFormat._Hex:
                        DTS_SPEC_Row["Value"] = Convert.ToInt32(item.dtValue.Rows[0][0].ToString(), 16);
                        break;

                    case enSPECFormat._String:
                        DTS_SPEC_Row["Value"] = item.dtValue.Rows[0][0].ToString();
                        break;
                }
                //if (item.Format != SenTest.enSPECFormat._Bool)
                //    DTS_SPEC_Row["Value"] = Convert.ToDouble(item.dtValue.Rows[0][0].ToString());

                DTS_SPEC.Rows.Add(DTS_SPEC_Row);
            }

            dgv_P2PTest_SPEC.DataSource = DTS_SPEC;

            foreach (DataGridViewColumn col in dgv_P2PTest_SPEC.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void Page_CurrentTest(MultiSenTest_VAR.TestStatus Page_CurrentTestItem)
        {
            lbRawDataPageName.Text = "● " + Page_CurrentTestItem.strTestItemName;

            DataTable DTS_SPEC = new DataTable();
            DataRow DTS_SPEC_Row;
            DTS_SPEC.Columns.Add("Name", typeof(string));
            DTS_SPEC.Columns.Add("Value", typeof(double));
            DTS_SPEC.Columns["Name"].ReadOnly = true;
            foreach (var item in Page_CurrentTestItem.SPEC)
            {
                if (item.Visible == enSPECVisible.False)
                    continue;
                DTS_SPEC_Row = DTS_SPEC.NewRow();
                DTS_SPEC_Row["Name"] = item.Name;
                switch (item.Format)
                {
                    case enSPECFormat._Bool:
                        break;

                    case enSPECFormat._Double:
                        DTS_SPEC_Row["Value"] = Convert.ToDouble(item.dtValue.Rows[0][0].ToString());
                        break;

                    case enSPECFormat._Hex:
                        DTS_SPEC_Row["Value"] = Convert.ToInt32(item.dtValue.Rows[0][0].ToString(), 16);
                        break;

                    case enSPECFormat._String:
                        DTS_SPEC_Row["Value"] = item.dtValue.Rows[0][0].ToString();
                        break;
                }
                //if (item.Format != SenTest.enSPECFormat._Bool)
                //    DTS_SPEC_Row["Value"] = Convert.ToDouble(item.dtValue.Rows[0][0].ToString());

                DTS_SPEC.Rows.Add(DTS_SPEC_Row);
            }

            dgv_CurrentTest_SPEC.DataSource = DTS_SPEC;

            foreach (DataGridViewColumn col in dgv_CurrentTest_SPEC.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        private void Page_PanelInfo(MultiSenTest_VAR.TestStatus Page_PanelInfoItem)
        {
            dgvPanelInfoSPEC.Columns.Clear();
            lbRPanelInfoPageName.Text = "● " + Page_PanelInfoItem.strTestItemName;
            List<MultiSenTest_VAR._SPEC_> NowSpec = Page_PanelInfoItem.SPEC;
            // 新增文字欄位
            dgvPanelInfoSPEC.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Name",
                ReadOnly = true
            });
            dgvPanelInfoSPEC.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Value",
                HeaderText = "Value",
            });
            string Column_1 = dgvPanelInfoSPEC.Columns[0].Name;
            string Column_2 = dgvPanelInfoSPEC.Columns[1].Name;
            int inCount = 0;
            foreach(var spec in NowSpec)
            {
                dgvPanelInfoSPEC.Rows.Add();
                string _strName = spec.Name;                
                string _strValue = spec.dtValue.Rows[0][0].ToString();
                if(spec.ShowType == enSPECShowType.DataGridViewComboBox)
                {
                    // 動態設定下拉選單內容
                    var comboBoxCell = new DataGridViewComboBoxCell();
                    string[] strCombo = spec.strDataFormat.Split(';');
                    comboBoxCell.Items.AddRange(strCombo);
                    
                    if(!strCombo.Any(x => x == _strValue))
                        comboBoxCell.Value = strCombo[0]; // 預設選擇
                    else
                        comboBoxCell.Value = _strValue; // 預設選擇                    
                    dgvPanelInfoSPEC.Rows[inCount].Cells[Column_1].Value = _strName;
                    dgvPanelInfoSPEC.Rows[inCount].Cells[Column_2] = comboBoxCell;
                }
                else
                {
                    if(spec.Name.Contains(Testitem_IC_Verify.Channel.ToString()))
                        _strValue = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.lsMainSetting.Find(x => x.strKey.Equals(spec.Name)).strValue;
                    dgvPanelInfoSPEC.Rows[inCount].Cells[Column_1].Value = _strName;
                    dgvPanelInfoSPEC.Rows[inCount].Cells[Column_2].Value = _strValue;
                }
                inCount++;
            }            
            foreach(DataGridViewColumn col in dgvPanelInfoSPEC.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            List<MultiSenTest_VAR.MainSetting> lsMainSettingBuf = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.lsMainSetting.FindAll(item => item.strName == "Panel_Info");
            //richTextBox_PanelInfo.Clear();
            richTextBox_PanelInfo.Text = "";
            for(int iCnt = 0; iCnt < lsMainSettingBuf.Count; iCnt++)
                richTextBox_PanelInfo.AppendText(lsMainSettingBuf[iCnt].strKey + " : " + lsMainSettingBuf[iCnt].strValue + "\n");
        }

        private void Page_PowerTest(MultiSenTest_VAR.TestStatus Page_PowerTest)
        {
            var lsTmp = Page_PowerTest.SPEC;
            int ItemIndex = -1;
            groupBox_LVDFlag.Enabled = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3) ? false : true;

            ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_PowerTest.VDD33_LVD_Flag_BitMask.ToString());
            groupBox_LVDFlag.Enabled = (ItemIndex == -1) ? false : true;
            if (ItemIndex != -1)
            {
                string ErrStr = "";
                var CheckNum = MyTool.HexStringToInt(lsTmp[ItemIndex].dtValue.Rows[0][0].ToString(), HEX_TYPE.BIG_ENDIAN, out ErrStr);
                var arr = groupBox_LVDFlag.Controls.OfType<UICheckBox>().Where(c => c.Name.Contains("chk_LVDFlag_Pin"));
                for (int iShift = 0; iShift < arr.Count(); iShift++)
                {
                    var _LVDDFlag_Checkbox = arr.FirstOrDefault(x => x.Name.Equals(string.Format("chk_LVDFlag_Pin{0}_Test", iShift)));
                    _LVDDFlag_Checkbox.Checked = (MyTool.GetIntegerSomeBit(CheckNum, iShift) == 1) ? true : false;
                }
            }
        }

        private DialogResult SelectColor(ref Color _SelectColor)
        {
            ColorDialog colorDialog1 = new ColorDialog();

            // Set the initial color of the dialog box
            //colorDialog1.Color = Color.Red;

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

        private void Page_ShortTest(MultiSenTest_VAR.TestStatus Page_ShortTestItem)
        {
            lbDataSection.Text = "● " + Page_ShortTestItem.strTestItemName;

            DataTable DTS_SPEC = new DataTable();
            DataRow DTS_SPEC_Row;
            DTS_SPEC.Columns.Add("Name", typeof(string));
            DTS_SPEC.Columns["Name"].ReadOnly = true;
            DTS_SPEC.Columns.Add("Value", typeof(double));
            //var chars = GBV.Globe.strProtocolVersion.ToCharArray();
            chkEnableHopping.Enabled = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6) && (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bUseNewFrequencyScan);//Protocol > 608;

            INI_Tool ini = new INI_Tool(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            num_PatternDelayTime.Value = ini.IniReadInt("Short_Test", "PatternDelayTime", 1000);

            enPatternType nowType = (enPatternType)Enum.Parse(typeof(enPatternType), ini.IniReadValue("Short_Test", "PatternType", "Close_Monitor"));
            if (Enum.IsDefined(typeof(enPatternType), nowType) == false)
                nowType = enPatternType.Close_Monitor;//
            cb_PatternType.SelectedIndex = (int)nowType;

            for (int inCount = 0; inCount < Page_ShortTestItem.SPEC.Count; inCount++)
            {
                if (Page_ShortTestItem.SPEC[inCount].Name == INI_Name_ShortTest.PatternEnable.ToString())
                {
                    chkEnablePattern.Checked = Page_ShortTestItem.SPEC[inCount].bTestEn;
                }
                if (Page_ShortTestItem.SPEC[inCount].Name == INI_Name_ShortTest.Frequency_Auto_PostIdle.ToString())
                {
                    chkEnableHopping.Checked = Page_ShortTestItem.SPEC[inCount].bTestEn;
                }

                if (Page_ShortTestItem.SPEC[inCount].Name == INI_Name_ShortTest.PatternColor.ToString())
                {
                    string sTemp = Page_ShortTestItem.SPEC[inCount].dtValue.Rows[0][0].ToString();
                    Color tempColor = System.Drawing.ColorTranslator.FromHtml(sTemp);
                    lb_PatternColor.BackColor = tempColor;

                }
                if (Page_ShortTestItem.SPEC[inCount].Visible != enSPECVisible.False)
                {
                    DTS_SPEC_Row = DTS_SPEC.NewRow();
                    DTS_SPEC_Row["Name"] = Page_ShortTestItem.SPEC[inCount].Name;
                    switch (Page_ShortTestItem.SPEC[inCount].Format)
                    {
                        case enSPECFormat._Bool:
                            DTS_SPEC_Row["Value"] = Page_ShortTestItem.SPEC[inCount].bTestEn;
                            break;

                        case enSPECFormat._Double:
                            DTS_SPEC_Row["Value"] = Convert.ToDouble(Page_ShortTestItem.SPEC[inCount].dtValue.Rows[0][0].ToString());
                            break;

                        case enSPECFormat._Hex:
                            DTS_SPEC_Row["Value"] = Convert.ToInt32(Page_ShortTestItem.SPEC[inCount].dtValue.Rows[0][0].ToString(), 16);
                            break;

                        case enSPECFormat._String:
                            DTS_SPEC_Row["Value"] = Page_ShortTestItem.SPEC[inCount].dtValue.Rows[0][0].ToString();
                            break;
                    }

                    //DTS_SPEC_Row["Value"] = Convert.ToDouble(Page_ShortTestItem.SPEC[inCount].dtValue.Rows[0][0].ToString());
                    //if (chars[0] == '6')
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                        if (string.Equals(DTS_SPEC_Row["Name"], INI_Name_ShortTest.FPC_Max_Threshold.ToString()))
                            ShowResConvert(DTS_SPEC_Row["Value"].ToString());
                    DTS_SPEC.Rows.Add(DTS_SPEC_Row);
                }
            }

            dgvShortTestSPEC.DataSource = DTS_SPEC;

            foreach (DataGridViewColumn col in dgvShortTestSPEC.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void Page_UniformityTest(MultiSenTest_VAR.TestStatus Page_UniformityItem)
        {
            panel_Uniformity.Controls.Clear();

            fmRawDataSetting = new SensorTest_RawDataSetting(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath, m_iSelectedDev);

            fmRawDataSetting.TopLevel = false;//IsMdiChild = false才能改變TopLevel
            fmRawDataSetting.TopMost = false;
            Size acutalSize = fmRawDataSetting.GetFormActualSize();
            if (acutalSize.Width < panel_Uniformity.Width && acutalSize.Height < panel_Uniformity.Height)
                fmRawDataSetting.Dock = DockStyle.Fill;
            else
                fmRawDataSetting.Dock = DockStyle.None;

            //fmRawDataSetting.Size = panel_Uniformity.Size;
            //fmRawDataSetting.Dock = DockStyle.Fill;

            //fmRawDataSetting.Dock = DockStyle.None;
            fmRawDataSetting.FormLoad_KeyWord = SensorTest_RawDataSetting.Uniformity_Item.Uniformity_RawData;

            panel_Uniformity.Controls.Add(fmRawDataSetting);
            fmRawDataSetting.Parent = panel_Uniformity;
            fmRawDataSetting.Show();
            //UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
        }


        private void Page_SRAMTest(MultiSenTest_VAR.TestStatus Page_SRAMTest)
        {

        }


        private void Page_FWFixed(MultiSenTest_VAR.TestStatus Page_FWFixed)
        {
            lbFW_FixedPageName.Text = "● " + Page_FWFixed.strTestItemName;
            var lsTmp = Page_FWFixed.SPEC;
            int ItemIndex = lsTmp.FindIndex(x => x.Name == INI_Name_FW_Fixed.Path.ToString());
            FWFixedPath.Text = lsTmp[ItemIndex].dtValue.Rows[0][0].ToString();

            DataTable DTS_SPEC = new DataTable();
            DTS_SPEC.Columns.Add("Name", typeof(string));
            DTS_SPEC.Columns["Name"].ReadOnly = true;
            DTS_SPEC.Columns.Add("Value", typeof(string));


            string sMCUForCheck = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("IC_Verify", "IC_Type");
            lbNowFWFixedIC.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.ILIxxxx_To_ILIxxxxS(sMCUForCheck);

            DataRow DTS_SPEC_Row;
            for (int iCnt = 0; iCnt < lsTmp.Count; iCnt++)
            {
                if (lsTmp[iCnt].ShowType == enSPECShowType.Checkbox)
                    continue;
                if (lsTmp[iCnt].Name == INI_Name_FW_Verify.Path.ToString())
                    continue;

                if (lsTmp[iCnt].Visible != enSPECVisible.False)
                {
                    DTS_SPEC_Row = DTS_SPEC.NewRow();
                    DTS_SPEC_Row["Name"] = lsTmp[iCnt].Name;
                    if (lsTmp[iCnt].dtValue != null)
                        DTS_SPEC_Row["Value"] = lsTmp[iCnt].dtValue.Rows[0][0].ToString();
                    DTS_SPEC.Rows.Add(DTS_SPEC_Row);
                }
            }
            dgv_FWFixed_Upgrade.DataSource = DTS_SPEC;

        }

        private void SubscribePage_TestFunc()
        {
            Page_TestFunc = new Dictionary<TestItemNameDefine, function>();
            Page_TestFunc.Add(TestItemNameDefine.enGpio_Test, Page_GpioTest);
            Page_TestFunc.Add(TestItemNameDefine.enOpenTest, Page_OpenTest);
            Page_TestFunc.Add(TestItemNameDefine.enP2PTest, Page_P2PTest);
            Page_TestFunc.Add(TestItemNameDefine.enPaintingTest, Page_PaintingTest);
            Page_TestFunc.Add(TestItemNameDefine.enPanel_Info, Page_PanelInfo);
            Page_TestFunc.Add(TestItemNameDefine.enShortTest, Page_ShortTest);
            Page_TestFunc.Add(TestItemNameDefine.enUniformityTest, Page_UniformityTest);
            Page_TestFunc.Add(TestItemNameDefine.enMicroOpenTest, Page_MicroOpenTest);
            Page_TestFunc.Add(TestItemNameDefine.enUpgrade, Page_FWUpgrade);
            Page_TestFunc.Add(TestItemNameDefine.enPowerTest, Page_PowerTest);
            Page_TestFunc.Add(TestItemNameDefine.enCurrentTest, Page_CurrentTest);
            Page_TestFunc.Add(TestItemNameDefine.enSRAMTest, Page_SRAMTest);
            Page_TestFunc.Add(TestItemNameDefine.enFWFixed, Page_FWFixed);
        }

        private void SwitchPages(TestItemNameDefine ArrIndex)
        {
            var Page_TestItem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[ArrIndex];
            Page_TestFunc[ArrIndex](Page_TestItem);
        }
        #region Painting
        private Dictionary<enPaintTestPattern, DataGridView> DataGridViewArr;

        private void SetDataGridViewStyle(enPaintTestPattern _Pattern)
        {
            DataGridViewTextBoxColumn col_1 = new DataGridViewTextBoxColumn();
            col_1.Name = "Name";
            col_1.HeaderText = "Name";
            col_1.SortMode = DataGridViewColumnSortMode.NotSortable;

            DataGridViewTextBoxColumn col_2 = new DataGridViewTextBoxColumn();
            col_2.Name = "Value";
            col_2.HeaderText = "Value";
            col_2.SortMode = DataGridViewColumnSortMode.NotSortable;

            DataGridViewArr[_Pattern].AllowUserToAddRows = false;
            DataGridViewArr[_Pattern].AllowUserToDeleteRows = false;
            DataGridViewArr[_Pattern].AllowUserToResizeRows = false;
            DataGridViewArr[_Pattern].AllowUserToResizeColumns = false;
            DataGridViewArr[_Pattern].RowHeadersVisible = false;

            DataGridViewArr[_Pattern].AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridViewArr[_Pattern].ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewArr[_Pattern].Dock = DockStyle.Fill;
            DataGridViewArr[_Pattern].EditMode = DataGridViewEditMode.EditOnF2;
            DataGridViewArr[_Pattern].RowTemplate.Height = 24;
            DataGridViewArr[_Pattern].CellBeginEdit += new DataGridViewCellCancelEventHandler(CellBeginEdit);
            DataGridViewArr[_Pattern].MouseLeave += new EventHandler(DGV_MouseLeave);
            DataGridViewArr[_Pattern].CellEndEdit += new DataGridViewCellEventHandler(CellEndEdit);
            DataGridViewArr[_Pattern].CellClick += new DataGridViewCellEventHandler(MouseClick_CellBeginEdit);
            DataGridViewArr[_Pattern].BackgroundColor = Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            DataGridViewArr[_Pattern].Columns.AddRange(new DataGridViewTextBoxColumn[] { col_1, col_2 });
        }

        private void Page_PaintingTest(MultiSenTest_VAR.TestStatus Page_PaintingTestItem)
        {
            // Init SPEC Area
            lbPaintPageName.Text = "● " + Page_PaintingTestItem.strTestItemName;

            gpPaintingItem.Visible = true;
            PaintingAttributes.Visible = true;

            tabCtl_FingerPatternItems.Style = UIStyle.Green;

            //tabCtl_PenPatternItems.TabPages.Clear();
            tabCtl_PenPatternItems.Style = UIStyle.Purple;

            List<MultiSenTest_VAR._SPEC_> tmpSPEC = Page_PaintingTestItem.SPEC;
            publicSPEC = Page_PaintingTestItem.SPEC; //20220413 : Steven Add 保存給切換畫面使用
            DataGridViewArr = new Dictionary<enPaintTestPattern, DataGridView>();

            foreach (var item in tmpSPEC)
            {
                var ItemName = enPaintTestPattern.Paint_PaintRectangle;
                Enum.TryParse<enPaintTestPattern>(item.Name, out ItemName);

                switch (ItemName)
                {
                    case enPaintTestPattern.Paint_PaintRectangle:
                    case enPaintTestPattern.Paint_PenRectangle:
                    case enPaintTestPattern.Paint_PaintRectangle3:
                    case enPaintTestPattern.Paint_PenRectangle3:
                        DataGridViewArr.Add(ItemName, new DataGridView());
                        SetDataGridViewStyle(ItemName);
                        Rectangle(item);
                        break;

                    case enPaintTestPattern.Paint_FreeDrawing:
                    case enPaintTestPattern.Paint_PenFreeDrawing:
                        DataGridViewArr.Add(ItemName, new DataGridView());
                        SetDataGridViewStyle(ItemName);
                        FreeDrawing(item);
                        break;

                    case enPaintTestPattern.Paint_PaintCross:
                    case enPaintTestPattern.Paint_PenCross:
                    case enPaintTestPattern.Paint_PaintCross3:
                    case enPaintTestPattern.Paint_PenCross3:
                        DataGridViewArr.Add(ItemName, new DataGridView());
                        SetDataGridViewStyle(ItemName);
                        Cross(item);
                        break;

                    case enPaintTestPattern.Paint_FingerDetection:
                        DataGridViewArr.Add(ItemName, new DataGridView());
                        SetDataGridViewStyle(ItemName);
                        FingerDetection(item);
                        break;

                    case enPaintTestPattern.Paint_PaintRectangle2:
                    case enPaintTestPattern.Paint_PenRectangle2:
                        DataGridViewArr.Add(ItemName, new DataGridView());
                        SetDataGridViewStyle(ItemName);
                        TrajectoryRectangle(item);
                        break;

                    case enPaintTestPattern.Paint_PaintCross2:
                    case enPaintTestPattern.Paint_PenCross2:
                        DataGridViewArr.Add(ItemName, new DataGridView());
                        SetDataGridViewStyle(ItemName);
                        TrajectoryCross(item);
                        break;

                    case enPaintTestPattern.Paint_PaintKey:
                        DataGridViewArr.Add(ItemName, new DataGridView());
                        SetDataGridViewStyle(ItemName);
                        PaintKey(item);
                        break;

                    default:
                        continue;
                }

            }

            panPaintingDemo.Controls.Clear();
            tabCtl_FingerPatternItems.SelectedIndex = 0;
            tabCtl_PenPatternItems.SelectedIndex = 0;

            UI_GBV.fm_SensorTest_Painting.SelectedDev = SelectedDev;
            UI_GBV.fm_SensorTest_Painting.TopLevel = false;
            UI_GBV.fm_SensorTest_Painting.Size = new System.Drawing.Size(1, 1);
            UI_GBV.fm_SensorTest_Painting.Dock = DockStyle.Fill;
            panPaintingDemo.Controls.Add(UI_GBV.fm_SensorTest_Painting);
            UI_GBV.fm_SensorTest_Painting.Painting_PanelReflash(true, UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern);
            UI_GBV.fm_SensorTest_Painting.Show();
        }

        private void NewDataGridViewComboBox(MultiSenTest_VAR._SPEC_ _item, DataGridView _dgv_)
        {
            DataTable tmpDT = new DataTable();
            tmpDT.Columns.Add("Name", typeof(string));
            tmpDT.Columns.Add("Value", typeof(int));
            tmpDT.Columns["Name"].ReadOnly = true;
            string[] strCombo = _item.strDataFormat.Split(';');
            for (int inComCount = 0; inComCount < strCombo.Length; inComCount++)
                tmpDT.Rows.Add(strCombo[inComCount], inComCount);

            DataGridViewComboBoxCell DGV_Combo = new DataGridViewComboBoxCell();
            DGV_Combo.DataSource = tmpDT;
            DGV_Combo.ValueMember = "Value";
            DGV_Combo.DisplayMember = "Name";

            string NowValue = _item.dtValue.Rows[0][0].ToString();
            DGV_Combo.Value = Convert.ToInt32(NowValue);

            _dgv_.Rows.Add();
            string Column_1 = _dgv_.Columns[0].Name;
            string Column_2 = _dgv_.Columns[1].Name;
            int iRowCounts = _dgv_.Rows.Count;
            _dgv_.Rows[iRowCounts - 1].Cells[Column_1].Value = "Enable";
            _dgv_.Rows[iRowCounts - 1].Cells[Column_2] = DGV_Combo;
            _dgv_.Rows[iRowCounts - 1].Cells[Column_2].Value = Convert.ToInt32(NowValue);
        }

        private void NewDataGridViewComboBox(MultiSenTest_VAR.LineTestSetting _item2, DataGridView _dgv_)
        {
            DataTable tmpDT = new DataTable();
            tmpDT.Columns.Add("Name", typeof(string));
            tmpDT.Columns.Add("Value", typeof(int));
            tmpDT.Columns["Name"].ReadOnly = true;

            List<string> strCombo = _item2.strDisplayType.Split(';').ToList();
            int inComCount = 0;
            foreach (var str in strCombo)
                tmpDT.Rows.Add(str, inComCount++);

            var DGV_Combo = new DataGridViewComboBoxCell();
            DGV_Combo.DataSource = tmpDT;
            DGV_Combo.ValueMember = "Value";
            DGV_Combo.DisplayMember = "Name";

            string NowValue = _item2.strDefaultValue;
            DGV_Combo.Value = Convert.ToInt32(strCombo.FindIndex(x => x == NowValue));
            string Column_1 = _dgv_.Columns[0].Name;
            string Column_2 = _dgv_.Columns[1].Name;
            int iRowCounts = _dgv_.Rows.Count;
            _dgv_.Rows[iRowCounts - 1].Cells[Column_1].Value = _item2.Name;
            _dgv_.Rows[iRowCounts - 1].Cells[Column_2] = DGV_Combo;
            _dgv_.Rows[iRowCounts - 1].Cells[Column_2].Value = Convert.ToInt32(strCombo.FindIndex(x => x == NowValue));
        }

        private void Cross(MultiSenTest_VAR._SPEC_ _item)
        {
            MultiSenTest_VAR._SPEC_ NowSpec = _item;
            var _NowSpecName = (enPaintTestPattern)Enum.Parse(typeof(enPaintTestPattern), NowSpec.Name);
            var tmpDgv = DataGridViewArr[_NowSpecName];
            if (_item.ShowType == enSPECShowType.DataGridViewComboBox)
                NewDataGridViewComboBox(_item, tmpDgv);

            var _Name = _NowSpecName.Desc();
            TabPage tb_Page = new TabPage(_Name);
            if (_NowSpecName == enPaintTestPattern.Paint_PenCross || _NowSpecName == enPaintTestPattern.Paint_PenCross3)
            {
                bool found = false;
                foreach (TabPage tab in tabCtl_PenPatternItems.TabPages)
                {
                    if (_Name.Equals(tab.Text))
                    {
                        found = true;
                        tb_Page = tab;
                        break;
                    }
                }
                tb_Page.Controls.Add(tmpDgv);
                if (!found)
                    tabCtl_PenPatternItems.TabPages.Add(tb_Page);
            }
            else
            {
                bool found = false;
                foreach (TabPage tab in tabCtl_FingerPatternItems.TabPages)
                {
                    if (_Name.Equals(tab.Text))
                    {
                        found = true;
                        tb_Page = tab;
                    }
                }
                tb_Page.Controls.Add(tmpDgv);
                if (!found)
                    tabCtl_FingerPatternItems.TabPages.Add(tb_Page);
            }
            //===============================================
            List<string> lsbuff = new List<string>();
            string strbuf = NowSpec.lsDrawingSpec.Find(x => x.Name.Contains("LineSetting")).strDefaultValue;
            lsbuff.Clear();
            lsbuff = strbuf.Split(';').ToList();
            m_lsCrossLinebuf.Clear();
            for (int iID = 0; iID < lsbuff.Count; iID++)
            {
                if (lsbuff[iID] != "")
                {
                    lsbuff[iID] += string.Format(",{0}", iID);
                    m_lsCrossLinebuf.Add(new LineSetting_Cross(lsbuff[iID].Split(',').Cast<object>().ToList()));
                }
            }
            PaintingTreeView.ExpandAll();
            PaintingTreeView.SelectedNode = PaintingTreeView.Nodes[0];

            PatternReset(false, enPaintTestPattern.Paint_PaintCross);

            //2. Add Value
            foreach (var item in NowSpec.lsDrawingSpec)
            {
                if (item.Visible == enSPECVisible.False)
                    continue;
                tmpDgv.Rows.Add();
                int iRowCounts = tmpDgv.RowCount;
                tmpDgv[0, iRowCounts - 1].Value = item.Name;
                tmpDgv[1, iRowCounts - 1].Value = item.strDefaultValue;
            }

            foreach (DataGridViewColumn col in tmpDgv.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            tmpDgv.ResumeLayout();
            tmpDgv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.PaintingSPEC_CellValueChanged);
        }

        private void FreeDrawing(MultiSenTest_VAR._SPEC_ _item)
        {
            MultiSenTest_VAR._SPEC_ NowSpec = _item;
            var _NowSpecName = (enPaintTestPattern)Enum.Parse(typeof(enPaintTestPattern), NowSpec.Name);
            var tmpDgv = DataGridViewArr[_NowSpecName];
            if (_item.ShowType == enSPECShowType.DataGridViewComboBox)
                NewDataGridViewComboBox(_item, tmpDgv);

            var _Name = _NowSpecName.Desc();
            TabPage tb_Page = new TabPage(_Name);
            if (_NowSpecName == enPaintTestPattern.Paint_PenFreeDrawing)
            {
                bool found = false;
                foreach (TabPage tab in tabCtl_PenPatternItems.TabPages)
                {
                    if (_Name.Equals(tab.Text))
                    {
                        found = true;
                        tb_Page = tab;
                        break;
                    }
                }
                tb_Page.Controls.Add(tmpDgv);
                if (!found)
                    tabCtl_PenPatternItems.TabPages.Add(tb_Page);
            }
            else
            {
                bool found = false;
                foreach (TabPage tab in tabCtl_FingerPatternItems.TabPages)
                {
                    if (_Name.Equals(tab.Text))
                    {
                        found = true;
                        tb_Page = tab;
                    }
                }
                tb_Page.Controls.Add(tmpDgv);
                if (!found)
                    tabCtl_FingerPatternItems.TabPages.Add(tb_Page);
            }

            //===============================================
            PatternReset(false, enPaintTestPattern.Paint_FreeDrawing);

            foreach (var item in NowSpec.lsDrawingSpec)
            {
                if (item.Visible == enSPECVisible.False)
                    continue;

                tmpDgv.Rows.Add();
                if (item.ShowType == enSPECShowType.DataGridViewComboBox)
                {
                    NewDataGridViewComboBox(item, tmpDgv);
                }
                else
                {
                    int iRowCounts = tmpDgv.RowCount;
                    tmpDgv[0, iRowCounts - 1].Value = item.Name;
                    tmpDgv[1, iRowCounts - 1].Value = item.strDefaultValue;
                }

            }

            foreach (DataGridViewColumn col in tmpDgv.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            tmpDgv.ResumeLayout();
            tmpDgv.CellValueChanged += new DataGridViewCellEventHandler(this.PaintingSPEC_CellValueChanged);
        }

        private void Rectangle(MultiSenTest_VAR._SPEC_ _item)
        {
            int DgvIndex = (int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern;

            MultiSenTest_VAR._SPEC_ NowSpec = _item;
            var _NowSpecName = (enPaintTestPattern)Enum.Parse(typeof(enPaintTestPattern), NowSpec.Name);
            var tmpDgv = DataGridViewArr[_NowSpecName];
            if (_item.ShowType == enSPECShowType.DataGridViewComboBox)
                NewDataGridViewComboBox(_item, tmpDgv);

            var _Name = _NowSpecName.Desc();
            TabPage tb_Page = new TabPage(_Name);


            if (_NowSpecName == enPaintTestPattern.Paint_PenRectangle || _NowSpecName == enPaintTestPattern.Paint_PenRectangle3)
            {
                bool found = false;
                foreach (TabPage tab in tabCtl_PenPatternItems.TabPages)
                {
                    if (_Name.Equals(tab.Text))
                    {
                        found = true;
                        tb_Page = tab;
                        break;
                    }
                }
                tb_Page.Controls.Add(tmpDgv);
                if (!found)
                    tabCtl_PenPatternItems.TabPages.Add(tb_Page);
            }
            else
            {
                bool found = false;
                foreach (TabPage tab in tabCtl_FingerPatternItems.TabPages)
                {
                    if (_Name.Equals(tab.Text))
                    {
                        found = true;
                        tb_Page = tab;
                    }
                }
                tb_Page.Controls.Add(tmpDgv);
                if (!found)
                    tabCtl_FingerPatternItems.TabPages.Add(tb_Page);
            }

            //===============================================
            // PaintingTreeView
            //===============================================
            List<string> lsbuff = new List<string>();
            string strbuf = NowSpec.lsDrawingSpec.Find(x => x.Name.Contains("LineSetting")).strDefaultValue;
            lsbuff.Clear();
            lsbuff = strbuf.Split(';').ToList();
            m_lsRectangleLinebuf.Clear();
            for (int iID = 0; iID < lsbuff.Count; iID++)
            {
                if (lsbuff[iID] != "")
                {
                    lsbuff[iID] += string.Format(",{0}", iID);
                    m_lsRectangleLinebuf.Add(new LineSetting_Rectangle(lsbuff[iID].Split(',').Cast<object>().ToList()));
                }
            }
            PaintingTreeView.ExpandAll();
            PaintingTreeView.SelectedNode = PaintingTreeView.Nodes[0];

            PatternReset(false, _NowSpecName);
            //TODO: 
            //1. dynamic New dgv Painting SPEC
            //2. Add Value
            foreach (var item in NowSpec.lsDrawingSpec)
            {
                if (item.Visible == enSPECVisible.False)
                    continue;
                tmpDgv.Rows.Add();
                if (item.ShowType == enSPECShowType.DataGridViewComboBox)
                {
                    NewDataGridViewComboBox(item, tmpDgv);
                }
                else
                {
                    int iRowCounts = tmpDgv.RowCount;
                    if ((_NowSpecName == enPaintTestPattern.Paint_PaintRectangle3 || _NowSpecName == enPaintTestPattern.Paint_PenRectangle3)
                        && item.Name == "LineGap")
                    {
                        DataGridViewArr[_NowSpecName].Rows.RemoveAt(iRowCounts - 1);
                        continue;
                    }

                    DataGridViewArr[_NowSpecName][0, iRowCounts - 1].Value = item.Name;
                    DataGridViewArr[_NowSpecName][1, iRowCounts - 1].Value = item.strDefaultValue;
                }
            }

            foreach (DataGridViewColumn col in DataGridViewArr[_NowSpecName].Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            DataGridViewArr[_NowSpecName].ResumeLayout();
            DataGridViewArr[_NowSpecName].CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.PaintingSPEC_CellValueChanged);
        }

        private void FingerDetection(MultiSenTest_VAR._SPEC_ _item)
        {
            int DgvIndex = (int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern;

            MultiSenTest_VAR._SPEC_ NowSpec = _item;
            var _NowSpecName = (enPaintTestPattern)Enum.Parse(typeof(enPaintTestPattern), NowSpec.Name);
            var tmpDgv = DataGridViewArr[_NowSpecName];
            if (_item.ShowType == enSPECShowType.DataGridViewComboBox)
                NewDataGridViewComboBox(_item, tmpDgv);

            var _Name = _NowSpecName.Desc();
            TabPage tb_Page = new TabPage(_Name);
            bool found = false;
            foreach (TabPage tab in tabCtl_FingerPatternItems.TabPages)
            {
                if (_Name.Equals(tab.Text))
                {
                    found = true;
                    tb_Page = tab;
                    break;
                }
            }
            tb_Page.Controls.Add(tmpDgv);
            if (!found)
                tabCtl_FingerPatternItems.TabPages.Add(tb_Page);

            //===============================================
            PatternReset(false, enPaintTestPattern.Paint_FingerDetection);

            foreach (var item in NowSpec.lsDrawingSpec)
            {
                if (item.Visible == enSPECVisible.False)
                    continue;
                tmpDgv.Rows.Add();
                if (item.ShowType == enSPECShowType.DataGridViewComboBox)
                {
                    NewDataGridViewComboBox(item, tmpDgv);
                }
                else
                {
                    int iRowCounts = tmpDgv.RowCount;
                    tmpDgv[0, iRowCounts - 1].Value = item.Name;
                    tmpDgv[1, iRowCounts - 1].Value = item.strDefaultValue;
                }

            }

            foreach (DataGridViewColumn col in tmpDgv.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            tmpDgv.ResumeLayout();
            tmpDgv.CellValueChanged += new DataGridViewCellEventHandler(this.PaintingSPEC_CellValueChanged);
        }

        private void TrajectoryRectangle(MultiSenTest_VAR._SPEC_ _item)
        {
            int DgvIndex = (int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern;

            MultiSenTest_VAR._SPEC_ NowSpec = _item;
            var _NowSpecName = (enPaintTestPattern)Enum.Parse(typeof(enPaintTestPattern), NowSpec.Name);
            var tmpDgv = DataGridViewArr[_NowSpecName];
            if (_item.ShowType == enSPECShowType.DataGridViewComboBox)
                NewDataGridViewComboBox(_item, tmpDgv);

            var _Name = _NowSpecName.Desc();
            TabPage tb_Page = new TabPage(_Name);

            if (_NowSpecName == enPaintTestPattern.Paint_PenRectangle2)
            {
                bool found = false;
                foreach (TabPage tab in tabCtl_PenPatternItems.TabPages)
                {
                    if (_Name.Equals(tab.Text))
                    {
                        found = true;
                        tb_Page = tab;
                        break;
                    }
                }
                tb_Page.Controls.Add(tmpDgv);
                if (!found)
                    tabCtl_PenPatternItems.TabPages.Add(tb_Page);
            }
            else
            {
                bool found = false;
                foreach (TabPage tab in tabCtl_FingerPatternItems.TabPages)
                {
                    if (_Name.Equals(tab.Text))
                    {
                        found = true;
                        tb_Page = tab;
                    }
                }
                tb_Page.Controls.Add(tmpDgv);
                if (!found)
                    tabCtl_FingerPatternItems.TabPages.Add(tb_Page);
            }

            PatternReset(false, enPaintTestPattern.Paint_PaintRectangle2);

            foreach (var item in NowSpec.lsDrawingSpec)
            {
                if (item.Visible == enSPECVisible.False)
                    continue;
                tmpDgv.Rows.Add();
                if (item.ShowType == enSPECShowType.DataGridViewComboBox)
                {
                    NewDataGridViewComboBox(item, tmpDgv);
                }
                else
                {
                    int iRowCounts = tmpDgv.RowCount;
                    tmpDgv[0, iRowCounts - 1].Value = item.Name;
                    tmpDgv[1, iRowCounts - 1].Value = item.strDefaultValue;
                }
            }

            foreach (DataGridViewColumn col in tmpDgv.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            tmpDgv.ResumeLayout();
            tmpDgv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.PaintingSPEC_CellValueChanged);
        }

        private void TrajectoryCross(MultiSenTest_VAR._SPEC_ _item)
        {
            int DgvIndex = (int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern;

            MultiSenTest_VAR._SPEC_ NowSpec = _item;
            var _NowSpecName = (enPaintTestPattern)Enum.Parse(typeof(enPaintTestPattern), NowSpec.Name);
            var tmpDgv = DataGridViewArr[_NowSpecName];
            if (_item.ShowType == enSPECShowType.DataGridViewComboBox)
                NewDataGridViewComboBox(_item, tmpDgv);

            var _Name = _NowSpecName.Desc();
            TabPage tb_Page = new TabPage(_Name);
            if (_NowSpecName == enPaintTestPattern.Paint_PenCross2)
            {
                bool found = false;
                foreach (TabPage tab in tabCtl_PenPatternItems.TabPages)
                {
                    if (_Name.Equals(tab.Text))
                    {
                        found = true;
                        tb_Page = tab;
                        break;
                    }
                }
                tb_Page.Controls.Add(tmpDgv);
                if (!found)
                    tabCtl_PenPatternItems.TabPages.Add(tb_Page);
            }
            else
            {
                bool found = false;
                foreach (TabPage tab in tabCtl_FingerPatternItems.TabPages)
                {
                    if (_Name.Equals(tab.Text))
                    {
                        found = true;
                        tb_Page = tab;
                    }
                }
                tb_Page.Controls.Add(tmpDgv);
                if (!found)
                    tabCtl_FingerPatternItems.TabPages.Add(tb_Page);
            }
            //if (_NowSpecName == enPaintTestPattern.Paint_PenTrajectoryCross)
            //{
            //    if (!DataGridViewArr.ContainsKey(_NowSpecName))
            //        tabCtl_PenPatternItems.TabPages.Add(tb_Page);
            //}
            //else
            //{
            //    if (!DataGridViewArr.ContainsKey(_NowSpecName))
            //        tabCtl_FingerPatternItems.TabPages.Add(tb_Page);
            //}
            //tb_Page.Controls.Add(DataGridViewArr[_NowSpecName]);

            PatternReset(false, enPaintTestPattern.Paint_PaintCross2);

            foreach (var item in NowSpec.lsDrawingSpec)
            {
                if (item.Visible == enSPECVisible.False)
                    continue;
                tmpDgv.Rows.Add();
                if (item.ShowType == enSPECShowType.DataGridViewComboBox)
                {
                    NewDataGridViewComboBox(item, tmpDgv);
                }
                else
                {
                    int iRowCounts = tmpDgv.RowCount;
                    tmpDgv[0, iRowCounts - 1].Value = item.Name;
                    tmpDgv[1, iRowCounts - 1].Value = item.strDefaultValue;
                }
            }

            foreach (DataGridViewColumn col in tmpDgv.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            tmpDgv.ResumeLayout();
            tmpDgv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.PaintingSPEC_CellValueChanged);
        }

        private void PaintKey(MultiSenTest_VAR._SPEC_ _item)
        {
            int DgvIndex = (int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern;

            MultiSenTest_VAR._SPEC_ NowSpec = _item;
            var _NowSpecName = (enPaintTestPattern)Enum.Parse(typeof(enPaintTestPattern), NowSpec.Name);
            var tmpDgv = DataGridViewArr[_NowSpecName];
            if (_item.ShowType == enSPECShowType.DataGridViewComboBox)
                NewDataGridViewComboBox(_item, tmpDgv);

            var _Name = _NowSpecName.Desc();
            TabPage tb_Page = new TabPage(_Name);

            bool found = false;
            foreach (TabPage tab in tabCtl_FingerPatternItems.TabPages)
            {
                if (_Name.Equals(tab.Text))
                {
                    found = true;
                    tb_Page = tab;
                    break;
                }
            }
            tb_Page.Controls.Add(tmpDgv);
            if (!found)
                tabCtl_FingerPatternItems.TabPages.Add(tb_Page);

            // PatternReset(Key);
            PatternReset(false, enPaintTestPattern.Paint_PaintKey);

            foreach (var item in NowSpec.lsDrawingSpec)
            {
                if (item.Visible == enSPECVisible.False)
                    continue;
                tmpDgv.Rows.Add();
                if (item.ShowType == enSPECShowType.DataGridViewComboBox)
                {
                    NewDataGridViewComboBox(item, tmpDgv);
                }
                else
                {
                    int iRowCounts = tmpDgv.RowCount;
                    tmpDgv[0, iRowCounts - 1].Value = item.Name;
                    tmpDgv[1, iRowCounts - 1].Value = item.strDefaultValue;
                }
            }

            foreach (DataGridViewColumn col in tmpDgv.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            tmpDgv.ResumeLayout();
            tmpDgv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.PaintingSPEC_CellValueChanged);
        }

        #endregion Painting

        #endregion Switch Page

        #region ToolStripMenuItem Click

        private void Set_UISettingToMainSetting()
        {
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.UpdateProtocolInfo();

            /*if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation != SensorTestStation.FPC)
                fmRawDataSetting.SaveMicroOpen_RawData(); //為了儲存TX_AvoidCorner的值*/
            //使用m_LATool來儲存spec

            #region Short
            MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enShortTest];

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_ShortTest.PatternEnable.ToString(), chkEnablePattern.Checked);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_ShortTest.PatternColor.ToString(), string.Format("#{0}{1}{2}", lb_PatternColor.BackColor.R.ToString("X2"), lb_PatternColor.BackColor.G.ToString("X2"), lb_PatternColor.BackColor.B.ToString("X2")));
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_ShortTest.Frequency_Auto_PostIdle.ToString(), chkEnableHopping.Checked);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_ShortTest.PatternDelayTime.ToString(), num_PatternDelayTime.Value.ToString());
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_ShortTest.PatternType.ToString(), ((enPatternType)cb_PatternType.SelectedIndex).ToString());
            #endregion

            #region OP Mode Section

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.TriggerOption, cmbTrigerMode.SelectedItem);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.SensorTestStation, cb_SensorTestStation.SelectedItem);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.TestFailStop, swTestFailStop.Active);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.ToolStart_AutoTest, swToolStart_AutoTest.Active);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.TestAllDone_FailStop, swTestAllDone_FailStop.Active);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.TestAllDone_FailClose, swTestAllDone_FailClose.Active);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.TestAllDone_PassClose, swTestAllDone_PassClose.Active);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.TestFinishPwrDown, swTestFinishPwrDown.Active);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.BarcodeWaitForTest, swBarcodeWaitForTest.Active);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.I2CDetect, swI2CDetect.Active);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.ShowMPResultButton, swOPModeReadMPResult.Active);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.ShowTestDetail, swShowTestDetail.Active); //20220719 Steven
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.ShowTestInfoViewer, swShowTestInfoViewer.Active); //20220725 Steven 
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.ShowTestErrorCode, swShowTestErrorCode.Active); //20221124 Steven
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.ShowBarCodeScanWaterMark, swShowBarcodeScanWaterMark.Active); //20230309 Steven
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.ShowProductionInfo, swShowICProductionInfo.Active); //20231205 Steven
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.ShowWarningBox, swShowOPHint.Active); //20231205 Steven

            if (string.Equals(cmbTrigerMode.SelectedItem.ToString(), TriggerMode.GPIO.Desc()))
            {
                /* Check validity of bridge gpio setting */
                if ((cmb_TS2_usage.SelectedItem.ToString() != enBridgeGpioUsage.unused.Desc() &&
                        cmb_TS2_usage.SelectedItem.ToString() == cmb_TS1_usage.SelectedItem.ToString()) ||
                    (cmb_TS2_usage.SelectedItem.ToString() != enBridgeGpioUsage.unused.Desc() &&
                        cmb_TS2_usage.SelectedItem.ToString() == cmb_PN0_usage.SelectedItem.ToString()) ||
                    (cmb_TS1_usage.SelectedItem.ToString() != enBridgeGpioUsage.unused.Desc() &&
                        cmb_TS1_usage.SelectedItem.ToString() == cmb_PN0_usage.SelectedItem.ToString()))
                {
                    MyUIPage.ShowWarningNotifier("[WARN] GPIO Trigger Mode: single usage set for multiple GPIO pin", false, 10000);
                }

                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioTS2,
                    string.Format("{0},{1}", cmb_TS2_usage.SelectedItem.ToString(), cmb_TS2_trigger.SelectedItem.ToString()));
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioTS1,
                    string.Format("{0},{1}", cmb_TS1_usage.SelectedItem.ToString(), cmb_TS1_trigger.SelectedItem.ToString()));
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioPN0,
                    string.Format("{0},{1}", cmb_PN0_usage.SelectedItem.ToString(), cmb_PN0_trigger.SelectedItem.ToString()));

                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioPassDuration
                    , string.IsNullOrEmpty(uiTextBox_PassDuration.Text) ? "0" : uiTextBox_PassDuration.Text);
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioFailDuration
                    , string.IsNullOrEmpty(uiTextBox_FailDuration.Text) ? "0" : uiTextBox_FailDuration.Text);
            }
            //LCM_Fixture_Setting
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.EnableFixtureCheck,
              cmbLCMDetectPin.SelectedItem.ToString() == "OFF" ? "False" : "True");

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.CheckLCMFixtureGPIOPin,
                   cmbLCMDetectPin.SelectedItem.ToString());
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.CountDownNumber,
                   textBox_ScanTimeout.Text);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.LCMDetectType,
                   cmbLCMDetectType.SelectedItem.ToString());
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.CheckLCMFixtureEachItem,
                uiChk_CheckEachTest.Checked ? "True" : "False");

            StaticVar.bSensorTestShowTestDetail = swShowTestDetail.Active;
            StaticVar.bSensorTestInfoViewerVisible = swShowTestInfoViewer.Active; //20220725 Steven
            StaticVar.bShowTestErrorCode = swShowTestErrorCode.Active; //20221124 Steven
            StaticVar.bShowBarCodeScanWaterMark = swShowBarcodeScanWaterMark.Active;
            StaticVar.bShowWarningBox = swShowOPHint.Active;

            fmSenTest_Detail.btn_ShowMPResult.Visible = swOPModeReadMPResult.Active;
            fmSenTest_Detail.ChangeTestInfoViewerVisible(swShowTestInfoViewer.Active); //20220725 Steven
            fmSenTest_Detail.ShowBarCodeScanWaterMark(StaticVar.bShowBarCodeScanWaterMark); //20230309 Steven

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation != SensorTestStation.FPC)
            {
                if (uiSwitch_AutoGenEn.Active)
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation = SensorTestStation.AUTOGEN;
                else
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation = SensorTestStation.MODULE;
            }
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("OP_Mode", enOP_Mode.SensorTestStation
                , UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation.Desc());

            #endregion

            #region Auto Gen Section
            Version strINIProfile = new Version(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.lsMainSetting.Find(x => x.strKey == "ProfileVersion").strValue);
            if (strINIProfile >= new Version(1, 0, 3, 4))
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("Auto_Gen.", enAuto_Gen.Enable, uiSwitch_AutoGenEn.Active);
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("Auto_Gen.", enAuto_Gen.PathRule, cb_autogen_path_rule.SelectedItem.ToString());
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("Auto_Gen.", enAuto_Gen.CollectionPath, tb_autogen_path.Text);
                if (!uitb_autogen_profilename.Text.Contains(".ini"))
                {
                    if (uitb_autogen_profilename.Text != "")
                        uitb_autogen_profilename.Text = uitb_autogen_profilename.Text + ".ini";
                }
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("Auto_Gen.", enAuto_Gen.NewProfileName, uitb_autogen_profilename.Text);
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("Auto_Gen.", enAuto_Gen.Count, uitb_count.Text);
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("Auto_Gen.", enAuto_Gen.Uniformity_Judgement
                    , UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.uniformity_judgement);
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("Auto_Gen.", enAuto_Gen.AddDate_toNewProfileName, chk_addDate.Checked);

            }
            #endregion

            #region Report Section
            string SectorName = "Report";
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.Path, tbReportPath.Text);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.PathRule, cbAbsoluteRelativePath.SelectedItem.ToString());
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.ReportFolderRule, iFolderRuleNumber.ToString());
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.ReportFileRule, iReportRuleNumber.ToString());
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.ReportFileFormat, txtBox_ReportRulePath.Text);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.Barcode_Length, textBox_BarcodeLength.Text);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.Barcode_ScanInterval, textBox_ScanInterval.Text);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.Barcode_SpecifiedFormat, textBox_BarcodeSpecFormat.Text);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.Barcode_FocusEnter, chkBox_FocusEnter.Checked.ToString());
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.MPResult_Save, chk_blackInfo.Checked.ToString());
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.MPResult_StationName, tb_stationName.Text);
            //20220705 Steven
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.ChromeInfo_CustomerID, txt_CustomID.Text);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.ChromeInfo_FWID, txt_FWID.Text);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.UID_Display_Count, num_UID_DisplayCount.Value.ToString());

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.MPResult_Number, cb_station_num.SelectedIndex + 1);
            else
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.MPResult_Number, "");
            //if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            //    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.MPResult_Number, cb_station_num.Text);
            //else
            //    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, enReport.MPResult_Number, "1");
            #endregion

            #region GPIO
            NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enGpio_Test];
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_GPIOTest.SensorID_Test.ToString(), chk_SensorID_Test.Checked);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_GPIOTest.SensorID_BitMask.ToString(), txt_BitMask.Text);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_GPIOTest.SensorID_CheckValue.ToString(), txt_SensorID.Text);
            #endregion

        }

        private void ToolStripMenuItem_CloseProfile_Click(object sender, EventArgs e)
        {
            if (fmRawDataSetting != null)
                fmRawDataSetting.Dispose_LATool_Panel();
            this.Close();
        }

        private void ToolStripMenuItem_LoadProfile_Click(object sender, EventArgs e)
        {
            if (fmRawDataSetting != null)
                fmRawDataSetting.Dispose_LATool_Panel();

            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Profile (*.ini)|*.ini";
            openDlg.InitialDirectory = ITS_Directory.Tool_CurrentDirectory + "Profile";
            openDlg.FileName = "";
            openDlg.CheckFileExists = true;    //如果當使用者指定不存在的檔名時對話方塊顯示警告訊息，屬性值為 true
            openDlg.CheckPathExists = true;    //如果當使用者指定了不存在的路徑時要顯示警告對話方塊，屬性值為 true
            openDlg.Multiselect = false;              //如果對話方塊允許一起或同時選取多個檔案，則為 true
            if (openDlg.ShowDialog(this) != DialogResult.OK)
                return;

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath = openDlg.FileName;
            //刷新Protocol & Tool
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation != SensorTestStation.FPC)
                if (fmSenTest_Detail.Check_and_Update_Profile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath) == false)
                    return;
            string strProtocol = "";
            //UI_GBV.fmITS_Tool.bReadProtocol(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath, out strProtocol);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.bReadProtocol(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath, out strProtocol, out UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU);
            UI_GBV.fmITS_Tool.ResetProtocol(m_iSelectedDev, strProtocol);

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ResetAllTestContains(enReadFrom.INI);

            INI_Tool HistoryINI = new INI_Tool(ITS_Directory.History_INI);
            string RelativePath = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            HistoryINI.IniWriteValueW("SensorTest_ProfilePath", "Path", RelativePath);
            //HistoryINI.IniWriteValue("SensorTest_ProfilePath", "Path", SenTest.strProfileAbsolutePath);
            tbSystemProfilePath.Text = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath;

            //if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
            if (fmSenTest_Multi == null)
            {
                fmSenTest_Detail.TestItem_GridInitial();
                fmSenTest_Detail.TriggerOptionChange();
            }
            else
            {
                for (int iDev = 0; iDev < StaticVar.iMaxSupportDev; iDev++)
                    fmSenTest_Multi.TestSequenceInit(iDev);
            }
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
            this.Close();
        }

        private bool bCheckSettingsIsCorrect
        {
            get
            {
                bool bResult = (!this.Enabled || IsSomeErrorSettings);
                if (bResult)
                    MyUIPage.ShowErrorDialog("ERROR Settings", ErrorSettingsReason, UIStyle.Red, true);

                return bResult;
            }
        }
        private void ToolStripMenuItem_SaveAsProfile_Click(object sender, EventArgs e)
        {
            if (bCheckSettingsIsCorrect)
                return;

            if (fmRawDataSetting != null)
                fmRawDataSetting.Dispose_LATool_Panel();
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "ini File|*.ini";
            dlg.Title = "Save As";

            if (UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable)
                Enable_Uniformity_Test();
            // If the file name is not an empty string open it for saving.
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && dlg.FileName != "")
            {
                Set_UISettingToMainSetting();

                //string strPath = Tool.GetAbsoluteToRelativePath(StaticVar.ITS_Directory.Tool_CurrentDirectory + @"\", dlg.FileName);
                string strPath = dlg.FileName;
                if (String.Compare(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath, strPath) == 0)
                {
                    if ((StaticVar.g_Lang == enLangTable.LANG_TRADITIONAL_CHINESE) || (StaticVar.g_Lang == enLangTable.LANG_SIMPLIFIED_CHINESE))
                        MyUIPage.ShowErrorNotifier("profile正在被使，請選擇其他profile", false, 3000);
                    else
                        MyUIPage.ShowErrorNotifier("This file is in using, please select another file.", false, 3000);
                    return;
                }
                CopyProfileINI(Path.GetFullPath(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath), dlg.FileName);

                //write real file
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath = strPath;
                vfWriteProfile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                INI_Tool HistoryINI = new INI_Tool(ITS_Directory.History_INI);
                string RelativePath = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                HistoryINI.IniWriteValueW("SensorTest_ProfilePath", "Path", RelativePath);

                tbSystemProfilePath.Text = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath;
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.vfLoadProfile(enReadFrom.INI, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                Initial_treeView(TestItemNameDefine.enUpgrade);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
                MyUIPage.ShowSuccessNotifier("Save Success !");
            }
        }

        private void ToolStripMenuItem_SaveCloseProfile_Click(object sender, EventArgs e)
        {
            if (bCheckSettingsIsCorrect)
                return;
            if (UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable)
                Enable_Uniformity_Test();
            if (fmRawDataSetting != null)
                fmRawDataSetting.Dispose_LATool_Panel();
            Set_UISettingToMainSetting();
            vfWriteProfile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);//SaveCloseAToolStripMenuItem_Click
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.vfLoadProfile(enReadFrom.INI, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
            this.Close();
        }

        private void ToolStripMenuItem_SaveProfile_Click(object sender, EventArgs e)
        {
            //這裡要做一個保護，確保在loadPanelInfo之中，按下Ctrl-S不會導致Tool掛掉
            if (bCheckSettingsIsCorrect)
                return;

            Point screenPos = System.Windows.Forms.Cursor.Position;
            Point leftTop = new System.Drawing.Point(0, 0);
            Cursor.Position = leftTop;
            MyTool.Delay_ms(100);
            Cursor.Position = screenPos;

            if (UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable)
                Enable_Uniformity_Test();
            Set_UISettingToMainSetting();
            vfWriteProfile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);//SaveSToolStripMenuItem_Click
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.vfLoadProfile(enReadFrom.INI, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
            MyUIPage.ShowSuccessNotifier("Save Success !");
        }

        #endregion ToolStripMenuItem Click

        #region 說明

        private void m_dgvRawDataSPEC_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (!dgv.IsHandleCreated)
                return;
            if (dgv.CurrentCell.ColumnIndex == 1)
                dgv.BeginEdit(true);

            INI_Name_OpenTest tmpEnName = INI_Name_OpenTest.Frame_Count;
            Enum.TryParse<INI_Name_OpenTest>(dgv.CurrentRow.Cells[0].EditedFormattedValue.ToString(), out tmpEnName);

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation == SensorTestStation.FPC)
            {
                switch (tmpEnName)
                {
                    case INI_Name_OpenTest.FPC_RX_Test_Count:
                    case INI_Name_OpenTest.FPC_TX_Test_Count:
                        richTextBox_Open_Hint.Text = string.Format("Open detection of test count.\n");
                        break;

                    case INI_Name_OpenTest.FPC_Test_Timeout:
                        richTextBox_Open_Hint.Text = string.Format("Pass: Less than FPC_Test_Timeout\n"
                            + "NG: Exceed FPC_Test_Timeout\n"
                            + "FPC_Test_Timeout\n"
                            + "The timeout time limit of open test.\n"
                            + "Increase this value: Increase tolerance.\n"
                            + "Decrease this value: Decrease tolerance.\n");
                        break;

                    case INI_Name_OpenTest.FPC_Min_Threshold:
                        richTextBox_Open_Hint.Text = string.Format("Pass: Exceed Min_Threshold\n"
                            + "NG: Less than Min_Threshold\n"
                            + "Min_Threshold\n"
                            + "Open detection of sensor pin.\n"
                            + "Increase this value: Decrease tolerance.\n"
                            + "Decrease this value: Increase tolerance.\n");
                        break;
                }
            }
            else
            {
                if (tmpEnName == INI_Name_OpenTest.Min_Threshold)
                {
                    richTextBox_Open_Hint.Text = string.Format("Pass: Exceed Min_Threshold\n"
                            + "NG: Less than Min_Threshold\n"
                            + "Min_Threshold\n"
                            + "Open detection of sensor pin.\n"
                            + "Increase this value: Decrease tolerance.\n"
                            + "Decrease this value: Increase tolerance.\n");
                }
            }
        }

        private void m_dgvShortTestSPEC_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (!dgv.IsHandleCreated)
                return;
            if (dgv.CurrentCell.ColumnIndex == 1)
                dgv.BeginEdit(true);

            switch (dgv.CurrentCell.RowIndex)
            {
                case 0:
                    double val = double.Parse(dgv.Rows[0].Cells[1].Value.ToString());
                    double result = 0.0;
                    if (val == 0)
                    {
                        richTextBox_Short_Hint.Text = string.Format("Pass: Less than Max_Threshold\n"
                                + "NG: Exceed Max_Threshold\n"
                                + "Short detection of sensor pin.\n"
                                + "Increase this value : Increase tolerance.\n"
                                + "Decrease this value: Decrease tolerance.\n");
                        // + "R=Unlimited(KΩ)\n"); //20220418 Steven : 為AUO 2901暫時隱藏(Roy要求)
                    }
                    else
                    {
                        //if (Enum.IsDefined(typeof(V3_MCU), UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.iICType))
                        if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                        {
                            //if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU == (int)V3_MCU.ILI2312 || UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU == (int)V3_MCU.ILI2315)
                            if (EnumTool.IsEnumDefined<OldV3_MCU>(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU))
                                result = 1333333 / val;
                            else
                                result = 2333333 / val;
                            result /= 1000.0;

                            richTextBox_Short_Hint.Text = string.Format("Pass: Less than Max_Threshold\n"
                                + "NG: Exceed Max_Threshold\n"
                                + "Short detection of sensor pin.\n"
                                + "Increase this value : Increase tolerance.\n"
                                + "Decrease this value: Decrease tolerance.\n");
                            //+ "R={0}(KΩ)\nDefault:7", Math.Round(result, 2).ToString());
                            //20220418 Steven : 為AUO 2901暫時隱藏(Roy要求)
                        }
                        else
                        {
                            richTextBox_Short_Hint.Text = string.Format("Pass: Less than Max_Threshold\n"
                                + "NG: Exceed Max_Threshold\n"
                                + "Short detection of sensor pin.\n"
                                + "Increase this value : Increase tolerance.\n"
                                + "Decrease this value: Decrease tolerance.\n");
                            //+ R={0}(MΩ)\n"                             
                            //+ "Default:4000", UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.ConvertRes(val, 0, 0));
                            //20220418 Steven : 為AUO 2901暫時隱藏(Roy要求)
                        }
                    }
                    break;

                default:
                    richTextBox_Short_Hint.Text = "";
                    break;
            }
        }

        private void m_dgvCurrentTestSPEC_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (!dgv.IsHandleCreated)
                return;
            if (dgv.CurrentCell.ColumnIndex == 1)
                dgv.BeginEdit(true);
            INI_Tool ini = new INI_Tool(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);

            string sSampling = ini.IniReadValue("Current_Test", "SamplingTime","200");
            switch (dgv.CurrentCell.RowIndex)
            {
                case 0:
                    double val = double.Parse(dgv.Rows[0].Cells[1].Value.ToString());

                    richTextBox_Current_Hint.Text = string.Format("The sampling time is {2}ms each time. \n"
                            + "The total test time is {3}ms multiplied by the test count.\n"
                            + "The test will be conducted {0} times, and if any test fails, the overall result will be Fail.\n"
                            + "Estimated test time:{1} ms\n", val, (val * Convert.ToInt32(sSampling)), sSampling, sSampling);

                    break;

                case 1:
                    richTextBox_Current_Hint.Text = string.Format("Unit : mA\n"
                        +"If any test exceeds the set value, the overall result will be Fail.\n");
                    break;

                default:
                    richTextBox_Short_Hint.Text = "";
                    break;
            }
        }

        #endregion 說明

        private void Percent_ValueChanged(object sender, EventArgs e)
        {
            var _sender = sender as NumericUpDown;
            decimal _Value = 0;
            switch (_sender.Name)
            {
                case "nuCross_Distance_Percent":
                    decimal _MaxValue = nuCrossDistance.Maximum;
                    _Value = nuCross_Distance_Percent.Value;
                    decimal _NowValue = (_Value * _MaxValue) / 100;
                    nuCrossDistance.Value = Math.Round(_NowValue, 0, MidpointRounding.AwayFromZero);
                    break;

                case "nu_W_Percent":
                    decimal W_MaxValue = nuPaintingItemWidth.Maximum;
                    _Value = nu_W_Percent.Value;
                    decimal W_NowValue = (_Value * W_MaxValue) / 100;
                    nuPaintingItemWidth.Value = Math.Round(W_NowValue, 0, MidpointRounding.AwayFromZero);
                    decimal StartX = nuPaintingItemX.Value;
                    if (StartX != 0)
                    {
                        nuPaintingItemX.Value = W_MaxValue - nuPaintingItemWidth.Value;
                    }
                    break;

                case "nu_H_Percent":
                    decimal H_MaxValue = nuPaintingItemHeight.Maximum;
                    _Value = nu_H_Percent.Value;
                    decimal H_NowValue = (_Value * H_MaxValue) / 100;
                    nuPaintingItemHeight.Value = Math.Round(H_NowValue, 0, MidpointRounding.AwayFromZero);
                    decimal StartY = nuPaintingItemY.Value;
                    if (StartY != 0)
                    {
                        nuPaintingItemY.Value = H_MaxValue - nuPaintingItemHeight.Value;
                    }
                    break;
            }
        }

        private void btn_autogen_assign_path_Click(object sender, EventArgs e)
        {
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.ChangeCollectionPath(cb_autogen_path_rule.SelectedItem.ToString(), ref strReportPath);
            tb_autogen_path.Text = strReportPath;
        }

        private void chk_autogen_enable_CheckedChanged(object sender, EventArgs e)
        {
            bool show_ch = false;
            if ((StaticVar.g_Lang == enLangTable.LANG_TRADITIONAL_CHINESE) || (StaticVar.g_Lang == enLangTable.LANG_SIMPLIFIED_CHINESE))
                show_ch = true;
            if (uiSwitch_AutoGenEn.Active)
            {
                if (UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.uniformity_judgement)
                {
                    if (show_ch)
                        MyUIPage.ShowInfoDialog("Auto Gen. Setting", "測試時只會執行[Short Test] [Open Test] [Uniformity Test]", UIStyle.Orange, false);
                    else
                        MyUIPage.ShowInfoDialog("Auto Gen. Setting", "This action will only run [Short Test] [Open Test] [Uniformity Test]", UIStyle.Orange, false);
                }
                else
                {
                    if (show_ch)
                        MyUIPage.ShowInfoDialog("Auto Gen. Setting", "測試時只會執行[Short Test] [Open Test]與蒐集[Uniformity Test]的raw data。", UIStyle.Orange, false);
                    else
                        MyUIPage.ShowInfoDialog("Auto Gen. Setting", "This action will only run [Short Test] [Open Test]", UIStyle.Orange, false);
                }
                cb_SensorTestStation.SelectedItem = SensorTestStation.AUTOGEN.Desc();
            }
            else
            {
                //if(tmp_SensorTestStation != SensorTestStation.AUTOGEN)
                //   cb_SensorTestStation.SelectedItem = tmp_SensorTestStation);
                cb_SensorTestStation.SelectedItem = SensorTestStation.MODULE.Desc();//2023/07/18 auto gen mode disable，會強制轉回module module
            }
        }

        private void btn_autogen_clr_Click(object sender, EventArgs e)
        {
            string strPath = string.Format(@"{0}", cb_autogen_path_rule.Text);
            var tmpItem = EnumTool.GetValueFromDescription<enReportPathRule>(cb_autogen_path_rule.SelectedItem.ToString());
            if (tmpItem == enReportPathRule.Relative_Path)
            {
                string tmp = strPath;
                if (!Path.IsPathRooted(strPath))
                    tmp = Path.GetFullPath(strPath);

                if (string.IsNullOrEmpty(Path.GetFileName(tmp)))
                    strPath = "";
                else
                    strPath = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, tmp);
            }
            else
            {
                strPath = Path.GetFullPath(strPath);
            }
            //DialogResult dr = MessageBox.Show("Do you want to Clear all files in the Collection path?", "Auto Gen. Clear Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (dr == DialogResult.Yes)
            if (MyUIPage.ShowAskDialog("Auto Gen. Clear Folder", "Do you want to Clear all files in the Collection path?", UIStyle.Orange, false))
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.DelectDir(strPath, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);

        }

        private void btn_collection_clean_Click(object sender, EventArgs e)
        {
            string strPath = string.Format(@"{0}", tb_autogen_path.Text);
            if (strPath == "")
            {
                MyUIPage.ShowErrorDialog("Collection path is empty");
                return;
            }
            var tmpItem = EnumTool.GetValueFromDescription<enReportPathRule>(cb_autogen_path_rule.SelectedItem.ToString());
            if (tmpItem == enReportPathRule.Relative_Path)
            {
                string tmp = strPath;
                if (!Path.IsPathRooted(strPath))
                    tmp = Path.GetFullPath(strPath);

                if (string.IsNullOrEmpty(Path.GetFileName(tmp)))
                    strPath = "";
                else
                    strPath = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, tmp);
            }
            else
            {
                strPath = Path.GetFullPath(strPath);
            }
            //DialogResult dr = MessageBox.Show("Do you want to clean all files in the collection path?", "Auto Gen. Clean the Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (dr == DialogResult.Yes)
            if (MyUIPage.ShowAskDialog("Auto Gen. Clear Folder", "Do you want to clean all files in the collection path?", UIStyle.Orange, false))
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.DelectDir(strPath, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            //create autogenlogs
            DirectoryInfo source = new DirectoryInfo(string.Format("{0}\\{1}", strPath, UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_PassFolderName));
            source.Create();
        }

        private void btn_autogen_set_Click(object sender, EventArgs e)
        {
            bool file_exist = true;
            SensorTest_AutoGen_Thr_Setting fm_Thr_Setting = new SensorTest_AutoGen_Thr_Setting(ref UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen, ref file_exist, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            if (file_exist)
                fm_Thr_Setting.ShowDialog(this);

            INI_Tool ini = new INI_Tool(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            if (UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.uniformity_judgement)
            {
                ini.IniWriteValue("Auto_Gen.", "Uniformity_Judgement", "True");
                if (UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable)
                {
                    Enable_Uniformity_Test();
                }
            }
            else
                ini.IniWriteValue("Auto_Gen.", "Uniformity_Judgement", "False");
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.Modify_Open_Thr();
        }

        public void Show_AutoGen_Setting(bool flag)
        {
            uigb_Autogen.Visible = flag;
        }

        private void uitb_count_TextChanged(object sender, EventArgs e)
        {
            foreach (char ch in uitb_count.Text)
            {
                if (!char.IsDigit(ch))//是否为数字
                {
                    uitb_count.Text = "";
                    return;
                }
            }
            if (uitb_count.Text != "")
            {
                int number = int.Parse(uitb_count.Text);
                if (number != 0)
                {
                    return;
                }
            }
        }

        private void uitb_autogen_profilename_TextChanged(object sender, EventArgs e)
        {
            if ((new System.Text.RegularExpressions.Regex("^[\u4e00-\u9fa5]{0,}$")).IsMatch(uitb_autogen_profilename.Text))
            {
                if (uitb_autogen_profilename.Text != "")
                    MyUIPage.ShowWarningDialog("File name can not use chinese characters");
                return;
            }
        }

        private void Switch_ValueChanged(object sender, bool value)
        {
            var _sender = sender as UISwitch;
            //Console.WriteLine("{0} = {1}", _sender.Name, value);
            string SectorName = "OP_Mode";
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue(SectorName, _sender.Name.Replace("sw", ""), value.ToString());
            _sender.Active = value;
            //==============================================
            // swTestAllDone_FailStop & swTestAllDone_FailClose
            //==============================================
            if (_sender == swTestAllDone_FailStop)
                if (value == true)
                    Switch_ValueChanged(swTestAllDone_FailClose, false);

            if (_sender == swTestAllDone_FailClose)
                if (value == true)
                    Switch_ValueChanged(swTestAllDone_FailStop, false);

            //==============================================
            // swTestFinishPwrDown & swI2CDetect
            //==============================================
            if (_sender == swTestFinishPwrDown)
                if (value == true)
                    Switch_ValueChanged(swI2CDetect, false);

            if (_sender == swI2CDetect)
                if (value == true)
                    Switch_ValueChanged(swTestFinishPwrDown, false);
        }

        private void tabControl_Painting_SelectedIndexChanged(object sender, EventArgs e)
        {
            var MySender = sender as UITabControl;
            if (MySender.SelectedTab == tabPage_Finger)
            {
                TabControl_SelectedIndexChanged(tabCtl_FingerPatternItems, e);
            }
            else if (MySender.SelectedTab == tabPage_Pen)
            {
                TabControl_SelectedIndexChanged(tabCtl_PenPatternItems, e);
            }
            PaintingTreeView.SelectedNode = null;
            PaintingTreeView.SelectedNode = PaintingTreeView.Nodes[0];
        }

        private void CB_SelectedValueChanged(object sender, EventArgs e)
        {
            var MySender = sender as UIComboBox;
            List<string> lsDescriptions = EnumTool.GetDescriptions<TestItemNameDefine>();
            int iSelect = lsDescriptions.FindIndex(x => x.Contains(tbSPEC_Settings.SelectedTab.Name));
            TestItemNameDefine TestitemName = (TestItemNameDefine)Enum.Parse(typeof(TestItemNameDefine), iSelect.ToString());
            List<MultiSenTest_VAR._SPEC_> tmpSPEC = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestitemName].SPEC;

            string strKeyWord = "";
            if (TestitemName == TestItemNameDefine.enGpio_Test)
            {
                if (MySender == CB_I2C_Touch_EN)
                    strKeyWord = INI_Name_GPIOTest.I2C_Touch_EN.ToString();
                else if (MySender == CB_I2C_GPIO_0)
                    strKeyWord = INI_Name_GPIOTest.I2C_GPIO_0.ToString();
                else if (MySender == CB_I2C_GPIO_1)
                    strKeyWord = INI_Name_GPIOTest.I2C_GPIO_1.ToString();
                else if (MySender == CB_I2C_Touch_EN_BridgePin)
                    strKeyWord = INI_Name_GPIOTest.I2C_Touch_EN_BridgePin.ToString();
                else if (MySender == CB_I2C_GPIO_0_BridgePin)
                    strKeyWord = INI_Name_GPIOTest.I2C_GPIO_0_BridgePin.ToString();
                else if (MySender == CB_I2C_GPIO_1_BridgePin)
                    strKeyWord = INI_Name_GPIOTest.I2C_GPIO_1_BridgePin.ToString();
                //20220714 Steven
                else if (MySender == CB_I2C_INT)
                    strKeyWord = INI_Name_GPIOTest.I2C_INT.ToString();
                else if (MySender == CB_I2C_INT_BridgePin)
                    strKeyWord = INI_Name_GPIOTest.I2C_INT_BridgePin.ToString();
                else if (MySender == CB_I2C_Reset)
                    strKeyWord = INI_Name_GPIOTest.I2C_Reset.ToString();
                else if (MySender == CB_I2C_Reset_BridgePin)
                    strKeyWord = INI_Name_GPIOTest.I2C_Reset_BridgePin.ToString();
                else
                    return;

                int item = tmpSPEC.FindIndex(x => x.Name.Contains(strKeyWord));
                var _tmp = tmpSPEC[item];
                _tmp.ComboBoxSelect = MySender.SelectedItem.ToString();
                tmpSPEC[item] = _tmp;
            }
        }

        private void txt_Judge(object sender, KeyPressEventArgs e)
        {
            TextBox txtTemp = sender as TextBox;
            try
            {
                int kc = (int)e.KeyChar;

                if (txtTemp.Text.Length >= 4 && kc != 8)
                {
                    e.Handled = true;
                    return;
                }

                if ((kc < 48 || kc > 57) && (kc < 65 || kc > 70) && (kc < 97 || kc > 102) && kc != 8)
                    e.Handled = true;
            }
            catch (Exception)
            {
            }
        }

        private void chkBox_FocusEnter_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox _chk = sender as CheckBox;
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("Report", enReport.Barcode_FocusEnter, _chk.Checked.ToString());
        }

        private void uigb_Autogen_Scroll(object sender, ScrollEventArgs e)
        {
            uigb_Autogen.Invalidate();
        }

        private void SensorTest_Setting_Resize(object sender, EventArgs e)
        {
            if (fmRawDataSetting != null)
            {
                Size acutalSize = fmRawDataSetting.GetFormActualSize();
                if (tbSPEC_Settings.SelectedTab == tpUniformityTest)
                {
                    if (acutalSize.Width < panel_Uniformity.Width && acutalSize.Height < panel_Uniformity.Height)
                        fmRawDataSetting.Dock = DockStyle.Fill;
                    else
                        fmRawDataSetting.Dock = DockStyle.None;
                }
                else if (tbSPEC_Settings.SelectedTab == tpMicroOpen)
                {
                    if (acutalSize.Width < panel_MicroOpen.Width && acutalSize.Height < panel_MicroOpen.Height)
                        fmRawDataSetting.Dock = DockStyle.Fill;
                    else
                        fmRawDataSetting.Dock = DockStyle.None;
                }
            }
            if (tbSPEC_Settings.SelectedTab == tpPaintingTest)
                tabControl_Painting_SelectedIndexChanged(this.tabControl_Painting, e);
        }

        private void chkEnableFunction_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox MySender = sender as CheckBox;
            if (MySender.Name == "chkEnablePattern")
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bIsPatternEnable = MySender.Checked;
            else
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bIsShortHopping = MySender.Checked;
        }

        private void btn_PatternColorChange_Click(object sender, EventArgs e)
        {
            Color tmpColor = Color.White;
            if (SelectColor(ref tmpColor) == DialogResult.OK)
                lb_PatternColor.BackColor = tmpColor;
        }

        private void Enable_Uniformity_Test()
        {
            //當uniformity測項納入判斷時，必須要將unformity顯示在主畫面上，否則發生fail時，主畫面會看不到fail是從何方來的
            string strNodeKey = "Test Flow";
            int inNodeIndex = Array.FindIndex(strMainNode, s => s.Equals(strNodeKey));
            if (treeView_MainSettings.Nodes[inNodeIndex].Nodes[(int)(TestItemNameDefine.enUniformityTest) - 2].Checked == false)
            {
                treeView_MainSettings.AfterCheck -= this.treeView_MainSettings_AfterCheck;
                treeView_MainSettings.Nodes[inNodeIndex].Nodes[(int)(TestItemNameDefine.enUniformityTest) - 2].Checked = true; //需要將noisefrequency and charge curve的項目減掉
                treeView_MainSettings.AfterCheck += this.treeView_MainSettings_AfterCheck;
                if ((StaticVar.g_Lang == enLangTable.LANG_TRADITIONAL_CHINESE) || (StaticVar.g_Lang == enLangTable.LANG_SIMPLIFIED_CHINESE))
                    MyUIPage.ShowInfoDialog("WARNING", "Uniformity Test 將會被啟動.", UIStyle.Orange, false);
                else
                    MyUIPage.ShowInfoDialog("WARNING", "Uniformity Test will be enabled", UIStyle.Orange, false);
            }
        }

        private void uiSwitch_AutoGenEn_ValueChanged(object sender, bool value)
        {
            if (uiSwitch_AutoGenEn.Active)
            {
                Enable_Uniformity_Test();
            }
            btn_autogen_thr_set.Enabled = uiSwitch_AutoGenEn.Active;
            uitb_count.Enabled = uiSwitch_AutoGenEn.Active;
            btn_collection_clr.Enabled = uiSwitch_AutoGenEn.Active;
            cb_autogen_path_rule.Enabled = uiSwitch_AutoGenEn.Active;
            chk_addDate.Enabled = uiSwitch_AutoGenEn.Active;
            uitb_autogen_profilename.Enabled = uiSwitch_AutoGenEn.Active;

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable = uiSwitch_AutoGenEn.Active;
            if (UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable)
            {
                string strNodeKey = "Test Flow";
                int inNodeIndex = Array.FindIndex(strMainNode, s => s.Equals(strNodeKey));
                treeView_MainSettings.AfterCheck -= this.treeView_MainSettings_AfterCheck;
                treeView_MainSettings.Nodes[inNodeIndex].Nodes[(int)(TestItemNameDefine.enOpenTest) - 2].Checked = true;//需要將noisefrequency and charge curve的項目減掉
                treeView_MainSettings.Nodes[inNodeIndex].Nodes[(int)(TestItemNameDefine.enShortTest) - 2].Checked = true;//需要將noisefrequency and charge curve的項目減掉
                treeView_MainSettings.AfterCheck += this.treeView_MainSettings_AfterCheck;
            }
        }

        private void chkBox_FocusEnter2_CheckedChanged(object sender, EventArgs e)
        {
            UICheckBox _chk = sender as UICheckBox;
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("Report", enReport.Barcode_FocusEnter, _chk.Checked.ToString());
        }

        private void textBox_ScanInterval_Leave(object sender, EventArgs e)
        {
            var _MySender = sender as UITextBox;
            if (string.IsNullOrEmpty(_MySender.Text))
                _MySender.Text = "0";
        }

        private void SensorTest_Setting_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Z:
                    if (e.Alt == false)
                        break;

#if LITE_BUILD
                    //Lite Build不支援開啟這個功能
                    break;
#endif

                    string domainName = Environment.UserDomainName;
                    if (domainName != "ILI")
                        break;
                    SensorTest_ProfileHideParams ProfileForm = new SensorTest_ProfileHideParams(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                    ProfileForm.ShowDialog();
                    if (ProfileForm.ReturnDialogResult != System.Windows.Forms.DialogResult.Cancel)
                    {
                        UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ResetAllTestContains(enReadFrom.INI);
                        Initial_treeView(TestItemNameDefine.enPanel_Info);//這一行要擺在最後面
                    }
                    break;
            }
        }

        //特別針對Interface的數值欄位開放修改屬性，其餘的保持關閉
        private void dgvPanelInfoSPEC_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex == 1 && e.ColumnIndex == 1)
                dgvPanelInfoSPEC.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
            else
                dgvPanelInfoSPEC.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
        }
    }
}