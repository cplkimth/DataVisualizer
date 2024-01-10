using DataVisualizer.Contract;

namespace DataVisualizer.TestClient;

internal class Program
{
    static void Main(string[] args)
    {
        const int Size = 1000;
        var list = new List<SampleItem>(Size);
        for (int i = 0; i < Size; i++)
        {
            list.Add(new SampleItem());
        }

        Visualizer.Serialzie(list, @"D:\incoming\DataVisualizer");
    }
}