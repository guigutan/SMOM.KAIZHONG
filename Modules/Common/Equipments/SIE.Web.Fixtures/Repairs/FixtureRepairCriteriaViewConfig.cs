using SIE.Fixtures.Repairs;

namespace SIE.Web.Fixtures.Repairs
{
    /// <summary>
    /// 工治具报修查询界面
    /// </summary>
    internal class FixtureRepairCriteriaViewConfig : WebViewConfig<FixtureRepairCriteria>
    {
        ///<summary>
        /// 配置查询视图 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.RepairState);
            View.Property(p => p.ApplyDate).UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.Month;
            });
            View.Property(p => p.RepairDate).UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.Month;
            }).HasLabel("维修时间");
        }
    }
}
