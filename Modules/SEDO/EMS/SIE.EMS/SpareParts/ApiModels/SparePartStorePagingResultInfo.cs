using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.ApiModels
{
    /// <summary>
    /// 设备备件库存数据
    /// </summary>
    [Serializable]
    public class SparePartStorePagingResultInfo : PagingResultInfo
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
        /// 单位
        /// </summary>
        public string SparePartUnit { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SparePartSpecification { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string SparePartTypeName { get; set; }

        /// <summary>
        /// 关联设备型号编码
        /// </summary>
        public string EquipModelCode { get; set; }

        /// <summary>
        /// 关联设备型号名称
        /// </summary>
        public string EquipModelName { get; set; }

        /// <summary>
        /// 生产厂商
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 库存总数
        /// </summary>
        public int StoreTotalQty { get; set; }

        /// <summary>
        /// 设备仓库库存信息
        /// </summary>
        public List<SparePartWarehouseInfo> SparePartWarehouseInfos { get; set; } = new List<SparePartWarehouseInfo>();
    }
}