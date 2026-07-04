using System;

namespace SIE.EventMessages.MES.Inspection.Models
{
    /// <summary>
    /// 报检条码查询信息
    /// </summary>
    [Serializable]
    public class BarcodeQueryInfo
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 报检类型 0成品 1首件 2抽检
        /// </summary>
        public int InspType { get; set; }
    }
}