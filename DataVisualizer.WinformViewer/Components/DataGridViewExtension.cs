using DataVisualizer.Contract;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace DataVisualizer.WinformViewer.Components;

public static class DataGridViewExtension
{
    public static void Initialize(this GridControl grid, IEnumerable<VisualColumn> visualColumns)
    {
        grid.BeginUpdate();

        GridView view = (GridView) grid.MainView;
        view.OptionsBehavior.Editable = false;
        view.OptionsView.ColumnAutoWidth = true;

        List<GridColumn> columns = new();
        
        visualColumns = visualColumns.OrderBy(x => x.DisplayIndex).ThenBy(x => x.FieldName);
        foreach (var visualColumn in visualColumns)
            columns.Add(CreateColumn(visualColumn));

        columns.Add(CreateColumn(new VisualColumn(string.Empty, ColumnType.Boolean, 0)));
        columns[^1].VisibleIndex = columns.Count - 1;

        view.Columns.AddRange(columns.ToArray());
        view.AutoFillColumn = columns[^1];

        grid.EndUpdate();
    }

    private static GridColumn CreateColumn(VisualColumn visualColumn)
    {
        GridColumn gridColumn = new GridColumn();
        gridColumn.Tag = visualColumn;

        if (visualColumn.FieldName != string.Empty)
        {
            gridColumn.FieldName = visualColumn.FieldName;
            // gridColumn.OptionsColumn.FixedWidth = true;
            gridColumn.VisibleIndex = visualColumn.DisplayIndex;
            gridColumn.Caption = visualColumn.FieldName;
        }
        gridColumn.Width = visualColumn.Width == 0 ? 75 : visualColumn.Width;

        if (visualColumn.Format != null)
        {
            gridColumn.DisplayFormat.FormatString = visualColumn.Format;
            gridColumn.DisplayFormat.FormatType = FormatType.Custom;
        }

        return gridColumn;
    }

    public static void SetColumnAlignment(this GridControl grid)
    {
        grid.BeginUpdate();
        
        GridView view = (GridView) grid.MainView;
        foreach (GridColumn column in view.Columns)
        {
            VisualColumn visualColumn = column.Tag as VisualColumn;
            if (visualColumn == null)
                continue;

            column.AppearanceCell.TextOptions.HAlignment = visualColumn.ColumnType switch
            {
                ColumnType.Boolean => HorzAlignment.Center,
                ColumnType.Byte => HorzAlignment.Far,
                ColumnType.SByte => HorzAlignment.Far,
                ColumnType.Single => HorzAlignment.Far,
                ColumnType.Double => HorzAlignment.Far,
                ColumnType.Int16 => HorzAlignment.Far,
                ColumnType.Int32 => HorzAlignment.Far,
                ColumnType.Int64 => HorzAlignment.Far,
                ColumnType.UInt16 => HorzAlignment.Far,
                ColumnType.UInt32 => HorzAlignment.Far,
                ColumnType.UInt64 => HorzAlignment.Far,
                _ => HorzAlignment.Near
            };

            column.SortMode = ColumnSortMode.DisplayText;
        }

        grid.EndUpdate();
    }
}