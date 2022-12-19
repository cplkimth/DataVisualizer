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

    #region Layout
    private static string? _myDocumentRoot;

    public static string MyDocumentRoot
    {
        get
        {
            if (_myDocumentRoot == null)
            {
                _myDocumentRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DataVisualizer\\");

                if (Directory.Exists(_myDocumentRoot) == false)
                    Directory.CreateDirectory(_myDocumentRoot);
            }

            return _myDocumentRoot;
        }
    }

    public static string? _layoutPath;

    public static string LayoutPath
    {
        get
        {
            if (_layoutPath == null)
            {
                _layoutPath = MyDocumentRoot + "\\";

                if (Directory.Exists(_layoutPath) is false)
                    Directory.CreateDirectory(_layoutPath);
            }

            return _layoutPath;
        }
    }

    public static string GetLayoutPath(RootForm containerForm, string guid)
    {
        return $"{LayoutPath}{containerForm.LayoutMark}_{guid}.xml";
    }

    private static string? GetLayoutKey(Component component)
    {
        return component switch
        {
            Control control => control.Tag as string,
            _ => null
        };
    }

    private static void SaveLayoutCore(this Component component, RootForm containerForm, Action<string> saveLayout, Action<string, OptionsLayoutBase> saveLayout2, OptionsLayoutBase layoutOption)
    {
        try
        {
            string? guid = GetLayoutKey(component);

            if (guid == null)
                return;

            string path = GetLayoutPath(containerForm, guid);

            saveLayout.Invoke(path);
            saveLayout2.Invoke(path, layoutOption);
        }
        catch
        {
        }
    }

    public static void SaveLayout(this Component component, RootForm containerForm, Action<string> saveLayout)
        => SaveLayoutCore(component, containerForm, saveLayout, null, null);

    public static void SaveLayout(this Component component, RootForm containerForm, Action<string, OptionsLayoutBase> saveLayout, OptionsLayoutBase layoutOption)
        => SaveLayoutCore(component, containerForm, null, saveLayout, layoutOption);

    private static void RestoreLayoutCore(this Component component, RootForm containerForm, Action<string> restoreLayout, Action<string, OptionsLayoutBase> restoreLayout2, OptionsLayoutBase layoutOption, Action preAction = null,
        Action postAction = null)
    {
        try
        {
            string guid = GetLayoutKey(component);

            if (guid == null)
                return;

            preAction?.Invoke();

            var path = GetLayoutPath(containerForm, guid);

            if (File.Exists(path) is false)
                return;

            restoreLayout?.Invoke(path);
            restoreLayout2?.Invoke(path, layoutOption);
            postAction?.Invoke();
        }
        catch
        {
        }
    }

    public static void RestoreLayout(this Component component, RootForm containerForm, Action<string> restoreLayout, Action preAction = null, Action postAction = null)
        => RestoreLayoutCore(component, containerForm, restoreLayout, null, null, preAction, postAction);

    public static void RestoreLayout(this Component component, RootForm containerForm, Action<string, OptionsLayoutBase> restoreLayout, OptionsLayoutBase layoutOption, Action preAction = null, Action postAction = null)
        => RestoreLayoutCore(component, containerForm, null, restoreLayout, null, preAction, postAction);

    public static void QueueGuidToDelete(this Control control)
    {
        string guid = GetLayoutKey(control);

        if (guid == null)
            return;

        var containerForm = FindInAncestors<RootForm>(control);
        containerForm.AddGuidToDelete(guid);
    }
    #endregion

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