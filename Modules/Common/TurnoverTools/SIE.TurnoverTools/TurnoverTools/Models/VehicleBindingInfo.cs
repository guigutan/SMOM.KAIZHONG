using System;

namespace SIE.TurnoverTools.TurnoverTools.Models
{
    /// <summary>
    /// 周转工具绑定-产品数据
    /// </summary>
    [Serializable]
    public class VehicleBindingInfo
    {
        /// <summary>
        /// 载具条码
        /// </summary>
        public string VehicleNo { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 载具数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 扫描时间
        /// </summary>
        public DateTime? ScanDate { get; set; }
    }
}
