namespace ITS_Studio
{
    partial class fmSensorTest_Painting
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
            this.m_panDrawing = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // m_panDrawing
            // 
            this.m_panDrawing.BackColor = System.Drawing.Color.Transparent;
            this.m_panDrawing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_panDrawing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_panDrawing.Location = new System.Drawing.Point(0, 0);
            this.m_panDrawing.Margin = new System.Windows.Forms.Padding(4);
            this.m_panDrawing.Name = "m_panDrawing";
            this.m_panDrawing.Size = new System.Drawing.Size(450, 450);
            this.m_panDrawing.TabIndex = 0;
            this.m_panDrawing.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaintEvent);
            // 
            // fmSensorTest_Painting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(450, 450);
            this.Controls.Add(this.m_panDrawing);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fmSensorTest_Painting";
            this.Text = "SensorTest_Painting";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SensorTest_Painting_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel m_panDrawing;

    }
}