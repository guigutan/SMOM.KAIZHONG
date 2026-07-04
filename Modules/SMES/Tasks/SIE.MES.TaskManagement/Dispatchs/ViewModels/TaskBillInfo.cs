using SIE.MES.WorkOrders;
using System;

namespace SIE.MES.TaskManagement.Dispatchs.ViewModels
{
    /// <summary>
    /// 任务单信息
    /// </summary>
    [Serializable]
    public class TaskBillInfo
    {
        /// <summary>
        /// 是否按照配置条件
        /// </summary>
        public bool IsAccordConfig { get; set; } = true;

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder { get; set; }
    }

    /// <summary>
    /// 任务单结果信息
    /// </summary>
    [Serializable]
    public class RstTaskBillInfo
    {
        /// <summary>
        /// 是否共模比报工
        /// </summary>
        public bool IsSyntype { get; set; }

        /// <summary>
        /// 产品族配置项是否共模比报工
        /// </summary>
        public bool OrgIsSyntype { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
