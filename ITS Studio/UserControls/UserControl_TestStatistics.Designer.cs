namespace ITS_Studio.ControlBox
{
    partial class UserControl_TestStatistics
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel_Left = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel_Right = new System.Windows.Forms.TableLayoutPanel();
            this.textBox_Pass = new System.Windows.Forms.Label();
            this.textBox_Fail = new System.Windows.Forms.Label();
            this.textBox_Total = new System.Windows.Forms.Label();
            this.textBox_Percent = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel_Left.SuspendLayout();
            this.tableLayoutPanel_Right.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.GreenYellow;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "PASS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "TOTAL";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel_Left);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel_Right);
            this.splitContainer1.Size = new System.Drawing.Size(167, 119);
            this.splitContainer1.SplitterDistance = 63;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 8;
            // 
            // tableLayoutPanel_Left
            // 
            this.tableLayoutPanel_Left.AutoSize = false;
            this.tableLayoutPanel_Left.ColumnCount = 1;
            this.tableLayoutPanel_Left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_Left.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel_Left.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel_Left.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel_Left.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel_Left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_Left.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_Left.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel_Left.Name = "tableLayoutPanel_Left";
            this.tableLayoutPanel_Left.RowCount = 4;
            this.tableLayoutPanel_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_Left.Size = new System.Drawing.Size(63, 119);
            this.tableLayoutPanel_Left.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.MediumPurple;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(3, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 32);
            this.label4.TabIndex = 6;
            this.label4.Text = "YIELD";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Red;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(3, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "FAIL";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel_Right
            // 
            this.tableLayoutPanel_Right.ColumnCount = 1;
            this.tableLayoutPanel_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_Right.Controls.Add(this.textBox_Percent, 0, 3);
            this.tableLayoutPanel_Right.Controls.Add(this.textBox_Total, 0, 2);
            this.tableLayoutPanel_Right.Controls.Add(this.textBox_Fail, 0, 1);
            this.tableLayoutPanel_Right.Controls.Add(this.textBox_Pass, 0, 0);
            this.tableLayoutPanel_Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_Right.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_Right.Name = "tableLayoutPanel_Right";
            this.tableLayoutPanel_Right.RowCount = 4;
            this.tableLayoutPanel_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_Right.Size = new System.Drawing.Size(103, 119);
            this.tableLayoutPanel_Right.TabIndex = 9;
            // 
            // textBox_Pass
            // 
            this.textBox_Pass.AutoEllipsis = true;
            this.textBox_Pass.AutoSize = false;
            this.textBox_Pass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Pass.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_Pass.Location = new System.Drawing.Point(3, 0);
            this.textBox_Pass.Name = "textBox_Pass";
            this.textBox_Pass.Size = new System.Drawing.Size(97, 29);
            this.textBox_Pass.TabIndex = 9;
            this.textBox_Pass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_Fail
            // 
            this.textBox_Fail.AutoEllipsis = true;
            this.textBox_Fail.AutoSize = false;
            this.textBox_Fail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Fail.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_Fail.Location = new System.Drawing.Point(3, 29);
            this.textBox_Fail.Name = "textBox_Fail";
            this.textBox_Fail.Size = new System.Drawing.Size(97, 29);
            this.textBox_Fail.TabIndex = 10;
            this.textBox_Fail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_Total
            // 
            this.textBox_Total.AutoEllipsis = true;
            this.textBox_Total.AutoSize = false;
            this.textBox_Total.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Total.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_Total.Location = new System.Drawing.Point(3, 58);
            this.textBox_Total.Name = "textBox_Total";
            this.textBox_Total.Size = new System.Drawing.Size(97, 29);
            this.textBox_Total.TabIndex = 11;
            this.textBox_Total.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_Percent
            // 
            this.textBox_Percent.AutoEllipsis = true;
            this.textBox_Percent.AutoSize = false;
            this.textBox_Percent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Percent.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_Percent.Location = new System.Drawing.Point(3, 87);
            this.textBox_Percent.Name = "textBox_Percent";
            this.textBox_Percent.Size = new System.Drawing.Size(97, 32);
            this.textBox_Percent.TabIndex = 12;
            this.textBox_Percent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserControl_TestStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = false;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UserControl_TestStatistics";
            this.Size = new System.Drawing.Size(167, 119);
            this.Load += new System.EventHandler(this.TestStatistics_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel_Left.ResumeLayout(false);
            this.tableLayoutPanel_Right.ResumeLayout(false);
            this.tableLayoutPanel_Right.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Left;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Right;
        private System.Windows.Forms.Label textBox_Pass;
        private System.Windows.Forms.Label textBox_Percent;
        private System.Windows.Forms.Label textBox_Total;
        private System.Windows.Forms.Label textBox_Fail;
    }
}
