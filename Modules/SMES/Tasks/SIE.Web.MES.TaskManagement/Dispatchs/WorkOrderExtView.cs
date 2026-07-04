using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.WorkOrders;
using SIE.Web.MES.TaskManagement.Dispatchs.Commands;

namespace SIE.Web.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 工单扩展视图
    /// </summary>
    internal class WorkOrderExtView : WebViewConfig<WorkOrder>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.UseCommand(GenerateTaskBIllCommand.FullName);
            View.RequirModuleResource("SIE.Web.MES.TaskManagement.Dispatchs.Scripts.WorkOrderBehavior.js");
            View.AddBehavior("SIE.Web.MES.TaskManagement.WorkOrderBehavior");
            View.AttachChildrenProperty(typeof(DispatchTask), (o) =>
            {
                var arg = o as ChildPagingDataArgs;
                var workOrder = arg.Parent as WorkOrder;
                if (workOrder == null) return null;
                var list = RT.Service.Resolve<DispatchController>().GetDispatchTasks(workOrder.Id, arg.PagingInfo);
                return list;
            }, DispatchTaskViewConfig.workOrderTaskView).HasLabel("任务单列表");
        }
    }
}