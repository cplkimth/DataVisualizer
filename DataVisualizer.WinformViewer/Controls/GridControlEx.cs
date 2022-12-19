#region
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using System.Windows.Media;
using DataVisualizer.WinformViewer;
using DataVisualizer.WinformViewer.Forms;
using DataVisualizer.WinformViewer.Interfaces;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Extensions;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Customization;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPivotGrid.Localization;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit.Forms;
using Action = System.Action;
using Color = System.Drawing.Color;
#endregion

namespace DataVisualizer.WinformViewer.Controls;

public class GridControlEx : GridControl, ILayout
{
    public GridView GridView => MainView as GridView;

    #region layout
    public void SaveLayout(RootForm containerForm)
    {
        this.SaveLayout(containerForm, GridView.SaveLayoutToXml);
    
        this.SaveLayout(containerForm, SaveColumnColors);
    }
    
    private void SaveColumnColors(string path)
    {
        var colors = GridView.Columns.ToDictionary(x => x.Name, x => x.AppearanceHeader.BackColor.ToArgb());
    
        var json = JsonSerializer.Serialize(colors);
        File.WriteAllText(path + ".json", json);
    }
    
    public void RestoreLayout(RootForm containerForm)
    {
        this.RestoreLayout(containerForm, GridView.RestoreLayoutFromXml, ForceInitialize);
    
        this.RestoreLayout(containerForm, RestoreColumnColors);
    }
    
    private void RestoreColumnColors(string path)
    {
        // �� �� ���� �簢���� �׷����� ���� ����
    
        var json = File.ReadAllText(path + ".json");
        var colors = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
    
        foreach (GridColumn column in GridView.Columns)
            if (colors.ContainsKey(column.Name))
                column.AppearanceHeader.BackColor = Color.FromArgb(colors[column.Name]);
    }
    #endregion

    [Browsable(true)]
    [DefaultValue(false)]
    public bool HideRowHeaderNumber { get; set; }

    [Browsable(true)]
    [DefaultValue(false)]
    public bool Unexportable { get; set; }

    [Browsable(true)]
    [DefaultValue(false)]
    public bool Editable { get; set; }

    [Browsable(true)]
    [DefaultValue(60)]
    public int IndicatorWidth { get; set; } = 60;

    /// <summary>
    ///   �׸����� �⺻ ������ �ҷ��´�.
    /// </summary>
    public void LoadPreset()
    {
        string guid = (string)GridView.Tag;
        if (guid == "custom")
            return;

        ShowOnlyPredefinedDetails = true;

        GridView.GroupPanelText = " ";

        GridView.OptionsView.ShowFooter = false;
        GridView.OptionsView.ShowAutoFilterRow = false;
        GridView.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;

        GridView.OptionsFilter.AllowFilterEditor = false;
        GridView.OptionsPrint.AutoWidth = true;
        GridView.OptionsView.ColumnAutoWidth = true;

        GridView.OptionsFind.ShowFindButton = false;
        GridView.OptionsFind.ShowClearButton = false;
        GridView.OptionsFind.FindDelay = 500;

        GridView.OptionsLayout.StoreDataSettings = true;

        #region PDF �������⸦ ���ؼ��� �ѱ���Ʈ�� �ʿ�
        // PDF �������⸦ ���ؼ��� �ѱ���Ʈ�� �ʿ�            
        //            GridView.Appearance.Row.Font = new Font("NanumGothic", 9F);
        //            GridView.Appearance.Row.Options.UseFont = true;
        //            GridView.Appearance.HeaderPanel.Font = new Font("NanumGothic", 9F);
        //            GridView.Appearance.HeaderPanel.Options.UseFont = true;

        //            GridView.Appearance.FocusedRow.Font = new Font(view.Appearance.FocusedRow.Font, FontStyle.Bold);
        #endregion

        if (HideRowHeaderNumber)
        {
            GridView.OptionsView.ShowIndicator = false;
        }
        else
        {
            GridView.IndicatorWidth = IndicatorWidth;
            GridView.CustomDrawRowIndicator += (sender, e) =>
                       {
                           if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                           {
                               e.Info.ImageIndex = -1;
                               e.Info.DisplayText = (e.RowHandle + 1).ToString();
                               e.Info.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                           }
                       };
        }

        GridView.PopupMenuShowing += GridView_PopupMenuShowing;

        GridView.CustomFilterDialog += (sender, e) => e.Handled = true;

        GridView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;

        if (Editable)
        {
            GridView.OptionsBehavior.Editable = true;
            GridView.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
        }
        else
        {
            GridView.OptionsBehavior.Editable = false;
            GridView.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
        }

        if (CanAcceptFile || CanAcceptObject)
        {
            AllowDrop = true;
            DragOver += Grid_DragOver;
            DragDrop += Grid_DragDrop;
        }

        if (CanDragObject)
        {
            MouseDown += GridView_MouseDown;
            MouseMove += GridView_MouseMove;
        }
    }

    #region GridView_PopupMenuShowing
    private void GridView_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
    {
        if (e.MenuType == GridMenuType.Column)
        {
            GridViewColumnMenu menu = e.Menu as GridViewColumnMenu;
            if (menu.Column == null)
                return;

            // ��� �޴��� �����Ѵ�.
            for (int i = menu.Items.Count - 1; i >= 0; i--)
                menu.Items.RemoveAt(i);

            menu.Items.Add(CreateMenu("���� ����", false, (_, _) => menu.Column.AppearanceHeader.BackColor = InputForm.InputColor() ?? Color.Empty));

            menu.Items.Add(CreateMenu("���� ����", false, (_, _) => menu.Column.AppearanceHeader.BackColor = Color.Transparent));

            menu.Items.Add(CreateMenu("�������� ����", true, (_, _) => menu.Column.SortOrder = ColumnSortOrder.Ascending));
            menu.Items.Add(CreateMenu("�������� ����", false, (_, _) => menu.Column.SortOrder = ColumnSortOrder.Descending));
            menu.Items.Add(CreateMenu("���� ����", false, (_, _) => menu.Column.SortOrder = ColumnSortOrder.None));

            menu.Items.Add(CreateMenu("���� ����", true, (_, _) => menu.Column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near));
            menu.Items.Add(CreateMenu("��� ����", false, (_, _) => menu.Column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center));
            menu.Items.Add(CreateMenu("������ ����", false, (_, _) => menu.Column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far));

            menu.Items.Add(CreateMenu("�ڵ� �ʺ� (�÷�)", true, (_, _) => menu.Column.BestFit()));
            menu.Items.Add(CreateMenu("������", false, (_, _) => { menu.Column.Fixed = menu.Column.Fixed == FixedStyle.Left ? FixedStyle.None : FixedStyle.Left; }));

            menu.Items.Add(CreateMenu("�׷� �г�", true, (_, _) => ShowHideGroupBox(!GridView.OptionsView.ShowGroupPanel)));
            menu.Items.Add(CreateMenu("��� �г�", false, (_, _) => ShowHideSummary(!GridView.OptionsView.ShowFooter)));

            menu.Items.Add(CreateMenu("��� �� �˻� �г� ���̱�", true, (_, _) => ShowFindPanel()));
            menu.Items.Add(CreateMenu("��� ���� ����", false, (_, _) => ClearAllFilters()));

            menu.Items.Add(CreateMenu("�ڵ� �ʺ� (����)", true, (_, _) => BestFitAllColumns(!GridView.OptionsView.ColumnAutoWidth)));
            // menu.Items.Add(CreateMenu("�÷� ���ñ�", true, (_, _) => ShowColumnChooser()));
            menu.Items.Add(CreateMenu("���̾ƿ� �ʱ�ȭ", false, (_, _) => this.QueueGuidToDelete()));

            if (Unexportable == false)
            {
                menu.Items.Add(CreateMenu("���� ���Ϸ� ��������", false, (_, _) => Export()));
                menu.Items.Add(CreateMenu("Ŭ������� ����", false, (_, _) => CopyAll()));
            }

        }
        else if (e.MenuType == GridMenuType.Group)
        {
            var menu = e.Menu as GridViewGroupPanelMenu;

            if (menu == null)
                return;

            menu.Items[0].Caption = "�׷� ��� ��ħ";
            menu.Items[1].Caption = "�׷� ��� ����";
            menu.Items[2].Caption = "��� �׷��� ����";
            menu.Items[3].Caption = "�׷� �г� ����";
        }
    }

    private DXMenuItem CreateMenu(string caption, bool beginGroup, EventHandler onClick)
    {
        DXMenuItem menu = new DXMenuItem(caption);
        menu.BeginGroup = beginGroup;
        menu.Click += onClick;

        return menu;
    }
    #endregion

    #region grid view commands
    public void ShowHideGroupBox(bool visible)
    {
        GridView.OptionsView.ShowGroupPanel = visible;
    }

    public void ShowHideSummary(bool visible)
    {
        GridView.OptionsView.ShowFooter = visible;
    }

    public void ShowFindPanel()
    {
        GridView.ShowFindPanel();
    }

    public void ShowHideAutoFilter(bool visible)
    {
        GridView.OptionsView.ShowAutoFilterRow = visible;
        if (visible == false)
            GridView.ActiveFilterString = string.Empty;
    }

    public string ClearAllFilters()
    {
        return GridView.ActiveFilterString = string.Empty;
    }

    public void BestFitAllColumns(bool value)
    {
        GridView.OptionsView.ColumnAutoWidth = value;
    }

    public void ShowColumnChooser()
    {
        GridView.ShowCustomization();
    }

    public void Export()
    {
        var options = new XlsxExportOptions(TextExportMode.Text, true, true);
        WinformUtility.ExportToExcel(ExportToXlsx, options);
    }

    public void CopyAll()
    {
        // MultiSelect �Ӽ��� �ӽ÷� Ǯ�� ��ü ���� �� �����Ѵ�. �۾��� ������ MultiSelect �Ӽ��� ���� ������ �����Ѵ�.
        bool multiSelect = GridView.OptionsSelection.MultiSelect;

        GridView.OptionsSelection.MultiSelect = true;
        GridView.SelectAll();
        GridView.CopyToClipboard();

        GridView.OptionsSelection.MultiSelect = multiSelect;

        XtraMessageBox.Show("��� �����͸� Ŭ������� �����Ͽ����ϴ�. ���� ��� �ٿ��ֱ� �� �� �ֽ��ϴ�.");
    }
    #endregion

    #region Register
    private Action? _doubleClickAction;
    private PopupMenu? _popupMenu;

    /// <summary>
    /// �׸����� �⺻ �̺�Ʈ �ڵ鷯�� ����Ѵ�.
    /// ������ �ο츦 ���� Ŭ�� �� doubleClickAction �븮�ڸ� ȣ���Ѵ�. 
    /// ���� RowCellClick �̺�Ʈ���� �˾��޴��� ���� �۾��� ����Ѵ�.
    /// ���κ䰡 GridView�� �ƴϸ� ���ܰ� �߻��Ѵ�.
    /// </summary>
    /// <param name="doubleClickAction"> </param>
    /// <param name="popupMenu"> </param>
    public void Register(Action doubleClickAction, PopupMenu popupMenu)
    {
        _doubleClickAction = doubleClickAction;
        _popupMenu = popupMenu;

        // ColumnButton (���� ���)�� Ŭ���ϸ� ��� �ο츦 �����Ѵ�.
        MouseClick += GridControl_MouseClick;

        // ���� Ŭ���ϸ� ���� â�� ����.
        MouseDoubleClick -= GridConrol_MouseDoubleClick;
        MouseDoubleClick += GridConrol_MouseDoubleClick;

        // ������ ��ư�� ������ �˾� �޴��� ����. ���ٽ��� ����ϸ� pair�� �׻� pairs�� ������ ���Ҹ� ����Ű�� �ȴ�.
        GridView.RowCellClick += GridView_RowCellClick;

        // NewItemRow�� Ŭ���ϸ� �˾��޴��� ����� �ʴ´�.
        if (popupMenu != null)
            popupMenu.BeforePopup += PopupMenu_BeforePopup;
    }

    /// <summary>
    /// �θ� Ŭ������ Register�� ���ؼ� ���ǵ� _doubleClickAction�� �������Ѵ�.
    /// </summary>
    /// <param name="doubleClickAction"></param>
    public void OverrideDoubleClickAction(Action doubleClickAction)
    {
        _doubleClickAction = doubleClickAction;
    }

    private void PopupMenu_BeforePopup(object sender, CancelEventArgs e)
    {
        if (GridView.FocusedRowHandle == NewItemRowHandle)
            e.Cancel = true;
    }

    private void GridView_RowCellClick(object sender, RowCellClickEventArgs e)
    {
        GridHitInfo hitInfo = GridView.CalcHitInfo(e.Location);

        if (e.Button == MouseButtons.Right && hitInfo.RowHandle != NewItemRowHandle && _popupMenu != null)
            _popupMenu.ShowPopup(e.Column.View.GridControl.PointToScreen(e.Location));
    }

    private void GridConrol_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            GridView view = (GridView)GetViewAt(e.Location);
            GridHitInfo hitInfo = view.CalcHitInfo(e.Location);
            if (hitInfo.InRow && hitInfo.RowHandle >= 0)
            {
                var focusedItem = view.GetFocusedRow();
                if (focusedItem != null && _doubleClickAction != null)
                    _doubleClickAction();
            }
        }
    }

    private void GridControl_MouseClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            GridView view = (GridView)GetViewAt(e.Location);
            GridHitInfo hitInfo = view.CalcHitInfo(e.Location);

            // ColumnButton (���� ���)�� Ŭ���ϸ� ��� �ο� ����
            if (hitInfo.HitTest == GridHitTest.ColumnButton)
                if (view.OptionsSelection.MultiSelect)
                    view.SelectAll();
        }
    }
    #endregion

    #region utility methods
    public void RefreshFocusedRow()
    {
        GridView.RefreshRow(GridView.FocusedRowHandle);
    }

    public List<T> GetSelectedObjects<T>()
    {
        var handles = GridView.GetSelectedRows().Where(x => x >= 0);

        return handles.ToList().ConvertAll(x => (T)GridView.GetRow(x));
    }

    private List<int> GetHandles(Func<int, bool> predicate)
    {
        List<int> handles = new List<int>();

        int handle = GridView.GetVisibleRowHandle(0);
        while (handle != InvalidRowHandle)
        {
            if (predicate(handle))
                handles.Add(handle);

            handle = GridView.GetNextVisibleRow(handle);
        }

        return handles;
    }

    public void SetRowValue(bool value)
    {
        SetRowValue(GridView.FocusedRowHandle, value);
    }

    public void SetRowValue(int rowHandle, bool value)
    {
        for (int i = 2; i < GridView.Columns.Count; i++)
            GridView.SetRowCellValue(rowHandle, GridView.Columns[i], value);
    }

    public void SetColumnValue(bool value)
    {
        int currentRowHandle = GridView.GetVisibleRowHandle(0);

        while (currentRowHandle != InvalidRowHandle)
        {
            GridView.SetRowCellValue(currentRowHandle, GridView.FocusedColumn, value);

            currentRowHandle = GridView.GetNextVisibleRow(currentRowHandle);
        }
    }

    public void SetAllRowsValue(bool value)
    {
        int currentRowHandle = GridView.GetVisibleRowHandle(0);

        while (currentRowHandle != InvalidRowHandle)
        {
            SetRowValue(currentRowHandle, value);

            currentRowHandle = GridView.GetNextVisibleRow(currentRowHandle);
        }
    }

    public void SetNewItemRowPosition(NewItemRowPosition newItemRowPosition)
    {
        GridView.OptionsView.NewItemRowPosition = newItemRowPosition;
    }

    public void DeleteFocusedRow()
    {
        GridView.DeleteRow(GridView.FocusedRowHandle);
    }

    public void ToReadOnly()
    {
        if (GridView != null)
            GridView.OptionsBehavior.ReadOnly = true;
    }

    public object GetFocusedRow()
    {
        return GridView.GetFocusedRow();
    }

    /// <summary>
    /// ��� �ο츦 ��ȸ�ϸ� Ư�� �۾��� �Ѵ�.
    /// </summary>
    /// <param name="func">�۾��� �޼���. �ִ� Ƚ���� �������� ���θ� ��ȯ�Ѵ�. null�� ��ȯ�ϸ� ��� ��ȸ�� �����.</param>
    /// <param name="maxCount">�ִ� Ƚ��. �ִ� Ƚ���� �����ϸ� �ο��� ��ȸ�� �����.</param>
    public void Traverse(Func<int, bool?> func, int maxCount = int.MaxValue)
    {
        int count = 0;

        int handle = GridView.GetVisibleRowHandle(0);
        while (handle != InvalidRowHandle)
        {
            if (count == maxCount)
                return;

            var result = func(handle);

            if (result == null)
                break;
            else if (result.Value)
                count++;

            handle = GridView.GetNextVisibleRow(handle);
        }
    }

    public void ToMultiSelectable()
    {
        GridView.OptionsSelection.MultiSelect = true;
        GridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
        GridView.OptionsSelection.CheckBoxSelectorColumnWidth = 20;
        GridView.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DefaultBoolean.True;
    }
    #endregion

    #region drag and drop
    /// <summary>
    ///     �巡�� �� ������� �������� ������ ���� �� ����.
    /// </summary>
    [DefaultValue(false)]
    public bool CanAcceptFile { get; set; }

    /// <summary>
    ///     �巡�� �� ������� �ٸ� �׸���κ��� ��ü�� ���� �� ����.
    /// </summary>
    [DefaultValue(false)]
    public bool CanAcceptObject { get; set; }

    private bool CanDragObject => DroppableType != null;

    /// <summary>
    ///     �巡�� �� ������� ���� �� �ִ� ��ü�� Ÿ��
    /// </summary>
    public Type DroppableType { get; set; }

    private void Grid_DragOver(object sender, DragEventArgs e)
    {
        bool acceptable = false;

        if (CanAcceptObject)
            if (e.Data.GetDataPresent(DroppableType))
                acceptable = true;

        if (CanAcceptFile)
            if (e.Data.GetDataPresent("FileNameW"))
            {
                if (e.Data.GetData("FileNameW") is string[] files && files.Length != 0)
                    acceptable = true;
            }

        e.Effect = acceptable ? DragDropEffects.Move : DragDropEffects.None;
    }

    private void Grid_DragDrop(object sender, DragEventArgs e)
    {
        if (CanAcceptObject)
        {
            GridControl grid = sender as GridControl;
            object list = grid.DataSource;
            object @object = e.Data.GetData(DroppableType);

            if (@object != null && list != null)
                OnObjectDropped(@object);
        }

        if (CanAcceptFile)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) == false)
                return;

            if (e.Data.GetData(DataFormats.FileDrop) is not string[] files || files.Length == 0)
                XtraMessageBox.Show("���ε��� ������ ���õ��� �ʾҽ��ϴ�");
            else
                OnFileDropped(files);
        }
    }

    private GridHitInfo _downHitInfo;

    private void GridView_MouseDown(object sender, MouseEventArgs e)
    {
        GridView view = sender as GridView;
        _downHitInfo = null;

        GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));

        if (ModifierKeys != Keys.None)
            return;

        if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
            _downHitInfo = hitInfo;
    }

    private void GridView_MouseMove(object sender, MouseEventArgs e)
    {
        GridView view = sender as GridView;
        if (e.Button == MouseButtons.Left && _downHitInfo != null)
        {
            Size dragSize = SystemInformation.DragSize;
            Rectangle dragRect = new Rectangle(new Point(_downHitInfo.HitPoint.X - dragSize.Width / 2,
                _downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                object @object = view.GetRow(_downHitInfo.RowHandle);
                view.GridControl.DoDragDrop(@object, DragDropEffects.Move);
                _downHitInfo = null;
                DXMouseEventArgs.GetMouseArgs(e).Handled = true;
            }
        }
    }
    #endregion

    #region FileDropped event things for C# 3.0
    public event EventHandler<FileDroppedEventArgs> FileDropped;

    protected virtual void OnFileDropped(FileDroppedEventArgs e)
    {
        FileDropped?.Invoke(this, e);
    }

    protected virtual void OnFileDropped(string[] files)
    {
        OnFileDropped(new FileDroppedEventArgs(files));
    }

    protected virtual FileDroppedEventArgs OnFileDroppedWithReturn(string[] files)
    {
        FileDroppedEventArgs args = new FileDroppedEventArgs(files);
        OnFileDropped(args);

        return args;
    }

    public class FileDroppedEventArgs : EventArgs
    {
        public string[] Files { get; set; }

        public FileDroppedEventArgs()
        {
        }

        public FileDroppedEventArgs(string[] files)
        {
            Files = files;
        }
    }
    #endregion

    #region ObjectDropped event things for C# 3.0
    public event EventHandler<ObjectDroppedEventArgs> ObjectDropped;

    protected virtual void OnObjectDropped(ObjectDroppedEventArgs e)
    {
        ObjectDropped?.Invoke(this, e);
    }

    protected virtual void OnObjectDropped(object droppedObject)
    {
        OnObjectDropped(new ObjectDroppedEventArgs(droppedObject));
    }

    protected virtual ObjectDroppedEventArgs OnObjectDroppedWithReturn(object droppedObject)
    {
        ObjectDroppedEventArgs args = new ObjectDroppedEventArgs(droppedObject);
        OnObjectDropped(args);

        return args;
    }

    public class ObjectDroppedEventArgs : EventArgs
    {
        public object DroppedObject { get; set; }

        public ObjectDroppedEventArgs()
        {
        }

        public ObjectDroppedEventArgs(object droppedObject)
        {
            DroppedObject = droppedObject;
        }
    }
    #endregion
}