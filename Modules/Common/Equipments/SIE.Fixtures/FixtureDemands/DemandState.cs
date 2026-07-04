using SIE.ObjectModel;

namespace SIE.Fixtures.FixtureDemands
{
    /// <summary>
	/// 出库状态
	/// </summary>
	public enum DemandState
    {
        /// <summary>
        /// 未出库
        /// </summary>
        [Label("未出库")]
        None = 5,

        /// <summary>
        /// 部分出库
        /// </summary>
        [Label("部分出库")]
        Part = 10,

        /// <summary>
        /// 出库完成
        /// </summary>
        [Label("出库完成")]
        Finish = 15,
    }
}
