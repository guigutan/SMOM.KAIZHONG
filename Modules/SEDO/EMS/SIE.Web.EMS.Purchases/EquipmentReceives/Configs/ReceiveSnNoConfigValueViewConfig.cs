using SIE.EMS.Purchases.EquipmentReceives.Configs;

namespace SIE.Web.EMS.Purchases.EquipmentReceives.Configs
{
    /// <summary>
    /// 序列号编码规则配置视图配置
    /// </summary>
    internal class ReceiveSnNoConfigValueViewConfig : WebViewConfig<ReceiveSnNoConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.NumberRuleId).Show(ShowInWhere.All);
        }
    }
}
