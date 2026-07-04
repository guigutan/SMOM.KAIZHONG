using SIE.ObjectModel;

namespace SIE.Inventory.Commom
{
    /// <summary>
    /// 调拨模式
    /// </summary>
    public enum AllotModel
    {
        /// <summary>
        /// 直接调拨
        /// </summary>
        [Label("直接调拨")]
        DIRECTALLOT,

        /// <summary>
        /// 两步调拨
        /// </summary>
        [Label("两步调拨")]
        TWOALLOT,

        /// <summary>
        /// 跨组织调拨
        /// </summary>
        [Label("跨组织调拨")]
        ACROSS,
    }
}
