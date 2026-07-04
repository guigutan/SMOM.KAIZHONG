using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// 点检计划备件更换数据
    /// </summary>
    [Serializable]
    public class CheckSparePartInfo
    {
        /// <summary>
        /// 备件ID
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string SparePartCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string SparePartName { get; set; }

        /// <summary>
        /// 可用数
        /// </summary>
        public int UseQty { get; set; }

        /// <summary>
        /// 备件申请单号
        /// </summary>
        public string OutDepotNo { get; set; }

        /// <summary>
        /// 备件出库单状态
        /// </summary>
        public int? OutDepotState { get; set; }

        /// <summary>
        /// 备件出库单状态名称
        /// </summary>
        public string OutDepotStateName { get; set; }

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
        /// 备件出库单明细ID
        /// </summary>
        public double? OutDtlId { get; set; }

        /// <summary>
        /// 更换数量
        /// </summary>
        public int ChangeQty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 备件更换项目ID
        /// </summary>
        public double CheckSparePartId { get; set; }

        /// <summary>
        /// 备件行状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 备件行状态名称
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public string SeriaNo { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 剩余数
        /// </summary>
        public int RemainingQty { get; set; }
    }
}
