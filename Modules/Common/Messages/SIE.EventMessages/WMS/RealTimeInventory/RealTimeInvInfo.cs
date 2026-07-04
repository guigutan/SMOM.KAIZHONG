using System;

namespace SIE.EventMessages.RealTimeInventory
{
    /// <summary>
    /// 实时库存
    /// </summary>
    [Serializable]
    public class RealTimeInvInfo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 现有数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal AvailableQty { get; set; }

        /// <summary>
        /// 分配数量
        /// </summary>
        public decimal AllottedQty { get; set; }

        /// <summary>
        /// 冻结数量
        /// </summary>
        public decimal FreezingQty { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 实际库存类型
        /// </summary>
        public int RealTimeInvType { get; set; }
    }
}
