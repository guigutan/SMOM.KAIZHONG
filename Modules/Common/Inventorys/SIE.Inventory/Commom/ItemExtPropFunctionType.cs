using SIE.ObjectModel;

namespace SIE.Inventory.Commom
{
    /// <summary>
	/// 功能类型
	/// </summary>
	public enum ItemExtPropFunctionType
    {
        /// <summary>
        /// ASN单据
        /// </summary>
        [Label("ASN")]
        ASN = 0,

        /// <summary>
        /// PO单据
        /// </summary>
        [Label("PO")]
        PO = 1,

        /// <summary>
        /// 移动单据
        /// </summary>
        [Label("MOVE")]
        MOVE = 2,

        /// <summary>
        /// 调拨单据
        /// </summary>
        [Label("ALLOCATE")]
        ALLOCATE = 3,

        /// <summary>
        /// 调整单据
        /// </summary>
        [Label("ADJUST")]
        ADJUST = 4,

        /// <summary>
        /// 冻结单据
        /// </summary>
        [Label("FROZEN")]
        FROZEN = 5,

        /// <summary>
        /// 盘点单据
        /// </summary>
        [Label("COUNT")]
        COUNT = 6,

        /// <summary>
        /// 发运单据
        /// </summary>
        [Label("SHIPMENT")]
        SHIPMENT = 7,

        /// <summary>
        /// LOT单据
        /// </summary>
        [Label("LOT")]
        LOT = 8,

        /// <summary>
        /// 库存报检
        /// </summary>
        [Label("INSPECTION")]
        INSPECTION = 9,

        /// <summary>
        /// IQC复检
        /// </summary>
        [Label("IQCRECHECK")]
        IQCRECHECK = 10,

        /// <summary>
        /// 补货计划
        /// </summary>
        [Label("REPLENISHPLAN")]
        REPLENISHPLAN = 11,

        /// <summary>
        /// 波次计划
        /// </summary>
        [Label("WAVEPLAN")]
        WAVEPLAN = 12,

        /// <summary>
        /// 收发控制
        /// </summary>
        [Label("ITEMIOLIMIT")]
        ITEMIOLIMIT = 13,

        /// <summary>
        /// 补货规则
        /// </summary>
        [Label("REPLENISHRULE")]
        REPLENISHRULE = 14,

        /// <summary>
        /// 播种池
        /// </summary>
        [Label("SOWPOOL")]
        SOWPOOL = 15,
    }
}