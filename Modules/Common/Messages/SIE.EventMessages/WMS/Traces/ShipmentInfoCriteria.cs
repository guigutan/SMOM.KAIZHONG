using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.WMS.Traces
{
    /// <summary>
    /// Wms出货信息查询实体
    /// </summary>
    [Serializable]
    public class ShipmentInfoCriteria
    {
        /// <summary>
        /// Sn
        /// </summary>
        public string ProductSn { get; set; }
    }
}
