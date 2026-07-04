using System;

namespace SIE.Equipments.ViewModels
{
    /// <summary>
    /// 设备类型信息
    /// </summary>
    [Serializable]
    public class EquipTypeInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string TypeName { get; set; }
    }
}