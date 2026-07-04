using System;

namespace SIE.MES.TaskManagement.Models
{
    /// <summary>
    /// 关联设备信息
    /// </summary>
    [Serializable]
    public class DispatchEquipInfo
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public double EquipmentId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }
    }
}