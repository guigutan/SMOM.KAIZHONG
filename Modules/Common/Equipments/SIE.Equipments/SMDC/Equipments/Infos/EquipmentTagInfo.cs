using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SMDC.Equipments.Infos
{
    /// <summary>
    /// 设备Tag
    /// </summary>
    [Serializable]
    public class EquipmentTagInfo
    {
        /// <summary>
        /// 设备Tag
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public String MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public String MinValue { get; set; }
    }

    /// <summary>
    /// 设备Tag全路径
    /// </summary>
    [Serializable]
    public class EquipmentTagFullNameInfo
    {
        /// <summary>
        /// 设备Tag全路径
        /// </summary>
        public String FullName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue { get; set; }
    }

}
