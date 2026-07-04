using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Traces
{
    /// <summary>
    /// Wms包装信息查询实体
    /// </summary>
    [Serializable]
    public class PackageInfoCriteria
    {
        /// <summary>
        /// Sn
        /// </summary>
        public string ProductSn { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>

        public double ProductId { get; set; }
    }
}
