using System;

namespace SIE.EMS.SpareParts.ApiModels
{
    /// <summary>
    /// 设备备件仓库信息
    /// </summary>
    [Serializable]
    public class SparePartWarehouseInfo
    {
        /// <summary>
        /// 备件ID
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 库存数
        /// </summary>
        public int StoreQty { get; set; }
    }
}
