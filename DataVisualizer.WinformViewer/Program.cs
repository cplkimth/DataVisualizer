using DataVisualizer.WinformViewer.Forms;

namespace DataVisualizer.WinformViewer;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        IsRunTime = true;
            
        Application.Run(MainForm.Instance);
    }

    public static bool IsRunTime { get; set; }
}