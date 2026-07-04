using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.LesStockCounts.Datas
{
    /// <summary>
    /// 线边仓盘点分组数据
    /// </summary>
    public class StockCountOnHandDatas
    {
        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 工厂ID
        /// </summary>
        public double? FactoryId { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal NgQty { get; set; }

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double? StorageLocationId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 批次ID
        /// </summary>
        public double? LotId { get; set; }
    }

    /// <summary>
    /// 完工回传数据
    /// </summary>
    [Serializable]
    public class FinishededStockCountData
    {
        /// <summary>
        /// 明细Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 盘点结果
        /// </summary>
        public LesStockCountDetailResult? StockCountDetailResult { get; set; }

        /// <summary>
        /// 差异数量
        /// </summary>
        public decimal? DiffCountQty { get; set; }

        /// <summary>
        /// 批次Id
        /// </summary>
        public double? LotId { get; set; }

        /// <summary>
        /// 序列号有异常
        /// </summary>
        public bool SnAbnormity { get; set; }

        /// <summary>
        /// 是否显示差异调账
        /// </summary>
        public bool IsDiffAjust { get; set; }
    }
}
