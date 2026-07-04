using System;

namespace SIE.Equipments.ViewModels
{
    /// <summary>
    /// 设备位置信息
    /// </summary>
    [Serializable]
    public class EquipLocationInfo
    {
        /// <summary>
        /// 分区
        /// </summary>
        public string Subarea { get; set; }

        /// <summary>
        /// 站位
        /// </summary>
        public string Stance { get; set; }
    }
}