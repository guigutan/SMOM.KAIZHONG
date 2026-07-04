using System;

namespace SIE.EventMessages.MES.WIP.Models
{
    /// <summary>
    /// 工单在制信息
    /// </summary>
    [Serializable]
    public class WorkOrderWipInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ItemId { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public double FactoryId { get; set; }
        /// <summary>
        /// 车间
        /// </summary>
        public double WorkShopId { get; set; }
        /// <summary>
        /// 产线
        /// </summary>
        public double LineId { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

    }
}
