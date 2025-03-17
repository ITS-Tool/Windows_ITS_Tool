namespace ITS_Studio.AllForms
{
    partial class Paintfm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_panAAscen = new System.Windows.Forms.Panel();
            this.MyPanelTest = new System.Windows.Forms.Panel();
            this.panel_Hover = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel_TipSwitch = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_BarrelSwitch = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_Erase = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_Invert = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_InRange = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_ReportRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_Pressure = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_X = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_Y = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_X_Tilt = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_Y_Tilt = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_Tilt = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_Azimuth = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_GapRatio = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_Jitter = new System.Windows.Forms.ToolStripStatusLabel();
            this.Main_MenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Clear_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Resolution_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Mode_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BackgroundColor_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PatternSelect_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Option_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Spin_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Pressure_ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.penTiltTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Performance_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SwapXY_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.touchUpCircle_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Hover_ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.penStatus_BarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowTrajectoryDot = new System.Windows.Forms.ToolStripMenuItem();
            this.Paint_Record = new System.Windows.Forms.ToolStripMenuItem();
            this.m_timer_CloseMonitor = new System.Windows.Forms.Timer(this.components);
            this.splitContainer_PaintMain = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.uiTabControl1 = new Sunny.UI.UITabControl();
            this.tabPage_TouchInfo = new System.Windows.Forms.TabPage();
            this.splitContainer_TouchInfo = new System.Windows.Forms.SplitContainer();
            this.m_dgvAlgoData = new System.Windows.Forms.DataGridView();
            this.m_dgvAlgoCol_Data = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgvFingerStatus = new System.Windows.Forms.DataGridView();
            this.m_dgvFigerCol_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgvFigerCol_X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_dgvFigerCol_Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage_Key = new System.Windows.Forms.TabPage();
            this.m_dgvKeyStatus = new System.Windows.Forms.DataGridView();
            this.m_dgvKeyCol_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage_DebugInfo = new System.Windows.Forms.TabPage();
            this.splitContainer_DataGridView = new System.Windows.Forms.SplitContainer();
            this.dataGridView_FingerDebugInfo = new System.Windows.Forms.DataGridView();
            this.dataGridView_PenDebugInfo = new System.Windows.Forms.DataGridView();
            this.uiPanel_OnAIR = new Sunny.UI.UIPanel();
            this.uiWaitingBar1 = new Sunny.UI.UIWaitingBar();
            this.uiSymbolLabel1 = new Sunny.UI.UISymbolLabel();
            this.splitKortek = new System.Windows.Forms.SplitContainer();
            this.m_dgvKortek = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.userControl_ReplayPaint = new CSharp_PlayBar.UserControl_PlayBar();
            this.m_panAAscen.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.Main_MenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_PaintMain)).BeginInit();
            this.splitContainer_PaintMain.Panel1.SuspendLayout();
            this.splitContainer_PaintMain.Panel2.SuspendLayout();
            this.splitContainer_PaintMain.SuspendLayout();
            this.panel2.SuspendLayout();
            this.uiTabControl1.SuspendLayout();
            this.tabPage_TouchInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_TouchInfo)).BeginInit();
            this.splitContainer_TouchInfo.Panel1.SuspendLayout();
            this.splitContainer_TouchInfo.Panel2.SuspendLayout();
            this.splitContainer_TouchInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvAlgoData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvFingerStatus)).BeginInit();
            this.tabPage_Key.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvKeyStatus)).BeginInit();
            this.tabPage_DebugInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_DataGridView)).BeginInit();
            this.splitContainer_DataGridView.Panel1.SuspendLayout();
            this.splitContainer_DataGridView.Panel2.SuspendLayout();
            this.splitContainer_DataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_FingerDebugInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_PenDebugInfo)).BeginInit();
            this.uiPanel_OnAIR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitKortek)).BeginInit();
            this.splitKortek.Panel1.SuspendLayout();
            this.splitKortek.Panel2.SuspendLayout();
            this.splitKortek.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvKortek)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_panAAscen
            // 
            this.m_panAAscen.BackColor = System.Drawing.Color.Black;
            this.m_panAAscen.Controls.Add(this.MyPanelTest);
            this.m_panAAscen.Controls.Add(this.panel_Hover);
            this.m_panAAscen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_panAAscen.Location = new System.Drawing.Point(0, 0);
            this.m_panAAscen.Name = "m_panAAscen";
            this.m_panAAscen.Size = new System.Drawing.Size(1636, 767);
            this.m_panAAscen.TabIndex = 0;
            this.m_panAAscen.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPanelAAScreen_Paint);
            // 
            // MyPanelTest
            // 
            this.MyPanelTest.BackColor = System.Drawing.Color.Transparent;
            this.MyPanelTest.Location = new System.Drawing.Point(583, 431);
            this.MyPanelTest.Name = "MyPanelTest";
            this.MyPanelTest.Size = new System.Drawing.Size(51, 44);
            this.MyPanelTest.TabIndex = 2;
            this.MyPanelTest.Visible = false;
            // 
            // panel_Hover
            // 
            this.panel_Hover.BackColor = System.Drawing.Color.Transparent;
            this.panel_Hover.BackgroundImage = global::ITS_Studio.Properties.Resources.HoverCircle;
            this.panel_Hover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_Hover.Location = new System.Drawing.Point(63, 79);
            this.panel_Hover.Margin = new System.Windows.Forms.Padding(4);
            this.panel_Hover.Name = "panel_Hover";
            this.panel_Hover.Size = new System.Drawing.Size(36, 39);
            this.panel_Hover.TabIndex = 0;
            this.panel_Hover.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel_TipSwitch,
            this.StatusLabel_BarrelSwitch,
            this.StatusLabel_Erase,
            this.StatusLabel_Invert,
            this.StatusLabel_InRange,
            this.StatusLabel_ReportRate,
            this.StatusLabel_Pressure,
            this.StatusLabel_X,
            this.StatusLabel_Y,
            this.StatusLabel_X_Tilt,
            this.StatusLabel_Y_Tilt,
            this.StatusLabel_Tilt,
            this.StatusLabel_Azimuth,
            this.StatusLabel_GapRatio,
            this.StatusLabel_Jitter});
            this.statusStrip1.Location = new System.Drawing.Point(0, 921);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1636, 24);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel_TipSwitch
            // 
            this.StatusLabel_TipSwitch.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusLabel_TipSwitch.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.StatusLabel_TipSwitch.Name = "StatusLabel_TipSwitch";
            this.StatusLabel_TipSwitch.Size = new System.Drawing.Size(68, 19);
            this.StatusLabel_TipSwitch.Text = "Tip Switch";
            // 
            // StatusLabel_BarrelSwitch
            // 
            this.StatusLabel_BarrelSwitch.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusLabel_BarrelSwitch.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.StatusLabel_BarrelSwitch.Name = "StatusLabel_BarrelSwitch";
            this.StatusLabel_BarrelSwitch.Size = new System.Drawing.Size(82, 19);
            this.StatusLabel_BarrelSwitch.Text = "Barrel Switch";
            // 
            // StatusLabel_Erase
            // 
            this.StatusLabel_Erase.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusLabel_Erase.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.StatusLabel_Erase.Name = "StatusLabel_Erase";
            this.StatusLabel_Erase.Size = new System.Drawing.Size(45, 19);
            this.StatusLabel_Erase.Text = "Eraser";
            // 
            // StatusLabel_Invert
            // 
            this.StatusLabel_Invert.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusLabel_Invert.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.StatusLabel_Invert.Name = "StatusLabel_Invert";
            this.StatusLabel_Invert.Size = new System.Drawing.Size(42, 19);
            this.StatusLabel_Invert.Text = "Invert";
            // 
            // StatusLabel_InRange
            // 
            this.StatusLabel_InRange.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusLabel_InRange.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.StatusLabel_InRange.Name = "StatusLabel_InRange";
            this.StatusLabel_InRange.Size = new System.Drawing.Size(61, 19);
            this.StatusLabel_InRange.Text = "In Range";
            // 
            // StatusLabel_ReportRate
            // 
            this.StatusLabel_ReportRate.BackColor = System.Drawing.Color.Black;
            this.StatusLabel_ReportRate.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusLabel_ReportRate.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.StatusLabel_ReportRate.ForeColor = System.Drawing.Color.Lime;
            this.StatusLabel_ReportRate.Name = "StatusLabel_ReportRate";
            this.StatusLabel_ReportRate.Size = new System.Drawing.Size(4, 19);
            // 
            // StatusLabel_Pressure
            // 
            this.StatusLabel_Pressure.ForeColor = System.Drawing.Color.Red;
            this.StatusLabel_Pressure.Name = "StatusLabel_Pressure";
            this.StatusLabel_Pressure.Size = new System.Drawing.Size(0, 19);
            // 
            // StatusLabel_X
            // 
            this.StatusLabel_X.ForeColor = System.Drawing.Color.Red;
            this.StatusLabel_X.Name = "StatusLabel_X";
            this.StatusLabel_X.Size = new System.Drawing.Size(0, 19);
            // 
            // StatusLabel_Y
            // 
            this.StatusLabel_Y.ForeColor = System.Drawing.Color.Red;
            this.StatusLabel_Y.Name = "StatusLabel_Y";
            this.StatusLabel_Y.Size = new System.Drawing.Size(0, 19);
            // 
            // StatusLabel_X_Tilt
            // 
            this.StatusLabel_X_Tilt.ForeColor = System.Drawing.Color.Red;
            this.StatusLabel_X_Tilt.Name = "StatusLabel_X_Tilt";
            this.StatusLabel_X_Tilt.Size = new System.Drawing.Size(0, 19);
            // 
            // StatusLabel_Y_Tilt
            // 
            this.StatusLabel_Y_Tilt.ForeColor = System.Drawing.Color.Red;
            this.StatusLabel_Y_Tilt.Name = "StatusLabel_Y_Tilt";
            this.StatusLabel_Y_Tilt.Size = new System.Drawing.Size(0, 19);
            // 
            // StatusLabel_Tilt
            // 
            this.StatusLabel_Tilt.ForeColor = System.Drawing.Color.Red;
            this.StatusLabel_Tilt.Name = "StatusLabel_Tilt";
            this.StatusLabel_Tilt.Size = new System.Drawing.Size(0, 19);
            // 
            // StatusLabel_Azimuth
            // 
            this.StatusLabel_Azimuth.ForeColor = System.Drawing.Color.Red;
            this.StatusLabel_Azimuth.Name = "StatusLabel_Azimuth";
            this.StatusLabel_Azimuth.Size = new System.Drawing.Size(0, 19);
            // 
            // StatusLabel_GapRatio
            // 
            this.StatusLabel_GapRatio.ForeColor = System.Drawing.Color.Red;
            this.StatusLabel_GapRatio.Name = "StatusLabel_GapRatio";
            this.StatusLabel_GapRatio.Size = new System.Drawing.Size(0, 19);
            // 
            // StatusLabel_Jitter
            // 
            this.StatusLabel_Jitter.ForeColor = System.Drawing.Color.Red;
            this.StatusLabel_Jitter.Name = "StatusLabel_Jitter";
            this.StatusLabel_Jitter.Size = new System.Drawing.Size(0, 19);
            // 
            // Main_MenuStrip
            // 
            this.Main_MenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.Main_MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Clear_ToolStripMenuItem,
            this.Resolution_ToolStripMenuItem,
            this.Mode_ToolStripMenuItem,
            this.BackgroundColor_ToolStripMenuItem,
            this.PatternSelect_ToolStripMenuItem,
            this.Option_ToolStripMenuItem,
            this.Spin_ToolStripMenuItem,
            this.Pressure_ToolStripMenuItem1,
            this.penTiltTypeToolStripMenuItem,
            this.toolStripSeparator1,
            this.Performance_ToolStripMenuItem,
            this.SwapXY_ToolStripMenuItem,
            this.touchUpCircle_ToolStripMenuItem,
            this.Hover_ToolStripMenuItem1,
            this.penStatus_BarToolStripMenuItem,
            this.ShowTrajectoryDot,
            this.Paint_Record});
            this.Main_MenuStrip.Name = "contextMenuStrip1";
            this.Main_MenuStrip.Size = new System.Drawing.Size(189, 362);
            // 
            // Clear_ToolStripMenuItem
            // 
            this.Clear_ToolStripMenuItem.Name = "Clear_ToolStripMenuItem";
            this.Clear_ToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.Clear_ToolStripMenuItem.Text = "Clear (C)";
            this.Clear_ToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // Resolution_ToolStripMenuItem
            // 
            this.Resolution_ToolStripMenuItem.Name = "Resolution_ToolStripMenuItem";
            this.Resolution_ToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.Resolution_ToolStripMenuItem.Text = "Resolution";
            // 
            // Mode_ToolStripMenuItem
            // 
            this.Mode_ToolStripMenuItem.Name = "Mode_ToolStripMenuItem";
            this.Mode_ToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.Mode_ToolStripMenuItem.Text = "Mode";
            // 
            // BackgroundColor_ToolStripMenuItem
            // 
            this.BackgroundColor_ToolStripMenuItem.Name = "BackgroundColor_ToolStripMenuItem";
            this.BackgroundColor_ToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.BackgroundColor_ToolStripMenuItem.Text = "Background Color";
            // 
            // PatternSelect_ToolStripMenuItem
            // 
            this.PatternSelect_ToolStripMenuItem.Name = "PatternSelect_ToolStripMenuItem";
            this.PatternSelect_ToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.PatternSelect_ToolStripMenuItem.Text = "Pattern";
            // 
            // Option_ToolStripMenuItem
            // 
            this.Option_ToolStripMenuItem.Name = "Option_ToolStripMenuItem";
            this.Option_ToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.Option_ToolStripMenuItem.Text = "Option";
            // 
            // Spin_ToolStripMenuItem
            // 
            this.Spin_ToolStripMenuItem.Name = "Spin_ToolStripMenuItem";
            this.Spin_ToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.Spin_ToolStripMenuItem.Text = "Spin";
            // 
            // Pressure_ToolStripMenuItem1
            // 
            this.Pressure_ToolStripMenuItem1.Name = "Pressure_ToolStripMenuItem1";
            this.Pressure_ToolStripMenuItem1.Size = new System.Drawing.Size(188, 22);
            this.Pressure_ToolStripMenuItem1.Text = "Pen Pressure Effect";
            // 
            // penTiltTypeToolStripMenuItem
            // 
            this.penTiltTypeToolStripMenuItem.Name = "penTiltTypeToolStripMenuItem";
            this.penTiltTypeToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.penTiltTypeToolStripMenuItem.Text = "Pen Tilt Type";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
            // 
            // Performance_ToolStripMenuItem
            // 
            this.Performance_ToolStripMenuItem.CheckOnClick = true;
            this.Performance_ToolStripMenuItem.Name = "Performance_ToolStripMenuItem";
            this.Performance_ToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.Performance_ToolStripMenuItem.Text = "Performance";
            this.Performance_ToolStripMenuItem.Click += new System.EventHandler(this.Performance_ToolStripMenuItem_Click);
            // 
            // SwapXY_ToolStripMenuItem
            // 
            this.SwapXY_ToolStripMenuItem.CheckOnClick = true;
            this.SwapXY_ToolStripMenuItem.Name = "SwapXY_ToolStripMenuItem";
            this.SwapXY_ToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.SwapXY_ToolStripMenuItem.Text = "Swap X Y";
            this.SwapXY_ToolStripMenuItem.Click += new System.EventHandler(this.SwapXY_ToolStripMenuItem_Click);
            // 
            // touchUpCircle_ToolStripMenuItem
            // 
            this.touchUpCircle_ToolStripMenuItem.CheckOnClick = true;
            this.touchUpCircle_ToolStripMenuItem.Name = "touchUpCircle_ToolStripMenuItem";
            this.touchUpCircle_ToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.touchUpCircle_ToolStripMenuItem.Text = "Touch Up Circle";
            this.touchUpCircle_ToolStripMenuItem.Click += new System.EventHandler(this.MenuItemTouchUp_Circle_Enable_Click);
            // 
            // Hover_ToolStripMenuItem1
            // 
            this.Hover_ToolStripMenuItem1.CheckOnClick = true;
            this.Hover_ToolStripMenuItem1.Name = "Hover_ToolStripMenuItem1";
            this.Hover_ToolStripMenuItem1.Size = new System.Drawing.Size(188, 22);
            this.Hover_ToolStripMenuItem1.Text = "Hover Circle";
            this.Hover_ToolStripMenuItem1.Click += new System.EventHandler(this.Hover_ToolStripMenuItem1_Click);
            // 
            // penStatus_BarToolStripMenuItem
            // 
            this.penStatus_BarToolStripMenuItem.CheckOnClick = true;
            this.penStatus_BarToolStripMenuItem.Name = "penStatus_BarToolStripMenuItem";
            this.penStatus_BarToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.penStatus_BarToolStripMenuItem.Text = "Pen Status Bar";
            this.penStatus_BarToolStripMenuItem.Click += new System.EventHandler(this.MenuItemTouchUp_PenStatusBar_Enable_Click);
            // 
            // ShowTrajectoryDot
            // 
            this.ShowTrajectoryDot.CheckOnClick = true;
            this.ShowTrajectoryDot.Name = "ShowTrajectoryDot";
            this.ShowTrajectoryDot.Size = new System.Drawing.Size(188, 22);
            this.ShowTrajectoryDot.Text = "Show Trajectory Dot";
            this.ShowTrajectoryDot.Click += new System.EventHandler(this.ShowTrajectoryDot_Click);
            // 
            // Paint_Record
            // 
            this.Paint_Record.CheckOnClick = true;
            this.Paint_Record.Name = "Paint_Record";
            this.Paint_Record.Size = new System.Drawing.Size(188, 22);
            this.Paint_Record.Text = "Record  (Ctrl+R)";
            this.Paint_Record.Click += new System.EventHandler(this.Paint_Record_Click);
            // 
            // m_timer_CloseMonitor
            // 
            this.m_timer_CloseMonitor.Interval = 1000;
            this.m_timer_CloseMonitor.Tick += new System.EventHandler(this.m_timer_CloseMonitor_Tick);
            // 
            // splitContainer_PaintMain
            // 
            this.splitContainer_PaintMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.splitContainer_PaintMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_PaintMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_PaintMain.IsSplitterFixed = true;
            this.splitContainer_PaintMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_PaintMain.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer_PaintMain.Name = "splitContainer_PaintMain";
            // 
            // splitContainer_PaintMain.Panel1
            // 
            this.splitContainer_PaintMain.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer_PaintMain.Panel2
            // 
            this.splitContainer_PaintMain.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer_PaintMain.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.splitContainer_PaintMain.Panel2.Controls.Add(this.splitKortek);
            this.splitContainer_PaintMain.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer_PaintMain.Size = new System.Drawing.Size(1942, 945);
            this.splitContainer_PaintMain.SplitterDistance = 300;
            this.splitContainer_PaintMain.SplitterWidth = 6;
            this.splitContainer_PaintMain.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.uiTabControl1);
            this.panel2.Controls.Add(this.uiPanel_OnAIR);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(300, 945);
            this.panel2.TabIndex = 2;
            // 
            // uiTabControl1
            // 
            this.uiTabControl1.Controls.Add(this.tabPage_TouchInfo);
            this.uiTabControl1.Controls.Add(this.tabPage_Key);
            this.uiTabControl1.Controls.Add(this.tabPage_DebugInfo);
            this.uiTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiTabControl1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiTabControl1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiTabControl1.Frame = null;
            this.uiTabControl1.ItemSize = new System.Drawing.Size(100, 20);
            this.uiTabControl1.Location = new System.Drawing.Point(0, 84);
            this.uiTabControl1.MainPage = "";
            this.uiTabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.uiTabControl1.MenuStyle = Sunny.UI.UIMenuStyle.White;
            this.uiTabControl1.Name = "uiTabControl1";
            this.uiTabControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.uiTabControl1.SelectedIndex = 0;
            this.uiTabControl1.Size = new System.Drawing.Size(296, 857);
            this.uiTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiTabControl1.Style = Sunny.UI.UIStyle.Orange;
            this.uiTabControl1.TabBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.uiTabControl1.TabIndex = 3;
            this.uiTabControl1.TabSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.uiTabControl1.TabSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTabControl1.TabSelectedHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTabControl1.TabStop = false;
            this.uiTabControl1.TabUnSelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiTabControl1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiTabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage_TouchInfo
            // 
            this.tabPage_TouchInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.tabPage_TouchInfo.Controls.Add(this.splitContainer_TouchInfo);
            this.tabPage_TouchInfo.Location = new System.Drawing.Point(0, 20);
            this.tabPage_TouchInfo.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_TouchInfo.Name = "tabPage_TouchInfo";
            this.tabPage_TouchInfo.Size = new System.Drawing.Size(296, 837);
            this.tabPage_TouchInfo.TabIndex = 0;
            this.tabPage_TouchInfo.Text = "Touch Info";
            // 
            // splitContainer_TouchInfo
            // 
            this.splitContainer_TouchInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_TouchInfo.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_TouchInfo.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_TouchInfo.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer_TouchInfo.Name = "splitContainer_TouchInfo";
            this.splitContainer_TouchInfo.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_TouchInfo.Panel1
            // 
            this.splitContainer_TouchInfo.Panel1.Controls.Add(this.m_dgvAlgoData);
            // 
            // splitContainer_TouchInfo.Panel2
            // 
            this.splitContainer_TouchInfo.Panel2.Controls.Add(this.m_dgvFingerStatus);
            this.splitContainer_TouchInfo.Size = new System.Drawing.Size(296, 837);
            this.splitContainer_TouchInfo.SplitterDistance = 289;
            this.splitContainer_TouchInfo.SplitterWidth = 6;
            this.splitContainer_TouchInfo.TabIndex = 3;
            // 
            // m_dgvAlgoData
            // 
            this.m_dgvAlgoData.AllowUserToAddRows = false;
            this.m_dgvAlgoData.AllowUserToDeleteRows = false;
            this.m_dgvAlgoData.AllowUserToResizeColumns = false;
            this.m_dgvAlgoData.AllowUserToResizeRows = false;
            this.m_dgvAlgoData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_dgvAlgoData.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgvAlgoData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.m_dgvAlgoData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dgvAlgoData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_dgvAlgoCol_Data});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgvAlgoData.DefaultCellStyle = dataGridViewCellStyle2;
            this.m_dgvAlgoData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgvAlgoData.Location = new System.Drawing.Point(0, 0);
            this.m_dgvAlgoData.Margin = new System.Windows.Forms.Padding(4);
            this.m_dgvAlgoData.MultiSelect = false;
            this.m_dgvAlgoData.Name = "m_dgvAlgoData";
            this.m_dgvAlgoData.ReadOnly = true;
            this.m_dgvAlgoData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.m_dgvAlgoData.RowTemplate.Height = 24;
            this.m_dgvAlgoData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_dgvAlgoData.Size = new System.Drawing.Size(296, 289);
            this.m_dgvAlgoData.TabIndex = 2;
            // 
            // m_dgvAlgoCol_Data
            // 
            this.m_dgvAlgoCol_Data.HeaderText = "Data";
            this.m_dgvAlgoCol_Data.Name = "m_dgvAlgoCol_Data";
            this.m_dgvAlgoCol_Data.ReadOnly = true;
            this.m_dgvAlgoCol_Data.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_dgvFingerStatus
            // 
            this.m_dgvFingerStatus.AllowUserToAddRows = false;
            this.m_dgvFingerStatus.AllowUserToDeleteRows = false;
            this.m_dgvFingerStatus.AllowUserToResizeRows = false;
            this.m_dgvFingerStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            this.m_dgvFingerStatus.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgvFingerStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.m_dgvFingerStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dgvFingerStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_dgvFigerCol_Name,
            this.m_dgvFigerCol_X,
            this.m_dgvFigerCol_Y});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgvFingerStatus.DefaultCellStyle = dataGridViewCellStyle4;
            this.m_dgvFingerStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgvFingerStatus.Location = new System.Drawing.Point(0, 0);
            this.m_dgvFingerStatus.Margin = new System.Windows.Forms.Padding(4);
            this.m_dgvFingerStatus.MultiSelect = false;
            this.m_dgvFingerStatus.Name = "m_dgvFingerStatus";
            this.m_dgvFingerStatus.ReadOnly = true;
            this.m_dgvFingerStatus.RowHeadersVisible = false;
            this.m_dgvFingerStatus.RowTemplate.Height = 24;
            this.m_dgvFingerStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_dgvFingerStatus.Size = new System.Drawing.Size(296, 542);
            this.m_dgvFingerStatus.TabIndex = 1;
            // 
            // m_dgvFigerCol_Name
            // 
            this.m_dgvFigerCol_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.m_dgvFigerCol_Name.HeaderText = "";
            this.m_dgvFigerCol_Name.MinimumWidth = 25;
            this.m_dgvFigerCol_Name.Name = "m_dgvFigerCol_Name";
            this.m_dgvFigerCol_Name.ReadOnly = true;
            this.m_dgvFigerCol_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.m_dgvFigerCol_Name.Width = 25;
            // 
            // m_dgvFigerCol_X
            // 
            this.m_dgvFigerCol_X.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.m_dgvFigerCol_X.HeaderText = "X";
            this.m_dgvFigerCol_X.MinimumWidth = 40;
            this.m_dgvFigerCol_X.Name = "m_dgvFigerCol_X";
            this.m_dgvFigerCol_X.ReadOnly = true;
            this.m_dgvFigerCol_X.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.m_dgvFigerCol_X.Width = 40;
            // 
            // m_dgvFigerCol_Y
            // 
            this.m_dgvFigerCol_Y.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.m_dgvFigerCol_Y.HeaderText = "Y";
            this.m_dgvFigerCol_Y.MinimumWidth = 40;
            this.m_dgvFigerCol_Y.Name = "m_dgvFigerCol_Y";
            this.m_dgvFigerCol_Y.ReadOnly = true;
            this.m_dgvFigerCol_Y.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.m_dgvFigerCol_Y.Width = 40;
            // 
            // tabPage_Key
            // 
            this.tabPage_Key.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.tabPage_Key.Controls.Add(this.m_dgvKeyStatus);
            this.tabPage_Key.Location = new System.Drawing.Point(0, 40);
            this.tabPage_Key.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_Key.Name = "tabPage_Key";
            this.tabPage_Key.Size = new System.Drawing.Size(200, 60);
            this.tabPage_Key.TabIndex = 1;
            this.tabPage_Key.Text = "Key";
            // 
            // m_dgvKeyStatus
            // 
            this.m_dgvKeyStatus.AllowUserToAddRows = false;
            this.m_dgvKeyStatus.AllowUserToDeleteRows = false;
            this.m_dgvKeyStatus.AllowUserToResizeColumns = false;
            this.m_dgvKeyStatus.AllowUserToResizeRows = false;
            this.m_dgvKeyStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_dgvKeyStatus.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgvKeyStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.m_dgvKeyStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dgvKeyStatus.ColumnHeadersVisible = false;
            this.m_dgvKeyStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_dgvKeyCol_Name});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgvKeyStatus.DefaultCellStyle = dataGridViewCellStyle6;
            this.m_dgvKeyStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgvKeyStatus.Location = new System.Drawing.Point(0, 0);
            this.m_dgvKeyStatus.Margin = new System.Windows.Forms.Padding(4);
            this.m_dgvKeyStatus.Name = "m_dgvKeyStatus";
            this.m_dgvKeyStatus.ReadOnly = true;
            this.m_dgvKeyStatus.RowHeadersVisible = false;
            this.m_dgvKeyStatus.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgvKeyStatus.RowTemplate.Height = 24;
            this.m_dgvKeyStatus.Size = new System.Drawing.Size(200, 60);
            this.m_dgvKeyStatus.TabIndex = 5;
            this.m_dgvKeyStatus.SelectionChanged += new System.EventHandler(this.m_dgvKeyStatus_SelectionChanged);
            // 
            // m_dgvKeyCol_Name
            // 
            this.m_dgvKeyCol_Name.HeaderText = "Name";
            this.m_dgvKeyCol_Name.Name = "m_dgvKeyCol_Name";
            this.m_dgvKeyCol_Name.ReadOnly = true;
            // 
            // tabPage_DebugInfo
            // 
            this.tabPage_DebugInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.tabPage_DebugInfo.Controls.Add(this.splitContainer_DataGridView);
            this.tabPage_DebugInfo.Location = new System.Drawing.Point(0, 20);
            this.tabPage_DebugInfo.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage_DebugInfo.Name = "tabPage_DebugInfo";
            this.tabPage_DebugInfo.Size = new System.Drawing.Size(296, 837);
            this.tabPage_DebugInfo.TabIndex = 2;
            this.tabPage_DebugInfo.Text = "Debug Info";
            // 
            // splitContainer_DataGridView
            // 
            this.splitContainer_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_DataGridView.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_DataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer_DataGridView.Name = "splitContainer_DataGridView";
            this.splitContainer_DataGridView.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_DataGridView.Panel1
            // 
            this.splitContainer_DataGridView.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.splitContainer_DataGridView.Panel1.Controls.Add(this.dataGridView_FingerDebugInfo);
            // 
            // splitContainer_DataGridView.Panel2
            // 
            this.splitContainer_DataGridView.Panel2.Controls.Add(this.dataGridView_PenDebugInfo);
            this.splitContainer_DataGridView.Size = new System.Drawing.Size(296, 837);
            this.splitContainer_DataGridView.SplitterDistance = 348;
            this.splitContainer_DataGridView.SplitterWidth = 6;
            this.splitContainer_DataGridView.TabIndex = 2;
            // 
            // dataGridView_FingerDebugInfo
            // 
            this.dataGridView_FingerDebugInfo.AllowUserToAddRows = false;
            this.dataGridView_FingerDebugInfo.AllowUserToDeleteRows = false;
            this.dataGridView_FingerDebugInfo.AllowUserToResizeColumns = false;
            this.dataGridView_FingerDebugInfo.AllowUserToResizeRows = false;
            this.dataGridView_FingerDebugInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_FingerDebugInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_FingerDebugInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_FingerDebugInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_FingerDebugInfo.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_FingerDebugInfo.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.dataGridView_FingerDebugInfo.Name = "dataGridView_FingerDebugInfo";
            this.dataGridView_FingerDebugInfo.RowHeadersVisible = false;
            this.dataGridView_FingerDebugInfo.RowTemplate.Height = 24;
            this.dataGridView_FingerDebugInfo.Size = new System.Drawing.Size(296, 348);
            this.dataGridView_FingerDebugInfo.TabIndex = 0;
            // 
            // dataGridView_PenDebugInfo
            // 
            this.dataGridView_PenDebugInfo.AllowUserToAddRows = false;
            this.dataGridView_PenDebugInfo.AllowUserToDeleteRows = false;
            this.dataGridView_PenDebugInfo.AllowUserToResizeColumns = false;
            this.dataGridView_PenDebugInfo.AllowUserToResizeRows = false;
            this.dataGridView_PenDebugInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_PenDebugInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_PenDebugInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_PenDebugInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_PenDebugInfo.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_PenDebugInfo.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.dataGridView_PenDebugInfo.Name = "dataGridView_PenDebugInfo";
            this.dataGridView_PenDebugInfo.RowHeadersVisible = false;
            this.dataGridView_PenDebugInfo.RowTemplate.Height = 24;
            this.dataGridView_PenDebugInfo.Size = new System.Drawing.Size(296, 483);
            this.dataGridView_PenDebugInfo.TabIndex = 1;
            // 
            // uiPanel_OnAIR
            // 
            this.uiPanel_OnAIR.Controls.Add(this.uiWaitingBar1);
            this.uiPanel_OnAIR.Controls.Add(this.uiSymbolLabel1);
            this.uiPanel_OnAIR.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiPanel_OnAIR.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(240)))));
            this.uiPanel_OnAIR.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(240)))));
            this.uiPanel_OnAIR.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiPanel_OnAIR.Location = new System.Drawing.Point(0, 0);
            this.uiPanel_OnAIR.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.uiPanel_OnAIR.MinimumSize = new System.Drawing.Size(2, 2);
            this.uiPanel_OnAIR.Name = "uiPanel_OnAIR";
            this.uiPanel_OnAIR.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.uiPanel_OnAIR.Size = new System.Drawing.Size(296, 84);
            this.uiPanel_OnAIR.Style = Sunny.UI.UIStyle.Custom;
            this.uiPanel_OnAIR.StyleCustomMode = true;
            this.uiPanel_OnAIR.TabIndex = 3;
            this.uiPanel_OnAIR.Text = null;
            this.uiPanel_OnAIR.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiPanel_OnAIR.Visible = false;
            this.uiPanel_OnAIR.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiWaitingBar1
            // 
            this.uiWaitingBar1.BackColor = System.Drawing.Color.Transparent;
            this.uiWaitingBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiWaitingBar1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiWaitingBar1.Interval = 100;
            this.uiWaitingBar1.Location = new System.Drawing.Point(0, 30);
            this.uiWaitingBar1.MinimumSize = new System.Drawing.Size(70, 23);
            this.uiWaitingBar1.Name = "uiWaitingBar1";
            this.uiWaitingBar1.Radius = 20;
            this.uiWaitingBar1.Size = new System.Drawing.Size(296, 54);
            this.uiWaitingBar1.Style = Sunny.UI.UIStyle.LayuiRed;
            this.uiWaitingBar1.StyleCustomMode = true;
            this.uiWaitingBar1.TabIndex = 4;
            this.uiWaitingBar1.Tag = "";
            this.uiWaitingBar1.TagString = "";
            this.uiWaitingBar1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiSymbolLabel1
            // 
            this.uiSymbolLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.uiSymbolLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiSymbolLabel1.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiSymbolLabel1.ForeColor = System.Drawing.Color.Red;
            this.uiSymbolLabel1.Location = new System.Drawing.Point(0, 0);
            this.uiSymbolLabel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSymbolLabel1.Name = "uiSymbolLabel1";
            this.uiSymbolLabel1.Padding = new System.Windows.Forms.Padding(28, 0, 0, 0);
            this.uiSymbolLabel1.Size = new System.Drawing.Size(296, 30);
            this.uiSymbolLabel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiSymbolLabel1.StyleCustomMode = true;
            this.uiSymbolLabel1.Symbol = 57371;
            this.uiSymbolLabel1.SymbolColor = System.Drawing.Color.Red;
            this.uiSymbolLabel1.TabIndex = 0;
            this.uiSymbolLabel1.Text = "ON AIR";
            this.uiSymbolLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // splitKortek
            // 
            this.splitKortek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitKortek.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitKortek.Location = new System.Drawing.Point(0, 0);
            this.splitKortek.Name = "splitKortek";
            this.splitKortek.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitKortek.Panel1
            // 
            this.splitKortek.Panel1.Controls.Add(this.m_dgvKortek);
            // 
            // splitKortek.Panel2
            // 
            this.splitKortek.Panel2.Controls.Add(this.m_panAAscen);
            this.splitKortek.Size = new System.Drawing.Size(1636, 921);
            this.splitKortek.SplitterDistance = 150;
            this.splitKortek.TabIndex = 3;
            // 
            // m_dgvKortek
            // 
            this.m_dgvKortek.AllowUserToAddRows = false;
            this.m_dgvKortek.AllowUserToDeleteRows = false;
            this.m_dgvKortek.AllowUserToResizeColumns = false;
            this.m_dgvKortek.AllowUserToResizeRows = false;
            this.m_dgvKortek.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_dgvKortek.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgvKortek.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.m_dgvKortek.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgvKortek.DefaultCellStyle = dataGridViewCellStyle8;
            this.m_dgvKortek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgvKortek.Location = new System.Drawing.Point(0, 0);
            this.m_dgvKortek.Margin = new System.Windows.Forms.Padding(4);
            this.m_dgvKortek.MultiSelect = false;
            this.m_dgvKortek.Name = "m_dgvKortek";
            this.m_dgvKortek.ReadOnly = true;
            this.m_dgvKortek.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.m_dgvKortek.RowTemplate.Height = 24;
            this.m_dgvKortek.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_dgvKortek.Size = new System.Drawing.Size(1636, 150);
            this.m_dgvKortek.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer_PaintMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.userControl_ReplayPaint);
            this.splitContainer1.Size = new System.Drawing.Size(1942, 1100);
            this.splitContainer1.SplitterDistance = 945;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 1;
            // 
            // userControl_ReplayPaint
            // 
            this.userControl_ReplayPaint.b_CDC_ReplayUseNewFormat = false;
            this.userControl_ReplayPaint.b_Paint_ReplayUseNewFormat = false;
            this.userControl_ReplayPaint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.userControl_ReplayPaint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControl_ReplayPaint.FileName_Text = "";
            this.userControl_ReplayPaint.FrameNum_Text = "0";
            this.userControl_ReplayPaint.Location = new System.Drawing.Point(0, 0);
            this.userControl_ReplayPaint.Margin = new System.Windows.Forms.Padding(6);
            this.userControl_ReplayPaint.multiple = 1D;
            this.userControl_ReplayPaint.Name = "userControl_ReplayPaint";
            this.userControl_ReplayPaint.SetBtn_Next_Visible = false;
            this.userControl_ReplayPaint.SetBtn_Prev_Visible = false;
            this.userControl_ReplayPaint.Size = new System.Drawing.Size(1942, 149);
            this.userControl_ReplayPaint.TabIndex = 0;
            this.userControl_ReplayPaint.TrackBar_En = true;
            this.userControl_ReplayPaint.TrackBar_Max = 100;
            this.userControl_ReplayPaint.TrackBar_Min = 0;
            this.userControl_ReplayPaint.TrackBar_Value = 0;
            // 
            // Paintfm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1942, 1100);
            this.ContextMenuStrip = this.Main_MenuStrip;
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Paintfm";
            this.Text = "PaintTool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Paintfm_FormClosing);
            this.Load += new System.EventHandler(this.Paintfm_Load);
            this.Shown += new System.EventHandler(this.Paintfm_Shown);
            this.Resize += new System.EventHandler(this.Paintfm_Resize);
            this.m_panAAscen.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.Main_MenuStrip.ResumeLayout(false);
            this.splitContainer_PaintMain.Panel1.ResumeLayout(false);
            this.splitContainer_PaintMain.Panel2.ResumeLayout(false);
            this.splitContainer_PaintMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_PaintMain)).EndInit();
            this.splitContainer_PaintMain.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.uiTabControl1.ResumeLayout(false);
            this.tabPage_TouchInfo.ResumeLayout(false);
            this.splitContainer_TouchInfo.Panel1.ResumeLayout(false);
            this.splitContainer_TouchInfo.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_TouchInfo)).EndInit();
            this.splitContainer_TouchInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvAlgoData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvFingerStatus)).EndInit();
            this.tabPage_Key.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvKeyStatus)).EndInit();
            this.tabPage_DebugInfo.ResumeLayout(false);
            this.splitContainer_DataGridView.Panel1.ResumeLayout(false);
            this.splitContainer_DataGridView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_DataGridView)).EndInit();
            this.splitContainer_DataGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_FingerDebugInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_PenDebugInfo)).EndInit();
            this.uiPanel_OnAIR.ResumeLayout(false);
            this.splitKortek.Panel1.ResumeLayout(false);
            this.splitKortek.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitKortek)).EndInit();
            this.splitKortek.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvKortek)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel m_panAAscen;
        private System.Windows.Forms.ContextMenuStrip Main_MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem Clear_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Resolution_ToolStripMenuItem;
        //private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        public System.Windows.Forms.Timer m_timer_CloseMonitor;
        private System.Windows.Forms.SplitContainer splitContainer_PaintMain;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView m_dgvFingerStatus;
        private System.Windows.Forms.DataGridView m_dgvAlgoData;
        public System.Windows.Forms.ToolStripMenuItem Mode_ToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem Option_ToolStripMenuItem;
        private System.Windows.Forms.DataGridView m_dgvKeyStatus;
        private System.Windows.Forms.ToolStripMenuItem touchUpCircle_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Spin_ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_dgvKeyCol_Name;
        private System.Windows.Forms.SplitContainer splitContainer_TouchInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_dgvFigerCol_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_dgvFigerCol_X;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_dgvFigerCol_Y;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_BarrelSwitch;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Invert;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_InRange;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Pressure;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_TipSwitch;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Erase;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_X_Tilt;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Y_Tilt;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_ReportRate;
        private System.Windows.Forms.ToolStripMenuItem penStatus_BarToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Paint_Record;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_dgvAlgoCol_Data;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_X;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Y;
        private Sunny.UI.UIWaitingBar uiWaitingBar1;
        private Sunny.UI.UIPanel uiPanel_OnAIR;
        private Sunny.UI.UISymbolLabel uiSymbolLabel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private CSharp_PlayBar.UserControl_PlayBar userControl_ReplayPaint;
        private System.Windows.Forms.ToolStripMenuItem Performance_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BackgroundColor_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PatternSelect_ToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer_DataGridView;
        private System.Windows.Forms.DataGridView dataGridView_FingerDebugInfo;
        private System.Windows.Forms.DataGridView dataGridView_PenDebugInfo;
        private System.Windows.Forms.ToolStripMenuItem Pressure_ToolStripMenuItem1;
        private Sunny.UI.UITabControl uiTabControl1;
        private System.Windows.Forms.TabPage tabPage_TouchInfo;
        private System.Windows.Forms.TabPage tabPage_Key;
        private System.Windows.Forms.TabPage tabPage_DebugInfo;
        private System.Windows.Forms.Panel MyPanelTest;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Jitter;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_GapRatio;
        private System.Windows.Forms.SplitContainer splitKortek;
        private System.Windows.Forms.DataGridView m_dgvKortek;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Tilt;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Azimuth;
        private System.Windows.Forms.ToolStripMenuItem penTiltTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SwapXY_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowTrajectoryDot;
        private System.Windows.Forms.Panel panel_Hover;
        private System.Windows.Forms.ToolStripMenuItem Hover_ToolStripMenuItem1;
    }
}