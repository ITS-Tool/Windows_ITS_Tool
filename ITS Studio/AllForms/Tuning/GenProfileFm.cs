using CommonExt;
using FuncMethodCmd;
using FuncMethodCmd.Tools;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ThirdPartyTools;
using System.Runtime.InteropServices;

namespace ITS_Studio.AllForms.Tuning
{
    public enum WM_MSG
    {
        //Common function
        WM_UPDATA_NODE_CHART = 0x0400 + 1,//WM_USER=0x0400;
        WM_CHANGE_RUN_BTN_STATUS,
        WM_SHOW_FILE_NAME,
        WM_LAYOUT_RADIOBUTTON,
        WM_PLEASE_WAIT,
        WM_UPDATA_ByNode_RADIO,
        WM_DISABLE_PAGE,
        WM_ENABLE_PAGE,
        WM_BLOCK_ADD,
        WM_BLOCK_MODIFY,
        WM_ZERO_VALID_FILE_CNT,
        WM_BY_GOLDEN_ENABLE,
    };
    public enum _STATUS_ : int
    {
        SENDING_FILE,
        PROGRAM_SLAVE,
        RECONNECTING,
        SEARCH_DEV,
        FINISH,
        SWITCH_MODE,
        PROGRAM_FAIL,
        SHOW,
        HIDE,
    }
    public partial class GenProfileFm : Form
    {
        
        private DynamicTool MyTool = new DynamicTool();
        private FuncMethodCmd.GBV.GlobeClass m_gbv = null;
        private SensorTestTool m_sensortest = null;
        private SensorTest_AutoGen m_autogen;
        private string m_nowProfile = "";
        private string m_nowLogPath = "";
        private string m_newProfile = ITS_Directory.Tool_CurrentDirectory + @"NewProfile.ini";
        private string m_PathRule = "Relative Path";
        private fmPleaseWait m_PleaseWait = new fmPleaseWait();
        private BackgroundWorker m_bwReadAllFiles;

        private IntPtr m_FormHandle;
        private IntPtr m_panel_Wait_Handle;
        public GenProfileFm()
        {
            InitializeComponent();
        } 
        public void Init()
        {
            m_gbv = UI_GBV.mDev[0].m_GBV.Globe_Multi;
            m_sensortest = new SensorTestTool(0, ref m_gbv);
            m_autogen = new SensorTest_AutoGen(m_sensortest);
            m_FormHandle = this.Handle;
            m_panel_Wait_Handle = panel_Wait.Handle;
        }
        private void btnRun_Click(object sender, EventArgs e)
        {

            m_bwReadAllFiles = new BackgroundWorker();
            m_bwReadAllFiles.WorkerSupportsCancellation = true;
            m_bwReadAllFiles.DoWork += new DoWorkEventHandler(bw_AutoGen);
            m_bwReadAllFiles.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_WorkerCompleted_AutoGen);
            m_bwReadAllFiles.RunWorkerAsync();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            StaticVar.NowExecuteTool = enToolName.Main;

            if (Directory.Exists(m_nowLogPath + @"\GenProfile_Pass"))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(m_nowLogPath + @"\GenProfile_Pass");
                foreach (FileInfo file in di.GetFiles())
                    file.Delete();
                foreach (DirectoryInfo dir in di.GetDirectories())
                    dir.Delete(true);
            }

            UI_GBV.fmITS_Tool.MainButton_Disable();
            UI_GBV.mDev[0].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_tabControlSwitch, (IntPtr)0, (IntPtr)UI_GBV.enMain_tabPage.tabPage_Home);
            UI_GBV.fmITS_Tool.Reset_UI();
        }

        private void btn_LoadProfile_Click(object sender, EventArgs e)
        {
            m_nowProfile = "";
            if (!SelectFile("Profile (*.ini)|*.ini;", Directory.GetCurrentDirectory() + @"\Profile", out m_nowProfile, true))
            {
                return;
            }
            if (!Path.IsPathRooted(m_nowProfile))
                m_nowProfile = MyTool.CombineAbsolutePath(ITS_Directory.Tool_CurrentDirectory, m_nowProfile);
            Txt_ProfilePath.Text = m_nowProfile;
            Get_ReportPath_FromIni(m_nowProfile);
            Action(true);

        }

        private void btn_Report_Click(object sender, EventArgs e)
        {
            ChangeCollectionPath(m_PathRule, ref m_nowLogPath);
            if (!Path.IsPathRooted(m_nowLogPath))
                m_nowLogPath = MyTool.CombineAbsolutePath(ITS_Directory.Tool_CurrentDirectory, m_nowLogPath);
            Txt_ReportPath.Text = m_nowLogPath;
            Action(true);
        }
        protected override void WndProc(ref Message m)
        {
            //const int WM_HOTKEY = 0x0312;
            WM_MSG pEnumPanoMsg = (WM_MSG)m.Msg;
            if (Utility.mMessageReceiver.Keys.Contains(pEnumPanoMsg))
                Utility.mMessageReceiver[pEnumPanoMsg](ref m);

            if (m.Msg == (int)WM_MSG.WM_PLEASE_WAIT)
            {
                OnShow_PleaseWait(ref m);

            }
            base.WndProc(ref m);
        }
        private void OnShow_PleaseWait(ref Message msg)
        {
            if (((_STATUS_)msg.WParam) == _STATUS_.HIDE)
            {
               // splitContainer4.Panel2Collapsed = true;
                if (this.InvokeRequired)
                    this.Invoke(new MethodInvoker(delegate() { m_PleaseWait.Close(); }));
                else
                    m_PleaseWait.Close();
            }
            else if (((_STATUS_)msg.WParam) == _STATUS_.SHOW)
            {
                //splitContainer4.Panel2Collapsed = false;
                if (this.InvokeRequired)
                {
                    if (msg.LParam == IntPtr.Zero)
                    {
                        this.Invoke(new MethodInvoker(delegate()
                        {
                            m_PleaseWait = new fmPleaseWait();
                            m_PleaseWait.Show();
                        }));
                    }
                    else
                    {
                        m_PleaseWait = new fmPleaseWait(msg.LParam);
                        m_PleaseWait.Show();
                    }
                }
                else
                {
                    m_PleaseWait = (msg.LParam == IntPtr.Zero) ? new fmPleaseWait() : new fmPleaseWait(msg.LParam);
                    m_PleaseWait.Show();
                }
            }
        }
#region Func
        private void bw_AutoGen(object sender, DoWorkEventArgs e)
        {
            UIPage MyUIPage = new UIPage();
            Win32API.SendMessage(m_FormHandle, WM_MSG.WM_PLEASE_WAIT, (IntPtr)_STATUS_.SHOW, m_panel_Wait_Handle);
            
            string message = "";
            if(StaticVar.g_Lang == enLangTable.LANG_ENGLISH)
                message = string.Format("Auto Gen. Function is done.\r\nThe new profile is in {0}", m_newProfile);
            else
                message = string.Format("Auto Gen. 執行完畢.\r\n產生的新profile在此路徑: {0}", m_newProfile);
            
            MyUIPage.ShowSuccessDialog(message, true);
        }
        
        private void bw_WorkerCompleted_AutoGen(object sender, RunWorkerCompletedEventArgs e)
        {
            Win32API.SendMessage(m_FormHandle, WM_MSG.WM_PLEASE_WAIT, (IntPtr)_STATUS_.HIDE, m_panel_Wait_Handle);
        }
        
        private void ChangeCollectionPath(string path_rule, ref string strReportPath)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.FileName = "Filename will be ignored";
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.ShowReadOnly = false;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.InitialDirectory = ITS_Directory.Tool_CurrentDirectory;
            openFileDialog1.ValidateNames = false;
            string folderPath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                folderPath = Path.GetDirectoryName(openFileDialog1.FileName);
            else
                return;

            var tmpItem = EnumTool.GetValueFromDescription<enReportPathRule>(path_rule);
            if (folderPath != "")
            {
                if (!Path.IsPathRooted(folderPath))
                    strReportPath = MyTool.CombineAbsolutePath(ITS_Directory.Tool_CurrentDirectory, folderPath);
                else
                    strReportPath = folderPath;
            }
        }

        private bool SelectFile(string strFilter, string strInitialDirectory, out string FileName, bool CheckFileExists)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = strFilter;  //在對話方塊中可以使用檔案篩選選項
            openDlg.InitialDirectory = strInitialDirectory;//執行檔資料夾的位置
            openDlg.FileName = "";                     //取得或設定含有檔案對話方塊中所選取檔名的字串
            openDlg.CheckFileExists = CheckFileExists;      //如果當使用者指定不存在的檔名時對話方塊顯示警告訊息，屬性值為 true
            openDlg.CheckPathExists = true;    //如果當使用者指定了不存在的路徑時要顯示警告對話方塊，屬性值為 true
            openDlg.Multiselect = false;              //如果對話方塊允許一起或同時選取多個檔案，則為 true
            FileName = "";
            if (openDlg.ShowDialog(this) != DialogResult.OK)
                return false;
            FileName = openDlg.FileName;
            return true;
        }

        private bool GetProfilePath()
        {
            bool ret = false;
            INI_Tool HistoryINI = new INI_Tool(ITS_Directory.History_INI);
            string strProfilePath = HistoryINI.IniReadValue("SensorTest_ProfilePath", "Path", "");
            m_nowProfile = (Path.IsPathRooted(strProfilePath)) ? strProfilePath : ITS_Directory.Tool_CurrentDirectory + strProfilePath;
            if (!File.Exists(m_nowProfile))
                return false;
            Match rxMatch = Regex.Match(m_nowProfile, @"(\\|/)(.*.ini)");
            string RelativePath = "";
            if (rxMatch.Success)
            {
                RelativePath = string.Format("{0}", MyTool.GetRelativePath(ITS_Directory.Tool_CurrentDirectory, m_nowProfile));
                Get_ReportPath_FromIni(m_nowProfile);
                ret = true;
            }
            else
                RelativePath = string.Format("Error : {0}", strProfilePath);
            if (!Path.IsPathRooted(RelativePath))
                RelativePath = MyTool.CombineAbsolutePath(ITS_Directory.Tool_CurrentDirectory, RelativePath);
            Txt_ProfilePath.Text = RelativePath;
            return ret;
        }

        public void Action(bool from_button)
        {
            Txt_NewProfilePath.Text = m_newProfile;
            btnRun.Enabled = true;
            if (!from_button)
                btnRun.Enabled = GetProfilePath();
            if (btnRun.Enabled)
            {
                //m_autogen.GenVal_fromIni(m_nowProfile);
                // m_autogen.ChangeCollectionPath("Absolute Path", ref m_newLogPath);
                m_autogen.m_setting.log_path = m_nowLogPath;
                m_autogen.m_setting.enable = true;
                //
                int all=0;
                int valid = GetVaildLogCnt(ref all);
                Txt_totalFiles.Text = all.ToString();
                Txt_validFiles.Text = valid.ToString();
                if (Txt_validFiles.Text == "0")
                    btnRun.Enabled = false;
                else
                    btnRun.Enabled =true;
            }
        }

        //取得蒐集log時所使用的profile的內容
        private bool get_profile_val(ref string ictype, ref int xch, ref int ych, ref int short_thr, ref int open_thr, ref List<MultiSenTest_VAR._RawGolden_> uni_thr, ref Int32[] crc, ref bool fw_verify)
        {
            INI_Tool ini = new INI_Tool(m_nowProfile);
            ictype = ini.IniReadValue("IC_Verify", "IC_Type", "");
            string val = ini.IniReadValue("Panel_Info", "XChannel", "");
            xch = Int16.Parse(val.ToString());
            val = ini.IniReadValue("Panel_Info", "YChannel", "");
            ych = Int16.Parse(val.ToString());
            //short
            val = ini.IniReadValue("Short_Test", "Max_Threshold", "");
            short_thr = Int16.Parse(val.ToString());
            //open
            val = ini.IniReadValue("Open_Test", "Min_Threshold", "");
            open_thr = Int16.Parse(val.ToString());
            //uniformity
            Get_UniformityRawData(ref uni_thr);
            //fw verify crc
            val = ini.IniReadValue("FW_Verify", "Master_CRC", "");
            string[] profile_crc = val.ToString().TrimEnd(',').Replace(" ", "").Replace("0x", "").Split(',').ToArray();
            for (int i = 0; i < profile_crc.Count(); i++)
                crc[i] = Int32.Parse(profile_crc[i], System.Globalization.NumberStyles.HexNumber);
            //fw enable
            val = ini.IniReadValue("FW_Verify", "Enable", "");
            fw_verify = (val == "True");
            //Uniformity_Judgement
            val = ini.IniReadValue("Auto_Gen.", "Uniformity_Judgement", "");
            m_autogen.m_setting.uniformity_judgement = (val == "True");
            //new profile name
            string rule = ini.IniReadValue("Auto_Gen.", "PathRule", "");
            val = ini.IniReadValue("Auto_Gen.", "NewProfileName", "");

            return true;
        }

        private void Get_UniformityRawData(ref List<MultiSenTest_VAR._RawGolden_> table)
        {
            table = new List<MultiSenTest_VAR._RawGolden_>();
            INI_Tool ini = new INI_Tool(m_nowProfile);
            List<string> ReadedList = ini.IniReadAllContains("Uniformity_RawData");
            if (ReadedList == null)
                return;

            foreach (string tmp1 in ReadedList)
            {
                if (tmp1.IndexOf("#") != -1)
                    continue;
                string[] strArray = tmp1.Split(';');
                MultiSenTest_VAR._RawGolden_ Value_item = new MultiSenTest_VAR._RawGolden_();
                Value_item.MaxValue = new List<string>();
                Value_item.Value = new List<string>();
                Value_item.MinValue = new List<string>();
                Value_item._DefineFunc_ = new List<MultiSenTest_VAR._DefineFunction>();
                foreach (string node in strArray)
                {
                    if (string.IsNullOrEmpty(node))
                        continue;

                    string[] values = node.Split(',');
                    if (values.Length > 3)
                    {
                        Value_item.Value.Add(values[0]);
                        Value_item.MaxValue.Add(values[1]);
                        Value_item.MinValue.Add(values[2]);
                        int iFunc_Value = 0;
                        int.TryParse(values[3], out iFunc_Value);
                    }
                }
                table.Add(Value_item);
            }
        }
        
        private bool Get_ReportPath_FromIni(string profile)
        {
            bool ret = false;

            INI_Tool ini = new INI_Tool(profile);
            m_PathRule = ini.IniReadValue("Auto_Gen.", "PathRule", "");
            m_nowLogPath = ini.IniReadValue("Auto_Gen.", "CollectionPath", "");
            if (!Path.IsPathRooted(m_nowLogPath))
                m_nowLogPath = MyTool.CombineAbsolutePath(ITS_Directory.Tool_CurrentDirectory, m_nowLogPath);

            Txt_ReportPath.Text = m_nowLogPath;

            if (m_nowLogPath != "")
                ret = true;
            return ret;
        }

        private int GetVaildLogCnt(ref int total_files)
        {
            total_files = 0;
            //
            string pass_folder = m_nowLogPath + "\\GenProfile_Pass\\";
            if (!Directory.Exists(pass_folder))
                Directory.CreateDirectory(pass_folder);
            else
            {
                DirectoryInfo directory = new DirectoryInfo(pass_folder);
                foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            }
            DirectoryInfo di = new DirectoryInfo(m_nowLogPath);
            SearchOption op = SearchOption.TopDirectoryOnly;
            if (chk_recursive.Checked)
                op = SearchOption.AllDirectories;

            var fileCount = (from file in di.EnumerateFiles("*.csv", op)
                             select file).Count();
            //
            total_files = fileCount;
            string ICType = "";
            int xch = 0, ych = 0, short_thr = 0, open_thr = 0;
            Int32[] crc = new Int32[10];
            bool fw_verify = false;
            int total = 0;
            List<MultiSenTest_VAR._RawGolden_> uni_thr = new List<MultiSenTest_VAR._RawGolden_>();
            get_profile_val(ref ICType, ref xch, ref ych, ref short_thr, ref open_thr, ref uni_thr, ref crc, ref fw_verify);

            foreach (var fi in di.EnumerateFiles("*.csv", op))
            {
                if (fi.Extension == ".csv" && fi.DirectoryName.Contains("GenProfile_Pass")==false)
                {
                    if (m_autogen.judgy_collection_log(fi.FullName, ICType, xch, ych, short_thr, open_thr, uni_thr, crc, fw_verify))
                    {
                        File.Copy(fi.FullName, m_nowLogPath + "\\GenProfile_Pass\\" + Path.GetFileName(fi.FullName), true);
                        total++;
                    }
                }
            }
           
            return total;
        }
        ////將取得蒐集log時所使用的profile的內容拿來判斷CSV檔是否為pass
        ////@retrun bool: true:pass false:fail
        //public bool judgy_collection_log(string log, string ictype, int xch, int ych, int short_thr, int open_thr, List<MultiSenTest_VAR._RawGolden_> uni_thr, Int32[] crc, bool fw_verify)
        //{
        //    bool ret = true;
        //    if (!m_sensortest.LoadReport(log, true))
        //        return false;
        //    //check ic type, xch, ych
        //    TestItemNameDefine type = TestItemNameDefine.enPanel_Info;
        //    string[] data;
        //    int idx = m_sensortest._ReportInfo.FindIndex(x => x.Contains("Test_Station"));
        //    data = m_sensortest._ReportInfo[idx].Trim().Replace(" ", "").Replace("*", "").Split(',').ToArray();
        //    if ("FPC" == data[1])
        //        return false;
        //    if (!m_sensortest._Report.ContainsKey(type))
        //        return false;
        //    idx = m_sensortest._Report[type].FindIndex(x => x.Contains("IC_Type"));
        //    data = m_sensortest._Report[type][idx].Trim().Replace(" ", "").Replace("*", "").Split(',').ToArray();
        //    if (ictype != data[1])
        //        return false;
        //    if (!m_sensortest._Report.ContainsKey(type))
        //        return false;
        //    idx = m_sensortest._Report[type].FindIndex(x => x.Contains("XChannel"));
        //    data = m_sensortest._Report[type][idx].Trim().Replace(" ", "").Replace("*", "").Split(',').ToArray();
        //    if (xch != Convert.ToInt32(data[1]))
        //        return false;
        //    if (!m_sensortest._Report.ContainsKey(type))
        //        return false;
        //    idx = m_sensortest._Report[type].FindIndex(x => x.Contains("YChannel"));
        //    data = m_sensortest._Report[type][idx].Trim().Replace(" ", "").Replace("*", "").Replace("*", "").Split(',').ToArray();
        //    if (ych != Convert.ToInt32(data[1]))
        //        return false;

        //    //get fw verify
        //    if (fw_verify)
        //    {
        //        type = TestItemNameDefine.enUpgrade;
        //        if (!m_sensortest._Report.ContainsKey(type))
        //            return false;
        //        var indexes = m_sensortest._Report[type].Select((element, index) => element.Trim().Contains("IC Verify Code") ? index : -1).Where(i => i >= 0).ToArray();
        //        if (indexes.Count() != 2)
        //        {
        //            return false;
        //        }
        //        data = m_sensortest._Report[type][indexes[1]].Trim().TrimEnd(',').Replace(" ", "").Replace("0x", "").Split(',').ToArray();


        //        Int32[] i_crc = new Int32[data.Count()];
        //        for (int i = 0; i < crc.Count(); i++)
        //        {
        //            if (crc[i] != 0 && (data.Count() > i + 1))
        //            {
        //                i_crc[i + 1] = Int32.Parse(data[i + 1], System.Globalization.NumberStyles.HexNumber);
        //                if (i_crc[i + 1] != crc[i])
        //                    return false;
        //            }
        //        }

        //    }
        //    //get short data
        //    type = TestItemNameDefine.enShortTest;
        //    if (!m_sensortest._Report.ContainsKey(type))
        //        return false;
        //    idx = m_sensortest._Report[type].FindLastIndex(x => x.Contains("X_SLK"));
        //    if (idx == -1)
        //        return false;
        //    data = m_sensortest._Report[type][idx].Trim().Replace(" ", "").Replace("*", "").Replace("#", "").Split(',').ToArray();
        //    for (int i = 1; i < data.Count() - 2; i++)
        //    {
        //        if (Convert.ToInt32(data[i]) > short_thr)
        //            return false;
        //    }
        //    idx = m_sensortest._Report[type].FindLastIndex(x => x.Contains("Y_SLK"));
        //    if (idx == -1)
        //        return false;
        //    data = m_sensortest._Report[type][idx].Trim().Replace(" ", "").Replace("*", "").Replace("#", "").Split(',').ToArray();
        //    for (int i = 1; i < data.Count() - 2; i++)
        //    {
        //        if (Convert.ToInt32(data[i]) > short_thr)
        //            return false;
        //    }
        //    //get open data
        //    type = TestItemNameDefine.enOpenTest;
        //    if (!m_sensortest._Report.ContainsKey(type))
        //        return false;
        //    idx = m_sensortest._Report[type].FindIndex(x => x.Contains("Y_"));
        //    if (idx == -1)
        //        return false;
        //    var ReportRaw = m_sensortest._Report[type].GetRange(idx, ych);
        //    int iRow = 0;
        //    foreach (var _row in ReportRaw)
        //    {
        //        var _new_row = Regex.Replace(_row, @"\s", "");
        //        var tmpArr = _new_row.Replace("*", "").Replace("#", "").Split(',');
        //        foreach (var cell in tmpArr)
        //        {
        //            int iNum = 0;
        //            if (!int.TryParse(cell, out iNum))
        //                continue;
        //            if (iNum < open_thr)
        //                return false;
        //        }
        //        iRow++;
        //    }
        //    if (!m_Uniformity_Judgement)
        //        return true;
        //    //get uniformity data
        //    type = TestItemNameDefine.enUniformityTest;
        //    if (!m_sensortest._Report.ContainsKey(type))
        //        return false;
        //    idx = m_sensortest._Report[type].FindLastIndex(x => x.Trim().Replace(" ", "").Replace("*", "").Contains("RawData_Max,(X"));
        //    if (idx == -1)
        //        return false;
        //    ReportRaw = m_sensortest._Report[type].GetRange(idx + 1, ych);
        //    iRow = 0;
        //    //int[,] OneFrame = new int[XCH, YCH];
        //    foreach (var _row in ReportRaw)
        //    {
        //        var _new_row = Regex.Replace(_row, @"\s", "");
        //        var tmpArr = _new_row.Replace("*", "").Replace("#", "").Split(',');
        //        int iCell = 0;
        //        foreach (var cell in tmpArr)
        //        {
        //            int iNum = 0;
        //            if (!int.TryParse(cell, out iNum))
        //                continue;
        //            //OneFrame[iCell++, iRow] = iNum;
        //            if (iNum > Convert.ToInt32(uni_thr[iRow].MaxValue[iCell])
        //                || iNum < Convert.ToInt32(uni_thr[iRow].MinValue[iCell]))
        //                return false;
        //            iCell++;
        //        }
        //        iRow++;
        //    }
        //    return ret;
        //}
#endregion

        private void btn_NewProfilePath_Click(object sender, EventArgs e)
        {
            if (!SelectFile("Profile (*.ini)|*.ini;", Directory.GetCurrentDirectory() + @"\Profile", out m_newProfile, false))
            {
                return;
            }
            Txt_NewProfilePath.Text = m_newProfile;
        }

        private void chk_recursive_CheckedChanged(object sender, EventArgs e)
        {
            int all = 0;
            int valid = GetVaildLogCnt(ref all);
            Txt_totalFiles.Text = all.ToString();
            Txt_validFiles.Text = valid.ToString();
            if (Txt_validFiles.Text == "0")
                btnRun.Enabled = false;
            else
                btnRun.Enabled = true;
        }
    }

    public class Win32API
    {
        /// <summary>
        /// 自定義的結構
        /// </summary>
        public struct My_lParam
        {
            public string s;
            public int array_idx;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, WM_MSG Msg, IntPtr wParam, ref My_lParam lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, WM_MSG Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, WM_MSG Msg, IntPtr wParam, Block_Detail lParam);

        //Import the RegisterHotKey Method
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        //支援簡中,繁中
        public static string Str_To_Unicode(string inStr)
        {
            byte[] textbuf = UnicodeEncoding.Unicode.GetBytes(inStr);
            string textAscii = string.Empty;//用来存储转换过后的ASCII码
            for(int i = 0; i < textbuf.Length; i++)
            {
                textAscii += textbuf[i].ToString("X2");
            }
            return textAscii;
        }

        public static string Unicode_To_Str(string inStr)
        {
            string textStr = string.Empty;
            int k = 0;//字节移动偏移量
            byte[] buffer = new byte[inStr.Length / 2];//存储变量的字节
            for(int i = 0; i < inStr.Length / 2; i++)
            {
                //每两位合并成为一个字节
                buffer[i] = byte.Parse(inStr.Substring(k, 2), System.Globalization.NumberStyles.HexNumber);
                k = k + 2;
            }
            //将字节转化成汉字
            textStr = UnicodeEncoding.Unicode.GetString(buffer);

            return textStr;
        }
    }
    public class Block_TH
    {
        public string max_th;
        public string min_th;
        public double[] limit_max_th = new double[4];//紀錄每個node的最大bias thr
        public double[] limit_min_th = new double[4];//紀錄每個node的最小bias thr

        public Block_TH ShallowCopy()
        {
            return (Block_TH)this.MemberwiseClone();
        }
    };
    public class Block_Detail
    {
        public int _idx;
        public int sx;
        public int sy;
        public int ex;
        public int ey;
        public Block_TH thr = new Block_TH();
    };

    public static class Utility
    {
        public delegate void function(ref Message msg);

        public static Dictionary<WM_MSG, function> mMessageReceiver = new Dictionary<WM_MSG, function>();

        public static string GetDescriptionFromEnumValue(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if(!type.IsEnum)
                throw new InvalidOperationException();
            foreach(var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if(attribute != null)
                {
                    if(attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if(field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", description);
            // or return default(T);
        }
    }
}
