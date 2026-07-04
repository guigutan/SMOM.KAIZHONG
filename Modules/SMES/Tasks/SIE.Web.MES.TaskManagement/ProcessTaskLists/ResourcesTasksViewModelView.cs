

using SIE.MES.TaskManagement.ProcessTaskLists;
using SIE.Web.MES.TaskManagement.ProcessTaskLists.Commands;

namespace SIE.Web.MES.TaskManagement.ProcessTaskLists
{
    /// <summary>
    /// 资源已派任务
    /// </summary>
    public class ResourcesTasksViewModelView : WebViewConfig<ResourcesTasksViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProcessTaskListViewModel));
        }


        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(TaskDeleteCommand).FullName,typeof(CancelDispatchTaskCommand).FullName);
            View.Property(p => p.No).ShowInList(120).Readonly();
            View.Property(p => p.ReportQty).ShowInList(100).Readonly();
            View.Property(p => p.DispatchQty).ShowInList(100).Readonly();
            View.Property(p => p.AssociatedWorkOrder).ShowInList(120).Readonly();

            View.Property(p => p.ProcessHourSum).ShowInList(150).Readonly();
            View.Property(p => p.ExpectedProductionTime).ShowInList(150).Readonly();
            View.Property(p => p.ProcessStandardHour).ShowInList(150).Readonly();
            View.Property(p => p.RemainingTime).ShowInList(150).Readonly().DisableSort();
            View.Property(p => p.ProcessName).ShowInList(100).Readonly();
            View.Property(p => p.TaskStatus).ShowInList(100).Readonly();
            View.Property(p => p.TaskPerformer).ShowInList(100).Readonly();
            View.Property(p => p.UpdateByName).ShowInList(120).Readonly();
            View.Property(p => p.UpdateDate).ShowInList(150).Readonly();
        }
    }
}
