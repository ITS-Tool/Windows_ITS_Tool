using CommonExt;
using FuncMethodCmd;
using Ilitek_HID;
using ILITek_ITS_Tool_Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ThirdPartyTools;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Globalization;
using Sunny.UI;
using FuncMethodCmd.Tools.ProcessorTools;
using ErrorMessage;
using ITS_Studio.AllForms.Tuning;

namespace ITS_Studio.AllForms
{
    public partial class ILITek_ITS_Tool : Form
    {
        enum WIN_API : int
        {
            SW_HIDE = 0,
            SW_NORMAL = 1,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
        }

        private delegate void ModeSelectFun(enPROTOCOL _protocol);

        public enum ICTableColumn : int
        {
            UI_ShowName = 0,
            Protocol,
            Interface,
        }

        public enum AutoUpdaterForm : int
        {
            Updater = 0,
            Downloader = 1,
        }

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

        public static BridgeSetting fm_BridgeSetting;
        public static CDCToolfm fm_CDCTool;
        public static Paintfm fm_PaintTool;
        public static WifiSettingfm fm_WifiSetting;
        public static C_Modelfm fm_C_Model;
        public static C_Modelfm_Replay fm_C_Model_Replay;

        private Consolefm fm_ConsoleTool;
        private FirmwareUpgradefm fm_FWUpgrade;
        private TuningToolfm fm_TuningTool;
        private AutoScanCHfm fm_AutoScanCH_Tool;
        private NoiseFrefm fm_NoiseFre;
        private TuningComparefm fm_TuningCompare;
        private ChargeCurvefm fm_ChargeCurve;
        private GenProfileFm fm_GenProfile;
        private fmPleaseWait PleaseWait;
        private NumberForm fm_NumberForm;
        private fmExternalPaint ExternalPaintForm;
        private SensorTest_Detail fm_SensorTest_D;
        private SensorTest_Multi fm_SensorTest_S;
        private SensorTest_FPC_Open fm_SensorTest_FPC_Open;

        private ExtendFlowLayout fm_ExtendFlowLayout;

        //private LegalNotice fm_LegalNotice;

        private Dictionary<enWM_MSG, function> mMessageReceiver = new Dictionary<enWM_MSG, function>();
        private Dictionary<TouchMode, ModeSelectFun> ModeSelect = new Dictionary<TouchMode, ModeSelectFun>();

        private bool b_showMPResult = false;
        private bool b_showCurrent = false;
        private bool b_Chroma_EN = false;		//For AUO
        private INI_Tool MY_INI = new INI_Tool(ITS_Directory.Setting_INI);

        private ToolStripItem[] ToolStripItem_Dev;
        private string ToolVersion;

        private UIStyleManager StyleManager;
        private UIPage MyUIPage = new UIPage();
        public bool CursorShown = true;
        public ILITek_ITS_Tool(params object[] param)
        {
            Directory.SetCurrentDirectory(ITS_Directory.Tool_CurrentDirectory);
            InitializeComponent();

            StaticVar.vendor = CustomID.Ilitek;
            //For AUO
            if(File.Exists(ITS_Directory.CsvSetting))
            {
                StaticVar.vendor = CustomID.AUO;
                b_Chroma_EN = true;
            }

            this.Text += GetAppVersion(StaticVar.vendor);

            StaticVar.ITS_ToolVer = Application.ProductVersion;
            SubscribeMessageReceiver();
            SubscribeModeSelectFunction();//支援Normal Mode、Debug Mode、Test Mode、Playback Mode

            if(IsToolProcessing("iUniTouch"))
            {
                MyUIPage.ShowWarningDialog("系统提示", "iUniTouch正在執行中...", UIStyle.Red, true);
            }

            string str = MY_INI.IniReadValue("Main", "Mode", enMode.RDMode.ToString());
            StaticVar._ITSTool_Var = (enMode)Enum.Parse(typeof(enMode), str);
            if(StaticVar._ITSTool_Var != enMode.RDMode)
            {
                if(MutexCheckApp(Application.ExecutablePath) > 1)
                {
                    if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                        MyUIPage.ShowErrorDialog("System Tips", "The application is already running...", UIStyle.Red, true);
                    else
                        MyUIPage.ShowErrorDialog("系统提示", "應用程式已經在執行中...", UIStyle.Red, true);
                    Environment.Exit(1);
                }
            }
            //testw.Restart();
            str = MY_INI.IniReadValue("DEVICE_ID", "VID", "222a").Trim().ToUpper().Replace("0X", "");
            ushort[] VID_Arr = str.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToUInt16(x, 16)).ToArray();
            for(int iCnt = 0; iCnt < StaticVar.iMaxSupportDev; iCnt++)
                UI_GBV.mDev[iCnt] = new CtrlILIDevice(iCnt, VID_Arr, StaticVar.bridgeSetting.m_bridge_var);
            InitDeviceButtons(StaticVar.iMaxSupportDev);
            //Console.WriteLine("\t[ILITek_ITS_Tool] new CtrlILIDevice {0} ms", testw.ElapsedMilliseconds);

            // Set Form Handle To Globe Variable
            StaticVar.MainFormHandle = this.Handle;
            StaticVar.MainFormParent = this.FindForm();


            //跟子視窗同步熱鍵，要將這些設true才有作用
            this.KeyPreview = true;
            this.IsMdiContainer = true;
            //testw.Restart();
            //先在這裡列舉螢幕
            DisplayClass _class = new DisplayClass();
            StaticVar.lsScreenNames = _class.GetDeviceManagerID().ToList();
            StaticVar.lsAllScreenInfo = _class.PairingScreenInfo(StaticVar.lsScreenNames);

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_UsbDeviceNotification, (IntPtr)m_iSelectedDev, StaticVar.MainFormHandle);
            NewForms();

            // Add Console Form To Table_Console
            SetFormProperty(fm_ConsoleTool, tabPage_Console, true);

            // Add CDC Form To Table_CDC
            SetFormProperty(fm_CDCTool, tabPage_CDCTool, true);

            // Add Painting Form To Table_painting
            SetFormProperty(fm_PaintTool, tabPage_PaintTool, true);

            // Add FWUpgrade Form To Table_FWUpgrade
            SetFormProperty(fm_FWUpgrade, tabPage_FWUpgrade, true);

            // Add Bridge Setting Form To Table_Console
            SetFormProperty(fm_BridgeSetting, m_panBridgeSetting, true);

            fm_BridgeSetting.Load_INI_Settings();

            // Add C model Form To Table_Console
            SetFormProperty(fm_C_Model, tabPage_CModel, true);

            // Add C model Replay Form To Table_Console
            SetFormProperty(fm_C_Model_Replay, tabPage_CModel_Replay, true);
            
            // Add Tuning Tool Form To Table_Tuning
            SetFormProperty(fm_TuningTool, tabPage_Tuning, true);

            // Add AutoTuning Tool Form To Table_Tuning
            SetFormProperty(fm_AutoScanCH_Tool, tabPage_Tuning, false);

            // Add NoiseFre Form To Table_Tuning
            SetFormProperty(fm_NoiseFre, tabPage_Tuning, false);

            // Add TuningCompare Form to Table_Tuning
            SetFormProperty(fm_TuningCompare, tabPage_Tuning, false);

            // Add TuningCompare Form to Table_Tuning
            SetFormProperty(fm_ChargeCurve, tabPage_Tuning, false);

            // Add SensorTest Form to Table_SensorTest
            SetFormProperty(fm_SensorTest_D, tabPage_SensorTest_D, true);

            // Add SensorTest Simple Form to Table_SensorTest_S
            SetFormProperty(fm_SensorTest_S, tabPage_SensorTest_S, true);
            //Console.WriteLine("\t[ILITek_ITS_Tool] fm_SensorTest_S {0} ms", testw.ElapsedMilliseconds);

            SetFormProperty(fm_GenProfile, tabPage_Tuning, false);

            // Init Tool Button
            Assign_UI_Btn();

            // Set Console Message (richTB_ConsoleMesg) Handle to Globe Variable
            UI_GBV.ToolMain.m_RichText_INFO_MSG = this.richTB_ConsoleMesg;
            UI_GBV.ToolMain.m_RichText_ERR_MSG = this.richTextBox_ErrMSG;
            UI_GBV.ToolMain.lbMax_Value = this.lb_Mutual_Max;
            UI_GBV.ToolMain.lbMin_Value = this.lb_Mutual_Min;
            UI_GBV.ToolMain.lbDelta_Value = this.lb_Mutual_Delta;

            // Hide m_tabControl Appearance
            m_tabControl.Appearance = TabAppearance.FlatButtons;
            m_tabControl.ItemSize = new Size(0, 1);
            m_tabControl.SizeMode = TabSizeMode.Fixed;
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                UILocalizeHelper.SetEN();
            else
                UILocalizeHelper.SetCH();

            fm_BridgeSetting.Enable_rTextBox_Current(StaticVar.bridgeSetting.m_bridge_var.bCurrentDetectEn);

            fm_ExtendFlowLayout.HexToIliClickEvent += m_btHexToIli_Click;
            fm_ExtendFlowLayout.HexToBinClickEvent += m_btHexToBin_Click;
            fm_ExtendFlowLayout.FrequencySpectrumClickEvent += btn_FreSpectrum_Click;
            fm_ExtendFlowLayout.ChargeCurveClickEvent += btnChargDump_Click;
            fm_ExtendFlowLayout.CModelClickEvent += m_btCModel_Click;
            fm_ExtendFlowLayout.EncryptClickEvent += fm_ExtendFlowLayout_EncryptClickEvent;
            fm_ExtendFlowLayout.GenProfileClickEvent += btnGenProfile_Click;
            menutool_autoUpdater.Checked = MY_INI.IniReadBool("Main", "AutoUpdate_Enable", false);

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.ExternalBarcode = "";
            StaticVar.EnClientMode = enITS_WakeUpMode.NONE;
            if(param.Length != 0)
            {
                if(param[0].ToString() == enITS_WakeUpMode.AUTO_BARCODE_MODE.ToString())
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.ExternalBarcode = param[1].ToString();
                if(param[0].ToString() == enITS_WakeUpMode.PIPE_MODE.ToString())
                    StaticVar.EnClientMode = enITS_WakeUpMode.PIPE_MODE;
                if(param[0].ToString() == enITS_WakeUpMode.SOCKET_MODE.ToString())
                    StaticVar.EnClientMode = enITS_WakeUpMode.SOCKET_MODE;
            }
            //若為特殊版，現階段會將download tool給隱藏起來
            if(StaticVar.vendor != CustomID.Ilitek)
                toolStripDropDownButton1.Visible = false;

            GetPacketSizeFromSetting();
            //Console.WriteLine("\t[ILITek_ITS_Tool] others {0} ms", testw.ElapsedMilliseconds);
            //testw.Stop();

        }

        private void GetPacketSizeFromSetting()
        {
            string sKey = MY_INI.IniReadValue("Main", "Report_Size", "PacketSz_1024_");

            foreach(PacketSz item in Enum.GetValues(typeof(PacketSz)))
            {
                if(item.ToString().Equals(sKey))
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.iFW_PrePacketSize = item;
            }
        }

        public ILITek_ITS_Tool()
            : this(new object[0])
        {
        }

        private bool IsToolProcessing(string keyword)
        {
            foreach(Process process in Process.GetProcesses())
            {
                if(process.ProcessName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    //Console.WriteLine($"Found: {process.ProcessName} (PID: {process.Id})");
                    return true;
                }
            }
            return false;
        }

        private int MutexCheckApp(string ExePath)
        {
            //var ProcessName = FileVersionInfo.GetVersionInfo(Application.ExecutablePath).InternalName;
            var ProcessName = FileVersionInfo.GetVersionInfo(ExePath).InternalName;
            var ProcessName_Ex = Path.GetFileNameWithoutExtension(ProcessName);

            Process[] processcollection = Process.GetProcessesByName(ProcessName_Ex);
            //foreach(var item in processcollection)
            //    MessageBox.Show(string.Format("{0} count={1}", item.ProcessName, item.Id));
            //return (processcollection.Length < 2);
            return processcollection.Length;
        }

        private void InitDeviceButtons(int iMaxSupportDev)
        {
            ToolStripItem_Dev = new ToolStripItem[iMaxSupportDev];
            DeviceButtons.DropDownItems.Clear();
            //DeviceButtons.Text = "";
            for(int i = 0; i < iMaxSupportDev; i++)
            {
                var Device = new ToolStripMenuItem();
                Device.Checked = false;
                Device.CheckOnClick = true;
                Device.CheckState = CheckState.Unchecked;
                Device.Visible = false;
                //Device.Name = string.Format("Device{0}", i + 1);
                Device.Size = new System.Drawing.Size(152, 22);
                Device.Text = Device.Name;

                Device.CheckedChanged += new System.EventHandler(OnCheckedChanged);
                ToolStripItem_Dev[i] = Device;
            }
            DeviceButtons.DropDownItems.AddRange(ToolStripItem_Dev);
        }

        private void ChangeDeviceButtons(USBHID_DATA Item, CheckState _CheckState, bool bVisible)
        {
            if(StaticVar._ITSTool_Var == enMode.RDMode)
            {
                var Device = (ToolStripMenuItem)ToolStripItem_Dev[Item.UI_Idx];
                Device.Checked = (_CheckState == CheckState.Checked);
                Device.Enabled = !Device.Checked;
                Device.CheckState = _CheckState;

                Device.Visible = bVisible;
                Device.Name = Item.Port_Info.Port_Chain;
                Device.Text = Device.Name;
                ToolStripItem_Dev[Item.UI_Idx] = Device;
                if(Device.Checked)
                    DeviceButtons.Text = Item.Port_Info.Port_Chain;
            }
        }

        private void OnCheckedChanged(object sender, EventArgs e)
        {
            var MyOwn = sender as ToolStripMenuItem;
            //Console.WriteLine("CheckState = {0} Checked = {1}", MyOwn.CheckState, MyOwn.Checked);
            if(!MyOwn.Checked || MyOwn.GetCurrentParent() == null)
                return;

            //if (MyOwn.GetCurrentParent() == null)
            //    return;
            MyOwn.CheckedChanged -= OnCheckedChanged;

            // Clear the checked state for all siblings.
            foreach(ToolStripItem item in ToolStripItem_Dev)
            {
                var radioItem = item as ToolStripMenuItem;
                radioItem.Checked = (radioItem.Name == MyOwn.Name) ? true : false;
                radioItem.Enabled = !radioItem.Checked;
                if(radioItem != null && radioItem.Checked)
                {
                    var Selected = MyOwn.GetCurrentParent().Items.IndexOf(radioItem);
                    Set_SelectedDev(Selected);
                    DeviceButtons.Text = MyOwn.Text;
                }
            }
            MyOwn.CheckedChanged += OnCheckedChanged;
            Reset_UI();
        }

        private void Set_SelectedDev(int iDev)
        {
            //Console.WriteLine("Set_SelectedDev = {0}", iDev);
            this.SelectedDev = iDev;
            fm_BridgeSetting.SelectedDev = iDev;
            fm_CDCTool.SelectedDev = iDev;
            fm_PaintTool.SelectedDev = iDev;
            fm_ConsoleTool.SelectedDev = iDev;
            fm_FWUpgrade.SelectedDev = iDev;
            fm_SensorTest_D.SelectedDev = iDev;
            fm_SensorTest_D.fm_Setting.SelectedDev = iDev;
            fm_AutoScanCH_Tool.SelectedDev = iDev;
            fm_TuningTool.SelectedDev = iDev;
            fm_NoiseFre.SelectedDev = iDev;
            fm_TuningCompare.SelectedDev = iDev;
            fm_ChargeCurve.SelectedDev = iDev;
            ExternalPaintForm.SelectedDev = iDev;
            
            fm_C_Model.SelectedDev = iDev;
            
        }

        #region USB devices GUID
        //Human Interface Device (HID)/人機界面設備	{4d1e55b2-f16f-11cf-88cb-001111000030}
        //USB Raw Device/USB設備	{a5dcbf10-6530-11d2-901f-00c04fb951ed}
        //private const int DbtDevtypDeviceinterface = 5;

        public enum DBTDEVTYP : int
        {
            DBT_DEVTYP_OEM = 0x00,                //OEM-defined device type
            DBT_DEVTYP_DEVNODE = 0x01,            //Devnode number
            DBT_DEVTYP_VOLUME = 0x02,             //Logical volume
            DBT_DEVTYP_PORT = 0x03,               //Serial, parallel
            DBT_DEVTYP_NET = 0x04,                //Network resource
            DBT_DEVTYP_DEVICEINTERFACE = 0x05,    //Device interface class
            DBT_DEVTYP_HANDLE = 0x06              //File system handle
        }

        private readonly Guid GuidDevinterfaceUSBDevice = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

        [DllImport("user32.dll")]
        private static extern bool UnregisterDeviceNotification(IntPtr handle);

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [Flags]
        public enum DEVICE_NOTIFY : uint
        {
            DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000,
            DEVICE_NOTIFY_SERVICE_HANDLE = 0x00000001,
            DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 0x00000004
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct DevBroadcastDeviceinterface
        {
            internal int Size;
            internal int DeviceType;
            internal int Reserved;
            internal Guid ClassGuid;
            internal short Name;
        }

        private void RegisterDeviceNotification(IntPtr windowHandle)
        {
            //Console.WriteLine("{0} >> [RegisterUsbDeviceNotification]\r\n", Tool.GetParentInfo());            
            DevBroadcastDeviceinterface dbi = new DevBroadcastDeviceinterface
            {
                DeviceType = (int)DBTDEVTYP.DBT_DEVTYP_DEVICEINTERFACE,
                Reserved = 0,
                ClassGuid = GuidDevinterfaceUSBDevice,
                Name = 0
            };
            dbi.Size = Marshal.SizeOf(dbi);

            IntPtr buffer = Marshal.AllocHGlobal(dbi.Size);
            Marshal.StructureToPtr(dbi, buffer, true);

            //m_NotificationHandle[iDev] = RegisterDeviceNotification(windowHandle, buffer, 0);
            if(m_NotificationHandle == IntPtr.Zero)
                m_NotificationHandle = RegisterDeviceNotification(windowHandle, buffer, (int)DEVICE_NOTIFY.DEVICE_NOTIFY_WINDOW_HANDLE);
        }

        //private IntPtr[] m_NotificationHandle = new IntPtr[StaticVar.iMaxSupportDev];
        private IntPtr m_NotificationHandle = IntPtr.Zero;
        private void OnRegisterUsbDeviceNotification(ref Message msg)
        {
            //var RegisteFormHandle = (IntPtr)msg.WParam;
            int iDev = (int)msg.WParam;
            var RegisteFormHandle = (IntPtr)msg.LParam;
            if(RegisteFormHandle == IntPtr.Zero)
            {
                UnregisterDeviceNotification(m_NotificationHandle);
                m_NotificationHandle = IntPtr.Zero;
                Console.WriteLine("Un-Register Device Notification_ID{0}", iDev);//這裡不能拿掉，不然壓測沒辦法判斷到
            }
            else
            {
                RegisterDeviceNotification(RegisteFormHandle);
                Console.WriteLine("Register Device Notification_ID{0}", iDev);//這裡不能拿掉，不然壓測沒辦法判斷到
            }
        }

        #endregion USB devices GUID

        private void NewForms()
        {
            fm_BridgeSetting = new BridgeSetting();
            fm_CDCTool = new CDCToolfm();
            fm_PaintTool = new Paintfm();
            fm_ConsoleTool = new Consolefm();
            fm_FWUpgrade = new FirmwareUpgradefm();
            fm_SensorTest_D = new SensorTest_Detail();
            fm_SensorTest_S = new SensorTest_Multi();
            //Console.WriteLine("\t[NewForms] new SensorTest_Multi {0} ms", testw.ElapsedMilliseconds);

            //testw.Restart();
            fm_AutoScanCH_Tool = new AutoScanCHfm();
            fm_TuningTool = new TuningToolfm();
            fm_NoiseFre = new NoiseFrefm();
            fm_TuningCompare = new TuningComparefm();
            fm_ChargeCurve = new ChargeCurvefm();
            PleaseWait = new fmPleaseWait();
            ExternalPaintForm = new fmExternalPaint();
            fm_ExtendFlowLayout = new ExtendFlowLayout();
            fm_C_Model = new C_Modelfm();
            fm_C_Model_Replay = new C_Modelfm_Replay();
            fm_NumberForm = new NumberForm();
            fm_GenProfile = new GenProfileFm();
        }

        private string GetCustomerVer(string name)
        {
            string[] words = name.Split(',');
            int index = words[1].IndexOf("Version=") + 8;
            string tmp = words[1].Substring(index);
            //words = tmp.Split('.');
            //int high_byte = Convert.ToByte(words[2], 16);
            //int low_byte = Convert.ToByte(words[3], 16);
            //return string.Format("{0}.{1}", high_byte.ToString(), low_byte.ToString());
            return tmp;
        }

        private string GetAppVersion(CustomID vendor)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            ToolVersion = fvi.ProductVersion;
            DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

            var vendor_list = GetCustomerVer(assembly.FullName).Split('.');
            var custom = vendor.Desc();
            StaticVar.ITS_ToolVer_Customer = String.Format("{0}.{1}.{2}", custom, vendor_list[2], vendor_list[3]);
#if (RELEASE)
            if (custom == vendor.ToString() || (vendor == CustomID.Ilitek))
                return string.Format("_{0}_RELEASE", ToolVersion);
            else
                return string.Format("_{0}_RELEASE [V{1}.{2}.{3}]", ToolVersion, custom, vendor_list[2], vendor_list[3]);
#else
            if(custom == vendor.ToString() || (vendor == CustomID.Ilitek))
                return string.Format("_{0}_Beta_{1}", ToolVersion, buildDate);
            else
                return string.Format("_{0}_Beta_[V{1}.{2}.{3}]_{4}", ToolVersion, custom, vendor_list[2], vendor_list[3], buildDate);
#endif
        }

        private void SetFormProperty(object _form, object _Controls, bool _display)
        {
            var form = _form as Form;
            if(form == null)
                return;
            form.TopLevel = false;
            form.Size = new System.Drawing.Size(1, 1);
            form.Dock = DockStyle.Fill;
            Control ctl = null;
            if(_Controls != null)
            {
                if(_Controls is TabPage)
                    ctl = _Controls as TabPage;
                else if(_Controls is Panel)
                    ctl = _Controls as Panel;

                if(ctl != tabPage_Tuning)
                    ctl.Controls.Clear();

                ctl.Controls.Add(form);
            }

            if(_display)
                form.Show();
            else
                form.Hide();

            if(_Controls != null)
                form.Parent = ctl;
        }

        private void MainButton_Enable(DevStatus status)
        {
            Color Disable_Color = BtnColor[(int)SHOW.DISABLE];
            Color Enable_Color = BtnColor[(int)SHOW.ENABLE];
            switch(status)
            {
                case DevStatus.Connected:
                    Wifi_checkBox_.Enabled = false;
                    Wifi_ip_address.Enabled = false;

                    ToolStripBar_Main.Enabled = true;
                    if(fm_SensorTest_D != null)
                    {
                        fm_SensorTest_D.fm_Setting.btPanelInfoLoad.Enabled = true;
                        fm_SensorTest_D.fm_Setting.btn_CaptureFW_CRC.Enabled = true;
                    }
                    m_tbgSensorTest.Enabled = true;

                    if(!Wifi_Var.useWifi)
                        m_panBridgeSetting.Enabled = false;
                    btn_Connect.Enabled = true;
                    btn_Connect.Image = Properties.Resources.connect;
                    btn_Connect.Text = "Connect";
                    DeviceButtons.Enabled = false;
                    var _FW_Mode = UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode;
                    Set_UI_BottonsMode(_FW_Mode);
                    //fm_ExtendFlowLayout.SetDeviceConnection(status);
                    break;

                case DevStatus.DeviceRemove:
                case DevStatus.Disconnect:
                    //TODO: [Joe] 目前不支援在 USB 插入裝置後, 使用 Wifi, 等支援這個行為, 在將這邊修掉
                    if(status == DevStatus.Disconnect && !Wifi_Var.useWifi)
                    {
                        Wifi_checkBox_.Enabled = false;
                        Wifi_ip_address.Enabled = false;
                    }
                    else
                    {
                        Wifi_checkBox_.Enabled = true;
                        Wifi_ip_address.Enabled = true;
                    }

                    if((status == DevStatus.DeviceRemove) && (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bRunningStatus))
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintFullScreenExit, (IntPtr)0, (IntPtr)0);
                    else
                        ToolStripBar_Main.Enabled = true;
                    if(fm_SensorTest_D != null)
                    {
                        fm_SensorTest_D.delSet_tabControl_TestBtnEnabled((status == DevStatus.DeviceRemove) ? false : true);
                        fm_SensorTest_D.fm_Setting.btPanelInfoLoad.Enabled = (status == DevStatus.DeviceRemove) ? false : true;
                        fm_SensorTest_D.fm_Setting.btn_CaptureFW_CRC.Enabled = (status == DevStatus.DeviceRemove) ? false : true;
                    }
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.HIDE, IntPtr.Zero);

                    foreach(var item in EnumTool.EnumToList<Tool_BTNs>())
                    {
                        if(!UI_GBV.ToolMain.btnMainButtons.ContainsKey(item))
                            continue;
                        switch(item)
                        {
                            case Tool_BTNs.BTN_CDC:
                            case Tool_BTNs.BTN_TUNING:
                            case Tool_BTNs.BTN_PAINTING:
                            case Tool_BTNs.BTN_FUNCGEN:
                            case Tool_BTNs.BTN_MOREITEM:
                            case Tool_BTNs.BTN_CMODEL:
                            case Tool_BTNs.BTN_FREQUENCYSPECTRUM:
                            case Tool_BTNs.BTN_CHARGECURVE:
                            
                                UI_GBV.ToolMain.btnMainButtons[item].Enabled = true;
                                UI_GBV.ToolMain.btnMainButtons[item].BackColor = Enable_Color;
                                break;
                            case Tool_BTNs.BTN_CONSOLE:
                                if(status == DevStatus.DeviceRemove)
                                {
                                    UI_GBV.ToolMain.btnMainButtons[item].Enabled = false;
                                    UI_GBV.ToolMain.btnMainButtons[item].BackColor = Disable_Color;
                                }
                                else
                                {
                                    UI_GBV.ToolMain.btnMainButtons[item].Enabled = true;
                                    UI_GBV.ToolMain.btnMainButtons[item].BackColor = Enable_Color;
                                }
                                break;

                            default:
                                UI_GBV.ToolMain.btnMainButtons[item].Enabled = false;
                                UI_GBV.ToolMain.btnMainButtons[item].BackColor = Disable_Color;
                                break;
                        }
                    }

                    m_panBridgeSetting.Enabled = true;
                    btn_Connect.Enabled = (status == DevStatus.Disconnect) ? true : false;
                    btn_Connect.Image = Properties.Resources.disconnect;
                    btn_Connect.Text = "Disconnect";
                    //fm_ExtendFlowLayout.SetDeviceConnection(status);

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_DBG_MSG_Show, (IntPtr)enDBG_MSG_.CLEAR, "");
                    toolStrip_IC_Name.Text = "";
                    toolStrip_IC_Name.Enabled = false;
                    DeviceButtons.Enabled = true;
                    break;
            }
        }

        protected override void WndProc(ref Message msg)
        {
            enWM_MSG pEnumPanoMsg = (enWM_MSG)msg.Msg;
            if(mMessageReceiver.Keys.Contains(pEnumPanoMsg))
                mMessageReceiver[pEnumPanoMsg](ref msg);
            base.WndProc(ref msg);
        }
        bool isCModelVisible = false;
        bool isChargeCurveVisible = false;
        private void Assign_UI_Btn()
        {
            UI_GBV.ToolMain.btnMainButtons = new Dictionary<Tool_BTNs, Button>();
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_HOME, m_btHome);
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_CONSOLE, m_btConsole);
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_PAINTING, m_btPainting);
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_CDC, m_btCDC);
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_UPGRADE, m_btUpgrade);
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_TUNING, m_btTuning);
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_SENSORTEST_D, m_btSensrTest_Single);
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_SENSORTEST_S, m_btSensrTest_Multi);
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_LOGANALYZER, m_btLogAnalyzer);
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_CMODEL, fm_ExtendFlowLayout.m_btCModel);            
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_FREQUENCYSPECTRUM, fm_ExtendFlowLayout.m_btFreSpectrum);
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_CHARGECURVE, fm_ExtendFlowLayout.m_btChargCurve);
            UI_GBV.ToolMain.btnMainButtons.Add(Tool_BTNs.BTN_MOREITEM, m_btOpenExtendUI);            

            var _list = MY_INI.IniReadAllKeys("DisplayFun");
            foreach(var item in EnumTool.EnumToList<Tool_BTNs>())
            {
                if(!UI_GBV.ToolMain.btnMainButtons.ContainsKey(item))
                    continue;
                UI_GBV.ToolMain.btnMainButtons[item].Visible = _list.Contains(item.ToString());
            }

            isCModelVisible = (_list.Contains(Tool_BTNs.BTN_CMODEL.ToString()));
            isChargeCurveVisible = (_list.Contains(Tool_BTNs.BTN_CHARGECURVE.ToString()));

            bool isHideSensorTest = !(_list.Contains(Tool_BTNs.BTN_SENSORTEST_D.ToString())
                || _list.Contains(Tool_BTNs.BTN_SENSORTEST_S.ToString())
                || _list.Contains(Tool_BTNs.BTN_LOGANALYZER.ToString()));

            if(isHideSensorTest)
                m_tbgSensorTest.Parent = null;
        }

        private void ConsoleInterface()
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarThread_Multi.Data_Decode_Type.DECODE_TYPE = enDECODE_TYPE.InterruptIn_Other;
            StaticVar.iNowDeviceCnts = Static_HID.EnumDevice(100);

            if(UI_GBV.fmITS_Tool.ResetProtocol(m_iSelectedDev
                , string.Format("{0}{1}", IC_PROTOCOL_FUN.enum_Protocol_Name.Protocol_DEF, IC_PROTOCOL_FUN.enum_Interface_Name._DEF)))// Update protocol and channel
                return;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();//開啟USB接收
        }

        private void ILITekTPTestStudio_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6 &&
                           UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum > Ver.AP_605)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.LockPoint(Command._SWITCH.Dis);

            StaticVar.bIsChangeToNormalMode = true;

            ResetAllToolStatus();

            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bUSBMonitor_Opened) //如果USB Monitor有打開再開始執行 20220407 - Steven
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
            }
            if(fm_CDCTool != null)
                fm_CDCTool.Close();
            if(fm_PaintTool != null)
                fm_PaintTool.Close();
            if(fm_FWUpgrade != null)
                fm_FWUpgrade.Close();
            if(fm_TuningTool != null)
                fm_TuningTool.Close();
            if(fm_AutoScanCH_Tool != null)
                fm_AutoScanCH_Tool.Close();
            if(fm_SensorTest_D != null)
                fm_SensorTest_D.Close();
            if(fm_C_Model != null)
                fm_C_Model.Close();
            
            //if(PleaseWait != null)
            //    PleaseWait.Close();
            // Avoid form close hang
            vfCloseAllThread(m_iSelectedDev);
            Application.DoEvents();
            if(Program.KeepRunning)
            {
                Process pro_Isc = new Process();
                pro_Isc.StartInfo.FileName = ITS_Directory.Tool_CurrentDirectory + System.AppDomain.CurrentDomain.FriendlyName;
                pro_Isc.Start();
            }
            GC.Collect();
            //FileInfo fileInfo = new FileInfo(ITS_Directory.MultiDev_Port);
            //if (fileInfo.Exists)            
            //    File.Delete(ITS_Directory.MultiDev_Port);
            Application.Exit(); //先關閉所有執行緒Message和其餘視窗

            CommonFlow.set_log_fclose();

            Environment.Exit(0);
        }

        // Hot Key List
        // C        : [Painting]    Clear Painting
        // SPACE    : [Painting]    Clear Painting
        // Ctrl + R : [CDC]         Record Start or Stop
        // Ctrl + M : [Painting]    Finger Monitor Start or Stop
        // Enter    : [Console]     Send CMD
        // F1       : [ILI_GP]      Special Tool : RefistryTool
        // Alt + Z  : [Main]        MainSetting Form
        // Alt + F1 : [Main]        SpecialSetting Form
        private void ILITekTPTestStudio_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Space:// Clear Painting
                case Keys.C:
                case Keys.Enter:
                    switch(StaticVar.NowExecuteTool)
                    {
                        case enToolName.SensorTestTool:
                            if((e.KeyCode != Keys.Space) && (e.KeyCode != Keys.Enter))
                                break;
                            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.DeviceRemove)
                                break;
                            string strTriggerOption = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "TriggerOption");
                            var _var = EnumTool.GetValueFromDescription<TriggerMode>(strTriggerOption);

                            if(_var == TriggerMode.BarCodeButton || _var == TriggerMode.BarCode)
                                if(e.KeyCode == Keys.Space)
                                    break;

                            if(StaticVar.bisEnterPressed)
                                break;
                            StaticVar.bisEnterPressed = true;

                            if(_var != TriggerMode.BarCode)
                            {
                                if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker == null)
                                    fm_SensorTest_D.delTrigerTest_API(_var);
                                else if(!UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.bgwWorker.IsBusy)
                                    fm_SensorTest_D.delTrigerTest_API(_var);
                            }
                            if(_var == TriggerMode.BarCodeButton)
                                fm_SensorTest_D.delBarcodeClear();

                            break;

                        case enToolName.MultiSensorTestTool:
                            if((e.KeyCode != Keys.Space) && (e.KeyCode != Keys.Enter))
                                break;
                            var triger = UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "TriggerOption");
                            //if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.GetMainSetValue("OP_Mode", "TriggerOption") != TriggerMode.BarCode.Desc())
                            //if(triger == TriggerMode.Button.Desc() || triger == TriggerMode.BarCodeButton.Desc())
                            if(triger == TriggerMode.Button.Desc())
                                fm_SensorTest_S.btnAll_Test_Click(this, null);
                            break;

                        case enToolName.PaintTool:
                            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bRunningStatus && !StaticVar.bPaint_Record && !StaticVar.IsPlayback_Mode)
                            {
                                fm_PaintTool.vfChangeResolution();
                                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.FullScreen)
                                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(this.Handle, enWM_MSG.WM_PaintFullScreenClear, (IntPtr)0, (IntPtr)0);
                            }
                            break;

                        case enToolName.C_Model:
                            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCModel_Multi.bRunningStatus)
                                fm_C_Model.Clear_Painting();
                            else
                                fm_C_Model_Replay.ChangeResolution();

                            break;

                        case enToolName.ConsoleTool:
                            if(e.KeyCode != Keys.Enter)
                                break;
                            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable._Console_VAR.bRunningStatus)
                                fm_ConsoleTool.vfWriteUSB();
                            break;

                    }
                    break;

                case Keys.E: //Ctrl + E (Force Finish Panel Test and save log)
                    switch(StaticVar.NowExecuteTool)
                    {
                        case enToolName.PanelTest:
                            if(e.Control == false)
                                break;
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.PanelTestTool_Var.bIsTesting = false;
                            break;
                        default:
                            break;
                    }
                    break;

                case Keys.R://Ctrl + R (CDC Record Start or Stop)
                    if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.bRunningStatus && e.Control == true)
                        fm_CDCTool.vfRecordCDC();
                    else if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bRunningStatus && e.Control == true)
                        fm_PaintTool.RecordPainting();
                    else if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCModel_Multi.bRunningStatus && e.Control == true)
                        fm_C_Model.Record_CDC_and_Painting();
                    break;

                case Keys.Z:
                    if(e.Alt == false)
                        break;

                    bool bAnyToolIsRuning = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarPaint_Multi.bRunningStatus
                        || UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.bRunningStatus
                        || UI_GBV.mDev[m_iSelectedDev].LocalVariable._Console_VAR.bRunningStatus
                        || UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bRunningStatus
                        || UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode != enSensorTest.Stop);
                    if(bAnyToolIsRuning)
                        break;

                    string value = "";
                    string password = ToolVersion.Replace(".", "").Split(' ')[0];
                    string str_1 = "", str_2 = "";
                    if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                        str_1 = global::ITS_Studio.Properties.Resources.Password_En;
                    else
                        str_1 = global::ITS_Studio.Properties.Resources.Password_Cn;
                    if(UIInputDialog.InputPasswordDialog(ref value, true, str_1, UIStyle.Orange, true))
                    {
                        if(string.Equals(value, password))
                        {
                            MainSetting fm_MainSetting = new MainSetting();
                            fm_MainSetting.ShowDialog(this);
                        }
                        else
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
                        }
                    }

                    break;

                case Keys.Escape:
                    if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
                    {
                        if(m_tabControl.SelectedTab == tabPage_SensorTest_D)
                            fm_SensorTest_D.vfStop(DevOption.ForceStop);
                    }
                    else
                    {
                        if(MyUIPage.ShowAskDialog(Application.ProductName, "Exit Tool?", UIStyle.Black, true))
                            this.Close();
                    }
                    break;

                case Keys.B:
                    if(e.Alt == true)
                        splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
                    break;

                case Keys.F9:
                    if(e.Control == false)
                        break;

                    b_showMPResult = !b_showMPResult;

                    UI_GBV.ToolMain.m_RichText_INFO_MSG.Text = "";

                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.m_HID_API.HID_CreateHandle(
                        StaticVar.lsMain_USBDevInfo[m_iSelectedDev].strDevPath);

                    CommonFlow.api_update_ts_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev);

                    CommonFlow.msg_str = String.Empty;
                    CommonFlow.api_print_ts_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev);

                    if(MY_INI.IniReadBool("Main", "Read_DualUSB_Info", false))
                        CommonFlow.api_read_then_print_m2v_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev);

                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.commonflow_get_info(
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.info[m_iSelectedDev]);

                    UI_GBV.mDev[m_iSelectedDev].m_GBV.ConsoleTool.strInforMessage = CommonFlow.msg_str + "\r\n";

                    UpdateInfoMSG();

                    if(b_showMPResult && UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                    {
                        string msg;
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.read_then_decode_mp_result(out msg);

                        UI_GBV.ToolMain.m_RichText_INFO_MSG.Text += msg;
                    }

                    break;

                case Keys.I:
                    if(e.Control == false)
                        break;

                    if(this.m_tbgInternalUse.Parent == null)
                        this.m_tbgInternalUse.Parent = this.m_tbMain;

                    break;

                case Keys.T:

                    if(e.Control == false || StaticVar._ITSTool_Var != enMode.RDMode)
                        break;

                    if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                    {
                        StaticVar.bIsChangeToNormalMode = !StaticVar.bIsChangeToNormalMode;

                        if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6 &&
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum > Ver.AP_605)
                        {
                            MyUIPage.ShowSuccessTip(string.Format("{0} switching Normal Mode", StaticVar.bIsChangeToNormalMode ? "Enable" : "Disable"),
                                1000, true);
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.LockPoint(StaticVar.bIsChangeToNormalMode ? Command._SWITCH.Dis : Command._SWITCH.En);
                        }
                        else
                        {
                            if(!StaticVar.bIsChangeToNormalMode)
                            {
                                MyUIPage.ShowSuccessTip("Disable switching Normal Mode", 1000, true);
                                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.TestMode, Command._SWITCH.Dis);
                            }
                            else
                            {
                                MyUIPage.ShowSuccessTip("Enable switching Normal Mode", 1000, true);
                                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
                            }
                            CommonFlow.setting[m_iSelectedDev].not_change_normal_mode = StaticVar.bIsChangeToNormalMode;
                            CommonFlow.ilitek_dev_setting(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.setting[m_iSelectedDev]);

                        }

                    }
                    break;
                case Keys.D:
                    button1_Click(this, null);
                    break;

            }
        }

        private void ILITekTPTestStudio_Load(object sender, EventArgs e)
        {
            StyleManager = new UIStyleManager(this.components);
            StyleManager.Style = UIStyle.Orange;

            if(StaticVar._ITSTool_Var != enMode.RDMode)
                this.Hide();

            FileInfo fileInfo = new FileInfo(ITS_Directory.AssignedPort_INI);
            if(fileInfo.Exists)
            {
                INI_Tool AssignedPort_INI = new INI_Tool(ITS_Directory.AssignedPort_INI);
                //StaticVar.AssignedPort_Str = AssignedPort_INI.IniReadValue("Assigne", "FixedPort", "");
                //StaticVar.AssignedPort_ID = AssignedPort_INI.IniReadValue("Assigne", "FixedPort_ID", "");
                StaticVar.AssignedPortInfo.AssignedPort_Chain = AssignedPort_INI.IniReadValue("Assigne", "FixedPort", "");
                //StaticVar.AssignedPortInfo.AssignedPort_DevID = AssignedPort_INI.IniReadValue("Assigne", "FixedPort_DevID", "");
            }

            StaticVar.iNowDeviceCnts = Static_HID.EnumDevice(100);
            if(StaticVar.iNowDeviceCnts > 0)
                Static_HID.SyncMaiDevInfoBuff(DevSyncStatus.AddDev);

            bool bChecked = false;
            foreach(var item in StaticVar.lsMain_USBDevInfo)
            {
                if(item.NowDevStatus != DevStatus.DeviceRemove)
                {
                    if(item.UI_Idx == -1)
                        continue;

                    ChangeDeviceButtons(item, (bChecked ? CheckState.Unchecked : CheckState.Checked), true);
                    if(!bChecked)
                        DeviceButtons.Text = item.Port_Info.Port_Chain;
                    bChecked = true;
                }
            }

            if(StaticVar.lsMain_USBDevInfo[SelectedDev].bIsHID_Over_I2C)
            {
                UI_GBV.ToolMain.btnMainButtons[Tool_BTNs.BTN_SENSORTEST_S].Visible = false;
                if(StaticVar._ITSTool_Var == enMode.Multi_OPMode)
                    StaticVar._ITSTool_Var = enMode.OPMode;
            }
            StaticVar.bDrawingShowPass = MY_INI.IniReadBool("SensorTest", "DrawingShowPass", false);

            if(StaticVar._ITSTool_Var == enMode.RDMode)
            {
                toolStrip_txt_Mode.Text = "RD Mode";
                toolStrip_txt_Mode.BackColor = Color.SandyBrown;

                var NowDevStatus = (StaticVar.iNowDeviceCnts > 0) ? DevStatus.Disconnect : DevStatus.DeviceRemove;
                if(fm_SensorTest_D != null)
                {
                    DevOption StopOption = (StaticVar.iNowDeviceCnts > 0) ? DevOption.AddDevice : DevOption.RemoveDevice;
                    fm_SensorTest_D.vfStop(StopOption);
                }
                Reset_ConnectionStatus(m_iSelectedDev, NowDevStatus);
                Reset_UI();

                //splitContainer_MainUI.SplitterDistance = 129;
                m_tbMain.Visible = true;
                m_tbMain.SelectedTab = m_tbgTPTool;
                this.m_tbgInternalUse.Parent = null;
            }
            else if(StaticVar._ITSTool_Var == enMode.OPMode)
            {
                ToolStripBar_Main.Visible = false;
                var NowDevStatus = (StaticVar.iNowDeviceCnts > 0) ? DevStatus.Disconnect : DevStatus.DeviceRemove;
                Reset_ConnectionStatus(m_iSelectedDev, NowDevStatus);
                Reset_UI();

                toolStrip_txt_Mode.Text = "OP Mode";
                toolStrip_txt_Mode.BackColor = Color.Yellow;

                ToolStripBar_Main.Enabled = false;
                btn_Connect.Enabled = false;
                btn_Connect.Text = "Connect";
                m_tbMain.Visible = false;
                splitContainer_MainUI.Panel1Collapsed = true;
                splitContainer_MainUI.Panel1.Hide();

                if(!b_Chroma_EN)	//For AUO
                    this.WindowState = System.Windows.Forms.FormWindowState.Maximized; //20220630 Steven Add for KC

                TimerClock.Stop();
                StatusLabel_Clock.Visible = false;
                m_tbMain.SelectedTab = m_tbgSensorTest;

                string tmpINI_Profile = "";
                fm_SensorTest_D.UpdateProfilePath(out tmpINI_Profile);
                if(Path.IsPathRooted(tmpINI_Profile))
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath = tmpINI_Profile;
                else
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath = string.Format(@"{0}{1}", ITS_Directory.Tool_CurrentDirectory, tmpINI_Profile);

                bool isEnableITS = MY_INI.IniReadBool("SensorTest", "EnableCallITS", false);
                //StaticVar.bEnableCallITS = isEnableITS;

                //Call ITS 
                if(isEnableITS)
                {
                    if(File.Exists(ITS_Directory.IDPinSettingPath))
                    {
                        string sRet = string.Empty;
                        List<enTestSelect> lsItemSequence = new List<enTestSelect>();
                        INI_Tool ChoiceINI = new INI_Tool(ITS_Directory.IDPinSettingPath);

                        sRet = ChoiceINI.IniReadValue("INI_FILE", "Test_Sequence", "");
                        string sbitMask = ChoiceINI.IniReadValue("INI_FILE", "SensorID_BitMask", "0xFF");
                        StaticVar.bitMask = Convert.ToByte(sbitMask, 16);

                        string[] sTemp = sRet.Split('&');
                        foreach(var item in sTemp)
                        {
                            lsItemSequence.Add(EnumTool.GetValueFromDescription<enTestSelect>(item));
                        }
                        //Connect Device
                        bool bConnectionResult = Wifi_Var.useWifi ? RemoteConnection() : LocalConnection();
                        if(bConnectionResult)
                        {
                            //if (!CallITSFunc(ChoiceINI, lsItemSequence))
                            string err_msg = "";
                            if(!UI_GBV.mDev[m_iSelectedDev].CallITSFunc(ChoiceINI, lsItemSequence, m_iSelectedDev, ref err_msg))
                            {
                                MyUIPage.ShowErrorDialog("Error", err_msg, UIStyle.Red, true);
                                ExceptionStop();
                                Environment.Exit(0);
                            }
                            ExceptionStop();
                        }
                        else
                        {
                            MyUIPage.ShowErrorDialog("Error", "Device Not Found", UIStyle.Red, true);
                            ExceptionStop();
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        MyUIPage.ShowErrorDialog("Error", "Call ITS Setting File Missing", UIStyle.Red, true);
                        ExceptionStop();
                        Environment.Exit(0);
                    }
                }
                //===================================
                if(fm_SensorTest_D != null)
                    m_btSensrTest_D_Click(null, null);

                //"OP_Mode", "ToolStart_AutoTest"
                if(StaticVar.bToolStart_AutoTest)
                {
                    fm_SensorTest_D.timer_AutoTest.Enabled = true;
                }
                else if(StaticVar.bI2CDetect)
                {
                    if(StaticVar.strInterface == DEV_INTERFACE_TYPE.I2C.ToString())
                        fm_SensorTest_D.timer_I2CDetect.Enabled = true;
                    StaticVar.iAtuoTestTimerInterval = MY_INI.IniReadInt("SensorTest", "AutoTestTimerInterval", 1000);
                    if(StaticVar.iAtuoTestTimerInterval <= 0)
                        StaticVar.iAtuoTestTimerInterval = 1000;
                    fm_SensorTest_D.timer_I2CDetect.Interval = StaticVar.iAtuoTestTimerInterval;
                    //打開Bridge Power和開啟USB接收
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.BridgeSettingInit(100);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();
                }
            }

            else if(StaticVar._ITSTool_Var == enMode.Multi_OPMode)
            {
                ToolStripBar_Main.Visible = false;
                toolStrip_txt_Mode.Text = "Multi OP Mode";
                this.toolStrip_txt_Mode.BackColor = Color.Yellow;

                ToolStripBar_Main.Enabled = false;
                btn_Connect.Enabled = false;
                btn_Connect.Text = "Connect";
                m_tbMain.Visible = false;
                splitContainer_MainUI.Panel1Collapsed = true;
                splitContainer_MainUI.Panel1.Hide();

                TimerClock.Stop();
                m_tbMain.SelectedTab = m_tbgSensorTest;
                fm_SensorTest_D.btn_ShowMPResult.Visible = false;

                m_btSensrTest_S_Click(null, null);
            }

            var FullScreenMode = MY_INI.IniReadBool("Main", "FullScreenMode", false);
            SetMainForm(FullScreenMode);
            this.Show();

            if(StaticVar._ITSTool_Var == enMode.RDMode)
                OpenAutoUpdater(false);
        }

        private bool CallITSFunc(INI_Tool ChoiceINI, List<enTestSelect> lsItemSequence)
        {
            byte[] ret = new byte[0];
            FW_MODE nowFWMode = new FW_MODE();
            bool bIsDevErr = false;
            bool isBLMode = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.CheckDevMode_API(ref bIsDevErr, FW_MODE.BL_MODE);
            nowFWMode = bIsDevErr ? FW_MODE.UNKNOW_MODE : (isBLMode ? FW_MODE.BL_MODE : FW_MODE.AP_MODE);

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_ProtocolVersion(out ret);// Protocol版本
            Version real_protocol_Version = new Version(ret[0], ret[1], ret[2]);

            Dictionary<string, string> mode_list = new Dictionary<string, string>();
            mode_list = ChoiceINI.IniReadAllKeysandValuesByString("Mode_Setting");
            if(mode_list == null)
            {
                MyUIPage.ShowErrorDialog("Error", "Mode list empty!\r\n", UIStyle.Red, true);
                //MessageBox.Show("Mode list empty!\r\n", "Error");
                return false;
            }
            string sSetting = string.Empty;

            foreach(var testItem in lsItemSequence)
            {
                switch(testItem)
                {
                    case enTestSelect.SensorID:

                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_TPSensor(nowFWMode, real_protocol_Version, out ret);
                        int TPID = (ret == null) ? -1 : ret[2];

                        sSetting += (sSetting == string.Empty) ? TPID.ToString() : string.Format("_{0}", TPID.ToString());

                        break;
                    case enTestSelect.EDID:
                        List<byte[]> lsUsingMonitorEDID = new List<byte[]>();
                        string EDID_TF_PATH = string.Empty;
                        try
                        {
                            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_UsingMonitorEDID(ref lsUsingMonitorEDID);
                            if(lsUsingMonitorEDID.Count > 0)
                            {
                                int EDIDStartByte = ChoiceINI.IniReadInt("EDID", "EDID_StartByte", 0);
                                int EDIDEndByte = ChoiceINI.IniReadInt("EDID", "EDID_EndByte", 0);

                                if(EDIDEndByte < EDIDStartByte)
                                {
                                    MyUIPage.ShowErrorDialog("Error", "End Byte's setting number can't less than Start Byte's setting number\n", UIStyle.Red, true);
                                    //MessageBox.Show("End Byte's setting number can't less than Start Byte's setting number\n", "Error");
                                    return false;
                                }

                                string sActualEDIDData = string.Empty;
                                for(int i = EDIDStartByte; i <= EDIDEndByte; i++)
                                {
                                    sActualEDIDData += lsUsingMonitorEDID[0][i].ToString("X2"); //假設只有接上一台顯示器
                                }
                                sSetting += (sSetting == string.Empty) ? sActualEDIDData : string.Format("_{0}", sActualEDIDData);

                            }

                        }
                        catch(Exception)
                        {
                            MyUIPage.ShowErrorDialog("Error", "EDID Read Fail!", UIStyle.Red, true);
                            //MessageBox.Show("EDID Read Fail!", "Error");
                            return false;
                        }

                        break;
                    case enTestSelect.FWID:

                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_ChromeInfo(nowFWMode, real_protocol_Version, out ret);
                        string sActualCFWID = (ret == null) ? "not support" : ret[1].ToString("X2") + ret[0].ToString("X2") + ret[3].ToString("X2") + ret[2].ToString("X2");
                        if(sActualCFWID.Equals("not support"))
                        {
                            MyUIPage.ShowErrorDialog("Error", "not supported FW ID.", UIStyle.Red, true);
                            //MessageBox.Show("not supported FW ID.", "Error");
                            return false;
                        }
                        sSetting += (sSetting == string.Empty) ? sActualCFWID : string.Format("_{0}", sActualCFWID);
                        break;
                    case enTestSelect.PID:
                        //待補                
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.Get_HID_Info(nowFWMode, real_protocol_Version, out ret);
                        string sPID = (ret == null) ? string.Format("{0:X4}", StaticVar.lsMain_USBDevInfo[m_iSelectedDev].usPID) : (ret[1] << 8 | ret[0]).ToString("X4");

                        sSetting += (sSetting == string.Empty) ? sPID : string.Format("_{0}", sPID);
                        break;
                    case enTestSelect.Null:
                    default:
                        return false;
                }
            }

            string TG_PF_Path = string.Empty;
            foreach(KeyValuePair<string, string> list in mode_list)
            {
                if(list.Key == sSetting)
                {
                    TG_PF_Path = list.Value;
                    break;
                }
            }
            if(TG_PF_Path == string.Empty)
            {
                MyUIPage.ShowErrorDialog("Error", "Mode setting not found.", UIStyle.Red, true);
                //MessageBox.Show("Mode setting not found.", "Error");
                return false;
            }
            WriteProfilePath(TG_PF_Path);

            return true;
        }

        private void WriteProfilePath(string sPath)
        {
            bool isHistoryIniExist = System.IO.File.Exists(ITS_Directory.History_INI);
            if(!isHistoryIniExist)
            {
                MyUIPage.ShowErrorDialog("Error", "History.ini doesn't exist\n", UIStyle.Red, true);
                //MessageBox.Show("History.ini doesn't exist\n", "Error");
                return;
            }
            var iniFile = new INI_Tool(ITS_Directory.History_INI);
            iniFile.IniWriteValueW("SensorTest_ProfilePath", "Path", sPath);
        }

        private void SetMainForm(params object[] param)
        {
            var bFullScreenMode = (bool)param[0];
            this.Invoke((MethodInvoker)delegate()
            {
                if(bFullScreenMode)
                    ShowWindowAsync(this.Handle, (int)WIN_API.SW_MAXIMIZE);
                else
                    ShowWindowAsync(this.Handle, (int)WIN_API.SW_NORMAL);
                SetForegroundWindow(this.Handle);// bring it to the foreground
            });
        }

        private void Reset_ConnectionStatus(int iSelected, DevStatus ToolStyle)
        {
            if(StaticVar.iNowDeviceCnts > 0)
            {
                var Dev = StaticVar.lsMain_USBDevInfo[iSelected];
                Dev.NowDevStatus = ToolStyle;
                StaticVar.lsMain_USBDevInfo[iSelected] = Dev;
            }

            if(StaticVar.NowExecuteTool == enToolName.MultiSensorTestTool)
                if(ToolStyle == DevStatus.Disconnect)
                    return;

            switch(ToolStyle)
            {
                case DevStatus.DeviceRemove:
                case DevStatus.Disconnect:
                    //ResetAllToolStatus();
                    var _msg = new Message();
                    _msg.WParam = (IntPtr)iSelected;
                    OnResetToolStatus(ref _msg);

                    UI_GBV.mDev[iSelected].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(0);
                    if(UI_GBV.mDev[iSelected].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.FullScreen)
                        if(ExternalPaintForm != null)
                            ExternalPaintForm.Hide();

                    UI_GBV.mDev[iSelected].LocalVariable.Var_Multi.ProtocolType = enPROTOCOL.none;
                    //UI_GBV.mDev[iSelected].m_GBV.Globe_Multi.iICType = 0x00;
                    //UI_GBV.mDev[iSelected].m_GBV.Globe_Multi.strICType = "";
                    if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo != null)
                    {
                        //Console.WriteLine("DevPanelInfo.MCU = {0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU);
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU = "";
                    }
                    break;
            }
        }

        private void ILITekTPTestStudio_Resize(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
                return;

            if(StaticVar.NowExecuteTool == enToolName.PaintTool)
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo != null)
                {
                    if(m_tabControl.SelectedTab == tabPage_PaintTool)
                        fm_PaintTool.vfChangeResolution();
                    if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.X_RES == 0
                        || UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.Y_RES == 0)
                        return;
                }
                fm_PaintTool.vfChangeResolution();
            }

            if(StaticVar.NowExecuteTool == enToolName.TuningTool)
                fm_TuningTool.vfGUIInit();
        }

        private bool LocalConnection()
        {
            UI_GBV.mDev[m_iSelectedDev].m_GBV.ConsoleTool.strInforMessage = "";
            UpdateInfoMSG();
            string bridge_mess = "";
            bool bConnectResult = UI_GBV.mDev[m_iSelectedDev].LocalConnectionBase(out bridge_mess);
            toolStrip_BridgeVer.Visible = false;
            if(StaticVar.lsMain_USBDevInfo.Length <= 0)
                return false;
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
            {
                toolStrip_BridgeVer.Text = string.Format("Bridge Version : {0}", UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.strBridgeVer);
                toolStrip_BridgeVer.ForeColor = (bridge_mess != "") ? Color.Red : Color.BlueViolet;
                UI_GBV.mDev[m_iSelectedDev].m_GBV.ConsoleTool.strErrorMessage += bridge_mess;
                toolStrip_BridgeVer.Visible = true;
                splitContainer1.Panel1Collapsed = false;
                if(StaticVar.bridgeSetting.m_bridge_var.bCurrentDetectEn && bConnectResult)
                {
                    b_showCurrent = true;
                    TimerClock.Start();
                }
            }
            else
            {
                splitContainer1.Panel1Collapsed = true;
            }
            if(bConnectResult)
            {
                toolStrip_IC_Name.Enabled = true;
                toolStrip_IC_Name.Text = string.Format("{0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU.Replace("ILI", ""));
            }

            Reset_ConnectionStatus(m_iSelectedDev, StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus);
            Reset_UI();

            UpdateInfoMSG();

            if(bConnectResult)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
            return bConnectResult;
        }

        private void LocalDisconnect()
        {
            bool bBridgePowerOff = false;
            if((StaticVar._ITSTool_Var != enMode.Multi_OPMode))//暫時DLL不支援evayn
                bBridgePowerOff = true;

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.TP_BackToNormal(bBridgePowerOff);
        }

        private void RemoteMonitorSocket_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = true;

            while(true)
            {
                Thread.Sleep(1000);

                if(Wifi_Var.monitor_BGwork.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                if(Wifi_Var.isConnected())
                    continue;

                StaticVar.LogWrite(ConsoleLogType.Error, "[WiFi] Wifi connection lost\r\n");
                e.Result = false;
                break;
            }
        }

        private void RemoteMonitorSocket_WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Cancelled == true)
                StaticVar.LogWrite(ConsoleLogType.Error, "[WiFi] RemoteMonitorSocket_WorkCompleted  Cancelled\r\n");
            else if((bool)e.Result == false)
                ExceptionStop();
        }

        private void RemoteConnection_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = Wifi_Var.Connect();
        }

        private void RemoteConnection_WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error != null || e.Cancelled || (bool)e.Result == false)
            {
                StaticVar.LogWrite(ConsoleLogType.Error, "[WiFi] RemoteConnection Error or Cancel return!!!\r\n");
                return;
            }

            if(fm_WifiSetting.enable_wifi_monitor())
            {
                Wifi_Var.monitor_BGwork = new BackgroundWorker();
                Wifi_Var.monitor_BGwork.DoWork += new DoWorkEventHandler(RemoteMonitorSocket_DoWork);
                Wifi_Var.monitor_BGwork.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RemoteMonitorSocket_WorkCompleted);
                Wifi_Var.monitor_BGwork.WorkerSupportsCancellation = true;
                Wifi_Var.monitor_BGwork.RunWorkerAsync();
            }

            btn_Connect.Text = "Connect";
            StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus = DevStatus.Connected;

            UI_GBV.mDev[m_iSelectedDev].m_GBV.ConsoleTool.strInforMessage = "";
            UpdateInfoMSG();

            if(StaticVar.lsMain_USBDevInfo.Length <= 0)
                return;

            /* Update iNowDeviceCnts */
            StaticVar.iNowDeviceCnts = StaticVar.lsMain_USBDevInfo.Length;

            /* Create MonitorWifi thread and get IC info */
            bool bConnectResult = UI_GBV.mDev[m_iSelectedDev].RemoteConnectionBase();
            //toolStrip_IC_Name.Enabled = true;
            //toolStrip_IC_Name.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.iICType.ToString("X4");
            UpdateInfoMSG();

            if(bConnectResult)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);

            /* Wifi ITS Painting for I2C depends on I2C driver version */
            if(!Wifi_Var.support_wifi_paint)
                m_btPainting.Visible = false;

            toolStrip_IC_Name.Enabled = true;
            toolStrip_IC_Name.Text = string.Format("{0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU.Replace("ILI", ""));
            Reset_ConnectionStatus(m_iSelectedDev, StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus);
            Reset_UI();

            fm_WifiSetting.Set_ConnectedUI();
        }

        private bool RemoteConnection()
        {
            if(Wifi_ip_address.SelectedItem == null)
                return false;

            m_btSensrTest_Multi.Visible = false;

            foreach(var item in EnumTool.EnumToList<Tool_BTNs>())
            {
                if(!UI_GBV.ToolMain.btnMainButtons.ContainsKey(item))
                    continue;
                UI_GBV.ToolMain.btnMainButtons[item].Enabled = false;
                UI_GBV.ToolMain.btnMainButtons[item].BackColor = BtnColor[(int)SHOW.DISABLE];
            }

            /* UI setting */
            StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus = DevStatus.Wifi_Connecting;
            btn_Connect.Enabled = true;
            btn_Connect.Image = Properties.Resources.ajax_loader;
            btn_Connect.Text = "Wifi Connecting";

            /* Wifi connect */
            Wifi_Var.serverIP = "";
            if(!String.Equals(Wifi_ip_address.SelectedItem.ToString(), Wifi_Var.adb_socket_ip))
                Wifi_Var.serverIP = Wifi_ip_address.SelectedItem.ToString();

            INI_Tool wifi_ini = new INI_Tool(ITS_Directory.WiFi);
            Wifi_Var.serverPort = wifi_ini.IniReadInt("ConnectionInfo", "Port", 8080);

            Wifi_Var.accept_BGwork = new BackgroundWorker();
            Wifi_Var.accept_BGwork.DoWork += new DoWorkEventHandler(RemoteConnection_DoWork);
            Wifi_Var.accept_BGwork.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RemoteConnection_WorkCompleted);
            Wifi_Var.accept_BGwork.WorkerSupportsCancellation = true;
            Wifi_Var.accept_BGwork.RunWorkerAsync();

            fm_WifiSetting.Set_ConnectingUI();
            return true;
        }

        private void RemoteDisconnect()
        {
            m_btSensrTest_Multi.Visible = true;

            if(Wifi_Var.accept_BGwork != null && Wifi_Var.accept_BGwork.IsBusy)
                Wifi_Var.accept_BGwork.CancelAsync();

            if(Wifi_Var.monitor_BGwork != null && Wifi_Var.monitor_BGwork.IsBusy)
                Wifi_Var.monitor_BGwork.CancelAsync();

            if(Wifi_Var.paint_BGwork != null && Wifi_Var.paint_BGwork.IsBusy)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.PaintTool_StopPainting(enInt_InDataType.Painting);

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.TP_BackToNormal(false);

            if(!Wifi_Var.support_wifi_paint)
                m_btPainting.Visible = true;

            /* Close client/server socket */
            Wifi_Var.Disconnect();

            fm_WifiSetting.Set_DefaultUI();

            return;
        }

        private void UpdateInfoMSG()
        {
            UI_GBV.ToolMain.m_RichText_ERR_MSG.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.ConsoleTool.strErrorMessage;
            UI_GBV.ToolMain.m_RichText_INFO_MSG.Text = UI_GBV.mDev[m_iSelectedDev].m_GBV.ConsoleTool.strInforMessage;

            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6 && UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum > Ver.MaxSupportProtocolVersion)
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_DBG_MSG_Show, (IntPtr)enDBG_MSG_.ERROR_MSG_APPEND,
                    string.Format("The current supported Protocol Version for the tool is only up to {0}.{1}.{2}. Please ensure that the firmware is correct.",
                    Ver.MaxSupportProtocolVersion.Major.ToString("X1"), Ver.MaxSupportProtocolVersion.Minor.ToString("X1"), Ver.MaxSupportProtocolVersion.Build.ToString("X1")));
        }

        //20220829 Steven Add
        private void OpenAutoUpdater(bool manually)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
            sw.Reset();//碼表歸零
            sw.Start();//碼表開始計時
            //
            bool updater_enable = false;
            if(MY_INI.IniReadValue("Main", "AutoUpdate_Enable", "False") == "True")
                updater_enable = true;
            else
                updater_enable = false;
            //
            if(updater_enable || manually)
            {
                if(!System.IO.File.Exists("Update\\Windows_Download_ITS.exe"))
                {
                    //取消找不到AutoUpdater的顯示，讓Sikuli呼叫時不會卡住
                    MyUIPage.ShowErrorDialog("Turn on a additional feature", "Windows_Download_ITS.exe doesn't exist.", UIStyle.Red, true);
                    return;
                }
                
                sw.Stop();//碼錶停止

                //印出所花費的總豪秒數

                string result1 = sw.Elapsed.TotalMilliseconds.ToString();
            }
        }

        private void m_btCDC_Click(object sender, EventArgs e)
        {
            StaticVar.NowExecuteTool = enToolName.CDCTool;
            MainButton_Disable();
            this.Refresh();
            fm_CDCTool.SetDefault_CDC_Type();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarThread_Multi.Data_Decode_Type.DECODE_TYPE = enDECODE_TYPE.InterruptIn_Other;
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus != DevStatus.Connected)
            {
                tab_Message.SelectedTab = tabPage_CDCPlay;
                lb_Mutual_Max.Text = "";
                lb_Mutual_Min.Text = "";
                lb_Mutual_Delta.Text = "";
            }

            UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame.Clear();
            //全部狀態都Reset
            var _msg = new Message();
            _msg.WParam = (IntPtr)m_iSelectedDev;
            OnResetToolStatus(ref _msg);

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_CDCTool);

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                ModeSelect[TouchMode.TestMode](UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType);
            else
                ModeSelect[TouchMode.Playback](UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType);

            Reset_UI();
            vbButtonSelect(m_btCDC);
        }

        private void m_btConsole_Click(object sender, EventArgs e)
        {
            splitContainer_MainUI.Enabled = false;
            string _str = "";
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_En;
            else
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_Cn;
            MyUIPage.ShowWaitForm(_str);
            StaticVar.NowExecuteTool = enToolName.ConsoleTool;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bI2C_NACK_ERROR_RETURN_HOME = false;

            var _msg = new Message();
            _msg.WParam = (IntPtr)m_iSelectedDev;
            OnResetToolStatus(ref _msg);

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_Console);

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus != DevStatus.DeviceRemove)
                ModeSelect[TouchMode.NormalMode](UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType);

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Disconnect)
            {
                if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.BridgeSettingInit_API(StaticVar.bridgeSetting.m_bridge_var.iPower_On_DelayTime);
                string bridge_mess = "";
                bool bConnectResult = UI_GBV.mDev[m_iSelectedDev].fakeConnectionBase(out bridge_mess);
                StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus = DevStatus.Disconnect;
            }
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

            bool isSPIWithHeader = MY_INI.IniReadBool("Setting_Bridge", "SPI_With_Header", true);

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.vfSetSPITestModeEnable(!isSPIWithHeader);
                //UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.vfSetSPIHeader(!isSPIWithHeader);
            }

            fm_ConsoleTool.Console_Init();
            fm_ConsoleTool.Console_Start();

            splitContainer_MainUI.Enabled = true;
            vbButtonSelect(m_btConsole);
            MyUIPage.HideWaitForm();
        }

        private void m_btHome_Click(object sender, EventArgs e)
        {
            StaticVar.NowExecuteTool = enToolName.Main;
            MainButton_Disable();

            var _msg = new Message();
            _msg.WParam = (IntPtr)m_iSelectedDev;
            OnResetToolStatus(ref _msg);

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_Home);

            vbButtonSelect(m_btHome);

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

            if((e != null) && (UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode != FW_MODE.UNKNOW_MODE))
                ModeSelect[TouchMode.SuspendMode](UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType);

            //Update Panel Info
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_DBG_MSG_Show, (IntPtr)enDBG_MSG_.INFO_MSG_COVER, "");

            toolStrip_BridgeVer.Visible = false;
            toolStrip_BridgeVer.Text = "";
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
            {
                //先關閉重置INT
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.SetBridge_INT_Status_API(GPIO_PIN_TYPE.INT_DISABLE);
                string bridge_mess = "";
                string strMsg = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.ReadBridgeVer_API(ref bridge_mess);
                if(bridge_mess != "")
                    MyUIPage.ShowErrorTip(bridge_mess, 1000, true);

                toolStrip_BridgeVer.Text = string.Format("Bridge Version : {0}", strMsg);
                toolStrip_BridgeVer.Visible = true;
            }

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.m_HID_API.HID_CreateHandle(
                StaticVar.lsMain_USBDevInfo[m_iSelectedDev].strDevPath);

            CommonFlow.api_update_ts_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev);

            CommonFlow.msg_str = String.Empty;
            CommonFlow.api_print_ts_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev);

            if(MY_INI.IniReadBool("Main", "Read_DualUSB_Info", false))
                CommonFlow.api_read_then_print_m2v_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev);

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.commonflow_get_info(
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.info[m_iSelectedDev]);

            UI_GBV.mDev[m_iSelectedDev].m_GBV.ConsoleTool.strInforMessage = CommonFlow.msg_str;

            UpdateInfoMSG();


            if((e != null) && (UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode != FW_MODE.UNKNOW_MODE))
                ModeSelect[TouchMode.NormalMode](UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType);

            Reset_UI();

            if(StaticVar.bridgeSetting.m_bridge_var.bCurrentDetectEn && !TimerClock.Enabled)
            {
                b_showCurrent = true;
                TimerClock.Start();
            }

        }

        private bool Wait_IntFlag_V6_Only(int RetryCounts)
        {
            do
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.g_IntFlag)
                    break;
                RetryCounts--;
                MyTool.Delay_ms(1);
            } while(!UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.g_IntFlag && (RetryCounts > 0));

            return (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.g_IntFlag);
        }

        //畫線時HID要切Engineer Mode，bridge可以不用
        private void m_btPainting_Click(object sender, EventArgs e)
        {
            StaticVar.NowExecuteTool = enToolName.PaintTool;
            MainButton_Disable();

            //全部狀態都Reset
            var _msg = new Message();
            _msg.WParam = (IntPtr)m_iSelectedDev;
            OnResetToolStatus(ref _msg);

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                MyTool.Delay_ms(1000);//因為從CDC切換過來的時候，要讓FW有時間切換。


            UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame.Clear();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_PaintTool);

            tabPage_PaintTool.Controls.Clear();

            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.FullScreen)
            {
                ExternalPaintForm = new fmExternalPaint();
                ExternalPaintForm.TopLevel = true;
                ExternalPaintForm.Hide();

                ExternalPaintForm.Controls.Clear();
                ExternalPaintForm.Controls.Add(fm_PaintTool);
                ExternalPaintForm.Show();
            }
            else
            {
                tabPage_PaintTool.Controls.Add(fm_PaintTool);
                fm_PaintTool.InitDataColumn();
            }

            //Wait until the last step is over before you can continue to go down.
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                ModeSelect[TouchMode.NormalMode](UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType);
            else
                ModeSelect[TouchMode.Playback](UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType);

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_DBG_MSG_Show, (IntPtr)enDBG_MSG_.CLEAR, "");

            //20220613 Steven : 先暫時放這解決連線後斷線沒有ChangeRes的問題
            fm_PaintTool.vfChangeResolution();
            //////////////////////////////

            Reset_UI();
            vbButtonSelect(m_btPainting);
        }

        private string strProtocol = "";

        private List<string> _ReportInfo;

        public string GetCVS_Protocol
        {
            set
            {
                strProtocol = "";
                Report_Processor DecodeReport = new Report_Processor(value);
                if((DecodeReport.lsReport.FindIndex(x => x.Contains("Report_Format_Version")) <= 0)
                    || (DecodeReport.lsReport.FindIndex(x => x.Contains("ProtocolVersion")) <= 0)
                    || (DecodeReport.lsReport.FindIndex(x => x.Contains("Interface")) <= 0))
                {
                    strProtocol = ErrMsg.SensorTest.Report_Format_Not_Support.ToString();
                    return;
                }

                _ReportInfo = new List<string>();
                int iTitleEndIndex = DecodeReport.lsReport.FindIndex(x => x.Contains("["));
                int iResult_Summary = DecodeReport.lsReport.FindLastIndex(x => x.Contains("Result_Summary"));
                if((iTitleEndIndex != -1) && (iResult_Summary != -1))
                {
                    _ReportInfo.AddRange(DecodeReport.lsReport.GetRange(0, iTitleEndIndex));
                    _ReportInfo.AddRange(DecodeReport.lsReport.GetRange(iResult_Summary, DecodeReport.lsReport.Count - iResult_Summary));
                }
            }
            get
            {
                if(strProtocol == ErrMsg.SensorTest.Report_Format_Not_Support.ToString())
                    return strProtocol;
                if(_ReportInfo.Count == 0)
                    return ErrMsg.SensorTest.Report_Format_Not_Support.ToString();
                string _protocol = _ReportInfo.Find(x => x.Contains("ProtocolVersion")).Split(',')[1];
                string _strInterface = _ReportInfo.Find(x => x.Contains("Interface")).Split(',')[1];//DEV_INTERFACE_TYPE.USB_BRIDGE
                DEV_INTERFACE_TYPE _Interface = DEV_INTERFACE_TYPE.DEF;
                if(EnumTool.IsContainsDescription<DEV_INTERFACE_TYPE>(_strInterface))
                    _Interface = EnumTool.GetValueFromDescription<DEV_INTERFACE_TYPE>(_strInterface);
                else
                    _Interface = (DEV_INTERFACE_TYPE)Enum.Parse(typeof(DEV_INTERFACE_TYPE), _strInterface);
                //var _Interface = (DEV_INTERFACE_TYPE)Enum.Parse(typeof(DEV_INTERFACE_TYPE), _strInterface);
                //DEV_INTERFACE_TYPE.USB_BRIDGE
                Version tmpver = StringTool.HexStringToVer(_protocol);
                strProtocol = string.Format("Protocol_V{0}_{1}", tmpver.Major, _Interface.Desc());
                return strProtocol;
            }
        }

        public bool ResetProtocol(int iSelectedDev, string strProtocol)
        {
            DataRow[] rows = UI_GBV.mDev[iSelectedDev].AutoSelectTable.Select(string.Format("UI_ShowName = '{0}'", strProtocol));
            int iSelect = (rows.Count() != 0) ? UI_GBV.mDev[iSelectedDev].AutoSelectTable.Rows.IndexOf(rows[0]) : -1;
            if(iSelect < 0)
            {
                string str = "";
                if(strProtocol.Contains(ErrMsg.SensorTest.Report_Format_Not_Support.ToString()))
                    str = strProtocol;
                else
                    str = string.Format("Please check {0} !!\r\n{1}", UI_GBV.mDev[iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath, strProtocol);
                MyUIPage.ShowErrorDialog(str);
            }

            if(strProtocol.Contains("V6"))
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType = enPROTOCOL.V6;
            else if(strProtocol.Contains("V3"))
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType = enPROTOCOL.V3;
            else
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType = enPROTOCOL.none;

            bool bResult = (iSelect < 0) ? false : true;
            if(bResult)
            {
                UI_GBV.mDev[iSelectedDev].set_toolStrip(iSelect);
                UI_GBV.mDev[iSelectedDev].ChangeProtocol();//evayn
            }

            return bResult;
        }

        private void m_btSensrTest_D_Click(object sender, EventArgs e)
        {
            StaticVar.NowExecuteTool = enToolName.SensorTestTool;
            MainButton_Disable();
            string _str = "";
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_En;
            else
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_Cn;
            MyUIPage.ShowWaitForm(_str);

            Message _msg = new Message();
            _msg.WParam = (IntPtr)m_iSelectedDev;
            OnResetToolStatus(ref _msg);
            m_tabControl.SelectedTab = tabPage_SensorTest_D;
            //=================================
            string tmpINI_Profile = "";
            fm_SensorTest_D.UpdateProfilePath(out tmpINI_Profile);
            if(Path.IsPathRooted(tmpINI_Profile))
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath = tmpINI_Profile;
            else
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath = string.Format(@"{0}{1}", ITS_Directory.Tool_CurrentDirectory, tmpINI_Profile);

            if(!File.Exists(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath))
            {//2023/07/19 當history.ini的profile path不存在，直接回傳的話，會導致vfProtocolInit()沒有執行，所以Var_Multi.DevPanelInfo為null。是否需要再處理這塊，討論。
                fm_SensorTest_D.delSet_tabControl_TestBtnEnabled(false);
                fm_SensorTest_D.m_btRunTest.Enabled = false;
                fm_SensorTest_D.dataGridView_MainTestItem.Enabled = false;
                MyUIPage.HideWaitForm();
                return;
            }
            else
            {
                fm_SensorTest_D.delSet_tabControl_TestBtnEnabled(true);
                fm_SensorTest_D.m_btRunTest.Enabled = true;
                fm_SensorTest_D.dataGridView_MainTestItem.Enabled = true;
            }

            string INI_ProfileVer = "";
            if(!fm_SensorTest_D.CheckProfileVersion(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath, out INI_ProfileVer))
            {
                //string ver = string.Format("V{0}", string.Join(".", VersionInfo.ProfileVersion.Select(x => x.ToString()).ToArray()));
                //MyUIPage.ShowWarningDialog("Warning", string.Format("Profile version is incompatible!\r\nTool is V{0}\r\nINI is V{1}", VersionInfo.ProfileVersion, INI_ProfileVer), UIStyle.Orange, true);
                string str = "";
                if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    str = string.Format("Profile version is incompatible!\r\nTool is V{0}\r\nINI is V{1}", VersionInfo.ProfileVersion, INI_ProfileVer);
                else
                    str = string.Format("Profile版本不相符!\r\n軟體工具是 V{0}\r\n檔案是 V{1}", VersionInfo.ProfileVersion, INI_ProfileVer);
                MyUIPage.ShowWarningTip(str, 3000, true);
                fm_SensorTest_D.delSet_tabControl_TestBtnEnabled(false);
                MyUIPage.HideWaitForm();
                //return;
            }

            if(sender == null)//OP Mode
            {
                string strProtocol = "";
                string strMCU = "";
                if(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.bReadProtocol(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath, out strProtocol, out strMCU))
                {
                    if(ResetProtocol(m_iSelectedDev, strProtocol) == false)
                    {
                        MyUIPage.HideWaitForm();
                        return;
                    }
                }
            }

            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.ResetAllTestContains(enReadFrom.INI);
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.GenVal_fromIni(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);

            fm_SensorTest_D.TestItem_GridInitial();
            fm_SensorTest_D.TriggerOptionChange();
            UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen.CopyRunningProfile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.strProfileAbsolutePath);
            fm_SensorTest_D.ChangeAutoGen_UI(UI_GBV.mDev[m_iSelectedDev].m_GBV._SensorTestTool_.m_AutoGen);
            //=================================

            if(StaticVar.lsMain_USBDevInfo.Length <= 0)
            {
                MyUIPage.HideWaitForm();
                return;
            }

            switch(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType)
            {
                case DEV_INTERFACE_TYPE.HID:
                case DEV_INTERFACE_TYPE.I2C:
                case DEV_INTERFACE_TYPE.WIFI_HID:
                case DEV_INTERFACE_TYPE.WIFI_I2C:
                    vbButtonSelect(m_btSensrTest_Single);
                    break;
            }
            //=================================

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bI2C_ByPassINT = MY_INI.IniReadBool("SensorTest", "FWUpgrade_I2CByPassINT", false);
         
            fm_SensorTest_D.btn_ShowMPResult.Visible = StaticVar.bOPModeShowMPResult;
            fm_SensorTest_D.ShowBarCodeScanWaterMark(StaticVar.bShowBarCodeScanWaterMark);
            fm_SensorTest_D.ChangeTestInfoViewerVisible(StaticVar.bSensorTestInfoViewerVisible);

            if(UI_GBV.fm_SensorTest_Painting != null)
            {
                UI_GBV.fm_SensorTest_Painting.Close();
                UI_GBV.fm_SensorTest_Painting.Dispose();
            }

            UI_GBV.fm_SensorTest_Painting = new fmSensorTest_Painting(ref UI_GBV.mDev[m_iSelectedDev].LocalVariable, m_iSelectedDev);

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_SensorTest_D);
            MyUIPage.HideWaitForm();
            Reset_UI();
        }

        private void m_btSensrTest_S_Click(object sender, EventArgs e)
        {
            MainButton_Disable();
            string _str = "";
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_En;
            else
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_Cn;
            MyUIPage.ShowWaitForm(_str);
            StaticVar.NowExecuteTool = enToolName.MultiSensorTestTool;
            ResetAllToolStatus();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bI2C_ByPassINT = MY_INI.IniReadBool("SensorTest", "FWUpgrade_I2CByPassINT", false);

            //UI_GBV.fm_SensorTest_Painting = new fmSensorTest_Painting(ref UI_GBV.mDev[m_iSelectedDev].LocalVariable);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_PostMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_SensorTest_S);

            //=================================
            if(!fm_SensorTest_S.SensorTest_Multi_Main())
            {
                //if(fm_SensorTest_S.m_TriggerOptions == TriggerMode.GPIO)
                MyUIPage.HideWaitForm();

                switch(fm_SensorTest_S.m_TriggerOptions)
                {
                    case TriggerMode.GPIO:
                        MyUIPage.ShowWarningDialog("Warning!!", "Not support GPIO trigger.", UIStyle.LayuiOrange, true);
                        break;
                    case TriggerMode.BarCode:
                    case TriggerMode.BarCodeButton:
                        MyUIPage.ShowWarningDialog("Warning!!", "Not support BarCode trigger.", UIStyle.LayuiOrange, true);
                        break;
                    case TriggerMode.I2C:
                        MyUIPage.ShowWarningDialog("Warning!!", "Not support I2C trigger.", UIStyle.LayuiOrange, true);
                        break;
                }
            }
            else
            {
                vbButtonSelect(m_btSensrTest_Multi);
                MyUIPage.HideWaitForm();
            }

            Reset_UI();
        }

        private void m_btTuning_Click(object sender, EventArgs e)
        {
            StaticVar.NowExecuteTool = enToolName.TuningTool;
            MainButton_Disable();

            tab_Message.SelectedTab = tabPage_ErrorMSG;

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_TuningChangeControl, (IntPtr)TuningPage.WriteParaPage, (IntPtr)null);
            var _msg = new Message();
            _msg.WParam = (IntPtr)m_iSelectedDev;
            OnResetToolStatus(ref _msg);

            m_tabControl.SelectedTab = tabPage_Tuning;
            vbButtonSelect(m_btTuning);

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
            {
                ModeSelect[TouchMode.NormalMode](UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType);
                MyTool.Delay_ms(700);
                fm_TuningTool.vbInit();
            }

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.HIDE, IntPtr.Zero);

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus != DevStatus.Connected)
            {
                m_tbgTPTool.Enabled = true;
                m_tabControl.SelectedTab = tabPage_Tuning;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_TuningChangeControl, (IntPtr)TuningPage.TuningCompare, (IntPtr)null);
            }
            //else
            //{
            Reset_UI();
            //}
        }

        private void m_btUpgrade_Click(object sender, EventArgs e)
        {
            StaticVar.NowExecuteTool = enToolName.UpgradeTool;
            fm_FWUpgrade.StartProgramEnable = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_FWUpgrade);

            MainButton_Disable();

            var _msg = new Message();
            _msg.WParam = (IntPtr)m_iSelectedDev;
            OnResetToolStatus(ref _msg);

            m_tabControl.SelectedTab = tabPage_FWUpgrade;
            vbButtonSelect(m_btUpgrade);

            //if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.NowDevStatus == DevStatus.Connected)
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                ModeSelect[TouchMode.NormalMode](UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType);

            fm_FWUpgrade.UpgradeInit();
            Reset_UI();
        }

        //======================================
        private void SubscribeMessageReceiver()
        {
            mMessageReceiver.Clear();
            mMessageReceiver.Add(enWM_MSG.WM_I2CError, OnI2CError);
            mMessageReceiver.Add(enWM_MSG.WM_ToolClose, OnToolClose);
            mMessageReceiver.Add(enWM_MSG.WM_GUI_SET_BTNs, OnSet_UI_BottonsMode);
            mMessageReceiver.Add(enWM_MSG.WM_Change_UI_Status, On_Change_UI_Status);
            mMessageReceiver.Add(enWM_MSG.WM_tabControlSwitch, OntabControlSwitch);
            mMessageReceiver.Add(enWM_MSG.WM_CommonFlow_Msg, OnCommonFlowMsg);

            //WM_MainToolPanel
            mMessageReceiver.Add(enWM_MSG.WM_Devicechange, OnDevicechange);

            mMessageReceiver.Add(enWM_MSG.WM_SetProgressBar, OnSet_ProgressBar);

            mMessageReceiver.Add(enWM_MSG.WM_ShowShortTestPattern, OnShowShortTestPattern);

            mMessageReceiver.Add(enWM_MSG.WM_SetProgressBar_Maximum, OnSet_ProgressBarMax);
            mMessageReceiver.Add(enWM_MSG.WM_TableMainEnable, OnTableMainEnable);
            mMessageReceiver.Add(enWM_MSG.WM_Program_UI_Status, OnFWUpgrade_SetMainButton);
            mMessageReceiver.Add(enWM_MSG.WM_PLEASE_WAIT, OnShow_PleaseWait);

            mMessageReceiver.Add(enWM_MSG.WM_PenDrawingLine, OnPaint_PenLine);
            mMessageReceiver.Add(enWM_MSG.WM_PaintDrawingLine, OnPaint_FingerLine);
            mMessageReceiver.Add(enWM_MSG.WM_PaintMenuClose, OnPaintMenuClose);


            mMessageReceiver.Add(enWM_MSG.WM_PaintCMMonitorData, OnDebugMonitor);

            mMessageReceiver.Add(enWM_MSG.WM_PaintFullScreenExit, OnPaintFullScreenExit);
            mMessageReceiver.Add(enWM_MSG.WM_PaintFullScreenClear, OnPaintFullScreenClear);
            mMessageReceiver.Add(enWM_MSG.WM_PaintBackToHome, OnPaintBackToHome);

            mMessageReceiver.Add(enWM_MSG.WM_SenTest_All_Finish, OnSenTest_All_Finish);
            //20220629 Stevne Add
            mMessageReceiver.Add(enWM_MSG.WM_SenTest_UpdateTestInfo, OnSenTest_UpdateTestInfo);

            mMessageReceiver.Add(enWM_MSG.WM_SenTest_UIUpdata, OnSenTestUIUpdata);
            mMessageReceiver.Add(enWM_MSG.WM_SenTest_ModeChange, OnSenTest_TestMode);
            mMessageReceiver.Add(enWM_MSG.WM_SenTest_DrawingReset, OnSenTest_DrawingReset);
            mMessageReceiver.Add(enWM_MSG.WM_SenTest_PaintFinish, OnSenTest_PaintFinish);
            mMessageReceiver.Add(enWM_MSG.WM_ReceiverData, OnConsoleReceiverData);

            mMessageReceiver.Add(enWM_MSG.WM_SenTest_Warning_FormShow, OnWarningFormShow);

            mMessageReceiver.Add(enWM_MSG.WM_DBG_MSG_Show, OnDebugMessage);
            mMessageReceiver.Add(enWM_MSG.WM_CDC_FrameShow, OnCDC_FrameInfo);
            mMessageReceiver.Add(enWM_MSG.WM_CDC_Replay_FrameInfo, OnCDC_Replay_FrameInfo);
            mMessageReceiver.Add(enWM_MSG.WM_CDC_Replay_SwitchTab, OnSetCDCPlayTab);

            mMessageReceiver.Add(enWM_MSG.WM_TuningChangeControl, OnTuningChangeControl);
            mMessageReceiver.Add(enWM_MSG.WM_ResetToolStatus, OnResetToolStatus);
            mMessageReceiver.Add(enWM_MSG.WM_UsbDeviceNotification, OnRegisterUsbDeviceNotification);
            mMessageReceiver.Add(enWM_MSG.WM_SenTest_FPC_OpenTest, OnSensorTest_FPC_Open);

            //Steven 20221025
            mMessageReceiver.Add(enWM_MSG.WM_PARENTNOTIFY, OnCloseExtendUI);
            //Arik Demo
            //mMessageReceiver.Add(enWM_MSG.WM_SenTest_ShowErrorMsg, OnShowSensorTestErrorMsg);

            //Evayn 20230306
            mMessageReceiver.Add(enWM_MSG.WM_ShowErrorDialog, OnShowErrorDialog);
            mMessageReceiver.Add(enWM_MSG.WM_ShowErrorDialog_Mask, OnShowErrorDialog_Mask);
            mMessageReceiver.Add(enWM_MSG.WM_ShowTips, OnShowTips);
            mMessageReceiver.Add(enWM_MSG.WM_EnterCModelPage, OnClickCModelBtn);

            mMessageReceiver.Add(enWM_MSG.WM_Show_SensorTest_Warning_Box, OnShowSensorTestWarningBox);
            mMessageReceiver.Add(enWM_MSG.WM_CheckLCMFixtureGPIO, OnCheckLCMFixtureGPIO);


        }
        private void PlayBackMode(enPROTOCOL _Protocol)
        {
            //MyTool.Save_DBG_Log("PlayBackMode");

            StaticVar.IsPlayback_Mode = true;
            if(StaticVar.NowExecuteTool == enToolName.CDCTool)
                fm_CDCTool.CDC_Toolfm_Init(false);

            if(StaticVar.NowExecuteTool == enToolName.PaintTool)
                fm_PaintTool.Paint_Toolfm_Init();
        }

        private void TestMode(enPROTOCOL _Protocol)
        {
            fm_PaintTool.Hide();
            fm_CDCTool.Hide();
            fm_CDCTool.MenuStrip_CDC.Enabled = true;
            StaticVar.IsPlayback_Mode = false;

            var _msg = new Message();
            _msg.WParam = (IntPtr)m_iSelectedDev;
            OnResetToolStatus(ref _msg);

            //20220505 Steven Add : CDC預設為固定值
            //if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.PenMode_SubName == null)
            //{
            //    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            //    {
            //        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.NowSelect_CDC_Type(V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SE_BK);
            //        fm_CDCTool.ResetDicStripCB_PenDataType();
            //    }
            //    else if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
            //    {
            //        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.NowSelect_CDC_Type(V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_ALL_NODE);
            //    }
            //}

            fm_CDCTool.MenuItem_Start.Enabled = true;
            fm_CDCTool.Show();
            //MyTool.Save_DBG_Log("TestMode");
            fm_CDCTool.CDC_Toolfm_Init(false);
            fm_CDCTool.CDC_ThreadStart();
        }

        private void NormalMode(enPROTOCOL _Protocol)
        {
            StaticVar.IsPlayback_Mode = false;

            var _msg = new Message();
            _msg.WParam = (IntPtr)m_iSelectedDev;
            OnResetToolStatus(ref _msg);

            if(m_tabControl.SelectedTab == tabPage_PaintTool)
            {
                fm_PaintTool.Paint_Toolfm_Init();
                fm_PaintTool.Show();
                fm_PaintTool.Paint_Toolfm_Init(m_iSelectedDev);
                fm_PaintTool.OnLine_StartPaint();
            }
            else if(m_tabControl.SelectedTab == tabPage_CModel)
            {
                fm_C_Model.InitialDebugMode();
                fm_C_Model.Show();
            }
            else
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
            }

        }

        private void SuspendMode(enPROTOCOL _Protocol)
        {
            if(_Protocol == enPROTOCOL.V6)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.SuspendMode, Command._SWITCH.Dis);
            else if(_Protocol == enPROTOCOL.V3)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.TestMode, Command._SWITCH.En);
        }

        private void SubscribeModeSelectFunction()
        {
            ModeSelect.Clear();
            ModeSelect.Add(TouchMode.NormalMode, NormalMode);
            ModeSelect.Add(TouchMode.TestMode, TestMode);
            //ModeSelect.Add(TouchMode.DebugMode, DebugMode);
            ModeSelect.Add(TouchMode.SuspendMode, SuspendMode);
            ModeSelect.Add(TouchMode.Playback, PlayBackMode);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus != DevStatus.Connected)
                return;
            m_tbMain.Enabled = false;

            if(m_tbMain.SelectedTab == m_tbgTPTool || m_tbMain.SelectedTab == m_tbgInternalUse)
            {
                if(StaticVar.bTestFinishPwrDown
                    && StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.BridgePowerControl_API(Bridge._On, 1000);
                if(m_tabControl.SelectedTab != tabPage_Tuning)
                {
                    StaticVar.NowExecuteTool = enToolName.Main;
                    m_tabControl.SelectedTab = tabPage_Home;
                    if(!string.IsNullOrEmpty(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.LastSelectedProtocol))
                    {
                        if(ResetProtocol(m_iSelectedDev, UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.LastSelectedProtocol) == false)
                            ResetProtocol(m_iSelectedDev, string.Format("{0}{1}", IC_PROTOCOL_FUN.enum_Protocol_Name.Protocol_DEF, IC_PROTOCOL_FUN.enum_Interface_Name._DEF));

                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.vfProtocolInit();
                        if(StaticVar.iNowDeviceCnts > 0)
                        {
                            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bBridges_Pwr_Reset)
                                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.BridgeSettingInit_API(StaticVar.bridgeSetting.m_bridge_var.iPower_On_DelayTime);
                            m_btHome_Click(null, e);
                        }
                    }
                    else
                    {
                        if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected && m_tbMain.SelectedTab == m_tbgTPTool)
                            m_btHome_Click(null, e);
                    }
                }
            }
            else if(m_tbMain.SelectedTab == m_tbgSensorTest)
            {
                StaticVar.NowExecuteTool = enToolName.SensorTestTool;
                ResetAllToolStatus();
                if(StaticVar._ITSTool_Var != enMode.Multi_OPMode)
                {
                    var _selected = UI_GBV.mDev[m_iSelectedDev].m_stuProtocolTable.SelectedIndex;
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.LastSelectedProtocol = UI_GBV.mDev[m_iSelectedDev].m_stuProtocolTable.item[_selected].ToString();
                    m_tabControl.SelectedTab = tabPage_SensorTest_D;
                    m_btSensrTest_D_Click(null, null);
                }
                else if(StaticVar._ITSTool_Var == enMode.Multi_OPMode)
                {
                    StaticVar.NowExecuteTool = enToolName.MultiSensorTestTool;
                    m_tabControl.SelectedTab = tabPage_SensorTest_S;
                }
            }
            m_tbMain.Enabled = true;
            //Reset_UI(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.NowDevStatus);
            Reset_UI();
            if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode != FW_MODE.BL_MODE &&
              UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6 &&
              StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
            {
                GetPacketSizeFromSetting();
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.iFW_PrePacketSize != PacketSz.PacketSz_1024_)
                {
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.Set_SinglePacketLength((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.iFW_PrePacketSize);// Size
                }
            }
        }

        // Show location Time in TP Tool Status Script Lablel
        private void TimerClock_Tick(object sender, EventArgs e)
        {
            int Ima = 0;
            if(b_showCurrent)
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.ReadBridgeCurrent_API(ref Ima);
                fm_BridgeSetting.Update_rTextBox_Current(Ima.ToString() + "\n");
            }
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            m_tbMain.Enabled = false;
            btn_Connect.Enabled = false;
            richTB_ConsoleMesg.Clear();
            //連線時清空CDC播放內容
            ILITek_ITS_Tool.fm_CDCTool.CDC_Replay_ThreadStop(true);

            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.LastSelectedProtocol = "";
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bI2C_NACK_ERROR_RETURN_HOME = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.bReadParams_done = false;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.ConsoleTool.strInforMessage = "";
            UI_GBV.mDev[m_iSelectedDev].m_GBV.ConsoleTool.strErrorMessage = "";

            if(m_tbMain.SelectedTab == m_tbgSensorTest)
                m_tbMain.SelectedTab = m_tbgTPTool;

            bool bConnectionResult = false;
            //try
            //{
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_Home);
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Disconnect)
            {
                CommonFlow.setting[m_iSelectedDev].no_retry = false;
                CommonFlow.setting[m_iSelectedDev].not_change_normal_mode = false;
                CommonFlow.ilitek_dev_setting(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.setting[m_iSelectedDev]);
                StaticVar.bIsChangeToNormalMode = true;

                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6 &&
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum > Ver.AP_605)
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.LockPoint(Command._SWITCH.Dis);

                ResetAllToolStatus();

                Wifi_checkBox_.Enabled = false;
                Wifi_ip_address.Enabled = false;
                bConnectionResult = Wifi_Var.useWifi ? RemoteConnection() : LocalConnection();

                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.iFW_PrePacketSize != PacketSz.PacketSz_1024_ &&
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6 &&
                    StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                {
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.Set_SinglePacketLength((int)UI_GBV.mDev[m_iSelectedDev].LocalVariable.iFW_PrePacketSize);// Size

                    fm_BridgeSetting.UpdateActualConnectI2CAddress((UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bIsI2CAddressSettingCorrect) ?
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.iI2CAddress.ToString("X2") :
                        "41");

                }
                else if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                {
                    fm_BridgeSetting.UpdateActualConnectI2CAddress((UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bIsI2CAddressSettingCorrect) ?
                          UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.iI2CAddress.ToString("X2") :
                          "41");
                }
            }
            else
            {
                CommonFlow.setting[m_iSelectedDev].no_retry = true;
                CommonFlow.setting[m_iSelectedDev].not_change_normal_mode = false;
                CommonFlow.ilitek_dev_setting(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.setting[m_iSelectedDev]);
                StaticVar.bIsChangeToNormalMode = true;

                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6 &&
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum > Ver.AP_605)
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.LockPoint(Command._SWITCH.Dis);

                ExceptionStop();
                splitContainer1.Panel1Collapsed = false;
                if(StaticVar.bridgeSetting.m_bridge_var.bCurrentDetectEn)
                {
                    b_showCurrent = false;
                    TimerClock.Stop();
                }
                fm_BridgeSetting.Clean_rTextBox_Current();
            }

            btn_Connect.Enabled = true;
            //}
            //catch (Exception ex)
            //{
            //    MyUIPage.ShowErrorDialog("系统提示", ex.ToString(), UIStyle.Red, true);
            //    ExceptionStop();
            //}
            if(!bConnectionResult)
                ExceptionStop();

            m_tbMain.Enabled = true;
            //TODO:這裡要將記憶體清空
        }

        public void ExceptionStop()
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bForceStopCDC = true;
            toolStrip_BridgeVer.Visible = false;
            toolStrip_BridgeVer.Text = "";

            if(Wifi_Var.useWifi)
                RemoteDisconnect();
            else
                LocalDisconnect();

            //UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.NowDevStatus = DevStatus.Disconnect;
            StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus = DevStatus.Disconnect;
            Reset_ConnectionStatus(m_iSelectedDev, DevStatus.Disconnect);
            Reset_UI();
        }

        private void vbButtonSelect(Button btn)
        {
            foreach(var ctl in UI_GBV.ToolMain.btnMainButtons)
            {
                if(ctl.Value == btn)
                    ctl.Value.BackColor = ctl.Value.Enabled ? Color.Azure : Color.Silver;
                else
                    ctl.Value.BackColor = ctl.Value.Enabled ? Color.Beige : Color.Silver;
            }

            if((StaticVar.bridgeSetting.m_bridge_var.bCurrentDetectEn) && (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C))
                b_showCurrent = (btn == UI_GBV.ToolMain.btnMainButtons[Tool_BTNs.BTN_HOME]) ? true : false;

        }

        private void ResetAllToolStatus()
        {
            //UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(this.Handle, enWM_MSG.WM_UsbDeviceNotification, this.Handle, IntPtr.Zero);
            Message tmp = new Message();
            for(int i = 0; i < StaticVar.iMaxSupportDev; i++)
            {
                tmp.WParam = (IntPtr)i;
                OnResetToolStatus(ref tmp);
            }
        }

        private void OnResetToolStatus(ref Message msg)
        {
            int iDev = (int)msg.WParam;

            vfCloseAllThread(iDev);

            if(UI_GBV.mDev[iDev].LocalVariable._Console_VAR.bRunningStatus)
                fm_ConsoleTool.Console_Stop();

            if(fm_CDCTool != null)
                fm_CDCTool.CDC_ThreadStop(iDev);

            if(UI_GBV.mDev[iDev].LocalVariable.VarPaint_Multi.bRunningStatus)
            {
                if(fm_PaintTool != null)
                    fm_PaintTool.StopPaint();
            }

            if(UI_GBV.mDev[iDev].LocalVariable.VarUpgrade_Multi.bRunningStatus)
            {
                if(fm_FWUpgrade != null)
                {
                    fm_FWUpgrade.ForceClearSelectItem();
                    fm_FWUpgrade.vbStop();
                }
            }

            if(UI_GBV.mDev[iDev].LocalVariable.TuningVar.bRunningStatus)
            {
                if(fm_TuningTool != null)
                    fm_TuningTool.vfStopTuning();
            }

            if(UI_GBV.mDev[iDev].LocalVariable.VarCModel_Multi.bRunningStatus)
            {
                if(fm_C_Model != null)
                    fm_C_Model.StopDebugMode();
            }
        }

        private void vfCloseAllThread(int iSelectedDev)
        {
            if(UI_GBV.mDev[iSelectedDev].m_GBV.Globe_Multi.ThreadFun != null)
                if((UI_GBV.mDev[iSelectedDev].m_GBV.Globe_Multi.ThreadFun.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) == 0)
                    MyTool.TerminateThreadAPI(UI_GBV.mDev[iSelectedDev].m_GBV.Globe_Multi.ThreadFun.ManagedThreadId, 0);
        }

        private void richTextBox_ErrMSG_TextChanged(object sender, EventArgs e)
        {
            richTextBox_ErrMSG.SelectionStart = richTextBox_ErrMSG.TextLength;
            richTextBox_ErrMSG.ScrollToCaret();
        }

        private void m_btLogAnalyzer_Click(object sender, EventArgs e)
        {
            if(!System.IO.File.Exists(UI_GBV.LogAnalysisTool))
            {
                MyUIPage.ShowErrorDialog("Turn on a additional feature", UI_GBV.LogAnalysisTool + " doesn't exist.", UIStyle.Red, true);
                return;
            }
            //StaticVar.NowExecuteTool = enToolName.AnalyzerTool;
            //m_btLogAnalyzer.Click -= m_btLogAnalyzer_Click;

            vbButtonSelect(m_btLogAnalyzer);
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = @"LogAnalysisTool";
            startInfo.FileName = UI_GBV.LogAnalysisTool;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "";

            using(Process exeProcess = Process.Start(startInfo))
            {
                //exeProcess.WaitForExit();
                //exeProcess.Close();
                //m_btLogAnalyzer.Click += m_btLogAnalyzer_Click;
            }
        }

        private void Wifi_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if(Wifi_checkBox_.Checked)
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_UsbDeviceNotification, (IntPtr)m_iSelectedDev, IntPtr.Zero);
                Wifi_Var.useWifi = true;
                StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus = DevStatus.Disconnect;

                /* UI Setting */
                string serverName = Dns.GetHostName();

                Wifi_ip_address.Visible = true;
                Wifi_ip_address.BeginUpdate();

                Wifi_ip_address.Items.Clear();
                for(int idx = 0; idx < Dns.GetHostEntry(serverName).AddressList.Length; idx++)
                {
                    if(Dns.GetHostEntry(serverName).AddressList[idx].AddressFamily == AddressFamily.InterNetworkV6)
                        continue;
                    Wifi_ip_address.Items.Add(Dns.GetHostEntry(serverName).AddressList[idx].ToString());
                }
                Wifi_ip_address.Items.Add(Wifi_Var.adb_socket_ip);

                Wifi_ip_address.EndUpdate();
                Wifi_ip_address.SelectedIndex = 0;

                btn_Connect.Enabled = true;
                btn_Connect.Image = Properties.Resources.disconnect;
                btn_Connect.Text = "Disconnect";

                fm_WifiSetting = new WifiSettingfm();
                SetFormProperty(fm_WifiSetting, m_panBridgeSetting, true);
            }
            else
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_UsbDeviceNotification, (IntPtr)m_iSelectedDev, StaticVar.MainFormHandle);
                Wifi_Var.useWifi = false;
                StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus = DevStatus.DeviceRemove;

                //TODO: [Joe] 建議開一個 remove device 的 api 統一處理 remove 流程
                StaticVar.lsMain_USBDevInfo[m_iSelectedDev].Dev_Item_Tag = null;

                /* UI Setting */
                Wifi_ip_address.Visible = false;

                btn_Connect.Enabled = false;
                btn_Connect.Image = Properties.Resources.disconnect;
                btn_Connect.Text = "Disconnect";

                SetFormProperty(fm_BridgeSetting, m_panBridgeSetting, true);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(splitContainer_MainUI.Panel1Collapsed)
            {
                tsb_collapse.Image = global::ITS_Studio.Properties.Resources.Actions_go_up_icon;
                splitContainer_MainUI.Panel1Collapsed = false;
                if(m_tabControl.SelectedTab == tabPage_PaintTool)
                    fm_PaintTool.vfChangeResolution();
            }
            else
            {
                tsb_collapse.Image = global::ITS_Studio.Properties.Resources.Actions_go_down_icon;
                splitContainer_MainUI.Panel1Collapsed = true;
                if(m_tabControl.SelectedTab == tabPage_PaintTool)
                    fm_PaintTool.vfChangeResolution();
            }
        }

        private void m_btCModel_Click()
        {
            StaticVar.NowExecuteTool = enToolName.C_Model;
            MainButton_Disable();

            //全部狀態都Reset
            var _msg = new Message();
            _msg.WParam = (IntPtr)m_iSelectedDev;
            OnResetToolStatus(ref _msg);
            fm_ExtendFlowLayout.Visible = false;
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                MyTool.Delay_ms(1000);//因為從CDC切換過來的時候，要讓FW有時間切換。

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus != DevStatus.Connected)
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_CModel_Replay);
            else
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_CModel);


            //tabPage_CModel.Controls.Clear();

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                ModeSelect[TouchMode.NormalMode](UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType);
            else
                ModeSelect[TouchMode.Playback](UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType);

            //tabPage_CModel.Controls.Add(fm_C_Model);

            //補充Initial Function....

            //Wait until the last step is over before you can continue to go down.

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_DBG_MSG_Show, (IntPtr)enDBG_MSG_.CLEAR, "");
            Reset_UI();
            vbButtonSelect(fm_ExtendFlowLayout.m_btCModel);
        }

        private void m_btFuncGen_Click(object sender, EventArgs e)
        {
        }

        private void fm_ExtendFlowLayout_EncryptClickEvent()
        {
            var _path = Application.StartupPath + @"\EncryptionTool\EncryptionApplication.exe";
            string msg = "", title = "";
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
            {
                msg = global::ITS_Studio.Properties.Resources.AdditionalFeatureMsg_EN;
                title = global::ITS_Studio.Properties.Resources.AdditionalFeatureTitle_EN;
            }
            else
            {
                msg = global::ITS_Studio.Properties.Resources.AdditionalFeatureMsg_CN;
                title = global::ITS_Studio.Properties.Resources.AdditionalFeatureTitle_CN;
            }
            if(!File.Exists(_path))
            {
                MyUIPage.ShowErrorDialog(title, string.Format(msg, Path.GetFileName(_path)), UIStyle.Red, true);
                return;
            }
            if(MutexCheckApp(_path) >= 1)
            {
                if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    MyUIPage.ShowErrorDialog("System Tips", "The application is already running...", UIStyle.Red, true);
                else
                    MyUIPage.ShowErrorDialog("系统提示", "應用程式已經在執行中...", UIStyle.Red, true);
                return;
            }

            fm_ExtendFlowLayout.Visible = false;

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            //startInfo.WorkingDirectory = "HexToILI";
            startInfo.FileName = "EncryptionApplication.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;//背景執行
            startInfo.Arguments = "";
            Process exeProcess = Process.Start(startInfo);
        }

        private void m_btHexToIli_Click()
        {
            var _path = Application.StartupPath + @"\HexToILI\ILITek_HexToILI.exe";
            string msg = "", title = "";
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
            {
                msg = global::ITS_Studio.Properties.Resources.AdditionalFeatureMsg_EN;
                title = global::ITS_Studio.Properties.Resources.AdditionalFeatureTitle_EN;
            }
            else
            {
                msg = global::ITS_Studio.Properties.Resources.AdditionalFeatureMsg_CN;
                title = global::ITS_Studio.Properties.Resources.AdditionalFeatureTitle_CN;
            }
            if(!File.Exists(_path))
            {
                MyUIPage.ShowErrorDialog(title, string.Format(msg, Path.GetFileName(_path)), UIStyle.Red, true);
                return;
            }

            fm_ExtendFlowLayout.Visible = false;

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = "HexToILI";
            startInfo.FileName = "HexToILI/ILITek_HexToILI.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "";
            Process exeProcess = Process.Start(startInfo);
        }

        private void m_btHexToBin_Click()
        {
            var _path = Application.StartupPath + @"\HexToBin\hex2bin.exe";
            string msg = "", title = "";
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
            {
                msg = global::ITS_Studio.Properties.Resources.AdditionalFeatureMsg_EN;
                title = global::ITS_Studio.Properties.Resources.AdditionalFeatureTitle_EN;
            }
            else
            {
                msg = global::ITS_Studio.Properties.Resources.AdditionalFeatureMsg_CN;
                title = global::ITS_Studio.Properties.Resources.AdditionalFeatureTitle_CN;
            }
            if(!File.Exists(_path))
            {
                MyUIPage.ShowErrorDialog(title, string.Format(msg, Path.GetFileName(_path)), UIStyle.Red, true);
                return;
            }
            fm_ExtendFlowLayout.Visible = false;

            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.FileName = "";
            openDlg.CheckFileExists = true;
            openDlg.CheckPathExists = true;
            openDlg.Multiselect = false;
            openDlg.InitialDirectory = Environment.CurrentDirectory;

            if(openDlg.ShowDialog(this) != DialogResult.OK)
                return;

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = "HexToBin";
            startInfo.FileName = "HexToBin/hex2bin.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = string.Format("\"{0}\"", openDlg.FileName);

            using(Process exeProcess = Process.Start(startInfo))
            {
                ;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;

            string keyword = "";
            string pdf = "";
            if(ci.DisplayName.Contains("Chinese"))
                keyword = "Windows_Studio_ITS_User_Guide(CHT)";
            else
                keyword = "Windows_Studio_ITS_User_Guide(ENG)";

            if(!Directory.Exists(@"Document"))
            {
                MyUIPage.ShowErrorDialog("Show User Manual", "Document\\ doesn't exist.", UIStyle.Red, true);
                return;
            }

            string[] files = Directory.GetFiles(@"Document", "*.pdf");
            foreach(var file in files)
                if(file.Contains(keyword))
                    pdf = file;

            if(!File.Exists(pdf))
            {
                MyUIPage.ShowErrorDialog("Show User Manual", pdf + " doesn't exist.", UIStyle.Red, true);
                return;
            }
            ProcessStartInfo startInfo = new ProcessStartInfo(pdf);
            Process.Start(startInfo);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if(!System.IO.File.Exists("Update\\Windows_Download_ITS.exe"))
            {
                MyUIPage.ShowErrorDialog("Turn on a additional feature", "Windows_Download_ITS.exe doesn't exist.", UIStyle.Red, true);
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "Update\\Windows_Download_ITS.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //{0}: Windows_Studio_ITS.exe
            //{1}: its ver string "0.1.0.0"
            //{2}: 0:更新ITS 1: 下載附加功能
            //{3}: 1:下載beta; 0: 下載正式版
            int beta_flag = 1;
#if (RELEASE)
            beta_flag = 0;
#endif

            startInfo.Arguments = String.Format("Windows_Studio_ITS.exe {0} 1 {1}", StaticVar.ITS_ToolVer, beta_flag);
            startInfo.Verb = "runas";
            using(Process exeProcess = Process.Start(startInfo))
            {
                //int result = exeProcess.ExitCode;
                //MyUIPage.ShowErrorDialog("Turn on a additional feature", "Windows_Download_ITS.exe doesn't exist.", UIStyle.Red, true);
                exeProcess.WaitForExit();
                exeProcess.Close();
            }
        }

        private void m_btOpenExtendUI_Click(object sender, EventArgs e)
        {
            Point actualPoint = this.PointToScreen(m_btOpenExtendUI.Location);
            fm_ExtendFlowLayout.Location = new Point(actualPoint.X, actualPoint.Y + ITS_MainTool_splitContainer.Height + 25);
            fm_ExtendFlowLayout.Visible = !fm_ExtendFlowLayout.Visible;
        }

        private void btn_FreSpectrum_Click()
        {
            StaticVar.NowExecuteTool = enToolName.FreSpectrumTool;

            var _msg = new Message();
            _msg.WParam = (IntPtr)m_iSelectedDev;
            OnResetToolStatus(ref _msg);

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                MyTool.Delay_ms(1000);//因為從CDC切換過來的時候，要讓FW有時間切換。
            fm_ExtendFlowLayout.Visible = false;

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_Tuning);

            //20220510 Steven 修改順序，先創建Handle再ShowBarData
            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._NoiseFrq_.vbStartNoiseFre();

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_TuningChangeControl, (IntPtr)TuningPage.NoiseFrePage, (IntPtr)null);

            UI_GBV.fmITS_Tool.MainButton_Disable();
        }

        private void btnChargDump_Click()
        {
            StaticVar.NowExecuteTool = enToolName.ChargeDumpTool;

            var _msg = new Message();
            _msg.WParam = (IntPtr)m_iSelectedDev;
            OnResetToolStatus(ref _msg);

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                MyTool.Delay_ms(1000);//因為從CDC切換過來的時候，要讓FW有時間切換。
            fm_ExtendFlowLayout.Visible = false;

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_Tuning);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_TuningChangeControl, (IntPtr)TuningPage.ChargeCurve, (IntPtr)null);
            UI_GBV.fmITS_Tool.MainButton_Disable();
        }

        private void m_tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            fm_ExtendFlowLayout.Visible = false;
            var MySender = sender as UITabControl;
            //Console.WriteLine("[m_tabControl_SelectedIndexChanged] = {0}", MySender.SelectedTab.Name);
            //Console.WriteLine("[m_tabControl_SelectedIndexChanged] LastExecuteTool = {0};NowExecuteTool = {1}"
            //    , StaticVar.LastExecuteTool, StaticVar.NowExecuteTool);
            if(StaticVar.NowExecuteTool == enToolName.SensorTestTool)
                fm_SensorTest_D.SensorTest_FormClose();
            if(StaticVar.NowExecuteTool == enToolName.MultiSensorTestTool)
                fm_SensorTest_S.SensorTest_Multi_FormClose();
            //StaticVar.NowExecuteTool = enToolName.Main;
        }

        private void ITS_Tool_Click(object sender, EventArgs e)
        {
            OpenAutoUpdater(true);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            menutool_autoUpdater.Checked = MY_INI.IniReadBool("Main", "AutoUpdate_Enable", false);
        }

        private void menutool_autoUpdater_Click(object sender, EventArgs e)
        {
            var MySender = sender as ToolStripMenuItem;
            MY_INI.IniWriteValue("Main", "AutoUpdate_Enable", MySender.Checked.ToString());
        }

        private void num_Win2Threshold_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown Mysender = sender as NumericUpDown;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.iWin2Threshold = (int)Mysender.Value;
        }

        public void UpdateRichTextBox(string msg, Color color)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    UpdateRichTextBox(msg, color);
                }));
            }
            else
            {
                if(richTextBox_ErrMSG.Lines.Length - 2 >= 0 && msg == richTextBox_ErrMSG.Lines[richTextBox_ErrMSG.Lines.Length - 2])
                    return;
                //set HideSelection to false it will keep its selection when it loses focus and stay auto-scrolled.
                richTextBox_ErrMSG.HideSelection = false;

                richTextBox_ErrMSG.SelectionStart = richTextBox_ErrMSG.TextLength;
                richTextBox_ErrMSG.SelectionLength = 0;
                richTextBox_ErrMSG.Focus();
                richTextBox_ErrMSG.SelectionColor = color;
                richTextBox_ErrMSG.AppendText(msg);
                richTextBox_ErrMSG.SelectionColor = richTextBox_ErrMSG.ForeColor;
            }
        }

        private void btnGenProfile_Click()
        {
            StaticVar.NowExecuteTool = enToolName.GenProfile;

            fm_ExtendFlowLayout.Visible = false;
            fm_GenProfile.Init();

            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_Tuning);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_TuningChangeControl, (IntPtr)TuningPage.GenProfile, (IntPtr)null);
            UI_GBV.fmITS_Tool.MainButton_Disable();
        }

        private void ClearRBuffer(ref byte[] rBuffer)
        {
            rBuffer = new byte[64];
            rBuffer[0] = 0x03;
        }

        enum EINK_CMD_Value : int
        {
            Read_register = 0x0001,
            Write_register = 0x0002,
            DMA1_Read = 0x0003,
            DMA2_Read = 0x0004,
            DMA3_Read = 0x0005,
            DMA4_Read = 0x0006,
            INT_Read = 0x8069,
        }

        private byte[] AssignData(EINK_CMD_Value CMD_Value, int Address, int Value, int w_length, int r_length)
        {
            byte[] wbuf = new byte[64];
            wbuf[0] = 0x03;
            wbuf[1] = 0xA3;
            wbuf[2] = (byte)w_length;
            wbuf[3] = (byte)r_length;
            wbuf[4] = (byte)(((int)CMD_Value & 0x0000FF00) >> 8);
            wbuf[5] = (byte)((int)CMD_Value & 0x000000FF);
            wbuf[6] = (byte)((Address & 0x0000FF00) >> 8);
            wbuf[7] = (byte)(Address & 0x000000FF);
            wbuf[8] = (byte)((Value & 0x0000FF00) >> 8);
            wbuf[9] = (byte)(Value & 0x000000FF);
            ;

            return wbuf;
        }

        private byte[] AssignData_Long(EINK_CMD_Value CMD_Value, int Address, int Value, int w_length, int r_length, bool isLongFormat = false)
        {
            byte[] wbuf = new byte[64];
            wbuf[0] = 0x03;
            wbuf[1] = 0xA4;
            wbuf[2] = (byte)((int)w_length & 0x000000FF);
            wbuf[3] = (byte)(((int)w_length & 0x0000FF00) >> 8);
            wbuf[4] = (byte)((int)r_length & 0x000000FF);
            wbuf[5] = (byte)(((int)r_length & 0x0000FF00) >> 8);
            wbuf[6] = (byte)(((int)CMD_Value & 0x0000FF00) >> 8);
            wbuf[7] = (byte)((int)CMD_Value & 0x000000FF);
            wbuf[8] = (byte)((Address & 0x0000FF00) >> 8);
            wbuf[9] = (byte)(Address & 0x000000FF);
            wbuf[10] = (byte)((Value & 0x0000FF00) >> 8);
            wbuf[11] = (byte)(Value & 0x000000FF);
            ;

            return wbuf;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] wbuf = new byte[64];
            byte[] rbuf = new byte[64];

            ClearRBuffer(ref rbuf);

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.BridgeSettingInit_API(StaticVar.bridgeSetting.m_bridge_var.iPower_On_DelayTime);
            string bridge_mess = "";
            bool bConnectResult = UI_GBV.mDev[m_iSelectedDev].fakeConnectionBase(out bridge_mess);


            //UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

            bool isSPIWithHeader = MY_INI.IniReadBool("Setting_Bridge", "SPI_With_Header", true);

            if(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.vfSetSPITestModeEnable(!isSPIWithHeader);

            if(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.SetBridge_INT_Status_API(GPIO_PIN_TYPE.HIGH))

                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);

            HID_API m_HID_API = new HID_API();
            IntPtr hNowDev = Static_HID.CreateFile(
                StaticVar.lsMain_USBDevInfo[m_iSelectedDev].strDevPath,
                enFILE.GENERIC_READ,
                enFILE.FILE_SHARE_READ | enFILE.FILE_SHARE_WRITE,
                IntPtr.Zero,
                enFILE.OPEN_EXISTING,
                enFILE.FILE_ATTRIBUTE_NORMAL,
                IntPtr.Zero);
            bool bUSBMonitor_Opened = m_HID_API.HID_CreateHandle(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].strDevPath);

            wbuf = AssignData(EINK_CMD_Value.Read_register, 0x0000, 0x0000, 6, 6);
            while(!(rbuf[8] == 0x55 && rbuf[9] == 0xAA))
            {
                ClearRBuffer(ref rbuf);
                m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);
                m_HID_API.HID_MonitorRead(out rbuf, ref UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bUSBMonitor_Opened);
            }
            MyTool.Delay_ms(1);

            wbuf = AssignData(EINK_CMD_Value.Write_register, 0x0013, 0xA200, 6, 0);
            m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);
            MyTool.Delay_ms(1);

            wbuf = AssignData(EINK_CMD_Value.Read_register, 0x0013, 0x0000, 6, 6);
            while(!(rbuf[8] == 0x02 && rbuf[9] == 0x00))
            {
                ClearRBuffer(ref rbuf);
                m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);
                m_HID_API.HID_MonitorRead(out rbuf, ref UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bUSBMonitor_Opened);
            }
            ClearRBuffer(ref rbuf);
            MyTool.Delay_ms(1);

            wbuf = AssignData(EINK_CMD_Value.Write_register, 0x0004, 0x1311, 6, 0);
            m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);
            MyTool.Delay_ms(1);

            wbuf = AssignData(EINK_CMD_Value.Write_register, 0x0005, 0x1400, 6, 0);
            m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);
            MyTool.Delay_ms(1);

            wbuf = AssignData(EINK_CMD_Value.Write_register, 0x0006, 0x3600, 6, 0);
            m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);
            MyTool.Delay_ms(1);

            wbuf = AssignData(EINK_CMD_Value.Write_register, 0x0007, 0x0000, 6, 0);
            m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);
            MyTool.Delay_ms(1);

            wbuf = AssignData(EINK_CMD_Value.Write_register, 0x0008, 0x0003, 6, 0);
            m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);
            MyTool.Delay_ms(1);

            wbuf = AssignData(EINK_CMD_Value.Write_register, 0x0009, 0x0000, 6, 0);
            m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);
            MyTool.Delay_ms(1);

            wbuf = AssignData(EINK_CMD_Value.Write_register, 0x000A, 0x0000, 6, 0);
            m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);
            MyTool.Delay_ms(1);

            wbuf = AssignData(EINK_CMD_Value.Write_register, 0x000B, 0x000C, 6, 0);
            m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);
            MyTool.Delay_ms(1);

            wbuf = AssignData(EINK_CMD_Value.Write_register, 0x000C, 0x3E80, 6, 0);
            m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);
            MyTool.Delay_ms(1);


            //wbuf = AssignData(EINK_CMD_Value.Write_register, 0x0001, 0x1023, 6, 0);

            wbuf = new byte[64];
            wbuf[0] = 0x03;
            wbuf[1] = 0xF8;
            wbuf[2] = 0x01;
            wbuf[3] = 0x1B; //27_Line
            m_HID_API.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], wbuf, false);

            m_HID_API.HID_MonitorRead(out rbuf, ref UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bUSBMonitor_Opened);
            if(rbuf[1] == 0xAD)
            {
                rbuf = new byte[4096];
                rbuf[0] = 0x03;

                m_HID_API.HIDD_Get_Feature(hNowDev, rbuf, rbuf.Length);
            }
            //Console.Write(rbuf[1]);
        }
    }

    public static class StressMarkExtensions
    {
        public static void MySetStyle(this UIMarkLabel label, STRESS_BUILD_Show style)
        {
            if(!Program.bSTRESS_BUILD)
                return;
            if((style == STRESS_BUILD_Show.OK) && (label.Text == STRESS_BUILD_Show.NG.ToString()))
                return;
            label.Text = style.ToString();

            switch(style)
            {
                case STRESS_BUILD_Show.Ready:
                    label.BackColor = Color.Transparent;
                    break;
                case STRESS_BUILD_Show.OK:
                    label.BackColor = Color.Green;
                    break;
                case STRESS_BUILD_Show.NG:
                    label.BackColor = Color.Red;
                    break;
            }
        }
    }
}