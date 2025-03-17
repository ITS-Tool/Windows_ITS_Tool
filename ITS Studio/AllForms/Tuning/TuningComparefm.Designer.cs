namespace ITS_Studio
{
    partial class TuningComparefm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TuningComparefm));
            this.DGV_CompareFile_R = new System.Windows.Forms.DataGridView();
            this.Column_PageIndex_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_ItemIndex_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Max_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Min_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Address_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Page_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Item_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Value_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Offset_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Format_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LBL_HexInfo_Right = new System.Windows.Forms.Label();
            this.TBOX_FilePath_Right = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TSB_NewCompare = new System.Windows.Forms.ToolStripButton();
            this.TSB_ExportMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.TSB_ExportLeftHexFile = new System.Windows.Forms.ToolStripMenuItem();
            this.TSB_ExportRightHexFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.OFD_OpenHexFile = new System.Windows.Forms.OpenFileDialog();
            this.SFD_SaveHexFile = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer_Left = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.TBOX_FilePath_Left = new System.Windows.Forms.TextBox();
            this.BTN_OpenFileDialog_L = new System.Windows.Forms.Button();
            this.btn_cp_toRight = new System.Windows.Forms.Button();
            this.LBL_HexInfo_Left = new System.Windows.Forms.Label();
            this.DGV_CompareFile_L = new System.Windows.Forms.DataGridView();
            this.Column_PageIndex_L = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_ItemIndex_L = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Max_L = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Min_L = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Address_L = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Page_L = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Item_L = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Value_L = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Offset_L = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Format_L = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer_Right = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.BTN_OpenFileDialog_R = new System.Windows.Forms.Button();
            this.btn_cp_toLeft = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_mode_select = new System.Windows.Forms.ComboBox();
            this.gb_ModeSel = new System.Windows.Forms.GroupBox();
            this.gb_Find = new System.Windows.Forms.GroupBox();
            this.btn_filter = new System.Windows.Forms.Button();
            this.btn_search = new System.Windows.Forms.Button();
            this.tb_find_string = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_CompareFile_R)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Left)).BeginInit();
            this.splitContainer_Left.Panel1.SuspendLayout();
            this.splitContainer_Left.Panel2.SuspendLayout();
            this.splitContainer_Left.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_CompareFile_L)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Right)).BeginInit();
            this.splitContainer_Right.Panel1.SuspendLayout();
            this.splitContainer_Right.Panel2.SuspendLayout();
            this.splitContainer_Right.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gb_ModeSel.SuspendLayout();
            this.gb_Find.SuspendLayout();
            this.SuspendLayout();
            // 
            // DGV_CompareFile_R
            // 
            this.DGV_CompareFile_R.AllowUserToAddRows = false;
            this.DGV_CompareFile_R.AllowUserToDeleteRows = false;
            this.DGV_CompareFile_R.AllowUserToResizeColumns = false;
            this.DGV_CompareFile_R.AllowUserToResizeRows = false;
            this.DGV_CompareFile_R.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_CompareFile_R.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.DGV_CompareFile_R.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_CompareFile_R.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_PageIndex_R,
            this.Column_ItemIndex_R,
            this.Column_Max_R,
            this.Column_Min_R,
            this.Column_Address_R,
            this.Column_Page_R,
            this.Column_Item_R,
            this.Column_Value_R,
            this.Column_Offset_R,
            this.Column_Format_R});
            this.DGV_CompareFile_R.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_CompareFile_R.Location = new System.Drawing.Point(0, 0);
            this.DGV_CompareFile_R.Margin = new System.Windows.Forms.Padding(4);
            this.DGV_CompareFile_R.MultiSelect = false;
            this.DGV_CompareFile_R.Name = "DGV_CompareFile_R";
            this.DGV_CompareFile_R.RowHeadersVisible = false;
            this.DGV_CompareFile_R.RowTemplate.Height = 24;
            this.DGV_CompareFile_R.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_CompareFile_R.Size = new System.Drawing.Size(367, 237);
            this.DGV_CompareFile_R.TabIndex = 6;
            this.DGV_CompareFile_R.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellEnter);
            this.DGV_CompareFile_R.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_CellMouseDown);
            this.DGV_CompareFile_R.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DGV_CellValidating);
            this.DGV_CompareFile_R.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellValueChanged);
            this.DGV_CompareFile_R.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_CompareFile_R_DataError);
            // 
            // Column_PageIndex_R
            // 
            this.Column_PageIndex_R.HeaderText = "PageIndex";
            this.Column_PageIndex_R.Name = "Column_PageIndex_R";
            this.Column_PageIndex_R.ReadOnly = true;
            this.Column_PageIndex_R.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_PageIndex_R.Visible = false;
            // 
            // Column_ItemIndex_R
            // 
            this.Column_ItemIndex_R.HeaderText = "ItemIndex";
            this.Column_ItemIndex_R.Name = "Column_ItemIndex_R";
            this.Column_ItemIndex_R.ReadOnly = true;
            this.Column_ItemIndex_R.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_ItemIndex_R.Visible = false;
            // 
            // Column_Max_R
            // 
            this.Column_Max_R.HeaderText = "Max";
            this.Column_Max_R.Name = "Column_Max_R";
            this.Column_Max_R.Visible = false;
            // 
            // Column_Min_R
            // 
            this.Column_Min_R.HeaderText = "Min";
            this.Column_Min_R.Name = "Column_Min_R";
            this.Column_Min_R.Visible = false;
            // 
            // Column_Address_R
            // 
            this.Column_Address_R.HeaderText = "Address";
            this.Column_Address_R.Name = "Column_Address_R";
            this.Column_Address_R.ReadOnly = true;
            // 
            // Column_Page_R
            // 
            this.Column_Page_R.HeaderText = "Group";
            this.Column_Page_R.Name = "Column_Page_R";
            this.Column_Page_R.ReadOnly = true;
            this.Column_Page_R.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Item_R
            // 
            this.Column_Item_R.HeaderText = "Item";
            this.Column_Item_R.Name = "Column_Item_R";
            this.Column_Item_R.ReadOnly = true;
            this.Column_Item_R.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Value_R
            // 
            this.Column_Value_R.HeaderText = "Value";
            this.Column_Value_R.Name = "Column_Value_R";
            this.Column_Value_R.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Offset_R
            // 
            this.Column_Offset_R.HeaderText = "Offset";
            this.Column_Offset_R.Name = "Column_Offset_R";
            this.Column_Offset_R.Visible = false;
            // 
            // Column_Format_R
            // 
            this.Column_Format_R.HeaderText = "Format";
            this.Column_Format_R.Name = "Column_Format_R";
            this.Column_Format_R.Visible = false;
            // 
            // LBL_HexInfo2
            // 
            this.LBL_HexInfo_Right.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LBL_HexInfo_Right.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_HexInfo_Right.Location = new System.Drawing.Point(0, -5);
            this.LBL_HexInfo_Right.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LBL_HexInfo_Right.Name = "LBL_HexInfo2";
            this.LBL_HexInfo_Right.Size = new System.Drawing.Size(243, 33);
            this.LBL_HexInfo_Right.TabIndex = 5;
            this.LBL_HexInfo_Right.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TBOX_FilePath2
            // 
            this.TBOX_FilePath_Right.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBOX_FilePath_Right.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TBOX_FilePath_Right.Location = new System.Drawing.Point(4, 4);
            this.TBOX_FilePath_Right.Margin = new System.Windows.Forms.Padding(4);
            this.TBOX_FilePath_Right.Multiline = true;
            this.TBOX_FilePath_Right.Name = "TBOX_FilePath2";
            this.TBOX_FilePath_Right.ReadOnly = true;
            this.TBOX_FilePath_Right.Size = new System.Drawing.Size(243, 32);
            this.TBOX_FilePath_Right.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSB_NewCompare,
            this.TSB_ExportMenu,
            this.toolStripButton1});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(738, 46);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TSB_NewCompare
            // 
            this.TSB_NewCompare.Image = ((System.Drawing.Image)(resources.GetObject("TSB_NewCompare.Image")));
            this.TSB_NewCompare.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.TSB_NewCompare.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.TSB_NewCompare.Name = "TSB_NewCompare";
            this.TSB_NewCompare.Size = new System.Drawing.Size(125, 43);
            this.TSB_NewCompare.Text = "New Compare";
            this.TSB_NewCompare.ToolTipText = "New Comparison";
            this.TSB_NewCompare.Click += new System.EventHandler(this.TSB_NewCompare_Click);
            // 
            // TSB_ExportMenu
            // 
            this.TSB_ExportMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSB_ExportLeftHexFile,
            this.TSB_ExportRightHexFile});
            this.TSB_ExportMenu.Image = ((System.Drawing.Image)(resources.GetObject("TSB_ExportMenu.Image")));
            this.TSB_ExportMenu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.TSB_ExportMenu.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.TSB_ExportMenu.Name = "TSB_ExportMenu";
            this.TSB_ExportMenu.Size = new System.Drawing.Size(111, 43);
            this.TSB_ExportMenu.Text = "Export File";
            this.TSB_ExportMenu.ToolTipText = "Export Hex File";
            // 
            // TSB_ExportLeftHexFile
            // 
            this.TSB_ExportLeftHexFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.TSB_ExportLeftHexFile.Enabled = false;
            this.TSB_ExportLeftHexFile.Image = global::ITS_Studio.Properties.Resources.Documents_icon16_16;
            this.TSB_ExportLeftHexFile.Name = "TSB_ExportLeftHexFile";
            this.TSB_ExportLeftHexFile.Size = new System.Drawing.Size(161, 22);
            this.TSB_ExportLeftHexFile.Text = "<- Export Left";
            this.TSB_ExportLeftHexFile.Click += new System.EventHandler(this.TSB_ExportHexFile_Click);
            // 
            // TSB_ExportRightHexFile
            // 
            this.TSB_ExportRightHexFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.TSB_ExportRightHexFile.Enabled = false;
            this.TSB_ExportRightHexFile.Image = global::ITS_Studio.Properties.Resources.Documents_icon16_16;
            this.TSB_ExportRightHexFile.Name = "TSB_ExportRightHexFile";
            this.TSB_ExportRightHexFile.Size = new System.Drawing.Size(161, 22);
            this.TSB_ExportRightHexFile.Text = "Export Right ->";
            this.TSB_ExportRightHexFile.Click += new System.EventHandler(this.TSB_ExportHexFile_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::ITS_Studio.Properties.Resources.Arrow_Back_icon_32_32;
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(80, 43);
            this.toolStripButton1.Text = "Return";
            this.toolStripButton1.ToolTipText = "Exit Application";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // OFD_OpenHexFile
            // 
            this.OFD_OpenHexFile.Filter = "Intel Hex File|*.hex";
            this.OFD_OpenHexFile.Title = "Select Hex File";
            // 
            // SFD_SaveHexFile
            // 
            this.SFD_SaveHexFile.DefaultExt = "hex";
            this.SFD_SaveHexFile.Filter = "Intel Hex File|*.hex";
            this.SFD_SaveHexFile.Title = "Save Hex File";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer_Left);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer_Right);
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(738, 317);
            this.splitContainer1.SplitterDistance = 370;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 4;
            // 
            // splitContainer_Left
            // 
            this.splitContainer_Left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Left.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_Left.IsSplitterFixed = true;
            this.splitContainer_Left.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Left.Name = "splitContainer_Left";
            this.splitContainer_Left.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Left.Panel1
            // 
            this.splitContainer_Left.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer_Left.Panel2
            // 
            this.splitContainer_Left.Panel2.Controls.Add(this.DGV_CompareFile_L);
            this.splitContainer_Left.Size = new System.Drawing.Size(370, 317);
            this.splitContainer_Left.SplitterDistance = 76;
            this.splitContainer_Left.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.TBOX_FilePath_Left);
            this.splitContainer2.Panel1.Controls.Add(this.BTN_OpenFileDialog_L);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btn_cp_toRight);
            this.splitContainer2.Panel2.Controls.Add(this.LBL_HexInfo_Left);
            this.splitContainer2.Size = new System.Drawing.Size(370, 76);
            this.splitContainer2.SplitterDistance = 43;
            this.splitContainer2.TabIndex = 5;
            // 
            // TBOX_FilePath1
            // 
            this.TBOX_FilePath_Left.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBOX_FilePath_Left.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TBOX_FilePath_Left.Location = new System.Drawing.Point(4, 4);
            this.TBOX_FilePath_Left.Margin = new System.Windows.Forms.Padding(4);
            this.TBOX_FilePath_Left.Multiline = true;
            this.TBOX_FilePath_Left.Name = "TBOX_FilePath1";
            this.TBOX_FilePath_Left.ReadOnly = true;
            this.TBOX_FilePath_Left.Size = new System.Drawing.Size(243, 32);
            this.TBOX_FilePath_Left.TabIndex = 0;
            // 
            // BTN_OpenFileDialog1
            // 
            this.BTN_OpenFileDialog_L.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_OpenFileDialog_L.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BTN_OpenFileDialog_L.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BTN_OpenFileDialog_L.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_OpenFileDialog_L.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_OpenFileDialog_L.Image = global::ITS_Studio.Properties.Resources.Actions_list_add_icon;
            this.BTN_OpenFileDialog_L.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BTN_OpenFileDialog_L.Location = new System.Drawing.Point(255, 4);
            this.BTN_OpenFileDialog_L.Margin = new System.Windows.Forms.Padding(4);
            this.BTN_OpenFileDialog_L.Name = "BTN_OpenFileDialog1";
            this.BTN_OpenFileDialog_L.Size = new System.Drawing.Size(112, 32);
            this.BTN_OpenFileDialog_L.TabIndex = 1;
            this.BTN_OpenFileDialog_L.Text = "Open File";
            this.BTN_OpenFileDialog_L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_OpenFileDialog_L.UseVisualStyleBackColor = true;
            this.BTN_OpenFileDialog_L.Click += new System.EventHandler(this.BTN_OpenFileDialog_Click);
            // 
            // btn_cp_toRight
            // 
            this.btn_cp_toRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_cp_toRight.Image = global::ITS_Studio.Properties.Resources.Actions_go_next_icon;
            this.btn_cp_toRight.Location = new System.Drawing.Point(338, 0);
            this.btn_cp_toRight.Name = "btn_cp_toRight";
            this.btn_cp_toRight.Size = new System.Drawing.Size(32, 29);
            this.btn_cp_toRight.TabIndex = 5;
            this.btn_cp_toRight.UseVisualStyleBackColor = true;
            this.btn_cp_toRight.Click += new System.EventHandler(this.btn_cp_toRight_Click);
            // 
            // LBL_HexInfo1
            // 
            this.LBL_HexInfo_Left.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LBL_HexInfo_Left.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_HexInfo_Left.Location = new System.Drawing.Point(3, -5);
            this.LBL_HexInfo_Left.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LBL_HexInfo_Left.Name = "LBL_HexInfo1";
            this.LBL_HexInfo_Left.Size = new System.Drawing.Size(243, 33);
            this.LBL_HexInfo_Left.TabIndex = 4;
            this.LBL_HexInfo_Left.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DGV_CompareFile_L
            // 
            this.DGV_CompareFile_L.AllowUserToAddRows = false;
            this.DGV_CompareFile_L.AllowUserToDeleteRows = false;
            this.DGV_CompareFile_L.AllowUserToResizeColumns = false;
            this.DGV_CompareFile_L.AllowUserToResizeRows = false;
            this.DGV_CompareFile_L.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_CompareFile_L.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.DGV_CompareFile_L.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_CompareFile_L.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_PageIndex_L,
            this.Column_ItemIndex_L,
            this.Column_Max_L,
            this.Column_Min_L,
            this.Column_Address_L,
            this.Column_Page_L,
            this.Column_Item_L,
            this.Column_Value_L,
            this.Column_Offset_L,
            this.Column_Format_L});
            this.DGV_CompareFile_L.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_CompareFile_L.Location = new System.Drawing.Point(0, 0);
            this.DGV_CompareFile_L.Margin = new System.Windows.Forms.Padding(4);
            this.DGV_CompareFile_L.MultiSelect = false;
            this.DGV_CompareFile_L.Name = "DGV_CompareFile_L";
            this.DGV_CompareFile_L.RowHeadersVisible = false;
            this.DGV_CompareFile_L.RowTemplate.Height = 24;
            this.DGV_CompareFile_L.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_CompareFile_L.Size = new System.Drawing.Size(370, 237);
            this.DGV_CompareFile_L.TabIndex = 3;
            this.DGV_CompareFile_L.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellEnter);
            this.DGV_CompareFile_L.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_CellMouseDown);
            this.DGV_CompareFile_L.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DGV_CellValidating);
            this.DGV_CompareFile_L.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellValueChanged);
            this.DGV_CompareFile_L.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_CompareFile_L_DataError);
            // 
            // Column_PageIndex_L
            // 
            this.Column_PageIndex_L.HeaderText = "PageIndex";
            this.Column_PageIndex_L.Name = "Column_PageIndex_L";
            this.Column_PageIndex_L.ReadOnly = true;
            this.Column_PageIndex_L.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_PageIndex_L.Visible = false;
            // 
            // Column_ItemIndex_L
            // 
            this.Column_ItemIndex_L.HeaderText = "ItemIndex";
            this.Column_ItemIndex_L.Name = "Column_ItemIndex_L";
            this.Column_ItemIndex_L.ReadOnly = true;
            this.Column_ItemIndex_L.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_ItemIndex_L.Visible = false;
            // 
            // Column_Max_L
            // 
            this.Column_Max_L.HeaderText = "Max";
            this.Column_Max_L.Name = "Column_Max_L";
            this.Column_Max_L.Visible = false;
            // 
            // Column_Min_L
            // 
            this.Column_Min_L.HeaderText = "Min";
            this.Column_Min_L.Name = "Column_Min_L";
            this.Column_Min_L.Visible = false;
            // 
            // Column_Address_L
            // 
            this.Column_Address_L.HeaderText = "Address";
            this.Column_Address_L.Name = "Column_Address_L";
            this.Column_Address_L.ReadOnly = true;
            // 
            // Column_Page_L
            // 
            this.Column_Page_L.HeaderText = "Group";
            this.Column_Page_L.Name = "Column_Page_L";
            this.Column_Page_L.ReadOnly = true;
            this.Column_Page_L.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Item_L
            // 
            this.Column_Item_L.HeaderText = "Item";
            this.Column_Item_L.Name = "Column_Item_L";
            this.Column_Item_L.ReadOnly = true;
            this.Column_Item_L.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Value_L
            // 
            this.Column_Value_L.HeaderText = "Value";
            this.Column_Value_L.Name = "Column_Value_L";
            this.Column_Value_L.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Offset_L
            // 
            this.Column_Offset_L.HeaderText = "Offset";
            this.Column_Offset_L.Name = "Column_Offset_L";
            this.Column_Offset_L.Visible = false;
            // 
            // Column_Format_L
            // 
            this.Column_Format_L.HeaderText = "Format";
            this.Column_Format_L.Name = "Column_Format_L";
            this.Column_Format_L.Visible = false;
            // 
            // splitContainer_Right
            // 
            this.splitContainer_Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Right.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_Right.IsSplitterFixed = true;
            this.splitContainer_Right.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Right.Name = "splitContainer_Right";
            this.splitContainer_Right.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Right.Panel1
            // 
            this.splitContainer_Right.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer_Right.Panel2
            // 
            this.splitContainer_Right.Panel2.Controls.Add(this.DGV_CompareFile_R);
            this.splitContainer_Right.Size = new System.Drawing.Size(367, 317);
            this.splitContainer_Right.SplitterDistance = 76;
            this.splitContainer_Right.TabIndex = 7;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.TBOX_FilePath_Right);
            this.splitContainer3.Panel1.Controls.Add(this.BTN_OpenFileDialog_R);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.btn_cp_toLeft);
            this.splitContainer3.Panel2.Controls.Add(this.LBL_HexInfo_Right);
            this.splitContainer3.Size = new System.Drawing.Size(367, 76);
            this.splitContainer3.SplitterDistance = 45;
            this.splitContainer3.TabIndex = 6;
            // 
            // BTN_OpenFileDialog2
            // 
            this.BTN_OpenFileDialog_R.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_OpenFileDialog_R.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BTN_OpenFileDialog_R.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BTN_OpenFileDialog_R.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_OpenFileDialog_R.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_OpenFileDialog_R.Image = global::ITS_Studio.Properties.Resources.Actions_list_add_icon;
            this.BTN_OpenFileDialog_R.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BTN_OpenFileDialog_R.Location = new System.Drawing.Point(255, 4);
            this.BTN_OpenFileDialog_R.Margin = new System.Windows.Forms.Padding(4);
            this.BTN_OpenFileDialog_R.Name = "BTN_OpenFileDialog2";
            this.BTN_OpenFileDialog_R.Size = new System.Drawing.Size(112, 32);
            this.BTN_OpenFileDialog_R.TabIndex = 2;
            this.BTN_OpenFileDialog_R.Text = "Open File";
            this.BTN_OpenFileDialog_R.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_OpenFileDialog_R.UseVisualStyleBackColor = true;
            this.BTN_OpenFileDialog_R.Click += new System.EventHandler(this.BTN_OpenFileDialog_Click);
            // 
            // btn_cp_toLeft
            // 
            this.btn_cp_toLeft.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_cp_toLeft.Image = global::ITS_Studio.Properties.Resources.Actions_go_previous_icon;
            this.btn_cp_toLeft.Location = new System.Drawing.Point(335, 0);
            this.btn_cp_toLeft.Name = "btn_cp_toLeft";
            this.btn_cp_toLeft.Size = new System.Drawing.Size(32, 27);
            this.btn_cp_toLeft.TabIndex = 6;
            this.btn_cp_toLeft.UseVisualStyleBackColor = true;
            this.btn_cp_toLeft.Click += new System.EventHandler(this.btn_cp_toLeft_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(738, 317);
            this.panel1.TabIndex = 7;
            // 
            // cb_mode_select
            // 
            this.cb_mode_select.Font = new System.Drawing.Font("新細明體", 8F);
            this.cb_mode_select.FormattingEnabled = true;
            this.cb_mode_select.ItemHeight = 11;
            this.cb_mode_select.Items.AddRange(new object[] {
            "0",
            "1",
            "2"});
            this.cb_mode_select.Location = new System.Drawing.Point(12, 20);
            this.cb_mode_select.Name = "cb_mode_select";
            this.cb_mode_select.Size = new System.Drawing.Size(100, 19);
            this.cb_mode_select.TabIndex = 9;
            this.cb_mode_select.Text = "0";
            this.cb_mode_select.Visible = false;
            this.cb_mode_select.SelectedIndexChanged += new System.EventHandler(this.cb_mode_select_SelectedIndexChanged);
            // 
            // gb_ModeSel
            // 
            this.gb_ModeSel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.gb_ModeSel.Controls.Add(this.cb_mode_select);
            this.gb_ModeSel.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.gb_ModeSel.Location = new System.Drawing.Point(326, 0);
            this.gb_ModeSel.Name = "gb_ModeSel";
            this.gb_ModeSel.Size = new System.Drawing.Size(118, 46);
            this.gb_ModeSel.TabIndex = 11;
            this.gb_ModeSel.TabStop = false;
            this.gb_ModeSel.Text = "ModeSel";
            this.gb_ModeSel.Visible = false;
            // 
            // gb_Find
            // 
            this.gb_Find.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Find.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.gb_Find.Controls.Add(this.btn_filter);
            this.gb_Find.Controls.Add(this.btn_search);
            this.gb_Find.Controls.Add(this.tb_find_string);
            this.gb_Find.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.gb_Find.Location = new System.Drawing.Point(450, 0);
            this.gb_Find.Name = "gb_Find";
            this.gb_Find.Size = new System.Drawing.Size(283, 46);
            this.gb_Find.TabIndex = 12;
            this.gb_Find.TabStop = false;
            // 
            // btn_filter
            // 
            this.btn_filter.Location = new System.Drawing.Point(165, 16);
            this.btn_filter.Name = "btn_filter";
            this.btn_filter.Size = new System.Drawing.Size(53, 23);
            this.btn_filter.TabIndex = 2;
            this.btn_filter.Text = "Find";
            this.btn_filter.UseVisualStyleBackColor = true;
            this.btn_filter.Click += new System.EventHandler(this.btn_filter_Click);
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(224, 16);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(53, 23);
            this.btn_search.TabIndex = 1;
            this.btn_search.Text = "Return";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // tb_find_string
            // 
            this.tb_find_string.Location = new System.Drawing.Point(6, 16);
            this.tb_find_string.Name = "tb_find_string";
            this.tb_find_string.Size = new System.Drawing.Size(158, 23);
            this.tb_find_string.TabIndex = 0;
            // 
            // TuningComparefm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(738, 363);
            this.Controls.Add(this.gb_Find);
            this.Controls.Add(this.gb_ModeSel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TuningComparefm";
            this.Text = "TuningCompare";
            this.VisibleChanged += new System.EventHandler(this.TuningCompare_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_CompareFile_R)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer_Left.Panel1.ResumeLayout(false);
            this.splitContainer_Left.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Left)).EndInit();
            this.splitContainer_Left.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_CompareFile_L)).EndInit();
            this.splitContainer_Right.Panel1.ResumeLayout(false);
            this.splitContainer_Right.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Right)).EndInit();
            this.splitContainer_Right.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.gb_ModeSel.ResumeLayout(false);
            this.gb_Find.ResumeLayout(false);
            this.gb_Find.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_CompareFile_R;
        private System.Windows.Forms.DataGridView DGV_CompareFile_L;
        private System.Windows.Forms.Label LBL_HexInfo_Right;
        private System.Windows.Forms.Button BTN_OpenFileDialog_R;
        private System.Windows.Forms.TextBox TBOX_FilePath_Right;
        private System.Windows.Forms.ToolStripMenuItem TSB_ExportRightHexFile;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton TSB_NewCompare;
        private System.Windows.Forms.ToolStripDropDownButton TSB_ExportMenu;
        private System.Windows.Forms.ToolStripMenuItem TSB_ExportLeftHexFile;
        private System.Windows.Forms.OpenFileDialog OFD_OpenHexFile;
        private System.Windows.Forms.SaveFileDialog SFD_SaveHexFile;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox TBOX_FilePath_Left;
        private System.Windows.Forms.Button BTN_OpenFileDialog_L;
        private System.Windows.Forms.Label LBL_HexInfo_Left;
        private System.Windows.Forms.SplitContainer splitContainer_Left;
        private System.Windows.Forms.SplitContainer splitContainer_Right;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btn_cp_toRight;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button btn_cp_toLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PageIndex_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_ItemIndex_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Max_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Min_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Address_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Page_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Item_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Value_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Offset_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Format_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PageIndex_L;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_ItemIndex_L;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Max_L;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Min_L;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Address_L;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Page_L;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Item_L;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Value_L;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Offset_L;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Format_L;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ComboBox cb_mode_select;
        private System.Windows.Forms.GroupBox gb_ModeSel;
        private System.Windows.Forms.GroupBox gb_Find;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.TextBox tb_find_string;
        private System.Windows.Forms.Button btn_filter;

    }
}