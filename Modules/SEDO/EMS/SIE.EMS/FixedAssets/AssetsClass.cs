using SIE.ObjectModel;

namespace SIE.EMS.FixedAssets
{
    /// <summary>
    /// 类型
    /// </summary>
    public enum AssetsClass
    {
        /// <summary>
        /// 资本化
        /// </summary>
        [Label("资本化")]
        Capitalization = 5,
        /// <summary>
        /// 备件
        /// </summary>
        [Label("在建工程")]
        Construction = 10,
    }
}
