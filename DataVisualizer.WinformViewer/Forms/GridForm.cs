#region
using System.ComponentModel;
using System.IO;
using DataVisualizer.Contract;
using DataVisualizer.WinformViewer.Components;
using Newtonsoft.Json;
#endregion

namespace DataVisualizer.WinformViewer.Forms;

public partial class GridForm : RootForm
{
    private GridForm()
    {
        InitializeComponent();
    }

    public GridForm(string filePath) : this()
    {
        _filePath = filePath;

        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var tokens = fileName.Split("_");
        var typeName = tokens[0].Split(".")[^1];
        var time = tokens[^1];
        _text = $"{typeName}-{time}";

        grdList.Tag = tokens[0];
    }

    private readonly string _filePath;

    private readonly string _text;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (DesignMode || Program.IsRunTime == false)
            return;

        Text = _text;

        var json = File.ReadAllText(_filePath);

        var tokens = json.Split(Visualizer.Separator);
        var metaJson = tokens[0];
        var dataJson = tokens[1];

        var visualColumns = JsonConvert.DeserializeObject<List<VisualColumn>>(metaJson);
        grdList.Initialize(visualColumns!.ToArray());

        var list = JsonConvert.DeserializeObject<List<dynamic>>(dataJson);

        bdsList.DataSource = list;

        grdList.LoadPreset();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}