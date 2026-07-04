using SIE.Fixtures;
using SIE.Fixtures.Warns;

namespace SIE.Web.Fixtures.Warns
{
    /// <summary>
    /// 工治具保养预警查询体-界面
    /// </summary>
    internal class FixtureWarnCriteriaViewConfig : WebViewConfig<FixtureWarnCriteria>
    {
        ///<summary>
        /// 配置查询视图 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.FixtureType).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CoreFixtureController>().GetFixtureTypes(pagingInfo, keyword);
            });
                View.Property(p => p.ModelCode);
            View.Property(p => p.EncodeCode);
        }
    }
}
