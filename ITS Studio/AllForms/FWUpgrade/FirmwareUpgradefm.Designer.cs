namespace ITS_Studio.AllForms
{
    partial class FirmwareUpgradefm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirmwareUpgradefm));
            this.richTB_Result = new System.Windows.Forms.RichTextBox();
            this.lab_1 = new System.Windows.Forms.Label();
            this.m_gpMemoryFlash = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lblSendingFile = new Sunny.UI.UIMarkLabel();
            this.uiCheckBoxGroup1 = new Sunny.UI.UICheckBoxGroup();
            this.progressBar1 = new Sunny.UI.UIProcessBar();
            this.m_picSpinner = new System.Windows.Forms.PictureBox();
            this.cbBox_FilePath = new Sunny.UI.UIComboBox();
            this.btnStartProgram = new Sunny.UI.UIImageButton();
            this.btnSelectFile = new Sunny.UI.UIImageButton();
            this.lb_StressBuild = new Sunny.UI.UIMarkLabel();
            this.Panel_WaitUI = new System.Windows.Forms.Panel();
            this.m_gpMemoryFlash.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_picSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStartProgram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectFile)).BeginInit();
            this.SuspendLayout();
            // 
            // richTB_Result
            // 
            this.richTB_Result.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.richTB_Result.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTB_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTB_Result.Location = new System.Drawing.Point(3, 18);
            this.richTB_Result.Name = "richTB_Result";
            this.richTB_Result.ReadOnly = true;
            this.richTB_Result.Size = new System.Drawing.Size(540, 562);
            this.richTB_Result.TabIndex = 13;
            this.richTB_Result.Text = "";
            this.richTB_Result.TextChanged += new System.EventHandler(this.richTB_Result_TextChanged);
            // 
            // lab_1
            // 
            this.lab_1.Location = new System.Drawing.Point(3, 9);
            this.lab_1.Name = "lab_1";
            this.lab_1.Size = new System.Drawing.Size(70, 15);
            this.lab_1.TabIndex = 0;
            this.lab_1.Text = "File Path";
            // 
            // m_gpMemoryFlash
            // 
            this.m_gpMemoryFlash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.m_gpMemoryFlash.Controls.Add(this.richTB_Result);
            this.m_gpMemoryFlash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_gpMemoryFlash.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_gpMemoryFlash.Location = new System.Drawing.Point(0, 0);
            this.m_gpMemoryFlash.Name = "m_gpMemoryFlash";
            this.m_gpMemoryFlash.Size = new System.Drawing.Size(546, 583);
            this.m_gpMemoryFlash.TabIndex = 2;
            this.m_gpMemoryFlash.TabStop = false;
            this.m_gpMemoryFlash.Text = "Message";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.splitContainer1.Panel2.Controls.Add(this.m_gpMemoryFlash);
            this.splitContainer1.Size = new System.Drawing.Size(1018, 583);
            this.splitContainer1.SplitterDistance = 468;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.splitContainer2.Panel1.Controls.Add(this.lblSendingFile);
            this.splitContainer2.Panel1.Controls.Add(this.uiCheckBoxGroup1);
            this.splitContainer2.Panel1.Controls.Add(this.progressBar1);
            this.splitContainer2.Panel1.Controls.Add(this.m_picSpinner);
            this.splitContainer2.Panel1.Controls.Add(this.cbBox_FilePath);
            this.splitContainer2.Panel1.Controls.Add(this.btnStartProgram);
            this.splitContainer2.Panel1.Controls.Add(this.btnSelectFile);
            this.splitContainer2.Panel1.Controls.Add(this.lab_1);
            this.splitContainer2.Panel1.Controls.Add(this.lb_StressBuild);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.Panel_WaitUI);
            this.splitContainer2.Size = new System.Drawing.Size(468, 583);
            this.splitContainer2.SplitterDistance = 209;
            this.splitContainer2.TabIndex = 21;
            // 
            // lblSendingFile
            // 
            this.lblSendingFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.lblSendingFile.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.lblSendingFile.Location = new System.Drawing.Point(12, 59);
            this.lblSendingFile.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.lblSendingFile.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.lblSendingFile.Name = "lblSendingFile";
            this.lblSendingFile.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblSendingFile.Size = new System.Drawing.Size(452, 23);
            this.lblSendingFile.Style = Sunny.UI.UIStyle.Orange;
            this.lblSendingFile.TabIndex = 31;
            this.lblSendingFile.Text = "Sending File...";
            this.lblSendingFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSendingFile.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiCheckBoxGroup1
            // 
            this.uiCheckBoxGroup1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiCheckBoxGroup1.ColumnCount = 2;
            this.uiCheckBoxGroup1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiCheckBoxGroup1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.uiCheckBoxGroup1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.uiCheckBoxGroup1.Location = new System.Drawing.Point(9, 85);
            this.uiCheckBoxGroup1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiCheckBoxGroup1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiCheckBoxGroup1.Name = "uiCheckBoxGroup1";
            this.uiCheckBoxGroup1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiCheckBoxGroup1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.uiCheckBoxGroup1.SelectedIndexes = ((System.Collections.Generic.List<int>)(resources.GetObject("uiCheckBoxGroup1.SelectedIndexes")));
            this.uiCheckBoxGroup1.Size = new System.Drawing.Size(332, 92);
            this.uiCheckBoxGroup1.Style = Sunny.UI.UIStyle.Orange;
            this.uiCheckBoxGroup1.TabIndex = 30;
            this.uiCheckBoxGroup1.Text = "Advanced Features";
            this.uiCheckBoxGroup1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiCheckBoxGroup1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiCheckBoxGroup1.ValueChanged += new Sunny.UI.UICheckBoxGroup.OnValueChanged(this.uiCheckBoxGroup1_ValueChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.DecimalPlaces = 0;
            this.progressBar1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.progressBar1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.progressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.progressBar1.Location = new System.Drawing.Point(89, 32);
            this.progressBar1.MinimumSize = new System.Drawing.Size(70, 5);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Radius = 10;
            this.progressBar1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.progressBar1.Size = new System.Drawing.Size(375, 23);
            this.progressBar1.Style = Sunny.UI.UIStyle.Orange;
            this.progressBar1.TabIndex = 26;
            this.progressBar1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // m_picSpinner
            // 
            this.m_picSpinner.Image = global::ITS_Studio.Properties.Resources.ajax_loader;
            this.m_picSpinner.Location = new System.Drawing.Point(12, 32);
            this.m_picSpinner.Name = "m_picSpinner";
            this.m_picSpinner.Size = new System.Drawing.Size(24, 24);
            this.m_picSpinner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.m_picSpinner.TabIndex = 15;
            this.m_picSpinner.TabStop = false;
            this.m_picSpinner.Visible = false;
            // 
            // cbBox_FilePath
            // 
            this.cbBox_FilePath.BackColor = System.Drawing.Color.Transparent;
            this.cbBox_FilePath.DataSource = null;
            this.cbBox_FilePath.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cbBox_FilePath.FillColor = System.Drawing.Color.White;
            this.cbBox_FilePath.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.cbBox_FilePath.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbBox_FilePath.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(235)))), ((int)(((byte)(212)))));
            this.cbBox_FilePath.ItemRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_FilePath.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_FilePath.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(249)))), ((int)(((byte)(241)))));
            this.cbBox_FilePath.Location = new System.Drawing.Point(89, 5);
            this.cbBox_FilePath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbBox_FilePath.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbBox_FilePath.Name = "cbBox_FilePath";
            this.cbBox_FilePath.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbBox_FilePath.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.cbBox_FilePath.Size = new System.Drawing.Size(375, 19);
            this.cbBox_FilePath.Style = Sunny.UI.UIStyle.Orange;
            this.cbBox_FilePath.TabIndex = 29;
            this.cbBox_FilePath.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbBox_FilePath.Watermark = "";
            this.cbBox_FilePath.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.cbBox_FilePath.SelectedIndexChanged += new System.EventHandler(this.cbBox_FilePath_1_SelectedIndexChanged);
            // 
            // btnStartProgram
            // 
            this.btnStartProgram.BackColor = System.Drawing.Color.Transparent;
            this.btnStartProgram.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStartProgram.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartProgram.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnStartProgram.Image = global::ITS_Studio.Properties.Resources.Play_Pressed_icon;
            this.btnStartProgram.ImageDisabled = global::ITS_Studio.Properties.Resources.Dark_Play_1_Pressed_icon_48_48;
            this.btnStartProgram.Location = new System.Drawing.Point(409, 98);
            this.btnStartProgram.Name = "btnStartProgram";
            this.btnStartProgram.Size = new System.Drawing.Size(55, 79);
            this.btnStartProgram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnStartProgram.TabIndex = 28;
            this.btnStartProgram.TabStop = false;
            this.btnStartProgram.Text = "Run";
            this.btnStartProgram.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStartProgram.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btnStartProgram.Click += new System.EventHandler(this.btnStartProgram_Click);
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSelectFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectFile.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSelectFile.Image = global::ITS_Studio.Properties.Resources.Documents_icon;
            this.btnSelectFile.ImageDisabled = global::ITS_Studio.Properties.Resources.Dark_Documents_icon;
            this.btnSelectFile.Location = new System.Drawing.Point(348, 98);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(55, 79);
            this.btnSelectFile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnSelectFile.TabIndex = 27;
            this.btnSelectFile.TabStop = false;
            this.btnSelectFile.Text = "Open";
            this.btnSelectFile.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSelectFile.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // lb_StressBuild
            // 
            this.lb_StressBuild.BackColor = System.Drawing.Color.Transparent;
            this.lb_StressBuild.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lb_StressBuild.Location = new System.Drawing.Point(5, 182);
            this.lb_StressBuild.MarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.lb_StressBuild.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.lb_StressBuild.Name = "lb_StressBuild";
            this.lb_StressBuild.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lb_StressBuild.Size = new System.Drawing.Size(100, 23);
            this.lb_StressBuild.Style = Sunny.UI.UIStyle.Orange;
            this.lb_StressBuild.TabIndex = 14;
            this.lb_StressBuild.Text = "StressBuild";
            this.lb_StressBuild.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_StressBuild.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // Panel_WaitUI
            // 
            this.Panel_WaitUI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.Panel_WaitUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_WaitUI.Location = new System.Drawing.Point(0, 0);
            this.Panel_WaitUI.Name = "Panel_WaitUI";
            this.Panel_WaitUI.Size = new System.Drawing.Size(468, 370);
            this.Panel_WaitUI.TabIndex = 20;
            // 
            // FirmwareUpgradefm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1018, 583);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FirmwareUpgradefm";
            this.Text = "FirmwareUpgradefm";
            this.m_gpMemoryFlash.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_picSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStartProgram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectFile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lab_1;
        private System.Windows.Forms.RichTextBox richTB_Result;
        private System.Windows.Forms.GroupBox m_gpMemoryFlash;
        private System.Windows.Forms.PictureBox m_picSpinner;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel Panel_WaitUI;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Sunny.UI.UIProcessBar progressBar1;
        private Sunny.UI.UIImageButton btnStartProgram;
        private Sunny.UI.UIImageButton btnSelectFile;
        private Sunny.UI.UIComboBox cbBox_FilePath;
        private Sunny.UI.UICheckBoxGroup uiCheckBoxGroup1;
        private Sunny.UI.UIMarkLabel lblSendingFile;
        private Sunny.UI.UIMarkLabel lb_StressBuild;
    }
}