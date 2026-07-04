using SIE.ObjectModel;

namespace SIE.Fixtures.FixtureDemands
{
    /// <summary>
	/// 领用状态
	/// </summary>
	public enum ReceiveState
    {
        /// <summary>
        /// 未领用
        /// </summary>
        [Label("未领用")]
        None = 5,

        /// <summary>
        /// 部分领用
        /// </summary>
        [Label("部分领用")]
        Part = 10,

        /// <summary>
        /// 领用完成
        /// </summary>
        [Label("领用完成")]
        Finish = 15,
    }
}
