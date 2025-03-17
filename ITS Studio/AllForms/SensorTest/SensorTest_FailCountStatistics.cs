using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CommonExt;
using ThirdPartyTools;

namespace ITS_Studio
{
    public partial class SensorTest_FailCountStatistics : Form
    {
        private List<Tuple<string, Hashtable>> lsTestFile;
        private Hashtable TotalFailCounts;
        private Hashtable FailItemFiles;

        public SensorTest_FailCountStatistics()
        {
            InitializeComponent();
        }
        private List<TestItemNameDefine> m_TestList = new List<TestItemNameDefine>();
        public void TestStatistics_Reset()
        {
            lsTestFile = new List<Tuple<string, Hashtable>>();
            TotalFailCounts = new Hashtable();
            FailItemFiles = new Hashtable();
            m_TestList = UI_GBV.mDev[0].m_GBV._SensorTestTool_.ArrTestResult.AsParallel().Where(x => x.Value.bEnable).Select(x => x.Key).ToList();

            //Pie
            pie_FailCounts.Clear();
            pie_FailCounts.Marks.Transparent = true;
            pie_FailCounts.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Percent;
            tChart_Pie.Refresh();

            //horizBar
            horizBar1.Clear();
            horizBar2.Clear();
            horizBar2.Marks.Transparent = true;
            horizBar2.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Value;
            
            foreach(var item in m_TestList)
            {
                if(item == TestItemNameDefine.enChargeCurveTest
                    || item == TestItemNameDefine.enFrequencyScan)
                    continue;

                horizBar1.Add(100, item.DescArr(':')[1]);
                horizBar2.Add(0, item.DescArr(':')[1]);
                TotalFailCounts.Add(item, 0);
                FailItemFiles.Add(item, "");
            }
            tChart_HorizBar.Refresh();

            InitializeDataTable();
            uiDataGridView1.DataSource = m_dataTable;
        }

        public void UpdateChart(string _LogFile)
        {
            if(lsTestFile.FindIndex(x => x.Item1.Contains(_LogFile)) == -1)
            {
                Tuple<string, Hashtable> MyTuple = new Tuple<string, Hashtable>(_LogFile, new Hashtable());
                foreach(var item in EnumTool.EnumToList<TestItemNameDefine>())
                {
                    if(item == TestItemNameDefine.enChargeCurveTest
                    || item == TestItemNameDefine.enFrequencyScan)
                        continue;
                    MyTuple.Item2.Add(item, 0);
                    
                }
                lsTestFile.Add(MyTuple);
            }

            Tuple<string, Hashtable> SelectedItem = lsTestFile.Find(x => x.Item1 == _LogFile);

            foreach(var item in UI_GBV.mDev[0].m_GBV._SensorTestTool_.ArrTestResult)
            {
                if(!item.Value.bEnable)
                    continue;
                if(item.Value.iFailCnts <= 0)
                    continue;
                
                TotalFailCounts[item.Key] = (int)TotalFailCounts[item.Key] + 1;
                FailItemFiles[item.Key] = FailItemFiles[item.Key] + _LogFile + ",";

                Hashtable myHashtable = SelectedItem.Item2;
                myHashtable[item.Key] = (int)myHashtable[item.Key] + 1;
                SelectedItem = Tuple.Create(SelectedItem.Item1, myHashtable);// 将修改后的 Hashtable 赋值回原組               
 
                string TestName = item.Key.DescArr(':')[1];                
                //Update Pie Chart
                Update_PieChartValue(TestName, (int)TotalFailCounts[item.Key]);
                
            }

            //Update Bar Chart            
            Update_horizBar2Value(TotalFailCounts);
            UpdateDataTable(TotalFailCounts);//所有檔案有多少個Fail
                
            tChart_Pie.Refresh();
        }

        private void Update_horizBar2Value(Hashtable AllFails)
        {
            int iTotalFile = lsTestFile.Count;
            foreach(var item in m_TestList)
            {
                var TestName = item.DescArr(':')[1];
                if(horizBar2.Labels.Contains(TestName))
                {
                    var idx = horizBar2.Labels.IndexOf(TestName);
                    if((int)AllFails[item] != 0)
                    {
                        var tmp = (int)AllFails[item];
                        horizBar2.XValues[idx] = ((double)tmp / (double)iTotalFile) * 100;
                    }
                }                
            }
            
            tChart_HorizBar.Refresh();
        }

        private void Update_PieChartValue(string TestName, int iValue)
        {
            if(pie_FailCounts.Labels.Contains(TestName))
            {
                var idx = pie_FailCounts.Labels.IndexOf(TestName);
                pie_FailCounts[idx].Y = iValue;
            }
            else
            {
                pie_FailCounts.Add(iValue, TestName);
            }
        }
        
        private void uiTabControlMenu1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("uiTabControlMenu1_SelectedIndexChanged");
        }

        private void uiTabControlMenu1_Selected(object sender, TabControlEventArgs e)
        {
            Console.WriteLine("uiTabControlMenu1_Selected");
            if(e.TabPage == tabPage_Files)
            {
            }
            else if(e.TabPage == tabPage_Items)
            {
                //UpdateChart();
            }
        }

        private void SensorTest_FailCountStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private DataTable m_dataTable;
        private string[] columnHeader = new string[] { "不良項目", "Log數量", "比重" };
        private void InitializeDataTable()
        {
            uiDataGridView1.ClearAll();
            
            // 創建一個包含資料的 DataTable
            m_dataTable = new DataTable();
            m_dataTable.Columns.Add(columnHeader[0], typeof(string));
            m_dataTable.Columns.Add(columnHeader[1], typeof(int));
            m_dataTable.Columns.Add(columnHeader[2], typeof(string));

            //加入Testitem
            foreach(var item in m_TestList)
            {
                if(item == TestItemNameDefine.enFrequencyScan)
                    continue;
                if(item == TestItemNameDefine.enChargeCurveTest)
                    continue;
                m_dataTable.Rows.Add(item.DescArr(':')[1], 0, "0.00%");
            }
        }
        
        private void UpdateDataTable(Hashtable AllFails)
        {
            double iTotalFile = lsTestFile.Count;
            foreach(var item in m_TestList)
            {
                var TestName = item.DescArr(':')[1];
                
                DataRow targetRow = m_dataTable.AsEnumerable().FirstOrDefault(row => row.Field<string>(columnHeader[0]) == TestName);
                if(targetRow == null)
                    continue;
                double num = (double)(int)AllFails[item];
                targetRow[columnHeader[1]] = num;
                targetRow[columnHeader[2]] = string.Format("{0:0.00}%", (num / iTotalFile) * 100);                
            }
        }

        private void tChart_HorizBar_ClickSeries(object sender, Steema.TeeChart.Styles.Series s, int valueIndex, MouseEventArgs e)
        {
            string str = "";
            foreach(var item in m_TestList)
            {
                 var TestName = item.DescArr(':')[1];
                 if(TestName == s.Labels[valueIndex])
                 {
                     str = FailItemFiles[item].ToString();
                     break;
                 }
            }

            if(string.IsNullOrEmpty(str))
                return;

            List<string> ls = new List<string>();
            ls = str.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
            // 創建新的 Form
            Form listViewForm = new Form();
            listViewForm.Text = string.Format("{0} Fail", s.Labels[valueIndex]);
            //listViewForm.Size = new System.Drawing.Size(400, 300);
            listViewForm.StartPosition = FormStartPosition.CenterScreen;

            // 創建 ListView
            ListView listView = new ListView();
            listView.Dock = DockStyle.Fill;
            listView.View = View.Details;
            listView.GridLines = true;
            listView.LabelEdit = false;
            listView.FullRowSelect = true;

            // 添加一些示範項目
            listView.Columns.Add("編號", 100);
            listView.Columns.Add("檔名", 500);
            int i = 1;
            foreach(var item in ls)
            {
                var item_1 = new ListViewItem(string.Format("No.{0}", i++));
                item_1.SubItems.Add(item);
                listView.Items.Add(item_1);
            }
            
            // 將 ListView 添加到 Form
            listViewForm.Controls.Add(listView);

            // 顯示新的 Form
            listViewForm.ShowDialog();

        }
    }
}
