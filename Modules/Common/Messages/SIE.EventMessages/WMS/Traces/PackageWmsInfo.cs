using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.WMS.Traces
{
    /// <summary>
    /// 包装信息的Wms信息
    /// </summary>
    [Serializable]
    public class PackageWmsInfo
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 发运单号
        /// </summary>
        public string ShippingOrderNo { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>

        public string DeliveryDate { get; set; }

    }
}
