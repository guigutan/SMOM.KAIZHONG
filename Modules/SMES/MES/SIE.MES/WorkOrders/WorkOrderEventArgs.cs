namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单事件基类
    /// </summary>
    public class WorkOrderEvent
    {
        /// <summary>
        /// 工单
        /// </summary>
        public double WorkOrderId { get; set; }
    }

    /// <summary>
    /// 工单创建事件
    /// </summary>
    public class WorkOrderCreateEvent : WorkOrderEvent { }

    /// <summary>
    /// 工单修改事件
    /// </summary>
    public class WorkOrderEditEvent : WorkOrderEvent { }

    /// <summary>
    /// 工单暂停
    /// </summary>
    public class WorkOrderPauseEvent : WorkOrderEvent { }

    /// <summary>
    /// 工单恢复
    /// </summary>
    public class WorkOrderResumeEvent : WorkOrderEvent { }

    /// <summary>
    /// 工单取消
    /// </summary>
    public class WorkOrderCancelEvent : WorkOrderEvent { }

    /// <summary>
    /// 工单关闭
    /// </summary>
    public class WorkOrderCloseEvent : WorkOrderEvent { }

    /// <summary>
    /// 工单发放
    /// </summary>
    public class WorkOrderReleaseEvent : WorkOrderEvent { }

    /// <summary>
    /// 工单取消发放
    /// </summary>
    public class WorkOrderCancelReleaseEvent : WorkOrderEvent { }

    /// <summary>
    /// 任务单删除
    /// </summary>
    public class DispatchTaskDeleteEvent : WorkOrderEvent { }

    /// <summary>
    /// 工单开始生产
    /// </summary>
    public class WorkOrderProductingEvent : WorkOrderEvent
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId { get; set; }
    }

    /// <summary>
    /// 工单完成
    /// </summary>
    public class WorkOrderFinishedEvent : WorkOrderEvent { }

    /// <summary>
    /// 工单修改更新工单任务单事件
    /// </summary>
    public class WorkOrderUpdateDispathTaskEvent : WorkOrderEvent
    {
        /// <summary>
        /// 工序BOM已变更
        /// </summary>
        public bool ProcessBomDirty { get; set; }
    }
}
