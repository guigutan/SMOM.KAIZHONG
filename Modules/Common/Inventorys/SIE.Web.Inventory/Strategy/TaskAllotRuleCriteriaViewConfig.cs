using SIE.Inventory.Strategy;

namespace SIE.Web.Inventory.Strategy
{
    /// <summary>
    /// 任务分配规则查询实体视图配置
    /// </summary>
    public class TaskAllotRuleCriteriaViewConfig : WebViewConfig<TaskAllotRuleCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.OperationType).Show();
                View.Property(p => p.WarehouseId).Show();
                View.Property(p => p.LogicArea).Show();
                View.Property(p => p.ItemCategory).Show();
                View.Property(p => p.Employee).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
            }
        }
    }
}
