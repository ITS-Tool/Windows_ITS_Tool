namespace ITS_Studio.AllForms
{
    partial class WifiSettingfm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl_WifiSetting = new Sunny.UI.UITabControl();
            this.tab_WifiSettingMain = new System.Windows.Forms.TabPage();
            this.uiPanel_gen_script = new Sunny.UI.UIPanel();
            this.uiTableLayoutPanel4 = new Sunny.UI.UITableLayoutPanel();
            this.btn_adb_device_list = new System.Windows.Forms.Button();
            this.listBox_adb_devices = new System.Windows.Forms.ListBox();
            this.uiLine_adb_devices = new Sunny.UI.UILine();
            this.cbBox_AccessType = new Sunny.UI.UIComboBox();
            this.uiLine4 = new Sunny.UI.UILine();
            this.uiTableLayoutPanel2 = new Sunny.UI.UITableLayoutPanel();
            this.btn_ToolPath = new Sunny.UI.UIImageButton();
            this.cbBox_ToolPath = new Sunny.UI.UIComboBox();
            this.uiLine3 = new Sunny.UI.UILine();
            this.button_wifiGo = new Sunny.UI.UISymbolButton();
            this.cbBox_Method = new Sunny.UI.UIComboBox();
            this.uiLine9 = new Sunny.UI.UILine();
            this.uiTitlePanel_advance_setting = new Sunny.UI.UITitlePanel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.uiCheckBox_Vid = new Sunny.UI.UICheckBox();
            this.textBox_Vid = new System.Windows.Forms.TextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.uiCheckBox_Netlink = new Sunny.UI.UICheckBox();
            this.textBox_Netlink = new System.Windows.Forms.TextBox();
            this.cb_INT_ack = new Sunny.UI.UICheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.uiCheckBox_Interface = new Sunny.UI.UICheckBox();
            this.cbBox_Interface = new Sunny.UI.UIComboBox();
            this.uiCheckBox_wifi_monitor = new Sunny.UI.UICheckBox();
            this.tab_AdbTcpipSetting = new System.Windows.Forms.TabPage();
            this.textBox_Get_Client_IP = new System.Windows.Forms.RichTextBox();
            this.uiLine5 = new Sunny.UI.UILine();
            this.textBox_RemoteDeviceIP = new System.Windows.Forms.TextBox();
            this.button_Set_ADB_Tcpip = new Sunny.UI.UISymbolButton();
            this.button_Get_Client_IP = new Sunny.UI.UISymbolButton();
            this.tab_WifiITSInfo = new System.Windows.Forms.TabPage();
            this.uiLine8 = new Sunny.UI.UILine();
            this.uiTextBox_driver_ver = new Sunny.UI.UITextBox();
            this.uiLine7 = new Sunny.UI.UILine();
            this.uiTextBox_daemon_ver = new Sunny.UI.UITextBox();
            this.uiLine6 = new Sunny.UI.UILine();
            this.uiLedStopwatch = new Sunny.UI.UILedStopwatch();
            this.uiGroupBox_wifi_setting = new Sunny.UI.UIGroupBox();
            this.panel_wifi_setting = new System.Windows.Forms.Panel();
            this.uiToolTip_adb_devices = new Sunny.UI.UIToolTip(this.components);
            this.tabControl_WifiSetting.SuspendLayout();
            this.tab_WifiSettingMain.SuspendLayout();
            this.uiPanel_gen_script.SuspendLayout();
            this.uiTableLayoutPanel4.SuspendLayout();
            this.uiTableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_ToolPath)).BeginInit();
            this.uiTitlePanel_advance_setting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tab_AdbTcpipSetting.SuspendLayout();
            this.tab_WifiITSInfo.SuspendLayout();
            this.uiGroupBox_wifi_setting.SuspendLayout();
            this.panel_wifi_setting.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_WifiSetting
            // 
            this.tabControl_WifiSetting.Controls.Add(this.tab_WifiSettingMain);
            this.tabControl_WifiSetting.Controls.Add(this.tab_AdbTcpipSetting);
            this.tabControl_WifiSetting.Controls.Add(this.tab_WifiITSInfo);
            this.tabControl_WifiSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_WifiSetting.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl_WifiSetting.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.tabControl_WifiSetting.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.tabControl_WifiSetting.ForbidCtrlTab = false;
            this.tabControl_WifiSetting.Frame = null;
            this.tabControl_WifiSetting.ItemSize = new System.Drawing.Size(150, 40);
            this.tabControl_WifiSetting.Location = new System.Drawing.Point(0, 0);
            this.tabControl_WifiSetting.MainPage = "";
            this.tabControl_WifiSetting.MenuStyle = Sunny.UI.UIMenuStyle.White;
            this.tabControl_WifiSetting.Name = "tabControl_WifiSetting";
            this.tabControl_WifiSetting.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl_WifiSetting.SelectedIndex = 0;
            this.tabControl_WifiSetting.Size = new System.Drawing.Size(308, 500);
            this.tabControl_WifiSetting.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_WifiSetting.Style = Sunny.UI.UIStyle.Orange;
            this.tabControl_WifiSetting.TabBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabControl_WifiSetting.TabIndex = 0;
            this.tabControl_WifiSetting.TabSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tabControl_WifiSetting.TabSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.tabControl_WifiSetting.TabSelectedHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.tabControl_WifiSetting.TabUnSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.tabControl_WifiSetting.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // tab_WifiSettingMain
            // 
            this.tab_WifiSettingMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.tab_WifiSettingMain.Controls.Add(this.uiPanel_gen_script);
            this.tab_WifiSettingMain.Controls.Add(this.button_wifiGo);
            this.tab_WifiSettingMain.Controls.Add(this.cbBox_Method);
            this.tab_WifiSettingMain.Controls.Add(this.uiLine9);
            this.tab_WifiSettingMain.Controls.Add(this.uiTitlePanel_advance_setting);
            this.tab_WifiSettingMain.Cursor = System.Windows.Forms.Cursors.Default;
            this.tab_WifiSettingMain.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.tab_WifiSettingMain.ForeColor = System.Drawing.Color.Transparent;
            this.tab_WifiSettingMain.Location = new System.Drawing.Point(0, 40);
            this.tab_WifiSettingMain.Name = "tab_WifiSettingMain";
            this.tab_WifiSettingMain.Size = new System.Drawing.Size(308, 460);
            this.tab_WifiSettingMain.TabIndex = 0;
            this.tab_WifiSettingMain.Text = "Connection Setting";
            // 
            // uiPanel_gen_script
            // 
            this.uiPanel_gen_script.Controls.Add(this.uiTableLayoutPanel4);
            this.uiPanel_gen_script.Controls.Add(this.uiLine_adb_devices);
            this.uiPanel_gen_script.Controls.Add(this.cbBox_AccessType);
            this.uiPanel_gen_script.Controls.Add(this.uiLine4);
            this.uiPanel_gen_script.Controls.Add(this.uiTableLayoutPanel2);
            this.uiPanel_gen_script.Controls.Add(this.uiLine3);
            this.uiPanel_gen_script.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel_gen_script.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiPanel_gen_script.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiPanel_gen_script.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.uiPanel_gen_script.Location = new System.Drawing.Point(0, 73);
            this.uiPanel_gen_script.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel_gen_script.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel_gen_script.Name = "uiPanel_gen_script";
            this.uiPanel_gen_script.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiPanel_gen_script.Size = new System.Drawing.Size(308, 344);
            this.uiPanel_gen_script.Style = Sunny.UI.UIStyle.Orange;
            this.uiPanel_gen_script.TabIndex = 30;
            this.uiPanel_gen_script.Text = "uiPanel1";
            this.uiPanel_gen_script.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiPanel_gen_script.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiTableLayoutPanel4
            // 
            this.uiTableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.uiTableLayoutPanel4.ColumnCount = 2;
            this.uiTableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.66234F));
            this.uiTableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.33766F));
            this.uiTableLayoutPanel4.Controls.Add(this.btn_adb_device_list, 1, 0);
            this.uiTableLayoutPanel4.Controls.Add(this.listBox_adb_devices, 0, 0);
            this.uiTableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTableLayoutPanel4.Location = new System.Drawing.Point(0, 116);
            this.uiTableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.uiTableLayoutPanel4.Name = "uiTableLayoutPanel4";
            this.uiTableLayoutPanel4.RowCount = 1;
            this.uiTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 206F));
            this.uiTableLayoutPanel4.Size = new System.Drawing.Size(308, 228);
            this.uiTableLayoutPanel4.Style = Sunny.UI.UIStyle.Orange;
            this.uiTableLayoutPanel4.TabIndex = 46;
            this.uiTableLayoutPanel4.TagString = null;
            // 
            // btn_adb_device_list
            // 
            this.btn_adb_device_list.BackgroundImage = global::ITS_Studio.Properties.Resources.refresh;
            this.btn_adb_device_list.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_adb_device_list.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_adb_device_list.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_adb_device_list.Location = new System.Drawing.Point(273, 3);
            this.btn_adb_device_list.Name = "btn_adb_device_list";
            this.btn_adb_device_list.Size = new System.Drawing.Size(32, 24);
            this.btn_adb_device_list.TabIndex = 32;
            this.btn_adb_device_list.UseVisualStyleBackColor = false;
            this.btn_adb_device_list.Click += new System.EventHandler(this.btn_adb_device_list_Click);
            // 
            // listBox_adb_devices
            // 
            this.listBox_adb_devices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox_adb_devices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_adb_devices.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.listBox_adb_devices.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.listBox_adb_devices.FormattingEnabled = true;
            this.listBox_adb_devices.HorizontalScrollbar = true;
            this.listBox_adb_devices.ItemHeight = 17;
            this.listBox_adb_devices.Location = new System.Drawing.Point(0, 0);
            this.listBox_adb_devices.Margin = new System.Windows.Forms.Padding(0);
            this.listBox_adb_devices.Name = "listBox_adb_devices";
            this.listBox_adb_devices.Size = new System.Drawing.Size(270, 228);
            this.listBox_adb_devices.TabIndex = 2;
            this.listBox_adb_devices.Tag = "";
            this.listBox_adb_devices.SelectedIndexChanged += new System.EventHandler(this.listBox_adb_devices_SelectedIndexChanged);
            // 
            // uiLine_adb_devices
            // 
            this.uiLine_adb_devices.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiLine_adb_devices.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiLine_adb_devices.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiLine_adb_devices.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiLine_adb_devices.Location = new System.Drawing.Point(0, 96);
            this.uiLine_adb_devices.MinimumSize = new System.Drawing.Size(2, 2);
            this.uiLine_adb_devices.Name = "uiLine_adb_devices";
            this.uiLine_adb_devices.Size = new System.Drawing.Size(308, 20);
            this.uiLine_adb_devices.Style = Sunny.UI.UIStyle.Orange;
            this.uiLine_adb_devices.TabIndex = 47;
            this.uiLine_adb_devices.Text = "Adb devices";
            this.uiLine_adb_devices.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLine_adb_devices.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // cbBox_AccessType
            // 
            this.cbBox_AccessType.DataSource = null;
            this.cbBox_AccessType.DisplayMember = "Interface";
            this.cbBox_AccessType.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbBox_AccessType.FillColor = System.Drawing.Color.White;
            this.cbBox_AccessType.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.cbBox_AccessType.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbBox_AccessType.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(235)))), ((int)(((byte)(212)))));
            this.cbBox_AccessType.ItemRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_AccessType.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_AccessType.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.cbBox_AccessType.Location = new System.Drawing.Point(0, 68);
            this.cbBox_AccessType.Margin = new System.Windows.Forms.Padding(0);
            this.cbBox_AccessType.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbBox_AccessType.Name = "cbBox_AccessType";
            this.cbBox_AccessType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbBox_AccessType.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_AccessType.Size = new System.Drawing.Size(308, 28);
            this.cbBox_AccessType.Style = Sunny.UI.UIStyle.Orange;
            this.cbBox_AccessType.TabIndex = 40;
            this.cbBox_AccessType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbBox_AccessType.Watermark = "Access Type";
            this.cbBox_AccessType.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.cbBox_AccessType.SelectedIndexChanged += new System.EventHandler(this.cbBox_AccessType_SelectedIndexChanged);
            // 
            // uiLine4
            // 
            this.uiLine4.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiLine4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiLine4.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiLine4.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiLine4.Location = new System.Drawing.Point(0, 48);
            this.uiLine4.MinimumSize = new System.Drawing.Size(2, 2);
            this.uiLine4.Name = "uiLine4";
            this.uiLine4.Size = new System.Drawing.Size(308, 20);
            this.uiLine4.Style = Sunny.UI.UIStyle.Orange;
            this.uiLine4.TabIndex = 39;
            this.uiLine4.Text = "Remote Access Type";
            this.uiLine4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLine4.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiTableLayoutPanel2
            // 
            this.uiTableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.uiTableLayoutPanel2.ColumnCount = 2;
            this.uiTableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73F));
            this.uiTableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27F));
            this.uiTableLayoutPanel2.Controls.Add(this.btn_ToolPath, 1, 0);
            this.uiTableLayoutPanel2.Controls.Add(this.cbBox_ToolPath, 0, 0);
            this.uiTableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiTableLayoutPanel2.Location = new System.Drawing.Point(0, 20);
            this.uiTableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.uiTableLayoutPanel2.Name = "uiTableLayoutPanel2";
            this.uiTableLayoutPanel2.RowCount = 1;
            this.uiTableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uiTableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.uiTableLayoutPanel2.Size = new System.Drawing.Size(308, 28);
            this.uiTableLayoutPanel2.Style = Sunny.UI.UIStyle.Orange;
            this.uiTableLayoutPanel2.TabIndex = 44;
            this.uiTableLayoutPanel2.TagString = null;
            // 
            // btn_ToolPath
            // 
            this.btn_ToolPath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_ToolPath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_ToolPath.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_ToolPath.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.btn_ToolPath.Image = global::ITS_Studio.Properties.Resources.Documents_icon16_16;
            this.btn_ToolPath.ImageDisabled = global::ITS_Studio.Properties.Resources.Dark_Documents_icon;
            this.btn_ToolPath.Location = new System.Drawing.Point(227, 3);
            this.btn_ToolPath.Name = "btn_ToolPath";
            this.btn_ToolPath.Size = new System.Drawing.Size(24, 22);
            this.btn_ToolPath.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btn_ToolPath.Style = Sunny.UI.UIStyle.Orange;
            this.btn_ToolPath.TabIndex = 42;
            this.btn_ToolPath.TabStop = false;
            this.btn_ToolPath.Text = null;
            this.btn_ToolPath.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_ToolPath.Click += new System.EventHandler(this.btn_ToolPath_Click);
            // 
            // cbBox_ToolPath
            // 
            this.cbBox_ToolPath.DataSource = null;
            this.cbBox_ToolPath.DisplayMember = "Interface";
            this.cbBox_ToolPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBox_ToolPath.FillColor = System.Drawing.Color.White;
            this.cbBox_ToolPath.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.cbBox_ToolPath.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbBox_ToolPath.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(235)))), ((int)(((byte)(212)))));
            this.cbBox_ToolPath.ItemRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_ToolPath.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_ToolPath.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.cbBox_ToolPath.Location = new System.Drawing.Point(0, 0);
            this.cbBox_ToolPath.Margin = new System.Windows.Forms.Padding(0);
            this.cbBox_ToolPath.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbBox_ToolPath.Name = "cbBox_ToolPath";
            this.cbBox_ToolPath.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbBox_ToolPath.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_ToolPath.Size = new System.Drawing.Size(224, 28);
            this.cbBox_ToolPath.Style = Sunny.UI.UIStyle.Orange;
            this.cbBox_ToolPath.TabIndex = 38;
            this.cbBox_ToolPath.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbBox_ToolPath.Watermark = "Deamon Tool Path";
            this.cbBox_ToolPath.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.cbBox_ToolPath.SelectedIndexChanged += new System.EventHandler(this.cbBox_ToolPath_SelectedIndexChanged);
            // 
            // uiLine3
            // 
            this.uiLine3.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiLine3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiLine3.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiLine3.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiLine3.Location = new System.Drawing.Point(0, 0);
            this.uiLine3.MinimumSize = new System.Drawing.Size(2, 2);
            this.uiLine3.Name = "uiLine3";
            this.uiLine3.Size = new System.Drawing.Size(308, 20);
            this.uiLine3.Style = Sunny.UI.UIStyle.Orange;
            this.uiLine3.TabIndex = 37;
            this.uiLine3.Text = "Daemon Tool Path";
            this.uiLine3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLine3.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // button_wifiGo
            // 
            this.button_wifiGo.BackColor = System.Drawing.Color.Transparent;
            this.button_wifiGo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_wifiGo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button_wifiGo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.button_wifiGo.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.button_wifiGo.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.button_wifiGo.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.button_wifiGo.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.button_wifiGo.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_wifiGo.Location = new System.Drawing.Point(0, 417);
            this.button_wifiGo.Margin = new System.Windows.Forms.Padding(20);
            this.button_wifiGo.MinimumSize = new System.Drawing.Size(1, 1);
            this.button_wifiGo.Name = "button_wifiGo";
            this.button_wifiGo.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.button_wifiGo.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.button_wifiGo.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.button_wifiGo.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.button_wifiGo.Size = new System.Drawing.Size(308, 43);
            this.button_wifiGo.Style = Sunny.UI.UIStyle.Orange;
            this.button_wifiGo.StyleCustomMode = true;
            this.button_wifiGo.TabIndex = 29;
            this.button_wifiGo.Text = "Apply All";
            this.button_wifiGo.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button_wifiGo.Click += new System.EventHandler(this.button_wifiGo_Click);
            // 
            // cbBox_Method
            // 
            this.cbBox_Method.DataSource = null;
            this.cbBox_Method.DisplayMember = "Interface";
            this.cbBox_Method.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbBox_Method.FillColor = System.Drawing.Color.White;
            this.cbBox_Method.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.cbBox_Method.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbBox_Method.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(235)))), ((int)(((byte)(212)))));
            this.cbBox_Method.ItemRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_Method.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_Method.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.cbBox_Method.Location = new System.Drawing.Point(0, 45);
            this.cbBox_Method.Margin = new System.Windows.Forms.Padding(0);
            this.cbBox_Method.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbBox_Method.Name = "cbBox_Method";
            this.cbBox_Method.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbBox_Method.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_Method.Size = new System.Drawing.Size(308, 28);
            this.cbBox_Method.Style = Sunny.UI.UIStyle.Orange;
            this.cbBox_Method.TabIndex = 41;
            this.cbBox_Method.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbBox_Method.Watermark = "Select a tool executed on device";
            this.cbBox_Method.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.cbBox_Method.SelectedIndexChanged += new System.EventHandler(this.cbBox_Method_SelectedIndexChanged);
            // 
            // uiLine9
            // 
            this.uiLine9.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiLine9.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiLine9.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiLine9.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiLine9.LineDashStyle = Sunny.UI.UILineDashStyle.Solid;
            this.uiLine9.Location = new System.Drawing.Point(0, 25);
            this.uiLine9.MinimumSize = new System.Drawing.Size(2, 2);
            this.uiLine9.Name = "uiLine9";
            this.uiLine9.Size = new System.Drawing.Size(308, 20);
            this.uiLine9.Style = Sunny.UI.UIStyle.Orange;
            this.uiLine9.TabIndex = 40;
            this.uiLine9.Text = "Tool Selection";
            this.uiLine9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLine9.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiTitlePanel_advance_setting
            // 
            this.uiTitlePanel_advance_setting.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.uiTitlePanel_advance_setting.Collapsed = true;
            this.uiTitlePanel_advance_setting.Controls.Add(this.splitContainer3);
            this.uiTitlePanel_advance_setting.Controls.Add(this.splitContainer2);
            this.uiTitlePanel_advance_setting.Controls.Add(this.cb_INT_ack);
            this.uiTitlePanel_advance_setting.Controls.Add(this.splitContainer1);
            this.uiTitlePanel_advance_setting.Controls.Add(this.uiCheckBox_wifi_monitor);
            this.uiTitlePanel_advance_setting.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiTitlePanel_advance_setting.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiTitlePanel_advance_setting.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiTitlePanel_advance_setting.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.uiTitlePanel_advance_setting.Location = new System.Drawing.Point(0, 0);
            this.uiTitlePanel_advance_setting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel_advance_setting.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel_advance_setting.Name = "uiTitlePanel_advance_setting";
            this.uiTitlePanel_advance_setting.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.uiTitlePanel_advance_setting.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTitlePanel_advance_setting.ShowCollapse = true;
            this.uiTitlePanel_advance_setting.ShowText = false;
            this.uiTitlePanel_advance_setting.Size = new System.Drawing.Size(308, 25);
            this.uiTitlePanel_advance_setting.Style = Sunny.UI.UIStyle.Orange;
            this.uiTitlePanel_advance_setting.TabIndex = 32;
            this.uiTitlePanel_advance_setting.Text = "Advance Setting";
            this.uiTitlePanel_advance_setting.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiTitlePanel_advance_setting.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTitlePanel_advance_setting.TitleHeight = 25;
            this.uiTitlePanel_advance_setting.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer3.Location = new System.Drawing.Point(0, 140);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.uiCheckBox_Vid);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer3.Panel2.Controls.Add(this.textBox_Vid);
            this.splitContainer3.Size = new System.Drawing.Size(308, 27);
            this.splitContainer3.SplitterDistance = 207;
            this.splitContainer3.TabIndex = 40;
            // 
            // uiCheckBox_Vid
            // 
            this.uiCheckBox_Vid.BackColor = System.Drawing.Color.Transparent;
            this.uiCheckBox_Vid.CheckBoxColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiCheckBox_Vid.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiCheckBox_Vid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiCheckBox_Vid.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiCheckBox_Vid.Location = new System.Drawing.Point(0, 0);
            this.uiCheckBox_Vid.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiCheckBox_Vid.Name = "uiCheckBox_Vid";
            this.uiCheckBox_Vid.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.uiCheckBox_Vid.Size = new System.Drawing.Size(207, 27);
            this.uiCheckBox_Vid.Style = Sunny.UI.UIStyle.Orange;
            this.uiCheckBox_Vid.TabIndex = 1;
            this.uiCheckBox_Vid.Text = "VID (USB/HID-over-I2C Only)";
            this.uiCheckBox_Vid.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // textBox_Vid
            // 
            this.textBox_Vid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox_Vid.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.textBox_Vid.Location = new System.Drawing.Point(0, 2);
            this.textBox_Vid.Name = "textBox_Vid";
            this.textBox_Vid.Size = new System.Drawing.Size(97, 25);
            this.textBox_Vid.TabIndex = 33;
            this.textBox_Vid.Text = "222a";
            this.textBox_Vid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Vid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Vid_KeyPress);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer2.Location = new System.Drawing.Point(0, 113);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.uiCheckBox_Netlink);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Panel2.Controls.Add(this.textBox_Netlink);
            this.splitContainer2.Size = new System.Drawing.Size(308, 27);
            this.splitContainer2.SplitterDistance = 207;
            this.splitContainer2.TabIndex = 39;
            // 
            // uiCheckBox_Netlink
            // 
            this.uiCheckBox_Netlink.BackColor = System.Drawing.Color.Transparent;
            this.uiCheckBox_Netlink.CheckBoxColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiCheckBox_Netlink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiCheckBox_Netlink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiCheckBox_Netlink.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiCheckBox_Netlink.Location = new System.Drawing.Point(0, 0);
            this.uiCheckBox_Netlink.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiCheckBox_Netlink.Name = "uiCheckBox_Netlink";
            this.uiCheckBox_Netlink.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.uiCheckBox_Netlink.Size = new System.Drawing.Size(207, 27);
            this.uiCheckBox_Netlink.Style = Sunny.UI.UIStyle.Orange;
            this.uiCheckBox_Netlink.TabIndex = 1;
            this.uiCheckBox_Netlink.Text = "Netlink Number (I2C-Only)";
            this.uiCheckBox_Netlink.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // textBox_Netlink
            // 
            this.textBox_Netlink.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox_Netlink.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.textBox_Netlink.Location = new System.Drawing.Point(0, 2);
            this.textBox_Netlink.Name = "textBox_Netlink";
            this.textBox_Netlink.Size = new System.Drawing.Size(97, 25);
            this.textBox_Netlink.TabIndex = 33;
            this.textBox_Netlink.Text = "2";
            this.textBox_Netlink.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Netlink.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Netlink_KeyPress);
            // 
            // cb_INT_ack
            // 
            this.cb_INT_ack.BackColor = System.Drawing.Color.Transparent;
            this.cb_INT_ack.CheckBoxColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cb_INT_ack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cb_INT_ack.Dock = System.Windows.Forms.DockStyle.Top;
            this.cb_INT_ack.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.cb_INT_ack.Location = new System.Drawing.Point(0, 91);
            this.cb_INT_ack.MinimumSize = new System.Drawing.Size(1, 1);
            this.cb_INT_ack.Name = "cb_INT_ack";
            this.cb_INT_ack.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.cb_INT_ack.Size = new System.Drawing.Size(308, 22);
            this.cb_INT_ack.Style = Sunny.UI.UIStyle.Orange;
            this.cb_INT_ack.TabIndex = 37;
            this.cb_INT_ack.Text = "INT Ack (I2C-Only)";
            this.cb_INT_ack.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.cb_INT_ack.CheckedChanged += new System.EventHandler(this.cb_INT_ack_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.Location = new System.Drawing.Point(0, 64);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.uiCheckBox_Interface);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.Controls.Add(this.cbBox_Interface);
            this.splitContainer1.Size = new System.Drawing.Size(308, 27);
            this.splitContainer1.SplitterDistance = 87;
            this.splitContainer1.TabIndex = 38;
            // 
            // uiCheckBox_Interface
            // 
            this.uiCheckBox_Interface.BackColor = System.Drawing.Color.Transparent;
            this.uiCheckBox_Interface.CheckBoxColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiCheckBox_Interface.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiCheckBox_Interface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiCheckBox_Interface.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiCheckBox_Interface.Location = new System.Drawing.Point(0, 0);
            this.uiCheckBox_Interface.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiCheckBox_Interface.Name = "uiCheckBox_Interface";
            this.uiCheckBox_Interface.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.uiCheckBox_Interface.Size = new System.Drawing.Size(87, 27);
            this.uiCheckBox_Interface.Style = Sunny.UI.UIStyle.Orange;
            this.uiCheckBox_Interface.TabIndex = 1;
            this.uiCheckBox_Interface.Text = "Interface";
            this.uiCheckBox_Interface.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // cbBox_Interface
            // 
            this.cbBox_Interface.DataSource = null;
            this.cbBox_Interface.DisplayMember = "Interface";
            this.cbBox_Interface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBox_Interface.FillColor = System.Drawing.Color.White;
            this.cbBox_Interface.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.cbBox_Interface.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbBox_Interface.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(235)))), ((int)(((byte)(212)))));
            this.cbBox_Interface.ItemRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_Interface.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_Interface.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.cbBox_Interface.Location = new System.Drawing.Point(0, 0);
            this.cbBox_Interface.Margin = new System.Windows.Forms.Padding(0);
            this.cbBox_Interface.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbBox_Interface.Name = "cbBox_Interface";
            this.cbBox_Interface.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbBox_Interface.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_Interface.Size = new System.Drawing.Size(217, 27);
            this.cbBox_Interface.Style = Sunny.UI.UIStyle.Orange;
            this.cbBox_Interface.TabIndex = 41;
            this.cbBox_Interface.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbBox_Interface.Watermark = "Interface";
            this.cbBox_Interface.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiCheckBox_wifi_monitor
            // 
            this.uiCheckBox_wifi_monitor.BackColor = System.Drawing.Color.Transparent;
            this.uiCheckBox_wifi_monitor.CheckBoxColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiCheckBox_wifi_monitor.Checked = true;
            this.uiCheckBox_wifi_monitor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiCheckBox_wifi_monitor.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiCheckBox_wifi_monitor.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiCheckBox_wifi_monitor.Location = new System.Drawing.Point(0, 35);
            this.uiCheckBox_wifi_monitor.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiCheckBox_wifi_monitor.Name = "uiCheckBox_wifi_monitor";
            this.uiCheckBox_wifi_monitor.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.uiCheckBox_wifi_monitor.Size = new System.Drawing.Size(308, 29);
            this.uiCheckBox_wifi_monitor.Style = Sunny.UI.UIStyle.Orange;
            this.uiCheckBox_wifi_monitor.TabIndex = 0;
            this.uiCheckBox_wifi_monitor.Text = "Wifi Monitoring";
            this.uiCheckBox_wifi_monitor.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiCheckBox_wifi_monitor.CheckedChanged += new System.EventHandler(this.uiCheckBox_wifi_monitor_CheckedChanged);
            // 
            // tab_AdbTcpipSetting
            // 
            this.tab_AdbTcpipSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.tab_AdbTcpipSetting.Controls.Add(this.textBox_Get_Client_IP);
            this.tab_AdbTcpipSetting.Controls.Add(this.uiLine5);
            this.tab_AdbTcpipSetting.Controls.Add(this.textBox_RemoteDeviceIP);
            this.tab_AdbTcpipSetting.Controls.Add(this.button_Set_ADB_Tcpip);
            this.tab_AdbTcpipSetting.Controls.Add(this.button_Get_Client_IP);
            this.tab_AdbTcpipSetting.ForeColor = System.Drawing.Color.Transparent;
            this.tab_AdbTcpipSetting.Location = new System.Drawing.Point(0, 40);
            this.tab_AdbTcpipSetting.Name = "tab_AdbTcpipSetting";
            this.tab_AdbTcpipSetting.Size = new System.Drawing.Size(200, 60);
            this.tab_AdbTcpipSetting.TabIndex = 1;
            this.tab_AdbTcpipSetting.Text = "ADB Tcpip Setting";
            // 
            // textBox_Get_Client_IP
            // 
            this.textBox_Get_Client_IP.BackColor = System.Drawing.SystemColors.InfoText;
            this.textBox_Get_Client_IP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Get_Client_IP.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_Get_Client_IP.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox_Get_Client_IP.Location = new System.Drawing.Point(0, 34);
            this.textBox_Get_Client_IP.Name = "textBox_Get_Client_IP";
            this.textBox_Get_Client_IP.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBox_Get_Client_IP.Size = new System.Drawing.Size(200, 0);
            this.textBox_Get_Client_IP.TabIndex = 33;
            this.textBox_Get_Client_IP.Text = "";
            // 
            // uiLine5
            // 
            this.uiLine5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiLine5.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiLine5.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiLine5.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiLine5.Location = new System.Drawing.Point(0, -23);
            this.uiLine5.MinimumSize = new System.Drawing.Size(2, 2);
            this.uiLine5.Name = "uiLine5";
            this.uiLine5.Size = new System.Drawing.Size(200, 20);
            this.uiLine5.Style = Sunny.UI.UIStyle.Orange;
            this.uiLine5.TabIndex = 38;
            this.uiLine5.Text = "Remote Device IP";
            this.uiLine5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLine5.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // textBox_RemoteDeviceIP
            // 
            this.textBox_RemoteDeviceIP.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox_RemoteDeviceIP.Location = new System.Drawing.Point(0, -3);
            this.textBox_RemoteDeviceIP.Name = "textBox_RemoteDeviceIP";
            this.textBox_RemoteDeviceIP.Size = new System.Drawing.Size(200, 25);
            this.textBox_RemoteDeviceIP.TabIndex = 32;
            this.textBox_RemoteDeviceIP.TextChanged += new System.EventHandler(this.textBox_RemoteDeviceIP_TextChanged);
            // 
            // button_Set_ADB_Tcpip
            // 
            this.button_Set_ADB_Tcpip.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_Set_ADB_Tcpip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button_Set_ADB_Tcpip.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.button_Set_ADB_Tcpip.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.button_Set_ADB_Tcpip.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.button_Set_ADB_Tcpip.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.button_Set_ADB_Tcpip.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.button_Set_ADB_Tcpip.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.button_Set_ADB_Tcpip.Location = new System.Drawing.Point(0, 22);
            this.button_Set_ADB_Tcpip.MinimumSize = new System.Drawing.Size(1, 1);
            this.button_Set_ADB_Tcpip.Name = "button_Set_ADB_Tcpip";
            this.button_Set_ADB_Tcpip.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.button_Set_ADB_Tcpip.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.button_Set_ADB_Tcpip.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.button_Set_ADB_Tcpip.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.button_Set_ADB_Tcpip.Size = new System.Drawing.Size(200, 38);
            this.button_Set_ADB_Tcpip.Style = Sunny.UI.UIStyle.Orange;
            this.button_Set_ADB_Tcpip.TabIndex = 37;
            this.button_Set_ADB_Tcpip.Text = "Setup ADB Tcpip";
            this.button_Set_ADB_Tcpip.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button_Set_ADB_Tcpip.Click += new System.EventHandler(this.button_Set_ADB_Tcpip_Click);
            // 
            // button_Get_Client_IP
            // 
            this.button_Get_Client_IP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_Get_Client_IP.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_Get_Client_IP.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.button_Get_Client_IP.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.button_Get_Client_IP.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.button_Get_Client_IP.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.button_Get_Client_IP.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.button_Get_Client_IP.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.button_Get_Client_IP.Location = new System.Drawing.Point(0, 0);
            this.button_Get_Client_IP.MinimumSize = new System.Drawing.Size(1, 1);
            this.button_Get_Client_IP.Name = "button_Get_Client_IP";
            this.button_Get_Client_IP.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.button_Get_Client_IP.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.button_Get_Client_IP.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.button_Get_Client_IP.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.button_Get_Client_IP.Size = new System.Drawing.Size(200, 34);
            this.button_Get_Client_IP.Style = Sunny.UI.UIStyle.Orange;
            this.button_Get_Client_IP.Symbol = 61931;
            this.button_Get_Client_IP.TabIndex = 36;
            this.button_Get_Client_IP.Text = "Get Remote Device IP";
            this.button_Get_Client_IP.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.button_Get_Client_IP.Click += new System.EventHandler(this.button_Get_Client_IP_Click);
            // 
            // tab_WifiITSInfo
            // 
            this.tab_WifiITSInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.tab_WifiITSInfo.Controls.Add(this.uiLine8);
            this.tab_WifiITSInfo.Controls.Add(this.uiTextBox_driver_ver);
            this.tab_WifiITSInfo.Controls.Add(this.uiLine7);
            this.tab_WifiITSInfo.Controls.Add(this.uiTextBox_daemon_ver);
            this.tab_WifiITSInfo.Controls.Add(this.uiLine6);
            this.tab_WifiITSInfo.Controls.Add(this.uiLedStopwatch);
            this.tab_WifiITSInfo.ForeColor = System.Drawing.Color.Transparent;
            this.tab_WifiITSInfo.Location = new System.Drawing.Point(0, 40);
            this.tab_WifiITSInfo.Name = "tab_WifiITSInfo";
            this.tab_WifiITSInfo.Size = new System.Drawing.Size(200, 60);
            this.tab_WifiITSInfo.TabIndex = 2;
            this.tab_WifiITSInfo.Text = "Advanced Info";
            // 
            // uiLine8
            // 
            this.uiLine8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiLine8.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiLine8.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiLine8.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiLine8.Location = new System.Drawing.Point(0, 6);
            this.uiLine8.MinimumSize = new System.Drawing.Size(2, 2);
            this.uiLine8.Name = "uiLine8";
            this.uiLine8.Size = new System.Drawing.Size(200, 20);
            this.uiLine8.Style = Sunny.UI.UIStyle.Orange;
            this.uiLine8.TabIndex = 39;
            this.uiLine8.Text = "Connection Time";
            this.uiLine8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLine8.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiTextBox_driver_ver
            // 
            this.uiTextBox_driver_ver.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTextBox_driver_ver.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.uiTextBox_driver_ver.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.uiTextBox_driver_ver.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTextBox_driver_ver.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.uiTextBox_driver_ver.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.uiTextBox_driver_ver.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.uiTextBox_driver_ver.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiTextBox_driver_ver.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiTextBox_driver_ver.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.uiTextBox_driver_ver.Location = new System.Drawing.Point(0, 68);
            this.uiTextBox_driver_ver.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTextBox_driver_ver.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTextBox_driver_ver.Name = "uiTextBox_driver_ver";
            this.uiTextBox_driver_ver.ReadOnly = true;
            this.uiTextBox_driver_ver.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTextBox_driver_ver.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTextBox_driver_ver.ShowText = false;
            this.uiTextBox_driver_ver.Size = new System.Drawing.Size(200, 28);
            this.uiTextBox_driver_ver.Style = Sunny.UI.UIStyle.Orange;
            this.uiTextBox_driver_ver.TabIndex = 37;
            this.uiTextBox_driver_ver.Text = "No I2C Driver Version";
            this.uiTextBox_driver_ver.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiTextBox_driver_ver.Watermark = "";
            this.uiTextBox_driver_ver.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLine7
            // 
            this.uiLine7.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiLine7.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiLine7.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiLine7.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiLine7.Location = new System.Drawing.Point(0, 48);
            this.uiLine7.MinimumSize = new System.Drawing.Size(2, 2);
            this.uiLine7.Name = "uiLine7";
            this.uiLine7.Size = new System.Drawing.Size(200, 20);
            this.uiLine7.Style = Sunny.UI.UIStyle.Orange;
            this.uiLine7.TabIndex = 36;
            this.uiLine7.Text = "I2C Driver Version";
            this.uiLine7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLine7.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiTextBox_daemon_ver
            // 
            this.uiTextBox_daemon_ver.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTextBox_daemon_ver.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.uiTextBox_daemon_ver.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.uiTextBox_daemon_ver.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTextBox_daemon_ver.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.uiTextBox_daemon_ver.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.uiTextBox_daemon_ver.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.uiTextBox_daemon_ver.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiTextBox_daemon_ver.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiTextBox_daemon_ver.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.uiTextBox_daemon_ver.Location = new System.Drawing.Point(0, 20);
            this.uiTextBox_daemon_ver.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTextBox_daemon_ver.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTextBox_daemon_ver.Name = "uiTextBox_daemon_ver";
            this.uiTextBox_daemon_ver.ReadOnly = true;
            this.uiTextBox_daemon_ver.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTextBox_daemon_ver.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTextBox_daemon_ver.ShowText = false;
            this.uiTextBox_daemon_ver.Size = new System.Drawing.Size(200, 28);
            this.uiTextBox_daemon_ver.Style = Sunny.UI.UIStyle.Orange;
            this.uiTextBox_daemon_ver.TabIndex = 35;
            this.uiTextBox_daemon_ver.Text = "Daemon Version";
            this.uiTextBox_daemon_ver.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiTextBox_daemon_ver.Watermark = "";
            this.uiTextBox_daemon_ver.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLine6
            // 
            this.uiLine6.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiLine6.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiLine6.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiLine6.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiLine6.Location = new System.Drawing.Point(0, 0);
            this.uiLine6.MinimumSize = new System.Drawing.Size(2, 2);
            this.uiLine6.Name = "uiLine6";
            this.uiLine6.Size = new System.Drawing.Size(200, 20);
            this.uiLine6.Style = Sunny.UI.UIStyle.Orange;
            this.uiLine6.TabIndex = 34;
            this.uiLine6.Text = "Daemon Version";
            this.uiLine6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLine6.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLedStopwatch
            // 
            this.uiLedStopwatch.BackColor = System.Drawing.Color.Black;
            this.uiLedStopwatch.CharCount = 8;
            this.uiLedStopwatch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiLedStopwatch.ForeColor = System.Drawing.Color.Lime;
            this.uiLedStopwatch.IntervalH = 5;
            this.uiLedStopwatch.Location = new System.Drawing.Point(0, 26);
            this.uiLedStopwatch.Name = "uiLedStopwatch";
            this.uiLedStopwatch.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.uiLedStopwatch.ShowType = Sunny.UI.UILedStopwatch.TimeShowType.hhmmss;
            this.uiLedStopwatch.Size = new System.Drawing.Size(200, 34);
            this.uiLedStopwatch.TabIndex = 40;
            this.uiLedStopwatch.Text = "00:00";
            // 
            // uiGroupBox_wifi_setting
            // 
            this.uiGroupBox_wifi_setting.Controls.Add(this.panel_wifi_setting);
            this.uiGroupBox_wifi_setting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox_wifi_setting.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiGroupBox_wifi_setting.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiGroupBox_wifi_setting.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.uiGroupBox_wifi_setting.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox_wifi_setting.Margin = new System.Windows.Forms.Padding(10);
            this.uiGroupBox_wifi_setting.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox_wifi_setting.Name = "uiGroupBox_wifi_setting";
            this.uiGroupBox_wifi_setting.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox_wifi_setting.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiGroupBox_wifi_setting.Size = new System.Drawing.Size(308, 532);
            this.uiGroupBox_wifi_setting.Style = Sunny.UI.UIStyle.Orange;
            this.uiGroupBox_wifi_setting.TabIndex = 32;
            this.uiGroupBox_wifi_setting.Text = "Wifi ITS Setting Panel";
            this.uiGroupBox_wifi_setting.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiGroupBox_wifi_setting.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // panel_wifi_setting
            // 
            this.panel_wifi_setting.AutoScroll = true;
            this.panel_wifi_setting.AutoScrollMinSize = new System.Drawing.Size(0, 500);
            this.panel_wifi_setting.BackColor = System.Drawing.Color.Transparent;
            this.panel_wifi_setting.Controls.Add(this.tabControl_WifiSetting);
            this.panel_wifi_setting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_wifi_setting.Location = new System.Drawing.Point(0, 32);
            this.panel_wifi_setting.Name = "panel_wifi_setting";
            this.panel_wifi_setting.Size = new System.Drawing.Size(308, 500);
            this.panel_wifi_setting.TabIndex = 1;
            // 
            // uiToolTip_adb_devices
            // 
            this.uiToolTip_adb_devices.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.uiToolTip_adb_devices.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.uiToolTip_adb_devices.OwnerDraw = true;
            // 
            // WifiSettingfm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.ClientSize = new System.Drawing.Size(308, 532);
            this.Controls.Add(this.uiGroupBox_wifi_setting);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WifiSettingfm";
            this.Text = "WifiSettingfm";
            this.Shown += new System.EventHandler(this.WifiSettingfm_Shown);
            this.tabControl_WifiSetting.ResumeLayout(false);
            this.tab_WifiSettingMain.ResumeLayout(false);
            this.uiPanel_gen_script.ResumeLayout(false);
            this.uiTableLayoutPanel4.ResumeLayout(false);
            this.uiTableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btn_ToolPath)).EndInit();
            this.uiTitlePanel_advance_setting.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tab_AdbTcpipSetting.ResumeLayout(false);
            this.tab_AdbTcpipSetting.PerformLayout();
            this.tab_WifiITSInfo.ResumeLayout(false);
            this.uiGroupBox_wifi_setting.ResumeLayout(false);
            this.panel_wifi_setting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBox_Get_Client_IP;
        private System.Windows.Forms.TextBox textBox_RemoteDeviceIP;
        private System.Windows.Forms.Button btn_adb_device_list;
        private System.Windows.Forms.TabPage tab_WifiSettingMain;
        private System.Windows.Forms.TabPage tab_AdbTcpipSetting;
        private Sunny.UI.UIComboBox cbBox_AccessType;
        private Sunny.UI.UILine uiLine4;
        private Sunny.UI.UIComboBox cbBox_ToolPath;
        private Sunny.UI.UILine uiLine3;
        private Sunny.UI.UIImageButton btn_ToolPath;
        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel4;
        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel2;
        private Sunny.UI.UILine uiLine5;
        private Sunny.UI.UISymbolButton button_Set_ADB_Tcpip;
        private Sunny.UI.UISymbolButton button_Get_Client_IP;
        private System.Windows.Forms.ListBox listBox_adb_devices;
        private Sunny.UI.UIGroupBox uiGroupBox_wifi_setting;
        private Sunny.UI.UISymbolButton button_wifiGo;
        private Sunny.UI.UILine uiLine_adb_devices;
        private Sunny.UI.UIPanel uiPanel_gen_script;
        private Sunny.UI.UICheckBox cb_INT_ack;
        private Sunny.UI.UITabControl tabControl_WifiSetting;
        private System.Windows.Forms.TabPage tab_WifiITSInfo;
        private Sunny.UI.UITextBox uiTextBox_driver_ver;
        private Sunny.UI.UILine uiLine7;
        private Sunny.UI.UITextBox uiTextBox_daemon_ver;
        private Sunny.UI.UILine uiLine6;
        private System.Windows.Forms.Panel panel_wifi_setting;
        private Sunny.UI.UIToolTip uiToolTip_adb_devices;
        private Sunny.UI.UILine uiLine8;
        private Sunny.UI.UILedStopwatch uiLedStopwatch;
        private Sunny.UI.UITitlePanel uiTitlePanel_advance_setting;
        private Sunny.UI.UICheckBox uiCheckBox_wifi_monitor;
        private Sunny.UI.UIComboBox cbBox_Method;
        private Sunny.UI.UILine uiLine9;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Sunny.UI.UICheckBox uiCheckBox_Interface;
        private Sunny.UI.UIComboBox cbBox_Interface;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Sunny.UI.UICheckBox uiCheckBox_Netlink;
        private System.Windows.Forms.TextBox textBox_Netlink;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private Sunny.UI.UICheckBox uiCheckBox_Vid;
        private System.Windows.Forms.TextBox textBox_Vid;
    }
}