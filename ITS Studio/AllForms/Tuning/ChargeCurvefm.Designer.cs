using ThirdPartyComponent;
namespace ITS_Studio.AllForms
{
    partial class ChargeCurvefm
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
            this.splitContainer_Main = new System.Windows.Forms.SplitContainer();
            this.GridView_Series = new System.Windows.Forms.DataGridView();
            this.tableLayout_Button = new System.Windows.Forms.TableLayoutPanel();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox_FixParams = new System.Windows.Forms.GroupBox();
            this.cbRecord = new System.Windows.Forms.CheckBox();
            this.tbFrame_Value = new System.Windows.Forms.TextBox();
            this.lbFrame_Name = new System.Windows.Forms.Label();
            this.cb_TwoShotEn = new System.Windows.Forms.CheckBox();
            this.uiGroupBox_CSub = new Sunny.UI.UIGroupBox();
            this.lbFixSC_X = new Sunny.UI.UIMarkLabel();
            this.tbFixSC_X = new Sunny.UI.UITextBox();
            this.lbFixSC_Y = new Sunny.UI.UIMarkLabel();
            this.tbFixSC_Y = new Sunny.UI.UITextBox();
            this.splitContainer_AllParams = new System.Windows.Forms.SplitContainer();
            this.gbSweepCharge = new Sunny.UI.UIGroupBox();
            this.lbSweepCharge_FixDumpUnit = new System.Windows.Forms.Label();
            this.lbSweepCharge_PostIdleUnit = new System.Windows.Forms.Label();
            this.tbSweepCharge_StepValue = new System.Windows.Forms.TextBox();
            this.tbSweepCharge_FixDumpValue = new System.Windows.Forms.TextBox();
            this.tbSweepCharge_PostIdleValue = new System.Windows.Forms.TextBox();
            this.tbSweepCharge_RangeEnd = new System.Windows.Forms.TextBox();
            this.tbSweepCharge_RangeStart = new System.Windows.Forms.TextBox();
            this.lbSweepCharge_RangeName = new System.Windows.Forms.Label();
            this.lbSweepCharge_Tidle = new System.Windows.Forms.Label();
            this.lbSweepCharge_RangeUnit = new System.Windows.Forms.Label();
            this.lbSweepCharge_StepName = new System.Windows.Forms.Label();
            this.lbSweepCharge_StepUnit = new System.Windows.Forms.Label();
            this.lbSweepCharge_PostIdleName = new System.Windows.Forms.Label();
            this.lbSweepCharge_FixDumpName = new System.Windows.Forms.Label();
            this.gbSweepDump_Freq = new Sunny.UI.UIGroupBox();
            this.lbSweepDump_FixChargeUnit = new System.Windows.Forms.Label();
            this.lbSweepDump_RangeName = new System.Windows.Forms.Label();
            this.lbSweepDump_PostIdleUnit = new System.Windows.Forms.Label();
            this.lbSweepDump_FixChargeName = new System.Windows.Forms.Label();
            this.tbSweepDump_StepValue = new System.Windows.Forms.TextBox();
            this.lbSweepDump_PostIdleName = new System.Windows.Forms.Label();
            this.tbSweepDump_FixChargeValue = new System.Windows.Forms.TextBox();
            this.lbSweepDump_StepUnit = new System.Windows.Forms.Label();
            this.tbSweepDump_PostIdleValue = new System.Windows.Forms.TextBox();
            this.lbSweepDump_StepName = new System.Windows.Forms.Label();
            this.tbSweepDump_RangeEnd = new System.Windows.Forms.TextBox();
            this.lbSweepDump_RangeUnit = new System.Windows.Forms.Label();
            this.tbSweepDump_RangeStart = new System.Windows.Forms.TextBox();
            this.lbSweepDump_Tidle = new System.Windows.Forms.Label();
            this.groupBox_ScanMode = new System.Windows.Forms.GroupBox();
            this.radioListBox_ChartDisp = new ThirdPartyComponent.RadioListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tbCtrl_Chart = new System.Windows.Forms.TabControl();
            this.tbOne_RawCode = new System.Windows.Forms.TabPage();
            this.tbCtl_One_RawCode = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chart_OneShot_FixDump = new Steema.TeeChart.TChart();
            this.fastLine1 = new Steema.TeeChart.Styles.FastLine();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chart_OneShot_FixCharge = new Steema.TeeChart.TChart();
            this.tbTwo_RawCode = new System.Windows.Forms.TabPage();
            this.tbCtl_Two_RawCode = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chart_TwoShot_Raw_FixDump = new Steema.TeeChart.TChart();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.chart_TwoShot_Raw_FixCharge = new Steema.TeeChart.TChart();
            this.tbTwo_DeltaC_Code = new System.Windows.Forms.TabPage();
            this.tbCtl_Two_DeltaC_Code = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.chart_TwoShot_DeltaCs_FixDump = new Steema.TeeChart.TChart();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.chart_TwoShot_DeltaCs_FixCharge = new Steema.TeeChart.TChart();
            this.tbTwo_DeltaC_Percent = new System.Windows.Forms.TabPage();
            this.tbCtl_Two_DeltaC_Percent = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.chart_TwoShot_DeltaCsPercent_FixDump = new Steema.TeeChart.TChart();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.chart_TwoShot_DeltaCsPercent_FixCharge = new Steema.TeeChart.TChart();
            this.richTextBox_msg = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_Max = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Avg = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).BeginInit();
            this.splitContainer_Main.Panel1.SuspendLayout();
            this.splitContainer_Main.Panel2.SuspendLayout();
            this.splitContainer_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridView_Series)).BeginInit();
            this.tableLayout_Button.SuspendLayout();
            this.groupBox_FixParams.SuspendLayout();
            this.uiGroupBox_CSub.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_AllParams)).BeginInit();
            this.splitContainer_AllParams.Panel1.SuspendLayout();
            this.splitContainer_AllParams.Panel2.SuspendLayout();
            this.splitContainer_AllParams.SuspendLayout();
            this.gbSweepCharge.SuspendLayout();
            this.gbSweepDump_Freq.SuspendLayout();
            this.groupBox_ScanMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tbCtrl_Chart.SuspendLayout();
            this.tbOne_RawCode.SuspendLayout();
            this.tbCtl_One_RawCode.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tbTwo_RawCode.SuspendLayout();
            this.tbCtl_Two_RawCode.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tbTwo_DeltaC_Code.SuspendLayout();
            this.tbCtl_Two_DeltaC_Code.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tbTwo_DeltaC_Percent.SuspendLayout();
            this.tbCtl_Two_DeltaC_Percent.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer_Main
            // 
            this.splitContainer_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Main.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_Main.IsSplitterFixed = true;
            this.splitContainer_Main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Main.Name = "splitContainer_Main";
            // 
            // splitContainer_Main.Panel1
            // 
            this.splitContainer_Main.Panel1.Controls.Add(this.GridView_Series);
            this.splitContainer_Main.Panel1.Controls.Add(this.tableLayout_Button);
            this.splitContainer_Main.Panel1.Controls.Add(this.groupBox_FixParams);
            this.splitContainer_Main.Panel1.Controls.Add(this.uiGroupBox_CSub);
            this.splitContainer_Main.Panel1.Controls.Add(this.splitContainer_AllParams);
            this.splitContainer_Main.Panel1.Controls.Add(this.groupBox_ScanMode);
            // 
            // splitContainer_Main.Panel2
            // 
            this.splitContainer_Main.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer_Main.Size = new System.Drawing.Size(1147, 586);
            this.splitContainer_Main.SplitterDistance = 392;
            this.splitContainer_Main.SplitterWidth = 1;
            this.splitContainer_Main.TabIndex = 1;
            // 
            // GridView_Series
            // 
            this.GridView_Series.AllowUserToAddRows = false;
            this.GridView_Series.AllowUserToDeleteRows = false;
            this.GridView_Series.AllowUserToResizeColumns = false;
            this.GridView_Series.AllowUserToResizeRows = false;
            this.GridView_Series.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridView_Series.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridView_Series.Location = new System.Drawing.Point(0, 268);
            this.GridView_Series.Name = "GridView_Series";
            this.GridView_Series.RowHeadersVisible = false;
            this.GridView_Series.RowTemplate.Height = 24;
            this.GridView_Series.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.GridView_Series.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.GridView_Series.Size = new System.Drawing.Size(392, 263);
            this.GridView_Series.TabIndex = 2;
            this.GridView_Series.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellContentClick);
            this.GridView_Series.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_Series_CellContentDoubleClick);
            this.GridView_Series.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellEndModify);
            this.GridView_Series.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellChangedValue);
            // 
            // tableLayout_Button
            // 
            this.tableLayout_Button.ColumnCount = 2;
            this.tableLayout_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout_Button.Controls.Add(this.btnRun, 0, 0);
            this.tableLayout_Button.Controls.Add(this.btnExit, 1, 0);
            this.tableLayout_Button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayout_Button.Location = new System.Drawing.Point(0, 531);
            this.tableLayout_Button.Name = "tableLayout_Button";
            this.tableLayout_Button.RowCount = 1;
            this.tableLayout_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_Button.Size = new System.Drawing.Size(392, 55);
            this.tableLayout_Button.TabIndex = 13;
            // 
            // btnRun
            // 
            this.btnRun.AutoSize = true;
            this.btnRun.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRun.Image = global::ITS_Studio.Properties.Resources.Play_Pressed_icon;
            this.btnRun.Location = new System.Drawing.Point(3, 3);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(190, 49);
            this.btnRun.TabIndex = 4;
            this.btnRun.Text = "Run";
            this.btnRun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Image = global::ITS_Studio.Properties.Resources.Arrow_Back_icon_64_64;
            this.btnExit.Location = new System.Drawing.Point(199, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(190, 49);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox_FixParams
            // 
            this.groupBox_FixParams.Controls.Add(this.cbRecord);
            this.groupBox_FixParams.Controls.Add(this.tbFrame_Value);
            this.groupBox_FixParams.Controls.Add(this.lbFrame_Name);
            this.groupBox_FixParams.Controls.Add(this.cb_TwoShotEn);
            this.groupBox_FixParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_FixParams.Enabled = false;
            this.groupBox_FixParams.Location = new System.Drawing.Point(0, 213);
            this.groupBox_FixParams.Name = "groupBox_FixParams";
            this.groupBox_FixParams.Size = new System.Drawing.Size(392, 55);
            this.groupBox_FixParams.TabIndex = 2;
            this.groupBox_FixParams.TabStop = false;
            this.groupBox_FixParams.Text = "Common Params";
            // 
            // cbRecord
            // 
            this.cbRecord.AutoSize = true;
            this.cbRecord.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbRecord.Location = new System.Drawing.Point(309, 20);
            this.cbRecord.Name = "cbRecord";
            this.cbRecord.Size = new System.Drawing.Size(78, 20);
            this.cbRecord.TabIndex = 27;
            this.cbRecord.Text = "Record";
            this.cbRecord.UseVisualStyleBackColor = true;
            // 
            // tbFrame_Value
            // 
            this.tbFrame_Value.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbFrame_Value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbFrame_Value.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbFrame_Value.ForeColor = System.Drawing.Color.Black;
            this.tbFrame_Value.Location = new System.Drawing.Point(69, 21);
            this.tbFrame_Value.MaxLength = 4;
            this.tbFrame_Value.Name = "tbFrame_Value";
            this.tbFrame_Value.Size = new System.Drawing.Size(30, 18);
            this.tbFrame_Value.TabIndex = 26;
            this.tbFrame_Value.Text = "1";
            this.tbFrame_Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbFrame_Value.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // lbFrame_Name
            // 
            this.lbFrame_Name.AutoSize = true;
            this.lbFrame_Name.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbFrame_Name.Location = new System.Drawing.Point(6, 24);
            this.lbFrame_Name.Name = "lbFrame_Name";
            this.lbFrame_Name.Size = new System.Drawing.Size(51, 12);
            this.lbFrame_Name.TabIndex = 6;
            this.lbFrame_Name.Text = "Frame : ";
            this.lbFrame_Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_TwoShotEn
            // 
            this.cb_TwoShotEn.AutoSize = true;
            this.cb_TwoShotEn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cb_TwoShotEn.Location = new System.Drawing.Point(109, 20);
            this.cb_TwoShotEn.Name = "cb_TwoShotEn";
            this.cb_TwoShotEn.Size = new System.Drawing.Size(149, 20);
            this.cb_TwoShotEn.TabIndex = 10;
            this.cb_TwoShotEn.Text = "Two Shot Enable";
            this.cb_TwoShotEn.UseVisualStyleBackColor = true;
            this.cb_TwoShotEn.CheckStateChanged += new System.EventHandler(this.cb_TwoShotEn_CheckStateChanged);
            // 
            // uiGroupBox_CSub
            // 
            this.uiGroupBox_CSub.AutoSize = true;
            this.uiGroupBox_CSub.Controls.Add(this.lbFixSC_X);
            this.uiGroupBox_CSub.Controls.Add(this.tbFixSC_X);
            this.uiGroupBox_CSub.Controls.Add(this.lbFixSC_Y);
            this.uiGroupBox_CSub.Controls.Add(this.tbFixSC_Y);
            this.uiGroupBox_CSub.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiGroupBox_CSub.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiGroupBox_CSub.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiGroupBox_CSub.Font = new System.Drawing.Font("新細明體", 9F);
            this.uiGroupBox_CSub.Location = new System.Drawing.Point(0, 148);
            this.uiGroupBox_CSub.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox_CSub.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox_CSub.Name = "uiGroupBox_CSub";
            this.uiGroupBox_CSub.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox_CSub.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiGroupBox_CSub.Size = new System.Drawing.Size(392, 65);
            this.uiGroupBox_CSub.Style = Sunny.UI.UIStyle.Orange;
            this.uiGroupBox_CSub.StyleCustomMode = true;
            this.uiGroupBox_CSub.TabIndex = 27;
            this.uiGroupBox_CSub.Text = "C-Sub";
            this.uiGroupBox_CSub.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiGroupBox_CSub.TitleInterval = 0;
            this.uiGroupBox_CSub.TitleTop = 8;
            this.uiGroupBox_CSub.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // lbFixSC_X
            // 
            this.lbFixSC_X.AutoSize = true;
            this.lbFixSC_X.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbFixSC_X.Location = new System.Drawing.Point(3, 16);
            this.lbFixSC_X.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.lbFixSC_X.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.lbFixSC_X.Name = "lbFixSC_X";
            this.lbFixSC_X.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lbFixSC_X.Size = new System.Drawing.Size(49, 22);
            this.lbFixSC_X.Style = Sunny.UI.UIStyle.Orange;
            this.lbFixSC_X.StyleCustomMode = true;
            this.lbFixSC_X.TabIndex = 0;
            this.lbFixSC_X.Text = "Fix SC X";
            this.lbFixSC_X.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbFixSC_X.UseCompatibleTextRendering = true;
            this.lbFixSC_X.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // tbFixSC_X
            // 
            this.tbFixSC_X.BackColor = System.Drawing.Color.Transparent;
            this.tbFixSC_X.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.tbFixSC_X.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.tbFixSC_X.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.tbFixSC_X.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.tbFixSC_X.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.tbFixSC_X.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.tbFixSC_X.ButtonSymbol = 61761;
            this.tbFixSC_X.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbFixSC_X.Enabled = false;
            this.tbFixSC_X.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.tbFixSC_X.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbFixSC_X.HasMaximum = true;
            this.tbFixSC_X.IconSize = 10;
            this.tbFixSC_X.Location = new System.Drawing.Point(3, 38);
            this.tbFixSC_X.Margin = new System.Windows.Forms.Padding(0);
            this.tbFixSC_X.Maximum = 999D;
            this.tbFixSC_X.MaximumEnabled = true;
            this.tbFixSC_X.MaxLength = 3;
            this.tbFixSC_X.Minimum = 0D;
            this.tbFixSC_X.MinimumSize = new System.Drawing.Size(1, 1);
            this.tbFixSC_X.Name = "tbFixSC_X";
            this.tbFixSC_X.Radius = 1;
            this.tbFixSC_X.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.tbFixSC_X.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.tbFixSC_X.ShowText = false;
            this.tbFixSC_X.Size = new System.Drawing.Size(125, 22);
            this.tbFixSC_X.Style = Sunny.UI.UIStyle.Orange;
            this.tbFixSC_X.StyleCustomMode = true;
            this.tbFixSC_X.SymbolSize = 16;
            this.tbFixSC_X.TabIndex = 1;
            this.tbFixSC_X.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.tbFixSC_X.Watermark = "";
            this.tbFixSC_X.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.tbFixSC_X.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // lbFixSC_Y
            // 
            this.lbFixSC_Y.AutoSize = true;
            this.lbFixSC_Y.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbFixSC_Y.Location = new System.Drawing.Point(208, 16);
            this.lbFixSC_Y.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.lbFixSC_Y.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.lbFixSC_Y.Name = "lbFixSC_Y";
            this.lbFixSC_Y.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lbFixSC_Y.Size = new System.Drawing.Size(49, 22);
            this.lbFixSC_Y.Style = Sunny.UI.UIStyle.Orange;
            this.lbFixSC_Y.StyleCustomMode = true;
            this.lbFixSC_Y.TabIndex = 1;
            this.lbFixSC_Y.Text = "Fix SC Y";
            this.lbFixSC_Y.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbFixSC_Y.UseCompatibleTextRendering = true;
            this.lbFixSC_Y.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // tbFixSC_Y
            // 
            this.tbFixSC_Y.BackColor = System.Drawing.Color.Transparent;
            this.tbFixSC_Y.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.tbFixSC_Y.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.tbFixSC_Y.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.tbFixSC_Y.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.tbFixSC_Y.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.tbFixSC_Y.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.tbFixSC_Y.ButtonSymbol = 61761;
            this.tbFixSC_Y.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbFixSC_Y.Enabled = false;
            this.tbFixSC_Y.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.tbFixSC_Y.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbFixSC_Y.HasMaximum = true;
            this.tbFixSC_Y.IconSize = 1;
            this.tbFixSC_Y.Location = new System.Drawing.Point(208, 38);
            this.tbFixSC_Y.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbFixSC_Y.Maximum = 255D;
            this.tbFixSC_Y.MaximumEnabled = true;
            this.tbFixSC_Y.MaxLength = 3;
            this.tbFixSC_Y.Minimum = 0D;
            this.tbFixSC_Y.MinimumSize = new System.Drawing.Size(1, 1);
            this.tbFixSC_Y.Name = "tbFixSC_Y";
            this.tbFixSC_Y.Radius = 1;
            this.tbFixSC_Y.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.tbFixSC_Y.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.tbFixSC_Y.ShowText = false;
            this.tbFixSC_Y.Size = new System.Drawing.Size(125, 22);
            this.tbFixSC_Y.Style = Sunny.UI.UIStyle.Orange;
            this.tbFixSC_Y.StyleCustomMode = true;
            this.tbFixSC_Y.SymbolSize = 16;
            this.tbFixSC_Y.TabIndex = 2;
            this.tbFixSC_Y.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.tbFixSC_Y.Watermark = "";
            this.tbFixSC_Y.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.tbFixSC_Y.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // splitContainer_AllParams
            // 
            this.splitContainer_AllParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer_AllParams.IsSplitterFixed = true;
            this.splitContainer_AllParams.Location = new System.Drawing.Point(0, 51);
            this.splitContainer_AllParams.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer_AllParams.Name = "splitContainer_AllParams";
            // 
            // splitContainer_AllParams.Panel1
            // 
            this.splitContainer_AllParams.Panel1.Controls.Add(this.gbSweepCharge);
            // 
            // splitContainer_AllParams.Panel2
            // 
            this.splitContainer_AllParams.Panel2.Controls.Add(this.gbSweepDump_Freq);
            this.splitContainer_AllParams.Size = new System.Drawing.Size(392, 97);
            this.splitContainer_AllParams.SplitterDistance = 194;
            this.splitContainer_AllParams.TabIndex = 2;
            // 
            // gbSweepCharge
            // 
            this.gbSweepCharge.AutoSize = true;
            this.gbSweepCharge.Controls.Add(this.lbSweepCharge_FixDumpUnit);
            this.gbSweepCharge.Controls.Add(this.lbSweepCharge_PostIdleUnit);
            this.gbSweepCharge.Controls.Add(this.tbSweepCharge_StepValue);
            this.gbSweepCharge.Controls.Add(this.tbSweepCharge_FixDumpValue);
            this.gbSweepCharge.Controls.Add(this.tbSweepCharge_PostIdleValue);
            this.gbSweepCharge.Controls.Add(this.tbSweepCharge_RangeEnd);
            this.gbSweepCharge.Controls.Add(this.tbSweepCharge_RangeStart);
            this.gbSweepCharge.Controls.Add(this.lbSweepCharge_RangeName);
            this.gbSweepCharge.Controls.Add(this.lbSweepCharge_Tidle);
            this.gbSweepCharge.Controls.Add(this.lbSweepCharge_RangeUnit);
            this.gbSweepCharge.Controls.Add(this.lbSweepCharge_StepName);
            this.gbSweepCharge.Controls.Add(this.lbSweepCharge_StepUnit);
            this.gbSweepCharge.Controls.Add(this.lbSweepCharge_PostIdleName);
            this.gbSweepCharge.Controls.Add(this.lbSweepCharge_FixDumpName);
            this.gbSweepCharge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSweepCharge.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.gbSweepCharge.Font = new System.Drawing.Font("新細明體", 9F);
            this.gbSweepCharge.Location = new System.Drawing.Point(0, 0);
            this.gbSweepCharge.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbSweepCharge.MinimumSize = new System.Drawing.Size(1, 1);
            this.gbSweepCharge.Name = "gbSweepCharge";
            this.gbSweepCharge.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.gbSweepCharge.RectColor = System.Drawing.SystemColors.ActiveBorder;
            this.gbSweepCharge.Size = new System.Drawing.Size(194, 97);
            this.gbSweepCharge.Style = Sunny.UI.UIStyle.Custom;
            this.gbSweepCharge.StyleCustomMode = true;
            this.gbSweepCharge.TabIndex = 28;
            this.gbSweepCharge.Text = "Sweep Charge";
            this.gbSweepCharge.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.gbSweepCharge.TitleInterval = 0;
            this.gbSweepCharge.TitleTop = 8;
            this.gbSweepCharge.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // lbSweepCharge_FixDumpUnit
            // 
            this.lbSweepCharge_FixDumpUnit.AutoSize = true;
            this.lbSweepCharge_FixDumpUnit.Location = new System.Drawing.Point(136, 77);
            this.lbSweepCharge_FixDumpUnit.Name = "lbSweepCharge_FixDumpUnit";
            this.lbSweepCharge_FixDumpUnit.Size = new System.Drawing.Size(25, 12);
            this.lbSweepCharge_FixDumpUnit.TabIndex = 27;
            this.lbSweepCharge_FixDumpUnit.Text = "Unit";
            // 
            // lbSweepCharge_PostIdleUnit
            // 
            this.lbSweepCharge_PostIdleUnit.AutoSize = true;
            this.lbSweepCharge_PostIdleUnit.Location = new System.Drawing.Point(105, 59);
            this.lbSweepCharge_PostIdleUnit.Name = "lbSweepCharge_PostIdleUnit";
            this.lbSweepCharge_PostIdleUnit.Size = new System.Drawing.Size(25, 12);
            this.lbSweepCharge_PostIdleUnit.TabIndex = 27;
            this.lbSweepCharge_PostIdleUnit.Text = "Unit";
            // 
            // tbSweepCharge_StepValue
            // 
            this.tbSweepCharge_StepValue.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbSweepCharge_StepValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSweepCharge_StepValue.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbSweepCharge_StepValue.ForeColor = System.Drawing.Color.Black;
            this.tbSweepCharge_StepValue.Location = new System.Drawing.Point(69, 39);
            this.tbSweepCharge_StepValue.MaxLength = 4;
            this.tbSweepCharge_StepValue.Name = "tbSweepCharge_StepValue";
            this.tbSweepCharge_StepValue.Size = new System.Drawing.Size(30, 16);
            this.tbSweepCharge_StepValue.TabIndex = 26;
            this.tbSweepCharge_StepValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSweepCharge_StepValue.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // tbSweepCharge_FixDumpValue
            // 
            this.tbSweepCharge_FixDumpValue.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbSweepCharge_FixDumpValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSweepCharge_FixDumpValue.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbSweepCharge_FixDumpValue.ForeColor = System.Drawing.Color.Black;
            this.tbSweepCharge_FixDumpValue.Location = new System.Drawing.Point(100, 75);
            this.tbSweepCharge_FixDumpValue.MaxLength = 4;
            this.tbSweepCharge_FixDumpValue.Name = "tbSweepCharge_FixDumpValue";
            this.tbSweepCharge_FixDumpValue.Size = new System.Drawing.Size(30, 16);
            this.tbSweepCharge_FixDumpValue.TabIndex = 25;
            this.tbSweepCharge_FixDumpValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSweepCharge_FixDumpValue.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // tbSweepCharge_PostIdleValue
            // 
            this.tbSweepCharge_PostIdleValue.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbSweepCharge_PostIdleValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSweepCharge_PostIdleValue.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbSweepCharge_PostIdleValue.ForeColor = System.Drawing.Color.Black;
            this.tbSweepCharge_PostIdleValue.Location = new System.Drawing.Point(69, 57);
            this.tbSweepCharge_PostIdleValue.MaxLength = 4;
            this.tbSweepCharge_PostIdleValue.Name = "tbSweepCharge_PostIdleValue";
            this.tbSweepCharge_PostIdleValue.Size = new System.Drawing.Size(30, 16);
            this.tbSweepCharge_PostIdleValue.TabIndex = 24;
            this.tbSweepCharge_PostIdleValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSweepCharge_PostIdleValue.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // tbSweepCharge_RangeEnd
            // 
            this.tbSweepCharge_RangeEnd.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbSweepCharge_RangeEnd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSweepCharge_RangeEnd.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbSweepCharge_RangeEnd.ForeColor = System.Drawing.Color.Black;
            this.tbSweepCharge_RangeEnd.Location = new System.Drawing.Point(100, 18);
            this.tbSweepCharge_RangeEnd.MaxLength = 4;
            this.tbSweepCharge_RangeEnd.Name = "tbSweepCharge_RangeEnd";
            this.tbSweepCharge_RangeEnd.Size = new System.Drawing.Size(30, 16);
            this.tbSweepCharge_RangeEnd.TabIndex = 23;
            this.tbSweepCharge_RangeEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSweepCharge_RangeEnd.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // tbSweepCharge_RangeStart
            // 
            this.tbSweepCharge_RangeStart.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbSweepCharge_RangeStart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSweepCharge_RangeStart.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbSweepCharge_RangeStart.ForeColor = System.Drawing.Color.Black;
            this.tbSweepCharge_RangeStart.Location = new System.Drawing.Point(47, 18);
            this.tbSweepCharge_RangeStart.MaxLength = 4;
            this.tbSweepCharge_RangeStart.Name = "tbSweepCharge_RangeStart";
            this.tbSweepCharge_RangeStart.Size = new System.Drawing.Size(30, 16);
            this.tbSweepCharge_RangeStart.TabIndex = 22;
            this.tbSweepCharge_RangeStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSweepCharge_RangeStart.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // lbSweepCharge_RangeName
            // 
            this.lbSweepCharge_RangeName.AutoSize = true;
            this.lbSweepCharge_RangeName.Location = new System.Drawing.Point(6, 18);
            this.lbSweepCharge_RangeName.Name = "lbSweepCharge_RangeName";
            this.lbSweepCharge_RangeName.Size = new System.Drawing.Size(35, 12);
            this.lbSweepCharge_RangeName.TabIndex = 0;
            this.lbSweepCharge_RangeName.Text = "Range";
            this.lbSweepCharge_RangeName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSweepCharge_Tidle
            // 
            this.lbSweepCharge_Tidle.AutoSize = true;
            this.lbSweepCharge_Tidle.Location = new System.Drawing.Point(83, 18);
            this.lbSweepCharge_Tidle.Name = "lbSweepCharge_Tidle";
            this.lbSweepCharge_Tidle.Size = new System.Drawing.Size(11, 12);
            this.lbSweepCharge_Tidle.TabIndex = 5;
            this.lbSweepCharge_Tidle.Text = "~";
            this.lbSweepCharge_Tidle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSweepCharge_RangeUnit
            // 
            this.lbSweepCharge_RangeUnit.AutoSize = true;
            this.lbSweepCharge_RangeUnit.Location = new System.Drawing.Point(136, 18);
            this.lbSweepCharge_RangeUnit.Name = "lbSweepCharge_RangeUnit";
            this.lbSweepCharge_RangeUnit.Size = new System.Drawing.Size(25, 12);
            this.lbSweepCharge_RangeUnit.TabIndex = 9;
            this.lbSweepCharge_RangeUnit.Text = "Unit";
            // 
            // lbSweepCharge_StepName
            // 
            this.lbSweepCharge_StepName.AutoSize = true;
            this.lbSweepCharge_StepName.Location = new System.Drawing.Point(6, 39);
            this.lbSweepCharge_StepName.Name = "lbSweepCharge_StepName";
            this.lbSweepCharge_StepName.Size = new System.Drawing.Size(25, 12);
            this.lbSweepCharge_StepName.TabIndex = 1;
            this.lbSweepCharge_StepName.Text = "Step";
            this.lbSweepCharge_StepName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSweepCharge_StepUnit
            // 
            this.lbSweepCharge_StepUnit.AutoSize = true;
            this.lbSweepCharge_StepUnit.Location = new System.Drawing.Point(105, 39);
            this.lbSweepCharge_StepUnit.Name = "lbSweepCharge_StepUnit";
            this.lbSweepCharge_StepUnit.Size = new System.Drawing.Size(25, 12);
            this.lbSweepCharge_StepUnit.TabIndex = 10;
            this.lbSweepCharge_StepUnit.Text = "Unit";
            // 
            // lbSweepCharge_PostIdleName
            // 
            this.lbSweepCharge_PostIdleName.AutoSize = true;
            this.lbSweepCharge_PostIdleName.Location = new System.Drawing.Point(6, 57);
            this.lbSweepCharge_PostIdleName.Name = "lbSweepCharge_PostIdleName";
            this.lbSweepCharge_PostIdleName.Size = new System.Drawing.Size(46, 12);
            this.lbSweepCharge_PostIdleName.TabIndex = 14;
            this.lbSweepCharge_PostIdleName.Text = "Post-Idle";
            // 
            // lbSweepCharge_FixDumpName
            // 
            this.lbSweepCharge_FixDumpName.AutoSize = true;
            this.lbSweepCharge_FixDumpName.Location = new System.Drawing.Point(6, 75);
            this.lbSweepCharge_FixDumpName.Name = "lbSweepCharge_FixDumpName";
            this.lbSweepCharge_FixDumpName.Size = new System.Drawing.Size(52, 12);
            this.lbSweepCharge_FixDumpName.TabIndex = 15;
            this.lbSweepCharge_FixDumpName.Text = "Fix Dump";
            // 
            // gbSweepDump_Freq
            // 
            this.gbSweepDump_Freq.AutoSize = true;
            this.gbSweepDump_Freq.Controls.Add(this.lbSweepDump_FixChargeUnit);
            this.gbSweepDump_Freq.Controls.Add(this.lbSweepDump_RangeName);
            this.gbSweepDump_Freq.Controls.Add(this.lbSweepDump_PostIdleUnit);
            this.gbSweepDump_Freq.Controls.Add(this.lbSweepDump_FixChargeName);
            this.gbSweepDump_Freq.Controls.Add(this.tbSweepDump_StepValue);
            this.gbSweepDump_Freq.Controls.Add(this.lbSweepDump_PostIdleName);
            this.gbSweepDump_Freq.Controls.Add(this.tbSweepDump_FixChargeValue);
            this.gbSweepDump_Freq.Controls.Add(this.lbSweepDump_StepUnit);
            this.gbSweepDump_Freq.Controls.Add(this.tbSweepDump_PostIdleValue);
            this.gbSweepDump_Freq.Controls.Add(this.lbSweepDump_StepName);
            this.gbSweepDump_Freq.Controls.Add(this.tbSweepDump_RangeEnd);
            this.gbSweepDump_Freq.Controls.Add(this.lbSweepDump_RangeUnit);
            this.gbSweepDump_Freq.Controls.Add(this.tbSweepDump_RangeStart);
            this.gbSweepDump_Freq.Controls.Add(this.lbSweepDump_Tidle);
            this.gbSweepDump_Freq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSweepDump_Freq.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.gbSweepDump_Freq.Font = new System.Drawing.Font("新細明體", 9F);
            this.gbSweepDump_Freq.Location = new System.Drawing.Point(0, 0);
            this.gbSweepDump_Freq.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbSweepDump_Freq.MinimumSize = new System.Drawing.Size(1, 1);
            this.gbSweepDump_Freq.Name = "gbSweepDump_Freq";
            this.gbSweepDump_Freq.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.gbSweepDump_Freq.RectColor = System.Drawing.SystemColors.ActiveBorder;
            this.gbSweepDump_Freq.Size = new System.Drawing.Size(194, 97);
            this.gbSweepDump_Freq.Style = Sunny.UI.UIStyle.Custom;
            this.gbSweepDump_Freq.StyleCustomMode = true;
            this.gbSweepDump_Freq.TabIndex = 28;
            this.gbSweepDump_Freq.Text = "Sweep Dump";
            this.gbSweepDump_Freq.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.gbSweepDump_Freq.TitleInterval = 0;
            this.gbSweepDump_Freq.TitleTop = 8;
            this.gbSweepDump_Freq.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // lbSweepDump_FixChargeUnit
            // 
            this.lbSweepDump_FixChargeUnit.AutoSize = true;
            this.lbSweepDump_FixChargeUnit.Location = new System.Drawing.Point(136, 77);
            this.lbSweepDump_FixChargeUnit.Name = "lbSweepDump_FixChargeUnit";
            this.lbSweepDump_FixChargeUnit.Size = new System.Drawing.Size(25, 12);
            this.lbSweepDump_FixChargeUnit.TabIndex = 26;
            this.lbSweepDump_FixChargeUnit.Text = "Unit";
            // 
            // lbSweepDump_RangeName
            // 
            this.lbSweepDump_RangeName.AutoSize = true;
            this.lbSweepDump_RangeName.Location = new System.Drawing.Point(6, 18);
            this.lbSweepDump_RangeName.Name = "lbSweepDump_RangeName";
            this.lbSweepDump_RangeName.Size = new System.Drawing.Size(35, 12);
            this.lbSweepDump_RangeName.TabIndex = 0;
            this.lbSweepDump_RangeName.Text = "Range";
            this.lbSweepDump_RangeName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSweepDump_PostIdleUnit
            // 
            this.lbSweepDump_PostIdleUnit.AutoSize = true;
            this.lbSweepDump_PostIdleUnit.Location = new System.Drawing.Point(105, 59);
            this.lbSweepDump_PostIdleUnit.Name = "lbSweepDump_PostIdleUnit";
            this.lbSweepDump_PostIdleUnit.Size = new System.Drawing.Size(25, 12);
            this.lbSweepDump_PostIdleUnit.TabIndex = 25;
            this.lbSweepDump_PostIdleUnit.Text = "Unit";
            // 
            // lbSweepDump_FixChargeName
            // 
            this.lbSweepDump_FixChargeName.AutoSize = true;
            this.lbSweepDump_FixChargeName.Location = new System.Drawing.Point(6, 75);
            this.lbSweepDump_FixChargeName.Name = "lbSweepDump_FixChargeName";
            this.lbSweepDump_FixChargeName.Size = new System.Drawing.Size(57, 12);
            this.lbSweepDump_FixChargeName.TabIndex = 15;
            this.lbSweepDump_FixChargeName.Text = "Fix Charge";
            // 
            // tbSweepDump_StepValue
            // 
            this.tbSweepDump_StepValue.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbSweepDump_StepValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSweepDump_StepValue.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbSweepDump_StepValue.ForeColor = System.Drawing.Color.Black;
            this.tbSweepDump_StepValue.Location = new System.Drawing.Point(69, 39);
            this.tbSweepDump_StepValue.MaxLength = 4;
            this.tbSweepDump_StepValue.Name = "tbSweepDump_StepValue";
            this.tbSweepDump_StepValue.Size = new System.Drawing.Size(30, 16);
            this.tbSweepDump_StepValue.TabIndex = 24;
            this.tbSweepDump_StepValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSweepDump_StepValue.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // lbSweepDump_PostIdleName
            // 
            this.lbSweepDump_PostIdleName.AutoSize = true;
            this.lbSweepDump_PostIdleName.Location = new System.Drawing.Point(6, 57);
            this.lbSweepDump_PostIdleName.Name = "lbSweepDump_PostIdleName";
            this.lbSweepDump_PostIdleName.Size = new System.Drawing.Size(46, 12);
            this.lbSweepDump_PostIdleName.TabIndex = 14;
            this.lbSweepDump_PostIdleName.Text = "Post-Idle";
            // 
            // tbSweepDump_FixChargeValue
            // 
            this.tbSweepDump_FixChargeValue.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbSweepDump_FixChargeValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSweepDump_FixChargeValue.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbSweepDump_FixChargeValue.ForeColor = System.Drawing.Color.Black;
            this.tbSweepDump_FixChargeValue.Location = new System.Drawing.Point(100, 75);
            this.tbSweepDump_FixChargeValue.MaxLength = 4;
            this.tbSweepDump_FixChargeValue.Name = "tbSweepDump_FixChargeValue";
            this.tbSweepDump_FixChargeValue.Size = new System.Drawing.Size(30, 16);
            this.tbSweepDump_FixChargeValue.TabIndex = 23;
            this.tbSweepDump_FixChargeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSweepDump_FixChargeValue.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // lbSweepDump_StepUnit
            // 
            this.lbSweepDump_StepUnit.AutoSize = true;
            this.lbSweepDump_StepUnit.Location = new System.Drawing.Point(105, 39);
            this.lbSweepDump_StepUnit.Name = "lbSweepDump_StepUnit";
            this.lbSweepDump_StepUnit.Size = new System.Drawing.Size(25, 12);
            this.lbSweepDump_StepUnit.TabIndex = 10;
            this.lbSweepDump_StepUnit.Text = "Unit";
            // 
            // tbSweepDump_PostIdleValue
            // 
            this.tbSweepDump_PostIdleValue.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbSweepDump_PostIdleValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSweepDump_PostIdleValue.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbSweepDump_PostIdleValue.ForeColor = System.Drawing.Color.Black;
            this.tbSweepDump_PostIdleValue.Location = new System.Drawing.Point(69, 57);
            this.tbSweepDump_PostIdleValue.MaxLength = 4;
            this.tbSweepDump_PostIdleValue.Name = "tbSweepDump_PostIdleValue";
            this.tbSweepDump_PostIdleValue.Size = new System.Drawing.Size(30, 16);
            this.tbSweepDump_PostIdleValue.TabIndex = 22;
            this.tbSweepDump_PostIdleValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSweepDump_PostIdleValue.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // lbSweepDump_StepName
            // 
            this.lbSweepDump_StepName.AutoSize = true;
            this.lbSweepDump_StepName.Location = new System.Drawing.Point(6, 39);
            this.lbSweepDump_StepName.Name = "lbSweepDump_StepName";
            this.lbSweepDump_StepName.Size = new System.Drawing.Size(25, 12);
            this.lbSweepDump_StepName.TabIndex = 1;
            this.lbSweepDump_StepName.Text = "Step";
            this.lbSweepDump_StepName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbSweepDump_RangeEnd
            // 
            this.tbSweepDump_RangeEnd.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbSweepDump_RangeEnd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSweepDump_RangeEnd.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbSweepDump_RangeEnd.ForeColor = System.Drawing.Color.Black;
            this.tbSweepDump_RangeEnd.Location = new System.Drawing.Point(100, 18);
            this.tbSweepDump_RangeEnd.MaxLength = 4;
            this.tbSweepDump_RangeEnd.Name = "tbSweepDump_RangeEnd";
            this.tbSweepDump_RangeEnd.Size = new System.Drawing.Size(30, 16);
            this.tbSweepDump_RangeEnd.TabIndex = 21;
            this.tbSweepDump_RangeEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSweepDump_RangeEnd.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // lbSweepDump_RangeUnit
            // 
            this.lbSweepDump_RangeUnit.AutoSize = true;
            this.lbSweepDump_RangeUnit.Location = new System.Drawing.Point(136, 18);
            this.lbSweepDump_RangeUnit.Name = "lbSweepDump_RangeUnit";
            this.lbSweepDump_RangeUnit.Size = new System.Drawing.Size(25, 12);
            this.lbSweepDump_RangeUnit.TabIndex = 9;
            this.lbSweepDump_RangeUnit.Text = "Unit";
            // 
            // tbSweepDump_RangeStart
            // 
            this.tbSweepDump_RangeStart.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbSweepDump_RangeStart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSweepDump_RangeStart.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbSweepDump_RangeStart.ForeColor = System.Drawing.Color.Black;
            this.tbSweepDump_RangeStart.Location = new System.Drawing.Point(47, 18);
            this.tbSweepDump_RangeStart.MaxLength = 4;
            this.tbSweepDump_RangeStart.Name = "tbSweepDump_RangeStart";
            this.tbSweepDump_RangeStart.Size = new System.Drawing.Size(30, 16);
            this.tbSweepDump_RangeStart.TabIndex = 20;
            this.tbSweepDump_RangeStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSweepDump_RangeStart.Validated += new System.EventHandler(this.TextBox_Validated);
            // 
            // lbSweepDump_Tidle
            // 
            this.lbSweepDump_Tidle.AutoSize = true;
            this.lbSweepDump_Tidle.Location = new System.Drawing.Point(83, 18);
            this.lbSweepDump_Tidle.Name = "lbSweepDump_Tidle";
            this.lbSweepDump_Tidle.Size = new System.Drawing.Size(11, 12);
            this.lbSweepDump_Tidle.TabIndex = 5;
            this.lbSweepDump_Tidle.Text = "~";
            this.lbSweepDump_Tidle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox_ScanMode
            // 
            this.groupBox_ScanMode.Controls.Add(this.radioListBox_ChartDisp);
            this.groupBox_ScanMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_ScanMode.Enabled = false;
            this.groupBox_ScanMode.Location = new System.Drawing.Point(0, 0);
            this.groupBox_ScanMode.Name = "groupBox_ScanMode";
            this.groupBox_ScanMode.Size = new System.Drawing.Size(392, 51);
            this.groupBox_ScanMode.TabIndex = 11;
            this.groupBox_ScanMode.TabStop = false;
            this.groupBox_ScanMode.Text = "Scan Mode";
            // 
            // radioListBox_ChartDisp
            // 
            this.radioListBox_ChartDisp.BackColor = System.Drawing.Color.Transparent;
            this.radioListBox_ChartDisp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.radioListBox_ChartDisp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioListBox_ChartDisp.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.radioListBox_ChartDisp.FormattingEnabled = true;
            this.radioListBox_ChartDisp.ItemHeight = 22;
            this.radioListBox_ChartDisp.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.radioListBox_ChartDisp.Location = new System.Drawing.Point(3, 18);
            this.radioListBox_ChartDisp.MultiColumn = true;
            this.radioListBox_ChartDisp.Name = "radioListBox_ChartDisp";
            this.radioListBox_ChartDisp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radioListBox_ChartDisp.Size = new System.Drawing.Size(386, 30);
            this.radioListBox_ChartDisp.TabIndex = 1;
            this.radioListBox_ChartDisp.SelectedIndexChanged += new System.EventHandler(this.radioListBox_ChartDisp_SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tbCtrl_Chart);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.richTextBox_msg);
            this.splitContainer2.Size = new System.Drawing.Size(754, 586);
            this.splitContainer2.SplitterDistance = 452;
            this.splitContainer2.TabIndex = 1;
            // 
            // tbCtrl_Chart
            // 
            this.tbCtrl_Chart.Controls.Add(this.tbOne_RawCode);
            this.tbCtrl_Chart.Controls.Add(this.tbTwo_RawCode);
            this.tbCtrl_Chart.Controls.Add(this.tbTwo_DeltaC_Code);
            this.tbCtrl_Chart.Controls.Add(this.tbTwo_DeltaC_Percent);
            this.tbCtrl_Chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCtrl_Chart.Location = new System.Drawing.Point(0, 0);
            this.tbCtrl_Chart.Name = "tbCtrl_Chart";
            this.tbCtrl_Chart.SelectedIndex = 0;
            this.tbCtrl_Chart.Size = new System.Drawing.Size(754, 452);
            this.tbCtrl_Chart.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tbCtrl_Chart.TabIndex = 1;
            this.tbCtrl_Chart.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tbCtrl_Chart_DrawItem);
            // 
            // tbOne_RawCode
            // 
            this.tbOne_RawCode.Controls.Add(this.tbCtl_One_RawCode);
            this.tbOne_RawCode.Location = new System.Drawing.Point(4, 22);
            this.tbOne_RawCode.Name = "tbOne_RawCode";
            this.tbOne_RawCode.Size = new System.Drawing.Size(746, 426);
            this.tbOne_RawCode.TabIndex = 0;
            this.tbOne_RawCode.Text = "One Shot - RawData Code";
            this.tbOne_RawCode.UseVisualStyleBackColor = true;
            // 
            // tbCtl_One_RawCode
            // 
            this.tbCtl_One_RawCode.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tbCtl_One_RawCode.Controls.Add(this.tabPage1);
            this.tbCtl_One_RawCode.Controls.Add(this.tabPage2);
            this.tbCtl_One_RawCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCtl_One_RawCode.Location = new System.Drawing.Point(0, 0);
            this.tbCtl_One_RawCode.Multiline = true;
            this.tbCtl_One_RawCode.Name = "tbCtl_One_RawCode";
            this.tbCtl_One_RawCode.SelectedIndex = 0;
            this.tbCtl_One_RawCode.Size = new System.Drawing.Size(746, 426);
            this.tbCtl_One_RawCode.TabIndex = 1;
            this.tbCtl_One_RawCode.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tbCtrl_Chart_DrawItem);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.tabPage1.Controls.Add(this.chart_OneShot_FixDump);
            this.tabPage1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(738, 400);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "(Sweep Charge)";
            // 
            // chart_OneShot_FixDump
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Bottom.AxisPen.Width = 1;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Axes.Bottom.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Axes.Bottom.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Axes.Bottom.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Axes.Bottom.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Axes.Depth.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Axes.Depth.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Axes.Depth.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Axes.Depth.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Axes.DepthTop.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Axes.DepthTop.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Axes.DepthTop.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Axes.DepthTop.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Left.AxisPen.Width = 1;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Axes.Left.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Axes.Left.Labels.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixDump.Axes.Left.MaximumOffset = 50;
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Left.Ticks.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Axes.Left.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Axes.Left.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Axes.Right.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Axes.Right.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Axes.Right.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Axes.Right.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Axes.Top.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Axes.Top.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Axes.Top.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Axes.Top.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixDump.BackColor = System.Drawing.Color.Transparent;
            this.chart_OneShot_FixDump.Cursor = System.Windows.Forms.Cursors.Default;
            this.chart_OneShot_FixDump.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Footer.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Footer.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Header.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Header.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixDump.Header.Lines = new string[] {
        "Max Data"};
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Legend.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Legend.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixDump.Legend.CheckBoxes = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Legend.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.chart_OneShot_FixDump.Legend.FontSeriesColor = true;
            this.chart_OneShot_FixDump.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Legend.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Legend.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixDump.Legend.Transparent = true;
            this.chart_OneShot_FixDump.Location = new System.Drawing.Point(3, 3);
            this.chart_OneShot_FixDump.Margin = new System.Windows.Forms.Padding(0);
            this.chart_OneShot_FixDump.Name = "chart_OneShot_FixDump";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Panel.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Panel.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Panel.Brush.Gradient.Visible = false;
            this.chart_OneShot_FixDump.Panel.MarginBottom = 3D;
            this.chart_OneShot_FixDump.Panel.MarginTop = 3D;
            this.chart_OneShot_FixDump.Panel.Transparent = true;
            this.chart_OneShot_FixDump.Series.Add(this.fastLine1);
            this.chart_OneShot_FixDump.Size = new System.Drawing.Size(732, 394);
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.SubFooter.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.SubFooter.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.SubHeader.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.SubHeader.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixDump.TabIndex = 2;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Walls.Back.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Walls.Back.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Walls.Back.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Walls.Back.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Walls.Bottom.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Walls.Bottom.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Walls.Left.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Walls.Left.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixDump.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixDump.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixDump.Walls.Right.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixDump.Walls.Right.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixDump.Walls.Visible = false;
            // 
            // fastLine1
            // 
            this.fastLine1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.fastLine1.ColorEach = false;
            // 
            // 
            // 
            this.fastLine1.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.fastLine1.Title = "fastLine1";
            this.fastLine1.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.Ignore;
            // 
            // 
            // 
            this.fastLine1.XValues.DataMember = "X";
            this.fastLine1.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.fastLine1.YValues.DataMember = "Y";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.tabPage2.Controls.Add(this.chart_OneShot_FixCharge);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(738, 400);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "(Sweep Dump)";
            // 
            // chart_OneShot_FixCharge
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Axes.Bottom.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Axes.Bottom.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Axes.Bottom.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Axes.Bottom.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Axes.Depth.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Axes.Depth.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Axes.Depth.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Axes.Depth.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Axes.DepthTop.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Axes.DepthTop.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Axes.DepthTop.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Axes.DepthTop.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Axes.Left.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Axes.Left.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Axes.Left.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Axes.Left.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Axes.Right.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Axes.Right.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Axes.Right.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Axes.Right.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Axes.Top.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Axes.Top.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Axes.Top.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Axes.Top.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixCharge.Cursor = System.Windows.Forms.Cursors.Default;
            this.chart_OneShot_FixCharge.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Footer.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Footer.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Header.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Header.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixCharge.Header.Lines = new string[] {
        "Max Data"};
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Legend.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Legend.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixCharge.Legend.CheckBoxes = true;
            this.chart_OneShot_FixCharge.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Legend.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Legend.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixCharge.Location = new System.Drawing.Point(3, 3);
            this.chart_OneShot_FixCharge.Margin = new System.Windows.Forms.Padding(0);
            this.chart_OneShot_FixCharge.Name = "chart_OneShot_FixCharge";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Panel.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Panel.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Panel.Brush.Gradient.Visible = false;
            this.chart_OneShot_FixCharge.Panel.MarginBottom = 3D;
            this.chart_OneShot_FixCharge.Panel.MarginTop = 3D;
            this.chart_OneShot_FixCharge.Panel.Transparent = true;
            this.chart_OneShot_FixCharge.Size = new System.Drawing.Size(732, 394);
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.SubFooter.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.SubFooter.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.SubHeader.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.SubHeader.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixCharge.TabIndex = 3;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Walls.Back.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Walls.Back.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Walls.Back.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Walls.Back.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Walls.Bottom.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Walls.Bottom.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Walls.Left.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Walls.Left.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_OneShot_FixCharge.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_OneShot_FixCharge.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_OneShot_FixCharge.Walls.Right.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_OneShot_FixCharge.Walls.Right.Bevel.StringColorTwo = "FF808080";
            this.chart_OneShot_FixCharge.Walls.Visible = false;
            // 
            // tbTwo_RawCode
            // 
            this.tbTwo_RawCode.Controls.Add(this.tbCtl_Two_RawCode);
            this.tbTwo_RawCode.Location = new System.Drawing.Point(4, 22);
            this.tbTwo_RawCode.Name = "tbTwo_RawCode";
            this.tbTwo_RawCode.Size = new System.Drawing.Size(746, 426);
            this.tbTwo_RawCode.TabIndex = 3;
            this.tbTwo_RawCode.Text = "Two Shot - RawData Code";
            this.tbTwo_RawCode.UseVisualStyleBackColor = true;
            // 
            // tbCtl_Two_RawCode
            // 
            this.tbCtl_Two_RawCode.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tbCtl_Two_RawCode.Controls.Add(this.tabPage3);
            this.tbCtl_Two_RawCode.Controls.Add(this.tabPage4);
            this.tbCtl_Two_RawCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCtl_Two_RawCode.Location = new System.Drawing.Point(0, 0);
            this.tbCtl_Two_RawCode.Multiline = true;
            this.tbCtl_Two_RawCode.Name = "tbCtl_Two_RawCode";
            this.tbCtl_Two_RawCode.SelectedIndex = 0;
            this.tbCtl_Two_RawCode.Size = new System.Drawing.Size(746, 426);
            this.tbCtl_Two_RawCode.TabIndex = 1;
            this.tbCtl_Two_RawCode.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tbCtrl_Chart_DrawItem);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.tabPage3.Controls.Add(this.chart_TwoShot_Raw_FixDump);
            this.tabPage3.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(738, 400);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "(Sweep Charge)";
            // 
            // chart_TwoShot_Raw_FixDump
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Bottom.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Axes.Bottom.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Bottom.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Axes.Bottom.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Depth.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Axes.Depth.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Depth.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Axes.Depth.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Axes.DepthTop.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Axes.DepthTop.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Axes.DepthTop.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Axes.DepthTop.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Left.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Axes.Left.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Left.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Axes.Left.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Right.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Axes.Right.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Right.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Axes.Right.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Top.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Axes.Top.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Axes.Top.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Axes.Top.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_Raw_FixDump.Cursor = System.Windows.Forms.Cursors.Default;
            this.chart_TwoShot_Raw_FixDump.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Footer.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Footer.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Header.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Header.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_Raw_FixDump.Header.Lines = new string[] {
        "Max Data"};
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Legend.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Legend.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_Raw_FixDump.Legend.CheckBoxes = true;
            this.chart_TwoShot_Raw_FixDump.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Legend.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Legend.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_Raw_FixDump.Location = new System.Drawing.Point(3, 3);
            this.chart_TwoShot_Raw_FixDump.Margin = new System.Windows.Forms.Padding(0);
            this.chart_TwoShot_Raw_FixDump.Name = "chart_TwoShot_Raw_FixDump";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Panel.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Panel.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Panel.Brush.Gradient.Visible = false;
            this.chart_TwoShot_Raw_FixDump.Panel.MarginBottom = 3D;
            this.chart_TwoShot_Raw_FixDump.Panel.MarginTop = 3D;
            this.chart_TwoShot_Raw_FixDump.Panel.Transparent = true;
            this.chart_TwoShot_Raw_FixDump.Size = new System.Drawing.Size(732, 394);
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.SubFooter.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.SubFooter.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.SubHeader.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.SubHeader.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_Raw_FixDump.TabIndex = 4;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Walls.Back.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Walls.Back.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Walls.Back.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Walls.Back.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Walls.Bottom.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Walls.Bottom.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Walls.Left.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Walls.Left.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixDump.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixDump.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixDump.Walls.Right.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixDump.Walls.Right.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_Raw_FixDump.Walls.Visible = false;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.tabPage4.Controls.Add(this.chart_TwoShot_Raw_FixCharge);
            this.tabPage4.Location = new System.Drawing.Point(4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(738, 400);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "(Sweep Dump)";
            // 
            // chart_TwoShot_Raw_FixCharge
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Bottom.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Axes.Bottom.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Bottom.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Axes.Bottom.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Depth.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Axes.Depth.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Depth.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Axes.Depth.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.DepthTop.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Axes.DepthTop.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.DepthTop.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Axes.DepthTop.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Left.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Axes.Left.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Left.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Axes.Left.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Right.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Axes.Right.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Right.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Axes.Right.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Top.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Axes.Top.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Axes.Top.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Axes.Top.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_Raw_FixCharge.Cursor = System.Windows.Forms.Cursors.Default;
            this.chart_TwoShot_Raw_FixCharge.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Footer.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Footer.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Header.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Header.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_Raw_FixCharge.Header.Lines = new string[] {
        "Max Data"};
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Legend.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Legend.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_Raw_FixCharge.Legend.CheckBoxes = true;
            this.chart_TwoShot_Raw_FixCharge.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Legend.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Legend.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_Raw_FixCharge.Location = new System.Drawing.Point(3, 3);
            this.chart_TwoShot_Raw_FixCharge.Margin = new System.Windows.Forms.Padding(0);
            this.chart_TwoShot_Raw_FixCharge.Name = "chart_TwoShot_Raw_FixCharge";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Panel.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Panel.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Panel.Brush.Gradient.Visible = false;
            this.chart_TwoShot_Raw_FixCharge.Panel.MarginBottom = 3D;
            this.chart_TwoShot_Raw_FixCharge.Panel.MarginTop = 3D;
            this.chart_TwoShot_Raw_FixCharge.Panel.Transparent = true;
            this.chart_TwoShot_Raw_FixCharge.Size = new System.Drawing.Size(732, 394);
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.SubFooter.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.SubFooter.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.SubHeader.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.SubHeader.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_Raw_FixCharge.TabIndex = 5;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Walls.Back.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Walls.Back.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Walls.Back.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Walls.Back.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Walls.Bottom.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Walls.Bottom.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Walls.Left.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Walls.Left.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_Raw_FixCharge.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_Raw_FixCharge.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_Raw_FixCharge.Walls.Right.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_Raw_FixCharge.Walls.Right.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_Raw_FixCharge.Walls.Visible = false;
            // 
            // tbTwo_DeltaC_Code
            // 
            this.tbTwo_DeltaC_Code.Controls.Add(this.tbCtl_Two_DeltaC_Code);
            this.tbTwo_DeltaC_Code.Location = new System.Drawing.Point(4, 22);
            this.tbTwo_DeltaC_Code.Name = "tbTwo_DeltaC_Code";
            this.tbTwo_DeltaC_Code.Size = new System.Drawing.Size(746, 426);
            this.tbTwo_DeltaC_Code.TabIndex = 1;
            this.tbTwo_DeltaC_Code.Text = "Two Shot - Delta Cs";
            this.tbTwo_DeltaC_Code.UseVisualStyleBackColor = true;
            // 
            // tbCtl_Two_DeltaC_Code
            // 
            this.tbCtl_Two_DeltaC_Code.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tbCtl_Two_DeltaC_Code.Controls.Add(this.tabPage5);
            this.tbCtl_Two_DeltaC_Code.Controls.Add(this.tabPage6);
            this.tbCtl_Two_DeltaC_Code.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCtl_Two_DeltaC_Code.Location = new System.Drawing.Point(0, 0);
            this.tbCtl_Two_DeltaC_Code.Multiline = true;
            this.tbCtl_Two_DeltaC_Code.Name = "tbCtl_Two_DeltaC_Code";
            this.tbCtl_Two_DeltaC_Code.SelectedIndex = 0;
            this.tbCtl_Two_DeltaC_Code.Size = new System.Drawing.Size(746, 426);
            this.tbCtl_Two_DeltaC_Code.TabIndex = 2;
            this.tbCtl_Two_DeltaC_Code.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tbCtrl_Chart_DrawItem);
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.tabPage5.Controls.Add(this.chart_TwoShot_DeltaCs_FixDump);
            this.tabPage5.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabPage5.Location = new System.Drawing.Point(4, 4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(738, 400);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "(Sweep Charge)";
            // 
            // chart_TwoShot_DeltaCs_FixDump
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Bottom.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Bottom.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Bottom.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Bottom.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Depth.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Depth.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Depth.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Depth.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.DepthTop.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Axes.DepthTop.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.DepthTop.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Axes.DepthTop.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Left.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Left.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Left.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Left.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Right.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Right.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Right.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Right.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Top.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Top.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Top.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Axes.Top.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCs_FixDump.Cursor = System.Windows.Forms.Cursors.Default;
            this.chart_TwoShot_DeltaCs_FixDump.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Footer.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Footer.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Header.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Header.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCs_FixDump.Header.Lines = new string[] {
        "Max Data"};
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Legend.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Legend.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCs_FixDump.Legend.CheckBoxes = true;
            this.chart_TwoShot_DeltaCs_FixDump.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Legend.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Legend.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCs_FixDump.Location = new System.Drawing.Point(3, 3);
            this.chart_TwoShot_DeltaCs_FixDump.Margin = new System.Windows.Forms.Padding(0);
            this.chart_TwoShot_DeltaCs_FixDump.Name = "chart_TwoShot_DeltaCs_FixDump";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Panel.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Panel.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Panel.Brush.Gradient.Visible = false;
            this.chart_TwoShot_DeltaCs_FixDump.Panel.MarginBottom = 3D;
            this.chart_TwoShot_DeltaCs_FixDump.Panel.MarginTop = 3D;
            this.chart_TwoShot_DeltaCs_FixDump.Panel.Transparent = true;
            this.chart_TwoShot_DeltaCs_FixDump.Size = new System.Drawing.Size(732, 394);
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.SubFooter.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.SubFooter.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.SubHeader.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.SubHeader.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCs_FixDump.TabIndex = 5;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Back.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Back.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Back.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Back.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Bottom.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Bottom.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Left.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Left.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Right.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Right.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCs_FixDump.Walls.Visible = false;
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.tabPage6.Controls.Add(this.chart_TwoShot_DeltaCs_FixCharge);
            this.tabPage6.Location = new System.Drawing.Point(4, 4);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(738, 400);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "(Sweep Dump)";
            // 
            // chart_TwoShot_DeltaCs_FixCharge
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Bottom.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Bottom.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Bottom.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Bottom.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Depth.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Depth.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Depth.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Depth.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.DepthTop.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.DepthTop.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.DepthTop.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.DepthTop.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Left.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Left.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Left.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Left.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Right.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Right.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Right.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Right.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Top.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Top.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Top.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Axes.Top.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCs_FixCharge.Cursor = System.Windows.Forms.Cursors.Default;
            this.chart_TwoShot_DeltaCs_FixCharge.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Footer.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Footer.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Header.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Header.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCs_FixCharge.Header.Lines = new string[] {
        "Max Data"};
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Legend.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Legend.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCs_FixCharge.Legend.CheckBoxes = true;
            this.chart_TwoShot_DeltaCs_FixCharge.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Legend.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Legend.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCs_FixCharge.Location = new System.Drawing.Point(3, 3);
            this.chart_TwoShot_DeltaCs_FixCharge.Margin = new System.Windows.Forms.Padding(0);
            this.chart_TwoShot_DeltaCs_FixCharge.Name = "chart_TwoShot_DeltaCs_FixCharge";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Panel.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Panel.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Panel.Brush.Gradient.Visible = false;
            this.chart_TwoShot_DeltaCs_FixCharge.Panel.MarginBottom = 3D;
            this.chart_TwoShot_DeltaCs_FixCharge.Panel.MarginTop = 3D;
            this.chart_TwoShot_DeltaCs_FixCharge.Panel.Transparent = true;
            this.chart_TwoShot_DeltaCs_FixCharge.Size = new System.Drawing.Size(732, 394);
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.SubFooter.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.SubFooter.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.SubHeader.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.SubHeader.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCs_FixCharge.TabIndex = 5;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Back.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Back.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Back.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Back.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Bottom.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Bottom.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Left.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Left.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Right.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Right.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCs_FixCharge.Walls.Visible = false;
            // 
            // tbTwo_DeltaC_Percent
            // 
            this.tbTwo_DeltaC_Percent.Controls.Add(this.tbCtl_Two_DeltaC_Percent);
            this.tbTwo_DeltaC_Percent.Location = new System.Drawing.Point(4, 22);
            this.tbTwo_DeltaC_Percent.Name = "tbTwo_DeltaC_Percent";
            this.tbTwo_DeltaC_Percent.Size = new System.Drawing.Size(746, 426);
            this.tbTwo_DeltaC_Percent.TabIndex = 2;
            this.tbTwo_DeltaC_Percent.Text = "Two Shot - Delta Cs (%)";
            this.tbTwo_DeltaC_Percent.UseVisualStyleBackColor = true;
            // 
            // tbCtl_Two_DeltaC_Percent
            // 
            this.tbCtl_Two_DeltaC_Percent.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tbCtl_Two_DeltaC_Percent.Controls.Add(this.tabPage7);
            this.tbCtl_Two_DeltaC_Percent.Controls.Add(this.tabPage8);
            this.tbCtl_Two_DeltaC_Percent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCtl_Two_DeltaC_Percent.Location = new System.Drawing.Point(0, 0);
            this.tbCtl_Two_DeltaC_Percent.Multiline = true;
            this.tbCtl_Two_DeltaC_Percent.Name = "tbCtl_Two_DeltaC_Percent";
            this.tbCtl_Two_DeltaC_Percent.SelectedIndex = 0;
            this.tbCtl_Two_DeltaC_Percent.Size = new System.Drawing.Size(746, 426);
            this.tbCtl_Two_DeltaC_Percent.TabIndex = 2;
            this.tbCtl_Two_DeltaC_Percent.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tbCtrl_Chart_DrawItem);
            // 
            // tabPage7
            // 
            this.tabPage7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.tabPage7.Controls.Add(this.chart_TwoShot_DeltaCsPercent_FixDump);
            this.tabPage7.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabPage7.Location = new System.Drawing.Point(4, 4);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(738, 400);
            this.tabPage7.TabIndex = 0;
            this.tabPage7.Text = "(Sweep Charge)";
            // 
            // chart_TwoShot_DeltaCsPercent_FixDump
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Bottom.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Bottom.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Bottom.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Bottom.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Depth.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Depth.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Depth.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Depth.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.DepthTop.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.DepthTop.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.DepthTop.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.DepthTop.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Left.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Left.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Left.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Left.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Right.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Right.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Right.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Right.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Top.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Top.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Top.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Axes.Top.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Cursor = System.Windows.Forms.Cursors.Default;
            this.chart_TwoShot_DeltaCsPercent_FixDump.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Footer.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Footer.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Header.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Header.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Header.Lines = new string[] {
        "Max Data"};
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Legend.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Legend.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Legend.CheckBoxes = true;
            this.chart_TwoShot_DeltaCsPercent_FixDump.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Legend.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Legend.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Location = new System.Drawing.Point(3, 3);
            this.chart_TwoShot_DeltaCsPercent_FixDump.Margin = new System.Windows.Forms.Padding(0);
            this.chart_TwoShot_DeltaCsPercent_FixDump.Name = "chart_TwoShot_DeltaCsPercent_FixDump";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Panel.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Panel.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Panel.Brush.Gradient.Visible = false;
            this.chart_TwoShot_DeltaCsPercent_FixDump.Panel.MarginBottom = 3D;
            this.chart_TwoShot_DeltaCsPercent_FixDump.Panel.MarginTop = 3D;
            this.chart_TwoShot_DeltaCsPercent_FixDump.Panel.Transparent = true;
            this.chart_TwoShot_DeltaCsPercent_FixDump.Size = new System.Drawing.Size(732, 394);
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.SubFooter.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.SubFooter.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.SubHeader.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.SubHeader.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCsPercent_FixDump.TabIndex = 5;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Back.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Back.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Back.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Back.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Bottom.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Bottom.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Left.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Left.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Right.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Right.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCsPercent_FixDump.Walls.Visible = false;
            // 
            // tabPage8
            // 
            this.tabPage8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.tabPage8.Controls.Add(this.chart_TwoShot_DeltaCsPercent_FixCharge);
            this.tabPage8.Location = new System.Drawing.Point(4, 4);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(738, 400);
            this.tabPage8.TabIndex = 1;
            this.tabPage8.Text = "(Sweep Dump)";
            // 
            // chart_TwoShot_DeltaCsPercent_FixCharge
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Bottom.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Bottom.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Bottom.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Bottom.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Depth.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Depth.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Depth.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Depth.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.DepthTop.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.DepthTop.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.DepthTop.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.DepthTop.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Left.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Left.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Left.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Left.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Right.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Right.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Right.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Right.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Top.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Top.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Top.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Axes.Top.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Cursor = System.Windows.Forms.Cursors.Default;
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Footer.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Footer.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Header.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Header.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Header.Lines = new string[] {
        "Max Data"};
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Legend.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Legend.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Legend.CheckBoxes = true;
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Legend.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Legend.Title.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Location = new System.Drawing.Point(3, 3);
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Margin = new System.Windows.Forms.Padding(0);
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Name = "chart_TwoShot_DeltaCsPercent_FixCharge";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Panel.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Panel.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Panel.Brush.Gradient.Visible = false;
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Panel.MarginBottom = 3D;
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Panel.MarginTop = 3D;
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Panel.Transparent = true;
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Size = new System.Drawing.Size(732, 394);
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.SubFooter.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.SubFooter.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.SubHeader.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.SubHeader.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.TabIndex = 5;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Back.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Back.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Back.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Back.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Bottom.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Bottom.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Left.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Left.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Right.Bevel.StringColorOne = "FFFFFFFF";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Right.Bevel.StringColorTwo = "FF808080";
            this.chart_TwoShot_DeltaCsPercent_FixCharge.Walls.Visible = false;
            // 
            // richTextBox_msg
            // 
            this.richTextBox_msg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.richTextBox_msg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_msg.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.richTextBox_msg.ForeColor = System.Drawing.Color.Red;
            this.richTextBox_msg.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_msg.Name = "richTextBox_msg";
            this.richTextBox_msg.ReadOnly = true;
            this.richTextBox_msg.Size = new System.Drawing.Size(754, 130);
            this.richTextBox_msg.TabIndex = 0;
            this.richTextBox_msg.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Max,
            this.toolStripMenuItem_Avg});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
            // 
            // toolStripMenuItem_Max
            // 
            this.toolStripMenuItem_Max.Checked = true;
            this.toolStripMenuItem_Max.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem_Max.Name = "toolStripMenuItem_Max";
            this.toolStripMenuItem_Max.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem_Max.Text = "Max Data";
            this.toolStripMenuItem_Max.Click += new System.EventHandler(this.toolStripMenuItem_Click);
            // 
            // toolStripMenuItem_Avg
            // 
            this.toolStripMenuItem_Avg.Name = "toolStripMenuItem_Avg";
            this.toolStripMenuItem_Avg.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem_Avg.Text = "Avg Data";
            this.toolStripMenuItem_Avg.Click += new System.EventHandler(this.toolStripMenuItem_Click);
            // 
            // ChargeCurvefm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.ClientSize = new System.Drawing.Size(1147, 586);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.splitContainer_Main);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChargeCurvefm";
            this.Text = "ChargeCurvefm";
            this.Load += new System.EventHandler(this.ChargeCurvefm_Load);
            this.VisibleChanged += new System.EventHandler(this.ChargeCurvefm_VisibleChanged);
            this.splitContainer_Main.Panel1.ResumeLayout(false);
            this.splitContainer_Main.Panel1.PerformLayout();
            this.splitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).EndInit();
            this.splitContainer_Main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridView_Series)).EndInit();
            this.tableLayout_Button.ResumeLayout(false);
            this.tableLayout_Button.PerformLayout();
            this.groupBox_FixParams.ResumeLayout(false);
            this.groupBox_FixParams.PerformLayout();
            this.uiGroupBox_CSub.ResumeLayout(false);
            this.uiGroupBox_CSub.PerformLayout();
            this.splitContainer_AllParams.Panel1.ResumeLayout(false);
            this.splitContainer_AllParams.Panel1.PerformLayout();
            this.splitContainer_AllParams.Panel2.ResumeLayout(false);
            this.splitContainer_AllParams.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_AllParams)).EndInit();
            this.splitContainer_AllParams.ResumeLayout(false);
            this.gbSweepCharge.ResumeLayout(false);
            this.gbSweepCharge.PerformLayout();
            this.gbSweepDump_Freq.ResumeLayout(false);
            this.gbSweepDump_Freq.PerformLayout();
            this.groupBox_ScanMode.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tbCtrl_Chart.ResumeLayout(false);
            this.tbOne_RawCode.ResumeLayout(false);
            this.tbCtl_One_RawCode.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tbTwo_RawCode.ResumeLayout(false);
            this.tbCtl_Two_RawCode.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tbTwo_DeltaC_Code.ResumeLayout(false);
            this.tbCtl_Two_DeltaC_Code.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tbTwo_DeltaC_Percent.ResumeLayout(false);
            this.tbCtl_Two_DeltaC_Percent.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_Main;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tbCtrl_Chart;
        private System.Windows.Forms.TabPage tbOne_RawCode;
        private System.Windows.Forms.TabPage tbTwo_DeltaC_Code;
        private System.Windows.Forms.Label lbSweepCharge_Tidle;
        private System.Windows.Forms.Label lbSweepCharge_RangeName;
        private System.Windows.Forms.Label lbSweepCharge_StepName;
        private System.Windows.Forms.TabPage tbTwo_DeltaC_Percent;
        private System.Windows.Forms.TabControl tbCtl_One_RawCode;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tbTwo_RawCode;
        private System.Windows.Forms.TabControl tbCtl_Two_RawCode;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabControl tbCtl_Two_DeltaC_Code;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabControl tbCtl_Two_DeltaC_Percent;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private RadioListBox radioListBox_ChartDisp;
        private System.Windows.Forms.DataGridView GridView_Series;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.CheckBox cb_TwoShotEn;
        private System.Windows.Forms.Label lbFrame_Name;
        private System.Windows.Forms.GroupBox groupBox_FixParams;
        private System.Windows.Forms.GroupBox groupBox_ScanMode;
        private System.Windows.Forms.Label lbSweepCharge_FixDumpName;
        private System.Windows.Forms.Label lbSweepCharge_RangeUnit;
        private System.Windows.Forms.Label lbSweepCharge_StepUnit;
        private System.Windows.Forms.TableLayoutPanel tableLayout_Button;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label lbSweepCharge_PostIdleName;
        private System.Windows.Forms.Label lbSweepDump_PostIdleName;
        private System.Windows.Forms.Label lbSweepDump_RangeName;
        private System.Windows.Forms.Label lbSweepDump_Tidle;
        private System.Windows.Forms.Label lbSweepDump_StepName;
        private System.Windows.Forms.Label lbSweepDump_RangeUnit;
        private System.Windows.Forms.Label lbSweepDump_FixChargeName;
        private System.Windows.Forms.Label lbSweepDump_StepUnit;
        private System.Windows.Forms.TextBox tbSweepDump_RangeStart;
        private System.Windows.Forms.TextBox tbSweepCharge_PostIdleValue;
        private System.Windows.Forms.TextBox tbSweepCharge_RangeEnd;
        private System.Windows.Forms.TextBox tbSweepCharge_RangeStart;
        private System.Windows.Forms.TextBox tbSweepDump_FixChargeValue;
        private System.Windows.Forms.TextBox tbSweepDump_PostIdleValue;
        private System.Windows.Forms.TextBox tbSweepDump_RangeEnd;
        private System.Windows.Forms.TextBox tbSweepCharge_FixDumpValue;
        private System.Windows.Forms.SplitContainer splitContainer_AllParams;
        private System.Windows.Forms.TextBox tbFrame_Value;
        private System.Windows.Forms.TextBox tbSweepDump_StepValue;
        private System.Windows.Forms.TextBox tbSweepCharge_StepValue;
        private System.Windows.Forms.RichTextBox richTextBox_msg;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Max;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Avg;
        private System.Windows.Forms.Label lbSweepDump_FixChargeUnit;
        private System.Windows.Forms.Label lbSweepDump_PostIdleUnit;
        private System.Windows.Forms.Label lbSweepCharge_FixDumpUnit;
        private System.Windows.Forms.Label lbSweepCharge_PostIdleUnit;
        private Steema.TeeChart.TChart chart_OneShot_FixDump;
        private Steema.TeeChart.TChart chart_OneShot_FixCharge;
        private Steema.TeeChart.TChart chart_TwoShot_Raw_FixDump;
        private Steema.TeeChart.TChart chart_TwoShot_Raw_FixCharge;
        private Steema.TeeChart.TChart chart_TwoShot_DeltaCs_FixDump;
        private Steema.TeeChart.TChart chart_TwoShot_DeltaCs_FixCharge;
        private Steema.TeeChart.TChart chart_TwoShot_DeltaCsPercent_FixDump;
        private Steema.TeeChart.TChart chart_TwoShot_DeltaCsPercent_FixCharge;
        private Steema.TeeChart.Styles.FastLine fastLine1;
        private Sunny.UI.UIGroupBox uiGroupBox_CSub;
        private Sunny.UI.UITextBox tbFixSC_X;
        private Sunny.UI.UIMarkLabel lbFixSC_X;
        private Sunny.UI.UITextBox tbFixSC_Y;
        private Sunny.UI.UIMarkLabel lbFixSC_Y;
        private Sunny.UI.UIGroupBox gbSweepCharge;
        private Sunny.UI.UIGroupBox gbSweepDump_Freq;
        private System.Windows.Forms.CheckBox cbRecord;
    }
}