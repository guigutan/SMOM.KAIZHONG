using SIE.ObjectModel;

namespace SIE.Items
{
    /// <summary>
    /// 来源类型
    /// </summary>
    public enum ItemSourceType
    {
        /// <summary>
        /// 外购
        /// </summary>
        [Label("外购")]
        Outsourcing,

        /// <summary>
        /// 自制
        /// </summary>
        [Label("自制")]
        SelfMade,

        /// <summary>
        /// 外协
        /// </summary>
        [Label("外协")]
        OutMade,
    }
}