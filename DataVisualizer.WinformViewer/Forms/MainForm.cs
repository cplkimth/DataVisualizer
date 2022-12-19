#region
using System.IO;
using DevExpress.XtraEditors;
#endregion

namespace DataVisualizer.WinformViewer.Forms;

public partial class MainForm : XtraForm
{
    public MainForm()
    {
        InitializeComponent();
    }

    private string _folder;

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);

        if (DesignMode || Program.IsRunTime == false)
            return;

#if DEBUG
        _folder = @"D:\incoming\DataVisualizer";
#else
        if (fbdDialog.ShowDialog() == DialogResult.OK)
        {
            _folder = fbdDialog.SelectedPath;
        }
        else
        {
            Close();
            return;
        }
#endif

        string? latestFile = GetLatestFile();
        if (latestFile != null)
            ShowFile(latestFile);

        fswWatcher.Path = _folder;
        fswWatcher.EnableRaisingEvents = true;
    }

    private void ShowFile(string filePath)
    {
        GridForm form = new GridForm(filePath);
        form.MdiParent = this;
        form.Show();
    }

    private string? GetLatestFile()
    {
        var directory = new DirectoryInfo(_folder);
        var files = directory.GetFiles("*.json");
        var latest = files.OrderByDescending(x => x.CreationTime).FirstOrDefault();
        return latest?.FullName;
    }

    private void fswWatcher_Created(object sender, FileSystemEventArgs e)
    {
        ShowFile(e.FullPath);
    }
}