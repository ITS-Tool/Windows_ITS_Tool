using CommonExt;
using Sunny.UI;
using System;
using System.Windows.Forms;
using ThirdPartyTools;

namespace ITS_Studio.AllForms
{
    public partial class MainSetting : Form
    {
        private INI_Tool MY_INI = new INI_Tool(ITS_Directory.Setting_INI);

        public MainSetting()
        {
            InitializeComponent();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            UISymbolButton myBtn = sender as UISymbolButton;

            if (myBtn == btn_OP_Mode)
                MY_INI.IniWriteValue("Main", "Mode", enMode.OPMode.ToString());
            else if (myBtn == btn_RD_Mode)
                MY_INI.IniWriteValue("Main", "Mode", enMode.RDMode.ToString());
            else if (myBtn == btn_Multi_OP_Mode)
                MY_INI.IniWriteValue("Main", "Mode", enMode.Multi_OPMode.ToString());

            vfRestart();
        }

        private void vfRestart()
        {
            Program.KeepRunning = true;
            Application.Restart();
            Application.Exit();
            Environment.Exit(0);
        }

        private void MainSetting_Shown(object sender, EventArgs e)
        {
            string str = MY_INI.IniReadValue("Main", "Mode", "RDMode");
            this.Text = string.Format("Now is {0}", str);
            var _list = MY_INI.IniReadAllKeys("DisplayFun");

            foreach (var item in EnumTool.EnumToList<Tool_BTNs>())
            {
                if(!UI_GBV.ToolMain.btnMainButtons.ContainsKey(item))
                    continue;
                switch (item)
                {
                    case Tool_BTNs.BTN_SENSORTEST_S:
                        btn_Multi_OP_Mode.Visible = _list.Contains(item.ToString());
                        break;
                    case Tool_BTNs.BTN_SENSORTEST_D:
                        btn_OP_Mode.Visible = _list.Contains(item.ToString());
                        break;
                }

            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}