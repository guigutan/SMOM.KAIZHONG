using SIE.ObjectModel;

namespace SIE.Core.WorkOrders
{
    /// <summary>
    /// 工单类型
    /// </summary>
    public enum WorkOrderType
    {
        /// <summary>
        /// 量产
        /// </summary>
        [Label("量产")]
        Mass = 0,

        /// <summary>
        /// 试产
        /// </summary>
        [Label("试产")]
        Pilot = 1,

        /// <summary>
        /// 返工
        /// </summary>
        [Label("返工")]
        Rework = 2,

        /// <summary>
        /// 校验
        /// </summary>
        [Label("校验")]
        Verify = 3,

        ///// <summary>
        ///// 补料
        ///// </summary>
        //[Label("补料")]
        //Replenishment = 5,

        ///// <summary>
        ///// 退库
        ///// </summary>
        //[Label("退库")]
        //ReturnWarehouse = 6,

        ///// <summary>
        ///// 内协
        ///// </summary>
        //[Label("内协")]
        //IntelCoordination = 7,

        ///// <summary>
        ///// 退货
        ///// </summary>
        //[Label("退货")]
        //BackGoods = 8,
    }
}