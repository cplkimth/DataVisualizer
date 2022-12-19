namespace DataVisualizer.Contract;

public class VisualColumnAttribute : Attribute
{
    public VisualColumnAttribute(int index = 0, string format = "", int width = 0)
    {
        Index = index;
        Format = format;
        Width = width;
    }

    public int Index { get; init; }

    public string? Format { get; init; }
    
    public int Width { get; init; }
}