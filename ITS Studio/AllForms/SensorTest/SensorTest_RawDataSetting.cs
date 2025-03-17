using CommonExt;
using ITS_Studio.AllForms;
using Steema.TeeChart.Styles;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ThirdPartyTools;

namespace ITS_Studio
{
    public partial class SensorTest_RawDataSetting : Form
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

        protected DynamicTool MyTool = new DynamicTool();

        public enum Uniformity_Item
        {
            Uniformity_RawData,
            Uniformity_Benchmark,
            MicroOpen
        }

        public Uniformity_Item FormLoad_KeyWord;

        private OneFrame_class OneFrame = new OneFrame_class();
        private int m_X_Ch = 0;
        private int m_Y_Ch = 0;
        private int m_Key_Ch = 0;
        private int NowGridSelectedNode = 0;
        private UIPage MyUIPage = new UIPage();
        public enum RawName_Define
        {
            //[Description("00001b")]
            Default = 0,

            //[Description("00010b")]
            Select = 1,

            //[Description("00100b")]
            Func = 2,

            //[Description("01000b")]
            MinValue = 3,

            //[Description("10000b")]
            MaxValue = 4,
        }

        public struct strFrameInfo
        {
            public List<double> lsFrameData;
            public bool boCheck;
            public double douMax;
            public double douMin;
        }

        public struct strNode
        {
            public string strName;
            public bool boCheck;
            public List<strFrameInfo> lsFrameInfo;
        }

        private List<strNode> m_lsNode = new List<strNode>();
        private Dictionary<string, List<double>> AllFrame = new Dictionary<string, List<double>>();
        private string ProfilePath = "";

        private INI_Tool Read_Profile;
        private List<MultiSenTest_VAR.MainSetting> lsMainSettingBuf;

        private INI_Tool m_logAnalysis_ini;
        Dictionary<Uniformity_SectorName, bool> m_TmpChecked;
        Dictionary<MicroOpen_TestItems, bool> m_TmpMicroOpenChecked;

        public SensorTest_RawDataSetting(string INI_Path, int iDeviceNum)
        {

            InitializeComponent();
            ProfilePath = INI_Path;
            Read_Profile = new INI_Tool(ProfilePath);
            // Hide m_tabControl Appearance
            tabControl_Function.Appearance = TabAppearance.FlatButtons;
            tabControl_Function.ItemSize = new Size(0, 1);
            tabControl_Function.SizeMode = TabSizeMode.Fixed;

            m_logAnalysis_ini = new INI_Tool(ITS_Directory.AnalyzerSetting_path);
            splitContainer_Main.Panel2.Enabled = false;
            m_iSelectedDev = iDeviceNum;
        }

        public Size GetFormActualSize()
        {
            return new Size((btn_ConformCellValue.Location.X + btn_ConformCellValue.Width), (tChart_OneData.Location.Y + tChart_OneData.Height));
        }
        
        private bool LoadOneFrameData(string strKeyWord)
        {
            OneFrame.Node = new List<uint>();
            OneFrame.Node_UpLimit = new List<int>();
            OneFrame.Node_LowLimit = new List<int>();
            OneFrame.Node_Func = new List<int>();

            List<string> ls_Buff = Read_Profile.IniReadAllContains(strKeyWord);
            if(ls_Buff == null)
                return false;
            if(ls_Buff.Count == 0)
                return false;

            object EnumType = null;
            if(Enum.IsDefined(typeof(Uniformity_SectorName), strKeyWord))
                EnumType = Enum.Parse(typeof(Uniformity_SectorName), strKeyWord);
            else if(Enum.IsDefined(typeof(MicroOpen_TestItems), strKeyWord))
                EnumType = Enum.Parse(typeof(MicroOpen_TestItems), strKeyWord);
            else
                return false;

            foreach(string OneRow in ls_Buff)
            {
                List<string> SplitWords = OneRow.Split(';').ToList();
                foreach(string strNode in SplitWords)
                {
                    string[] tmpArr = strNode.Split(',');
                    if(tmpArr.Length < 4)
                        continue;
                    OneFrame.FrameType = EnumType;
                    //Arik fix Convert.ToInt32  FormatException
                    try
                    {
                        OneFrame.Node.Add(Convert.ToUInt32(tmpArr[0]));
                        OneFrame.Node_UpLimit.Add((int)Convert.ToDouble(tmpArr[1]));
                        OneFrame.Node_LowLimit.Add((int)Convert.ToDouble(tmpArr[2]));
                        OneFrame.Node_Func.Add(Convert.ToInt32(tmpArr[3]));
                    }
                    catch(FormatException)
                    {
                        //MessageBox.Show("Invalid String!", "Convert.ToInt32 error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MyUIPage.ShowErrorDialog("Convert.ToInt32 error", "Invalid String!", UIStyle.Red, true);
                        return false;
                    }
                    catch(OverflowException)
                    {
                        //MessageBox.Show("The string cannot fit in 32 bit value!", "Convert.ToInt32 error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MyUIPage.ShowErrorDialog("Convert.ToInt32 error", "The string cannot fit in 32 bit value!", UIStyle.Red, true);
                        return false;
                    }
                }
            }
            return true;
        }

        private void ShowOnGrid(int Frame_Width, int Frame_Height, OneFrame_class FrameData)
        {
            //===== tChart_OneData
            tChart_OneData.Axes.Top.SetMinMax(0, Frame_Width + 1);
            tChart_OneData.Axes.Left.SetMinMax(0, Frame_Height + 1);
            tChart_OneData.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            //tChart_OneData.Panel.Gradient.Visible = false;
            //tChart_OneData.Aspect.View3D = false;

            ColorGridView.Clear();
            ColorGridView.UsePaletteMin = true;
            ColorGridView.ValueFormat = "0";
            ColorGridView.PaletteStep = 1;
            int TotalSteps = 0;
            foreach(var item in EnumTool.EnumToList<RawName_Define>())
                TotalSteps = MyTool.SetIntegerSomeBit(TotalSteps, (int)item, true);
            ColorGridView.PaletteSteps = TotalSteps;

            //OneFrame
            int iX_index = 0, iY_index = 0;
            foreach(int iValue in FrameData.Node)
            {
                if(iX_index == Frame_Width)
                {
                    iX_index = 0;
                    iY_index++;
                }
                //Console.WriteLine("({0},{1}) = {2}", iX_index, iY_index, iValue);
                ColorGridView.Add(iX_index + 1, iValue, iY_index + 1);
                iX_index++;
            }
        }

        private List<string> ReadReport(string[] strFileNames, string KeyWord)
        {
            List<string> lsDirs = new List<string>();
            foreach(string Onefile in strFileNames)
            {
                var logFile = File.ReadAllLines(Onefile);
                var logList = new List<string>(logFile);
                if(logList.Exists(x => x.Contains(KeyWord)))
                    lsDirs.Add(Onefile);
            }
            return lsDirs;
        }

        private void m_tsmClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach(TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if(node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private bool SaveAdjustOneFrame(string strProfilePath, string SectorName, int iX_Ch, int iY_Ch)
        {
            if(string.IsNullOrEmpty(strProfilePath))
                return false;

            if(OneFrame.Node.Count != (iY_Ch * iX_Ch))
                return false;
            Read_Profile.ClearSector(SectorName);

            //OneFrame
            StringBuilder StringArr = new StringBuilder();
            {
                for(int iRow = 0; iRow < iY_Ch; iRow++)
                {
                    for(int iCol = 0; iCol < iX_Ch; iCol++)
                    {
                        //int index = (iCol * iX_Ch) + iRow;
                        int index = (iRow * iX_Ch) + iCol;
                        StringArr.AppendFormat("{0},{1},{2},{3};", OneFrame.Node[index], OneFrame.Node_UpLimit[index], OneFrame.Node_LowLimit[index], OneFrame.Node_Func[index]);
                    }
                    StringArr.AppendLine("");
                }
            }


            Read_Profile.IniWriteAllSector(SectorName, StringArr);
            return true;
        }

        private void SensorTest_RawDataSetting_Load(object sender, EventArgs e)
        {
            grpBox_DefaultValue.Enabled = false;
            MultiSenTest_VAR.TestStatus NowTestitem;
            splitContainer_Main.Enabled = true;
            lsMainSettingBuf = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.lsMainSetting.FindAll(item => item.strName == "Panel_Info");
            int.TryParse(lsMainSettingBuf.Find(x => x.strKey == enPanel_Info.XChannel.ToString()).strValue, out m_X_Ch);
            int.TryParse(lsMainSettingBuf.Find(x => x.strKey == enPanel_Info.YChannel.ToString()).strValue, out m_Y_Ch);
            int.TryParse(lsMainSettingBuf.Find(x => x.strKey == enPanel_Info.TouchKeyChannel.ToString()).strValue, out m_Key_Ch);
            ComboBox_CDC_Type.Visible = false;

            uiTabControl_UniDataOpt.Visible = false;
            uiTabControl_McrOpenDataOpt.Visible = false;

            //uiSplitContainer1.SplitterDistance = uiSplitContainer1.Width / 2;
            switch(FormLoad_KeyWord)
            {
                case Uniformity_Item.Uniformity_RawData:
                    NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enUniformityTest];
                    ResetUniformity_UI(NowTestitem);
                    break;

                case Uniformity_Item.MicroOpen:
                    NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enMicroOpenTest];
                    ResetMicroOpen_UI(NowTestitem);
                    break;
            }            
        }
        
        public void ConformCheckedBuffFunc()
        {
            if(m_TmpChecked != null)
            {
                MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enUniformityTest];
                foreach(var item in m_TmpChecked)
                {
                    string _name = string.Format("{0}_En", item.Key);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, _name, item.Value);
                }
            }
            if (m_TmpMicroOpenChecked != null)
            {
                MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enMicroOpenTest];
                foreach (var item in m_TmpMicroOpenChecked)
                {
                    string _name = string.Format("{0}_En", item.Key);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, _name, item.Value);
                }
            }
        }
        
        private void ResetMicroOpen_UI(MultiSenTest_VAR.TestStatus InNowTestitem)
        {
            splitContainer_Main.Panel2.Enabled = true;
            grpBox_Allnode_Def_Spec.Enabled = false;
            uiSplitContainer1.SplitterDistance = (int)((double)uiSplitContainer1.Width * 0.5);
            tabControl_Function.SelectedTab = tabPage_MicroOpen;

            ckListBox_MicroOpen.ItemCheck -= ckListBox_ItemCheck;
            ckListBox_MicroOpen.Items.Clear();
            var tmpList = EnumTool.EnumToList<MicroOpen_TestItems>();
            ckListBox_MicroOpen.Items.AddRange(tmpList.Select(x => x.ToString()).ToArray());
            bool bEnable = false;
            int ItemCnts = 0;
            foreach(var item in tmpList)
            {
                string strKey = string.Format("{0}_En", item);
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetSpecData(InNowTestitem.SPEC, strKey, out bEnable);
                List<string> ls_Buff = Read_Profile.IniReadAllContains(item.ToString());
                switch(item)
                {
                    case MicroOpen_TestItems.RX_Delta:
                        if(!bEnable)
                            break;

                        if((ls_Buff == null) || (ls_Buff.Count == 0))
                        {
                            MyUIPage.ShowErrorDialog("Tip", "MicroOpen format error!", UIStyle.Red, true);
                            splitContainer_Main.Enabled = false;
                            return;
                        }
                        List<string> splitwords = ls_Buff[0].Split(';').ToList();//多一個，所以必須減一
                        int _y_ch = m_Y_Ch;
                        int _x_ch = m_X_Ch - 1;
                        if((_y_ch != ls_Buff.Count) || (_x_ch != splitwords.Count - 1))
                        {
                            MyUIPage.ShowErrorDialog("Tip", "MicroOpen Nodes not mapping!", UIStyle.Red, true);
                            splitContainer_Main.Enabled = false;
                            return;
                        }
                        break;

                    case MicroOpen_TestItems.TX_Avg_Delta:
                        if(!bEnable && !UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable)
                            break;

                        if(ls_Buff == null)
                        {
                            MyUIPage.ShowErrorDialog("Tip", "MicroOpen format error!", UIStyle.Red, true);
                            splitContainer_Main.Enabled = false;
                            return;
                        }

                        if(m_Y_Ch - 1 != ls_Buff.Count)
                        {
                            MyUIPage.ShowErrorDialog("Tip", "MicroOpen Tx Avg not mapping!", UIStyle.Red, true);
                            splitContainer_Main.Enabled = false;
                            return;
                        }
                        break;
                }

                ckListBox_MicroOpen.SetItemChecked(ItemCnts, bEnable);
                ItemCnts++;
            }

            ckListBox_MicroOpen.ItemCheck += ckListBox_ItemCheck;

            grpBox_MicroOpen_Def_Spec.Enabled = false;
          
            MultiSenTest_VAR.TestStatus NowTestitemMircoOpen = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enMicroOpenTest];
            m_TmpMicroOpenChecked = new Dictionary<MicroOpen_TestItems, bool>();
            foreach (var item in EnumTool.EnumToList<MicroOpen_TestItems>())
            {
                string _name = string.Format("{0}_En", item);
                bool bFlag = false;
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetSpecData(NowTestitemMircoOpen.SPEC, _name, out bFlag);
                m_TmpMicroOpenChecked.Add(item, bFlag);
            }
        }

        private void ResetUniformity_UI(MultiSenTest_VAR.TestStatus InNowTestitem)
        {
            tabControl_Function.SelectedTab = tabPage_Uniformity;
            grpBox_Allnode_Def_Spec.Enabled = false;
            ckListBox_AllNode.ItemCheck -= ckListBox_ItemCheck;
            ckListBox_AllNode.Items.Clear();
            var lsSectorNames = EnumTool.EnumToList<Uniformity_SectorName>().Select(x => x).ToList();

            if(m_Key_Ch == 0)
                lsSectorNames.Remove(Uniformity_SectorName.Uniformity_KeyRawData);

            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
            {
                lsSectorNames.Remove(Uniformity_SectorName.Uniformity_TxDiff);
                lsSectorNames.Remove(Uniformity_SectorName.Uniformity_RxDiff);
                ComboBox_CDC_Type.SelectedIndexChanged -= ComboBox_CDC_Type_SelectedIndexChanged;
                ComboBox_CDC_Type.Visible = true;

                ComboBox_CDC_Type.SelectedItem = Read_Profile.IniReadBool("Uniformity_Test", "V3_BK_Enable", false) ? V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_ALL_NODE.ToString() : V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_ALL_NODE_NBK.ToString();
                ComboBox_CDC_Type.SelectedIndexChanged += ComboBox_CDC_Type_SelectedIndexChanged;
            }
            ////判斷Golden第一個數值是否為-1，-1代表不含Golden
            ckListBox_AllNode.Items.AddRange(lsSectorNames.Select(x => x.ToString()).ToArray());
            grpBox_AllNodeEns.Height = ckListBox_AllNode.GetItemHeight(0) * (lsSectorNames.Count + 2);
            bool bEnable = false;
            int ItemCnts = 0;
            foreach(var sector in lsSectorNames)
            {
                string strKey = string.Format("{0}_En", sector);
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetSpecData(InNowTestitem.SPEC, strKey, out bEnable);
                if(bEnable || UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable)
                {
                    var SectorName = sector.ToString();
                    
                    List<string> lsUniformity_Buff = Read_Profile.IniReadAllContains(SectorName);
                    if(lsUniformity_Buff == null)
                    {
                        MyUIPage.ShowErrorDialog("Tip", string.Format("[{0}] Uniformity format error!", sector), UIStyle.Red);
                        splitContainer_Main.Enabled = false;
                        return;
                    }

                    int _Y_Ch = 0;
                    int _X_Ch = 0;
                    switch(sector)
                    {
                        case Uniformity_SectorName.Uniformity_KeyRawData:
                            _Y_Ch = 1;
                            _X_Ch = m_Key_Ch;
                            break;
                        case Uniformity_SectorName.Uniformity_RawData:
                            _Y_Ch = m_Y_Ch;
                            _X_Ch = m_X_Ch;
                            break;
                        case Uniformity_SectorName.Uniformity_RxDiff:
                            _Y_Ch = m_Y_Ch;
                            _X_Ch = m_X_Ch - 1;
                            break;
                        case Uniformity_SectorName.Uniformity_TxDiff:
                        case Uniformity_SectorName.Uniformity_Win1:
                            _Y_Ch = m_Y_Ch - 1;
                            _X_Ch = m_X_Ch;
                            break;
                        case Uniformity_SectorName.Uniformity_Win2:
                            _Y_Ch = m_Y_Ch - 1;
                            _X_Ch = m_X_Ch - 1;
                            break;
                    }

                    if(lsUniformity_Buff.Count != 0)
                    {
                        List<string> SplitWords = lsUniformity_Buff[0].Split(';').ToList();
                        if((_Y_Ch != lsUniformity_Buff.Count) || (_X_Ch != SplitWords.Count - 1))
                        {
                            lbNowSelect.Text = "Nodes not mapping!";
                            splitContainer_Main.Enabled = false;
                            return;
                        }
                    }
                }
                ckListBox_AllNode.SetItemChecked(ItemCnts, bEnable);
                ItemCnts++;
            }

            ckListBox_AllNode.ItemCheck += ckListBox_ItemCheck;

            MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enUniformityTest];
            m_TmpChecked = new Dictionary<Uniformity_SectorName, bool>();
          
            foreach(var item in EnumTool.EnumToList<Uniformity_SectorName>())
            {
                string _name = string.Format("{0}_En", item);
                bool bFlag = false;
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetSpecData(NowTestitem.SPEC, _name, out bFlag);
                m_TmpChecked.Add(item, bFlag);
            }
         
        }

        private void Txt_Golden_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                int iNowValue = 0;
                int.TryParse(((TextBox)sender).Text, out iNowValue);
                if(iNowValue > 100)
                    ((TextBox)sender).Text = "100";
            }

            MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enUniformityTest];
            if(((TextBox)sender).Name == "Txt_Golden_Up")
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, "Benchmark_UpLimit(%)", ((TextBox)sender).Text);
            else if(((TextBox)sender).Name == "Txt_Golden_Low")
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, "Benchmark_LowLimit(%)", ((TextBox)sender).Text);
        }

        private void ChangeSpecValue(int iValue, RawName_Define DataType, ref OneFrame_class _Frame)
        {
            int iDefault = 0;
            if(Enum.IsDefined(typeof(Uniformity_SectorName), _Frame.FrameType.ToString()))
            {
                string strSectorName = TestItemNameDefine.enUniformityTest.DescArr(':')[1];
                var item = (Uniformity_SectorName)_Frame.FrameType;
                switch(item)
                {
                    case Uniformity_SectorName.Uniformity_RawData:
                        if(DataType == RawName_Define.MaxValue)
                        {
                            //iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_Max_Threshold.ToString(), 0);
                            iDefault = 5000;
                            _Frame.Node_UpLimit[NowGridSelectedNode] = iValue;
                        }
                        else if(DataType == RawName_Define.MinValue)
                        {
                            //iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_Min_Threshold.ToString(), 0);
                            iDefault = 0;
                            _Frame.Node_LowLimit[NowGridSelectedNode] = iValue;
                        }
                        ShowOnGrid(m_X_Ch, m_Y_Ch, _Frame);
                        break;

                    case Uniformity_SectorName.Uniformity_Win1:
                        if(DataType == RawName_Define.MaxValue)
                        {
                            //iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_UniformityTest.Uniformity_Win1_Max_Threshold.ToString(), 0);
                            iDefault = 5000;
                            _Frame.Node_UpLimit[NowGridSelectedNode] = iValue;
                        }
                        ShowOnGrid(m_X_Ch, m_Y_Ch - 1, _Frame);
                        break;

                    case Uniformity_SectorName.Uniformity_Win2:
                        if(DataType == RawName_Define.MaxValue)
                        {
                            //iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_UniformityTest.Uniformity_Win2_Max_Threshold.ToString(), 0);
                            iDefault = 5000;
                            _Frame.Node_UpLimit[NowGridSelectedNode] = iValue;
                        }
                        ShowOnGrid(m_X_Ch - 1, m_Y_Ch - 1, _Frame);
                        break;

                    case Uniformity_SectorName.Uniformity_KeyRawData:
                        //m_Key_Ch
                        if(DataType == RawName_Define.MaxValue)
                        {
                            iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_UniformityTest.Uniformity_KeyRawData_Max_Threshold.ToString(), 0);
                            _Frame.Node_UpLimit[NowGridSelectedNode] = iValue;
                        }
                        else if(DataType == RawName_Define.MinValue)
                        {
                            iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_UniformityTest.Uniformity_KeyRawData_Min_Threshold.ToString(), 0);
                            _Frame.Node_LowLimit[NowGridSelectedNode] = iValue;
                        }
                        ShowOnGrid(m_Key_Ch, 1, _Frame);
                        break;

                    //Y少一條
                    case Uniformity_SectorName.Uniformity_TxDiff:
                        double TxDiff_tmp = (double)NowGridSelectedNode;
                        bool bIsTxDiffEdge = false;

                        if(TxDiff_tmp > (m_X_Ch * 0) && TxDiff_tmp < (m_X_Ch * 1))
                            bIsTxDiffEdge = true;
                        else if(TxDiff_tmp > (m_X_Ch * (m_Y_Ch - 2)) && TxDiff_tmp < (m_X_Ch * (m_Y_Ch - 1)))
                            bIsTxDiffEdge = true;
                        else if((TxDiff_tmp % m_X_Ch) == 0)
                            bIsTxDiffEdge = true;
                        else if(((TxDiff_tmp + 1) % m_X_Ch) == 0)
                            bIsTxDiffEdge = true;

                        //string strTxDiff = (bIsTxDiffEdge)
                        //    ? INI_Name_UniformityTest.Uniformity_TxDiffEdge_Max_Threshold.ToString()
                        //    : INI_Name_UniformityTest.Uniformity_TxDiff_Max_Threshold.ToString();

                        //iDefault = Read_Profile.IniReadValue(strSectorName, strTxDiff, "0%").Replace("%", "").ToInt();
                        iDefault = bIsTxDiffEdge ? 100 : 5000;
                        _Frame.Node_UpLimit[NowGridSelectedNode] = iValue;

                        ShowOnGrid(m_X_Ch, m_Y_Ch - 1, _Frame);
                        break;

                    //X少一條
                    case Uniformity_SectorName.Uniformity_RxDiff:
                        //Console.WriteLine(NowGridSelectedNode);
                        double RxDiff_tmp = (double)NowGridSelectedNode;
                        bool bIsRxDiffEdge = false;

                        if(RxDiff_tmp > ((m_X_Ch - 1) * 0) && RxDiff_tmp < ((m_X_Ch - 1) * 1))
                            bIsRxDiffEdge = true;
                        else if(RxDiff_tmp > ((m_X_Ch - 1) * (m_Y_Ch - 1)) && RxDiff_tmp < ((m_X_Ch - 1) * m_Y_Ch))
                            bIsRxDiffEdge = true;
                        else if((RxDiff_tmp % (m_X_Ch - 1)) == 0)
                            bIsRxDiffEdge = true;
                        else if(((RxDiff_tmp + 1) % (m_X_Ch - 1)) == 0)
                            bIsRxDiffEdge = true;

                        //string strRxDiff = (bIsRxDiffEdge)
                        //    ? INI_Name_UniformityTest.Uniformity_RxDiffEdge_Max_Threshold.ToString()
                        //    : INI_Name_UniformityTest.Uniformity_RxDiff_Max_Threshold.ToString();
                        
                        //iDefault = Read_Profile.IniReadValue(strSectorName, strRxDiff, "0%").Replace("%", "").ToInt();
                        iDefault = bIsRxDiffEdge ? 100 : 5000;
                        _Frame.Node_UpLimit[NowGridSelectedNode] = iValue;

                        ShowOnGrid(m_X_Ch - 1, m_Y_Ch, _Frame);
                        break;
                }
            }
            else if(Enum.IsDefined(typeof(MicroOpen_TestItems), _Frame.FrameType.ToString()))
            {
                string strSectorName = TestItemNameDefine.enMicroOpenTest.DescArr(':')[1];
                var item = (MicroOpen_TestItems)_Frame.FrameType;
                switch(item)
                {
                    case MicroOpen_TestItems.RX_Delta:
                        if(DataType == RawName_Define.MaxValue)
                        {
                            //iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_MicroOpenTest.RX_Delta_Threshold.ToString(), 0);
                            _Frame.Node_UpLimit[NowGridSelectedNode] = iValue;
                        }
                        ShowOnGrid(m_X_Ch - 1, m_Y_Ch, _Frame);
                        break;

                    case MicroOpen_TestItems.TX_Avg_Delta:
                        if(DataType == RawName_Define.MaxValue)
                        {
                            iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_MicroOpenTest.TX_Avg_Delta_Threshold.ToString(), 0);
                            _Frame.Node_UpLimit[NowGridSelectedNode] = iValue;
                        }
                        ShowOnGrid(1, m_Y_Ch - 1, _Frame);
                        break;
                }
            }

            if(DataType == RawName_Define.Func)
                _Frame.Node_Func[NowGridSelectedNode] = iValue;

            uint iTmp = _Frame.Node[NowGridSelectedNode];
            bool bTmp = (iValue != iDefault) ? true : false;
            _Frame.Node[NowGridSelectedNode] = MyTool.SetIntegerSomeBit(iTmp, (int)DataType, bTmp);
        }

        private void cbNodeTest_En_Click(object sender, EventArgs e)
        {
            bool bChecked = ((CheckBox)sender).Checked;
            foreach(var Bit in (enDefineFunction[])Enum.GetValues(typeof(enDefineFunction)))
            {
                if(Bit == enDefineFunction.NodeTest_En)
                {
                    int itemValue = OneFrame.Node_Func[NowGridSelectedNode];
                    itemValue = MyTool.SetIntegerSomeBit(itemValue, (int)Bit, bChecked);
                    ChangeSpecValue(itemValue, RawName_Define.Func, ref OneFrame);
                }
            }
            grpBox_AllNodeEns.Enabled = false;
            richTextBox_Hint.Text = "";
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // e.KeyChar == (Char)48 ~ 57 -----> 0~9
            // e.KeyChar == (Char)8 -----------> Backpace
            // e.KeyChar == (Char)13-----------> Enter
            if((e.KeyChar >= 48) && (e.KeyChar <= 57)) // e.KeyChar == (Char)48 ~ 57 -----> 0~9
                e.Handled = false;
            else if(e.KeyChar == (Char)_KeyEventArg.ENTER || e.KeyChar == (Char)_KeyEventArg.Backpace)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void Txt_Golden_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(((TextBox)sender).Text))
                ((TextBox)sender).Text = "100";
        }

        private T[,] OneD_2<T>(T[] obj, int len)
        {
            if(obj.Length % len != 0)
                return null;
            int width = obj.Length / len;
            T[,] obj2 = new T[len, width];
            for(int i = 0; i < obj.Length; i++)
            {
                obj2[i / width, i % width] = obj[i];
            }
            return obj2;
        }
        
        public static T[] TwoD_1<T>(T[,] obj)
        {
            T[] obj2 = new T[obj.Length];
            for(int i = 0; i < obj.Length; i++)
                obj2[i] = obj[i / obj.GetLength(1), i % obj.GetLength(1)];
            return obj2;
        }

        public bool GetAvoidCornerStatus()
        {
            return Def_cbNodeTest_En.Checked;
        }

        enum enType
        {
            Uniformity_RawDataMax,
            Uniformity_RawDataMin
        }

        private List<int> CalculatePercentValue(string ThresholdPercent,string EdgeThresholdPercent, enType _Type)
        {            
            var GoldenListData = Read_Profile.IniReadAllContains("Uniformity_Golden").Select(x => x.Split(";").Where(str_2 => !string.IsNullOrEmpty(str_2)).ToArray()).ToArray();            
            if(_Type == enType.Uniformity_RawDataMax)
            {
                double iUp_Limit = 0, iUp_Limit_Edge = 0;
                double.TryParse(Txt_MaxThresholdPercent.Text, out iUp_Limit);
                double.TryParse(Txt_MaxEdgeThresholdPercent.Text, out iUp_Limit_Edge);
                
                List<int> list = new List<int>();
                for(int iX = 0; iX < GoldenListData.Length; iX++)
                {
                    for(int iY = 0; iY < GoldenListData[iX].Length; iY++)
                    {
                        bool IsEdge = (iX == 0) || (iY == 0) || (iX == GoldenListData.GetUpperBound(0)) || (iY == GoldenListData[iX].GetUpperBound(0));
                        if(IsEdge)
                        {
                            var num_EdgeGolden = double.Parse(GoldenListData[iX][iY].Split(",")[1]);
                            list.Add((int)(num_EdgeGolden * (iUp_Limit_Edge / 100 + 1) + 0.5));
                        }
                        else
                        {
                            var num_Golden = double.Parse(GoldenListData[iX][iY].Split(",")[1]);
                            list.Add((int)(num_Golden * (iUp_Limit / 100 + 1) + 0.5));
                        }                        
                    }
                }
                return list;                
            }
            if(_Type == enType.Uniformity_RawDataMin)
            {
                double iLow_Limit = 0, iLow_Limit_Edge = 0;
                double.TryParse(Txt_MinThresholdPercent.Text, out iLow_Limit);
                double.TryParse(Txt_MinEdgeThresholdPercent.Text, out iLow_Limit_Edge);

                List<int> list = new List<int>();
                for(int iX = 0; iX < GoldenListData.Length; iX++)
                {
                    for(int iY = 0; iY < GoldenListData[iX].Length; iY++)
                    {
                        bool IsEdge = (iX == 0) || (iY == 0) || (iX == GoldenListData.GetUpperBound(0)) || (iY == GoldenListData[iX].GetUpperBound(0));
                        if(IsEdge)
                        {
                            var num_EdgeGolden = double.Parse(GoldenListData[iX][iY].Split(",")[1]);
                            list.Add((int)(num_EdgeGolden * (1 - iLow_Limit_Edge / 100) + 0.5));
                        }
                        else
                        {
                            var num_Golden = double.Parse(GoldenListData[iX][iY].Split(",")[1]);
                            list.Add((int)(num_Golden * (1 - iLow_Limit / 100) + 0.5));
                        }
                    }
                }
                return list;      
            }
            return null;
        }

        private void btn_RestoreDefault_Click(object sender, EventArgs e)
        {
            if(MyUIPage.ShowAskDialog("Warmning", "Recover Default Settings ?", UIStyle.Orange, true) == false)
                return;
            List<uint> NodeStatus = OneFrame.Node.Select(x => x = 0).ToList();
            OneFrame.Node = NodeStatus;
            if(tabControl_Function.SelectedTab == tabPage_Uniformity)
            {
                Uniformity_SectorName _SelectStr;
                if(!Uniformity_SectorName.TryParse(lbNowSelect.Text, out _SelectStr))
                    return;
                int iUp_Limit = 0;
                //===============================
                // Max  
                if(panel_MaxPercent.Visible)
                {
                    //根據百分比，Golden數值 * (1 + Percent) 取4捨5入                    
                    OneFrame.Node_UpLimit = CalculatePercentValue(Txt_MaxThresholdPercent.Text, Txt_MaxEdgeThresholdPercent.Text, enType.Uniformity_RawDataMax);
                }
                else
                {
                    //Fix Value setting
                    int.TryParse(Txt_MaxUniformity.Text, out iUp_Limit);
                    OneFrame.Node_UpLimit = OneFrame.Node_UpLimit.Select(x => x = iUp_Limit).ToList();
                }
                //===============================
                //遇到TxDiff & RxDiff重新再算一次Node_UpLimit
                if((_SelectStr == Uniformity_SectorName.Uniformity_TxDiff) || (_SelectStr == Uniformity_SectorName.Uniformity_RxDiff))
                {
                    int i_Y_Ch = (_SelectStr == Uniformity_SectorName.Uniformity_TxDiff) ? m_Y_Ch - 1 : m_Y_Ch;
                    var tmpArr = OneD_2(OneFrame.Node_UpLimit.ToArray(), i_Y_Ch);
                    int.TryParse(Txt_MinUniformity.Text, out iUp_Limit);
                    for(int x = 0; x < tmpArr.GetLength(0); x++)
                    {
                        for(int y = 0; y < tmpArr.GetLength(1); y++)
                        {
                            if((x == 0) || x == tmpArr.GetLength(0) - 1)
                                tmpArr[x, y] = iUp_Limit;
                            else if((y == 0) || (y == tmpArr.GetLength(1) - 1))
                                tmpArr[x, y] = iUp_Limit;
                        }
                    }
                    OneFrame.Node_UpLimit = TwoD_1(tmpArr).ToList();
                }
                
                //===============================
                // Min
                int iLow_Limit = -1;
                if((_SelectStr == Uniformity_SectorName.Uniformity_RawData) || (_SelectStr == Uniformity_SectorName.Uniformity_KeyRawData))
                {
                    if(panel_MinPercent.Visible)
                    {
                        //根據百分比，Golden數值 * (1 + Percent) 取4捨5入                    
                        OneFrame.Node_LowLimit = CalculatePercentValue(Txt_MinThresholdPercent.Text, Txt_MinEdgeThresholdPercent.Text, enType.Uniformity_RawDataMin);
                    }
                    else
                    {
                        int.TryParse(Txt_MinUniformity.Text, out iLow_Limit);
                        OneFrame.Node_LowLimit = OneFrame.Node_LowLimit.Select(x => x = iLow_Limit).ToList();                        
                    }                    
                }
                //Def_cbNodeTest_En
                int Def_FuncNumber = 0;
                foreach(var Bit in (enDefineFunction[])Enum.GetValues(typeof(enDefineFunction)))
                    if(Bit == enDefineFunction.NodeTest_En)
                        Def_FuncNumber = MyTool.SetIntegerSomeBit(Def_FuncNumber, (int)Bit, Def_cbNodeTest_En.Checked);

                List<int> AvgNode_Func = OneFrame.Node_Func.Select(x => x = Def_FuncNumber).ToList();
                OneFrame.Node_Func = AvgNode_Func;

                grpBox_AllNodeEns.Enabled = false;
                richTextBox_Hint.Text = "";

                if(_SelectStr == Uniformity_SectorName.Uniformity_RawData)
                    ShowOnGrid(m_X_Ch, m_Y_Ch, OneFrame);
                else if(_SelectStr == Uniformity_SectorName.Uniformity_Win1)
                    ShowOnGrid(m_X_Ch, m_Y_Ch - 1, OneFrame);
                else if(_SelectStr == Uniformity_SectorName.Uniformity_Win2)
                    ShowOnGrid(m_X_Ch - 1, m_Y_Ch - 1, OneFrame);
                else if(_SelectStr == Uniformity_SectorName.Uniformity_KeyRawData)
                    ShowOnGrid(m_Key_Ch, 1, OneFrame);
                else if(_SelectStr == Uniformity_SectorName.Uniformity_TxDiff)
                    ShowOnGrid(m_X_Ch, m_Y_Ch - 1, OneFrame);
                else if(_SelectStr == Uniformity_SectorName.Uniformity_RxDiff)
                    ShowOnGrid(m_X_Ch - 1, m_Y_Ch, OneFrame);
            }
            else if(tabControl_Function.SelectedTab == tabPage_MicroOpen)
            {
                int iMicroOpen_threshold = 0;
                int.TryParse(Txt_MicroOpen_threshold.Text, out iMicroOpen_threshold);
                OneFrame.Node_UpLimit = OneFrame.Node_UpLimit.Select(x => x = iMicroOpen_threshold).ToList();

                //Def_cbNodeTest_En
                int Def_FuncNumber = 0;
                foreach(var Bit in (enDefineFunction[])Enum.GetValues(typeof(enDefineFunction)))
                    if(Bit == enDefineFunction.NodeTest_En)
                        Def_FuncNumber = MyTool.SetIntegerSomeBit(Def_FuncNumber, (int)Bit, Def_cbNodeTest_En.Checked);
                OneFrame.Node_Func = OneFrame.Node_Func.Select(x => x = Def_FuncNumber).ToList();

                grpBox_TestEns.Enabled = false;
                richTextBox_Hint.Text = "";

                MicroOpen_TestItems _SelectStr;
                if(!MicroOpen_TestItems.TryParse(lbNowSelect.Text, out _SelectStr))
                    return;

                if(_SelectStr == MicroOpen_TestItems.RX_Delta)
                    ShowOnGrid(m_X_Ch - 1, m_Y_Ch, OneFrame);
                else if(_SelectStr == MicroOpen_TestItems.TX_Avg_Delta)
                    ShowOnGrid(1, m_Y_Ch - 1, OneFrame);
            }

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = true;
            timer_BlinkRestoreButton.Enabled = false;

            btn_RestoreDefault.BackColor = Color.SeaShell;

            bool bResult = false;
            if(tabControl_Function.SelectedTab == tabPage_Uniformity)
                bResult = SaveUniformity_RawData();
            else if(tabControl_Function.SelectedTab == tabPage_MicroOpen)
                bResult = SaveMicroOpen_RawData();

            if(bResult)
                MyUIPage.ShowSuccessDialog("", "Save Success !", UIStyle.Green, true);
            else
                MyUIPage.ShowErrorDialog("", "Profile not correct !", UIStyle.Red, true);
        }

        public bool SaveMicroOpen_RawData()
        {
            bool bResult = false;
            string strSectorName = TestItemNameDefine.enMicroOpenTest.DescArr(':')[1];
            MicroOpen_TestItems _SelectStr;
            if(!MicroOpen_TestItems.TryParse(lbNowSelect.Text, out _SelectStr))
                return bResult;
            MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enMicroOpenTest];
            string str = "";
            switch(_SelectStr)
            {
                case MicroOpen_TestItems.RX_Delta:
                    bResult = SaveAdjustOneFrame(ProfilePath, _SelectStr.ToString(), m_X_Ch - 1, m_Y_Ch);

                    //str = INI_Name_MicroOpenTest.RX_Delta_Threshold.ToString();
                    //Read_Profile.IniWriteValue(strSectorName, str, Txt_MicroOpen_threshold.Text);
                    //UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, str, Txt_MicroOpen_threshold.Text);

                    str = INI_Name_MicroOpenTest.RX_Delta_Threshold_Tolerance.ToString();
                    Read_Profile.IniWriteValue(strSectorName, str, Txt_RX_Delta_Tolerance.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, str, Txt_RX_Delta_Tolerance.Text);

                    break;

                case MicroOpen_TestItems.TX_Avg_Delta:
                    bResult = SaveAdjustOneFrame(ProfilePath, _SelectStr.ToString(), 1, m_Y_Ch - 1);

                    str = INI_Name_MicroOpenTest.TX_Avg_Delta_Threshold.ToString();
                    Read_Profile.IniWriteValue(strSectorName, str, Txt_MicroOpen_threshold.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, str, Txt_MicroOpen_threshold.Text);

                    str = INI_Name_MicroOpenTest.TX_Avg_Delta_Threshold_AvoidCorner.ToString();
                    Read_Profile.IniWriteValue(strSectorName, str, TX_Avg_AvoidCorner.Checked.ToString());
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, str, TX_Avg_AvoidCorner.Checked);
                    break;
            }

            if(bResult)
            {
                foreach(var item_2 in EnumTool.EnumToList<MicroOpen_TestItems>())
                    Read_Profile.IniWriteValue(strSectorName, string.Format("{0}_En", item_2), bool.FalseString);

                //foreach(var item in ckListBox_MicroOpen.CheckedItems)
                //    if(Enum.IsDefined(typeof(MicroOpen_TestItems), item))
                //        Read_Profile.IniWriteValue(strSectorName, string.Format("{0}_En", item), bool.TrueString);
          
                foreach (var item in m_TmpMicroOpenChecked)
                {
                    Read_Profile.IniWriteValue(strSectorName, string.Format("{0}_En", item.Key), bool.TrueString);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, string.Format("{0}_En", item.Key), item.Value);
                }
            }
            grpBox_TestEns.Enabled = true;
            grpBox_MicroOpen_Def_Spec.Enabled = true;
            return bResult;
        }

        private bool  SaveUniformity_RawData()
        {
            bool bResult = false;
            string strSectorName = TestItemNameDefine.enUniformityTest.DescArr(':')[1];
            Uniformity_SectorName _SelectStr;
            if(!Uniformity_SectorName.TryParse(lbNowSelect.Text, out _SelectStr))
                return bResult;
            MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enUniformityTest];
            
            switch(_SelectStr)
            {
                case Uniformity_SectorName.Uniformity_KeyRawData:
                    bResult = SaveAdjustOneFrame(ProfilePath, _SelectStr.ToString(), m_Key_Ch, 1);
                    Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_KeyRawData_Max_Threshold.ToString(), Txt_MaxUniformity.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_KeyRawData_Max_Threshold.ToString(), Txt_MaxUniformity.Text);

                    Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_KeyRawData_Min_Threshold.ToString(), Txt_MinUniformity.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_KeyRawData_Min_Threshold.ToString(), Txt_MinUniformity.Text);
                    break;

                case Uniformity_SectorName.Uniformity_RawData:
                    bResult = SaveAdjustOneFrame(ProfilePath, _SelectStr.ToString(), m_X_Ch, m_Y_Ch);
                    //Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_Max_Threshold.ToString(), Txt_MaxUniformity.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_RawData_Max_Threshold.ToString(), Txt_MaxUniformity.Text);

                    //Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_Min_Threshold.ToString(), Txt_MinUniformity.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_RawData_Min_Threshold.ToString(), Txt_MinUniformity.Text);

                    Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_Max_Threshold_Tolerance.ToString(), Txt_MaxTolerance.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_RawData_Max_Threshold_Tolerance.ToString(), Txt_MaxTolerance.Text);

                    Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_Min_Threshold_Tolerance.ToString(), Txt_MinTolerance.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_RawData_Min_Threshold_Tolerance.ToString(), Txt_MinTolerance.Text);

                    break;

                case Uniformity_SectorName.Uniformity_RawData_MinMax:
                    bResult = true;
                    Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_MinMax_Min_Threshold.ToString(), Txt_MaxUniformity.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_RawData_MinMax_Min_Threshold.ToString(), Txt_MaxUniformity.Text);
                    break;

                case Uniformity_SectorName.Uniformity_Win1:
                    bResult = SaveAdjustOneFrame(ProfilePath, _SelectStr.ToString(), m_X_Ch, m_Y_Ch - 1);
                    //Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_Win1_Max_Threshold.ToString(), Txt_MaxUniformity.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_Win1_Max_Threshold.ToString(), Txt_MaxUniformity.Text);

                    Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_Win1_Max_Threshold_Tolerance.ToString(), Txt_MaxTolerance.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_Win1_Max_Threshold_Tolerance.ToString(), Txt_MaxTolerance.Text);
                    break;

                case Uniformity_SectorName.Uniformity_Win2:
                    bResult = SaveAdjustOneFrame(ProfilePath, _SelectStr.ToString(), m_X_Ch - 1, m_Y_Ch - 1);
                    //Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_Win2_Max_Threshold.ToString(), Txt_MaxUniformity.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_Win2_Max_Threshold.ToString(), Txt_MaxUniformity.Text);

                    Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_Win2_Max_Threshold_Tolerance.ToString(), Txt_MaxTolerance.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_Win2_Max_Threshold_Tolerance.ToString(), Txt_MaxTolerance.Text);
                    break;

                case Uniformity_SectorName.Uniformity_TxDiff:
                    bResult = SaveAdjustOneFrame(ProfilePath, _SelectStr.ToString(), m_X_Ch, m_Y_Ch - 1);

                    //Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_TxDiff_Max_Threshold.ToString(), Txt_MaxUniformity.Text + "%");
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_TxDiff_Max_Threshold.ToString(), Txt_MaxUniformity.Text);

                    //Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_TxDiff_Max_Threshold_Tolerance.ToString(), Txt_MaxTolerance.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_TxDiff_Max_Threshold_Tolerance.ToString(), Txt_MaxTolerance.Text);

                    //Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_TxDiffEdge_Max_Threshold.ToString(), Txt_MinUniformity.Text + "%");
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_TxDiffEdge_Max_Threshold.ToString(), Txt_MinUniformity.Text);

                    //Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_TxDiffEdge_Max_Threshold_Tolerance.ToString(), Txt_MinTolerance.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_TxDiffEdge_Max_Threshold_Tolerance.ToString(), Txt_MinTolerance.Text);

                    break;

                case Uniformity_SectorName.Uniformity_RxDiff:
                    bResult = SaveAdjustOneFrame(ProfilePath, _SelectStr.ToString(), m_X_Ch - 1, m_Y_Ch);

                    //Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_RxDiff_Max_Threshold.ToString(), Txt_MaxUniformity.Text + "%");
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_RxDiff_Max_Threshold.ToString(), Txt_MaxUniformity.Text);

                    //Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_RxDiff_Max_Threshold_Tolerance.ToString(), Txt_MaxTolerance.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_RxDiff_Max_Threshold_Tolerance.ToString(), Txt_MaxTolerance.Text);

                    //Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_RxDiffEdge_Max_Threshold.ToString(), Txt_MinUniformity.Text + "%");
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_RxDiffEdge_Max_Threshold.ToString(), Txt_MinUniformity.Text);

                    //Read_Profile.IniWriteValue(strSectorName, INI_Name_UniformityTest.Uniformity_RxDiffEdge_Max_Threshold_Tolerance.ToString(), Txt_MinTolerance.Text);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, INI_Name_UniformityTest.Uniformity_RxDiffEdge_Max_Threshold_Tolerance.ToString(), Txt_MinTolerance.Text);
                    
                    break;
            }
            
            //=====================================
            if(bResult)
            {
                foreach(var item_2 in EnumTool.EnumToList<Uniformity_SectorName>())
                    Read_Profile.IniWriteValue(strSectorName, string.Format("{0}_En", item_2), bool.FalseString);

                //foreach(var item in ckListBox_AllNode.CheckedItems)
                //    if(Enum.IsDefined(typeof(Uniformity_SectorName), item))
                //        Read_Profile.IniWriteValue(strSectorName, string.Format("{0}_En", item), bool.TrueString);
                foreach(var item in m_TmpChecked)
                {
                    Read_Profile.IniWriteValue(strSectorName, string.Format("{0}_En", item.Key), bool.TrueString);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, string.Format("{0}_En", item.Key), item.Value);                    
                }
            }
            grpBox_AllNodeEns.Enabled = true;
            return bResult;
        }

        private void UpdateMicroOpen_Chart_LA(string SelectStr)
        {
            lbNowSelect.Text = SelectStr;
            MicroOpen_TestItems _SelectStr;
            if(!MicroOpen_TestItems.TryParse(SelectStr, out _SelectStr))
                return;
            int Frame_Width = m_X_Ch;
            int Frame_Height = m_Y_Ch;
            bool bIsChecked = m_TmpMicroOpenChecked[SelectStr.ToEnum<MicroOpen_TestItems>()];
            switch(_SelectStr)
            {
                case MicroOpen_TestItems.RX_Delta:
                    uiTabControl_McrOpenDataOpt.SelectedTab = bIsChecked ? tabPage_McrOpenLA : null;
                    uiTabControl_McrOpenDataOpt.Visible = bIsChecked ? true : false;
                    panel_Common.Visible = false;
                    splitContainer_Main.Panel2Collapsed = true;
                    break;

                case MicroOpen_TestItems.TX_Avg_Delta:
                    uiTabControl_McrOpenDataOpt.SelectedTab = tabPage_McrOpenOri;
                    uiTabControl_McrOpenDataOpt.Visible = true;
                    panel_Common.Visible = true;
                    splitContainer_Main.Panel2Collapsed = !bIsChecked;
                    
                    Frame_Height -= 1;
                    Frame_Width = 1;
                    string strSectorName = TestItemNameDefine.enMicroOpenTest.DescArr(':')[1];
                    TX_Avg_AvoidCorner.Checked = Read_Profile.IniReadBool(strSectorName, INI_Name_MicroOpenTest.TX_Avg_Delta_Threshold_AvoidCorner.ToString(), false);
                    Txt_RX_Delta_Tolerance.Visible = false;
                    lbl_RX_Delta_Tolerance.Visible = false;
                    groupBox_TX_Avg_AvoidCorner.Visible = true;
                    grpBox_DefaultValue.Enabled = bIsChecked;
                    break;
            }

            Txt_MicroOpen_threshold.TextChanged -= Txt_TextChanged;
            Txt_RX_Delta_Tolerance.TextChanged -= Txt_TextChanged;

            uiIntegerUpDown_X.Minimum = 1;
            uiIntegerUpDown_Y.Minimum = 1;
            uiIntegerUpDown_X.Maximum = Frame_Width;
            uiIntegerUpDown_Y.Maximum = Frame_Height;

            if(LoadOneFrameData(_SelectStr.ToString()))
                ShowOnGrid(Frame_Width, Frame_Height, OneFrame);
            if(_SelectStr == MicroOpen_TestItems.TX_Avg_Delta)
                Txt_MicroOpen_threshold.Text = OneFrame.Node_UpLimit[0].ToString();

            //===================
            //綁定事件
            Txt_MicroOpen_threshold.TextChanged += Txt_TextChanged;
            Txt_RX_Delta_Tolerance.TextChanged += Txt_TextChanged;

            //===================
            //Reset to default
            grpBox_MicroOpen_Def_Spec.Enabled = true;
        }

        private void UpdateMicroOpen_Chart_Ori(string SelectStr)
        {
            lbNowSelect.Text = SelectStr;
            MicroOpen_TestItems _SelectStr;
            if(!MicroOpen_TestItems.TryParse(SelectStr, out _SelectStr))
                return;

            //MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enMicroOpenTest];
            bool bIsChecked = m_TmpMicroOpenChecked[SelectStr.ToEnum<MicroOpen_TestItems>()];
            splitContainer_Main.Panel2Collapsed = !bIsChecked;
            grpBox_DefaultValue.Enabled = bIsChecked;
            uiTabControl_McrOpenDataOpt.Visible = true;
            string strSectorName = TestItemNameDefine.enMicroOpenTest.DescArr(':')[1];
            int Frame_Width = m_X_Ch;
            int Frame_Height = m_Y_Ch;

            //===================
            //解除事件
            Txt_MicroOpen_threshold.TextChanged -= Txt_TextChanged;
            Txt_RX_Delta_Tolerance.TextChanged -= Txt_TextChanged;

            switch(_SelectStr)
            {
                case MicroOpen_TestItems.RX_Delta:
                    Frame_Width--;
                    //Txt_MicroOpen_threshold.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_MicroOpenTest.RX_Delta_Threshold.ToString(), "0");
                    Txt_RX_Delta_Tolerance.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_MicroOpenTest.RX_Delta_Threshold_Tolerance.ToString(), "0");

                    Txt_RX_Delta_Tolerance.Visible = true;
                    lbl_RX_Delta_Tolerance.Visible = true;
                    groupBox_TX_Avg_AvoidCorner.Visible = false;
                    break;

                case MicroOpen_TestItems.TX_Avg_Delta:
                    Frame_Height--;
                    Frame_Width = 1;
                    Txt_MicroOpen_threshold.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_MicroOpenTest.TX_Avg_Delta_Threshold.ToString(), "0");
                    TX_Avg_AvoidCorner.Checked = Read_Profile.IniReadBool(strSectorName, INI_Name_MicroOpenTest.TX_Avg_Delta_Threshold_AvoidCorner.ToString(), false);

                    Txt_RX_Delta_Tolerance.Visible = false;
                    lbl_RX_Delta_Tolerance.Visible = false;
                    groupBox_TX_Avg_AvoidCorner.Visible = true;
                    break;
            }

            uiIntegerUpDown_X.Minimum = 1;
            uiIntegerUpDown_Y.Minimum = 1;
            uiIntegerUpDown_X.Maximum = Frame_Width;
            uiIntegerUpDown_Y.Maximum = Frame_Height;
            if(LoadOneFrameData(_SelectStr.ToString()))
                ShowOnGrid(Frame_Width, Frame_Height, OneFrame);
            //===================
            //綁定事件
            Txt_MicroOpen_threshold.TextChanged += Txt_TextChanged;
            Txt_RX_Delta_Tolerance.TextChanged += Txt_TextChanged;

            //===================
            //Reset to default
            grpBox_MicroOpen_Def_Spec.Enabled = true;
        }
       
        private void UpdateAllNode_Chart_Ori(string SelectStr)
        {
            lbNowSelect.Text = SelectStr;
            Uniformity_SectorName _SelectStr;
            if(!Uniformity_SectorName.TryParse(SelectStr, out _SelectStr))
                return;
            //MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enUniformityTest];
            bool bIsChecked = m_TmpChecked[SelectStr.ToEnum<Uniformity_SectorName>()];
            if(_SelectStr == Uniformity_SectorName.Uniformity_RawData_MinMax)            
                splitContainer_Main.Panel2Collapsed = true;
            else            
                splitContainer_Main.Panel2Collapsed = !bIsChecked;
            uiSplitContainer1.SplitterDistance = (int)((double)uiSplitContainer1.Width * 0.5);

            uiTabControl_UniDataOpt.SelectedTab = tabPage_UniDataOri;
            panel_Common.Visible = true;
            uiTabControl_UniDataOpt.Visible = true;

            grpBox_DefaultValue.Enabled = bIsChecked;
            grpBox_Allnode_Def_Spec.Enabled = bIsChecked;
            string strSectorName = TestItemNameDefine.enUniformityTest.DescArr(':')[1];

            int Frame_Width = (_SelectStr == Uniformity_SectorName.Uniformity_KeyRawData) ? m_Key_Ch : m_X_Ch;
            int Frame_Height = (_SelectStr == Uniformity_SectorName.Uniformity_KeyRawData) ? 1 : m_Y_Ch;

            //===================
            //解除事件
            Txt_MaxUniformity.TextChanged -= Txt_TextChanged;
            Txt_MaxTolerance.TextChanged -= Txt_TextChanged;
            Txt_MinUniformity.TextChanged -= Txt_TextChanged;
            Txt_MinTolerance.TextChanged -= Txt_TextChanged;
            Txt_MaxThresholdPercent.TextChanged -= Txt_TextChanged;
            Txt_MaxEdgeThresholdPercent.TextChanged -= Txt_TextChanged;
            Txt_MinThresholdPercent.TextChanged -= Txt_TextChanged;
            Txt_MinEdgeThresholdPercent.TextChanged -= Txt_TextChanged;

            //20220701
            //Txt_MinMax_Min.TextChanged -= Txt_TextChanged;
            bool bLoadOneFrameData = LoadOneFrameData(_SelectStr.ToString());
            groupBox_Max.Text = "Max";
            groupBox_Max.Visible = true;

            groupBox_Min.Text = "Min";
            switch(_SelectStr)
            {
                case Uniformity_SectorName.Uniformity_RawData_MinMax:
                    panel_MaxPercent.Visible = false;
                    panel_MaxThreshold.Visible = true;
                    panel_MaxTolerance.Visible = false;

                    lb_MaxThreshold.Text = "Threshold";
                    Txt_MaxUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_MinMax_Min_Threshold.ToString(), "0");

                    groupBox_Min.Visible = false;

                    groupBox_Max.Text = "MinMax";
                    lb_MaxThreshold.Text = "Threshold(%)";
                    Txt_MaxUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_MinMax_Min_Threshold.ToString(), "0");
                    break;

                case Uniformity_SectorName.Uniformity_RawData:
                    lb_MaxThreshold.Text = "Threshold";
                    //Txt_MaxUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_Max_Threshold.ToString(), "0");
                    Txt_MaxUniformity.Text = "5000";

                    lb_MaxTolerance.Text = "Tolerance";
                    panel_MaxTolerance.Visible = false;
                    Txt_MaxTolerance.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_Max_Threshold_Tolerance.ToString(), "1");

                    groupBox_Min.Visible = true;
                    uiLabel_MinThreshold.Text = "Threshold";
                    //Txt_MinUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_Min_Threshold.ToString(), "0");
                    Txt_MinUniformity.Text = "0";

                    uiLabel_MinTolerance.Text = "Tolerance";
                    panel_MinTolerance.Visible = false;
                    Txt_MinTolerance.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_Min_Threshold_Tolerance.ToString(), "1");

                    var RawListData = Read_Profile.IniReadAllContains("Uniformity_RawData").Select(x => x.Split(";").Where(str_2 => !string.IsNullOrEmpty(str_2)).ToArray()).ToArray();
                    var num_MaxEdgeRaw = double.Parse(RawListData[0][0].Split(",")[1]);
                    var num_MaxRaw = double.Parse(RawListData[1][1].Split(",")[1]);
                    var num_MinEdgeRaw = double.Parse(RawListData[0][0].Split(",")[2]);
                    var num_MinRaw = double.Parse(RawListData[1][1].Split(",")[2]);

                    panel_MaxPercent.Visible = false;
                    panel_MaxThreshold.Visible = !panel_MaxPercent.Visible;

                    panel_MinPercent.Visible = panel_MaxPercent.Visible;
                    panel_MinThreshold.Visible = !panel_MaxPercent.Visible;
                    panel_MaxPercent.Visible = false;
                    panel_MinPercent.Visible = false;
                    panel_MaxThreshold.Visible = true;
                    panel_MinThreshold.Visible = true;
                    break;

                case Uniformity_SectorName.Uniformity_Win1:
                    Frame_Height--;
                    lb_MaxThreshold.Text = "Threshold";
                    panel_MaxThreshold.Visible = true;
                    Txt_MaxUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_Win1_Max_Threshold.ToString(), "0");

                    lb_MaxTolerance.Text = "Tolerance";
                    panel_MaxTolerance.Visible = false;
                    Txt_MaxTolerance.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_Win1_Max_Threshold_Tolerance.ToString(), "1");

                    groupBox_Min.Visible = false;
                    uiLabel_MinThreshold.Text = "";
                    panel_MinThreshold.Visible = false;
                    Txt_MinUniformity.Text = "";

                    uiLabel_MinTolerance.Text = "";
                    panel_MinTolerance.Visible = false;
                    Txt_MinTolerance.Text = "";

                    panel_MaxPercent.Visible = false;
                    panel_MinPercent.Visible = false;
                    break;

                case Uniformity_SectorName.Uniformity_Win2:
                    Frame_Width--;
                    Frame_Height--;
                    lb_MaxThreshold.Text = "Threshold";
                    panel_MaxThreshold.Visible = true;
                    //Txt_MaxUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_Win2_Max_Threshold.ToString(), "0");
                    Txt_MaxUniformity.Text = "5000";

                    lb_MaxTolerance.Text = "Tolerance";
                    panel_MaxTolerance.Visible = true;
                    Txt_MaxTolerance.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_Win2_Max_Threshold_Tolerance.ToString(), "1");

                    groupBox_Min.Visible = false;
                    panel_MinThreshold.Visible = false;
                    Txt_MinUniformity.Text = "0";
                    panel_MinTolerance.Visible = false;
                    Txt_MinTolerance.Text = "0";

                    panel_MaxPercent.Visible = false;
                    panel_MinPercent.Visible = false;
                    break;

                case Uniformity_SectorName.Uniformity_KeyRawData:
                    lb_MaxThreshold.Text = "Threshold";
                    panel_MaxThreshold.Visible = true;
                    Txt_MaxUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_KeyRawData_Max_Threshold.ToString(), "0");

                    lb_MaxTolerance.Text = "Tolerance";
                    panel_MaxTolerance.Visible = true;
                    Txt_MaxTolerance.Text = "0";

                    groupBox_Min.Visible = true;
                    uiLabel_MinThreshold.Text = "Threshold";
                    panel_MinThreshold.Visible = true;
                    Txt_MinUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_KeyRawData_Min_Threshold.ToString(), "0");

                    uiLabel_MinTolerance.Text = "Tolerance";
                    panel_MinTolerance.Visible = false;
                    Txt_MinTolerance.Text = "0";

                    panel_MaxPercent.Visible = false;
                    panel_MinPercent.Visible = false;
                    break;

                case Uniformity_SectorName.Uniformity_TxDiff:
                    Frame_Height--;
                    lb_MaxThreshold.Text = "Threshold (%)";
                    panel_MaxThreshold.Visible = true;
                    //Txt_MaxUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_TxDiff_Max_Threshold.ToString(), "0").Replace("%", "");
                    Txt_MaxUniformity.Text = "5000";

                    lb_MaxTolerance.Text = "Tolerance";
                    panel_MaxTolerance.Visible = true;
                    //Txt_MaxTolerance.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_TxDiff_Max_Threshold_Tolerance.ToString(), "0");
                    Txt_MaxTolerance.Text = "0";

                    groupBox_Min.Visible = true;
                    groupBox_Min.Text = "Edge Max";

                    uiLabel_MinThreshold.Text = "Threshold (%)";
                    panel_MinThreshold.Visible = true;
                    //Txt_MinUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_TxDiffEdge_Max_Threshold.ToString(), "0").Replace("%", "");
                    Txt_MinUniformity.Text = "0";

                    uiLabel_MinTolerance.Text = "Tolerance";
                    panel_MinTolerance.Visible = true;
                    //Txt_MinTolerance.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_TxDiffEdge_Max_Threshold_Tolerance.ToString(), "0");
                    Txt_MinTolerance.Text = "0";

                    panel_MaxPercent.Visible = false;
                    panel_MinPercent.Visible = false;
                    break;

                case Uniformity_SectorName.Uniformity_RxDiff:
                    Frame_Width--;
                    lb_MaxThreshold.Text = "Threshold (%)";
                    panel_MaxThreshold.Visible = true;
                    //Txt_MaxUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_RxDiff_Max_Threshold.ToString(), "0").Replace("%", "");
                    Txt_MaxUniformity.Text = "5000";

                    lb_MaxTolerance.Text = "Tolerance";
                    panel_MaxTolerance.Visible = true;
                    //Txt_MaxTolerance.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_RxDiff_Max_Threshold_Tolerance.ToString(), "0");
                    Txt_MaxTolerance.Text = "0";

                    groupBox_Min.Visible = true;
                    groupBox_Min.Text = "Edge Max";

                    uiLabel_MinThreshold.Text = "Threshold (%)";
                    panel_MinThreshold.Visible = true;
                    //Txt_MinUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_RxDiffEdge_Max_Threshold.ToString(), "0").Replace("%", "");
                    Txt_MinUniformity.Text = "100";

                    uiLabel_MinTolerance.Text = "Tolerance";
                    panel_MinTolerance.Visible = true;
                    //Txt_MinTolerance.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_RxDiffEdge_Max_Threshold_Tolerance.ToString(), "0");
                    Txt_MinTolerance.Text = "0";

                    panel_MaxPercent.Visible = false;
                    panel_MinPercent.Visible = false;
                    break;
            }
            if(bLoadOneFrameData)
                ShowOnGrid(Frame_Width, Frame_Height, OneFrame);
            //===================
            //綁定事件
            Txt_MinTolerance.TextChanged += Txt_TextChanged;
            Txt_MinUniformity.TextChanged += Txt_TextChanged;
            Txt_MaxTolerance.TextChanged += Txt_TextChanged;
            Txt_MaxUniformity.TextChanged += Txt_TextChanged;
            //Txt_MinMax_Min.TextChanged += Txt_TextChanged;
            Txt_MaxThresholdPercent.TextChanged += Txt_TextChanged;
            Txt_MaxEdgeThresholdPercent.TextChanged += Txt_TextChanged;
            Txt_MinThresholdPercent.TextChanged += Txt_TextChanged;
            Txt_MinEdgeThresholdPercent.TextChanged += Txt_TextChanged;
            //===================
            //Reset to default
            //grpBox_Allnode_Def_Spec.Enabled = true;
            uiIntegerUpDown_X.Minimum = 1;
            uiIntegerUpDown_Y.Minimum = 1;
            uiIntegerUpDown_X.Maximum = Frame_Width;
            uiIntegerUpDown_Y.Maximum = Frame_Height;
        }
        //true: mount Log Analysis tool 畫面成功
        private bool UpdateAllNode_Chart_LA(string SelectStr)
        {
            lbNowSelect.Text = SelectStr;
            Uniformity_SectorName _SelectStr;
            if(!Uniformity_SectorName.TryParse(SelectStr, out _SelectStr))
                return false;

            bool bIsChecked = m_TmpChecked[SelectStr.ToEnum<Uniformity_SectorName>()];
            if((_SelectStr == Uniformity_SectorName.Uniformity_RawData_MinMax)
                || (_SelectStr == Uniformity_SectorName.Uniformity_KeyRawData))
            {
                uiTabControl_UniDataOpt.SelectedTab = tabPage_UniDataOri;
                panel_Common.Visible = true;
                uiTabControl_UniDataOpt.Visible = true;
            }
            else
            {
                if(uiTabControl_UniDataOpt.SelectedTab != tabPage_UniDataLA)
                    uiTabControl_UniDataOpt.SelectedTab = tabPage_UniDataLA;
                tabPage_UniDataLA.Parent = bIsChecked ? uiTabControl_UniDataOpt : null;
                panel_Common.Visible = false;
                uiTabControl_UniDataOpt.Visible = bIsChecked;
                return true;
            }
            uiSplitContainer1.SplitterDistance = (int)((double)uiSplitContainer1.Width * 0.5);

            grpBox_DefaultValue.Enabled = bIsChecked;
            grpBox_Allnode_Def_Spec.Enabled = bIsChecked;
            string strSectorName = TestItemNameDefine.enUniformityTest.DescArr(':')[1];

            int Frame_Width = (_SelectStr == Uniformity_SectorName.Uniformity_KeyRawData) ? m_Key_Ch : m_X_Ch;
            int Frame_Height = (_SelectStr == Uniformity_SectorName.Uniformity_KeyRawData) ? 1 : m_Y_Ch;

            //===================
            //解除事件
            Txt_MaxUniformity.TextChanged -= Txt_TextChanged;
            Txt_MaxTolerance.TextChanged -= Txt_TextChanged;
            Txt_MinUniformity.TextChanged -= Txt_TextChanged;
            Txt_MinTolerance.TextChanged -= Txt_TextChanged;
            Txt_MaxThresholdPercent.TextChanged -= Txt_TextChanged;
            Txt_MaxEdgeThresholdPercent.TextChanged -= Txt_TextChanged;
            Txt_MinThresholdPercent.TextChanged -= Txt_TextChanged;
            Txt_MinEdgeThresholdPercent.TextChanged -= Txt_TextChanged;

            //20220701
            //Txt_MinMax_Min.TextChanged -= Txt_TextChanged;
            bool bLoadOneFrameData = LoadOneFrameData(_SelectStr.ToString());
            groupBox_Max.Text = "Max";
            groupBox_Max.Visible = true;

            groupBox_Min.Text = "Min";
            switch(_SelectStr)
            {
                case Uniformity_SectorName.Uniformity_RawData_MinMax:
                    panel_MaxPercent.Visible = false;
                    panel_MaxThreshold.Visible = true;
                    panel_MaxTolerance.Visible = false;

                    lb_MaxThreshold.Text = "Threshold";
                    Txt_MaxUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_MinMax_Min_Threshold.ToString(), "0");

                    groupBox_Min.Visible = false;

                    groupBox_Max.Text = "MinMax";
                    lb_MaxThreshold.Text = "Threshold(%)";
                    Txt_MaxUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_MinMax_Min_Threshold.ToString(), "0");
                    break;

                case Uniformity_SectorName.Uniformity_KeyRawData:
                    lb_MaxThreshold.Text = "Threshold";
                    panel_MaxThreshold.Visible = true;
                    Txt_MaxUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_KeyRawData_Max_Threshold.ToString(), "0");

                    lb_MaxTolerance.Text = "Tolerance";
                    panel_MaxTolerance.Visible = true;
                    Txt_MaxTolerance.Text = "0";

                    groupBox_Min.Visible = true;
                    uiLabel_MinThreshold.Text = "Threshold";
                    panel_MinThreshold.Visible = true;
                    Txt_MinUniformity.Text = Read_Profile.IniReadValue(strSectorName, INI_Name_UniformityTest.Uniformity_KeyRawData_Min_Threshold.ToString(), "0");

                    uiLabel_MinTolerance.Text = "Tolerance";
                    panel_MinTolerance.Visible = false;
                    Txt_MinTolerance.Text = "0";

                    panel_MaxPercent.Visible = false;
                    panel_MinPercent.Visible = false;
                    break;
            }
            if(bLoadOneFrameData)
                ShowOnGrid(Frame_Width, Frame_Height, OneFrame);
            //===================
            //綁定事件
            Txt_MinTolerance.TextChanged += Txt_TextChanged;
            Txt_MinUniformity.TextChanged += Txt_TextChanged;
            Txt_MaxTolerance.TextChanged += Txt_TextChanged;
            Txt_MaxUniformity.TextChanged += Txt_TextChanged;
            //Txt_MinMax_Min.TextChanged += Txt_TextChanged;
            Txt_MaxThresholdPercent.TextChanged += Txt_TextChanged;
            Txt_MaxEdgeThresholdPercent.TextChanged += Txt_TextChanged;
            Txt_MinThresholdPercent.TextChanged += Txt_TextChanged;
            Txt_MinEdgeThresholdPercent.TextChanged += Txt_TextChanged;
            //===================
            //Reset to default
            //grpBox_Allnode_Def_Spec.Enabled = true;
            uiIntegerUpDown_X.Minimum = 1;
            uiIntegerUpDown_Y.Minimum = 1;
            uiIntegerUpDown_X.Maximum = Frame_Width;
            uiIntegerUpDown_Y.Maximum = Frame_Height;
            return false;
        }

        private int LoadLimitValue(string strProfilePath, string strName, string strKeyName)
        {
            return Read_Profile.IniReadInt(strName, strKeyName, 0);
        }

        private void btn_SetToAllNode_MouseLeave(object sender, EventArgs e)
        {
            richTextBox_Hint.Text = "";
        }

        private void ckListBox_AllNode_MouseLeave(object sender, EventArgs e)
        {
            richTextBox_Hint.Text = "";
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void toolStripButton_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton_Maximize_Click(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else if(this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
        }

        private void ckListBox_AllNode_MouseEnter(object sender, EventArgs e)
        {
            if(splitContainer_Main.Enabled)
                this.Cursor = System.Windows.Forms.Cursors.Hand;
            else
                this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void tChart_OneData_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                tChart_OneData.ClickSeries -= tChart_OneData_ClickSeries;
                lb_SelectionMode.Text = "Zoom";
            }
            else
            {
                tChart_OneData.ClickSeries += tChart_OneData_ClickSeries;
                lb_SelectionMode.Text = "Select";
            }
        }
        
        private void Txt_TextChanged(object sender, EventArgs e)
        {
            var MySender = sender as UITextBox;
            timer_BlinkRestoreButton.Enabled = true;
            if(tabControl_Function.SelectedTab == tabPage_Uniformity)
            {
                if(!Enum.IsDefined(typeof(Uniformity_SectorName), ckListBox_AllNode.SelectedItem.ToString()))
                    return;
                var SelectedItem = (Uniformity_SectorName)Enum.Parse(typeof(Uniformity_SectorName), ckListBox_AllNode.SelectedItem.ToString());
                var NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enUniformityTest];

                if(MySender == Txt_MaxTolerance)
                {
                    string _Key = "";
                    if(SelectedItem == Uniformity_SectorName.Uniformity_TxDiff)
                        _Key = "Uniformity_TxDiff_Max_Threshold_Tolerance";
                    else if(SelectedItem == Uniformity_SectorName.Uniformity_RxDiff)
                        _Key = "Uniformity_RxDiff_Max_Threshold_Tolerance";
                    else if(SelectedItem == Uniformity_SectorName.Uniformity_Win2)
                        _Key = "Uniformity_Win2_Max_Threshold_Tolerance";                                        
                    var _Select = NowTestitem.SPEC.Find(x => x.Name == _Key);
                    _Select.dtValue.Rows[0][0] = MySender.IntValue;
                }
                else if(MySender == Txt_MinTolerance)
                {
                    string _Key = "";
                    if(SelectedItem == Uniformity_SectorName.Uniformity_TxDiff)
                        _Key = "Uniformity_TxDiffEdge_Max_Threshold_Tolerance";
                    else
                        _Key = "Uniformity_RxDiffEdge_Max_Threshold_Tolerance";

                    var _Select = NowTestitem.SPEC.Find(x => x.Name == _Key);
                    _Select.dtValue.Rows[0][0] = MySender.IntValue;
                }
                else if(MySender == Txt_MaxUniformity)
                {                    
                    string _Key = "";
                    if(SelectedItem == Uniformity_SectorName.Uniformity_RawData)
                        _Key = "Uniformity_RawData_Max_Threshold";
                    else if(SelectedItem == Uniformity_SectorName.Uniformity_KeyRawData)
                        _Key = "Uniformity_KeyRawData_Max_Threshold";
                    else if(SelectedItem == Uniformity_SectorName.Uniformity_Win1)
                        _Key = "Uniformity_Win1_Max_Threshold";
                    else if(SelectedItem == Uniformity_SectorName.Uniformity_Win2)
                        _Key = "Uniformity_Win2_Max_Threshold";
                    else if(SelectedItem == Uniformity_SectorName.Uniformity_TxDiff)
                        _Key = "Uniformity_TxDiff_Max_Threshold";
                    else if(SelectedItem == Uniformity_SectorName.Uniformity_RxDiff)
                        _Key = "Uniformity_RxDiff_Max_Threshold";
                    else if(SelectedItem == Uniformity_SectorName.Uniformity_RawData_MinMax)
                        _Key = "Uniformity_RawData_MinMax_Min_Threshold";
                    var _Select = NowTestitem.SPEC.Find(x => x.Name == _Key);
                    _Select.dtValue.Rows[0][0] = MySender.IntValue;

                }
                else if(MySender == Txt_MinUniformity)
                {
                    string _Key = "";
                    if(SelectedItem == Uniformity_SectorName.Uniformity_RawData)
                        _Key = "Uniformity_RawData_Min_Threshold";
                    else if(SelectedItem == Uniformity_SectorName.Uniformity_KeyRawData)
                        _Key = "Uniformity_KeyRawData_Min_Threshold";
                    else if(SelectedItem == Uniformity_SectorName.Uniformity_TxDiff)
                        _Key = "Uniformity_TxDiffEdge_Max_Threshold";
                    else if(SelectedItem == Uniformity_SectorName.Uniformity_RxDiff)
                        _Key = "Uniformity_RxDiffEdge_Max_Threshold";

                    var _Select = NowTestitem.SPEC.Find(x => x.Name == _Key);
                    _Select.dtValue.Rows[0][0] = MySender.IntValue;
                }
            }
            else if(tabControl_Function.SelectedTab == tabPage_MicroOpen)
            {
                if(!Enum.IsDefined(typeof(MicroOpen_TestItems), ckListBox_MicroOpen.SelectedItem.ToString()))
                    return;
                var SelectedItem = (MicroOpen_TestItems)Enum.Parse(typeof(MicroOpen_TestItems), ckListBox_MicroOpen.SelectedItem.ToString());
                var NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enMicroOpenTest];

                if(MySender == Txt_MicroOpen_threshold)
                {
                    string _Key = "";
                    if(SelectedItem == MicroOpen_TestItems.TX_Avg_Delta)
                    {
                        //_Key = "TX_Avg_Delta_Threshold";
                        //var _Select = NowTestitem.SPEC.Find(x => x.Name == _Key);
                        //_Select.dtValue.Rows[0][0] = MySender.IntValue;
                    }
                    else if(SelectedItem == MicroOpen_TestItems.RX_Delta)
                    {
                        _Key = "RX_Delta_Threshold";
                        var _Select = NowTestitem.SPEC.Find(x => x.Name == _Key);
                        _Select.dtValue.Rows[0][0] = MySender.IntValue;
                    }
                }
                else if(MySender == Txt_RX_Delta_Tolerance)
                {
                    string _Key = "";
                    if(SelectedItem == MicroOpen_TestItems.RX_Delta)
                    {
                        _Key = "RX_Delta_Threshold_Tolerance";
                        var _Select = NowTestitem.SPEC.Find(x => x.Name == _Key);
                        _Select.dtValue.Rows[0][0] = MySender.IntValue;
                    }
                }
            }
        }

        private void timer_BlinkRestoreButton_Tick(object sender, EventArgs e)
        {
            if(btn_RestoreDefault.BackColor == Color.SeaShell)
                btn_RestoreDefault.BackColor = Color.Yellow;
            else if(btn_RestoreDefault.BackColor == Color.Yellow)
                btn_RestoreDefault.BackColor = Color.SeaShell;
            else
                btn_RestoreDefault.BackColor = Color.Yellow;
        }

        private void ckListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var MySender = sender as CheckedListBox;
            if(MySender.SelectedItem == null)
                return;
          
            //grpBox_AllNodeEns.Enabled = false;
           
            string SelectStr = MySender.SelectedItem.ToString();
            MultiSenTest_VAR.TestStatus NowTestitem = new MultiSenTest_VAR.TestStatus();
            if(Enum.IsDefined(typeof(Uniformity_SectorName), SelectStr))
            {
                if(SelectStr == Uniformity_SectorName.Uniformity_RawData.ToString())
                {

                    if(e.NewValue == CheckState.Unchecked)
                    { 
                        string msg="";
                        if ((StaticVar.g_Lang == enLangTable.LANG_TRADITIONAL_CHINESE)
                            || (StaticVar.g_Lang == enLangTable.LANG_SIMPLIFIED_CHINESE))
                            msg = string.Format("{0} 不能被 {1}!", SelectStr, e.NewValue);
                        else
                            msg = string.Format("{0} can't be {1}!", SelectStr, e.NewValue);
                        //MyUIPage.ShowErrorTip(msg, 1000, false);
                        MyUIPage.ShowErrorDialog(msg, true);
                        e.NewValue = e.CurrentValue;
                        grpBox_AllNodeEns.Enabled = true;
                        return;
                    }
                }

                NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enUniformityTest];
            }
            else if(Enum.IsDefined(typeof(MicroOpen_TestItems), SelectStr))
            {
                NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enMicroOpenTest];
            }
            else
            {
                richTextBox_Hint.Text = "Selected Item fail!";
                MyUIPage.ShowErrorDialog("Selected Item fail!", true);
                grpBox_AllNodeEns.Enabled = true;
                return;
            }

            //string str = string.Format("{0}_En", SelectStr);            
            //UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, str, (e.NewValue == CheckState.Checked));
            //richTextBox_Hint.Text = string.Format("Set {0} Test {1}!", str, e.NewValue);
            if (tabControl_Function.SelectedTab == tabPage_Uniformity)
                m_TmpChecked[SelectStr.ToEnum<Uniformity_SectorName>()] = e.NewValue == CheckState.Checked;
            else
                m_TmpMicroOpenChecked[SelectStr.ToEnum<MicroOpen_TestItems>()] = e.NewValue == CheckState.Checked;
           
            richTextBox_Hint.Text = string.Format("Set {0} Test {1}!", SelectStr, e.NewValue);
            grpBox_AllNodeEns.Enabled = true;
            //grpBox_Allnode_Def_Spec.Enabled = (e.NewValue == CheckState.Checked);
        }

        private void ckListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            CheckedListBox MySender = sender as CheckedListBox;
            if(MySender.SelectedItem == null)
                return;
            //bool bIsChecked = MySender.GetItemChecked(MySender.SelectedIndex);
            
            this.Enabled = false;
            
            Version strINIProfile = new Version(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("System", "ProfileVersion"));
            bool bUseNewChart = (strINIProfile > new Version(1, 0, 3, 6));
            string SelectStr = MySender.SelectedItem.ToString();
            
            string _str = "";
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_En;
            else
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_Cn;            
            MyUIPage.ShowWaitForm(_str);

#if BLOCKLAYOUT_DISABLE
                if (Enum.IsDefined(typeof(Uniformity_SectorName), SelectStr))
                {
                     UpdateAllNode_Chart_Ori(SelectStr);
                }
                else if (Enum.IsDefined(typeof(MicroOpen_TestItems), SelectStr))
                {
                    UpdateMicroOpen_Chart_Ori(SelectStr);
                }
#else
            bool LA_ret = true;
            if(Enum.IsDefined(typeof(Uniformity_SectorName), SelectStr))
            {
                if(bUseNewChart)
                    LA_ret = UpdateAllNode_Chart_LA(SelectStr);
                else
                    UpdateAllNode_Chart_Ori(SelectStr);
            }
            else if(Enum.IsDefined(typeof(MicroOpen_TestItems), SelectStr))
            {
                if(bUseNewChart)
                    UpdateMicroOpen_Chart_LA(SelectStr);
                else
                    UpdateMicroOpen_Chart_Ori(SelectStr);
            }
#endif
            splitContainer_Main.Panel2.Enabled = true;

            MyUIPage.HideWaitForm();

            this.Enabled = true;
            m_lbItemName_2.Focus();
            if(!LA_ret)
                return;

        }

        private void tChart_OneData_MouseEnter(object sender, EventArgs e)
        {
            tChart_OneData.ClickSeries += tChart_OneData_ClickSeries;
            lb_SelectionMode.Text = "";
        }

        private void tChart_OneData_MouseLeave(object sender, EventArgs e)
        {
            tChart_OneData.ClickSeries -= tChart_OneData_ClickSeries;
            uiToolTip1.Active = false;
            lb_SelectionMode.Text = "";
        }

        private Point NowClickPoint;
        
        private void tChart_OneData_ClickSeries(object sender, Series _series, int valueIndex, MouseEventArgs e)
        {
            if(e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            lb_SelectionMode.Text = "Select";
            tChart_OneData.ClickSeries -= tChart_OneData_ClickSeries;
            int iWidth = (int)_series.ValuesLists[0].Maximum;
            int iHeight = (int)_series.ValuesLists[2].Maximum;

            int X_index = (valueIndex + 1) % iWidth;
            int Y_index = ((valueIndex + 1) / iWidth);

            int iX = (X_index == 0) ? iWidth : X_index;
            int iY = (X_index == 0) ? Y_index : Y_index + 1;
            label_Position.Text = string.Format("X_Ch : {0} Y_Ch : {1}", iX, iY);
            NowClickPoint = new Point(iX, iY);
            if(OneFrame.Node_UpLimit == null)
                return;
            if(OneFrame.Node_LowLimit == null)
                return;
            var Node_UpLimit = OneFrame.Node_UpLimit[valueIndex];
            var Node_LowLimit = OneFrame.Node_LowLimit[valueIndex];
            var _Value = Node_LowLimit == -1 ?
                string.Format("Node_UpLimit : {0}", Node_UpLimit) :
                string.Format("Node_UpLimit : {0}\nNode_LowLimit : {1}", Node_UpLimit, Node_LowLimit);
            uiToolTip1.Active = true;
            uiToolTip1.SetToolTip(tChart_OneData, _Value, string.Format("X:{0} Y:{1}", iX, iY));
            NowGridSelectedNode = valueIndex;
        }

        private void tChart_OneData_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if(OneFrame.Node_UpLimit == null)
                    return;
                if(OneFrame.Node_LowLimit == null)
                    return;
                Size _Size = new Size(m_X_Ch, m_Y_Ch);
                ShowFrame(new fmRawCellSetting(ProfilePath, _Size, NowClickPoint, NowGridSelectedNode, ref OneFrame));
            }
            else if(e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                lb_SelectionMode.Text = "Mouse Double Click not support MouseButtons-Right.";
            }
        }

        private void ShowFrame(Form frame)
        {
            frame.WindowState = FormWindowState.Normal;
            var _Result = frame.ShowDialog(this);

            string SelectStr = "";
            if(_Result == DialogResult.OK)
            {
                bool bResult = false;
                if(tabControl_Function.SelectedTab == tabPage_Uniformity)
                {
                    bResult = SaveUniformity_RawData();

                    Uniformity_SectorName _SelectStr;
                    if(Uniformity_SectorName.TryParse(ckListBox_AllNode.SelectedItem.ToString(), out _SelectStr))
                    {
                        if(_SelectStr == Uniformity_SectorName.Uniformity_RawData)
                            ShowOnGrid(m_X_Ch, m_Y_Ch, OneFrame);
                        else if(_SelectStr == Uniformity_SectorName.Uniformity_Win1)
                            ShowOnGrid(m_X_Ch, m_Y_Ch - 1, OneFrame);
                        else if(_SelectStr == Uniformity_SectorName.Uniformity_Win2)
                            ShowOnGrid(m_X_Ch - 1, m_Y_Ch - 1, OneFrame);
                        else if(_SelectStr == Uniformity_SectorName.Uniformity_KeyRawData)
                            ShowOnGrid(m_Key_Ch, 1, OneFrame);
                        else if(_SelectStr == Uniformity_SectorName.Uniformity_TxDiff)
                            ShowOnGrid(m_X_Ch, m_Y_Ch - 1, OneFrame);
                        else if(_SelectStr == Uniformity_SectorName.Uniformity_RxDiff)
                            ShowOnGrid(m_X_Ch - 1, m_Y_Ch, OneFrame);
                    }
                }
                else if(tabControl_Function.SelectedTab == tabPage_MicroOpen)
                {
                    bResult = SaveMicroOpen_RawData();
                    SelectStr = ckListBox_MicroOpen.SelectedItem.ToString();
                    MicroOpen_TestItems _SelectStr;
                    if(MicroOpen_TestItems.TryParse(ckListBox_MicroOpen.SelectedItem.ToString(), out _SelectStr))
                    {
                        if(_SelectStr == MicroOpen_TestItems.RX_Delta)
                            ShowOnGrid(m_X_Ch - 1, m_Y_Ch, OneFrame);
                        else if(_SelectStr == MicroOpen_TestItems.TX_Avg_Delta)
                            ShowOnGrid(1, m_Y_Ch - 1, OneFrame);
                    }
                }
                if(bResult)
                    MyUIPage.ShowSuccessDialog("", "Save Success !", UIStyle.Green, true);
                else
                    MyUIPage.ShowErrorDialog("", "Profile not correct !", UIStyle.Red, true);
            }
            frame.Dispose();
        }

        private void btn_ConformCellValue_Click(object sender, EventArgs e)
        {
            int _X_Ch = m_X_Ch;
            int _Y_Ch = m_Y_Ch;
            if(tabControl_Function.SelectedTab == tabPage_Uniformity)
            {
                Uniformity_SectorName _SelectStr;
                if(Uniformity_SectorName.TryParse(ckListBox_AllNode.SelectedItem.ToString(), out _SelectStr))
                {
                    switch(_SelectStr)
                    {
                        case Uniformity_SectorName.Uniformity_RxDiff:
                            _X_Ch = m_X_Ch - 1;
                            break;

                        case Uniformity_SectorName.Uniformity_TxDiff:
                        case Uniformity_SectorName.Uniformity_Win1:
                            _Y_Ch = m_Y_Ch - 1;
                            break;

                        case Uniformity_SectorName.Uniformity_Win2:
                            _X_Ch = m_X_Ch - 1;
                            _Y_Ch = m_Y_Ch - 1;
                            break;
                    }
                }
            }
            else if(tabControl_Function.SelectedTab == tabPage_MicroOpen)
            {
                MicroOpen_TestItems _SelectStr;
                if(MicroOpen_TestItems.TryParse(ckListBox_MicroOpen.SelectedItem.ToString(), out _SelectStr))
                {
                    if(_SelectStr == MicroOpen_TestItems.RX_Delta)
                    {
                        _X_Ch = m_X_Ch - 1;
                    }
                    else if(_SelectStr == MicroOpen_TestItems.TX_Avg_Delta)
                    {
                        _X_Ch = 1;
                        _Y_Ch = m_Y_Ch - 1;
                    }
                }
            }

            Size _Size = new Size(_X_Ch, _Y_Ch);
            int iNum = (uiIntegerUpDown_Y.Value - 1) * _X_Ch + uiIntegerUpDown_X.Value - 1;
            //NowGridSelectedNode
            NowClickPoint = new Point(uiIntegerUpDown_X.Value, uiIntegerUpDown_Y.Value);
            ShowFrame(new fmRawCellSetting(ProfilePath, _Size, NowClickPoint, iNum, ref OneFrame));
        }

        private void TX_Avg_AvoidCorner_CheckedChanged(object sender, EventArgs e)
        {
            var MySender = sender as CheckBox;
            var NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enUniformityTest];
            string _Key = "";
            _Key = "TX_Avg_Delta_Threshold_AvoidCorner";
            var _Select = NowTestitem.SPEC.Find(x => x.Name == _Key);
            string sStatus = (MySender.Checked) ? "True" : "False";

            _Select.dtValue.Rows[0][0] = sStatus;
        }
        //20220628 Steven Add for new test item
        private void Def_MinMax_En_CheckedChanged(object sender, EventArgs e)
        {
            grpBox_AllNodeEns.Enabled = false;
            var MySender = sender as CheckBox;

            MultiSenTest_VAR.TestStatus NowTestitem = new MultiSenTest_VAR.TestStatus();
            NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[TestItemNameDefine.enUniformityTest];
            string str = string.Format("Uniformity_RawData_MinMax_En");
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetSpecData(ref NowTestitem.SPEC, str, MySender.Checked);

            grpBox_AllNodeEns.Enabled = true;
        }

        private void ComboBox_CDC_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Mysender = sender as UIComboBox;
            bool bV3_BK_Enable = ComboBox_CDC_Type.SelectedItem.ToString() == V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_ALL_NODE.ToString();
            Read_Profile.IniWriteValue("Uniformity_Test", "V3_BK_Enable", bV3_BK_Enable);
            //Read_Profile.IniWriteValue("Uniformity_Test", "CDC_Type", Mysender.SelectedItem.ToString());
        }
        
        public void Dispose_LATool_Panel()
        {
            panel_UniDataLA.Controls.Clear();
            foreach (Control c in panel_UniDataLA.Controls)
                c.Dispose();
            //
            panel_McrOpenLA.Controls.Clear();
            foreach (Control c in panel_McrOpenLA.Controls)
                c.Dispose();
        }

        private void tabControl_Function_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabPage_MicroOpen.Parent != null)
                splitContainer_Main.Panel2Collapsed = true;
        }
    }

    public class OneFrame_class : ICloneable
    {
        public object FrameType { get; set; }
        public List<uint> Node { get; set; }
        public List<int> Node_UpLimit { get; set; }
        public List<int> Node_LowLimit { get; set; }
        public List<int> Node_Func { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        // method for cloning object
        public OneFrame_class DeepCopy()
        {
            OneFrame_class deepcopyCompany = new OneFrame_class();
            deepcopyCompany.FrameType = this.FrameType;
            deepcopyCompany.Node = new List<uint>(this.Node);
            deepcopyCompany.Node_UpLimit = new List<int>(this.Node_UpLimit);
            deepcopyCompany.Node_LowLimit = new List<int>(this.Node_LowLimit);
            deepcopyCompany.Node_Func = new List<int>(this.Node_Func);
            return deepcopyCompany;
        }
    }
}