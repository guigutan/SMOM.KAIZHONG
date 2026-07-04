using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Microsoft.Identity.Client;
using SIE.CrossPlatform.Collect.Common.ApiCall;
using SIE.CrossPlatform.Collect.Common.Controls;
using SIE.CrossPlatform.Collect.Common.Extensions;
using SIE.CrossPlatform.Collect.Common.Settings;
using SIE.CrossPlatform.Collect.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SIE.CrossPlatform.Collect
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel MainViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.xpTitle.ATitle = "";
            this.WindowState = WindowState.Maximized;
            MainViewModel = new MainWindowViewModel();
            this.DataContext = MainViewModel;
            this.PointerPressed += MainWindow_PointerPressed;
        }

        private void MainWindow_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (e.Pointer.Type == PointerType.Mouse)
            {
                this.BeginMoveDrag(e);
            }
        }


        /// <summary>
        /// 过站采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var moveWin = new MoveWindow();
            var result = await moveWin.ShowDialog<bool>(this);
            if (result)
            {
                this.Show();
            }
        }
        private async void Button_Login_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Login();
        }


        private void Window_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            //to do  
        }

        private async void Login()
        {
            LoginInfo.Instance.UserCode = "liyi"; //"liyi";
            LoginInfo.Instance.Password = "666666";
            AppSettings.Instance.ApiUrl = "192.168.175.137:1154";
            try
            {
                MainViewModel.IsBusy = true;
                await ApiHelper.Login(LoginCallback);
                MainViewModel.IsBusy = false;
            }
            catch (Exception ex)
            {
                MainViewModel.IsBusy = false;
                MessageBox.ShowMessage(ex.Message);
            }
        }
        private async void LoginCallback<T>(ApiResult<T> result, string apiType, string method, string postData)
        {
            if (LoginInfo.Instance.UserId <= 0) //登录失败
            {
                this.txtStatus.Text = string.Format("{0}{1}", "登录状态:", result.Message);
                MainViewModel.IsBusy = false;
                return;
            }
            //获取库存组织
            await ApiHelper.PostAsync<List<InvOrg>>("InvOrgController", "getinvorguser", OnGetInvorgUserCallback, LoginInfo.Instance.UserId.ToString());


            LoginInfo.Instance.SaveHistoryAccount(); //登录成功后记住账号
            LoginInfo.Instance.RemberLanguage = Global.Language == null ? "" : Global.Language.Code;
            this.xpTitle.AUserInfo = LoginInfo.Instance.UserName;

            //设置组织架构名称
            InvOrg org = LoginInfo.Instance.AllInvOrgs.Find(p => p.Code == LoginInfo.Instance.InvOrgId);
            if (org != null)
                this.xpTitle.AInvOrg =org.Name;
            else
                this.xpTitle.AInvOrg ="请设置组织架构".L10N();
            this.txtStatus.Text = string.Format("{0}{1}", "登录状态:", "成功");
        }
        private void OnGetInvorgUserCallback<T>(ApiResult<T> result, string apiType, string method, string postData)
        {
            if (!result.Success)
            {
                MainViewModel.IsBusy = false;
                //ShowTips(result.Message);
                return;
            }
            LoginInfo.Instance.AllInvOrgs = result.Result as List<InvOrg>;


        }

    }
}