using SIE.ObjectModel;

namespace SIE.CSM.Suppliers
{
    /// <summary>
    /// 采购供应模式
    /// </summary>
    public enum PurchaseSupplyType
    {
        /// <summary>
        /// 普通
        /// </summary>
        [Label("普通")]
        General = 0,

        /// <summary>
        /// VMI
        /// </summary>
        [Label("VMI")]
        VMI = 1,
    }
}
