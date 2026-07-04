using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.WIP;
using SIE.MES.WorkOrders;
using SIE.Threading;
using System;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Events
{
    /// <summary>
    /// 自动报工监听
    /// </summary>
    public class TaskManagerListener
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static TaskManagerListener Instance { get; } = new TaskManagerListener();

        /// <summary>
        /// 订阅事件总线
        /// </summary>
        public void Start()
        {
            RT.EventBus.Subscribe<WipCollectedEvent>(this, e =>
            {
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<ReportController>().AutoTaskReport(e);
                }).WithCurrentThreadContext());
            });
            //任务关闭时触发自动报工剩余未报工数量进行报工
            RT.EventBus.Subscribe<DispatchTaskClose>(this, e =>
            {
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<ReportController>().AutoTaskReport(e.DispatchTasks);
                }).WithCurrentThreadContext());
            });
            RT.EventBus.Subscribe<WorkOrderCreateEvent>(this, e =>
            {
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<DispatchController>().WorkOrderGenerateDispathTask(e.WorkOrderId, true);
                }).WithCurrentThreadContext());
            });
            RT.EventBus.Subscribe<WorkOrderEditEvent>(this, e =>
            {
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<DispatchController>().WorkOrderGenerateDispathTask(e.WorkOrderId, false);
                }).WithCurrentThreadContext());
            });
            RT.EventBus.Subscribe<DispatchTaskDeleteEvent>(this, e =>
            {
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<DispatchController>().DeleteCancelDispatchTasks(e.WorkOrderId);
                }).WithCurrentThreadContext());
            });

            RT.EventBus.Subscribe<WorkOrderUpdateDispathTaskEvent>(this, e =>
            {
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<DispatchController>().WorkOrderUpdateDispathTask(e.WorkOrderId, e.ProcessBomDirty);
                }).WithCurrentThreadContext());
            });
        }
    }
}