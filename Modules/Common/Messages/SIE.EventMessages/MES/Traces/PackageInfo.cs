using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Traces
{
    /// <summary>
    /// Wms包装信息
    /// </summary>
    [Serializable]
    public class PackageInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// TreePId
        /// </summary>
        public string TreePId { get; set; }

        /// <summary>
        /// 包装号
        /// </summary>
        public string PackageNo { get; set; }

        /// <summary>
        /// 包装单位
        /// </summary>
        public string PackageUnitName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 包装时间
        /// </summary>
        public DateTime? PackageTime { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 发运单
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

        /// <summary>
        /// 入库条码
        /// </summary>
        public string StorageBarcode { get; set; }
    }
}
