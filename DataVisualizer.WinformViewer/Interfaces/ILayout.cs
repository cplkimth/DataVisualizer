#region
using DataVisualizer.WinformViewer.Forms;
#endregion

namespace DataVisualizer.WinformViewer.Interfaces;

public interface IPresettable
{
    void LoadPreset();
}

public interface ILayout : IPresettable
{
    void SaveLayout(RootForm containerForm);
    void RestoreLayout(RootForm containerForm);
}