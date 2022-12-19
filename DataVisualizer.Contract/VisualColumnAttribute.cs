namespace DataVisualizer.Contract;

public class VisualColumnAttribute : Attribute
{
    public VisualColumnAttribute(int index = 0, int width = 0, string format = "")
    {
        Index = index;
        Width = width;
        Format = format;
    }

    public int Index { get; init; }
    
    public int Width { get; init; }

    public string Format { get; init; }
}