#region
using System.ComponentModel;
using DevExpress.Xpo.Helpers;
using System.IO;
using DevExpress.XtraEditors;
using DataVisualizer.WinformViewer.Interfaces;
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

        RestoreChildrenLayout();   
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        SaveChildrenLayout();

        _presettables.Clear();

        RemoveUserLayouts();

        base.OnClosing(e);
    }

    private void SaveChildrenLayout()
    {
        this.SuspendDrawing();

        foreach (var item in _presettables.OfType<ILayout>())
            item.SaveLayout(this);

        this.ResumeDrawing();
    }

    private void RestoreChildrenLayout()
    {
        this.SuspendDrawing();

        foreach (var item in _presettables.OfType<ILayout>())
            item.RestoreLayout(this);

        this.ResumeDrawing();
    }

    private void RemoveUserLayouts()
    {
        foreach (var guid in _guidsToDelete)
        {
            var files = Directory.GetFiles(WinformUtility.LayoutPath, "*.xml").Where(x => x.Contains(guid));
            foreach (var file in files)
                File.Delete(file);
        }

        _guidsToDelete.Clear();
    }
}