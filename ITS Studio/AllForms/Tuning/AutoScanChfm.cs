using CommonExt;
using FuncMethodCmd;
using ITS_Studio.AllForms;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ThirdPartyTools;

namespace ITS_Studio
{
    public partial class AutoScanCHfm : Form
    {
        enum Scan_CH_Range_V3 : int
        {
            TX_START_CH = 0,
            TX_END_CH,
            RX_START_CH,
            RX_END_CH,
        }
        enum Scan_CH_Range_V6_Die1 : int
        {
            TX0_START_CH = 0,
            TX0_END_CH,
            RX0_START_CH,
            RX0_END_CH,
        }
        enum Scan_CH_Range_V6_Die2 : int
        {
            TX1_START_CH = 4,
            TX1_END_CH,
            RX1_START_CH,
            RX1_END_CH,
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
        private UIPage MyUIPage = new UIPage();
        private Dictionary<enWM_MSG, function> mMessageReceiver = new Dictionary<enWM_MSG, function>();

        //private TuningToolfm fm_TuningTool = new TuningToolfm();
        private DynamicTool MyTool = new DynamicTool();

        private bool bCommitPara = false;
        private bool ScanChResult = true;
        private TuningTool _TuningTool_;
        private INI_Tool CDC_Tool_INI = new INI_Tool(ITS_Directory.CDCTool_INI);

        public AutoScanCHfm()
        {
            InitializeComponent();
            StaticVar.AutoScanFormHandle = this.Handle;
            StaticVar.AutoScan_WaitUIPanelHandle = this.panel1.Handle;

            mMessageReceiver.Add(enWM_MSG.WM_AutoScanCH_Message, OnAutoScanMessage);
        }

        protected override void WndProc(ref Message msg)
        {
            enWM_MSG pEnumPanoMsg = (enWM_MSG)msg.Msg;
            if(mMessageReceiver.Keys.Contains(pEnumPanoMsg))
                mMessageReceiver[pEnumPanoMsg](ref msg);
            base.WndProc(ref msg);
        }

        private void OnAutoScanMessage(ref Message msg)
        {
            var iMsgType = (enDBG_MSG_)msg.WParam;
            if(iMsgType == enDBG_MSG_.CLEAR)
            {
                richTextBox1.Clear();
            }
            else
            {
                string str = Marshal.PtrToStringAuto(msg.LParam);
                if(str.Contains("[ERROR]"))
                {
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectionFont = new Font("Consolas", 12.0F, FontStyle.Bold);
                }
                else if(str.Contains("[DONE]"))
                {
                    richTextBox1.SelectionColor = Color.Green;
                    richTextBox1.SelectionFont = new Font("Consolas", 12.0F, FontStyle.Bold);
                }
                else
                {
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.SelectionFont = new Font("Consolas", 12.0F, FontStyle.Regular);
                }
                richTextBox1.AppendText(str);
            }
        }

        private bool bfUpdateChannelVal(int[] val)
        {
            bool bResult = true;
            string FailItem = "";
            switch(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType)
            {
                case enPROTOCOL.V3:
                    foreach(var item in EnumTool.EnumToList<Scan_CH_Range_V3>())
                    {
                        if(_TuningTool_.bfSetDataToTable(item.ToString(), val[(int)item]))
                            continue;
                        FailItem = item.ToString();
                        bResult = false;
                        break;
                    }
                    break;

                case enPROTOCOL.V6:
                    foreach(var item in EnumTool.EnumToList<Scan_CH_Range_V6_Die1>())
                    {
                        if(_TuningTool_.bfSetDataToTable(item.ToString(), val[(int)item]))
                            continue;
                        FailItem = item.ToString();
                        bResult = false;
                        break;
                    }

                    if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iDieNum == 2)
                    {
                        foreach(var item in EnumTool.EnumToList<Scan_CH_Range_V6_Die2>())
                        {
                            if(_TuningTool_.bfSetDataToTable(item.ToString(), val[(int)item]))
                                continue;
                            FailItem = item.ToString();
                            bResult = false;
                            break;
                        }
                    }
                    break;
            }
            if(!bResult)
            {
                if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    MyUIPage.ShowErrorNotifier(string.Format("Failed! Can not find the key_word: {0}", FailItem), true);
                else
                    MyUIPage.ShowErrorNotifier(string.Format("失敗! 找不到關鍵字: {0}", FailItem), true);
            }
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_UI_Init, (IntPtr)0, (IntPtr)0);
            return bResult;
        }

        private void Bt_Exit_Click(object sender, EventArgs e)
        {
            UI_GBV.fmITS_Tool.MainButton_Disable();
            btnScanChStart.Enabled = false;
            string strErrMessage = "";
            if(ScanChResult && bCommitPara)
            {
                if(_TuningTool_.vbWriteParameter(ref strErrMessage))
                {
                    MyTool.Delay_ms(1000);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.SuspendMode, Command._SWITCH.Dis);
                    string str_1 = "", str2 = "";
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.Get_IC_Information(UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode
                        , false, ref str_1, ref str2);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_UI_Init, (IntPtr)0, (IntPtr)0);
                }
                else
                {
                    string msg = "";
                    if(_TuningTool_.m_write_param_CRC_ret == TuningTool.en_V6_Para_CRC_Ret.Modify_ByFw)
                    {
                        bool bResult = _TuningTool_.vbReadParameter(ref strErrMessage);
                        if(bResult)
                        {
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.TuningFormHandle, enWM_MSG.WM_Tuning_UI_Init, (IntPtr)0, (IntPtr)0);
                        }
                        else
                        {
                            if(sender != null)
                            {
                                if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                                    msg = "Failed to read parameter!";
                                else
                                    msg = "讀取參數失敗!";
                            }
                            else
                            {
                                msg = strErrMessage;
                            }

                            MyUIPage.ShowErrorDialog("Auto Scan Channel", msg, UIStyle.Red, true);
                        }
                    }
                    else
                    {
                        if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                            msg = string.Format("Transfer error!, Return = {0}", _TuningTool_.m_write_param_CRC_ret);
                        else
                            msg = string.Format("傳輸錯誤!, Return = {0}", _TuningTool_.m_write_param_CRC_ret);
                        MyUIPage.ShowErrorDialog("Auto Scan Channel", msg, UIStyle.Red, true);
                    }
                    string str_1 = "", str2 = "";
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.SuspendMode, Command._SWITCH.Dis);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.Get_IC_Information(UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode
                        , false, ref str_1, ref str2);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
                }
                ScanChResult = false;

                if (Wifi_Var.useWifi)
                    Wifi_Var.Update_Panelinfo();

                /* update commonflow's panel info. */
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.m_HID_API.HID_CreateHandle(
                    StaticVar.lsMain_USBDevInfo[m_iSelectedDev].strDevPath);
                CommonFlow.api_update_ts_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.commonflow_get_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.info[m_iSelectedDev]);
            }
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_TuningChangeControl, (IntPtr)TuningPage.WriteParaPage, (IntPtr)null);

            UI_GBV.fmITS_Tool.Reset_UI();
            btnScanChStart.Enabled = true;
        }

        private void btnScanChStart_Click(object sender, EventArgs e)
        {
            btnScanChStart.Enabled = false;
            Bt_Exit.Enabled = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.AutoScanFormHandle, enWM_MSG.WM_AutoScanCH_Message, (IntPtr)enDBG_MSG_.CLEAR, "");

            ILITek_ITS_Tool.fm_CDCTool.ResetAxesLimit();
            string strErrorMSG = "";
            UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType = enCDCDataFormat.inOriginal;
            //DialogResult tmpResult = DialogResult.Cancel;
            ScanChResult = true;
            do
            {
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum == Ver.AP_600)
                {
                    if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                        strErrorMSG += string.Format(" [auto scan channel] Not support protocol {0}\n", UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum.ToString());
                    else
                        strErrorMSG += string.Format(" [auto scan channel] 不支援協議版本 {0}\n", UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum.ToString());
                    ScanChResult = false;
                    break;
                }

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.SHOW, (IntPtr)StaticVar.AutoScan_WaitUIPanelHandle);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.AutoScanFormHandle, enWM_MSG.WM_AutoScanCH_Message, (IntPtr)enDBG_MSG_.INFO_MSG_APPEND, "Start to scan...\n");

                byte[] channel_range = new byte[8];
                ScanChResult = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._AutoScanCh_.AutoScanChannel(CDC_Tool_INI, out channel_range);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.HIDE, (IntPtr)StaticVar.AutoScan_WaitUIPanelHandle);
                if(!ScanChResult)
                {
                    //if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                        strErrorMSG += "Failed to Auto Scan Channel.\n";
                    //else
                        //strErrorMSG += "執行AutoScanChannel失敗.\n";
                    break;
                }

                string StrPrt = "";
                string strTitle = "";
                if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iDieNum != 1)
                {
                    if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    {
                        strTitle = "Confirm Message";
                        StrPrt = string.Format("Define the pin range:\n"
                            + "RX0 Pin Range = RX{0} ~ RX{1}\n"
                            + "TX0 Pin Range = TX{2} ~ TX{3}\n"
                            + "RX1 Pin Range = RX{4} ~ RX{5}\n"
                            + "TX1 Pin Range = TX{6} ~ TX{7}\n"
                            + "Overwrite above settings into table [CDC] and write parameters to FW?"
                            , channel_range[2], channel_range[3], channel_range[0], channel_range[1]
                            , channel_range[6], channel_range[7], channel_range[4], channel_range[5]);
                    }
                    else
                    {
                        strTitle = "確認對話視窗";
                        StrPrt = string.Format("定義腳位範圍:\n"
                            + "RX0 Pin 範圍 = RX{0} ~ RX{1}\n"
                            + "TX0 Pin 範圍 = TX{2} ~ TX{3}\n"
                            + "RX1 Pin 範圍 = RX{4} ~ RX{5}\n"
                            + "TX1 Pin 範圍 = TX{6} ~ TX{7}\n"
                            + "是否要覆寫上述設定到[CDC]頁籤，並寫入參數到FW?"
                            , channel_range[2], channel_range[3], channel_range[0], channel_range[1]
                            , channel_range[6], channel_range[7], channel_range[4], channel_range[5]);
                    }
                }
                else
                {
                    if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                    {
                        strTitle = "Confirm Message";
                        StrPrt = string.Format("Define the pin range:\r\n"
                            + "RX Pin Range = RX{0} ~ RX{1}\r\n"
                            + "TX Pin Range = TX{2} ~ TX{3}\r\n"
                            + "Overwrite above settings into table [CDC] and write parameters to FW?"
                            , channel_range[2], channel_range[3], channel_range[0], channel_range[1]);
                    }
                    else
                    {
                        strTitle = "確認對話視窗";
                        StrPrt = string.Format("定義腳位範圍:\n"
                           + "RX Pin 範圍 = RX{0} ~ RX{1}\n"
                           + "TX Pin 範圍 = TX{2} ~ TX{3}\n"
                           + "是否要覆寫上述設定到[CDC]頁籤，並寫入參數到FW?"
                           , channel_range[2], channel_range[3], channel_range[0], channel_range[1]);
                    }
                }

                if(MyUIPage.ShowAskDialog(strTitle, StrPrt, UIStyle.Orange, true))
                {
                    bfUpdateChannelVal(channel_range.Select(x => (int)x).ToArray());
                    bCommitPara = true;
                }
                else
                {
                    strErrorMSG += "User Canceled.";
                    bCommitPara = false;
                }

            } while(false);

            //if(!ScanChResult)
            //    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.AutoScanFormHandle, enWM_MSG.WM_AutoScanCH_Message, (IntPtr)enDBG_MSG_.INFO_MSG_APPEND
            //        , (StaticVar.g_Lang == enLangTable.LANG_ENGLISH) ? string.Format("[ERROR] Failed to scan channel.\n{0}", strErrorMSG) : string.Format("[ERROR] 掃描失敗.\n{0}", strErrorMSG));
            //else
            //    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.AutoScanFormHandle, enWM_MSG.WM_AutoScanCH_Message, (IntPtr)enDBG_MSG_.INFO_MSG_APPEND
            //        , (StaticVar.g_Lang == enLangTable.LANG_ENGLISH) ? string.Format("[DONE] Scanning channel successfully\n") : string.Format("[DONE] 掃描成功\n"));
            if(!ScanChResult)
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.AutoScanFormHandle, enWM_MSG.WM_AutoScanCH_Message, (IntPtr)enDBG_MSG_.INFO_MSG_APPEND
                    ,string.Format("[ERROR] Failed to scan channel.\n{0}", strErrorMSG));
            else
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.AutoScanFormHandle, enWM_MSG.WM_AutoScanCH_Message, (IntPtr)enDBG_MSG_.INFO_MSG_APPEND
                    , string.Format("[DONE] Scanning channel successfully\n"));
            btnScanChStart.Enabled = true;
            Bt_Exit.Enabled = true;
            string str_1 = "", str2 = "";
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.Get_IC_Information(UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode
                       , false, ref str_1, ref str2);
        }

        private void AutoScanCH_Load(object sender, EventArgs e)
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType = enCDCDataFormat.inOriginal;

            _TuningTool_ = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._TuningTool_;

            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
        }
    }
}