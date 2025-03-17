
using CommonExt;
using Sunny.UI;
using System;
using System.Windows.Forms;
using ThirdPartyTools;

namespace ITS_Studio.AllForms
{
    public partial class BridgeSetting : Form
    {
        private int m_iSelectedDev = 0;
        public int SelectedDev { set { m_iSelectedDev = value; } get { return m_iSelectedDev; } }

        public INI_Tool My_INI;

        public BridgeSetting()
        {
            InitializeComponent();

            //tabControl1.Appearance = TabAppearance.FlatButtons;
            //tabControl1.SizeMode = TabSizeMode.Fixed;
            //tabControl1.ItemSize = new Size(0, 1);
            tabControl_Interface.TabVisible = false;
        }

        public void Update_rTextBox_Current(string msg)
        {
            rTextBox_Current.Text = msg;
        }

        public void Enable_rTextBox_Current(bool en)
        {
            //groupBox_Current.Visible = en;
            uiLabel_Curr.Visible = en;
            rTextBox_Current.Visible = en;
            label1.Visible = en;
        }

        public void Clean_rTextBox_Current()
        {
            rTextBox_Current.Text = "0";
        }
        
        public void Load_INI_Settings()
        {
            My_INI = new INI_Tool(ITS_Directory.Setting_INI);
            LoadBridgeSettings();
            m_cmbVDDIO.SelectedIndexChanged += new System.EventHandler(this.m_cmbVDDIO_SelectedIndexChanged);
            ckBox_RepeatStart.Checked = StaticVar.bridgeSetting.m_bridge_var.bRepeatStart;
            ckBox_PwrEnRST.Checked = StaticVar.bridgeSetting.m_bridge_var.bBridges_Pwr_Reset;
            txtBox_HostDelay.Text = StaticVar.bridgeSetting.m_bridge_var.HostDelay.ToString();
        }

        private bool LoadBridgeSettings()
        {
            // I2C Clock
            m_cmbBridgeClock.Items.AddRange(EnumTool.GetDescriptions<Bridge_CLK>().ToArray());
            var _Value = (Bridge_CLK)UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.byI2CClock_CM;
            m_cmbBridgeClock.SelectedItem = _Value.Desc();

            lb_I2C_Clock.Visible = UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bI2CClkVisible;
            m_cmbBridgeClock.Visible = UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bI2CClkVisible;

            // SPI Clock
            m_cmbBridgeSPIClock.Items.AddRange(EnumTool.GetDescriptions<Bridge_SPI_CLK>().ToArray());
            var _ValueSPI = (Bridge_SPI_CLK)UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bySPIClock_CM;
            m_cmbBridgeSPIClock.SelectedItem = _ValueSPI.Desc();

            lb_SPI_Clock.Visible = UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bSPIClkVisible;
            m_cmbBridgeSPIClock.Visible = UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bSPIClkVisible;

            // Selected Interface
            m_cmbInterfaceSelect.Items.AddRange(EnumTool.GetDescriptions<enBridgeInterface>().ToArray());
            var _ValueInterface = (enBridgeInterface)UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.byInterface;
            m_cmbInterfaceSelect.SelectedItem = _ValueInterface.Desc();

            if (!UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bInterfaceSelectVisible)
            {
                m_cmbInterfaceSelect.Visible = false;
                lb_Interface.Visible = false;
                //tabPage_SPI.Parent = null;
            }
            
            // PHAPOL
            m_cmbBridgePhaPol.Items.AddRange(EnumTool.GetDescriptions<enSPIPhaPol>().ToArray());
            var _ValuePhaPol = (enSPIPhaPol)UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bySPIPhaPol;
            m_cmbBridgePhaPol.SelectedItem = _ValuePhaPol.Desc();

            lb_SPI_PhaPol.Visible = UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bSPIPhaPolVisible;
            m_cmbBridgePhaPol.Visible = UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bSPIPhaPolVisible;
            // I2C Address
            m_tbI2CAdd.Text = string.Format("{0:X}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.iI2CAddress);

            // SPI Header          
            m_tbSPIHeader.Text = string.Format("{0:X}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.iSPIHeader);

            // Power - VDD
            m_cmbVDD.Items.AddRange(EnumTool.GetDescriptions<Bridge_Volt>().ToArray());
            m_cmbVDD.SelectedIndex = UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.VDD_CMD;

            // Power - VDDIO
            m_cmbVDDIO.Items.AddRange(EnumTool.GetDescriptions<Bridge_Volt>().ToArray());
            var item = (Bridge_Volt)Enum.Parse(typeof(Bridge_Volt), string.Format("V{0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.VDDIO_CMD));
            m_cmbVDDIO.SelectedItem = item.Desc();

            ckBox_PwrEnRST.Checked = UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bEN_PIN_RESET;

            // Host Delay
            txtBox_HostDelay.Text = UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.HostDelay.ToString();

            ckBox_RepeatStart.Checked = UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bRepeatStart;
            return true;
        }

        public void UpdateActualConnectI2CAddress(string sAddress)
        {
            m_tbI2CAdd_Actual.Text = sAddress;
            this.Refresh();
        }

        private void m_cmbBridgeClock_SelectedIndexChanged(object sender, EventArgs e)
        {
            var MySender = sender as ComboBox;
            var _Select = EnumTool.GetValueFromDescription<Bridge_CLK>(MySender.SelectedItem.ToString());
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.byI2CClock_CM = (byte)_Select;
            My_INI.IniWriteValue("Setting_Bridge", "I2C_Clock", MySender.SelectedItem.ToString());
        }

        private void m_cmbVDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            var MySender = sender as UIComboBox;

            string str = Enum.GetName(typeof(Bridge_Volt), MySender.SelectedIndex);
            if (MySender.SelectedIndex != UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.VDD_CMD)
            {
                UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.VDD_CMD = MySender.SelectedIndex;
                My_INI.IniWriteValue("Setting_Bridge", "VDD", str);
            }
        }

        private void m_cmbVDDIO_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpStr = Enum.Parse(typeof(Bridge_Volt), string.Format("V{0}", UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.VDDIO_CMD)).ToString();
            string str = Enum.GetName(typeof(Bridge_Volt), m_cmbVDDIO.SelectedIndex);
            if (tmpStr != str)
            {
                int.TryParse(str.Replace("V", ""), out UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.VDDIO_CMD);
                My_INI.IniWriteValue("Setting_Bridge", "VDDIO", str);
            }
        }

        private void textBox_HostDelay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 48) && (e.KeyChar <= 57))
            {
                string strValueBuf = txtBox_HostDelay.Text;
                if (txtBox_HostDelay.SelectionLength > 0)
                    strValueBuf = strValueBuf.Remove(txtBox_HostDelay.SelectionStart, txtBox_HostDelay.SelectionLength);
                strValueBuf = strValueBuf.Insert(txtBox_HostDelay.SelectionStart, e.KeyChar.ToString());
                e.Handled = false;
            }
            else if (e.KeyChar == (Char)8) // Backpace
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void m_tbI2CAdd_TextChanged(object sender, EventArgs e)
        {
            //string strbuf = "";            
            //if (m_tbI2CAdd.Text.Length > 0)
            //{
            //    strbuf = Convert.ToString(Convert.ToInt32(m_tbI2CAdd.Text.Substring(0, 1), 16), 2).PadLeft(4, '0');
            //    if (m_tbI2CAdd.Text.Length == 2)
            //        strbuf += " " + Convert.ToString(Convert.ToInt32(m_tbI2CAdd.Text.Substring(1, 1), 16), 2).PadLeft(4, '0');
            //    else
            //        strbuf = "0000 " + strbuf;
            //}
            var strbuf = m_tbI2CAdd.Text.Trim();
            var ibuf = string.IsNullOrEmpty(strbuf) ? 0x41 : Convert.ToInt32(strbuf, 16);
            My_INI.IniWriteValue("Setting_Bridge", "I2C_Addr", ibuf.ToString("X"));
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.iI2CAddress = ibuf;
        }

        private void textBox_HostDelay_TextChanged(object sender, EventArgs e)
        {
            My_INI.IniWriteValue("Setting_Bridge", "I2C_W_R_GapTime", txtBox_HostDelay.Text);
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.HostDelay = string.IsNullOrEmpty(txtBox_HostDelay.Text) ? 0 : Convert.ToInt32(txtBox_HostDelay.Text);
        }

        private void ckBox_RepeatStart_Click(object sender, EventArgs e)
        {
            My_INI.IniWriteValue("Setting_Bridge", "I2C_RepeatStart", ckBox_RepeatStart.Checked.ToString());
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bRepeatStart = My_INI.IniReadBool("Setting_Bridge", "I2C_RepeatStart", false);
        }

        private void ckBox_PwrEnRST_Click(object sender, EventArgs e)
        {
            My_INI.IniWriteValue("Setting_Bridge", "POWERON_RESET", ckBox_PwrEnRST.Checked.ToString());
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bEN_PIN_RESET = My_INI.IniReadBool("Setting_Bridge", "POWERON_RESET", false);
        }

        private void m_tbSPIHeader_TextChanged(object sender, EventArgs e)
        {
            var strbuf = m_tbSPIHeader.Text.Trim();
            var ibuf = string.IsNullOrEmpty(strbuf) ? 0x41 : Convert.ToInt32(strbuf, 16);
            My_INI.IniWriteValue("Setting_Bridge", "SPI_Header", ibuf.ToString("X"));
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.iSPIHeader = ibuf;
        }

        private void ckBox_SPIRepeatStart_Click(object sender, EventArgs e)
        {
            My_INI.IniWriteValue("Setting_Bridge", "SPI_RepeatStart", ckBox_SPIRepeatStart.Checked.ToString());
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bSPIRepeatStart = My_INI.IniReadBool("Setting_Bridge", "SPI_RepeatStart", false);
        }

        private void ckBox_SPIPwrEnRST_Click(object sender, EventArgs e)
        {
            My_INI.IniWriteValue("Setting_Bridge", "SPI_POWERON_RESET", ckBox_SPIPwrEnRST.Checked.ToString());
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bEN_PIN_RESET = My_INI.IniReadBool("Setting_Bridge", "SPI_POWERON_RESET", false);
        }

        private void m_cmbBridgeSPIClock_SelectedIndexChanged(object sender, EventArgs e)
        {
            var MySender = sender as ComboBox;
            var _Select = EnumTool.GetValueFromDescription<Bridge_SPI_CLK>(MySender.SelectedItem.ToString());
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bySPIClock_CM = (byte)_Select;
            My_INI.IniWriteValue("Setting_Bridge", "SPI_Clock", MySender.SelectedItem.ToString());
        }

        private void m_cmbInterfaceSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var MySender = sender as UIComboBox;
            var _Select = EnumTool.ParseEnum<enBridgeInterface>(MySender.SelectedItem.ToString());
            
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.byInterface = (byte)_Select;
            My_INI.IniWriteValue("Setting_Bridge", "NowInterface", MySender.SelectedItem.ToString());
            tabControl_Interface.SelectedTab = (_Select == enBridgeInterface.I2C) ? tabPage_I2C : tabPage_SPI;
        }

        private void m_cmbBridgePhaPol_SelectedIndexChanged(object sender, EventArgs e)
        {
            var MySender = sender as ComboBox;
            var _Select = EnumTool.GetValueFromDescription<enSPIPhaPol>(MySender.SelectedItem.ToString());
            UI_GBV.mDev[m_iSelectedDev].LocalVariable.BriSettings_Var.bySPIPhaPol = (byte)_Select;
            My_INI.IniWriteValue("Setting_Bridge", "SPI_PHAPOL", MySender.SelectedItem.ToString());
        }
        
       
    }
}