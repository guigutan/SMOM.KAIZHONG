using SIE.ObjectModel;

namespace SIE.Core.Items
{
    /// <summary>
    /// 批次规则
    /// </summary>
    public enum BatchRule
    {
        /// <summary>
        /// 按产品分批
        /// </summary>
        [Label("按产品分批")]
        Product,

        /// <summary>
        /// 按载具分批
        /// </summary>
        [Label("按载具分批")]
        Vehicle,

        /// <summary>
        /// 按工单分批
        /// </summary>
        [Label("按工单分批")]
        WorkOrder,

        /// <summary>
        /// 按固定值
        /// </summary>
        [Label("按固定值")]
        FixedValue,

    }
}
