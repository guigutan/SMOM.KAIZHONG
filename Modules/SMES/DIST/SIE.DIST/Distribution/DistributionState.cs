using SIE.ObjectModel;

namespace SIE.DIST
{
    /// <summary>
    /// 载具配送状态
    /// </summary>
    public enum DistributionState
    {
        /// <summary>
        /// 已上料
        /// </summary>
        [Label("已上料")]
        LoadItem = 1,

        /// <summary>
        /// 配送中
        /// </summary>
        [Label("配送中")]
        DistributionIn = 2,

        /// <summary>
        /// 部分上料
        /// </summary>
        [Label("部分上料")]
        LoadPartItem = 4
    }
}