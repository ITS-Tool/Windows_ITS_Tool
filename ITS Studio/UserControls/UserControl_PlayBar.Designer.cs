namespace CSharp_PlayBar
{
    partial class UserControl_PlayBar
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lb_fileName = new Sunny.UI.UILabel();
            this.uiSymbolLabel1 = new Sunny.UI.UISymbolLabel();
            this.uiTrackBar1 = new Sunny.UI.UITrackBar();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.lb_Frame_Number = new Sunny.UI.UILedLabel();
            this.uiLedLabel1 = new Sunny.UI.UILedLabel();
            this.btn_ShowForm = new Sunny.UI.UISymbolButton();
            this.btn_Fast_Forward = new Sunny.UI.UISymbolButton();
            this.btn_Rewind = new Sunny.UI.UISymbolButton();
            this.btn_Next = new Sunny.UI.UISymbolButton();
            this.btn_Prev = new Sunny.UI.UISymbolButton();
            this.btn_Stop = new Sunny.UI.UISymbolButton();
            this.btn_Play = new Sunny.UI.UISymbolButton();
            this.btn_Load_File = new Sunny.UI.UISymbolButton();
            this.uiToolTip1 = new Sunny.UI.UIToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lb_fileName);
            this.splitContainer1.Panel1.Controls.Add(this.uiSymbolLabel1);
            this.splitContainer1.Panel1.Controls.Add(this.uiTrackBar1);
            this.splitContainer1.Panel1.Controls.Add(this.uiPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.splitContainer1.Panel2.Controls.Add(this.uiLedLabel1);
            this.splitContainer1.Panel2.Controls.Add(this.btn_ShowForm);
            this.splitContainer1.Panel2.Controls.Add(this.btn_Fast_Forward);
            this.splitContainer1.Panel2.Controls.Add(this.btn_Rewind);
            this.splitContainer1.Panel2.Controls.Add(this.btn_Next);
            this.splitContainer1.Panel2.Controls.Add(this.btn_Prev);
            this.splitContainer1.Panel2.Controls.Add(this.btn_Stop);
            this.splitContainer1.Panel2.Controls.Add(this.btn_Play);
            this.splitContainer1.Panel2.Controls.Add(this.btn_Load_File);
            this.splitContainer1.Size = new System.Drawing.Size(595, 108);
            this.splitContainer1.SplitterDistance = 75;
            this.splitContainer1.TabIndex = 5;
            // 
            // lb_fileName
            // 
            this.lb_fileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_fileName.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lb_fileName.Location = new System.Drawing.Point(184, 0);
            this.lb_fileName.Name = "lb_fileName";
            this.lb_fileName.Size = new System.Drawing.Size(411, 58);
            this.lb_fileName.TabIndex = 3;
            this.lb_fileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lb_fileName.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiSymbolLabel1
            // 
            this.uiSymbolLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiSymbolLabel1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiSymbolLabel1.Location = new System.Drawing.Point(84, 0);
            this.uiSymbolLabel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSymbolLabel1.Name = "uiSymbolLabel1";
            this.uiSymbolLabel1.Padding = new System.Windows.Forms.Padding(28, 5, 0, 5);
            this.uiSymbolLabel1.Size = new System.Drawing.Size(100, 58);
            this.uiSymbolLabel1.Style = Sunny.UI.UIStyle.Orange;
            this.uiSymbolLabel1.Symbol = 61462;
            this.uiSymbolLabel1.TabIndex = 1;
            this.uiSymbolLabel1.Text = "File Name :";
            this.uiSymbolLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiTrackBar1
            // 
            this.uiTrackBar1.BarSize = 12;
            this.uiTrackBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiTrackBar1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiTrackBar1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiTrackBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.uiTrackBar1.Location = new System.Drawing.Point(84, 58);
            this.uiTrackBar1.MaximumSize = new System.Drawing.Size(0, 17);
            this.uiTrackBar1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTrackBar1.Name = "uiTrackBar1";
            this.uiTrackBar1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.uiTrackBar1.ShowFocusColor = true;
            this.uiTrackBar1.Size = new System.Drawing.Size(511, 17);
            this.uiTrackBar1.Style = Sunny.UI.UIStyle.Gray;
            this.uiTrackBar1.StyleCustomMode = true;
            this.uiTrackBar1.TabIndex = 0;
            this.uiTrackBar1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiTrackBar1.ValueChanged += new System.EventHandler(this.uiTrackBar1_ValueChanged);
            this.uiTrackBar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uiTrackBar1_MouseDown);
            this.uiTrackBar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uiTrackBar1_MouseUp);
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.lb_Frame_Number);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiPanel1.FillColor = System.Drawing.Color.Empty;
            this.uiPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiPanel1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.RadiusSides = Sunny.UI.UICornerRadiusSides.None;
            this.uiPanel1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiPanel1.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.uiPanel1.Size = new System.Drawing.Size(84, 75);
            this.uiPanel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.uiPanel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // lb_Frame_Number
            // 
            this.lb_Frame_Number.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lb_Frame_Number.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lb_Frame_Number.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.lb_Frame_Number.Location = new System.Drawing.Point(0, 38);
            this.lb_Frame_Number.MinimumSize = new System.Drawing.Size(1, 1);
            this.lb_Frame_Number.Name = "lb_Frame_Number";
            this.lb_Frame_Number.Radius = 0;
            this.lb_Frame_Number.Size = new System.Drawing.Size(84, 37);
            this.lb_Frame_Number.Style = Sunny.UI.UIStyle.Orange;
            this.lb_Frame_Number.StyleCustomMode = true;
            this.lb_Frame_Number.TabIndex = 2;
            this.lb_Frame_Number.Text = "0";
            this.lb_Frame_Number.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.lb_Frame_Number.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lb_Frame_Number_KeyDown);
            this.lb_Frame_Number.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lb_Frame_Number_MouseDoubleClick);
            // 
            // uiLedLabel1
            // 
            this.uiLedLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiLedLabel1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiLedLabel1.ForeColor = System.Drawing.Color.Yellow;
            this.uiLedLabel1.IntervalOn = 1;
            this.uiLedLabel1.Location = new System.Drawing.Point(400, 0);
            this.uiLedLabel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiLedLabel1.Name = "uiLedLabel1";
            this.uiLedLabel1.Size = new System.Drawing.Size(195, 29);
            this.uiLedLabel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiLedLabel1.StyleCustomMode = true;
            this.uiLedLabel1.TabIndex = 13;
            this.uiLedLabel1.Text = "Speed : 1";
            this.uiLedLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLedLabel1.ZoomScaleDisabled = true;
            this.uiLedLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // btn_ShowForm
            // 
            this.btn_ShowForm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_ShowForm.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_ShowForm.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_ShowForm.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_ShowForm.FillDisableColor = System.Drawing.Color.Transparent;
            this.btn_ShowForm.FillHoverColor = System.Drawing.Color.LimeGreen;
            this.btn_ShowForm.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_ShowForm.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_ShowForm.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_ShowForm.Location = new System.Drawing.Point(350, 0);
            this.btn_ShowForm.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_ShowForm.Name = "btn_ShowForm";
            this.btn_ShowForm.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_ShowForm.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btn_ShowForm.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_ShowForm.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_ShowForm.Size = new System.Drawing.Size(50, 29);
            this.btn_ShowForm.Style = Sunny.UI.UIStyle.Custom;
            this.btn_ShowForm.Symbol = 62162;
            this.btn_ShowForm.SymbolPressColor = System.Drawing.Color.LightGray;
            this.btn_ShowForm.TabIndex = 16;
            this.btn_ShowForm.TipsFont = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_ShowForm.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_ShowForm.Click += new System.EventHandler(this.btn_ShowForm_Click);
            // 
            // btn_Fast_Forward
            // 
            this.btn_Fast_Forward.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Fast_Forward.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Fast_Forward.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Fast_Forward.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Fast_Forward.FillDisableColor = System.Drawing.Color.Transparent;
            this.btn_Fast_Forward.FillHoverColor = System.Drawing.Color.LimeGreen;
            this.btn_Fast_Forward.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Fast_Forward.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Fast_Forward.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_Fast_Forward.Location = new System.Drawing.Point(300, 0);
            this.btn_Fast_Forward.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_Fast_Forward.Name = "btn_Fast_Forward";
            this.btn_Fast_Forward.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Fast_Forward.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btn_Fast_Forward.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Fast_Forward.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Fast_Forward.Size = new System.Drawing.Size(50, 29);
            this.btn_Fast_Forward.Style = Sunny.UI.UIStyle.Custom;
            this.btn_Fast_Forward.Symbol = 61543;
            this.btn_Fast_Forward.SymbolPressColor = System.Drawing.Color.LightGray;
            this.btn_Fast_Forward.TabIndex = 12;
            this.btn_Fast_Forward.TipsFont = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Fast_Forward.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_Fast_Forward.Click += new System.EventHandler(this.btn_Fast_Forward_Click);
            // 
            // btn_Rewind
            // 
            this.btn_Rewind.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Rewind.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Rewind.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Rewind.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Rewind.FillDisableColor = System.Drawing.Color.Transparent;
            this.btn_Rewind.FillHoverColor = System.Drawing.Color.LimeGreen;
            this.btn_Rewind.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Rewind.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Rewind.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_Rewind.Location = new System.Drawing.Point(250, 0);
            this.btn_Rewind.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_Rewind.Name = "btn_Rewind";
            this.btn_Rewind.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Rewind.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btn_Rewind.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Rewind.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Rewind.Size = new System.Drawing.Size(50, 29);
            this.btn_Rewind.Style = Sunny.UI.UIStyle.Custom;
            this.btn_Rewind.Symbol = 61544;
            this.btn_Rewind.TabIndex = 11;
            this.btn_Rewind.TipsFont = new System.Drawing.Font("微軟正黑體", 9F);
            this.btn_Rewind.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_Rewind.Click += new System.EventHandler(this.btn_Rewind_Click);
            // 
            // btn_Next
            // 
            this.btn_Next.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Next.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Next.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Next.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Next.FillDisableColor = System.Drawing.Color.Transparent;
            this.btn_Next.FillHoverColor = System.Drawing.Color.LimeGreen;
            this.btn_Next.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Next.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Next.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_Next.Location = new System.Drawing.Point(200, 0);
            this.btn_Next.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Next.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btn_Next.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Next.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Next.Size = new System.Drawing.Size(50, 29);
            this.btn_Next.Style = Sunny.UI.UIStyle.Custom;
            this.btn_Next.Symbol = 61518;
            this.btn_Next.TabIndex = 15;
            this.btn_Next.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_Next.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Next_MouseDown);
            this.btn_Next.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            // 
            // btn_Prev
            // 
            this.btn_Prev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Prev.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Prev.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Prev.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Prev.FillDisableColor = System.Drawing.Color.Transparent;
            this.btn_Prev.FillHoverColor = System.Drawing.Color.LimeGreen;
            this.btn_Prev.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Prev.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Prev.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_Prev.Location = new System.Drawing.Point(150, 0);
            this.btn_Prev.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_Prev.Name = "btn_Prev";
            this.btn_Prev.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Prev.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btn_Prev.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Prev.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Prev.Size = new System.Drawing.Size(50, 29);
            this.btn_Prev.Style = Sunny.UI.UIStyle.Custom;
            this.btn_Prev.Symbol = 61514;
            this.btn_Prev.TabIndex = 14;
            this.btn_Prev.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_Prev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Prev_MouseDown);
            this.btn_Prev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            // 
            // btn_Stop
            // 
            this.btn_Stop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Stop.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Stop.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Stop.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Stop.FillDisableColor = System.Drawing.Color.Transparent;
            this.btn_Stop.FillHoverColor = System.Drawing.Color.LimeGreen;
            this.btn_Stop.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Stop.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Stop.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_Stop.Location = new System.Drawing.Point(100, 0);
            this.btn_Stop.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Stop.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btn_Stop.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Stop.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Stop.Size = new System.Drawing.Size(50, 29);
            this.btn_Stop.Style = Sunny.UI.UIStyle.Custom;
            this.btn_Stop.Symbol = 61517;
            this.btn_Stop.TabIndex = 10;
            this.btn_Stop.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // btn_Play
            // 
            this.btn_Play.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Play.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Play.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Play.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Play.FillDisableColor = System.Drawing.Color.Transparent;
            this.btn_Play.FillHoverColor = System.Drawing.Color.LimeGreen;
            this.btn_Play.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Play.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Play.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_Play.Location = new System.Drawing.Point(50, 0);
            this.btn_Play.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_Play.Name = "btn_Play";
            this.btn_Play.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Play.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btn_Play.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Play.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Play.Size = new System.Drawing.Size(50, 29);
            this.btn_Play.Style = Sunny.UI.UIStyle.Custom;
            this.btn_Play.Symbol = 61515;
            this.btn_Play.TabIndex = 9;
            this.btn_Play.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_Play.Click += new System.EventHandler(this.btn_Play_Click);
            // 
            // btn_Load_File
            // 
            this.btn_Load_File.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Load_File.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Load_File.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Load_File.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Load_File.FillDisableColor = System.Drawing.Color.Transparent;
            this.btn_Load_File.FillHoverColor = System.Drawing.Color.LimeGreen;
            this.btn_Load_File.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Load_File.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Load_File.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_Load_File.Location = new System.Drawing.Point(0, 0);
            this.btn_Load_File.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_Load_File.Name = "btn_Load_File";
            this.btn_Load_File.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btn_Load_File.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btn_Load_File.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Load_File.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btn_Load_File.Size = new System.Drawing.Size(50, 29);
            this.btn_Load_File.Style = Sunny.UI.UIStyle.Custom;
            this.btn_Load_File.Symbol = 61522;
            this.btn_Load_File.TabIndex = 8;
            this.btn_Load_File.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_Load_File.Click += new System.EventHandler(this.btn_Load_File_Click);
            // 
            // uiToolTip1
            // 
            this.uiToolTip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.uiToolTip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.uiToolTip1.OwnerDraw = true;
            // 
            // UserControl_PlayBar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "UserControl_PlayBar";
            this.Size = new System.Drawing.Size(595, 108);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.uiPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Sunny.UI.UISymbolLabel uiSymbolLabel1;
        private Sunny.UI.UILedLabel lb_Frame_Number;
        private Sunny.UI.UILabel lb_fileName;
        private Sunny.UI.UISymbolButton btn_Load_File;
        private Sunny.UI.UISymbolButton btn_Play;
        private Sunny.UI.UISymbolButton btn_Fast_Forward;
        private Sunny.UI.UISymbolButton btn_Rewind;
        private Sunny.UI.UISymbolButton btn_Stop;
        private Sunny.UI.UILedLabel uiLedLabel1;
        protected Sunny.UI.UITrackBar uiTrackBar1;
        private Sunny.UI.UISymbolButton btn_Next;
        private Sunny.UI.UISymbolButton btn_Prev;
        private Sunny.UI.UISymbolButton btn_ShowForm;
        private Sunny.UI.UIToolTip uiToolTip1;
        private Sunny.UI.UIPanel uiPanel1;
    }
}
