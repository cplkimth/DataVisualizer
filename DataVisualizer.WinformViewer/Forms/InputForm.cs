#region
using DevExpress.XtraEditors;
#endregion

namespace DataVisualizer.WinformViewer.Forms;

public partial class InputForm : XtraForm
{
    private InputForm()
    {
        InitializeComponent();
    }

    public InputForm(BaseEdit baseEdit) : this()
    {
        _baseEdit = baseEdit;

        if (_baseEdit is TextEdit textEdit)
            textEdit.KeyUp += TextEdit_KeyUp;
        else
            _baseEdit.EditValueChanged += (_, _) => Close();

        if (_baseEdit is PopupBaseEdit popupBaseEdit)
            popupBaseEdit.Closed += (_, _) => Close();
    }

    private void TextEdit_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            Close();
    }

    private readonly BaseEdit _baseEdit;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (DesignMode || Program.IsRunTime == false)
            return;

        Size = new Size(200, 20);

        _baseEdit.Dock = DockStyle.Fill;
        Controls.Add(_baseEdit);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);

        if (DesignMode || Program.IsRunTime == false)
            return;

        if (_baseEdit is PopupBaseEdit popupBaseEdit)
            popupBaseEdit.ShowPopup();
    }

    private void txeEdit_EditValueChanged(object sender, EventArgs e) => Close();

    private static T? Input<T>(BaseEdit baseEdit) where T : struct
    {
        var form = new InputForm(baseEdit);
        form.ShowDialog();

        if (form._baseEdit.EditValue is T value)
            return value;

        return null;
    }

    public static Color? InputColor() => Input<Color>(new ColorEdit());

    public static DateTime? InputDate() => Input<DateTime>(new DateEdit());

    public static decimal? InputNumber() => Input<decimal>(new SpinEdit());

    public static string InputText()
    {
        var form = new InputForm(new TextEdit());
        form.ShowDialog();

        return ((TextEdit) form._baseEdit).Text;
    }
}