namespace ITS_Studio.UserControls
{
    partial class Ctrl_TuningCompare
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Column_Min_Same = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Page_Same = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Item_Same = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Value_Same = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_OriginalRow_Same = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Format_Same = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GBOX_Additional = new System.Windows.Forms.GroupBox();
            this.DGV_AddList = new System.Windows.Forms.DataGridView();
            this.Column_PageIndex_Additional = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_ItemIndex_Additional = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Max_Additional = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Min_Additional = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Address_Additional = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Page_Additional = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Item_Additional = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Value_Additional = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_OriginalRow_Additional = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Format_Additional = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Address_Same = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Max_Same = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.GBOX_Different = new System.Windows.Forms.GroupBox();
            this.DGV_DiffList = new System.Windows.Forms.DataGridView();
            this.Column_PageIndex_Different = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_ItemIndex_Different = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Max_Different = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Min_Different = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Address_Different = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Page_Different = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Item_Different = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Value_Different = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_OriginalRow_Different = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Format_Different = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.GBOX_Same = new System.Windows.Forms.GroupBox();
            this.DGV_SameList = new System.Windows.Forms.DataGridView();
            this.Column_PageIndex_Same = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_ItemIndex_Same = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GBOX_Additional.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_AddList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.GBOX_Different.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_DiffList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.GBOX_Same.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_SameList)).BeginInit();
            this.SuspendLayout();
            // 
            // Column_Min_Same
            // 
            this.Column_Min_Same.HeaderText = "Min";
            this.Column_Min_Same.Name = "Column_Min_Same";
            this.Column_Min_Same.Visible = false;
            // 
            // Column_Page_Same
            // 
            this.Column_Page_Same.HeaderText = "Group";
            this.Column_Page_Same.Name = "Column_Page_Same";
            this.Column_Page_Same.ReadOnly = true;
            this.Column_Page_Same.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Item_Same
            // 
            this.Column_Item_Same.HeaderText = "Item";
            this.Column_Item_Same.Name = "Column_Item_Same";
            this.Column_Item_Same.ReadOnly = true;
            this.Column_Item_Same.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Value_Same
            // 
            this.Column_Value_Same.HeaderText = "Value";
            this.Column_Value_Same.Name = "Column_Value_Same";
            this.Column_Value_Same.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_OriginalRow_Same
            // 
            this.Column_OriginalRow_Same.HeaderText = "OriginalRow";
            this.Column_OriginalRow_Same.Name = "Column_OriginalRow_Same";
            this.Column_OriginalRow_Same.Visible = false;
            // 
            // Column_Format_Same
            // 
            this.Column_Format_Same.HeaderText = "Format";
            this.Column_Format_Same.Name = "Column_Format_Same";
            this.Column_Format_Same.Visible = false;
            // 
            // GBOX_Additional
            // 
            this.GBOX_Additional.BackColor = System.Drawing.Color.Yellow;
            this.GBOX_Additional.Controls.Add(this.DGV_AddList);
            this.GBOX_Additional.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GBOX_Additional.Location = new System.Drawing.Point(0, 0);
            this.GBOX_Additional.Margin = new System.Windows.Forms.Padding(4);
            this.GBOX_Additional.Name = "GBOX_Additional";
            this.GBOX_Additional.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.GBOX_Additional.Padding = new System.Windows.Forms.Padding(4);
            this.GBOX_Additional.Size = new System.Drawing.Size(438, 117);
            this.GBOX_Additional.TabIndex = 1;
            this.GBOX_Additional.TabStop = false;
            this.GBOX_Additional.Text = "Additional";
            // 
            // DGV_AddList
            // 
            this.DGV_AddList.AllowUserToAddRows = false;
            this.DGV_AddList.AllowUserToDeleteRows = false;
            this.DGV_AddList.AllowUserToResizeColumns = false;
            this.DGV_AddList.AllowUserToResizeRows = false;
            this.DGV_AddList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_AddList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_AddList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_PageIndex_Additional,
            this.Column_ItemIndex_Additional,
            this.Column_Max_Additional,
            this.Column_Min_Additional,
            this.Column_Address_Additional,
            this.Column_Page_Additional,
            this.Column_Item_Additional,
            this.Column_Value_Additional,
            this.Column_OriginalRow_Additional,
            this.Column_Format_Additional});
            this.DGV_AddList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_AddList.Location = new System.Drawing.Point(4, 19);
            this.DGV_AddList.Margin = new System.Windows.Forms.Padding(4);
            this.DGV_AddList.MultiSelect = false;
            this.DGV_AddList.Name = "DGV_AddList";
            this.DGV_AddList.RowHeadersVisible = false;
            this.DGV_AddList.RowTemplate.Height = 24;
            this.DGV_AddList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_AddList.Size = new System.Drawing.Size(430, 94);
            this.DGV_AddList.TabIndex = 5;
            this.DGV_AddList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_List_DataError);
            // 
            // Column_PageIndex_Additional
            // 
            this.Column_PageIndex_Additional.HeaderText = "PageIndex";
            this.Column_PageIndex_Additional.Name = "Column_PageIndex_Additional";
            this.Column_PageIndex_Additional.ReadOnly = true;
            this.Column_PageIndex_Additional.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_PageIndex_Additional.Visible = false;
            // 
            // Column_ItemIndex_Additional
            // 
            this.Column_ItemIndex_Additional.HeaderText = "ItemIndex";
            this.Column_ItemIndex_Additional.Name = "Column_ItemIndex_Additional";
            this.Column_ItemIndex_Additional.ReadOnly = true;
            this.Column_ItemIndex_Additional.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_ItemIndex_Additional.Visible = false;
            // 
            // Column_Max_Additional
            // 
            this.Column_Max_Additional.HeaderText = "Max";
            this.Column_Max_Additional.Name = "Column_Max_Additional";
            this.Column_Max_Additional.Visible = false;
            // 
            // Column_Min_Additional
            // 
            this.Column_Min_Additional.HeaderText = "Min";
            this.Column_Min_Additional.Name = "Column_Min_Additional";
            this.Column_Min_Additional.Visible = false;
            // 
            // Column_Address_Additional
            // 
            this.Column_Address_Additional.HeaderText = "Address";
            this.Column_Address_Additional.Name = "Column_Address_Additional";
            this.Column_Address_Additional.ReadOnly = true;
            // 
            // Column_Page_Additional
            // 
            this.Column_Page_Additional.HeaderText = "Group";
            this.Column_Page_Additional.Name = "Column_Page_Additional";
            this.Column_Page_Additional.ReadOnly = true;
            this.Column_Page_Additional.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Item_Additional
            // 
            this.Column_Item_Additional.HeaderText = "Item";
            this.Column_Item_Additional.Name = "Column_Item_Additional";
            this.Column_Item_Additional.ReadOnly = true;
            this.Column_Item_Additional.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Value_Additional
            // 
            this.Column_Value_Additional.HeaderText = "Value";
            this.Column_Value_Additional.Name = "Column_Value_Additional";
            this.Column_Value_Additional.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_OriginalRow_Additional
            // 
            this.Column_OriginalRow_Additional.HeaderText = "OriginalRow";
            this.Column_OriginalRow_Additional.Name = "Column_OriginalRow_Additional";
            this.Column_OriginalRow_Additional.Visible = false;
            // 
            // Column_Format_Additional
            // 
            this.Column_Format_Additional.HeaderText = "Format";
            this.Column_Format_Additional.Name = "Column_Format_Additional";
            this.Column_Format_Additional.Visible = false;
            // 
            // Column_Address_Same
            // 
            this.Column_Address_Same.HeaderText = "Address";
            this.Column_Address_Same.Name = "Column_Address_Same";
            this.Column_Address_Same.ReadOnly = true;
            // 
            // Column_Max_Same
            // 
            this.Column_Max_Same.HeaderText = "Max";
            this.Column_Max_Same.Name = "Column_Max_Same";
            this.Column_Max_Same.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.GBOX_Different);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(438, 408);
            this.splitContainer1.SplitterDistance = 141;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // GBOX_Different
            // 
            this.GBOX_Different.BackColor = System.Drawing.Color.DeepPink;
            this.GBOX_Different.Controls.Add(this.DGV_DiffList);
            this.GBOX_Different.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GBOX_Different.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.GBOX_Different.Location = new System.Drawing.Point(0, 0);
            this.GBOX_Different.Margin = new System.Windows.Forms.Padding(4);
            this.GBOX_Different.Name = "GBOX_Different";
            this.GBOX_Different.Padding = new System.Windows.Forms.Padding(4);
            this.GBOX_Different.Size = new System.Drawing.Size(438, 141);
            this.GBOX_Different.TabIndex = 0;
            this.GBOX_Different.TabStop = false;
            this.GBOX_Different.Text = "Different";
            // 
            // DGV_DiffList
            // 
            this.DGV_DiffList.AllowUserToAddRows = false;
            this.DGV_DiffList.AllowUserToDeleteRows = false;
            this.DGV_DiffList.AllowUserToResizeColumns = false;
            this.DGV_DiffList.AllowUserToResizeRows = false;
            this.DGV_DiffList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_DiffList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_DiffList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_PageIndex_Different,
            this.Column_ItemIndex_Different,
            this.Column_Max_Different,
            this.Column_Min_Different,
            this.Column_Address_Different,
            this.Column_Page_Different,
            this.Column_Item_Different,
            this.Column_Value_Different,
            this.Column_OriginalRow_Different,
            this.Column_Format_Different});
            this.DGV_DiffList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_DiffList.Location = new System.Drawing.Point(4, 24);
            this.DGV_DiffList.Margin = new System.Windows.Forms.Padding(4);
            this.DGV_DiffList.MultiSelect = false;
            this.DGV_DiffList.Name = "DGV_DiffList";
            this.DGV_DiffList.RowHeadersVisible = false;
            this.DGV_DiffList.RowTemplate.Height = 24;
            this.DGV_DiffList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_DiffList.Size = new System.Drawing.Size(430, 113);
            this.DGV_DiffList.TabIndex = 4;
            this.DGV_DiffList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_List_DataError);
            // 
            // Column_PageIndex_Different
            // 
            this.Column_PageIndex_Different.HeaderText = "PageIndex";
            this.Column_PageIndex_Different.Name = "Column_PageIndex_Different";
            this.Column_PageIndex_Different.ReadOnly = true;
            this.Column_PageIndex_Different.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_PageIndex_Different.Visible = false;
            // 
            // Column_ItemIndex_Different
            // 
            this.Column_ItemIndex_Different.HeaderText = "ItemIndex";
            this.Column_ItemIndex_Different.Name = "Column_ItemIndex_Different";
            this.Column_ItemIndex_Different.ReadOnly = true;
            this.Column_ItemIndex_Different.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_ItemIndex_Different.Visible = false;
            // 
            // Column_Max_Different
            // 
            this.Column_Max_Different.HeaderText = "Max";
            this.Column_Max_Different.Name = "Column_Max_Different";
            this.Column_Max_Different.Visible = false;
            // 
            // Column_Min_Different
            // 
            this.Column_Min_Different.HeaderText = "Min";
            this.Column_Min_Different.Name = "Column_Min_Different";
            this.Column_Min_Different.Visible = false;
            // 
            // Column_Address_Different
            // 
            this.Column_Address_Different.HeaderText = "Address";
            this.Column_Address_Different.Name = "Column_Address_Different";
            this.Column_Address_Different.ReadOnly = true;
            // 
            // Column_Page_Different
            // 
            this.Column_Page_Different.HeaderText = "Group";
            this.Column_Page_Different.Name = "Column_Page_Different";
            this.Column_Page_Different.ReadOnly = true;
            this.Column_Page_Different.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Item_Different
            // 
            this.Column_Item_Different.HeaderText = "Item";
            this.Column_Item_Different.Name = "Column_Item_Different";
            this.Column_Item_Different.ReadOnly = true;
            this.Column_Item_Different.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Value_Different
            // 
            this.Column_Value_Different.HeaderText = "Value";
            this.Column_Value_Different.Name = "Column_Value_Different";
            this.Column_Value_Different.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_OriginalRow_Different
            // 
            this.Column_OriginalRow_Different.HeaderText = "OriginalRow";
            this.Column_OriginalRow_Different.Name = "Column_OriginalRow_Different";
            this.Column_OriginalRow_Different.Visible = false;
            // 
            // Column_Format_Different
            // 
            this.Column_Format_Different.HeaderText = "Format";
            this.Column_Format_Different.Name = "Column_Format_Different";
            this.Column_Format_Different.Visible = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.GBOX_Same);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.GBOX_Additional);
            this.splitContainer2.Size = new System.Drawing.Size(438, 262);
            this.splitContainer2.SplitterDistance = 140;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // GBOX_Same
            // 
            this.GBOX_Same.BackColor = System.Drawing.Color.Lime;
            this.GBOX_Same.Controls.Add(this.DGV_SameList);
            this.GBOX_Same.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GBOX_Same.Location = new System.Drawing.Point(0, 0);
            this.GBOX_Same.Margin = new System.Windows.Forms.Padding(4);
            this.GBOX_Same.Name = "GBOX_Same";
            this.GBOX_Same.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.GBOX_Same.Padding = new System.Windows.Forms.Padding(4);
            this.GBOX_Same.Size = new System.Drawing.Size(438, 140);
            this.GBOX_Same.TabIndex = 1;
            this.GBOX_Same.TabStop = false;
            this.GBOX_Same.Text = "Same";
            // 
            // DGV_SameList
            // 
            this.DGV_SameList.AllowUserToAddRows = false;
            this.DGV_SameList.AllowUserToDeleteRows = false;
            this.DGV_SameList.AllowUserToResizeColumns = false;
            this.DGV_SameList.AllowUserToResizeRows = false;
            this.DGV_SameList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_SameList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_SameList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_PageIndex_Same,
            this.Column_ItemIndex_Same,
            this.Column_Max_Same,
            this.Column_Min_Same,
            this.Column_Address_Same,
            this.Column_Page_Same,
            this.Column_Item_Same,
            this.Column_Value_Same,
            this.Column_OriginalRow_Same,
            this.Column_Format_Same});
            this.DGV_SameList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_SameList.Location = new System.Drawing.Point(4, 19);
            this.DGV_SameList.Margin = new System.Windows.Forms.Padding(4);
            this.DGV_SameList.MultiSelect = false;
            this.DGV_SameList.Name = "DGV_SameList";
            this.DGV_SameList.RowHeadersVisible = false;
            this.DGV_SameList.RowTemplate.Height = 24;
            this.DGV_SameList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_SameList.Size = new System.Drawing.Size(430, 117);
            this.DGV_SameList.TabIndex = 5;
            this.DGV_SameList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_List_DataError);
            // 
            // Column_PageIndex_Same
            // 
            this.Column_PageIndex_Same.HeaderText = "PageIndex";
            this.Column_PageIndex_Same.Name = "Column_PageIndex_Same";
            this.Column_PageIndex_Same.ReadOnly = true;
            this.Column_PageIndex_Same.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_PageIndex_Same.Visible = false;
            // 
            // Column_ItemIndex_Same
            // 
            this.Column_ItemIndex_Same.HeaderText = "ItemIndex";
            this.Column_ItemIndex_Same.Name = "Column_ItemIndex_Same";
            this.Column_ItemIndex_Same.ReadOnly = true;
            this.Column_ItemIndex_Same.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_ItemIndex_Same.Visible = false;
            // 
            // Ctrl_TuningCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.splitContainer1);
            this.Name = "Ctrl_TuningCompare";
            this.Size = new System.Drawing.Size(438, 408);
            this.GBOX_Additional.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_AddList)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.GBOX_Different.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_DiffList)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.GBOX_Same.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_SameList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Min_Same;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Page_Same;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Item_Same;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Value_Same;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_OriginalRow_Same;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Format_Same;
        private System.Windows.Forms.GroupBox GBOX_Additional;
        public System.Windows.Forms.DataGridView DGV_AddList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PageIndex_Additional;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_ItemIndex_Additional;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Max_Additional;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Min_Additional;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Address_Additional;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Page_Additional;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Item_Additional;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Value_Additional;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_OriginalRow_Additional;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Format_Additional;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Address_Same;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Max_Same;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox GBOX_Different;
        public System.Windows.Forms.DataGridView DGV_DiffList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PageIndex_Different;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_ItemIndex_Different;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Max_Different;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Min_Different;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Address_Different;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Page_Different;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Item_Different;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Value_Different;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_OriginalRow_Different;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Format_Different;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox GBOX_Same;
        public System.Windows.Forms.DataGridView DGV_SameList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PageIndex_Same;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_ItemIndex_Same;

    }
}
