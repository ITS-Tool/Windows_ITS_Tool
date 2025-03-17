using CommonExt;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThirdPartyComponent;
using ThirdPartyTools;
using StreamString;
using SocketNamespace;
using System.Net.Sockets;
using System.IO.Pipes;
using System.Net;

namespace ITS_Studio
{
    public partial class SensorTest_Multi : Form
    {
        public struct stuDev
        {
            public int idx;
            //public System.Timers.Timer timer_GpioScan;
            public Task timer_GpioScan;
            public CancellationTokenSource _CanceToken;
            public MultiSenTest_VAR.TestResult Status;
            public Device_Port_Binding PortInfo;
            public bool IsInEditMode;
            public object progressBar;
            public stuDev(MultiSenTest_VAR.TestResult _Status, Device_Port_Binding _PortInfo, bool _IsInEditMode, object _progressBar)
            {
                this.Status = _Status;
                this.PortInfo = _PortInfo;
                this.IsInEditMode = _IsInEditMode;
                this.progressBar = _progressBar;

                this.idx = 0;
                this.timer_GpioScan = null;
                this._CanceToken = null;
            }
        }

        public enum TableHeaderName
        {
            [Description("none")]
            none = -1,
            [Description("TriggerOption")]
            Column_Trigger = 0,
            [Description("ProcessBar")]
            Column_Process = 1,
            [Description("PortChain")]
            Column_PortChain = 2,
            [Description("Report")]
            Column_Report,
        }

        public enum enColor
        {
            En_Color,
            Dis_Color
        };
        public enum enCalResult
        {
            Total,
            Pass,
            Fail
        };
        public enum enProfile
        {
            Read,
            Write
        };

        private int[] iProgressMaximum = new int[StaticVar.iMaxSupportDev];
        private int[] iProgressValue = new int[StaticVar.iMaxSupportDev];
        private stuDev[] TestDev = new stuDev[StaticVar.iMaxSupportDev];

        private DataGridViewCell m_EditingCell;

        public SensorTest_Setting fm_Setting;

        protected DynamicTool MyTool = new DynamicTool();

        private INI_Tool SettingINI = new INI_Tool(ITS_Directory.Setting_INI);

        private int m_iBarCodeLength = 0;

        private System.Timers.Timer m_tiNowClock = new System.Timers.Timer();

        //private NamedTimer[] t_BarcodeCheck = new NamedTimer[StaticVar.iMaxSupportDev];
        
        //private NamedTimer t_BarCodePolling;
        
        public TriggerMode m_TriggerOptions = TriggerMode.Button;

        private List<KeyValuePair<enCalResult, int>> CalResult = new List<KeyValuePair<enCalResult, int>>();
        
        private List<MultiSenTest_VAR.TestResult> BarcodeScaned = new List<MultiSenTest_VAR.TestResult>();
        
        private int[] FailCount;
        
        private UIPage MyUIPage = new UIPage();

        public SensorTest_Multi()
        {
            InitializeComponent();

            m_tiNowClock.Enabled = false;
            m_tiNowClock.Interval = 1000;
            m_tiNowClock.Elapsed += new System.Timers.ElapsedEventHandler(t_NowClock_Tick);
            m_tiNowClock.AutoReset = true;
            m_tiNowClock.SynchronizingObject = this;
        }

        private void lblProfileName_Click(object sender, EventArgs e)
        {
            if((StaticVar._ITSTool_Var != enMode.RDMode) && (!File.Exists("VaneDebug")))
            {
                string value = "";
                string str_1 = "", str_2 = "";
                if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    str_1 = global::ITS_Studio.Properties.Resources.Password_En;
                else
                    str_1 = global::ITS_Studio.Properties.Resources.Password_Cn;
                if(!UIInputDialog.InputPasswordDialog(ref value, true, str_1, UIStyle.Orange, true))
                    return;

                Assembly assembly = Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                string ToolVersion = fvi.ProductVersion;
                string password = ToolVersion.Replace(".", "").Split(' ')[0];

                if(!string.Equals(value, password))
                {
                    if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    {
                        str_1 = global::ITS_Studio.Properties.Resources.PasswordErr_En;
                        str_2 = global::ITS_Studio.Properties.Resources.PasswordTip_En;
                    }
                    else
                    {
                        str_1 = global::ITS_Studio.Properties.Resources.PasswordErr_Cn;
                        str_2 = global::ITS_Studio.Properties.Resources.PasswordTip_Cn;
                    }
                    MyUIPage.ShowInfoDialog(str_1, string.Format(str_2, value), UIStyle.Orange, false);
                    return;
                }
            }

            SensorTest_Multi_init(enProfile.Write);
        }

        public bool UpdateProfile(enProfile _Opt)
        {
            INI_Tool MY_INI = new INI_Tool(ITS_Directory.History_INI);
            string AbsolutePath = "";
            if(_Opt == enProfile.Read)
            {
                string strProfilePath = MY_INI.IniReadValue("SensorTest_ProfilePath", "Path", "");
                AbsolutePath = (Path.IsPathRooted(strProfilePath)) ? strProfilePath : ITS_Directory.Tool_CurrentDirectory + strProfilePath;
                lblProfileName.Text = strProfilePath;
            }
            else if(_Opt == enProfile.Write)
            {
                OpenFileDialog openDlg = new OpenFileDialog();
                openDlg.Filter = "Profile (*.ini)|*.ini";  //在對話方塊中可以使用檔案篩選選項
                openDlg.InitialDirectory = ITS_Directory.Tool_CurrentDirectory + "Profile";//執行檔資料夾的位置
                openDlg.FileName = "";                     //取得或設定含有檔案對話方塊中所選取檔名的字串
                openDlg.CheckFileExists = true;      //如果當使用者指定不存在的檔名時對話方塊顯示警告訊息，屬性值為 true
                openDlg.CheckPathExists = true;    //如果當使用者指定了不存在的路徑時要顯示警告對話方塊，屬性值為 true
                openDlg.Multiselect = false;              //如果對話方塊允許一起或同時選取多個檔案，則為 true
                if(openDlg.ShowDialog(this) != DialogResult.OK)
                    return true;

                AbsolutePath = openDlg.FileName;
                string RelativePath = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, openDlg.FileName);
                MY_INI.IniWriteValueW("SensorTest_ProfilePath", "Path", RelativePath);
                lblProfileName.Text = RelativePath;
            }


            if(!File.Exists(AbsolutePath))
                return false;

            string INI_ProfileVer = "";
            if(!CheckProfileVersion(AbsolutePath, out INI_ProfileVer))
                return false;

            string strProtocol = "";
            for(int iDevIndex = 0; iDevIndex < StaticVar.iMaxSupportDev; iDevIndex++)
            {
                if(UI_GBV.mDev[iDevIndex].LocalVariable.Var_Multi.DevPanelInfo == null)
                    UI_GBV.mDev[iDevIndex].LocalVariable.Var_Multi.DevPanelInfo = new Panel_Info();

                if(!UI_GBV.mDev[iDevIndex].m_GBV._SensorTestTool_.bReadProtocol(AbsolutePath, out strProtocol, out UI_GBV.mDev[iDevIndex].LocalVariable.Var_Multi.DevPanelInfo.MCU))
                    return false;

                UI_GBV.mDev[iDevIndex].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath = AbsolutePath;
                if(!UI_GBV.fmITS_Tool.ResetProtocol(iDevIndex, strProtocol))
                    return false;
            }
            return true;
        }

        private bool CheckProfileVersion(string strProfileAbsolutePath, out string INI_Profile)
        {
            INI_Tool MyINI = new INI_Tool(strProfileAbsolutePath);
            INI_Profile = MyINI.IniReadValue("System", "ProfileVersion", "");
            Version INI_ProfileVer = new Version(INI_Profile);
            return INI_ProfileVer == VersionInfo.ProfileVersion;
        }

        private bool SensorTest_Multi_init(enProfile _enProfile)
        {
            //讀取Profile
            if(!UpdateProfile(_enProfile))
            {
                btn_TestAll.Enabled = false;
                return false;
            }
            
            //全部元件初始化
            for(int iDevIndex = 0; iDevIndex < StaticVar.iMaxSupportDev; iDevIndex++)
            {
                TestDev[iDevIndex] = new stuDev(MultiSenTest_VAR.TestResult.NA, new Device_Port_Binding("", "", "", new List<string>()), false, new object());

                //t_BarcodeCheck[iDevIndex] = new NamedTimer(iDevIndex);
                //t_BarcodeCheck[iDevIndex].Enabled = false;
                //t_BarcodeCheck[iDevIndex].Interval = 100;
                //t_BarcodeCheck[iDevIndex].Elapsed += new System.Timers.ElapsedEventHandler(t_BarcodeAutoMode_Tick);
                //t_BarcodeCheck[iDevIndex].AutoReset = true;
                //t_BarcodeCheck[iDevIndex].SynchronizingObject = this;

                enReadFrom ReadFrom = (iDevIndex == 0) ? enReadFrom.INI : enReadFrom.Memory;
                UI_GBV.mDev[iDevIndex].m_GBV._SensorTestTool_.ResetAllTestContains(ReadFrom);

                TestSequenceInit(iDevIndex);
                //if(m_TriggerOptions == TriggerMode.BarCode)
                //    BarCodeTest_En(iDevIndex, true);
            }

            LoadSystemSettingINI();
            
            //假如Profile的測試開啟了Barcode、GPIO Trigger，都要提示不支援
            if(m_TriggerOptions != TriggerMode.Button)
                return false;

            //if(m_TriggerOptions == TriggerMode.BarCode)
            //{
            //    t_BarCodePolling = new NamedTimer("BarCodePolling");
            //    t_BarCodePolling.Enabled = false;
            //    t_BarCodePolling.Interval = 100;
            //    t_BarCodePolling.Elapsed += new System.Timers.ElapsedEventHandler(t_BarcodePolling_Tick);
            //    t_BarCodePolling.AutoReset = true;
            //    t_BarCodePolling.SynchronizingObject = this;
            //}
            //else if(m_TriggerOptions == TriggerMode.GPIO)
            //{
            //    string[] ts2 = UI_GBV.mDev[0].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioTS2.ToString()).Split(',');
            //    string[] ts1 = UI_GBV.mDev[0].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioTS1.ToString()).Split(',');
            //    string[] pn0 = UI_GBV.mDev[0].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", enOP_Mode.BridgeGpioPN0.ToString()).Split(',');

            //    StaticVar.gpio_check_list.Clear();

            //    if(ts2.Length == 2)
            //    {
            //        StaticVar.gpio_check_list.Add(enBridge_Pin.TS_2, new Tuple<enBridgeGpioUsage, GPIO_PIN_TYPE>(
            //            EnumTool.GetValueFromDescription<enBridgeGpioUsage>(ts2[0]),
            //            (ts2[1] == enBridgeGpioTrigger.high.Desc()) ? GPIO_PIN_TYPE.HIGH : GPIO_PIN_TYPE.LOW));
            //    }

            //    if(ts1.Length == 2)
            //    {
            //        StaticVar.gpio_check_list.Add(enBridge_Pin.TS_1, new Tuple<enBridgeGpioUsage, GPIO_PIN_TYPE>(
            //            EnumTool.GetValueFromDescription<enBridgeGpioUsage>(ts1[0]),
            //            (ts1[1] == enBridgeGpioTrigger.high.Desc()) ? GPIO_PIN_TYPE.HIGH : GPIO_PIN_TYPE.LOW));
            //    }

            //    if(pn0.Length == 2)
            //    {
            //        StaticVar.gpio_check_list.Add(enBridge_Pin.PN_0, new Tuple<enBridgeGpioUsage, GPIO_PIN_TYPE>(
            //            EnumTool.GetValueFromDescription<enBridgeGpioUsage>(pn0[0]),
            //            (pn0[1] == enBridgeGpioTrigger.high.Desc()) ? GPIO_PIN_TYPE.HIGH : GPIO_PIN_TYPE.LOW));
            //    }
            //}
            //統計欄初始化
            TestStatistics_Reset();

            //主畫面初始化
            ClearGridView_Main();

            //確認裝置有多少隻
            AddTestDev();
            return true;
        }

        public bool SensorTest_Multi_Main()
        {
            return SensorTest_Multi_init(enProfile.Read);
        }

        private void GridViewBeginEdit(DataGridViewRow row)
        {
            var tmp = TableHeaderName.Column_Trigger.Desc();
            if(!GridView_Main.Focused)
                GridView_Main.Focus();
            GridView_Main.CurrentCell = row.Cells[tmp];

            GridView_Main.BeginEdit(true);
            if(GridView_Main.CurrentCell.IsInEditMode)
                TestDev[row.Index].IsInEditMode = true;
        }

        //private void t_BarcodePolling_Tick(object sender, EventArgs e)
        //{
        //    t_BarCodePolling.Stop();
        //    //Console.WriteLine("[t_BarcodePolling_Tick] t_BarCodePolling.Stop");
        //    var _Column_Trigger = TableHeaderName.Column_Trigger.Desc();
        //    foreach(var Row in GridView_Main.Rows.Cast<DataGridViewRow>())
        //    {
        //        var iRow = Row.Index;
        //        //Console.WriteLine("t_BarcodePolling_Tick {0} Judge ReadOnly", iRow);
        //        if(Row.Cells[_Column_Trigger].ReadOnly == true)
        //            continue;

        //        //Console.WriteLine("t_BarcodePolling_Tick {0} Testing", iRow);
        //        if(BarcodeScaned[iRow] == MultiSenTest_VAR.TestResult.Testing)
        //            continue;

        //        if(BarcodeScaned[iRow] == MultiSenTest_VAR.TestResult.NT)
        //        {
        //            //Console.WriteLine("iRow {0} = {1}", iRow, BarcodeScaned[iRow]);
        //            while(!TestDev[iRow].IsInEditMode)
        //            {
        //                Application.DoEvents();
        //                GridViewBeginEdit(Row);
        //            }
        //        }

        //        break;
        //    }
        //    //if (BarcodeScaned.FindAll(x => x == MultiSenTest_VAR.TestResult.NT).Count == 0)
        //    var All_Alive = BarcodeScaned.FindAll(x => x != MultiSenTest_VAR.TestResult.NA);
        //    if(All_Alive.FindAll(x => x == MultiSenTest_VAR.TestResult.NT).Count == 0)
        //    {
        //        for(int i = 0; i < BarcodeScaned.Count; i++)
        //        {
        //            if(BarcodeScaned[i] == MultiSenTest_VAR.TestResult.NA)
        //                continue;
        //            BarcodeScaned[i] = MultiSenTest_VAR.TestResult.NT;
        //        }
        //    }
        //}

        private void ClearGridView_Main()
        {
            GridView_Main.SuspendLayout();
            GridView_Main.ClearAll();
            //========================================
            //新增測項內容
            List<DataColumn> _DataColumn = new List<DataColumn>();
            var tmpList = new List<string>();

            //第一行
            _DataColumn.Add(new DataColumn(TableHeaderName.Column_PortChain.Desc(), typeof(string)));
            tmpList.Add(TableHeaderName.Column_PortChain.Desc());

            //測項行
            foreach(var item in UI_GBV.mDev[0].m_GBV._SensorTestTool_.ArrTestResult)
            {
                if(!item.Value.bEnable)
                    continue;
                if(item.Key == TestItemNameDefine.enPaintingTest)
                    continue;
                if(item.Key == TestItemNameDefine.enFrequencyScan)
                    continue;
                if(item.Key == TestItemNameDefine.enChargeCurveTest)
                    continue;
                _DataColumn.Add(new DataColumn(ReturnTestName(item.Key), typeof(string)));
                tmpList.Add(ReturnTestName(item.Key));
            }

            //最後一行
            _DataColumn.Add(new DataColumn(TableHeaderName.Column_Report.Desc(), typeof(string)));
            tmpList.Add(TableHeaderName.Column_Report.Desc());

            DataTable m_MyDT = new DataTable();
            m_MyDT.Columns.AddRange(_DataColumn.ToArray());
            GridView_Main.DataSource = m_MyDT;

            //========================================
            //設計每一個column的屬性
            foreach(var name in tmpList)
            {
                GridView_Main.Columns[name].Frozen = false;
                GridView_Main.Columns[name].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                GridView_Main.Columns[name].ReadOnly = true;
                if(name == TableHeaderName.Column_Report.Desc())
                {
                    using(Font font = new Font(GridView_Main.DefaultCellStyle.Font.FontFamily, 14, FontStyle.Bold | FontStyle.Underline))
                        GridView_Main.Columns[name].HeaderCell.Style.Font = font;
                }
            }

            //========================================
            //取決於測試的功能是button還是Barcode
            string strTriggerOption = UI_GBV.mDev[0].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "TriggerOption");
            if(strTriggerOption == TriggerMode.BarCode.Desc())
            {
                DataGridViewTextBoxColumn barcode = new DataGridViewTextBoxColumn();
                barcode.Name = TableHeaderName.Column_Trigger.Desc();
                GridView_Main.Columns.Insert((int)TableHeaderName.Column_Trigger, barcode);
                GridView_Main.Columns[barcode.Name].Frozen = true;
                GridView_Main.Columns[barcode.Name].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                GridView_Main.Columns[barcode.Name].ReadOnly = false;
                GridView_Main.Columns[barcode.Name].Width = 200;
                GridView_Main.Columns[barcode.Name].Name = barcode.Name;
            }
            else
            {
                DataGridViewButtonColumn btn_Test = new DataGridViewButtonColumn();
                btn_Test.Name = TableHeaderName.Column_Trigger.Desc();
                btn_Test.Text = "Start Test";
                btn_Test.UseColumnTextForButtonValue = true;
                GridView_Main.Columns.Insert((int)TableHeaderName.Column_Trigger, btn_Test);
                GridView_Main.Columns[btn_Test.Name].Frozen = true;
                GridView_Main.Columns[btn_Test.Name].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                GridView_Main.Columns[btn_Test.Name].ReadOnly = false;
                GridView_Main.Columns[btn_Test.Name].Name = btn_Test.Name;
            }

            DataGridViewProgressColumn ProcessColumn = new DataGridViewProgressColumn();
            ProcessColumn.Name = TableHeaderName.Column_Process.Desc();
            GridView_Main.Columns.Insert((int)TableHeaderName.Column_Process, ProcessColumn);
            GridView_Main.Columns[ProcessColumn.Name].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            GridView_Main.Columns[ProcessColumn.Name].ReadOnly = true;
            GridView_Main.Columns[ProcessColumn.Name].Name = ProcessColumn.Name;

            for(var col = 0; col < GridView_Main.ColumnCount; col++)
                GridView_Main.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
            GridView_Main.ResumeLayout();
        }

        public void GpioScanEvent(stuDev dev)
        {
            if(dev._CanceToken == null)
                return;
            while(!dev._CanceToken.IsCancellationRequested)
            {
                this.Invoke(new Action(() =>
                    {                        
                        MyTool.Delay_ms(100);

                        if(dev._CanceToken.IsCancellationRequested)
                            return;
                        if(StaticVar.lsMain_USBDevInfo[dev.idx].NowDevStatus == DevStatus.DeviceRemove)
                            return;

                        var _Select = StaticVar.gpio_check_list.First(x => x.Value.Item1 == enBridgeGpioUsage.START_PIN);
                        if(!UI_GBV.mDev[dev.idx].m_GBV.Globe_Multi.m_Method_CMD.get_bridge_pin(_Select.Key, _Select.Value.Item2))
                            return;

                        if(dev._CanceToken.IsCancellationRequested)
                            return;
                        
                        dev._CanceToken.Cancel();
                        TestFunc(dev.PortInfo.Port_Chain);
                    }));
            };
            //System.Diagnostics.Debug.WriteLine("{0} / {1}", matched, total);
            //Console.WriteLine("{0} / {1}", matched, total);
        }
        
        private void RecoverGPIO_Pin(int DevID)
        {
            /* set initial state of each gpio output */
            foreach(var item in StaticVar.gpio_check_list)
            {
                switch(item.Value.Item1)
                {
                    case enBridgeGpioUsage.PASS_PIN:
                    case enBridgeGpioUsage.FAIL_PIN:
                    case enBridgeGpioUsage.START_PIN:
                        if(item.Value.Item2 == GPIO_PIN_TYPE.HIGH)
                            UI_GBV.mDev[DevID].m_GBV.Globe_Multi.m_Method_CMD.set_bridge_pin(item.Key, GPIO_PIN_TYPE.LOW);
                        else if(item.Value.Item2 == GPIO_PIN_TYPE.LOW)
                            UI_GBV.mDev[DevID].m_GBV.Globe_Multi.m_Method_CMD.set_bridge_pin(item.Key, GPIO_PIN_TYPE.HIGH);
                        break;
                }
            }
        }
        
        private void AddTestDev()
        {
            UpdateDeviceList();
            var str = TableHeaderName.Column_Process.Desc();
            
            var AllDev = StaticVar.lsMain_USBDevInfo.ToList().FindAll(x => x.UI_Idx != -1);
            foreach(var dev in AllDev)
            {
                //如果不包含
                if(TestDev.ToList().FindIndex(x => x.PortInfo.Equals(dev.Port_Info)) != -1)
                    continue;
                if(dev.UI_Idx == -1)
                    continue;

                TestDev[dev.UI_Idx].idx = dev.UI_Idx;

                TestDev[dev.UI_Idx].PortInfo = dev.Port_Info;
                TestDev[dev.UI_Idx].Status = MultiSenTest_VAR.TestResult.NT;
                TestDev[dev.UI_Idx].IsInEditMode = false;
                TestDev[dev.UI_Idx].progressBar = GridView_Main.Rows[dev.UI_Idx].Cells[str];
                //Console.WriteLine("iDev_{0} = {1}", dev.UI_Idx, TestDev[dev.UI_Idx].Status);

                if(dev.NowDevStatus == DevStatus.DeviceRemove)
                    SetRowsTestEn(dev.UI_Idx, false);
                var DEV_ID = TestDev[dev.UI_Idx].idx;
                /* start Gpio timer */
                if(m_TriggerOptions == TriggerMode.GPIO)
                {
                    Console.WriteLine("[init_bridge_pin] mDev:{0} UI_Idx:{1}", DEV_ID, dev.UI_Idx);
                    UI_GBV.mDev[DEV_ID].m_GBV.Globe_Multi.m_Method_CMD.init_bridge_pin(
                        (StaticVar.gpio_check_list[enBridge_Pin.TS_2].Item1 == enBridgeGpioUsage.START_PIN) ? true : false,
                        (StaticVar.gpio_check_list[enBridge_Pin.TS_1].Item1 == enBridgeGpioUsage.START_PIN) ? true : false,
                        (StaticVar.gpio_check_list[enBridge_Pin.PN_0].Item1 == enBridgeGpioUsage.START_PIN) ? true : false);
                    RecoverGPIO_Pin(DEV_ID);
                   
                    UI_GBV.mDev[DEV_ID].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();
                    //MyTool.Delay_ms(1000);//如果沒有Delay，上電時間過程中，TimerTick觸發的時間點會問到Low的答案，導致錯誤觸發測試。
                    
                    TestDev[dev.UI_Idx]._CanceToken = new CancellationTokenSource();
                    TestDev[dev.UI_Idx].timer_GpioScan = Task.Factory.StartNew(() =>
                        GpioScanEvent(TestDev[dev.UI_Idx]), TestDev[dev.UI_Idx]._CanceToken.Token);
                }
            }            
        }

        private void UpdateDeviceList()
        {
            DataTable_Interchange DT_Tool = new DataTable_Interchange();

            DataTable m_MyDT = (DataTable)(GridView_Main.DataSource);

            DataColumn[] columns = m_MyDT.Columns.Cast<DataColumn>().ToArray();
            //var _List_1 = StaticVar.lsMain_USBDevInfo.ToList().FindAll(x => x.Dev_Item_Tag != null);
            var _List_1 = StaticVar.lsMain_USBDevInfo.ToList().FindAll(x => x.UI_Idx != -1);
            var _List = _List_1.FindAll(x => !string.IsNullOrEmpty(x.Port_Info.Port_Chain));
            foreach(var Dev in _List)
            {
                bool anyFieldContainsPepsi = m_MyDT.AsEnumerable().Any(row => columns.Any(col => row[col].ToString() == Dev.Port_Info.Port_Chain));
                if(anyFieldContainsPepsi)
                    continue;
                DataRow _row_ = m_MyDT.NewRow();

                for(int col = 0; col < m_MyDT.Columns.Count; col++)
                {
                    switch(col)
                    {
                        case 0:
                            _row_[col] = Dev.Port_Info.Port_Chain;
                            break;

                        default:
                            _row_[col] = (col == m_MyDT.Columns.Count - 1) ? "" : "Ready";
                            break;

                    }
                }
                int i = StaticVar.lsMain_USBDevInfo.ToList().FindIndex(x => x.Port_Info.Port_Chain == Dev.Port_Info.Port_Chain);
                StaticVar.lsMain_USBDevInfo[i].UI_Idx = m_MyDT.Rows.Count;
                m_MyDT.Rows.Add(_row_);
                BarcodeScaned.Add(MultiSenTest_VAR.TestResult.NT);
            }
            GridView_Main.DataSource = m_MyDT;
        }

        private void t_BarcodeAutoMode_Tick(object sender, EventArgs e)
        {
            //var _Time = sender as NamedTimer;
            //var index = _Time.Index;

            //if (((TestDev[index].Txt_QRCode.TextLength) == m_iLength) && (m_iLength != 0))
            //{
            //    t_BarCodeAutoMode[index].Enabled = false;
            //    TestDev[index].Txt_QRCode.Enabled = false;
            //    //var _value = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.lsMainSetting.Find(x => x.strKey == "BarcodeWaitForTest").strValue;
            //    //var _bValue = true;
            //    //bool.TryParse(_value, out _bValue);
            //    //if (_bValue)
            //    //    WarningFormShow("Tip", "Press Enter");
            //    //EnterBarcodeTest(TestDev[index].Txt_QRCode);

            //    //this.BeginInvoke(new Action(() => { EnterBarcodeTest(TestDev[index].Txt_QRCode); }), null);
            //    EnterBarcodeTest(TestDev[index].Txt_QRCode);
            //    TestDev[index].Txt_QRCode.Focus();
            //    TestDev[index].Txt_QRCode.Clear();
            //}
            //else
            //{
            //    TestDev[index].Txt_QRCode.Focus();
            //    TestDev[index].Txt_QRCode.Clear();
            //}
        }

        private void t_NowClock_Tick(object sender, EventArgs e)
        {
            lbNowClock.Text = DateTime.Now.ToString("HH:mm:ss");
            if(TestDev.ToList().FindAll(x => x.Status == MultiSenTest_VAR.TestResult.Testing).Count > 0)
            {
                if(btn_TestAll.Enabled)
                    btn_TestAll.Enabled = !btn_TestAll.Enabled;
            }
            else
            {
                if(!btn_TestAll.Enabled)
                    btn_TestAll.Enabled = !btn_TestAll.Enabled;
            }
        }

        private void TestFunc(string PortChain)
        {
            DataGridViewRow row = GridView_Main.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[TableHeaderName.Column_PortChain.Desc()].Value.ToString().Equals(PortChain)).First();
            var iRow = row.Index;
            if(GridView_Main.Rows[iRow].Cells[TableHeaderName.Column_Trigger.Desc()].ReadOnly == true)
                return;
            if(m_TriggerOptions == TriggerMode.BarCode)
                BarcodeScaned[iRow] = MultiSenTest_VAR.TestResult.Testing;
            //GridView_Main.Rows[iRow].DefaultCellStyle.BackColor = Color.White;
            GridView_Main.Rows[iRow].DefaultCellStyle.BackColor = (iRow % 2 == 0) ? GridView_Main.StripeEvenColor : GridView_Main.StripeOddColor;

            for(int icol = 0; icol < GridView_Main.ColumnCount; icol++)
            {
                if(icol < 3)
                    continue;
                GridView_Main.Rows[iRow].Cells[icol].Style.BackColor = (iRow % 2 == 0) ? GridView_Main.StripeEvenColor : GridView_Main.StripeOddColor;
                GridView_Main.Rows[iRow].Cells[icol].Value = "";
            }

            var iBuffIdx = StaticVar.lsMain_USBDevInfo.ToList().FindIndex(x => x.Port_Info.Port_Chain == PortChain);
            if(iBuffIdx < 0)
                return;

            var iUiIdx = TestDev.ToList().FindIndex(x => x.PortInfo.Port_Chain == PortChain);
            if(iUiIdx < 0)
                return;
            TestDev[iUiIdx].Status = MultiSenTest_VAR.TestResult.Testing;
            SetRowsTestStatus(iRow, true);
            //Console.WriteLine("TestFunc() iDev_{0} = {1}", iUiIdx, TestDev[iUiIdx].Status);

            UI_GBV.mDev[iBuffIdx].LocalVariable.VarSenTest_Multi._GridView_Cell.strTotleReult_ErrCode = "";

            //====================================
            if(UI_GBV.mDev[iBuffIdx].m_GBV.Globe_Multi.ThreadFun != null)
                if((UI_GBV.mDev[iBuffIdx].m_GBV.Globe_Multi.ThreadFun.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) == 0)
                    return;

            //====================================
            // Reload INI
            UI_GBV.mDev[iBuffIdx].m_GBV._SensorTestTool_.LoadTestEnable(enReadFrom.INI);

            int error = UI_GBV.mDev[iBuffIdx].m_GBV.Globe_Multi.m_Flow.commonflow_add_dev(string.Format("{0}", iBuffIdx));
            if (error < 0)
                return;

            // Start Test
            UI_GBV.mDev[iBuffIdx].m_GBV._SensorTestTool_.MainEntry(enSensorTest.MultiMode);

            //Enter TestMode
            UI_GBV.mDev[iBuffIdx].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_SenTest_ModeChange, (IntPtr)((int)TouchMode.TestMode), string.Format("{0},{1}", iBuffIdx, TouchMode_UI.UI_Only));

        }

        private void ButtonTest()
        {
            foreach(var Row in GridView_Main.Rows.Cast<DataGridViewRow>())
            {
                if(GridView_Main.Rows[Row.Index].Cells[TableHeaderName.Column_Trigger.Desc()].ReadOnly == true)
                    continue;
                var Column_PortChain = TableHeaderName.Column_PortChain.Desc();
                var UI_PortChain = Row.Cells[Column_PortChain].Value.ToString();
                var Dev = TestDev.First(x => x.PortInfo.Port_Chain == UI_PortChain);
                if(Dev.Status == MultiSenTest_VAR.TestResult.Testing)
                    continue;
                TestFunc(UI_PortChain);
            }
        }

        public void btnAll_Test_Click(object sender, EventArgs e)
        {
            if(m_TriggerOptions == TriggerMode.Button)
            {
                ButtonTest();
            }
            //else if(m_TriggerOptions == TriggerMode.BarCode)
            //{
            //    if(btn_TestAll.Text.Contains("Stop"))
            //    {
            //        t_BarCodePolling.Stop();
            //        btn_TestAll.Text = "Start All";
            //    }
            //    else
            //    {
            //        t_BarCodePolling.Start();//按下開始之後
            //        btn_TestAll.Text = "Stop All";
            //    }
            //}
        }

        public void TestStatistics_Reset()
        {
            var option = new UIPieOption();

            //Title
            option.Title = new UITitle();
            option.Title.Text = "Fail Distribution";
            //option.Title.SubText = "Distribution";
            //option.Title.Left = UILeftAlignment.Center;

            //ToolTip
            option.ToolTip.Visible = true;

            //Legend
            option.Legend = new UILegend();
            option.Legend.Orient = UIOrient.Vertical;
            option.Legend.Top = UITopAlignment.Top;
            option.Legend.Left = UILeftAlignment.Left;

            //Series
            var series = new UIPieSeries();
            series.Name = "FailCount";
            series.Center = new UICenter(50, 55);
            series.Radius = 60;
            series.Label.Show = true;

            //增加數據
            FailCount = new int[UI_GBV.mDev[0].m_GBV._SensorTestTool_.ArrTestResult.Count];
            foreach(var item in UI_GBV.mDev[0].m_GBV._SensorTestTool_.ArrTestResult)
            {
                if((item.Key == TestItemNameDefine.enPaintingTest) || (item.Key == TestItemNameDefine.enP2PTest))
                {
                    FailCount[(int)item.Key] = 0;
                    continue;
                }
                if(!item.Value.bEnable)
                    continue;
                string TestName = item.Key.DescArr(':')[1];
                option.Legend.AddData(TestName);
                series.AddData(TestName, 0);
            }

            //增加Series
            option.Series.Clear();
            option.Series.Add(series);

            //设置Option
            uiPieChart1.SetOption(option);
            userControl_TestStatistics1.Clear();
            userControl_TestStatistics1.Show_StatisticsInfo();

            CalResult = new List<KeyValuePair<enCalResult, int>>();
            foreach(var item in EnumTool.EnumToList<enCalResult>())
                CalResult.Add(new KeyValuePair<enCalResult, int>(item, 0));
        }

        private void UpdateChart(int iSelectedDev)
        {
            if(!string.IsNullOrEmpty(UI_GBV.mDev[iSelectedDev].LocalVariable.VarSenTest_Multi._GridView_Cell.strTotleReult_ErrCode))
            {
                FailCount[(int)TestItemNameDefine.enPanel_Info]++;
                string TestName = TestItemNameDefine.enPanel_Info.DescArr(':')[1];
                uiPieChart1.Update("FailCount", TestName, FailCount[(int)TestItemNameDefine.enPanel_Info]);
            }
            else
            {
                foreach(var item in UI_GBV.mDev[iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult)
                {
                    if(!item.Value.bEnable)
                        continue;
                    if(item.Value.iFailCnts <= 0)
                        continue;
                    FailCount[(int)item.Key]++;
                    string TestName = item.Key.DescArr(':')[1];
                    uiPieChart1.Update("FailCount", TestName, FailCount[(int)item.Key]);
                }
            }
            uiPieChart1.Refresh();
        }

        private string ReturnTestName(TestItemNameDefine TestItem)
        {
            return TestItem.DescArr(':')[1];
        }

        //測試順序
        public void TestSequenceInit(int iDevIndex)
        {
            UI_GBV.mDev[iDevIndex].LocalVariable.VarSenTest_Multi.DT_SensorTest.Clear();

            int iGridCount = 0;
            var SearchDic = new Dictionary<TestItemNameDefine, MultiSenTest_VAR.TestStatus>(UI_GBV.mDev[iDevIndex].m_GBV._SensorTestTool_.ArrTestResult);
            foreach(var item in SearchDic)
            {
                var NowTestitem = UI_GBV.mDev[iDevIndex].m_GBV._SensorTestTool_.ArrTestResult[item.Key];
                if(NowTestitem.strTestItemName == UI_GBV.mDev[iDevIndex].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enPaintingTest))
                    continue;

                if(item.Value.bTestitemVisible && item.Value.bEnable)
                    NowTestitem.iTestSequence = iGridCount++;

                UI_GBV.mDev[iDevIndex].m_GBV._SensorTestTool_.ArrTestResult[item.Key] = NowTestitem;
            }
        }

        private void LoadSystemSettingINI()
        {
            string strToolStart_AutoTest = UI_GBV.mDev[0].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "ToolStart_AutoTest");
            strToolStart_AutoTest = string.IsNullOrEmpty(strToolStart_AutoTest) ? "False" : strToolStart_AutoTest;

            string strTriggerOption = UI_GBV.mDev[0].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "TriggerOption");
            m_TriggerOptions = EnumTool.GetValueFromDescription<TriggerMode>(strTriggerOption);
            //lbMode.Text = string.Format("[{0}]", strTriggerOption);
            btn_TestAll.Text = string.Format("[{0}]\nStandBy", strTriggerOption);
            if(m_TriggerOptions == TriggerMode.Button)
                btn_TestAll.Image = Properties.Resources.LH1_Play_icon64_64;
            if(m_TriggerOptions == TriggerMode.BarCode)
                btn_TestAll.Image = Properties.Resources.Ecommerce_Barcode_Scanner_icon64_64;
            if(m_TriggerOptions == TriggerMode.GPIO)            
                btn_TestAll.Image = Properties.Resources.Stop_Normal_Red_icon;
            

            int.TryParse(UI_GBV.mDev[0].m_GBV._SensorTestTool_.GetMainSetValue("Report", "Barcode_Length"), out m_iBarCodeLength);

            StaticVar.bToolStart_AutoTest = Convert.ToBoolean(strToolStart_AutoTest);
            //StaticVar.bDisplayFrameData = SettingINI.IniReadBool("SensorTest", "Report_Show_FrameData", false);
            StaticVar.bReportShowResistance = SettingINI.IniReadBool("SensorTest", "Report_Show_Resistance", false);
        }
        
        public void Set_ProgressBar(int idx, ref Message msg)
        {
            string str = Marshal.PtrToStringAuto(msg.LParam);
            string strName = str.Split(':')[0];
            var Testitem = (TestItemNameDefine)Enum.Parse(typeof(TestItemNameDefine), strName);
            var TestName = Testitem.DescArr(':')[1];
            iProgressValue[idx] = (int)msg.WParam;
            if(iProgressMaximum[idx] <= 0)
                iProgressMaximum[idx] = 1;

            double dbValue = ((double)iProgressValue[idx] / iProgressMaximum[idx]) * 100;
            dbValue = dbValue > 100 ? 100 : dbValue;
            if(this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    GridView_Main.Rows[idx].Cells[(int)TableHeaderName.Column_Process].Value = (int)dbValue;
                }));
            }
            else
            {
                GridView_Main.Rows[idx].Cells[(int)TableHeaderName.Column_Process].Value = (int)dbValue;
            }

            //TestDev[idx].progressBar.Value = (int)dbValue;
            //TestDev[idx].progressBar.CustomText = TestName;
        }

        public void Set_ProgressBar_Max(ref Message msg)
        {
            int index = (int)msg.LParam;
            iProgressMaximum[index] = (int)msg.WParam;
        }

        private void Delay(int millisecondsDelay)
        {
            Thread.Sleep(millisecondsDelay);
        }

        private void TriggerGPIO_Reset(bool test_fail, int iSelectedDev)
        {
            /* bridge GPIO notify */
            //if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C &&
            //    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "TriggerOption") == EnumTool.GetEnumDescription(TriggerMode.GPIO))
            if(StaticVar.lsMain_USBDevInfo[iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C
                && m_TriggerOptions == TriggerMode.GPIO)
            {
                var _getvalue = "";
                foreach(var item in StaticVar.gpio_check_list)
                {
                    if(!test_fail)
                    {
                        if(item.Value.Item1 == enBridgeGpioUsage.PASS_PIN)
                        {
                            UI_GBV.mDev[iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.set_bridge_pin(item.Key, item.Value.Item2);
                            _getvalue = UI_GBV.mDev[iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "BridgeGpioPassDuration");
                            break;
                        }
                    }
                    else
                    {
                        if(item.Value.Item1 == enBridgeGpioUsage.FAIL_PIN)
                        {
                            UI_GBV.mDev[iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.set_bridge_pin(item.Key, item.Value.Item2);
                            _getvalue = UI_GBV.mDev[iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "BridgeGpioFailDuration");
                            break;
                        }
                    }
                }

                if(_getvalue != "0")
                {
                    Task.Factory.StartNew(() =>
                    {
                        var iValue = 0;
                        int.TryParse(_getvalue, out iValue);
                        Delay(iValue);
                        RecoverGPIO_Pin(iSelectedDev);
                    });
                }
            }
        }

        public void TestItem_All_Finish(int iSelectedDev)
        {
            //Console.WriteLine("({0}) TestItem_All_Finish", iSelectedDev);
            UI_GBV.mDev[iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(0);

            int iDev = StaticVar.lsMain_USBDevInfo[iSelectedDev].UI_Idx;
            var Port_Chain = StaticVar.lsMain_USBDevInfo[iSelectedDev].Port_Info.Port_Chain;
            DataGridViewRow row = GridView_Main.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[TableHeaderName.Column_PortChain.Desc()].Value.ToString().Equals(Port_Chain)).First();
            var iRow = row.Index;
            var Column_Report = TableHeaderName.Column_Report.Desc();
            using(Font font = new Font(GridView_Main.DefaultCellStyle.Font.FontFamily, 14, FontStyle.Bold | FontStyle.Underline))
                GridView_Main.Rows[iRow].Cells[Column_Report].Style.Font = font;
            /* bridge GPIO notify */
            int FailCnts = UI_GBV.mDev[iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult.ToList().FindAll(x => x.Value.iFailCnts > 0).Count;
            bool test_fail = (FailCnts > 0 || UI_GBV.mDev[iSelectedDev].LocalVariable.VarSenTest_Multi.iTotalFailCounts > 0);

            //SetRowsTestEn(iRow, true);
            SetRowsTestStatus(iRow, false);

            if(UI_GBV.mDev[iSelectedDev].LocalVariable.VarSenTest_Multi.bIsDevErr)
            {
                GridView_Main.Rows[iRow].Cells[Column_Report].Style.BackColor = Color.Gainsboro;
                GridView_Main.Rows[iRow].Cells[Column_Report].Value = MultiSenTest_VAR.TestResult.NA.ToString();
                TestDev[iDev].Status = MultiSenTest_VAR.TestResult.FAIL;
                return;
            }

            //Result欄位
            if(UI_GBV.mDev[iSelectedDev].LocalVariable.VarSenTest_Multi.iTotalFailCounts > 0)
            {
                // Fail
                GridView_Main.Rows[iRow].Cells[Column_Report].Style.BackColor = Color.Red;
                GridView_Main.Rows[iRow].Cells[Column_Report].Value = MultiSenTest_VAR.TestResult.FAIL.ToString();
                TestDev[iDev].Status = MultiSenTest_VAR.TestResult.FAIL;
            }
            else
            {
                if(StaticVar.lsMain_USBDevInfo[iSelectedDev].NowDevStatus != DevStatus.DeviceRemove)
                {
                    // Pass
                    GridView_Main.Rows[iRow].Cells[Column_Report].Style.BackColor = Color.LightGreen;
                    GridView_Main.Rows[iRow].Cells[Column_Report].Value = MultiSenTest_VAR.TestResult.PASS.ToString();
                    TestDev[iDev].Status = MultiSenTest_VAR.TestResult.PASS;
                }
            }

            //統計
            if(UI_GBV.mDev[iSelectedDev].LocalVariable.VarSenTest_Multi.AllTestCount.iFailCounts == 0)
            {
                if(StaticVar.lsMain_USBDevInfo[iSelectedDev].NowDevStatus != DevStatus.DeviceRemove)
                {
                    var tmp = CalResult[(int)enCalResult.Pass].Value;
                    CalResult[(int)enCalResult.Pass] = new KeyValuePair<enCalResult, int>(enCalResult.Pass, ++tmp);
                    tmp = CalResult[(int)enCalResult.Total].Value;
                    CalResult[(int)enCalResult.Total] = new KeyValuePair<enCalResult, int>(enCalResult.Total, ++tmp);
                }
            }
            else
            {
                var tmp = CalResult[(int)enCalResult.Fail].Value;
                CalResult[(int)enCalResult.Fail] = new KeyValuePair<enCalResult, int>(enCalResult.Fail, ++tmp);
                tmp = CalResult[(int)enCalResult.Total].Value;
                CalResult[(int)enCalResult.Total] = new KeyValuePair<enCalResult, int>(enCalResult.Total, ++tmp);
            }

            userControl_TestStatistics1.Set_NowValue(CalResult[(int)enCalResult.Pass].Value
                , CalResult[(int)enCalResult.Fail].Value
                , CalResult[(int)enCalResult.Total].Value);

            userControl_TestStatistics1.Show_StatisticsInfo();
            UpdateChart(iSelectedDev);

            /* restart Gpio timer */
            if(m_TriggerOptions == TriggerMode.GPIO)
            {
                MyTool.Delay_ms(100);
                if(StaticVar.lsMain_USBDevInfo[iSelectedDev].NowDevStatus == DevStatus.DeviceRemove)
                {
                    if(TestDev[iDev]._CanceToken != null)
                        TestDev[iDev]._CanceToken.Cancel();
                    return;
                }
                TriggerGPIO_Reset(test_fail, iSelectedDev);
                UI_GBV.mDev[iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor(); 
                TestDev[iDev]._CanceToken = new CancellationTokenSource();

                TestDev[iDev].timer_GpioScan = Task.Factory.StartNew(() =>
                        GpioScanEvent(TestDev[iDev]), TestDev[iDev]._CanceToken.Token);
            }
        }

        //================================================================
        public void TestDeviceNode(int MainBuff_Idx, int UI_Idx, DevOption _StopOption)
        {
            switch(_StopOption)
            {
                case DevOption.RemoveDevice:
                case DevOption.I2CErr:
                    Remove_Device(MainBuff_Idx, UI_Idx, _StopOption);
                    break;

                case DevOption.AddDevice:
                    Add_Device(MainBuff_Idx, UI_Idx);
                    break;
            }
        }

        private void Remove_Device(int MainBuff_Idx, int UI_Idx, DevOption _StopOption)
        {
            TestItem_All_Finish(MainBuff_Idx);
            if(_StopOption == DevOption.RemoveDevice)
            {
                if(UI_Idx != -1)
                {
                    TestDev[MainBuff_Idx] = new stuDev(MultiSenTest_VAR.TestResult.NA, new Device_Port_Binding("", "", "", new List<string>()), false, new object());
                    SetRowsTestEn(UI_Idx, false);
                    //Console.WriteLine("Remove_Device() UI_Idx:{0} = NA", UI_Idx);
                    BarcodeScaned[UI_Idx] = MultiSenTest_VAR.TestResult.NA;

                    if(m_TriggerOptions == TriggerMode.GPIO)
                    {
                        Console.WriteLine("[Remove_Device] mDev:{0} UI_Idx:{1}", MainBuff_Idx, UI_Idx);
                        if(TestDev[MainBuff_Idx]._CanceToken != null)
                            TestDev[MainBuff_Idx]._CanceToken.Cancel();
                        RecoverGPIO_Pin(MainBuff_Idx);
                    }
                }
            }
            //else
            //{
            //    //if (UI_GBV.mDev[MainBuff_Idx].LocalVariable.VarSenTest_Multi.bTriggerByBridge)
            //    //{
            //    //    Console.WriteLine("");
            //    //    UI_GBV.mDev[MainBuff_Idx].m_GBV.Globe_Multi.m_Flow.BridgePowerControl(Bridge._On, 100);
            //    //    UI_GBV.mDev[MainBuff_Idx].LocalVariable.VarThread_Multi.Data_Decode_Type.DECODE_TYPE = enDECODE_TYPE.InterruptIn_Other;
            //    //    UI_GBV.mDev[MainBuff_Idx].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();
            //    //}
            //}
        }

        private void Add_Device(int MainBuff_Idx, int UI_Idx)
        {
            //Console.WriteLine("Add_Device() MainBuff_Idx:{0} UI_Idx:{1}", MainBuff_Idx, UI_Idx);
            var _PortChain = StaticVar.lsMain_USBDevInfo[MainBuff_Idx].Port_Info.Port_Chain;
            bool bDevExisted = false;
            if(GridView_Main.Rows.Count > UI_Idx)
            {
                var Column_Trigger = TableHeaderName.Column_Trigger.Desc();
                if(GridView_Main.Rows[UI_Idx].Cells[Column_Trigger].ReadOnly == false)
                    bDevExisted = true;
            }

            //Console.WriteLine("MainBuff_Idx:{0} _PortChain:{1} TestDev[{2}]", MainBuff_Idx, _PortChain, UI_Idx);
            if(!string.IsNullOrEmpty(_PortChain))
            {
                AddTestDev();
                SetRowsTestEn(UI_Idx, true);
            }
            if(bDevExisted)
                return;
            string strProtocol = "";
            string MCU = "";
            if(UI_GBV.mDev[MainBuff_Idx].m_GBV._SensorTestTool_.bReadProtocol(UI_GBV.mDev[MainBuff_Idx].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath, out strProtocol, out MCU))
                if(UI_GBV.fmITS_Tool.ResetProtocol(MainBuff_Idx, strProtocol) == false)
                    return;

            UI_GBV.mDev[MainBuff_Idx].m_GBV._SensorTestTool_.ResetAllTestContains(enReadFrom.Memory);
            TestSequenceInit(UI_Idx);
        }

        private void SetRowsTestStatus(int iRow, bool bIsTesting)
        {
            var Column_Trigger = TableHeaderName.Column_Trigger.Desc();
            if(bIsTesting)
            {
                GridView_Main.Rows[iRow].DefaultCellStyle.BackColor = (iRow % 2 == 0) ? GridView_Main.StripeEvenColor : GridView_Main.StripeOddColor;
                for(int icol = 0; icol < GridView_Main.ColumnCount; icol++)
                {
                    if(icol < 3)
                        continue;
                    GridView_Main.Rows[iRow].Cells[icol].Value = "";
                }
                GridView_Main.Rows[iRow].Cells[Column_Trigger].ReadOnly = true;
                int _w = GridView_Main.Columns[Column_Trigger].Width;
                GridView_Main.Rows[iRow].Cells[Column_Trigger].Style.Padding = new Padding(_w, 0, 0, 0);
                //Console.WriteLine("========> [SetRowsTestStatus] {0} {1}", iRow, true);
            }
            else
            {
                GridView_Main.Rows[iRow].Cells[Column_Trigger].ReadOnly = false;
                GridView_Main.Rows[iRow].Cells[Column_Trigger].Style.Padding = new Padding(0, 0, 0, 0);
                //Console.WriteLine("========> [SetRowsTestStatus] {0} {1}", iRow, false);
            }
        }

        private void SetRowsTestEn(int iRow, bool _En)
        {
            GridView_Main.Rows[iRow].DefaultCellStyle.BackColor = (_En)
                ? ((iRow % 2 == 0) ? GridView_Main.StripeEvenColor : GridView_Main.StripeOddColor)
                : Color.Gainsboro;

            for(int icol = 0; icol < GridView_Main.ColumnCount; icol++)
            {
                GridView_Main.Rows[iRow].Cells[icol].Style.BackColor = (_En)
                    ? ((iRow % 2 == 0) ? GridView_Main.StripeEvenColor : GridView_Main.StripeOddColor)
                    : Color.Gainsboro;

                if(icol < 3)
                    continue;

                if(GridView_Main.Columns[icol].HeaderText != "Report")
                    GridView_Main.Rows[iRow].Cells[icol].Value = (_En) ? "Ready" : "";
                else
                    GridView_Main.Rows[iRow].Cells[icol].Value = "";
            }

            var Column_Trigger = TableHeaderName.Column_Trigger.Desc();
            GridView_Main.Rows[iRow].Cells[Column_Trigger].ReadOnly = _En ? false : true;
            Console.WriteLine("========> [SetRowsTestEn] {0} {1}", iRow, _En);
            if(_En == false)
            {
                int _w = GridView_Main.Columns[Column_Trigger].Width;
                GridView_Main.Rows[iRow].Cells[Column_Trigger].Style.Padding = new Padding(_w, 0, 0, 0);
            }
            else
            {
                GridView_Main.Rows[iRow].Cells[Column_Trigger].Style.Padding = new Padding(0, 0, 0, 0);
            }
        }

        //================================================================
        public void UpdateMainTestGrid(int iDevIndex, int iTestitem, MultiSenTest_VAR.TestResult Result)
        {
            GridView_Main.SuspendLayout();
            var Port_Chain = StaticVar.lsMain_USBDevInfo[iDevIndex].Port_Info.Port_Chain;
            if(Port_Chain == null)
                return;
            DataGridViewRow row = GridView_Main.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[TableHeaderName.Column_PortChain.Desc()].Value.ToString().Equals(Port_Chain)).First();
            var iRow = row.Index;

            if(iTestitem != -1)
            {
                switch(Result)
                {
                    case MultiSenTest_VAR.TestResult.PASS:
                        GridView_Main.Rows[iRow].Cells[iTestitem].Style.BackColor = Color.LightGreen;
                        GridView_Main.Rows[iRow].Cells[iTestitem].Value = Result.ToString();
                        break;

                    case MultiSenTest_VAR.TestResult.FAIL:
                        GridView_Main.Rows[iRow].Cells[iTestitem].Style.BackColor = Color.Red;
                        GridView_Main.Rows[iRow].Cells[iTestitem].Value = Result.ToString();
                        break;

                    case MultiSenTest_VAR.TestResult.Testing:
                        GridView_Main.Rows[iRow].Cells[iTestitem].Style.BackColor = Color.Yellow;
                        GridView_Main.Rows[iRow].Cells[iTestitem].Value = Result.ToString();
                        break;

                    default:
                        GridView_Main.Rows[iRow].Cells[iTestitem].Style.BackColor = DefaultBackColor;
                        break;
                }
            }
            //Application.DoEvents();
            GridView_Main.ResumeLayout();
        }

        //private void BarCodeTest_En(int iDevIndex, bool bEnable)
        //{
        //    if(bEnable)
        //    {
        //        int iInterval = 0;
        //        int.TryParse(UI_GBV.mDev[iDevIndex].m_GBV._SensorTestTool_.GetMainSetValue("Report", "Barcode_ScanInterval"), out iInterval);
        //        if(iInterval != 0)
        //            t_BarcodeCheck[iDevIndex].Interval = iInterval;
        //    }
        //}

        private void GridView_Main_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            var _sender = sender as UIDataGridView;
            if(_sender.IsCurrentCellDirty)
                _sender.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void GridView_Main_SelectionChanged(object sender, EventArgs e)
        {
            var _sender = sender as UIDataGridView;
            _sender.ClearSelection();
        }

        private void SetGridTestStatus(UIDataGridView GridView, DataGridViewCellEventArgs ea)
        {
            foreach(DataGridViewColumn col in GridView.Columns)
            {
                var Header = col.HeaderText;
                GridView.Rows[ea.RowIndex].Cells[Header].Style.BackColor = (ea.RowIndex % 2 == 0) ? GridView_Main.StripeEvenColor : GridView_Main.StripeOddColor;
                if(EnumTool.IsContainsDescription<TableHeaderName>(Header))
                {
                    switch(EnumTool.GetValueFromDescription<TableHeaderName>(Header))
                    {
                        case TableHeaderName.Column_Trigger:
                        case TableHeaderName.Column_PortChain:
                        case TableHeaderName.Column_Process:
                            continue;

                        case TableHeaderName.Column_Report:
                            GridView.Rows[ea.RowIndex].Cells[Header].Value = "";
                            break;

                        default:
                            GridView.Rows[ea.RowIndex].Cells[Header].Value = "Ready";
                            break;
                    }
                }
                else
                {
                    GridView.Rows[ea.RowIndex].Cells[Header].Value = "Ready";
                }
            }
        }

        private void GridView_Main_CellClick(object sender, DataGridViewCellEventArgs ea)
        {
            if((ea.RowIndex == -1) || (ea.ColumnIndex == -1))
                return;

            var GridView = sender as UIDataGridView;
            if(GridView.Rows[ea.RowIndex].Cells[ea.ColumnIndex].Style.Padding.Left != 0)
                return;

            if(!EnumTool.IsContainsDescription<TableHeaderName>(GridView.Columns[ea.ColumnIndex].Name))
                return;

            var ColHeader = EnumTool.GetValueFromDescription<TableHeaderName>(GridView.Columns[ea.ColumnIndex].Name);
            if(ColHeader == TableHeaderName.none)
                return;

            var _PortChain = TableHeaderName.Column_PortChain.Desc();
            switch(ColHeader)
            {
                case TableHeaderName.Column_Trigger:
                    //=================================================    
                    //設定測試狀態UI
                    SetGridTestStatus(GridView, ea);

                    //=================================================
                    switch(m_TriggerOptions)
                    {
                        case TriggerMode.BarCode:
                            GridView.CurrentCell = GridView.Rows[ea.RowIndex].Cells[ea.ColumnIndex];
                            GridView.BeginEdit(true);
                            break;
                        case TriggerMode.GPIO:
                        case TriggerMode.Button:
                            var PortChain = GridView.Rows[ea.RowIndex].Cells[_PortChain];
                            TestFunc(PortChain.Value.ToString());
                            break;

                        default:
                            GridView.BeginEdit(false);
                            break;
                    }
                    break;

                case TableHeaderName.Column_Report:
                    if(string.IsNullOrEmpty(GridView.Rows[ea.RowIndex].Cells[ea.ColumnIndex].Value.ToString()))
                        return;
                    var Port_Info = GridView.Rows[ea.RowIndex].Cells[_PortChain].Value.ToString();
                    var BuffIdx = StaticVar.lsMain_USBDevInfo.ToList().FindIndex(x => x.Port_Info.Port_Chain == Port_Info);
                    System.Diagnostics.Process.Start("notepad", UI_GBV.mDev[BuffIdx].LocalVariable.VarSenTest_Multi.strFinalReportPath);
                    break;
            }
        }

        private void SensorTest_Multi__Shown(object sender, EventArgs e)
        {
            m_tiNowClock.Start();
            this.uiTitlePanel1.Collapsed = true;
            //GridView_Main.Rows[0].Selected = false;
            ////先全部隱藏
            //CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[GridView_Main.DataSource];
            //currencyManager1.SuspendBinding();

            //for (int i = 0; i < GridView_Main.Rows.Count; i++)
            //    GridView_Main.Rows[i].Visible = false;
            //currencyManager1.ResumeBinding();
        }

        private void GridView_Main_EnabledChanged(object sender, EventArgs e)
        {
            var _DataGridView = sender as DataGridView;
            if(_DataGridView.Enabled)
            {
                _DataGridView.DefaultCellStyle.BackColor = SystemColors.Control;
                _DataGridView.DefaultCellStyle.ForeColor = SystemColors.GrayText;
                _DataGridView.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                _DataGridView.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.GrayText;
                //_DataGridView.CurrentCell = null;
                _DataGridView.ReadOnly = true;
                _DataGridView.EnableHeadersVisualStyles = false;
            }
            else
            {
                _DataGridView.DefaultCellStyle.BackColor = SystemColors.Window;
                _DataGridView.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                _DataGridView.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Window;
                _DataGridView.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
                //_DataGridView.CurrentCell = null;
                _DataGridView.ReadOnly = false;
                _DataGridView.EnableHeadersVisualStyles = true;
            }
        }

        private void GridView_Main_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            var Gridview = sender as DataGridView;

            if(Gridview.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
            {
                Gridview.Cursor = Cursors.Default;

                if(Gridview.Columns[e.ColumnIndex].HeaderText == TableHeaderName.Column_Report.Desc())
                {
                    if(!string.IsNullOrEmpty(Gridview.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                    {
                        Gridview.Cursor = Cursors.Hand;
                        Gridview.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.DodgerBlue;
                    }
                }
            }
            else
            {
                Gridview.Cursor = Cursors.Hand;
            }
        }

        private void GridView_Main_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            var Gridview = sender as DataGridView;
            Gridview.Cursor = Cursors.Default;
            if(Gridview.Columns[e.ColumnIndex].HeaderText == TableHeaderName.Column_Report.Desc())
            {
                var _Value = Gridview.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if(!string.IsNullOrEmpty(_Value))
                {
                    if(_Value == MultiSenTest_VAR.TestResult.PASS.ToString())
                        GridView_Main.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGreen;
                    else
                        GridView_Main.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                }
            }
        }

        /// <summary>
        /// •星號 (*)
        /// •垂直線 (|)
        /// •反斜線 (\)
        /// •冒號 (:)
        /// •雙引號 (“)
        /// •小於 (<)
        /// •大於 (>)
        /// •問號 (?)
        /// •正斜線 (/)
        /// </summary>
        /// <param name="FN"></param>
        /// <returns></returns>
        private string MakeFilenameValid(string FN)
        {
            if(FN == null)
                throw new ArgumentNullException();
            if(FN.EndsWith("."))
                FN = Regex.Replace(FN, @"\.+$", "");
            //if (FN.Length == 0) throw new ArgumentException();
            if(FN.Length > 245)
                throw new PathTooLongException();
            foreach(char c in System.IO.Path.GetInvalidFileNameChars())
                FN = FN.Replace(c, '_');
            return FN;
        }

        private void GridView_Main_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var Gridview = sender as DataGridView;
            //Console.WriteLine("GridView_Main_EditingControlShowing : {0}", Gridview.Columns[Gridview.CurrentCell.ColumnIndex].HeaderText);
            if(Gridview.Columns[Gridview.CurrentCell.ColumnIndex].HeaderText != TableHeaderName.Column_Trigger.Desc())
                return;

            m_EditingCell = Gridview.CurrentCell;

            DataGridViewTextBoxEditingControl Ctrl = (DataGridViewTextBoxEditingControl)e.Control;
            Ctrl.Leave -= new EventHandler(TextBox_Leave);
            //Gridview.CurrentCell.Value = null;
            Ctrl.Leave += new EventHandler(TextBox_Leave);
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            var Ctrl = sender as DataGridViewTextBoxEditingControl;
            Ctrl.Leave -= new EventHandler(TextBox_Leave);
        }

        private void GridView_Main_KeyUp(object sender, KeyEventArgs e)
        {
            if(((DataGridView)sender).CurrentCell == null)
                return;
            if(((DataGridView)sender).CurrentCell.ColumnIndex != (int)TableHeaderName.Column_Trigger)
                return;
            if(m_EditingCell == null)
                return;

            if(e.KeyCode == Keys.Enter)
            {
                if(m_EditingCell.Value == null)//Barcode內容為空的
                {
                    GridView_Main.Focus();
                    GridView_Main.CurrentCell = m_EditingCell;
                    GridView_Main.CurrentCell.Value = "";
                    GridView_Main.BeginEdit(true);
                }
                else if(m_iBarCodeLength != 0)//Barcode設為0代表不判斷長度
                {
                    if(m_EditingCell.Value.ToString().Length != m_iBarCodeLength)
                    {
                        GridView_Main.Focus();
                        GridView_Main.CurrentCell = m_EditingCell;
                        GridView_Main.CurrentCell.Value = "";
                        GridView_Main.BeginEdit(true);
                    }
                    else
                    {
                        string strValue = m_EditingCell.Value.ToString();
                        int UI_Idx = m_EditingCell.RowIndex;
                        int iBuffIdx = StaticVar.lsMain_USBDevInfo.ToList().FindIndex(x => x.UI_Idx == UI_Idx);
                        UI_GBV.mDev[iBuffIdx].LocalVariable.VarSenTest_Multi.strBarCodeText = MakeFilenameValid(strValue);
                        TestFunc(StaticVar.lsMain_USBDevInfo[iBuffIdx].Port_Info.Port_Chain);
                        TestDev[UI_Idx].IsInEditMode = false;
                        //if(btn_TestAll.Text.Contains("Stop"))
                        //{
                        //    t_BarCodePolling.Start();//輸入Enter鍵之後，開始掃描
                        //}
                    }
                }
                else
                {
                    //var QRcode = sender as DataGridView;
                    string strValue = m_EditingCell.Value.ToString();
                    int UI_Idx = m_EditingCell.RowIndex;
                    int iBuffIdx = StaticVar.lsMain_USBDevInfo.ToList().FindIndex(x => x.UI_Idx == UI_Idx);
                    UI_GBV.mDev[iBuffIdx].LocalVariable.VarSenTest_Multi.strBarCodeText = MakeFilenameValid(strValue);
                    //this.Invoke(new MethodInvoker(delegate() { UpdateDeviceList(); }));
                    TestFunc(StaticVar.lsMain_USBDevInfo[iBuffIdx].Port_Info.Port_Chain);
                    TestDev[UI_Idx].IsInEditMode = false;
                }
            }
        }

        private void GridView_Main_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //if(btn_TestAll.Text.Contains("Stop"))
            //{
            //    t_BarCodePolling.Stop();
            //}
        }

        private void GridView_Main_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if(btn_TestAll.Text.Contains("Stop"))
            //{
            //    t_BarCodePolling.Start();
            //}
        }

        private void GridView_Main_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void GridView_Main_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if(e.RowIndex < 0)
                return;

            //I supposed your button column is at index 0
            if(e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                if(m_TriggerOptions == TriggerMode.BarCode)
                {
                    var w = Properties.Resources.Ecommerce_Barcode_Scanner_icon.Width;
                    var h = Properties.Resources.Ecommerce_Barcode_Scanner_icon.Height;
                    var x = e.CellBounds.Left + 5;
                    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                    e.Graphics.DrawImage(ITS_Studio.Properties.Resources.Ecommerce_Barcode_Scanner_icon, new Rectangle(x, y, w, h));
                }
                else
                {
                    var w = Properties.Resources.LH1_Play_icon16_16.Width;
                    var h = Properties.Resources.LH1_Play_icon16_16.Height;
                    var x = e.CellBounds.Left + 5;
                    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                    e.Graphics.DrawImage(ITS_Studio.Properties.Resources.LH1_Play_icon16_16, new Rectangle(x, y, w, h));
                }
                e.Handled = true;
            }
        }

        private void btn_TestAll_MouseHover(object sender, EventArgs e)
        {
            //btn_TestAll.Text = string.Format("[{0}]", strTriggerOption);

            if(m_TriggerOptions == TriggerMode.Button)
            {
                btn_TestAll.Cursor = Cursors.Hand;
                btn_TestAll.Image = Properties.Resources.LH1_Play_icon128_128;
            }
            else if(m_TriggerOptions == TriggerMode.BarCode)
            {
                btn_TestAll.Cursor = Cursors.Hand;
                btn_TestAll.Image = Properties.Resources.Ecommerce_Barcode_Scanner_icon128_128;
            }
            else
            {
                btn_TestAll.Cursor = Cursors.No;
                btn_TestAll.Image = Properties.Resources.Stop_Normal_Red_icon;
            }
            Console.WriteLine("btn_TestAll_MouseHover");
        }

        private void btn_TestAll_MouseLeave(object sender, EventArgs e)
        {
            btn_TestAll_EnabledChanged(sender, e);
            Console.WriteLine("btn_TestAll_MouseLeave");
        }

        private void btn_TestAll_EnabledChanged(object sender, EventArgs e)
        {
            if(btn_TestAll.Enabled)
            {
                if(m_TriggerOptions == TriggerMode.Button)
                    btn_TestAll.Image = Properties.Resources.LH1_Play_icon64_64;
                else if(m_TriggerOptions == TriggerMode.BarCode)
                    btn_TestAll.Image = Properties.Resources.Ecommerce_Barcode_Scanner_icon64_64;
                else
                    btn_TestAll.Image = Properties.Resources.Stop_Normal_Red_icon;
            }
            else
            {
                btn_TestAll.Image = Properties.Resources.Stop_Normal_Red_icon;
            }
        }

        #region Socket & PIPE
        private StreamStringClass _StreamStr;
        private BackgroundWorker PipeBgw;
        private ClassSocket MyServerSocket = new ClassSocket();
        private Socket tmpMyServerSocket;

        private void SensorTest_Multi__Load(object sender, EventArgs e)
        {
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

        private void SendToSocket(object parms)
        {
            if(parms.GetType().Equals(typeof(string)))
            {
                MyServerSocket.AsyncSendMessage(tmpMyServerSocket, (string)parms);
            }
        }

        private void MethodInvokerFunc(string cmdstr)
        {
            try
            {
                //var arr = cmdstr.Split(',');                
                //DEV{n},{Barcode}; => DEVICE 1,BARCODE xxxxx
                //or DEV{n}; => DEVICE 1
                var _PortChain = TableHeaderName.Column_PortChain.Desc();
                //int iRowIndex = -1;
                string ErrStr = "";

                var parms = cmdstr.Split(' ');
                if(!Enum.IsDefined(typeof(enITS_Params), parms[0]))
                    return;

                //switch(EnumTool.ParseEnum<enITS_Params>(parms[0]))
                //{
                //    case enITS_Params.DEVICE:
                string strCMD = "";
                if(Enum.IsDefined(typeof(enCMD_Type), parms[2]))
                {
                    switch(EnumTool.ParseEnum<enCMD_Type>(parms[2]))
                    {
                        case enCMD_Type.Connect_Status:
                            strCMD = ConnectStatus_Func(parms);
                            break;
                        case enCMD_Type.Power_Off:
                            strCMD = BridgePowerOff_Func(parms);
                            break;
                        case enCMD_Type.Start_Test:
                            strCMD = StartTest_Func(parms);
                            break;
                    }
                    if(StaticVar.EnClientMode == enITS_WakeUpMode.SOCKET_MODE)
                    {
                        SendToSocket(strCMD);
                    }
                    else if(StaticVar.EnClientMode == enITS_WakeUpMode.PIPE_MODE)
                    {
                        if(_StreamStr.WriteString(strCMD, out ErrStr) == -1)
                            MyUIPage.ShowErrorDialog("Pipe 錯誤提示", ErrStr, UIStyle.Red, false);
                    }
                }
                //        break;

                //    case enITS_Params.BARCODE:
                //        MyUIPage.ShowInfoTip(string.Format("[Multi] {0}", parms[1]), 1000, false);

                //        iRowIndex = int.Parse(parms[1]);
                //        var PortChain = GridView_Main.Rows[iRowIndex].Cells[_PortChain];
                //        if(_StreamStr.WriteString(string.Format("(bgw_DoWork) PortChain : {0}", PortChain.Value), out ErrStr) == -1)
                //            MyUIPage.ShowErrorDialog("Pipe 錯誤提示", ErrStr, UIStyle.Red, false);
                //        TestFunc(PortChain.Value.ToString());
                //        break;

                //}

            }
            catch(Exception ex)
            {
                MyUIPage.ShowErrorTip(ex.Message, 2000, false);
            }
        }

        private string BridgePowerOff_Func(string[] parms)
        {
            var Device = parms[1].Split(';');
            string strSend = enITS_Params.DEVICE.ToString() + ";" + parms[2] + ";";
            foreach(var dev in Device)
            {
                if(string.IsNullOrEmpty(dev))
                    continue;
                Match rxMatch = Regex.Match(dev, @"^DEV([0-9]+)");
                int iRowIndex = int.Parse(rxMatch.Groups[1].Value) - 1;
                //var _CellPos = EnumTool.GetEnumDescription(TableHeaderName.Column_PortChain);
                //var PortChain = GridView_Main.Rows[iRowIndex].Cells[_CellPos];

                //_CellPos = EnumTool.GetEnumDescription(TableHeaderName.Column_Trigger);
                strSend += string.Format("{0},DONE;", rxMatch.Groups[1].Value);
                //strSend += string.Format("{0};", GridView_Main.Rows[iRowIndex].Cells[_CellPos].ReadOnly ? "ERR" : "DONE");

                //if(!GridView_Main.Rows[iRowIndex].Cells[_CellPos].ReadOnly)
                UI_GBV.mDev[iRowIndex].m_GBV.Globe_Multi.m_Flow.BridgePowerControl(Bridge._Off, 100);
                //TestFunc(PortChain.Value.ToString());
            }
            return strSend;

        }

        private string StartTest_Func(string[] parms)
        {
            var Device = parms[1].Split(';');
            string strSend = enITS_Params.DEVICE.ToString() + ";" + parms[2] + ";";
            foreach(var dev in Device)
            {
                if(string.IsNullOrEmpty(dev))
                    continue;
                Match rxMatch = Regex.Match(dev, @"^DEV([0-9]+)");
                int iRowIndex = int.Parse(rxMatch.Groups[1].Value) - 1;
                var _CellPos = TableHeaderName.Column_PortChain.Desc();
                var PortChain = GridView_Main.Rows[iRowIndex].Cells[_CellPos];

                _CellPos = TableHeaderName.Column_Trigger.Desc();
                strSend += string.Format("DEV{0},", rxMatch.Groups[1].Value);
                strSend += string.Format("{0};", GridView_Main.Rows[iRowIndex].Cells[_CellPos].ReadOnly ? "ERR" : "DONE");

                if(!GridView_Main.Rows[iRowIndex].Cells[_CellPos].ReadOnly)
                    TestFunc(PortChain.Value.ToString());
            }
            return strSend;
        }

        private string ConnectStatus_Func(string[] parms)
        {
            string strSend = enITS_Params.DEVICE.ToString() + ";" + parms[2] + ";";

            if(Regex.IsMatch(parms[1], @"^[0-9]+$"))
            {
                int iRowIndex = int.Parse(parms[1]);
                //var _PortChain = EnumTool.GetEnumDescription(TableHeaderName.Column_PortChain);
                //var PortChain_2 = GridView_Main.Rows[iRowIndex].Cells[_PortChain];
                //strSend = string.Format("(bgw_DoWork) PortChain : {0}", PortChain_2.Value);
            }
            else//parms[1] = all
            {
                var Column_Trigger = TableHeaderName.Column_Trigger.Desc();
                foreach(var Row in GridView_Main.Rows.Cast<DataGridViewRow>())
                {
                    var iRow = Row.Index;
                    strSend += string.Format("{0},{1};", iRow + 1, !GridView_Main.Rows[iRow].Cells[Column_Trigger].ReadOnly);
                }
            }
            return strSend;
        }

        private void SensorTest_Multi__FormClosing(object sender, FormClosingEventArgs e)
        {
            if(PipeBgw != null)
                PipeBgw.CancelAsync();
            string ErrStr = "";
            if(_StreamStr == null)
                return;
            if(StaticVar.EnClientMode == enITS_WakeUpMode.PIPE_MODE)
            {
                if(_StreamStr.WriteString(enITS_Params.DISCONNECT.ToString(), out ErrStr) == -1)
                    MyTool.Save_DBG_Log("[SensorTest_Detail_FormClosing] WriteString {0}", ErrStr);
            }
            if(StaticVar.EnClientMode == enITS_WakeUpMode.SOCKET_MODE)
            {
                //TODO
            }
        }

        public void SensorTest_Multi_FormClose()
        {
            if(m_TriggerOptions == TriggerMode.GPIO)
            {
                var AllDev = StaticVar.lsMain_USBDevInfo.ToList().FindAll(x => x.UI_Idx != -1);
                foreach(var dev in AllDev)
                {
                    if(TestDev[dev.UI_Idx]._CanceToken != null)
                        TestDev[dev.UI_Idx]._CanceToken.Cancel();
                }
            }
        }
        #endregion Socket & PIPE
    }

    public class NamedTimer : System.Timers.Timer
    {
        public readonly string Name;
        public readonly int Index;

        public NamedTimer(string name)
        {
            this.Name = name;
        }

        public NamedTimer(int num)
        {
            this.Index = num;
        }
    }
}