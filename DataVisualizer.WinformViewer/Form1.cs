using System.ComponentModel;
using Library.Forms;
using Newtonsoft.Json;

namespace DataVisualizer.WinformViewer;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (DesignMode || Program.IsRunTime == false)
            return;

        var columnInfos = new ColumnInfo[]
                          {
                              new("StockId", typeof(string)),
                              new("At", typeof(DateTime), "d"),
                              new("PriceType", typeof(byte)),
                              new("IsDaily", typeof(bool)),
                              new("Open", typeof(double), "N1", 50),
                              new("Close", typeof(double), "N2", 80),
                              new("High", typeof(double), "N2", 60),
                              new("Low", typeof(double), "N2", 70),
                              new("Volume", typeof(double), "N0", 90),
                          };
        grid.Initialize(columnInfos);

        var path = Path.Combine(Path.GetTempPath(), $"Price.json");
        var json = File.ReadAllText(path);
        var list = JsonConvert.DeserializeObject<List<object>>(json);

        SortableBindingList<object> bindingList = new(list!);

        grid.DataSource = bdsList;
        bdsList.DataSource = bindingList!;
    }
}