using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.SpareParts.ApiModels
{
    /// <summary>
    /// 备件BOM信息
    /// </summary>
    [Serializable]
    public class SpareBomInfo
    {
        /// <summary>
        /// 备件Id
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName { get; set; }

        /// <summary>
        /// 备件单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 型号规格
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName { get; set; }

        /// <summary>
        /// 备件型号编码
        /// </summary>
        public string SpEquipModelCode { get; set; }

        /// <summary>
        /// 备件型号名称
        /// </summary>
        public string SpEquipModelName { get; set; }

        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer { get; set; }
    }

    /// <summary>
    /// 备件库存
    /// </summary>
    [Serializable]
    public class SpareStoryInfo
    {
        /// <summary>
        /// 备件Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Value { get; set; }
    }

    /// <summary>
    /// 备件申请单信息
    /// </summary>
    [Serializable]
    public class SpareOutInfo
    {
        /// <summary>
        /// 备件Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 使用数量
        /// </summary>
        public int UseCount { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public int OutDepotCount { get; set; }
    }
}
