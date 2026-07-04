using SIE.Equipments.Enums;
using System;

namespace SIE.EMS.API.APIModels
{
    /// <summary>
    /// 设备的运行状态信息
    /// </summary>
    [Serializable]
    public class EquipmentRunningStateInfo
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public double? Id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备名称名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        public EquipRunningState? State { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public string Time { get; set; }
    }
}