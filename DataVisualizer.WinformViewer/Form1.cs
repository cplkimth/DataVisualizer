using System.ComponentModel;
using System.Dynamic;
using System.Text.Json;
using DataVisualizer.Contract;
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

        var path = Path.Combine(@"D:\incoming\DataVisualizer\Quantool.Data.Price", "Price.json");
        var json = File.ReadAllText(path);

        var tokens = json.Split(Visualizer.Separator);
        var metaJson = tokens[0];
        var dataJson = tokens[1];
        
        var visualColumns = JsonConvert.DeserializeObject<List<VisualColumn>>(metaJson);
        grid.Initialize(visualColumns!.ToArray());

        var list = JsonConvert.DeserializeObject<List<dynamic>>(dataJson);

        grid.DataSource = new SortableBindingList<dynamic>(list!);
        // bdsList.DataSource = bindingList;
    }

    private static object GetValue(ColumnType columnType, JsonElement property)
    {
        return (columnType switch
        {
            ColumnType.Boolean => property.GetBoolean(),
            ColumnType.Byte => property.GetByte(),
            ColumnType.DateTime => property.GetDateTime(),
            ColumnType.DateTimeOffset => property.GetDateTimeOffset(),
            ColumnType.Decimal => property.GetDecimal(),
            ColumnType.Double => property.GetDouble(),
            ColumnType.Guid => property.GetGuid(),
            ColumnType.Int16 => property.GetInt16(),
            ColumnType.Int32 => property.GetInt32(),
            ColumnType.Int64 => property.GetInt64(),
            ColumnType.SByte => property.GetSByte(),
            ColumnType.Single => property.GetSingle(),
            ColumnType.String => property.GetString(),
            ColumnType.UInt16 => property.GetUInt16(),
            ColumnType.UInt32 => property.GetUInt32(),
            ColumnType.UInt64 => property.GetUInt64(),
            _=> property.GetRawText()
        })!;
    }
}