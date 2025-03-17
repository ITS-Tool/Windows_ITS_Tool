using CommonExt;
using FuncMethodCmd;
using System;
using System.Windows.Forms;

namespace ITS_Studio.AllForms
{
    public partial class fmExternalPaint : Form
    {
        private int m_iSelectedDev = 0;
        public int SelectedDev { set { m_iSelectedDev = value; } get { return m_iSelectedDev; } }

        public fmExternalPaint()
        {
            InitializeComponent();
            this.KeyPreview = true;//跟父視窗同步熱鍵，要將這個設true才有作用
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private void fmExternalPaint_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Quit Painting
                case Keys.Escape:
                case Keys.Q:
                    if (e.KeyCode == Keys.Q)
                        if (!e.Control)
                            break;

                    //TODO: [Joe] StopPaint 提前主要是確認 wifi painting flow 能夠先通知daemon
                    ILITek_ITS_Tool.fm_PaintTool.StopPaint();

                    UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);
                    this.Hide();
                    UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintBackToHome, (IntPtr)0, (IntPtr)0);
                    break;
           
                case Keys.Space:
                case Keys.C:// Clear Painting
                    if (UI_GBV.mDev[m_iSelectedDev].LocalVariable.PaintTool_Var.CoordinateDisplayMode == DataDisplayMode.FullScreen)
                        BTN_Clear_Screen_Click(sender, null);
                    break;
            }
        }

        public void BTN_Clear_Screen_Click(object sender, EventArgs e)
        {
            ILITek_ITS_Tool.fm_PaintTool.m_panAAscen.Refresh();
            //ILITek_ITS_Tool.fm_PaintTool.InitVirtualButton();
            ILITek_ITS_Tool.fm_PaintTool.vfChangeResolution(); //Steven : InitVirtualButton 已整合到vfChangeRes
            
            if ((UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.DevPanelInfo.iKeyAmounts > 0) &&
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi.m_Flow.isI2C(StaticVar.lsMain_USBDevInfo[0].stuDeviceType))
                ILITek_ITS_Tool.fm_PaintTool.PaintKeyRect();
        }

        public void BTN_Exit_Click(object sender, EventArgs e)
        {
            //TODO: [Joe] StopPaint 提前主要是確認 wifi painting flow 能夠先通知daemon
            ILITek_ITS_Tool.fm_PaintTool.StopPaint();
            
            if (StaticVar.iNowDeviceCnts != 0)
                UI_GBV.mDev[m_iSelectedDev].m_GBV.Globe_Multi._DeviceMain.SetTouchMode_API(TouchMode.NormalMode, Command._SWITCH.Dis);

            // 0x03, 0xA3, 0x03, 0x00, 0xF0, 0x00
            //GBV.Globe_Multi[0].Flow.SetTouchMode(TouchMode.NormalMode, Command._SWITCH.Dis);
            this.Hide();
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.Var_Multi.MultiDev_SendMSG(StaticVar.MainFormHandle, enWM_MSG.WM_PaintBackToHome, (IntPtr)0, (IntPtr)0);
        }

        private void fmExternalPaint_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}