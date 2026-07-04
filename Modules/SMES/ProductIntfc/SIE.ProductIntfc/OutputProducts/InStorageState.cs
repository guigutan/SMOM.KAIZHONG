using SIE.ObjectModel;

namespace SIE.ProductIntfc.OutputProducts
{
    /// <summary>
    /// 入库状态
    /// </summary>
    public enum InStorageState
    {
        /// <summary>
        /// 创建
        /// </summary>
        [Label("创建")]
        Created = 10,

        /// <summary>
        /// 入库
        /// </summary>
        [Label("入库")]
        InStorage = 20,

        /// <summary>
        /// 出库
        /// </summary>
        [Label("出库")]
        OutStorage = 30,

        /// <summary>
        /// 分配
        /// </summary>
        [Label("分配")]
        Assign = 40,

        /// <summary>
        /// 待退
        /// </summary>
        [Label("待退")]
        Returned = 50,
    }
}
