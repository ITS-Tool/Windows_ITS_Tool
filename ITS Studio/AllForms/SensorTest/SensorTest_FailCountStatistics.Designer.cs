namespace ITS_Studio
{
    partial class SensorTest_FailCountStatistics
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
            if(disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SensorTest_FailCountStatistics));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tChart_Pie = new Steema.TeeChart.TChart();
            this.pie_FailCounts = new Steema.TeeChart.Styles.Pie();
            this.uiTabControlMenu1 = new Sunny.UI.UITabControlMenu();
            this.tabPage_Files = new System.Windows.Forms.TabPage();
            this.tChart_HorizBar = new Steema.TeeChart.TChart();
            this.horizBar1 = new Steema.TeeChart.Styles.HorizBar();
            this.horizBar2 = new Steema.TeeChart.Styles.HorizBar();
            this.uiDataGridView1 = new Sunny.UI.UIDataGridView();
            this.Column_FailItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_LogNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Percent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage_Items = new System.Windows.Forms.TabPage();
            this.marksTip1 = new Steema.TeeChart.Tools.MarksTip();
            this.uiTabControlMenu1.SuspendLayout();
            this.tabPage_Files.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).BeginInit();
            this.tabPage_Items.SuspendLayout();
            this.SuspendLayout();
            // 
            // tChart_Pie
            // 
            // 
            // 
            // 
            this.tChart_Pie.Aspect.Elevation = 315;
            this.tChart_Pie.Aspect.ElevationFloat = 315D;
            this.tChart_Pie.Aspect.Orthogonal = false;
            this.tChart_Pie.Aspect.Perspective = 0;
            this.tChart_Pie.Aspect.Rotation = 360;
            this.tChart_Pie.Aspect.RotationFloat = 360D;
            this.tChart_Pie.Aspect.View3D = false;
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
            this.tChart_Pie.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Axes.Bottom.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Axes.Bottom.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Axes.Bottom.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Axes.Bottom.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Axes.Depth.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Axes.Depth.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Axes.Depth.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Axes.Depth.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Axes.DepthTop.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Axes.DepthTop.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Axes.DepthTop.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Axes.DepthTop.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Axes.Left.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Axes.Left.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Axes.Left.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Axes.Left.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Axes.Right.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Axes.Right.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Axes.Right.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Axes.Right.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Axes.Top.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Axes.Top.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Axes.Top.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Axes.Top.Title.Bevel.StringColorTwo = "FF808080";
            this.tChart_Pie.Axes.Visible = false;
            this.tChart_Pie.BackColor = System.Drawing.Color.Transparent;
            this.tChart_Pie.Cursor = System.Windows.Forms.Cursors.Default;
            this.tChart_Pie.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Footer.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Footer.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.tChart_Pie.Header.Alignment = System.Drawing.StringAlignment.Near;
            // 
            // 
            // 
            this.tChart_Pie.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Header.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Header.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.tChart_Pie.Header.Font.Bold = true;
            this.tChart_Pie.Header.Font.Name = "HP Simplified";
            this.tChart_Pie.Header.Font.Size = 15;
            this.tChart_Pie.Header.Font.SizeFloat = 15F;
            this.tChart_Pie.Header.Lines = new string[] {
        "Total fail items percentage."};
            this.tChart_Pie.Header.TextAlign = System.Drawing.StringAlignment.Center;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Legend.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Legend.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.tChart_Pie.Legend.Brush.ImageMode = Steema.TeeChart.Drawing.ImageMode.Normal;
            this.tChart_Pie.Legend.ImageMode = Steema.TeeChart.Drawing.ImageMode.Normal;
            this.tChart_Pie.Legend.MaxNumRows = 1;
            this.tChart_Pie.Legend.ResizeChart = false;
            // 
            // 
            // 
            this.tChart_Pie.Legend.Shadow.Visible = false;
            // 
            // 
            // 
            this.tChart_Pie.Legend.Symbol.Width = 10;
            this.tChart_Pie.Legend.TextStyle = Steema.TeeChart.LegendTextStyles.RightValue;
            this.tChart_Pie.Legend.TextSymbolGap = 0;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Legend.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Legend.Title.Bevel.StringColorTwo = "FF808080";
            this.tChart_Pie.Legend.TopLeftPos = 0;
            this.tChart_Pie.Legend.Transparent = true;
            this.tChart_Pie.Location = new System.Drawing.Point(0, 0);
            this.tChart_Pie.Name = "tChart_Pie";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Panel.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Panel.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Panel.Brush.Gradient.Visible = false;
            this.tChart_Pie.Panel.MarginBottom = 0D;
            this.tChart_Pie.Panel.MarginLeft = 0D;
            this.tChart_Pie.Panel.MarginRight = 0D;
            this.tChart_Pie.Panel.MarginTop = 0D;
            this.tChart_Pie.Panel.Transparent = true;
            // 
            // 
            // 
            this.tChart_Pie.Panning.Allow = Steema.TeeChart.ScrollModes.None;
            this.tChart_Pie.Series.Add(this.pie_FailCounts);
            this.tChart_Pie.Size = new System.Drawing.Size(793, 613);
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.SubFooter.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.SubFooter.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.SubHeader.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.SubHeader.Bevel.StringColorTwo = "FF808080";
            this.tChart_Pie.TabIndex = 4;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Walls.Back.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Walls.Back.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Walls.Bottom.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Walls.Bottom.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Walls.Left.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Walls.Left.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_Pie.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_Pie.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_Pie.Walls.Right.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_Pie.Walls.Right.Bevel.StringColorTwo = "FF808080";
            this.tChart_Pie.Walls.Visible = false;
            // 
            // 
            // 
            this.tChart_Pie.Zoom.Allow = false;
            // 
            // pie_FailCounts
            // 
            // 
            // 
            // 
            this.pie_FailCounts.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(192)))), ((int)(((byte)(93)))));
            this.pie_FailCounts.Circled = true;
            this.pie_FailCounts.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.pie_FailCounts.Dark3D = false;
            this.pie_FailCounts.Depth = 0;
            this.pie_FailCounts.LabelMember = "Labels";
            // 
            // 
            // 
            this.pie_FailCounts.MarksPie.LegSize = 0;
            this.pie_FailCounts.MarksPie.VertCenter = false;
            this.pie_FailCounts.MultiPie = Steema.TeeChart.Styles.MultiPies.Disabled;
            // 
            // 
            // 
            this.pie_FailCounts.OtherSlice.Color = System.Drawing.Color.Empty;
            this.pie_FailCounts.OtherSlice.Style = Steema.TeeChart.Styles.PieOtherStyles.None;
            this.pie_FailCounts.OtherSlice.Text = "";
            this.pie_FailCounts.OtherSlice.Value = 0D;
            this.pie_FailCounts.Title = "pie_FailCounts";
            this.pie_FailCounts.Transparency = 0;
            // 
            // 
            // 
            this.pie_FailCounts.XValues.DataMember = "Angle";
            this.pie_FailCounts.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.pie_FailCounts.YValues.DataMember = "Pie";
            // 
            // uiTabControlMenu1
            // 
            this.uiTabControlMenu1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.uiTabControlMenu1.Controls.Add(this.tabPage_Files);
            this.uiTabControlMenu1.Controls.Add(this.tabPage_Items);
            this.uiTabControlMenu1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTabControlMenu1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiTabControlMenu1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiTabControlMenu1.Location = new System.Drawing.Point(0, 0);
            this.uiTabControlMenu1.Multiline = true;
            this.uiTabControlMenu1.Name = "uiTabControlMenu1";
            this.uiTabControlMenu1.SelectedIndex = 0;
            this.uiTabControlMenu1.Size = new System.Drawing.Size(994, 613);
            this.uiTabControlMenu1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiTabControlMenu1.TabIndex = 5;
            this.uiTabControlMenu1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiTabControlMenu1.SelectedIndexChanged += new System.EventHandler(this.uiTabControlMenu1_SelectedIndexChanged);
            this.uiTabControlMenu1.Selected += new System.Windows.Forms.TabControlEventHandler(this.uiTabControlMenu1_Selected);
            // 
            // tabPage_Files
            // 
            this.tabPage_Files.Controls.Add(this.tChart_HorizBar);
            this.tabPage_Files.Controls.Add(this.uiDataGridView1);
            this.tabPage_Files.Location = new System.Drawing.Point(201, 0);
            this.tabPage_Files.Name = "tabPage_Files";
            this.tabPage_Files.Size = new System.Drawing.Size(793, 613);
            this.tabPage_Files.TabIndex = 1;
            this.tabPage_Files.Text = "Fail檔案分佈";
            this.tabPage_Files.UseVisualStyleBackColor = true;
            // 
            // tChart_HorizBar
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Aspect.ColorPaletteIndex = 9;
            this.tChart_HorizBar.Aspect.View3D = false;
            this.tChart_HorizBar.Aspect.ZoomText = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Bottom.Automatic = false;
            this.tChart_HorizBar.Axes.Bottom.AutomaticMaximum = false;
            this.tChart_HorizBar.Axes.Bottom.AutomaticMinimum = false;
            this.tChart_HorizBar.Axes.Bottom.IncrementOffset = 1D;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Axes.Bottom.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Axes.Bottom.Labels.Bevel.StringColorTwo = "FF808080";
            this.tChart_HorizBar.Axes.Bottom.Maximum = 110D;
            this.tChart_HorizBar.Axes.Bottom.MaximumOffset = 1;
            this.tChart_HorizBar.Axes.Bottom.MaximumRound = true;
            this.tChart_HorizBar.Axes.Bottom.Minimum = 0D;
            this.tChart_HorizBar.Axes.Bottom.MinimumRound = true;
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Bottom.MinorTicks.Length = 0;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Axes.Bottom.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Axes.Bottom.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Axes.Depth.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Axes.Depth.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Axes.Depth.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Axes.Depth.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Axes.DepthTop.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Axes.DepthTop.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Axes.DepthTop.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Axes.DepthTop.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Left.Automatic = false;
            this.tChart_HorizBar.Axes.Left.AutomaticMaximum = false;
            this.tChart_HorizBar.Axes.Left.AutomaticMinimum = false;
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Left.AxisPen.Width = 0;
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Left.Grid.Centered = true;
            this.tChart_HorizBar.Axes.Left.Grid.Visible = false;
            this.tChart_HorizBar.Axes.Left.Inverted = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Axes.Left.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Axes.Left.Labels.Bevel.StringColorTwo = "FF808080";
            this.tChart_HorizBar.Axes.Left.Maximum = 5D;
            this.tChart_HorizBar.Axes.Left.Minimum = 0D;
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Left.MinorTicks.Length = 0;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Axes.Left.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Axes.Left.Title.Bevel.StringColorTwo = "FF808080";
            this.tChart_HorizBar.Axes.Left.Title.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Axes.Right.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Axes.Right.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Axes.Right.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Axes.Right.Title.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Axes.Top.Labels.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Axes.Top.Labels.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Axes.Top.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Axes.Top.Title.Bevel.StringColorTwo = "FF808080";
            this.tChart_HorizBar.BackColor = System.Drawing.Color.Transparent;
            this.tChart_HorizBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.tChart_HorizBar.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Footer.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Footer.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.tChart_HorizBar.Header.Alignment = System.Drawing.StringAlignment.Near;
            // 
            // 
            // 
            this.tChart_HorizBar.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Header.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Header.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.tChart_HorizBar.Header.Font.Bold = true;
            this.tChart_HorizBar.Header.Font.Name = "HP Simplified";
            this.tChart_HorizBar.Header.Font.Size = 15;
            this.tChart_HorizBar.Header.Font.SizeFloat = 15F;
            this.tChart_HorizBar.Header.Lines = new string[] {
        "Fail file percentage"};
            this.tChart_HorizBar.Header.TextAlign = System.Drawing.StringAlignment.Center;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Legend.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Legend.Bevel.StringColorTwo = "FF808080";
            this.tChart_HorizBar.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Palette;
            this.tChart_HorizBar.Legend.TextStyle = Steema.TeeChart.LegendTextStyles.Plain;
            this.tChart_HorizBar.Legend.TextSymbolGap = 0;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Legend.Title.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Legend.Title.Bevel.StringColorTwo = "FF808080";
            this.tChart_HorizBar.Legend.Visible = false;
            this.tChart_HorizBar.Location = new System.Drawing.Point(0, 0);
            this.tChart_HorizBar.Name = "tChart_HorizBar";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Panel.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Panel.Bevel.StringColorTwo = "FF808080";
            this.tChart_HorizBar.Series.Add(this.horizBar1);
            this.tChart_HorizBar.Series.Add(this.horizBar2);
            this.tChart_HorizBar.Size = new System.Drawing.Size(793, 315);
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.SubFooter.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.SubFooter.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.SubHeader.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.SubHeader.Bevel.StringColorTwo = "FF808080";
            this.tChart_HorizBar.TabIndex = 5;
            this.tChart_HorizBar.Tools.Add(this.marksTip1);
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Walls.Back.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Walls.Back.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Walls.Bottom.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Walls.Bottom.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Walls.Left.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Walls.Left.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_HorizBar.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart_HorizBar.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart_HorizBar.Walls.Right.Bevel.StringColorOne = "FFFFFFFF";
            this.tChart_HorizBar.Walls.Right.Bevel.StringColorTwo = "FF808080";
            this.tChart_HorizBar.Walls.Visible = false;
            this.tChart_HorizBar.ClickSeries += new Steema.TeeChart.TChart.SeriesEventHandler(this.tChart_HorizBar_ClickSeries);
            // 
            // horizBar1
            // 
            // 
            // 
            // 
            this.horizBar1.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(155)))), ((int)(((byte)(254)))));
            // 
            // 
            // 
            this.horizBar1.Brush.Gradient.Direction = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.horizBar1.Brush.Gradient.Visible = true;
            this.horizBar1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(155)))), ((int)(((byte)(254)))));
            this.horizBar1.ColorEach = false;
            this.horizBar1.ColorMember = "Colors";
            this.horizBar1.Colors = ((Steema.TeeChart.Styles.ColorList)(resources.GetObject("horizBar1.Colors")));
            // 
            // 
            // 
            // 
            // 
            // 
            this.horizBar1.Marks.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.horizBar1.Marks.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.horizBar1.Marks.Bevel.StringColorOne = "FFFFFFFF";
            this.horizBar1.Marks.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.horizBar1.Marks.Shadow.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.horizBar1.Marks.Symbol.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.horizBar1.Marks.Symbol.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.horizBar1.Marks.Symbol.Bevel.StringColorOne = "FFFFFFFF";
            this.horizBar1.Marks.Symbol.Bevel.StringColorTwo = "FF808080";
            this.horizBar1.Marks.Visible = false;
            this.horizBar1.MultiBar = Steema.TeeChart.Styles.MultiBars.None;
            // 
            // 
            // 
            this.horizBar1.Pen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(93)))), ((int)(((byte)(152)))));
            this.horizBar1.Title = "horizBar1";
            // 
            // 
            // 
            this.horizBar1.XValues.DataMember = "X";
            // 
            // 
            // 
            this.horizBar1.YValues.DataMember = "Bar";
            this.horizBar1.YValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // horizBar2
            // 
            // 
            // 
            // 
            this.horizBar2.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(209)))), ((int)(((byte)(36)))));
            // 
            // 
            // 
            this.horizBar2.Brush.Gradient.Direction = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.horizBar2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(209)))), ((int)(((byte)(36)))));
            this.horizBar2.ColorEach = false;
            this.horizBar2.ColorMember = "Colors";
            this.horizBar2.Colors = ((Steema.TeeChart.Styles.ColorList)(resources.GetObject("horizBar2.Colors")));
            this.horizBar2.MultiBar = Steema.TeeChart.Styles.MultiBars.None;
            // 
            // 
            // 
            this.horizBar2.Pen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(125)))), ((int)(((byte)(22)))));
            this.horizBar2.Title = "horizBar2";
            // 
            // 
            // 
            this.horizBar2.XValues.DataMember = "X";
            // 
            // 
            // 
            this.horizBar2.YValues.DataMember = "Bar";
            this.horizBar2.YValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // uiDataGridView1
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.uiDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.uiDataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.uiDataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.uiDataGridView1.ColumnHeadersHeight = 32;
            this.uiDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.uiDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_FailItem,
            this.Column_LogNumber,
            this.Column_Percent});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.uiDataGridView1.DefaultCellStyle = dataGridViewCellStyle8;
            this.uiDataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiDataGridView1.EnableHeadersVisualStyles = false;
            this.uiDataGridView1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiDataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.Location = new System.Drawing.Point(0, 315);
            this.uiDataGridView1.Name = "uiDataGridView1";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.uiDataGridView1.RowTemplate.Height = 24;
            this.uiDataGridView1.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.SelectedIndex = -1;
            this.uiDataGridView1.ShowCellToolTips = false;
            this.uiDataGridView1.Size = new System.Drawing.Size(793, 298);
            this.uiDataGridView1.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.Style = Sunny.UI.UIStyle.Custom;
            this.uiDataGridView1.TabIndex = 6;
            this.uiDataGridView1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // Column_FailItem
            // 
            this.Column_FailItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column_FailItem.HeaderText = "不良項目";
            this.Column_FailItem.Name = "Column_FailItem";
            this.Column_FailItem.Width = 98;
            // 
            // Column_LogNumber
            // 
            this.Column_LogNumber.HeaderText = "Log數量";
            this.Column_LogNumber.Name = "Column_LogNumber";
            // 
            // Column_Percent
            // 
            this.Column_Percent.HeaderText = "比重";
            this.Column_Percent.Name = "Column_Percent";
            // 
            // tabPage_Items
            // 
            this.tabPage_Items.Controls.Add(this.tChart_Pie);
            this.tabPage_Items.Location = new System.Drawing.Point(201, 0);
            this.tabPage_Items.Name = "tabPage_Items";
            this.tabPage_Items.Size = new System.Drawing.Size(793, 613);
            this.tabPage_Items.TabIndex = 0;
            this.tabPage_Items.Text = "Fail 項目百分比";
            this.tabPage_Items.UseVisualStyleBackColor = true;
            // 
            // marksTip1
            // 
            this.marksTip1.Series = this.horizBar2;
            this.marksTip1.Style = Steema.TeeChart.Styles.MarksStyles.Value;
            // 
            // SensorTest_FailCountStatistics
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(994, 613);
            this.Controls.Add(this.uiTabControlMenu1);
            this.Name = "SensorTest_FailCountStatistics";
            this.Text = "SensorTest_FailCountStatistics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SensorTest_FailCountStatistics_FormClosing);
            this.uiTabControlMenu1.ResumeLayout(false);
            this.tabPage_Files.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).EndInit();
            this.tabPage_Items.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Steema.TeeChart.TChart tChart_Pie;
        private Sunny.UI.UITabControlMenu uiTabControlMenu1;
        private System.Windows.Forms.TabPage tabPage_Items;
        private System.Windows.Forms.TabPage tabPage_Files;
        private Steema.TeeChart.TChart tChart_HorizBar;
        private Steema.TeeChart.Styles.HorizBar horizBar1;
        private Steema.TeeChart.Styles.Pie pie_FailCounts;
        private Steema.TeeChart.Styles.HorizBar horizBar2;
        private Sunny.UI.UIDataGridView uiDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_FailItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_LogNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Percent;
        private Steema.TeeChart.Tools.MarksTip marksTip1;

    }
}