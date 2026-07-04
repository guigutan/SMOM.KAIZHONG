using SIE.Inventory.Commom;
using SIE.Inventory.Transactions;
using SIE.Items;

namespace SIE.Inventory.TransactionProcessing
{
    /// <summary>
    /// 库存事务交易数据
    /// </summary>
    public class InvCollectData
    {
        /// <summary>
        /// 物料
        /// </summary>
        public Item item { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public Lot lot { get; set; }

        /// <summary>
        /// 交易双方参数
        /// </summary>
        public StockTrans stockTrans { get; set; }

        /// <summary>
        /// 交易相关数据
        /// </summary>
        public BaseTransactionData baseTransactionData { get; set; }

        /// <summary>
        /// 来源库位：库存是否自分配数，
        /// 是则扣减来源库位分配数，否则扣减来源可用数（一般用于发货时取已分配好数量）
        /// </summary>
        public bool isFromAllottedQty { get; set; }

        /// <summary>
        /// 目标库位：库存是否至分配数，
        /// 是则增加目标库位分配数，否则增加可用数（一般用于发货时锁定中转库位库存）
        /// </summary>
        public bool isToAllottedQty { get; set; }

        /// <summary>
        /// 是否验证LPN多库位存储
        /// </summary>
        public bool isValidateMulLoc { get; set; }

        /// <summary>
        /// 是否验证LPN库存状态
        /// </summary>
        public bool isValidateState { get; set; } = true;

        /// <summary>
        /// 是否收货
        /// </summary>
        public bool isReceive { get; set; }

        /// <summary>
        /// 是否不验证未质检库存(超期复检)
        /// </summary>
        public bool isNotCheckNone { get; set; }

        /// <summary>
        /// 是否更新垛表
        /// </summary>
        public bool IsUpdatePile { get; set; } = true;

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType? BusinessType { get; set; } = null;

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; } = string.Empty;

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; } = string.Empty;

        /// <summary>
        /// 是否忽略扩展属性
        /// </summary>
        public bool IsIgnoreItemExtProp { get; set; }

        /// <summary>
        /// 辅助单位单价
        /// </summary>
        public decimal? PurUnitPrice { get; set; }

        /// <summary>
        /// 事务交易所属的库存组织 只用于当前库存组织写其他库存组织的事务交易
        /// </summary>
        public int? InvOrgId { get; set; }
    }
}
