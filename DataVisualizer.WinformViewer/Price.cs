namespace DataVisualizer.WinformViewer;

public partial class Price
{
    public string StockId { get; set; }

    public DateTime At { get; set; }

    public byte PriceType { get; set; }

    public double Open { get; set; }

    public double High { get; set; }

    public double Low { get; set; }

    public double Close { get; set; }

    public double Volume { get; set; }
}