using SIE.EventMessages.Common.Traces;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.WMS.Traces
{
    /// <summary>
    ///  库存物料
    /// </summary>
    [Serializable]
    public class WmsItemInfo
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<WmsItemData> Data { get; set; } = new List<WmsItemData>();
    }

    /// <summary>
    /// 库存物料详细信息
    /// </summary>
    [Serializable]
    public class WmsItemData
    {
        /// <summary>
        /// 追溯类型
        /// </summary>
        public TraceType TraceType { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// SN
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string ItemLot { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 供应商Id(物料追溯时用到，序列号、批次追溯无需使用到)
        /// </summary>
        public double? SupplierId { get; set; }



    }
}
