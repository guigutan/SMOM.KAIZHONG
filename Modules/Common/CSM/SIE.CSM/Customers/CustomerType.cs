using SIE.ObjectModel;

namespace SIE.CSM.Customers
{
    /// <summary>
    /// 类型
    /// </summary>
    public enum CustomerType
    {
        /// <summary>
        /// 客户
        /// </summary>
        [Label("客户")]
        CUSTOMER,

        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
        SHIPPER,

        /// <summary>
        /// 承运人
        /// </summary>
        [Label("承运人")]
        CARRIER,
    }
}
