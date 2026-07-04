using System;

namespace SIE.EventMessages.WMS
{
    /// <summary>
    /// 序列号数据
    /// </summary>
    [Serializable]
    public class PackingLabelSnData
    {
        /// <summary>
        /// 状态 10创建 20入库 30出库 40分配 50待退 60冻结
        /// </summary>
        public int SnState { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId { get; set; }

        /// <summary>
        /// 序列号条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public int OnhandState { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public double? LotId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId { get; set; }

        /// <summary>
        /// 是否序列号
        /// </summary>
        public bool IsSer { get; set; }
    }
}
