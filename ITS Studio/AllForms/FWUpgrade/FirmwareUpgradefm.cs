using CommonExt;
using FuncMethodCmd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ThirdPartyTools;
using System.Linq;
using System.ComponentModel;
using Sunny.UI;
using System.Text.RegularExpressions;

namespace ITS_Studio.AllForms
{
    public partial class FirmwareUpgradefm : Form
    {
        private int m_iSelectedDev = 0;
        public int SelectedDev { set { m_iSelectedDev = value; } get { return m_iSelectedDev; } }
        private int m_iFailCnts = 0;
        public UIComboBox m_comboBox_FilePath;
        public string[] strFilePathArray;
        private UIPage MyUIPage = new UIPage();
        enum ProgramFunc
        {
            [Description("Force Upgrade")]
            Force_Upgrade = 0,

            [Description("FW Verify Only")]
            FW_CheckOnly = 1,

            [Description("I2C HW Reset")]
            I2C_HW_Reset = 2,

            [Description("I2C ByPass INT")]
            I2C_ByPassINT = 3,
        }

        private const int iMaxPathItem = 4;
        private INI_Tool MY_INI = new INI_Tool(ITS_Directory.History_INI);
        private INI_Tool MY_SettingsINI = new INI_Tool(ITS_Directory.Setting_INI);
        protected DynamicTool MyTool = new DynamicTool();

        public bool StartProgramEnable
        {
            set { btnStartProgram.Enabled = value; }
        }

        private bool use_commonflow = true;

        public FirmwareUpgradefm()
        {
            InitializeComponent();
            
            lb_StressBuild.Visible = Program.bSTRESS_BUILD;

            SubscribeMessageReceiver();
            StaticVar.FirmwareUpgradeFormHandle = this.Handle;
            StaticVar.Program_WaitUIPanelHandle = this.Panel_WaitUI.Handle;

            m_comboBox_FilePath = this.cbBox_FilePath;
            strFilePathArray = new string[iMaxPathItem];

            if (Directory.Exists("NoFwUpgradeCommonFlow"))
                use_commonflow = false;
        }

        private delegate void delEditText(string myStr, Control ctl);
        private delegate void delUI_Control(Control ctl, bool bEn);
        private Dictionary<enWM_MSG, function> mMessageReceiver = new Dictionary<enWM_MSG, function>();

        private void SubscribeMessageReceiver()
        {
            mMessageReceiver.Add(enWM_MSG.WM_Program_Status, OnFWUpgradeSendingFileMessage);
            mMessageReceiver.Add(enWM_MSG.WM_Program_Message, OnFWSetMessage);
            mMessageReceiver.Add(enWM_MSG.WM_Upgrade_Info, OnShowMsg);
        }

        protected override void WndProc(ref Message msg)
        {
            enWM_MSG pEnumPanoMsg = (enWM_MSG)msg.Msg;
            if (mMessageReceiver.Keys.Contains(pEnumPanoMsg))
                mMessageReceiver[pEnumPanoMsg](ref msg);
            base.WndProc(ref msg);
        }

        public void ForceClearSelectItem()  //裝置拔除後清空選項
        {
            uiCheckBoxGroup1.UnSelectAll();
            uiCheckBoxGroup1.SelectedIndexes = new List<int>();
        }

        public void UpgradeInit()
        {
            uiCheckBoxGroup1.Items.Clear();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bI2C_ByPassINT = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bForceUpgrade = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bFW_CheckOnly = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bBridge_HW_Reset = false;

            var Des = EnumTool.GetDescriptions<ProgramFunc>();
            foreach (var item in Des)
            {
                if (item.Contains("I2C"))
                    if (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType != DEV_INTERFACE_TYPE.I2C)
                    {
                        continue;
                    }

                uiCheckBoxGroup1.Items.Add(item.ToString());
            }

            var Ori_ = uiCheckBoxGroup1.SelectedItems.Select(x => EnumTool.GetValueFromDescription<ProgramFunc>(x.ToString())).ToList();
            foreach (var item in Ori_)
            {
                switch (item)
                {
                    case ProgramFunc.Force_Upgrade:
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bForceUpgrade = true;
                        break;
                    case ProgramFunc.FW_CheckOnly:
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bFW_CheckOnly = true;
                        break;
                    case ProgramFunc.I2C_ByPassINT:
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bI2C_ByPassINT = true;
                        break;
                    case ProgramFunc.I2C_HW_Reset:
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bBridge_HW_Reset = true;
                        break;
                }
            }

            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Hex files (*.hex)|*.hex";
            openDlg.DefaultExt = "hex";
            openDlg.FileName = "";
            openDlg.CheckFileExists = true;
            openDlg.CheckPathExists = true;
            openDlg.Multiselect = false;

            cbBox_FilePath.Items.Clear();

            vfLoadFilePathItem();

            if (strFilePathArray[0] != "")            
                m_comboBox_FilePath.SelectedIndex = 0;
            
            //reset UI
            vfSetSendingFileUI(enSTATUS_.HIDE, enSTATUS_.SENDING_FILE);
            //labDownloadPCT.Text = "0%";
            progressBar1.Value = 0;
            progressBar1.Style = UIStyle.Orange;
            progressBar1.Refresh();
        }

        private void OnFWUpgradeSendingFileMessage(ref Message msg)
        {
            vfSetSendingFileUI((enSTATUS_)msg.WParam, (enSTATUS_)msg.LParam);

            if (((enSTATUS_)msg.WParam) == enSTATUS_.HIDE)
                vfSetSendingFileUI(enSTATUS_.HIDE, (enSTATUS_)msg.LParam);
            else if (((enSTATUS_)msg.WParam) == enSTATUS_.SHOW)
                vfSetSendingFileUI(enSTATUS_.SHOW, (enSTATUS_)msg.LParam);
        }

        private void vfSetSendingFileUI(enSTATUS_ Visible, enSTATUS_ inMessageType)
        {
            switch (inMessageType)
            {
                case enSTATUS_.SENDING_FILE:
                    this.lblSendingFile.Text = "Programming now.....";
                    break;

                case enSTATUS_.RECONNECTING:
                    this.lblSendingFile.Text = "Reconnecting now.....";
                    break;

                case enSTATUS_.SEARCH_DEV:
                    this.lblSendingFile.Text = "Search Device now.....";
                    break;

                case enSTATUS_.SWITCH_MODE:
                    this.lblSendingFile.Text = "Switch Mode now.....";
                    break;

                case enSTATUS_.FINISH:
                    this.lblSendingFile.Text = "Programming Done.";
                    break;

                case enSTATUS_.PROGRAM_FAIL:
                    this.lblSendingFile.Text = "Programming Failure.";
                    break;

                case enSTATUS_.PROGRAM_SLAVE:
                    this.lblSendingFile.Text = "Programming Slave now.....";
                    break;
            }
            bool bVisible = (Visible == enSTATUS_.SHOW) ? true : false;
            m_picSpinner.Visible = bVisible;
            this.lblSendingFile.Visible = bVisible;
        }

        public void vbStop()
        {
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.bRunningStatus = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_Program_UI_Status, (IntPtr)UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode, (IntPtr)0);
        }

        public void Set_ProgressBar_Max(ref Message message)
        {
            progressBar1.Maximum = (int)message.WParam;
            //labDownloadPCT.Text = "0%";
            progressBar1.Value = 0;
            progressBar1.Refresh();
        }
        enum enStatus
        {
            Testing,
            Stop,
        }
        private void SetProgramButton(enStatus _status)
        {
            this.Invoke(new MethodInvoker(delegate()
                    {
                        if(_status == enStatus.Stop)
                        {
                            btnStartProgram.Enabled = true;
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.update_abort = false;
                            btnStartProgram.Text = "Run";
                            btnStartProgram.Image = global::ITS_Studio.Properties.Resources.Play_Pressed_icon;
                            btnSelectFile.Enabled = true;
                        }
                        else
                        {
                            btnStartProgram.Text = "Abort";
                            btnStartProgram.Image = global::ITS_Studio.Properties.Resources.Stop_Normal_Red_icon;
                            btnSelectFile.Enabled = false;
                        }
                    }));
        }

        private void OnFWSetMessage(ref Message msg)
        {
            OnShowProgramMessage(msg);
        }

        private void OnShowMsg(ref Message msg)
        {
            string str = string.Format("{0}", Marshal.PtrToStringAuto(msg.WParam));
            richTB_Result.Text += str;
            if(Program.bSTRESS_BUILD)
            {
                //if(str.StartsWith("Block"))
                if(str.EndsWith("matched\n"))
                {
                    Match match = Regex.Match(str, @"(matched|not matched)");
                    if(match.Success)
                        if(match.Value == "not matched")
                            m_iFailCnts++;
                }
                else if(str.Contains("[ilitek_update_start] start to program") || str.Contains("[ERR]"))
                {
                    m_iFailCnts = 0;//歸零
                }
            }
        }

        private void OnShowProgramMessage(Message msg)
        {
            bool bAppend = ((enDBG_MSG_)msg.WParam == enDBG_MSG_.ERROR_MSG_APPEND);
            
            if (!bAppend)
                bAppend |= ((enDBG_MSG_)msg.WParam == enDBG_MSG_.INFO_MSG_APPEND);

            if (!bAppend)
                bAppend |= ((int)msg.WParam == 0);

            if (bAppend)
            {
                if (this.InvokeRequired)
                {
                    delEditText MyFunc = new delEditText(AppandText);
                    this.BeginInvoke(MyFunc, Marshal.PtrToStringAuto(msg.LParam), richTB_Result);
                }
                else
                {
                    AppandText(Marshal.PtrToStringAuto(msg.LParam), richTB_Result);
                    progressBar1.Style = ((enDBG_MSG_)msg.WParam == enDBG_MSG_.ERROR_MSG_APPEND) ? UIStyle.Red : UIStyle.Orange;
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    delEditText MyFunc = new delEditText(WriteText);
                    this.BeginInvoke(MyFunc, Marshal.PtrToStringAuto(msg.LParam), richTB_Result);
                }
                else
                {
                    WriteText(Marshal.PtrToStringAuto(msg.LParam), richTB_Result);
                }
            }
        }

        public void Set_ProgressBar(ref Message msg)
        {
            if ((progressBar1.Value + 1) > progressBar1.Maximum)
            {
                progressBar1.Value = progressBar1.Maximum;
            }
            else
            {
                int iNum = ((int)msg.WParam > progressBar1.Maximum) ? progressBar1.Maximum : (int)msg.WParam;
                progressBar1.Value = iNum;
            }
            //labDownloadPCT.Text = string.Format("{0:0%}", (double)progressBar1.Value / progressBar1.Maximum);
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            INI_Tool HistoryINI = new INI_Tool(ITS_Directory.History_INI);
            string strHexPath = HistoryINI.IniReadValue("FWUpgrade_FilePath", "Path", "");
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.InitialDirectory = strHexPath;
            openDlg.Filter = "Hex/Bin files (*.hex,*.bin)|*.hex;*.bin";
            openDlg.DefaultExt = "hex";
            openDlg.FileName = "";
            openDlg.CheckFileExists = true;
            openDlg.CheckPathExists = true;
            openDlg.Multiselect = false;
            if(File.Exists(openDlg.InitialDirectory) == false)
                openDlg.InitialDirectory = ITS_Directory.Tool_CurrentDirectory + "ParaData";

            btnStartProgram.Enabled = false;
            btnSelectFile.Enabled = false;
            string strErrorMSG = string.Empty;

            if(openDlg.ShowDialog(this) != DialogResult.OK)
            {
                btnSelectFile.Enabled = true;
                btnStartProgram.Enabled = (UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.strFilePath == null) ?
                    false : DecodeFile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.strFilePath, ref strErrorMSG);                
                return;
            }
            //string strFilePath = openDlg.FileNames.GetValue(0).ToString();// 將讀取到的檔案路徑塞給"strFilePath"            
            string strFilePath = openDlg.FileName;

            if(DecodeFile(strFilePath, ref strErrorMSG))
            {
                HistoryINI.IniWriteValueW("FWUpgrade_FilePath", "Path", strFilePath);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.strFilePath = strFilePath;
                vfFilePathItemAdd(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.strFilePath);
                btnStartProgram.Enabled = true;
                lb_StressBuild.MySetStyle(STRESS_BUILD_Show.Ready);
            }
            else
            {
                btnStartProgram.Enabled = false;
                richTB_Result.Text = strErrorMSG;
                lb_StressBuild.MySetStyle(STRESS_BUILD_Show.NG);
            }
            btnSelectFile.Enabled = true;
        }

        private void btnStartProgram_Click(object sender, EventArgs e)
        {
            string strErrorMSG = "";
            if(!DecodeFile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.strFilePath, ref strErrorMSG))
                return;
            
            if (btnStartProgram.Text.Equals("Abort"))
            {
                btnStartProgram.Enabled = false;
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.update_abort = true;
                CommonFlow.setting[m_iSelectedDev].no_retry = true;
                CommonFlow.ilitek_dev_setting(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.setting[m_iSelectedDev]);
                return;
            }
            SetProgramButton(enStatus.Testing);
            /// =======================================
            richTB_Result.Text = "";
            progressBar1.Value = 0;
            progressBar1.Refresh();
            progressBar1.Style = UIStyle.Orange;
            /// =======================================
            if (StaticVar.lsMain_USBDevInfo.Length <= 0)
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bRunningStatus = false;
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.ThreadFun = null;
            }
            else
            {
                /* Disable other MainForm UI button */
                UI_GBV.fmITS_Tool.MainButton_Disable();

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bRunningStatus = true;
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.ThreadFun = new Thread(() => Thread_Upgrade());
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.ThreadFun.Start();
            }
        }

        private void cbBox_FilePath_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTB_Result.Text = "";
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.strFilePath = strFilePathArray[m_comboBox_FilePath.SelectedIndex];

            string strErrorMSG = "";
            btnStartProgram.Enabled = DecodeFile(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.strFilePath, ref strErrorMSG);
            richTB_Result.Text = strErrorMSG;
        }

        private void UI_Control(Control ctl, bool En)
        {
            if (this.InvokeRequired)
            {
                delUI_Control myUpdate = new delUI_Control(UI_Control);
                
                this.Invoke(myUpdate, ctl, En);
            }
            else
            {
                ctl.Enabled = En;
                this.Invoke(new Action(() =>
                {
                    UI_GBV.fmITS_Tool.Reset_UI();
                    if(Program.bSTRESS_BUILD)
                    {
                        if(!En)//歸零
                        {
                            lb_StressBuild.MySetStyle(STRESS_BUILD_Show.Ready);
                            m_iFailCnts = 0;
                        }
                    }
                }));
            }
        }

        private void Thread_Upgrade()
        {
            DEV_INTERFACE_TYPE tmpINTERFACE_TYPE = StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType;
            string strStatusMessage = "";
            bool ret = true;
            string strErrMessage = "";
            try
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_UsbDeviceNotification, (IntPtr)m_iSelectedDev, IntPtr.Zero);

                UI_Control(uiCheckBoxGroup1, false);
                UI_Control(cbBox_FilePath, false);

                SetProgramButton(enStatus.Testing);
                //Enable UI
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_TableMainEnable, (IntPtr)enSwitch.Disable, (IntPtr)0);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.FirmwareUpgradeFormHandle, enWM_MSG.WM_Program_Status, (IntPtr)enSTATUS_.SHOW, (IntPtr)enSTATUS_.SWITCH_MODE);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.SHOW, (IntPtr)StaticVar.Program_WaitUIPanelHandle);

                //Enter Upgrade Flow
                bool bForceUpgrade = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bForceUpgrade;
                bool bFW_CheckOnly = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bFW_CheckOnly;

                do
                {
                    ret = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_.UpgradeAction_API(ref strStatusMessage, bForceUpgrade, bFW_CheckOnly) == 0;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.FirmwareUpgradeFormHandle, enWM_MSG.WM_Program_Message, (IntPtr)enDBG_MSG_.INFO_MSG_APPEND, strStatusMessage);

                    if((!ret) && (!UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bFW_CheckOnly))
                    {
                        strErrMessage += "UpgradeAction failed\r\n";
                        if(Program.bSTRESS_BUILD)
                            m_iFailCnts++;

                        break;
                    }

                } while(false);

                if(!ret)
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.FirmwareUpgradeFormHandle, enWM_MSG.WM_Program_Message, (IntPtr)enDBG_MSG_.ERROR_MSG_APPEND, strErrMessage);
                    if(Program.bSTRESS_BUILD)
                        m_iFailCnts++;

                }
            }
            catch(Exception ex)
            {
                MyUIPage.ShowErrorDialog("系统提示", ex.ToString(), UIStyle.Red, true);
            }
            finally
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_UsbDeviceNotification, (IntPtr)m_iSelectedDev, StaticVar.MainFormHandle);

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PLEASE_WAIT, (IntPtr)enSTATUS_.HIDE, (IntPtr)StaticVar.Program_WaitUIPanelHandle);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_Program_UI_Status, (IntPtr)UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode, (IntPtr)0);

                MyTool.Delay_ms(500);
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bRunningStatus = false;

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.FirmwareUpgradeFormHandle, enWM_MSG.WM_Program_Status, (IntPtr)enSTATUS_.HIDE, (IntPtr)enSTATUS_.SENDING_FILE);
                SetProgramButton(enStatus.Stop);
                ////////////////////////////////////////
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.TuningVar.bReadParams_done = false;
                UI_Control(uiCheckBoxGroup1, true);
                UI_Control(cbBox_FilePath, true);
                this.Invoke(new Action(() =>
                {
                    UI_GBV.fmITS_Tool.Reset_UI();
                    lb_StressBuild.MySetStyle(m_iFailCnts == 0 ? STRESS_BUILD_Show.OK : STRESS_BUILD_Show.NG);
                }));
            }
        }

        private void WriteText(string myStr, Control ctl)
        {
            if (this.InvokeRequired)
            {
                delEditText myUpdate = new delEditText(WriteText);
                this.Invoke(myUpdate, myStr, ctl);
            }
            else
            {
                ctl.Text = myStr;
            }
        }

        private void AppandText(string myStr, Control ctl)
        {
            if (this.InvokeRequired)
            {
                delEditText myUpdate = new delEditText(AppandText);
                this.Invoke(myUpdate, myStr, ctl);
            }
            else
            {
                ctl.Text += myStr;
            }
        }

        private void vfFilePathItemAdd(string strNewPath)
        {
            // Remove All Item
            m_comboBox_FilePath.Items.Clear();
            // Re-Sorting Item
            for (int inCount = iMaxPathItem - 1; inCount > 0; inCount--)
            {
                strFilePathArray[inCount] = strFilePathArray[inCount - 1];
            }
            strFilePathArray[0] = strNewPath;
            for (int inCount = 0; inCount < iMaxPathItem; inCount++)
            {
                if ((strFilePathArray[inCount] != null) && (strFilePathArray[inCount] != ""))
                {
                    if (strFilePathArray[inCount].Length > 60)
                    {
                        m_comboBox_FilePath.Items.Add(strFilePathArray[inCount].Substring(0, 20) + "..." + strFilePathArray[inCount].Substring(strFilePathArray[inCount].Length - 20, 20));
                    }
                    else
                    {
                        m_comboBox_FilePath.Items.Add(strFilePathArray[inCount]);
                    }
                }
            }
            m_comboBox_FilePath.SelectedIndex = 0;
            // Save History ini
            vfSaveFilePathItem();
        }

        private void vfLoadFilePathItem()
        {
            for (int inCount = 0; inCount < iMaxPathItem; inCount++)
            {
                string strSector = "FWUpgrade_FilePath";
                string strValue = string.Format("Number {0:d}", inCount + 1);
                strFilePathArray[inCount] = MY_INI.IniReadValue(strSector, strValue, "");

                if ((strFilePathArray[inCount] != null) && (strFilePathArray[inCount] != ""))
                {
                    m_comboBox_FilePath.Items.Add(Path.GetFileName(strFilePathArray[inCount]));
                }
            }
        }

        private void vfSaveFilePathItem()
        {
            for (int inCount = 0; inCount < iMaxPathItem; inCount++)
            {
                MY_INI.IniWriteValueW("FWUpgrade_FilePath", string.Format("Number {0:d}", inCount + 1), strFilePathArray[inCount]);
            }
        }

        private bool DecodeFile(string strFilePath, ref string strErrorMSG)
        {
            strErrorMSG = "";
            int ErrCode = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_.DecodeFile_API(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU, strFilePath, ref strErrorMSG, false);

            if (use_commonflow)
                return (ErrCode < 0) ? false : true;
            if (ErrCode > 0)
            {
                //if (Enum.IsDefined(typeof(V3_MCU), UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU))
                if (EnumTool.IsEnumDefined<V3_MCU>(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.MCU))
                {
                    richTB_Result.Text += string.Format("AP CRC : 0x{0,8:X4} Checksum : 0x{1:X}\r\n"
                    , UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_.GetHexCodeSignature(SignatureType.CRC, HEX.APCode)
                    , UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_.GetHexCodeSignature(SignatureType.Checksum, HEX.APCode));

                    richTB_Result.Text += string.Format("Data CRC : 0x{0,8:X4} Checksum : 0x{1:X}\r\n"
                    , UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_.GetHexCodeSignature(SignatureType.CRC, HEX.DataCode)
                    , UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_.GetHexCodeSignature(SignatureType.Checksum, HEX.DataCode));
                }
                else
                {
                    List<KeyValuePair<string, uint>> AllCRC = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._FWUpgrade_.GetHexCodeSignature_Data(SignatureType.CRC);
                    for (int i = 0; i < AllCRC.Count; i++)
                        richTB_Result.Text += string.Format("Data Block_{0} Range: {1} CRC:0x{2:X4}\r\n", i, AllCRC[i].Key, AllCRC[i].Value);

                    if (!UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bDualIC_M2_Exist)
                        richTB_Result.Text += string.Format("IC Counts : {0}\r\n", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.HEX_Panel_Info.iIC_Counts);
                    else
                        richTB_Result.Text += string.Format("IC Counts : {0} + M2V\r\n", UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.HEX_Panel_Info.iIC_Counts);
                }
            }

            return (ErrCode < 0) ? false : true;
        }

        private void richTB_Result_TextChanged(object sender, EventArgs e)
        {
            richTB_Result.SelectionStart = richTB_Result.TextLength;
            richTB_Result.ScrollToCaret();
        }

        //private bool CheckProgramFunc(ProgramFunc SelectItem)
        //{
        //    return checkedListBox1.GetItemChecked((int)SelectItem);
        //}

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var MySender = sender as CheckedListBox;
            if (MySender.SelectedItem == null)
                return;
            string SelectStr = MySender.SelectedItem.ToString();
            int i = MySender.SelectedIndex;

            var Selected = EnumTool.GetValueFromDescription<ProgramFunc>(SelectStr);
            switch (Selected)
            {
                case ProgramFunc.Force_Upgrade:
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bForceUpgrade = !MySender.GetItemChecked(i);
                    for (int item = 0; item < MySender.Items.Count; item++)
                    {
                        if(MySender.GetItemText(MySender.Items[item]) == ProgramFunc.FW_CheckOnly.Desc())
                        {
                            MySender.SetSelected(item, false);
                            MySender.SetItemCheckState(item, CheckState.Unchecked);
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bFW_CheckOnly = false;
                        }
                    }
                    break;

                case ProgramFunc.FW_CheckOnly:
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bFW_CheckOnly = !MySender.GetItemChecked(i);
                    for (int item = 0; item < MySender.Items.Count; item++)
                    {
                        if(MySender.GetItemText(MySender.Items[item]) == ProgramFunc.Force_Upgrade.Desc())
                        {
                            MySender.SetSelected(item, false);
                            MySender.SetItemCheckState(item, CheckState.Unchecked);
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bForceUpgrade = false;
                        }
                    }
                    break;

                case ProgramFunc.I2C_ByPassINT:
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bI2C_ByPassINT = !MySender.GetItemChecked(i);
                    break;

                case ProgramFunc.I2C_HW_Reset:
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bBridge_HW_Reset = !MySender.GetItemChecked(i);
                    break;
            }
        }

        private void uiCheckBoxGroup1_ValueChanged(object sender, int index, string SelectStr, bool isChecked)
        {
            uiCheckBoxGroup1.ValueChanged -= new Sunny.UI.UICheckBoxGroup.OnValueChanged(this.uiCheckBoxGroup1_ValueChanged);
            var MySender = sender as UICheckBoxGroup;
            var Selected = EnumTool.GetValueFromDescription<ProgramFunc>(SelectStr);
            var Ori_ = MySender.SelectedItems.Select(x => EnumTool.GetValueFromDescription<ProgramFunc>(x.ToString())).ToList();
            switch (Selected)
            {
                case ProgramFunc.Force_Upgrade:
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bForceUpgrade = isChecked;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bFW_CheckOnly = false;
                    uiCheckBoxGroup1.UnSelectAll();
                    if (Ori_.Contains(ProgramFunc.FW_CheckOnly))
                        Ori_.RemoveAll(x => x == ProgramFunc.FW_CheckOnly);

                    uiCheckBoxGroup1.SelectedIndexes = new List<int>(Ori_.Select(x => (int)x).ToArray());
                    break;

                case ProgramFunc.FW_CheckOnly:
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bFW_CheckOnly = isChecked;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bForceUpgrade = false;
                    uiCheckBoxGroup1.UnSelectAll();
                    if (Ori_.Contains(ProgramFunc.Force_Upgrade))
                        Ori_.RemoveAll(x => x == ProgramFunc.Force_Upgrade);

                    uiCheckBoxGroup1.SelectedIndexes = new List<int>(Ori_.Select(x => (int)x).ToArray());
                    break;

                case ProgramFunc.I2C_ByPassINT:
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarUpgrade_Multi.bI2C_ByPassINT = isChecked;
                    break;

                case ProgramFunc.I2C_HW_Reset:
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarSenTest_Multi.bBridge_HW_Reset = isChecked;
                    break;
            }
            uiCheckBoxGroup1.ValueChanged += new Sunny.UI.UICheckBoxGroup.OnValueChanged(this.uiCheckBoxGroup1_ValueChanged);
        }
    }
}