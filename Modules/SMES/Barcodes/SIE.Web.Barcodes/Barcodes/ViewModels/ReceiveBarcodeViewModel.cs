using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Web.Barcodes.ViewModels
{
    /// <summary>
    /// 条码领用视图模型
    /// </summary>
    [Serializable, RootEntity]
    [Label("条码领用")]
    public class ReceiveBarcodeViewModel : ViewModel
    {
        #region 用户 UserName
        /// <summary>
        /// 用户
        /// </summary>
        [Label("用户名")]
        [Required]
        public static readonly Property<string> UserNameProperty = P<ReceiveBarcodeViewModel>.Register(e => e.UserName);

        /// <summary>
        /// 用户
        /// </summary>
        public string UserName
        {
            get { return this.GetProperty(UserNameProperty); }
            set { this.SetProperty(UserNameProperty, value); }
        }
        #endregion

        #region 密码 Password
        /// <summary>
        /// 密码
        /// </summary>
        [Label("密码")]
        public static readonly Property<string> PasswordProperty = P<ReceiveBarcodeViewModel>.Register(e => e.Password);

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return this.GetProperty(PasswordProperty); }
            set { this.SetProperty(PasswordProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 条码领用视图配置
    /// </summary>
    internal class ReceiveBarcodeViewModelViewConfig : WebViewConfig<ReceiveBarcodeViewModel>
    {
        /// <summary>
        /// 详细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.UserName);
            View.Property(p => p.Password).UsePasswordEditor();
        }
    }
}