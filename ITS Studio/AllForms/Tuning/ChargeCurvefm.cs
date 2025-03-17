using CommonExt;
using FuncMethodCmd;
using Steema.TeeChart;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using ThirdPartyTools;
using System.IO;

namespace ITS_Studio.AllForms
{
    public partial class ChargeCurvefm : Form
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

        private NotStaticVar m_LocalVariable;
        private ChargeCurveTool m_LocalChargeCurveTool;
        //private enFrqDataType NowSelectedScanMode = enFrqDataType._MC_SINE_;
        //private Point[] m_Positions;
        private Point[] m_Default_Positions;
        private bool[] bSeriesEn;
        //private bool _TwoShotEn = false;
        private string DataItemType = "Max";
        private DataTable m_MyDT;
        private List<string> m_lsDataColumn;
        private UIPage MyUIPage = new UIPage();
        private DynamicTool MyTool = new DynamicTool();
        private UIStyleManager StyleManager;
        private INI_Tool History_INI = new INI_Tool(ITS_Directory.History_INI);
        private bool use_commonflow = true;

        public ChargeCurvefm()
        {
            InitializeComponent();

            if (Directory.Exists("NoCurveCommonFlow"))
                use_commonflow = false;
        }

        private void ChargeCurvefm_Load(object sender, EventArgs e)
        {
            StyleManager = new UIStyleManager(this.components);
            StyleManager.Style = UIStyle.Orange;
        }

        private string[] strColArr = new string[] { "Enable", "X - CH", "Y - CH" };
        
        private void DataGridview_Attribute()
        {
            m_MyDT = new DataTable();
            DataColumn[] _DataColumn = new DataColumn[] {
                new DataColumn(strColArr[0], typeof(Boolean)) ,
                new DataColumn(strColArr[1], typeof(string)),
                new DataColumn(strColArr[2], typeof(string))
            };
            m_lsDataColumn = _DataColumn.Select(x => x.ColumnName).ToList();
            if(m_MyDT.Columns.Count != 0)
                m_MyDT.Columns.Clear();

            m_MyDT.Columns.AddRange(_DataColumn);
            GridView_Series.DataSource = m_MyDT;

            GridView_Series.Columns[m_lsDataColumn[0]].Frozen = true;
            GridView_Series.Columns[m_lsDataColumn[0]].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            GridView_Series.Columns[m_lsDataColumn[0]].ReadOnly = false;
            GridView_Series.Columns[m_lsDataColumn[0]].Width = 62;
            GridView_Series.Columns[m_lsDataColumn[0]].SortMode = DataGridViewColumnSortMode.NotSortable;

            GridView_Series.Columns[m_lsDataColumn[1]].Frozen = false;
            GridView_Series.Columns[m_lsDataColumn[1]].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            GridView_Series.Columns[m_lsDataColumn[1]].ReadOnly = false;
            GridView_Series.Columns[m_lsDataColumn[1]].SortMode = DataGridViewColumnSortMode.NotSortable;

            GridView_Series.Columns[m_lsDataColumn[2]].Frozen = false;
            GridView_Series.Columns[m_lsDataColumn[2]].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            GridView_Series.Columns[m_lsDataColumn[2]].ReadOnly = false;
            GridView_Series.Columns[m_lsDataColumn[2]].SortMode = DataGridViewColumnSortMode.NotSortable;
            
        }

        private void UpdateOfflineRows(DataTable MyDT)
        {
            int irow = 0;
            foreach(var item in m_LocalVariable.ChargeCurveVar.m_Positions)
            {
                MyDT.Rows[irow][strColArr[1]] = item.X;
                MyDT.Rows[irow][strColArr[2]] = item.Y;
                irow++;
            }            
        }

        private void DefaultRows(enFrqDataType ScanMode, DataTable MyDT, int iRows)
        {
            if(MyDT.Rows.Count != 0)
                MyDT.Rows.Clear();
            int iX = m_LocalVariable.Var_Multi.DevPanelInfo.iAA_X;
            int iY = m_LocalVariable.Var_Multi.DevPanelInfo.iAA_Y;
            int[] X_pos;
            int[] Y_pos;
            if(ScanMode == enFrqDataType._SC_SW_Cap_)
            {
                X_pos = new int[] { 2, (iX / 5 * 2), (iX / 2), (iX / 5 * 4), iX - 1 };
                Y_pos = new int[] { 2, ((iY - 3) / 4 * 2), ((iY - 3) / 4 * 3), iY - 1 };
            }
            else
            {
                X_pos = new int[] { 2, iX / 2, iX - 1 };
                Y_pos = new int[] { 2, iY / 2, iY - 1 };
            }

            for(int iRow = 0; iRow < iRows; iRow++)
            {
                switch(iRow)
                {
                    case 0:
                        if(ScanMode == enFrqDataType._SC_SW_Cap_)
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[0], -1);
                        else
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[0], Y_pos[0]);
                        break;

                    case 1:
                        if(ScanMode == enFrqDataType._SC_SW_Cap_)
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[1], -1);
                        else
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[1], Y_pos[0]);
                        break;

                    case 2:
                        if(ScanMode == enFrqDataType._SC_SW_Cap_)
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[2], -1);
                        else
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[2], Y_pos[0]);
                        break;

                    case 3:
                        if(ScanMode == enFrqDataType._SC_SW_Cap_)
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[3], -1);
                        else
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[0], Y_pos[1]);
                        break;

                    case 4:
                        if(ScanMode == enFrqDataType._SC_SW_Cap_)
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[4], -1);
                        else
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[1], Y_pos[1]);
                        break;
                    //=====================================================================================
                    case 5:
                        if(ScanMode == enFrqDataType._SC_SW_Cap_)
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(-1, Y_pos[0]);
                        else
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[2], Y_pos[1]);
                        break;

                    case 6:
                        if(ScanMode == enFrqDataType._SC_SW_Cap_)
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(-1, Y_pos[1]);
                        else
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[0], Y_pos[2]);
                        break;

                    case 7:
                        if(ScanMode == enFrqDataType._SC_SW_Cap_)
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(-1, Y_pos[2]);
                        else
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[1], Y_pos[2]);
                        break;

                    case 8:
                        if(ScanMode == enFrqDataType._SC_SW_Cap_)
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(-1, Y_pos[3]);
                        else
                            m_LocalVariable.ChargeCurveVar.m_Positions[iRow] = new Point(X_pos[2], Y_pos[2]);
                        break;
                }

                DataRow _row_ = MyDT.NewRow();
                bSeriesEn[iRow] = true;
                _row_[m_lsDataColumn[0]] = true;
                _row_[m_lsDataColumn[1]] = string.Format("{0}", m_LocalVariable.ChargeCurveVar.m_Positions[iRow].X);
                _row_[m_lsDataColumn[2]] = string.Format("{0}", m_LocalVariable.ChargeCurveVar.m_Positions[iRow].Y);
                MyDT.Rows.Add(_row_);
            }

            m_LocalVariable.ChargeCurveVar.m_Positions.CopyTo(m_Default_Positions, 0);
        }

        private void CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var _Sender = sender as DataGridView;
            _Sender.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void CellChangedValue(object sender, DataGridViewCellEventArgs e)
        {
            var _Sender = sender as DataGridView;
            var _Value = _Sender.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString();
            if(string.IsNullOrEmpty(_Value))
                _Sender.CancelEdit();

            if(e.ColumnIndex == 0)
            {
                int num_1 = -1, num_2 = -1;
                if(_Value == bool.TrueString)
                {
                    //var idx = e.ColumnIndex * 3 + e.RowIndex - 1;
                    num_1 = m_Default_Positions[e.RowIndex].X;
                    num_2 = (m_LocalVariable.ChargeCurveVar.NowSelectedScanMode == enFrqDataType._SC_SW_Cap_) ? -1 : m_Default_Positions[e.RowIndex].Y;
                }

                _Sender.Rows[e.RowIndex].Cells[1].Value = num_1.ToString();
                _Sender.Rows[e.RowIndex].Cells[2].Value = num_2.ToString();
                m_LocalVariable.ChargeCurveVar.m_Positions[e.RowIndex] = new Point(num_1, num_2);
                bSeriesEn[e.RowIndex] = bool.Parse(_Value);
            }
        }

        //col和row從0開始計算
        private void CellEndModify(object sender, DataGridViewCellEventArgs e)
        {
            var _Sender = sender as DataGridView;

            if(e.ColumnIndex == 0)
                return;

            var _Value = _Sender.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString();

            if(string.IsNullOrEmpty(_Value))
            {
                _Sender.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                _Sender.CancelEdit();
            }

            int num_1 = -1, num_2 = -1;
            bool bIsNumber = int.TryParse(_Value, out num_1);
            if(!bIsNumber)
                return;

            if(e.ColumnIndex == 1)
            {
                if(m_LocalVariable.ChargeCurveVar.NowSelectedScanMode == enFrqDataType._SC_SW_Cap_)
                {
                    m_LocalVariable.ChargeCurveVar.m_Positions[e.RowIndex] = new Point(num_1, num_2);
                    _Sender.Rows[e.RowIndex].Cells[2].Value = num_2.ToString();
                }
                else
                {
                    m_LocalVariable.ChargeCurveVar.m_Positions[e.RowIndex] = new Point(num_1, m_LocalVariable.ChargeCurveVar.m_Positions[e.RowIndex].Y);
                }
            }
            else if(e.ColumnIndex == 2)
            {
                if(m_LocalVariable.ChargeCurveVar.NowSelectedScanMode == enFrqDataType._SC_SW_Cap_)
                {
                    m_LocalVariable.ChargeCurveVar.m_Positions[e.RowIndex] = new Point(num_2, num_1);
                    _Sender.Rows[e.RowIndex].Cells[1].Value = num_2.ToString();
                }
                else
                {
                    m_LocalVariable.ChargeCurveVar.m_Positions[e.RowIndex] = new Point(m_LocalVariable.ChargeCurveVar.m_Positions[e.RowIndex].X, num_1);
                }
            }

        }

        private void UpdateOffline_Param(enFrqDataType _Type)
        {
            var Dic = m_LocalVariable.ChargeCurveVar.OfflineParam;
            tbFrame_Value.Text = Dic[_Type].Frame_Cnt.ToString();

            gbSweepCharge.Enabled = (_Type == enFrqDataType._MC_SINE_) ? false : true;

            lbSweepDump_PostIdleName.Visible = (_Type == enFrqDataType._MC_SINE_) ? false : true;
            tbSweepDump_PostIdleValue.Visible = (_Type == enFrqDataType._MC_SINE_) ? false : true;
            lbSweepDump_FixChargeName.Visible = (_Type == enFrqDataType._MC_SINE_) ? false : true;
            tbSweepDump_FixChargeValue.Visible = (_Type == enFrqDataType._MC_SINE_) ? false : true;
            lbSweepDump_PostIdleUnit.Visible = (_Type == enFrqDataType._MC_SINE_) ? false : true;
            lbSweepDump_FixChargeUnit.Visible = (_Type == enFrqDataType._MC_SINE_) ? false : true;

            tabPage2.Parent = (_Type == enFrqDataType._MC_SINE_) ? null : tbCtl_One_RawCode;

            tbSweepDump_RangeStart.Text = Dic[_Type].SweepDump_Freq_Start.ToString();
            tbSweepDump_RangeEnd.Text = Dic[_Type].SweepDump_Freq_End.ToString();
            tbSweepDump_StepValue.Text = Dic[_Type].SweepDump_Freq_Step.ToString();
            tbSweepDump_PostIdleValue.Text = Dic[_Type].SweepDump_PostIdle == -1 ? "" : Dic[_Type].SweepDump_PostIdle.ToString();
            tbSweepDump_FixChargeValue.Text = Dic[_Type].FixCharge == -1 ? "" : Dic[_Type].FixCharge.ToString();
            tbFixSC_X.Text = Dic[_Type].CsubFix_MC_SC_X == -1 ? "" : Dic[_Type].CsubFix_MC_SC_X.ToString();
            tbFixSC_Y.Text = Dic[_Type].CsubFix_SC_Y == -1 ? "" : Dic[_Type].CsubFix_SC_Y.ToString();

            tbSweepCharge_RangeStart.Text = Dic[_Type].SweepCharge_Start == -1 ? "" : Dic[_Type].SweepCharge_Start.ToString();
            tbSweepCharge_RangeEnd.Text = Dic[_Type].SweepCharge_End == -1 ? "" : Dic[_Type].SweepCharge_End.ToString();
            tbSweepCharge_StepValue.Text = Dic[_Type].SweepCharge_Step == -1 ? "" : Dic[_Type].SweepCharge_Step.ToString();
            tbSweepCharge_PostIdleValue.Text = Dic[_Type].SweepCharge_PostIdle == -1 ? "" : Dic[_Type].SweepCharge_PostIdle.ToString();
            tbSweepCharge_FixDumpValue.Text = Dic[_Type].FixDump == -1 ? "" : Dic[_Type].FixDump.ToString();

            switch(_Type)
            {
                case enFrqDataType._MC_SINE_:
                    gbSweepDump_Freq.Text = "Frequency";
                    gbSweepCharge.Text = "";

                    lbSweepDump_RangeUnit.Text = "KHz";
                    lbSweepDump_StepUnit.Text = "KHz";
                    lbSweepDump_PostIdleUnit.Text = "KHz";
                    lbSweepDump_FixChargeUnit.Text = "KHz";

                    lbSweepCharge_RangeUnit.Text = "";
                    lbSweepCharge_StepUnit.Text = "";

                    lbSweepCharge_PostIdleUnit.Text = "";
                    lbSweepCharge_FixDumpUnit.Text = "";

                    chart_OneShot_FixDump.Axes.Bottom.Title.Text = "Frequency Range (KHz)";
                    chart_TwoShot_Raw_FixDump.Axes.Bottom.Title.Text = "Frequency Range (KHz)";
                    chart_TwoShot_DeltaCs_FixDump.Axes.Bottom.Title.Text = "Frequency Range (KHz)";
                    chart_TwoShot_DeltaCsPercent_FixDump.Axes.Bottom.Title.Text = "Frequency Range (KHz)";
                    break;

                case enFrqDataType._MC_SW_Cap_:
                case enFrqDataType._SC_SW_Cap_:
                    gbSweepDump_Freq.Text = "Sweep Dump";
                    gbSweepCharge.Text = "Sweep Charge";

                    lbSweepDump_RangeUnit.Text = "Code";
                    lbSweepDump_StepUnit.Text = "Code";
                    lbSweepDump_PostIdleUnit.Text = "Code";
                    lbSweepDump_FixChargeUnit.Text = "Code";

                    lbSweepCharge_RangeUnit.Text = "Code";
                    lbSweepCharge_StepUnit.Text = "Code";

                    lbSweepCharge_PostIdleUnit.Text = "Code";
                    lbSweepCharge_FixDumpUnit.Text = "Code";

                    chart_OneShot_FixDump.Axes.Bottom.Title.Text = "Fix Dump x, search charge (code)";
                    chart_TwoShot_Raw_FixDump.Axes.Bottom.Title.Text = "Fix Dump x, search charge (code)";
                    chart_TwoShot_DeltaCs_FixDump.Axes.Bottom.Title.Text = "Fix Dump x, search charge (code)";
                    chart_TwoShot_DeltaCsPercent_FixDump.Axes.Bottom.Title.Text = "Fix Dump x, search charge (code)";

                    chart_OneShot_FixCharge.Axes.Bottom.Title.Text = "Fix charge x, search Dump (code)";
                    chart_TwoShot_Raw_FixCharge.Axes.Bottom.Title.Text = "Fix charge x, search Dump (code)";
                    chart_TwoShot_DeltaCs_FixCharge.Axes.Bottom.Title.Text = "Fix charge x, search Dump (code)";
                    chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Bottom.Title.Text = "Fix charge x, search Dump (code)";
                    break;

                default:
                    break;
            }
            
        }

        private void SetUI_DefaultParam(enFrqDataType _Type)
        {
            var Dic = m_LocalVariable.ChargeCurveVar.DefaultParam;
            tbFrame_Value.Text = Dic[_Type].Frame_Cnt.ToString();

            gbSweepCharge.Enabled = (_Type == enFrqDataType._MC_SINE_) ? false : true;

            lbSweepDump_PostIdleName.Visible = (_Type == enFrqDataType._MC_SINE_) ? false : true;
            tbSweepDump_PostIdleValue.Visible = (_Type == enFrqDataType._MC_SINE_) ? false : true;
            lbSweepDump_FixChargeName.Visible = (_Type == enFrqDataType._MC_SINE_) ? false : true;
            tbSweepDump_FixChargeValue.Visible = (_Type == enFrqDataType._MC_SINE_) ? false : true;
            lbSweepDump_PostIdleUnit.Visible = (_Type == enFrqDataType._MC_SINE_) ? false : true;
            lbSweepDump_FixChargeUnit.Visible = (_Type == enFrqDataType._MC_SINE_) ? false : true;

            tabPage2.Parent = (_Type == enFrqDataType._MC_SINE_) ? null : tbCtl_One_RawCode;

            tbSweepDump_RangeStart.Text = Dic[_Type].SweepDump_Freq_Start.ToString();
            tbSweepDump_RangeEnd.Text = Dic[_Type].SweepDump_Freq_End.ToString();
            tbSweepDump_StepValue.Text = Dic[_Type].SweepDump_Freq_Step.ToString();
            tbSweepDump_PostIdleValue.Text = Dic[_Type].SweepDump_PostIdle == -1 ? "" : Dic[_Type].SweepDump_PostIdle.ToString();
            tbSweepDump_FixChargeValue.Text = Dic[_Type].FixCharge == -1 ? "" : Dic[_Type].FixCharge.ToString();
            tbFixSC_X.Text = Dic[_Type].CsubFix_MC_SC_X == -1 ? "" : Dic[_Type].CsubFix_MC_SC_X.ToString();
            tbFixSC_Y.Text = Dic[_Type].CsubFix_SC_Y == -1 ? "" : Dic[_Type].CsubFix_SC_Y.ToString();

            tbSweepCharge_RangeStart.Text = Dic[_Type].SweepCharge_Start == -1 ? "" : Dic[_Type].SweepCharge_Start.ToString();
            tbSweepCharge_RangeEnd.Text = Dic[_Type].SweepCharge_End == -1 ? "" : Dic[_Type].SweepCharge_End.ToString();
            tbSweepCharge_StepValue.Text = Dic[_Type].SweepCharge_Step == -1 ? "" : Dic[_Type].SweepCharge_Step.ToString();
            tbSweepCharge_PostIdleValue.Text = Dic[_Type].SweepCharge_PostIdle == -1 ? "" : Dic[_Type].SweepCharge_PostIdle.ToString();
            tbSweepCharge_FixDumpValue.Text = Dic[_Type].FixDump == -1 ? "" : Dic[_Type].FixDump.ToString();

            switch(_Type)
            {
                case enFrqDataType._MC_SINE_:
                    gbSweepDump_Freq.Text = "Frequency";
                    gbSweepCharge.Text = "";

                    lbSweepDump_RangeUnit.Text = "KHz";
                    lbSweepDump_StepUnit.Text = "KHz";
                    lbSweepDump_PostIdleUnit.Text = "KHz";
                    lbSweepDump_FixChargeUnit.Text = "KHz";

                    lbSweepCharge_RangeUnit.Text = "";
                    lbSweepCharge_StepUnit.Text = "";

                    lbSweepCharge_PostIdleUnit.Text = "";
                    lbSweepCharge_FixDumpUnit.Text = "";

                    chart_OneShot_FixDump.Axes.Bottom.Title.Text = "Frequency Range (KHz)";
                    chart_TwoShot_Raw_FixDump.Axes.Bottom.Title.Text = "Frequency Range (KHz)";
                    chart_TwoShot_DeltaCs_FixDump.Axes.Bottom.Title.Text = "Frequency Range (KHz)";
                    chart_TwoShot_DeltaCsPercent_FixDump.Axes.Bottom.Title.Text = "Frequency Range (KHz)";
                    break;

                case enFrqDataType._MC_SW_Cap_:
                case enFrqDataType._SC_SW_Cap_:
                    gbSweepDump_Freq.Text = "Sweep Dump";
                    gbSweepCharge.Text = "Sweep Charge";

                    lbSweepDump_RangeUnit.Text = "Code";
                    lbSweepDump_StepUnit.Text = "Code";
                    lbSweepDump_PostIdleUnit.Text = "Code";
                    lbSweepDump_FixChargeUnit.Text = "Code";

                    lbSweepCharge_RangeUnit.Text = "Code";
                    lbSweepCharge_StepUnit.Text = "Code";

                    lbSweepCharge_PostIdleUnit.Text = "Code";
                    lbSweepCharge_FixDumpUnit.Text = "Code";

                    chart_OneShot_FixDump.Axes.Bottom.Title.Text = "Fix Dump x, search charge (code)";
                    chart_TwoShot_Raw_FixDump.Axes.Bottom.Title.Text = "Fix Dump x, search charge (code)";
                    chart_TwoShot_DeltaCs_FixDump.Axes.Bottom.Title.Text = "Fix Dump x, search charge (code)";
                    chart_TwoShot_DeltaCsPercent_FixDump.Axes.Bottom.Title.Text = "Fix Dump x, search charge (code)";

                    chart_OneShot_FixCharge.Axes.Bottom.Title.Text = "Fix charge x, search Dump (code)";
                    chart_TwoShot_Raw_FixCharge.Axes.Bottom.Title.Text = "Fix charge x, search Dump (code)";
                    chart_TwoShot_DeltaCs_FixCharge.Axes.Bottom.Title.Text = "Fix charge x, search Dump (code)";
                    chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Bottom.Title.Text = "Fix charge x, search Dump (code)";
                    break;

                default:
                    break;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            StaticVar.NowExecuteTool = enToolName.Main;

            WriteTextSafe("");
            UI_GBV.fmITS_Tool.MainButton_Disable();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_Home);
            UI_GBV.fmITS_Tool.Reset_UI();
        }

        private void radioListBox_ChartDisp_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteTextSafe("");
            m_LocalVariable.ChargeCurveVar.NowSelectedScanMode = EnumTool.GetValueFromDescription<enFrqDataType>(radioListBox_ChartDisp.SelectedItem.ToString());
            SetUI_DefaultParam(m_LocalVariable.ChargeCurveVar.NowSelectedScanMode);            
            DefaultRows(m_LocalVariable.ChargeCurveVar.NowSelectedScanMode, m_MyDT, m_LocalChargeCurveTool.iPointCnts);

            foreach(var item in tbCtrl_Chart.TabPages)
                ((TabPage)item).Parent = null;

            tbOne_RawCode.Parent = tbCtrl_Chart;
            switch(m_LocalVariable.ChargeCurveVar.NowSelectedScanMode)
            {
                case enFrqDataType._MC_SINE_:
                    uiGroupBox_CSub.Enabled = false;
                    lbFixSC_X.Text = "";
                    lbFixSC_X.MarkColor = SystemColors.ControlDark;

                    lbFixSC_Y.Text = "";
                    lbFixSC_Y.MarkColor = SystemColors.ControlDark;
                    break;

                case enFrqDataType._MC_SW_Cap_:
                    uiGroupBox_CSub.Enabled = true;
                    lbFixSC_X.Text = "Fix MC";
                    lbFixSC_X.MarkColor = SystemColors.MenuHighlight;
                    lbFixSC_X.Style = UIStyle.Orange;

                    tbFixSC_X.Enabled = true;

                    lbFixSC_Y.Enabled = false;
                    lbFixSC_Y.Text = "";
                    lbFixSC_Y.MarkColor = SystemColors.ControlDark;

                    tbFixSC_Y.Enabled = false;
                    break;

                case enFrqDataType._SC_SW_Cap_:
                    uiGroupBox_CSub.Enabled = true;
                    lbFixSC_X.Text = "Fix SC X";
                    lbFixSC_X.MarkColor = SystemColors.MenuHighlight;
                    lbFixSC_X.Style = UIStyle.Orange;

                    tbFixSC_X.Enabled = true;

                    lbFixSC_Y.Enabled = true;
                    lbFixSC_Y.Text = "Fix SC Y";
                    lbFixSC_Y.MarkColor = SystemColors.MenuHighlight;
                    lbFixSC_Y.Style = UIStyle.Orange;

                    tbFixSC_Y.Enabled = true;
                    break;
            }

            splitContainer_AllParams.Panel1Collapsed = (m_LocalVariable.ChargeCurveVar.NowSelectedScanMode == enFrqDataType._MC_SINE_) ? true : false;
            if(!cb_TwoShotEn.Checked)
                return;
            tbTwo_RawCode.Parent = tbCtrl_Chart;
            tbTwo_DeltaC_Code.Parent = tbCtrl_Chart;
            tbTwo_DeltaC_Percent.Parent = tbCtrl_Chart;

            tabPage4.Parent = (m_LocalVariable.ChargeCurveVar.NowSelectedScanMode == enFrqDataType._MC_SINE_) ? null : tbCtl_Two_RawCode;
            tabPage6.Parent = (m_LocalVariable.ChargeCurveVar.NowSelectedScanMode == enFrqDataType._MC_SINE_) ? null : tbCtl_Two_DeltaC_Code;
            tabPage8.Parent = (m_LocalVariable.ChargeCurveVar.NowSelectedScanMode == enFrqDataType._MC_SINE_) ? null : tbCtl_Two_DeltaC_Percent;

        }

        private void cb_TwoShotEn_CheckStateChanged(object sender, EventArgs e)
        {
            if(cb_TwoShotEn.Checked)
            {
                tbTwo_RawCode.Parent = tbCtrl_Chart;
                tabPage4.Parent = (m_LocalVariable.ChargeCurveVar.NowSelectedScanMode == enFrqDataType._MC_SINE_) ? null : tbCtl_Two_RawCode;
                tbTwo_DeltaC_Code.Parent = tbCtrl_Chart;
                tabPage6.Parent = (m_LocalVariable.ChargeCurveVar.NowSelectedScanMode == enFrqDataType._MC_SINE_) ? null : tbCtl_Two_DeltaC_Code;
                tbTwo_DeltaC_Percent.Parent = tbCtrl_Chart;
                tabPage8.Parent = (m_LocalVariable.ChargeCurveVar.NowSelectedScanMode == enFrqDataType._MC_SINE_) ? null : tbCtl_Two_DeltaC_Percent;
            }
            else
            {
                tbTwo_RawCode.Parent = null;
                //tabPage4.Parent = null;
                tbTwo_DeltaC_Code.Parent = null;
                //tabPage6.Parent = null;
                tbTwo_DeltaC_Percent.Parent = null;
                //tabPage8.Parent = null;
            }
            m_LocalVariable.ChargeCurveVar._TwoShotEn = cb_TwoShotEn.Checked;
        }

        private void ChargeCurvefm_VisibleChanged(object sender, EventArgs e)
        {
            splitContainer_AllParams.SplitterDistance = splitContainer_AllParams.Width / 2;
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                btnRun.Text = "Run";
            else
                btnRun.Text = "Replay";
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string FileName = "";
            WriteTextSafe("");

            BackgroundWorker _ChowChart = new BackgroundWorker();
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
            {
                _ChowChart.DoWork += new DoWorkEventHandler(bgwWorker_DoWork);
                _ChowChart.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwWorker_RunWorkerCompleted);
            }
            else
            {

                if(!SelectFile(out FileName))
                    return;

                _ChowChart.DoWork += new DoWorkEventHandler(bgwWorker_DoWork_offline);
                _ChowChart.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwWorker_RunWorkerCompleted_offline);
            }
            string _str = "";
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_En;
            else
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_Cn;
            MyUIPage.ShowWaitForm(_str);
            _ChowChart.WorkerSupportsCancellation = true;
            _ChowChart.RunWorkerAsync(FileName);

            splitContainer_Main.Panel1.Enabled = false;
            tbCtrl_Chart.Parent = null;
        }
        
        private ParseLog _ParseLog = new ParseLog();
        
        private bool ResetProtocol(List<string> lsTmp)
        {
            DEV_INTERFACE_TYPE TestInterface = DEV_INTERFACE_TYPE.HID;
            string _MCU = "";
            Pen_CDC_Format.enPenType _PenType = Pen_CDC_Format.enPenType.Disable;
            Version _Version = new Version();
            enPROTOCOL _PROTOCOL = enPROTOCOL.none;
            int iAA_X = 0, iAA_Y = 0;
            foreach(var item in lsTmp)
            {
                var _str = item.Split(',');
                if(!EnumTool.IsContainsDescription<ChargeCurveTool.KeyWord>(_str[0].Trim()))
                    continue;
                switch(EnumTool.GetValueFromDescription<ChargeCurveTool.KeyWord>(_str[0].Trim()))
                {
                    case ChargeCurveTool.KeyWord.IC_Type:
                        _MCU = _str[1].Trim();
                        break;

                    case ChargeCurveTool.KeyWord.Pen_Type:
                        _PenType = EnumTool.ParseEnum<Pen_CDC_Format.enPenType>(_str[1].Trim());
                        break;

                    case ChargeCurveTool.KeyWord.Protocol:
                        //_Version = new Version(_str[1].Trim());
                        _Version = StringTool.HexStringToVer(_str[1]);
                        _PROTOCOL = (_Version.Major == 6) ? enPROTOCOL.V6 : enPROTOCOL.V3;
                        break;

                    case ChargeCurveTool.KeyWord.AA_X:
                        iAA_X = int.Parse(_str[1].Trim());
                        break;

                    case ChargeCurveTool.KeyWord.AA_Y:
                        iAA_Y = int.Parse(_str[1].Trim());
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
            
            this.Invoke(new MethodInvoker(delegate()
            {
                ResetChart(true);
            }));


            m_LocalVariable.Var_Multi.DevPanelInfo.MCU = _MCU;
            m_LocalVariable.Var_Multi.DevPanelInfo.SupportPenType = _PenType;
            m_LocalVariable.Var_Multi.Protocol_VerNum = _Version;
            m_LocalVariable.Var_Multi.ProtocolType = _PROTOCOL;
            m_LocalVariable.Var_Multi.DevPanelInfo.iAA_X = iAA_X;
            m_LocalVariable.Var_Multi.DevPanelInfo.iAA_Y = iAA_Y;

            return true;
        }

        private bool LoadLogFileAndResetProtocol(string FileName)
        {
            List<string> lsTmp = new List<string>();
            if(!_ParseLog.SeekReadFileRange_EndTag_1(FileName, 0, "Start_Testing_Time", "===", ref lsTmp))
                return false;
            if(!ResetProtocol(lsTmp))
                return false;
            return true;
        }

        private bool SelectFile(out string strFilePath)
        {
            strFilePath = "";
            //string strInitialDirectory = ITS_Directory.RecordPath + "Record_ChargeCurve"
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Report (*.txt)|*.txt";  //在對話方塊中可以使用檔案篩選選項
            openDlg.DefaultExt = "txt";
            //=========================================
            var tmpPath = History_INI.IniReadValue("Replay_FilePath", "ChargeCurve_Tool_Path", "");
            if(string.IsNullOrEmpty(tmpPath))
                tmpPath = ITS_Directory.RecordPath + "Record_ChargeCurve";

            tmpPath = Path.GetFullPath(tmpPath);
            if(!File.Exists(tmpPath))
                tmpPath = ITS_Directory.RecordPath + "Record_ChargeCurve";

            if(tmpPath.Length > 259)
            {
                MyUIPage.ShowErrorDialog("Error", "The path length exceeds the operating system limit.", UIStyle.Red, true);
                History_INI.IniWriteValueW("Replay_FilePath", "ChargeCurve_Tool_Path", "");
                return false;
            }

            openDlg.InitialDirectory = tmpPath;
            //=========================================
            //openDlg.InitialDirectory = strInitialDirectory;//執行檔資料夾的位置
            openDlg.FileName = "";                     //取得或設定含有檔案對話方塊中所選取檔名的字串
            openDlg.CheckFileExists = true;      //如果當使用者指定不存在的檔名時對話方塊顯示警告訊息，屬性值為 true
            openDlg.CheckPathExists = true;    //如果當使用者指定了不存在的路徑時要顯示警告對話方塊，屬性值為 true
            openDlg.Multiselect = false;              //如果對話方塊允許一起或同時選取多個檔案，則為 true
            openDlg.Title = "Source Change Curve File";
            
            if(openDlg.ShowDialog(this) != DialogResult.OK)
                return false;
            strFilePath = openDlg.FileName;
            string RelativePath = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, strFilePath);
            History_INI.IniWriteValueW("Replay_FilePath", "ChargeCurve_Tool_Path", RelativePath);
            return true;
        }

        private void bgwWorker_DoWork_offline(object sender, DoWorkEventArgs e)
        {
            string FileName = (string)e.Argument;
            e.Result = LoadLogFileAndResetProtocol(FileName);
            if(!(bool)e.Result)
            {
                MyUIPage.ShowErrorDialog("系统提示", "Load Log File Error!", UIStyle.Red, true);
                e.Cancel = true;
                return;
            }

            m_LocalVariable.ChargeCurveVar.ChargeCurveData.Clear();
            foreach(var item in EnumTool.EnumToList<ChargeCurveTool_VAR.ChargeData_Type>())
                m_LocalVariable.ChargeCurveVar.ChargeCurveData.Add(item, new ChargeCurveTool_VAR.RawData());

            ChargeCurveTool_VAR._Param CmdParams = new ChargeCurveTool_VAR._Param();
            CmdParams = m_LocalVariable.ChargeCurveVar.OfflineParam[m_LocalVariable.ChargeCurveVar.NowSelectedScanMode];
            e.Result = m_LocalChargeCurveTool.Get_CurveData_API_Offline(FileName, ref m_LocalVariable.ChargeCurveVar.NowSelectedScanMode, ref CmdParams, ref m_LocalVariable.ChargeCurveVar._TwoShotEn, ref m_LocalVariable.ChargeCurveVar.m_Positions);
            m_LocalVariable.ChargeCurveVar.OfflineParam[m_LocalVariable.ChargeCurveVar.NowSelectedScanMode] = CmdParams;
            if(!(bool)e.Result)
            {
                MyUIPage.ShowErrorDialog("系统提示", "Parse Log File Error!", UIStyle.Red, true);
                e.Cancel = true;
                return;
            }
        }

        private void bgwWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            m_LocalVariable.ChargeCurveVar.FW_MaxSteps_OriData = new List<byte>();
            m_LocalVariable.ChargeCurveVar.ChargeCurveData.Clear();
            foreach(var item in EnumTool.EnumToList<ChargeCurveTool_VAR.ChargeData_Type>())
                m_LocalVariable.ChargeCurveVar.ChargeCurveData.Add(item, new ChargeCurveTool_VAR.RawData());
            ChargeCurveTool_VAR._Param CmdParams = new ChargeCurveTool_VAR._Param();
            int iShotCnts = m_LocalVariable.ChargeCurveVar._TwoShotEn ? 2 : 1;

            if (use_commonflow)
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.m_HID_API.HID_CreateHandle(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].strDevPath);

                if (Wifi_Var.useWifi)
                    CommonFlow.CDC.cdc_cb[m_iSelectedDev].get_raw = new CommonFlow.CDC.get_raw_t(
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.get_raw);

                INI_Tool cdc_ini = new INI_Tool(ITS_Directory.CDCTool_INI);
                CommonFlow.CDC.setting.skip_checksum =
                    cdc_ini.IniReadBool("System", "Skip_Checksum", false);

                m_LocalVariable.tpdev.cdc =
                    CommonFlow.CDC.cdc_init(m_LocalVariable.tpdev.dev, true, ref CommonFlow.CDC.cdc_cb[m_iSelectedDev], IntPtr.Zero);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.commonflow_get_info(
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.info[m_iSelectedDev]);

                INI_Tool ini = new INI_Tool(ITS_Directory.Setting_INI);
                string cdc_len = ini.IniReadValue("Main", "Report_Size", "PacketSz_1024_");
                foreach (PacketSz item in Enum.GetValues(typeof(PacketSz)))
                {
                    if (!item.ToString().Equals(cdc_len))
                        continue;

                    CommonFlow.CDC.cdc_set_data_length(m_LocalVariable.tpdev.cdc, Convert.ToUInt16(item));
                    break;
                }

                CommonFlow.CDC.cdc_prepare(m_LocalVariable.tpdev.cdc);
            }

            for(int iShotCnt = 0; iShotCnt < iShotCnts; iShotCnt++)
            {
                if(iShotCnt != 0)
                {
                    if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                        MyUIPage.ShowWarningDialog("Tip", "Please put on the copper pillar.", UIStyle.Orange, true);
                    else
                        MyUIPage.ShowWarningDialog("提示", "請將銅柱放在上面", UIStyle.Orange, true);
                }
                if (use_commonflow)
                {
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.charge_curve_main(iShotCnt == 0);
                    if (iShotCnt != 0)
                        m_LocalChargeCurveTool.ResortDeltaC_Buffer(m_LocalVariable.ChargeCurveVar.DefaultParam[m_LocalVariable.ChargeCurveVar.NowSelectedScanMode]);
                    continue;
                }

                //1. Set Test Mode + 100ms
                m_LocalChargeCurveTool.ChargeCurveTool_Init();

                //2. Set_ChargeCurve_Info & Get Return Data
                CmdParams = m_LocalVariable.ChargeCurveVar.DefaultParam[m_LocalVariable.ChargeCurveVar.NowSelectedScanMode];
                int Steps_PerSetCDC_Init = 0;
                string strErrMsg = "";
                if(!m_LocalChargeCurveTool.Set_ChargeCurve_Info(m_LocalVariable.ChargeCurveVar.NowSelectedScanMode, CmdParams, m_LocalVariable.ChargeCurveVar.m_Positions, out Steps_PerSetCDC_Init, ref strErrMsg))
                {
                    WriteTextSafe(string.Format("[{0}] {1}", m_LocalVariable.ChargeCurveVar.NowSelectedScanMode, strErrMsg));
                    return;
                }

                //3. Set_CDC_Init & Wait Ack 
                //4. Read Data (0xF2) Fix Charge / Fix Dump            
                if(!m_LocalChargeCurveTool.Get_CurveData_API(iShotCnt, Steps_PerSetCDC_Init, CmdParams, ref strErrMsg))
                {
                    WriteTextSafe(string.Format("[{0}] {1}", m_LocalVariable.ChargeCurveVar.NowSelectedScanMode, strErrMsg));
                    return;
                }
            }

            if (use_commonflow)
            {
                CommonFlow.CDC.cdc_unprepare(m_LocalVariable.tpdev.cdc);

                CommonFlow.CDC.cdc_exit(m_LocalVariable.tpdev.cdc);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();
            }

            if(cbRecord.Checked)
            {
                m_LocalChargeCurveTool.Gen_ChargeCurve_Report(false, m_LocalVariable.ChargeCurveVar.NowSelectedScanMode,
                                                              m_LocalVariable.ChargeCurveVar.DefaultParam[m_LocalVariable.ChargeCurveVar.NowSelectedScanMode],
                                                              m_LocalVariable.ChargeCurveVar.m_Positions, iShotCnts);
            }
        }

        //private void Warning_FormFunc()
        //{
        //    //WarningForm Warning_Form = new WarningForm("Wait", "Please put on the copper pillar.", 300);
        //    WarningForm Warning_Form ;
        //    if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
        //        Warning_Form = new WarningForm("Wait", "Please put on the copper pillar.", 300);
        //    else
        //        Warning_Form = new WarningForm("請稍等", "請將銅柱放在上面.", 300);
        //    if(Warning_Form.InvokeRequired)
        //    {
        //        var d = new MethodInvoker(Warning_FormFunc);
        //        richTextBox_msg.Invoke(d, null);
        //    }
        //    else
        //    {
        //        if(Warning_Form.ShowDialog(this) == DialogResult.OK)
        //            Warning_Form.Dispose();
        //    }
        //}

        private delegate void MethodInvoker_str(string strErrMsg);

        private void WriteTextSafe(string text)
        {
            if(richTextBox_msg.InvokeRequired)
            {
                var d = new MethodInvoker_str(WriteTextSafe);
                richTextBox_msg.Invoke(d, new object[] { text });
            }
            else
            {
                richTextBox_msg.Text = text;
            }
        }
        
        private void bgwWorker_RunWorkerCompleted_offline(object sender, RunWorkerCompletedEventArgs e)
        {
            //bool bResult = (bool)e.Result;
            //if(!bResult)
            //    return;
            tbCtl_One_RawCode.SelectedTab = tabPage1;
            //5. Update to Chart
            if(m_LocalVariable.ChargeCurveVar.NowSelectedScanMode == enFrqDataType._MC_SINE_)
            {
                tabPage1.Text = "(Frequency)";
                tabPage2.Parent = null;
                tabPage3.Text = "(Frequency)";
                tabPage4.Parent = null;
                tabPage5.Text = "(Frequency)";
                tabPage6.Parent = null;
                tabPage7.Text = "(Frequency)";
                tabPage8.Parent = null;
            }
            else
            {
                tabPage1.Text = "(Sweep Charge)";
                tabPage2.Parent = tbCtl_One_RawCode;
                tabPage3.Text = "(Sweep Charge)";
                tabPage4.Parent = tbCtl_Two_RawCode;
                tabPage5.Text = "(Sweep Charge)";
                tabPage6.Parent = tbCtl_Two_DeltaC_Code;
                tabPage7.Text = "(Sweep Charge)";
                tabPage8.Parent = tbCtl_Two_DeltaC_Percent;
            }
            //EnumTool.GetEnumDescription<enFrqDataType>((int)enumType)
            radioListBox_ChartDisp.SelectedItem = m_LocalVariable.ChargeCurveVar.NowSelectedScanMode.Desc();
            cb_TwoShotEn.Checked = m_LocalVariable.ChargeCurveVar._TwoShotEn;
            //Update Param
            UpdateOffline_Param(m_LocalVariable.ChargeCurveVar.NowSelectedScanMode);

            //Update Gridview
            UpdateOfflineRows(m_MyDT);

            //Update Chart
            UpdateSeriesBuff(m_LocalVariable.ChargeCurveVar.NowSelectedScanMode);

            splitContainer_Main.Panel1.Enabled = true;
            tbCtrl_Chart.Parent = splitContainer2.Panel1;

            MyUIPage.HideWaitForm();
        }
        
        private void bgwWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tbCtl_One_RawCode.SelectedTab = tabPage1;
            //5. Update to Chart
            if(m_LocalVariable.ChargeCurveVar.NowSelectedScanMode == enFrqDataType._MC_SINE_)
            {
                tabPage1.Text = "(Frequency)";
                tabPage2.Parent = null;
                tabPage3.Text = "(Frequency)";
                tabPage4.Parent = null;
                tabPage5.Text = "(Frequency)";
                tabPage6.Parent = null;
                tabPage7.Text = "(Frequency)";
                tabPage8.Parent = null;
            }
            else
            {
                tabPage1.Text = "(Sweep Charge)";
                tabPage2.Parent = tbCtl_One_RawCode;
                tabPage3.Text = "(Sweep Charge)";
                tabPage4.Parent = tbCtl_Two_RawCode;
                tabPage5.Text = "(Sweep Charge)";
                tabPage6.Parent = tbCtl_Two_DeltaC_Code;
                tabPage7.Text = "(Sweep Charge)";
                tabPage8.Parent = tbCtl_Two_DeltaC_Percent;
            }
            UpdateSeriesBuff(m_LocalVariable.ChargeCurveVar.NowSelectedScanMode);

            splitContainer_Main.Panel1.Enabled = true;
            tbCtrl_Chart.Parent = splitContainer2.Panel1;

            //Back to normal mode
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
            MyUIPage.HideWaitForm();
        }

        private void UpdateSeriesBuff(enFrqDataType _Type)
        {
            var Dic = m_LocalVariable.ChargeCurveVar.OfflineParam;

            foreach(var item in m_LocalVariable.ChargeCurveVar.ChargeCurveData)
            {
                if(item.Value.Chart_MaxData.Count != 0)
                {
                    if(_Type == enFrqDataType._MC_SINE_)
                    {
                        if(item.Key == ChargeCurveTool_VAR.ChargeData_Type.OneShot_Sine)
                            UpdateChart(chart_OneShot_FixDump, _Type, item.Value, DataItemType);
                        else if(item.Key == ChargeCurveTool_VAR.ChargeData_Type.TwoShot_Raw_Sine)
                            UpdateChart(chart_TwoShot_Raw_FixDump, _Type, item.Value, DataItemType);
                        else if(item.Key == ChargeCurveTool_VAR.ChargeData_Type.TwoShot_DeltaCs_Sine)
                            UpdateChart(chart_TwoShot_DeltaCs_FixDump, _Type, item.Value, DataItemType);
                        else if(item.Key == ChargeCurveTool_VAR.ChargeData_Type.TwoShot_DeltaCsPercent_Sine)
                            UpdateChart(chart_TwoShot_DeltaCsPercent_FixDump, _Type, item.Value, DataItemType);
                    }
                    else
                    {
                        var _Chart = tbCtrl_Chart.Controls.Find(string.Format("chart_{0}", item.Key), true)[0];
                        UpdateChart((TChart)_Chart, _Type, item.Value, DataItemType);
                    }
                }
            }
        }

        private void UpdateChart(TChart _chart, enFrqDataType _FrqDataType, Object ChargeCurveData, string SelectDataType)
        {
            _chart.Visible = false;
            _chart.Series.Clear();
            var Data = ((ChargeCurveTool_VAR.RawData)ChargeCurveData);

            List<int[]> lstChart = new List<int[]>();

            string TitleName = string.Format("({0}) {1}", _FrqDataType, SelectDataType);
            if(SelectDataType.Contains("Max"))
            {
                lstChart = Data.Chart_MaxData;
            }
            else if(SelectDataType.Contains("Avg"))
            {
                lstChart = Data.Chart_AvgData;
            }
            _chart.Header.Lines = new string[] { TitleName };
            int iDataSize = lstChart.Count;
            Steema.TeeChart.Styles.FastLine[] seriess = new Steema.TeeChart.Styles.FastLine[m_LocalChargeCurveTool.iPointCnts];
            var rand = new Random();
            //int AxisY_Max = 0, AxisY_Min = 0;
            for(int iSeries = 0; iSeries < m_LocalChargeCurveTool.iPointCnts; iSeries++)
            {
                if(!bSeriesEn[iSeries])
                    continue;
                seriess[iSeries] = new Steema.TeeChart.Styles.FastLine();
                seriess[iSeries].Title = string.Format("Series{0}", iSeries);

                for(int idx = 0; idx < iDataSize; idx++)
                {
                    int i_X = Data.SpecRange.Item2 + (Data.SpecRange.Item3 * idx);
                    seriess[iSeries].Add(i_X, lstChart[idx][iSeries]);
                }
                _chart.Axes.Left.MaximumOffset = 50;
                _chart.Axes.Bottom.AxisPen.Width = 1;
                _chart.Axes.Left.AxisPen.Width = 1;
                _chart.Legend.FontSeriesColor = true;
                _chart.Legend.LegendStyle = LegendStyles.Series;
                _chart.Legend.Transparent = true;
                _chart.Series.Add(seriess[iSeries]);
            }
            _chart.Visible = true;
        }

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(m_LocalVariable == null)
                return;
            var MySender = sender as ToolStripMenuItem;
            if(MySender == toolStripMenuItem_Max)
            {
                DataItemType = "Max";
                toolStripMenuItem_Avg.Checked = false;
                toolStripMenuItem_Max.Checked = true;
            }
            else if(MySender == toolStripMenuItem_Avg)
            {
                DataItemType = "Avg";
                toolStripMenuItem_Avg.Checked = true;
                toolStripMenuItem_Max.Checked = false;
            }
            UpdateSeriesBuff(m_LocalVariable.ChargeCurveVar.NowSelectedScanMode);
        }

        private void TextBox_Validated(object sender)
        {
            if(m_LocalVariable == null)
                return;
            var _MySender = sender as TextBox;
            var Dic = m_LocalVariable.ChargeCurveVar.DefaultParam;
            var _Value = Dic[m_LocalVariable.ChargeCurveVar.NowSelectedScanMode];
            int TmpValue = -1;
            //==================================================
            // Sweep Charge
            if(_MySender == tbSweepDump_RangeStart)
            {
                if(_Value.SweepDump_Freq_Start_Range.Length == 0)
                    return;
                int.TryParse(_MySender.Text, out TmpValue);
                if(ArrayTool.NumberInRange(TmpValue, _Value.SweepDump_Freq_Start_Range[0], _Value.SweepDump_Freq_Start_Range[1]))
                    _Value.SweepDump_Freq_Start = TmpValue;
                else
                    WriteTextSafe(string.Format("{0} out of range [{1}~{2}]", TmpValue, _Value.SweepDump_Freq_Start_Range[0], _Value.SweepDump_Freq_Start_Range[1]));
                _MySender.Text = _Value.SweepDump_Freq_Start.ToString();
            }
            else if(_MySender == tbSweepDump_RangeEnd)
            {
                if(_Value.SweepDump_Freq_End_Range.Length == 0)
                    return;
                int.TryParse(_MySender.Text, out TmpValue);
                if(ArrayTool.NumberInRange(TmpValue, _Value.SweepDump_Freq_End_Range[0], _Value.SweepDump_Freq_End_Range[1]))
                    _Value.SweepDump_Freq_End = TmpValue;
                else
                    WriteTextSafe(string.Format("{0} out of range [{1}~{2}]", TmpValue, _Value.SweepDump_Freq_End_Range[0], _Value.SweepDump_Freq_End_Range[1]));
                _MySender.Text = _Value.SweepDump_Freq_End.ToString();
            }
            else if(_MySender == tbSweepDump_StepValue)
            {
                int.TryParse(_MySender.Text, out _Value.SweepDump_Freq_Step);
            }
            else if(_MySender == tbSweepDump_PostIdleValue)
            {
                if(_Value.SweepDump_PostIdle_Range.Length == 0)
                    return;
                int.TryParse(_MySender.Text, out TmpValue);
                if(ArrayTool.NumberInRange(TmpValue, _Value.SweepDump_PostIdle_Range[0], _Value.SweepDump_PostIdle_Range[1]))
                    _Value.SweepDump_PostIdle = TmpValue;
                else
                    WriteTextSafe(string.Format("{0} out of range [{1}~{2}]", TmpValue, _Value.SweepDump_PostIdle_Range[0], _Value.SweepDump_PostIdle_Range[1]));
                _MySender.Text = _Value.SweepDump_PostIdle.ToString();
            }
            else if(_MySender == tbSweepDump_FixChargeValue)
            {
                if(_Value.FixCharge_Range.Length == 0)
                    return;
                int.TryParse(_MySender.Text, out TmpValue);
                if(ArrayTool.NumberInRange(TmpValue, _Value.FixCharge_Range[0], _Value.FixCharge_Range[1]))
                    _Value.FixCharge = TmpValue;
                else
                    WriteTextSafe(string.Format("{0} out of range [{1}~{2}]", TmpValue, _Value.FixCharge_Range[0], _Value.FixCharge_Range[1]));
                _MySender.Text = _Value.FixCharge.ToString();
            }
            //==================================================
            // Sweep Dump
            else if(_MySender == tbSweepCharge_StepValue)
            {
                int.TryParse(_MySender.Text, out _Value.SweepCharge_Step);
            }
            else if(_MySender == tbSweepCharge_FixDumpValue)
            {
                if(_Value.FixDump_Range.Length == 0)
                    return;
                int.TryParse(_MySender.Text, out TmpValue);
                if(ArrayTool.NumberInRange(TmpValue, _Value.FixDump_Range[0], _Value.FixDump_Range[1]))
                    _Value.FixDump = TmpValue;
                else
                    WriteTextSafe(string.Format("{0} out of range [{1}~{2}]", TmpValue, _Value.FixDump_Range[0], _Value.FixDump_Range[1]));
                _MySender.Text = _Value.FixDump.ToString();
            }
            else if(_MySender == tbSweepCharge_PostIdleValue)
            {
                if(_Value.SweepCharge_PostIdle_Range.Length == 0)
                    return;
                int.TryParse(_MySender.Text, out TmpValue);
                if(ArrayTool.NumberInRange(TmpValue, _Value.SweepCharge_PostIdle_Range[0], _Value.SweepCharge_PostIdle_Range[1]))
                    _Value.SweepCharge_PostIdle = TmpValue;
                else
                    WriteTextSafe(string.Format("{0} out of range [{1}~{2}]", TmpValue, _Value.SweepCharge_PostIdle_Range[0], _Value.SweepCharge_PostIdle_Range[1]));
                _MySender.Text = _Value.SweepCharge_PostIdle.ToString();
            }
            else if(_MySender == tbSweepCharge_RangeEnd)
            {
                if(_Value.SweepCharge_End_Range.Length == 0)
                    return;
                int.TryParse(_MySender.Text, out TmpValue);
                if(ArrayTool.NumberInRange(TmpValue, _Value.SweepCharge_End_Range[0], _Value.SweepCharge_End_Range[1]))
                    _Value.SweepCharge_End = TmpValue;
                else
                    WriteTextSafe(string.Format("{0} out of range [{1}~{2}]", TmpValue, _Value.SweepCharge_End_Range[0], _Value.SweepCharge_End_Range[1]));
                _MySender.Text = _Value.SweepCharge_End.ToString();
            }
            else if(_MySender == tbSweepCharge_RangeStart)
            {
                if(_Value.SweepCharge_Start_Range.Length == 0)
                    return;
                int.TryParse(_MySender.Text, out TmpValue);
                if(ArrayTool.NumberInRange(TmpValue, _Value.SweepCharge_Start_Range[0], _Value.SweepCharge_Start_Range[1]))
                    _Value.SweepCharge_Start = TmpValue;
                else
                    WriteTextSafe(string.Format("{0} out of range [{1}~{2}]", TmpValue, _Value.SweepCharge_Start_Range[0], _Value.SweepCharge_Start_Range[1]));
                _MySender.Text = _Value.SweepCharge_Start.ToString();
            }
            //==================================================
            // Frame
            else if(_MySender == tbFrame_Value)
                int.TryParse(_MySender.Text, out _Value.Frame_Cnt);

            //==================================================
            Dic[m_LocalVariable.ChargeCurveVar.NowSelectedScanMode] = _Value;
        }

        private void UITextBox_Validated(object sender)
        {
            var _MySender = sender as UITextBox;
            var Dic = m_LocalVariable.ChargeCurveVar.DefaultParam;
            var _Value = Dic[m_LocalVariable.ChargeCurveVar.NowSelectedScanMode];
            int TmpValue = -1;
            // Csub
            if(_MySender == tbFixSC_X)
            {
                int.TryParse(_MySender.Text, out TmpValue);
                if(ArrayTool.NumberInRange(TmpValue, _Value.CsubFix_MC_SC_X_Range[0], _Value.CsubFix_MC_SC_X_Range[1]))
                    _Value.CsubFix_MC_SC_X = TmpValue;
                else
                    WriteTextSafe(string.Format("{0} out of range [{1}~{2}]", TmpValue, _Value.CsubFix_MC_SC_X_Range[0], _Value.CsubFix_MC_SC_X_Range[1]));
                _MySender.Text = _Value.CsubFix_MC_SC_X.ToString();
            }
            else if(_MySender == tbFixSC_Y)
            {
                int.TryParse(_MySender.Text, out TmpValue);
                if(ArrayTool.NumberInRange(TmpValue, _Value.CsubFix_SC_Y_Range[0], _Value.CsubFix_SC_Y_Range[1]))
                    _Value.CsubFix_SC_Y = TmpValue;
                else
                    WriteTextSafe(string.Format("{0} out of range [{1}~{2}]", TmpValue, _Value.CsubFix_SC_Y_Range[0], _Value.CsubFix_SC_Y_Range[1]));
                _MySender.Text = _Value.CsubFix_SC_Y.ToString();
            }

            //==================================================
            Dic[m_LocalVariable.ChargeCurveVar.NowSelectedScanMode] = _Value;
        }

        private void TextBox_Validated(object sender, EventArgs e)
        {
            var _name = sender.GetType().Name;
            if(_name == "UITextBox")
                UITextBox_Validated(sender);
            else
                TextBox_Validated(sender);

        }

        private void tbCtrl_Chart_DrawItem(object sender, DrawItemEventArgs e)
        {
            //var tabControl = (TabControl)sender;
            //StringFormat StrFormat = new StringFormat();
            //StrFormat.LineAlignment = StringAlignment.Center;
            //StrFormat.Alignment = StringAlignment.Center;
            //SolidBrush bruColor = new SolidBrush(Color.SeaShell);
            //SolidBrush bruFont = new SolidBrush(Color.Black);
            //Font font = new System.Drawing.Font("Consolas", 9F, FontStyle.Italic | FontStyle.Underline);

            //Rectangle rec = tabControl.Bounds;
            //e.Graphics.FillRectangle(bruColor, rec);
            ////绘制标签样式
            //for(int i = 0; i < tabControl.TabPages.Count; i++)
            //{
            //    //取得標籤範圍
            //    Rectangle recChild = tabControl.GetTabRect(i);

            //    //繪製標籤顏色
            //    e.Graphics.FillRectangle(bruColor, recChild);

            //    //繪製標籤文字                
            //    DrawRotatedTextAt(e.Graphics, 0, tabControl.TabPages[i].Text, recChild.X + 3, recChild.Y + 3, font, bruFont);
            //}
        }

        public void ResetChart(bool bIsOffline)
        {
            if(bIsOffline)
            {
                groupBox_ScanMode.Enabled = false;
                splitContainer_AllParams.Enabled = false;

                gbSweepDump_Freq.Enabled = false;
                groupBox_FixParams.Enabled = false;
                GridView_Series.ReadOnly = true;
                tbFixSC_X.Enabled = true;
                tbFixSC_Y.Enabled = true;
            }
            else
            {
                groupBox_ScanMode.Enabled = true;
                splitContainer_AllParams.Enabled = true;

                gbSweepDump_Freq.Enabled = true;
                groupBox_FixParams.Enabled = true;
                GridView_Series.ReadOnly = false;
                tbFixSC_X.Enabled = false;
                tbFixSC_Y.Enabled = false;
            }

            chart_OneShot_FixDump.Series.Clear();
            chart_OneShot_FixCharge.Series.Clear();
            chart_TwoShot_Raw_FixDump.Series.Clear();
            chart_TwoShot_Raw_FixCharge.Series.Clear();
            chart_TwoShot_DeltaCs_FixDump.Series.Clear();
            chart_TwoShot_DeltaCs_FixCharge.Series.Clear();
            chart_TwoShot_DeltaCsPercent_FixDump.Series.Clear();
            chart_TwoShot_DeltaCsPercent_FixCharge.Series.Clear();

            m_LocalVariable = UI_GBV.mDev[m_iSelectedDev].LocalVariable;
            m_LocalChargeCurveTool = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ChargeCurveTool_;
            m_LocalChargeCurveTool.LoadXML_Params(ITS_Directory.ChargeCurveSetting, bIsOffline);
            m_LocalVariable.ChargeCurveVar.m_Positions = new Point[m_LocalChargeCurveTool.iPointCnts];
            m_Default_Positions = new Point[m_LocalChargeCurveTool.iPointCnts];
            bSeriesEn = new bool[m_LocalChargeCurveTool.iPointCnts];

            DataGridview_Attribute();

            radioListBox_ChartDisp.SelectedIndexChanged -= radioListBox_ChartDisp_SelectedIndexChanged;
            radioListBox_ChartDisp.ClearSelected();
            radioListBox_ChartDisp.Items.Clear();
            foreach(enFrqDataType enumType in Enum.GetValues(typeof(enFrqDataType)))
            {
                if(enumType == enFrqDataType._Pen_ || enumType == enFrqDataType._SC_Dump1_ || enumType == enFrqDataType._SC_Dump2_)
                    continue;
                radioListBox_ChartDisp.Items.Add(EnumTool.GetEnumDescription<enFrqDataType>((int)enumType));
            }
            radioListBox_ChartDisp.SelectedIndexChanged += radioListBox_ChartDisp_SelectedIndexChanged;
            radioListBox_ChartDisp.SelectedItem = enFrqDataType._MC_SINE_.Desc();

        }

        private void DrawRotatedTextAt(Graphics gr, float angle, string txt, int x, int y, Font the_font, Brush the_brush)
        {
            // Save the graphics state.
            GraphicsState state = gr.Save();
            gr.ResetTransform();

            // Rotate.
            gr.RotateTransform(angle);

            // Translate to desired position. Be sure to append
            // the rotation so it occurs after the rotation.
            gr.TranslateTransform(x, y, MatrixOrder.Append);

            // Draw the text at the origin.
            gr.DrawString(txt, the_font, the_brush, 0, 0);

            // Restore the graphics state.
            gr.Restore(state);
        }

        private void GridView_Series_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            //判断是否可以编辑
            if(dgv.Columns[e.ColumnIndex].Name == "Enable")
            {
                CellContentClick(sender, e);
            }
        }

    }
}
