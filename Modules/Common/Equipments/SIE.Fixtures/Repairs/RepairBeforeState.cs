using SIE.ObjectModel;

namespace SIE.Fixtures.Repairs
{
    /// <summary>
    /// 报修前状态
    /// </summary>
    public enum RepairBeforeState
    {
        /// <summary>
        /// 在线
        /// </summary>
        [Label("在线")]
        Online = 5,

        /// <summary>
        /// 在库
        /// </summary>
        [Label("在库")]
        InStock = 10,
    }
}
