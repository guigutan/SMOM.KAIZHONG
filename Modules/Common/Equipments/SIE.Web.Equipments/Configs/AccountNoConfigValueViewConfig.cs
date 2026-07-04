using SIE.Equipments.Configs;

namespace SIE.Web.Equipments.Configs
{
    /// <summary>
    /// 报检单号配置值视图配置
    /// </summary>
    internal class AccountNoConfigValueViewConfig : WebViewConfig<AccountNoConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.NumberRule).Show(ShowInWhere.All);
        }
    }
}
