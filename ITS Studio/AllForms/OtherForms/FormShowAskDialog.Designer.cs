namespace ITS_Studio
{
    partial class FormShowAskDialog
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
            this.uiTitlePanel1 = new Sunny.UI.UITitlePanel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.uiButton2 = new Sunny.UI.UIButton();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.uiTitlePanel1.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTitlePanel1
            // 
            this.uiTitlePanel1.AutoSize = true;
            this.uiTitlePanel1.Controls.Add(this.uiLabel1);
            this.uiTitlePanel1.Controls.Add(this.uiPanel1);
            this.uiTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTitlePanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiTitlePanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiTitlePanel1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.uiTitlePanel1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.uiTitlePanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel1.Name = "uiTitlePanel1";
            this.uiTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 45, 0, 0);
            this.uiTitlePanel1.Radius = 10;
            this.uiTitlePanel1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTitlePanel1.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.uiTitlePanel1.ShowText = false;
            this.uiTitlePanel1.Size = new System.Drawing.Size(391, 269);
            this.uiTitlePanel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiTitlePanel1.TabIndex = 0;
            this.uiTitlePanel1.Text = "Tips";
            this.uiTitlePanel1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.uiTitlePanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiTitlePanel1.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiTitlePanel1.TitleHeight = 45;
            this.uiTitlePanel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLabel1
            // 
            this.uiLabel1.BackColor = System.Drawing.Color.Transparent;
            this.uiLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel1.Location = new System.Drawing.Point(0, 45);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(391, 173);
            this.uiLabel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiLabel1.TabIndex = 3;
            this.uiLabel1.Text = "uiLabel1";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.uiButton2);
            this.uiPanel1.Controls.Add(this.uiButton1);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiPanel1.Location = new System.Drawing.Point(0, 218);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(391, 51);
            this.uiPanel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiPanel1.TabIndex = 2;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiPanel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiButton2
            // 
            this.uiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiButton2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiButton2.LightStyle = true;
            this.uiButton2.Location = new System.Drawing.Point(206, 0);
            this.uiButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Radius = 10;
            this.uiButton2.Size = new System.Drawing.Size(185, 51);
            this.uiButton2.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton2.TabIndex = 1;
            this.uiButton2.Text = "No";
            this.uiButton2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiButton2.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiButton1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiButton1.LightStyle = true;
            this.uiButton1.Location = new System.Drawing.Point(0, 0);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Radius = 10;
            this.uiButton1.Size = new System.Drawing.Size(206, 51);
            this.uiButton1.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton1.TabIndex = 0;
            this.uiButton1.Text = "Yes";
            this.uiButton1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // FormShowAskDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 269);
            this.Controls.Add(this.uiTitlePanel1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormShowAskDialog";
            this.Text = "FormShowAskDialog";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.uiTitlePanel1.ResumeLayout(false);
            this.uiPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunny.UI.UITitlePanel uiTitlePanel1;
        private Sunny.UI.UIButton uiButton2;
        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UILabel uiLabel1;
    }
}