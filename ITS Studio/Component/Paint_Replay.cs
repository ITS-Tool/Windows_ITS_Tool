using CommonExt;
using FuncMethodCmd;
using ITS_Studio;
using ITS_Studio.AllForms;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ThirdPartyTools;
using System.Threading.Tasks;

namespace CSharp_PlayBar
{
    public class Paint_Replay : IDisposable
    {
        private UIPage MyUIPage = new UIPage();
        private DynamicTool MyTool = new DynamicTool();
        
        private UserControl_PlayBar Ctrl_PlayBar;
        private NotStaticVar LocalVariable = null;
        private GBV.GlobeClass Globe_Multi = null;
        private int iChannelX = 0, iChannelY = 0;
        private ParseLog _ParseLog = new ParseLog();
        private INI_Tool History_INI = new INI_Tool(ITS_Directory.History_INI);

        public Paint_Replay(UserControl_PlayBar _MyPlayBar, NotStaticVar _LocalVariable, GBV.GlobeClass _Globe_Multi)
        {
            Ctrl_PlayBar = _MyPlayBar;
            LocalVariable = _LocalVariable;
            Globe_Multi = _Globe_Multi;
        }
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private bool GetSelectFileName(ref string strFilePath)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Source csv/paint files (*.csv , *.paint)|*.csv;*.paint";
            openDlg.DefaultExt = "csv";
            //=========================================
            var tmpPath = History_INI.IniReadValue("Replay_FilePath", "Painting_Tool_Path", "");
            if(string.IsNullOrEmpty(tmpPath))
                tmpPath = ITS_Directory.RecordPath + "Record_Paint";

            tmpPath = Path.GetFullPath(tmpPath);
            if(!File.Exists(tmpPath))
                tmpPath = ITS_Directory.RecordPath + "Record_Paint";

            if(tmpPath.Length > 259)
            {
                MyUIPage.ShowErrorDialog("Error", "The path length exceeds the operating system limit.", UIStyle.Red, true);
                History_INI.IniWriteValueW("Replay_FilePath", "Painting_Tool_Path", "");
                return false;
            }

            openDlg.InitialDirectory = tmpPath;
            //=========================================
            //openDlg.InitialDirectory = ITS_Directory.RecordPath + "Record_Paint";
            openDlg.FileName = "";
            openDlg.CheckFileExists = true;
            openDlg.CheckPathExists = true;
            openDlg.Multiselect = false;
            openDlg.Title = "Source Paint File";
            if(openDlg.ShowDialog() != DialogResult.OK)
                return false;

            strFilePath = openDlg.FileNames.GetValue(0).ToString();

            string RelativePath = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, strFilePath);
            History_INI.IniWriteValueW("Replay_FilePath", "Painting_Tool_Path", RelativePath);

            return true;
        }
        
        public bool bUseNewFormat = false;
        
        public bool Load_Painting_RecordFile()
        {
            string strFilePath = "";
            if(!GetSelectFileName(ref strFilePath))
                return false;
            string _str = "";
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_En;
            else
                _str = global::ITS_Studio.Properties.Resources.ProcessingTip_Cn;
            MyUIPage.ShowWaitForm(_str);
            string ErrStr = "";
            bool bResult = false;
            Task workTask = null;

            Ctrl_PlayBar.SetTitlePanelCollapsed = true;
            if(Path.GetExtension(strFilePath) == ".csv")
            {
                bUseNewFormat = false;
                workTask = Task.Factory.StartNew(() => Playback_Parse_csv_File(strFilePath, ref ErrStr)).ContinueWith(x =>
                {
                    bResult = x.Result;
                });
            }
            else
            {                
                bUseNewFormat = true;
                workTask = Task.Factory.StartNew(() => Playback_Parse_paint_File(strFilePath, ref ErrStr)).ContinueWith(x =>
                {
                    bResult = x.Result;
                });
            }
            if(workTask.Status != TaskStatus.RanToCompletion)
                workTask.Wait();//等待直到工作結束
            //===========================================
            Ctrl_PlayBar.SetTitlePanelCollapsed = (LocalVariable.PaintTool_Var.RecordToolName == PaintTool_Var.RecordTool.none);
            Ctrl_PlayBar.SetTitlePanelTitle = LocalVariable.PaintTool_Var.RecordToolName.Desc();
            Ctrl_PlayBar.FrameNum_Text = "0";
            Ctrl_PlayBar.TrackBar_En = bResult;            
            Ctrl_PlayBar.TrackBar_Min = 1;
            if(Path.GetExtension(strFilePath) == ".csv")
                Ctrl_PlayBar.TrackBar_Max = LocalVariable._PlayBar_VAR.PlayBar_Frame.Count;
            else
                Ctrl_PlayBar.TrackBar_Max = LocalVariable.VarPaint_Multi.FileEmployee.iTotal_Frame_Count;
            Ctrl_PlayBar.TrackBar_Value = 1;


            if(!bResult)
            {
                MyUIPage.HideWaitForm();
                MyUIPage.ShowErrorNotifier(ErrStr, false, 10000);
                return false;
            }
          
            Ctrl_PlayBar.FileName_Text = Path.GetFileName(strFilePath);
            //===========================================

            LocalVariable.VarPaint_Multi.bRunningStatus = true;
            if(LocalVariable.PaintTool_Var.CoordinateDisplayMode != DataDisplayMode.FullScreen)
                ILITek_ITS_Tool.fm_PaintTool.InitDataColumn();
            Globe_Multi.m_Flow.PaintTool_StartPainting(enInt_InDataType.Painting);
            ILITek_ITS_Tool.fm_PaintTool.Paint_Toolfm_Init(0);
            MyUIPage.HideWaitForm();
            return true;
        }
       
        private bool Playback_Parse_paint_File(string strFilePath, ref string ErrStr)
        {
            bool bFileIsNormal = false;
            if(LocalVariable.Var_Multi.DevPanelInfo == null)
                LocalVariable.Var_Multi.DevPanelInfo = new Panel_Info();
            
            LocalVariable.VarPaint_Multi.FileEmployee = new _FileEmployee();
            LocalVariable.VarPaint_Multi.FileEmployee.FileName = strFilePath;         

            //預先將狀態設為Disable
            LocalVariable.Var_Multi.DevPanelInfo.SupportPenType = Pen_CDC_Format.enPenType.Disable;

            _ParseLog.SeekReadFileRange_EndTag_1(strFilePath, 0, "INFO_START_TAG", string.Format("#{0}@", 1), ref LocalVariable.VarPaint_Multi.FileEmployee.InfoBuff);
            
            //==============================================
            // Update Title Info
            //==============================================    
            if(!UpdateTitleInfo(LocalVariable.VarPaint_Multi.FileEmployee.InfoBuff, ref ErrStr))                            
                return bFileIsNormal;

            var tmp = LocalVariable.VarPaint_Multi.FileEmployee.InfoBuff.Find(x => x.Contains("Total_Frame_Count:"));
            if(tmp != null)
            {
                string pattern = @"Total_Frame_Count:\s*(\d+)";
                Match match = Regex.Match(tmp, pattern);
                LocalVariable.VarPaint_Multi.FileEmployee.iTotal_Frame_Count = int.Parse(match.Groups[1].Value);
            }
            //==============================================
            // Read Frame No.
            //==============================================                                
            _ParseLog.Capture_All_TagID(LocalVariable.VarPaint_Multi.FileEmployee.FileName, ref LocalVariable.VarPaint_Multi.FileEmployee.ID_Table);
           
            //=========================================
            bFileIsNormal = LocalVariable.VarPaint_Multi.FileEmployee.iTotal_Frame_Count > 0 ? true : false;
            if(!bFileIsNormal)
                ErrStr += "Frame Counts = 0\r\n";

            return bFileIsNormal;
        }

        private bool Playback_Parse_csv_File(string strFilePath, ref string ErrStr)
        {
            bool bFileIsNormal = false;
            if (LocalVariable.Var_Multi.DevPanelInfo == null)
                LocalVariable.Var_Multi.DevPanelInfo = new Panel_Info();
            //預先將狀態設為Disable
            LocalVariable.Var_Multi.DevPanelInfo.SupportPenType = Pen_CDC_Format.enPenType.Disable;
            //==============================================
            // Update Title Info
            //==============================================
            var logList = new List<string>(File.ReadAllLines(strFilePath));
            if (!UpdateTitleInfo(logList, ref ErrStr))            
                return bFileIsNormal;
            
            //==============================================
            // Read Frame No.
            //==============================================            
            var lsFrameNo = logList.FindAll(x => x.StartsWith(Paint_RecordFrameTitle.No.Desc()));
            LocalVariable._PlayBar_VAR.PlayBar_Frame.Clear();

            //========================================
            //New Buff
            foreach (var TitleWord in lsFrameNo)
            {
                PLAYBAR_FRAME_STRUCT Employee = new PLAYBAR_FRAME_STRUCT();
                int.TryParse(TitleWord.Split('=')[1], out Employee.FrameNo);
                NewBuffer(ref Employee);

                int ListIndx = logList.FindIndex(x => x == TitleWord);
                Fill_Data(logList, ListIndx, ref Employee);
                LocalVariable._PlayBar_VAR.PlayBar_Frame.Add(Employee);
            }

            //=========================================
            //Ctrl_PlayBar.TrackBar_Min = 1;
            //Ctrl_PlayBar.TrackBar_Max = LocalVariable._PlayBar_VAR.PlayBar_Frame.Count;
            //Ctrl_PlayBar.TrackBar_Value = 1;
            //bFileIsNormal = Ctrl_PlayBar.TrackBar_Max > 0 ? true : false;
            bFileIsNormal = LocalVariable._PlayBar_VAR.PlayBar_Frame.Count > 0 ? true : false;
            if(!bFileIsNormal)
                ErrStr += "Frame Counts = 0\r\n";
           
            return bFileIsNormal;
        }
        
        private void Fill_Data(List<string> logList, int KeyIndex, ref PLAYBAR_FRAME_STRUCT Employee)
        {
            //Frame Data
            int Index = logList.FindIndex(KeyIndex, x => x.StartsWith(Paint_RecordFrameTitle.Data.Desc()));
            if (Index == -1)
                return;
            Employee.Record_PaintData = logList[Index + 1].Split(',')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => byte.Parse(x, NumberStyles.HexNumber)).ToArray();
            
            //Delta Time
            Index = logList.FindIndex(KeyIndex, x => x.StartsWith(Paint_RecordFrameTitle.Time.Desc()));
            if (Index == -1)
                return;

            Employee.FrameTime = int.Parse(logList[Index].Split('=')[1].Trim()) - 2;//補償UI處理的時間差
            Employee.FrameTime = Employee.FrameTime > 0 ? Employee.FrameTime : 0;
        }
        
        private void NewBuffer(ref PLAYBAR_FRAME_STRUCT Employee)
        {
            Employee.Record_PaintData = new byte[64];
        }

        public bool UpdateTitleInfo(List<string> logList, ref string ErrStr)
        {
            bool bFileIsNormal = false;
            //預先初始化應該要擺在最前面
            LocalVariable.Var_Multi.DevPanelInfo.SupportPenType = Pen_CDC_Format.enPenType.Disable;
            LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts = 0;
            LocalVariable.Var_Multi.DevPanelInfo.iAA_Mutual_Points = 0;
            LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo = new Key_Prototype(enKey_Mode.NO_Key, null);

            foreach (var employee in EnumTool.EnumToList<GenLog.Painting.PAINT_INFO_TAG>())
            {
                var keyword = employee.Desc();
                if(string.IsNullOrEmpty(keyword))
                    continue;
                var _Found = logList.FindIndex(x => x.Contains(keyword));
                if (_Found == -1)                
                    continue;
                
                string strValue = logList[_Found];
                string strInterfaceProtocol = string.Empty;
                if(employee == GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_TOOL_NAME)
                {
                    if(strValue.Split(':')[1].Contains("Paint"))
                        continue;

                    ErrStr += string.Format("{0} format error\n", employee);
                    bFileIsNormal = false;
                    break;
                }
                if (!Regex.IsMatch(strValue, @".*(.*)=(.*)"))
                {
                    ErrStr += string.Format("{0} format error\n", employee);
                    bFileIsNormal = false;
                    break;
                }
                bFileIsNormal = true;
                KeyValuePair<string, string> MyPair = new KeyValuePair<string, string>(strValue.Split('=')[0].Trim(), strValue.Split('=')[1].Trim());
                switch (employee)
                {
                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_PTL_TYPE:  
                        LocalVariable.Var_Multi.ProtocolType = (MyPair.Value.Contains("V3")) ? enPROTOCOL.V3 : enPROTOCOL.V6;

                        if (MyPair.Value.Split('_')[1].Contains("HID-I2C"))
                            strInterfaceProtocol = MyPair.Value.Split('_')[1].Replace("HID-I2C", "HID");
                        else
                            strInterfaceProtocol = MyPair.Value.Split('_')[1];

                        var mySelect = EnumTool.GetValueFromDescription<DEV_INTERFACE_TYPE>(strInterfaceProtocol);
                        Globe_Multi.TestInterface = (DEV_INTERFACE_TYPE)mySelect;
                        StaticVar.lsMain_USBDevInfo[0].stuDeviceTypeForReplay = Globe_Multi.TestInterface;

                        if (!UI_GBV.fmITS_Tool.ResetProtocol(0, string.Format("Protocol_{0}_{1}"
                            , LocalVariable.Var_Multi.ProtocolType
                            , Globe_Multi.TestInterface.Desc())))
                        {
                            ErrStr += string.Format("ResetProtocol Fail: ProtocolType={0} TestInterface={1}\n", LocalVariable.Var_Multi.ProtocolType, Globe_Multi.TestInterface);
                            bFileIsNormal = false;
                        }
                        break;

                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_X_RES:
                        double.TryParse(MyPair.Value, out LocalVariable.Var_Multi.DevPanelInfo.X_RES);
                        break;

                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_Y_RES:
                        double.TryParse(MyPair.Value, out LocalVariable.Var_Multi.DevPanelInfo.Y_RES);
                        break;

                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_X_CH:
                        iChannelX = 0;
                        int.TryParse(MyPair.Value, out iChannelX);
                        LocalVariable.Var_Multi.DevPanelInfo.iAA_X = iChannelX;
                        LocalVariable.Var_Multi.DevPanelInfo.iSelf_Channel_X = iChannelX;
                        break;

                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_Y_CH:
                        iChannelY = 0;
                        int.TryParse(MyPair.Value, out iChannelY);
                        LocalVariable.Var_Multi.DevPanelInfo.iAA_Y = iChannelY;
                        LocalVariable.Var_Multi.DevPanelInfo.iSelf_Channel_Y = iChannelY;
                        break;

                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_FINGERS:
                        int.TryParse(MyPair.Value, out LocalVariable.Var_Multi.DevPanelInfo.iMaxTouchPoint);
                        break;

                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_SUPPORT_KEY:
                        int.TryParse(MyPair.Value, out LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts);
                        break;

                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_KEY_MODE:
                        enKey_Mode _KeyMode = enKey_Mode.NO_Key;
                        Enum.TryParse<enKey_Mode>(MyPair.Value, out _KeyMode);
                        LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo = new Key_Prototype(_KeyMode, null);
                        break;

                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_KEY_SIZE:                        
                        var lsKeys = new List<KEY_TEMPLATE>();
                        var Arr_1 = MyPair.Value.Split(',').ToArray();
                        int iKeys = 1;
                        foreach (var item in Arr_1)
                        {
                            Match _Match = Regex.Match(item, @"\(Start:\((.*)\) W:(.*) H:(.*)\)");
                            var _pos = _Match.Groups[1].ToString().Split(' ').Select(x=>int.Parse(x.Trim())).ToArray();
                            lsKeys.Add(new KEY_TEMPLATE ( iKeys
                                , new Point(_pos[0], _pos[1])
                                , int.Parse(_Match.Groups[2].ToString())
                                , int.Parse(_Match.Groups[3].ToString()) ));
                            iKeys++;
                        }
                        LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Template = lsKeys.ToArray();
                        LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_ScreenPosition = new KEY_TEMPLATE[LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts];
                        break;
                    
                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_RECORDED_BY:
                        LocalVariable.PaintTool_Var.RecordToolName = EnumTool.GetValueFromDescription<PaintTool_Var.RecordTool>(MyPair.Value);
                        break;
                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_TOUCH_FORMAT:
                        var ArrFormat = MyPair.Value.Split(',').Select(x => x.Trim()).ToArray();
                        foreach (var item in ArrFormat)
                        {
                            if (((string)item).Length == 0)
                                break;
                            LocalVariable.Var_Multi.DevPanelInfo.ReportFormatV6 = ((CMD_V6.ReportFormat)Convert.ToInt32(item)).ToString();
                        }
                        break;
                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_PEN_TYPE:
                        var Arr = MyPair.Value.Split(',').Select(x => x.Trim()).ToArray();
                        foreach (var item in Arr)
                        {
                            if (((string)item).Length == 0)
                                break;
                            LocalVariable.Var_Multi.DevPanelInfo.SupportPenType |= EnumTool.ParseEnum<Pen_CDC_Format.enPenType>(item);
                        }
                        break;
                    default:
                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_TOOL_NAME:
                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_IC_TYPE:
                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_PTL_VER:
                    case GenLog.Painting.PAINT_INFO_TAG.PAINT_INFO_FW_VER:
                                      
                        break;
                }
            }
            return bFileIsNormal;
        }

        public void PlayFile_Paint()
        {
            //Console.WriteLine("PlayFile_Paint() <<< {0}", Ctrl_PlayBar.GetButtonStatus);
            switch (Ctrl_PlayBar.GetButtonStatus)
            {
                case ButtonStatus.Play:
                    ILITek_ITS_Tool.fm_PaintTool.Paint_Replay_Pause = false;
                    Ctrl_PlayBar.ChangeStatus(ButtonStatus.Pause);
                    break;
                case ButtonStatus.Ready:
                case ButtonStatus.Stop:
                    ILITek_ITS_Tool.fm_PaintTool.Paint_Toolfm_Init(0);
                    ILITek_ITS_Tool.fm_PaintTool.Painting_Replay_Start(bUseNewFormat);
                    Ctrl_PlayBar.ChangeStatus(ButtonStatus.Pause);
                    break;
                case ButtonStatus.Pause:
                    ILITek_ITS_Tool.fm_PaintTool.Paint_Replay_Pause = true;
                    Ctrl_PlayBar.ChangeStatus(ButtonStatus.Play);
                    break;
            }
        }

        public void StopPlay()
        {
            ILITek_ITS_Tool.fm_PaintTool.StopPaint();
        }
    }
}
