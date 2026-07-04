using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SIE.CrossPlatform.Collect.Common.ApiCall;
using SIE.CrossPlatform.Collect.Common.Controls;
using SIE.CrossPlatform.Collect.Common.Settings;
using SIE.CrossPlatform.Collect.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SIE.CrossPlatform.Collect;

public partial class TestPage : Window
{
    public TestPageViewModel TestViewModel { get; set; }
    public TestPage()
    {
        InitializeComponent();
        TestViewModel = new TestPageViewModel();
        this.DataContext = TestViewModel;
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //²âÊÔµÇÂ¼
        LoginInfo.Instance.UserCode = "liyi";
        LoginInfo.Instance.Password = "666666";
        AppSettings.Instance.ApiUrl = "192.168.175.137:1154";
        try
        {
            TestViewModel.IsBusy = true;
            await ApiHelper.Login(LoginCallback);
            
            TestViewModel.IsBusy = false;
        }
        catch (Exception ex)
        {
            MessageBox.ShowMessage(ex.Message);
        }

    }

    private void LoginCallback<T>(ApiResult<T> result, string apiType, string method, string postData)
    {
        if (LoginInfo.Instance.UserId <= 0) //µÇÂ¼Ê§°Ü
        {
            this.txtStatus.Text = string.Format("{0}{1}", "µÇÂ¼×´Ì¬:", result.Message);
            return;
        }
        LoginInfo.Instance.SaveHistoryAccount(); //µÇÂ¼³É¹¦ºó¼Ç×¡ÕËºÅ
        LoginInfo.Instance.RemberLanguage = Global.Language == null ? "" : Global.Language.Code;
        this.txtStatus.Text = string.Format("{0}{1}", "µÇÂ¼×´Ì¬:", "³É¹¦");
    }
}