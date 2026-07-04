using System;

namespace SIE.EMS.Equipments.ApiModels
{
    /// <summary>
    /// SCADA设备信息
    /// </summary>
    [Serializable]
    public class ScadaEquipInfo
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备型号ID
        /// </summary>
        public double EquipModelId { get; set; }

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrgId { get; set; }
    }
}