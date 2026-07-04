using System;
using System.Collections.Generic;

namespace SIE.TurnoverTools.TurnoverTools.Models
{
    /// <summary>
    /// 载具条码信息
    /// </summary>
    [Serializable]
    public class ScanTurnoverInfo
    {
        /// <summary>
        /// 载具号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 绑定条码明细
        /// </summary>
        public List<ToolBindingInfo> ToolBindings { get; } = new List<ToolBindingInfo>();
    }

    /// <summary>
    /// 绑定条码明细
    /// </summary>
    [Serializable]
    public class ToolBindingInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 已入库数量
        /// </summary>
        public decimal InStorageQty { get; set; }
    }

    /// <summary>
    /// 产品条码信息
    /// </summary>
    [Serializable]
    public class ScanSnInfo
    {
        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }
    }
}
