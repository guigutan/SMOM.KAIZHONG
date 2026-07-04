using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SIE.CrossPlatform.Collect.Common.ApiCall;
using SIE.CrossPlatform.Collect.Common.Controls;
using SIE.CrossPlatform.Collect.Common.Extensions;

namespace SIE.CrossPlatform.Collect.Common.Forms;

public partial class XPFormChangeInvOrg : BaseDialog
{
    public XPFormChangeInvOrg()
    {
        InitializeComponent();
        this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        this.Loaded += ChangeInvOrg_Loaded;
        this.XPDialogTitle.ATitle = "切换库存组织".L10N();
        this.XPDialogTitle.AOkClick += XPDialogTitle_AOkClick;
        this.PointerPressed += XPFormChangeInvOrg_PointerPressed;
    }
    private void XPFormChangeInvOrg_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.Pointer.Type == PointerType.Mouse)
        {
            this.BeginMoveDrag(e);
        }
    }
    private async void XPDialogTitle_AOkClick(object? sender, EventArgs e)
    {
        if (xpListBox1.SelectedIndex >= 0)
        {
            InvOrg newOrg = LoginInfo.Instance.AllInvOrgs[xpListBox1.SelectedIndex];
            await ApiHelper.PostAsync<string>("UserController", "changeinvorg", OnChangeInvOrgCallback, LoginInfo.Instance.UserId, newOrg.Code);
        }

        Close(true);//关闭并返回值
    }

    private void ChangeInvOrg_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.xpListBox1.ItemsSource = LoginInfo.Instance.AllInvOrgs;

        if (LoginInfo.Instance.AllInvOrgs.Count > 0)
        {
            var index = LoginInfo.Instance.AllInvOrgs.FindIndex(m => m.Id == LoginInfo.Instance.InvOrgId);
            this.xpListBox1.SelectedIndex = index;
        }
    }
    private void OnChangeInvOrgCallback<T>(ApiResult<T> result, string apiType, string method, string postData)
    {
        if (!result.Success)
        {
            MessageBox.ShowMessage(result.Message);
            return;
        }

        InvOrg newOrg = LoginInfo.Instance.AllInvOrgs[xpListBox1.SelectedIndex];
        LoginInfo.Instance.InvOrgId = newOrg.Id;
    }
}