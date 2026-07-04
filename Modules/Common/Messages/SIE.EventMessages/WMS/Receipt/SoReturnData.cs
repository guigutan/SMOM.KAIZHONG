using System;
using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// 供应商退货发运单数据
    /// </summary>
    [Serializable]
    public class SoReturnData
    {
        /// <summary>
        /// ASN单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 采购订单明细
        /// </summary>
        public double? PoDtlId { get; set; }

        /// <summary>
        /// 订货数
        /// </summary>
        public decimal ExceptQty { get; set; }

        /// <summary>
        /// 实发数
        /// </summary>
        public decimal ActualQty { get; set; }

        /// <summary>
        /// 发货需求
        /// </summary>
        public List<SoReturnRequireData> Requires { get; set; } = new List<SoReturnRequireData>();
    }

    public class SoReturnRequireData
    {
        /// <summary>
        /// 采购订单明细
        /// </summary>
        public double PoDtlId { get; set; }


        /// <summary>
        /// 需求数
        /// </summary>
        public decimal RequireQty { get; set; }

        /// <summary>
        ///  送货明细Id
        /// </summary>
        public double? DeliveryId { get; set; }

    }
}
