using SIE.ObjectModel;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 序列号库存状态
    /// </summary>
    public enum OrdNumStoreStatus
    {
        /// <summary>
        /// 入库
        /// </summary>
        [Label("入库")]
        In = 0,

        /// <summary>
        /// 出库
        /// </summary>
        [Label("出库")]
        Out = 1,

        /// <summary>
        /// 使用中
        /// </summary>
        [Label("使用中")]
        Using = 2,

        /// <summary>
        /// 委外维修
        /// </summary>
        [Label("委外维修")]
        Outsourced = 3,
    }
}