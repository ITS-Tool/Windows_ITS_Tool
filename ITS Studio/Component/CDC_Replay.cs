using CommonExt;
using FuncMethodCmd;
using ITS_Studio;
using ITS_Studio.AllForms;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ThirdPartyTools;
using System.Threading.Tasks;

namespace CSharp_PlayBar
{
    public class CDC_Replay : IDisposable
    {
        private _DEBUG_ DBG_Tool = new _DEBUG_();
        private UIPage MyUIPage = new UIPage();
        private DynamicTool MyTool = new DynamicTool();
        private NotStaticVar LocalVariable = null;
        private GBV.GlobeClass Globe_Multi = null;
        private int iChannelX = 0, iChannelY = 0;
        private CDCGetStatistics CDCStatisticsFm;
        private UserControl_PlayBar Ctrl_PlayBar;
        public bool bUseNewFormat = false;
        private ParseLog _ParseLog = new ParseLog();
        private INI_Tool History_INI = new INI_Tool(ITS_Directory.History_INI);

        public CDC_Replay(UserControl_PlayBar _MyPlayBar, NotStaticVar _LocalVariable, GBV.GlobeClass _Globe_Multi)
        {
            //Now_Action = _ButtonStatus;
            Ctrl_PlayBar = _MyPlayBar;
            LocalVariable = _LocalVariable;
            Globe_Multi = _Globe_Multi;
            if (LocalVariable.Var_Multi.DevPanelInfo == null)
                LocalVariable.Var_Multi.DevPanelInfo = new Panel_Info();
            if (Ctrl_PlayBar != null)
                if (Ctrl_PlayBar.ShowCDCStatisticsFunc == null)           
                    Ctrl_PlayBar.ShowCDCStatisticsFunc += btn_ShowCDCStatisticsForm_Click;
        }
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private bool GetSelectFileName(ref string strFilePath)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Source csv/cdc files (*.csv , *.cdc)|*.csv;*.cdc";
            openDlg.DefaultExt = "csv";
            //=========================================
            var tmpPath = History_INI.IniReadValue("Replay_FilePath", "CDC_Tool_Path", "");
            if(string.IsNullOrEmpty(tmpPath))
                tmpPath = ITS_Directory.RecordPath + "Record_CDC";
            
            tmpPath = Path.GetFullPath(tmpPath);
            if(!File.Exists(tmpPath))
                tmpPath = ITS_Directory.RecordPath + "Record_CDC";

            if(tmpPath.Length > 259)
            {
                MyUIPage.ShowErrorDialog("Error", "The path length exceeds the operating system limit.", UIStyle.Red, true);
                History_INI.IniWriteValueW("Replay_FilePath", "CDC_Tool_Path", "");
                return false;
            }
            
            openDlg.InitialDirectory = tmpPath;
            //=========================================
            //openDlg.InitialDirectory = ITS_Directory.RecordPath + "Record_CDC";
            openDlg.FileName = "";
            openDlg.CheckFileExists = true;
            openDlg.CheckPathExists = true;
            openDlg.Multiselect = false;
            openDlg.Title = "Source CDC File";
            if(openDlg.ShowDialog() != DialogResult.OK)
                return false;
            
            strFilePath = openDlg.FileNames.GetValue(0).ToString();

            string RelativePath = MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, strFilePath);
            History_INI.IniWriteValueW("Replay_FilePath", "CDC_Tool_Path", RelativePath);

            return true;
        }

        public bool Load_CDC_RecordFile()
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
            UI_GBV.ToolMain.lbMax_Value.Text = "";
            UI_GBV.ToolMain.lbMin_Value.Text = "";
            UI_GBV.ToolMain.lbDelta_Value.Text = "";

            string ErrStr = "";
            bool bResult = false;
            Task workTask = null;
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
                workTask = Task.Factory.StartNew(() => Playback_Parse_cdc_File(strFilePath, ref ErrStr)).ContinueWith(x =>
                {
                    bResult = x.Result;
                });
            }

            if(workTask.Status != TaskStatus.RanToCompletion)
                workTask.Wait();//等待直到工作結束
            //===========================================
            CDCStatisticsFm = new CDCGetStatistics(LocalVariable.VarCDCTool_Multi.FileEmployee.iTotal_Frame_Count);
            CDCStatisticsFm.Owner = StaticVar.MainFormParent;
            CDCStatisticsFm.StartPosition = FormStartPosition.CenterScreen;
            if(bUseNewFormat)
                CDCStatisticsFm.GetCDCStatisticsFunc += btn_GetCDCStatistics_Click_CDC;
            else
                CDCStatisticsFm.GetCDCStatisticsFunc += btn_GetCDCStatistics_Click_CSV;

            Ctrl_PlayBar.FrameNum_Text = "0";
            Ctrl_PlayBar.TrackBar_En = bResult;
            if(!bResult)
            {
                MyUIPage.HideWaitForm();
                MyUIPage.ShowErrorNotifier(ErrStr, false, 10000);
                return false;                
            }

            Ctrl_PlayBar.FileName_Text = Path.GetFileName(strFilePath);
            //===========================================
            //Set_1stFrame
            LocalVariable.VarCDCTool_Multi.bGet1stFrame = false;
            ILITek_ITS_Tool.fm_CDCTool.CDC_Toolfm_Init(false);
            if (!ILITek_ITS_Tool.fm_CDCTool.AssignSelectFrameTo_1St(bUseNewFormat))
            {
                MyUIPage.ShowErrorDialog("CDC Format Error", "This file is not a CDC recording, or there is an error in the file format.", UIStyle.Red, true);
                MyUIPage.HideWaitForm();
                return false;
            }
            //ILITek_ITS_Tool.fm_CDCTool.AssignSelectFrameTo_1St(bUseNewFormat);
            MyUIPage.HideWaitForm();
            return true;
        }

        private bool btn_ShowCDCStatisticsForm_Click(out string strMessage)
        {
            FormCollection fc = Application.OpenForms;
            //判斷Form有沒有被開啟
            strMessage = "";
            foreach(Form frm in fc)
            {
                if(frm == CDCStatisticsFm)
                {
                    strMessage = "請勿重複點選";
                    return false;
                }
            }
            if(bUseNewFormat)
            {
                if(!File.Exists(LocalVariable.VarCDCTool_Multi.FileEmployee.FileName))
                {
                    strMessage = "請先匯入.CDC檔案";
                    return false;
                }
                if(CDCStatisticsFm.IsDisposed)
                {
                    CDCStatisticsFm = new CDCGetStatistics(LocalVariable.VarCDCTool_Multi.FileEmployee.iTotal_Frame_Count);
                    CDCStatisticsFm.Owner = StaticVar.MainFormParent;
                    CDCStatisticsFm.StartPosition = FormStartPosition.CenterScreen;
                    CDCStatisticsFm.GetCDCStatisticsFunc += btn_GetCDCStatistics_Click_CDC;
                    CDCStatisticsFm.iMaxFrameCount = LocalVariable.VarCDCTool_Multi.FileEmployee.iTotal_Frame_Count;
                    CDCStatisticsFm.iEndFrame = LocalVariable.VarCDCTool_Multi.FileEmployee.iTotal_Frame_Count;
                    CDCStatisticsFm.Show();
                }
                else
                {
                    CDCStatisticsFm.iMaxFrameCount = LocalVariable.VarCDCTool_Multi.FileEmployee.iTotal_Frame_Count;
                    CDCStatisticsFm.iEndFrame = LocalVariable.VarCDCTool_Multi.FileEmployee.iTotal_Frame_Count;
                    CDCStatisticsFm.Show();
                }
    
            }
            else
            {
                if(LocalVariable._PlayBar_VAR.PlayBar_Frame.Count == 0)
                {
                    strMessage = "請先匯入CDC檔案";
                    return false;
                }


                if(CDCStatisticsFm.IsDisposed)
                {
                    CDCStatisticsFm = new CDCGetStatistics(LocalVariable._PlayBar_VAR.PlayBar_Frame.Count);
                    CDCStatisticsFm.Owner = StaticVar.MainFormParent;
                    CDCStatisticsFm.StartPosition = FormStartPosition.CenterScreen;
                    CDCStatisticsFm.GetCDCStatisticsFunc += btn_GetCDCStatistics_Click_CSV;
                    CDCStatisticsFm.iMaxFrameCount = LocalVariable._PlayBar_VAR.PlayBar_Frame.Count;
                    CDCStatisticsFm.iEndFrame = LocalVariable._PlayBar_VAR.PlayBar_Frame.Count;
                    CDCStatisticsFm.Show();
                }
                else
                {
                    CDCStatisticsFm.iMaxFrameCount = LocalVariable._PlayBar_VAR.PlayBar_Frame.Count;
                    CDCStatisticsFm.iEndFrame = LocalVariable._PlayBar_VAR.PlayBar_Frame.Count;
                    CDCStatisticsFm.Show();
                }
            }
           
            return true;
        }

        private void btn_GetCDCStatistics_Click_CDC(object sender, EventArgs e)
        {
            int iStartFrame = CDCStatisticsFm.iStartFrame;
            int iEndFrame = CDCStatisticsFm.iEndFrame;
            int iWin2Threshold = CDCStatisticsFm.iWin2Threshold;
            int iMax = int.MinValue;
            int iMin = int.MaxValue;
            int iMaxTmp = iMax;
            int iMinTmp = iMin;
            int iMaxFrame = 0;
            int iMinFrame = 0;
            int iDelta = 0;
            int iWin2FailCnt = 0;
            int iAA_X = LocalVariable.Var_Multi.DevPanelInfo.iAA_X - 1;
            int iAA_Y = LocalVariable.Var_Multi.DevPanelInfo.iAA_Y - 1;
            List<int> lsWin2FailFrame = new List<int>();
            if(iEndFrame < iStartFrame)
            {
                MessageBox.Show("Start Frame不可以大於End Frame", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //這裡要做分析
            //==============================================
            // New Buff
            //==============================================
            //LocalVariable.VarCDCTool_Multi.FileEmployee.ID_Table
            LocalVariable._PlayBar_VAR.PlayBar_Frame.Clear();
            bool bIsContains_Pen = (LocalVariable.VarCDCTool_Multi.CDC_Data_Style & enCDCSMStyle.Pen) == enCDCSMStyle.Pen;
            bIsContains_Pen &= (LocalVariable.Var_Multi.DevPanelInfo.SupportPenType != Pen_CDC_Format.enPenType.Disable);
            bool bIsShort = false;
            bool bIsPenRawData = false;
            if(LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                bIsShort = LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT;
                bIsPenRawData |= LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_MPP;
                bIsPenRawData |= LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_USI;
                bIsPenRawData |= LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_WGP;
            }
            else if(LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
            {
                bIsShort = (LocalVariable.VarCDCTool_Multi.V3_NowSelect_CDC.Mutual_CDC_Name == V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT);
            }

            for(int i = iStartFrame; i <= iEndFrame; i++)
            {
                //====================================================
                PLAYBAR_FRAME_STRUCT Employee = new PLAYBAR_FRAME_STRUCT();
                Employee.FrameNo = i;
                NewBuffer(ref Employee);
                
                if(bIsShort)
                {
                    Fill_Short_FrameData(i, ref Employee);
                }
                else
                {
                    Fill_AA_FrameData(i, ref Employee);
                }

                LocalVariable._PlayBar_VAR.PlayBar_Frame.Add(Employee);
                Globe_Multi._CDCTool_.MaxMinAnalysis((i - iStartFrame));
                
                //====================================================
                if(LocalVariable._PlayBar_VAR.PlayBar_Frame.Count != 0)
                {
                    var idx = LocalVariable._PlayBar_VAR.PlayBar_Frame.Count - 1;
                    iMax = Math.Max(iMax, LocalVariable._PlayBar_VAR.PlayBar_Frame[idx].AA_Mutual.Cast<int>().Max());
                    iMin = Math.Min(iMin, LocalVariable._PlayBar_VAR.PlayBar_Frame[idx].AA_Mutual.Cast<int>().Min());
                }
                //====================================================
                if(iMax != iMaxTmp)
                {
                    iMaxFrame = i;
                    iMaxTmp = iMax;
                }

                if(iMin != iMinTmp)
                {
                    iMinFrame = i;
                    iMinTmp = iMin;
                }

                LocalVariable.VarCDCTool_Multi.MutualData_Win2 = new int[iAA_X, iAA_Y];
                LocalVariable.VarCDCTool_Multi.MutualData = LocalVariable._PlayBar_VAR.PlayBar_Frame[i - iStartFrame].AA_Mutual;
                for(int iX = 0; iX < iAA_X; iX++)
                {
                    for(int iY = 0; iY < iAA_Y; iY++)
                    {
                        var num_1 = LocalVariable.VarCDCTool_Multi.MutualData[iX, iY];
                        var num_2 = LocalVariable.VarCDCTool_Multi.MutualData[iX + 1, iY];
                        var num_3 = LocalVariable.VarCDCTool_Multi.MutualData[iX, iY + 1];
                        var num_4 = LocalVariable.VarCDCTool_Multi.MutualData[iX + 1, iY + 1];
                        int douValue = Math.Abs(Math.Abs(num_1 - num_2) - Math.Abs(num_3 - num_4));
                        LocalVariable.VarCDCTool_Multi.MutualData_Win2[iX, iY] = (int)(douValue);
                        if(douValue > iWin2Threshold)
                        {
                            iWin2FailCnt++;
                            if(!lsWin2FailFrame.Contains(i))
                                lsWin2FailFrame.Add(i);
                        }
                    }
                }
            }

            if((iEndFrame - iStartFrame) > LocalVariable._PlayBar_VAR.PlayBar_Frame.Count)
            {
                MessageBox.Show("Start / End 範圍不可大於總Frame數", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(iWin2Threshold == -1)
            {
                MessageBox.Show("Win2 Threshold不可為空值", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iDelta = iMax - iMin;
            CDCStatisticsFm.iMaxValue = iMax;
            CDCStatisticsFm.iMinValue = iMin;
            CDCStatisticsFm.iDeltaValue = iDelta;
            CDCStatisticsFm.iMaxFrameValue = iMaxFrame;
            CDCStatisticsFm.iMinFrameValue = iMinFrame;
            CDCStatisticsFm.iWin2Cnt = iWin2FailCnt;
            CDCStatisticsFm.ClearWin2FrameList();
            CDCStatisticsFm.AddWin2FrameList(lsWin2FailFrame);
        }

        private void btn_GetCDCStatistics_Click_CSV(object sender, EventArgs e)
        {
            int iStartFrame = CDCStatisticsFm.iStartFrame;
            int iEndFrame = CDCStatisticsFm.iEndFrame;
            int iWin2Threshold = CDCStatisticsFm.iWin2Threshold;

            if (iEndFrame < iStartFrame)
            {
                MessageBox.Show("Start Frame不可以大於End Frame", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(iStartFrame > LocalVariable._PlayBar_VAR.PlayBar_Frame.Count 
                || iEndFrame > LocalVariable._PlayBar_VAR.PlayBar_Frame.Count)
            {
                MessageBox.Show("Start / End 範圍不可大於總Frame數", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (iWin2Threshold == -1)
            {
                MessageBox.Show("Win2 Threshold不可為空值", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int iMax = int.MinValue;
            int iMin = int.MaxValue;
            int iMaxTmp = iMax;
            int iMinTmp = iMin;

            int iMaxFrame = 0;
            int iMinFrame = 0;

            int iDelta = 0;

            int iWin2FailCnt = 0;
            int iAA_X = LocalVariable.Var_Multi.DevPanelInfo.iAA_X - 1;
            int iAA_Y = LocalVariable.Var_Multi.DevPanelInfo.iAA_Y - 1;
            List<int> lsWin2FailFrame = new List<int>();
            for (int i = iStartFrame; i <= iEndFrame; i++)
            {
                LocalVariable.VarCDCTool_Multi.MutualData = LocalVariable._PlayBar_VAR.PlayBar_Frame[i-1].AA_Mutual;
                LocalVariable.VarCDCTool_Multi.SelfData[enSelf.Sense] = LocalVariable._PlayBar_VAR.PlayBar_Frame[i-1].AA_Self_X;
                LocalVariable.VarCDCTool_Multi.SelfData[enSelf.Drive] = LocalVariable._PlayBar_VAR.PlayBar_Frame[i-1].AA_Self_Y;

                LocalVariable.VarCDCTool_Multi.MutualKey = LocalVariable._PlayBar_VAR.PlayBar_Frame[i-1].Key_Mutual;
                LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Sense] = LocalVariable._PlayBar_VAR.PlayBar_Frame[i-1].Key_Self_X;
                LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Drive] = LocalVariable._PlayBar_VAR.PlayBar_Frame[i-1].Key_Self_Y;
    
                if (LocalVariable._PlayBar_VAR.PlayBar_Frame[i-1].Record_PenData != null)
                {
                    LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._X] = LocalVariable._PlayBar_VAR.PlayBar_Frame[i-1].Record_PenData[enPenCDC._X];
                    LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._Y] = LocalVariable._PlayBar_VAR.PlayBar_Frame[i-1].Record_PenData[enPenCDC._Y];
                }

                Globe_Multi._CDCTool_.MaxMinAnalysis(i-1);

                iMax = Math.Max(iMax, LocalVariable.VarCDCTool_Multi.MutualData.Cast<int>().Max());
                iMin = Math.Min(iMin, LocalVariable.VarCDCTool_Multi.MutualData.Cast<int>().Min());

                if (iMax != iMaxTmp)
                {
                    iMaxFrame = i;
                    iMaxTmp = iMax;
                }

                if (iMin != iMinTmp)
                {
                    iMinFrame = i;
                    iMinTmp = iMin;
                }

                LocalVariable.VarCDCTool_Multi.MutualData_Win2 = new int[iAA_X, iAA_Y];
                for (int iX = 0; iX < iAA_X; iX++)
                {
                    for (int iY = 0; iY < iAA_Y; iY++)
                    {
                        var num_1 = LocalVariable.VarCDCTool_Multi.MutualData[iX, iY];
                        var num_2 = LocalVariable.VarCDCTool_Multi.MutualData[iX + 1, iY];
                        var num_3 = LocalVariable.VarCDCTool_Multi.MutualData[iX, iY + 1];
                        var num_4 = LocalVariable.VarCDCTool_Multi.MutualData[iX + 1, iY + 1];
                        int douValue = Math.Abs(Math.Abs(num_1 - num_2) - Math.Abs(num_3 - num_4));
                        LocalVariable.VarCDCTool_Multi.MutualData_Win2[iX,iY] = (int)(douValue);
                        if(douValue > iWin2Threshold)
                        {
                            iWin2FailCnt++;
                            if(!lsWin2FailFrame.Contains(i))
                                lsWin2FailFrame.Add(i);
                        }
                    }
                }               
            }
            iDelta = iMax - iMin;
            CDCStatisticsFm.iMaxValue = iMax;
            CDCStatisticsFm.iMinValue = iMin;
            CDCStatisticsFm.iDeltaValue = iDelta;
            CDCStatisticsFm.iMaxFrameValue = iMaxFrame;
            CDCStatisticsFm.iMinFrameValue = iMinFrame;
            CDCStatisticsFm.iWin2Cnt = iWin2FailCnt;
            CDCStatisticsFm.ClearWin2FrameList();
            CDCStatisticsFm.AddWin2FrameList(lsWin2FailFrame);
        }

        public void PlayFile_CDC()
        {
            //Console.WriteLine("PlayFile_CDC() <<< {0}", Ctrl_PlayBar.GetButtonStatus);
            switch (Ctrl_PlayBar.GetButtonStatus)
            {
                case ButtonStatus.Play:
                    ILITek_ITS_Tool.fm_CDCTool.CDC_Replay_Pause = false;
                    Ctrl_PlayBar.ChangeStatus(ButtonStatus.Pause);
                    break;

                case ButtonStatus.Ready:
                case ButtonStatus.Stop:
                    ILITek_ITS_Tool.fm_CDCTool.CDC_Toolfm_Init(false);
                    ILITek_ITS_Tool.fm_CDCTool.CDC_Replay_Start(bUseNewFormat);
                    Ctrl_PlayBar.ChangeStatus(ButtonStatus.Pause);
                    break;

                case ButtonStatus.Pause:
                    ILITek_ITS_Tool.fm_CDCTool.CDC_Replay_Pause = true;
                    Ctrl_PlayBar.ChangeStatus(ButtonStatus.Play);
                    break;

                default:
                    break;
            }
        }

        public void StopPlay()
        {
            ILITek_ITS_Tool.fm_CDCTool.CDC_Replay_ThreadStop(bUseNewFormat);
        }
        
        private bool Playback_Parse_cdc_File(string strFilePath, ref string ErrStr)
        {
            bool bFileIsNormal = false;
            if(LocalVariable.Var_Multi.DevPanelInfo == null)
                LocalVariable.Var_Multi.DevPanelInfo = new Panel_Info();

            LocalVariable.VarCDCTool_Multi.FileEmployee = new _FileEmployee();
            LocalVariable.VarCDCTool_Multi.FileEmployee.FileName = strFilePath;            

            //預先將狀態設為Disable
            LocalVariable.Var_Multi.DevPanelInfo.SupportPenType = Pen_CDC_Format.enPenType.Disable;

            _ParseLog.SeekReadFileRange_EndTag_1(strFilePath, 0, "INFO_START_TAG", string.Format("#{0}@", 1), ref LocalVariable.VarCDCTool_Multi.FileEmployee.InfoBuff);

            //==============================================
            // Update Title Info
            //==============================================                        
            if(!UpdateTitleInfo(LocalVariable.VarCDCTool_Multi.FileEmployee.InfoBuff, null, ref ErrStr))
            {                
                Ctrl_PlayBar.TrackBar_En = bFileIsNormal;
                return bFileIsNormal;
            }

            var tmp = LocalVariable.VarCDCTool_Multi.FileEmployee.InfoBuff.Find(x => x.Contains("Total_Frame_Count:"));
            if(tmp != null)
            {
                string pattern = @"Total_Frame_Count:\s*(\d+)";
                Match match = Regex.Match(tmp, pattern);
                LocalVariable.VarCDCTool_Multi.FileEmployee.iTotal_Frame_Count = int.Parse(match.Groups[1].Value);                
            }
            //==============================================
            // Read Frame No.
            //==============================================
            _ParseLog.Capture_All_TagID(LocalVariable.VarCDCTool_Multi.FileEmployee.FileName, ref LocalVariable.VarCDCTool_Multi.FileEmployee.ID_Table);
           
            //=========================================
            LocalVariable.VarCDCTool_Multi.bRunningStatus = true;
            Ctrl_PlayBar.TrackBar_Min = 1;
            Ctrl_PlayBar.TrackBar_Max = LocalVariable.VarCDCTool_Multi.FileEmployee.iTotal_Frame_Count;
            Ctrl_PlayBar.TrackBar_Value = 1;
            bFileIsNormal = Ctrl_PlayBar.TrackBar_Max > 0 ? true : false;

            if(!bFileIsNormal)
                ErrStr += "Not support format.\r\n";            
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
            //DBG_Tool.ClrString();
            DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Restart);
            var logList = new List<string>(File.ReadAllLines(strFilePath));
            DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Show, "ReadAllLines\n");

            DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Restart);
            if (!UpdateTitleInfo(logList, "No", ref ErrStr))
            {
                Ctrl_PlayBar.TrackBar_En = bFileIsNormal;
                return bFileIsNormal;
            }
            DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Show, "UpdateTitleInfo()\n");

            //==============================================
            // Read Frame No.
            //==============================================            
            DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Restart);
            var lsFrameNo = logList.FindAll(x => x.StartsWith(CDC_RecordFrameTitle.No.Desc()));
            DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Show, "FindAll CDC_RecordFrameTitle\n");

            LocalVariable._PlayBar_VAR.PlayBar_Frame.Clear();
            DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Stop);
            bool bIsContains_Pen = (LocalVariable.VarCDCTool_Multi.CDC_Data_Style & enCDCSMStyle.Pen) == enCDCSMStyle.Pen;
            bIsContains_Pen &= (LocalVariable.Var_Multi.DevPanelInfo.SupportPenType != Pen_CDC_Format.enPenType.Disable);
            bool bIsShort = false;
            bool bIsPenRawData = false;
            if (LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                bIsShort = LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT;
                bIsPenRawData |= LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_MPP;
                bIsPenRawData |= LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_USI;
                bIsPenRawData |= LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_WGP;
            }
            else if (LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
            {
                bIsShort = (LocalVariable.VarCDCTool_Multi.V3_NowSelect_CDC.Mutual_CDC_Name == V3_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SHORT);
            }
            //========================================
            //New Buff
            int LastIndex = 0;
            foreach (var TitleWord in lsFrameNo)
            {
                PLAYBAR_FRAME_STRUCT Employee = new PLAYBAR_FRAME_STRUCT();
                int.TryParse(TitleWord.Split('=')[1], out Employee.FrameNo);
                DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Restart, "FrameNo_{0}", Employee.FrameNo);
                NewBuffer(ref Employee);

                int No_Indx = logList.FindIndex(LastIndex, x => x == TitleWord);
                if (bIsShort)
                {
                    Fill_Short_FrameData(logList, No_Indx, ref Employee);
                }
                else
                {
                    if (!bIsPenRawData)
                    {
                        int iAA_Index = logList.FindIndex(No_Indx, x => x.StartsWith(CDC_RecordFrameTitle.Driven_Data.Desc()));
                        if (iAA_Index != -1)
                            Fill_AA_FrameData(logList, iAA_Index, ref Employee);
                    }

                    if (LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts > 0)
                        Fill_Key_FrameData(logList, No_Indx, ref Employee);

                    if (bIsContains_Pen)
                    {
                        bool is_se = false;
                        if (LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_SE_BK)
                            is_se = true;
                        if (!Fill_Pen_FrameData(logList, LocalVariable.VarCDCTool_Multi.PenMode_SubName, No_Indx, is_se, ref Employee))
                        {
                            bFileIsNormal = false;
                            break;
                        }
                    }
                }
                LastIndex = No_Indx;
                LocalVariable._PlayBar_VAR.PlayBar_Frame.Add(Employee);
                DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Show);
            }

            //=========================================
            LocalVariable.VarCDCTool_Multi.bRunningStatus = true;
            Ctrl_PlayBar.TrackBar_Min = 1;
            Ctrl_PlayBar.TrackBar_Max = LocalVariable._PlayBar_VAR.PlayBar_Frame.Count;
            Ctrl_PlayBar.TrackBar_Value = 1;
            
            bFileIsNormal = Ctrl_PlayBar.TrackBar_Max > 0 ? true : false;

            if (!bFileIsNormal)
                ErrStr += "Not support format.\r\n";

            DBG_Tool.ClrString();
            DBG_Tool.WriteTimer(_DEBUG_.enStopWatch.Stop);
            return bFileIsNormal;
        }

        public bool UpdateTitleInfo(List<string> logList, string strEndTag, ref string ErrStr)
        {
            bool bFileIsNormal = false;
            List<string> TitlelogList = new List<string>();
            if (!string.IsNullOrEmpty(strEndTag))
            {
                var TitleEnd = logList.FindIndex(x => x.Contains(strEndTag));
                if(TitleEnd == -1)
                {
                    ErrStr += "File format error\n";
                    return false;
                }
                TitlelogList = logList.GetRange(0, TitleEnd);
            }
            else
            {
                TitlelogList = logList;
            }
            
            //預先初始化應該要擺在最前面
            LocalVariable.Var_Multi.DevPanelInfo.SupportPenType = Pen_CDC_Format.enPenType.Disable;
            LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts = 0;
            LocalVariable.Var_Multi.DevPanelInfo.iAA_Mutual_Points = 0;
            LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo = new Key_Prototype(enKey_Mode.NO_Key, null);
            
            foreach (var employee in EnumTool.EnumToList<GenLog.CDC.CDC_INFO_TAG>())
            {
                var keyword = TitlelogList.FindIndex(x => x.Contains(employee.Desc()));
                if (keyword == -1)
                    continue;

                string strValue = TitlelogList[keyword];
                string strInterfaceProtocol = string.Empty;
                if(employee == GenLog.CDC.CDC_INFO_TAG.CDC_INFO_LOG_HEADER)
                {
                    if(strValue.Split(':')[1].Contains("CDC"))
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
                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_PTL_TYPE:
                        LocalVariable.Var_Multi.ProtocolType = (MyPair.Value.Contains("V3")) ? enPROTOCOL.V3 : enPROTOCOL.V6;
                        if (MyPair.Value.Split('_')[1].Contains("HID-I2C"))
                            strInterfaceProtocol = MyPair.Value.Split('_')[1].Replace("HID-I2C", "HID");
                        else
                            strInterfaceProtocol = MyPair.Value.Split('_')[1];

                        var mySelect = EnumTool.GetValueFromDescription<DEV_INTERFACE_TYPE>(strInterfaceProtocol);
                        Globe_Multi.TestInterface = (DEV_INTERFACE_TYPE)mySelect;

                        if(!UI_GBV.fmITS_Tool.ResetProtocol(0, string.Format("Protocol_{0}_{1}", LocalVariable.Var_Multi.ProtocolType, Globe_Multi.TestInterface.Desc())))
                        {
                            ErrStr += string.Format("ResetProtocol Fail: ProtocolType={0} TestInterface={1}\n", LocalVariable.Var_Multi.ProtocolType, Globe_Multi.TestInterface);
                            bFileIsNormal = false;
                        }
                        break;

                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_DATA_FORMAT:
                        if (LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                            LocalVariable.VarCDCTool_Multi.V3_NowSelect_CDC.DATA_Resolution_Mutual
                                = (MyPair.Value.Contains("16_Bits")) ? V3_CDC_MODE_Define._DATA_Resolution._16_Bits : V3_CDC_MODE_Define._DATA_Resolution._8_Bits;
                        break;

                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_SENSING_METHOD:
                        int SelectIndex = 0;
                        if (LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V3)
                        {
                            object tmp_CDC_Style_V3 = new List<V3_CDC_MODE_Define.CDC_Contain>();
                            Globe_Multi._CDCTool_.CDC_Contain_Init(ref tmp_CDC_Style_V3, false);//先填參數
                            var tmpList = EnumTool.EnumToList<V3_CDC_MODE_Define.CDC_Type_Mutual>().ToList();
                            SelectIndex = tmpList.FindIndex(x => x.ToString() == MyPair.Value);
                            if(SelectIndex == -1)
                                SelectIndex = 0;
                            LocalVariable.VarCDCTool_Multi.V3_NowSelect_CDC = ((List<V3_CDC_MODE_Define.CDC_Contain>)tmp_CDC_Style_V3)[SelectIndex];
                            LocalVariable.VarCDCTool_Multi.CDC_Data_Style = LocalVariable.VarCDCTool_Multi.V3_NowSelect_CDC.iCDC_Data_Style;
                            LocalVariable._CDCTool_VAR.strTChartTitle = LocalVariable.VarCDCTool_Multi.V3_NowSelect_CDC.strCDCFormatText;
                        }
                        else if (LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                        {
                            object tmp_CDC_Style_V6 = new List<V6_CDC_MODE_Define.CDC_Contain>();
                            Globe_Multi._CDCTool_.CDC_Contain_Init(ref tmp_CDC_Style_V6, false);//先填參數
                            var tmpList = EnumTool.EnumToList<V6_CDC_MODE_Define.CDC_Type_Mutual>().ToList();
                            SelectIndex = tmpList.FindIndex(x => x.ToString() == MyPair.Value);
                            if (SelectIndex == -1)
                                SelectIndex = 0;
                            LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC = ((List<V6_CDC_MODE_Define.CDC_Contain>)tmp_CDC_Style_V6)[SelectIndex];
                            LocalVariable.VarCDCTool_Multi.CDC_Data_Style = LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.CDC_Data_Style;
                            LocalVariable._CDCTool_VAR.strTChartTitle = LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.strCDCFormatText;
                        }

                        //LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo = new Key_Prototype(enKey_Mode.NO_Key, null);
                        break;

                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_X_CH:
                        iChannelX = 0;
                        int.TryParse(MyPair.Value, out iChannelX);
                        LocalVariable.Var_Multi.DevPanelInfo.iAA_X = iChannelX;
                        LocalVariable.Var_Multi.DevPanelInfo.iSelf_Channel_X = iChannelX;
                        break;

                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_Y_CH:
                        iChannelY = 0;
                        int.TryParse(MyPair.Value, out iChannelY);
                        LocalVariable.Var_Multi.DevPanelInfo.iAA_Y = iChannelY;
                        LocalVariable.Var_Multi.DevPanelInfo.iSelf_Channel_Y = iChannelY;
                        break;

                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_MCU_TYPE:
                        //LocalVariable.Var_Multi.DevPanelInfo.MCU = Int32.Parse(MyPair.Value.Trim().Replace("ILI", ""));
                        LocalVariable.Var_Multi.DevPanelInfo.MCU = MyPair.Value.Trim();
                        LocalVariable.Var_Multi.DevPanelInfo.MCU = Globe_Multi.m_Flow.ILIxxxx_To_ILIxxxxS(LocalVariable.Var_Multi.DevPanelInfo.MCU);
                        //Globe_Multi.iICType = Convert.ToInt16(LocalVariable.Var_Multi.DevPanelInfo.MCU.ToString(), 16);
                        //Globe_Multi.strICType = MyPair.Value.Trim();
                        //Console.WriteLine("DevPanelInfo.MCU = {0}", LocalVariable.Var_Multi.DevPanelInfo.MCU);            
                        break;

                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_KEY_MODE:
                        enKey_Mode _KeyMode = enKey_Mode.NO_Key;
                        Enum.TryParse<enKey_Mode>(MyPair.Value, out _KeyMode);
                        LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo = new Key_Prototype(_KeyMode, null);
                        //UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.g_KeyInfo.Key_Mode
                        break;

                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_SUPPORT_KEY:
                        bool bSupportKey = false;
                        bool.TryParse(MyPair.Value, out bSupportKey);
                        if (!bSupportKey)
                            LocalVariable.VarCDCTool_Multi.CDC_Data_Style &= ~enCDCSMStyle.Key;
                        break;

                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_KEY_CNTS:
                        LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts = Int32.Parse(MyPair.Value);
                        break;

                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_PEN_TYPE:
                        var Arr = MyPair.Value.Split(',').Select(x => x.Trim()).ToArray();
                        foreach (var item in Arr)
                        {
                            if (((string)item).Length == 0)
                                break;
                            LocalVariable.Var_Multi.DevPanelInfo.SupportPenType |= EnumTool.ParseEnum<Pen_CDC_Format.enPenType>(item);
                        }

                        break;
                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_DMSG_TYPE:
                        if (!MyPair.Value.IsNullOrWhiteSpace())
                        {
                            LocalVariable.Var_Multi.DevPanelInfo.iCModelDmsgType = Convert.ToInt32(MyPair.Value, 16);

                        }
                        break;
                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_CDC_TYPE:
                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_IC_TYPE:
                    case GenLog.CDC.CDC_INFO_TAG.CDC_INFO_OVER_8BITS:
                    default:
                        break;
                }
                if (!bFileIsNormal)
                    break;
            }

            if (LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                if ((LocalVariable.VarCDCTool_Multi.CDC_Data_Style & enCDCSMStyle.Pen) == enCDCSMStyle.Pen &&
                    LocalVariable.Var_Multi.DevPanelInfo.SupportPenType != Pen_CDC_Format.enPenType.Disable)
                    NewPenData(LocalVariable.VarCDCTool_Multi.V6_NowSelect_CDC.Mutual_CDC_Name, LocalVariable.Var_Multi.DevPanelInfo.SupportPenType.ToString());
                else
                    LocalVariable.Var_Multi.DevPanelInfo.SupportPenType = Pen_CDC_Format.enPenType.Disable;
            }

            if (bFileIsNormal)
                LocalVariable.Var_Multi.DevPanelInfo.iAA_Mutual_Points = iChannelX * iChannelY;

            if (!bFileIsNormal)
                ErrStr += "Not support format.\r\n";

            return bFileIsNormal;
        }

        private void NewPenData(V6_CDC_MODE_Define.CDC_Type_Mutual Mutual_CDC_Name, string MyPairValue)
        {
            Match rxMatch = Match.Empty;
            LocalVariable.VarCDCTool_Multi.PenMode_SubName = null;
            if (Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_USI)
            {
                //if (!EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_USI>(MyPairValue))
                //return;
                //Pen_CDC_Format.TEST_MODE_PEN_USI item = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_USI>(MyPairValue);
                LocalVariable.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_USI.ACK; //20220414 Steven: 為支持Deamon , USI預設顯示ACK
                LocalVariable.VarCDCTool_Multi.USI_PenData.Clear();
                foreach (var _data in EnumTool.EnumToList<Pen_CDC_Format.TEST_MODE_PEN_USI>())
                    LocalVariable.VarCDCTool_Multi.USI_PenData.Add(_data, new Int16[2][]);
            }
            else if (Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_WGP)
            {
                //if (!EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_WGP>(MyPairValue))
                //    return;
                //Pen_CDC_Format.TEST_MODE_PEN_WGP item = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_WGP>(MyPairValue);                
                LocalVariable.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_WGP.TipRing0;//20220414 Steven: 為支持Deamon , WGP預設顯示TipRing0
                LocalVariable.VarCDCTool_Multi.WGP_PenData.Clear();
                foreach (var _data in EnumTool.EnumToList<Pen_CDC_Format.TEST_MODE_PEN_WGP>())
                    LocalVariable.VarCDCTool_Multi.WGP_PenData.Add(_data, new Int16[2][]);
            }
            else if (Mutual_CDC_Name == V6_CDC_MODE_Define.CDC_Type_Mutual.TEST_MODE_PEN_MPP)
            {
                //if (!EnumTool.IsContainsDescription<Pen_CDC_Format.TEST_MODE_PEN_MPP>(MyPairValue))
                //    return;
                //Pen_CDC_Format.TEST_MODE_PEN_MPP item = EnumTool.GetValueFromDescription<Pen_CDC_Format.TEST_MODE_PEN_MPP>(MyPairValue);
                LocalVariable.VarCDCTool_Multi.PenMode_SubName = Pen_CDC_Format.TEST_MODE_PEN_MPP.BeaconLF;//20220414 Steven: 為支持Deamon , MPP預設顯示BeaconLF
                LocalVariable.VarCDCTool_Multi.MPP_PenData.Clear();
                foreach (var _data in EnumTool.EnumToList<Pen_CDC_Format.TEST_MODE_PEN_MPP>())
                    LocalVariable.VarCDCTool_Multi.MPP_PenData.Add(_data, new Int16[2][]);
            }
        }

        private void NewBuffer(ref PLAYBAR_FRAME_STRUCT Employee)
        {
            //AA Area
            Employee.AA_Mutual = new int[iChannelX, iChannelY];

            //Self
            Employee.AA_Self_X = new int[iChannelX];
            Employee.AA_Self_Y = new int[iChannelY];

            //Key
            bool bIsContains_Key = (LocalVariable.VarCDCTool_Multi.CDC_Data_Style & enCDCSMStyle.Key) == enCDCSMStyle.Key;
            if (bIsContains_Key)
            {
                Employee.Key_Mutual = new int[LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts];
                Employee.Key_Self_X = new int[LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts];
                Employee.Key_Self_Y = new int[1];
            }

            //Pen
            bool bIsContains_Pen = (LocalVariable.VarCDCTool_Multi.CDC_Data_Style & enCDCSMStyle.Pen) == enCDCSMStyle.Pen;
            if (bIsContains_Pen)
            {
                Employee.Record_PenData = new Dictionary<enPenCDC, List<short>>();
                Employee.Record_PenData.Add(enPenCDC._X, new List<short>());
                Employee.Record_PenData.Add(enPenCDC._Y, new List<short>());
            }

            //Short
            Employee.Record_ShortData = new Dictionary<SHORT_NAME, SHORT_STRUCT>();
            foreach (var item in EnumTool.EnumToList<SHORT_NAME>())
            {
                SHORT_STRUCT tmp = new SHORT_STRUCT() { ListArray = new List<int>() };
                Employee.Record_ShortData.Add(item, tmp);
            }
        }

        private void Fill_Short_FrameData(int SelectIdx, ref PLAYBAR_FRAME_STRUCT Employee)
        {
            ParseLog._CDC_Datas CDC_Datas = new ParseLog._CDC_Datas();
            var iAA_X = LocalVariable.Var_Multi.DevPanelInfo.iAA_X;
            var iAA_Y = LocalVariable.Var_Multi.DevPanelInfo.iAA_Y;
            _ParseLog.Transfer_CDC(LocalVariable.VarCDCTool_Multi.FileEmployee, SelectIdx, ref CDC_Datas);
            List<SHORT_NAME> SupportName = new List<SHORT_NAME>();
            if (LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                SupportName.Add(SHORT_NAME.X_SLK);
                SupportName.Add(SHORT_NAME.Y_SLK);
            }
            else
            {
                SupportName.Add(SHORT_NAME.X_SLK);
                SupportName.Add(SHORT_NAME.Y_SLK);
                SupportName.Add(SHORT_NAME.X_LK);
                SupportName.Add(SHORT_NAME.Y_LK);
            }

            foreach (var name in SupportName)
            {
                SHORT_STRUCT tmpSHORT_STRUCT = Employee.Record_ShortData[name];
                switch(name)
                {
                    case SHORT_NAME.X_SLK:
                        tmpSHORT_STRUCT.ListArray = CDC_Datas.Self_X.Select(x => (int)x).ToList();
                        break;
                    case SHORT_NAME.X_LK:
                        tmpSHORT_STRUCT.ListArray = CDC_Datas.Self_LK_X.Select(x => (int)x).ToList();
                        break;
                    case SHORT_NAME.Y_SLK:
                        tmpSHORT_STRUCT.ListArray = CDC_Datas.Self_Y.Select(x => (int)x).ToList();
                        break;
                    case SHORT_NAME.Y_LK:
                        tmpSHORT_STRUCT.ListArray = CDC_Datas.Self_LK_Y.Select(x => (int)x).ToList();
                        break;
                }
                Employee.Record_ShortData[name] = tmpSHORT_STRUCT;
            }
        }

        private void Fill_Short_FrameData(List<string> logList, int Index, ref PLAYBAR_FRAME_STRUCT Employee)
        {
            int Keyword_Index = logList.FindIndex(Index, x => x.StartsWith(CDC_RecordFrameTitle.Short_Data.Desc()));
            List<SHORT_NAME> SupportName = new List<SHORT_NAME>();
            if (LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
            {
                SupportName.Add(SHORT_NAME.X_SLK);
                SupportName.Add(SHORT_NAME.Y_SLK);
            }
            else
            {
                SupportName.Add(SHORT_NAME.X_SLK);
                SupportName.Add(SHORT_NAME.Y_SLK);
                SupportName.Add(SHORT_NAME.X_LK);
                SupportName.Add(SHORT_NAME.Y_LK);
            }

            var FrameData = logList.GetRange(Keyword_Index + 1, SupportName.Count).ToList();

            foreach (var name in SupportName)
            {
                SHORT_STRUCT tmpSHORT_STRUCT = Employee.Record_ShortData[name];
                int num = 0;
                var row = FrameData.Find(x => x.StartsWith(name.ToString()));
                tmpSHORT_STRUCT.ListArray = row.Split(',').Where(x => int.TryParse(x, out num) == true).Select(x => int.Parse(x)).ToList();
                Employee.Record_ShortData[name] = tmpSHORT_STRUCT;
            }
        }

        private bool Fill_Pen_FrameData(List<string> logList, object _Type, int No_Index, bool is_se, ref PLAYBAR_FRAME_STRUCT Employee)
        {
            bool isValid = false;
            var iTitleSize = Enum.GetNames(typeof(CDC_RecordFrameTitle)).Length;
            int iRowCnt = 0;
            int iColCnt = 0;
            //var dict = logList.Select((s, i) => new { s, i }).ToDictionary(x => x.i, x => x.s);
            //Console.WriteLine(dict);
            foreach (var item in EnumTool.EnumToList<enPenCDC>())
            {
                if (item == enPenCDC.none)
                    continue;
                int Keyword_Index = -1;
                //int iAA_Channels = 0;
                if (item == enPenCDC._X)
                {
                    if (_Type == null)
                    {
                        iRowCnt = (int)Pen_CDC_Format.TEST_MODE_PEN_X.TipRing_X;
                        //iAA_Channels = iChannelY;
                    }
                    else
                    {
                        if (_Type is Pen_CDC_Format.TEST_MODE_PEN_USI)
                        {
                            foreach (var item_2 in EnumTool.GetDescriptions<Pen_CDC_Format.TEST_MODE_PEN_USI_X>())
                            {
                                Match rxMatch = Regex.Match(item_2, @"Size:(.*)");
                                iRowCnt += int.Parse(rxMatch.Groups[1].ToString());
                            }
                        }
                        else if (_Type is Pen_CDC_Format.TEST_MODE_PEN_WGP)
                        {
                            foreach (var item_2 in EnumTool.GetDescriptions<Pen_CDC_Format.TEST_MODE_PEN_WGP_X>())
                            {
                                Match rxMatch = Regex.Match(item_2, @"Size:(.*)");
                                iRowCnt += int.Parse(rxMatch.Groups[1].ToString());
                            }
                        }
                        else if (_Type is Pen_CDC_Format.TEST_MODE_PEN_MPP)
                        {
                            foreach (var item_2 in EnumTool.GetDescriptions<Pen_CDC_Format.TEST_MODE_PEN_MPP_X>())
                            {
                                Match rxMatch = Regex.Match(item_2, @"Size:(.*)");
                                iRowCnt += int.Parse(rxMatch.Groups[1].ToString());
                            }
                        }
                    }
                    //==============================================
                    //方法1
                    //int iRange = iAA_Channels + iRowCnt + 4 + iTitleSize;
                    //Keyword_Index = logList.FindIndex(KeyIndex, iRange, x => x.StartsWith(EnumTool.GetEnumDescription(CDC_RecordFrameTitle.Pen_RawData_X)));

                    //方法2
                    Keyword_Index = logList.FindIndex(No_Index, x => x.StartsWith(CDC_RecordFrameTitle.Pen_RawData_X.Desc()));
                    if (Keyword_Index == -1)
                        return isValid;
                    var FrameData = logList.GetRange(Keyword_Index + 2, iRowCnt).ToList();

                    //方法3
                    //Keyword_Index = dict.Skip(KeyIndex).First(x => x.Value.StartsWith(EnumTool.GetEnumDescription(CDC_RecordFrameTitle.Pen_RawData_X))).Key;
                    //var FrameData = dict.OrderBy(d => d.Key).Skip(Keyword_Index + 2).Take(iRowCnt).Select(x => x.Value).ToList();

                    foreach (var row in FrameData)
                    {
                        int num = 0;
                        List<short> d = row.Split(',').Where(x => int.TryParse(x, out num) == true).Select(x => short.Parse(x)).ToList();
                        if (d.Count() == LocalVariable.Var_Multi.DevPanelInfo.iAA_X)
                            Employee.Record_PenData[item].AddRange(d);
                        else
                            return isValid;
                    }
                }

                if (item == enPenCDC._Y)
                {
                    //iRowCnt = iChannelY;
                    if (_Type == null)
                    {
                        iColCnt = (int)Pen_CDC_Format.TEST_MODE_PEN_Y.TipRing_Y;
                        //iAA_Channels = iChannelY;
                    }
                    else
                    {
                        if (_Type is Pen_CDC_Format.TEST_MODE_PEN_USI)
                        {
                            foreach (var item_2 in EnumTool.GetDescriptions<Pen_CDC_Format.TEST_MODE_PEN_USI_Y>())
                            {
                                Match rxMatch = Regex.Match(item_2, @"Size:(.*)");
                                iColCnt += int.Parse(rxMatch.Groups[1].ToString());
                            }
                        }
                        else if (_Type is Pen_CDC_Format.TEST_MODE_PEN_WGP)
                        {
                            foreach (var item_2 in EnumTool.GetDescriptions<Pen_CDC_Format.TEST_MODE_PEN_WGP_Y>())
                            {
                                Match rxMatch = Regex.Match(item_2, @"Size:(.*)");
                                iColCnt += int.Parse(rxMatch.Groups[1].ToString());
                            }
                        }
                        else if (_Type is Pen_CDC_Format.TEST_MODE_PEN_MPP)
                        {
                            foreach (var item_2 in EnumTool.GetDescriptions<Pen_CDC_Format.TEST_MODE_PEN_MPP_Y>())
                            {
                                Match rxMatch = Regex.Match(item_2, @"Size:(.*)");
                                iColCnt += int.Parse(rxMatch.Groups[1].ToString());
                            }
                        }
                    }
                    //==============================================
                    //方法1
                    //int iRange = iAA_Channels + iRowCnt + 4 + 2 + iTitleSize;
                    //Keyword_Index = logList.FindIndex(KeyIndex, iRange, x => x.StartsWith(EnumTool.GetEnumDescription(CDC_RecordFrameTitle.Pen_RawData_Y)));

                    //方法2
                    Keyword_Index = logList.FindIndex(No_Index, x => x.StartsWith(CDC_RecordFrameTitle.Pen_RawData_Y.Desc()));
                    if (Keyword_Index == -1)
                        return isValid;
                    var FrameData = logList.GetRange(Keyword_Index + 1, iChannelY).ToList();

                    //方法3
                    //Keyword_Index = dict.Skip(KeyIndex).First(x => x.Value.StartsWith(EnumTool.GetEnumDescription(CDC_RecordFrameTitle.Pen_RawData_Y))).Key;
                    //var FrameData = dict.OrderBy(d => d.Key).Skip(Keyword_Index + 1).Take(iChannelY).Select(x => x.Value).ToList();
                    int sz = 0;
                    if (is_se)
                        sz = (int)Pen_CDC_Format.TEST_MODE_PEN_X.TipRing_X;
                    else
                        sz = iColCnt;
                    List<short> tmp = new List<short>();
                    foreach (var row in FrameData)
                    {
                        int num = 0;
                        List<short> d = row.Split(',').Where(x => int.TryParse(x, out num) == true).Select(x => short.Parse(x)).ToList();
                        if (d.Count() == sz)
                            tmp.AddRange(d);
                        else
                            return isValid;
                    }

                    for (int iX = 0; iX < iColCnt; iX++)
                        for (int iY = 0; iY < iChannelY; iY++)
                            Employee.Record_PenData[item].Add(tmp[iX + iY * iColCnt]);
                }
                if (Keyword_Index == -1)
                    continue;
            }
            isValid = true;
            return isValid;
        }

        private void Fill_Key_FrameData(List<string> logList, int KeyIndex, ref PLAYBAR_FRAME_STRUCT Employee)
        {
            int iKey_Index = logList.FindIndex(KeyIndex, x => x.StartsWith(CDC_RecordFrameTitle.Key_Data.Desc()));
            if (iKey_Index == -1)
                return;
            int iRowCnt = 1;
            bool IsContainSelfKey = (LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6);

            if (IsContainSelfKey)
                iRowCnt++;

            var FrameData = logList.GetRange(iKey_Index + 2, iRowCnt).ToList();

            foreach (var row in FrameData)
            {
                int num = 0;
                var list = row.Split(',').Where(x => int.TryParse(x, out num) == true).Select(x => int.Parse(x)).ToList();
                if (row.Contains("KeyDrv1"))
                {
                    if (IsContainSelfKey)
                        Employee.Key_Mutual = list.GetRange(0, list.Count - 1).ToArray();
                    else
                        Employee.Key_Mutual = list.GetRange(0, list.Count).ToArray();
                    if (IsContainSelfKey)
                        Employee.Key_Self_Y[0] = list[list.Count - 1];

                }
                else
                {
                    Employee.Key_Self_X = list.ToArray();
                }
            }
        }
        
        private void Fill_AA_FrameData(int SelectIdx, ref PLAYBAR_FRAME_STRUCT Employee)
        {
            ParseLog._CDC_Datas CDC_Datas = new ParseLog._CDC_Datas();
            var iAA_X = LocalVariable.Var_Multi.DevPanelInfo.iAA_X;
            var iAA_Y = LocalVariable.Var_Multi.DevPanelInfo.iAA_Y;
            _ParseLog.Transfer_CDC(LocalVariable.VarCDCTool_Multi.FileEmployee, SelectIdx, ref CDC_Datas);
            //bool bIsContains_Self_Y = (LocalVariable.VarCDCTool_Multi.CDC_Data_Style & enCDCSMStyle.Self_Y) == enCDCSMStyle.Self_Y;
            //bool bIsContains_Self_X = (LocalVariable.VarCDCTool_Multi.CDC_Data_Style & enCDCSMStyle.Self_X) == enCDCSMStyle.Self_X;

            //int iRowCnt = iChannelY;
            //if(bIsContains_Self_X)
            //    iRowCnt++;
            //var FrameData = logList.GetRange(KeyIndex + 2, iRowCnt).ToList();

            //List<int> tmpList = new List<int>();
            //int inum = 0;
            //foreach(var row in FrameData)
            //{
            //    int num = 0;
            //    var list = row.Split(',').Where(x => int.TryParse(x, out num) == true).Select(x => int.Parse(x)).ToList();
            //    if(tmpList.Count >= Employee.AA_Mutual.Length)
            //    {
            //        if(bIsContains_Self_X)
            //            Employee.AA_Self_X = list.GetRange(0, list.Count).ToArray();
            //    }
            //    else
            //    {
            //        tmpList.AddRange(list.GetRange(0, LocalVariable.Var_Multi.DevPanelInfo.iAA_X)); // 20220413 Steven 原本紀錄-1會導致mployee.AA_Mutual[ix, iy] = tmpList[ix_ch * iy + ix];爆掉
            //        if(bIsContains_Self_Y)
            //            Employee.AA_Self_Y[inum++] = list[list.Count - 1];
            //    }
            //}
            
            int ix_ch = Employee.AA_Mutual.GetLength(0);
            int iy_ch = Employee.AA_Mutual.GetLength(1);
            for(int iy = 0; iy < iy_ch; iy++)
                for(int ix = 0; ix < ix_ch; ix++)
                    Employee.AA_Mutual[ix, iy] = CDC_Datas.CDC[ix_ch * iy + ix];
        }
        
        private void Fill_AA_FrameData(List<string> logList, int KeyIndex, ref PLAYBAR_FRAME_STRUCT Employee)
        {
            bool bIsContains_Self_Y = (LocalVariable.VarCDCTool_Multi.CDC_Data_Style & enCDCSMStyle.Self_Y) == enCDCSMStyle.Self_Y;
            bool bIsContains_Self_X = (LocalVariable.VarCDCTool_Multi.CDC_Data_Style & enCDCSMStyle.Self_X) == enCDCSMStyle.Self_X;

            int iRowCnt = iChannelY;
            if (bIsContains_Self_X)
                iRowCnt++;
            var FrameData = logList.GetRange(KeyIndex + 2, iRowCnt).ToList();

            List<int> tmpList = new List<int>();
            int inum = 0;
            foreach (var row in FrameData)
            {
                int num = 0;
                var list = row.Split(',').Where(x => int.TryParse(x, out num) == true).Select(x => int.Parse(x)).ToList();
                if (tmpList.Count >= Employee.AA_Mutual.Length)
                {
                    if (bIsContains_Self_X)
                        Employee.AA_Self_X = list.GetRange(0, list.Count).ToArray();
                }
                else
                {
                    tmpList.AddRange(list.GetRange(0, LocalVariable.Var_Multi.DevPanelInfo.iAA_X)); // 20220413 Steven 原本紀錄-1會導致mployee.AA_Mutual[ix, iy] = tmpList[ix_ch * iy + ix];爆掉
                    if (bIsContains_Self_Y)
                        Employee.AA_Self_Y[inum++] = list[list.Count - 1];
                }
            }
            int ix_ch = Employee.AA_Mutual.GetLength(0);
            int iy_ch = Employee.AA_Mutual.GetLength(1);
            for (int iy = 0; iy < iy_ch; iy++)
                for (int ix = 0; ix < ix_ch; ix++)
                    Employee.AA_Mutual[ix, iy] = tmpList[ix_ch * iy + ix];
        }
    }
}
