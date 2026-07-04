using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.WorkOrders.Models
{
    /// <summary>
    /// 工单信息（供LES备料计算使用）
    /// </summary>
    [Serializable]
    public class WoBomInfoForLes
    {
        /// <summary>
        /// ID
        /// </summary>
        public double Id { get; set; }

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
        /// 总需求量
        /// </summary>
        public decimal RequestQty { get; set; }

        /// <summary>
        /// 单机定额
        /// </summary>
        public decimal SingleQty { get; set; }

        /// <summary>
        /// 现有数量
        /// </summary>
        public decimal OnhandQty { get; set; }

        /// <summary>
        /// 物料消耗类型(0-拉式物料;1-推式物料;2-储备物料)
        /// </summary>
        public int ConsumeMode { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName { get; set; }
    }
}
