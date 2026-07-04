using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 接收类型
    /// </summary>
    public enum ReceiveType
    {
        /// <summary>
        /// 采购接收
        /// </summary>
        [Category("SparePartReceive")]
        [Label("采购接收")]
        Purchase = 10,
        /// <summary>
        /// 赠品接收
        /// </summary>
        [Category("SparePartReceive")]
        [Label("赠品接收")]
        Giveaway = 20,
        /// <summary>
        /// 客供接收
        /// </summary>
        [Label("客供接收")]
        Customer = 30,
        /// <summary>
        /// 租赁接收
        /// </summary>
        [Category("SparePartReceive")]
        [Label("租赁接收")]
        Lease = 40,
        /// <summary>
        /// 委外返厂
        /// </summary>
        [Category("SparePartReceive")]
        [Label("委外返厂")]
        Outsourced = 50,
        /// <summary>
        /// 其他接收
        /// </summary>
        [Category("SparePartReceive")]
        [Label("其他接收")]
        Other = 60,
    }
}