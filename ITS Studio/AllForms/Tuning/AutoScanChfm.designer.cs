namespace ITS_Studio
{
    partial class AutoScanCHfm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoScanCHfm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Bt_Exit = new System.Windows.Forms.Button();
            this.btnScanChStart = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_Info = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabPage_CDC_1 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_Info.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.Bt_Exit);
            this.splitContainer1.Panel1.Controls.Add(this.btnScanChStart);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // Bt_Exit
            // 
            resources.ApplyResources(this.Bt_Exit, "Bt_Exit");
            this.Bt_Exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bt_Exit.Image = global::ITS_Studio.Properties.Resources.Arrow_Back_icon_64_64;
            this.Bt_Exit.Name = "Bt_Exit";
            this.Bt_Exit.UseVisualStyleBackColor = true;
            this.Bt_Exit.Click += new System.EventHandler(this.Bt_Exit_Click);
            // 
            // btnScanChStart
            // 
            resources.ApplyResources(this.btnScanChStart, "btnScanChStart");
            this.btnScanChStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnScanChStart.Image = global::ITS_Studio.Properties.Resources.Search_icon;
            this.btnScanChStart.Name = "btnScanChStart";
            this.btnScanChStart.UseVisualStyleBackColor = true;
            this.btnScanChStart.Click += new System.EventHandler(this.btnScanChStart_Click);
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage_Info);
            this.tabControl1.Controls.Add(this.tabPage_CDC_1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage_Info
            // 
            resources.ApplyResources(this.tabPage_Info, "tabPage_Info");
            this.tabPage_Info.Controls.Add(this.richTextBox1);
            this.tabPage_Info.Name = "tabPage_Info";
            this.tabPage_Info.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            // 
            // tabPage_CDC_1
            // 
            resources.ApplyResources(this.tabPage_CDC_1, "tabPage_CDC_1");
            this.tabPage_CDC_1.Name = "tabPage_CDC_1";
            this.tabPage_CDC_1.UseVisualStyleBackColor = true;
            // 
            // AutoScanCHfm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutoScanCHfm";
            this.Load += new System.EventHandler(this.AutoScanCH_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_Info.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button Bt_Exit;
        private System.Windows.Forms.Button btnScanChStart;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_Info;
        private System.Windows.Forms.TabPage tabPage_CDC_1;

    }
}