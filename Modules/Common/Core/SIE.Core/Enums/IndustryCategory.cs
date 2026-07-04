using SIE.ObjectModel;

namespace SIE.Core.Enums
{
    /// <summary>
    /// 行业属性
    /// </summary>
    public enum IndustryCategory
    {
        /// <summary>
        /// 通用设备
        /// </summary>
        [Label("通用设备")]
        GeneralEquipment,

        /// <summary>
        /// 电子行业
        /// </summary>
        [Label("电子行业")]
        ElecInitIndustry,

        /// <summary>
        /// PCB行业
        /// </summary>
        [Label("PCB行业")]
        PcbInitIndustry,

        /// <summary>
        /// 物流设备
        /// </summary>
        [Label("物流设备")]
        LogisticsEquipment
    }
}
