#region
using System.ComponentModel;
using System.IO;
using DataVisualizer.WinformViewer.Interfaces;
using DevExpress.XtraEditors;
#endregion

namespace DataVisualizer.WinformViewer.Forms;

public partial class RootForm : XtraForm
{
    public RootForm()
    {
        InitializeComponent();
    }

    public string LayoutMark { get; set; } = string.Empty;

    private readonly List<string> _guidsToDelete = new();

    private List<IPresettable> _presettables;

    public void AddGuidToDelete(string guid)
    {
        _guidsToDelete.Add(guid);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (DesignMode || Program.IsRunTime == false)
            return;

        // IPresettable 인터페이스가 정의된 컨트롤들을 초기화
        _presettables = this.GetChildren<IPresettable>();
        _presettables.ForEach(x => x.LoadPreset());
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);

        if (DesignMode || Program.IsRunTime == false)
            return;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _presettables.Clear();

        base.OnClosing(e);
    }
}