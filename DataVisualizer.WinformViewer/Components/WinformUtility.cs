#region
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using DataVisualizer.WinformViewer.Controls;
using DataVisualizer.WinformViewer.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
#endregion

namespace DataVisualizer.WinformViewer;

public static class WinformUtility
{
    public static void ExportToExcel(Action<string, XlsxExportOptions> exportAction, XlsxExportOptions? options = null)
    {
        SaveFileDialog sfdExcelExport = new SaveFileDialog();
        sfdExcelExport.Filter = "Excel 2007 (*.xlsx)|*.xlsx";

        if (sfdExcelExport.ShowDialog() != DialogResult.OK)
            return;

        if (sfdExcelExport.FileName.EndsWith(".xlsx"))
            exportAction(sfdExcelExport.FileName, options);
        else
            return;

        if (AskSure("엑셀 파일을 저장하였습니다. 저장한 파일을 열까요?"))
            Process.Start(sfdExcelExport.FileName);
    }

    public static bool AskSure(string message, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button2)
    {
        return
            XtraMessageBox.Show(message, "질문", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                defaultButton) == DialogResult.Yes;
    }

    public static T FindInAncestors<T>(this Control from) where T : Control
    {
        Control control = from;

        while (true)
        {
            if (control is T)
                return (T) control;

            control = control.Parent;

            if (control == null)
                return null;
        }
    }

    public static List<T> GetChildren<T>(this Control container) where T : class
    {
        List<T> controls = new List<T>();

        GetChildrenCore(container, controls);

        return controls;
    }

    private static void GetChildrenCore<T>(Control container, List<T> controls) where T : class
    {
        var fields = container.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (var field in fields)
            if (typeof(T).IsAssignableFrom(field.FieldType))
            {
                T control = (T)field.GetValue(container);

                if (control != null)
                    controls.Add(control);
            }

            else if (field.FieldType.IsSubclassOf(typeof(RootControl)))
            {
                var rootControl = (RootControl)field.GetValue(container);
                GetChildrenCore(rootControl, controls);
            }
    }

    #region SuspendDrawing / ResumeDrawing
    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

    private const int WM_SETREDRAW = 11;

    public static void SuspendDrawing(this XtraForm form)
    {
        SendMessage(form.Handle, WM_SETREDRAW, false, 0);
    }

    public static void ResumeDrawing(this XtraForm form)
    {
        SendMessage(form.Handle, WM_SETREDRAW, true, 0);

        form.Refresh();
    }
    #endregion
}