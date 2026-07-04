using SIE.EMS.FixedAssets.Accounts.Config;

namespace SIE.Web.EMS.FixedAssets.Accounts.Config
{
    /// <summary>
    /// 配置项值页面
    /// </summary>
    public class FixedAssetAccountConfigValueViewConfig : WebViewConfig<FixedAssetAccountConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.NumberRuleId).Show().HasLabel("资产编码规则");
            View.Property(p => p.DepreciationWay).Show();
        }
    }
}
