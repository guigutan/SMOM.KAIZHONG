using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;

namespace SIE.Web.MES.TaskManagement.Dispatchs.ViewModels
{
    /// <summary>
    /// 任务详情ViewModel视图
    /// </summary>
    public class TaskDetailViewModelViewConfig : WebViewConfig<TaskDetailViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(DispatchTask));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.WithoutPaging();//去除分页
            View.Property(p => p.No).ShowInList(120).Readonly().DisableSort();
            View.Property(p => p.DispatchQty).ShowInList(80).Readonly().DisableSort();
            View.Property(p => p.WorkOrderNo).ShowInList(130).Readonly().HasLabel("工单编号");
            View.Property(p => p.ProductCode).ShowInList(130).Readonly().DisableSort();
            View.Property(p => p.Priority).UseEnumEditor(p => p.ColumnXType = "ReportDispatchPriorityComboBox").ShowInList(60).Readonly();
            View.Property(p => p.ProcessName).ShowInList(120).Readonly().HasLabel("工序");
            View.Property(p => p.ResourceName).ShowInList().Readonly().HasLabel("资源");
            View.Property(p => p.ReportQty).UseDisplayEditor(p => p.ColumnXType = "ReportDispatchTaskDisplay").ShowInList(120).Readonly().HasLabel("任务进度");
            View.Property(p => p.TaskStatus).UseEnumEditor().ShowInList(80).Readonly().HasLabel("任务状态");
            View.Property(p => p.SpecificationCode).ShowInList().Readonly();
            View.Property(p => p.SpecificationName).ShowInList().Readonly();
            View.Property(p => p.ReportMode).UseEnumEditor().ShowInList().Readonly().HasLabel("报工方式");
            View.Property(p => p.BeginTime).ShowInList(150).Readonly();
            View.Property(p => p.EndTime).ShowInList(150).Readonly();
            View.Property(p => p.PlanBeginTime).ShowInList(150).Readonly();
            View.Property(p => p.PlanEndTime).ShowInList(150).Readonly();
        }
    }
}
