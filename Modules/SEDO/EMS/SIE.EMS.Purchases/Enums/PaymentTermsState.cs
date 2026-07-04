using SIE.ObjectModel;

namespace SIE.EMS.Purchases.Enums
{
    /// <summary>
    /// 付款条件状态
    /// </summary>
    public enum PaymentTermsState
    {
        /// <summary>
        /// 未申请
        /// </summary>
        [Label("未申请")]
        NotApplied = 0,
        /// <summary>
        /// 已申请
        /// </summary>
        [Label("已申请")]
        Applied = 1
    }
}
