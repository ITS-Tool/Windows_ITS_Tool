namespace ITS_Studio
{
    partial class TuningToolfm
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
            this.splitContainer_Main = new System.Windows.Forms.SplitContainer();
            this.splitContainer_Functions = new System.Windows.Forms.SplitContainer();
            this.m_tb_TPLName = new System.Windows.Forms.TextBox();
            this.m_btScanCH = new System.Windows.Forms.Button();
            this.m_butTuningCompare = new System.Windows.Forms.Button();
            this.m_btMergeToHex = new System.Windows.Forms.Button();
            this.m_butReadParam = new System.Windows.Forms.Button();
            this.m_butWriteParam = new System.Windows.Forms.Button();
            this.tabControl_Bottom = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gBox_TuningMode = new System.Windows.Forms.GroupBox();
            this.cbx_Tuning_Mode = new System.Windows.Forms.ComboBox();
            this.Lbl_ModeSelect = new System.Windows.Forms.Label();
            this.m_butCommitParam = new System.Windows.Forms.Button();
            this.splitContainer_Para = new System.Windows.Forms.SplitContainer();
            this.m_tbTuningParameter = new System.Windows.Forms.TabControl();
            this.m_gboxHint = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).BeginInit();
            this.splitContainer_Main.Panel1.SuspendLayout();
            this.splitContainer_Main.Panel2.SuspendLayout();
            this.splitContainer_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Functions)).BeginInit();
            this.splitContainer_Functions.Panel1.SuspendLayout();
            this.splitContainer_Functions.Panel2.SuspendLayout();
            this.splitContainer_Functions.SuspendLayout();
            this.tabControl_Bottom.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.gBox_TuningMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Para)).BeginInit();
            this.splitContainer_Para.Panel1.SuspendLayout();
            this.splitContainer_Para.Panel2.SuspendLayout();
            this.splitContainer_Para.SuspendLayout();
            this.m_gboxHint.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer_Main
            // 
            this.splitContainer_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Main.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_Main.IsSplitterFixed = true;
            this.splitContainer_Main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Main.Name = "splitContainer_Main";
            // 
            // splitContainer_Main.Panel1
            // 
            this.splitContainer_Main.Panel1.AutoScroll = true;
            this.splitContainer_Main.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.splitContainer_Main.Panel1.Controls.Add(this.splitContainer_Functions);
            // 
            // splitContainer_Main.Panel2
            // 
            this.splitContainer_Main.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.splitContainer_Main.Panel2.Controls.Add(this.splitContainer_Para);
            this.splitContainer_Main.Size = new System.Drawing.Size(700, 544);
            this.splitContainer_Main.SplitterDistance = 297;
            this.splitContainer_Main.TabIndex = 0;
            // 
            // splitContainer_Functions
            // 
            this.splitContainer_Functions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Functions.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer_Functions.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Functions.Name = "splitContainer_Functions";
            this.splitContainer_Functions.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Functions.Panel1
            // 
            this.splitContainer_Functions.Panel1.AutoScroll = true;
            this.splitContainer_Functions.Panel1.Controls.Add(this.m_tb_TPLName);
            this.splitContainer_Functions.Panel1.Controls.Add(this.m_btScanCH);
            this.splitContainer_Functions.Panel1.Controls.Add(this.m_butTuningCompare);
            this.splitContainer_Functions.Panel1.Controls.Add(this.m_btMergeToHex);
            this.splitContainer_Functions.Panel1.Controls.Add(this.m_butReadParam);
            this.splitContainer_Functions.Panel1.Controls.Add(this.m_butWriteParam);
            // 
            // splitContainer_Functions.Panel2
            // 
            this.splitContainer_Functions.Panel2.Controls.Add(this.tabControl_Bottom);
            this.splitContainer_Functions.Size = new System.Drawing.Size(297, 544);
            this.splitContainer_Functions.SplitterDistance = 403;
            this.splitContainer_Functions.TabIndex = 25;
            // 
            // m_tb_TPLName
            // 
            this.m_tb_TPLName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.m_tb_TPLName.Cursor = System.Windows.Forms.Cursors.Default;
            this.m_tb_TPLName.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_tb_TPLName.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_tb_TPLName.Location = new System.Drawing.Point(0, 262);
            this.m_tb_TPLName.Name = "m_tb_TPLName";
            this.m_tb_TPLName.ReadOnly = true;
            this.m_tb_TPLName.Size = new System.Drawing.Size(297, 26);
            this.m_tb_TPLName.TabIndex = 23;
            // 
            // m_btScanCH
            // 
            this.m_btScanCH.Cursor = System.Windows.Forms.Cursors.Default;
            this.m_btScanCH.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_btScanCH.Enabled = false;
            this.m_btScanCH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.m_btScanCH.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btScanCH.Image = global::ITS_Studio.Properties.Resources.Document_Chart_icon;
            this.m_btScanCH.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_btScanCH.Location = new System.Drawing.Point(0, 212);
            this.m_btScanCH.Name = "m_btScanCH";
            this.m_btScanCH.Size = new System.Drawing.Size(297, 50);
            this.m_btScanCH.TabIndex = 22;
            this.m_btScanCH.Text = "Auto Scan Channel";
            this.m_btScanCH.UseVisualStyleBackColor = true;
            this.m_btScanCH.Click += new System.EventHandler(this.m_btScanCH_Click);
            // 
            // m_butTuningCompare
            // 
            this.m_butTuningCompare.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_butTuningCompare.Enabled = false;
            this.m_butTuningCompare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.m_butTuningCompare.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_butTuningCompare.Image = global::ITS_Studio.Properties.Resources.Document_Copy_icon;
            this.m_butTuningCompare.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_butTuningCompare.Location = new System.Drawing.Point(0, 156);
            this.m_butTuningCompare.Name = "m_butTuningCompare";
            this.m_butTuningCompare.Size = new System.Drawing.Size(297, 56);
            this.m_butTuningCompare.TabIndex = 19;
            this.m_butTuningCompare.Text = "Tuning Compare";
            this.m_butTuningCompare.UseVisualStyleBackColor = true;
            this.m_butTuningCompare.Click += new System.EventHandler(this.m_butTuningCompare_Click);
            // 
            // m_btMergeToHex
            // 
            this.m_btMergeToHex.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_btMergeToHex.Enabled = false;
            this.m_btMergeToHex.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.m_btMergeToHex.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btMergeToHex.Image = global::ITS_Studio.Properties.Resources.File_Open_icon;
            this.m_btMergeToHex.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_btMergeToHex.Location = new System.Drawing.Point(0, 106);
            this.m_btMergeToHex.Name = "m_btMergeToHex";
            this.m_btMergeToHex.Size = new System.Drawing.Size(297, 50);
            this.m_btMergeToHex.TabIndex = 0;
            this.m_btMergeToHex.Text = "Merge To Hex";
            this.m_btMergeToHex.UseVisualStyleBackColor = true;
            this.m_btMergeToHex.Click += new System.EventHandler(this.m_btMergeToHex_Click);
            // 
            // m_butReadParam
            // 
            this.m_butReadParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_butReadParam.Enabled = false;
            this.m_butReadParam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.m_butReadParam.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_butReadParam.Image = global::ITS_Studio.Properties.Resources.Document_icon;
            this.m_butReadParam.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_butReadParam.Location = new System.Drawing.Point(0, 53);
            this.m_butReadParam.Name = "m_butReadParam";
            this.m_butReadParam.Size = new System.Drawing.Size(297, 53);
            this.m_butReadParam.TabIndex = 9;
            this.m_butReadParam.Text = "Read Parameter";
            this.m_butReadParam.UseVisualStyleBackColor = true;
            this.m_butReadParam.Click += new System.EventHandler(this.m_btnReadParam_Click);
            // 
            // m_butWriteParam
            // 
            this.m_butWriteParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_butWriteParam.Enabled = false;
            this.m_butWriteParam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.m_butWriteParam.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_butWriteParam.Image = global::ITS_Studio.Properties.Resources.Document_Edit_icon;
            this.m_butWriteParam.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_butWriteParam.Location = new System.Drawing.Point(0, 0);
            this.m_butWriteParam.Name = "m_butWriteParam";
            this.m_butWriteParam.Size = new System.Drawing.Size(297, 53);
            this.m_butWriteParam.TabIndex = 7;
            this.m_butWriteParam.Text = "Write Parameter";
            this.m_butWriteParam.UseVisualStyleBackColor = true;
            this.m_butWriteParam.Click += new System.EventHandler(this.m_butWriteParam_Click);
            // 
            // tabControl_Bottom
            // 
            this.tabControl_Bottom.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl_Bottom.Controls.Add(this.tabPage1);
            this.tabControl_Bottom.Controls.Add(this.tabPage2);
            this.tabControl_Bottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Bottom.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl_Bottom.ItemSize = new System.Drawing.Size(0, 1);
            this.tabControl_Bottom.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Bottom.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl_Bottom.Name = "tabControl_Bottom";
            this.tabControl_Bottom.Padding = new System.Drawing.Point(0, 0);
            this.tabControl_Bottom.SelectedIndex = 0;
            this.tabControl_Bottom.Size = new System.Drawing.Size(297, 137);
            this.tabControl_Bottom.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_Bottom.TabIndex = 24;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(289, 128);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(283, 122);
            this.panel1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gBox_TuningMode);
            this.tabPage2.Location = new System.Drawing.Point(4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(289, 128);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gBox_TuningMode
            // 
            this.gBox_TuningMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.gBox_TuningMode.Controls.Add(this.cbx_Tuning_Mode);
            this.gBox_TuningMode.Controls.Add(this.Lbl_ModeSelect);
            this.gBox_TuningMode.Controls.Add(this.m_butCommitParam);
            this.gBox_TuningMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gBox_TuningMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gBox_TuningMode.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gBox_TuningMode.Location = new System.Drawing.Point(3, 3);
            this.gBox_TuningMode.Name = "gBox_TuningMode";
            this.gBox_TuningMode.Size = new System.Drawing.Size(283, 122);
            this.gBox_TuningMode.TabIndex = 20;
            this.gBox_TuningMode.TabStop = false;
            this.gBox_TuningMode.Text = "Tuning Mode";
            this.gBox_TuningMode.Visible = false;
            // 
            // cbx_Tuning_Mode
            // 
            this.cbx_Tuning_Mode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.cbx_Tuning_Mode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbx_Tuning_Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_Tuning_Mode.FormattingEnabled = true;
            this.cbx_Tuning_Mode.Items.AddRange(new object[] {
            "0",
            "1"});
            this.cbx_Tuning_Mode.Location = new System.Drawing.Point(3, 32);
            this.cbx_Tuning_Mode.Name = "cbx_Tuning_Mode";
            this.cbx_Tuning_Mode.Size = new System.Drawing.Size(277, 22);
            this.cbx_Tuning_Mode.TabIndex = 17;
            this.cbx_Tuning_Mode.Visible = false;
            this.cbx_Tuning_Mode.SelectedIndexChanged += new System.EventHandler(this.cbx_Tuning_Mode_SelectedIndexChanged);
            // 
            // Lbl_ModeSelect
            // 
            this.Lbl_ModeSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.Lbl_ModeSelect.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_ModeSelect.Location = new System.Drawing.Point(3, 18);
            this.Lbl_ModeSelect.Name = "Lbl_ModeSelect";
            this.Lbl_ModeSelect.Size = new System.Drawing.Size(277, 14);
            this.Lbl_ModeSelect.TabIndex = 18;
            this.Lbl_ModeSelect.Text = "UI Mode Select";
            this.Lbl_ModeSelect.Visible = false;
            // 
            // m_butCommitParam
            // 
            this.m_butCommitParam.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_butCommitParam.Enabled = false;
            this.m_butCommitParam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.m_butCommitParam.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_butCommitParam.Image = global::ITS_Studio.Properties.Resources.page_process_icon;
            this.m_butCommitParam.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_butCommitParam.Location = new System.Drawing.Point(3, 69);
            this.m_butCommitParam.Name = "m_butCommitParam";
            this.m_butCommitParam.Size = new System.Drawing.Size(277, 50);
            this.m_butCommitParam.TabIndex = 10;
            this.m_butCommitParam.Text = "Set FW Tuning Mode";
            this.m_butCommitParam.UseVisualStyleBackColor = true;
            this.m_butCommitParam.Visible = false;
            this.m_butCommitParam.Click += new System.EventHandler(this.m_butSetTuningMode_Click);
            // 
            // splitContainer_Para
            // 
            this.splitContainer_Para.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Para.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer_Para.IsSplitterFixed = true;
            this.splitContainer_Para.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Para.Name = "splitContainer_Para";
            this.splitContainer_Para.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Para.Panel1
            // 
            this.splitContainer_Para.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.splitContainer_Para.Panel1.Controls.Add(this.m_tbTuningParameter);
            // 
            // splitContainer_Para.Panel2
            // 
            this.splitContainer_Para.Panel2.AutoScroll = true;
            this.splitContainer_Para.Panel2.Controls.Add(this.m_gboxHint);
            this.splitContainer_Para.Size = new System.Drawing.Size(399, 544);
            this.splitContainer_Para.SplitterDistance = 416;
            this.splitContainer_Para.TabIndex = 0;
            // 
            // m_tbTuningParameter
            // 
            this.m_tbTuningParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tbTuningParameter.Location = new System.Drawing.Point(0, 0);
            this.m_tbTuningParameter.Name = "m_tbTuningParameter";
            this.m_tbTuningParameter.SelectedIndex = 0;
            this.m_tbTuningParameter.Size = new System.Drawing.Size(399, 416);
            this.m_tbTuningParameter.TabIndex = 0;
            // 
            // m_gboxHint
            // 
            this.m_gboxHint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.m_gboxHint.Controls.Add(this.richTextBox1);
            this.m_gboxHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_gboxHint.Location = new System.Drawing.Point(0, 0);
            this.m_gboxHint.Name = "m_gboxHint";
            this.m_gboxHint.Size = new System.Drawing.Size(399, 124);
            this.m_gboxHint.TabIndex = 0;
            this.m_gboxHint.TabStop = false;
            this.m_gboxHint.Text = "Hint";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.EnableAutoDragDrop = true;
            this.richTextBox1.Location = new System.Drawing.Point(3, 18);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(393, 103);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // TuningToolfm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(700, 544);
            this.Controls.Add(this.splitContainer_Main);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(700, 300);
            this.Name = "TuningToolfm";
            this.Text = "TuningTool";
            this.splitContainer_Main.Panel1.ResumeLayout(false);
            this.splitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).EndInit();
            this.splitContainer_Main.ResumeLayout(false);
            this.splitContainer_Functions.Panel1.ResumeLayout(false);
            this.splitContainer_Functions.Panel1.PerformLayout();
            this.splitContainer_Functions.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Functions)).EndInit();
            this.splitContainer_Functions.ResumeLayout(false);
            this.tabControl_Bottom.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.gBox_TuningMode.ResumeLayout(false);
            this.splitContainer_Para.Panel1.ResumeLayout(false);
            this.splitContainer_Para.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Para)).EndInit();
            this.splitContainer_Para.ResumeLayout(false);
            this.m_gboxHint.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_Main;
        private System.Windows.Forms.Button m_butReadParam;
        private System.Windows.Forms.SplitContainer splitContainer_Para;
        private System.Windows.Forms.GroupBox m_gboxHint;
        private System.Windows.Forms.ComboBox cbx_Tuning_Mode;
        private System.Windows.Forms.Label Lbl_ModeSelect;
        public System.Windows.Forms.Button m_butTuningCompare;
        private System.Windows.Forms.Button m_btMergeToHex;
        public System.Windows.Forms.Button m_butCommitParam;
        private System.Windows.Forms.GroupBox gBox_TuningMode;
        private System.Windows.Forms.Button m_btScanCH;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabControl m_tbTuningParameter;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer_Functions;
        private System.Windows.Forms.TabControl tabControl_Bottom;
        private System.Windows.Forms.Button m_butWriteParam;
        private System.Windows.Forms.TextBox m_tb_TPLName;
        private System.Windows.Forms.Panel panel1;
    }
}