namespace ITS_Studio.UserControls
{
    partial class UserControl_LabelTextBox
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
            if(disposing && (components != null))
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
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.uiLedLabel1 = new Sunny.UI.UILedLabel();
            this.uiPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiLabel1
            // 
            this.uiLabel1.AutoEllipsis = true;
            this.uiLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiLabel1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiLabel1.Location = new System.Drawing.Point(0, 0);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(56, 23);
            this.uiLabel1.Style = Sunny.UI.UIStyle.Orange;
            this.uiLabel1.TabIndex = 0;
            this.uiLabel1.Text = "uiLabel1";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(56, 0);
            this.splitter1.MinExtra = 1;
            this.splitter1.MinSize = 1;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1, 23);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.uiLedLabel1);
            this.uiPanel1.Controls.Add(this.splitter1);
            this.uiPanel1.Controls.Add(this.uiLabel1);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiPanel1.Size = new System.Drawing.Size(282, 23);
            this.uiPanel1.Style = Sunny.UI.UIStyle.Orange;
            this.uiPanel1.TabIndex = 3;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiPanel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLedLabel1
            // 
            this.uiLedLabel1.BackColor = System.Drawing.Color.Transparent;
            this.uiLedLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLedLabel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLedLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiLedLabel1.Location = new System.Drawing.Point(57, 0);
            this.uiLedLabel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiLedLabel1.Name = "uiLedLabel1";
            this.uiLedLabel1.Size = new System.Drawing.Size(225, 23);
            this.uiLedLabel1.Style = Sunny.UI.UIStyle.Orange;
            this.uiLedLabel1.StyleCustomMode = true;
            this.uiLedLabel1.TabIndex = 3;
            this.uiLedLabel1.Text = "0";
            this.uiLedLabel1.UseDoubleClick = true;
            this.uiLedLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // UserControl_LabelTextBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.uiPanel1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "UserControl_LabelTextBox";
            this.Size = new System.Drawing.Size(282, 23);
            this.uiPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Splitter splitter1;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UILedLabel uiLedLabel1;

    }
}
