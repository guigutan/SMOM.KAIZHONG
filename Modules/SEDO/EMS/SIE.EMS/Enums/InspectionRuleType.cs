using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 项目类型
    /// </summary>
    public enum InspectionRuleType
    {
        /// <summary>
        /// 设备定检
        /// </summary>
        [Label("设备定检")]
        Regular = 10,
        /// <summary>
        /// 计量检验
        /// </summary>
        [Label("计量检验")]
        Metrology = 20,
    }
}