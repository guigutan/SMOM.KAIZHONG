using SIE.Domain;
using SIE.EventMessages.Common.Traces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.QMS.Traces
{
    /// <summary>
    /// 产品检验追溯信息查询实体
    /// </summary>
    [Serializable]
    public class TraceInfoForProductQmsCriteria
    {
        /// <summary>
        /// 首检报检单Id
        /// </summary>
        public double? FirstInspLogId { get; set; }

        /// <summary>
        /// 成品报检单Id
        /// </summary>
        public double? ShippingInspLogId { get; set; }      

        /// <summary>
        /// 产品id
        /// </summary>
        public double ProductId { get; set;}

        /// <summary>
        /// 工单Id
        /// </summary>

        public double WorkOrderId { get; set; }

        /// <summary>
        /// Wms发运单号
        /// </summary>
        public string ShipmentNo { get; set; }
    }
}
