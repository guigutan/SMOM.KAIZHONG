using SIE.ObjectModel;

namespace SIE.Core.Configs
{
    /// <summary>
    /// 看板数据源类型
    /// </summary>
    public enum DashboardDataSourceType
    {
        /// <summary>
        /// 演示数据
        /// </summary>
        [Label("演示数据")]
        FromDemo = 5,
        /// <summary>
        /// 生产数据
        /// </summary>
        [Label("生产数据")]
        FromProdution = 10,

    }
}
