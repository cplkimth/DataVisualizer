using System.Windows.Forms;
using DataVisualizer.Contract;
using System.Linq;

namespace DataVisualizer.WinformViewer;

public static class DataGridViewExtension
{
    public static void Initialize(this DataGridView grid, IEnumerable<VisualColumn> visualColumns)
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
        
        visualColumns = visualColumns.OrderBy(x => x.DisplayIndex).ThenBy(x => x.FieldName);
        foreach (var visualColumn in visualColumns)
        {
            var column = CeateColumn(visualColumn);
            columns.Add(column);
        }

        grid.Columns.AddRange(columns.ToArray());
    }

    private static DataGridViewColumn CeateColumn(VisualColumn visualColumn)
    {
        DataGridViewCellStyle style = new DataGridViewCellStyle();
        
        if (visualColumn.Format != null)
            style.Format = visualColumn.Format;
        style.NullValue = null;

        DataGridViewColumn grdiColumn = visualColumn.ColumnType switch
        {
            ColumnType.Boolean => new DataGridViewCheckBoxColumn(),
            _ => new DataGridViewTextBoxColumn()
        };
        grdiColumn.DataPropertyName = visualColumn.FieldName;
        grdiColumn.DefaultCellStyle = style;
        grdiColumn.HeaderText = visualColumn.FieldName;
        grdiColumn.ReadOnly = true;

        if (visualColumn.Width.HasValue)
            grdiColumn.Width = visualColumn.Width.Value;

        style.Alignment = visualColumn.ColumnType switch
        {
            ColumnType.Boolean => DataGridViewContentAlignment.MiddleCenter,
            ColumnType.Byte => DataGridViewContentAlignment.MiddleRight,
            ColumnType.SByte => DataGridViewContentAlignment.MiddleRight,
            ColumnType.Single => DataGridViewContentAlignment.MiddleRight,
            ColumnType.Double => DataGridViewContentAlignment.MiddleRight,
            ColumnType.Int16 => DataGridViewContentAlignment.MiddleRight,
            ColumnType.Int32 => DataGridViewContentAlignment.MiddleRight,
            ColumnType.Int64 => DataGridViewContentAlignment.MiddleRight,
            ColumnType.UInt16 => DataGridViewContentAlignment.MiddleRight,
            ColumnType.UInt32 => DataGridViewContentAlignment.MiddleRight,
            ColumnType.UInt64 => DataGridViewContentAlignment.MiddleRight,
            _ => DataGridViewContentAlignment.MiddleLeft
        };

        grdiColumn.SortMode = DataGridViewColumnSortMode.Automatic;

        return grdiColumn;
    }
}