using Avalonia.Controls;

namespace SIE.CrossPlatform.Collect.Common.Controls;

public partial class XPDialogTitle : UserControl
{
    public Window ParentForm { get; set; }

    public XPDialogTitle()
    {
        InitializeComponent();
        this.Loaded += XPDialogTitle_Loaded;
    }

    private void XPDialogTitle_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.ParentForm = GetWindow();
    }

    public string ATitle
    {
        get { return this.labelTitle.Text; }
        set { this.labelTitle.Text = value; }
    }


    #region 取消按钮事件
    /// <summary>
    /// 取消按钮事件
    /// </summary>
    public event EventHandler ACancelClick;

    private void buttonCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ACancelClick == null)
        {
            //默认事件
            if (this.ParentForm != null)
                this.ParentForm.Close();
        }
        else
        {
            ACancelClick.Invoke(this, e);
        }
    }
    #endregion

    #region 确定按钮事件
    /// <summary>
    /// 确定按钮事件
    /// </summary>
    public event EventHandler AOkClick;

    private void buttonOk_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (AOkClick == null)
        {
            //默认事件
        }
        else
        {
            AOkClick.Invoke(this, e);
        }
    }
    #endregion
    /// <summary>
    /// 获取用户控件的窗体
    /// </summary>
    /// <returns></returns>
    private Window GetWindow()
    {
        var topLevel = TopLevel.GetTopLevel(this);
        //默认事件
        if (topLevel is Window parentWin)
        {
            return parentWin;
        }
        return null;
    }
}