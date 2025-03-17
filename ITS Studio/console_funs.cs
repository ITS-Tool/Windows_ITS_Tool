using CommonExt;
using FuncMethodCmd;
using ILITek_ITS_Tool_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ThirdPartyTools;

namespace ITS_Console
{
    public struct console_ctl
    {
        public bool b_log;
        public string str_log_path;
        public string[] str_cmd;
        public int mode;
        public bool b_pause;
        public bool b_show;
        public int i_delay_time;
        public upgrade_info upgrade;
    };

    public struct upgrade_info
    {
        public bool b_upgrade;
        public bool b_force;
        public bool b_check_fw;
        public string hex;
        public string tpl_file;
    };
    
    public struct cdc
    {
        public bool enable;
        public int frame_cnt;
        public object type;
        public enCDCDataFormat format;
        public bool b_record;
        public PaintTool_Var.enum_Pen_Status i_waittype;//0:not wait, 1:wait hover, 2: wait pen
        public int i_waittime;

    }

    public enum ErrorCodes : uint
    {
        [Description("Description for No Error\r\n")]
        ITS_ERR_NO_NG = 0x00000000,

        [Description("Description for Initial the device failed\r\n")]
        ITS_ERR_INITAIL_DEVICE_NG = 0x00008001,

        [Description("Description for Show IC information failed\r\n")]
        ITS_ERR_SHOW_IC_INFORMAITON_NG = 0x00008002,

        [Description("Description for Command is empty\r\n")]
        ITS_ERR_CMD_EMPTY_NG = 0x00008003,

        [Description("Description for Checking ic cmd failed\r\n")]
        ITS_ERR_Do_CHECK_IC_NG = 0x00008004,

        [Description("Description for Sending cmd failed\r\n")]
        ITS_ERR_SEND_CMD_NG = 0x00008005,

        [Description("Description for Set the tuning mode number failed\r\n")]
        ITS_ERR_SET_TUNING_MODE_NG = 0x00008006,

        [Description("Description for Executing set the tuning mode failed\r\n")]
        ITS_ERR_DO_SET_TUNING_MODE_NG = 0x00008007,

        [Description("Description for Parseing the cdc cmd failed\r\n")]
        ITS_ERR_PARSE_CDC_CMD_NG = 0x00008008,

        [Description("Description for Doing the cdc cmd failed\r\n")]
        ITS_ERR_DO_CDC_CMD_NG = 0x00008009,

        [Description("Description for Getting the tuning mode number failed\r\n")]
        ITS_ERR_GET_TUNING_MODE_NG = 0x0000800A,

        [Description("Description for Enable the touch mode failed\r\n")]
        ITS_ERR_ENALBE_TOUCH_MODE_NG = 0x0000800B,

        [Description("Description for Disable the touch mode failed\r\n")]
        ITS_ERR_DISALBE_TOUCH_MODE_NG = 0x0000800C,

        [Description("Description for the numbers of noise frequency is wroing\r\n")]
        ITS_ERR_NOISE_FRQ_PARAMS_NUMBERS_NG = 0x0000800D,

        [Description("Description for the range of the sine parameters of noise frequency is out of range\r\n")]
        ITS_ERR_NOISE_FRQ_SINE_PARAMS_NUMBERS_NG = 0x0000800E,

        [Description("Description for the range of the swcap parameters of noise frequency is out of range\r\n")]
        ITS_ERR_NOISE_FRQ_SWCAP_PARAMS_NUMBERS_NG = 0x00008010,

        [Description("Description for the range of the self parameters of noise frequency is out of range\r\n")]
        ITS_ERR_NOISE_FRQ_SELF_PARAMS_NUMBERS_NG = 0x00008011,

        [Description("Description for the type of noise frequency is wrong\r\n")]
        ITS_ERR_NOISE_FRQ_TYPE_NG = 0x00008012,

        [Description("Description for the frame count of noise frequency is wrong\r\n")]
        ITS_ERR_NOISE_FRQ_FRAME_CNT_NG = 0x00008012,

        [Description("Description for the pen signal of noise frequency is wrong\r\n")]
        ITS_ERR_NOISE_FRQ_PEN_SIGNAL_NG = 0x00008013,

        [Description("Description for the pen type of noise frequency is empty\r\n")]
        ITS_ERR_NOISE_FRQ_PEN_TYPE_EMPTY = 0x00008014,

        [Description("Description for upgrading fw failed\r\n")]
        ITS_ERR_UPGRADE_FW_NG = 0x00008015,

        [Description("Description for Parseing the cdc chart type failed\r\n")]
        ITS_ERR_PARSE_CDC_CHARTTYPE_NG = 0x00008016,

        [Description("Description for Others failed\r\n")]
        ITS_ERR_OTHERS_NG = 0x00008888,
    }

    internal partial class ConsoleFunsClass
    {
        #region USB devices GUID
        //Human Interface Device (HID)/人機界面設備	{4d1e55b2-f16f-11cf-88cb-001111000030}
        //USB Raw Device/USB設備	{a5dcbf10-6530-11d2-901f-00c04fb951ed}
        private const int DbtDevtypDeviceinterface = 5;
        private readonly Guid GuidDevinterfaceUSBDevice = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

        [DllImport("user32.dll")]
        private static extern bool UnregisterDeviceNotification(IntPtr handle);

        private void UnregisterUsbDeviceNotification()
        {
            UnregisterDeviceNotification(m_NotificationHandle);
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

        public void RegisterUsbDeviceNotification(IntPtr windowHandle)
        {
           // Console.WriteLine("{0} >> [RegisterUsbDeviceNotification]\r\n", Tool.GetParentInfo());

            DevBroadcastDeviceinterface dbi = new DevBroadcastDeviceinterface
            {
                DeviceType = DbtDevtypDeviceinterface,
                Reserved = 0,
                ClassGuid = GuidDevinterfaceUSBDevice,
                Name = 0
            };
            dbi.Size = Marshal.SizeOf(dbi);

            IntPtr buffer = Marshal.AllocHGlobal(dbi.Size);
            Marshal.StructureToPtr(dbi, buffer, true);

            m_NotificationHandle = RegisterDeviceNotification(windowHandle, buffer, 0);
        }

        private IntPtr m_NotificationHandle;

        #endregion USB devices GUID
    }
    internal partial class ConsoleFunsClass
    {//for other using
        private void vfWriteUSB_Base(string text, ref string result, ref byte[] ByteArray)
        {
            var RegexRule = new Regex("0x", RegexOptions.IgnoreCase);
            string OriSentence = text;
            string newSentence = RegexRule.Replace(OriSentence, "");//去除0x
            newSentence = newSentence.Replace(",", " ");
            string[] tmp = newSentence.Split(' ');
            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i].Length < 2)
                {
                    tmp[i] = tmp[i].PadLeft(2, '0');
                }
                else if (tmp[i].Length > 2)
                {
                    result = string.Format("Type format Error : {0}", tmp[i]);
                    return;
                }
            }
            OriSentence = string.Join("", tmp);
            newSentence = RegexRule.Replace(OriSentence.Trim().Replace(" ", ""), "");
            newSentence = newSentence.Replace(",", "");
            int inLen = newSentence.Length;

            if (inLen % 2 > 0)
                return;

            ByteArray = ArrayTool.StringToByteArray(newSentence);

            if (newSentence.Length < 6)
                return;

            m_IliFunc.m_HidAPI.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_IliFunc.m_iDev], ByteArray, true);
        }

        private bool Console_Read_Base(ref StringBuilder strbudBuffer)
        {
            int inDataTabCount = 0;
            byte Header = 0;
            do
            {
                m_IliFunc.MyTool.Delay_ms(1);
            } while (m_var._Console_VAR.byReceiverData.Count == 0);
            Header = m_var._Console_VAR.byReceiverData[(int)Packet.IC_Header];

            if (Header == (byte)Command.Bridge_CMD._0XAC)
                return false;

            strbudBuffer.Append("[RECV] <<<\t");

            int iValue = Header & 0xF0;
            if (Header == 0xA3)
            {
                int iHeader_Length = EnumTool.EnumToList<Packet>().Count();
                int iRcvLeng = m_var._Console_VAR.byReceiverData[(int)Packet.R_1] + iHeader_Length;
                string strprt = string.Join(" ", m_var._Console_VAR.byReceiverData.GetRange(0, iRcvLeng).Select(x => x.ToString("X2")).ToArray());
                strbudBuffer.AppendLine(strprt);
                strbudBuffer.AppendLine("=======================");
                strbudBuffer.AppendLine(JudgeReturnCommand(m_var._Console_VAR.byReceiverData));
                strbudBuffer.AppendLine("=======================");
            }
            else if (Header == 0xA4)
            {
                int iRcvLen = m_var._Console_VAR.byReceiverData[3] + m_var._Console_VAR.byReceiverData[4];
            }
            else
            {
                if ((iValue == 0x40) || (iValue == 0x00))//畫線報點
                {
                    strbudBuffer.Append("Position\r\n");
                    int inTotalCount = 0x40;

                    List<string> tmp = m_var._Console_VAR.byReceiverData.GetRange(0, inTotalCount).Select(x => string.Format(" {0:X2}", x)).ToList();
                    var _strprt = from p in tmp
                                  select new { Owner = ((++inDataTabCount % 32 == 0) ? (p + "\r\n") : (inDataTabCount % 4) == 0 ? p + " " : p) }.Owner;
                    strbudBuffer.Append(string.Join("", _strprt.ToArray()));
                }
            }
            return true;
        }

        private string JudgeReturnCommand(List<byte> DataBuff)
        {
            byte _CMD = DataBuff[(int)Packet.CMD];
            byte _ReadLen = DataBuff[(int)Packet.R_1];
            StringBuilder Log = new StringBuilder();

            byte _FirstData = (byte)Packet.R_1 + 1;
            switch (_CMD)
            {
                case (byte)Command.TP_CMD_0XC.Get_CheckMode_C0:
                    if (Enum.IsDefined(typeof(FW_MODE), (int)DataBuff[_FirstData]))
                        Log.AppendFormat("FW : {0}", Enum.GetName(typeof(FW_MODE), DataBuff[_FirstData]));
                    break;

                case (byte)Command.TP_CMD_0X4.Get_FirmwareVersion_40:
                    Log.AppendFormat("FW Ver. : V {0}", string.Join(".", DataBuff.GetRange(_FirstData, _ReadLen).Select(x => string.Format("{0:X2}", x))));
                    break;

                case (byte)Command.TP_CMD_0X4.Get_ProtocolVersion_42:
                    int iTitle = (int)DataBuff[_FirstData];
                    Log.AppendFormat("IC protocol is V{0}", iTitle);
                    Log.AppendLine();
                    Log.AppendFormat("Protocol Ver. : V {0}", string.Join(".", DataBuff.GetRange(_FirstData, _ReadLen).Select(x => string.Format("{0:X2}", x))));
                    break;

                case (byte)Command.TP_CMD_0X6.Get_MCU_KernelVersion_61:
                    int iMCU = (int)DataBuff[_FirstData] | ((int)DataBuff[_FirstData + 1]) << 8;
                    Log.AppendFormat("MCU {0:X4}", iMCU);
                    Log.AppendLine();
                    Log.AppendFormat("KernelVersion : V {0}", string.Join(".", DataBuff.GetRange(_FirstData, _ReadLen).Select(x => string.Format("{0:X2}", x))));
                    break;

                case (byte)Command.TP_CMD_0X2.Get_PanelInfo_20:
                    Log.AppendFormat("Panel Info : {0}", string.Join(".", DataBuff.GetRange(_FirstData, _ReadLen).Select(x => string.Format("{0:X2}", x))));
                    break;

                case (byte)Command.TP_CMD_0X4.Get_CorelVersion_43:
                    Log.AppendFormat("Core Ver. : V {0}", string.Join(".", DataBuff.GetRange(_FirstData, _ReadLen).Select(x => string.Format("{0:X2}", x))));
                    break;

                case (byte)Command.TP_CMD_0XC.Get_Verify_Code_C7:
                    Log.AppendFormat("Verify Code : {0}", string.Join("", DataBuff.GetRange(_FirstData, _ReadLen).Select(x => string.Format("{0:X2}", x))));
                    break;

                default:
                    break;
            }
            return Log.ToString();
        }

        private void Console_Start()
        {
            m_gc._ConsolTool.ConsoleTool_Start();
        }

        private void Console_Stop()
        {
            m_gc._ConsolTool.ConsoleTool_Stop();
            m_var.Var_Multi.bThreadDecodeData_En = false;
            m_var._Console_VAR.bRunningStatus = false;
        }

        private bool Check_PenType(string pen)
        {
            bool ret = true;
            switch (pen)
            {
                case "wgp":
                    if ((m_var.Var_Multi.DevPanelInfo.SupportPenType & Pen_CDC_Format.enPenType.WGP) != Pen_CDC_Format.enPenType.WGP)
                    {
                        Console.WriteLine("Error: FW didn't support WGP\r\n");
                        ret = false;
                    }
                    break;
                case "usi":
                    if ((m_var.Var_Multi.DevPanelInfo.SupportPenType & Pen_CDC_Format.enPenType.USI) != Pen_CDC_Format.enPenType.USI)
                    {
                        Console.WriteLine("Error: FW didn't support USI\r\n");
                        ret = false;
                    }
                    break;
                case "mpp":
                    if ((m_var.Var_Multi.DevPanelInfo.SupportPenType & Pen_CDC_Format.enPenType.MPP) != Pen_CDC_Format.enPenType.MPP)
                    {
                        Console.WriteLine("Error: FW didn't support MPP\r\n");
                        ret = false;
                    }
                    break;
                default:
                    ret = false;
                    break;
            }
            return ret;
        }
    }

    internal partial class ConsoleFunsClass
    {//for fw upgrade using
        private bool FWUpgrade(string file_path, bool force_upgrade, bool check_fw)
        {
            if (b_alreadyInit == false)
            {
                StaticVar.LogWrite("Error: Please initial the system first\r\n");
                return false;
            }
            else
            {
                Console_Start();
                if (FWUpgradeBase(file_path, force_upgrade, check_fw))
                {
                    if (check_fw)
                        StaticVar.LogWrite("Info : Check FW done...\r\n");
                    else
                        StaticVar.LogWrite("Info : Upgrade FW done...\r\n");
                    return true;
                }
                else
                {
                    if (check_fw)
                        StaticVar.LogWrite("Info : FW was different...\r\n");
                    else
                        StaticVar.LogWrite("Info : Upgrading FW was a failed...\r\n");
                    return false;
                }
            }
        }

        private bool FWUpgradeBase(string strFilePath, bool bForceUpgrade, bool bFW_CheckOnly)
        {
            string strErrorMSG = "";
            bool bResult = m_gc._FWUpgrade_.DecodeFile_API(m_var.Var_Multi.DevPanelInfo.MCU, strFilePath, ref strErrorMSG);

#if false
            if (bResult)
            {
                StaticVar.LogWrite(string.Format("CRC : 0x{0,8:X8} Checksum : 0x{1:X}\r\n"
                    , m_gc._FWUpgrade_.GetHexCodeSignature(SignatureType.CRC, HEX.APCode)
                    , m_gc._FWUpgrade_.GetHexCodeSignature(SignatureType.Checksum, HEX.APCode)));
                StaticVar.LogWrite(string.Format("CRC : 0x{0,8:X8} Checksum : 0x{1:X}\r\n"
                    , m_gc._FWUpgrade_.GetHexCodeSignature(SignatureType.CRC, HEX.DataCode)
                    , m_gc._FWUpgrade_.GetHexCodeSignature(SignatureType.Checksum, HEX.DataCode)));
            }
            else
            {
                StaticVar.LogWrite(strErrorMSG);
                strErrorMSG = "";
                List<KeyValuePair<string, uint>> AllCRC = m_gc._FWUpgrade_.GetHexCodeSignature_Data(SignatureType.CRC);
                if (AllCRC != null)
                {
                    for (int i = 0; i < AllCRC.Count; i++)
                        strErrorMSG += string.Format("Data Block_{0} Range: {1} CRC:0x{2:X4}\r\n", i, AllCRC[i].Key, AllCRC[i].Value);

                    if (!m_var.VarUpgrade_Multi.bDualIC_M2_Exist)
                        strErrorMSG += string.Format("IC Counts : {0}\r\n", m_var.VarUpgrade_Multi.HEX_Panel_Info.iIC_Counts);
                    else
                        strErrorMSG += string.Format("IC Counts : {0} + M2V\r\n", m_var.VarUpgrade_Multi.HEX_Panel_Info.iIC_Counts);
                    StaticVar.LogWrite(strErrorMSG);
                }
                return false;
            }
#endif
            string strStatusMessage = "";
            bool ret = false;
            try
            {
                ret = m_gc._FWUpgrade_.UpgradeAction_API(ref strStatusMessage, bForceUpgrade, bFW_CheckOnly);
            }
            catch (Exception ex)
            {
                StaticVar.LogWrite(ex.ToString());
                return false;
            }

            if (ret)
            {
                ret &= m_gc.m_Flow.SetTouchMode(TouchMode.SuspendMode, Command._SWITCH.Dis);
                if (m_gc._DeviceMain.CheckDevMode_API(FW_MODE.AP_MODE))
                {
                    m_IliFunc.MyTool.Delay_ms(1000);
                    if (!m_IliFunc.m_GBV.Globe_Multi._DeviceMain.Get_IC_Information(m_var.FW_Upgrade_DevStatus.Master_Mode
                        , false, ref strStatusMessage, ref m_IliFunc.m_GBV.ConsoleTool.strErrorMessage))
                    {
                        StaticVar.LogWrite((m_IliFunc.m_GBV.ConsoleTool.strErrorMessage));
                        ret = false;
                    }
                }
                ret &= m_gc.m_Flow.SetTouchMode(TouchMode.NormalMode, Command._SWITCH.Dis);
            }
            return ret;
        }

        private int DoFWUpgrade(upgrade_info _upgrade)
        {
            if (_upgrade.hex != "")
            {
                if (_upgrade.b_check_fw != true)
                {
                    StaticVar.LogWrite("\r\n==========Old Firmware Information==========\r\n");
                    ShowICInfo();
                    StaticVar.LogWrite("\r\n============================================\r\n");
                }
                bool ret = FWUpgrade(_upgrade.hex, _upgrade.b_force, _upgrade.b_check_fw);
                if (!ret)
                {
                    if (_upgrade.b_check_fw != true)
                    {
                        return (int)ErrorCodes.ITS_ERR_UPGRADE_FW_NG;
                    }
                }
                else
                {
                    if (_upgrade.b_check_fw != true)
                    {
                        StaticVar.LogWrite("Info : Successful programming\r\n");
                        StaticVar.LogWrite("\r\n==========New Firmware Information==========\r\n");
                        ShowICInfo();
                        StaticVar.LogWrite("\r\n============================================\r\n");
                    }
                }
            }
            return (int)ErrorCodes.ITS_ERR_NO_NG;
        }
    }

    internal partial class ConsoleFunsClass
    {//for noise frequency using
        private List<NoiseFrqVariable.PenSingalFormat> m_ls_PenSingal = new List<NoiseFrqVariable.PenSingalFormat>();
        private NoiseFrqVariable.FrqParams m_PenParams = new NoiseFrqVariable.FrqParams();

        private ErrorCodes NoiseFrq_Init_Params(string str)
        {
            INI_Tool MY_INI = new INI_Tool(ITS_Directory.Setting_INI);
            if (m_IliFunc.LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_Start = MY_INI.IniReadInt("NoiseFrq_V6", "Sine_Start", 30);
                m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_End = MY_INI.IniReadInt("NoiseFrq_V6", "Sine_End", 90);
                m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_Fr_Num = MY_INI.IniReadInt("NoiseFrq_V6", "Sine_Frame_Cnt", 1);
                m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Swcap_Start = MY_INI.IniReadInt("NoiseFrq_V6", "Swcap_Start", 30);
                m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Swcap_End = MY_INI.IniReadInt("NoiseFrq_V6", "Swcap_End", 90);
                m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Self_Start = MY_INI.IniReadInt("NoiseFrq_V6", "Self_Start", 30);
                m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Self_End = MY_INI.IniReadInt("NoiseFrq_V6", "Self_End", 90);
            }
            else
            {
                m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_Start = MY_INI.IniReadInt("NoiseFrq_V3", "Sine_Start", 30);
                m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_End = MY_INI.IniReadInt("NoiseFrq_V3", "Sine_End", 90);
            }

            string[] words = str.Split(' ');
            NoiseFrqVariable.FrqParams Sine = new NoiseFrqVariable.FrqParams();
            NoiseFrqVariable.FrqParams SWCap = new NoiseFrqVariable.FrqParams();
            NoiseFrqVariable.FrqParams Self = new NoiseFrqVariable.FrqParams();
            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                if (words.Count() < 12)
                    return ErrorCodes.ITS_ERR_NOISE_FRQ_PARAMS_NUMBERS_NG;
            }
            else
            {
                if (words.Count() < 4)
                    return ErrorCodes.ITS_ERR_NOISE_FRQ_PARAMS_NUMBERS_NG;
            }

            Sine.iStart = Convert.ToInt16(words[0]);
            Sine.iEnd = Convert.ToInt16(words[1]);
            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                Sine.iStep = Convert.ToInt16(words[2]);
            else
                Sine.iStep = (int)(Convert.ToDouble(words[2]) * 10);
            Sine.iMinSignal = Convert.ToInt16(words[3]);
            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                SWCap.iStart = Convert.ToInt16(words[4]);
                SWCap.iEnd = Convert.ToInt16(words[5]);
                SWCap.iStep = Convert.ToInt16(words[6]);
                SWCap.iMinSignal = Convert.ToInt16(words[7]);
                Self.iStart = Convert.ToInt16(words[8]);
                Self.iEnd = Convert.ToInt16(words[9]);
                Self.iStep = Convert.ToInt16(words[10]);
                Self.iMinSignal = Convert.ToInt16(words[11]);
            }
            ErrorCodes ret = check_params(Sine, SWCap, Self);
            if (ret == ErrorCodes.ITS_ERR_NO_NG)
                m_gc._NoiseFrq_.ConsoleTool_SetParams(Sine, SWCap, Self);
            return ret;
        }

        private ErrorCodes SetPen_params(string protocol, string lspen)
        {
            INI_Tool MY_INI = new INI_Tool(ITS_Directory.Setting_INI);
            if (m_IliFunc.LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                if (protocol != null)
                {
                    m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Pen_WGP_Start = MY_INI.IniReadInt("NoiseFrq_V6", "Pen_WGP_Start", 10);
                    m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Pen_WGP_End = MY_INI.IniReadInt("NoiseFrq_V6", "Pen_WGP_End", 300);
                    m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Pen_USI_Start = MY_INI.IniReadInt("NoiseFrq_V6", "Pen_USI_Start", 10);
                    m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Pen_USI_End = MY_INI.IniReadInt("NoiseFrq_V6", "Pen_USI_End", 300);
                    m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Pen_MPP_Start = MY_INI.IniReadInt("NoiseFrq_V6", "Pen_MPP_Start", 10);
                    m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Pen_MPP_End = MY_INI.IniReadInt("NoiseFrq_V6", "Pen_MPP_End", 300);
                    m_PenParams.iStep = 1;
                    if (protocol == "wgp")
                    {
                        m_PenParams.iPenProtocol = 1;
                        m_PenParams.iStart = m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Pen_WGP_Start;
                        m_PenParams.iEnd = m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Pen_WGP_End;
                    }
                    else if (protocol == "usi")
                    {
                        m_PenParams.iPenProtocol = 2;
                        m_PenParams.iStart = m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Pen_USI_Start;
                        m_PenParams.iEnd = m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Pen_USI_End;
                    }
                    else if (protocol == "mpp")
                    {
                        m_PenParams.iPenProtocol = 3;
                        m_PenParams.iStart = m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Pen_MPP_Start;
                        m_PenParams.iEnd = m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Pen_MPP_End;
                    }
                    else
                    {
                        return ErrorCodes.ITS_ERR_NOISE_FRQ_TYPE_NG;
                    }
                }
                if(lspen!=null)
                {
                    m_ls_PenSingal.Clear();
                    string[] words = lspen.Split(';');
                    if (words.Count() < 2)
                        return ErrorCodes.ITS_ERR_NOISE_FRQ_PEN_SIGNAL_NG;
                    else
                    {
                        foreach(string i in words)
                        {
                            string[] w = i.Split(',');
                             m_ls_PenSingal.Add(new NoiseFrqVariable.PenSingalFormat
                            {
                                sFreq = w[0],
                                sValue = w[1]
                            });
                        }
                        m_PenParams.ls_PenSignal = m_ls_PenSingal; //不適用
                    }
                }
                m_gc._NoiseFrq_.SetParams(null, null, null, m_PenParams);
            }
            return ErrorCodes.ITS_ERR_NO_NG;
        }

        private ErrorCodes check_params(NoiseFrqVariable.FrqParams Sine, NoiseFrqVariable.FrqParams SWCap, NoiseFrqVariable.FrqParams Self)
        {
            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                //==========================================================
                int MinGapProtect = 5;
                if (((Sine.iStart + (Sine.iStep * MinGapProtect)) >= Sine.iEnd)
                    || (Sine.iStart < m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_Start)
                    || (Sine.iEnd > m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_End))
                {
                    StaticVar.LogWrite("Error: Invalid Sine Freq Range!\r\n");
                    return ErrorCodes.ITS_ERR_NOISE_FRQ_SINE_PARAMS_NUMBERS_NG;
                }

                if (((SWCap.iStart + (SWCap.iStep * MinGapProtect)) >= SWCap.iEnd)
                    || (SWCap.iStart < m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Swcap_Start)
                    || (SWCap.iEnd > m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Swcap_End))
                {
                    StaticVar.LogWrite("Error: Invalid SWCap Freq Range!");
                    return ErrorCodes.ITS_ERR_NOISE_FRQ_SWCAP_PARAMS_NUMBERS_NG;
                }

                if (((Self.iStart + (Self.iStep * MinGapProtect)) >= Self.iEnd)
                    || (Self.iStart < m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Self_Start)
                    || (Self.iEnd > m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Self_End))
                {
                    StaticVar.LogWrite("Error: Invalid Self Freq Range!");
                    return ErrorCodes.ITS_ERR_NOISE_FRQ_SELF_PARAMS_NUMBERS_NG;
                }
            }
            else
            {
                if (Sine.iStart >= Sine.iEnd)
                {
                    StaticVar.LogWrite("Error: Invalid Freq Range!");
                    return ErrorCodes.ITS_ERR_NOISE_FRQ_SINE_PARAMS_NUMBERS_NG;
                }
                int Kernel_Ver = (m_var.Var_Multi.GetVal(Command.enum_VERSION.MCU_Kernel_Ver1) << 8)
               + m_var.Var_Multi.GetVal(Command.enum_VERSION.MCU_Kernel_Ver0);
                if (Enum.IsDefined(typeof(OldV3_MCU), Kernel_Ver))
                {

                    if (Sine.iEnd > m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_End)
                    {
                        StaticVar.LogWrite(string.Format("23xx IC, set end freq to {0}KHz....\r\n", m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_End.ToString()));
                        return ErrorCodes.ITS_ERR_NOISE_FRQ_SINE_PARAMS_NUMBERS_NG;
                    }
                    if (Sine.iStart < m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_Start)
                    {
                        StaticVar.LogWrite(string.Format("23xx IC, set start freq to {0}KHz....\r\n", m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_Start.ToString()));
                        return ErrorCodes.ITS_ERR_NOISE_FRQ_SINE_PARAMS_NUMBERS_NG;
                    }
                }
                else if (Kernel_Ver == (int)V3_MCU.IC_2510 || Kernel_Ver == (int)V3_MCU.IC_2511 || Kernel_Ver == (int)V3_MCU.IC_2712)
                {
                    if (Sine.iEnd > m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_End)
                    {
                        StaticVar.LogWrite(string.Format("23xx IC,  set end freq to {0}KHz....\r\n", m_IliFunc.m_GBV.Globe_Multi._NoiseFrq_.m_FrqRangeVar.Sine_End.ToString()));
                        return ErrorCodes.ITS_ERR_NOISE_FRQ_SINE_PARAMS_NUMBERS_NG;
                    }
                }
            }
            return ErrorCodes.ITS_ERR_NO_NG;
        }

        private ErrorCodes ScanNoiseFrq_cmd(string item_new, ref bool NoiseFrq_en, ref bool pen_setting)
        {
            ErrorCodes ret = ErrorCodes.ITS_ERR_NO_NG;
            if (item_new.Contains("/nf="))
            {
                string[] words = item_new.Split('=');
                if (words.Count() < 2)
                    words[1] = "30 90 5 7 4 300 2 7 4 300 2 7";
                else
                {
                    if (words[1] == "")
                        words[1] = "30 90 5 7 4 300 2 7 4 300 2 7";
                }
                ret = NoiseFrq_Init_Params(words[1]);
                if (ret != ErrorCodes.ITS_ERR_NO_NG)
                {
                    StaticVar.LogWrite("Error: Parameter string is wrong: {0}\r\n", words[1]);
                    return ret;
                }
                NoiseFrq_en = true;
            }
            else if(item_new.Contains("/pen="))
            {
                string[] words = item_new.Split('=');
                if (words.Count() < 2)
                {
                    StaticVar.LogWrite("Error: Parameter string is wrong: {0}\r\n", item_new);
                    return ErrorCodes.ITS_ERR_NOISE_FRQ_TYPE_NG;
                }
                else
                {
                    if (words[1] == "")
                    {
                        StaticVar.LogWrite("Error: Parameter string is wrong: {0}\r\n", item_new);
                        return ErrorCodes.ITS_ERR_NOISE_FRQ_TYPE_NG;
                    }
                }
                if (Check_PenType(words[1]) == false)
                    return ErrorCodes.ITS_ERR_NOISE_FRQ_TYPE_NG;
                ErrorCodes err = SetPen_params(words[1], null);
                if (err != ErrorCodes.ITS_ERR_NO_NG)
                    return err;
                pen_setting = true;
            }
            else if (item_new.Contains("/nf_type="))
            {
                string[] words = item_new.Split('=');
                if (words.Count() == 2)
                {
                    int val = Convert.ToInt16(words[1]);
                    if (val >= 1 && val <= 7)
                    {
                        bool a = (val & (int)BIT._0) > 0 ? true : false;
                        bool b = ((val & (int)BIT._1) >> 1) > 0 ? true : false;
                        bool c = ((val & (int)BIT._2) >> 2) > 0 ? true : false;
                        if (m_var.Var_Multi.Protocol_VerNum < Protocol_Ver.v601)
                            m_var.NoiseFrqVar.iDataTypeCnt = 1;
                        else
                            m_var.NoiseFrqVar.iDataTypeCnt = m_gc._NoiseFrq_.Judge_ScanDataType(a, b, c);
                    }
                }
                else
                {
                    StaticVar.LogWrite("Error: Parameter string is wrong: {0}\r\n", item_new);
                    return ErrorCodes.ITS_ERR_NOISE_FRQ_TYPE_NG;
                }
            }
            else if (item_new.Contains("/nf_cnt="))
            {
                string[] words = item_new.Split('=');
                if (words.Count() == 2)
                {
                    if (m_var.Var_Multi.Protocol_VerNum < Protocol_Ver.v601)
                        m_var.NoiseFrqVar.iFrameNum = 0;
                    else
                        m_var.NoiseFrqVar.iFrameNum = Convert.ToInt16(words[1]);
                }
                else
                {
                    StaticVar.LogWrite("Error: Parameter string is wrong: {0}\r\n", item_new);
                    return ErrorCodes.ITS_ERR_NOISE_FRQ_FRAME_CNT_NG;
                }
            }
            else if (item_new.Contains("/pen_signal"))
            {
                string[] words = item_new.Split('=');
                if (words.Count() < 2)
                    words[1] = "10,10;300,10";
                else
                {
                    if (words[1] == "")
                        words[1] = "10,10;300,10";
                }
                ErrorCodes err = SetPen_params(null, words[1]);
                if (err != ErrorCodes.ITS_ERR_NO_NG)
                    return err;
            }
            return ret;
        }

        private ErrorCodes DoNoiseFrq(bool NoiseFrq_en, bool bPenSetting)
        {
            if (NoiseFrq_en)
            {
                //vfSetUIDatas()
                if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V3)
                    m_var.NoiseFrqVar.iDataTypeCnt = m_gc._NoiseFrq_.Judge_ScanDataType(true, false, false);

                StaticVar.LogWrite("\r\nStart to scan noise frequency...\r\n");
                if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V3)
                StaticVar.LogWrite("Noise Frequency Sine Setting:\r\n   Range: {0}~{1} KHz\r\n   Step:  {2} KHz\r\n   Minimun Signal Level:  {3}\r\n\r\n", m_var.NoiseFrqVar.Sine._params.iStart, m_var.NoiseFrqVar.Sine._params.iEnd, (double)(m_var.NoiseFrqVar.Sine._params.iStep/10.0), m_var.NoiseFrqVar.Sine._params.iMinSignal);
                else
                    StaticVar.LogWrite("Noise Frequency Sine Setting:\r\n   Range: {0}~{1} KHz\r\n   Step:  {2} KHz\r\n   Minimun Signal Level:  {3}\r\n\r\n", m_var.NoiseFrqVar.Sine._params.iStart, m_var.NoiseFrqVar.Sine._params.iEnd, m_var.NoiseFrqVar.Sine._params.iStep, m_var.NoiseFrqVar.Sine._params.iMinSignal);


                if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                {
                    StaticVar.LogWrite("Noise Frequency SWCap Setting:\r\n   Range: {0}~{1} KHz\r\n   Step:  {2} KHz\r\n   Minimun Signal Level:  {3}\r\n\r\n", m_var.NoiseFrqVar.SWCap._params.iStart, m_var.NoiseFrqVar.SWCap._params.iEnd, m_var.NoiseFrqVar.SWCap._params.iStep, m_var.NoiseFrqVar.SWCap._params.iMinSignal);

                    StaticVar.LogWrite("Noise Frequency Self Setting:\r\n   Range: {0}~{1} KHz\r\n   Step:  {2} KHz\r\n   Minimun Signal Level:  {3}\r\n\r\n", m_var.NoiseFrqVar.Self._params.iStart, m_var.NoiseFrqVar.Self._params.iEnd, m_var.NoiseFrqVar.Self._params.iStep, m_var.NoiseFrqVar.Self._params.iMinSignal);

                    if (Enum.IsDefined(typeof(V6_29XX_MCU), m_var.Var_Multi.DevPanelInfo.MCU))
                    {
                        m_gc._NoiseFrq_.isForPenTest = true;
                        if (bPenSetting)
                        {
                            StaticVar.LogWrite("Noise Frequency Pen Setting:\r\n   Range: {0}~{1} KHz\r\n   Step:  {2} KHz\r\n   Minimun Signal Level:  {3}\r\n", m_var.NoiseFrqVar.Pen._params.iStart, m_var.NoiseFrqVar.Pen._params.iEnd, m_var.NoiseFrqVar.Pen._params.iStep, m_var.NoiseFrqVar.Pen._params.iMinSignal);
                            StaticVar.LogWrite("   Threshold level:Range    Value\r\n");
                            foreach (var i in m_var.NoiseFrqVar.Pen._params.ls_PenSignal)
                                StaticVar.LogWrite("                   {0}      {1}\r\n", i.sFreq, i.sValue);
                        }
                        else
                        {
                            StaticVar.LogWrite("Noise Frequency Pen Setting: Empty\r\n");
                            return ErrorCodes.ITS_ERR_NOISE_FRQ_PEN_TYPE_EMPTY;
                        }
                    }
                    else
                        m_gc._NoiseFrq_.isForPenTest = false;

                    StaticVar.LogWrite("Frame Counts:{0}\r\n", m_var.NoiseFrqVar.iFrameNum);
                    if (m_var.NoiseFrqVar.iDataTypeCnt == 0 || m_var.NoiseFrqVar.iFrameNum == 0)
                    {
                        StaticVar.LogWrite("Error: Noise Frequency was a failed. Parameter string is wrong: \r\n");
                        m_gc._NoiseFrq_.Gen_NoiseFre_Report(false);
                        return ErrorCodes.ITS_ERR_OTHERS_NG;
                    }
                    
                }
                m_gc._NoiseFrq_.vbStartNoiseFre();
                m_gc._NoiseFrq_.InitNoiseFreqBuffer(m_var.Var_Multi.Protocol_VerNum);
                m_gc._NoiseFrq_.NoiseFreTool_Init();
                string strError = "";
                if (m_gc._NoiseFrq_.NoiseFreTool_Start(ref strError) == false)
                {
                    StaticVar.LogWrite("Error: {0}\r\n", strError);
                    return ErrorCodes.ITS_ERR_OTHERS_NG;
                }
                else
                {
                    if(m_gc._NoiseFrq_.UpdateJudgement())
                        StaticVar.LogWrite("Info : Noise frequency result was pass\r\n");
                    else
                        StaticVar.LogWrite("Info : Noise frequency result was failure\r\n");

                    m_gc._NoiseFrq_.Gen_NoiseFre_Report(true);
                }
                StaticVar.LogWrite("Info : Noise Frequency done\r\n");
            }
            return ErrorCodes.ITS_ERR_NO_NG;
        }
    }

    internal partial class ConsoleFunsClass
    {//for cdc tool using
        private Stopwatch m_checktime = new Stopwatch();

        private INI_Tool m_CDC_Tool_INI;

        private int m_Max_PenX_Bottom, m_Max_PenX_Left, m_Max_PenY_Bottom, m_Max_PenY_Left;

        private void SetDefault_CDC_Type()
        {
            m_gc._CDCTool_.SetDefault_CDC_Type_Base();
        }

        private void CDCTool_Init()
        {
            if (m_gc._CDCTool_ != null)
                m_gc._CDCTool_.CDCTool_Init(CDC_Tool_INI, false, null, false);//開啟Test Mode,INT Monitor
            StaticVar.bCDC_Record = false;
            StaticVar.bCDC_Start2Record = false;
        }
        private Dictionary<int, string> dicDataType = new Dictionary<int, string>() { { 0, "MutualData" }, { 1, "SelfData" }, { 2, "KeyData" }, { 3, "PenData" } };
        private void TChart_Init(bool bRecoverRec, bool is_mutual)
        {
            //INI_Tool CDC_Tool_INI = new INI_Tool(ITS_Directory.CDCTool_INI);
            #region CDC_DataType Hide or Show
            if (is_mutual)
            {
                StaticVar.CDC_DataType = enCDC_Data.None;
                foreach (var item in dicDataType)
                {
                    var _value = CDC_Tool_INI.IniReadBool("CDC_DataType", item.Value, false);
                    var _val = EnumTool.GetValueFromDescription<enCDC_Data>(item.Value);
                    if (_value)
                        StaticVar.CDC_DataType |= _val;

                    //CbTreeView_ShowHide.Nodes[item.Key].Checked = _value;
                    //LastTreeNodeCollection[item.Key].Checked = _value;
                }
            }
            else
            {
                StaticVar.CDC_DataType = enCDC_Data.None;
                foreach (var item in EnumTool.EnumToList<enCDC_Data>())
                    StaticVar.CDC_DataType |= item;
            }
            #endregion CDC_DataType Hide or Show
        }

        private void ShowShortData(List<double> _data, string name)
        {
#if false
            StaticVar.LogWrite("<<<{0}>>>\r\n", name);
            for (int i = 0; i < _data.Count(); i++)
            {
                StaticVar.LogWrite("X_{0:D3} ", i+1);
            }
            StaticVar.LogWrite("\r\n");
            for (int i = 0; i < _data.Count(); i++)
            {
                StaticVar.LogWrite("{0} ", _data[i]);
            }
            StaticVar.LogWrite("\r\n");
#endif
        }

        private void Update_ShortPage()
        {
            var item = (enShortDataFormat)m_IliFunc.LocalVariable._CDCTool_VAR.CDCDataType;
            List<double> X_DIFF_Array = new List<double>();
            List<double> Y_DIFF_Array = new List<double>();
            m_gc._CDCTool_.AssignDiffArray(ref X_DIFF_Array, ref Y_DIFF_Array);

            List<double> X_SLK_Array = m_gc._CDCTool_.AssignShortArray(m_CDC_Tool_INI, "ShortTest", SHORT_NAME.X_SLK, item);
            List<double> Y_SLK_Array = m_gc._CDCTool_.AssignShortArray(m_CDC_Tool_INI, "ShortTest", SHORT_NAME.Y_SLK, item);
            List<double> X_LK_Array = m_gc._CDCTool_.AssignShortArray(m_CDC_Tool_INI, "ShortTest", SHORT_NAME.X_LK, item);
            List<double> Y_LK_Array = m_gc._CDCTool_.AssignShortArray(m_CDC_Tool_INI, "ShortTest", SHORT_NAME.Y_LK, item);

            bool bShow_LK_Data = (X_LK_Array.Count != 0) || (Y_LK_Array.Count != 0);

            if (bShow_LK_Data)
            {
                X_DIFF_Array.Insert(0, X_DIFF_Array[0]);
                ShowShortData(X_DIFF_Array, "X_DIFF");

                Y_DIFF_Array.Insert(0, Y_DIFF_Array[0]);
                ShowShortData(Y_DIFF_Array, "Y_DIFF");

                Y_LK_Array.Insert(0, Y_LK_Array[0]);
                ShowShortData(Y_LK_Array,"Y_LK");

                X_LK_Array.Insert(0, X_LK_Array[0]);
                ShowShortData(X_LK_Array,"X_LK");
            }

            X_SLK_Array.Insert(0, X_SLK_Array[0]);
            ShowShortData(X_SLK_Array, "X_SLK");

            Y_SLK_Array.Insert(0, Y_SLK_Array[0]);
            ShowShortData(Y_SLK_Array,"Y_SLK");

            //第0位置故意填一個重複值，讓UI可以保持曲線。
            if (StaticVar.bCDC_Record && StaticVar.bCDC_Start2Record)
                m_gc._CDCTool_.RecordCDC_Short(m_var.VarCDCTool_Multi.Dic_ShortValue.Values.ToList());
        }

        private void TChart_AddMutual()
        {
#if  false 
            StaticVar.LogWrite("         ");
            for (int X = 0; X < m_var.Var_Multi.DevPanelInfo.iAA_X; X++)
                StaticVar.LogWrite(string.Format("X_{0:D3} ", X+1));
            StaticVar.LogWrite("\r\n");
#endif
            double douValue = 0;
            
            for (int iY = m_var.Var_Multi.DevPanelInfo.iAA_Y-1; iY >=0 ; iY--)
            {
                for (int iX = 0; iX < m_var.Var_Multi.DevPanelInfo.iAA_X; iX++)
                {
                    m_gc._CDCTool_.GetNowCDCValue(iX, iY, out douValue);
                    //if (iX == 0)
                    //    StaticVar.LogWrite(string.Format("Y_{0:D3} ", iY+1));
                    //StaticVar.LogWrite("{0,5} ", douValue);
                }
                //StaticVar.LogWrite("\r\n");
            }
            //StaticVar.LogWrite("\r\n");
        }

        private void TChart_AddSelf()
        {
            List<double> ls = new List<double>();
            foreach (var item in EnumTool.EnumToList<enCDCSMStyle>())
            {
                switch (item)
                {
                    case enCDCSMStyle.Self_X: 
                        StaticVar.LogWrite("<<<Self X>>>\r\n");
                        break;
                    case enCDCSMStyle.Self_Y:
                        StaticVar.LogWrite("<<<Self Y>>>\r\n");
                        break;
                    default:
                        continue;
                }
                m_gc._CDCTool_.GetNowSelfValue(item, ref ls);
#if false
                for (int X = 0; X < ls.Count(); X++)
                    StaticVar.LogWrite(string.Format("X_{0:D3} ", X+1));
                StaticVar.LogWrite("\r\n");
                for (int i = 0; i < ls.Count(); i++)
                    StaticVar.LogWrite("{0,5} ", ls[i]);
                StaticVar.LogWrite("\r\n");
#endif
            }
        }

        private void TChart_AddKey(enCDCSMStyle _CDC_Type, int width)
        {
            double douValue = 0;
            bool bHideColor = false;
            StaticVar.LogWrite("<<<Key>>>\r\n");
            for (int iKey = 0; iKey < width; iKey++)
            {
                if (iKey >= m_var.Var_Multi.DevPanelInfo.iKeyAmounts)
                    bHideColor = true;

                if (bHideColor == false)
                {
                    m_gc._CDCTool_.GetNowKeyValue(_CDC_Type, iKey, ref douValue);

                    if (_CDC_Type == enCDCSMStyle.Mutual)
                        StaticVar.LogWrite(string.Format("Mutual [{0}]:{1:D3}\r\n ", iKey + 1, (Int16)douValue));

                    if (_CDC_Type == enCDCSMStyle.Self_X)
                        StaticVar.LogWrite(string.Format("Self X [{0}]:{1:D3}\r\n", iKey + 1, (Int16)douValue));

                    if (_CDC_Type == enCDCSMStyle.Self_Y)
                        StaticVar.LogWrite(string.Format("Self Y [{0}]:{1:D3}\r\n", iKey + 1, (Int16)douValue));
                }
            }
        }

        private void TChart_AddPen_SE()
        {
            Int16 _uValue = 0;
            if (m_var.VarCDCTool_Multi.Orig_PenData.ContainsKey(enPenCDC._X))
            {
                var _DataCnts = (int)Pen_CDC_Format.TEST_MODE_PEN_X.TipRing_X;
                var _DataLength = m_var.VarCDCTool_Multi.Orig_PenData[enPenCDC._X].Count / _DataCnts;
#if false
                StaticVar.LogWrite("         ");
                for (int X = 0; X < _DataLength; X++)
                    StaticVar.LogWrite(string.Format("X_{0:D3} ", X));
#endif
                for (int iY = 0; iY < _DataCnts; iY++)
                {
                    for (int iX = 0; iX < _DataLength; iX++)
                    {
                        m_gc._CDCTool_.GetNowPenValue(enPenCDC._X, (iY * _DataLength + iX), ref _uValue);
                        //if (iX == 0)
                        //    StaticVar.LogWrite(string.Format("Y_{0:D3} ", iY));
                        //StaticVar.LogWrite(string.Format("{0:D3} ", _uValue));
                    }
                    //StaticVar.LogWrite("\r\n");
                }
            }

            if (m_var.VarCDCTool_Multi.Orig_PenData.ContainsKey(enPenCDC._Y))
            {
                var _DataCnts = (int)Pen_CDC_Format.TEST_MODE_PEN_Y.TipRing_Y;
                var _DataLength = m_var.VarCDCTool_Multi.Orig_PenData[enPenCDC._Y].Count / _DataCnts;
                for (int iX = 0; iX < _DataCnts; iX++)
                {
                    for (int iY = 0; iY < _DataLength; iY++)
                    {
                        m_gc._CDCTool_.GetNowPenValue(enPenCDC._Y, (iX * _DataLength + iY), ref _uValue);
                        //if (iY == 0)
                        //    StaticVar.LogWrite(string.Format("X_{0:D3} ", iX));
                        //StaticVar.LogWrite(string.Format("{0:D3} ", _uValue));
                    }
                    //StaticVar.LogWrite("\r\n");
                }
            }

        }

        private void Update_MutualPage()
        {
            //Mutual
            var Mask = enCDCSMStyle.Mutual;
            if ((m_var.VarCDCTool_Multi.CDC_Data_Style & Mask) != enCDCSMStyle.None)
            {
                TChart_AddMutual();
            }
            //Self
            Mask = enCDCSMStyle.Self_Y | enCDCSMStyle.Self_X;
            if ((m_var.VarCDCTool_Multi.CDC_Data_Style & Mask) != enCDCSMStyle.None)
            {
                TChart_AddSelf();
            }
            //Key
            Mask = enCDCSMStyle.Key;
            if ((m_var.VarCDCTool_Multi.CDC_Data_Style & Mask) != enCDCSMStyle.None)
            {
                if (m_var.Var_Multi.DevPanelInfo.g_KeyInfo != null)
                {
                    var Key_Mode = m_var.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Mode;
                    if (m_var.VarCDCTool_Multi.IsHW_Key(Key_Mode))
                    {

                        TChart_AddKey(enCDCSMStyle.Mutual, m_var.Var_Multi.DevPanelInfo.iKeyAmounts);

                        if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                        {
                            TChart_AddKey(enCDCSMStyle.Self_X, m_var.Var_Multi.DevPanelInfo.iKeyAmounts);
                            TChart_AddKey(enCDCSMStyle.Self_Y, 1);
                        }
                    }
                }
            }
            //================================
            //Pen SE
            //================================
            if (m_var.Var_Multi.DevPanelInfo.SupportPenType != Pen_CDC_Format.enPenType.Disable)
            {
                Mask = enCDCSMStyle.Pen;
                if ((m_var.VarCDCTool_Multi.CDC_Data_Style & Mask) != enCDCSMStyle.None)
                {
                    TChart_AddPen_SE();
                }
            }
            //Record
            if (StaticVar.bCDC_Record && StaticVar.bCDC_Start2Record)
                m_gc._CDCTool_.RecordCDC_RAW(m_var.VarCDCTool_Multi.MutualData, int.MinValue, int.MaxValue);

        }

        private void tabPage_Pen_ResetChart()
        {
            
            //目前X最大數量的是TEST_MODE_PEN_USI_X.ACK
            Match rxMatch = Regex.Match(EnumTool.GetEnumDescription(Pen_CDC_Format.TEST_MODE_PEN_USI_X.ACK), @"Size:(.*)");
            int iRange = int.Parse(rxMatch.Groups[1].ToString());
            m_Max_PenX_Bottom = m_var.Var_Multi.DevPanelInfo.iAA_X;
            m_Max_PenX_Left = iRange;
            //目前Y最大數量的是TEST_MODE_PEN_MPP_Y.DigitalHF_TipRing_Y
            rxMatch = Regex.Match(EnumTool.GetEnumDescription(Pen_CDC_Format.TEST_MODE_PEN_MPP_Y.DigitalHF_TipRing_Y), @"Size:(.*)");
            iRange = int.Parse(rxMatch.Groups[1].ToString());
            m_Max_PenY_Bottom = iRange;
            m_Max_PenY_Left = m_var.Var_Multi.DevPanelInfo.iAA_Y;
        }

        private bool AddcolorGrid_PenX(object _InSelect, Int16[] Data)
        {
            string Chart_PenX_Text = "";
            m_gc._CDCTool_.AddcolorGrid_PenX_Core(_InSelect, Data, m_Max_PenX_Left, m_Max_PenX_Bottom, out Chart_PenX_Text);
#if false
            StaticVar.LogWrite("{0}\r\n", Chart_PenX_Text);

            StaticVar.LogWrite("         ");
            for (int X = 0; X < m_Max_PenX_Bottom; X++)
                StaticVar.LogWrite(string.Format("X_{0:D3} ", X+1));
            StaticVar.LogWrite("\r\n");
            
            for (int iY = m_Max_PenX_Left-1; iY >= 0; iY--)
            {
                for (int iX = 0; iX < m_Max_PenX_Bottom; iX++)
                {
                    if (iX == 0)
                        StaticVar.LogWrite(string.Format("Y_{0:D3} ", iY+1));
                    StaticVar.LogWrite("{0,5} ", m_gc._CDCTool_.m_pen_val[iY * m_Max_PenX_Bottom + iX]);
                }
                StaticVar.LogWrite("\r\n");
            }
            StaticVar.LogWrite("\r\n");
#endif
            //colorGrid_PenX.Add(Program.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.m_pen_X.ToArray(),
            //    Program.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.m_pen_val.ToArray(),
            //    Program.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.m_pen_Y.ToArray());
            //Console.WriteLine("Max {0} ,Min {1};colorGrid_PenX (Max:{2} Min:{3})", iMax, iMin, colorGrid_PenX.MaxYValue(), colorGrid_PenX.MinYValue());
            return true;
        }

        private bool AddcolorGrid_PenY(object _InSelect, Int16[] Data)
        {
            Type _MyType = _InSelect.GetType();

            string Chart_PenY_Text = "";
            m_gc._CDCTool_.AddcolorGrid_PenY_Core(_InSelect, Data, m_Max_PenY_Left, m_Max_PenY_Bottom, out Chart_PenY_Text);
#if false
            StaticVar.LogWrite("{0}\r\n", Chart_PenY_Text);

            StaticVar.LogWrite("         ");
            for (int X = 0; X < m_Max_PenY_Bottom; X++)
                StaticVar.LogWrite(string.Format("X_{0:D3} ", X+1));
            StaticVar.LogWrite("\r\n");

            for (int iY = m_Max_PenY_Left - 1; iY >= 0; iY--)
            {
                for (int iX = 0; iX < m_Max_PenY_Bottom; iX++)
                {
                    if (iX == 0)
                        StaticVar.LogWrite(string.Format("Y_{0:D3} ", iY+1));
                    StaticVar.LogWrite("{0,5} ", m_gc._CDCTool_.m_pen_val[iX * m_Max_PenY_Left + iY]);
                }
                StaticVar.LogWrite("\r\n");
            }
            StaticVar.LogWrite("\r\n");
#endif
            return true;
        }

        private void TChart_AddPen(V6_CDC_MODE_Define.CDC_Type_Mutual CDC_Type, object DataType)
        {
            bool hide=false;
            switch (CDC_Type)
            {
                case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_USI:
                    if (!DataType.GetType().Equals(typeof(Pen_CDC_Format.TEST_MODE_PEN_USI)))
                        return;
                    var _PEN_USI = (Pen_CDC_Format.TEST_MODE_PEN_USI)DataType;
                    hide = EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_USI_X>(_PEN_USI.ToString()) ? false : true;
                    if (!hide)
                    {
                        var Data = m_var.VarCDCTool_Multi.USI_PenData[_PEN_USI][(int)enPenCDC._X];
                        var _Select = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_USI_X>(_PEN_USI.ToString());
                        if (!AddcolorGrid_PenX(_Select, Data))
                            return;
                    }

                    hide = EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_USI_Y>(_PEN_USI.ToString()) ? false : true;
                    if (!hide)
                    {
                        var Data = m_var.VarCDCTool_Multi.USI_PenData[_PEN_USI][(int)enPenCDC._Y];
                        var _Select = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_USI_Y>(_PEN_USI.ToString());
                        if (!AddcolorGrid_PenY(_Select, Data))
                            return;
                    }
                    break;

                case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_WGP:
                    if (!DataType.GetType().Equals(typeof(Pen_CDC_Format.TEST_MODE_PEN_WGP)))
                        return;
                    var _PEN_WGP = (Pen_CDC_Format.TEST_MODE_PEN_WGP)DataType;
                    hide = EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_WGP_X>(_PEN_WGP.ToString()) ? false : true;
                    if (!hide)
                    {
                        var Data = m_var.VarCDCTool_Multi.WGP_PenData[_PEN_WGP][(int)enPenCDC._X];
                        var _Select = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_WGP_X>(_PEN_WGP.ToString());
                        if (!AddcolorGrid_PenX(_Select, Data))
                            return;
                    }

                    hide = EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_WGP_Y>(_PEN_WGP.ToString()) ? false : true;
                    if (!hide)
                    {
                        var Data = m_var.VarCDCTool_Multi.WGP_PenData[_PEN_WGP][(int)enPenCDC._Y];
                        var _Select = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_WGP_Y>(_PEN_WGP.ToString());
                        if (!AddcolorGrid_PenY(_Select, Data))
                            return;
                    }
                    break;

                case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_MPP:
                    if (!DataType.GetType().Equals(typeof(Pen_CDC_Format.TEST_MODE_PEN_MPP)))
                        return;
                    var _PEN_MPP = (Pen_CDC_Format.TEST_MODE_PEN_MPP)DataType;
                    hide = EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_MPP_X>(_PEN_MPP.ToString()) ? false : true;
                    if (!hide)
                    {
                        var Data = m_var.VarCDCTool_Multi.MPP_PenData[_PEN_MPP][(int)enPenCDC._X];
                        var _Select = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_MPP_X>(_PEN_MPP.ToString());
                        if (!AddcolorGrid_PenX(_Select, Data))
                            return;
                    }

                    hide = EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_MPP_Y>(_PEN_MPP.ToString()) ? false : true;
                    if (!hide)
                    {
                        var Data = m_var.VarCDCTool_Multi.MPP_PenData[_PEN_MPP][(int)enPenCDC._Y];
                        var _Select = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_MPP_Y>(_PEN_MPP.ToString());
                        if (!AddcolorGrid_PenY(_Select, Data))
                            return;
                    }
                    break;
            }

        }

        public void ShowAllNodeColorGrid()
        {
            //if (bOptionChanged || !m_var.VarCDCTool_Multi.g_bRunningStatus)
            //    return;

            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V3)
            {
                var _V3_select = m_var.VarCDCTool_Multi.V3_NowSelect_CDC;
                if (_V3_select == null)
                    return;

                switch (_V3_select.Mutual_CDC_Name)
                {
                    case V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT:
                        Update_ShortPage();
                        break;

                    default:
                        Update_MutualPage();
                        break;
                }
            }
            else if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                var _V6_select = m_var.VarCDCTool_Multi.V6_NowSelect_CDC;
                if (_V6_select == null)
                    return;

                switch (_V6_select.Mutual_CDC_Name)
                {
                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT:
                        Update_ShortPage();
                        break;

                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_USI:
                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_MPP:
                    case V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_WGP:
                        //TChart_ColorReset_Pen(_V6_select.Mutual_CDC_Name);
                        TChart_AddPen(_V6_select.Mutual_CDC_Name, m_var.VarCDCTool_Multi.PenMode_SubName);
                        if (StaticVar.bCDC_Record && StaticVar.bCDC_Start2Record)
                            m_gc._CDCTool_.RecordPen_RAW(_V6_select.Mutual_CDC_Name);
                        break;

                    default:
                       
                        Update_MutualPage();
                        
                        break;
                }
            }
        }

        private bool CDC_Thread(ref string strErrCode, int frame_cnt) 
        {
            bool bResult = false;
            bool IsNeedLimitAnalyze = true;
            int cnt = frame_cnt;
            int idx = 0;
            m_var.VarCDCTool_Multi.bRunningStatus = true;
            StaticVar.NowExecuteTool = enToolName.CDCTool;
            try
            {
                tabPage_Pen_ResetChart();
                //這一段是為了配合流程而加的，但會導致I2C速度變慢，但為了配合流程不行拿掉。
                if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                    if (StaticVar.lsMain_USBDevInfo[0].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                        m_gc._CDCTool_.OpenBridge_INT_Status();//0:INT Event為Low觸動; 1:INT Event為High觸動

                m_var.VarCDCTool_Multi.cdc_waitHandle = new ManualResetEvent(false);
                m_var.VarCDCTool_Multi.cdc_waitHandle.Reset();
                do
                {
                    strErrCode = "";
                    StaticVar.LogWrite("\r\nFrame[{0}]\r\n", idx++);
                    bResult = m_gc._CDCTool_.CDC_MainEntry_Local(ref strErrCode, IsNeedLimitAnalyze, true);
                    //print data on screen
                    ShowAllNodeColorGrid();
                    if(StaticVar.bCDC_Start2Record)
                        --cnt;
                } while (bResult && cnt>0);
                //
                m_var.VarCDCTool_Multi.cdc_waitHandle.Set();
                m_var.VarCDCTool_Multi.bRunningStatus = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                strErrCode += ex;
            }
            return bResult;
        }

        private bool OnCDCStart(ref string strErrCode, int frame_cnt, bool is_mutual)
        {
            //CDC_Toolfm_Init(false)
            //
            if (m_gc._CDCTool_ != null)
            {
                m_gc._CDCTool_.CDCTool_Init(CDC_Tool_INI, true, null, false);//開啟Test Mode,INT Monitor 
                TouchMode mode = TouchMode.NormalMode;
                m_gc.m_Flow.GetTouchMode(ref mode);
                if (mode != TouchMode.TestMode)
                    m_gc._CDCTool_.CDCTool_Init(CDC_Tool_INI, true, null, false);//開啟Test Mode,INT Monitor 
            }

            //TChart_Init(false, is_mutual);
            //CDC_Toolfm_Init(false) end
            return CDC_Thread(ref strErrCode, frame_cnt);
        }

        private void vfRecordCDC()
        {
            if (StaticVar.bCDC_Record)
            {
                // Stop Record
                StaticVar.bCDC_Record = false;
                StaticVar.bCDC_Start2Record = false;
                m_checktime.Stop();
            }
            else
            {
                // Start Record
                m_gc._CDCTool_.m_iFileCount = 0;
                m_gc._CDCTool_.m_iRecordFrameCounts = 1;
                string FILE_NAME = ITS_Directory.Tool_CurrentDirectory + "\\Record_File";
                if (!Directory.Exists(FILE_NAME))
                    Directory.CreateDirectory(FILE_NAME);
                m_gc._CDCTool_.RecordFile_Header(GetProductVersion());
                StaticVar.bCDC_Record = true;
            }
        }

        private bool pen_params(string type, ref cdc _cdc)
        {
            bool ret = true;
            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V3)
            {
                Console.WriteLine("Error: V3 protocol didn't support {0}\r\n", type);
                return false;
            }
            switch (type)
            {
                case "mpp_beaconlf":
                    _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_MPP;
                        m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_MPP.BeaconLF;
                    break;
                case "mpp_digitallf":
                    _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_MPP;
                    m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_MPP.DigitalLF;
                    break;
                case "mpp_porttypehf":
                    _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_MPP;
                    m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_MPP.PortTypeHF;
                    break;
                case "mpp_digitalhf":
                   _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_MPP;
                   m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_MPP.DigitalHF;
                    break;
                case "mpp_presure":
                    _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_MPP;
                    m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_MPP.Presure;
                    break;
                case "mpp_beaconhf":
                     _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_MPP;
                     m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_MPP.BeaconHF;
                    break;
                case "usi_ack":
                     _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_USI;
                     m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_USI.ACK;
                        break;
                case "usi_tip_0":
                        _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_USI;
                        m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_USI.Tip_0;
                        break;
                case "usi_tip_1":
                        _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_USI;
                        m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_USI.Tip_1;
                        break;
                case "usi_tip_2":
                        _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_USI;
                        m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_USI.Tip_2;
                        break;
                case "usi_st":
                        _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_USI;
                        m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_USI.ST;
                        break;
                case "wgp_tipring0":
                        _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_WGP;
                        m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_WGP.TipRing0;
                        break;
                case "wgp_tipring1":
                        _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_WGP;
                        m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_WGP.TipRing1;
                        break;
                case "wgp_tipring2":
                        _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_WGP;
                        m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_WGP.TipRing2;
                        break;
                case "wgp_tipring3":
                        _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_WGP;
                        m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_WGP.TipRing3;
                        break;
                case "wgp_st":
                        _cdc.type = V6_CDC_MODE_Define.CDC_Type_Pen.TEST_MODE_PEN_WGP;
                        m_var.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_WGP.ST;
                        break;
                default:
                Console.WriteLine("Error: Unknown type: {0}\r\n", type);
                ret = false;
                    break;
            }
            if(ret)
            {
                string[] _type = type.Split('_');
                ret = Check_PenType(_type[0]);
            }
            return ret;
        }

        public bool parseCDC_Chart(string item_new)
        {
            string[] words = item_new.Split('=');
            bool ret = false;
            if (words.Count() == 2)
            {
                words = words[1].ToLower().Split(',');
                foreach(string w in words)
                {
                    if (w == "mutual")
                    {
                        StaticVar.CDC_DataType |= enCDC_Data.MutualData;
                        m_var.VarCDCTool_Multi.CDC_Data_Style |= enCDCSMStyle.Mutual;
                    }
                    else if (w == "self")
                    {
                        StaticVar.CDC_DataType |= enCDC_Data.SelfData;
                        m_var.VarCDCTool_Multi.CDC_Data_Style |= enCDCSMStyle.Self_X;
                        m_var.VarCDCTool_Multi.CDC_Data_Style |= enCDCSMStyle.Self_Y;
                    }
                    else if (w == "key")
                    {
                        StaticVar.CDC_DataType |= enCDC_Data.KeyData;
                        m_var.VarCDCTool_Multi.CDC_Data_Style |= enCDCSMStyle.Key;
                    }
                    else if (w == "pen")
                    {
                        StaticVar.CDC_DataType |= enCDC_Data.PenData;
                        m_var.VarCDCTool_Multi.CDC_Data_Style |= enCDCSMStyle.Pen;
                    }
                    else
                        StaticVar.CDC_DataType |= enCDC_Data.None;
                }
                ret = true;

            }
            return ret;
        }

        public bool parseCDC_Params(string item_new, ref cdc _cdc)
        {
            string[] words = item_new.Split('=');
            _cdc.enable = true;
            _cdc.frame_cnt = -1;
            _cdc.format = enCDCDataFormat.inOriginal;
            _cdc.b_record = false;
            bool ret = true;
            if (words.Count() == 2)
            {
                words = words[1].Split(' ');
                if (words.Count() > 1)
                    _cdc.frame_cnt = Convert.ToUInt16(words[0]);
                if (words.Count() >= 2)
                {
                    switch(words[1])
                    {
                        case "raw_bk":
                            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                                _cdc.type = V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_RAW_BK;
                            else
                                _cdc.type = V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_RAW_BK;
                            break;
                        case "raw_nbk":
                             if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                            _cdc.type = V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_RAW_NBK;
                        else
                            _cdc.type = V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_ALL_NODE_NBK;
                            break;
                        case "bg_bk":
                             if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                            _cdc.type = V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_BG_BK;
                        else
                            _cdc.type = V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_BG;
                            break;
                        case "se_bk":
                            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                                _cdc.type = V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SE_BK;
                            else
                                _cdc.type = V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SE;
                            break;
                        case"dac_p":
                            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                                _cdc.type = V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_DAC_P;
                            else
                                _cdc.type = V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_DAC_P;
                                break;
                        case "dac_n":
                            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                                _cdc.type = V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_DAC_N;
                            else
                                _cdc.type = V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_DAC_N;
                            break;
                        case "open":
                            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                                _cdc.type = V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_OPEN;
                            else
                                _cdc.type = V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_OPEN;
                                break;
                        case "short":
                            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                                _cdc.type = V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT;
                            else
                                _cdc.type = V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT;
                            break;
                            //2901
                        case "mpp_beaconlf":
                        case "mpp_digitallf":
                        case "mpp_porttypehf":
                        case "mpp_digitalhf":
                        case "mpp_presure":
                        case "mpp_beaconhf":
                        case "usi_ack":
                        case "usi_tip_0":
                        case "usi_tip_1":
                        case "usi_tip_2":
                        case "usi_st":
                        case "wgp_tipring0":
                        case "wgp_tipring1":
                        case "wgp_tipring2":
                        case "wgp_tipring3":
                        case "wgp_st":
                            ret = pen_params(words[1], ref _cdc);
                            break;
                        default:
                            Console.WriteLine("Error: Cmd is not supported {0}\r\n", words[1]);
                            ret = false;
                            break;
                    }
                }
                if (words.Count() >= 3)
                {
                    if (words[2].Contains("ori"))
                        _cdc.format = enCDCDataFormat.inOriginal;
                    else if (words[2].Contains("max"))
                        _cdc.format = enCDCDataFormat.inMax;
                    else if (words[2].Contains("min"))
                        _cdc.format = enCDCDataFormat.inMin;
                    else if (words[2].Contains("delta"))
                        _cdc.format = enCDCDataFormat.iDelta;
                    else if (words[2].Contains("signal"))
                        _cdc.format = enCDCDataFormat.iSignal;
                    else
                    {
                        Console.WriteLine("Error: Cmd is not supported {0}\r\n", item_new);
                        ret = false;
                    }
                }
                if (words.Count() >= 4)
                {
                    if (words[3].Contains("r"))
                        _cdc.b_record = true;
                }
            }
            return ret;
        }

        public void DetectPen(PaintTool_Var.enum_Pen_Status wait_type, ref bool getpen)
        {
            do
            {
                PaintTool_Var.enum_Pen_Status type = m_gc.m_Flow.Get_TouchHover_Type();
                if (type != PaintTool_Var.enum_Pen_Status.NA)
                {
                    if (type == wait_type)
                    {
                        getpen = true;
                        StaticVar.LogWrite("Info: get a pen.\r\n");
                        break;
                    }
                }
                Thread.Sleep(1);
            } while (true);
        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            StaticVar.bCDC_Start2Record = true;
        }

        private void RunTimer(int delay)
        {
            if(delay==0)
            {
                StaticVar.bCDC_Start2Record = true;
                return;
            }
            // Create a timer and set a two second interval.
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Interval = (double)delay;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = false;

            // Start the timer
            aTimer.Enabled = true;
        }
        public ErrorCodes DoCDC(cdc _cdc, int delay)
        {
            if (!_cdc.enable)
                return ErrorCodes.ITS_ERR_NO_NG;
            //
            bool getpen=false;
            if (_cdc.i_waittype != PaintTool_Var.enum_Pen_Status.NA && _cdc.b_record)
            {
                m_var.VarPaint_Multi.stuPen_Old[0] = new MultiPaintTool_VAR.PenStatus();
                StaticVar.LogWrite("\n**********Getting started with the stylus within " +  _cdc.i_waittime.ToString() + " seconds**********\r\n");
                var task = Task.Factory.StartNew(() => DetectPen(_cdc.i_waittype, ref getpen));
                task.Wait(_cdc.i_waittime*1000);
                if (!getpen)
                {
                    StaticVar.LogWrite("Error: No pens were detected.\r\n");
                    return ErrorCodes.ITS_ERR_OTHERS_NG;
                }
            }
            //
            string strErrCode = "";
            SetDefault_CDC_Type();
            m_var.VarThread_Multi.Data_Decode_Type.DECODE_TYPE = enDECODE_TYPE.InterruptIn_Other;
            string type = "";
            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                type = EnumTool.GetEnumDescription((CommonExt.V6_CDC_MODE_Define.CDC_Type_Mutual)_cdc.type);
            else
                type = EnumTool.GetEnumDescription((CommonExt.V3_CDC_MODE_Define.CDC_Type_Mutual)_cdc.type);
            
            m_gc._CDCTool_.NowSelect_CDC(type);
            m_var._CDCTool_VAR.CDCDataType = _cdc.format;
            //CDCTool_Init();
            if (_cdc.b_record)
                vfRecordCDC();
            bool is_mutual = true;
            if (type.ToUpper().Contains("short") || type.ToLower().Contains("SHORT") || type.ToLower().Contains("WGP") || type.ToLower().Contains("USI") || type.ToLower().Contains("MPP"))
                is_mutual = false;
            RunTimer(delay);
            if (!OnCDCStart(ref strErrCode, _cdc.frame_cnt, is_mutual))
            {
                StaticVar.LogWrite("Error: Get the cdc data was a failed\r\n");
                return ErrorCodes.ITS_ERR_OTHERS_NG;
            }
            
            if (!m_gc._CDCTool_.CDC_Stop())
            {
                StaticVar.LogWrite("Error: Failed to stop getting the cdc data.\r\n");
                return ErrorCodes.ITS_ERR_OTHERS_NG;
            }
            if (_cdc.b_record)
                vfRecordCDC();
            return ErrorCodes.ITS_ERR_NO_NG;
        }
    }

    internal partial class ConsoleFunsClass
    {//main
        private bool b_alreadyInit = false;
        public bool m_b_isDone = false;
        private CtrlILIDevice m_IliFunc;
        private NotStaticVar m_var;
        private GBV.GlobeClass m_gc;
        public Dictionary<int, string> m_ErrorCode = new Dictionary<int, string>();
        private INI_Tool CDC_Tool_INI;

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

        public string GetAppVersion(CustomID vendor)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string ToolVersion = fvi.ProductVersion;
            DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

            var custom = EnumTool.GetEnumDescription(vendor);
            StaticVar.ITS_ToolVer_Customer = String.Format("{0}.{1}.{2}", custom, "0", "0");
#if (RELEASE)
            if (custom == vendor.ToString() || (vendor == CustomID.Ilitek))
                return string.Format("_{0}_RELEASE", ToolVersion);
            else
                return string.Format("_{0}_RELEASE [V{1}.{2}.{3}]", ToolVersion, custom, "0", "0");
#else
            if (custom == vendor.ToString() || (vendor == CustomID.Ilitek))
                return string.Format("_{0}_Beta_{1}", ToolVersion, buildDate);
            else
                return string.Format("_{0}_Beta_[V{1}.{2}.{3}]_{4}", ToolVersion, custom, "0", "0", buildDate);
#endif
        }

        public string GetProductVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.ProductVersion.ToString();
        }

        public ConsoleFunsClass(CtrlILIDevice iliFunc)
        {
            m_IliFunc = iliFunc;
            m_var = m_IliFunc.LocalVariable;
            m_gc = m_IliFunc.m_GBV.Globe_Multi;
            m_ls_PenSingal.Add(new NoiseFrqVariable.PenSingalFormat { sFreq = "10", sValue = "10" });
            m_ls_PenSingal.Add(new NoiseFrqVariable.PenSingalFormat { sFreq = "300", sValue = "10" });
            m_PenParams.ls_PenSignal = m_ls_PenSingal;

            CDC_Tool_INI = new INI_Tool(ITS_Directory.CDCTool_INI);
        }

        public int Action(object m_args, console_ctl console_ctl)
        {
            ErrorCodes ret = ErrorCodes.ITS_ERR_NO_NG;
            do
            {

                m_gc.m_Flow.Set_Received0x0C_Flag(false);
                if (SystemInit() == false)
                {
                    ret = ErrorCodes.ITS_ERR_INITAIL_DEVICE_NG;
                    break;
                }
                m_gc.m_Flow.Set_Received0x0C_Flag(true);
                StaticVar.LogWrite("\r\n");
                upgrade_info _upgrade = (upgrade_info)console_ctl.upgrade;
                cdc _cdc = new cdc();
                _cdc.enable = false;
                _cdc.i_waittype = PaintTool_Var.enum_Pen_Status.NA;
                bool bNoiseFrq_en = false, bPenSetting=false;
                foreach (var item in (string[])m_args)
                {
                    string item_new = item.ToLower();
                    if (item_new.Contains("/i"))
                    {
                        StaticVar.LogWrite("CMD: Show IC Information\r\n");
                        if (!ShowICInfo())
                        {
                            ret = ErrorCodes.ITS_ERR_SHOW_IC_INFORMAITON_NG;
                            break;
                        }
                    }
                    else if (item_new.Contains("/chkic="))
                    {
                        StaticVar.LogWrite("CMD: Check IC\r\n");
                        string[] words = item_new.Split('=');
                        if (words.Count() < 2 || words[1] == "")
                        {
                            ret = ErrorCodes.ITS_ERR_CMD_EMPTY_NG;
                            break;
                        }
                        int chk_mcu = Convert.ToInt32(words[1], 16);
                        int get_mcu = 0;
                        if (!CheckMCUVer(chk_mcu, ref get_mcu))
                        {
                            ret = ErrorCodes.ITS_ERR_Do_CHECK_IC_NG;
                            break;
                        }
                    }
                    else if (item_new.Contains("/cmd="))
                    {
                        string[] words = item_new.Split('=');
                        char[] charsToTrim = { '/', 'c', 'm', 'd', '=' };
                        string str_cmd = item_new.Trim(charsToTrim);
                        StaticVar.LogWrite("CMD: {0}\r\n", str_cmd);
                        if (!CmdCtrl(str_cmd))
                        {
                            ret = ErrorCodes.ITS_ERR_SEND_CMD_NG;
                            break;
                        }
                    }
                    else if (item_new.Contains("/setmode="))
                    {
                        StaticVar.LogWrite("CMD: Set the tuning mode\r\n");
                        string[] words = item_new.Split('=');
                        if (words.Count() < 2 || words[1] == "")
                        {
                            ret = ErrorCodes.ITS_ERR_SET_TUNING_MODE_NG;
                            break;
                        }
                        int mode = Convert.ToUInt16(words[1]);
                        if (mode >= 0 && mode <= 2)
                        {
                            if (!SetSwitchMode((byte)mode))
                            {
                                ret = ErrorCodes.ITS_ERR_DO_SET_TUNING_MODE_NG;
                                break;
                            }
                        }
                        else
                        {
                            StaticVar.LogWrite(string.Format("Error: The setting number({0}) is not supported\n", mode));
                            ret = ErrorCodes.ITS_ERR_SET_TUNING_MODE_NG;
                            break;
                        }
                    }
                    else if (item_new.Contains("/cdc="))
                    {
                        StaticVar.LogWrite("CMD: Get the cdc data\r\n");
                        if (!parseCDC_Params(item_new, ref _cdc))
                        {
                            ret = ErrorCodes.ITS_ERR_PARSE_CDC_CMD_NG;
                            break;
                        }
                    }
                    else if (item_new.Contains("/chart="))
                    {
                        StaticVar.LogWrite("CMD: Set the cdc chart\r\n");
                        if (!parseCDC_Chart(item_new))
                        {
                            ret = ErrorCodes.ITS_ERR_PARSE_CDC_CHARTTYPE_NG;
                            break;
                        }
                    }
                    else if (item_new.Contains("/wait_hover"))
                    {
                        StaticVar.LogWrite("CMD: Set wait_hover\r\n");
                        string[] words = item_new.Split('=');
                        if (words.Count() == 2)
                            _cdc.i_waittime = Int16.Parse(words[1]);
                        else
                            _cdc.i_waittime = 15;
                        _cdc.i_waittype = PaintTool_Var.enum_Pen_Status.Hover;
                        m_gc.m_Flow.g_StopReadMonitor(100);
                        m_IliFunc.m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.En);
                        m_gc.m_Flow.g_OpenReadMonitor();//開啟USB接收
                        m_gc.m_Flow.PaintTool_StartPainting(enInt_InDataType.Painting);
                    }
                    else if(item_new.Contains("/wait_pen"))
                    {
                        StaticVar.LogWrite("CMD: Set wait_pen\r\n");
                        string[] words = item_new.Split('=');
                        if (words.Count() == 2)
                            _cdc.i_waittime = Int16.Parse(words[1]);
                        else
                            _cdc.i_waittime = 15;
                        _cdc.i_waittype = PaintTool_Var.enum_Pen_Status.Touch;
                        m_gc.m_Flow.g_StopReadMonitor(100);
                        m_IliFunc.m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.En);
                        m_gc.m_Flow.g_OpenReadMonitor();//開啟USB接收
                        m_gc.m_Flow.PaintTool_StartPainting(enInt_InDataType.Painting);
                    }
                    else if (item_new.Contains("/delay="))
                    {
                        StaticVar.LogWrite("CMD: Delay...\r\n");
                        string[] words = item_new.Split('=');
                        if (words.Count() < 2 || words[1] == "")
                        {
                            ret = ErrorCodes.ITS_ERR_CMD_EMPTY_NG;
                            break;
                        }
                        console_ctl.i_delay_time = Convert.ToUInt16(words[1]);
                        if (!_cdc.enable)
                            System.Threading.Thread.Sleep(console_ctl.i_delay_time);
                    }
                    else if(item_new.Contains("/sensortest="))
                    {
                        StaticVar.LogWrite(string.Format("CMD: {0}\r\n",item_new));
                        string[] words = item_new.Split('=');
                        if (words.Count() < 2 || words[1] == "")
                        {
                            ret = ErrorCodes.ITS_ERR_CMD_EMPTY_NG;
                            break;
                        }
                        words[1] = words[1].Trim('\"');
                        string[] cmd = words[1].Split(',');
                        if(cmd.Count()>1)
                            Do_SensorTest(cmd[0],cmd[1]);
                        else
                            Do_SensorTest(cmd[0], "");

                    }
                    else if (item_new == "/getmode")
                    {
                        StaticVar.LogWrite("CMD: Get the tuning mode\r\n");
                        byte now_mode = 0;
                        if (!GetSwitchMode(ref now_mode))
                        {
                            ret = ErrorCodes.ITS_ERR_GET_TUNING_MODE_NG;
                            break;
                        }
                        StaticVar.LogWrite(string.Format("Info : The mode is {0}\n", now_mode));
                    }
                    else if (item_new == "/sw_rst")
                    {
                        StaticVar.LogWrite("CMD: Software reset\r\n");
                        SoftReset();
                    }
                    else if (item_new == "/touch=off")
                    {
                        StaticVar.LogWrite("CMD: Disable the touch mode\r\n");
                        if (!DisableTouchMode())
                        {
                            ret = ErrorCodes.ITS_ERR_DISALBE_TOUCH_MODE_NG;
                            break;
                        }
                    }
                    else if (item_new == "/touch=on")
                    {
                        StaticVar.LogWrite("CMD: Enable the touch mode\r\n");
                        if (!EnableTouchMode())
                        {
                            ret = ErrorCodes.ITS_ERR_ENALBE_TOUCH_MODE_NG;
                            break;
                        }
                    }
                    else
                    {
                        ret = ScanNoiseFrq_cmd(item_new, ref bNoiseFrq_en, ref bPenSetting);
                        if (ret != ErrorCodes.ITS_ERR_NO_NG)
                            break;
                    }
                }//end foreach (var item in (string[])m_args)
                if (ret != ErrorCodes.ITS_ERR_NO_NG)
                    break;
                ret = (ErrorCodes)DoFWUpgrade(_upgrade);
                if (ret != ErrorCodes.ITS_ERR_NO_NG)
                    break;
                if (bNoiseFrq_en)
                {
                    ret = (ErrorCodes)DoNoiseFrq(bNoiseFrq_en, bPenSetting);
                    bNoiseFrq_en = false;
                    if (ret != ErrorCodes.ITS_ERR_NO_NG)
                        break;
                } 
                if (_cdc.enable)
                {
                    ret = (ErrorCodes)DoCDC(_cdc, console_ctl.i_delay_time);
                    _cdc.enable = false;
                    if (ret != ErrorCodes.ITS_ERR_NO_NG)
                        break;
                }
                m_b_isDone = true;
            } while (false);
            if (ret != ErrorCodes.ITS_ERR_NO_NG)
                StaticVar.LogWrite(string.Format("{0}\r\n",Enum.GetName(typeof(ErrorCodes), (ErrorCodes)ret)));

            if (m_IliFunc.LocalVariable.Var_Multi.bUSBMonitor_Opened) //如果USB Monitor有打開再開始執行 20220407 - Steven
            {
                m_IliFunc.m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
                m_IliFunc.m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
            }   
            return (int)ret;
        }

        public bool SystemInit()
        {
            b_alreadyInit = LocalConnection();
            return b_alreadyInit;
        }

        public bool Do_SensorTest(string profile, string barcode)
        {
            if(File.Exists(profile)==false)
            {
                StaticVar.LogWrite(string.Format("The system cannot find the hex file - {0}\r\n", profile));
                return false;
            }
            m_IliFunc.m_GBV._SensorTestTool_.LocalVariable.VarSenTest_Multi.strProfileAbsolutePath = profile;
            m_IliFunc.m_GBV._SensorTestTool_.ResetAllTestContains(enReadFrom.INI);
            m_IliFunc.m_GBV._SensorTestTool_.LoadTestEnable(enReadFrom.INI);
            m_IliFunc.m_GBV._SensorTestTool_.LocalVariable.VarSenTest_Multi.SensorTestMode = enSensorTest.SingleMode;
            if (!m_IliFunc.m_GBV._SensorTestTool_.Start_Test_Init(FunType.Test))
            {
                if (StaticVar._ITSTool_Var == enMode.Console_Mode)
                    StaticVar.LogWrite(string.Format("{0}\r\n", m_IliFunc.m_GBV._SensorTestTool_.LocalVariable.VarSenTest_Multi._GridView_Cell.strTotleReult_ErrCode));
                return false;
            }
            bool Cancel = false;
            if(barcode!="")
                m_IliFunc.m_GBV._SensorTestTool_.LocalVariable.VarSenTest_Multi.strBarCodeText = m_IliFunc.m_GBV._SensorTestTool_.MakeFilenameValid(barcode);
            m_IliFunc.m_GBV._SensorTestTool_.RunTest_Function_Core(FunType.Test, "", ref Cancel);
            m_IliFunc.m_GBV._SensorTestTool_.Finish_Test(FunType.Test, Cancel);
            return true;
        }

        public bool LocalConnection()
        {
            m_IliFunc.m_GBV.ConsoleTool.strInforMessage = "";
            string strMsg = "";
            bool bConnectResult = m_IliFunc.LocalConnectionBase(out strMsg);
            if (StaticVar.lsMain_USBDevInfo.Length <= 0)
                return false;

            if (StaticVar.lsMain_USBDevInfo[0].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
            {
                //string bridge_mess = "";
                //string strMsg = m_IliFunc.m_GBV.Globe_Multi._DeviceMain.ReadBridgeVer_API(ref bridge_mess);
                if (strMsg != "")
                    m_IliFunc.m_GBV.ConsoleTool.strInforMessage += strMsg;
                m_IliFunc.m_GBV.ConsoleTool.strInforMessage += string.Format("Bridge Version : {0}", strMsg);
            }
            
            Reset_ConnectionStatus(0, StaticVar.lsMain_USBDevInfo[0].NowDevStatus);

            //Reset_UI(m_iSelectedDev);

            //UpdateInfoMSG();

            if (bConnectResult)
                m_IliFunc.m_GBV.Globe_Multi.m_Flow.SetTouchMode(TouchMode.NormalMode, Command._SWITCH.Dis);

            //m_GBV.ConsoleTool.strInforMessage = "";
            //bool isRowZero = false;
            //bool bConnectResult = LocalConnectionBase(out isRowZero);
            //if (bConnectResult)
            //    m_GBV.Globe_Multi.m_Flow.SetTouchMode(TouchMode.NormalMode, Command._SWITCH.Dis);
            //else
            //{
            //    if (StaticVar._ITSTool_Var == enMode.Console_Mode)
            //        StaticVar.LogWrite("{0}\r\n", m_GBV.ConsoleTool.strInforMessage);
            //}
            return bConnectResult;
        }

        private void Reset_ConnectionStatus(int iSelected, DevStatus ToolStyle)
        {
            if (StaticVar.iNowDeviceCnts > 0)
            {
                var Dev = StaticVar.lsMain_USBDevInfo[iSelected];
                Dev.NowDevStatus = ToolStyle;
                StaticVar.lsMain_USBDevInfo[iSelected] = Dev;
            }

            switch (ToolStyle)
            {
                case DevStatus.DeviceRemove:
                case DevStatus.Disconnect:
                    //OnResetAllToolsStatus();
                    m_IliFunc.m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(0);
                    m_IliFunc.LocalVariable.Var_Multi.ProtocolType = enPROTOCOL.none;
                    m_IliFunc.m_GBV.Globe_Multi.iICType = 0x00;
                    //m_GBV.Globe_Multi.iICType = 0x00;
                    break;
            }
        }

        public bool ShowICInfo()
        {
            if (b_alreadyInit == false)
            {
                StaticVar.LogWrite("Error: please initial the system first\r\n");
                return false;
            }

            if (!m_gc._DeviceMain.Get_IC_Information(m_var.FW_Upgrade_DevStatus.Master_Mode
                         , false, ref m_IliFunc.m_GBV.ConsoleTool.strInforMessage, ref m_IliFunc.m_GBV.ConsoleTool.strErrorMessage))
            {
                StaticVar.LogWrite(m_IliFunc.m_GBV.ConsoleTool.strErrorMessage);
                return false;
            }
            else
            {
                StaticVar.LogWrite(m_IliFunc.m_GBV.ConsoleTool.strInforMessage);
                return true;
            }
        }

        public bool DisableTouchMode()
        {
            if (b_alreadyInit == false)
            {
                StaticVar.LogWrite("Error: please initial the system first\r\n");
                return false;
            }
            TouchMode mode;
            if (m_var.Var_Multi.ProtocolType == enPROTOCOL.V6)
                mode = TouchMode.SuspendMode;
            else
                mode = TouchMode.TestMode;

            if (m_gc.m_Flow.SetTouchMode(mode, Command._SWITCH.En))
            {
                StaticVar.LogWrite("Info : Disables the touch mode...\r\n");
                return true;
            }
            else
            {
                StaticVar.LogWrite("Error : Disables the touch mode...\r\n");
                return false;
            }
        }

        public bool EnableTouchMode()
        {
            if (b_alreadyInit == false)
            {
                StaticVar.LogWrite("Error: please initial the system first\r\n");
                return false;
            }

            if (m_gc.m_Flow.SetTouchMode(TouchMode.NormalMode, Command._SWITCH.Dis))
            {
                StaticVar.LogWrite("Info : Enable the touch mode...\r\n");
                return true;
            }
            else
            {
                StaticVar.LogWrite("Error : Enable the touch mode...\r\n");
                return false;
            }
        }

        public bool GetSwitchMode(ref byte now_mode)
        {
            if (b_alreadyInit == false)
            {
                StaticVar.LogWrite("Error: please initial the system first\r\n");
                return false;
            }
            m_gc.m_Flow.Update_TuningSupportMode();
            StaticVar.LogWrite(string.Format("Info : The current tuning mode is {0}.\n", m_var.TuningVar.inFWCurrentModeNumber));
            now_mode = (byte)m_var.TuningVar.inFWCurrentModeNumber;
            return true;
        }

        public bool SetSwitchMode(byte mode)
        {
            bool ret = false;
            if (b_alreadyInit == false)
            {
                StaticVar.LogWrite("Error: please initial the system first\r\n");
                return ret;
            }
            if (m_var.TuningVar.inFWEnabledModes != 3)
            {
                StaticVar.LogWrite("Error: This fw version only supports one mode\r\n");
            }
            else
            {
                if (mode >= 0 && mode <= 2)
                {
                    ret = m_gc._TuningTool_.SwitchTuningMode((byte)mode);
                }
                else
                {
                    StaticVar.LogWrite(string.Format("Error: The setting number({0}) was not supported.\n", mode));
                }
            }
            return ret;
        }

        public bool CmdCtrl(string str_cmd)
        {
            bool ret = false;
            if (b_alreadyInit == false)
            {
                StaticVar.LogWrite("Error: please initial the system first\r\n");
                return ret;
            }
            else
            {
                m_gc.m_Flow.g_StopReadMonitor(100);
                m_gc.m_Flow.g_OpenReadMonitor();//開啟USB接收
                Console_Start();
                StaticVar.LogWrite(string.Format("[SEND] >>>      {0}\n", str_cmd));
                string result = "";
                byte[] ByteArray = new byte[100];
                vfWriteUSB_Base(str_cmd, ref result, ref ByteArray);
                if (ByteArray[0] != 0x03)
                {
                    StaticVar.LogWrite("Error: return byte[0]!=0x03\r\n");
                    return false;
                }
                int iReturnLength = (int)ByteArray[3];

                if (iReturnLength != 0)
                {
                    StringBuilder strbudBuffer = new StringBuilder();
                    if (!Console_Read_Base(ref strbudBuffer))
                    {
                        StaticVar.LogWrite("Error: execution was a failed\r\n");
                        return false;
                    }
                    else
                        StaticVar.LogWrite(strbudBuffer.ToString());
                }

                return true;
            }
        }

        public bool CheckMCUVerandHex(string file)
        {
            string strErrorMSG = "";
            //int MCU = GBV.Globe_M[0].DevPanelInfo.MCU;
            if (m_gc._FWUpgrade_.DecodeFile_API(m_var.Var_Multi.DevPanelInfo.MCU, file, ref strErrorMSG))
            {
                StaticVar.LogWrite(string.Format("Info: MCU version match\n"));
                return true;
            }
            else
            {
                StaticVar.LogWrite(string.Format("Error: MCU version doesn't match, IC=ILI{0:x4}, want to upgrade=ILI{1:x4}\n Error Message: {2}\r\n"
                    , m_var.Var_Multi.DevPanelInfo.MCU
                    , m_var.VarUpgrade_Multi.HEX_Panel_Info.iHex_MCU
                    , strErrorMSG)
                    );
                return false;
            }
        }

        public bool CheckMCUVer(int chk_MCU, ref int MCU)
        {
            MCU = m_var.Var_Multi.DevPanelInfo.MCU;
            if (MCU != chk_MCU)
            {
                StaticVar.LogWrite((string.Format("Error: MCU version doesn't match, IC=ILI{0:x4}, want to upgrade=ILI{1:x4}\n", MCU, chk_MCU)));
                return false;
            }
            else
            {
                StaticVar.LogWrite(string.Format("Info: MCU version match\n"));
                return true;
            }
        }

        public bool GetMCUVer(ref int MCU)
        {
            MCU = m_var.Var_Multi.DevPanelInfo.MCU;
            if (MCU != 0)
            {
                StaticVar.LogWrite(string.Format("Info: MCU is {0:X}\n", MCU));
                return true;
            }
            else
            {
                StaticVar.LogWrite("Error: Get the IC mcu is 0\r\n");
                return false;
            }
        }

        public void SoftReset()
        {
            if (m_gc.m_Method_CMD.SetSoftReset())
                StaticVar.LogWrite("Info: pass\r\n");
            else
                StaticVar.LogWrite("Error: failed\r\n");
        }
    }
}