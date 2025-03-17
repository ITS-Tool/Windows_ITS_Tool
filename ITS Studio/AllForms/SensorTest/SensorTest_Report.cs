using CommonExt;
using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ThirdPartyTools;
using System.Linq;
using System.Collections.Generic;
using Sunny.UI;

namespace ITS_Studio
{
    public partial class SensorTest_Report : Form
    {
        private int m_iSelectedDev = -1;
        public int SelectedDev { set { m_iSelectedDev = value; } get { return m_iSelectedDev; } }

        public UILinkLabel[] lbSectPage = new UILinkLabel[1];

        public HScrollBar CtrlBar = new HScrollBar();
        public Panel m_pn_hsbFrameData = new Panel();
        public Label m_lbFrameCount = new Label();

        private TestItemNameDefine SelectDefine;
        private MultiSenTest_VAR.TestStatus NowTestitem;
        private string[] strLabelText = null;

        private int iSelectReportPage = 0;
        private int _iSelectItemRow = 0;
        private Dictionary<string, _SpecLimitData> SpecData = new Dictionary<string, _SpecLimitData>();
        private struct _SpecLimitData
        {
            public List<int> SpecMax;
            public List<int> SpecMin;
        }

        public SensorTest_Report(int _SelectedDev,int iItemRow)
        {
            InitializeComponent();
            SelectedDev = _SelectedDev;
            _iSelectItemRow = iItemRow;
            uiTitlePanel_SpecData.Collapsed = true;
            dataGridView_Result.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            foreach (var page in tabControl2.TabPages)
                ((TabPage)page).Parent = null;//隱藏

            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            //放大過程的動畫
            //tChart1.Zoom.Animated = true;
        }

        protected override void OnNotifyMessage(Message m)
        {
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        private void SensorTest_Report_Load(object sender, EventArgs e)
        {
            int inLinkLabelNu = 0;
            // Open Form Init
            iSelectReportPage = 0;
            string strKey = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.DT_SensorTest.Rows[_iSelectItemRow][(int)MultiSenTest_VAR.GridView_Cell.ResultTable.TestName].ToString();

            SelectDefine = new TestItemNameDefine();
            foreach (var varValue in EnumTool.EnumToList<TestItemNameDefine>())
            {
                if (UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(varValue).Equals(strKey))
                {
                    SelectDefine = varValue;
                    break;
                }
            }
            NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[SelectDefine];
            Label_Panel.Controls.Clear();

            var listCount = NowTestitem.IsReportNull.FindAll(x => x == false);
            inLinkLabelNu = listCount.Count;

            if (NowTestitem.DT_FramDataArr != null)
                if (NowTestitem.DT_FramDataArr.Length > 1)
                    inLinkLabelNu++;

            lbSectPage = new UILinkLabel[inLinkLabelNu];
            strLabelText = new string[lbSectPage.Length];
            int iNowReportNum = NowTestitem.DT_ReportArr.Length;
            int inX = 0;
            int iReportIdx = 0;

            for (int inCount = 0; inCount < NowTestitem.IsReportNull.Count; inCount++)
            {
                if (NowTestitem.IsReportNull[inCount])
                    continue;

                lbSectPage[iReportIdx] = new UILinkLabel();
                lbSectPage[iReportIdx].AutoSize = true;
                strLabelText[iReportIdx] = NowTestitem.ReportPageName[inCount].Trim();
                lbSectPage[iReportIdx].Text = (iReportIdx == iSelectReportPage) ? (@"* " + strLabelText[iReportIdx]) : strLabelText[iReportIdx];
                Label_Panel.Controls.Add(lbSectPage[iReportIdx]);

                lbSectPage[iReportIdx].Location = new Point(inX, 0);
                inX += lbSectPage[iReportIdx].Width + 2;
                lbSectPage[iReportIdx].LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.vfChangePageEvent);
                iReportIdx++;
            }

            //==================================================
            //Frame Data
            if (NowTestitem.DT_FramDataArr != null)
            {
                if (NowTestitem.DT_FramDataArr.Length > 1)
                {
                    lbSectPage[inLinkLabelNu - 1] = new UILinkLabel();
                    strLabelText[inLinkLabelNu - 1] = "Frame Data";
                    lbSectPage[inLinkLabelNu - 1].Text = strLabelText[inLinkLabelNu - 1];
                    Label_Panel.Controls.Add(lbSectPage[inLinkLabelNu - 1]);
                    lbSectPage[inLinkLabelNu - 1].Location = new Point(0, 0);
                    lbSectPage[inLinkLabelNu - 1].LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.vfChangePageEvent);
                }
            }

            if (NowTestitem.DT_FramDataArr != null)
            {
                if (NowTestitem.DT_FramDataArr.Length > 1)
                {
                    CtrlBar.Minimum = 0;
                    CtrlBar.Maximum = NowTestitem.DT_FramDataArr.Length - 1;
                    CtrlBar.SmallChange = 1;
                    CtrlBar.LargeChange = 1;
                    CtrlBar.Dock = System.Windows.Forms.DockStyle.Fill;
                    CtrlBar.Enabled = true;
                    CtrlBar.ValueChanged += new System.EventHandler(vfChangeFrameEvent);

                    m_pn_hsbFrameData.Size = CtrlBar.Size;
                    m_pn_hsbFrameData.Enabled = false;
                    m_pn_hsbFrameData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    m_pn_hsbFrameData.Controls.Add(CtrlBar);
                    //m_pn_hsbFrameData.Location = new Point(lbSectPage[lbSectPage.Length - 1].Location.X + lbSectPage[lbSectPage.Length - 1].Width, 0);
                    Label_Panel.Controls.Add(m_pn_hsbFrameData);

                    m_lbFrameCount.Visible = false;
                    m_lbFrameCount.Location = new Point(m_pn_hsbFrameData.Location.X + m_pn_hsbFrameData.Width, 0);
                    Label_Panel.Controls.Add(m_lbFrameCount);
                }
            }
            //==================================================

            if (lbSectPage.Length != 0)
                vfChangePageEvent(lbSectPage[iSelectReportPage], null);

            SPEC_Data();
        }

        private void vfChangePageEvent(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel mLinkLabel = (LinkLabel)sender;
            string strSelectKeyWord = mLinkLabel.Text.Replace("*", "").Trim();

            int inSelectIndex = Array.FindIndex(NowTestitem.ReportPageName, s => s.Equals(strSelectKeyWord));
            if ((inSelectIndex < NowTestitem.iResultCnts) && (inSelectIndex >= 0))
            {
                m_pn_hsbFrameData.Enabled = false;
                m_lbFrameCount.Visible = false;
                iSelectReportPage = inSelectIndex;
                vfShowPageLog(iSelectReportPage);
            }
            else
            {
                //Frame 在最後一個
                if (strSelectKeyWord.Contains("Frame Data"))
                {
                    m_pn_hsbFrameData.Enabled = true;
                    m_lbFrameCount.Visible = true;
                    iSelectReportPage = lbSectPage.Length - 1;
                    CtrlBar.Value = 0;
                    vfChangeFrameEvent(null, null);
                }
            }
            int iFind = 0;
            foreach (var item in strLabelText)
            {
                if (item == strSelectKeyWord)
                    lbSectPage[iFind].Text = string.Format("* {0}", item);
                else
                    lbSectPage[iFind].Text = string.Format(" {0}", item);
                iFind++;
            }
        }

        private void vfChangeFrameEvent(object sender, EventArgs e)
        {
            if (!CtrlBar.Enabled)
                return;

            m_lbFrameCount.Text = string.Format("{0}", CtrlBar.Value + 1);
            dataGridView_Result.DataSource = null;
            dataGridView_Result.SuspendLayout();
            dataGridView_Result.Rows.Clear();

            if (NowTestitem.DT_FramDataArr[CtrlBar.Value] != null)
            {
                DataTable dt_Report = new DataTable();
                dt_Report = NowTestitem.DT_FramDataArr[CtrlBar.Value].Copy();
                dataGridView_Result.DataSource = dt_Report;
            }

            // Set Cell Color And Text
            for (int inRow = 0; inRow < dataGridView_Result.RowCount; inRow++)
            {
                for (int inColumn = 0; inColumn < dataGridView_Result.ColumnCount; inColumn++)
                {
                    string strData = dataGridView_Result.Rows[inRow].Cells[inColumn].Value.ToString();
                    Match rxMatch = Regex.Match(strData, @"(\#|\*)(.*)");
                    if (!rxMatch.Success)
                    {
                        dataGridView_Result.Rows[inRow].Cells[inColumn].Value = strData;
                        continue;
                    }

                    dataGridView_Result.Rows[inRow].Cells[inColumn].Value = rxMatch.Groups[2].ToString();

                    if (rxMatch.Groups[1].ToString() == "*")
                        dataGridView_Result.Rows[inRow].Cells[inColumn].Style.BackColor = Color.Tomato;
                    else if (rxMatch.Groups[1].ToString() == "#")
                        dataGridView_Result.Rows[inRow].Cells[inColumn].Style.BackColor = Color.Khaki;
                }
            }

            // Set Row Text
            for (int inCount = 0; inCount < NowTestitem.RowHeaderName[0].Length; inCount++)
                dataGridView_Result.Rows[inCount].HeaderCell.Value = NowTestitem.RowHeaderName[0][inCount];


            dataGridView_Result.ResumeLayout(true);
            // Disable SortMode
            for (int inCount = 0; inCount < dataGridView_Result.ColumnCount; inCount++)
            {
                dataGridView_Result.Columns[inCount].SortMode = DataGridViewColumnSortMode.Programmatic;
                dataGridView_Result.Columns[inCount].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

        }

        private void vfShowPageLog(int iSelectReport)
        {
            if (NowTestitem.DT_ReportArr[iSelectReport] == null)
                return;
            colorGrid1.Clear();

            DataTable dt_Report = new DataTable();
            dt_Report = NowTestitem.DT_ReportArr[iSelectReport].Copy();
            string sSelect_Name = NowTestitem.ReportPageName[iSelectReport];
            switch (SelectDefine)
            {
                //Color Grid
                case TestItemNameDefine.enMicroOpenTest:
                case TestItemNameDefine.enUniformityTest:
                case TestItemNameDefine.enOpenTest:
                    //20220629 Steven Add for new function
                    if (!sSelect_Name.Contains("MinMax"))
                    {
                        tabPage_Gridview.Parent = null;
                        tabPage_Chart.Parent = tabControl2;
                        //tChart1.Axes.Bottom.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.iAA_X + 1);
                        //tChart1.Axes.Left.SetMinMax(0, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.iAA_Y + 1);
                        int iX_Counts = dt_Report.Columns.Count;
                        int iY_Counts = dt_Report.Rows.Count;
                        tChart1.Axes.Bottom.SetMinMax(0, iX_Counts + 1);
                        tChart1.Axes.Left.SetMinMax(0, iY_Counts + 1);
                        for (int iX = 0; iX < iX_Counts; iX++)
                        {
                            for (int iY = 0; iY < iY_Counts; iY++)
                            {
                                var _Value = dt_Report.Rows[iY][iX].ToString();
                                Match rxMatch = Regex.Match(_Value, @"(\#|\*)(.*)");
                                if (rxMatch.Success)
                                {
                                    colorGrid1.Add(iX + 1, double.Parse(rxMatch.Groups[2].ToString()), iY + 1, Color.OrangeRed);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(_Value))
                                        colorGrid1.Add(iX + 1, double.Parse(_Value), iY + 1, Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233))))));
                                }
                            }
                        }
                    }
                    else
                    {
                        //MinMax需要顯示Gridview
                        tabPage_Chart.Parent = null;
                        tabPage_Gridview.Parent = tabControl2;

                        dataGridView_Result.DataSource = null;
                        dataGridView_Result.Rows.Clear();
                        dataGridView_Result.SuspendLayout();
                        dataGridView_Result.DataSource = dt_Report;
                        //DynamicTool MyTool = new DynamicTool();
                        //MyTool.WantToSeeDatatabledt_Reportz()
                        string strDataForMinMax = "";

                        strDataForMinMax = dataGridView_Result.Rows[0].Cells[0].Value.ToString();
                        Match rxMatch = Regex.Match(strDataForMinMax, @"(\#|\*)(.*)");

                        if (rxMatch.Success)
                        {
                            dataGridView_Result.Rows[0].Cells[0].Value = rxMatch.Groups[2].ToString();
                            if (rxMatch.Groups[1].ToString() == "*")
                                dataGridView_Result.Rows[0].Cells[0].Style.BackColor = Color.Tomato;
                            else if (rxMatch.Groups[1].ToString() == "#")
                                dataGridView_Result.Rows[0].Cells[0].Style.BackColor = Color.Khaki;
                        }
                        else
                        {
                            dataGridView_Result.Rows[0].Cells[0].Value = strDataForMinMax;
                        }

                        // Set Row Text
                        //for (int inCount = 0; inCount < dataGridView_Result.RowCount; inCount++)
                        dataGridView_Result.Rows[0].HeaderCell.Value = "MinMax(%)";
                        dataGridView_Result.ResumeLayout(true);
                        // Disable SortMode

                        dataGridView_Result.Columns[0].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dataGridView_Result.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                    }

                    break;


                default://Gridview
                    tabPage_Gridview.Parent = tabControl2;
                    dataGridView_Result.DataSource = null;
                    dataGridView_Result.Rows.Clear();
                    dataGridView_Result.SuspendLayout();
                    dataGridView_Result.DataSource = dt_Report;

                    string strData = "";
                    for (int iRow = 0; iRow < dataGridView_Result.RowCount; iRow++)
                    {
                        for (int iColumn = 0; iColumn < dataGridView_Result.ColumnCount; iColumn++)
                        {
                            strData = dataGridView_Result.Rows[iRow].Cells[iColumn].Value.ToString();
                            Match rxMatch = Regex.Match(strData, @"(\#|\*)(.*)");
                            if (rxMatch.Success)
                            {
                                dataGridView_Result.Rows[iRow].Cells[iColumn].Value = rxMatch.Groups[2].ToString();
                                if (rxMatch.Groups[1].ToString() == "*")
                                    dataGridView_Result.Rows[iRow].Cells[iColumn].Style.BackColor = Color.Tomato;
                                else if (rxMatch.Groups[1].ToString() == "#")
                                    dataGridView_Result.Rows[iRow].Cells[iColumn].Style.BackColor = Color.Khaki;
                            }
                            else
                            {
                                dataGridView_Result.Rows[iRow].Cells[iColumn].Value = strData;
                            }
                        }
                    }

                    // Set Row Text
                    for (int inCount = 0; inCount < dataGridView_Result.RowCount; inCount++)
                        dataGridView_Result.Rows[inCount].HeaderCell.Value = NowTestitem.RowHeaderName[iSelectReport][inCount];
                    dataGridView_Result.ResumeLayout(true);
                    // Disable SortMode
                    for (int inCount = 0; inCount < dataGridView_Result.ColumnCount; inCount++)
                    {
                        dataGridView_Result.Columns[inCount].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dataGridView_Result.Columns[inCount].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                    break;
            }

        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SPEC_Data()
        {
            SpecData.Clear();
            DataTable DTS_SPEC = new DataTable();
            DataRow DTS_SPEC_Row;
            DTS_SPEC.Columns.Add("Name", typeof(string));
            DTS_SPEC.Columns["Name"].ReadOnly = true;
            DTS_SPEC.Columns.Add("Spec.", typeof(string));
            foreach (var item in NowTestitem.SPEC)
            {
                if ((NowTestitem.strTestItemName == "Uniformity_Test") || (NowTestitem.strTestItemName == "MicroOpen_Test"))
                {
                    if (item.ShowType == enSPECShowType.DataGridViewTextBox)
                    {
                        if (!item.Name.Contains("_Tolerance"))
                            continue;
                        DTS_SPEC_Row = DTS_SPEC.NewRow();
                        DTS_SPEC_Row["Name"] = item.Name;
                        DTS_SPEC_Row["Spec."] = item.dtValue.Rows[0][0].ToString();
                        DTS_SPEC.Rows.Add(DTS_SPEC_Row);
                    }
                    else if (item.ShowType == enSPECShowType.DataTable)
                    {
                        if (item.lsDataTable_Value != null)
                        {
                            _SpecLimitData SpecLimitData = new _SpecLimitData();
                            if (NowTestitem.strTestItemName == "Uniformity_Test")
                            {
                                if (!Enum.IsDefined(typeof(Uniformity_SectorName), item.Name))
                                    continue;
                                var _name = (Uniformity_SectorName)Enum.Parse(typeof(Uniformity_SectorName), item.Name);
                                if ((_name == Uniformity_SectorName.Uniformity_RawData) || (_name == Uniformity_SectorName.Uniformity_KeyRawData))
                                {
                                    SpecLimitData.SpecMin = new List<int>();
                                    foreach (var item_2 in item.lsDataTable_Value)
                                        SpecLimitData.SpecMin.AddRange(item_2.MinValue.Select(x => int.Parse(x)).ToArray());
                                }
                                //if ((_name == Uniformity_SectorName.Uniformity_TxDiff)
                                //    || (_name == Uniformity_SectorName.Uniformity_RxDiff))
                                //{
                                //    SpecLimitData.SpecGolden = new List<int>();
                                //    var _Golden = NowTestitem.SPEC.Find(x => x.Name == "Uniformity_Golden").lsDataTable_Value;
                                //    foreach (var item_2 in _Golden)
                                //        SpecLimitData.SpecGolden.AddRange(item_2.MaxValue.Select(x=>int.Parse(x)));
                                //}
                            }
                            SpecLimitData.SpecMax = new List<int>();
                            foreach (var item_2 in item.lsDataTable_Value)
                                SpecLimitData.SpecMax.AddRange(item_2.MaxValue.Select(x => int.Parse(x)).ToArray());

                            SpecData.Add(item.Name, SpecLimitData);
                        }
                    }
                }
                else
                {
                    if (item.ShowType == enSPECShowType.DataGridViewTextBox)
                    {
                        DTS_SPEC_Row = DTS_SPEC.NewRow();
                        DTS_SPEC_Row["Name"] = item.Name;
                        DTS_SPEC_Row["Spec."] = item.dtValue.Rows[0][0].ToString();
                        DTS_SPEC.Rows.Add(DTS_SPEC_Row);
                    }
                }
            }

            dataGridView_SPEC.DataSource = DTS_SPEC;
        }

        private void uiTitlePanel_SpecData_ClientSizeChanged(object sender, EventArgs e)
        {
            if (!uiTitlePanel_SpecData.Collapsed)
                return;
            var _Sender = sender as DataGridView;
            for (int inCount = 0; inCount < dataGridView_SPEC.ColumnCount; inCount++)
            {
                dataGridView_SPEC.Columns[inCount].SortMode = DataGridViewColumnSortMode.Programmatic;
                dataGridView_SPEC.Columns[inCount].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void dataGridView_SPEC_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < dataGridView_SPEC.ColumnCount; i++)
                dataGridView_SPEC.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
        }
        private Point _valueIndex_P = new Point();
        private string KeyName = "";
        private double Z_value = 0;
        private string SelectReportPage = "";
        
        private void tChart1_ClickSeries(object sender, Steema.TeeChart.Styles.Series s, int valueIndex, MouseEventArgs e)
        {
            //Console.WriteLine("tChart1_ClickSeries");
            var _X = (int)s.ValuesLists[0].Value[valueIndex] - 1;
            var _Y = (int)s.ValuesLists[2].Value[valueIndex] - 1;
            _valueIndex_P = new Point(_X, _Y);
            var _SelectPage = lbSectPage.ToList().First(x => x.Text.Contains("*"));
            SelectReportPage = _SelectPage.Text.Replace("*", "").Trim();
            KeyName = ":" + NowTestitem.Spec_UI_PageName;
            Z_value = s.ValuesLists[1].Value[valueIndex];
        }

        private void dataGridView_Result_SelectionChanged(object sender, EventArgs e)
        {
            ((DataGridView)sender).ClearSelection();
        }
        
        private Point _Down = new Point();
        private Point _Up = new Point();

        private void tChart1_MouseClick(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("tChart1_MouseClick");
            _Up = e.Location;
            int height = (_Up.Y - _Down.Y);
            int width = (_Up.X - _Down.X);
            if ((Math.Abs(height) < 50) || (Math.Abs(width) < 50))
                return;
            
            if (width > 0)
            {
                //放大
                Rectangle r = new Rectangle(_Down.X, _Down.Y, Math.Abs(width), Math.Abs(height));
                //tChart1.Zoom.Animated = true;
                //tChart1.Zoom.AnimatedSteps = 100;
                tChart1.Zoom.ZoomRect(r);        
            }
            else
            { 
                //還原
                tChart1.Zoom.Undo();
            }   
        }

        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("tChart1_MouseDown");
            _Down = e.Location;
        }

        private void tChart1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("tChart1_MouseDoubleClick");
            int idx = 0;
            string message = "";
            var _SpecData = "";
            if(TestItemNameDefine.enUniformityTest.Desc().Contains(KeyName))
            {
                if (SelectReportPage.Contains("RawData_Max") || SelectReportPage.Contains("RawData_Min"))
                {
                    idx = (_valueIndex_P.Y * UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X) + _valueIndex_P.X;
                    _SpecData = Uniformity_SectorName.Uniformity_RawData.ToString();
                    message = string.Format("Max = {0} Min = {1}\nValue = {2}"
                    , SpecData[_SpecData].SpecMax[idx]
                    , SpecData[_SpecData].SpecMin[idx]
                    , Z_value);
                }
                else if (SelectReportPage.Contains("Win1_Max"))
                {
                    idx = (_valueIndex_P.Y * (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X - 1)) + _valueIndex_P.X;
                    _SpecData = Uniformity_SectorName.Uniformity_Win1.ToString();
                    message = string.Format("Max = {0} \nValue = {1}"
                    , SpecData[_SpecData].SpecMax[idx]
                    , Z_value);
                }
                else if (SelectReportPage.Contains("Win2_Max"))
                {
                    idx = (_valueIndex_P.Y * (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X - 1)) + _valueIndex_P.X;
                    _SpecData = Uniformity_SectorName.Uniformity_Win2.ToString();
                    message = string.Format("Max = {0} \nValue = {1}"
                    , SpecData[_SpecData].SpecMax[idx]
                    , Z_value);
                }
                else if (SelectReportPage.Contains("TxDiff"))
                {
                    idx = (_valueIndex_P.Y * (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X)) + _valueIndex_P.X;
                    _SpecData = Uniformity_SectorName.Uniformity_TxDiff.ToString();
                    message = string.Format("Max = {0}% \nValue = {1}%"
                    , SpecData[_SpecData].SpecMax[idx]
                    , Z_value);
                }
                else if (SelectReportPage.Contains("RxDiff"))
                {
                    idx = (_valueIndex_P.Y * (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X - 1)) + _valueIndex_P.X;
                    _SpecData = Uniformity_SectorName.Uniformity_RxDiff.ToString();
                    message = string.Format("Max = {0}% \nValue = {1}%"
                    , SpecData[_SpecData].SpecMax[idx]
                    , Z_value);
                }
                else
                {
                    idx = (_valueIndex_P.Y * (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts - 1)) + _valueIndex_P.X;
                    _SpecData = Uniformity_SectorName.Uniformity_KeyRawData.ToString();
                    message = string.Format("Max = {0} \nValue = {1}"
                    , SpecData[_SpecData].SpecMax[idx]
                    , Z_value);
                }
            }
            else if(TestItemNameDefine.enMicroOpenTest.Desc().Contains(KeyName))
            {
                if (SelectReportPage == "TX_Avg_Delta")
                    idx = _valueIndex_P.Y;
                else
                    idx = (_valueIndex_P.Y * (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X - 1)) + _valueIndex_P.X;

                message = string.Format("Max = {0} \nValue = {1}"
                , SpecData[SelectReportPage].SpecMax[idx]
                , Z_value);
            }
            string _ch = string.Format("(X:{0} Y:{1})", _valueIndex_P.X + 1, _valueIndex_P.Y + 1);
            uiToolTip1.SetToolTip(tChart1, message, _ch);
        }
    }
}