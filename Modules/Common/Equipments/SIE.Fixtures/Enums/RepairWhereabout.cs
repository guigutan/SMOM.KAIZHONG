using SIE.ObjectModel;

namespace SIE.Fixtures.Enums
{
    /// <summary>
    /// 维修去向
    /// </summary>
    public enum RepairWhereabout
    {
        /// <summary>
        /// 入库
        /// </summary>
        [Label("入库")]
        In = 10,
        /// <summary>
        /// 继续使用
        /// </summary>
        [Label("继续使用")]
        Use = 20,
    }
}
