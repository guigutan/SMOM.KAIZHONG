using System;

namespace SIE.EventMessages.APS.SaleOrderEvents
{
    /// <summary>
    /// 销售订单数据传递类
    /// </summary>
    public class SaleOrderFlatObject
    {
        /// <summary>
        /// 销售订单编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 销售订单明细行号
        /// </summary>
        public string LineNo { get; set; }
        /// <summary>
        /// 销售订单明细数量
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// 客户交期
        /// </summary>
        public DateTime RequireDelivery { get; set; }
        /// <summary>
        /// 删除or修改
        /// </summary>
        public FlatType Identification { get; set; }
    }

    /// <summary>
    /// 销售订单明细操作类型
    /// </summary>
    public enum FlatType
    {
        /// <summary>
        /// 修改
        /// </summary>
        Alter,
        /// <summary>
        /// 删除
        /// </summary>
        Delete
    }
}
