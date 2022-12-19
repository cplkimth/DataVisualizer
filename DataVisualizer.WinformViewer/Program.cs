namespace DataVisualizer.WinformViewer
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            IsRunTime = true;
            
            Application.Run(new Form1());
        }

        public static bool IsRunTime { get; set; }
    }
}