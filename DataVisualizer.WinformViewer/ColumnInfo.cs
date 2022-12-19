namespace DataVisualizer.WinformViewer;

public record ColumnInfo(string FieldName, Type DataType, string? Format = null, int? Width = null);
