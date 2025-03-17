namespace ITS_Studio
{
    partial class NoiseFrequencyFormulaTransform
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.uiSplitContainer1 = new Sunny.UI.UISplitContainer();
            this.uiDataGridView1 = new Sunny.UI.UIDataGridView();
            this.ColumnCharge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDump = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnToFreq = new Sunny.UI.UIButton();
            this.btnToPostIdle = new Sunny.UI.UIButton();
            this.uiLabel1 = new Sunny.UI.UILabel();
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainer1)).BeginInit();
            this.uiSplitContainer1.Panel1.SuspendLayout();
            this.uiSplitContainer1.Panel2.SuspendLayout();
            this.uiSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // uiSplitContainer1
            // 
            this.uiSplitContainer1.CollapsePanel = Sunny.UI.UISplitContainer.UICollapsePanel.None;
            this.uiSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.uiSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.uiSplitContainer1.MinimumSize = new System.Drawing.Size(20, 20);
            this.uiSplitContainer1.Name = "uiSplitContainer1";
            this.uiSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // uiSplitContainer1.Panel1
            // 
            this.uiSplitContainer1.Panel1.Controls.Add(this.uiDataGridView1);
            // 
            // uiSplitContainer1.Panel2
            // 
            this.uiSplitContainer1.Panel2.Controls.Add(this.btnToFreq);
            this.uiSplitContainer1.Panel2.Controls.Add(this.btnToPostIdle);
            this.uiSplitContainer1.Panel2.Controls.Add(this.uiLabel1);
            this.uiSplitContainer1.Size = new System.Drawing.Size(877, 261);
            this.uiSplitContainer1.SplitterDistance = 79;
            this.uiSplitContainer1.SplitterWidth = 1;
            this.uiSplitContainer1.TabIndex = 2;
            this.uiSplitContainer1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiDataGridView1
            // 
            this.uiDataGridView1.AllowUserToAddRows = false;
            this.uiDataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(251)))), ((int)(((byte)(250)))));
            this.uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.uiDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.uiDataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(251)))), ((int)(((byte)(250)))));
            this.uiDataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(190)))), ((int)(((byte)(172)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(190)))), ((int)(((byte)(172)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.uiDataGridView1.ColumnHeadersHeight = 32;
            this.uiDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.uiDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCharge,
            this.ColumnDump});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(242)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.uiDataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.uiDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiDataGridView1.EnableHeadersVisualStyles = false;
            this.uiDataGridView1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiDataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(199)))), ((int)(((byte)(183)))));
            this.uiDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.uiDataGridView1.Name = "uiDataGridView1";
            this.uiDataGridView1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(190)))), ((int)(((byte)(172)))));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(251)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(190)))), ((int)(((byte)(172)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(242)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.uiDataGridView1.RowTemplate.Height = 24;
            this.uiDataGridView1.ScrollBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(251)))), ((int)(((byte)(250)))));
            this.uiDataGridView1.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(190)))), ((int)(((byte)(172)))));
            this.uiDataGridView1.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(190)))), ((int)(((byte)(172)))));
            this.uiDataGridView1.SelectedIndex = -1;
            this.uiDataGridView1.Size = new System.Drawing.Size(877, 79);
            this.uiDataGridView1.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(251)))), ((int)(((byte)(250)))));
            this.uiDataGridView1.Style = Sunny.UI.UIStyle.Colorful;
            this.uiDataGridView1.TabIndex = 2;
            this.uiDataGridView1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // ColumnCharge
            // 
            this.ColumnCharge.HeaderText = "Charge";
            this.ColumnCharge.Name = "ColumnCharge";
            // 
            // ColumnDump
            // 
            this.ColumnDump.HeaderText = "Dump";
            this.ColumnDump.Name = "ColumnDump";
            // 
            // btnToFreq
            // 
            this.btnToFreq.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToFreq.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnToFreq.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.btnToFreq.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.btnToFreq.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.btnToFreq.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.btnToFreq.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.btnToFreq.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnToFreq.Location = new System.Drawing.Point(762, 111);
            this.btnToFreq.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnToFreq.Name = "btnToFreq";
            this.btnToFreq.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(190)))), ((int)(((byte)(40)))));
            this.btnToFreq.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(203)))), ((int)(((byte)(83)))));
            this.btnToFreq.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.btnToFreq.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(152)))), ((int)(((byte)(32)))));
            this.btnToFreq.Size = new System.Drawing.Size(115, 35);
            this.btnToFreq.Style = Sunny.UI.UIStyle.Green;
            this.btnToFreq.StyleCustomMode = true;
            this.btnToFreq.TabIndex = 2;
            this.btnToFreq.Text = "Frequency";
            this.btnToFreq.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btnToFreq.Click += new System.EventHandler(this.btnToFreq_Click);
            // 
            // btnToPostIdle
            // 
            this.btnToPostIdle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToPostIdle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnToPostIdle.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnToPostIdle.Location = new System.Drawing.Point(762, 146);
            this.btnToPostIdle.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnToPostIdle.Name = "btnToPostIdle";
            this.btnToPostIdle.Size = new System.Drawing.Size(115, 35);
            this.btnToPostIdle.TabIndex = 1;
            this.btnToPostIdle.Text = "Post-Idle";
            this.btnToPostIdle.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btnToPostIdle.Click += new System.EventHandler(this.btnToPostIdle_Click);
            // 
            // uiLabel1
            // 
            this.uiLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel1.Location = new System.Drawing.Point(0, 0);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(762, 181);
            this.uiLabel1.TabIndex = 0;
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // NoiseFrequencyFormulaTransform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 261);
            this.Controls.Add(this.uiSplitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NoiseFrequencyFormulaTransform";
            this.Text = "NoiseFrequencyFormulaTransform";
            this.Load += new System.EventHandler(this.NoiseFrequencyFormulaTransform_Load);
            this.uiSplitContainer1.Panel1.ResumeLayout(false);
            this.uiSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainer1)).EndInit();
            this.uiSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UISplitContainer uiSplitContainer1;
        private Sunny.UI.UIDataGridView uiDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCharge;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDump;
        private Sunny.UI.UIButton btnToPostIdle;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIButton btnToFreq;

    }
}