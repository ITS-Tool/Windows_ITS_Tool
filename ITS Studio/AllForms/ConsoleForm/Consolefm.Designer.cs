namespace ITS_Studio.AllForms
{
    partial class Consolefm
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
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_StartScript = new Sunny.UI.UISymbolButton();
            this.btn_OpenScript = new Sunny.UI.UISymbolButton();
            this.lbl_ScriptFileName = new System.Windows.Forms.Label();
            this.txt_SendCMD = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_Read = new System.Windows.Forms.ComboBox();
            this.comboBox_Send = new System.Windows.Forms.ComboBox();
            this.Prompt = new System.Windows.Forms.RichTextBox();
            this.RichEdit_ConsoleBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "CMD";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.RichEdit_ConsoleBox);
            this.splitContainer1.Size = new System.Drawing.Size(742, 498);
            this.splitContainer1.SplitterDistance = 138;
            this.splitContainer1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.panel1.Controls.Add(this.btn_StartScript);
            this.panel1.Controls.Add(this.btn_OpenScript);
            this.panel1.Controls.Add(this.lbl_ScriptFileName);
            this.panel1.Controls.Add(this.txt_SendCMD);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.comboBox_Read);
            this.panel1.Controls.Add(this.comboBox_Send);
            this.panel1.Controls.Add(this.Prompt);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(742, 138);
            this.panel1.TabIndex = 8;
            // 
            // btn_StartScript
            // 
            this.btn_StartScript.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_StartScript.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btn_StartScript.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btn_StartScript.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.btn_StartScript.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btn_StartScript.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btn_StartScript.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.btn_StartScript.Location = new System.Drawing.Point(342, 112);
            this.btn_StartScript.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_StartScript.Name = "btn_StartScript";
            this.btn_StartScript.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btn_StartScript.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.btn_StartScript.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btn_StartScript.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btn_StartScript.Size = new System.Drawing.Size(133, 23);
            this.btn_StartScript.Style = Sunny.UI.UIStyle.Orange;
            this.btn_StartScript.Symbol = 61459;
            this.btn_StartScript.TabIndex = 24;
            this.btn_StartScript.Text = "Run Script";
            this.btn_StartScript.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_StartScript.Click += new System.EventHandler(this.btn_StartScript_Click);
            // 
            // btn_OpenScript
            // 
            this.btn_OpenScript.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_OpenScript.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btn_OpenScript.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btn_OpenScript.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.btn_OpenScript.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btn_OpenScript.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btn_OpenScript.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_OpenScript.Location = new System.Drawing.Point(342, 84);
            this.btn_OpenScript.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_OpenScript.Name = "btn_OpenScript";
            this.btn_OpenScript.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btn_OpenScript.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.btn_OpenScript.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btn_OpenScript.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btn_OpenScript.Size = new System.Drawing.Size(133, 23);
            this.btn_OpenScript.Style = Sunny.UI.UIStyle.Orange;
            this.btn_OpenScript.Symbol = 61564;
            this.btn_OpenScript.TabIndex = 23;
            this.btn_OpenScript.Text = "Open Script";
            this.btn_OpenScript.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_OpenScript.Click += new System.EventHandler(this.btn_OpenScript_Click);
            // 
            // lbl_ScriptFileName
            // 
            this.lbl_ScriptFileName.AutoSize = true;
            this.lbl_ScriptFileName.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ScriptFileName.Location = new System.Drawing.Point(334, 59);
            this.lbl_ScriptFileName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_ScriptFileName.Name = "lbl_ScriptFileName";
            this.lbl_ScriptFileName.Size = new System.Drawing.Size(135, 19);
            this.lbl_ScriptFileName.TabIndex = 21;
            this.lbl_ScriptFileName.Text = "Script File : ";
            // 
            // txt_SendCMD
            // 
            this.txt_SendCMD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_SendCMD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_SendCMD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_SendCMD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_SendCMD.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_SendCMD.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txt_SendCMD.Location = new System.Drawing.Point(66, 27);
            this.txt_SendCMD.Name = "txt_SendCMD";
            this.txt_SendCMD.Size = new System.Drawing.Size(664, 16);
            this.txt_SendCMD.TabIndex = 16;
            this.txt_SendCMD.TextChanged += new System.EventHandler(this.m_rtbSendCMD_TextChanged);
            this.txt_SendCMD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m_rtbSendCMD_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 19);
            this.label3.TabIndex = 15;
            this.label3.Text = "Read";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 19);
            this.label2.TabIndex = 14;
            this.label2.Text = "Send";
            // 
            // comboBox_Read
            // 
            this.comboBox_Read.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Read.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_Read.FormattingEnabled = true;
            this.comboBox_Read.Location = new System.Drawing.Point(67, 84);
            this.comboBox_Read.Name = "comboBox_Read";
            this.comboBox_Read.Size = new System.Drawing.Size(253, 23);
            this.comboBox_Read.TabIndex = 13;
            this.comboBox_Read.SelectedIndexChanged += new System.EventHandler(this.comboBox_Read_SelectedIndexChanged);
            // 
            // comboBox_Send
            // 
            this.comboBox_Send.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Send.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_Send.FormattingEnabled = true;
            this.comboBox_Send.Location = new System.Drawing.Point(67, 55);
            this.comboBox_Send.Name = "comboBox_Send";
            this.comboBox_Send.Size = new System.Drawing.Size(253, 23);
            this.comboBox_Send.TabIndex = 12;
            this.comboBox_Send.SelectedIndexChanged += new System.EventHandler(this.comboBox_Send_SelectedIndexChanged);
            // 
            // Prompt
            // 
            this.Prompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.Prompt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Prompt.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Prompt.Enabled = false;
            this.Prompt.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Prompt.Location = new System.Drawing.Point(63, 3);
            this.Prompt.Name = "Prompt";
            this.Prompt.ReadOnly = true;
            this.Prompt.Size = new System.Drawing.Size(374, 20);
            this.Prompt.TabIndex = 11;
            this.Prompt.Text = "{Report_ID} {0xA3} {W_Len} {R_Len} {Data ...}";
            // 
            // RichEdit_ConsoleBox
            // 
            this.RichEdit_ConsoleBox.BackColor = System.Drawing.SystemColors.InfoText;
            this.RichEdit_ConsoleBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichEdit_ConsoleBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RichEdit_ConsoleBox.ForeColor = System.Drawing.Color.LimeGreen;
            this.RichEdit_ConsoleBox.Location = new System.Drawing.Point(0, 0);
            this.RichEdit_ConsoleBox.Name = "RichEdit_ConsoleBox";
            this.RichEdit_ConsoleBox.ReadOnly = true;
            this.RichEdit_ConsoleBox.Size = new System.Drawing.Size(742, 356);
            this.RichEdit_ConsoleBox.TabIndex = 0;
            this.RichEdit_ConsoleBox.Text = "";
            // 
            // Consolefm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(742, 498);
            this.Controls.Add(this.splitContainer1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Consolefm";
            this.Text = "Consolefm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox Prompt;
        private System.Windows.Forms.RichTextBox RichEdit_ConsoleBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_Read;
        private System.Windows.Forms.ComboBox comboBox_Send;
        private System.Windows.Forms.TextBox txt_SendCMD;
        private System.Windows.Forms.Label lbl_ScriptFileName;
        private Sunny.UI.UISymbolButton btn_StartScript;
        private Sunny.UI.UISymbolButton btn_OpenScript;
    }
}