using CommonExt;
using Sunny.UI;
using System;
using System.Drawing;
using System.Windows.Forms;
using ThirdPartyTools;

namespace ITS_Studio.AllForms
{
    public partial class fmRawCellSetting : Form
    {
        private OneFrame_class m_OriOneFrame;
        private OneFrame_class m_ModifyOneFrame;
        private INI_Tool Read_Profile;
        private int m_X_Ch = 0;
        private int m_Y_Ch = 0;
        private int m_NowGridSelectedNode = 0;

        protected DynamicTool MyTool = new DynamicTool();
        private Point m_NowClickPoint;
        public fmRawCellSetting(string INI_Path, Size _size, Point NowClickPoint, int NowGridSelectedNode, ref OneFrame_class InOneFrame)
        {
            InitializeComponent();

            Read_Profile = new INI_Tool(INI_Path);
            m_X_Ch = _size.Width;
            m_Y_Ch = _size.Height;
            m_NowClickPoint = NowClickPoint;
            m_NowGridSelectedNode = NowGridSelectedNode;
            
            m_ModifyOneFrame = InOneFrame;
            
            m_OriOneFrame = new OneFrame_class();
            m_OriOneFrame = InOneFrame.DeepCopy();
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

        private void ChangeSpecValue(int iValue, SensorTest_RawDataSetting.RawName_Define DataType, ref OneFrame_class _Frame)
        {
            int iDefault = 0;
            if (Enum.IsDefined(typeof(Uniformity_SectorName), _Frame.FrameType.ToString()))
            {
                string strSectorName = TestItemNameDefine.enUniformityTest.DescArr(':')[1];
                var item = (Uniformity_SectorName)_Frame.FrameType;
                switch (item)
                {
                    case Uniformity_SectorName.Uniformity_RawData:
                        if (DataType == SensorTest_RawDataSetting.RawName_Define.MaxValue)
                        {
                            //iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_Max_Threshold.ToString(), 0);
                            iDefault = 5000;
                            _Frame.Node_UpLimit[m_NowGridSelectedNode] = iValue;
                        }
                        else if (DataType == SensorTest_RawDataSetting.RawName_Define.MinValue)
                        {
                            //iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_UniformityTest.Uniformity_RawData_Min_Threshold.ToString(), 0);
                            iDefault = 0;
                            _Frame.Node_LowLimit[m_NowGridSelectedNode] = iValue;
                        }
                        break;

                    case Uniformity_SectorName.Uniformity_Win1:
                        if (DataType == SensorTest_RawDataSetting.RawName_Define.MaxValue)
                        {
                            //iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_UniformityTest.Uniformity_Win1_Max_Threshold.ToString(), 0);
                            iDefault = 5000;
                            _Frame.Node_UpLimit[m_NowGridSelectedNode] = iValue;
                        }
                        break;

                    case Uniformity_SectorName.Uniformity_Win2:
                        if (DataType == SensorTest_RawDataSetting.RawName_Define.MaxValue)
                        {
                            //iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_UniformityTest.Uniformity_Win2_Max_Threshold.ToString(), 0);
                            iDefault = 5000;
                            _Frame.Node_UpLimit[m_NowGridSelectedNode] = iValue;
                        }
                        break;

                    case Uniformity_SectorName.Uniformity_KeyRawData:
                        //m_Key_Ch
                        if (DataType == SensorTest_RawDataSetting.RawName_Define.MaxValue)
                        {
                            iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_UniformityTest.Uniformity_KeyRawData_Max_Threshold.ToString(), 0);
                            _Frame.Node_UpLimit[m_NowGridSelectedNode] = iValue;
                        }
                        else if (DataType == SensorTest_RawDataSetting.RawName_Define.MinValue)
                        {
                            iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_UniformityTest.Uniformity_KeyRawData_Min_Threshold.ToString(), 0);
                            _Frame.Node_LowLimit[m_NowGridSelectedNode] = iValue;
                        }
                        break;

                    //Y少一條
                    case Uniformity_SectorName.Uniformity_TxDiff:
                        double TxDiff_tmp = (double)m_NowGridSelectedNode;
                        bool bIsTxDiffEdge = false;

                        if (TxDiff_tmp > (m_X_Ch * 0) && TxDiff_tmp < (m_X_Ch * 1))
                            bIsTxDiffEdge = true;
                        else if (TxDiff_tmp > (m_X_Ch * (m_Y_Ch - 2)) && TxDiff_tmp < (m_X_Ch * (m_Y_Ch - 1)))
                            bIsTxDiffEdge = true;
                        else if ((TxDiff_tmp % m_X_Ch) == 0)
                            bIsTxDiffEdge = true;
                        else if (((TxDiff_tmp + 1) % m_X_Ch) == 0)
                            bIsTxDiffEdge = true;

                        //string strTxDiff = (bIsTxDiffEdge)
                        //    ? INI_Name_UniformityTest.Uniformity_TxDiffEdge_Max_Threshold.ToString()
                        //    : INI_Name_UniformityTest.Uniformity_TxDiff_Max_Threshold.ToString();

                        //iDefault = Read_Profile.IniReadValue(strSectorName, strTxDiff, "0%").Replace("%", "").ToInt();
                        iDefault = bIsTxDiffEdge ? 100 : 5000;
                        _Frame.Node_UpLimit[m_NowGridSelectedNode] = iValue;
                        break;

                    //X少一條
                    case Uniformity_SectorName.Uniformity_RxDiff:
                        double RxDiff_tmp = (double)m_NowGridSelectedNode;
                        bool bIsRxDiffEdge = false;

                        if (RxDiff_tmp > ((m_X_Ch - 1) * 0) && RxDiff_tmp < ((m_X_Ch - 1) * 1))
                            bIsRxDiffEdge = true;
                        else if (RxDiff_tmp > ((m_X_Ch - 1) * (m_Y_Ch - 1)) && RxDiff_tmp < ((m_X_Ch - 1) * m_Y_Ch))
                            bIsRxDiffEdge = true;
                        else if ((RxDiff_tmp % (m_X_Ch - 1)) == 0)
                            bIsRxDiffEdge = true;
                        else if (((RxDiff_tmp + 1) % (m_X_Ch - 1)) == 0)
                            bIsRxDiffEdge = true;

                        //string strRxDiff = (bIsRxDiffEdge)
                        //    ? INI_Name_UniformityTest.Uniformity_RxDiffEdge_Max_Threshold.ToString()
                        //    : INI_Name_UniformityTest.Uniformity_RxDiff_Max_Threshold.ToString();

                        //iDefault = Read_Profile.IniReadValue(strSectorName, strRxDiff, "0%").Replace("%", "").ToInt();
                        iDefault = bIsRxDiffEdge ? 100 : 5000;
                        _Frame.Node_UpLimit[m_NowGridSelectedNode] = iValue;
                        break;
                }
            }
            else if (Enum.IsDefined(typeof(MicroOpen_TestItems), _Frame.FrameType.ToString()))
            {
                string strSectorName = TestItemNameDefine.enMicroOpenTest.DescArr(':')[1];
                var item = (MicroOpen_TestItems)_Frame.FrameType;
                switch (item)
                {
                    case MicroOpen_TestItems.RX_Delta:
                        if (DataType == SensorTest_RawDataSetting.RawName_Define.MaxValue)
                        {
                            //iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_MicroOpenTest.RX_Delta_Threshold.ToString(), 0);
                            iDefault = 200;
                            _Frame.Node_UpLimit[m_NowGridSelectedNode] = iValue;
                        }
                        break;

                    case MicroOpen_TestItems.TX_Avg_Delta:
                        if (DataType == SensorTest_RawDataSetting.RawName_Define.MaxValue)
                        {
                            iDefault = Read_Profile.IniReadInt(strSectorName, INI_Name_MicroOpenTest.TX_Avg_Delta_Threshold.ToString(), 0);
                            _Frame.Node_UpLimit[m_NowGridSelectedNode] = iValue;
                        }
                        break;
                }
            }

            if (DataType == SensorTest_RawDataSetting.RawName_Define.Func)
                _Frame.Node_Func[m_NowGridSelectedNode] = iValue;

            uint iTmp = _Frame.Node[m_NowGridSelectedNode];
            bool bTmp = (iValue != iDefault) ? true : false;
            _Frame.Node[m_NowGridSelectedNode] = MyTool.SetIntegerSomeBit(iTmp, (int)DataType, bTmp);
        }

        private void textBox_NodeTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(((UITextBox)sender).Text))
                return;
            int iValue = 0;
            int.TryParse(((UITextBox)sender).Text, out iValue);
            if (((UITextBox)sender) == textBox_NodeUpLimit1)
                ChangeSpecValue(iValue, SensorTest_RawDataSetting.RawName_Define.MaxValue, ref m_ModifyOneFrame);
            else if (((UITextBox)sender) == textBox_NodeLowLimit1)
                ChangeSpecValue(iValue, SensorTest_RawDataSetting.RawName_Define.MinValue, ref m_ModifyOneFrame);
        }

        private void cbNodeTest_En_Click(object sender, EventArgs e)
        {
            bool bChecked = ((UICheckBox)sender).Checked;
            foreach (var Bit in (enDefineFunction[])Enum.GetValues(typeof(enDefineFunction)))
            {
                if (Bit == enDefineFunction.NodeTest_En)
                {
                    int itemValue = m_ModifyOneFrame.Node_Func[m_NowGridSelectedNode];
                    itemValue = MyTool.SetIntegerSomeBit(itemValue, (int)Bit, bChecked);
                    ChangeSpecValue(itemValue, SensorTest_RawDataSetting.RawName_Define.Func, ref m_ModifyOneFrame);
                }
            }
        }

        private void textBox_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(((UITextBox)sender).Text))
                ((UITextBox)sender).Text = "0";
            textBox_NodeTextChanged(sender, e);            
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            //還原
            m_ModifyOneFrame.Node[m_NowGridSelectedNode] = m_OriOneFrame.Node[m_NowGridSelectedNode];
            m_ModifyOneFrame.Node_Func[m_NowGridSelectedNode] = m_OriOneFrame.Node_Func[m_NowGridSelectedNode];
            m_ModifyOneFrame.Node_LowLimit[m_NowGridSelectedNode] = m_OriOneFrame.Node_LowLimit[m_NowGridSelectedNode];
            m_ModifyOneFrame.Node_UpLimit[m_NowGridSelectedNode] = m_OriOneFrame.Node_UpLimit[m_NowGridSelectedNode];
            DialogResult = DialogResult.Cancel;
        }

        private void fmRawCellSetting_Load(object sender, EventArgs e)
        {
            textBox_NodeUpLimit1.TextChanged -= textBox_NodeTextChanged;
            textBox_NodeLowLimit1.TextChanged -= textBox_NodeTextChanged;
            uiCheckBox1.Click -= cbNodeTest_En_Click;
            uiTitlePanel1.Text = string.Format("X_Ch:{0} Y_Ch:{1}", m_NowClickPoint.X, m_NowClickPoint.Y);
            textBox_NodeUpLimit1.Text = m_ModifyOneFrame.Node_UpLimit[m_NowGridSelectedNode].ToString();
            textBox_NodeLowLimit1.Text = m_ModifyOneFrame.Node_LowLimit[m_NowGridSelectedNode].ToString();
            foreach (var item in (enDefineFunction[])Enum.GetValues(typeof(enDefineFunction)))
            {
                int itemValue = (int)EnumTool.GetEnumDescription_Bit(item, new int());
                if (item == enDefineFunction.NodeTest_En)
                    uiCheckBox1.Checked = (m_ModifyOneFrame.Node_Func[m_NowGridSelectedNode] & itemValue) != 0;
            }
            textBox_NodeUpLimit1.TextChanged += textBox_NodeTextChanged;
            textBox_NodeLowLimit1.TextChanged += textBox_NodeTextChanged;
            uiCheckBox1.Click += cbNodeTest_En_Click;

            textBox_NodeUpLimit1.Enabled = (m_ModifyOneFrame.Node_UpLimit[m_NowGridSelectedNode] == -1) ? false : true;
            textBox_NodeUpLimit1.Text = m_ModifyOneFrame.Node_UpLimit[m_NowGridSelectedNode].ToString();

            textBox_NodeLowLimit1.Enabled = (m_ModifyOneFrame.Node_LowLimit[m_NowGridSelectedNode] == -1) ? false : true;
            textBox_NodeLowLimit1.Text = m_ModifyOneFrame.Node_LowLimit[m_NowGridSelectedNode].ToString();
        }
    }
}
