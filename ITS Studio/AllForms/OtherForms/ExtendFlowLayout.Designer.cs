namespace ITS_Studio.AllForms
{
    partial class ExtendFlowLayout
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.m_btFreSpectrum = new System.Windows.Forms.Button();
            this.m_btChargCurve = new System.Windows.Forms.Button();
            this.m_btCModel = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.m_btFreSpectrum);
            this.flowLayoutPanel1.Controls.Add(this.m_btChargCurve);
            this.flowLayoutPanel1.Controls.Add(this.m_btCModel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(292, 327);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // m_btFreSpectrum
            // 
            this.m_btFreSpectrum.BackColor = System.Drawing.Color.Tan;
            this.m_btFreSpectrum.Enabled = false;
            this.m_btFreSpectrum.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btFreSpectrum.Image = global::ITS_Studio.Properties.Resources.Document_Preview_icon;
            this.m_btFreSpectrum.Location = new System.Drawing.Point(3, 3);
            this.m_btFreSpectrum.Name = "m_btFreSpectrum";
            this.m_btFreSpectrum.Size = new System.Drawing.Size(84, 96);
            this.m_btFreSpectrum.TabIndex = 26;
            this.m_btFreSpectrum.Text = "Frequency Spectrum";
            this.m_btFreSpectrum.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.m_btFreSpectrum.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_btFreSpectrum.UseVisualStyleBackColor = false;
            this.m_btFreSpectrum.Click += new System.EventHandler(this.m_btFreSpectrum_Click);
            // 
            // m_btChargCurve
            // 
            this.m_btChargCurve.BackColor = System.Drawing.Color.Tan;
            this.m_btChargCurve.Cursor = System.Windows.Forms.Cursors.Default;
            this.m_btChargCurve.Enabled = false;
            this.m_btChargCurve.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btChargCurve.Image = global::ITS_Studio.Properties.Resources.Document_Line_Chart_icon;
            this.m_btChargCurve.Location = new System.Drawing.Point(93, 3);
            this.m_btChargCurve.Name = "m_btChargCurve";
            this.m_btChargCurve.Size = new System.Drawing.Size(84, 96);
            this.m_btChargCurve.TabIndex = 27;
            this.m_btChargCurve.Text = "Charge Curve";
            this.m_btChargCurve.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.m_btChargCurve.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_btChargCurve.UseVisualStyleBackColor = false;
            this.m_btChargCurve.Click += new System.EventHandler(this.m_btChargCurve_Click);
            // 
            // m_btCModel
            // 
            this.m_btCModel.BackColor = System.Drawing.Color.Tan;
            this.m_btCModel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btCModel.Image = global::ITS_Studio.Properties.Resources.c_model_64_64;
            this.m_btCModel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.m_btCModel.Location = new System.Drawing.Point(183, 1);
            this.m_btCModel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.m_btCModel.Name = "m_btCModel";
            this.m_btCModel.Size = new System.Drawing.Size(84, 96);
            this.m_btCModel.TabIndex = 28;
            this.m_btCModel.Text = "C Model";
            this.m_btCModel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.m_btCModel.UseVisualStyleBackColor = false;
            this.m_btCModel.Click += new System.EventHandler(this.m_btCModel_Click);
            // 
            // ExtendFlowLayout
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(292, 327);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExtendFlowLayout";
            this.Text = "ExtendFlowLayout";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Deactivate += new System.EventHandler(this.ExtendFlowLayout_Deactivate);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        public System.Windows.Forms.Button m_btFreSpectrum;
        public System.Windows.Forms.Button m_btChargCurve;
        public System.Windows.Forms.Button m_btCModel;
    }
}