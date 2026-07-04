using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Abnormals;

namespace SIE.Web.Fixtures.Abnormals
{
    /// <summary>
    /// 工治具异常类型查询体视图配置
    /// </summary>
    internal class FixtureAbnormalCriteriaViewConfig : WebViewConfig<FixtureAbnormalCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.Detail);
                View.Property(p => p.AbnormalType).Show(ShowInWhere.Detail);
                View.Property(p => p.Description).Show(ShowInWhere.Detail);
                View.Property(p => p.FixtureType).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureTypes(pagingInfo, keyword);
                }).Show(ShowInWhere.Detail);
            }
        }
    }
}
