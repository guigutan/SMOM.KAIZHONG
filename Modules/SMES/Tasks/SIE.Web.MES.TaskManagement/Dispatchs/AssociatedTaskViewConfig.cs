using SIE.ManagedProperty;
using SIE.MES.TaskManagement.Dispatchs;

namespace SIE.Web.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 关联任务单视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class AssociatedTaskViewConfig : WebViewConfig<AssociatedTask>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.TaskAssociated).Readonly().ShowInList();
            View.Property(p => p.TaskNo).Readonly().ShowInList();
            View.Property(p => p.TaskDispatchQty).Readonly().ShowInList();
            View.Property(p => p.TaskReportQty).Readonly().ShowInList();
            View.Property(p => p.TaskNgQty).Readonly().ShowInList();
            View.Property(p => p.TaskTaskStatus).ShowInList();
            View.Property(p => p.TaskPriority).Readonly().ShowInList();
            View.Property(p => p.TaskTaskPerformer).Readonly().ShowInList();
            View.Property(p => p.TaskPlanBeginTime).Readonly().ShowInList();
            View.Property(p => p.TaskPlanEndTime).Readonly().ShowInList();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
