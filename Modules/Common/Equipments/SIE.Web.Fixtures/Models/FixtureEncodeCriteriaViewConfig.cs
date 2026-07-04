using SIE.Fixtures;
using SIE.Fixtures.Models;

namespace SIE.Web.Fixtures.Models
{
    /// <summary>
    /// 工治具编码查询实体-界面
    /// </summary>
    internal class FixtureEncodeCriteriaViewConfig : WebViewConfig<FixtureEncodeCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("工治具编码").Show(ShowInWhere.Detail);
                View.Property(p => p.FixtureModelName).Show(ShowInWhere.Detail);
                View.Property(p => p.ManageMode).Show(ShowInWhere.Detail);
                View.Property(p => p.FixtureType).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureTypes(pagingInfo, keyword);
                }).Show(ShowInWhere.Detail);
            }
        }
    }
}
