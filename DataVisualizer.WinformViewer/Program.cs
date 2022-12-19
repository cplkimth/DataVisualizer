using DataVisualizer.WinformViewer.Forms;

namespace DataVisualizer.WinformViewer;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        IsRunTime = true;
            
        Application.Run(new MainForm());
    }

    public static bool IsRunTime { get; set; }
}