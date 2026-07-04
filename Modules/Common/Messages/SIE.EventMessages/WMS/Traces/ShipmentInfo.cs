using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.WMS.Traces
{
    /// <summary>
    /// Wms出货信息
    /// </summary>
    [Serializable]
    public class ShipmentInfo
    {
        /// <summary>
        /// 发运单号
        /// </summary>
        public string ShippingOrderNo { get; set; }
    }
}
