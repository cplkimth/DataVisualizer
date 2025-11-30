#region
using System.IO;
using System.Windows.Forms;
#endregion

namespace DataVisualizer.WinformViewer.Forms;

public partial class MainForm : Form
{
    #region singleton

    private static readonly Lazy<MainForm> _instance = new(() => new MainForm());

    public static MainForm Instance => _instance.Value;

    private MainForm()
    {
        InitializeComponent();
    }

    #endregion

    public string Folder { get; private set; }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);

        if (DesignMode || Program.IsRunTime == false)
            return;

#if DEBUG
        Folder = @"D:\incoming\DataVisualizer";
#else
// Folder = @"D:\incoming\DataVisualizer";
        if (fbdDialog.ShowDialog() == DialogResult.OK)
        {
            Folder = fbdDialog.SelectedPath;
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

        fswWatcher.Path = Folder;
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
        var directory = new DirectoryInfo(Folder);
        var files = directory.GetFiles("*.json");
        var latest = files.MaxBy(x => x.CreationTime);
        return latest?.FullName;
    }

    private void fswWatcher_Created(object sender, FileSystemEventArgs e)
    {
        ShowFile(e.FullPath);
    }
}