using System.Windows.Forms;

namespace DataVisualizer.WinformViewer;

public static class DataGridViewExtension
{
    public static void Initialize(this DataGridView grid, params ColumnInfo[] columnInfos)
    {
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToOrderColumns = true;
        grid.AutoGenerateColumns = false;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grid.ReadOnly = true;
        grid.RowTemplate.Height = 25;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = true;

        List<DataGridViewColumn> columns = new();
        foreach (var columnInfo in columnInfos)
        {
            var column = CeateColumn(columnInfo);
            columns.Add(column);
        }

        grid.Columns.AddRange(columns.ToArray());
    }

    private static DataGridViewColumn CeateColumn(ColumnInfo columnInfo)
    {
        DataGridViewCellStyle style = new DataGridViewCellStyle();
        
        if (columnInfo.Format != null)
            style.Format = columnInfo.Format;
        style.NullValue = null;

        DataGridViewColumn column = columnInfo.DataType == typeof(bool) ? new DataGridViewCheckBoxColumn() : new DataGridViewTextBoxColumn();
        column.DataPropertyName = columnInfo.FieldName;
        column.DefaultCellStyle = style;
        column.HeaderText = columnInfo.FieldName;
        column.ReadOnly = true;

        if (columnInfo.Width.HasValue)
            column.Width = columnInfo.Width.Value;

        if (columnInfo.DataType == typeof(string))
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        else if (columnInfo.DataType == typeof(bool))
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        else
            style.Alignment = DataGridViewContentAlignment.MiddleRight;

        column.SortMode = DataGridViewColumnSortMode.Automatic;

        return column;
    }
}