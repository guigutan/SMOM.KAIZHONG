using SIE.Warehouses;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 配置工作区查询实体视图
    /// </summary>
    internal class WorkAreaCriteriaViewConfig : WebViewConfig<WorkAreaCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.WarehouseId).Show();
                View.Property(p => p.StorageLocationId).Show();
                View.Property(p => p.EmployeeId).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show();
            }
        }
    }
}
