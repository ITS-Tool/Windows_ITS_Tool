namespace ITS_Studio.AllForms
{
    partial class WarningForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btn_OK = new System.Windows.Forms.Button();
            this.timer_Blink = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.statusStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btn_OK);
            this.splitContainer1.Panel2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitContainer1.Size = new System.Drawing.Size(800, 219);
            this.splitContainer1.SplitterDistance = 166;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoEllipsis = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Consolas", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.LightSkyBlue;
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(794, 142);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.UseMnemonic = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.toolStripStatusLabel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.Image = global::ITS_Studio.Properties.Resources.Warning;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(785, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            // 
            // btn_OK
            // 
            this.btn_OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.btn_OK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_OK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OK.Image = global::ITS_Studio.Properties.Resources.green_ok_icon;
            this.btn_OK.Location = new System.Drawing.Point(0, 0);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(800, 52);
            this.btn_OK.TabIndex = 0;
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // timer_Blink
            // 
            this.timer_Blink.Tick += new System.EventHandler(this.timer_Blink_Tick);
            // 
            // WarningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.btn_OK;
            this.ClientSize = new System.Drawing.Size(800, 219);
            this.Controls.Add(this.splitContainer1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WarningForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WarningForm";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Load += new System.EventHandler(this.WarningForm_Load);
            this.Shown += new System.EventHandler(this.WarningForm_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Timer timer_Blink;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}