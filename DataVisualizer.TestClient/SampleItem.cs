using DataVisualizer.Contract;

namespace DataVisualizer.TestClient;

public class SampleItem
{
    [VisualColumn(0)]
    public string Name { get; } = Random.Shared.Next().ToString("D10");
    
    [VisualColumn(1)]
    public string Extension { get; } = Random.Shared.Next().ToString("D3");
    
    [VisualColumn(2)]
    public DateTime CreatedAt { get; } = DateTime.Now.AddSeconds(Random.Shared.Next(3600 * 24 * 1000) * -1);
    
    [VisualColumn(3, "N0")]
    public int Length { get; } = Random.Shared.Next(1024, 1024 * 1204 * 1024);
    
    [VisualColumn(5, "P2")]
    public double Percent { get; } = Random.Shared.NextDouble();
    
    [VisualColumn(4, "N0")]
    public double LengthInMB => Length / 1024.0 / 1024;
}