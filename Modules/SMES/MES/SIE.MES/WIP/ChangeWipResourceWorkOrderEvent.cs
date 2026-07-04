using System;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 切换产线在制工单命令
    /// </summary>
    [Serializable]
    public class ChangeWipResourceWorkOrderEvent
    {
        /// <summary>
        /// 工单
        /// </summary>
        public double WorkOrderId { get; set; }
    }
}
