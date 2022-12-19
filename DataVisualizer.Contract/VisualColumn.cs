namespace DataVisualizer.Contract;

public record VisualColumn(string FieldName, ColumnType ColumnType, int DisplayIndex, string Format = "", int Width = 0);
