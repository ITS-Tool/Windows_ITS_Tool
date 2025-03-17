namespace ITS_Studio.AllForms
{
    partial class FixtureHint
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
            this.uiSymbolLabel1 = new Sunny.UI.UISymbolLabel();
            this.uiLedDisplay2 = new Sunny.UI.UILedDisplay();
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
            this.uiLedDisplay1.Location = new System.Drawing.Point(188, 167);
            this.uiLedDisplay1.Name = "uiLedDisplay1";
            this.uiLedDisplay1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.uiLedDisplay1.Size = new System.Drawing.Size(172, 172);
            this.uiLedDisplay1.TabIndex = 0;
            this.uiLedDisplay1.Text = "0";
            // 
            // uiSymbolLabel1
            // 
            this.uiSymbolLabel1.Font = new System.Drawing.Font("Consolas", 30F, System.Drawing.FontStyle.Bold);
            this.uiSymbolLabel1.ForeColor = System.Drawing.Color.Red;
            this.uiSymbolLabel1.Location = new System.Drawing.Point(0, 0);
            this.uiSymbolLabel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSymbolLabel1.Name = "uiSymbolLabel1";
            this.uiSymbolLabel1.Padding = new System.Windows.Forms.Padding(28, 0, 0, 0);
            this.uiSymbolLabel1.Size = new System.Drawing.Size(385, 148);
            this.uiSymbolLabel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiSymbolLabel1.Symbol = 61530;
            this.uiSymbolLabel1.SymbolColor = System.Drawing.Color.Red;
            this.uiSymbolLabel1.TabIndex = 3;
            this.uiSymbolLabel1.Text = "请点亮LCM";
            this.uiSymbolLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiLedDisplay2
            // 
            this.uiLedDisplay2.BackColor = System.Drawing.Color.Transparent;
            this.uiLedDisplay2.BorderInWidth = 0;
            this.uiLedDisplay2.BorderWidth = 0;
            this.uiLedDisplay2.CharCount = 1;
            this.uiLedDisplay2.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiLedDisplay2.ForeColor = System.Drawing.Color.Red;
            this.uiLedDisplay2.IntervalH = 4;
            this.uiLedDisplay2.IntervalIn = 4;
            this.uiLedDisplay2.IntervalOn = 20;
            this.uiLedDisplay2.IntervalV = 4;
            this.uiLedDisplay2.LedBackColor = System.Drawing.Color.Transparent;
            this.uiLedDisplay2.Location = new System.Drawing.Point(26, 167);
            this.uiLedDisplay2.Name = "uiLedDisplay2";
            this.uiLedDisplay2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.uiLedDisplay2.Size = new System.Drawing.Size(172, 172);
            this.uiLedDisplay2.TabIndex = 4;
            this.uiLedDisplay2.Text = "0";
            // 
            // FixtureHint
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(385, 412);
            this.Controls.Add(this.uiLedDisplay2);
            this.Controls.Add(this.uiSymbolLabel1);
            this.Controls.Add(this.uiLedDisplay1);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FixtureHint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FixtureHint_FormClosing);
            this.Load += new System.EventHandler(this.FixtureHint_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UILedDisplay uiLedDisplay1;
        private Sunny.UI.UISymbolLabel uiSymbolLabel1;
        private Sunny.UI.UILedDisplay uiLedDisplay2;
    }
}