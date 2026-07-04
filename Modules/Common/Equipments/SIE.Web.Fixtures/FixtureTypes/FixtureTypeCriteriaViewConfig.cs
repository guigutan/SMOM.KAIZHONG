using SIE.Fixtures.FixtureTypes;

namespace SIE.Web.Fixtures.FixtureTypes
{
    /// <summary>
    /// 查询页面
    /// </summary>
    public class FixtureTypeCriteriaViewConfig : WebViewConfig<FixtureTypeCriteria>
    {
        /// <summary>
        /// 配置查询界面
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
