using SIE.ObjectModel;

namespace SIE.Core.Enums
{
    /// <summary>
    /// 轨道类型
    /// </summary>
    public enum OrbitType
    {
        /// <summary>
        /// 单轨
        /// </summary>
        [Label("单轨")]
        Monorail = 5,

        /// <summary>
        /// 双轨
        /// </summary>
        [Label("双轨")]
        DoubleRail = 10
    }
}