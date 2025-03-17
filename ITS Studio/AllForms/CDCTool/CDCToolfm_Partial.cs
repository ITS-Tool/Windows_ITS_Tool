using CommonExt;
using CSharp_PlayBar;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using ThirdPartyTools;
using System.Linq;

namespace ITS_Studio.AllForms
{
    public partial class CDCToolfm
    {
        protected class Node
        {
            public int iMutual_Max = int.MinValue;
            public int iMutual_Min = int.MaxValue;

            public int iSelfX_Max = int.MinValue;
            public int iSelfX_Min = int.MaxValue;

            public int iSelfY_Max = int.MinValue;
            public int iSelfY_Min = int.MaxValue;
        }

        private int m_iSelectedDev = 0;
        public int SelectedDev { set { m_iSelectedDev = value; } get { return m_iSelectedDev; } }

        [Flags]
        enum enShortType
        {
            None = 0x00,
            SLK = 0x01,
            LK = 0x10,
        }
        //private bool bIsShort = false;
        private enShortType ShortType = enShortType.SLK;
        #region CDC_Replay
        private BackgroundWorker Work_CDC_Replay;
        private ManualResetEvent Pause_CDC_Replay;

        public void CDC_Replay_Start(bool bUseNewFormat)
        {
            MenuItem_DataFormat.Enabled = false;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.bRunningStatus = true;
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.bForceStopCDC = false;

            Pause_CDC_Replay = new ManualResetEvent(true);
            Work_CDC_Replay = new BackgroundWorker();
            if (bUseNewFormat)
                Work_CDC_Replay.DoWork += new DoWorkEventHandler(CDC_Replay_cdc);
            else
                Work_CDC_Replay.DoWork += new DoWorkEventHandler(CDC_Replay_csv);

            Work_CDC_Replay.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CDC_Replay_Completed);
            Work_CDC_Replay.WorkerSupportsCancellation = true;
            Work_CDC_Replay.RunWorkerAsync();
        }

        public bool CDC_Replay_Pause
        {
            set
            {
                if (value == true)
                    Pause_CDC_Replay.Reset();
                else
                    Pause_CDC_Replay.Set();
            }
        }
        private int iProgess = 0;

        private void CDC_Replay_cdc(object sender, DoWorkEventArgs e)
        {
            var FrameCnts = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.FileEmployee.iTotal_Frame_Count;
            for (int idx = 1; idx <= FrameCnts; idx++)
            {
                if (Work_CDC_Replay.CancellationPending)
                    break;
                if (idx != (iProgess + 1))
                    idx = (iProgess + 2);
                this.Invoke(new MethodInvoker(delegate()
                {
                    userControl_PlayBar_CDC.TrackBar_Value = idx;
                }));
                Pause_CDC_Replay.WaitOne(Timeout.Infinite);
                //i = iProgess;
                //iProgess++;
            }
        }

        private void CDC_Replay_csv(object sender, DoWorkEventArgs e)
        {
            var FrameInfo = UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame;
            for (int i = 0; iProgess < FrameInfo.Count; i++)
            {
                if (Work_CDC_Replay.CancellationPending)
                    break;

                this.Invoke(new MethodInvoker(delegate()
                {
                    userControl_PlayBar_CDC.TrackBar_Value = FrameInfo[i].FrameNo;
                }));
                Pause_CDC_Replay.WaitOne(Timeout.Infinite);
                i = iProgess;
                iProgess++;
            }
        }

        private void CDC_Replay_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            userControl_PlayBar_CDC.Stop_API();
            userControl_PlayBar_CDC.ChangeStatus(ButtonStatus.Stop);
        }

        public void CDC_Replay_ThreadStop(bool bUseNewFormat)
        {
            if (Work_CDC_Replay != null)
            {
                if (Work_CDC_Replay.IsBusy)
                    Work_CDC_Replay.CancelAsync();
                MenuItem_DataFormat.Enabled = true;
                CDC_Replay_Pause = false;
                iProgess = 0;

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.bRunningStatus = true;
                bool bEnter = bUseNewFormat;
                if (!bUseNewFormat)
                    bEnter |= (UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame.Count != 0);

                if (bEnter)
                {
                    AssignSelectFrameTo_1St(bUseNewFormat);
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        userControl_PlayBar_CDC.TrackBar_Value = this.userControl_PlayBar_CDC.TrackBar_Min;
                    }));
                    //StartReplay_Frame(this.userControl_PlayBar1.TrackBar_Min);
                }

                this.userControl_PlayBar_CDC.TrackBar_Value = this.userControl_PlayBar_CDC.TrackBar_Min;
                this.userControl_PlayBar_CDC.FrameNum_Text = this.userControl_PlayBar_CDC.TrackBar_Min.ToString();

                UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.bRunningStatus = false;
            }

        }

        private void AssignSelectFrameToBuffer(int SelectIndex, bool bUseNewFormat)
        {
            //Console.WriteLine("AssignSelectFrameToBuffer({0})", SelectIndex);
            if (bUseNewFormat)
            {
                //TODO:使用C-Module的方法將Frame資料讀出來到MutualData裡面                 
                ParseLog._CDC_Datas CDC_Datas = new ParseLog._CDC_Datas();
                _ParseLog.Transfer_CDC(UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.FileEmployee, SelectIndex + 1, ref CDC_Datas);

                var iAA_X = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_X;
                var iAA_Y = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iAA_Y;

                if (ShortType.HasFlag(enShortType.SLK))
                {
                    SHORT_STRUCT tmpSHORT_STRUCT = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Dic_ShortValue[SHORT_NAME.X_SLK];
                    tmpSHORT_STRUCT.ListArray = CDC_Datas.Self_X.Select(x => (int)x).ToList();
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Dic_ShortValue[SHORT_NAME.X_SLK] = tmpSHORT_STRUCT;

                    if (ShortType.HasFlag(enShortType.LK))
                    {
                        tmpSHORT_STRUCT = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Dic_ShortValue[SHORT_NAME.X_LK];
                        tmpSHORT_STRUCT.ListArray = CDC_Datas.Self_LK_X.Select(x => (int)x).ToList();
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Dic_ShortValue[SHORT_NAME.X_LK] = tmpSHORT_STRUCT;
                    }

                    tmpSHORT_STRUCT = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Dic_ShortValue[SHORT_NAME.Y_SLK];
                    tmpSHORT_STRUCT.ListArray = CDC_Datas.Self_Y.Select(x => (int)x).ToList();
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Dic_ShortValue[SHORT_NAME.Y_SLK] = tmpSHORT_STRUCT;

                    if (ShortType.HasFlag(enShortType.LK))
                    {
                        tmpSHORT_STRUCT = UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Dic_ShortValue[SHORT_NAME.Y_LK];
                        tmpSHORT_STRUCT.ListArray = CDC_Datas.Self_LK_Y.Select(x => (int)x).ToList();
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Dic_ShortValue[SHORT_NAME.Y_LK] = tmpSHORT_STRUCT;
                    }
                }
                else
                {
                    var Mutual_CDC_Arr = ArrayTool.ConvertListTo2DArray(CDC_Datas.CDC.Select(x => (int)x).ToList(), iAA_X, iAA_Y);
                    var SelfX_CDC_Arr = CDC_Datas.Self_X.Select(x => (int)x).ToArray();
                    var SelfY_CDC_Arr = CDC_Datas.Self_Y.Select(x => (int)x).ToArray();

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData = Mutual_CDC_Arr;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData[enSelf.Sense] = SelfX_CDC_Arr;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData[enSelf.Drive] = SelfY_CDC_Arr;

                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.SupportPenType != Pen_CDC_Format.enPenType.Disable)
                    {
                        if (CDC_Datas.PEN_CDC_X.Count != 0 && CDC_Datas.PEN_CDC_Y.Count != 0)
                        {
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._X] = CDC_Datas.PEN_CDC_X;
                            UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._Y] = CDC_Datas.PEN_CDC_Y;
                        }
                    }
                                           
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts != 0)
                    {
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualKey = CDC_Datas.Key_MC.Select(x => (int)x).ToArray();
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Sense] = CDC_Datas.Key_SC_X.Select(x => (int)x).ToArray();
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Drive] = CDC_Datas.Key_SC_Y.Select(x => (int)x).ToArray();
                    }

                    //UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.GetFirstFrameData(true);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.MaxMinAnalysis(SelectIndex,true);

                }            
            }
            else
            {
                PLAYBAR_FRAME_STRUCT InPlayBar_Frame = UI_GBV.mDev[m_iSelectedDev].LocalVariable._PlayBar_VAR.PlayBar_Frame[SelectIndex];
                //if(bIsShort)
                if (ShortType != enShortType.None)
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Dic_ShortValue = InPlayBar_Frame.Record_ShortData;
                }
                else
                {
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualData = InPlayBar_Frame.AA_Mutual;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData[enSelf.Sense] = InPlayBar_Frame.AA_Self_X;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfData[enSelf.Drive] = InPlayBar_Frame.AA_Self_Y;

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.MutualKey = InPlayBar_Frame.Key_Mutual;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Sense] = InPlayBar_Frame.Key_Self_X;
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.SelfKey[enSelf.Drive] = InPlayBar_Frame.Key_Self_Y;

                    //20220413 Steven  : 避免陣列沒有初始化導致FAIL(可接受為空值，但不能為null)
                    //1.將_PlayBar_VAR裡面的資料搬出來到Total Buff
                    if (InPlayBar_Frame.Record_PenData != null)
                    {
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._X] = InPlayBar_Frame.Record_PenData[enPenCDC._X];
                        UI_GBV.mDev[m_iSelectedDev].LocalVariable.VarCDCTool_Multi.Orig_PenData[enPenCDC._Y] = InPlayBar_Frame.Record_PenData[enPenCDC._Y];
                    }
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.GetFirstFrameData();
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.MaxMinAnalysis(SelectIndex);
                    //2.將Total Buff資料分類到各自Buff
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._CDCTool_.PenDataClassification_Main();
                }
            }

            enCDCDataFormat dataFormat = (enCDCDataFormat)UI_GBV.mDev[m_iSelectedDev].LocalVariable._CDCTool_VAR.CDCDataType;
            Message _var = new Message();
            if (Work_CDC_Replay == null)
                _var.WParam = (IntPtr)0;
            else
                _var.WParam = (Work_CDC_Replay.IsBusy) ? (IntPtr)1 : (IntPtr)0;
            OnCDCColorGrid(_var);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_CDC_Replay_FrameInfo, (IntPtr)SelectIndex, (IntPtr)((int)dataFormat));

            this.Invoke(new MethodInvoker(delegate()
            {
                this.userControl_PlayBar_CDC.FrameNum_Text = this.userControl_PlayBar_CDC.TrackBar_Value.ToString();
            }));
        }

        private void OnReplay_Frame(Message msg)
        {
            iProgess = (int)msg.WParam;
            AssignSelectFrameToBuffer((int)msg.WParam, userControl_PlayBar_CDC.b_CDC_ReplayUseNewFormat);
        }
        #endregion CDC_Replay

        private void OnCDCColorGrid(Message msg)
        {
            ShowAllNodeColorGrid((int)msg.WParam == 1);
        }

        private void OnCDCStart(Message msg)
        {
            CDC_Toolfm_Init(false);
            CDC_ThreadStart();
        }

        private void OnCDCStop(Message msg)
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                CDC_ThreadStop((int)msg.WParam);
            }));
            //MyTool.WaitUntilFinish(CDC_ThreadStop, 1000);
        }
    }
}