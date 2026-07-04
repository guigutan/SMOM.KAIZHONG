using SIE.ObjectModel;

namespace SIE.EMS.Purchases.Enums
{
    /// <summary>
    /// 采购订单状态
    /// </summary>
    public enum PurchaseOrderStatus
    {
        /// <summary>
        /// 待收货
        /// </summary>
        [Label("待收货")]
        TobeRecive = 10,
        /// <summary>
        /// 部分收货
        /// </summary>
        [Label("部分收货")]
        PartRecive = 20,
        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已完成")]
        Complete = 30,
        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Close = 40,

        /// <summary>
        /// 已收货
        /// </summary>
        [Label("已收货")]
        Recived=50
    }
}
