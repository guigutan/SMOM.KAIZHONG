using SIE.ObjectModel;

namespace SIE.Fixtures.FixtureRecords
{
    /// <summary>
	/// 类型
	/// </summary>
	public enum RecordType
    {
        /// <summary>
        /// 入库
        /// </summary>
        [Label("入库")]
        In = 5,

        /// <summary>
        /// 出库
        /// </summary>
        [Label("出库")]
        Out = 10,
    }
}
