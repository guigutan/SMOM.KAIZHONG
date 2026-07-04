using System;

namespace SIE.EventMessages.MES.LoadItems.Models
{
    /// <summary>
    /// 工单耗用信息
    /// </summary>
    [Serializable]
    public class WoCostItemInfo
    {

        /// <summary>
        /// 耗用单号
        /// </summary>
        public string CostNo { get; set; }
        /// <summary>
        /// 状态 (10=待提交,20=已提交,30=提交失败,40=关闭)
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 物料Id
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
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }
        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

    }
}
