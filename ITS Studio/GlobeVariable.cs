using CommonExt;
using ITS_Studio.AllForms;
using System.Windows.Forms;
using System;
using System.Collections.Generic;


namespace ITS_Studio
{
    public static class UI_GBV
    {
        public static ILITek_ITS_Tool_Lib.CtrlILIDevice[] mDev;
        public static ILITek_ITS_Tool fmITS_Tool;
        public static fmSensorTest_Painting fm_SensorTest_Painting;
        public static string LogAnalysisTool = AppDomain.CurrentDomain.BaseDirectory +@"LogAnalysisTool\LogAnalysis_Tool.exe";
        
        public enum enMain_tabPage
        {
            tabPage_Home = 0,
            tabPage_Console,
            tabPage_PaintTool,
            tabPage_CDCTool,
            tabPage_FWUpgrade,
            tabPage_Tuning,
            tabPage_SensorTest_D,
            tabPage_SensorTest_S,
            tabPage_CModel,
            tabPage_CModel_Replay,
        }

        public class ToolMain
        {
            public static Dictionary<Tool_BTNs,Button> btnMainButtons = null;
            public static RichTextBox m_RichText_ERR_MSG = new RichTextBox();
            public static RichTextBox m_RichText_INFO_MSG = new RichTextBox();
            public static TextBox lbMax_Value = new TextBox();
            public static TextBox lbMin_Value = new TextBox();
            public static TextBox lbDelta_Value = new TextBox();
        }
    }
}