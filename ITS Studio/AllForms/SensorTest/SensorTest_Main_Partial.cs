using CommonExt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ThirdPartyTools;
using System.Threading.Tasks;
using System.IO.Pipes;
using StreamString;
using Sunny.UI;
using System.Net.Sockets;
using SocketNamespace;
using System.Net;

namespace ITS_Studio
{
    public partial class SensorTest_Detail
    {
        private SensorTest_FailCountStatistics StatisticsForm = new SensorTest_FailCountStatistics();

        private string ReturnTestName(TestItemNameDefine TestItem)
        {
            return TestItem.DescArr(':')[1];
        }

        private void ClearGridView_VerifyTest_Main(List<string> lsReport)
        {
            GridView_VerifyTest.DataSource = null;
            GridView_VerifyTest.SuspendLayout();
            GridView_VerifyTest.ClearAll();

            //========================================
            //新增測項內容
            List<DataColumn> _DataColumn = new List<DataColumn>();
            //List<object> _DataColumn = new List<object>();
            _DataColumn.Add(new DataColumn(TableHeaderName._Source.Desc(), typeof(string)));

            var tmpColumnList = new List<string>();
            //測項行
            
            foreach(var item in UI_GBV.mDev[0].m_GBV._SensorTestTool_.ArrTestResult)
            {
                if(!item.Value.bEnable)
                    continue;
                if(item.Key == TestItemNameDefine.enPaintingTest
                    || item.Key == TestItemNameDefine.enFrequencyScan
                    || item.Key == TestItemNameDefine.enChargeCurveTest
                    )
                    continue;
                string TestName = ReturnTestName(item.Key);
                _DataColumn.Add(new DataColumn(TestName, typeof(string)));
                tmpColumnList.Add(TestName);
            }

            //最後一行
            _DataColumn.Add(new DataColumn(TableHeaderName._Report.Desc(), typeof(string)));

            //DGV_CB_ViewReport = new System.Windows.Forms.DataGridViewLinkColumn();
            tmpColumnList.Add(TableHeaderName._Report.Desc());

            DataTable m_MyDT = new DataTable();
            m_MyDT.Columns.AddRange(_DataColumn.ToArray());
            
            //DataGridViewColumn
            foreach(var Dev in lsReport)
            {
                DataRow DTS_SPEC_Row = m_MyDT.NewRow();
                m_MyDT.Rows.Add(DTS_SPEC_Row);
            }
            GridView_VerifyTest.DataSource = m_MyDT;
            GridView_VerifyTest.ResumeLayout(true);

            //========================================
            //設計每一個column的屬性
            foreach(var name in tmpColumnList)
            {
                GridView_VerifyTest.Columns[name].Frozen = false;
                GridView_VerifyTest.Columns[name].ReadOnly = true;

                if(name == TableHeaderName._Report.Desc())
                {
                    using(Font _font = new Font(GridView_VerifyTest.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Bold | FontStyle.Underline))
                        GridView_VerifyTest.Columns[name].HeaderCell.Style.Font = _font;
                }
            }
            
            // Set Row Text
            for(int iRow = 0; iRow < GridView_VerifyTest.RowCount; iRow++)
                GridView_VerifyTest.Rows[iRow].Cells[(int)TableHeaderName._Source].Value = Path.GetFileNameWithoutExtension(lsReport[iRow]);

            // Disable SortMode
            for(int inCount = 0; inCount < GridView_VerifyTest.ColumnCount; inCount++)
                GridView_VerifyTest.Columns[inCount].SortMode = DataGridViewColumnSortMode.NotSortable;

            StatisticsForm.TestStatistics_Reset();
        }
        
        //private Hashtable FailCount_test;
        
        //private void TestStatistics_Reset(List<string> ItemNames)
        //{
        //    FailCount_test = new Hashtable();
        //    foreach(var item in EnumTool.EnumToList<TestItemNameDefine>())
        //        FailCount_test.Add(item, 0);
            
        //    pie_FailCounts.Clear();
        //    //Label背景透明
        //    pie_FailCounts.Marks.Transparent = true;
        //    pie_FailCounts.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Percent;
        //    //tChart_Pie.Panel.Transparent = true;
        //    tChart_Pie.Refresh();
        //}

        //private void UpdateChart()
        //{
        //    foreach(var item in UI_GBV.mDev[0].m_GBV._SensorTestTool_.ArrTestResult)
        //    {
        //        if(!item.Value.bEnable)
        //            continue;
        //        if(item.Value.iFailCnts <= 0)
        //            continue;
        //        //FailCount[(int)item.Key]++;
        //        FailCount_test[item.Key] = (int)FailCount_test[item.Key] + 1;
        //        string TestName = item.Key.DescArr(':')[1];
        //        if(pie_FailCounts.Labels.Contains(TestName))
        //        {
        //            var idx = pie_FailCounts.Labels.IndexOf(TestName);
        //            pie_FailCounts[idx].Y = (int)FailCount_test[item.Key];
        //        }
        //        else
        //        {
        //            pie_FailCounts.Add((int)FailCount_test[item.Key], TestName);
        //        }
        //    }
        //    tChart_Pie.Refresh();
        //}

        private void UpdateGridView_VerifyTest_Main(object Cell_Pos, object Cell_Style)
        {
            var _ValueStyle = (VerifyTest_CellStyle)Cell_Style;
            var _ValuePosition = (VerifyTest_CellPosition)Cell_Pos;
            
            int iRow = (GridView_VerifyTest.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells[0].Value.ToString() == _ValuePosition.Row_SourceFile)
                    .Select(r => r.Index)).First();
            var TestName = "";
            if (!string.IsNullOrEmpty(_ValuePosition.Col_Report))
                TestName = TableHeaderName._Report.Desc();
            else
                TestName = _ValuePosition.Col_Testitem.DescArr(':')[1];
            
            var iCol = (GridView_VerifyTest.Columns.Cast<DataGridViewColumn>()
                    .Where(r => r.HeaderText == TestName).Select(x=>x.Index)).First();
            
            //Text
            GridView_VerifyTest.Rows[iRow].Cells[iCol].Value = _ValueStyle.strResult;
            //Font
            GridView_VerifyTest.Rows[iRow].Cells[iCol].Style.Font = _ValueStyle.FontStyle;
            //Back Color
            GridView_VerifyTest.Rows[iRow].Cells[iCol].Style.BackColor = _ValueStyle.BackColor;

        }

        private void OnSenTestUIUpdata(ref Message msg)
        {
            var CellStyle = (VerifyTest_CellStyle)Marshal.PtrToStructure(msg.LParam, typeof(VerifyTest_CellStyle));
            var CellPosition = (VerifyTest_CellPosition)Marshal.PtrToStructure(msg.WParam, typeof(VerifyTest_CellPosition));
            UpdateGridView_VerifyTest_Main(CellPosition, CellStyle);
        }

        protected override void WndProc(ref Message msg)
        {
            enWM_MSG pEnumPanoMsg = (enWM_MSG)msg.Msg;
            if (mMessageReceiver.Keys.Contains(pEnumPanoMsg))
                mMessageReceiver[pEnumPanoMsg](ref msg);
            base.WndProc(ref msg);
        }

        private StreamStringClass _StreamStr;
        
        private BackgroundWorker PipeBgw;
        
        private ClassSocket MyServerSocket = new ClassSocket();
        
        private Socket tmpMyServerSocket;
        
        private void SendToSocket(object parms)
        {
            if(parms.GetType().Equals(typeof(string)))
            {
                MyServerSocket.AsyncSendMessage(tmpMyServerSocket, (string)parms);
            }
        }
        
        private void SensorTest_Detail_Load(object sender, EventArgs e)
        {
            if(StaticVar.iNowDeviceCnts <= 0)
            {
                tabControl_TestBtn.Enabled = false;
                fm_Setting.btPanelInfoLoad.Enabled = false;
                fm_Setting.btn_CaptureFW_CRC.Enabled = false;
            }

            if((Directory.Exists(ITS_Directory.Online_MultiProfileFolder))
                && (Directory.GetFiles(ITS_Directory.Online_MultiProfileFolder).Length != 0))
                tabPage_StressTest.Parent = tabControl_SensorTestFunc;
            else
                tabPage_StressTest.Parent = null;
                    //CheckBoxGroup_VerifyTest.Items.Add(CheckBoxGroupItem.Online_MultiProfile.Desc());

            if(Directory.Exists(ITS_Directory.VerifyTestFolder))
                tabPage_BackTesting.Parent = tabControl_SensorTestFunc;
            else
                tabPage_BackTesting.Parent = null;
                //CheckBoxGroup_VerifyTest.Items.Add(CheckBoxGroupItem.VerifySingleProfile.Desc());

            if(tabControl_SensorTestFunc.TabCount < 1)
            {
                tabControl_SensorTestFunc.TabVisible = false;
            }
            else
            {
                tabControl_SensorTestFunc.TabVisible = true;
                //tabControl_SensorTestFunc.SelectedIndex = 0;
            }
            
            switch(StaticVar.EnClientMode)
            {
                case enITS_WakeUpMode.PIPE_MODE:
                    NamedPipeClientStream _pipeClient = new NamedPipeClientStream(".", "ITS_Pipe", PipeDirection.InOut, PipeOptions.Asynchronous);
                    _StreamStr = new StreamStringClass(_pipeClient);

                    PipeBgw = new BackgroundWorker();
                    PipeBgw.DoWork += RcvFromPipeMsg;
                    PipeBgw.WorkerSupportsCancellation = true;
                    try
                    {
                        PipeBgw.RunWorkerAsync();
                        _pipeClient.Connect(10000);
                        string ErrStr = "";
                        if(_StreamStr.WriteString("Connected Server success", out ErrStr) == -1)
                            MyUIPage.ShowErrorDialog("Pipe 錯誤提示", ErrStr, UIStyle.Red, false);
                    }
                    catch(Exception ex)
                    {
                        MyTool.Save_DBG_Log("bgw Exception ={0}", ex.Message);
                        Console.WriteLine("Exception : {0}", ex.ToString());
                    }
                    break;

                case enITS_WakeUpMode.SOCKET_MODE:
                    MyServerSocket._GetSocketMessage += RcvFromMyToolSocketMsg;

                    var _serverip = IPAddress.Parse("127.0.0.1");
                    var _Port = int.Parse("54321");
                    //if(!PingServer(_serverip))
                    //    return;

                    Task.Factory.StartNew(() => SOCKET_MODE_Connect(_serverip, _Port));
                    break;
            }

        }
        
        private void SOCKET_MODE_Connect(IPAddress _serverip, int _Port)
        {
            tmpMyServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            MyServerSocket.AsyncConnectServer(tmpMyServerSocket, _serverip, _Port);
        }
        
        private void RcvFromPipeMsg(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                string cmdstr = _StreamStr.ReadString();//from client write
                this.Invoke(new MethodInvoker(delegate()
                {
                    MethodInvokerFunc(cmdstr);
                }));
            }
        }
        
        private void RcvFromMyToolSocketMsg(string str, Color _color)
        {
            //Console.WriteLine(str);
            this.Invoke(new MethodInvoker(delegate()
            {
                MethodInvokerFunc(str);
            }));

        }
        
        private void MethodInvokerFunc(string cmdstr)
        {
            var parms = cmdstr.Split(' ');
            if(!Enum.IsDefined(typeof(enITS_Params), parms[0]))
                return;
            //Socket連線之前，m_TriggerOptions還尚未賦予值
            if(m_TriggerOptions != TriggerMode.BarCode)
            {
                MyUIPage.ShowErrorDialog("Error", "Only support barcode mode", UIStyle.Red, true);
                return;
            }                
            string strCMD = "";
            if(Enum.IsDefined(typeof(enCMD_Type), parms[2]))
            {
                switch(EnumTool.ParseEnum<enCMD_Type>(parms[2]))
                {
                    case enCMD_Type.Connect_Status:
                        //strCMD = ConnectStatus_Func(parms);
                        break;
                    case enCMD_Type.Power_Off:
                        //strCMD = BridgePowerOff_Func(parms);
                        break;
                    case enCMD_Type.Start_Test:
                        strCMD = StartTest_Func(parms);
                        break;
                    case enCMD_Type.Version:
                        strCMD = GetAllVersion(parms);
                        break;
                    case enCMD_Type.Close_Tool:
                        //strCMD = BridgePowerOff_Func(parms);
                        break;
                }
                if(StaticVar.EnClientMode == enITS_WakeUpMode.SOCKET_MODE)
                {
                    SendToSocket(strCMD);
                }
                else if(StaticVar.EnClientMode == enITS_WakeUpMode.PIPE_MODE)
                {
                    string ErrStr = "";
                    if(_StreamStr.WriteString(strCMD, out ErrStr) == -1)
                        MyUIPage.ShowErrorDialog("Pipe 錯誤提示", ErrStr, UIStyle.Red, false);
                }
            }
            //if(parms[1] == enCMD_Type.Start_Test.ToString())
            //{
            //    if(parms[2] == enITS_Params.BARCODE.ToString())
            //    {
            //        textBox_BarCode.Text = parms[3];
            //        tmpKeyEventArgs = new KeyEventArgs(Keys.Enter);
            //        textBox_BarCode_TextChanged(textBox_BarCode);
            //    }
            //}

        }
    }
}
