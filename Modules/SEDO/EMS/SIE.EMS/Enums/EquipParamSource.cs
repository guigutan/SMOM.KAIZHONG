using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 设备参数来源
    /// </summary>
    public enum EquipParamSource
    {
        /// <summary>
        /// 手动取值
        /// </summary>
        [Label("手动取值")]
        ManualValue,

        /// <summary>
        /// 自动取值
        /// </summary>
        [Label("自动取值")]
        AutomaticValue,

        /// <summary>
        /// 否
        /// </summary>
        [Label("否")]
        NoValue
    }
}
