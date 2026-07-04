using SIE.ObjectModel;

namespace SIE.CSM.Suppliers
{
    /// <summary>
    /// 委外收货扣料处理
    /// </summary>
    public enum OutsourcingReceiveType
    {
        /// <summary>
        /// 库存能耗尽耗
        /// </summary>
        [Label("库存能耗尽耗")]
        UseAllHand = 0,

        /// <summary>
        /// 仅创建耗料单
        /// </summary>
        [Label("仅创建耗料单")]
        OnlyCreateBill = 1,

        /// <summary>
        /// 创建耗料单并分配库存
        /// </summary>
        [Label("创建耗料单并分配库存")]
        CreateBillAndAllot = 2,

        /// <summary>
        /// 耗料不足禁止收货
        /// </summary>
        [Label("耗料不足禁止收货")]
        NoHandDisabled = 3,
    }
}
