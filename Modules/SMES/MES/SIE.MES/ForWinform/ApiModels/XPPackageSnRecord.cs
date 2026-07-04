using SIE.MES.WIP.NewPackages;
using SIE.MES.WIP.Packings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ForWinform.ApiModels
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
        /// 
        /// </summary>
        /// <param name="packageSnRecord"></param>
        public XPPackageSnRecord(WIP.NewPackages.PackageSnRecord packageSnRecord)
        {
            this.Id = packageSnRecord.Id;
            this.Sn = packageSnRecord.Sn;
            this.WorkOrderId = packageSnRecord.WorkOrderId;
            this.ProductId = packageSnRecord.ProductId;
            this.PackageUnitId = packageSnRecord.PackageUnitId;
            this.WoSn = packageSnRecord.WoSn;
            this.ResourceId = packageSnRecord.ResourceId;
            this.ProcessId = packageSnRecord.ProcessId;
            this.StationId = packageSnRecord.StationId;
            this.PackedQty = packageSnRecord.PackedQty;
            this.ItemQty = packageSnRecord.ItemQty;
            this.PackageUnitName = packageSnRecord.PackageUnitName;
            this.ProductCode = packageSnRecord.ProductCode;
            this.ProductName = packageSnRecord.ProductName;
            this.WoNo = packageSnRecord.WoNo;
        }
        /// <summary>
        /// 空构造
        /// </summary>
        public XPPackageSnRecord() { }

        /// <summary>
        /// 获取直接包装条码记录
        /// </summary>
        /// <param name="packageSnRecord"></param>
        /// <returns></returns>
        public static XPPackageSnRecord GetDirectPackageSnRecord(DirectPackageSnRecord packageSnRecord)
        {
            return new XPPackageSnRecord()
            {

                Id = packageSnRecord.Id,
                Sn = packageSnRecord.Sn,
                WorkOrderId = packageSnRecord.WorkOrderId,
                ProductId = packageSnRecord.ProductId,
                PackageUnitId = packageSnRecord.PackageUnitId,
                WoSn = packageSnRecord.WoSn,
                ResourceId = packageSnRecord.ResourceId,
                ProcessId = packageSnRecord.ProcessId,
                StationId = packageSnRecord.StationId,
                PackedQty = packageSnRecord.PackedQty,
                ItemQty = packageSnRecord.ItemQty,
                PackageUnitName = packageSnRecord.PackageUnitName,
                ProductCode = packageSnRecord.ProductCode,
                ProductName = packageSnRecord.ProductName,
                WoNo = packageSnRecord.WoNo
            };
        }
    }
}
