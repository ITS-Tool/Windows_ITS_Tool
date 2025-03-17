using CommonExt;
using FuncMethodCmd;
using ITS_Studio.AllForms;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ThirdPartyTools;
using System.Linq;

namespace ITS_Studio
{
    public delegate void SetUI_Update(List<NoiseFrqVariable._NoiseData_> Buff);
    public partial class NoiseFrefm : Form
    {
        private NotStaticVar m_LocalVariable;
        private DynamicTool MyTool = new DynamicTool();
        private UIPage MyUIPage = new UIPage();
        private NoiseFrequencyFormulafm NoiseFrequencyFormulaForm = new NoiseFrequencyFormulafm();
        private NoiseFrequencyFormulaTransform NoiseFrequencyFormulaTransformfm = new NoiseFrequencyFormulaTransform();
        private NoiseFrqVariable.enDataType NowSelectDataType = NoiseFrqVariable.enDataType.None;
        private List<NoiseFrqVariable.PenSingalFormat> ls_PenSingal = new List<NoiseFrqVariable.PenSingalFormat>();
        private bool use_commonflow = true;
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
        private Version m_Protocol_VerNum = new Version(0, 0, 0);
        private bool NoiseFre_ret = false;
        private ParseLog _ParseLog = new ParseLog();

        private FunType _nowFunType = FunType.NULL;
        private FunType nowFunType
        {
            set
            {
                _nowFunType = value;
                SetUI_ReadOnly(_nowFunType == FunType.ReplayLog);
            }
            get
            {
                return _nowFunType;
            }
        }
        private enum en_UI_NoiseDataType
        {
            [Description("Noise Max Value")]
            Maximum = 0,
            [Description("Noise Average Value")]
            Average,
            [Description("Noise Frame Count")]
            FrequencyCnts
        }

        public NoiseFrefm()
        {
            InitializeComponent();

            StaticVar.NoiseFre_FormHandle = this.Handle;
            if(UI_GBV.mDev == null)
                return;

            if(UI_GBV.mDev != null)
                UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType = enCDCDataFormat.inOriginal;

            use_commonflow = (Directory.Exists("NoFreqCommonFlow")) ? false : true;
        }
        private void TreeViewBeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = (nowFunType == FunType.ReplayLog) ? true : false;
            //e.Cancel = ((e.Node.Text == "System") || (e.Node.Text == "Report")) ? true : false;
        }
        private void TreeViewInitialize(bool bSupportCMD_Switch)
        {
            TreeView_ScanType.Nodes.Clear();

            TreeView_ScanType.CheckBoxes = true;
            TreeView_ScanType.ShowLines = false;
            TreeView_ScanType.ShowRootLines = false;
            if(bSupportCMD_Switch)
            {
                foreach(var item in EnumTool.EnumToList<en_UI_NoiseDataType>())
                {
                    var treeNode = new System.Windows.Forms.TreeNode(item.Desc());
                    treeNode.Checked = false;
                    treeNode.Name = item.ToString();
                    treeNode.Text = item.Desc();
                    TreeView_ScanType.Nodes.Add(treeNode);
                }
            }
            else
            {
                var desc = en_UI_NoiseDataType.Maximum;
                var treeNode = new System.Windows.Forms.TreeNode(desc.Desc());
                treeNode.Checked = false;
                treeNode.Name = desc.ToString();
                treeNode.Text = desc.Desc();
                TreeView_ScanType.Nodes.Add(treeNode);
            }
            //TreeView_ScanType.BeforeCheck -= TreeViewBeforeCheck;
            //TreeView_ScanType.BeforeCheck += TreeViewBeforeCheck;
            TreeView_ScanType.AfterCheck -= TreeView_AfterCheck;
            TreeView_ScanType.AfterCheck += TreeView_AfterCheck;
        }

        private void TreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //if(e.Node.Parent == null)
            //    return;
            var _TreeView = sender as TestTreeView;
            int checkedNodeCount = _TreeView.Nodes.Cast<TreeNode>().SelectMany(GetCheckedNodes).Count();
            if(checkedNodeCount < 1)
                e.Node.Checked = true;
        }

        // 遞迴地獲取已勾選的節點
        private IEnumerable<TreeNode> GetCheckedNodes(TreeNode node)
        {
            if(node.Checked)
                yield return node;
            //子層遞迴
            foreach(TreeNode childNode in node.Nodes)
            {
                foreach(TreeNode checkedNode in GetCheckedNodes(childNode))
                {
                    yield return checkedNode;
                }
            }
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var _TreeView = sender as TestTreeView;
            if(_TreeView.SelectedNode == null)
                return;
            var ChkItem = EnumTool.GetValueFromDescription<en_UI_NoiseDataType>(e.Node.Text);
            //Console.WriteLine(e.Node.Text);
            showScanDataMinFre(ChkItem);
            vfShowBarData((enFrqDataType)tabControl_DataType.SelectedIndex);
        }

        public void NoiseFrefmEntry()
        {
            m_Protocol_VerNum = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum;
            NoiseFrefmEntry(m_Protocol_VerNum);
        }

        private string m_strErrorCode = "";

        private void CloseForm(string str)
        {
            FormCollection forms = Application.OpenForms;
            foreach(Form item in forms)
            {
                if(item.Name == str)
                {
                    item.Close();
                    break;
                }
            }
        }
        private void UncheckAllNodes(TreeNodeCollection nodes)
        {
            foreach(TreeNode node in nodes)
            {
                // 將節點的勾選狀態設置為 false
                node.Checked = false;

                // 遞迴取消子節點的勾選狀態
                if(node.Nodes.Count > 0)
                {
                    UncheckAllNodes(node.Nodes);
                }
            }
        }
        private void btn_Exit_Click(object sender, EventArgs e)
        {
            SeriesBar.Clear();
            TreeView_ScanType.BeforeCheck -= TreeViewBeforeCheck;
            UncheckAllNodes(TreeView_ScanType.Nodes);

            //TreeView_ScanType.Nodes.Clear();

            //Reset Value
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.NoiseFrqVar = new NoiseFrqVariable();
            SetScanDataType(enPROTOCOL.none);
            SetNoiseFre_BtnStatus(true);

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.vbExitNoiseFre();

            List<string> CloseForms = new List<string> { NoiseFrequencyFormulaForm.Name, NoiseFrequencyFormulaTransformfm.Name };
            foreach(var item in CloseForms)
                CloseForm(item);

            nowFunType = FunType.NULL;
            UI_GBV.fmITS_Tool.MainButton_Disable();
            StaticVar.NowExecuteTool = enToolName.Main;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_Home);
            UI_GBV.fmITS_Tool.Reset_UI();
        }

        private bool Set_UIData(enPROTOCOL _protocol)
        {
            bool bResult = false;
            switch(_protocol)
            {
                default:
                case enPROTOCOL.none:
                    return false;
                case enPROTOCOL.V3:
                    bResult = Set_UIData_V3();
                    break;
                case enPROTOCOL.V6:
                    bResult = Set_UIData_V6();
                    break;
            }
            if(bResult)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.InitNoiseFreqBuffer(m_Protocol_VerNum);
            return bResult;
        }

        private bool Set_UIData_V6()
        {
            bool ret = true;
            if(m_Protocol_VerNum >= Ver.AP_601)
            {
                if(m_Protocol_VerNum >= Ver.AP_60A)
                {
                    m_LocalVariable.NoiseFrqVar.iFrameNum_MC = tbox_MutualFrameCnt.IntValue;
                    m_LocalVariable.NoiseFrqVar.iFrameNum = tbox_SelfFrameCnt.IntValue;
                    m_LocalVariable.NoiseFrqVar.iFrameNum_Dump = tbox_DumpFrameCnt.IntValue;
                }
                else
                {
                    m_LocalVariable.NoiseFrqVar.iFrameNum = tbox_FrameCnt.IntValue;
                }

                //CheckboxData
                m_LocalVariable.NoiseFrqVar.iDataTypeCnt = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.Judge_ScanDataType(
                    TreeView_ScanType.Nodes[(int)en_UI_NoiseDataType.Maximum].Checked
                    , TreeView_ScanType.Nodes[(int)en_UI_NoiseDataType.Average].Checked
                    , TreeView_ScanType.Nodes[(int)en_UI_NoiseDataType.FrequencyCnts].Checked);

                //lock the check box
                TreeView_ScanType.Enabled = false;
            }
            else
            {
                m_LocalVariable.NoiseFrqVar.iFrameNum = 0;
                m_LocalVariable.NoiseFrqVar.iScanDataRet = 0;
                m_LocalVariable.NoiseFrqVar.iDataTypeCnt = (int)NoiseFrqVariable.enDataType.Maximum;
                m_LocalVariable.NoiseFrqVar.SelectDataType = NoiseFrqVariable.enDataType.Maximum;
            }

            //==========================================================
            //Sine 
            //==========================================================
            NoiseFrqVariable.FrqParams Sine = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.Set_FreqData(
                Pen_CDC_Format.enPenType.Disable
                , tbox_SineStartFreq.Text
                , tbox_SineEndFreq.Text
                , cb_SineStepFre.Text
                , tbox_SineMinSignal.Text);
            if((Sine.iStart < UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Sine_Start)
                || (Sine.iEnd > UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Sine_End))
            {
                MyUIPage.ShowErrorDialog("Warning", "Invalid Sine Frequency Range!", UIStyle.Red, true);
                tbox_SineStartFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Sine_Start.ToString();
                tbox_SineEndFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Sine_End.ToString();
                Sine.iStart = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Sine_Start;
                Sine.iEnd = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Sine_End;
                ret = false;
            }
            //==========================================================
            // SW CAP
            //==========================================================
            NoiseFrqVariable.FrqParams SWCap = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.Set_FreqData(
                Pen_CDC_Format.enPenType.Disable,
                tbox_SWCapStart.Text,
                tbox_SWCapEnd.Text,
                cb_SWCapStep.Text,
                tbox_SWCapMinSignal.Text);
            if((SWCap.iStart < UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Swcap_Start)
                || (SWCap.iEnd > UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Swcap_End))
            {
                MyUIPage.ShowErrorDialog("Warning", "Invalid SWCap Frequency Range!", UIStyle.Red, true);
                tbox_SWCapStart.Text = string.Format("{0}", UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Swcap_Start);
                tbox_SWCapEnd.Text = string.Format("{0}", UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Swcap_End);
                SWCap.iStart = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Swcap_Start;
                SWCap.iEnd = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Swcap_End;
                //SWCap.FrameCnts = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Swcap_FrameCnts;
                ret = false;
            }
            //==========================================================
            // SELF
            //==========================================================
            NoiseFrqVariable.FrqParams Self = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.Set_FreqData(
                Pen_CDC_Format.enPenType.Disable,
                tbox_SelfStartFre.Text,
                tbox_SelfEndFre.Text,
                cb_SelfStepFre.Text,
                tbox_SelfMinSignal.Text);

            if((Self.iStart < UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Self_Start)
                || (Self.iEnd > UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Self_End))
            {
                MyUIPage.ShowErrorDialog("Warning", "Invalid Self Frequency Range!", UIStyle.Red, true);
                tbox_SelfStartFre.Text = string.Format("{0}", UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Self_Start);
                tbox_SelfEndFre.Text = string.Format("{0}", UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Self_End);
                Self.iStart = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Self_Start;
                Self.iEnd = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Self_End;
                //Self.FrameCnts = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Self_FrameCnts;
                ret = false;
            }

            //==========================================================
            // DUMP
            //==========================================================
            string sDump1 = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.GetSelfDumpStringValue((int)enDumpType.Dump1);
            NoiseFrqVariable.FrqParams Dump1 = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.Set_FreqData(
               Pen_CDC_Format.enPenType.Disable,
               tbox_Dump1StartFre.Text,
               tbox_Dump1EndFre.Text,
               cb_Dump1StepFre.Text,
               tbox_Dump1MinSignal.Text,
               sDump1);

            if((Dump1.iStart < UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump1_Start)
                || (Dump1.iEnd > UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump1_End))
            {
                MyUIPage.ShowErrorDialog("Warning", "Invalid Self Frequency Range!", UIStyle.Red, true);
                tbox_Dump1StartFre.Text = string.Format("{0}", UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump1_Start);
                tbox_Dump1EndFre.Text = string.Format("{0}", UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump1_End);
                Dump1.iStart = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump1_Start;
                Dump1.iEnd = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump1_End;
                //Dump1.FrameCnts = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump_FrameCnts;
                ret = false;
            }

            string sDump2 = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.GetSelfDumpStringValue((int)enDumpType.Dump2);
            NoiseFrqVariable.FrqParams Dump2 = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.Set_FreqData(
                Pen_CDC_Format.enPenType.Disable,
                tbox_Dump2StartFre.Text,
                tbox_Dump2EndFre.Text,
                cb_Dump2StepFre.Text,
                tbox_Dump2MinSignal.Text,
                sDump2);

            if((Dump2.iStart < UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump2_Start)
                || (Dump2.iEnd > UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump2_End))
            {
                MyUIPage.ShowErrorDialog("Warning", "Invalid Self Frequency Range!", UIStyle.Red, true);
                tbox_Dump2StartFre.Text = string.Format("{0}", UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump2_Start);
                tbox_Dump2EndFre.Text = string.Format("{0}", UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump2_End);
                Dump2.iStart = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump2_Start;
                Dump2.iEnd = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump2_End;
                //Dump2.FrameCnts = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Dump_FrameCnts;
                ret = false;
            }

            //================================
            // Pen
            //================================
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.isForPenTest = (tabControl_DataType.SelectedTab == tabPage_Pen);

            //UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.isForPenTest = (radioListBox_ChartDisp.SelectedIndex == (int)enFrqDataType._Pen_);

            var _SelectedItem = Pen_CDC_Format.enPenType.Disable;
            if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.isForPenTest)
                _SelectedItem = (Pen_CDC_Format.enPenType)cb_PenProtocol.SelectedItem;
            NoiseFrqVariable.FrqParams Pen = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.Set_FreqData(_SelectedItem, ls_PenSingal);
            if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.isForPenTest)
                if(m_Protocol_VerNum >= Ver.AP_601)
                    m_LocalVariable.NoiseFrqVar.iFrameNum = tbox_FrameCnt.IntValue;
            //================================
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.SetParams(Sine, SWCap, Self, Dump1, Dump2, Pen);

            tbox_SineMinSignal.Text = m_LocalVariable.NoiseFrqVar.Sine._params.iMinSignal.ToString();
            tbox_SWCapMinSignal.Text = m_LocalVariable.NoiseFrqVar.SWCap._params.iMinSignal.ToString();
            tbox_SelfMinSignal.Text = m_LocalVariable.NoiseFrqVar.Self._params.iMinSignal.ToString();
            return ret;
        }

        private bool Set_UIData_V3()
        {
            bool ret = true;

            m_LocalVariable.NoiseFrqVar.SelectDataType = NoiseFrqVariable.enDataType.Maximum;

            NoiseFrqVariable.FrqParams Sine = new NoiseFrqVariable.FrqParams();
            Sine.iStart = Convert.ToInt16(tbox_SineStartFreq.Text);
            Sine.iEnd = Convert.ToInt16(tbox_SineEndFreq.Text);
            Sine.iStep = (int)(Convert.ToDouble(cb_SineStepFre.Text.ToString()) * 10);
            tbox_SelfStartFre.Text = string.Format("{0}", 30);
            tbox_SelfEndFre.Text = string.Format("{0}", 90);

            if(Sine.iStart > Sine.iEnd) //20230523 Steven:經過Roy要求改為可以設定一樣            
                ret = false;

            if(EnumTool.IsEnumDefined<OldV3_MCU>(m_LocalVariable.Var_Multi.DevPanelInfo.MCU))
            {
                if(Sine.iEnd > UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Sine_End)
                {
                    tbox_SineEndFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Sine_End.ToString();
                    m_LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, "23xx IC, set end Frequency to 90KHz....\r\n", Color.Red.ToArgb());
                    ret = false;
                }

                if(Sine.iStart < UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Sine_Start)
                {
                    tbox_SineStartFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Sine_Start.ToString();
                    m_LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, string.Format("23xx IC, set start Frequency to {0}KHz....\r\n", tbox_SineStartFreq.Text), Color.Red.ToArgb());
                    ret = false;
                }
            }
            else
            {
                if(Sine.iEnd > UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Sine_End)
                {
                    tbox_SineEndFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Sine_End.ToString();
                    m_LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, string.Format("25xx IC, set end Frequency to {0}KHz....\r\n", tbox_SineEndFreq.Text), Color.Red.ToArgb());
                    ret = false;
                }
            }

            Sine.iEnd = Convert.ToInt16(tbox_SineEndFreq.Text);
            int iTotalCount = 10 * (Sine.iEnd - Sine.iStart) / Sine.iStep + 1;

            if(iTotalCount > 250)
            {
                m_LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, "Too many samples, decrease Step Frequency\r\n", Color.Red.ToArgb());
                ret = false;
                while(iTotalCount > 250)
                {
                    Sine.iStep++;
                    iTotalCount = 10 * (Sine.iEnd - Sine.iStart) / Sine.iStep + 1;
                };
                double tmp = (double)Sine.iStep / 10.0;
                cb_SineStepFre.Text = tmp.ToString();
            }

            m_LocalVariable.NoiseFrqVar.Sine._params.iStart = Convert.ToInt16(tbox_SineStartFreq.Text);
            m_LocalVariable.NoiseFrqVar.Sine._params.iEnd = Convert.ToInt16(tbox_SineEndFreq.Text);
            m_LocalVariable.NoiseFrqVar.Sine._params.iStep = (int)(Convert.ToDouble(cb_SineStepFre.Text.ToString()) * 10);
            m_LocalVariable.NoiseFrqVar.Sine._params.iMinSignal = Convert.ToInt16(tbox_SineMinSignal.Text);
            return ret;
        }

        private void SetUIToDefaultValue(enPROTOCOL _ProtocolType)
        {
            tbox_SineStartFreq.Text = "";
            tbox_SineEndFreq.Text = "";
            cb_SineStepFre.Text = "";
            tbox_SineMinSignal.Text = "";

            tbox_SWCapStart.Text = "";
            tbox_SWCapEnd.Text = "";
            cb_SWCapStep.Text = "";
            tbox_SWCapMinSignal.Text = "";

            tbox_SelfStartFre.Text = "";
            tbox_SelfEndFre.Text = "";
            cb_SelfStepFre.Text = "";
            tbox_SelfMinSignal.Text = "";

            tbox_Dump1StartFre.Text = "";
            tbox_Dump1EndFre.Text = "";
            cb_Dump1StepFre.Text = "";
            tbox_Dump1MinSignal.Text = "";

            tbox_Dump2StartFre.Text = "";
            tbox_Dump2EndFre.Text = "";
            cb_Dump2StepFre.Text = "";
            tbox_Dump2MinSignal.Text = "";

            if(_ProtocolType == enPROTOCOL.V3)
            {
                tbox_SineStartFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Sine_Start.ToString();
                tbox_SineEndFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Sine_End.ToString();
                cb_SineStepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Sine_FreqStep;
                tbox_SineMinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Sine_MinSignalLevel.ToString();
                groupBox_FrameCount.Visible = false;
            }
            else if(_ProtocolType == enPROTOCOL.V6)
            {
                tbox_SineStartFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Sine_Start.ToString();
                tbox_SineEndFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Sine_End.ToString();
                cb_SineStepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Sine_FreqStep;
                tbox_SineMinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Sine_MinSignalLevel.ToString();

                tbox_SWCapStart.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Swcap_Start.ToString();
                tbox_SWCapEnd.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Swcap_End.ToString();
                cb_SWCapStep.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Swcap_FreqStep;
                tbox_SWCapMinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Swcap_MinSignalLevel.ToString();

                tbox_SelfStartFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Self_Start.ToString();
                tbox_SelfEndFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Self_End.ToString();
                cb_SelfStepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Self_FreqStep;
                tbox_SelfMinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Self_MinSignalLevel.ToString();

                tbox_Dump1StartFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Dump1_Start.ToString();
                tbox_Dump1EndFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Dump1_End.ToString();
                cb_Dump1StepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Dump1_FreqStep;
                tbox_Dump1MinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Dump1_MinSignalLevel.ToString();

                tbox_Dump2StartFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Dump2_Start.ToString();
                tbox_Dump2EndFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Dump2_End.ToString();
                cb_Dump2StepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Dump2_FreqStep;
                tbox_Dump2MinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Dump2_MinSignalLevel.ToString();

                if(m_Protocol_VerNum >= Ver.AP_601)
                {
                    tbox_MutualFrameCnt.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.MC_FrameCount.ToString();
                    tbox_SelfFrameCnt.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.SC_FrameCount.ToString();
                    tbox_DumpFrameCnt.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.Dump_FrameCount.ToString();
                }
                else
                {
                    tbox_FrameCnt.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyDefault.SC_FrameCount.ToString();
                }

                groupBox_FrameCount.Visible = true;
            }
        }

        private void SetScanDataType(enPROTOCOL _ProtocolType)
        {
            if(_ProtocolType == enPROTOCOL.none)
                return;
            foreach(TreeNode item in TreeView_ScanType.Nodes)
                item.Checked = false;
            TreeView_ScanType.Enabled = (_ProtocolType != enPROTOCOL.none);
            if(_ProtocolType != enPROTOCOL.none)
            {
                TreeView_ScanType.Nodes[(int)en_UI_NoiseDataType.Maximum].Checked = true;
                TreeView_ScanType.SelectedNode = TreeView_ScanType.Nodes[(int)en_UI_NoiseDataType.Maximum];
            }
        }

        private void NoiseFrefmEntry(Version _Version)
        {
            m_Protocol_VerNum = _Version;
            m_LocalVariable = UI_GBV.mDev[m_iSelectedDev].LocalVariable;

            chk_noiseFrq.Checked = false;
            //根據V3 / V6 決定要顯示哪幾個tabPage
            ResetTabParent(m_LocalVariable.Var_Multi.ProtocolType);
            tabControl_DataType.SelectedTab = tabPage_Sine;
            if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_ == null)
                return;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.UpdateFrqRangeVar();
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.SetFrqDefaultVar();

            //=====================================
            // Sine
            //=====================================
            if(tabPage_Sine.Parent != null)
            {
                cb_SineStepFre.Items.Clear();
                cb_SineStepFre.Items.AddRange(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.ls_SineFreqStep);
                cb_SineStepFre.SelectedIndex = (m_Protocol_VerNum.Major == 0x03) ? cb_SineStepFre.Items.Count - 1 : 0;
            }
            //=====================================
            // SW_CAP
            //=====================================
            if(tabPage_SWCap.Parent != null)
            {
                cb_SWCapStep.Items.Clear();
                cb_SWCapStep.Items.AddRange(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.ls_SWCapStep);
                if(cb_SWCapStep.Items.Count != 0)
                    cb_SWCapStep.SelectedIndex = 0;
            }
            //=====================================
            // Self
            //=====================================
            if(tabPage_Self.Parent != null)
            {
                cb_SelfStepFre.Items.Clear();
                cb_SelfStepFre.Items.AddRange(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.ls_SelfStepFre);
                if(cb_SelfStepFre.Items.Count != 0)
                    cb_SelfStepFre.SelectedIndex = 0;
            }
            //=====================================
            // Dump1
            //=====================================
            if(tabPage_Dump1.Parent != null)
            {
                cb_Dump1StepFre.Items.Clear();
                cb_Dump1StepFre.Items.AddRange(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.ls_Dump1StepFre);
                if(cb_Dump1StepFre.Items.Count != 0)
                    cb_Dump1StepFre.SelectedIndex = 0;
            }
            //=====================================
            // Dump2
            //=====================================
            if(tabPage_Dump2.Parent != null)
            {
                cb_Dump2StepFre.Items.Clear();
                cb_Dump2StepFre.Items.AddRange(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.ls_Dump2StepFre);
                if(cb_Dump2StepFre.Items.Count != 0)
                    cb_Dump2StepFre.SelectedIndex = 0;
            }
            //=====================================
            //Pen
            //=====================================
            if(m_LocalVariable.Var_Multi.DevPanelInfo.SupportPenType == Pen_CDC_Format.enPenType.Disable) //有筆的格式                    
            {
                tabPage_Pen.Parent = null;
            }
            else
            {
                cb_PenProtocol.Items.Clear();
                if((m_LocalVariable.Var_Multi.DevPanelInfo.SupportPenType & Pen_CDC_Format.enPenType.WGP) == Pen_CDC_Format.enPenType.WGP)
                    cb_PenProtocol.Items.Add(Pen_CDC_Format.enPenType.WGP);

                if((m_LocalVariable.Var_Multi.DevPanelInfo.SupportPenType & Pen_CDC_Format.enPenType.USI) == Pen_CDC_Format.enPenType.USI)
                    cb_PenProtocol.Items.Add(Pen_CDC_Format.enPenType.USI);

                if((m_LocalVariable.Var_Multi.DevPanelInfo.SupportPenType & Pen_CDC_Format.enPenType.MPP) == Pen_CDC_Format.enPenType.MPP)
                    cb_PenProtocol.Items.Add(Pen_CDC_Format.enPenType.MPP);

                if(cb_PenProtocol.Items.Count != 0)
                    cb_PenProtocol.SelectedIndex = 0; //預設選第一個不要空白
            }

            SetUIToDefaultValue(m_LocalVariable.Var_Multi.ProtocolType);
            //=====================================
            // Scan Data Type
            //=====================================
            TreeView_ScanType.Nodes.Clear();
            bool bSupportCMD_Switch = (m_Protocol_VerNum >= Ver.AP_601) ? true : false;
            TreeViewInitialize(bSupportCMD_Switch);

            //=====================================
            // Frame Count
            //=====================================
            if(m_Protocol_VerNum >= Ver.AP_60A)
                uiTabControl_FrameCount.SelectedTab = tabPage_FrameCnt_60A;
            else
                uiTabControl_FrameCount.SelectedTab = tabPage_FrameCnt_609;
            //=====================================
            SeriesBar.Clear();
            SeriesLine.Clear();

            m_tChart_NoiseFre.Axes.Left.Increment = 1;
            m_tChart_NoiseFre.Refresh();
            this.Refresh();
        }

        private void bgwWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if(use_commonflow)
            {
                m_LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.SHOW, (IntPtr)0);
                m_LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, "Scanning....\r\n", Color.Blue.ToArgb());

                GenLog.NoiseFrequence.ResetFreqHandle(m_LocalVariable.NoiseFrqVar.handle);

                int error = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.freq_main(
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.isForPenTest);
                NoiseFre_ret = (error < 0) ? false : true;
                return;
            }

            m_LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.SHOW, (IntPtr)0);
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.NoiseFreTool_Init();
            m_LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, "Scanning....\r\n", Color.Blue.ToArgb());
            m_strErrorCode = "";

            NoiseFre_ret = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.NoiseFreTool_Start(ref m_strErrorCode);
        }

        private void bgwWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(NoiseFre_ret == false)
            {
                MyUIPage.ShowErrorDialog("Frequency Scanning", m_strErrorCode, UIStyle.Red, true);
            }
            else
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.UpdateJudgement();
                vfShowBarData((enFrqDataType)tabControl_DataType.SelectedIndex);
            }

            if(chk_noiseFrq.Checked)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.Gen_NoiseFre_Report(NoiseFre_ret);

            Btn_Stop();

            //20220616 Steven : 為解決HIDOverI2C 做實驗 新增0x38 Lock後解開LockPoint
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
            m_LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.HIDE, (IntPtr)0);

            //chkListBox_ScanType.Enabled = true;
            TreeView_ScanType.Enabled = true;

            //ensable button's action
            SetNoiseFre_BtnStatus(true);

            SelectFirstChecked();
            GC.Collect();
        }
        private void SelectFirstChecked()
        {
            foreach(TreeNode item in TreeView_ScanType.Nodes)
            {
                if(item.Checked)
                {
                    TreeView_ScanType.SelectedNode = item;
                    break;
                }
            }
        }
        private void SetNoiseFre_BtnStatus(bool status)
        {
            tableLayoutPanel_Sine.Enabled = status;
            tableLayoutPanel_SWCap.Enabled = status;
            tableLayoutPanel_Self.Enabled = status;
            tableLayoutPanel_Dump1.Enabled = status;
            tableLayoutPanel_Dump2.Enabled = status;
            groupBox_PenSetting.Enabled = status;

            tbox_SineMinSignal.Enabled = status;
            btn_ResetSineSignalLevel.Enabled = status;

            tbox_SWCapMinSignal.Enabled = status;
            btn_ResetSWCapSignalLevel.Enabled = status;

            tbox_SelfMinSignal.Enabled = status;
            btn_ResetSelfSignalLevel.Enabled = status;

            tbox_Dump1MinSignal.Enabled = status;
            btn_ResetDump1SignalLevel.Enabled = status;

            tbox_Dump2MinSignal.Enabled = status;
            btn_ResetDump2SignalLevel.Enabled = status;

            if(nowFunType == FunType.ReplayLog)
            {
                tbox_SineMinSignal.Enabled = true;
                btn_ResetSineSignalLevel.Enabled = true;

                tbox_SWCapMinSignal.Enabled = true;
                btn_ResetSWCapSignalLevel.Enabled = true;

                tbox_SelfMinSignal.Enabled = true;
                btn_ResetSelfSignalLevel.Enabled = true;

                tbox_Dump1MinSignal.Enabled = true;
                btn_ResetDump1SignalLevel.Enabled = true;

                tbox_Dump2MinSignal.Enabled = true;
                btn_ResetDump2SignalLevel.Enabled = true;
            }

            //gbox_ScanDataTypeCommand.Enabled = status;

            groupBox_FrameCount.Enabled = status;
        }

        private void btn_Scan_Click(object sender, EventArgs e)
        {
            SetNoiseFre_BtnStatus(false);
            SeriesLine.Pointer.Visible = true;
            TreeView_ScanType.BeforeCheck -= TreeViewBeforeCheck;
            switch(btn_Scan.Text)
            {
                case "Scan":
                    nowFunType = FunType.Test;
                    if(!Set_UIData(m_LocalVariable.Var_Multi.ProtocolType))
                        SetNoiseFre_BtnStatus(true);
                    SeriesBar.Clear();

                    MyUIPage.ShowInfoDialog("", "Please Touch The Panel \n and click \"OK\" button when you are ready  !", UIStyle.Orange, true);

                    if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.ProtectShowCountDown > 0)
                    {
                        NumberForm _numfm = new NumberForm(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.ProtectShowCountDown);
                        _numfm.ShowDialog();
                    }

                    m_LocalVariable.TuningVar.bRunningStatus = true;
                    btn_Scan.Text = "Stop";

                    BackgroundWorker bgwWorker = new BackgroundWorker();
                    bgwWorker.DoWork += new DoWorkEventHandler(bgwWorker_DoWork);
                    bgwWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwWorker_RunWorkerCompleted);
                    bgwWorker.WorkerSupportsCancellation = true;
                    bgwWorker.RunWorkerAsync();
                    break;

                case "Replay":
                    nowFunType = FunType.ReplayLog;                   
                    if(ReplayLog())
                        TreeView_ScanType.Enabled = true;
                    TreeView_ScanType.BeforeCheck += TreeViewBeforeCheck;
                    SetNoiseFre_BtnStatus(false);
                    break;

                case "Stop":
                default:
                    m_LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_Message, "User Stop!\r\n", Color.Red.ToArgb());
                    Btn_Stop();
                    MyTool.Delay_ms(100);
                    SetNoiseFre_BtnStatus(true);
                    break;
            }
            TreeView_ScanType.Enabled = true;
        }

        private void showScanDataMinFre(en_UI_NoiseDataType SelectedItem)
        {
            int[] iMinFre, iMinCap, iMinSelf;
            int iSuggestFre, iSuggestCap, iSuggestSelf;
            if(m_LocalVariable == null)
                return;
            switch(SelectedItem)
            {
                case en_UI_NoiseDataType.Maximum:
                    NowSelectDataType = NoiseFrqVariable.enDataType.Maximum;
                    iMinFre = m_LocalVariable.NoiseFrqVar.Sine.iMin_Fre;
                    iSuggestFre = m_LocalVariable.NoiseFrqVar.Sine.iSuggestFre;
                    iMinCap = m_LocalVariable.NoiseFrqVar.SWCap.iMin_Fre;
                    iSuggestCap = m_LocalVariable.NoiseFrqVar.SWCap.iSuggestFre;
                    iMinSelf = m_LocalVariable.NoiseFrqVar.Self.iMin_Fre;
                    iSuggestSelf = m_LocalVariable.NoiseFrqVar.Self.iSuggestFre;
                    break;
                case en_UI_NoiseDataType.Average:
                    NowSelectDataType = NoiseFrqVariable.enDataType.Average;
                    iMinFre = m_LocalVariable.NoiseFrqVar.Sine.iMin_Avg;
                    iSuggestFre = m_LocalVariable.NoiseFrqVar.Sine.iSuggestAvg;
                    iMinCap = m_LocalVariable.NoiseFrqVar.SWCap.iMin_Avg;
                    iSuggestCap = m_LocalVariable.NoiseFrqVar.SWCap.iSuggestAvg;
                    iMinSelf = m_LocalVariable.NoiseFrqVar.Self.iMin_Avg;
                    iSuggestSelf = m_LocalVariable.NoiseFrqVar.Self.iSuggestAvg;
                    break;
                case en_UI_NoiseDataType.FrequencyCnts:
                    NowSelectDataType = NoiseFrqVariable.enDataType.FrequencyCnts;
                    iMinFre = m_LocalVariable.NoiseFrqVar.Sine.iMin_FrCnt;
                    iSuggestFre = m_LocalVariable.NoiseFrqVar.Sine.iSuggestFrCnt;
                    iMinCap = m_LocalVariable.NoiseFrqVar.SWCap.iMin_FrCnt;
                    iSuggestCap = m_LocalVariable.NoiseFrqVar.SWCap.iSuggestFrCnt;
                    iMinSelf = m_LocalVariable.NoiseFrqVar.Self.iMin_FrCnt;
                    iSuggestSelf = m_LocalVariable.NoiseFrqVar.Self.iSuggestFrCnt;
                    break;
            }
        }

        private void AddSeriesLine_Pen(List<string> sLogRawData, List<NoiseFrqVariable._NoiseData_> _Data, int iCount)
        {
            ls_PenSingal.Clear();
            int iIndex = sLogRawData.FindIndex(0, x => x.Contains("Spec"));
            if(iIndex != -1)
            {
                string[] sTempArrary = sLogRawData[iIndex].Replace("Spec: ", "").Split(',');
                for(int i = 0; i < sTempArrary.Length - 1; i++)
                {
                    NoiseFrqVariable.PenSingalFormat penSingalFormat = new NoiseFrqVariable.PenSingalFormat();
                    penSingalFormat.sFreq = sTempArrary[i].Split(':')[0];
                    penSingalFormat.sValue = sTempArrary[i].Split(':')[1];
                    ls_PenSingal.Add(penSingalFormat);
                }

            }
            else //New
            {
                iIndex = sLogRawData.FindIndex(0, x => x.Contains("Pen_Threshold"));
                if(iIndex != -1)
                {
                    string[] sTempArrary = sLogRawData[iIndex].Replace("Pen_Threshold              ,", "").Replace(",", "").Split('/');
                    for(int i = 0; i < sTempArrary.Length - 1; i++)
                    {
                        NoiseFrqVariable.PenSingalFormat penSingalFormat = new NoiseFrqVariable.PenSingalFormat();
                        penSingalFormat.sFreq = sTempArrary[i].Split(':')[0];
                        penSingalFormat.sValue = sTempArrary[i].Split(':')[1];
                        ls_PenSingal.Add(penSingalFormat);
                    }
                }

            }

            foreach(var item in ls_PenSingal)
            {
                if(Convert.ToInt32(item.sFreq) == _Data[iCount].iFrequency)
                {
                    SeriesLine.Add(iCount, Convert.ToInt32(item.sValue));
                    break;
                }
            }
        }

        private void ReplayBarData(int iTotalCount, enFrqDataType SelectType, NoiseFrqVariable.enDataType type, List<NoiseFrqVariable._NoiseData_> _Data, ref int iMax)
        {
            if(_Data.Count == 0)
                return;
            int iCounts = iTotalCount;
            bool[] node_failed = new bool[iCounts];
            if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_._Report == null)
                return;
            List<string> sLogRawData = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_._Report[SelectType];
            if(type == NoiseFrqVariable.enDataType.Maximum) //lsMax
            {
                int iIndex = sLogRawData.FindIndex(0, x => x.Contains("Maximum"));
                if(iIndex != -1)
                {
                    string sFrequence = sLogRawData[iIndex + 1];
                    string sValue = sLogRawData[iIndex + 3];
                    string[] sValueTempArrary = sValue.Replace(" ", "").Split(',');
                    for(int i = 1; i < sValueTempArrary.Length - 1; i++)
                    {
                        if(sValueTempArrary[i].Contains("*"))
                            node_failed[i - 1] = true;
                    }
                }
                else
                {
                    iIndex = sLogRawData.FindIndex(0, x => x.Contains("Data Type    ,1"));
                    if(iIndex != -1)
                    {
                        int iTempCount = sLogRawData.FindIndex(iIndex, x => x.Contains("Frequency"));
                        if(iTempCount == -1)
                            iTempCount = sLogRawData.FindIndex(iIndex, x => x.Contains("PostIdle"));
                        string sFrequence = sLogRawData[iTempCount];
                        iTempCount = sLogRawData.FindIndex(iIndex, x => x.Contains("Value"));
                        string sValue = sLogRawData[iTempCount];
                        string[] sValueTempArrary = sValue.Replace(" ", "").Split(',');
                        for(int i = 1; i < sValueTempArrary.Length - 1; i++)
                        {
                            if(sValueTempArrary[i].Contains("*"))
                                node_failed[i - 1] = true;
                        }

                    }
                }
            }
            else if(type == NoiseFrqVariable.enDataType.Average)
            {
                int iIndex = sLogRawData.FindIndex(0, x => x.Contains("Average"));
                if(iIndex != -1)
                {
                    string sFrequence = sLogRawData[iIndex + 1];
                    string sValue = sLogRawData[iIndex + 3];
                    string[] sValueTempArrary = sValue.Replace(" ", "").Split(',');
                    for(int i = 1; i < sValueTempArrary.Length - 1; i++)
                        if(sValueTempArrary[i].Contains("*"))
                            node_failed[i - 1] = true;
                }
                else
                {
                    iIndex = sLogRawData.FindIndex(0, x => x.Contains("Data Type    ,2"));
                    if(iIndex != -1)
                    {
                        int iTempCount = sLogRawData.FindIndex(iIndex, x => x.Contains("Frequency"));
                        if(iTempCount == -1)
                            iTempCount = sLogRawData.FindIndex(iIndex, x => x.Contains("PostIdle"));
                        string sFrequence = sLogRawData[iTempCount];
                        iTempCount = sLogRawData.FindIndex(iIndex, x => x.Contains("Value"));
                        string sValue = sLogRawData[iTempCount];
                        string[] sValueTempArrary = sValue.Replace(" ", "").Split(',');
                        for(int i = 1; i < sValueTempArrary.Length - 1; i++)
                            if(sValueTempArrary[i].Contains("*"))
                                node_failed[i - 1] = true;
                    }
                }
            }
            else if(type == NoiseFrqVariable.enDataType.FrequencyCnts)
            {
                int iIndex = sLogRawData.FindIndex(0, x => x.Contains("Frame Count"));
                if(iIndex != -1)
                {
                    int iTempCount = sLogRawData.FindIndex(iCounts, x => x.Contains("Frequency"));
                    string sFrequence = sLogRawData[iTempCount];
                    iTempCount = sLogRawData.FindIndex(iCounts, x => x.Contains("Value"));
                    string sValue = sLogRawData[iTempCount];
                    string[] sValueTempArrary = sValue.Replace(" ", "").Split(',');
                    for(int i = 1; i < sValueTempArrary.Length - 1; i++)
                        if(sValueTempArrary[i].Contains("*"))
                            node_failed[i - 1] = true;
                }
                else
                {
                    iIndex = sLogRawData.FindIndex(0, x => x.Contains("Data Type    ,4"));
                    if(iIndex != -1)
                    {
                        int iTempCount = sLogRawData.FindIndex(iIndex, x => x.Contains("Frequency"));
                        if(iTempCount == -1)
                            iTempCount = sLogRawData.FindIndex(iIndex, x => x.Contains("PostIdle"));
                        string sFrequence = sLogRawData[iTempCount];
                        iTempCount = sLogRawData.FindIndex(iIndex, x => x.Contains("Value"));
                        string sValue = sLogRawData[iTempCount];
                        string[] sValueTempArrary = sValue.Replace(" ", "").Split(',');
                        for(int i = 1; i < sValueTempArrary.Length - 1; i++)
                            if(sValueTempArrary[i].Contains("*"))
                                node_failed[i - 1] = true;
                    }
                }
            }

            int iValue;
            string strMessage = "";
            string strUnit = "";
            for(int iCount = 0; iCount < iCounts; iCount++)
            {
                iValue = _Data[iCount].iValue;
                strMessage = string.Format("{0:D}{1}", _Data[iCount].iFrequency, strUnit);
                if(SelectType == enFrqDataType._Pen_)
                    AddSeriesLine_Pen(sLogRawData, _Data, iCount);
                Color tmp = (node_failed[iCount]) ? Color.OrangeRed : Color.Green;
                SeriesBar.Add(iValue, strMessage, tmp);
                if(iMax < iValue)
                    iMax = iValue;
            }
        }

        private void ShowBarData(int iTotalCount, enFrqDataType SelectType, NoiseFrqVariable.enDataType type, List<NoiseFrqVariable._NoiseData_> _Data, ref int iMax)
        {
            if(nowFunType == FunType.ReplayLog && SelectType == enFrqDataType._Pen_)
            {
                ReplayBarData(iTotalCount, SelectType, type, _Data, ref iMax);
                return;
            }

            if(m_LocalVariable.NoiseFrqVar.iDataTypeCnt == 0)
                m_LocalVariable.NoiseFrqVar.iDataTypeCnt = 1;

            int iCounts = iTotalCount / m_LocalVariable.NoiseFrqVar.iDataTypeCnt;

            bool[] node_failed = new bool[iCounts];
            if (nowFunType == FunType.ReplayLog)
            {
                int iMaxValue = 0;
                switch (SelectType)
	            {
		            case enFrqDataType._MC_SINE_:
                        iMaxValue = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_MinSignalLevel;
                        break;
                    case enFrqDataType._MC_SW_Cap_:
                        iMaxValue = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Swcap_MinSignalLevel;
                        break;
                    case enFrqDataType._SC_SW_Cap_:
                        iMaxValue = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Self_MinSignalLevel;
                        break;
                    case enFrqDataType._SC_Dump1_:
                        iMaxValue = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump1_MinSignalLevel;
                        break;
                    case enFrqDataType._SC_Dump2_:
                        iMaxValue = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump2_MinSignalLevel;
                        break;
                    default:
                    break;
	            }
                for (int i = 0; i < node_failed.Length; i++)
                {
                    node_failed[i] = _Data[i].iValue > iMaxValue;
                }
            }
            else
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.get_Freq_Result(SelectType, type, ref node_failed);
            }
                
            int iCount = 0;
            foreach(var _data in _Data)
            {
                if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.isForPenTest)
                {
                    foreach(var item in ls_PenSingal)
                    {
                        if(Convert.ToInt32(item.sFreq) == _data.iFrequency)
                        {
                            SeriesLine.Add(iCount, Convert.ToInt32(item.sValue));
                            break;
                        }
                    }
                }

                Color tmp = (node_failed[iCount]) ? Color.OrangeRed : Color.Green;
                SeriesBar.Add(_data.iValue, string.Format("{0:#,0} {1}", _data.iFrequency, _data.strUnit), tmp);
                iMax = (iMax < _data.iValue) ? _data.iValue : iMax;
                iCount++;
            }
            Console.WriteLine("[ShowBarData] Data Counts = {0}", iCount);
        }

        private void ShowLimitLineDatas(List<NoiseFrqVariable.PenSingalFormat> _Data)
        {
            int iValue;
            string strMessage = "";
            string strUnit = "";
            m_LocalVariable.NoiseFrqVar.iDataTypeCnt = (m_LocalVariable.NoiseFrqVar.iDataTypeCnt == 0) ? 1 : m_LocalVariable.NoiseFrqVar.iDataTypeCnt;
            //for (int iCount = 0; iCount < iTotalCount / GBV.Globe_M[iDev]._TuningTool_.iDataTypeCnt; iCount++)
            int iCounts = _Data.Count;
            int iTotaliCounts = Convert.ToInt32(_Data[iCounts - 1].sFreq) - Convert.ToInt32(_Data[0].sFreq) + 1;

            for(int iCount = Convert.ToInt32(_Data[0].sFreq); iCount <= Convert.ToInt32(_Data[iCounts - 1].sFreq); iCount++)
            {
                foreach(var item in _Data)
                {
                    if(Convert.ToInt32(item.sFreq) == iCount)
                    {
                        strMessage = string.Format("{0:D}{1}", item.sFreq, strUnit);
                        iValue = Convert.ToInt32(item.sValue);
                        SeriesLine.Add(iValue, strMessage);
                        break;
                    }
                }
            }
        }

        private void vfShowBarData(enFrqDataType SelectType)
        {
            int iLeftAxesMax = 0;
            int iBottomAxesMax = 0;
            m_tChart_NoiseFre.Footer.Text = "";
            m_tChart_NoiseFre.Footer.Visible = false;
            SeriesBar.Clear();
            SeriesLine.Clear();
            if(m_LocalVariable == null)
                return;
            NoiseFrqVariable._DataType_ temp_DataType_ = new NoiseFrqVariable._DataType_();
            switch(SelectType)
            {
                case enFrqDataType._MC_SINE_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.Sine;
                    break;

                case enFrqDataType._MC_SW_Cap_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.SWCap;
                    break;

                case enFrqDataType._SC_SW_Cap_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.Self;
                    break;

                case enFrqDataType._SC_Dump1_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.Dump1;
                    break;

                case enFrqDataType._SC_Dump2_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.Dump2;
                    break;

                case enFrqDataType._Pen_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.Pen;
                    break;

                default:
                    return;
            }
            
            if(m_LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                if(m_Protocol_VerNum >= Ver.AP_601 || nowFunType == FunType.ReplayLog)
                {
                    if(TreeView_ScanType.Nodes[(int)en_UI_NoiseDataType.Maximum].IsSelected)
                    {
                        if(temp_DataType_.m_listMax.Count != 0)
                            ShowBarData(temp_DataType_._params.iTotalCount, SelectType, NowSelectDataType, temp_DataType_.m_listMax, ref iLeftAxesMax);
                        iBottomAxesMax = temp_DataType_.m_listMax.Count;
                    }
                    else if(TreeView_ScanType.Nodes[(int)en_UI_NoiseDataType.Average].IsSelected)
                    {
                        if(temp_DataType_.m_listAvg.Count != 0)
                            ShowBarData(temp_DataType_._params.iTotalCount, SelectType, NowSelectDataType, temp_DataType_.m_listAvg, ref iLeftAxesMax);
                        iBottomAxesMax = temp_DataType_.m_listAvg.Count;
                    }
                    else
                    {
                        if(temp_DataType_.m_listFrCnt.Count != 0)
                            ShowBarData(temp_DataType_._params.iTotalCount, SelectType, NowSelectDataType, temp_DataType_.m_listFrCnt, ref iLeftAxesMax);
                        iBottomAxesMax = temp_DataType_.m_listFrCnt.Count;
                    }
                }
                else
                {
                    if(temp_DataType_.m_listMax.Count != 0)
                        ShowBarData(temp_DataType_._params.iTotalCount, SelectType, NowSelectDataType, temp_DataType_.m_listMax, ref iLeftAxesMax);
                    iBottomAxesMax = temp_DataType_.m_listMax.Count;
                }
            }
            else if(m_LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
            {
                NowSelectDataType = NoiseFrqVariable.enDataType.Maximum;
                if(temp_DataType_.m_listMax.Count != 0)
                    ShowBarData(temp_DataType_._params.iTotalCount, SelectType, NowSelectDataType, temp_DataType_.m_listMax, ref iLeftAxesMax);
                iBottomAxesMax = temp_DataType_.m_listMax.Count;
            }

            if(NowSelectDataType != NoiseFrqVariable.enDataType.None)
            {
                m_tChart_NoiseFre.Footer.Text = string.Format("[{0}] {1}", SelectType.Desc(), NowSelectDataType);
                m_tChart_NoiseFre.Footer.Visible = true;
            }            
            m_tChart_NoiseFre.Axes.Left.SetMinMax(0, iLeftAxesMax + 5);
            m_tChart_NoiseFre.Axes.Bottom.SetMinMax(0, iBottomAxesMax);


            m_tChart_NoiseFre.Refresh();
            Console.WriteLine("(Maximum) Left={0} Bottom={1}", m_tChart_NoiseFre.Axes.Left.Maximum, m_tChart_NoiseFre.Axes.Bottom.Maximum);
            //m_tChart_NoiseFre.Axes.Left.Increment = 1;
            //m_tChart_NoiseFre.Axes.Bottom.Increment = 1;
            
        }

        private void Btn_Stop()
        {
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.vbStopNoiseFre();
            btn_Scan.Text = "Scan";
        }

        private void NoiseFrefmV6_Resize(object sender, EventArgs e)
        {
            splitContainer_NoiseFreV6.SplitterDistance = 420;
            //btn_Set.Width = splitContainer_NoiseFreq.Panel2.Width / 3;
            btn_Scan.Width = splitContainer_NoiseFreq.Panel2.Width / 2;
            btn_Exit.Width = splitContainer_NoiseFreq.Panel2.Width / 2;
        }

        private void NoiseFrefmV6_Load(object sender, EventArgs e)
        {
            if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_ != null)
            {
                dataGrid_PenSignalLevel.Rows.Clear();
                dataGrid_PenSignalLevel.Rows.Add(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultStartFreq.ToString(), "10");
                dataGrid_PenSignalLevel.Rows.Add(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultEndFreq.ToString(), "10");
            }
            btn_SineFormula.Click += new EventHandler(ShowFormula);
            btn_SWCapFormula.Click += new EventHandler(ShowFormula);
            btn_SelfFormula.Click += new EventHandler(ShowFormula);
            btn_Dump1Formula.Click += new EventHandler(ShowFormula);
            btn_Dump2Formula.Click += new EventHandler(ShowFormula);
            uiBtn_PenFormula.Click += new EventHandler(ShowFormula);

        }

        private void tabControl_DataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            vfShowBarData((enFrqDataType)tabControl_DataType.SelectedIndex);

            switch((enFrqDataType)tabControl_DataType.SelectedIndex)
            {
                case enFrqDataType._MC_SINE_:
                    tabControl_DataType.SelectedTab = tabPage_Sine;
                    splitContainer3.Panel2Collapsed = true;
                    break;

                case enFrqDataType._MC_SW_Cap_:
                    tabControl_DataType.SelectedTab = tabPage_SWCap;
                    splitContainer3.Panel2Collapsed = false;
                    break;

                case enFrqDataType._SC_SW_Cap_:
                    tabControl_DataType.SelectedTab = tabPage_Self;
                    splitContainer3.Panel2Collapsed = false;
                    break;

                case enFrqDataType._SC_Dump1_:
                    tabControl_DataType.SelectedTab = tabPage_Dump1;
                    splitContainer3.Panel2Collapsed = false;
                    break;

                case enFrqDataType._SC_Dump2_:
                    tabControl_DataType.SelectedTab = tabPage_Dump2;
                    splitContainer3.Panel2Collapsed = false;
                    break;

                case enFrqDataType._Pen_:
                    tabControl_DataType.SelectedTab = tabPage_Pen;
                    splitContainer3.Panel2Collapsed = true;
                    uiTabControl_FrameCount.SelectedTab = tabPage_FrameCnt_609;
                    break;

                default:
                    break;
            }

            if((enFrqDataType)tabControl_DataType.SelectedIndex != enFrqDataType._Pen_)
            {
                if(m_Protocol_VerNum >= Ver.AP_60A)
                    uiTabControl_FrameCount.SelectedTab = tabPage_FrameCnt_60A;
                else
                    uiTabControl_FrameCount.SelectedTab = tabPage_FrameCnt_609;
            }
            //radioListBox_ChartDisp.SelectedIndex = tabControl_DataType.SelectedIndex;
        }

        private void Tb_NoiseFre_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(((TextBox)sender).Text))
                return;

            if(System.Text.RegularExpressions.Regex.IsMatch(((TextBox)sender).Text, "[^0-9]"))
                ((TextBox)sender).Text = tbox_SineStartFreq.Text.Remove(tbox_SineStartFreq.Text.Length - 1);

        }

        private void cb_NoiseReturnType_CheckedChanged(object sender, EventArgs e)
        {
            //CheckBox Var = sender as CheckBox;
            //switch(Var.Name)
            //{
            //    case "cb_Noise_Max":
            //        radio_Noise_Max.Enabled = Var.Checked;
            //        if(!radio_Noise_Max.Enabled && radio_Noise_Avg.Enabled)
            //            radio_Noise_Avg.Select();
            //        else if(!radio_Noise_Max.Enabled && !radio_Noise_Avg.Enabled)
            //            radio_FrCnt.Select();
            //        break;
            //    case "cb_Noise_Avg":
            //        break;
            //    case "cb_FrameCnt":
            //        break;
            //}
        }

        private void radio_NoiseReturnType_CheckedChanged(object sender, EventArgs e)
        {
            //RadioButton chk = sender as RadioButton;
            //Console.WriteLine("Name:{0} Checked:{1}", chk.Name, chk.Checked);
            //if(!chk.Checked)
            //    return;
            //showScanDataMinFre(chk);
            //vfShowBarData((enFrqDataType)tabControl_DataType.SelectedIndex);
        }

        private void btn_SetLimit_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < this.dataGrid_PenSignalLevel.CurrentCell.RowIndex; i++)
            {
                if(dataGrid_PenSignalLevel.Rows[i].Cells[0].Value == null || dataGrid_PenSignalLevel.Rows[i].Cells[1].Value == null)
                {
                    //MessageBox.Show("設定欄位不得有空值!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MyUIPage.ShowErrorNotifier("設定欄位不得有空值!", true);
                    return;
                }
            }

            ls_PenSingal.Clear();
            foreach(DataGridViewRow rowData in dataGrid_PenSignalLevel.Rows)
            {
                if(rowData.Cells[0].Value != null && rowData.Cells[1].Value != null)
                {
                    ls_PenSingal.Add(new NoiseFrqVariable.PenSingalFormat
                    {
                        sFreq = rowData.Cells[0].Value.ToString(),
                        sValue = rowData.Cells[1].Value.ToString()
                    });
                }
            }
            //ShowLimitLineDatas(ls_PenSingal);
        }

        private void dataGrid_PenSignalLevel_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            e.SortResult = (Convert.ToInt32(e.CellValue1) - Convert.ToInt32(e.CellValue2) > 0) ? 1 : (Convert.ToInt32(e.CellValue1) - Convert.ToInt32(e.CellValue2) < 0) ? -1 : 0;
            e.Handled = true;
        }

        private void cb_PenProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            var MySender = sender as UIComboBox;
            switch((Pen_CDC_Format.enPenType)MySender.SelectedItem)
            {
                case Pen_CDC_Format.enPenType.WGP:
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultStartFreq = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Pen_WGP_Start;
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultEndFreq = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Pen_WGP_End;
                    break;
                case Pen_CDC_Format.enPenType.USI:
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultStartFreq = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Pen_USI_Start;
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultEndFreq = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Pen_USI_End;
                    break;
                case Pen_CDC_Format.enPenType.MPP:
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultStartFreq = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Pen_MPP_Start;
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultEndFreq = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyLimit.Pen_MPP_End;
                    break;
                default:
                    break;
            }
            if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultStartFreq != 0)
            {
                dataGrid_PenSignalLevel.Rows.Clear();
                dataGrid_PenSignalLevel.Rows.Add(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultStartFreq.ToString(), "10");
                dataGrid_PenSignalLevel.Rows.Add(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultEndFreq.ToString(), "10");
                ls_PenSingal.Clear();
                foreach(DataGridViewRow rowData in dataGrid_PenSignalLevel.Rows)
                {
                    if(rowData.Cells[0].Value != null && rowData.Cells[1].Value != null)
                    {
                        ls_PenSingal.Add(new NoiseFrqVariable.PenSingalFormat
                        {
                            sFreq = rowData.Cells[0].Value.ToString(),
                            sValue = rowData.Cells[1].Value.ToString()
                        });
                    }
                }
            }

        }

        private void dataGrid_PenSignalLevel_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int count = dataGrid_PenSignalLevel.Rows.Count;
            if(count > 1 && dataGrid_PenSignalLevel.CurrentCell.RowIndex > 0)
            {
                foreach(char ch in dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    if(!char.IsDigit(ch))//是否为数字
                    {
                        //MessageBox.Show("只能輸入數字!");
                        MyUIPage.ShowErrorNotifier("只能輸入數字!", true);
                        return;
                    }
                }

            }

            if(e.ColumnIndex == 0)
            {
                if(count > 1 && dataGrid_PenSignalLevel.CurrentCell.RowIndex > 0)
                {
                    for(int i = 0; i < this.dataGrid_PenSignalLevel.CurrentCell.RowIndex; i++)
                    {
                        if(dataGrid_PenSignalLevel.Rows[i].Cells[0].Value.ToString() == dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex].Cells[0].Value.ToString())
                        {
                            MyUIPage.ShowWarningNotifier("請不要輸入重複的頻率數值!", true);
                            dataGrid_PenSignalLevel.Rows.Remove(dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex]);
                            break;
                        }
                    }

                    if(Convert.ToInt32(dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex].Cells[0].Value.ToString()) < UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultStartFreq
                        || Convert.ToInt32(dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex].Cells[0].Value.ToString()) > UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.iPenDefaultEndFreq)
                    {
                        MyUIPage.ShowWarningNotifier("請不要輸入小於Start或大於End的值!", true);
                        dataGrid_PenSignalLevel.Rows.Remove(dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex]);
                    }
                }
            }
            else if(e.ColumnIndex == 1)
            {

                if(count > 1 && dataGrid_PenSignalLevel.CurrentCell.RowIndex > 0)
                {
                    if(dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex].Cells[0].Value == null)
                    {
                        MyUIPage.ShowWarningNotifier("請先輸入Freq.", true);
                        return;
                    }

                    if(dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex].Cells[0].Value != null && dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex].Cells[1].Value != null)
                        dataGrid_PenSignalLevel.Sort(dataGrid_PenSignalLevel.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
                }
            }
        }

        private void dataGrid_PenSignalLevel_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 1)
            {
                int count = dataGrid_PenSignalLevel.Rows.Count;
                if(count > 1 && dataGrid_PenSignalLevel.CurrentCell.RowIndex > 0)
                {
                    if(dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex].Cells[0].Value != null && dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex].Cells[1].Value == null)
                    {
                        //MessageBox.Show("Value不可以空白", "提示");
                        MyUIPage.ShowWarningNotifier("Value不可以空白", true);
                    }
                }
            }
        }

        private void dataGrid_PenSignalLevel_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if(e.ColumnIndex == 0)
            {
                for(int i = 0; i < this.dataGrid_PenSignalLevel.CurrentCell.RowIndex; i++)
                {
                    if(dataGrid_PenSignalLevel.Rows[i].Cells[0].Value == null || dataGrid_PenSignalLevel.Rows[i].Cells[1].Value == null)
                    {
                        //MessageBox.Show("前面欄位不可以有空值!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MyUIPage.ShowErrorNotifier("前面欄位不可以有空值", true);
                        //dataGrid_PenSignalLevel.Rows.Remove(dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex]);
                        break;
                    }
                }
                dataGrid_PenSignalLevel.CancelEdit();
            }

            else if(e.ColumnIndex == 1)
            {
                int count = dataGrid_PenSignalLevel.Rows.Count;
                if(count > 1 && dataGrid_PenSignalLevel.CurrentCell.RowIndex > 0)
                {
                    if(dataGrid_PenSignalLevel.Rows[dataGrid_PenSignalLevel.CurrentCell.RowIndex].Cells[0].Value == null)
                        //MessageBox.Show("Freq不可以空白", "提示");
                        MyUIPage.ShowWarningNotifier("Freq不可以空白", true);

                }
            }

        }

        private void ResetTabParent(enPROTOCOL _protocol)
        {
            if(_protocol == enPROTOCOL.none)
            {
                tabPage_Sine.Parent = null;
                tabPage_SWCap.Parent = null;
                tabPage_Self.Parent = null;
                tabPage_Dump1.Parent = null;
                tabPage_Dump2.Parent = null;
                tabPage_Pen.Parent = null;
            }
            else if(_protocol == enPROTOCOL.V3)
            {
                tabPage_Sine.Parent = tabControl_DataType;
                tabPage_Sine.Text = "Square";
                tabPage_SWCap.Parent = null;
                tabPage_Self.Parent = null;
                tabPage_Dump1.Parent = null;
                tabPage_Dump2.Parent = null;
                tabPage_Pen.Parent = null;
            }
            else if(_protocol == enPROTOCOL.V6)
            {
                tabPage_Sine.Parent = tabControl_DataType;
                tabPage_Sine.Text = "Sine";
                tabPage_SWCap.Parent = tabControl_DataType;
                tabPage_Self.Parent = tabControl_DataType;
                tabPage_Dump1.Parent = tabControl_DataType;
                tabPage_Dump2.Parent = tabControl_DataType;
                tabPage_Pen.Parent = tabControl_DataType;
            }
        }

        private bool PreloadFile_ResetProtocol(string FileName, ref Version ReportVer)
        {
            List<string> lsTmp = new List<string>();
            if(!_ParseLog.SeekReadFileRange_EndTag_1(FileName, 0, "Start_Testing_Time", "===", ref lsTmp))
                return false;
            if(!ResetProtocol(lsTmp, ref ReportVer))
                return false;
            return true;
        }

        private bool ResetProtocol(List<string> lsTmp, ref Version ReportVer)
        {
            DEV_INTERFACE_TYPE TestInterface = DEV_INTERFACE_TYPE.HID;
            string _MCU = "";
            //Pen_CDC_Format.enPenType _PenType = Pen_CDC_Format.enPenType.Disable;
            Version _Version = new Version();
            enPROTOCOL _PROTOCOL = enPROTOCOL.none;
            ReportVer = new Version(0, 0, 0, 0);
            Pen_CDC_Format.enPenType SupportPenType = Pen_CDC_Format.enPenType.Disable;

            foreach(var item in lsTmp)
            {
                var _str = item.Split(',');
                if(!EnumTool.IsContainsDescription<NoiseFrequency.KeyWord>(_str[0].Trim()))
                    continue;

                switch(EnumTool.GetValueFromDescription<NoiseFrequency.KeyWord>(_str[0].Trim()))
                {
                    case NoiseFrequency.KeyWord.IC_Type:
                        Regex regex = new Regex(@"^ILI(?!.*ILI).*$");
                        if(!regex.IsMatch(_str[1].Trim()))
                            return false;
                        _MCU = _str[1].Trim();
                        break;

                    case NoiseFrequency.KeyWord.Protocol:
                        if(_str[1].Split('.').Length > 3)
                            return false;
                        _Version = StringTool.HexStringToVer(_str[1]);
                        _PROTOCOL = (_Version.Major == 6) ? enPROTOCOL.V6 : enPROTOCOL.V3;
                        break;

                    case NoiseFrequency.KeyWord.Report_Ver:
                        ReportVer = new Version(_str[1].Trim());
                        //isOldVersion = (_Version.Major == 6) ? false : true;
                        break;

                    case NoiseFrequency.KeyWord.Pen_Type:
                        foreach (Pen_CDC_Format.enPenType _pen in EnumTool.EnumToList<Pen_CDC_Format.enPenType>())
                        {
                            int iPenNum = Convert.ToInt32((_str[1].Trim()));
                            if (((int)_pen & iPenNum) == iPenNum)
                                SupportPenType |= _pen;
                        }
                        break;
                    default:
                        break;
                }
            }

            if(!UI_GBV.fmITS_Tool.ResetProtocol(0, string.Format("Protocol_{0}_{1}", _PROTOCOL, TestInterface.Desc())))
            {
                MessageBox.Show("Protocol / Interface錯誤", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            m_LocalVariable.Var_Multi.DevPanelInfo.SupportPenType = SupportPenType; 

            NoiseFrefmEntry(_Version);

            m_LocalVariable.Var_Multi.DevPanelInfo.MCU = _MCU;
            m_LocalVariable.Var_Multi.Protocol_VerNum = _Version;
            m_LocalVariable.Var_Multi.ProtocolType = _PROTOCOL;
           
            return true;
        }

        private bool ReplayLog()
        {
            string FileName = "";
            Version ReportVer = new Version();
            if(!SelectFile("Report (*.csv)|*.csv", ITS_Directory.RecordPath + "Record_NoiseFrq", out FileName))
                return false;

            //回讀首先要先預讀"IC Type" & "Protocol"，然後再做ResetProtocol
            if(!PreloadFile_ResetProtocol(FileName, ref ReportVer))
                return false;
            bool isOldVersion = (ReportVer <= new Version(0, 0, 0, 1));
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.LoadReport(FileName, isOldVersion);

            SetLogValueToUI();

            if(!UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.SetAllDataFromReport(isOldVersion))
            {
                MyUIPage.ShowErrorDialog("File Error", "Decode file format got some confuse!", UIStyle.Red, true);
                return false;
            }

            if(m_LocalVariable.NoiseFrqVar.Pen.m_listMax.Count == 0
                && m_LocalVariable.NoiseFrqVar.Pen.m_listAvg.Count == 0
                && m_LocalVariable.NoiseFrqVar.Pen.m_listFrCnt.Count == 0)
            {
                vfShowBarData(enFrqDataType._MC_SINE_);
                tabControl_DataType.SelectedIndex = (int)enFrqDataType._MC_SINE_;
                SetchkListBox_ScanType();
            }
            else
            {
                vfShowBarData(enFrqDataType._Pen_);
                tabControl_DataType.SelectedIndex = (int)enFrqDataType._Pen_;
                TreeView_ScanType.SelectedNode = TreeView_ScanType.Nodes[0];
            }
            TheValue(m_LocalVariable.Var_Multi.ProtocolType);

            SelectFirstChecked();

            return true;
        }

        private void TheValue(enPROTOCOL _ProtocolType)
        {
            if(_ProtocolType == enPROTOCOL.V3)
            {
                tbox_SineStartFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_Start.ToString();
                tbox_SineEndFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_End.ToString();
                cb_SineStepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_FreqStep;
                tbox_SineMinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_MinSignalLevel.ToString();
            }
            else if(_ProtocolType == enPROTOCOL.V6)
            {
                tbox_SineStartFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_Start.ToString();
                tbox_SineEndFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_End.ToString();
                cb_SineStepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_FreqStep;
                tbox_SineMinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_MinSignalLevel.ToString();

                tbox_SWCapStart.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Swcap_Start.ToString();
                tbox_SWCapEnd.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Swcap_End.ToString();
                cb_SWCapStep.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Swcap_FreqStep;
                tbox_SWCapMinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Swcap_MinSignalLevel.ToString();

                tbox_SelfStartFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Self_Start.ToString();
                tbox_SelfEndFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Self_End.ToString();
                cb_SelfStepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Self_FreqStep;
                tbox_SelfMinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Self_MinSignalLevel.ToString();

                tbox_Dump1StartFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump1_Start.ToString();
                tbox_Dump1EndFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump1_End.ToString();
                cb_Dump1StepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump1_FreqStep;
                tbox_Dump1MinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump1_MinSignalLevel.ToString();

                tbox_Dump2StartFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump2_Start.ToString();
                tbox_Dump2EndFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump2_End.ToString();
                cb_Dump2StepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump2_FreqStep;
                tbox_Dump2MinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump2_MinSignalLevel.ToString();

                if(m_Protocol_VerNum >= Ver.AP_601)
                {
                    tbox_MutualFrameCnt.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.MC_FrameCount.ToString();
                    tbox_SelfFrameCnt.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.SC_FrameCount.ToString();
                    tbox_DumpFrameCnt.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump_FrameCount.ToString();
                }
                else
                {

                    tbox_FrameCnt.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.SC_FrameCount.ToString();
                }
            }
        }
        private void SetchkListBox_ScanType()
        {
            TreeView_ScanType.AfterCheck -= TreeView_AfterCheck;
            foreach(TreeNode item in TreeView_ScanType.Nodes)
            {
                var _name = EnumTool.ParseEnum<NoiseFrqVariable.enDataType>(item.Name);
                item.Checked = (m_LocalVariable.NoiseFrqVar.SelectDataType.HasFlag(_name));
                TreeView_ScanType.SelectedNode = item;
            }
            TreeView_ScanType.AfterCheck += TreeView_AfterCheck;
        }

        private void SetUI_ReadOnly(bool bIsReadOnly)
        {
            tbox_SineStartFreq.ReadOnly = bIsReadOnly;
            tbox_SineEndFreq.ReadOnly = bIsReadOnly;
            cb_SineStepFre.Enabled = !bIsReadOnly;
            tbox_SineMinSignal.ReadOnly = false;

            tbox_SWCapStart.ReadOnly = bIsReadOnly;
            tbox_SWCapEnd.ReadOnly = bIsReadOnly;
            cb_SWCapStep.Enabled = !bIsReadOnly;
            tbox_SWCapMinSignal.ReadOnly = false;

            tbox_SelfStartFre.ReadOnly = bIsReadOnly;
            tbox_SelfEndFre.ReadOnly = bIsReadOnly;
            cb_SelfStepFre.Enabled = !bIsReadOnly;
            tbox_SelfMinSignal.ReadOnly = false;

            tbox_Dump1StartFre.ReadOnly = bIsReadOnly;
            tbox_Dump1EndFre.ReadOnly = bIsReadOnly;
            cb_Dump1StepFre.Enabled = !bIsReadOnly;
            tbox_Dump1MinSignal.ReadOnly = false;

            tbox_Dump2StartFre.ReadOnly = bIsReadOnly;
            tbox_Dump2EndFre.ReadOnly = bIsReadOnly;
            cb_Dump2StepFre.Enabled = !bIsReadOnly;
            tbox_Dump2MinSignal.ReadOnly = false;

        }

        private void SetLogValueToUI()
        {
            tbox_SineStartFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_Start.ToString();
            tbox_SineEndFreq.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_End.ToString();
            cb_SineStepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_FreqStep;
            tbox_SineMinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_MinSignalLevel.ToString();

            tbox_SWCapStart.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Swcap_Start.ToString();
            tbox_SWCapEnd.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Swcap_End.ToString();
            cb_SWCapStep.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Swcap_FreqStep;
            tbox_SWCapMinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Swcap_MinSignalLevel.ToString();

            tbox_SelfStartFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Self_Start.ToString();
            tbox_SelfEndFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Self_End.ToString();
            cb_SelfStepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Self_FreqStep;
            tbox_SelfMinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Self_MinSignalLevel.ToString();

            tbox_Dump1StartFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump1_Start.ToString();
            tbox_Dump1EndFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump1_End.ToString();
            cb_Dump1StepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump1_FreqStep;
            tbox_Dump1MinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump1_MinSignalLevel.ToString();

            tbox_Dump2StartFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump2_Start.ToString();
            tbox_Dump2EndFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump2_End.ToString();
            cb_Dump2StepFre.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump2_FreqStep;
            tbox_Dump2MinSignal.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump2_MinSignalLevel.ToString();

        }

        //20220808
        private bool SelectFile(string strFilter, string strInitialDirectory, out string FileName)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = strFilter;  //在對話方塊中可以使用檔案篩選選項
            openDlg.InitialDirectory = strInitialDirectory;//執行檔資料夾的位置
            openDlg.FileName = "";                     //取得或設定含有檔案對話方塊中所選取檔名的字串
            openDlg.CheckFileExists = true;      //如果當使用者指定不存在的檔名時對話方塊顯示警告訊息，屬性值為 true
            openDlg.CheckPathExists = true;    //如果當使用者指定了不存在的路徑時要顯示警告對話方塊，屬性值為 true
            openDlg.Multiselect = false;              //如果對話方塊允許一起或同時選取多個檔案，則為 true
            FileName = "";
            if(openDlg.ShowDialog(this) != DialogResult.OK)
                return false;
            FileName = openDlg.FileName;
            return true;
        }

        private void ShowFormula(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            //判斷Form有沒有被開啟
            foreach(Form frm in fc)
            {
                if(frm.Name == NoiseFrequencyFormulaForm.Name)
                    return;
            }

            if(NoiseFrequencyFormulaForm.IsDisposed)
                NoiseFrequencyFormulaForm = new NoiseFrequencyFormulafm();

            NoiseFrequencyFormulaForm.Show();
        }

        private void btnTransform_Click(object sender, EventArgs e)
        {
            if(m_Protocol_VerNum.Major != 0x06)
                return;
            var _FrqDataType = (enFrqDataType)tabControl_DataType.SelectedIndex;
            NoiseFrqVariable._DataType_ temp_DataType_ = new NoiseFrqVariable._DataType_();
            switch(_FrqDataType)
            {
                case enFrqDataType._MC_SW_Cap_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.SWCap;
                    break;

                case enFrqDataType._SC_SW_Cap_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.Self;
                    break;

                case enFrqDataType._SC_Dump1_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.Dump1;
                    break;

                case enFrqDataType._SC_Dump2_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.Dump2;
                    break;

                default:
                    return;
            }


            NoiseFrequencyFormulaTransformfm = new NoiseFrequencyFormulaTransform(m_iSelectedDev, ref m_LocalVariable);

            var tmpBuff = new List<NoiseFrqVariable._NoiseData_>();

            switch(NowSelectDataType)
            {
                case NoiseFrqVariable.enDataType.Average:
                    tmpBuff = temp_DataType_.m_listAvg;
                    break;
                case NoiseFrqVariable.enDataType.FrequencyCnts:
                    tmpBuff = temp_DataType_.m_listFrCnt;
                    break;
                case NoiseFrqVariable.enDataType.Maximum:
                    tmpBuff = temp_DataType_.m_listMax;
                    break;
            }
            NoiseFrequencyFormulaTransformfm.ImportData(_FrqDataType, tmpBuff);
            NoiseFrequencyFormulaTransformfm.MySetUI_Update += SetBarData;
            NoiseFrequencyFormulaTransformfm.Show();
        }

        private void SetBarData(List<NoiseFrqVariable._NoiseData_> Buff)
        {
            int iMax = 0;
            NoiseFrqVariable._DataType_ temp_DataType_ = new NoiseFrqVariable._DataType_();
            var _FrqDataType = (enFrqDataType)tabControl_DataType.SelectedIndex;
            switch(_FrqDataType)
            {
                case enFrqDataType._MC_SW_Cap_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.SWCap;
                    break;

                case enFrqDataType._SC_SW_Cap_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.Self;
                    break;

                case enFrqDataType._SC_Dump1_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.Dump1;
                    break;

                case enFrqDataType._SC_Dump2_:
                    temp_DataType_ = m_LocalVariable.NoiseFrqVar.Dump2;
                    break;

                default:
                    return;
            }
            SeriesBar.Clear();
            SeriesLine.Clear();
            ShowBarData(temp_DataType_._params.iTotalCount, _FrqDataType, NowSelectDataType, Buff, ref iMax);
            m_tChart_NoiseFre.Axes.Left.SetMinMax(0, iMax + 5);
        }

        private void NoiseFrefm_VisibleChanged(object sender, EventArgs e)
        {
            bool bIsDevOffline = StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus != DevStatus.Connected;
            if(!bIsDevOffline)
            {
                btn_Scan.Text = "Scan";
                chk_noiseFrq.Enabled = true;
            }
            else
            {
                btn_Scan.Text = "Replay";
                chk_noiseFrq.Enabled = false;
            }

            SetNoiseFre_BtnStatus(chk_noiseFrq.Enabled);
            var _PROTOCOL = (m_LocalVariable == null || bIsDevOffline) ? enPROTOCOL.none : m_LocalVariable.Var_Multi.ProtocolType;
            SetScanDataType(_PROTOCOL);
        }

        private void chkListBox_ScanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var checkedListBox = sender as CheckedListBox;
            if(checkedListBox.SelectedItem == null)
                return;
            var ChkItem = EnumTool.GetValueFromDescription<en_UI_NoiseDataType>(checkedListBox.SelectedItem.ToString());
            showScanDataMinFre(ChkItem);
            vfShowBarData((enFrqDataType)tabControl_DataType.SelectedIndex);
        }

        private void chkListBox_ScanType_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var checkedListBox = sender as CheckedListBox;

            //至少保留最後一個項目被勾選
            if(e.NewValue == CheckState.Unchecked && checkedListBox.CheckedItems.Count <= 1 && checkedListBox.GetItemChecked(e.Index))
                e.NewValue = CheckState.Checked;
        }

        private void cb_Step_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46)
            {
                e.Handled = true;
            }
        }


        private void cb_Step_TextChanged(object sender, EventArgs e)
        {
            ComboBox mySender = sender as ComboBox;
            if (mySender.Text.Contains("."))
                return;
            if (mySender.Text == "")
                mySender.Text = "0";
            double number = double.Parse(mySender.Text);
         
            mySender.Text = number.ToString();
            if (number >= 0 && number <= 255)
            {
                return;
            }
            mySender.Text = mySender.Text.Remove(mySender.Text.Length - 1);
            mySender.SelectionStart = mySender.Text.Length;
        }

        private void btn_SetScale_Click(object sender, EventArgs e)
        {
            int iMax = 0;
            Int32.TryParse(numScale.Value.ToString(), out iMax);
            if(iMax > 0)
                m_tChart_NoiseFre.Axes.Left.SetMinMax(0,iMax);
        }

        private void btn_ResetSignalLevel_Click(object sender, EventArgs e)
        {
            int iValue = -1;
           

            switch ((enFrqDataType)tabControl_DataType.SelectedIndex)
            {
                case enFrqDataType._MC_SINE_:
                    int.TryParse(tbox_SineMinSignal.Text, out iValue);
                    if (iValue > 0 && (nowFunType == FunType.ReplayLog))
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Sine_MinSignalLevel = iValue;
                    else if (iValue > 0)
                        m_LocalVariable.NoiseFrqVar.Sine._params.iMinSignal = iValue;
                    break;

                case enFrqDataType._MC_SW_Cap_:
                    int.TryParse(tbox_SWCapMinSignal.Text, out iValue);
                    if (iValue > 0 && (nowFunType == FunType.ReplayLog))
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Swcap_MinSignalLevel = iValue;
                    else if (iValue > 0)
                        m_LocalVariable.NoiseFrqVar.SWCap._params.iMinSignal = iValue;
                    break;
                case enFrqDataType._SC_SW_Cap_:
                    int.TryParse(tbox_SelfMinSignal.Text, out iValue);
                    if (iValue > 0 && (nowFunType == FunType.ReplayLog))
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Self_MinSignalLevel = iValue;
                    else if (iValue > 0)
                        m_LocalVariable.NoiseFrqVar.Self._params.iMinSignal = iValue;
                    break;

                case enFrqDataType._SC_Dump1_:
                    int.TryParse(tbox_Dump1MinSignal.Text, out iValue);
                    if (iValue > 0 && (nowFunType == FunType.ReplayLog))
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump1_MinSignalLevel = iValue;
                    else if (iValue > 0)
                        m_LocalVariable.NoiseFrqVar.Dump1._params.iMinSignal = iValue;
                    break;

                case enFrqDataType._SC_Dump2_:
                    int.TryParse(tbox_Dump2MinSignal.Text, out iValue);
                    if (iValue > 0 && (nowFunType == FunType.ReplayLog))
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.m_FrequencyReplay.Dump2_MinSignalLevel = iValue;
                    else if (iValue > 0)
                        m_LocalVariable.NoiseFrqVar.Dump2._params.iMinSignal = iValue;
                    break;

                case enFrqDataType._Pen_:
                    break;

                default:
                    break;
            }
            if (nowFunType != FunType.ReplayLog)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.UpdateJudgement();

            vfShowBarData((enFrqDataType)tabControl_DataType.SelectedIndex);
        }
    }
}