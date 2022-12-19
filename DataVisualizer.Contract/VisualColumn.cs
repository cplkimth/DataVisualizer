namespace DataVisualizer.Contract;

public record VisualColumn(string FieldName, ColumnType ColumnType, int DisplayIndex, string? Format = null, int? Width = null);
