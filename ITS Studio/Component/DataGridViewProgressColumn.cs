using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ThirdPartyComponent
{
    public class DataGridViewProgressColumn : DataGridViewImageColumn
    {
        public static Color _ProgressBarColor;

        public DataGridViewProgressColumn()
        {
            CellTemplate = new DataGridViewProgressCell();
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DataGridViewProgressCell)))
                {
                    throw new InvalidCastException("Must be a DataGridViewProgressCell");
                }
                base.CellTemplate = value;
            }
        }

        ///// <summary>
        ///// ProgressBar最大値
        ///// </summary>
        //public int Maximum
        //{
        //    get
        //    {
        //        return ((DataGridViewProgressBarCell)this.CellTemplate).Maximum;
        //    }
        //    set
        //    {
        //        if (this.Maximum == value)
        //            return;
        //        ((DataGridViewProgressBarCell)this.CellTemplate).Maximum = value;
        //        if (this.DataGridView == null)
        //            return;
        //        int rowCount = this.DataGridView.RowCount;
        //        for (int i = 0; i < rowCount; i++)
        //        {
        //            DataGridViewRow r = this.DataGridView.Rows.SharedRow(i);
        //            ((DataGridViewProgressBarCell)r.Cells[this.Index]).Maximum = value;
        //        }
        //    }
        //}
        ///// <summary>
        ///// ProgressBar最小値
        ///// </summary>
        //public int Mimimum
        //{
        //    get
        //    {
        //        return ((DataGridViewProgressBarCell)this.CellTemplate).Mimimum;
        //    }
        //    set
        //    {
        //        if (this.Mimimum == value)
        //            return;
        //        ((DataGridViewProgressBarCell)this.CellTemplate).Mimimum = value;
        //        if (this.DataGridView == null)
        //            return;
        //        int rowCount = this.DataGridView.RowCount;
        //        for (int i = 0; i < rowCount; i++)
        //        {
        //            DataGridViewRow r = this.DataGridView.Rows.SharedRow(i);
        //            ((DataGridViewProgressBarCell)r.Cells[this.Index]).Mimimum = value;
        //        }
        //    }
        //}

        [Browsable(true)]
        public Color ProgressBarColor
        {
            get
            {
                if (this.ProgressBarCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.ProgressBarCellTemplate.ProgressBarColor;
            }
            set
            {
                if (this.ProgressBarCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.ProgressBarCellTemplate.ProgressBarColor = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        DataGridViewProgressCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewProgressCell;
                        if (dataGridViewCell != null)
                        {
                            dataGridViewCell.SetProgressBarColor(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                    // TODO: This column and/or grid rows may need to be autosized depending on their
                    //       autosize settings. Call the autosizing methods to autosize the column, rows,
                    //       column headers / row headers as needed.
                }
            }
        }

        private DataGridViewProgressCell ProgressBarCellTemplate
        {
            get
            {
                return (DataGridViewProgressCell)this.CellTemplate;
            }
        }
    }
}