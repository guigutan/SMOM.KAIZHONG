using SIE.ObjectModel;

namespace SIE.Defects.InspectionItems
{
    /// <summary>
    /// 检验标识
    /// </summary>
    public enum CheckTag
    {
        /// <summary>
        /// 定量
        /// </summary>
        [Label("定量")]
        Quantitative,

        /// <summary>
        /// 定性
        /// </summary>
        [Label("定性")]
        Qualitative,
    }
}