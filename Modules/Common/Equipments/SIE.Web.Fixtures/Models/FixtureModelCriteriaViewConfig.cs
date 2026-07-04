using SIE.Fixtures;
using SIE.Fixtures.Models;

namespace SIE.Web.Fixtures.Models
{
    /// <summary>
    /// 工治具型号查询实体视图配置
    /// </summary>
    internal class FixtureModelCriteriaViewConfig : WebViewConfig<FixtureModelCriteria>
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
                View.Property(p => p.Name).Show(ShowInWhere.Detail);
                View.Property(p => p.ManageMode).Show(ShowInWhere.Detail);
                View.Property(p => p.FixtureType).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureTypes(pagingInfo, keyword);
                }).Show(ShowInWhere.Detail);
            }
        }
    }
}
