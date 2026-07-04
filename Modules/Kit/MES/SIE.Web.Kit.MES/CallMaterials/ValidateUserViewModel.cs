using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 账号验证ViewModel
    /// </summary>
    [RootEntity]
    public class ValidateUserViewModel : ViewModel
    {
        #region 账号 Code
        /// <summary>
        /// 账号
        /// </summary>
        [Label("账号")]
        [Required]
        public static readonly Property<string> CodeProperty = P<ValidateUserViewModel>.Register(e => e.Code);

        /// <summary>
        /// 账号
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 密码 Pwd
        /// <summary>
        /// 密码
        /// </summary>
        [Label("密码")]
        public static readonly Property<string> PwdProperty = P<ValidateUserViewModel>.Register(e => e.Pwd);

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get { return this.GetProperty(PwdProperty); }
            set { this.SetProperty(PwdProperty, value); }
        }
        #endregion        
    }

    /// <summary>
    /// 账号验证视图配置
    /// </summary>
    public class ValidateUserViewConfig : WebViewConfig<ValidateUserViewModel>
    {
        /// <summary>
        /// 默认表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.Code);
            View.Property(p => p.Pwd).UsePasswordEditor();
        }
    }
}
