using SIE.ObjectModel;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
	/// 规则类型
	/// </summary>
	public enum RuleType
    {
        /// <summary>
        /// 直线
        /// </summary>
        [Label("直线")]
        Line,

        /// <summary>
        /// 曲线
        /// </summary>
        [Label("曲线")]
        Curve,
    }
}
