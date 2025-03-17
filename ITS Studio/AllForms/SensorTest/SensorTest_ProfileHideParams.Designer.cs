namespace ITS_Studio.AllForms.SensorTest
{
    partial class SensorTest_ProfileHideParams
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SensorTest_ProfileHideParams));
            this.uiCheckBoxGroup1 = new Sunny.UI.UICheckBoxGroup();
            this.btn_SelectAll = new Sunny.UI.UIButton();
            this.btn_UnSelectAll = new Sunny.UI.UIButton();
            this.btn_Conform = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // uiCheckBoxGroup1
            // 
            this.uiCheckBoxGroup1.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiCheckBoxGroup1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiCheckBoxGroup1.Location = new System.Drawing.Point(0, 0);
            this.uiCheckBoxGroup1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiCheckBoxGroup1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiCheckBoxGroup1.Name = "uiCheckBoxGroup1";
            this.uiCheckBoxGroup1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiCheckBoxGroup1.SelectedIndexes = ((System.Collections.Generic.List<int>)(resources.GetObject("uiCheckBoxGroup1.SelectedIndexes")));
            this.uiCheckBoxGroup1.Size = new System.Drawing.Size(270, 370);
            this.uiCheckBoxGroup1.TabIndex = 0;
            this.uiCheckBoxGroup1.Text = "(加入 / 刪除) 隱藏的測項";
            this.uiCheckBoxGroup1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiCheckBoxGroup1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiCheckBoxGroup1.ValueChanged += new Sunny.UI.UICheckBoxGroup.OnValueChanged(this.uiCheckBoxGroup1_ValueChanged);
            // 
            // btn_SelectAll
            // 
            this.btn_SelectAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_SelectAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_SelectAll.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_SelectAll.LightStyle = true;
            this.btn_SelectAll.Location = new System.Drawing.Point(270, 0);
            this.btn_SelectAll.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_SelectAll.Name = "btn_SelectAll";
            this.btn_SelectAll.Radius = 35;
            this.btn_SelectAll.Size = new System.Drawing.Size(114, 35);
            this.btn_SelectAll.TabIndex = 1;
            this.btn_SelectAll.Text = "全選";
            this.btn_SelectAll.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_SelectAll.Click += new System.EventHandler(this.btn_SelectAll_Click);
            // 
            // btn_UnSelectAll
            // 
            this.btn_UnSelectAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_UnSelectAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_UnSelectAll.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_UnSelectAll.LightStyle = true;
            this.btn_UnSelectAll.Location = new System.Drawing.Point(270, 35);
            this.btn_UnSelectAll.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_UnSelectAll.Name = "btn_UnSelectAll";
            this.btn_UnSelectAll.Radius = 35;
            this.btn_UnSelectAll.Size = new System.Drawing.Size(114, 35);
            this.btn_UnSelectAll.TabIndex = 2;
            this.btn_UnSelectAll.Text = "全不選";
            this.btn_UnSelectAll.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_UnSelectAll.Click += new System.EventHandler(this.btn_UnSelectAll_Click);
            // 
            // btn_Conform
            // 
            this.btn_Conform.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Conform.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_Conform.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_Conform.Location = new System.Drawing.Point(270, 335);
            this.btn_Conform.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_Conform.Name = "btn_Conform";
            this.btn_Conform.Size = new System.Drawing.Size(114, 35);
            this.btn_Conform.TabIndex = 3;
            this.btn_Conform.Text = "存檔離開";
            this.btn_Conform.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_Conform.Click += new System.EventHandler(this.btn_Conform_Click);
            // 
            // SensorTest_ProfileHideParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 370);
            this.Controls.Add(this.btn_Conform);
            this.Controls.Add(this.btn_UnSelectAll);
            this.Controls.Add(this.btn_SelectAll);
            this.Controls.Add(this.uiCheckBoxGroup1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SensorTest_ProfileHideParams";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.SensorTest_ProfileHideParams_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UICheckBoxGroup uiCheckBoxGroup1;
        private Sunny.UI.UIButton btn_SelectAll;
        private Sunny.UI.UIButton btn_UnSelectAll;
        private Sunny.UI.UIButton btn_Conform;
    }
}