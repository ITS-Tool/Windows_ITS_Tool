using CommonExt;
using FuncMethodCmd;
using FuncMethodCmd.Tools.ProcessorTools;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ThirdPartyTools;

namespace ITS_Studio
{
    public partial class TuningToolfm : Form
    {
        private int m_iSelectedDev = 0;
        public int SelectedDev { set { m_iSelectedDev = value; } get { return m_iSelectedDev; } }
        private UIPage MyUIPage = new UIPage();

        private Dictionary<enWM_MSG, function> mMessageReceiver = new Dictionary<enWM_MSG, function>();
        private INI_Tool MY_SettingsINI = new INI_Tool(ITS_Directory.Setting_INI);
        protected DynamicTool MyTool = new DynamicTool();
        private bool m_btnReadParam_Click_done = false;//確定ReadParam已作動完成
        public struct Page
        {
            public TabPage m_tabPg_TuningPage;
            public DataGridView m_dg_TuningParameter;
            public DataGridViewTextBoxColumn[] m_dgtb_Column;
        }

        private Page[] stuPageControl = null;
        
        public TuningToolfm()
        {
            InitializeComponent();

            //StaticVar.Tuning_WaitUIPanelHandle = this.panel_Wait.Handle;
            StaticVar.TuningFormHandle = this.Handle;

            mMessageReceiver.Add(enWM_MSG.WM_Tuning_Message, OnUpdateRichTextBox);
            mMessageReceiver.Add(enWM_MSG.WM_Tuning_UI_Init, OnTuning_UI_Init);

        }
        
        protected override void WndProc(ref Message msg)
        {
            enWM_MSG pEnumPanoMsg = (enWM_MSG)msg.Msg;
            if (mMessageReceiver.Keys.Contains(pEnumPanoMsg))
                mMessageReceiver[pEnumPanoMsg](ref msg);
            base.WndProc(ref msg);
        }

        private void OnUpdateRichTextBox(ref Message msg)
        {
            string strMsg = Marshal.PtrToStringAuto(msg.WParam);
            Color wordColor = Color.FromArgb((Int32)msg.LParam);
            UpdateRichTextBox(UI_GBV.ToolMain.m_RichText_ERR_MSG, strMsg, wordColor);
        }

        private void OnTuning_UI_Init(ref Message msg)
        {
            vfGUIInit();
        }

        private void UpdateRichTextBox(RichTextBox rtxbox, string msg, Color color)
        {
            if (msg == null || rtxbox == null)
                return;

            if (rtxbox.InvokeRequired)
            {
                rtxbox.Invoke((EventHandler)delegate
                {
                    UpdateRichTextBox(rtxbox, msg, color);
                });
            }
            else
            {
                if (rtxbox.Lines.Length - 2 >= 0 && msg == rtxbox.Lines[rtxbox.Lines.Length - 2])
                    return;
                //set HideSelection to false it will keep its selection when it loses focus and stay auto-scrolled.
                rtxbox.HideSelection = false;

                rtxbox.SelectionStart = rtxbox.TextLength;
                rtxbox.SelectionLength = 0;
                rtxbox.Focus();
                rtxbox.SelectionColor = color;
                rtxbox.AppendText(msg);
                rtxbox.SelectionColor = rtxbox.ForeColor;
            }
        }

        public bool vbInit()
        {
            MyUIPage.TopLevel = true;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.bRunningStatus = true;

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.bReadParams_done == false || UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.m_ReEnum_Item_Dic_Record.Count() == 0)
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXMLGroups.Count != 0)
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXMLGroups.Clear();

                if(!UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.bfLoadTPL())
                {
                    string message = "";
                    if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                        message = global::ITS_Studio.Properties.Resources.LoadingTPLFailure_EN;
                    else                    
                        message = global::ITS_Studio.Properties.Resources.LoadingTPLFailure_CN;
                    
                    //MyUIPage.ShowErrorDialog("Tuning Tool", "Loading TPL file was failure!", UIStyle.Red, false);
                    MyUIPage.ShowErrorDialog("Tuning Tool", message, UIStyle.Red, false);
                    return false;
                }

                m_btnReadParam_Click(null, null);
                while (!m_btnReadParam_Click_done)
                    System.Threading.Thread.Sleep(100);//因為read parameters 用background worker完成，需要使用flag來識別read parameters做完沒。
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.bReadParams_done = true;
            }

            ctrl_tuningPage_button(true);

            //check_enableAutoScanChBTN();

            Match rxMatch = Regex.Match(UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.strTPLPathandFileName, @".*\\(.*).tpl");
            if (rxMatch.Success)
                m_tb_TPLName.Text = rxMatch.Groups[1].ToString() + ".tpl";

            return true;
        }

        public void vfGUIInit()
        {
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXML.Count == 0)
                return;
            bool b3ModeShareVisible = MY_SettingsINI.IniReadBool("TUNING", "3Mode_Share_Visible", false);
            bool bAllItemVisble = MY_SettingsINI.IniReadBool("TUNING", "Item_All_Visible", false);
            int iCnt = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXMLGroups.Count * UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWSupportModes;
            stuPageControl = new Page[iCnt];
            m_tbTuningParameter.Visible = false;
            m_tbTuningParameter.SuspendLayout();
            m_tbTuningParameter.TabPages.Clear();

            int TotalMode = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWSupportModes;
            int StructureNum = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXML[UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXML.Count - 1].inPageID;

            int PageIndex = 0;

            int[,] ParaIndex = new int[UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWSupportModes, UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXMLGroups.Count];

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWEnabledModes == 1)
            {
                this.cbx_Tuning_Mode.Visible = false;
                this.m_butCommitParam.Visible = false;
                this.Lbl_ModeSelect.Visible = false;
                this.gBox_TuningMode.Visible = false;
                splitContainer_Functions.Panel2Collapsed = true;
                tabControl_Bottom.SelectedTab = tabPage1;
            }
            else
            {
                if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWEnabledModes == 3) && (this.cbx_Tuning_Mode.Items.Count != 3))
                {
                    this.cbx_Tuning_Mode.Items.Add("2");
                }

                this.cbx_Tuning_Mode.Visible = true;
                this.m_butCommitParam.Visible = true;
                this.Lbl_ModeSelect.Visible = true;
                this.gBox_TuningMode.Visible = true;
                splitContainer_Functions.Panel2Collapsed = false;
                tabControl_Bottom.SelectedTab = tabPage2;
            }
            this.cbx_Tuning_Mode.SelectedIndex = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWCurrentModeNumber;

            //update xml page to group
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.Updata_group(StructureNum, TotalMode, ParaIndex);

            for (int inGroup = 0; inGroup < UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXMLGroups.Count; inGroup++)
            {
                for (int inMode = 0; inMode < TotalMode; inMode++)
                {
                    PageIndex = inGroup * UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWSupportModes + inMode;
                    // Init Struct & Control Box
                    stuPageControl[PageIndex].m_tabPg_TuningPage = new TabPage();
                    stuPageControl[PageIndex].m_dg_TuningParameter = new DataGridView();
                    stuPageControl[PageIndex].m_dg_TuningParameter.TabIndex = PageIndex;

                    stuPageControl[PageIndex].m_dg_TuningParameter.DataError += delegate(object sender, DataGridViewDataErrorEventArgs e)
                    {
                    };
                    stuPageControl[PageIndex].m_dg_TuningParameter.AllowUserToAddRows = false;
                    stuPageControl[PageIndex].m_dg_TuningParameter.AllowUserToDeleteRows = false;
                    stuPageControl[PageIndex].m_dg_TuningParameter.AllowUserToResizeRows = false;
                    stuPageControl[PageIndex].m_dg_TuningParameter.MultiSelect = false;
                    stuPageControl[PageIndex].m_dg_TuningParameter.RowHeadersVisible = false;
                    stuPageControl[PageIndex].m_dg_TuningParameter.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

                    stuPageControl[PageIndex].m_dg_TuningParameter.CellBeginEdit += m_dg_TuningParameter_CellBeginEdit;
                    stuPageControl[PageIndex].m_dg_TuningParameter.CellEndEdit += m_dg_TuningParameter_CellEndEdit;
                    stuPageControl[PageIndex].m_dg_TuningParameter.SelectionChanged += m_dg_TuningParameter_SelectionChanged;

                    stuPageControl[PageIndex].m_dgtb_Column = new DataGridViewTextBoxColumn[2];
                    stuPageControl[PageIndex].m_dgtb_Column[0] = new DataGridViewTextBoxColumn();
                    stuPageControl[PageIndex].m_dgtb_Column[0].HeaderText = "Item";
                    stuPageControl[PageIndex].m_dgtb_Column[0].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                    stuPageControl[PageIndex].m_dgtb_Column[0].ReadOnly = true;

                    stuPageControl[PageIndex].m_dgtb_Column[1] = new DataGridViewTextBoxColumn();
                    stuPageControl[PageIndex].m_dgtb_Column[1].HeaderText = "Value";
                    stuPageControl[PageIndex].m_dgtb_Column[1].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

                    stuPageControl[PageIndex].m_tabPg_TuningPage = new TabPage();
                    stuPageControl[PageIndex].m_tabPg_TuningPage.Controls.Add(stuPageControl[PageIndex].m_dg_TuningParameter);
                    stuPageControl[PageIndex].m_tabPg_TuningPage.Text = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].strPageName;

                    stuPageControl[PageIndex].m_dg_TuningParameter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                    stuPageControl[PageIndex].m_dgtb_Column[0],
                    stuPageControl[PageIndex].m_dgtb_Column[1]});

                    stuPageControl[PageIndex].m_dg_TuningParameter.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
                    stuPageControl[PageIndex].m_dg_TuningParameter.Dock = System.Windows.Forms.DockStyle.Fill;

                    for (int inCount = 0; inCount < UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar.Count; inCount++)
                    {
                        stuPageControl[PageIndex].m_dg_TuningParameter.Rows.Add();

                        if ((inMode > 0) && (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].boShare == true) && (b3ModeShareVisible == false))
                            stuPageControl[PageIndex].m_dg_TuningParameter.Rows[inCount].Visible = false;
                        else
                            stuPageControl[PageIndex].m_dg_TuningParameter.Rows[inCount].Visible = bAllItemVisble ? true : UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].boVisible;


                        stuPageControl[PageIndex].m_dg_TuningParameter[0, inCount].Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].strName;
                        stuPageControl[PageIndex].m_dg_TuningParameter[1, inCount].Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].alValue[0];

                        if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].inShowType == ShowType.COMBOBOX)
                        {
                            stuPageControl[PageIndex].m_dg_TuningParameter[1, inCount].Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].alValue[0];
                            DataGridViewComboBoxCell DGVCBC = new DataGridViewComboBoxCell();
                            DGVCBC.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            DGVCBC.DisplayStyleForCurrentCellOnly = true;
                            DGVCBC.Items.Clear();

                            for (int inItemCount = 1; inItemCount < UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].alValue.Count; ++inItemCount)
                            {
                                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].inFormat == ValueFormat.HEX)
                                    DGVCBC.Items.Add(string.Format("0x{0:X}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].alValue[inItemCount]));// Hex
                                else
                                    DGVCBC.Items.Add(UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].alValue[inItemCount].ToString());// Dec and Bin
                            }

                            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].inFormat == ValueFormat.HEX)
                                DGVCBC.Value = string.Format("0x{0:X}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].alValue[0]);
                            else
                                DGVCBC.Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].alValue[0].ToString();

                            stuPageControl[PageIndex].m_dg_TuningParameter[1, inCount] = DGVCBC;
                        }
                        else
                        {
                            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].inFormat == ValueFormat.HEX)
                                stuPageControl[PageIndex].m_dg_TuningParameter[1, inCount].Value = string.Format("0x{0:X}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].alValue[0]);
                            else
                                stuPageControl[PageIndex].m_dg_TuningParameter[1, inCount].Value = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[PageIndex].lsTuningVar[inCount].alValue[0].ToString();
                        }
                    }
                }
            }

            // Add Page in GUI TabControl
            for (int inPage = 0; inPage < iCnt; inPage++)
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[inPage].inModeID == (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWCurrentModeNumber + 1))
                    if (!stuPageControl[inPage].m_tabPg_TuningPage.Text.Equals("NA"))
                        m_tbTuningParameter.Controls.Add(stuPageControl[inPage].m_tabPg_TuningPage);
            }

            m_tbTuningParameter.ResumeLayout(true);
            m_tbTuningParameter.Visible = true;
        }
    
        private void m_dg_TuningParameter_SelectionChanged(object sender, EventArgs e)
        {
            var MySender = sender as DataGridView;
            //MySender.Visible = false;
            int rowCnt = MySender.RowCount;

            var RowIndex = MySender.CurrentRow.Index;
            if ((RowIndex < MySender.RowCount) & (RowIndex >= 0))
            {
                for (int i = 0; i < rowCnt; i++)
                {
                    if (i == RowIndex)
                    {
                        MySender[0, i].Style.BackColor = Color.Blue;
                        MySender[0, i].Style.ForeColor = Color.White;
                        richTextBox1.Text = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[MySender.TabIndex].lsTuningVar[i].strHint;
                    }
                    else
                    {
                        MySender[0, i].Style.BackColor = Color.FromArgb(240, 240, 240);
                        MySender[0, i].Style.ForeColor = Color.Black;
                    }
                }
            }
        }

        private object tmpOriValue = null;

        private void m_dg_TuningParameter_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var _GridView = sender as DataGridView;
            tmpOriValue = _GridView.CurrentCell.FormattedValue;
        }

        private void m_dg_TuningParameter_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var _GridView = sender as DataGridView;
            uint inCellValue = 0;

            if (e.ColumnIndex == 0)
                return;
            var _value = _GridView.CurrentCell.EditedFormattedValue;
            if (_value.GetType().Equals(typeof(string)))
            {
                if (string.IsNullOrEmpty(_value.ToString()))
                {
                    //MyUIPage.ShowInfoDialog("Tuning Tool", "Please input value!", UIStyle.Orange, false);
                    MyUIPage.ShowErrorTip("value is null", 1000, true);
                    _GridView.CurrentCell.Value = tmpOriValue;
                    return;
                }
            }

            // Get Page Value
            int inPageValue = _GridView.TabIndex;
            int inSelcetCellIndex = e.RowIndex;

            var TuningParameter = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[inPageValue].lsTuningVar[inSelcetCellIndex];
            // Find select Item String in lsGroup Array
            if (stuPageControl[inPageValue].m_dg_TuningParameter[0, inSelcetCellIndex].Value.ToString() != TuningParameter.strName)
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, "inSelcetCellIndex not match!" + Environment.NewLine, Color.Green.ToArgb());
                return;
            }

            switch (TuningParameter.inShowType)
            {
                case ShowType.COMBOBOX:
                    inCellValue = uint.Parse(_GridView.CurrentCell.FormattedValue.ToString());
                    break;
                case ShowType.TEXTBOX:
                    string strTmp = _GridView.CurrentCell.FormattedValue.ToString().ToUpper();
                    var _CellValue = strTmp.StartsWith("0X") ? strTmp.Substring(2) : strTmp;

                    if (TuningParameter.inFormat == ValueFormat.HEX)   //Hex
                    {
                        if (!StringTool.IsHex(_CellValue) || _CellValue.Count() > 8)
                        {
                            MyUIPage.ShowErrorTip(string.Format("{0} is not a hexadecimal character", _CellValue), 1000, false);
                            _GridView.CurrentCell.Value = tmpOriValue;
                            vfGUIInit();
                            return;
                        }

                        inCellValue = (uint.Parse(_CellValue, System.Globalization.NumberStyles.AllowHexSpecifier));
                    }
                    else
                    {
                        if (!StringTool.IsDecimal(_CellValue) || _CellValue.Count() > 8)
                        {
                            MyUIPage.ShowErrorTip(string.Format("{0} is not a decimal character", _CellValue), 1000, false);
                            _GridView.CurrentCell.Value = tmpOriValue;
                            vfGUIInit();
                            return;
                        }

                        inCellValue = (uint.Parse(_CellValue));
                    }
                    break;
            }

            //===================================
            //檢查Range有沒有超過範圍
            //===================================
            if (inCellValue > TuningParameter.inMax)
            {
                MyUIPage.ShowErrorDialog("Tuning Tool", "Input value greater than MAX value!", UIStyle.Orange, false);
                inCellValue = TuningParameter.inMax;
                TuningParameter.alValue[0] = (int)inCellValue;
                stuPageControl[inPageValue].m_dg_TuningParameter[1, inSelcetCellIndex].Value = inCellValue;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message
                    , string.Format("Input value greater than MAX value!, MAX value is {0}", inCellValue) + Environment.NewLine, Color.Green.ToArgb());
                return;
            }

            if (inCellValue < TuningParameter.inMin)
            {
                MyUIPage.ShowErrorDialog("Tuning Tool", "Input value less than MIN value!", UIStyle.Orange, false);
                inCellValue = TuningParameter.inMin;
                stuPageControl[inPageValue].m_dg_TuningParameter[1, inSelcetCellIndex].Value = inCellValue;
                TuningParameter.alValue[0] = (int)inCellValue;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message
                    , string.Format("Input value less than MIN value!, MIN value is {0}", inCellValue) + Environment.NewLine, Color.Green.ToArgb());
                return;
            }

            TuningParameter.alValue[0] = (int)inCellValue;

            //TuningParameter.strGroup
            //UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.vbSysInfoUpdate(m_tbTuningParameter.SelectedIndex, inSelcetCellIndex, (int)inCellValue); //Share value changed
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.vbSysInfoUpdate(TuningParameter.strGroup, inSelcetCellIndex, (int)inCellValue); //Share value changed
        }

        public void vfStopTuning()
        {
            this.Invoke(new MethodInvoker(delegate()
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.bRunningStatus = false;
                    ctrl_tuningPage_button(false);
                }));
        }

        private void ctrl_tuningPage_button(bool status)
        {
            m_butWriteParam.Enabled = status;
            m_butWriteParam.Cursor = status ? Cursors.Hand : Cursors.Default;

            m_butReadParam.Enabled = status;
            m_butReadParam.Cursor = status ? Cursors.Hand : Cursors.Default;

            m_butTuningCompare.Enabled = status;
            m_butTuningCompare.Cursor = status ? Cursors.Hand : Cursors.Default;

            m_btMergeToHex.Enabled = status;
            m_btMergeToHex.Cursor = status ? Cursors.Hand : Cursors.Default;

            m_butCommitParam.Enabled = status;
            m_butCommitParam.Cursor = status ? Cursors.Hand : Cursors.Default;

            ////20220810
            //btn_FreSpectrum.Enabled = true;
            //btn_FreSpectrum.Cursor = status ? Cursors.Hand : Cursors.Default;

            //btnChargDump.Enabled = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3) ? false : true;
            //btnChargDump.Cursor = (btnChargDump.Enabled) ? Cursors.Hand : Cursors.Default;

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo == null)
            {
                m_btScanCH.Enabled = false;
                m_btScanCH.Cursor = Cursors.Default;
                return;
            }

            var Now_MCU = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU;

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)//20211228 Steven修改判斷Protocol Type 取代MCU Name(Kernel Version)
            {
                //bool bEnable = (Now_MCU != (int)V6_MCU_Dual.IC_2326);
                bool bEnable = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iIC_Counts <= 1);
                //bEnable &= (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU != 0);
                bEnable &= !string.IsNullOrEmpty(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU);
                bEnable &= status;
                m_btScanCH.Enabled = bEnable;
                m_btScanCH.Cursor = bEnable ? Cursors.Hand : Cursors.Default;
            }
            else
            {
                //if (!Enum.IsDefined(typeof(OldV3_MCU), (int)Now_MCU))
                if (!EnumTool.IsEnumDefined<OldV3_MCU>(Now_MCU))
                {
                    byte[] ReturnValue;
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.Get_CoreVersion_API(out ReturnValue);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.CoreVersion.AddRange(ReturnValue);

                    if (ReturnValue[0] == 6 && ReturnValue[1] == 9)
                    {
                        m_btScanCH.Enabled = false;
                        m_btScanCH.Cursor = Cursors.Default;
                    }
                    else
                    {
                        m_btScanCH.Enabled = status;
                        m_btScanCH.Cursor = status ? Cursors.Hand : Cursors.Default;
                    }
                }
                else
                {
                    m_btScanCH.Enabled = status;
                    m_btScanCH.Cursor = status ? Cursors.Hand : Cursors.Default;
                }
            }
            m_tbTuningParameter.Visible = status;
        }

        private void m_butWriteParam_Click(object sender, EventArgs e)
        {
            var bResult = false;
            string errMessage = "";
            string msg = "";

            UI_GBV.fmITS_Tool.MainButton_Disable();
            ctrl_tuningPage_button(false);

            string strErrMessage = "", strmessage = "";
            splitContainer_Functions.Panel1.Enabled = false;
            splitContainer_Functions.Panel2Collapsed = false;
            tabControl_Bottom.SelectedTab = tabPage1;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.SHOW, (IntPtr)this.panel1.Handle);
            //UnRegister PNP
            DEV_INTERFACE_TYPE tmpINTERFACE_TYPE = StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType;
            bool bProtocolOver604 = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum >= Ver.AP_604;
            bool bProtocolTypeV3 = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3);

            //if (bProtocolOver604 || bProtocolTypeV3)
            if ((tmpINTERFACE_TYPE == DEV_INTERFACE_TYPE.HID) || (tmpINTERFACE_TYPE == DEV_INTERFACE_TYPE.HID_BL))
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_UsbDeviceNotification, (IntPtr)m_iSelectedDev, IntPtr.Zero);
            bResult = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.WriteParams(ref errMessage);
            if (bResult)
            {
                do
                {
                    MyTool.Delay_ms(2000);
                    if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.m_bneedtoReEnum == false)
                    {
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.SuspendMode, Command._SWITCH.Dis);
                        var IC_Mode = UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode;
                        bResult = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.Get_IC_Information(IC_Mode, false, ref strmessage, ref strErrMessage);
                        Color _color = Color.Green;
                        if(bResult)
                        {
                            _color = Color.Green;
                            msg = (StaticVar.g_Lang == enLangTable.LANG_ENGLISH) ? "Writing parameters was successful!\n" : "參數寫入成功\n";
                        }
                        else
                        {
                            _color = Color.Red;
                            msg = (StaticVar.g_Lang == enLangTable.LANG_ENGLISH) ? "Writing parameters was successful, but getting IC information was failure!\n" : "參數寫入成功，但取得IC資訊失敗!\n";                            
                        }
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, msg, _color.ToArgb());
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
                    }
                    MyTool.Delay_ms(1000);
                    vfGUIInit();
                    
                    if((tmpINTERFACE_TYPE != DEV_INTERFACE_TYPE.WIFI_HID) && (tmpINTERFACE_TYPE != DEV_INTERFACE_TYPE.WIFI_I2C))
                    {
                        if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.m_bneedtoModifyINI)
                        {
                            MY_SettingsINI.IniWriteValue("DEVICE_ID", "VID", string.Format("{0:X}", Convert.ToUInt32(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.m_ReEnum_Item_Dic_Record[TuningTool.en_ReEnumItem.USB_VID.Desc()])));
                            if(!StaticVar.m_VID_Filter.Contains(Convert.ToUInt16(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.m_ReEnum_Item_Dic_Record[TuningTool.en_ReEnumItem.USB_VID.Desc()])))
                                StaticVar.m_VID_Filter.Add(Convert.ToUInt16(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.m_ReEnum_Item_Dic_Record[TuningTool.en_ReEnumItem.USB_VID.Desc()]));
                        }
                    }

                    //TODO: [Joe] 確認一下這邊需不需要把Wifi 也加進去
                    if((tmpINTERFACE_TYPE != DEV_INTERFACE_TYPE.HID) && (tmpINTERFACE_TYPE != DEV_INTERFACE_TYPE.HID_BL))
                    {
                        //20230516 Steven : 走Bridge是不會需要ReEnum裝置的，所以要補上Get_IC_Information避免Painting Mapping Resolution失敗
                        if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.m_bneedtoReEnum)
                        {
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

                            StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus = DevStatus.Connected;
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.SuspendMode, Command._SWITCH.Dis);
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.CheckDevMode_API(FW_MODE.AP_MODE);
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.Get_IC_Information(UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode, false, ref strmessage, ref strErrMessage);
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
                        }

                        msg = (StaticVar.g_Lang == enLangTable.LANG_ENGLISH) ?
                            "Writing parameters was successful!" :
                            "參數寫入成功!";
                        MyUIPage.ShowInfoDialog("Tuning Tool", msg, UIStyle.Orange, false);
                        break;
                    }

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.FirmwareUpgradeFormHandle, enWM_MSG.WM_Program_Status
                        , (IntPtr)enSTATUS_.SHOW, (IntPtr)enSTATUS_.RECONNECTING);

                    if(!UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.m_bneedtoReEnum)
                    {
                        msg = (StaticVar.g_Lang == enLangTable.LANG_ENGLISH) ?
                            "Writing parameters was successful!" :
                            "參數寫入成功!";
                        MyUIPage.ShowInfoDialog("Tuning Tool", msg, UIStyle.Orange, false);
                        break;
                    }

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message
                        , "Start to ReEnum USB device!\r\n", Color.Green.ToArgb());
                    bResult = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.ReEnumUSB_Change_Vid(ref strmessage);
                    //20220831 Steven Add
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message
                        , "End to ReEnum USB device!\r\n", Color.Green.ToArgb());
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode = (bResult == true) ? FW_MODE.AP_MODE : FW_MODE.BL_MODE;
                    if(!bResult)
                    {
                         msg = (StaticVar.g_Lang == enLangTable.LANG_ENGLISH) ? 
                            "Writing Parameters was failed(cause: re-enumeration usb)" : 
                            "參數寫入失敗!(重列舉裝置失敗)";
                        MyUIPage.ShowErrorDialog("Tuning Tool", msg, UIStyle.Red, false);
                        break;
                    }

                    //20220831 Steven Add
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

                    StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus = DevStatus.Connected;

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_Program_UI_Status
                        , (IntPtr)UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode, (IntPtr)0);

                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.SuspendMode, Command._SWITCH.Dis);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.CheckDevMode_API(FW_MODE.AP_MODE);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.Get_IC_Information(UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode, false, ref strmessage, ref strErrMessage);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
                    MyUIPage.ShowInfoDialog("Tuning Tool", (StaticVar.g_Lang == enLangTable.LANG_ENGLISH) ? "Writing Parameters was Successful!" : "參數寫入成功!", UIStyle.Orange, false);
                    
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.m_bneedtoReEnum = false;

                } while(false);
            }
            else
            {
                if(!bProtocolOver604) //當Write失敗時，Protocol 604以前不需要下65 02 Get CRC，在這邊需要分開判斷
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, string.Format("{0}\n", errMessage), Color.Red.ToArgb());
                    MyUIPage.ShowErrorDialog("Tuning Tool", "Writing parameters was a failed!", UIStyle.Orange, false);
                }
                else
                {
                    msg = "";
                    switch(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.m_write_param_CRC_ret)
                    {
                        case TuningTool.en_V6_Para_CRC_Ret.Modify_ByFw:
                            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                                msg = "Parameters were modified by firmware, the tool will read the newest parameters from FW.";
                            else
                                msg = "FW參數已修改成功，Tool將重新讀取FW參數!\n";
                            MyUIPage.ShowErrorDialog("Tuning Tool", msg, UIStyle.Orange, false);
                            m_btnReadParam_Click(null, null);
                            while (!m_btnReadParam_Click_done)
                                System.Threading.Thread.Sleep(100);//因為read parameters 用background worker完成，需要使用flag來識別read parameters做完沒。
                            //bResult = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.Get_IC_Information(UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode, false, ref strmessage, ref strErrMessage);
                            bResult = true;

                            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                                msg = (bResult) ? "Reading parameters was successful!\n" : "Reading parameters was successful, but getting IC information was a failed!\n";
                            else
                                msg = (bResult) ? "參數讀取成功!\n" : "參數讀取成功, 但取得IC資訊失敗!\n";
                            Color _col = (bResult) ? Color.Green : Color.Red;
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, msg, _col.ToArgb());
                            
                            break;
                        
                        case TuningTool.en_V6_Para_CRC_Ret.Transfer_Err:
                            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                                msg = "Transfer error\n";
                            else
                                msg = "傳送失敗\n";
                            MyUIPage.ShowErrorDialog("Tuning Tool", msg, UIStyle.Orange, false);
                            break;
                        
                        default:
                        case TuningTool.en_V6_Para_CRC_Ret.OK:
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, string.Format("{0}\n", errMessage), Color.Red.ToArgb());
                            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                                msg = "Write Parameters was failure\n";
                            else
                                msg = "寫入參數失敗\n";
                            MyUIPage.ShowErrorDialog("Tuning Tool", msg, UIStyle.Orange, false);
                            break;
                    }
                }
            }

            if (Wifi_Var.useWifi)
                Wifi_Var.Update_Panelinfo();

            /* update commonflow's panel info. */
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.m_HID_API.HID_CreateHandle(
                StaticVar.lsMain_USBDevInfo[m_iSelectedDev].strDevPath);
            CommonFlow.api_update_ts_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev);
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.commonflow_get_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.info[m_iSelectedDev]);

            //if (bProtocolOver604 || bProtocolTypeV3)
            if ((tmpINTERFACE_TYPE == DEV_INTERFACE_TYPE.HID) || (tmpINTERFACE_TYPE == DEV_INTERFACE_TYPE.HID_BL))
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_UsbDeviceNotification, (IntPtr)m_iSelectedDev, StaticVar.MainFormHandle);

            splitContainer_Functions.Panel1.Enabled = true;
            UI_GBV.fmITS_Tool.Reset_UI();
            tabControl_Bottom.SelectedTab = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWEnabledModes == 1) ? tabPage1 : tabPage2;
            splitContainer_Functions.Panel2Collapsed = (tabControl_Bottom.SelectedTab == tabPage1) ? true : false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.HIDE, (IntPtr)this.panel1.Handle);
            ctrl_tuningPage_button(true);
        }

        private void bgwWorker_DoWork(out bool bResult, out string ErrMsg)
        {
            var _Mode = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6) ? TouchMode.SuspendMode : TouchMode.TestMode;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(_Mode, Command._SWITCH.Dis);

            string Msg = "";
            bResult = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.vbReadParameter(ref Msg);
            ErrMsg = Msg;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
            m_btnReadParam_Click_done = true;
        }

        private void bgwWorker_RunWorkerCompleted(object sender, bool bResult, string ErrMsg, bool PopErrMsg)
        {
            bool flag = true;
            string msg = "";
            UIStyle _uiStyle = bResult ? UIStyle.Orange : UIStyle.Red;
            if (bResult)            
                vfGUIInit();
            
            //if((sender == null) && !bResult)
            if(sender == null) 
            {
                flag = false;
                msg = ErrMsg;
            }
            else
            {
                if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    msg = (bResult) ? "Reading parameters was successful!\n" : "Failed to read parameters!\n" + ErrMsg;
                else
                    msg = (bResult) ? "參數讀取成功!\n" : "參數讀取失敗!\n";
            }

            Color _col = (bResult) ? Color.Green : Color.Red;            
            if(sender != null && PopErrMsg)            
                MyUIPage.ShowInfoDialog("Tuning Tool", msg, _uiStyle, true);
                
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, msg, _col.ToArgb());

            //UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);//移到bgwWorker_DoWork 20240111 evayn
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.HIDE, (IntPtr)this.panel1.Handle);
            ctrl_tuningPage_button(flag);
            //splitContainer_Functions.Panel2Collapsed = true;
            splitContainer_Functions.Panel1.Enabled = true;
            tabControl_Bottom.SelectedTab = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWEnabledModes == 1) ? tabPage1 : tabPage2;

        }

        private void m_btnReadParam_Click(object sender, EventArgs e)
        {
            //string strValue = "2316";
            //IntPtr ic_name = Marshal.StringToHGlobalAnsi(strValue);
            //strValue = "0.0.0.D";
            //IntPtr core = Marshal.StringToHGlobalAnsi(strValue);
            splitContainer_Functions.Panel1.Enabled = false;
            tabControl_Bottom.SelectedTab = tabPage1;
            splitContainer_Functions.Panel2Collapsed = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.SHOW, (IntPtr)this.panel1.Handle);
            bool PopErrMsg = (sender == null) ? true : false;
            
            //Binding delegate
            if(UI_GBV.fmITS_Tool != null)
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.eventFunc_UpdateRichTextBox -= UI_GBV.fmITS_Tool.UpdateRichTextBox;
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.eventFunc_UpdateRichTextBox += UI_GBV.fmITS_Tool.UpdateRichTextBox;
            }
            bool bResult = false;
            string ErrMsg = "";
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (_object, _EventArgs) => { bgwWorker_DoWork(out bResult, out ErrMsg); };
            worker.RunWorkerCompleted += (_object, _EventArgs) => { bgwWorker_RunWorkerCompleted(_object, bResult, ErrMsg, PopErrMsg); };

            m_btnReadParam_Click_done = false;

            worker.RunWorkerAsync();

        }

        private void m_butSetTuningMode_Click(object sender, EventArgs e)
        {
            //Binding delegate
            if(UI_GBV.fmITS_Tool != null)
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.eventFunc_UpdateRichTextBox -= UI_GBV.fmITS_Tool.UpdateRichTextBox;
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.eventFunc_UpdateRichTextBox += UI_GBV.fmITS_Tool.UpdateRichTextBox;
            }
            int mode = this.cbx_Tuning_Mode.SelectedIndex;
            string msg = "";
            bool bResult = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.SwitchTuningMode((byte)mode);
            if (bResult)
            {
                m_btnReadParam_Click("3mode", null);
                while (!m_btnReadParam_Click_done)
                    System.Threading.Thread.Sleep(100);//因為read parameters 用background worker完成，需要使用flag來識別read parameters做完沒。
            }
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                msg = bResult ? "Set Tuning Mode Successfully!\n" : "Set Tuning Mode Failure!\n";
            else
                msg = bResult ? "Tuning Mode 設定成功!\n" : "Tuning Mode 設定失敗!\n";
            UIStyle _uiStyle = bResult ? UIStyle.Orange : UIStyle.Red;
            MyUIPage.ShowInfoDialog("Tuning Tool", msg, _uiStyle, false);
            //MyUIPage.ShowInfoDialog("Tuning Tool", "Set Tuning Mode Successfully!", UIStyle.Orange, false);
        }

        private void cbx_Tuning_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_tbTuningParameter.TabPages.Clear();
            int mode = this.cbx_Tuning_Mode.SelectedIndex + 1;
            // Add Page in GUI TabControl
            int iCnts = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXMLGroups.Count * UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWSupportModes;
            for (int inPage = 0; inPage < iCnts; inPage++)
            {
                if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsGroup[inPage].inModeID == mode) && (stuPageControl[inPage].m_tabPg_TuningPage != null))
                {
                    if ((stuPageControl[inPage].m_tabPg_TuningPage.Text == "SysInfo") && (mode != 1))
                        stuPageControl[inPage].m_dg_TuningParameter.Enabled = false;

                    m_tbTuningParameter.Controls.Add(stuPageControl[inPage].m_tabPg_TuningPage);
                }
            }
        }

        #region Merge Tool
        private void m_btMergeToHex_Click(object sender, EventArgs e)
        {
            // Select Source Hex File            
            string msg = (StaticVar.g_Lang == enLangTable.LANG_ENGLISH) ? "Please select the code base hex file." : "請選擇基底Hex";

            if(!MyUIPage.ShowAskDialog(Application.ProductName, msg, UIStyle.Orange, true))
                return;
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Source Hex files (*.hex)|*.hex";
            openDlg.DefaultExt = "hex";
            openDlg.InitialDirectory = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.strTPLPath;
            openDlg.FileName = "";
            openDlg.CheckFileExists = true;
            openDlg.CheckPathExists = true;
            openDlg.Multiselect = false;
            openDlg.Title = "Source Hex File";
            if (openDlg.ShowDialog(this) != DialogResult.OK)
                return;

            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            msg = (StaticVar.g_Lang == enLangTable.LANG_ENGLISH) ? "Please select where the file ready to save" : "請選擇文件要儲存的位置";
            MyUIPage.ShowInfoDialog(Application.ProductName, msg, UIStyle.Orange, true);
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "Hex File|*.hex";
            saveDlg.Title = "Please select where the file ready to save?";
            saveDlg.InitialDirectory = ITS_Directory.Tool_CurrentDirectory;
            // If the file name is not an empty string open it for saving.
            if (saveDlg.ShowDialog(this) == DialogResult.OK && saveDlg.FileName != "")
            {
                if (saveDlg.FilterIndex == 1)
                {
                    string strSORHexFilePath = openDlg.FileNames.GetValue(0).ToString();// 將讀取到的檔案路徑塞給 "strFilePath"
                    string strSaveHexFilePath = saveDlg.FileName;
                    bool Result = vbSaveToHex(strSORHexFilePath, strSaveHexFilePath);
                    UIStyle _uiStyle = Result ? UIStyle.Orange:UIStyle.Red;
                    if(Result)                    
                        msg = (StaticVar.g_Lang == enLangTable.LANG_ENGLISH) ? "TPL To Hex Successfully!" : "TPL 轉 Hex 成功!";                    
                    else                    
                        msg = (StaticVar.g_Lang == enLangTable.LANG_ENGLISH) ? "TPL To Hex Failure!" : "TPL 轉 Hex 失敗!";                        
                    
                    MyUIPage.ShowInfoDialog("Tuning Tool", msg, _uiStyle, true);
                }
                return;
            }
        }

        private bool vbSaveToHex(string SourceHexFilePath, string strSaveHexFilePath)
        {
            string strErrorMSG = "";
            string Msg = "";
            //當merge to hex前，需要先去讀當前parameters
            var _Mode = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6) ? TouchMode.SuspendMode : TouchMode.TestMode;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(_Mode, Command._SWITCH.Dis);
            bool bResult = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.vbReadParameter(ref Msg);
            string ErrMsg = Msg;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
            //
            string iHexMCU = "";
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_.PreloadHexMemorymapping(SourceHexFilePath, ref iHexMCU);
            bool boReturnValue = false;
            if (!System.IO.File.Exists(SourceHexFilePath))
            {
                strErrorMSG += string.Format("Hex file is not found! file = {0}\r\n", SourceHexFilePath);
                    MyUIPage.ShowErrorDialog("Tuning Tool", strErrorMSG, UIStyle.Red, true);
                return boReturnValue;
            }

            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_.DecodeFile_API_forMerage2Hex(iHexMCU, SourceHexFilePath, ref strErrorMSG)!=0)
            {
                MyUIPage.ShowErrorDialog("Tuning Tool", strErrorMSG, UIStyle.Red, true);
                return false;
            }

            Hex_Processor HexProcess = new Hex_Processor(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow);
            HexProcess.ParserHexInfo();

            if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU != HexProcess.DecodeHexInfo.McuName)
                || (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.strTuningVersionTPL != UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.strTuningVersionHex))
            {
                MyUIPage.ShowErrorDialog("Tuning Tool", "Hex file incorrect!", UIStyle.Red, true);
                return false;
            }

            StringBuilder strTPLBuffer = new StringBuilder();
            Console.WriteLine("strTPLBuffer={0}\r\n", strTPLBuffer);
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.CompareTool.vbPreTuningDataToSave(ref strTPLBuffer, UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXML, HexProcess);

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.CompareTool.Combine_TuningToHex(strTPLBuffer, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.CodeBlock, UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXML, HexProcess, strSaveHexFilePath);
            return true;
        }
        #endregion Merge Tool

        private void m_butTuningCompare_Click(object sender, EventArgs e)
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_TuningChangeControl, (IntPtr)TuningPage.TuningCompare, (IntPtr)null);
            UI_GBV.fmITS_Tool.MainButton_Disable();
        }

        private void btn_FreSpectrum_Click(object sender, EventArgs e)
        {
            //20220510 Steven 修改順序，先創建Handle再ShowBarData
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.vbStartNoiseFre();

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_TuningChangeControl, (IntPtr)TuningPage.NoiseFrePage, (IntPtr)null);

            UI_GBV.fmITS_Tool.MainButton_Disable();
        }

        private void m_btScanCH_Click(object sender, EventArgs e)
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_TuningChangeControl, (IntPtr)TuningPage.AutoScanCHPage, (IntPtr)null);
            UI_GBV.fmITS_Tool.MainButton_Disable();
        }

        private void btnChargDump_Click(object sender, EventArgs e)
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_TuningChangeControl, (IntPtr)TuningPage.ChargeCurve, (IntPtr)null);
            UI_GBV.fmITS_Tool.MainButton_Disable();
        }
    }
}