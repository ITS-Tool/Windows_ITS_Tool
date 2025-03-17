namespace ITS_Studio
{
    partial class SensorTest_FPC_Open
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Status_Timer = new System.Windows.Forms.ToolStripStatusLabel();
            this.Status_ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tChart1 = new Steema.TeeChart.TChart();
            this.bar1 = new Steema.TeeChart.Styles.Bar();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.AllowMerge = false;
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status_Timer,
            this.Status_ProgressBar});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 446);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(654, 46);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Status_Timer
            // 
            this.Status_Timer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Status_Timer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Status_Timer.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status_Timer.ForeColor = System.Drawing.Color.DarkGreen;
            this.Status_Timer.Name = "Status_Timer";
            this.Status_Timer.Size = new System.Drawing.Size(330, 41);
            this.Status_Timer.Text = "toolStripStatusLabel1";
            this.Status_Timer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Status_ProgressBar
            // 
            this.Status_ProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Status_ProgressBar.Name = "Status_ProgressBar";
            this.Status_ProgressBar.Size = new System.Drawing.Size(200, 40);
            this.Status_ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // tChart1
            // 
            // 
            // 
            // 
            this.tChart1.Aspect.ColorPaletteIndex = 16;
            this.tChart1.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.tChart1.Aspect.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            this.tChart1.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Automatic = false;
            this.tChart1.Axes.Bottom.AutomaticMaximum = false;
            this.tChart1.Axes.Bottom.AutomaticMinimum = false;
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.tChart1.Axes.Bottom.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
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
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Axes.Bottom.Maximum = 5D;
            this.tChart1.Axes.Bottom.Minimum = 0D;
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.MinorTicks.Visible = false;
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Ticks.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.TicksInner.Visible = true;
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
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Depth.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            // 
            // 
            // 
            this.tChart1.Axes.Depth.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.tChart1.Axes.Depth.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
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
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Depth.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Axes.Depth.MinorTicks.Visible = false;
            // 
            // 
            // 
            this.tChart1.Axes.Depth.Ticks.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            // 
            // 
            // 
            this.tChart1.Axes.Depth.TicksInner.Visible = true;
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
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.DepthTop.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            // 
            // 
            // 
            this.tChart1.Axes.DepthTop.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.tChart1.Axes.DepthTop.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
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
            this.tChart1.Axes.DepthTop.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Axes.DepthTop.MinorTicks.Visible = false;
            // 
            // 
            // 
            this.tChart1.Axes.DepthTop.Ticks.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            // 
            // 
            // 
            this.tChart1.Axes.DepthTop.TicksInner.Visible = true;
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
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Left.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            // 
            // 
            // 
            this.tChart1.Axes.Left.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.tChart1.Axes.Left.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
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
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Left.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Axes.Left.MinorTicks.Visible = false;
            // 
            // 
            // 
            this.tChart1.Axes.Left.Ticks.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            // 
            // 
            // 
            this.tChart1.Axes.Left.TicksInner.Visible = true;
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
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Right.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            // 
            // 
            // 
            this.tChart1.Axes.Right.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.tChart1.Axes.Right.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
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
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Right.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Axes.Right.MinorTicks.Visible = false;
            // 
            // 
            // 
            this.tChart1.Axes.Right.Ticks.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            // 
            // 
            // 
            this.tChart1.Axes.Right.TicksInner.Visible = true;
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
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Top.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            // 
            // 
            // 
            this.tChart1.Axes.Top.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.tChart1.Axes.Top.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
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
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Top.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Axes.Top.MinorTicks.Visible = false;
            // 
            // 
            // 
            this.tChart1.Axes.Top.Ticks.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            // 
            // 
            // 
            this.tChart1.Axes.Top.TicksInner.Visible = true;
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
            // 
            // 
            // 
            this.tChart1.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.tChart1.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart1.Header.Bevel.StringColorOne = "FFFFFF00";
            this.tChart1.Header.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            this.tChart1.Header.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            // 
            // 
            // 
            this.tChart1.Header.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.tChart1.Header.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tChart1.Header.Brush.Gradient.SigmaFocus = 0F;
            this.tChart1.Header.Brush.Gradient.SigmaScale = 0F;
            this.tChart1.Header.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChart1.Header.Brush.Gradient.UseMiddle = true;
            // 
            // 
            // 
            this.tChart1.Header.Font.Bold = true;
            // 
            // 
            // 
            this.tChart1.Header.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tChart1.Header.Font.Name = "Consolas";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Header.Font.Shadow.Brush.Visible = false;
            this.tChart1.Header.Font.Shadow.Smooth = true;
            this.tChart1.Header.Font.Shadow.SmoothBlur = -9;
            this.tChart1.Header.Font.Size = 20;
            this.tChart1.Header.Font.SizeFloat = 20F;
            // 
            // 
            // 
            this.tChart1.Header.Shadow.Height = 0;
            this.tChart1.Header.Shadow.Width = 0;
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
            this.tChart1.Legend.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.tChart1.Legend.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tChart1.Legend.Brush.Gradient.SigmaFocus = 0F;
            this.tChart1.Legend.Brush.Gradient.SigmaScale = 0F;
            this.tChart1.Legend.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tChart1.Legend.Brush.Gradient.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Legend.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Legend.Pen.Visible = false;
            // 
            // 
            // 
            this.tChart1.Legend.Shadow.Width = 0;
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
            this.tChart1.Location = new System.Drawing.Point(0, 0);
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
            this.tChart1.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // 
            // 
            this.tChart1.Panel.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tChart1.Panel.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tChart1.Panel.Brush.Gradient.SigmaFocus = 0F;
            this.tChart1.Panel.Brush.Gradient.SigmaScale = 0F;
            this.tChart1.Panel.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tChart1.Panel.Brush.Gradient.UseMiddle = false;
            this.tChart1.Panel.Brush.Gradient.Visible = false;
            // 
            // 
            // 
            this.tChart1.Panel.Shadow.Height = 0;
            this.tChart1.Panel.Shadow.Width = 0;
            this.tChart1.Series.Add(this.bar1);
            this.tChart1.Size = new System.Drawing.Size(654, 446);
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
            this.tChart1.Walls.Back.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.tChart1.Walls.Back.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tChart1.Walls.Back.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            // 
            // 
            // 
            this.tChart1.Walls.Back.Pen.Visible = false;
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
            this.tChart1.Walls.Bottom.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.tChart1.Walls.Bottom.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tChart1.Walls.Bottom.Brush.Gradient.SigmaFocus = 0F;
            this.tChart1.Walls.Bottom.Brush.Gradient.SigmaScale = 0F;
            this.tChart1.Walls.Bottom.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Walls.Bottom.Pen.Visible = false;
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
            // 
            // 
            // 
            this.tChart1.Walls.Left.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.tChart1.Walls.Left.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tChart1.Walls.Left.Brush.Gradient.SigmaFocus = 0F;
            this.tChart1.Walls.Left.Brush.Gradient.SigmaScale = 0F;
            this.tChart1.Walls.Left.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Walls.Left.Pen.Visible = false;
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
            // 
            // 
            // 
            this.tChart1.Walls.Right.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            // 
            // 
            // 
            this.tChart1.Walls.Right.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.tChart1.Walls.Right.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tChart1.Walls.Right.Brush.Gradient.SigmaFocus = 0F;
            this.tChart1.Walls.Right.Brush.Gradient.SigmaScale = 0F;
            this.tChart1.Walls.Right.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Walls.Right.Pen.Visible = false;
            this.tChart1.Walls.Visible = false;
            // 
            // 
            // 
            this.tChart1.Zoom.Allow = false;
            this.tChart1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tChart1_KeyDown);
            // 
            // bar1
            // 
            // 
            // 
            // 
            this.bar1.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bar1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bar1.ColorEach = false;
            this.bar1.Dark3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.bar1.Marks.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bar1.Marks.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.bar1.Marks.Bevel.StringColorOne = "FFFFFFFF";
            this.bar1.Marks.Bevel.StringColorTwo = "FF808080";
            // 
            // 
            // 
            // 
            // 
            // 
            this.bar1.Marks.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.bar1.Marks.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bar1.Marks.Brush.Gradient.SigmaFocus = 0F;
            this.bar1.Marks.Brush.Gradient.SigmaScale = 0F;
            this.bar1.Marks.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bar1.Marks.Brush.Visible = false;
            this.bar1.Marks.Clip = true;
            // 
            // 
            // 
            this.bar1.Marks.Font.Bold = true;
            // 
            // 
            // 
            this.bar1.Marks.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bar1.Marks.Font.Name = "Consolas";
            this.bar1.Marks.Font.Size = 12;
            this.bar1.Marks.Font.SizeFloat = 12F;
            this.bar1.Marks.FontSeriesColor = true;
            this.bar1.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Value;
            // 
            // 
            // 
            // 
            // 
            // 
            this.bar1.Marks.Symbol.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bar1.Marks.Symbol.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.bar1.Marks.Symbol.Bevel.StringColorOne = "FFFFFFFF";
            this.bar1.Marks.Symbol.Bevel.StringColorTwo = "FF808080";
            this.bar1.Marks.Transparent = true;
            this.bar1.Marks.Visible = false;
            // 
            // 
            // 
            this.bar1.Pen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(138)))), ((int)(((byte)(54)))));
            this.bar1.Title = "bar1";
            // 
            // 
            // 
            this.bar1.XValues.DataMember = "X";
            this.bar1.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.bar1.YValues.DataMember = "Bar";
            // 
            // SensorTest_FPC_Open
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(654, 492);
            this.Controls.Add(this.tChart1);
            this.Controls.Add(this.statusStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SensorTest_FPC_Open";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SensorTest_FPC_Open";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.SensorTest_FPC_Open_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SensorTest_FPC_Open_FormClosing);
            this.Shown += new System.EventHandler(this.SensorTest_FPC_Open_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Steema.TeeChart.TChart tChart1;
        private Steema.TeeChart.Styles.Bar bar1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel Status_Timer;
        private System.Windows.Forms.ToolStripProgressBar Status_ProgressBar;
    }
}