using SIE.ObjectModel;

namespace SIE.Fixtures.Fixtures.Accounts
{
    /// <summary>
	/// 质量状态
	/// </summary>
	public enum FixtureQualityState
    {
        /// <summary>
        /// 合格
        /// </summary>
        [Label("合格")]
        Pass = 5,

        /// <summary>
        /// 不合格
        /// </summary>
        [Label("不合格")]
        Ng = 10,
    }
}
