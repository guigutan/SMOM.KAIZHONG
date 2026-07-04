using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.WIP.TaskExtensions;
using System;
using System.Linq;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 任务自动报工
    /// </summary>
    class WipTaskReport : IWipTaskReport
    {
        /// <summary>
        /// 获取工单报工任务
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="retrospectType">追溯方式</param>
        /// <param name="processId">工序Id</param>
        /// <returns>报工任务列表</returns>
        public EntityList<ReportTaskViewModel> GetReportTasks(double employeeId, Core.Items.RetrospectType retrospectType, double processId)
        {
            var controller = RT.Service.Resolve<DispatchController>();
            var tasks = RT.Service.Resolve<DispatchController>().GetEmployeeRefDispatchTasks(employeeId, retrospectType, processId);
            var result = new EntityList<ReportTaskViewModel>();
            if (tasks.Count == 0)
                return result;
            EntityList<DispatchTask> orderTasks = new EntityList<DispatchTask>();
            var config = controller.GetDispatchTaskConfig();
            if (config.ReportOrder.HasValue)
            {
                if (config.ReportOrder == ReportOrder.Priority)
                    orderTasks.AddRange(tasks.OrderByDescending(p => p.Priority));
                else if (config.ReportOrder == ReportOrder.BeginDate)
                    orderTasks.AddRange(tasks.OrderBy(p => p.PlanBeginTime));

            }
            else
                orderTasks.AddRange(tasks);
            orderTasks.ForEach(task =>
            {
                result.Add(new ReportTaskViewModel()
                {
                    No = task.No,
                    BeginTime = task.BeginTime,
                    DispatchQty = task.DispatchQty,
                    EndTime = task.EndTime,
                    ToReportQty = task.DispatchQty - task.ReportQty,
                    NgQty = task.NgQty,
                    OkQty = task.OkQty,
                    CreateDate = task.CreateDate,
                    PlanBeginTime = task.PlanBeginTime,
                    PlanEndTime = task.PlanEndTime,
                    Priority = task.Priority.ToLabel(),
                    ProcessName = task.ProcessName,
                    ProductCode = task.ProductCode,
                    ReportQty = task.ReportQty,
                    ResourceName = task.ResourceName,
                    TaskStatus = task.TaskStatus.ToLabel(),
                    WorkOrderNo = task.WorkOrderNo,
                    WorkOrderId = task.WorkOrderId ?? 0
                });
            });
            return result;
        }

        /// <summary>
        /// 验证工单任务自动报工
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="employeeId">员工Id</param>
        /// <param name="processId">工序ID</param>
        /// <returns>验证通过返回true，失败抛异常</returns>
        public bool ValidateAutoReport(double workOrderId, double employeeId, double processId)
        {
            return RT.Service.Resolve<ReportController>().ValidateWipReport(workOrderId, employeeId, processId);
        }

        /// <summary>
        /// 是否配置任务单生产
        /// </summary>
        /// <returns>任务单生产返回true，否则返回false</returns>
        public bool IsTaskWip()
        {
            return RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig()?.IsGenerate == true;
        }

        /// <summary>
        /// 工单是否自动报工
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>自动报工返回true，否则返回false</returns>
        public int? GetWorkOrdeReportModel(double workOrderId)
        {
            var reportModel = RT.Service.Resolve<DispatchController>().GetTaskReportModel(workOrderId);
            if (reportModel == null)
                return null;
            return (int)reportModel;
        }
    }
}