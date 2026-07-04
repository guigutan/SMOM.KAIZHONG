using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.ApiModels
{
    /// <summary>
    /// 设备备件信息
    /// </summary>
    [Serializable]
    public class SparePartInfo
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
        /// 库存数
        /// </summary>
        public int StoreQty { get; set; }

        /// <summary>
        /// 剩余数
        /// </summary>
        public int RemainingQty { get; set; }

        /// <summary>
        /// Base64图片
        /// </summary>
        public string PhotoBase64 { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double EquipModelId { get; set; }

        /// <summary>
        /// 是否备件BOM 0-不是 1-是
        /// </summary>
        public int IsBom { get; set; }
    }
}
