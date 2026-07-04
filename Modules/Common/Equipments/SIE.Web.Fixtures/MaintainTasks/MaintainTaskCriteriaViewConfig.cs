using SIE.Fixtures;
using SIE.Fixtures.MaintainTasks;

namespace SIE.Web.Fixtures.MaintainTasks
{
    /// <summary>
    /// 保养任务查询体-界面
    /// </summary>
    internal class MaintainTaskCriteriaViewConfig : WebViewConfig<MaintainTaskCriteria>
    {
        ///<summary>
        /// 配置查询视图 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.RelatedNo);
            View.Property(p => p.MaintainType);
            View.Property(p => p.IdCode);
            View.Property(p => p.EncodeCode);
            View.Property(p => p.FixtureType).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CoreFixtureController>().GetFixtureTypes(pagingInfo, keyword);
            });
            View.Property(p => p.State);
            View.Property(p => p.ApplyDate).UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.Month;
            });
            View.Property(p => p.FinishDate).UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.All;
            });
        }
    }
}
