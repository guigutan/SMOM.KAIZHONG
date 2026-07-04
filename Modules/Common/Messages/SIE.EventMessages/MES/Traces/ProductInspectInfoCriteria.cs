using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Traces
{
    /// <summary>
    /// 产品报检信息查询实体
    /// </summary>
    [Serializable]
    public class ProductInspectInfoCriteria
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>

        public string ProductSn { get; set; }
    }
}
