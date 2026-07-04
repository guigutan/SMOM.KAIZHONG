using SIE.ObjectModel;

namespace SIE.Fixtures.Fixtures.Accounts
{
    /// <summary>
    /// 使用履历类型
    /// </summary>
    public enum UseResumeType
    {
        /// <summary>
        /// 出库
        /// </summary>
        [Label("出库")]
        Unload = 5,

        /// <summary>
        /// 领用
        /// </summary>
        [Label("领用")]
        Receive = 10,

        /// <summary>
        /// 归还
        /// </summary>
        [Label("归还")]
        Return = 15,

        /// <summary>
        /// 上架
        /// </summary>
        [Label("入库")]
        Shelf = 20,

        /// <summary>
        /// 上线/下线
        /// </summary>
        [Label("上线/下线")]
        OnOffline = 30,
    }
}
