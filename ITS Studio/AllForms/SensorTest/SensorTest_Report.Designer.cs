namespace ITS_Studio
{
    partial class SensorTest_Report
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SensorTest_Report));
            this.dataGridView_Result = new System.Windows.Forms.DataGridView();
            this.Label_Panel = new System.Windows.Forms.Panel();
            this.uiTitlePanel_SpecData = new Sunny.UI.UITitlePanel();
            this.dataGridView_SPEC = new System.Windows.Forms.DataGridView();
            this.tChart1 = new Steema.TeeChart.TChart();
            this.colorGrid1 = new Steema.TeeChart.Styles.ColorGrid();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage_Chart = new System.Windows.Forms.TabPage();
            this.tabPage_Gridview = new System.Windows.Forms.TabPage();
            this.uiToolTip1 = new Sunny.UI.UIToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Result)).BeginInit();
            this.uiTitlePanel_SpecData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_SPEC)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage_Chart.SuspendLayout();
            this.tabPage_Gridview.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_Result
            // 
            this.dataGridView_Result.AllowUserToAddRows = false;
            this.dataGridView_Result.AllowUserToDeleteRows = false;
            this.dataGridView_Result.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Result.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_Result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Result.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Result.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_Result.MultiSelect = false;
            this.dataGridView_Result.Name = "dataGridView_Result";
            this.dataGridView_Result.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Result.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_Result.RowTemplate.Height = 24;
            this.dataGridView_Result.Size = new System.Drawing.Size(978, 371);
            this.dataGridView_Result.TabIndex = 2;
            this.dataGridView_Result.SelectionChanged += new System.EventHandler(this.dataGridView_Result_SelectionChanged);
            // 
            // Label_Panel
            // 
            this.Label_Panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.Label_Panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Label_Panel.Location = new System.Drawing.Point(0, 284);
            this.Label_Panel.Name = "Label_Panel";
            this.Label_Panel.Size = new System.Drawing.Size(992, 41);
            this.Label_Panel.TabIndex = 2;
            // 
            // uiTitlePanel_SpecData
            // 
            this.uiTitlePanel_SpecData.Controls.Add(this.dataGridView_SPEC);
            this.uiTitlePanel_SpecData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiTitlePanel_SpecData.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.uiTitlePanel_SpecData.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.uiTitlePanel_SpecData.ForeColor = System.Drawing.Color.White;
            this.uiTitlePanel_SpecData.Location = new System.Drawing.Point(0, 325);
            this.uiTitlePanel_SpecData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel_SpecData.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel_SpecData.Name = "uiTitlePanel_SpecData";
            this.uiTitlePanel_SpecData.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.uiTitlePanel_SpecData.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTitlePanel_SpecData.ShowCollapse = true;
 
            this.uiTitlePanel_SpecData.Size = new System.Drawing.Size(992, 376);
            this.uiTitlePanel_SpecData.Style = Sunny.UI.UIStyle.Custom;
            this.uiTitlePanel_SpecData.TabIndex = 4;
            this.uiTitlePanel_SpecData.Text = "Spec. Data";
            this.uiTitlePanel_SpecData.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiTitlePanel_SpecData.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
         
            this.uiTitlePanel_SpecData.ClientSizeChanged += new System.EventHandler(this.uiTitlePanel_SpecData_ClientSizeChanged);
            // 
            // dataGridView_SPEC
            // 
            this.dataGridView_SPEC.AllowUserToAddRows = false;
            this.dataGridView_SPEC.AllowUserToDeleteRows = false;
            this.dataGridView_SPEC.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.dataGridView_SPEC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_SPEC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_SPEC.Location = new System.Drawing.Point(0, 35);
            this.dataGridView_SPEC.Name = "dataGridView_SPEC";
            this.dataGridView_SPEC.ReadOnly = true;
            this.dataGridView_SPEC.RowHeadersVisible = false;
            this.dataGridView_SPEC.RowTemplate.Height = 24;
            this.dataGridView_SPEC.Size = new System.Drawing.Size(992, 341);
            this.dataGridView_SPEC.TabIndex = 3;
            this.dataGridView_SPEC.Paint += new System.Windows.Forms.PaintEventHandler(this.dataGridView_SPEC_Paint);
            // 
            // tChart1
            // 
            // 
            // 
            // 
            this.tChart1.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tChart1.Axes.Bottom.AxisPen.Width = 1;
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Grid.Centered = true;
            this.tChart1.Axes.Bottom.Grid.Visible = false;
            this.tChart1.Axes.Bottom.Increment = 1D;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Axes.Bottom.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Axes.Bottom.Labels.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Axes.Bottom.Labels.OnAxis = false;
            this.tChart1.Axes.Bottom.Labels.ValueFormat = "0";
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.MinorTicks.Length = 1;
            this.tChart1.Axes.Bottom.MinorTicks.Visible = false;
            this.tChart1.Axes.Bottom.TickOnLabelsOnly = false;
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Ticks.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Axes.Bottom.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Axes.Bottom.Title.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Axes.Bottom.Title.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Axes.Depth.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Axes.Depth.Labels.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Axes.Depth.Labels.OnAxis = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Axes.Depth.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Axes.Depth.Title.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Axes.Depth.Title.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.DepthTop.AxisPen.Visible = false;
            // 
            // 
            // 
            this.tChart1.Axes.DepthTop.Grid.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Axes.DepthTop.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Axes.DepthTop.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.DepthTop.Labels.Font.Shadow.Brush.Visible = false;
            this.tChart1.Axes.DepthTop.Labels.OnAxis = false;
            this.tChart1.Axes.DepthTop.Labels.ValueFormat = "0";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Axes.DepthTop.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Axes.DepthTop.Title.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Axes.DepthTop.Title.Visible = false;
            this.tChart1.Axes.DrawBehind = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Left.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tChart1.Axes.Left.AxisPen.Width = 1;
            // 
            // 
            // 
            this.tChart1.Axes.Left.Grid.Centered = true;
            this.tChart1.Axes.Left.Grid.Color = System.Drawing.Color.WhiteSmoke;
            this.tChart1.Axes.Left.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            this.tChart1.Axes.Left.Grid.Visible = false;
            this.tChart1.Axes.Left.Increment = 1D;
            this.tChart1.Axes.Left.Inverted = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Axes.Left.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Axes.Left.Labels.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Axes.Left.Labels.OnAxis = false;
            this.tChart1.Axes.Left.Labels.ValueFormat = "0";
            // 
            // 
            // 
            this.tChart1.Axes.Left.MinorTicks.Length = 1;
            this.tChart1.Axes.Left.MinorTicks.Visible = false;
            this.tChart1.Axes.Left.TickOnLabelsOnly = false;
            // 
            // 
            // 
            this.tChart1.Axes.Left.Ticks.Length = 0;
            this.tChart1.Axes.Left.Ticks.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Axes.Left.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Axes.Left.Title.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Axes.Left.Title.Visible = false;
            // 
            // 
            // 
            this.tChart1.Axes.Right.Automatic = false;
            this.tChart1.Axes.Right.AutomaticMaximum = false;
            this.tChart1.Axes.Right.AutomaticMinimum = false;
            // 
            // 
            // 
            this.tChart1.Axes.Right.AxisPen.Width = 1;
            // 
            // 
            // 
            this.tChart1.Axes.Right.Grid.Centered = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Axes.Right.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Axes.Right.Labels.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Axes.Right.Labels.OnAxis = false;
            this.tChart1.Axes.Right.Labels.Visible = false;
            this.tChart1.Axes.Right.Maximum = 1.5D;
            this.tChart1.Axes.Right.Minimum = 0.5D;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Axes.Right.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Axes.Right.Title.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Axes.Right.Title.Visible = false;
            // 
            // 
            // 
            this.tChart1.Axes.Top.Automatic = false;
            this.tChart1.Axes.Top.AutomaticMaximum = false;
            this.tChart1.Axes.Top.AutomaticMinimum = false;
            // 
            // 
            // 
            this.tChart1.Axes.Top.AxisPen.Width = 1;
            // 
            // 
            // 
            this.tChart1.Axes.Top.Grid.Centered = true;
            this.tChart1.Axes.Top.Grid.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Axes.Top.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Axes.Top.Labels.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Axes.Top.Labels.OnAxis = false;
            this.tChart1.Axes.Top.Labels.Visible = false;
            this.tChart1.Axes.Top.Maximum = 0D;
            this.tChart1.Axes.Top.Minimum = 0D;
            // 
            // 
            // 
            this.tChart1.Axes.Top.MinorTicks.Visible = false;
            this.tChart1.Axes.Top.TickOnLabelsOnly = false;
            // 
            // 
            // 
            this.tChart1.Axes.Top.Ticks.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Axes.Top.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Axes.Top.Title.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Axes.Top.Title.Visible = false;
            this.tChart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.tChart1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Footer.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Footer.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.tChart1.Header.AdjustFrame = false;
            // 
            // 
            // 
            this.tChart1.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Header.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Header.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Header.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Legend.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Legend.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Legend.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Legend.Title.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Legend.Visible = false;
            this.tChart1.Location = new System.Drawing.Point(3, 3);
            this.tChart1.Name = "tChart1";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Panel.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Panel.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.tChart1.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            // 
            // 
            // 
            this.tChart1.Panel.Brush.Gradient.Visible = false;
            this.tChart1.Panel.Brush.Visible = false;
            this.tChart1.Panel.MarginBottom = 0D;
            this.tChart1.Panel.MarginLeft = 0D;
            this.tChart1.Panel.MarginRight = 0D;
            this.tChart1.Panel.MarginTop = 0D;
            // 
            // 
            // 
            this.tChart1.Panel.Pen.Visible = true;
            // 
            // 
            // 
            this.tChart1.Panel.Shadow.Smooth = true;
            this.tChart1.Panel.Shadow.Visible = true;
            this.tChart1.Panel.Transparent = true;
            // 
            // 
            // 
            this.tChart1.Panning.Allow = Steema.TeeChart.ScrollModes.None;
            this.tChart1.Series.Add(this.colorGrid1);
            this.tChart1.Size = new System.Drawing.Size(978, 269);
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.SubFooter.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.SubFooter.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.SubHeader.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.SubHeader.Bevel.StringColorTwo = "FF808080";
            this.tChart1.TabIndex = 0;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Walls.Back.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Walls.Back.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Walls.Bottom.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Walls.Bottom.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Walls.Left.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Walls.Left.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.tChart1.Walls.Left.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tChart1.Walls.Left.Brush.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Walls.Right.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart1.Walls.Right.Bevel.StringColorTwo = "FF808080";
            this.tChart1.Walls.Visible = false;
            this.tChart1.ClickSeries += new Steema.TeeChart.TChart.SeriesEventHandler(this.tChart1_ClickSeries);
            this.tChart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tChart1_MouseClick);
            this.tChart1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tChart1_MouseDoubleClick);
            this.tChart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tChart1_MouseDown);
            // 
            // colorGrid1
            // 
            // 
            // 
            // 
            this.colorGrid1.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.colorGrid1.CenteredPoints = true;
            this.colorGrid1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.colorGrid1.ColorEach = false;
            this.colorGrid1.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorGrid1.HorizAxis = Steema.TeeChart.Styles.HorizontalAxis.Both;
            // 
            // 
            // 
            // 
            // 
            // 
            this.colorGrid1.Marks.Arrow.Visible = false;
            this.colorGrid1.Marks.ArrowLength = 0;
            // 
            // 
            // 
            this.colorGrid1.Marks.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.colorGrid1.Marks.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.colorGrid1.Marks.Bevel.StringColorOne = "FFFFFFFF";
            this.colorGrid1.Marks.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.colorGrid1.Marks.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.colorGrid1.Marks.Brush.Visible = false;
            // 
            // 
            // 
            this.colorGrid1.Marks.Font.Name = "微軟正黑體";
            // 
            // 
            // 
            // 
            // 
            // 
            this.colorGrid1.Marks.Font.Shadow.Brush.Visible = false;
            this.colorGrid1.Marks.Font.Shadow.SmoothBlur = -75;
            // 
            // 
            // 
            this.colorGrid1.Marks.Shadow.Visible = false;
            this.colorGrid1.Marks.ShapeStyle = Steema.TeeChart.Drawing.TextShapeStyle.RoundRectangle;
            this.colorGrid1.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Value;
            // 
            // 
            // 
            // 
            // 
            // 
            this.colorGrid1.Marks.Symbol.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.colorGrid1.Marks.Symbol.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.colorGrid1.Marks.Symbol.Bevel.StringColorOne = "FFFFFFFF";
            this.colorGrid1.Marks.Symbol.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.colorGrid1.Marks.Symbol.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(238)))), ((int)(((byte)(236)))), ((int)(((byte)(62)))));
            // 
            // 
            // 
            this.colorGrid1.Marks.Symbol.Brush.Gradient.Transparency = 100;
            // 
            // 
            // 
            // 
            // 
            // 
            this.colorGrid1.Marks.Symbol.Shadow.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            // 
            // 
            // 
            this.colorGrid1.Marks.Symbol.Shadow.Brush.Gradient.Transparency = 100;
            this.colorGrid1.Marks.Symbol.Shadow.Visible = false;
            this.colorGrid1.Marks.Transparent = true;
            this.colorGrid1.Marks.Visible = true;
            this.colorGrid1.MidColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.colorGrid1.NumXValues = 4;
            this.colorGrid1.NumZValues = 4;
            this.colorGrid1.PaletteMin = 0D;
            this.colorGrid1.PaletteStep = 0D;
            this.colorGrid1.PaletteSteps = 2;
            this.colorGrid1.PaletteStyle = Steema.TeeChart.Styles.PaletteStyles.Pale;
            this.colorGrid1.ShowInLegend = false;
            this.colorGrid1.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.colorGrid1.TimesZOrder = 1;
            this.colorGrid1.Title = "colorGrid1";
            this.colorGrid1.VertAxis = Steema.TeeChart.Styles.VerticalAxis.Both;
            this.colorGrid1.XStep = 1;
            // 
            // 
            // 
            this.colorGrid1.XValues.DataMember = "X";
            // 
            // 
            // 
            this.colorGrid1.YValues.DataMember = "Y";
            this.colorGrid1.ZStep = 1;
            // 
            // 
            // 
            this.colorGrid1.ZValues.DataMember = "Z";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage_Chart);
            this.tabControl2.Controls.Add(this.tabPage_Gridview);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.ItemSize = new System.Drawing.Size(0, 1);
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(992, 284);
            this.tabControl2.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl2.TabIndex = 5;
            // 
            // tabPage_Chart
            // 
            this.tabPage_Chart.Controls.Add(this.tChart1);
            this.tabPage_Chart.Location = new System.Drawing.Point(4, 5);
            this.tabPage_Chart.Name = "tabPage_Chart";
            this.tabPage_Chart.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Chart.Size = new System.Drawing.Size(984, 275);
            this.tabPage_Chart.TabIndex = 1;
            this.tabPage_Chart.Text = "tabPage_Chart";
            this.tabPage_Chart.UseVisualStyleBackColor = true;
            // 
            // tabPage_Gridview
            // 
            this.tabPage_Gridview.Controls.Add(this.dataGridView_Result);
            this.tabPage_Gridview.Location = new System.Drawing.Point(4, 5);
            this.tabPage_Gridview.Name = "tabPage_Gridview";
            this.tabPage_Gridview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Gridview.Size = new System.Drawing.Size(984, 377);
            this.tabPage_Gridview.TabIndex = 0;
            this.tabPage_Gridview.Text = "tabPage_Gridview";
            this.tabPage_Gridview.UseVisualStyleBackColor = true;
            // 
            // uiToolTip1
            // 
            this.uiToolTip1.AutomaticDelay = 0;
            this.uiToolTip1.AutoPopDelay = 10000;
            this.uiToolTip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.uiToolTip1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiToolTip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.uiToolTip1.InitialDelay = 0;
            this.uiToolTip1.OwnerDraw = true;
            this.uiToolTip1.ReshowDelay = 0;
            this.uiToolTip1.TitleFont = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // SensorTest_Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.ClientSize = new System.Drawing.Size(992, 701);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.Label_Panel);
            this.Controls.Add(this.uiTitlePanel_SpecData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SensorTest_Report";
            this.Text = "SensorTest_Report";
            this.Load += new System.EventHandler(this.SensorTest_Report_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Result)).EndInit();
            this.uiTitlePanel_SpecData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_SPEC)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage_Chart.ResumeLayout(false);
            this.tabPage_Gridview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_Result;
        private System.Windows.Forms.Panel Label_Panel;
        private System.Windows.Forms.DataGridView dataGridView_SPEC;
        private Sunny.UI.UITitlePanel uiTitlePanel_SpecData;
        private Steema.TeeChart.TChart tChart1;
        private Steema.TeeChart.Styles.ColorGrid colorGrid1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage_Gridview;
        private System.Windows.Forms.TabPage tabPage_Chart;
        private Sunny.UI.UIToolTip uiToolTip1;


    }
}