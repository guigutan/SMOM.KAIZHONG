using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘采集物料数据
    /// </summary>
    [Serializable]
    public class EdgeMaterial
    {

        /// <summary>
        /// 物料ID
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 条码来源类型
        /// </summary>
        public SingleLabels.LoadItemSourceType? SourceType { get; set; }

        /// <summary>
        /// 条码来源Id
        /// </summary>
        public string SourceId { get; set; }

    }
}
