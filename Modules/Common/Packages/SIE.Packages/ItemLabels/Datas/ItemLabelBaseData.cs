using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Packages.ItemLabels.Datas
{
    /// <summary>
    /// 物料标签基本数据
    /// </summary>
    [Serializable]
    public class ItemLabelBaseData
    {
        /// <summary>
        /// 物料标签id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 上级包装
        /// </summary>
        public string PackageNo { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性(展示)
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 库位id
        /// </summary>
        public double? StorageLocationId { get; set; }

        /// <summary>
        /// 是否序列号管控
        /// </summary>
        public bool? IsSerialNumber { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
