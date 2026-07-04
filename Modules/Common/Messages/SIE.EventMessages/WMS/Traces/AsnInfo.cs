using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.WMS.Traces
{
    /// <summary>
    /// Asn单信息
    /// </summary>
    [Serializable]
    public class AsnInfo
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<AsnInfoData> Data { get; set; } = new List<AsnInfoData>();

    }
    /// <summary>
    /// Asn单详细信息
    /// </summary>
    [Serializable]
    public class AsnInfoData
    {
        /// <summary>
        /// asn明细Id（主键）
        /// </summary>
        public double AsnDetailId { get; set; }

        /// <summary>
        /// Asn单号
        /// </summary>
        public string AsnNo { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 生产批次
        /// </summary>
        public string ProductionLot { get; set; }
        /// <summary>
        /// 物料批次
        /// </summary>
        public string ItemLot { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate { get; set; }
        /// <summary>
        /// 收货日期
        /// </summary>
        public DateTime? CollectDate { get; set; }

    }

}
