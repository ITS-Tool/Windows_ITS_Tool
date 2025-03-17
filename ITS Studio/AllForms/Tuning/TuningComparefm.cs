using CommonExt;
using FuncMethodCmd;
using FuncMethodCmd.Tools.ProcessorTools;
using ITS_Studio.UserControls;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ThirdPartyTools;

namespace ITS_Studio
{
    public partial class TuningComparefm : Form
    {
        private enum _ModeStatus
        {
            NotSupported = 0xFF,
        };
        private int m_iSelectedDev = 0;
        public int SelectedDev { set { m_iSelectedDev = value; } get { return m_iSelectedDev; } }
        private UIPage MyUIPage = new UIPage();
        //private int tmp_iStartAddress = 0;
        private Hex_Processor HexProcess_Left = null;
        private Hex_Processor HexProcess_Right = null;
        private List<string>[] m_val_right = new List<string>[3];
        private List<string>[] m_val_left = new List<string>[3];
        private List<TuningVariable.XML_DATA> m_xml_right = null;
        private List<TuningVariable.XML_DATA> m_xml_left = null;

        private List<MultiFirmwareTool_VAR.HEX_CONTENT> CodeBlock_Left = null;
        private List<MultiFirmwareTool_VAR.HEX_CONTENT> CodeBlock_Right = null;
        private Ctrl_TuningCompare UserCtl_Left = new Ctrl_TuningCompare(Ctrl_TuningCompare.EnumAlignSide._LEFT_);
        private Ctrl_TuningCompare UserCtl_Right = new Ctrl_TuningCompare(Ctrl_TuningCompare.EnumAlignSide._RIGHT_);

        private enum _BITS_ : int
        { _8_ = 1, _16_ = 2, _32_ = 4 };

        private enum _Column
        {
            _PAGE_INDEX_ = 0,
            _ITEM_INDEX_ = 1,
            _MAX_VALUE_ = 2,
            _MIN_VALUE_ = 3,
            _ADDRESS_ = 4,
            _PAGE_NAME_ = 5,
            _ITEM_NAME_ = 6,
            _VALUE_ = 7,
            _CORRESPOND_INDEX_ = 8,
            _FORMAT_ = 9,
        };

        private enum TuningFormat { _16 = 0, _10 = 1 };

        //private int _4Byte_Addr
        //{
        //    set { tmp_iStartAddress = value; }
        //    get { return (tmp_iStartAddress & 0xFFFF); }
        //}

        public TuningComparefm()
        {
            InitializeComponent();
            this.Text = this.Text + string.Format(" V{0}", Application.ProductVersion);

            UserCtl_Left.DGV_SameList.Scroll += DGV_SameList_Scroll;
            UserCtl_Left.DGV_DiffList.Scroll += DGV_SameList_Scroll;
            UserCtl_Left.DGV_AddList.CellValidating += DGV_CellValidating;
            UserCtl_Left.DGV_DiffList.CellValidating += DGV_CellValidating;
            UserCtl_Left.DGV_SameList.CellValidating += DGV_CellValidating;
            UserCtl_Left.DGV_AddList.CellValueChanged += DGV_CellValueChanged;
            UserCtl_Left.DGV_DiffList.CellValueChanged += DGV_CellValueChanged;
            UserCtl_Left.DGV_SameList.CellValueChanged += DGV_CellValueChanged;

            UserCtl_Left.DGV_SameList.MultiSelect = true;
            UserCtl_Left.DGV_SameList.SelectionChanged += new EventHandler(DGV_L_SelectionChanged);
            UserCtl_Right.DGV_SameList.MultiSelect = true;
            UserCtl_Right.DGV_SameList.SelectionChanged += new EventHandler(DGV_R_SelectionChanged);
            //

            UserCtl_Left.DGV_DiffList.MultiSelect = true;
            UserCtl_Left.DGV_DiffList.SelectionChanged += new EventHandler(DGV_L_SelectionChanged);
            UserCtl_Right.DGV_DiffList.MultiSelect = true;
            UserCtl_Right.DGV_DiffList.SelectionChanged += new EventHandler(DGV_R_SelectionChanged);
            //
            //UserCtl_Left.DGV_DiffList.CellMouseDown += DGV_CellMouseDown;
            //UserCtl_Left.DGV_SameList.CellMouseDown += DGV_CellMouseDown;

            UserCtl_Right.DGV_SameList.Scroll += DGV_SameList_Scroll;
            UserCtl_Right.DGV_DiffList.Scroll += DGV_SameList_Scroll;
            UserCtl_Right.DGV_AddList.CellValidating += DGV_CellValidating;
            UserCtl_Right.DGV_DiffList.CellValidating += DGV_CellValidating;
            UserCtl_Right.DGV_SameList.CellValidating += DGV_CellValidating;
            UserCtl_Right.DGV_AddList.CellValueChanged += DGV_CellValueChanged;
            UserCtl_Right.DGV_DiffList.CellValueChanged += DGV_CellValueChanged;
            UserCtl_Right.DGV_SameList.CellValueChanged += DGV_CellValueChanged;
            //UserCtl_Right.DGV_DiffList.CellMouseDown += DGV_CellMouseDown;
            //UserCtl_Right.DGV_SameList.CellMouseDown += DGV_CellMouseDown;

            //UserCtl2.DGV_AddList.Tag = UserCtl2.DGV_DiffList.Tag = UserCtl2.DGV_SameList.Tag = "UserCtl2";
            HexProcess_Left = null;
            HexProcess_Right = null;
            CodeBlock_Left = new List<MultiFirmwareTool_VAR.HEX_CONTENT>();
            CodeBlock_Right = new List<MultiFirmwareTool_VAR.HEX_CONTENT>();

            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_ == null)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_ = new FirmwareUpgrade(SelectedDev, UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow);

            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.CompareTool == null)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.CompareTool = new TuningCompareTool(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow);

            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_ == null)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_ = new TuningTool(SelectedDev, ref UI_GBV.mDev[m_iSelectedDev].m_GBV);
            //
            for (int i = 0; i < m_val_right.Count(); i++)
            {
                m_val_right[i] = new List<string>();
                m_val_left[i] = new List<string>();
            }
            gb_Find.Enabled = false;
        }

        private void DGV_R_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView DGV = (DataGridView)sender;
            DataGridView DGV_L;
            if (DGV.Name == "DGV_DiffList")
                DGV_L = UserCtl_Left.DGV_DiffList;
            else
                DGV_L = UserCtl_Left.DGV_SameList;

            DGV_L.SelectionChanged -= new EventHandler(DGV_L_SelectionChanged);
             DGV_L.ClearSelection();
            foreach (DataGridViewCell cell in DGV.SelectedCells)
            {
                int rowIndex = cell.RowIndex;
                if (DGV_L.Rows.Count != 0)
                {
                    DGV_L.Rows[cell.RowIndex].Selected = true;
                }
            }
            DGV_L.SelectionChanged += new EventHandler(DGV_L_SelectionChanged);
        }

        private void DGV_L_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView DGV = (DataGridView)sender;
            DataGridView DGV_R;
            if (DGV.Name == "DGV_DiffList")
                DGV_R = UserCtl_Right.DGV_DiffList;
            else
                DGV_R = UserCtl_Right.DGV_SameList;
            DGV_R.SelectionChanged -= new EventHandler(DGV_R_SelectionChanged);
            DGV_R.ClearSelection();
            foreach (DataGridViewCell cell in DGV.SelectedCells)
            {
                int rowIndex = cell.RowIndex;
                if (DGV_R.Rows.Count != 0)
                    DGV_R.Rows[cell.RowIndex].Selected = true;
            }
            DGV_R.SelectionChanged += new EventHandler(DGV_R_SelectionChanged);
        }
        
        public void SetUserControlComboBoxCell(DataGridView Origianl_DGV, int RowIndex, DataGridView Destination_DGV)
        {
            if (Origianl_DGV.Rows[RowIndex].Cells[(int)_Column._VALUE_].GetType().Equals(typeof(DataGridViewComboBoxCell)))
            {
                DataGridViewComboBoxCell ComboBoxCell = new DataGridViewComboBoxCell();
                ComboBoxCell.ValueType = typeof(int);
                for (int Count = 0; Count < ((DataGridViewComboBoxCell)Origianl_DGV.Rows[RowIndex].Cells[(int)_Column._VALUE_]).Items.Count; ++Count)
                    ComboBoxCell.Items.Add(((DataGridViewComboBoxCell)Origianl_DGV.Rows[RowIndex].Cells[(int)_Column._VALUE_]).Items[Count]);

                ComboBoxCell.Value = Origianl_DGV.Rows[RowIndex].Cells[(int)_Column._VALUE_].Value;

                Destination_DGV.Rows[Destination_DGV.Rows.Count - 1].Cells[(int)_Column._VALUE_] = ComboBoxCell;
            }
        }

        private bool Between(uint value, uint[] ranges)
        {
            if (ranges[1] > ranges[0])
                return ((value >= ranges[0]) && (value <= ranges[1]));
            else if (ranges[0] > ranges[1])
                return ((value >= ranges[1]) && (value <= ranges[0]));
            else
                return false;
        }

        private void BTN_OpenFileDialog_Click(object sender, EventArgs e)
        {
            cb_mode_select.SelectedIndex = cb_mode_select.FindStringExact("0");
            //
            string strDefault = Directory.Exists(UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.strTPLPath) ? UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.strTPLPath : Environment.CurrentDirectory;
            OFD_OpenHexFile.InitialDirectory = strDefault;
            if (OFD_OpenHexFile.ShowDialog(this) != DialogResult.OK)
                return;

            if (!File.Exists(OFD_OpenHexFile.FileName))
            {
                MessageBox.Show("Cannot find selected file!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Path.GetExtension(OFD_OpenHexFile.FileName).ToLower() != ".hex")
            {
                MessageBox.Show("Selected cannot parse file!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var _Sender = sender as Button;
            if (_Sender == BTN_OpenFileDialog_L)
            {
                CodeBlock_Left.Clear();
                if (BTN_OpenFileInitialize(ref HexProcess_Left, ref m_xml_left, ref DGV_CompareFile_L, ref m_val_left))
                {
                    TBOX_FilePath_Left.Text = OFD_OpenHexFile.FileName;
                    //bool bISV3MCU = Enum.IsDefined(typeof(V3_MCU), (int)HexProcess_Left.DecodeHexInfo.McuName); //這邊是用來判斷V3使用的 不是V3就會是V6，故不用修改
                    bool bISV3MCU = EnumTool.IsEnumDefined<V3_MCU>(HexProcess_Left.DecodeHexInfo.McuName); //這邊是用來判斷V3使用的 不是V3就會是V6，故不用修改
                    var MCU = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.ILIxxxx_To_ILIxxxxS(HexProcess_Left.DecodeHexInfo.McuName);
                    string HexInfoString = "";

                    if(bISV3MCU)
                        HexInfoString = string.Format("IC:{0}, Code Base:V{1}", MCU, string.Join(".", HexProcess_Left.DecodeHexInfo.ArrCodeVersion.Select(x => x.ToString("X")).ToArray()));
                    else
                        HexInfoString = string.Format("IC:{0}, Tuning ver:V{1}", MCU, string.Join(".", HexProcess_Left.DecodeHexInfo.ArrTuningVersion.Select(x => x.ToString("X")).ToArray()));
                    
                    LBL_HexInfo_Left.Text = HexInfoString;
                    TSB_ExportLeftHexFile.Enabled = true;
                    //save AP code from Firmware_M[iDev].CodeBlock
                    CodeBlock_Left = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.CodeBlock.ToList();
                }
                if (HexProcess_Right == null)
                {
                    DGV_CompareFile_R.Rows.Clear();
                    TBOX_FilePath_Right.Clear();
                }
                if (HexProcess_Left == null)
                    return;
                if (EnumTool.IsEnumDefined<V3_MCU>(HexProcess_Left.DecodeHexInfo.McuName))
                    HexProcess_Left.DecodeHexInfo.EnableModeCnt = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWSupportModes;
                if (HexProcess_Left != null)
                {
                    if (HexProcess_Left.DecodeHexInfo.EnableModeCnt >= 2)
                    {
                        gb_ModeSel.Visible = true;
                        cb_mode_select.Visible = true;
                    }
                    else
                    {
                        gb_ModeSel.Visible = false;
                        cb_mode_select.Visible = false;
                    }
                }
            }
            else
            {
                CodeBlock_Right.Clear();
                if (BTN_OpenFileInitialize(ref HexProcess_Right, ref m_xml_right, ref DGV_CompareFile_R, ref m_val_right))
                {
                    TBOX_FilePath_Right.Text = OFD_OpenHexFile.FileName;
                    //bool bISV3MCU = Enum.IsDefined(typeof(V3_MCU), (int)HexProcess_Right.DecodeHexInfo.McuName); //這邊是用來判斷V3使用的 不是V3就會是V6，故不用修改
                    bool bISV3MCU = EnumTool.IsEnumDefined<V3_MCU>(HexProcess_Right.DecodeHexInfo.McuName); //這邊是用來判斷V3使用的 不是V3就會是V6，故不用修改
                    var MCU = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.ILIxxxx_To_ILIxxxxS(HexProcess_Right.DecodeHexInfo.McuName);
                    string HexInfoString = "";
                    if (bISV3MCU)
                        HexInfoString = string.Format("IC:{0}, Code Base:V{1}", MCU, string.Join(".", HexProcess_Right.DecodeHexInfo.ArrCodeVersion.Select(x => x.ToString("X")).ToArray()));

                    else
                        HexInfoString = string.Format("IC:{0}, Tuning ver:V{1}", MCU, string.Join(".", HexProcess_Right.DecodeHexInfo.ArrTuningVersion.Select(x => x.ToString("X")).ToArray()));

                    LBL_HexInfo_Right.Text = HexInfoString;
                    TSB_ExportRightHexFile.Enabled = true;
                    if (HexProcess_Left == null)
                    {
                        DGV_CompareFile_L.Rows.Clear();
                        TBOX_FilePath_Left.Clear();
                    }
                    //save AP code from Firmware_M[iDev].CodeBlock
                    CodeBlock_Right = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.CodeBlock.ToList();
                }
                if (HexProcess_Right != null)
                {
                    if (EnumTool.IsEnumDefined<V3_MCU>(HexProcess_Right.DecodeHexInfo.McuName))
                        HexProcess_Right.DecodeHexInfo.EnableModeCnt = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWSupportModes;
                    if (HexProcess_Right.DecodeHexInfo.EnableModeCnt >= 2)
                    {
                        gb_ModeSel.Visible = true;
                        cb_mode_select.Visible = true;
                    }
                    else
                    {
                        if (EnumTool.IsEnumDefined<V3_MCU>(HexProcess_Right.DecodeHexInfo.McuName))
                        {
                            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.inFWSupportModes >= 2)
                            {
                                gb_ModeSel.Visible = true;
                                cb_mode_select.Visible = true;
                            }
                            else
                            {
                                gb_ModeSel.Visible = false;
                                cb_mode_select.Visible = false;
                            }

                        }
                    }
                }
            }

            if (HexProcess_Left != null && HexProcess_Right != null)
            {
                if ((HexProcess_Right.DecodeHexInfo.EnableModeCnt == 1 && HexProcess_Left.DecodeHexInfo.EnableModeCnt == 1))
                {
                    gb_ModeSel.Visible = false;
                    cb_mode_select.Visible = false;
                }
                if (HexProcess_Left.DecodeHexInfo.McuName != HexProcess_Right.DecodeHexInfo.McuName)
                {
                    string LeftStr = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.ILIxxxx_To_ILIxxxxS(HexProcess_Left.DecodeHexInfo.McuName);
                    string RightStr = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.ILIxxxx_To_ILIxxxxS(HexProcess_Right.DecodeHexInfo.McuName);
                    MyUIPage.ShowErrorDialog("Can't Compare Different IC!!"
                        , string.Format("Left : {0} , Right : {1}", LeftStr, RightStr), UIStyle.Red, true);
                    gb_Find.Enabled = false;
                }
                else
                {
                    gb_Find.Enabled = true;

                }

                //MessageBox.Show("Cannot Compare Different IC!!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ComapreAndListItem();
        }

        private bool BTN_OpenFileInitialize(ref Hex_Processor HexProcess, ref List<TuningVariable.XML_DATA> xml, ref DataGridView DGV_CompareFile, ref List<string>[] val)
        {
            //====================================================
            // 1. Read Hex
            string strErrorMSG = "";
            string strHexMCU = "";
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_.PreloadHexMemorymapping(OFD_OpenHexFile.FileName, ref strHexMCU);
            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_.DecodeFile_API_forMerage2Hex(strHexMCU, OFD_OpenHexFile.FileName, ref strErrorMSG)!=0)
            {
                MessageBox.Show(strErrorMSG, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //====================================================
            // 2. Parser Tuning Version
            HexProcess = new Hex_Processor(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow);
            HexProcess.ParserHexInfo();
            //StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus
            //string TPL_Path = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.CompareTool.ParserTPL_Path(HexProcess, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.NowDevStatus != DevStatus.Connected);
            string TPL_Path = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.CompareTool.ParserTPL_Path(HexProcess, StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus != DevStatus.Connected);
            if (string.IsNullOrEmpty(TPL_Path))
            {
                string msg = string.Format("TPL Path is null", TPL_Path);
                MessageBox.Show(msg, "Cannot find TPL file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!File.Exists(TPL_Path))
            {
                string msg = string.Format("TPL Path : {0}", TPL_Path);
                MessageBox.Show(msg, "Cannot find TPL file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //====================================================
            // 3. Decode TPL
            //List<string> XML_Buff;
            //List<TuningVariable.XML_DATA> tmpBuff = new List<TuningVariable.XML_DATA>();
            //Tuning compare 使用Address排列，所以page2GroupEn要關閉
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.strTPLPathandFileName = TPL_Path;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.vbDecodePTL();

            //xml = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.m_XMLContent.ToList();
            xml = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXML.ToList();
            //uint TuningStartAddress = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_.m_XMLContent[0].inStartAddr;
            uint TuningStartAddress = UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.lsXML[0].inStartAddr;

            foreach (MultiFirmwareTool_VAR.HEX_CONTENT block in UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.CodeBlock)
            {
                if (block.HEX_DataArr == null)
                    continue;
                if (block.StartAddress < TuningStartAddress)
                    continue;

                HexProcess.DecodeHexInfo.TuningDataBuffer = new Dictionary<uint, byte>();
                foreach (var item1 in block.HEX_DataArr)
                {
                    uint iAddr = item1.Key;
                    foreach (var item2 in item1.Value)
                        HexProcess.DecodeHexInfo.TuningDataBuffer.Add(iAddr++, item2);
                }
                break;
            }

            //====================================================
            // 4. 根據TPL位置，將HEX資訊讀出來Mapping
            DGV_CompareFile.Rows.Clear();
            int mode = 1;
            if (!ProcessTPL(HexProcess.DecodeHexInfo.bIsV6, xml, HexProcess.DecodeHexInfo.TuningDataBuffer, ref DGV_CompareFile, mode, 1, ref val, false))
                return false;

            return true;
        }

        private void ComapreAndListItem()
        {
            if (HexProcess_Left != null && HexProcess_Right != null && (HexProcess_Left.DecodeHexInfo.McuName == HexProcess_Right.DecodeHexInfo.McuName))
            {
                DGV_CompareFile_L.Visible = false;
                DGV_CompareFile_R.Visible = false;
                UserCtl_Left.DGV_AddList.Rows.Clear();
                UserCtl_Left.DGV_DiffList.Rows.Clear();
                UserCtl_Left.DGV_SameList.Rows.Clear();
                UserCtl_Left.DGV_AddList.Refresh();
                UserCtl_Left.DGV_DiffList.Refresh();
                UserCtl_Left.DGV_SameList.Refresh();
                UserCtl_Left.Parent = splitContainer_Left.Panel2;
                UserCtl_Left.Dock = DockStyle.Fill;

                UserCtl_Right.DGV_AddList.Rows.Clear();
                UserCtl_Right.DGV_DiffList.Rows.Clear();
                UserCtl_Right.DGV_SameList.Rows.Clear();
                UserCtl_Right.DGV_AddList.Refresh();
                UserCtl_Right.DGV_DiffList.Refresh();
                UserCtl_Right.DGV_SameList.Refresh();
                UserCtl_Right.Parent = splitContainer_Right.Panel2;
                UserCtl_Right.Dock = DockStyle.Fill;

                bool FindTheSameItem;
                bool[] DGV2_Confirm;
                DGV2_Confirm = Enumerable.Repeat(false, DGV_CompareFile_R.RowCount).ToArray();
                for (int CountDGV1 = 0; CountDGV1 < DGV_CompareFile_L.RowCount; ++CountDGV1)
                {
                    FindTheSameItem = false;
                    for (int CountDGV2 = 0; CountDGV2 < DGV_CompareFile_R.RowCount; ++CountDGV2)
                    {
                        if (DGV2_Confirm[CountDGV2] == true)
                            continue;

                        bool bResult = DGV_CompareFile_L.Rows[CountDGV1].Visible && DGV_CompareFile_R.Rows[CountDGV2].Visible;
                        bResult &= string.Equals(DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._PAGE_INDEX_].Value.ToString(), DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._PAGE_INDEX_].Value.ToString());
                        bResult &= string.Equals(DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._ITEM_NAME_].Value.ToString(), DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._ITEM_NAME_].Value.ToString());
                        if (bResult)
                        {
                            if ((DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._VALUE_].Value == null) || (DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._VALUE_].Value == null))
                                continue;

                            if (DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._VALUE_].Value.ToString() == DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._VALUE_].Value.ToString())
                            {
                                //===================================================

                                UserCtl_Left.DGV_SameList.Rows.Add(new Object[] {
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._PAGE_NAME_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._ITEM_INDEX_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._MAX_VALUE_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._MIN_VALUE_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._ADDRESS_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._PAGE_NAME_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._ITEM_NAME_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._VALUE_].Value,
                                            CountDGV1,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._FORMAT_].Value});
                                // For show type setting
                                SetUserControlComboBoxCell(DGV_CompareFile_L, CountDGV1, UserCtl_Left.DGV_SameList);
                                for (int Count = (int)_Column._PAGE_NAME_; Count <= (int)_Column._VALUE_; ++Count)
                                    UserCtl_Left.DGV_SameList.Rows[UserCtl_Left.DGV_SameList.Rows.Count - 1].Cells[Count].ToolTipText =
                                        DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._PAGE_NAME_].ToolTipText;

                                if (DGV_CompareFile_L.Rows[CountDGV1].Visible == false)
                                    UserCtl_Left.DGV_SameList.Rows[UserCtl_Left.DGV_SameList.Rows.Count - 1].Visible = false;
                                //===================================================

                                UserCtl_Right.DGV_SameList.Rows.Add(new Object[] {
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._PAGE_NAME_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._ITEM_INDEX_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._MAX_VALUE_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._MIN_VALUE_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._ADDRESS_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._PAGE_NAME_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._ITEM_NAME_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._VALUE_].Value,
                                            CountDGV2,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._FORMAT_].Value});
                                FindTheSameItem = true;
                                // For show type setting
                                SetUserControlComboBoxCell(DGV_CompareFile_R, CountDGV2, UserCtl_Right.DGV_SameList);
                                for (int Count = (int)_Column._PAGE_NAME_; Count <= (int)_Column._VALUE_; ++Count)
                                    UserCtl_Right.DGV_SameList.Rows[UserCtl_Right.DGV_SameList.Rows.Count - 1].Cells[Count].ToolTipText =
                                        DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._PAGE_NAME_].ToolTipText;

                                if (DGV_CompareFile_R.Rows[CountDGV2].Visible == false)
                                    UserCtl_Right.DGV_SameList.Rows[UserCtl_Right.DGV_SameList.Rows.Count - 1].Visible = false;
                                //===================================================
                                DGV2_Confirm[CountDGV2] = true;
                                FindTheSameItem = true;

                                break;
                            }
                            else
                            {
                                //===================================================
                                UserCtl_Left.DGV_DiffList.Rows.Add(new Object[] {
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._PAGE_NAME_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._ITEM_INDEX_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._MAX_VALUE_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._MIN_VALUE_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._ADDRESS_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._PAGE_NAME_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._ITEM_NAME_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._VALUE_].Value,
                                            CountDGV1,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._FORMAT_].Value
                                });

                                // For show type setting
                                SetUserControlComboBoxCell(DGV_CompareFile_L, CountDGV1, UserCtl_Left.DGV_DiffList);

                                for (int Count = (int)_Column._PAGE_NAME_; Count <= (int)_Column._VALUE_; ++Count)
                                    UserCtl_Left.DGV_DiffList.Rows[UserCtl_Left.DGV_DiffList.Rows.Count - 1].Cells[Count].ToolTipText =
                                        DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._PAGE_NAME_].ToolTipText;

                                if (DGV_CompareFile_L.Rows[CountDGV1].Visible == false)
                                    UserCtl_Left.DGV_DiffList.Rows[UserCtl_Left.DGV_DiffList.Rows.Count - 1].Visible = false;
                                //===================================================

                                UserCtl_Right.DGV_DiffList.Rows.Add(new Object[] {
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._PAGE_NAME_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._ITEM_INDEX_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._MAX_VALUE_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._MIN_VALUE_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._ADDRESS_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._PAGE_NAME_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._ITEM_NAME_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._VALUE_].Value,
                                            CountDGV2,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._FORMAT_].Value});
                                // For show type setting
                                SetUserControlComboBoxCell(DGV_CompareFile_R, CountDGV2, UserCtl_Right.DGV_DiffList);
                                for (int Count = (int)_Column._PAGE_NAME_; Count <= (int)_Column._VALUE_; ++Count)
                                    UserCtl_Right.DGV_DiffList.Rows[UserCtl_Right.DGV_DiffList.Rows.Count - 1].Cells[Count].ToolTipText =
                                        DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._PAGE_NAME_].ToolTipText;

                                if (DGV_CompareFile_R.Rows[CountDGV2].Visible == false)
                                    UserCtl_Right.DGV_DiffList.Rows[UserCtl_Right.DGV_DiffList.Rows.Count - 1].Visible = false;

                                DGV2_Confirm[CountDGV2] = true;
                                FindTheSameItem = true;

                                break;
                            }
                        }
                    }
                    if (!FindTheSameItem)
                    {
                        UserCtl_Left.DGV_AddList.Rows.Add(new Object[] {
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._PAGE_NAME_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._ITEM_INDEX_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._MAX_VALUE_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._MIN_VALUE_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._ADDRESS_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._PAGE_NAME_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._ITEM_NAME_].Value,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._VALUE_].Value,
                                            CountDGV1,
                                            DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._FORMAT_].Value});
                        // For show type setting
                        SetUserControlComboBoxCell(DGV_CompareFile_L, CountDGV1, UserCtl_Left.DGV_AddList);
                        for (int Count = (int)_Column._PAGE_NAME_; Count <= (int)_Column._VALUE_; ++Count)
                        {
                            UserCtl_Left.DGV_AddList.Rows[UserCtl_Left.DGV_AddList.Rows.Count - 1].Cells[Count].ToolTipText =
                                DGV_CompareFile_L.Rows[CountDGV1].Cells[(int)_Column._PAGE_NAME_].ToolTipText;
                        }
                        if (DGV_CompareFile_L.Rows[CountDGV1].Visible == false)
                        {
                            UserCtl_Left.DGV_AddList.Rows[UserCtl_Left.DGV_AddList.Rows.Count - 1].Visible = false;
                        }
                    }
                }
                for (int CountDGV2 = 0; CountDGV2 < DGV_CompareFile_R.RowCount; ++CountDGV2)
                {
                    if (DGV2_Confirm[CountDGV2] == true)
                    {
                        continue;
                    }
                    UserCtl_Right.DGV_AddList.Rows.Add(new Object[] {
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._PAGE_NAME_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._ITEM_INDEX_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._MAX_VALUE_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._MIN_VALUE_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._ADDRESS_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._PAGE_NAME_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._ITEM_NAME_].Value,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._VALUE_].Value,
                                            CountDGV2,
                                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._FORMAT_].Value});
                    // For show type setting
                    SetUserControlComboBoxCell(DGV_CompareFile_R, CountDGV2, UserCtl_Right.DGV_AddList);
                    for (int Count = (int)_Column._PAGE_NAME_; Count <= (int)_Column._VALUE_; ++Count)
                    {
                        UserCtl_Right.DGV_AddList.Rows[UserCtl_Right.DGV_AddList.Rows.Count - 1].Cells[Count].ToolTipText =
                            DGV_CompareFile_R.Rows[CountDGV2].Cells[(int)_Column._PAGE_NAME_].ToolTipText;
                    }
                    if (DGV_CompareFile_R.Rows[CountDGV2].Visible == false)
                    {
                        UserCtl_Right.DGV_AddList.Rows[UserCtl_Right.DGV_AddList.Rows.Count - 1].Visible = false;
                    }
                }
            }
            else
            {
                DGV_CompareFile_L.Visible = true;
                DGV_CompareFile_R.Visible = true;
                UserCtl_Left.Parent = null;
                UserCtl_Right.Parent = null;
            }
        }

        private void DGV_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            bool validClick = (e.RowIndex != -1 && e.ColumnIndex != -1); //Make sure the clicked row/column is valid.
            var datagridview = sender as DataGridView;

            // Check to make sure the cell clicked is the cell containing the combobox
            if (datagridview.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell && validClick)
            {
                datagridview.BeginEdit(true);
                ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            }
            else if (datagridview.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewTextBoxCell && validClick)
            {
                datagridview.BeginEdit(true);
            }
        }

        private void DGV_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView DGV = (DataGridView)sender;
            DataGridView DGV_L, DGV_R;
            if (DGV.Name == "DGV_DiffList")
            {
                DGV_L = UserCtl_Left.DGV_DiffList;
                DGV_R = UserCtl_Right.DGV_DiffList;
                
            }
            else
            {
                DGV_L = UserCtl_Left.DGV_SameList;
                DGV_R = UserCtl_Right.DGV_SameList;

            }
            if (e.Button == MouseButtons.Left)
            {
                if (e.RowIndex >= 0)
                {
                    if (DGV_R.Rows.Count != 0 && DGV_L.Rows.Count != 0)
                    {
                        DGV_R.ClearSelection();
                        DGV_R.Rows[e.RowIndex].Selected = true;
                        DGV_R.CurrentCell = DGV_R.Rows[e.RowIndex].Cells[e.ColumnIndex];
                        DGV_L.ClearSelection();
                        DGV_L.Rows[e.RowIndex].Selected = true;
                        DGV_L.CurrentCell = DGV_L.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                }
            }
        }

        private void DGV_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != (int)_Column._VALUE_)
                return;

            DataGridView DGV = (DataGridView)sender;
            UInt32 ValueBuffer = 0;
            try
            {
                int iNum = Convert.ToUInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._FORMAT_].Value) == (int)TuningFormat._16 ? 16 : 10;
                ValueBuffer = Convert.ToUInt32(e.FormattedValue.ToString(), iNum);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Value Format Error!!" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }

            uint iMax = Convert.ToUInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._MAX_VALUE_].Value);
            uint iMin = Convert.ToUInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._MIN_VALUE_].Value);
            if (!Between(ValueBuffer, new uint[] { iMin, iMax }))
            {
                DGV.EditingControl.Text = DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString();
                string StringBuffer = string.Format("Value Valid Range = {0} ~ {1}", iMin, iMax);
                MessageBox.Show(StringBuffer, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void DGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            DataGridView DGV = (DataGridView)sender;
            DGV.CellValueChanged -= DGV_CellValueChanged;
            if (DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].GetType() != typeof(DataGridViewComboBoxCell))
            {
                string StringValueBuffer = "";
                int ValueBuffer = 0;
                switch (Convert.ToUInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._FORMAT_].Value))
                {
                    case (int)TuningFormat._16:
                        if (DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString().ToLower().Contains("0x"))
                        {
                            ValueBuffer = Convert.ToInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString(), 16);
                            StringValueBuffer = "0x" + Convert.ToString(ValueBuffer, 16).ToUpper();
                        }
                        else
                        {
                            if (System.Text.RegularExpressions.Regex.IsMatch(DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString(), "^[0-9]+$"))
                            {
                                ValueBuffer = Convert.ToInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString(), 10);
                            }
                            else if (System.Text.RegularExpressions.Regex.IsMatch(DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString(), "^[0-9a-fA-F]+$"))
                            {
                                ValueBuffer = Convert.ToInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString(), 16);
                            }
                            else
                                ValueBuffer = 0;
                            StringValueBuffer = "0x" + Convert.ToString(ValueBuffer, 16).ToUpper();
                        }
                        break;

                    case (int)TuningFormat._10:
                        if (DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString().ToLower().Contains("0x"))
                        {
                            ValueBuffer = Convert.ToInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString().Replace("0x", ""), 16);
                        }
                        else
                        {
                            ValueBuffer = Convert.ToInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString());
                        }
                        StringValueBuffer = Convert.ToString(ValueBuffer, 10);
                        break;
                }
                DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value = StringValueBuffer;
                //update the m_val_right or m_val_left
                if (DGV == DGV_CompareFile_L && m_val_left[0].Count != 0)
                    m_val_left[cb_mode_select.SelectedIndex][e.RowIndex] = StringValueBuffer;
                else if (DGV == DGV_CompareFile_R && m_val_right[0].Count != 0)
                    m_val_right[cb_mode_select.SelectedIndex][e.RowIndex] = StringValueBuffer;
            }
            else
            {
                if ((m_val_left[0].Count != 0) || m_val_right[0].Count != 0)
                {
                    string StringValueBuffer = "";
                    int ValueBuffer = 0;
                    switch (Convert.ToUInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._FORMAT_].Value))
                    {
                        case (int)TuningFormat._16:
                            ValueBuffer = Convert.ToInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString(), 16);
                            StringValueBuffer = "0x" + Convert.ToString(ValueBuffer, 16).ToUpper();
                            break;

                        case (int)TuningFormat._10:
                            ValueBuffer = Convert.ToInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString());
                            StringValueBuffer = Convert.ToString(ValueBuffer, 10);
                            break;
                    }

                    if (DGV == DGV_CompareFile_L)
                        m_val_left[cb_mode_select.SelectedIndex][e.RowIndex] = StringValueBuffer;
                    else if (DGV == DGV_CompareFile_R)
                        m_val_right[cb_mode_select.SelectedIndex][e.RowIndex] = StringValueBuffer;
                }
            }
            //=================================================
            // Control 專用
            //=================================================
            if (DGV.Equals(UserCtl_Left.DGV_AddList)
                    || DGV.Equals(UserCtl_Left.DGV_DiffList)
                    || DGV.Equals(UserCtl_Left.DGV_SameList))
            {
                int RowCount = Convert.ToInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._CORRESPOND_INDEX_].Value);
                string strValue = DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString();
                DGV_CompareFile_L.Rows[RowCount].Cells[(int)_Column._VALUE_].Value = strValue;
            }
            else if (DGV.Equals(UserCtl_Right.DGV_AddList)
                  || DGV.Equals(UserCtl_Right.DGV_DiffList)
                  || DGV.Equals(UserCtl_Right.DGV_SameList))
            {
                int RowCount = Convert.ToInt32(DGV.Rows[e.RowIndex].Cells[(int)_Column._CORRESPOND_INDEX_].Value);
                string strValue = DGV.Rows[e.RowIndex].Cells[(int)_Column._VALUE_].Value.ToString();
                DGV_CompareFile_R.Rows[RowCount].Cells[(int)_Column._VALUE_].Value = strValue;
            }
            DGV.CellValueChanged += DGV_CellValueChanged;
        }

        private void DGV_SameList_Scroll(object sender, ScrollEventArgs e)
        {
            if (sender.Equals(UserCtl_Left.DGV_SameList))
            {
                UserCtl_Right.DGV_SameList.FirstDisplayedScrollingRowIndex = UserCtl_Left.DGV_SameList.FirstDisplayedScrollingRowIndex;
            }
            else if (sender.Equals(UserCtl_Right.DGV_SameList))
            {
                UserCtl_Left.DGV_SameList.FirstDisplayedScrollingRowIndex = UserCtl_Right.DGV_SameList.FirstDisplayedScrollingRowIndex;
            }
            else if (sender.Equals(UserCtl_Left.DGV_DiffList))
            {
                UserCtl_Right.DGV_DiffList.FirstDisplayedScrollingRowIndex = UserCtl_Left.DGV_DiffList.FirstDisplayedScrollingRowIndex;
            }
            else if (sender.Equals(UserCtl_Right.DGV_DiffList))
            {
                UserCtl_Left.DGV_DiffList.FirstDisplayedScrollingRowIndex = UserCtl_Right.DGV_DiffList.FirstDisplayedScrollingRowIndex;
            }
        }

        private void ExportParametertoHex(string strSORHexFilePath, List<MultiFirmwareTool_VAR.HEX_CONTENT> APCodeList,　Hex_Processor HexProcess, ref List<TuningVariable.XML_DATA> InXMLContent, ref List<string>[] val)
        {
            SFD_SaveHexFile.InitialDirectory = Environment.CurrentDirectory;
            if (SFD_SaveHexFile.ShowDialog(this) != DialogResult.OK)
                return;
           
            List<TuningVariable.XML_DATA> TmpXMLContent = new List<TuningVariable.XML_DATA>(InXMLContent);
            int TotalMode = TmpXMLContent[TmpXMLContent.Count - 1].inModeID;
            int PagePerMode = TmpXMLContent[TmpXMLContent.Count - 1].inPageID;
            int inPages = TmpXMLContent.Count / TotalMode;
            int inPageCount = 0;
            int Count = 0;
            if (EnumTool.IsEnumDefined<V3_MCU>(HexProcess.DecodeHexInfo.McuName))
            {
                for (int iModeCount = 0; iModeCount < TotalMode; iModeCount++)
                {
                    for (int iPage = 0; iPage < inPages; iPage++)
                    {
                        inPageCount = iPage * TotalMode + iModeCount;
                        for (int iCnt = 0; iCnt < TmpXMLContent[inPageCount].lsTuningVar.Count; ++iCnt)
                        {
                            NumberStyles tmpNumber;
                            object tmpValue;
                            tmpNumber = (val[iModeCount][Count].Contains("0x") || val[iModeCount][Count].Contains("0X")) ? NumberStyles.HexNumber : NumberStyles.Integer;
                            tmpValue = val[iModeCount][Count];
                            int iValue = 0;
                            if (tmpValue != null)
                                int.TryParse(tmpValue.ToString().Replace("0x", ""), tmpNumber, null, out iValue);

                            TmpXMLContent[inPageCount].lsTuningVar[iCnt].alValue[0] = iValue;
                            Count++;
                        }
                    }
                    Count = 0;
                }
            }
            else
            {
                for (int iPage = 0; iPage < inPages; iPage++)
                {
                    for (int iModeCount = 0; iModeCount < TotalMode; iModeCount++)
                    {
                        inPageCount = iPage * TotalMode + iModeCount;
                        for (int iCnt = 0; iCnt < TmpXMLContent[inPageCount].lsTuningVar.Count; ++iCnt)
                        {
                            NumberStyles tmpNumber;
                            object tmpValue;
                            tmpNumber = (val[iModeCount][Count].Contains("0x") || val[iModeCount][Count].Contains("0X")) ? NumberStyles.HexNumber : NumberStyles.Integer;
                            tmpValue = val[iModeCount][Count];
                            int iValue = 0;
                            if (tmpValue != null)
                                int.TryParse(tmpValue.ToString().Replace("0x", ""), tmpNumber, null, out iValue);

                            TmpXMLContent[inPageCount].lsTuningVar[iCnt].alValue[0] = iValue;
                            Count++;
                        }
                        Count = 0;
                    }
                }
            }
            InXMLContent = TmpXMLContent;
            StringBuilder strTPLBuffer = new StringBuilder();
            Console.WriteLine("strTPLBuffer={0}\r\n", strTPLBuffer);
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.CompareTool.vbPreTuningDataToSave(ref strTPLBuffer, TmpXMLContent, HexProcess);
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.CompareTool.Combine_TuningToHex(strTPLBuffer, APCodeList, TmpXMLContent, HexProcess, SFD_SaveHexFile.FileName);
            MessageBox.Show("Export Hex File Success!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool GetData(Dictionary<uint, byte> Dic, uint startIndex, int Len, out List<byte> OutData)
        {
            OutData = new List<byte>();
            uint iEnd = startIndex + (uint)Len;
            for (uint iAddr = startIndex; iAddr < iEnd; iAddr++)
            {
                if (!Dic.ContainsKey(startIndex))
                    return false;
                OutData.Add(Dic[iAddr]);
            }
            return true;
        }

        private bool ProcessTPL(bool is_v6, List<TuningVariable.XML_DATA> xml, Dictionary<uint, byte> TuningData, ref DataGridView DGV_Parameter, int modeNum, int enable_modes, ref List<string>[] val, bool from_val)
        {
            if (enable_modes < modeNum && modeNum != (int)_ModeStatus.NotSupported)
            {
                return true;
            }
            //===============================
            if (!from_val || modeNum != (int)_ModeStatus.NotSupported)//若是從檔案讀進來的資料或是V6從表格修改，則會DGV_Parameter清零初始化，V3若從表格修改不能初始化DGV_Parameter，否則修改的內容會消失。
            {
                DGV_Parameter.Rows.Clear();
                DGV_Parameter.Refresh();
            }
            if (!from_val || modeNum == (int)_ModeStatus.NotSupported)
            {
                for (int i = 0; i < val.Count(); i++)
                    val[i].Clear();
            }
            if (TuningData == null)
            {
                MessageBox.Show("The hex code does not contain tuning parameters.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            DataGridViewRowCollection rows = DGV_Parameter.Rows;
            bool fMSGBOX = false;
            int iPageCnts = 0;
            int iItemCnts = 0;
            INI_Tool MY_SettingsINI = new INI_Tool(ITS_Directory.Setting_INI);
            bool bAllItemVisble = MY_SettingsINI.IniReadBool("TUNING", "Item_All_Visible", false);
            int modecnt = xml[xml.Count()-1].inModeID;
            int pagecnt = xml.Count/modecnt;
            foreach (var page in xml)
            {
                List<TuningVariable.TuningParameter> tmpTuningVar = page.lsTuningVar;
                if (is_v6)
                    iItemCnts = 0;
                else
                {
                    int pid = page.inPageID-1;
                    iItemCnts = 0;
                    if (pid != 0)
                    {
                        do
                        {
                            if (--pid >= 0)
                                iItemCnts += xml[(pid) * modecnt + (modeNum - 1)].lsTuningVar.Count();
                        } while (pid > 0);
                    }
                    //if (((page.inPageID - 1) % pagecnt) == 0)//0 4 8
                    //    iItemCnts = 0;
                    //else if (((page.inPageID - 1) % pagecnt) == 1)//1 5 9
                    //    iItemCnts = xml[(page.inPageID - 2) * modecnt + (modeNum - 1)].lsTuningVar.Count();
                    //else if (((page.inPageID - 1) % pagecnt) == 2)//2 6 10
                    //    iItemCnts = xml[(page.inPageID - 3) * modecnt + (modeNum - 1)].lsTuningVar.Count() + xml[(page.inPageID - 2) * modecnt + (modeNum - 1)].lsTuningVar.Count();
                    //else if (((page.inPageID - 1) % pagecnt) == 3)//3 7 11
                    //    iItemCnts = xml[(page.inPageID - 4) * modecnt + (modeNum - 1)].lsTuningVar.Count() + xml[(page.inPageID - 3) * modecnt + (modeNum - 1)].lsTuningVar.Count() + xml[(page.inPageID - 2) * modecnt + (modeNum - 1)].lsTuningVar.Count();
                }
                foreach (var item in tmpTuningVar)
                {
                    uint ValueOffset = page.inStartAddr + (uint)item.inOffset;
                    int iCapLen = 1;
                    switch (item.inLen)
                    {
                        case (int)_BITS_._32_:
                            iCapLen = 4;
                            break;

                        case (int)_BITS_._16_:
                            iCapLen = 2;
                            break;
                    }
                    //int iCapLen = (item.inLen == (int)_BITS_._32_) ? 4 : ((item.inLen == (int)_BITS_._16_) ? 2 : 1);
                    uint iDefaultValue = Convert.ToUInt32(item.alValue[0]);
                    List<byte> TmpList;
                    if (!GetData(TuningData, ValueOffset, iCapLen, out TmpList))
                    {
                        string tmpBuffer = string.Format("TPL Offset 0x{0:X} out of range!\r\n", ValueOffset);
                        MessageBox.Show(tmpBuffer, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    UInt32 ValueBuffer = (uint)ArrayTool.ArrayToInt(TmpList.Select(x => (int)x).ToArray(), false);
                    //==================================================
                    uint iMax = item.inMax;
                    uint iMin = item.inMin;
                    bool bVisible = bAllItemVisble ? true : item.boVisible;
                    if (!Between(ValueBuffer, new uint[] { iMin, iMax }) && bVisible)
                    {
                        string tmpBuffer = string.Format("\"{0}\".\"{1}\" = {2} in the invalid range({3}~{4}),\n The value replaces by default value!"
                            , page.strPageName
                            , item.strName
                            , ValueBuffer
                            , iMax, iMin);

                        if (!fMSGBOX)
                        {
                            MessageBox.Show(tmpBuffer, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            fMSGBOX = true;
                        }
                        ValueBuffer = iDefaultValue;
                        ValueBuffer = (ValueBuffer > iMax) ? iMax : ValueBuffer;
                        ValueBuffer = (ValueBuffer < iMin) ? iMin : ValueBuffer;
                    }

                    //==================================================
                    string ItemAddress = string.Format("0x{0:X}", ValueOffset);
                    string strValue = (item.inFormat == ValueFormat.HEX) ? ("0x" + Convert.ToString(ValueBuffer, 16).ToUpper()) : Convert.ToString(ValueBuffer, 10);
                    if (page.inModeID == modeNum)
                    {
                        if (from_val && is_v6)
                            rows.Add(new Object[] { iPageCnts, iItemCnts, iMax, iMin, ItemAddress, item.strGroup, item.strName, val[modeNum - 1][iItemCnts], item.inOffset, (int)item.inFormat });
                        else
                            rows.Add(new Object[] { iPageCnts, iItemCnts, iMax, iMin, ItemAddress, item.strGroup, item.strName, strValue, item.inOffset, (int)item.inFormat });

                        if (item.boVisible == false && bAllItemVisble == false)
                            rows[rows.Count - 1].Visible = false;
                    }
                    // For show type setting
                    if (item.inShowType == ShowType.COMBOBOX)
                    {
                        DataGridViewComboBoxCell ComboBoxCell = new DataGridViewComboBoxCell();
                        ComboBoxCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                        ComboBoxCell.DisplayStyleForCurrentCellOnly = true;
                        ComboBoxCell.Items.Clear();
                        ComboBoxCell.ValueType = typeof(int);

                        foreach (var employee in item.alValue.GetRange(1, item.alValue.Count - 1))
                        {
                            ComboBoxCell.Items.Add(employee);
                            if (from_val)
                            {
                                uint now_val = 0;
                                if (val[modeNum - 1][iItemCnts].Contains("0x") || val[modeNum - 1][iItemCnts].Contains("0X"))
                                {
                                    string str = val[modeNum - 1][iItemCnts].Replace("0x", "");
                                    str = str.Replace("0X", "");
                                    now_val = Convert.ToUInt32(str, 16);
                                }
                                else
                                    now_val = Convert.ToUInt32(val[modeNum - 1][iItemCnts]);
                                if (Convert.ToUInt32(employee) == now_val)
                                    ComboBoxCell.Value = employee;

                            }
                            else
                            {
                                if (Convert.ToUInt32(employee) == ValueBuffer)
                                    ComboBoxCell.Value = employee;
                            }
                        }
                        if ((page.inModeID == modeNum))
                            rows[rows.Count - 1].Cells[(int)_Column._VALUE_] = ComboBoxCell;
                    }
                    if (!from_val)
                    {
                        val[page.inModeID - 1].Add(strValue);
                    }
                    if (page.inModeID == modeNum)
                    {
                        for (int Count = (int)_Column._PAGE_NAME_; Count <= (int)_Column._VALUE_; ++Count)
                            rows[rows.Count - 1].Cells[Count].ToolTipText = item.strHint;
                    }
                    iItemCnts++;
                }
                iPageCnts++;
            }
            return true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            UI_GBV.fmITS_Tool.MainButton_Disable();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_TuningChangeControl, (IntPtr)TuningPage.WriteParaPage, (IntPtr)null);
            UI_GBV.fmITS_Tool.Reset_UI();
            m_xml_right = null;
            m_xml_left = null;
            gb_ModeSel.Visible = false;
            gb_Find.Enabled = false;
        }

        private void TriggerCellValidatingEvent(DataGridView DGV)
        {
            if (DGV.RowCount == 0)
                return;
            DataGridViewCell DGVC = DGV.CurrentCell;
            DGV.CurrentCell = null;
            DGV.CurrentCell = DGVC;
        }

        private void TSB_ExportHexFile_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem MySender = sender as ToolStripMenuItem;
            DGV_CompareFile_L.EndEdit();
            DGV_CompareFile_R.EndEdit();
            UserCtl_Left.DGV_AddList.EndEdit();
            UserCtl_Left.DGV_DiffList.EndEdit();
            UserCtl_Left.DGV_SameList.EndEdit();
            UserCtl_Right.DGV_AddList.EndEdit();
            UserCtl_Right.DGV_DiffList.EndEdit();
            UserCtl_Right.DGV_SameList.EndEdit();
            if (MySender == TSB_ExportLeftHexFile)
            {
                TriggerCellValidatingEvent(UserCtl_Left.DGV_AddList);
                TriggerCellValidatingEvent(UserCtl_Left.DGV_DiffList);
                TriggerCellValidatingEvent(UserCtl_Left.DGV_SameList);
                TriggerCellValidatingEvent(DGV_CompareFile_L);
                ExportParametertoHex(TBOX_FilePath_Left.Text, CodeBlock_Left,　HexProcess_Left, ref m_xml_left, ref m_val_left);
            }
            else
            {
                TriggerCellValidatingEvent(UserCtl_Right.DGV_AddList);
                TriggerCellValidatingEvent(UserCtl_Right.DGV_DiffList);
                TriggerCellValidatingEvent(UserCtl_Right.DGV_SameList);
                TriggerCellValidatingEvent(DGV_CompareFile_R);
                ExportParametertoHex(TBOX_FilePath_Right.Text, CodeBlock_Right,　HexProcess_Right, ref m_xml_right, ref m_val_right);
            }
        }

        private void TSB_NewCompare_Click(object sender, EventArgs e)
        {
            TBOX_FilePath_Left.Text = String.Empty;
            TBOX_FilePath_Right.Text = String.Empty;

            LBL_HexInfo_Left.Text = String.Empty;
            LBL_HexInfo_Right.Text = String.Empty;

            TSB_ExportLeftHexFile.Enabled = false;
            TSB_ExportRightHexFile.Enabled = false;

            DGV_CompareFile_L.Rows.Clear();
            DGV_CompareFile_R.Rows.Clear();

            DGV_CompareFile_L.Visible = true;
            DGV_CompareFile_R.Visible = true;

            UserCtl_Left.Parent = null;
            UserCtl_Right.Parent = null;

            HexProcess_Left = null;
            HexProcess_Right = null;

            gb_ModeSel.Visible = false;
            m_xml_left = null;
            m_xml_right = null;
            gb_Find.Enabled = false;
        }

        private void TuningCompare_VisibleChanged(object sender, EventArgs e)
        {
            //TODO: DGV_CompareFile1 reset
            //StaticVar.LogWrite("TuningCompare_VisibleChanged\r\n");
            HexProcess_Left = null;
            HexProcess_Right = null;
            DGV_CompareFile_L.Rows.Clear();
            DGV_CompareFile_R.Rows.Clear();
            TBOX_FilePath_Left.Clear();
            TBOX_FilePath_Right.Clear();
            UserCtl_Left.DGV_DiffList.Rows.Clear();
            UserCtl_Left.DGV_SameList.Rows.Clear();
            UserCtl_Left.DGV_AddList.Rows.Clear();
            UserCtl_Right.DGV_DiffList.Rows.Clear();
            UserCtl_Right.DGV_SameList.Rows.Clear();
            UserCtl_Right.DGV_AddList.Rows.Clear();
            UserCtl_Left.DGV_AddList.Rows.Clear();
            TSB_ExportLeftHexFile.Enabled = false;
            TSB_ExportRightHexFile.Enabled = false;
        }

        private void DGV_CompareFile_L_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Error happened " + e.Context.ToString());

            if (e.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (e.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (e.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (e.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((e.Exception) is System.Data.ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[e.RowIndex].ErrorText = "an error";
                view.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "an error";

                e.ThrowException = false;
            }
        }

        private void DGV_CompareFile_R_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Error happened " + e.Context.ToString());

            if (e.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (e.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (e.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (e.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((e.Exception) is System.Data.ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[e.RowIndex].ErrorText = "an error";
                view.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "an error";

                e.ThrowException = false;
            }
        }

        private void btn_cp_toRight_Click(object sender, EventArgs e)
        {
            if (m_xml_left == null || m_xml_right == null || UserCtl_Left.DGV_DiffList.SelectedRows.Count == 0)
                return;
            DataGridViewSelectedRowCollection tmp = UserCtl_Left.DGV_DiffList.SelectedRows;
            foreach (DataGridViewRow row in tmp)
            {
                UserCtl_Right.DGV_DiffList.Rows[row.Index].Cells[(int)_Column._VALUE_].Value = UserCtl_Left.DGV_DiffList.Rows[row.Index].Cells[(int)_Column._VALUE_].Value;

                //UserCtl_Left.DGV_DiffList.Rows[row.Index].DefaultCellStyle.SelectionBackColor = Color.White;
                //UserCtl_Left.DGV_DiffList.Rows[row.Index].DefaultCellStyle.SelectionForeColor = Color.Black; //This is the text color 
            }
            //update the m_val_right
            int mode = 0;
            //if (Enum.IsDefined(typeof(V3_MCU), (int)HexProcess_Right.DecodeHexInfo.McuName))
            if (EnumTool.IsEnumDefined<V3_MCU>(HexProcess_Right.DecodeHexInfo.McuName))
                mode = cb_mode_select.SelectedIndex + 1;
            else
                mode = cb_mode_select.SelectedIndex + 1;
            if (!ProcessTPL(HexProcess_Right.DecodeHexInfo.bIsV6, m_xml_right, HexProcess_Right.DecodeHexInfo.TuningDataBuffer, ref DGV_CompareFile_R, mode, HexProcess_Right.DecodeHexInfo.EnableModeCnt, ref m_val_right, true))
            {
                MessageBox.Show("ProcessTPL() of right side hex file occurs error when copy value to right\r\n", "Compare Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btn_cp_toLeft_Click(object sender, EventArgs e)
        {
            if (m_xml_left == null || m_xml_right == null || UserCtl_Right.DGV_DiffList.SelectedRows.Count==0)
                return;
            DataGridViewSelectedRowCollection tmp = UserCtl_Right.DGV_DiffList.SelectedRows;
            foreach (DataGridViewRow row in tmp)
            {
                UserCtl_Left.DGV_DiffList.Rows[row.Index].Cells[(int)_Column._VALUE_].Value = UserCtl_Right.DGV_DiffList.Rows[row.Index].Cells[(int)_Column._VALUE_].Value;
                //UserCtl_Right.DGV_DiffList.Rows[row.Index].DefaultCellStyle.SelectionBackColor = Color.White;
                //UserCtl_Right.DGV_DiffList.Rows[row.Index].DefaultCellStyle.SelectionForeColor = Color.Black; //This is the text color 
            }
            //update the m_val_left
            int mode = 0;
            //if (Enum.IsDefined(typeof(V3_MCU), (int)HexProcess_Left.DecodeHexInfo.McuName))
            if (EnumTool.IsEnumDefined<V3_MCU>(HexProcess_Left.DecodeHexInfo.McuName))
                mode = cb_mode_select.SelectedIndex + 1;
            else
                mode = cb_mode_select.SelectedIndex + 1;
            if (!ProcessTPL(HexProcess_Left.DecodeHexInfo.bIsV6, m_xml_left, HexProcess_Left.DecodeHexInfo.TuningDataBuffer, ref DGV_CompareFile_L, mode, HexProcess_Left.DecodeHexInfo.EnableModeCnt, ref m_val_left, true))
            {
                MessageBox.Show("ProcessTPL() of left side hex file occurs error when copy value to left\r\n", "Compare Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void cb_mode_select_SelectedIndexChanged(object sender, EventArgs e)
        {
            int mode = this.cb_mode_select.SelectedIndex + 1;
            // 4. 根據TPL位置，將HEX資訊讀出來Mapping
            if (m_xml_left != null)
            {
                DGV_CompareFile_L.Rows.Clear();
                if (HexProcess_Left.DecodeHexInfo.EnableModeCnt >= 2 || mode == 1)
                {
                    if (!ProcessTPL(HexProcess_Left.DecodeHexInfo.bIsV6, 
                        m_xml_left,
                        HexProcess_Left.DecodeHexInfo.TuningDataBuffer,
                        ref DGV_CompareFile_L,
                        mode,
                        HexProcess_Left.DecodeHexInfo.EnableModeCnt,
                        ref m_val_left,
                        true))
                    {
                        MessageBox.Show("ProcessTPL() on the left occurs an error\r\n", "Compare Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (HexProcess_Left.DecodeHexInfo.EnableModeCnt < mode)
                {
                    MessageBox.Show(string.Format("File on the left only supports {0} mode\r\n", HexProcess_Left.DecodeHexInfo.EnableModeCnt), "Compare Tool", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (m_xml_right != null)
            {
                DGV_CompareFile_R.Rows.Clear();
                if (HexProcess_Right.DecodeHexInfo.EnableModeCnt >= 2 || mode == 1)
                {
                    if (!ProcessTPL(HexProcess_Right.DecodeHexInfo.bIsV6, 
                        m_xml_right,
                        HexProcess_Right.DecodeHexInfo.TuningDataBuffer,
                        ref DGV_CompareFile_R,
                        mode,
                        HexProcess_Right.DecodeHexInfo.EnableModeCnt,
                        ref m_val_right,
                        true))
                    {
                        MessageBox.Show("ProcessTPL() on the right occurs an error\r\n", "Compare Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (HexProcess_Right.DecodeHexInfo.EnableModeCnt < mode)
                {
                    MessageBox.Show(string.Format("File on the right only supports {0} mode\r\n", HexProcess_Right.DecodeHexInfo.EnableModeCnt), "Compare Tool", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (m_xml_right != null && m_xml_left != null)
            {
                ComapreAndListItem();
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            UserCtl_Right.DGV_SameList.ClearSelection();
            UserCtl_Left.DGV_SameList.ClearSelection();
            UserCtl_Right.DGV_AddList.ClearSelection();
            UserCtl_Left.DGV_AddList.ClearSelection();
            UserCtl_Right.DGV_DiffList.ClearSelection();
            UserCtl_Left.DGV_DiffList.ClearSelection();
#if true 
            tb_find_string.Text = "";
            ComapreAndListItem();
                return;
#else
            int rowIndex = -1;
            if (tb_find_string.Text == "")
            {
                ComapreAndListItem();
                return;
            }

            foreach (DataGridViewRow row in UserCtl_Right.DGV_SameList.Rows)
            {
                if (row.Cells[6].Value.ToString().Equals(tb_find_string.Text))
                {
                    rowIndex = row.Index;
                    UserCtl_Right.DGV_SameList.Rows[rowIndex].Selected = true;
                    UserCtl_Left.DGV_SameList.Rows[rowIndex].Selected = true;
                    break;
                }
            }
            if (rowIndex != -1)
            {
                if (UserCtl_Right.DGV_SameList.Rows[rowIndex].Visible)
                {
                    UserCtl_Left.DGV_SameList.FirstDisplayedScrollingRowIndex = rowIndex;
                    UserCtl_Right.DGV_SameList.FirstDisplayedScrollingRowIndex = rowIndex;
                }
                else
                    rowIndex = -2;
                goto Find_End;
            }
            foreach (DataGridViewRow row in UserCtl_Right.DGV_DiffList.Rows)
            {
                if (row.Cells[6].Value.ToString().Equals(tb_find_string.Text))
                {
                    rowIndex = row.Index;
                    UserCtl_Right.DGV_DiffList.Rows[rowIndex].Selected = true;
                    UserCtl_Left.DGV_DiffList.Rows[rowIndex].Selected = true;
                    break;
                }
            }
            if (rowIndex != -1)
            {
                if (UserCtl_Right.DGV_DiffList.Rows[rowIndex].Visible)
                {
                    UserCtl_Left.DGV_DiffList.FirstDisplayedScrollingRowIndex = rowIndex;
                    UserCtl_Right.DGV_DiffList.FirstDisplayedScrollingRowIndex = rowIndex;
                }
                else
                    rowIndex = -2;
                goto Find_End;
            }
            foreach (DataGridViewRow row in UserCtl_Right.DGV_AddList.Rows)
            {
                if (row.Cells[6].Value.ToString().Equals(tb_find_string.Text))
                {
                    rowIndex = row.Index;
                    UserCtl_Right.DGV_AddList.Rows[rowIndex].Selected = true;
                    break;
                }
            }
            if (rowIndex != -1)
            {
                if (UserCtl_Right.DGV_AddList.Rows[rowIndex].Visible)
                    UserCtl_Right.DGV_AddList.FirstDisplayedScrollingRowIndex = rowIndex;
                else
                    rowIndex = -2;
                goto Find_End;
            }
            foreach (DataGridViewRow row in UserCtl_Left.DGV_AddList.Rows)
            {
                if (row.Cells[6].Value.ToString().Equals(tb_find_string.Text))
                {
                    rowIndex = row.Index;
                    UserCtl_Left.DGV_AddList.Rows[rowIndex].Selected = true;
                    break;
                }
            }
            if (rowIndex != -1)
            {
                if (UserCtl_Left.DGV_AddList.Rows[rowIndex].Visible)
                    UserCtl_Left.DGV_AddList.FirstDisplayedScrollingRowIndex = rowIndex;
                else
                    rowIndex = -2;
                goto Find_End;
            }
Find_End:
            if (rowIndex == -1)
                MessageBox.Show(string.Format("Not found {0}", tb_find_string.Text), "",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if(rowIndex == -2)
                MessageBox.Show(string.Format("{0} is in anthor mode.", tb_find_string.Text), "",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif

        }

        private void btn_filter_Click(object sender, EventArgs e)
        {
            UserCtl_Right.DGV_SameList.ClearSelection();
            UserCtl_Left.DGV_SameList.ClearSelection();
            UserCtl_Right.DGV_AddList.ClearSelection();
            UserCtl_Left.DGV_AddList.ClearSelection();
            UserCtl_Right.DGV_DiffList.ClearSelection();
            UserCtl_Left.DGV_DiffList.ClearSelection();
            if (tb_find_string.Text == "")
            {
                ComapreAndListItem();
                return;
            }
            else
            {
                foreach (DataGridViewRow row in UserCtl_Right.DGV_SameList.Rows)
                {
                    if (row.Cells[6].Value.ToString().Contains(tb_find_string.Text) && UserCtl_Left.DGV_SameList.Rows[row.Index].Visible)
                    {
                        UserCtl_Left.DGV_SameList.Rows[row.Index].Visible = true;
                        UserCtl_Right.DGV_SameList.Rows[row.Index].Visible = true;
                    }
                    else
                    {
                        UserCtl_Left.DGV_SameList.Rows[row.Index].Visible = false;
                        UserCtl_Right.DGV_SameList.Rows[row.Index].Visible = false;
                    }
                }
                foreach (DataGridViewRow row in UserCtl_Right.DGV_DiffList.Rows)
                {
                    if (row.Cells[6].Value.ToString().Contains(tb_find_string.Text) && UserCtl_Left.DGV_DiffList.Rows[row.Index].Visible)
                    {
                        UserCtl_Left.DGV_DiffList.Rows[row.Index].Visible = true;
                        UserCtl_Right.DGV_DiffList.Rows[row.Index].Visible = true;
                    }
                    else
                    {
                        UserCtl_Left.DGV_DiffList.Rows[row.Index].Visible = false;
                        UserCtl_Right.DGV_DiffList.Rows[row.Index].Visible = false;
                    }
                }
                foreach (DataGridViewRow row in UserCtl_Right.DGV_AddList.Rows)
                {
                    if (row.Cells[6].Value.ToString().Contains(tb_find_string.Text) && UserCtl_Right.DGV_AddList.Rows[row.Index].Visible)
                        UserCtl_Right.DGV_AddList.Rows[row.Index].Visible = true;
                    else
                        UserCtl_Right.DGV_AddList.Rows[row.Index].Visible = false;
                }
                foreach (DataGridViewRow row in UserCtl_Left.DGV_AddList.Rows)
                {
                    if (row.Cells[6].Value.ToString().Contains(tb_find_string.Text) && UserCtl_Left.DGV_AddList.Rows[row.Index].Visible)
                        UserCtl_Left.DGV_AddList.Rows[row.Index].Visible = true;
                    else
                        UserCtl_Left.DGV_AddList.Rows[row.Index].Visible = false;
                }
            }
        }
    }
}