using CommonExt;
using Steema.TeeChart.Styles;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ThirdPartyTools;
using FuncMethodCmd;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ITS_Studio.AllForms
{
    public partial class CDCToolfm : Form
    {
        private bool[] bTeeChartVisible = new bool[2] { true, false };
        private int Max_PenX_Bottom = 0;
        private int Max_PenX_Left = 0;
        private int Max_PenY_Bottom = 0;
        private int Max_PenY_Left = 0;

        private bool use_commonflow = true;

        private enum enTeeChart : int
        {
            in2D = 0,
            in3D = 1
        }

        private Node Key_Node = new Node();
        private Node Frame_Node = new Node();
        private Node Pen_Node = new Node();

        private int inDataFormatItemCount = 0;

        private ToolStripMenuItem[] StripCB_DataFormat;

        private ToolStripMenuItem[] StripCB_CDCFormat;

        private Dictionary<Pen_CDC_Format.enPenType, ToolStripMenuItem[]> DicStripCB_PenDataType = new Dictionary<Pen_CDC_Format.enPenType, ToolStripMenuItem[]>();

        private ToolStripMenuItem[] tmpToolStripMenuItem;

        private string[] strDataFormatItemText;

        private delegate void function(Message msg);

        private Dictionary<enWM_MSG, function> mMessageReceiver = new Dictionary<enWM_MSG, function>();

        private System.Timers.Timer m_timerChangeCDOption = new System.Timers.Timer();

        private System.Timers.Timer m_timerCDC_Status = new System.Timers.Timer();

        private bool bOptionChanged = false;

        protected DynamicTool MyTool = new DynamicTool();

        protected INI_Tool CDC_Tool_INI = new INI_Tool(ITS_Directory.CDCTool_INI);

        private TreeNode[] LastTreeNodeCollection;

        private Stopwatch CDC_Status = new Stopwatch();

        public CDCToolfm()
        {
            InitializeComponent();

            StaticVar.CDCToolformHandle = this.Handle;

            if(UI_GBV.mDev != null)
                UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType = enCDCDataFormat.inOriginal;

            StripCB_Diagram.SelectedIndex = 0;

            m_splitTChat.Panel2Collapsed = true;
            m_splitTChat.Panel2.Hide();

            ComboTreeViewInit();

            m_timerChangeCDOption.Enabled = false;
            m_timerChangeCDOption.Interval = 1000;
            m_timerChangeCDOption.Elapsed += new System.Timers.ElapsedEventHandler(m_timChangeCDCOption_Tick);
            m_timerCDC_Status.Enabled = false;
            m_timerCDC_Status.Interval = 1000;
            m_timerCDC_Status.Elapsed += new System.Timers.ElapsedEventHandler(m_timerCDC_Status_Tick);
            /// =============================================
            /// 註冊事件
            mMessageReceiver.Add(enWM_MSG.WM_CDC_Record_Idx, OnCDC_Sync_RecordIdx);
            mMessageReceiver.Add(enWM_MSG.WM_CDC_Replay_Frame, OnReplay_Frame);
            mMessageReceiver.Add(enWM_MSG.WM_CDC_SyncContexMenu, OnCDC_SyncContexMenu);
            mMessageReceiver.Add(enWM_MSG.WM_CDC_TypeInit, OnRegistMenuItemEvent);
            mMessageReceiver.Add(enWM_MSG.WM_CDC_CheckedFormat, OnCheckStripMenuItem_CDCFormat);
            mMessageReceiver.Add(enWM_MSG.WM_ShowColorGrid, OnCDCColorGrid);
            mMessageReceiver.Add(enWM_MSG.WM_CDC_Start, OnCDCStart);

            use_commonflow = (Directory.Exists("NoCdcCommonFlow")) ? false : true;
        }

        private Dictionary<int, string> dicDataType = new Dictionary<int, string>() { { 0, "MutualData" }, { 1, "SelfData" }, { 2, "KeyData" }, { 3, "PenData" } };

        private void ComboTreeViewInit()
        {
            CbTreeView_ShowHide.Nodes.Clear();
            LastTreeNodeCollection = new TreeNode[dicDataType.Count];
            foreach(var item in dicDataType)
            {
                CbTreeView_ShowHide.Nodes.Add(new TreeNode(item.Value.ToString()));
                LastTreeNodeCollection[item.Key] = new TreeNode(item.Value.ToString());
                var _value = CDC_Tool_INI.IniReadBool("CDC_DataType", item.Value, false);
                var _val = EnumTool.GetValueFromDescription<enCDC_Data>(item.Value);
                if(_value)
                    StaticVar.CDC_DataType |= _val;

                CbTreeView_ShowHide.Nodes[item.Key].Checked = _value;
                LastTreeNodeCollection[item.Key].Checked = _value;
            }
        }

        public void AssignReplayBar()
        {
            userControl_PlayBar_CDC.AssignToolFunc(StaticVar.NowExecuteTool, this.userControl_PlayBar_CDC, UI_GBV.mDev[m_iSelectedDev]);
        }

        private void m_timerCDC_Status_Tick(object sender, EventArgs e)
        {
            UpdateCDC_LED(CDC_Status.Elapsed);
        }

        private void m_timChangeCDCOption_Tick(object sender, EventArgs e)
        {
            m_timerChangeCDOption.Enabled = false;

            if(bOptionChanged || UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.bRunningStatus)
            {
                if(StaticVar._ITSTool_Var != enMode.Console_Mode)
                    this.Invoke(new Action(() =>
                    {
                        OnCDCStart(new Message());
                    }), null);
                bOptionChanged = false;
            }
        }

        protected void StripCB_CDCFormat_Click(object sender, EventArgs e)
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bForceStopCDC = true;
            if(StaticVar._ITSTool_Var != enMode.Console_Mode)
                this.Invoke(new Action(() =>
                {
                    OnCDCStop(new Message()
                    {
                        WParam = (IntPtr)m_iSelectedDev
                    });
                }), null);

            ToolStripMenuItem MenuItem = (ToolStripMenuItem)sender;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.NowSelect_CDC(MenuItem.Text);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.PenMode_SubName = null;

            ////清除細項的勾勾
            ResetDicStripCB_PenDataType();
            bOptionChanged = true;
            m_timerChangeCDOption.Enabled = true;
        }

        public void SetDefault_CDC_Type()
        {
            //if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.NowDevStatus != DevStatus.Connected)
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus != DevStatus.Connected)
            {
                tabControl_CDC.SelectedTab = tabPage_Mutual;

                if(MenuStrip_CDC.Items.Count == 0)
                    return;

                if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_ != null)
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.SetDefault_CDC_Type_Base();
            }
        }

        private void MenuItem_DataFormatItem(object DataType, enCDCDataFormat Selected)
        {
            if(DataType.GetType().Equals(typeof(enCDCDataFormat)))
            {
                inDataFormatItemCount = Enum.GetNames(typeof(enCDCDataFormat)).Count();
                strDataFormatItemText = new string[inDataFormatItemCount];
                MenuItem_DataFormat.DropDownItems.Clear();
                StripCB_DataFormat = new ToolStripMenuItem[inDataFormatItemCount];

                foreach(enCDCDataFormat item in (enCDCDataFormat[])Enum.GetValues(typeof(enCDCDataFormat)))
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
            else if(DataType.GetType().Equals(typeof(enShortDataFormat)))//enShortDataFormat
            {
                inDataFormatItemCount = Enum.GetNames(typeof(enShortDataFormat)).Count();
                strDataFormatItemText = new string[inDataFormatItemCount];
                MenuItem_DataFormat.DropDownItems.Clear();
                StripCB_DataFormat = new ToolStripMenuItem[inDataFormatItemCount];
                var iSelected = (int)Selected;

                foreach(enShortDataFormat item in (enShortDataFormat[])Enum.GetValues(typeof(enShortDataFormat)))
                {
                    StripCB_DataFormat[(int)item] = new ToolStripMenuItem();
                    StripCB_DataFormat[(int)item].Size = new System.Drawing.Size(152, 22);
                    StripCB_DataFormat[(int)item].Text = item.Desc();
                    StripCB_DataFormat[(int)item].Click += new System.EventHandler(StripCB_DataFormat_Short_Click);
                    StripCB_DataFormat[(int)item].Checked = false;
                    //目前V3暫時不開放short選擇DataFormat
                    if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                        StripCB_DataFormat[(int)item].Enabled = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.V3_NowSelect_CDC.Mutual_CDC_Name == V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT) ? false : true;

                    strDataFormatItemText[(int)item] = item.Desc();
                    MenuItem_DataFormat.DropDownItems.Add(StripCB_DataFormat[(int)item]);
                }

                StripCB_DataFormat[iSelected].Checked = true;
            }
        }

        private bool bSwitchShort = false;

        public void ResetDicStripCB_PenDataType()
        {
            //清除細項的勾勾
            foreach(var item in DicStripCB_PenDataType)
                foreach(ToolStripMenuItem item_2 in item.Value)
                    item_2.Checked = false;
        }

        public void CDC_Toolfm_Init(bool bRecoverRec)
        {
            this.Visible = false;

            var para = enTextBox.TextBox_Info | enTextBox.textBox_Delta | enTextBox.textBox_Max
                | enTextBox.textBox_Min | enTextBox.textBox_CurrentDelta | enTextBox.textBox_CurrentMax
                | enTextBox.textBox_CurrentMin | enTextBox.textBox_Win2Cnt;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_CDC_FrameShow, (IntPtr)para, "");

            bool bInitFlag = false;
            switch(StaticVar.NowExecuteTool)
            {
                case enToolName.CDCTool:
                    bInitFlag = StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected;
                    break;
                case enToolName.SensorTestTool:
                case enToolName.MultiSensorTestTool:
                    bInitFlag = StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Disconnect;
                    break;
            }

            if(bInitFlag)
            {
                if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_ != null)
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.CDCTool_Init(CDC_Tool_INI, true, null, false, is_commonflow: use_commonflow);//開啟Test Mode,INT Monitor
            }
            userControl_PlayBar_CDC.Visible = StaticVar.IsPlayback_Mode;
            userControl_PlayBar_CDC.ButtonVisible = !StaticVar.IsPlayback_Mode;
            MenuItem_Start.Enabled = !StaticVar.IsPlayback_Mode;

            TChart_Init(bRecoverRec);

            this.Visible = true;
        }

        private void SetMutualSplitterDistance()
        {
            if(!CDC_MainChart.Panel2Collapsed)
            {
                if(!splitContainer_Right.Panel2Collapsed && !splitContainer_Right.Panel1Collapsed)
                    splitContainer_Right.SplitterDistance = splitContainer_Right.Height * CDC_Tool_INI.IniReadInt("SplitDistance", "Mutual_Horizontal_SplitDist", 80) / 100;

                if(!splitContainer_Left.Panel2Collapsed && !splitContainer_Left.Panel1Collapsed)
                    splitContainer_Left.SplitterDistance = splitContainer_Left.Height * CDC_Tool_INI.IniReadInt("SplitDistance", "Mutual_Horizontal_SplitDist", 80) / 100;

                if(!splitContainer_MainBlock.Panel2Collapsed)
                    splitContainer_MainBlock.SplitterDistance = splitContainer_MainBlock.Width * CDC_Tool_INI.IniReadInt("SplitDistance", "Mutual_Vertical_SplitDist", 80) / 100;
            }
            //
            if(!CDC_MainChart.Panel1Collapsed)
            {
                if(!splitContainer_SelfKey_X.Panel2Collapsed)
                    splitContainer_SelfKey_X.SplitterDistance = splitContainer_SelfKey_X.Height * CDC_Tool_INI.IniReadInt("SplitDistance", "Key_Horizontal_SplitDist", 60) / 100;

                if(!splitContainer_SelfKey_Y.Panel2Collapsed)
                    splitContainer_SelfKey_Y.SplitterDistance = splitContainer_SelfKey_Y.Height * CDC_Tool_INI.IniReadInt("SplitDistance", "Key_Horizontal_SplitDist", 60) / 100;

                if(!splitContainer_SelfKey.Panel2Collapsed)
                    splitContainer_SelfKey.SplitterDistance = splitContainer_SelfKey.Width * CDC_Tool_INI.IniReadInt("SplitDistance", "Key_Vertical_SplitDist", 80) / 100;
            }
        }

        private void tabPage_Short_ResetChart()
        {
            // Selet 2D
            bTeeChartVisible[(int)enTeeChart.in2D] = true;
            m_splitTChat.Panel1Collapsed = false;
            m_splitTChat.Panel1.Show();

            bTeeChartVisible[(int)enTeeChart.in3D] = false;
            m_splitTChat.Panel2Collapsed = true;
            m_splitTChat.Panel2.Hide();
            bool bIsHW_Key = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.IsHW_Key(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Mode);
            //=====================================
            Chart_ShortX.Text = "X Channel";
            Chart_ShortX.Legend.Visible = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6) ? false : true;
            bezier_X_DIFF.Visible = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6) ? false : true;
            if(bIsHW_Key)
                Chart_ShortX.Axes.Bottom.SetMinMax(1, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X + UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts);
            else
                Chart_ShortX.Axes.Bottom.SetMinMax(1, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X);

            Chart_ShortX.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            bezier_X_LK.Visible = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6) ? false : true;
            bezier_X_LK.Clear();
            bar_X_SLK.Clear();
            //=====================================
            Chart_ShortY.Text = "Y Channel";
            Chart_ShortY.Legend.Visible = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6) ? false : true;
            bezier_Y_DIFF.Visible = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6) ? false : true;
            if(bIsHW_Key)
                Chart_ShortY.Axes.Bottom.SetMinMax(1, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y + 1);
            else
                Chart_ShortY.Axes.Bottom.SetMinMax(1, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y);

            Chart_ShortY.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            bezier_Y_LK.Visible = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6) ? false : true;
            bezier_Y_LK.Clear();
            bar_Y_SLK.Clear();
            splitContainer_Short.SplitterDistance = splitContainer_Short.Height / 2;
        }

        private void tabPage_Mutual_ResetChart()
        {
            // =================================
            // Mutual Data Chart
            // =================================
            bool bContainMutual = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.CDC_Data_Style.HasFlag(enCDCSMStyle.Mutual);
            bContainMutual &= StaticVar.CDC_DataType.HasFlag(enCDC_Data.MutualData);
            Chart_Mutual_2D.Visible = bContainMutual;

            bannerTool1.Active = Program.bSTRESS_BUILD;
            bannerTool2.Active = Program.bSTRESS_BUILD;

            if(bContainMutual)
            {
                //splitContainer_Left.Panel1Collapsed = false;
                Chart_Mutual_2D.Axes.Bottom.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X + 1);
                Chart_Mutual_2D.Axes.Left.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y + 1);
                Chart_Mutual_2D.Text = "2D " + UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.strTChartTitle;
                Chart_Mutual_2D.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                colorGrid_2D.Clear();
                colorGrid_2D.UsePaletteMin = true;

                Chart_Mutual_3D.Text = "3D " + UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.strTChartTitle;
                Chart_Mutual_3D.Axes.Bottom.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X + 1);
                Chart_Mutual_3D.Axes.Left.SetMinMax(1000, 5000);
                Chart_Mutual_3D.Axes.Depth.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y + 1);
                Chart_Mutual_3D.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                m_surface_3D.Clear();
                m_surface_3D.UsePaletteMin = true;
            }

            // =================================
            // Self_X Raw Data Chart
            // =================================
            bool bContainSelf_X = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.CDC_Data_Style.HasFlag(enCDCSMStyle.Self_X);
            bContainSelf_X &= StaticVar.CDC_DataType.HasFlag(enCDC_Data.SelfData);
            Chart_SelfX.Visible = bContainSelf_X;
            if(bContainSelf_X)
            {
                Chart_SelfX.Axes.Bottom.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X + 1);
                colorGrid_SelfX.Clear();
                colorGrid_SelfX.UsePaletteMin = true;
            }

            // =================================
            // Self_Y Raw Data Chart
            // =================================
            bool bContainSelf_Y = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.CDC_Data_Style.HasFlag(enCDCSMStyle.Self_Y);
            bContainSelf_Y &= StaticVar.CDC_DataType.HasFlag(enCDC_Data.SelfData);
            Chart_SelfY.Visible = bContainSelf_Y;
            if(bContainSelf_Y)
            {
                Chart_SelfY.Axes.Left.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y + 1);
                colorGrid_SelfY.Clear();
                colorGrid_SelfY.UsePaletteMin = true;
            }

            // =================================
            // Key Raw Data Chart
            // =================================
            bool bContainKey = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.CDC_Data_Style.HasFlag(enCDCSMStyle.Key);
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo != null)
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo != null)
                    bContainKey &= UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.IsHW_Key(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Mode);
            bContainKey &= StaticVar.CDC_DataType.HasFlag(enCDC_Data.KeyData);
            Chart_MutualKey.Visible = bContainKey;
            if(bContainKey)
            {
                Chart_SelfKeyX.Visible = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6);
                Chart_SelfKeyY.Visible = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6);
                splitContainer_SelfKey_X.Panel2Collapsed = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3);
                splitContainer_SelfKey.Panel2Collapsed = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3);
                //Mutual Key
                Chart_MutualKey.Axes.Bottom.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts + 1);
                Chart_MutualKey.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                Chart_MutualKey.Width = Chart_SelfX.Width;
                colorGrid_MutualKey.Clear();
                colorGrid_MutualKey.UsePaletteMin = true;

                //Self Key
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                {
                    Chart_SelfKeyX.Axes.Bottom.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts + 1);

                    Chart_SelfKeyX.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                    Chart_SelfKeyX.Width = Chart_SelfX.Width;
                    colorGrid_SelfKeyX.Clear();
                    colorGrid_SelfKeyX.UsePaletteMin = true;

                    Chart_SelfKeyY.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                    Chart_SelfKeyY.Width = Chart_SelfY.Width;
                    colorGrid_SelfKeyY.Clear();
                    colorGrid_SelfKeyY.UsePaletteMin = true;
                }
            }

            // =================================
            // Pen Raw Data Chart
            // =================================            
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                //更新StripCB_CDCFormatType，將目前沒用到的Type隱藏(Hide Pen Type)
                var CDC_AllTypes = EnumTool.GetDescriptions<V6_CDC_MODE_Define.CDC_Type_Mutual>();
                var PenTypes = Enum.GetNames(typeof(Pen_CDC_Format.enPenType)).ToList();
                var intersectedList = CDC_AllTypes.Intersect(PenTypes);
                foreach(var item in intersectedList)
                {
                    var _found = StripCB_CDCFormat.First(x => x.Text == item);
                    var _item = EnumTool.ParseEnum<Pen_CDC_Format.enPenType>(item);
                    _found.Visible = ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType & _item) != Pen_CDC_Format.enPenType.Disable);
                }
            }

            bool bIsContainsPen = false;
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType != Pen_CDC_Format.enPenType.Disable)
                bIsContainsPen = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.CDC_Data_Style.HasFlag(enCDCSMStyle.Pen);
            bIsContainsPen &= StaticVar.CDC_DataType.HasFlag(enCDC_Data.PenData);
            Chart_PenX_SE.Visible = bIsContainsPen;
            Chart_PenY_SE.Visible = bIsContainsPen;

            if(bIsContainsPen)
            {
                Chart_PenX_SE.Axes.Bottom.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X + 1);
                int iMax = (int)Pen_CDC_Format.TEST_MODE_PEN_X.TipRing_X;
                Chart_PenX_SE.Axes.Left.SetMinMax(0, iMax + 1);

                Chart_PenY_SE.Axes.Left.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y + 1);
                iMax = (int)Pen_CDC_Format.TEST_MODE_PEN_Y.TipRing_Y;
                Chart_PenY_SE.Axes.Bottom.SetMinMax(0, iMax + 1);

                colorGrid_PenX_SE.Clear();
                colorGrid_PenX_SE.UsePaletteMin = true;
                colorGrid_PenY_SE.Clear();
                colorGrid_PenY_SE.UsePaletteMin = true;
            }

            //=================================            
            CDC_MainChart.Panel1Collapsed = !bContainKey;
            CDC_MainChart.Panel2Collapsed = (!bContainMutual && !bContainSelf_X && !bIsContainsPen && !bContainSelf_Y);
            if(!bContainMutual && !bContainSelf_X && !bIsContainsPen && !bContainSelf_Y && !bContainKey)
                CDC_MainChart.Visible = false;
            else
                CDC_MainChart.Visible = true;

            splitContainer_MainBlock.Panel1Collapsed = (!bContainMutual && !bContainSelf_X && !bIsContainsPen);
            splitContainer_MainBlock.Panel2Collapsed = (!bContainSelf_Y && !bIsContainsPen);

            splitContainer_Left.Panel1Collapsed = (!bContainMutual);
            splitContainer_Left.Panel2Collapsed = (!bContainSelf_X && !bIsContainsPen);

            uiSplitContainer_SelfX.Panel1Collapsed = (!bContainSelf_X);
            uiSplitContainer_SelfX.Panel2Collapsed = (!bIsContainsPen);

            uiSplitContainer_SelfY.Panel1Collapsed = !bContainSelf_Y;
            uiSplitContainer_SelfY.Panel2Collapsed = !bIsContainsPen;

            //splitContainer_Short.SplitterDistance = splitContainer_Short.Height;

        }

        private void tabPage_Pen_ResetChart()
        {
            colorGrid_PenX.Clear();
            colorGrid_PenX.UsePaletteMin = true;
            colorGrid_PenY.Clear();
            colorGrid_PenY.UsePaletteMin = true;
            //目前X最大數量的是TEST_MODE_PEN_USI_X.ACK
            Match rxMatch = Regex.Match(Pen_CDC_Format.TEST_MODE_PEN_USI_X.ACK.Desc(), @"Size:(.*)");
            int iRange = int.Parse(rxMatch.Groups[1].ToString());
            Chart_PenX.Axes.Bottom.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X + 1);
            Chart_PenX.Axes.Left.SetMinMax(0, iRange + 1);//最大的是USI
            Max_PenX_Bottom = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X;
            Max_PenX_Left = iRange;
            //目前Y最大數量的是TEST_MODE_PEN_MPP_Y.DigitalHF_TipRing_Y
            rxMatch = Regex.Match(Pen_CDC_Format.TEST_MODE_PEN_MPP_Y.DigitalHF_TipRing_Y.Desc(), @"Size:(.*)");
            iRange = int.Parse(rxMatch.Groups[1].ToString());
            Chart_PenY.Axes.Bottom.SetMinMax(0, iRange + 1);
            Chart_PenY.Axes.Left.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y + 1);
            Max_PenY_Bottom = iRange;
            Max_PenY_Left = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y;
        }

        private void TChart_Init(bool bRecoverRec)
        {
            ResetAxesLimit();
            //預設為上一次的狀態，但CDC 的cell是呈現original的樣子，所以暫時先將check box改為original，使狀態與CDC cell一致。
            switch(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType)
            {
                case enPROTOCOL.V3:
                    var _V3_select = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.V3_NowSelect_CDC;
                    if(_V3_select == null)
                        break;
                    tabControl_CDC.SelectedTab = (_V3_select.Mutual_CDC_Name == V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT) ? tabPage_Short : tabPage_Mutual;
                    break;

                case enPROTOCOL.V6:
                    var _V6_select = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC;
                    if(_V6_select == null)
                        break;

                    switch(_V6_select.Mutual_CDC_Name)
                    {
                        case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT:
                            tabControl_CDC.SelectedTab = tabPage_Short;
                            break;
                        case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_USI:
                        case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_MPP:
                        case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_WGP:
                            tabControl_CDC.SelectedTab = tabPage_Pen;
                            break;
                        default:
                            tabControl_CDC.SelectedTab = tabPage_Mutual;
                            break;
                    }
                    break;

                default:
                    tabControl_CDC.SelectedTab = null;
                    break;
            }

            //20221018:Vane++ 新功能，選擇要在CDC畫面中顯示的內容
            #region CDC_DataType Hide or Show
            if(tabControl_CDC.SelectedTab == tabPage_Mutual)
            {
                CbTreeView_ShowHide.Visible = true;
                StaticVar.CDC_DataType = enCDC_Data.None;
                foreach(var item in dicDataType)
                {
                    var _value = CDC_Tool_INI.IniReadBool("CDC_DataType", item.Value, false);
                    var _val = EnumTool.GetValueFromDescription<enCDC_Data>(item.Value);
                    if(_value)
                        StaticVar.CDC_DataType |= _val;

                    CbTreeView_ShowHide.Nodes[item.Key].Checked = _value;
                    LastTreeNodeCollection[item.Key].Checked = _value;
                }
            }
            else
            {
                CbTreeView_ShowHide.Visible = false;
                StaticVar.CDC_DataType = enCDC_Data.None;
                foreach(var item in EnumTool.EnumToList<enCDC_Data>())
                    StaticVar.CDC_DataType |= item;
            }
            #endregion CDC_DataType Hide or Show

            var SelectedDataType = (enCDCDataFormat)UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType;
            if(tabControl_CDC.SelectedTab == tabPage_Short)
            {
                bSwitchShort = true;
                SelectedDataType = enCDCDataFormat.inOriginal;

                UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType = SelectedDataType;
                enShortDataFormat ShortDataType = new enShortDataFormat();
                MenuItem_DataFormatItem(ShortDataType, SelectedDataType);

                if((MenuStrip_CDC.Items.Count != 0) && (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3))
                    MenuItem_DataFormat.Visible = false;

                MenuItem_ShowNumber.Visible = true;
                MenuItem_AxesFlip.Visible = false;
            }
            else if((tabControl_CDC.SelectedTab == tabPage_Mutual) || (tabControl_CDC.SelectedTab == tabPage_Pen))
            {
                if((bRecoverRec) && (StripCB_DataFormat.Count() > 1))
                {
                    string str = StripCB_DataFormat.First(x => x.Checked).Text;
                    SelectedDataType = EnumTool.GetValueFromDescription<enCDCDataFormat>(str);
                }

                if(bSwitchShort)
                    SelectedDataType = enCDCDataFormat.inOriginal;
                bSwitchShort = false;

                UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType = SelectedDataType;
                enCDCDataFormat MutualDataType = new enCDCDataFormat();
                MenuItem_DataFormatItem(MutualDataType, SelectedDataType);

                if((MenuStrip_CDC.Items.Count != 0) && (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3))
                    MenuItem_DataFormat.Visible = true;

                MenuItem_ShowNumber.Visible = true;
                MenuItem_AxesFlip.Visible = true;
            }

            SetChartMode(tabControl_CDC.SelectedTab);
        }

        private void SetChartMode(TabPage _Page)
        {
            if(_Page == tabPage_Mutual)
            {
                Chart_Mutual_2D.Visible = true;
                Chart_Mutual_3D.Visible = true;
                Chart_SelfX.Visible = true;
                Chart_SelfY.Visible = true;
                Chart_MutualKey.Visible = true;
                Chart_SelfKeyX.Visible = true;
                Chart_SelfKeyY.Visible = true;
                //====================================
                Chart_ShortX.Visible = false;
                Chart_ShortY.Visible = false;
                //====================================
                Chart_PenX.Visible = false;
                Chart_PenY.Visible = false;

                tabPage_Mutual_ResetChart();
                if(StripCB_Diagram.Control != null)
                    StripCB_Diagram.Enabled = true;
            }
            else if(_Page == tabPage_Short)
            {
                Chart_Mutual_2D.Visible = false;
                Chart_Mutual_3D.Visible = false;
                Chart_SelfX.Visible = false;
                Chart_SelfY.Visible = false;
                Chart_MutualKey.Visible = false;
                Chart_SelfKeyX.Visible = false;
                Chart_SelfKeyY.Visible = false;
                //====================================
                Chart_ShortX.Visible = true;
                Chart_ShortY.Visible = true;
                //====================================
                Chart_PenX.Visible = false;
                Chart_PenY.Visible = false;

                tabPage_Short_ResetChart();
                if(StripCB_Diagram.Control != null)
                    StripCB_Diagram.Enabled = false;
            }
            else if(_Page == tabPage_Pen)
            {
                Chart_Mutual_2D.Visible = false;
                Chart_Mutual_3D.Visible = false;
                Chart_SelfX.Visible = false;
                Chart_SelfY.Visible = false;
                Chart_MutualKey.Visible = false;
                Chart_SelfKeyX.Visible = false;
                Chart_SelfKeyY.Visible = false;
                //====================================
                Chart_ShortX.Visible = false;
                Chart_ShortY.Visible = false;
                //====================================
                Chart_PenX.Visible = true;
                Chart_PenY.Visible = true;

                tabPage_Pen_ResetChart();
                if(StripCB_Diagram.Control != null)
                    StripCB_Diagram.Enabled = false;
            }
            else
            {
                Chart_Mutual_2D.Visible = false;
                Chart_Mutual_3D.Visible = false;
                Chart_SelfX.Visible = false;
                Chart_SelfY.Visible = false;
                Chart_MutualKey.Visible = false;
                Chart_SelfKeyX.Visible = false;
                Chart_SelfKeyY.Visible = false;
                //====================================
                Chart_ShortX.Visible = false;
                Chart_ShortY.Visible = false;
                //====================================
                Chart_PenX.Visible = false;
                Chart_PenY.Visible = false;
                if(StripCB_Diagram.Control != null)
                    StripCB_Diagram.Enabled = false;
            }
        }

        public void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.bRunningStatus == false)
                {
                    CDC_Toolfm_Init(true);
                    CDC_ThreadStart();
                }
                else
                {
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        CDC_ThreadStop(m_iSelectedDev);
                    }));
                }
            }
            catch(Exception ex)
            {
                //MyTool.Save_DBG_Log("[startToolStripMenuItem_Click] {0}", ex);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_CDC_FrameShow, (IntPtr)enTextBox.TextBox_Info, ex.ToString());
                UI_GBV.fmITS_Tool.ExceptionStop();
            }
        }

        private BackgroundWorker cdc_bgworker = new BackgroundWorker();

        public void CDC_ThreadStart()
        {
            if(cdc_bgworker.CancellationPending)
                cdc_bgworker.Dispose();
            m_timerCDC_Status.Start();

            MenuItem_DataFormat.Enabled = true;
            MenuItem_Start.Text = "Stop";//Start的狀態改為Stop
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.bRunningStatus = true;
            MyTool.Delay_ms(100);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bForceStopCDC = false;

            cdc_bgworker = new BackgroundWorker();
            cdc_bgworker.DoWork += CDC_Thread;
            cdc_bgworker.WorkerSupportsCancellation = true;
            cdc_bgworker.RunWorkerAsync();
        }

        public bool CDC_ThreadStop(int iSelectedDev)
        {
            MenuItem_Start.Enabled = false;
            UI_GBV.mDev[iSelectedDev].LocalVariable.Var_Multi.bForceStopCDC = true;
            m_timerCDC_Status.Stop();
            MenuItem_Start.Text = "Start";
            MenuItem_Start.Enabled = true;

            if(UI_GBV.mDev[iSelectedDev].LocalVariable.VarCDCTool_Multi.cdc_waitHandle != null)
            {
                CommonFlow.setting[iSelectedDev].no_retry = true;
                CommonFlow.ilitek_dev_setting(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev,
                                              ref CommonFlow.setting[m_iSelectedDev]);
                cdc_bgworker.CancelAsync();

                UI_GBV.mDev[iSelectedDev].LocalVariable.VarCDCTool_Multi.cdc_waitHandle.WaitOne();

                CommonFlow.setting[iSelectedDev].no_retry = false;
                CommonFlow.ilitek_dev_setting(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev,
                                              ref CommonFlow.setting[m_iSelectedDev]);
                UI_GBV.mDev[iSelectedDev].LocalVariable.VarCDCTool_Multi.cdc_waitHandle = null;
            }

            if(UI_GBV.mDev[iSelectedDev].m_GBV.Globe_Multi._CDCTool_ != null)
                UI_GBV.mDev[iSelectedDev].m_GBV.Globe_Multi._CDCTool_.CDC_Stop();

            if(StaticVar.bCDC_Record)
                vfRecordCDC();

            return !UI_GBV.mDev[iSelectedDev].LocalVariable.VarCDCTool_Multi.bRunningStatus;
        }

        private void CDC_Thread(object sender, DoWorkEventArgs e)
        {
            string strErrCode = "";
            bool bResult = false;
            bool IsNeedLimitAnalyze = true;
            bool IsNeedShowUI = true;
            //這一段是為了配合流程而加的，但會導致I2C速度變慢，但為了配合流程不行拿掉。
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                if(StaticVar.lsMain_USBDevInfo[SelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.OpenBridge_INT_Status();//0:INT Event為Low觸動; 1:INT Event為High觸動

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.cdc_waitHandle = new AutoResetEvent(false);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.cdc_waitHandle.Reset();
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.Start_ListenInterrupt_API(enInt_InDataType.CDC);
            if(use_commonflow)
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.m_HID_API.HID_CreateHandle(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].strDevPath);

                CommonFlow.CDC.setting.freq.prepared = false;
                CommonFlow.CDC.setting.open.prepared = false;
                CommonFlow.CDC.setting._short.prepared = false;
                CommonFlow.CDC.setting.p2p.prepared = false;

                INI_Tool cdc_ini = new INI_Tool(ITS_Directory.CDCTool_INI);
                CommonFlow.CDC.setting.skip_checksum =
                    cdc_ini.IniReadBool("System", "Skip_Checksum", false);

                CommonFlow.CDC.cdc_cb[m_iSelectedDev].update_grid =
                    new CommonFlow.update_grid_t(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.update_grid);

                if(Wifi_Var.useWifi)
                    CommonFlow.CDC.cdc_cb[m_iSelectedDev].get_raw =
                        new CommonFlow.CDC.get_raw_t(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.get_raw);

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.cdc =
                    CommonFlow.CDC.cdc_init(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, true, ref CommonFlow.CDC.cdc_cb[m_iSelectedDev], IntPtr.Zero);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.commonflow_get_info(
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev,
                    ref CommonFlow.info[m_iSelectedDev]);

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.mc =
                    new Int32[CommonFlow.info[m_iSelectedDev].tp.x_ch * CommonFlow.info[m_iSelectedDev].tp.y_ch];
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.sc_x =
                    new Int32[CommonFlow.info[m_iSelectedDev].tp.x_ch];
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.sc_y =
                    new Int32[CommonFlow.info[m_iSelectedDev].tp.y_ch];
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.pen_x =
                    new Int32[CommonFlow.info[m_iSelectedDev].tp.x_ch * 8];
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.pen_y =
                    new Int32[CommonFlow.info[m_iSelectedDev].tp.y_ch * 8];
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.key_mc =
                    new Int32[CommonFlow.info[m_iSelectedDev].tp.key_num];
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.key_x =
                    new Int32[CommonFlow.info[m_iSelectedDev].tp.key_num];
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.key_y =
                    new Int32[1];

                INI_Tool ini = new INI_Tool(ITS_Directory.Setting_INI);
                string cdc_len = ini.IniReadValue("Main", "Report_Size", "PacketSz_1024_");
                foreach(PacketSz item in Enum.GetValues(typeof(PacketSz)))
                {
                    if(!item.ToString().Equals(cdc_len))
                        continue;

                    CommonFlow.CDC.cdc_set_data_length(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.cdc, Convert.ToUInt16(item));
                    break;
                }

                CommonFlow.CDC.cdc_prepare(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.cdc);

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.iErrPktCnts = 0;

                do
                {
                    CDC_Status.Restart();
                    strErrCode = "";
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.CDC_Main(ref strErrCode, IsNeedLimitAnalyze, IsNeedShowUI);
                } while(!cdc_bgworker.CancellationPending);

                CommonFlow.CDC.cdc_exit(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.cdc);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();
            }
            else
            {
                do
                {
                    CDC_Status.Restart();
                    strErrCode = "";
                    bResult = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.CDC_MainEntry_Local(ref strErrCode, IsNeedLimitAnalyze, IsNeedShowUI);
                } while(bResult && !cdc_bgworker.CancellationPending);
            }


            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.bRunningStatus = false;
            if(bResult == false)
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_CDC_FrameShow, (IntPtr)enTextBox.TextBox_Info, strErrCode);

                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode != enSensorTest.Stop)
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.iTotalFailCounts++;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_SenTest_All_Finish, (IntPtr)0, strErrCode);
                }
            }

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.cdc_waitHandle.Set();
            UpdateCDC_LED(CDC_Status.Elapsed, true);
        }

        private void UpdateCDC_LED(TimeSpan _TimeeSpan, bool bForceStop = false)
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                TimeSpan elapsedTime = _TimeeSpan;

                Console.WriteLine("函式執行時間： " + elapsedTime.TotalMilliseconds + " 毫秒");
                CDC_StressMark.Visible = Program.bSTRESS_BUILD;

                if(bForceStop)
                {
                    pictureBox1.Image = global::ITS_Studio.Properties.Resources.icons8_no_connection_24;
                    CDC_Status.Stop();
                    if(Program.bSTRESS_BUILD)
                    {
                        CDC_StressMark.Text = "";
                        CDC_StressMark.BackColor = Color.Transparent;
                    }
                    return;
                }

                if(elapsedTime.TotalMilliseconds < 1001)
                {
                    pictureBox1.Image = global::ITS_Studio.Properties.Resources.icons8_strong_signal_24;
                    if(Program.bSTRESS_BUILD)
                    {
                        if(string.IsNullOrEmpty(CDC_StressMark.Text))
                            CDC_StressMark.BackColor = Color.Green;
                    }
                }
                else if((elapsedTime.TotalMilliseconds > 1000) && (elapsedTime.TotalMilliseconds < 3001))
                {
                    pictureBox1.Image = global::ITS_Studio.Properties.Resources.icons8_signal_24;
                    if(Program.bSTRESS_BUILD)
                    {
                        if(string.IsNullOrEmpty(CDC_StressMark.Text))
                            CDC_StressMark.BackColor = Color.Yellow;
                    }
                }
                else if((elapsedTime.TotalMilliseconds > 3000) && (elapsedTime.TotalMilliseconds < 5001))
                {
                    pictureBox1.Image = global::ITS_Studio.Properties.Resources.icons8_low_connection_24;
                    if(Program.bSTRESS_BUILD)
                        CDC_StressMark.BackColor = Color.Red;
                }
                else
                {
                    pictureBox1.Image = global::ITS_Studio.Properties.Resources.icons8_no_connection_24;
                    CDC_Status.Stop();
                    if(Program.bSTRESS_BUILD)
                        CDC_StressMark.BackColor = Color.Transparent;

                }
            }));
        }

        private void ShowAllNodeColorGrid(bool bReplay = false)
        {
            if(bOptionChanged || !UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.bRunningStatus)
                return;

            //UpdateCDC_LED(CDC_Status.Elapsed);

            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
            {
                var _V3_select = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.V3_NowSelect_CDC;
                if(_V3_select == null)
                    return;

                switch(_V3_select.Mutual_CDC_Name)
                {
                    case V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT:
                        Update_ShortPage();
                        break;

                    default:
                        Update_MutualPage();
                        break;
                }
            }
            else if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                var _V6_select = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC;
                if(_V6_select == null)
                    return;

                switch(_V6_select.Mutual_CDC_Name)
                {
                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT:
                        Update_ShortPage();
                        break;

                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_USI:
                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_MPP:
                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_WGP:
                        TChart_ColorReset_Pen(_V6_select.Mutual_CDC_Name);
                        TChart_AddPen(_V6_select.Mutual_CDC_Name, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.PenMode_SubName);
                        if(StaticVar.bCDC_Record)
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.RecordPen_RAW(_V6_select.Mutual_CDC_Name);
                        break;

                    default:
                        Update_MutualPage();
                        break;
                }
            }

            if(bReplay)
            {
                Application.DoEvents();
            }
            else
            {
                //實驗用，非最終寫法
                //if (!cdc_bgworker.CancellationPending)
                Task _task = new Task(() =>
                {
                    Application.DoEvents();
                });
                _task.Start();
                _task.Wait(2000);
            }
        }

        private ParseLog _ParseLog = new ParseLog();

        public bool AssignSelectFrameTo_1St(bool bUseNewFormat)
        {
            if((UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                && (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT))
                ShortType = enShortType.SLK;
            else if((UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                && (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.V3_NowSelect_CDC.Mutual_CDC_Name == V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT))
                ShortType = enShortType.SLK | enShortType.LK;
            else
                ShortType = enShortType.None;

            if(ShortType != enShortType.None)
                return true;

            var Mutual_CDC_Arr = new int[0, 0];
            var SelfX_CDC_Arr = new int[0];
            var SelfY_CDC_Arr = new int[0];
            var Key_MC = new int[0];
            var Key_SC_X = new int[0];
            var Key_SC_Y = new int[0];

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.InitCDCBuffer();

            var _PenType = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType;

            if(bUseNewFormat)
            {
                //這裡要搜尋第一面資料，並且把資料轉給UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData
                ParseLog._CDC_Datas CDC_Datas = new ParseLog._CDC_Datas();
                _ParseLog.Transfer_CDC(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.FileEmployee, 1, ref CDC_Datas);

                var iX = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X;
                var iY = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y;

                //不是所有的Mutual_CDC_Name都有Self資料，當Mutual_CDC_Name為TEST_MODE_OPEN時不需要判斷有無Self資料 modify by evayn
                if(CDC_Datas.CDC.Count == 0 || (iX * iY != CDC_Datas.CDC.Count)/*|| CDC_Datas.Self_X.Count == 0 || CDC_Datas.Self_Y.Count == 0*/)
                {
                    return false;
                }

                Mutual_CDC_Arr = ArrayTool.ConvertListTo2DArray(CDC_Datas.CDC.Select(x => (int)x).ToList(), iX, iY);
                SelfX_CDC_Arr = CDC_Datas.Self_X.Select(x => (int)x).ToArray();
                SelfY_CDC_Arr = CDC_Datas.Self_Y.Select(x => (int)x).ToArray();

                if(_PenType != Pen_CDC_Format.enPenType.Disable)
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.PenData_1stFrame[enPenCDC._X] = CDC_Datas.PEN_CDC_X;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.PenData_1stFrame[enPenCDC._Y] = CDC_Datas.PEN_CDC_Y;
                }

                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts != 0)
                {
                    Key_MC = CDC_Datas.Key_MC.Select(x => (int)x).ToArray();
                    Key_SC_X = CDC_Datas.Key_SC_X.Select(x => (int)x).ToArray();
                    Key_SC_Y = CDC_Datas.Key_SC_Y.Select(x => (int)x).ToArray();
                }

            }
            else
            {
                Mutual_CDC_Arr = (int[,])UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame[0].AA_Mutual.Clone();
                SelfX_CDC_Arr = (int[])UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame[0].AA_Self_X.Clone();
                SelfY_CDC_Arr = (int[])UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame[0].AA_Self_Y.Clone();

                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts != 0)
                {
                    Key_MC = (int[])UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame[0].Key_Mutual.Clone();
                    Key_SC_X = (int[])UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame[0].Key_Self_X.Clone();
                    Key_SC_Y = (int[])UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame[0].Key_Self_Y.Clone();
                }

                //================================
                // Pen
                //================================

                if(_PenType != Pen_CDC_Format.enPenType.Disable)
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData = new Dictionary<enPenCDC, List<short>>(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.PenData_1stFrame = new Dictionary<enPenCDC, List<short>>(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData);
                }

            }
            //================================
            // Mutual
            //================================     

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_1stFrame = Mutual_CDC_Arr;

            //================================
            // Self
            //================================          
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData_1stFrame[enSelf.Sense] = SelfX_CDC_Arr;

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData_1stFrame[enSelf.Drive] = SelfY_CDC_Arr;

            //================================
            // Key
            //================================
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualKey_1stFrame = Key_MC;


            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey_1stFrame[enSelf.Sense] = Key_SC_X;

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey_1stFrame[enSelf.Drive] = Key_SC_Y;

            AssignSelectFrameToBuffer(0, bUseNewFormat);

            return true;
        }

        //==========================================

        private void Update_MutualPage()
        {
            if(Program.bSTRESS_BUILD)
            {
                Console.WriteLine("[Update_MutualPage]");
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                    Console.WriteLine("\t{0}"
                        , UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name.Desc());
                else if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                    Console.WriteLine("\t{0}"
                        , UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.V3_NowSelect_CDC.Mutual_CDC_Name.Desc());
            }
            //Mutual
            var Mask = enCDCSMStyle.Mutual;
            if((UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.CDC_Data_Style & Mask) != enCDCSMStyle.None)
            {
                colorGrid_2D.Clear();
                m_surface_3D.Clear();
                int iMin, iMax, iWin2Cnt = 0;
                TChart_FindLimit_Mutual(out iMax, out iMin, out iWin2Cnt);
                TChart_ColorReset_Mutual(iMin, iMax, iWin2Cnt);
                TChart_AddMutual();            
            }

            //Self
            Mask = enCDCSMStyle.Self_Y | enCDCSMStyle.Self_X;
            if((UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.CDC_Data_Style & Mask) != enCDCSMStyle.None)
            {
                TChart_ColorReset_Self();
                TChart_AddSelf();
            }

            //Key
            Mask = enCDCSMStyle.Key;
            if((UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.CDC_Data_Style & Mask) != enCDCSMStyle.None)
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo != null)
                {
                    var Key_Mode = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Mode;
                    if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.IsHW_Key(Key_Mode))
                    {
                        TChart_ColorReset_Key();

                        TChart_AddKey(enCDCSMStyle.Mutual, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts);

                        if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                        {
                            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Sense].Length != 0)
                                TChart_AddKey(enCDCSMStyle.Self_X, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts);
                            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Drive].Length != 0)
                                TChart_AddKey(enCDCSMStyle.Self_Y, 1);
                        }
                    }
                }
            }
            //================================
            //Pen SE
            //================================
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType != Pen_CDC_Format.enPenType.Disable)
            {
                Mask = enCDCSMStyle.Pen;
                if((UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.CDC_Data_Style & Mask) != enCDCSMStyle.None)
                {
                    TChart_ColorReset_PenSE();
                    TChart_AddPen_SE();
                }
            }
            uiErrorCnt.Text = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.iErrPktCnts.ToString();
            if(Program.bSTRESS_BUILD)
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.iErrPktCnts != 0)
                    CDC_StressMark.Text = "ErrPktCnts";
                Console.WriteLine("[Update_MutualPage] Done");
            }
            this.Refresh();
            //======================================
            //Record
            if(StaticVar.bCDC_Record)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.RecordCDC_RAW(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData, Frame_Node.iMutual_Min, Frame_Node.iMutual_Max);
        }

        private void Update_ShortPage()
        {
            var item = (enShortDataFormat)UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType;
            List<double> X_DIFF_Array = new List<double>();
            List<double> Y_DIFF_Array = new List<double>();
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.AssignDiffArray(ref X_DIFF_Array, ref Y_DIFF_Array);

            List<double> X_SLK_Array = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.AssignShortArray(CDC_Tool_INI, "ShortTest", SHORT_NAME.X_SLK, item);
            List<double> Y_SLK_Array = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.AssignShortArray(CDC_Tool_INI, "ShortTest", SHORT_NAME.Y_SLK, item);
            List<double> X_LK_Array = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.AssignShortArray(CDC_Tool_INI, "ShortTest", SHORT_NAME.X_LK, item);
            List<double> Y_LK_Array = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.AssignShortArray(CDC_Tool_INI, "ShortTest", SHORT_NAME.Y_LK, item);

            bool bShow_LK_Data = (X_LK_Array.Count != 0) || (Y_LK_Array.Count != 0);
            Chart_ShortX.Legend.Visible = bShow_LK_Data;
            Chart_ShortY.Legend.Visible = bShow_LK_Data;
            bezier_X_DIFF.Visible = bShow_LK_Data;
            bezier_Y_DIFF.Visible = bShow_LK_Data;

            if(bShow_LK_Data)
            {
                bezier_X_DIFF.Clear();
                X_DIFF_Array.Insert(0, X_DIFF_Array[0]);
                bezier_X_DIFF.Add(X_DIFF_Array.ToArray());

                bezier_Y_DIFF.Clear();
                Y_DIFF_Array.Insert(0, Y_DIFF_Array[0]);
                bezier_Y_DIFF.Add(Y_DIFF_Array.ToArray());

                bezier_Y_LK.Clear();
                if(Y_LK_Array.Count != 0)
                {
                    Y_LK_Array.Insert(0, Y_LK_Array[0]);
                    bezier_Y_LK.Add(Y_LK_Array.ToArray());
                }

                bezier_X_LK.Clear();
                if(X_LK_Array.Count != 0)
                {
                    X_LK_Array.Insert(0, X_LK_Array[0]);
                    bezier_X_LK.Add(X_LK_Array.ToArray());
                }
            }

            bar_X_SLK.Clear();
            if(X_SLK_Array.Count != 0)
            {
                X_SLK_Array.Insert(0, X_SLK_Array[0]);
                bar_X_SLK.Add(X_SLK_Array.ToArray());
            }

            bar_Y_SLK.Clear();
            if(Y_SLK_Array.Count != 0)
            {
                Y_SLK_Array.Insert(0, Y_SLK_Array[0]);
                bar_Y_SLK.Add(Y_SLK_Array.ToArray());
            }
            //第0位置故意填一個重複值，讓UI可以保持曲線。
            if(Program.bSTRESS_BUILD)
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.iErrPktCnts != 0)
                    CDC_StressMark.Text = "ErrPktCnts";
                Console.WriteLine("[Update_MutualPage] Done");
            }
            if(StaticVar.bCDC_Record)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.RecordCDC_Short(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Dic_ShortValue.Values.ToList());
            //UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.RecordCDC_Short(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.iShortValue);
        }

        private void ResetColorGrid(int iMax, int iMin, ColorGrid colorGrid, ref int _NodeMax, ref int _NodeMin)
        {
            if((_NodeMin < iMin) && (_NodeMax > iMax))
                return;

            _NodeMin = (_NodeMin > iMin) ? iMin : _NodeMin;
            colorGrid.PaletteMin = _NodeMin;
            _NodeMax = (_NodeMax < iMax) ? iMax : _NodeMax;

            if(_NodeMax - _NodeMin < 5)
            {
                colorGrid.PaletteStep = 1;
                colorGrid.PaletteSteps = 5;
            }
            else
            {
                if(_NodeMax - _NodeMin > 256)
                {
                    colorGrid.PaletteStep = (_NodeMax - _NodeMin) / 256;
                    colorGrid.PaletteSteps = 256;
                }
                else
                {
                    colorGrid.PaletteStep = 1;
                    colorGrid.PaletteSteps = _NodeMax - _NodeMin;
                }
            }
            colorGrid.UsePaletteMin = true;
        }

        //==========================================

        private void TChart_FindLimit_Mutual(out int iMax, out int iMin, out int iWinCnt)
        {
            int columnCount = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X;
            int rowCount = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y;
            var tmp = (enCDCDataFormat)UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType;
            iMin = 0;
            iMax = 0;
            iWinCnt = 0;
            if(tmp == enCDCDataFormat.iSignal)
            {
                var iAA_X = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X;
                var iAA_Y = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y;
                for(int iX = 0; iX < iAA_X; iX++)
                {
                    for(int iY = 0; iY < iAA_Y; iY++)
                    {
                        var douValue = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_1stFrame[iX, iY] - UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData[iX, iY]);
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Signal[iX, iY] = (int)(douValue);
                    }
                }
                iMin = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Signal.Cast<int>().Min());
                iMax = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Signal.Cast<int>().Max());
            }
            else if(tmp == enCDCDataFormat.iDelta)
            {
                var iAA_X = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X;
                var iAA_Y = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y;
                for(int iX = 0; iX < iAA_X; iX++)
                {
                    for(int iY = 0; iY < iAA_Y; iY++)
                    {
                        var douValue = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Max[iX, iY] - UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Min[iX, iY]);
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Delta[iX, iY] = (int)(douValue);
                    }
                }
                iMin = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Delta.Cast<int>().Min());
                iMax = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Delta.Cast<int>().Max());
            }
            else if(tmp == enCDCDataFormat.iWindows2)
            {
                var iAA_X = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X - 1;
                var iAA_Y = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y - 1;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Win2 = new int[iAA_X, iAA_Y];
                for(int iX = 0; iX < iAA_X; iX++)
                {
                    for(int iY = 0; iY < iAA_Y; iY++)
                    {
                        var num_1 = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData[iX, iY];
                        var num_2 = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData[iX + 1, iY];
                        var num_3 = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData[iX, iY + 1];
                        var num_4 = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData[iX + 1, iY + 1];
                        var douValue = Math.Abs(Math.Abs(num_1 - num_2) - Math.Abs(num_3 - num_4));
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Win2[iX, iY] = (int)(douValue);
                    }
                }

                iMin = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Win2.Cast<int>().Min());
                iMax = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Win2.Cast<int>().Max());

                foreach(int item in UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Win2)
                    if(item > UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.iWin2Threshold)
                        iWinCnt++;

            }
            else
            {
                var rowList = Enumerable.Range(0, columnCount)
                    .Select(x => Enumerable.Range(0, rowCount)
                        .Select(y => UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData[x, y])
                        .ToArray())
                    .ToArray();

                iMin = int.MaxValue;
                iMax = int.MinValue;
                foreach(var item in rowList)
                {
                    int iNow_MIN = item.Cast<int>().Min();
                    iMin = (iNow_MIN < iMin) ? iNow_MIN : iMin;
                    int iNow_MAX = item.Cast<int>().Max();
                    iMax = (iNow_MAX > iMax) ? iNow_MAX : iMax;
                }
            }
        }

        private void TChart_ColorReset_Mutual(int inMin, int inMax, int inWin2Cnt)
        {
            int inMinMaxChangeFlag = 0;
            if((Frame_Node.iMutual_Min > inMin) || (Frame_Node.iMutual_Max < inMax))
            {
                if(Frame_Node.iMutual_Min > inMin)
                {
                    Frame_Node.iMutual_Min = inMin;
                    colorGrid_2D.PaletteMin = Frame_Node.iMutual_Min;
                    m_surface_3D.PaletteMin = Frame_Node.iMutual_Min;
                    inMinMaxChangeFlag++;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_CDC_FrameShow
                        , (IntPtr)enTextBox.textBox_Min, inMin.ToString());
                }

                if(Frame_Node.iMutual_Max < inMax)
                {
                    Frame_Node.iMutual_Max = inMax;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_CDC_FrameShow
                        , (IntPtr)enTextBox.textBox_Max, inMax.ToString());
                }

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_CDC_FrameShow
                    , (IntPtr)enTextBox.textBox_Delta, (Frame_Node.iMutual_Max - Frame_Node.iMutual_Min).ToString());

                if(Frame_Node.iMutual_Max - Frame_Node.iMutual_Min < 3)
                {
                    colorGrid_2D.PaletteStep = 1;
                    colorGrid_2D.PaletteSteps = 5;
                    m_surface_3D.PaletteStep = 1;
                    m_surface_3D.PaletteSteps = 5;
                }
                else
                {
                    if(Frame_Node.iMutual_Max - Frame_Node.iMutual_Min > 256)
                    {
                        colorGrid_2D.PaletteStep = (Frame_Node.iMutual_Max - Frame_Node.iMutual_Min) / 256;
                        colorGrid_2D.PaletteSteps = 256;
                        m_surface_3D.PaletteStep = (Frame_Node.iMutual_Max - Frame_Node.iMutual_Min) / 256;
                        m_surface_3D.PaletteSteps = 256;
                        inMinMaxChangeFlag++;
                    }
                    else
                    {
                        colorGrid_2D.PaletteStep = 1;
                        colorGrid_2D.PaletteSteps = Frame_Node.iMutual_Max - Frame_Node.iMutual_Min;
                        m_surface_3D.PaletteStep = 1;
                        m_surface_3D.PaletteSteps = Frame_Node.iMutual_Max - Frame_Node.iMutual_Min;
                        inMinMaxChangeFlag++;
                    }
                }

                if(inMinMaxChangeFlag > 0)
                {
                    Chart_Mutual_3D.Axes.Left.SetMinMax((Frame_Node.iMutual_Min - 200), (Frame_Node.iMutual_Max + 300));
                    m_surface_3D.Clear();
                }
            }
            //20220811 Steven : 當前Frame的MaxMin
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_CDC_FrameShow
                       , (IntPtr)enTextBox.textBox_CurrentMin, inMin.ToString());
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_CDC_FrameShow
                       , (IntPtr)enTextBox.textBox_CurrentMax, inMax.ToString());
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_CDC_FrameShow
                    , (IntPtr)enTextBox.textBox_CurrentDelta, (inMax - inMin).ToString());
            //20230627
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_CDC_FrameShow
                       , (IntPtr)enTextBox.textBox_Win2Cnt, inWin2Cnt.ToString());
        }

        private void TChart_ColorReset_Self()
        {
            colorGrid_SelfX.Clear();
            colorGrid_SelfY.Clear();
            var tmp = (enCDCDataFormat)UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType;
            var _Max = 0;
            var _Min = 0;
            if((UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.CDC_Data_Style & enCDCSMStyle.Self_X) == enCDCSMStyle.Self_X)
            {
                if(tmp == enCDCDataFormat.iSignal)
                {
                    _Max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData_Signal[enSelf.Sense].Cast<int>().Max();
                    _Min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData_Signal[enSelf.Sense].Cast<int>().Min();
                }
                else if(tmp == enCDCDataFormat.iDelta)
                {
                    _Max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData_Delta[enSelf.Sense].Cast<int>().Max();
                    _Min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData_Delta[enSelf.Sense].Cast<int>().Min();
                }
                else
                {
                    _Max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData[enSelf.Sense].Cast<int>().Max();
                    _Min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData[enSelf.Sense].Cast<int>().Min();
                }
                ResetColorGrid(_Max, _Min, colorGrid_SelfX, ref Frame_Node.iSelfX_Max, ref Frame_Node.iSelfX_Min);
            }

            if((UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.CDC_Data_Style & enCDCSMStyle.Self_Y) == enCDCSMStyle.Self_Y)
            {
                if(tmp == enCDCDataFormat.iSignal)
                {
                    _Max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData_Signal[enSelf.Drive].Cast<int>().Max();
                    _Min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData_Signal[enSelf.Drive].Cast<int>().Min();
                }
                else if(tmp == enCDCDataFormat.iDelta)
                {
                    _Max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData_Delta[enSelf.Drive].Cast<int>().Max();
                    _Min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData_Delta[enSelf.Drive].Cast<int>().Min();
                }
                else
                {
                    _Max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData[enSelf.Drive].Cast<int>().Max();
                    _Min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData[enSelf.Drive].Cast<int>().Min();
                }
                ResetColorGrid(_Max, _Min, colorGrid_SelfY, ref Frame_Node.iSelfY_Max, ref Frame_Node.iSelfY_Min);
            }
        }

        private void TChart_ColorReset_Key()
        {
            var _Max = 0;
            var _Min = 0;
            colorGrid_MutualKey.Clear();
            if((enCDCDataFormat)UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType == enCDCDataFormat.iSignal)
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualKey_Signal != null)
                {
                    _Max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualKey_Signal.Cast<int>().Max();
                    _Min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualKey_Signal.Cast<int>().Min();
                }
            }
            else
            {
                _Max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualKey.Cast<int>().Max();
                _Min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualKey.Cast<int>().Min();
            }
            ResetColorGrid(_Max, _Min, colorGrid_MutualKey, ref Key_Node.iMutual_Max, ref Key_Node.iMutual_Min);

            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType != enPROTOCOL.V6)
                return;

            colorGrid_SelfKeyX.Clear();
            if((enCDCDataFormat)UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType == enCDCDataFormat.iSignal)
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey_Signal[enSelf.Sense].Length != 0)
                {
                    _Min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey_Signal[enSelf.Sense].Cast<int>().Min();
                    _Max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey_Signal[enSelf.Sense].Cast<int>().Max();
                }
            }
            else
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Sense].Length != 0)
                {
                    _Min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Sense].Cast<int>().Min();
                    _Max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Sense].Cast<int>().Max();
                }
            }
            ResetColorGrid(_Max, _Min, colorGrid_SelfKeyX, ref Key_Node.iSelfX_Max, ref Key_Node.iSelfX_Min);

            colorGrid_SelfKeyY.Clear();
            if((enCDCDataFormat)UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType == enCDCDataFormat.iSignal)
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey_Signal[enSelf.Drive].Length != 0)
                {
                    _Min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey_Signal[enSelf.Drive].Cast<int>().Min();
                    _Max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey_Signal[enSelf.Drive].Cast<int>().Max();
                }
            }
            else
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Drive].Length != 0)
                {
                    _Min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Drive].Cast<int>().Min();
                    _Max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Drive].Cast<int>().Max();
                }
            }
            ResetColorGrid(_Max, _Min, colorGrid_SelfKeyY, ref Key_Node.iSelfY_Max, ref Key_Node.iSelfY_Min);
        }

        private void TChart_ColorReset_PenSE()
        {
            colorGrid_PenX_SE.Clear();
            colorGrid_PenY_SE.Clear();
            var _Max = 0;
            var _Min = 0;
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData.ContainsKey(enPenCDC._X))
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._X].Count > 0)
                {
                    _Max = (Int16)UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._X].Max();
                    _Min = (Int16)UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._X].Min();
                }
            }
            ResetColorGrid(_Max, _Min, colorGrid_PenX_SE, ref Pen_Node.iSelfX_Max, ref Pen_Node.iSelfX_Min);

            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData.ContainsKey(enPenCDC._Y))
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._Y].Count > 0)
                {
                    _Max = (Int16)UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._Y].Max();
                    _Min = (Int16)UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._Y].Min();
                }
            }
            ResetColorGrid(_Max, _Min, colorGrid_PenY_SE, ref Pen_Node.iSelfY_Max, ref Pen_Node.iSelfY_Min);
        }

        private void TChart_ColorReset_Pen(V6_CDC_MODE_Define.CDC_Type_Mutual CDC_Type)
        {
            colorGrid_PenX.Clear();
            colorGrid_PenY.Clear();

            var X_Max = Int16.MinValue;
            var X_Min = Int16.MaxValue;
            var Y_Max = Int16.MinValue;
            var Y_Min = Int16.MaxValue;
            var _name = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.PenMode_SubName;
            var _Value = new Int16[0];

            switch(CDC_Type)
            {
                case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_USI:
                    var usi_data = (Pen_CDC_Format.TEST_MODE_PEN_USI)_name;
                    _Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.USI_PenData[usi_data][(int)enPenCDC._X];
                    if(_Value != null)
                    {
                        X_Max = _Value.Max();
                        X_Min = _Value.Min();
                    }

                    _Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.USI_PenData[usi_data][(int)enPenCDC._Y];
                    if(_Value != null)
                    {
                        Y_Max = _Value.Max();
                        Y_Min = _Value.Min();
                    }
                    break;

                case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_WGP:
                    var wgp_data = (Pen_CDC_Format.TEST_MODE_PEN_WGP)_name;
                    _Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.WGP_PenData[wgp_data][(int)enPenCDC._X];
                    if(_Value != null)
                    {
                        X_Max = _Value.Max();
                        X_Min = _Value.Min();
                    }

                    _Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.WGP_PenData[wgp_data][(int)enPenCDC._Y];
                    if(_Value != null)
                    {
                        Y_Max = _Value.Max();
                        Y_Min = _Value.Min();
                    }
                    break;

                case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_MPP:
                    var mpp_data = (Pen_CDC_Format.TEST_MODE_PEN_MPP)_name;
                    _Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MPP_PenData[mpp_data][(int)enPenCDC._X];
                    if(_Value != null)
                    {
                        X_Max = _Value.Max();
                        X_Min = _Value.Min();
                    }

                    _Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MPP_PenData[mpp_data][(int)enPenCDC._Y];
                    if(_Value != null)
                    {
                        Y_Max = _Value.Max();
                        Y_Min = _Value.Min();
                    }
                    break;
            }

            if((X_Max != Int16.MinValue) || (X_Min != Int16.MaxValue))
                ResetColorGrid(X_Max, X_Min, colorGrid_PenX, ref Pen_Node.iSelfX_Max, ref Pen_Node.iSelfX_Min);

            if((Y_Max != Int16.MinValue) || (Y_Min != Int16.MaxValue))
                ResetColorGrid(Y_Max, Y_Min, colorGrid_PenY, ref Pen_Node.iSelfY_Max, ref Pen_Node.iSelfY_Min);
        }

        //==========================================

        private void TChart_AddMutual()
        {
            double douValue = 0;
            var iAA_X = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X;
            var iAA_Y = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y;
            if((enCDCDataFormat)UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType == enCDCDataFormat.iWindows2)
            {
                iAA_X = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X - 1;
                iAA_Y = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y - 1;
            }

            for(int iX = 0; iX < iAA_X; iX++)
            {
                for(int iY = 0; iY < iAA_Y; iY++)
                {
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.GetNowCDCValue(iX, iY, out douValue);

                    if(bTeeChartVisible[(int)enTeeChart.in2D])
                        colorGrid_2D.Add(iX + 1, douValue, iY + 1);

                    if(bTeeChartVisible[(int)enTeeChart.in3D])
                        m_surface_3D.Add(iX + 1, douValue, iY + 1);
                }
            }
        }

        private void TChart_AddSelf()
        {
            List<double> ls = new List<double>();
            double[] num_1, num_2;
            foreach(var item in EnumTool.EnumToList<enCDCSMStyle>())
            {
                switch(item)
                {
                    case enCDCSMStyle.Self_X:
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.GetNowSelfValue(item, ref ls);
                        num_1 = Enumerable.Range(1, ls.Count).Select(x => (double)x).ToArray();
                        num_2 = Enumerable.Repeat(1, ls.Count).Select(x => (double)x).ToArray();
                        colorGrid_SelfX.Add(num_1, ls.ToArray(), num_2);
                        break;
                    case enCDCSMStyle.Self_Y:
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.GetNowSelfValue(item, ref ls);
                        num_1 = Enumerable.Range(1, ls.Count).Select(x => (double)x).ToArray();
                        num_2 = Enumerable.Repeat(UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.inTChartSelfYIndex, ls.Count).Select(x => (double)x).ToArray();
                        colorGrid_SelfY.Add(num_2, ls.ToArray(), num_1);
                        break;
                    default:
                        break;
                }
            }
        }

        private void TChart_AddKey(enCDCSMStyle _CDC_Type, int width)
        {
            double douValue = 0;
            bool bHideColor = false;
            for(int iKey = 0; iKey < width; iKey++)
            {
                if(iKey >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts)
                    bHideColor = true;

                if(bHideColor == false)
                {
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.GetNowKeyValue(_CDC_Type, iKey, ref douValue);

                    if(_CDC_Type == enCDCSMStyle.Mutual)
                        colorGrid_MutualKey.Add(iKey + 1, (Int16)douValue, 0);

                    if(_CDC_Type == enCDCSMStyle.Self_X)
                        colorGrid_SelfKeyX.Add(iKey + 1, (Int16)douValue, 0);

                    if(_CDC_Type == enCDCSMStyle.Self_Y)
                        colorGrid_SelfKeyY.Add(iKey + 1, (Int16)douValue, 0);
                }
            }
        }

        private void TChart_AddPen_SE()
        {
            Int16 _uValue = 0;
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData.ContainsKey(enPenCDC._X))
            {
                var _DataCnts = (int)Pen_CDC_Format.TEST_MODE_PEN_X.TipRing_X;
                var _DataLength = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._X].Count / _DataCnts;
                for(int iY = 0; iY < _DataCnts; iY++)
                {
                    for(int iX = 0; iX < _DataLength; iX++)
                    {
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.GetNowPenValue(enPenCDC._X, (iY * _DataLength + iX), ref _uValue);
                        colorGrid_PenX_SE.Add(iX + 1, _uValue, iY + 1);
                    }
                }
            }

            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData.ContainsKey(enPenCDC._Y))
            {
                var _DataCnts = (int)Pen_CDC_Format.TEST_MODE_PEN_Y.TipRing_Y;
                var _DataLength = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._Y].Count / _DataCnts;
                for(int iX = 0; iX < _DataCnts; iX++)
                {
                    for(int iY = 0; iY < _DataLength; iY++)
                    {
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.GetNowPenValue(enPenCDC._Y, (iX * _DataLength + iY), ref _uValue);
                        colorGrid_PenY_SE.Add(iX + 1, _uValue, iY + 1);
                    }
                }
            }
        }

        private bool AddcolorGrid_PenX(object _InSelect, Int16[] Data)
        {
            string Chart_PenX_Text = "";
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.AddcolorGrid_PenX_Core(_InSelect, Data, Max_PenX_Left, Max_PenX_Bottom, out Chart_PenX_Text);

            Chart_PenX.Text = Chart_PenX_Text;
            colorGrid_PenX.Add(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.m_pen_X.ToArray(),
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.m_pen_val.ToArray(),
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.m_pen_Y.ToArray());
            //Console.WriteLine("Max {0} ,Min {1};colorGrid_PenX (Max:{2} Min:{3})", iMax, iMin, colorGrid_PenX.MaxYValue(), colorGrid_PenX.MinYValue());
            return true;
        }

        private bool AddcolorGrid_PenY(object _InSelect, Int16[] Data)
        {
            Type _MyType = _InSelect.GetType();

            string Chart_PenY_Text = "";
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.AddcolorGrid_PenY_Core(_InSelect, Data, Max_PenY_Left, Max_PenY_Bottom, out Chart_PenY_Text);

            colorGrid_PenY.Add(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.m_pen_X.ToArray(),
               UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.m_pen_val.ToArray(),
               UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.m_pen_Y.ToArray());
            return true;
        }

        private void TChart_AddPen(V6_CDC_MODE_Define.CDC_Type_Mutual CDC_Type, object DataType)
        {
            switch(CDC_Type)
            {
                case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_USI:
                    if(!DataType.GetType().Equals(typeof(Pen_CDC_Format.TEST_MODE_PEN_USI)))
                        return;
                    var _PEN_USI = (Pen_CDC_Format.TEST_MODE_PEN_USI)DataType;
                    splitContainer_PenData.Panel1Collapsed = EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_USI_X>(_PEN_USI.ToString()) ? false : true;
                    if(!splitContainer_PenData.Panel1Collapsed)
                    {
                        var Data = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.USI_PenData[_PEN_USI][(int)enPenCDC._X];
                        var _Select = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_USI_X>(_PEN_USI.ToString());
                        if(!AddcolorGrid_PenX(_Select, Data))
                            return;
                    }

                    splitContainer_PenData.Panel2Collapsed = EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_USI_Y>(_PEN_USI.ToString()) ? false : true;
                    if(!splitContainer_PenData.Panel2Collapsed)
                    {
                        var Data = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.USI_PenData[_PEN_USI][(int)enPenCDC._Y];
                        var _Select = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_USI_Y>(_PEN_USI.ToString());
                        if(!AddcolorGrid_PenY(_Select, Data))
                            return;
                    }
                    break;

                case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_WGP:
                    if(!DataType.GetType().Equals(typeof(Pen_CDC_Format.TEST_MODE_PEN_WGP)))
                        return;
                    var _PEN_WGP = (Pen_CDC_Format.TEST_MODE_PEN_WGP)DataType;
                    splitContainer_PenData.Panel1Collapsed = EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_WGP_X>(_PEN_WGP.ToString()) ? false : true;
                    if(!splitContainer_PenData.Panel1Collapsed)
                    {
                        var Data = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.WGP_PenData[_PEN_WGP][(int)enPenCDC._X];
                        var _Select = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_WGP_X>(_PEN_WGP.ToString());
                        if(!AddcolorGrid_PenX(_Select, Data))
                            return;
                    }

                    splitContainer_PenData.Panel2Collapsed = EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_WGP_Y>(_PEN_WGP.ToString()) ? false : true;
                    if(!splitContainer_PenData.Panel2Collapsed)
                    {
                        var Data = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.WGP_PenData[_PEN_WGP][(int)enPenCDC._Y];
                        var _Select = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_WGP_Y>(_PEN_WGP.ToString());
                        if(!AddcolorGrid_PenY(_Select, Data))
                            return;
                    }
                    break;

                case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_MPP:
                    if(!DataType.GetType().Equals(typeof(Pen_CDC_Format.TEST_MODE_PEN_MPP)))
                        return;
                    var _PEN_MPP = (Pen_CDC_Format.TEST_MODE_PEN_MPP)DataType;
                    splitContainer_PenData.Panel1Collapsed = EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_MPP_X>(_PEN_MPP.ToString()) ? false : true;
                    if(!splitContainer_PenData.Panel1Collapsed)
                    {
                        var Data = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MPP_PenData[_PEN_MPP][(int)enPenCDC._X];
                        var _Select = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_MPP_X>(_PEN_MPP.ToString());
                        if(!AddcolorGrid_PenX(_Select, Data))
                            return;
                    }

                    splitContainer_PenData.Panel2Collapsed = EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_MPP_Y>(_PEN_MPP.ToString()) ? false : true;
                    if(!splitContainer_PenData.Panel2Collapsed)
                    {
                        var Data = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MPP_PenData[_PEN_MPP][(int)enPenCDC._Y];
                        var _Select = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_MPP_Y>(_PEN_MPP.ToString());
                        if(!AddcolorGrid_PenY(_Select, Data))
                            return;
                    }
                    break;
            }

            //if (UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.boRecordStatus)
            //    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.RecordCDC_RAW(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData, Frame_Node.iMutual_Min, Frame_Node.iMutual_Max);
        }

        private void StripCB_DiagramChanged(object sender, EventArgs e)
        {
            if(StripCB_Diagram.SelectedIndex == 0)
            {
                // Selet 2D
                bTeeChartVisible[(int)enTeeChart.in2D] = true;
                m_splitTChat.Panel1Collapsed = false;
                m_splitTChat.Panel1.Show();

                //m_tChart_3D.Visible = false;
                bTeeChartVisible[(int)enTeeChart.in3D] = false;
                m_splitTChat.Panel2Collapsed = true;
                m_splitTChat.Panel2.Hide();
            }
            if(StripCB_Diagram.SelectedIndex == 1)
            {
                // Selet 3D
                bTeeChartVisible[(int)enTeeChart.in2D] = false;
                m_splitTChat.Panel1Collapsed = true;
                m_splitTChat.Panel1.Hide();

                //m_tChart_3D.Visible = true;
                bTeeChartVisible[(int)enTeeChart.in3D] = true;
                m_splitTChat.Panel2Collapsed = false;
                m_splitTChat.Panel2.Show();
            }
            if(StripCB_Diagram.SelectedIndex == 2)
            {
                // Selet 2D + 3D
                bTeeChartVisible[(int)enTeeChart.in2D] = true;
                m_splitTChat.Panel1Collapsed = false;
                m_splitTChat.Panel1.Show();

                //m_tChart_3D.Visible = true;
                bTeeChartVisible[(int)enTeeChart.in3D] = true;
                m_splitTChat.Panel2Collapsed = false;
                m_splitTChat.Panel2.Show();
            }
            Chart_Mutual_2D.Visible = bTeeChartVisible[(int)enTeeChart.in2D];
            Chart_Mutual_3D.Visible = bTeeChartVisible[(int)enTeeChart.in3D];

            MenuStrip_CDC.Close();
        }

        private void MenuItemRecord_Click(object sender, EventArgs e)
        {
            vfRecordCDC();
        }

        public void vfRecordCDC()
        {
            if(StaticVar.bCDC_Record)
            {
                GenLog.CDC.RemoveCdcHandle(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.handle);

                // Stop Record
                StaticVar.bCDC_Record = false;
                m_labRecordNu.Visible = false;
                MenuItem_Record.Checked = false;
                if(MenuStrip_CDC.Items.Count != 0)
                    MenuItem_CDCOption.Visible = true;
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.m_checktime.Stop();
            }
            else
            {
                // Start Record
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.m_iFileCount = 0;
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.m_iRecordFrameCounts = 1;
                string FILE_NAME = ITS_Directory.RecordPath + @"\Record_CDC";
                if(!Directory.Exists(FILE_NAME))
                    Directory.CreateDirectory(FILE_NAME);

                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.RecordFile_Header(Application.ProductVersion);

                StaticVar.bCDC_Record = true;
                m_labRecordNu.Visible = true;
                MenuItem_Record.Checked = true;
                if(MenuStrip_CDC.Items.Count != 0)
                    MenuItem_CDCOption.Visible = false;
            }
        }

        private void StripCB_DataFormat_Short_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem m = (ToolStripMenuItem)sender;
            int inSelectIndex = Array.FindIndex(strDataFormatItemText, s => s.Equals(m.Text));

            var item = StripCB_DataFormat.First(x => x.Checked == true);
            item.Checked = false;
            StripCB_DataFormat[inSelectIndex].Checked = true;
            //重置刻度
            Chart_ShortX.Axes.Left.Automatic = false;
            Chart_ShortY.Axes.Left.Automatic = false;

            Chart_ShortX.Axes.Left.Automatic = true;
            Chart_ShortY.Axes.Left.Automatic = true;
            var Selectitem = EnumTool.GetValueFromDescription<enShortDataFormat>(m.Text);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType = (enShortDataFormat)Selectitem;
        }

        private void StripCB_DataFormat_Mutual_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            int inSelectIndex = Array.FindIndex(strDataFormatItemText, s => s.Equals(menu.Text));

            var item = StripCB_DataFormat.First(x => x.Checked == true);
            item.Checked = false;
            StripCB_DataFormat[inSelectIndex].Checked = true;
            ResetAxesLimit();

            var mySelect = EnumTool.GetValueFromDescription<enCDCDataFormat>(menu.Text);

            UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType = mySelect;

            if(!StaticVar.IsPlayback_Mode)
            {
                if(inSelectIndex == (int)enCDCDataFormat.iSignal)
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.bGet1stFrame = false;
            }
            else
            {
                bool bisSuccess = true;
                if(this.userControl_PlayBar_CDC.TrackBar_Value == this.userControl_PlayBar_CDC.TrackBar_Min)
                    bisSuccess = AssignSelectFrameTo_1St(userControl_PlayBar_CDC.b_CDC_ReplayUseNewFormat);

                if(bisSuccess)
                    Update_MutualPage();
                else
                    MyUI.ShowErrorDialog("CDC Format Error", "This file is not a CDC recording, or there is an error in the file format.", UIStyle.Red, true);
            }
        }

        private void MenuStrip_CDC_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ToolStripMenuItem _PEN = new ToolStripMenuItem();//ToolStripItem
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                _PEN.Name = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.strCDCFormatText;
                _PEN.Text = _PEN.Name;
            }
            int ItemIdx = -1;
            bool bContainPenItem = false;
            bool bContainsSubItems = false;
            foreach(ToolStripMenuItem contextMenuItem in MenuStrip_CDC.Items)
            {
                ItemIdx++;
                if(contextMenuItem.Text.Equals(_PEN.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    bContainPenItem = true;
                    bContainsSubItems = contextMenuItem.HasDropDownItems;
                    break;
                }
            }

            MenuItem_Record.Checked = StaticVar.bCDC_Record;

            if(StaticVar.IsPlayback_Mode)
            {
                MenuItem_Record.Enabled = false;
                MenuItem_CDCOption.Enabled = false;
                MenuItem_Diagram.Enabled = true;

                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC == null)
                    return;
                var Mutual_CDC_Name = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name;

                Pen_CDC_Format.enPenType _PenType = Pen_CDC_Format.enPenType.Disable;
                if(Enum.IsDefined(typeof(Pen_CDC_Format.enPenType), _PEN.Text))
                    _PenType = EnumTool.ParseEnum<Pen_CDC_Format.enPenType>(_PEN.Text);

                if(!bContainPenItem)
                {
                    foreach(var item in EnumTool.EnumToList<Pen_CDC_Format.enPenType>())
                    {
                        var _found = MenuStrip_CDC.Items.Find(item.ToString(), false);
                        if(_found.Length > 0)
                            MenuStrip_CDC.Items.Remove(_found[0]);
                    }

                    if(_PenType == Pen_CDC_Format.enPenType.Disable)
                        return;

                    if(Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SE_BK)
                        return;

                    _PEN.DropDownItems.AddRange(DicStripCB_PenDataType[_PenType]);
                    MenuStrip_CDC.Items.Add(_PEN);
                }
                else
                {
                    bool bRemovePEN = false;
                    bRemovePEN |= (_PenType == Pen_CDC_Format.enPenType.Disable);
                    bRemovePEN |= (Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SE_BK);

                    if(bRemovePEN)
                        MenuStrip_CDC.Items.RemoveAt(ItemIdx);

                    if(!bRemovePEN && !bContainsSubItems)
                    {
                        MenuStrip_CDC.Items.RemoveAt(ItemIdx);
                        _PEN.DropDownItems.AddRange(DicStripCB_PenDataType[_PenType]);
                        MenuStrip_CDC.Items.Add(_PEN);
                    }
                }
            }
            else
            {
                MenuItem_Record.Enabled = true;
                MenuItem_CDCOption.Enabled = true;
                MenuItem_Diagram.Enabled = true;

                if(bContainPenItem)
                    MenuStrip_CDC.Items.RemoveAt(ItemIdx);
            }
        }

        protected override void WndProc(ref Message msg)
        {
            enWM_MSG pEnumPanoMsg = (enWM_MSG)msg.Msg;
            if(mMessageReceiver.Keys.Contains(pEnumPanoMsg))
                mMessageReceiver[pEnumPanoMsg](msg);
            base.WndProc(ref msg);
        }

        private void OnCDC_Sync_RecordIdx(Message msg)
        {
            m_labRecordNu.Text = Marshal.PtrToStringAuto(msg.LParam);
        }

        //將UI的ToolStripMenuItem加入清單內容
        private void OnCDC_SyncContexMenu(Message msg)
        {
            MenuItem_CDCOption.DropDownItems.Clear();
            MenuItem_CDCOption.DropDownItems.AddRange(StripCB_CDCFormat);
        }

        private void DicStripCB_PenDataTypeAdd(V6_CDC_MODE_Define.CDC_Type_Mutual _CDC_Type, Pen_CDC_Format.enPenType _PenType, EventHandler _event)
        {
            int i = 0;
            if(DicStripCB_PenDataType.NotContainsKey(_PenType))
            {
                switch(_CDC_Type)
                {
                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_MPP:
                        var EmployeeMPP = EnumTool.EnumToList<Pen_CDC_Format.TEST_MODE_PEN_MPP>();
                        DicStripCB_PenDataType.Add(_PenType, new ToolStripMenuItem[EmployeeMPP.Count()]);

                        foreach(var _item in EmployeeMPP)
                        {
                            DicStripCB_PenDataType[_PenType][i] = new ToolStripMenuItem();
                            DicStripCB_PenDataType[_PenType][i].Size = new Size(152, 22);
                            DicStripCB_PenDataType[_PenType][i].Text = _item.ToString();
                            //DicStripCB_PenDataType[_PenType][i].Checked = true;
                            DicStripCB_PenDataType[_PenType][i].Click -= _event;
                            DicStripCB_PenDataType[_PenType][i].Click += new System.EventHandler(_event);
                            i++;
                        }
                        break;

                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_WGP:
                        var EmployeeWGP = EnumTool.EnumToList<Pen_CDC_Format.TEST_MODE_PEN_WGP>();
                        DicStripCB_PenDataType.Add(_PenType, new ToolStripMenuItem[EmployeeWGP.Count()]);

                        foreach(var _item in EmployeeWGP)
                        {
                            DicStripCB_PenDataType[_PenType][i] = new ToolStripMenuItem();
                            DicStripCB_PenDataType[_PenType][i].Size = new Size(152, 22);
                            DicStripCB_PenDataType[_PenType][i].Text = _item.ToString();
                            //DicStripCB_PenDataType[_PenType][i].Checked = true;
                            DicStripCB_PenDataType[_PenType][i].Click -= _event;
                            DicStripCB_PenDataType[_PenType][i].Click += new System.EventHandler(_event);
                            i++;
                        }
                        break;

                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_USI:
                        var EmployeeUSI = EnumTool.EnumToList<Pen_CDC_Format.TEST_MODE_PEN_USI>();
                        DicStripCB_PenDataType.Add(_PenType, new ToolStripMenuItem[EmployeeUSI.Count()]);

                        foreach(var _item in EmployeeUSI)
                        {
                            DicStripCB_PenDataType[_PenType][i] = new ToolStripMenuItem();
                            DicStripCB_PenDataType[_PenType][i].Size = new Size(152, 22);
                            DicStripCB_PenDataType[_PenType][i].Text = _item.ToString();
                            //DicStripCB_PenDataType[_PenType][i].Checked = true;
                            DicStripCB_PenDataType[_PenType][i].Click -= _event;
                            DicStripCB_PenDataType[_PenType][i].Click += new System.EventHandler(_event);
                            i++;
                        }
                        break;
                }
            }
        }

        private void OnRegistMenuItemEvent(Message msg)
        {
            int iCounts = (int)msg.WParam;
            int _LPara = (int)msg.LParam;
            if((_LPara == 0) && (iCounts != 0))
            {
                StripCB_CDCFormat = new ToolStripMenuItem[iCounts];
                return;
            }

            bool bIsV6 = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6);
            int idx = (int)msg.WParam;

            StripCB_CDCFormat[idx] = new ToolStripMenuItem();
            StripCB_CDCFormat[idx].Size = new System.Drawing.Size(152, 22);

            if(bIsV6)
                StripCB_CDCFormat[idx].Text = ((V6_CDC_MODE_Define.CDC_Type_Mutual)_LPara).Desc();
            else
                StripCB_CDCFormat[idx].Text = ((V3_CDC_MODE_Define.CDC_Type_Mutual)_LPara).Desc();

            if(bIsV6)
            {
                var item = (V6_CDC_MODE_Define.CDC_Type_Mutual)_LPara;
                switch(item)
                {
                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_BG_BK:
                        StripCB_CDCFormat[idx].Name = item.ToString();
                        StripCB_CDCFormat[idx].Text = item.Desc();
                        var BG_Str = EnumTool.EnumToList<V6_CDC_MODE_Define.TEST_MODE_BG_BK_Params>();
                        tmpToolStripMenuItem = new ToolStripMenuItem[BG_Str.Count()];

                        int i = 0;
                        foreach(var sub_item in BG_Str)
                        {
                            tmpToolStripMenuItem[i] = new ToolStripMenuItem();
                            tmpToolStripMenuItem[i].Size = new Size(152, 22);
                            tmpToolStripMenuItem[i].Text = sub_item.Desc();
                            tmpToolStripMenuItem[i].Name = sub_item.ToString();
                            tmpToolStripMenuItem[i].Click -= StripCB_MutualParams_Click;
                            tmpToolStripMenuItem[i].Click += new System.EventHandler(StripCB_MutualParams_Click);
                            i++;
                        }
                        StripCB_CDCFormat[idx].DropDownItems.Clear();
                        StripCB_CDCFormat[idx].DropDownItems.AddRange(tmpToolStripMenuItem);
                        break;

                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_P2P:
                        StripCB_CDCFormat[idx].Visible = false;
                        break;

                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_MPP:
                        DicStripCB_PenDataTypeAdd(item, Pen_CDC_Format.enPenType.MPP, StripCB_PenDataType_Click);
                        StripCB_CDCFormat[idx].Name = item.Desc();
                        StripCB_CDCFormat[idx].Text = item.Desc();
                        StripCB_CDCFormat[idx].DropDownItems.Clear();
                        StripCB_CDCFormat[idx].DropDownItems.AddRange(DicStripCB_PenDataType[Pen_CDC_Format.enPenType.MPP]);
                        //預設
                        return;

                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_WGP:
                        DicStripCB_PenDataTypeAdd(item, Pen_CDC_Format.enPenType.WGP, StripCB_PenDataType_Click);
                        StripCB_CDCFormat[idx].Name = item.Desc();
                        StripCB_CDCFormat[idx].Text = item.Desc();
                        StripCB_CDCFormat[idx].DropDownItems.Clear();
                        StripCB_CDCFormat[idx].DropDownItems.AddRange(DicStripCB_PenDataType[Pen_CDC_Format.enPenType.WGP]);
                        return;

                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_USI:
                        DicStripCB_PenDataTypeAdd(item, Pen_CDC_Format.enPenType.USI, StripCB_PenDataType_Click);
                        StripCB_CDCFormat[idx].Name = item.Desc();
                        StripCB_CDCFormat[idx].Text = item.Desc();
                        StripCB_CDCFormat[idx].DropDownItems.Clear();
                        StripCB_CDCFormat[idx].DropDownItems.AddRange(DicStripCB_PenDataType[Pen_CDC_Format.enPenType.USI]);
                        return;
                }
            }
            //if (iCounts != 0)
            if(StripCB_CDCFormat[idx].DropDownItems.Count == 0)
                StripCB_CDCFormat[idx].Click += new System.EventHandler(StripCB_CDCFormat_Click);
        }

        private void StripCB_MutualParams_Click(object sender, EventArgs e)
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bForceStopCDC = true;
            if(StaticVar._ITSTool_Var != enMode.Console_Mode)
                if(!StaticVar.IsPlayback_Mode)
                    OnCDCStop(new Message());

            ToolStripMenuItem menu = (ToolStripMenuItem)sender;

            var _SelectParam = (V6_CDC_MODE_Define.TEST_MODE_BG_BK_Params)Enum.Parse(typeof(V6_CDC_MODE_Define.TEST_MODE_BG_BK_Params), menu.Name);
            //清除細項的勾勾
            ResetDicStripCB_PenDataType();
            foreach(ToolStripMenuItem item in tmpToolStripMenuItem)
                item.Checked = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.BG_SelectedParam = _SelectParam;

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.NowSelect_CDC(menu.OwnerItem.Text);
            //將細項的選項打勾
            tmpToolStripMenuItem[(int)_SelectParam].Checked = true;

            if(!StaticVar.IsPlayback_Mode)
            {
                bOptionChanged = true;
                m_timerChangeCDOption.Enabled = true;
            }
        }

        private void OnCheckStripMenuItem_CDCFormat(Message msg)
        {
            foreach(ToolStripMenuItem item in StripCB_CDCFormat)
                item.Checked = false;
            int index = (int)msg.WParam;
            StripCB_CDCFormat[index].Checked = true;
            //下列只有V6才支援
            if(DicStripCB_PenDataType.Count <= 0)
                return;

            //這裡不要放清除，不然勾勾會一直被清除
            ////清除細項的勾勾
            //ResetDicStripCB_PenDataType();
        }

        private void StripCB_PenDataType_Click(object sender, EventArgs e)
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bForceStopCDC = true;
            if(StaticVar._ITSTool_Var != enMode.Console_Mode)
                if(!StaticVar.IsPlayback_Mode)
                    OnCDCStop(new Message());

            ToolStripMenuItem menu = (ToolStripMenuItem)sender;

            var _PenType = (Pen_CDC_Format.enPenType)Enum.Parse(typeof(Pen_CDC_Format.enPenType), menu.OwnerItem.Name);

            //清除細項的勾勾
            ResetDicStripCB_PenDataType();
            foreach(ToolStripMenuItem item_2 in tmpToolStripMenuItem)
                item_2.Checked = false;

            switch(_PenType)
            {
                case Pen_CDC_Format.enPenType.MPP:
                    var _MPP = Enum.Parse(typeof(Pen_CDC_Format.TEST_MODE_PEN_MPP), menu.Text);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.PenMode_SubName = (Pen_CDC_Format.TEST_MODE_PEN_MPP)_MPP;
                    break;
                case Pen_CDC_Format.enPenType.USI:
                    var _USI = Enum.Parse(typeof(Pen_CDC_Format.TEST_MODE_PEN_USI), menu.Text);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.PenMode_SubName = (Pen_CDC_Format.TEST_MODE_PEN_USI)_USI;
                    break;
                case Pen_CDC_Format.enPenType.WGP:
                    var _WGP = Enum.Parse(typeof(Pen_CDC_Format.TEST_MODE_PEN_WGP), menu.Text);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.PenMode_SubName = (Pen_CDC_Format.TEST_MODE_PEN_WGP)_WGP;
                    break;
            }
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.NowSelect_CDC(_PenType.ToString());

            //將細項的選項打勾
            var item = DicStripCB_PenDataType[_PenType][(int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.PenMode_SubName];
            item.Checked = true;

            if(!StaticVar.IsPlayback_Mode)
            {
                bOptionChanged = true;
                m_timerChangeCDOption.Enabled = true;
            }
        }

        private void Chart_DoubleClick(object sender, EventArgs e)
        {
            ResetAxesLimit();
        }

        private void Chart_UndoZoom(object sender, EventArgs e)
        {
            if(MenuItem_ShowNumber.Checked)
                return;
            //=================================
            if(sender == Chart_Mutual_2D)
            {
                colorGrid_2D.Marks.Transparent = false;
                colorGrid_2D.Marks.Visible = false;
            }
            else if(sender == Chart_SelfX)
            {
                colorGrid_SelfX.Marks.Transparent = false;
                colorGrid_SelfX.Marks.Visible = false;
            }
            else if(sender == Chart_SelfY)
            {
                colorGrid_SelfY.Marks.Transparent = false;
                colorGrid_SelfY.Marks.Visible = false;
            }
            //=================================
            else if(sender == Chart_MutualKey)
            {
                colorGrid_MutualKey.Marks.Transparent = false;
                colorGrid_MutualKey.Marks.Visible = false;
            }
            else if(sender == Chart_SelfKeyX)
            {
                colorGrid_SelfKeyX.Marks.Transparent = false;
                colorGrid_SelfKeyX.Marks.Visible = false;
            }
            else if(sender == Chart_SelfKeyY)
            {
                colorGrid_SelfKeyY.Marks.Transparent = false;
                colorGrid_SelfKeyY.Marks.Visible = false;
            }
            //=================================
            else if(sender == Chart_PenX_SE)
            {
                colorGrid_PenX_SE.Marks.Transparent = false;
                colorGrid_PenX_SE.Marks.Visible = false;
            }

            else if(sender == Chart_PenY_SE)
            {
                colorGrid_PenY_SE.Marks.Transparent = false;
                colorGrid_PenY_SE.Marks.Visible = false;
            }
            //=================================
            else if(sender == Chart_PenX)
            {
                colorGrid_PenX.Marks.Transparent = false;
                colorGrid_PenX.Marks.Visible = false;
            }
            else if(sender == Chart_PenY)
            {
                colorGrid_PenY.Marks.Transparent = false;
                colorGrid_PenY.Marks.Visible = false;
            }
        }

        private void Chart_Zoom(object sender, EventArgs e)
        {
            if(sender == Chart_Mutual_2D)
            {
                if(!colorGrid_2D.Marks.Visible)
                {
                    colorGrid_2D.Marks.Transparent = true;
                    colorGrid_2D.Marks.Visible = true;
                }
            }
            else if(sender == Chart_SelfX)
            {
                if(!colorGrid_SelfX.Marks.Visible)
                {
                    colorGrid_SelfX.Marks.Transparent = true;
                    colorGrid_SelfX.Marks.Visible = true;
                }
            }
            else if(sender == Chart_SelfY)
            {
                if(!colorGrid_SelfY.Marks.Visible)
                {
                    colorGrid_SelfY.Marks.Transparent = true;
                    colorGrid_SelfY.Marks.Visible = true;
                }
            }
            else if(sender == Chart_MutualKey)
            {
                if(!colorGrid_MutualKey.Marks.Visible)
                {
                    colorGrid_MutualKey.Marks.Transparent = true;
                    colorGrid_MutualKey.Marks.Visible = true;
                }
            }
            else if(sender == Chart_SelfKeyX)
            {
                if(!colorGrid_SelfKeyX.Marks.Visible)
                {
                    colorGrid_SelfKeyX.Marks.Transparent = true;
                    colorGrid_SelfKeyX.Marks.Visible = true;
                }
            }
            else if(sender == Chart_SelfKeyY)
            {
                if(!colorGrid_SelfKeyY.Marks.Visible)
                {
                    colorGrid_SelfKeyY.Marks.Transparent = true;
                    colorGrid_SelfKeyY.Marks.Visible = true;
                }
            }
            else if(sender == Chart_PenX_SE)
            {
                if(!colorGrid_PenX_SE.Marks.Visible)
                {
                    colorGrid_PenX_SE.Marks.Transparent = true;
                    colorGrid_PenX_SE.Marks.Visible = true;
                }
            }
            else if(sender == Chart_PenY_SE)
            {
                if(!colorGrid_PenY_SE.Marks.Visible)
                {
                    colorGrid_PenY_SE.Marks.Transparent = true;
                    colorGrid_PenY_SE.Marks.Visible = true;
                    colorGrid_PenY_SE.Marks.Angle = 45D;
                }
            }
            else if(sender == Chart_PenX)
            {
                if(!colorGrid_PenX.Marks.Visible)
                {
                    colorGrid_PenX.Marks.Transparent = true;
                    colorGrid_PenX.Marks.Visible = true;
                }
            }
            else if(sender == Chart_PenY)
            {
                if(!colorGrid_PenY.Marks.Visible)
                {
                    colorGrid_PenY.Marks.Transparent = true;
                    colorGrid_PenY.Marks.Visible = true;
                }
            }
        }

        public void ResetAxesLimit()
        {
            Frame_Node.iMutual_Max = int.MinValue;
            Frame_Node.iMutual_Min = int.MaxValue;
            Frame_Node.iSelfX_Max = int.MinValue;
            Frame_Node.iSelfX_Min = int.MaxValue;
            Frame_Node.iSelfY_Max = int.MinValue;
            Frame_Node.iSelfY_Min = int.MaxValue;

            Key_Node.iMutual_Max = int.MinValue;
            Key_Node.iMutual_Min = int.MaxValue;
            Key_Node.iSelfX_Max = int.MinValue;
            Key_Node.iSelfX_Min = int.MaxValue;
            Key_Node.iSelfY_Max = int.MinValue;
            Key_Node.iSelfY_Min = int.MaxValue;

            Pen_Node.iMutual_Max = int.MinValue;
            Pen_Node.iMutual_Min = int.MaxValue;
            Pen_Node.iSelfX_Max = int.MinValue;
            Pen_Node.iSelfX_Min = int.MaxValue;
            Pen_Node.iSelfY_Max = int.MinValue;
            Pen_Node.iSelfY_Min = int.MaxValue;
        }

        private void MenuItem_ShowNumber_Click(object sender, EventArgs e)
        {
            if(MenuItem_ShowNumber.Checked)
            {
                MenuItem_ShowNumber.Checked = false;
                colorGrid_2D.Marks.Visible = false;
                colorGrid_SelfX.Marks.Visible = false;
                colorGrid_SelfY.Marks.Visible = false;
                colorGrid_MutualKey.Marks.Visible = false;
                colorGrid_SelfKeyX.Marks.Visible = false;
                colorGrid_SelfKeyY.Marks.Visible = false;
                colorGrid_PenX_SE.Marks.Visible = false;
                colorGrid_PenY_SE.Marks.Visible = false;
                colorGrid_PenX.Marks.Visible = false;
                colorGrid_PenY.Marks.Visible = false;
                bar_X_SLK.Marks.Visible = false;
                bar_Y_SLK.Marks.Visible = false;
                //bezier_X_LK.Marks.Visible = false;
                //bezier_Y_LK.Marks.Visible = false;
                //bezier_X_DIFF.Marks.Visible = false;
                //bezier_Y_DIFF.Marks.Visible = false;
            }
            else
            {
                MenuItem_ShowNumber.Checked = true;
                colorGrid_2D.Marks.Transparent = true;
                colorGrid_2D.Marks.Visible = true;
                colorGrid_SelfX.Marks.Transparent = true;
                colorGrid_SelfX.Marks.Visible = true;
                colorGrid_SelfY.Marks.Transparent = true;
                colorGrid_SelfY.Marks.Visible = true;
                colorGrid_MutualKey.Marks.Transparent = true;
                colorGrid_MutualKey.Marks.Visible = true;
                colorGrid_SelfKeyX.Marks.Transparent = true;
                colorGrid_SelfKeyX.Marks.Visible = true;
                colorGrid_SelfKeyY.Marks.Transparent = true;
                colorGrid_SelfKeyY.Marks.Visible = true;

                colorGrid_PenX_SE.Marks.Transparent = true;
                colorGrid_PenX_SE.Marks.Visible = true;

                colorGrid_PenX.Marks.Transparent = true;
                colorGrid_PenX.Marks.Visible = true;
                colorGrid_PenY.Marks.Transparent = true;
                colorGrid_PenY.Marks.Visible = true;

                colorGrid_PenY_SE.Marks.Transparent = true;
                colorGrid_PenY_SE.Marks.Angle = 45D;
                colorGrid_PenY_SE.Marks.Visible = true;

                bar_X_SLK.Marks.Visible = true;
                bar_Y_SLK.Marks.Visible = true;
                //bezier_X_LK.Marks.Visible = true;
                //bezier_Y_LK.Marks.Visible = true;
                //bezier_X_DIFF.Marks.Visible = true;
                //bezier_Y_DIFF.Marks.Visible = true;
            }
        }

        private void axis_CheckStateChanged(object sender, EventArgs e)
        {
            var _tmp = sender as ToolStripMenuItem;

            if(_tmp == axisXToolStripMenuItem)
            {
                Chart_Mutual_2D.Axes.Bottom.Inverted = (_tmp.CheckState == CheckState.Checked);
                Chart_SelfX.Axes.Bottom.Inverted = (_tmp.CheckState == CheckState.Checked);
                Chart_Mutual_3D.Axes.Bottom.Inverted = (_tmp.CheckState == CheckState.Checked);
                Chart_PenX_SE.Axes.Bottom.Inverted = (_tmp.CheckState == CheckState.Checked);
                Chart_PenY_SE.Axes.Bottom.Inverted = (_tmp.CheckState == CheckState.Checked);
                if(!CDC_MainChart.Panel1Collapsed)
                {
                    Chart_MutualKey.Axes.Bottom.Inverted = (_tmp.CheckState == CheckState.Checked);
                    Chart_SelfKeyX.Axes.Bottom.Inverted = (_tmp.CheckState == CheckState.Checked);
                }
                Chart_PenX.Axes.Bottom.Inverted = (_tmp.CheckState == CheckState.Checked);
                Chart_PenY.Axes.Bottom.Inverted = (_tmp.CheckState == CheckState.Checked);
            }
            else if(_tmp == axisYToolStripMenuItem)
            {
                Chart_Mutual_2D.Axes.Left.Inverted = (_tmp.CheckState == CheckState.Checked);
                Chart_SelfY.Axes.Left.Inverted = (_tmp.CheckState == CheckState.Checked);
                Chart_Mutual_3D.Axes.Depth.Inverted = (_tmp.CheckState == CheckState.Checked);
                Chart_PenX_SE.Axes.Left.Inverted = (_tmp.CheckState == CheckState.Checked);
                Chart_PenY_SE.Axes.Left.Inverted = (_tmp.CheckState == CheckState.Checked);
                Chart_PenX.Axes.Left.Inverted = (_tmp.CheckState == CheckState.Checked);
                Chart_PenY.Axes.Left.Inverted = (_tmp.CheckState == CheckState.Checked);
            }
        }

        //Steven 20220609 : 方向鍵通常會被其他元件攔截，所以通常不會觸發KeyDown/KeyPress
        //重寫控制元件ProcessCmdKey 先攔截處理
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(!userControl_PlayBar_CDC.Visible)
                return false;

            switch(keyData)
            {
                case Keys.Right:
                    userControl_PlayBar_CDC.TrackBarValuePlus();
                    break;
                case Keys.Left:
                    userControl_PlayBar_CDC.TrackBarValueReduce();
                    break;
                default:
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void uiComboTreeView1_DropDownClosed(object sender, EventArgs e)
        {
            foreach(var item in dicDataType)
                CbTreeView_ShowHide.Nodes[item.Key].Checked = LastTreeNodeCollection[item.Key].Checked;

            CbTreeView_ShowHide.Text = "Select Chart";
            if(IsBtnClicked)
            {
                if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                {
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        CDC_ThreadStop(m_iSelectedDev);
                    }));
                }

                this.Invoke(new MethodInvoker(delegate()
                {
                    CDC_Toolfm_Init(true);
                }));

                if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                    CDC_ThreadStart();

                IsBtnClicked = false;
            }

        }

        private bool IsBtnClicked = false;

        private void uiComboTreeView1_NodesSelected(object sender, TreeNodeCollection nodes)
        {
            IsBtnClicked = true;

            this.Invoke(new MethodInvoker(delegate()
                {
                    //uiComboTreeView1.Visible = false;
                    foreach(var item in dicDataType)
                    {
                        //更新tmp buffer
                        LastTreeNodeCollection[item.Key].Checked = nodes[item.Key].Checked;
                        CDC_Tool_INI.IniWriteValue("CDC_DataType", item.Value, nodes[item.Key].Checked);
                    }
                }));
        }

        private UIPage MyUI = new UIPage();

        private void Chart_Mutual_3D_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            double Test = colorLine_Limit.Value;
            if(MyUI.InputDoubleDialog(ref Test))
            {
                colorLine_Limit.Active = true;
                colorLine_Limit.Value = Test;
                annotation_NowValue.Active = true;
                annotation_NowValue.Text = string.Format("Now Limit :{0}", Test);
            }
            else
            {
                colorLine_Limit.Active = false;
                annotation_NowValue.Active = false;
            }
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            var _sender = sender as SplitContainer;
            if(_sender == splitContainer_Left)
            {
                if(splitContainer_Right.Panel2Collapsed || splitContainer_Right.Panel1Collapsed)
                    return;
                if(splitContainer_Right.SplitterDistance != e.SplitY)
                    splitContainer_Right.SplitterDistance = e.SplitY;
                if(bChangeByManual)
                {
                    var _value = (int)(((double)splitContainer_Right.SplitterDistance) / ((double)splitContainer_Right.Height) * 100 + 0.5);
                    CDC_Tool_INI.IniWriteValue("SplitDistance", "Mutual_Horizontal_SplitDist", _value.ToString());
                    bChangeByManual = false;
                }
            }
            else if(_sender == splitContainer_Right)
            {
                if(splitContainer_Left.Panel2Collapsed || splitContainer_Left.Panel1Collapsed)
                    return;
                if(splitContainer_Left.SplitterDistance != e.SplitY)
                    splitContainer_Left.SplitterDistance = e.SplitY;
                if(bChangeByManual)
                {
                    var _value = (int)(((double)splitContainer_Left.SplitterDistance) / ((double)splitContainer_Left.Height) * 100 + 0.5);
                    CDC_Tool_INI.IniWriteValue("SplitDistance", "Mutual_Horizontal_SplitDist", _value.ToString());
                    bChangeByManual = false;
                }
            }
            else if(_sender == splitContainer_MainBlock)
            {
                if(bChangeByManual)
                {
                    var _value = (int)(((double)splitContainer_MainBlock.SplitterDistance) / ((double)splitContainer_MainBlock.Width) * 100 + 0.5);
                    CDC_Tool_INI.IniWriteValue("SplitDistance", "Mutual_Vertical_SplitDist", _value.ToString());
                    bChangeByManual = false;
                }
            }
            else if(_sender == splitContainer_SelfKey_X)
            {
                if(splitContainer_SelfKey_Y.Panel2Collapsed || splitContainer_SelfKey_Y.Panel1Collapsed)
                    return;
                if(splitContainer_SelfKey_Y.SplitterDistance != e.SplitY)
                    splitContainer_SelfKey_Y.SplitterDistance = e.SplitY;
                if(bChangeByManual)
                {
                    var _value = (int)(((double)splitContainer_SelfKey_Y.SplitterDistance) / ((double)splitContainer_SelfKey_Y.Height) * 100 + 0.5);
                    CDC_Tool_INI.IniWriteValue("SplitDistance", "Key_Horizontal_SplitDist", _value.ToString());
                    bChangeByManual = false;
                }
            }
            else if(_sender == splitContainer_SelfKey_Y)
            {
                if(splitContainer_SelfKey_X.Panel2Collapsed || splitContainer_SelfKey_X.Panel1Collapsed)
                    return;
                if(splitContainer_SelfKey_X.SplitterDistance != e.SplitY)
                    splitContainer_SelfKey_X.SplitterDistance = e.SplitY;
                if(bChangeByManual)
                {
                    var _value = (int)(((double)splitContainer_SelfKey_X.SplitterDistance) / ((double)splitContainer_SelfKey_X.Height) * 100 + 0.5);
                    CDC_Tool_INI.IniWriteValue("SplitDistance", "Key_Horizontal_SplitDist", _value.ToString());
                    bChangeByManual = false;
                }
            }
            else if(_sender == splitContainer_SelfKey)
            {
                if(bChangeByManual)
                {
                    var _value = (int)(((double)splitContainer_SelfKey.SplitterDistance) / ((double)splitContainer_SelfKey.Width) * 100 + 0.5);
                    CDC_Tool_INI.IniWriteValue("SplitDistance", "Key_Vertical_SplitDist", _value.ToString());
                    bChangeByManual = false;
                }
            }
        }

        private void CDCToolfm_VisibleChanged(object sender, EventArgs e)
        {
            SetMutualSplitterDistance();
        }

        private void CDCToolfm_SizeChanged(object sender, EventArgs e)
        {
            SetMutualSplitterDistance();
        }

        private bool bChangeByManual = false;

        private void SplitContainer_MutualAdjust_MouseUp(object sender, MouseEventArgs e)
        {
            bChangeByManual = true;
        }
    }
}