using SIE.ObjectModel;

namespace SIE.Inventory.Commom
{
    /// <summary>
    /// 共享类型
    /// </summary>
    public enum ShareType
    {
        /// <summary>
        /// 仅有我
        /// </summary>
        [Label("仅有我")]
        Only = 0,

        /// <summary>
        /// 所有人
        /// </summary>
        [Label("所有人")]
        All = 10,
    }
}