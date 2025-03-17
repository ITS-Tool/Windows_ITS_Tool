using CommonExt;
using FuncMethodCmd;
using ITS_Studio.AllForms;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ThirdPartyTools;
using System.Linq;
using FuncMethodCmd.Tools;
using System.Threading;
using System.ComponentModel;
using ITS_Studio.AllForms.SensorTest;
using System.Threading.Tasks;
using ILITek_ITS_Tool_Lib;
using ITS_Studio.Components;

namespace ITS_Studio
{
    public partial class SensorTest_Detail : Form
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
        private bool isBarCodeTextBoxFocus = false;
        private int inProgressMaximum = 0;
        private int inProgressValue = 0;
        private INI_Tool SensorTest_Profile = new INI_Tool(ITS_Directory.History_INI);
        private int m_iProfileBarcodeLength = 0;
        private int iTimerCnts = 0;
        protected DynamicTool MyTool = new DynamicTool();
        private System.Timers.Timer timer_BarcodeScan;
        private System.Timers.Timer timer_FocusBarcodeText;
        private bool b_Chroma_EN = false;		//For AUO
        private KeyEventArgs tmpKeyEventArgs;

        private int TmpFlag = 0;

        private Task Task_GpioScan;

        public CancellationTokenSource _CanceGPIO_Token;

        private TriggerMode m_TriggerOptions
        {
            get
            {
                string _TriggerOptions = "";
                try
                {
                    _TriggerOptions = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "TriggerOption");
                }
                catch(Exception)
                {
                    _TriggerOptions = TriggerMode.Button.ToString();
                }
                return EnumTool.ParseEnum<TriggerMode>(_TriggerOptions);
            }
        }
        private UIPage MyUIPage = new UIPage();

        public SensorTest_Setting fm_Setting;

        private fmPleaseWait PleaseWait;

        private FunType nowFunType = FunType.NULL;

        public delSetUIEnabled delSet_tabControl_TestBtnEnabled;

        public delScriptFunc delBarcodeClear;

        public delScriptFunc delTrigerTest_API;

        private SensorTest_MPResult fmShowMPResult;

        private SensorTest_Pattern fm_ShortPattern = new SensorTest_Pattern();

        private enum DeviceDetectedStatus
        {
            NotDetected = 0,
            Detected = 1,
            TestFinish = 2,
        } //20211224 Steven add for I2C Detect

        private enum CheckBoxGroupItem
        {
            [Description("Single Profile Verify")]
            VerifySingleProfile,
            [Description("Multi Profile (OnLine)")]
            Online_MultiProfile,
            //[Description("Multi Profile (OffLine)")]
            //Offline_MultiProfile,            
        }

        private DeviceDetectedStatus e_DeviceStatus = DeviceDetectedStatus.NotDetected; //20211224 Steven add for I2C Detect

        private bool isHWReset = false;//20211224 Steven add for I2C Detect  
        
        //===>MP_StressFiles
        /// <summary>
        /// 匯入多個Profile的資料夾，自動輪詢Profile並與裝置跑測試產出各自報告。
        /// </summary>
        private string[] Online_MultiProfiles;
        //private bool bIsOnline_MultiProfile = false;
        
        //===>VerifyTest
        /// <summary>
        /// 匯入多個Report，依據Profile的卡控條件判斷此次Report如果套用這個卡控設定是否會PASS。
        /// </summary>
        private string[] VerifyTestFiles;
        //private bool bIsVerifyTest = false;

        private string _OutFolderName = "";
        private string _OutFolderAndResult = "";

        /// <summary>
        /// 匯入多個Profile的資料夾，並且匯入各個對應的Report，確認跑出來的Report是否會跟匯入的數值一致。
        /// </summary>

        private Dictionary<enWM_MSG, function> mMessageReceiver = new Dictionary<enWM_MSG, function>();

        //For AUO
        private NetConnection NetConnect;
        private Thread m_thrWaitData;
        private NetConnection.MyObject obj = new NetConnection.MyObject();

        public SensorTest_Detail()
        {
            InitializeComponent();
            this.FormClosing += SensorTest_Detail_FormClosing;
            StaticVar.SensorTest_D_FormHandle = this.Handle;
            mMessageReceiver.Add(enWM_MSG.WM_SenTest_UIUpdata, OnSenTestUIUpdata);

            // Hide m_tabControl Appearance
            //tabControl_SensorTest.Appearance = TabAppearance.FlatButtons;
            //tabControl_SensorTest.ItemSize = new Size(0, 1);
            //tabControl_SensorTest.SizeMode = TabSizeMode.Fixed;

            DGV_CB_TestName.SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV_CB_Result.SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV_Progress.SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV_CB_ViewReport.SortMode = DataGridViewColumnSortMode.NotSortable;

            dataGridView_MainTestItem.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView_MainTestItem.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            DGV_Progress.Visible = true;

            TestStatistics.Set_NowValue(0, 0, 0);
            TestStatistics.Show_StatisticsInfo();
            fm_Setting = new SensorTest_Setting();
            fm_Setting.fmSenTest_Detail = this;
            
            tabPage_AutoGen.Parent = null;
            
            delSet_tabControl_TestBtnEnabled += Set_tabControl_TestBtnEnabled;
            delBarcodeClear += ClearBarcode_API;
            delTrigerTest_API += TrigerTest_API;

            //For AUO			
            if(File.Exists(ITS_Directory.CsvSetting))
            {
                NetConnect = new NetConnection();
                NetConnect.ReadINISetting();
                NetConnect.ConnectServer();


                obj.bBusy = false;
                obj.result = "";
                obj.data = "";
                m_thrWaitData = new Thread(NetConnect.WaitData);
                m_thrWaitData.Start(obj);

                b_Chroma_EN = true;
            }
        }

        private void timer_FocusBarcode_Tick(object sender, EventArgs e)
        {
            if(tabControl_TestBtn.SelectedTab == tabPage_Barcode)
            {
                if(!isBarCodeTextBoxFocus)
                    textBox_BarCode.Focus();
                //For AUO				
                if((obj.result != "") && (b_Chroma_EN))
                {
                    textBox_BarCode.Text = obj.result;
                    textBox_BarCode.Focus();

                    if((textBox_BarCode.TextLength == m_iProfileBarcodeLength) || (0 == m_iProfileBarcodeLength))
                    {
                        obj.result = "";
                        obj.bBusy = true;
                        EnterBarcodeTest(textBox_BarCode.Text);
                    }
                }

                if(iTimerCnts > 0)
                {
                    uiLedDisplay_Count.Text = string.Format("{0} Sec", (int)iTimerCnts / 1000);
                    iTimerCnts -= (int)timer_FocusBarcodeText.Interval;
                }
            }
            else if(tabControl_TestBtn.SelectedTab == tabPage_BarcodeBtn)
            {
                Barcode_2.Focus();
            }
        }

        public bool CheckProfileVersion(string strProfileAbsolutePath, out string INI_Profile)
        {
            INI_Tool MyINI = new INI_Tool(strProfileAbsolutePath);
            INI_Profile = MyINI.IniReadValue("System", "ProfileVersion", "");
            Version INI_ProfileVer = new Version(INI_Profile);
            return INI_ProfileVer == VersionInfo.ProfileVersion;
            //int[] _ProfileArr = INI_Profile.Split('.').Select(x => int.Parse(x)).ToArray();
            //bool bIsSame = true;
            //for(int i = 0; i < 3; i++)
            //    bIsSame &= (_ProfileArr[i] == VersionInfo.ProfileVersion[i]);
            //return bIsSame;
        }

        public void UpdateProfilePath(out string ProfilePath)
        {
            string strProfilePath = SensorTest_Profile.IniReadValue("SensorTest_ProfilePath", "Path", "");
            ProfilePath = (Path.IsPathRooted(strProfilePath)) ? strProfilePath : ITS_Directory.Tool_CurrentDirectory + strProfilePath;

            Match rxMatch = Regex.Match(ProfilePath, @"(\\|/)(.*.ini)");
            string RelativePath = "";
            if(rxMatch.Success)
                RelativePath = string.Format("Profile Name : {0}", MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, ProfilePath));
            else
                RelativePath = string.Format("Error : {0}", strProfilePath);
            m_lbProfileName.Text = RelativePath;
        }

        //20220706 Steven Add
        public void UpdateTestInfo()
        {
            byte[] ReturnValue = new byte[0];
            string sModuleName = string.Empty;
            string sSensorID = string.Empty;
            string sFWID = string.Empty;
            byte[] ret = new byte[0];
            //Suspend mode
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.SuspendMode, Command._SWITCH.Dis);

            #region Check Mode & Protocol
            //20220708 Steven
            FW_MODE nowFWMode = new FW_MODE();
            bool bIsDevErr = false;
            bool isBLMode = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.CheckDevMode_API(ref bIsDevErr, FW_MODE.BL_MODE);
            nowFWMode = bIsDevErr ? FW_MODE.UNKNOW_MODE : (isBLMode ? FW_MODE.BL_MODE : FW_MODE.AP_MODE);

            //讀取Protocol Version，確認BL Mode或AP Mode的版本
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_ProtocolVersion(out ReturnValue);// Protocol版本
            //string strVer = string.Format("v{0:X}{1:X}{2:X}", ReturnValue[0], ReturnValue[1], ReturnValue[2]);
            //Protocol_Ver real_protocol_Version = (Enum.IsDefined(typeof(Protocol_Ver), strVer))
            //    ? (Protocol_Ver)Enum.Parse(typeof(Protocol_Ver), strVer) : Protocol_Ver.vUnknow;
            Version real_protocol_Version = new Version(ReturnValue[0], ReturnValue[1], ReturnValue[2]);
            //UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sProtocolVersion = string.Format("{0:X}.{1:X}.{2:X}", ReturnValue[0], ReturnValue[1], ReturnValue[2]);

            if(nowFWMode == FW_MODE.AP_MODE)
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sMode = "AP";
            else if(nowFWMode == FW_MODE.BL_MODE)
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sMode = "BL";
            else
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sMode = "UNKNOWN";

            ///////////////////////////////////////////////////////////////////
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_ChromeInfo(nowFWMode, real_protocol_Version, out ret);
            sFWID = (ret == null) ? "not support" : ret[3].ToString("X2") + ret[2].ToString("X2");

            ListViewItem ItemFWID = new ListViewItem("FW ID");
            ItemFWID.SubItems.Add(sFWID);
            listView_TestInfo.Items.Add(ItemFWID);
            /////////////////////////////////////////////////////////////////// 
            #endregion

            #region Get FW Version
            ret = new byte[0];
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.Get_FirmwareVersion_API(out ret);

            ListViewItem ItemFWVersion = new ListViewItem("FW Version");
            ItemFWVersion.SubItems.Add(string.Join(".", ret.Select(x => string.Format("{0:X}", x))));

            listView_TestInfo.Items.Add(ItemFWVersion);

            #endregion

            #region Get_PID
            ret = new byte[0];
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_HID_Info(nowFWMode, real_protocol_Version, out ret); //傳入現在的Mode和Protocol Version

            ListViewItem ItemPID = new ListViewItem("PID");
            if(ret != null)
                ItemPID.SubItems.Add(string.Format("{0:X4}", (ret[1] << 8 | ret[0]).ToString("X4")));
            else
                ItemPID.SubItems.Add(string.Format("{0:X4}", StaticVar.lsMain_USBDevInfo[m_iSelectedDev].usPID));

            listView_TestInfo.Items.Add(ItemPID);


            #endregion

            //20220707 Joe++ SensorID / EDID需要存到CSV -Steven
            #region Get Sensor ID
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_TPSensor(nowFWMode, real_protocol_Version, out ret);
            if(ret != null)
            {
                if(ret[0] != 0x5A || ret[1] != 0xA5)
                {
                    sSensorID = string.Format("header not match {0}-{1}-{2}", ret[0].ToString("X2"), ret[1].ToString("X2"), ret[2].ToString("X2"));
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sSensorID = sSensorID;
                }
                else
                {
                    sSensorID = ret[2].ToString("X2");
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sSensorID = string.Format("0x{0:X2}", sSensorID);
                }

            }
            else
            {
                sSensorID = "not support";
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sSensorID = sSensorID;
            }

            ListViewItem ItemSensorID = new ListViewItem("Sensor ID");
            ItemSensorID.SubItems.Add(sSensorID);

            listView_TestInfo.Items.Add(ItemSensorID);

            #endregion

            #region Get_ModuleName
            if(nowFWMode == FW_MODE.AP_MODE)
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.Get_KernelVersion_API(nowFWMode, out ReturnValue);
                //Module Name
                if(!UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bUseKernelInfo)
                    sModuleName += string.Format("{0}", StringTool.ASCIIToString(ReturnValue, 6, ReturnValue.Length - 6));
                else
                    sModuleName += string.Format("{0}", StringTool.ASCIIToString(ReturnValue, 10, 18));
            }
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sModualName = sModuleName;

            #endregion

            #region ERRORCODE

            string sErrorCode = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sErrorCode;
            if(sErrorCode != "" && (!UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable) && StaticVar.bShowTestErrorCode)
                m_LbErrorCode.Text = "Test Error Code:" + sErrorCode;
            #endregion

            //////////////////////////////////////////////////////////////////////
            string sProductionInfo = string.Empty;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_ICProductionInfo(nowFWMode, real_protocol_Version, out ret);
            if(ret != null)
            {
                for(int i = 0; i < ret.Length; i++)
                {
                    if(i != ret.Length - 1)
                        sProductionInfo += ret[i].ToString("X2") + ".";
                    else
                        sProductionInfo += ret[i].ToString("X2");
                }
            }
            else
            {
                sProductionInfo = "not support";
            }
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sICProductionInfo = sProductionInfo;

            ListViewItem ItemEDID_Manufacturer = new ListViewItem("EDID Manufacturer Name");
            ItemEDID_Manufacturer.SubItems.Add(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sManufactureCode);

            //UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sEDID = sEDID_ManufacturerCode + "-";

            listView_TestInfo.Items.Add(ItemEDID_Manufacturer);

            //sEDID_ProductCode = lsUsingMonitorEDID[0][11].ToString("X2") + lsUsingMonitorEDID[0][10].ToString("X2");

            ListViewItem ItemEDID_Product = new ListViewItem("EDID Product Code");
            ItemEDID_Product.SubItems.Add(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sProductCode);

            //UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sEDID += sEDID_ProductCode;

            listView_TestInfo.Items.Add(ItemEDID_Product);
            //#endregion

        }

        private void Delay(int millisecondsDelay)
        {
            Thread.Sleep(millisecondsDelay);
        }

        private void TriggerGPIO_Reset(bool test_fail)
        {
            /* bridge GPIO notify */
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C
                && m_TriggerOptions == TriggerMode.GPIO)
            {
                var _getvalue = "";
                foreach(var item in StaticVar.gpio_check_list)
                {
                    if(!test_fail)
                    {
                        if(item.Value.Item1 == enBridgeGpioUsage.PASS_PIN)
                        {
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.set_bridge_pin(item.Key, item.Value.Item2);
                            _getvalue = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "BridgeGpioPassDuration");
                            break;
                        }
                    }
                    else
                    {
                        if(item.Value.Item1 == enBridgeGpioUsage.FAIL_PIN)
                        {
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.set_bridge_pin(item.Key, item.Value.Item2);
                            _getvalue = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "BridgeGpioFailDuration");
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
                        //MyTool.Delay_ms(iValue);
                        RecoverGPIO_Pin();
                    });
                }
            }
        }
        private void SetResultPic(MultiSenTest_VAR.TestResult _InTestResult)
        {
            switch(_InTestResult)
            {
                case MultiSenTest_VAR.TestResult.PASS:
                    m_pbResult.Visible = true;
                    m_pbResult.Image = global::ITS_Studio.Properties.Resources.PASS;
                    StaticVar.bisEnterPressed = false;
                    break;
                case MultiSenTest_VAR.TestResult.FAIL:
                    m_pbResult.Visible = true;
                    m_pbResult.Image = global::ITS_Studio.Properties.Resources.FAIL;
                    StaticVar.bisEnterPressed = false;
                    break;
                case MultiSenTest_VAR.TestResult.Testing:
                    m_pbResult.Visible = true;
                    m_pbResult.Image = global::ITS_Studio.Properties.Resources.loading;
                    break;
                default:
                    m_pbResult.Visible = false;
                    m_pbResult.Image = null;
                    break;
            }
        }
        public void Test_All_Finish(string _Result)
        {
            string result = "Click for result";
            SensorTest_AutoGen autogen = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen;

            int FailCnts = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult.ToList().FindAll(x => x.Value.iFailCnts > 0).Count;
            bool test_fail = (FailCnts > 0 || UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.iTotalFailCounts > 0);

            if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.use_commonflow)
                test_fail = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.iTotalFailCounts > 0;

            if(!StaticVar.bSensorTestShowTestDetail)
                dataGridView_MainTestItem.Rows[dataGridView_MainTestItem.Rows.Count - 1].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.SPEC].Value = "Follow ILITEK SPEC";

            /* UI update */
            if(test_fail)
            {
                // Fail
                SetResultPic(MultiSenTest_VAR.TestResult.FAIL);
                dataGridView_MainTestItem.Rows[dataGridView_MainTestItem.Rows.Count - 1].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Style.BackColor = Color.Red;
                dataGridView_MainTestItem.Rows[dataGridView_MainTestItem.Rows.Count - 1].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Value = "Fail";
            }
            else
            {
                // Pass
                SetResultPic(MultiSenTest_VAR.TestResult.PASS);
                dataGridView_MainTestItem.Rows[dataGridView_MainTestItem.Rows.Count - 1].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Style.BackColor = Color.Green;
                dataGridView_MainTestItem.Rows[dataGridView_MainTestItem.Rows.Count - 1].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Value = "Pass";

                if(autogen.m_setting.enable)
                {
                    //auto gen, if the report is pass, the report will be copy to AutoGenLogs\\ and count++
                    if(autogen.m_setting.pass_log_cnts < autogen.m_setting.counts)
                        autogen.CopyPassReport(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strFinalReportPath);
                    //2022/10/14將新產生的profile複製到\AutoGenPass備份動作取消by evayn
                    //autogen.CopyRunningProfile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                    if(autogen.m_setting.pass_log_cnts >= autogen.m_setting.counts)
                    {
                        //RunningLogAnalysisTool();
                        result = "";
                    }
                    else
                    {
                        tb_targetNum.Text = autogen.m_setting.counts.ToString();
                        tb_passNum.Text = autogen.m_setting.pass_log_cnts.ToString();
                        tb_autogen_logfrom.Text = autogen.m_setting.log_path;
                    }
                }
            }

            TestStatistics.Set_NowValue(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.AllTestCount.iPassCounts
                , UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.AllTestCount.iFailCounts
                , UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.AllTestCount.iTotalCounts);
            TestStatistics.Show_StatisticsInfo();

            tabControl_TestBtn.Enabled = true;
            btn_LoadProfile.Enabled = true;
            btn_ReplayReport.Enabled = true;
            btn_ShowMPResult.Enabled = true;
            tabControl_SensorTest.Enabled = true;

            // Clode 0xF0 Monitor
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(0);

            m_btRunTest.Image = global::ITS_Studio.Properties.Resources.Play_Pressed_icon;

            if(result != "")
                dataGridView_MainTestItem.Rows[dataGridView_MainTestItem.Rows.Count - 1].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.TestName].Value
                    = string.Format("Test Result ({0:0.00}sec)", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.TotalTestTimeSpand);
            else
                dataGridView_MainTestItem.Rows[dataGridView_MainTestItem.Rows.Count - 1].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.TestName].Value = "";

            if(!UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi._GridView_Cell.strTotleReult_ErrCode.Contains("Report Path not Found"))
            {
                dataGridView_MainTestItem.Rows[dataGridView_MainTestItem.Rows.Count - 1].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.ReportLink].Value = result;
            }

            if(tabControl_TestBtn.SelectedTab == tabPage_Button)
            {
                textBox_BarCode.Enabled = false;
                m_tmRunButton.Enabled = true;
                if(timer_BarcodeScan != null)
                    timer_BarcodeScan.Stop();
                BarcodeTimer_En(false);
            }
            else
            {
                textBox_BarCode.Enabled = true;
                m_tmRunButton.Enabled = false;
                if(tabControl_TestBtn.SelectedTab == tabPage_Barcode)
                {
                    textBox_BarCode.Focus();
                    textBox_BarCode.Clear();
                    iTimerCnts = 0;
                    if(timer_FocusBarcodeText != null)
                        timer_FocusBarcodeText.Start();
                }
                else if(tabControl_TestBtn.SelectedTab == tabPage_BarcodeBtn)
                {
                    Barcode_2.Focus();
                    Barcode_2.Clear();
                    if(timer_FocusBarcodeText != null)
                        timer_FocusBarcodeText.Start();
                }
            }

            //For AUO
            if(b_Chroma_EN)
                obj.bBusy = false;
            if(_NowStation == SensorTestStation.FPC)
            {
                string msg = "Test Finish\n";
                msg += UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsFPC_Warning[0];
                WarningFormShow("Tip", msg);

                //FPC Mode不要下Normal Mode command
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_SenTest_ModeChange, (IntPtr)((int)TouchMode.NormalMode), string.Format("{0},{1}", m_iSelectedDev, TouchMode_UI.UI_Only));
            }
            else
            {
                if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.SetBridge_INT_Status_API(GPIO_PIN_TYPE.INT_DISABLE);

                if(!_Result.Contains("Protocol not match"))
                {
                    if((!StaticVar.bTestFinishPwrDown) && (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C))
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_SenTest_ModeChange, (IntPtr)((int)TouchMode.NormalMode), string.Format("{0},{1}", m_iSelectedDev, TouchMode_UI.CMD_and_UI));

                    if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType != DEV_INTERFACE_TYPE.I2C)
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_SenTest_ModeChange, (IntPtr)((int)TouchMode.NormalMode), string.Format("{0},{1}", m_iSelectedDev, TouchMode_UI.CMD_and_UI));

                }
            }

            if(StaticVar.bI2CDetect && StaticVar._ITSTool_Var == enMode.OPMode)
            {
                timer_I2CDetect.Enabled = true;
                e_DeviceStatus = DeviceDetectedStatus.TestFinish;
                if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.LocalVariable.Var_Multi.bI2CError)
                {
                    PleaseWait = new fmPleaseWait();
                    PleaseWait.Show();
                }
            }

            if(StaticVar.EnClientMode != enITS_WakeUpMode.NONE)
            {
                var TestResult = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.iTotalFailCounts == (int)MultiSenTest_VAR.TestResult.PASS ? "PASS" : "FAIL";
                var Barcode = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strBarCodeText;
                var ReportPath = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strFinalReportPath;
                string ErrStr = "";
                if (TestResult.Equals("FAIL"))
                {
                    string sErrorCode = string.Empty;
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sErrorCode != string.Empty)
                    {
                        string[] sErrorArrary = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.sErrorCode.Split(',');

                        foreach (var item in sErrorArrary)
                        {
                            if (item == "")
                                continue;
                            int value = Convert.ToInt32(item, 16);
                            GenLog.SensorTest.ERROR_CODE errorName = (GenLog.SensorTest.ERROR_CODE)value;
                            sErrorCode += string.Format("{0}:{1},", item, errorName.ToString());
                        }
                    }
                    else
                    {
                        sErrorCode += string.Format("{0}:{1},", "-200", "I2C critical error");
                    }

                    if (!sErrorCode.IsNullOrEmpty())
                        if (sErrorCode.Length > 0)
                            sErrorCode = sErrorCode.Remove(sErrorCode.Length - 1, 1);

                    var strPar = string.Format("{0};{1}={2};{3}={4};{5}={6};{7}={8}"
                       , enITS_Params.RESPOND
                       , enCustomFormatMember.Barcode
                       , Barcode
                       , enCustomFormatMember.Result
                       , TestResult
                       , enCustomFormatMember.LogPath
                       , ReportPath
                       , enCustomFormatMember.ErrorStatus
                       , sErrorCode);
                    if (StaticVar.EnClientMode == enITS_WakeUpMode.PIPE_MODE)
                        if (_StreamStr.WriteString(strPar, out ErrStr) == -1)
                            MyUIPage.ShowErrorDialog("Pipe 錯誤提示", ErrStr, UIStyle.Red, false);

                    if (StaticVar.EnClientMode == enITS_WakeUpMode.SOCKET_MODE)
                        SendToSocket(strPar);
                }
                else
                {
                    var strPar = string.Format("{0};{1}={2};{3}={4};{5}={6}"
                   , enITS_Params.RESPOND
                   , enCustomFormatMember.Barcode
                   , Barcode
                   , enCustomFormatMember.Result
                   , TestResult
                   , enCustomFormatMember.LogPath
                   , ReportPath);

                    if (StaticVar.EnClientMode == enITS_WakeUpMode.PIPE_MODE)
                        if (_StreamStr.WriteString(strPar, out ErrStr) == -1)
                            MyUIPage.ShowErrorDialog("Pipe 錯誤提示", ErrStr, UIStyle.Red, false);

                    if (StaticVar.EnClientMode == enITS_WakeUpMode.SOCKET_MODE)
                        SendToSocket(strPar);
                }

            }
            /* restart Gpio timer */
            if(m_TriggerOptions == TriggerMode.GPIO)
            {
                MyTool.Delay_ms(100);
                TriggerGPIO_Reset(test_fail);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();
                _CanceGPIO_Token = new CancellationTokenSource();
                Task_GpioScan = Task.Factory.StartNew(() => GpioScanEvent(), _CanceGPIO_Token.Token);
            }

        }

        public void WarningFormShow(string strTip, string strMsg)
        {
            WarningForm Warning_Form = new WarningForm(strTip, strMsg, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.BlinkTimer);
            if(Warning_Form.ShowDialog(this) == DialogResult.OK)
                Warning_Form.Dispose();
        }

        public void Painting_Finish()
        {
            UI_GBV.fm_SensorTest_Painting.m_bTestRunning = false;
            tabControl_SensorTest.SelectedTab = tabPage_SensorTestMain;
            tabPage_SensorTestPaint.Controls.Clear();
            UI_GBV.fm_SensorTest_Painting.m_CursorShown = true;
#if !LITE_BUILD
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_SensorTest_D);
#endif
        }
        
        public void SetMainFormLocation(ref Form ChildForm, int monitor_num)
        {
            if(monitor_num > Screen.AllScreens.Length)
                monitor_num = 1;
            ChildForm.Left = Screen.AllScreens[monitor_num - 1].Bounds.X;
            ChildForm.Top = Screen.AllScreens[monitor_num - 1].Bounds.Y;
            ChildForm.Width = Screen.AllScreens[monitor_num - 1].Bounds.Width;
            ChildForm.Height = Screen.AllScreens[monitor_num - 1].Bounds.Height;
            if(UI_GBV.fm_SensorTest_Painting != null)
                UI_GBV.fm_SensorTest_Painting.DrawingPanelStart = new Point(ChildForm.Left, ChildForm.Top);
            //Console.WriteLine("[SetMainFormLocation]=>{0},{1},{2},{3}", ChildForm.Left, ChildForm.Top, ChildForm.Width, ChildForm.Height);
        }

        public void Drawing_Reset()
        {
            tabControl_SensorTest.SelectedTab = tabPage_SensorTestPaint;
            UI_GBV.fm_SensorTest_Painting.SelectedDev = SelectedDev;
            INI_Tool MyINI = new INI_Tool(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            Form childform = UI_GBV.fm_SensorTest_Painting.FindForm();
            var Testitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enPaintingTest);
            if(StaticVar.bFull_Screen)
            {
                childform.TopLevel = true;
                childform.Owner = StaticVar.MainFormParent;
                childform.StartPosition = FormStartPosition.Manual;

                string Number = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("System", "Screen_Number");
                var iniScreenNum = 0;
                int.TryParse(Number, out iniScreenNum);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.iScreenNum = iniScreenNum;//Convert.ToUInt16(Number, 16);
                if(StaticVar.bAutoRotationAngle)
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.RotationAngle = StaticVar.lsAllScreenInfo.Find(x => x.ScreenNumber == iniScreenNum).RotationAngle;
                else
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.RotationAngle = Orientations.DEGREES_CW_0;
                SetMainFormLocation(ref childform, iniScreenNum);
            }
            else
            {
                childform.TopLevel = false;
                childform.Size = new System.Drawing.Size(1, 1);
                childform.Dock = DockStyle.Fill;
                tabControl_SensorTest.Enabled = true;
                tabPage_SensorTestPaint.Controls.Add(childform);
                childform.WindowState = FormWindowState.Normal;
            }

            //UI_GBV.fm_SensorTest_Painting.Show();
            UI_GBV.fm_SensorTest_Painting.PenHandle_Init();
            UI_GBV.fm_SensorTest_Painting.SensorTest_ResetPattern(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern);

            bool bNeedTimer = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.g_NowTestPattern == enPaintTestPattern.Paint_FreeDrawing && UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.bClickResultButton) ? false : true;
            UI_GBV.fm_SensorTest_Painting.SensorTest_TestStart(bNeedTimer);
            childform.Show();
            childform.Refresh();
            UI_GBV.fm_SensorTest_Painting.iEscape = 0;
        }

        public void ShowShortPattern(bool isShow, bool isCloseMonitor)
        {
            Form childForm = fm_ShortPattern.FindForm();
            INI_Tool MyINI = new INI_Tool(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            if(isShow)
            {
                if(isCloseMonitor)
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "scrnsave.scr";
                    startInfo.Arguments = @"/s";
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardInput = true;
                    process.StartInfo = startInfo;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.Close();
                }
                else
                {
                    childForm.TopLevel = true;
                    childForm.Owner = StaticVar.MainFormParent;
                    childForm.StartPosition = FormStartPosition.Manual;
                    string sTemp = MyINI.IniReadValue("Short_Test", "PatternColor", "#FFFFFF");
                    Color tempColor = System.Drawing.ColorTranslator.FromHtml(sTemp);
                    childForm.BackColor = tempColor;

                    string Number = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("System", "Screen_Number");
                    var iniScreenNum = 0;
                    int.TryParse(Number, out iniScreenNum);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.iScreenNum = iniScreenNum;//Convert.ToUInt16(Number, 16);
                    SetMainFormLocation(ref childForm, iniScreenNum);
                    fm_ShortPattern.SetLabelMiddle();
                    childForm.Show();
                    childForm.Refresh();
                }
            }
            else
            {
                if(isCloseMonitor)
                    Cursor.Position = new Point(200, 350);
                else
                    childForm.Hide();
            }

        }

        public void vfStop(DevOption StopOption)
        {
            switch(StopOption)
            {
                case DevOption.RemoveDevice: // PnP Remove Device
                    vfCancelAll("No Devcie");
                    break;

                case DevOption.I2CErr: // 0xF0
                    vfCancelAll("I2C Error");
                    break;

                case DevOption.ForceStop:
                    vfCancelAll("User Force Stop");
                    break;

                default:
                    break;
            }
        }

        public void TestItem_GridInitial()
        {
            _NowStation = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation;
            ////20220719 Steven Add
            string strToolStart_ShowTestDetail = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "ShowTestDetail");
            strToolStart_ShowTestDetail = string.IsNullOrEmpty(strToolStart_ShowTestDetail) ? "True" : strToolStart_ShowTestDetail;
            StaticVar.bSensorTestShowTestDetail = Convert.ToBoolean(strToolStart_ShowTestDetail);

            DataTable_Interchange DT_Tool = new DataTable_Interchange();

            DataRow Row;
            int inGridCount = 0;
            SetResultPic(MultiSenTest_VAR.TestResult.NT);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.DT_SensorTest.Clear();
            dataGridView_MainTestItem.Rows.Clear();

            // Test Item
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.DT_SensorTest = DT_Tool.DataGridView2DataTable(dataGridView_MainTestItem);
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsTestItemDefine.Count != 0)
            {
                int iNowRow = 0;
                var Words = EnumTool.EnumToList<Uniformity_SectorName>().Select(x => string.Format("{0}_En", x)).ToList();
                Words.AddRange(EnumTool.EnumToList<MicroOpen_TestItems>().Select(x => string.Format("{0}_En", x)).ToList());

                foreach(var enumType in EnumTool.EnumToList<TestItemNameDefine>())
                {
                    MultiSenTest_VAR.TestStatus NowTestitem = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[enumType];
                    if(!NowTestitem.bTestitemVisible)
                        continue;

                    //20230707 在autogen mode下，enOpenTest/enShortTest 一定會測試
                    if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable)
                    {
                        if(enumType != TestItemNameDefine.enOpenTest || enumType != TestItemNameDefine.enShortTest)
                            if(!NowTestitem.bEnable)
                                continue;
                    }
                    else
                    {
                        if(!NowTestitem.bEnable)
                            continue;
                    }

                    string strTestName = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(enumType);
                    if(!UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsTestItemDefine.Exists(x => x.Testitem_Name == strTestName))
                        continue;

                    Row = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.DT_SensorTest.NewRow();
                    var FoundItem = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.lsTestItemDefine.Find(x => x.Testitem_Name == strTestName);
                    Row["Test_Name"] = FoundItem.Testitem_Name;

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.DT_SensorTest.Rows.Add(Row);

                    NowTestitem.iTestSequence = inGridCount;
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[enumType] = NowTestitem;

                    if(StaticVar.bSensorTestShowTestDetail) //20220719 Steven
                    {
                        if(FoundItem.Testitem_Name != "FrequencyScan")
                            dataGridView_MainTestItem.Rows.Add(FoundItem.Testitem_Name);
                    }
                    inGridCount++;
                    //Status
                    if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable)
                    {
                        if(enumType == TestItemNameDefine.enGpio_Test
                             || enumType == TestItemNameDefine.enP2PTest
                            || enumType == TestItemNameDefine.enPaintingTest
                            || enumType == TestItemNameDefine.enPowerTest
                            || enumType == TestItemNameDefine.enSRAMTest
                            || enumType == TestItemNameDefine.enFWFixed)
                        {
                            dataGridView_MainTestItem.Rows[iNowRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Value = "No Test";
                            dataGridView_MainTestItem.Rows[iNowRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Style.ForeColor = Color.Blue;
                            dataGridView_MainTestItem.Rows[iNowRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Style.Font
                                = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

                        }
                        else if((enumType == TestItemNameDefine.enUniformityTest && !UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.uniformity_judgement)
                            || enumType == TestItemNameDefine.enMicroOpenTest)
                        {
                            dataGridView_MainTestItem.Rows[iNowRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Value = "Data Collection";
                            dataGridView_MainTestItem.Rows[iNowRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Style.ForeColor = Color.Blue;
                            dataGridView_MainTestItem.Rows[iNowRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Style.Font
                                = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                        }
                    }
                    // SPEC
                    List<MultiSenTest_VAR._SPEC_> lsSPEC = NowTestitem.SPEC;
                    StringBuilder strbudBuffer = new StringBuilder();
                    for(int iCnt = 0; iCnt < lsSPEC.Count; iCnt++)
                    {
                        string FindKeyName = lsSPEC[iCnt].Name;
                        bool IsPaintingTest = Enum.IsDefined(typeof(enPaintTestPattern), FindKeyName);
                        if(IsPaintingTest)
                        {
                            if(lsSPEC[iCnt].Visible == enSPECVisible.True)
                                if(lsSPEC[iCnt].bTestEn)
                                    strbudBuffer.AppendFormat("Pattern {0}\r\n", FindKeyName);

                            foreach(var str in lsSPEC[iCnt].lsDrawingSpec)
                                if(str.Visible == enSPECVisible.True)
                                    strbudBuffer.AppendFormat("{0} = {1}\r\n", str.Name, str.strDefaultValue);
                        }
                        else
                        {
                            if((enumType == TestItemNameDefine.enMicroOpenTest) || (enumType == TestItemNameDefine.enUniformityTest))
                            {
                                if(lsSPEC[iCnt].ShowType == enSPECShowType.Checkbox)
                                {
                                    var ItemChecked = lsSPEC[iCnt].bTestEn;

                                    if(ItemChecked)
                                        if(lsSPEC[iCnt].Name.Contains("_En"))
                                            strbudBuffer.AppendFormat("{0}\r\n", lsSPEC[iCnt].Name);
                                }
                            }
                            else
                            {
                                if(lsSPEC[iCnt].Visible == enSPECVisible.True)
                                {
                                    switch(lsSPEC[iCnt].ShowType)
                                    {
                                        case enSPECShowType.DataGridViewTextBox:
                                        case enSPECShowType.DataGridViewComboBox:
                                            strbudBuffer.AppendFormat("{0} = {1}\r\n", lsSPEC[iCnt].Name, lsSPEC[iCnt].dtValue.Rows[0][0].ToString());
                                            break;

                                        case enSPECShowType.Checkbox:
                                            //var ItemChecked = ((CheckBox)lsSPEC[inSPECCount]._Object).Checked;
                                            var ItemChecked = lsSPEC[iCnt].bTestEn;
                                            if(ItemChecked)
                                                strbudBuffer.AppendFormat("{0}\r\n", lsSPEC[iCnt].Name);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    if(StaticVar.bSensorTestShowTestDetail) //20220719 Steven
                        if(enumType != TestItemNameDefine.enFrequencyScan)
                            dataGridView_MainTestItem.Rows[iNowRow++].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.SPEC].Value = strbudBuffer.ToString();
                }
            }
            // Test Result
            dataGridView_MainTestItem.Rows.Add("Test Result");
            var RowIndex = dataGridView_MainTestItem.Rows.Count - 1;

            foreach(var item in EnumTool.EnumToList<MultiSenTest_VAR.GridView_Cell.ResultTable>())
            {
                if(item == MultiSenTest_VAR.GridView_Cell.ResultTable.Processbar)
                    dataGridView_MainTestItem.Rows[RowIndex].Cells[(int)item] = new DataGridViewTextBoxCell();
                else
                    dataGridView_MainTestItem.Rows[RowIndex].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Style.Font
                        = new Font("Consolas", 14F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            }
        }

        public void TriggerOptionChange()
        {
            uiSymbolLabel_GPIO.Visible = false;
            timer_BarcodeScan = null;
            timer_FocusBarcodeText = null;
            switch(m_TriggerOptions)
            {
                case TriggerMode.BarCode:
                    timer_FocusBarcodeText = new System.Timers.Timer();
                    timer_FocusBarcodeText.Enabled = false;
                    timer_FocusBarcodeText.Interval = 500;
                    timer_FocusBarcodeText.Elapsed += new System.Timers.ElapsedEventHandler(timer_FocusBarcode_Tick);
                    timer_FocusBarcodeText.AutoReset = true;
                    timer_FocusBarcodeText.SynchronizingObject = this;

                    timer_BarcodeScan = new System.Timers.Timer();
                    timer_BarcodeScan.Enabled = false;
                    timer_BarcodeScan.Interval = 100;
                    timer_BarcodeScan.Elapsed += new System.Timers.ElapsedEventHandler(timer_BarcodeScan_Tick);
                    timer_BarcodeScan.AutoReset = true;
                    timer_BarcodeScan.SynchronizingObject = this;
                    tabControl_TestBtn.SelectedTab = tabPage_Barcode;
                    int.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Report", enReport.Barcode_Length.ToString()), out m_iProfileBarcodeLength);
                    textBox_BarCode.Enabled = true;
                    m_tmRunButton.Enabled = false;
                    timer_FocusBarcodeText.Start();
                    break;

                case TriggerMode.BarCodeButton:
                    timer_FocusBarcodeText = new System.Timers.Timer();
                    timer_FocusBarcodeText.Enabled = false;
                    timer_FocusBarcodeText.Interval = 500;
                    timer_FocusBarcodeText.Elapsed += new System.Timers.ElapsedEventHandler(timer_FocusBarcode_Tick);
                    timer_FocusBarcodeText.AutoReset = true;
                    timer_FocusBarcodeText.SynchronizingObject = this;
                    tabControl_TestBtn.SelectedTab = tabPage_BarcodeBtn;
                    textBox_BarCode.Enabled = false;
                    m_tmRunButton.Enabled = true;
                    timer_FocusBarcodeText.Start();
                    break;

                case TriggerMode.GPIO:
                    tabControl_TestBtn.SelectedTab = tabPage_Button;
                    textBox_BarCode.Enabled = false;
                    m_tmRunButton.Enabled = true;
                    uiSymbolLabel_GPIO.Visible = true;
                    switch(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType)
                    {
                        case DEV_INTERFACE_TYPE.HID:
                        case DEV_INTERFACE_TYPE.HID_BL:
                        case DEV_INTERFACE_TYPE.WIFI_HID:
                            MyUIPage.ShowErrorDialog("ERROR", "HID not support GPIO Trigger Mode.", UIStyle.Red, true);
                            return;
                    }
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();
                    StaticVar.gpio_check_list.Clear();
                    enBridgeGpioUsage usage;
                    GPIO_PIN_TYPE type;

                    List<Tuple<enOP_Mode, enBridge_Pin>> list = new List<Tuple<enOP_Mode, enBridge_Pin>>();
                    list.Add(new Tuple<enOP_Mode, enBridge_Pin>(enOP_Mode.BridgeGpioTS2, enBridge_Pin.TS_2));
                    list.Add(new Tuple<enOP_Mode, enBridge_Pin>(enOP_Mode.BridgeGpioTS1, enBridge_Pin.TS_1));
                    list.Add(new Tuple<enOP_Mode, enBridge_Pin>(enOP_Mode.BridgeGpioPN0, enBridge_Pin.PN_0));

                    bool[] bFlag = new bool[list.Count];
                    for(int i = 0; i < list.Count; i++)
                    {
                        string[] arr = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", list[i].Item1.ToString()).Split(',');
                        usage = EnumTool.GetValueFromDescription<enBridgeGpioUsage>(arr[0]);
                        type = (arr[1] == enBridgeGpioTrigger.high.Desc()) ? GPIO_PIN_TYPE.HIGH : GPIO_PIN_TYPE.LOW;
                        bFlag[i] = (usage == enBridgeGpioUsage.START_PIN) ? true : false;
                        StaticVar.gpio_check_list.Add(list[i].Item2, new Tuple<enBridgeGpioUsage, GPIO_PIN_TYPE>(usage, type));
                    }

                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.init_bridge_pin(bFlag[0], bFlag[1], bFlag[2]);
                    RecoverGPIO_Pin();
                    _CanceGPIO_Token = new CancellationTokenSource();
                    Task_GpioScan = Task.Factory.StartNew(() => GpioScanEvent(), _CanceGPIO_Token.Token);

                    break;
                default:
                    tabControl_TestBtn.SelectedTab = tabPage_Button;
                    textBox_BarCode.Enabled = false;
                    m_tmRunButton.Enabled = true;
                    if(timer_FocusBarcodeText != null)
                        timer_FocusBarcodeText.Stop();
                    break;
            }
        }

        private void RecoverGPIO_Pin()
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
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.set_bridge_pin(item.Key, GPIO_PIN_TYPE.LOW);
                        else if(item.Value.Item2 == GPIO_PIN_TYPE.LOW)
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.set_bridge_pin(item.Key, GPIO_PIN_TYPE.HIGH);
                        break;
                }
            }
        }

        private void BarcodeTimer_En(bool bEnable)
        {
            if((timer_BarcodeScan == null) || (timer_FocusBarcodeText == null))
                return;
            if(bEnable)
            {
                int iInterval = 0;
                int.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Report", enReport.Barcode_ScanInterval.ToString()), out iInterval);
                if(iInterval == 0)
                    bEnable = false;
                else
                    timer_BarcodeScan.Interval = iInterval;
                iTimerCnts = iInterval;
                timer_BarcodeScan.Stop();
                timer_FocusBarcodeText.Stop();
            }

            if(bEnable)
            {
                timer_BarcodeScan.Start();
                timer_FocusBarcodeText.Start();
            }
            else
            {
                timer_BarcodeScan.Stop();
                timer_FocusBarcodeText.Stop();
                uiLedDisplay_Count.Text = "0 Sec";
            }
        }

        public void UpdateMainTestGrid(int iSelectedDev, int iRow)
        {
            dataGridView_MainTestItem.SuspendLayout();
            // DGV_CB_Result

            if(StaticVar.bSensorTestShowTestDetail) //20220719 Steven
            {
                int iCol = (int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status;
                dataGridView_MainTestItem.Rows[iRow].Cells[iCol].Value = UI_GBV.mDev[iSelectedDev].LocalVariable.VarSenTest_Multi.DT_SensorTest.Rows[iRow][iCol].ToString();
                dataGridView_MainTestItem.Rows[iRow].Cells[iCol].Style.ForeColor = (string.Equals(dataGridView_MainTestItem.Rows[iRow].Cells[iCol].Value.ToString(), "Pass")) ? Color.Green : Color.Red;

                if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable
                    && (string.Equals(dataGridView_MainTestItem.Rows[iRow].Cells[iCol].Value.ToString(), "Data Collection")))
                    dataGridView_MainTestItem.Rows[iRow].Cells[iCol].Style.ForeColor = Color.Blue;

                // DGV_CB_ViewReport
                iCol = (int)MultiSenTest_VAR.GridView_Cell.ResultTable.ReportLink;
                dataGridView_MainTestItem.Rows[iRow].Cells[iCol].Value = UI_GBV.mDev[iSelectedDev].LocalVariable.VarSenTest_Multi.DT_SensorTest.Rows[iRow][iCol].ToString();
            }
            dataGridView_MainTestItem.ResumeLayout(true);
        }

        public void Set_ProgressBar(ref Message msg)
        {
            dataGridView_MainTestItem.SuspendLayout();
            string str = Marshal.PtrToStringAuto(msg.LParam);

            string strName = str.Split(':')[0];
            var Testitem = (TestItemNameDefine)Enum.Parse(typeof(TestItemNameDefine), strName);
            //int iObj = (int)msg.LParam;
            //var Testitem = (TestItemNameDefine)iObj;

            inProgressValue = (int)msg.WParam;
            if(inProgressMaximum <= 0)
                inProgressMaximum = 1;
            int inIndex = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult[Testitem].iTestSequence;
            double dbValue = ((double)inProgressValue / inProgressMaximum) * 100;
            dbValue = dbValue > 100 ? 100 : dbValue;
            //string StatusValue = string.Format("{0:0.00}%", dbValue);
            string ProcessbarValue = string.Format("{0:0}", dbValue);

            if(StaticVar.bSensorTestShowTestDetail && inIndex != -1) //20220719 Steven 
            {
                //20230630 增加 && inIndex != -1為了讓在autogen mode時，當uniformity測項在背景執行時只蒐集log並不顯示在畫面上也可以成功產出報告。
                dataGridView_MainTestItem.Rows[inIndex].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Processbar].Value = ProcessbarValue;
            }

            dataGridView_MainTestItem.ResumeLayout(true);
        }

        public void Set_ProgressBar_Max(ref Message m)
        {
            inProgressMaximum = (int)m.WParam;
        }

        private void dataGridView_MainTestItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string SelectColumn = dataGridView_MainTestItem.Columns[e.ColumnIndex].HeaderText;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange = false;
            m_LbErrorCode.Text = "";
            if(e.RowIndex == -1)
                return;
            if(SelectColumn == "View Report")
            {
                int iReportLinkCol = (int)MultiSenTest_VAR.GridView_Cell.ResultTable.ReportLink;

                if(dataGridView_MainTestItem.Rows[e.RowIndex].Cells[iReportLinkCol].Value == null)
                    return;

                if(e.RowIndex >= UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.DT_SensorTest.Rows.Count || !StaticVar.bSensorTestShowTestDetail) //20220719 Steven
                {
                    if(dataGridView_MainTestItem.Rows[e.RowIndex].Cells[iReportLinkCol].Value.ToString() == "Click for result")
                    {
                        if((nowFunType == FunType.ReplayLog) || (nowFunType == FunType.Test))
                        {
                            Process.Start("notepad.exe", string.Format(@"{0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strFinalReportPath));
                        }
                        else if(nowFunType == FunType.VerifyTest)
                        {
                            Process.Start(string.Format(@"{0}", _OutFolderName));
                        }
                    }
                }
                else
                {
                    if(dataGridView_MainTestItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Click for result")
                    {
                        SensorTest_Report fm_Report = new SensorTest_Report(SelectedDev, e.RowIndex);
                        fm_Report.Text = dataGridView_MainTestItem.Rows[e.RowIndex].Cells[0].Value.ToString();
                        fm_Report.ShowDialog(this);
                    }
                }
            }
            else if(SelectColumn == "SPEC")
            {
                if(nowFunType == FunType.ReplayLog)
                    return;
                if(!StaticVar.bSensorTestShowTestDetail) //20220721 Steven
                    return;
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bRunningStatus)
                    return;
                if(e.RowIndex < UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.DT_SensorTest.Rows.Count)
                {
                    var SelectItem = EnumTool.GetValueFromDescription<TestItemNameDefine>(dataGridView_MainTestItem.Rows[e.RowIndex].Cells[0].Value.ToString());

                    EnterTestSettings(SelectItem);
                }
            }
            fm_Setting.m_ui_on_treeview = false;//目前在main。
        }

        public void ChangeTestInfoViewerVisible(bool isVisible) //20220725 Steven
        {
            uiSplitContainer_Testitem.Panel1Collapsed = !isVisible;
        }

        public void ShowBarCodeScanWaterMark(bool isShowWaterMark)
        {
            textBox_BarCode.Watermark = (isShowWaterMark) ? "Barcode" : "";
            Barcode_2.Watermark = (isShowWaterMark) ? "Barcode" : "";
        }

        public void ChangeAutoGen_UI(SensorTest_AutoGen autogen)
        {
            if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.lsMainSetting.Find(x => x.strName == "Auto_Gen.").strValue == null)
            {
                fm_Setting.Show_AutoGen_Setting(false);
            }
            else
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                {
                    fm_Setting.Show_AutoGen_Setting(false);
                    autogen.m_setting.enable = false;
                }
                else
                {
                    fm_Setting.Show_AutoGen_Setting(true);
                }
            }
            
            if(autogen.m_setting.enable)
            {
                autogen.Get_logPassCnts();
                tabPage_AutoGen.Parent = tabControl_SensorTestFunc;
                _NowStation = SensorTestStation.AUTOGEN;
                tb_targetNum.Text = autogen.m_setting.counts.ToString();
                tb_passNum.Text = autogen.m_setting.pass_log_cnts.ToString();
                tb_autogen_logfrom.Text = autogen.m_setting.log_path;
                tabControl_TestResult.Visible = false;
                //splitContainer_TestResult.Panel2Collapsed = true;
                TestStatistics.Visible = false;
                SetResultPic(MultiSenTest_VAR.TestResult.NT);
                btn_ReplayReport.Text = "Reload Coll. Logs";

                if(autogen.m_setting.log_path == "" 
                    || Path.GetFileNameWithoutExtension(autogen.m_setting.new_ProfileName) == "")
                {
                    m_btRunTest.Enabled = false;
                    textBox_BarCode.Enabled = false;
                    if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                        MyUIPage.ShowErrorDialog("Collection path or new profile name is empty!");
                    else
                        MyUIPage.ShowErrorDialog("蒐集路徑內容無效 或者 新的Profile檔名是空白的!");

                }
                else
                {
                    if(!Directory.Exists(autogen.m_setting.log_path))
                    {
                        m_btRunTest.Enabled = false;
                        textBox_BarCode.Enabled = false;
                        if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                            MyUIPage.ShowErrorDialog("Collection path is not a valid directory.!\n");
                        else
                            MyUIPage.ShowErrorDialog("蒐集路徑並非有效的內容!\n");
                    }
                    else
                    {
                        m_btRunTest.Enabled = true;
                        textBox_BarCode.Enabled = true;
                    }
                }
            }
            else
            {
                tabPage_AutoGen.Parent = null;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.AllTestCount.iPassCounts = 0;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.AllTestCount.iFailCounts = 0;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.AllTestCount.iTotalCounts = 0;
                TestStatistics.Set_NowValue(0, 0, 0);
                TestStatistics.Show_StatisticsInfo();
                //splitContainer_TestResult.Panel2Collapsed = false;
                tabControl_TestResult.Visible = true;
                TestStatistics.Visible = true;
                btn_ReplayReport.Text = "Load Report";
                m_btRunTest.Enabled = true;
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if(this.dataGridView_MainTestItem.IsCurrentCellDirty)
                this.dataGridView_MainTestItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

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
        private bool CheckPWD()
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
                    return false;
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
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

                    return false;
                }
            }
            return true;
        }
        
        private void btnLoadProfile_Click(object sender, EventArgs e)
        {
            if(tabControl_TestBtn.SelectedTab == tabPage_Barcode)
                timer_FocusBarcodeText.Stop();

            if(!CheckPWD())
            {
                if(tabControl_TestBtn.SelectedTab == tabPage_Barcode)
                    timer_FocusBarcodeText.Start();
                return;
            }

            string FileName = "";
            if(!SelectFile("Profile / IliTest (*.ini,*ilitest)|*.ini;*.ilitest", Path.GetDirectoryName(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath), out FileName))
            {
                if(tabControl_TestBtn.SelectedTab == tabPage_Barcode)
                    timer_FocusBarcodeText.Start();
                return;
            }

            //if(FileName.Contains(".ilitest"))
            //{
            //    //Steven Add for Sensor Test GenTool
            //    Dictionary<string, Dictionary<string, string>> dicIliTest = new Dictionary<string, Dictionary<string, string>>();

            //    foreach(var item in fm_Setting.m_LATool.m_template_pro_proc.m_TemplateData)
            //        dicIliTest.Add(item.Key, new Dictionary<string, string>(item.Value));

            //    ReadIliTestFile(FileName, ref dicIliTest);
            //    FileName = GenProfileByIliTestFile(dicIliTest);
            //}
            _NowStation = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation;
            Check_and_Update_Profile(FileName);
            tabControl_TestBtn.Enabled = (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.DeviceRemove) ? false : true;
            m_btRunTest.Enabled = (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.DeviceRemove) ? false : true;
            dataGridView_MainTestItem.Enabled = (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.DeviceRemove) ? false : true;
            if(_NowStation == SensorTestStation.VERIFY_TEST)
                ClearGridView_VerifyTest_Main(VerifyTestFiles.ToList());

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode = enSensorTest.Stop;

            if(Directory.Exists(ITS_Directory.VerifyTestFolder))
                tabPage_BackTesting.Parent = tabControl_SensorTestFunc;
            else
                tabPage_BackTesting.Parent = null;
        }
        //true: 最新的profile false: 非最新
        public bool Check_and_Update_Profile(string FileName)
        {
            INI_Tool MyINI = new INI_Tool(FileName);
            bool ret = true;
            string Mode = MyINI.IniReadValue("OP_Mode", "SensorTestStation", "");
            if (Mode == "FPC_Mode")
                goto END;
                
            string INI_Profile = MyINI.IniReadValue("System", "ProfileVersion", "");
            Version INI_ProfileVer = new Version(INI_Profile);
            if(INI_ProfileVer !=  VersionInfo.ProfileVersion)
            {
                string str = "", str1 = "";
                if ((StaticVar.g_Lang == enLangTable.LANG_TRADITIONAL_CHINESE)
                                    || (StaticVar.g_Lang == enLangTable.LANG_SIMPLIFIED_CHINESE))
                {
                    str = "Profile可以更新到最新的版本，請問你想要更新嗎?";
                    str1 = "原來的profile會備份到 ProfileBackup\\ path.";
                }
                else
                {
                    str = "Update profile version with newer versions, Would you like to update?";
                    str1 = "Old profile will be copied in ProfileBackup\\ path.";
                }
                if (MyUIPage.ShowAskDialog(Application.ProductName, str, UIStyle.Black, true))
                {
                    if (!Directory.Exists("ProfileBackup"))
                        Directory.CreateDirectory("ProfileBackup");
                    File.Copy(FileName, @"ProfileBackup\" + Path.GetFileName(FileName), true);
                    MyUIPage.ShowInfoDialog(Application.ProductName, str1, UIStyle.Blue, true);
                }
                else
                    ret = false;
            }
        END:
            if(ret)
                ChangeProfile(FileName);
            return ret;
        }

        private void ReadIliTestFile(string sFileName, ref Dictionary<string, Dictionary<string, string>> dicIliTest)
        {
            StreamReader srRead = new StreamReader(sFileName);
            string sLine = string.Empty;
            while((sLine = srRead.ReadLine()) != null)
            {
                string[] sTemp = sLine.Split('=');
                if(!dicIliTest.ContainsKey(sTemp[0]))
                {
                    Dictionary<string, string> dicItem = new Dictionary<string, string>();
                    dicItem.Add(sTemp[1], sTemp[2]);
                    dicIliTest.Add(sTemp[0], dicItem);
                }
                else
                {
                    if(!dicIliTest[sTemp[0]].ContainsKey(sTemp[1]))
                        dicIliTest[sTemp[0]].Add(sTemp[1], sTemp[2]);
                    else
                        dicIliTest[sTemp[0]][sTemp[1]] = sTemp[2];
                }
            }
            srRead.Close();
        }

        private string GenProfileByIliTestFile(Dictionary<string, Dictionary<string, string>> dicIliTest)
        {
            return "";
        }

        private void ChangeProfile(string FileName)
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath = FileName;

            FileName = (Path.IsPathRooted(FileName)) ? FileName : ITS_Directory.Tool_CurrentDirectory + FileName;
            string RelativePath = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, FileName);
            SensorTest_Profile.IniWriteValueW("SensorTest_ProfilePath", "Path", RelativePath);

            string strProtocol = "";
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo == null)
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo = new Panel_Info();
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.vfProtocolInit();//2023/07/19 當history.ini的profile path不存在，會導致Var_Multi.DevPanelInfo為null。
            }
            if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.bReadProtocol(FileName, out strProtocol, out UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU))
            {
                if(UI_GBV.fmITS_Tool.ResetProtocol(m_iSelectedDev, strProtocol) == false)
                {
                    if(tabControl_TestBtn.SelectedTab == tabPage_Barcode)
                    {
                        timer_FocusBarcodeText.Start();
                        return;
                    }
                }
            }
            nowFunType = FunType.NULL;
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ResetAllTestContains(enReadFrom.INI);
            //auto gen
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.GenVal_fromIni(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            TestItem_GridInitial();
            TriggerOptionChange();
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.CopyRunningProfile(FileName);
            ChangeAutoGen_UI(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen);

            m_lbProfileName.Text = string.Format("Profile Name : {0}", RelativePath);

            string strToolStart_AutoTest = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "ToolStart_AutoTest");
            strToolStart_AutoTest = string.IsNullOrEmpty(strToolStart_AutoTest) ? "False" : strToolStart_AutoTest;
            StaticVar.bToolStart_AutoTest = Convert.ToBoolean(strToolStart_AutoTest);

            string strToolStart_I2CDetect = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "I2CDetect");
            strToolStart_I2CDetect = string.IsNullOrEmpty(strToolStart_I2CDetect) ? "False" : strToolStart_I2CDetect;
            StaticVar.bI2CDetect = Convert.ToBoolean(strToolStart_I2CDetect);

            //20220719 Steven
            string strToolStart_ShowTestDetail = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "ShowTestDetail");
            strToolStart_ShowTestDetail = string.IsNullOrEmpty(strToolStart_ShowTestDetail) ? "True" : strToolStart_ShowTestDetail;
            StaticVar.bSensorTestShowTestDetail = Convert.ToBoolean(strToolStart_ShowTestDetail);

            //20230327
            string strShowLogWithSpace = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "ShowLogWithSpace");
            strShowLogWithSpace = string.IsNullOrEmpty(strShowLogWithSpace) ? "True" : strShowLogWithSpace;
            StaticVar.bShowLogWithSpace = Convert.ToBoolean(strShowLogWithSpace);

            UI_GBV.fm_SensorTest_Painting = new fmSensorTest_Painting(ref UI_GBV.mDev[m_iSelectedDev].LocalVariable, m_iSelectedDev);

            if(tabControl_TestBtn.SelectedTab == tabPage_Barcode)
                timer_FocusBarcodeText.Start();
            //mp result button
            var mp_button = false;
            bool.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "ShowMPResultButton"), out mp_button);
            btn_ShowMPResult.Visible = mp_button;

            //20220725 Steven
            string show_TestInfoViewer = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "ShowTestInfoViewer");
            if(show_TestInfoViewer == "False")
                ChangeTestInfoViewerVisible(false);
            else
                ChangeTestInfoViewerVisible(true);


            SetResultPic(MultiSenTest_VAR.TestResult.NT);
        }

        private void RunTest_Click()
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.dtStartTime = DateTime.Now;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.Start_ListenInterrupt_API(enInt_InDataType.OTHER_MODE);
            if(StaticVar.bI2CDetect)
                timer_I2CDetect.Enabled = false;
#if !LITE_BUILD
            UI_GBV.fmITS_Tool.ITS_MainTool_splitContainer.Panel1.Enabled = false;
#endif
            tabControl_TestBtn.Enabled = false;
            btn_LoadProfile.Enabled = false;
            btn_ReplayReport.Enabled = false;
            btn_ShowMPResult.Enabled = false;

            tabControl_SensorTest.Enabled = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi._GridView_Cell.strTotleReult_ErrCode = "";
            inProgressValue = 0;

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

            if(_NowStation == SensorTestStation.AUTOGEN)
            {
                if (!doAutoGen_check())
                {
                    tabControl_TestBtn.Enabled = true;
                    btn_LoadProfile.Enabled = true;
                    btn_ReplayReport.Enabled = true;
                    tabControl_SensorTest.Enabled = true;
                    btn_ShowMPResult.Enabled = true;
                    return;
                }

                //if(RunningLogAnalysisTool())
                {
#if !LITE_BUILD
                    UI_GBV.fmITS_Tool.ITS_MainTool_splitContainer.Panel1.Enabled = true;
#endif
                    tabControl_TestBtn.Enabled = true;
                    btn_LoadProfile.Enabled = true;
                    btn_ReplayReport.Enabled = true;
                    tabControl_SensorTest.Enabled = true;
                    btn_ShowMPResult.Enabled = true;
                    SetResultPic(MultiSenTest_VAR.TestResult.NT);
                    TestStatistics.Visible = true;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.AllTestCount.iPassCounts = 0;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.AllTestCount.iFailCounts = 0;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.AllTestCount.iTotalCounts = 0;
                    TestStatistics.Set_NowValue(0, 0, 0);
                    return;
                }
            }

            //====================================
            if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.ThreadFun != null)
            {
                if((UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.ThreadFun.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) == 0)
                {
                    Match rxMatch = Regex.Match(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strTestItemName, @"(Paint - )");
                    if(rxMatch.Success)
                        Painting_Finish();
#if !LITE_BUILD
                    UI_GBV.fmITS_Tool.ITS_MainTool_splitContainer.Panel1.Enabled = true;
#endif
                    return;
                }
            }

            //====================================
            //20220706 Steven Add
            listView_TestInfo.Items.Clear();
            m_LbErrorCode.Text = "";

            TestItem_GridInitial(); // Add to Clean all DataGridVeiw's Record
            
            //TriggerOptionChange();
            dataGridView_MainTestItem.SuspendLayout();
            for(int iRow = 0; iRow < dataGridView_MainTestItem.Rows.Count; iRow++)
            {
                if(_NowStation != SensorTestStation.AUTOGEN
                    || ((string)dataGridView_MainTestItem.Rows[iRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.TestName].Value != UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enGpio_Test)
                    && (string)dataGridView_MainTestItem.Rows[iRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.TestName].Value != UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enPaintingTest)
                    && (string)dataGridView_MainTestItem.Rows[iRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.TestName].Value != UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enP2PTest)
                    && (string)dataGridView_MainTestItem.Rows[iRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.TestName].Value != UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enUniformityTest)
                    && (string)dataGridView_MainTestItem.Rows[iRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.TestName].Value != UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enMicroOpenTest)
                     && (string)dataGridView_MainTestItem.Rows[iRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.TestName].Value != UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enPowerTest)
                     && (string)dataGridView_MainTestItem.Rows[iRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.TestName].Value != UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enSRAMTest)
                     && (string)dataGridView_MainTestItem.Rows[iRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.TestName].Value != UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Judgement.ReturnTestName(TestItemNameDefine.enFWFixed))
                    )
                    dataGridView_MainTestItem.Rows[iRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Value = "";

                dataGridView_MainTestItem.Rows[iRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.ReportLink].Value = "";
                if(iRow != dataGridView_MainTestItem.Rows.Count)
                    dataGridView_MainTestItem.Rows[iRow].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Processbar].Value = 0;
            }
            dataGridView_MainTestItem.Rows[dataGridView_MainTestItem.Rows.Count - 1].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Style.BackColor = Color.White;
            dataGridView_MainTestItem.Rows[dataGridView_MainTestItem.Rows.Count - 1].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.TestName].Value = "Test Result";
            dataGridView_MainTestItem.ResumeLayout(true);

            INI_Tool SettingINI = new INI_Tool(ITS_Directory.Setting_INI);
            //StaticVar.bDisplayFrameData = SettingINI.IniReadBool("SensorTest", "Report_Show_FrameData", false);
            StaticVar.bReportShowResistance = SettingINI.IniReadBool("SensorTest", "Report_Show_Resistance", false);

            //====================================
            // Reload INI
            if(!File.Exists(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath))
            {
                MyUIPage.ShowErrorDialog("Load a profile", string.Format("{0} is not found", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath), UIStyle.Red, true);
                tabControl_TestBtn.Enabled = true;
                btn_LoadProfile.Enabled = true;
                tabControl_SensorTest.Enabled = true;
                return;
            }

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.LoadTestEnable(enReadFrom.INI);

            if (StaticVar._ITSTool_Var != enMode.RDMode)
            {
                int error = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.commonflow_add_dev(string.Format("{0}", m_iSelectedDev));
                if (error < 0)
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.iTotalFailCounts++;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.AllTestCount.iTotalCounts++;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.AllTestCount.iFailCounts++;
                    //MyUIPage.ShowErrorDialog("Error", string.Format("device init. failed, err: {0}", error), UIStyle.Red, true);
                    MyUIPage.ShowErrorTip(string.Format("device init. failed, err: {0}", error), 3000, false);
                    if (StaticVar.EnClientMode != enITS_WakeUpMode.NONE) //Steven Check
                    {
                        var TestResult = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.iTotalFailCounts == (int)MultiSenTest_VAR.TestResult.PASS ? "PASS" : "FAIL";
                        var Barcode = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strBarCodeText;
                        string ErrStr = "";
                        var strPar = string.Format("{0};{1};{2};{3}"
                            , enITS_Params.RESPOND
                            , "TEST"
                            , "DEVICE"
                            , "UNREADY");

                        if (StaticVar.EnClientMode == enITS_WakeUpMode.PIPE_MODE)
                            if (_StreamStr.WriteString(strPar, out ErrStr) == -1)
                                MyUIPage.ShowErrorDialog("Pipe 錯誤提示", ErrStr, UIStyle.Red, false);

                        if (StaticVar.EnClientMode == enITS_WakeUpMode.SOCKET_MODE)
                            SendToSocket(strPar);
                    }

                    tabControl_TestBtn.Enabled = true;
                    btn_LoadProfile.Enabled = true;
                    tabControl_SensorTest.Enabled = true;

                    GenCommonflow_add_dev_ErrorLog(error);

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode = enSensorTest.Stop;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bRunningStatus = false;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_SenTest_All_Finish, (IntPtr)m_iSelectedDev, UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi._GridView_Cell.strTotleReult_ErrCode);

                    return;
                }
            }

            // Start Test
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.MainEntry(enSensorTest.SingleMode);

            //Disable UI
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_SenTest_ModeChange
                , (IntPtr)((int)TouchMode.TestMode)
                , string.Format("{0},{1}", m_iSelectedDev, TouchMode_UI.UI_Only));

            SetResultPic(MultiSenTest_VAR.TestResult.Testing);
            m_btRunTest.Image = global::ITS_Studio.Properties.Resources.Pause_Normal_Red_icon;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.m_bDrawTestStop = false;
        }


        private void GenCommonflow_add_dev_ErrorLog(int iError)
        {
            SensorTestTool _SensorTestTool_ = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_;
            NotStaticVar _LocalVariable = UI_GBV.mDev[m_iSelectedDev].LocalVariable;
            _LocalVariable.VarSenTest_Multi.dtEndTime = DateTime.Now;
            _LocalVariable.VarSenTest_Multi.TotalTestTimeSpand =
                _LocalVariable.VarSenTest_Multi.dtEndTime.Subtract(_LocalVariable.VarSenTest_Multi.dtStartTime).TotalSeconds;

            StringBuilder tmpLog = new StringBuilder();
            tmpLog.AppendLine("==============================================================================");
            tmpLog.AppendFormat("ILITek C-TP Utility {0}: Sensor Test\n", StaticVar.ITS_ToolVer);
            tmpLog.AppendFormat("Customer: V {0}\n", StaticVar.ITS_ToolVer_Customer);
            tmpLog.AppendLine("Confidentiality Notice:");
            tmpLog.AppendLine("Any information of this tool is confidential and privileged.");
            tmpLog.AppendLine("@2024 ILI TECHNOLOGY CORP. All Rights Reserved.");
            tmpLog.AppendLine("==============================================================================");
            tmpLog.AppendFormat("Start_Testing_Time\t\t,{0:yyyy_MM_dd_HH:mm:ss}\n", _LocalVariable.VarSenTest_Multi.dtStartTime);
            tmpLog.AppendFormat("Bar_Code\t\t\t\t,{0}\n", _LocalVariable.VarSenTest_Multi.strBarCodeText);
            tmpLog.AppendFormat("ProFile_Path\t\t\t,{0}\n", _LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            tmpLog.AppendFormat("IC Type\t\t\t\t\t,{0}\n", _SensorTestTool_.GetMainSetValue("IC_Verify", "IC_Type"));
            tmpLog.AppendFormat("IC Channel\t\t\t\t,X:{0} Y:{1}\n"
                , _SensorTestTool_.GetMainSetValue("Panel_Info", "XChannel")
                , _SensorTestTool_.GetMainSetValue("Panel_Info", "YChannel"));
            tmpLog.AppendFormat("Test_Station\t\t\t,{0}\n", _LocalVariable.VarSenTest_Multi.NowStation);
            tmpLog.AppendFormat("TouchKeyChannel\t\t\t,{0}\n", _SensorTestTool_.GetMainSetValue("Panel_Info", "TouchKeyChannel"));
            tmpLog.AppendFormat("X_resolution\t\t\t,{0}\n", _SensorTestTool_.GetMainSetValue("Panel_Info", "X_resolution"));
            tmpLog.AppendFormat("Y_resolution\t\t\t,{0}\n", _SensorTestTool_.GetMainSetValue("Panel_Info", "Y_resolution"));
            tmpLog.AppendFormat("Fingers_counts\t\t\t,{0}\n", _SensorTestTool_.GetMainSetValue("Panel_Info", "Fingers_counts"));
            tmpLog.AppendFormat("ProFile_Version\t\t\t,{0}\n", _SensorTestTool_.GetMainSetValue("System", "ProfileVersion"));
            tmpLog.AppendFormat("ProFile_Date\t\t\t,{0}\n", _SensorTestTool_.GetMainSetValue("System", "Save_Date"));
            tmpLog.AppendFormat("Report_Format_Version\t,{0}\n", "0.0.3.0");
            tmpLog.AppendFormat("CommonFlow Ver.\t\t\t,0x{0:X8}\n", 0x00000300);
            tmpLog.AppendFormat("ProtocolVersion\t\t\t,{0}\n", _SensorTestTool_.GetMainSetValue("System", "ProtocolVersion"));
            tmpLog.AppendFormat("Interface\t\t\t\t,{0}\n", StaticVar.strInterface);
            tmpLog.AppendLine("EDID\t\t\t\t\t,");
            tmpLog.AppendFormat("Test_Result_Code\t\t,{0} ({1})\n", iError, ((Errno)iError).ToString());
            tmpLog.AppendLine("==============================================================================");
            tmpLog.AppendLine("Result_Summary,");
            tmpLog.AppendLine("Due to a critical error, testing cannot proceed.");
            tmpLog.AppendLine("==============================================================================");
            tmpLog.AppendFormat("End_Testing_Time\t\t,{0:yyyy_MM_dd_HH:mm:ss}\n", _LocalVariable.VarSenTest_Multi.dtEndTime);
            tmpLog.AppendFormat("Total_Testing_Time\t\t,{0} sec\n", _LocalVariable.VarSenTest_Multi.TotalTestTimeSpand);
            tmpLog.AppendLine("==============================================================================");
            string strFileNameFormat = _SensorTestTool_.ConvertReportPath();
            using (StreamWriter file = new StreamWriter(strFileNameFormat))
            {
                file.WriteLine(tmpLog.ToString()); // "sb" is the StringBuilder
            }
            if (StaticVar._ITSTool_Var != enMode.Console_Mode)
                _LocalVariable.VarSenTest_Multi.strFinalReportPath = strFileNameFormat;
        }

        private void MP_StressTest()
        {
            ProcessBarStressTest.Maximum = Online_MultiProfiles.Length;
            ProcessBarStressTest.Value = 0;
            int Expected = 0;
            int Unexpected = 0;
            lbExpected.Text = string.Format("Expected : {0}", Expected);
            lbUnexpected.Text = string.Format("Unexpected : {0}", Unexpected);
            var _NowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            foreach(var _Profile in Online_MultiProfiles)
            {
                ChangeProfile(_Profile);
                INI_Tool _INI = new INI_Tool(_Profile);
                var ExpectedResult = _INI.IniReadValue("StressTest", "ExpectedResult", "");
                if(string.IsNullOrEmpty(ExpectedResult))
                    continue;
                tabControl_TestBtn.Enabled = (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.DeviceRemove) ? false : true;
                m_btRunTest.Enabled = (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.DeviceRemove) ? false : true;
                dataGridView_MainTestItem.Enabled = (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.DeviceRemove) ? false : true;

                RunTest_Click();

                do
                {
                    MyTool.Delay_ms(1000);
                } while(!m_btRunTest.Enabled);
                MyUIPage.ShowSuccessTip(string.Format("{0} test done!", Path.GetFileName(_Profile)), 2000);

                //====================================
                //測試結果
                //====================================
                _OutFolderName = string.Format(@"{0}\{1}", ITS_Directory.StressTestReport, _NowTime);
                if(!Directory.Exists(_OutFolderName))
                    Directory.CreateDirectory(_OutFolderName);

                bool bExpected = true;
                foreach(var item in UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult)
                {
                    if(item.Value.iFailCnts < 0)
                        continue;
                    bExpected &= item.Value.iFailCnts == 0;
                }

                var Result = bExpected ? "PASS" : "FAIL";
                if(bExpected)
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        lbExpected.Text = string.Format("{0} : {1}", Result, ++Expected);
                    }));
                else
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        lbUnexpected.Text = string.Format("{0} : {1}", Result, ++Unexpected);
                    }));

                //UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.vfTestDataGridShowResult(FunType.VerifyTest, TableHeaderName._Report, 0, false, Path.GetFileNameWithoutExtension(_LogFile));
                _OutFolderAndResult = string.Format(@"{0}\{1}\", _OutFolderName, Result);
                if(!Directory.Exists(_OutFolderAndResult))
                    Directory.CreateDirectory(_OutFolderAndResult);
                var _path = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.tmpReportLink;
                var NewPath = string.Format(@"{0}{1}", _OutFolderAndResult, Path.GetFileName(_path));
                File.Move(_path, NewPath);

                ////var Result = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.iTotalFailCounts == 0 ? "PASS" : "FAIL";
                //if (ExpectedResult.ToUpper() == Result)
                //    Expected++;
                //else
                //    Unexpected++;

                lbExpected.Text = string.Format("Expected : {0}", Expected);
                lbUnexpected.Text = string.Format("Unexpected : {0}", Unexpected);
                ProcessBarStressTest.Value++;
            }
        }

        private void ReflashGridView_VerifyTest()
        {
            for(int iRow = 0; iRow < GridView_VerifyTest.RowCount; iRow++)
            {
                for(int iCol = 0; iCol < GridView_VerifyTest.ColumnCount; iCol++)
                {
                    if(iCol == 0)
                        continue;
                    GridView_VerifyTest.Rows[iRow].Cells[iCol].Style.BackColor = Color.White;
                    GridView_VerifyTest.Rows[iRow].Cells[iCol].Value = "";
                }
            }
        }

        private void LogVerifyTest(object sender, DoWorkEventArgs e)
        {
            ProcessBarStressTest.Maximum = VerifyTestFiles.Length;
            ProcessBarStressTest.Value = 0;
            int Expected = 0;
            int Unexpected = 0;
            this.Invoke(new MethodInvoker(delegate()
            {
                lbExpected.Text = string.Format("PASS : {0}", Expected);
                lbUnexpected.Text = string.Format("FAIL : {0}", Unexpected);
                ReflashGridView_VerifyTest();
                dataGridView_MainTestItem.Rows.Clear();
            }));

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.ReportLink = new Dictionary<string, string>();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode = enSensorTest.SingleMode;
            var _NowTime = DateTime.Now;

            foreach(var _LogFile in VerifyTestFiles)
            {
                if(!File.Exists(_LogFile))
                    continue;
                //Flag狀態都先設好
                UI_GBV.fmITS_Tool.GetCVS_Protocol = _LogFile;
                string strProtocol = UI_GBV.fmITS_Tool.GetCVS_Protocol;
                if(UI_GBV.fmITS_Tool.ResetProtocol(m_iSelectedDev, strProtocol) == false)
                {
                    e.Result = false;
                    continue;
                }

                this.Invoke(new MethodInvoker(delegate()
                {
                    tabControl_TestBtn.Enabled = false;
                }));
                
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.LoadReport(_LogFile, false);

                string ErrMsg = "";
                if(!UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ResetAllTestContains(enReadFrom.INI, ref ErrMsg))
                {
                    e.Result = false;
                    //this.Invoke(new MethodInvoker(delegate()
                    //{
                    //    dataGridView_MainTestItem.Rows.Clear();
                    //}));
                    throw new Exception(ErrMsg);
                }
                //UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation = SensorTestStation.VERIFY_TEST;                
                this.Invoke(new MethodInvoker(delegate()
                {
                    TestItem_GridInitial();
                    _NowStation = SensorTestStation.VERIFY_TEST;
                }));

                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.LoadTestEnable(enReadFrom.INI);

                //nowFunType = FunType.VerifyTest;
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Start_Test_Init(nowFunType);
                bool bCancel = false;
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.RunTest_Function_Core(nowFunType, Path.GetFileNameWithoutExtension(_LogFile), ref bCancel);
                e.Cancel = bCancel;
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Finish_Test(FunType.VerifyTest, e.Cancel);
                //====================================
                //測試結果
                //====================================
                _OutFolderName = string.Format(@"{0}\{1}", ITS_Directory.VerifyTestFolder, _NowTime.ToString("yyyyMMdd"));
                if(!Directory.Exists(_OutFolderName))
                    Directory.CreateDirectory(_OutFolderName);

                bool bExpected = true;
                foreach(var item in UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ArrTestResult)
                {
                    if(item.Value.iFailCnts < 0)
                        continue;
                    bExpected &= item.Value.iFailCnts == 0;
                }

                this.Invoke(new MethodInvoker(delegate()
                {
                    if(bExpected)
                        lbExpected.Text = string.Format("{0} : {1}", "PASS", ++Expected);
                    else
                        lbUnexpected.Text = string.Format("{0} : {1}", "FAIL", ++Unexpected);
                }));

                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.vfTestDataGridShowResult(FunType.VerifyTest, TableHeaderName._Report, 0, false, Path.GetFileNameWithoutExtension(_LogFile));
                _OutFolderAndResult = string.Format(@"{0}\", _OutFolderName);
                //_OutFolderAndResult = string.Format(@"{0}\{1}\", _OutFolderName, Result);
                if(!Directory.Exists(_OutFolderAndResult))
                    Directory.CreateDirectory(_OutFolderAndResult);
                var _path = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.tmpReportLink;
                var NewPath = string.Format(@"{0}{1}", _OutFolderAndResult, Path.GetFileName(_path));
                File.Move(_path, NewPath);
                //產出報告的超連結
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.ReportLink.Add(Path.GetFileNameWithoutExtension(_LogFile), NewPath);
                this.Invoke(new MethodInvoker(delegate()
                {
                    StatisticsForm.UpdateChart(_LogFile);
                }));

                ProcessBarStressTest.Value++;
            }
            e.Result = true;
        }

        private void LogVerifyTestDone(object sender, RunWorkerCompletedEventArgs _event)
        {
            tabControl_TestBtn.Enabled = true;
            if((bool)(_event.Result) == false)
                MyUIPage.ShowErrorDialog("Report format Error", _event.Error.ToString(), UIStyle.Red, true);
        }

        private void btnRunTest_Click(object sender, EventArgs e)
        {
            switch(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType)
            {
                case DEV_INTERFACE_TYPE.HID:
                case DEV_INTERFACE_TYPE.HID_BL:
                case DEV_INTERFACE_TYPE.WIFI_HID:
                    if(m_TriggerOptions == TriggerMode.GPIO)
                    {
                        MyUIPage.ShowErrorDialog("ERROR", "HID not support GPIO Trigger Mode.", UIStyle.Red, true);
                        if(_CanceGPIO_Token != null)
                            _CanceGPIO_Token.Cancel();
                        return;
                    }
                    break;
            }

            if(Task_GpioScan != null)
                if(!Task_GpioScan.IsCompleted)
                    _CanceGPIO_Token.Cancel();
            if(m_TriggerOptions == TriggerMode.GPIO)
                RecoverGPIO_Pin();
            if(m_TriggerOptions == TriggerMode.Button)
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strBarCodeText = "";
            if(_NowStation == SensorTestStation.STRESS_TEST)//多個Profile對裝置的壓測
            {
                nowFunType = FunType.Test;
                MP_StressTest();
            }
            else if(_NowStation == SensorTestStation.VERIFY_TEST)//多個Report對單一Profile回測
            {
                StatisticsForm.TestStatistics_Reset();
                nowFunType = FunType.VerifyTest;
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker = new BackgroundWorker();
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker.DoWork += new DoWorkEventHandler(LogVerifyTest);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LogVerifyTestDone);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker.WorkerSupportsCancellation = true;
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker.RunWorkerAsync();                
            }
            else
            {
                nowFunType = FunType.Test;
                RunTest_Click();
            }
        }

        private bool doAutoGen_check()
        {
            SensorTestTool s_tool = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_;
            if(string.IsNullOrEmpty(s_tool.m_AutoGen.m_setting.log_path))
            {
                //Disable UI
                tabControl_TestBtn.Enabled = true;
                btn_LoadProfile.Enabled = true;
                btn_ReplayReport.Enabled = true;
                btn_ShowMPResult.Enabled = true;
                tabControl_SensorTest.Enabled = true;
                MyUIPage.ShowErrorTip("Log path is empty");
                return false;
            }
            if(string.IsNullOrEmpty(s_tool.m_AutoGen.m_setting.new_ProfileName))
            {
                //Disable UI
                tabControl_TestBtn.Enabled = true;
                btn_LoadProfile.Enabled = true;
                btn_ReplayReport.Enabled = true;
                btn_ShowMPResult.Enabled = true;
                tabControl_SensorTest.Enabled = true;
                MyUIPage.ShowErrorTip("The new profile name is empty");
                return false;
            }
            if(s_tool.m_AutoGen.m_setting.counts <= 0)
            {
                //Disable UI
                tabControl_TestBtn.Enabled = true;
                btn_LoadProfile.Enabled = true;
                btn_ReplayReport.Enabled = true;
                btn_ShowMPResult.Enabled = true;
                tabControl_SensorTest.Enabled = true;
                MyUIPage.ShowErrorTip("The number of target logs is 0");
                return false;
            }
            /*string curFile = UI_GBV.LogAnalysisTool;
            if (!File.Exists(curFile))
            {
                MyUIPage.ShowErrorTip(UI_GBV.LogAnalysisTool + " was not found.");
                return false;
            }*/
            return true;
        }

        //private bool RunningLogAnalysisTool()
        //{
        //    //check the number of files of collection folder
        //    SensorTestTool s_tool = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_;
        //    if(s_tool.m_AutoGen.m_setting.pass_log_cnts >= s_tool.m_AutoGen.m_setting.counts)
        //    {
        //        if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType != DEV_INTERFACE_TYPE.I2C)
        //        {
        //            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_SenTest_ModeChange, (IntPtr)((int)TouchMode.NormalMode), string.Format("{0},{1}", m_iSelectedDev, TouchMode_UI.CMD_Only));
        //        }
        //        else
        //        {
        //            if(!StaticVar.bTestFinishPwrDown)
        //                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_SenTest_ModeChange, (IntPtr)((int)TouchMode.NormalMode), string.Format("{0},{1}", m_iSelectedDev, TouchMode_UI.CMD_Only));
        //        }
        //        //
        //        string new_Profile = string.Format("{0}\\{1}", s_tool.m_AutoGen.m_setting.log_path, s_tool.m_AutoGen.m_setting.new_ProfileName);
        //        if(new_Profile == UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath)
        //        {

        //            bool myResult = MyUIPage.ShowAskDialog(string.Format("New profile and specified profile are the same file. Do you want to overwrite?"), true);
        //            if(!myResult)
        //            {
        //                new_Profile = string.Format("{0}\\{1}_new.ini", s_tool.m_AutoGen.m_setting.log_path, Path.GetFileNameWithoutExtension(new_Profile));
        //                MyUIPage.ShowSuccessTip(string.Format("New profile name is {0}", Path.GetFileName(new_Profile)), 2000);
        //            }

        //        }
        //        if (s_tool.m_AutoGen.m_setting.addDate)
        //        {
        //            new_Profile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(new_Profile), System.IO.Path.GetFileNameWithoutExtension(new_Profile)) + DateTime.Now.ToString("yyyyMMddHHmmss") + ".ini";
        //        }
        //        //gen new profile;
        //        string _str = "";
        //        if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
        //            _str = global::ITS_Studio.Properties.Resources.ProcessingTip_En;
        //        else
        //            _str = global::ITS_Studio.Properties.Resources.ProcessingTip_Cn;
        //        MyUIPage.ShowWaitForm(_str);

        //        if (_NowStation != SensorTestStation.FPC)
        //        {
        //            double open_thr;
        //            if (fm_Setting.m_LATool.RunAutoGen(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath
        //                , s_tool.m_AutoGen.m_setting.log_path + "\\" + s_tool.m_AutoGen.m_PassFolderName
        //                , new_Profile, "ByNode", out open_thr) == false)
        //            {
        //                MyUIPage.HideWaitForm();
        //                if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
        //                    MyUIPage.ShowErrorDialog("Running Log Analyzer has some problems, please check AutoGen_Log.txt\r\n");
        //                else
        //                    MyUIPage.ShowErrorDialog("執行Log Analyzer 發生了一些技術問題, 請參考AutoGen_Log.txt的錯誤說明\r\n");
        //            }
        //            else
        //            {
        //                MyUIPage.HideWaitForm();
        //                string message = "";
        //                if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
        //                {
        //                    if (open_thr!=0.0)
        //                        message = string.Format("Auto Gen. Function is done. and open threshold is {0}%.\r\nThe new profile is in {1}", open_thr.ToString(), s_tool.m_AutoGen.m_setting.log_path);
        //                    else
        //                        message = string.Format("Auto Gen. Function is done.\r\nThe new profile is in {0}", s_tool.m_AutoGen.m_setting.log_path);
        //                }
        //                else
        //                {
        //                    if (open_thr != 0.0)
        //                        message = string.Format("Auto Gen. 執行完畢, open threshold被更新成{0}%.\r\n產生的新profile在此路徑: {1}", open_thr.ToString(), s_tool.m_AutoGen.m_setting.log_path);
        //                    else
        //                        message = string.Format("Auto Gen. 執行完畢.\r\n產生的新profile在此路徑: {0}", s_tool.m_AutoGen.m_setting.log_path);
        //                }
        //                MyUIPage.ShowSuccessDialog(message, true);
        //                string destname = string.Format("{0}\\{1}\\{2}", s_tool.m_AutoGen.m_setting.log_path, s_tool.m_AutoGen.m_PassFolderName, Path.GetFileName(new_Profile));
        //                File.Copy(new_Profile, destname, true);
        //                //使用新產生的profile取代舊的profile來進行sensor test正常測試。
        //                ChangeProfile(new_Profile);
        //            }
        //        }

        //        if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType != DEV_INTERFACE_TYPE.I2C)
        //        {
        //            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.SuspendMode, Command._SWITCH.Dis);
        //        }
        //        else
        //        {
        //            if(!StaticVar.bTestFinishPwrDown)
        //                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.SuspendMode, Command._SWITCH.Dis);
        //        }
        //        return true;
        //    }

        //    return false;
        //}

        private void m_tmRunButton_Tick(object sender, EventArgs e)
        {
            m_tmRunButton.Enabled = false;
            //m_btLoadPath.Enabled = m_btRunTest.Enabled;
            tabControl_SensorTest.Enabled = true;
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            ((DataGridView)sender).ClearSelection();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.Stop)            
                EnterTestSettings(TestItemNameDefine.enPanel_Info);
        }

        private void EnterTestSettings(TestItemNameDefine SelectItem)
        {
            if(_NowStation != SensorTestStation.FPC)            
                if(Check_and_Update_Profile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath) == false)
                    return;               
            

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
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
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
            //var SelectItem = EnumTool.GetValueFromDescription<TestItemNameDefine>(dataGridView_MainTestItem.Rows[e.RowIndex].Cells[0].Value.ToString());
            fm_Setting.Initial_treeView(SelectItem);
            fm_Setting.ShowDialog(this);

            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bSettingsChange)
            {
                string tmpINI_Profile = "";
                UpdateProfilePath(out tmpINI_Profile);
                if(Path.IsPathRooted(tmpINI_Profile))
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath = tmpINI_Profile;
                else
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath = string.Format(@"{0}{1}", ITS_Directory.Tool_CurrentDirectory, tmpINI_Profile);
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ResetAllTestContains(enReadFrom.INI);
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.GenVal_fromIni(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.CopyRunningProfile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                ChangeAutoGen_UI(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen);
                TestItem_GridInitial();
                TriggerOptionChange();
                SetResultPic(MultiSenTest_VAR.TestResult.NT);
                m_LbErrorCode.Text = "";
            }

            if(tabControl_TestBtn.SelectedTab == tabPage_Barcode)
                textBox_BarCode.Focus();
            else if(tabControl_TestBtn.SelectedTab == tabPage_BarcodeBtn)
                Barcode_2.Focus();
        }

        private void vfCancelAll(string strError)
        {
            if(!UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.use_commonflow)
            {
                if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker != null)
                {
                    if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker.IsBusy)
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker.CancelAsync();

                    while(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker.IsBusy)
                        Application.DoEvents();
                }
            }

            // Clode 0xF0 Monitor
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.Stop)
                return;

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(0);
            int iRowIndex = dataGridView_MainTestItem.Rows.Count - 1;
            if(iRowIndex < 0)
                return;

            dataGridView_MainTestItem.Rows[iRowIndex].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Style.BackColor = Color.Red;
            dataGridView_MainTestItem.Rows[iRowIndex].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Style.Font
                = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridView_MainTestItem.Rows[iRowIndex].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.Status].Value = "Fail";
            dataGridView_MainTestItem.Rows[iRowIndex].Cells[(int)MultiSenTest_VAR.GridView_Cell.ResultTable.ReportLink].Value = strError;

            tabControl_TestBtn.Enabled = true;
            btn_LoadProfile.Enabled = true;
            tabControl_SensorTest.Enabled = true;
            //m_pbResult.Image = global::ITS_Studio.Properties.Resources.Cute_Ball_Stop_icon;
            SetResultPic(MultiSenTest_VAR.TestResult.FAIL);
            m_btRunTest.Image = global::ITS_Studio.Properties.Resources.Play_Pressed_icon;

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_SenTest_ModeChange, (IntPtr)((int)TouchMode.NormalMode), string.Format("{0},{1}", m_iSelectedDev, TouchMode_UI.CMD_and_UI));

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode = enSensorTest.Stop;

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.LoadTestEnable(enReadFrom.INI);

            if(tabControl_TestBtn.SelectedTab == tabPage_Button)
            {
                m_tmRunButton.Enabled = true;
                BarcodeTimer_En(false);
                textBox_BarCode.Enabled = false;
            }
            else if(tabControl_TestBtn.SelectedTab == tabPage_BarcodeBtn)
            {
                BarcodeTimer_En(false);
                timer_FocusBarcodeText.Start();
            }
            else
            {
                textBox_BarCode.Enabled = true;
                BarcodeTimer_En(true);
                m_tmRunButton.Enabled = false;
                timer_FocusBarcodeText.Start();
            }

            if(UI_GBV.fm_SensorTest_Painting.m_bTestRunning)
                Painting_Finish();//暫時DLL不支援 evayn
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


                strSend += string.Format("DEV{0},", rxMatch.Groups[1].Value);
                if ((UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker != null) && (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker.IsBusy))
                {
                    strSend += "is Testing.";
                }
                else
                {
                    var IsDeviceRemove = StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.DeviceRemove;
                    strSend += string.Format("{0};", IsDeviceRemove ? "ERR" : "DONE");
                    if ((parms[3] == enITS_Params.BARCODE.ToString()) && (!string.IsNullOrEmpty(parms[4])))
                    {
                        textBox_BarCode.Text = parms[4];
                        tmpKeyEventArgs = new KeyEventArgs(Keys.Enter);
                        textBox_BarCode_TextChanged(textBox_BarCode);
                        //EnterBarcodeTest(parms[4]);
                    }
                    else
                    {
                        nowFunType = FunType.Test;
                        RunTest_Click();
                    }
                }
            }
            return strSend;
        }

        private string GetAllVersion(string[] parms)
        {
            var Device = parms[1].Split(';');
            string strSend = enITS_Params.DEVICE.ToString() + ";" + parms[2] + ";";

            foreach (var dev in Device)
            {
                if (string.IsNullOrEmpty(dev))
                    continue;
                Match rxMatch = Regex.Match(dev, @"^DEV([0-9]+)");
                int iRowIndex = int.Parse(rxMatch.Groups[1].Value) - 1;


                strSend += string.Format("DEV{0},", rxMatch.Groups[1].Value);
                var IsDeviceRemove = StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.DeviceRemove;
                strSend += string.Format("{0};", IsDeviceRemove ? "ERR" : "DONE");

                string sBridgeFW = "0.0.0.0";
                if (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                {
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();
                    byte[] bridge_ver;
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_BridgeVersion(out bridge_ver);
                    sBridgeFW = string.Empty;
                    for (int i = 0; i < bridge_ver.Length; i++)
                    {
                        if (i == bridge_ver.Length - 1)
                            sBridgeFW += bridge_ver[i].ToString();
                        else
                            sBridgeFW += bridge_ver[i].ToString() + ".";
                    }
                }

                byte[] ret = new byte[0];

                int error = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.commonflow_add_dev(string.Format("{0}", m_iSelectedDev));
                if (error < 0)
                {
                    //MyUIPage.ShowErrorDialog("Error", string.Format("device init. failed, err: {0}", error), UIStyle.Red, true);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.iTotalFailCounts++;
                    if (StaticVar.EnClientMode != enITS_WakeUpMode.NONE) //Steven Check
                    {
                        var TestResult = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.iTotalFailCounts == (int)MultiSenTest_VAR.TestResult.PASS ? "PASS" : "FAIL";
                        var Barcode = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strBarCodeText;
                        string ErrStr = "";
                        var strPar = string.Format("{0};{1};{2}={3};{4}={5}"
                            , enITS_Params.RESPOND
                            , "VERSION"
                            , enCustomFormatMember.Result
                            , "NG"
                            , enCustomFormatMember.Barcode
                            , Barcode);

                        if (StaticVar.EnClientMode == enITS_WakeUpMode.PIPE_MODE)
                            if (_StreamStr.WriteString(strPar, out ErrStr) == -1)
                                MyUIPage.ShowErrorDialog("Pipe 錯誤提示", ErrStr, UIStyle.Red, false);

                        if (StaticVar.EnClientMode == enITS_WakeUpMode.SOCKET_MODE)
                            SendToSocket(strPar);
                    }
                    return strSend;
                }
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.commonflow_get_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.info[m_iSelectedDev]);


                string sPID = "";
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_HID_Info(FW_MODE.AP_MODE, UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum, out ret); //傳入現在的Mode和Protocol Version

                ListViewItem ItemPID = new ListViewItem("PID");
                if (ret != null)
                    sPID = string.Format("{0:X4}", (ret[1] << 8 | ret[0]).ToString("X4"));
                else
                    sPID = string.Format("{0:X4}", StaticVar.lsMain_USBDevInfo[m_iSelectedDev].usPID);

                string sFWVersion = string.Empty;
                for (int i = 0; i < UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.FirmwareVer.Count; i++)
                {
                    if (i != UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.FirmwareVer.Count - 1)
                        sFWVersion += string.Format("0x{0:00}.", UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.FirmwareVer[i]);
                    else
                        sFWVersion += string.Format("0x{0:00}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.FirmwareVer[i]);
                }


                if (StaticVar.EnClientMode != enITS_WakeUpMode.NONE) //Steven Check
                {
                    string ErrStr = "";

                    var strPar = string.Format("{0};{1};{2}={3};{4}={5};{6}={7};{8}={9};{10}={11};{12}={13};{14}={15}"
                        , enITS_Params.RESPOND
                        , "VERSION"
                        , enCustomFormatMember.Result
                        , "OK"
                        , "Bridge Version"
                        , sBridgeFW
                        , "Module Name"
                        , StringTool.ASCIIToString(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.ModuleName.ToArray())
                        , "Firmware Version"
                        , sFWVersion
                        , "AP CRC"
                        , (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode != null) ?
                            "0x" + UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.VerifyCode[0] :
                            "0x00"
                        , "Protocol Version"
                        , string.Format("0x{0:X}.0x{1:X}.0x{2:X}",
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum.Major,
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum.Minor,
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum.Build)
                        , "PID"
                        , "0x" + sPID);

                    if (StaticVar.EnClientMode == enITS_WakeUpMode.PIPE_MODE)
                        if (_StreamStr.WriteString(strPar, out ErrStr) == -1)
                            MyUIPage.ShowErrorDialog("Pipe 錯誤提示", ErrStr, UIStyle.Red, false);

                    if (StaticVar.EnClientMode == enITS_WakeUpMode.SOCKET_MODE)
                        SendToSocket(strPar);
                }
            }
            return strSend;
        }

        private void timer_BarcodeScan_Tick(object sender, EventArgs e)
        {
            timer_BarcodeScan.Stop();
            uiLedDisplay_Count.Text = "0 Sec";
            lb_LenNum.Text = "";

            if(((textBox_BarCode.TextLength) == m_iProfileBarcodeLength) && (m_iProfileBarcodeLength != 0))
            {
                textBox_BarCode.Enabled = false;
                var _value = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.lsMainSetting.Find(x => x.strKey == "BarcodeWaitForTest").strValue;
                var _bValue = true;
                bool.TryParse(_value, out _bValue);
                if(_bValue)
                    WarningFormShow("Tip", "Press Enter");

                //For AUO
                if(b_Chroma_EN)
                    obj.bBusy = true;
                timer_FocusBarcodeText.Stop();
                EnterBarcodeTest(textBox_BarCode.Text);
            }
            else
            {
                MyUIPage.ShowErrorTip("Barcode length not math!", 3000, true);
                textBox_BarCode.Focus();
                textBox_BarCode.Clear();
            }
        }

        public void GpioScanEvent()
        {
            if(_CanceGPIO_Token == null)
                return;
            while(!_CanceGPIO_Token.IsCancellationRequested)
            {
                this.Invoke(new Action(() =>
                {
                    MyTool.Delay_ms(100);
                    //更新UI上面的動畫
                    if(TmpFlag >= uiSymbolLabel_GPIO.Width)
                        TmpFlag = 0;
                    uiSymbolLabel_GPIO.SymbolOffset = new Point(TmpFlag, 0);
                    TmpFlag += 5;

                    /* handling form closing event */
                    if(!UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bUSBMonitor_Opened)
                        return;
                    if(_CanceGPIO_Token.IsCancellationRequested)
                        return;

                    var _Select = StaticVar.gpio_check_list.First(x => x.Value.Item1 == enBridgeGpioUsage.START_PIN);
                    if(!UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.get_bridge_pin(_Select.Key, _Select.Value.Item2))
                        return;

                    if(_CanceGPIO_Token.IsCancellationRequested)
                        return;

                    _CanceGPIO_Token.Cancel();

                    btnRunTest_Click(this, null);
                }));
            }
        }

        private void EnterBarcodeTest(string strBarcode)
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strBarCodeText = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.MakeFilenameValid(strBarcode);
            iTimerCnts = 0;
            btnRunTest_Click(this, null);
        }

        private void timer_AutoTest_Tick(object sender, EventArgs e)
        {
            timer_AutoTest.Enabled = false;

            if(m_TriggerOptions == TriggerMode.Button)
            {
                btnRunTest_Click(this, null);
            }
            else if(m_TriggerOptions == TriggerMode.BarCode)
            {
                //BarcodeTimer_En(true);
                textBox_BarCode.Text = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.ExternalBarcode;
                textBox_BarCode_TextChanged(textBox_BarCode);
            }
        }

        private void timer_I2CDetect_Tick(object sender, EventArgs e)
        {
            bool isDeceted = false;
            timer_I2CDetect.Enabled = false;
            string strProtocolVersion = string.Empty;
            if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.LocalVariable.Var_Multi.bI2CError) //Timer打開就發現I2C Error，可以確定是測試過程中發生            
                if(e_DeviceStatus == DeviceDetectedStatus.TestFinish) //狀態為測試完成，搭配I2C Error得知為測試過程中I2C出問題或是HW Reset                
                    isHWReset = true;

            if(isHWReset) //測試過程中觸發Reset,略過第一筆收的資料。
            {
                if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.Update_ProtocolVersion(ref strProtocolVersion))
                {
                    timer_I2CDetect.Enabled = true;
                    isHWReset = false;
                    return;
                }
                else
                {
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.BridgeSettingInit(600);
                    isDeceted = false;
                    UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.LocalVariable.Var_Multi.bI2CError = false;
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.LocalVariable.Var_Multi.bI2C_NACK_ERROR_RETURN_HOME = false;
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();
                    isHWReset = false;
                    timer_I2CDetect.Enabled = true;
                    return;
                }
            }
            else //正常結束模式
            {
                if(PleaseWait != null)
                {
                    PleaseWait.Close();
                    PleaseWait = null;
                }

                if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.Update_ProtocolVersion(ref strProtocolVersion))
                {
                    //Tool.Delay_ms(100);
                    //讀取到的Protocol Version必須和INI設定相符，才會開始測試
                    isDeceted = true;
                    UI_GBV.mDev[m_iSelectedDev].ReturnProtocolName(); //重新抓回Protocol設定
                }
                else
                {
                    if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.LocalVariable.Var_Multi.bI2CError) //因為讀取到F0導致的失敗
                    {
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.BridgeSettingInit(600);
                        isDeceted = false;
                        if(e_DeviceStatus == DeviceDetectedStatus.TestFinish)
                            e_DeviceStatus = DeviceDetectedStatus.NotDetected;
                        UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.LocalVariable.Var_Multi.bI2CError = false;
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.LocalVariable.Var_Multi.bI2C_NACK_ERROR_RETURN_HOME = false;
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();
                        timer_I2CDetect.Enabled = true;
                        return;
                    }
                    else
                    {
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();
                        timer_I2CDetect.Enabled = true;
                        return;
                    }
                }

                if(isDeceted && e_DeviceStatus == DeviceDetectedStatus.NotDetected)
                    e_DeviceStatus = DeviceDetectedStatus.Detected;
                else
                    timer_I2CDetect.Enabled = true;

                if(e_DeviceStatus == DeviceDetectedStatus.Detected)
                    RunTest_Click();
            }
        }

        private void textBox_BarCode_KeyDown(object sender, KeyEventArgs e)
        {
            tmpKeyEventArgs = e;
            this.Invoke(new Action(() =>
            {
                textBox_BarCode_TextChanged(sender);
            }));
        }

        private void CleanBarcode(object sender)
        {
            UITextBox Barcode = sender as UITextBox;
            Barcode.Enabled = false;
            Barcode.Focus();
            Barcode.Clear();

        }

        private void textBox_BarCode_TextChanged(object sender)
        {
            if(tmpKeyEventArgs == null)
                return;
            UITextBox Barcode = sender as UITextBox;
            Application.DoEvents();
            lb_LenNum.Text = Barcode.TextLength.ToString();

            //長度設為0
            if(m_iProfileBarcodeLength == 0)
            {
                //有機率性的會進入到這裡
                if(tmpKeyEventArgs == null)
                    return;
                if(tmpKeyEventArgs.KeyValue == (Char)_KeyEventArg.ENTER)
                {
                    timer_BarcodeScan.Stop();
                    timer_FocusBarcodeText.Stop();
                    EnterBarcodeTest(Barcode.Text);
                    CleanBarcode(Barcode);
                    tmpKeyEventArgs = null;
                }
            }
            else
            {
                if(Barcode.TextLength == 0)
                    return;
                //=======================
                //偵測到字串輸入，開始倒數計時
                if(!timer_BarcodeScan.Enabled)
                    BarcodeTimer_En(true);

                //=======================
                //檢查Barcode長度與Profile長度有無匹配
                bool bBarcode_FocusEnter = false;
                bool.TryParse(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Report", enReport.Barcode_FocusEnter.ToString()), out bBarcode_FocusEnter);
                if(bBarcode_FocusEnter)
                {
                    if(tmpKeyEventArgs.KeyValue != (Char)_KeyEventArg.ENTER)
                        return;

                    if(Barcode.TextLength != m_iProfileBarcodeLength)
                    {
                        MyUIPage.ShowErrorTip("Barcode length not math!", 1000, true);
                        CleanBarcode(Barcode);
                        Barcode.Enabled = true;
                        lb_LenNum.Text = Barcode.TextLength.ToString();
                        timer_FocusBarcodeText.Start();
                        return;
                    }
                }

                if(Barcode.TextLength != m_iProfileBarcodeLength)
                    return;
                //=======================
                //檢查Barcode格式有無匹配特殊規範
                var NowBarcode = Barcode.Text;
                var strBarcode_SpecifiedFormat = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("Report", enReport.Barcode_SpecifiedFormat.ToString());
                #region Vane測試實驗用
                if(strBarcode_SpecifiedFormat.Contains("{"))
                {
                    string pattern = @"\{[^{}]+\}";
                    string replacedText = Regex.Replace(strBarcode_SpecifiedFormat, pattern, ",");

                    if(replacedText.StartsWith(","))
                        replacedText = "0" + replacedText;
                    if(replacedText.EndsWith(","))
                        replacedText = replacedText + "0";
                    var Arr = replacedText.Split(',').Select(x => int.Parse(x)).ToArray();
                    string pattern2 = @"\{([^}]*)\}";
                    MatchCollection MatchKey = Regex.Matches(strBarcode_SpecifiedFormat, pattern2);

                    if(!CompareMatchCollections(Arr, MatchKey, NowBarcode))
                    {
                        MyUIPage.ShowErrorTip("Barcode format not math!", 1000, true);
                        CleanBarcode(Barcode);
                        Barcode.Enabled = true;
                        lb_LenNum.Text = Barcode.TextLength.ToString();
                        timer_FocusBarcodeText.Start();
                        return;
                    }
                }
                #endregion Vane測試實驗用
                //=======================
                timer_BarcodeScan.Stop();
                timer_FocusBarcodeText.Stop();
                Barcode.Enabled = false;
                iTimerCnts = 0;
                //=======================
                //等待按下Button鍵的功能                
                var _value = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.lsMainSetting.Find(x => x.strKey == "BarcodeWaitForTest").strValue;
                var _bValue = true;
                bool.TryParse(_value, out _bValue);
                if(_bValue)
                    WarningFormShow("Tip", "Press Enter");

                //=======================
                //進入測試
                EnterBarcodeTest(Barcode.Text);
                CleanBarcode(Barcode);
                timer_FocusBarcodeText.Start();
            }
            lb_LenNum.Text = Barcode.TextLength.ToString();
        }

        private bool CompareMatchCollections(int[] TargetPos, MatchCollection TargetKeyword, string InputStr)
        {
            int iCnt = 0;
            var test = InputStr.ToCharArray();
            int iBypass = 0;
            foreach(Match item2 in TargetKeyword)
            {
                iBypass += TargetPos[iCnt];
                var TestStr = new string(test.Skip(iBypass).Take(item2.Groups[1].Length).ToArray());
                iBypass += item2.Groups[1].Length;
                if(TestStr != item2.Groups[1].Value)
                    return false;
                iCnt++;
            }
            return true;
        }

        private void m_btLoadReport_Click(object sender, EventArgs e)
        {
            if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.enable)
            {
                string strReportPath = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.log_path;
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.ChangeCollectionPath(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.path_rule.Desc(), ref strReportPath);
                //將新的log path寫入profile
                if(strReportPath == "")
                    return;
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("Auto_Gen.", "CollectionPath", strReportPath);
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.SetMainSetValue("Auto_Gen.", "PathRule", "Absolute Path");
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.GenVal_fromIni(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
                ChangeAutoGen_UI(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen);
                SetResultPic(MultiSenTest_VAR.TestResult.NT);
            }
            else
            {
                if(dataGridView_MainTestItem.Enabled == false)
                    dataGridView_MainTestItem.Enabled = true;
                string FileName = "";
                //TODO：先確認Profile版本正不正確，會影響Report分析的判斷方式。
                if(!SelectFile("Report (*.csv)|*.csv", ITS_Directory.Tool_CurrentDirectory + "Log", out FileName))
                    return;
                tabPage_BackTesting.Parent = null;
                FileName = (Path.IsPathRooted(FileName)) ? FileName : ITS_Directory.Tool_CurrentDirectory + FileName;
                string RelativePath = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, FileName);
                //Flag狀態都先設好
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode = enSensorTest.SingleMode;
                UI_GBV.fmITS_Tool.GetCVS_Protocol = FileName;
                string strProtocol = UI_GBV.fmITS_Tool.GetCVS_Protocol;
                dataGridView_MainTestItem.Rows.Clear();

                if(UI_GBV.fmITS_Tool.ResetProtocol(m_iSelectedDev, strProtocol) == false)
                    return;

                tabControl_TestBtn.Enabled = false;
                _NowStation = SensorTestStation.REPORT;
                
                if(!UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.LoadReport(FileName, false))
                    throw new Exception(string.Format("Load {0} Fail!", FileName));
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.LoadTestEnable(enReadFrom.Report);
                string ErrMsg = "";
                if(!UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ResetAllTestContains(enReadFrom.Report, ref ErrMsg))
                {
                    MyUIPage.ShowErrorDialog("Report format Error", ErrMsg, UIStyle.Red, true);
                    dataGridView_MainTestItem.Rows.Clear();
                    return;
                }
                TestItem_GridInitial();
                if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.isFPC)
                {
                    //MessageBox.Show("FPC Log is not supported", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MyUIPage.ShowWarningDialog("Note", "FPC Log is not supported", UIStyle.Red, true);
                    if(btn_ShowMPResult.Visible)
                        btn_ShowMPResult.Visible = false;
                    return;
                }

                nowFunType = FunType.ReplayLog;
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.Start_Test_Init(nowFunType);
                bool bCancel = false;
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.RunTest_Function_Core(nowFunType, FileName, ref bCancel);
                //若mpresult的button有顯示，則隱藏
                if(btn_ShowMPResult.Visible)
                    btn_ShowMPResult.Visible = false;
                m_lbProfileName.Text = string.Format("Report Name : {0}", RelativePath);
            }
            m_LbErrorCode.Text = "";
        }

        private void btn_clr_logs_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.log_path);

            foreach(FileInfo file in di.GetFiles())
                file.Delete();

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.pass_log_cnts = 0;
            tb_targetNum.Text = string.Format("{0}/{1}", UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.pass_log_cnts
                , UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.counts);
            tb_autogen_logfrom.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.m_setting.log_path;
        }

        private void SetTitle(SensorTestStation _SensorTestStation)
        {
            switch(_SensorTestStation)
            {
                case SensorTestStation.MODULE:
                    lbStationMode.Style = UIStyle.Green;
                    break;
                case SensorTestStation.FPC:
                    lbStationMode.Style = UIStyle.Red;
                    break;
                case SensorTestStation.AUTOGEN:
                    lbStationMode.Style = UIStyle.Purple;
                    break;
                case SensorTestStation.REPORT:
                    lbStationMode.Style = UIStyle.Blue;
                    break;
                case SensorTestStation.STRESS_TEST:
                    lbStationMode.Style = UIStyle.Black;
                    break;
                case SensorTestStation.VERIFY_TEST:
                    lbStationMode.Style = UIStyle.LayuiRed;
                    break;
                default:
                    lbStationMode.Style = UIStyle.Orange;
                    break;
            }

            lbStationMode.Text = _SensorTestStation.Desc();
        }

        private void SensorTest_Detail_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer_I2CDetect.Enabled = false;
            timer_I2CDetect.Dispose();

            try
            {
                if(PipeBgw != null)
                    PipeBgw.CancelAsync();

                string ErrStr = "";
                if(StaticVar.EnClientMode == enITS_WakeUpMode.PIPE_MODE)
                {
                    if(_StreamStr.WriteString(enITS_Params.DISCONNECT.ToString(), out ErrStr) == -1)
                        MyTool.Save_DBG_Log("[SensorTest_Detail_FormClosing] WriteString {0}", ErrStr);
                }
                //if (StaticVar.EnClientMode == enITS_WakeUpMode.SOCKET_MODE)
                //{
                //    //TODO
                //}
                if(NetConnect != null)
                    NetConnect.Disconnect();
            }
            catch(Exception ex)
            {
                MyTool.Save_DBG_Log("[SensorTest_Detail_FormClosing] {0}", ex.ToString());
            }
        }

        private void GridView_VerifyTest_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var MySender = sender as UIDataGridView;
            string SelectColumn = MySender.Columns[e.ColumnIndex].HeaderText;
            if(e.RowIndex == -1)
                return;
            if(SelectColumn == TableHeaderName._Report.Desc())
            {
                if(MySender.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                    return;
                var _HeaderCell = MySender.Rows[e.RowIndex].Cells[0].Value.ToString();
                Process.Start("notepad.exe", string.Format(@"{0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.ReportLink[_HeaderCell]));
            }
        }

        private void m_btShowMPResult_Click(object sender, EventArgs e)
        {
            if (StaticVar._ITSTool_Var != enMode.RDMode)
            {
                int error = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.commonflow_add_dev(string.Format("{0}", m_iSelectedDev));
                if (error < 0)
                {
                    MyUIPage.ShowErrorDialog("Error", string.Format("device init. failed, err: {0}", error), UIStyle.Red, true);

                    tabControl_TestBtn.Enabled = true;
                    btn_LoadProfile.Enabled = true;
                    tabControl_SensorTest.Enabled = true;
                    return;
                }
            }

            fmShowMPResult = new SensorTest_MPResult();
            DialogResult _Result = fmShowMPResult.ShowDialog();
            if(_Result == DialogResult.Abort)
                MyUIPage.ShowErrorDialog("ERROR", fmShowMPResult.ErrorMessage, UIStyle.Red, true);

            if (StaticVar._ITSTool_Var != enMode.RDMode)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.commonflow_remove_dev();
        }

        private void Set_tabControl_TestBtnEnabled(bool bEn)
        {
            tabControl_TestBtn.Enabled = bEn;
            btn_ShowMPResult.Enabled = bEn;
        }

        private bool TrigerTest_API(params object[] Parameter)
        {
            var para1 = (TriggerMode)Parameter[0];
            if(para1 == TriggerMode.Button)
                btnRunTest_Click(this, null);
            else if(para1 == TriggerMode.BarCodeButton)
                btnBarcodeBtn_Click(this, null);
            else
                return false;

            return true;
        }

        private void btnBarcodeBtn_Click(object sender, EventArgs e)
        {
            EnterBarcodeTest(Barcode_2.Text);
            ClearBarcode_API();
            Barcode_2.Focus();
        }

        private bool ClearBarcode_API(params object[] Parameter)
        {
            Barcode_2.Clear();
            return true;
        }

        private void Barcode_2_TextChanged(object sender, EventArgs e)
        {
            var Mysender = sender as UITextBox;

            userControl_LabelTextBox1.NumberText = Mysender.TextLength;
        }

        private void Barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
        }

        private void textBox_BarCode_Enter(object sender, EventArgs e)
        {
            isBarCodeTextBoxFocus = true;
        }

        private void textBox_BarCode_Leave(object sender, EventArgs e)
        {
            isBarCodeTextBoxFocus = false;
        }

        public void SensorTest_FormClose()
        {
            if(m_TriggerOptions == TriggerMode.GPIO)
                this._CanceGPIO_Token.Cancel();
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            StatisticsForm.Show();
        }
        private SensorTestStation _NowStation
        {
            set
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation != value)
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation = value;
                SetTitle(value);
                if(value == SensorTestStation.AUTOGEN)                
                    tabControl_SensorTestFunc.SelectedTab = tabPage_AutoGen;                
            }
            get
            {
                return UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation;
            }
            //tabControl_TestResult.Visible = true;
        }
        private void btn_LoadReports_Click(object sender, EventArgs e)
        {
            if(_NowStation == SensorTestStation.VERIFY_TEST)
            {
                btn_ReplayReport.Enabled = true;
                string ErrMsg = "";
                UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ResetAllTestContains(enReadFrom.INI, ref ErrMsg);//Reload Profile
                tabControl_TestResult.SelectedTab = tabPage_TestStatistics;
                btn_ShowMPResult.Enabled = StaticVar.bOPModeShowMPResult;
                btn_ShowMPResult.Visible = StaticVar.bOPModeShowMPResult;
                _NowStation = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation;
                tabControl_SensorTest.SelectedTab = tabPage_SensorTestMain;
                //回到預設狀態
                btn_LoadReports.Text = "Enter Backtesting Mode";
                btn_LoadReports.Symbol = 61584;
                btn_LoadReports.Style = UIStyle.Green;
            }
            else
            {
                using(var fbd = new FolderSelectDialog())
                {
                    if(string.IsNullOrEmpty(fbd.DirectoryPath))
                        fbd.DirectoryPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Log";

                    if(fbd.ShowDialog(this) != DialogResult.OK)
                        return;
                    _NowStation = SensorTestStation.VERIFY_TEST;
                    tabControl_TestResult.SelectedTab = tabPage_TestExpected;
                    var ext = new List<string> { "csv" };
                    VerifyTestFiles = Directory.GetFiles(fbd.DirectoryPath, "*.*", SearchOption.AllDirectories).Where(s => ext.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant())).ToArray();
                    ClearGridView_VerifyTest_Main(VerifyTestFiles.ToList());
                    //bIsVerifyTest = true;
                    tabControl_TestBtn.Enabled = true;
                    tabControl_TestBtn.SelectedTab = tabPage_Button;
                    tabControl_SensorTest.SelectedTab = tabPage_VerifyTest;
                    //清除其他的狀態
                    Online_MultiProfiles = null;
                    btn_ReplayReport.Enabled = false;
                    btn_ShowMPResult.Enabled = false;
                    m_btRunTest.Enabled = true;
                    //進入回測狀態
                    btn_LoadReports.Text = "Exit Backtesting Mode";
                    btn_LoadReports.Symbol = 61579;
                    btn_LoadReports.Style = UIStyle.Gray;
                }
            }
        }

        private void btn_LoadProfiles_Click(object sender, EventArgs e)
        {
            using(var fbd = new FolderSelectDialog())
            {
                //if(fbd.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(fbd.DirectoryPath))
                if(fbd.ShowDialog(this) == DialogResult.OK)
                {
                    //Mysender.UnSelectAll();
                    //UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation = SensorTestStation.STRESS_TEST;
                    _NowStation = SensorTestStation.STRESS_TEST;
                    tabControl_TestResult.SelectedTab = tabPage_TestExpected;
                    var ext = new List<string> { "ini" };
                    Online_MultiProfiles = Directory.GetFiles(fbd.DirectoryPath, "*.*", SearchOption.AllDirectories).Where(s => ext.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant())).ToArray();
                    //bIsOnline_MultiProfile = true;
                    //Mysender.SetItemCheckState(index, true);
                    //清除其他的狀態
                    //bIsVerifyTest = false;
                    VerifyTestFiles = null;
                }
                else
                {
                    _NowStation = SensorTestStation.MODULE;
                    //Mysender.SetItemCheckState(index, false);
                    //UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.NowStation = SensorTestStation.MODULE;
                    tabControl_TestResult.SelectedTab = tabPage_TestStatistics;
                    Online_MultiProfiles = null;
                    //bIsOnline_MultiProfile = false;                    
                }
            }
        }
    }
}