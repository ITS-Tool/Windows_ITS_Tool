namespace ITS_Studio.AllForms
{
    partial class NumberForm
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
            this.uiLedDisplay1 = new Sunny.UI.UILedDisplay();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uiSymbolLabel1 = new Sunny.UI.UISymbolLabel();
            this.uiSymbolLabel2 = new Sunny.UI.UISymbolLabel();
            this.uiSymbolLabel3 = new Sunny.UI.UISymbolLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // uiLedDisplay1
            // 
            this.uiLedDisplay1.BackColor = System.Drawing.Color.Transparent;
            this.uiLedDisplay1.BorderInWidth = 0;
            this.uiLedDisplay1.BorderWidth = 0;
            this.uiLedDisplay1.CharCount = 1;
            this.uiLedDisplay1.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiLedDisplay1.ForeColor = System.Drawing.Color.Red;
            this.uiLedDisplay1.IntervalH = 4;
            this.uiLedDisplay1.IntervalIn = 4;
            this.uiLedDisplay1.IntervalOn = 20;
            this.uiLedDisplay1.IntervalV = 4;
            this.uiLedDisplay1.LedBackColor = System.Drawing.Color.Transparent;
            this.uiLedDisplay1.Location = new System.Drawing.Point(99, 338);
            this.uiLedDisplay1.Name = "uiLedDisplay1";
            this.uiLedDisplay1.Size = new System.Drawing.Size(172, 172);
            this.uiLedDisplay1.TabIndex = 0;
            this.uiLedDisplay1.Text = "1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::ITS_Studio.Properties.Resources.scrolldowngesture;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(360, 277);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // uiSymbolLabel1
            // 
            this.uiSymbolLabel1.Font = new System.Drawing.Font("Consolas", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiSymbolLabel1.ForeColor = System.Drawing.Color.Red;
            this.uiSymbolLabel1.Location = new System.Drawing.Point(12, 295);
            this.uiSymbolLabel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSymbolLabel1.Name = "uiSymbolLabel1";
            this.uiSymbolLabel1.Padding = new System.Windows.Forms.Padding(28, 0, 0, 0);
            this.uiSymbolLabel1.Size = new System.Drawing.Size(335, 37);
            this.uiSymbolLabel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiSymbolLabel1.Symbol = 361538;
            this.uiSymbolLabel1.SymbolColor = System.Drawing.Color.Red;
            this.uiSymbolLabel1.TabIndex = 3;
            this.uiSymbolLabel1.Text = "put on your finger";
            this.uiSymbolLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiSymbolLabel2
            // 
            this.uiSymbolLabel2.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Bold);
            this.uiSymbolLabel2.ForeColor = System.Drawing.Color.Blue;
            this.uiSymbolLabel2.Location = new System.Drawing.Point(0, 352);
            this.uiSymbolLabel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSymbolLabel2.Name = "uiSymbolLabel2";
            this.uiSymbolLabel2.Padding = new System.Windows.Forms.Padding(28, 0, 0, 0);
            this.uiSymbolLabel2.Size = new System.Drawing.Size(384, 54);
            this.uiSymbolLabel2.Style = Sunny.UI.UIStyle.Custom;
            this.uiSymbolLabel2.Symbol = 61553;
            this.uiSymbolLabel2.SymbolColor = System.Drawing.Color.Blue;
            this.uiSymbolLabel2.TabIndex = 4;
            this.uiSymbolLabel2.Text = "Please do not touch the panel.";
            this.uiSymbolLabel2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiSymbolLabel3
            // 
            this.uiSymbolLabel3.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Bold);
            this.uiSymbolLabel3.ForeColor = System.Drawing.Color.Blue;
            this.uiSymbolLabel3.Location = new System.Drawing.Point(1, 401);
            this.uiSymbolLabel3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSymbolLabel3.Name = "uiSymbolLabel3";
            this.uiSymbolLabel3.Padding = new System.Windows.Forms.Padding(28, 0, 0, 0);
            this.uiSymbolLabel3.Size = new System.Drawing.Size(384, 54);
            this.uiSymbolLabel3.Style = Sunny.UI.UIStyle.Custom;
            this.uiSymbolLabel3.Symbol = 61553;
            this.uiSymbolLabel3.SymbolColor = System.Drawing.Color.Blue;
            this.uiSymbolLabel3.TabIndex = 5;
            this.uiSymbolLabel3.Text = "請不要觸摸測試面板";
            this.uiSymbolLabel3.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // NumberForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(385, 533);
            this.Controls.Add(this.uiSymbolLabel3);
            this.Controls.Add(this.uiSymbolLabel2);
            this.Controls.Add(this.uiSymbolLabel1);
            this.Controls.Add(this.uiLedDisplay1);
            this.Controls.Add(this.pictureBox1);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NumberForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NumberForm_FormClosing);
            this.Load += new System.EventHandler(this.NumberForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UILedDisplay uiLedDisplay1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Sunny.UI.UISymbolLabel uiSymbolLabel1;
        private Sunny.UI.UISymbolLabel uiSymbolLabel2;
        private Sunny.UI.UISymbolLabel uiSymbolLabel3;
    }
}