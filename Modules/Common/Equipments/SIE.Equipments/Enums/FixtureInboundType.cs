using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 工治具入库类型
    /// </summary>
    public enum FixtureInboundType
    {
        /// <summary>
        /// 采购入库
        /// </summary>        
        [Label("采购入库")]
        Po = 10,
        /// <summary>
        /// 赠品入库
        /// </summary>

        [Label("赠品入库")]
        Gift = 20,
        /// <summary>
        /// 租赁入库
        /// </summary>

        [Label("租赁入库")]
        Lease = 30,
        /// <summary>
        /// 客供入库
        /// </summary>
        [Label("客供入库")]
        Guest = 40,
        /// <summary>
        /// 委外返厂入库
        /// </summary>

        [Label("委外返厂入库")]
        Outsourced = 50,
        /// <summary>
        /// 归还入库
        /// </summary>

        [Label("归还入库")]
        Return = 60,
        /// <summary>
        /// 其他接收入库
        /// </summary>

        [Label("其他接收入库")]
        Other = 70,

        /// <summary>
        /// 维修入库
        /// </summary>
        [Label("维修入库")]
        RepairIn = 80,

        /// <summary>
        /// 现场退库
        /// </summary>

        [Label("现场退库")]
        Scene = 90,
        /// <summary>
        /// 台账入库
        /// </summary>

        [Label("台账入库")]
        AccountIn = 100,
    }
}