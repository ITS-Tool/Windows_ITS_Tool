namespace ITS_Studio.AllForms.SensorTest
{
    partial class SensorTest_Pattern
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
            this.lb_Data = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb_Data
            // 
            this.lb_Data.AutoSize = true;
            this.lb_Data.BackColor = System.Drawing.Color.Transparent;
            this.lb_Data.Font = new System.Drawing.Font("標楷體", 24F, System.Drawing.FontStyle.Bold);
            this.lb_Data.ForeColor = System.Drawing.Color.Silver;
            this.lb_Data.Location = new System.Drawing.Point(357, 345);
            this.lb_Data.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_Data.Name = "lb_Data";
            this.lb_Data.Size = new System.Drawing.Size(236, 32);
            this.lb_Data.TabIndex = 1;
            this.lb_Data.Text = "Short Testing";
            // 
            // SensorTest_Pattern
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(984, 750);
            this.Controls.Add(this.lb_Data);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SensorTest_Pattern";
            this.Text = "SensorTest_Pattern";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_Data;
    }
}