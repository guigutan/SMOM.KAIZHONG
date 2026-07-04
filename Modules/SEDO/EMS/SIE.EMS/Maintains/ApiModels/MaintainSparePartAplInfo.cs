using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 保养计划备件申请数据
    /// </summary>
    [Serializable]
    public class MaintainSparePartAplInfo
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
        /// 库存数
        /// </summary>
        public int StoreQty { get; set; }

        /// <summary>
        /// 可用数
        /// </summary>
        public int UseQty { get; set; }

        /// <summary>
        /// 备件申请单号
        /// </summary>
        public string SparePartApplyNo { get; set; }

        /// <summary>
        /// 备件申请单状态
        /// </summary>
        public int? SparePartApplyState { get; set; }

        /// <summary>
        /// 备件申请单状态名称
        /// </summary>
        public string ApplyStateName { get; set; }

        /// <summary>
        /// 申请数量
        /// </summary>
        public int ApplyQty { get; set; }

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
        /// 备件申请单明细ID
        /// </summary>
        public double? AppDtlId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 备件更换项目ID
        /// </summary>
        public double MaintainSparePartId { get; set; }

        /// <summary>
        /// 是否已申请
        /// </summary>
        public bool IsApply { get; set; }
    }
}
