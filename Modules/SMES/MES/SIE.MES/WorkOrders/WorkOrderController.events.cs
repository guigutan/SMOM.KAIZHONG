namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单控制器
    /// </summary>
    partial class WorkOrderController
    {
        /// <summary>
        /// 通知恢复工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        protected virtual void NotifyResume(WorkOrder workOrder)
        {
            var e = new WorkOrderResumeEvent { WorkOrderId = workOrder.Id };
            RT.EventBus.Publish(e);
        }

        /// <summary>
        /// 通知暂停工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        protected virtual void NotifyPause(WorkOrder workOrder)
        {
            var e = new WorkOrderPauseEvent { WorkOrderId = workOrder.Id };
            RT.EventBus.Publish(e);
        }

        /// <summary>
        /// 通知取消工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        protected virtual void NotifyCancel(WorkOrder workOrder)
        {
            var e = new WorkOrderCancelEvent { WorkOrderId = workOrder.Id };
            RT.EventBus.Publish(e);
        }

        /// <summary>
        /// 通知关闭工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        protected virtual void NotifyClose(WorkOrder workOrder)
        {
            var e = new WorkOrderCloseEvent { WorkOrderId = workOrder.Id };
            RT.EventBus.Publish(e);
        }

        /// <summary>
        /// 通知发放工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        protected virtual void NotifyRelease(WorkOrder workOrder)
        {
            var e = new WorkOrderReleaseEvent { WorkOrderId = workOrder.Id };
            RT.EventBus.Publish(e);
        }

        /// <summary>
        /// 通知取消发放工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        protected virtual void NotifyCancelRelease(WorkOrder workOrder)
        {
            var e = new WorkOrderCancelReleaseEvent { WorkOrderId = workOrder.Id };
            RT.EventBus.Publish(e);
        }
    }
}