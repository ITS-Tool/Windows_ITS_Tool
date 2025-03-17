using FuncMethodCmd.Tools;
using Sunny.UI;
using System;
using System.Windows.Forms;

namespace ITS_Studio.AllForms.SensorTest
{
    public partial class SensorTest_AutoGen_Thr_Setting : Form
    {
        private SensorTest_AutoGen m_autogen;
        public SensorTest_AutoGen_Thr_Setting(ref SensorTest_AutoGen autogen, ref bool file_exist, string profile)
        {
            InitializeComponent();
            m_autogen = autogen;
            if (m_autogen.GetCurrect_LogToolThr(profile) == false)
            {
                UIPage MyUIPage = new UIPage();
                MyUIPage.ShowErrorDialog("Error", string.Format("{0} cann't be found\r\n", CommonExt.ITS_Directory.AnalyzerSetting_path), UIStyle.Red, true);
                file_exist = false;
            }
            else
            {
                btn_uniformity_en.Active = m_autogen.m_setting.uniformity_judgement;
                ShowData(m_autogen.m_thr_use);
                file_exist = true;
            }
            //
        }

        private void uiBtn_default_Click(object sender, EventArgs e)
        {
            btn_uniformity_en.Active = false;
            ShowData(m_autogen.m_thr_def);
        }

        private void ShowData(AutoGenThr thr)
        {
            uicb_openthr.Text = thr.open.ToString();
        }

        private void uibtn_ok_Click(object sender, EventArgs e)
        {
            m_autogen.m_thr_use.open = Convert.ToInt32(uicb_openthr.Text);
            m_autogen.m_setting.uniformity_judgement = btn_uniformity_en.Active;
            Close();
        }

        private void uibtn_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_uniformity_en_ValueChanged(object sender, bool value)
        {
            var _sender = sender as UISwitch;
        }
    }
}
