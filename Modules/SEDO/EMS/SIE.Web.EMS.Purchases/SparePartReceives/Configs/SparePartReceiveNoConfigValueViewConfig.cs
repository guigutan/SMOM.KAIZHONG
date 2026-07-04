using SIE.EMS.Purchases.SparePartReceives.Configs;

namespace SIE.Web.EMS.Purchases.SparePartReceives.Configs
{
    /// <summary>
    /// 批次序列号编码规则配置界面
    /// </summary>
    internal class SparePartReceiveNoConfigValueViewConfig : WebViewConfig<SparePartReceiveNoConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.LotNumberRuleId).Show(ShowInWhere.All);
            View.Property(p => p.SnNumberRuleId).Show(ShowInWhere.All);
        }
    }
}
