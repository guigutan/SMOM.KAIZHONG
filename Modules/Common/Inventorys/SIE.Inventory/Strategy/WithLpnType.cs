using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 带LPN库存
    /// </summary>
    public enum WithLpnType
    {
        /// <summary>
        /// 强制
        /// </summary>
        [Label("强制")]
        Force = 10,

        ///// <summary>
        ///// 优先
        ///// </summary>
        //[Label("优先")]
        //First = 20,

        ///// <summary>
        ///// 靠后
        ///// </summary>
        //[Label("靠后")]
        //Next = 30,

        /// <summary>
        /// 忽略
        /// </summary>
        [Label("忽略")]
        Ignore = 40,
    }
}