using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CommonExt;
using ThirdPartyTools;

namespace ITS_Studio
{
    public partial class NoiseFrequencyFormulaTransform : Form
    {
        public SetUI_Update MySetUI_Update;
        
        protected NotStaticVar LocalVariable;
        protected int m_iDev = -1;
        public NoiseFrequencyFormulaTransform()
        {
            InitializeComponent();
        }
        public NoiseFrequencyFormulaTransform(int inIndex, ref NotStaticVar _LocalVariable)
        {
            m_iDev = inIndex;
            LocalVariable = _LocalVariable;
            FrequencyScan_Init("");
            
            InitializeComponent();
        }
        protected void FrequencyScan_Init(string strMCU)
        {
            UI_GBV.mDev[m_iDev].m_GBV.Globe_Multi._NoiseFrq_.LoadXML_Params(ITS_Directory.FrequencyScanSetting, false);
        }
        
        private void btnToPostIdle_Click(object sender, EventArgs e)
        {
            MySetUI_Update(m_InBuffer);
        }

        private void btnToFreq_Click(object sender, EventArgs e)
        {
            List<NoiseFrqVariable._NoiseData_> MyTrans = new List<NoiseFrqVariable._NoiseData_>();            
            //匯入資料
            var _charge = (byte)StringTool.StringToByte(uiDataGridView1.Rows[0].Cells[0].Value.ToString());
            var _dump = (byte)StringTool.StringToByte(uiDataGridView1.Rows[0].Cells[1].Value.ToString());
            
            for(int i = 0; i < m_InBuffer.Count; i++)
            {
                MyTrans.Add(new NoiseFrqVariable._NoiseData_
                {
                    iValue = m_InBuffer[i].iValue,
                    iFrequency = Formula(m_InBuffer[i].iFrequency, m_InFrqDataType, _charge, _dump),
                    strUnit = "Hz",
                });
            }
            MyTrans.Sort((a, b) => b.iFrequency.CompareTo(a.iFrequency));//降冪排列
            MySetUI_Update(MyTrans);
            
        }
        private List<NoiseFrqVariable._NoiseData_> m_InBuffer;
        private enFrqDataType m_InFrqDataType;
        public void ImportData(enFrqDataType InFrqDataType, List<NoiseFrqVariable._NoiseData_> InBuffer)
        {
            m_InBuffer = InBuffer;
            m_InFrqDataType = InFrqDataType;

            var ShowMsg = string.Format("MCU Type : {0}\r\n", "ILI2510");
            ShowMsg += string.Format("Data Type : {0}\r\n", InFrqDataType.Desc());
            ShowMsg += string.Format("1 Period Freq : {0} Hz",LocalVariable.NoiseFrqVar.DefaultParam[m_InFrqDataType].Formula);
            uiLabel1.Text = ShowMsg;
        }

        private int Formula(int PostIdle, enFrqDataType mode, byte _charge, byte _dump)
        {
            return -1;
        }

        private void NoiseFrequencyFormulaTransform_Load(object sender, EventArgs e)
        {
            uiDataGridView1.ClearAll();
            //LocalVariable.NoiseFrqVar.DefaultParam[m_InFrqDataType].charge
            //LocalVariable.NoiseFrqVar.DefaultParam[m_InFrqDataType].dump
            List<Data> lsdata = new List<Data>();
            Data data = new Data();
            data.Charge =string.Format("0x{0:X2}", LocalVariable.NoiseFrqVar.DefaultParam[m_InFrqDataType].charge);
            data.Dump = string.Format("0x{0:X2}", LocalVariable.NoiseFrqVar.DefaultParam[m_InFrqDataType].dump);
            lsdata.Add(data);
            uiDataGridView1.DataSource = lsdata;
        }

        
    }
    public class Data
    {
        public string Charge
        {
            get;
            set;
        }

        public string Dump
        {
            get;
            set;
        }
        
        //public override string ToString()
        //{
        //    return Charge;
        //}
    }
}
