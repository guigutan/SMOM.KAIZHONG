using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// 物料标签
    /// </summary>
    [Serializable]
    public class XPItemLabel
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 单位Id
        /// </summary>
        public double UnitId { get; set; }

        /// <summary>
        /// 来源工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 包装关系Id
        /// </summary>
        public double RelationId { get; set; }

        /// 原始标签
        /// </summary>
        public string OriginalLabel { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

    }
}
