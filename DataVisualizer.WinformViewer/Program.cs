using DataVisualizer.WinformViewer.Forms;
using System.Runtime.InteropServices;
using System.Text;

namespace DataVisualizer.WinformViewer;

internal static class Program
{
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern int AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern int FreeConsole();

    public static bool IsRunTime { get; set; }

    [STAThread]
    static void Main()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        IsRunTime = true;

        AllocConsole();
        Application.Run(MainForm.Instance);
        FreeConsole();
    }
}