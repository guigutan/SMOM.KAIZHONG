using SIE.Fixtures.Models.Config;

namespace SIE.Web.Fixtures.Models.Config
{
    /// <summary>
    /// 配置项值页面
    /// </summary>
    public class FixturesModelsConfigValueViewConfig : WebViewConfig<FixturesModelsConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.NumberRuleId).Show().HasLabel("编码规则");
        }
    }
}
