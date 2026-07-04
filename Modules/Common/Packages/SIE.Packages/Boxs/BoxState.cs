using SIE.ObjectModel;

namespace SIE.Packages.Boxs
{
    /// <summary>
    /// 周转箱状态
    /// </summary>
    public enum BoxState
    {
        /// <summary>
        /// 使用中
        /// </summary>
        [Label("使用中")]
        Inuse,

        /// <summary>
        /// 闲置
        /// </summary>
        [Label("闲置")]
        Unused,

        /// <summary>
        /// 维修中
        /// </summary>
        [Label("维修中")]
        Repairing,

        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        Scrap
    }
}