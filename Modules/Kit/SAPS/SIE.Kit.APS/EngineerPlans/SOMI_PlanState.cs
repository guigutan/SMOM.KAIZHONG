using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Kit.APS.EngineerPlans
{
    /// <summary>
    /// 工程计划状态
    /// </summary>
    public enum SOMI_PlanState
    {
        /// <summary>
        /// 未计划
        /// </summary>
        [Label("未计划")]
        [Category("AllowEdit")]
        WaitToPlan = 0,
        /// <summary>
        /// 已计划
        /// </summary>
        [Label("已计划")]
        [Category("AllowEdit")]
        Scheduled = 1,

        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已完成")]
        Finish = 2,

        /// <summary>
        /// 订单行已删除
        /// </summary>
        [Label("订单行已删除")]
        Deleted = 3

    }
}
