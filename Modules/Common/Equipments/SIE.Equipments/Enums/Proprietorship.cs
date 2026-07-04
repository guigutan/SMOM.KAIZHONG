using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 产权归属
    /// </summary>
    public enum Proprietorship
    {
        /// <summary>
        /// 自有
        /// </summary>
        [Label("自有")]
        Own = 5,

        /// <summary>
        /// 租赁
        /// </summary>
        [Label("租赁")]
        Lease = 10,

        /// <summary>
        /// 客供
        /// </summary>
        [Label("客供")]
        ByCustomer = 15,
    }
}