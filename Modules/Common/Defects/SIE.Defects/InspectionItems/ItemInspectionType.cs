using SIE.ObjectModel;

namespace SIE.Defects.InspectionItems
{
    /// <summary>
    /// 检验类型
    /// </summary>
    public enum ItemInspectionType
    {
        /// <summary>
        /// 常规检
        /// </summary>
        [Label("常规检")]
        RoutineTest,

        /// <summary>
        /// 型式试验
        /// </summary>
        [Label("型式试验")]
        TypeTest,
    }
}