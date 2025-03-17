
using CommonExt;
using FuncMethodCmd;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ThirdPartyTools;

namespace ITS_Studio.AllForms
{
    public partial class Consolefm : Form
    {
        private int inUSBData_TextLen = 0;
        private int m_iSelectedDev = 0;
        public int SelectedDev { set { m_iSelectedDev = value; } get { return m_iSelectedDev; } }
        protected List<string> _ScriptInfo;
        List<string> sUIList;
        public Consolefm()
        {
            InitializeComponent();
        }

        private void CollectCommand()
        {
            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool == null)
                return;

            comboBox_Send.Items.Clear();
            comboBox_Send.Items.AddRange(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool.Send_CMD_List.Select(x => x.Key).ToArray());
            comboBox_Read.Items.Clear();
            comboBox_Read.Items.AddRange(UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool.Recv_CMD_List.Select(x => x.Key).ToArray());
        }

        private void comboBox_Read_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _MyObj = sender as ComboBox;
            if (!UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool.Recv_CMD_List.ContainsKey(_MyObj.Text))
                return;
            var _para = _MyObj.Text;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool.Recv_CMD_List[_MyObj.Text](_para);
        }

        public enum ScriptType : int
        {
            [StringValue("Forloop")]
            Forloop = 0,
            [StringValue("Write")]
            Write = 1,
            [StringValue("Delay")]
            Delay = 2,
            [StringValue("Read")]
            Read = 3,
        }

        public struct ScriptTestItem
        {
            public ScriptType scriptType;
            public int iLoopTimes;
            public int iDelayTimes;
            public byte[] bDataArrary;
        }

        public void m_rtbUSBData_TextAdd(string strValue)
        {
            strValue += "\n";
            inUSBData_TextLen += strValue.Length;
            RichEdit_ConsoleBox.AppendText(strValue);
            m_rtbUSBData_EndLine();
        }

        public bool Console_Init()
        {
            CollectCommand();

            splitContainer1.IsSplitterFixed = true;

            // m_rtbUSBData
            m_rtbUSBData_TextClear();

            string strbuf = Prompt.Text;
            string strKey = @"{W_Len}";
            Prompt.Select(strbuf.IndexOf(strKey), strKey.Length);
            Prompt.SelectionColor = Color.Blue;

            strKey = @"{R_Len}";
            Prompt.Select(strbuf.IndexOf(strKey), strKey.Length);
            Prompt.SelectionColor = Color.Red;

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

        public void Console_Read()
        {
            StringBuilder strbudBuffer = new StringBuilder();

            int inDataTabCount = 0;
            byte Header = UI_GBV.mDev[m_iSelectedDev].LocalVariable._Console_VAR.byReceiverData[(int)Packet.IC_Header];

            if (Header == (byte)Command.Bridge_CMD._0XAC)
                return;

            strbudBuffer.Append("[RECV] <<<\t");

            int iValue = Header & 0xF0;
            if (Header == 0xA3)
            {
                int iHeader_Length = EnumTool.EnumToList<Packet>().Count();
                int iRcvLeng = UI_GBV.mDev[m_iSelectedDev].LocalVariable._Console_VAR.byReceiverData[(int)Packet.R_1] + iHeader_Length;
                if (iRcvLeng <= 64)
                {
                    string strprt = string.Join(" ", UI_GBV.mDev[m_iSelectedDev].LocalVariable._Console_VAR.byReceiverData.GetRange(0, iRcvLeng).Select(x => x.ToString("X2")).ToArray());
                    strbudBuffer.AppendLine(strprt);
                    strbudBuffer.AppendLine("=======================");
                    strbudBuffer.AppendLine(JudgeReturnCommand(UI_GBV.mDev[m_iSelectedDev].LocalVariable._Console_VAR.byReceiverData));
                    strbudBuffer.AppendLine("=======================");
                }
               
            }
            else if (Header == 0xA4)
            {
                int iRcvLen = UI_GBV.mDev[m_iSelectedDev].LocalVariable._Console_VAR.byReceiverData[3] + UI_GBV.mDev[m_iSelectedDev].LocalVariable._Console_VAR.byReceiverData[4];
            }
            else
            {
                if ((iValue == 0x40) || (iValue == 0x00))//畫線報點
                {
                    strbudBuffer.Append("Position\r\n");
                    int inTotalCount = 0x40;

                    List<string> tmp = UI_GBV.mDev[m_iSelectedDev].LocalVariable._Console_VAR.byReceiverData.GetRange(0, inTotalCount).Select(x => string.Format(" {0:X2}", x)).ToList();
                    var _strprt = from p in tmp
                                  select new { Owner = ((++inDataTabCount % 32 == 0) ? (p + "\r\n") : (inDataTabCount % 4) == 0 ? p + " " : p) }.Owner;
                    strbudBuffer.Append(string.Join("", _strprt.ToArray()));
                }
                else
                {
                    strbudBuffer.Append("Unknown\r\n");
                    List<string> tmp = UI_GBV.mDev[m_iSelectedDev].LocalVariable._Console_VAR.byReceiverData.GetRange(0, 64).Select(x => string.Format(" {0:X2}", x)).ToList();
                    var _strprt = from p in tmp
                                  select new { Owner = ((++inDataTabCount % 32 == 0) ? (p + "\r\n") : (inDataTabCount % 4) == 0 ? p + " " : p) }.Owner;
                    strbudBuffer.Append(string.Join("", _strprt.ToArray()));
                }
            }
            
            m_rtbUSBData_TextAdd(strbudBuffer.ToString());
        }

        public void Console_Start()
        {
            if (UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool == null)
                return;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool.ConsoleTool_Start();
        }

        public void Console_Stop()
        {
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool.ConsoleTool_Stop();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bThreadDecodeData_En = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable._Console_VAR.bRunningStatus = false;
        }

        public void vfPrintOUTPackage(byte[] OutBuffer)
        {
            string strprt = vstrPrintOUTPackage(OutBuffer);
            m_rtbUSBData_TextAdd(strprt);
        }

        //public void vfPrintStringOnm_rtbUSBData(string strPrintString)
        //{
        //    this.Invoke(new MethodInvoker(delegate()
        //    {
        //        m_rtbUSBData_TextAdd(strPrintString);
        //    }));
        //}

        private void comboBox_Send_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _MyObj = sender as ComboBox;
            if (!UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool.Send_CMD_List.ContainsKey(_MyObj.Text))
                return;

            string pattern = @"\([^)]+\)";
            Match _KeyWord = Regex.Match(_MyObj.Text, pattern);
            if (_MyObj.Text.Contains("Change Mode"))
            {
                var _para_tmp = _KeyWord.ToString().Replace("(", "").Replace(")", "");
                var _para = byte.Parse(_para_tmp);
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                    m_rtbUSBData_TextAdd(string.Format("[SEND] >>> 03 A3 04 00 68 5A A5 {0}\t", _para));
                else
                    m_rtbUSBData_TextAdd(string.Format("[SEND] >>> 03 A3 04 00 68 55 AA {0}\t", _para));
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool.Send_CMD_List[_MyObj.Text](_para);

                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_StopReadMonitor(100);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.m_HID_API.HID_CreateHandle(
                StaticVar.lsMain_USBDevInfo[m_iSelectedDev].strDevPath);
                CommonFlow.api_update_ts_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

                CommonFlow.msg_str = String.Empty;
                CommonFlow.api_print_ts_info(UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.commonflow_get_info(
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.tpdev.dev, ref CommonFlow.info[m_iSelectedDev]);

                UI_GBV.mDev[m_iSelectedDev].m_GBV.ConsoleTool.strInforMessage = CommonFlow.msg_str;

            }
            else if ((_MyObj.Text.Contains("Set_TestMode")) || (_MyObj.Text.Contains("Set_EngineerMode")))
            {
                var _para_tmp = _KeyWord.ToString().Replace("(", "").Replace(")", "");
                var _para = (Command._SWITCH)Enum.Parse(typeof(Command._SWITCH), _para_tmp);
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool.Send_CMD_List[_MyObj.Text](_para);
            }
            else
            {
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool.Send_CMD_List[_MyObj.Text](null);
            }
        }

        public void vfWriteUSB()
        {
            var RegexRule = new Regex("0x", RegexOptions.IgnoreCase);
            string OriSentence = txt_SendCMD.Text;
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
                    m_rtbUSBData_TextAdd(string.Format("Type format Error : {0}", tmp[i]));
                    return;
                }
            }
            OriSentence = string.Join("", tmp);
            newSentence = RegexRule.Replace(OriSentence.Trim().Replace(" ", ""), "");
            newSentence = newSentence.Replace(",", "");
            int inLen = newSentence.Length;

            if (inLen % 2 > 0)
                return;

            byte[] ByteArray = ArrayTool.StringToByteArray(newSentence);

            if (newSentence.Length < 6)
                return;

            if (!Wifi_Var.useWifi)
            {
                UI_GBV.mDev[m_iSelectedDev].m_HidAPI.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], ByteArray, true);
            }
            else
            {
                Wifi_Var.AsyncSend(Wifi_Var.packetType, ref ByteArray, ByteArray.Length);

                if (Wifi_Var.needRead(ref ByteArray))
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.HID_Receive_ACK = false;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarThread_Multi.m_bThreadListening = true;
                }
                else
                {
                    Wifi_Var.ReceiveAck();
                }

            }

            string strprt = vstrPrintOUTPackage(ByteArray);
            m_rtbUSBData_TextAdd(strprt);
            txt_SendCMD.Text = "";
        }

        public string vstrPrintOUTPackage(byte[] OutBuffer)
        {
            string strprt = "[SEND] >>>\t";
            int iW_Len = OutBuffer[2];
            for (int index = 0; index < (OutBuffer[2] + 4); index++)
            {
                strprt += string.Format("{0:X2} ", OutBuffer[index]);
            }
            return strprt;
        }

        // e.KeyChar == (Char)8 -----------> Backpace
        // e.KeyChar == (Char)13-----------> Enter
        // e.KeyChar == (Char)32-----------> SPACE
        private void m_rtbSendCMD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 48) && (e.KeyChar <= 57)) // e.KeyChar == (Char)48 ~ 57 -----> 0~9
                e.Handled = false;
            else if (e.KeyChar == (Char)_KeyEventArg.SPACE || e.KeyChar == (Char)_KeyEventArg.Backpace) // SPACE & Backpace
                e.Handled = false;
            else if ((e.KeyChar >= 65) && (e.KeyChar <= 70)) // A ~ F// e.KeyChar == (Char)65 ~ 70 -----> A ~ F
                e.Handled = false;
            else if ((e.KeyChar >= 97) && (e.KeyChar <= 102)) // a ~ f// e.KeyChar == (Char)97 ~ 102 ----> a ~ f
                e.Handled = false;
            else
                e.Handled = true;
        }

        //過濾並整理command 格式
        private void m_rtbSendCMD_TextChanged(object sender, EventArgs e)
        {
            string OriSentence = txt_SendCMD.Text.ToUpper();
            var RegexRule = new Regex("0X", RegexOptions.IgnoreCase);
            string newSentence = RegexRule.Replace(OriSentence, "").Replace(",", " "); ;//去除0x

            string str = string.Join("", newSentence.Split(' ').Where(x => x.Length % 2 != 0).Select(x => x.Insert(0, "0")).ToArray());
            if (!string.IsNullOrEmpty(str))//非正確的格式
                OriSentence = OriSentence.Remove(OriSentence.Length - 1).Insert(OriSentence.Length - 1, str);

            newSentence = RegexRule.Replace(OriSentence.Trim().Replace(" ", ""), "").Replace(",", "");
            int inLen = newSentence.Length;
            if (inLen % 2 > 0)
                return;

            //byte[] ByteArray = ArrayTool.StringToByteArray(newSentence);

            //if (newSentence.Length < 6)
            //    return;

            //if (ByteArray[0] != 0x03)
            //    return;

            // Bridge IC command
            //if (ByteArray[1] == (byte)Head_0XA.IC_0xA3)
            //{
            //    if ((newSentence.Length >= 5) && (newSentence.Length <= 6))
            //    {
            //        string WriteLen = ByteArray[2].ToString("X2");
            //        if (ByteArray[2] == ByteArray[0])
            //            m_rtbSendCMD.Select(OriSentence.LastIndexOf(WriteLen), WriteLen.Length);
            //        else
            //            m_rtbSendCMD.Select(OriSentence.IndexOf(WriteLen), WriteLen.Length);
            //        m_rtbSendCMD.SelectionColor = Color.Blue;
            //    }
            //    else if ((newSentence.Length >= 7) && (newSentence.Length <= 8))
            //    {
            //        string ReadLen = ByteArray[3].ToString("X2");
            //        if (ByteArray.Contains(ByteArray[3]))
            //            m_rtbSendCMD.Select(OriSentence.LastIndexOf(ReadLen), ReadLen.Length);
            //        else
            //            m_rtbSendCMD.Select(OriSentence.IndexOf(ReadLen), ReadLen.Length);
            //        m_rtbSendCMD.SelectionColor = Color.Red;
            //    }
            //    m_rtbSendCMD.Select(m_rtbSendCMD.Text.Length, 0);
            //    m_rtbSendCMD.SelectionColor = Color.Black;
            //}
        }

        private void m_rtbUSBData_EndLine()
        {
            RichEdit_ConsoleBox.SelectionStart = inUSBData_TextLen;
            RichEdit_ConsoleBox.ScrollToCaret();
        }

        private void m_rtbUSBData_TextClear()
        {
            RichEdit_ConsoleBox.Clear();
            inUSBData_TextLen = 0;
        }

        protected DynamicTool MyTool = new DynamicTool();
        List<Action> funList = new List<Action>();
        public delegate void Metodo<T, U>(T parametro1, U parametro2);
        public void FunctionListAdd<T, U>(Metodo<T, U> metodo, T thing, U uther)
        {
            funList.Add(() => metodo.Invoke(thing, uther));
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
            if (openDlg.ShowDialog(this) != DialogResult.OK)
                return false;
            FileName = openDlg.FileName;
            return true;
        }

        private void ExecuteFunction(int loopTimes, List<ScriptTestItem> testItems)
        {
            byte[] DataBytes = new byte[64];
            string sInfo = string.Empty;
            sUIList = new List<string>();
            for (int i = 0; i < loopTimes; i++)
            {
                foreach (var item in testItems)
                {
                    switch (item.scriptType)
                    {
                        case ScriptType.Write:
                            sInfo = string.Empty;
                            for (int j = 0; j < item.bDataArrary.Length; j++)
                            {
                                sInfo += item.bDataArrary[j].ToString("X2") + " ";
                            }
                            UI_GBV.mDev[m_iSelectedDev].m_HidAPI.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], item.bDataArrary, true);
                            sUIList.Add(string.Format("[SEND] >>> {0}\t", sInfo));
                            //m_rtbUSBData_TextAdd();
                            break;
                        case ScriptType.Read:
                            UI_GBV.mDev[m_iSelectedDev].m_HidAPI.HID_WriteData(StaticVar.lsMain_USBDevInfo[m_iSelectedDev], item.bDataArrary, true);
                            break;
                        case ScriptType.Delay:
                            MyTool.DelayForConsole_ms(item.iDelayTimes);
                            sUIList.Add(string.Format("[DELAY] >>> {0} ms\t", item.iDelayTimes.ToString()));
                            //m_rtbUSBData_TextAdd();
                            break;
                        default:
                            break;
                    }

                }
            }          
        }

        private void btn_OpenScript_Click(object sender, EventArgs e)
        {
            bool isFormatCorrect = true;
            string FileName = "";
            byte[] bTempArrary = new byte[64];
            byte[] _bDataArrary = new byte[64];
            int iIndex = 0;
            int _iDelayTimes = 0;
            _ScriptInfo = new List<string>();
            if (!SelectFile("Report (*.csv)|*.csv", ITS_Directory.Tool_CurrentDirectory, out FileName))
                return;
            UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._ConsolTool.LoadScript(FileName, out _ScriptInfo);
            lbl_ScriptFileName.Text = "Script File : " + FileName;
            funList.Clear();
            for (int i = 0; i < _ScriptInfo.Count; i++)
            {
                _ScriptInfo[i] = _ScriptInfo[i].Replace(" ", "").Replace("\t", ""); ;
                string[] sTemp = Regex.Split(_ScriptInfo[i], ",");
                sTemp[0] = sTemp[0].Replace(" ", "");

                if (Enum.IsDefined(typeof(ScriptType), sTemp[0])) //確認string也在enum內
                {
                    List<ScriptTestItem> testItem = new List<ScriptTestItem>();

                    switch ((ScriptType)Enum.Parse(typeof(ScriptType), sTemp[0])) // string => enum
                    {
                        case ScriptType.Forloop:
                            int _iLoopTimes = Convert.ToInt32(sTemp[1]);
                            for (int j = i + 1; j < _ScriptInfo.Count; j++)
                            {
                                bTempArrary = new byte[64];
                                _bDataArrary = new byte[64];
                                iIndex = 0;
                                sTemp = Regex.Split(_ScriptInfo[j], ",");
                                if (sTemp[0].Contains("\t"))
                                {
                                    sTemp[0] = sTemp[0].Replace(" ", "");
                                    sTemp[0] = sTemp[0].Replace("\t", "");
                                    if (sTemp.Length < 2)
                                        continue;
                                    ScriptType testKey = (ScriptType)Enum.Parse(typeof(ScriptType), sTemp[0]);
                                    foreach (string item in sTemp)
                                    {
                                        if (!Enum.IsDefined(typeof(ScriptType), item))
                                        {
                                            if (testKey != ScriptType.Delay)
                                            {
                                                bTempArrary[iIndex++] = Convert.ToByte(item, 16);
                                                _iDelayTimes = 0;
                                            }

                                            else
                                            {
                                                _iDelayTimes = Convert.ToInt32(item);
                                            }
                                        }
                                    }

                                    if (testKey == ScriptType.Read)
                                    {
                                        _bDataArrary[0] = bTempArrary[0];
                                        _bDataArrary[1] = bTempArrary[1];
                                        _bDataArrary[2] = 0x00;
                                        _bDataArrary[3] = bTempArrary[2];
                                    }
                                    else if (testKey == ScriptType.Write)
                                    {
                                        for (int k = 0; k < bTempArrary.Length; k++)
                                        {
                                            _bDataArrary[k] = bTempArrary[k];
                                        }
                                    }
                                    //else if (testKey == ScriptType.Delay)
                                    //{

                                    //}

                                    testItem.Add(new ScriptTestItem
                                    {
                                        iLoopTimes = _iLoopTimes,
                                        iDelayTimes = _iDelayTimes,
                                        scriptType = testKey,
                                        bDataArrary = _bDataArrary
                                    });
                                    i = j;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            FunctionListAdd(ExecuteFunction, _iLoopTimes, testItem);

                            break;
                        case ScriptType.Write:
                            bTempArrary = new byte[64];
                            _bDataArrary = new byte[64];
                            iIndex = 0;
                            foreach (string item in sTemp)
                            {
                                if (!Enum.IsDefined(typeof(ScriptType), item))
                                {
                                    bTempArrary[iIndex++] = Convert.ToByte(item, 16);
                                }
                            }
                            for (int j = 0; j < bTempArrary.Length; j++)
                            {
                                _bDataArrary[j] = bTempArrary[j];
                            }
                            testItem.Add(new ScriptTestItem
                            {
                                iLoopTimes = 1,
                                scriptType = ScriptType.Write,
                                bDataArrary = _bDataArrary
                            });
                            FunctionListAdd(ExecuteFunction, 1, testItem);

                            break;
                        case ScriptType.Read:
                            bTempArrary = new byte[64];
                            iIndex = 0;
                            foreach (string item in sTemp)
                            {
                                if (!Enum.IsDefined(typeof(ScriptType), item))
                                {
                                    bTempArrary[iIndex++] = Convert.ToByte(item, 16);
                                }
                            }
                            _bDataArrary = new byte[64];
                            _bDataArrary[0] = bTempArrary[0];
                            _bDataArrary[1] = bTempArrary[1];
                            _bDataArrary[2] = 0x00;
                            _bDataArrary[3] = bTempArrary[2];
                            testItem.Add(new ScriptTestItem
                            {
                                iLoopTimes = 1,
                                scriptType = ScriptType.Read,
                                bDataArrary = _bDataArrary // Read格式
                            });
                            FunctionListAdd(ExecuteFunction, 1, testItem);
                            break;
                        case ScriptType.Delay:
                            bTempArrary = new byte[64];
                            iIndex = 0;
                            foreach (string item in sTemp)
                            {
                                if (!Enum.IsDefined(typeof(ScriptType), item))
                                {
                                    //bTempArrary[iIndex++] = ;
                                    _iDelayTimes = Convert.ToInt32(item);
                                }
                            }
                            testItem.Add(new ScriptTestItem
                            {
                                iLoopTimes = 1,
                                scriptType = ScriptType.Delay,
                                bDataArrary = bTempArrary,
                                iDelayTimes = _iDelayTimes
                            });
                            FunctionListAdd(ExecuteFunction, 1, testItem);
                            break;

                        default:
                            m_rtbUSBData_TextAdd("格式錯誤!請檢查腳本");
                            isFormatCorrect = false;
                            break;
                    }
                }

            }
            if (isFormatCorrect)
                btn_StartScript.Enabled = true;

        }

        private void btn_StartScript_Click(object sender, EventArgs e)
        {
            btn_StartScript.Enabled = false;
            btn_OpenScript.Enabled = false;

            foreach (var item in funList)
            {
                item.Invoke();
            }

            foreach (var item in sUIList)
            {
                m_rtbUSBData_TextAdd(item);
            }

            btn_StartScript.Enabled = true;
            btn_OpenScript.Enabled = true;
        }
    }
}