namespace ITS_Studio.AllForms
{
    partial class LegalNotice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LegalNotice));
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPanel1
            // 
            this.uiPanel1.BackColor = System.Drawing.Color.White;
            this.uiPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("uiPanel1.BackgroundImage")));
            this.uiPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.uiPanel1.Controls.Add(this.pictureBox1);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel1.FillColorGradient = true;
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Radius = 50;
            this.uiPanel1.RectColor = System.Drawing.Color.Transparent;
            this.uiPanel1.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.uiPanel1.Size = new System.Drawing.Size(791, 437);
            this.uiPanel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiPanel1.TabIndex = 3;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiPanel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::ITS_Studio.Properties.Resources.loading;
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(260, 331);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 103);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // LegalNotice
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(791, 437);
            this.Controls.Add(this.uiPanel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LegalNotice";
            this.Opacity = 0.8D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.SystemColors.WindowFrame;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LegalNotice_FormClosing);
            this.uiPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIPanel uiPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;


    }
}