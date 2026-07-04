using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.WMS.Traces
{
    /// <summary>
    /// 包装信息的Wms信息查询实体
    /// </summary>
    [Serializable]
    public class PackageWmsInfoCriteria
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// Sn
        /// </summary>
        public string ProductSn { get; set; }

        /// <summary>
        /// 入库条码
        /// </summary>
        public string StorageBarcode { get; set; }
    }
}
