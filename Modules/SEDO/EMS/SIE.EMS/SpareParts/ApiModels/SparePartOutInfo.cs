using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.ApiModels
{
    /// <summary>
    /// 备件出库单明细信息
    /// </summary>
    [Serializable]
    public class SparePartOutInfo : SparePartInfo
    {
        /// <summary>
        /// 申请单号
        /// </summary>
        public string OutDepotNo { get; set; }

        /// <summary>
        /// 出库单行号
        /// </summary>
        public int LineNo { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName { get; set; }

        /// <summary>
        /// 设备台账ID
        /// </summary>
        public double? EquipId { get; set; }

        /// <summary>
        /// 出库单设备型号
        /// </summary>
        public double? OutEquipModelId { get; set; }

        /// <summary>
        /// 备件出库单明细ID
        /// </summary>
        public double OutDtlId { get; set; }

        /// <summary>
        /// 出库仓库ID
        /// </summary>
        public double? OutStockWarehouseId { get; set; }

        /// <summary>
        /// 出库仓库编码
        /// </summary>
        public string OutStockWarehouseCode { get; set; }

        /// <summary>
        /// 出库仓库名称
        /// </summary>
        public string OutStockWarehouseName { get; set; }

        /// <summary>
        /// 备件出库单状态
        /// 0:(Ing, 待出库)
        ///1:(Ed, 已出库)
        /// </summary>
        public int OutDepotState { get; set; }

        /// <summary>
        /// 备件出库单状态名称
        /// </summary>
        public string OutDepotStateName { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public string SeriaNo { get; set; }

        /// <summary>
        /// 序列号Id
        /// </summary>
        public double? SeriaNoId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 批次号Id
        /// </summary>
        public double? BatchNoId { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo { get; set; }
    }
}
