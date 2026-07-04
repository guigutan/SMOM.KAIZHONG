using SIE.Inventory.Task;
using SIE.Web.Warehouses;
using System;

namespace SIE.Web.Inventory.Task
{
    /// <summary>
    /// 任务管理查询 视图配置
    /// </summary>
    class TaskManagementCriteriaViewConfig : WebViewConfig<TaskManagementCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("任务管理查询".L10N());
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.All);
                View.Property(p => p.TaskGroupNo).Show(ShowInWhere.All);
                View.Property(p => p.BillNo).Show(ShowInWhere.All);
                View.Property(p => p.State).UseEnumMutilEditor(p => p.EnumType = typeof(TaskState)).Show(ShowInWhere.All);
                View.Property(p => p.Level).Show(ShowInWhere.All);
                View.Property(p => p.OperationType).Show(ShowInWhere.All);
                View.Property(p => p.FromWarehouse).UseAllWarehouseEditor().Show(ShowInWhere.All);
                View.Property(p => p.ToWarehouse).UseAllWarehouseEditor().Show(ShowInWhere.All);
                View.Property(p => p.Item).Show(ShowInWhere.All);
                View.Property(p => p.SuggestToLoc).Show(ShowInWhere.All);
                View.Property(p => p.Lot).Show(ShowInWhere.All);
                View.Property(p => p.LPN).Show(ShowInWhere.All);
                View.Property(p => p.SuggestToLpn).Show(ShowInWhere.All);
                View.Property(p => p.ActualFromLpn).Show(ShowInWhere.All);
                View.Property(p => p.ActualToLpn).Show(ShowInWhere.All);
                View.Property(p => p.ActualOperator).Show(ShowInWhere.All);
                View.Property(p => p.ReleaseDate).Show(ShowInWhere.All);
                ////View.Property(p => p.EndDateTime).UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.All; }).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
                {
                    p.DateFormat = "Y/m/d";
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show();
                View.Property(p => p.CreateById).Show();
            }
        }
    }
}
