using System;

namespace SIE.Equipments.ViewModels
{
    /// <summary>
    /// 设备型号信息
    /// </summary>
    [Serializable]
    public class EquipModelInfo
    {
        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 设备类型编号
        /// </summary>
        public string TypeCode { get; set; }
    }
}