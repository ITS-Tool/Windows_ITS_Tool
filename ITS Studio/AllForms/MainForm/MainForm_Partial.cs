using CommonExt;
using Ilitek_HID;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ThirdPartyTools;
using FuncMethodCmd;
using Sunny.UI;

namespace ITS_Studio.AllForms
{
    public partial class ILITek_ITS_Tool
    {
        private Color[] BtnColor = new Color[2] { Color.Silver, Color.Beige };

        private enum SHOW : int { ENABLE = 1, DISABLE = 0 };

        protected DynamicTool MyTool = new DynamicTool();

        public void Reset_UI()
        {
            MainButton_Enable(StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus);
            m_tbgTPTool.Enabled = true;
            if (StaticVar.iNowDeviceCnts == 0)
            {
                InitDeviceButtons(StaticVar.iMaxSupportDev);
                DeviceButtons.Text = "";
            }
            else if (StaticVar.iNowDeviceCnts == 1)
            {
                DeviceButtons.Text = StaticVar.lsMain_USBDevInfo[m_iSelectedDev].Port_Info.Port_Chain;
            }

            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
            {
                if (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus != DevStatus.Connected)
                    return;

                fm_ExtendFlowLayout.m_btCModel.Visible = false;
                fm_ExtendFlowLayout.m_btChargCurve.Visible = false;
            }
            else
            {
                fm_ExtendFlowLayout.m_btCModel.Visible = isCModelVisible;
                fm_ExtendFlowLayout.m_btChargCurve.Visible = isChargeCurveVisible;
            }
        }

        public void MainButton_Disable()
        {
            m_tbgTPTool.Enabled = false;
            btn_Connect.Enabled = false;
            foreach (var item in EnumTool.EnumToList<Tool_BTNs>())
            {
                if(!UI_GBV.ToolMain.btnMainButtons.ContainsKey(item))
                    continue;
                UI_GBV.ToolMain.btnMainButtons[item].Enabled = false;
                UI_GBV.ToolMain.btnMainButtons[item].BackColor = BtnColor[(int)SHOW.DISABLE];
            }
        }

        public void Set_UI_BottonsMode(FW_MODE _ModeSelect)
        {
            foreach (var item in EnumTool.EnumToList<Tool_BTNs>())
            {
                if(!UI_GBV.ToolMain.btnMainButtons.ContainsKey(item))
                    continue;
                switch (item)
                {
                    case Tool_BTNs.BTN_PAINTING:
                    case Tool_BTNs.BTN_CDC:
                    case Tool_BTNs.BTN_TUNING:
                    case Tool_BTNs.BTN_CMODEL:
                    case Tool_BTNs.BTN_FREQUENCYSPECTRUM:
                        if (_ModeSelect == FW_MODE.BL_MODE)
                        {
                            UI_GBV.ToolMain.btnMainButtons[item].Enabled = false;
                            UI_GBV.ToolMain.btnMainButtons[item].BackColor = BtnColor[(int)SHOW.DISABLE];
                        }
                        else if (_ModeSelect == FW_MODE.AP_MODE)
                        {
                            UI_GBV.ToolMain.btnMainButtons[item].Enabled = true;
                            UI_GBV.ToolMain.btnMainButtons[item].BackColor = BtnColor[(int)SHOW.ENABLE];
                        }
                        break;

                    default:
                        UI_GBV.ToolMain.btnMainButtons[item].Enabled = true;
                        UI_GBV.ToolMain.btnMainButtons[item].BackColor = BtnColor[(int)SHOW.ENABLE];
                        break;
                }
            }
        }
    }

    public partial class ILITek_ITS_Tool
    {
        private void OnConsoleAppendLog(ref Message msg)
        {
            fm_ConsoleTool.m_rtbUSBData_TextAdd(Marshal.PtrToStringAuto(msg.WParam));
        }

        private void OnConsoleReceiverData(ref Message msg)
        {
            fm_ConsoleTool.Console_Read();
        }

        private void OnDebugMessage(ref Message msg)
        {
            int iParaNum = (int)msg.WParam;
            string strPara = Marshal.PtrToStringAuto(msg.LParam);
            foreach (var item in EnumTool.EnumToList<enDBG_MSG_>())
            {
                int iShift = int.Parse(item.Desc().Replace("bit_", ""));
                if (MyTool.GetIntegerSomeBit(iParaNum, iShift) != 1)
                    continue;

                switch (item)
                {
                    case enDBG_MSG_.ERROR_MSG_APPEND:
                        richTextBox_ErrMSG.AppendText(strPara);
                        break;

                    case enDBG_MSG_.ERROR_MSG_COVER:
                        richTextBox_ErrMSG.Text = strPara;
                        break;

                    case enDBG_MSG_.INFO_MSG_APPEND:
                        richTB_ConsoleMesg.AppendText(strPara);
                        break;

                    case enDBG_MSG_.INFO_MSG_COVER:
                        richTB_ConsoleMesg.Text = strPara;
                        break;

                    default:
                        richTB_ConsoleMesg.Clear();
                        richTextBox_ErrMSG.Clear();
                        break;
                }

            }
        }

        private void OnCDC_FrameInfo(ref Message msg)
        {
            enTextBox _Value = (enTextBox)msg.WParam.ToInt32();
            if(_Value.HasFlag(enTextBox.textBox_Max))
                textBox_Max.Text = Marshal.PtrToStringAuto(msg.LParam);
            if(_Value.HasFlag(enTextBox.textBox_Min))
                textBox_Min.Text = Marshal.PtrToStringAuto(msg.LParam);
            if(_Value.HasFlag(enTextBox.textBox_Delta))
                textBox_Delta.Text = Marshal.PtrToStringAuto(msg.LParam);
            if(_Value.HasFlag(enTextBox.TextBox_Info))
                richTextBox_Info.Text = Marshal.PtrToStringAuto(msg.LParam);
            if(_Value.HasFlag(enTextBox.textBox_CurrentMax))
                textBox_CurrentMax.Text = Marshal.PtrToStringAuto(msg.LParam);
            if(_Value.HasFlag(enTextBox.textBox_CurrentMin))
                textBox_CurrentMin.Text = Marshal.PtrToStringAuto(msg.LParam);
            if(_Value.HasFlag(enTextBox.textBox_CurrentDelta))
                textBox_CurrentDelta.Text = Marshal.PtrToStringAuto(msg.LParam);
            if(_Value.HasFlag(enTextBox.textBox_Win2Cnt))
                textBox_Win2Cnt.Text = Marshal.PtrToStringAuto(msg.LParam);
        }

        private void OnCDC_Replay_FrameInfo(ref Message msg)
        {
            enCDCDataFormat iSelectedFormat = (enCDCDataFormat)((int)msg.LParam);
            int para = (int)(msg.WParam);
            int max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData.Cast<int>().Max();
            int min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData.Cast<int>().Min();

            int iWin2Cnt = 0;

            switch (iSelectedFormat)
            {
                case enCDCDataFormat.inOriginal:
                    max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData.Cast<int>().Max();
                    min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData.Cast<int>().Min();
                    break;
                case enCDCDataFormat.inMax:
                    max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Max.Cast<int>().Max();
                    min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Max.Cast<int>().Min();
                    break;
                case enCDCDataFormat.inMin:
                    max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Min.Cast<int>().Max();
                    min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Min.Cast<int>().Min();
                    break;
                case enCDCDataFormat.iDelta:
                    max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Delta.Cast<int>().Max();
                    min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Delta.Cast<int>().Min();
                    break;
                case enCDCDataFormat.iSignal:
                    max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Signal.Cast<int>().Max();
                    min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Signal.Cast<int>().Min();
                    break;
                case enCDCDataFormat.iWindows2:
                    max = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Win2.Cast<int>().Max();
                    min = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Win2.Cast<int>().Min();
                    foreach (int item in UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData_Win2)
                        if (item > UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.iWin2Threshold)
                            iWin2Cnt++;
                    break;
                default:
                    break;
            }
            int delta = max - min;

            //UI_GBV.ToolMain.lbFrame_Number.Text = para.ToString();
            //UI_GBV.ToolMain.m_TrackBar_CDC_Play.Value = para + 1;
            //trackBar_CDCPlay.Value = para;

            lb_Mutual_Max.Text = max.ToString();
            lb_Mutual_Min.Text = min.ToString();
            lb_Mutual_Delta.Text = delta.ToString();

            text_ReplayWin2Cnt.Text = iWin2Cnt.ToString();
        }

        private void OnSetCDCPlayTab(ref Message msg)
        {
            tab_Message.SelectedTab = tabPage_CDCPlay;
        }
        private Dictionary<int, Tuple<int, string>> killedDEV = new Dictionary<int, Tuple<int, string>>();
        private void OnDevicechange(ref Message msg)
        {
            //TODO: [Joe] 避免使用Wifi後, 使用USB會進 AllDeviceOffline, 使連線斷掉
            if (Wifi_Var.useWifi)
                return;

            Declare_Variables.DeviceEvent tmpWParam = (Declare_Variables.DeviceEvent)msg.WParam;
            bool bFilter = (tmpWParam == Declare_Variables.DeviceEvent.DBT_DEVICE_REMOVE_COMPLETE);
            bFilter |= (tmpWParam == Declare_Variables.DeviceEvent.DBT_DEVICE_ARRIVAL);
            if (!bFilter)
                return;
            //Static_HID.muEnumDevice.WaitOne();
            bool bIsMultiSensorTest = (m_tbMain.SelectedTab == m_tbgSensorTest) && (StaticVar.NowExecuteTool == enToolName.MultiSensorTestTool);
            //如果這一個500ms時間拿掉，會導致UI在裝置移除的時候沒辦法同步狀態。
            //StaticVar.iNowDeviceCnts = Static_HID.EnumDevice(500);
            StaticVar.iNowDeviceCnts = Static_HID.EnumDevice(0);
            var All_HID_Over_I2C_Dev = StaticVar.lsMain_USBDevInfo.ToList().FindAll(x => x.bIsHID_Over_I2C == true);
            if (All_HID_Over_I2C_Dev.Count != 0)
                return;
            switch (tmpWParam)
            {
                case Declare_Variables.DeviceEvent.DBT_DEVICE_REMOVE_COMPLETE:

                    if (Static_HID.SyncMaiDevInfoBuff(DevSyncStatus.RemoveDev) == false)
                        return;

                    if (StaticVar.NowExecuteTool == enToolName.SensorTestTool)
                        if (StaticVar.iNowDeviceCnts != 0)
                            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bRunningStatus == true)
                                return;

                    if ((StaticVar._ITSTool_Var == enMode.RDMode) || (StaticVar._ITSTool_Var == enMode.OPMode))
                        Reset_UI();

                    bool isNotInListDevice = false;
                    int idx = -1;

                    if (StaticVar.bridgeSetting.m_bridge_var.bCurrentDetectEn)
                    {
                        b_showCurrent = false;
                        TimerClock.Stop();
                    }

                    foreach (var item in StaticVar.lsMain_USBDevInfo)
                    {
                        idx++;
                        if (item.bIsHID_Over_I2C)
                            return;

                        if (string.IsNullOrEmpty(item.Port_Info.Port_Chain))
                            continue;

                        isNotInListDevice = true;
                        switch (item.NowDevStatus)
                        {
                            case DevStatus.DeviceRemove:
                            case DevStatus.Disconnect:
                                if (item.UI_Idx == -1)
                                    continue;
                                var Device = (ToolStripMenuItem)ToolStripItem_Dev[item.UI_Idx];
                                CheckState _CheckState = (Device.Checked) ? CheckState.Checked : CheckState.Unchecked;
                                bool bEnable = item.NowDevStatus == DevStatus.DeviceRemove ? false : true;
                                ChangeDeviceButtons(item, _CheckState, bEnable);
                               
                                if ((item.UI_Idx == this.SelectedDev) && (StaticVar._ITSTool_Var == enMode.RDMode))
                                {
                                    /* device is removed, skipped all retry flow */
                                    CommonFlow.setting[m_iSelectedDev].no_retry = true;
                                    CommonFlow.ilitek_dev_setting(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.setting[m_iSelectedDev]);
                                    if ((StaticVar._ITSTool_Var == enMode.RDMode))
                                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_Home);

                                    UI_GBV.mDev[item.UI_Idx].LocalVariable.Var_Multi.bForceStopCDC = true;
                                    fm_FWUpgrade.ForceClearSelectItem();
                                }
                                break;

                            default:
                                break;
                        }
                        if (bIsMultiSensorTest && (item.NowDevStatus == DevStatus.DeviceRemove))
                        {
                            if (!string.IsNullOrEmpty(item.Port_Info.Port_Chain))
                            {
                                fm_SensorTest_S.TestDeviceNode(idx, item.UI_Idx, DevOption.RemoveDevice);
                                if (!killedDEV.ContainsKey(idx))
                                    killedDEV.Add(idx, new Tuple<int, string>(item.UI_Idx, item.Port_Info.Port_Chain));
                            }
                        }
                    }

                    if (!isNotInListDevice)
                    {
                        /* device is removed, skipped all retry flow */
                        CommonFlow.setting[m_iSelectedDev].no_retry = true;
                        CommonFlow.ilitek_dev_setting(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.setting[m_iSelectedDev]);
                        if ((StaticVar._ITSTool_Var == enMode.RDMode))
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)m_iSelectedDev, (IntPtr)UI_GBV.enMain_tabPage.tabPage_Home);
                    }
                    break;

                case Declare_Variables.DeviceEvent.DBT_DEVICE_ARRIVAL:
                    if (Static_HID.SyncMaiDevInfoBuff(DevSyncStatus.AddDev) == false)
                        return;
                    JoinDevice();
                    idx = -1;
                    foreach (var item in StaticVar.lsMain_USBDevInfo)
                    {
                        idx++;
                        if (item.Dev_Item_Tag == null)
                            continue;
                        if (StaticVar.iNowDeviceCnts == 1)
                        {
                            ChangeDeviceButtons(item, CheckState.Checked, true);
                            //當裝置只有一個的時候指定選Index 0
                            Set_SelectedDev(0);
                            DeviceButtons.Text = ToolStripItem_Dev[0].Text;
                        }
                        else
                        {
                            if (item.UI_Idx == -1)
                                continue;
                            if (item.NowDevStatus == DevStatus.Connected)
                                continue;

                            var Device = (ToolStripMenuItem)ToolStripItem_Dev[item.UI_Idx];
                            CheckState _CheckState = (Device.Checked) ? CheckState.Checked : CheckState.Unchecked;
                            ChangeDeviceButtons(item, _CheckState, true);
                        }

                        if (bIsMultiSensorTest)
                        {
                            Console.WriteLine("DBT_DEVICE_ARRIVAL : {0}", item.Port_Info.Port_Chain);
                            if (killedDEV.ContainsValue(new Tuple<int, string>(item.UI_Idx, item.Port_Info.Port_Chain)))
                            {
                                fm_SensorTest_S.TestDeviceNode(idx, item.UI_Idx, DevOption.AddDevice);
                                killedDEV.Remove(idx);
                            }
                            else if (killedDEV.Count == 0)
                            {
                                if (item.IsFirstPlug == enFirstPlug.YES)
                                    fm_SensorTest_S.TestDeviceNode(idx, item.UI_Idx, DevOption.AddDevice);
                            }
                        }
                    }

                    break;
            }

        }

        private void JoinDevice()
        {
            if (m_tbMain.SelectedTab == m_tbgTPTool || m_tbMain.SelectedTab == m_tbgInternalUse)
            {
                //這裡要判斷，新增裝置會導致這一個Flag被設為斷線，如果原本這個Flag有在使用的話就會出錯。
                if (btn_Connect.Enabled == false)
                {
                    Reset_ConnectionStatus(m_iSelectedDev, StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus);
                    Reset_UI();
                    btn_Connect.Enabled = true;
                }
            }
            else if (m_tbMain.SelectedTab == m_tbgSensorTest)
            {
                if (StaticVar.NowExecuteTool != enToolName.MultiSensorTestTool)
                {
                    if (StaticVar.NowExecuteTool == enToolName.SensorTestTool)
                        if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bRunningStatus == true)
                            return;
                    Reset_ConnectionStatus(m_iSelectedDev, StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus);
                    Reset_UI();
                    if (StaticVar._ITSTool_Var == enMode.OPMode)
                    {
                        toolStrip_txt_Mode.Text = "OP Mode";
                        toolStrip_txt_Mode.BackColor = Color.Yellow;
                        ToolStripBar_Main.Enabled = false;

                        m_tbMain.Visible = false;
                        splitContainer_MainUI.Panel1Collapsed = true;
                        splitContainer_MainUI.Panel1.Hide();

                    }
                    string _str = "";
                    if (StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                        _str = global::ITS_Studio.Properties.Resources.ProcessingTip_En;
                    else
                        _str = global::ITS_Studio.Properties.Resources.ProcessingTip_Cn;
                    MyUIPage.ShowWaitForm(_str);

                    TimerClock.Stop();
                    StatusLabel_Clock.Visible = false;
                    fm_SensorTest_D.delSet_tabControl_TestBtnEnabled(true);
                    //m_btSensrTest_D_Click(null, null);
                    fm_SensorTest_D.vfStop(DevOption.AddDevice);

                    MyUIPage.HideWaitForm();
                }
            }
        }

        private void OnSet_ProgressBar(ref Message msg)
        {
            var idx = 0;
            string str = Marshal.PtrToStringAuto(msg.LParam);
            if (!string.IsNullOrEmpty(str))
            {
                if (!Regex.IsMatch(str, @":([0-9]+)"))
                    return;
                str = str.Split(':')[1];
                idx = int.Parse(str);
            }

            if (UI_GBV.mDev[idx].LocalVariable.VarUpgrade_Multi.bRunningStatus)
                fm_FWUpgrade.Set_ProgressBar(ref msg);

            if (UI_GBV.mDev[idx].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
                fm_SensorTest_D.Set_ProgressBar(ref msg);

            if (UI_GBV.mDev[idx].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.MultiMode)
                fm_SensorTest_S.Set_ProgressBar(idx, ref msg);
        }

        private void OnSet_ProgressBarMax(ref Message msg)
        {
            var idx = (int)msg.LParam;
            if (UI_GBV.mDev[idx].LocalVariable.VarUpgrade_Multi.bRunningStatus)
                fm_FWUpgrade.Set_ProgressBar_Max(ref msg);

            if (UI_GBV.mDev[idx].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
                fm_SensorTest_D.Set_ProgressBar_Max(ref msg);

            if (UI_GBV.mDev[idx].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.MultiMode)
                fm_SensorTest_S.Set_ProgressBar_Max(ref msg);
        }
        
        private void OnFWUpgrade_SetMainButton(ref Message msg)
        {
            FW_MODE FW_Status = (FW_MODE)msg.WParam;
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    m_tbMain.Enabled = true;
                    Reset_UI();
                    Set_UI_BottonsMode(FW_Status);
                }));
            }
            else
            {
                m_tbMain.Enabled = true;
                //Reset_UI(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.NowDevStatus);
                Reset_UI();
                Set_UI_BottonsMode(FW_Status);
            }
        }

        private void OnShow_PleaseWait(ref Message msg)
        {
            if (((enSTATUS_)msg.WParam) == enSTATUS_.HIDE)
            {
                if (this.InvokeRequired)
                    this.Invoke(new MethodInvoker(delegate() { PleaseWait.Close(); }));
                else
                    PleaseWait.Close();
            }
            else if (((enSTATUS_)msg.WParam) == enSTATUS_.SHOW)
            {
                if (PleaseWait.Visible == true)
                    return;

                if (this.InvokeRequired)
                {
                    if (msg.LParam == IntPtr.Zero)
                    {
                        this.Invoke(new MethodInvoker(delegate()
                        {
                            PleaseWait = new fmPleaseWait();
                            PleaseWait.Show();
                        }));
                    }
                    else
                    {
                        PleaseWait = new fmPleaseWait(msg.LParam);
                        PleaseWait.Show();
                    }
                }
                else
                {
                    PleaseWait = (msg.LParam == IntPtr.Zero) ? new fmPleaseWait() : new fmPleaseWait(msg.LParam);
                    PleaseWait.Show();
                }
            }
        }

        private void OnTableMainEnable(ref Message msg)
        {
            var _Res = (enSwitch)msg.WParam;
            m_tbMain.Enabled = (_Res == enSwitch.Enable);
        }

        private void OnToolClose(ref Message msg)
        {
            this.Close();
        }

        private void OnI2CError(ref Message msg)
        {
            int iDev = (int)msg.LParam;
            if (UI_GBV.mDev[iDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
            {
                fm_SensorTest_D.vfStop(DevOption.I2CErr);
            }
            else if (UI_GBV.mDev[iDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.MultiMode)
            {
                fm_SensorTest_S.TestDeviceNode(iDev, StaticVar.lsMain_USBDevInfo[iDev].UI_Idx, DevOption.I2CErr);
            }
            else
            {
                StaticVar.lsMain_USBDevInfo[iDev].NowDevStatus = DevStatus.Disconnect;
                Reset_ConnectionStatus(iDev, DevStatus.Disconnect);
                Reset_UI();
            }
        }

        private void OnPaint_PenLine(ref Message msg)
        {
            var iSelectedDev = (int)msg.WParam;
            if (StaticVar.bEnterLatencyCalibrationMode)
            {
                return;
            }

            switch (UI_GBV.mDev[iSelectedDev].LocalVariable.Var_Multi.m_Int_InDataType)
            {
                case enInt_InDataType.Painting:
                    fm_PaintTool.DrawingLine_Pen();
                    break;

                case enInt_InDataType.SenTest_Painting:
                    UI_GBV.fm_SensorTest_Painting.DrawingLineTest_Pen();
                    break;
                
                case enInt_InDataType.CModelReplay:
                    fm_C_Model_Replay.DrawingLine_Pen();
                    break;
            }
        }

        private void OnDebugMonitor(ref Message msg)
        {
            bool bUpdateInfo = (int)msg.WParam == 1 ? true : false;
            if (bUpdateInfo)
                fm_PaintTool.OnShowMonitorLog();
        }

        private void OnPaint_FingerLine(ref Message msg)
        {
            int iSelectedDev = (int)msg.WParam;
            if (StaticVar.bEnterLatencyCalibrationMode)
            {
                return;
            }
            
            switch(UI_GBV.mDev[iSelectedDev].LocalVariable.Var_Multi.m_Int_InDataType)
            {
                case enInt_InDataType.Painting:
                    fm_PaintTool.DrawingLine_Finger();
                    break;

                case enInt_InDataType.SenTest_Painting:
                    if(UI_GBV.mDev[iSelectedDev].LocalVariable.VarPaint_Multi.NowPaintTestObj == enPaintTestObj.Finger)
                        UI_GBV.fm_SensorTest_Painting.DrawingLineTest_Finger();
                    break;
                
                case enInt_InDataType.CModelReplay:
                    fm_C_Model_Replay.DrawingLine_Finger();
                    break;
            }
        }

        private void OnPaintMenuClose(ref Message msg)
        {
            fm_PaintTool.MenuClose();
        }

        private void OnPaintFullScreenExit(ref Message msg)
        {
            if (!StaticVar.bPaint_Record)
            {
                if (ExternalPaintForm != null)
                    ExternalPaintForm.BTN_Exit_Click(null, null);
            }

        }

        private void OnPaintFullScreenClear(ref Message msg)
        {
            if (!StaticVar.bPaint_Record && !StaticVar.IsPlayback_Mode)
            {
                ExternalPaintForm.BTN_Clear_Screen_Click(null, null);
            }
        }

        private void OnPaintBackToHome(ref Message msg)
        {
            m_tabControl.SelectedTab = tabPage_Home;
            vbButtonSelect(m_btHome);
        }

        private void OnSenTest_PaintFinish(ref Message msg)
        {
            int iDev = (int)msg.LParam;

            if(UI_GBV.mDev[iDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
                fm_SensorTest_D.Painting_Finish();
            //else if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.MultiMode)
            //    fm_SensorTest_S.vfSetPainting_Finish(ref msg);
        }

        private void OnSenTest_DrawingReset(ref Message msg)
        {
            int iDev = (int)msg.LParam;
            if(UI_GBV.mDev[iDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
                fm_SensorTest_D.Drawing_Reset();
        }

        private void OnShowShortTestPattern(ref Message msg)
        {
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
                fm_SensorTest_D.ShowShortPattern(((enSTATUS_)msg.WParam) == enSTATUS_.SHOW, ((enPatternType)msg.LParam) == enPatternType.Close_Monitor);
        }

        private void OnShowSensorTestWarningBox(ref Message msg)
        {
            if (((enSTATUS_)msg.WParam) == enSTATUS_.HIDE)
            {
                if (this.InvokeRequired)
                    this.Invoke(new MethodInvoker(delegate() { fm_NumberForm.Close(); }));
                else
                    fm_NumberForm.Close();
            }
            else if (((enSTATUS_)msg.WParam) == enSTATUS_.SHOW)
            {
                if (fm_NumberForm.Visible == true)
                    return;

                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        fm_NumberForm = new NumberForm();
                        fm_NumberForm.Show();
                    }));
                }
                else
                {
                    fm_NumberForm = new NumberForm();
                    fm_NumberForm.Show();
                }
            }
        }

        private void OnSenTest_TestMode(ref Message msg)
        {
            string[] strMsg = Marshal.PtrToStringAuto(msg.LParam).Split(',');
            int iDev = 0;
            int.TryParse(strMsg[0], out iDev);

            var _Mode = (TouchMode_UI)Enum.Parse(typeof(TouchMode_UI), strMsg[1]);

            // Disable m_tspMain
            if ((int)msg.WParam == (int)TouchMode.TestMode) // Test Mode = True
            {
                //StaticVar.CDC_Var.UI_CHART_MODE = StaticVar.CDC_Var._CDC_CHART_MODE_.CDC_ONLY;
                if (_Mode == TouchMode_UI.CMD_Only)
                {
                    ModeSelect[TouchMode.TestMode](UI_GBV.mDev[iDev].LocalVariable.Var_Multi.ProtocolType);
                }
                else if (_Mode == TouchMode_UI.UI_Only)
                {
                    ToolStripBar_Main.Enabled = false;
                    ITS_MainTool_splitContainer.Panel1.Enabled = false;
                }
                else
                {
                    ModeSelect[TouchMode.TestMode](UI_GBV.mDev[iDev].LocalVariable.Var_Multi.ProtocolType);
                    ToolStripBar_Main.Enabled = false;
                    ITS_MainTool_splitContainer.Panel1.Enabled = false;
                }
            }
            else if ((int)msg.WParam == (int)TouchMode.NormalMode)
            {
                if (_Mode == TouchMode_UI.CMD_Only)
                {
                    ModeSelect[TouchMode.NormalMode](UI_GBV.mDev[iDev].LocalVariable.Var_Multi.ProtocolType);
                }
                else if (_Mode == TouchMode_UI.UI_Only)
                {
                    ToolStripBar_Main.Enabled = true;
                    ITS_MainTool_splitContainer.Panel1.Enabled = true;
                }
                else
                {
                    ModeSelect[TouchMode.NormalMode](UI_GBV.mDev[iDev].LocalVariable.Var_Multi.ProtocolType);
                    ToolStripBar_Main.Enabled = true;
                    ITS_MainTool_splitContainer.Panel1.Enabled = true;
                }
            }
        }

        //20220629 Steven Add
        private void OnSenTest_UpdateTestInfo(ref Message msg)
        {
            int iSelectedDev = (int)(msg.WParam);

            if (UI_GBV.mDev[iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
            {
                fm_SensorTest_D.UpdateTestInfo();
            }
            //else if (UI_GBV.mDev[iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.MultiMode)
            //{
            //    fm_SensorTest_S.TestItem_All_Finish(iSelectedDev);
            //}
        }

        private void OnSenTest_All_Finish(ref Message msg)
        {
            int iSelectedDev = (int)(msg.WParam);
            string _Result = Marshal.PtrToStringAuto(msg.LParam);

            //if (UI_GBV.mDev[iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
            if (StaticVar.NowExecuteTool == enToolName.SensorTestTool)
                fm_SensorTest_D.Test_All_Finish(_Result);
            //else if (UI_GBV.mDev[iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.MultiMode)
            else if (StaticVar.NowExecuteTool == enToolName.MultiSensorTestTool)
                fm_SensorTest_S.TestItem_All_Finish(iSelectedDev);
        }

        private void OnSenTestUIUpdata(ref Message msg)
        {
            var iDevIndex = (int)msg.WParam;
            var StrArr = Marshal.PtrToStringAuto(msg.LParam).Split(',');
            var iTestItem = 0;
            int.TryParse(StrArr[0], out iTestItem);

            if (UI_GBV.mDev[iDevIndex].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.SingleMode)
            {
                fm_SensorTest_D.UpdateMainTestGrid(iDevIndex, iTestItem);
            }

            if (UI_GBV.mDev[iDevIndex].LocalVariable.VarSenTest_Multi.SensorTestMode == enSensorTest.MultiMode)
            {
                var Result = MultiSenTest_VAR.TestResult.NA;
                Enum.TryParse<MultiSenTest_VAR.TestResult>(StrArr[1], out Result);
                fm_SensorTest_S.UpdateMainTestGrid(iDevIndex, iTestItem + 3, Result);//因為最前面兩個不是測項
            }
        }

        private void OnSet_UI_BottonsMode(ref Message msg)
        {
            Set_UI_BottonsMode((FW_MODE)msg.LParam);
        }

        private void On_Change_UI_Status(ref Message msg)
        {
            UI_Component_Set MyVar = (UI_Component_Set)msg.WParam;
            if ((int)MyVar == (int)UI_Component_Set.splitContainer_Panel1)
            {
                ITS_MainTool_splitContainer.Panel1.Enabled = true;
            }
        }

        private void OnTuningChangeControl(ref Message msg)
        {
            TuningPage MyVar = (TuningPage)msg.WParam;
            fm_AutoScanCH_Tool.Visible = false;
            fm_NoiseFre.Visible = false;
            fm_TuningCompare.Visible = false;
            fm_TuningTool.Visible = false;
            fm_ChargeCurve.Visible = false;
            switch (MyVar)
            {
                case TuningPage.AutoScanCHPage:
                    fm_AutoScanCH_Tool.Visible = true;
                    fm_AutoScanCH_Tool.Show();
                    break;

                case TuningPage.NoiseFrePage:
                    fm_NoiseFre.NoiseFrefmEntry();
                    fm_NoiseFre.Visible = true;
                    fm_NoiseFre.Show();
                    break;

                case TuningPage.TuningCompare:
                    fm_TuningCompare.Visible = true;
                    fm_TuningCompare.Show();
                    break;

                case TuningPage.WriteParaPage:
                    fm_TuningTool.Visible = true;
                    fm_TuningTool.Show();
                    break;

                case TuningPage.ChargeCurve:
                    if (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].NowDevStatus == DevStatus.Connected)
                        fm_ChargeCurve.ResetChart(false);
                    fm_ChargeCurve.Visible = true;
                    fm_ChargeCurve.Show();
                    break;

                case TuningPage.GenProfile:
                    fm_GenProfile.Visible = true;
                    fm_GenProfile.Action(false);
                    fm_GenProfile.Show();
                    break;
                default:
                    break;
            }
        }

        private void OnWarningFormShow(ref Message msg)
        {
            string strTip = Marshal.PtrToStringAuto(msg.WParam);
            string strMsg = Marshal.PtrToStringAuto(msg.LParam);
            if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.SensorTestMode != enSensorTest.MultiMode)
                fm_SensorTest_D.WarningFormShow(strTip, strMsg);
        }

        private void OntabControlSwitch(ref Message msg)
        {
            var iDev = (int)msg.WParam;            
            var item = (UI_GBV.enMain_tabPage)msg.LParam;

            if (StaticVar.bridgeSetting.m_bridge_var.bCurrentDetectEn)
            {
                b_showCurrent = false;
                TimerClock.Stop();
            }
           
            switch(item)
            {
                default:
                case UI_GBV.enMain_tabPage.tabPage_Home:
                    m_tabControl.SelectedTab = tabPage_Home;
                    tab_Message.SelectedTab = tabPage_ErrorMSG;                  
                    break;

                case UI_GBV.enMain_tabPage.tabPage_CDCTool:
                    m_tabControl.SelectedTab = tabPage_CDCTool;
                    fm_CDCTool.AssignReplayBar();

                    if(StaticVar.lsMain_USBDevInfo[iDev].NowDevStatus != DevStatus.Connected)
                        tab_Message.SelectedTab = tabPage_CDCPlay;
                    else
                        tab_Message.SelectedTab = tabPage_CDCInfo;
                    break;

                case UI_GBV.enMain_tabPage.tabPage_Console:
                    m_tabControl.SelectedTab = tabPage_Console;
                    tab_Message.SelectedTab = tabPage_ErrorMSG;
                    break;

                case UI_GBV.enMain_tabPage.tabPage_FWUpgrade:
                    m_tabControl.SelectedTab = tabPage_FWUpgrade;
                    tab_Message.SelectedTab = tabPage_ErrorMSG;
                    break;

                case UI_GBV.enMain_tabPage.tabPage_PaintTool:
                    m_tabControl.SelectedTab = tabPage_PaintTool;
                    fm_PaintTool.AssignReplayBar();
                    tab_Message.SelectedTab = tabPage_ErrorMSG;
                    break;

                case UI_GBV.enMain_tabPage.tabPage_SensorTest_S:
                    m_tabControl.SelectedTab = tabPage_SensorTest_S;
                    tab_Message.SelectedTab = tabPage_ErrorMSG;
                    break;

                case UI_GBV.enMain_tabPage.tabPage_Tuning:
                    m_tabControl.SelectedTab = tabPage_Tuning;
                    tab_Message.SelectedTab = tabPage_ErrorMSG;
                    break;

                case UI_GBV.enMain_tabPage.tabPage_SensorTest_D:
                    m_tabControl.SelectedTab = tabPage_SensorTest_D;
                    tab_Message.SelectedTab = tabPage_ErrorMSG;
                    break;

                case UI_GBV.enMain_tabPage.tabPage_CModel:
                    m_tabControl.SelectedTab = tabPage_CModel;
                    tab_Message.SelectedTab = tabPage_ErrorMSG;
                    break;
                
                case UI_GBV.enMain_tabPage.tabPage_CModel_Replay:
                    m_tabControl.SelectedTab = tabPage_CModel_Replay;
                    tab_Message.SelectedTab = tabPage_ErrorMSG;
                    break;
            }
            if(item != UI_GBV.enMain_tabPage.tabPage_Console)
                if(StaticVar.lsMain_USBDevInfo[iDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                    if(StaticVar.lsMain_USBDevInfo[iDev].NowDevStatus == DevStatus.Connected)
                        UI_GBV.mDev[iDev].m_GBV.Globe_Multi.m_Method_CMD.vfSetSPITestModeEnable(false);
        }

        private void OnSensorTest_FPC_Open(ref Message msg)
        {
            var _ActItem = (enSenTest_FPC_OpenTest)msg.WParam;
            switch (_ActItem)
            {
                case enSenTest_FPC_OpenTest._Set_AllInitValue:
                    var Employee = (_FPC_OpenTest)Marshal.PtrToStructure(msg.LParam, typeof(_FPC_OpenTest));
                    fm_SensorTest_FPC_Open = new SensorTest_FPC_Open();
                    fm_SensorTest_FPC_Open.Set_Chart_Axes(Employee.iAA_RawData, Employee.strTitle);
                    fm_SensorTest_FPC_Open.Set_Timer_Max(Employee.Set_Timer_Max);
                    fm_SensorTest_FPC_Open.Set_Bar_Max(Employee.Set_Bar_Max);
                    break;

                case enSenTest_FPC_OpenTest._Set_BarValue:
                    Employee = (_FPC_OpenTest)Marshal.PtrToStructure(msg.LParam, typeof(_FPC_OpenTest));
                    fm_SensorTest_FPC_Open.Set_Bar_Value(Employee.BarValues);
                    Application.DoEvents();
                    break;

                case enSenTest_FPC_OpenTest._Set_TimerText:
                    fm_SensorTest_FPC_Open.Set_Timer_Text((int)msg.LParam);
                    break;

                case enSenTest_FPC_OpenTest._Show:
                    fm_SensorTest_FPC_Open.TopMost = true;
                    fm_SensorTest_FPC_Open.Owner = StaticVar.MainFormParent;
                    fm_SensorTest_FPC_Open.Show();
                    break;

                case enSenTest_FPC_OpenTest._Stop:
                    fm_SensorTest_FPC_Open.Close();
                    break;
            }
        }

        private void OnCloseExtendUI(ref Message msg)
        {
            var _ActItem = (enWM_MSG)msg.WParam;

            if (_ActItem == enWM_MSG.WM_RBUTTONDOWN)
            {
                fm_ExtendFlowLayout.Visible = false;
            }

        }

        private void OnCommonFlowMsg(ref Message msg)
        {
            if (m_tabControl.SelectedTab == tabPage_FWUpgrade)
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.FirmwareUpgradeFormHandle,
                        enWM_MSG.WM_Upgrade_Info, msg.LParam, null);
            }

            if (CommonFlow.msg_str != null && (int)msg.WParam < (int)CommonFlow.ilitek_log_level.log_level_tag)
            {
                CommonFlow.msg_str += Marshal.PtrToStringAuto(msg.LParam);
                return;
            }

            if (m_tabControl.SelectedTab == tabPage_Home ||
                m_tabControl.SelectedTab == tabPage_SensorTest_D)
            {
                /* show mp result */
                CommonFlow.msg_str += Marshal.PtrToStringAuto(msg.LParam);
            }
        }

        private void OnShowErrorDialog(ref Message msg)
        {
            UIPage MyUIPage = new UIPage();
            string strTip = Marshal.PtrToStringAuto(msg.WParam);
            string strMsg = Marshal.PtrToStringAuto(msg.LParam);
            MyUIPage.ShowErrorDialog(strTip, strMsg, UIStyle.Red, true);
        }

        private void OnShowTips(ref Message msg)
        {
            UIPage MyUIPage = new UIPage();
            string strTip = Marshal.PtrToStringAuto(msg.WParam);
            int delay = (int)msg.LParam;
            MyUIPage.ShowErrorTip(strTip, delay, true);
        }

        private void OnShowErrorDialog_Mask(ref Message msg)
        {
            UIPage MyUIPage = new UIPage();
            string strTip = Marshal.PtrToStringAuto(msg.WParam);
            MyUIPage.ShowErrorDialog(strTip, false);
        }
        
        private void OnClickCModelBtn(ref Message msg)
        {
            m_btCModel_Click();
        }

        private void OnCheckLCMFixtureGPIO(ref Message msg)
        {
            if (msg.WParam == (IntPtr)0)
            {
                if (StaticVar.iCheckFuxtureGPIOCountNumber > 0)
                {
                    FixtureHint _fixturefm = new FixtureHint(StaticVar.iCheckFuxtureGPIOCountNumber, false);
                    _fixturefm.Show();

                    while (_fixturefm.Visible)
                    {
                        MyTool.Delay_ms(10);

                        if (!UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bUSBMonitor_Opened)
                            return;

                        var _Select = StaticVar.gpio_check_list.First(x => x.Value.Item1 == enBridgeGpioUsage.START_PIN);

                        if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.GetBridge_Pin_Status_For_LCM_Fixture_Detect(_Select.Key, _Select.Value.Item2))
                        {
                            StaticVar.bIsFixtureOn = true;
                            _fixturefm.Close();
                            break;
                        }


                        Application.DoEvents();
                    }
                }

            }
            else
            {
                if (StaticVar.iNoisePatternCountDownNumber > 0)
                {
                    FixtureHint _fixturefm = new FixtureHint(StaticVar.iNoisePatternCountDownNumber, true);
                    _fixturefm.Show();

                    while (_fixturefm.Visible)
                    {
                        MyTool.Delay_ms(10);

                        if (!UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bUSBMonitor_Opened)
                            return;

                        var _Select = StaticVar.gpio_check_list.First(x => x.Value.Item1 == enBridgeGpioUsage.Noise_Pattern_PIN);

                        GPIO_PIN_TYPE TargetPin = (_Select.Value.Item2 == GPIO_PIN_TYPE.HIGH) ? GPIO_PIN_TYPE.LOW : GPIO_PIN_TYPE.HIGH;

                        if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Method_CMD.GetBridge_Pin_Status_For_LCM_Fixture_Detect(_Select.Key, TargetPin))
                        {
                            StaticVar.bIsPatternAlreadyChange = true;
                            _fixturefm.Close();
                            break;
                        }


                        Application.DoEvents();
                    }
                }

            }

        }
    }
}