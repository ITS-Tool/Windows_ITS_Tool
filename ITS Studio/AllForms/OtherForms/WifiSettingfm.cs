using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using CommonExt;
using ThirdPartyTools;
using Sunny.UI;

namespace ITS_Studio.AllForms
{
    public partial class WifiSettingfm : Form
    {
        private string RemoteMethod = null;
        private int AccessType;
        private string ToolPath = null;
        private string ToolName = null;
        private string ShFilePath = null;
        private string BatchFilePath = null;
        private string Client_IP = null;
        private bool useApk = false;
        
        private string script_path = "\\Wifi\\Script";
        private string daemon_path = "\\Wifi\\Linux_Daemon_ITS";
        private string adb_port = "5555";

        private string device_select = null;

        private INI_Tool WiFi_INI = new INI_Tool(ITS_Directory.WiFi);
        private UIPage ui = new UIPage();

        private bool wifi_monitor = true;

        private enum enum_AccessType : int
        {
            adb = 0,
            ssh,
        }

        public enum UI_type : int
        {
            connecting = 0,
            connected,
        }

        public class adb_device
        {
            public string Details { get; set; }
            public string Text { get; set; }
            public string adb_id;
        }

        private List<adb_device> adb_devices = new List<adb_device>();
        private BindingSource bs = new BindingSource();

        public WifiSettingfm()
        {
            InitializeComponent();
            bs.DataSource = adb_devices;
            tabControl_WifiSetting.SelectedIndexChanged += new EventHandler(this.tabControl_WifiSetting_SelectedIndexChanged);
        }

        private void WifiSettingfm_Shown(object sender, EventArgs e)
        {
            cbBox_Method.Items.Clear();
            cbBox_Method.Items.AddRange(new string[] { "Use Linux_Daemon_ITS", "Use Android APK" });
            cbBox_Method.SelectedIndex = 0;

            cbBox_AccessType.Items.Clear();
            cbBox_AccessType.Items.AddRange(new string[] { "Android (bat file with adb)", "Other Linux OS (sh file)", "Android (APK)" }); cbBox_AccessType.SelectedIndex = 0;

            AccessType = cbBox_AccessType.SelectedIndex;

            ToolPath = WiFi_INI.IniReadValue("ConnectionInfo", "ToolPath", "");
            if (File.Exists(ToolPath))
            {
                cbBox_ToolPath.Items.Clear();
                cbBox_ToolPath.Items.Add(ToolPath);
                cbBox_ToolPath.SelectedIndex = 0;
            }

            BatchFilePath = WiFi_INI.IniReadValue("ConnectionInfo", "BatchFilePath", "");

            device_select = WiFi_INI.IniReadValue("ConnectionInfo", "DeviceSelect", "");

            cbBox_Interface.Items.Clear();
            cbBox_Interface.Items.AddRange(new string[] { "i2c", "usb", "hid-over-i2c" });
            cbBox_Interface.SelectedIndex = 0;

            Wifi_Var.INT_ack = WiFi_INI.IniReadBool("RemoteDeviceInfo", "I2C_INT_ack", true);
            cb_INT_ack.Checked = Wifi_Var.INT_ack;
            cb_INT_ack.Visible = true;

            Set_DefaultUI();
        }

        private void FileWrite(FileStream fs, string format, params object[] arg)
        {
            string str = string.Format(format, arg);
            byte[] info = new UTF8Encoding(true).GetBytes(str);
            fs.Write(info, 0, info.Length);
        }

        private void FileWriteLine(FileStream fs, string format, params object[] arg)
        {
            format += "\n";

            FileWrite(fs, format, arg);
        }

        private string parse_and_replace_string(string line)
        {
            string str;

            str = line.Replace("#ToolName#", ToolName);
            str = str.Replace("#Server_IP#", Wifi_Var.serverIP);
            str = str.Replace("#INT_ack#", (Wifi_Var.INT_ack) ? "--INT-ack=y" : "--INT-ack=n");
            str = str.Replace("#ToolPath#", ToolPath);
            str = str.Replace("#ShFilePath#", ShFilePath);

            str = str.Replace("#Client_IP#", Client_IP);
            str = str.Replace("#adb_port#", adb_port);
            str = str.Replace("#device_select#", device_select);

            str = str.Replace("#Server_Port#", Wifi_Var.serverPort.ToString());

            str = str.Replace("#Interface#", uiCheckBox_Interface.Checked ?
                string.Format("--interface={0}", cbBox_Interface.Text) :
                String.Empty);

            str = str.Replace("#Netlink#", uiCheckBox_Netlink.Checked ?
                string.Format("--netlink={0}", textBox_Netlink.Text) :
                String.Empty);

            str = str.Replace("#Vid#", uiCheckBox_Vid.Checked ?
                string.Format("--vid={0}", textBox_Vid.Text) :
                String.Empty);

            return str;
        }

        private bool create_bat_file()
        {
            string bat_file_format_path;
            FileStream fs;

            BatchFilePath = Directory.GetCurrentDirectory();
            BatchFilePath += string.Format(@"{0}\Wifi_Daemon_Init.bat", script_path);
            if (File.Exists(BatchFilePath))
                File.Delete(BatchFilePath);

            bat_file_format_path = Directory.GetCurrentDirectory();
            bat_file_format_path += string.Format(@"{0}\Wifi_Daemon_Init_bat_format.txt", script_path);
            if (!File.Exists(bat_file_format_path))
                return false;

            fs = File.Create(BatchFilePath);

            /* adb socket setting command */
            if (string.IsNullOrEmpty(Wifi_Var.serverIP))
            {
                string adb_socket_format_path = Directory.GetCurrentDirectory();
                adb_socket_format_path += string.Format(@"{0}\adb_socket_format.txt", script_path);
                if (File.Exists(adb_socket_format_path))
                {
                    string[] cmds = File.ReadAllLines(adb_socket_format_path);
                    foreach (string cmd in cmds)
                    {
                        string str = parse_and_replace_string(cmd);
                        FileWriteLine(fs, str);
                    }
                }
            }

            string[] lines = File.ReadAllLines(bat_file_format_path);
            foreach (string line in lines)
            {
                string str = parse_and_replace_string(line);
                FileWriteLine(fs, str);
            }

            fs.Close();

            return true;
        }

        private bool create_sh_file()
        {
            string sh_file_format_path;
            FileStream fs;

            ShFilePath = Directory.GetCurrentDirectory();
            ShFilePath += string.Format(@"{0}\Wifi_Daemon_Init.sh", script_path);
            if (File.Exists(ShFilePath))
                File.Delete(ShFilePath);

            sh_file_format_path = Directory.GetCurrentDirectory();
            sh_file_format_path += string.Format(@"{0}\Wifi_Daemon_Init_sh_format.txt", script_path);
            if (!File.Exists(sh_file_format_path))
                return false;

            fs = File.Create(ShFilePath);

            string[] lines = File.ReadAllLines(sh_file_format_path);
            foreach (string line in lines)
            {
                string str = parse_and_replace_string(line);
                FileWriteLine(fs, str);
            }

            fs.Close();

            return true;
        }

        private string RunBatchFile(string file_path, bool no_window, bool get_output)
        {
            string file_ext = Path.GetExtension(file_path).ToLower();
            string text = null;

            if (file_path == null || !File.Exists(file_path) || !String.Equals(file_ext, ".bat"))
            {
                ui.ShowErrorTip(string.Format("[Error] .bat path: {0} is not found or invalid.", file_path), 3000);
                return text;
            }

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = file_path;

            if (no_window)
            {
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
            }

            if (get_output)
            {
                info.RedirectStandardOutput = true;
                info.RedirectStandardInput = true;
                info.RedirectStandardError = true;
            }

            var proc = Process.Start(info);

            if (get_output)
            {
                using (StreamReader reader = proc.StandardOutput)
                {
                    text = reader.ReadToEnd();
                }
            }

            if (no_window)
            {
                proc.WaitForExit();
                proc.Close();
            }

            return text;
        }

        private void cb_INT_ack_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_INT_ack.Checked)
            {
                Wifi_Var.INT_ack = true;
            }
            else
            {
                Wifi_Var.INT_ack = false;
            }
        }

        private void cbBox_ToolPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolPath = cbBox_ToolPath.Text;
            ToolName = Path.GetFileName(ToolPath);
        }

        private void btn_ToolPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.FileName = "";
            openDlg.CheckFileExists = true;
            openDlg.CheckPathExists = true;
            openDlg.Multiselect = false;

            string path = Directory.GetCurrentDirectory();
            if (File.Exists(ToolPath))
                path = Path.GetDirectoryName(ToolPath);
            else if (Directory.Exists(path + daemon_path))
                path += daemon_path;

            openDlg.InitialDirectory = path;

            if (openDlg.ShowDialog(this) != DialogResult.OK)
                return;

            ToolPath = openDlg.FileNames.GetValue(0).ToString();
            cbBox_ToolPath.Items.Add(ToolPath);
            cbBox_ToolPath.SelectedIndex = cbBox_ToolPath.Items.Count - 1;

            ToolName = Path.GetFileName(ToolPath);
        }

        private void cbBox_AccessType_SelectedIndexChanged(object sender, EventArgs e)
        {
            AccessType = cbBox_AccessType.SelectedIndex;

            if (AccessType == (int)enum_AccessType.adb)
            {
                uiLine_adb_devices.Visible = true;
                btn_adb_device_list.Visible = true;
                listBox_adb_devices.Visible = true;
                btn_adb_device_list_Click(null, null);

                if (!tabControl_WifiSetting.TabPages.Contains(tab_AdbTcpipSetting))
                    tabControl_WifiSetting.TabPages.Add(tab_AdbTcpipSetting);
            }
            else
            {
                uiLine_adb_devices.Visible = false;
                btn_adb_device_list.Visible = false;
                listBox_adb_devices.Visible = false;

                if (tabControl_WifiSetting.TabPages.Contains(tab_AdbTcpipSetting))
                    tabControl_WifiSetting.TabPages.Remove(tab_AdbTcpipSetting);
            }
        }

        private void set_UI(UI_type type)
        {
            tabControl_WifiSetting.TabPages.Clear();

            if (type == UI_type.connecting)
            {
                tabControl_WifiSetting.TabPages.Add(tab_WifiSettingMain);
                tabControl_WifiSetting_SelectedIndexChanged(null, null);
            }
            else
            {
                tabControl_WifiSetting.TabPages.Add(tab_WifiITSInfo);
            }
        }

        public void Set_DefaultUI()
        {
            set_UI(UI_type.connecting);
            uiGroupBox_wifi_setting.Enabled = false;

            stop_time_record();
        }

        public void Set_ConnectingUI()
        {
            set_UI(UI_type.connecting);
            uiGroupBox_wifi_setting.Enabled = true;
        }

        public void Set_ConnectedUI()
        {
            set_UI(UI_type.connected);

            uiTextBox_daemon_ver.Text = string.Format("{0}.{1}.{2}.{3}",
                                                      (StaticVar.lsMain_USBDevInfo[0].daemon_ver & 0xFF000000) >> 24,
                                                      (StaticVar.lsMain_USBDevInfo[0].daemon_ver & 0xFF0000) >> 16,
                                                      (StaticVar.lsMain_USBDevInfo[0].daemon_ver & 0xFF00) >> 8,
                                                      StaticVar.lsMain_USBDevInfo[0].daemon_ver & 0xFF);

            uiTextBox_driver_ver.Text = "No I2C driver version";

            if (StaticVar.lsMain_USBDevInfo[0].stuDeviceType == DEV_INTERFACE_TYPE.WIFI_I2C)
            {
                uiTextBox_driver_ver.Text = string.Format("{0}.{1}.{2}.{3}",
                                                      (StaticVar.lsMain_USBDevInfo[0].driver_ver & 0xFF000000) >> 24,
                                                      (StaticVar.lsMain_USBDevInfo[0].driver_ver & 0xFF0000) >> 16,
                                                      (StaticVar.lsMain_USBDevInfo[0].driver_ver & 0xFF00) >> 8,
                                                      StaticVar.lsMain_USBDevInfo[0].driver_ver & 0xFF);
            }

            start_time_record();
        }

        private void button_wifiGo_Click(object sender, EventArgs e)
        {
            WiFi_INI.IniWriteValue("ConnectionInfo", "ServerIP", Wifi_Var.serverIP);

            cbBox_ToolPath_SelectedIndexChanged(null, null);
            Console.Write(ToolPath);
            if (ToolPath == null || !File.Exists(ToolPath) && !useApk)
            {
                ui.ShowErrorTip("[Error] Daemon Tool Path is invalid");
                return;
            }

            WiFi_INI.IniWriteValue("ConnectionInfo", "ToolPath", ToolPath);
            WiFi_INI.IniWriteValue("RemoteDeviceInfo", "I2C_INT_ack", Wifi_Var.INT_ack.ToString());

            if (!create_sh_file())
                return;

            if (AccessType == (int)enum_AccessType.adb)
            {
                if (listBox_adb_devices.SelectedItem == null)
                {
                    ui.ShowErrorTip("[Error] Please select a Adb devices below!");
                    return;
                }

                adb_device device = listBox_adb_devices.SelectedItem as adb_device;
                device_select = device.adb_id;
                WiFi_INI.IniWriteValue("ConnectionInfo", "DeviceSelect", device_select);

                if (!create_bat_file())
                    return;

                WiFi_INI.IniWriteValue("ConnectionInfo", "BatchFilePath", BatchFilePath);
                RunBatchFile(BatchFilePath, false, false);
            }
            else
            {
                MessageBox.Show(string.Format("[Remind] {0} created, please push it to remote device manually.", ShFilePath), "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return;
        }

        private void button_Set_ADB_Tcpip_Click(object sender, EventArgs e)
        {
            if (Client_IP == null)
                return;

            string format_path;
            FileStream fs;

            string bat_path = Directory.GetCurrentDirectory();
            bat_path += string.Format(@"{0}\adb_tcpip.bat", script_path);
            if (File.Exists(bat_path))
                File.Delete(bat_path);

            format_path = Directory.GetCurrentDirectory();
            format_path += string.Format(@"{0}\adb_tcpip_format.txt", script_path);
            if (!File.Exists(format_path))
                return;

            fs = File.Create(bat_path);

            string[] lines = File.ReadAllLines(format_path);
            foreach (string line in lines)
            {
                string str = parse_and_replace_string(line);
                FileWriteLine(fs, str);
            }

            fs.Close();

            RunBatchFile(bat_path, true, false);

            if (check_adb_devices_connected(Client_IP))
            {
                MessageBox.Show("[Remind] Adb tcpip wireless are set, please plug out USB!", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("[Error] Adb tcpip wireless set failed!", "",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool check_adb_devices_connected(string key)
        {
            string bat_path = Directory.GetCurrentDirectory();
            bat_path += string.Format(@"{0}\adb_list_devices.bat", script_path);

            string text = null;
            int retry = 10;
            do
            {
                if (--retry < 0)
                    return false;
                
                text = RunBatchFile(bat_path, true, true);
                textBox_Get_Client_IP.Text = text;
            } while (!text.Contains(key));

            return true;
        }

        private void textBox_RemoteDeviceIP_TextChanged(object sender, EventArgs e)
        {
            Client_IP = textBox_RemoteDeviceIP.Text.ToString();
        }

        private void button_Get_Client_IP_Click(object sender, EventArgs e)
        {
            string bat_path = Directory.GetCurrentDirectory();
            bat_path += string.Format(@"{0}\adb_list_ip.bat", script_path);

            textBox_Get_Client_IP.Text = RunBatchFile(bat_path, true, true);
        }

        private void btn_adb_device_list_Click(object sender, EventArgs e)
        {
            adb_devices.Clear();

            string bat_path = Directory.GetCurrentDirectory();
            bat_path += string.Format(@"{0}\adb_list_devices.bat", script_path);

            string text = RunBatchFile(bat_path, true, true);
            if (text == null)
                return;

            using (StringReader reader = new StringReader(text))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("transport_id"))
                    {
                        string[] str = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        string id = str[0];
                        string state = str[1];
                        string product = str[2].Remove(0, 8);
                        
                        adb_devices.Add(new adb_device() {
                                            Details = line,
                                            Text = string.Format("{0} | {1} | {2}", id, state, product),
                                            adb_id = id
                                            });
                        Console.WriteLine(line);
                    }
                }
            }

            if (adb_devices.Count == 0 && e != null)
            {
                ui.ShowErrorTip("[Error] No adb devices was found, please check...");
            }

            listBox_adb_devices.DisplayMember = "Text";
            listBox_adb_devices.DataSource = bs;
            bs.ResetBindings(false);
        }

        private void tabControl_WifiSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl_WifiSetting.SelectedTab == tab_WifiSettingMain)
                cbBox_AccessType_SelectedIndexChanged(null, null);
        }

        private void listBox_adb_devices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_adb_devices.Items.Count > 0)
            {
                uiToolTip_adb_devices.Active = true;

                adb_device device = listBox_adb_devices.SelectedItem as adb_device;
                uiToolTip_adb_devices.SetToolTip(listBox_adb_devices, device.Details);
            }
            else
            {
                uiToolTip_adb_devices.Active = false;
            }
        }

        private void start_time_record()
        {
            uiLedStopwatch.Start();
        }

        private void stop_time_record()
        {
            uiLedStopwatch.Stop();
        }

        private void uiCheckBox_wifi_monitor_CheckedChanged(object sender, EventArgs e)
        {
            wifi_monitor = uiCheckBox_wifi_monitor.Checked;

            if (!wifi_monitor)
                MessageBox.Show("[Warning] Disable Wifi monitor would stop checking wifi connection status!",
                                "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public bool enable_wifi_monitor()
        {
            return wifi_monitor;
        }

        private void cbBox_Method_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoteMethod = cbBox_Method.SelectedItem.ToString();

            useApk = false;
            uiPanel_gen_script.Enabled = false;

            if (String.Equals(RemoteMethod, "Use Android APK"))
                useApk = true;
            else
                uiPanel_gen_script.Enabled = true;
        }

        private void check_decimal(KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if (!Char.IsDigit(c) && c != 8)
            {
                e.Handled = true;
            }
        }

        private void check_hex(KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if ((c >= 'a' && c <= 'f') ||
                (c >= 'A' && c <= 'F') ||
                (c >= '0' && c <= '9') ||
                c == 8)
                return;

            e.Handled = true;
        }

        private void textBox_Netlink_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_decimal(e);
        }

        private void textBox_Vid_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_hex(e);
        }
    }
}
