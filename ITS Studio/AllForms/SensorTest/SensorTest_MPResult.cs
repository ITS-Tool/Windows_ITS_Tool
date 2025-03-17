using CommonExt;
using System;
using System.Windows.Forms;

namespace ITS_Studio.AllForms.SensorTest
{
    public partial class SensorTest_MPResult : Form
    {
        private int m_iSelectedDev = 0;
        public string ErrorMessage = "";
        public SensorTest_MPResult()
        {
            InitializeComponent();
            richTB_MPResultMesg.Clear();

        }

        private void SensorTest_MPResult_Load(object sender, EventArgs e)
        {
            try
            {
                if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.ProtocolType == enPROTOCOL.V6)
                {
                    if (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType == DEV_INTERFACE_TYPE.I2C)
                        UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.BridgeSettingInit_API(StaticVar.bridgeSetting.m_bridge_var.iPower_On_DelayTime);
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.g_OpenReadMonitor();

                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode = UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.CheckDevMode_API(FW_MODE.AP_MODE) ? FW_MODE.AP_MODE : FW_MODE.BL_MODE;
                    var _NowMode = UI_GBV.mDev[m_iSelectedDev].LocalVariable.FW_Upgrade_DevStatus.Master_Mode;

                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.Get_IC_Information(_NowMode
                             , false, ref UI_GBV.mDev[m_iSelectedDev].m_GBV.ConsoleTool.strInforMessage, ref UI_GBV.mDev[m_iSelectedDev].m_GBV.ConsoleTool.strErrorMessage);

                    DEV_INTERFACE_TYPE profileInterface = (DEV_INTERFACE_TYPE)Enum.Parse(typeof(DEV_INTERFACE_TYPE), StaticVar.strInterface);
                    this.DialogResult = DialogResult.None;

                    if (StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType != profileInterface) //實際Interface和Profile不符合
                    {
                        //MessageBox.Show("Please confirm that the actual interface and profile settings are the same\r\n or please reload panel information.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //ErrorMessage = "Please confirm that the Protocol Version and profile settings are the same or please reload panel information.\r\n";
                        if (StaticVar.g_Lang != enLangTable.LANG_ENGLISH)
                            ErrorMessage = "請確認實際Interface和Profile設定是否相同\r\n或是請重新Load Panel Information\n";
                        else
                            ErrorMessage = "Please confirm whether the actual Interface and Profile settings are the same or please reload panel information.\n";
                        ErrorMessage += string.Format("profile Interface : {0}\r\n", profileInterface);
                        ErrorMessage += string.Format("Device Interface : {0}\r\n", StaticVar.lsMain_USBDevInfo[m_iSelectedDev].stuDeviceType);
                        this.DialogResult = DialogResult.Abort;
                        this.Close();
                        return;
                    }
                    //string sActualProtocolVersion = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNumArr[0].ToString()+"."+UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNumArr[1].ToString()+"."+UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNumArr[2].ToString();

                    //Console.Write(sActualProtocolVersion);
                    //var _hexStr = StringTool.VersionToHexString(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum);
                    bool bComapreOK = UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum.Major != 0x00;
                    bComapreOK &= StaticVar.INI_ProtocolVersion.Major != 0x00;
                    bComapreOK &= UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum == StaticVar.INI_ProtocolVersion;
                    //if(UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum != StaticVar.INI_ProtocolVersion)
                    if(!bComapreOK)
                    {
                        if (StaticVar.g_Lang != enLangTable.LANG_ENGLISH)
                            ErrorMessage = "請確認實際Protocol Version和Profile設定相同\n或是請重新Load Panel Information\n";
                        else
                            ErrorMessage = "Please confirm whether the Protocol Version and Profile settings are the same or please reload panel information.\n";

                        ErrorMessage += string.Format("profile Protocol_VerNum : {0}\n", StaticVar.INI_ProtocolVersion);
                        ErrorMessage += string.Format("Device Protocol_VerNum : {0}\n", UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.Protocol_VerNum);
                        this.DialogResult = DialogResult.Abort;
                        this.Close();
                        return;
                    }

                    string msg;
                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.read_then_decode_mp_result(out msg);
                    richTB_MPResultMesg.Text = msg;
                }
                else
                {
                    if (StaticVar.g_Lang != enLangTable.LANG_ENGLISH)
                        ErrorMessage = "V3 IC不支援讀取MP Result\n";
                    else
                        ErrorMessage = "V3 IC is not suppot read MP Result\n";

                    this.DialogResult = DialogResult.Abort;
                    this.Close();
                    return;

                   // MessageBox.Show("V3 IC不支援讀取MP Result", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                   
            }
            catch (Exception)
            {
                if (StaticVar.g_Lang != enLangTable.LANG_ENGLISH)
                    ErrorMessage = "請確認接上的IC和Profile設定相同\n或是請重新Load Panel Information\n";
                else
                    ErrorMessage = "Please confirm that the connected IC and Profile settings are the same or please reload panel information.\n";

                this.DialogResult = DialogResult.Abort;
                this.Close();
                return;

                //MessageBox.Show("請確認接上的IC和Profile設定相同\n或是請重新Load Panel Information\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        //private string VersionToHexString(Version version)
        //{
        //    // 使用 ToString 方法，指定 "X" 格式，即十六進位格式
        //    // 這裡使用 2 位數字的每一部分
        //    if(version.Revision == -1)
        //        return string.Format("{0:X}.{1:X}.{2:X}", version.Major, version.Minor, version.Build);
        //    else
        //        return string.Format("{0:X}.{1:X}.{2:X}.{3:X}", version.Major, version.Minor, version.Build, version.Revision);
        //}

    }
}
