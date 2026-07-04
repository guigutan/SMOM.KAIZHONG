using SIE.XPCJ.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// 待包装SN扫描记录
    /// </summary>
    [Serializable]
    public class XPPackageSnRecord
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 包装单位ID
        /// </summary>
        public double PackageUnitId { get; set; }

        /// <summary>
        /// 工单条码号
        /// </summary>
        public string WoSn { get; set; }

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 已加入包装数
        /// </summary>
        public decimal PackedQty { get; set; }

        /// <summary>
        /// 物料数量
        /// </summary>
        public decimal ItemQty { get; set; }

        /// <summary>
        /// 包装单位名称
        /// </summary>
        public string PackageUnitName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 打包明细记录
        /// </summary>
        public List<XPPackageSnRecord> ListChild { get; set; } = new List<XPPackageSnRecord>();
    }
}
